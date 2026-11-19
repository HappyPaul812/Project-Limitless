from pathlib import Path, PurePosixPath
from zipfile import ZipFile
from hashlib import sha256
from io import BytesIO
from PIL import Image,ImageDraw,ImageFont
import numpy as np,json,re

out=Path("Temp/WispIdle8V2Review");out.mkdir(exist_ok=True)
source=Path("F:/Downloads")
hashes=["a4db522ec21d96edd7718655bb5889f32d9d25c5cfe79c0c44dbbf729428e58d","8be07f1984d77065743ff587fb2319e3225f1003e1d8201169974cb9dc413f9c"]
def sha(b):return sha256(b).hexdigest()
def png(data):
    with Image.open(BytesIO(data)) as im:
        assert im.format=="PNG"
        return im.copy()
def archive(name,expected):
    raw=(source/name).read_bytes()
    assert sha(raw)==expected
    with ZipFile(BytesIO(raw)) as z:
        names=z.namelist()
        assert not z.testzip() and len(set(names))==len(names)
        assert all(not PurePosixPath(n).is_absolute() and ".." not in PurePosixPath(n).parts and "\\" not in n for n in names)
        entries={n:z.read(n) for n in names}
    manifest=json.loads(entries["Manifest.json"])["files"]
    if isinstance(manifest,list):manifest={x["path"]:x for x in manifest}
    sums={n:h for h,n in (l.split(maxsplit=1) for l in entries["SHA256SUMS.txt"].decode().splitlines() if l.strip())}
    assert set(manifest)==set(sums)-{"Manifest.json"}==set(entries)-{"Manifest.json","SHA256SUMS.txt"}
    assert all(sha(entries[n])==h for n,h in sums.items())
    assert all((len(entries[n]),sums[n])==(v["bytes"],v["sha256"]) for n,v in manifest.items())
    sheetpath=next(n for n in entries if "4096x512" in n)
    sheet=png(entries[sheetpath])
    assert sheet.size==(4096,512) and sheet.mode=="RGBA"
    frames=[sheet.crop((i*512,0,(i+1)*512,512)) for i in range(8)]
    parts=sorted(n for n in entries if n.startswith("Sprite/") and n!=sheetpath)
    assert len(parts)==8 and len(set(f.tobytes() for f in frames))==8
    for f,part in zip(frames,parts):
        assert f.tobytes()==png(entries[part]).tobytes()
        x1,y1,x2,y2=f.getchannel("A").getbbox()
        assert 0<min(x1,y1,512-x2,512-y2)
    gifs={}
    for n,b in entries.items():
        if n.endswith(".gif"):
            with Image.open(BytesIO(b)) as im:
                timing=[]
                for i in range(im.n_frames):
                    im.seek(i);im.load();timing.append(im.info.get("duration"))
                assert im.n_frames==8 and im.info.get("loop")==0 and timing==[120]*8
                gifs[n]=dict(size=im.size,timing=timing,loop=0,sha256=sha(b))
    previews={n:png(b).size for n,b in entries.items() if n.startswith("Preview/") and n.endswith(".png")}
    return frames,entries,dict(zip=name,sha256=expected,bytes=len(raw),crc=len(entries),manifest=len(manifest),sums=len(sums),sheetSha256=sha(entries[sheetpath]),gifs=gifs,previews=previews)

v1,e1,a1=archive("BlizzardWisp_Issue112_Idle8.zip",hashes[0])
v2,e2,a2=archive("BlizzardWisp_Issue112_Idle8_v2.zip",hashes[1])
key=Path("Temp/Issue112WispReview20261117/Delivery/Candidate/BlizzardWisp_M5_KeyPose_512_RGBA_CANDIDATE.png")
keysha=sha(key.read_bytes())
assert keysha=="0c72c3f8fc80c9e74bf648236a0ff1746ef344ef71f03b4c005c38db59915586"
assert a1["sheetSha256"]=="28a8ae1d325617431ed3c2441467c235c323ce229fe91fa5190018ac8bde1cd7"
assert a2["sheetSha256"]=="ba6a7bddcd37bdf7fe78939862e93a50828561a08564756572d0bba19f51692a"
for n,b in e2.items():
    if n.endswith((".gif","Review.png")):(out/Path(n).name).write_bytes(b)
def scaled(f,s):return f.resize((s,s),Image.Resampling.LANCZOS)
for size in (128,40):
    strip=png(e2[f"Preview/Actual_{size}px_RGBA.png"])
    assert strip.size==(size*8,size) and strip.mode=="RGBA"
    assert all(strip.crop((i*size,0,(i+1)*size,size)).tobytes()==scaled(v2[i],size).tobytes() for i in range(8))

font=ImageFont.truetype("C:/Windows/Fonts/malgun.ttf",19)
label=ImageFont.truetype("C:/Windows/Fonts/malgun.ttf",15)
bg="#23313e"
def paste(canvas,f,xy,size):canvas.alpha_composite(scaled(f,size),xy)
board=Image.new("RGBA",(1200,531),bg)
d=ImageDraw.Draw(board)
d.text((20,12),"눈보라 정령 Idle8 · 같은 프레임과 크기로 비교",font=font,fill="white")
for i in range(8):d.text((112+133*i,49),str(i+1),font=label,fill="#ccdceb")
for y,size,frames,title in [(78,128,v1,"v1 · 128"),(214,128,v2,"v2 · 128"),(376,40,v1,"v1 · 40"),(437,40,v2,"v2 · 40")]:
    d.text((16,y+10),title,font=label,fill="white")
    for i,f in enumerate(frames):paste(board,f,(100+133*i+(128-size)//2,y),size)
d.text((19,498),"각 행의 마지막 프레임 다음에는 첫 프레임이 재생됩니다.",font=label,fill="#ccdceb")
board.convert("RGB").save(out/"V1_V2_FrameComparison.png")

motion=[]
for i in range(8):
    b=Image.new("RGBA",(1135,710),bg)
    d=ImageDraw.Draw(b)
    for col,(n,frames) in enumerate([("v1",v1),("v2",v2)]):
        x=22+col*560
        d.text((x,12),n+" · 512px",font=font,fill="white")
        paste(b,frames[i],(x,40),512)
        d.text((x,571),"128px",font=label,fill="white")
        d.text((x+175,571),"40px",font=label,fill="white")
        paste(b,frames[i],(x+49,568),128)
        paste(b,frames[i],(x+226,570),40)
    motion.append(b.convert("RGB"))
motion[0].save(out/"V1_V2_512_128_40_Loop.gif",save_all=True,append_images=motion[1:],duration=120,loop=0,disposal=2)

def differences(frames,size):
    rgb=np.stack([np.array(Image.alpha_composite(Image.new("RGBA",(size,size),bg),scaled(f,size)).convert("RGB"),dtype=np.float32) for f in frames])
    delta=np.abs(np.roll(rgb,-1,axis=0)-rgb)
    return [round(float(d.mean()),3) for d in delta]
def region(frames):
    # 중심부 핵과 주변 눈바람의 같은 고정 좌표 영역에서 전이별 평균 픽셀 차이를 구합니다.
    rgb=np.stack([np.array(Image.alpha_composite(Image.new("RGBA",(512,512),bg),f).convert("RGB"),dtype=np.float32) for f in frames])
    y,x=np.indices((512,512))
    r=np.hypot(x-256,y-256)
    delta=np.abs(np.roll(rgb,-1,axis=0)-rgb)
    return {key:[round(float(d[mask].mean()),3) for d in delta] for key,mask in {"core_r45":r<45,"band_r70_130":(r>=70)&(r<=130)}.items()}
report=dict(archives=[a1,a2],keySha256=keysha,keyEqualsFrame0=[png(key.read_bytes()).tobytes()==v[0].tobytes() for v in (v1,v2)],
            transitionMAE={f"v{i}":{str(s):differences(v,s) for s in (512,128,40)} for i,v in enumerate((v1,v2),1)},
            regionalMAE={f"v{i}":region(v) for i,v in enumerate((v1,v2),1)})
(out/"Review.json").write_text(json.dumps(report,indent=2),encoding="utf-8")
print(json.dumps(report))


def validate_gif(data,frames):
    with Image.open(BytesIO(data)) as gif:
        errors=[]
        for i in range(8):
            gif.seek(i)
            rgba=gif.convert("RGBA")
            back=rgba.getpixel((0,0))
            def comp(img):
                return np.array(Image.alpha_composite(Image.new("RGBA",img.size,back),img).convert("RGB"),dtype=np.float32)
            actual=comp(rgba)
            values=[float(np.abs(actual-comp(f)).mean()) for f in frames]
            assert int(np.argmin(values))==i and values[i]<3.0
            errors.append(round(values[i],3))
    return errors
report["gifNearestSheetFrameMAE"]={
    "v1":validate_gif(e1[next(n for n in e1 if n.endswith(".gif"))],v1),
    "v2":validate_gif(e2["Preview/BlizzardWisp_Idle8_v2_Loop.gif"],v2)}
(out/"Review.json").write_text(json.dumps(report,indent=2),encoding="utf-8")
print(report["gifNearestSheetFrameMAE"])


with Image.open(BytesIO(e2["Preview/BlizzardWisp_v1_v2_Comparison.gif"])) as gif:
    scores={"v1":[],"v2":[]}
    for i in range(8):
        gif.seek(i)
        actual=gif.convert("RGB")
        color=actual.getpixel((0,0))+(255,)
        for label,start,frames in [("v1",12,v1),("v2",356,v2)]:
            panel=np.array(actual.crop((start,48,start+320,368)),dtype=np.float32)
            errors=[float(np.abs(panel-np.array(Image.alpha_composite(Image.new("RGBA",(320,320),color),scaled(f,320)).convert("RGB"))).mean()) for f in frames]
            assert int(np.argmin(errors))==i
            scores[label].append(round(errors[i],3))
report["comparisonGifNearestFrameMAE"]=scores
(out/"Review.json").write_text(json.dumps(report,indent=2),encoding="utf-8")
print(scores)
