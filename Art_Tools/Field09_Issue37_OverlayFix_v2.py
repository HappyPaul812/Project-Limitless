#!/usr/bin/env python3
"""LIMITLESS / Field09 Issue #37: isolated Cracked Overlay v2 repair.

Input: LL_Field09_Redesign_Ground_Cracked_Overlay_v1.png (512x512 RGBA)
Output: ONLY overlay v2 PNG (512x512 RGBA). NO Base, Slab or Unity modifications.
Usage: python rebuild_overlay_v2.py original_overlay_v1.png overlay_v2.png
For v1 PNG released in Sample3 v1 ZIP; the input SHA256 is enforced.
"""
import sys
import hashlib
from pathlib import Path
import numpy as np
from PIL import Image

OLD_SHA = '56d8e0ea9bf3ca2167227362b87bb219a21d4470f317456f36eb8aba99caedeb'
# This repeated 512px sprite is 4 Unity world-units at PPU128. The 9WU overlay
# is clipped at 2.25 repeats; the right limit x=-1.5 maps to sprite x=128.
# Crack intersections there occur near y=(43,273,362) in the current master.
RIGHT_LIMIT_SOURCE_X = 128
CONTACTS = [(43, 30, 22), (273, 30, 34), (362, 27, 30)]


def repair(source):
    source = np.asarray(source, dtype=np.uint8)
    if source.shape != (512, 512, 4):
        raise ValueError(f'Unexpected RGBA shape: {source.shape}')
    yy, xx = np.mgrid[0:512, 0:512]
    fade = np.zeros((512,512),dtype=np.float32)
    for yc, horizontal_span, vertical_span in CONTACTS:
        # Serpentine endcaps around each of the three actual crack intersections.
        # Deliberately NOT a generic edge blur, full-width alpha fade or 1px erase.
        # Controls allow each individual lava vein to dissipate BEFORE the world clip.
        drift = 1.8*np.sin((yy-yc)*0.19)+1.1*np.cos((yy-yc)*0.09)
        dx = np.abs(xx - RIGHT_LIMIT_SOURCE_X + drift)
        horizontal = np.clip((horizontal_span-dx)/horizontal_span,0,1)**1.55
        vertical = np.clip((vertical_span-np.abs(yy-yc))/vertical_span,0,1)**0.45
        fade = np.maximum(fade, horizontal*vertical)
    result = source.copy()
    result[:,:,3] = np.clip(np.round(source[:,:,3].astype(np.float64)*(1-0.998*fade)),0,255).astype('uint8')
    # All original artwork RGB remains EXACTLY untouched; only alpha changes.
    assert np.array_equal(result[:,:,:3], source[:,:,:3])
    assert np.array_equal(result[:,0,:],source[:,0,:]) and np.array_equal(result[:,-1,:],source[:,-1,:])
    assert np.array_equal(result[0,:,:],source[0,:,:]) and np.array_equal(result[-1,:,:],source[-1,:,:])
    return result


def main():
    if len(sys.argv)!=3:
        raise SystemExit(__doc__)
    p=Path(sys.argv[1]);out=Path(sys.argv[2]);digest=hashlib.sha256(p.read_bytes()).hexdigest()
    if digest!=OLD_SHA: raise ValueError(f'Original overlay SHA mismatch: {digest} != {OLD_SHA}')
    arr=np.asarray(Image.open(p).convert('RGBA'))
    edited=repair(arr)
    out.parent.mkdir(parents=True,exist_ok=True)
    Image.fromarray(edited,'RGBA').save(out,optimize=False)
    print('v2 PNG:',out,'SHA256:',hashlib.sha256(out.read_bytes()).hexdigest(), 'changed_pixels:',int(np.any(edited!=arr,axis=2).sum()))

if __name__=='__main__':
    main()
