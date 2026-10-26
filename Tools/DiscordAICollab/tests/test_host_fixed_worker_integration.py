"""합성 Windows 토큰→실제 HostRuntime→실제 FixedPlanningWorker→sidecar를 fake Popen으로 검증합니다."""
import base64
import copy
import hashlib
import importlib.util
import io
import json
from pathlib import Path
import shutil
import socket
import struct
import subprocess
import tempfile
import unittest
from unittest.mock import patch

from collab.broker_host_runtime import HostRuntime
from collab import broker_host_runtime as runtime_module
from collab.fixed_planning_worker import FixedPlanningWorker
from collab.mobile_planning_buttons import contract

DIAG=Path(__file__).resolve().parents[2]/'DiscordAIServiceDiagnostics'
def module(path,name):
    spec=importlib.util.spec_from_file_location(name,path);value=importlib.util.module_from_spec(spec);spec.loader.exec_module(value);return value
sidecar=module(DIAG/'fixed-planning-entry.py','tested_fixed_entry')
core=module(DIAG/'inference-once-core.py','tested_frozen_core')


class IntegrationTests(unittest.TestCase):
    def setUp(self):
        scratch=Path('F:/study/codex/Project-Limitless/Temp/HostWorkerIntegration');scratch.mkdir(exist_ok=True)
        self.temp=tempfile.TemporaryDirectory(dir=scratch);self.root=Path(self.temp.name)
        self.install=self.root/'Install';diag=self.install/'Code/Tools/DiscordAIServiceDiagnostics';diag.mkdir(parents=True)
        shutil.copyfile(DIAG/'mobile-worker-contract.py',diag/'mobile-worker-contract.py')
        legacy=self.install/'Legacy/Tools/DiscordAIServiceDiagnostics/inference-once90.py';legacy.parent.mkdir(parents=True);legacy.write_bytes(b'FROZEN_FAKE_ONLY')
        state=self.root/'State';state.mkdir()
        self.now=1793005000.0;self.elapsed=0.0;self.calls=[];self.engine_calls=0;self.last_permit=None
        self.config={'execution_mode':'OWNER_APPROVED_READONLY_ONCE','bot_sid':'BOT','admin_sid':'ADMIN','worker_sid':'WORKER','state_root':str(state),
            'legacy_permit':{'approved':False,'entry_sha256':hashlib.sha256(legacy.read_bytes()).hexdigest(),'policy':copy.deepcopy(core.POLICY)}}
        self.context={'sid':'WORKER','session':0,'logon':4,'admin':False,'protected_install':True,'protected_state':True,'fixed_task':True,'manual':True,'approved_package':True}
        plan={'task_id':'L-host','role':'PlanningAgent','scope':contract.POLICY['scope'],'expected':contract.EXPECTED,'policy':contract.POLICY}
        self.ticket={'request_id':'b'*32,'task_id':'L-host','task_sha256':contract.digest(plan),'plan':plan,
            'owner':contract.OWNER,'application':contract.TRIAL_APP,'guild':contract.GUILD,'channel':contract.CHANNEL,
            'created':self.now,'deadline':self.now+90,'approved_until':self.now+300,'policy':contract.POLICY}
        self.rehash()
        self.failure=None
        self.worker=FixedPlanningWorker(self.install,self.config,clock=lambda:self.elapsed,popen=self.fake_popen)
        self.host=self.runtime(recover=True)
        self.guards=[patch.object(subprocess,'Popen',side_effect=AssertionError('REAL_PROCESS_FORBIDDEN')),
                     patch.object(socket,'socket',side_effect=AssertionError('NETWORK_FORBIDDEN'))]
        for guard in self.guards:guard.start()

    def tearDown(self):
        for guard in reversed(self.guards):guard.stop()
        self.temp.cleanup()

    def runtime(self,recover=False):
        return HostRuntime(self.install,self.config,context=self.context,utc=lambda:self.now+self.elapsed,clock=lambda:self.elapsed,worker=self.worker,recover=recover)

    def rehash(self):self.ticket['ticket_sha256']=contract.digest({k:v for k,v in self.ticket.items() if k!='ticket_sha256'})

    def send(self,op,value,*,admin=False,peer=None):
        key={'arm':'permit','submit':'ticket','cancel':'request_id','result':'request_id'}[op]
        message=json.dumps({'op':op,key:value},ensure_ascii=False).encode()
        context=peer or {'Sid':'ADMIN' if admin else 'BOT','Elevated':admin,'Administrator':admin,'Local':True,'Integrity':12288 if admin else 8192}
        return self.host.handle({'peer':context,'message':base64.b64encode(message).decode()})

    def arm(self):
        return self.send('arm',{'request_id':self.ticket['request_id'],'task_sha256':self.ticket['task_sha256'],'expires':self.now+300,'policy':contract.POLICY},admin=True)

    def fake_popen(self,command,**kwargs):
        self.calls.append(command)
        if self.failure=='start':raise OSError('synthetic')
        test=self
        class Input(io.BytesIO):
            def flush(stream):
                if getattr(stream,'processed',False):return
                stream.processed=True;raw=stream.getvalue();size=struct.unpack('<I',raw[:4])[0]
                payload=json.loads(raw[4:]);test.assertEqual(size,len(raw)-4)
                entry=sidecar.execute_payload(test.install,test.config,payload,engine_loader=test.engine_loader,utc=lambda:test.now+test.elapsed)
                child={'account':'PAUL-PC\\LIMITLESSRelaySvc','request_id':payload['ticket']['request_id'],'sid_matches':True,'session_id':0,'logon_type':4,
                       'status':entry['status'],'codex_home_unchanged':True,'entry_result':entry,
                       'job':{'exit_code':0,'timed_out':False,'cleanup_verified':True,'active_processes':0}}
                if test.failure=='missing':child={}
                process.stdout=io.BytesIO(json.dumps(child,ensure_ascii=False).encode());process.stderr=io.BytesIO(b'')
        class Process:
            returncode=0
            def __init__(process):process.stdin=Input()
            def wait(process,timeout):
                if test.failure=='timeout':test.elapsed+=timeout;raise subprocess.TimeoutExpired(command,timeout)
                return process.returncode
            def kill(process):process.returncode=-1;test.failure=None
        process=Process();return process

    def engine_loader(self,path,name):
        test=self
        class Engine:
            PERMIT=None
            def run(engine,request,budget):
                test.engine_calls+=1
                test.last_permit=json.loads(engine.PERMIT.read_text(encoding='utf8'))
                test.assertTrue(test.last_permit['approved']);test.assertEqual(request,test.last_permit['request_id'])
                test.assertEqual(core.POLICY,test.last_permit['policy'])
                events=[{'type':'thread.started'},{'type':'turn.started'},
                    {'type':'item.completed','item':{'type':'agent_message','text':contract.EXPECTED}},
                    {'type':'turn.completed','usage':{'input_tokens':10,'output_tokens':2}}]
                if test.failure=='forbidden':events.insert(2,{'type':'item.completed','item':{'type':'command_execution'}})
                checked=core.validate('\n'.join(json.dumps(e,ensure_ascii=False) for e in events).encode(),0,contract.EXPECTED)
                return {'request_id':request,'status':'VERIFIED' if checked['status']=='PASS' else 'NOT_VERIFIED','validation':checked,
                    'inference_exec_launches':1,'usage_app_server_launches':1,'application_retries':0,'mode_changed':False,
                    'credential_store_selection':'EXISTING_KEYRING_EXPLICIT',
                    'process':{'exit_code':0,'timed_out':False,'pipes_complete':True,'stdout_truncated':False,'stderr_truncated':False},
                    'official_usage_before':{'source':'CODEX_APP_SERVER_OFFICIAL','observed':test.now,'ordinary_allowed':True,
                        'windows':[{'minutes':n,'used_percent':0,'resets_at':test.now+1000} for n in (300,10080)]}}
        return Engine()

    def test_complete_real_branch_with_fake_engine_and_native_contract(self):
        self.assertEqual('ARMED',self.arm()['status']);result=self.send('submit',self.ticket)
        self.assertEqual('VERIFIED',result['status']);self.assertEqual(1,self.engine_calls)
        self.assertEqual(contract.EXPECTED,result['final_response'])
        self.assertIn('fixed-planning-worker.ps1',self.calls[0][4])
        self.assertNotIn('prompt',' '.join(self.calls[0]))

    def test_fake_install_never_imports_model_engine(self):
        self.config['execution_mode']='FAKE_ONLY';self.arm()
        self.assertEqual('HOLD',self.send('submit',self.ticket)['status']);self.assertEqual(0,self.engine_calls)
        proof=Path(self.config['state_root'])/'Workers'/self.ticket['request_id']/'safe-result.json'
        self.assertEqual('FAKE_FIXED_WORKER_COMPLETE',json.loads(proof.read_text(encoding='utf8'))['stage'])

    def test_same_sid_rejected_at_host_broker(self):
        self.host.broker.admin_sid='BOT';self.assertEqual('HOLD',self.arm()['status']);self.assertEqual([],self.calls)

    def test_untrusted_peer_cannot_execute(self):
        self.arm();peer={'Sid':'OTHER','Elevated':False,'Administrator':False,'Local':True,'Integrity':8192}
        self.assertEqual('HOLD',self.send('submit',self.ticket,peer=peer)['status']);self.assertEqual([],self.calls)

    def test_request_tamper_stops_before_fixed_worker(self):
        self.arm();self.ticket['channel']=1;self.rehash()
        self.assertEqual('HOLD',self.send('submit',self.ticket)['status']);self.assertEqual([],self.calls)

    def test_duplicate_does_not_launch_again(self):
        self.arm();self.send('submit',self.ticket);self.send('submit',self.ticket)
        self.assertEqual(1,len(self.calls));self.assertEqual(1,self.engine_calls)

    def test_expired_request_no_launch(self):
        self.arm();self.elapsed=91
        self.assertEqual('HOLD',self.send('submit',self.ticket)['status']);self.assertEqual([],self.calls)

    def test_cancel_before_launch(self):
        self.arm();self.send('cancel',self.ticket['request_id'])
        self.assertEqual('HOLD',self.send('submit',self.ticket)['status']);self.assertEqual([],self.calls)

    def test_start_failure_no_retry(self):
        self.arm();self.failure='start';self.send('submit',self.ticket);self.send('submit',self.ticket)
        self.assertEqual(1,len(self.calls))

    def test_forbidden_jsonl_never_verified(self):
        self.arm();self.failure='forbidden'
        self.assertEqual('HOLD',self.send('submit',self.ticket)['status'])

    def test_result_missing_never_verified(self):
        self.arm();self.failure='missing'
        self.assertEqual('HOLD',self.send('submit',self.ticket)['status'])

    def test_rpc_processes_do_not_recover_each_other(self):
        self.arm();self.host=self.runtime(recover=False)
        self.assertEqual('VERIFIED',self.send('submit',self.ticket)['status'])

    def test_host_restart_revokes_arm(self):
        self.arm();self.host=self.runtime(recover=True)
        self.assertEqual('HOLD',self.send('submit',self.ticket)['status']);self.assertEqual([],self.calls)

    def test_host_restart_retains_consumption(self):
        self.arm();self.send('submit',self.ticket);self.host=self.runtime(recover=True);self.send('submit',self.ticket)
        self.assertEqual(1,len(self.calls))

    def test_scoped_permit_deactivated_after_engine(self):
        self.arm();self.send('submit',self.ticket)
        path=Path(self.config['state_root'])/'Workers'/self.ticket['request_id']/'permit.json'
        self.assertFalse(json.loads(path.read_text(encoding='utf8'))['approved'])

    def test_timeout_does_not_reexecute(self):
        self.arm();self.failure='timeout';result=self.send('submit',self.ticket)
        self.assertEqual('HOLD',result['status']);self.send('submit',self.ticket);self.assertEqual(1,len(self.calls))

    def test_entry_rejects_non_batch_os_identity_before_broker(self):
        for value in ({'sid':'WORKER','session':9,'logon':2,'admin':False,'elevated':False},
                      {'sid':'WORKER','session':0,'logon':5,'admin':False,'elevated':False}):
            with patch.object(runtime_module,'verify_manifest',return_value=self.config),patch.object(runtime_module,'native_identity',return_value=value),patch.object(runtime_module,'HostRuntime',side_effect=AssertionError('MUST_NOT_CREATE_BROKER')):
                with self.assertRaises(ValueError):runtime_module.main(self.install,'initialize',b'{}')


if __name__=='__main__':unittest.main()
