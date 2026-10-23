"""추가 아트 담당 실행 경계만 fake CLI로 검증합니다. 실제 AI 호출0."""
import hashlib
import json
import tempfile
import unittest
from pathlib import Path
from types import SimpleNamespace
from collab.art_roles_once import run_pair,make_plan,ROLES
from collab.core import Store,Identity,Denied
from collab.real_roles import run_once,grant_once,digest,slot_role,AUTO_PLANNING_SLOT,AUTO_CODEX_SLOT


class ArtRolesTests(unittest.TestCase):
    def setUp(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        self.tmp=tempfile.TemporaryDirectory(dir='data/tmp');self.root=Path(self.tmp.name).resolve()
        self.store=Store(self.root/'db.sqlite',{1},clock=lambda:1792720800);self.owner=Identity(1,1,1)
        docs=[]
        for path in ('AGENTS.md','x/SPRITE_ART.md','x/SPRITE_ART_LATEST.md','x/WORLD_ART_RENEWAL.md','x/WORLD_ART_RENEWAL_LATEST.md'):
            content='합성 보호 지침';raw=content.encode()
            docs.append({'path':path,'commit':'a'*40,'content':content,'blob':hashlib.sha1(b'blob '+str(len(raw)).encode()+b'\0'+raw).hexdigest()})
        self.sources={'docs':docs,'refs':{'main':'a'*40},'comments':{str(n):[{'body':'합성 미술승인 대기'}] for n in (59,63,65,58)}}
        self.calls=[];self.sent=[];self.fail=None;self.fail_publish=False
    def tearDown(self):self.tmp.cleanup()
    def invoke(self,command,**kwargs):
        if '--version' in command:return SimpleNamespace(returncode=0,stdout='codex-cli fake',stderr='')
        if 'login' in command:return SimpleNamespace(returncode=0,stdout='using ChatGPT',stderr='')
        if 'features' in command:return SimpleNamespace(returncode=0,stdout='\n'.join(k+' stable false' for k in ('shell_tool','apps','browser_use','computer_use','multi_agent')),stderr='')
        role=kwargs['input'].split()[2];self.calls.append(role)
        self.assertEqual(90,kwargs['timeout']);self.assertIn('--sandbox',command)
        answer={'role':role,'summary':'문서상 기술과 미술 승인 대기를 구분합니다.','next_review':'대표 승인 대기'}
        text=json.dumps(answer);Path(command[command.index('--output-last-message')+1]).write_text(text,encoding='utf8')
        events=[{'type':'item.completed','item':{'type':'agent_message','text':text}},{'type':'turn.completed','usage':{'input_tokens':10,'output_tokens':5}}]
        if role==self.fail:events.append({'type':'error','message':'synthetic'})
        return SimpleNamespace(returncode=0,stdout='\n'.join(json.dumps(x) for x in events),stderr='')
    def runner(self,*args):return run_once(*args,invoke=self.invoke)
    def publish(self,text):
        if self.fail_publish:raise RuntimeError('synthetic')
        self.sent.append(text);return {'message_id':'FAKE'}
    def run_job(self):return run_pair(self.store,self.owner,self.sources,self.root/'job','fake',self.publish,self.runner)
    def test_two_verified_distinct_roles_no_next_job(self):
        r=self.run_job();self.assertEqual('COMPLETED',r['state']);self.assertEqual(list(ROLES),self.calls)
        self.assertEqual(2,len(self.store.snapshot()['tasks']))
        for role,msg in zip(ROLES,self.sent):self.assertIn('[REAL AI / VERIFIED / '+role+']',msg)
        self.assertTrue(all(t['art']=='USER_REVIEW_PENDING' for t in self.store.snapshot()['tasks']))
    def test_first_failure_stops(self):
        self.fail=ROLES[0];r=self.run_job();self.assertEqual('FAILED',r['state']);self.assertEqual([ROLES[0]],self.calls);self.assertEqual([],self.sent)
    def test_second_failure_no_verified_post(self):
        self.fail=ROLES[1];r=self.run_job();self.assertEqual('FAILED',r['state']);self.assertEqual(1,len(self.sent))
    def test_discord_failure_stops(self):
        self.fail_publish=True;self.assertEqual('FAILED',self.run_job()['state']);self.assertEqual([ROLES[0]],self.calls)
    def test_duplicate_denied(self):
        self.run_job()
        with self.assertRaises(Denied):self.run_job()
        self.assertEqual(2,len(self.calls))
    def test_wrong_slot_role_denied(self):
        p=make_plan(ROLES[0],self.sources,self.store.clock);p['role']=ROLES[1];p['sha256']=digest({k:v for k,v in p.items() if k!='sha256'})
        with self.assertRaises(Denied):grant_once(self.store,self.owner,p)
        self.assertEqual([],self.calls)
    def test_source_tampering_denied_before_call(self):
        self.sources['docs'][0]['content']='tampered'
        with self.assertRaises(Denied):self.run_job()
        self.assertEqual([],self.calls)
    def test_previous_roles_binding_preserved(self):
        self.assertEqual('PlanningAgent',slot_role(AUTO_PLANNING_SLOT));self.assertEqual('CodexAgent',slot_role(AUTO_CODEX_SLOT))


if __name__=='__main__':unittest.main()
