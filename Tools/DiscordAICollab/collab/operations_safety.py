"""종료 요청용 읽기 전용 장치·업무 검사. OS 종료 API는 없습니다."""
import hashlib
import platform
from pathlib import Path


def pc_preflight(store,db):
    pc_id=hashlib.sha256((platform.node()+'|'+str(Path(__file__).resolve().parent.parent)).encode()).hexdigest()[:16]
    running=db.execute("SELECT count(*) FROM tasks WHERE state='RUNNING'").fetchone()[0]
    return {'pc_id':pc_id,'platform':platform.system(),'running_tasks':running,
            'unsaved_user_work':'UNKNOWN','file_copy_idle':'UNKNOWN','unity_idle':'NOT_VERIFIED',
            'scoped_integration_approval':'MISSING','github_local_sync':'HOLD_NO_APPROVAL',
            'shutdown_enabled':False,'os_shutdown_calls':0,'cancel_grace_seconds':120}
