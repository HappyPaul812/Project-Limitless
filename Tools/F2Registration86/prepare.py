from pathlib import Path
import subprocess,re,json,hashlib,shutil
root=Path('F:/study/codex/Project-Limitless');task=root/'Temp/F2BuildSettingsFinal20261103';wt=task/'Worktree';q=task/'QA/Client';name='Unity/Client/ProjectSettings/EditorBuildSettings.asset'
git=lambda *args,cwd=root:subprocess.check_output(['git',*args],cwd=cwd)
sha=lambda b:hashlib.sha256(b).hexdigest()
original=(root/name).read_bytes();base=git('show','HEAD:'+name);scene='Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity';meta=root/'Unity/Client/Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity.meta'
guid=re.search(r'^guid: ([0-9a-f]{32})',meta.read_text(),re.M).group(1);assert guid=='bcd0e6f38e69dc141b741fe5247855cf'
assert scene.encode() not in original and scene.encode() not in base
def insert(raw):
    nl=b'\r\n' if b'\r\n' in raw else b'\n';marker=b'  m_configObjects:'+nl
    assert raw.count(marker)==1
    extra=nl.join([b'  - enabled: 1',b'    path: '+scene.encode(),b'    guid: '+guid.encode()])+nl
    return raw.replace(marker,extra+marker),extra
candidate,extra=insert(original);stage,_=insert(git('show','HEAD:'+name,cwd=wt))
assert candidate.replace(extra,b'',1)==original
(wt/name).write_bytes(stage)
(task/'OriginalBuildSettings.asset').write_bytes(original);(task/'CandidateWithUserConfig.asset').write_bytes(candidate)
(task/'ApprovedF2Only.patch').write_bytes(git('diff','--binary','--',name,cwd=wt))
def records(raw):return re.findall(rb'  - enabled: ([01])\r?\n    path: ([^\r\n]+)\r?\n    guid: ([^\r\n]+)',raw)
before=records(original);after=records(candidate);assert len(before)==25 and len(after)==26 and before==after[:25] and len({x[1] for x in after})==26 and after[-1][2].decode()==guid
backup=json.loads((root/'Temp/LocalSync20261103/BackupManifest.json').read_text(encoding='utf-8'))
user={x['path']:sha((root/x['path']).read_bytes()) for x in backup['dirty_nonprivate'] if x['exists']}
assert len(user)==847
manifest={'head':git('rev-parse','HEAD').decode().strip(),'status_z_sha256':sha(git('status','--porcelain=v1','-uall','-z')),'index_staged':git('diff','--cached','--name-only').decode().strip(),'original_sha256':sha(original),'candidate_sha256':sha(candidate),'scene_path':scene,'scene_guid':guid,'existing_scenes':[tuple(y.decode() for y in x) for x in before],'candidate_scenes':[tuple(y.decode() for y in x) for x in after],'user_files':user,'settings_sha256':{p:sha((root/'Unity/Client'/p).read_bytes()) for p in ['ProjectSettings/ProjectVersion.txt','Packages/manifest.json','Packages/packages-lock.json']},'user_config_suffix_exact':original.split(b'  m_configObjects:')[1]==candidate.split(b'  m_configObjects:')[1]}
assert not manifest['index_staged'] and manifest['user_config_suffix_exact']
(task/'ProtectionBefore.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2),encoding='utf-8');shutil.copy2(root/'.git/index',task/'OriginalIndexBefore')
# 원본과 똑같은 user app-ui 차이를 가진 작은 Git fixture에서 선택적 F2 hunk staging/ff를 먼저 검증합니다.
fixture=task/'GitPreflightV2';fixture.mkdir();run=lambda *a,input=None:subprocess.run(['git',*a],cwd=fixture,input=input,capture_output=True)
assert run('init','--initial-branch=fixture','-q').returncode==0
run('config','user.name','Local QA');run('config','user.email','local-qa@invalid');run('config','core.autocrlf','true')
f=fixture/name;f.parent.mkdir(parents=True);f.write_bytes(base);run('add','--',name);run('commit','-qm','Chore: fixture baseline');old=run('rev-parse','HEAD').stdout.strip().decode();f.write_bytes(insert(base)[0]);run('commit','-qam','Chore: fixture F2 only');new=run('rev-parse','HEAD').stdout.strip().decode();assert run('switch','--detach',old).returncode==0
f.write_bytes(original);userdiff=run('diff','--',name).stdout;f.write_bytes(candidate);applied=run('apply','--cached',str(task/'ApprovedF2Only.patch'));merged=run('merge','--ff-only',new)
payload=lambda diff:[line for line in diff.splitlines() if line[:1] in [b'+',b'-'] and not line.startswith((b'+++',b'---'))]
proof={'cached_patch_exit':applied.returncode,'ff_exit':merged.returncode,'working_bytes_exact':f.read_bytes()==candidate,'user_unstaged_hunk_payload_exact':payload(run('diff','--',name).stdout)==payload(userdiff),'cached_diff_empty':run('diff','--cached','--name-only').stdout.strip()==b''}
(task/'GitPreflight.json').write_text(json.dumps(proof,indent=2),encoding='utf-8');assert all([applied.returncode==0,merged.returncode==0,proof['working_bytes_exact'],proof['user_unstaged_hunk_payload_exact'],proof['cached_diff_empty']])
if not (q/'Assets').exists():shutil.copytree(root/'Unity/Client/Assets',q/'Assets',ignore=shutil.ignore_patterns('*.wav','*.mp3','*.ogg','*.zip','*.psd','*.blend','UserData','Save','Saves'))
for folder in ['ProjectSettings','Packages']:
    if not (q/folder).exists():shutil.copytree(root/'Unity/Client'/folder,q/folder)
for p in (q/'Assets').rglob('*.cs'):
    if '/Editor/' in p.as_posix():p.rename(p.with_suffix('.cs.disabled'))
(q/'ProjectSettings/EditorBuildSettings.asset').write_bytes(candidate)
for n,d in [('F2RegistrationBatch.cs','Assets/Editor'),('F2RegistrationProbe.cs','Assets/QAF2Registration')]:
    dest=q/d/n;dest.parent.mkdir(parents=True,exist_ok=True);shutil.copy2(wt/'Tools/F2Registration86'/n,dest)
print(json.dumps({'old_scene_count':len(before),'new_scene_count':len(after),'existing25_exact':before==after[:25],'config_suffix_exact':manifest['user_config_suffix_exact'],'git_preflight':proof,'source_file_sha256':manifest['original_sha256'],'candidate_sha256':manifest['candidate_sha256']},ensure_ascii=False))
