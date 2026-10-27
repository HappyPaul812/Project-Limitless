"""r11 원본은 읽기만 합니다. 명시적 계약 adapter 아래 검증된 native/wire 구현을 조합합니다."""
import argparse
import hashlib
import json
import re
import uuid
import zipfile
from pathlib import Path

HERE=Path(__file__).resolve().parent
REPO=HERE.parents[2]
ORIGINAL=REPO.parents[2]
BASE=ORIGINAL/'Temp/Session0V2Candidate20261027-r11'
BASE_MANIFEST='e8d6ed623934a577f88a5bb99ebafc78a2a469c10485a0c648c40dd457356dc0'
def sha(b):return hashlib.sha256(b).hexdigest()
def read(p):
    s=p.lstat()
    if p.is_symlink() or getattr(s,'st_file_attributes',0)&0x400:raise ValueError('D3_SOURCE_REPARSE')
    return p.read_bytes()
def build(destination):
    dst=Path(destination).absolute();temp=(ORIGINAL/'Temp').resolve()
    relative=dst.resolve().relative_to(temp)
    if len(relative.parts)!=1 or not re.fullmatch(r'Session0DedicatedV2Candidate20261027-r\d+',relative.name):raise ValueError('D3_OUTPUT_SCOPE')
    archive=dst.with_suffix('.zip');sidecar=dst.with_suffix('.package.json')
    if any(p.exists() for p in (dst,archive,sidecar)):raise ValueError('D3_NEW_ONLY')
    raw=read(BASE/'HostManifest.json')
    if sha(raw)!=BASE_MANIFEST:raise ValueError('D3_BASE_MANIFEST')
    old=json.loads(raw);files={};reuse={};transforms={}
    def borrowed(name):
        b=read(BASE/name)
        if sha(b)!=old['files_sha256'][name]:raise ValueError('D3_BORROWED_SHA')
        reuse[name]=sha(b);return b
    for n in old['files_sha256']:
        if n.startswith('Runtime/'):files[n]=borrowed(n)
    for n in ('Session0PipeBoundary.cs','V2Publish.cs','V2Host.cs','BrokerJob90.cs','service-token-metadata.py','entry-v2.py','v2_worker.py','v2_runtime.py','v2_transport.py'):
        files['Code/V2/'+n]=borrowed('Code/V2/'+n)
    # 변경 위치를 제한·검사합니다. 코드 전체에 이름/경로를 전역 치환하지 않습니다.
    stage=borrowed('Code/V2/V2Staging.cs').decode('utf8').splitlines();root_slots=scope_slots=0
    for i,line in enumerate(stage):
        if 'public const string Root=' in line:stage[i]=' public const string Root=DedicatedContract.StagingRoot;';root_slots+=1
        if '"STAGE_INSTALL_SCOPE"' in line:stage[i]='   Need(DedicatedContract.StageBundle(bundle),"STAGE_INSTALL_SCOPE");';scope_slots+=1
    if (root_slots,scope_slots)!=(1,1):raise ValueError('D3_STAGE_ADAPTER_SLOTS')
    files['Code/V2/V2Staging.cs']=('\n'.join(stage)+'\n').encode();transforms['V2Staging.cs']='Root dependency+dedicated bundle validator, exact two slots'
    fence=borrowed('Code/V2/V2WorkerFence.cs').decode('utf8').splitlines();slots=0
    for i,line in enumerate(fence):
        if 'Math.Abs(created[rootPid]' in line:fence[i]='   if(!created.ContainsKey(rootPid)||created[rootPid]/10!=System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime().Ticks/10)return false;';slots+=1
    if slots!=1:raise ValueError('D3_FENCE_ADAPTER_SLOT')
    files['Code/V2/V2WorkerFence.cs']=('\n'.join(fence)+'\n').encode();transforms['V2WorkerFence.cs']='WMI microsecond generation equality, one slot'
    for p in HERE.iterdir():
        if p.name.startswith(('test_','build-')) or p.suffix not in ('.cs','.ps1','.py'):continue
        key=('Deployment/' if p.name=='install-session0-v2.ps1' else 'Code/V2/')+p.name
        files[key]=read(p)
    # 기존 staging bootstrap은 검증 메모리 assembly를 로드합니다. 결과 protocol label만3으로 맞춥니다.
    stage_ps=borrowed('Deployment/stage-v2-package.ps1').decode('utf8')
    marker='schema=2;status=\'HOLD\''
    if stage_ps.count(marker)!=1:raise ValueError('D3_STAGE_PS_SLOT')
    files['Deployment/stage-dedicated.ps1']=stage_ps.replace(marker,'schema=3;status=\'HOLD\'').encode()
    preserved=json.loads(borrowed('Deployment/V1Preservation.json'))
    pins=json.loads(borrowed('Deployment/OperationalPins.json'))
    files['Deployment/Preservation.json']=(json.dumps({'v1_files':preserved,'onboot':pins,'r11_artifacts':{str(BASE/'HostManifest.json'):BASE_MANIFEST,str(BASE.with_suffix('.zip')):'0250f736b44798d3ba73b5d9674fc154fa233bc74e3b972f85931c2aa59a41d0'}},sort_keys=True,indent=2)+'\n').encode()
    files['Deployment/AuthenticationPlan.md']=read(HERE/'AuthenticationPlan.md')
    usings=set();bodies=[]
    for n in sorted(k for k in files if k.endswith('.cs')):
        body=[]
        for line in files[n].decode('utf-8-sig').splitlines():
            if re.fullmatch(r'using [A-Za-z0-9_.]+;',line.strip()):usings.add(line.strip())
            else:body.append(line)
        bodies.append('\n'.join(body))
    files['Code/V2/InstallAssembly.cs']=('\n'.join(sorted(usings))+'\n'+'\n'.join(bodies)+'\n').encode()
    from v2_contract import manifest,CODE,STATE_BASE,AUTH,APPROVAL,STAGING,BOT,WORKER,TASK,relative as validate_relative
    package=uuid.uuid4().hex
    m={'schema':3,'wire_schema':2,'kind':'LIMITLESS_DEDICATED_V2_CANDIDATE','package_id':package,'epoch':'20261027-dedicated-'+package,'approval_id':'dedicated-install-'+package,'approved':False,'auto_start':False,'execution_mode':'FAKE_ONLY','max_seconds':90,'max_parallel_jobs':1,'bot_account':BOT,'worker_account':WORKER,'code_root':CODE,'state_root':STATE_BASE,'codex_home':AUTH,'approval_root':APPROVAL,'staging_root':STAGING,'task_name':TASK,'pipe_name':'LimitlessDedicatedV2-'+package,'auth_policy':'EMPTY_NO_IMPORT_NO_LOGIN','worker_install_state':'ENABLED_BATCH_ONLY','real_ai_enabled':False,'files_sha256':{k:sha(v) for k,v in sorted(files.items())}}
    raw=(json.dumps(m,ensure_ascii=False,sort_keys=True,indent=2)+'\n').encode();manifest(raw)
    for n in files:validate_relative(n)
    dst.mkdir()
    for n,b in files.items():p=dst/n;p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(b)
    (dst/'HostManifest.json').write_bytes(raw);all_files=dict(files);all_files['HostManifest.json']=raw
    with zipfile.ZipFile(archive,'x',compression=zipfile.ZIP_DEFLATED) as z:
        for n,b in sorted(all_files.items()):info=zipfile.ZipInfo(n,(2026,10,27,0,0,0));info.compress_type=zipfile.ZIP_DEFLATED;z.writestr(info,b)
    with zipfile.ZipFile(archive) as z:
        if z.testzip() or len(z.namelist())!=len(all_files) or set(z.namelist())!=set(all_files) or any(sha(z.read(n))!=sha(b) for n,b in all_files.items()):raise ValueError('D3_ZIP')
    result={'status':'DEDICATED_OFFLINE_CANDIDATE_NOT_APPROVED_NOT_INSTALLED','candidate':str(dst),'zip':str(archive),'package_id':package,'payload_count':len(files),'installer_sha256':sha(files['Deployment/install-session0-v2.ps1']),'manifest_sha256':sha(raw),'zip_sha256':sha(read(archive)),'file_sha256':{k:sha(v) for k,v in all_files.items()},'borrowed_sha256':reuse,'explicit_adapters':transforms,'r11_diff':{'added':sorted(set(files)-set(old['files_sha256'])),'changed':sorted(n for n in files if n in old['files_sha256'] and sha(files[n])!=old['files_sha256'][n]),'omitted_not_deleted':sorted(set(old['files_sha256'])-set(files))},'actual_account_creation':0,'actual_authentication':0,'actual_installs':0}
    sidecar.write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf8');print(json.dumps({k:v for k,v in result.items() if k not in ('file_sha256','borrowed_sha256','explicit_adapters','r11_diff')}));return result
if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--destination',required=True);build(p.parse_args().destination)
