"""향후 별도 승인된 계정 간 fake-only IPC 시험. 모델/Discord 경로 없음."""
import argparse
import hashlib
import json
from pathlib import Path
import sys
import time
import uuid

INSTALL=Path(__file__).resolve().parents[3]
sys.path.insert(0,str(INSTALL/'Code/Tools/DiscordAICollab'))
from collab.broker_host_runtime import verify_manifest,native_identity
from collab.mobile_planning_buttons import contract
from collab.session0_pipe_transport import WindowsPipeTransport


def probe(action,path):
    config=verify_manifest(INSTALL)
    if config['execution_mode']!='FAKE_ONLY':raise ValueError('FAKE_PROBE_ONLY')
    actual=native_identity()
    expected=config['admin_sid'] if action=='arm' else config['bot_sid']
    if actual['sid']!=expected or (action=='arm' and not actual['elevated']):raise ValueError('PROBE_ACTUAL_SID_HOLD')
    path=Path(path)
    if action=='prepare':
        plan={'task_id':'L-probe-'+uuid.uuid4().hex[:8],'role':'PlanningAgent','scope':contract.POLICY['scope'],
              'expected':contract.EXPECTED,'policy':contract.POLICY,'upstream':{}}
        data={'request_id':uuid.uuid4().hex,'task_sha256':contract.digest(plan),'plan':plan}
        with path.open('x',encoding='utf8') as stream:json.dump(data,stream,ensure_ascii=False)
        return {'status':'PLAN_PREPARED_NOT_PERMIT','request_id':data['request_id']}
    data=contract.strict(path.read_text(encoding='utf8'))
    pins={name:config['files_sha256']['Code/Tools/DiscordAIServiceDiagnostics/'+name] for name in ('Session0PipeBoundary.cs','session0-pipe-client.ps1')}
    transport=WindowsPipeTransport(config['pipe_name'],source_pins=pins)
    now=time.time()
    if action=='arm':
        message={'op':'arm','permit':{'request_id':data['request_id'],'task_sha256':data['task_sha256'],'expires':now+300,'policy':contract.POLICY}}
    elif action=='submit':
        ticket={**data,'task_id':data['plan']['task_id'],'owner':contract.OWNER,'application':contract.TRIAL_APP,
                'guild':contract.GUILD,'channel':contract.CHANNEL,'approved_until':now+120,'created':now,'deadline':now+90,'policy':contract.POLICY}
        ticket['ticket_sha256']=contract.digest(ticket)
        message={'op':'submit','ticket':ticket}
    else:message={'op':action,'request_id':data['request_id']}
    reply=transport.exchange(message,now+(90 if action=='submit' else 5))
    return reply.value


if __name__=='__main__':
    parser=argparse.ArgumentParser();parser.add_argument('--action',choices=['prepare','arm','submit','result','cancel'],required=True);parser.add_argument('--plan',required=True)
    args=parser.parse_args()
    try:print(json.dumps(probe(args.action,args.plan),ensure_ascii=False))
    except Exception:print('{"status":"HOLD","reason":"FAKE_IPC_PROBE_HOLD_NO_RETRY"}');raise SystemExit(2)
