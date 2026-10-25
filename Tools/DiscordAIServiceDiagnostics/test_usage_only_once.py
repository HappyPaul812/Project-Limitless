"""새 usage-only 진입점만 합성 승인/응답으로 검사합니다. 기존 22건은 재실행하지 않습니다."""
import ast
import contextlib
import hashlib
import importlib.util
import io
import json
import os
from pathlib import Path
import tempfile
import time
import unittest
from unittest.mock import patch

HERE=Path(__file__).resolve().parent
spec=importlib.util.spec_from_file_location('usage_only_once',HERE/'usage-only-once.py')
m=importlib.util.module_from_spec(spec);spec.loader.exec_module(m)
OUT=HERE/'data/usage-only-offline';OUT.mkdir(parents=True,exist_ok=True)
def sha(path):return hashlib.sha256(path.read_bytes()).hexdigest()

class UsageOnlyTests(unittest.TestCase):
    def setUp(self):
        self.temp=tempfile.TemporaryDirectory(dir=OUT);self.addCleanup(self.temp.cleanup)
        self.root=Path(self.temp.name).resolve();self.tool=self.root/'Tools/DiscordAICollab'
        (self.tool/'collab').mkdir(parents=True)
        old={}
        names=sorted(m.CHANGED)+[f'collab/unchanged_{i}.py' for i in range(39)]+['DiscordSecretStore.psm1']
        for name in names:
            path=self.tool/name;path.write_bytes(b'# synthetic old bytes\n');old[name]=sha(path)
            if name in m.CHANGED:path.write_bytes(b'# synthetic changed bytes\n')
        (self.tool/m.NEW).write_bytes(b'# synthetic new diagnostic module\n')
        (self.tool/'roles.json').write_bytes(b'{}')
        self.cli=self.root/'fake-cli-not-executable';self.cli.write_bytes(b'NO_EXECUTION_ALLOWED')
        self.profile=self.root/'LIMITLESSRelaySvc';(self.profile/'.codex').mkdir(parents=True)
        self.base={'service_sid':'SYNTHETIC_SID','service_ai_bridge':'READONLY_AUTH_AND_RESUME',
                   'tool_root':'OLD_APPROVED_ROOT','runtime_sha256':old,
                   'service_codex':{'path':str(self.cli),'sha256':sha(self.cli)}}
        self.boot=self.root/'boot.json';self.boot.write_text(json.dumps(self.base))
        self.request='a'*32
        self.permit={'approved':True,'kind':'USAGE_ONLY_ONCE','request_id':self.request,'expires_at':time.time()+300,
                     'boot_approval_sha256':sha(self.boot),'trial_tool_root':str(self.tool),'entry_sha256':sha(m.SCRIPT),
                     'roles_sha256':sha(self.tool/'roles.json'),'runtime_sha256':{n:sha(self.tool/n) for n in set(old)|{m.NEW}},
                     'cli_sha256':sha(self.cli),'mode':'MANUAL','limits':m.LIMITS.copy()}
        self.permitfile=self.root/'permit.json';self.calls=[];self.factory_calls=[];self.now=0;self.mode='normal'
        self.context={'sid':'SYNTHETIC_SID','name':'FIXTURE\\LIMITLESSRelaySvc','admin':False,'profile':str(self.profile)}
        owner=self
        class FakeProvider:
            executable=str(owner.cli)
            environment={'CODEX_HOME':str(owner.profile/'.codex')}
            async def read(self,stage):
                owner.calls.append(stage)
                if owner.mode=='error':raise m.Denied('OFFICIAL_READ_REJECTED')
                if owner.mode=='timeout':raise TimeoutError()
                return {'ordinary_allowed':True,'windows':[{'used_percent':5},{'used_percent':10}]}
        def factory(**kwargs):
            self.factory_calls.append(kwargs)
            self.assertEqual(kwargs['approval']['service_codex'],self.base['service_codex'])
            self.assertEqual(kwargs['root'],self.tool)
            if self.mode=='late':self.now=19
            return FakeProvider()
        self.stack=contextlib.ExitStack();self.addCleanup(self.stack.close)
        for target,value in [('TOOL',self.tool),('BOOT',self.boot),('PERMIT',self.permitfile),('windows_context',lambda:self.context),('prepare_provider',factory)]:
            self.stack.enter_context(patch.object(m,target,value))
        self.stack.enter_context(patch.dict(os.environ,{'USERPROFILE':str(self.profile)}))
    def run_fixture(self):
        self.permitfile.write_text(json.dumps(self.permit))
        return m.run(self.request,clock=lambda:self.now)
    def test_single_read_and_consumed_slot(self):
        r=self.run_fixture();self.assertEqual(r['status'],'USAGE_ONLY_OBSERVED')
        self.assertEqual(self.calls,['OFFICIAL_USAGE_READ'])
        r=self.run_fixture();self.assertEqual(r['reason'],'USAGE_ONLY_ALREADY_CONSUMED_HOLD');self.assertEqual(len(self.calls),1)
        for key in ('model_calls','discord_posts','sqlite_operations','automatic_retries'):self.assertEqual(r[key],0)
    def test_rejected_response_hold_and_private_record(self):
        self.mode='error';r=self.run_fixture();self.assertEqual(r['reason'],'OFFICIAL_READ_REJECTED');self.assertEqual(len(self.calls),1)
        logs=list((self.profile/'.codex/limitless-usage-diagnostics').glob('*.json'))
        self.assertEqual(len(logs),1);record=json.loads(logs[0].read_text());self.assertEqual(record['status'],'HOLD');self.assertEqual(record['stage'],'USAGE_ONLY_READ')
    def test_timeout_no_retry(self):
        self.mode='timeout';r=self.run_fixture();self.assertEqual(r['reason'],'USAGE_ONLY_TIMEOUT_HOLD');self.assertEqual(len(self.calls),1)
    def test_unapproved_request_no_provider(self):
        self.permit['approved']=False;r=self.run_fixture();self.assertEqual(r['reason'],'USAGE_ONLY_OWNER_APPROVAL_HOLD');self.assertFalse(self.factory_calls)
    def test_approval_expires_no_read(self):
        self.permit['expires_at']=0;self.assertEqual(self.run_fixture()['reason'],'USAGE_ONLY_APPROVAL_EXPIRED_HOLD');self.assertFalse(self.calls)
    def test_unchanged_psm_hash_must_not_expand(self):
        self.permit['runtime_sha256']['DiscordSecretStore.psm1']='0'*64
        self.assertEqual(self.run_fixture()['reason'],'USAGE_ONLY_UNCHANGED_SHA_HOLD');self.assertFalse(self.calls)
    def test_runtime_extra_file_not_approved(self):
        self.permit['runtime_sha256']['collab/extra.py']='0'*64
        self.assertEqual(self.run_fixture()['reason'],'USAGE_ONLY_RUNTIME_SET_HOLD');self.assertFalse(self.calls)
    def test_tampered_candidate_no_read(self):
        (self.tool/m.NEW).write_bytes(b'changed without approval')
        self.assertEqual(self.run_fixture()['reason'],'USAGE_ONLY_RUNTIME_SHA_HOLD');self.assertFalse(self.calls)
    def test_elapsed_preflight_leaves_cleanup_reserve(self):
        self.mode='late';self.assertEqual(self.run_fixture()['reason'],'USAGE_ONLY_DEADLINE_HOLD');self.assertFalse(self.calls)
    def test_wrong_limits_and_cli_pin_no_read(self):
        self.permit['limits']['max_usage_reads']=2
        self.assertEqual(self.run_fixture()['reason'],'USAGE_ONLY_LIMITS_HOLD');self.assertFalse(self.calls)
        self.permit['limits']=m.LIMITS.copy();self.permit['cli_sha256']='0'*64
        self.assertEqual(self.run_fixture()['reason'],'USAGE_ONLY_CLI_SHA_HOLD');self.assertFalse(self.calls)
    def test_no_request_does_not_inspect_service(self):
        with patch.object(m,'windows_context') as context,contextlib.redirect_stdout(io.StringIO()):
            self.assertEqual(m.main([]),2);context.assert_not_called()
    def test_no_forbidden_execution_dependencies(self):
        tree=ast.parse(m.SCRIPT.read_text(encoding='utf8'))
        names={n.id for n in ast.walk(tree) if isinstance(n,ast.Name)}
        self.assertFalse(names&{'Store','Scheduler','publish','connect_boot_ai','subprocess','ResumeQueue'})
        self.assertEqual(sum(isinstance(n,ast.Call) and isinstance(n.func,ast.Attribute) and n.func.attr=='read' and isinstance(n.func.value,ast.Name) and n.func.value.id=='provider' for n in ast.walk(tree)),1)

if __name__=='__main__':unittest.main(verbosity=2)
