"""새 실제 인계 한 건의 fake CLI 테스트. 실제 모델/Discord 호출은 없습니다."""
import hashlib
import json
import tempfile
import unittest
from pathlib import Path
from types import SimpleNamespace
from collab.auto63 import run_chain,canonical
from collab.core import Store,Identity,Denied
from collab.real_roles import run_once,AUTO_CODEX_SLOT


class Auto63Tests(unittest.TestCase):
    def setUp(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        self.tmp=tempfile.TemporaryDirectory(dir='data/tmp');self.root=Path(self.tmp.name).resolve()
        self.store=Store(self.root/'db.sqlite',{1},clock=lambda:1792720800);self.owner=Identity(1,1,1)
        docs=[]
        for path in ('AGENTS.md','문서/담당/PLANNING.md','문서/담당/CODEX.md'):
            content='합성 역할 지침 '+path;b=content.encode()
            docs.append({'path':path,'commit':'a'*40,'content':content,'blob':hashlib.sha1(b'blob '+str(len(b)).encode()+b'\0'+b).hexdigest()})
        self.sources={'docs':docs,'issue':{'body':'합성 독침벌 Issue63'},'comments':[{'url':'https://example.invalid/mock','body':'합성 미술승인/권리 보류'}]}
        self.calls=[];self.sent=[];self.fail_role=None;self.bad_target=False;self.fail_publish=False

    def tearDown(self):self.tmp.cleanup()

    def invoke(self,command,**kwargs):
        if '--version' in command:return SimpleNamespace(returncode=0,stdout='codex-cli fake',stderr='')
        if 'login' in command:return SimpleNamespace(returncode=0,stdout='using ChatGPT',stderr='')
        if 'features' in command:return SimpleNamespace(returncode=0,stdout='\n'.join(k+' stable false' for k in ('shell_tool','apps','browser_use','computer_use','multi_agent')),stderr='')
        prompt=kwargs['input'];role='PlanningAgent' if prompt.startswith('You are PlanningAgent') else 'CodexAgent'
        self.calls.append((role,prompt))
        answer={'role':role,'summary':'Codex에 독침벌 권리와 승인 조건의 문서 분석을 요청합니다.' if role=='PlanningAgent' else '단일포즈 기술PASS, 권리 및 대표 미술승인 대기입니다.',
                'next_review':'CodexAgent' if role=='PlanningAgent' else 'PlanningAgent'}
        if self.bad_target and role=='PlanningAgent':answer['next_review']='NewArtAgent'
        text=json.dumps(answer);Path(command[command.index('--output-last-message')+1]).write_text(text,encoding='utf8')
        events=[{'type':'item.completed','item':{'type':'agent_message','text':text}},{'type':'turn.completed','usage':{'input_tokens':10,'output_tokens':5}}]
        if role==self.fail_role:events.append({'type':'error','message':'SYNTHETIC_SECRET_NOT_REAL'})
        return SimpleNamespace(returncode=0,stdout='\n'.join(json.dumps(x) for x in events),stderr='')

    def runner(self,*args):return run_once(*args,invoke=self.invoke)

    def publish(self,text):
        if self.fail_publish:raise RuntimeError('synthetic network failure')
        self.sent.append(text);return {'message_id':'MOCK-'+str(len(self.sent))}

    def run_job(self):return run_chain(self.store,self.owner,self.sources,self.root/'job','fake',self.publish,self.runner)

    def test_success_exact_request_and_reply_return(self):
        r=self.run_job();self.assertEqual('COMPLETED',r['state']);self.assertEqual(2,len(self.calls));self.assertEqual(3,len(self.sent))
        self.assertEqual(r['request_sha256'],r['forwarded_request_sha256'])
        self.assertIn(canonical(r['request']),self.calls[1][1])
        evidence=json.loads(self.store.get(r['planning_task'])['evidence'])
        self.assertEqual(r['codex_task'],evidence['automatic_return']['from_task'])
        self.assertEqual(r['returned_answer_sha256'],evidence['automatic_return']['answer_sha256'])
        self.assertEqual(r['planning_task'],self.store.get(r['codex_task'])['root'])
        self.assertTrue(all(t['art']=='USER_REVIEW_PENDING' for t in self.store.snapshot()['tasks']))

    def test_same_job_never_restarts(self):
        self.run_job()
        with self.assertRaises(Denied):self.run_job()
        self.assertEqual(2,len(self.calls))

    def test_planning_failure_stops_before_codex(self):
        self.fail_role='PlanningAgent';r=self.run_job()
        self.assertEqual('FAILED',r['state']);self.assertEqual(1,len(self.calls));self.assertFalse(self.sent)
        self.assertNotIn('SYNTHETIC_SECRET',json.dumps(r))

    def test_codex_failure_no_reply_attached(self):
        self.fail_role='CodexAgent';r=self.run_job()
        self.assertEqual('FAILED',r['state']);self.assertEqual(2,len(self.calls));self.assertFalse(r['reply_received'])
        self.assertNotIn('automatic_return',json.loads(self.store.get(r['planning_task'])['evidence']))

    def test_discord_failure_stops_before_second_ai(self):
        self.fail_publish=True;r=self.run_job();self.assertEqual('FAILED',r['state']);self.assertEqual(1,len(self.calls))

    def test_wrong_target_never_starts_art_or_codex(self):
        self.bad_target=True;r=self.run_job();self.assertEqual('FAILED',r['state']);self.assertEqual(1,len(self.calls))
        with self.store.tx() as db:self.assertIsNone(db.execute('SELECT value FROM settings WHERE key=?',(AUTO_CODEX_SLOT,)).fetchone())

    def test_tampered_source_no_job_or_model(self):
        self.sources['docs'][0]['content']='변조'
        with self.assertRaises(Denied):self.run_job()
        self.assertFalse(self.calls)
