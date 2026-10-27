"""V2 고정 client만 실행하는 IPC transport. 설치/Task/AI 진입점은 없습니다."""
import json
import subprocess
import time
from v2_contract import need, strict, sha

class Transport:
    def __init__(self,root,m,b,manifest_sha,binding_sha,*,popen=None,utc=time.time):
        self.root,self.m,self.b,self.ms,self.bs,self.popen,self.utc=root,m,b,manifest_sha,binding_sha,popen or subprocess.Popen,utc
    def exchange(self,message,deadline):
        channel={'arm':'arm','submit':'work','result':'control','cancel':'control'}.get(message.get('op'));need(channel is not None)
        need(message.get('epoch')==self.m['epoch'] and message.get('approval_id')==self.m['approval_id'])
        pipe=self.m['pipe_name']+'-'+channel
        for n in ('Code/V2/session0-client-v2.ps1','Code/V2/Session0PipeBoundary.cs','Code/V2/V2Contract.cs'):
            need(sha((self.root/n).read_bytes())==self.m['files_sha256'][n])
        raw=json.dumps(message,allow_nan=False).encode();need(len(raw)<=16384)
        seconds=min(90,deadline-self.utc());need(seconds>1)
        command=['C:/Program Files/PowerShell/7/pwsh.exe','-NoProfile','-NonInteractive','-File',str(self.root/'Code/V2/session0-client-v2.ps1'),'-PipeName',pipe,'-ManifestSha',self.ms,'-BindingSha',self.bs]
        proc=self.popen(command,stdin=subprocess.PIPE,stdout=subprocess.PIPE,stderr=subprocess.DEVNULL,creationflags=getattr(subprocess,'CREATE_NO_WINDOW',0))
        try:out,_=proc.communicate(raw,timeout=seconds)
        except subprocess.TimeoutExpired:
            proc.kill();proc.communicate(timeout=0.2);raise ValueError('V2_IPC_TIMEOUT_NO_RETRY') from None
        need(proc.returncode==0 and len(out)<=16384);value=strict(out.decode('utf-8-sig'));need(value.get('status')!='VERIFIED');return value
