# 서비스 계정 Codex 구독 인증 초기화

2026-10-24 KST. 서비스 계정 콘솔에서 공식 ChatGPT 구독 로그인·App Server 사용량 조회가 실제 성공했습니다(AUTH_AND_USAGE_VERIFIED). MANUAL 유지·실제 추론0입니다. 부팅 배치 문맥 재확인과 A 활성화는 별도입니다.

## 확인한 공식 방법과 설치 버전

[공식 인증 문서](https://learn.chatgpt.com/docs/auth)에 headless용 `codex login --device-auth`가 제공됩니다. 개인 계정의 ChatGPT 보안 설정 또는 workspace 관리자 권한에서 기기 인증 로그인이 허용되어야 합니다. 이는 브라우저 콜백을 대신하는 방식이며 바울 대표의 직접 인증은 필요합니다.

로컬 공식 실행 파일 `codex-cli 0.164.0-alpha.11.1`의 login --help에서 --device-auth 지원을 확인했습니다. 개인 폴더의 실행 ACL에 서비스 계정이 없어 동일 SHA 실행 파일만 data/runtime/codex에 복사했습니다. 인증·설정 파일 복사나 원본 ACL 변경은 없습니다. 서비스 계정 실제 Inspect에서 context_verified=true·버전·device-auth 지원·공유 실행 경로를 확인했습니다. 이후 대표의 직접 기기 인증으로 subscription_authenticated=true·login_context_unchanged=true·설정SHA 전후 동일·공식 두 사용량 창 조회까지 통과했습니다.

현재 상태: 대표의 직접 로그인은 완료됐으므로 재로그인할 필요가 없습니다. 아래는 완료한 공식 절차와 재현 방법입니다. 배치 검증은 관리자 창에서도 임시 작업 등록 단계 CimException으로 막혀 미검증이며, 이 실패를 구독 인증 실패로 취급하지 않습니다. 기존 부팅 작업은 수정하지 않았습니다.

## 1. 서비스 계정에서 읽기 점검 (현재 통과)

관리자 PowerShell에서:

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\start-service-auth-console.ps1" -Action Inspect
```

Windows 표준 Start-Process -Credential -LoadUserProfile로 바울 대표가 직접 서비스 계정 콘솔을 엽니다. 부모의 개인 프로필/API 비밀 환경을 제거하고 승인 SID의 프로필만 지정합니다. Windows 암호는 로컬 창에만 입력합니다. 자동으로 창을 열거나 로그인하지 않습니다. 기존 Bot/Art/OnBoot/작업 스케줄러를 변경하지 않습니다.

초기 임시 작업 방식은 REGISTER_SERVICE_LOGON_TASK/CimException으로 막혀 CLI에 도달하지 못했습니다. runas 명령은 현재 PowerShell의 네이티브 인수 전달 문제로 사용법만 출력해 폐기했습니다. PowerShell 전용 실행 도구로 같은 서비스 SID·프로필·CLI 읽기 점검은 실제 통과했습니다. 이 이력으로 배치 실행까지 통과했다고 주장하지 않습니다.

이 점검은 프로필이 없더라도 고정 CLI의 SHA·실제 접근·버전·device-auth 지원을 먼저 확인합니다. 서비스 자신의 .codex를 지정하며 개인 계정 인증 저장소를 읽지 않습니다. login status는 자기 프로필이 확인될 때만 실행하고 원문은 버립니다. CLI 자체가 자기 임시 디렉터리를 준비할 수 있지만 이 점검에서 로그인을 시작하거나 설정을 덮어쓰지 않습니다.

## 2. 바울 대표의 직접 기기 인증

1번에서 context_verified=true·CLI 접근·device_auth_supported=true를 확인한 뒤 진행합니다. 다음 명령은 **바울 대표가 방금 열린 서비스 계정 콘솔에서 직접 실행**합니다. Codex가 화면을 조작하거나 이 명령을 자동 실행하지 않습니다.

```powershell
& "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\service-codex-auth.ps1" -Action Login
```

- 서비스 계정 콘솔을 닫았다면 start-service-auth-console.ps1 -Action Login으로 다시 직접 엽니다. Windows 암호는 로컬 입력에만 사용하고 저장하지 않습니다.
- 서비스 계정 자신의 .codex 디렉터리만 초기화합니다. 현재 소유자가 해당 SID인지 확인하고 소유자는 변경하지 않은 채 DACL만 해당 SID·SYSTEM·Administrators로 보호합니다. 다른 사용자 프로필/상위 경로는 수정하지 않습니다.
- 신규 config.toml에 keyring 저장과 ChatGPT 로그인만 지정합니다. 기존 별도 config가 있으면 덮어쓰지 않고 HOLD합니다.
- 기기 인증 코드는 이 로컬 창의 native Codex 출력에만 나타납니다. 바울 대표가 안내된 공식 OpenAI 주소를 브라우저에서 열고 직접 로그인/코드 입력합니다. **코드·토큰·Windows 암호·전체 콘솔 출력은 채팅/Discord/GitHub에 보내지 마세요.**
- keyring은 공식 OS 자격 증명 저장소이며 지원되지 않으면 실패합니다. 파일 저장·API 키로 자동 전환하지 않습니다. Codex 자체의 로컬 로그인 진단 로그도 보호 프로필에 남을 수 있으므로 이를 열어 보내지 않습니다.
- 로그인 완료 뒤 같은 프로세스에서 login status를 분류하고 계정 SID·USERPROFILE·CODEX_HOME·실행 파일 SHA·config SHA 일치를 확인합니다. 이어 공식 App Server의 계정/사용량 읽기만 수행합니다.
- 결과 JSON만 `data/boot/service-codex-auth-local.json`에 남깁니다. 이 JSON에는 기기 코드/토큰/메일을 포함하지 않습니다. 사용자가 결과를 전달할 때도 최종 JSON만 전달합니다.

기기 인증이 비활성/거부되거나 OS keyring을 사용할 수 없으면 HOLD합니다. 다른 계정의 auth.json/키링/토큰을 복사하거나 로그인 정책을 우회하지 않습니다.

## 3. 실제 배치 문맥에서 다시 확인

로컬 콘솔 성공은 배치 계정의 동작을 보장하지 않습니다. 원래 Windows 사용자에서 관리자 PowerShell을 열어 아래 단발 VerifyAuthAsService를 실행합니다. 서비스 계정 콘솔은 일반계정이므로 여기서 실행하면 HOLD_ADMIN_LOCAL_EXECUTION_REQUIRED가 정상 반환됩니다. 이 검증은 새 로그인/기기 코드/AI 없이 같은 SID/프로필/실행파일/설정SHA/login status/사용량만 읽습니다:

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\probe-service-ai.ps1" -Action VerifyAuthAsService
```

READONLY_LINK_READY는 인증/공식 조회가 가능하다는 의미이며 A 활성화나 실행 승인이 아닙니다. MANUAL을 유지합니다. 사용량 두 창 소진/UNKNOWN/오래된 관측은 기존 게이트가 HOLD합니다. 현재 CLI 경로는 고정 SHA로 검증하므로 앱 업데이트로 파일이 바뀌면 자동 우회 없이 다시 검토합니다.

## 인증이 지원되지 않거나 배치 문맥에서 실패할 때의 대안

PIN 전에는 기존 LIMITLESSRelaySvc의 Discord 중계·아트 감시만 유지합니다. 로그인 이후 바울 대표 Windows 계정에서 별도의 읽기 전용 AI Worker를 구동하는 방식을 제안합니다. 이 Worker는 자신의 공식 Codex 인증을 사용하고 기존 단일 SQLite/ResumeQueue·승인·lease·사용량·중복 제한을 사용하며 Bot/Art를 새로 띄우지 않습니다. 서비스 계정으로 비밀을 전달하지 않습니다. 로그아웃/미실행 동안 AI는 HOLD하고 중계는 계속 동작합니다.

이는 **대안 설계이며 이번에 자동 등록/구동하지 않았습니다.** 별도 승인과 두 실행 문맥의 동시 작업 차단·정상 중지 검증이 필요합니다.

## 인증 후에도 남는 A 실운영 조건

새 코드의 정상 로드/동일 Bot·Art lock 확인, 새 정확한 단일 읽기 계획/슬롯 승인, 바울 대표의 명시적 A 활성화, 별도 승인된 읽기 전용 추론1회 전체 검증이 필요합니다. 이번 작업의 실제 추론0·유료전환0·제품/Save/아트 변경0·Push/main 병합0을 유지합니다.

구현 보정: 최초 로그인 전 Set-Acl의 소유자 재지정이 PrivilegeNotHeldException(원본57행)을 일으켰습니다. 일반 서비스 계정에 권한을 추가하지 않고 Get-Acl로 기존 소유자를 보존한 DACL 갱신으로 수정했습니다. 자기 소유 테스트 디렉터리에서 소유자 보존·보호 DACL·쓰기를 검증했습니다. 실제 OAuth 이전 실패였으므로 이를 Codex 구독 미지원으로 분류하지 않습니다.

최종 ACL 보정: 서비스 계정에서는 Get-Acl 기반의 Set-Acl 호출도 PrivilegeNotHeldException(보정본64행)을 냈습니다. Set-Acl을 제거하고 [Microsoft가 문서화한 SetNamedSecurityInfoW](https://learn.microsoft.com/en-us/windows/win32/api/aclapi/nf-aclapi-setnamedsecurityinfow)에 DACL_SECURITY_INFORMATION/PROTECTED_DACL_SECURITY_INFORMATION만 전달하도록 변경했습니다. 소유자·그룹·SACL 포인터는 NULL이며 복원/관리자 권한을 부여하지 않습니다. 고정 자기 프로필·소유자 검사 후 실행하며 Win32 숫자 결과를 기록합니다. 원래 cmdlet 테스트 통과를 서비스 실제 통과로 승격하지 않습니다.


실제 성공 기록: 서비스 콘솔 로그인과 같은 문맥의 login status·설정 SHA·공식 App Server 조회 완료. DACL 네이티브 반환0. 증거 문서/00_프로젝트/Evidence/ServiceCodexAuth20261024/login-success.json. 권한 cmdlet의 두 초기 실패는 수정 후 실제 성공으로 닫았으며, 배치 등록/키링 읽기는 독립 검증 항목입니다.

최종 배치 결과: 관리자 PowerShell의 VerifyAuthAsService가 REGISTER_SERVICE_LOGON_TASK 단계에서 CimException/HRESULT -2146233088로 실패했으며 native_status_hex는 비어 있습니다. Windows 등록 원인 미확인 상태로 남기고 기존 OnBoot2종을 대체/재등록하거나 새 로그인으로 우회하지 않았습니다. 서비스 콘솔 인증·설정 일치·공식 조회 성공은 유효하며, 무인 A 실운영의 배치 검증은 별도 조건입니다.
