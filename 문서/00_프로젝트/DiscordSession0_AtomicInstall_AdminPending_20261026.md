# Issue66 원자 SDDL 후보 실제 설치 승인 — 관리자 로컬 입력 대기

2026-10-26. 기준 `3f6ec5861622296100377277c908bc6915840d94`, branch `codex/discord-usage-diagnostics-20261026`.

**PACKAGE_PASS / WAIT_ADMIN_INSTALL / 설치 시도0**. 대표가 새 후보의 최소권한 계정 및 Disabled/trigger0/FAKE_ONLY 예약 작업 설치를 명시 승인했습니다. 추가 승인 질문은 필요하지 않습니다. 그러나 현재 Codex는 `PAUL-PC\windows` Session9 비관리자 토큰이므로 실제 설치와 로컬 암호 입력은 대표가 관리자 PowerShell7에서 직접 수행해야 합니다. Codex가 암호를 수집하거나 UAC/PIN/인증을 우회하지 않습니다.

## 확인 결과

- 승인 Installer SHA `117065bbfca686474f9104bc0dff20fa6528dfb9c204e5a2f2720d91994958e9` 일치.
- 승인 Manifest SHA `6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab` 일치.
- 승인 ZIP SHA `4b2c56edf5bb8edc6ee7a40a10504f537223bc77874b43a36e61be735c318f83` 일치.
- 정확855개 hash 일치, 원본 승인 Manifest9aa4…와 비교 시 설치기1만 변경/854 동일. 후보 및 원본 설치기/Manifest 수정0.
- 새 Program Files/ProgramData 설치 경로 없음. 계정/task 실제 부재·운영 OnBoot XML 전후 비교는 관리자 사전검사에서 확인하며 지금 추정하지 않습니다.
- 실제 설치/새 계정/task/ACL/Host/IPC/FakeWorker/모델/usage/Discord/운영 SQLite 작업0, 기존 부팅/봇/아트/인증/CODEX_HOME/Unity/게임/Save/Push/main/PC 변경0. MANUAL/A HOLD 유지.
- 기존29/35 성공시험 재실행0. 새 실행 보조 스크립트 Parser PASS, 읽기 전용 LSA reader C# 컴파일 PASS(LSA native 호출0), 파일 preflight PASS. 실제 관리자 분기는 아직 실행하지 않았습니다.
- 초기 새 보조 스크립트 preflight의 byte[] 반환이 PowerShell pipeline에서 object[]로 풀리는 오류는 단일 배열 반환으로 수정했습니다. 초기 HOLD 증거는 보존했고 설치 시도는 모든 preflight에서0입니다. 설치 등록 재시도에 해당하지 않습니다.

최종 사전검사 증거: `팀운영/Evidence/Session0AtomicInstall20261026/0bdcc00662cd4d07926f5230d6457018.json`.

## 실제 설치/검증 보조 스크립트

`Tools/DiscordAIServiceDiagnostics/run-atomic-trial-install.ps1` 신규, payload에는 추가하지 않았습니다. SHA256 `75ce33d60c34327b37d61cabd2fa362941933a280f919426c01eb7c9118bab70`.

지원 Action은 Preflight/Install/Inspect뿐이며 Host 시작/Enable/IPC/AI/Discord 기능이 없습니다. Install은 관리자 토큰을 확인한 뒤 다음을 수행합니다.

1. 새 후보의 세 승인 SHA 및855개 범위를 검사. 원본 Manifest도 기존 SHA를 대조한 뒤854 동일 확인.
2. 기존 Worker 실제 SID와 boot approval SHA 확인. 관리자 SID와 Worker SID 분리. 기존 OnBoot2개 XML을 읽어 SHA만 기록하며 원문/인수는 저장·표시하지 않음. 조회 실패는 암호 입력 전에 HOLD.
3. 새 설치 경로/계정/작업 부재 확인. 기존 task가 있으면 덮어쓰지 않고 HOLD.
4. hash 검증한 **승인 설치기 bytes**만 메모리 ScriptBlock으로 정확히 한 번 실행. 원본/후보 파일 수정 없음. 로컬 SecureString/Credential 입력은 승인 설치기가 수행. 래퍼가 암호/토큰/구독자료를 읽거나 저장하지 않음.
5. 성공 시 설치기의 read-only Verify를 재사용하여 실제 SDDL 및 코드·상태 ACL 확인. 신규 BOT/ADMIN/WORKER 실제 SID3개 분리, Bot 로컬 그룹 Users 하나, 직접 LSA권한 SeBatchLogonRight 하나, 24시간 계정 만료 검사.
6. 설치 payload855개 SHA·정확857개 총파일(payload855+HostManifest+DeploymentRecord) 확인. Task 고정 실행 경로/인수·작업계정·Password/Limited/Disabled/trigger0/restart0 확인.
7. 기존 OnBoot2개 XML SHA 및 boot approval SHA 전후 일치 검사. 불명/불일치면 HOLD. 등록 실패·부분 설치 시 가능한 한 새 task/SID/SD를 읽기만 하여 기록. 자동 재등록/자동 삭제/권한 수정 없음.

Session0/logon4는 이번에 **실행하지 않고 Password/Limited 예약 작업 설정**을 확인합니다. 실제 Session0 세션 관찰/FAKE_ONLY IPC PASS로 보고하지 않습니다. LSA 직접 권한 readback은 [공식 LsaEnumerateAccountRights](https://learn.microsoft.com/en-us/windows/win32/api/ntsecapi/nf-ntsecapi-lsaenumerateaccountrights)에 따라 POLICY_LOOKUP_NAMES handle만 사용합니다. 권한 부여/제거 API는 새 reader에 없습니다.

결과는 비밀정보 없는 JSON만 로컬 `팀운영/Evidence/Session0AtomicInstall20261026/<새 request_id>.json`에 기록하고 화면에 표시합니다. 이 작업 폴더 receipt는 결과 인계용이며 관리자 승인/실행권을 대신하지 않습니다. 오류 본문/스택 대신 type/HRESULT 및 고정 형식 오류코드만 기록합니다.

## 대표 직접 실행 명령 — 관리자 PowerShell7에서 한 번

아래는 외부 SHA로 검증한 새 보조 스크립트 bytes를 메모리에서 실행합니다. 파일 경로를 다시 실행하여 검증 뒤 바뀐 파일을 사용하는 방식은 쓰지 않습니다. 시작 메뉴에서 PowerShell7을 관리자 권한으로 실행한 뒤 전체 블록을 한 번 입력합니다.

```powershell
$trialPath = 'F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\run-atomic-trial-install.ps1'
$trialBytes = [IO.File]::ReadAllBytes($trialPath)
if ([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($trialBytes)).ToLower() -ne '75ce33d60c34327b37d61cabd2fa362941933a280f919426c01eb7c9118bab70') { throw 'RUNNER_SHA_HOLD' }
& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($trialBytes))) -Action Install
```

입력 요청은 새 LIMITLESSBotTrial Windows 암호와 기존 LIMITLESSRelaySvc Windows 암호입니다. 대표가 그 로컬 입력창에만 입력하고 채팅에 보내지 않습니다. Bot Token·Codex 인증파일은 입력/복사하지 않습니다.

출력이 `INSTALL_PASS_DISABLED_FAKE_ONLY_NO_EXECUTION`이면 설치 PASS입니다. HOLD/오류면 **같은 Install 명령을 다시 실행하지 않고** JSON 결과만 인계합니다. 등록 결과가 불명확하면 자동 삭제하지 않습니다. 현재는 관리자 실행 결과를 받지 못했으므로 신규 BOT SID·실제 작업 owner/DACL·부팅 보존 PASS는 미확정입니다.

## 되돌리기 및 다음 단계

현재 설치0이므로 운영 복구 작업은 없습니다. 향후 부분 설치이면 새 task 경로/description/fixed action/SID/SD 및 설치 record를 먼저 확인해야 합니다. 자기 신규 시험 자원임이 확인된 뒤에만 별도 승인 범위에서 stop/disable·프로세스트리0·단일 task 삭제·새 Bot 비활성화를 수행하고 증거는 보존합니다. 기존 계정/작업/ACL을 복구 대상으로 삼지 않습니다. DeploymentRecord가 없으면 기존 Remove를 무조건 실행하지 않습니다.

**이번 승인은 설치와 읽기 전용 검증까지입니다. 설치 PASS 즉시 중지하며 FAKE_ONLY 실전 IPC·모델/usage/Discord/A 모드 실행은 하지 않습니다.** 관리자 결과 확인 후 Issue66 및 담당 인계에 실제 SID·실제 SD·부팅 보존·최종 설치 상태를 이어서 기록합니다.
