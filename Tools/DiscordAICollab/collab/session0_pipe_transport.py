"""정상 사용자 봇의 로컬 IPC 전용. service task 시작·임의 명령·인증 복사 기능 없음."""
import hashlib
import json
from pathlib import Path
import re
import subprocess
import time

from .core import Denied
from .mobile_planning_buttons import contract

HERE = Path(__file__).resolve().parents[2] / 'DiscordAIServiceDiagnostics'
BOOT = Path('C:/ProgramData/LimitlessRelay/boot-approval.json')
BOOT_SHA = '3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'
_REPLY_SEAL = object()


class PipeReply:
    def __init__(self, value, seal=None):
        if seal is not _REPLY_SEAL: raise Denied('AUTHENTICATED_PIPE_REPLY_REQUIRED')
        self.value, self._seal = value, seal

    def verified(self):
        return self._seal is _REPLY_SEAL


class WindowsPipeTransport:
    """별도 배포 승인으로 시작된 서비스 endpoint만 접속합니다. busy/오류 재접속0."""
    def __init__(self, pipe_name, *, source_pins):
        if not re.fullmatch(r'LimitlessPlanning-[a-f0-9]{32}', pipe_name): raise Denied('PIPE_NAME_HOLD')
        if set(source_pins) != {'Session0PipeBoundary.cs', 'session0-pipe-client.ps1'}: raise Denied('PIPE_PINS_HOLD')
        self.pipe_name, self.pins = pipe_name, dict(source_pins)

    def exchange(self, message, deadline):
        raw = json.dumps(message, ensure_ascii=False, allow_nan=False).encode()
        if len(raw) > 16384: raise Denied('PIPE_FRAME_HOLD')
        boot = BOOT.read_bytes()
        if hashlib.sha256(boot).hexdigest() != BOOT_SHA: raise Denied('PIPE_BOOT_SHA_HOLD')
        service_sid = contract.strict(boot.decode('utf-8-sig'))['service_sid']
        for name, sha in self.pins.items():
            if hashlib.sha256((HERE / name).read_bytes()).hexdigest() != sha: raise Denied('PIPE_SOURCE_SHA_HOLD')
        seconds = min(90.0, deadline - time.time())
        if seconds <= 1: raise Denied('PIPE_DEADLINE_HOLD')
        # 이 자식은 IPC만 담당합니다. 모델/usage CLI 실행이나 shell 해석은 없습니다.
        channel = {'arm': 'arm', 'submit': 'work', 'cancel': 'control', 'result': 'control'}.get(message.get('op'))
        if channel is None: raise Denied('PIPE_OPERATION_HOLD')
        command = ['C:/Program Files/PowerShell/7/pwsh.exe', '-NoProfile', '-NonInteractive', '-File',
                   str(HERE / 'session0-pipe-client.ps1'), '-PipeName', self.pipe_name + '-' + channel,
                   '-ServiceSid', service_sid, '-BudgetMs', str(max(1, int((seconds - 0.5) * 1000)))]
        process = subprocess.Popen(command, stdin=subprocess.PIPE, stdout=subprocess.PIPE, stderr=subprocess.DEVNULL,
                                   creationflags=getattr(subprocess, 'CREATE_NO_WINDOW', 0))
        try:
            output, _ = process.communicate(raw, timeout=seconds)
        except subprocess.TimeoutExpired:
            process.kill(); process.communicate(timeout=1)
            raise Denied('PIPE_TIMEOUT_NO_RETRY') from None
        if process.returncode != 0 or len(output) > 16384: raise Denied('PIPE_REPLY_HOLD')
        return PipeReply(contract.strict(output.decode('utf-8-sig')), _REPLY_SEAL)
