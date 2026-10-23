"""수령→검역→대기 업무→보고 outbox. AI·Unity 실행 및 미술 승인은 없습니다."""
import hashlib
import json
import os
import tempfile
from pathlib import Path
from .core import Denied
from .intake import safe_root,MAX_ZIP
from .art_inspection import inspect


class Deliveries:
    def __init__(self,store,root,base_sha,catalog):
        if len(base_sha)!=40 or any(c not in '0123456789abcdef' for c in base_sha):raise Denied('BASE_SHA_INVALID')
        self.store=store;self.root=safe_root(root);self.base_sha=base_sha;self.catalog=catalog
        self.vault=safe_root(self.root/'quarantine')
        with store.tx() as db:
            db.executescript('''
            CREATE TABLE IF NOT EXISTS art_deliveries(sha TEXT PRIMARY KEY,status TEXT,artifact TEXT,report TEXT,task_id TEXT,created REAL);
            CREATE TABLE IF NOT EXISTS art_delivery_events(event_key TEXT PRIMARY KEY,sha TEXT,result TEXT,created REAL);
            CREATE TABLE IF NOT EXISTS art_delivery_outbox(event_key TEXT PRIMARY KEY,text TEXT,status TEXT,message_id TEXT,created REAL);
            CREATE TABLE IF NOT EXISTS art_delivery_cursors(channel TEXT PRIMARY KEY,message_id TEXT);
            ''')

    def seen(self,key):
        with self.store.tx() as db:return db.execute('SELECT 1 FROM art_delivery_events WHERE event_key=?',(key,)).fetchone() is not None

    def reject_transport(self,identity,key,reason):
        self.store.authorize(identity)
        result={'status':'REJECTED','sha256':None,'task_id':None,'reason':reason,'bytes_downloaded':0}
        with self.store.tx() as db:
            old=db.execute('SELECT result FROM art_delivery_events WHERE event_key=?',(key,)).fetchone()
            if old:return json.loads(old[0])
            db.execute('INSERT INTO art_delivery_events VALUES(?,?,?,?)',(key,None,json.dumps(result),self.store.clock()))
            db.execute('INSERT INTO art_delivery_outbox VALUES(?,?,?,?,?)',(key,'[ART DELIVERY / REJECTED] 형식·크기·읽기 제한으로 다운로드/업무 등록하지 않았습니다. SHA 미수신 · 미술승인/게임적용0.','PENDING',None,self.store.clock()))
        return result

    def receive(self,identity,key,name,data,metadata=None,source='WINDOWS_INBOX'):
        self.store.authorize(identity)
        if not isinstance(key,str) or not 1<=len(key)<=256:raise Denied('EVENT_KEY_INVALID')
        if not isinstance(data,bytes) or not 0<len(data)<=MAX_ZIP:raise Denied('DELIVERY_SIZE_LIMIT')
        if metadata is not None and (not isinstance(metadata,bytes) or len(metadata)>1024*1024):raise Denied('METADATA_SIZE_OR_TYPE_INVALID')
        digest=hashlib.sha256(data).hexdigest()
        with self.store.tx() as db:
            old=db.execute('SELECT result FROM art_delivery_events WHERE event_key=?',(key,)).fetchone()
            if old:return json.loads(old[0])
        from .scheduling import gate
        with self.store.tx() as db:
            if gate(db, self.store.clock(), intake=True):
                raise Denied('SCHEDULE_HOLD: 아트 원본은 납품함에 남겨두고 신규 접수를 보류합니다')
        try:report=inspect(name,data,metadata,self.catalog,self.base_sha)
        except Denied as exc:
            label=str(exc)
            # 검사기 고정 코드만 노출하고 임의 예외·파일 내용은 기록하지 않습니다.
            if not label.replace('_','').isalnum() or not label.isascii():label='UNSAFE_DELIVERY_REJECTED'
            report={'status':'REJECTED','integrity':'TECH_FAIL','reason':label,'art':'USER_REVIEW_PENDING','unity':'NOT_VERIFIED','rights':'RIGHTS_SOURCE_PENDING'}
        except Exception:
            report={'status':'REJECTED','integrity':'NOT_VERIFIED','reason':'INSPECTION_FAILED_CLOSED','art':'USER_REVIEW_PENDING','unity':'NOT_VERIFIED','rights':'RIGHTS_SOURCE_PENDING'}
        report.update(sha256=digest,bytes=len(data),original_filename=name[:240],source=source,ai_called=False,product_applied=False)
        # 원래 파일명을 목적 경로로 사용하지 않습니다. SHA 디렉터리 원본은 덮어쓰지 않습니다.
        target=self.vault/digest
        suffix='.png' if name.lower().endswith('.png') else '.zip'
        artifact=target/('source'+suffix)
        if target.exists():
            safe_root(target)
            if artifact.is_symlink() or not artifact.is_file() or artifact.read_bytes()!=data:raise Denied('QUARANTINE_TAMPERED')
        else:
            with tempfile.TemporaryDirectory(dir=self.vault,prefix='staging-') as tmp:
                stage=Path(tmp)/'bundle';stage.mkdir();(stage/artifact.name).write_bytes(data)
                if metadata is not None:(stage/'received-metadata.json').write_bytes(metadata)
                try:os.rename(stage,target)
                except FileExistsError:
                    if artifact.is_symlink() or not artifact.is_file() or artifact.read_bytes()!=data:raise Denied('QUARANTINE_RACE_OR_TAMPER')
        with self.store.tx() as db:
            old=db.execute('SELECT result FROM art_delivery_events WHERE event_key=?',(key,)).fetchone()
            if old:return json.loads(old[0])
            prior=db.execute('SELECT task_id,status FROM art_deliveries WHERE sha=?',(digest,)).fetchone()
            if prior:
                result={'status':'DUPLICATE','sha256':digest,'task_id':prior['task_id'],'original_status':prior['status'],'new_task_created':False}
            else:
                task_id=None
                if report['status']!='REJECTED':
                    payload={'title':'FILE RECEIVED / Codex 아트 검수 요청 / '+digest[:12],
                     'role':'CodexAgent','issue':report.get('issue'),'base_sha':self.base_sha,
                     'scope':'PHASE3_REVIEW_REQUEST_ONLY:'+digest,
                     'protected_assets':['Unity','Save','UserData','Wolf','Bear','Fox','Ch2_official_monsters','approved_environment','all_original_assets'],
                     'inputs':[str(artifact),'SHA256:'+digest,'ARTIST:'+report.get('artist_role','UNKNOWN')],
                     'verification':['아트 파일·프레임·출처 검토 요청만','실제 Unity/AI 실행은 별도 승인','미술 승인은 바울 대표만'],
                     'approval_required':True,'artifact':str(artifact),'artifact_sha':digest,
                     'inspection':{'integrity':report['integrity'],'rights':report['rights'],'hold_reasons':report['hold_reasons'],
                                   'report_lookup_sha256':digest,'art':'USER_REVIEW_PENDING','unity':'NOT_VERIFIED'},
                     'artist_role':report.get('artist_role','UNKNOWN'),'delivery_source':source}
                    task_id=self.store._create(db,'art-review:'+digest,payload)
                    # 기존 MockWorker/실행기가 자동으로 집어가지 못하도록 명시적 BLOCKED 상태입니다.
                    db.execute("UPDATE tasks SET state='BLOCKED',evidence=? WHERE id=?",(json.dumps({'mode':'ART_FILE_INTAKE_ONLY','sha256':digest,'integrity':report['integrity'],'unity':'NOT_VERIFIED','art':'USER_REVIEW_PENDING','ai_called':False}),task_id))
                    self.store.audit(db,'ART_REVIEW_REQUEST_CREATED_NO_EXEC',task_id)
                db.execute('INSERT INTO art_deliveries VALUES(?,?,?,?,?,?)',(digest,report['status'],str(artifact),json.dumps(report,ensure_ascii=False),task_id,self.store.clock()))
                result={'status':report['status'],'sha256':digest,'task_id':task_id,'new_task_created':task_id is not None,'inspection':report}
            db.execute('INSERT INTO art_delivery_events VALUES(?,?,?,?)',(key,digest,json.dumps(result,ensure_ascii=False),self.store.clock()))
            text=('[ART DELIVERY / '+result['status']+'] SHA256 '+digest+'\nCodex 검수 요청: '+str(result.get('task_id') or '안전성 거부로 등록하지 않음')+
                  '\n파일 수령/형식 검사는 미술 승인이 아닙니다. USER_REVIEW_PENDING · Unity/AI 실행0 · 게임 적용0.')
            db.execute('INSERT INTO art_delivery_outbox VALUES(?,?,?,?,?)',(key,text,'PENDING',None,self.store.clock()))
            self.store.audit(db,'ART_DELIVERY_'+result['status'],result.get('task_id') or '',source)
        return result

    def report(self,digest):
        with self.store.tx() as db:
            row=db.execute('SELECT report FROM art_deliveries WHERE sha=?',(digest,)).fetchone()
            return json.loads(row[0]) if row else None

    def recover_outbox(self):
        with self.store.tx() as db:db.execute("UPDATE art_delivery_outbox SET status='UNCERTAIN' WHERE status='SENDING'")

    async def flush(self,publish):
        with self.store.tx() as db:keys=[r[0] for r in db.execute("SELECT event_key FROM art_delivery_outbox WHERE status='PENDING' ORDER BY created LIMIT 20")]
        for key in keys:
            with self.store.tx() as db:
                row=db.execute('SELECT * FROM art_delivery_outbox WHERE event_key=?',(key,)).fetchone()
                if row['status']!='PENDING':continue
                db.execute("UPDATE art_delivery_outbox SET status='SENDING' WHERE event_key=?",(key,))
            try:
                message_id=await publish(row['text'])
                with self.store.tx() as db:db.execute("UPDATE art_delivery_outbox SET status='SENT',message_id=? WHERE event_key=?",(str(message_id),key))
            except Exception:
                # 전송 성공 여부 불명은 자동 중복 게시하지 않고 운영자 확인으로 남깁니다.
                with self.store.tx() as db:db.execute("UPDATE art_delivery_outbox SET status='UNCERTAIN' WHERE event_key=?",(key,))
