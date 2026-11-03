from pathlib import Path
import json,hashlib,shutil,base64
root=Path('F:/study/codex/Project-Limitless');t=root/'Temp/F2BuildSettingsFinal20261103';w=t/'Worktree';out=w/'문서/00_프로젝트/Evidence/F2BuildSettingsFinal86_20261103';out.mkdir(parents=True,exist_ok=True);e=t/'QA/Evidence';a=json.loads((t/'ProtectionBefore.json').read_text(encoding='utf-8'));proof=json.loads((t/'AppliedProof.json').read_text(encoding='utf-8'));sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
assert (e/'status.txt').read_text()=='PASS' and not (e/'Errors.txt').read_text() and len((e/'checks.txt').read_text().splitlines())==26
assert sha(t/'QA/Client/ProjectSettings/EditorBuildSettings.asset')==a['candidate_sha256']
for n in ['AppliedProof.json','GitFullPreflight.json']:shutil.copy2(t/n,out/n)
for p in e.iterdir():
    if p.is_file():shutil.copy2(p,out/p.name)
manifest={k:a[k] for k in ['head','original_sha256','candidate_sha256','scene_path','scene_guid','existing_scenes','candidate_scenes','user_config_suffix_exact']}
manifest.update(qa_buildsettings_after_sha256=sha(t/'QA/Client/ProjectSettings/EditorBuildSettings.asset'),unity_auto_serialization_changed=False,unity='6000.6.5f1',revision='3ff58d469c8a',pass_count=26,fail_count=0,other_user_files_preserved=846,buildsettings_user_change_preserved=True)
(out/'SceneAndSettingsManifest.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2),encoding='utf-8')
review=t/'Review';review.mkdir(exist_ok=True)
html='<!doctype html><html lang="ko"><meta charset="utf-8"><title>F2 Scene 등록 최종 검증</title><style>body{font:16px system-ui;background:#14202e;color:#eee;margin:24px}img{max-width:100%}code{display:block;overflow-wrap:anywhere}</style><h1>F2 Scene 1건 등록 — OFFLINE 증거 열람판</h1><p>실제 Unity6000.6.5f1/rev3ff58d469c8a Camera RenderTexture. 격리26 PASS/0 FAIL. 기존25 Scene·app-ui·사용자미커밋변경 보존 후 원본에 등록 반영. 원본 Editor 손플레이는 수행하지 않았습니다.</p>'
capture=[]
for p in sorted(e.glob('*.png')):
    capture.append({'file':p.name,'sha256':sha(p)});html+='<h2>'+p.name+'</h2><code>'+sha(p)+'</code><img src="data:image/png;base64,'+base64.b64encode(p.read_bytes()).decode()+'">'
(review/'index.html').write_text(html,encoding='utf-8');(out/'Captures.json').write_text(json.dumps(capture,indent=2),encoding='utf-8')
summary={'viewer':str(review/'index.html'),'viewer_sha256':sha(review/'index.html'),'unity_log':str(t/'unity.log'),'unity_log_sha256':sha(t/'unity.log'),'checks':26,'fail':0,'original_editor_executed':False,'original_local_sync':'DONE'}
(out/'QASummary.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2),encoding='utf-8')
for n in ['prepare.py','git_full_preflight.py','apply_and_prove.py','publish.py']:shutil.copy2(t/n,w/'Tools/F2Registration86'/n)
print(json.dumps(summary,ensure_ascii=False))
