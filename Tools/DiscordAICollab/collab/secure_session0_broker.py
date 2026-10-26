"""서비스 내부 단발 중개기. 파일을 실행 명령이나 관리자 승인으로 해석하지 않습니다.

Peer는 네이티브 pipe gateway가 Windows 토큰에서 얻습니다. JSON의 sid/admin 필드는
절대 Peer로 사용하지 않습니다. 이 모듈은 격리 후보이며 운영 등록 기능이 없습니다.
"""
from dataclasses import dataclass
import json
import math
import re
import sqlite3
import threading

from .core import Denied
from .mobile_planning_buttons import contract


@dataclass(frozen=True)
class Peer:
    sid: str
    elevated: bool
    administrator: bool
    local: bool
    integrity: int


class Broker:
    """새 서비스 전용 DB만 주입합니다. 원본 업무 SQLite와는 별개의 소비 장부입니다."""
    def __init__(self, path, *, bot_sid, admin_sid, service_sid, utc, monotonic, context):
        self.path, self.bot_sid, self.admin_sid, self.service_sid = str(path), bot_sid, admin_sid, service_sid
        self.utc, self.monotonic, self.context = utc, monotonic, context
        self.lock = threading.RLock()
        with self.db() as db:
            db.executescript('''
              CREATE TABLE IF NOT EXISTS permits(request TEXT PRIMARY KEY,task_sha TEXT,expires REAL,state TEXT);
              CREATE TABLE IF NOT EXISTS calls(request TEXT PRIMARY KEY,ticket_sha TEXT,state TEXT,result TEXT);
              CREATE TABLE IF NOT EXISTS control(id INTEGER PRIMARY KEY CHECK(id=1),spent INTEGER NOT NULL);
              INSERT OR IGNORE INTO control VALUES(1,0);
            ''')
            # 프로세스가 재시작돼도 소비를 복구하거나 불명 업무를 다시 시작하지 않습니다.
            db.execute("UPDATE calls SET state='HOLD' WHERE state='RUNNING'")
            db.execute("UPDATE permits SET state='REVOKED' WHERE state='ARMED'")

    def db(self):
        class Connection:
            def __enter__(owner):
                owner.db = sqlite3.connect(self.path, isolation_level=None, timeout=1)
                owner.db.row_factory = sqlite3.Row
                owner.db.execute('PRAGMA synchronous=FULL')
                owner.db.execute('BEGIN IMMEDIATE')
                return owner.db
            def __exit__(owner, kind, value, trace):
                try: owner.db.rollback() if kind else owner.db.commit()
                finally: owner.db.close()
        return Connection()

    def _service(self):
        expected = {'sid': self.service_sid, 'session': 0, 'logon': 4, 'admin': False,
                    'protected_install': True, 'protected_state': True, 'fixed_task': True,
                    'manual': True, 'approved_package': True}
        if (self.context != expected or any(type(self.context[k]) is not type(v) for k, v in expected.items())
                or not self.service_sid or self.service_sid in {self.bot_sid, self.admin_sid}
                or self.bot_sid == self.admin_sid):
            raise Denied('BROKER_SERVICE_BOUNDARY_HOLD')

    def _peer(self, peer, admin=False):
        if type(peer) is not Peer or not peer.local: raise Denied('BROKER_LOCAL_PEER_REQUIRED')
        if admin:
            if (peer.sid != self.admin_sid or peer.elevated is not True or peer.administrator is not True
                    or type(peer.integrity) is not int or peer.integrity < 12288):
                raise Denied('BROKER_ELEVATED_ADMIN_REQUIRED')
        elif peer.sid != self.bot_sid or peer.elevated or peer.administrator:
            raise Denied('BROKER_LIMITED_BOT_REQUIRED')

    def arm(self, peer, value):
        self._service(); self._peer(peer, admin=True)
        if (type(value) is not dict or set(value) != {'request_id', 'task_sha256', 'expires', 'policy'}
                or not re.fullmatch('[a-f0-9]{32}', value.get('request_id', ''))
                or not re.fullmatch('[a-f0-9]{64}', value.get('task_sha256', ''))
                or contract.digest(value.get('policy')) != contract.digest(contract.POLICY)
                or type(value.get('expires')) not in (int, float) or not math.isfinite(value['expires'])
                or not self.utc() < value['expires'] <= self.utc() + 600):
            raise Denied('BROKER_ARM_SCHEMA_HOLD')
        with self.lock, self.db() as db:
            if db.execute('SELECT spent FROM control').fetchone()[0] or db.execute('SELECT COUNT(*) FROM permits').fetchone()[0]:
                raise Denied('BROKER_ONE_SHOT_ALREADY_ARMED')
            db.execute('INSERT INTO permits VALUES(?,?,?,?)', (value['request_id'], value['task_sha256'], value['expires'], 'ARMED'))
        return {'status': 'ARMED', 'request_id': value['request_id']}

    def cancel(self, peer, request):
        self._service(); self._peer(peer)
        with self.lock, self.db() as db:
            db.execute("UPDATE permits SET state='CANCELLED' WHERE request=?", (request,))
            db.execute("UPDATE calls SET state='CANCELLED' WHERE request=? AND state='RUNNING'", (request,))
        return {'status': 'CANCELLED', 'request_id': request}

    def _cancelled(self, request):
        with self.db() as db:
            row = db.execute('SELECT state FROM permits WHERE request=?', (request,)).fetchone()
            return not row or row[0] != 'CONSUMED'

    def submit(self, peer, ticket, worker):
        """worker.start는 Job에 할당한 비동기 handle만 반환해야 합니다. 임의 명령은 받지 않습니다."""
        self._service(); self._peer(peer)
        start = self.monotonic()
        if type(ticket) is not dict: raise Denied('BROKER_TICKET_SCHEMA_HOLD')
        with self.lock, self.db() as db:
            arm = db.execute('SELECT * FROM permits WHERE request=?', (ticket.get('request_id'),)).fetchone()
            if not arm or arm['state'] != 'ARMED' or arm['expires'] <= self.utc(): raise Denied('BROKER_PERMIT_HOLD')
            contract.validate_ticket(ticket, {'request_id': arm['request'], 'task_sha256': arm['task_sha'],
                'ticket_sha256': ticket.get('ticket_sha256'), 'policy': contract.POLICY}, self.utc())
            if db.execute('SELECT spent FROM control').fetchone()[0]: raise Denied('BROKER_ALREADY_CONSUMED')
            # durable consume는 사용량 조회·프로세스 생성보다 먼저 commit합니다.
            db.execute('UPDATE control SET spent=1 WHERE id=1')
            db.execute("UPDATE permits SET state='CONSUMED' WHERE request=?", (arm['request'],))
            db.execute('INSERT INTO calls VALUES(?,?,?,NULL)', (arm['request'], ticket['ticket_sha256'], 'RUNNING'))
        request = ticket['request_id']
        deadline = start + min(90.0, ticket['deadline'] - self.utc())
        result = contract.project_child({}, ticket, self.utc())
        handle = None
        try:
            if deadline - self.monotonic() <= 5: raise Denied('BROKER_BUDGET_HOLD')
            if self._cancelled(request): raise Denied('BROKER_CANCELLED')
            # 준비 시간도 차감하며 마지막 2초는 강제 종료·확인에 예약합니다.
            handle = worker.start(ticket, deadline - 2)
            while True:
                if self._cancelled(request): raise Denied('BROKER_CANCELLED')
                if self.monotonic() >= deadline - 2: raise Denied('BROKER_DEADLINE_HOLD')
                child = handle.poll(min(0.05, deadline - 2 - self.monotonic()))
                if child is not None:
                    if len(json.dumps(child, ensure_ascii=False).encode()) > 32768: raise Denied('BROKER_OUTPUT_SIZE_HOLD')
                    result = contract.project_child(child, ticket, self.utc())
                    break
        except Exception:
            result = contract.project_child({}, ticket, self.utc())
        finally:
            # close는 성공/실패 모두 Job 트리를 종료하고 bounded active0 증거를 반환해야 합니다.
            clean = False
            if handle is not None:
                try: clean = handle.close(deadline) is True
                except Exception: pass
            if not clean or self.monotonic() > deadline or self._cancelled(request):
                result = contract.project_child({}, ticket, self.utc())
            with self.lock, self.db() as db:
                db.execute('UPDATE calls SET state=?,result=? WHERE request=?', (result['status'], json.dumps(result), request))
                db.execute("UPDATE permits SET state='DISABLED' WHERE request=? AND state='CONSUMED'", (request,))
        return result

    def result(self, peer, request):
        self._service(); self._peer(peer)
        with self.db() as db:
            row = db.execute('SELECT * FROM calls WHERE request=?', (request,)).fetchone()
            if not row or not row['result']: return {'status': 'HOLD', 'request_id': request}
            return contract.strict(row['result'])


def dispatch(broker, peer, raw, worker):
    """네이티브 gateway 전용 고정 schema. JSON에는 실행 경로나 권한값이 없습니다."""
    try:
        if type(raw) is not bytes or len(raw) > 16384: raise Denied('BROKER_FRAME_HOLD')
        message = contract.strict(raw.decode('utf8'))
        if type(message) is not dict: raise Denied('BROKER_FRAME_HOLD')
        op = message.get('op')
        key = {'arm': 'permit', 'submit': 'ticket', 'cancel': 'request_id', 'result': 'request_id'}.get(op)
        if key is None or set(message) != {'op', key}: raise Denied('BROKER_COMMAND_HOLD')
        if op == 'arm': return broker.arm(peer, message[key])
        if op == 'submit': return broker.submit(peer, message[key], worker)
        if not re.fullmatch('[a-f0-9]{32}', message[key]): raise Denied('BROKER_REQUEST_ID_HOLD')
        return getattr(broker, op)(peer, message[key])
    except Exception:
        return {'status': 'HOLD', 'reason': 'BROKER_REQUEST_REJECTED_NO_RETRY'}
