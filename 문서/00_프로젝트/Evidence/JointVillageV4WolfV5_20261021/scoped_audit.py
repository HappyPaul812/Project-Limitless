"""새 두 PNG와 Frame05만 픽셀 대조하고 KEEP 자산은 SHA로 확인합니다."""
from pathlib import Path
import json,hashlib,zipfile,csv
import numpy as np
from PIL import Image
from scipy.ndimage import label

R=Path('F:/study/codex/Project-Limitless');B=R/'Temp/JointVillageV4WolfV5QA20261021';E=B/'Worktree/문서/00_프로젝트/Evidence/JointVillageV4WolfV5_20261021';OLD=R/'Temp/JointVillageV3WolfV4QA20261021/Extracted';sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
targets=['Buildings/Frostway_SkillShop_Building.png','Buildings/Modular/Frostway_Roof_Gable.png']
for key,name,expected in [('A','Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v4.zip','ee25baef91ec740b85dd3bb62b32583292d703f9261863bd534d0eb8241d1896'),('B','Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5.zip','16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351')]:
 d=B/'Extracted'/key;p=Path('F:/Downloads')/name;z=zipfile.ZipFile(p);m=json.loads((d/'Manifest.json').read_text(encoding='utf-8'));refs={x['path']:x for x in m['assets']} if 'assets' in m else m['files'];files=[]
 for f in sorted(d.rglob('*')):
  if not f.is_file():continue
  rel=f.relative_to(d).as_posix();rec={'path':rel,'sha256':sha(f),'bytes':f.stat().st_size}
  if rel in refs:rec['manifest_sha_match']=rec['sha256']==refs[rel]['sha256']
  files.append(rec)
 result={'zip':name,'zip_sha256':sha(p),'zip_bytes':p.stat().st_size,'expected_match':sha(p)==expected,'entries':len(z.infolist()),'crc_bad':z.testzip(),'manifest_sha_mismatches':[x['path'] for x in files if x.get('manifest_sha_match') is False],'files':files,'delta':[]}
 if key=='A':
  game=[x for x in refs if not x.startswith(('QA/','Preview/'))];result['game_count']=len(game);keep=[]
  for rel in game:
   if rel not in targets:keep.append({'path':rel,'previous_sha256':sha(OLD/key/rel),'sha256':sha(d/rel),'equal':sha(OLD/key/rel)==sha(d/rel)});continue
   a=np.array(Image.open(d/rel).convert('RGBA'));b=np.array(Image.open(OLD/key/rel).convert('RGBA'));mask=np.any(a!=b,axis=2);ys,xs=np.where(mask);lab,n=label(b[:,:,3]>0,np.ones((3,3)));counts=np.bincount(lab.ravel());counts[0]=0;largest=int(np.argmax(counts));main=lab==largest
   result['delta'].append({'path':rel,'mode':Image.open(d/rel).mode,'size':list(a.shape[:2][::-1]),'sha256':sha(d/rel),'changed_pixels':int(mask.sum()),'changed_bbox_exclusive':[int(xs.min()),int(ys.min()),int(xs.max()+1),int(ys.max()+1)],'changed_main_component_pixels':int((mask&main).sum()),'changed_pixels_new_alpha_nonzero':int((a[:,:,3][mask]>0).sum()),'unchanged_outside_diff':bool(np.array_equal(a[~mask],b[~mask]))})
  result['keep41']=keep;result['sha256sums_mismatches']=[];cnt=0
  for line in (d/'SHA256SUMS.txt').read_text().splitlines():
   h,rel=line.split(None,1);rel=rel.strip().lstrip('*');cnt+=1
   if sha(d/rel)!=h:result['sha256sums_mismatches'].append(rel)
  result['sha256sums_count']=cnt
 else:
  strip=Image.open(d/'Assets/Wolf_Run6_v5_Strip_3072x384.png').convert('RGBA');atlas=Image.open(d/'Assets/Wolf_Run6_v5_Atlas3x2_1536x768.png').convert('RGBA')
  for i in range(6):
   rel=f'Assets/Poses/Wolf_Run_{i:02}_v5.png';prev=OLD/'B'/f'Assets/Poses/Wolf_Run_{i:02}_v4.png';im=Image.open(d/rel).convert('RGBA');a=np.array(im);b=np.array(Image.open(prev).convert('RGBA'));mask=np.any(a!=b,axis=2);ys,xs=np.where(mask)
   result['delta'].append({'frame':i,'sha_equal':sha(d/rel)==sha(prev),'sha256':sha(d/rel),'previous_sha256':sha(prev),'changed_pixels':int(mask.sum()),'changed_bbox_exclusive':[int(xs.min()),int(ys.min()),int(xs.max()+1),int(ys.max()+1)] if len(xs) else None,'strip_equal':im.tobytes()==strip.crop((i*512,0,(i+1)*512,384)).tobytes(),'atlas_equal':im.tobytes()==atlas.crop(((i%3)*512,(i//3)*384,(i%3+1)*512,(i//3+1)*384)).tobytes()})
  result['animations']=[]
  for p in (d/'Preview').iterdir():
   if p.suffix.lower() not in ['.gif','.apng']:continue
   im=Image.open(p);times=[]
   for i in range(im.n_frames):im.seek(i);im.load();times.append(im.info.get('duration'))
   result['animations'].append({'path':p.name,'frames':im.n_frames,'durations_ms':times})
  # 174행 좌표CSV는 파일 존재·해시만으로 픽셀정합을 주장하지 않고 실제 변경점과 별도로 보존합니다.
 (E/f'{key}_Validation.json').write_text(json.dumps(result,indent=2),encoding='utf-8');print(key,result['expected_match'],result['manifest_sha_mismatches'],result['delta']);print('keep',sum(not x['equal'] for x in result.get('keep41',[])))
