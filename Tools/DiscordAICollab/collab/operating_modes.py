"""운영 모드는 실행 승인을 만들지 않습니다. 체크포인트는 안전 경계만 보존합니다."""
import json
from .core import Denied

SCHEMA = """
CREATE TABLE IF NOT EXISTS operation_mode(singleton INTEGER PRIMARY KEY CHECK(singleton=1),mode TEXT,updated REAL);
INSERT OR IGNORE INTO operation_mode VALUES(1,'MANUAL',0);
CREATE TABLE IF NOT EXISTS operation_scopes(schedule TEXT PRIMARY KEY,scope TEXT,approval TEXT);
CREATE TABLE IF NOT EXISTS operation_runtime(singleton INTEGER PRIMARY KEY CHECK(singleton=1),state TEXT,recheck REAL,updated REAL);
CREATE TABLE IF NOT EXISTS operation_history(id INTEGER PRIMARY KEY,mode TEXT,reason TEXT,created REAL);
CREATE TABLE IF NOT EXISTS operation_checkpoints(task TEXT PRIMARY KEY,role TEXT,scope TEXT,base_sha TEXT,stage TEXT,remaining TEXT,failure TEXT,updated REAL);
"""
def initialize(db):
    for sql in SCHEMA.split(';'):
        if sql.strip():db.execute(sql)

def mode(db):return db.execute('SELECT mode FROM operation_mode WHERE singleton=1').fetchone()[0]

def checkpoint(db,now,reason):
    # RUNNING 추론을 강제 종료하거나 소비된 호출을 재발행하지 않습니다.
    for row in db.execute("SELECT * FROM tasks WHERE state NOT IN ('DONE','REJECTED')").fetchall():
        payload=json.loads(row['payload'])
        db.execute('INSERT OR REPLACE INTO operation_checkpoints VALUES(?,?,?,?,?,?,?,?)',
            (row['id'],row['role'],str(payload.get('scope','')),str(payload.get('base_sha','')),row['state'],
             '기존 승인·미소비 호출만 재검증; 진행중 호출은 종료 결과 보존',reason,now))

    if db.execute("SELECT 1 FROM sqlite_master WHERE name='control_resume'").fetchone():
        for row in db.execute("SELECT * FROM control_resume WHERE state='HELD'").fetchall():
            try:plan=json.loads(row['plan'])
            except (ValueError,TypeError):continue
            db.execute('INSERT OR REPLACE INTO operation_checkpoints VALUES(?,?,?,?,?,?,?,?)',
                (row['id'],plan.get('role','UNKNOWN'),plan.get('slot',''),plan.get('source_refs',{}).get('main',''),
                 'HELD','유효한 미소비 승인·최신 정본·사용량 재검증',reason,now))

def change(db,now,target):
    if target not in ('MANUAL','AUTO_RESUME','SAFE_SHUTDOWN'):raise Denied('모드는 MANUAL / AUTO_RESUME / SAFE_SHUTDOWN입니다')
    old=mode(db)
    active=db.execute("SELECT 1 FROM control_schedules WHERE state NOT IN ('CANCELLED','SUPERSEDED') LIMIT 1").fetchone()
    queue_running=db.execute("SELECT 1 FROM sqlite_master WHERE name='control_resume'").fetchone() and db.execute("SELECT 1 FROM control_resume WHERE state='RUNNING'").fetchone()
    if old!=target and (queue_running or db.execute("SELECT 1 FROM tasks WHERE state='RUNNING'").fetchone() or (target=='AUTO_RESUME' and active)):
        raise Denied('실행 중 업무 또는 마감 예약 충돌: 먼저 안전 마감·예약 취소가 필요합니다')
    checkpoint(db,now,'MODE_CHANGE')
    db.execute('UPDATE operation_mode SET mode=?,updated=? WHERE singleton=1',(target,now))
    db.execute('INSERT INTO operation_history(mode,reason,created) VALUES(?,?,?)',(target,'OWNER_COMMAND',now))
    return {'operation_mode':target,'note':'기존 승인 범위·만료 유지 / 추가 호출 승인·Git 통합·PC 종료 승인 아님'}

def observe(db,now,usage):
    current=mode(db)
    state='READY_FOR_APPROVED_WORK' if usage['status']=='AVAILABLE' else 'WAITING_FOR_QUOTA_VERIFICATION'
    db.execute('INSERT OR REPLACE INTO operation_runtime VALUES(1,?,?,?)',(state,usage.get('recheck_at'),now))
    if current=='AUTO_RESUME' and usage['status']!='AVAILABLE':
        checkpoint(db,now,'WAITING_FOR_QUOTA_VERIFICATION' if usage['status']!='EXHAUSTED' else 'QUOTA_EXHAUSTED')
    return current
