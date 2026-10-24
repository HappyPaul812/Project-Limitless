"""실패 원문 대신 고정 분류와 내용을 제거한 재생용 이벤트만 남깁니다."""
import hashlib
import json
from collections import Counter
from pathlib import Path

from .output_parser import EVENTS, strict_json

# 인증정보·모델 추론·명령 인수를 기록하지 않도록 알려진 유형만 보존합니다.
ITEM_TYPES = {'reasoning', 'agent_message', 'command_execution', 'file_change', 'mcp_tool_call',
              'web_search', 'todo_list', 'error', 'warning', 'collab_tool_call', 'image_generation'}
ERROR_CODES = {'invalid_api_key', 'invalid_authentication', 'authentication_error', 'insufficient_quota',
               'rate_limit_exceeded', 'model_not_found', 'model_not_supported', 'permission_denied',
               'invalid_request_error', 'invalid_json_schema', 'unsupported_value', 'server_error'}
SIGNALS = {
    'AUTH': ('unauthorized', 'authentication failed', 'invalid_api_key', 'invalid authentication', '401'),
    'NETWORK': ('failed to connect', 'connection refused', 'connection reset', 'dns error',
                'network is unreachable', 'stream disconnected', 'reconnecting'),
    'TLS': ('certificate verify failed', 'invalid peer certificate', 'tls handshake'),
    'PERMISSION': ('permission denied', 'access is denied', 'os error 5'),
    'SANDBOX': ('sandbox failed', 'sandbox setup', 'helper_unknown_error'),
    'MODEL': ('model_not_found', 'model not found', 'model is not supported', 'model_not_supported'),
    'QUOTA': ('rate limit', 'quota', '429'),
    'CONFIG_SCHEMA': ('unknown field', 'unexpected argument', 'invalid_json_schema', 'invalid schema',
                      'unsupported parameter', 'unsupported_value'),
}


def classify(value):
    text = value.lower() if isinstance(value, str) else ''
    return [label for label, patterns in SIGNALS.items() if any(p in text for p in patterns)] or ['UNKNOWN']


def safe_error(event):
    """자유 형식 오류는 메모리에서 분류하고 비밀이 섞일 수 있는 원문은 반환하지 않습니다."""
    error = event.get('error')
    obj = error if isinstance(error, dict) else event
    message = obj.get('message', error if isinstance(error, str) else '')
    code = obj.get('code')
    if not (type(code) is int or isinstance(code, str) and code in ERROR_CODES):
        code = None
    return {'code': code, 'categories': classify(message), 'message_recorded': False}


def failure_projection(stdout, stderr):
    details = {'errors': [], 'turn_failed': [], 'items': [], 'invalid_lines': [],
               'stderr_categories': classify(stderr), 'raw_output_logged': False,
               'replay_kind': 'CONTENT_OMITTED_FAILURE_PROJECTION_NOT_ORIGINAL'}
    replay = []
    counts = Counter()
    for number, line in enumerate(stdout.splitlines(), 1):
        if not line.strip():
            continue
        try:
            event = strict_json(line)
            if not isinstance(event, dict):
                raise ValueError()
        except (ValueError, TypeError):
            details['invalid_lines'].append(number)
            replay.append('{INVALID_JSONL')
            continue
        kind = event.get('type')
        safe = {'type': kind if isinstance(kind, str) and kind in EVENTS else 'unknown_event'}
        if kind in ('error', 'turn.failed'):
            value = {'line': number, **safe_error(event)}
            details['errors' if kind == 'error' else 'turn_failed'].append(value)
            safe['error'] = {'code': value['code'], 'message': '[CONTENT_OMITTED]'}
        if kind in ('item.started', 'item.updated', 'item.completed'):
            item = event.get('item')
            original_type = item.get('type') if isinstance(item, dict) else None
            item_type = original_type if isinstance(original_type, str) and original_type in ITEM_TYPES else 'unknown_item'
            counts[item_type] += 1
            details['items'].append({'line': number, 'event_type': kind, 'item_type': item_type,
                                     'policy_allowed': item_type in ('reasoning', 'agent_message')})
            safe['item'] = {'type': item_type}
            if item_type == 'agent_message':
                safe['item']['text'] = '[CONTENT_OMITTED]'
        replay.append(json.dumps(safe))
    details['item_types'] = dict(counts)
    details['stdout_sha256'] = hashlib.sha256(stdout.encode()).hexdigest()
    details['stdout_chars'] = len(stdout)
    details['stderr_chars'] = len(stderr)
    return details, '\n'.join(replay)


def save_failure_evidence(directory, stdout, stderr):
    details, replay = failure_projection(stdout, stderr)
    directory = Path(directory)
    (directory / 'failure-diagnostics.json').write_text(json.dumps(details, indent=2), encoding='utf8')
    (directory / 'failure-projection.jsonl').write_text(replay, encoding='utf8')
    return details
