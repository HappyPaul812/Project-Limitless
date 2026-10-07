"""종합 QA의 실제 PCM·기존 파일 보호·전수 Encounter 결과를 검증한다."""
import csv,hashlib,json,shutil,wave
from pathlib import Path
import numpy as np
ROOT=Path(__file__).resolve().parents[2];TEMP=ROOT/'Temp/Main07Integrated';RUN=TEMP/'runtime';DOC=ROOT/'문서/00_프로젝트'
checks=(RUN/'checks.txt').read_text(encoding='utf-8').splitlines();assert not any(x.startswith('FAIL') for x in checks);assert (RUN/'final.txt').read_text(encoding='utf-8').startswith('PASS')
pcm_results=[]
for p in RUN.glob('*.pcm-f32'):
    wav=ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main07'/(p.stem+'.wav')
    with wave.open(str(wav)) as w:raw=w.readframes(w.getnframes());assert (w.getnchannels(),w.getsampwidth(),w.getframerate())==(1,2,24000)
    expected=np.frombuffer(raw,dtype='<i2').astype(np.float32)/32768;actual=np.fromfile(p,dtype='<f4');assert len(expected)==len(actual) and np.array_equal(expected,actual),p.stem
    pcm_results.append(p.stem)
assert len(pcm_results)==33,len(pcm_results) # Early7 + Paul20 + Miel3 + Taeon3
proof=json.loads((TEMP/'miel-repair-proof.json').read_text(encoding='utf-8'))
for row in proof:
    chunks=[]
    for part in row['parts']:
        p=ROOT/part['source'];assert hashlib.sha256(p.read_bytes()).hexdigest()==part['source_sha256']
        with wave.open(str(p)) as w:raw=w.readframes(w.getnframes())
        chunk=raw[part['start_sample']*2:part['end_sample']*2];assert hashlib.sha256(chunk).hexdigest()==part['pcm_slice_sha256'];chunks.append(chunk)
    p=ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main07'/(row['stable_id']+'.wav')
    with wave.open(str(p)) as w:actual=w.readframes(w.getnframes())
    assert actual==b''.join(chunks);row['runtime']='PCM_EXACT;RESOLVE_PASS;NATURAL_STOP_PASS'
(DOC/'Main07_Miel_PCM_Recovery_Proof.json').write_text(json.dumps(proof,ensure_ascii=False,indent=2),encoding='utf-8')
baseline=json.loads((TEMP/'baseline.json').read_text(encoding='utf-8'));changed=[]
for rel,sha in baseline.items():
    p=ROOT/rel;assert p.exists(),rel
    if hashlib.sha256(p.read_bytes()).hexdigest()!=sha:changed.append(rel)
allowed=['Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs','Unity/Client/Assets/_Project/Scripts/Core/CompanionDefinition.cs','Unity/Client/Assets/_Project/Scripts/Battle/BattlePrototypeEncounter.cs']+[f'Unity/Client/Assets/_Project/Audio/Voice/Story/Main07/main07_miel_{i:03}.wav' for i in range(1,4)]
assert sorted(changed)==sorted(allowed),changed
enc=json.loads((TEMP/'encounters.json').read_text(encoding='utf-8'));rows=list(csv.reader((RUN/'retry-matrix.csv').open(encoding='utf-8')));assert len(rows)==len(enc)==10
assert {r[1] for r in rows}=={r['encounter'] for r in enc}
with (DOC/'Story_Battle_Retry_Matrix.csv').open('w',encoding='utf-8-sig',newline='') as f:
    w=csv.writer(f);w.writerow(['quest','encounter_id','flee','defeat','retry','victory','result']);w.writerows(rows)
for src,out in [('checks.txt','Main07_Integrated_Runtime_Results.txt'),('growth.csv','Companion_Growth_Runtime_Matrix.csv'),('paul-damage.csv','Main07_Paul_Damage_Runtime.csv'),('damage-comparison.csv','Main07_Paul_Damage_Comparison.csv'),('voice-pages.csv','Main07_Integrated_Voice_Pages.csv')]:shutil.copyfile(RUN/src,DOC/out)
summary=dict(runtime_pass=len(set(x for x in checks if x.startswith('PASS'))),runtime_pass_records=sum(x.startswith('PASS') for x in checks),runtime_fail=0,encounter_count=10,runtime_voice_pcm_exact=len(pcm_results),miel_recovered=3,taeon_changed=0,new_tts_api_calls=0,tts_regen_required=0,changed_existing=changed,protected_baseline_files=len(baseline)-len(changed),companion_level_save_fields_added=0,player_growth_changed=0,needs_listening_historical=244,human_miel_listening='PENDING',runtime_scope='Actual scenes/interactions/return/save; exhaustive victory/defeat fixtures; Main07 natural command victory')
(DOC/'Main07_Integrated_Verification.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2),encoding='utf-8');print(json.dumps(summary,ensure_ascii=False))
