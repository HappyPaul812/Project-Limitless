"""바울 대표의 새 승인: NewArt 문서 검토1회만. 다른 역할·재시도·아트 제작 없음."""
import asyncio
import json
import logging
import os
import shutil
from pathlib import Path

import discord
from .art_roles_once import make_plan
from .core import Store,Identity,Denied
from .real_roles import NEW_ART_RETEST_SLOT,digest,validate,grant_once,run_once,discord_text


def prepare(sources,clock):
    # #65 중복 입력은 생략하고 #59 최신 및 #58 후속 결정을 사용합니다.
    sources={**sources,'comments':{**sources['comments'],'65':[]}}
    plan=make_plan('NewArtAgent',sources,clock)
    plan['slot']=NEW_ART_RETEST_SLOT
    plan['sha256']=digest({k:v for k,v in plan.items() if k!='sha256'});validate(plan)
    return plan


def execute(store,owner,plan,root,executable,runner=run_once):
    if plan['role']!='NewArtAgent' or plan['slot']!=NEW_ART_RETEST_SLOT or plan.get('issue')!=59:
        raise Denied('NewArt #59 별도 재시험 승인 범위 아님')
    validate(plan);root=Path(root);root.mkdir(parents=True,exist_ok=True)
    grant_once(store,owner,plan)
    (root/'plan.json').write_text(json.dumps(plan,ensure_ascii=False,indent=2),encoding='utf8')
    result=runner(store,owner,plan,root/'empty',executable)
    (root/'result.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    return result


def main():
    root=Path('data/ai-ready/newart-retest')
    owners=[int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')]
    guild=int(os.environ['LIMITLESS_GUILD_ID']);channels=[int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')]
    store=Store('data/phase2/relay.sqlite',set(owners),guild,channels);owner=Identity(owners[0],guild,channels[0])
    sources=json.loads(Path('data/ai-ready/newart-retest-sources.json').read_text(encoding='utf8'))
    result=execute(store,owner,prepare(sources,store.clock),root,shutil.which('codex'))
    print(json.dumps(result,ensure_ascii=True,indent=2),flush=True)
    # 실패 시 여기서 중지합니다. 보고 실패도 모델을 다시 실행하지 않습니다.
    if result['status']!='PASS' or result.get('queue_acceptance'):return 2
    logging.getLogger('discord').disabled=True
    async def publish():
        async with discord.Client(intents=discord.Intents.none()) as client:
            await client.login(os.environ['DISCORD_BOT_TOKEN']);channel=await client.fetch_channel(channels[0])
            if not isinstance(channel,discord.TextChannel) or channel.guild.id!=guild:raise Denied('CHANNEL_SCOPE_INVALID')
            message=await channel.send(discord_text(result)+'\n업무 '+result['task_id'],allowed_mentions=discord.AllowedMentions.none())
            return {'status':'MESSAGE_SENT','message_id':str(message.id),'task_id':result['task_id']}
    receipt=asyncio.run(asyncio.wait_for(publish(),45))
    (root/'discord-receipt.json').write_text(json.dumps(receipt,indent=2),encoding='utf8')
    print(json.dumps(receipt));return 0


if __name__=='__main__':
    try:raise SystemExit(main())
    except Exception:
        print('NEWART_RETEST_STOPPED_NO_RETRY: 비밀 원문 출력 없음');raise SystemExit(2)
