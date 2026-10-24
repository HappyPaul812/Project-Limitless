import unittest
from datetime import datetime
from unittest.mock import Mock
from tests import test_operations as fixtures
from collab.core import Identity,Denied,KST,Store
from collab.operating_modes import mode
from collab.scheduling import gate,format_control,shutdown_preview
from collab.resume_queue import ResumeQueue
class ModesTests(unittest.TestCase,fixtures.Fixture):
    setUp=fixtures.Fixture.setup
    tearDown=fixtures.Fixture.cleanup
    def cmd(self,a,args=None,event=None):return self.scheduler.command(self.owner,event or str(self.now)+a,a,args)
    def schedule(self):return self.cmd('schedule',{'until':'2026-10-24 02:00'})
    def test_default(self):self.assertEqual(self.cmd('mode')['operation_mode'],'MANUAL')
    def test_a(self):self.assertEqual(self.cmd('resume')['operation_mode'],'AUTO_RESUME')
    def test_b(self):self.assertEqual(self.cmd('mode',{'value':'SAFE_SHUTDOWN'})['operation_mode'],'SAFE_SHUTDOWN')
    def test_invalid(self):
        with self.assertRaises(Denied):self.cmd('mode',{'value':'evil'})
    def test_owner(self):
        with self.assertRaises(Denied):self.scheduler.command(Identity(2,1,1),'bad','mode',{'value':'AUTO_RESUME'})
    def test_dedup(self):
        self.cmd('resume',event='same');self.cmd('resume',event='same')
        with self.store.tx() as db:self.assertEqual(db.execute('SELECT count(*) FROM operation_history').fetchone()[0],1)
    def test_event_tamper(self):
        self.cmd('mode',event='same')
        with self.assertRaises(Denied):self.cmd('mode',{'value':'AUTO_RESUME'},event='same')
    def test_five_hour(self):
        self.cmd('resume');self.observe(100,5);self.assertEqual(self.scheduler.tick()['usage']['status'],'EXHAUSTED');self.assertEqual(self.decision(),'USAGE_HOLD')
    def test_weekly(self):self.observe(5,100);self.assertEqual(self.decision(),'USAGE_HOLD')
    def test_unknown(self):self.assertEqual(self.decision(),'USAGE_HOLD')
    def test_recovery(self):
        self.observe(100,100);self.now+=1201;self.assertEqual(self.decision(),'USAGE_HOLD');self.observe();self.assertIsNone(self.decision())
    def test_sunday(self):
        self.now=datetime(2026,10,25,1,tzinfo=KST).timestamp()
        with self.store.tx() as db:self.assertIsNone(gate(db,self.now,intake=True))
    def test_no_manual_resume(self):
        runner=Mock();ResumeQueue(self.store).tick(self.owner,runner);runner.assert_not_called()
    def test_finish_schedule(self):
        self.cmd('mode',{'value':'SAFE_SHUTDOWN'});r=self.cmd('finish',{'until':'2026-10-24 02:00','scope':'읽기 전용 시험'});self.assertIn('SCH-',r['id']);self.assertIn('서울',format_control('finish',r))
    def test_finish_requires_b(self):
        with self.assertRaises(Denied):self.cmd('finish',{'until':'2026-10-24 02:00'})
    def test_a_schedule_conflict(self):
        self.cmd('resume')
        with self.assertRaises(Denied):self.schedule()
    def test_b_a_conflict(self):
        self.schedule()
        with self.assertRaises(Denied):self.cmd('resume')
    def test_cancel(self):
        r=self.schedule();self.cmd('cancel',{'schedule_id':r['id'],'release_intake':True});self.assertEqual(self.cmd('resume')['operation_mode'],'AUTO_RESUME')
    def test_restart(self):
        self.cmd('resume');other=Store(self.root/'db.sqlite',{1},clock=lambda:self.now)
        with other.tx() as db:self.assertEqual(mode(db),'AUTO_RESUME')
    def test_cleanup(self):
        self.schedule();self.now+=51*60;self.assertEqual(self.decision(),'SCHEDULE_HOLD')
    def test_unsaved(self):self.assertEqual(shutdown_preview({},self.now)['state'],'HOLD')
    def test_shutdown(self):
        r=self.schedule();v=self.cmd('shutdown',{'schedule_id':r['id'],'confirm':r['id']});self.assertEqual(v['state'],'HOLD');self.assertEqual(v['preflight']['os_shutdown_calls'],0)

    def test_checkpoint_preserves_original(self):
        from collab.__main__ import sample
        task=self.store.command(self.owner,'new','task',sample('PlanningAgent'))['task_id']
        with self.store.tx() as db:before=dict(db.execute('SELECT * FROM tasks WHERE id=?',(task,)).fetchone())
        self.cmd('resume');self.observe(100,5);self.scheduler.tick()
        with self.store.tx() as db:
            point=dict(db.execute('SELECT * FROM operation_checkpoints WHERE task=?',(task,)).fetchone())
            after=dict(db.execute('SELECT * FROM tasks WHERE id=?',(task,)).fetchone())
            self.assertEqual(db.execute('SELECT state FROM operation_runtime').fetchone()[0],'WAITING_FOR_QUOTA_VERIFICATION')
        self.assertEqual(before,after);self.assertEqual(point['failure'],'QUOTA_EXHAUSTED');self.assertTrue(point['base_sha']);self.assertTrue(point['scope'])
    def test_running_switch_blocked(self):
        from collab.__main__ import sample
        task=self.store.command(self.owner,'new','task',sample('PlanningAgent'))['task_id']
        with self.store.tx() as db:db.execute("UPDATE tasks SET state='RUNNING' WHERE id=?",(task,))
        with self.assertRaises(Denied):self.cmd('resume')

class ModeResumeTests(fixtures.ResumeTests):
    def setUp(self):
        super().setUp()
        self.scheduler.command(self.owner,'mode-a','resume')
        from unittest.mock import patch
        self.github=patch('collab.adapters.GitHubReader.read',return_value={'sha':'a'*40})
        self.read=self.github.start();self.addCleanup(self.github.stop)
    def test_changed_sha_hold(self):
        self.enqueue();self.observe();self.read.return_value={'sha':'b'*40}
        runner=Mock();self.queue.tick(self.owner,runner);runner.assert_not_called();self.assertEqual('HOLD',self.state())
    def test_unknown_sha_hold(self):
        self.enqueue();self.observe();self.read.side_effect=Denied('offline')
        runner=Mock();self.queue.tick(self.owner,runner);self.queue.tick(self.owner,runner)
        runner.assert_not_called();self.assertEqual('HOLD',self.state());self.assertEqual(self.read.call_count,1)
