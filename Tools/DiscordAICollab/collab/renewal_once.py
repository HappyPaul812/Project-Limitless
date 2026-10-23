"""대표 승인 RenewalArtAgent 읽기전용1회. 기존 세 역할·미사용 승인 슬롯 보호."""
import asyncio
import json
import logging
import os
import shutil
from pathlib import Path

import discord
from .art_roles_once import make_plan
from .core import Store,Identity,Denied
from .real_roles import RENEWAL_ART_SLOT,digest,validate,grant_once,run_once,discord_text


def prepare(sources,clock):
    plan=make_plan('RenewalArtAgent',sources,clock)
    plan['prompt']+='\n바울 대표라는 호칭을 사용하세요. 다음은 Issue65의 독침벌 관련 부분만 발췌한 참고 데이터입니다:\n'+json.dumps(sources['comments']['65'],ensure_ascii=False)
    plan['prompt_bytes']=len(plan['prompt'].encode())
    plan['sha256']=digest({k:v for k,v in plan.items() if k!='sha256'});validate(plan)
    return plan


def execute(store,owner,plan,root,executable,runner=run_once):
    if plan['role']!='RenewalArtAgent' or plan['slot']!=RENEWAL_ART_SLOT or plan.get('issue')!=63:
        raise Denied('Renewal Issue63 단발 승인 범위 아님')
    validate(plan);root=Path(root);root.mkdir(parents=True,exist_ok=True)
    # 이전 실패한 두 담당 묶음은 재개하지 않고 아직 미사용인 Renewal 고정 슬롯만 사용합니다.
    grant_once(store,owner,plan)
    (root/'plan.json').write_text(json.dumps(plan,ensure_ascii=False,indent=2),encoding='utf8')
    result=runner(store,owner,plan,root/'empty',executable)
    (root/'result.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    return result


def main():
    root=Path('data/ai-ready/renewal-once')
    owners=[int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')]
    guild=int(os.environ['LIMITLESS_GUILD_ID']);channels=[int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')]
    store=Store('data/phase2/relay.sqlite',set(owners),guild,channels);owner=Identity(owners[0],guild,channels[0])
    sources=json.loads(Path('data/ai-ready/renewal-once-sources.json').read_text(encoding='utf8'))
    result=execute(store,owner,prepare(sources,store.clock),root,shutil.which('codex'))
    print(json.dumps(result,ensure_ascii=True,indent=2),flush=True)
    if result['status']!='PASS' or result.get('queue_acceptance'):return 2
    logging.getLogger('discord').disabled=True
    async def publish():
        async with discord.Client(intents=discord.Intents.none()) as client:
            await client.login(os.environ['DISCORD_BOT_TOKEN']);channel=await client.fetch_channel(channels[0])
            if not isinstance(channel,discord.TextChannel) or channel.guild.id!=guild:raise Denied('CHANNEL_SCOPE_INVALID')
            sent=await channel.send(discord_text(result)+'\n업무 '+result['task_id'],allowed_mentions=discord.AllowedMentions.none())
            return {'status':'MESSAGE_SENT','message_id':str(sent.id),'task_id':result['task_id']}
    receipt=asyncio.run(asyncio.wait_for(publish(),45))
    (root/'discord-receipt.json').write_text(json.dumps(receipt,indent=2),encoding='utf8')
    print(json.dumps(receipt));return 0


if __name__=='__main__':
    try:raise SystemExit(main())
    except Exception:
        print('RENEWAL_ONCE_STOPPED_NO_RETRY: 비밀 원문 출력 없음');raise SystemExit(2)
