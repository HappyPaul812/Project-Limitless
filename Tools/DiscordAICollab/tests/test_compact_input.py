import copy
import hashlib
import unittest
from collab.compact_input import compact_candidate
from collab.core import Denied
from collab.real_roles import validate


class CompactTests(unittest.TestCase):
    def sources(self):
        files=[]
        for path,text in [('AGENTS.md','원본 보호·승인은 바울 대표만 결정합니다.'),('문서/담당/PLANNING.md','기획 역할 전체 원문'),('문서/인계/PLANNING_LATEST.md','최신 인계 발췌')]:
            b=text.encode();sha=hashlib.sha1(b'blob '+str(len(b)).encode()+b'\0'+b).hexdigest()
            files.append({'path':path,'content':text,'sha':sha,'commit':'a'*40,'ref':'docs/planning' if 'LATEST' in path else 'main'})
        return {'refs':{'main':'a'*40,'docs/planning':'b'*40},'files':files}

    def test_full_policy_and_role_retained(self):
        s=self.sources(); p=compact_candidate(s);validate(p)
        for f in s['files']: self.assertIn(f['content'],p['prompt'])
        self.assertEqual(s['refs'],p['source_refs']);self.assertFalse(p['auto_next_ai'])

    def test_duplicate_source_removed_and_input_not_modified(self):
        s=self.sources(); s['files'].append(copy.deepcopy(s['files'][0]));before=copy.deepcopy(s)
        p=compact_candidate(s)
        self.assertEqual(1,p['prompt'].count('SOURCE AGENTS.md'));self.assertEqual(before,s)

    def test_missing_policy_denied(self):
        s=self.sources();s['files']=s['files'][1:]
        with self.assertRaises(Denied):compact_candidate(s)

    def test_truncated_policy_blob_denied(self):
        s=self.sources();s['files'][0]['content']='잘린 원문'
        with self.assertRaises(Denied):compact_candidate(s)
