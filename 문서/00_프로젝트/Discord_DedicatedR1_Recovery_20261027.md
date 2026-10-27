# Issue66 — r1 설치 재개 요청 / 최소 읽기 감사

바울 대표의 “다시 해줘” 요청에 따라 실패 설치의 재개 준비를 진행했습니다. 기존 설치 명령을 그대로 반복하거나 승인/소비를 삭제하는 승인이란 뜻으로 처리하지 않습니다. 대표 직접 관리자 실행·기존 자산 보존·단발/오류HOLD 조건을 유지합니다. 현재는 **HOLD_REQUIRES_RECOVERY_REVIEW**, 설치 재시도0입니다.

실제 읽기에서 `C:\ProgramData\LimitlessRelayDedicatedV2Approvals` 폴더 존재를 확인했으나 내부 열거는 access denied입니다. 비관리자 권한으로 승인/소비/계정/Task를 확정하지 않습니다. 별도 비밀 없는 child 메타 조회에서 stdin/stdout redirected=true를 관측했으나 이 비대화형 환경 결과를 대표 관리자 창의 정확한 오류 원인으로 단정하지 않습니다.

새 `Tools/DiscordAIServiceDiagnostics/R1Preparation/probe-install-hold-readonly.ps1`은 승인 root/고정 승인JSON binding·만료·관리자일치, 소비 디렉터리/마커/lease, 신규 Code/State, 계정2 SID/활성 상태, 고정 Task의 metadata 및 protected Result 개수만 읽습니다. 암호/Token/환경/명령줄/인증/Result본문을 읽지 않고 생성·수정·삭제·설치·LSA·Task실행 경로가 없습니다. 보호 부모를 읽지 못한 경우 자식의 ItemNotFound를 ABSENT로 오인하지 않고 UNKNOWN으로 보존합니다.

도구 SHA256 `c34be6d7bd023c3575b2cf16a2b53f4a06c3225209fcfff42edf8916ffad5874`. PowerShell 구문 PASS, 비관리자 신규 진단 결과는 UNKNOWN9개·Code/State 부재2개, changes/install/password/IPC/model0입니다. [정제 결과](팀운영/Evidence/DedicatedR1Preparation20261027/InstallRecoveryReadonlyNonAdmin.json). 이는 기존12/10/151 재실행이 아니며 protected 상태 PASS가 아닙니다.

대표가 관리자 PowerShell7에서 아래 읽기 한 줄만 실행하고 JSON을 전달합니다. 암호를 묻지 않으며 창을 닫는 exit가 없습니다.

```powershell
$r1ProbeBytes=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\probe-install-hold-readonly.ps1');if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r1ProbeBytes)).ToLowerInvariant() -cne 'c34be6d7bd023c3575b2cf16a2b53f4a06c3225209fcfff42edf8916ffad5874'){throw 'R1_PROBE_SHA_HOLD'};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r1ProbeBytes)))
```

읽기 도구 자체는 retry_authorized_by_probe=false이며 어느 결과도 자동 설치를 실행하지 않습니다. 관리자 JSON 수신 후 consumed 여부·기존 계정/Task·approval 만료와 정체를 분리해 판단하고, 입력 handle 경계의 필요한 수정/복구 범위를 검토합니다. 현재 고정r1/기존helper/설치기/승인/소비/staging 불변, 암호변경/재발급승인/자동삭제/계정설치0입니다. 유효 토큰·GROUP/Session0IPC/Job독점·V1/MANUAL/A HOLD·기존 운영/인증 보존, Push0. 브랜치 codex/discord-usage-diagnostics-20261026, 시작HEAD88e2c71bbec6165b3c29747149cfcaf4b55f4574.
