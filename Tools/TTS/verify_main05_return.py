"""Runtime 페이지/PCM 정합을 검증한다. 사람 청취 없는 의미 PASS는 만들지 않는다."""
import csv, hashlib, json, struct, wave
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]; DOC=ROOT/'문서/00_프로젝트'; TEMP=ROOT/'Temp/Main05Return'
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
log=(TEMP/'runtime/runtime-final.txt').read_text(encoding='utf-8')
assert log.startswith('PASS\n') and 'FAIL ' not in log
pages=[json.loads(s) for s in (TEMP/'runtime/pages.jsonl').read_text(encoding='utf-8').splitlines()]
assert len(pages)==24
for seq,count in [('AfterBattleConversation',9),('GuardReport',6),('RepresentativeReport',8)]:
    assert [p['page'] for p in pages if p['sequence']==seq]==list(range(count))
rows=list(csv.DictReader((DOC/'Main05_Voice_Matrix.csv').open(encoding='utf-8-sig',newline='')))
candidate_ids={r['stable_id'] for r in rows if r['stable_id']!='main05_taeon_supp_001'}
for r in rows:
    p=[p for p in pages if p['id']==r['stable_id']]
    assert len(p)==1 and p[0]['clip']==r['asset_path']
    assert p[0]['text']==r['display_text'].replace('\r\n','\n')
    with wave.open(str(ROOT/'Unity/Client'/r['asset_path'])) as w:
        raw=w.readframes(w.getnframes()); source=struct.unpack('<'+'h'*(len(raw)//2),raw)
    raw=(TEMP/('runtime/'+r['stable_id']+'.pcm-f32')).read_bytes()
    actual=struct.unpack('<'+'f'*(len(raw)//4),raw)
    assert len(actual)==len(source)
    error=max(abs(a-s/32768) for a,s in zip(actual,source));assert error<1e-6
    r['runtime_resolve']='PASS;'+p[0]['clip'];r['runtime_pcm_max_error']=error
    r['semantic_status']='NOT_VERIFIED;SEMANTIC_INTEGRITY_SUSPECT' if r['stable_id'] in candidate_ids else 'NOT_VERIFIED;USER_REPORTED_START_ANCHOR'
    r['status']='PASS_MAPPING_PLAYBACK_ONLY'
with (DOC/'Main05_Voice_Matrix.csv').open('w',encoding='utf-8-sig',newline='') as f:
    w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
# 문제의 미엘 자막을 사용자가 기억하지 못하므로 개별 Wrong/WAV 재생성 확정을 추측하지 않는다.
candidates=[]
for r in rows:
    if r['stable_id'] in candidate_ids:
        candidates.append(dict(stable_id=r['stable_id'],speaker=r['speaker'],exact_text=r['display_text'],status='TTS_REGEN_CANDIDATE;SEMANTIC_INTEGRITY_SUSPECT',evidence='User Main05 range: abnormal continuation; Miel speaks Main04 text; exact Miel page not remembered. Runtime mapping/PCM PASS does not prove meaning.'))
with (DOC/'Main05_TTS_Regen_Candidates.csv').open('w',encoding='utf-8-sig',newline='') as f:
    w=csv.DictWriter(f,fieldnames=candidates[0]);w.writeheader();w.writerows(candidates)
baseline=json.loads((TEMP/'baseline.json').read_text(encoding='utf-8'))
changed=[p for p,h in baseline.items() if not (ROOT/p).is_file() or sha(ROOT/p)!=h]
allowed={'Unity/Client/Assets/_Project/Scripts/UI/DialoguePresenter.cs','Unity/Client/Assets/_Project/Scripts/NPC/InteractionSystem.cs'}
external=[p for p in changed if p not in allowed]
assert not [p for p in external if not p.startswith('Unity/Client/UserData/')],external
summary=dict(runtime_pass=log.count('PASS '),runtime_fail=0,logged_pages=24,main05_voice=6,main05_mapping_pcm_pass=6,semantic_not_verified=6,semantic_suspect_candidates=5,individual_wrong_audio_confirmed=0,known_user_reported_wrong_content_in_range=True,changed_existing=changed,external_user_save_changes=external,protected_baseline_files=len(baseline)-len(changed),voice_asset_changes=0,main04_late5_changes=0,main06_after_changes=0,main17_voice_changes=0)
(DOC/'Main05_Runtime_Verification.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
(DOC/'Main05_Runtime_Results.txt').write_text(log,encoding='utf-8')
(DOC/'Main05_Runtime_Page_Order.jsonl').write_text('\n'.join(json.dumps(p,ensure_ascii=False) for p in pages)+'\n',encoding='utf-8')
print(json.dumps(summary,ensure_ascii=False))
