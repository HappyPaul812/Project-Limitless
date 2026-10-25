"""사용량 진단 시험 전용 로컬 가짜 서버입니다. 네트워크/인증/모델 코드는 없습니다."""
import json
import sys
import time

scenario=sys.argv[1]
if scenario=='early_exit':
    sys.stderr.write('Connection closed user@example.test Bearer secret-value C:\\Users\\PrivateName\\auth.json\n')
    raise SystemExit(17)
if scenario=='stderr_flood':
    sys.stderr.write(('Access denied password=not-public user@example.test\n')*30000)
    sys.stderr.flush()
for line in sys.stdin:
    request=json.loads(line)
    number=request.get('id')
    if number is None:continue
    if scenario=='timeout':
        sys.stderr.write('Request timed out\n');sys.stderr.flush();time.sleep(60)
    if scenario=='invalid_json':
        print('not json secret-value',flush=True);continue
    if scenario=='not_object':
        print('[]',flush=True);continue
    if scenario=='oversized':
        print('x'*200000,flush=True);continue
    fail_at=1 if scenario=='initialize_denied' else 2 if scenario=='auth_denied' else 3
    if number==fail_at and scenario in ('auth_denied','access_denied','rate_limited','initialize_denied'):
        code={'auth_denied':401,'access_denied':403,'rate_limited':429,'initialize_denied':-32603}[scenario]
        message={401:'Authentication failed token expired',403:'Access permission denied',429:'Too many requests rate limit exceeded',-32603:'Internal server error'}[code]
        message+=' user@example.test Bearer secret-value C:\\Users\\PrivateName\\auth.json password=not-public'
        print(json.dumps({'id':number,'error':{'code':code,'message':message,
            'data':{'http_status':code,'retry_after':5,'reason':'Access denied','email':'user@example.test','token':'secret-value','huge':'S'*5000}}}),flush=True)
        continue
    if number==1:result={}
    elif number==2:result={'account':{'type':'chatgpt','email':'never-record@example.test'}}
    else:
        result={'ordinaryUsageAllowed':scenario!='blocked', 'rateLimits':{'limitId':'codex',
            'primary':{'usedPercent':100 if scenario=='exhausted' else 5,'windowDurationMins':300,'resetsAt':time.time()+3600},
            'secondary':{'usedPercent':10,'windowDurationMins':10080,'resetsAt':time.time()+86400}}}
        if scenario=='invalid_usage':result={'unexpected':'user@example.test'}
    print(json.dumps({'id':number,'result':result}),flush=True)
raise SystemExit(23 if scenario=='nonzero_after_response' else 0)
