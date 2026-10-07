"""기존 Main07 Paul20의 형식·무음·ASR 포함내용과 복구 경계 후보를 보관한다."""
import csv,hashlib,json,wave
from pathlib import Path
import numpy as np
ROOT=Path(__file__).resolve().parents[2];TEMP=ROOT/'Temp/Main07Recovery';DOC=ROOT/'문서/00_프로젝트'
manifest={r['clip_id']:r for r in csv.DictReader((ROOT/'Limitless_TTS_Output_PreSerin/dialogue_manifest_main01_12.csv').open(encoding='utf-8-sig',newline=''))}
asr=json.loads((TEMP/'paul-asr.json').read_text(encoding='utf-8'))
def inspect(p):
    with wave.open(str(p)) as w:
        raw=w.readframes(w.getnframes());rate=w.getframerate();channels=w.getnchannels();bits=w.getsampwidth()*8
    assert channels==1 and bits==16
    pcm=np.frombuffer(raw,dtype='<i2').astype(float);step=rate//100
    levels=[float(np.sqrt(np.mean(pcm[i:i+step]**2))/32768) for i in range(0,len(pcm),step)]
    quiet=np.array(levels)<10**(-45/20);spans=[];start=None
    for i,q in enumerate(list(quiet)+[False]):
        if q and start is None:start=i
        if not q and start is not None:
            if (i-start)*.01>=.15:spans.append([round(start*.01,3),round(min(i*step,len(pcm))/rate,3)])
            start=None
    return dict(duration=len(pcm)/rate,sample_rate=rate,channels=channels,bit_depth=bits,silence_threshold_db=-45,silences=spans,leading_silence=spans[0][1] if spans and spans[0][0]==0 else 0,trailing_silence=len(pcm)/rate-spans[-1][0] if spans and abs(spans[-1][1]-len(pcm)/rate)<.011 else 0,sha256=hashlib.sha256(p.read_bytes()).hexdigest(),pcm_sha256=hashlib.sha256(raw).hexdigest())
contained={1:['001','002','003'],2:['004','005'],3:['006','007'],4:['008_PART1'],5:['008_PART2'],6:['009'],7:['010'],8:['011'],9:['012'],10:['013'],11:['014'],12:['015'],13:['016'],14:['017','018','019'],15:['020'],16:['main09_paul_001', 'main09_paul_002'],17:['main09_paul_003', 'main09_paul_004'],18:['main09_paul_005_PART1'],19:['main09_paul_005_PART2'],20:['main09_paul_006_PART1']}
results={};rows=[]
for i in range(1,21):
    id=f'main07_paul_{i:03}';p=TEMP/'original-unity'/(id+'.wav')
    if not p.exists():p=ROOT/('Unity/Client/Assets/_Project/Audio/Voice/Story/Main07/'+id+'.wav')
    v=inspect(p);v.update(expected_text=manifest[id]['text'],asr_segments=asr[id]['segments'],contained_dialogue_ids=contained[i]);results[id]=v
    src=ROOT/'Limitless_TTS_Output_PreSerin'/manifest[id]['output_file'];assert p.read_bytes()==src.read_bytes()
    rows.append(dict(stable_id=id,expected_text=v['expected_text'],actual_asr_text=' | '.join(s['text'].strip() for s in v['asr_segments']),contained_dialogue_ids=';'.join(contained[i]),asr_segment_count=len(v['asr_segments']),matched_page_units=len(contained[i]),duration=v['duration'],sample_rate=v['sample_rate'],channels=v['channels'],bit_depth=v['bit_depth'],sha256=v['sha256'],pcm_sha256=v['pcm_sha256'],leading_silence=v['leading_silence'],trailing_silence=v['trailing_silence'],silence_intervals=json.dumps(v['silences']),root_cause='UNCUT_MULTI_SENTENCE_WAV;WRONG_SEGMENT_BOUNDARY;SEGMENT_INDEX_SHIFT',status='RECOVERY_PENDING'))
(TEMP/'paul-original-audit.json').write_text(json.dumps(results,ensure_ascii=False,indent=2),encoding='utf-8')
with (DOC/'Main07_Paul_Full_Original_Audit.csv').open('w',encoding='utf-8-sig',newline='') as f:
    w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
print(json.dumps({k:v['silences'] for k,v in results.items() if k in ['main07_paul_001','main07_paul_002','main07_paul_003','main07_paul_014']},ensure_ascii=False))
