from pathlib import Path
import hashlib,json,subprocess
root=Path('F:/study/codex/Project-Limitless');task=root/'Temp/LocalSync20261103';wt=task/'Worktree';a=json.loads((task/'BackupManifest.json').read_text(encoding='utf-8'))
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
git=lambda *args,cwd=root:subprocess.check_output(['git',*args],cwd=cwd)
protected_changed_all=[e['path'] for e in a['protected_tracked'] if not (root/e['path']).is_file() or sha(root/e['path'])!=e['sha256']]
approved_paths=set(git('diff','--name-only','-z',a['head'],'HEAD').decode().split('\0'))
expected_document_changes=[p for p in protected_changed_all if p in approved_paths and p.endswith('.md')]
protected_changed=[p for p in protected_changed_all if p not in expected_document_changes]
user_changed=[e['path'] for e in a['dirty_nonprivate'] if not (root/e['path']).is_file() or sha(root/e['path'])!=e['sha256']]
before=set((task/'OriginalStatusBefore.bin').read_bytes().split(b'\0'));after_raw=git('status','--porcelain=v1','-uall','-z');after=set(after_raw.split(b'\0'));expected_removed={b' M Unity/Client/ProjectSettings/ProjectVersion.txt'}
refs_after=set(git('for-each-ref','--format=%(refname:short) %(objectname)','refs/heads').decode().splitlines());preserved=[line for line in a['branches'] if not line.startswith(('main ','codex/local-sync-approved-20261103 '))];lost=[line for line in preserved if line not in refs_after]
spider=json.loads((task/'SpiderApprovedAssetManifest.json').read_text(encoding='utf-8'));art=[{'path':e['asset'],'sha256':e['sha256'],'same':sha(root/e['asset'])==e['sha256']} for e in spider]
for folder in ['Resources/Main24FrostOwlCandidate','Resources/Main24F2Candidate']:
    for p in (wt/'Unity/Client/Assets/_Project'/folder).rglob('*.png'):
        rel=p.relative_to(wt);art.append({'path':str(rel),'sha256':sha(p),'same':sha(root/rel)==sha(p)})
build=(root/'Unity/Client/ProjectSettings/EditorBuildSettings.asset').read_text(encoding='utf-8')
result={'head':git('rev-parse','HEAD').decode().strip(),'branch':git('branch','--show-current').decode().strip(),'protected_tracked_checked':len(a['protected_tracked']),'protected_changed':protected_changed,'backup_user_files_checked':len(a['dirty_nonprivate']),'user_changed':user_changed,'dirty_before_count':len([x for x in before if x]),'dirty_after_count':len([x for x in after if x]),'only_status_change_approved_version_recorded':before-after==expected_removed and not after-before,'index_staged_after':git('diff','--cached','--name-only').decode().strip(),'preserved_branch_count':len(preserved),'lost_branch_refs':lost,'approved_and_placeholder_png':art,'original_buildsettings_sha_unchanged':sha(root/'Unity/Client/ProjectSettings/EditorBuildSettings.asset')==a['buildsettings_worktree_sha256'],'original_f2_scene_registered': 'World_NorthF2_CANDIDATE.unity' in build,'original_app_ui_preserved':'com.unity.dt.app-ui:' in build,'version_worktree':(root/'Unity/Client/ProjectSettings/ProjectVersion.txt').read_text().strip(),'version_head':git('show','HEAD:Unity/Client/ProjectSettings/ProjectVersion.txt').decode().strip(),'remote_push':False,'overall_original_sync':'HOLD_F2_BUILDSETTINGS_CONNECTION','approved_file_sync':'DONE'}
result['approved_document_changes']=expected_document_changes
(task/'SyncProof.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8');print(json.dumps({k:v for k,v in result.items() if k!='approved_and_placeholder_png'},ensure_ascii=False))
assert not protected_changed and not user_changed and not lost and result['only_status_change_approved_version_recorded'] and all(e['same'] for e in art) and result['original_buildsettings_sha_unchanged'] and not result['index_staged_after'] and result['version_head']==result['version_worktree']
