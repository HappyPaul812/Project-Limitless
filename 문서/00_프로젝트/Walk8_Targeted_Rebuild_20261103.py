# -*- coding: utf-8 -*-
"""LIMITLESS Issue #60 Walk8 offline remake (2026-11-03).

Run ONLY in an isolated temporary folder; never write to Unity Assets, Save or main.
Requires Python 3.10+, numpy, opencv-python, Pillow.

python Walk8_Targeted_Rebuild_20261103.py --source-zip F:\Downloads\LIMITLESS_Spider_Idle8_Walk8.zip --out F:\Downloads\Walk8_Rebuilt_Review

If a different source ZIP is used, verify it contains Spider/Walk/ForestSpider_Walk_00..07.png.
Result: eight 512 RGBA frames, 4x2 sheet, 10FPS GIF and per-frame SHA/changed pixels.
Only frames 01/02/03/05/06/07 are changed; 00 and 04 are byte-exact copies.
The approved v5 source PNG and all Idle8 bytes are NEVER rewritten.
This is a per-leg local image deformation candidate, NOT hand-painted IK / foot-lock PASS.
"""
import argparse
import csv
import hashlib
import io
import json
import zipfile
from pathlib import Path

import cv2
import numpy as np
from PIL import Image

FOOT_KNEE = {
    "L_REAR": ((24,346),(83,279)),
    "L_UP": ((72,226),(120,137)),
    "L_MID": ((82,286),(106,225)),
    "L_FRONT": ((129,379),(150,320)),
    "R_FRONT": ((273,402),(314,329)),
    "R_MID": ((406,405),(393,324)),
    "R_REAR": ((496,310),(467,264)),
    "R_UP": ((472,345),(452,275)),
}
SWING = {
    1: {"L_FRONT": (12,-12), "R_REAR":(13,-10), "L_UP":(4,-4)},
    2: {"L_FRONT": (23,-22), "R_REAR":(21,-16), "L_MID":(7,-10),"R_UP":(5,-7)},
    3: {"L_FRONT": (13,-4), "R_REAR":(15,-3), "L_UP":(2,-3)},
    5: {"R_FRONT":(-13,-13), "L_REAR":(-11,-12), "R_UP":(-5,-5)},
    6: {"R_FRONT":(-22,-22), "L_REAR":(-20,-17), "R_MID":(-6,-11),"L_UP":(-6,-7)},
    7: {"R_FRONT":(-11,-4), "L_REAR":(-13,-3), "R_UP":(-3,-3)},
}
def sha(data):
    return hashlib.sha256(data).hexdigest()

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--source-zip", required=True)
    ap.add_argument("--out", required=True)
    a = ap.parse_args()
    out = Path(a.out).resolve()
    if any(q.lower() in ("assets","userdata","save") for q in out.parts):
        raise SystemExit("Unsafe output directory. Use an isolated Temp/Review folder.")
    (out/"Walk").mkdir(parents=True, exist_ok=True)
    y,x = np.mgrid[0:512,0:512].astype(np.float32)
    frames=[]
    audit=[]
    with zipfile.ZipFile(a.source_zip) as z:
        for i in range(8):
            fn=f"ForestSpider_Walk_{i:02d}.png"
            original=z.read("Spider/Walk/"+fn)
            arr=np.array(Image.open(io.BytesIO(original)).convert("RGBA"))
            if arr.shape!=(512,512,4):
                raise ValueError(fn + " has unexpected dimensions")
            if i in (0,4):
                result=arr
                newbytes=original  # exact preservation
            else:
                dx=np.zeros((512,512),np.float32)
                dy=np.zeros_like(dx)
                for tag,(fx,fy) in SWING[i].items():
                    foot,knee=FOOT_KNEE[tag]
                    for (px,py),weight,sx,sy in [
                        (foot,1.0,37,44),(knee,.40,44,47)
                    ]:
                        g=np.exp(-.5*((x-px)**2/sx**2+(y-py)**2/sy**2)).astype("float32")
                        dx += fx*weight*g
                        dy += fy*weight*g
                mapx=(x-dx).astype("float32")
                mapy=(y-dy).astype("float32")
                result=cv2.remap(arr,mapx,mapy,cv2.INTER_LINEAR,
                                 borderMode=cv2.BORDER_CONSTANT,borderValue=(0,0,0,0))
                unchanged=(np.abs(dx)+np.abs(dy)<.03)
                result[unchanged]=arr[unchanged]
                mem=io.BytesIO()
                Image.fromarray(result,"RGBA").save(mem,format="PNG")
                newbytes=mem.getvalue()
            (out/"Walk"/fn).write_bytes(newbytes)
            frames.append(Image.fromarray(result,"RGBA"))
            audit.append({
                "frame": i, "old_sha256":sha(original), "new_sha256":sha(newbytes),
                "changed_rgba_pixels":int(np.any(arr!=result,axis=2).sum()),
                "decision":"KEEP" if i in (0,4) else "LOCAL_LEG_REVISE"
            })
    # 4x2 / 512 cell sheet; not imported into Unity.
    sheet=Image.new("RGBA",(2048,1024),(0,0,0,0))
    for i,f in enumerate(frames):
        sheet.alpha_composite(f,((i%4)*512,(i//4)*512))
    sheet.save(out/"Walk8_4x2_512_RGBA.png")
    for size in (128,40):
        imgs=[]
        for f in frames:
            bg=Image.new("RGBA",(size,size),(216,222,216,255))
            bg.alpha_composite(f.resize((size,size),Image.Resampling.LANCZOS))
            imgs.append(bg.convert("RGB"))
        imgs[0].save(out/f"Walk8_{size}px_10FPS.gif",save_all=True,
                     append_images=imgs[1:],duration=[100]*8,loop=0,
                     optimize=False,disposal=2)
    (out/"SHA_Audit.json").write_text(json.dumps(audit,indent=2),encoding="utf-8")
    print("OFFLINE FILES GENERATED; Walk00/04 exact KEEP, 6 frames changed.")
    print("10FPS GIF uses 100ms per frame. Unity/foot lock/motion art not verified.")
    for q in audit:
        print(q["frame"],q["changed_rgba_pixels"],q["new_sha256"])

if __name__=="__main__":
    main()
