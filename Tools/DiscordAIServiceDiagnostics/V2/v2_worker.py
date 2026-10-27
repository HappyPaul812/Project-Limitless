"""고정 V2 Worker 실행·bounded 출력. 테스트는 Popen 대신 가짜 handle을 주입합니다."""
import json
import struct
import subprocess
import threading
import time
from v2_contract import need, strict

class Worker:
    def __init__(self, root, manifest_sha, binding_sha, *, clock=time.monotonic, popen=None):
        self.root,self.manifest_sha,self.binding_sha,self.clock,self.popen=root,manifest_sha,binding_sha,clock,popen or subprocess.Popen
    def start(self, request, epoch, deadline):
        budget=min(80000,int((deadline-self.clock()-1)*1000));need(budget>=1000)
        command=['C:/Program Files/PowerShell/7/pwsh.exe','-NoProfile','-NonInteractive','-File',str(self.root/'Code/V2/fixed-worker-v2.ps1'),'-ManifestSha',self.manifest_sha,'-BindingSha',self.binding_sha]
        raw=json.dumps({'request_id':request,'epoch':epoch,'budget_ms':budget},allow_nan=False).encode();need(len(raw)<=16384)
        process=self.popen(command,stdin=subprocess.PIPE,stdout=subprocess.PIPE,stderr=subprocess.PIPE,cwd=str(self.root),creationflags=getattr(subprocess,'CREATE_NO_WINDOW',0))
        try: process.stdin.write(struct.pack('<I',len(raw))+raw);process.stdin.flush()
        except Exception:
            process.kill();raise ValueError('V2_WORKER_START_HOLD') from None
        return Handle(process,self.clock)

class Handle:
    def __init__(self,process,clock):
        self.p,self.clock=process,clock;self.buffers={};self.totals={};self.threads=[]
        for name,cap in (('stdout',16384),('stderr',4096)):
            self.buffers[name]=bytearray();self.totals[name]=0
            def drain(selected=name,limit=cap):
                try:
                    while True:
                        data=getattr(process,selected).read(4096)
                        if not data: break
                        self.totals[selected]+=len(data);room=limit-len(self.buffers[selected]);self.buffers[selected].extend(data[:max(0,room)])
                finally: getattr(process,selected).close()
            t=threading.Thread(target=drain,daemon=True);t.start();self.threads.append(t)
    def poll(self,seconds):
        try: self.p.wait(timeout=max(0,seconds))
        except subprocess.TimeoutExpired: return None
        for t in self.threads:t.join(timeout=0.05)
        need(not any(t.is_alive() for t in self.threads) and self.p.returncode==0 and self.totals['stdout']<=16384 and self.totals['stderr']<=4096)
        return strict(bytes(self.buffers['stdout']).decode('utf-8-sig'))
    def close(self,deadline):
        if not self.p.stdin.closed:self.p.stdin.close()
        try:self.p.wait(timeout=max(0.01,min(1,deadline-self.clock())))
        except subprocess.TimeoutExpired:
            self.p.kill()
            try:self.p.wait(timeout=0.2)
            except subprocess.TimeoutExpired:pass
            return False  # wrapper 종료만으로 전체 Job 정리를 증명하지 않습니다.
        for t in self.threads:t.join(timeout=max(0,min(0.1,deadline-self.clock())))
        return self.clock()<=deadline and not any(t.is_alive() for t in self.threads)
