"""봇/서비스 공통의 순수 계약. 인증·프로세스·네트워크·승인 발급 기능은 없습니다."""
import hashlib
import json
import math
import re

TRIAL_APP = 1564156403571105906
PRODUCTION_APP = 1563102982386749440
GUILD = 1563112744436244541
CHANNEL = 1564163234338967552
OWNER = 740791801987727412
EXPECTED = 'LIMITLESS 기획 담당 AI입니다. 읽기 전용 연결 시험에 성공했습니다.'
POLICY = {'role': 'PlanningAgent', 'scope': 'PLANNING_CONNECTIVITY_READONLY',
          'model_exec_max': 1, 'usage_app_server_max': 1, 'automatic_retries': 0,
          'local_limit_seconds': 90, 'sandbox': 'read-only', 'tools': False,
          'provider': 'openai', 'authentication': 'existing_chatgpt_keyring', 'mode': 'MANUAL',
          'internal_http_retries': 'OFFICIAL_BUILTIN_DEFAULT_ALLOWED',
          'internal_stream_reconnects': 'OFFICIAL_BUILTIN_DEFAULT_ALLOWED',
          'remote_requests_exactly_one_guaranteed': False}


def digest(value):
    return hashlib.sha256(json.dumps(value, sort_keys=True, ensure_ascii=False, allow_nan=False).encode()).hexdigest()


def strict(text):
    def pairs(items):
        out = {}
        for key, value in items:
            if key in out: raise ValueError('DUPLICATE_KEY')
            out[key] = value
        return out
    return json.loads(text, object_pairs_hook=pairs,
                      parse_constant=lambda _: (_ for _ in ()).throw(ValueError('NONFINITE')))


def validate_ticket(ticket, arm, now):
    keys = {'request_id', 'task_id', 'task_sha256', 'plan', 'owner', 'application', 'guild', 'channel',
            'approved_until', 'created', 'deadline', 'policy', 'ticket_sha256'}
    if not isinstance(ticket, dict) or set(ticket) != keys: raise ValueError('TICKET_FIELDS')
    if not re.fullmatch('[0-9a-f]{32}', ticket['request_id']): raise ValueError('REQUEST_ID')
    if ticket['ticket_sha256'] != digest({k: v for k, v in ticket.items() if k != 'ticket_sha256'}): raise ValueError('TICKET_SHA')
    if ticket['task_sha256'] != digest(ticket['plan']): raise ValueError('PLAN_SHA')
    if ticket['policy'] != POLICY or any(type(ticket['policy'][k]) is not type(v) for k, v in POLICY.items()): raise ValueError('POLICY')
    plan = ticket['plan']
    if (plan.get('role') != POLICY['role'] or plan.get('scope') != POLICY['scope']
            or plan.get('expected') != EXPECTED or plan.get('policy') != POLICY
            or plan.get('task_id') != ticket['task_id']): raise ValueError('PLANNING_SCOPE')
    for key, expected in [('owner', OWNER), ('application', TRIAL_APP), ('guild', GUILD), ('channel', CHANNEL)]:
        if type(ticket[key]) is not int or ticket[key] != expected: raise ValueError('IDENTITY_BINDING')
    for key in ('approved_until', 'created', 'deadline'):
        if type(ticket[key]) not in (int, float) or not math.isfinite(ticket[key]): raise ValueError('TIME_TYPE')
    if not ticket['created'] <= now < ticket['deadline'] <= ticket['created'] + 90: raise ValueError('DEADLINE')
    if ticket['created'] >= ticket['approved_until'] or now >= ticket['approved_until']: raise ValueError('APPROVAL_EXPIRED')
    if (arm.get('request_id') != ticket['request_id'] or arm.get('task_sha256') != ticket['task_sha256']
            or arm.get('ticket_sha256') != ticket['ticket_sha256'] or arm.get('policy') != POLICY): raise ValueError('ARM_BINDING')
    return True


def quota_available(value, now):
    if not isinstance(value, dict) or value.get('source') != 'CODEX_APP_SERVER_OFFICIAL': return False
    observed = value.get('observed')
    if type(observed) not in (int, float) or not math.isfinite(observed) or not 0 <= now - observed <= 30: return False
    windows = value.get('windows')
    if not isinstance(windows, list) or len(windows) != 2: return False
    if any(not isinstance(w, dict) for w in windows) or {w.get('minutes') for w in windows} != {300, 10080}: return False
    for w in windows:
        if any(type(w.get(k)) not in (int, float) or not math.isfinite(w[k]) for k in ('used_percent', 'resets_at')): return False
        if not 0 <= w['used_percent'] < 100 or w['resets_at'] <= now: return False
    return value.get('ordinary_allowed') is True


def project_child(child, ticket, now):
    """기존 성공 wrapper 결과에서 고정 문장과 숫자만 공개합니다."""
    entry = child.get('entry_result') or {}
    validation, process = entry.get('validation') or {}, entry.get('process') or {}
    job = child.get('job') or {}
    before = entry.get('official_usage_before') or {}
    observed = before.get('observed')
    usage_preflight_valid = (type(observed) in (int, float) and math.isfinite(observed)
                             and ticket['created'] <= observed <= now and quota_available(before, observed))
    def number(data, key, value):
        return type(data.get(key)) is int and data[key] == value
    passed = (child.get('request_id') == ticket['request_id'] and entry.get('request_id') == ticket['request_id']
              and child.get('account') == 'PAUL-PC\\LIMITLESSRelaySvc'
              and child.get('sid_matches') is True and child.get('session_id') == 0 and child.get('logon_type') == 4
              and number(child, 'session_id', 0) and number(child, 'logon_type', 4)
              and child.get('status') == 'VERIFIED' and entry.get('status') == 'VERIFIED'
              and number(process, 'exit_code', 0) and process.get('timed_out') is False
              and process.get('pipes_complete') is True and process.get('stdout_truncated') is False
              and process.get('stderr_truncated') is False and validation.get('status') == 'PASS'
              and validation.get('reason_codes') == [] and number(validation, 'completed_turns', 1)
              and validation.get('final_response') == EXPECTED and number(entry, 'inference_exec_launches', 1)
              and number(entry, 'usage_app_server_launches', 1) and number(entry, 'application_retries', 0)
              and entry.get('credential_store_selection') == 'EXISTING_KEYRING_EXPLICIT'
              and entry.get('mode_changed') is False and number(job, 'exit_code', 0)
              and child.get('codex_home_unchanged') is True and job.get('timed_out') is False
              and job.get('cleanup_verified') is True and number(job, 'active_processes', 0)
              and usage_preflight_valid and now <= ticket['deadline'])
    usage = validation.get('usage') or {}
    safe_usage = {k: usage[k] for k in ('input_tokens', 'output_tokens', 'cached_input_tokens')
                  if type(usage.get(k)) is int and usage[k] >= 0}
    return {'request_id': ticket['request_id'], 'task_id': ticket['task_id'], 'task_sha256': ticket['task_sha256'],
            'ticket_sha256': ticket['ticket_sha256'], 'status': 'VERIFIED' if passed else 'HOLD',
            'final_response': EXPECTED if passed else None, 'usage': safe_usage if passed else {},
            'reason': 'EXACT_PLANNING_SENTENCE' if passed else 'SERVICE_RESULT_REJECTED',
            'finished': now, 'role': 'PlanningAgent', 'model_exec_launches': entry.get('inference_exec_launches', 0)}
