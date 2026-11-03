from pathlib import Path
import json,hashlib,shutil,base64
root=Path('F:/study/codex/Project-Limitless');task=root/'Temp/LocalSync20261103';wt=task/'Worktree';e=task/'QA/Evidence';out=wt/'문서/00_프로젝트/Evidence/LocalSync86_20261103';out.mkdir(parents=True,exist_ok=True)
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
checks=(e/'checks.txt').read_text().splitlines();assert len(checks)==44 and all(s.startswith('PASS ') for s in checks) and (e/'Errors.txt').read_text()==''
for name in ['StaticIntegrationQA.json','ResolvedSceneExternalReferences.json','SpiderApprovedAssetManifest.json']:
    shutil.copy2(task/name,out/name)
for p in e.iterdir():
    if p.is_file():shutil.copy2(p,out/p.name)
captures=[{'name':p.name,'sha256':sha(p),'bytes':p.stat().st_size} for p in sorted(e.glob('*.png'))]
(out/'CaptureSHA256.json').write_text(json.dumps(captures,indent=2),encoding='utf-8')
review=task/'Review';review.mkdir(exist_ok=True)
html='<!doctype html><html lang="ko"><meta charset="utf-8"><title>Issue86 실제 통합 QA</title><style>body{font:16px system-ui;background:#14202e;color:#eee;margin:24px}img{max-width:100%;height:auto}code{display:block;overflow-wrap:anywhere}</style><h1>Issue #86 통합 QA — OFFLINE 증거 열람판</h1><p>실제 Unity6000.6.5f1 / revision3ff58d469c8a Camera RenderTexture. 합성 데이터·실제 게임 UI이며 OS GameView 아님. Unity44 PASS /0 FAIL. 원본 F2 BuildSettings 연결은 사용자 app-ui 설정 보호로 HOLD,26Scene 제안은 QA 복제본에만 사용했습니다.</p>'
for p in sorted(e.glob('*.png')):html+='<h2>'+p.name+'</h2><code>'+sha(p)+'</code><img src="data:image/png;base64,'+base64.b64encode(p.read_bytes()).decode()+'">'
(review/'index.html').write_text(html,encoding='utf-8');summary={'unity':'6000.6.5f1','revision':'3ff58d469c8a','pass':44,'fail':0,'runtime_errors':0,'offline_viewer':str(review/'index.html'),'viewer_sha256':sha(review/'index.html'),'capture_count':len(captures),'qa_log':str(task/'unity-fixed.log'),'qa_log_sha256':sha(task/'unity-fixed.log'),'official_buildsettings':'HOLD_NOT_MODIFIED','synthetic_save_only':True}
(out/'QASummary.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2),encoding='utf-8')
tools=wt/'Tools/LocalSync86'
for name in ['audit_backup.py','stage_assets.py','prepare_qa.py','static_qa.py','publish_qa.py']:
    shutil.copy2(task/name,tools/name)
print(json.dumps(summary,ensure_ascii=False))
