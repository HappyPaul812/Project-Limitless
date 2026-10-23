"""4역할 계획 생성과 준비 상태 보고 전용입니다. 모델 실행/승인 기능은 없습니다."""
import argparse
import asyncio
import json
import logging
import os
import shutil
from pathlib import Path

import discord
from .core import Store, Denied
from .real_roles import ROLE_FILES, SLOT, make_plan, discord_text
from .phase2 import cli_info, verify_cli_controls


def prepare(root):
    source=json.loads((root/'sources.json').read_text(encoding='utf8'))
    plans=[make_plan(role,source) for role in ROLE_FILES]
    for plan in plans:
        (root/(plan['role']+'.plan.json')).write_text(json.dumps(plan,ensure_ascii=False,indent=2),encoding='utf8')
    executable=shutil.which('codex')
    config=verify_cli_controls(executable,root) if executable else {'status':'NOT_VERIFIED'}
    auth=cli_info(executable) if executable else {'subscription_available':False}
    result={'status':'WAITING_FOR_OWNER_APPROVAL','actual_inference_calls':0,
            'auth':auth,'config':config,'api_calls':0,'additional_api_cost_usd':0,
            'subscription_consumption_this_probe':'NOT_EXECUTED',
            'source_refs':source['refs'],
            'plans':[{'role':p['role'],'sha256':p['sha256'],'prompt_bytes':p['prompt_bytes'],
                      'timeout_seconds':90,'expected_response':'600 Korean characters requested; not a billing token cap'} for p in plans]}
    (root/'readiness.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    return result


async def report(root, actual_result=False):
    result=json.loads((root/'readiness.json').read_text(encoding='utf8'))
    channels=[int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')]
    guild=int(os.environ['LIMITLESS_GUILD_ID'])
    owners={int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')}
    store=Store(Path('data/phase2/relay.sqlite'),owners,guild,channels)
    with store.tx() as db:
        kinds={r[0] for r in db.execute("SELECT kind FROM audit WHERE kind LIKE 'DISCORD_REPLY_SENT_%'")}
    slash={name:('PASS' if 'DISCORD_REPLY_SENT_'+name.upper() in kinds else 'NOT_VERIFIED')
           for name in ('status','pending','handoff')}
    message=('[AI 연결 준비 / 실제 추론 미실행] Slash 실제 응답 영수증 '+json.dumps(slash,ensure_ascii=False)+
             '. 4역할 GitHub 문서 스냅샷·독립 계획 준비. ChatGPT 구독 인증/CLI 설정검사 '+
             ('PASS' if result['auth']['subscription_available'] and result['config']['status']=='PASS' else 'NOT_VERIFIED')+
             '. 다음 실제 AI는 4역할 합계 읽기전용1회·90초·별도 계획 SHA 승인 필요. MOCK 결과와 REAL AI 표시 분리. '
             '현재 추론0·추가유료API0·게임변경0·미술승인0·Push/merge0. 실제 AI 연쇄호출은 비활성.')
    if actual_result:
        # 임의 모델 결과 JSON을 신뢰하지 않고 승인 실행기가 기록한 큐 근거만 게시합니다.
        with store.tx() as db:
            row=db.execute('SELECT evidence FROM tasks WHERE event_key=?',(SLOT,)).fetchone()
        if not row or not row[0]:
            raise Denied('실제 AI 결과 없음: MOCK를 REAL로 게시하지 않습니다')
        message=discord_text(json.loads(row[0]))
    logging.getLogger('discord').disabled=True
    async with discord.Client(intents=discord.Intents.none()) as client:
        await client.login(os.environ['DISCORD_BOT_TOKEN'])
        channel=await client.fetch_channel(channels[0])
        if not isinstance(channel,discord.TextChannel) or channel.guild.id!=guild:
            raise Denied('허용된 서버 텍스트 채널 필요')
        sent=await channel.send(message,allowed_mentions=discord.AllowedMentions.none())
    receipt={'status':'MESSAGE_SENT','message_id':str(sent.id),'slash_receipts':slash,'kind':'REAL_RESULT' if actual_result else 'READINESS_ONLY'}
    (root/('discord-result-report.json' if actual_result else 'discord-report.json')).write_text(json.dumps(receipt,indent=2),encoding='utf8')
    return receipt


def main():
    parser=argparse.ArgumentParser()
    parser.add_argument('action',choices=('prepare','report','result-report'))
    args=parser.parse_args(); root=Path('data/ai-ready'); root.mkdir(parents=True,exist_ok=True)
    try:
        result=prepare(root) if args.action=='prepare' else asyncio.run(asyncio.wait_for(report(root,args.action=='result-report'),45))
        print(json.dumps(result,ensure_ascii=False,indent=2))
    except Exception:
        print('AI_READINESS_NOT_VERIFIED: 비밀 없는 설정/문서/채널 상태를 확인하세요.')
        raise SystemExit(2)


if __name__=='__main__':
    main()
