# -*- coding: utf-8 -*-
from pathlib import Path
import json,hashlib,subprocess,shutil,gzip,io,zipfile
from PIL import Image,ImageDraw
root=Path(r'F:/study/codex/Project-Limitless');base=root/'Temp/WolfRunV5Apply20261021';wt=base/'Worktree';e=wt/'문서/00_프로젝트/Evidence/WolfRunV5Apply20261021'
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
def git(*args,cwd=wt):return subprocess.check_output(['git',*args],cwd=cwd)
def save(n,obj): (e/n).write_text(json.dumps(obj,ensure_ascii=False,indent=2),encoding='utf8')
r=json.loads((base/'Runtime/RuntimeQA.json').read_text(encoding='utf8'))
assert all(c.startswith('PASS ') for c in r['checks'])
assert len(r['samples'])>15
shutil.copy2(base/'Runtime/RuntimeQA.json',e/'RuntimeQA.json')
summary={'checks_pass':len(r['checks']),'checks_fail':0,'runtime_errors':r['errors'],'trials':[]}
for t in range(3):
 samples=[s for s in r['samples'] if s['trial']==t]
 frames=[s['frame'] for s in samples]
 assert all(i in frames for i in range(6))
 assert any(a==5 and b==0 for a,b in zip(frames,frames[1:]))
 intervals=[b['seconds']-a['seconds'] for a,b in zip(samples,samples[1:])]
 summary['trials'].append({'trial':t,'frames':frames,'frame_intervals_seconds':intervals,'engine_frames':[s['engineFrame'] for s in samples],'x_first':samples[0]['x'],'x_last':samples[-1]['x']})
 # 본게임 Presenter의 실제 RT 캡처를 늑대 위치에 맞춰 잘라 128px 표시 크기를 유지합니다.
 images=[]
 preview_samples=samples[6:12]
 for s in preview_samples:
  image=Image.open(base/'Runtime'/s['png']).convert('RGB');cx=640+s['x'];cy=360-s['y']
  image=image.crop((round(cx-80),round(cy-104),round(cx+80),round(cy+8)))
  images.append(image)
 durations=[max(10,round(i*100)*10) for i in intervals[6:12]]
 images[0].save(e/f'ActualPresenter_Trial{t}_128px.gif',save_all=True,append_images=images[1:],duration=durations,loop=0)
 for index in [0,4,5]:
  s=next(s for s in samples if s['frame']==index)
  shutil.copy2(base/'Runtime'/s['png'],e/f'ActualPresenter_Trial{t}_Frame{index}.png')
save('RuntimeSummary.json',summary)
for name in ['WolfApplyQA.cs','WolfApplyBoot.cs','prepare.py','finalize.py']:shutil.copy2(base/name,e/name)
with gzip.open(e/'Unity3.log.txt.gz','wb') as f:f.write((base/'Unity3.log').read_bytes())
baseline='e4e9891f6f01ea95cada9e63b1f09946d94d6041'
product_changed=git('diff','--name-only','--diff-filter=M',baseline,'--','Unity').decode().splitlines()
product_new=git('diff','--name-only','--diff-filter=A',baseline,'--','Unity').decode().splitlines()+git('ls-files','--others','--exclude-standard','Unity').decode().splitlines()
assert len(product_changed)==4 and len(product_new)==2
assert all('Wolf_Run6_v5' in p for p in product_new)
parity={p:sha(wt/p)==sha(base/'UnityQA'/Path(p).relative_to('Unity/Client')) for p in product_changed+product_new}
assert all(parity.values())
old='Unity/Client/Assets/_Project/Resources/CompanionAssaultValidation/Wolf_Run.png'
legacy={old:sha(root/old)==sha(wt/old),old+'.meta':(root/(old+'.meta')).read_text(encoding='utf8')==(wt/(old+'.meta')).read_text(encoding='utf8')}
assert all(legacy.values())
save('StaticDeltaQA.json',{'changed_existing':product_changed,'new':product_new,'unity_copy_sha_parity':parity,'legacy_sheet_meta_preserved':legacy,'files':{p:sha(wt/p) for p in product_changed+product_new}})
before=json.loads((base/'ProtectionBefore.json').read_text(encoding='utf8'))
after={str(p.relative_to(root)):sha(p) for folder in ['Unity/Client/Assets','Unity/Client/Packages','Unity/Client/ProjectSettings','Unity/Client/UserData','UserData'] for p in (root/folder).rglob('*') if p.is_file()}
protection={'protected_files':len(before['files']),'changed':[p for p,h in before['files'].items() if after.get(p)!=h],'new':[p for p in after if p not in before['files']],'root_head_unchanged':git('rev-parse','HEAD',cwd=root).decode().strip()==before['head'],'root_status_bytes_unchanged':hashlib.sha256(git('status','--porcelain=v1','-z',cwd=root)).hexdigest()==before['status'],'original_editor_actions':0,'push_merge':0}
assert not protection['changed'] and not protection['new'] and protection['root_head_unchanged'] and protection['root_status_bytes_unchanged']
save('Protection.json',protection)
print(json.dumps(summary,ensure_ascii=False)[:3000]);print(protection)
