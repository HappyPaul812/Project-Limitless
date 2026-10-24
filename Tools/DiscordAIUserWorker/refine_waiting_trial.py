"""미승인 신규 슬롯 하나에 추가 업무 금지·검증 성공 게시 조건을 고정합니다. 실행 승인은 없습니다."""
import ast
import difflib
import hashlib
import json
from pathlib import Path
import sqlite3
import sys
import time

TOOLS=Path(__file__).resolve().parent.parent
sys.path.insert(0,str(TOOLS/'DiscordAICollab'))
from collab.real_roles import digest


def main():
    out=TOOLS/'DiscordAICollab/data/user-worker-trial'
    plan=json.loads((out/'plan.json').read_text(encoding='utf8'))
    contract=json.loads((out/'contract.json').read_text(encoding='utf8'))
    prepared=json.loads((out/'prepared.json').read_text(encoding='utf8'))
    if plan['slot']!='USER_WORKER_PLANNING_READONLY_20261024_01':raise ValueError('EXACT_NEW_SLOT_REQUIRED')
    source=Path(__file__).resolve().parent/'durable_queue.py'
    base=source.read_text(encoding='utf8')
    old="            if state == 'DONE' and result.get('task_id'):"
    new="            if state == 'DONE' and result.get('task_id') and chosen.get('auto_handoff', True):"
    if base.count(old)!=1:raise ValueError('HANDOFF_ANCHOR_CHANGED')
    candidate=base.replace(old,new,1)
    anchor="                if state != 'DONE':"
    publish='''                if state == 'DONE' and chosen.get('publish_verified') is True:
                    from collab.real_roles import discord_text
                    db.execute('INSERT OR IGNORE INTO control_outbox VALUES(?,?,?,?)',
                               ('user-worker:'+chosen['sha256']+':VERIFIED',discord_text(result),'PENDING',None))
'''
    if candidate.count(anchor)!=1:raise ValueError('OUTBOX_ANCHOR_CHANGED')
    candidate=candidate.replace(anchor,publish+anchor,1)
    ast.parse(candidate)
    target=source.parent/'candidates/durable_queue.py'
    target.write_text(candidate,encoding='utf8')
    (target.parent/'durable_queue.diff').write_text(''.join(difflib.unified_diff(base.splitlines(True),candidate.splitlines(True),fromfile='APPROVED/DiscordAIUserWorker/durable_queue.py',tofile='CANDIDATE/DiscordAIUserWorker/durable_queue.py',n=0)),encoding='utf8')
    worker={'active_path':str(source.resolve()),'active_sha256':hashlib.sha256(source.read_bytes()).hexdigest(),
            'candidate_path':str(target.resolve()),'candidate_sha256':hashlib.sha256(target.read_bytes()).hexdigest(),'deployed':False,
            'changes':['해당 계획 auto_handoff=false이면 추가 인계 금지','publish_verified=true 계획의 PASS 결과만 기존 outbox 게시']}
    contract['worker_code']=worker
    old_plan_sha=plan['sha256']
    contract_sha=digest(contract)
    plan.update(auto_handoff=False,publish_verified=True,contract_sha256=contract_sha)
    plan['sha256']=digest({k:v for k,v in plan.items() if k!='sha256'})
    db=sqlite3.connect((TOOLS/'DiscordAICollab/data/phase2/relay.sqlite').as_uri()+'?mode=rw',uri=True,timeout=15,isolation_level=None)
    try:
        db.execute('BEGIN IMMEDIATE')
        row=db.execute('SELECT value FROM settings WHERE key=?',(plan['slot'],)).fetchone()
        pending=json.loads(row[0]) if row else {}
        if pending.get('state')!='WAITING_FOR_OWNER_APPROVAL' or pending.get('sha256')!=old_plan_sha:raise ValueError('WAITING_PLAN_CHANGED')
        job=db.execute('SELECT state FROM user_worker_jobs WHERE id=?',(old_plan_sha,)).fetchone()
        if not job or job[0]!='WAITING_FOR_OWNER_APPROVAL':raise ValueError('WAITING_JOB_REQUIRED')
        pending.update(sha256=plan['sha256'],contract_sha256=contract_sha)
        db.execute('UPDATE settings SET value=? WHERE key=?',(json.dumps(pending),plan['slot']))
        db.execute('UPDATE user_worker_jobs SET id=?,plan=? WHERE id=?',(plan['sha256'],json.dumps(plan),old_plan_sha))
        db.execute('UPDATE user_worker_checkpoints SET job=? WHERE job=?',(plan['sha256'],old_plan_sha))
        db.execute('INSERT INTO audit(at,kind,task,actor) VALUES(?,?,?,?)',(time.time(),'UNAPPROVED_TRIAL_SCOPE_FINALIZED',plan['sha256'],'LOCAL_PREPARATION_ONLY'))
        db.commit()
    except BaseException:
        db.rollback();raise
    finally:db.close()
    prepared.update(plan_sha256=plan['sha256'],contract_sha256=contract_sha,worker_code=worker)
    for name,value in (('plan.json',plan),('contract.json',contract),('prepared.json',prepared)):
        (out/name).write_text(json.dumps(value,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps({'status':'WAITING_FOR_OWNER_APPROVAL','slot':plan['slot'],'plan_sha256':plan['sha256'],
                      'contract_sha256':contract_sha,'worker_code':worker,'actual_ai_calls':0},ensure_ascii=False,indent=2))


if __name__=='__main__':main()
