# 전용 Worker V2 r2 단발 설치 준비 — 2026-10-27

상태: PREPARATION_PASS / WAITING_ADMIN_INSTALL_RESULT. 실제 설치·암호 입력·계정/Task/승인 생성 0회. Codex가 설치를 실행하지 않았습니다.
브랜치 codex/discord-usage-diagnostics-20261026, 기준 26386d499665e3c713f1742ff7bdde04490b0dc6. LOCAL_COMMITTED / REMOTE_NOT_PUSHED(최종 커밋은 Issue 후속 보고).

## 고정 후보 및 실행기

Package ID 3effaac30bf44760986fee9e6019b283.
Staging C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\3effaac30bf44760986fee9e6019b283.
Installer SHA256 83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f
Manifest SHA256 7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976
ZIP SHA256 4efa0aa8a38b32fa40fa5a2dfb4042e028eeb07b1ab8d73c713a1d80efe028d0

새 install-r2-only.ps1 SHA256 d1fe5023184d6c89389f058ca0bc9a7cd3bbfe685827fa57ab3847fbeed3c05b
R2InstallControl.ps1 SHA256 e51dc17dab7ab8c07a50ba5d8b809c8752ac15dfbbb34d7f7e9ed9eb853792e5
test_r2_install.ps1 SHA256 4782bfd8af57204b16528e24b1601aa4bf609e4ff39b2a648424bd475e19dbc3

새 실행기는 고정 Installer 바이트를 검증하고 고정 r2 패키지의 Core/Windows native adapter를 직접 사용합니다. 기존 r1 Install/Resume 실행기 및 포괄적 단계 표기의 Installer wrapper는 실행하지 않습니다. consumeDirectory/Consume 이름 분리와 실제 adapter 객체 생성 경로를 검증했습니다. r2 승인 ID dedicated-install-3effaac30bf44760986fee9e6019b283, epoch 20261027-dedicated-3effaac30bf44760986fee9e6019b283만 허용합니다.

## 최소 검증과 실행 계약

신규 오프라인 20 PASS / 0 FAIL. 실제 실행기 Inspect에서 payload766 및 native ScriptMethod13 구성 PASS, 생성/암호 입력0. 기존151/Preflight/Stage 성공 검사는 반복하지 않았습니다.
가짜 Scheduler로 실제 패키지 Task 메서드를 실행하여 CREATE_ONLY flags50/등록 시 SDDL/Disabled/Trigger0/재시도0/90초/기존 Task 거부를 확인했습니다.
실제 coordinator 객체의 이름 충돌, r1 승인 거부, 빈 lease를 포함한 과거 증거 fingerprint, 단계별 오류 HOLD/정제 코드/단발 실행을 검증했습니다. 증거는 팀운영/Evidence/DedicatedV2R2Install20261027.

실제 Windows 사전조건 PASS는 아직 주장하지 않습니다. 대표 직접 실행 시 protected Stage의 Owner/Group/DACL/Handle/FileID/NTFS/ADS/reparse/hardlink/767파일+ZIP/바이트를 검증합니다. 기존 보호 승인 부모와 r1 Stage는 읽기만 하고 과거 기록 fingerprint를 보존합니다. 신규 이름·계정·Task·Code/State·승인/소비/journal 충돌, 기존 V1/Worker/OnBoot 보존 Snapshot을 확인합니다.
로컬 SecureString 암호2개 입력 후 생성 직전 재검증하고 유효15분의 r2 신규 승인만 CREATE_NEW합니다. native Core의 실행 직전 SHA/권한/승인 검증과 단발 소비·설치 lease를 유지합니다.
새 Windows 발급 SID2개, Bot Disabled, Worker 제한 Batch·Users-only, 독립 보호 Code/State/Auth, 빈 CODEX_HOME, Task1 Disabled/Trigger0/FAKE_ONLY/real_ai_enabled=false를 설치 후 재조회합니다. 기존 인증 복사·기존 Worker 중단·Stage 재생성은 없습니다.

## 대표 관리자 PowerShell 7 직접 실행 — 최대 1회

아래 명령은 새 NoProfile 자식 프로세스를 같은 콘솔에 연결합니다. 입력/출력을 redirect하거나 캡처하지 않습니다. 짧은 부트스트랩만 인코딩하며 비밀번호는 명령 인수가 아닙니다. PowerShell 창을 닫는 exit를 사용하지 않습니다.

`powershell
$r2InstallCmd='$r2InstallBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R2Install\install-r2-only.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r2InstallBytes)).ToLowerInvariant() -cne "d1fe5023184d6c89389f058ca0bc9a7cd3bbfe685827fa57ab3847fbeed3c05b"){throw "R2_INSTALL_TOOL_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r2InstallBytes))) -Action Install -ConfirmR2AccountsTaskInstallOnce';Start-Process -FilePath 'C:\Program Files\PowerShell\7\pwsh.exe' -ArgumentList @('-NoLogo','-NoProfile','-EncodedCommand',[Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($r2InstallCmd))) -NoNewWindow -Wait
`

성공 기대 status는 R2_INSTALLED_VERIFIED_DISABLED_NO_START입니다. native_result와 audit의 실제 SID/권리/경로 SDDL/Task/해시를 판독해야 설치 PASS로 보고합니다.
어떤 HOLD라도 즉시 중지하고 같은 명령을 다시 실행하지 않습니다. 승인 생성 시도 이후에는 보호 승인 부모의 Entry-3effaac30bf44760986fee9e6019b283.json 및 native 소비 경로 결과를 보존합니다. 승인 전 오류는 콘솔 정제 JSON만 남을 수 있습니다. 원시 오류 코드와 단계는 보존하되 비밀번호/토큰/명령줄 원문은 기록하지 않습니다. 부분 계정/SID/파일/Task는 자동 삭제·권한 완화·복구하지 않으며 별도 복구 검토가 필요합니다.

설치 결과 수령 전까지 실제 SID/상태/Task 보안은 미확인입니다. 설치 성공 후에도 새 유효 토큰·GROUP Task 영향·Session0 IPC·Job 독점/정리는 미검증 HOLD입니다. V1 REFERENCE_VERIFICATION_HOLD 및 MANUAL/A HOLD 유지. Task 시작·IPC/FakeWorker/AI·로그인·사용량·Discord·기존 운영/Unity/Save 변경·Push/병합 금지. 결과 판독 후 중지하고 IPC는 별도 승인입니다.
