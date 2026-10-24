"""주일 정책 회귀: 격리 SQLite·가짜 CLI만 사용하며 실제 AI/Discord 호출은 하지 않습니다."""
import json
import unittest
from datetime import datetime
from unittest.mock import Mock, patch
from tests import test_operations as ops
Fixture=ops.Fixture
from tests import test_real_roles as roles
from tests import test_phase2 as phase
FakeCLI=phase.FakeCLI
from collab.core import KST, Denied
from collab.scheduling import gate
from collab.real_roles import make_plan, grant_once, run_once, CODEX_SLOT, digest
from collab.resume_queue import ResumeQueue
from collab.__main__ import sample

SUNDAY=datetime(2026,10,25,12,tzinfo=KST).timestamp()

class SundayTests(unittest.TestCase, Fixture):
    setUp=Fixture.setup
    tearDown=Fixture.cleanup
    def test_seoul_week_boundaries_no_calendar_block(self):
        for stamp in (datetime(2026,10,24,23,59,59,tzinfo=KST).timestamp(), datetime(2026,10,25,0,tzinfo=KST).timestamp(), datetime(2026,10,25,23,59,59,tzinfo=KST).timestamp(), datetime(2026,10,26,0,tzinfo=KST).timestamp()):
            self.now=stamp;self.observe()
            with self.store.tx() as db:
                self.assertIsNone(gate(db,stamp,intake=True));self.assertIsNone(gate(db,stamp,ai=True))
    def test_sunday_schedule_preserves_deadline_validation(self):
        self.now=SUNDAY
        self.scheduler.command(self.owner,'s','schedule',{'until':'13:00','next_start':'2026-10-25 14:00'})
        with self.assertRaises(Denied):self.scheduler.command(self.owner,'bad','schedule',{'until':'13:00','next_start':'2026-10-25 12:30'})
    def test_manual_no_runner_even_after_sunday_recovery(self):
        self.now=SUNDAY;self.observe();runner=Mock()
        ResumeQueue(self.store).tick(self.owner,runner);runner.assert_not_called()
        with self.store.tx() as db:self.assertEqual(db.execute('SELECT mode FROM operation_mode').fetchone()[0],'MANUAL')
    def test_sunday_usage_unknown_exhausted_stale_block(self):
        self.now=SUNDAY;self.assertEqual(self.decision(),'USAGE_HOLD')
        self.observe(100,100);self.assertEqual(self.decision(),'USAGE_HOLD')
        self.now+=1201;self.assertEqual(self.decision(),'USAGE_HOLD')
        self.observe();self.assertIsNone(self.decision())
    def test_waiting_art_does_not_block_independent_task_and_needs_owner(self):
        self.now=SUNDAY
        first=self.store.command(self.owner,'art','task',sample('RenewalArtAgent'))['task_id']
        self.store.triage(first);claimed=self.store.claim()
        self.store.finish(first,claimed['lease'],'TECH_PASS',{'mode':'MOCK'})
        second=self.store.command(self.owner,'code','task',sample('CodexAgent'))['task_id']
        self.store.triage(second);self.assertEqual(self.store.claim()['id'],second)
        self.assertEqual(self.store.get(first)['state'],'REVIEW_PENDING')
        self.assertEqual(self.store.get(first)['art'],'USER_REVIEW_PENDING')
        for action in ('apply','push','merge'):
            with self.assertRaises(Denied):self.store.command(self.owner,action,action,{})

class SundayQueueTests(unittest.TestCase, Fixture):
    def setUp(self):
        ops.ResumeTests.setUp(self);self.now=SUNDAY
        self.plan=make_plan('PlanningAgent',self.sources if hasattr(self,'sources') else {'refs':{'main':'a'*40},'files':[{'path':'문서/담당/PLANNING.md','ref':'main','content':'fixture'}]},clock=lambda:self.now)
        self.scheduler.command(self.owner,'a','resume')
        self.patch=patch('collab.adapters.GitHubReader.read',return_value={'sha':'a'*40});self.patch.start();self.addCleanup(self.patch.stop)
    tearDown=Fixture.cleanup
    enqueue=ops.ResumeTests.enqueue
    def test_sunday_quota_recovery_exactly_once(self):
        self.enqueue();self.observe(100,100);runner=Mock(return_value={'status':'PASS','mode':'REAL_CLI_READONLY','answer':{'role':'PlanningAgent','summary':'FAKE'}})
        self.queue.tick(self.owner,runner);runner.assert_not_called()
        self.now+=61;self.observe();self.queue.tick(self.owner,runner);self.queue.tick(self.owner,runner)
        self.assertEqual(runner.call_count,1)
        with self.store.tx() as db:self.assertEqual(db.execute('SELECT state FROM control_resume').fetchone()[0],'DONE')
    def test_sunday_unapproved_plan_cannot_enqueue(self):
        with self.assertRaises(Denied):self.queue.defer(self.owner,self.plan,self.workspace,self.binary)
    def test_sunday_role_pause_skips_to_independent_approved_queue(self):
        self.enqueue()
        other=make_plan('CodexAgent',{'refs':{'main':'a'*40},'files':[{'path':'문서/담당/CODEX.md','ref':'main','content':'fixture'}]},clock=lambda:self.now)
        other['slot']=CODEX_SLOT;other['sha256']=digest({k:v for k,v in other.items() if k!='sha256'})
        grant_once(self.store,self.owner,other);self.queue.defer(self.owner,other,self.root/'second',self.binary)
        with self.store.tx() as db:db.execute("UPDATE settings SET value='1' WHERE key='role:PlanningAgent'")
        self.observe();runner=Mock(return_value={'status':'PASS','mode':'REAL_CLI_READONLY','answer':{'role':'CodexAgent','summary':'FAKE'}})
        self.queue.tick(self.owner,runner);self.assertEqual(runner.call_count,1);self.assertEqual(runner.call_args.args[2]['role'],'CodexAgent')
        with self.store.tx() as db:self.assertEqual(db.execute('SELECT state FROM control_resume WHERE id=?',(self.plan['sha256'],)).fetchone()[0],'HELD')
    def test_sunday_concurrency_and_pause_preserved(self):
        self.enqueue();self.observe();runner=Mock()
        with self.store.tx() as db:db.execute("UPDATE settings SET value='1' WHERE key='paused'")
        self.queue.tick(self.owner,runner);runner.assert_not_called()
        with self.store.tx() as db:
            db.execute("UPDATE settings SET value='0' WHERE key='paused'")
            db.execute("UPDATE control_resume SET state='RUNNING'")
        self.queue.tick(self.owner,runner);runner.assert_not_called()

class SundayCLITests(unittest.TestCase):
    setUp=roles.RealRoleTests.setUp
    tearDown=roles.RealRoleTests.tearDown
    fake=roles.RealRoleTests.fake
    def test_actual_readonly_path_sunday_fake_cli_once(self):
        from collab.scheduling import Scheduler
        from tests.test_operations import raw
        from collab.usage_bridge import normalize
        self.now=SUNDAY;self.plan=make_plan('PlanningAgent',self.sources,clock=lambda:self.now)
        scheduler=Scheduler(self.store);scheduler.record_official_usage(normalize(raw(self.now),self.now),self.now)
        grant_once(self.store,self.owner,self.plan);invoke=Mock(side_effect=self.fake)
        result=run_once(self.store,self.owner,self.plan,self.root/'empty','codex-test',invoke=invoke)
        self.assertEqual(result['status'],'PASS');self.assertEqual(sum('exec' in c.args[0] for c in invoke.call_args_list),1)
        with self.assertRaises(Denied):run_once(self.store,self.owner,self.plan,self.root/'retry','codex-test',invoke=invoke)
        self.assertTrue(all(t['art']=='USER_REVIEW_PENDING' for t in self.store.snapshot()['tasks']))

class SundayProbeTests(unittest.TestCase):
    setUp=phase.Phase2Tests.setUp
    tearDown=phase.Phase2Tests.tearDown
    def test_sunday_probe_still_needs_explicit_approval(self):
        from collab.phase2 import run_probe_once
        from collab.scheduling import Scheduler
        from tests.test_operations import raw
        from collab.usage_bridge import normalize
        self.store.clock=lambda:SUNDAY
        Scheduler(self.store).record_official_usage(normalize(raw(SUNDAY),SUNDAY),SUNDAY)
        fake=FakeCLI()
        with self.assertRaises(Denied):run_probe_once(self.store,self.root/'empty','codex',invoke=fake)
        self.assertEqual(fake.inference_count,0)
        result=run_probe_once(self.store,self.root/'empty','codex',approved=True,invoke=fake)
        self.assertEqual(result['status'],'PASS');self.assertEqual(fake.inference_count,1)
