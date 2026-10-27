# 전용 Worker r1 관리자 PowerShell 7 명령

현재 실행 허용 범위는 1번 읽기 전용 사전검사입니다. 2·3번은 각각 바울 대표의 별도 승인 후에만 실행합니다. 각 코드 블록은 한 줄이며 기존 PowerShell 창을 닫지 않습니다. 새 helper의 고정 SHA를 확인한 메모리 바이트를 새 NoProfile 프로세스에서 실행합니다.

## 1. 읽기 전용 관리자 사전검사

```powershell
& 'C:/Program Files/PowerShell/7/pwsh.exe' -NoLogo -NoProfile -Command '$b=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\prepare-r1.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne "c6a4b7b6dec6158b7191bdecf120d0c9efb3bc2b668e009aeba104975337671d"){throw "R1_HELPER_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b))) -Step Preflight'
```

출력 JSON의 HOLD를 먼저 판독합니다. 새 SID·토큰·실제 Batch 실행은 계정 생성 전 미검증이며 이 명령은 생성하지 않습니다. 이전151/Worker11 진단을 실행하지 않습니다.

## 2. 후속 staging 전용 승인 후 — 현재 실행 금지

```powershell
& 'C:/Program Files/PowerShell/7/pwsh.exe' -NoLogo -NoProfile -Command '$b=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\prepare-r1.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne "c6a4b7b6dec6158b7191bdecf120d0c9efb3bc2b668e009aeba104975337671d"){throw "R1_HELPER_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b))) -Step Stage -ApprovalScope "STAGE_R1_ONLY_NO_INSTALL"'
```

새 BA/SY staging만 생성합니다. 설치·계정·Task·승인 파일은 생성하지 않습니다. 부분 생성 시 HOLD·보존·재시도0입니다.

## 3. 후속 실제 설치 승인 후 — 현재 실행 금지

```powershell
& 'C:/Program Files/PowerShell/7/pwsh.exe' -NoLogo -NoProfile -Command '$b=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R1Preparation\prepare-r1.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne "c6a4b7b6dec6158b7191bdecf120d0c9efb3bc2b668e009aeba104975337671d"){throw "R1_HELPER_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b))) -Step Install -ApprovalScope "INSTALL_R1_ONCE_TWO_NEW_ACCOUNTS_BATCH_WORKER_DISABLED_TASK"'
```

범위: 전용 보호 승인 root/15분 승인 JSON·소비/증거, 신규 계정2/전용 권리·Code/State/Auth·Disabled Task1. 신규 Worker만 ENABLED_BATCH_ONLY, Bot Disabled/Task Disabled·Trigger0·FAKE_ONLY·빈 auth. 로컬 SecureString 암호 두 번 입력 외 암호를 인수/파일로 넘기지 않습니다. 실행 후에도 IPC/Task/로그인을 시작하지 않습니다.

[전체 사전·사후 조건과 부분 실패 복구](Discord_DedicatedR1_InstallPreparation_20261027.md)를 먼저 확인합니다.
