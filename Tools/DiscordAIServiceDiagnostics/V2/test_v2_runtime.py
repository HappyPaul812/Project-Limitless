"""새 V2 runtime/transport/Worker 접합만 검사. OS 계정/실제 IPC/Worker 생성은 금지합니다."""
import copy
import io
import json
from pathlib import Path
import socket
import sqlite3
import subprocess
import tempfile
import unittest
from unittest.mock import patch
from v2_contract import manifest,binding,sha,CODE,STATE,ACCOUNT,TASK,BOOT_SHA,OLD_SID
from v2_runtime import Runtime,project_worker
from v2_transport import Transport
from v2_worker import Worker

class FakeJob:
    def __init__(self,owner):self.owner=owner;self.closed=0
    def poll(self,seconds):
        o=self.owner;o.t+=seconds
        if o.fault=='timeout':return None
        if o.fault=='exception':raise RuntimeError('SYNTHETIC_SECRET')
        if o.fault=='cancel':o.runtime.dispatch(o.bot,dict(o.common,op='cancel'))
        value={'schema':2,'request_id':o.common['request_id'],'epoch':o.m['epoch'],'status':'FAKE_ONLY_COMPLETE','model_calls':0,'usage_reads':0,'discord_posts':0,'automatic_retries':0,'job':{'exit_code':0,'active_processes':0,'cleanup_verified':True,'timed_out':False}}
        if o.fault=='secret':value['secret']='SYNTHETIC_SECRET'
        if o.fault=='real':value['status']='VERIFIED'
        if o.fault=='active':value['job']['active_processes']=1
        if o.fault=='unclean':value['job']['cleanup_verified']=False
        if o.fault=='wrong_epoch':value['epoch']='V1'
        if o.fault=='model':value['model_calls']=1
        if o.fault=='home':value['CODEX_HOME']='SYNTHETIC_SECRET'
        if o.fault=='over90':o.t=91
        return value
    def close(self,deadline):self.closed+=1;return self.owner.fault!='close_fail' and self.owner.t<=deadline

class FakeWorker:
    def __init__(self,owner):self.owner=owner;self.calls=0;self.last=None
    def start(self,request,epoch,deadline):
        self.calls+=1
        if self.owner.fault=='start':raise RuntimeError('SYNTHETIC_SECRET')
        self.last=FakeJob(self.owner);return self.last

class V2Tests(unittest.TestCase):
    def setUp(self):
        self.t=0.;self.fault=None
        self.guards=[patch.object(subprocess,'Popen',side_effect=AssertionError('REAL_PROCESS_FORBIDDEN')),patch.object(socket,'socket',side_effect=AssertionError('NETWORK_FORBIDDEN'))]
        for g in self.guards:g.start()
        scratch=Path(__file__).resolve().parents[3]/'Temp/V2OfflineScratch';scratch.mkdir(parents=True,exist_ok=True)
        # TemporaryDirectory 정리는 합성 fixture만 대상으로 하며 실제 설치/계정과 무관합니다.
        self.scratch=scratch.resolve();self.temp=tempfile.TemporaryDirectory(dir=str(self.scratch));self.root=Path(self.temp.name)
        self.files={}
        for n in ['Code/V2/session0-client-v2.ps1','Code/V2/Session0PipeBoundary.cs','Code/V2/V2Contract.cs']+['Code/V2/fake%d.txt'%x for x in range(6)]:
            p=self.root/n;p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(b'FAKE_NOT_EXECUTED');self.files[n]=sha(p.read_bytes())
        package='1'*32
        self.m={'schema':2,'kind':'LIMITLESS_SESSION0_V2_CANDIDATE','package_id':package,'epoch':'20261027-v2-'+package,'approval_id':'v2-install-'+package,'approved':False,'auto_start':False,'execution_mode':'FAKE_ONLY','max_seconds':90,'bot_account':ACCOUNT,'code_root':CODE,'state_root':STATE,'task_name':TASK,'pipe_name':'LimitlessPlanningV2-'+package,'boot_approval_sha256':BOOT_SHA,'files_sha256':self.files}
        self.raw=json.dumps(self.m).encode();manifest(self.raw)
        self.b={'schema':2,'kind':'LIMITLESS_V2_INSTALLED_BINDING','package_id':package,'epoch':self.m['epoch'],'approval_id':self.m['approval_id'],'manifest_sha256':sha(self.raw),'bot_sid':'S-1-5-21-111-222-333-1010','worker_sid':'S-1-5-21-111-222-333-1007','admin_sid':'S-1-5-21-111-222-333-1001','execution_mode':'FAKE_ONLY','auto_start':False};binding(json.dumps(self.b),self.m,sha(self.raw))
        self.worker=FakeWorker(self);self.db=self.root/'v2.sqlite'
        self.admin={'Sid':self.b['admin_sid'],'Elevated':True,'Administrator':True,'Local':True,'Integrity':12288}
        self.bot={'Sid':self.b['bot_sid'],'Elevated':False,'Administrator':False,'Local':True,'Integrity':8192}
        self.common={'request_id':'2'*32,'epoch':self.m['epoch'],'approval_id':self.m['approval_id']}
        self.runtime=self.make_runtime(True)
    def make_runtime(self,initialize=False):return Runtime(self.db,self.m,self.b,self.worker,utc=lambda:1000000+self.t,clock=lambda:self.t,initialize=initialize)
    def tearDown(self):
        self.assertEqual(self.root.resolve().parent,self.scratch)
        self.assertTrue(self.scratch.is_relative_to(Path(__file__).resolve().parents[3]))
        self.temp.cleanup()
        for g in reversed(self.guards):g.stop()
    def arm(self):return self.runtime.dispatch(self.admin,dict(self.common,op='arm',task_sha256='a'*64,expires=1000300))
    def submit(self):return self.runtime.dispatch(self.bot,dict(self.common,op='submit',task_sha256='a'*64,deadline=1000000+self.t+90))
    def test_joined_fake_job_result_not_AI_verified(self):
        self.assertEqual(self.arm()['status'],'ARMED');r=self.submit();self.assertEqual(r['status'],'FAKE_ONLY_COMPLETE_NOT_AI_VERIFIED');self.assertEqual(self.worker.calls,1);self.assertEqual(self.worker.last.closed,1)
        self.assertEqual(self.runtime.dispatch(self.bot,dict(self.common,op='result')),r)
    def test_duplicate_consumption_survives_new_runtime(self):
        self.arm();self.submit();self.runtime=self.make_runtime(True);self.assertEqual(self.submit()['status'],'HOLD');self.assertEqual(self.arm()['status'],'HOLD');self.assertEqual(self.worker.calls,1)
    def test_restart_revokes_unconsumed_arm(self):
        self.arm();self.runtime=self.make_runtime(True);self.assertEqual(self.submit()['status'],'HOLD');self.assertEqual(self.worker.calls,0)
    def test_v1_epoch_record_cannot_be_reused(self):
        self.arm()
        with sqlite3.connect(self.db) as db:db.execute("UPDATE meta SET epoch='V1'")
        with self.assertRaises(ValueError):self.make_runtime()
    def test_v1_approval_rejected_before_launch(self):
        self.arm();msg=dict(self.common,op='submit',task_sha256='a'*64,deadline=1000090);msg['approval_id']='INSTALL1';self.assertEqual(self.runtime.dispatch(self.bot,msg)['status'],'HOLD');self.assertEqual(self.worker.calls,0)
    def test_non_native_peer_schema_cannot_arm(self):
        p=dict(self.admin);p['sid']='SYNTHETIC_SECRET';self.assertEqual(self.runtime.dispatch(p,dict(self.common,op='arm',task_sha256='a'*64,expires=1000100))['status'],'HOLD')
    def test_unprivileged_arm(self):self.assertEqual(self.runtime.dispatch(self.bot,dict(self.common,op='arm',task_sha256='a'*64,expires=1000100))['status'],'HOLD')
    def test_deadline_over90_and_nan_denied(self):
        self.arm()
        for d in [1000091,float('nan'),float('inf'),True]:
            self.assertEqual(self.runtime.dispatch(self.bot,dict(self.common,op='submit',task_sha256='a'*64,deadline=d))['status'],'HOLD')
        self.assertEqual(self.worker.calls,0)
    def test_cancel_before_launch(self):
        self.arm();self.runtime.dispatch(self.bot,dict(self.common,op='cancel'));self.assertEqual(self.submit()['status'],'HOLD');self.assertEqual(self.worker.calls,0)
    def test_worker_failure_matrix_no_leak_retry_and_job_closed(self):
        for fault in ['timeout','exception','secret','real','active','unclean','wrong_epoch','model','home','over90','close_fail','cancel','start']:
            with self.subTest(fault=fault):
                self.db=self.root/(fault+'.sqlite');self.t=0;self.fault=fault;self.worker=FakeWorker(self);self.runtime=self.make_runtime(True);self.arm();r=self.submit()
                self.assertEqual(r['status'],'HOLD');self.assertNotIn('SYNTHETIC_SECRET',json.dumps(r));self.submit();self.assertEqual(self.worker.calls,1)
                with sqlite3.connect(self.db) as db:self.assertNotIn('SYNTHETIC_SECRET',str(db.execute('SELECT result FROM calls').fetchall()))
                if self.worker.last:self.assertEqual(self.worker.last.closed,1)
                self.assertLessEqual(self.t,91) # 91초 모의 완료는 반드시 HOLD입니다.
    def test_fake_90second_timeout_closes_within_budget(self):
        self.fault='timeout';self.arm();r=self.submit();self.assertEqual(r['status'],'HOLD');self.assertLessEqual(self.t,90);self.assertEqual(self.worker.last.closed,1)
    def test_manifest_schema_and_path_contract(self):
        for k,v in [('schema',1),('code_root',r'C:\Program Files\LimitlessRelayTrial'),('pipe_name','LimitlessPlanning-'+'1'*32),('approved',True),('auto_start',True),('execution_mode','REAL'),('max_seconds',91),('schema',True)]:
            with self.subTest(k=k,v=v):
                m=copy.deepcopy(self.m);m[k]=v
                with self.assertRaises((ValueError,TypeError)):manifest(json.dumps(m))
    def test_manifest_duplicate_key_and_file_alias_denied(self):
        with self.assertRaises(ValueError):manifest(self.raw.decode().replace('"schema": 2','"schema": 2,"schema": 2'))
        for n in ['Code/../x','Code/a:ADS','Code/a\\b','Code/CON.txt','Code/x.','Code//x']:
            m=copy.deepcopy(self.m);m['files_sha256'][n]='a'*64
            with self.assertRaises(ValueError):manifest(json.dumps(m))
    def test_binding_old_sid_schema_epoch_denied(self):
        for k,v in [('schema',1),('bot_sid',OLD_SID),('bot_sid',self.b['worker_sid']),('epoch','V1'),('manifest_sha256','f'*64),('auto_start',True)]:
            b=copy.deepcopy(self.b);b[k]=v
            with self.assertRaises(ValueError):binding(json.dumps(b),self.m,sha(self.raw))
    def test_transport_pins_epoch_and_channel_no_legacy_command(self):
        calls=[]
        class Reply:
            returncode=0
            def communicate(self,raw,timeout):return b'{"schema":2,"status":"HOLD"}',None
        def popen(command,**kw):calls.append(command);return Reply()
        t=Transport(self.root,self.m,self.b,sha(self.raw),'b'*64,popen=popen,utc=lambda:1000000)
        self.assertEqual(t.exchange(dict(self.common,op='submit'),1000090)['status'],'HOLD');self.assertEqual(len(calls),1)
        self.assertIn(self.m['pipe_name']+'-work',calls[0]);self.assertNotIn('-ServiceSid',calls[0]);self.assertIn('-BindingSha',calls[0]);self.assertNotIn('LimitlessPlanning-', ' '.join(calls[0]))
    def test_transport_tampered_client_stops_before_popen(self):
        (self.root/'Code/V2/session0-client-v2.ps1').write_bytes(b'TAMPER')
        t=Transport(self.root,self.m,self.b,sha(self.raw),'b'*64,popen=lambda *a,**k:self.fail('MUST_NOT_LAUNCH'),utc=lambda:1000000)
        with self.assertRaises(ValueError):t.exchange(dict(self.common,op='submit'),1000090)
    def test_real_worker_command_only_fake_popen_injected(self):
        calls=[]
        class Process:
            returncode=0
            def __init__(self):self.stdin=io.BytesIO();self.stdout=io.BytesIO(b'{"schema":2,"status":"HOLD"}');self.stderr=io.BytesIO(b'')
            def wait(self,timeout):return 0
        def popen(command,**kw):calls.append(command);return Process()
        w=Worker(self.root,sha(self.raw),'b'*64,clock=lambda:0,popen=popen);h=w.start('2'*32,self.m['epoch'],88);self.assertEqual(h.poll(0.05)['status'],'HOLD');self.assertTrue(h.close(90));self.assertEqual(len(calls),1);self.assertIn('fixed-worker-v2.ps1',str(calls[0][4]));self.assertIn('-BindingSha',calls[0])
    def test_fixed_entry_reads_only_consumed_running_request(self):
        import importlib.util
        import v2_runtime
        spec=importlib.util.spec_from_file_location('tested_v2_entry',Path(v2_runtime.__file__).with_name('entry-v2.py'));entry=importlib.util.module_from_spec(spec);spec.loader.exec_module(entry)
        with self.runtime.db() as db:db.execute('INSERT INTO calls VALUES(?,NULL,?)',(self.common['request_id'],'RUNNING'))
        result=entry.fake_value(self.db,self.common['request_id'],self.m['epoch'],80000,self.m['approval_id']);self.assertEqual(result['status'],'FAKE_ONLY_COMPLETE');self.assertEqual(result['model_calls'],0)
        with self.assertRaises(ValueError):entry.fake_value(self.db,self.common['request_id'],'V1',80000,self.m['approval_id'])
        with self.assertRaises(ValueError):entry.fake_value(self.db,self.common['request_id'],self.m['epoch'],80000,'INSTALL1')

if __name__=='__main__':unittest.main(verbosity=2)
