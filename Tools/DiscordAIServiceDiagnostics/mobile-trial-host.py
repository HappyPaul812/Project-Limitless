"""고정 시험 앱·채널의 한시적 Fake Host. token은 자기 DPAPI 저장소에서만 메모리로 읽습니다."""
import argparse
import asyncio
import ctypes
from ctypes import wintypes
from datetime import datetime, timedelta, timezone
import hashlib
import http.client
import json
import logging
from pathlib import Path
import ssl
import sys
import time

TRIAL = 1564156403571105906
PRODUCTION = 1563102982386749440
GUILD = 1563112744436244541
CHANNEL = 1564163234338967552
OWNER = 740791801987727412
NAME = 'limitless-trial'
KST = timezone(timedelta(hours=9))
SECRET = Path('F:/study/codex/Project-Limitless/Temp/MobileTrialCredentials20261026/LimitlessTest/private/connection.json')


def read_json(path):
    return json.loads(Path(path).read_text(encoding='utf-8-sig'))


def save(path, value, *, exclusive=False):
    with Path(path).open('x' if exclusive else 'w', encoding='utf8') as stream:
        json.dump(value, stream, ensure_ascii=False, indent=2)


def token_from_own_dpapi():
    """ConvertFrom-SecureString의 Windows CurrentUser DPAPI 바이트만 해제합니다."""
    if sys.platform != 'win32' or SECRET.stat().st_size > 65536:
        raise ValueError('DPAPI_STORE_HOLD')
    record = read_json(SECRET)
    if (record['protection'] != 'DPAPI_CURRENT_USER' or record['version'] != 1
            or record['owners'] != str(OWNER) or record['guild'] != str(GUILD)
            or record['channels'] != str(CHANNEL)):
        raise ValueError('DPAPI_BINDING_HOLD')
    class Blob(ctypes.Structure):
        _fields_ = [('size', wintypes.DWORD), ('data', ctypes.POINTER(ctypes.c_ubyte))]
    raw = bytearray.fromhex(record['tokenCipher'])
    array = (ctypes.c_ubyte * len(raw)).from_buffer(raw)
    incoming, outgoing = Blob(len(raw), array), Blob()
    crypt = ctypes.WinDLL('crypt32', use_last_error=True)
    crypt.CryptUnprotectData.argtypes = [ctypes.POINTER(Blob), ctypes.c_void_p, ctypes.c_void_p,
                                       ctypes.c_void_p, ctypes.c_void_p, wintypes.DWORD, ctypes.POINTER(Blob)]
    crypt.CryptUnprotectData.restype = wintypes.BOOL
    kernel = ctypes.WinDLL('kernel32', use_last_error=True)
    kernel.LocalFree.argtypes = [ctypes.c_void_p]
    kernel.LocalFree.restype = ctypes.c_void_p
    try:
        if not crypt.CryptUnprotectData(ctypes.byref(incoming), None, None, None, None, 1, ctypes.byref(outgoing)):
            raise ValueError('CURRENT_USER_DPAPI_HOLD')
        result = ctypes.string_at(outgoing.data, outgoing.size).decode('utf-16-le')
        if not 1 <= len(result) <= 4096:
            raise ValueError('TOKEN_LENGTH_HOLD')
        return result
    finally:
        raw[:] = b'\0' * len(raw)
        if outgoing.data:
            ctypes.memset(outgoing.data, 0, outgoing.size)
            kernel.LocalFree(outgoing.data)


class Rest:
    """Discord HTTPS 고정 호스트, caller 단발 요청. 자동 retry/redirect/원문 오류 출력 없음."""
    def __init__(self, token, root):
        self.token, self.root = token, root

    def request(self, method, path, payload=None):
        allowed = {
            ('GET', '/oauth2/applications/@me'),
            ('GET', f'/applications/{TRIAL}/commands'),
            ('GET', f'/applications/{TRIAL}/guilds/{GUILD}/commands'),
            ('POST', f'/applications/{TRIAL}/guilds/{GUILD}/commands'),
        }
        if (method, path) not in allowed:
            marker = self.root / 'registered.json'
            if method != 'DELETE' or not marker.exists() or path != f'/applications/{TRIAL}/guilds/{GUILD}/commands/{read_json(marker)["id"]}':
                raise ValueError('REST_SCOPE_HOLD')
        connection = http.client.HTTPSConnection('discord.com', timeout=10, context=ssl.create_default_context())
        try:
            data = json.dumps(payload).encode() if payload is not None else None
            connection.request(method, '/api/v10' + path, data,
                               {'Authorization': 'Bot ' + self.token, 'Content-Type': 'application/json',
                                'User-Agent': 'Limitless-Mobile-Fake-Trial/1.0'})
            response = connection.getresponse()
            raw = response.read(524289)
            if len(raw) > 524288:
                raise ValueError('REST_SIZE_HOLD')
            parsed = json.loads(raw) if raw else None
            if response.status not in (200, 201, 204):
                save(self.root / 'rest-error.json', {'method': method, 'http_status': response.status,
                     'error_code': parsed.get('code') if isinstance(parsed, dict) and type(parsed.get('code')) is int else None})
                raise ValueError('DISCORD_HTTP_HOLD')
            return parsed
        finally:
            connection.close()


def owner_matches(app):
    return (str(app.get('id')) == str(TRIAL) and
            (str((app.get('owner') or {}).get('id')) == str(OWNER)
             or str((app.get('team') or {}).get('owner_user_id')) == str(OWNER)))


def command_payload(group, tree):
    payload = group.to_dict(tree)
    for key in ('contexts', 'integration_types', 'dm_permission'):
        payload.pop(key, None)  # guild CREATE에는 global 전용 필드를 보내지 않습니다.
    payload['default_member_permissions'] = '0'
    return payload


def canonical_inventory(items):
    return json.dumps(sorted(items, key=lambda x: str(x['id'])), sort_keys=True, ensure_ascii=False)


def cleanup(root, rest):
    """정확한 반환 ID만 한 번 삭제합니다. 불명 결과를 추정해 재시도하지 않습니다."""
    marker = root / 'registered.json'
    if not marker.exists():
        return {'status': 'CREATE_RESULT_UNKNOWN_HOLD' if (root / 'create-attempted.json').exists() else 'NO_COMMAND_CREATED', 'command_deleted': False}
    command = read_json(marker)
    if str(command.get('application_id')) != str(TRIAL) or command.get('name') != NAME:
        raise ValueError('DELETE_BINDING_HOLD')
    if (root / 'delete-attempted.json').exists():
        return read_json(root / 'cleanup.json') if (root / 'cleanup.json').exists() else {'status': 'DELETE_RESULT_UNKNOWN_NO_RETRY'}
    save(root / 'delete-attempted.json', {'id': command['id']}, exclusive=True)
    rest.request('DELETE', f'/applications/{TRIAL}/guilds/{GUILD}/commands/{command["id"]}')
    after = rest.request('GET', f'/applications/{TRIAL}/guilds/{GUILD}/commands')
    save(root / 'commands-after.json', after)
    global_after = rest.request('GET', f'/applications/{TRIAL}/commands')
    save(root / 'global-commands-after.json', global_after)
    before = read_json(root / 'commands-before.json')
    matched = (canonical_inventory(before) == canonical_inventory(after)
               and canonical_inventory(read_json(root / 'global-commands-before.json')) == canonical_inventory(global_after))
    result = {'status': 'CLEANUP_PASS' if matched else 'INVENTORY_CHANGED_HOLD',
              'command_deleted': True, 'command_id': command['id'], 'original_inventory_restored': matched}
    save(root / 'cleanup.json', result)
    return result


def open_trial_relay(root, config):
    from collab.mobile_offline import OfflineMobileRelay
    if config.get('resume_unconsumed_queue'):
        queue = root / 'Queue/mobile-offline.sqlite'
        if hashlib.sha256(queue.read_bytes()).hexdigest() != config['resume_queue_sha256']:
            raise ValueError('RESUME_QUEUE_SHA_HOLD')
        relay = OfflineMobileRelay.reopen(root / 'Queue', {OWNER}, clock=time.time, guild=GUILD, channels=(CHANNEL,))
        with relay.store.tx() as db:
            tasks = db.execute('SELECT id,role,state FROM tasks').fetchall()
            if (len(tasks) != 1 or tasks[0]['id'] != config['resume_task_id']
                    or tasks[0]['role'] != 'PlanningAgent' or tasks[0]['state'] != 'NEW'
                    or db.execute('SELECT COUNT(*) FROM mobile_grants').fetchone()[0] != 0):
                raise ValueError('ONLY_UNCONSUMED_NEW_TASK_CAN_RESUME')
    else:
        relay = OfflineMobileRelay.create(root / 'Queue', {OWNER}, clock=time.time, guild=GUILD, channels=(CHANNEL,))
    return relay


async def serve(root, config, rest, token):
    import discord
    from discord import app_commands
    from collab.mobile_discord_trial import MobileTrialController, build_trial_group, registration_plan
    started = time.monotonic()
    deadline = started + 1170
    expires = datetime.now(KST) + timedelta(seconds=1170)
    stop = asyncio.Event()
    result = {'status': 'NOT_READY', 'mode': 'MANUAL', 'model_calls': 0, 'official_usage_reads': 0,
              'operational_sqlite_operations': 0, 'application_retries': 0, 'trial_application_id': str(TRIAL),
              'production_application_id': str(PRODUCTION), 'expires_at_kst': expires.isoformat()}
    relay = open_trial_relay(root, config)
    controller = MobileTrialController(relay, base_sha=config['base_sha'], trial_application_id=TRIAL, production_application_id=PRODUCTION)
    intents = discord.Intents.none()
    intents.guilds = True  # role/channel overwrite 확인용 비특권 intent만 사용합니다.
    client = discord.Client(intents=intents, allowed_mentions=discord.AllowedMentions.none())
    tree = app_commands.CommandTree(client, fallback_to_global=False)
    async def intake_allowed(interaction):
        return not stop.is_set()
    tree.interaction_check = intake_allowed
    group = build_trial_group(controller)
    tree.add_command(group, guild=discord.Object(id=GUILD))  # 로컬 추가만, sync하지 않습니다.
    ready_started = False

    @client.event
    async def on_ready():
        nonlocal ready_started
        if ready_started:
            result['status'] = 'DUPLICATE_READY_HOLD';stop.set();return
        ready_started = True
        try:
            if client.application_id != TRIAL or not client.user.bot:
                raise ValueError('GATEWAY_APPLICATION_HOLD')
            guild = client.get_guild(GUILD)
            if guild is None:
                raise ValueError('GUILD_ACCESS_HOLD')
            channel = guild.get_channel(CHANNEL)
            if not isinstance(channel, discord.TextChannel):
                raise ValueError('TEXT_CHANNEL_HOLD')
            owner = await guild.fetch_member(OWNER)
            bot = guild.me or await guild.fetch_member(client.user.id)
            op, bp = channel.permissions_for(owner), channel.permissions_for(bot)
            if not (op.view_channel and op.use_application_commands and op.administrator
                    and bp.view_channel and bp.send_messages) or bp.administrator:
                raise ValueError('CHANNEL_PERMISSION_HOLD')
            before = rest.request('GET', f'/applications/{TRIAL}/guilds/{GUILD}/commands')
            global_commands = rest.request('GET', f'/applications/{TRIAL}/commands')
            save(root / 'commands-before.json', before, exclusive=True)
            save(root / 'global-commands-before.json', global_commands, exclusive=True)
            registration_plan(production_application_id=PRODUCTION, trial_application_id=TRIAL, guild_id=GUILD,
                              existing_commands=before + global_commands)
            payload = command_payload(group, tree)
            save(root / 'create-attempted.json', {'payload_sha256': hashlib.sha256(json.dumps(payload, sort_keys=True).encode()).hexdigest()}, exclusive=True)
            created = rest.request('POST', f'/applications/{TRIAL}/guilds/{GUILD}/commands', payload)
            if not isinstance(created, dict) or not str(created.get('id', '')).isdigit():
                raise ValueError('CREATE_RESULT_HOLD')
            # 반환 ID는 후속 검증보다 먼저 보존하여 cleanup 대상이 사라지지 않게 합니다.
            save(root / 'registered.json', created, exclusive=True)
            if created.get('name') != NAME or str(created.get('application_id')) != str(TRIAL):
                raise ValueError('CREATED_COMMAND_SCOPE_HOLD')
            result.update(status='READY_FOR_MOBILE_FAKE_TRIAL', command_id=created['id'], owner_permissions_pass=True,
                          bot_permissions_pass=True, gateway='PASS', actual_models=0, host_pid=__import__('os').getpid())
            save(root / 'ready.json', result, exclusive=True)
        except Exception as exc:
            result.update(status='PREFLIGHT_OR_REGISTRATION_HOLD', error_type=type(exc).__name__)
            result['error_code'] = exc.code if type(getattr(exc, 'code', None)) is int else None
            result['http_status'] = exc.status if type(getattr(exc, 'status', None)) is int else None
            stop.set()

    @client.event
    async def on_disconnect():
        if result['status'] == 'READY_FOR_MOBILE_FAKE_TRIAL' and not stop.is_set():
            result['status'] = 'GATEWAY_DISCONNECTED_HOLD';stop.set()

    async def watch():
        while not stop.is_set():
            if (root / 'stop-request').exists() or time.monotonic() >= deadline:
                result['stop_reason'] = 'OWNER_STOP' if (root / 'stop-request').exists() else 'EXPIRED'
                stop.set();break
            # 실제 사용량이 아닌 고정 Fake fixture입니다. API 조회나 승인 갱신은 없습니다.
            now = time.time()
            controller.quota.observe(now, [{'minutes': 300, 'used_percent': 0, 'resets_at': now + 86400},
                                           {'minutes': 10080, 'used_percent': 0, 'resets_at': now + 604800}])
            with relay.store.tx() as db:
                failed = db.execute("SELECT 1 FROM trial_events WHERE state='UNKNOWN_NO_RETRY'").fetchone()
                failed = failed or db.execute("SELECT 1 FROM tasks WHERE state='NOT_VERIFIED'").fetchone()
                events = db.execute('SELECT id,state,message_id FROM trial_events').fetchall()
            save(root / 'mobile-observed.json', {'fake_executor_calls': len(relay.executor.calls),
                 'trial_events': [dict(x) for x in events], 'model_calls': 0, 'official_usage_reads': 0})
            if failed:
                result['status'] = 'WORK_OR_REPORT_HOLD';stop.set();break
            await asyncio.sleep(1)

    start_task = asyncio.create_task(client.start(token, reconnect=False))
    watch_task = asyncio.create_task(watch())
    stop_task = asyncio.create_task(stop.wait())
    try:
        done, _ = await asyncio.wait([start_task, stop_task], return_when=asyncio.FIRST_COMPLETED)
        if start_task in done and not stop.is_set():
            result['status'] = 'GATEWAY_ENDED_HOLD'
            # 예외 원문을 저장하지 않고 태스크를 회수합니다.
            try: start_task.result()
            except Exception as exc: result['error_type'] = type(exc).__name__
    finally:
        stop.set()
        controller.recover()  # 진행 중 항목만 결과 불명 처리하고 소비·완료 근거는 보존합니다.
        watch_task.cancel();stop_task.cancel()
        try: await asyncio.wait_for(client.close(), 5)
        except Exception: result['close_status'] = 'HOLD'
        for task in (start_task, watch_task, stop_task):
            if not task.done(): task.cancel()
        await asyncio.gather(start_task, watch_task, stop_task, return_exceptions=True)
        result['fake_executor_calls'] = len(relay.executor.calls)
        try: result['cleanup'] = cleanup(root, rest)
        except Exception as exc: result['cleanup'] = {'status': 'CLEANUP_HOLD_NO_RETRY', 'error_type': type(exc).__name__}
        if result['status'] == 'READY_FOR_MOBILE_FAKE_TRIAL' and result.get('stop_reason'):
            result['status'] = 'STOPPED_' + result['stop_reason']
        save(root / 'summary.json', result)
    return result


def main():
    logging.disable(logging.CRITICAL)
    parser = argparse.ArgumentParser()
    parser.add_argument('--root', required=True)
    parser.add_argument('--cleanup-only', action='store_true')
    args = parser.parse_args()
    root = Path(args.root).resolve()
    config = read_json(root / 'run-config.json')
    if config['trial_application_id'] != TRIAL or config['production_application_id'] != PRODUCTION:
        raise ValueError('CONFIG_APPLICATION_HOLD')
    for relative, expected in config['package_sha256'].items():
        if hashlib.sha256((root / 'Package' / relative).read_bytes()).hexdigest() != expected:
            raise ValueError('PACKAGE_SHA_HOLD')
    sys.path.insert(0, str(root / 'Package/Tools/DiscordAICollab'))
    sys.path.insert(1, config['sdk_site_packages'])
    if not args.cleanup_only:
        save(root / 'launch-consumed.json', {'started_kst': datetime.now(KST).isoformat()}, exclusive=True)
    token = token_from_own_dpapi()
    rest = Rest(token, root)
    try:
        if not owner_matches(rest.request('GET', '/oauth2/applications/@me')):
            raise ValueError('APP_OWNER_HOLD')
        if args.cleanup_only:
            return cleanup(root, rest)
        return asyncio.run(serve(root, config, rest, token))
    finally:
        rest.token = None
        token = None


if __name__ == '__main__':
    try:
        output = main()
        print(json.dumps({'status': output['status'], 'model_calls': 0, 'official_usage_reads': 0}))
    except Exception as exc:
        print(json.dumps({'status': 'HOST_HOLD_NO_RETRY', 'error_type': type(exc).__name__, 'model_calls': 0, 'official_usage_reads': 0}))
        raise SystemExit(2)
