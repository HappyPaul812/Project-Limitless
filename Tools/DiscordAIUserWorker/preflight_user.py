"""일반 사용자 구독 상태와 공식 사용량만 읽습니다. 슬롯 승인·exec·Worker 시작은 없습니다."""
import asyncio
import hashlib
import json
from pathlib import Path
import sqlite3
import sys

TOOLS=Path(__file__).resolve().parent.parent
sys.path.insert(0,str(TOOLS));sys.path.insert(0,str(TOOLS/'DiscordAICollab'))
from DiscordAIUserWorker.durable_queue import current_user_context,own_environment
from collab.phase2 import cli_info
from collab.usage_bridge import OfficialUsage
from collab.core import Denied


def main():
    result={'status':'HOLD','actual_ai_calls':0,'mode_changed':False,'login_changed':False}
    try:
        registry=json.loads(Path('C:/ProgramData/LimitlessRelay/user-worker-integrity.json').read_text(encoding='utf-8-sig'))
        entries=[*registry['files'],*registry['approved_runtime_files'],*registry['worker_adapter_files'],*registry['user_cli_files']]
        if any(hashlib.sha256(Path(e['absolute_path']).read_bytes()).hexdigest()!=e['sha256'] for e in entries):
            raise Denied('REGISTERED_CONTENT_CHANGED')
        context=current_user_context()
        if context['sid']!=registry['user_sid'] or context['session_id']<=0 or context['is_admin']:
            raise Denied('REGISTERED_NORMAL_USER_SESSION_REQUIRED')
        result.update(account=context['name'],session_id=context['session_id'],profile=context['profile'],
                      codex_home=context['codex_home'],registered_content_checks=len(entries))
        environment=own_environment(context)
        cli=cli_info(registry['user_cli_path'],environ=environment)
        result['authentication']='PASS' if cli['subscription_available'] else 'FAIL'
        result['cli']=cli
        if not cli['subscription_available']:
            raise Denied('OFFICIAL_CHATGPT_LOGIN_NOT_CONFIRMED')
        result['usage']=asyncio.run(OfficialUsage(registry['user_cli_path'],environment=environment).read())
        result['official_usage']='PASS'
        result['status']='READONLY_PREFLIGHT_PASS'
    except Exception as exc:
        result['reason']=str(exc) if isinstance(exc,Denied) else 'READONLY_PREFLIGHT_NOT_VERIFIED'
    with sqlite3.connect((TOOLS/'DiscordAICollab/data/phase2/relay.sqlite').as_uri()+'?mode=ro',uri=True) as db:
        result['mode']=db.execute('SELECT mode FROM operation_mode WHERE singleton=1').fetchone()[0]
        result['worker_claims']=db.execute('SELECT count(*) FROM user_worker_lock').fetchone()[0]
    root=TOOLS/'DiscordAICollab/data/user-worker-trial'
    root.mkdir(exist_ok=True)
    (root/'preflight.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps(result,ensure_ascii=False,indent=2))


if __name__=='__main__':main()
