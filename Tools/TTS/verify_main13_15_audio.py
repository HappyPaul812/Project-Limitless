"""새 Voice/BGM 원본 바이트와 기존 대사 보존을 검증합니다."""
import collections
import hashlib
import json
import re
import subprocess
from audit_main13_15_voice import ROOT, INPUT, run
from audit_story_voice import calls, LITERAL

report=run()
assert report['status']=={'MAPPED':41} and not report['duplicates']
catalog=(ROOT/'Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset').read_text()
guids=[]
for row in report['rows']:
    original=INPUT/row['output_file']
    copied=ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story'/f"Main{row['quest_no']}"/original.name
    assert copied.read_bytes()==original.read_bytes()
    meta=copied.with_suffix('.wav.meta').read_text()
    assert all(s in meta for s in ('forceToMono: 0','normalize: 0','compressionFormat: 0'))
    guid=re.search(r'^guid: (\w+)',meta,re.M)[1]
    assert guid in catalog and row['clip_id'] in catalog
    guids.append(guid)
assert len(set(guids))==41
for name in ('Before_the_First_Light','Morning_at_the_Gate','Morning_Over_the_Ridge'):
    original=__import__('pathlib').Path('F:/Downloads')/(name+'.mp3')
    copied=ROOT/'Unity/Client/Assets/_Project/Audio/Music'/(name+'.mp3')
    assert original.read_bytes()==copied.read_bytes()
for relative in sorted({r['source_file'] for r in report['rows']}):
    # 이번 시작 이전 commit에 대해 모든 대사와 scalar 본문이 유지되었는지 검사합니다.
    before=subprocess.check_output(['git','show','1b660af:'+relative],cwd=ROOT).decode('utf-8-sig')
    after=(ROOT/relative).read_text(encoding='utf-8-sig')
    assert [(c['speaker'],c['text']) for c in calls(before)]==[(c['speaker'],c['text']) for c in calls(after)]
    if relative.endswith('Chapter2IntroFlow.cs'):
        for m in re.finditer(r'case ('+LITERAL+r'): return ('+LITERAL+r');',before):
            assert m[0] in after
print(json.dumps({'voice_original_bytes':41,'bgm_original_bytes':3,'unique_guids':41,
                  'catalog_entries':len(re.findall(r'^  - id:',catalog,re.M)),
                  'quests':dict(collections.Counter(r['quest_no'] for r in report['rows'])),
                  'segment_seconds':sum(a['seconds'] for a in report['audio'] if a['path'] in {r['output_file'] for r in report['rows']})}))
