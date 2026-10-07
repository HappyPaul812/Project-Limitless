# -*- coding: utf-8 -*-
"""명확한 문장 사이 무음에서 태온001 뒤의 잘못 붙은 대표 보고를 제거한다."""
import hashlib, json, wave
from pathlib import Path
import numpy as np
ROOT = Path(__file__).resolve().parents[2]
TEMP = ROOT/'Temp/Main05GuardVoice'
source = ROOT/'Limitless_TTS_Output_Missing_Main01_05_08_12_v2/taeon/main05_taeon_supp_001.wav'
target = ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main05/main05_taeon_supp_001.wav'
with wave.open(str(source), 'rb') as wav:
    params=wav.getparams(); raw=wav.readframes(wav.getnframes())
assert (params.framerate,params.nchannels,params.sampwidth)==(24000,1,2)
assert hashlib.sha256(source.read_bytes()).hexdigest()=='7d3389e5572169ba64e158be7561aa0e6dbfb518b240217aa6bf4a304a9f5f7b'
# ASR의 마지막 정상 단어 6.06초 뒤, 다음 발화 6.94초 앞의 6.17~7.00초 무음 내부다.
# 정상 문장 뒤 약 0.4초의 자연 여백을 보존하며 리샘플/증폭/페이드는 하지 않는다.
cut=6.60; frames=round(cut*params.framerate)
audio=np.frombuffer(raw,dtype='<i2').astype(float)/32768
window=audio[round(6.5*params.framerate):round(6.7*params.framerate)]
assert np.sqrt(np.mean(window**2))<10**(-45/20)
with wave.open(str(target),'wb') as wav:
    wav.setparams(params);wav.writeframes(raw[:frames*params.sampwidth])
with wave.open(str(target),'rb') as wav:
    repaired=wav.readframes(wav.getnframes())
assert repaired==raw[:frames*params.sampwidth]
report=dict(source=source.relative_to(ROOT).as_posix(),target=target.relative_to(ROOT).as_posix(),old_duration=params.nframes/params.framerate,new_duration=cut,cut_sample=frames,quiet_boundary=[6.17,7.00],last_normal_word_end=6.06,next_wrong_utterance_start=6.94,source_sha256=hashlib.sha256(source.read_bytes()).hexdigest(),sha256=hashlib.sha256(target.read_bytes()).hexdigest(),pcm_sha256=hashlib.sha256(repaired).hexdigest(),raw_pcm_prefix_exact=True,tts_api_calls=0,user_listening='USER_LISTENING_REQUIRED')
(TEMP/'repair.json').write_text(json.dumps(report,ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps(report,ensure_ascii=False,indent=2))
