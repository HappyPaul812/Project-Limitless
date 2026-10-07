# -*- coding: utf-8 -*-
"""Main05 여섯 음성을 독립 전사하고 원본 PCM/보호 기준을 기록한다. TTS 호출 없음."""
import hashlib, json, os, sys, wave
from pathlib import Path
ROOT = Path(__file__).resolve().parents[2]
TEMP = ROOT / 'Temp/Main05GuardVoice'
TEMP.mkdir(parents=True, exist_ok=True)
sys.stdout.reconfigure(encoding='utf-8')
if '--baseline' in sys.argv:
    paths = []
    for name in ['Unity/Client/Assets', 'Unity/Client/Packages', 'Unity/Client/ProjectSettings', 'Unity/Client/UserData']:
        paths.extend(p for p in (ROOT/name).rglob('*') if p.is_file())
    for folder in ROOT.glob('Limitless_TTS*'):
        paths.extend(p for p in folder.rglob('*') if p.is_file())
    baseline = {p.relative_to(ROOT).as_posix(): hashlib.sha256(p.read_bytes()).hexdigest() for p in paths}
    (TEMP/'baseline.json').write_text(json.dumps(baseline, indent=2), encoding='utf-8')
    for p in (ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main05').glob('*.wav'):
        (TEMP/p.name).write_bytes(p.read_bytes())
    print('baseline', len(baseline))
    sys.exit()
OLD = ROOT/'Temp/Main07Recovery'
sys.path.insert(0, str(OLD/'asr-deps'))
os.environ['HF_HOME'] = str(OLD/'hf-cache')
from faster_whisper import WhisperModel
model = WhisperModel('small', device='cpu', compute_type='int8', cpu_threads=8, download_root=str(OLD/'models'))
out = TEMP/('after-asr.json' if '--after' in sys.argv else 'before-asr.json')
results = json.loads(out.read_text(encoding='utf-8')) if out.exists() else {}
files = list((ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main05').glob('*.wav'))
if '--after' not in sys.argv:
    files += list((ROOT/'Limitless_TTS_Output_Missing_Main01_05_08_12_v2/taeon').glob('main05*.wav'))
for p in sorted(files):
    key = p.relative_to(ROOT).as_posix()
    if key in results: continue
    with wave.open(str(p), 'rb') as wav:
        params = wav.getparams(); pcm = wav.readframes(wav.getnframes())
    segments, info = model.transcribe(str(p), language='ko', beam_size=5, temperature=0, word_timestamps=True, vad_filter=False, condition_on_previous_text=False)
    rows = [dict(start=s.start, end=s.end, text=s.text, words=[dict(start=w.start,end=w.end,word=w.word) for w in s.words or []]) for s in segments]
    results[key] = dict(file=key, duration=info.duration, rate=params.framerate, channels=params.nchannels, width=params.sampwidth, sha256=hashlib.sha256(p.read_bytes()).hexdigest(), pcm_sha256=hashlib.sha256(pcm).hexdigest(), segments=rows)
    out.write_text(json.dumps(results,ensure_ascii=False,indent=2),encoding='utf-8')
    print(key, ' | '.join(s['text'] for s in rows), flush=True)
