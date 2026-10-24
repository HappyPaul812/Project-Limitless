"""부팅 계정에서 공식 읽기 조회를 통과한 경우에만 기존 A 재개 실행기를 연결합니다.
인증/계정/사용량 불명은 중계만 유지합니다. 로그인·자격 증명 복사·승인 발급·추론은 없습니다.
"""
import asyncio
import json
import os
from pathlib import Path
import shutil
import subprocess
import time

from .core import Denied
from .phase2 import child_environment
from .usage_bridge import OfficialUsage, UsageMonitor
from .scheduling import Scheduler
from .resume_queue import ResumeQueue

ROOT=Path(__file__).resolve().parent.parent
APPROVAL=Path('C:/ProgramData/LimitlessRelay/boot-approval.json')

def windows_context():
    """Windows가 보고하는 실행 토큰을 확인합니다. 환경변수의 사용자 이름만 믿지 않습니다."""
    command='$i=[Security.Principal.WindowsIdentity]::GetCurrent(); @{sid=$i.User.Value; name=$i.Name; profile=[Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile); admin=([Security.Principal.WindowsPrincipal]::new($i)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)} | ConvertTo-Json -Compress'
    try:
        result=subprocess.run(['pwsh','-NoProfile','-Command',command],capture_output=True,text=True,
            timeout=5,creationflags=getattr(subprocess,'CREATE_NO_WINDOW',0))
        if result.returncode:raise ValueError()
        return json.loads(result.stdout)
    except Exception:raise Denied('SERVICE_IDENTITY_NOT_VERIFIED') from None

def service_environment(context,approval,environ,root):
    """서비스 토큰·고정 배포 위치가 일치할 때 자기 프로필만 하위 프로세스에 전달합니다."""
    if approval.get('service_ai_bridge')!='READONLY_AUTH_AND_RESUME':raise Denied('SERVICE_BRIDGE_NOT_DEPLOYED')
    if (context.get('sid')!=approval.get('service_sid') or not context.get('sid')
        or str(context.get('name','')).split('\\')[-1]!='LIMITLESSRelaySvc' or context.get('admin') is not False):
        raise Denied('SERVICE_ACCOUNT_CONTEXT_REQUIRED')
    if Path(approval.get('tool_root','')).resolve()!=Path(root).resolve():raise Denied('SERVICE_DEPLOYMENT_ROOT_MISMATCH')
    profile=Path(environ.get('USERPROFILE',''))
    if (not profile.is_absolute() or not profile.name.lower().startswith('limitlessrelaysvc')
        or profile.resolve()!=Path(context.get('profile','')).resolve()):
        raise Denied('SERVICE_OWN_PROFILE_REQUIRED')
    home=profile/'.codex'
    if not home.is_dir():raise Denied('SERVICE_CODEX_PROFILE_NOT_INITIALIZED')
    if home.is_symlink() or home.resolve().parent!=profile.resolve():raise Denied('SERVICE_OWN_PROFILE_REQUIRED')
    env=child_environment(environ)
    # 상위 CODEX_HOME/HOME으로 다른 사용자의 로그인 저장소를 가리키지 못하게 합니다.
    for key in list(env):
        if key.lower() in ('codex_home','home','userprofile'):del env[key]
    env.update(USERPROFILE=str(profile),HOME=str(profile),CODEX_HOME=str(home))
    return env

def prepare_provider(root=ROOT,context=None,approval=None,environ=None,executable=None):
    context=windows_context() if context is None else context
    approval=json.loads(APPROVAL.read_text(encoding='utf8')) if approval is None else approval
    env=service_environment(context,approval,os.environ if environ is None else environ,root)
    binary=executable or shutil.which('codex',path=env.get('PATH',env.get('Path','')))
    if not binary:raise Denied('SERVICE_CODEX_CLI_NOT_FOUND')
    return OfficialUsage(binary,environment=env)

def probe(root=ROOT,provider_factory=None):
    """현재 계정의 공식 인증·5시간/주간 한도만 1회 읽고 정규화된 숫자만 반환합니다."""
    result={'status':'HOLD','reason':'SERVICE_READINESS_UNKNOWN','observed':time.time(),
            'actual_ai_calls':0,'api_fallback':False,'mode_changed':False}
    provider=None
    try:
        provider=(provider_factory or prepare_provider)(root)
        value=asyncio.run(provider.read())
        # ordinary_allowed=false나 소진도 인증 성공과 실행 허용을 구분합니다. 실행 여부는 기존 gate가 판단합니다.
        result.update(status='READONLY_LINK_READY',reason='OWNER_A_ACTIVATION_AND_EXACT_TASK_APPROVAL_REQUIRED',usage=value)
    except Exception as exc:
        reason=str(exc) if isinstance(exc,Denied) else 'SERVICE_READINESS_UNKNOWN'
        if reason.isascii() and reason.replace('_','').isalnum():result['reason']=reason
        provider=None
    return result,provider

def connect_boot_ai(store,data_root):
    """기존 Bot의 동일 lock/DB 안에서만 연결합니다. 새 Bot·Art·worker 프로세스를 만들지 않습니다."""
    result,provider=probe()
    Path(data_root,'service-ai-readiness.json').write_text(json.dumps(result,indent=2),encoding='utf8')
    if provider is None:return None,None,result
    scheduler=Scheduler(store)
    scheduler.record_official_usage(result['usage'],result['usage']['observed'])
    queue=ResumeQueue(store);queue.recover()
    return UsageMonitor(scheduler,provider),queue,result

def main():
    # 독립 프로브는 봇/감시/DB/AI를 시작하거나 모드를 바꾸지 않습니다.
    import argparse,re
    parser=argparse.ArgumentParser();parser.add_argument('--request-id',default='local')
    request=parser.parse_args().request_id
    if request!='local' and not re.fullmatch('[a-f0-9]{32}',request):raise SystemExit(2)
    result,_=probe();result['request_id']=request
    output=ROOT/'data/boot'/('service-ai-probe-'+request+'.json');output.parent.mkdir(parents=True,exist_ok=True)
    output.write_text(json.dumps(result,indent=2),encoding='utf8')
    print(json.dumps(result))

if __name__=='__main__':main()
