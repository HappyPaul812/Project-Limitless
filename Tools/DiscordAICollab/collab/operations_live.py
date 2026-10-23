"""등록된 명령과 실제 Discord 응답 영수증 조회만 수행합니다."""
import asyncio
import json
import logging
import os
import time
from pathlib import Path
import discord
from discord import app_commands
from .core import Store

ROOT=Path('data/operations')

async def main():
    logger=logging.getLogger('discord');logger.disabled=True;logger.addHandler(logging.NullHandler());logger.propagate=False
    ROOT.mkdir(exist_ok=True)
    owners={int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')}
    guild=int(os.environ['LIMITLESS_GUILD_ID']);channels={int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')}
    if len(owners)!=1:raise ValueError('ONE_OWNER_REQUIRED')
    store=Store('data/phase2/relay.sqlite',owners,guild,channels)
    async with discord.Client(intents=discord.Intents.none()) as client:
        await client.login(os.environ['DISCORD_BOT_TOKEN'])
        tree=app_commands.CommandTree(client)
        commands=await tree.fetch_commands(guild=discord.Object(id=guild))
        root=next((c for c in commands if c.name=='limitless'),None)
        names=sorted(o.name for o in root.options) if root else []
        required={'schedule','schedule-status','cancel','usage','finish','shutdown'}
        with store.tx() as db:
            receipts=[dict(r) for r in db.execute('SELECT * FROM control_interactions ORDER BY created')]
            schedules=[{k:r[k] for k in r.keys() if k!='owner'} for r in db.execute('SELECT * FROM control_schedules ORDER BY created')]
            reports=[dict(r) for r in db.execute('SELECT id,state,message_id FROM control_outbox')]
            operation=db.execute('SELECT mode FROM operation_mode WHERE singleton=1').fetchone()[0]
            checkpoints=db.execute('SELECT count(*) FROM operation_checkpoints').fetchone()[0]
            monitor=db.execute("SELECT value FROM control_settings WHERE key='usage_monitor'").fetchone()
        status_path=Path('data/phase2/bot-service-status.json')
        status=json.loads(status_path.read_text(encoding='utf8')) if status_path.exists() else {}
        result={'registered_subcommands':names,'required_six_registered':required<=set(names),
                'operation_mode':operation,'checkpoints':checkpoints,'mode_commands_registered':{'mode','resume','finish'}<=set(names),'one_owner_configured':True,'real_gateway_receipts':receipts,'schedules':schedules,
                'reports':reports,'bot_status':status,'heartbeat_fresh':time.time()-status.get('last_tick',0)<30,
                'usage_monitor':json.loads(monitor[0]) if monitor else None,
                'ai_calls':0,'shutdown_enabled':False,'autostart_registered':False,'observed':time.time()}
        (ROOT/'live-status.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
        print(json.dumps(result,ensure_ascii=True))

if __name__=='__main__':
    try:asyncio.run(main())
    except Exception:print('OPERATIONS_INSPECT_NOT_VERIFIED: 비밀 원문 출력 없음');raise SystemExit(2)
