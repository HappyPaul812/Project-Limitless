"""설치 전 검토용 공개 코드/독립 Python/기존 frozen47 패키지. 계정·토큰·서비스 변경 없음."""
import argparse
import hashlib
import json
from pathlib import Path
import shutil
import sys
import uuid
import zipfile


def prepare(repo, destination, runtime_source, prepared):
    repo,destination,runtime_source,prepared=map(Path,(repo,destination,runtime_source,prepared))
    if destination.exists():raise ValueError('NEW_BUNDLE_DIRECTORY_REQUIRED')
    destination.mkdir(parents=True)
    # 서비스 인증/프로필/DB는 읽거나 복사하지 않습니다. manifest에 고정된 소스만 다룹니다.
    code=destination/'Code/Tools'
    (code/'DiscordAICollab/collab').mkdir(parents=True)
    for source in (repo/'Tools/DiscordAICollab/collab').glob('*.py'):
        shutil.copyfile(source,code/'DiscordAICollab/collab'/source.name)
    shutil.copyfile(repo/'Tools/DiscordAICollab/roles.json',code/'DiscordAICollab/roles.json')
    shutil.copyfile(repo/'Tools/DiscordAICollab/DiscordSecretStore.psm1',code/'DiscordAICollab/DiscordSecretStore.psm1')
    diag=code/'DiscordAIServiceDiagnostics';diag.mkdir()
    names=['Session0Host.cs','Session0PipeBoundary.cs','BrokerJob90.cs','mobile-worker-contract.py',
           'broker-host-entry.py','fixed-planning-worker.ps1','fixed-planning-entry.py','start-session0-host.ps1',
           'session0-pipe-client.ps1','probe-session0-host.py','TrialAccountPolicy.cs','service-token-metadata.py','setup-trial-bot-own-token.ps1']
    for name in names:shutil.copyfile(repo/'Tools/DiscordAIServiceDiagnostics'/name,diag/name)
    (destination/'Deployment').mkdir()
    shutil.copyfile(repo/'Tools/DiscordAIServiceDiagnostics/install-session0-trial.ps1',destination/'Deployment/install-session0-trial.ps1')
    previous=json.loads((prepared/'Manifest.json').read_text(encoding='utf-8-sig'))
    expected=previous['files_sha256']
    with zipfile.ZipFile(prepared/'inference-package.zip') as archive:
        if len(archive.namelist())!=47 or set(archive.namelist())!=set(expected):raise ValueError('LEGACY47_HOLD')
        for name in archive.namelist():
            if '..' in Path(name).parts or '\\' in name or name.startswith('/'):raise ValueError('LEGACY_PATH_HOLD')
            data=archive.read(name)
            if hashlib.sha256(data).hexdigest()!=expected[name]:raise ValueError('LEGACY_SHA_HOLD')
            target=destination/'Legacy'/name;target.parent.mkdir(parents=True,exist_ok=True);target.write_bytes(data)
    # 원본 frozen entry의 readonly 빈 workspace 위치입니다.
    (destination/'EmptyWorkspace').mkdir()
    runtime=destination/'Runtime';runtime.mkdir()
    for source in runtime_source.glob('*.dll'):shutil.copyfile(source,runtime/source.name)
    shutil.copyfile(runtime_source/'python.exe',runtime/'python.exe')
    shutil.copytree(runtime_source/'DLLs',runtime/'DLLs',ignore=shutil.ignore_patterns('__pycache__','*.pyc'))
    shutil.copytree(runtime_source/'Lib',runtime/'Lib',ignore=shutil.ignore_patterns('site-packages','__pycache__','*.pyc','test','tests','idlelib','tkinter','sitecustomize.py','usercustomize.py'))
    for pattern in ('libssl*.dll','libcrypto*.dll','libffi*.dll','ffi*.dll','sqlite*.dll','zlib*.dll','liblzma*.dll','libbz2*.dll','libexpat*.dll'):
        for source in (runtime_source/'Library/bin').glob(pattern):shutil.copyfile(source,runtime/source.name)
    (runtime/'python312._pth').write_text('.\nLib\nDLLs\n',encoding='ascii')
    files={str(p.relative_to(destination)).replace('\\','/'):hashlib.sha256(p.read_bytes()).hexdigest()
           for p in sorted(destination.rglob('*')) if p.is_file()}
    config={'schema':1,'execution_mode':'FAKE_ONLY','bot_account':'LIMITLESSBotTrial','bot_sid':'SET_AT_INSTALL',
            'admin_sid':'SET_AT_INSTALL','worker_sid':'FROM_EXISTING_BOOT_APPROVAL','state_root':'C:/ProgramData/LimitlessRelayTrial',
            'task_name':'LIMITLESS-Trial-S0Host','pipe_name':'LimitlessPlanning-'+uuid.uuid4().hex,
            'files_sha256':files,'legacy_permit':previous['permit'],'auto_start':False}
    boot=json.loads(Path('C:/ProgramData/LimitlessRelay/boot-approval.json').read_text(encoding='utf8'))
    config['external_cli_root']=str(Path(boot['service_codex']['path']).parent.parent)
    config['external_cli_files']=previous['official_package_sha256']
    config['legacy_permit'].update(approved=False,request_id='',expires_at=0,trial_tool_root='SET_AT_INSTALL')
    (destination/'HostManifest.json').write_text(json.dumps(config,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
    return {'bundle':str(destination),'files':len(files),'manifest_sha256':hashlib.sha256((destination/'HostManifest.json').read_bytes()).hexdigest(),
            'installer_sha256':hashlib.sha256((destination/'Deployment/install-session0-trial.ps1').read_bytes()).hexdigest()}


if __name__=='__main__':
    parser=argparse.ArgumentParser();parser.add_argument('--destination',required=True);args=parser.parse_args()
    repo=Path(__file__).resolve().parents[2]
    print(json.dumps(prepare(repo,args.destination,Path(sys.base_prefix),repo.parent/'InferenceTrial90AuthFixPrepared20261026'),ensure_ascii=False))
