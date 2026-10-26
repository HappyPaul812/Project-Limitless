"""새 서비스 경계만 합성 검증합니다. 모델/Discord/사용량/운영 DB 접근 없음."""
import copy
import hashlib
import json
from pathlib import Path
import socket
import subprocess
import tempfile
import unittest
from unittest.mock import patch

from collab.core import Denied
from collab.mobile_planning_buttons import contract
from collab.secure_session0_broker import Broker, Peer, dispatch
from collab.session0_planning_bridge import LiveSession0Bridge, TrustedProjection
from collab.session0_pipe_transport import PipeReply, WindowsPipeTransport
from collab import session0_pipe_transport as transport_module


class Worker:
    def __init__(self, test):
        self.test, self.calls, self.closed, self.active = test, 0, 0, 0
        self.missing = self.dirty = self.fail = self.slow = False
        self.callback = None
    def start(self, ticket, deadline):
        self.calls += 1
        if self.fail: raise OSError('synthetic')
        self.active = 2
        return self
    def poll(self, seconds):
        self.test.elapsed += seconds
        if self.callback:
            callback, self.callback = self.callback, None
            callback()
        if self.missing: return None
        if self.slow: self.test.elapsed += 100
        return copy.deepcopy(self.test.child)
    def close(self, deadline):
        self.closed += 1
        if not self.dirty: self.active = 0
        return self.active == 0


class SecureBrokerTests(unittest.TestCase):
    def setUp(self):
        scratch = Path('F:/study/codex/Project-Limitless/Temp/SecureBrokerTests'); scratch.mkdir(exist_ok=True)
        self.temp = tempfile.TemporaryDirectory(dir=scratch)
        self.path = Path(self.temp.name) / 'service-only.sqlite'
        self.elapsed, self.base = 0.0, 1793000000.0
        self.bot_sid, self.service_sid = 'S-1-5-21-1-1001', 'S-1-5-21-1-1002'
        self.admin_sid = 'S-1-5-21-1-1003'
        self.bot = Peer(self.bot_sid, False, False, True, 8192)
        self.admin = Peer(self.admin_sid, True, True, True, 12288)
        self.context = {'sid': self.service_sid, 'session': 0, 'logon': 4, 'admin': False,
            'protected_install': True, 'protected_state': True, 'fixed_task': True,
            'manual': True, 'approved_package': True}
        self.broker = self.new_broker()
        plan = {'task_id': 'L-offline', 'role': 'PlanningAgent', 'scope': contract.POLICY['scope'],
                'expected': contract.EXPECTED, 'policy': contract.POLICY}
        self.ticket = {'request_id': 'a' * 32, 'task_id': plan['task_id'], 'task_sha256': contract.digest(plan),
            'plan': plan, 'owner': contract.OWNER, 'application': contract.TRIAL_APP, 'guild': contract.GUILD,
            'channel': contract.CHANNEL, 'approved_until': self.base + 300, 'created': self.base,
            'deadline': self.base + 90, 'policy': contract.POLICY}
        self.rehash()
        self.permit = {'request_id': self.ticket['request_id'], 'task_sha256': self.ticket['task_sha256'],
                       'expires': self.base + 300, 'policy': contract.POLICY}
        self.child = {'request_id': self.ticket['request_id'], 'account': 'PAUL-PC\\LIMITLESSRelaySvc',
            'sid_matches': True, 'session_id': 0, 'logon_type': 4, 'status': 'VERIFIED', 'codex_home_unchanged': True,
            'job': {'exit_code': 0, 'timed_out': False, 'cleanup_verified': True, 'active_processes': 0},
            'entry_result': {'request_id': self.ticket['request_id'], 'status': 'VERIFIED',
                'inference_exec_launches': 1, 'usage_app_server_launches': 1, 'application_retries': 0,
                'credential_store_selection': 'EXISTING_KEYRING_EXPLICIT', 'mode_changed': False,
                'official_usage_before': {'source': 'CODEX_APP_SERVER_OFFICIAL', 'observed': self.base,
                    'ordinary_allowed': True, 'windows': [{'minutes': n, 'used_percent': 1, 'resets_at': self.base + 1000} for n in (300,10080)]},
                'process': {'exit_code': 0, 'timed_out': False, 'pipes_complete': True, 'stdout_truncated': False, 'stderr_truncated': False},
                'validation': {'status': 'PASS', 'reason_codes': [], 'completed_turns': 1,
                    'final_response': contract.EXPECTED, 'usage': {'input_tokens': 1,'output_tokens': 1}}}}
        self.worker = Worker(self)
        self.guards = [patch.object(subprocess, 'Popen', side_effect=AssertionError('NO_PROCESS')),
                       patch.object(socket, 'socket', side_effect=AssertionError('NO_NETWORK'))]
        for guard in self.guards: guard.start()

    def tearDown(self):
        for guard in self.guards: guard.stop()
        self.temp.cleanup()

    def new_broker(self):
        return Broker(self.path, bot_sid=self.bot_sid, admin_sid=self.admin_sid, service_sid=self.service_sid,
                      utc=lambda: self.base + self.elapsed, monotonic=lambda: self.elapsed, context=self.context)

    def rehash(self):
        self.ticket['ticket_sha256'] = contract.digest({k:v for k,v in self.ticket.items() if k!='ticket_sha256'})

    def arm(self): self.broker.arm(self.admin, self.permit)
    def submit(self): return self.broker.submit(self.bot, self.ticket, self.worker)

    def test_normal_arm_delivery_sanitized_response(self):
        self.arm(); result = self.submit()
        self.assertEqual('VERIFIED', result['status'])
        self.assertEqual(contract.EXPECTED, result['final_response'])
        self.assertEqual((1,1,0), (self.worker.calls,self.worker.closed,self.worker.active))
        self.assertEqual(result, self.broker.result(self.bot,self.ticket['request_id']))

    def test_same_sid_limited_token_cannot_arm(self):
        self.broker.admin_sid = self.bot_sid
        with self.assertRaises(Denied): self.broker.arm(self.bot,self.permit)
        with self.assertRaises(Denied): self.broker.arm(Peer(self.bot_sid,True,True,True,12288),self.permit)

    def test_elevated_flag_without_admin_rejected(self):
        with self.assertRaises(Denied): self.broker.arm(Peer(self.admin_sid,True,False,True,12288),self.permit)

    def test_wrong_admin_sid_rejected(self):
        with self.assertRaises(Denied): self.broker.arm(Peer('other',True,True,True,12288),self.permit)

    def test_remote_peer_rejected(self):
        with self.assertRaises(Denied): self.broker.arm(Peer(self.bot_sid,True,True,False,12288),self.permit)

    def test_json_admin_claim_not_peer(self):
        with self.assertRaises(Denied): self.broker.arm({'sid':self.bot_sid,'elevated':True},self.permit)

    def test_unarmed_request_rejected(self):
        with self.assertRaises(Denied): self.submit()
        self.assertEqual(0,self.worker.calls)

    def test_second_arm_rejected(self):
        self.arm()
        with self.assertRaises(Denied): self.arm()

    def test_duplicate_consumption_rejected(self):
        self.arm(); self.submit()
        with self.assertRaises(Denied): self.submit()
        self.assertEqual(1,self.worker.calls)

    def test_owner_guild_channel_app_tamper_rejected(self):
        self.arm()
        original = copy.deepcopy(self.ticket)
        for key in ('owner','guild','channel','application'):
            self.ticket=copy.deepcopy(original); self.ticket[key]=1; self.rehash()
            with self.subTest(key=key), self.assertRaises(ValueError): self.submit()
        self.assertEqual(0,self.worker.calls)

    def test_request_id_and_full_sha_tamper_rejected(self):
        self.arm(); self.ticket['request_id']='b'*32; self.rehash()
        with self.assertRaises(Denied): self.submit()

    def test_plan_tamper_rejected(self):
        self.arm(); self.ticket['plan']['scope']='WRITE'; self.rehash()
        with self.assertRaises(ValueError): self.submit()

    def test_expired_arm_rejected(self):
        self.arm(); self.elapsed=301
        with self.assertRaises(Denied): self.submit()

    def test_expired_ticket_rejected(self):
        self.arm(); self.elapsed=91
        with self.assertRaises(ValueError): self.submit()

    def test_cancel_before_launch(self):
        self.arm(); self.broker.cancel(self.bot,self.ticket['request_id'])
        with self.assertRaises(Denied): self.submit()
        self.assertEqual(0,self.worker.calls)

    def test_cancel_during_work_stops_tree(self):
        self.arm(); self.worker.missing=True
        self.worker.callback=lambda:self.broker.cancel(self.bot,self.ticket['request_id'])
        self.assertEqual('HOLD',self.submit()['status'])
        self.assertEqual(0,self.worker.active)

    def test_missing_result_deadline_stops_tree(self):
        self.arm(); self.worker.missing=True
        self.assertEqual('HOLD',self.submit()['status'])
        self.assertLessEqual(self.elapsed,90)
        self.assertEqual(0,self.worker.active)

    def test_late_result_not_accepted(self):
        self.arm(); self.worker.slow=True
        self.assertEqual('HOLD',self.submit()['status'])

    def test_cleanup_unknown_hold(self):
        self.arm(); self.worker.dirty=True
        self.assertEqual('HOLD',self.submit()['status'])

    def test_start_failure_consumed_no_retry(self):
        self.arm(); self.worker.fail=True
        self.assertEqual('HOLD',self.submit()['status'])
        with self.assertRaises(Denied): self.submit()

    def test_restart_revokes_armed_permit(self):
        self.arm(); self.broker=self.new_broker()
        with self.assertRaises(Denied): self.submit()

    def test_restart_retains_consumption(self):
        self.arm(); self.submit(); self.broker=self.new_broker()
        with self.assertRaises(Denied): self.submit()
        with self.assertRaises(Denied): self.arm()

    def test_wrong_service_context_holds(self):
        for key,value in [('session',9),('logon',2),('admin',True),('protected_install',False),('protected_state',False),('fixed_task',False),('approved_package',False),('manual',False)]:
            self.context[key],before=value,self.context[key]
            with self.subTest(key=key),self.assertRaises(Denied): self.arm()
            self.context[key]=before

    def test_service_same_sid_as_bot_holds(self):
        self.broker.service_sid=self.bot_sid
        with self.assertRaises(Denied): self.arm()

    def test_quota_exhausted_result_hold(self):
        self.arm(); self.child['entry_result']['status']='NOT_VERIFIED'
        self.child['entry_result']['inference_exec_launches']=0
        self.child['entry_result']['official_usage_before']['windows'][0]['used_percent']=100
        self.assertEqual('HOLD',self.submit()['status'])

    def test_missing_final_hold(self):
        self.arm(); self.child['entry_result']['validation']['final_response']=None
        self.assertEqual('HOLD',self.submit()['status'])

    def test_projection_strips_private_fields(self):
        self.arm(); self.child['private_path']='PRIVATE'; self.child['stderr']='SECRET'
        self.assertNotIn('PRIVATE',json.dumps(self.submit()))

    def test_unknown_operation_and_file_paths_rejected(self):
        for msg in ({'op':'exec','path':'evil.exe'},{'op':'arm','permit':self.permit,'admin':True}):
            self.assertEqual('HOLD',dispatch(self.broker,self.admin,json.dumps(msg).encode(),self.worker)['status'])

    def test_oversize_duplicate_json_rejected(self):
        for raw in (b'x'*16385,b'{"op":"arm","op":"submit"}'):
            self.assertEqual('HOLD',dispatch(self.broker,self.admin,raw,self.worker)['status'])

    def test_forged_projection_cannot_enter_live(self):
        pipe=WindowsPipeTransport('LimitlessPlanning-'+'a'*32,source_pins={'Session0PipeBoundary.cs':'0'*64,'session0-pipe-client.ps1':'0'*64})
        with self.assertRaises(Denied): TrustedProjection.from_pipe(pipe,{'status':'VERIFIED'})
        with self.assertRaises(Denied): PipeReply({'status':'VERIFIED'})
        with self.assertRaises(Denied): LiveSession0Bridge(pipe)._project(self.ticket,{'status':'VERIFIED'})

    def pipe_fixture(self, value):
        folder=Path(self.temp.name)
        boot=folder/'boot.json'; boot.write_text(json.dumps({'service_sid':self.service_sid}),encoding='utf8')
        pins={}
        for name in ('Session0PipeBoundary.cs','session0-pipe-client.ps1'):
            (folder/name).write_bytes(b'OFFLINE_SOURCE'); pins[name]=hashlib.sha256(b'OFFLINE_SOURCE').hexdigest()
        class Process:
            returncode=0
            def communicate(inner, raw, timeout): return json.dumps(value,ensure_ascii=False).encode(),None
        class Fixture:
            def __enter__(inner):
                inner.guards=[patch.object(transport_module,'BOOT',boot),patch.object(transport_module,'BOOT_SHA',hashlib.sha256(boot.read_bytes()).hexdigest()),
                    patch.object(transport_module,'HERE',folder),patch.object(subprocess,'Popen',return_value=Process()),patch('time.time',return_value=self.base)]
                for guard in inner.guards:guard.start()
                return WindowsPipeTransport('LimitlessPlanning-'+'a'*32,source_pins=pins)
            def __exit__(inner,*args):
                for guard in reversed(inner.guards):guard.stop()
        return Fixture()

    def test_mocked_ipc_helper_complete_reply(self):
        result=contract.project_child(self.child,self.ticket,self.base)
        with self.pipe_fixture(result) as pipe:
            reply=LiveSession0Bridge(pipe).submit(self.ticket)
            self.assertFalse(reply.fixture)
            self.assertEqual(contract.EXPECTED,reply.value['final_response'])

    def test_mocked_ipc_wrong_hash_rejected(self):
        result=contract.project_child(self.child,self.ticket,self.base);result['task_sha256']='0'*64
        with self.pipe_fixture(result) as pipe,self.assertRaises(Denied):LiveSession0Bridge(pipe).submit(self.ticket)

    def test_mocked_ipc_private_usage_field_rejected(self):
        result=contract.project_child(self.child,self.ticket,self.base);result['usage']['private']='secret'
        with self.pipe_fixture(result) as pipe,self.assertRaises(Denied):LiveSession0Bridge(pipe).submit(self.ticket)

    def test_result_lookup_never_reexecutes(self):
        self.arm();self.submit()
        for _ in range(3): self.broker.result(self.bot,self.ticket['request_id'])
        self.assertEqual(1,self.worker.calls)


if __name__=='__main__': unittest.main()
