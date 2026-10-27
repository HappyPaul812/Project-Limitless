"""C#/PowerShell과 같은 V2 schema2 계약. 검증은 명령/계정/IPC를 실행하지 않습니다."""
import hashlib
import json
import ntpath
import re

CODE = r'C:\Program Files\LimitlessRelayTrialV2_20261027'
STATE = r'C:\ProgramData\LimitlessRelayTrialV2_20261027'
ACCOUNT = 'LIMITLESSBotTrialV2'
TASK = 'LIMITLESS-Trial-V2-20261027-S0Host'
BOOT_SHA = '3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'
OLD_SID = 'S-1-5-21-2858504392-1958168942-1662906075-1008'

def need(value):
    if not value: raise ValueError('V2_CONTRACT_HOLD')

def strict(raw):
    def pairs(items):
        value = {}
        for k, v in items:
            need(k not in value); value[k] = v
        return value
    return json.loads(raw, object_pairs_hook=pairs, parse_constant=lambda x: need(False))

def sha(data): return hashlib.sha256(data).hexdigest()

def sid(value): return type(value) is str and re.fullmatch(r'S-1-5-21-\d+-\d+-\d+-\d+', value) is not None

def relative(name):
    need(type(name) is str and re.fullmatch(r'(Code|Runtime|Deployment)/[^\x00-\x1f\\:*?<>|]+', name))
    for p in name.split('/'):
        need(p not in ('', '.', '..') and not p.endswith(('.', ' ')) and not re.match(r'^(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])(\.|$)', p, re.I))

def manifest(raw):
    m = strict(raw)
    need(type(m) is dict and set(m) == {'schema','kind','package_id','epoch','approval_id','approved','auto_start','execution_mode','max_seconds','bot_account','code_root','state_root','task_name','pipe_name','boot_approval_sha256','files_sha256'})
    p = m['package_id']; need(type(p) is str and re.fullmatch('[a-f0-9]{32}', p))
    need(type(m['schema']) is int and m['schema'] == 2 and m['kind'] == 'LIMITLESS_SESSION0_V2_CANDIDATE')
    need(m['epoch'] == '20261027-v2-'+p and m['approval_id'] == 'v2-install-'+p and m['pipe_name'] == 'LimitlessPlanningV2-'+p)
    need(m['approved'] is False and m['auto_start'] is False and m['execution_mode'] == 'FAKE_ONLY' and type(m['max_seconds']) is int and m['max_seconds'] == 90)
    need((m['code_root'],m['state_root'],m['bot_account'],m['task_name'],m['boot_approval_sha256']) == (CODE,STATE,ACCOUNT,TASK,BOOT_SHA))
    need(type(m['files_sha256']) is dict and 8 < len(m['files_sha256']) <= 2048)
    seen = set()
    for name, digest in m['files_sha256'].items():
        relative(name); need(name.lower() not in seen); seen.add(name.lower())
        need(type(digest) is str and re.fullmatch('[a-f0-9]{64}', digest))
    return m

def binding(raw, m, manifest_sha):
    b = strict(raw)
    need(set(b) == {'schema','kind','package_id','epoch','approval_id','manifest_sha256','bot_sid','worker_sid','admin_sid','execution_mode','auto_start'})
    need(type(b['schema']) is int and b['schema'] == 2 and b['kind'] == 'LIMITLESS_V2_INSTALLED_BINDING')
    need(all(b[k] == m[k] for k in ('package_id','epoch','approval_id')) and b['manifest_sha256'] == manifest_sha)
    need(b['execution_mode'] == 'FAKE_ONLY' and b['auto_start'] is False)
    values = [b[k] for k in ('bot_sid','worker_sid','admin_sid')]
    need(all(sid(x) for x in values) and len(set(values+[OLD_SID])) == 4)
    return b

def verify_install(root, manifest_sha, binding_sha):
    # 실제 진입점만 고정 경로로 읽습니다. 테스트는 manifest()/binding()에 메모리 fixture를 전달합니다.
    need(ntpath.normcase(str(root)) == ntpath.normcase(CODE))
    raw = (root/'HostManifest.json').read_bytes(); need(sha(raw) == manifest_sha); m = manifest(raw)
    raw_b = (root/'InstalledBinding.json').read_bytes(); need(sha(raw_b) == binding_sha); b = binding(raw_b,m,manifest_sha)
    for name, digest in m['files_sha256'].items():
        if name.startswith('Runtime/'): continue  # Host가 시작 전에 전부 검증하고 코드 root는 Worker에게 읽기 전용입니다.
        path = root/name; need(not path.is_symlink() and sha(path.read_bytes()) == digest)
    return m, b
