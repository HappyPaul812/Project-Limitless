"""원본 JSONL은 실행 사용자 DPAPI로 암호화하고, 비밀 제거 진단은 같은 자기 프로필에 둡니다."""
import ctypes
from ctypes import wintypes
import hashlib
import json
import re
from pathlib import Path


class Blob(ctypes.Structure):
    _fields_ = [('size', wintypes.DWORD), ('data', ctypes.POINTER(ctypes.c_ubyte))]


def protect_bytes(data):
    # CURRENT_USER DPAPI + UI_FORBIDDEN: 다른 계정으로 옮기거나 대화형 인증을 요구하지 않습니다.
    crypt = ctypes.WinDLL('crypt32', use_last_error=True)
    kernel = ctypes.WinDLL('kernel32', use_last_error=True)
    crypt.CryptProtectData.argtypes = [ctypes.POINTER(Blob), wintypes.LPCWSTR, ctypes.c_void_p,
                                      ctypes.c_void_p, ctypes.c_void_p, wintypes.DWORD, ctypes.POINTER(Blob)]
    crypt.CryptProtectData.restype = wintypes.BOOL
    kernel.LocalFree.argtypes = [ctypes.c_void_p]
    kernel.LocalFree.restype = ctypes.c_void_p
    buffer = ctypes.create_string_buffer(data)
    source = Blob(len(data), ctypes.cast(buffer, ctypes.POINTER(ctypes.c_ubyte)))
    target = Blob()
    try:
        if not crypt.CryptProtectData(ctypes.byref(source), 'LIMITLESS JSONL', None, None, None, 1, ctypes.byref(target)):
            raise OSError('PRIVATE_DIAGNOSTIC_ENCRYPTION_FAILED')
        return ctypes.string_at(target.data, target.size)
    finally:
        ctypes.memset(buffer, 0, len(buffer))
        if target.data:
            ctypes.memset(target.data, 0, target.size)
            kernel.LocalFree(target.data)


SECRET_KEY = re.compile(r'(?i)(authorization|cookie|password|passwd|secret|(?:^|_)token$|access.?token|refresh.?token|id.?token|api.?key|credential)')


def redact_text(text, secrets=()):
    """알려진 환경 비밀과 흔한 토큰·헤더·URL 자격 증명은 지웁니다. 외부 게시용 자료가 아닙니다."""
    for value in sorted((v for v in secrets if isinstance(v, str) and len(v) >= 4), key=len, reverse=True):
        text = text.replace(value, '[REDACTED]')
    text = re.sub(r'(?i)\bBearer\s+[^\s"\'<>]+', 'Bearer [REDACTED]', text)
    text = re.sub(r'\beyJ[A-Za-z0-9_-]+\.[A-Za-z0-9_-]+\.[A-Za-z0-9_-]+', '[REDACTED_JWT]', text)
    text = re.sub(r'\bsk-[A-Za-z0-9_-]+', '[REDACTED_KEY]', text)
    text = re.sub(r'(?i)(https?://)[^/\s:@]+:[^/\s@]+@', r'\1[REDACTED]@', text)
    text = re.sub(r'(?i)((?:access_token|refresh_token|id_token|api_key|password|secret|token)\s*[=:]\s*)[^\s&,;"\']+', r'\1[REDACTED]', text)
    text = re.sub(r'(?im)^\s*(authorization|cookie|set-cookie)\s*:.*$', r'\1: [REDACTED]', text)
    return text


def redact_value(value, secrets=()):
    if isinstance(value, dict):
        return {redact_text(str(k), secrets): '[REDACTED]' if SECRET_KEY.search(str(k)) else redact_value(v, secrets)
                for k, v in value.items()}
    if isinstance(value, list):
        return [redact_value(v, secrets) for v in value]
    return redact_text(value, secrets) if isinstance(value, str) else value


def prepare_archive(profile, request_id):
    if not re.fullmatch('[a-f0-9]{32}', request_id):
        raise ValueError('DIAGNOSTIC_REQUEST_ID_REQUIRED')
    home = Path(profile) / '.codex'
    folder = home / 'limitless-diagnostics' / request_id
    if home.is_symlink() or (home / 'limitless-diagnostics').is_symlink() or folder.is_symlink():
        raise ValueError('DIAGNOSTIC_LINK_REJECTED')
    # 서비스 자신의 이미 보호된 .codex ACL을 상속합니다. ACL/소유자/인증 설정은 수정하지 않습니다.
    folder.mkdir(parents=True, exist_ok=False)
    protect_bytes(b'LIMITLESS_LOCAL_DIAGNOSTIC_PREFLIGHT')
    return folder


def save_execution_log(folder, stdout, stderr, metadata, secrets=(), protector=protect_bytes):
    """원본 바이트를 복호화 가능한 형태로 자기 계정에만 보존합니다. workspace에는 복사하지 않습니다."""
    folder = Path(folder)
    encrypted = protector(stdout)
    (folder / 'stdout.original.jsonl.dpapi').write_bytes(encrypted)
    clean_stderr = redact_text(stderr.decode('utf8', errors='replace'), secrets)
    (folder / 'stderr.redacted.txt').write_text(clean_stderr, encoding='utf8')
    first_error = None
    failures = []
    sanitized = []
    for number, line in enumerate(stdout.decode('utf8', errors='replace').splitlines(), 1):
        try:
            event = json.loads(line)
        except (ValueError, TypeError):
            sanitized.append(json.dumps({'type': 'unparsed_line', 'line': number, 'text': redact_text(line, secrets)}))
            continue
        clean = redact_value(event, secrets)
        sanitized.append(json.dumps(clean, ensure_ascii=False))
        if isinstance(event, dict) and event.get('type') == 'error' and first_error is None:
            first_error = {'line': number, 'event': clean}
        if isinstance(event, dict) and event.get('type') == 'turn.failed':
            failures.append({'line': number, 'event': clean})
    # 이벤트 전체 구조·유형을 남긴 비밀 제거 사본도 자기 프로필에만 저장합니다.
    (folder / 'stdout.redacted.jsonl').write_text('\n'.join(sanitized), encoding='utf8')
    record = {**metadata, 'first_error': first_error, 'turn_failed': failures,
              'original_stdout_sha256': hashlib.sha256(stdout).hexdigest(),
              'original_stdout_bytes': len(stdout), 'raw_stdout_storage': 'CURRENT_USER_DPAPI',
              'plaintext_original_written': False, 'redaction_scope': 'BEST_EFFORT_LOCAL_ONLY_NEVER_AUTO_PUBLISH'}
    (folder / 'execution.json').write_text(json.dumps(redact_value(record, secrets), ensure_ascii=False, indent=2), encoding='utf8')
    # 부모/공용 결과에는 경로와 크기만 반환합니다. 메시지·원본 이벤트는 포함하지 않습니다.
    return {'directory': str(folder), 'raw_stdout_storage': 'CURRENT_USER_DPAPI',
            'original_stdout_bytes': len(stdout), 'private_only': True}
