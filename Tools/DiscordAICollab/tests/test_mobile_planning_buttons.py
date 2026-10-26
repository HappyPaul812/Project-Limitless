"""새 버튼/서비스 인계 경계만 합성 검증합니다. 네트워크·CLI 실행 금지."""
import copy
import json
from pathlib import Path
import socket
import subprocess
import tempfile
import unittest
from unittest.mock import patch
import discord  # SDK의 OS 버전 초기화는 실행/네트워크 금지 계측 전에 마칩니다.
from types import SimpleNamespace

from collab.core import Identity, Denied
from collab.mobile_offline import OfflineMobileRelay
from collab.mobile_planning_buttons import PlanningButtons, contract, make_view, build_group
from collab.session0_planning_bridge import (OfflineSession0Harness, LiveSession0Bridge,
    TrustedProjection, deployment_plan, verify_frozen_runner, send_result_once, OfflineButtonWorkerConnection)


def drive(coroutine):
    try: coroutine.send(None)
    except StopIteration as stop: return stop.value
    raise AssertionError('UNEXPECTED_EXTERNAL_AWAIT')


class ButtonWorkerTests(unittest.TestCase):
    def setUp(self):
        root = Path('F:/study/codex/Project-Limitless/Temp/ButtonWorkerTests')
        root.mkdir(exist_ok=True)
        self.temp = tempfile.TemporaryDirectory(dir=root)
        self.now = 1792992000.0
        self.clock = lambda: self.now
        self.relay = OfflineMobileRelay.create(Path(self.temp.name) / 'trial', {contract.OWNER},
            clock=self.clock, guild=contract.GUILD, channels=(contract.CHANNEL,))
        self.flow = PlanningButtons(self.relay, offline_fixture=True)
        self.owner = Identity(contract.OWNER, contract.GUILD, contract.CHANNEL)
        self.task = self.flow.create_task(self.owner, '1')
        self.panel = self.flow.panel(self.owner, self.task)
        self.flow.bind_message(self.panel, 123)
        self.seq = 10
        self.context = {'account': 'PAUL-PC\\LIMITLESSRelaySvc', 'sid_matches': True,
                        'session_id': 0, 'logon_type': 4, 'is_admin': False}
        self.quota = {'source': 'CODEX_APP_SERVER_OFFICIAL', 'observed': self.now, 'ordinary_allowed': True,
            'windows': [{'minutes': m, 'used_percent': 20, 'resets_at': self.now + 1000} for m in (300, 10080)]}
        self.events = [{'type': 'thread.started'}, {'type': 'turn.started'},
            {'type': 'item.completed', 'item': {'type': 'agent_message', 'text': contract.EXPECTED}},
            {'type': 'turn.completed', 'usage': {'input_tokens': 10, 'output_tokens': 2, 'cached_input_tokens': 5}}]
        self.blocks = [patch.object(subprocess, 'Popen', side_effect=AssertionError('PROCESS_FORBIDDEN')),
                       patch.object(socket, 'socket', side_effect=AssertionError('NETWORK_FORBIDDEN'))]
        for block in self.blocks: block.start()

    def tearDown(self):
        for block in reversed(self.blocks): block.stop()
        self.temp.cleanup()

    def click(self, action, **kwargs):
        self.seq += 1
        return self.flow.click(kwargs.get('identity', self.owner), kwargs.get('app', contract.TRIAL_APP),
            self.panel, kwargs.get('message', 123), kwargs.get('event', str(self.seq)), action)

    def ticket(self):
        self.click('approve')
        req = self.click('run')['request_id']
        return self.flow.claim_handoff(req)

    def harness(self, **kwargs):
        return OfflineSession0Harness(self.clock, kwargs.get('context', self.context), kwargs.get('quota', self.quota),
            '\n'.join(json.dumps(e, ensure_ascii=False) for e in kwargs.get('events', self.events)).encode(),
            kwargs.get('final', contract.EXPECTED), exit_code=kwargs.get('exit_code', 0))

    def arm(self, ticket):
        return {k: ticket[k] for k in ('request_id', 'task_sha256', 'ticket_sha256', 'policy')}

    def test_sdk_five_buttons_no_sha_parameter(self):
        view = make_view(self.flow, self.panel)
        self.assertEqual(['업무 내용 보기', '실행 승인', '실행', '취소', '진행 상황 확인'], [x.label for x in view.children])
        self.assertTrue(all(len(x.custom_id) <= 100 for x in view.children))
        group = build_group(self.flow)
        self.assertEqual('limitless-trial', group.name)
        self.assertEqual([], group.commands[0].parameters)

    def test_approval_not_execution(self):
        self.click('approve')
        self.assertEqual('TRIAGED', self.relay.store.get(self.task)['state'])
        with self.relay.store.tx() as db: self.assertEqual(0, db.execute('SELECT COUNT(*) FROM planning_jobs').fetchone()[0])

    def test_view_and_status(self):
        self.assertIn('PlanningAgent', self.click('view')['text'])
        self.assertEqual('NEW', self.click('status')['status'])

    def test_nonowner_denied(self):
        with self.assertRaises(Denied): self.click('approve', identity=Identity(2, contract.GUILD, contract.CHANNEL))

    def test_wrong_app_denied(self):
        with self.assertRaises(Denied): self.click('approve', app=contract.PRODUCTION_APP)

    def test_wrong_channel_denied(self):
        with self.assertRaises(Denied): self.click('approve', identity=Identity(contract.OWNER, contract.GUILD, 2))

    def test_wrong_message_denied(self):
        with self.assertRaises(Denied): self.click('approve', message=456)

    def test_absolute_expiry(self):
        self.click('status'); self.now += 300
        with self.assertRaises(Denied): self.click('approve')

    def test_run_requires_approval(self):
        with self.assertRaises(Denied): self.click('run')

    def test_duplicate_receipt_and_request(self):
        self.click('approve')
        first = self.click('run', event='111')
        self.assertEqual(first, self.click('run', event='111'))
        self.assertEqual('ALREADY_REQUESTED_NO_RETRY', self.click('run')['status'])
        self.assertIsNotNone(self.flow.claim_handoff(first['request_id']))
        self.assertIsNone(self.flow.claim_handoff(first['request_id']))

    def test_changed_interaction_denied(self):
        self.click('view', event='111')
        with self.assertRaises(Denied): self.click('approve', event='111')

    def test_sha_role_and_upstream_change_denied(self):
        with self.relay.store.tx() as db:
            row = self.relay.store.task(db, self.task)
            payload = json.loads(row['payload']); payload['upstream'] = {'bad': 'new'}
            db.execute('UPDATE tasks SET payload=?,role=? WHERE id=?', (json.dumps(payload), 'CodexAgent', self.task))
        with self.assertRaises(Denied): self.click('approve')

    def test_cancel_before_run(self):
        self.click('approve'); self.click('cancel')
        with self.assertRaises(Denied): self.click('run')

    def test_cancel_after_consumption_no_restore(self):
        ticket = self.ticket(); self.click('cancel')
        result = self.harness().submit(ticket, self.arm(ticket))
        self.assertFalse(self.flow.accept_result(ticket['request_id'], result))
        with self.relay.store.tx() as db: self.assertEqual('CONSUMED', db.execute('SELECT state FROM mobile_grants').fetchone()[0])

    def test_full_fixture_result_and_report(self):
        ticket = self.ticket(); worker = self.harness()
        result = worker.submit(ticket, self.arm(ticket))
        self.assertEqual('VERIFIED', result.value['status'])
        self.assertTrue(self.flow.accept_result(ticket['request_id'], result))
        self.assertEqual(contract.EXPECTED, self.flow.claim_report(ticket['request_id']))
        self.flow.report_result(ticket['request_id'], '123')
        self.assertIsNone(self.flow.claim_report(ticket['request_id']))
        self.assertEqual((1, 1), (worker.fake_usage_calls, worker.fake_exec_calls))
        self.assertEqual(1, len(self.relay.store.snapshot()['tasks']))

    def test_quota_five_hour_hold(self):
        ticket = self.ticket(); self.quota['windows'][0]['used_percent'] = 100
        worker = self.harness()
        with self.assertRaises(Denied): worker.submit(ticket, self.arm(ticket))
        self.assertEqual(0, worker.fake_exec_calls)
        with self.assertRaises(Denied): worker.submit(ticket, self.arm(ticket))

    def test_quota_weekly_hold(self):
        ticket = self.ticket(); self.quota['windows'][1]['used_percent'] = 100
        with self.assertRaises(Denied): self.harness().submit(ticket, self.arm(ticket))

    def test_stale_quota_hold(self):
        ticket = self.ticket(); self.quota['observed'] -= 31
        with self.assertRaises(Denied): self.harness().submit(ticket, self.arm(ticket))

    def test_service_context_variations_hold(self):
        ticket = self.ticket()
        for key, value in [('account', 'PAUL-PC\\windows'), ('sid_matches', False), ('session_id', 9), ('logon_type', 2), ('is_admin', True)]:
            with self.subTest(key=key), self.assertRaises(Denied):
                self.harness(context=dict(self.context, **{key: value})).submit(ticket, self.arm(ticket))

    def test_whole_deadline_hold(self):
        ticket = self.ticket(); self.now += 90
        with self.assertRaises(ValueError): self.harness().submit(ticket, self.arm(ticket))

    def test_ticket_and_arm_sha_tamper(self):
        ticket = self.ticket(); arm = self.arm(ticket); arm['task_sha256'] = '0' * 64
        with self.assertRaises(ValueError): self.harness().submit(ticket, arm)

    def test_jsonl_forbidden_tool_hold(self):
        ticket = self.ticket(); self.events.insert(2, {'type': 'item.completed', 'item': {'type': 'command_execution'}})
        self.assertEqual('HOLD', self.harness().submit(ticket, self.arm(ticket)).value['status'])

    def test_wrong_final_hold(self):
        ticket = self.ticket()
        self.assertEqual('HOLD', self.harness(final='wrong').submit(ticket, self.arm(ticket)).value['status'])

    def test_nonzero_exit_hold(self):
        ticket = self.ticket()
        self.assertEqual('HOLD', self.harness(exit_code=1).submit(ticket, self.arm(ticket)).value['status'])

    def test_fixture_cannot_enter_live(self):
        ticket = self.ticket(); projection = self.harness().submit(ticket, self.arm(ticket))
        self.flow.offline_fixture = False
        with self.assertRaises(Denied): self.flow.accept_result(ticket['request_id'], projection)
        with self.assertRaises(Denied): TrustedProjection(projection.value, fixture=False)

    def test_plain_dictionary_not_trusted(self):
        ticket = self.ticket()
        with self.assertRaises(Denied): self.flow.accept_result(ticket['request_id'], {'status': 'VERIFIED'})

    def test_discord_failure_never_reruns(self):
        ticket = self.ticket(); worker = self.harness()
        self.flow.accept_result(ticket['request_id'], worker.submit(ticket, self.arm(ticket)))
        async def failed(text): raise OSError('synthetic')
        self.assertFalse(drive(send_result_once(self.flow, ticket['request_id'], failed)))
        self.assertFalse(drive(send_result_once(self.flow, ticket['request_id'], failed)))
        self.assertEqual(1, worker.fake_exec_calls)

    def test_restart_holds_and_preserves_consumption(self):
        ticket = self.ticket(); self.flow.recover()
        with self.relay.store.tx() as db:
            self.assertEqual('HOLD', db.execute('SELECT state FROM planning_jobs').fetchone()[0])
            self.assertEqual('CONSUMED', db.execute('SELECT state FROM mobile_grants').fetchone()[0])

    def test_live_gate_disabled_and_frozen_runner_unchanged(self):
        self.assertTrue(verify_frozen_runner())
        self.assertFalse(deployment_plan()['approved'])
        with self.assertRaises(Denied): LiveSession0Bridge().submit({})

    def test_sdk_callback_handoff_once(self):
        calls = []
        class Response:
            done = False
            async def defer(self, **kwargs): self.done = True
            def is_done(self): return self.done
            async def send_message(self, *args, **kwargs): self.done = True
        class Followup:
            async def send(self, *args, **kwargs): return SimpleNamespace(id=555)
        interaction = SimpleNamespace(user=SimpleNamespace(id=contract.OWNER, bot=False), guild_id=contract.GUILD,
            channel_id=contract.CHANNEL, application_id=contract.TRIAL_APP,
            permissions=SimpleNamespace(use_application_commands=True),
            app_permissions=SimpleNamespace(view_channel=True, send_messages=True),
            response=Response(), followup=Followup(), message=SimpleNamespace(id=123), id=500)
        async def submit(request, received): calls.append(request)
        view = make_view(self.flow, self.panel, on_run=submit)
        drive(view.children[1].callback(interaction))
        interaction.id = 501
        drive(view.children[2].callback(interaction))
        drive(view.children[2].callback(interaction))
        self.assertEqual(1, len(calls))

    def test_sdk_permission_denied_before_approval(self):
        calls = []
        class Response:
            def is_done(self): return False
            async def send_message(self, *args, **kwargs): calls.append('HOLD')
        interaction = SimpleNamespace(user=SimpleNamespace(id=contract.OWNER, bot=False), guild_id=contract.GUILD,
            channel_id=contract.CHANNEL, application_id=contract.TRIAL_APP,
            permissions=SimpleNamespace(use_application_commands=False),
            app_permissions=SimpleNamespace(view_channel=True, send_messages=True), response=Response())
        drive(make_view(self.flow, self.panel).children[1].callback(interaction))
        self.assertEqual(['HOLD'], calls)
        self.assertEqual('NEW', self.relay.store.get(self.task)['state'])

    def test_cancel_completed_preserves_result(self):
        ticket = self.ticket()
        self.flow.accept_result(ticket['request_id'], self.harness().submit(ticket, self.arm(ticket)))
        self.assertEqual('ALREADY_COMPLETE', self.click('cancel')['status'])
        self.assertEqual(contract.EXPECTED, self.click('status')['final_response'])

    def test_approval_click_does_not_extend_expiry(self):
        self.click('approve')
        with self.relay.store.tx() as db: before = db.execute('SELECT expires FROM mobile_grants').fetchone()[0]
        self.now += 10; self.click('approve')
        with self.relay.store.tx() as db: self.assertEqual(before, db.execute('SELECT expires FROM mobile_grants').fetchone()[0])

    def test_strict_json_duplicate_keys(self):
        with self.assertRaises(ValueError): contract.strict('{"x":1,"x":2}')
        with self.assertRaises(ValueError): contract.strict('{"x":NaN}')

    def test_button_worker_discord_connection(self):
        ticket = self.ticket(); messages = []
        class Followup:
            async def send(self, text, **kwargs):
                messages.append(text)
                return SimpleNamespace(id=888)
        connection = OfflineButtonWorkerConnection(self.flow, self.harness())
        drive(connection.on_run(ticket['request_id'], SimpleNamespace(followup=Followup())))
        self.assertEqual([contract.EXPECTED], messages)
        with self.relay.store.tx() as db:
            self.assertEqual('SENT', db.execute('SELECT report FROM planning_jobs').fetchone()[0])


if __name__ == '__main__': unittest.main()
