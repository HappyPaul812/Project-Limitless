from pathlib import Path
import json,hashlib,shutil,gzip,csv
import numpy as np
from PIL import Image,ImageDraw

R=Path('F:/study/codex/Project-Limitless');B=R/'Temp/JointVillageV4WolfV5QA20261021';OLD=R/'Temp/JointVillageV3WolfV4QA20261021';E=B/'Worktree/문서/00_프로젝트/Evidence/JointVillageV4WolfV5_20261021';P=B/'UnityHarness/Output'
for p in P.glob('A_*.png'):shutil.copy2(p,E/p.name)
shutil.copy2(P/'Village.json',E/'A_Unity_Village.json');shutil.copy2(P/'Playback.json',E/'B_Unity_Playback.json')
(E/'Unity.log.txt.gz').write_bytes(gzip.compress((B/'UnityHarness/Unity.log').read_bytes(),mtime=0))
result=[]
for ratio in ['16x9','4x3','21x9']:
 name='A_Unity_'+ratio+'.png';a=Image.open(OLD/'UnityHarness/Output'/name).convert('RGB');b=Image.open(P/name).convert('RGB');ar=np.array(a);br=np.array(b);mask=np.any(ar!=br,axis=2);ys,xs=np.where(mask)
 result.append({'name':name,'changed_render_pixels':int(mask.sum()),'changed_bbox_exclusive':[int(xs.min()),int(ys.min()),int(xs.max()+1),int(ys.max()+1)] if len(xs) else None})
 c=Image.new('RGB',(1200,365),'#30363d');d=ImageDraw.Draw(c)
 for i,im in enumerate([a,b]):
  im.thumbnail((600,337));c.paste(im,(i*600,28));d.text((i*600+8,6),ratio+(' BEFORE v3' if i==0 else ' AFTER v4')+' | actual Unity render',fill='white')
 c.save(E/('A_Unity_BeforeAfter_'+ratio+'.jpg'))
(E/'A_RenderDelta.json').write_text(json.dumps(result,indent=2))
zoom=Image.new('RGB',(900,700),'#30363d');d=ImageDraw.Draw(zoom)
for row,(rel,box) in enumerate([('Buildings/Frostway_SkillShop_Building.png',(0,180,70,390)),('Buildings/Modular/Frostway_Roof_Gable.png',(70,150,160,219))]):
 for col,root in enumerate([OLD/'Extracted/A',B/'Extracted/A']):
  im=Image.open(root/rel).convert('RGBA').crop(box);scale=min(400/im.width,300/im.height);im=im.resize((int(im.width*scale),int(im.height*scale)))
  x=col*450;y=row*350;zoom.paste(im,(x+15,y+30),im);d.text((x+8,y+5),Path(rel).name+(' v3' if col==0 else ' v4'),fill='white')
zoom.save(E/'A_RawTarget_BeforeAfter.png')
data=json.loads((P/'Playback.json').read_text());cycle=[]
for i,s in enumerate(data['samples']):
 if s['frame']==0 and s['seconds']>.4:
  c=data['samples'][i:i+6]
  if [x['frame'] for x in c]==list(range(6)):cycle=c;break
assert cycle
frames=[];single=[];seq=Image.new('RGB',(1024,864),'#30363d')
for i,s in enumerate(cycle):
 im=Image.open(P/s['png']).convert('RGB');can=Image.new('RGB',(512,320),'#30363d');can.paste(im,(0,32));d=ImageDraw.Draw(can);d.text((8,6),f'UNITY PLAY: v4 LEFT / v5 RIGHT | frame {i:02}',fill='white');d.text((8,21),'128px cells above / 40px cells below; fixed size/pivot',fill='white');frames.append(can);single.append(im.crop((320,42,448,138)));seq.paste(im,((i%2)*512,(i//2)*288));shutil.copy2(P/s['png'],E/f'B_Unity_Frame{i:02}.png')
for name,fs in [('B_Unity_v4_v5_128_40px.gif',frames),('B_Unity_v5_128px.gif',single)]:fs[0].save(E/name,save_all=True,append_images=fs[1:],duration=[80,80,90,80,80,90],loop=0)
seq.save(E/'B_Unity_Sequence.png');steady=[x for x in data['samples'] if x['seconds']>.5];dt=[steady[i+1]['seconds']-steady[i]['seconds'] for i in range(len(steady)-1)]
(E/'B_Unity_Summary.json').write_text(json.dumps({'unity':data['unityVersion'],'graphics':data['graphics'],'elapsed':data['elapsed'],'engine_frame_advance':data['samples'][-1]['engineFrame']-data['samples'][0]['engineFrame'],'steady_dt_min':min(dt),'steady_dt_max':max(dt),'complete_cycle':[x['frame'] for x in cycle]},indent=2))
a=np.array(Image.open(B/'Extracted/B/Assets/Poses/Wolf_Run_05_v5.png'));b=np.array(Image.open(OLD/'Extracted/B/Assets/Poses/Wolf_Run_05_v4.png'));ys,xs=np.where(np.any(a!=b,axis=2));actual={(int(x),int(y),*b[y,x].tolist(),*a[y,x].tolist()) for x,y in zip(xs,ys)}
rows=list(csv.DictReader((B/'Extracted/B/QA/Frame05_Changed_Pixels.csv').open()));reported={tuple(int(row[k]) for k in ['x','y','v4_r','v4_g','v4_b','v4_a','v5_r','v5_g','v5_b','v5_a']) for row in rows};(E/'B_ChangedCSVCheck.json').write_text(json.dumps({'rows':len(rows),'unique_rows':len(reported),'exact_match_actual_rgba_changes':actual==reported},indent=2))
c=Image.new('RGB',(800,530),'#30363d');d=ImageDraw.Draw(c)
for i,r in enumerate([OLD/'Extracted/B',B/'Extracted/B']):
 version='v4' if i==0 else 'v5';im=Image.open(r/f'Assets/Poses/Wolf_Run_05_{version}.png').convert('RGBA').crop((290,205,370,305)).resize((400,500));c.paste(im,(i*400,30),im);d.text((i*400+8,5),version+' actual raw x290..370 y205..305',fill='white')
c.save(E/'B_Frame05_Raw_BeforeAfter.png');print('render_delta',result);print('timing',min(dt),max(dt),'CSV',actual==reported)
