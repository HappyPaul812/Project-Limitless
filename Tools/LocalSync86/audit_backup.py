from pathlib import Path
import subprocess,json,hashlib,shutil
root=Path('F:/study/codex/Project-Limitless');task=root/'Temp/LocalSync20261103';task.mkdir(exist_ok=True)
def git(*args):return subprocess.check_output(['git',*args],cwd=root)
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
def private(p):
    parts={s.casefold() for s in Path(p).parts}
    return bool(parts & {'userdata','saves','save','savedata','usersettings','library','temp','.git','.codex','.agents','.aws','syntheticsaves','syntheticsettings'}) or Path(p).suffix.casefold() in {'.sav','.save','.key','.pem'} or Path(p).name.casefold().startswith('.env')
status=git('status','--porcelain=v1','-uall','-z');(task/'OriginalStatusBefore.bin').write_bytes(status)
entries=status.split(b'\0');dirty=[];skip=[];i=0
while i<len(entries):
    e=entries[i];i+=1
    if not e:continue
    state=e[:2].decode();p=e[3:].decode('utf-8');item={'status':state,'path':p}
    if state[0] in 'RC' or state[1] in 'RC':item['old_path']=entries[i].decode('utf-8');i+=1
    if private(p):skip.append({'path':p,'reason':'personal_or_excluded_path_metadata_only'});continue
    f=root/p;item['exists']=f.is_file()
    if f.is_file():
        item.update(bytes=f.stat().st_size,sha256=sha(f));out=task/'Backup/UserChanges'/p;out.parent.mkdir(parents=True,exist_ok=True);shutil.copy2(f,out);assert sha(out)==item['sha256']
    dirty.append(item)
head=git('rev-parse','HEAD').decode().strip();branches=git('for-each-ref','--format=%(refname:short) %(objectname)','refs/heads').decode().splitlines()
changed=git('diff','--name-only','-z',head,'47f479cce4fd282ac4c9e49ecfb75e2b1a25e461','--','Unity/Client').decode().split('\0');changed=[p for p in changed if p]
tracked=[p for p in git('ls-files','-z').decode().split('\0') if p and not private(p)]
# 승인 변경 경로 외의 기존 tracked 파일도 해시로 지킵니다. 개인 Save/UserData/Temp는 열지 않습니다.
protected=[{'path':p,'sha256':sha(root/p)} for p in tracked if (root/p).is_file() and p not in changed]
result={'head':head,'branch':git('branch','--show-current').decode().strip(),'status_sha256':hashlib.sha256(status).hexdigest(),'dirty_nonprivate':dirty,'skipped_metadata_only':skip,'branches':branches,'approved_candidate_unity_changes':changed,'dirty_overlap':sorted({e['path'] for e in dirty}&set(changed)),'protected_tracked':protected,'version_worktree':(root/'Unity/Client/ProjectSettings/ProjectVersion.txt').read_text(),'buildsettings_worktree_sha256':sha(root/'Unity/Client/ProjectSettings/EditorBuildSettings.asset')}
(task/'BackupManifest.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps({'head':head,'backup_entries':len(dirty),'backup_files':sum(e['exists'] for e in dirty),'skip_count':len(skip),'protected_tracked':len(protected),'candidate_unity_changes':len(changed),'dirty_overlap':result['dirty_overlap'],'version_worktree':result['version_worktree']},ensure_ascii=False))
