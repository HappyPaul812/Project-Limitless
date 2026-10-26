"""단발 추론의 순수 검증·유한 출력 수집입니다. CLI 경로나 인증을 자체 선택하지 않습니다."""
import json
import math
import subprocess
import threading
import time
from collections import Counter

EXPECTED = 'LIMITLESS 기획 담당 AI입니다. 읽기 전용 연결 시험에 성공했습니다.'
PROMPT = ('다음 문장을 글자와 문장부호까지 정확히 그대로 답변하세요. 설명·인용부호·다른 문장을 추가하지 마세요. '
          '도구를 호출하거나 파일·외부 시스템을 읽거나 변경하지 마세요.\n' + EXPECTED)
POLICY = {'application_retries': 0, 'inference_exec_max': 1, 'usage_app_server_max': 1,
          'internal_http_retries': 'OFFICIAL_BUILTIN_DEFAULT_ALLOWED',
          'internal_stream_reconnects': 'OFFICIAL_BUILTIN_DEFAULT_ALLOWED',
          'remote_requests_exactly_one_guaranteed': False, 'local_limit_seconds': 90,
          'discord_posts': 0, 'sqlite_work_operations': 0, 'mode': 'MANUAL'}


def strict_json(text):
    def pairs(items):
        value = {}
        for key, item in items:
            if key in value: raise ValueError('DUPLICATE_JSON_KEY')
            value[key] = item
        return value
    return json.loads(text, object_pairs_hook=pairs,
                      parse_constant=lambda _: (_ for _ in ()).throw(ValueError('NONFINITE_JSON')))


def remaining(deadline, reserve=0, minimum=0.05, clock=time.monotonic):
    value = deadline - clock() - reserve
    if not math.isfinite(value) or value < minimum: raise TimeoutError('INFERENCE_DEADLINE_HOLD')
    return value


def collect(command, prompt, environment, cwd, deadline, clock=time.monotonic):
    """두 파이프를 동시에 끝까지 비웁니다. 초과분은 버리고 성공 판정은 차단합니다.

    timeout 시 루트 kill 후에도 자손이 pipe를 붙잡을 수 있으므로 join도 제한합니다.
    호출자는 반드시 별도 Windows Job 안에 있어야 전체 자손 종료가 보장됩니다.
    """
    remaining(deadline, minimum=0.2, clock=clock)
    output = {'stdout': bytearray(), 'stderr': bytearray()}
    totals = {'stdout': 0, 'stderr': 0}
    errors = []
    caps = {'stdout': 262144, 'stderr': 32768}
    process = subprocess.Popen(command, stdin=subprocess.PIPE, stdout=subprocess.PIPE, stderr=subprocess.PIPE,
                               env=environment, cwd=cwd, creationflags=getattr(subprocess, 'CREATE_NO_WINDOW', 0))
    def drain(name):
        try:
            while True:
                block = getattr(process, name).read(4096)
                if not block: return
                totals[name] += len(block)
                room = caps[name] - len(output[name])
                if room > 0: output[name].extend(block[:room])
        except OSError: errors.append(name)
        finally: getattr(process, name).close()
    def feed():
        try:
            process.stdin.write(prompt.encode('utf8')); process.stdin.close()
        except (OSError, ValueError): errors.append('stdin')
        finally:
            if not process.stdin.closed: process.stdin.close()
    threads = [threading.Thread(target=drain, args=(name,), daemon=True) for name in output]
    threads.append(threading.Thread(target=feed, daemon=True))
    for thread in threads: thread.start()
    timed_out = False
    try:
        process.wait(timeout=remaining(deadline, reserve=0.5, clock=clock))
    except (subprocess.TimeoutExpired, TimeoutError):
        timed_out = True
        process.kill()
        try: process.wait(timeout=0.3)
        except subprocess.TimeoutExpired: pass
    for thread in threads: thread.join(timeout=max(0, min(0.1, deadline-clock())))
    return {'stdout': bytes(output['stdout']), 'stderr': bytes(output['stderr']),
            'stdout_bytes': totals['stdout'], 'stderr_bytes': totals['stderr'],
            'stdout_truncated': totals['stdout'] > caps['stdout'], 'stderr_truncated': totals['stderr'] > caps['stderr'],
            'exit_code': process.returncode, 'timed_out': timed_out,
            'pipes_complete': not any(t.is_alive() for t in threads) and not errors}


def validate(stdout, exit_code, last_message):
    """JSONL 실행 검증은 기존과 동일하게 알 수 없는 이벤트·도구·실패를 거부합니다.

    이번 계약은 JSON role 객체 대신 정확한 고정 한국어 문자열입니다.
    최종 파일/이벤트/요청문장 3자를 일치시키고 완료 turn·사용량도 요구합니다.
    """
    reasons = []; counts = Counter(); items = Counter(); finals = []; usage = {}; started = False; completed = False
    def reject(reason):
        if reason not in reasons: reasons.append(reason)
    if exit_code != 0: reject('EXIT_NONZERO')
    try: text = stdout.decode('utf8', errors='strict')
    except UnicodeError: text = ''; reject('JSONL_INVALID_UTF8')
    if not text.strip(): reject('JSONL_EMPTY')
    allowed = {'thread.started', 'turn.started', 'turn.completed', 'turn.failed', 'error',
               'item.started', 'item.updated', 'item.completed'}
    for line in text.splitlines():
        if not line.strip(): continue
        try:
            event = strict_json(line)
            if not isinstance(event, dict) or type(event.get('type')) is not str: raise ValueError()
        except (ValueError, TypeError): reject('JSONL_INVALID'); continue
        kind = event['type']; counts[kind if kind in allowed else 'unknown'] += 1
        if kind not in allowed: reject('UNKNOWN_EVENT_TYPE')
        if kind in {'turn.failed', 'error'}: reject('CLI_ERROR_EVENT')
        if kind == 'turn.started':
            if started or completed: reject('TURN_SEQUENCE_INVALID')
            started = True
        if kind == 'turn.completed':
            if not started or completed or not finals: reject('TURN_SEQUENCE_INVALID')
            completed = True; raw = event.get('usage')
            if not isinstance(raw, dict): reject('USAGE_MISSING'); continue
            for key in ('input_tokens', 'output_tokens', 'cached_input_tokens'):
                value = raw.get(key)
                if key == 'cached_input_tokens' and value is None: continue
                if type(value) is not int or value < 0: reject('USAGE_INVALID')
                else: usage[key] = value
            if usage.get('cached_input_tokens', 0) > usage.get('input_tokens', 0): reject('USAGE_INVALID')
        if kind.startswith('item.'):
            if not started or completed: reject('TURN_SEQUENCE_INVALID')
            item = event.get('item')
            if not isinstance(item, dict): reject('ITEM_INVALID'); continue
            item_type = item.get('type'); items[item_type if item_type in {'reasoning', 'agent_message'} else 'forbidden'] += 1
            if item_type not in {'reasoning', 'agent_message'}: reject('FORBIDDEN_ITEM')
            if item_type == 'agent_message' and kind == 'item.completed':
                if item.get('phase') not in (None, 'final_answer'): reject('FINAL_PHASE_INVALID')
                if item.get('text') != EXPECTED: reject('EXACT_SENTENCE_MISMATCH')
                finals.append(item.get('text'))
    if counts['thread.started'] != 1 or counts['turn.started'] != 1 or counts['turn.completed'] != 1: reject('TURN_COUNT_INVALID')
    if len(finals) != 1: reject('FINAL_COUNT_INVALID')
    # 마지막 줄바꿈만 CLI 파일 포맷으로 허용하며 공백·다른 문장은 허용하지 않습니다.
    if not isinstance(last_message, str) or last_message.rstrip('\r\n') != EXPECTED: reject('FINAL_FILE_MISMATCH')
    return {'status': 'PASS' if not reasons else 'NOT_VERIFIED', 'reason_codes': reasons,
            'completed_turns': counts['turn.completed'], 'event_counts': dict(counts),
            'item_counts': dict(items), 'usage': usage,
            'final_response': EXPECTED if not reasons else None}
