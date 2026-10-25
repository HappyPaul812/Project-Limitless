"""새 진단 경계만 검사합니다. 실제 Codex/사용량/AI/운영 DB는 사용하지 않습니다."""
import asyncio
import json
import os
from pathlib import Path
import subprocess
import sqlite3
import sys
import tempfile
import unittest
from unittest.mock import patch

from collab.core import Denied
from collab.usage_bridge import OfficialUsage, UsageMonitor
from collab.usage_diagnostics import ProtectedUsageLog, safe_error, safe_text, MAX_RECORD, MAX_RECORDS
from collab.scheduling import gate

HERE=Path(__file__).parent
ROOT=HERE.parent/'data/usage-diagnostics-offline'
ROOT.mkdir(parents=True,exist_ok=True)
REAL_SPAWN=asyncio.create_subprocess_exec
REAL_TIMEOUT=asyncio.timeout

class Capture:
    def prepare(self):pass
    def write(self,record):self.record=record.copy()

class UsageDiagnosticsTests(unittest.TestCase):
    def exercise(self,scenario):
        sink=Capture();spawns=[]
        async def fixture_spawn(executable,*args,**kwargs):
            self.assertEqual(executable,'NEVER_EXECUTE_CODEX')
            self.assertEqual(args,('app-server','--listen','stdio://'))
            self.assertEqual(kwargs['stderr'],asyncio.subprocess.PIPE)
            spawns.append(1)
            return await REAL_SPAWN(sys.executable,str(HERE/'fake_usage_app_server.py'),scenario,**kwargs)
        provider=OfficialUsage('NEVER_EXECUTE_CODEX',environment={},diagnostics=sink)
        error=None;value=None
        # timeout분기는 실제 18초를 기다리지 않고 0.15초 fixture로 검증합니다.
        with patch('asyncio.create_subprocess_exec',fixture_spawn),patch('asyncio.timeout',lambda _:REAL_TIMEOUT(0.15 if scenario=='timeout' else 8)):
            try:value=asyncio.run(provider.read(stage='OFFICIAL_USAGE_AND_APPROVAL'))
            except Denied as exc:error=str(exc)
        self.assertEqual(len(spawns),1)
        record=sink.record
        encoded=json.dumps(record)
        for secret in ['user@example.test','never-record@example.test','secret-value','PrivateName','not-public','auth.json']:
            self.assertNotIn(secret,encoded)
        self.assertLess(len(encoded.encode()),MAX_RECORD)
        self.assertEqual(record['stage'],'OFFICIAL_USAGE_AND_APPROVAL')
        self.assertTrue(record['location'].startswith('usage_bridge.OfficialUsage.read.'))
        self.assertIsInstance(record['process_exit_code'],int)
        return error,value,record

    def test_normal(self):
        error,value,record=self.exercise('normal');self.assertIsNone(error)
        self.assertTrue(value['ordinary_allowed']);self.assertEqual(record['status'],'PASS');self.assertEqual(record['process_exit_code'],0)

    def test_auth_rejection(self):
        error,_,r=self.exercise('auth_denied');self.assertEqual(error,'OFFICIAL_READ_REJECTED')
        self.assertEqual((r['rpc_method'],r['request_id'],r['error']['code']),('account/read',2,401))
        self.assertIn('Authentication failed',r['error']['message']);self.assertEqual(r['status'],'HOLD')

    def test_access_rejection(self):
        error,_,r=self.exercise('access_denied');self.assertEqual(error,'OFFICIAL_READ_REJECTED')
        self.assertEqual((r['rpc_method'],r['request_id'],r['error']['code']),('account/rateLimits/read',3,403))
        self.assertEqual(r['error']['data']['http_status'],403)

    def test_rate_rejection(self):
        error,_,r=self.exercise('rate_limited');self.assertEqual(error,'OFFICIAL_READ_REJECTED')
        self.assertEqual(r['error']['code'],429);self.assertEqual(r['error']['data']['retry_after'],5)

    def test_initialize_rejection(self):
        error,_,r=self.exercise('initialize_denied');self.assertEqual(error,'OFFICIAL_READ_REJECTED')
        self.assertEqual((r['rpc_method'],r['request_id'],r['error']['code']),('initialize',1,-32603))

    def test_invalid_json(self):
        error,_,r=self.exercise('invalid_json');self.assertEqual(error,'APP_SERVER_PROTOCOL_INVALID');self.assertEqual(r['status'],'HOLD')

    def test_invalid_object(self):
        error,_,_=self.exercise('not_object');self.assertEqual(error,'APP_SERVER_PROTOCOL_INVALID')

    def test_invalid_usage(self):
        error,_,r=self.exercise('invalid_usage');self.assertEqual(error,'CODEX_BUCKET_UNKNOWN')
        self.assertTrue(r['location'].endswith('.normalize'))

    def test_early_exit(self):
        error,_,r=self.exercise('early_exit');self.assertEqual(error,'APP_SERVER_CLOSED_BEFORE_RESPONSE')
        self.assertEqual(r['process_exit_code'],17);self.assertIn('Connection closed',r['stderr'])

    def test_timeout(self):
        error,_,r=self.exercise('timeout');self.assertEqual(error,'OFFICIAL_USAGE_TIMEOUT')
        self.assertIn(r['termination'],['TERMINATED_BY_READER','KILLED_BY_READER']);self.assertIn('timed out',r['stderr'])

    def test_stderr_flood(self):
        error,_,r=self.exercise('stderr_flood');self.assertIsNone(error)
        self.assertTrue(r['stderr_truncated']);self.assertGreater(r['stderr_bytes'],32768);self.assertLessEqual(len(r['stderr']),4096)

    def test_oversized_stdout(self):
        error,_,r=self.exercise('oversized');self.assertIsNotNone(error);self.assertEqual(r['status'],'HOLD')

    def test_nonzero_after_valid_response(self):
        error,_,r=self.exercise('nonzero_after_response');self.assertEqual(error,'APP_SERVER_EXIT_NONZERO')
        self.assertEqual(r['process_exit_code'],23);self.assertEqual(r['status'],'HOLD')

    def test_exhausted_and_disallowed_still_not_permission(self):
        _,value,_=self.exercise('exhausted');self.assertEqual(value['windows'][0]['used_percent'],100)
        self.assert_usage_hold(value)
        _,value,_=self.exercise('blocked');self.assertFalse(value['ordinary_allowed']);self.assert_usage_hold(value)

    def assert_usage_hold(self,value):
        with sqlite3.connect(':memory:') as db:
            db.row_factory=sqlite3.Row
            db.executescript('CREATE TABLE control_usage(id INTEGER,observed REAL,source TEXT,body TEXT);CREATE TABLE control_schedules(id TEXT,state TEXT,created REAL);')
            db.execute('INSERT INTO control_usage VALUES(1,?,?,?)',(value['observed'],value['source'],json.dumps(value)))
            self.assertEqual(gate(db,value['observed'],ai=True),'USAGE_HOLD')

    def test_no_log_permission_no_spawn(self):
        sink=Capture();sink.prepare=lambda:(_ for _ in ()).throw(PermissionError('private'))
        with patch('asyncio.create_subprocess_exec') as spawn:
            with self.assertRaisesRegex(Denied,'USAGE_DIAGNOSTIC_PREFLIGHT_HOLD'):
                asyncio.run(OfficialUsage('NEVER_EXECUTE_CODEX',diagnostics=sink).read())
            spawn.assert_not_called()

    def test_log_failure_blocks_even_valid_usage(self):
        class Broken(Capture):
            def write(self,record):raise PermissionError('secret')
        async def spawn(*args,**kwargs):
            return await REAL_SPAWN(sys.executable,str(HERE/'fake_usage_app_server.py'),'normal',**kwargs)
        with patch('asyncio.create_subprocess_exec',spawn):
            with self.assertRaisesRegex(Denied,'USAGE_DIAGNOSTIC_WRITE_HOLD'):
                asyncio.run(OfficialUsage('NEVER_EXECUTE_CODEX',diagnostics=Broken()).read())

    def test_private_path_capacity_and_no_overwrite(self):
        # 합성 fixture만 임시 프로필에 씁니다. 실제 서비스 프로필/ACL은 사용하지 않습니다.
        with tempfile.TemporaryDirectory(dir=ROOT) as folder,patch.dict(os.environ,{'USERPROFILE':folder}):
            home=Path(folder)/'.codex';home.mkdir();log=ProtectedUsageLog(home);log.prepare()
            log.write({'stage':'OFFLINE_FIXTURE','stderr':'Access denied'})
            self.assertEqual(json.loads(log.file.read_text())['stderr'],'Access denied')
            first=log.file.read_bytes()
            for _ in range(MAX_RECORDS-1):ProtectedUsageLog(home).prepare()
            with self.assertRaisesRegex(Denied,'CAPACITY'):ProtectedUsageLog(home).prepare()
            self.assertEqual(log.file.read_bytes(),first)
            with self.assertRaisesRegex(Denied,'PATH'):ProtectedUsageLog(Path(folder)).prepare()

    def test_diagnostic_privacy_and_size(self):
        message='Access denied "Private Name" user@example.test Bearer secret-value password=not-public '+('X'*100000)
        self.assertLessEqual(len(safe_text(message)),2048)
        r=safe_error({'code':403,'message':message,'data':{'password':message,'http_status':403,'reason':message}})
        self.assertEqual(r['code'],403);self.assertNotIn('Private',json.dumps(r));self.assertNotIn('password',r['data'])

    def test_reparse_path_rejected(self):
        # symlink를 만들거나 ACL을 바꾸지 않고 Windows reparse 속성을 모의합니다.
        from collab.usage_diagnostics import no_link
        from types import SimpleNamespace
        with tempfile.TemporaryDirectory(dir=ROOT) as folder:
            with patch.object(Path,'is_symlink',return_value=False),patch.object(Path,'exists',return_value=True),patch.object(Path,'lstat',return_value=SimpleNamespace(st_file_attributes=0x400)):
                with self.assertRaisesRegex(Denied,'PATH'):no_link(Path(folder))

    def test_record_limit_fails_closed(self):
        with tempfile.TemporaryDirectory(dir=ROOT) as folder,patch.dict(os.environ,{'USERPROFILE':folder}):
            home=Path(folder)/'.codex';home.mkdir();log=ProtectedUsageLog(home);log.prepare()
            with self.assertRaisesRegex(Denied,'SIZE'):log.write({'stderr':'x'*(MAX_RECORD+1)})

    def test_service_provider_requires_private_log(self):
        from collab import service_ai
        with patch.object(service_ai,'windows_context',return_value={}),patch.object(service_ai,'service_environment',return_value={'CODEX_HOME':'TEST_PROFILE/.codex'}),patch.object(service_ai,'select_executable',return_value='NEVER_EXECUTE_CODEX'):
            provider=service_ai.prepare_provider(approval={})
        self.assertIsInstance(provider.diagnostics,ProtectedUsageLog)

    def test_related_runner_stages_connected(self):
        # 실행기 자체를 실행하지 않고 두 사전 게이트의 명시 단계 전달을 확인합니다.
        import ast
        paths=[HERE.parent/'collab/service_batch_once.py',HERE.parent.parent/'DiscordAIServiceDiagnostics/service-final-once.py']
        for path in paths:
            tree=ast.parse(path.read_text(encoding='utf8'))
            self.assertTrue(any(isinstance(n,ast.Call) and isinstance(n.func,ast.Attribute) and n.func.attr=='read' and any(k.arg=='stage' and isinstance(k.value,ast.Constant) and k.value.value=='OFFICIAL_USAGE_AND_APPROVAL' for k in n.keywords) for n in ast.walk(tree)))

if __name__=='__main__':unittest.main(verbosity=2)
