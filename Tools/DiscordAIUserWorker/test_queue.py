"""격리 SQLite와 모의 callback만 씁니다. 실제 CLI·인증·네트워크 호출은 없습니다."""
import hashlib
import json
from pathlib import Path
import sys
import tempfile
import unittest
from unittest.mock import Mock, patch

TOOLS = Path(__file__).resolve().parent.parent
sys.path.insert(0, str(TOOLS))
sys.path.insert(0, str(TOOLS / 'DiscordAICollab'))
from collab.core import Store, Identity, Denied
from collab.real_roles import make_plan, digest, grant_once, ROLE_FILES, RETEST_SLOT, CODEX_SLOT, NEW_ART_SLOT, RENEWAL_ART_SLOT
from DiscordAIUserWorker.durable_queue import UserWorker, enqueue, inspect, existing_role_runner

SID = 'S-1-5-21-111-222-333-1001'


class QueueTests(unittest.TestCase):
    def setUp(self):
        root = TOOLS / 'DiscordAICollab/data/user-worker-tests'
        root.mkdir(parents=True, exist_ok=True)
        self.temp = tempfile.TemporaryDirectory(dir=root)
        self.root = Path(self.temp.name).resolve()
        self.now = 1792720800
        self.store = Store(self.root / 'relay.sqlite', {1}, clock=lambda: self.now)
        self.owner = Identity(1, 1, 1)
        self.binary = self.root / 'MOCK_NOT_EXECUTABLE.exe'
        self.binary.write_bytes(b'FIXTURE_ONLY')
        self.sha = hashlib.sha256(self.binary.read_bytes()).hexdigest()
        self.worker = UserWorker(self.store, self.owner, self.binary, self.sha, self.root / 'empty')
        self.context = {'sid': SID, 'session_id': 8, 'is_admin': False, 'name': 'PC\\User',
                        'profile': str(self.root / 'profile'), 'codex_home': str(self.root / 'profile/.codex')}
        self.auth = Mock(return_value=True)
        self.usage = Mock(side_effect=self.observation)
        self.refs = Mock(side_effect=lambda plan: plan['source_refs'])
        self.runner = Mock(return_value={'status': 'PASS', 'mode': 'REAL_CLI_READONLY'})

    def tearDown(self):
        self.temp.cleanup()

    def observation(self, *args):
        return {'source':'CODEX_APP_SERVER_OFFICIAL','observed':self.now,'ordinary_allowed':True,
                'windows':[{'minutes':300,'used_percent':5,'resets_at':self.now+1000},
                           {'minutes':10080,'used_percent':16,'resets_at':self.now+10000}]}

    def plan(self, role='PlanningAgent', grant=True):
        sources = {'refs': {'main': 'a'*40}, 'files': [
            {'path':'문서/담당/'+name+'.md','ref':'main','content':'FIXTURE_ONLY'} for name in ROLE_FILES.values()]}
        plan = make_plan(role, sources, clock=lambda:self.now)
        plan['slot'] = {'PlanningAgent':RETEST_SLOT,'CodexAgent':CODEX_SLOT,
                        'NewArtAgent':NEW_ART_SLOT,'RenewalArtAgent':RENEWAL_ART_SLOT}[role]
        plan['sha256'] = digest({k:v for k,v in plan.items() if k != 'sha256'})
        if grant:
            grant_once(self.store, self.owner, plan)
        return plan

    def tick(self, **kwargs):
        return self.worker.once(kwargs.pop('context',self.context), released=kwargs.pop('released',True),
                                authenticate=self.auth, official_usage=self.usage, source_refs=self.refs,
                                runner=kwargs.pop('runner', self.runner), **kwargs)

    def add(self, **kwargs):
        plan = self.plan(**kwargs)
        enqueue(self.store, self.owner, plan, SID)
        return plan

    def test_unapproved_and_wrong_owner_rejected(self):
        plan = self.plan(grant=False)
        with self.assertRaises(Denied): enqueue(self.store,self.owner,plan,SID)
        with self.assertRaises(Denied): enqueue(self.store,Identity(2,1,1),plan,SID)

    def test_pending_persisted_in_same_database(self):
        plan = self.add()
        reopened = Store(self.store.path,{1},clock=lambda:self.now)
        self.assertEqual('PENDING_LOGIN',inspect(reopened)[0]['state'])
        enqueue(reopened,self.owner,plan,SID)
        self.assertEqual(1,len(inspect(reopened)))
        self.runner.assert_not_called()

    def test_default_disabled_before_auth(self):
        self.add()
        self.assertEqual('HOLD_NOT_RELEASED',self.tick(released=False)['state'])
        self.auth.assert_not_called(); self.runner.assert_not_called()

    def test_session_zero_admin_and_service_rejected(self):
        self.add()
        for delta in ({'session_id':0},{'is_admin':True},{'name':'PC\\LIMITLESSRelaySvc'}):
            self.assertEqual('HOLD_USER_LOGON_REQUIRED',self.tick(context={**self.context,**delta})['state'])
        self.auth.assert_not_called()

    def test_wrong_sid_or_home_does_not_call(self):
        self.add()
        self.assertEqual('NO_ELIGIBLE_JOB',self.tick(context={**self.context,'sid':SID+'9'})['state'])
        self.assertEqual('HOLD_OWN_PROFILE_REQUIRED',self.tick(context={**self.context,'codex_home':str(self.root/'foreign')})['state'])
        self.runner.assert_not_called()

    def test_expired_or_consumed_never_renewed(self):
        self.add(); self.now += 601
        self.assertEqual('NO_ELIGIBLE_JOB',self.tick()['state'])
        self.assertEqual('WAITING_REAPPROVAL',inspect(self.store)[0]['state'])
        self.runner.assert_not_called()

    def test_tampered_plan_rejected(self):
        plan = self.plan(); plan['prompt'] += 'tampered'
        with self.assertRaises(Denied): enqueue(self.store,self.owner,plan,SID)

    def test_immutable_worker_assignment(self):
        plan = self.add()
        with self.assertRaises(Denied): enqueue(self.store,self.owner,plan,SID+'9')

    def test_auth_failure_holds_without_consuming(self):
        self.add();self.auth.return_value=False
        self.assertEqual('PENDING_LOGIN',self.tick()['state'])
        self.runner.assert_not_called();self.usage.assert_not_called()

    def test_unknown_exhausted_or_disallowed_usage(self):
        self.add()
        for value in ({}, {**self.observation(),'ordinary_allowed':False},
                      {**self.observation(),'windows':[{**self.observation()['windows'][0],'used_percent':100},self.observation()['windows'][1]]}):
            self.usage.side_effect=None;self.usage.return_value=value
            self.assertEqual('PENDING_LOGIN',self.tick()['state'])
        self.runner.assert_not_called()

    def test_canonical_change_requires_reapproval(self):
        self.add();self.refs.side_effect=None;self.refs.return_value={'main':'b'*40}
        self.assertEqual('WAITING_REAPPROVAL',self.tick()['state'])
        self.runner.assert_not_called()

    def test_runtime_change_blocked_before_and_after_preflight(self):
        self.add();self.binary.write_bytes(b'CHANGED')
        self.assertEqual('HOLD_RUNTIME_CHANGED',self.tick()['state'])
        self.binary.write_bytes(b'FIXTURE_ONLY')
        def change(*args):
            self.binary.write_bytes(b'CHANGED_AGAIN');return self.observation()
        self.usage.side_effect=change
        self.assertEqual('WAITING_REAPPROVAL',self.tick()['state'])
        self.runner.assert_not_called()

    def test_one_result_and_no_duplicate(self):
        self.add();self.assertEqual('DONE',self.tick()['state'])
        self.assertEqual('NO_ELIGIBLE_JOB',self.tick()['state']);self.runner.assert_called_once()

    def test_runner_failure_never_retried(self):
        self.add();self.runner.side_effect=RuntimeError('SECRET_ERROR')
        self.assertEqual('NOT_VERIFIED',self.tick()['state'])
        self.tick();self.runner.assert_called_once()
        with self.store.tx() as db:
            self.assertNotIn('SECRET_ERROR',db.execute('SELECT result FROM user_worker_jobs').fetchone()[0])

    def test_persistent_claim_blocks_second_worker_after_crash(self):
        self.add()
        with self.store.tx() as db: db.execute('INSERT INTO user_worker_lock VALUES(1,?,?)',('INTERRUPTED','x'))
        self.assertEqual('HOLD_WORKER_BUSY_OR_INTERRUPTED',self.tick()['state'])
        self.runner.assert_not_called()

    def test_concurrent_claim_and_pause_or_schedule_blocked(self):
        self.add()
        def nested(*args):
            self.assertEqual('HOLD_WORKER_BUSY_OR_INTERRUPTED',self.tick()['state']);return True
        self.auth.side_effect=nested
        self.assertEqual('DONE',self.tick()['state'])

    def test_four_roles_reuse_existing_runner_without_real_call(self):
        for role in ROLE_FILES:
            plan = self.add(role=role)
            with patch('DiscordAIUserWorker.durable_queue.current_user_context',return_value=self.context), patch('DiscordAIUserWorker.durable_queue.run_once',return_value={'status':'PASS','mode':'REAL_CLI_READONLY'}) as existing:
                self.assertEqual('DONE',self.tick(runner=existing_role_runner)['state'])
                self.assertEqual(plan, existing.call_args.args[2])

    def test_consumed_grant_cannot_enter_queue(self):
        plan = self.plan()
        with self.store.tx() as db:
            grant = json.loads(self.store.setting(db,plan['slot'])); grant['state']='CONSUMED'
            db.execute('UPDATE settings SET value=? WHERE key=?',(json.dumps(grant),plan['slot']))
        with self.assertRaises(Denied): enqueue(self.store,self.owner,plan,SID)

    def test_role_pause_blocks_without_auth(self):
        self.add()
        with self.store.tx() as db: db.execute("UPDATE settings SET value='1' WHERE key='role:PlanningAgent'")
        self.assertEqual('NO_ELIGIBLE_JOB',self.tick()['state'])
        self.auth.assert_not_called()

    def test_mode_and_pause_are_never_changed(self):
        self.add()
        with self.store.tx() as db:db.execute("UPDATE settings SET value='1' WHERE key='paused'")
        self.assertEqual('HOLD_PAUSED',self.tick()['state'])
        with self.store.tx() as db:
            self.assertEqual('MANUAL',db.execute('SELECT mode FROM operation_mode WHERE singleton=1').fetchone()[0])
        self.runner.assert_not_called()


if __name__ == '__main__':
    unittest.main()
