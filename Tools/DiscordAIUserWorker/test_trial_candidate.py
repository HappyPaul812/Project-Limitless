"""미배포 후보만 격리 DB와 모의 CLI로 검증합니다. 실제 인증·AI·Discord 호출은 없습니다."""
import importlib.util
import json
from pathlib import Path
import sys
import unittest
from types import SimpleNamespace
from unittest.mock import Mock

HERE=Path(__file__).resolve().parent
sys.path.insert(0,str(HERE))
from test_queue import QueueTests,SID
from collab.core import Denied,Identity
from collab.scheduling import Scheduler
from DiscordAIUserWorker.durable_queue import initialize


def load_candidate(name,path):
    spec=importlib.util.spec_from_file_location(name,path)
    module=importlib.util.module_from_spec(spec);spec.loader.exec_module(module)
    return module


class TrialTests(QueueTests):
    def setUp(self):
        super().setUp()
        self.roles=load_candidate('collab._planning_trial_candidate',HERE/'candidates/real_roles.py')
        self.queue=load_candidate('DiscordAIUserWorker._planning_trial_queue',HERE/'candidates/durable_queue.py')
        self.queue.validate=self.roles.validate
        self.trial=json.loads((HERE.parent/'DiscordAICollab/data/user-worker-trial/plan.json').read_text(encoding='utf8'))
        self.now=self.trial['created']
        self.exec_count=0
        self.summary='기획 정본을 관리합니다.\n결정을 문서에 기록합니다.'
        with self.store.tx() as db:
            initialize(db)
            self.old_slots=[self.roles.SLOT,self.roles.RETEST_SLOT,self.roles.CODEX_SLOT,self.roles.AUTO_PLANNING_SLOT,
                            self.roles.AUTO_CODEX_SLOT,self.roles.NEW_ART_SLOT,self.roles.RENEWAL_ART_SLOT,self.roles.NEW_ART_RETEST_SLOT,*self.roles.FOUR_SLOTS.values()]
            for slot in self.old_slots:db.execute('INSERT INTO settings VALUES(?,?)',(slot,json.dumps({'state':'CONSUMED','sha256':'a'*64})))
            db.execute('INSERT INTO settings VALUES(?,?)',(self.trial['slot'],json.dumps({'state':'WAITING_FOR_OWNER_APPROVAL','sha256':self.trial['sha256'],'role':'PlanningAgent','expires':0})))
            db.execute('INSERT INTO user_worker_jobs VALUES(?,?,?,?,?,?)',(self.trial['sha256'],json.dumps(self.trial),SID,'WAITING_FOR_OWNER_APPROVAL',None,self.now))
        Scheduler(self.store).record_official_usage(self.observation(),self.now)

    def fake_cli(self,command,**kwargs):
        if '--version' in command:return SimpleNamespace(returncode=0,stdout='codex-cli fixture',stderr='')
        if 'login' in command:return SimpleNamespace(returncode=0,stdout='Logged in using ChatGPT',stderr='')
        if 'features' in command:return SimpleNamespace(returncode=0,stdout='\n'.join(k+' stable false' for k in ('shell_tool','apps','browser_use','computer_use','multi_agent')),stderr='')
        self.exec_count+=1
        answer={'role':'PlanningAgent','summary':self.summary,'next_review':'바울 대표'}
        text=json.dumps(answer,ensure_ascii=False)
        Path(command[command.index('--output-last-message')+1]).write_text(text,encoding='utf8')
        events=[{'type':'item.completed','item':{'type':'agent_message','text':text}},
                {'type':'turn.completed','usage':{'input_tokens':12,'cached_input_tokens':0,'output_tokens':8}}]
        return SimpleNamespace(returncode=0,stdout='\n'.join(json.dumps(x) for x in events),stderr='')

    def run_candidate(self):
        return self.roles.run_once(self.store,self.owner,self.trial,self.root/'trial-empty',str(self.binary),invoke=self.fake_cli)

    def test_waiting_slot_cannot_invoke_even_cli(self):
        invoke=Mock()
        with self.assertRaises(Denied):self.roles.run_once(self.store,self.owner,self.trial,self.root/'trial-empty',str(self.binary),invoke=invoke)
        invoke.assert_not_called()

    def test_explicit_fixture_approval_consumes_only_new_slot_once(self):
        self.roles.approve_waiting_planning_trial(self.store,self.owner,self.trial)
        result=self.run_candidate();self.assertEqual('PASS',result['status'])
        self.assertEqual(1,self.exec_count)
        with self.assertRaises(Denied):self.run_candidate()
        self.assertEqual(1,self.exec_count)
        with self.store.tx() as db:
            for slot in self.old_slots:self.assertEqual('CONSUMED',json.loads(self.store.setting(db,slot))['state'])
            self.assertEqual(1,db.execute('SELECT count(*) FROM tasks').fetchone()[0])
            self.assertEqual('REVIEW_PENDING',db.execute('SELECT state FROM tasks').fetchone()[0])

    def test_six_sentences_rejected_before_tech_pass(self):
        self.summary='계약을 확인합니다.\n'*6
        self.roles.approve_waiting_planning_trial(self.store,self.owner,self.trial)
        result=self.run_candidate()
        self.assertEqual('NOT_VERIFIED',result['status'])
        self.assertEqual('PLANNING_TRIAL_CONTRACT_REJECTED',result['reason'])
        with self.store.tx() as db:self.assertEqual('NOT_VERIFIED',db.execute('SELECT tech FROM tasks').fetchone()[0])

    def test_wrong_owner_or_tampered_plan_not_approved(self):
        with self.assertRaises(Denied):self.roles.approve_waiting_planning_trial(self.store,Identity(2,1,1),self.trial)
        bad={**self.trial,'prompt':'TAMPERED'}
        with self.assertRaises(Denied):self.roles.approve_waiting_planning_trial(self.store,self.owner,bad)

    def test_trial_worker_no_extra_handoff_and_verified_outbox_only(self):
        self.roles.approve_waiting_planning_trial(self.store,self.owner,self.trial)
        worker=self.queue.UserWorker(self.store,self.owner,self.binary,self.sha,self.root/'isolated')
        self.queue.record_handoff=Mock(side_effect=AssertionError('NO_FOLLOWUP_ALLOWED'))
        def runner(*args):return self.roles.run_once(*args,invoke=self.fake_cli)
        result=worker.once(self.context,released=True,authenticate=lambda *args:True,
                           official_usage=lambda *args:self.observation(),source_refs=lambda plan:plan['source_refs'],runner=runner)
        self.assertEqual('DONE',result['state']);self.queue.record_handoff.assert_not_called()
        with self.store.tx() as db:
            self.assertEqual(1,db.execute('SELECT count(*) FROM tasks').fetchone()[0])
            self.assertEqual(0,db.execute('SELECT count(*) FROM user_worker_handoffs').fetchone()[0])
            self.assertIn('[REAL AI / VERIFIED]',db.execute('SELECT body FROM control_outbox').fetchone()[0])

    def test_failed_trial_never_queues_verified(self):
        self.summary='계약을 확인합니다.\n'*6
        self.roles.approve_waiting_planning_trial(self.store,self.owner,self.trial)
        worker=self.queue.UserWorker(self.store,self.owner,self.binary,self.sha,self.root/'isolated')
        def runner(*args):return self.roles.run_once(*args,invoke=self.fake_cli)
        result=worker.once(self.context,released=True,authenticate=lambda *args:True,
                           official_usage=lambda *args:self.observation(),source_refs=lambda plan:plan['source_refs'],runner=runner)
        self.assertEqual('NOT_VERIFIED',result['state'])
        with self.store.tx() as db:self.assertNotIn('VERIFIED]',db.execute('SELECT body FROM control_outbox').fetchone()[0])


def load_tests(loader,tests,pattern):
    return unittest.TestSuite(TrialTests(name) for name in TrialTests.__dict__ if name.startswith('test_'))


if __name__=='__main__':unittest.main()
