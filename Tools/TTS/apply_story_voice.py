"""감사 결과가 정확히 대응한 대사에만 ID를 넣고 원본 WAV를 복사합니다."""
import collections
import hashlib
import json
import pathlib
import shutil
import audit_story_voice

root = audit_story_voice.ROOT
report = audit_story_voice.run(root/'Limitless_TTS_Output_PreSerin')
grouped = collections.defaultdict(list)
for row in report['rows']:
    if row['status'] == 'MAPPED':
        grouped[row['source_file']].append(row)
for relative, rows in grouped.items():
    path = root/relative
    source = path.read_text(encoding='utf-8-sig')
    for row in sorted(rows, key=lambda x: x['call']['end'], reverse=True):
        call = row['call']
        if call['id']:
            assert call['id'] == row['clip_id'], '기존 ID 충돌'
        else:
            source = source[:call['end']] + ', "' + row['clip_id'] + '"' + source[call['end']:]
    for helper in ('P', 'T', 'M'):
        source = source.replace('DialogueLine '+helper+'(string text)',
                                'DialogueLine '+helper+'(string text, string dialogueId = null)')
    # 위 세 helper만 선택적 ID를 전달합니다. Player helper나 다른 기존 대사는 그대로 둡니다.
    import re
    source = re.sub(r'(DialogueLine [PTM]\(string text, string dialogueId = null\) => new DialogueLine\([^\n]+, text)\);',
                    r'\1, dialogueId);', source)
    path.write_text(source, encoding='utf-8')
    for row in rows:
        original = root/'Limitless_TTS_Output_PreSerin'/row['output_file']
        destination = root/'Unity/Client/Assets/_Project/Audio/Voice/Story'/f"Main{int(row['quest_no']):02d}"/original.name
        destination.parent.mkdir(parents=True, exist_ok=True)
        if destination.exists():
            assert destination.read_bytes() == original.read_bytes(), '기존 Asset 충돌'
        else:
            shutil.copyfile(original, destination)
        assert hashlib.sha256(destination.read_bytes()).digest() == hashlib.sha256(original.read_bytes()).digest()
print('Mapped and copied', sum(map(len, grouped.values())))
