"""대표 첨부 재대조·TEST_ONLY 승인 차단만 검사합니다. 네트워크/AI 호출 없음."""
import hashlib
import json
import unittest
from pathlib import Path

from tests import test_art_delivery as fixtures
from collab.owner_receipt import mark_test_only
from collab.core import Denied


class OwnerReceiptTests(unittest.TestCase):
    setUp=fixtures.DeliveryTests.setUp
    tearDown=fixtures.DeliveryTests.tearDown

    def normal(self):
        result=self.engine.receive(self.owner,'discord:10:20','normal.png',self.data,source='DISCORD_ATTACHMENT')
        return {'event_key':'discord:10:20','task_id':result['task_id'],'sha256':result['sha256']}

    def mark(self,expected,**overrides):
        transport={'owner_verified':True,'normal_attachment':True,'original_report_verified':True,**overrides}
        return mark_test_only(self.engine,self.owner,expected,self.data,transport)

    def test_real_receipt_marked_without_new_task_or_file_change(self):
        expected=self.normal()
        with self.store.tx() as db:before=db.execute('SELECT count(*) FROM tasks').fetchone()[0]
        result=self.mark(expected)
        task=self.store.get(result['task_id'])
        self.assertEqual('BLOCKED',task['state'])
        self.assertTrue(json.loads(task['payload'])['test_only'])
        self.assertTrue(json.loads(task['evidence'])['test_only'])
        self.assertEqual(self.data,Path(result['artifact']).read_bytes())
        with self.store.tx() as db:self.assertEqual(before,db.execute('SELECT count(*) FROM tasks').fetchone()[0])

    def test_missing_owner_or_report_proof_rejected(self):
        expected=self.normal()
        for key in ('owner_verified','normal_attachment','original_report_verified'):
            with self.assertRaises(Denied):self.mark(expected,**{key:False})

    def test_sha_or_stored_tamper_rejected(self):
        expected=self.normal()
        with self.assertRaises(Denied):self.mark({**expected,'sha256':'a'*64})
        report=self.engine.report(expected['sha256'])
        task=self.store.get(expected['task_id'])
        Path(json.loads(task['payload'])['artifact']).write_bytes(b'tampered')
        with self.assertRaises(Denied):self.mark(expected)

    def test_self_upload_cannot_be_presented_as_owner_normal(self):
        result=self.engine.receive(self.owner,'discord:10:20','normal.png',self.data,source='DISCORD_SELF_TRANSPORT_TEST')
        expected={'event_key':'discord:10:20','task_id':result['task_id'],'sha256':result['sha256']}
        with self.assertRaises(Denied):self.mark(expected)

    def test_replay_one_test_report_and_no_extra_task(self):
        expected=self.normal();one=self.mark(expected);two=self.mark(expected)
        self.assertEqual(one['task_id'],two['task_id'])
        with self.store.tx() as db:
            self.assertEqual(1,db.execute("SELECT count(*) FROM art_delivery_outbox WHERE event_key LIKE 'owner-receipt-test:%'").fetchone()[0])
            self.assertEqual(1,db.execute('SELECT count(*) FROM tasks').fetchone()[0])

    def test_test_only_cannot_approve_even_after_state_changed(self):
        expected=self.normal();self.mark(expected)
        with self.store.tx() as db:db.execute("UPDATE tasks SET state='APPROVAL_PENDING',tech='TECH_PASS' WHERE id=?",(expected['task_id'],))
        payload=json.loads(self.store.get(expected['task_id'])['payload'])
        with self.assertRaisesRegex(Denied,'TEST_ONLY'):
            self.store.command(self.owner,'try-approve','approve',{'task_id':expected['task_id'],'sha':expected['sha256'],'scope':payload['scope']})
        with self.store.tx() as db:self.assertEqual(0,db.execute('SELECT count(*) FROM approvals').fetchone()[0])

    def test_other_tasks_unchanged(self):
        other=self.engine.receive(self.owner,'old','old.png',self.data+b'bad',source='WINDOWS_SHARED_INBOX')
        expected=self.normal()
        with self.store.tx() as db:
            rows=[tuple(r) for r in db.execute('SELECT * FROM art_deliveries WHERE sha!=?',(expected['sha256'],))]
        self.mark(expected)
        with self.store.tx() as db:self.assertEqual(rows,[tuple(r) for r in db.execute('SELECT * FROM art_deliveries WHERE sha!=?',(expected['sha256'],))])
