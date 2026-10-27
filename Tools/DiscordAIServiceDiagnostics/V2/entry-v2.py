"""보호된 V2 Host/Worker의 고정 진입점. 환경변수나 wire SID를 신뢰하지 않습니다."""
import argparse
import json
from pathlib import Path
import sqlite3
import sys

ROOT=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(Path(__file__).resolve().parent))
from v2_contract import need, strict, verify_install, STATE
from v2_runtime import Runtime, safe_hold
from v2_worker import Worker

def fake_value(db,request,epoch,budget,approval_id):
    need(type(budget) is int and 1000<=budget<=80000)
    with sqlite3.connect(db.resolve().as_uri()+'?mode=ro',uri=True) as conn:
        need(conn.execute('SELECT epoch,approval FROM meta').fetchall()==[(epoch,approval_id)])
        need(conn.execute('SELECT state FROM calls WHERE request=?',(request,)).fetchone()==('RUNNING',))
    return {'schema':2,'request_id':request,'epoch':epoch,'status':'FAKE_ONLY_COMPLETE','model_calls':0,'usage_reads':0,'discord_posts':0,'automatic_retries':0}

def main(action,raw,manifest_sha,binding_sha):
    m,b=verify_install(ROOT,manifest_sha,binding_sha)
    import importlib.util
    spec=importlib.util.spec_from_file_location('v2_native_identity',Path(__file__).with_name('service-token-metadata.py'));native=importlib.util.module_from_spec(spec);spec.loader.exec_module(native)
    need(native.native_identity()=={'sid':b['worker_sid'],'session':0,'logon':4,'admin':False,'elevated':False})
    db=Path(STATE)/'broker-v2.sqlite'
    if action=='fake_worker':
        p=strict(raw);need(set(p)=={'request_id','epoch','budget_ms'} and p['epoch']==m['epoch'])
        return fake_value(db,p['request_id'],p['epoch'],p['budget_ms'],m['approval_id'])
    runtime=Runtime(db,m,b,Worker(ROOT,manifest_sha,binding_sha),initialize=action=='initialize')
    if action=='initialize':return {'schema':2,'status':'READY_FAKE_ONLY','epoch':m['epoch']}
    envelope=strict(raw);need(set(envelope)=={'peer','message'})
    # peer는 C#이 실제 pipe 토큰에서 만든 envelope이며, 직접 실행도 Worker OS 토큰이 필수입니다.
    return runtime.dispatch(envelope['peer'],envelope['message'])

if __name__=='__main__':
    p=argparse.ArgumentParser();p.add_argument('--action',choices=['initialize','dispatch','fake_worker'],required=True);p.add_argument('--manifest-sha',required=True);p.add_argument('--binding-sha',required=True);a=p.parse_args()
    try:
        raw=sys.stdin.buffer.read(32769);need(len(raw)<=32768)
        print(json.dumps(main(a.action,raw.decode('utf8'),a.manifest_sha,a.binding_sha),allow_nan=False))
    except Exception:print(json.dumps(safe_hold()))
