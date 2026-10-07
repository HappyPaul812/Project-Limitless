"""기존 원본의 명확한 페이지 경계만 PCM으로 복구한다. 새 TTS·리샘플링·인코딩은 없다."""
import csv,hashlib,json,shutil,wave
from pathlib import Path
import numpy as np
ROOT=Path(__file__).resolve().parents[2];TEMP=ROOT/'Temp/Main07Integrated';DOC=ROOT/'문서/00_프로젝트';DEST=ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main07'
manifest={r['clip_id']:r for r in csv.DictReader((ROOT/'Limitless_TTS_Output_PreSerin/dialogue_manifest_main01_12.csv').open(encoding='utf-8-sig',newline=''))}
asr=json.loads((TEMP/'voice-original-asr.json').read_text(encoding='utf-8'));baseline=json.loads((TEMP/'baseline.json').read_text(encoding='utf-8'))
def read(p):
    with wave.open(str(p)) as w:
        fmt=(w.getnchannels(),w.getsampwidth(),w.getframerate());pcm=w.readframes(w.getnframes())
    assert fmt==(1,2,24000)
    return pcm,fmt
def inspect(p):
    raw,fmt=read(p);x=np.frombuffer(raw,dtype='<i2').astype(float);step=240;quiet=[np.sqrt(np.mean(x[i:i+step]**2))/32768<10**(-45/20) for i in range(0,len(x),step)];spans=[];start=None
    for i,q in enumerate(quiet+[False]):
        if q and start is None:start=i
        if not q and start is not None:
            if i-start>=15:spans.append([round(start*.01,3),min(i*step,len(x))/24000])
            start=None
    return dict(duration=len(x)/24000,sample_rate=24000,channels=1,bit_depth=16,pcm_sha256=hashlib.sha256(raw).hexdigest(),sha256=hashlib.sha256(p.read_bytes()).hexdigest(),silences=spans,leading_silence=spans[0][1] if spans and spans[0][0]==0 else 0,trailing_silence=len(x)/24000-spans[-1][0] if spans and abs(spans[-1][1]-len(x)/24000)<.011 else 0)
archive=TEMP/'original-voice';archive.mkdir(exist_ok=True);audit=[]
assert not (TEMP/'miel-repair-proof.json').exists(),'One-shot: preserve original repair evidence'
for speaker in ['miel','taeon']:
    for i in range(1,4):
        id=f'main07_{speaker}_{i:03}';p=DEST/(id+'.wav');key=p.relative_to(ROOT).as_posix();assert hashlib.sha256(p.read_bytes()).hexdigest()==baseline[key];shutil.copyfile(p,archive/p.name)
        v=inspect(p);v.update(stable_id=id,expected_text=manifest[id]['text'],actual_asr_text=' | '.join(s['text'].strip() for s in asr[key]['segments']),status='WRONG_SEGMENT_BOUNDARY;SEGMENT_INDEX_SHIFT' if speaker=='miel' else 'ASR_PAGE_BOUNDARY_MATCH;UNCHANGED;USER_LISTENING_PENDING');audit.append(v)
(DOC/'Main07_Miel_Taeon_Original_PCM_Audit.json').write_text(json.dumps(audit,ensure_ascii=False,indent=2),encoding='utf-8')
source=ROOT/'Limitless_TTS_Output_PreSerin/miel'
plan={1:[('main07_miel_001',0,None),('main07_miel_002',0,None)],2:[('main07_miel_003',0,None)],3:[('main09_miel_002',0,4.72)]}
proof=[]
for i,parts in plan.items():
    chunks=[];detail=[]
    for name,start,end in parts:
        p=source/(name+'.wav');raw,fmt=read(p);info=inspect(p)
        if end is not None:assert any(a<end<b for a,b in info['silences'])
        a=round(start*24000);b=len(raw)//2 if end is None else round(end*24000);chunk=raw[2*a:2*b];chunks.append(chunk)
        detail.append(dict(source=p.relative_to(ROOT).as_posix(),source_sha256=info['sha256'],start_sample=a,end_sample=b,pcm_slice_sha256=hashlib.sha256(chunk).hexdigest()))
    pcm=b''.join(chunks);out=DEST/f'main07_miel_{i:03}.wav'
    with wave.open(str(out),'wb') as w:w.setnchannels(1);w.setsampwidth(2);w.setframerate(24000);w.writeframes(pcm)
    assert read(out)[0]==pcm
    proof.append(dict(stable_id=out.stem,expected_text=manifest[out.stem]['text'],parts=detail,final=inspect(out),method='RAW_PCM_CONCAT' if len(parts)>1 else 'RAW_PCM_SLICE' if parts[0][2] is not None else 'RAW_PCM_REASSIGN',runtime='PENDING',user_listening='PENDING'))
(TEMP/'miel-repair-proof.json').write_text(json.dumps(proof,ensure_ascii=False,indent=2),encoding='utf-8')
(DOC/'Main07_Miel_PCM_Recovery_Proof.json').write_text(json.dumps(proof,ensure_ascii=False,indent=2),encoding='utf-8')
shutil.copyfile(TEMP/'voice-original-asr.json',DOC/'Main07_Miel_Taeon_Original_ASR.json')
print('Recovered Miel3; Taeon3 unchanged; rawPCM-only; source/meta/catalog unchanged; TTS calls0')
