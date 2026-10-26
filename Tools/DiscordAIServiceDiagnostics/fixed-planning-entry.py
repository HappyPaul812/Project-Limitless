"""고정 작업의 유일한 모델 연결점. 기존 frozen entry/구독/검증기를 그대로 사용합니다."""
import copy
import hashlib
import importlib.util
import json
from pathlib import Path
import sqlite3
import sys
import time


def load(path,name):
    spec=importlib.util.spec_from_file_location(name,path)
    module=importlib.util.module_from_spec(spec);spec.loader.exec_module(module);return module


def execute_payload(install,config,payload,*,engine_loader=load,utc=time.time):
    install=Path(install)
    contract=load(install/'Code/Tools/DiscordAIServiceDiagnostics/mobile-worker-contract.py','fixed_contract')
    if set(payload)!={'ticket','budget_ms'}:raise ValueError('FIXED_PAYLOAD_HOLD')
    ticket=payload['ticket'];request=ticket['request_id']
    contract.validate_ticket(ticket,ticket,utc())
    if type(payload['budget_ms']) is not int or not 1000<=payload['budget_ms']<=75000:raise ValueError('FIXED_BUDGET_HOLD')
    # 별도 서비스 장부를 읽기 전용으로 확인합니다. 운영 업무 SQLite는 열지 않습니다.
    db=sqlite3.connect((Path(config['state_root'])/'broker.sqlite').resolve().as_uri()+'?mode=ro',uri=True)
    try:
        row=db.execute('SELECT c.ticket_sha,p.state,c.state FROM calls c JOIN permits p ON p.request=c.request WHERE c.request=?',(request,)).fetchone()
        if row!=(ticket['ticket_sha256'],'CONSUMED','RUNNING'):raise ValueError('FIXED_CONSUMPTION_HOLD')
    finally:db.close()
    private=Path(config['state_root'])/'Workers'/request
    private.mkdir(parents=True,exist_ok=False)
    (private/'started.json').write_text(json.dumps({'request_id':request,'ticket_sha256':ticket['ticket_sha256']}),encoding='utf8')
    if config['execution_mode']=='FAKE_ONLY':
        # 실제 entry를 import조차 하지 않습니다. 가짜 성공은 실제 VERIFIED가 될 수 없습니다.
        result={'request_id':request,'status':'NOT_VERIFIED','reason':'FAKE_FIXED_WORKER_COMPLETE',
                'stage':'FAKE_FIXED_WORKER_COMPLETE','inference_exec_launches':0,'usage_app_server_launches':0,
                'application_retries':0,'discord_posts':0,'sqlite_work_operations':0,'mode_changed':False}
    elif config['execution_mode']=='OWNER_APPROVED_READONLY_ONCE':
        legacy=install/'Legacy/Tools/DiscordAIServiceDiagnostics/inference-once90.py'
        if hashlib.sha256(legacy.read_bytes()).hexdigest()!=config['legacy_permit']['entry_sha256']:raise ValueError('FIXED_LEGACY_SHA_HOLD')
        permit=copy.deepcopy(config['legacy_permit'])
        permit.update(approved=True,request_id=request,expires_at=min(ticket['deadline'],utc()+90),
                      trial_tool_root=str(install/'Legacy/Tools/DiscordAICollab'))
        permit_path=private/'permit.json'
        with permit_path.open('x',encoding='utf8') as stream:json.dump(permit,stream)
        try:
            engine=engine_loader(legacy,'frozen_fixed_planning')
            # 기존 파일/검증을 바꾸지 않고 승인 파일 위치만 서비스 자기 시험 폴더에 바인딩합니다.
            engine.PERMIT=permit_path
            result=engine.run(request,min(payload['budget_ms'],max(1000,int((ticket['deadline']-utc()-2)*1000))))
        finally:
            permit['approved']=False;permit_path.write_text(json.dumps(permit),encoding='utf8')
    else:raise ValueError('FIXED_MODE_HOLD')
    (private/'safe-result.json').write_text(json.dumps(result,ensure_ascii=False),encoding='utf8')
    return result


if __name__=='__main__':
    install=Path(__file__).resolve().parents[3]
    try:
        config=json.loads((install/'HostManifest.json').read_text(encoding='utf-8-sig'))
        metadata=load(install/'Code/Tools/DiscordAIServiceDiagnostics/service-token-metadata.py','fixed_os_metadata').native_identity()
        if metadata!={'sid':config['worker_sid'],'session':0,'logon':4,'admin':False,'elevated':False}:raise ValueError('FIXED_REAL_OS_TOKEN_HOLD')
        # 실제 서비스 SID/Session은 wrapper가 OS 토큰+LSA로 검사한 뒤에만 이 고정 파일을 시작합니다.
        # 직접 봇에서 호출해도 서비스 전용 SQLite/Workers ACL과 소비 검사를 통과할 수 없습니다.
        raw=sys.stdin.buffer.read(16385)
        if len(raw)>16384:raise ValueError('FRAME_HOLD')
        result=execute_payload(install,config,json.loads(raw))
        print(json.dumps(result,ensure_ascii=False))
        raise SystemExit(0 if result.get('status')=='VERIFIED' or result.get('stage')=='FAKE_FIXED_WORKER_COMPLETE' else 2)
    except Exception:
        print('{"status":"NOT_VERIFIED","reason":"FIXED_ENTRY_HOLD_NO_RETRY"}')
        raise SystemExit(2)
