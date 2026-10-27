"""새 binding3와 기존 wire2의 결합만 검사합니다. Worker/Job/시계는 가짜입니다."""
import argparse,copy,hashlib,importlib.util,json,subprocess,socket,sys,tempfile
from pathlib import Path

def main(candidate,output):
    root=Path(candidate);sys.path.insert(0,str(root/'Code/V2'))
    import v2_contract as c
    from v2_runtime import Runtime
    rows=[]
    def check(name,fn):
        try: assert fn();rows.append({'name':name,'status':'PASS'})
        except Exception as e: rows.append({'name':name,'status':'FAIL','type':type(e).__name__})
    def denied(fn):
        try:fn();return False
        except (ValueError,KeyError,TypeError):return True
    def forbidden(*a,**k):raise AssertionError('REAL_EXECUTION_FORBIDDEN')
    subprocess.Popen=forbidden;subprocess.run=forbidden;socket.socket=forbidden
    raw=(root/'HostManifest.json').read_bytes();m=c.manifest(raw);ms=c.sha(raw)
    b=dict(schema=3,kind='DEDICATED_V2_OS_BINDING',package_id=m['package_id'],epoch=m['epoch'],approval_id=m['approval_id'],manifest_sha256=ms,bot_sid='S-1-5-21-1-2-3-1200',worker_sid='S-1-5-21-1-2-3-1201',admin_sid='S-1-5-21-1-2-3-1100',execution_mode='FAKE_ONLY',auto_start=False)
    check('BINDING3_NEW_ROLES',lambda:c.binding(json.dumps(b),m,ms)==b)
    for k,v in [('schema',2),('kind','V2_BINDING'),('worker_sid',c.OLD_WORKER),('worker_sid',b['bot_sid']),('admin_sid',b['worker_sid']),('epoch','old'),('approval_id','old'),('manifest_sha256','0'*64)]:
        bad=b|{k:v};check('BINDING_REJECT_'+k+'_'+str(v)[-4:],lambda bad=bad:denied(lambda:c.binding(json.dumps(bad),m,ms)))
    for k,v in [('schema',2),('approved',True),('auto_start',True),('real_ai_enabled',True),('execution_mode','AI'),('auth_policy','COPY'),('worker_account','LIMITLESSRelaySvc'),('code_root',r'C:\Program Files\LimitlessRelayTrial'),('max_seconds',91),('max_parallel_jobs',2),('worker_install_state','ENABLED_INTERACTIVE')]:
        bad=m|{k:v};check('MANIFEST_REJECT_'+k,lambda bad=bad:denied(lambda:c.manifest(json.dumps(bad))))
    check('NO_DUPLICATE_KEYS',lambda:denied(lambda:c.strict('{"a":1,"a":2}')))
    check('EXACT_ZIP_SOURCE_BYTES',lambda:all(c.sha((root/n).read_bytes())==h for n,h in m['files_sha256'].items()))
    for p in ['Code/../auth','Code/a:secret','Code/CON.txt','Code/a.','Code/abc\\x']:
        check('REJECT_PATH_'+p,lambda p=p:denied(lambda:c.relative(p)))
    admin=dict(Sid=b['admin_sid'],Elevated=True,Administrator=True,Local=True,Integrity=12288)
    bot=dict(Sid=b['bot_sid'],Elevated=False,Administrator=False,Local=True,Integrity=8192)
    class Clock:
        n=0
        def __call__(self):return self.n
    class Worker:
        def __init__(self,clock,mode):self.clock,self.mode,self.starts,self.closes,self.nested=clock,mode,0,0,None
        def start(self,req,epoch,deadline):
            self.starts+=1;self.req,self.epoch=req,epoch
            if self.mode=='start_error':raise PermissionError('DO_NOT_RECORD_THIS_SECRET')
            if self.nested:self.nested_result=self.nested()
            return self
        def poll(self,interval):
            self.clock.n+=interval
            if self.mode=='timeout':self.clock.n=91;return None
            return dict(schema=2,request_id=self.req,epoch=self.epoch,status='FAKE_ONLY_COMPLETE',model_calls=1 if self.mode=='AI' else 0,usage_reads=0,discord_posts=0,automatic_retries=0,job=dict(exit_code=0,active_processes=1 if self.mode=='child_alive' else 0,cleanup_verified=True,timed_out=False))
        def close(self,deadline):self.closes+=1;return self.mode!='cleanup_unknown'
    request='a'*32
    arm=dict(op='arm',request_id=request,epoch=m['epoch'],approval_id=m['approval_id'],task_sha256='b'*64,expires=1050)
    submit=dict(op='submit',request_id=request,epoch=m['epoch'],approval_id=m['approval_id'],task_sha256='b'*64,deadline=1090)
    # SQLite는 운영 DB와 무관한 자동 생성 Temp 안에서만 사용합니다.
    with tempfile.TemporaryDirectory(prefix='DedicatedV2Fake_',dir=str(root.parent)) as td:
        for i,mode in enumerate(['ok','start_error','timeout','AI','child_alive','cleanup_unknown','concurrent']):
            clock=Clock();w=Worker(clock,mode);db=Path(td)/str(i);r=Runtime(db,m,b,w,utc=lambda:1000,clock=clock,initialize=True)
            if mode=='concurrent':w.nested=lambda:r.dispatch(bot,submit)
            check('ARM_'+mode,lambda:r.dispatch(admin,arm)['status']=='ARMED')
            result=r.dispatch(bot,submit)
            check('FAKE_JOB_'+mode,lambda result=result,mode=mode:result['status']==('FAKE_ONLY_COMPLETE_NOT_AI_VERIFIED' if mode in ('ok','concurrent') else 'HOLD'))
            check('SINGLE_CONSUME_'+mode,lambda:r.dispatch(bot,submit)['status']=='HOLD' and w.starts==1)
            check('NO_SECRET_'+mode,lambda:'DO_NOT_RECORD_THIS_SECRET' not in json.dumps(result))
            if mode!='start_error':check('OWN_JOB_CLOSE_'+mode,lambda:w.closes==1)
            if mode=='concurrent':check('CONCURRENT_SUBMIT_BLOCKED',lambda:w.nested_result['status']=='HOLD')
        w=Worker(Clock(),'ok');r=Runtime(Path(td)/'identity',m,b,w,utc=lambda:1000,initialize=True)
        for sid in (c.OLD_WORKER,c.OLD_BOT,b['worker_sid']):check('OTHER_SID_ARM_'+sid[-4:],lambda sid=sid:r.dispatch(admin|{'Sid':sid},arm)['status']=='HOLD')
        check('OLD_EPOCH_ARM',lambda:r.dispatch(admin,arm|{'epoch':'old'})['status']=='HOLD')
        check('OLD_CONSUMPTION_DB_REJECT',lambda:denied(lambda:Runtime(Path(td)/'identity',m|{'epoch':'old'},b,w,initialize=True)))
    result=dict(schema=3,pass_count=sum(r['status']=='PASS' for r in rows),fail_count=sum(r['status']=='FAIL' for r in rows),tests=rows,actual_worker_calls=0,actual_ipc_calls=0,actual_ai_calls=0)
    Path(output).write_text(json.dumps(result,indent=2)+'\n',encoding='utf8');print(json.dumps({k:v for k,v in result.items() if k!='tests'}))
    if result['fail_count']:raise AssertionError('OFFLINE_FAILURE')
if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--candidate',required=True);p.add_argument('--output',required=True);a=p.parse_args();main(a.candidate,a.output)
