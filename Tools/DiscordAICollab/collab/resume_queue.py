"""새 승인을 만들지 않는 읽기 전용 단발 재개 큐. 소비/만료/결과불명 슬롯은 재실행하지 않습니다."""
import hashlib
import json
from pathlib import Path
from .core import Denied
from .intake import safe_root
from .scheduling import gate


def initialize(db):
    db.execute('CREATE TABLE IF NOT EXISTS control_resume(id TEXT PRIMARY KEY,plan TEXT,workspace TEXT,executable TEXT,exe_sha TEXT,state TEXT,result TEXT,created REAL)')


class ResumeQueue:
    def __init__(self,store):
        self.store=store
        with store.tx() as db:initialize(db)

    def defer(self,identity,plan,workspace,executable):
        from .real_roles import validate
        self.store.authorize(identity);validate(plan)
        binary=Path(executable)
        if not binary.is_absolute() or not binary.is_file() or binary.is_symlink():raise Denied('RESUME_OFFICIAL_EXECUTABLE_REQUIRED')
        workspace=safe_root(workspace)
        if any(workspace.iterdir()):raise Denied('RESUME_EMPTY_ISOLATION_REQUIRED')
        exe_sha=hashlib.sha256(binary.read_bytes()).hexdigest()
        with self.store.tx() as db:
            grant_row=db.execute('SELECT value FROM settings WHERE key=?',(plan['slot'],)).fetchone()
            grant=json.loads(grant_row[0]) if grant_row else {}
            if (grant.get('state')!='APPROVED' or grant.get('sha256')!=plan['sha256']
                    or grant.get('role')!=plan['role'] or grant.get('expires',0)<self.store.clock()):
                raise Denied('RESUME_UNCONSUMED_EXACT_APPROVAL_REQUIRED')
            if db.execute('SELECT 1 FROM control_resume WHERE id=?',(plan['sha256'],)).fetchone():return
            db.execute('INSERT INTO control_resume VALUES(?,?,?,?,?,?,?,?)',
                       (plan['sha256'],json.dumps(plan),str(workspace),str(binary),exe_sha,'HELD',None,self.store.clock()))

    def recover(self):
        with self.store.tx() as db:
            db.execute("UPDATE control_resume SET state='NOT_VERIFIED',result='INTERRUPTED_NO_RETRY' WHERE state='RUNNING'")

    def tick(self,identity,runner=None):
        from .real_roles import run_once,validate,discord_text
        self.store.authorize(identity)
        with self.store.tx() as db:
            if gate(db,self.store.clock(),ai=True,duration=90) or self.store.setting(db,'paused')=='1':return None
            if db.execute("SELECT count(*) FROM tasks WHERE state='RUNNING'").fetchone()[0]>=2:return None
            if db.execute("SELECT count(*) FROM control_resume WHERE state='RUNNING'").fetchone()[0]>=1:return None
            rows=db.execute("SELECT * FROM control_resume WHERE state='HELD' ORDER BY created,id").fetchall()
            chosen=None
            for row in rows:
                try:
                    plan=json.loads(row['plan']);validate(plan)
                except (Denied,ValueError,TypeError):
                    db.execute("UPDATE control_resume SET state='NOT_VERIFIED',result='PLAN_INVALID' WHERE id=?",(row['id'],));continue
                grant_row=db.execute('SELECT value FROM settings WHERE key=?',(plan['slot'],)).fetchone()
                grant=json.loads(grant_row[0]) if grant_row else {}
                if (grant.get('state')!='APPROVED' or grant.get('sha256')!=row['id']
                        or grant.get('role')!=plan['role'] or grant.get('expires',0)<self.store.clock()):
                    db.execute("UPDATE control_resume SET state='EXPIRED_OR_CONSUMED' WHERE id=?",(row['id'],));continue
                if self.store.setting(db,'role:'+plan['role'])=='1':continue
                binary=Path(row['executable'])
                if binary.is_symlink() or not binary.is_file() or hashlib.sha256(binary.read_bytes()).hexdigest()!=row['exe_sha']:
                    db.execute("UPDATE control_resume SET state='NOT_VERIFIED',result='EXECUTABLE_CHANGED' WHERE id=?",(row['id'],));continue
                db.execute("UPDATE control_resume SET state='RUNNING' WHERE id=?",(row['id'],))
                chosen=dict(row);break
        if chosen is None:return None
        try:
            result=(runner or run_once)(self.store,identity,json.loads(chosen['plan']),chosen['workspace'],chosen['executable'])
            passed=result.get('status')=='PASS' and result.get('mode')=='REAL_CLI_READONLY'
            text=discord_text(result)
            state='DONE' if passed else 'NOT_VERIFIED'
        except Exception:
            result={'status':'NOT_VERIFIED','reason':'RESUME_ATTEMPT_STOPPED_NO_RETRY'}
            state='NOT_VERIFIED';text='[RESUME / NOT_VERIFIED] 승인된 재개 시도가 중지됐습니다. 실제 추론 시작 여부 미확인·자동 재시도·유료 전환 없음.'
        with self.store.tx() as db:
            db.execute('UPDATE control_resume SET state=?,result=? WHERE id=?',(state,json.dumps(result),chosen['id']))
            db.execute('INSERT OR IGNORE INTO control_outbox VALUES(?,?,?,?)',('resume:'+chosen['id'],text,'PENDING',None))
        return {'id':chosen['id'],'state':state}
