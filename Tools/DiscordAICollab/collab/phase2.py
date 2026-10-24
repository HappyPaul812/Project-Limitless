"""바울 대표가 승인한 고정 읽기 전용 추론1회만 실행하는 제한 검증기입니다."""
from __future__ import annotations

import argparse
import json
import os
import re
import shutil
import subprocess
from pathlib import Path

from .core import Denied, KST, Store
from .intake import safe_root
from datetime import datetime

APPROVAL = 'LIMITLESS_PHASE2_20261023_ONE_READONLY_TURN'
MARKER = 'LIMITLESS_PHASE2_OK'
PROMPT = ('This is a read-only connectivity probe. Do not call tools or access files, '
          'accounts, network services, or other agents. Reply with exactly LIMITLESS_PHASE2_OK.')
REQUIRED = ('DISCORD_BOT_TOKEN','LIMITLESS_OWNER_IDS','LIMITLESS_GUILD_ID','LIMITLESS_CHANNEL_IDS')


def discord_preflight(environ=None):
    env = os.environ if environ is None else environ
    missing = [k for k in REQUIRED if not env.get(k,'').strip()]
    if missing:
        return {'status':'WAITING_FOR_USER_SETUP','missing_fields':missing,'live_test':'NOT_VERIFIED'}
    try:
        owners = [int(x) for x in env['LIMITLESS_OWNER_IDS'].split(',')]
        channels = [int(x) for x in env['LIMITLESS_CHANNEL_IDS'].split(',')]
        guild = int(env['LIMITLESS_GUILD_ID'])
        if guild<=0 or not owners or not channels or any(x<=0 for x in owners+channels):
            raise ValueError()
    except (ValueError,TypeError):
        return {'status':'WAITING_FOR_USER_SETUP','reason':'INVALID_NUMERIC_IDS','live_test':'NOT_VERIFIED'}
    return {'status':'LOCAL_CONFIG_READY' if env.get('LIMITLESS_ALLOW_DISCORD')=='1' else 'WAITING_FOR_USER_SETUP',
            'reason':'READY' if env.get('LIMITLESS_ALLOW_DISCORD')=='1' else 'EXPLICIT_CONNECTION_SWITCH_REQUIRED',
            'live_test':'NOT_VERIFIED'}


def child_environment(environ=None):
    source = os.environ if environ is None else environ
    # SDK/API·Discord·GitHub 키와 URL override가 모델 하위 프로세스로 흘러가지 않습니다.
    allowed = {'systemroot','windir','userprofile','home','appdata','localappdata','temp','tmp',
               'path','pathext','comspec','codex_home','lang','lc_all'}
    return {k:v for k,v in source.items() if k.lower() in allowed}


def cli_info(executable,invoke=subprocess.run,environ=None):
    env = child_environment(environ)
    try:
        version = invoke([executable,'--version'],capture_output=True,text=True,encoding='utf8',errors='replace',env=env,timeout=15)
        auth = invoke([executable,'login','status'],capture_output=True,text=True,encoding='utf8',errors='replace',env=env,timeout=15)
        match = re.search(r'codex-cli ([0-9a-zA-Z.\-]+)',version.stdout)
        # 로그인 출력은 메모리에서 종류만 비교하고 로그나 파일에 저장하지 않습니다.
        eligible = auth.returncode==0 and 'using ChatGPT' in auth.stdout+auth.stderr
        return {'cli_version':match[1] if match else 'UNKNOWN','subscription_available':eligible}
    except (OSError,subprocess.SubprocessError):
        return {'cli_version':'UNKNOWN','subscription_available':False}


def probe_command(executable,workspace):
    args = [str(executable),'exec','--json','--ephemeral','--ignore-user-config',
            '--sandbox','read-only','--skip-git-repo-check','-C',str(workspace)]
    controls = ['forced_login_method="chatgpt"','model_provider="openai"',
                'approval_policy="never"','features.shell_tool=false','features.shell_snapshot=false',
                'features.apps=false','apps._default.enabled=false','features.browser_use=false',
                'features.computer_use=false','features.multi_agent=false','mcp_servers={}','plugins={}','hooks={}',
                'web_search="disabled"','project_doc_max_bytes=0','model_reasoning_effort="low"']
    for item in controls:
        args.extend(['-c',item])
    return args+['-']


def verify_cli_controls(executable,workspace,invoke=subprocess.run):
    """features list로 bootstrap만 검사합니다. 모델 추론/인증정보 출력은 없습니다."""
    command=probe_command(executable,workspace)
    controls=command[command.index('-c'):-1]
    try:
        checked=invoke([str(executable),'features','list']+controls,cwd=str(workspace),
                       capture_output=True,text=True,encoding='utf8',errors='replace',
                       env=child_environment(),timeout=20)
        disabled={line.split()[0] for line in checked.stdout.splitlines()
                  if len(line.split())>=3 and line.split()[-1]=='false'}
        needed={'shell_tool','apps','browser_use','computer_use','multi_agent'}
        return {'status':'PASS' if checked.returncode==0 and needed<=disabled else 'NOT_VERIFIED',
                'mode':'CONFIG_ONLY_NO_INFERENCE','exit_code':checked.returncode,
                'required_tool_controls_disabled':needed<=disabled,
                'builtin_provider_override':False,
                'transport_retry_policy':'BUILTIN_BOUNDED_DEFAULT; NO_APPLICATION_RETRY'}
    except (OSError,subprocess.SubprocessError):
        return {'status':'NOT_VERIFIED','mode':'CONFIG_ONLY_NO_INFERENCE'}


def parse_events(stdout,returncode):
    """임의 모델 텍스트·도구 호출은 저장하지 않습니다. 숫자 사용량과 고정 응답만 채택합니다."""
    turns=0; valid=False; forbidden=False; failed=False; quota=False; usage={}
    for line in stdout.splitlines():
        try:
            event=json.loads(line)
        except (ValueError,TypeError):
            failed=True; continue
        if not isinstance(event,dict):
            failed=True; continue
        kind=event.get('type')
        if kind=='turn.completed':
            turns+=1
            candidate=event.get('usage') or {}
            if isinstance(candidate,dict):
                for key in ('input_tokens','cached_input_tokens','output_tokens','reasoning_output_tokens'):
                    val=candidate.get(key)
                    if type(val) is int and val>=0:
                        usage[key]=usage.get(key,0)+val
        if kind in ('error','turn.failed'):
            failed=True
            # 서버 오류는 분류만 하고 원문/인증정보를 외부에 전달하지 않습니다.
            message=json.dumps(event).lower()
            quota=quota or any(x in message for x in ('quota','usage limit','rate limit','429'))
        if kind in ('item.started','item.updated','item.completed'):
            item=event.get('item') or {}
            if not isinstance(item,dict):
                failed=True; continue
            if item.get('type')=='agent_message' and kind=='item.completed':
                valid=item.get('text','').strip()==MARKER
            elif item.get('type') not in ('agent_message','reasoning'):
                forbidden=True
    success=returncode==0 and turns==1 and valid and not failed and not forbidden
    return {'status':'PASS' if success else 'NOT_VERIFIED','completed_turns':turns,
            'response_marker_valid':valid,'tool_use_detected':forbidden,'usage':usage,
            'reason':'QUOTA_EXCEEDED' if quota else ('OK' if success else 'CLI_OR_RESPONSE_FAILED')}


def run_probe_once(store,workspace,executable,*,approved=False,invoke=subprocess.run):
    if not approved:
        raise Denied('이번 고정 추론1회에 대한 명시적 승인이 필요합니다')
    from .scheduling import gate
    with store.tx() as db:
        if gate(db, store.clock(), ai=True, duration=90):
            raise Denied('SCHEDULE_OR_USAGE_HOLD: 레거시 단발 시험도 공식 사용량 확인 필요')
    info=cli_info(executable,invoke)
    if not info['subscription_available']:
        return {'status':'WAITING_FOR_USER_SETUP','reason':'CHATGPT_SUBSCRIPTION_SIGN_IN_REQUIRED','cli_version':info['cli_version'],'cli_exec_attempts':0}
    root=safe_root(workspace)
    if any(root.iterdir()):
        raise Denied('추론 검증은 비어 있는 전용 작업공간만 허용합니다')
    if verify_cli_controls(executable,root,invoke)['status']!='PASS':
        return {'status':'NOT_VERIFIED','reason':'CLI_CONTROLS_NOT_VALIDATED','cli_exec_attempts':0}
    # 기존 API 비용 예산을 올리지 않고 이번 구독 단발 승인만 별도 장부에 선점합니다.
    with store.tx() as db:
        if gate(db, store.clock(), ai=True, duration=90):
            raise Denied('SCHEDULE_OR_USAGE_HOLD: 실행 직전 재확인 실패')
        if db.execute('SELECT 1 FROM settings WHERE key=?',(APPROVAL,)).fetchone():
            raise Denied('이번 승인 추론1회를 이미 사용했습니다; 재호출 금지')
        if store.setting(db,'paused')=='1':
            raise Denied('정지 중에는 추론을 시작하지 않습니다')
        if db.execute("SELECT COUNT(*) FROM tasks WHERE state='RUNNING'").fetchone()[0]>=2:
            raise Denied('최대 동시작업2개 제한')
        payload={'title':'Phase2 고정 읽기 전용 추론1회','role':'CodexAgent','issue':None,
                 'base_sha':'de6198480f4021f4aecaae51cbc88c57234b3d77','scope':APPROVAL,
                 'protected_assets':['Unity','Save','approved_art'],'inputs':['fixed synthetic prompt only'],
                 'verification':['exact marker; one turn; no tools'],'approval_required':True,
                 'artifact':'NO_ART_ASSET_FROM_PROBE'}
        task_id=store._create(db,APPROVAL,payload)
        db.execute("INSERT INTO settings VALUES(?,?)",(APPROVAL,'RESERVED'))
        db.execute("UPDATE tasks SET state='RUNNING',lease=?,lease_until=? WHERE id=?",(APPROVAL,store.clock()+300,task_id))
        store.audit(db,'SUBSCRIPTION_PROBE_RESERVED',task_id)
    result={'cli_version':info['cli_version'],'cli_exec_attempts':1,'api_fallback':False,
            'task_id':task_id,'approval':APPROVAL,'additional_api_cost_usd':0,
            'subscription_cost':'INCLUDED_ALLOWANCE; EXACT_MONEY_NOT_AVAILABLE'}
    try:
        completed=invoke(probe_command(executable,root),input=PROMPT,capture_output=True,text=True,
                         encoding='utf8',errors='replace',env=child_environment(),timeout=90,
                         creationflags=getattr(subprocess,'CREATE_NO_WINDOW',0))
        result.update(parse_events(completed.stdout,completed.returncode))
        if result['status']!='PASS' and result['reason']=='CLI_OR_RESPONSE_FAILED':
            raw=(completed.stdout+completed.stderr).lower()
            result['reason']='CLI_EXEC_FAILED_NO_RETRY'
            for fragment,label in [('unknown field','CONFIG_UNSUPPORTED'),('unrecognized feature','CONFIG_UNSUPPORTED'),
                                   ('unauthorized','AUTH_REJECTED'),('401','AUTH_REJECTED'),
                                   ('failed to connect','NETWORK_FAILED'),('dns','NETWORK_FAILED'),
                                   ('certificate','TLS_FAILED')]:
                if fragment in raw:
                    result['reason']=label; break
        result['exit_code']=completed.returncode
        # stdout/stderr 원문은 여기서 폐기하며 응답 임의 텍스트를 큐에 넣지 않습니다.
    except subprocess.TimeoutExpired:
        result.update(status='NOT_VERIFIED',reason='TIMEOUT_NO_RETRY',completed_turns=0,usage={})
    except OSError:
        result.update(status='NOT_VERIFIED',reason='LAUNCH_FAILED_NO_RETRY',completed_turns=0,usage={})
    result['workspace_unchanged']=not any(root.iterdir())
    if not result['workspace_unchanged']:
        result.update(status='NOT_VERIFIED',reason='WORKSPACE_CHANGED')
    with store.tx() as db:
        task=store.task(db,task_id)
        usage=result.get('usage',{})
        now=datetime.fromtimestamp(store.clock(),KST)
        db.execute('INSERT INTO usage VALUES(?,?,?,?,?,?,?,?,?)',(APPROVAL,task_id,'text',0,
                   usage.get('input_tokens',0),usage.get('output_tokens',0),
                   'COMPLETED' if result['status']=='PASS' else 'UNKNOWN',now.strftime('%Y-%m-%d'),now.strftime('%Y-%m')))
        # 동시에 pause된 작업의 늦은 결과로 실행 상태를 복구하지 않습니다.
        if task['state']=='RUNNING' and task['lease']==APPROVAL and store.clock()<task['lease_until']:
            db.execute('UPDATE tasks SET state=?,tech=?,evidence=?,lease=NULL,lease_until=NULL WHERE id=?',
                       ('REVIEW_PENDING' if result['status']=='PASS' else 'NOT_VERIFIED',
                        'TECH_PASS' if result['status']=='PASS' else 'NOT_VERIFIED',
                        json.dumps({'mode':'CODEX_SUBSCRIPTION_PROBE',**result}),task_id))
        db.execute('UPDATE settings SET value=? WHERE key=?',(result['status'],APPROVAL))
        if result.get('reason')=='QUOTA_EXCEEDED':
            db.execute("UPDATE settings SET value='1' WHERE key='paused'")
        store.audit(db,'SUBSCRIPTION_PROBE_FINISHED',task_id)
    return result


def main():
    parser=argparse.ArgumentParser(description='Phase2 안전 설정 확인/단발 추론')
    parser.add_argument('action',choices=('preflight','probe'))
    parser.add_argument('--data',default=str(Path(__file__).parent.parent/'data/phase2'))
    parser.add_argument('--workspace')
    parser.add_argument('--approved-once',action='store_true')
    args=parser.parse_args()
    if args.action=='preflight':
        print(json.dumps(discord_preflight(),ensure_ascii=False,indent=2)); return
    if not args.workspace:
        parser.error('빈 전용 --workspace 경로 필요')
    root=safe_root(args.data)
    store=Store(root/'relay.sqlite',{1})  # 로컬 검증 신원. 실제 Discord 신원으로 전송하지 않음.
    executable=shutil.which('codex')
    if not executable:
        result={'status':'WAITING_FOR_USER_SETUP','reason':'CODEX_CLI_NOT_INSTALLED','cli_exec_attempts':0}
    else:
        result=run_probe_once(store,args.workspace,executable,approved=args.approved_once)
    (root/'probe-result.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf8')
    print(json.dumps(result,ensure_ascii=False,indent=2))


if __name__=='__main__':
    try:
        main()
    except Denied as exc:
        print(str(exc)); raise SystemExit(2)
