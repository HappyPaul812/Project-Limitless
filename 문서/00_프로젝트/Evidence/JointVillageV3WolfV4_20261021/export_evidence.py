from pathlib import Path
import json,hashlib,csv,shutil,gzip
import numpy as np
from PIL import Image,ImageDraw

ROOT=Path('F:/study/codex/Project-Limitless');BASE=ROOT/'Temp/JointVillageV3WolfV4QA20261021';E=BASE/'Worktree/문서/00_프로젝트/Evidence/JointVillageV3WolfV4_20261021';A=BASE/'Extracted/A';B=BASE/'Extracted/B';PLAY=BASE/'UnityHarness/Output'
v=json.loads((E/'A_Validation.json').read_text());assets=[x for x in v['files'] if x['path'].endswith('.png') and not x['path'].startswith(('Preview/','QA/'))]
gallery=Image.new('RGB',(1260,9*185),'#30363d');d=ImageDraw.Draw(gallery)
issues={'Buildings/Frostway_SkillShop_Building.png':(12,196,29,376),'Buildings/Frostway_GeneralShop_Building.png':(12,254,14,301),'Buildings/Modular/Frostway_Roof_Gable.png':(100,183,144,207),'Buildings/Modular/Frostway_Roof_Shingle_B.png':(12,34,39,188),'Buildings/Modular/Frostway_Door_StoneFrame.png':(12,33,25,181),'Buildings/Modular/Frostway_Door_WoodFrame.png':(12,31,24,182),'Buildings/Modular/Frostway_Door_StoneArch.png':(12,137,13,191)}
issues.update({'Buildings/Modular/Frostway_Wall_Plain.png':(12,136,16,187),'Props/Frostway_Fence.png':(270,12,435,65),'Props/Frostway_SupplyBox_Food.png':(12,81,32,136),'Props/Frostway_SupplyCrate_Brace.png':(12,124,20,159),'Props/Frostway_SupplyCrate_Small.png':(12,13,36,129),'Props/Frostway_SupplyCrate_Wood.png':(12,28,37,157),'Props/Frostway_SupplySacks.png':(12,12,68,21),'Props/Frostway_Washline.png':(104,12,228,23),'Props/Frostway_Bench.png':(12,108,17,127),'Portal_Inactive/Frostway_StonePlinth_Short.png':(21,12,235,112),'Portal_Inactive/Frostway_Portalsite_LowWall.png':(12,136,18,152)})
with (E/'A_PNG43_Review.csv').open('w',encoding='utf-8-sig',newline='') as f:
 w=csv.writer(f);w.writerow(['path','sha256','format_tech','art','use','reason'])
 for i,r in enumerate(assets):
  p=r['path'];im=Image.open(A/p).convert('RGBA');im.thumbnail((225,140));x=i%5*252;y=i//5*185;gallery.paste(im,(x+(250-im.width)//2,y+20),im);d.text((x+4,y+163),Path(p).stem.replace('Frostway_','')[:35],fill='white')
  art='FAIL' if p in issues else 'HOLD' if p.startswith(('Tiles/','Transitions/','Buildings/Modular/','Signs/')) else 'PASS'
  use='FAIL' if p in issues else 'HOLD' if art=='HOLD' else 'PASS'
  why='foreign source fragment; see exact rect' if p in issues else 'repeat/module alignment/service scale requires review' if art=='HOLD' else 'offline art recommendation only; final user approval pending'
  w.writerow([p,r['sha256'],'PASS',art,use,why])
gallery.save(E/'A_PNG43_ContactSheet.png');(E/'A_DefectRects.json').write_text(json.dumps({'origin':'top-left pixels, right/bottom exclusive','foreign_fragment_rects':issues},indent=2))
crops=Image.new('RGB',(1050,600),'#30363d');d=ImageDraw.Draw(crops)
for i,(p,rect) in enumerate(list(issues.items())[:6]):
 im=Image.open(A/p).convert('RGBA');l,t,r,b=rect;l=max(0,l-8);t=max(0,t-8);r=min(im.width,r+8);b=min(im.height,b+8);im=im.crop((l,t,r,b));im.thumbnail((310,250));x=i%3*350;y=i//3*300;crops.paste(im,(x+15,y+35),im);d.text((x+5,y+6),Path(p).stem.replace('Frostway_',''),fill='white');d.text((x+5,y+20),str(rect),fill='#ffc85a')
crops.save(E/'A_ForeignFragment_Crops.png')
f1=ROOT/'Temp/Main22FirstPlayable20261021/Worktree/Unity/Client/Assets/_Project/Art/Environment/Chapter3/NorthF1'
paths=[f1/'NorthF1_Ground_Earth_Base_v2.png',f1/'NorthF1_Ground_ThinFrost_Base_v3.png']+sorted((A/'Tiles').glob('*.png'))
same=Image.new('RGB',(960,190),'#30363d');d=ImageDraw.Draw(same);sources=[]
for i,p in enumerate(paths):
 im=Image.open(p).convert('RGBA').resize((128,128));same.paste(im,(i*160+10,35),im);d.text((i*160+3,5),p.stem.replace('Frostway_','').replace('NorthF1_','')[:22],fill='white');sources.append({'path':str(p),'sha256':hashlib.sha256(p.read_bytes()).hexdigest(),'ppu':128,'world_width':2,'display_px':128})
same.save(E/'A_F1_SameScale.png');(E/'A_F1_References.json').write_text(json.dumps(sources,indent=2))
for p in PLAY.glob('A_*.png'):shutil.copy2(p,E/p.name)
shutil.copy2(PLAY/'Village.json',E/'A_Unity_Village.json')
(E/'Unity_Final.log.txt.gz').write_bytes(gzip.compress((BASE/'UnityHarness/Unity_Final.log').read_bytes(),mtime=0))
data=json.loads((PLAY/'Playback.json').read_text());shutil.copy2(PLAY/'Playback.json',E/'B_Unity_Playback.json')
samples=data['samples'];cycle=[]
for i,s in enumerate(samples):
 if s['frame']==0 and s['seconds']>.4:
  c=samples[i:i+6]
  if [x['frame'] for x in c]==list(range(6)):cycle=c;break
assert cycle
frames=[];single=[];sequence=Image.new('RGB',(1024,864),'#30363d')
for i,s in enumerate(cycle):
 im=Image.open(PLAY/s['png']).convert('RGB');caption=Image.new('RGB',(512,320),'#30363d');caption.paste(im,(0,32));d=ImageDraw.Draw(caption);d.text((12,6),f'UNITY PLAY: v3 LEFT / v4 RIGHT | frame {i:02}',fill='white');d.text((12,21),'Fixed 128px cells; lower row 40px cells',fill='white');frames.append(caption);single.append(im.crop((320,42,448,138)));sequence.paste(im,((i%2)*512,(i//2)*288));shutil.copy2(PLAY/s['png'],E/f'B_Unity_Frame{i:02}.png')
for name,fs in [('B_Unity_v3_v4_128_40px.gif',frames),('B_Unity_v4_128px.gif',single)]:fs[0].save(E/name,save_all=True,append_images=fs[1:],duration=[80,80,90,80,80,90],loop=0)
sequence.save(E/'B_Unity_Sequence.png')
steady=[s for s in samples if s['seconds']>.5];dt=[steady[i+1]['seconds']-steady[i]['seconds'] for i in range(len(steady)-1)]
(E/'B_Unity_Summary.json').write_text(json.dumps({'graphics':data['graphics'],'unity':data['unityVersion'],'elapsed_seconds':data['elapsed'],'engine_frame_advance':samples[-1]['engineFrame']-samples[0]['engineFrame'],'steady_dt_min':min(dt),'steady_dt_max':max(dt),'full_cycle_indices':[s['frame'] for s in cycle],'gif_ms':[80,80,90,80,80,90]},indent=2))
old=ROOT/'Temp/JointVillageWolfQA20261021/Extracted/B/Assets/Poses';compare=Image.new('RGB',(1024,800),'#30363d');d=ImageDraw.Draw(compare)
for row,i in enumerate([4,5]):
 for col,(p,suffix) in enumerate([(old,'v3'),(B/'Assets/Poses','v4')]):
  im=Image.open(p/f'Wolf_Run_{i:02}_{suffix}.png').convert('RGBA');compare.paste(im,(col*512,row*400+16),im);d.text((col*512+6,row*400),f'{i:02} {suffix} raw fixed canvas',fill='white')
compare.save(E/'B_Raw04_05_Comparison.png')
# 꼬리 영역의 좌표 끝점을 명시해 납품 문서의 모호함을 분리합니다.
a=np.array(Image.open(B/'Assets/Poses/Wolf_Run_04_v4.png'));b=np.array(Image.open(old/'Wolf_Run_04_v3.png'));diff=np.any(a!=b,axis=2)
(E/'B_TailROI.json').write_text(json.dumps({'half_open_roi':[346,88,485,164],'changed_pixels_half_open':int(diff[88:164,346:485].sum()),'inclusive_roi':[346,88,485,164],'changed_pixels_inclusive':int(diff[88:165,346:486].sum()),'boundary_changed_y164_x':np.where(diff[164])[0].tolist()},indent=2))
print('exported',len(assets),'PNG reviews; steady',min(dt),max(dt))
