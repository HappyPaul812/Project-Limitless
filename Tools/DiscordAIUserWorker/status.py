"""현재 DB 상태만 읽습니다. migration·인증·네트워크·모델·worker 실행이 없습니다."""
import json
from pathlib import Path
import sqlite3

root = Path(__file__).resolve().parent.parent / 'DiscordAICollab'
with sqlite3.connect((root / 'data/phase2/relay.sqlite').as_uri() + '?mode=ro', uri=True) as db:
    exists = db.execute("SELECT 1 FROM sqlite_master WHERE name='user_worker_jobs'").fetchone() is not None
    result = {'status':'OFFLINE_CONNECTED_EXECUTION_DISABLED' if exists else 'PREPARED_NOT_DEPLOYED','worker_enabled':False,'actual_ai_calls':0,
              'mode':db.execute('SELECT mode FROM operation_mode WHERE singleton=1').fetchone()[0],
              'queue_schema_deployed':exists,
              'jobs':db.execute('SELECT state,count(*) FROM user_worker_jobs GROUP BY state').fetchall() if exists else []}
print(json.dumps(result, indent=2))
