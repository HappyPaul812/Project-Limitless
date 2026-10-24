"""새 서비스 연결 경로만 격리 검사합니다. 실제 인증·Discord·AI 호출 없음."""
import json
import os
from pathlib import Path
import unittest
from unittest.mock import AsyncMock,Mock,patch
from tests import test_operations as fixtures
from collab.core import Denied
from collab.service_ai import service_environment,probe,connect_boot_ai
from collab.usage_bridge import OfficialUsage,normalize
from collab.scheduling import Scheduler
from collab.real_roles import ROLE_FILES

class ServiceAITests(unittest.TestCase,fixtures.Fixture):
    setUp=fixtures.Fixture.setup
    tearDown=fixtures.Fixture.cleanup
    def context(self):
        profile=self.root/'LIMITLESSRelaySvc';(profile/'.codex').mkdir(parents=True,exist_ok=True)
        context={'sid':'TEST_SID','name':'TEST\\LIMITLESSRelaySvc','admin':False,'profile':str(profile)}
        approval={'service_sid':'TEST_SID','tool_root':str(self.root),'service_ai_bridge':'READONLY_AUTH_AND_RESUME'}
        env={'USERPROFILE':str(profile),'HOME':'OTHER_USER','CODEX_HOME':'OTHER_USER','OPENAI_API_KEY':'NEVER_FORWARD','DISCORD_BOT_TOKEN':'NEVER_FORWARD','PATH':'TEST_PATH'}
        return context,approval,env
    def test_wrong_account_never_queries_auth(self):
        context,approval,env=self.context();context['sid']='OTHER'
        with self.assertRaises(Denied):service_environment(context,approval,env,self.root)
    def test_no_admin_or_undeployed_bridge(self):
        context,approval,env=self.context();context['admin']=True
        with self.assertRaises(Denied):service_environment(context,approval,env,self.root)
        context['admin']=False;approval.pop('service_ai_bridge')
        with self.assertRaises(Denied):service_environment(context,approval,env,self.root)
    def test_only_own_home_and_no_api_or_discord_keys(self):
        context,approval,env=self.context();safe=service_environment(context,approval,env,self.root)
        self.assertEqual(safe['CODEX_HOME'],str(Path(context['profile'])/'.codex'))
        self.assertEqual(safe['HOME'],context['profile']);self.assertNotIn('OPENAI_API_KEY',safe);self.assertNotIn('DISCORD_BOT_TOKEN',safe)
        provider=OfficialUsage('FAKE',environment=safe);self.assertEqual(provider.environment,safe)
    def test_foreign_profile_or_uninitialized_is_hold(self):
        context,approval,env=self.context();env['USERPROFILE']=str(self.root/'LIMITLESSRelaySvcOther')
        with self.assertRaises(Denied):service_environment(context,approval,env,self.root)
        env['USERPROFILE']=context['profile'];(Path(context['profile'])/'.codex').rmdir()
        with self.assertRaises(Denied):service_environment(context,approval,env,self.root)
    def test_probe_failure_sanitizes_and_has_no_provider(self):
        factory=Mock(side_effect=Denied('SERVICE_ACCOUNT_CONTEXT_REQUIRED'))
        result,provider=probe(self.root,factory)
        self.assertEqual(result['reason'],'SERVICE_ACCOUNT_CONTEXT_REQUIRED');self.assertIsNone(provider)
        self.assertEqual(result['actual_ai_calls'],0);self.assertFalse(result['api_fallback'])
    def test_auth_rejection_keeps_relay_only_no_queue(self):
        data=self.root/'bot';data.mkdir()
        with patch('collab.service_ai.probe',return_value=({'status':'HOLD','reason':'CHATGPT_SUBSCRIPTION_REQUIRED'},None)),patch('collab.service_ai.ResumeQueue') as queue:
            monitor,resume,result=connect_boot_ai(self.store,data)
        self.assertIsNone(monitor);self.assertIsNone(resume);queue.assert_not_called()
        with self.store.tx() as db:self.assertEqual(db.execute('SELECT mode FROM operation_mode').fetchone()[0],'MANUAL')
    def test_success_connects_existing_monitor_queue_without_activating_a(self):
        data=self.root/'bot';data.mkdir();value=normalize(fixtures.raw(self.now),self.now)
        provider=Mock();provider.read=AsyncMock(return_value=value)
        result,_=probe(self.root,lambda root:provider)
        with patch('collab.service_ai.probe',return_value=(result,provider)):
            monitor,queue,result=connect_boot_ai(self.store,data)
        self.assertEqual(type(queue).__name__,'ResumeQueue');self.assertEqual(type(monitor).__name__,'UsageMonitor')
        with self.store.tx() as db:
            self.assertEqual(db.execute('SELECT mode FROM operation_mode').fetchone()[0],'MANUAL')
            self.assertEqual(db.execute('SELECT count(*) FROM operation_history').fetchone()[0],0)
            self.assertEqual(db.execute('SELECT count(*) FROM usage').fetchone()[0],0)
        runner=Mock();queue.tick(self.owner,runner);runner.assert_not_called()
    def test_exhausted_authenticated_account_is_connected_but_usage_hold(self):
        data=self.root/'bot';data.mkdir();value=normalize(fixtures.raw(self.now,100,100),self.now)
        provider=Mock();provider.read=AsyncMock(return_value=value)
        result,_=probe(self.root,lambda root:provider)
        self.assertEqual(result['status'],'READONLY_LINK_READY')
        with patch('collab.service_ai.probe',return_value=(result,provider)):
            monitor,queue,_=connect_boot_ai(self.store,data)
        self.assertEqual(self.decision(),'USAGE_HOLD')
    def test_interrupted_checkpoint_not_retried_on_connection(self):
        from collab.resume_queue import ResumeQueue
        queue=ResumeQueue(self.store)
        with self.store.tx() as db:db.execute('INSERT INTO control_resume VALUES(?,?,?,?,?,?,?,?)',('fake','{}','unused','unused','unused','RUNNING',None,self.now))
        data=self.root/'bot';data.mkdir();value=normalize(fixtures.raw(self.now),self.now);provider=Mock()
        with patch('collab.service_ai.probe',return_value=({'status':'READONLY_LINK_READY','usage':value},provider)):
            connect_boot_ai(self.store,data)
        with self.store.tx() as db:self.assertEqual(db.execute('SELECT state FROM control_resume').fetchone()[0],'NOT_VERIFIED')
    def test_four_verified_roles_reuse_existing_runner(self):
        from collab import real_roles, resume_queue
        self.assertEqual(set(ROLE_FILES),{'PlanningAgent','CodexAgent','NewArtAgent','RenewalArtAgent'})
        self.assertIn('run_once',real_roles.__dict__)
        self.assertNotIn('run_once',__import__('collab.service_ai',fromlist=['*']).__dict__)

class BootConnectionTests(unittest.TestCase,fixtures.Fixture):
    setUp=fixtures.Fixture.setup
    tearDown=fixtures.Fixture.cleanup
    def call(self,ready):
        from collab.bot import run_bot
        env={'LIMITLESS_OWNER_IDS':'1','LIMITLESS_GUILD_ID':'1','LIMITLESS_CHANNEL_IDS':'1','DISCORD_BOT_TOKEN':'FAKE_NOT_USABLE','LIMITLESS_ALLOW_DISCORD':'1','LIMITLESS_BOOT_RELAY_ONLY':'1'}
        monitor,queue=(Mock(),Mock()) if ready else (None,None)
        result={'status':'READONLY_LINK_READY' if ready else 'HOLD'}
        with patch.dict(os.environ,env),patch('collab.phase2.discord_preflight',return_value={'status':'LOCAL_CONFIG_READY'}),patch('collab.service_ai.connect_boot_ai',return_value=(monitor,queue,result)),patch('collab.bot.create_bot') as factory:
            run_bot(self.root/'boot')
            self.assertIs(factory.call_args.args[2],monitor);self.assertIs(factory.call_args.args[3],queue)
            self.assertEqual(factory.call_args.kwargs['relay_only'],not ready)
            self.assertTrue(factory.return_value.run.call_args.kwargs['reconnect'])
            factory.return_value.run.assert_called_once()
    def test_ready_reuses_one_existing_bot(self):self.call(True)
    def test_failed_auth_keeps_one_relay_bot(self):self.call(False)
