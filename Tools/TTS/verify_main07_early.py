"""수정 전후 실제 Input/페이지/PCM과 보호 baseline을 대조한다."""
import csv,hashlib,json,struct,wave
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];DOC=ROOT/'문서/00_프로젝트';TEMP=ROOT/'Temp/Main07Early'
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
read=lambda p:list(csv.DictReader(p.open(encoding='utf-8-sig',newline='')))
def save(p,rows):
    with p.open('w',encoding='utf-8-sig',newline='') as f:
        w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
before=(TEMP/'before/final.txt').read_text(encoding='utf-8');log=(TEMP/'final/final.txt').read_text(encoding='utf-8')
assert before.startswith('PASS\n') and log.startswith('PASS\n') and 'FAIL ' not in log
assert 'OBSERVED close sameframe dialogueOpen=True' in before and 'OBSERVED close A sameframe dialogueOpen=True' in before
assert 'OBSERVED close sameframe dialogueOpen=False' in log and 'OBSERVED close A sameframe dialogueOpen=False' in log
trace=[json.loads(s) for s in (TEMP/'final/trace.jsonl').read_text(encoding='utf-8').splitlines()]
handoff=read(DOC/'Main07_Early_TTS_Handoff.csv');newids={r['stable_id']:r for r in handoff}
early=[r for r in trace if r['stage']=='PageDisplay' and r['sequence'] in ['WheelTracks','WoundedTraveler']]
assert len(early)==8
for r in early:
    if r['speaker']=='player':assert not r['id'] and not r['clip']
    else:assert r['id'] in newids and r['text']==newids[r['id']]['exact_text'] and not r['clip']
paul=[r for r in trace if r['sequence']=='PaulFirst']
start=next(r for r in paul if r['stage']=='PageDisplay' and r['page']==0)
end=next(r for r in paul if r['stage']=='NoInputWaitEnd')
assert end['page']==0 and end['id']=='main07_paul_001' and end['text']==start['text'] and end['time']-start['time']>=start['length']+5
assert all(not r['clip'] or r['clip']=='main07_paul_001' for r in paul if start['time']<=r['time']<=end['time'])
assert any(r['stage']=='PageDisplay' and r['page']==1 and r['clip']=='main07_paul_002' for r in paul)
clips=read(DOC/'Main07_Paul_Clip_Matrix.csv')
for r in clips:
    asset=ROOT/r['unity_asset'];assert sha(asset)==r['sha256']==r['source_sha256']
    with wave.open(str(asset)) as w:raw=w.readframes(w.getnframes())
    expected=struct.unpack('<'+'h'*(len(raw)//2),raw)
    for phase in ['before','final']:
        raw=(TEMP/phase/(r['stable_id']+'.pcm-f32')).read_bytes();actual=struct.unpack('<'+'f'*(len(raw)//4),raw)
        assert len(actual)==len(expected);error=max(abs(a-b/32768) for a,b in zip(actual,expected));assert error<1e-6
    r.update(runtime_resolve='PASS_REFERENCE_ONLY',runtime_pcm_error=error,semantic_status='WRONG_AUDIO_CONTENT_USER_PAGE0_CONFIRMED;TTS_REGEN_REQUIRED' if r['stable_id'].endswith('001') else 'NOT_VERIFIED_CONTENT;CORRECT_REFERENCE_PCM')
save(DOC/'Main07_Paul_Clip_Matrix.csv',clips)
baseline=json.loads((TEMP/'baseline.json').read_text(encoding='utf-8'));changed=[p for p,h in baseline.items() if not (ROOT/p).is_file() or sha(ROOT/p)!=h]
assert changed==['Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs'],changed
matrix=read(DOC/'Story_Dialogue_Audit_Matrix.csv');assert sum('NEEDS_LISTENING' in r['audit_result'] for r in matrix)==244
summary=dict(status='INPUT_FIX_VERIFIED;TTS_PENDING',early_stable_ids_assigned=7,tts_required=7,tts_regen_required=1,paul_classification='WRONG_AUDIO_MAPPING_CATEGORY;WAV_CONTENT_001',page_auto_advance_observed=0,no_input_wait_seconds=end['time']-start['time'],page_after_no_input=0,paul002_playback_during_wait=0,explicit_next_page=1,paul001_002_runtime_pcm_error=0,before_close_sameframe_reopens=2,after_close_sameframe_reopens=0,runtime_pass=log.count('PASS '),runtime_fail=0,not_verified_content=1,new_tts_runtime_pending=7,changed_existing=changed,protected_files=len(baseline)-1,existing_voice_changes=0,historical_needs_listening=244)
(DOC/'Main07_Early_Verification.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
for phase,name in [('before','Before'),('final','Final')]:
    logtext=(TEMP/phase/'final.txt').read_text(encoding='utf-8')
    (DOC/('Main07_Early_'+name+'_Results.txt')).write_text('\n'.join(x.rstrip() for x in logtext.splitlines())+'\n',encoding='utf-8')
(DOC/'Main07_Early_Runtime_Trace.jsonl').write_text('\n'.join(json.dumps(r,ensure_ascii=False) for r in trace)+'\n',encoding='utf-8')
# 원문 정본은 해당 7개의 제목 ID만 갱신하며 본문과 기존 Paul 번호는 보존한다.
p=ROOT/'문서/03_스토리/LOCAL_Story_Dialogue_원문_부록.md';text=p.read_text(encoding='utf-8-sig')
for r in handoff:
    location=text.index(r['exact_text']);head=text.rfind('### ',0,location);headend=text.index('\n',head)
    assert text[head:headend].startswith('### M07:') or text[head:headend]=='### '+r['stable_id']
    text=text[:head]+'### '+r['stable_id']+text[headend:]
p.write_text(text,encoding='utf-8')
manifest=[dict(clip_id=r['stable_id'],quest_no=7,speaker=r['speaker'],text=r['exact_text'],voice_id=r['voice_id'],output_file=r['output_file'],character_id=r['speaker_id'],source_file=r['source_file'],status='TTS_REQUIRED') for r in handoff]
save(ROOT/'Tools/TTS/main07_early_tts_manifest.csv',manifest)
print(json.dumps(summary,ensure_ascii=False))
