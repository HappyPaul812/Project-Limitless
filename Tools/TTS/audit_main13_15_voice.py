"""Main13~15 CSV를 LOCAL 본문·화자와 대조하고 원본 파일을 검증합니다."""
import collections
import csv
import hashlib
import json
import pathlib
import re
import wave
from audit_story_voice import ROOT, calls, LITERAL, decode

INPUT = ROOT/'Limitless_TTS_Output_Main13_15'
CHARACTERS = {'태온':'companion_taeon','미엘':'companion_miel','폴':'companion_paul',
              '세린':'companion_serin','레온':'arbel-leon'}

def run():
    rows = list(csv.DictReader((INPUT/'dialogue_manifest_main13_15.csv').open(encoding='utf-8-sig')))
    casting = json.loads((INPUT/'CASTING.json').read_text(encoding='utf-8-sig'))
    duplicate = [id for id,n in collections.Counter(r['clip_id'] for r in rows).items() if n>1]
    for row in rows:
        source = (ROOT/row['source_file']).read_text(encoding='utf-8-sig')
        row['character_id'] = CHARACTERS.get(row['speaker'])
        row['status'] = 'UNMAPPED'
        candidates = [c for c in calls(source) if c['speaker']==row['speaker'] and c['text']==row['text']]
        # Chapter2의 조사 5종은 Line switch 본문이며 TryInteract에서 폴이 발화합니다.
        scalar = [decode(m[1]) for m in re.finditer(r'case "'+re.escape(row['scene_key'])+r'": return ('+LITERAL+r');',source)]
        if row['clip_id'] in duplicate: row['status']='DUPLICATE_ID'
        elif not (INPUT/row['output_file']).is_file(): row['status']='MISSING_AUDIO'
        elif casting[row['speaker']]['voice_id']!=row['voice_id']: row['status']='VOICE_ID_MISMATCH'
        elif len(candidates)==1:
            row['status']='MAPPED';row['call']=candidates[0]
            if candidates[0]['id'] and candidates[0]['id']!=row['clip_id']:row['status']='DIALOGUE_ID_MISMATCH'
        elif row['text'] in scalar and row['speaker']=='폴':
            row['status']='MAPPED';row['scalar']=True
        else: row['status']='TEXT_OR_SPEAKER_MISMATCH'
    audio=[]
    for path in sorted(INPUT.rglob('*.wav')):
        with wave.open(str(path)) as wav:
            frames=wav.readframes(wav.getnframes())
            assert len(frames)==wav.getnframes()*wav.getnchannels()*wav.getsampwidth()
            audio.append(dict(path=path.relative_to(INPUT).as_posix(),seconds=wav.getnframes()/wav.getframerate(),
                              rate=wav.getframerate(),channels=wav.getnchannels(),bits=wav.getsampwidth()*8,
                              sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    return dict(rows=rows,audio=audio,duplicates=duplicate,status=dict(collections.Counter(r['status'] for r in rows)),
                characters=dict(collections.Counter(r['speaker'] for r in rows)))

if __name__=='__main__':
    report=run()
    (ROOT/'Tools/TTS/main13_15_voice_audit.json').write_text(json.dumps(report,ensure_ascii=False,indent=2),encoding='utf-8')
    print(json.dumps({'rows':len(report['rows']),'audio':len(report['audio']),'status':report['status'],
                      'seconds':sum(a['seconds'] for a in report['audio'])}))
