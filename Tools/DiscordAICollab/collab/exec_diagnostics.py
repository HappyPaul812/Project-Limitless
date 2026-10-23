"""실행 실패의 관측 지점만 기록합니다. stderr·환경값·인증 원문은 저장하지 않습니다."""
from pathlib import Path


def launch_manifest(command, environment, cli, controls, prompt):
    # command는 내부 probe_command가 만든 인수뿐이며 prompt/비밀은 argv에 없습니다.
    return {'argv':list(command),'launcher_cwd':str(Path.cwd()),
            'timeout_seconds':90,'stdin_utf8_bytes':len(prompt.encode()),
            'environment_keys':sorted(environment), 'environment_values_logged':False,
            'cli_version':cli['cli_version'],'chatgpt_auth_confirmed':cli['subscription_available'],
            'controls_status':controls['status'],'explicit_model':None,
            'model_resolution':'CLI_DEFAULT_NOT_INDEPENDENTLY_RECORDED'}


def failure_observation(result, stderr='', *, elapsed_ms, process_returned=False, os_error=None):
    events=result.get('diagnostics',{}).get('event_counts',{})
    if os_error is not None:stage='PROCESS_CREATION_FAILED'
    elif result.get('execution_status')=='TIMEOUT':stage='TIMEOUT_STAGE_UNCONFIRMED'
    elif events.get('turn.started') or events.get('turn.completed'):stage='TURN_EVENT_OBSERVED'
    elif events.get('thread.started'):stage='THREAD_EVENT_OBSERVED'
    elif not events and process_returned:stage='PROCESS_RETURNED_BEFORE_FIRST_JSONL_EVENT'
    else:stage='STAGE_UNCONFIRMED'
    raw=stderr.lower()
    signals=[]
    # 고정 분류는 진단용일 뿐 PASS/검증 판정에는 영향을 주지 않습니다.
    for label,patterns in (
        ('FILESYSTEM_PERMISSION_DENIED',('permission denied','access is denied','os error 5')),
        ('DATABASE_OPEN_FAILED',('unable to open database','failed to open database')),
        ('SCHEMA_READ_FAILED',('failed to read output schema','failed to read schema')),
        ('CONFIG_REJECTED',('unknown field','unexpected argument','reserved provider')),
        ('AUTH_REJECTED',('unauthorized','authentication failed')),
        ('NETWORK_FAILED',('failed to connect','connection refused','dns error')),
        ('TLS_FAILED',('certificate verify failed','invalid peer certificate'))):
        if any(p in raw for p in patterns):signals.append(label)
    return {'observed_stage':stage,'elapsed_ms':max(0,round(elapsed_ms)),
            'process_returned':process_returned,'completed_turn_observed':bool(events.get('turn.completed')),
            'stderr_signals':signals or ['UNCLASSIFIED_NO_RAW_LOG'],
            'os_errno':getattr(os_error,'errno',None),'windows_error':getattr(os_error,'winerror',None),
            'raw_output_logged':False}
