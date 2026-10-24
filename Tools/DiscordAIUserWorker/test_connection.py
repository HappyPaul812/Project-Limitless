"""기존 연결 테스트 fixture를 재사용합니다. 운영 DB/Discord/실제 CLI는 호출하지 않습니다."""
import json
from pathlib import Path
import sys
import unittest
from unittest.mock import Mock

HERE=Path(__file__).resolve().parent
sys.path.insert(0,str(HERE))
from test_queue import QueueTests, SID
from collab.core import Denied
from collab.real_roles import digest, grant_once
from collab.scheduling import Scheduler
from DiscordAIUserWorker.durable_queue import enqueue, inspect, execution_policy, existing_role_runner
from DiscordAIUserWorker.connect_offline import connect, snapshot


def run_local_coroutine(coroutine):
    # publish 모의 함수에는 I/O await가 없습니다. Windows IOCP를 만들지 않고 실제 outbox 코루틴만 재생합니다.
    try:
        coroutine.send(None)
    except StopIteration as done:
        return done.value
    finally:
        coroutine.close()
    raise AssertionError('OFFLINE_TEST_UNEXPECTED_EXTERNAL_AWAIT')


class ConnectionTests(QueueTests):
    # 기존20개도 같은 변경 코드에 회귀 적용합니다. 별도 모델 callback은 모두 모의입니다.
    def real_task_fixture(self, store, owner, plan, workspace, binary):
        with store.tx() as db:
            payload={'title':'OFFLINE FIXTURE','role':plan['role'],'issue':None,'base_sha':plan['source_refs']['main'],
                     'scope':plan['slot'],'protected_assets':['Unity'],'inputs':[],'verification':[],
                     'approval_required':True,'artifact':'NO_ART'}
            task=store._create(db,plan['slot'],payload)
            db.execute("UPDATE tasks SET state='RUNNING',lease='OFFLINE',lease_until=? WHERE id=?",(self.now+120,task))
        result={'task_id':task,'status':'PASS','mode':'REAL_CLI_READONLY','plan_sha256':plan['sha256']}
        store.finish(task,'OFFLINE','TECH_PASS',result)
        return result

    def test_new_art_to_codex_handoff_without_new_approval(self):
        self.add(role='NewArtAgent');self.runner.side_effect=self.real_task_fixture
        self.assertEqual('DONE',self.tick()['state'])
        with self.store.tx() as db:
            handoff=db.execute('SELECT * FROM user_worker_handoffs').fetchone()
            self.assertEqual('CodexAgent',handoff['next_role']);self.assertEqual('NEW',handoff['state'])
            self.assertEqual(1,db.execute("SELECT count(*) FROM tasks WHERE event_key LIKE 'handoff:%'").fetchone()[0])
        self.tick();self.runner.assert_called_once()

    def test_codex_to_planning_and_planning_to_owner(self):
        for role,target,state in (('CodexAgent','PlanningAgent','NEW'),('PlanningAgent','PlanningAgent','APPROVAL_PENDING')):
            self.add(role=role);self.runner.side_effect=self.real_task_fixture
            self.assertEqual('DONE',self.tick()['state'])
            with self.store.tx() as db:
                row=db.execute('SELECT * FROM user_worker_handoffs ORDER BY rowid DESC LIMIT 1').fetchone()
                self.assertEqual(target,row['next_role']);self.assertEqual(state,row['state'])

    def test_renewal_to_codex(self):
        self.add(role='RenewalArtAgent');self.runner.side_effect=self.real_task_fixture
        self.assertEqual('DONE',self.tick()['state'])
        with self.store.tx() as db:
            self.assertEqual('CodexAgent',db.execute('SELECT next_role FROM user_worker_handoffs').fetchone()[0])

    def test_failure_checkpoint_and_discord_outbox_once_offline(self):
        self.add();self.runner.side_effect=RuntimeError('PRIVATE_FAILURE_TEXT')
        self.assertEqual('NOT_VERIFIED',self.tick()['state'])
        self.tick()
        with self.store.tx() as db:
            self.assertEqual('NOT_VERIFIED',db.execute('SELECT state FROM user_worker_checkpoints').fetchone()[0])
            body=db.execute('SELECT body FROM control_outbox').fetchone()[0]
            self.assertIn('HOLD',body);self.assertNotIn('PRIVATE_FAILURE_TEXT',body)
        sent=[]
        async def publish(text):sent.append(text);return 'OFFLINE_MESSAGE_ID'
        run_local_coroutine(Scheduler(self.store).flush_reports(publish))
        run_local_coroutine(Scheduler(self.store).flush_reports(publish))
        self.assertEqual(1,len(sent))
        with self.store.tx() as db:self.assertEqual('SENT',db.execute('SELECT state FROM control_outbox').fetchone()[0])

    def test_quota_hold_checkpoint_without_runner(self):
        self.add()
        def exhausted(*args):
            value=self.observation();value['windows'][0]['used_percent']=100;return value
        self.usage.side_effect=exhausted
        self.assertEqual('PENDING_LOGIN',self.tick()['state']);self.runner.assert_not_called()
        with self.store.tx() as db:
            self.assertEqual('PENDING_LOGIN',db.execute('SELECT state FROM user_worker_checkpoints').fetchone()[0])
            self.assertEqual(1,db.execute('SELECT count(*) FROM control_outbox').fetchone()[0])

    def test_operational_migration_preserves_all_original_tables(self):
        with self.store.tx() as db:before=snapshot(db)
        value=connect(self.store.path,self.root/'backup')
        self.assertTrue(value['original_rows_preserved']);self.assertEqual(0,value['actual_ai_calls'])
        with self.store.tx() as db:self.assertEqual(before,snapshot(db))
        self.assertTrue(Path(value['backup']).exists())

    def test_registration_does_not_grant_execution(self):
        registry=self.root/'registry.json'
        registry.write_text(json.dumps({'kind':'USER_WORKER_INTEGRITY_ONLY','execution_permission':False}),encoding='utf8')
        with self.assertRaises(Denied):execution_policy(registry)

    def test_checkpoint_scope_checked_before_queueing(self):
        plan=self.plan(grant=False)
        with self.store.tx() as db:
            payload={'title':'OFFLINE CHECKPOINT','role':plan['role'],'issue':None,'base_sha':'a'*40,
                     'scope':plan['slot'],'protected_assets':[],'inputs':[],'verification':[],
                     'approval_required':True,'artifact':'NO_ART'}
            task=self.store._create(db,'CHECKPOINT_FIXTURE',payload)
            db.execute('INSERT INTO operation_checkpoints VALUES(?,?,?,?,?,?,?,?)',
                       (task,plan['role'],plan['slot'],'a'*40,'NEW','승인 범위 재확인','FIXTURE',self.now))
        plan['checkpoint_task']=task;plan['sha256']=digest({k:v for k,v in plan.items() if k!='sha256'})
        grant_once(self.store,self.owner,plan)
        enqueue(self.store,self.owner,plan,SID)
        with self.store.tx() as db:db.execute("UPDATE operation_checkpoints SET base_sha=?",('b'*40,))
        self.assertEqual('NO_ELIGIBLE_JOB',self.tick()['state']);self.runner.assert_not_called()


def load_tests(loader, tests, pattern):
    # fixture 상속으로 기존20개가 이 파일에서 두 번 실행되지 않도록 신규 연결8개만 고릅니다.
    return unittest.TestSuite(ConnectionTests(name) for name in ConnectionTests.__dict__ if name.startswith('test_'))


if __name__=='__main__':
    unittest.main()
