"""discord.py의 정식 Gateway/Slash API만 사용합니다. 자동 채팅·세션 스크래핑은 없습니다."""
from __future__ import annotations

import asyncio
import json
import logging
import os
import uuid
from pathlib import Path

import discord
from discord import app_commands

from .core import Denied, Identity, Store
from .intake import MAX_ZIP, inspect_zip, safe_root
from .query_text import format_query
from .scheduling import Scheduler, format_control


def create_bot(store: Store, data_root, usage_monitor=None, resume_queue=None, relay_only=False):
    root = safe_root(data_root)
    scheduler = Scheduler(store)

    def receipt(kind):
        with store.tx() as db:
            store.audit(db,kind,actor='discord_adapter')

    class Relay(discord.Client):
        def __init__(self):
            super().__init__(intents=discord.Intents.none(),allowed_mentions=discord.AllowedMentions.none())
            self.tree = app_commands.CommandTree(self)
            self.verified_ready = asyncio.Event()
            self.control_timer = None
            self.stop_task = None

        async def setup_hook(self):
            guild = discord.Object(id=store.guild)
            existing = await self.tree.fetch_commands(guild=guild)
            if any(command.name != 'limitless' for command in existing):
                raise Denied('UNRELATED_COMMANDS_PRESENT: 다른 명령을 지우지 않고 연결을 중단합니다')
            self.tree.copy_global_to(guild=guild)
            commands = await self.tree.sync(guild=guild)
            names = sorted(c.name for c in self.tree.get_command('limitless').commands)
            (root/'discord-connection.json').write_text(json.dumps({
                'status':'COMMANDS_REGISTERED','root_commands':len(commands),'subcommands':names,
                'gateway':'NOT_VERIFIED','tokens_logged':False},indent=2),encoding='utf8')

            (root/'bot-stop-request').unlink(missing_ok=True)
            scheduler.recover_reports()
            if resume_queue:
                resume_queue.recover()
            self.control_timer = asyncio.create_task(self.control_loop())

        async def control_loop(self):
            while not self.is_closed():
                if (root/'bot-stop-request').exists():
                    # timer가 SDK closing task를 직접 기다리면 close()의 timer 취소가 SDK 종료까지 취소합니다.
                    # 현재 루프를 먼저 끝내고 별도 close task가 연결을 닫도록 합니다.
                    self.stop_task=asyncio.create_task(self.close())
                    return
                try:
                    if usage_monitor:
                        await usage_monitor.refresh()
                    await asyncio.to_thread(scheduler.tick)
                    if resume_queue:
                        actor=Identity(next(iter(store.owners)),store.guild,min(store.channels))
                        await asyncio.to_thread(resume_queue.tick,actor)
                    async def publish(text):
                        channel = await self.fetch_channel(min(store.channels))
                        if channel.guild.id != store.guild:
                            raise Denied('예약 보고 채널 범위 불일치')
                        message = await channel.send(text, allowed_mentions=discord.AllowedMentions.none())
                        return message.id
                    await scheduler.flush_reports(publish)
                    (root/'bot-service-status.json').write_text(json.dumps({'status':'RUNNING','last_tick':store.clock(),'ai_worker_enabled':False,'approved_readonly_resume_only':resume_queue is not None,'shutdown_enabled':False}),encoding='utf8')
                except Exception:
                    # 게이트는 모든 시작 트랜잭션에서도 재확인합니다. 보고 실패로 실행을 허용하지 않습니다.
                    receipt('CONTROL_TICK_OR_REPORT_NOT_VERIFIED')
                await asyncio.sleep(5)

        async def close(self):
            if self.control_timer and not self.control_timer.done() and self.control_timer is not asyncio.current_task():
                self.control_timer.cancel()
                try:
                    await self.control_timer
                except asyncio.CancelledError:
                    pass
            await super().close()
            (root/'bot-service-status.json').write_text(json.dumps({'status':'STOPPED','ai_calls':0}),encoding='utf8')

        async def on_ready(self):
            path = root/'discord-connection.json'
            result=json.loads(path.read_text(encoding='utf8'))
            result.update(status='CONNECTED_COMMANDS_REGISTERED',gateway='PASS')
            path.write_text(json.dumps(result,indent=2),encoding='utf8')
            receipt('DISCORD_READY')
            self.verified_ready.set()
            print('Discord 연결 및 명령 등록 완료. 실제 명령·권한 검증은 설정한 서버에서 수행하세요.',flush=True)

    bot = Relay()
    group = app_commands.Group(name='limitless',description='LIMITLESS 협업 로컬 파일럿')
    bot.tree.add_command(group)

    def identity(interaction):
        return Identity(interaction.user.id,interaction.guild_id or 0,
                        interaction.channel_id or 0,interaction.user.bot)

    async def dispatch(interaction, action, args=None):
        # 모든 명령은 서버/채널/숫자 ID를 Store에서 다시 인증합니다.
        await interaction.response.defer(ephemeral=True,thinking=True)
        try:
            result = await asyncio.to_thread(store.command,identity(interaction),str(interaction.id),action,args)
            text = format_query(action,result,store) if action in ('status','pending') else json.dumps(result,ensure_ascii=False)
            if len(text)>1800:
                text = text[:1750]+'… (로컬 장부에서 전체 확인)'
            await interaction.followup.send(text,ephemeral=True,allowed_mentions=discord.AllowedMentions.none())
            receipt('DISCORD_REPLY_SENT_'+action.upper())
        except Denied as exc:
            await interaction.followup.send(str(exc),ephemeral=True)
            receipt('DISCORD_DENIAL_REPLY_SENT')
        except Exception:
            await interaction.followup.send('처리 실패: 관리자에게 로컬 장부 확인을 요청하세요.',ephemeral=True)

    @group.command(name='status',description='상태 조회')
    async def status(interaction: discord.Interaction):
        await dispatch(interaction,'status')

    @group.command(name='task',description='JSON 작업 계약 접수; 첨부는 검역만 합니다')
    async def task(interaction: discord.Interaction, contract: str, attachment: discord.Attachment | None = None):
        await interaction.response.defer(ephemeral=True,thinking=True)
        try:
            store.authorize(identity(interaction))
            if len(contract)>20000:
                raise Denied('계약 크기 초과')
            payload = json.loads(contract)
            if not isinstance(payload,dict):
                raise Denied('JSON 객체 계약 필요')
            if attachment:
                if not 0<attachment.size<=MAX_ZIP or not attachment.filename.lower().endswith('.zip'):
                    raise Denied('첨부 ZIP 크기·형식 오류')
                # URL을 사용자가 입력하지 않으며 SDK가 반환한 Discord Attachment만 읽습니다.
                incoming = root/'inbox'
                incoming.mkdir(exist_ok=True)
                path = incoming/(uuid.uuid4().hex+'.zip')
                data = await attachment.read()
                if len(data)>MAX_ZIP:
                    raise Denied('첨부 크기 초과')
                path.write_bytes(data)
                report = await asyncio.to_thread(inspect_zip,path,root/'quarantine',payload.get('artifact_sha'))
                payload.update(artifact=report['artifact'],artifact_sha=report['sha256'])
            result = await asyncio.to_thread(store.command,identity(interaction),str(interaction.id),'task',payload)
            await interaction.followup.send(json.dumps(result,ensure_ascii=False)+' / 접수만 완료·실제 QA 미실행',ephemeral=True)
            receipt('DISCORD_REPLY_SENT_TASK')
        except (Denied,json.JSONDecodeError) as exc:
            # JSON 오류에 입력 원문이 포함되지 않게 합니다.
            await interaction.followup.send(str(exc) if isinstance(exc,Denied) else 'JSON 형식 오류',ephemeral=True)
            receipt('DISCORD_DENIAL_REPLY_SENT')
        except Exception:
            await interaction.followup.send('첨부 또는 접수 실패; 원문 비밀은 기록하지 않습니다.',ephemeral=True)

    @group.command(name='handoff',description='검수 보고 인계')
    async def handoff(interaction: discord.Interaction, task_id: str):
        await dispatch(interaction,'handoff',{'task_id':task_id})

    @group.command(name='pending',description='승인·검수·결과불명 대기')
    async def pending(interaction: discord.Interaction):
        await dispatch(interaction,'pending')

    @group.command(name='approve',description='특정 SHA·범위의 미술 승인 기록; 게임 적용 안 함')
    async def approve(interaction: discord.Interaction, task_id: str, sha: str, scope: str):
        await dispatch(interaction,'approve',{'task_id':task_id,'sha':sha,'scope':scope})

    @group.command(name='reject',description='특정 후보 반려')
    async def reject(interaction: discord.Interaction, task_id: str, sha: str, scope: str):
        await dispatch(interaction,'reject',{'task_id':task_id,'sha':sha,'scope':scope})

    @group.command(name='pause',description='전체 또는 역할 긴급 정지')
    async def pause(interaction: discord.Interaction, role: str = 'all'):
        await dispatch(interaction,'pause',{'role':role})

    @group.command(name='unpause',description='기존 수동 정지 해제; AI 재실행 승인 아님')
    async def resume(interaction: discord.Interaction, role: str = 'all'):
        await dispatch(interaction,'resume',{'role':role})

    @group.command(name='mode',description='운영 모드 조회·선택: MANUAL / AUTO_RESUME / SAFE_SHUTDOWN')
    @app_commands.choices(value=[app_commands.Choice(name=x,value=x) for x in ('MANUAL','AUTO_RESUME','SAFE_SHUTDOWN')])
    async def operating_mode(interaction: discord.Interaction,value: str | None = None):
        await control_dispatch(interaction,'mode',dict(value=value))

    @group.command(name='resume',description='A 자동 재개 모드 설정; 기존 승인·사용량·SHA 재검증 필수')
    async def auto_resume(interaction: discord.Interaction):
        await control_dispatch(interaction,'resume')

    @group.command(name='budget',description='micro-USD 호출 상한 설정; 실제 AI 실행은 별도 단계')
    async def budget(interaction: discord.Interaction, task_limit: int, day_limit: int, month_limit: int):
        await dispatch(interaction,'budget',dict(task_limit=task_limit,day_limit=day_limit,month_limit=month_limit))

    async def control_dispatch(interaction, action, args=None):
        await interaction.response.defer(ephemeral=True, thinking=True)
        try:
            store.authorize(identity(interaction))
            if relay_only and (action=='resume' or (action=='mode' and (args or {}).get('value') not in (None,'MANUAL'))):
                raise Denied('로그인 전 중계 전용: MANUAL 유지·서비스 계정 Codex 인증 미검증으로 AI HOLD')
            if action=='usage' and usage_monitor:
                await usage_monitor.refresh(force=True)
            result = await asyncio.to_thread(scheduler.command, identity(interaction),
                                             str(interaction.id), action, args)
            if relay_only and action=='usage':
                result={'status':'UNKNOWN','boot_relay_only':True}
            response = await interaction.followup.send(format_control(action, result)[:1800], ephemeral=True,
                                            allowed_mentions=discord.AllowedMentions.none(),wait=True)
            with store.tx() as db:
                db.execute('INSERT OR REPLACE INTO control_interactions VALUES(?,?,?,?,?,?,?)',
                    (str(interaction.id),action,str(response.id),1,int(bool(response.flags.ephemeral)),int(getattr(interaction,'type',None)==discord.InteractionType.application_command),store.clock()))
            receipt('DISCORD_CONTROL_REPLY_' + action.upper())
        except Denied as exc:
            await interaction.followup.send(str(exc), ephemeral=True,
                                            allowed_mentions=discord.AllowedMentions.none())
            receipt('DISCORD_CONTROL_DENIED')
        except Exception:
            await interaction.followup.send('안전 제어 처리 실패·작업 보류. 로컬 장부를 확인하세요.', ephemeral=True)

    @group.command(name='schedule', description='서울 시각 마감 예약·수정; AI·통합·PC 종료 승인 아님')
    async def schedule(interaction: discord.Interaction, until: str, cleanup_minutes: int = 10,
                       next_start: str | None = None, schedule_id: str | None = None):
        await control_dispatch(interaction, 'schedule', dict(until=until, cleanup_minutes=cleanup_minutes,
                                                           next_start=next_start, schedule_id=schedule_id))

    @group.command(name='schedule-status', description='예약·사용량 보류 상태 확인')
    async def schedule_status(interaction: discord.Interaction):
        await control_dispatch(interaction, 'schedule-status')

    @group.command(name='cancel', description='예약 및 모의 종료 요청 취소; 승인·수동 정지는 유지')
    async def cancel(interaction: discord.Interaction, schedule_id: str, release_intake: bool = False):
        await control_dispatch(interaction, 'cancel', dict(schedule_id=schedule_id,release_intake=release_intake))

    @group.command(name='usage', description='공식 관측의 5시간·주간 한도 및 회복 시각')
    async def usage(interaction: discord.Interaction):
        await control_dispatch(interaction, 'usage')

    @group.command(name='finish', description='신규 접수 차단·마감 점검 기록; Git 통합 없음')
    async def finish(interaction: discord.Interaction, schedule_id: str | None = None, until: str | None = None, cleanup_minutes: int = 10, scope: str | None = None):
        await control_dispatch(interaction, 'finish', dict(schedule_id=schedule_id,until=until,cleanup_minutes=cleanup_minutes,scope=scope))

    @group.command(name='shutdown', description='특정 예약 PC 종료 조건 모의 확인; 실제 종료 비활성')
    async def shutdown(interaction: discord.Interaction, schedule_id: str, confirm: str):
        await control_dispatch(interaction, 'shutdown', dict(schedule_id=schedule_id, confirm=confirm))

    return bot


def run_bot(data_root):
    from .phase2 import discord_preflight
    status=discord_preflight()
    if status['status']!='LOCAL_CONFIG_READY':
        raise Denied('WAITING_FOR_USER_SETUP: 바울 대표가 로컬 토큰·숫자 ID·실연결 스위치를 설정해야 합니다')
    if os.getenv('LIMITLESS_ALLOW_DISCORD')!='1':
        raise Denied('Discord 실연결은 별도 승인 후 LIMITLESS_ALLOW_DISCORD=1 설정 필요')
    try:
        owners = {int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')}
        guild = int(os.environ['LIMITLESS_GUILD_ID'])
        channels = {int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')}
        token = os.environ['DISCORD_BOT_TOKEN']
        if not token or guild<=0 or not channels or len(owners)!=1 or any(x<=0 for x in channels):
            raise ValueError()
    except (KeyError,ValueError):
        raise Denied('Discord ID·토큰 환경변수가 필요합니다') from None
    root = safe_root(data_root)
    store = Store(root/'relay.sqlite',owners,guild,channels)
    store.recover()
    # token을 예외/로그에 노출하지 않으며 discord.py의 기본 파일 로그도 끕니다.
    # SDK 내부 예외 원문에 HTTP 인증정보가 섞일 가능성을 피하고 분류만 노출합니다.
    logging.getLogger('discord').disabled=True
    logging.getLogger('discord').addHandler(logging.NullHandler())
    logging.getLogger('discord').propagate=False
    import msvcrt
    lock=(root/'bot-service.lock').open('a+b');lock.seek(0);lock.write(b'1');lock.flush();lock.seek(0)
    try:
        msvcrt.locking(lock.fileno(),msvcrt.LK_NBLCK,1)
    except OSError:
        lock.close()
        raise Denied('BOT_SERVICE_ALREADY_RUNNING') from None
    from .usage_bridge import UsageMonitor
    boot_relay=os.getenv('LIMITLESS_BOOT_RELAY_ONLY')=='1'
    monitor=None if boot_relay else UsageMonitor(Scheduler(store))
    from .resume_queue import ResumeQueue
    resume_queue=None if boot_relay else ResumeQueue(store)
    relay_only=boot_relay
    if boot_relay:
        # 서비스 계정의 공식 읽기 검증만 수행합니다. 실패해도 Discord 중계는 유지하고 AI만 HOLD합니다.
        # MANUAL/A 선택은 기존 DB·대표 명령을 따르며 부팅 자체가 모드/새 업무 승인을 만들지 않습니다.
        from .service_ai import connect_boot_ai
        monitor,resume_queue,readiness=connect_boot_ai(store,root)
        relay_only=readiness['status']!='READONLY_LINK_READY'
        if resume_queue is None:ResumeQueue(store).recover()
    try:
        create_bot(store,root,monitor,resume_queue,relay_only=relay_only).run(token,log_handler=None,reconnect=boot_relay)
    except Exception:
        raise Denied('DISCORD_CONNECTION_FAILED: 토큰·초대·서버/채널 권한을 로컬에서 확인하세요. 원문 로그는 보존하지 않습니다') from None
    finally:
        lock.close()
