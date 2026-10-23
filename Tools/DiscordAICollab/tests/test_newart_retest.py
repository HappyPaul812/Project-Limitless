"""새 승인 슬롯 경계만 fake CLI로 검사합니다. 모델·Discord 호출0."""
import json
import unittest
from tests.test_art_roles import ArtRolesTests as Fixtures
from collab.newart_retest import prepare,execute
from collab.real_roles import run_once,NEW_ART_SLOT,NEW_ART_RETEST_SLOT
from collab.core import Denied


class NewArtRetestTests(unittest.TestCase):
    setUp=Fixtures.setUp
    tearDown=Fixtures.tearDown
    invoke=Fixtures.invoke
    def runner(self,*args):return run_once(*args,invoke=self.invoke)
    def run_trial(self):
        return execute(self.store,self.owner,prepare(self.sources,self.store.clock),self.root/'trial','fake',self.runner)
    def test_new_slot_one_role_old_consumed_preserved(self):
        old=json.dumps({'state':'CONSUMED','sha256':'old','role':'NewArtAgent'})
        with self.store.tx() as db:db.execute('INSERT INTO settings VALUES(?,?)',(NEW_ART_SLOT,old))
        r=self.run_trial();self.assertEqual('PASS',r['status']);self.assertEqual(['NewArtAgent'],self.calls)
        self.assertEqual(1,len(self.store.snapshot()['tasks']));self.assertNotIn('handoff',r)
        with self.store.tx() as db:self.assertEqual(old,self.store.setting(db,NEW_ART_SLOT))
    def test_duplicate_trial_denied(self):
        self.run_trial()
        with self.assertRaises(Denied):self.run_trial()
        self.assertEqual(['NewArtAgent'],self.calls)
    def test_failed_output_never_runs_renewal(self):
        self.fail='NewArtAgent';r=self.run_trial();self.assertEqual('NOT_VERIFIED',r['status'])
        self.assertEqual(['NewArtAgent'],self.calls);self.assertEqual(1,len(self.store.snapshot()['tasks']))
    def test_other_role_rejected_before_runner(self):
        plan=prepare(self.sources,self.store.clock);plan['role']='RenewalArtAgent'
        with self.assertRaises(Denied):execute(self.store,self.owner,plan,self.root/'trial','fake',self.runner)
        self.assertEqual([],self.calls)


if __name__=='__main__':unittest.main()
