"""예약은 실행 승인이 아닙니다. 모든 위험 동작은 모의 판정만 제공하고 실패 시 보류합니다."""
from __future__ import annotations

import hashlib
import json
import math
import re
import uuid
from datetime import datetime

from .core import Denied, KST

ACTIONS = {'schedule', 'schedule-status', 'cancel', 'usage', 'finish', 'shutdown'}
SCHEMA = '''
CREATE TABLE IF NOT EXISTS control_schedules(
 id TEXT PRIMARY KEY, deadline REAL NOT NULL, cleanup INTEGER NOT NULL,
 next_start REAL, state TEXT NOT NULL, owner INTEGER NOT NULL, created REAL NOT NULL);
CREATE TABLE IF NOT EXISTS control_events(
 key TEXT PRIMARY KEY, fingerprint TEXT NOT NULL, result TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS control_usage(
 id INTEGER PRIMARY KEY, observed REAL NOT NULL, source TEXT NOT NULL, body TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS control_holds(
 schedule TEXT NOT NULL, task TEXT NOT NULL, original_state TEXT NOT NULL,
 PRIMARY KEY(schedule,task));
CREATE TABLE IF NOT EXISTS control_reports(
 id TEXT PRIMARY KEY, schedule TEXT NOT NULL, body TEXT NOT NULL, created REAL NOT NULL);
CREATE TABLE IF NOT EXISTS control_settings(key TEXT PRIMARY KEY,value TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS control_interactions(interaction TEXT PRIMARY KEY,action TEXT,response_id TEXT,owner_verified INTEGER,ephemeral INTEGER,gateway INTEGER,created REAL);
CREATE TABLE IF NOT EXISTS control_outbox(
 id TEXT PRIMARY KEY, body TEXT NOT NULL, state TEXT NOT NULL, message_id TEXT);
INSERT OR IGNORE INTO control_settings VALUES('mode','SIMULATION_ONLY');
'''


def initialize(db):
    # executescript는 기존 트랜잭션을 commit하므로 각 문장을 따로 실행합니다.
    for statement in SCHEMA.split(';'):
        if statement.strip():
            db.execute(statement)


def seoul(timestamp):
    return datetime.fromtimestamp(timestamp, KST).strftime('%Y-%m-%d %H:%M:%S KST')


def parse_time(text, now):
    """날짜 없는 HH:MM은 오늘입니다. 지난 시각을 내일로 임의 해석하지 않습니다."""
    if not isinstance(text, str):
        raise Denied('시각은 YYYY-MM-DD HH:MM 또는 오늘 HH:MM으로 입력하세요')
    text = text.strip().removeprefix('오늘 ')
    if re.fullmatch(r'\d{2}:\d{2}', text):
        text = datetime.fromtimestamp(now, KST).strftime('%Y-%m-%d ') + text
    try:
        value = datetime.strptime(text, '%Y-%m-%d %H:%M').replace(tzinfo=KST).timestamp()
    except ValueError:
        raise Denied('서울 시각 형식이 올바르지 않습니다') from None
    if not now < value <= now + 366 * 86400:
        raise Denied('지난 시각 또는 1년을 넘는 예약은 등록하지 않습니다')
    return value


def schedule_row(db):
    return db.execute("SELECT * FROM control_schedules WHERE state NOT IN ('CANCELLED','SUPERSEDED') ORDER BY created DESC,id DESC LIMIT 1").fetchone()


def advance(db, now):
    row = schedule_row(db)
    if row is None:
        return None
    row = dict(row)
    state = row['state']
    if state == 'ACTIVE' and now >= row['deadline'] - row['cleanup']:
        state = 'DRAINING'
    if state == 'DRAINING' and now >= row['deadline']:
        state = 'CLOSED'
    if state != row['state']:
        db.execute('UPDATE control_schedules SET state=? WHERE id=?', (state, row['id']))
        row['state'] = state
        mode=db.execute("SELECT value FROM control_settings WHERE key='mode'").fetchone()[0]
        text = ('[SCHEDULE / '+mode+'] 예약 ' + row['id'] + ' / ' + state +
                ' 신규 접수·실행 보류. 진행중 업무는 강제 종료하지 않으며 미완료 장부를 보존합니다. '
                'Git 통합·PC 종료·추가 AI 호출 없음.')
        db.execute('INSERT OR IGNORE INTO control_outbox VALUES(?,?,?,?)',
                   (row['id'] + ':' + state, text, 'PENDING', None))
    if row['next_start'] and now >= row['next_start'] and state == 'CLOSED':
        # 다음 시각은 재개 의무가 아닌 재검증 요청입니다. 새 승인·관측 없이 실행하지 않습니다.
        text = ('[SCHEDULE / RECHECK_REQUIRED] 예약 ' + row['id'] +
                ' 다음 확인 시각입니다. 새 공식 사용량·유효 실행 승인·새 작업 시간창을 확인하세요. 자동 AI 실행0.')
        db.execute('INSERT OR IGNORE INTO control_outbox VALUES(?,?,?,?)',
                   (row['id'] + ':NEXT_CHECK', text, 'PENDING', None))
    if state in ('DRAINING', 'CLOSED', 'CANCELLED_HOLD'):
        # 업무 상태를 바꾸지 않고 보류 인덱스만 저장하므로 승인/lease/evidence를 훼손하지 않습니다.
        db.execute("INSERT OR IGNORE INTO control_holds SELECT ?,id,state FROM tasks WHERE state IN ('NEW','TRIAGED')", (row['id'],))
    return row


def usage_decision(db, now):
    row = db.execute('SELECT * FROM control_usage ORDER BY id DESC LIMIT 1').fetchone()
    if row is None:
        return {'status': 'UNKNOWN', 'reason': '공식 사용량 확인 필요', 'recheck_at': None}
    value = json.loads(row['body'])
    if value.get('status') == 'UNKNOWN':
        return {'status':'UNKNOWN','reason':'공식 자동 조회 실패·새 확인 전 실행 차단','recheck_at':None,'source':value['source']}
    exhausted = [w for w in value['windows'] if w['used_percent'] >= 100]
    recheck = max((w['resets_at'] for w in exhausted), default=None)
    if exhausted:
        return {'status': 'EXHAUSTED' if now < recheck else 'RECHECK_REQUIRED',
                'reason': '회복 시각 후 새 공식 관측 필요', 'recheck_at': recheck, **value}
    if value.get('ordinary_allowed') is not True:
        return {'status': 'UNKNOWN', 'reason': '공식 일반 사용 허용을 확인할 수 없습니다', 'recheck_at': None, **value}
    if not 0 <= now - row['observed'] <= 300 or any(now >= w['resets_at'] for w in value['windows']):
        return {'status': 'UNKNOWN', 'reason': '관측 만료: 새 공식 사용량 확인 필요', 'recheck_at': None, **value}
    return {'status': 'AVAILABLE', 'reason': '새 관측 기준 잔여 있음; 실행 승인은 별도', 'recheck_at': None, **value}


def gate(db, now, *, ai=False, intake=False, duration=90):
    """공통 트랜잭션에서 마감과 사용량을 재검증합니다. 회복만으로 모델을 호출하지 않습니다."""
    row = advance(db, now)
    if not intake and datetime.fromtimestamp(now, KST).weekday() == 6:
        return 'SUNDAY_HOLD'
    if row:
        if row['state'] != 'ACTIVE':
            return 'SCHEDULE_HOLD'
        # intake도 정리 시각 이후 차단하며 90초 실행은 정리 시작 전에 종료돼야 합니다.
        if not intake and now + duration >= row['deadline'] - row['cleanup']:
            return 'INSUFFICIENT_TIME'
    if ai and usage_decision(db, now)['status'] != 'AVAILABLE':
        return 'USAGE_HOLD'
    return None


class Scheduler:
    def __init__(self, store):
        self.store = store

    def record_official_usage(self, body, observed):
        """로컬 신뢰 브리지 전용. Discord 명령으로 사용량 숫자를 입력받지 않습니다.

        앱 공식 도구를 사람이 실행한 세션에서 정규화해서 전달합니다. 봇에는 자동 API가
        연결되지 않았으므로 관측 만료 후 호출을 차단합니다. 토큰/계정/크레딧은 저장하지 않습니다.
        """
        now = self.store.clock()
        if not isinstance(observed, (float, int)) or not math.isfinite(observed) or not 0 <= now - observed <= 60:
            raise Denied('최신 공식 관측만 허용합니다')
        if not isinstance(body, dict) or body.get('source') not in ('CODEX_APP_OFFICIAL_TOOL','CODEX_APP_SERVER_OFFICIAL'):
            raise Denied('지원되는 공식 관측 출처가 아닙니다')
        windows = body.get('windows')
        if not isinstance(windows, list) or len(windows) != 2:
            raise Denied('5시간 및 주간 한도 모두 필요합니다')
        normalized = []
        for item, minutes in zip(windows, (300, 10080)):
            if not isinstance(item, dict):
                raise Denied('사용량 창 형식 오류')
            percent, reset = item.get('used_percent'), item.get('resets_at')
            if (item.get('minutes') != minutes or type(percent) not in (int, float) or not math.isfinite(percent)
                    or not 0 <= percent <= 100 or type(reset) not in (int, float)
                    or not math.isfinite(reset) or reset <= observed):
                raise Denied('사용량·회복 시각 확인 불가')
            normalized.append({'minutes': minutes, 'used_percent': percent, 'resets_at': reset})
        if type(body.get('ordinary_allowed')) is not bool:
            raise Denied('공식 일반 사용 허용 여부가 필요합니다')
        clean = {'observed': observed, 'source': body['source'], 'windows': normalized,
                 'ordinary_allowed': body['ordinary_allowed']}
        with self.store.tx() as db:
            db.execute('INSERT INTO control_usage(observed,source,body) VALUES(?,?,?)',
                       (observed, clean['source'], json.dumps(clean)))
        return clean

    def invalidate_usage(self, reason):
        with self.store.tx() as db:
            body={'status':'UNKNOWN','source':'CODEX_APP_SERVER_OFFICIAL','reason':reason}
            db.execute('INSERT INTO control_usage(observed,source,body) VALUES(?,?,?)',
                       (self.store.clock(),body['source'],json.dumps(body)))

    def tick(self):
        with self.store.tx() as db:
            row = advance(db, self.store.clock())
            mode=db.execute("SELECT value FROM control_settings WHERE key='mode'").fetchone()[0]
            return {'mode': mode, 'schedule': row,
                    'usage': usage_decision(db, self.store.clock()),
                    'ai_calls': 0, 'shutdown_calls': 0, 'git_mutations': 0}

    def recover_reports(self):
        with self.store.tx() as db:
            db.execute("UPDATE control_outbox SET state='UNCERTAIN' WHERE state='SENDING'")

    async def flush_reports(self, publish):
        # Discord 전송 성공 여부가 불분명하면 재전송하지 않습니다. 토큰/예외 원문은 기록하지 않습니다.
        with self.store.tx() as db:
            keys = [r[0] for r in db.execute("SELECT id FROM control_outbox WHERE state='PENDING' ORDER BY id")]
        for key in keys:
            with self.store.tx() as db:
                row = db.execute('SELECT * FROM control_outbox WHERE id=?', (key,)).fetchone()
                if row['state'] != 'PENDING':
                    continue
                db.execute("UPDATE control_outbox SET state='SENDING' WHERE id=?", (key,))
            try:
                message_id = await publish(row['body'])
                with self.store.tx() as db:
                    db.execute("UPDATE control_outbox SET state='SENT',message_id=? WHERE id=?", (str(message_id), key))
            except Exception:
                with self.store.tx() as db:
                    db.execute("UPDATE control_outbox SET state='UNCERTAIN' WHERE id=?", (key,))

    def command(self, identity, event, action, args=None):
        self.store.authorize(identity)
        args = args or {}
        if action not in ACTIONS or not isinstance(args, dict) or not isinstance(event, str) or not 1 <= len(event) <= 160:
            raise Denied('예약 명령 형식 오류')
        fingerprint = hashlib.sha256(json.dumps([action, args], sort_keys=True).encode()).hexdigest()
        key = f'{identity.guild_id}:{identity.user_id}:{event}'
        with self.store.tx() as db:
            old = db.execute('SELECT * FROM control_events WHERE key=?', (key,)).fetchone()
            if old:
                if old['fingerprint'] != fingerprint:
                    raise Denied('같은 이벤트의 다른 예약 요청 거부')
                return json.loads(old['result'])
            now = self.store.clock()
            current = advance(db, now)
            mode=db.execute("SELECT value FROM control_settings WHERE key='mode'").fetchone()[0]
            if action == 'schedule':
                cleanup = args.get('cleanup_minutes', 10)
                if type(cleanup) is not int or not 2 <= cleanup <= 120:
                    raise Denied('정리 시간은 2~120분입니다')
                deadline = parse_time(args.get('until'), now)
                if deadline - cleanup * 60 <= now:
                    raise Denied('정리 시간을 확보할 수 없는 마감입니다')
                next_start = parse_time(args['next_start'], now) if args.get('next_start') else None
                if next_start and (next_start <= deadline or datetime.fromtimestamp(next_start, KST).weekday() == 6):
                    raise Denied('다음 시작은 마감 이후이며 주일이 아니어야 합니다')
                if datetime.fromtimestamp(deadline, KST).weekday() == 6 or datetime.fromtimestamp(now, KST).weekday() == 6:
                    raise Denied('주일 작업 예약 금지')
                if current:
                    if args.get('schedule_id') != current['id']:
                        raise Denied('현재 예약 ID를 지정하여 수정하세요')
                    db.execute("UPDATE control_schedules SET state='SUPERSEDED' WHERE id=?", (current['id'],))
                elif args.get('schedule_id'):
                    raise Denied('수정 대상 예약이 없습니다')
                sid = 'SCH-' + uuid.uuid4().hex[:12]
                db.execute('INSERT INTO control_schedules VALUES(?,?,?,?,?,?,?)',
                           (sid, deadline, cleanup * 60, next_start, 'ACTIVE', identity.user_id, now))
                result = {'mode': mode, 'id': sid, 'state': 'ACTIVE',
                          'deadline': seoul(deadline), 'stop_intake': seoul(deadline - cleanup * 60),
                          'next_start': seoul(next_start) if next_start else '미지정',
                          'note': '예약은 AI·통합·PC 종료 승인이 아닙니다'}
            elif action == 'usage':
                result = usage_decision(db, now)
            elif action == 'schedule-status':
                result = {'schedule': current, 'usage': usage_decision(db, now),
                          'mode': mode, 'next_start_behavior': '시간 이후 재검증만; 자동 AI 실행 없음'}
            else:
                if not current or args.get('schedule_id') != current['id']:
                    raise Denied('현재 예약 ID가 필요합니다')
                if action == 'cancel':
                    release = args.get('release_intake') is True
                    if release and db.execute("SELECT count(*) FROM tasks WHERE state='RUNNING'").fetchone()[0]:
                        raise Denied('실행 중 업무가 있으면 접수 보류를 해제할 수 없습니다')
                    state = 'CANCELLED' if current['state'] == 'ACTIVE' or release else 'CANCELLED_HOLD'
                    db.execute('UPDATE control_schedules SET state=?,next_start=NULL WHERE id=?', (state, current['id']))
                    result = {'state': state, 'id': current['id'], 'note': '종료 요청 취소·기존 수동 정지와 승인 보류 유지'}
                elif action == 'finish':
                    db.execute("UPDATE control_schedules SET state='DRAINING' WHERE id=?", (current['id'],))
                    advance(db, now)
                    tasks = [dict(r) for r in db.execute('SELECT id,role,state,tech,art FROM tasks ORDER BY created,id')]
                    running = [r['id'] for r in tasks if r['state'] == 'RUNNING']
                    result = {'id': current['id'], 'state': 'HOLD' if running else 'REVIEW_REQUIRED',
                              'running': running, 'tasks': tasks, 'integration': 'HOLD_NO_SCOPED_APPROVAL_OR_ADAPTER',
                              'note': '상태 정리 기록만 완료. 실행 중 업무 강제 종료·Git 동기화·PC 종료 없음'}
                else:
                    # Discord 확인도 현재는 모의 의사 기록입니다. local evidence를 외부 인수로 받지 않습니다.
                    if args.get('confirm') != current['id']:
                        raise Denied('해당 예약 ID로 PC 종료 의사를 다시 확인하세요')
                    from .operations_safety import pc_preflight
                    preflight=pc_preflight(self.store,db)
                    result = {'id': current['id'], 'state': 'HOLD', 'mode': 'SHUTDOWN_DISABLED', 'preflight':preflight,
                              'blockers': ['실제 종료 비활성', '별도 로컬 확인·권한 미검증',
                                           'Git 동기화·보호·미저장 작업·Unity·복사·최종 보고 검증 필요'],
                              'cancel': '/limitless cancel schedule_id:' + current['id']}
                rid = 'CTL-' + uuid.uuid4().hex[:12]
                db.execute('INSERT INTO control_reports VALUES(?,?,?,?)', (rid, current['id'], json.dumps(result), now))
                if action in ('finish','shutdown'):
                    text='[OPERATIONS / HOLD] '+action+' 예약 '+current['id']+' / 승인·동기화·미저장 작업 검증 부족. 실제 PC 종료·Push·병합 없음.'
                    db.execute('INSERT OR IGNORE INTO control_outbox VALUES(?,?,?,?)',(rid,text,'PENDING',None))
            db.execute('INSERT INTO control_events VALUES(?,?,?)', (key, fingerprint, json.dumps(result)))
            self.store.audit(db, 'CONTROL_' + action.upper(), actor=identity.user_id)
            return result


def shutdown_preview(evidence, now):
    """오직 합성 데이터의 종료 조건 시험입니다. 이 함수에는 OS 실행 경로가 없습니다."""
    required = ('owner_confirmed', 'local_identity_verified', 'local_shutdown_privilege',
                'scope_completed', 'integration_explicitly_approved', 'sync_verified',
                'protected_files_verified', 'unity_idle', 'copy_idle', 'unsaved_work_clear',
                'no_schedule_conflict', 'discord_report_sent', 'cancel_available')
    blockers = [k for k in required if evidence.get(k) is not True]
    stamp = evidence.get('verified_at')
    if type(stamp) not in (int, float) or not math.isfinite(stamp) or not 0 <= now - stamp <= 30:
        blockers.append('fresh_evidence')
    if evidence.get('cancelled') is True:
        blockers.append('cancelled')
    return {'state': 'HOLD' if blockers else 'WOULD_SHUTDOWN', 'blockers': blockers,
            'mode': 'SIMULATION_ONLY', 'os_calls': 0, 'cancel_grace_seconds': 120}


def format_control(action, value):
    """Discord 응답에는 비밀이나 전체 업무 payload를 포함하지 않습니다."""
    labels = {'AVAILABLE': '사용 가능 관측', 'UNKNOWN': '확인 불가·보류',
              'EXHAUSTED': '한도 소진·보류', 'RECHECK_REQUIRED': '회복 시각 경과·재확인 필요'}
    if action == 'usage':
        lines = ['Codex 사용량: ' + labels.get(value['status'], value['status'])]
        for window in value.get('windows', []):
            name = '5시간' if window['minutes'] == 300 else '주간'
            lines.append(f"{name}: 사용 {window['used_percent']}% / 회복 {seoul(window['resets_at'])}")
        if 'observed' in value:
            lines.append('확인 시각: ' + seoul(value['observed']))
        return '\n'.join(lines + [('공식 App Server 자동 조회 연결·확인 불가 시 보류.' if value.get('source')=='CODEX_APP_SERVER_OFFICIAL' else '공식 자동 갱신 미연결.')+' 유료 전환·크레딧·재설정권 사용 없음.'])
    if action == 'schedule':
        return f"예약 {value['id']} / 서울 시각\n신규 접수 중단: {value['stop_intake']}\n마감: {value['deadline']}\n다음 확인: {value['next_start']}\n{value['note']}"
    if action == 'schedule-status':
        row = value['schedule']
        if not row:
            return '현재 예약 없음. 실제 종료·자동 AI 실행 비활성.'
        return (f"예약 {row['id']} / {row['state']}\n마감: {seoul(row['deadline'])}\n"
                f"다음 확인: {seoul(row['next_start']) if row['next_start'] else '미지정'}\n"
                + ('운영 제어·실제 종료 비활성.' if value.get('mode')=='LIVE_CONTROL' else '모의 안전 제어 모드·다음 시각 자동 호출 없음.'))
    if action == 'finish':
        counts = {}
        for task in value['tasks']:
            counts[task['state']] = counts.get(task['state'], 0) + 1
        return f"마감 점검: {value['state']}\n실행 중 {len(value['running'])}건 / 상태별 {counts}\n통합 HOLD·실제 종료 없음. 미완료 업무는 장부에 보존했습니다."
    if action == 'cancel':
        return f"예약 취소: {value['id']} / {value['state']}\n{value['note']}"
    preflight=value.get('preflight',{})
    return ('PC 종료 HOLD / 대상 PC '+preflight.get('pc_id','확인 불가')+'\n실행 중 업무 '+str(preflight.get('running_tasks','미확인'))+'건 / 미저장 작업·복사·동기화 승인 미검증.\n실제 종료 비활성. 취소: '+value['cancel']+' release_intake:true')
