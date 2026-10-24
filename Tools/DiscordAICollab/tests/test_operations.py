"""운영 연결 변경분만 검사. 모델·Discord·종료·작업 등록 호출 없음."""
import asyncio
import copy
import json
import tempfile
import unittest
from datetime import datetime
from pathlib import Path
from types import SimpleNamespace
from unittest.mock import AsyncMock,Mock

from collab.core import Store,Identity,Denied,KST
from collab.scheduling import Scheduler,gate
from collab.usage_bridge import OfficialUsage,UsageMonitor,normalize,ALLOWED
from collab.resume_queue import ResumeQueue
from collab.real_roles import make_plan,grant_once,ROLE_FILES
from collab.operations_safety import pc_preflight
from collab.query_text import kind


def raw(now,primary=20,secondary=3):
    return {'ordinaryUsageAllowed':True,'rateLimitsByLimitId':{'codex':{'limitId':'codex',
            'primary':{'usedPercent':primary,'windowDurationMins':300,'resetsAt':now+600},
            'secondary':{'usedPercent':secondary,'windowDurationMins':10080,'resetsAt':now+1200}}},
            'accountId':'DO_NOT_PERSIST','rateLimitResetCredits':{'credits':['DO_NOT_PERSIST']}}


class Fixture:
    def setup(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        self.temp=tempfile.TemporaryDirectory(dir='data/tmp');self.root=Path(self.temp.name).resolve()
        self.now=datetime(2026,10,24,1,tzinfo=KST).timestamp()
        self.store=Store(self.root/'db.sqlite',{1},clock=lambda:self.now)
        self.scheduler=Scheduler(self.store);self.owner=Identity(1,1,1)
    def cleanup(self):self.temp.cleanup()
    def observe(self,primary=20,secondary=3):
        value=normalize(raw(self.now,primary,secondary),self.now)
        self.scheduler.record_official_usage(value,self.now)
    def decision(self):
        with self.store.tx() as db:return gate(db,self.now,ai=True)


class UsageTests(unittest.TestCase,Fixture):
    setUp=Fixture.setup;tearDown=Fixture.cleanup
    def test_only_read_rpc_allowlist(self):
        self.assertEqual({'initialize','initialized','account/read','account/rateLimits/read'},ALLOWED)
        self.assertNotIn('turn/start',ALLOWED)
    def test_whitelist_normalization_drops_account_and_credits(self):
        value=normalize(raw(self.now),self.now)
        self.assertEqual('CODEX_APP_SERVER_OFFICIAL',value['source'])
        self.assertNotIn('DO_NOT_PERSIST',json.dumps(value))
        self.assertEqual([300,10080],[w['minutes'] for w in value['windows']])
    def test_missing_ordinary_permission_is_not_guessed(self):
        item=raw(self.now);item.pop('ordinaryUsageAllowed')
        with self.assertRaises(Denied):normalize(item,self.now)
    def test_missing_or_wrong_window_blocks(self):
        for field in ('primary','secondary'):
            item=raw(self.now);item['rateLimitsByLimitId']['codex'][field]=None
            with self.assertRaises(Denied):normalize(item,self.now)
    def test_wrong_bucket_or_nan_rejected(self):
        item=raw(self.now);item['rateLimitsByLimitId']['codex']['limitId']='another'
        with self.assertRaises(Denied):normalize(item,self.now)
        with self.assertRaises(Denied):normalize(raw(self.now,float('nan')),self.now)
    def test_failure_invalidates_even_previous_available_snapshot(self):
        self.observe();self.assertIsNone(self.decision())
        self.scheduler.invalidate_usage('READ_FAILED')
        self.assertEqual('USAGE_HOLD',self.decision())
    def test_two_exhausted_windows_use_later_reset(self):
        self.observe(100,100)
        self.assertEqual(self.now+1200,self.scheduler.tick()['usage']['recheck_at'])
        self.assertEqual('USAGE_HOLD',self.decision())
    def test_exhausted_backend_denial_preserves_known_reset(self):
        value=raw(self.now,100);value['ordinaryUsageAllowed']=False
        normalized=normalize(value,self.now)
        self.scheduler.record_official_usage(normalized,self.now)
        self.assertEqual('EXHAUSTED',self.scheduler.tick()['usage']['status'])
        self.assertEqual(self.now+600,self.scheduler.tick()['usage']['recheck_at'])

    def test_release_intake_requires_explicit_confirmation(self):
        sid=self.scheduler.command(self.owner,'s','schedule',{'until':'2026-10-24 03:00'})['id']
        self.scheduler.command(self.owner,'f','finish',{'schedule_id':sid})
        self.assertEqual('CANCELLED_HOLD',self.scheduler.command(self.owner,'c','cancel',{'schedule_id':sid})['state'])
        result=self.scheduler.command(self.owner,'release','cancel',{'schedule_id':sid,'release_intake':True})
        self.assertEqual('CANCELLED',result['state'])
        with self.store.tx() as db:self.assertIsNone(gate(db,self.now,intake=True))
    def test_live_schedule_status_format_is_korean(self):
        from collab.scheduling import format_control
        self.scheduler.command(self.owner,'schedule','schedule',{'until':'2026-10-24 03:00'})
        with self.store.tx() as db:db.execute("UPDATE control_settings SET value='LIVE_CONTROL' WHERE key='mode'")
        value=self.scheduler.command(self.owner,'status','schedule-status')
        self.assertEqual('LIVE_CONTROL',value['mode'])
        self.assertIn('운영 제어',format_control('schedule-status',value))

    def test_shutdown_preflight_is_bound_but_never_enables_shutdown(self):
        with self.store.tx() as db:value=pc_preflight(self.store,db)
        self.assertEqual(16,len(value['pc_id']))
        self.assertFalse(value['shutdown_enabled'])
        self.assertEqual('UNKNOWN',value['unsaved_user_work'])
    def test_test_only_query_label_is_not_art_approval(self):
        self.assertIn('TEST_ONLY',kind({'payload':json.dumps({'test_only':True})}))


class MonitorTests(unittest.IsolatedAsyncioTestCase,Fixture):
    async def asyncSetUp(self):self.setup()
    async def asyncTearDown(self):self.cleanup()
    async def test_live_reader_success_cached_without_duplicate_queries(self):
        provider=SimpleNamespace(read=AsyncMock(return_value=normalize(raw(self.now),self.now)))
        monitor=UsageMonitor(self.scheduler,provider)
        await monitor.refresh();await monitor.refresh()
        self.assertEqual(1,provider.read.call_count)
        self.assertIsNone(self.decision())
    async def test_three_failures_open_circuit_and_do_not_guess(self):
        provider=SimpleNamespace(read=AsyncMock(side_effect=Denied('OFFICIAL_USAGE_TIMEOUT')))
        monitor=UsageMonitor(self.scheduler,provider)
        for _ in range(4):
            await monitor.refresh();self.now+=301
        self.assertEqual(3,provider.read.call_count)
        self.assertEqual('USAGE_HOLD',self.decision())
    async def test_recovery_is_rechecked_after_reset_then_gate_opens(self):
        provider=SimpleNamespace(read=AsyncMock(return_value=normalize(raw(self.now,100),self.now)))
        monitor=UsageMonitor(self.scheduler,provider)
        await monitor.refresh();self.assertEqual('USAGE_HOLD',self.decision())
        self.now+=601
        provider.read.return_value=normalize(raw(self.now,10),self.now)
        await monitor.refresh()
        self.assertEqual(2,provider.read.call_count)
        self.assertIsNone(self.decision())
    async def test_stop_marker_closes_without_cancelling_sdk_close(self):
        from collab.bot import create_bot
        bot=create_bot(self.store,self.root/'bot-stop')
        (self.root/'bot-stop/bot-stop-request').write_text('STOP')
        bot.control_timer=asyncio.create_task(bot.control_loop())
        await bot.control_timer
        await bot.stop_task
        self.assertTrue(bot.is_closed())
        self.assertFalse(bot.stop_task.cancelled())
        self.assertEqual('STOPPED',json.loads((self.root/'bot-stop/bot-service-status.json').read_text())['status'])

    async def test_unauthorized_usage_does_not_reach_provider(self):
        from collab.bot import create_bot
        monitor=SimpleNamespace(refresh=AsyncMock())
        bot=create_bot(self.store,self.root/'bot',monitor)
        interaction=SimpleNamespace(id=1,user=SimpleNamespace(id=2,bot=False),guild_id=1,channel_id=1,
                    response=SimpleNamespace(defer=AsyncMock()),followup=SimpleNamespace(send=AsyncMock()))
        try:await bot.tree.get_command('limitless').get_command('usage').callback(interaction)
        finally:await bot.close()
        monitor.refresh.assert_not_called()
        self.assertIn('바울 대표 ID',interaction.followup.send.call_args.args[0])


class ResumeTests(unittest.TestCase,Fixture):
    def setUp(self):
        self.setup();self.queue=ResumeQueue(self.store)
        sources={'refs':{'main':'a'*40},'files':[{'path':'문서/담당/'+n+'.md','ref':'main','content':'fixture'} for n in ROLE_FILES.values()]}
        self.plan=make_plan('PlanningAgent',sources,clock=lambda:self.now)
        self.binary=self.root/'official-test.exe';self.binary.write_bytes(b'FAKE_ONLY_NOT_EXECUTABLE')
        self.workspace=self.root/'isolated'
    tearDown=Fixture.cleanup
    def enqueue(self):
        grant_once(self.store,self.owner,self.plan)
        self.queue.defer(self.owner,self.plan,self.workspace,self.binary)
    def state(self):
        with self.store.tx() as db:return db.execute('SELECT state FROM control_resume').fetchone()[0]
    def test_unapproved_plan_cannot_enter_resume_queue(self):
        with self.assertRaises(Denied):self.queue.defer(self.owner,self.plan,self.workspace,self.binary)
    def test_unknown_usage_keeps_granted_job_held_without_runner_call(self):
        self.enqueue();runner=Mock()
        self.assertIsNone(self.queue.tick(self.owner,runner));runner.assert_not_called()
        self.assertEqual('HELD',self.state())
    def test_exact_approved_job_runs_at_most_once_after_recovery(self):
        self.enqueue();self.observe()
        runner=Mock(return_value={'status':'PASS','mode':'REAL_CLI_READONLY','answer':{'role':'PlanningAgent','summary':'FAKE ONLY'}})
        self.queue.tick(self.owner,runner);self.queue.tick(self.owner,runner)
        self.assertEqual(1,runner.call_count);self.assertEqual('DONE',self.state())
        self.assertEqual(self.plan,runner.call_args.args[2])
    def test_expired_grant_not_extended_or_retried(self):
        self.enqueue();self.now+=601;self.observe()
        runner=Mock();self.queue.tick(self.owner,runner)
        runner.assert_not_called();self.assertEqual('EXPIRED_OR_CONSUMED',self.state())
    def test_consumed_slot_not_reused(self):
        self.enqueue();self.observe()
        with self.store.tx() as db:
            value=json.loads(self.store.setting(db,self.plan['slot']));value['state']='CONSUMED'
            db.execute('UPDATE settings SET value=? WHERE key=?',(json.dumps(value),self.plan['slot']))
        runner=Mock();self.queue.tick(self.owner,runner);runner.assert_not_called()
    def test_failed_attempt_has_no_auto_retry(self):
        self.enqueue();self.observe()
        runner=Mock(side_effect=Denied('FIXTURE_ONLY'))
        self.queue.tick(self.owner,runner);self.queue.tick(self.owner,runner)
        self.assertEqual(1,runner.call_count);self.assertEqual('NOT_VERIFIED',self.state())
    def test_restart_running_is_unknown_never_retried(self):
        self.enqueue();self.observe()
        with self.store.tx() as db:db.execute("UPDATE control_resume SET state='RUNNING'")
        self.queue.recover();runner=Mock();self.queue.tick(self.owner,runner)
        runner.assert_not_called();self.assertEqual('NOT_VERIFIED',self.state())
    def test_changed_executable_blocks(self):
        self.enqueue();self.observe();self.binary.write_bytes(b'changed')
        runner=Mock();self.queue.tick(self.owner,runner)
        runner.assert_not_called();self.assertEqual('NOT_VERIFIED',self.state())
    def test_corrupt_pending_plan_is_not_retried(self):
        self.enqueue();self.observe()
        with self.store.tx() as db:db.execute("UPDATE control_resume SET plan='invalid-json'")
        runner=Mock();self.queue.tick(self.owner,runner)
        runner.assert_not_called();self.assertEqual('NOT_VERIFIED',self.state())

    def test_sunday_manual_never_runs(self):
        self.scheduler.command(self.owner,'manual-sunday','mode',{'value':'MANUAL'})
        self.enqueue();self.now=datetime(2026,10,25,1,tzinfo=KST).timestamp();self.observe()
        runner=Mock();self.queue.tick(self.owner,runner);runner.assert_not_called()
