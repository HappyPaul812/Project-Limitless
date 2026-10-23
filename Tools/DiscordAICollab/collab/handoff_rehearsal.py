"""모의 업무 한 건만 진행합니다. 실제 AI/아트/제품 승인과 분리합니다."""
import argparse
import json
from pathlib import Path

from .__main__ import sample
from .core import Store, Identity, ROLES, Denied

KEY = 'MOCK_DISCORD_FOLLOWUP_20261023_ONE_CASE'


def prepare(store, owner):
    payload = sample('RenewalArtAgent')
    payload['title'] = 'MOCK ONLY / Discord 후속 인계 1건'
    root = store.command(owner, KEY, 'task', payload)['task_id']
    task = store.get(root)
    if task['state'] == 'NEW':
        store.triage(root)
    if store.get(root)['state'] == 'TRIAGED':
        # 기존 대기 업무를 claim하지 않도록 이 모의 업무만 대상으로 하는 전용 DB를 씁니다.
        running = store.claim()
        if not running or running['id'] != root:
            raise Denied('모의 전용 큐 필요')
        store.finish(root, running['lease'], 'TECH_PASS', {'mode':'MOCK','external_ai_called':False})
    return root


def advance(store, owner, root):
    """실제 handoff로 만들어진 후속 작업만 마무리합니다. 반복해도 신규 root 없음."""
    for _ in range(3):
        with store.tx() as db:
            rows = list(db.execute('SELECT id,state FROM tasks WHERE root=? ORDER BY depth', (root,)))
        for row in rows:
            if row['state'] == 'NEW':
                store.triage(row['id'])
            if store.get(row['id'])['state'] == 'TRIAGED':
                running = store.claim()
                if not running or running['id'] != row['id']:
                    raise Denied('모의 큐 상태 충돌')
                store.finish(row['id'], running['lease'], 'TECH_PASS', {'mode':'MOCK','external_ai_called':False})
            if store.get(row['id'])['state'] == 'REVIEW_PENDING':
                store.auto_handoff(row['id'])
    snap = store.snapshot()
    with store.tx() as db:
        ids={r[0] for r in db.execute('SELECT id FROM tasks WHERE root=?',(root,))}
        usage=db.execute('SELECT COUNT(*) FROM usage WHERE task IN (SELECT id FROM tasks WHERE root=?)',(root,)).fetchone()[0]
    tasks = [t for t in snap['tasks'] if t['id'] in ids]
    checks = {
        'single_root_three_stages': len(tasks)==3,
        'renewal_codex_planning': [t['role'] for t in tasks]==['RenewalArtAgent','CodexAgent','PlanningAgent'],
        'owner_approval_pending': tasks[-1]['state']=='APPROVAL_PENDING',
        'no_art_approval': all(t['art']=='USER_REVIEW_PENDING' for t in tasks),
        'main_art_routes_to_codex': ROLES['NewArtAgent']['next']=='CodexAgent',
        'sfx_paused': snap['roles']['SFXAgent'],
        'no_ai_usage': usage==0,
    }
    try:
        store.command(Identity(owner.user_id+1,owner.guild_id,owner.channel_id),KEY+'deny','approve',{})
        checks['non_owner_denied']=False
    except Denied:
        checks['non_owner_denied']=True
    return {'mode':'MOCK_NO_AI','checks':checks,'tasks':tasks,
            'pass':sum(checks.values()),'fail':sum(not x for x in checks.values()),
            'main_art_execution':'NOT_VERIFIED_ROUTE_ONLY','ai_calls':0,'additional_api_cost_usd':0}


def main():
    parser=argparse.ArgumentParser()
    parser.add_argument('--data',required=True)
    args=parser.parse_args()
    root=Path(args.data); root.mkdir(parents=True,exist_ok=True)
    store=Store(root/'mock.sqlite',{1}); owner=Identity(1,1,1)
    task=prepare(store,owner)
    result=advance(store,owner,task)
    (root/'result.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps(result,ensure_ascii=False,indent=2))
    return int(result['fail']>0)


if __name__=='__main__':
    raise SystemExit(main())
