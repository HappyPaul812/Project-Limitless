"""원본 바이트·메타·ID 및 기존 대사 보존을 최종 검증합니다."""
import collections
import json
import re
import subprocess
import audit_story_voice as audit

root = audit.ROOT
report = audit.run(root/'Limitless_TTS_Output_PreSerin')
mapped = [r for r in report['rows'] if r['status'] == 'MAPPED']
assert len(mapped) == 102 and not report['duplicates']
casting = json.loads((root/'Limitless_TTS_Output_PreSerin/CASTING.json').read_text(encoding='utf-8-sig'))
guids = set()
files = set()
for row in mapped:
    assert row['call']['id'] == row['clip_id']
    assert casting[row['speaker']]['voice_id'] == row['voice_id']
    src = root/'Limitless_TTS_Output_PreSerin'/row['output_file']
    dst = root/'Unity/Client/Assets/_Project/Audio/Voice/Story'/f"Main{int(row['quest_no']):02d}"/src.name
    assert dst.read_bytes() == src.read_bytes()
    meta = dst.with_suffix('.wav.meta').read_text(encoding='utf-8')
    assert 'forceToMono: 0' in meta and 'normalize: 0' in meta and 'compressionFormat: 0' in meta
    guid = re.search(r'^guid: (\w+)', meta, re.M)[1]
    assert guid not in guids
    guids.add(guid)
    files.add(row['source_file'])
for file in files:
    previous = subprocess.check_output(['git', 'show', 'HEAD:'+file], cwd=root).decode('utf-8-sig')
    current = (root/file).read_text(encoding='utf-8-sig')
    before = [(c['speaker'], c['text']) for c in audit.calls(previous)]
    after = [(c['speaker'], c['text']) for c in audit.calls(current)]
    assert before == after, '기존 화자/본문 변경: '+file
catalog = (root/'Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset').read_text()
assert len(re.findall(r'^  - id:', catalog, re.M)) == 102
assert all(guid in catalog for guid in guids)
print(json.dumps({'original_bytes':len(mapped), 'unique_guids':len(guids), 'catalog_references':102,
                  'unchanged_dialogue_files':len(files), 'decoded_audio':sum(a['decoded'] for a in report['audio']),
                  'mapped_seconds':sum(a['seconds'] for a in report['audio'] if a['path'] in {r['output_file'] for r in mapped})}))
