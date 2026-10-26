# Issue66 부분 설치 읽기 전용 조사 — NOT_VERIFIED / 관리자 진단 대기

2026-10-26. 기준 `a0037099cb3d9ef86d5f28b7bdc56f63c1d5f738`, branch `codex/discord-usage-diagnostics-20261026`. 이전 설치 요청 `0f8e15c951a144efa79f5ccdbeed9c55`는 **Install1 소비 완료**로 유지합니다. 재설치·복구·삭제·활성화·등록·IPC를 하지 않았습니다.

## 직접 원인 판정

**설치 실패 직접 원인 NOT_VERIFIED**. HRESULT -2146233087은 이번 기록의 RuntimeException 값이며, 이 값만으로 Win32/LSA/컴파일/암호 오류를 구분할 수 없습니다. 승인 설치기175행의 고정 문자열 throw가 원래 ErrorRecord/InnerException을 이어 전달하거나 저장하지 않습니다. 기존 JSON에도 원래 예외가 없습니다. 이를 현재 진단 오류의 접근 거부와 혼동하면 안 됩니다.

확정된 실행 경계: 이전 관리자 결과에서 계정1008 생성 및 실패 catch의 비활성화가 기록됐고, State root는 미생성입니다. 정적 코드 순서상 Add-LocalGroupMember→SID검사→TrialAccountPolicy Add-Type/GrantNewBotBatch→Manifest 생성→설치 ACL→State 생성 사이를 조사해야 합니다. **어느 한 함수가 실패했다고 단정하지 않습니다.** 현재 일반 토큰은 해당 보호 파일과 로그를 읽을 수 없어 더 좁히지 못했습니다.

창 종료는 별개로 확정했습니다. 기존 보조 명령185행 `exit 2`를 현재 대화형 PowerShell 메모리 ScriptBlock에서 호출하도록 안내했기 때문에 HOLD 후 PowerShell 세션까지 종료됐습니다. 새 읽기 전용 진단에는 ExitStatement가 없고 기존 설치 스크립트를 실행하지 않습니다.

## 실제 상태와 한계

| 항목 | 이번 확인 | 미확정 사항 |
|---|---|---|
| 기존 설치 증거 | SHA `80523d2b2320cabce31789d7322a3a7c480ff8f225d42c1f0120a3bdcbea5d6b` 동일, Install1 | 원래 예외 본문/InnerException 미보존 |
| 시험 계정1008 | 이전 관리자 JSON에서 Enabled=false | 현재 계정 조회 NullReferenceException. 그룹/만료/직접배치권한·현재활성 상태 재확인 못함 |
| 시험 task | 현재 Scheduler 접근 거부(0x80070005) | 존재/부재/owner/DACL/Principal/action 전부 UNKNOWN. 삭제됐다고 판단하지 않음 |
| Program Files 설치 root | 현재 존재·reparse 아님 | 내부목록/ACL/855파일 SHA 조회 거부 |
| Code/HostManifest/DeploymentRecord | 보호된 부모 때문에 조회 불가 | ItemNotFound 응답만으로 부재 판단하지 않음 |
| ProgramData State root | **읽기 가능한 부모 목록에서 부재 확인** | 설치기 단계별 원래 예외는 별도 조사 필요 |
| PowerShell/TaskScheduler 오류로그 | 현재4개 채널 모두 조회 거부 | 로그 없음/원본예외 복구불가로 단정하지 않음 |
| OnBoot2개 | 이전 실제 관리자 Install 전후 SHA 동일 증거 보존 | 이번 토큰에서 export 재조회 불가 |
| boot approval | SHA3e032ebc… 현재 동일 | 내용/인증자료 읽지 않음 |
| 승인 패키지 | Installer117065bb…/Manifest6efbd783…/zip4b2c56ed… 현재 동일 | 승인 파일 수정0 |

남은 자원: 코드 설치 root 확정, 신규 계정1008의 생성/Disabled 기록 확정, 내부 파일·Manifest·task 실제 상태는 관리자 읽기 전용 확인 필요. 기존 소비/결과 기록을 지우거나 초기화하지 않았습니다.

## 새 진단 도구 및 오프라인 검증

`Tools/DiscordAIServiceDiagnostics/probe-partial-install-readonly.ps1` 신규. SHA `2e53e4898c10fd0ddd8654b1a368ec1e386ac114dc9e8f2393ff40780f4922b8`.

설치/Remove/Inspect 등 기존 실행기를 전혀 호출하지 않습니다. 계정/그룹/LSA 직접권한, task GetTask/GetSecurityDescriptor, code/state 파일 메타데이터·SHA·ACL, 고정855개 payload SHA, 기존 부팅 작업 XML의 SHA만 읽습니다. LSA는 읽기 권한0x800 handle만 사용하며 계정권한 부여/제거 API가 없습니다.

과거 로그는 Install 결과 파일 시각 기준 약12분, 지정4개 로그의 오류/경고 각최대30건으로 제한합니다. 원문 저장/출력은 없고 이 설치와 관련된 marker가 있는 경우 CS오류번호/HRESULT/고정 함수명·오류분류만 반환합니다. 타 로그의 내용이나 명령/인수/암호/token/인증 저장소는 수집 대상으로 삼지 않습니다. 실제 task의 전체 arguments도 출력하지 않고 hash만 기록합니다.

출력은 정제 JSON과 예외 type/HRESULT/InnerException chain/Win32 native code입니다. 부재 판정에는 읽기 가능한 부모목록 또는 관리자 task exact-not-found+hidden 포함 열거 확인을 요구합니다. 조회 실패/모순은 UNKNOWN으로 유지합니다. 증거 JSON만 작업 브랜치에 새로 작성하고 시스템 상태와 이전 기록은 변경하지 않습니다.

새 `test-partial-readonly-offline.ps1` **16 PASS/0 FAIL**: 구문/no-exit/설치·변경명령 없음/변경 COM 없음/readonly C#컴파일(native0)/inner code 보존·메시지 제외/조회실패와 부재 구분/관리자·비관리자 fake Scheduler/모순·권한거부/로그 비밀정보 제거·길이 제한. 기존29/35 전체 테스트는 반복하지 않았습니다. 오프라인 도구 검증은 실제 관리자 상태조회 PASS와 다릅니다.

현재 일반 토큰에서 최종 진단 `328ab78096e14557837c5e2af2727e10` 실행 완료, 최종상태 NOT_VERIFIED_HOLD. 관리자 진단은 아직 미실행입니다.

## 관리자 PowerShell7 읽기 전용 명령 — 암호 입력 없음

아래 한 줄은 새 진단 도구 bytes SHA를 확인한 뒤 그 bytes만 실행합니다. `exit`가 없어 창을 종료하지 않습니다. 기존 설치 명령은 다시 실행하지 않습니다.

```powershell
& { $diagBytes=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\probe-partial-install-readonly.ps1'); if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($diagBytes)).ToLower() -ne '2e53e4898c10fd0ddd8654b1a368ec1e386ac114dc9e8f2393ff40780f4922b8'){throw 'DIAGNOSTIC_SHA_HOLD'}; & ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($diagBytes))) }
```

비밀번호/토큰 입력 요청이 나오면 진행하지 않습니다. 이 도구에는 해당 입력 기능이 없습니다. 출력된 JSON만 인계받아 기존 증거와 대조합니다. 관리자 진단 후에도 자동복구/재설치/IPC 없이 중지합니다.

## 변경 필요 코드·SHA와 최소 후속 계획

아래는 **제안**이며 기존 승인 코드 수정0입니다.

| 파일 | 현재 SHA256 | 필요한 변경/조건 |
|---|---|---|
| install-session0-trial.ps1 | `117065bbfca686474f9104bc0dff20fa6528dfb9c204e5a2f2720d91994958e9` | 실패 단계 및 원래 예외/InnerException 정제 보존 후 HOLD. 원인 확정 뒤 해당 함수만 최소 수정 후보화. 지금 수정/실행하지 않음 |
| run-atomic-trial-install.ps1 | `75ce33d60c34327b37d61cabd2fa362941933a280f919426c01eb7c9118bab70` | 대화형 exit2 제거 또는 자식 실행 격리. 이전 Install1 소비/중복 차단 유지. 지금 실행 금지 |
| TrialAccountPolicy.cs | `1d85ea2b3b99027dbf0eeb4ee61b590b8be3539d7c9e7992af0dee03c8d8d96e` | 현재 원인으로 확정하지 않음. 실제 직접권한/로그 결과가 근거가 될 때만 검토 |
| 새 readonly 진단 | `2e53e4898c10fd0ddd8654b1a368ec1e386ac114dc9e8f2393ff40780f4922b8` | 관리자 읽기 전용 실행만. 설치 payload/승인 목록 추가 아님 |
| 새 offline test | `bc1d7826bc42709f3b5c03b5ba8024127424fbb2f1f7b78ec76ba230e98a9023` | 합성검사16 PASS. Windows 권한 검증 대체 아님 |

1. 관리자 진단 증거로 직접 원인을 확정하거나 NOT_VERIFIED 유지.
2. 원인이 확인되면 최소 수정·오프라인 검증·새 Installer/Manifest/ZIP SHA를 보고. 기존 승인 파일 불변 유지.
3. 부분 자원은 자기 신규 SID/path/task/record 일치가 확인된 경우에만 별도 승인받아 정리. 기존 부팅·운영 계정·ACL·인증은 복구 대상으로 삼지 않음.
4. 새 후보 SHA와 기존 부분 자원 정리 범위에 대한 **대표의 별도 복구·재설치 승인** 전 재설치 금지. 이전 Install1을 재사용하지 않고 신규 범위를 명확히 구분.

## 운영 보호/기록

이번 추가 설치/계정·task·service·ACL 변경/활성화/삭제/IPC/Fake/모델/usage/Discord/운영SQLite/인증자료 읽기0. MANUAL/A HOLD 유지. 기존 봇/아트 재시작0, Unity/게임/Save/Push/main/PC 변경0. 원본 tracked dirty308/untracked36 동일. 실제 운영 건강을 새로 검증했다고 주장하지 않습니다.

증거: `팀운영/Evidence/Session0PartialReadOnly20261026/`의 최종328ab780…json, OfflineTests.txt, SourceSHA256.json 및 이전11088f28…json. CURRENT_STATUS/CODEX_LATEST 및 Issue66에 실제 상태를 기록합니다.

**읽기 전용 조사 결과는 NOT_VERIFIED/HOLD. 관리자 증거 수집을 제외한 복구·재설치는 진행하지 않습니다.**
