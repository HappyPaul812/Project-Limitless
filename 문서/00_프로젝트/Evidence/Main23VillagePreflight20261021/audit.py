"""Main23 문서 작업의 읽기 전용 ID 조사 및 원본 파일 보호 비교."""
from pathlib import Path
import hashlib, json, subprocess, sys

ROOT = Path('F:/study/codex/Project-Limitless')
WORK = ROOT / 'Temp/Main23VillagePreflight20261021/Worktree'
OUT = Path(__file__).resolve().parent
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()

def snapshot():
    files = {}
    for directory in ['Assets', 'UserData', 'Packages', 'ProjectSettings']:
        for path in (ROOT / 'Unity/Client' / directory).rglob('*'):
            if path.is_file(): files[path.relative_to(ROOT).as_posix()] = sha(path)
    return {'files': files,
            'head': subprocess.check_output(['git','rev-parse','HEAD'],cwd=ROOT).decode().strip(),
            'status_sha256': hashlib.sha256(subprocess.check_output(['git','status','--porcelain=v1','-uall'],cwd=ROOT)).hexdigest()}

if sys.argv[1] == 'before':
    (OUT/'ProtectionBefore.json').write_text(json.dumps(snapshot(),indent=2),encoding='utf-8')
    print('Original baseline recorded')
elif sys.argv[1] == 'after':
    before=json.loads((OUT/'ProtectionBefore.json').read_text(encoding='utf-8')); after=snapshot()
    result={'before_count':len(before['files']),'after_count':len(after['files']),
            'changed':[p for p,v in before['files'].items() if after['files'].get(p)!=v],
            'added':sorted(set(after['files'])-set(before['files'])),
            'root_head_unchanged':before['head']==after['head'],
            'root_status_unchanged':before['status_sha256']==after['status_sha256'],
            'editor_actions':0,'runtime_tests_repeated':0}
    (OUT/'ProtectionResult.json').write_text(json.dumps(result,indent=2),encoding='utf-8');print(result)
elif sys.argv[1] == 'ids':
    ids=['World_FrostwayVillage','main_23_southbound_traces',
         'Spawn_FrostwayVillage_FromNorthF1','Spawn_NorthF1_FromFrostwayVillage',
         'Spawn_FrostwayVillage_Checkpoint','safezone_frostway_village',
         'frostway-guide','frostway-skill-shop','frostway-supply','frostway-inn','frostway-party',
         'safezone_frostway_village-pet-adoption','safezone_frostway_village-pet-management',
         'northf1_main23_deep','northf1_main23_traces','northf1_main23_animal_trace','northf1_main23_cart_trace',
         'frostway_main23_arrival','frostway_main23_checkpoint','shop_frostway_supply']
    result={'scope':'Unity/Client/Assets and ProjectSettings; original disk + inherited Main22 baseline',
            'candidate_ids':ids,'matches':{},'quest_catalogs':{},'scene_names':{}}
    for label,base in [('original',ROOT),('main22_baseline',WORK)]:
        hits={x:[] for x in ids}; quests=[]; scenes=[]
        for folder in ['Assets','ProjectSettings']:
            for p in (base/'Unity/Client'/folder).rglob('*'):
                if not p.is_file() or p.suffix.lower() not in ['.cs','.asset','.unity','.prefab','.meta','.json','.txt']: continue
                data=p.read_text(encoding='utf-8-sig',errors='replace')
                for x in ids:
                    if x in data: hits[x].append(p.relative_to(base).as_posix())
                if p.suffix=='.unity': scenes.append(p.stem)
                if p.parent.name=='QuestDefinitions' and p.suffix=='.asset':
                    for line in data.splitlines():
                        if line.strip().startswith('questId:'): quests.append(line.strip().split(':',1)[1].strip())
        result['matches'][label]=hits
        result['quest_catalogs'][label]={'count':len(quests),'duplicate_ids':sorted({x for x in quests if quests.count(x)>1}),'ids':quests}
        result['scene_names'][label]={'count':len(scenes),'duplicates':sorted({x for x in scenes if scenes.count(x)>1})}
    (OUT/'IdAudit.json').write_text(json.dumps(result,indent=2),encoding='utf-8')
    print('Candidate matches:',sum(len(v) for h in result['matches'].values() for v in h.values()))
