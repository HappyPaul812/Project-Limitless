"""고정 Planning Worker. wire에서 exe/argv/prompt/permit 경로를 받지 않습니다."""
import json
from pathlib import Path
import struct
import subprocess
import threading
import time

from .core import Denied
from .mobile_planning_buttons import contract


class FixedPlanningWorker:
    def __init__(self, install, config, *, clock=time.monotonic, popen=None):
        self.install, self.config, self.clock, self.popen = Path(install), config, clock, popen or subprocess.Popen

    def start(self, ticket, deadline):
        if self.config['execution_mode'] not in {'FAKE_ONLY', 'OWNER_APPROVED_READONLY_ONCE'}:
            raise Denied('FIXED_WORKER_MODE_HOLD')
        budget = min(75000, int((deadline - self.clock() - 1) * 1000))
        if budget < 1000: raise Denied('FIXED_WORKER_BUDGET_HOLD')
        command = ['C:/Program Files/PowerShell/7/pwsh.exe', '-NoProfile', '-NonInteractive', '-File',
                   str(self.install/'Code/Tools/DiscordAIServiceDiagnostics/fixed-planning-worker.ps1')]
        payload = json.dumps({'ticket': ticket, 'budget_ms': budget}, ensure_ascii=False, allow_nan=False).encode()
        if len(payload) > 16384:raise Denied('FIXED_WORKER_FRAME_HOLD')
        process = self.popen(command, stdin=subprocess.PIPE, stdout=subprocess.PIPE, stderr=subprocess.PIPE,
            cwd=str(self.install), creationflags=getattr(subprocess, 'CREATE_NO_WINDOW', 0))
        try:
            process.stdin.write(struct.pack('<I',len(payload))+payload); process.stdin.flush()
        except Exception:
            process.kill(); raise Denied('FIXED_WORKER_START_HOLD') from None
        # stdin을 열어 둡니다. 취소/부모 종료 시 EOF가 Native Job 취소로 전달됩니다.
        return FixedHandle(process, self.clock)


class FixedHandle:
    def __init__(self, process, clock):
        self.process, self.clock = process, clock
        self.buffers, self.totals = {}, {}
        self.threads = []
        for name, cap in (('stdout',32768),('stderr',8192)):
            self.buffers[name]=bytearray(); self.totals[name]=0
            def drain(selected=name, limit=cap):
                try:
                    while True:
                        block=getattr(process,selected).read(4096)
                        if not block: break
                        self.totals[selected]+=len(block)
                        room=limit-len(self.buffers[selected])
                        if room>0:self.buffers[selected].extend(block[:room])
                finally:getattr(process,selected).close()
            thread=threading.Thread(target=drain,daemon=True);thread.start();self.threads.append(thread)

    def poll(self, seconds):
        try:self.process.wait(timeout=max(0,seconds))
        except subprocess.TimeoutExpired:return None
        for thread in self.threads:thread.join(timeout=0.05)
        if (any(thread.is_alive() for thread in self.threads) or self.totals['stdout']>32768
                or self.totals['stderr']>8192 or self.process.returncode!=0):
            raise Denied('FIXED_WORKER_RESULT_HOLD')
        value=contract.strict(bytes(self.buffers['stdout']).decode('utf-8-sig'))
        if type(value) is not dict:raise Denied('FIXED_WORKER_SCHEMA_HOLD')
        return value

    def close(self, deadline):
        if not self.process.stdin.closed:self.process.stdin.close()
        try:self.process.wait(timeout=max(0.01,min(1.5,deadline-self.clock())))
        except subprocess.TimeoutExpired:
            self.process.kill()
            try:self.process.wait(timeout=max(0.01,min(0.2,deadline-self.clock())))
            except subprocess.TimeoutExpired:return False
            # wrapper 강제 종료만으로 성공을 추정하지 않습니다. 상위 Host Job도 전체 트리를 정리합니다.
            return False
        for thread in self.threads:thread.join(timeout=max(0,min(0.1,deadline-self.clock())))
        return self.clock()<=deadline and not any(t.is_alive() for t in self.threads)
