"""Phase3 파일/보안/큐/outbox 테스트. 실제 AI·Unity·Discord 호출은 없습니다."""
import asyncio
import hashlib
import io
import json
import tempfile
import unittest
import zipfile
from pathlib import Path
from PIL import Image
from collab.core import Store,Identity,Denied
from collab.art_delivery import Deliveries
from collab.art_inspection import inspect,png,archive
from collab.delivery_watch import FolderWatcher,snapshot
from collab.adapters import MockWorker


def sprite():
    im=Image.new('RGBA',(16,8),(0,0,0,0));im.putpixel((4,4),(230,170,70,255));out=io.BytesIO();im.save(out,format='PNG');return out.getvalue()


def manifest(data,name='sample.png'):
    return {'artist_role':'RenewalArtAgent','issue':63,'base_sha':'a'*40,'approval_status':'USER_REVIEW_PENDING',
            'source':'synthetic test','commercial_rights':True,'license_evidence':'LICENSE.txt',
            'files':[{'path':name,'sha256':hashlib.sha256(data).hexdigest(),'dimensions':[16,8],'frames':2,'frame_size':[8,8]}]}


def bundle(files,compression=zipfile.ZIP_STORED):
    out=io.BytesIO()
    with zipfile.ZipFile(out,'w',compression=compression) as z:
        for name,data in files.items():z.writestr(name,data)
    return out.getvalue()


class DeliveryTests(unittest.TestCase):
    def setUp(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        self.tmp=tempfile.TemporaryDirectory(dir='data/tmp');self.root=Path(self.tmp.name).resolve()
        self.store=Store(self.root/'relay.sqlite',{1},clock=lambda:1792720800);self.owner=Identity(1,1,1)
        self.catalog={'names':['wolf.png','bear.png','fox.png'],'sha256':['f'*64]}
        self.engine=Deliveries(self.store,self.root/'delivery','a'*40,self.catalog)
        self.data=sprite();self.meta=json.dumps(manifest(self.data)).encode()
    def tearDown(self):self.tmp.cleanup()
    def receive(self,name='sample.png',data=None,meta=True,key='evt1'):
        return self.engine.receive(self.owner,key,name,self.data if data is None else data,self.meta if meta is True else None if meta is False else meta)
    def test_png_snapshot_hash_queue_and_pending(self):
        r=self.receive();self.assertEqual('HOLD',r['status']);self.assertEqual(hashlib.sha256(self.data).hexdigest(),r['sha256'])
        t=self.store.get(r['task_id']);self.assertEqual('BLOCKED',t['state']);self.assertEqual('USER_REVIEW_PENDING',t['art'])
        p=json.loads(t['payload']);self.assertEqual('RenewalArtAgent',p['artist_role']);self.assertEqual(63,p['issue'])
        self.assertEqual(self.data,Path(p['artifact']).read_bytes());self.assertEqual('NOT_VERIFIED',t['tech'])
    def test_newart_issue59_request_stays_pending(self):
        m=manifest(self.data);m.update(artist_role='NewArtAgent',issue=59)
        r=self.receive(meta=json.dumps(m).encode());p=json.loads(self.store.get(r['task_id'])['payload'])
        self.assertEqual('NewArtAgent',p['artist_role']);self.assertEqual(59,p['issue']);self.assertEqual('USER_REVIEW_PENDING',self.store.get(r['task_id'])['art'])
    def test_concurrent_same_sha_creates_only_one_task(self):
        from concurrent.futures import ThreadPoolExecutor
        with ThreadPoolExecutor(max_workers=2) as pool:
            futures=[pool.submit(self.receive,key=k) for k in ('parallel-a','parallel-b')]
            results=[f.result() for f in futures]
        self.assertEqual({'HOLD','DUPLICATE'},{r['status'] for r in results});self.assertEqual(1,len(self.store.snapshot()['tasks']))
    def test_same_event_and_sha_never_duplicate_tasks(self):
        a=self.receive();b=self.receive();c=self.receive(key='evt2')
        self.assertEqual(a,b);self.assertEqual('DUPLICATE',c['status']);self.assertEqual(a['task_id'],c['task_id']);self.assertEqual(1,len(self.store.snapshot()['tasks']))
    def test_restart_dedup(self):
        self.receive();again=Deliveries(self.store,self.root/'delivery','a'*40,self.catalog)
        r=again.receive(self.owner,'evt2','sample.png',self.data,self.meta);self.assertEqual('DUPLICATE',r['status'])
    def test_missing_manifest_hold_with_review_request(self):
        r=self.receive(meta=None);self.assertEqual('HOLD',r['status']);self.assertIn('MANIFEST_MISSING',r['inspection']['hold_reasons']);self.assertTrue(r['task_id'])
    def test_unknown_rights_hold(self):
        r=self.receive();self.assertEqual('RIGHTS_SOURCE_PENDING',r['inspection']['rights']);self.assertEqual('USER_REVIEW_PENDING',r['inspection']['art'])
    def test_untrusted_approval_cannot_approve(self):
        m=manifest(self.data);m['approval_status']='USER_ART_APPROVED'
        r=self.receive(meta=json.dumps(m).encode());self.assertIn('UNTRUSTED_APPROVAL_CLAIM',r['inspection']['hold_reasons'])
        self.assertEqual('USER_REVIEW_PENDING',self.store.get(r['task_id'])['art'])
        with self.assertRaises(Denied):self.store.command(self.owner,'approve1','approve',{'task_id':r['task_id'],'sha':r['sha256'],'scope':json.loads(self.store.get(r['task_id'])['payload'])['scope']})
    def test_mock_worker_does_not_claim_delivery_request(self):
        r=self.receive();self.assertIsNone(MockWorker(self.store).tick());self.assertEqual('BLOCKED',self.store.get(r['task_id'])['state'])
    def test_bad_actor_denied(self):
        with self.assertRaises(Denied):self.engine.receive(Identity(2,1,1),'x','a.png',self.data)
        self.assertEqual([],self.store.snapshot()['tasks'])
    def test_protected_name_hold(self):
        m=manifest(self.data,'wolf.png');r=self.receive('wolf.png',meta=json.dumps(m).encode())
        self.assertIn('PROTECTED_ASSET_COLLISION',r['inspection']['hold_reasons'])
    def test_protected_hash_hold_after_rename(self):
        self.engine.catalog['sha256']=[hashlib.sha256(self.data).hexdigest()]
        r=self.receive();self.assertIn('PROTECTED_ASSET_COLLISION',r['inspection']['hold_reasons'])
    def test_wrong_dimensions_and_frame_layout_hold(self):
        m=manifest(self.data);m['files'][0].update(dimensions=[8,8],frame_size=[7,7]);r=self.receive(meta=json.dumps(m).encode())
        self.assertIn('SPRITE_DIMENSIONS_UNKNOWN_OR_MISMATCH',r['inspection']['hold_reasons']);self.assertIn('FRAME_CONTRACT_UNKNOWN_OR_MISMATCH',r['inspection']['hold_reasons'])
    def test_sha_mismatch_rejected_no_task(self):
        m=manifest(self.data);m['files'][0]['sha256']='b'*64;r=self.receive(meta=json.dumps(m).encode())
        self.assertEqual('REJECTED',r['status']);self.assertIsNone(r['task_id'])
    def test_png_disguised_and_trailing_payload_rejected(self):
        self.assertEqual('REJECTED',self.receive(data=b'MZfake')['status'])
        self.assertEqual('REJECTED',self.receive(data=self.data+b'MZevil',key='b')['status'])
    def test_png_corrupt_crc_rejected(self):
        data=bytearray(self.data);data[-1]^=1
        self.assertEqual('REJECTED',self.receive(data=bytes(data))['status'])
    def test_safe_zip_crc_and_manifest(self):
        m=manifest(self.data);license_data=b'SYNTHETIC TEST ONLY';m['files'].append({'path':'LICENSE.txt','sha256':hashlib.sha256(license_data).hexdigest()})
        data=bundle({'sample.png':self.data,'LICENSE.txt':license_data,'Manifest.json':json.dumps(m).encode()})
        r=self.receive('sample.zip',data,False);self.assertEqual('HOLD',r['status']);self.assertEqual('PASS',r['inspection']['zip_crc']);self.assertEqual('RIGHTS_HUMAN_REVIEW_PENDING',r['inspection']['rights'])
    def test_zip_no_manifest_hold(self):
        r=self.receive('sample.zip',bundle({'sample.png':self.data}),False);self.assertEqual('HOLD',r['status']);self.assertIn('MANIFEST_MISSING',r['inspection']['hold_reasons'])
    def test_zip_traversal_rejected(self):
        r=self.receive('sample.zip',bundle({'../escape.png':self.data}),False);self.assertEqual('REJECTED',r['status']);self.assertFalse((self.root/'escape.png').exists())
    def test_zip_absolute_and_windows_path_rejected(self):
        for name in ('/x.png','C:/x.png','Assets/x.png'):
            self.assertEqual('REJECTED',self.receive('sample.zip',bundle({name:self.data}),False,key=name)['status'])
    def test_raw_backslash_zip_name_rejected_before_windows_normalization(self):
        out=io.BytesIO()
        with zipfile.ZipFile(out,'w') as z:
            entry=zipfile.ZipInfo('safe.png');entry.filename=entry.orig_filename='folder\\x.png';z.writestr(entry,self.data)
        self.assertEqual('REJECTED',self.receive('sample.zip',out.getvalue(),False)['status'])
    def test_zip_duplicate_case_and_executable_rejected(self):
        self.assertEqual('REJECTED',self.receive('sample.zip',bundle({'A.png':self.data,'a.png':self.data}),False)['status'])
        self.assertEqual('REJECTED',self.receive('sample.zip',bundle({'x.exe':b'MZbad'}),False,key='badexe')['status'])
    def test_zip_link_rejected(self):
        out=io.BytesIO()
        with zipfile.ZipFile(out,'w') as z:
            entry=zipfile.ZipInfo('link.png');entry.create_system=3;entry.external_attr=0o120777<<16;z.writestr(entry,'/tmp/x')
        self.assertEqual('REJECTED',self.receive('sample.zip',out.getvalue(),False)['status'])
    def test_zip_bomb_ratio_rejected(self):
        data=bundle({'bomb.txt':b'0'*1000000},zipfile.ZIP_DEFLATED)
        self.assertEqual('REJECTED',self.receive('sample.zip',data,False)['status'])
    def test_zip_crc_corruption_rejected(self):
        data=bundle({'sample.png':self.data})
        bad=bytearray(data);at=data.index(self.data);bad[at+len(self.data)-1]^=1
        self.assertEqual('REJECTED',self.receive('sample.zip',bytes(bad),False)['status'])
    def test_zip_entry_count_limit(self):
        data=bundle({str(i)+'.txt':b'x' for i in range(257)})
        self.assertEqual('REJECTED',self.receive('sample.zip',data,False)['status'])
    def test_writer_handle_blocks_snapshot_until_closed(self):
        p=self.root/'writing.png'
        with p.open('wb') as writer:
            writer.write(self.data);writer.flush()
            with self.assertRaises(Denied):snapshot(p)
        self.assertEqual(self.data,snapshot(p))
    def test_duplicate_manifest_keys_hold(self):
        r=self.receive(meta=b'{"files":[],"files":[]}');self.assertEqual('HOLD',r['status']);self.assertIn('MANIFEST_INVALID',r['inspection']['hold_reasons'])
    def test_snapshot_immutable_tamper_refused(self):
        r=self.receive();p=Path(json.loads(self.store.get(r['task_id'])['payload'])['artifact']);p.write_bytes(b'tampered')
        with self.assertRaises(Denied):self.receive(key='evt2')
    def test_folder_waits_for_stable_file_and_dedups(self):
        clock=[0];w=FolderWatcher(self.engine,self.owner,self.root/'incoming',quiet_seconds=3,clock=lambda:clock[0])
        p=w.inbox/'sample.png';p.write_bytes(self.data);p.with_name(p.name+'.manifest.json').write_bytes(self.meta)
        self.assertEqual([],w.tick());clock[0]=1;self.assertEqual([],w.tick());clock[0]=4
        self.assertEqual(1,len(w.tick()));self.assertEqual([],w.tick());self.assertEqual(1,len(self.store.snapshot()['tasks']))
    def test_changing_file_resets_quiet_period(self):
        clock=[0];w=FolderWatcher(self.engine,self.owner,self.root/'incoming',3,lambda:clock[0]);p=w.inbox/'x.png';p.write_bytes(self.data)
        w.tick();clock[0]=4;p.write_bytes(self.data+b'x');self.assertEqual([],w.tick());clock[0]=5;self.assertEqual([],w.tick())
    def test_partial_file_ignored(self):
        w=FolderWatcher(self.engine,self.owner,self.root/'incoming',0);(w.inbox/'sample.png.part').write_bytes(self.data)
        for _ in range(3):self.assertEqual([],w.tick())
    def test_outbox_report_and_idempotence(self):
        self.receive();sent=[]
        async def publish(text):sent.append(text);return '42'
        asyncio.run(self.engine.flush(publish));asyncio.run(self.engine.flush(publish));self.assertEqual(1,len(sent));self.assertIn('USER_REVIEW_PENDING',sent[0])
    def test_outbox_unknown_failure_no_auto_duplicate(self):
        self.receive();calls=[]
        async def fail(text):calls.append(text);raise RuntimeError('synthetic')
        asyncio.run(self.engine.flush(fail));asyncio.run(self.engine.flush(fail));self.assertEqual(1,len(calls))
        with self.store.tx() as db:self.assertEqual('UNCERTAIN',db.execute('SELECT status FROM art_delivery_outbox').fetchone()[0])


if __name__=='__main__':unittest.main()
