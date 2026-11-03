from pathlib import Path
import shutil,json,hashlib,re,uuid
root=Path('F:/study/codex/Project-Limitless');task=root/'Temp/LocalSync20261103';wt=task/'Worktree'
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
src=root/'Temp/OwlSpiderEdgeReview20261103/Extracted/Spider';static=next((src/'Source_KEEP').glob('*.png'))
assert sha(static)=='c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9'
spec=json.loads((root/'Temp/Issue84OwlSpiderUnityDelta20261103/InputSHA.json').read_text(encoding='utf-8-sig'))[1]['copied']
source=[static]+[src/'Walk'/f'ForestSpider_Walk_{i:02}.png' for i in range(8)]
for p,entry in zip(source[1:],spec):assert sha(p)==entry['sha256']
folder=wt/'Unity/Client/Assets/_Project/Art/ApprovedReferences/ForestSpiderOrganicV5';folder.mkdir(parents=True,exist_ok=True)
guid=lambda p:uuid.uuid5(uuid.NAMESPACE_URL,'limitless-approved-20261103/'+p).hex
template=(wt/'Unity/Client/Assets/_Project/Resources/Main24FrostOwlCandidate/Idle/FrostOwl_v3_Idle_00_512_RGBA.png.meta').read_text(encoding='utf-8')
records=[]
for p in source:
    out=folder/p.name;shutil.copy2(p,out);meta=re.sub(r'(?m)^guid: .*$', 'guid: '+guid(p.name),template);meta=meta.replace('spritePixelsToUnits: 512','spritePixelsToUnits: 256');out.with_suffix('.png.meta').write_text(meta,encoding='utf-8');records.append({'source':str(p),'asset':str(out.relative_to(wt)),'sha256':sha(p),'role':'STATIC_APPROVED_REFERENCE' if p==static else 'WALK8_USER_ART_APPROVED','ppu':256,'pivot':[.5,.5],'runtime_binding':'NOT_ACTIVATED'})
for folder2 in [folder.parent,folder]:
    m=folder2.with_name(folder2.name+'.meta')
    if not m.exists():m.write_text('fileFormatVersion: 2\nguid: '+guid(folder2.name)+'\nfolderAsset: yes\nDefaultImporter:\n  externalObjects: {}\n  userData:\n  assetBundleName:\n  assetBundleVariant:\n',encoding='utf-8')
(task/'SpiderApprovedAssetManifest.json').write_text(json.dumps(records,ensure_ascii=False,indent=2),encoding='utf-8')
# 통합 준비에서는 user app-ui를 포함한 원본 BuildSettings를 읽어 복제 QA 전용 제안만 만듭니다.
p=root/'Unity/Client/ProjectSettings/EditorBuildSettings.asset';settings=p.read_text(encoding='utf-8');fragment='  - enabled: 1\n    path: Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity\n    guid: bcd0e6f38e69dc141b741fe5247855cf\n'
assert 'World_NorthF2_CANDIDATE' not in settings
(task/'BuildSettings_F2_StagingProposal.asset').write_text(settings.replace('  m_configObjects:\n',fragment+'  m_configObjects:\n'),encoding='utf-8')
print('approved Spider reference PNG',len(records),'runtime definition changes0; BuildSettings proposal QA-only')
