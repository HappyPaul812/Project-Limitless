# -*- coding: utf-8 -*-
from pathlib import Path
import hashlib,json,subprocess,shutil,zipfile,datetime
root=Path(r'F:/study/codex/Project-Limitless');base=root/'Temp/WolfOriginalApply20261021';wt=root/'Temp/WolfRunV5Apply20261021/Worktree'
base.mkdir(parents=True,exist_ok=True);e=wt/'문서/00_프로젝트/Evidence/WolfOriginalApply20261021';e.mkdir(parents=True,exist_ok=True)
def git(*args):return subprocess.check_output(['git',*args],cwd=root)
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
def snapshot():
 names=set(git('ls-files','-z','--cached','--others','--exclude-standard').decode('utf8').split('\0'))
 for folder in ['Unity/Client/UserData','UserData']:
  names.update(p.relative_to(root).as_posix() for p in (root/folder).rglob('*') if p.is_file())
 return {name:sha(root/name) for name in sorted(names) if name and not name.startswith('Temp/') and (root/name).is_file()}
assert subprocess.check_output(['git','rev-parse','HEAD'],cwd=wt).decode().strip()=='ab1b47ff9d46f4ed5883b85b73e87ca2036c83e6'
paths=subprocess.check_output(['git','diff','--name-only','e4e9891f','HEAD','--','Unity'],cwd=wt).decode().splitlines();assert len(paths)==6
assert sha(Path('F:/Downloads/Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5.zip'))=='16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351'
before=snapshot();head=git('rev-parse','HEAD').decode().strip();index=sha(root/'.git/index');status=git('status','--porcelain=v1','-z')
(base/'GitBefore.bin').write_bytes(status);(base/'ProtectionBefore.json').write_text(json.dumps(before,ensure_ascii=False,indent=2),encoding='utf8')
rows=[];conflicts=[]
for name in paths:
 dst=root/name;src=wt/name;existed=dst.exists()
 if existed:
  if git('diff','HEAD','--',name).strip():conflicts.append(name)
 else:
  try:git('cat-file','-e','HEAD:'+name);conflicts.append(name)
  except subprocess.CalledProcessError:pass
 rows.append({'path':name,'existed_before':existed,'before_sha256':sha(dst) if existed else None,'approved_sha256':sha(src)})
assert not conflicts,conflicts
for r in rows:
 if r['existed_before']:
  backup=base/'Backup'/r['path'];backup.parent.mkdir(parents=True,exist_ok=True);shutil.copy2(root/r['path'],backup);assert sha(backup)==r['before_sha256']
 (base/'BackupManifest.json').write_text(json.dumps(rows,indent=2),encoding='utf8')
# 복사 직전 다시 읽어 이후 사용자 수정이 들어온 경우 전체 반영을 중단합니다.
assert all((sha(root/r['path'])==r['before_sha256']) if r['existed_before'] else not (root/r['path']).exists() for r in rows)
for r in rows:shutil.copy2(wt/r['path'],root/r['path'])
after=snapshot();changed=[n for n,h in before.items() if after.get(n)!=h];new=[n for n in after if n not in before]
assert set(changed)=={r['path'] for r in rows if r['existed_before']}
assert set(new)=={r['path'] for r in rows if not r['existed_before']}
for r in rows:r['readback_sha256']=sha(root/r['path']);assert r['readback_sha256']==r['approved_sha256']
result={'status':'ORIGINAL_DISK_APPLIED','source_commit':'ab1b47ff9d46f4ed5883b85b73e87ca2036c83e6','baseline_files':len(before),'rows':rows,'changed':changed,'new':new,'unauthorized_changes':[],'root_head_unchanged':git('rev-parse','HEAD').decode().strip()==head,'root_index_unchanged':sha(root/'.git/index')==index,'backup_directory':str(base/'Backup'),'reused_runtime_pass':106,'runtime_repeated':0,'original_editor_actions':0,'natural_import':'NOT_VERIFIED','handplay':'NOT_VERIFIED','push_pr_merge':0}
assert result['root_head_unchanged'] and result['root_index_unchanged']
(e/'OriginalApplyResult.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
(base/'ProtectionAfterA.json').write_text(json.dumps(after,ensure_ascii=False,indent=2),encoding='utf8')
shutil.copy2(base/'BackupManifest.json',e/'BackupManifest.json');shutil.copy2(Path(__file__),e/'apply_original.py')
print(json.dumps(result,ensure_ascii=False,indent=2))
