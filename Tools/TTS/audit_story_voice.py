"""제작 manifest를 LOCAL 대사와 대조합니다. 음성 파일을 생성하거나 가공하지 않습니다."""
import argparse
import collections
import csv
import hashlib
import json
import pathlib
import re
import wave

ROOT = pathlib.Path(__file__).resolve().parents[2]
LITERAL = r'"(?:\\.|[^"\\])*"'
CHARACTERS = {'태온': 'companion_taeon', '미엘': 'companion_miel', '폴': 'companion_paul'}


def decode(value):
    value = value.strip()
    return json.loads(value) if re.fullmatch(LITERAL, value) else None


def arguments(source, opening):
    # 문자열 내부의 쉼표·괄호는 대사 내용이므로 인수 경계로 취급하지 않습니다.
    depth, quoted, escaped, start = 1, False, False, opening + 1
    result = []
    for i in range(start, len(source)):
        char = source[i]
        if quoted:
            if escaped:
                escaped = False
            elif char == '\\':
                escaped = True
            elif char == '"':
                quoted = False
            continue
        if char == '"':
            quoted = True
        elif char in '([{':
            depth += 1
        elif char in ')]}':
            depth -= 1
            if depth == 0:
                result.append(source[start:i].strip())
                return result, i
        elif char == ',' and depth == 1:
            result.append(source[start:i].strip())
            start = i + 1
    raise ValueError('닫히지 않은 C# 인수')


def calls(source):
    fixed = {}
    pattern = r'DialogueLine\s+(\w+)\(string\s+\w+(?:,\s*string\s+\w+\s*=\s*null)?\)\s*=>\s*new DialogueLine\([^,]+,\s*(' + LITERAL + ')'
    for match in re.finditer(pattern, source):
        fixed[match[1]] = decode(match[2])
    for match in re.finditer(r'(?P<kind>new\s+DialogueLine|L|Line|P|T|M|U)\s*\(', source):
        kind = match['kind']
        args, end = arguments(source, match.end() - 1)
        speaker = fixed.get(kind)
        text = decode(args[0]) if speaker and args else None
        if kind in ('new DialogueLine', 'L', 'Line') and len(args) >= 3:
            speaker, text = decode(args[1]), decode(args[2])
            # 플레이어 표시 이름이 실행 중 정해져도 해당 본문은 정적 대사 통계에 포함합니다.
            if text is not None and speaker is None:
                speaker = '플레이어/동적 화자'
        if kind == 'U' and args:
            speaker, text = '플레이어', decode(args[0])
        if text is not None:
            yield dict(start=match.start(), end=end, kind=kind, speaker=speaker,
                       text=text, id=decode(args[-1]) if len(args) > (1 if kind in fixed or kind == 'U' else 3) else None)


def run(input_root):
    with (input_root / 'dialogue_manifest_main01_12.csv').open(encoding='utf-8-sig', newline='') as stream:
        rows = list(csv.DictReader(stream))
    files = list(input_root.rglob('*.wav'))
    audio = []
    for path in sorted(files):
        with wave.open(str(path)) as clip:
            frames = clip.readframes(clip.getnframes())
            expected = clip.getnframes() * clip.getnchannels() * clip.getsampwidth()
            audio.append(dict(path=path.relative_to(input_root).as_posix(), bytes=path.stat().st_size,
                              seconds=clip.getnframes()/clip.getframerate(), rate=clip.getframerate(),
                              channels=clip.getnchannels(), bits=clip.getsampwidth()*8,
                              decoded=len(frames) == expected, sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    duplicate_ids = [key for key, count in collections.Counter(r['clip_id'] for r in rows).items() if count > 1]
    source_cache = {}
    for row in rows:
        path = ROOT / row['source_file']
        if path.exists() and row['source_file'] not in source_cache:
            source = path.read_text(encoding='utf-8-sig')
            source_cache[row['source_file']] = (source, list(calls(source)))
        row['character_id'] = CHARACTERS.get(row['speaker'], '')
        row['status'] = 'UNMAPPED'
        row['reason'] = ''
        if row['clip_id'] in duplicate_ids:
            row['status'] = 'DUPLICATE_ID'
        elif not (input_root / row['output_file']).is_file():
            row['status'] = 'MISSING_AUDIO'
        elif not row['character_id'] or not 1 <= int(row['quest_no']) <= 12:
            row['status'] = 'OUT_OF_SCOPE'
        elif row['source_file'].endswith('Chapter2IntroFlow.cs'):
            # 두 CSV 항목은 field05_main14_strong_pulse 블록입니다. Main12라는 파일명보다 LOCAL 분기를 우선합니다.
            row['status'] = 'OUT_OF_SCOPE_MAIN14'
            row['reason'] = 'field05_main14_strong_pulse, 세린 등장 후 Main14'
        elif row['source_file'] not in source_cache:
            row['status'] = 'MISSING_SOURCE'
        else:
            candidates = [c for c in source_cache[row['source_file']][1]
                          if c['speaker'] == row['speaker'] and c['text'] == row['text']]
            if len(candidates) == 1:
                row['status'] = 'MAPPED'
                row['call'] = candidates[0]
            elif len(candidates) > 1:
                row['reason'] = '동일 화자/본문의 여러 위치: 추측하지 않음'
            else:
                row['status'] = 'TEXT_AUDIO_MISMATCH'
    quest_stats = []
    world = ROOT / 'Unity/Client/Assets/_Project/Scripts/World'
    for quest in range(1, 13):
        source_path = next(iter(world.glob(f'MainQuest{quest:02d}*Flow.cs')), None)
        if quest == 1:
            source_path = ROOT / 'Unity/Client/Assets/_Project/Scripts/NPC/MainQuest01NpcFlow.cs'
        source = source_path.read_text(encoding='utf-8-sig') if source_path else ''
        authored = list(calls(source))
        total = len(authored)
        if quest in (1, 3):
            # string[] 구형 대화는 정의 배열과 ShowSequence의 인라인 배열의 본문을 셉니다.
            arrays = re.findall(r'(?:string\[\]\s+\w+\s*=|string\[\]\s+\w+\(\)\s*=>)\s*(?:new\[\]\s*)?\{(.*?)\}', source, re.S)
            arrays += re.findall(r'ShowSequence\([^;]*?new\[\]\s*\{(.*?)\}', source, re.S)
            total += sum(len(re.findall(LITERAL, array)) for array in arrays)
        elif quest == 2:
            reaction = source[source.index('private static string GetPathReaction'):]
            total = len(re.findall(r'return\s+' + LITERAL, reaction)) + 1  # 공통 결론 한 쪽
        elif quest == 12:
            # Main12의 명시적 조사 3종 및 잡화상인 대화 1종. Main13/14 공유 코드 전체를 세지 않습니다.
            total = 4
        incoming = [r for r in rows if int(r['quest_no']) == quest]
        mapped = sum(r['status'] == 'MAPPED' for r in incoming)
        quest_stats.append(dict(quest=quest, dialogue=total, input=len(incoming), mapped=mapped,
                                no_voice=total-mapped, mismatch=sum(r['status'] == 'TEXT_AUDIO_MISMATCH' for r in incoming),
                                unmapped=sum(r['status'] != 'MAPPED' for r in incoming)))
    return dict(audio=audio, rows=rows, quests=quest_stats, duplicates=duplicate_ids,
                status=dict(collections.Counter(r['status'] for r in rows)),
                characters=dict(collections.Counter(r['speaker'] for r in rows)))


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--input', type=pathlib.Path, default=ROOT/'Limitless_TTS_Output_PreSerin')
    parser.add_argument('--output', type=pathlib.Path, required=True)
    options = parser.parse_args()
    report = run(options.input)
    options.output.parent.mkdir(parents=True, exist_ok=True)
    options.output.write_text(json.dumps(report, ensure_ascii=False, indent=2), encoding='utf-8')
    print(json.dumps({key: report[key] for key in ('quests', 'status', 'characters', 'duplicates')}, ensure_ascii=False))
    print('audio', len(report['audio']), 'seconds', sum(a['seconds'] for a in report['audio']))
