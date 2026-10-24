from pathlib import Path
import json,hashlib,csv,re,zipfile
from PIL import Image
import numpy as np
R=Path('F:/study/codex/Project-Limitless');O=R/'Temp/ArtPilots20261023'; E=O/'Worktree/문서/00_프로젝트/Evidence/ArtPilots20261023'
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
checks=[]
for name in ['Main23','Bee']:
 d=O/'Input'/name
 for l in (d/'SHA256SUMS.txt').read_text(encoding='utf-8-sig').splitlines():
  if not l.strip():continue
  h,f=l.split(None,1);f=f.strip().lstrip('*');p=d/f
  checks.append(dict(package=name,kind='SHA256SUMS',file=f,pass_=p.is_file() and sha(p)==h.lower()))
 m=json.loads((d/'Manifest.json').read_text(encoding='utf-8-sig'))
 for item in m['files']:
  f=item.get('file',item.get('path'));h=item.get('sha256')
  if f and h:checks.append(dict(package=name,kind='Manifest',file=f,pass_=(d/f).is_file() and sha(d/f)==h.lower()))
(E/'InternalManifestChecks.json').write_text(json.dumps(checks,indent=2),encoding='utf8');print('manifest:',len(checks),'fail:',[c for c in checks if not c['pass_']])
base=R/'Unity/Client/Assets'; paths=[]
for role,guid in [('Idle','24000000000000000000000000000004'),('Attack','24000000000000000000000000000005')]:
 matches=[p for p in base.rglob('*.meta') if ('guid: '+guid) in p.read_text(errors='replace')]
 for p in matches:
  active=p.with_suffix('');original=base/'ThirdParty/2DPIXX/PilotBee/Original'/active.name
  paths.append(dict(role=role,guid=guid,active=str(active.relative_to(R)),sha256=sha(active),source=str(original.relative_to(R)),sourceSHA256=sha(original),byte_identical=sha(active)==sha(original),size=Image.open(active).size))
(E/'VenomBeeSourceMapping.json').write_text(json.dumps(paths,indent=2),encoding='utf8')
print(json.dumps(paths,indent=2))
# 部分alphaを不良と決めつけず、実際の値分布と黒縁の候補数を採取します。
alpha=[]
for p in list((O/'Input/Main23').glob('*/*CANDIDATE.png'))+[O/'Input/Bee/VenomBee_OriginalKeyArt_Pilot_v1_512.png',O/'Input/Bee/VenomBee_Preview_128.png',O/'Input/Bee/VenomBee_Preview_40.png']:
 a=np.array(Image.open(p).convert('RGBA'));v,c=np.unique(a[:,:,3],return_counts=True);mask=(a[:,:,3]>0)&(a[:,:,3]<255);black=(a[:,:,:3].max(2)<35)&(a[:,:,3]>128)
 alpha.append(dict(file=str(p.relative_to(O/'Input')),top_alpha_values=sorted(zip(v.tolist(),c.tolist()),key=lambda t:t[1],reverse=True)[:7],partial_black_count=int((black&mask).sum()),black_opaque_count=int((black&~mask).sum())))
(E/'AlphaHistogram.json').write_text(json.dumps(alpha,indent=2),encoding='utf8');print(json.dumps(alpha,indent=2))
