"""기존 승인44 bytes를 재사용하는 로컬 준비 전용. 배포/permit/CLI/usage/모델 실행은 없습니다."""
import hashlib
import importlib.util
import json
from pathlib import Path
import uuid
import zipfile

HERE=Path(__file__).resolve().parent
REPO=HERE.parents[1]
PREPARED=REPO.parent/'InferenceTrial90Prepared20261026'
EVIDENCE=REPO/'문서/00_프로젝트/Evidence/Inference90Prepared20261026'


def sha(path):return hashlib.sha256(path.read_bytes()).hexdigest()


def prepare(*,launcher_name='run-approved-inference-once90.ps1',base_commit='67669b9b69b297280381a258bb3c5a80ae427bcd',
            prior_marker=None,validation_results=None):
    if launcher_name not in {'run-approved-inference-once90.ps1','run-approved-inference-authfix-once90.ps1'}:
        raise RuntimeError('LAUNCHER_SCOPE_HOLD')
    if PREPARED.exists() or EVIDENCE.exists():raise RuntimeError('PREPARATION_EXISTS_DO_NOT_REGENERATE_APPROVAL_ID')
    spec=importlib.util.spec_from_file_location('core_prepare',HERE/'inference-once-core.py')
    core=importlib.util.module_from_spec(spec);spec.loader.exec_module(core)
    old_draft=REPO/'문서/00_프로젝트/Evidence/UsageTrialBindingFix20261026/usage-only-approval.DRAFT_NOT_AUTHORIZED.json'
    if sha(old_draft)!='1f561edc3f68e1c7fd599b1608c4b43613e80e9a30705d89e27673bb6f9fae79':raise RuntimeError('PRIOR_DRAFT_SHA_HOLD')
    draft=json.loads(old_draft.read_text(encoding='utf8'))
    source=REPO.parent/'UsageTrialLaunchBindingFix20261026/usage-package.zip'
    if sha(source)!='55241c88d0f43c518faac6f66d7b892fc3128e4bffa320bfc899f613e5b466a9':raise RuntimeError('PRIOR_PACKAGE_SHA_HOLD')
    files={}
    with zipfile.ZipFile(source) as archive:
        for name,value in draft['runtime_sha256'].items():
            relative='Tools/DiscordAICollab/'+name;data=archive.read(relative)
            if hashlib.sha256(data).hexdigest()!=value:raise RuntimeError('PRIOR_RUNTIME_SHA_HOLD')
            files[relative]=data
        files['Tools/DiscordAICollab/roles.json']=archive.read('Tools/DiscordAICollab/roles.json')
    for name in ('inference-once90.py','inference-once-core.py'):
        files['Tools/DiscordAIServiceDiagnostics/'+name]=(HERE/name).read_bytes()
    if len(files)!=47:raise RuntimeError('PACKAGE_SET_HOLD')
    request=uuid.uuid4().hex
    boot=Path('C:/ProgramData/LimitlessRelay/boot-approval.json')
    if sha(boot)!=draft['boot_approval_sha256']:raise RuntimeError('BOOT_APPROVAL_SHA_HOLD')
    base=json.loads(boot.read_text(encoding='utf-8-sig'))
    changed={'collab/usage_bridge.py','collab/service_ai.py','collab/service_batch_once.py'}
    unchanged=[k for k,v in base['runtime_sha256'].items() if k not in changed and draft['runtime_sha256'][k]==v]
    if len(unchanged)!=40:raise RuntimeError('UNCHANGED40_HOLD')
    operating={k:sha(Path(base['tool_root'])/k) for k in base['runtime_sha256']}
    if operating!=base['runtime_sha256']:raise RuntimeError('OPERATING_RUNTIME_SHA_HOLD')
    cli=Path(base['service_codex']['path'])
    if sha(cli)!=draft['cli_sha256']:raise RuntimeError('CLI_SHA_HOLD')
    official_path=REPO/'문서/00_프로젝트/Evidence/ServiceCLI_OfficialPackage_20261024.json'
    if sha(official_path)!='04cecc241628c7a17a46ef9fc3befbe32e287792f938ecc4f854f20a40eb9833':raise RuntimeError('OFFICIAL_PACKAGE_MANIFEST_HOLD')
    official=json.loads(official_path.read_text(encoding='utf8'))
    official_files={f['path']:f['sha256'] for f in official['files']}
    if len(official_files)!=45 or any(sha(cli.parent.parent/k)!=v for k,v in official_files.items()):raise RuntimeError('OFFICIAL_PACKAGE_SHA_HOLD')
    metadata=Path(base['tool_root'])/'BatchReadOnlyMetadata.psm1'
    if sha(metadata)!='09458bfc9a2b5516784efa7e9bda7013506dc0c21673b8aad7aec84240b60faf':raise RuntimeError('METADATA_SHA_HOLD')
    markers={str(p):sha(p) for p in [Path('C:/ProgramData/LimitlessRelay/UsageOnly20261026.launch-consumed.json'),
        Path('C:/ProgramData/LimitlessRelay/UsageOnly20261026.bindingfix-e10d02cb.launch-consumed.json')]}
    if prior_marker is not None:
        expected=Path('C:/ProgramData/LimitlessRelay/InferenceOnce90-717a0afa735d485daf790579b964e8c2.launch-consumed.json')
        if Path(prior_marker)!=expected:raise RuntimeError('PRIOR_INFERENCE_MARKER_SCOPE_HOLD')
        markers[str(expected)]=sha(expected)
    PREPARED.mkdir();EVIDENCE.mkdir()
    package=PREPARED/'inference-package.zip'
    with zipfile.ZipFile(package,'x',compression=zipfile.ZIP_DEFLATED) as archive:
        for name,data in sorted(files.items()):archive.writestr(name,data)
    permit={'approved':False,'kind':'INFERENCE_ONCE90','request_id':request,'expires_at':0,
      'boot_approval_sha256':draft['boot_approval_sha256'],'trial_tool_root':'SET_ONLY_ON_OWNER_MANUAL_LAUNCH',
      'entry_sha256':sha(HERE/'inference-once90.py'),'core_sha256':sha(HERE/'inference-once-core.py'),
      'roles_sha256':draft['roles_sha256'],'runtime_sha256':draft['runtime_sha256'],'cli_sha256':draft['cli_sha256'],'policy':core.POLICY}
    manifest={'kind':'PREPARED_INFERENCE_ONCE90','request_id':request,'permit':permit,
      'launcher_sha256':sha(HERE/launcher_name),'job_sha256':sha(HERE/'InferenceJob90.cs'),
      'metadata_sha256':sha(metadata),'archive_sha256':sha(package),'files_sha256':{k:hashlib.sha256(v).hexdigest() for k,v in files.items()},
      'official_package_sha256':official_files,'previous_markers_sha256':markers}
    raw=json.dumps(manifest,ensure_ascii=False,indent=2)+'\n'
    (PREPARED/'Manifest.json').write_text(raw,encoding='utf8');(EVIDENCE/'Manifest.PREPARED_NOT_LAUNCHED.json').write_text(raw,encoding='utf8')
    record={'status':'PREPARED_NOT_DEPLOYED_NOT_LAUNCHED','request_id':request,'consumed':False,'permit_active':False,
      'model_calls':0,'official_usage_reads':0,'operating_changes':0,'runtime44_count':44,'unchanged40_preserved':True,
      'package_files':47,'archive_sha256':sha(package),'manifest_sha256':sha(PREPARED/'Manifest.json'),
      'new_source_sha256':{p.name:sha(p) for p in HERE.iterdir() if p.name in {
          'inference-once-core.py','inference-once90.py','InferenceJob90.cs',launcher_name,
          'test_inference_once90.py','test_inference_job90.ps1','prepare-inference-once90.py'}},
      'offline_results':validation_results if validation_results is not None else {'python_new_cases_pass':36,'native_new_cases_pass':7,'phase_budget_cases_pass':1,'fail':0},
      'prior_operating_runtime43_sha256':operating,
      'previous_reserved_request':'e0d2e6fb9a6e48708b35052c02ad133c','previous_reserved_request_consumed':False,
      'manual_command':'pwsh -NoProfile -File "'+str(HERE/launcher_name)+'"',
      'process_scope_owner_clarified':'ONE_USAGE_APP_SERVER_PLUS_ONE_INFERENCE_EXEC',
      'budget':{'task_limit_s':90,'supervisor_max_ms':75000,'supervisor_absolute_guard_ms':85000,'worker_max_ms':70000,'worker_logging_reserve_s':3,'parent_wait_ceiling_ms':80000},
      'not_measured':['live service execution','live model result','live internal retry count'],
      'base_commit':base_commit}
    with zipfile.ZipFile(package) as archive:
        if len(archive.namelist())!=47 or any(hashlib.sha256(archive.read(k)).hexdigest()!=v for k,v in manifest['files_sha256'].items()):raise RuntimeError('NEW_PACKAGE_VERIFY_HOLD')
    (EVIDENCE/'Preparation.json').write_text(json.dumps(record,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
    print(json.dumps({'request_id':request,'source_sha256':record['new_source_sha256'],'package_sha256':record['archive_sha256'],'manifest_sha256':record['manifest_sha256'],'prepared_only':True},ensure_ascii=False))


if __name__=='__main__':prepare()
