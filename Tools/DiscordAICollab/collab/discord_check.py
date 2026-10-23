"""최대45초 연결·명령 등록·고정 메시지1개 검사. AI 호출이나 제품 적용은 없습니다."""
from __future__ import annotations

import argparse
import asyncio
import json
import logging
import os
from pathlib import Path

import discord

from .bot import create_bot
from .core import Denied, Store
from .intake import safe_root
from .phase2 import discord_preflight

EXPECTED={'status','task','handoff','pending','approve','reject','pause','resume','budget'}
MESSAGE='[LIMITLESS AI Team] 연결 테스트 완료. 승인·비용 정책 유지 / AI 호출 0 / 제품 변경 0.'


async def check_connection(data_root,timeout=45,*,environ=None,factory=create_bot):
    env=os.environ if environ is None else environ
    preflight=discord_preflight(env)
    result={'status':preflight['status'],'gateway':'NOT_VERIFIED','commands':'NOT_VERIFIED',
            'message':'NOT_VERIFIED','ai_calls':0,'product_changes':0,
            'token_logged':False,'slash_interactions':'NOT_VERIFIED'}
    if preflight['status']!='LOCAL_CONFIG_READY':
        return result
    if not 1<=timeout<=45:
        raise Denied('연결 검증 제한은 최대45초입니다')
    root=safe_root(data_root)
    owners={int(x) for x in env['LIMITLESS_OWNER_IDS'].split(',')}
    guild=int(env['LIMITLESS_GUILD_ID'])
    channels=[int(x) for x in env['LIMITLESS_CHANNEL_IDS'].split(',')]
    store=Store(root/'relay.sqlite',owners,guild,set(channels))
    client=factory(store,root)
    logger=logging.getLogger('discord'); logger.addHandler(logging.NullHandler()); logger.propagate=False
    connection=None; ready=None
    try:
        async with client:
            async def exercise():
                nonlocal connection,ready
                connection=asyncio.create_task(client.start(env['DISCORD_BOT_TOKEN'],reconnect=False))
                ready=asyncio.create_task(client.verified_ready.wait())
                done,_=await asyncio.wait({connection,ready},return_when=asyncio.FIRST_COMPLETED)
                if connection in done:
                    connection.result()
                    if not client.verified_ready.is_set():
                        raise Denied('DISCONNECTED_BEFORE_READY')
                await ready
                result['gateway']='PASS'
                registered=await client.tree.fetch_commands(guild=discord.Object(id=guild))
                group=next((c for c in registered if c.name=='limitless'),None)
                names={x['name'] for x in group.to_dict().get('options',[])} if group else set()
                if names!=EXPECTED:
                    raise Denied('COMMAND_CONTRACT_MISMATCH')
                result.update(commands='PASS',command_count=len(names))
                channel=await client.fetch_channel(channels[0])
                if getattr(getattr(channel,'guild',None),'id',None)!=guild or not isinstance(channel,discord.TextChannel):
                    raise Denied('CHANNEL_GUILD_OR_TYPE_MISMATCH')
                message=await channel.send(MESSAGE,allowed_mentions=discord.AllowedMentions.none())
                result.update(message='PASS',message_id=str(message.id),status='PASS')
                with store.tx() as db:
                    store.audit(db,'DISCORD_BOUNDED_CONNECTION_MESSAGE_SENT',actor='discord_check')
            await asyncio.wait_for(exercise(),timeout=timeout)
    except asyncio.TimeoutError:
        result.update(status='NOT_VERIFIED',reason='TIMEOUT_NO_RETRY')
    except discord.LoginFailure:
        result.update(status='FAIL',reason='TOKEN_REJECTED')
    except discord.Forbidden:
        result.update(status='FAIL',reason='GUILD_OR_CHANNEL_PERMISSION_DENIED')
    except discord.NotFound:
        result.update(status='FAIL',reason='GUILD_OR_CHANNEL_NOT_FOUND')
    except Denied as exc:
        allowed={'COMMAND_CONTRACT_MISMATCH','CHANNEL_GUILD_OR_TYPE_MISMATCH','DISCONNECTED_BEFORE_READY'}
        reason=str(exc) if str(exc) in allowed else 'COMMAND_SYNC_SAFETY_STOP'
        result.update(status='FAIL',reason=reason)
    except Exception:
        result.update(status='NOT_VERIFIED',reason='CONNECTION_FAILED_NO_RAW_LOG')
    finally:
        # 검사 후 연결을 닫고 다시 로그인하지 않습니다. 미술·비용 승인 상태는 바꾸지 않습니다.
        if not client.is_closed():
            try: await client.close()
            except Exception: pass
        for task in (connection,ready):
            if task is not None and not task.done(): task.cancel()
        await asyncio.gather(*(t for t in (connection,ready) if t is not None),return_exceptions=True)
    return result


def main():
    parser=argparse.ArgumentParser(description='Discord bounded live connection test')
    parser.add_argument('--data',default=str(Path(__file__).parent.parent/'data/phase2'))
    parser.add_argument('--timeout',type=int,default=45)
    args=parser.parse_args()
    root=safe_root(args.data)
    result=asyncio.run(check_connection(root,args.timeout))
    (root/'discord-live-test.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps(result,ensure_ascii=False,indent=2))
    return 0 if result['status']=='PASS' else 2


if __name__=='__main__':
    raise SystemExit(main())
