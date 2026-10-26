"""대표의 d9fa4f7c 수정본 신규1회 승인만 준비합니다. 과거 패키지/요청/소비마커는 보존합니다."""
import hashlib
import importlib.util
import json
from pathlib import Path

HERE=Path(__file__).resolve().parent
PINS={'inference-once90.py':'b841cf6557046ada96b67564be7e093dcb0666980577de8ab37273dd0c22b1ab',
      'inference-once-core.py':'c9fa6329849ed28713bbfb0fb2f7ded043a04c721c02c10c80fb71d5206adcdf',
      'InferenceJob90.cs':'93a9cd26ee7bf268e9327cb10bed92cc27d273ffc697e9da5b679675cca846cf'}
BASE='d9fa4f7c4e7eeb8f78c34ab6bc19ec92d4595635'


def sha(path):return hashlib.sha256(path.read_bytes()).hexdigest()


def prepare():
    for name,pin in PINS.items():
        if sha(HERE/name)!=pin:raise RuntimeError('OWNER_APPROVED_AUTHFIX_SOURCE_SHA_HOLD')
    proof=Path('C:/ProgramData/LimitlessRelay/InferenceLogReviews/93fa150e749d4d6ea661d6040e3aed8b/Output/projection.json')
    if sha(proof)!='20b0e1b09c31b3f772b46f773448e4d30662482a839c21e1e0f58816824fcb15':raise RuntimeError('AUTHFIX_SERVICE_PROOF_SHA_HOLD')
    value=json.loads(proof.read_text(encoding='utf8'))
    if (value['current_auth_config_metadata']['sha256']!='818c2e7b52f1c574fd1f9e0ac49dd876ad10746ff5ad95ce427d636938747bc3'
        or value['current_auth_config_metadata']['credential_store']!='keyring'
        or not value['service_sid_matches']):raise RuntimeError('AUTHFIX_EXISTING_KEYRING_HOLD')
    spec=importlib.util.spec_from_file_location('prepare_shared',HERE/'prepare-inference-once90.py')
    shared=importlib.util.module_from_spec(spec);spec.loader.exec_module(shared)
    shared.PREPARED=shared.REPO.parent/'InferenceTrial90AuthFixPrepared20261026'
    shared.EVIDENCE=shared.REPO/'문서/00_프로젝트/Evidence/Inference90AuthFixPrepared20261026'
    shared.prepare(launcher_name='run-approved-inference-authfix-once90.ps1',base_commit=BASE,
        prior_marker=Path('C:/ProgramData/LimitlessRelay/InferenceOnce90-717a0afa735d485daf790579b964e8c2.launch-consumed.json'),
        validation_results={'candidate_regression38_reused':True,'native_job7_reused':True,
                            'phase_budget_reused':True,'supported_controls11_false_reused':True,'full_suites_rerun':0})
    path=shared.PREPARED/'Manifest.json'
    manifest=json.loads(path.read_text(encoding='utf8'))
    if manifest['request_id'] in {'717a0afa735d485daf790579b964e8c2','e0d2e6fb9a6e48708b35052c02ad133c'}:raise RuntimeError('NEW_REQUEST_COLLISION_HOLD')
    manifest['owner_authorization_scope']='AUTHFIX_D9FA4F7C_NEW_READONLY_ONCE_20261026'
    manifest['approved_service_config_sha256']=value['current_auth_config_metadata']['sha256']
    raw=json.dumps(manifest,ensure_ascii=False,indent=2)+'\n'
    path.write_text(raw,encoding='utf8')
    (shared.EVIDENCE/'Manifest.PREPARED_NOT_LAUNCHED.json').write_text(raw,encoding='utf8')
    record_path=shared.EVIDENCE/'Preparation.json';record=json.loads(record_path.read_text(encoding='utf8'))
    record.update(owner_authorization_scope=manifest['owner_authorization_scope'],manifest_sha256=sha(path),
                  prior_failed_inference_request='717a0afa735d485daf790579b964e8c2',
                  service_config_sha256=value['current_auth_config_metadata']['sha256'],
                  service_proof_sha256=sha(proof),phase1_status='AUTHORIZED_NOT_STARTED',phase2_status='NOT_STARTED_AWAIT_VERIFIED',
                  phase2_conditionally_authorized=True)
    record['new_source_sha256']['prepare-inference-authfix-once90.py']=sha(Path(__file__))
    record_path.write_text(json.dumps(record,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
    print(json.dumps({'status':'AUTHFIX_APPROVED_NEW_TRIAL_READY_NOT_LAUNCHED','request_id':manifest['request_id'],
                     'archive_sha256':manifest['archive_sha256'],'manifest_sha256':record['manifest_sha256'],
                     'launcher_sha256':manifest['launcher_sha256'],'permit_active':False,'consumed':False}))


if __name__=='__main__':prepare()
