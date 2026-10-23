"""실제 CLI/모델/Discord 없이 과거 빈 출력 형태와 새 진단 경계를 검증합니다."""
import json
import subprocess
import unittest
from types import SimpleNamespace
from pathlib import Path
from tests.test_art_roles import ArtRolesTests as Fixtures
from collab.real_roles import run_once,grant_once
from collab.art_roles_once import make_plan,ROLES
from collab.exec_diagnostics import launch_manifest,failure_observation


class ExecDiagnosticsTests(unittest.TestCase):
    setUp=Fixtures.setUp
    tearDown=Fixtures.tearDown
    publish=Fixtures.publish
    run_job=Fixtures.run_job

    def invoke(self,command,**kwargs):
        if 'exec' not in command:return Fixtures.invoke(self,command,**kwargs)
        mode=getattr(self,'mode','PASS')
        if mode=='EMPTY':
            self.calls.append(kwargs['input'].split()[2])
            return SimpleNamespace(returncode=1,stdout='',stderr='SYNTHETIC_PRIVATE_VALUE ' + 'x'*306)
        if mode=='PERMISSION':return SimpleNamespace(returncode=1,stdout='',stderr='Permission denied: SYNTHETIC_PRIVATE_VALUE')
        if mode=='OSERROR':raise OSError(5,'SYNTHETIC_PRIVATE_VALUE')
        if mode=='TIMEOUT':raise subprocess.TimeoutExpired(command,90,stderr='SYNTHETIC_PRIVATE_VALUE')
        return Fixtures.invoke(self,command,**kwargs)

    def runner(self,*args):return run_once(*args,invoke=self.invoke)

    def single(self,role):
        plan=make_plan(role,self.sources,self.store.clock);grant_once(self.store,self.owner,plan)
        return self.runner(self.store,self.owner,plan,self.root/role,'fake')

    def test_success_two_roles_manifest_security_and_no_behavior_change(self):
        result=self.run_job();self.assertEqual('COMPLETED',result['state'])
        for stage in result['stages']:
            self.assertEqual('PASS',stage['status'])
            self.assertEqual('TURN_EVENT_OBSERVED',stage['launch_observation']['observed_stage'])
        manifests=list(self.root.rglob('launch-manifest.json'));self.assertEqual(2,len(manifests))
        for p in manifests:
            m=json.loads(p.read_text(encoding='utf8'));self.assertEqual(90,m['timeout_seconds'])
            self.assertEqual('read-only',m['argv'][m['argv'].index('--sandbox')+1])
            self.assertIn('--json',m['argv']);self.assertIn('--output-last-message',m['argv'])
            self.assertFalse(m['environment_values_logged']);self.assertIsNone(m['explicit_model'])
            self.assertNotIn('DISCORD_BOT_TOKEN',m['environment_keys'])

    def test_historical_shape_newart_fails_and_renewal_stops(self):
        self.mode='EMPTY';r=self.run_job();self.assertEqual('FAILED',r['state']);self.assertEqual([ROLES[0]],self.calls)
        stage=r['stages'][0];self.assertEqual('NOT_VERIFIED',stage['status'])
        self.assertEqual('PROCESS_RETURNED_BEFORE_FIRST_JSONL_EVENT',stage['launch_observation']['observed_stage'])
        self.assertEqual(['UNCLASSIFIED_NO_RAW_LOG'],stage['launch_observation']['stderr_signals'])
        self.assertEqual([],self.sent)
        self.assertFalse(any('SYNTHETIC_PRIVATE_VALUE' in p.read_text(encoding='utf8') for p in self.root.rglob('*.json')))

    def test_renewal_same_empty_output_handling(self):
        self.mode='EMPTY';r=self.single(ROLES[1]);self.assertEqual('NOT_VERIFIED',r['status'])
        self.assertEqual('PROCESS_RETURNED_BEFORE_FIRST_JSONL_EVENT',r['launch_observation']['observed_stage'])

    def test_permission_classification_does_not_pass(self):
        self.mode='PERMISSION';r=self.single(ROLES[0]);self.assertEqual('NOT_VERIFIED',r['status'])
        self.assertEqual(['FILESYSTEM_PERMISSION_DENIED'],r['launch_observation']['stderr_signals'])

    def test_process_creation_failure_distinct(self):
        self.mode='OSERROR';r=self.single(ROLES[0]);self.assertEqual('PROCESS_CREATION_FAILED',r['launch_observation']['observed_stage'])
        self.assertFalse(r['launch_observation']['process_returned']);self.assertEqual(5,r['launch_observation']['os_errno'])

    def test_timeout_is_not_nonzero_exit(self):
        self.mode='TIMEOUT';r=self.single(ROLES[0]);self.assertEqual('TIMEOUT',r['execution_status'])
        self.assertEqual('TIMEOUT_STAGE_UNCONFIRMED',r['launch_observation']['observed_stage'])

    def test_thread_only_does_not_claim_completed_inference(self):
        r=failure_observation({'diagnostics':{'event_counts':{'thread.started':1}}},elapsed_ms=2,process_returned=True)
        self.assertEqual('THREAD_EVENT_OBSERVED',r['observed_stage']);self.assertFalse(r['completed_turn_observed'])

    def test_manifest_never_contains_environment_values_or_prompt(self):
        m=launch_manifest(['fake','exec','--json'],{'USERPROFILE':'SYNTHETIC_PRIVATE_VALUE'},
            {'cli_version':'fake','subscription_available':True},{'status':'PASS'},'SYNTHETIC_PRIVATE_PROMPT')
        self.assertNotIn('SYNTHETIC_PRIVATE',json.dumps(m))


if __name__=='__main__':unittest.main()
