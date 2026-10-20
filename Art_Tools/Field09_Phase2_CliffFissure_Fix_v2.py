#!/usr/bin/env python3
# LIMITLESS Field09 Issue #38 — reproduce ONLY Cliff Blocker/Fissure Edge v2 pixel art.
# Source: Phase2 Pilot v1 actual PNGs, never overwrite originals. pip: Pillow numpy scipy
# Usage: python Field09_Phase2_CliffFissure_Fix_v2.py <extracted_original_Assets_folder> <new_output_folder>
import sys, hashlib
from pathlib import Path
import numpy as np
from PIL import Image
from scipy.ndimage import gaussian_filter, distance_transform_edt

FILES={
 "Cliff_Blocker":("LL_Field09_Phase2_Cliff_Blocker_v1.png",
                 "2e7a863547688773dbb3d6672623e0958fbab01c8241d2665d8739a0777a231c",
                 "ad995f3cc4914759d6c88fa47c80227cb1a7c5f1a9a549679e06472402fee9f1"),
 "Fissure_Edge":("LL_Field09_Phase2_Fissure_Edge_v1.png",
                 "7c892de74b1f3327b37f4fb3ff7673954d64542498b32f6ab76826e6ca53b1d0",
                 "c3dc0c131c11fef72e4d210a1a43528f9882d8c50486e8da0bc15b6984ea145a")
}
def sha(p):return hashlib.sha256(Path(p).read_bytes()).hexdigest()

def repair(source,kind):
    src=source.copy()
    yy,xx=np.indices((512,512))
    alpha=src[:,:,3].astype(np.float32)
    R,G,B=np.moveaxis(src[:,:,:3],-1,0).astype(np.float32)
    if kind=="Cliff_Blocker": bottom_start=265;stone_start=280
    else: bottom_start=210;stone_start=270
    perimeter_distance=distance_transform_edt(alpha>12)
    pale_matte=(G>83)&(B>70)&(G-B>-15)&(G-B<32)&(R>G+9)&(R<G+78)
    desaturated_fringe=(G>65)&(B>60)&(np.abs(G-B)<20)&(R>G+15)&(R<G+65)
    fringe=pale_matte|(desaturated_fringe & (perimeter_distance<10))
    specific_region=(yy>=bottom_start)&(perimeter_distance<21)
    matte_pixels=fringe & specific_region
    alpha_fade=np.clip(gaussian_filter(matte_pixels.astype("float32"),1.6)*2.6,0,1)
    alpha_fade=np.where((yy>=bottom_start)&(perimeter_distance<24),alpha_fade,0)
    new_alpha=np.uint8(np.clip(np.round(alpha*(1-alpha_fade)),0,255))
    core=matte_pixels & (perimeter_distance<14)
    new_alpha[core]=np.uint8(np.minimum(new_alpha[core],np.round(alpha[core]*0.02)))
    src[:,:,3]=new_alpha
    for x in range(512):
        candidates=np.flatnonzero((new_alpha[:,x]>190)&(yy[:,x]>stone_start))
        if len(candidates)==0:continue
        bottom=int(candidates[-1]);top=max(0,bottom-16)
        coords=np.arange(top,bottom+1)
        depth=np.clip((coords-(bottom-16))/16,0,1)
        stone=(R[top:bottom+1,x]<145)&(B[top:bottom+1,x]>R[top:bottom+1,x]*0.35)&(new_alpha[top:bottom+1,x]>70)
        for channel in range(3):
            values=src[top:bottom+1,x,channel].astype(np.float32)
            values[stone]*=1.0-0.12*depth[stone]
            src[top:bottom+1,x,channel]=np.uint8(np.clip(np.round(values),0,255))
    return src

def main():
    if len(sys.argv)!=3:raise SystemExit(__doc__ or "args: inputAssetsFolder outputFolder")
    root=Path(sys.argv[1]);target=Path(sys.argv[2]);target.mkdir(parents=True,exist_ok=True)
    for kind,(name,oldsha,newsha) in FILES.items():
        path=root/name
        assert sha(path)==oldsha, f"Source SHA mismatch: {name}"
        with Image.open(path) as im:
            im.load()
            assert im.size==(512,512) and im.mode=="RGBA", f"Source format mismatch: {name}"
            src=np.array(im)
        new=repair(src,kind)
        output=target/name.replace("_v1.png","_v2.png")
        Image.fromarray(new,"RGBA").save(output,optimize=True)
        current=sha(output)
        assert current==newsha, f"Output SHA mismatch: {output} {current} expected {newsha}"
        print(kind,"PASS",output,newsha)
    print("ONLY two candidate art PNGs generated. Official Glow, 3 approved samples, Scene and main were not modified.")

if __name__=="__main__":main()
