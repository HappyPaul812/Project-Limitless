# -*- coding: utf-8 -*-
from pathlib import Path
import json,hashlib,subprocess,gzip,shutil,csv
import numpy as np
from PIL import Image,ImageDraw
root=Path(r'F:/study/codex/Project-Limitless');base=root/'Temp/FrostwayV5DeltaQA20261021';wt=base/'Worktree';e=wt/'문서/00_프로젝트/Evidence/FrostwayV5DeltaQA20261021';out=base/'UnityQA/Output'
def save(n,obj):(e/n).write_text(json.dumps(obj,ensure_ascii=False,indent=2),encoding='utf8')
r=json.loads((out/'UnityDelta.json').read_text());assert all(c['pass'] for c in r['checks'])
for p in out.iterdir():shutil.copy2(p,e/p.name)
for n in ['TileDeltaBoot.cs','prepare.py','export.py']:shutil.copy2(base/n,e/n)
with gzip.open(e/'Unity.log.txt.gz','wb') as f:f.write((base/'Unity.log').read_bytes())
for kind in ['Repeat3x3','Connections']:
 for ratio in ['16x9','4x3','21x9']:
  old=Image.open(out/f'v4_{kind}_{ratio}.png');new=Image.open(out/f'v5_{kind}_{ratio}.png');im=Image.new('RGB',(old.width*2,old.height+32),'#30363d');im.paste(old,(0,32));im.paste(new,(old.width,32));d=ImageDraw.Draw(im);d.text((10,8),'v4 / UNITY / SAME SCALE',fill='white');d.text((old.width+10,8),'v5 / UNITY / SAME SCALE',fill='white');im.save(e/f'Compare_{kind}_{ratio}.jpg',quality=94)
for name,rect in [('Earth',(352,2,736,386)),('Gravel',(864,2,1248,386)),('ThinFrost',(352,514,736,898))]:
 im=Image.new('RGB',(784,420),'#30363d');d=ImageDraw.Draw(im)
 for i,v in enumerate(['v4','v5']):im.paste(Image.open(out/f'{v}_Repeat3x3_16x9.png').crop(rect),(i*400,28));d.text((i*400+7,8),v+' '+name+' / actual Unity / tile128px',fill='white')
 im.save(e/f'Compare_{name}_Unity128px.png')
sorting={n:np.array_equal(np.array(Image.open(out/f'v4_{n}.png')),np.array(Image.open(out/f'v5_{n}.png'))) for n in ['DoorFront','DoorBehind']}
# 지면만 바뀌므로 전체 사진은 같지 않습니다. 건물/휠체어 ROI는 개별 기록으로 남깁니다.
save('DeltaSummary.json',{'unity':r['unity'],'graphics':r['graphics'],'focused_route_checks':len(r['checks']),'route_fail':0,'walk_radius':.3,'wheelchair_radius':.5,'mobility_scope':'Physics2D shape query proxies, original wheelchair visual; not production controller or manual gameplay','tile_display_pixels':128,'tech':'PASS','art':{'Earth':'HOLD','ThinFrost':'HOLD','Gravel':'HOLD'},'improvement':'Selected high-contrast clusters reduced, but grass/stone motifs still repeat visibly at 128px in 3x3 and village ground. Final acceptance reserved to user.','remaining_motif_rois_half_open':{'Earth_ThinFrost':[[0,96,89,256],[92,160,182,251]],'Gravel':[[4,2,118,122],[80,160,160,236]]},'sorting_full_image_equal':sorting,'original_editor_actions':0,'product_assets_changed':0,'previous_full_qa_repeated':0})
recon=json.loads((e/'OPEN16_Reconciliation.json').read_text(encoding='utf8'))
for row in recon['items']:row['severity']='기존 미술 FAIL / 게임 사용 FAIL; 원본에 우선순위·심각도 등급 미기재'
save('OPEN16_Reconciliation.json',recon)
with (e/'OPEN16_Reconciliation.csv').open('w',encoding='utf-8-sig',newline='') as f:
 w=csv.DictWriter(f,fieldnames=list(recon['items'][0]));w.writeheader();w.writerows(recon['items'])
# A 적용 완료 직후의 원본을 B 보호 기준으로 삼습니다. Wolf 승인6파일은 의도된 A 변경입니다.
baseline=json.loads((root/'Temp/WolfOriginalApply20261021/ProtectionAfterA.json').read_text(encoding='utf8'));changed=[p for p,h in baseline.items() if not (root/p).exists() or hashlib.sha256((root/p).read_bytes()).hexdigest()!=h];assert not changed
save('OriginalProtection_B.json',{'baseline':'after authorized A Wolf apply','baseline_files':len(baseline),'changed':changed,'original_unity_assets_added_by_B':0,'root_head':subprocess.check_output(['git','rev-parse','HEAD'],cwd=root).decode().strip(),'original_editor_actions':0})
print('Exported',len(r['images']),'Unity renders; route delta',len(r['checks']),'PASS; OPEN16 original found;0 resolved')
