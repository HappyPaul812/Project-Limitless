"""미진행 NEW 업무의 격리 재개만 검사합니다. 기존 Host/SDK 전체 테스트는 반복하지 않습니다."""
import hashlib
import importlib.util
from pathlib import Path
import sqlite3
import sys
import tempfile
import unittest

HERE = Path(__file__).resolve().parent
sys.path.insert(0, str(HERE.parent / 'DiscordAICollab'))
from collab.mobile_offline import OfflineMobileRelay
from collab.core import Identity

spec = importlib.util.spec_from_file_location('resume_host', HERE / 'mobile-trial-host.py')
host = importlib.util.module_from_spec(spec)
spec.loader.exec_module(host)


class ResumeTests(unittest.TestCase):
    def setUp(self):
        scratch = Path('F:/study/codex/Project-Limitless/Temp/RemainingWebUnit')
        scratch.mkdir(exist_ok=True)
        self.tmp = tempfile.TemporaryDirectory(dir=scratch)
        self.root = Path(self.tmp.name)
        relay = OfflineMobileRelay.create(self.root / 'Queue', {host.OWNER}, clock=lambda: 1792995000,
                                         guild=host.GUILD, channels=(host.CHANNEL,))
        payload = dict(title='합성 재개 검증', role='PlanningAgent', issue=66, base_sha='a'*40,
                       scope='OFFLINE_TEXT_REVIEW', protected_assets=[], inputs=[], verification=[],
                       approval_required=True, artifact='', offline_only=True)
        self.task = relay.mobile(Identity(host.OWNER, host.GUILD, host.CHANNEL), 'fixture', 'task', payload)['task_id']
        self.db = self.root / 'Queue/mobile-offline.sqlite'

    def tearDown(self):
        self.tmp.cleanup()

    def config(self):
        return dict(resume_unconsumed_queue=True, resume_task_id=self.task,
                    resume_queue_sha256=hashlib.sha256(self.db.read_bytes()).hexdigest())

    def test_new_without_grant_preserves_task(self):
        relay = host.open_trial_relay(self.root, self.config())
        self.assertEqual('NEW', relay.store.get(self.task)['state'])

    def test_changed_snapshot_rejected(self):
        cfg = self.config()
        cfg['resume_queue_sha256'] = '0'*64
        with self.assertRaises(ValueError): host.open_trial_relay(self.root, cfg)

    def mutate(self, sql, values=()):
        db = sqlite3.connect(self.db)
        try: db.execute(sql, values); db.commit()
        finally: db.close()

    def test_any_existing_grant_rejected_even_if_hash_matches(self):
        self.mutate('INSERT INTO mobile_grants VALUES(?,?,?,?)', (self.task, 'x', 9999999999, 'CONSUMED'))
        with self.assertRaises(ValueError): host.open_trial_relay(self.root, self.config())

    def test_non_new_task_rejected_even_if_hash_matches(self):
        self.mutate("UPDATE tasks SET state='TRIAGED'")
        with self.assertRaises(ValueError): host.open_trial_relay(self.root, self.config())


if __name__ == '__main__':
    unittest.main()
