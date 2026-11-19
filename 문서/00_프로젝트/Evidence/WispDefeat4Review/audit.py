from pathlib import Path,PurePosixPath
from zipfile import ZipFile
from io import BytesIO
from hashlib import sha256
from PIL import Image,ImageDraw,ImageFont
import numpy as np,json
src=Path('F:/Downloads')
out=Path('Temp/WispDefeat4Review');out.mkdir(exist_ok=True)
def sha(data):return sha256(data).hexdigest()
def img(data):
    with Image.open(BytesIO(data)) as im:
        assert im.format=='PNG'
        return im.copy()
def pack(kind,count,sha_png,sha_zip=None):
    suffix={'v1':'Idle8','v2':'Idle8_v2'}.get(kind,kind)
    raw=(src/f'BlizzardWisp_Issue112_{suffix}.zip').read_bytes()
    if sha_zip: assert sha(raw)==sha_zip
    with ZipFile(BytesIO(raw)) as z:
        assert z.testzip() is None
        paths=z.namelist()
        assert len(set(paths))==len(paths)
        assert all(not PurePosixPath(n).is_absolute() and '..' not in PurePosixPath(n).parts and '\\' not in n for n in paths)
        data={n:z.read(n) for n in paths}
    manifest=json.loads(data['Manifest.json'])['files']
    if isinstance(manifest,list):manifest={v['path']:v for v in manifest}
    sums={n:h for h,n in (l.split(maxsplit=1) for l in data['SHA256SUMS.txt'].decode().splitlines() if l.strip())}
    assert set(manifest)==set(sums)-{'Manifest.json'} and set(sums)==set(data)-{'SHA256SUMS.txt'}
    assert all((len(data[n]),sha(data[n]))==(v['bytes'],v['sha256']) for n,v in manifest.items())
    assert all(sha(data[n])==h for n,h in sums.items())
    sheet=next(n for n in data if n.startswith('Sprite/') and f'{count*512}x512' in n)
    assert sha(data[sheet])==sha_png
    image=img(data[sheet]);assert image.size==(count*512,512) and image.mode=='RGBA'
    frames=[image.crop((i*512,0,(i+1)*512,512)) for i in range(count)]
    parts=sorted(n for n in data if n.startswith('Sprite/') and n!=sheet)
    assert len(parts)==count and len({f.tobytes() for f in frames})==count
    for f,n in zip(frames,parts):
        assert f.tobytes()==img(data[n]).tobytes()
        l,t,r,b=f.getchannel('A').getbbox()
        assert min(l,t,512-r,512-b)>0
    return frames,data,dict(zipSha256=sha(raw),zipBytes=len(raw),crc=len(paths),manifest=len(manifest),sums=len(sums),sheetSha256=sha_png)

v1,_,a1=pack('v1',8,'28a8ae1d325617431ed3c2441467c235c323ce229fe91fa5190018ac8bde1cd7')
v2,_,a2=pack('v2',8,'ba6a7bddcd37bdf7fe78939862e93a50828561a08564756572d0bba19f51692a')
attack,_,aa=pack('Attack4',4,'f8ca104d24c2826c4a757c5dd3ad80e30e29b222cbdcd6e38c4eaa4b051a9934','a3f919971890686617e8335e65749f1c05dc71394a5b3f11d9234b8b589d792f')
hit,_,ah=pack('Hit4',4,'2e07e2178f48bf0dcd6367a73a9f9c4af5311073df2dc2c9a7e33e139b311de1','be090af54ba2e4699b72b4325e876d2248c79b2cb1a0ca6800bb890b34625a8c')
death,data,ad=pack('Defeat4',4,'5f4c5942895035b23d824707a7e4e534c20c323b4a3aa82bc5cd824735d23299','006f1d72bf7b536ced44ea8998abd8402d63950e78c0862a1cab4a91dba28a91')
keypath=Path('Temp/Issue112WispReview20261117/Delivery/Candidate/BlizzardWisp_M5_KeyPose_512_RGBA_CANDIDATE.png')
keysha=sha(keypath.read_bytes());assert keysha=='0c72c3f8fc80c9e74bf648236a0ff1746ef344ef71f03b4c005c38db59915586'
live=Path('Unity/Client/Assets/_Project/Art/Monsters/BlizzardWisp/BlizzardWisp_Idle8_v2.png')
assert sha(live.read_bytes())==a2['sheetSha256']
assert attack[-1].tobytes()==v2[6].tobytes() and hit[-1].tobytes()==v2[1].tobytes()
for size in [128,40]:
    strip=img(data[f'Preview/BlizzardWisp_Defeat4_Actual_{size}px_RGBA.png'])
    assert strip.size==(size*4,size) and strip.mode=='RGBA'
    assert all(strip.crop((i*size,0,(i+1)*size,size)).tobytes()==f.resize((size,size),Image.Resampling.LANCZOS).tobytes() for i,f in enumerate(death))
for name,b in data.items():
    if name.startswith('Preview/') and name.endswith(('Review.png','.gif')):
        (out/Path(name).name).write_bytes(b)
g=out/'BlizzardWisp_Defeat4_NonLoop_FinalHold.gif'
delays=[]; gif_scores=[]
with Image.open(g) as im:
    assert im.n_frames==4 and 'loop' not in im.info
    for i in range(4):
        im.seek(i);decoded=im.convert('RGBA')
        delays.append(im.info.get('duration'))
        bg=decoded.getpixel((0,0))
        def comp(x):return np.array(Image.alpha_composite(Image.new('RGBA',im.size,bg),x.resize(im.size,Image.Resampling.LANCZOS)).convert('RGB'),dtype=np.float32)
        a=comp(decoded); scores=[float(np.abs(a-comp(f)).mean()) for f in death]
        assert int(np.argmin(scores))==i
        gif_scores.append(round(scores[i],3))
assert delays==[160,190,230,1600]
font=ImageFont.truetype('C:/Windows/Fonts/malgun.ttf',17)
board=Image.new('RGBA',(1120,343),'#23313e');d=ImageDraw.Draw(board)
d.text((17,12),'눈보라 정령 · 승인 모션에서 전투불능까지 동일 배율',font=font,fill='white')
columns=[('원화',img(keypath.read_bytes())),('Idle v2',v2[1]),('Attack 방출',attack[1]),('Hit 마지막',hit[-1])]+[(f'Defeat {i+1}',f) for i,f in enumerate(death)]
for i,(title,f) in enumerate(columns):
    x=20+136*i
    d.text((x,55),title,font=font,fill='#dae9f5')
    for size,y in [(128,85),(40,239)]:
        board.alpha_composite(f.resize((size,size),Image.Resampling.LANCZOS),(x+(128-size)//2,y))
d.text((18,302),'위 128px · 아래 40px | Attack 방출은 대표 셀, Hit 마지막은 Idle v2와 같은 셀',font=font,fill='#dae9f5')
board.convert('RGB').save(out/'Approved_to_Defeat_Comparison.png')
metrics=[]
for f in death:
    a=f.getchannel('A')
    metrics.append(dict(bbox=a.getbbox(),alphaPixels=sum(a.histogram()[1:])))
report=dict(archives=dict(v1=a1,v2=a2,attack=aa,hit=ah,defeat=ad),keySha256=keysha,frames=metrics,gifDelaysMs=delays,gifNearestFrameMAE=gif_scores,gifSha256=sha(g.read_bytes()))
(out/'Review.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
print(json.dumps(report))
