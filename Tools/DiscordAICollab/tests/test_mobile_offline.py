"""새 연결 경계만 합성 DB/실행기/Discord로 검증합니다. 실제 호출·조회는 없습니다."""
import json
import tempfile
import unittest
from pathlib import Path
from unittest.mock import patch

from collab.core import Identity, Denied
from collab.mobile_offline import OfflineMobileRelay, FakeExecutor, ORDER, digest


class MobileOfflineTests(unittest.TestCase):
    def setUp(self):
        scratch = Path('F:/study/codex/Project-Limitless/Temp/MobileOffline')
        scratch.mkdir(exist_ok=True)
        self.temp = tempfile.TemporaryDirectory(dir=scratch)
        self.root = Path(self.temp.name) / 'trial'
        self.now = 1792991000.0
        self.clock = lambda: self.now
        self.relay = OfflineMobileRelay.create(self.root, {1}, clock=self.clock)
        self.owner = Identity(1, 1, 1)
        self.contract = {'title': '합성 모바일 업무', 'role': ORDER[0], 'issue': 66,
                         'base_sha': 'a' * 40, 'scope': 'OFFLINE_TEXT_REVIEW',
                         'protected_assets': ['Unity', 'Save'], 'inputs': [],
                         'verification': ['MOCK_JSONL'], 'approval_required': True,
                         'artifact': '', 'offline_only': True}
        self.seq = 0

    def tearDown(self):
        self.temp.cleanup()

    def event(self):
        self.seq += 1
        return 'offline-event-' + str(self.seq)

    def task(self):
        return self.relay.mobile(self.owner, self.event(), 'task', self.contract)['task_id']

    def approve(self, task):
        return self.relay.mobile(self.owner, self.event(), 'approve-execution',
                                 {'task_id': task, 'sha256': digest(self.relay.plan(task)), 'expires': self.now + 120})

    def execute(self, task):
        self.approve(task)
        self.assertEqual('OFFLINE_PASS', self.relay.step(self.owner, task)['status'])
        self.assertEqual('SENT', self.relay.report(self.owner, task)['status'])

    def branch(self):
        root = self.task()
        self.execute(root)
        return root, self.relay.advance(self.owner, root)['tasks']

    def test_full_mobile_four_role_handoff_and_planning_return(self):
        root, arts = self.branch()
        for task in arts:
            self.execute(task)
        codex = self.relay.advance(self.owner, root)['tasks'][0]
        self.assertEqual(set(ORDER[1:3]), set(self.relay.plan(codex)['upstream']))
        self.execute(codex)
        self.assertEqual('OFFLINE_COMPLETE_OWNER_REVIEW', self.relay.advance(self.owner, root)['status'])
        self.assertEqual([root, *arts, codex], self.relay.executor.calls)
        self.assertEqual(4, len(self.relay.discord.messages))
        self.assertIn('four_role_return', json.loads(self.relay.store.get(root)['evidence']))
        with self.relay.store.tx() as db:
            self.assertEqual('MANUAL', db.execute('SELECT mode FROM operation_mode').fetchone()[0])
            self.assertEqual(0, db.execute('SELECT COUNT(*) FROM approvals').fetchone()[0])
            self.assertEqual(0, db.execute('SELECT COUNT(*) FROM usage').fetchone()[0])
        self.assertTrue(all(t['art'] == 'USER_REVIEW_PENDING' for t in self.relay.store.snapshot()['tasks']))

    def test_owner_guild_channel_and_bot_identity_denied(self):
        for identity in (Identity(2, 1, 1), Identity(1, 2, 1), Identity(1, 1, 2), Identity(1, 1, 1, True)):
            with self.subTest(identity=identity), self.assertRaises(Denied):
                self.relay.mobile(identity, self.event(), 'task', self.contract)
        self.assertEqual([], self.relay.store.snapshot()['tasks'])

    def test_duplicate_mobile_intake_and_changed_event(self):
        event = self.event()
        first = self.relay.mobile(self.owner, event, 'task', self.contract)
        self.assertEqual(first, self.relay.mobile(self.owner, event, 'task', self.contract))
        with self.assertRaises(Denied):
            self.relay.mobile(self.owner, event, 'task', dict(self.contract, title='changed'))

    def test_no_execution_approval_no_fake_call(self):
        self.assertEqual('HOLD', self.relay.step(self.owner, self.task())['status'])
        self.assertEqual([], self.relay.executor.calls)

    def test_wrong_scope_role_or_live_contract_rejected(self):
        for field, value in [('scope', 'WRITE'), ('role', 'TTSAgent'), ('offline_only', False), ('approval_required', False)]:
            with self.subTest(field=field), self.assertRaises(Denied):
                self.relay.mobile(self.owner, self.event(), 'task', dict(self.contract, **{field: value}))

    def test_wrong_digest_approval_rejected(self):
        task = self.task()
        with self.assertRaises(Denied):
            self.relay.mobile(self.owner, self.event(), 'approve-execution', {'task_id': task, 'sha256': '0' * 64, 'expires': self.now + 1})

    def test_expired_approval_hold_without_call(self):
        task = self.task()
        self.approve(task)
        self.now += 121
        self.assertEqual('HOLD', self.relay.step(self.owner, task)['status'])
        self.assertEqual([], self.relay.executor.calls)

    def test_approval_expiry_limit(self):
        task = self.task()
        for expires in (self.now - 1, self.now + 301, float('nan'), True):
            with self.subTest(expires=expires), self.assertRaises(Denied):
                self.relay.mobile(self.owner, self.event(), 'approve-execution', {'task_id': task, 'sha256': digest(self.relay.plan(task)), 'expires': expires})

    def test_duplicate_approval_receipt_does_not_reissue(self):
        task = self.task()
        args = {'task_id': task, 'sha256': digest(self.relay.plan(task)), 'expires': self.now + 120}
        event = self.event()
        first = self.relay.mobile(self.owner, event, 'approve-execution', args)
        self.relay.step(self.owner, task)
        self.assertEqual(first, self.relay.mobile(self.owner, event, 'approve-execution', args))
        self.relay.step(self.owner, task)
        self.assertEqual([task], self.relay.executor.calls)

    def test_changed_contract_before_execution(self):
        task = self.task()
        self.approve(task)
        with self.relay.store.tx() as db:
            payload = dict(self.contract, base_sha='b' * 40)
            db.execute('UPDATE tasks SET payload=? WHERE id=?', (json.dumps(payload), task))
        self.assertEqual('HOLD', self.relay.step(self.owner, task)['status'])
        self.assertEqual([], self.relay.executor.calls)

    def test_pause_and_fake_quota_block(self):
        task = self.task()
        self.approve(task)
        self.relay.fake_usage_available = False
        self.assertEqual('HOLD', self.relay.step(self.owner, task)['status'])
        self.assertEqual([], self.relay.executor.calls)
        self.relay.mobile(self.owner, self.event(), 'pause', {})
        with self.assertRaises(Denied):
            self.approve(self.task())

    def test_role_pause_blocks_next_handoff(self):
        root = self.task()
        self.execute(root)
        self.relay.mobile(self.owner, self.event(), 'pause', {'role': ORDER[1]})
        with self.assertRaises(Denied):
            self.relay.advance(self.owner, root)

    def test_failures_checkpoint_no_retry_no_handoff(self):
        for failure in ('timeout', 'exception', 'tool', 'error', 'invalid', 'exit', 'route'):
            with self.subTest(failure=failure):
                task = self.task()
                self.approve(task)
                self.relay.executor.failure = failure
                before = len(self.relay.executor.calls)
                self.assertEqual('HOLD', self.relay.step(self.owner, task)['status'])
                self.assertEqual('HOLD', self.relay.advance(self.owner, task)['status'])
                self.relay.step(self.owner, task)
                self.assertEqual(before + 1, len(self.relay.executor.calls))
                with self.relay.store.tx() as db:
                    self.assertIsNotNone(db.execute('SELECT * FROM operation_checkpoints WHERE task=?', (task,)).fetchone())

    def test_report_failure_before_or_after_send_never_retries(self):
        for failure in ('before', 'after'):
            with self.subTest(failure=failure):
                task = self.task()
                self.approve(task)
                self.relay.step(self.owner, task)
                self.relay.discord.failure = failure
                self.assertEqual('UNKNOWN_NO_RETRY', self.relay.report(self.owner, task)['status'])
                self.relay.discord.failure = None
                self.assertEqual('UNKNOWN_NO_RETRY', self.relay.report(self.owner, task)['status'])
                self.assertEqual('HOLD', self.relay.advance(self.owner, task)['status'])
                self.relay.step(self.owner, task)
        self.assertEqual(2, len(self.relay.executor.calls))

    def test_duplicate_report_and_handoff(self):
        root, arts = self.branch()
        self.assertEqual('SENT', self.relay.report(self.owner, root)['status'])
        self.assertEqual(arts, self.relay.advance(self.owner, root)['tasks'])
        self.assertEqual(1, len(self.relay.discord.messages))
        self.assertEqual(3, len(self.relay.store.snapshot()['tasks']))

    def test_fanin_requires_both_art_results_and_reports(self):
        root, arts = self.branch()
        self.execute(arts[0])
        self.assertEqual(arts, self.relay.advance(self.owner, root)['tasks'])
        self.assertEqual(3, len(self.relay.store.snapshot()['tasks']))

    def test_tampered_upstream_blocks_child_before_call(self):
        root, arts = self.branch()
        self.approve(arts[0])
        with self.relay.store.tx() as db:
            evidence = json.loads(self.relay.store.task(db, root)['evidence'])
            evidence['answer']['summary'] = 'tampered'
            db.execute('UPDATE tasks SET evidence=? WHERE id=?', (json.dumps(evidence), root))
        self.assertEqual('HOLD', self.relay.step(self.owner, arts[0])['status'])
        self.assertEqual([root], self.relay.executor.calls)

    def test_recovery_consumed_grant_cannot_run(self):
        task = self.task()
        self.approve(task)
        with self.relay.store.tx() as db:
            db.execute("UPDATE mobile_grants SET state='CONSUMED' WHERE task=?", (task,))
            db.execute("UPDATE tasks SET state='RUNNING',lease='old',lease_until=? WHERE id=?", (self.now + 90, task))
        reopened = OfflineMobileRelay.reopen(self.root, {1}, clock=self.clock)
        reopened.recover()
        reopened.step(self.owner, task)
        self.assertEqual([], reopened.executor.calls)
        self.assertEqual('NOT_VERIFIED', reopened.store.get(task)['state'])

    def test_real_callbacks_and_existing_paths_rejected(self):
        with self.assertRaises(Denied):
            OfflineMobileRelay.create(self.root, {1}, clock=self.clock)
        task = self.task()
        self.approve(task)
        self.relay.executor = lambda _: None
        with self.assertRaises(Denied):
            self.relay.step(self.owner, task)

    def test_lease_timeout_rejects_result_and_preserves_consumption(self):
        task = self.task()
        self.approve(task)
        original = FakeExecutor.execute
        def late(executor, plan):
            result = original(executor, plan)
            self.now += 91
            return result
        with patch.object(FakeExecutor, 'execute', late):
            self.assertEqual('HOLD', self.relay.step(self.owner, task)['status'])
        self.relay.step(self.owner, task)
        self.assertEqual([task], self.relay.executor.calls)

    def test_identity_configuration_cannot_change_on_reopen(self):
        with self.assertRaises(Denied):
            OfflineMobileRelay.reopen(self.root, {2}, clock=self.clock)

    def test_failure_report_is_hold_not_pass(self):
        task = self.task()
        self.approve(task)
        self.relay.executor.failure = 'timeout'
        result = self.relay.step(self.owner, task)
        self.assertEqual('TIMEOUT_NO_RETRY', result['reason'])
        self.relay.report(self.owner, task)
        self.assertIn('[MOCK / HOLD]', self.relay.discord.messages[task])

    def test_duplicate_worker_does_not_cancel_inflight_attempt(self):
        task = self.task()
        self.approve(task)
        original = FakeExecutor.execute
        def concurrent(executor, plan):
            other = OfflineMobileRelay.reopen(self.root, {1}, clock=self.clock)
            self.assertEqual('ALREADY_CONSUMED_NO_RETRY', other.step(self.owner, task)['status'])
            self.assertEqual([], other.executor.calls)
            return original(executor, plan)
        with patch.object(FakeExecutor, 'execute', concurrent):
            self.assertEqual('OFFLINE_PASS', self.relay.step(self.owner, task)['status'])
        self.assertEqual([task], self.relay.executor.calls)

    def test_mode_change_blocks_without_enabling_a_mode(self):
        task = self.task()
        self.approve(task)
        # 운영 설정이 아니라 합성 DB의 fail-closed 입력입니다.
        with self.relay.store.tx() as db:
            db.execute("UPDATE operation_mode SET mode='INVALID_TEST_MODE'")
        self.assertEqual('HOLD', self.relay.step(self.owner, task)['status'])
        self.assertEqual([], self.relay.executor.calls)


if __name__ == '__main__':
    unittest.main()
