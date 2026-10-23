"""바울 대표가 승인한 고정 PlanningAgent 계획1회만 실행하는 운영 진입점입니다."""
import json
import os
import shutil
from pathlib import Path
from .core import Store,Identity,Denied
from .real_roles import SLOT,grant_once,run_once,validate

APPROVED_SHA='e9e0e683f43456f67b7468e014699221e3c563eb63f9ba4257a466b3dbba0b38'


def main():
    root=Path('data/ai-ready')
    plan=json.loads((root/'PlanningAgent.plan.json').read_text(encoding='utf8'))
    validate(plan)
    if plan['sha256']!=APPROVED_SHA or plan['role']!='PlanningAgent':
        raise Denied('승인된 PlanningAgent 계획 SHA 불일치')
    owners=[int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')]
    guild=int(os.environ['LIMITLESS_GUILD_ID'])
    channels=[int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')]
    store=Store('data/phase2/relay.sqlite',set(owners),guild,channels)
    owner=Identity(owners[0],guild,channels[0])
    with store.tx() as db:
        if db.execute('SELECT 1 FROM settings WHERE key=?',(SLOT,)).fetchone():
            raise Denied('승인 슬롯 이미 존재: 자동 재실행/재승인 금지')
    executable=shutil.which('codex')
    if not executable:
        raise Denied('CLI_NOT_INSTALLED: 추론0')
    grant_once(store,owner,plan)
    result=run_once(store,owner,plan,root/'planning-approved-empty',executable)
    (root/'planning-actual-result.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps(result,ensure_ascii=False,indent=2))
    return 0 if result['status']=='PASS' else 2


if __name__=='__main__':
    try:
        raise SystemExit(main())
    except Denied as exc:
        print(str(exc))
        raise SystemExit(2)
    except Exception:
        print('PLANNING_ONCE_NOT_VERIFIED: 예외 원문은 출력하지 않으며 자동 재실행하지 않습니다.')
        raise SystemExit(2)
