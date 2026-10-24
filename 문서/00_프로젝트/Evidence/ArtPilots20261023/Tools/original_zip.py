from pathlib import Path
import zipfile,hashlib,json
R=Path('F:/study/codex/Project-Limitless');E=R/'Temp/ArtPilots20261023/Worktree/문서/00_프로젝트/Evidence/ArtPilots20261023';p=R/'Unity/Client/Assets/ThirdParty/2DPIXX/PilotBee/Original/pilot_bee.zip'
with zipfile.ZipFile(p) as z:
 obj={'archive':str(p.relative_to(R)),'sha256':hashlib.sha256(p.read_bytes()).hexdigest(),'crc_bad':z.testzip(),'entries':z.namelist(),'png_entry_matches':{}}
 for n in z.namelist():
  if n.endswith('.png'):
   target=p.parent/Path(n).name
   if target.exists():obj['png_entry_matches'][n]=hashlib.sha256(z.read(n)).hexdigest()==hashlib.sha256(target.read_bytes()).hexdigest()
  if n.lower().endswith('license.txt'):obj['license_text']=z.read(n).decode(errors='replace')
(E/'PilotBeeOriginalArchive.json').write_text(json.dumps(obj,indent=2),encoding='utf8');print(json.dumps(obj,indent=2))
