"""네 역할의 실제 중계 코드를 fake CLI로 검증. 실제 AI·Discord 호출0."""
import hashlib
import contextlib
import io
import json
import unittest
from pathlib import Path
from types import SimpleNamespace
from tests.test_art_roles import ArtRolesTests as Fixtures
from collab.four_role_pilot import run_pilot,ORDER,verify_packet
from collab.real_roles import run_once
from collab.core import Denied


class FourRolePilotTests(unittest.TestCase):
    def tearDown(self):
        self.redirect.__exit__(None,None,None);Fixtures.tearDown(self)
    def setUp(self):
        Fixtures.setUp(self)
        self.redirect=contextlib.redirect_stdout(io.StringIO());self.redirect.__enter__()
        for name in ('PLANNING','CODEX'):
            content='합성 역할 지침';b=content.encode()
            self.sources['docs'].append({'path':'x/'+name+'.md','commit':'a'*40,'content':content,'blob':hashlib.sha1(b'blob '+str(len(b)).encode()+b'\0'+b).hexdigest()})
        self.fail_role=None;self.wrong_target=False;self.tamper_parent=False;self.fail_message=0;self.prompts=[]
    def invoke(self,command,**kwargs):
        if 'exec' not in command:return Fixtures.invoke(self,command,**kwargs)
        role=kwargs['input'].split()[2];self.calls.append(role);self.prompts.append(kwargs['input'])
        self.assertEqual(90,kwargs['timeout'])
        summary='#59 -> NewArtAgent 검토 요청, #63 -> RenewalArtAgent 검토 요청입니다.' if role=='PlanningAgent' else role+' 실제 선행 요청에 대한 합성 답변입니다. 미술승인 대기/HOLD 유지.'
        target='NewArtAgent,RenewalArtAgent' if role=='PlanningAgent' else ('PlanningAgent' if role=='CodexAgent' else 'CodexAgent')
        if self.wrong_target:target='UnknownAgent'
        answer={'role':role,'summary':summary,'next_review':target};text=json.dumps(answer)
        Path(command[command.index('--output-last-message')+1]).write_text(text,encoding='utf8')
        events=[{'type':'item.completed','item':{'type':'agent_message','text':text}},{'type':'turn.completed','usage':{'input_tokens':10,'output_tokens':5}}]
        if role==self.fail_role:events.append({'type':'error','message':'synthetic'})
        return SimpleNamespace(returncode=0,stdout='\n'.join(json.dumps(x) for x in events),stderr='')
    def runner(self,*args):
        if self.tamper_parent and args[2]['role']=='NewArtAgent':
            with self.store.tx() as db:
                row=self.store.task(db,args[2]['parent_task']);e=json.loads(row['evidence']);e['answer']['summary']='tampered'
                db.execute('UPDATE tasks SET evidence=? WHERE id=?',(json.dumps(e),row['id']))
        return run_once(*args,invoke=self.invoke)
    def publish(self,text):
        if self.fail_message==len(self.sent)+1:raise RuntimeError('synthetic network failure')
        self.sent.append(text);return {'message_id':'FAKE-'+str(len(self.sent))}
    def run_job(self):return run_pilot(self.store,self.owner,self.sources,self.root/'job','fake',self.publish,self.runner)
    def test_success_exact_four_calls_transfers_and_return(self):
        r=self.run_job();self.assertEqual('COMPLETED',r['state']);self.assertEqual(list(ORDER),self.calls)
        self.assertEqual(4,r['actual_cli_attempts']);self.assertEqual(5,len(self.sent));self.assertEqual(3,len(r['transfers']))
        self.assertTrue(r['reply_received']);self.assertFalse(r['manual_relay'])
        root=self.store.get(r['stages'][0]['task_id']);e=json.loads(root['evidence'])
        self.assertEqual(r['stages'][3]['answer'],e['four_role_return']['answer'])
        self.assertTrue(all(self.store.get(t['id'])['root']==root['id'] and t['art']=='USER_REVIEW_PENDING' for t in self.store.snapshot()['tasks']))
        self.assertEqual('REVIEW_PENDING',root['state'])
        for role in ORDER[1:]:
            p=json.loads((self.root/'job'/(role+'-plan.json')).read_text(encoding='utf8'));verify_packet(p,r['stages'])
    def test_duplicate_never_restarts(self):
        self.run_job()
        with self.assertRaises(Denied):self.run_job()
        self.assertEqual(4,len(self.calls))
    def test_planning_failure_stops_all(self):
        self.fail_role='PlanningAgent';self.assertEqual('FAILED',self.run_job()['state']);self.assertEqual(['PlanningAgent'],self.calls)
    def test_newart_failure_stops_renewal_codex(self):
        self.fail_role='NewArtAgent';self.assertEqual('FAILED',self.run_job()['state']);self.assertEqual(list(ORDER[:2]),self.calls)
    def test_renewal_failure_stops_codex(self):
        self.fail_role='RenewalArtAgent';r=self.run_job();self.assertEqual('FAILED',r['state']);self.assertEqual(list(ORDER[:3]),self.calls)
    def test_codex_failure_no_return(self):
        self.fail_role='CodexAgent';r=self.run_job();self.assertEqual('FAILED',r['state']);self.assertFalse(r['reply_received']);self.assertEqual(4,len(self.calls))
    def test_discord_failure_stops_next_call(self):
        self.fail_message=1;self.assertEqual('FAILED',self.run_job()['state']);self.assertEqual(['PlanningAgent'],self.calls)
    def test_wrong_routing_stops(self):
        self.wrong_target=True;self.assertEqual('FAILED',self.run_job()['state']);self.assertEqual(['PlanningAgent'],self.calls)
    def test_mutated_upstream_denied_before_next_exec(self):
        self.tamper_parent=True;self.assertEqual('FAILED',self.run_job()['state']);self.assertEqual(['PlanningAgent'],self.calls)
    def test_source_tampering_denied_without_call(self):
        self.sources['docs'][0]['content']='tampered'
        with self.assertRaises(Denied):self.run_job()
        self.assertEqual([],self.calls)


if __name__=='__main__':unittest.main()
