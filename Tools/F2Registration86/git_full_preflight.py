from pathlib import Path
import subprocess,json
root=Path('F:/study/codex/Project-Limitless');t=root/'Temp/F2BuildSettingsFinal20261103';f=t/'GitFullPreflightV2';f.mkdir();name='Unity/Client/ProjectSettings/EditorBuildSettings.asset'
run=lambda *a:subprocess.run(['git',*a],cwd=f,capture_output=True)
assert run('init','--initial-branch=fixture','-q').returncode==0
run('config','user.name','Local QA');run('config','user.email','local-qa@invalid');run('config','core.autocrlf','true')
p=f/name;p.parent.mkdir(parents=True);base=subprocess.check_output(['git','show','HEAD:'+name],cwd=root);p.write_bytes(base);run('add','--',name);run('commit','-qm','Chore: fixture baseline');old=run('rev-parse','HEAD').stdout.decode().strip()
assert run('apply','--cached',str(t/'ApprovedF2Only.patch')).returncode==0
assert run('-c','core.autocrlf=false','apply',str(t/'ApprovedF2Only.patch')).returncode==0
run('commit','-qm','Chore: approved F2 only');target=run('rev-parse','HEAD').stdout.decode().strip();assert run('switch','--detach',old).returncode==0
p.write_bytes((t/'OriginalBuildSettings.asset').read_bytes());before=run('diff','--',name).stdout
applied=run('-c','core.autocrlf=false','apply',str(t/'WorkingF2WithUserContext.patch'));cached=run('apply','--cached',str(t/'ApprovedF2Only.patch'));merged=run('merge','--ff-only',target)
payload=lambda diff:[line for line in diff.splitlines() if line[:1] in [b'+',b'-'] and not line.startswith((b'+++',b'---'))]
proof={'working_apply_exit':applied.returncode,'cached_apply_exit':cached.returncode,'ff_exit':merged.returncode,'bytes_exact':p.read_bytes()==(t/'CandidateWithUserConfig.asset').read_bytes(),'user_diff_payload_preserved':payload(before)==payload(run('diff','--',name).stdout),'index_clean':not run('diff','--cached','--name-only').stdout.strip()}
(t/'GitFullPreflight.json').write_text(json.dumps(proof,indent=2),encoding='utf-8');print(json.dumps(proof));assert all([proof['working_apply_exit']==0,proof['cached_apply_exit']==0,proof['ff_exit']==0,proof['bytes_exact'],proof['user_diff_payload_preserved'],proof['index_clean']])
