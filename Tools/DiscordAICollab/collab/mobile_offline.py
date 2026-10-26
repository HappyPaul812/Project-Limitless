"""모바일→승인 큐→4역할→보고의 격리 리허설. 운영 bot/실행기에 연결하지 않습니다.

Store 권한/receipt/lease, 기존 역할 표, JSONL 검증기, 체크포인트를 재사용합니다.
실제 CLI/Discord 콜백을 받지 않으며 새 합성 DB에서만 생성할 수 있습니다.
"""
import json
import sqlite3
import uuid
from pathlib import Path

from .core import Store, Denied
from .real_roles import ROLE_FILES, FOUR_SLOTS, digest
from .output_parser import parse_result
from .operating_modes import mode, checkpoint
from .scheduling import gate

ORDER = tuple(FOUR_SLOTS)  # 기존 Planning→NewArt→Renewal→Codex 순서
TARGET = {ORDER[0]: ','.join(ORDER[1:3]), ORDER[1]: ORDER[3],
          ORDER[2]: ORDER[3], ORDER[3]: ORDER[0]}
SCHEMA = (
    'CREATE TABLE mobile_meta(id TEXT PRIMARY KEY)',
    'CREATE TABLE mobile_grants(task TEXT PRIMARY KEY,sha TEXT,expires REAL,state TEXT)',
    'CREATE TABLE mobile_reports(task TEXT PRIMARY KEY,state TEXT,message TEXT)',
    'CREATE TABLE mobile_links(parent TEXT,role TEXT,task TEXT UNIQUE,PRIMARY KEY(parent,role))',
)


class FakeExecutor:
    """실행 함수 주입 대신 고정 합성 이벤트만 만듭니다. timeout도 대기 없이 합성합니다."""
    def __init__(self):
        self.calls = []
        self.failure = None

    def execute(self, plan):
        self.calls.append(plan['task_id'])
        if self.failure == 'timeout':
            raise TimeoutError()
        if self.failure == 'exception':
            raise RuntimeError('synthetic')
        answer = {'role': plan['role'], 'summary': '합성 오프라인 검토 결과입니다. 미술 승인 대기를 유지합니다.',
                  'next_review': TARGET[plan['role']]}
        if self.failure == 'route':
            answer['next_review'] = 'UnknownAgent'
        text = json.dumps(answer, ensure_ascii=False)
        events = [{'type': 'thread.started', 'thread_id': 'OFFLINE'}, {'type': 'turn.started'},
                  {'type': 'item.completed', 'item': {'type': 'agent_message', 'text': text}},
                  {'type': 'turn.completed', 'usage': {'input_tokens': 0, 'output_tokens': 0}}]
        if self.failure == 'tool':
            events.insert(2, {'type': 'item.completed', 'item': {'type': 'command_execution'}})
        if self.failure == 'error':
            events.append({'type': 'error', 'message': 'synthetic'})
        raw = '\n'.join(json.dumps(x, ensure_ascii=False) for x in events)
        return raw if self.failure != 'invalid' else '{', 1 if self.failure == 'exit' else 0, text


class FakeDiscord:
    """메모리상 보고 수신함. 실제 SDK/토큰/네트워크를 사용하지 않습니다."""
    def __init__(self):
        self.messages = {}
        self.failure = None

    def send(self, key, text):
        if self.failure == 'before':
            raise RuntimeError('synthetic')
        self.messages.setdefault(key, text)
        if self.failure == 'after':
            raise RuntimeError('synthetic ambiguous delivery')
        return 'FAKE-' + key


class OfflineMobileRelay:
    @classmethod
    def create(cls, root, owners, *, clock, guild=1, channels=(1,)):
        root = Path(root).resolve()
        # 기존 DB/슬롯을 열기 전에 거부합니다. 운영 Store를 주입할 수 없습니다.
        if root.exists():
            raise Denied('NEW_EMPTY_OFFLINE_DIRECTORY_REQUIRED')
        root.mkdir(parents=True)
        store = Store(root / 'mobile-offline.sqlite', owners, guild, channels, clock)
        identity = uuid.uuid4().hex
        with store.tx() as db:
            for sql in SCHEMA:
                db.execute(sql)
            db.execute('INSERT INTO mobile_meta VALUES(?)', (identity,))
        (root / 'OFFLINE_ONLY.json').write_text(json.dumps({'id': identity, 'owners': sorted(owners),
                                                          'guild': guild, 'channels': sorted(channels)}), encoding='utf8')
        return cls(store)

    @classmethod
    def reopen(cls, root, owners, *, clock, guild=1, channels=(1,)):
        root = Path(root).resolve()
        marker = json.loads((root / 'OFFLINE_ONLY.json').read_text(encoding='utf8'))
        if (marker['owners'] != sorted(owners) or marker['guild'] != guild
                or marker['channels'] != sorted(channels)):
            raise Denied('OFFLINE_IDENTITY_CONFIGURATION_CHANGED')
        path = root / 'mobile-offline.sqlite'
        # 재시작은 이미 합성용으로 생성된 DB만 허용하고 운영 DB를 초기화하지 않습니다.
        with sqlite3.connect(path.as_uri() + '?mode=ro', uri=True) as db:
            row = db.execute('SELECT id FROM mobile_meta').fetchone()
            if not row or row[0] != marker['id']:
                raise Denied('OFFLINE_DATABASE_BINDING_REQUIRED')
        return cls(Store(path, owners, guild, channels, clock))

    def __init__(self, store):
        # create/reopen 이외의 운영 Store 주입도 차단합니다.
        with store.tx() as db:
            if not db.execute("SELECT 1 FROM sqlite_master WHERE name='mobile_meta'").fetchone():
                raise Denied('OFFLINE_DATABASE_REQUIRED')
        self.store = store
        self.executor = FakeExecutor()
        self.discord = FakeDiscord()
        self.fake_usage_available = True

    def _guard(self, db, role):
        if (mode(db) != 'MANUAL' or self.store.setting(db, 'paused') != '0'
                or self.store.setting(db, 'role:' + role) != '0'
                or gate(db, self.store.clock(), duration=90)):
            raise Denied('OFFLINE_POLICY_HOLD')
        # 공식 사용량으로 위장하지 않는 합성 입력입니다. 실제 usage gate 변경은 없습니다.
        if not self.fake_usage_available:
            raise Denied('FAKE_USAGE_HOLD')

    def plan(self, task_id):
        return self._plan(self.store.get(task_id))

    @staticmethod
    def _plan(task):
        payload = json.loads(task['payload'])
        return {'task_id': task['id'], 'role': task['role'], 'contract': payload,
                'scope': payload['scope'], 'base_sha': payload['base_sha'],
                'role_document': ROLE_FILES[task['role']], 'mode': 'OFFLINE_ONLY',
                'max_fake_calls': 1, 'timeout_seconds': 90, 'application_retries': 0,
                'upstream': payload.get('upstream', {})}

    def mobile(self, identity, event, action, args):
        """SDK interaction ID에 대응하는 receipt입니다. 기존 미술 승인과 구분합니다."""
        self.store.authorize(identity)
        if action in ('status', 'pending', 'pause', 'resume'):
            return self.store.command(identity, event, action, args)
        if action == 'task':
            if (args.get('role') != ORDER[0] or args.get('offline_only') is not True
                    or args.get('scope') != 'OFFLINE_TEXT_REVIEW' or args.get('approval_required') is not True):
                raise Denied('OFFLINE_PLANNING_CONTRACT_REQUIRED')
            return self.store.command(identity, event, action, args)
        if action != 'approve-execution':
            raise Denied('OFFLINE_COMMAND_NOT_ALLOWED')
        if not isinstance(event, str) or not 1 <= len(event) <= 160:
            raise Denied('EVENT_INVALID')
        key = f'{identity.guild_id}:{identity.user_id}:{event}'
        fingerprint = digest([action, args])
        with self.store.tx() as db:
            old = db.execute('SELECT result,fingerprint FROM receipts WHERE key=?', (key,)).fetchone()
            if old:
                if old['fingerprint'] != fingerprint:
                    raise Denied('EVENT_CHANGED')
                return json.loads(old['result'])
            task = self.store.task(db, args['task_id'])
            plan = self._plan(task)
            self._guard(db, task['role'])
            if (task['state'] != 'NEW' or args.get('sha256') != digest(plan)
                    or type(args.get('expires')) not in (int, float)
                    or not self.store.clock() < args['expires'] <= self.store.clock() + 300):
                raise Denied('EXACT_FRESH_EXECUTION_APPROVAL_REQUIRED')
            db.execute('INSERT INTO mobile_grants VALUES(?,?,?,?)',
                       (task['id'], digest(plan), args['expires'], 'APPROVED'))
            db.execute("UPDATE tasks SET state='TRIAGED' WHERE id=?", (task['id'],))
            result = {'task_id': task['id'], 'state': 'OFFLINE_APPROVED', 'sha256': digest(plan)}
            db.execute('INSERT INTO receipts VALUES(?,?,?)', (key, json.dumps(result), fingerprint))
            self.store.audit(db, 'OFFLINE_EXEC_APPROVED', task['id'], identity.user_id)
            return result

    def _hold(self, task_id, reason):
        with self.store.tx() as db:
            db.execute("UPDATE tasks SET state='NOT_VERIFIED',tech='NOT_VERIFIED',lease=NULL,lease_until=NULL WHERE id=?", (task_id,))
            checkpoint(db, self.store.clock(), reason)
            db.execute('INSERT OR IGNORE INTO mobile_reports VALUES(?,?,?)', (task_id, 'PENDING', None))
            self.store.audit(db, 'OFFLINE_HOLD_' + reason, task_id)
        return {'status': 'HOLD', 'reason': reason}

    def step(self, identity, task_id):
        self.store.authorize(identity)
        if type(self.executor) is not FakeExecutor or type(self.discord) is not FakeDiscord:
            raise Denied('FAKE_ONLY_NO_LIVE_CALLBACKS')
        plan = self.plan(task_id)
        try:
            with self.store.tx() as db:
                task = self.store.task(db, task_id)
                plan = self._plan(task)
                grant = db.execute('SELECT * FROM mobile_grants WHERE task=?', (task_id,)).fetchone()
                if grant and grant['state'] == 'CONSUMED':
                    return {'status': 'ALREADY_CONSUMED_NO_RETRY', 'task_id': task_id}
                self._guard(db, task['role'])
                payload = plan['contract']
                if payload.get('offline_only') is not True or payload['scope'] != 'OFFLINE_TEXT_REVIEW':
                    raise Denied('OFFLINE_CONTRACT_CHANGED')
                if task['role'] != ORDER[0]:
                    parents = (ORDER[0],) if task['role'] != ORDER[3] else ORDER[1:3]
                    upstream = {}
                    for role in parents:
                        parent = db.execute('SELECT * FROM tasks WHERE root=? AND role=?', (task['root'], role)).fetchone()
                        sent = db.execute("SELECT 1 FROM mobile_reports WHERE task=? AND state='SENT'", (parent['id'],)).fetchone() if parent else None
                        if not parent or parent['state'] != 'REVIEW_PENDING' or parent['tech'] != 'TECH_PASS' or not sent:
                            raise Denied('UPSTREAM_NOT_VERIFIED')
                        evidence = json.loads(parent['evidence'])
                        upstream[role] = evidence['answer']
                        if digest(evidence['answer']) != evidence['answer_sha256']:
                            raise Denied('UPSTREAM_CHANGED')
                    if upstream != plan['upstream'] or digest(upstream) != payload.get('upstream_sha256'):
                        raise Denied('UPSTREAM_PACKET_CHANGED')
                # 소비·만료·계약 변경 시 승인을 재발급하지 않습니다.
                if (not grant or task['state'] != 'TRIAGED' or grant['state'] != 'APPROVED'
                        or grant['sha'] != digest(plan) or grant['expires'] <= self.store.clock()):
                    raise Denied('UNCONSUMED_EXACT_GRANT_REQUIRED')
                if db.execute("SELECT 1 FROM tasks WHERE state='RUNNING'").fetchone():
                    raise Denied('SINGLE_FAKE_WORKER_LIMIT')
                if db.execute('SELECT COUNT(*) FROM mobile_grants WHERE state=\'CONSUMED\' AND task IN (SELECT id FROM tasks WHERE root=?)', (task['root'],)).fetchone()[0] >= 4:
                    raise Denied('FOUR_ROLE_LIMIT')
                token = uuid.uuid4().hex
                db.execute("UPDATE mobile_grants SET state='CONSUMED' WHERE task=?", (task_id,))
                db.execute("UPDATE tasks SET state='RUNNING',lease=?,lease_until=? WHERE id=?", (token, self.store.clock() + 90, task_id))
            raw, code, final = self.executor.execute(plan)
            if len(raw.encode()) > 65536 or len(final.encode()) > 8192:
                return self._hold(task_id, 'OUTPUT_LIMIT')
            result = parse_result(raw, code, plan['role'], last_message=final, require_last_message=True)
            if result['status'] != 'PASS' or result['answer']['next_review'] != TARGET[plan['role']]:
                return self._hold(task_id, 'RESULT_REJECTED')
            with self.store.tx() as db:
                self._guard(db, plan['role'])
                current = self.store.task(db, task_id)
                if digest(self._plan(current)) != digest(plan):
                    raise Denied('CONTRACT_CHANGED_DURING_EXECUTION')
            evidence = {'mode': 'MOCK', 'test_only': True, 'status': 'OFFLINE_PASS',
                        'answer': result['answer'], 'answer_sha256': digest(result['answer']), 'upstream': plan['upstream']}
            self.store.finish(task_id, token, 'TECH_PASS', evidence)
            with self.store.tx() as db:
                db.execute('INSERT INTO mobile_reports VALUES(?,?,?)', (task_id, 'PENDING', None))
            return {'status': 'OFFLINE_PASS', 'task_id': task_id}
        except TimeoutError:
            return self._hold(task_id, 'TIMEOUT_NO_RETRY')
        except Exception:
            # 예외 원문을 공개하지 않으며 재실행 없이 체크포인트를 남깁니다.
            task = self.store.get(task_id)
            if task['state'] in ('REVIEW_PENDING', 'DONE'):
                return {'status': 'ALREADY_FINISHED_NO_RETRY', 'task_id': task_id}
            return self._hold(task_id, 'STOPPED_NO_RETRY')

    def report(self, identity, task_id):
        self.store.authorize(identity)
        if type(self.discord) is not FakeDiscord:
            raise Denied('FAKE_ONLY')
        with self.store.tx() as db:
            row = db.execute('SELECT * FROM mobile_reports WHERE task=?', (task_id,)).fetchone()
            if not row or row['state'] != 'PENDING':
                return {'status': row['state'] if row else 'NO_REPORT'}
            db.execute("UPDATE mobile_reports SET state='PUBLISHING' WHERE task=?", (task_id,))
        try:
            task = self.store.get(task_id)
            verdict = 'OFFLINE_PASS' if task['tech'] == 'TECH_PASS' else 'HOLD'
            message = self.discord.send(task_id, '[MOCK / ' + verdict + '] ' + task['role'] + ' / 미술승인·제품적용 없음')
            state = 'SENT'
        except Exception:
            state, message = 'UNKNOWN_NO_RETRY', None
        with self.store.tx() as db:
            db.execute('UPDATE mobile_reports SET state=?,message=? WHERE task=?', (state, message, task_id))
            if state != 'SENT':
                checkpoint(db, self.store.clock(), 'REPORT_UNKNOWN_NO_RETRY')
        return {'status': state}

    def advance(self, identity, root_id):
        """두 아트 담당으로 분기하고 Codex로 취합합니다. 인계에는 추론이 없습니다."""
        self.store.authorize(identity)
        with self.store.tx() as db:
            root = self.store.task(db, root_id)
            if root['root'] != root_id or root['role'] != ORDER[0]:
                raise Denied('PLANNING_ROOT_REQUIRED')
            self._guard(db, root['role'])
            rows = {r['role']: dict(r) for r in db.execute('SELECT * FROM tasks WHERE root=?', (root_id,))}
            def accepted(role):
                t = rows.get(role)
                if not t or t['state'] != 'REVIEW_PENDING' or t['tech'] != 'TECH_PASS':
                    return False
                sent = db.execute("SELECT 1 FROM mobile_reports WHERE task=? AND state='SENT'", (t['id'],)).fetchone()
                e = json.loads(t['evidence'])
                return bool(sent and e.get('mode') == 'MOCK' and e.get('test_only') is True and digest(e['answer']) == e['answer_sha256'])
            if not accepted(ORDER[0]):
                return {'status': 'HOLD', 'tasks': []}
            next_roles = ORDER[1:3] if not all(accepted(r) for r in ORDER[1:3]) else (ORDER[3],)
            if accepted(ORDER[3]):
                evidence = json.loads(root['evidence'])
                evidence['four_role_return'] = json.loads(rows[ORDER[3]]['evidence'])['answer']
                db.execute('UPDATE tasks SET evidence=? WHERE id=?', (json.dumps(evidence, ensure_ascii=False), root_id))
                return {'status': 'OFFLINE_COMPLETE_OWNER_REVIEW', 'tasks': []}
            children = []
            for role in next_roles:
                old = db.execute('SELECT task FROM mobile_links WHERE parent=? AND role=?', (root_id, role)).fetchone()
                if old:
                    children.append(old['task'])
                    continue
                self._guard(db, role)
                payload = json.loads(root['payload'])
                upstream_roles = (ORDER[0],) if role != ORDER[3] else ORDER[1:3]
                upstream = {r: json.loads(rows[r]['evidence'])['answer'] for r in upstream_roles}
                payload.update(role=role, upstream=upstream, upstream_sha256=digest(upstream))
                child = self.store._create(db, 'offline-handoff:' + root_id + ':' + role, payload,
                                           root=root_id, depth=2 if role == ORDER[3] else 1)
                db.execute('INSERT INTO mobile_links VALUES(?,?,?)', (root_id, role, child))
                children.append(child)
            return {'status': 'WAIT_OWNER_EXECUTION_APPROVAL', 'tasks': children}

    def recover(self):
        self.store.recover()
        with self.store.tx() as db:
            # 중단된 리허설은 자동 재개하거나 승인을 재발급하지 않습니다.
            db.execute("UPDATE tasks SET state='NOT_VERIFIED',lease=NULL,lease_until=NULL WHERE state='RUNNING'")
            db.execute("UPDATE mobile_reports SET state='UNKNOWN_NO_RETRY' WHERE state='PUBLISHING'")
            checkpoint(db, self.store.clock(), 'INTERRUPTED_NO_RETRY')
