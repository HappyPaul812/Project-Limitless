"""합성 데이터만 사용: 실제 AI/Discord/Unity/Git 통합/Windows 종료 0회."""
import asyncio
import json
import tempfile
import unittest
from datetime import datetime
from pathlib import Path
from unittest.mock import AsyncMock, Mock, patch
from types import SimpleNamespace
from concurrent.futures import ThreadPoolExecutor

from collab.core import Store, Identity, Denied, KST
from collab.scheduling import Scheduler, gate, parse_time, shutdown_preview, format_control
from collab.__main__ import sample
from collab.real_roles import make_plan, run_once, grant_once, ROLE_FILES


class SafetyTests(unittest.TestCase):
    def setUp(self):
        Path('data/tmp').mkdir(parents=True, exist_ok=True)
        self.temp = tempfile.TemporaryDirectory(dir='data/tmp')
        self.root = Path(self.temp.name).resolve()
        self.now = datetime(2026, 10, 23, 12, tzinfo=KST).timestamp()
        self.store = Store(self.root/'db.sqlite', {1}, clock=lambda: self.now)
        self.owner = Identity(1, 1, 1)
        self.scheduler = Scheduler(self.store)
        self.sequence = 0

    def tearDown(self):
        self.temp.cleanup()

    def command(self, action, **args):
        self.sequence += 1
        return self.scheduler.command(self.owner, str(self.sequence), action, args)

    def schedule(self, **args):
        return self.command('schedule', **{'until': '23:00', **args})

    def decision(self, **args):
        with self.store.tx() as db:
            return gate(db, self.now, **args)

    def usage(self, first=10, second=20, reset=600):
        return self.scheduler.record_official_usage({'source': 'CODEX_APP_OFFICIAL_TOOL', 'ordinary_allowed': True, 'windows': [
            {'minutes': 300, 'used_percent': first, 'resets_at': self.now+reset},
            {'minutes': 10080, 'used_percent': second, 'resets_at': self.now+reset+100}]}, self.now)

    def task(self):
        return self.store.command(self.owner, 'task', 'task', sample('PlanningAgent'))['task_id']

    def test_register_seoul_and_default_cleanup(self):
        result = self.schedule()
        self.assertIn('22:50', result['stop_intake'])
        self.assertIn('23:00', result['deadline'])
        self.assertEqual(result['mode'], 'SIMULATION_ONLY')

    def test_invalid_time_and_past_never_rolls_to_tomorrow(self):
        for value in ('11:00', '25:00', 'tomorrow', '2026-11-30T20:00Z', None):
            with self.assertRaises(Denied):
                parse_time(value, self.now)

    def test_cleanup_time_required(self):
        for args in ({'until': '12:05'}, {'cleanup_minutes': 0}, {'cleanup_minutes': True}):
            with self.assertRaises(Denied):
                self.schedule(**args)

    def test_duplicate_event_reuses_schedule(self):
        args = {'until': '23:00'}
        one = self.scheduler.command(self.owner, 'same', 'schedule', args)
        self.assertEqual(one, self.scheduler.command(self.owner, 'same', 'schedule', args))
        with self.assertRaises(Denied):
            self.scheduler.command(self.owner, 'same', 'schedule', {'until': '22:00'})

    def test_modify_requires_current_id(self):
        old = self.schedule()
        with self.assertRaises(Denied):
            self.schedule(until='22:00')
        new = self.schedule(until='22:00', schedule_id=old['id'])
        self.assertNotEqual(old['id'], new['id'])
        self.assertIn('22:00', new['deadline'])

    def test_cancel_active_does_not_clear_manual_pause(self):
        result = self.schedule()
        with self.store.tx() as db:
            db.execute("UPDATE settings SET value='1' WHERE key='paused'")
        self.assertEqual('CANCELLED', self.command('cancel', schedule_id=result['id'])['state'])
        self.assertTrue(self.store.snapshot()['paused'])

    def test_cancel_closed_keeps_admission_hold(self):
        result = self.schedule()
        self.now += 12*3600
        self.command('cancel', schedule_id=result['id'])
        self.assertEqual('SCHEDULE_HOLD', self.decision(intake=True))

    def test_cleanup_blocks_intake_and_preserves_pending_task(self):
        tid = self.task()
        self.schedule()
        self.now += (10*60+50)*60
        self.scheduler.tick()
        with self.assertRaises(Denied):
            self.store.command(self.owner, 'late', 'task', sample('CodexAgent'))
        self.assertEqual('NEW', self.store.get(tid)['state'])
        with self.store.tx() as db:
            self.assertEqual(1, db.execute('SELECT count(*) FROM control_holds').fetchone()[0])

    def test_long_job_not_started_before_cleanup(self):
        self.schedule()
        self.now += (10*60+48)*60+31
        self.assertEqual('INSUFFICIENT_TIME', self.decision(duration=90))
        self.assertIsNone(self.decision(intake=True))

    def test_claim_stops_but_running_lease_untouched(self):
        tid = self.task()
        self.store.triage(tid)
        running = self.store.claim()
        before = self.store.get(tid)
        sid = self.schedule()['id']
        result = self.command('finish', schedule_id=sid)
        self.assertEqual('HOLD', result['state'])
        self.assertEqual(before, self.store.get(tid))
        self.assertEqual(tid, running['id'])
        self.assertIsNone(self.store.claim())

    def test_restart_restores_schedule_and_hold(self):
        tid = self.task()
        sid = self.schedule()['id']
        self.now += 12*3600
        self.scheduler.tick()
        recovered = Scheduler(Store(self.store.path, {1}, clock=lambda: self.now))
        self.assertEqual(sid, recovered.tick()['schedule']['id'])
        self.assertEqual('CLOSED', recovered.tick()['schedule']['state'])
        self.assertEqual('NEW', self.store.get(tid)['state'])

    def test_next_start_does_not_implicitly_approve_ai(self):
        self.schedule(next_start='2026-10-24 10:00')
        self.now += 23*3600
        self.assertEqual('SCHEDULE_HOLD', self.decision(ai=True))
        self.scheduler.tick(); self.scheduler.tick()
        with self.store.tx() as db:
            self.assertEqual(1, db.execute("SELECT count(*) FROM control_outbox WHERE id LIKE '%:NEXT_CHECK'").fetchone()[0])

    def test_sunday_start_rejected(self):
        with self.assertRaises(Denied):
            self.schedule(next_start='2026-10-25 10:00')
        self.now = datetime(2026,10,25,12,tzinfo=KST).timestamp()
        self.assertEqual('SUNDAY_HOLD', self.decision(ai=True))
        with self.assertRaises(Denied):
            self.schedule()

    def test_unknown_usage_blocks_before_cli(self):
        self.assertEqual('USAGE_HOLD', self.decision(ai=True))
        sources={'refs':{'main':'a'*40},'files':[{'path':'문서/담당/'+n+'.md','ref':'main','content':'fixture'} for n in ROLE_FILES.values()]}
        plan = make_plan('PlanningAgent', sources, clock=lambda: self.now)
        grant_once(self.store, self.owner, plan)
        invoke = Mock(side_effect=AssertionError('CLI must not start'))
        with self.assertRaises(Denied):
            run_once(self.store, self.owner, plan, self.root/'empty', 'fake', invoke=invoke)
        invoke.assert_not_called()

    def test_legacy_probe_also_blocks_before_cli_without_usage(self):
        from collab.phase2 import run_probe_once
        invoke = Mock(side_effect=AssertionError('No legacy CLI calls'))
        with self.assertRaises(Denied):
            run_probe_once(self.store,self.root/'legacy','fake',approved=True,invoke=invoke)
        invoke.assert_not_called()

    def test_fresh_two_windows_allow_gate_not_execution(self):
        self.usage()
        self.assertIsNone(self.decision(ai=True))
        self.assertEqual('AVAILABLE', self.command('usage')['status'])

    def test_five_hour_exhausted_blocks(self):
        self.usage(first=100)
        self.assertEqual('USAGE_HOLD', self.decision(ai=True))
        self.assertEqual('EXHAUSTED', self.command('usage')['status'])

    def test_weekly_exhausted_blocks(self):
        self.usage(second=100)
        self.assertEqual('USAGE_HOLD', self.decision(ai=True))
        self.assertEqual(self.now+700, self.command('usage')['recheck_at'])

    def test_both_windows_require_latest_reset(self):
        self.usage(first=100, second=100)
        self.assertEqual(self.now+700, self.command('usage')['recheck_at'])

    def test_reset_time_passed_requires_new_observation(self):
        self.usage(first=100)
        self.now += 701
        self.assertEqual('RECHECK_REQUIRED', self.command('usage')['status'])
        self.assertEqual('USAGE_HOLD', self.decision(ai=True))
        self.usage()
        self.assertIsNone(self.decision(ai=True))

    def test_stale_available_snapshot_blocks(self):
        self.usage()
        self.now += 301
        self.assertEqual('UNKNOWN', self.command('usage')['status'])
        self.assertEqual('USAGE_HOLD', self.decision(ai=True))

    def test_official_denial_blocks_even_with_unused_percent(self):
        value = self.usage()
        value['ordinary_allowed'] = False
        self.scheduler.record_official_usage(value, self.now)
        self.assertEqual('USAGE_HOLD', self.decision(ai=True))

    def test_stale_or_nan_official_values_rejected(self):
        for percent in (float('nan'), float('inf'), True, -1, 101):
            with self.assertRaises(Denied):
                self.usage(first=percent)

    def test_invalid_missing_or_unofficial_observations(self):
        for body in ({}, {'source':'guessed'}, {'source':'CODEX_APP_OFFICIAL_TOOL','windows':[]},
                     {'source':'CODEX_APP_OFFICIAL_TOOL','windows':[{},{}]}):
            with self.assertRaises(Denied):
                self.scheduler.record_official_usage(body, self.now)
        with self.assertRaises(Denied):
            self.scheduler.record_official_usage({}, self.now-61)

    def test_discord_cannot_set_usage_or_run_arbitrary_commands(self):
        for name in ('set-usage', 'exec', 'merge', 'buy-credits', 'reset'):
            with self.assertRaises(Denied):
                self.command(name)

    def test_all_commands_require_owner_guild_channel_and_human(self):
        for identity in (Identity(2,1,1), Identity(1,2,1), Identity(1,1,2), Identity(1,1,1,True)):
            for action in ('schedule','schedule-status','usage','cancel','finish','shutdown'):
                with self.assertRaises(Denied):
                    self.scheduler.command(identity, 'denied', action, {})

    def test_shutdown_requires_specific_confirmation(self):
        sid = self.schedule()['id']
        with self.assertRaises(Denied):
            self.command('shutdown', schedule_id=sid, confirm='yes')
        value = self.command('shutdown', schedule_id=sid, confirm=sid)
        self.assertEqual('HOLD', value['state'])
        self.assertEqual('SIMULATION_ONLY', value['mode'])

    def test_finish_stores_tasks_without_mutating_approvals_or_evidence(self):
        tid = self.task()
        before = self.store.get(tid)
        sid = self.schedule()['id']
        result = self.command('finish', schedule_id=sid)
        self.assertEqual('REVIEW_REQUIRED', result['state'])
        self.assertEqual(before, self.store.get(tid))
        with self.store.tx() as db:
            self.assertEqual(0, db.execute('SELECT count(*) FROM approvals').fetchone()[0])
            self.assertEqual(1, db.execute('SELECT count(*) FROM control_reports').fetchone()[0])

    def test_concurrent_duplicate_command_creates_once(self):
        with ThreadPoolExecutor(max_workers=2) as pool:
            results = list(pool.map(lambda _: self.scheduler.command(self.owner,'dup','schedule',{'until':'23:00'}),range(2)))
        self.assertEqual(results[0], results[1])
        with self.store.tx() as db:
            self.assertEqual(1, db.execute('SELECT count(*) FROM control_schedules').fetchone()[0])

    def test_safety_status_korean_and_no_raw_payload(self):
        self.usage()
        text = format_control('usage', self.command('usage'))
        self.assertIn('5시간', text)
        self.assertIn('주간', text)
        self.assertIn('KST', text)
        self.assertNotIn('windows', text)

    def test_finish_does_not_erase_usage_or_consumed_slots(self):
        with self.store.tx() as db:
            db.execute("INSERT INTO settings VALUES('consumed-fixture',?)", (json.dumps({'state':'CONSUMED'}),))
        sid = self.schedule()['id']
        self.command('finish', schedule_id=sid)
        with self.store.tx() as db:
            self.assertEqual('CONSUMED', json.loads(self.store.setting(db,'consumed-fixture'))['state'])

    def test_concurrency_two_limit_still_applies(self):
        for index in range(3):
            tid = self.store.command(self.owner,'task'+str(index),'task',sample('PlanningAgent'))['task_id']
            self.store.triage(tid)
        self.assertIsNotNone(self.store.claim())
        self.assertIsNotNone(self.store.claim())
        self.assertIsNone(self.store.claim())

    def test_finished_result_can_arrive_while_schedule_closed(self):
        tid = self.task()
        self.store.triage(tid)
        run = self.store.claim()
        sid = self.schedule()['id']
        self.command('finish', schedule_id=sid)
        self.store.finish(tid, run['lease'], 'TECH_PASS', {'mode':'MOCK','status':'PASS'})
        self.assertEqual('REVIEW_PENDING', self.store.get(tid)['state'])

    def test_folder_intake_hold_preserves_file_for_later(self):
        from collab.art_delivery import Deliveries
        from tests.test_art_delivery import sprite
        delivery = Deliveries(self.store,self.root/'quarantine','a'*40,{'names':[],'sha256':[]})
        sid = self.schedule()['id']
        self.command('finish',schedule_id=sid)
        with self.assertRaises(Denied):
            delivery.receive(self.owner,'late-file','late.png',sprite())
        self.assertFalse(delivery.seen('late-file'))

    def test_fixed_tick_on_isolated_db_has_no_external_calls(self):
        import shutil
        import collab.safety_tick as tick
        target = self.root/'tick-root'
        (target/'data/phase2').mkdir(parents=True)
        shutil.copy2(self.store.path,target/'data/phase2/relay.sqlite')
        with patch.object(tick,'ROOT',target):
            tick.main()
        result = json.loads((target/'data/safety/tick-status.json').read_text(encoding='utf8'))
        self.assertEqual('SIMULATION_ONLY',result['mode'])
        self.assertEqual(0,result['ai_calls'])
        self.assertEqual(0,result['os_shutdown_calls'])

    def test_no_production_shutdown_or_git_executor(self):
        import collab.scheduling as module
        self.assertFalse(hasattr(module, 'subprocess'))
        self.assertFalse(hasattr(module, 'os'))
        self.assertFalse(hasattr(module, 'ctypes'))


class ShutdownPreviewTests(unittest.TestCase):
    def setUp(self):
        self.now = 1000
        self.evidence = {key: True for key in ('owner_confirmed','local_identity_verified',
            'local_shutdown_privilege','scope_completed','integration_explicitly_approved',
            'sync_verified','protected_files_verified','unity_idle','copy_idle',
            'unsaved_work_clear','no_schedule_conflict','discord_report_sent','cancel_available')}
        self.evidence['verified_at'] = self.now

    def test_all_conditions_only_would_shutdown(self):
        result = shutdown_preview(self.evidence, self.now)
        self.assertEqual('WOULD_SHUTDOWN', result['state'])
        self.assertEqual(0, result['os_calls'])
        self.assertGreaterEqual(result['cancel_grace_seconds'], 120)

    def test_each_missing_or_false_check_blocks(self):
        for key in self.evidence:
            if key == 'verified_at': continue
            for bad in (False, None, 'true', 1):
                with self.subTest(key=key,bad=bad):
                    self.assertEqual('HOLD', shutdown_preview({**self.evidence,key:bad}, self.now)['state'])

    def test_sync_conflict_blocks(self):
        self.evidence['sync_verified'] = False
        self.assertIn('sync_verified', shutdown_preview(self.evidence,self.now)['blockers'])

    def test_unity_copy_and_unsaved_work_block(self):
        for key in ('unity_idle','copy_idle','unsaved_work_clear'):
            result = shutdown_preview({**self.evidence,key:False},self.now)
            self.assertIn(key, result['blockers'])

    def test_user_cancellation_blocks(self):
        self.assertIn('cancelled',shutdown_preview({**self.evidence,'cancelled':True},self.now)['blockers'])

    def test_old_or_future_evidence_blocks(self):
        for stamp in (900,1001,float('nan'),True,None):
            self.assertIn('fresh_evidence',shutdown_preview({**self.evidence,'verified_at':stamp},self.now)['blockers'])


class DiscordSafetyAdapterTests(unittest.IsolatedAsyncioTestCase):
    async def asyncSetUp(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        self.temp = tempfile.TemporaryDirectory(dir='data/tmp')
        self.store=Store(Path(self.temp.name)/'db.sqlite',{1},clock=lambda:datetime(2026,10,23,12,tzinfo=KST).timestamp())
        from collab.bot import create_bot
        self.bot=create_bot(self.store,Path(self.temp.name)/'adapter')

    async def asyncTearDown(self):
        await self.bot.close()
        self.temp.cleanup()

    def interaction(self, owner=1):
        return SimpleNamespace(id=123,user=SimpleNamespace(id=owner,bot=False),guild_id=1,channel_id=1,
                               response=SimpleNamespace(defer=AsyncMock()),followup=SimpleNamespace(send=AsyncMock()))

    async def test_six_new_and_nine_old_commands_preserved(self):
        names={c.name for c in self.bot.tree.get_command('limitless').commands}
        self.assertEqual(15,len(names))
        self.assertTrue({'schedule','schedule-status','cancel','usage','finish','shutdown'} <= names)
        self.assertTrue({'status','pending','task','handoff','approve','reject','pause','resume','budget'} <= names)

    async def test_schedule_reply_is_ephemeral_korean(self):
        item=self.interaction()
        command=self.bot.tree.get_command('limitless').get_command('schedule')
        await command.callback(item,'23:00')
        args=item.followup.send.call_args
        self.assertTrue(args.kwargs['ephemeral'])
        self.assertIn('예약',args.args[0])
        self.assertIn('종료 승인이 아닙니다',args.args[0])

    async def test_deadline_outbox_posts_once_and_restores_receipt(self):
        scheduler=Scheduler(self.store)
        owner=Identity(1,1,1)
        scheduler.command(owner,'schedule','schedule',{'until':'23:00'})
        self.store.clock=lambda:datetime(2026,10,23,22,50,tzinfo=KST).timestamp()
        scheduler.tick(); scheduler.tick()
        publish=AsyncMock(return_value='mock-message-id')
        await scheduler.flush_reports(publish)
        await scheduler.flush_reports(publish)
        self.assertEqual(1,publish.call_count)
        with self.store.tx() as db:
            row=db.execute('SELECT state,message_id FROM control_outbox').fetchone()
            self.assertEqual(('SENT','mock-message-id'),tuple(row))

    async def test_report_ambiguous_no_automatic_retry(self):
        scheduler=Scheduler(self.store)
        scheduler.command(Identity(1,1,1),'schedule','schedule',{'until':'23:00'})
        self.store.clock=lambda:datetime(2026,10,23,23,1,tzinfo=KST).timestamp()
        scheduler.tick()
        publish=AsyncMock(side_effect=RuntimeError('synthetic'))
        await scheduler.flush_reports(publish)
        await scheduler.flush_reports(publish)
        self.assertEqual(1,publish.call_count)
        with self.store.tx() as db:
            self.assertEqual('UNCERTAIN',db.execute('SELECT state FROM control_outbox').fetchone()[0])

    async def test_inflight_report_recovery_never_reposts(self):
        scheduler=Scheduler(self.store)
        with self.store.tx() as db:
            db.execute("INSERT INTO control_outbox VALUES('test','no secret','SENDING',NULL)")
        scheduler.recover_reports()
        publish=AsyncMock()
        await scheduler.flush_reports(publish)
        publish.assert_not_called()

    async def test_unauthorized_shutdown_rejected_in_adapter(self):
        item=self.interaction(owner=2)
        command=self.bot.tree.get_command('limitless').get_command('shutdown')
        await command.callback(item,'SCH-no','SCH-no')
        self.assertIn('바울 대표 ID',item.followup.send.call_args.args[0])
        with self.store.tx() as db:
            self.assertEqual(0, db.execute('SELECT count(*) FROM control_reports').fetchone()[0])
