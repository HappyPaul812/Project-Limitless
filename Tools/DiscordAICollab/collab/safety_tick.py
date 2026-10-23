"""Windows 작업 스케줄러의 고정 진입점: 장부 점검만, 네트워크/AI/종료/명령 실행 없음."""
import json
import sqlite3
import time
from pathlib import Path
from .scheduling import advance, initialize, usage_decision

ROOT = Path(__file__).resolve().parent.parent


def main():
    path = ROOT / 'data/phase2/relay.sqlite'
    if not path.is_file() or path.is_symlink():
        raise SystemExit('안전 점검 DB가 없습니다. 기존 봇 설정부터 확인하세요.')
    db = sqlite3.connect(path, timeout=15, isolation_level=None)
    db.row_factory = sqlite3.Row
    try:
        db.execute('BEGIN IMMEDIATE')
        initialize(db)
        now = time.time()
        row = advance(db, now)
        result = {'schedule': row, 'usage': usage_decision(db, now), 'checked_at': now,
                  'mode': 'SIMULATION_ONLY', 'ai_calls': 0, 'os_shutdown_calls': 0, 'git_mutations': 0}
        db.commit()
    except BaseException:
        db.rollback()
        raise
    finally:
        db.close()
    folder = ROOT / 'data/safety'
    folder.mkdir(exist_ok=True)
    # 프로세스마다 별도 임시 파일을 사용하고 atomic replace로 독자가 부분 JSON을 읽지 않게 합니다.
    import os
    temp = folder / ('tick-' + str(os.getpid()) + '.tmp')
    temp.write_text(json.dumps(result, ensure_ascii=False, indent=2), encoding='utf8')
    temp.replace(folder / 'tick-status.json')
    print('예약 장부 점검 완료 / AI·종료·Git 변경 0')


if __name__ == '__main__':
    main()
