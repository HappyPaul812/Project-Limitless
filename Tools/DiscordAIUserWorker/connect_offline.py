"""운영 SQLite의 Worker 표만 연결합니다. AI·인증·usage·Discord·Worker 실행은 없습니다."""
import hashlib
import json
from pathlib import Path
import sqlite3
import sys
import uuid
from contextlib import closing

TOOLS = Path(__file__).resolve().parent.parent
sys.path.insert(0, str(TOOLS))
sys.path.insert(0, str(TOOLS / 'DiscordAICollab'))
from DiscordAIUserWorker.durable_queue import initialize

PROTECTED = ('tasks','settings','approvals','operation_mode','operation_checkpoints',
             'control_resume','control_schedules','operation_history','usage','audit','control_usage','control_outbox')


def snapshot(db):
    result = {}
    for table in PROTECTED:
        if not db.execute("SELECT 1 FROM sqlite_master WHERE type='table' AND name=?",(table,)).fetchone():
            result[table] = {'count':None,'sha256':None}
            continue
        rows = [tuple(row) for row in db.execute('SELECT * FROM '+table+' ORDER BY rowid').fetchall()]
        result[table] = {'count':len(rows),'sha256':hashlib.sha256(json.dumps(rows,ensure_ascii=False).encode()).hexdigest()}
    return result


def connect(path, backup_root):
    path, backup_root = Path(path).resolve(), Path(backup_root).resolve()
    backup_root.mkdir(parents=True,exist_ok=True)
    backup = backup_root / ('before-worker-connect-'+uuid.uuid4().hex+'.sqlite')
    with closing(sqlite3.connect(path.as_uri()+'?mode=ro',uri=True)) as source, closing(sqlite3.connect(backup)) as target:
        source.backup(target)
    db = sqlite3.connect(path.as_uri()+'?mode=rw',uri=True,timeout=15,isolation_level=None)
    try:
        db.execute('BEGIN IMMEDIATE')
        if db.execute('SELECT mode FROM operation_mode WHERE singleton=1').fetchone()[0] != 'MANUAL':
            raise ValueError('MANUAL_REQUIRED')
        before = snapshot(db)
        initialize(db)
        after = snapshot(db)
        if before != after:
            raise ValueError('ORIGINAL_ROWS_CHANGED')
        db.commit()
        result = {'status':'OFFLINE_CONNECTED_EXECUTION_DISABLED','database':str(path),'backup':str(backup),
                  'original_rows_preserved':True,'original_table_counts':{k:v['count'] for k,v in before.items()},
                  'worker_jobs':db.execute('SELECT count(*) FROM user_worker_jobs').fetchone()[0],
                  'actual_ai_calls':0,'worker_enabled':False,'auto_start_registered':False,'mode':'MANUAL'}
        return result
    except BaseException:
        db.rollback();raise
    finally:
        db.close()


if __name__ == '__main__':
    root = TOOLS/'DiscordAICollab'
    result = connect(root/'data/phase2/relay.sqlite',root/'data/user-worker-backups')
    (root/'data/user-worker-connection.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps(result,ensure_ascii=False,indent=2))
