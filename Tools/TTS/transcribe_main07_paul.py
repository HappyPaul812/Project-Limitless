"""기존 PCM의 발화/문장 경계를 오프라인 ASR로 기록한다. TTS 생성 API를 호출하지 않는다."""
import json,os,sys
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];TEMP=ROOT/'Temp/Main07Recovery';TEMP.mkdir(parents=True,exist_ok=True)
sys.stdout.reconfigure(encoding='utf-8')
sys.path.insert(0,str(TEMP/'asr-deps'));os.environ['HF_HOME']=str(TEMP/'hf-cache')
from faster_whisper import WhisperModel
model=WhisperModel('small',device='cpu',compute_type='int8',cpu_threads=8,download_root=str(TEMP/'models'))
early='--early' in sys.argv;repaired='--repaired' in sys.argv
out=TEMP/('early-asr.json' if early else 'repaired-asr.json' if repaired else 'paul-asr.json');results=json.loads(out.read_text(encoding='utf-8')) if out.exists() else {}
folder=ROOT/'Limitless_TTS_Main07_Early_7' if early else ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main07'
for p in sorted(folder.glob('*.wav' if early else 'main07_paul_*.wav')):
    if p.stem in results:continue
    segments,info=model.transcribe(str(p),language='ko',beam_size=5,temperature=0,word_timestamps=True,vad_filter=False,condition_on_previous_text=False)
    rows=[]
    for s in segments:
        rows.append(dict(start=s.start,end=s.end,text=s.text,avg_logprob=s.avg_logprob,no_speech_prob=s.no_speech_prob,words=[dict(start=w.start,end=w.end,word=w.word,probability=w.probability) for w in s.words or []]))
    results[p.stem]=dict(file=str(p),duration=info.duration,segments=rows)
    out.write_text(json.dumps(results,ensure_ascii=False,indent=2),encoding='utf-8')
    print(p.stem,' | '.join(s['text'] for s in rows),flush=True)
