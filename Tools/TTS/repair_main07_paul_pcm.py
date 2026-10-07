"""확인된 문장·무음 경계에서 원본 PCM을 분리/재배치한다. 원본과 GUID는 보존한다."""
import csv,hashlib,json,shutil,wave
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];TEMP=ROOT/'Temp/Main07Recovery';DOC=ROOT/'문서/00_프로젝트';DEST=ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main07'
audit=json.loads((TEMP/'paul-original-audit.json').read_text(encoding='utf-8'))
manifest={r['clip_id']:r for r in csv.DictReader((ROOT/'Limitless_TTS_Output_PreSerin/dialogue_manifest_main01_12.csv').open(encoding='utf-8-sig',newline=''))}
source=ROOT/'Limitless_TTS_Output_PreSerin/paul'
def sid(i):return f'main07_paul_{i:03}'
# 각 경계는 ASR 단어 순서와 -45dB RMS의 긴 무음 중앙을 함께 대조했습니다.
# 001의 두 번째 구간에 붙은 불필요한 "아"(4.40~4.60)는 두 무음 사이에서 제외합니다.
plan={1:[(1,0,4.03)],2:[(1,4.96,8.52)],3:[(1,9.35,None)],4:[(2,0,4.44)],5:[(2,4.44,None)],6:[(3,0,5.19)],7:[(3,5.19,None)],8:[(4,0,None),(5,0,None)],9:[(6,0,None)],10:[(7,0,None)],11:[(8,0,None)],12:[(9,0,None)],13:[(10,0,None)],14:[(11,0,None)],15:[(12,0,None)],16:[(13,0,None)],17:[(14,0,1.54)],18:[(14,1.54,7.79)],19:[(14,7.79,None)],20:[(15,0,None)]}
archive=TEMP/'original-unity';archive.mkdir(parents=True,exist_ok=True)
assert not (TEMP/'repair-plan.json').exists(),'Do not overwrite original repair evidence'
for i in range(1,21):
    p=DEST/(sid(i)+'.wav');assert hashlib.sha256(p.read_bytes()).hexdigest()==audit[sid(i)]['sha256'];shutil.copyfile(p,archive/p.name)
rows=[];proof=[]
for target,parts in plan.items():
    chunks=[];details=[];fmt=None
    for index,start,end in parts:
        id=sid(index);p=source/(id+'.wav')
        with wave.open(str(p)) as w:
            current=(w.getnchannels(),w.getsampwidth(),w.getframerate());raw=w.readframes(w.getnframes());frames=w.getnframes()
        assert current==(1,2,24000) and (fmt is None or current==fmt);fmt=current
        a=round(start*fmt[2]);b=frames if end is None else round(end*fmt[2]);assert 0<=a<b<=frames
        for t in [start,end]:
            if t is None or t==0:continue
            assert any(lo<t<hi for lo,hi in audit[id]['silences']),(id,t)
        chunk=raw[a*2:b*2];chunks.append(chunk);details.append(dict(source_wav=p.relative_to(ROOT).as_posix(),start_sample=a,end_sample=b,start_sec=start,end_sec=frames/fmt[2] if end is None else end,sha256=hashlib.sha256(chunk).hexdigest()))
    pcm=b''.join(chunks);out=DEST/(sid(target)+'.wav')
    with wave.open(str(out),'wb') as w:w.setnchannels(fmt[0]);w.setsampwidth(fmt[1]);w.setframerate(fmt[2]);w.writeframes(pcm)
    with wave.open(str(out)) as w:assert w.readframes(w.getnframes())==pcm
    proof.append(dict(stable_id=sid(target),parts=details,pcm_sha256=hashlib.sha256(pcm).hexdigest(),sha256=hashlib.sha256(out.read_bytes()).hexdigest(),duration=len(pcm)/2/fmt[2]))
    rows.append(dict(stable_id=sid(target),expected_text=manifest[sid(target)]['text'],old_duration=audit[sid(target)]['duration'],final_duration=len(pcm)/2/fmt[2],source_parts=json.dumps(details,ensure_ascii=False),method='PCM_CONCAT_CONTIGUOUS_PAGE_PARTS' if len(parts)>1 else 'EXISTING_PAGE_PCM_SLICE' if parts[0][1]!=0 or parts[0][2] is not None else 'EXISTING_NORMAL_PAGE_PCM_REASSIGN',sha256=proof[-1]['sha256'],pcm_sha256=proof[-1]['pcm_sha256'],status='REPAIRED;ASR_RECHECK_PENDING;USER_LISTENING_PENDING'))
(TEMP/'repair-plan.json').write_text(json.dumps(proof,ensure_ascii=False,indent=2),encoding='utf-8')
with (DOC/'Main07_Paul_Recovery_Matrix.csv').open('w',encoding='utf-8-sig',newline='') as f:
    w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
print('PASS20 restored from15 old PCM; split source001/002/003/014; concatenate source004+005; no encoding/resample/TTS; meta preserved')
