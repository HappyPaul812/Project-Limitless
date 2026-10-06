"""현재 Source/Manifest를 C# 읽기 전용 감사의 입력으로 준비한다. TTS/WAV/게임 데이터는 수정하지 않는다."""
import csv
import hashlib
import json
import re
from collections import Counter
from pathlib import Path
from story_dialogue_consistency_audit import ROOT, authored

OUT = ROOT / 'Temp/StorySemanticRisk20261006'
DOC = ROOT / '문서/00_프로젝트'


def prepare():
    OUT.mkdir(parents=True, exist_ok=True)
    old = list(csv.DictReader((DOC / 'Story_Dialogue_Audit_Matrix.csv').open(encoding='utf-8-sig')))
    by_key = {r['audit_key']: r for r in old}
    by_id = {r['dialogue_id']: r for r in old if r['dialogue_id']}
    regen = {r['dialogue_id']: r for r in csv.DictReader((DOC / 'Story_Dialogue_TTS_REGEN_REQUIRED.csv').open(encoding='utf-8-sig'))}
    protected = {f'main03_taeon_supp_{i:03}' for i in range(3, 13)}
    protected |= {f'main04_miel_supp_{i:03}' for i in range(1, 9)}
    protected |= {f'main04_taeon_supp_{i:03}' for i in range(1, 6)}
    quests = {}
    for p in (ROOT/'Unity/Client/Assets/_Project/Resources/QuestDefinitions').glob('Main*.asset'):
        no = re.match(r'Main(\d+)', p.name)
        q = re.search(r'^  questId: (.+)$', p.read_text(encoding='utf-8-sig'), re.M)
        if no and q:
            quests[int(no[1])] = q[1]
    quests = {}
    for p in (ROOT/'Unity/Client/Assets/_Project/Resources/QuestDefinitions').glob('Main*.asset'):
        no = re.match(r'Main(\d+)', p.name)
        q = re.search(r'^  questId: (.+)$', p.read_text(encoding='utf-8-sig'), re.M)
        if no and q:
            quests[int(no[1])] = q[1]
    rows, order = [], Counter()
    for a in authored():
        key = f"M{a['main']:02}:{a['source']}:{a['source_pos']}"
        previous = by_id.get(a['dialogue_id'], by_key.get(key, {}))
        r = dict(a, audit_key=key, semantic_state=previous.get('audit_result', 'NEEDS_LISTENING'),
                 user_pass=a['dialogue_id'] in protected, runtime_pass=a['dialogue_id'] in protected,
                 protected_pass=a['dialogue_id'] in protected, known_regen=a['dialogue_id'] in regen,
                 evidence='', sequence_rank=0, page=0, sequence=a['branch'], route='shared')
        r['production_batch'] = previous.get('replacement_source','').replace('\\','/').split('/')[0]
        r['quest'] = quests.get(a['main'], 'OpeningIntro' if a['main']==0 else '')
        r['scene'] = previous.get('scene','OpeningIntro' if a['main']==0 else '')
        r['quest'] = quests.get(a['main'], 'OpeningIntro' if a['main']==0 else '')
        r['scene'] = previous.get('scene','OpeningIntro' if a['main']==0 else '')
        r['production_batch'] = previous.get('replacement_source','').replace('\\','/').split('/')[0]
        if r['protected_pass']:
            r['semantic_state'] = 'PROTECTED_PASS'
            r['evidence'] = ('Main03_Supp008_012_Final_적용_QA.md;Main03_Taeon_Voice_Regen_적용_QA.md;Story_Voice_Main01_12_QA.md'
                             if a['main'] == 3 else 'Main04_FirstEncounter_Final_적용_QA.md;Story_Voice_Main01_12_QA.md')
        elif r['known_regen']:
            r['semantic_state'] = regen[a['dialogue_id']]['reason'] + ';TTS_REGEN_REQUIRED'
            r['evidence'] = regen[a['dialogue_id']]['evidence']
        order[(a['main'], a['branch'])] += 1
        r['page'] = order[(a['main'], a['branch'])]
        # Source 파일 선언 순서와 진행 순서가 다른 지점은 QuestDefinition/Flow의 호출 순서를 명시한다.
        maps = {1:['RepresentativeDialogue','GuardDialogue'],
                3:['FirstConversation','TryInteract','AfterBattleConversation','TryReach'],
                4:['FirstConversation','EncounterConversation','AfterBattleConversation'],
                5:['GuardReport','RepresentativeReport']}
        if a['main'] in maps:
            r['sequence_rank'] = maps[a['main']].index(a['branch']) + 1
        elif a['main'] == 2:
            r['sequence_rank'] = 2 if a['branch'] == 'all_paths/common_conclusion' else 1
            r['route'] = a['branch']
        else:
            r['sequence_rank'] = 1
        source = (ROOT / a['source']).read_text(encoding='utf-8-sig')
        if a['main'] in (9, 10, 11) and a['branch'] == 'Lines':
            cases = list(re.finditer(r'case\s+(\d+)\s*:', source[:a['source_pos']]))
            step = int(cases[-1][1]) if cases else 99
            r['sequence_rank'] = step + 1
            r['sequence'] = f'Lines/step{step}'
        if a['main'] == 10 and a['branch'] == 'TryInteract':
            r['sequence_rank'], r['sequence'] = 7, 'BattleRetry'
        if a['main']==7:
            if a['branch']=='PaulFirst':
                r['sequence_rank']=3
            elif a['branch']=='MielMeeting':
                r['sequence_rank']=5
            else:
                r['sequence_rank'],r['sequence']=(1,'WheelTracks') if r['page']<=4 else (2,'WoundedTraveler') if r['page']<=8 else (6,'PaulFarewell')
        if a['main'] == 11 and a['branch'] == 'AfterBoss':
            r['sequence_rank'] = 1
        if a['main'] == 8:
            before = source[:a['source_pos']]
            cond = list(re.finditer(r'if\s*\(id\s*==\s*(\w+)\)', before))
            r['sequence'] = cond[-1][1] if cond else 'DeepArea'
            # 마지막 return은 깊은 구역 진입 전후 두 배열로 이어진다.
            if a['dialogue_id'] in {f'main08_miel_supp_{i:03}' for i in range(6,9)} | {f'main08_taeon_supp_{i:03}' for i in range(9,12)}:
                r['sequence'] = 'DeepArea'
            r['sequence_rank'] = ['Trace01','Trace02','Trace03','WheelTracks','Investigation','DeepArea'].index(r['sequence'])+1
        # Chapter2는 ID 번호/화자별 정렬 대신 objective 순서로 분기별 ID 배열을 배치한다.
        packs = {
            12: [['main12_paul_supp_001'], ['main12_paul_supp_002'], ['main12_taeon_supp_001']],
            13: [['main13_paul_001'],['main13_paul_002'],['main13_miel_001','main13_serin_001','main13_serin_002','main13_serin_003'],['main13_serin_004'],['main13_leon_001']],
            14: [['main14_leon_001'],['main14_paul_001'],['main14_paul_002'],['main14_serin_001','main14_taeon_001'],['main14_paul_003'],['main14_serin_002','main14_miel_001','main14_serin_003','main14_taeon_002'],['main14_leon_002']],
            15: [['main15_leon_001','main15_serin_001'],['main15_miel_001','main15_paul_001','main15_taeon_001','main15_serin_002'],['main15_taeon_002'],['main15_serin_003'],['main15_paul_004','main15_taeon_003','main15_serin_006'],['main15_paul_005'],['main15_serin_004','main15_paul_002','main15_serin_005','main15_paul_003'],['main15_leon_002'],['main15_serin_007','main15_taeon_004','main15_serin_008','main15_miel_002','main15_paul_006']]}
        for rank, ids in enumerate(packs.get(a['main'], []), 1):
            if a['dialogue_id'] in ids:
                r['sequence_rank'], r['page'], r['sequence'] = rank, ids.index(a['dialogue_id'])+1, f'QuestSequence{rank:02}'
        if a['main'] == 16:
            scene, route = a['branch'].split('/')
            r['sequence_rank'] = ['start','ash','vibration','tracks','witness_ground','witness_emerge','retry','afterimage','canyon','report'].index(scene)+1
            r['sequence'], r['route'] = scene, route
        rows.append(r)
    # 분기 내 Page는 ID 없는 Player도 포함한다. 번호가 다른 분기 사이에 누적되지 않도록 재계산한다.
    counts = Counter()
    for r in rows:
        if r['main'] not in (12,13,14,15,16):
            counts[(r['main'],r['sequence'])] += 1
            r['page'] = counts[(r['main'],r['sequence'])]
    manifests = []
    for p in sorted(ROOT.glob('Limitless_TTS_Output_*/dialogue_manifest*.csv')):
        for r in csv.DictReader(p.open(encoding='utf-8-sig')):
            manifests.append(dict(dialogue_id=r['clip_id'],text=r['text'],speaker=r['speaker'],
                                  path=p.relative_to(ROOT).as_posix(),batch=p.parent.name,voice_id=r.get('voice_id','')))
    p = ROOT / 'Tools/TTS/opening_narration.csv'
    for r in csv.DictReader(p.open(encoding='utf-8-sig')):
        manifests.append(dict(dialogue_id=r['id'],text=r['text'],speaker='Narrator',path=p.relative_to(ROOT).as_posix(),batch='Opening',voice_id=r.get('voice_id','')))
    sources = {r['source'] for r in rows} | {r['path'] for r in manifests}
    data = dict(rows=rows, manifests=manifests, source_hashes=[dict(path=p,sha256=hashlib.sha256((ROOT/p).read_bytes()).hexdigest()) for p in sorted(sources)])
    (OUT / 'input.json').write_text(json.dumps(data,ensure_ascii=False,indent=2),encoding='utf-8')
    print(json.dumps(dict(dialogue=len(rows),manifest=len(manifests),protected=len(protected)),ensure_ascii=False))


if __name__ == '__main__':
    prepare()
