"""전용 Worker의 schema3 배포/OS binding. 기존 wire2 runtime을 엄격한 새 계약 아래 재사용합니다."""
import hashlib
import json
import ntpath
import os
import re

CODE=r'C:\Program Files\LimitlessRelayDedicatedV2_20261027'
STATE_BASE=r'C:\ProgramData\LimitlessRelayDedicatedV2_20261027'
STATE=STATE_BASE+r'\Data'
AUTH=STATE_BASE+r'\Auth\CodexHome'
APPROVAL=r'C:\ProgramData\LimitlessRelayDedicatedV2Approvals'
STAGING=r'C:\Program Files\LimitlessRelayDedicatedV2Staging20261027'
BOT='LIMITLESSBotV2D'
WORKER='LIMITLESSWorkV2D'
TASK='LIMITLESS-DedicatedV2-20261027-Host'
OLD_WORKER='S-1-5-21-2858504392-1958168942-1662906075-1007'
OLD_BOT='S-1-5-21-2858504392-1958168942-1662906075-1008'

def need(ok):
    if not ok: raise ValueError('DEDICATED_V2_CONTRACT_HOLD')

def strict(raw):
    def pairs(items):
        result={}
        for k,v in items: need(k not in result); result[k]=v
        return result
    return json.loads(raw,object_pairs_hook=pairs,parse_constant=lambda _: need(False))

def sha(data):return hashlib.sha256(data).hexdigest()
def sid(value):return type(value) is str and re.fullmatch(r'S-1-5-21-\d+-\d+-\d+-\d+',value) is not None
def relative(value):
    need(type(value) is str and re.fullmatch(r'(Code|Runtime|Deployment)/[^\x00-\x1f\\:*?<>|]+',value))
    for part in value.split('/'):
        need(part not in ('','.','..') and not part.endswith(('.', ' ')) and not re.match(r'^(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])(\.|$)',part,re.I))

def manifest(raw):
    m=strict(raw)
    need(type(m) is dict and set(m)=={'schema','wire_schema','kind','package_id','epoch','approval_id','approved','auto_start','execution_mode','max_seconds','max_parallel_jobs','bot_account','worker_account','code_root','state_root','codex_home','approval_root','staging_root','task_name','pipe_name','auth_policy','real_ai_enabled','worker_install_state','files_sha256'})
    p=m['package_id'];need(type(p) is str and re.fullmatch('[a-f0-9]{32}',p))
    need(type(m['schema']) is int and m['schema']==3 and type(m['wire_schema']) is int and m['wire_schema']==2 and m['kind']=='LIMITLESS_DEDICATED_V2_CANDIDATE')
    need(m['epoch']=='20261027-dedicated-'+p and m['approval_id']=='dedicated-install-'+p and m['pipe_name']=='LimitlessDedicatedV2-'+p)
    need(m['approved'] is False and m['auto_start'] is False and m['real_ai_enabled'] is False and m['execution_mode']=='FAKE_ONLY' and m['auth_policy']=='EMPTY_NO_IMPORT_NO_LOGIN' and m['worker_install_state']=='ENABLED_BATCH_ONLY' and type(m['max_seconds']) is int and m['max_seconds']==90 and type(m['max_parallel_jobs']) is int and m['max_parallel_jobs']==1)
    for k,v in {'bot_account':BOT,'worker_account':WORKER,'code_root':CODE,'state_root':STATE_BASE,'codex_home':AUTH,'approval_root':APPROVAL,'staging_root':STAGING,'task_name':TASK}.items():need(m[k]==v)
    need(type(m['files_sha256']) is dict and 8<len(m['files_sha256'])<=2048)
    names=set()
    for n,d in m['files_sha256'].items():relative(n);need(n.lower() not in names and type(d) is str and re.fullmatch('[a-f0-9]{64}',d));names.add(n.lower())
    return m

def binding(raw,m,manifest_sha):
    b=strict(raw)
    need(set(b)=={'schema','kind','package_id','epoch','approval_id','manifest_sha256','bot_sid','worker_sid','admin_sid','execution_mode','auto_start'})
    need(type(b['schema']) is int and b['schema']==3 and b['kind']=='DEDICATED_V2_OS_BINDING' and b['manifest_sha256']==manifest_sha)
    need(all(b[k]==m[k] for k in ('package_id','epoch','approval_id')) and b['execution_mode']=='FAKE_ONLY' and b['auto_start'] is False)
    roles=[b[k] for k in ('bot_sid','worker_sid','admin_sid')];need(all(sid(v) for v in roles) and len(set(roles+[OLD_WORKER,OLD_BOT]))==5)
    return b

def verify_install(root,ms,bs):
    need(ntpath.normcase(str(root))==ntpath.normcase(CODE))
    mr=(root/'HostManifest.json').read_bytes();need(sha(mr)==ms);m=manifest(mr)
    br=(root/'InstalledBinding.json').read_bytes();need(sha(br)==bs);b=binding(br,m,ms)
    need(os.environ.get('CODEX_HOME')==AUTH and not os.environ.get('OPENAI_API_KEY') and not os.environ.get('CODEX_API_KEY'))
    from pathlib import Path
    need(Path(AUTH).is_dir() and not any(Path(AUTH).iterdir()))
    for n,d in m['files_sha256'].items():
        if not n.startswith('Runtime/'):need(sha((root/n).read_bytes())==d)
    return m,b
