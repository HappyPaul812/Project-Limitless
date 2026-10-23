"""완료된 납품 파일만 snapshot. 폴더 외부·링크·쓰는 중인 파일은 읽지 않습니다."""
import ctypes
import hashlib
import os
import time
from pathlib import Path
from .core import Denied
from .intake import safe_root,MAX_ZIP


def snapshot(path,limit=MAX_ZIP):
    path=Path(path)
    if path.is_symlink() or not path.is_file():raise Denied('INPUT_NOT_REGULAR_FILE')
    safe_root(path.parent)
    before=path.stat()
    if not 0<before.st_size<=limit:raise Denied('INPUT_SIZE_LIMIT')
    handle=None
    if os.name=='nt':
        from ctypes import wintypes
        create=ctypes.windll.kernel32.CreateFileW;create.argtypes=[wintypes.LPCWSTR,wintypes.DWORD,wintypes.DWORD,ctypes.c_void_p,wintypes.DWORD,wintypes.DWORD,wintypes.HANDLE];create.restype=wintypes.HANDLE
        handle=create(str(path.absolute()),0x80000000,1,None,3,0x80,None)
        if handle==ctypes.c_void_p(-1).value:raise Denied('INPUT_STILL_OPEN_FOR_WRITE')
    try:
        data=path.read_bytes();after=path.stat()
        if len(data)>limit or (before.st_size,before.st_mtime_ns)!=(after.st_size,after.st_mtime_ns):raise Denied('INPUT_CHANGED_DURING_READ')
        return data
    finally:
        if handle is not None:
            close=ctypes.windll.kernel32.CloseHandle;close.argtypes=[ctypes.c_void_p];close(handle)


class FolderWatcher:
    def __init__(self,deliveries,owner,inbox,quiet_seconds=3,clock=time.monotonic):
        self.deliveries=deliveries;self.owner=owner;self.inbox=safe_root(inbox)
        self.quiet_seconds=quiet_seconds;self.clock=clock;self.observed={};self.errors={};self.finished={}

    def tick(self):
        results=[]
        safe_root(self.inbox)
        for entry in os.scandir(self.inbox):
            p=Path(entry.path)
            if p.suffix.lower() not in ('.png','.zip') or not entry.is_file(follow_symlinks=False):continue
            # PNG에는 같은 이름+.manifest.json, ZIP에는 선택적 역할/Issue context를 사용합니다.
            side=p.with_name(p.name+'.manifest.json')
            try:
                st=p.stat();ss=side.stat() if side.exists() else None
                marker=(st.st_size,st.st_mtime_ns,ss.st_size if ss else None,ss.st_mtime_ns if ss else None)
                if self.finished.get(p.name)==marker:continue
                previous=self.observed.get(p.name)
                if previous is None or previous[0]!=marker:self.observed[p.name]=(marker,self.clock(),1);continue
                self.observed[p.name]=(marker,previous[1],previous[2]+1)
                if previous[2]<2 or self.clock()-previous[1]<self.quiet_seconds:continue
                if st.st_size>MAX_ZIP or (ss and ss.st_size>1024*1024):
                    key='folder-rejected:'+hashlib.sha256((p.name.casefold()+str(marker)).encode()).hexdigest()
                    if not self.deliveries.seen(key):results.append(self.deliveries.reject_transport(self.owner,key,'INPUT_OR_METADATA_SIZE_LIMIT'))
                    self.finished[p.name]=marker
                    continue
                data=snapshot(p);metadata=snapshot(side,1024*1024) if ss else None
                fingerprint=hashlib.sha256(p.name.casefold().encode()+hashlib.sha256(data).digest()+hashlib.sha256(metadata or b'').digest()).hexdigest()
                key='folder:'+fingerprint
                if self.deliveries.seen(key):self.finished[p.name]=marker;continue
                results.append(self.deliveries.receive(self.owner,key,p.name,data,metadata,'WINDOWS_SHARED_INBOX'))
                self.finished[p.name]=marker
            except (Denied,OSError):
                # 원문 경로/내용을 로그에 쓰지 않습니다. 파일이 안정화되면 다음 tick에서 다시 검사합니다.
                self.errors[p.name]='WAITING_STABLE_OR_ACCESSIBLE_FILE'
        return results
