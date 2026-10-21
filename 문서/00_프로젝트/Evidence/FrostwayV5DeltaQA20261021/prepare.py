# -*- coding: utf-8 -*-
from pathlib import Path
import zipfile,hashlib,json,io,csv,shutil
import numpy as np
from PIL import Image,ImageDraw
root=Path(r'F:/study/codex/Project-Limitless');base=root/'Temp/FrostwayV5DeltaQA20261021';wt=base/'Worktree';e=wt/'문서/00_프로젝트/Evidence/FrostwayV5DeltaQA20261021';e.mkdir(parents=True,exist_ok=True)
def sha(b):return hashlib.sha256(b).hexdigest()
z5p=Path('F:/Downloads/Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v5.zip');z4p=Path('F:/Downloads/Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v4.zip');assert sha(z5p.read_bytes())=='1affc1a4c23f3bcc6a9e9e18924b08bce3134eab065d8362757725f96e7f71b4'
z5=zipfile.ZipFile(z5p);z4=zipfile.ZipFile(z4p);assert z5.testzip() is None
assets=[n for n in z5.namelist() if n.endswith('.png') and n.split('/')[0] in ['Tiles','Transitions','Buildings','Props','Signs','Portal_Inactive']];assert len(assets)==43
manifest=json.loads(z5.read('Manifest.json'));refs=manifest['changes'];assert all(sha(z5.read(r['path']))==r['v5_sha256'] for r in refs)
sumsname=next(n for n in z5.namelist() if n.endswith('SHA256SUMS.txt'));sums=[]
for line in z5.read(sumsname).decode().splitlines():
 if not line.strip():continue
 h,n=line.split(maxsplit=1);n=n.lstrip('* ');assert sha(z5.read(n))==h;sums.append(n)
changed=[n for n in assets if z5.read(n)!=z4.read(n)];assert len(changed)==3 and all(n.startswith('Tiles/') for n in changed)
records=[]
for n in assets:
 im=Image.open(io.BytesIO(z5.read(n)));im.load()
 if n not in changed:continue
 a=np.array(Image.open(io.BytesIO(z4.read(n))).convert('RGBA'));b=np.array(im.convert('RGBA'));d=np.any(a!=b,axis=2);ys,xs=np.where(d)
 r={'path':n,'sha256_v4':sha(z4.read(n)),'sha256_v5':sha(z5.read(n)),'size':im.size,'mode':im.mode,'changed_rgba_pixels':int(d.sum()),'bbox_half_open':[int(xs.min()),int(ys.min()),int(xs.max()+1),int(ys.max()+1)],'border_1px_unchanged':bool(np.array_equal(a[0],b[0]) and np.array_equal(a[-1],b[-1]) and np.array_equal(a[:,0],b[:,0]) and np.array_equal(a[:,-1],b[:,-1])),'left_right_equal':bool(np.array_equal(b[:,0],b[:,-1])),'top_bottom_equal':bool(np.array_equal(b[0],b[-1])),'alpha_min':int(b[:,:,3].min()),'alpha_max':int(b[:,:,3].max())};records.append(r)
 assert im.size==(256,256) and im.mode=='RGBA'
(e/'PackageDelta.json').write_text(json.dumps({'sha256':sha(z5p.read_bytes()),'bytes':z5p.stat().st_size,'entries':len(z5.infolist()),'crc_bad':None,'manifest_match':len(refs),'sha256sums_match':len(sums),'changed':records,'unchanged_40':{n:sha(z5.read(n)) for n in assets if n not in changed}},indent=2),encoding='utf8')
prior=root/'Temp/JointVillageV3WolfV4QA20261021/Worktree/문서/00_프로젝트/Evidence/JointVillageV3WolfV4_20261021';csvp=prior/'A_PNG43_Review.csv';rectp=prior/'A_DefectRects.json';shutil.copy2(csvp,e/csvp.name);shutil.copy2(rectp,e/rectp.name)
oldrects=json.loads(rectp.read_text())['foreign_fragment_rects'];rows=list(csv.DictReader(csvp.open(encoding='utf-8-sig')));openrows=[r for r in rows if r['art']=='FAIL' and r['path'] not in ['Buildings/Frostway_SkillShop_Building.png','Buildings/Modular/Frostway_Roof_Gable.png']];assert len(openrows)==16
out=[];gallery=Image.new('RGB',(1200,4*250),'#30363d');draw=ImageDraw.Draw(gallery)
for i,r in enumerate(openrows):
 n=r['path'];rect=oldrects[n];same=sha(z5.read(n))==r['sha256'];assert same
 out.append({'file':n,'rect_half_open':rect,'severity':'기존 ART_FAIL / GAME_USE_FAIL (original CSV); priority not assigned in source','reproduction':'Display isolated RGBA PNG over contrasting background; inspect original top-left pixel rectangle at native scale or zoom. Actual village visibility depends on placement/sorting.','source_sha256':r['sha256'],'v5_sha256':sha(z5.read(n)),'resolution':'OPEN_UNCHANGED_BYTES','visual_recheck':'No new full-asset art QA; inherited coordinate and exact bytes verified'})
 im=Image.open(io.BytesIO(z5.read(n))).convert('RGBA');overlay=im.copy();ImageDraw.Draw(overlay).rectangle((rect[0],rect[1],rect[2]-1,rect[3]-1),outline='red',width=2);overlay.thumbnail((190,180));x=i%4*300;y=i//4*250;gallery.paste(overlay,(x,y+55),overlay);crop=im.crop(tuple(rect));crop.thumbnail((100,160));gallery.paste(crop,(x+195,y+60),crop);draw.text((x+3,y+3),n.split('/')[-1].replace('Frostway_','')[:38],fill='white');draw.text((x+3,y+23),str(rect),fill='#ffcc66');draw.text((x+3,y+230),'OPEN: v3/v4/v5 SHA identical',fill='#ffcc66')
gallery.save(e/'OPEN16_OriginalRects_v5.png');(e/'OPEN16_Reconciliation.json').write_text(json.dumps({'source_commit':'695f6f74e63996bb9d0ba0561dc6518f1e7af049','source_csv_sha256':sha(csvp.read_bytes()),'source_rect_sha256':sha(rectp.read_bytes()),'coordinate_origin':'top-left; right/bottom exclusive','found':True,'open_count':16,'solved_by_v5':0,'items':out},ensure_ascii=False,indent=2),encoding='utf8')
# QA 프로젝트에는 입력 PNG를 원본 바이트 그대로 복사합니다. 본게임 Assets는 건드리지 않습니다.
qa=base/'UnityQA';(qa/'Assets/Editor').mkdir(parents=True,exist_ok=True);(qa/'Packages').mkdir(exist_ok=True);(qa/'ProjectSettings').mkdir(exist_ok=True)
(qa/'Packages/manifest.json').write_text('{"dependencies":{"com.unity.modules.imageconversion":"1.0.0","com.unity.modules.jsonserialize":"1.0.0","com.unity.modules.physics2d":"1.0.0"}}')
shutil.copy2(root/'Unity/Client/ProjectSettings/ProjectVersion.txt',qa/'ProjectSettings/ProjectVersion.txt')
for label,z in [('v4',z4),('v5',z5)]:
 for n in assets:
  p=qa/'Input'/label/n;p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(z.read(n))
wheel=root/'Temp/JointVillageV4WolfV5QA20261021/UnityHarness/Input/wheel.png';shutil.copy2(wheel,qa/'Input/wheel.png')
print('SHA/CRC/Manifest PASS;3 changed/40 unchanged; OPEN16 original evidence found;0 resolved')
print(json.dumps(records,indent=2))
