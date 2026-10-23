"""권한·상태·임대·승인·비용을 SQLite 트랜잭션 안에서 함께 처리합니다."""
from __future__ import annotations

import hashlib
import json
import re
import sqlite3
import time
import uuid
from contextlib import contextmanager
from dataclasses import dataclass
from datetime import datetime, timedelta, timezone
from pathlib import Path

KST = timezone(timedelta(hours=9))
ROLES = json.loads((Path(__file__).parent.parent / 'roles.json').read_text(encoding='utf8'))
TERMINAL = {'DONE', 'FAILED', 'CANCELLED', 'NOT_VERIFIED'}
STATES = {'NEW','TRIAGED','RUNNING','REVIEW_PENDING','APPROVAL_PENDING','APPROVED',
          'DONE','BLOCKED','FAILED','CANCELLED','NOT_VERIFIED'}


class Denied(ValueError):
    """권한·계약 위반이며 원문 비밀이나 외부 입력을 오류에 복사하지 않습니다."""


def sha256(path):
    with Path(path).open('rb') as stream:
        return hashlib.file_digest(stream, 'sha256').hexdigest()


def valid_sha(value):
    if not isinstance(value, str) or not re.fullmatch('[0-9a-f]{64}', value):
        raise Denied('SHA256 형식 오류')
    return value


@dataclass(frozen=True)
class Identity:
    user_id: int
    guild_id: int
    channel_id: int
    is_bot: bool = False


class Store:
    def __init__(self, path, owners, guild=1, channels=(1,), clock=time.time):
        self.path = Path(path)
        self.path.parent.mkdir(parents=True, exist_ok=True)
        self.owners = frozenset(owners)
        self.guild, self.channels, self.clock = guild, frozenset(channels), clock
        if not self.owners or any(type(x) is not int or x <= 0 for x in self.owners):
            raise Denied('소유자 숫자 ID 설정 필요')
        with self.tx() as db:
            db.executescript('''
            CREATE TABLE IF NOT EXISTS tasks(
              id TEXT PRIMARY KEY, event_key TEXT UNIQUE NOT NULL, title TEXT NOT NULL,
              role TEXT NOT NULL, requester TEXT NOT NULL, state TEXT NOT NULL,
              payload TEXT NOT NULL, root TEXT NOT NULL, depth INTEGER NOT NULL,
              lease TEXT, lease_until REAL, evidence TEXT, tech TEXT NOT NULL DEFAULT 'NOT_VERIFIED',
              art TEXT NOT NULL DEFAULT 'USER_REVIEW_PENDING', created REAL NOT NULL);
            CREATE TABLE IF NOT EXISTS receipts(key TEXT PRIMARY KEY, result TEXT NOT NULL, fingerprint TEXT NOT NULL);
            CREATE TABLE IF NOT EXISTS audit(id INTEGER PRIMARY KEY, at REAL, kind TEXT, task TEXT, actor TEXT);
            CREATE TABLE IF NOT EXISTS settings(key TEXT PRIMARY KEY, value TEXT NOT NULL);
            CREATE TABLE IF NOT EXISTS approvals(task TEXT, sha TEXT, scope TEXT, decision TEXT,
              actor INTEGER, at REAL, PRIMARY KEY(task,sha,scope));
            CREATE TABLE IF NOT EXISTS usage(id TEXT PRIMARY KEY, task TEXT, category TEXT,
              units INTEGER, input_tokens INTEGER, output_tokens INTEGER, status TEXT,
              day TEXT, month TEXT);
            ''')
            db.execute("INSERT OR IGNORE INTO settings VALUES('paused','0')")
            for role, policy in ROLES.items():
                db.execute('INSERT OR IGNORE INTO settings VALUES(?,?)',
                           ('role:' + role, '1' if policy['mode'] == 'PAUSED' else '0'))
            for key in ('task_limit','day_limit','month_limit'):
                db.execute('INSERT OR IGNORE INTO settings VALUES(?,?)', (key, '0'))

        from .scheduling import initialize
        with self.tx() as db:
            initialize(db)

    @contextmanager
    def tx(self):
        db = sqlite3.connect(self.path, timeout=15, isolation_level=None)
        db.row_factory = sqlite3.Row
        try:
            db.execute('PRAGMA foreign_keys=ON')
            db.execute('BEGIN IMMEDIATE')
            yield db
            db.commit()
        except BaseException:
            db.rollback()
            raise
        finally:
            db.close()

    def audit(self, db, kind, task='', actor='system'):
        # 원문·키·토큰은 장부에 저장하지 않고 식별자와 고정 이벤트만 남깁니다.
        db.execute('INSERT INTO audit(at,kind,task,actor) VALUES(?,?,?,?)',
                   (self.clock(), kind, task, str(actor)))

    def authorize(self, identity):
        if (identity.is_bot or identity.user_id not in self.owners or
                identity.guild_id != self.guild or identity.channel_id not in self.channels):
            with self.tx() as db:
                self.audit(db, 'AUTH_DENIED', actor=identity.user_id)
            raise Denied('허용된 서버·채널의 바울 대표 ID만 실행할 수 있습니다')

    @staticmethod
    def setting(db, key):
        row = db.execute('SELECT value FROM settings WHERE key=?', (key,)).fetchone()
        if row is None:
            raise Denied('설정 없음')
        return row[0]

    @staticmethod
    def task(db, task_id):
        row = db.execute('SELECT * FROM tasks WHERE id=?', (task_id,)).fetchone()
        if row is None:
            raise Denied('작업 없음')
        return dict(row)

    def get(self, task_id):
        with self.tx() as db:
            return self.task(db, task_id)

    def snapshot(self):
        with self.tx() as db:
            return {'paused':self.setting(db,'paused') == '1',
                    'tasks':[dict(r) for r in db.execute('SELECT id,title,role,state,tech,art FROM tasks ORDER BY created')],
                    'usage':[dict(r) for r in db.execute('SELECT category,SUM(units) units FROM usage GROUP BY category')],
                    'roles':{r:self.setting(db,'role:'+r) == '1' for r in ROLES},
                    'limits':{k:int(self.setting(db,k)) for k in ('task_limit','day_limit','month_limit')}}

    def _create(self, db, key, payload, requester='owner', root='', depth=0):
        previous = db.execute('SELECT id FROM tasks WHERE event_key=?', (key,)).fetchone()
        if previous:
            return previous[0]
        from .scheduling import gate
        if gate(db, self.clock(), intake=True):
            raise Denied('SCHEDULE_HOLD: 신규 업무 접수 중단·다음 예약 확인 필요')
        required = ('title','role','issue','base_sha','scope','protected_assets',
                    'inputs','verification','approval_required','artifact')
        if not isinstance(payload, dict) or any(k not in payload for k in required):
            raise Denied('작업 계약 필수 필드 누락')
        if len(json.dumps(payload)) > 20000 or not isinstance(payload['title'],str) or not payload['title'].strip():
            raise Denied('작업 계약 크기·제목 오류')
        if payload['role'] not in ROLES or type(payload['approval_required']) is not bool:
            raise Denied('역할·승인 형식 오류')
        if (not isinstance(payload['scope'],str) or not 1<=len(payload['scope'])<=512 or
            not isinstance(payload['base_sha'],str) or not re.fullmatch('[0-9a-f]{40}',payload['base_sha']) or
            not isinstance(payload['artifact'],str) or
            any(not isinstance(payload[k],list) for k in ('inputs','verification','protected_assets'))):
            raise Denied('작업 범위·기준 SHA·입력 계약 오류')
        if 'artifact_sha' in payload:
            valid_sha(payload['artifact_sha'])
        if self.setting(db, 'role:'+payload['role']) == '1':
            raise Denied('역할 정지됨')
        if depth > 4:
            raise Denied('인계 횟수 초과')
        if ROLES[payload['role']]['mode'] == 'on_demand' and requester != 'owner':
            raise Denied('요청 시에만 실행하는 역할')
        task_id = 'L-' + uuid.uuid4().hex[:16]
        db.execute('INSERT INTO tasks(id,event_key,title,role,requester,state,payload,root,depth,created) '
                   'VALUES(?,?,?,?,?,?,?,?,?,?)',
                   (task_id,key,payload['title'],payload['role'],requester,'NEW',
                    json.dumps(payload,ensure_ascii=False),root or task_id,depth,self.clock()))
        self.audit(db,'TASK_NEW',task_id,requester)
        return task_id

    def command(self, identity, event_key, action, args=None):
        """전송 재시도도 결과를 재사용하며 승인/정지와 receipt를 함께 확정합니다."""
        self.authorize(identity)
        try:
            return self._authorized_command(identity,event_key,action,args)
        except Denied:
            # 실패한 트랜잭션은 먼저 롤백하고 거부 사실만 별도로 보존합니다.
            with self.tx() as db:
                self.audit(db,'COMMAND_DENIED',actor=identity.user_id)
            raise

    def _authorized_command(self,identity,event_key,action,args):
        args = args or {}
        if not isinstance(action,str) or not isinstance(args,dict):
            raise Denied('명령 형식 오류')
        if not isinstance(event_key,str) or not 1 <= len(event_key) <= 160:
            raise Denied('이벤트 키 오류')
        with self.tx() as db:
            key = f'{identity.guild_id}:{identity.user_id}:{event_key}'
            fingerprint = hashlib.sha256(json.dumps([action,args],sort_keys=True).encode()).hexdigest()
            old = db.execute('SELECT result,fingerprint FROM receipts WHERE key=?',(key,)).fetchone()
            if old:
                if old['fingerprint'] != fingerprint:
                    raise Denied('같은 이벤트 키의 다른 요청 거부')
                return json.loads(old[0])
            result = self._command(db, identity, key, action, args)
            db.execute('INSERT INTO receipts VALUES(?,?,?)',(key,json.dumps(result,ensure_ascii=False),fingerprint))
            self.audit(db,'COMMAND_'+action.upper(),str(args.get('task_id','')),identity.user_id)
            return result

    def _command(self, db, identity, key, action, args):
        if action == 'task':
            return {'task_id':self._create(db,key,args)}
        if action in ('pause','resume'):
            role = args.get('role','all')
            if role != 'all' and role not in ROLES:
                raise Denied('역할 오류')
            target = 'paused' if role == 'all' else 'role:'+role
            db.execute('UPDATE settings SET value=? WHERE key=?',('1' if action=='pause' else '0',target))
            # 실행 중인 작업은 재시도하지 않습니다. 외부 실행기의 강제 종료는 아직 없습니다.
            if action == 'pause':
                clause, values = ('',()) if role == 'all' else (' AND role=?',(role,))
                db.execute("UPDATE tasks SET state='NOT_VERIFIED',lease=NULL,lease_until=NULL "
                           "WHERE state='RUNNING'"+clause, values)
            return {'role':role,'paused':action=='pause'}
        if action == 'budget':
            for field in ('task_limit','day_limit','month_limit'):
                value = args.get(field)
                if type(value) is not int or not 0 <= value <= 10000000:
                    raise Denied('비용 한도는 0 이상의 정수 micro-USD입니다')
                db.execute('UPDATE settings SET value=? WHERE key=?',(str(value),field))
            return {'budget':args}
        if action == 'pending':
            return {'tasks':[dict(r) for r in db.execute("SELECT id,role,state,tech,art FROM tasks WHERE state IN ('REVIEW_PENDING','APPROVAL_PENDING','BLOCKED','NOT_VERIFIED')")]}
        if action == 'status':
            return {'paused':self.setting(db,'paused'),'counts':[dict(r) for r in db.execute('SELECT state,COUNT(*) count FROM tasks GROUP BY state')]}
        if action in ('approve','reject'):
            task = self.task(db,args.get('task_id'))
            digest = valid_sha(args.get('sha'))
            scope = args.get('scope')
            payload = json.loads(task['payload'])
            if scope != payload['scope'] or digest != payload.get('artifact_sha'):
                raise Denied('검토한 SHA·범위와 다릅니다')
            if task['state'] != 'APPROVAL_PENDING':
                raise Denied('승인 대기 상태가 아닙니다')
            if action == 'approve' and task['tech'] != 'TECH_PASS':
                raise Denied('기술 검수 미완료')
            evidence = json.loads(task['evidence'] or '{}')
            if action == 'approve':
                if str(evidence.get('mode','')).startswith('MOCK') or evidence.get('mode') in ('CODEX_SUBSCRIPTION_PROBE','REAL_CLI_READONLY'):
                    raise Denied('모의·추론 연결 결과를 실제 미술 승인에 사용할 수 없습니다')
                artifact = Path(payload['artifact'])
                if not artifact.is_file() or artifact.is_symlink() or sha256(artifact) != digest:
                    raise Denied('실제 승인 대상 파일이 없거나 SHA가 변경되었습니다')
            decision = 'ART_APPROVED' if action=='approve' else 'ART_REJECTED'
            db.execute('INSERT OR REPLACE INTO approvals VALUES(?,?,?,?,?,?)',
                       (task['id'],digest,scope,decision,identity.user_id,self.clock()))
            db.execute('UPDATE tasks SET state=?,art=? WHERE id=?',
                       ('APPROVED' if action=='approve' else 'BLOCKED',decision,task['id']))
            return {'state':'APPROVED' if action=='approve' else 'BLOCKED','product_applied':False}
        if action == 'handoff':
            task = self.task(db,args.get('task_id'))
            if task['state'] != 'REVIEW_PENDING':
                raise Denied('기술 보고 이후에만 인계합니다')
            return self._handoff(db,task)
        if action in ('apply','push','merge','shell','publish'):
            raise Denied('파일럿에서 실행할 수 없는 별도 승인 단계입니다')
        raise Denied('허용되지 않은 명령')

    def triage(self, task_id):
        with self.tx() as db:
            task = self.task(db,task_id)
            if task['state'] == 'NEW':
                db.execute("UPDATE tasks SET state='TRIAGED' WHERE id=?",(task_id,))
                self.audit(db,'TRIAGED',task_id)

    def claim(self):
        with self.tx() as db:
            from .scheduling import gate
            if gate(db, self.clock(), duration=300):
                return None
            if datetime.fromtimestamp(self.clock(),KST).weekday() == 6 or self.setting(db,'paused') == '1':
                return None
            if db.execute("SELECT COUNT(*) FROM tasks WHERE state='RUNNING'").fetchone()[0] >= 2:
                return None
            rows = db.execute("SELECT * FROM tasks WHERE state='TRIAGED' ORDER BY created,id").fetchall()
            for row in rows:
                if self.setting(db,'role:'+row['role']) == '1':
                    continue
                token = uuid.uuid4().hex
                db.execute("UPDATE tasks SET state='RUNNING',lease=?,lease_until=? WHERE id=?",
                           (token,self.clock()+300,row['id']))
                self.audit(db,'CLAIM',row['id'],row['role'])
                return self.task(db,row['id'])
            return None

    def recover(self):
        with self.tx() as db:
            rows = db.execute("SELECT id FROM tasks WHERE state='RUNNING' AND lease_until<=?",(self.clock(),)).fetchall()
            for row in rows:
                db.execute("UPDATE tasks SET state='NOT_VERIFIED',lease=NULL,lease_until=NULL WHERE id=?",(row[0],))
                self.audit(db,'LEASE_EXPIRED_NO_RETRY',row[0])
            return len(rows)

    def finish(self, task_id, token, tech, evidence):
        if tech not in ('TECH_PASS','TECH_FAIL','NOT_VERIFIED'):
            raise Denied('기술 결과 오류')
        # 작업자는 미술 승인·DONE 전환을 직접 요청할 수 없습니다.
        if not isinstance(evidence,dict) or len(json.dumps(evidence)) > 20000:
            raise Denied('검증 근거 형식 오류')
        with self.tx() as db:
            task = self.task(db,task_id)
            if task['state'] != 'RUNNING' or token != task['lease'] or self.clock() >= task['lease_until']:
                raise Denied('현재 실행 임대가 아닙니다')
            state = 'REVIEW_PENDING' if tech=='TECH_PASS' else ('FAILED' if tech=='TECH_FAIL' else 'NOT_VERIFIED')
            db.execute('UPDATE tasks SET state=?,tech=?,evidence=?,lease=NULL,lease_until=NULL WHERE id=?',
                       (state,tech,json.dumps(evidence,ensure_ascii=False),task_id))
            self.audit(db,'REPORT_'+tech,task_id,task['role'])

    def _handoff(self, db, task):
        next_role = ROLES[task['role']]['next']
        if next_role == 'owner':
            db.execute("UPDATE tasks SET state='APPROVAL_PENDING' WHERE id=?",(task['id'],))
            self.audit(db,'OWNER_REVIEW_PENDING',task['id'],'PlanningAgent')
            return {'task_id':task['id'],'state':'APPROVAL_PENDING'}
        payload = json.loads(task['payload'])
        payload.update(role=next_role, title=task['title'][:180]+' / 인계')
        child = self._create(db,'handoff:'+task['id'],payload,task['role'],task['root'],task['depth']+1)
        db.execute("UPDATE tasks SET state='DONE' WHERE id=?",(task['id'],))
        self.audit(db,'HANDOFF',task['id'],next_role)
        return {'task_id':child,'state':'NEW'}

    def auto_handoff(self, task_id):
        with self.tx() as db:
            task = self.task(db,task_id)
            # 중복 worker 결과/재시작에서도 같은 후속 ID만 돌려줍니다.
            old = db.execute('SELECT id FROM tasks WHERE event_key=?',('handoff:'+task_id,)).fetchone()
            if old:
                return {'task_id':old[0],'state':self.task(db,old[0])['state']}
            if task['state'] == 'APPROVAL_PENDING':
                return {'task_id':task_id,'state':'APPROVAL_PENDING'}
            if task['state'] != 'REVIEW_PENDING':
                raise Denied('인계할 검수 보고가 없습니다')
            return self._handoff(db,task)

    def replace_artifact(self, identity, task_id, digest):
        self.authorize(identity)
        valid_sha(digest)
        with self.tx() as db:
            task = self.task(db,task_id)
            if task['state'] == 'RUNNING':
                raise Denied('실행 중 산출물 교체 금지')
            payload = json.loads(task['payload'])
            if payload.get('artifact_sha') == digest:
                return
            payload['artifact_sha'] = digest
            db.execute("UPDATE tasks SET payload=?,state='NEW',tech='NOT_VERIFIED',art='USER_REVIEW_PENDING',evidence=NULL WHERE id=?",
                       (json.dumps(payload,ensure_ascii=False),task_id))
            self.audit(db,'SHA_CHANGED_APPROVAL_INVALIDATED',task_id,identity.user_id)

    def approval_valid(self, task_id, path, scope):
        digest = sha256(path)
        with self.tx() as db:
            task = self.task(db,task_id)
            payload = json.loads(task['payload'])
            found = db.execute("SELECT 1 FROM approvals WHERE task=? AND sha=? AND scope=? AND decision='ART_APPROVED'",
                               (task_id,digest,scope)).fetchone()
            return bool(found and task['state']=='APPROVED' and payload.get('artifact_sha')==digest and payload['scope']==scope)

    def reserve(self, task_id, category, units):
        """micro-USD 상한을 선점합니다. 결과불명/실패도 임의로 비용을 환불하지 않습니다."""
        if category not in ('text','image','tts','music','video') or type(units) is not int or units < 0:
            raise Denied('비용 형식 오류')
        now = datetime.fromtimestamp(self.clock(),KST)
        day,month = now.strftime('%Y-%m-%d'), now.strftime('%Y-%m')
        with self.tx() as db:
            task = self.task(db,task_id)
            if (task['state'] != 'RUNNING' or self.clock()>=task['lease_until'] or
                now.weekday()==6 or self.setting(db,'paused') == '1'):
                raise Denied('실행 중 작업만 비용 선점 가능')
            if db.execute('SELECT 1 FROM usage WHERE id=?',(task_id,)).fetchone():
                raise Denied('이미 호출을 선점한 작업입니다')
            for column,value,key in (('task',task_id,'task_limit'),('day',day,'day_limit'),('month',month,'month_limit')):
                # column은 위 상수 목록에서만 나오며 사용자 SQL이 아닙니다.
                used = db.execute(f'SELECT COALESCE(SUM(units),0) FROM usage WHERE {column}=?',(value,)).fetchone()[0]
                limit = int(self.setting(db,key))
                if limit == 0 or used+units > limit:
                    raise Denied('예산 초과 또는 호출 예산 미승인')
            db.execute('INSERT INTO usage VALUES(?,?,?,?,?,?,?,?,?)',
                       (task_id,task_id,category,units,0,0,'RESERVED',day,month))
            self.audit(db,'USAGE_RESERVED',task_id,category)
            return task_id

    def settle(self, reservation, input_tokens=0, output_tokens=0, status='COMPLETED'):
        if status not in ('COMPLETED','QUOTA_EXCEEDED','UNKNOWN','FAILED') or any(type(x) is not int or x<0 for x in (input_tokens,output_tokens)):
            raise Denied('사용량 결과 오류')
        with self.tx() as db:
            row = db.execute('SELECT status FROM usage WHERE id=?',(reservation,)).fetchone()
            if not row or row[0] != 'RESERVED':
                raise Denied('유효한 호출 선점 없음')
            db.execute('UPDATE usage SET input_tokens=?,output_tokens=?,status=? WHERE id=?',
                       (input_tokens,output_tokens,status,reservation))
            if status == 'QUOTA_EXCEEDED':
                db.execute("UPDATE settings SET value='1' WHERE key='paused'")
            self.audit(db,'USAGE_'+status,reservation)
