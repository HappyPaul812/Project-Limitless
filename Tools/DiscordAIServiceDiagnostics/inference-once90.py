"""서비스 자기 구독으로 usage App Server1개 후 추론 exec1개만 허용하는 격리 entry입니다."""
import argparse
import asyncio
import hashlib
import importlib.util
import json
import os
from pathlib import Path
import re
import sys
import time

HERE = Path(__file__).resolve().parent
TOOL = HERE.parent / 'DiscordAICollab'
sys.path.insert(0, str(TOOL))
from collab.core import Denied
from collab.phase2 import probe_command
from collab.service_ai import prepare_provider, windows_context
from collab.usage_diagnostics import no_link, safe_error, safe_text
from collab.private_execution_log import prepare_archive, save_execution_log

spec = importlib.util.spec_from_file_location('inference_core', HERE / 'inference-once-core.py')
core = importlib.util.module_from_spec(spec); spec.loader.exec_module(core)
BOOT = Path('C:/ProgramData/LimitlessRelay/boot-approval.json')
PERMIT = Path('C:/ProgramData/LimitlessRelay/inference-once90-approval.json')
CHANGED = {'collab/usage_bridge.py', 'collab/service_ai.py', 'collab/service_batch_once.py'}
NEW = 'collab/usage_diagnostics.py'
SERVICE_CONFIG_SHA = '818c2e7b52f1c574fd1f9e0ac49dd876ad10746ff5ad95ce427d636938747bc3'


def digest(path, deadline):
    no_link(path); value = hashlib.sha256()
    with path.open('rb') as source:
        for block in iter(lambda: source.read(1048576), b''):
            core.remaining(deadline); value.update(block)
    return value.hexdigest()


def read(path):
    no_link(path)
    with path.open('rb') as source: raw = source.read(65537)
    if len(raw) > 65536: raise Denied('INFERENCE_JSON_SIZE_HOLD')
    value = core.strict_json(raw.decode('utf-8-sig'))
    if not isinstance(value, dict): raise Denied('INFERENCE_JSON_FORMAT_HOLD')
    return value


def preserved_keyring_control(home):
    """기존 인증 당시 keyring 설정을 그대로 선택합니다. 저장·전환·토큰 읽기는 하지 않습니다.

    exec의 ignore-user-config가 이 설정도 생략하므로 동일한 저장소 선택만 명시합니다.
    현재 서비스 설정이 기존 승인 기록과 다르면 임의 해석하지 않고 모델/usage 전에 멈춥니다.
    """
    path = home/'config.toml'; no_link(path)
    with path.open('rb') as source: raw = source.read(4097)
    if len(raw) > 4096 or hashlib.sha256(raw).hexdigest() != SERVICE_CONFIG_SHA:
        raise Denied('INFERENCE_EXISTING_AUTH_CONFIG_CHANGED_HOLD')
    return 'cli_auth_credentials_store="keyring"'


def verify(base, permit, request, deadline):
    keys = {'approved', 'kind', 'request_id', 'expires_at', 'boot_approval_sha256', 'trial_tool_root',
            'entry_sha256', 'core_sha256', 'roles_sha256', 'runtime_sha256', 'cli_sha256', 'policy'}
    if (set(permit) != keys or permit['approved'] is not True or permit['kind'] != 'INFERENCE_ONCE90'
        or permit['request_id'] != request or permit['policy'] != core.POLICY
        or any(type(permit['policy'][k]) is not type(v) for k, v in core.POLICY.items())): raise Denied('INFERENCE_PERMIT_HOLD')
    if type(permit['expires_at']) not in (int, float) or not time.time() < permit['expires_at'] <= time.time()+600:
        raise Denied('INFERENCE_PERMIT_EXPIRED_HOLD')
    if digest(BOOT, deadline) != permit['boot_approval_sha256']: raise Denied('INFERENCE_BOOT_SHA_HOLD')
    if Path(permit['trial_tool_root']).resolve() != TOOL.resolve(): raise Denied('INFERENCE_ROOT_HOLD')
    for path, key in [(Path(__file__), 'entry_sha256'), (HERE/'inference-once-core.py', 'core_sha256'), (TOOL/'roles.json', 'roles_sha256')]:
        if digest(path, deadline) != permit[key]: raise Denied('INFERENCE_ENTRY_SHA_HOLD')
    old = base['runtime_sha256']; candidate = permit['runtime_sha256']
    if len(old) != 43 or len(candidate) != 44 or set(candidate) != set(old)|{NEW} or NEW in old:
        raise Denied('INFERENCE_RUNTIME_SET_HOLD')
    if {f'collab/{p.name}' for p in (TOOL/'collab').glob('*.py')} != {k for k in candidate if k.startswith('collab/')}:
        raise Denied('INFERENCE_RUNTIME_COUNT_HOLD')
    for name, value in candidate.items():
        path = TOOL/name
        if not path.resolve().is_relative_to(TOOL.resolve()): raise Denied('INFERENCE_RUNTIME_PATH_HOLD')
        no_link(path.parent)
        if name not in CHANGED|{NEW} and value != old[name]: raise Denied('INFERENCE_UNCHANGED40_HOLD')
        if digest(path, deadline) != value: raise Denied('INFERENCE_RUNTIME_SHA_HOLD')
    pin = base['service_codex']
    if permit['cli_sha256'] != pin['sha256'] or digest(Path(pin['path']), deadline) != pin['sha256']:
        raise Denied('INFERENCE_CLI_SHA_HOLD')


def run(request, budget_ms):
    result = {'status': 'NOT_VERIFIED', 'stage': 'PREFLIGHT', 'reason': 'INFERENCE_NOT_STARTED',
              'request_id': request, 'inference_exec_launches': 0, 'usage_app_server_launches': 0,
              'application_retries': 0, 'discord_posts': 0, 'sqlite_work_operations': 0,
              'mode_changed': False, 'internal_retry_count': 'UNKNOWN', 'policy': core.POLICY}
    deadline = time.monotonic()+budget_ms/1000
    archive = None; last = None; snapshot = None; workspace = None
    try:
        if not re.fullmatch('[a-f0-9]{32}', request) or not 1000 <= budget_ms <= 75000: raise Denied('INFERENCE_ARGUMENT_HOLD')
        base = read(BOOT); permit = read(PERMIT)
        context = windows_context()
        provider = prepare_provider(root=TOOL, context=context, approval={**base, 'tool_root': str(TOOL)})
        home = Path(provider.environment['CODEX_HOME'])
        no_link(home); no_link(home.parent)
        if (context['name'] != 'PAUL-PC\\LIMITLESSRelaySvc' or context['sid'] != base['service_sid']
            or os.environ.get('LIMITLESS_BATCH_LOGON_TYPE') != '4'
            or os.environ.get('LIMITLESS_BATCH_SESSION_ID') != '0'): raise Denied('INFERENCE_SERVICE_CONTEXT_HOLD')
        verify(base, permit, request, deadline)
        store_control = preserved_keyring_control(home)
        result['credential_store_selection'] = 'EXISTING_KEYRING_EXPLICIT'
        archive = prepare_archive(home.parent, request)
        no_link(archive); last = archive/'final-response.txt'
        workspace = TOOL.parents[2]/'EmptyWorkspace'
        no_link(workspace)
        if not workspace.is_dir() or any(workspace.iterdir()): raise Denied('INFERENCE_EMPTY_WORKSPACE_HOLD')
        snapshot = []
        (archive/'preflight.safe.json').write_text(json.dumps({'request_id': request,
            'stage': 'BEFORE_USAGE', 'account': context['name'], 'session_id': 0, 'logon_type': 4,
            'policy': core.POLICY, 'cli_sha256': permit['cli_sha256'],
            'existing_auth_config_sha256': SERVICE_CONFIG_SHA}), encoding='utf8')
        slot = home/'limitless-inference-once90'
        no_link(slot); slot.mkdir(exist_ok=True)
        with (slot/(request+'.consumed')).open('x', encoding='ascii') as target: target.write('INFERENCE_ONCE90_NO_RETRY')
        # 공식 usage read 안의 account/read가 구독 인증을 확인하므로 login/features CLI는 추가하지 않습니다.
        result['stage'] = 'OFFICIAL_USAGE_BEFORE'
        core.remaining(deadline, reserve=8, minimum=20)
        result['usage_app_server_launches'] = 1
        result['official_usage_before'] = asyncio.run(provider.read(stage='OFFICIAL_USAGE_AND_APPROVAL'))
        value = result['official_usage_before']
        if value.get('ordinary_allowed') is not True or any(w['used_percent'] >= 100 for w in value['windows']):
            raise Denied('INFERENCE_USAGE_HOLD')
        if time.time()-value['observed'] > 30: raise Denied('INFERENCE_USAGE_STALE_HOLD')
        command = probe_command(provider.executable, workspace)[:-1]
        # 고정CLI는 tools.view_image를무시합니다. 실제features조회에서 false를확인한키만사용합니다.
        for item in [store_control, 'features.view_image=false', 'features.image_generation=false',
                     'features.sleep_tool=false', 'features.tool_suggest=false',
                     'features.hooks=false', 'features.memories=false']:
            command += ['-c', item]
        command += ['--output-last-message', str(last), '-']
        if any('model_providers.' in arg for arg in command): raise Denied('INFERENCE_PROVIDER_OVERRIDE_HOLD')
        result['stage'] = 'INFERENCE_ONCE'
        core.remaining(deadline, reserve=4, minimum=5)
        (archive/'launch.safe.json').write_text(json.dumps({'request_id': request,
            'stage': result['stage'], 'argv': command, 'environment_keys': sorted(provider.environment),
            'prompt_sha256': hashlib.sha256(core.PROMPT.encode('utf8')).hexdigest(),
            'remaining_seconds_before_spawn': core.remaining(deadline), 'policy': core.POLICY}), encoding='utf8')
        result['inference_exec_launches'] = 1
        # 이 한 지점만 추론 CLI 프로세스를 만듭니다. 오류/timeout/출력거부 뒤 재호출 경로는 없습니다.
        captured = core.collect(command, core.PROMPT, provider.environment, workspace, deadline-3)
        result['process'] = {k: v for k, v in captured.items() if k not in ('stdout', 'stderr')}
        secrets = [v for k, v in os.environ.items() if any(word in k.upper() for word in ('TOKEN', 'SECRET', 'PASSWORD', 'API_KEY', 'USERNAME', 'USERPROFILE'))]
        save_execution_log(archive, captured['stdout'], captured['stderr'],
                           {'stage': result['stage'], 'request_id': request, 'policy': core.POLICY, 'process': result['process']}, secrets)
        # 구조화 오류의 정수code·정제message는 자기 보호 프로필에만 보관합니다.
        failures = []
        for line in captured['stdout'].decode('utf8', errors='replace').splitlines():
            try:
                event = core.strict_json(line)
                if isinstance(event, dict) and event.get('type') in ('error', 'turn.failed'):
                    raw = event.get('error') or {'message': event.get('message')}
                    failures.append({'event': event['type'], 'error': safe_error(raw, secrets)})
            except (ValueError, TypeError): pass
        (archive/'errors.safe.json').write_text(json.dumps({'stage': result['stage'], 'errors': failures[:8],
            'exit_code': captured['exit_code'], 'stderr': safe_text(captured['stderr'].decode('utf8', errors='replace'), 4096, secrets)}), encoding='utf8')
        if captured['timed_out'] or not captured['pipes_complete'] or captured['stdout_truncated'] or captured['stderr_truncated']:
            raise Denied('INFERENCE_PROCESS_OR_OUTPUT_HOLD')
        if last.exists() and last.stat().st_size <= 4096:
            final = last.read_text(encoding='utf8')
        else: final = None
        result['stage'] = 'JSONL_VALIDATION'
        result['validation'] = core.validate(captured['stdout'], captured['exit_code'], final)
        if result['validation']['status'] != 'PASS': raise Denied('INFERENCE_RESPONSE_HOLD')
        if any(workspace.iterdir()): raise Denied('INFERENCE_WORKSPACE_CHANGED_HOLD')
        core.remaining(deadline)
        result.update(status='VERIFIED', stage='COMPLETE', reason='EXACT_READONLY_SENTENCE_VERIFIED')
    except FileExistsError: result['reason'] = 'INFERENCE_REQUEST_ALREADY_CONSUMED_HOLD'
    except TimeoutError: result['reason'] = 'INFERENCE_DEADLINE_HOLD'
    except Exception as exc:
        value = str(exc) if isinstance(exc, Denied) else 'INFERENCE_EXECUTION_HOLD'
        result['reason'] = value if re.fullmatch('[A-Z0-9_]+', value) else 'INFERENCE_EXECUTION_HOLD'
        result['error_type'] = type(exc).__name__
        result['os_error'] = {key: getattr(exc, key, None) for key in ('errno', 'winerror')}
    finally:
        if workspace is not None and snapshot is not None and any(workspace.iterdir()):
            result.update(status='NOT_VERIFIED', reason='INFERENCE_WORKSPACE_CHANGED_HOLD')
        if last is not None:
            try: last.unlink(missing_ok=True)
            except OSError: result.update(status='NOT_VERIFIED', reason='INFERENCE_FINAL_CLEANUP_HOLD')
        if archive is not None:
            try: (archive/'result.safe.json').write_text(json.dumps(result, ensure_ascii=False), encoding='utf8')
            except OSError: result.update(status='NOT_VERIFIED', reason='INFERENCE_DIAGNOSTIC_WRITE_HOLD')
    return result


if __name__ == '__main__':
    parser = argparse.ArgumentParser(); parser.add_argument('--request-id', required=True)
    parser.add_argument('--budget-ms', type=int, required=True); args = parser.parse_args()
    value = run(args.request_id, args.budget_ms)
    print(json.dumps(value, ensure_ascii=False))
    raise SystemExit(0 if value['status'] == 'VERIFIED' else 2)
