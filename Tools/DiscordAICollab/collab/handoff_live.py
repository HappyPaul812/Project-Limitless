"""허가된 채널에 모의 검증 안내/결과만 전송합니다. 사용자 Slash를 위조하지 않습니다."""
import asyncio
import json
import os
import logging
from pathlib import Path
import discord
from .core import Store, Identity, Denied
from .handoff_rehearsal import KEY, prepare, advance


async def main():
    root=Path('data/phase2')
    owners=[int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')]
    guild=int(os.environ['LIMITLESS_GUILD_ID'])
    channels=[int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')]
    store=Store(root/'relay.sqlite',owners,guild,channels)
    owner=Identity(owners[0],guild,channels[0])
    with store.tx() as db:
        prior=db.execute('SELECT id FROM tasks WHERE event_key=?',(f'{guild}:{owners[0]}:{KEY}',)).fetchone()
        foreign=db.execute("SELECT COUNT(*) FROM tasks WHERE state IN ('TRIAGED','RUNNING')").fetchone()[0]
    if foreign:
        raise Denied('기존 실행 중 업무 보호: 중단')
    task=prepare(store,owner)
    if os.environ.get('LIMITLESS_HANDOFF_FINISH')=='1':
        with store.tx() as db:
            handed=db.execute("SELECT COUNT(*) FROM audit WHERE kind='COMMAND_HANDOFF' AND task=?",(task,)).fetchone()[0]
        if handed:
            result=advance(store,owner,task)
        else:
            result={'status':'WAITING_FOR_USER_HANDOFF','mode':'MOCK_NO_AI',
                    'task_id':task,'ai_calls':0,'art_approved':False,'product_changes':0}
        with store.tx() as db:
            kinds={r[0] for r in db.execute("SELECT kind FROM audit WHERE kind LIKE 'DISCORD_REPLY_SENT_%'")}
        result['discord_reply_receipts']={name:('PASS' if 'DISCORD_REPLY_SENT_'+name.upper() in kinds else 'NOT_VERIFIED') for name in ('status','pending','handoff')}
        summary=(f"{result['pass']} PASS / {result['fail']} FAIL. 리뉴얼→Codex→기획→바울 대표 승인 대기."
                 if handed else f'실제 handoff 실행 대기: {task}. 로컬 복제 큐8점검 PASS·회귀1 PASS; 실제 인계 완료 아님.')
        text=f"[MOCK 검증 결과] {summary} AI 호출0·미술 승인0·제품 변경0. 메인 아트는 라우팅 설정만 확인. 실제 Slash 응답: {result['discord_reply_receipts']}"
        (root/'handoff-live-result.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    else:
        text=f'[MOCK ONLY / 인계 검증 1건] 모의 리뉴얼 보고 준비 완료. 바울 대표 계정에서 /limitless handoff task_id:{task} 를 실행해 주세요. 이어서 /limitless status 와 /limitless pending 확인. 실제 AI·아트 제작·미술 승인·제품 적용 없음.'
    logging.getLogger('discord').disabled=True
    async with discord.Client(intents=discord.Intents.none()) as client:
        await client.login(os.environ['DISCORD_BOT_TOKEN'])
        channel=await client.fetch_channel(channels[0])
        if not isinstance(channel,discord.TextChannel) or channel.guild.id!=guild:
            raise Denied('채널 범위 오류')
        message=await channel.send(text,allowed_mentions=discord.AllowedMentions.none())
        print(json.dumps({'status':'MESSAGE_SENT','task_id':task,'message_id':str(message.id),'ai_calls':0}))


if __name__=='__main__':
    try:
        asyncio.run(asyncio.wait_for(main(),45))
    except Exception:
        print('HANDOFF_NOT_VERIFIED: 비밀 없는 장부/권한/사용자 handoff 실행 여부를 확인하세요.')
        raise SystemExit(2)
