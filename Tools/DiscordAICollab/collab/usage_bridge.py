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
    def __init__(self,executable=None):
        self.executable=executable or shutil.which('codex')

    async def read(self):
        if not self.executable:raise Denied('CODEX_CLI_NOT_INSTALLED')
        process=None
        try:
            async with asyncio.timeout(20):
                process=await asyncio.create_subprocess_exec(self.executable,'app-server','--listen','stdio://',
                    stdin=asyncio.subprocess.PIPE,stdout=asyncio.subprocess.PIPE,stderr=asyncio.subprocess.DEVNULL,
                    env=child_environment(),limit=1024*1024,
                    creationflags=getattr(subprocess,'CREATE_NO_WINDOW',0))
                async def rpc(method,number=None,params=None):
                    if method not in ALLOWED:raise Denied('RPC_METHOD_FORBIDDEN')
                    item={'method':method}
                    if number is not None:item['id']=number
                    if params is not None:item['params']=params
                    process.stdin.write((json.dumps(item)+'\n').encode());await process.stdin.drain()
                    if number is None:return None
                    for _ in range(64):
                        line=await process.stdout.readline()
                        if not line:raise Denied('APP_SERVER_CLOSED_BEFORE_RESPONSE')
                        try:event=json.loads(line)
                        except (ValueError,UnicodeError):raise Denied('APP_SERVER_PROTOCOL_INVALID') from None
                        if event.get('id')==number:
                            if 'error' in event:raise Denied('OFFICIAL_READ_REJECTED')
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
                return normalize(raw,time.time())
        except Denied:raise
        except TimeoutError:raise Denied('OFFICIAL_USAGE_TIMEOUT') from None
        except Exception:raise Denied('OFFICIAL_USAGE_UNAVAILABLE') from None
        finally:
            if process:
                if process.stdin:process.stdin.close()
                try:await asyncio.wait_for(process.wait(),2)
                except TimeoutError:
                    process.terminate()
                    try:await asyncio.wait_for(process.wait(),2)
                    except TimeoutError:process.kill();await process.wait()


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
