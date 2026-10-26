"""등록/접속 기능 없는 Discord 시험 어댑터. 오직 Phase2의 합성 DB·FakeExecutor만 연결합니다."""
import json
import math
import re

from .core import Denied, Identity
from .mobile_offline import OfflineMobileRelay, FakeExecutor, FakeDiscord, ORDER, digest
from .operating_modes import checkpoint, mode

GROUP = 'limitless-trial'
COMMANDS = ('task', 'plan', 'approve-execution', 'cancel-execution', 'run-fake', 'handoff', 'status')


def registration_plan(*, production_application_id, trial_application_id, guild_id, existing_commands):
    """별도 시험 앱의 단일 guild 명령 생성 계획만 반환합니다. HTTP/전체 sync는 없습니다."""
    ids = (production_application_id, trial_application_id, guild_id)
    if any(type(x) is not int or x <= 0 for x in ids) or production_application_id == trial_application_id:
        raise Denied('DISTINCT_APPROVED_TRIAL_APPLICATION_REQUIRED')
    if not isinstance(existing_commands, list) or any(not isinstance(c, dict) for c in existing_commands):
        raise Denied('COMMAND_INVENTORY_REQUIRED')
    if any(c.get('name') == GROUP for c in existing_commands):
        raise Denied('COMMAND_EXISTS_NO_OVERWRITE')
    return {'operation': 'CREATE_ONE_GUILD_COMMAND_AFTER_APPROVAL', 'group': GROUP,
            'application_id': trial_application_id, 'guild_id': guild_id,
            'bulk_overwrite': False, 'global_registration': False, 'production_restart': False,
            'rollback': 'DELETE_ONLY_RETURNED_TRIAL_COMMAND_ID', 'actual_requests': 0}


class TrialQuota:
    """공식 조회가 아닌 명시적 합성 관측입니다. Discord 사용자 입력으로 갱신하지 않습니다."""
    def __init__(self):
        self.observed = None
        self.windows = []

    def observe(self, observed, windows):
        if (type(observed) not in (int, float) or not math.isfinite(observed)
                or not isinstance(windows, list) or len(windows) != 2 or any(not isinstance(w, dict) for w in windows)
                or {w.get('minutes') for w in windows} != {300, 10080}):
            raise Denied('FAKE_QUOTA_INVALID')
        for window in windows:
            for key in ('used_percent', 'resets_at'):
                if type(window.get(key)) not in (int, float) or not math.isfinite(window[key]):
                    raise Denied('FAKE_QUOTA_INVALID')
            if not 0 <= window['used_percent'] <= 100 or window['resets_at'] <= observed:
                raise Denied('FAKE_QUOTA_INVALID')
        self.observed, self.windows = observed, json.loads(json.dumps(windows))

    def decision(self, now):
        if self.observed is None or now < self.observed:
            return 'UNKNOWN', None
        exhausted = [w for w in self.windows if w['used_percent'] >= 100]
        if exhausted:
            reset = max(w['resets_at'] for w in exhausted)
            return ('EXHAUSTED' if now < reset else 'RECHECK_REQUIRED'), reset
        if now - self.observed > 300:
            return 'UNKNOWN', None
        if any(now >= w['resets_at'] for w in self.windows):
            return 'RECHECK_REQUIRED', None
        return 'AVAILABLE', None


class MobileTrialController:
    def __init__(self, relay, *, base_sha, trial_application_id, production_application_id):
        if type(relay) is not OfflineMobileRelay or not re.fullmatch('[0-9a-f]{40}', base_sha):
            raise Denied('ISOLATED_FAKE_RELAY_REQUIRED')
        self.relay, self.store, self.base_sha = relay, relay.store, base_sha
        registration_plan(production_application_id=production_application_id,
                          trial_application_id=trial_application_id, guild_id=self.store.guild, existing_commands=[])
        self.trial_application_id = trial_application_id
        self.quota = TrialQuota()  # 재기동 시에도 새 합성 관측 전 HOLD
        with self.store.tx() as db:
            db.execute('CREATE TABLE IF NOT EXISTS trial_events(id TEXT PRIMARY KEY,fingerprint TEXT,state TEXT,result TEXT,message_id TEXT)')
            db.execute('CREATE TABLE IF NOT EXISTS trial_results(task TEXT PRIMARY KEY,event TEXT UNIQUE)')
            db.execute('CREATE TABLE IF NOT EXISTS trial_meta(key TEXT PRIMARY KEY,value TEXT)')
            db.execute("INSERT OR IGNORE INTO trial_meta VALUES('base_sha',?)", (base_sha,))
            if db.execute("SELECT value FROM trial_meta WHERE key='base_sha'").fetchone()[0] != base_sha:
                raise Denied('TRIAL_BASE_CHANGED')
            binding = json.dumps([trial_application_id, production_application_id])
            db.execute("INSERT OR IGNORE INTO trial_meta VALUES('application_binding',?)", (binding,))
            if db.execute("SELECT value FROM trial_meta WHERE key='application_binding'").fetchone()[0] != binding:
                raise Denied('TRIAL_APPLICATION_CHANGED')

    def authorize(self, identity, permissions):
        self.store.authorize(identity)
        required = {'owner_use_commands', 'bot_view_channel', 'bot_send_messages'}
        if not isinstance(permissions, dict) or any(permissions.get(k) is not True for k in required):
            raise Denied('DISCORD_PERMISSION_HOLD')

    def _guard(self, db):
        if (mode(db) != 'MANUAL' or type(self.relay.executor) is not FakeExecutor
                or type(self.relay.discord) is not FakeDiscord):
            raise Denied('FAKE_MANUAL_ONLY')

    def _cancel(self, task_id):
        with self.store.tx() as db:
            task = self.store.task(db, task_id)
            # root 취소는 분기된 후속 업무도 취소합니다. 소비/근거/보고는 삭제하지 않습니다.
            ids = [r[0] for r in db.execute('SELECT id FROM tasks WHERE root=?', (task_id,))] if task_id == task['root'] else [task_id]
            for item in ids:
                db.execute("UPDATE mobile_grants SET state='CANCELLED' WHERE task=? AND state='APPROVED'", (item,))
                db.execute("UPDATE tasks SET state='CANCELLED',lease=NULL,lease_until=NULL WHERE id=?", (item,))
            checkpoint(db, self.store.clock(), 'OWNER_CANCELLED_NO_RETRY')
            self.store.audit(db, 'TRIAL_OWNER_CANCELLED', task_id)
        return '취소 완료. 승인·소비·검증 기록은 보존했습니다.'

    def command(self, identity, event, action, args, permissions):
        self.authorize(identity, permissions)
        if action not in COMMANDS or not isinstance(event, str) or not re.fullmatch('[0-9]{1,32}', event):
            raise Denied('TRIAL_COMMAND_INVALID')
        if not isinstance(args, dict) or len(json.dumps(args)) > 2048:
            raise Denied('TRIAL_ARGUMENT_INVALID')
        fingerprint = digest([identity.user_id, identity.guild_id, identity.channel_id, action, args])
        with self.store.tx() as db:
            self._guard(db)
            old = db.execute('SELECT * FROM trial_events WHERE id=?', (event,)).fetchone()
            if old:
                if old['fingerprint'] != fingerprint:
                    raise Denied('INTERACTION_CHANGED')
                return old['result'] or '처리 중 또는 결과 확인 대기입니다. 자동 재실행하지 않습니다.'
            db.execute('INSERT INTO trial_events VALUES(?,?,?,?,?)', (event, fingerprint, 'STARTED', None, None))
        try:
            text = self._execute(identity, event, action, args)
        except Exception:
            # 입력·토큰·SDK 오류 원문을 Discord에 반사하지 않습니다.
            text = 'HOLD: 요청을 완료하지 못했습니다. 승인·범위·만료·정지 상태를 확인해 주세요. 자동 재실행 없음.'
            with self.store.tx() as db:
                checkpoint(db, self.store.clock(), 'TRIAL_COMMAND_HOLD')
        text = '[시험 / 가짜 실행 / MANUAL]\n' + text
        with self.store.tx() as db:
            db.execute("UPDATE trial_events SET state='READY',result=? WHERE id=?", (text[:1800], event))
        return text[:1800]

    def _execute(self, identity, event, action, args):
        relay = self.relay
        if action == 'status':
            quota, reset = self.quota.decision(self.store.clock())
            rows = self.store.snapshot()['tasks']
            lines = ['AI 실제 호출 0회 · 운영 DB 변경 없음', '모의 사용량: ' + quota + ((' / 회복 확인 시각 ' + str(reset)) if reset else '')]
            with self.store.tx() as db:
                unknown = db.execute("SELECT COUNT(*) FROM trial_events WHERE state='UNKNOWN_NO_RETRY'").fetchone()[0]
            lines.append('보고/처리 확인 대기: ' + str(unknown) + '건 · 자동 재전송 없음')
            lines += [t['id'] + ' · ' + t['role'] + ' · ' + t['state'] for t in rows[:8]]
            return '\n'.join(lines)
        if action == 'task':
            title = args.get('title', '')
            if not isinstance(title, str) or not 1 <= len(title) <= 120 or any(ord(c) < 32 for c in title):
                raise Denied('TITLE_INVALID')
            with self.store.tx() as db:
                if db.execute('SELECT COUNT(*) FROM tasks WHERE id=root').fetchone()[0] >= 1:
                    raise Denied('ONE_ROOT_PER_TRIAL')
                if db.execute("SELECT 1 FROM trial_meta WHERE key='root_event'").fetchone():
                    raise Denied('ROOT_ALREADY_RESERVED_NO_RETRY')
                db.execute("INSERT INTO trial_meta VALUES('root_event',?)", (event,))
            contract = {'title': title, 'role': ORDER[0], 'issue': 66, 'base_sha': self.base_sha,
                        'scope': 'OFFLINE_TEXT_REVIEW', 'protected_assets': ['Unity', 'Save'],
                        'inputs': [], 'verification': ['MOCK_JSONL'], 'approval_required': True,
                        'artifact': '', 'offline_only': True}
            task = relay.mobile(identity, event + ':intake', 'task', contract)['task_id']
            return '접수 완료: ' + task + '\n아직 실행하지 않았습니다. plan으로 SHA를 확인해 주세요.'
        task_id = args.get('task_id')
        if not isinstance(task_id, str) or not re.fullmatch('L-[0-9a-f]{16}', task_id):
            raise Denied('TASK_ID_INVALID')
        task = self.store.get(task_id)
        if action == 'cancel-execution':
            return self._cancel(task_id)
        if action == 'plan':
            plan = relay.plan(task_id)
            return (task_id + ' · ' + task['role'] + '\n범위: OFFLINE_TEXT_REVIEW\nSHA: ' + digest(plan)
                    + '\n가짜 호출 최대1회 · 실제 AI0 · 승인 유효기간 최대300초\n미술 승인과 별개입니다.')
        if action == 'approve-execution':
            ttl = args.get('ttl_seconds', 120)
            if type(ttl) is not int or not 1 <= ttl <= 300:
                raise Denied('EXPIRY_INVALID')
            relay.mobile(identity, event + ':grant', 'approve-execution', {'task_id': task_id,
                         'sha256': args.get('sha256'), 'expires': self.store.clock() + ttl})
            return task_id + '\n실행 승인 기록 완료. 아직 실행하지 않았습니다. run-fake로 단발 시험합니다.'
        if action == 'run-fake':
            if task['state'] == 'CANCELLED':
                return task_id + '\nHOLD: 취소한 업무입니다. 승인·소비 기록 보존.'
            quota, _ = self.quota.decision(self.store.clock())
            if quota != 'AVAILABLE':
                with self.store.tx() as db:
                    checkpoint(db, self.store.clock(), 'TRIAL_QUOTA_' + quota)
                return task_id + '\nHOLD: 모의 사용량 ' + quota + '. 승인·체크포인트 보존, 소비·실행 없음.'
            # 초기 승인 취소/만료, 역할/모드/계약/상위 SHA는 기존 실행기가 다시 검사합니다.
            result = relay.step(identity, task_id)
            if result['status'] != 'OFFLINE_PASS':
                return task_id + '\nHOLD: ' + result['status'] + ' · 자동 재실행 없음.'
            relay.report(identity, task_id)  # 기존 가짜 outbox. 실제 전송 성공과 구분합니다.
            with self.store.tx() as db:
                db.execute('INSERT INTO trial_results VALUES(?,?)', (task_id, event))
            return task_id + ' · ' + task['role'] + '\n가짜 실행 PASS / JSONL 검증 PASS\n실제 AI 호출0 · 미술 승인·게임 적용 없음.'
        if action == 'handoff':
            with self.store.tx() as db:
                rows = db.execute('SELECT id FROM tasks WHERE root=? AND tech=\'TECH_PASS\'', (task_id,)).fetchall()
                for row in rows:
                    delivered = db.execute("SELECT 1 FROM trial_results r JOIN trial_events e ON e.id=r.event WHERE r.task=? AND e.state='SENT'", (row['id'],)).fetchone()
                    if not delivered:
                        raise Denied('DISCORD_RECEIPT_REQUIRED')
            result = relay.advance(identity, task_id)
            return result['status'] + '\n' + '\n'.join(result['tasks']) + '\n후속 업무는 각각 새 실행 승인 필요. 추가 AI 호출 없음.'
        raise Denied('TRIAL_COMMAND_INVALID')

    def claim_delivery(self, event):
        with self.store.tx() as db:
            row = db.execute('SELECT * FROM trial_events WHERE id=?', (event,)).fetchone()
            if not row or row['state'] != 'READY':
                return None
            db.execute("UPDATE trial_events SET state='SENDING' WHERE id=?", (event,))
            return row['result']

    def delivery_result(self, event, message_id=None):
        with self.store.tx() as db:
            state = 'SENT' if message_id is not None else 'UNKNOWN_NO_RETRY'
            db.execute("UPDATE trial_events SET state=?,message_id=? WHERE id=? AND state='SENDING'", (state, str(message_id) if message_id is not None else None, event))
            if message_id is None:
                checkpoint(db, self.store.clock(), 'DISCORD_DELIVERY_UNKNOWN_NO_RETRY')

    def recover(self):
        self.relay.recover()
        with self.store.tx() as db:
            db.execute("UPDATE trial_events SET state='UNKNOWN_NO_RETRY' WHERE state IN ('STARTED','SENDING')")
            checkpoint(db, self.store.clock(), 'TRIAL_RESTART_NO_RETRY')


def build_trial_group(controller):
    """discord.py 로컬 명령 객체만 생성합니다. login/run/sync/명령 등록은 제공하지 않습니다."""
    import discord
    from discord import app_commands
    group = app_commands.Group(name=GROUP, description='LIMITLESS 가짜 실행 한정 연결 시험',
                               guild_only=True, default_permissions=discord.Permissions.none())

    async def dispatch(interaction, action, args):
        identity = Identity(interaction.user.id, interaction.guild_id or 0, interaction.channel_id or 0, interaction.user.bot)
        permissions = {'owner_use_commands': interaction.permissions.use_application_commands,
                       'bot_view_channel': interaction.app_permissions.view_channel,
                       'bot_send_messages': interaction.app_permissions.send_messages}
        try:
            if interaction.application_id != controller.trial_application_id:
                raise Denied('TRIAL_APPLICATION_MISMATCH')
            controller.authorize(identity, permissions)
        except Denied:
            await interaction.response.send_message('HOLD: 허용된 대표·서버·채널·권한을 확인해 주세요.', ephemeral=True, allowed_mentions=discord.AllowedMentions.none())
            return
        await interaction.response.defer(ephemeral=True, thinking=True)
        event = str(interaction.id)
        try:
            controller.command(identity, event, action, args, permissions)
            text = controller.claim_delivery(event)
            if text is None:
                return
            message = await interaction.followup.send(text, ephemeral=True, wait=True, allowed_mentions=discord.AllowedMentions.none())
            controller.delivery_result(event, message.id)
        except Exception:
            controller.delivery_result(event)

    @group.command(name='status', description='시험 업무·모의 사용량·HOLD 상태')
    async def status(interaction: discord.Interaction):
        await dispatch(interaction, 'status', {})

    @group.command(name='task', description='가짜 실행 시험 업무 접수; 실제 AI 실행 없음')
    async def task(interaction: discord.Interaction, title: str):
        await dispatch(interaction, 'task', {'title': title})

    @group.command(name='plan', description='업무 역할·범위·정확한 승인 SHA 확인')
    async def plan(interaction: discord.Interaction, task_id: str):
        await dispatch(interaction, 'plan', {'task_id': task_id})

    @group.command(name='approve-execution', description='가짜 실행 단발 승인; 미술 최종 승인 아님')
    async def approve(interaction: discord.Interaction, task_id: str, sha256: str, ttl_seconds: int = 120):
        await dispatch(interaction, 'approve-execution', {'task_id': task_id, 'sha256': sha256, 'ttl_seconds': ttl_seconds})

    @group.command(name='cancel-execution', description='시험 업무 실행 승인 취소·기록 보존')
    async def cancel(interaction: discord.Interaction, task_id: str):
        await dispatch(interaction, 'cancel-execution', {'task_id': task_id})

    @group.command(name='run-fake', description='승인된 가짜 worker 최대1회; 실제 AI0')
    async def run_fake(interaction: discord.Interaction, task_id: str):
        await dispatch(interaction, 'run-fake', {'task_id': task_id})

    @group.command(name='handoff', description='보고 영수증 확인 후 4역할 업무 인계')
    async def handoff(interaction: discord.Interaction, task_id: str):
        await dispatch(interaction, 'handoff', {'task_id': task_id})
    return group
