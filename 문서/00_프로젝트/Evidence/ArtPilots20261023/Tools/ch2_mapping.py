from pathlib import Path
import re,hashlib,json
R=Path('F:/study/codex/Project-Limitless');O=R/'Temp/ArtPilots20261023';W=O/'Worktree';A=W/'Unity/Client/Assets';E=W/'문서/00_프로젝트/Evidence/ArtPilots20261023'
names=['11_SootHound','12_HeatwindHawk','13_FissureLizard','14_EmberBeetle','15_EmberWraith'];defs=[]
metas={}
for p in A.rglob('*.meta'):
 s=p.read_text(errors='replace');m=re.search(r'^guid: (\S+)',s,re.M)
 if m:metas[m[1]]=p
for name in names:
 p=A/'_Project/Resources/MonsterDefinitions'/f'{name}.asset';s=p.read_text();g=re.search(r'fieldSprite:.*guid: ([0-9a-f]+)',s)[1];meta=metas[g];png=meta.with_suffix('');text=meta.read_text();defs.append(dict(definition=name,guid=g,file=str(png.relative_to(W)),sha256=hashlib.sha256(png.read_bytes()).hexdigest(),ppu=re.search(r'spritePixelsToUnits: (\S+)',text)[1],basis='actual explicitIdleFrames[0] in current MonsterDefinition'))
(E/'Chapter2FiveReferenceMapping.json').write_text(json.dumps(defs,indent=2),encoding='utf8');print(json.dumps(defs,indent=2))
