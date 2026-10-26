"""소비된717a 시험 로그만 서비스 자기 토큰으로 읽습니다. CLI/네트워크/DB 실행 경로가 없습니다."""
import argparse
from collections import Counter
import ctypes
from ctypes import wintypes
import hashlib
import json
import os
from pathlib import Path
import re
import stat

REQUEST='717a0afa735d485daf790579b964e8c2'
BOOT=Path('C:/ProgramData/LimitlessRelay/boot-approval.json')
BOOT_SHA='3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f'
SUMMARY=Path('C:/ProgramData/LimitlessRelay/InferenceTrials')/REQUEST/'Output/summary.json'
SUMMARY_SHA='900e016992f1b27af291b56129a42d66b329a1ffbe50a2d007a1fa75f3137dae'
WORDS=set(('authentication authorization unauthorized unauthenticated forbidden access permission permissions denied '
 'invalid expired missing required token credentials credential account subscription login refresh '
 'failed failure error internal server client request response method initialize initialized '
 'rate limit limits exceeded usage quota too many requests retry retrying retries reconnect reconnecting later '
 'unavailable timeout timed out connection connect connecting disconnected disconnect closed reset refused network '
 'transport tls certificate proxy http https status not supported found allowed valid malformed protocol json rpc '
 'parse unexpected eof end file read write process exit terminated killed stderr warning debug info overloaded busy '
 'service the a an is was are has been could cannot unable to for from of in on with and or please unknown '
 'insufficient scope capacity conflict bad gateway upstream resource deadline cancellation session sandbox setup '
 'windows code codex auth rejected stream streaming send sending received receive receiving before completion '
 'complete response request body url endpoint web websocket websockets sse attempt attempts exhausted '
 'mcp tool tools startup started starting stopped stop server servers notification notice system policy '
 'bootstrap configuration reserved built builtin provider providers contains overridden argument unexpected '
 'connectionerror timeoutError hostname lookup resolve resolution dns socket handshake negotiation '
 'support supports unsupported version feature features enabled disabled resource temporarily interrupted '
 'failed to get authentication token permission denied remote peer closed without successful '
 'agent turn item final output input command execution retryable nonretryable transporterror '
 'module program file path cannot find sandboxed elevated elevation restricted desktop '
 'child parent handle job broken pipe operation canceled cancelled context initialization '
 'preflight environment user profile logon interactive batch error occurred encountered').split())
SAFE_CODES={'unauthorized','forbidden','permission_denied','rate_limit_exceeded','invalid_request_error',
 'bad_request','internal_server_error','network_error','stream_disconnected','http_connection_failed',
 'connection_failed','sandbox_setup_failed','model_not_found','context_window_exceeded','unknown'}
EVENTS={'thread.started','turn.started','turn.completed','turn.failed','error','item.started','item.updated','item.completed'}


def strict_json(text):
    def pairs(items):
        result={}
        for key,value in items:
            if key in result:raise ValueError('DUPLICATE_JSON_KEY')
            result[key]=value
        return result
    return json.loads(text,object_pairs_hook=pairs,parse_constant=lambda _:(_ for _ in ()).throw(ValueError('NONFINITE_JSON')))


def no_link(path):
    if path.is_symlink() or (path.exists() and getattr(path.lstat(),'st_file_attributes',0)&getattr(stat,'FILE_ATTRIBUTE_REPARSE_POINT',0x400)):
        raise ValueError('LOG_PATH_LINK_HOLD')


def bounded_read(path,cap):
    no_link(path)
    with path.open('rb') as source:raw=source.read(cap+1)
    if len(raw)>cap:raise ValueError('LOG_INPUT_SIZE_HOLD')
    return raw


def sha(raw):return hashlib.sha256(raw).hexdigest()


def safe_text(value,cap=2048):
    if not isinstance(value,str):return '[REDACTED_NON_TEXT]'
    value=value[:65536]
    value=re.sub(r'(?im)^\s*(authorization|cookie|set-cookie)\s*:.*$','[REDACTED]',value)
    value=re.sub(r'(?i)\bBearer\s+\S+|\bsk-\S+|\beyJ\S+','[REDACTED]',value)
    value=re.sub(r'(?i)\b(?:password|secret|token|api_key|access_token|refresh_token|id_token)\s*[=:]\s*\S+','[REDACTED]',value)
    value=re.sub(r'https?://\S+|(?<!\S)[^\s@]+@[^\s@]+|[A-Za-z]:[\\/]\S+|["\'][^"\']*["\']','[REDACTED]',value)
    words=re.findall(r'[A-Za-z]+|[^\sA-Za-z]+',value)
    result=' '.join(w if w.lower() in WORDS else '[REDACTED]' for w in words)
    return re.sub(r'(?:\[REDACTED\]\s*){2,}','[REDACTED] ',result)[:cap]


def identifier(value):
    # CLI enum형식만 허용합니다. 자유문/경로/긴값/비밀필드명은 노출하지 않습니다.
    if isinstance(value,str) and re.fullmatch('[a-z][a-z0-9_]{0,39}',value) and not re.search('token|password|secret|credential|api_key',value):
        return value
    return 'UNRECOGNIZED_OR_REDACTED_TYPE'


def signals(text):
    text=text.lower()
    rules={
      'AUTH_OR_PERMISSION':['unauthorized','authentication failed','permission denied','forbidden','401','403'],
      'QUOTA_OR_RATE_LIMIT':['429','rate limit','quota exceeded'],
      'DNS':['dns','name resolution','failed to lookup','resolve host','hostname'],
      'TLS':['certificate','tls','ssl'],
      'TRANSPORT':['stream disconnected','error sending request','failed to connect','connection reset','connection refused','websocket','transport'],
      'RECONNECT_OR_RETRY':['reconnecting','retrying','retry attempt'],
      'SANDBOX':['sandbox','elevation','restricted token'],
      'CONFIG':['reserved built-in','unknown field','unexpected argument','failed to load bootstrap'],
      'MCP':['mcp','mcp startup'],
    }
    return [key for key,fragments in rules.items() if any(part in text for part in fragments)]


def error_projection(event,line):
    raw=event.get('error')
    message=event.get('message')
    if isinstance(raw,dict):message=raw.get('message',message);code=raw.get('code')
    else:code=event.get('code');message=raw if isinstance(raw,str) else message
    if not isinstance(message,str):message=json.dumps(raw if raw is not None else {},ensure_ascii=False)
    code=code if type(code) is int and -(2**31)<=code<2**31 else (code if code in SAFE_CODES else None) if isinstance(code,str) else None
    statuses=sorted({int(v) for v in re.findall(r'(?i)(?:http(?:\s+status)?|status(?:\s+code)?)[\s:=]+([1-5][0-9]{2})\b',message)})
    os_codes=sorted({int(v) for v in re.findall(r'(?i)(?:os error|winerror|errno)[\s:(]+([0-9]{1,5})\b',message)})
    return {'line':line,'event':event['type'],'error_code':code,'message':safe_text(message),
            'http_status_codes':statuses,'os_error_codes':os_codes,'signals':signals(message),
            'endpoint_class':'OFFICIAL_CHATGPT_CODEX' if 'chatgpt.com/backend-api/codex' in message.lower() else 'NOT_EXPORTED_OR_UNKNOWN'}


def project(stdout,stderr,execution,launch,preflight):
    counts=Counter();items=[];errors=[];failed=[];invalid=[];turn_started=False;turn_completed=0;agent_messages=0
    for number,line in enumerate(stdout.decode('utf8',errors='strict').splitlines(),1):
        if not line.strip():continue
        try:
            event=strict_json(line)
            if not isinstance(event,dict) or type(event.get('type')) is not str:raise ValueError()
        except (ValueError,TypeError):invalid.append(number);continue
        kind=event['type'];counts[kind if kind in EVENTS else 'unknown']+=1
        if kind=='turn.started':turn_started=True
        if kind=='turn.completed':turn_completed+=1
        if kind in ('error','turn.failed'):
            value=error_projection(event,number)
            if kind=='error':errors.append(value)
            else:failed.append(value)
        if kind=='item.completed':
            item=event.get('item')
            if not isinstance(item,dict):invalid.append(number);continue
            item_type=identifier(item.get('type'))
            if item_type=='agent_message':agent_messages+=1
            items.append({'line':number,'item_type':item_type,'before_turn_started':not turn_started,
                'legacy_validator_allowed':item_type in ('reasoning','agent_message'),
                'diagnostic_message':safe_text(item.get('message',item.get('text','')),512),
                'status':item.get('status') if item.get('status') in ('completed','failed','in_progress') else None})
    args=launch.get('argv',[])
    if not isinstance(args,list) or not all(isinstance(arg,str) for arg in args):args=[]
    sandbox_index=args.index('--sandbox') if '--sandbox' in args else -1
    expected={'model_provider="openai"','forced_login_method="chatgpt"','approval_policy="never"','features.shell_tool=false',
              'features.apps=false','features.browser_use=false','features.computer_use=false','features.multi_agent=false',
              'mcp_servers={}','plugins={}','hooks={}','web_search="disabled"','tools.view_image=false'}
    flags={value:value in args for value in sorted(expected)}
    cli_sha=preflight.get('cli_sha256')
    return {'status':'SANITIZED_LOG_PROJECTION','source_request_id':REQUEST,'event_counts':dict(counts),
      'errors':errors[:32],'error_events_total':len(errors),'turn_failed':failed[:8],'completed_items':items[:16],
      'invalid_line_numbers':invalid[:32],'records_truncated':len(errors)>32 or len(failed)>8 or len(items)>16,
      'remote_response_evidence':{'completed_turns':turn_completed,'agent_message_items':agent_messages,
         'success_confirmed':False,'physical_request_count':'UNKNOWN'},
      'stderr':{'bytes':len(stderr),'summary':safe_text(stderr.decode('utf8',errors='replace'),4096),
                'signals':signals(stderr.decode('utf8',errors='replace'))},
      'process':{'exit_code':execution.get('process',{}).get('exit_code') if type(execution.get('process',{}).get('exit_code')) is int else None,
                 'timed_out':execution.get('process',{}).get('timed_out') if type(execution.get('process',{}).get('timed_out')) is bool else None},
      'launch_environment':{'sandbox_requested_read_only':sandbox_index>=0 and len(args)>sandbox_index+1 and args[sandbox_index+1]=='read-only',
        'sandbox_actual_enforcement':'NOT_PROVEN_BY_ARGV_ALONE','requested_controls':flags,
        'ignore_user_config':'--ignore-user-config' in args,'ephemeral':'--ephemeral' in args,
        'provider_override_present':any('model_providers.' in arg for arg in args),
        'cli_sha256':cli_sha if isinstance(cli_sha,str) and re.fullmatch('[a-f0-9]{64}',cli_sha) else None,
        'exec_authentication_success':'NOT_CONFIRMED','environment_values_exported':False},
      'model_calls':0,'official_usage_reads':0,'discord_posts':0,'sqlite_work_operations':0}


class Blob(ctypes.Structure):
    _fields_=[('size',wintypes.DWORD),('data',ctypes.POINTER(ctypes.c_ubyte))]


def own_sid():
    kernel=ctypes.WinDLL('kernel32',use_last_error=True);adv=ctypes.WinDLL('advapi32',use_last_error=True)
    kernel.GetCurrentProcess.restype=wintypes.HANDLE
    kernel.CloseHandle.argtypes=[wintypes.HANDLE];kernel.LocalFree.argtypes=[ctypes.c_void_p]
    adv.OpenProcessToken.argtypes=[wintypes.HANDLE,wintypes.DWORD,ctypes.POINTER(wintypes.HANDLE)]
    adv.GetTokenInformation.argtypes=[wintypes.HANDLE,ctypes.c_int,ctypes.c_void_p,wintypes.DWORD,ctypes.POINTER(wintypes.DWORD)]
    adv.ConvertSidToStringSidW.argtypes=[ctypes.c_void_p,ctypes.POINTER(wintypes.LPWSTR)]
    handle=wintypes.HANDLE();size=wintypes.DWORD();text=wintypes.LPWSTR()
    if not adv.OpenProcessToken(kernel.GetCurrentProcess(),8,ctypes.byref(handle)):raise ValueError('LOG_TOKEN_HOLD')
    try:
        adv.GetTokenInformation(handle,1,None,0,ctypes.byref(size));buffer=ctypes.create_string_buffer(size.value)
        if not adv.GetTokenInformation(handle,1,buffer,size,ctypes.byref(size)):raise ValueError('LOG_TOKEN_HOLD')
        sid=ctypes.cast(buffer,ctypes.POINTER(ctypes.c_void_p))[0]
        if not adv.ConvertSidToStringSidW(sid,ctypes.byref(text)):raise ValueError('LOG_TOKEN_HOLD')
        return text.value
    finally:
        if text:kernel.LocalFree(text)
        kernel.CloseHandle(handle)


def decrypt_own(raw):
    crypt=ctypes.WinDLL('crypt32',use_last_error=True);kernel=ctypes.WinDLL('kernel32',use_last_error=True)
    crypt.CryptUnprotectData.argtypes=[ctypes.POINTER(Blob),ctypes.c_void_p,ctypes.c_void_p,ctypes.c_void_p,ctypes.c_void_p,wintypes.DWORD,ctypes.POINTER(Blob)]
    kernel.LocalFree.argtypes=[ctypes.c_void_p]
    buffer=ctypes.create_string_buffer(raw);source=Blob(len(raw),ctypes.cast(buffer,ctypes.POINTER(ctypes.c_ubyte)));target=Blob()
    try:
        if not crypt.CryptUnprotectData(ctypes.byref(source),None,None,None,None,1,ctypes.byref(target)):raise ValueError('LOG_OWN_DPAPI_HOLD')
        if target.size>262144:raise ValueError('LOG_PLAINTEXT_SIZE_HOLD')
        return ctypes.string_at(target.data,target.size)
    finally:
        ctypes.memset(buffer,0,len(buffer))
        if target.data:ctypes.memset(target.data,0,target.size);kernel.LocalFree(target.data)


def run(review):
    if not re.fullmatch('[a-f0-9]{32}',review):raise ValueError('LOG_REVIEW_ID_HOLD')
    boot_raw=bounded_read(BOOT,65536)
    if sha(boot_raw)!=BOOT_SHA:raise ValueError('LOG_BOOT_SHA_HOLD')
    boot=strict_json(boot_raw.decode('utf-8-sig'))
    # 보호 로그를 여는 것보다 먼저 실제 토큰SID를 확인합니다. env 사용자명만 믿지 않습니다.
    if own_sid()!=boot['service_sid']:raise ValueError('LOG_SERVICE_SID_REQUIRED')
    profile=Path('C:/Users/LIMITLESSRelaySvc')
    if Path(os.environ.get('USERPROFILE','')).resolve()!=profile.resolve():raise ValueError('LOG_SERVICE_PROFILE_REQUIRED')
    no_link(profile);no_link(profile/'.codex')
    folder=profile/'.codex/limitless-diagnostics'/REQUEST;no_link(folder);no_link(folder.parent)
    names={'encrypted':'stdout.original.jsonl.dpapi','stderr':'stderr.redacted.txt','execution':'execution.json',
           'launch':'launch.safe.json','preflight':'preflight.safe.json'}
    data={k:bounded_read(folder/name,524288 if k=='encrypted' else 65536) for k,name in names.items()}
    execution=strict_json(data['execution'].decode('utf8'))
    stdout=decrypt_own(data['encrypted'])
    if sha(stdout)!=execution.get('original_stdout_sha256'):raise ValueError('LOG_ORIGINAL_SHA_HOLD')
    value=project(stdout,data['stderr'],execution,strict_json(data['launch'].decode('utf8')),strict_json(data['preflight'].decode('utf8')))
    del stdout
    summary_raw=bounded_read(SUMMARY,65536)
    if sha(summary_raw)!=SUMMARY_SHA:raise ValueError('LOG_SOURCE_SUMMARY_SHA_HOLD')
    summary=strict_json(summary_raw.decode('utf-8-sig'))
    if value['event_counts']!=summary['child']['entry_result']['validation']['event_counts']:raise ValueError('LOG_EVENT_COUNTS_MISMATCH_HOLD')
    after={k:sha(bounded_read(folder/name,524288 if k=='encrypted' else 65536)) for k,name in names.items()}
    if after!={k:sha(v) for k,v in data.items()}:raise ValueError('LOG_SOURCE_CHANGED_HOLD')
    value.update(review_id=review,service_sid_matches=True,source_logs_unchanged=True,source_file_sha256=after,
      original_jsonl_sha_verified=True,read_as_service_account=True,prior_usage_chatgpt_preflight_passed=True)
    target=Path('C:/ProgramData/LimitlessRelay/InferenceLogReviews')/review/'Output/projection.json'
    no_link(target.parent);no_link(target.parent.parent)
    raw=json.dumps(value,ensure_ascii=False,allow_nan=False).encode('utf8')
    if len(raw)>65536:raise ValueError('LOG_EXPORT_SIZE_HOLD')
    with target.open('xb') as output:output.write(raw)
    return {'status':'SANITIZED_LOG_PROJECTION_WRITTEN','review_id':review,'source_request_id':REQUEST,
       'projection_sha256':sha(raw),'source_logs_unchanged':True,'model_calls':0,'official_usage_reads':0,'sqlite_work_operations':0,'discord_posts':0}


if __name__=='__main__':
    parser=argparse.ArgumentParser();parser.add_argument('--review-id',required=True);args=parser.parse_args()
    try:result=run(args.review_id)
    except Exception as exc:
        reason=str(exc) if isinstance(exc,ValueError) and re.fullmatch('[A-Z0-9_]+',str(exc)) else 'LOG_PROJECTION_HOLD'
        result={'status':'HOLD','reason':reason,'error_type':type(exc).__name__,'model_calls':0,'official_usage_reads':0}
    print(json.dumps(result));raise SystemExit(0 if result['status']=='SANITIZED_LOG_PROJECTION_WRITTEN' else 2)
