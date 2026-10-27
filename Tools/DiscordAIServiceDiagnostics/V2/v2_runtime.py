"""새 V2 장부의 단발 FAKE_ONLY 중개. AI/Discord/운영 DB 모듈은 import하지 않습니다."""
import json
import math
import re
import sqlite3
import time
from v2_contract import need

def safe_hold(): return {'schema':2,'status':'HOLD','reason':'V2_REQUEST_OR_WORKER_HOLD','model_calls':0,'usage_reads':0,'discord_posts':0,'automatic_retries':0}

def project_worker(value, request, epoch, clean, elapsed):
    keys = {'schema','request_id','epoch','status','model_calls','usage_reads','discord_posts','automatic_retries','job'}
    need(type(value) is dict and set(value) == keys and type(value['schema']) is int and value['schema'] == 2)
    need(value['request_id'] == request and value['epoch'] == epoch and value['status'] == 'FAKE_ONLY_COMPLETE')
    need(all(type(value[k]) is int and value[k] == 0 for k in ('model_calls','usage_reads','discord_posts','automatic_retries')))
    j=value['job']; need(type(j) is dict and set(j)=={'exit_code','active_processes','cleanup_verified','timed_out'})
    need(type(j['exit_code']) is int and j['exit_code']==0 and type(j['active_processes']) is int and j['active_processes']==0 and j['cleanup_verified'] is True and j['timed_out'] is False)
    need(clean is True and 0 <= elapsed <= 90)
    return {'schema':2,'request_id':request,'epoch':epoch,'status':'FAKE_ONLY_COMPLETE_NOT_AI_VERIFIED','model_calls':0,'usage_reads':0,'discord_posts':0,'automatic_retries':0,'job_cleanup_verified':True}

class Runtime:
    def __init__(self, db_path, manifest, bound, worker, *, utc=time.time, clock=time.monotonic, initialize=False):
        self.path,self.m,self.b,self.worker,self.utc,self.clock = str(db_path),manifest,bound,worker,utc,clock
        with self.db() as db:
            db.executescript('CREATE TABLE IF NOT EXISTS meta(epoch TEXT,approval TEXT); CREATE TABLE IF NOT EXISTS permit(request TEXT PRIMARY KEY,task_sha TEXT,expires REAL,state TEXT); CREATE TABLE IF NOT EXISTS calls(request TEXT PRIMARY KEY,result TEXT,state TEXT);')
            rows=db.execute('SELECT * FROM meta').fetchall()
            if not rows: need(initialize); db.execute('INSERT INTO meta VALUES(?,?)',(manifest['epoch'],manifest['approval_id']))
            else: need(rows == [(manifest['epoch'],manifest['approval_id'])])
            if initialize:
                # 재시작은 재실행 허가가 아닙니다. 불명 처리와 미사용 허가를 HOLD로 고정합니다.
                db.execute("UPDATE calls SET state='HOLD' WHERE state='RUNNING'")
                db.execute("UPDATE permit SET state='REVOKED' WHERE state='ARMED'")

    def db(self):
        class Tx:
            def __enter__(owner):
                owner.conn=sqlite3.connect(self.path,timeout=0,isolation_level=None)
                owner.conn.execute('PRAGMA synchronous=FULL');owner.conn.execute('BEGIN IMMEDIATE');return owner.conn
            def __exit__(owner,kind,value,trace):
                try: owner.conn.rollback() if kind else owner.conn.commit()
                finally: owner.conn.close()
        return Tx()

    def peer(self,p,admin):
        need(type(p) is dict and set(p)=={'Sid','Elevated','Administrator','Local','Integrity'})
        need(p['Local'] is True and type(p['Integrity']) is int and type(p['Elevated']) is bool and type(p['Administrator']) is bool)
        if admin: need(p['Sid']==self.b['admin_sid'] and p['Elevated'] and p['Administrator'] and p['Integrity']>=12288)
        else: need(p['Sid']==self.b['bot_sid'] and not p['Elevated'] and not p['Administrator'])

    def dispatch(self,peer,msg):
        try:
            need(type(msg) is dict)
            op=msg.get('op');need(op in ('arm','submit','result','cancel'));self.peer(peer,op=='arm')
            common={'op','request_id','epoch','approval_id'};keys=common|({'task_sha256','expires'} if op=='arm' else {'task_sha256','deadline'} if op=='submit' else set())
            need(set(msg)==keys and msg['epoch']==self.m['epoch'] and msg['approval_id']==self.m['approval_id'])
            req=msg['request_id'];need(type(req) is str and re.fullmatch('[a-f0-9]{32}',req))
            if op in ('arm','submit'): need(type(msg['task_sha256']) is str and re.fullmatch('[a-f0-9]{64}',msg['task_sha256']))
            if op=='arm':
                need(type(msg['expires']) in (int,float) and math.isfinite(msg['expires']) and self.utc()<msg['expires']<=self.utc()+300)
                with self.db() as db:
                    need(db.execute('SELECT COUNT(*) FROM permit').fetchone()[0]==0 and db.execute('SELECT COUNT(*) FROM calls').fetchone()[0]==0)
                    db.execute('INSERT INTO permit VALUES(?,?,?,?)',(req,msg['task_sha256'],msg['expires'],'ARMED'))
                return {'schema':2,'status':'ARMED','request_id':req,'epoch':self.m['epoch']}
            if op=='cancel':
                with self.db() as db: db.execute("UPDATE permit SET state='CANCELLED' WHERE request=?",(req,))
                return {'schema':2,'status':'CANCELLED','request_id':req}
            if op=='result':
                with self.db() as db: row=db.execute('SELECT result FROM calls WHERE request=?',(req,)).fetchone()
                return json.loads(row[0]) if row and row[0] else safe_hold()
            need(type(msg['deadline']) in (int,float) and math.isfinite(msg['deadline']) and self.utc()<msg['deadline']<=self.utc()+90)
            start=self.clock();deadline=start+min(90,msg['deadline']-self.utc())
            with self.db() as db:
                row=db.execute('SELECT task_sha,expires,state FROM permit WHERE request=?',(req,)).fetchone()
                need(row and row[0]==msg['task_sha256'] and row[1]>self.utc() and row[2]=='ARMED')
                need(db.execute('SELECT COUNT(*) FROM calls').fetchone()[0]==0)
                db.execute("UPDATE permit SET state='CONSUMED' WHERE request=?",(req,));db.execute('INSERT INTO calls VALUES(?,NULL,?)',(req,'RUNNING'))
            # commit後だけ固定Workerを呼びます。ファイル/argv/promptはwireから受け取りません。
            handle=None;child=None;clean=False;result=safe_hold()
            try:
                need(deadline-self.clock()>5);handle=self.worker.start(req,self.m['epoch'],deadline-2)
                while self.clock()<deadline-2:
                    with self.db() as db: state=db.execute('SELECT state FROM permit WHERE request=?',(req,)).fetchone()[0]
                    need(state=='CONSUMED')
                    child=handle.poll(min(0.05,deadline-2-self.clock()))
                    if child is not None: break
            except Exception: child=None
            finally:
                if handle is not None:
                    try: clean=handle.close(deadline) is True
                    except Exception: clean=False
            try:
                with self.db() as db: state=db.execute('SELECT state FROM permit WHERE request=?',(req,)).fetchone()[0]
                need(state=='CONSUMED');result=project_worker(child,req,self.m['epoch'],clean,self.clock()-start)
            except Exception: result=safe_hold()
            with self.db() as db:
                db.execute('UPDATE calls SET result=?,state=? WHERE request=?',(json.dumps(result),'DONE' if result['status'].startswith('FAKE_ONLY_COMPLETE') else 'HOLD',req))
                db.execute("UPDATE permit SET state='DISABLED' WHERE request=? AND state='CONSUMED'",(req,))
            return result
        except Exception: return safe_hold()
