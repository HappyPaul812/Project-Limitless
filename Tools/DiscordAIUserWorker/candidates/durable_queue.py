"""기존 SQLite와 승인·4역할 실행기를 재사용하는 준비 코드입니다. 실행 CLI/자동 시작은 없습니다."""
import hashlib
import json
import os
from pathlib import Path
import uuid

from collab.core import Denied
from collab.real_roles import validate, slot_role, run_once
from collab.scheduling import Scheduler, gate


def execution_policy(path=Path('C:/ProgramData/LimitlessRelay/user-worker-integrity.json')):
    """내용 등록과 실행 허가를 분리합니다. 현재 등록은 false이므로 실제 경로는 계속 HOLD합니다."""
    registry = json.loads(path.read_text(encoding='utf-8-sig'))
    if registry.get('kind') != 'USER_WORKER_INTEGRITY_ONLY' or registry.get('execution_permission') is not True:
        raise Denied('WORKER_EXECUTION_NOT_APPROVED')
    entries = registry.get('worker_adapter_files') or []
    original = registry.get('approved_runtime_files') or []
    if not entries or len(original) != 38:
        raise Denied('WORKER_ADAPTER_PIN_REQUIRED')
    for entry in [*registry['files'], *entries, *original]:
        source = Path(entry['absolute_path'])
        if source.is_symlink() or hashlib.sha256(source.read_bytes()).hexdigest() != entry['sha256']:
            raise Denied('WORKER_REGISTERED_CONTENT_CHANGED')
    cli_files = registry.get('user_cli_files') or []
    if not cli_files:
        raise Denied('WORKER_FULL_CLI_PIN_REQUIRED')
    for entry in cli_files:
        if hashlib.sha256(Path(entry['absolute_path']).read_bytes()).hexdigest() != entry['sha256']:
            raise Denied('WORKER_USER_CLI_CHANGED')
    return registry


def current_user_context():
    """활성 사용자 SID·프로필은 Windows 토큰에서 읽고 서비스/환경변수 사칭을 피합니다."""
    import ctypes
    from collab.service_ai import windows_context
    value = windows_context()
    session = ctypes.c_ulong()
    if not ctypes.windll.kernel32.ProcessIdToSessionId(os.getpid(), ctypes.byref(session)):
        raise Denied('WORKER_SESSION_NOT_VERIFIED')
    value.update(session_id=session.value, is_admin=value.pop('admin'),
                 codex_home=os.environ.get('CODEX_HOME') or str(Path(value['profile']) / '.codex'))
    return value


def official_authenticate(executable, context):
    from collab.phase2 import cli_info
    return cli_info(str(executable), environ=own_environment(context))['subscription_available']


def official_usage(executable, context):
    import asyncio
    from collab.usage_bridge import OfficialUsage
    return asyncio.run(OfficialUsage(str(executable), environment=own_environment(context)).read())


def own_environment(context):
    from collab.phase2 import child_environment
    profile = context['profile']
    return {**child_environment(), 'HOME': profile, 'USERPROFILE': profile,
            'CODEX_HOME': str(Path(profile) / '.codex')}


def official_source_refs(plan):
    from collab.adapters import GitHubReader
    reader = GitHubReader()
    return {ref: reader.read('base' if ref == 'main' else 'canonical_ref', None if ref == 'main' else ref)['sha']
            for ref in plan['source_refs']}


def initialize(db):
    # 승인·예약·기존 업무 테이블은 바꾸지 않고 같은 DB에 Worker 전용 표만 추가합니다.
    db.execute('CREATE TABLE IF NOT EXISTS user_worker_jobs(id TEXT PRIMARY KEY,plan TEXT NOT NULL,worker_sid TEXT NOT NULL,state TEXT NOT NULL,result TEXT,created REAL NOT NULL)')
    db.execute('CREATE TABLE IF NOT EXISTS user_worker_lock(singleton INTEGER PRIMARY KEY CHECK(singleton=1),owner TEXT NOT NULL,job TEXT NOT NULL)')
    db.execute('CREATE TABLE IF NOT EXISTS user_worker_checkpoints(job TEXT PRIMARY KEY,role TEXT,state TEXT,remaining TEXT,updated REAL)')
    db.execute('CREATE TABLE IF NOT EXISTS user_worker_handoffs(job TEXT PRIMARY KEY,parent_task TEXT,next_task TEXT,next_role TEXT,state TEXT)')


def approval(store, db, plan):
    validate(plan)
    if slot_role(plan['slot']) != plan['role']:
        raise Denied('WORKER_ROLE_SCOPE_MISMATCH')
    row = db.execute('SELECT value FROM settings WHERE key=?', (plan['slot'],)).fetchone()
    grant = json.loads(row[0]) if row else {}
    if (grant.get('state') != 'APPROVED' or grant.get('sha256') != plan['sha256']
            or grant.get('role') != plan['role'] or grant.get('expires', 0) <= store.clock()
            or not 0 <= store.clock() - plan['created'] <= 86400):
        raise Denied('WORKER_EXACT_UNCONSUMED_APPROVAL_REQUIRED')
    prior = db.execute('SELECT state FROM tasks WHERE event_key=?', (plan['slot'],)).fetchone()
    if prior and prior['state'] not in ('NEW', 'TRIAGED'):
        raise Denied('WORKER_PRIOR_ATTEMPT_OR_RESULT_HOLD')
    checkpoint_task = plan.get('checkpoint_task')
    if checkpoint_task:
        checkpoint = db.execute('SELECT * FROM operation_checkpoints WHERE task=?', (checkpoint_task,)).fetchone()
        task = db.execute('SELECT role,state,payload FROM tasks WHERE id=?', (checkpoint_task,)).fetchone()
        payload = json.loads(task['payload']) if task else {}
        if (not checkpoint or not task or task['state'] not in ('NEW', 'TRIAGED')
                or task['role'] != plan['role'] or payload.get('scope') != plan['slot']
                or payload.get('base_sha') != plan['source_refs']['main']
                or checkpoint['role'] != plan['role'] or checkpoint['scope'] != plan['slot']
                or checkpoint['base_sha'] != plan['source_refs']['main'] or checkpoint['stage'] == 'RUNNING'):
            raise Denied('WORKER_CHECKPOINT_SCOPE_HOLD')


def enqueue(store, identity, plan, worker_sid):
    """부팅 중계 쪽 API입니다. 기존 정확한 승인이 있는 계획만 보관하며 새 승인/호출을 만들지 않습니다."""
    store.authorize(identity)
    if not isinstance(worker_sid, str) or not worker_sid.startswith('S-1-5-21-'):
        raise Denied('WORKER_USER_SID_REQUIRED')
    with store.tx() as db:
        initialize(db)
        approval(store, db, plan)
        db.execute('INSERT OR IGNORE INTO user_worker_jobs VALUES(?,?,?,?,?,?)',
                   (plan['sha256'], json.dumps(plan), worker_sid, 'PENDING_LOGIN', None, store.clock()))
        # 동일 SHA를 다른 사용자에게 다시 배정하는 것도 허용하지 않습니다.
        row = db.execute('SELECT worker_sid FROM user_worker_jobs WHERE id=?', (plan['sha256'],)).fetchone()
        if row[0] != worker_sid:
            raise Denied('WORKER_ASSIGNMENT_IMMUTABLE')
        db.execute('INSERT OR IGNORE INTO user_worker_checkpoints VALUES(?,?,?,?,?)',
                   (plan['sha256'], plan['role'], 'PENDING_LOGIN', '로그인·공식 사용량·미소비 승인 재확인', store.clock()))
    return plan['sha256']


def inspect(store):
    with store.tx() as db:
        initialize(db)
        return [dict(row) for row in db.execute('SELECT id,worker_sid,state,created FROM user_worker_jobs ORDER BY created,id')]


def existing_role_runner(store, identity, plan, workspace, executable):
    # Planning/Codex/NewArt/Renewal 모두 같은 기존 승인·1회소비·JSONL/최종응답 검증 경로를 사용합니다.
    context = current_user_context()
    environment = own_environment(context)
    old = {key: os.environ.get(key) for key in ('HOME', 'USERPROFILE', 'CODEX_HOME')}
    try:
        # 전용 Worker 프로세스의 자기 프로필 경로만 지정합니다. 인증파일/키링은 복사하지 않습니다.
        for key in old:
            os.environ[key] = environment[key]
        from .runtime_adapter import recorded_run_once
        return recorded_run_once(store, identity, plan, workspace, executable, context, run_once)
    finally:
        for key, value in old.items():
            if value is None:
                os.environ.pop(key, None)
            else:
                os.environ[key] = value


class UserWorker:
    """단일 업무만 처리하는 준비 상태 머신입니다. 기본값은 인증/네트워크 호출 전 HOLD입니다."""
    def __init__(self, store, identity, executable, executable_sha, workspace_root):
        self.store, self.identity = store, identity
        self.binary, self.binary_sha = Path(executable), executable_sha
        self.workspace = Path(workspace_root)

    def once(self, context, *, released=False, authenticate=None, official_usage=None,
             source_refs=None, runner=existing_role_runner):
        # 이번 작업에는 released=True를 사용하는 실행 CLI·설정·실제 호출 승인을 제공하지 않습니다.
        if not released:
            return {'state': 'HOLD_NOT_RELEASED', 'actual_ai_calls': 0}
        if runner is existing_role_runner:
            try:
                policy = execution_policy()
            except (Denied, OSError, ValueError):
                return {'state': 'HOLD_EXECUTION_NOT_APPROVED', 'actual_ai_calls': 0}
            # 실제 실행 경로에서는 호출자가 넘긴 모의 context를 신뢰하지 않습니다.
            context = current_user_context()
            if context.get('sid') != policy.get('user_sid') or self.binary != Path(policy.get('user_cli_path', '')):
                return {'state': 'HOLD_REGISTERED_USER_OR_CLI_MISMATCH', 'actual_ai_calls': 0}
        self.store.authorize(self.identity)
        if (context.get('session_id', 0) <= 0 or context.get('is_admin') is not False
                or context.get('name', '').split('\\')[-1].lower() == 'limitlessrelaysvc'):
            return {'state': 'HOLD_USER_LOGON_REQUIRED', 'actual_ai_calls': 0}
        profile = Path(context.get('profile', ''))
        if not profile.is_absolute() or Path(context.get('codex_home', '')) != profile / '.codex':
            return {'state': 'HOLD_OWN_PROFILE_REQUIRED', 'actual_ai_calls': 0}
        if (not self.binary.is_absolute() or self.binary.is_symlink() or not self.binary.is_file()
                or hashlib.sha256(self.binary.read_bytes()).hexdigest() != self.binary_sha):
            return {'state': 'HOLD_RUNTIME_CHANGED', 'actual_ai_calls': 0}
        if any(callback is None for callback in (authenticate, official_usage, source_refs)):
            return {'state': 'HOLD_PREFLIGHT_NOT_CONFIGURED', 'actual_ai_calls': 0}
        token = uuid.uuid4().hex
        chosen = None
        with self.store.tx() as db:
            initialize(db)
            # 크래시 후 claim을 자동으로 빼앗지 않아 실행 여부 불명의 재시도를 막습니다.
            if db.execute('SELECT 1 FROM user_worker_lock').fetchone():
                return {'state': 'HOLD_WORKER_BUSY_OR_INTERRUPTED', 'actual_ai_calls': 0}
            if db.execute('SELECT mode FROM operation_mode WHERE singleton=1').fetchone()[0] != 'MANUAL':
                return {'state': 'HOLD_MANUAL_REQUIRED', 'actual_ai_calls': 0}
            if self.store.setting(db, 'paused') == '1':
                return {'state': 'HOLD_PAUSED', 'actual_ai_calls': 0}
            for row in db.execute("SELECT * FROM user_worker_jobs WHERE state='PENDING_LOGIN' ORDER BY created,id").fetchall():
                if row['worker_sid'] != context.get('sid'):
                    continue
                try:
                    plan = json.loads(row['plan'])
                    approval(self.store, db, plan)
                except (Denied, ValueError, TypeError):
                    db.execute("UPDATE user_worker_jobs SET state='WAITING_REAPPROVAL' WHERE id=?", (row['id'],))
                    continue
                if self.store.setting(db, 'role:' + plan['role']) == '1':
                    continue
                if db.execute("SELECT 1 FROM tasks WHERE state='RUNNING' LIMIT 1").fetchone():
                    return {'state': 'HOLD_OTHER_AI_RUNNING', 'actual_ai_calls': 0}
                db.execute('INSERT INTO user_worker_lock VALUES(1,?,?)', (token, row['id']))
                db.execute("UPDATE user_worker_jobs SET state='CHECKING' WHERE id=?", (row['id'],))
                chosen = plan
                break
        if chosen is None:
            return {'state': 'NO_ELIGIBLE_JOB', 'actual_ai_calls': 0}
        state, reason, result = 'PENDING_LOGIN', 'PREFLIGHT_HOLD', None
        try:
            if authenticate(self.binary, context) is not True:
                raise Denied('WORKER_CHATGPT_AUTH_HOLD')
            observed = official_usage(self.binary, context)
            Scheduler(self.store).record_official_usage(observed, observed['observed'])
            if source_refs(chosen) != chosen['source_refs']:
                state = 'WAITING_REAPPROVAL'
                raise Denied('WORKER_CANONICAL_CHANGED')
            if hashlib.sha256(self.binary.read_bytes()).hexdigest() != self.binary_sha:
                state = 'WAITING_REAPPROVAL'
                raise Denied('WORKER_RUNTIME_CHANGED_DURING_PREFLIGHT')
            with self.store.tx() as db:
                approval(self.store, db, chosen)
                if (gate(db, self.store.clock(), ai=True, duration=90)
                        or self.store.setting(db, 'paused') == '1'
                        or self.store.setting(db, 'role:' + chosen['role']) == '1'
                        or db.execute('SELECT mode FROM operation_mode WHERE singleton=1').fetchone()[0] != 'MANUAL'
                        or db.execute("SELECT 1 FROM tasks WHERE state='RUNNING' LIMIT 1").fetchone()):
                    raise Denied('WORKER_FINAL_GATE_HOLD')
                db.execute("UPDATE user_worker_jobs SET state='RUNNING' WHERE id=?", (chosen['sha256'],))
            # 이 경계 후 실패/중단은 결과 불명으로 두고 재시도하지 않습니다. run_once도 승인을 다시 확인합니다.
            state = 'NOT_VERIFIED'
            self.workspace.mkdir(parents=True, exist_ok=True)
            result = runner(self.store, self.identity, chosen, self.workspace / chosen['sha256'], self.binary)
            state = 'DONE' if (result.get('status') == 'PASS' and result.get('mode') == 'REAL_CLI_READONLY'
                               and result.get('queue_acceptance') != 'NOT_VERIFIED_PAUSE_OR_LEASE_CHANGED') else 'NOT_VERIFIED'
            reason = 'EXISTING_RUNNER_RESULT'
            if state == 'DONE' and result.get('task_id') and chosen.get('auto_handoff', True):
                state = 'NOT_VERIFIED'
                result['worker_handoff'] = record_handoff(self.store, chosen, result)
                state = 'DONE'
        except Exception as exc:
            # 외부 예외 원문·토큰·인증정보는 큐에 저장하지 않습니다.
            reason = str(exc) if isinstance(exc, Denied) else 'WORKER_STOPPED_NO_RETRY'
        finally:
            with self.store.tx() as db:
                db.execute('UPDATE user_worker_jobs SET state=?,result=? WHERE id=?',
                           (state, json.dumps({'reason': reason, 'result': result}), chosen['sha256']))
                db.execute('INSERT OR REPLACE INTO user_worker_checkpoints VALUES(?,?,?,?,?)',
                           (chosen['sha256'], chosen['role'], state,
                            '후속 계획 별도 승인 필요' if state == 'DONE' else 'HOLD·기존 승인/사용량/체크포인트 재확인', self.store.clock()))
                if state == 'DONE' and chosen.get('publish_verified') is True:
                    from collab.real_roles import discord_text
                    db.execute('INSERT OR IGNORE INTO control_outbox VALUES(?,?,?,?)',
                               ('user-worker:'+chosen['sha256']+':VERIFIED',discord_text(result),'PENDING',None))
                if state != 'DONE':
                    # 기존 봇의 영속 outbox에만 연결합니다. Worker는 Discord 토큰을 읽거나 직접 게시하지 않습니다.
                    text = '[USER AI WORKER / HOLD] '+chosen['role']+' / '+chosen['sha256'][:12]+' / '+state+' / 자동 재시도·유료전환 없음·MANUAL 유지'
                    db.execute('INSERT OR IGNORE INTO control_outbox VALUES(?,?,?,?)',
                               ('user-worker:'+chosen['sha256']+':'+state, text, 'PENDING', None))
                db.execute('DELETE FROM user_worker_lock WHERE owner=?', (token,))
        return {'id': chosen['sha256'], 'state': state, 'reason': reason}


def record_handoff(store, plan, result):
    """검증된 기존 task만 기존 역할 규칙으로 인계합니다. 새 AI 승인/후속 호출은 만들지 않습니다."""
    task = store.get(result['task_id'])
    evidence = json.loads(task['evidence'] or '{}')
    if (task['role'] != plan['role'] or task['event_key'] != plan['slot'] or task['tech'] != 'TECH_PASS'
            or evidence.get('status') != 'PASS' or evidence.get('mode') != 'REAL_CLI_READONLY'
            or evidence.get('plan_sha256') != plan['sha256']):
        raise Denied('WORKER_HANDOFF_EVIDENCE_NOT_VERIFIED')
    handoff = store.auto_handoff(task['id'])
    next_task = store.get(handoff['task_id'])
    with store.tx() as db:
        db.execute('INSERT OR IGNORE INTO user_worker_handoffs VALUES(?,?,?,?,?)',
                   (plan['sha256'], task['id'], next_task['id'], next_task['role'], handoff['state']))
    return {**handoff, 'next_role': next_task['role'], 'next_ai_approved': False}
