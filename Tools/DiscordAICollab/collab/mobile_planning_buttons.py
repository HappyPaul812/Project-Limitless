"""시험 앱의 Planning 한 건용 버튼/SQLite 경계. OS 실행·서비스 인증 접근은 없습니다."""
import importlib.util
import json
from pathlib import Path
import re
import uuid

from .core import Denied, Identity
from .mobile_offline import OfflineMobileRelay
from .operating_modes import checkpoint, mode
from .scheduling import gate

_path = Path(__file__).resolve().parents[2] / 'DiscordAIServiceDiagnostics/mobile-worker-contract.py'
_spec = importlib.util.spec_from_file_location('mobile_worker_contract', _path)
contract = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(contract)

ACTIONS = {'view', 'approve', 'run', 'cancel', 'status'}
SCHEMA = (
    'CREATE TABLE IF NOT EXISTS planning_panels(token TEXT PRIMARY KEY,task TEXT,sha TEXT,owner INTEGER,guild INTEGER,channel INTEGER,expires REAL,message TEXT,state TEXT)',
    'CREATE TABLE IF NOT EXISTS planning_clicks(id TEXT PRIMARY KEY,fingerprint TEXT,result TEXT)',
    'CREATE TABLE IF NOT EXISTS planning_jobs(id TEXT PRIMARY KEY,task TEXT UNIQUE,ticket TEXT,state TEXT,result TEXT,report TEXT,message TEXT)',
)


class PlanningButtons:
    def __init__(self, relay, *, offline_fixture=False, prepared_request_id=None):
        # 새 격리 DB만 허용하고 기존 운영 Store를 받지 않습니다.
        if type(relay) is not OfflineMobileRelay:
            raise Denied('ISOLATED_RELAY_REQUIRED')
        self.relay, self.store = relay, relay.store
        if (self.store.owners != {contract.OWNER} or self.store.guild != contract.GUILD
                or self.store.channels != {contract.CHANNEL}):
            raise Denied('FIXED_TRIAL_OWNER_AND_CHANNEL_REQUIRED')
        self.offline_fixture = offline_fixture
        if prepared_request_id is not None and not re.fullmatch('[a-f0-9]{32}', prepared_request_id):
            raise Denied('PREPARED_REQUEST_ID_HOLD')
        self.prepared_request_id = prepared_request_id
        with self.store.tx() as db:
            for sql in SCHEMA: db.execute(sql)
            db.execute('CREATE TABLE IF NOT EXISTS planning_meta(key TEXT PRIMARY KEY,value TEXT)')

    def authorize(self, identity, application):
        self.store.authorize(identity)
        if application != contract.TRIAL_APP or type(application) is not int:
            raise Denied('TRIAL_APPLICATION_REQUIRED')

    def create_task(self, identity, event):
        self.authorize(identity, contract.TRIAL_APP)
        with self.store.tx() as db:
            old = db.execute('SELECT id FROM tasks').fetchall()
            if old:
                if len(old) != 1: raise Denied('ONE_PLANNING_ONLY')
                return old[0][0]
            if db.execute("SELECT 1 FROM planning_meta WHERE key='root_event'").fetchone(): raise Denied('ROOT_ALREADY_RESERVED')
            db.execute("INSERT INTO planning_meta VALUES('root_event',?)", (event,))
        payload = {'title': 'PlanningAgent 읽기 전용 연결 확인', 'role': 'PlanningAgent', 'issue': 66,
                   'base_sha': '243bf0bab0841b545f5ff9cc4f187f2880826b4c', 'scope': contract.POLICY['scope'],
                   'inputs': [], 'protected_assets': ['Unity', 'Save'], 'verification': ['EXACT_SENTENCE_JSONL'],
                   'approval_required': True, 'artifact': '', 'expected': contract.EXPECTED}
        # 기존 core 계약/receipt를 재사용하며 FakeExecutor는 실행하지 않습니다.
        return self.store.command(identity, event, 'task', payload)['task_id']

    def binding(self, task):
        payload = json.loads(task['payload'])
        return {'task_id': task['id'], 'role': task['role'], 'scope': payload['scope'], 'contract': payload,
                'root': task['root'], 'depth': task['depth'], 'upstream': payload.get('upstream', {}),
                'expected': payload.get('expected'), 'policy': contract.POLICY}

    def panel(self, identity, task_id, application=contract.TRIAL_APP):
        self.authorize(identity, application)
        task = self.store.get(task_id)
        token = uuid.uuid4().hex
        with self.store.tx() as db:
            db.execute('INSERT INTO planning_panels VALUES(?,?,?,?,?,?,?,?,?)',
                       (token, task_id, contract.digest(self.binding(task)), identity.user_id, identity.guild_id,
                        identity.channel_id, self.store.clock() + 300, None, 'ACTIVE'))
        return token

    def bind_message(self, token, message):
        with self.store.tx() as db:
            db.execute('UPDATE planning_panels SET message=? WHERE token=? AND message IS NULL', (str(message), token))

    def _checked_panel(self, db, identity, application, token, message):
        row = db.execute('SELECT * FROM planning_panels WHERE token=?', (token,)).fetchone()
        if (not row or row['state'] != 'ACTIVE' or row['expires'] <= self.store.clock()
                or row['owner'] != identity.user_id or row['guild'] != identity.guild_id
                or row['channel'] != identity.channel_id or row['message'] != str(message)):
            raise Denied('PANEL_EXPIRED_OR_CONTEXT_CHANGED')
        task = self.store.task(db, row['task'])
        if contract.digest(self.binding(task)) != row['sha']:
            raise Denied('FULL_TASK_SHA_CHANGED')
        return row, task

    def click(self, identity, application, token, message, interaction_id, action):
        self.authorize(identity, application)
        if action not in ACTIONS or not re.fullmatch('[0-9]{1,32}', str(interaction_id)):
            raise Denied('BUTTON_ACTION_INVALID')
        fingerprint = contract.digest([identity.user_id, identity.guild_id, identity.channel_id, application, token, str(message), action])
        with self.store.tx() as db:
            row, task = self._checked_panel(db, identity, application, token, message)
            old = db.execute('SELECT * FROM planning_clicks WHERE id=?', (str(interaction_id),)).fetchone()
            if old:
                if old['fingerprint'] != fingerprint: raise Denied('INTERACTION_CHANGED')
                return json.loads(old['result'])
            if mode(db) != 'MANUAL': raise Denied('MANUAL_REQUIRED')
            result = self._action(db, identity, row, task, action)
            db.execute('INSERT INTO planning_clicks VALUES(?,?,?)', (str(interaction_id), fingerprint, json.dumps(result, ensure_ascii=False)))
            self.store.audit(db, 'PLANNING_BUTTON_' + action.upper(), task['id'], identity.user_id)
            return result

    def _action(self, db, identity, panel, task, action):
        grant = db.execute('SELECT * FROM mobile_grants WHERE task=?', (task['id'],)).fetchone()
        job = db.execute('SELECT * FROM planning_jobs WHERE task=?', (task['id'],)).fetchone()
        if action == 'view':
            return {'status': 'DETAILS', 'text': 'PlanningAgent 읽기 전용 한국어 응답1건. 파일/도구/다른 역할 실행 없음. 최대90초·exec1·재실행0. 검증 SHA ' + panel['sha'][:8] + '… (전체는 서버 검증).'}
        if action == 'status':
            return {'status': task['state'], 'job': job['state'] if job else 'NOT_REQUESTED',
                    'report': job['report'] if job else None,
                    'final_response': (json.loads(job['result']).get('final_response') if job and job['result'] else None)}
        if action == 'cancel':
            if job and job['state'] == 'VERIFIED':
                return {'status': 'ALREADY_COMPLETE', 'text': '이미 완료된 결과입니다. 승인·소비·결과 기록을 보존합니다.'}
            db.execute("UPDATE mobile_grants SET state='CANCELLED' WHERE task=? AND state='APPROVED'", (task['id'],))
            db.execute("UPDATE tasks SET state='CANCELLED',lease=NULL,lease_until=NULL WHERE id=?", (task['id'],))
            db.execute("UPDATE planning_jobs SET state='CANCELLED' WHERE task=?", (task['id'],))
            checkpoint(db, self.store.clock(), 'BUTTON_OWNER_CANCELLED_NO_RETRY')
            return {'status': 'CANCELLED', 'request_id': job['id'] if job else None}
        if (self.store.setting(db, 'paused') != '0' or self.store.setting(db, 'role:' + task['role']) != '0'
                or gate(db, self.store.clock(), duration=90)):
            raise Denied('SCHEDULE_OR_PAUSE_HOLD')
        plan = self.binding(task)
        if plan['role'] != 'PlanningAgent' or plan['scope'] != contract.POLICY['scope'] or plan['expected'] != contract.EXPECTED:
            raise Denied('PLANNING_ONE_SENTENCE_ONLY')
        if action == 'approve':
            if grant:
                if grant['state'] == 'APPROVED' and grant['sha'] == panel['sha'] and grant['expires'] > self.store.clock():
                    return {'status': 'ALREADY_APPROVED_NO_EXTENSION'}
                raise Denied('APPROVAL_ALREADY_USED_OR_EXPIRED')
            if task['state'] != 'NEW': raise Denied('NEW_TASK_REQUIRED')
            db.execute('INSERT INTO mobile_grants VALUES(?,?,?,?)', (task['id'], panel['sha'], panel['expires'], 'APPROVED'))
            db.execute("UPDATE tasks SET state='TRIAGED' WHERE id=?", (task['id'],))
            return {'status': 'APPROVED', 'text': '이 Planning 업무만 승인했습니다. 아직 실행하지 않았습니다.'}
        if job:
            return {'status': 'ALREADY_REQUESTED_NO_RETRY', 'request_id': job['id']}
        if (not grant or grant['state'] != 'APPROVED' or grant['sha'] != panel['sha']
                or grant['expires'] <= self.store.clock() or task['state'] != 'TRIAGED'):
            raise Denied('EXACT_UNCONSUMED_APPROVAL_REQUIRED')
        if db.execute('SELECT COUNT(*) FROM planning_jobs').fetchone()[0]: raise Denied('ONE_WORKER_REQUEST_PER_TRIAL')
        now = self.store.clock()
        ticket = {'request_id': self.prepared_request_id or uuid.uuid4().hex, 'task_id': task['id'], 'task_sha256': panel['sha'], 'plan': plan,
                  'owner': identity.user_id, 'application': contract.TRIAL_APP, 'guild': identity.guild_id,
                  'channel': identity.channel_id, 'approved_until': grant['expires'], 'created': now,
                  'deadline': now + 90, 'policy': contract.POLICY}
        ticket['ticket_sha256'] = contract.digest(ticket)
        db.execute("UPDATE mobile_grants SET state='CONSUMED' WHERE task=?", (task['id'],))
        db.execute('INSERT INTO planning_jobs VALUES(?,?,?,?,?,?,?)',
                   (ticket['request_id'], task['id'], json.dumps(ticket, ensure_ascii=False), 'QUEUED', None, 'NONE', None))
        return {'status': 'QUEUED', 'request_id': ticket['request_id'],
                'text': '요청 접수. 별도 실전 arm·Session0 검증 전에는 실제 AI를 시작하지 않습니다.'}

    def ticket(self, request):
        with self.store.tx() as db:
            row = db.execute('SELECT ticket FROM planning_jobs WHERE id=?', (request,)).fetchone()
            if not row: raise Denied('REQUEST_NOT_FOUND')
            return json.loads(row[0])

    def claim_handoff(self, request):
        with self.store.tx() as db:
            row = db.execute('SELECT * FROM planning_jobs WHERE id=?', (request,)).fetchone()
            if not row or row['state'] != 'QUEUED': return None
            ticket = json.loads(row['ticket'])
            task = self.store.task(db, row['task'])
            if (self.store.clock() >= ticket['deadline'] or task['state'] == 'CANCELLED'
                    or mode(db) != 'MANUAL' or self.store.setting(db, 'paused') != '0'
                    or self.store.setting(db, 'role:' + task['role']) != '0'
                    or contract.digest(self.binding(task)) != ticket['task_sha256']):
                db.execute("UPDATE planning_jobs SET state='HOLD' WHERE id=?", (request,))
                checkpoint(db, self.store.clock(), 'HANDOFF_POLICY_HOLD')
                return None
            db.execute("UPDATE planning_jobs SET state='HANDED_OFF' WHERE id=?", (request,))
            return ticket

    def hold(self, request, reason='SERVICE_HANDOFF_HOLD'):
        with self.store.tx() as db:
            db.execute("UPDATE planning_jobs SET state='HOLD' WHERE id=?", (request,))
            checkpoint(db, self.store.clock(), reason)

    def accept_result(self, request, projection):
        from .session0_planning_bridge import TrustedProjection
        if type(projection) is not TrustedProjection or (projection.fixture and not self.offline_fixture):
            raise Denied('PROTECTED_SERVICE_PROJECTION_REQUIRED')
        projection = projection.value
        ticket = self.ticket(request)
        with self.store.tx() as db:
            job = db.execute('SELECT * FROM planning_jobs WHERE id=?', (request,)).fetchone()
            if job['state'] != 'HANDED_OFF': return False
            good = (projection.get('request_id') == request and projection.get('task_id') == ticket['task_id']
                    and projection.get('ticket_sha256') == ticket['ticket_sha256']
                    and projection.get('task_sha256') == ticket['task_sha256']
                    and projection.get('status') == 'VERIFIED' and projection.get('final_response') == contract.EXPECTED
                    and type(projection.get('model_exec_launches')) is int and projection['model_exec_launches'] == 1
                    and self.store.clock() <= ticket['deadline'])
            if not good:
                db.execute("UPDATE planning_jobs SET state='HOLD' WHERE id=?", (request,))
                checkpoint(db, self.store.clock(), 'SERVICE_PROJECTION_HOLD')
                return False
            db.execute("UPDATE planning_jobs SET state='VERIFIED',result=?,report='PENDING' WHERE id=?", (json.dumps(projection, ensure_ascii=False), request))
            evidence_mode = 'OFFLINE_SERVICE_FIXTURE' if self.offline_fixture else 'REAL_SERVICE_READONLY'
            db.execute("UPDATE tasks SET state='REVIEW_PENDING',tech='TECH_PASS',evidence=? WHERE id=?", (json.dumps({'mode': evidence_mode, 'result': projection}), ticket['task_id']))
            return True

    def claim_report(self, request):
        with self.store.tx() as db:
            row = db.execute('SELECT * FROM planning_jobs WHERE id=?', (request,)).fetchone()
            if not row or row['report'] != 'PENDING': return None
            db.execute("UPDATE planning_jobs SET report='SENDING' WHERE id=?", (request,))
            return json.loads(row['result'])['final_response']

    def report_result(self, request, message_id=None):
        with self.store.tx() as db:
            db.execute("UPDATE planning_jobs SET report=?,message=? WHERE id=? AND report='SENDING'",
                       ('SENT' if message_id is not None else 'UNKNOWN_NO_RETRY', str(message_id) if message_id is not None else None, request))
            if message_id is None: checkpoint(db, self.store.clock(), 'DISCORD_REPORT_UNKNOWN_NO_RETRY')

    def recover(self):
        with self.store.tx() as db:
            db.execute("UPDATE planning_panels SET state='EXPIRED' WHERE state='ACTIVE'")
            db.execute("UPDATE planning_jobs SET state='HOLD' WHERE state IN ('QUEUED','HANDED_OFF')")
            db.execute("UPDATE planning_jobs SET report='UNKNOWN_NO_RETRY' WHERE report='SENDING'")
            checkpoint(db, self.store.clock(), 'BUTTON_WORKER_RESTART_NO_RETRY')


def human_text(value):
    if value.get('final_response'): return value['final_response']
    if value.get('text'): return value['text']
    return {'CANCELLED': '취소했습니다. 소비·검증 기록은 보존합니다.',
            'ALREADY_REQUESTED_NO_RETRY': '이미 요청한 업무입니다. 다시 실행하지 않습니다.',
            'ALREADY_APPROVED_NO_EXTENSION': '이미 승인했습니다. 승인 기간은 연장하지 않습니다.',
            'TRIAGED': '승인됨 · 실행 요청 전입니다.', 'REVIEW_PENDING': '응답 검증이 완료됐습니다.',
            'NEW': '승인 대기입니다.'}.get(value['status'], '진행 상태: ' + str(value.get('job', value['status'])))


def make_view(workflow, token, *, on_run=None, on_cancel=None):
    """실제 SDK 버튼5개. 서비스 요청 전달 callback은 별도 arm 경계 뒤에만 연결합니다."""
    import discord
    class PlanningView(discord.ui.View):
        def __init__(self):
            super().__init__(timeout=300)
            for action, label, style in [('view', '업무 내용 보기', discord.ButtonStyle.secondary),
                                          ('approve', '실행 승인', discord.ButtonStyle.success),
                                          ('run', '실행', discord.ButtonStyle.primary),
                                          ('cancel', '취소', discord.ButtonStyle.danger),
                                          ('status', '진행 상황 확인', discord.ButtonStyle.secondary)]:
                button = discord.ui.Button(label=label, style=style, custom_id='lm:' + token + ':' + action)
                async def callback(interaction, selected=action):
                    identity = Identity(interaction.user.id, interaction.guild_id or 0, interaction.channel_id or 0, interaction.user.bot)
                    try:
                        workflow.authorize(identity, interaction.application_id)
                        if not (interaction.permissions.use_application_commands and interaction.app_permissions.view_channel and interaction.app_permissions.send_messages):
                            raise Denied('DISCORD_PERMISSION_HOLD')
                        await interaction.response.defer(ephemeral=True, thinking=True)
                        value = workflow.click(identity, interaction.application_id, token, interaction.message.id, str(interaction.id), selected)
                        if selected == 'run' and value['status'] == 'QUEUED' and on_run is not None and workflow.claim_handoff(value['request_id']) is not None:
                            try:
                                await on_run(value['request_id'], interaction)
                            except Exception:
                                workflow.hold(value['request_id'])
                                raise Denied('SERVICE_HANDOFF_HOLD') from None
                        if selected == 'cancel' and value.get('request_id') and on_cancel is not None:
                            await on_cancel(value['request_id'])
                        await interaction.followup.send('[MANUAL / Planning 한 건]\n' + human_text(value)[:1600],
                                                        ephemeral=True, allowed_mentions=discord.AllowedMentions.none(), wait=True)
                    except Exception:
                        if not interaction.response.is_done():
                            await interaction.response.send_message('HOLD: 대표·업무·권한·만료를 확인해 주세요. 자동 재실행 없음.', ephemeral=True, allowed_mentions=discord.AllowedMentions.none())
                        else:
                            await interaction.followup.send('HOLD: 요청 결과 확인 필요. 자동 재실행 없음.', ephemeral=True, allowed_mentions=discord.AllowedMentions.none(), wait=True)
                button.callback = callback
                self.add_item(button)

        async def on_timeout(self):
            for child in self.children: child.disabled = True
            with workflow.store.tx() as db:
                db.execute("UPDATE planning_panels SET state='EXPIRED' WHERE token=?", (token,))
            self.stop()
    return PlanningView()


def build_group(workflow, *, on_run=None, on_cancel=None):
    import discord
    from discord import app_commands
    group = app_commands.Group(name='limitless-trial', description='Planning 한 건 모바일 버튼 승인 시험',
                               guild_only=True, default_permissions=discord.Permissions.none())
    @group.command(name='task', description='읽기 전용 Planning 업무와 버튼 열기')
    async def task(interaction: discord.Interaction):
        identity = Identity(interaction.user.id, interaction.guild_id or 0, interaction.channel_id or 0, interaction.user.bot)
        workflow.authorize(identity, interaction.application_id)
        if not (interaction.permissions.use_application_commands and interaction.app_permissions.view_channel and interaction.app_permissions.send_messages):
            raise Denied('DISCORD_PERMISSION_HOLD')
        await interaction.response.defer(ephemeral=True, thinking=True)
        task_id = workflow.create_task(identity, str(interaction.id))
        token = workflow.panel(identity, task_id, interaction.application_id)
        message = await interaction.followup.send('PlanningAgent 확인 응답1건입니다. 내용을 확인한 뒤 승인·실행을 각각 눌러주세요.',
                    view=make_view(workflow, token, on_run=on_run, on_cancel=on_cancel), ephemeral=True, wait=True, allowed_mentions=discord.AllowedMentions.none())
        workflow.bind_message(token, message.id)
    return group
