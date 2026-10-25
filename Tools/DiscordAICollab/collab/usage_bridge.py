"""공식 app-server 읽기 RPC만 사용합니다. 모델 turn·로그인·유료 API·리셋은 없습니다."""
import asyncio
import json
import math
import shutil
import subprocess
import time
from pathlib import Path

from .core import Denied
from .phase2 import child_environment
from .usage_diagnostics import MAX_STDERR, STAGES, safe_error, safe_text

ALLOWED={'initialize','initialized','account/read','account/rateLimits/read'}


def normalize(raw, observed):
    if not isinstance(raw,dict):raise Denied('OFFICIAL_USAGE_INVALID')
    bucket=(raw.get('rateLimitsByLimitId') or {}).get('codex') or raw.get('rateLimits')
    if not isinstance(bucket,dict) or bucket.get('limitId')!='codex':raise Denied('CODEX_BUCKET_UNKNOWN')
    windows=[]
    for name,minutes in (('primary',300),('secondary',10080)):
        window=bucket.get(name)
        if not isinstance(window,dict) or window.get('windowDurationMins')!=minutes:
            raise Denied('USAGE_WINDOW_UNKNOWN')
        value,reset=window.get('usedPercent'),window.get('resetsAt')
        if (type(value) not in (int,float) or not math.isfinite(value) or not 0<=value<=100
                or type(reset) not in (int,float) or not math.isfinite(reset) or reset<=observed):
            raise Denied('USAGE_VALUE_OR_RESET_UNKNOWN')
        windows.append({'minutes':minutes,'used_percent':value,'resets_at':reset})
    ordinary=raw.get('ordinaryUsageAllowed')
    if type(ordinary) is not bool:raise Denied('ORDINARY_USAGE_PERMISSION_UNKNOWN')
    return {'source':'CODEX_APP_SERVER_OFFICIAL','observed':observed,
            'ordinary_allowed':ordinary and bucket.get('spendControlReached') is not True and bucket.get('rateLimitReachedType') is None,
            'windows':windows}


class OfficialUsage:
    def __init__(self,executable=None,environment=None,diagnostics=None):
        self.executable=executable or shutil.which('codex')
        self.environment=child_environment() if environment is None else child_environment(environment)
        self.diagnostics=diagnostics

    async def read(self,stage='OFFICIAL_USAGE_READ'):
        if not self.executable:raise Denied('CODEX_CLI_NOT_INSTALLED')
        process=None
        stderr_task=None
        stdout_task=None
        stderr_buffer=bytearray()
        stderr_bytes=0
        record={'stage':stage if stage in STAGES else 'OFFICIAL_USAGE_READ',
                'location':'usage_bridge.OfficialUsage.read.spawn', 'rpc_method':None,'request_id':None,
                'status':'INCOMPLETE','reason':None,'process_exit_code':None,'termination':'NOT_STARTED'}
        # 환경 비밀은 마스킹에만 쓰며 로그나 app-server 환경에 추가하지 않습니다.
        import os
        secrets=[v for k,v in os.environ.items() if any(x in k.upper() for x in ('TOKEN','SECRET','PASSWORD','API_KEY','USERNAME','USERPROFILE'))]
        if self.diagnostics:
            try:self.diagnostics.prepare()
            except Exception:raise Denied('USAGE_DIAGNOSTIC_PREFLIGHT_HOLD') from None
        async def drain_stderr():
            nonlocal stderr_bytes
            while True:
                chunk=await process.stderr.read(4096)
                if not chunk:return
                stderr_bytes+=len(chunk)
                # 메모리 상한 뒤에도 읽어 버려서 stderr 파이프가 프로세스를 멈추지 않게 합니다.
                room=MAX_STDERR-len(stderr_buffer)
                if room>0:stderr_buffer.extend(chunk[:room])
        async def discard_stdout():
            while await process.stdout.read(4096):pass
        try:
            # 전체 20초 중 마지막 2초는 종료·파이프 정리에 남깁니다. 재실행은 없습니다.
            async with asyncio.timeout(18):
                process=await asyncio.create_subprocess_exec(self.executable,'app-server','--listen','stdio://',
                    stdin=asyncio.subprocess.PIPE,stdout=asyncio.subprocess.PIPE,stderr=asyncio.subprocess.PIPE,
                    env=self.environment,limit=65536,
                    creationflags=getattr(subprocess,'CREATE_NO_WINDOW',0))
                record['termination']='WAITING'
                stderr_task=asyncio.create_task(drain_stderr())
                async def rpc(method,number=None,params=None):
                    if method not in ALLOWED:raise Denied('RPC_METHOD_FORBIDDEN')
                    record.update(rpc_method=method,request_id=number,location='usage_bridge.OfficialUsage.read.rpc')
                    item={'method':method}
                    if number is not None:item['id']=number
                    if params is not None:item['params']=params
                    process.stdin.write((json.dumps(item)+'\n').encode());await process.stdin.drain()
                    if number is None:return None
                    for _ in range(64):
                        try:line=await process.stdout.readline()
                        except (ValueError,asyncio.LimitOverrunError):raise Denied('APP_SERVER_PROTOCOL_INVALID') from None
                        if not line:raise Denied('APP_SERVER_CLOSED_BEFORE_RESPONSE')
                        try:event=json.loads(line)
                        except (ValueError,UnicodeError):raise Denied('APP_SERVER_PROTOCOL_INVALID') from None
                        if not isinstance(event,dict):raise Denied('APP_SERVER_PROTOCOL_INVALID')
                        if type(event.get('id')) is int and event.get('id')==number:
                            if 'error' in event:
                                record['error']=safe_error(event['error'],secrets)
                                raise Denied('OFFICIAL_READ_REJECTED')
                            if 'result' not in event:raise Denied('APP_SERVER_PROTOCOL_INVALID')
                            return event.get('result')
                        # 서버의 추가 인증·attestation 요구에는 응답하지 않고 보류합니다.
                        if 'id' in event and 'method' in event:raise Denied('UNSUPPORTED_SERVER_REQUEST')
                    raise Denied('APP_SERVER_EVENT_LIMIT')
                await rpc('initialize',1,{'clientInfo':{'name':'limitless_usage_readonly','title':'LIMITLESS usage monitor','version':'1.0.0'}})
                await rpc('initialized',params={})
                account=await rpc('account/read',2,{'refreshToken':False})
                if not isinstance(account,dict) or (account.get('account') or {}).get('type')!='chatgpt':
                    raise Denied('CHATGPT_SUBSCRIPTION_REQUIRED')
                account=None
                raw=await rpc('account/rateLimits/read',3)
                record['location']='usage_bridge.OfficialUsage.read.normalize'
                value=normalize(raw,time.time())
                record['status']='PASS'
                return value
        except Denied as exc:
            record.update(status='HOLD',reason=str(exc))
            raise
        except TimeoutError:
            record.update(status='HOLD',reason='OFFICIAL_USAGE_TIMEOUT')
            raise Denied('OFFICIAL_USAGE_TIMEOUT') from None
        except Exception:
            record.update(status='HOLD',reason='OFFICIAL_USAGE_UNAVAILABLE')
            raise Denied('OFFICIAL_USAGE_UNAVAILABLE') from None
        finally:
            cleanup_failed=False
            if process:
                # RPC 수신이 끝난 뒤 stdout도 비워 wait()와 파이프 사이의 교착을 방지합니다.
                stdout_task=asyncio.create_task(discard_stdout())
                try:
                    if process.stdin:process.stdin.close()
                    try:
                        await asyncio.wait_for(process.wait(),0.8)
                        record['termination']='EXITED'
                    except TimeoutError:
                        record['termination']='TERMINATED_BY_READER'
                        process.terminate()
                        try:await asyncio.wait_for(process.wait(),0.5)
                        except TimeoutError:
                            record['termination']='KILLED_BY_READER'
                            process.kill()
                            await asyncio.wait_for(process.wait(),0.3)
                except (Exception,asyncio.CancelledError):
                    cleanup_failed=True
                    record['termination']='CLEANUP_NOT_VERIFIED'
                    try:process.kill()
                    except (OSError,ProcessLookupError):pass
                finally:
                    record['process_exit_code']=process.returncode
                    for task in (stderr_task,stdout_task):
                        if task and not task.done():
                            try:await asyncio.wait_for(asyncio.shield(task),0.1)
                            except (Exception,asyncio.CancelledError):task.cancel()
                        if task and task.done() and not task.cancelled():
                            try:task.result()
                            except Exception:cleanup_failed=True
            record.update(stderr=safe_text(stderr_buffer.decode('utf8',errors='replace'),4096,secrets),
                          stderr_bytes=stderr_bytes,stderr_truncated=stderr_bytes>MAX_STDERR)
            stderr_buffer.clear()
            if cleanup_failed:record.update(status='HOLD',reason='USAGE_PROCESS_CLEANUP_HOLD')
            exit_failed=process is not None and record['status']=='PASS' and process.returncode!=0
            if exit_failed:record.update(status='HOLD',reason='APP_SERVER_EXIT_NONZERO')
            log_failed=False
            if self.diagnostics:
                try:self.diagnostics.write(record)
                except Exception:log_failed=True
            # 성공 응답이어도 종료/로그 보존이 불명확하면 실행 허용으로 이어지지 않습니다.
            if log_failed:raise Denied('USAGE_DIAGNOSTIC_WRITE_HOLD') from None
            if cleanup_failed:raise Denied('USAGE_PROCESS_CLEANUP_HOLD') from None
            if exit_failed:raise Denied('APP_SERVER_EXIT_NONZERO') from None


class UsageMonitor:
    """사용량 조회만 자동화합니다. 모델 실행 권한은 생성하지 않습니다."""
    def __init__(self,scheduler,provider=None):
        self.scheduler=scheduler;self.store=scheduler.store;self.provider=provider or OfficialUsage()
        self.lock=asyncio.Lock()

    async def refresh(self,force=False):
        async with self.lock:
            with self.store.tx() as db:
                row=db.execute("SELECT value FROM control_settings WHERE key='usage_monitor'").fetchone()
                state=json.loads(row[0]) if row else {'failures':0,'next_check':0,'circuit':'CLOSED'}
            now=self.store.clock()
            if not force and (state['circuit']=='OPEN' or now<state['next_check']):return state
            if force:state['failures']=0;state['circuit']='CLOSED'
            try:
                value=await self.provider.read()
                self.scheduler.record_official_usage(value,value['observed'])
                exhausted=[w['resets_at'] for w in value['windows'] if w['used_percent']>=100]
                state={'failures':0,'circuit':'CLOSED','next_check':max(exhausted) if exhausted else now+120,
                       'status':'EXHAUSTED' if exhausted else ('OBSERVED' if value['ordinary_allowed'] else 'BLOCKED'),'last_success':self.store.clock()}
            except Exception as exc:
                reason=str(exc) if isinstance(exc,Denied) and str(exc).isascii() and str(exc).replace('_','').isalnum() else 'OFFICIAL_USAGE_UNKNOWN'
                count=state['failures']+1
                self.scheduler.invalidate_usage(reason)
                state={'failures':count,'circuit':'OPEN' if count>=3 else 'CLOSED',
                       'next_check':now+(60 if count==1 else 300),'status':'UNKNOWN','reason':reason}
            with self.store.tx() as db:
                db.execute("INSERT OR REPLACE INTO control_settings VALUES('usage_monitor',?)",(json.dumps(state),))
            return state
