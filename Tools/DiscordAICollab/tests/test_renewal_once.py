"""Renewal 단발 경계만 fake CLI 검증. 실제 AI·Discord 호출0."""
import json
import unittest
from tests.test_art_roles import ArtRolesTests as Fixtures
from collab.renewal_once import prepare,execute
from collab.real_roles import run_once,NEW_ART_RETEST_SLOT
from collab.core import Denied


class RenewalOnceTests(unittest.TestCase):
    setUp=Fixtures.setUp
    tearDown=Fixtures.tearDown
    invoke=Fixtures.invoke
    def runner(self,*args):return run_once(*args,invoke=self.invoke)
    def run_trial(self):
        return execute(self.store,self.owner,prepare(self.sources,self.store.clock),self.root/'trial','fake',self.runner)
    def test_one_role_prior_newart_slot_preserved(self):
        old=json.dumps({'state':'CONSUMED','sha256':'old','role':'NewArtAgent'})
        with self.store.tx() as db:db.execute('INSERT INTO settings VALUES(?,?)',(NEW_ART_RETEST_SLOT,old))
        r=self.run_trial();self.assertEqual('PASS',r['status']);self.assertEqual(['RenewalArtAgent'],self.calls)
        self.assertEqual(1,len(self.store.snapshot()['tasks']));self.assertNotIn('handoff',r)
        with self.store.tx() as db:self.assertEqual(old,self.store.setting(db,NEW_ART_RETEST_SLOT))
    def test_duplicate_denied(self):
        self.run_trial()
        with self.assertRaises(Denied):self.run_trial()
        self.assertEqual(['RenewalArtAgent'],self.calls)
    def test_failure_never_starts_another_role(self):
        self.fail='RenewalArtAgent';r=self.run_trial();self.assertEqual('NOT_VERIFIED',r['status'])
        self.assertEqual(['RenewalArtAgent'],self.calls)
    def test_other_role_denied(self):
        p=prepare(self.sources,self.store.clock);p['role']='NewArtAgent'
        with self.assertRaises(Denied):execute(self.store,self.owner,p,self.root/'trial','fake',self.runner)
        self.assertEqual([],self.calls)


if __name__=='__main__':unittest.main()
