from pathlib import Path
import subprocess,json,re,hashlib,collections
root=Path('F:/study/codex/Project-Limitless');task=root/'Temp/LocalSync20261103';wt=task/'Worktree';a=json.loads((task/'BackupManifest.json').read_text(encoding='utf-8'))
def git(*args,cwd=wt):return subprocess.check_output(['git',*args],cwd=cwd)
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
changes=git('diff','--name-only','-z',a['head'],'HEAD').decode().split('\0');changes=[p for p in changes if p]
overlap=sorted(set(changes)&{e['path'] for e in a['dirty_nonprivate']});version='Unity/Client/ProjectSettings/ProjectVersion.txt'
approved_version=git('show','5f619bc0:Unity/Client/ProjectSettings/ProjectVersion.txt').decode().strip()
assert (wt/version).read_text().strip()==approved_version==(root/version).read_text().strip()
guid_to_paths=collections.defaultdict(list)
for p in (wt/'Unity/Client/Assets').rglob('*.meta'):
    m=re.search(r'(?m)^guid: ([0-9a-f]{32})',p.read_text(encoding='utf-8-sig',errors='replace'))
    if m:guid_to_paths[m.group(1)].append(str(p.relative_to(wt)))
new_meta=[p for p in changes if p.endswith('.meta') and (wt/p).exists()]
new_collisions={g:paths for g,paths in guid_to_paths.items() if len(paths)>1 and set(paths)&set(new_meta)}
scene=wt/'Unity/Client/Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity'; refs=set(re.findall(r'guid: ([0-9a-f]{32})',scene.read_text(encoding='utf-8')))
external_refs={}
for p in (task/'QA/Client/Library/PackageCache').glob('com.unity.render-pipelines.universal@*/Runtime/Materials/Sprite-Lit-Default.mat.meta'):
    m=re.search(r'(?m)^guid: ([0-9a-f]{32})',p.read_text(encoding='utf-8'))
    if m:external_refs[m.group(1)]=str(p)
missing=[g for g in refs if not g.startswith('0000000000000000') and g not in guid_to_paths and g not in external_refs]
quest_map=collections.defaultdict(list)
for p in (wt/'Unity/Client/Assets/_Project/Resources/QuestDefinitions').glob('*.asset'):
    m=re.search(r'(?m)^  questId: (.+)',p.read_text(encoding='utf-8'))
    if m:quest_map[m.group(1)].append(p.name)
duplicatequests={k:v for k,v in quest_map.items() if len(v)>1}
build_original=(root/'Unity/Client/ProjectSettings/EditorBuildSettings.asset').read_text(encoding='utf-8');build_stage=(wt/'Unity/Client/ProjectSettings/EditorBuildSettings.asset').read_text(encoding='utf-8');proposal=(task/'BuildSettings_F2_StagingProposal.asset').read_text(encoding='utf-8')
old_scenes=re.findall(r'    path: (.+)',build_original);new_scenes=re.findall(r'    path: (.+)',proposal)
preserved=[line for line in a['branches'] if not line.startswith('main ') and not line.startswith('codex/local-sync-approved-20261103 ')]
result={'source_commits':{'46':'356f9226b8ed83120f828709045d50a5041055a7','73':'27f7d1db9c3510fd39ca8bc34f09d8b8cb20e9f1','80':'70e363bea43d72c7e68abb089952e2d6f6a77a8b','83':'e32d814f7f0dee8622cf386541eab94bf1fc1d69','85':'61dce08b271bd2e801205a711a8af7d54400c84f','79_version':'5f619bc002a2b0725de65d154e377064884b5f43'},'integration_head':git('rev-parse','HEAD').decode().strip(),'changed_paths':len(changes),'dirty_overlap':overlap,'approved_version_overlap_exact_semantics':overlap==[version],'new_guid_collisions':new_collisions,'preexisting_duplicate_guid_groups':len([v for v in guid_to_paths.values() if len(v)>1]),'f2_missing_guid_refs':missing,'duplicate_quest_ids':duplicatequests,'main24_quest_files':quest_map.get('main_24_first_snow_warning',[]),'official_buildsettings_stage_unchanged':git('show',a['head']+':Unity/Client/ProjectSettings/EditorBuildSettings.asset').decode().strip()==build_stage.strip(),'user_app_ui_preserved_in_proposal':'com.unity.dt.app-ui:' in proposal,'original_scene_count':len(old_scenes),'proposal_scene_count':len(new_scenes),'original_scene_order_preserved':new_scenes[:-1]==old_scenes,'f2_added_only_to_qa_proposal':new_scenes[-1]=='Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity','held_buildsettings':True,'preserved_branch_refs':preserved,'packages_stage_unchanged':git('diff','--name-only',a['head'],'HEAD','--','Unity/Client/Packages').decode().strip()==''}
(task/'StaticIntegrationQA.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8')
(task/'ResolvedSceneExternalReferences.json').write_text(json.dumps({'package_refs':external_refs,'builtin_refs':sorted(g for g in refs if g.startswith('0000000000000000')),'preexisting_guid_duplicates':{g:p for g,p in guid_to_paths.items() if len(p)>1}},ensure_ascii=False,indent=2),encoding='utf-8')
print(json.dumps({k:v for k,v in result.items() if k not in ['preserved_branch_refs','source_commits']},ensure_ascii=False))
assert not new_collisions and not missing and not duplicatequests and result['official_buildsettings_stage_unchanged'] and result['original_scene_order_preserved'] and result['packages_stage_unchanged']
