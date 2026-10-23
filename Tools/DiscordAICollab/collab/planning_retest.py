"""바울 대표의 최종 재시험 승인1회. 과거 슬롯/장부는 변경하지 않습니다."""
import json
import os
import shutil
from pathlib import Path
from .core import Store,Identity,Denied
from .real_roles import RETEST_SLOT,grant_once,run_once,validate


def main():
    root=Path('data/ai-ready')
    plan=json.loads((root/'PlanningAgent.final-retest.json').read_text(encoding='utf8'))
    validate(plan)
    if plan['role']!='PlanningAgent' or plan['slot']!=RETEST_SLOT:
        raise Denied('이번 Planning 재시험 범위가 아닙니다')
    manifest=json.loads((root/'final-retest-manifest.json').read_text(encoding='utf8'))
    if plan['sha256']!=manifest['plan_sha256']:
        raise Denied('재시험 계획 SHA 불일치')
    owners=[int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')]
    guild=int(os.environ['LIMITLESS_GUILD_ID']);channels=[int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')]
    store=Store('data/phase2/relay.sqlite',set(owners),guild,channels)
    owner=Identity(owners[0],guild,channels[0])
    with store.tx() as db:
        if db.execute('SELECT 1 FROM settings WHERE key=?',(RETEST_SLOT,)).fetchone():
            raise Denied('이번 재시험 슬롯은 이미 존재합니다. 재승인/재실행하지 않습니다')
    executable=shutil.which('codex')
    if not executable:raise Denied('CLI_NOT_INSTALLED')
    grant_once(store,owner,plan)
    result=run_once(store,owner,plan,root/'final-retest-empty',executable)
    (root/'final-retest-result.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps(result,ensure_ascii=False,indent=2))
    return 0 if result['status']=='PASS' else 2


if __name__=='__main__':
    try:raise SystemExit(main())
    except Denied as exc:
        print(str(exc));raise SystemExit(2)
    except Exception:
        print('FINAL_RETEST_NOT_VERIFIED: 원문 로그/인증정보를 출력하지 않습니다. 재시도 금지.');raise SystemExit(2)
