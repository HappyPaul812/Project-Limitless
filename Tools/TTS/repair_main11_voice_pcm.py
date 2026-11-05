from pathlib import Path
import wave,hashlib,json,shutil
root=Path(__file__).resolve().parents[2]
source=root/'Google TTS/Limitless_TTS_Output_PreSerin'
destination=root/'Temp/Issue93Voice/Repaired'
destination.mkdir(exist_ok=True)
# ASR의 문장 순서와 원본의 무음 구간을 함께 확인한 경계입니다. PCM 샘플만 재배치하며 생성/재인코딩하지 않습니다.
plan={
 'main11_miel_001':('miel/main11_miel_002.wav',None,None),
 'main11_miel_002':('miel/main11_miel_003.wav',None,None),
 'main11_miel_003':('miel/main11_miel_004.wav',None,None),
 'main11_miel_004':('miel/main11_miel_005.wav',0,2.44),
 'main11_miel_005':('miel/main11_miel_005.wav',2.44,None),
 'main11_paul_001':('paul/paul_main01_12_full.wav',246.56,251.10),
 'main11_paul_002':('paul/paul_main01_12_full.wav',253.47,258.84),
 'main11_paul_003':('paul/main11_paul_002.wav',None,None),
 'main11_paul_008':('paul/main11_paul_009.wav',0,6.14),
 'main11_paul_009':('paul/main11_paul_009.wav',7.5,None),
}
proof=[]
for identifier,(relative,start,end) in plan.items():
    src=source/relative;dest=destination/(identifier+'.wav');old=hashlib.sha256((root/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main11'/dest.name).read_bytes()).hexdigest()
    with wave.open(str(src)) as w:fmt=w.getparams();raw=w.readframes(w.getnframes())
    assert fmt.nchannels==1 and fmt.sampwidth==2 and fmt.framerate==24000
    a=0 if start is None else round(start*fmt.framerate);b=fmt.nframes if end is None else round(end*fmt.framerate)
    pcm=raw[a*2:b*2]
    if start is None and end is None:shutil.copyfile(src,dest)
    else:
        with wave.open(str(dest),'wb') as w:w.setparams(fmt);w.writeframes(pcm)
    with wave.open(str(dest)) as w:assert w.readframes(w.getnframes())==pcm
    proof.append(dict(id=identifier,source=src.relative_to(root).as_posix(),source_sha256=hashlib.sha256(src.read_bytes()).hexdigest(),old_sha256=old,final_sha256=hashlib.sha256(dest.read_bytes()).hexdigest(),start_sample=a,end_sample=b,pcm_sha256=hashlib.sha256(pcm).hexdigest(),method='RAW_PCM_SLICE' if start is not None or end is not None else 'EXISTING_WAV_COPY'))
(root/'Temp/Issue93Voice/Evidence/repair-proof.json').write_text(json.dumps(proof,ensure_ascii=False,indent=2),encoding='utf8')
print('Repaired',len(proof),'WAVs; meta/catalog/text unchanged; TTS calls 0')
