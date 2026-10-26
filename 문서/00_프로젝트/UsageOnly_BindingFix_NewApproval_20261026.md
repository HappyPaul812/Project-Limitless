# Issue66 PowerShell 수정본 신규 승인 단발 시험 — 관리자 실행 대기 (2026-10-26)

**NEW_OWNER_APPROVAL_READY / WAITING_LOCAL_ADMIN_EXECUTION / LIVE_USAGE0**.

바울 대표가 e10d02cbb4fccc9679a2ebd035c8426f3c603557의 변수 수정본으로 새 usage-only 시험1회를 명시적으로 승인했습니다. 과거 승인 슬롯을 초기화해 재사용하는 절차가 아닙니다.

## 정확한 직접 실행 명령

관리자 PowerShell에서 다음을 실행합니다. 서비스 Windows 암호는 로컬 Get-Credential에만 입력하며 채팅으로 보내지 않습니다. 이 명령은 **이번 새 승인에 해당하는 실제 사용량 최대1회**를 수행합니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\run-approved-usage-once.ps1"
```

성공·실패 무관 결과를 확인하고 중지합니다. 명령을 다시 실행하거나 마커를 삭제하지 않습니다. 모델/A/Discord/SQLite업무·운영코드/CLI·CODEX_HOME·인증 변경은 허용하지 않습니다.

## e10 수정 재사용과 최소 추가 범위

- serviceCodexHome / childSummaryPath rename을 그대로 재사용했습니다. 자동HOME와 switchChild를 건드리지 않습니다. 부모·자식 AST변수 충돌0 확인.
- 이번 승인키는 **BINDINGFIX_E10D02CB_20261026**입니다. 새 launch marker는 `C:/ProgramData/LimitlessRelay/UsageOnly20261026.bindingfix-e10d02cb.launch-consumed.json`입니다. 시작 직전 CreateNew로 소비하며 같은 명령의 두 번째 실행은 HOLD합니다. 아직 만들지 않았습니다.
- 이전 `UsageOnly20261026.launch-consumed.json` SHA `62a867f542d6800a68f9a626d1c792ace7143081a7ff8e15ce1638b412cab97b`와 request987a...·배포본·진단은 그대로 보존합니다. 실제 실행 전후에도 이 SHA를 비교하며 삭제/덮어쓰기하지 않습니다. 새 request ID는 관리자 실행 시 Guid로 발급하고 이미 존재하는 trial 폴더는 거부합니다.
- 기존44개 runtime 승인맵·보존40/변경3+신규1·PowerShell 보안모듈은 바꾸지 않았습니다. 기존 정상검사 전체를 반복하지 않되, 새 보호 배포 경로의 필수44 SHA gate와 entry의 실행 전 검사는 생략하지 않습니다.
- 성공 시 공식 사용량 결과를 보고하기 위해 Python entry가 **같은1회 응답의 정규화된 source/observed/ordinary_allowed/windows 숫자**만 결과에 포함하도록2줄을 추가했습니다. 조회/인증/차단 로직은 그대로이며 provider.read는 정적으로1곳입니다. 새 entry SHA를 정확한 permit과 ZIP에 반영했습니다. 재조회·계정정보/토큰 출력을 추가하지 않았습니다.
- PowerShell은 숫자보고의 필드4개·windows2개/각3필드·유한숫자·범위/시간·boolean을 검사한 뒤 안전한 숫자만 복사합니다. 추가 개인정보필드·숫자가 아닌 값·누락은 HOLD합니다.

## 변경 SHA와 패키지

| 대상 | SHA256 |
| --- | --- |
| e10 변수수정 launcher 기준 | 3a57e70ac09ac2d357d4224ef4cc9c5ad23d7610f499380436eeb2aac80fec74 |
| 새 승인 launcher | 7fe0b17f73c6a55fd3a676d10bddc5060a7e9f335d40a8e95f8f50a850bd39aa |
| 새 숫자보고 entry | 670c922eb22d721e2c79eb52ef39e54c9ca627cefa0d7bfd38cf94c9eeeb38ed |
| 새 46파일 ZIP | 55241c88d0f43c518faac6f66d7b892fc3128e4bffa320bfc899f613e5b466a9 |
| 새 비활성 permit 초안 | 1f561edc3f68e1c7fd599b1608c4b43613e80e9a30705d89e27673bb6f9fae79 |

ZIP은 `Temp/DiscordUsageDiagnostics20261026/UsageTrialLaunchBindingFix20261026/usage-package.zip`입니다. 이전 ZIP395264...와 ReviewPackage·이전초안을 덮어쓰지 않았습니다. 새 permit 초안은 approved=false/만료0이며 실전 parent가 배포검증 PASS 후 새 request/정확trial루트/유효기간으로만 활성화합니다. 기존 boot-approval는 그대로입니다.

## 검증과 실제 현재 상태

- 이번 요청의 표적 변수 회귀3 PASS, 새 숫자보고 경계3 PASS: 값보존/개인정보추가필드차단/비숫자차단. entry구문·provider.read1곳·보고대입1곳 정적검사 PASS. launcher최종parser/비관리자무변경가드/diff검사 PASS.
- 기존44/22+12/Job4 전체검사 재실행0. runtime44 맵 비교만 동일 확인. 기존17초내 잔여Python예산·20초Scheduler·KillOnJobClose·Session0/logon4·Password/Limited·재시작0/단발마커를 유지합니다.
- 현재 **새 ProgramData배포/permit/새request/새launchmarker/실조회/모델/Discord0**입니다. 준비물은 격리worktree와새Temp ZIP에만 있습니다. 기존마커/ZIP의 SHA 동일 확인. 운영파일·OnBoot/봇·아트·SQLite/MANUAL·인증·ACL·Unity/Save/게임·Push/main병합·PC종료/재부팅 변경0.
- 현재agent는 비관리자이므로 실제 서비스 실행을 대신하지 않습니다. 대표 직접 실행 결과를 기다리며 새 승인을 다시 묻지 않습니다.

## 결과와 종료

결과 위치는 `C:\ProgramData\LimitlessRelay\UsageTrials\<새request>\Output\summary.json`입니다. 서비스 실제계정/Session0·logon4/시도수·성공시 entry_result.official_usage·실패시 rpc.method/error_code 및 자기프로필 정제로그 참조·Job자식정리·permit_deactivated/cleanup/boot·ledger·approval·previous_launch_marker_preserved를 확인합니다. 상세stderr/오류메시지는 서비스자기보호프로필에만 남습니다.

성공·실패와 관계없이 해당nonce permit을 활성경로에서 비활성보관본으로 옮기고, 해당임시작업만 정리하며 모든소비마커/증거를 보존합니다. UNKNOWN_MAX1/오류/HOLD 상태에서 재시도하지 않습니다. 실제AI/A는 여전히 미승인입니다.

브랜치 codex/discord-usage-diagnostics-20261026. 최종commit은Issue66/최종보고참조. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / OWNER_ADMIN_ACTION_PENDING**.
