"""기존 서비스 로그만 검사합니다. CLI·네트워크·비밀 저장소·예약 작업을 호출하지 않습니다."""
import json
import re
from datetime import datetime, timezone
from pathlib import Path
from .failure_evidence import classify

ROOT = Path(__file__).resolve().parent.parent


def inspect():
    summary = json.loads((ROOT / 'data/service-batch-once-20261024/batch-summary.json').read_text(encoding='utf-8-sig'))
    start = summary['result']['official_usage_before']['observed']
    end = start + summary['result']['elapsed_seconds'] + 30
    # 인증·키링·설정·상태DB는 열지 않고 기존 텍스트 진단로그만 검사합니다.
    profile = Path('C:/Users/LIMITLESSRelaySvc/.codex')
    report = {'status': 'NOT_VERIFIED', 'actual_ai_calls': 0, 'raw_messages_logged': False,
              'window_utc': [datetime.fromtimestamp(t, timezone.utc).isoformat() for t in (start - 10, end)],
              'files': [], 'observations': []}
    try:
        files = list((profile / 'log').glob('*.log'))
        for path in files:
            report['files'].append({'path': str(path), 'bytes': path.stat().st_size})
            # 시각이 확인된 같은 실행 구간만 검사하며 자유 텍스트는 반환하지 않습니다.
            with path.open(encoding='utf8', errors='replace') as source:
                for number, line in enumerate(source, 1):
                    match = re.search(r'\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(?:\.\d+)?(?:Z|[+-]\d\d:\d\d)', line)
                    if not match:
                        continue
                    at = datetime.fromisoformat(match[0].replace('Z', '+00:00')).timestamp()
                    if not start - 10 <= at <= end:
                        continue
                    categories = classify(line)
                    if categories != ['UNKNOWN']:
                        report['observations'].append({'file': path.name, 'line': number,
                                                       'at': match[0], 'categories': categories})
        report['reason'] = 'SAFE_CLASSIFICATIONS_ONLY_REVIEW_REQUIRED' if report['observations'] else 'NO_MATCHING_EXISTING_LOG_EVIDENCE'
    except PermissionError:
        report['reason'] = 'SERVICE_LOG_ACCESS_DENIED_NO_ACL_CHANGE'
    return report


def main():
    report = inspect()
    output = ROOT / 'data/service-batch-once-20261024/offline-log-diagnosis.json'
    output.write_text(json.dumps(report, indent=2), encoding='utf8')
    print(json.dumps(report, indent=2))


if __name__ == '__main__':
    main()
