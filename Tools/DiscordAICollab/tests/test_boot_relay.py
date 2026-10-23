"""부팅 준비 격리 검사. 서비스 계정/태스크/인터넷 장애를 실제 생성하지 않습니다."""
import json
import os
import tempfile
import unittest
import xml.etree.ElementTree as ET
from pathlib import Path
from types import SimpleNamespace
from unittest.mock import Mock,patch
from tests import test_operations as fixtures
from collab.boot_relay import Supervisor,delay,prerequisites
from collab.bot import create_bot,run_bot
from collab.core import Denied

class SupervisorTests(unittest.TestCase,fixtures.Fixture):
    def setUp(self):
        self.setup()
        for rel in ('data/phase2/relay.sqlite','data/phase3/config.json','data/private/boot-discord/connection.json'):
            path=self.root/rel;path.parent.mkdir(parents=True,exist_ok=True);path.write_text('TEST_ONLY')
    tearDown=fixtures.Fixture.cleanup
    def worker(self,**kwargs):return Supervisor('Bot',self.root,sleep=lambda _:None,**kwargs)
    def test_missing_service_account_store_never_uses_user_store(self):
        (self.root/'data/private/boot-discord/connection.json').unlink()
        original=self.root/'data/private/discord/connection.json';original.parent.mkdir(parents=True);original.write_text('DO_NOT_READ')
        runner=Mock();self.assertIn('HOLD_STORAGE',self.worker(network=lambda:True,launch=runner).run());runner.assert_not_called()
    def test_wifi_delay_then_connected(self):
        sleeper=Mock();worker=Supervisor('Bot',self.root,network=Mock(side_effect=[False,False,True]),sleep=sleeper,
            launch=Mock(return_value=SimpleNamespace(poll=lambda:0,returncode=0)))
        self.assertEqual(worker.run(),'STOPPED_SAFELY');self.assertEqual(worker.attempts,1);self.assertEqual(sleeper.call_count,2)
    def test_offline_bounded_no_launch(self):
        launch=Mock();worker=self.worker(network=lambda:False,launch=launch)
        self.assertEqual(worker.run(max_waits=2),'WAITING_FOR_NETWORK_REVIEW_REQUIRED');launch.assert_not_called()
    def test_auth_or_runtime_error_no_blind_retry(self):
        launch=Mock(return_value=SimpleNamespace(poll=lambda:1,returncode=1));worker=self.worker(network=lambda:True,launch=launch)
        self.assertEqual(worker.run(),'HOLD_AUTH_OR_RUNTIME_NO_BLIND_RETRY');self.assertEqual(launch.call_count,1)
    def test_disconnect_rechecks_before_restart(self):
        launch=Mock(side_effect=[SimpleNamespace(poll=lambda:1,returncode=1),SimpleNamespace(poll=lambda:0,returncode=0)])
        worker=self.worker(network=Mock(side_effect=[True,False,True]),launch=launch)
        self.assertEqual(worker.run(),'STOPPED_SAFELY');self.assertEqual(launch.call_count,2)
    def test_reconnect_limit(self):
        network=Mock(side_effect=[True,False]*3)
        launch=Mock(return_value=SimpleNamespace(poll=lambda:1,returncode=1))
        self.assertEqual(self.worker(network=network,launch=launch).run(),'HOLD_RECONNECT_LIMIT');self.assertEqual(launch.call_count,3)
    def test_owner_stop_before_launch(self):
        stop=self.root/'data/boot/Bot-stop';stop.parent.mkdir();stop.touch();launch=Mock()
        self.assertEqual(self.worker(network=lambda:True,launch=launch).run(),'STOPPED_BY_OWNER');launch.assert_not_called()
    def test_stop_during_child_writes_safe_marker_only(self):
        poll=Mock(side_effect=[None,0]);child=SimpleNamespace(poll=poll,returncode=0)
        def launch():
            folder=self.root/'data/boot';folder.mkdir();(folder/'Bot-stop').touch();return child
        worker=self.worker(network=lambda:True,launch=launch);self.assertEqual(worker.run(),'STOPPED_SAFELY')
        self.assertEqual((self.root/'data/phase2/bot-stop-request').read_text(),'STOP')
    def test_backoff_capped(self):self.assertEqual(delay(100),120);self.assertEqual(delay(0),5)
    def test_fixed_services(self):
        with self.assertRaises(ValueError):Supervisor('arbitrary shell',self.root)
    def test_boot_xml_offline_disabled_password_standard_account(self):
        ns={'t':'http://schemas.microsoft.com/windows/2004/02/mit/task'}
        for role in ('Art','Bot'):
            xml=ET.parse('data/boot/LIMITLESS-'+role+'-OnBoot.xml')
            self.assertIsNotNone(xml.find('.//t:BootTrigger',ns));self.assertIsNone(xml.find('.//t:LogonTrigger',ns))
            self.assertEqual(xml.findtext('.//t:LogonType',namespaces=ns),'Password')
            self.assertEqual(xml.findtext('.//t:RunLevel',namespaces=ns),'LeastPrivilege')
            self.assertEqual(xml.findtext('.//t:Settings/t:Enabled',namespaces=ns),'false')
            self.assertEqual(xml.findtext('.//t:MultipleInstancesPolicy',namespaces=ns),'IgnoreNew')
            self.assertEqual(xml.findtext('.//t:AllowHardTerminate',namespaces=ns),'false')
            self.assertNotIn('SYSTEM',xml.findtext('.//t:Principal/t:UserId',namespaces=ns))
    def test_boot_run_has_no_usage_or_ai_worker(self):
        with patch.dict(os.environ,{'LIMITLESS_OWNER_IDS':'1','LIMITLESS_GUILD_ID':'1','LIMITLESS_CHANNEL_IDS':'1',
                'DISCORD_BOT_TOKEN':'UNUSABLE_TEST_ONLY','LIMITLESS_ALLOW_DISCORD':'1','LIMITLESS_BOOT_RELAY_ONLY':'1'}),              patch('collab.phase2.discord_preflight',return_value={'status':'LOCAL_CONFIG_READY'}),              patch('collab.usage_bridge.UsageMonitor') as usage,patch('collab.bot.create_bot') as factory:
            factory.return_value.run=Mock();run_bot(self.root/'isolated_bot')
            usage.assert_not_called();self.assertIsNone(factory.call_args.args[2]);self.assertIsNone(factory.call_args.args[3])
            self.assertTrue(factory.call_args.kwargs['relay_only']);self.assertTrue(factory.return_value.run.call_args.kwargs['reconnect'])

class BootAdapterTests(unittest.IsolatedAsyncioTestCase,fixtures.Fixture):
    async def asyncSetUp(self):
        self.setup();self.bot=create_bot(self.store,self.root/'bot',relay_only=True)
    async def asyncTearDown(self):await self.bot.close();self.cleanup()
    async def test_mode_a_denied_and_usage_hold(self):
        from unittest.mock import AsyncMock
        async def call(name,number,**kwargs):
            result=SimpleNamespace(id=number+100,flags=SimpleNamespace(ephemeral=True))
            item=SimpleNamespace(id=number,user=SimpleNamespace(id=1,bot=False),guild_id=1,channel_id=1,
                response=SimpleNamespace(defer=AsyncMock()),followup=SimpleNamespace(send=AsyncMock(return_value=result)),type=None)
            await self.bot.tree.get_command('limitless').get_command(name).callback(item,**kwargs)
            return item.followup.send.call_args.args[0]
        self.assertIn('AI HOLD',await call('resume',1));self.assertIn('서비스 계정',await call('usage',2))
        with self.store.tx() as db:self.assertEqual(db.execute('SELECT mode FROM operation_mode').fetchone()[0],'MANUAL');self.assertEqual(db.execute('SELECT count(*) FROM usage').fetchone()[0],0)
