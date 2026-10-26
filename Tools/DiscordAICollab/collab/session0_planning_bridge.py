"""Session0 계약 어댑터. 현재 후보는 오프라인 전용이며 실제 프로세스를 실행하지 않습니다.

실전 arm/보호 mailbox/Scheduler 전달은 별도 관리자 배포 검증 전까지 닫혀 있습니다.
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
        # 실제 결과의 출처 검증을 구현하기 전에는 live 객체 발급 자체를 막습니다.
        if self.fixture is not True:
            raise Denied('LIVE_PROTECTED_MAILBOX_NOT_ARMED')


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
                'real_service_sid_session0_logon4', 'admin_owned_arm_and_result_acl',
                'bot_write_inbox_only', 'protected_projection_reader',
                'outer_90s_monotonic_supervisor_and_cancel', 'exact44_and_cli_config_sha']}


class LiveSession0Bridge:
    """실제 권한 경계 배포 전에는 모든 요청을 HOLD합니다."""
    def submit(self, ticket):
        raise Denied('LIVE_DEPLOYMENT_NOT_APPROVED_OR_ARMED')


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
