from pathlib import Path
from PIL import Image, ImageDraw, ImageFont, ImageOps
from zipfile import ZipFile
from hashlib import sha256
from io import BytesIO
import json, re

out = Path("Temp/Issue116_117_118")
out.mkdir(parents=True, exist_ok=True)
resources = Path("Unity/Client/Assets/_Project/Resources")
def sha(data): return sha256(data).hexdigest()
def load(data):
    with Image.open(BytesIO(data)) as img:
        assert img.format == "PNG"
        return img.copy()
def review(name, digest, prefix, size):
    path = Path("F:/Downloads") / name
    assert sha(path.read_bytes()) == digest
    with ZipFile(path) as archive:
        assert archive.testzip() is None
        names = archive.namelist()
        assert len(set(names)) == len(names)
        manifest = json.loads(archive.read("Manifest.json"))
        mf = {entry["path"]: entry for entry in manifest["files"]}
        assert set(mf) == set(names) - {"Manifest.json","SHA256SUMS.txt"}
        sums = dict((p,h) for h,p in (s.split(maxsplit=1) for s in archive.read("SHA256SUMS.txt").decode().splitlines() if s.strip()))
        assert set(sums) == set(names) - {"SHA256SUMS.txt"}
        for p,h in sums.items(): assert sha(archive.read(p)) == h, p
        for p,entry in mf.items(): assert (len(archive.read(p)),sums[p]) == (entry["bytes"],entry["sha256"]),p
        entries = [p for p in names if p.startswith(prefix) and p.endswith(".png")]
        images, rows = {}, []
        for p in entries:
            data = archive.read(p)
            img = load(data)
            assert img.size == (size,size) and img.mode == "RGBA"
            alpha = img.getchannel("A")
            box = alpha.getbbox()
            assert alpha.getextrema() == (0,255) and 0 < min(box[0],box[1],size-box[2],size-box[3])
            if size == 128:
                assert not sum(alpha.histogram()[1:255])
                assert img.resize((64,64),Image.Resampling.NEAREST).resize((128,128),Image.Resampling.NEAREST).tobytes() == img.tobytes()
            rows.append(dict(file=p, sha256=sha(data), bytes=len(data), bbox=box, alpha0=alpha.histogram()[0], partialAlpha=sum(alpha.histogram()[1:255])))
            images[Path(p).name] = img
        assert len(set(row["sha256"] for row in rows)) == len(rows)
        for p in names:
            if p.startswith("Review_OFFLINE/") and p.endswith(".png"):
                (out/"Pet-512-Delivered.png").write_bytes(archive.read(p))
        return images, dict(zip=name,bytes=path.stat().st_size,sha256=digest,crc="PASS",manifestCount=len(mf),sumsCount=len(sums),assets=rows),manifest

loot, loot_info, loot_manifest = review("Chapter3_Loot_Issue116.zip","8d7ff5e8c3c16aebe9d2992ace2326002e8e72946d62c1b0faad37a251acb54c","Icons/",128)
pet, pet_info, _ = review("Limitless_PetManagement_117.zip","72b7d5312097ad0599916f92f656cdfbe896742bfb9c07468c5adf97b448cf8d","Candidates_512/",512)
assert len(loot)==6 and len(pet)==1
ch2_doc = Path("문서/00_프로젝트/Chapter2_Loot_Items_20261009.md").read_text(encoding="utf-8")
ch2 = {}
for match in re.finditer(r"\|(Loot_\w+\.png)\|\d+\|\[[^\]]+\]\|\d+\|"+r"\x60([a-f0-9]{64})\x60",ch2_doc):
    p,h = match.groups()
    data = (resources/"ItemIcons"/p).read_bytes()
    assert sha(data)==h, p
    ch2[p] = (load(data), h)
assert len(ch2)==7
for ref in loot_manifest["reference_sha256"]:
    assert ch2[ref["path"]][1]==ref["sha256"]
adopt = Path("Temp/Issue103Defeat113Review20261117/Pet113/Candidates_512/Arbel_PetAdoption.png").read_bytes()
assert sha(adopt)=="d64c342bd4bd8953bafce1feb8949d3cb5ce1a4b67ecf88cda3a6757beb4e47d"
npc = [("펫 관리 후보", next(iter(pet.values()))),("펫 분양 #113",load(adopt))]
npc_hashes = {"PetAdoption113":sha(adopt)}
for title,p in [("은행 OGA16","OGA16_Bank_Original_Approved.png"),("장비상인 OGA17","OGA17_EquipmentShop_Original_Approved.png"),("레온","Main13_Leon_Approved.png")]:
    data=(resources/"VillageNpcSprites/Eldiran"/p).read_bytes()
    npc.append((title,load(data)))
    npc_hashes[p]=sha(data)

font_path="C:/Windows/Fonts/malgun.ttf"
def font(n): return ImageFont.truetype(font_path,n)
def tile(board,img,x,y,size,gray=False):
    tile=img.convert("RGBA")
    if gray:
        alpha=tile.getchannel("A")
        tile=ImageOps.grayscale(tile).convert("RGBA")
        tile.putalpha(alpha)
    tile=tile.resize((size,size),Image.Resampling.NEAREST)
    board.alpha_composite(tile,(x,y))
def board(filename,groups,widths,sizes,title):
    card=max(sizes)+18
    b=Image.new("RGBA",(max(widths)*card+48,59+len(groups)*(61+sum(sizes)+len(sizes)*7)), "#172435")
    d=ImageDraw.Draw(b)
    d.text((24,15),title,font=font(22),fill="white")
    y=58
    for heading,items in groups:
        d.text((24,y),heading,font=font(16),fill="#a5c7ea")
        y+=28
        for col,(label,img) in enumerate(items):
            x=24+card*col
            d.text((x+card//2,y+8),label,font=font(12),fill="white",anchor="mm")
            row=y+20
            for idx,size in enumerate(sizes):
                xx=x+(card-size)//2
                d.rectangle((xx,row,xx+size-1,row+size-1),fill="#31445b" if idx==0 else "#101b2a")
                tile(b,img,xx,row,size,gray=idx==2)
                row+=size+7
        y+=33+sum(sizes)+len(sizes)*7
    b.convert("RGB").save(out/filename)

ch2_labels=["그을음 송곳니","열풍 깃털","균열 비늘","화열 갑각","잔불 파편","흑요석 갑각","열핵 조각"]
loot_group=[(i["name"],loot[Path(i["file"]).name]) for i in loot_manifest["icons"]]
board("Loot-Comparison.png",[("Chapter 2 승인 실물 · 128 / 40 / 40px 흑백",list(zip(ch2_labels,(v[0] for v in ch2.values())))),("Chapter 3 후보 · 128 / 40 / 40px 흑백",loot_group)],[7,6],[128,40,40],"Issue #116 · 전리품 6종 동일 배율")
board("Pet-Comparison.png",[("동일 캔버스 · 128 / 40 / 40px 흑백",npc)],[5],[128,40,40],"Issue #117 · 펫 관리 담당과 승인 NPC")
# 모든 원화는 캔버스 기준으로 나란히 축소합니다. 크롭이나 실루엣 확대를 하지 않습니다.
(out/"ArtQA.json").write_text(json.dumps(dict(packages=[loot_info,pet_info],ch2References={p:v[1] for p,v in ch2.items()},npcReferences=npc_hashes),indent=2,ensure_ascii=False),encoding="utf-8")
