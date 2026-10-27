"""V1 미설치 공개 후보에서 Runtime만 SHA 대조 복사하고 V2 전용 코드로 새 ZIP을 만듭니다."""
import argparse
import hashlib
import json
import os
import re
from pathlib import Path
import uuid
import zipfile

HERE=Path(__file__).resolve().parent
REPO=HERE.parents[2]
ORIGINAL=REPO.parents[2]
BASE=ORIGINAL/'Temp/Session0LsaRepairCandidate20261026'
BASE_SHA='33ed9a8318c039cd70cf8260338f56e81025ce891c0156ac18556c2aec36d165'

def digest(data):return hashlib.sha256(data).hexdigest()
def plain(path):
    info=path.lstat()
    if path.is_symlink() or getattr(info,'st_file_attributes',0)&0x400:raise ValueError('V2_SOURCE_REPARSE_HOLD')
    return path.read_bytes()
def build(destination):
    destination=Path(destination).absolute();archive=destination.with_suffix('.zip')
    # 후보 생성기는 보호 설치 경로를 만들 수 없습니다. 프로젝트 Temp 아래 새 산출물만 허용합니다.
    try:
        temp=(ORIGINAL/'Temp').resolve();temp.relative_to(ORIGINAL.resolve())
        relative=destination.resolve().relative_to(temp)
        if len(relative.parts)!=1 or not re.fullmatch(r'Session0V2Candidate20261027-r[0-9]+',relative.name):raise ValueError()
    except ValueError:raise ValueError('V2_PACKAGE_DESTINATION_SCOPE_HOLD') from None
    if destination.exists() or archive.exists():raise ValueError('V2_NEW_CANDIDATE_ONLY')
    source_raw=plain(BASE/'HostManifest.json')
    if digest(source_raw)!=BASE_SHA:raise ValueError('V2_BASE_MANIFEST_PIN_HOLD')
    source=json.loads(source_raw.decode('utf-8-sig'))
    v1=plain(REPO/'문서/00_프로젝트/팀운영/Evidence/Session0LsaRepair20261026/BaselineInventory.json')
    if digest(v1)!='c2d398be42d7a12fb4574bff5e2a8820271ec82e355993f24e017a6e706f16d3':raise ValueError('V2_V1_INVENTORY_PIN_HOLD')
    old=json.loads(v1.decode('utf-8-sig'));old.pop('HostManifest.json')
    if len(old)!=855:raise ValueError('V2_V1_EXACT855_HOLD')
    payload={}
    for name,sha in source['files_sha256'].items():
        if name.startswith('Runtime/'):
            data=plain(BASE/name)
            if digest(data)!=sha:raise ValueError('V2_RUNTIME_COPY_SHA_HOLD')
            payload[name]=data
    for path in sorted(HERE.iterdir()):
        if path.name in ('run-session0-v2-install.ps1','stage-v2-package.ps1'):
            payload['Deployment/'+path.name]=plain(path);continue
        if path.name.startswith(('test_','build-','run-session0-','prepare-','probe-')) or path.name=='V2StagingPlan.ps1' or path.suffix not in ('.cs','.ps1','.py'):continue
        if path.name=='install-session0-v2.ps1':payload['Deployment/'+path.name]=plain(path)
        else:payload['Code/V2/'+path.name]=plain(path)
    original_diag=HERE.parent
    for name,expected in [('TrialAccountPolicy.cs','d9a0bc50137119bc28c17c5496b5fc20b4da81925b806bc472a538c4415f850b'),('BrokerJob90.cs','') ,('service-token-metadata.py','')]:
        key='Code/Tools/DiscordAIServiceDiagnostics/'+name
        data=plain(original_diag/name);pin=expected or source['files_sha256'][key]
        if digest(data)!=pin:raise ValueError('V2_REUSED_SOURCE_SHA_HOLD')
        payload['Code/V2/'+name]=data
    payload['Deployment/V1Preservation.json']=(json.dumps(old,sort_keys=True,indent=2)+'\n').encode()
    payload['Deployment/OperationalPins.json']=(json.dumps({'LIMITLESS-Art-OnBoot':'bb5ccabf40f7148fca44fdf538bd30cca2590e08122338cc0ae1c0d1836ab579','LIMITLESS-Bot-OnBoot':'e0c74168b935aa5d3fa418d65d66e03e49abeb1179975586c0ead4afd5b4df99'},sort_keys=True,indent=2)+'\n').encode()
    # 관리자 설치는 mutable candidate의 파일을 다시 compile하지 않고 검증한 bytes만 사용합니다.
    usings=set();bodies=[]
    for name in ('V2Contract.cs','V2Publish.cs','V2PolicyReadback.cs','TrialAccountPolicy.cs','V2Staging.cs'):
        lines=payload['Code/V2/'+name].decode('utf-8-sig').splitlines();body=[]
        for line in lines:
            if re.fullmatch(r'using [A-Za-z0-9_.]+;',line.strip()):usings.add(line.strip())
            else:body.append(line)
        bodies.append('\n'.join(body))
    payload['Code/V2/InstallAssembly.cs']=('\n'.join(sorted(usings))+'\n'+'\n'.join(bodies)+'\n').encode()
    from v2_contract import manifest,CODE,STATE,ACCOUNT,TASK,BOOT_SHA,relative
    package=uuid.uuid4().hex
    m={'schema':2,'kind':'LIMITLESS_SESSION0_V2_CANDIDATE','package_id':package,'epoch':'20261027-v2-'+package,'approval_id':'v2-install-'+package,'approved':False,'auto_start':False,'execution_mode':'FAKE_ONLY','max_seconds':90,'bot_account':ACCOUNT,'code_root':CODE,'state_root':STATE,'task_name':TASK,'pipe_name':'LimitlessPlanningV2-'+package,'boot_approval_sha256':BOOT_SHA,'files_sha256':{k:digest(v) for k,v in sorted(payload.items())}}
    raw=(json.dumps(m,ensure_ascii=False,sort_keys=True,indent=2)+'\n').encode();manifest(raw)
    for n in payload:relative(n)
    destination.mkdir(parents=True)
    for n,data in payload.items():p=destination/n;p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(data)
    (destination/'HostManifest.json').write_bytes(raw)
    all_files=dict(payload);all_files['HostManifest.json']=raw
    with zipfile.ZipFile(archive,'x',compression=zipfile.ZIP_DEFLATED) as z:
        for n,data in sorted(all_files.items()):
            info=zipfile.ZipInfo(n,date_time=(2026,10,27,0,0,0));info.compress_type=zipfile.ZIP_DEFLATED;z.writestr(info,data)
    with zipfile.ZipFile(archive) as z:
        if z.testzip() is not None or set(z.namelist())!=set(all_files) or any(digest(z.read(n))!=digest(v) for n,v in all_files.items()):raise ValueError('V2_ZIP_VERIFY_HOLD')
    result={'status':'V2_OFFLINE_CANDIDATE_NOT_APPROVED_NOT_INSTALLED','candidate':str(destination),'zip':str(archive),'package_id':package,'epoch':m['epoch'],'approval_id':m['approval_id'],'payload_count':len(payload),'approved':False,'auto_start':False,'execution_mode':'FAKE_ONLY','installer_sha256':digest(payload['Deployment/install-session0-v2.ps1']),'manifest_sha256':digest(raw),'zip_sha256':digest(plain(archive)),'file_sha256':{k:digest(v) for k,v in sorted(all_files.items())},'v1_comparison':{'v1_count':len(old),'unchanged':sorted(k for k in old if k in payload and old[k]==digest(payload[k])),'changed':sorted(k for k in old if k in payload and old[k]!=digest(payload[k])),'added':sorted(set(payload)-set(old)),'omitted_from_v2_not_deleted_from_v1':sorted(set(old)-set(payload))},'actual_system_installs':0}
    sidecar=destination.with_suffix('.package.json');sidecar.write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
    print(json.dumps({k:v for k,v in result.items() if k not in ('file_sha256','v1_comparison')},ensure_ascii=False))
    return result
if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--destination',required=True);a=p.parse_args();build(a.destination)
