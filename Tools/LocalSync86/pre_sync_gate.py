from pathlib import Path
import hashlib,json,subprocess,shutil
root=Path('F:/study/codex/Project-Limitless');task=root/'Temp/LocalSync20261103';wt=task/'Worktree';a=json.loads((task/'BackupManifest.json').read_text(encoding='utf-8'))
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
git=lambda *args,cwd=root:subprocess.check_output(['git',*args],cwd=cwd)
protected_changed=[e['path'] for e in a['protected_tracked'] if not (root/e['path']).is_file() or sha(root/e['path'])!=e['sha256']]
dirty_changed=[e['path'] for e in a['dirty_nonprivate'] if not (root/e['path']).is_file() or sha(root/e['path'])!=e['sha256']]
changes=git('diff','--name-only','-z',a['head'],'HEAD',cwd=wt).decode().split('\0');overlap=sorted(set(changes)&{e['path'] for e in a['dirty_nonprivate']})
version='Unity/Client/ProjectSettings/ProjectVersion.txt';wanted=git('show','HEAD:'+version,cwd=wt).decode().strip();version_exact=(root/version).read_text().strip()==wanted
original_index=root/'.git/index';shutil.copy2(original_index,task/'Backup/OriginalIndexBeforeSync')
result={'original_head':git('rev-parse','HEAD').decode().strip(),'target':git('rev-parse','HEAD',cwd=wt).decode().strip(),'protected_tracked_checked':len(a['protected_tracked']),'protected_changed':protected_changed,'backup_user_files_checked':len(a['dirty_nonprivate']),'user_changed':dirty_changed,'dirty_overlap':overlap,'version_semantics_exact':version_exact,'status_unchanged':hashlib.sha256(git('status','--porcelain=v1','-uall','-z')).hexdigest()==a['status_sha256'],'index_staged_before':git('diff','--cached','--name-only').decode().strip(),'buildsettings_user_file_unchanged':sha(root/'Unity/Client/ProjectSettings/EditorBuildSettings.asset')==a['buildsettings_worktree_sha256']}
(task/'PreSyncGate.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8');print(json.dumps(result,ensure_ascii=False))
assert result['original_head']==a['head'] and not protected_changed and not dirty_changed and overlap==[version] and version_exact and result['status_unchanged'] and not result['index_staged_before'] and result['buildsettings_user_file_unchanged']
