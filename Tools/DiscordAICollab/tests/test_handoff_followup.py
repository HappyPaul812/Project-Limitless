import tempfile
import unittest
from pathlib import Path
from collab.core import Store, Identity, Denied
from collab.handoff_rehearsal import prepare, advance


class FollowupTests(unittest.TestCase):
    def test_one_case_restart_and_approval_gate(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        with tempfile.TemporaryDirectory(dir='data/tmp') as tmp:
            store=Store(Path(tmp)/'queue.sqlite',{1})
            owner=Identity(1,1,1)
            root=prepare(store,owner)
            result=advance(store,owner,root)
            self.assertEqual(0,result['fail'])
            self.assertEqual(root,prepare(store,owner))
            again=advance(store,owner,root)
            self.assertEqual(result,again)
            last=result['tasks'][-1]['id']
            with self.assertRaises(Denied):
                store.command(owner,'no-real-art','approve',{'task_id':last,'sha':'0'*64,'scope':'MOCK_REVIEW_ONLY'})
            self.assertEqual('APPROVAL_PENDING',store.get(last)['state'])
