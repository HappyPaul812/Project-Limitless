"""현재 프로세스 토큰·LSA 로그온 메타데이터만 조회합니다. 비밀정보/권한 변경 없음."""
import ctypes
from ctypes import wintypes

def native_identity():
    """환경변수 대신 현재 프로세스 토큰을 직접 조회합니다. 계정/권한을 변경하지 않습니다."""
    kernel,adv=ctypes.WinDLL('kernel32',use_last_error=True),ctypes.WinDLL('advapi32',use_last_error=True)
    kernel.GetCurrentProcess.restype=wintypes.HANDLE
    adv.OpenProcessToken.argtypes=[wintypes.HANDLE,wintypes.DWORD,ctypes.POINTER(wintypes.HANDLE)]
    adv.GetTokenInformation.argtypes=[wintypes.HANDLE,ctypes.c_int,ctypes.c_void_p,wintypes.DWORD,ctypes.POINTER(wintypes.DWORD)]
    adv.ConvertSidToStringSidW.argtypes=[ctypes.c_void_p,ctypes.POINTER(wintypes.LPWSTR)]
    kernel.LocalFree.argtypes=[ctypes.c_void_p];kernel.CloseHandle.argtypes=[wintypes.HANDLE]
    token=wintypes.HANDLE()
    if not adv.OpenProcessToken(kernel.GetCurrentProcess(),8,ctypes.byref(token)):raise ValueError('HOST_TOKEN_HOLD')
    try:
        def info(kind):
            size=wintypes.DWORD();adv.GetTokenInformation(token,kind,None,0,ctypes.byref(size))
            if not 0<size.value<65536:raise ValueError('HOST_TOKEN_SIZE_HOLD')
            buffer=ctypes.create_string_buffer(size.value)
            if not adv.GetTokenInformation(token,kind,buffer,size,ctypes.byref(size)):raise ValueError('HOST_TOKEN_QUERY_HOLD')
            return buffer
        user=info(1);text=wintypes.LPWSTR()
        if not adv.ConvertSidToStringSidW(ctypes.c_void_p.from_buffer(user),ctypes.byref(text)):raise ValueError('HOST_SID_HOLD')
        try:sid=text.value
        finally:kernel.LocalFree(text)
        class Luid(ctypes.Structure):_fields_=[('low',wintypes.DWORD),('high',wintypes.LONG)]
        class Text(ctypes.Structure):_fields_=[('length',wintypes.USHORT),('max',wintypes.USHORT),('buffer',ctypes.c_void_p)]
        class Stats(ctypes.Structure):_fields_=[('id',Luid),('auth',Luid),('expiration',ctypes.c_longlong),('kind',wintypes.DWORD),('level',wintypes.DWORD),('charged',wintypes.DWORD),('available',wintypes.DWORD),('groups',wintypes.DWORD),('privileges',wintypes.DWORD),('modified',Luid)]
        class Logon(ctypes.Structure):_fields_=[('size',wintypes.DWORD),('id',Luid),('user',Text),('domain',Text),('package',Text),('kind',wintypes.DWORD),('session',wintypes.DWORD)]
        security=ctypes.WinDLL('secur32',use_last_error=True)
        security.LsaGetLogonSessionData.argtypes=[ctypes.POINTER(Luid),ctypes.POINTER(ctypes.c_void_p)]
        security.LsaFreeReturnBuffer.argtypes=[ctypes.c_void_p]
        stats=Stats.from_buffer_copy(info(10));data=ctypes.c_void_p()
        status=security.LsaGetLogonSessionData(ctypes.byref(stats.auth),ctypes.byref(data))
        if status!=0:raise ValueError('HOST_LOGON_QUERY_HOLD_'+format(status&0xffffffff,'08X'))
        try:logon=ctypes.cast(data,ctypes.POINTER(Logon)).contents;logon_type=int(logon.kind);logon_session=int(logon.session)
        finally:security.LsaFreeReturnBuffer(data)
        session=int.from_bytes(info(12).raw[:4],'little')
        if session!=logon_session:raise ValueError('HOST_SESSION_MISMATCH_HOLD')
        adv.ConvertStringSidToSidW.argtypes=[wintypes.LPCWSTR,ctypes.POINTER(ctypes.c_void_p)]
        adv.CheckTokenMembership.argtypes=[wintypes.HANDLE,ctypes.c_void_p,ctypes.POINTER(wintypes.BOOL)]
        admin_sid=ctypes.c_void_p();member=wintypes.BOOL()
        if not adv.ConvertStringSidToSidW('S-1-5-32-544',ctypes.byref(admin_sid)):raise ValueError('HOST_ADMIN_QUERY_HOLD')
        try:
            if not adv.CheckTokenMembership(None,admin_sid,ctypes.byref(member)):raise ValueError('HOST_ADMIN_QUERY_HOLD')
        finally:kernel.LocalFree(admin_sid)
        return {'sid':sid,'session':session,'logon':logon_type,'admin':bool(member.value),
                'elevated':int.from_bytes(info(20).raw[:4],'little')!=0}
    finally:kernel.CloseHandle(token)
