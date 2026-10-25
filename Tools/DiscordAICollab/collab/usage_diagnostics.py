"""사용량 오류만 서비스 자기 프로필에 보존합니다. 인증/계정 응답 원문은 저장하지 않습니다."""
import json
import os
from pathlib import Path
import re
import stat
import uuid

from .core import Denied

MAX_STDERR = 32768
MAX_RECORD = 24576
MAX_RECORDS = 64
# 자유문을 그대로 남기지 않습니다. 알려진 진단 단어 외 값은 이름/비밀일 수 있어 지웁니다.
WORDS = set(('authentication authorization unauthorized unauthenticated forbidden access permission permissions denied '
             'invalid expired missing required token credentials credential account subscription login refresh '
             'failed failure error internal server client request response method initialize initialized '
             'rate limit limits exceeded usage quota too many requests retry later unavailable timeout timed out '
             'connection connect closed reset refused network transport tls certificate proxy http status '
             'not supported found allowed valid malformed protocol json rpc parse unexpected eof end file '
             'read write process exit terminated killed stderr warning debug info overloaded busy service '
             'the a an is was are has been could cannot unable to for from of in on with and or please '
             'unknown insufficient scope capacity conflict bad gateway upstream resource deadline cancellation '
             'session sandbox setup windows code codex auth rejected').split())
STAGES = {'OFFICIAL_USAGE_READ', 'OFFICIAL_USAGE_AND_APPROVAL', 'SERVICE_READINESS'}


def safe_text(value, limit=2048, secrets=()):
    """메시지의 진단 어휘만 남깁니다. 숫자/주소/경로/식별자/임의 인용문은 보존하지 않습니다."""
    if not isinstance(value, str):
        return '[REDACTED]'
    text = value[:65536]
    for secret in secrets:
        if isinstance(secret, str) and secret:
            text = text.replace(secret, '[REDACTED]')
    text = re.sub(r'(?im)^\s*(authorization|cookie|set-cookie)\s*:.*$', '[REDACTED]', text)
    text = re.sub(r'(?i)\bBearer\s+\S+|\bsk-\S+|\beyJ\S+', '[REDACTED]', text)
    text = re.sub(r'(?i)\b(?:password|secret|token|api_key|access_token|refresh_token|id_token)\s*[=:]\s*\S+', '[REDACTED]', text)
    text = re.sub(r'https?://\S+|(?<!\S)[^\s@]+@[^\s@]+|[A-Za-z]:[\\/]\S+|["\'][^"\']*["\']', '[REDACTED]', text)
    tokens = re.findall(r'[A-Za-z]+|[^\sA-Za-z]+', text)
    clean = ' '.join(word if word.lower() in WORDS else '[REDACTED]' for word in tokens)
    clean = re.sub(r'(?:\[REDACTED\]\s*){2,}', '[REDACTED] ', clean)
    return clean[:limit]


def safe_error(error, secrets=()):
    if not isinstance(error, dict):
        return {'code': None, 'message': '[INVALID_ERROR_OBJECT]'}
    code = error.get('code')
    result = {'code': code if type(code) is int and -(2**31) <= code < 2**31 else None,
              'message': safe_text(error.get('message'), secrets=secrets)}
    # 임의 data 필드명/값에는 계정·개인정보가 있으므로 진단에 필요한 제한된 구조만 허용합니다.
    data = error.get('data')
    if isinstance(data, dict):
        result['data'] = {}
        for key in ('status', 'statusCode', 'httpStatus', 'http_status', 'retry_after', 'retryAfter', 'reason', 'message'):
            if key not in data:
                continue
            value = data[key]
            if key in ('status', 'statusCode', 'httpStatus', 'http_status') and type(value) is int and 100 <= value <= 599:
                result['data'][key] = value
            elif key in ('retry_after', 'retryAfter') and type(value) is int and 0 <= value <= 86400:
                result['data'][key] = value
            else:
                result['data'][key] = safe_text(value, 512, secrets)
        result['data_fields_omitted'] = len(data) - len(result['data'])
    elif data is not None:
        result['data'] = '[REDACTED_UNSTRUCTURED_DATA]'
    return result


def no_link(path):
    """기존 ACL을 바꾸지 않고 symlink와 Windows junction/reparse point를 거부합니다."""
    if path.is_symlink() or (path.exists() and getattr(path.lstat(), 'st_file_attributes', 0) & getattr(stat, 'FILE_ATTRIBUTE_REPARSE_POINT', 0x400)):
        raise Denied('USAGE_DIAGNOSTIC_PATH_HOLD')


class ProtectedUsageLog:
    """검증된 서비스 CODEX_HOME만 받습니다. ACL 상속 외 권한 변경·기존 로그 삭제는 하지 않습니다."""
    def __init__(self, home):
        self.home = Path(home)
        self.file = None

    def prepare(self):
        profile = Path(os.environ.get('USERPROFILE', ''))
        if (not profile.is_absolute() or not self.home.is_absolute()
                or self.home != profile / '.codex' or not self.home.is_dir()):
            raise Denied('USAGE_DIAGNOSTIC_PATH_HOLD')
        for p in [profile, self.home]:
            no_link(p)
        folder = self.home / 'limitless-usage-diagnostics'
        no_link(folder)
        folder.mkdir(exist_ok=True)
        if len(list(folder.iterdir())) >= MAX_RECORDS:
            raise Denied('USAGE_DIAGNOSTIC_CAPACITY_HOLD')
        self.file = folder / ('usage-' + uuid.uuid4().hex + '.json')
        # 새 파일만 생성합니다. 기록이 불가능하면 사용량 subprocess를 실행하지 않습니다.
        with self.file.open('x', encoding='utf8') as f:
            f.write('{"stage":"PREFLIGHT","status":"INCOMPLETE"}')

    def write(self, record):
        if self.file is None:
            raise Denied('USAGE_DIAGNOSTIC_WRITE_HOLD')
        for p in [self.home.parent, self.home, self.file.parent, self.file]:
            no_link(p)
        raw = json.dumps(record, ensure_ascii=True, allow_nan=False).encode('ascii')
        if len(raw) > MAX_RECORD:
            raise Denied('USAGE_DIAGNOSTIC_SIZE_HOLD')
        with self.file.open('wb') as f:
            f.write(raw)
