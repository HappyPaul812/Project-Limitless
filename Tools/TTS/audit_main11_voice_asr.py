import sys, os, json, hashlib, wave
from pathlib import Path
root=Path(__file__).resolve().parents[2]
sys.stdout.reconfigure(encoding='utf-8')
old=root/'Temp/Main07Recovery'
sys.path.insert(0,str(old/'asr-deps'))
os.environ['HF_HUB_OFFLINE']='1'
from faster_whisper import WhisperModel
snapshot=next((old/'models/models--Systran--faster-whisper-small/snapshots').iterdir())
model=WhisperModel(str(snapshot),device='cpu',compute_type='int8',cpu_threads=8,local_files_only=True)
phase=sys.argv[1];paths=[Path(p) for p in sys.argv[2:]]
out=root/'Temp/Issue93Voice/Evidence'/f'{phase}-asr.json'
rows=json.loads(out.read_text(encoding='utf8')) if out.exists() else {}
for path in paths:
    key=str(path)
    if key in rows:continue
    segments,info=model.transcribe(str(path),language='ko',beam_size=5,temperature=0,word_timestamps=True,vad_filter=False,condition_on_previous_text=False)
    parts=[dict(start=s.start,end=s.end,text=s.text,words=[dict(start=w.start,end=w.end,word=w.word) for w in s.words or []]) for s in segments]
    with wave.open(str(path)) as w: pcm=w.readframes(w.getnframes());rate=w.getframerate()
    rows[key]=dict(sha256=hashlib.sha256(path.read_bytes()).hexdigest(),pcm_sha256=hashlib.sha256(pcm).hexdigest(),rate=rate,duration=info.duration,segments=parts)
    out.write_text(json.dumps(rows,ensure_ascii=False,indent=2),encoding='utf8')
    print(path.name, ' | '.join(s['text'] for s in parts),flush=True)
