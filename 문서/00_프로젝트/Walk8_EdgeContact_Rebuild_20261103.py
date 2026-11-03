# -*- coding: utf-8 -*-
"""LIMITLESS / Issue60 / SAFE OFFLINE Walk8 edge repair.
Requires numpy, opencv-python, Pillow in existing Python environment.
Example:
python Walk8_EdgeContact_Rebuild_20261103.py --source F:/Downloads/LIMITLESS_WALK8_FIX.zip --approved F:/Downloads/LIMITLESS_Spider_Idle8_Walk8.zip --output C:/Limitless_Temp/Walk8EdgeReview
The input ZIPs stay read-only; output only to a separate Temp folder.
This fixes four clipped leg edges. Actual grounded locomotion is NOT verified.
"""
import argparse,hashlib,io,json,zipfile
from pathlib import Path
import numpy as np
import cv2
from PIL import Image
EXPECTED="c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9"
PARAM={
1:("right",477,284,36,57,25.,293,496.5,.27,-1.0),
2:("right",475,277,38,58,27.,278,495.5,.32,-1.5),
6:("left",35,353,41,74,26.,370,17.0,.24,2.0),
7:("left",35,350,40,68,26.,358,17.0,.37,.6)}
ROW={1:list(range(277,303)),2:list(range(262,289))+list(range(294,297)),
6:list(range(328,386)),7:list(range(345,352))+list(range(353,372))}
def sha(b):return hashlib.sha256(b).hexdigest()
def fix(a,i):
    side,cx,cy,sx,sy,delta,tipy,cut,curve,roll=PARAM[i]
    y,x=np.mgrid[0:512,0:512].astype(np.float32)
    g=np.exp(-.5*((x-cx)/sx)**2-.5*((y-cy)/sy)**2)
    if side=="right":
        anchor=np.clip((x-411)/99,0,1)**1.1
        disp=delta*g*anchor
        mx=(x+disp).astype("float32")
        my=(y-roll*g*anchor).astype("float32")
    else:
        anchor=np.clip((121-x)/115,0,1)**1.1
        disp=delta*g*anchor
        mx=(x-disp).astype("float32")
        my=(y-roll*g*anchor).astype("float32")
    f=a.astype(np.float32)
    pm=np.concatenate([f[:,:,:3]*f[:,:,3:4]/255,f[:,:,3:4]],axis=2)
    moved=cv2.remap(pm,mx,my,cv2.INTER_LINEAR,
        borderMode=cv2.BORDER_CONSTANT,borderValue=(0,0,0,0))
    alpha=moved[:,:,3:4]
    rgb=np.divide(moved[:,:,:3],np.maximum(alpha/255,1e-4),
       out=np.zeros_like(moved[:,:,:3]),where=alpha>.001)
    b=np.uint8(np.clip(np.concatenate([rgb,alpha],axis=2),0,255).round())
    if side=="right":
        cutoff=cut-curve*np.abs(y-tipy)
        taper=np.clip((cutoff-x)/2.3,0,1)
        zone=np.clip((x-443)/35,0,1)*np.clip((y-233)/25,0,1)*np.clip((342-y)/24,0,1)
    else:
        cutoff=cut+curve*np.abs(y-tipy)
        taper=np.clip((x-cutoff)/2.3,0,1)
        zone=np.clip((108-x)/48,0,1)*np.clip((y-308)/28,0,1)*np.clip((407-y)/30,0,1)
    b[:,:,3]=np.uint8(np.clip(np.rint(b[:,:,3].astype(float)*((1-zone)+zone*taper)),0,255))
    edge=511 if side=="right" else 0
    assert not np.any(b[ROW[i],edge,3]),f"Walk{i:02d} edge still clipped"
    return b
def main():
    p=argparse.ArgumentParser()
    p.add_argument("--source",required=True)
    p.add_argument("--approved",required=True)
    p.add_argument("--output",required=True)
    q=p.parse_args()
    src,approved,out=Path(q.source),Path(q.approved),Path(q.output)
    if not src.is_file() or not approved.is_file():
        raise SystemExit("FILE_NOT_FOUND: check exact existing local ZIP paths")
    if any(part.lower() in {"assets","save","userdata","resources","scenes"} for part in out.parts):
        raise SystemExit("OUTPUT_IN_GAME_DIR_FORBIDDEN")
    (out/"Walk").mkdir(parents=True,exist_ok=True)
    (out/"Preview").mkdir(exist_ok=True)
    with zipfile.ZipFile(approved) as z:
        static=z.read("Spider/Source_KEEP/ForestSpider_v5_APPROVED_BYTE_KEEP.png")
        assert sha(static)==EXPECTED,"Approved original hash mismatch"
        idle=[sha(z.read(f"Spider/Idle/ForestSpider_Idle_{i:02d}.png")) for i in range(8)]
    sheet=Image.new("RGBA",(2048,1024),(0,0,0,0))
    images=[];audit=[]
    with zipfile.ZipFile(src) as z:
        for i in range(8):
            name=f"ForestSpider_Walk_{i:02d}.png"
            old=z.read("Walk/"+name)
            a=np.array(Image.open(io.BytesIO(old)).convert("RGBA"))
            assert a.shape==(512,512,4)
            if i in PARAM:
                new=fix(a,i)
                buffer=io.BytesIO()
                Image.fromarray(new,"RGBA").save(buffer,format="PNG",compress_level=7)
                data=buffer.getvalue()
            else:
                new=a;data=old
            (out/"Walk"/name).write_bytes(data)
            audit.append({"frame":i,"action":"EDGE_FIX" if i in PARAM else "EXACT_KEEP",
              "old_sha":sha(old),"new_sha":sha(data),
              "changed_pixels":int(np.any(a!=new,axis=2).sum())})
            im=Image.fromarray(new,"RGBA")
            images.append(im)
            sheet.alpha_composite(im,((i%4)*512,(i//4)*512))
    sheet.save(out/"Walk8_4x2_512_RGBA.png")
    for size in (128,40):
        frames=[]
        for im in images:
            bg=Image.new("RGBA",(size,size),(215,219,207,255))
            bg.alpha_composite(im.resize((size,size),Image.Resampling.LANCZOS))
            frames.append(bg.convert("RGB"))
        frames[0].save(out/"Preview"/f"Walk8_{size}px_10FPS.gif",
            save_all=True,append_images=frames[1:],duration=[100]*8,loop=0,
            disposal=2,optimize=False)
    qa={"approved_sha":EXPECTED,"idle8_keep_sha":idle,"walk":audit,
         "edge_clipping":"specified edge ranges alpha 0",
         "ground_contact":"NOT_VERIFIED","fps":10,
         "main_unity_save_changed":False}
    (out/"QA.json").write_text(json.dumps(qa,indent=2),encoding="utf-8")
    manifest=[]
    for file in sorted(out.rglob("*")):
        if file.is_file():
            manifest.append({"path":file.relative_to(out).as_posix(),
                             "sha256":sha(file.read_bytes()),"bytes":file.stat().st_size})
    (out/"Manifest.json").write_text(json.dumps(manifest,indent=2),encoding="utf-8")
    print("OFFLINE_EDGE_TECH_PASS; GAIT_ART_REVIEW_HOLD")
    print(json.dumps(audit,indent=2))
if __name__=="__main__":main()
