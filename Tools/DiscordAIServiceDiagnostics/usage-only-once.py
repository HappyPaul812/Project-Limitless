"""별도 승인된 사용량 읽기만 1회 수행합니다. 모델/Discord/SQLite/운영 모드 코드는 호출하지 않습니다."""
import argparse
import asyncio
import hashlib
import json
import math
import os
from pathlib import Path
import re
import sys
import time

SCRIPT=Path(__file__).resolve()
TOOL=SCRIPT.parents[2]/'Tools/DiscordAICollab'
sys.path.insert(0,str(TOOL))
from collab.core import Denied
from collab.service_ai import prepare_provider, windows_context
from collab.usage_diagnostics import ProtectedUsageLog, no_link

BOOT=Path('C:/ProgramData/LimitlessRelay/boot-approval.json')
PERMIT=Path('C:/ProgramData/LimitlessRelay/usage-only-approval.json')
CHANGED={'collab/usage_bridge.py','collab/service_ai.py','collab/service_batch_once.py'}
NEW='collab/usage_diagnostics.py'
LIMITS={'max_usage_reads':1,'max_seconds':20,'automatic_retries':0,'model_calls':0,'discord_posts':0,'sqlite_operations':0}


def digest(path,deadline,clock):
    no_link(path)
    h=hashlib.sha256()
    with path.open('rb') as stream:
        for block in iter(lambda:stream.read(1024*1024),b''):
            if clock()>=deadline:raise Denied('USAGE_ONLY_DEADLINE_HOLD')
            h.update(block)
    return h.hexdigest()


def read_json(path):
    no_link(path)
    with path.open('rb') as stream:
        raw=stream.read(65537)
    if len(raw)>65536:raise Denied('USAGE_ONLY_APPROVAL_SIZE_HOLD')
    def unique(pairs):
        result={}
        for key,value in pairs:
            if key in result:raise Denied('USAGE_ONLY_DUPLICATE_APPROVAL_KEY_HOLD')
            result[key]=value
        return result
    value=json.loads(raw,object_pairs_hook=unique)
    if not isinstance(value,dict):raise Denied('USAGE_ONLY_APPROVAL_FORMAT_HOLD')
    return value


def private_home(base,context):
    """사전 검사 실패도 자기 프로필에만 기록합니다. 이 확인 자체는 실행 승인이 아닙니다."""
    profile=Path(os.environ.get('USERPROFILE',''))
    if (not context.get('sid') or context['sid']!=base.get('service_sid')
            or str(context.get('name','')).split('\\')[-1]!='LIMITLESSRelaySvc'
            or context.get('admin') is not False or not profile.is_absolute()
            or not profile.name.lower().startswith('limitlessrelaysvc')
            or profile.resolve()!=Path(context.get('profile','')).resolve()):
        raise Denied('USAGE_ONLY_SERVICE_IDENTITY_HOLD')
    return profile/'.codex'


def validate(base,permit,request_id,deadline,clock):
    required={'approved','kind','request_id','expires_at','boot_approval_sha256','trial_tool_root',
              'entry_sha256','roles_sha256','runtime_sha256','cli_sha256','mode','limits'}
    if set(permit)!=required or permit.get('approved') is not True or permit.get('kind')!='USAGE_ONLY_ONCE':
        raise Denied('USAGE_ONLY_OWNER_APPROVAL_HOLD')
    if permit['request_id']!=request_id or permit['mode']!='MANUAL':raise Denied('USAGE_ONLY_SCOPE_HOLD')
    limits=permit['limits']
    if not isinstance(limits,dict) or set(limits)!=set(LIMITS) or any(type(limits[k]) is not int or limits[k]!=v for k,v in LIMITS.items()):
        raise Denied('USAGE_ONLY_LIMITS_HOLD')
    expiry=permit['expires_at'];now=time.time()
    if type(expiry) not in (int,float) or not math.isfinite(expiry) or not now<expiry<=now+600:
        raise Denied('USAGE_ONLY_APPROVAL_EXPIRED_HOLD')
    if permit['boot_approval_sha256']!=digest(BOOT,deadline,clock):raise Denied('USAGE_ONLY_BOOT_APPROVAL_CHANGED_HOLD')
    if Path(permit['trial_tool_root']).resolve()!=TOOL.resolve():raise Denied('USAGE_ONLY_ROOT_HOLD')
    if permit['entry_sha256']!=digest(SCRIPT,deadline,clock):raise Denied('USAGE_ONLY_ENTRY_SHA_HOLD')
    if permit['roles_sha256']!=digest(TOOL/'roles.json',deadline,clock):raise Denied('USAGE_ONLY_ROLES_SHA_HOLD')
    old=base.get('runtime_sha256',{});candidate=permit['runtime_sha256']
    # 42 Python + 기존 PSM1을 보존하고 검토된 모듈 한 개만 추가합니다. wildcard 확장은 없습니다.
    if (len(old)!=43 or sum(k.startswith('collab/') and k.endswith('.py') for k in old)!=42
            or 'DiscordSecretStore.psm1' not in old or NEW in old or not CHANGED<=old.keys()
            or not isinstance(candidate,dict) or set(candidate)!=set(old)|{NEW}):
        raise Denied('USAGE_ONLY_RUNTIME_SET_HOLD')
    if {f'collab/{p.name}' for p in (TOOL/'collab').glob('*.py')}!={k for k in candidate if k.startswith('collab/')}:
        raise Denied('USAGE_ONLY_RUNTIME_COUNT_HOLD')
    for relative,expected in candidate.items():
        path=TOOL/relative
        if not path.resolve().is_relative_to(TOOL.resolve()):raise Denied('USAGE_ONLY_PATH_HOLD')
        no_link(path.parent)
        if relative not in CHANGED|{NEW} and expected!=old[relative]:raise Denied('USAGE_ONLY_UNCHANGED_SHA_HOLD')
        if digest(path,deadline,clock)!=expected:raise Denied('USAGE_ONLY_RUNTIME_SHA_HOLD')
    pin=base.get('service_codex',{})
    if not pin.get('path') or permit['cli_sha256']!=pin.get('sha256') or digest(Path(pin['path']),deadline,clock)!=pin.get('sha256'):
        raise Denied('USAGE_ONLY_CLI_SHA_HOLD')


def consume(home,request_id):
    folder=home/'limitless-usage-once'
    no_link(folder);folder.mkdir(exist_ok=True)
    slot=folder/(request_id+'.consumed');no_link(slot)
    try:
        with slot.open('x') as stream:stream.write('USAGE_ONLY_ONCE')
    except FileExistsError:raise Denied('USAGE_ONLY_ALREADY_CONSUMED_HOLD') from None


async def read_once(provider,budget):
    # 남은 전체 시간에서도 종료 처리용 2초를 예약합니다. 내부 18초보다 짧아질 수 있습니다.
    return await asyncio.wait_for(provider.read(stage='OFFICIAL_USAGE_READ'),budget)


def run(request_id,clock=None):
    clock=clock or time.monotonic
    deadline=clock()+20
    result={'status':'HOLD','reason':'USAGE_ONLY_NOT_VERIFIED','usage_read_attempts':0,
            'model_calls':0,'discord_posts':0,'sqlite_operations':0,'automatic_retries':0,'mode_changed':False}
    log=None;record={'stage':'USAGE_ONLY_PREFLIGHT','location':'usage-only-once.run','status':'HOLD','reason':None}
    try:
        if not isinstance(request_id,str) or not re.fullmatch('[a-f0-9]{32}',request_id):raise Denied('USAGE_ONLY_REQUEST_ID_HOLD')
        base=read_json(BOOT)
        context=windows_context()
        home=private_home(base,context)
        log=ProtectedUsageLog(home);log.prepare()
        permit=read_json(PERMIT)
        validate(base,permit,request_id,deadline,clock)
        # 별도 승인된 trial 루트만 허용합니다. 원본 boot approval 파일/운영 경로는 수정하지 않습니다.
        trial_policy={**base,'tool_root':str(TOOL)}
        provider=prepare_provider(root=TOOL,context=context,approval=trial_policy)
        if Path(provider.executable)!=Path(base['service_codex']['path']) or Path(provider.environment['CODEX_HOME'])!=home:
            raise Denied('USAGE_ONLY_PROVIDER_CONTEXT_HOLD')
        budget=min(18,deadline-clock()-2)
        if budget<=0:raise Denied('USAGE_ONLY_DEADLINE_HOLD')
        consume(home,request_id)
        result['usage_read_attempts']=1
        record['stage']='USAGE_ONLY_READ'
        value=asyncio.run(read_once(provider,budget))
        if clock()>=deadline:raise Denied('USAGE_ONLY_DEADLINE_HOLD')
        # 조회 성공은 업무 실행 승인이 아닙니다. 소진/불명 상태는 이 진입점에서도 HOLD합니다.
        if value.get('ordinary_allowed') is not True or any(w['used_percent']>=100 for w in value['windows']):
            raise Denied('USAGE_ONLY_QUOTA_HOLD')
        result.update(status='USAGE_ONLY_OBSERVED',reason='NO_MODEL_OR_MODE_CHANGE')
        record['status']='PASS'
    except TimeoutError:result['reason']='USAGE_ONLY_TIMEOUT_HOLD'
    except Exception as exc:
        reason=str(exc) if isinstance(exc,Denied) else 'USAGE_ONLY_PREFLIGHT_OR_READ_HOLD'
        result['reason']=reason if re.fullmatch('[A-Z_]+',reason) else 'USAGE_ONLY_HOLD'
    finally:
        record.update(reason=result['reason'],request_id=request_id if isinstance(request_id,str) and re.fullmatch('[a-f0-9]{32}',request_id) else None)
        if log:
            try:log.write(record)
            except Exception:result.update(status='HOLD',reason='USAGE_ONLY_DIAGNOSTIC_WRITE_HOLD')
    return result


def main(argv=None):
    parser=argparse.ArgumentParser();parser.add_argument('--request-id')
    request=parser.parse_args(argv).request_id
    result=run(request) if request else {'status':'HOLD','reason':'USAGE_ONLY_EXPLICIT_REQUEST_REQUIRED'}
    print(json.dumps(result))
    return 0 if result['status']=='USAGE_ONLY_OBSERVED' else 2


if __name__=='__main__':raise SystemExit(main())
