from pathlib import Path
import subprocess,json,hashlib,re
root=Path('F:/study/codex/Project-Limitless');t=root/'Temp/F2BuildSettingsFinal20261103';w=t/'Worktree';name='Unity/Client/ProjectSettings/EditorBuildSettings.asset';a=json.loads((t/'ProtectionBefore.json').read_text(encoding='utf-8'))
sha=lambda b:hashlib.sha256(b).hexdigest()
run=lambda *args,cwd=root:subprocess.run(['git',*args],cwd=cwd,capture_output=True)
target=run('rev-parse','HEAD',cwd=w).stdout.decode().strip();assert run('diff','--name-only',a['head'],target).stdout.decode().splitlines()==[name]
assert run('rev-parse','HEAD').stdout.decode().strip()==a['head'] and not run('diff','--cached','--name-only').stdout.strip()
assert sha(run('status','--porcelain=v1','-uall','-z').stdout)==a['status_z_sha256']
assert all(sha((root/p).read_bytes())==h for p,h in a['user_files'].items())
assert all(sha((root/'Unity/Client'/p).read_bytes())==h for p,h in a['settings_sha256'].items())
assert (t/'QA/Evidence/status.txt').read_text()=='PASS' and not (t/'QA/Evidence/Errors.txt').read_text()
assert len((t/'QA/Evidence/checks.txt').read_text().splitlines())==26
assert sha((t/'QA/Client/ProjectSettings/EditorBuildSettings.asset').read_bytes())==a['candidate_sha256']
proof=json.loads((t/'GitFullPreflight.json').read_text());assert proof['working_apply_exit']==proof['cached_apply_exit']==proof['ff_exit']==0 and proof['bytes_exact'] and proof['user_diff_payload_preserved'] and proof['index_clean']
wp=str(t/'WorkingF2WithUserContext.patch');ip=str(t/'ApprovedF2Only.patch')
assert run('-c','core.autocrlf=false','apply','--check',wp).returncode==0
assert run('apply','--cached','--check',ip).returncode==0
payload=lambda diff:[x for x in diff.splitlines() if x[:1] in [b'+',b'-'] and not x.startswith((b'+++',b'---'))]
before_payload=payload(run('diff','--',name).stdout);working_applied=False;cached_applied=False
try:
    r=run('-c','core.autocrlf=false','apply',wp);assert r.returncode==0,r.stderr.decode();working_applied=True
    assert sha((root/name).read_bytes())==a['candidate_sha256']
    r=run('apply','--cached',ip);assert r.returncode==0,r.stderr.decode();cached_applied=True
    assert run('rev-parse',':'+name).stdout==run('rev-parse',target+':'+name).stdout
    assert payload(run('diff','--',name).stdout)==before_payload
    r=run('merge','--ff-only','--no-stat',target);assert r.returncode==0,r.stderr.decode()
except Exception:
    # 실패 시 승인한 F2 hunk만 되돌립니다. 사용자 원본/전체 index를 백업으로 덮어쓰지 않습니다.
    if run('rev-parse','HEAD').stdout.decode().strip()==a['head']:
        if cached_applied:run('apply','--cached','--reverse',ip)
        if working_applied and sha((root/name).read_bytes())==a['candidate_sha256']:run('-c','core.autocrlf=false','apply','--reverse',wp)
    raise
after=(root/name).read_bytes();records=re.findall(rb'  - enabled: ([01])\r?\n    path: ([^\r\n]+)\r?\n    guid: ([^\r\n]+)',after);records=[tuple(x.decode() for x in r) for r in records]
user_bad=[p for p,h in a['user_files'].items() if p!=name and sha((root/p).read_bytes())!=h]
result={'head':run('rev-parse','HEAD').stdout.decode().strip(),'branch':run('branch','--show-current').stdout.decode().strip(),'product_commit':target,'f2_registered':len(records)==26 and records[-1][1]==a['scene_path'] and records[-1][2]==a['scene_guid'],'original25_exact':records[:25]==[tuple(x) for x in a['existing_scenes']],'config_suffix_exact':after.split(b'  m_configObjects:')[1]==(t/'OriginalBuildSettings.asset').read_bytes().split(b'  m_configObjects:')[1],'working_bytes_exact_candidate':sha(after)==a['candidate_sha256'],'other_user_files_checked':len(a['user_files'])-1,'other_user_files_changed':user_bad,'user_unstaged_hunk_payload_exact':payload(run('diff','--',name).stdout)==before_payload,'status_z_exact':sha(run('status','--porcelain=v1','-uall','-z').stdout)==a['status_z_sha256'],'staged_after':run('diff','--cached','--name-only').stdout.decode().strip(),'version_packages_unchanged':all(sha((root/'Unity/Client'/p).read_bytes())==h for p,h in a['settings_sha256'].items()),'unity_auto_serialization_changed':False,'qa_pass':26,'qa_fail':0,'original_editor_executed':False,'qa_scope':'ISOLATED_CLONE_WITH_IDENTICAL_CANDIDATE_BUILDSETTINGS','original_local_sync':'DONE','remote_push':False}
(t/'AppliedProof.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8');print(json.dumps(result,ensure_ascii=False))
assert result['f2_registered'] and result['original25_exact'] and result['config_suffix_exact'] and result['working_bytes_exact_candidate'] and not user_bad and result['status_z_exact'] and result['user_unstaged_hunk_payload_exact'] and not result['staged_after'] and result['version_packages_unchanged']
