"""부팅 중계 전용 감독기. 고정 Bot/Art 경로만 실행하며 모델 실행/OS 종료 기능이 없습니다."""
import json
import os
import subprocess
import sys
import time
import urllib.request
from pathlib import Path

ROOT=Path(__file__).resolve().parent.parent

def network_ready():
    # 공개 Discord HTTPS만 확인합니다. 토큰·모델 API·개인 네트워크 주소를 전송하지 않습니다.
    try:
        request=urllib.request.Request('https://discord.com/api/v10/gateway',headers={'User-Agent':'LIMITLESS-Boot-Readiness'})
        class NoRedirect(urllib.request.HTTPRedirectHandler):
            def redirect_request(self,*args,**kwargs):return None
        with urllib.request.build_opener(NoRedirect()).open(request,timeout=10) as response:
            value=json.loads(response.read(4096))
        return value.get('url')=='wss://gateway.discord.gg'
    except Exception:return False

def delay(attempt):return min(120,5*(2**min(attempt,5)))

def prerequisites(root):
    # 원본 비밀 파일은 검사하지 않습니다. 새 계정 자신의 저장소 존재 여부만 확인합니다.
    checks={'sqlite':(root/'data/phase2/relay.sqlite').is_file(),
        'boot_token_store':(root/'data/private/boot-discord/connection.json').is_file(),
        'art_config':(root/'data/phase3/config.json').is_file()}
    return all(checks.values())

class Supervisor:
    def __init__(self,service,root=ROOT,network=network_ready,sleep=time.sleep,launch=None):
        if service not in ('Bot','Art'):raise ValueError('FIXED_SERVICE_REQUIRED')
        self.service,self.root,self.network,self.sleep=service,Path(root),network,sleep
        self.launch=launch or self.spawn
        self.state='NOT_STARTED';self.attempts=0
    def save(self,state):
        self.state=state
        folder=self.root/'data/boot';folder.mkdir(parents=True,exist_ok=True)
        (folder/(self.service+'-status.json')).write_text(json.dumps({'state':state,'launch_attempts':self.attempts,
            'ai':'HOLD','mode':'MANUAL_REQUIRED','at':time.time()}),encoding='utf8')
        return state
    def spawn(self):
        import shutil
        executable=shutil.which('pwsh')
        if not executable:raise ValueError('POWERSHELL_REQUIRED')
        # 임의 명령은 입력받지 않으며 API 키와 상위 계정의 CODEX_HOME을 전달하지 않습니다.
        allowed={'systemroot','windir','path','pathext','comspec','userprofile','appdata','localappdata','temp','tmp'}
        environment={k:v for k,v in os.environ.items() if k.lower() in allowed}
        action='Run' if self.service=='Bot' else 'Phase3Run'
        return subprocess.Popen([executable,'-NoProfile','-File',str(self.root/'register-discord.ps1'),
            '-Action',action,'-BootRelay'],cwd=self.root,env=environment,stdout=subprocess.DEVNULL,stderr=subprocess.DEVNULL,
            creationflags=getattr(subprocess,'CREATE_NO_WINDOW',0))
    def run(self,max_waits=12):
        stop=self.root/'data/boot'/(self.service+'-stop')
        waits=0
        while self.attempts<3:
            if stop.exists():return self.save('STOPPED_BY_OWNER')
            if not prerequisites(self.root):return self.save('HOLD_STORAGE_OR_NEW_ACCOUNT_TOKEN_SETUP')
            if not self.network():
                if waits>=max_waits:return self.save('WAITING_FOR_NETWORK_REVIEW_REQUIRED')
                self.save('WAITING_FOR_NETWORK');self.sleep(delay(waits));waits+=1;continue
            self.attempts+=1
            try:child=self.launch()
            except Exception:return self.save('HOLD_LAUNCH_FAILED')
            self.save('RELAY_RUNNING_AI_HOLD')
            while child.poll() is None:
                if stop.exists():
                    marker='bot-stop-request' if self.service=='Bot' else 'stop-request'
                    phase='phase2' if self.service=='Bot' else 'phase3'
                    (self.root/'data'/phase/marker).write_text('STOP',encoding='ascii')
                    # 강제 종료하지 않습니다. 기존 실행기가 안전 중지를 끝낼 때까지 기다립니다.
                    self.save('DRAINING_NO_FORCE_TERMINATION')
                self.sleep(5)
            if child.returncode==0:return self.save('STOPPED_SAFELY')
            if self.network():return self.save('HOLD_AUTH_OR_RUNTIME_NO_BLIND_RETRY')
            self.save('CONNECTION_LOST_RECHECK');self.sleep(delay(self.attempts))
        return self.save('HOLD_RECONNECT_LIMIT')

def main():
    # 승인·현재 서비스 SID·배포 SHA는 고정 PowerShell 진입점에서 먼저 확인합니다.
    if len(sys.argv)!=2 or sys.argv[1] not in ('Bot','Art'):raise SystemExit(2)
    import msvcrt
    folder=ROOT/'data/boot';folder.mkdir(parents=True,exist_ok=True)
    with (folder/(sys.argv[1]+'-supervisor.lock')).open('a+b') as lock:
        lock.seek(0);lock.write(b'1');lock.flush();lock.seek(0)
        try:msvcrt.locking(lock.fileno(),msvcrt.LK_NBLCK,1)
        except OSError:raise SystemExit(0)
        result=Supervisor(sys.argv[1]).run()
        # 지속 HOLD는 OS 재시작 폭주를 막기 위해 정상 종료하고 운영자 검토를 기다립니다.
        print(result)
if __name__=='__main__':main()
