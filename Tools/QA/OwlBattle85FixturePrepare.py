# -*- coding: utf-8 -*-
from pathlib import Path
import shutil,json,hashlib,zipfile
root=Path(r'F:/study/codex/Project-Limitless');task=root/'Temp/Main24FrostOwlFinish20261103';w=task/'Worktree';q=task/'QA/Client';p=w/'Unity/Client'
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
z=Path(r'F:/Downloads/Limitless_FrostOwl_Attack02_Continuity_20261103.zip')
assert sha(z)=='d6648b7451dc2184c2187c0b1c2f8e0f52fc800ac6975753d0f6d38f6fbf9192'
art=p/'Assets/_Project/Resources/Main24FrostOwlCandidate';art.mkdir(parents=True,exist_ok=True)
proof={'zip_sha256':sha(z),'frames':[]}
with zipfile.ZipFile(z) as archive:
 for i in range(4):
  entry=next(n for n in archive.namelist() if f'Attack_{i:02}_512_RGBA' in n and n.endswith('.png') and 'Candidate/Attack/' in n)
  out=art/'Attack'/Path(entry).name;out.parent.mkdir(exist_ok=True);out.write_bytes(archive.read(entry));proof['frames'].append({'file':str(out.relative_to(p/'Assets')),'sha256':sha(out),'source':str(z)+'!'+entry})
refs=json.loads((root/'Temp/Issue84OwlSpiderUnityDelta20261103/ReferenceProtection.json').read_text(encoding='utf-8-sig'))
for name,h in refs.items():
 if 'Owl' not in name or '/Frames/Idle/' not in name.replace('\\','/'):continue
 source=Path(name);assert sha(source)==h
 out=art/'Idle'/source.name;out.parent.mkdir(exist_ok=True);shutil.copy2(source,out);proof['frames'].append({'file':str(out.relative_to(p/'Assets')),'sha256':h,'source':str(source)})
assert len(proof['frames'])==12
(task/'InputProtection.json').write_text(json.dumps(proof,ensure_ascii=False,indent=2),encoding='utf-8')
# 새 fixture에만 복사합니다. 원본 Unity나 UserData는 경유하지 않습니다.
q.mkdir(parents=True,exist_ok=True)
if not (q/'Assets').exists():
 shutil.copytree(p/'Assets',q/'Assets',ignore=shutil.ignore_patterns('*.wav','*.mp3','*.ogg','*.zip','*.psd','*.blend'))
else:
 for source in (p/'Assets/_Project/Scripts').rglob('*.cs'):
  out=q/'Assets/_Project/Scripts'/source.relative_to(p/'Assets/_Project/Scripts');out.parent.mkdir(parents=True,exist_ok=True);shutil.copy2(source,out)
 shutil.copytree(art,q/'Assets/_Project/Resources/Main24FrostOwlCandidate',dirs_exist_ok=True)
# 이전 자동 QA는 가져오지 않습니다. 이번 Editor harness만 새로 배치합니다.
editor=q/'Assets/_Project/Scripts/Editor'
if editor.exists():
 for f in editor.rglob('*.cs'):f.rename(f.with_suffix('.cs.disabled'))
prior=root/'Temp/Main24F2FirstTraversal20261103/QA/Client'
for folder in ['Packages','ProjectSettings']:
 if not (q/folder).exists():shutil.copytree(prior/folder,q/folder)
assert '6000.6.5f1' in (q/'ProjectSettings/ProjectVersion.txt').read_text()
for name,folder in [('OwlBattle85Batch.cs','Assets/Editor'),('OwlBattle85Probe.cs','Assets/QA85')]:
 (q/folder).mkdir(parents=True,exist_ok=True);shutil.copy2(task/name,q/folder/name)
print('Fixture prepared',len(proof['frames']),'unchanged approved frames')
