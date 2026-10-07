"""Main07 기존 Miel/Taeon PCM 경계를 오프라인 분석한다. TTS API는 호출하지 않는다."""
import json,os,sys
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];OLD=ROOT/'Temp/Main07Recovery';TEMP=ROOT/'Temp/Main07Integrated';TEMP.mkdir(parents=True,exist_ok=True)
sys.stdout.reconfigure(encoding='utf-8');sys.path.insert(0,str(OLD/'asr-deps'));os.environ['HF_HOME']=str(OLD/'hf-cache')
from faster_whisper import WhisperModel
model=WhisperModel('small',device='cpu',compute_type='int8',cpu_threads=8,download_root=str(OLD/'models'))
repaired='--repaired' in sys.argv
out=TEMP/('voice-repaired-asr.json' if repaired else 'voice-original-asr.json');results=json.loads(out.read_text(encoding='utf-8')) if out.exists() else {}
folder=ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main07'
files=[p for p in folder.glob('*.wav') if p.stem in [f'main07_{s}_{i:03}' for s in ['miel','taeon'] for i in range(1,4)]]
if not repaired:
    # 마지막 미엘 문장이 다음 이름의 chunk에 있을 수 있으므로 원본의 모든 Main07 미엘을 조사한다.
    files+=list((ROOT/'Limitless_TTS_Output_PreSerin/miel').glob('main07_miel_*.wav'))
    files+=[ROOT/f'Limitless_TTS_Output_PreSerin/miel/main09_miel_{i:03}.wav' for i in range(1,4)]
    files+=[ROOT/f'Limitless_TTS_Output_PreSerin/miel/main09_miel_{i:03}.wav' for i in range(1,4)]
for p in sorted(files):
    key=p.relative_to(ROOT).as_posix()
    if key in results:continue
    seg,info=model.transcribe(str(p),language='ko',beam_size=5,temperature=0,word_timestamps=True,vad_filter=False,condition_on_previous_text=False)
    rows=[dict(start=s.start,end=s.end,text=s.text,words=[dict(start=w.start,end=w.end,word=w.word) for w in s.words or []]) for s in seg]
    results[key]=dict(file=key,duration=info.duration,segments=rows)
    out.write_text(json.dumps(results,ensure_ascii=False,indent=2),encoding='utf-8');print(key,' | '.join(s['text'] for s in rows),flush=True)
