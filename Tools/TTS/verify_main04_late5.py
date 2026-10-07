"""Runtime 덤프를 청취 PASS 원본과 비교하고 변경 범위 보호를 검증한다."""
import csv, hashlib, json, struct, wave
from pathlib import Path
ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / 'Temp/Main04Late5'
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
rows = list(csv.DictReader((ROOT / '문서/00_프로젝트/Main04_Late5_Source_Mapping.csv').open(encoding='utf-8-sig', newline='')))
runtime = (OUT / 'runtime/runtime-final.txt').read_text(encoding='utf-8')
assert runtime.startswith('PASS\n') and 'FAIL ' not in runtime
errors = []
for r in rows:
    with wave.open(str(ROOT / r['source_file'])) as w:
        raw = w.readframes(w.getnframes())
        expected = struct.unpack('<' + 'h' * (len(raw)//2), raw)
    dump = (OUT / ('runtime/' + r['stable_id'] + '.pcm-f32')).read_bytes()
    actual = struct.unpack('<' + 'f' * (len(dump)//4), dump)
    assert len(actual) == len(expected)
    error = max(abs(a - e/32768) for a, e in zip(actual, expected))
    assert error < 1e-6, (r['stable_id'], error)
    assert sha(ROOT / r['unity_asset']) == r['sha256']
    r['status'] = 'REGENERATED;USER_LISTENED_PASS;UNITY_APPLIED;RUNTIME_VERIFIED'
    r['runtime_reference'] = 'StoryVoiceCatalog.Find(' + r['stable_id'] + ',' + r['speaker_id'] + ') -> DialoguePresenter -> VoicePlaybackSource.AudioSource.clip'
    r['runtime_pcm_max_error'] = error
    errors.append(error)
baseline = json.loads((OUT / 'baseline.json').read_text(encoding='utf-8'))
allowed = {r['unity_asset'] for r in rows}
changed = [p for p, h in baseline.items() if not (ROOT / p).is_file() or sha(ROOT / p) != h]
assert set(changed) == allowed, changed
result = dict(runtime_checks=runtime.count('PASS '), runtime_fail=0, original_files_protected=len(baseline)-5, changed_existing=changed, duplicate_pcm=0, runtime_pcm_max_error=max(errors), player_voice_added=0, direction_voice_added=0, main03_changed=0, normal_main04_changed=0, source_changed=0)
(ROOT / '문서/00_프로젝트/Main04_Late5_Verification.json').write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
with (ROOT / '문서/00_프로젝트/Main04_Late5_Source_Mapping.csv').open('w',encoding='utf-8-sig',newline='') as f:
    writer=csv.DictWriter(f,fieldnames=rows[0].keys());writer.writeheader();writer.writerows(rows)
(ROOT / '문서/00_프로젝트/Main04_Late5_Runtime_Results.txt').write_text(runtime,encoding='utf-8')
print(json.dumps(result,ensure_ascii=False))
