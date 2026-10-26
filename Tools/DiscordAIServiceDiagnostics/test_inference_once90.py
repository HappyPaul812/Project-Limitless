"""새 90초 경로의 오프라인 회귀만 검증합니다. 공식 CLI/인증/네트워크를 실행하지 않습니다."""
import importlib.util
import hashlib
import json
import os
from pathlib import Path
import sys
import tempfile
import time
import unittest
import uuid
from unittest.mock import patch

HERE = Path(__file__).resolve().parent
def load(name, path):
    spec=importlib.util.spec_from_file_location(name,path); value=importlib.util.module_from_spec(spec);spec.loader.exec_module(value);return value
m=load('inference_entry_test',HERE/'inference-once90.py'); c=m.core

def events(message=None, extra=None, usage=None):
    answer=c.EXPECTED if message is None else message
    value=[{'type':'thread.started','thread_id':'SYNTHETIC'}, {'type':'turn.started'}]
    if extra:value.append(extra)
    value += [{'type':'item.completed','item':{'id':'fixed','type':'agent_message','phase':'final_answer','text':answer}},
              {'type':'turn.completed','usage':usage if usage is not None else {'input_tokens':12,'output_tokens':8}}]
    return ('\n'.join(json.dumps(v,ensure_ascii=False) for v in value)+'\n').encode()


class ValidationTests(unittest.TestCase):
    def test_exact_sentence_and_usage(self):
        r=c.validate(events(),0,c.EXPECTED+'\n');self.assertEqual(r['status'],'PASS');self.assertEqual(r['completed_turns'],1)
    def test_wrong_sentence(self):self.assertEqual(c.validate(events('wrong'),0,'wrong')['status'],'NOT_VERIFIED')
    def test_final_file_required(self):self.assertEqual(c.validate(events(),0,None)['status'],'NOT_VERIFIED')
    def test_exit_nonzero(self):self.assertIn('EXIT_NONZERO',c.validate(events(),2,c.EXPECTED)['reason_codes'])
    def test_tool_rejected(self):self.assertIn('FORBIDDEN_ITEM',c.validate(events(extra={'type':'item.started','item':{'type':'command_execution'}}),0,c.EXPECTED)['reason_codes'])
    def test_failed_event_rejected(self):self.assertIn('CLI_ERROR_EVENT',c.validate(events(extra={'type':'turn.failed','error':{'code':401,'message':'denied'}}),0,c.EXPECTED)['reason_codes'])
    def test_unknown_event_rejected(self):self.assertIn('UNKNOWN_EVENT_TYPE',c.validate(events(extra={'type':'unexpected'}),0,c.EXPECTED)['reason_codes'])
    def test_duplicate_turn_rejected(self):self.assertEqual(c.validate(events()+events(),0,c.EXPECTED)['status'],'NOT_VERIFIED')
    def test_missing_usage_rejected(self):self.assertIn('USAGE_INVALID',c.validate(events(usage={}),0,c.EXPECTED)['reason_codes'])
    def test_invalid_utf8_rejected(self):self.assertEqual(c.validate(b'\xff',0,c.EXPECTED)['status'],'NOT_VERIFIED')
    def test_duplicate_json_key_rejected(self):self.assertEqual(c.validate(b'{"type":"x","type":"turn.completed"}\n',0,c.EXPECTED)['status'],'NOT_VERIFIED')
    def test_deadline_accounts_for_preparation(self):
        self.assertEqual(c.remaining(90,reserve=5,clock=lambda:70),15)
        with self.assertRaises(TimeoutError):c.remaining(90,reserve=5,minimum=5,clock=lambda:82)


class EntryTests(unittest.TestCase):
    def setUp(self):
        # Windows 경로 길이 제한이 시험 결과에 섞이지 않도록 짧은 전용 workspace 경로를 씁니다.
        scratch=Path('F:/study/codex/Project-Limitless/Temp/Inf90Offline');scratch.mkdir(parents=True,exist_ok=True)
        # 합성 보호 프로필은 증거로 남깁니다. sandbox에서 .codex 재귀삭제를 시도하지 않습니다.
        self.root=scratch/uuid.uuid4().hex;self.profile=self.root/'LIMITLESSRelaySvc';(self.profile/'.codex').mkdir(parents=True)
        (self.profile/'.codex/config.toml').write_bytes(b'cli_auth_credentials_store = "keyring"\r\nforced_login_method = "chatgpt"\r\n')
        self.folder=self.root/'Trial';self.tool=self.folder/'Package/Tools/DiscordAICollab';self.tool.mkdir(parents=True)
        (self.folder/'EmptyWorkspace').mkdir();self.archive=self.profile/'.codex/archive'
        self.base={'service_sid':'SYNTHETIC','service_codex':{'path':'NOT_AN_EXECUTABLE'}}
        self.request='c'*32;self.calls=[];self.mode='normal';self.log_count=0
        owner=self
        class Provider:
            executable='NOT_AN_EXECUTABLE'
            environment={'CODEX_HOME':str(owner.profile/'.codex')}
            async def read(self,stage):
                owner.calls.append('usage')
                if owner.mode=='usage_error':raise m.Denied('OFFICIAL_READ_REJECTED')
                return {'source':'CODEX_APP_SERVER_OFFICIAL','ordinary_allowed':owner.mode!='quota',
                        'observed':time.time()-(45 if owner.mode=='stale' else 0),
                        'windows':[{'minutes':300,'used_percent':5,'resets_at':time.time()+300},
                                   {'minutes':10080,'used_percent':10,'resets_at':time.time()+500}]}
        def archive(profile,request):self.archive.mkdir(exist_ok=False);return self.archive
        def collect(command,prompt,environment,cwd,deadline):
            self.calls.append('exec');self.assertEqual(prompt,c.PROMPT)
            self.assertNotIn('model_providers.openai',' '.join(command))
            self.assertIn('--sandbox',command);self.assertIn('read-only',command)
            self.assertIn('cli_auth_credentials_store="keyring"',command)
            self.assertFalse(any('TOKEN' in name or 'API_KEY' in name for name in environment))
            if self.mode=='no_budget':raise TimeoutError()
            (self.archive/'final-response.txt').write_text(c.EXPECTED,encoding='utf8')
            return {'stdout':events('bad') if self.mode=='invalid' else events(),'stderr':b'',
                    'stdout_truncated':self.mode=='flood','stderr_truncated':False,'timed_out':self.mode=='timeout',
                    'pipes_complete':True,'exit_code':0,'stdout_bytes':100,'stderr_bytes':0}
        def log(*args,**kwargs):
            self.log_count+=1
            if self.mode=='log_fail':raise OSError('synthetic')
        self.stack=unittest.mock.patch.multiple(m,TOOL=self.tool,
            read=lambda path:self.base if path==m.BOOT else {'cli_sha256':'0'*64},
            windows_context=lambda:{'name':'PAUL-PC\\LIMITLESSRelaySvc','sid':'SYNTHETIC'},
            verify=lambda *args:None,prepare_provider=lambda **kwargs:Provider(),prepare_archive=archive,save_execution_log=log)
        self.stack.start();self.addCleanup(self.stack.stop)
        self.env=patch.dict(os.environ,{'LIMITLESS_BATCH_LOGON_TYPE':'4','LIMITLESS_BATCH_SESSION_ID':'0'});self.env.start();self.addCleanup(self.env.stop)
        self.capture=patch.object(c,'collect',collect);self.capture.start();self.addCleanup(self.capture.stop)
        # fake read에는 await가 없습니다. 제한 sandbox의 loopback socketpair를 열 필요도 없습니다.
        def fake_run(coroutine):
            try: coroutine.send(None)
            except StopIteration as done: return done.value
            finally: coroutine.close()
            raise AssertionError('FIXTURE_MUST_NOT_AWAIT_NETWORK')
        self.async_runner=patch.object(m.asyncio,'run',fake_run);self.async_runner.start();self.addCleanup(self.async_runner.stop)
    def run_case(self):return m.run(self.request,75000)
    def test_success_once_only_and_cleanup(self):
        r=self.run_case();self.assertEqual(r['status'],'VERIFIED');self.assertEqual(self.calls,['usage','exec'])
        self.assertFalse((self.archive/'final-response.txt').exists())
        r=self.run_case();self.assertEqual(r['status'],'NOT_VERIFIED');self.assertEqual(self.calls,['usage','exec'])
    def test_usage_error_stops_before_exec(self):self.mode='usage_error';self.assertEqual(self.run_case()['status'],'NOT_VERIFIED');self.assertEqual(self.calls,['usage'])
    def test_quota_stops_before_exec(self):self.mode='quota';self.assertEqual(self.run_case()['status'],'NOT_VERIFIED');self.assertEqual(self.calls,['usage'])
    def test_stale_usage_stops_before_exec(self):self.mode='stale';self.assertEqual(self.run_case()['status'],'NOT_VERIFIED');self.assertEqual(self.calls,['usage'])
    def test_invalid_response_no_retry(self):self.mode='invalid';self.assertEqual(self.run_case()['status'],'NOT_VERIFIED');self.assertEqual(self.calls,['usage','exec'])
    def test_truncated_response_no_retry(self):self.mode='flood';self.assertEqual(self.run_case()['status'],'NOT_VERIFIED');self.assertEqual(self.calls,['usage','exec'])
    def test_timeout_no_retry(self):self.mode='timeout';self.assertEqual(self.run_case()['status'],'NOT_VERIFIED');self.assertEqual(self.calls,['usage','exec'])
    def test_log_failure_hold(self):self.mode='log_fail';self.assertEqual(self.run_case()['status'],'NOT_VERIFIED');self.assertEqual(self.calls,['usage','exec'])
    def test_wrong_service_context_no_usage(self):
        with patch.object(m,'windows_context',return_value={'name':'WRONG','sid':'WRONG'}):r=self.run_case()
        self.assertEqual(r['status'],'NOT_VERIFIED');self.assertEqual(self.calls,[])
    def test_no_budget_argument_no_usage(self):self.assertEqual(m.run(self.request,1)['status'],'NOT_VERIFIED');self.assertEqual(self.calls,[])
    def test_changed_auth_config_stops_before_usage(self):
        (self.profile/'.codex/config.toml').write_text('cli_auth_credentials_store="file"',encoding='utf8')
        r=self.run_case();self.assertEqual(r['reason'],'INFERENCE_EXISTING_AUTH_CONFIG_CHANGED_HOLD');self.assertEqual(self.calls,[])
    def test_missing_auth_config_stops_before_usage(self):
        (self.profile/'.codex/config.toml').unlink()
        self.assertEqual(self.run_case()['status'],'NOT_VERIFIED');self.assertEqual(self.calls,[])


class CaptureTests(unittest.TestCase):
    def test_both_pipes_drained_and_bounded(self):
        command=[sys.executable,'-c','import os;os.write(1,b"a"*400000);os.write(2,b"b"*100000)']
        r=c.collect(command,'',os.environ.copy(),HERE,time.monotonic()+5)
        self.assertTrue(r['pipes_complete']);self.assertEqual(r['exit_code'],0)
        self.assertEqual(len(r['stdout']),262144);self.assertEqual(len(r['stderr']),32768)
        self.assertTrue(r['stdout_truncated']);self.assertTrue(r['stderr_truncated'])
    def test_timeout_bounded(self):
        start=time.monotonic();r=c.collect([sys.executable,'-c','import time;time.sleep(10)'],'',os.environ.copy(),HERE,start+0.8)
        self.assertTrue(r['timed_out']);self.assertLess(time.monotonic()-start,2)


class PermitTests(unittest.TestCase):
    def setUp(self):
        self.root=Path('F:/study/codex/Project-Limitless/Temp/Inf90Offline')/uuid.uuid4().hex
        self.tool=self.root/'tool';(self.tool/'collab').mkdir(parents=True)
        self.sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
        names=sorted(m.CHANGED)+[f'collab/u{i}.py' for i in range(39)]+['DiscordSecretStore.psm1']
        old={}
        for name in names:
            p=self.tool/name;p.write_bytes(b'old');old[name]=self.sha(p)
            if name in m.CHANGED:p.write_bytes(b'changed')
        (self.tool/m.NEW).write_bytes(b'new')
        (self.tool/'roles.json').write_bytes(b'{}')
        self.cli=self.root/'not-executable';self.cli.write_bytes(b'fixture')
        self.base={'runtime_sha256':old,'service_codex':{'path':str(self.cli),'sha256':self.sha(self.cli)}}
        self.boot=self.root/'boot.json';self.boot.write_text(json.dumps(self.base))
        self.permit={'approved':True,'kind':'INFERENCE_ONCE90','request_id':'a'*32,'expires_at':time.time()+300,
          'boot_approval_sha256':self.sha(self.boot),'trial_tool_root':str(self.tool),'entry_sha256':self.sha(Path(m.__file__)),
          'core_sha256':self.sha(HERE/'inference-once-core.py'),'roles_sha256':self.sha(self.tool/'roles.json'),
          'runtime_sha256':{k:self.sha(self.tool/k) for k in set(old)|{m.NEW}},'cli_sha256':self.sha(self.cli),'policy':dict(c.POLICY)}
        self.targets=patch.multiple(m,BOOT=self.boot,TOOL=self.tool);self.targets.start();self.addCleanup(self.targets.stop)
    def verify(self):m.verify(self.base,self.permit,'a'*32,time.monotonic()+5)
    def test_exact44_and_unchanged40(self):self.verify()
    def test_bom_approval_file_reader(self):
        self.boot.write_text(json.dumps(self.base),encoding='utf-8-sig')
        self.assertEqual(m.read(self.boot),self.base)
    def test_inactive_permit_rejected(self):
        self.permit['approved']=False
        with self.assertRaises(m.Denied):self.verify()
    def test_expired_permit_rejected(self):
        self.permit['expires_at']=time.time()-1
        with self.assertRaises(m.Denied):self.verify()
    def test_wrong_request_rejected(self):
        self.permit['request_id']='b'*32
        with self.assertRaises(m.Denied):self.verify()
    def test_policy_no_auto_expansion(self):
        self.permit['policy']['inference_exec_max']=2
        with self.assertRaises(m.Denied):self.verify()
    def test_policy_types_strict(self):
        self.permit['policy']['application_retries']=False
        with self.assertRaises(m.Denied):self.verify()
    def test_unchanged_file_cannot_be_reapproved(self):
        p=self.tool/'collab/u0.py';p.write_bytes(b'changed');self.permit['runtime_sha256']['collab/u0.py']=self.sha(p)
        with self.assertRaises(m.Denied):self.verify()
    def test_missing_module_rejected(self):
        self.permit['runtime_sha256'].pop(m.NEW)
        with self.assertRaises(m.Denied):self.verify()
    def test_entry_tamper_rejected(self):
        self.permit['entry_sha256']='0'*64
        with self.assertRaises(m.Denied):self.verify()
    def test_root_tamper_rejected(self):
        self.permit['trial_tool_root']=str(self.root)
        with self.assertRaises(m.Denied):self.verify()
    def test_cli_tamper_rejected(self):
        self.cli.write_bytes(b'tamper')
        with self.assertRaises(m.Denied):self.verify()


if __name__=='__main__':unittest.main(verbosity=2)
