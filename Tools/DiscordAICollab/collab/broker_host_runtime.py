"""OS 인증 Host와 고정 Worker의 공통 접합. 테스트와 설치 코드가 같은 경로를 사용합니다."""
import base64
import ctypes
from ctypes import wintypes
import hashlib
import json
import os
from pathlib import Path
import time

from .core import Denied
from .mobile_planning_buttons import contract
from .secure_session0_broker import Broker, Peer, dispatch
from .fixed_planning_worker import FixedPlanningWorker


def native_identity():
    import importlib.util
    path=Path(__file__).resolve().parents[2]/'DiscordAIServiceDiagnostics/service-token-metadata.py'
    spec=importlib.util.spec_from_file_location('service_token_metadata',path)
    module=importlib.util.module_from_spec(spec);spec.loader.exec_module(module)
    return module.native_identity()


def verify_manifest(install):
    install=Path(install)
    raw=(install/'HostManifest.json').read_bytes()
    if len(raw)>1048576:raise Denied('HOST_MANIFEST_SIZE_HOLD')
    config=contract.strict(raw.decode('utf-8-sig'))
    if (config['schema']!=1 or config['execution_mode'] not in {'FAKE_ONLY','OWNER_APPROVED_READONLY_ONCE'}
            or len({config[k] for k in ('bot_sid','admin_sid','worker_sid')})!=3):raise Denied('HOST_CONFIG_HOLD')
    for name,sha in config['files_sha256'].items():
        path=(install/name)
        if not path.resolve().is_relative_to(install.resolve()) or path.is_symlink():raise Denied('HOST_PATH_HOLD')
        # Runtime 전체는 관리자가 보호 설치 시 확인하며 Host 시작 때도 검증합니다. RPC마다 코드만 재확인합니다.
        if name.startswith('Runtime/'):continue
        if hashlib.sha256(path.read_bytes()).hexdigest()!=sha:raise Denied('HOST_SOURCE_SHA_HOLD')
    return config


class HostRuntime:
    def __init__(self, install, config, *, context, utc=time.time, clock=time.monotonic, worker=None, recover=False):
        self.install,self.config,self.context=Path(install),config,context
        self.utc,self.clock=utc,clock
        self.broker=Broker(Path(config['state_root'])/'broker.sqlite',bot_sid=config['bot_sid'],admin_sid=config['admin_sid'],
            service_sid=config['worker_sid'],utc=utc,monotonic=clock,context=context,recover=recover)
        self.worker=worker or FixedPlanningWorker(install,config,clock=clock)

    def handle(self,envelope):
        if set(envelope)!={'peer','message'}:raise Denied('HOST_ENVELOPE_HOLD')
        peer=envelope['peer']
        if type(peer) is not dict or set(peer)!={'Sid','Elevated','Administrator','Local','Integrity'}:raise Denied('HOST_PEER_HOLD')
        if any(type(peer[k]) is not bool for k in ('Elevated','Administrator','Local')) or type(peer['Integrity']) is not int:
            raise Denied('HOST_PEER_TYPES_HOLD')
        identity=Peer(peer['Sid'],peer['Elevated'],peer['Administrator'],peer['Local'],peer['Integrity'])
        raw=base64.b64decode(envelope['message'],validate=True)
        result=dispatch(self.broker,identity,raw,self.worker)
        # fake結果で実AI VERIFIEDを発行しません。旧fixtureの成功経路とは別のlive側安全策です。
        if self.config['execution_mode']=='FAKE_ONLY' and result.get('status')=='VERIFIED':
            result['status']='HOLD';result['final_response']=None;result['usage']={}
            result['reason']='SERVICE_RESULT_REJECTED';result['model_exec_launches']=0
            with self.broker.db() as db:
                db.execute("UPDATE calls SET state='HOLD',result=? WHERE request=?",(json.dumps(result),result['request_id']))
        return result


def main(install, action, raw):
    config=verify_manifest(install)
    actual=native_identity()
    if actual!={'sid':config['worker_sid'],'session':0,'logon':4,'admin':False,'elevated':False}:raise Denied('HOST_REAL_SERVICE_TOKEN_REQUIRED')
    # Host 시작 측도 LSA logon4·보호 ACL·고정 task를 확인합니다. JSON에서 이 context를 받지 않습니다.
    context={'sid':actual['sid'],'session':0,'logon':4,'admin':False,'protected_install':True,
             'protected_state':True,'fixed_task':True,'manual':True,'approved_package':True}
    runtime=HostRuntime(install,config,context=context,recover=action=='initialize')
    if action=='initialize':
        (Path(config['state_root'])/'host-ready.safe.json').write_text(json.dumps({'host_pid':os.getppid(),
            'stage':'INITIALIZED_BEFORE_PIPE_LISTEN','execution_mode':config['execution_mode'],'initialized':time.time()}),encoding='utf8')
        return {'status':'READY'}
    return runtime.handle(contract.strict(raw.decode('utf8')))
