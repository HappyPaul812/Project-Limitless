"""Session0 계약 어댑터. 현재 미배포 후보이며 이번에는 합성 검증만 수행합니다.

실전 arm/인증된 pipe/Scheduler 전달은 별도 관리자 배포 검증 전까지 닫혀 있습니다.
봇 프로세스에 서비스 인증이나 임의 명령 실행 함수를 전달하지 않습니다.
"""
from dataclasses import dataclass
import importlib.util
from pathlib import Path

from .core import Denied
from .mobile_planning_buttons import contract

DIAGNOSTICS = Path(__file__).resolve().parents[2] / 'DiscordAIServiceDiagnostics'
PINS = {'inference-once90.py': 'b841cf6557046ada96b67564be7e093dcb0666980577de8ab37273dd0c22b1ab',
        'inference-once-core.py': 'c9fa6329849ed28713bbfb0fb2f7ded043a04c721c02c10c80fb71d5206adcdf',
        'InferenceJob90.cs': '93a9cd26ee7bf268e9327cb10bed92cc27d273ffc697e9da5b679675cca846cf',
        'run-approved-inference-authfix-once90.ps1': 'a9986f29c4a6ee8dc4d52439a86fce3cf451ac3f4e03b1daaae4d1ede0ec726a'}


@dataclass(frozen=True)
class TrustedProjection:
    value: dict
    fixture: bool = True

    def __post_init__(self):
        # 직접 live 객체를 만들 수 없고 인증된 pipe 응답 경로에서만 발급합니다.
        if self.fixture is not True:
            raise Denied('LIVE_PROTECTED_MAILBOX_NOT_ARMED')

    @classmethod
    def from_pipe(cls, transport, value):
        from .session0_pipe_transport import WindowsPipeTransport, PipeReply
        if type(transport) is not WindowsPipeTransport or type(value) is not PipeReply or not value.verified():
            raise Denied('WINDOWS_PIPE_PROOF_REQUIRED')
        # Windows 토큰으로 확인한 서버의 bounded 응답만 live로 발급합니다.
        result = cls.__new__(cls)
        object.__setattr__(result, 'value', value.value)
        object.__setattr__(result, 'fixture', False)
        return result


def verify_frozen_runner():
    import hashlib
    return all(hashlib.sha256((DIAGNOSTICS / name).read_bytes()).hexdigest() == sha for name, sha in PINS.items())


def deployment_plan():
    """실행 기능 없는 관리자 검토 자료. 이 결과는 permit이 아닙니다."""
    return {'approved': False, 'status': 'OFFLINE_READY_LIVE_HOLD', 'policy': dict(contract.POLICY),
            'runner_pins': dict(PINS), 'runtime_count': 44, 'runtime_unchanged': 40,
            'service': 'PAUL-PC\\LIMITLESSRelaySvc', 'session': 0, 'logon': 4,
            'native_budget_ms': 75000, 'python_budget_ms': 70000,
            'deadline_includes': ['handoff', 'preparation', 'usage', 'inference', 'cleanup'],
            'required_live_gates': ['separate_explicit_approval', 'admin_prearmed_temporary_task',
                'distinct_bot_admin_service_sids', 'real_service_sid_session0_logon4',
                'protected_install_and_service_state', 'native_authenticated_pipe',
                'outer_90s_monotonic_supervisor_and_cancel', 'exact44_and_cli_config_sha']}


class LiveSession0Bridge:
    """인증된 로컬 pipe만 사용합니다. 서비스/task를 시작하거나 승인하지 않습니다."""
    def __init__(self, transport=None):
        self.transport = transport

    def submit(self, ticket):
        from .session0_pipe_transport import WindowsPipeTransport
        if type(self.transport) is not WindowsPipeTransport:
            raise Denied('LIVE_DEPLOYMENT_NOT_APPROVED_OR_ARMED')
        import time
        contract.validate_ticket(ticket, ticket, time.time())
        value = self.transport.exchange({'op': 'submit', 'ticket': ticket}, ticket['deadline'])
        return self._project(ticket, value)

    def _project(self, ticket, reply):
        from .session0_pipe_transport import PipeReply
        if type(reply) is not PipeReply or not reply.verified(): raise Denied('SERVICE_AUTHENTICATED_REPLY_REQUIRED')
        value = reply.value
        keys = {'request_id', 'task_id', 'task_sha256', 'ticket_sha256', 'status', 'final_response',
                'usage', 'reason', 'finished', 'role', 'model_exec_launches'}
        if type(value) is not dict or set(value) != keys: raise Denied('SERVICE_REPLY_SCHEMA_HOLD')
        if any(value[k] != ticket[k] for k in ('request_id', 'task_id', 'task_sha256', 'ticket_sha256')):
            raise Denied('SERVICE_REPLY_BINDING_HOLD')
        if value['role'] != 'PlanningAgent' or value['status'] not in {'VERIFIED', 'HOLD'}:
            raise Denied('SERVICE_REPLY_STATUS_HOLD')
        if value['status'] == 'VERIFIED' and value['final_response'] != contract.EXPECTED:
            raise Denied('SERVICE_REPLY_FINAL_HOLD')
        import math
        if (value['reason'] not in {'EXACT_PLANNING_SENTENCE', 'SERVICE_RESULT_REJECTED'}
                or type(value['finished']) not in (int, float) or not math.isfinite(value['finished'])
                or not ticket['created'] <= value['finished'] <= ticket['deadline']
                or type(value['model_exec_launches']) is not int or value['model_exec_launches'] not in (0, 1)
                or type(value['usage']) is not dict
                or set(value['usage']) - {'input_tokens', 'output_tokens', 'cached_input_tokens'}
                or any(type(v) is not int or v < 0 for v in value['usage'].values())
                or (value['status'] == 'HOLD' and (value['final_response'] is not None or value['usage']))):
            raise Denied('SERVICE_SANITIZED_REPLY_HOLD')
        return TrustedProjection.from_pipe(self.transport, reply)

    def cancel(self, request):
        import time
        from .session0_pipe_transport import WindowsPipeTransport
        if type(self.transport) is not WindowsPipeTransport: raise Denied('LIVE_PIPE_NOT_CONFIGURED')
        return self.transport.exchange({'op': 'cancel', 'request_id': request}, time.time() + 2)


class OfflineSession0Harness:
    """합성 서비스 환경만 받고 기존 JSONL 검증 함수를 재사용합니다."""
    def __init__(self, clock, context, quota, jsonl, final, *, exit_code=0):
        self.clock, self.context, self.quota = clock, context, quota
        self.jsonl, self.final, self.exit_code = jsonl, final, exit_code
        self.consumed, self.active = set(), False
        self.fake_usage_calls = self.fake_exec_calls = 0

    def submit(self, ticket, arm):
        if not verify_frozen_runner(): raise Denied('FROZEN_RUNNER_SHA_HOLD')
        contract.validate_ticket(ticket, arm, self.clock())
        expected = {'account': 'PAUL-PC\\LIMITLESSRelaySvc', 'sid_matches': True,
                    'session_id': 0, 'logon_type': 4, 'is_admin': False}
        if self.context != expected or any(type(self.context[k]) is not type(v) for k, v in expected.items()):
            raise Denied('SERVICE_CONTEXT_HOLD')
        if self.active or ticket['request_id'] in self.consumed:
            raise Denied('ALREADY_CONSUMED_OR_BUSY')
        self.consumed.add(ticket['request_id'])
        self.active = True
        try:
            self.fake_usage_calls += 1
            if not contract.quota_available(self.quota, self.clock()): raise Denied('USAGE_HOLD')
            if self.clock() >= ticket['deadline']: raise Denied('DEADLINE_HOLD')
            self.fake_exec_calls += 1
            spec = importlib.util.spec_from_file_location('frozen_inference_validator', DIAGNOSTICS / 'inference-once-core.py')
            core = importlib.util.module_from_spec(spec)
            spec.loader.exec_module(core)
            validation = core.validate(self.jsonl, self.exit_code, self.final)
            entry = {'request_id': ticket['request_id'], 'status': 'VERIFIED' if validation['status'] == 'PASS' else 'NOT_VERIFIED',
                     'validation': validation, 'process': {'exit_code': self.exit_code, 'timed_out': False,
                        'pipes_complete': True, 'stdout_truncated': False, 'stderr_truncated': False},
                     'official_usage_before': self.quota, 'inference_exec_launches': 1, 'usage_app_server_launches': 1,
                     'application_retries': 0, 'credential_store_selection': 'EXISTING_KEYRING_EXPLICIT', 'mode_changed': False}
            child = {**expected, 'request_id': ticket['request_id'], 'status': entry['status'], 'entry_result': entry,
                     'codex_home_unchanged': True, 'job': {'exit_code': 0, 'timed_out': False, 'cleanup_verified': True, 'active_processes': 0}}
            return TrustedProjection(contract.project_child(child, ticket, self.clock()))
        finally:
            self.active = False


async def send_result_once(workflow, request, send):
    """Discord 응답 실패/불명은 재추론하지 않고 체크포인트에 남깁니다."""
    final = workflow.claim_report(request)
    if final is None: return False
    try:
        message = await send(final)
        if message is None: raise ValueError('MESSAGE_ID_MISSING')
        workflow.report_result(request, message)
        return True
    except Exception:
        workflow.report_result(request)
        return False


class OfflineButtonWorkerConnection:
    """버튼 callback → 소비된 ticket → 합성 Session0 → 검증 → 단발 보고 연결입니다."""
    def __init__(self, workflow, worker):
        if not workflow.offline_fixture or type(worker) is not OfflineSession0Harness:
            raise Denied('OFFLINE_CONNECTION_ONLY')
        self.workflow, self.worker = workflow, worker

    async def on_run(self, request, interaction):
        ticket = self.workflow.ticket(request)
        arm = {k: ticket[k] for k in ('request_id', 'task_sha256', 'ticket_sha256', 'policy')}
        try:
            projection = self.worker.submit(ticket, arm)
            if not self.workflow.accept_result(request, projection): return
        except Exception:
            self.workflow.hold(request)
            return
        async def send(text):
            import discord
            message = await interaction.followup.send(text, ephemeral=True, wait=True,
                                                       allowed_mentions=discord.AllowedMentions.none())
            return message.id
        await send_result_once(self.workflow, request, send)


class LiveButtonWorkerConnection:
    """승인된 시험 봇에 주입할 callback. 등록/접속/arm/자동 재실행은 하지 않습니다."""
    def __init__(self, workflow, bridge):
        if workflow.offline_fixture or type(bridge) is not LiveSession0Bridge:
            raise Denied('LIVE_BUTTON_CONNECTION_CONTRACT_HOLD')
        self.workflow,self.bridge=workflow,bridge
        self.started=set()  # 프로세스 재시작 후에도 서비스의 durable 소비 장부가 재실행을 차단합니다.

    async def on_run(self,request,interaction):
        import asyncio
        if request in self.started:return
        self.started.add(request)
        try:
            result=await asyncio.to_thread(self.bridge.submit,self.workflow.ticket(request))
            if not self.workflow.accept_result(request,result):return
        except Exception:
            self.workflow.hold(request);return
        async def send(text):
            import discord
            message=await interaction.followup.send(text,ephemeral=True,wait=True,allowed_mentions=discord.AllowedMentions.none())
            return message.id
        await send_result_once(self.workflow,request,send)

    async def on_cancel(self,request):
        import asyncio
        try:await asyncio.to_thread(self.bridge.cancel,request)
        except Exception:self.workflow.hold(request,'SERVICE_CANCEL_UNKNOWN_NO_RETRY')
