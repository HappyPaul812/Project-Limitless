"""후보 패키지 검사4개. 설치/계정/서비스/Discord/모델/usage 작업은 없습니다."""
import argparse
import hashlib
import json
from pathlib import Path
import subprocess


def verify(root):
    root=Path(root);manifest=json.loads((root/'HostManifest.json').read_text(encoding='utf8'))
    expected=manifest['files_sha256']
    present={str(p.relative_to(root)).replace('\\','/') for p in root.rglob('*') if p.is_file()}
    checks={}
    checks['full_package_sha']=present==set(expected)|{'HostManifest.json'} and all(hashlib.sha256((root/name).read_bytes()).hexdigest()==sha for name,sha in expected.items())
    python=str(root/'Runtime/python.exe')
    host_code='import ssl,sqlite3,ctypes,asyncio,tomllib,sys;sys.path.insert(0,'+repr(str(root/'Code/Tools/DiscordAICollab'))+');import collab.broker_host_runtime;print("OK_HOST_IMPORT_ONLY")'
    legacy=root/'Legacy/Tools/DiscordAIServiceDiagnostics/inference-once90.py'
    worker_code='import importlib.util;s=importlib.util.spec_from_file_location("legacy_import_only",'+repr(str(legacy))+');m=importlib.util.module_from_spec(s);s.loader.exec_module(m);print("OK_WORKER_IMPORT_ONLY")'
    for name,code,expected_text in [('isolated_runtime_host_import',host_code,'OK_HOST_IMPORT_ONLY'),('frozen_worker_import_no_run',worker_code,'OK_WORKER_IMPORT_ONLY')]:
        result=subprocess.run([python,'-I','-B','-X','utf8','-c',code],capture_output=True,timeout=15)
        checks[name]=result.returncode==0 and result.stdout.decode().strip()==expected_text
    result=subprocess.run(['C:/Program Files/PowerShell/7/pwsh.exe','-NoProfile','-NonInteractive','-File',str(root/'Deployment/install-session0-trial.ps1'),'-Action','Plan'],capture_output=True,timeout=10)
    plan=json.loads(result.stdout)
    checks['installer_plan_no_changes']=result.returncode==0 and plan['system_changes']==0 and not plan['auto_start'] and not plan['task_enabled'] and plan['execution_mode']=='FAKE_ONLY'
    return {'checks':checks,'pass':sum(checks.values()),'fail':len(checks)-sum(checks.values()),'files':len(expected),
            'bytes':sum(p.stat().st_size for p in root.rglob('*') if p.is_file()),'manifest_sha256':hashlib.sha256((root/'HostManifest.json').read_bytes()).hexdigest(),
            'installer_sha256':hashlib.sha256((root/'Deployment/install-session0-trial.ps1').read_bytes()).hexdigest(),
            'actual_models':0,'actual_usage':0,'actual_discord':0,'system_changes':0}


if __name__=='__main__':
    parser=argparse.ArgumentParser();parser.add_argument('--bundle',required=True);parser.add_argument('--output',required=True);args=parser.parse_args()
    result=verify(args.bundle);Path(args.output).write_text(json.dumps(result,indent=2)+'\n',encoding='utf8');print(json.dumps(result))
    raise SystemExit(2 if result['fail'] else 0)
