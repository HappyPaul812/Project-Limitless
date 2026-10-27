# 전용 Worker r1 관리자 PowerShell 7 명령

최신 native 설치 결과는 HOLD입니다. **아래4번을 포함해 설치/재개 명령을 다시 실행하지 않습니다.** 원본 승인 보존/새승인/marker 생성·자식1회는 이미 보고됐습니다. 현재는 [Recovery보고 최상단의 guard/현재상태 읽기 전용 명령](Discord_DedicatedR1_Recovery_20261027.md)만 실행 대상으로 안내합니다.

후속 재개: 관리자 읽기에서 만료 승인만 남고 계정/Task/소비/설치 경로가 없음을 확인했습니다. 앞선 대표 재개 요청에 따른 **4번 새 보존형 재개 명령**만 사용합니다. 기존1/2/3은 이력이며 재실행하지 않습니다. 4번은 원본 승인 보존 이동·새15분 승인·고정 설치기 콘솔 실행을 최대1회 수행하므로 내용을 확인한 후 대표가 직접 실행합니다.

2026-10-27 후속: Stage 성공 수신 후 바울 대표가 **3번 Install의 계정2·Task1 설치1회만 승인**했습니다. 대표가 관리자 PowerShell7에서 3번만 최대1회 직접 실행합니다. **1·2번 재실행 금지.** 도구SHA/고정3SHA·보안 분기를 확인했고 실제 생성/설치 결과는 아직 대기입니다. 결과 정상/HOLD 후 중지하며 Task/IPC/AI를 시작하지 않습니다. 상세 Discord_DedicatedR1_Install_20261027.md 참조.

## 1. 읽기 전용 관리자 사전검사

```powershell
& 'C:/Program Files/PowerShell/7/pwsh.exe' -NoLogo -NoProfile -Command '$b=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\prepare-r1.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne "c6a4b7b6dec6158b7191bdecf120d0c9efb3bc2b668e009aeba104975337671d"){throw "R1_HELPER_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b))) -Step Preflight'
```

출력 JSON의 HOLD를 먼저 판독합니다. 새 SID·토큰·실제 Batch 실행은 계정 생성 전 미검증이며 이 명령은 생성하지 않습니다. 이전151/Worker11 진단을 실행하지 않습니다.

## 2. Staging 전용 생성 1회 실행 완료 — 재실행 금지

```powershell
& 'C:/Program Files/PowerShell/7/pwsh.exe' -NoLogo -NoProfile -Command '$b=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\prepare-r1.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne "c6a4b7b6dec6158b7191bdecf120d0c9efb3bc2b668e009aeba104975337671d"){throw "R1_HELPER_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b))) -Step Stage -ApprovalScope "STAGE_R1_ONLY_NO_INSTALL"'
```

새 BA/SY staging만 생성합니다. 설치·계정·Task·승인 파일은 생성하지 않습니다. 부분 생성 시 HOLD·보존·재시도0입니다.

## 3. 설치 HOLD 결과 수신 — 재실행 금지, 아래 명령은 이력

```powershell
& 'C:/Program Files/PowerShell/7/pwsh.exe' -NoLogo -NoProfile -Command '$b=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\prepare-r1.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne "c6a4b7b6dec6158b7191bdecf120d0c9efb3bc2b668e009aeba104975337671d"){throw "R1_HELPER_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b))) -Step Install -ApprovalScope "INSTALL_R1_ONCE_TWO_NEW_ACCOUNTS_BATCH_WORKER_DISABLED_TASK"'
```

범위: 전용 보호 승인 root/15분 승인 JSON·소비/증거, 신규 계정2/전용 권리·Code/State/Auth·Disabled Task1. 신규 Worker만 ENABLED_BATCH_ONLY, Bot Disabled/Task Disabled·Trigger0·FAKE_ONLY·빈 auth. 로컬 SecureString 암호 두 번 입력 외 암호를 인수/파일로 넘기지 않습니다. 실행 후에도 IPC/Task/로그인을 시작하지 않습니다.

[전체 사전·사후 조건과 부분 실패 복구](Discord_DedicatedR1_InstallPreparation_20261027.md)를 먼저 확인합니다.

## 4. 만료 승인 보존·콘솔 직접 재개 — native HOLD 후 재실행 금지 / 이력

```powershell
$resumeBytes=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\resume-r1-console.ps1');if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($resumeBytes)).ToLowerInvariant() -cne 'cc6357c8e071ebad6109d51a6b8f9c6f3d08d9aa177b0b6e15c15d057ac589a4'){throw 'R1_RESUME_SHA_HOLD'};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($resumeBytes))) -Action Resume -ConfirmPreserveExpiredApprovalAndInstallOnce
```

관리자 실제 콘솔에서만 실행하며 입력/출력 리디렉션이면 승인 변경 전 HOLD합니다. 이전승인 SHA·만료/미소비·무계정/Task/경로·보호 stage를 검사하고 원본을 보호 Resume기록에 보존한 뒤 새15분 승인을 발급합니다. 암호는 설치기의 로컬 보안 입력만 사용하고 대화에 보내지 않습니다. 새 창을 띄우거나 기존 창을 닫는 exit는 없습니다. stdout를 수집하지 않는 Start-Process -NoNewWindow -Wait로 고정 설치기만 실행합니다.

설치기 JSON과 재개 envelope를 구분해 두 결과 JSON만 전달합니다. `CHILD_RETURNED_REVIEW_INSTALLER_JSON_NOT_SUCCESS_PROOF`/exit0 자체가 설치 성공은 아닙니다. 오류/HOLD/부분 상태면 재실행·삭제·승인 재발급 없이 중지합니다. 실제 Task/IPC/AI/로그인 시작은 하지 않습니다. [보존 절차·24개 새 fake 검증·남은 제한](Discord_DedicatedR1_Recovery_20261027.md) 참조.
