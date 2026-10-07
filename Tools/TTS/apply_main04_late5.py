"""사용자 청취 PASS 원본 5개만 대조·복사하고 보호 해시를 남긴다."""
import csv, hashlib, json, re, shutil, wave
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / 'Limitless_TTS_Regen_Main04_LateSequence_5'
DEST = ROOT / 'Unity/Client/Assets/_Project/Audio/Voice/Story/Main04'
OUT = ROOT / 'Temp/Main04Late5'
OUT.mkdir(parents=True, exist_ok=True)
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
checkpoint = json.loads((SOURCE / '_PASS_CHECKPOINT.json').read_text(encoding='utf-8-sig'))
handoff = {r['dialogue_id']: r for r in csv.DictReader((ROOT / '문서/00_프로젝트/Main04_Direction_Miel009_TTS_Handoff.csv').open(encoding='utf-8-sig', newline=''))}
catalog = (ROOT / 'Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset').read_text(encoding='utf-8-sig')
flow = (ROOT / 'Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs').read_text(encoding='utf-8-sig')
expected = ['main04_miel_supp_009', 'main04_taeon_supp_006', 'main04_miel_supp_010', 'main04_taeon_supp_007', 'main04_miel_supp_011']
assert list(checkpoint) == expected
baseline = {}
for folder in ['Unity/Client/Assets/_Project', 'Unity/Client/UserData', 'Unity/Client/ProjectSettings', 'Unity/Client/Packages', 'Limitless_TTS_Regen_Main04_LateSequence_5']:
    for p in (ROOT / folder).rglob('*'):
        if p.is_file(): baseline[p.relative_to(ROOT).as_posix()] = sha(p)
(OUT / 'baseline.json').write_text(json.dumps(baseline, ensure_ascii=False), encoding='utf-8')
rows = []
pcm_hashes = []
for id in expected:
    c, h = checkpoint[id], handoff[id]
    source, dest = SOURCE / (id + '.wav'), DEST / (id + '.wav')
    assert c['actual_listening'] == 'PASS'
    assert c['exact_text'].replace('\n', '\n') == h['exact_text'].replace('\r\n', '\n')
    assert c['sha256'] == sha(source)
    with wave.open(str(source)) as w:
        pcm = w.readframes(w.getnframes())
        pcm_sha = hashlib.sha256(pcm).hexdigest()
        assert pcm_sha == c['pcm_sha256'] and w.getsampwidth() == 2
        duration, rate, channels = w.getnframes() / w.getframerate(), w.getframerate(), w.getnchannels()
    assert '"' + c['exact_text'].replace('\n', '\\n') + '", "' + id + '"' in flow
    guid = re.search(r'guid: (\w+)', Path(str(dest) + '.meta').read_text()).group(1)
    assert re.search(r'id: ' + id + r'\s+clip: \{fileID: 8300000, guid: ' + guid + r', type: 3\}\s+speakerId: ' + h['speaker_id'], catalog)
    pcm_hashes.append(pcm_sha)
    rows.append(dict(stable_id=id, speaker=h['speaker_name'], speaker_id=h['speaker_id'], dialogue=c['exact_text'], source_file=source.relative_to(ROOT).as_posix(), unity_asset=dest.relative_to(ROOT).as_posix(), guid=guid, duration=duration, sample_rate=rate, channels=channels, sha256=sha(source), pcm_sha256=pcm_sha, old_sha256=sha(dest), status='REGENERATED;USER_LISTENED_PASS;UNITY_APPLIED'))
assert len(set(pcm_hashes)) == 5
existing_pcm = {}
for p in (ROOT / 'Unity/Client/Assets/_Project/Audio/Voice').rglob('*.wav'):
    if p.stem in expected: continue
    with wave.open(str(p)) as w: existing_pcm[p.as_posix()] = hashlib.sha256(w.readframes(w.getnframes())).hexdigest()
assert not set(pcm_hashes).intersection(existing_pcm.values())
for r in rows:
    shutil.copyfile(ROOT / r['source_file'], ROOT / r['unity_asset'])
    assert sha(ROOT / r['unity_asset']) == r['sha256']
with (ROOT / '문서/00_프로젝트/Main04_Late5_Source_Mapping.csv').open('w', encoding='utf-8-sig', newline='') as f:
    writer = csv.DictWriter(f, fieldnames=rows[0].keys()); writer.writeheader(); writer.writerows(rows)
print('PASS source/text/speaker/GUID/catalog/PCM duplicate0; copied exactly5; baseline', len(baseline))
