"""가짜 subprocess만 사용합니다. 실제 모델 호출/토큰 사용은 없습니다."""
import json
import subprocess
import tempfile
import unittest
from pathlib import Path
from types import SimpleNamespace
from unittest.mock import Mock

from collab.core import Store, Identity, Denied
from collab.real_roles import SLOT, RETEST_SLOT, CODEX_SLOT, ROLE_FILES, digest, make_plan, grant_once, run_once, parse_result, discord_text


class RealRoleTests(unittest.TestCase):
    def setUp(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        self.tmp=tempfile.TemporaryDirectory(dir='data/tmp')
        self.root=Path(self.tmp.name).resolve()
        self.now=1792720800  # 금요일 KST 고정 시각
        self.store=Store(self.root/'queue.sqlite',{1},clock=lambda:self.now)
        self.owner=Identity(1,1,1)
        self.sources={'refs':{'main':'a'*40},'files':[
            {'path':'문서/담당/'+name+'.md','ref':'main','content':'모의 역할 문서'} for name in ROLE_FILES.values()]}
        self.plan=make_plan('PlanningAgent',self.sources,clock=lambda:self.now)

    def tearDown(self):
        self.tmp.cleanup()

    def fake(self,command,**kwargs):
        if '--version' in command: return SimpleNamespace(returncode=0,stdout='codex-cli test',stderr='')
        if 'login' in command: return SimpleNamespace(returncode=0,stdout='Logged in using ChatGPT',stderr='')
        if 'features' in command:
            return SimpleNamespace(returncode=0,stdout='\n'.join(k+' stable false' for k in ('shell_tool','apps','browser_use','computer_use','multi_agent')),stderr='')
        answer={'role':'PlanningAgent','summary':'독립 모의 모델 응답','next_review':'바울 대표'}
        if '--output-last-message' in command:
            Path(command[command.index('--output-last-message')+1]).write_text(json.dumps(answer),encoding='utf8')
        events=[{'type':'item.completed','item':{'type':'agent_message','text':json.dumps(answer)}},
                {'type':'turn.completed','usage':{'input_tokens':12,'output_tokens':8}}]
        return SimpleNamespace(returncode=0,stdout='\n'.join(json.dumps(x) for x in events),stderr='')

    def execute(self,invoke=None):
        return run_once(self.store,self.owner,self.plan,self.root/'empty','codex-test',invoke=invoke or self.fake)

    def test_four_independent_plans_share_one_slot(self):
        plans=[make_plan(role,self.sources) for role in ROLE_FILES]
        self.assertEqual(4,len({p['sha256'] for p in plans}))
        self.assertEqual({SLOT},{p['slot'] for p in plans})

    def test_unapproved_does_not_even_inspect_auth(self):
        invoke=Mock()
        with self.assertRaises(Denied): self.execute(invoke)
        invoke.assert_not_called()

    def test_nonowner_cannot_grant(self):
        with self.assertRaises(Denied): grant_once(self.store,Identity(2,1,1),self.plan)

    def test_plan_tamper_denied(self):
        self.plan['prompt']+='altered'
        with self.assertRaises(Denied): grant_once(self.store,self.owner,self.plan)

    def test_expired_grant_denied_without_subprocess(self):
        grant_once(self.store,self.owner,self.plan); self.now+=601
        invoke=Mock()
        with self.assertRaises(Denied): self.execute(invoke)
        invoke.assert_not_called()

    def test_success_handoff_does_not_execute_second_ai(self):
        grant_once(self.store,self.owner,self.plan)
        invoke=Mock(side_effect=self.fake)
        result=self.execute(invoke)
        self.assertEqual('PASS',result['status'])
        self.assertEqual('APPROVAL_PENDING',result['handoff']['state'])
        self.assertEqual(1,sum('exec' in c.args[0] for c in invoke.call_args_list))
        self.assertTrue(all(t['art']=='USER_REVIEW_PENDING' for t in self.store.snapshot()['tasks']))
        self.assertIn('[REAL AI',discord_text(result))

    def test_consumed_slot_blocks_different_role_and_reexecution(self):
        grant_once(self.store,self.owner,self.plan); self.execute()
        with self.assertRaises(Denied): self.execute()
        with self.assertRaises(Denied): grant_once(self.store,self.owner,make_plan('NewArtAgent',self.sources))

    def test_timeout_no_retry_and_unknown_usage(self):
        grant_once(self.store,self.owner,self.plan)
        def invoke(command,**kwargs):
            if 'exec' in command: raise subprocess.TimeoutExpired(command,90)
            return self.fake(command,**kwargs)
        result=self.execute(invoke)
        self.assertEqual('NOT_VERIFIED',result['status'])
        self.assertEqual({},result['usage'])
        with self.assertRaises(Denied): self.execute(invoke)

    def test_tool_event_output_is_rejected(self):
        raw=json.dumps({'type':'item.completed','item':{'type':'command_execution'}})
        self.assertEqual('NOT_VERIFIED',parse_result(raw,0,'CodexAgent')['status'])

    def test_multiple_turns_and_wrong_role_rejected(self):
        raw=self.fake(['exec']).stdout
        self.assertEqual('NOT_VERIFIED',parse_result(raw,0,'CodexAgent')['status'])
        self.assertEqual('NOT_VERIFIED',parse_result(raw+'\n'+json.dumps({'type':'turn.completed'}),0,'PlanningAgent')['status'])

    def test_api_only_auth_never_executes(self):
        grant_once(self.store,self.owner,self.plan)
        calls=[]
        def invoke(command,**kwargs):
            calls.append(command)
            if 'login' in command: return SimpleNamespace(returncode=0,stdout='API key',stderr='')
            return self.fake(command,**kwargs)
        with self.assertRaises(Denied): self.execute(invoke)
        self.assertFalse(any('exec' in c for c in calls))

    def test_mock_cannot_be_labelled_real(self):
        self.assertIn('[MOCK',discord_text({'mode':'MOCK','status':'PASS'}))
        self.assertIn('[MOCK',discord_text({'mode':'MOCK_NO_AI','status':'PASS'}))

    def test_first_trial_other_roles_are_denied(self):
        with self.assertRaises(Denied):
            grant_once(self.store,self.owner,make_plan('CodexAgent',self.sources,clock=lambda:self.now))

    def test_explicit_retest_slot_preserves_consumed_first_slot(self):
        grant_once(self.store,self.owner,self.plan);self.execute()
        retest={**self.plan,'slot':RETEST_SLOT}
        retest['sha256']=digest({k:v for k,v in retest.items() if k!='sha256'})
        grant_once(self.store,self.owner,retest)
        result=run_once(self.store,self.owner,retest,self.root/'retest-empty','codex-test',invoke=self.fake)
        self.assertEqual('PASS',result['status'])
        with self.store.tx() as db:
            for slot in (SLOT,RETEST_SLOT):
                self.assertEqual('CONSUMED',json.loads(self.store.setting(db,slot))['state'])
        with self.assertRaises(Denied):grant_once(self.store,self.owner,retest)
        self.assertIn('[REAL AI / VERIFIED]',discord_text(result))

    def test_codex_slot_separates_role_and_never_runs_next_planning_ai(self):
        grant_once(self.store,self.owner,self.plan);first=self.execute()
        p={**self.plan,'slot':CODEX_SLOT,'role':'CodexAgent','issue':63}
        p['sha256']=digest({k:v for k,v in p.items() if k!='sha256'})
        grant_once(self.store,self.owner,p)
        def fake(command,**kwargs):
            result=self.fake(command,**kwargs)
            if 'exec' in command:
                result.stdout=result.stdout.replace('PlanningAgent','CodexAgent')
                Path(command[command.index('--output-last-message')+1]).write_text(json.dumps({'role':'CodexAgent','summary':'독침벌 검토 대기 요약','next_review':'기획'}),encoding='utf8')
                events=[{'type':'item.completed','item':{'type':'agent_message','text':json.dumps({'role':'CodexAgent','summary':'독침벌 검토 대기 요약','next_review':'기획'})}},{'type':'turn.completed','usage':{'input_tokens':1,'output_tokens':1}}]
                result.stdout='\n'.join(json.dumps(e) for e in events)
            return result
        invoked=Mock(side_effect=fake)
        result=run_once(self.store,self.owner,p,self.root/'codex-empty','fake',invoke=invoked)
        self.assertEqual('PASS',result['status']);self.assertIn('[REAL AI / VERIFIED / CodexAgent]',discord_text(result))
        self.assertEqual(1,sum('exec' in c.args[0] for c in invoked.call_args_list))
        self.assertEqual('NEW',result['handoff']['state'])
        self.assertEqual('APPROVAL_PENDING',self.store.get(first['task_id'])['state'])
        with self.assertRaises(Denied):grant_once(self.store,self.owner,p)

    def test_codex_slot_cannot_run_art_or_planning(self):
        for role in ('PlanningAgent','NewArtAgent','RenewalArtAgent'):
            p={**self.plan,'slot':CODEX_SLOT,'role':role};p['sha256']=digest({k:v for k,v in p.items() if k!='sha256'})
            with self.assertRaises(Denied):grant_once(self.store,self.owner,p)
