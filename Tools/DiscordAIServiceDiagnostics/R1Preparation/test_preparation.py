"""새 관리자 절차의 정적 경계만 검사합니다. 기존151/실제 Stage·Install을 실행하지 않습니다."""
import hashlib,json,re,sys
from pathlib import Path
here=Path(__file__).resolve().parent
script=here/'prepare-r1.ps1';raw=script.read_bytes();s=raw.decode('utf8');rows=[]
def check(name,ok):rows.append({'name':name,'status':'PASS' if ok else 'FAIL'})
check('DEFAULT_READONLY',"[string]$Step='Preflight'" in s)
check('EXPLICIT_STAGE_APPROVAL',"$ApprovalScope -cne 'STAGE_R1_ONLY_NO_INSTALL'" in s and s.index("throw 'R1_SEPARATE_STAGE_APPROVAL_REQUIRED'")<s.index('[LimitlessV2Staging]::Create('))
check('EXPLICIT_INSTALL_APPROVAL',"$ApprovalScope -cne 'INSTALL_R1_ONCE_TWO_NEW_ACCOUNTS_BATCH_WORKER_DISABLED_TASK'" in s and s.index("throw 'R1_SEPARATE_INSTALL_APPROVAL_REQUIRED'")<s.index('[LimitlessV2Publish]::NewDirectory('))
check('PREFLIGHT_HOLD_BLOCKS_MUTATION',s.index("throw 'R1_PRECONDITIONS_HOLD'")<s.index('[LimitlessV2Staging]::Create('))
check('FIXED_R1_BYTES',all(v in s for v in ('83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f','43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3','be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25')))
check('NO_OLD_PACKAGE_OR_LOGIN',not re.search(r'r11|LIMITLESSBotTrialV2|codex login|auth\.json|ChatGPT',s))
check('NO_DELETE_RETRY_OR_EXIT',not re.search(r'(?im)^\s*(exit|Remove-|Stop-Process|Restart-|Set-Acl|Enable-LocalUser|New-LocalUser|Start-ScheduledTask)',s) and 'automatic_retries=0' in s)
check('DEFERRED_NEW_TOKEN_NOT_PASS',all(v in s for v in ('OS_ISSUED_NEW_SIDS','NEW_ACCOUNT_GROUPS_EFFECTIVE_BATCH_RIGHTS','NEW_SID_GROUP_TASK_APPLICABILITY','HOLD_NEW_TOKEN_GROUP_EXCLUSIVITY_NOT_VERIFIED')))
check('APPROVAL_DISTINCT_EMPTY_AUTH_CONTRACT',"kind='DEDICATED_INSTALL_ONCE'" in s and "worker_install_state='ENABLED_BATCH_ONLY'" in s and "expires_unix=$now+900" in s)
check('PROTECTED_STAGE_HANDLES_HELD_ACROSS_INSTALL',"$session=[LimitlessV2Staging]::Verify" in s and '$session.Recheck();$phase=\'FIXED_INSTALLER_FRESH_CHILD\'' in s and s.index('}finally{if($session)')>s.index('$json=&'))
out=Path(sys.argv[1]);out.mkdir(parents=True,exist_ok=True)
digest=hashlib.sha256(raw).hexdigest()
base='& \'C:/Program Files/PowerShell/7/pwsh.exe\' -NoLogo -NoProfile -Command \'$b=[IO.File]::ReadAllBytes("'+str(script)+'");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne "'+digest+'"){throw "R1_HELPER_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b))) '
commands={
 'Preflight_READONLY_NOW':base+'-Step Preflight\'',
 'Stage_FUTURE_SEPARATE_APPROVAL':base+'-Step Stage -ApprovalScope "STAGE_R1_ONLY_NO_INSTALL"\'',
 'Install_FUTURE_SEPARATE_APPROVAL':base+'-Step Install -ApprovalScope "INSTALL_R1_ONCE_TWO_NEW_ACCOUNTS_BATCH_WORKER_DISABLED_TASK"\''}
(out/'Commands.json').write_text(json.dumps(commands,ensure_ascii=False,indent=2)+'\n',encoding='utf8')
result={'kind':'NEW_PREPARATION_STATIC_CHECKS_NOT_151_RERUN','pass_count':sum(r['status']=='PASS' for r in rows),'fail_count':sum(r['status']=='FAIL' for r in rows),'helper_sha256':digest,'tests':rows,'actual_staging_install_calls':0}
(out/'PreparationTests.json').write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf8');print(json.dumps(result))
if result['fail_count']:raise AssertionError('PREPARATION_HOLD')
