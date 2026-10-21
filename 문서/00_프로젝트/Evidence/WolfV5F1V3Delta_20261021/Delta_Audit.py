from pathlib import Path
from PIL import Image,ImageDraw
import numpy as np,json,hashlib,zipfile,csv,shutil
r=Path.cwd();o=r/'Temp/ArtReviewWolfV5F1V3_20261021';old=r/'Temp/ArtReviewWolfV4F1V2_20261021';e=o/'Worktree/문서/00_프로젝트/Evidence/WolfV5F1V3Delta_20261021';e.mkdir(parents=True,exist_ok=True)
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
load=lambda p:Image.open(p).convert('RGBA')
wpaths=[old/'Wolf/Assets/Wolf_KeyArt_v3_ORIGINAL_READ_ONLY.png',old/'Wolf/Assets/Wolf_KeyArt_v3_AlphaFix_v4.png',o/'Wolf/Assets/Wolf_KeyArt_v3_AlphaFix_v5.png'];ws=[load(p) for p in wpaths];a,b,c=map(np.array,ws);change=b[:,:,3]!=c[:,:,3]
v={'wolf_sources':{p.name:sha(p) for p in wpaths},'wolf_rgb_changed':int(np.any(b[:,:,:3]!=c[:,:,:3],axis=2).sum()),'wolf_alpha_changed':int(change.sum()),'wolf_alpha_increased':int((c[:,:,3]>b[:,:,3]).sum()),'wolf_regions':{},'packages':{},'f1':{}}
rows=list(csv.DictReader((o/'Wolf/QA/Changed_Alpha_Coordinates_v4_to_v5.csv').open()));coords=set()
for row in rows:
 x,y=int(row['x']),int(row['y']);assert int(b[y,x,3])==int(row['old_alpha_v4']) and int(c[y,x,3])==int(row['new_alpha_v5']);coords.add((y,x))
assert coords==set(zip(*np.where(change)));v['wolf_csv_exact']=True
boxes=[('Chest',(610,775,710,880)),('Prior flagged rear leg',(140,1110,210,1195)),('Delivered leg edit',(460,1140,570,1205)),('Front paw',(740,1190,915,1270))]
board=Image.new('RGB',(980,930),(28,34,42));d=ImageDraw.Draw(board);d.text((10,10),'OFFLINE / NOT UNITY GAMEVIEW | v3 / v4 / v5 | same source crops, nearest 2x',fill='white')
for j,(label,box) in enumerate(boxes):
 x0,y0,x1,y1=box;v['wolf_regions'][label]={'box':box,'changed':int(change[y0:y1,x0:x1].sum())};y=45+j*215
 d.text((10,y),label+' '+str(box),fill='white')
 for i,im in enumerate(ws):
  crop=im.crop(box).resize(((x1-x0)*2,(y1-y0)*2),Image.Resampling.NEAREST);x=10+i*325;d.rectangle((x,y+20,x+310,y+205),fill=(44,95,57));board.paste(crop,(x,y+25),crop);d.text((x+250,y+25),'v'+str(i+3),fill='white')
board.save(e/'Wolf_v3_v4_v5_AlphaCrops.png')
board=Image.new('RGB',(600,220),(28,34,42));d=ImageDraw.Draw(board);d.text((10,8),'OFFLINE / NOT UNITY GAMEVIEW | same full canvas 128px high',fill='white')
for i,im in enumerate(ws):
 t=im.resize((round(im.width*128/im.height),128),Image.Resampling.LANCZOS);x=25+i*195;d.rectangle((x,50,x+128,178),fill=(75,91,65));board.paste(t,(x+17,50),t);d.text((x,190),'Wolf v'+str(i+3),fill='white')
board.save(e/'Wolf_128px_v3_v4_v5.png')
for key,name in [('Wolf','Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v5.zip'),('F1','Limitless_Chapter3_NorthF1_Environment_6_TargetedFix_v3.zip')]:
 zpath=Path('F:/Downloads')/name;z=zipfile.ZipFile(zpath);decoded=0
 for n in z.namelist():
  if n.lower().endswith('.png'):
   im=Image.open(o/key/n);im.load();decoded+=1
 v['packages'][key]={'name':name,'sha256':sha(zpath),'files':len(z.namelist()),'crc':z.testzip(),'png_decode_count':decoded}
for line in (o/'F1/SHA256SUMS.txt').read_text().splitlines():
 h,n=line.split(None,1);assert sha(o/'F1'/n.strip())==h
v['f1_sha_manifest_matches']=19
art=o/'F1/Art';prev=old/'F1/Art';earth=load(art/'NorthF1_Ground_Earth_Base_v2.png')
for p in sorted(art.glob('*.png')):
 im=Image.open(p);im.load();pold=prev/p.name.replace('_v3','_v2');before=np.array(load(pold));after=np.array(load(p));v['f1'][p.name]={'sha256':sha(p),'size':im.size,'mode':im.mode,'same_v2_bytes':sha(p)==sha(pold),'changed_pixels':int(np.any(before!=after,axis=2).sum())}
for version,base in [('v2',prev),('v3',art)]:
 thin=load(base/('NorthF1_Ground_ThinFrost_Base_'+version+'.png'));trans=load(base/('NorthF1_EarthToFrost_Transition_Overlay_'+version+'.png'));comp=Image.alpha_composite(earth,trans);t=np.array(thin);ec=np.array(comp);er=np.array(earth)
 v['f1'][f'thin_{version}_edge_max']=[int(np.abs(t[:,0].astype(int)-t[:,-1].astype(int)).max()),int(np.abs(t[0].astype(int)-t[-1].astype(int)).max())]
 v['f1'][f'join_{version}_max']=[int(np.abs(er[:,-1].astype(int)-ec[:,0].astype(int)).max()),int(np.abs(ec[:,-1].astype(int)-t[:,0].astype(int)).max())]
 v['f1'][f'thin_{version}_rgb_distance_gt28']=int((np.linalg.norm(t[:,:,:3].astype(float)-er[:,:,:3],axis=2)>28).sum())
 for count in [2,3]:
  repeat=Image.new('RGBA',(256*count,256*count))
  for x in range(count):
   for y in range(count):repeat.paste(thin,(x*256,y*256))
  repeat.convert('RGB').save(e/f'ThinFrost_{version}_{count}x{count}.png')
 strip=Image.new('RGBA',(768,512))
 for y in [0,256]:
  for i,im in enumerate([earth,comp,thin]):strip.paste(im,(i*256,y))
 strip.convert('RGB').save(e/f'Transition_{version}_Adjacent.png')
board=Image.new('RGB',(800,1110),(28,34,42));d=ImageDraw.Draw(board);d.text((10,8),'OFFLINE / NOT UNITY GAMEVIEW | 3x3 native pixels, v2 above / v3 below',fill='white')
for i,version in enumerate(['v2','v3']):
 im=Image.open(e/f'ThinFrost_{version}_3x3.png');board.paste(im.resize((512,512),Image.Resampling.NEAREST),(10,35+i*535));d.text((540,45+i*535),version,fill='white')
board.save(e/'ThinFrost_Repeat_Comparison.png')
board=Image.new('RGB',(790,1090),(28,34,42));d=ImageDraw.Draw(board);d.text((10,8),'OFFLINE / NOT UNITY GAMEVIEW | Earth -> Earth+Transition -> ThinFrost',fill='white')
for i,version in enumerate(['v2','v3']):board.paste(Image.open(e/f'Transition_{version}_Adjacent.png'),(10,45+i*520));d.text((10,28+i*520),version,fill='white')
board.save(e/'Transition_Adjacent_Comparison.png')
board=Image.new('RGB',(560,500),(28,34,42));d=ImageDraw.Draw(board);d.text((10,8),'OFFLINE / NOT UNITY GAMEVIEW | 256px / 128px',fill='white')
for i,(version,base) in enumerate([('v2',prev),('v3',art)]):
 im=Image.alpha_composite(earth,load(base/('NorthF1_Prop_Waymarker_01_'+version+'.png')));x=10+i*275;board.paste(im,(x,50));board.paste(im.resize((128,128),Image.Resampling.NEAREST),(x+60,345));d.text((x,30),version,fill='white')
board.save(e/'Waymarker_256_128_Comparison.png')
shutil.copy(o/'Wolf/QA/Changed_Alpha_Coordinates_v4_to_v5.csv',e/'Delivered_Alpha_Changes.csv')
(e/'Validation.json').write_text(json.dumps(v,indent=2),encoding='utf-8');print(json.dumps(v,indent=2))
