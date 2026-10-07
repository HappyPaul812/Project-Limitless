"""복구27개 Runtime PCM과 원본 보존·내용 경계를 대조한다. 사람 청취는 별도로 남긴다."""
import csv,difflib,hashlib,json,re,struct,subprocess,wave
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];DOC=ROOT/'문서/00_프로젝트';TEMP=ROOT/'Temp/Main07Recovery'
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
read=lambda p:list(csv.DictReader(p.open(encoding='utf-8-sig',newline='')))
def save(p,rows):
    with p.open('w',encoding='utf-8-sig',newline='') as f:
        w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
normalize=lambda s:re.sub(r'[^가-힣A-Za-z0-9]','',s)
log=(TEMP/'runtime/final.txt').read_text(encoding='utf-8');assert log.startswith('PASS\n') and 'FAIL ' not in log
trace=[json.loads(s) for s in (TEMP/'runtime/trace.jsonl').read_text(encoding='utf-8').splitlines()]
early=read(DOC/'Main07_Early7_Applied_Mapping.csv');repairs=read(DOC/'Main07_Paul_Recovery_Matrix.csv')
original=json.loads((TEMP/'paul-original-audit.json').read_text(encoding='utf-8'));repaired=json.loads((TEMP/'repaired-asr.json').read_text(encoding='utf-8'));earlyasr=json.loads((TEMP/'early-asr.json').read_text(encoding='utf-8'))
listening=json.loads((TEMP/'user_listening_confirmation.json').read_text(encoding='utf-8'))['sha256']
proof=json.loads((TEMP/'repair-plan.json').read_text(encoding='utf-8'));assert len(proof)==20 and len(repaired)==20 and len(earlyasr)==7
for r in proof:
    pcm=b''
    for part in r['parts']:
        with wave.open(str(ROOT/part['source_wav'])) as w:raw=w.readframes(w.getnframes())
        raw=raw[part['start_sample']*2:part['end_sample']*2];assert hashlib.sha256(raw).hexdigest()==part['sha256'];pcm+=raw
    asset=ROOT/('Unity/Client/Assets/_Project/Audio/Voice/Story/Main07/'+r['stable_id']+'.wav')
    with wave.open(str(asset)) as w:assert w.readframes(w.getnframes())==pcm
    assert sha(asset)==r['sha256']
for r in early+repairs:
    id=r['stable_id'];asset=ROOT/r['unity_asset'] if 'unity_asset' in r else ROOT/('Unity/Client/Assets/_Project/Audio/Voice/Story/Main07/'+id+'.wav')
    assert sha(asset)==r['sha256']
    with wave.open(str(asset)) as w:raw=w.readframes(w.getnframes())
    source=struct.unpack('<'+'h'*(len(raw)//2),raw);raw=(TEMP/'runtime'/(id+'.pcm-f32')).read_bytes();actual=struct.unpack('<'+'f'*(len(raw)//4),raw)
    assert len(source)==len(actual);error=max(abs(a-b/32768) for a,b in zip(actual,source));assert error<1e-6
    page=[p for p in trace if p['stage']=='PageDisplay' and p['id']==id and p['sequence']!='PaulFirst'];assert len(page)==1 and page[0]['clip']==id
    r.update(runtime_resolve='PASS;'+page[0]['path'],runtime_pcm_max_error=error)
    recognition=repaired[id] if id in repaired else earlyasr[id];text=' '.join(s['text'].strip() for s in recognition['segments'])
    expected=r.get('expected_text',r.get('exact_text'));ratio=difflib.SequenceMatcher(None,normalize(expected),normalize(text)).ratio();assert ratio>.7,(id,ratio,text)
    r.update(asr_text=text,asr_text_similarity=ratio,asr_status='CORRESPONDING_PAGE_ONLY;APPROXIMATE_TRANSCRIPT;NOT_HUMAN_LISTENING')
    if id in repaired:
        if id in listening:assert listening[id]==r['sha256']
        r['status']='RECOVERED_PCM;ASR_PAGE_BOUNDARY_PASS;RUNTIME_PASS;'+('USER_LISTENED_PASS' if id in listening else 'USER_LISTENING_PENDING')
save(DOC/'Main07_Early7_Applied_Mapping.csv',early);save(DOC/'Main07_Paul_Recovery_Matrix.csv',repairs)
paul=[p for p in trace if p['sequence']=='PaulFirst'];start=next(p for p in paul if p['stage']=='PageDisplay' and p['page']==0);end=next(p for p in paul if p['stage']=='NoInputWaitEnd')
assert end['page']==0 and end['clip']=='main07_paul_001' and end['time']-start['time']>=10
assert all(p['clip']!='main07_paul_002' for p in paul if start['time']<=p['time']<=end['time'])
assert '원래' not in ' '.join(s['text'] for s in repaired['main07_paul_001']['segments']) and '반갑' not in ' '.join(s['text'] for s in repaired['main07_paul_002']['segments'])
baseline=json.loads((TEMP/'baseline.json').read_text(encoding='utf-8'));changed=[p for p,h in baseline.items() if not (ROOT/p).is_file() or sha(ROOT/p)!=h]
allowed={'Unity/Client/Assets/_Project/Scripts/Editor/Main07EarlyDialogueAudit.cs','Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset'}|{'Unity/Client/Assets/_Project/Audio/Voice/Story/Main07/'+r['stable_id']+'.wav' for r in repairs}
assert set(changed)==allowed,changed
new=[p.relative_to(ROOT).as_posix() for p in (ROOT/'Unity/Client/Assets').rglob('*') if p.is_file() and p.relative_to(ROOT).as_posix() not in baseline]
assert set(new)=={r['unity_asset']+suffix for r in early for suffix in ['', '.meta']},new
catalog=ROOT/'Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset';old=subprocess.check_output(['git','show','0249f49:'+catalog.relative_to(ROOT).as_posix()]).decode('utf-8-sig')
entries=lambda text:re.findall(r'id: (\S+)\s+clip: \{fileID: 8300000, guid: (\w+), type: 3\}\s+speakerId: (\S+)',text)
oldentries=entries(old);newentries=entries(catalog.read_text(encoding='utf-8-sig'));assert newentries[:len(oldentries)]==oldentries and len(newentries)==len(oldentries)+7
with (DOC/'Main07_Paul_TTS_Regen_Required.csv').open('w',encoding='utf-8-sig',newline='') as f:csv.writer(f).writerow(['stable_id','speaker','exact_text','status','evidence'])
summary=dict(status='PCM_RECOVERY_RUNTIME_PASS;USER_LISTENING_PENDING',early_applied=7,early_runtime_resolve_pcm_pass=7,paul_audited=20,paul_recovered=20,paul_runtime_pcm_pass=20,root_cause=['UNCUT_MULTI_SENTENCE_WAV','WRONG_SEGMENT_BOUNDARY','SEGMENT_INDEX_SHIFT'],new_tts_api_calls=0,tts_regen_required=0,source_reencoded=0,source_resampled=0,original_source_changed=0,existing_guid_changed=0,other_voice_changed=0,needs_listening_historical=244,paul001_duration=next(r['final_duration'] for r in repairs if r['stable_id']=='main07_paul_001'),paul002_duration=next(r['final_duration'] for r in repairs if r['stable_id']=='main07_paul_002'),no_input_wait_seconds=end['time']-start['time'],paul002_playback_during_wait=0,runtime_pass=log.count('PASS '),runtime_fail=0,changed_existing=changed,protected_baseline_files=len(baseline)-len(changed),new_voice_assets=7,early_user_listening_pending=7,paul_user_listening_pending=20,pipeline_original_script_not_found=True)
summary.update(paul_user_listened_pass=len(listening),paul_user_listening_pending=20-len(listening),user_listened_ids=list(listening))
(DOC/'Main07_Recovery_Verification.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
(DOC/'Main07_Recovery_Runtime_Results.txt').write_text('\n'.join(x.rstrip() for x in log.splitlines())+'\n',encoding='utf-8')
(DOC/'Main07_Recovery_Runtime_Trace.jsonl').write_text('\n'.join(json.dumps(p,ensure_ascii=False) for p in trace)+'\n',encoding='utf-8')
print(json.dumps(summary,ensure_ascii=False))
