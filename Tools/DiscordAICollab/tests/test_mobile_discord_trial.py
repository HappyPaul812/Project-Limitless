"""Phase3의 새 SDK/권한/승인/보고 경계만 검증. Gateway/HTTP/모델 실행 없음."""
import json
import tempfile
import unittest
from pathlib import Path
from types import SimpleNamespace as NS

import discord
from discord import app_commands
from collab.core import Denied, Identity
from collab.mobile_offline import OfflineMobileRelay, digest
from collab.mobile_discord_trial import (MobileTrialController, TrialQuota, build_trial_group,
                                        registration_plan, COMMANDS, GROUP)


def complete(coroutine):
    """가짜 await만 있는 callback을 직접 완료해 asyncio의 OS 소켓 생성도 피합니다."""
    try:
        coroutine.send(None)
    except StopIteration as done:
        return done.value
    finally:
        coroutine.close()
    raise AssertionError('Unexpected external await')


class Interaction:
    def __init__(self, event, *, user=1, guild=1, channel=1, bot=False, allow=True, fail=False):
        self.id, self.guild_id, self.channel_id = event, guild, channel
        self.application_id = 20
        self.user = NS(id=user, bot=bot)
        self.permissions = NS(use_application_commands=allow)
        self.app_permissions = NS(view_channel=True, send_messages=True)
        self.sent, self.deferred, self.fail = [], None, fail
        self.response = NS(send_message=self.send, defer=self.defer)
        self.followup = NS(send=self.send)

    async def defer(self, **kwargs):
        self.deferred = kwargs

    async def send(self, text, **kwargs):
        if self.fail:
            raise RuntimeError('SYNTHETIC_TOKEN_NOT_FOR_LOGS')
        self.sent.append((text, kwargs))
        return NS(id=self.id + 100000)


class DiscordTrialTests(unittest.TestCase):
    def setUp(self):
        scratch = Path('F:/study/codex/Project-Limitless/Temp/MobileDiscordTrial')
        scratch.mkdir(exist_ok=True)
        self.temp = tempfile.TemporaryDirectory(dir=scratch)
        self.now, self.seq = 1792995000.0, 1000
        self.clock = lambda: self.now
        self.relay = OfflineMobileRelay.create(Path(self.temp.name) / 'trial', {1}, clock=self.clock)
        self.controller = MobileTrialController(self.relay, base_sha='a' * 40, trial_application_id=20, production_application_id=10)
        self.group = build_trial_group(self.controller)
        self.fresh()

    def tearDown(self):
        self.temp.cleanup()

    def fresh(self, *, short=0, weekly=0, reset=200):
        self.controller.quota.observe(self.now, [{'minutes': 300, 'used_percent': short, 'resets_at': self.now + reset},
                                               {'minutes': 10080, 'used_percent': weekly, 'resets_at': self.now + reset + 500}])

    def call(self, action, *, interaction=None, **kwargs):
        self.seq += 1
        interaction = interaction or Interaction(self.seq)
        complete(self.group.get_command(action).callback(interaction, **kwargs))
        return interaction

    def task(self):
        self.call('task', title='모바일 합성 연결 시험')
        return self.relay.store.snapshot()['tasks'][0]['id']

    def approve(self, task):
        return self.call('approve-execution', task_id=task, sha256=digest(self.relay.plan(task)))

    def run_fake(self, task):
        return self.call('run-fake', task_id=task)

    def test_sdk_command_payload_is_separate_guild_only_default_denied(self):
        tree = NS(allowed_contexts=app_commands.AppCommandContext(), allowed_installs=app_commands.AppInstallationType())
        payload = self.group.to_dict(tree)
        self.assertEqual(GROUP, payload['name'])
        self.assertFalse(payload['dm_permission'])
        self.assertEqual(0, payload['default_member_permissions'])
        self.assertEqual(set(COMMANDS), {c['name'] for c in payload['options']})
        self.assertNotIn('approve', {c['name'] for c in payload['options']})

    def test_sdk_mobile_task_plan_approve_fake_report_and_handoff(self):
        root = self.task()
        plan = self.call('plan', task_id=root)
        self.assertIn(digest(self.relay.plan(root)), plan.sent[0][0])
        self.approve(root)
        self.assertEqual([], self.relay.executor.calls)
        answer = self.run_fake(root)
        self.assertIn('가짜 실행 PASS', answer.sent[0][0])
        self.assertTrue(answer.deferred['ephemeral'])
        self.assertTrue(answer.sent[0][1]['ephemeral'])
        self.assertFalse(answer.sent[0][1]['allowed_mentions'].everyone)
        self.call('handoff', task_id=root)
        arts = [t['id'] for t in self.relay.store.snapshot()['tasks'] if t['id'] != root]
        for task in arts:
            self.approve(task)
            self.run_fake(task)
        self.call('handoff', task_id=root)
        codex = next(t['id'] for t in self.relay.store.snapshot()['tasks'] if t['role'] == 'CodexAgent')
        self.approve(codex)
        self.run_fake(codex)
        final = self.call('handoff', task_id=root)
        self.assertIn('OFFLINE_COMPLETE_OWNER_REVIEW', final.sent[0][0])
        self.assertEqual(4, len(self.relay.executor.calls))

    def test_sdk_authorization_denies_user_guild_channel_bot_permission(self):
        for kwargs in ({'user': 2}, {'guild': 2}, {'channel': 2}, {'guild': 0}, {'bot': True}, {'allow': False}):
            with self.subTest(kwargs=kwargs):
                i = self.call('task', interaction=Interaction(2000, **kwargs), title='시험')
                self.assertIn('HOLD', i.sent[0][0])
                self.assertIsNone(i.deferred)
        self.assertEqual([], self.relay.store.snapshot()['tasks'])

    def test_bot_permissions_are_rechecked_each_callback(self):
        for field in ('view_channel', 'send_messages'):
            i = Interaction(2100)
            setattr(i.app_permissions, field, False)
            self.call('task', interaction=i, title='시험')
            self.assertIn('HOLD', i.sent[0][0])
        self.assertEqual([], self.relay.store.snapshot()['tasks'])

    def test_duplicate_interaction_has_one_worker_one_report(self):
        task = self.task()
        self.approve(task)
        i = Interaction(3000)
        self.call('run-fake', interaction=i, task_id=task)
        self.call('run-fake', interaction=i, task_id=task)
        self.assertEqual(1, len(i.sent))
        self.assertEqual([task], self.relay.executor.calls)

    def test_changed_interaction_does_not_execute_again(self):
        task = self.task()
        self.approve(task)
        self.call('status', interaction=Interaction(3100))
        self.call('run-fake', interaction=Interaction(3100), task_id=task)
        self.assertEqual([], self.relay.executor.calls)

    def test_cancel_preserves_grant_and_blocks_worker(self):
        task = self.task()
        self.approve(task)
        self.call('cancel-execution', task_id=task)
        self.assertIn('HOLD', self.run_fake(task).sent[0][0])
        self.assertEqual([], self.relay.executor.calls)
        with self.relay.store.tx() as db:
            self.assertEqual('CANCELLED', db.execute('SELECT state FROM mobile_grants WHERE task=?', (task,)).fetchone()[0])

    def test_expired_approval_is_not_renewed(self):
        task = self.task()
        self.approve(task)
        self.now += 121
        self.fresh()
        self.assertIn('HOLD', self.run_fake(task).sent[0][0])
        self.assertEqual([], self.relay.executor.calls)

    def test_quota_exhaustion_reset_and_fresh_observation_preserve_unconsumed(self):
        task = self.task()
        self.approve(task)
        self.fresh(short=100, reset=10)
        self.assertIn('EXHAUSTED', self.run_fake(task).sent[0][0])
        self.now += 11
        self.assertIn('RECHECK_REQUIRED', self.run_fake(task).sent[0][0])
        self.assertEqual([], self.relay.executor.calls)
        with self.relay.store.tx() as db:
            self.assertEqual('APPROVED', db.execute('SELECT state FROM mobile_grants WHERE task=?', (task,)).fetchone()[0])
        self.fresh()
        self.assertIn('가짜 실행 PASS', self.run_fake(task).sent[0][0])

    def test_weekly_and_both_windows_choose_latest_reset(self):
        self.fresh(short=100, weekly=100, reset=10)
        self.assertEqual(('EXHAUSTED', self.now + 510), self.controller.quota.decision(self.now))
        self.now += 11
        self.assertEqual('EXHAUSTED', self.controller.quota.decision(self.now)[0])
        self.now += 500
        self.assertEqual('RECHECK_REQUIRED', self.controller.quota.decision(self.now)[0])

    def test_unknown_stale_future_quota_cannot_run(self):
        task = self.task()
        self.approve(task)
        self.controller.quota = TrialQuota()
        self.assertIn('UNKNOWN', self.run_fake(task).sent[0][0])
        self.fresh()
        self.now -= 1
        self.assertIn('UNKNOWN', self.run_fake(task).sent[0][0])
        self.now += 302
        self.assertIn('UNKNOWN', self.run_fake(task).sent[0][0])
        self.assertEqual([], self.relay.executor.calls)

    def test_actual_report_failure_blocks_handoff_even_fake_report_sent(self):
        task = self.task()
        self.approve(task)
        failed = Interaction(4000, fail=True)
        self.call('run-fake', interaction=failed, task_id=task)
        self.call('run-fake', interaction=failed, task_id=task)
        self.assertIn('HOLD', self.call('handoff', task_id=task).sent[0][0])
        self.assertEqual(1, len(self.relay.store.snapshot()['tasks']))
        self.assertEqual([task], self.relay.executor.calls)
        status = self.call('status').sent[0][0]
        self.assertIn('확인 대기: 1건', status)
        self.assertNotIn('SYNTHETIC_TOKEN_NOT_FOR_LOGS', status)

    def test_root_cancel_also_cancels_children_without_erasing_consumption(self):
        root = self.task()
        self.approve(root)
        self.run_fake(root)
        self.call('handoff', task_id=root)
        self.call('cancel-execution', task_id=root)
        self.assertTrue(all(t['state'] == 'CANCELLED' for t in self.relay.store.snapshot()['tasks']))
        with self.relay.store.tx() as db:
            self.assertEqual('CONSUMED', db.execute('SELECT state FROM mobile_grants WHERE task=?', (root,)).fetchone()[0])

    def test_wrong_sha_scope_and_art_approval_not_exposed(self):
        task = self.task()
        answer = self.call('approve-execution', task_id=task, sha256='f' * 64)
        self.assertIn('HOLD', answer.sent[0][0])
        self.assertIsNone(self.group.get_command('approve'))
        self.assertEqual([], self.relay.executor.calls)

    def test_restart_keeps_unknown_and_never_auto_runs_or_resends(self):
        task = self.task()
        self.approve(task)
        event = '5000'
        perms = dict(owner_use_commands=True, bot_view_channel=True, bot_send_messages=True)
        self.controller.command(Identity(1, 1, 1), event, 'run-fake', {'task_id': task}, perms)
        self.controller.claim_delivery(event)
        self.controller.recover()
        self.assertIsNone(self.controller.claim_delivery(event))
        self.assertEqual([task], self.relay.executor.calls)

    def test_registration_plan_has_no_bulk_overwrite_or_production_restart(self):
        plan = registration_plan(production_application_id=1, trial_application_id=2, guild_id=3, existing_commands=[{'name': 'other'}])
        self.assertFalse(plan['bulk_overwrite'])
        self.assertFalse(plan['production_restart'])
        self.assertEqual(0, plan['actual_requests'])
        for app, commands in ((1, []), (2, [{'name': GROUP}])):
            with self.assertRaises(Denied):
                registration_plan(production_application_id=1, trial_application_id=app, guild_id=3, existing_commands=commands)

    def test_mobile_text_limits_and_no_user_input_echo(self):
        self.call('task', title='@everyone SYNTHETIC_PRIVATE_INPUT')
        answer = self.call('status').sent[0][0]
        self.assertNotIn('SYNTHETIC_PRIVATE_INPUT', answer)
        self.assertLessEqual(len(answer), 1800)
        with self.relay.store.tx() as db:
            self.assertEqual('MANUAL', db.execute('SELECT mode FROM operation_mode').fetchone()[0])
            self.assertEqual(0, db.execute('SELECT COUNT(*) FROM approvals').fetchone()[0])
            self.assertEqual(0, db.execute('SELECT COUNT(*) FROM usage').fetchone()[0])

    def test_wrong_application_and_production_app_binding_denied(self):
        i = Interaction(6000)
        i.application_id = 10
        self.call('task', interaction=i, title='시험')
        self.assertIn('HOLD', i.sent[0][0])
        with self.assertRaises(Denied):
            MobileTrialController(self.relay, base_sha='a' * 40, trial_application_id=10, production_application_id=10)
        with self.assertRaises(Denied):
            MobileTrialController(self.relay, base_sha='a' * 40, trial_application_id=21, production_application_id=10)


if __name__ == '__main__':
    unittest.main()
