"""Main16 manifest의 안정 ID·화자·본문 및 원본 바이트를 검증합니다."""
import collections
import csv
import hashlib
import json
import pathlib
import re
import shutil
import sys
import uuid
import wave

ROOT = pathlib.Path(__file__).resolve().parents[2]
SOURCE = ROOT / 'Limitless_TTS_Output_Main16'
CLIENT = ROOT / 'Unity/Client'
DEST = CLIENT / 'Assets/_Project/Audio/Voice/Story/Main16'
CATALOG = CLIENT / 'Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset'


def run(apply=False):
    rows = list(csv.DictReader((SOURCE/'dialogue_manifest_main16.csv').open(encoding='utf-8-sig')))
    code = (CLIENT/'Assets/_Project/Scripts/World/Main16DialogueCatalog.cs').read_text(encoding='utf-8-sig')
    lines = {m[1]: {'speaker_id': m[2], 'text': m[3]} for m in re.finditer(
        r'Line\("([^"]+)", "([^"]*)", "([^"]*)"\)', code)}
    casting = json.loads((SOURCE/'CASTING.json').read_text(encoding='utf-8-sig'))
    assert len({r['clip_id'] for r in rows}) == len(rows), 'Duplicate ID'
    for r in rows:
        assert r['clip_id'] in lines, r['clip_id']
        assert lines[r['clip_id']] == {'speaker_id': r['speaker_id'], 'text': r['text']}, r['clip_id']
        assert r['speaker_id'] and r['speaker_id'] != 'player'
        assert casting[r['speaker']]['voice_id'] == r['voice_id']
        assert casting[r['speaker']]['speaker_id'] == r['speaker_id']
        assert (SOURCE/r['output_file']).is_file()
    assert {id for id, line in lines.items() if line['speaker_id'] not in ('', 'player')} == {r['clip_id'] for r in rows}
    audio = []
    for path in sorted(SOURCE.rglob('*.wav')):
        with wave.open(str(path)) as w:
            assert len(w.readframes(w.getnframes())) == w.getnframes()*w.getnchannels()*w.getsampwidth()
            audio.append(dict(file=path.relative_to(SOURCE).as_posix(), seconds=w.getnframes()/w.getframerate(),
                              rate=w.getframerate(), channels=w.getnchannels(), bits=w.getsampwidth()*8,
                              sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    catalog = CATALOG.read_text(encoding='utf-8')
    old_entries = catalog.split('  - id: main16_')[0]
    if apply:
        DEST.mkdir(parents=True, exist_ok=True)
        folder_meta = DEST.with_suffix('.meta')
        if not folder_meta.exists():
            folder_meta.write_text('fileFormatVersion: 2\nguid: '+uuid.uuid4().hex+'\nfolderAsset: yes\nDefaultImporter:\n  externalObjects: {}\n', encoding='utf-8')
        template = (DEST.parent/'Main15/main15_taeon_004.wav.meta').read_text()
        for r in rows:
            path = DEST / pathlib.Path(r['output_file']).name
            original = SOURCE/r['output_file']
            if path.exists():
                assert path.read_bytes() == original.read_bytes()
            else:
                shutil.copyfile(original, path)
            meta = path.with_suffix('.wav.meta')
            if not meta.exists():
                meta.write_text(re.sub(r'guid: \w+', 'guid: '+uuid.uuid4().hex, template), encoding='utf-8')
            guid = re.search(r'^guid: (\w+)', meta.read_text(), re.M)[1]
            if '  - id: '+r['clip_id']+'\n' not in catalog:
                catalog += '  - id: '+r['clip_id']+'\n    clip: {fileID: 8300000, guid: '+guid+', type: 3}\n    speakerId: '+r['speaker_id']+'\n'
        CATALOG.write_text(catalog, encoding='utf-8')
        assert catalog.startswith(old_entries)
    mapped = 0
    for r in rows:
        path = DEST / pathlib.Path(r['output_file']).name
        if path.exists():
            assert path.read_bytes() == (SOURCE/r['output_file']).read_bytes()
            guid = re.search(r'^guid: (\w+)', path.with_suffix('.wav.meta').read_text(), re.M)[1]
            assert '  - id: '+r['clip_id']+'\n    clip: {fileID: 8300000, guid: '+guid+', type: 3}\n    speakerId: '+r['speaker_id'] in catalog
            mapped += 1
    report = dict(dialogue_total=len(lines), manifest_rows=len(rows), mapped=mapped,
                  voice_none=sum(l['speaker_id'] in ('', 'player') for l in lines.values()),
                  player_silent=sum(l['speaker_id']=='player' for l in lines.values()),
                  observation_silent=sum(l['speaker_id']=='' for l in lines.values()),
                  audio_count=len(audio), audio_extensions=dict(collections.Counter(p.suffix for p in SOURCE.rglob('*') if p.suffix=='.wav')),
                  total_seconds=sum(a['seconds'] for a in audio),
                  segment_seconds=sum(a['seconds'] for a in audio if a['file'] in {r['output_file'] for r in rows}),
                  characters=dict(collections.Counter(r['speaker'] for r in rows)),
                  hearing_rows=sum('_hearing_' in r['clip_id'] for r in rows), default_rows=sum('_default_' in r['clip_id'] for r in rows),
                  unmapped=len(rows)-mapped, text_mismatch=0, speaker_mismatch=0, missing_audio=0, duplicate_id=0,
                  catalog_entries=len(re.findall(r'^  - id:', catalog, re.M)), rows=rows, audio=audio)
    (ROOT/'Tools/TTS/main16_voice_audit.json').write_text(json.dumps(report, ensure_ascii=False, indent=2), encoding='utf-8')
    print(json.dumps({k:v for k,v in report.items() if k not in ('rows','audio')}, ensure_ascii=False))


if __name__ == '__main__':
    run('--apply' in sys.argv)
