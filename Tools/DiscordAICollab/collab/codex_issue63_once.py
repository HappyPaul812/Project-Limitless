"""바울 대표 승인 CodexAgent Issue63 문서 요약1회. Unity/제품 검수가 아닙니다."""
import json
import os
import shutil
from pathlib import Path
from .core import Store,Identity,Denied
from .real_roles import CODEX_SLOT,grant_once,run_once,validate


def main():
    root=Path('data/ai-ready')
    plan=json.loads((root/'CodexAgent.issue63.json').read_text(encoding='utf8'));validate(plan)
    manifest=json.loads((root/'codex-issue63-manifest.json').read_text(encoding='utf8'))
    if plan['slot']!=CODEX_SLOT or plan['role']!='CodexAgent' or plan.get('issue')!=63 or plan['sha256']!=manifest['plan_sha256']:
        raise Denied('CodexAgent Issue63 승인 계획 불일치')
    owners=[int(x) for x in os.environ['LIMITLESS_OWNER_IDS'].split(',')]
    guild=int(os.environ['LIMITLESS_GUILD_ID']);channels=[int(x) for x in os.environ['LIMITLESS_CHANNEL_IDS'].split(',')]
    store=Store('data/phase2/relay.sqlite',set(owners),guild,channels);owner=Identity(owners[0],guild,channels[0])
    with store.tx() as db:
        if db.execute('SELECT 1 FROM settings WHERE key=?',(CODEX_SLOT,)).fetchone():
            raise Denied('이번 Codex 승인 슬롯은 이미 존재합니다. 자동 재실행하지 않습니다')
    executable=shutil.which('codex')
    if not executable:raise Denied('CLI_NOT_INSTALLED')
    grant_once(store,owner,plan)
    result=run_once(store,owner,plan,root/'codex-issue63-empty',executable)
    (root/'codex-issue63-result.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps(result,ensure_ascii=True,indent=2))
    return 0 if result['status']=='PASS' else 2


if __name__=='__main__':
    try:raise SystemExit(main())
    except Denied as exc:
        print(str(exc));raise SystemExit(2)
    except Exception:
        print('CODEX_ISSUE63_NOT_VERIFIED: 원문/비밀을 출력하지 않습니다. 자동 재시도 금지.');raise SystemExit(2)
