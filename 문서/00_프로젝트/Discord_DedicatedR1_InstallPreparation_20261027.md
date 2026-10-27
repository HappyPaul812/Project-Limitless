# Issue #66 — 전용 Worker r1 실제 설치 사전 준비

2026-10-27. **사전 절차 준비 완료, 현재 실제 설치 사전조건은 HOLD**입니다. 새 후보/구조를 만들지 않고 기존 r1을 고정했습니다. 기존 151 PASS는 이전 오프라인 근거로 재사용했고 다시 실행하지 않았습니다. 최신 [기획 판독](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6434585541)의 설치 전후 증거 분리 방침을 확인했습니다.

기준 구현 `60fc6484e9a77b12f52b527b3b18eeb5161d6113`, 작업 시작 HEAD `057d8503bbef69a17cfbf6465d60e5cf90c83c25`, 브랜치 `codex/discord-usage-diagnostics-20261026`. 신규 외부 절차 도구 `Tools/DiscordAIServiceDiagnostics/R1Preparation/prepare-r1.ps1`만 추가하며 r1/V1/r11·기존 DedicatedV2 코드는 수정하지 않습니다. 실제 생성·승인·설치·시작은 이번 범위에 없습니다.

사전 준비 로컬 커밋 `862473f7e66f010acafbe7a99750200fa55a2a64`, [Issue #66 실제 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6434754454). LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 후속 문서 커밋은 보고 연결만 포함합니다.

## 고정 후보와 실제 판독

원본 `F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r1.zip`.

| 항목 | SHA256 |
|---|---|
| Installer | `83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f` |
| Manifest | `43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3` |
| ZIP | `be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25` |
| 새 외부 관리자 절차 도구 | `c6a4b7b6dec6158b7191bdecf120d0c9efb3bc2b668e009aeba104975337671d` |

후보 고정 무결성 확인은 이번 설치 준비를 위한 읽기입니다. 전체151시험/기존 관리자 전수조사/Worker 진단을 다시 실행하지 않았습니다. 내장 `Archive` 순수 검증으로 ZIP 전체·Manifest·Installer·payload766 SHA, entry767·경로/중복/링크 속성 제한을 확인했고 C#은 메모리에서만 로드했습니다.

새 전용 사전검사의 실제 비관리자 결과는 **PASS5 / HOLD7**입니다. 상세 [ReadOnlyPreflight](팀운영/Evidence/DedicatedR1Preparation20261027/ReadOnlyPreflight.json).

| 설치 전 항목 | 실제 판정/근거 |
|---|---|
| r1 정확한 SHA·766+1 | PASS |
| 새 Code/State 경로 부재 | PASS, 읽기 시점의 부재만 확인 |
| 새 IPC namespace 부재 | PASS, 연결하지 않고 이름만 열거 |
| 새 staging root 부재 | PASS |
| 새 승인 root 부재 | PASS |
| 관리자 PowerShell7 x64 | HOLD, 현재 admin=false |
| 새 계정명 2개 부재 | HOLD, Get-LocalUser NullReferenceException / HRESULT -2147467261. 부재·권한거부로 임의 해석하지 않음 |
| 새 Task 이름 부재 | HOLD, UnauthorizedAccessException / HRESULT -2147024891 |
| C:\, Program Files, ProgramData 부모 보호 3개 | HOLD, 고정 native read adapter의 관리자 요구 guard에서 InvalidOperationException / -2146233079. 보호 검증 PASS 아님 |
| 기존 Worker/V1 계정 SID·활성 상태 | HOLD, 이번 provider 조회 NullReferenceException / -2147467261. 기존 관리자 증거는 보존하되 현재 재확인 성공으로 바꾸지 않음 |

최초 출력의 오류 기록을 inner HRESULT까지 보존하도록 새 도구만 보완한 뒤 이 신규 좁은 검사만 재검증했습니다. 최초 결과도 InitialReadOnlyPreflight.json에 남겼습니다. Stage/Install 경로 호출은 모두0입니다. 새 절차 정적 경계 **10 PASS/0 FAIL**, PowerShell 구문 PASS이며 [PreparationTests](팀운영/Evidence/DedicatedR1Preparation20261027/PreparationTests.json)는 기존151이나 native staging 성공을 뜻하지 않습니다.

## 설치 전·설치 후·실행 전 계약

| 시점 | 반드시 확보할 증거 | 현재 상태/실패 정책 |
|---|---|---|
| 설치 전 읽기 | 고정 r1·실행 관리자 실제 SID·새 계정명/Task/Code/State/Pipe 부재·부모 Owner/ACL/실제 경로·기존 Worker/V1 상태 | 위5PASS/7HOLD. 관리자 좁은 사전검사 필요 |
| staging 생성 후, 설치 전 | BA/SY 생성 DACL·모든 파일 Owner/Group/ACL·정확 entry·ZIP/Manifest/766 SHA·handle/FileID·단일 link·ADS/reparse/경로별칭 없음 | 실제 staging 미생성으로 NOT_VERIFIED. 별도 승인 후 생성·readback |
| 계정 생성 직후 | Windows 발급 실제 Bot/Worker SID·관리자/기존 모든 local SID와 분리, Users-only·최소 직접 권리·계정 만료 | **신규 SID 미존재로 설치 전 검증 불가**. 임의 SID를 채우지 않음 |
| 설치 중 readback | 두 계정 Disabled 생성 → 권리/보호 파일/빈 auth/binding → 신규 Worker만 batch-only Enabled; Bot Disabled 유지 | 설치 승인에 이 활성화 범위 포함. 기존 SID1007/1008 활성화·권리 변경0 |
| 설치 직후 정적 감사 | 실제 SID/name/owner/group/DACL·Code/State/Auth 분리·file SHA/binding·Task Owner/DACL/Principal=새Worker PASSWORD1·RunLevel0·Disabled/trigger0/PT90/IgnoreNew/restart0·기존 운영 snapshot 보존·생성 목록/소비/증거 | 실제 설치 전에는 NOT_VERIFIED. 미확인/부분 실패 HOLD, Task 시작 금지 |
| 설치 후 별도 Batch 토큰 검증 | 새 Worker 실제 Session0·Windows Security Batch4·비관리자 토큰·enabled/deny-only/restricted 그룹·유효 Batch allow/deny·로그온 범위 | Disabled Task 등록만으로 실제 토큰이 만들어지지 않음. 별도 읽기 토큰 시험 승인/도구 준비 필요, 이번 실행0 |
| 실제 FAKE_ONLY IPC 전 | 새 SID의 GROUP/직접 launcher 영향·자격증명 비공유·lease/consume/승인·PID생성시각·Job 정리·Bot 제한 Batch client 경로 | 별도 승인, 현재 HOLD. 설치 성공과 IPC 승인 구분 |

새 Worker 이름 `LIMITLESSWorkV2D`, Bot `LIMITLESSBotV2D`. 설치의 `ENABLED_BATCH_ONLY`는 계정 종류가 아니라 **Batch 허용 + interactive/RDP/network/service 로그온 deny + Users-only + 전용 Task Disabled**를 조합한 계약입니다. OS의 일반 Enabled 속성만으로 로그온 종류가 제한되는 것은 아닙니다. 기존 운영 Worker나 관리자 인증은 복사하지 않습니다. 새 Auth/CODEX_HOME은 EMPTY이며 실제 로그인/AI 기능은 이번 후보의 범위가 아닙니다.

직접 LSA 권리 readback만으로 그룹/GPO를 포함한 유효 Batch 로그온 허용을 단정하지 않습니다. 상속된 `SeDenyBatchLogonRight`가 적용되면 Batch 허용을 이깁니다. 불명/거부면 정책을 변경하지 않고 HOLD합니다. [Microsoft Deny batch 정책](https://learn.microsoft.com/en-us/previous-versions/windows/it-pro/windows-10/security/threat-protection/security-policy-settings/deny-log-on-as-a-batch-job).

## 기존 GROUP 작업과 새 SID

기존 관리자 정본의 GROUP92+직접 Worker4 결과를 재사용합니다. 92개를 신규 Worker 작업으로 판정하지 않으며 기존 SID1007 토큰 그룹을 새 Worker 토큰으로 간주하지 않습니다. Everyone/AuthenticatedUsers/Users 같은 그룹 영향은 새 토큰에서도 별도 판독해야 합니다. 로컬/내장/도메인·중첩 그룹, enabled/deny-only/restricted 속성과 Task의 실제 실행 권한/세션 요건을 구분합니다. 미지원·조회 실패는 UNKNOWN/HOLD입니다.

Scheduler GROUP4는 그룹 기반 활성화이고 Security 로그의 Batch4와 별개입니다. [Microsoft TASK_LOGON_TYPE](https://learn.microsoft.com/en-us/windows/win32/api/taskschd/ne-taskschd-task_logon_type). 새 계정이 없으므로 지금 GROUP 실행 불가를 증명할 수 없습니다. interactive deny는 제한 설계이며 기존 토큰/관리자/비협조 launcher까지 봉쇄했다는 증거가 아닙니다.

**고정 r1의 현재 Host 코드는 실행 가능 GROUP Task를 만나면 그룹 무관성을 자동 판별하지 않고 HOLD합니다.** 기존 관측 같은 GROUP 상태가 유지되면 향후 Host도 막힐 수 있습니다. 설치 후 무관성이 확인돼도 코드 guard가 자동 해제되는 것은 아닙니다. 이번에는 guard·기존 Task를 변경하지 않고, 설치는 Disabled/미시작 자산 배치에 한정합니다. 후속 IPC 전에 이 guard의 처리와 추가 증거를 별도 검토해야 합니다. 같은 PC/구독·전역 공용 자원 충돌도 신규 SID만으로 해소되지 않습니다.

## 관리자 절차와 승인 분리

복사 가능한 **각각 한 줄 명령**은 [관리자 명령](Discord_DedicatedR1_AdminCommands_20261027.md)에 있습니다. 새 helper를 메모리로 읽어 위 SHA와 비교한 바이트만 새 NoProfile PowerShell7에서 실행하므로 사용자 수정 가능한 F: 스크립트를 무검증 실행하지 않습니다. 실행 중인 부모 PowerShell 창을 닫는 exit 명령은 없습니다.

1. **지금 실행 가능한 읽기 전용 Preflight:** 새 namespace/부모·계정 provider/Task 부재만 좁게 확인합니다. 새 SID/토큰/실제 Job은 NOT_VERIFIED로 명시합니다. 결과가 HOLD이면 생성 단계로 넘어가지 않습니다. 원시 예외문·명령줄·환경·암호·인증을 출력하지 않습니다. 정제 JSON만 반환합니다.
2. **staging 전용 별도 승인 후 Stage:** `STAGE_R1_ONLY_NO_INSTALL` 범위. 신규 root `C:\Program Files\LimitlessRelayDedicatedV2Staging20261027`, package `a5af3087b78b45d1baae46067d5b7ac9\Bundle` 및 Bundle.zip만 생성합니다. 부모 전체 NTFS/경로/owner/writer 검증 → source handle/고정 SHA → SECURITY_ATTRIBUTES BA/SY protected DACL CREATE_NEW → 모든 파일 실제 metadata/SHA/readback·767 entries 확인 → handle 재조회입니다. 일반 사용자 쓰기는 ACL grant하지 않으며 예상 밖 ACE·alias·hardlink·ADS·reparse·파일을 거부합니다. 기존 원본 F: 폴더/ZIP/ACL은 수정하지 않습니다. 기존 staging root가 있으면 이번 절차는 재사용/삭제하지 않고 HOLD합니다.
3. **실제 설치 별도 승인 후 Install:** `INSTALL_R1_ONCE_TWO_NEW_ACCOUNTS_BATCH_WORKER_DISABLED_TASK` 범위. 읽기 사전조건을 해당 시점 재확인하고 protected staging 전체를 다시 검증한 handle을 설치 종료까지 유지합니다. 관리자 전용 새 `C:\ProgramData\LimitlessRelayDedicatedV2Approvals`와 단발 승인 JSON을 CREATE_NEW/DACL readback으로 만듭니다. 실제 관리자 SID·3SHA·schema3/새epoch·두계정명·15분만료·preserve_existing·ENABLED_BATCH_ONLY를 묶습니다. 승인 root가 이미 있으면 재사용/재시도 없이 HOLD합니다. 새 PowerShell7에서 보호된 r1 Installer만 실행하고 Bot/Worker 암호를 **로컬 SecureString 두 번** 입력합니다. CLI/파일/로그에 암호·기존 Token을 넣지 않습니다. 후보 Manifest의 approved=false는 유지하며 외부 설치 승인과 분리합니다.
4. **설치 결과 후 정지:** helper는 native 결과를 정제 JSON으로 보존/반환합니다. 설치기는 소비 후 보호 Result JSON/생성 SID를 승인 root에 기록합니다. helper 성공도 `INSTALL_REPORTED_COMPLETE_POSTINSTALL_AUDIT_REQUIRED`이며 runtime은 HOLD_NO_START입니다. 별도 설치 후 감사가 끝나도 IPC/Task 시작·Bot 활성화·인증은 자동 실행하지 않습니다.

Step2/3 명령은 **현재 승인·실행 대상이 아닌 후속 절차**입니다. helper의 명시 scope 문자열 자체가 이번 승인이라는 뜻은 아닙니다. 바울 대표의 다음 승인과 관리자 직접 실행이 모두 필요합니다. 정책/권한을 완화하거나 새 SID의 토큰 검증을 설치 전 PASS로 대체하지 않습니다.

## 실패·부분 자산·증거·복구

- 어느 단계든 HOLD/오류/불명 시 중지합니다. 무조건 재실행·같은 승인 재사용·자동 삭제·ACL 완화·기존 Cleanup/Install 호출을 하지 않습니다. 실제 staging/계정/Task는 이번에 생성되지 않았으므로 이번 복구 대상 운영 자산은 없습니다.
- Stage 중단: 신규 staging 아래 확인된 partial 파일과 metadata·오류코드 JSON을 보존합니다. 생성 완료라고 추정하지 않습니다. 같은 root로 자동 재시도하지 않습니다.
- 승인 root만 생성된 중단: 해당 root/approval 파일을 보존하고 소비 전/후를 증거로 구분합니다. 승인 미소비가 재실행 허가는 아닙니다. helper는 기존 root 재사용을 거부합니다.
- 계정/권리/파일/Task 중단: 실제 생성 SID·description의 package·정확 경로·Owner/DACL·SHA·Task명이 신규 목록과 일치하는지 관리자 읽기 감사로 확인합니다. Worker가 enable 후 Task 등록에 실패했다면 신규 Worker가 Enabled일 수 있으므로 **자동 비활성화도 하지 않고** 우선 HOLD·상태 보고 후 해당 신규 SID만 비활성화/격리할 별도 승인을 받습니다. 기존 Worker/V1은 대상으로 삼지 않습니다.
- 원상복구는 “기존 상태를 건드리지 않은 채 신규 자산 목록을 보존”하는 것이 첫 단계입니다. 후속 정리 승인을 받으면 토큰/실행/참조 부재 확인 후 정확한 신규 자산만 다루는 새 계획을 작성합니다. 새 승인·후보 재사용 가능성도 재심사하며 기존 소비/승인 증거는 보존합니다.
- 결과 JSON의 코드/상태만 보고하고 명령줄·환경·인증·암호·Raw task XML은 저장하지 않습니다. 관리자 출력은 필요하면 프로젝트 Temp의 새 이름 JSON으로 수동 보존합니다. 보호 설치 증거는 별도 승인 root의 `dedicated-install-a5af3087b78b45d1baae46067d5b7ac9\Result-*.json`이며 소비 전 오류에는 이 파일이 없을 수 있습니다.

## 완료 범위와 다음 행동

새 helper·명령 3개·전후 체크리스트·10개 정적 검사·정제된 비관리자 읽기 결과를 준비했습니다. **다음 최소 행동은 관리자 Preflight 한 줄만 실행하고 JSON을 판독하는 것**입니다. staging·승인 파일 생성·계정2/권리·Task1·설치는 각각 다음 승인 없이는 실행하지 않습니다.

기존151 재실행0, r1/r11/V1 후보 수정0, 실제 계정/권리/ACL/staging/승인/Task/설치/로그인/IPC/Worker/AI/usage/Discord/A/Push/main병합/종료·재부팅0. 기존 Worker1007/11개·V1 REFERENCE_VERIFICATION_HOLD·OnBoot2·봇/아트·CODEX_HOME/인증·운영SQLite·게임/Unity/Save 보호, MANUAL/A HOLD 유지. 준비 완료 후 중지합니다.
