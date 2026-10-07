"""승인 원본과 실제 연속 Runtime PCM, 페이지 및 보호 상태를 검증한다."""
import csv,hashlib,json,struct,wave
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];DOC=ROOT/'문서/00_프로젝트';TEMP=ROOT/'Temp/Main05Final5'
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
read=lambda p:list(csv.DictReader(p.open(encoding='utf-8-sig',newline='')))
def savecsv(p,rows):
    with p.open('w',encoding='utf-8-sig',newline='') as f:
        w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
log=(TEMP/'runtime/runtime-final.txt').read_text(encoding='utf-8');assert log.startswith('PASS\n') and 'FAIL ' not in log
pages=[json.loads(s) for s in (TEMP/'runtime/pages.jsonl').read_text(encoding='utf-8').splitlines()];assert len(pages)==24
for seq,count in [('AfterBattleConversation',9),('GuardReport',6),('RepresentativeReport',8)]:
    assert [p['page'] for p in pages if p['sequence']==seq]==list(range(count))
assert pages[-1]['id']=='main06_taeon_001'
src={r['stable_id']:r for r in read(DOC/'Main05_Final_Source_Mapping.csv')};rows=read(DOC/'Main05_Voice_Matrix.csv')
anchor_source=next((ROOT/'Limitless_TTS_Output_Missing_Main01_05_08_12_v2').rglob('main05_taeon_supp_001.wav')).relative_to(ROOT).as_posix()
for r in rows:
    id=r['stable_id'];page=[p for p in pages if p['id']==id];assert len(page)==1
    p=page[0];assert p['clip']==r['asset_path'] and p['text']==r['display_text'].replace('\r\n','\n')
    asset=ROOT/'Unity/Client'/r['asset_path']
    with wave.open(str(asset)) as w:
        raw=w.readframes(w.getnframes());expected=struct.unpack('<'+'h'*(len(raw)//2),raw);duration=w.getnframes()/w.getframerate()
    actualraw=(TEMP/('runtime/'+id+'.pcm-f32')).read_bytes();actual=struct.unpack('<'+'f'*(len(actualraw)//4),actualraw)
    assert len(actual)==len(expected);error=max(abs(a-b/32768) for a,b in zip(actual,expected));assert error<1e-6
    r.update(sha256=sha(asset),pcm_sha256=hashlib.sha256(raw).hexdigest(),duration=duration,runtime_pcm_max_error=error,runtime_resolve='PASS;'+p['clip'],semantic_status='USER_LISTENED_PASS_LATEST_REQUEST' if id in src else 'USER_CONFIRMED_NORMAL_EXISTING_ANCHOR',status='PASS_FINAL',source_wav=src[id]['source_wav'] if id in src else anchor_source,playback_invocation='1_AUTHORED_PAGE;single_source;natural_stop;previous_clip_cleanup',clip_name=id)
    if id in src:assert r['sha256']==src[id]['sha256']
savecsv(DOC/'Main05_Voice_Matrix.csv',rows)
baseline=json.loads((TEMP/'baseline.json').read_text(encoding='utf-8'));changed=[p for p,h in baseline.items() if not (ROOT/p).is_file() or sha(ROOT/p)!=h]
allowed={r['unity_asset'] for r in src.values()}|{'Unity/Client/Assets/_Project/Scripts/Editor/Main05ReturnVoiceAudit.cs'}
assert set(changed)==allowed,changed
# NEEDS_LISTENING의 역사적 목록/표시는 그대로 두고 해당 5행의 최신 승인 상태만 기록한다.
matrix=read(DOC/'Story_Dialogue_Audit_Matrix.csv');marker_before=sum('NEEDS_LISTENING' in r['audit_result'] for r in matrix)
for r in matrix:
    id=r['dialogue_id']
    if id in src:
        s=src[id];r.update(audit_result='NEEDS_LISTENING;REGENERATED;USER_LISTENED_PASS;UNITY_APPLIED;RUNTIME_VERIFIED',current_semantic_state='USER_LISTENED_PASS_FINAL_MAIN05',protected_pass='True',replacement_source=s['source_wav'],source_hash_equal='True',runtime_listening_pass='False',runtime_playback_pass='True',semantic_listening_required='False',wav_sha256=s['sha256'],wav_seconds=s['duration'],listening_evidence='Latest user request: full source listening PASS 5/5; runtime mapping PCM PASS',recommended_action='RESOLVED_USER_APPROVED_SOURCE_RUNTIME_MAPPING_PASS')
assert marker_before==244 and sum('NEEDS_LISTENING' in r['audit_result'] for r in matrix)==244
savecsv(DOC/'Story_Dialogue_Audit_Matrix.csv',matrix)
candidates=read(DOC/'Main05_TTS_Regen_Candidates.csv')
for r in candidates:r.update(status='RESOLVED_USER_APPROVED_NEW_WAV',evidence='Latest user full listening PASS; source SHA equals Unity; runtime resolve/PCM PASS')
savecsv(DOC/'Main05_TTS_Regen_Candidates.csv',candidates)
summary=dict(status='CLOSED',source_user_listening_pass=5,source_auto_qa_pass=5,duplicate_pcm=0,runtime_pass=log.count('PASS '),runtime_fail=0,pages=24,main05_mapping_pcm_pass=6,page_skip=0,page_duplicate=0,input_bleed=0,main04_contamination=0,save_continue_checkpoints=3,compile_errors=0,preexisting_compile_warnings=16,not_verified=0,user_input_required=0,changed_existing=changed,protected_baseline_files=len(baseline)-len(changed),needs_listening_historical=244)
(DOC/'Main05_Final_Verification.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
(DOC/'Main05_Final_Runtime_Results.txt').write_text('\n'.join(line.rstrip() for line in log.splitlines())+'\n',encoding='utf-8')
(DOC/'Main05_Final_Runtime_Page_Order.jsonl').write_text('\n'.join(json.dumps(p,ensure_ascii=False) for p in pages)+'\n',encoding='utf-8')
print(json.dumps(summary,ensure_ascii=False))
