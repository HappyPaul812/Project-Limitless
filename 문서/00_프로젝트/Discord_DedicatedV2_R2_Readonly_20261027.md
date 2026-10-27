# r2 부분 설치 — 관리자 읽기 전용 진단 준비 (2026-10-27)

상태 READONLY_TOOL_READY / WAITING_ADMIN_RESULT / PARTIAL_INSTALL_HOLD.
브랜치 codex/discord-usage-diagnostics-20261026, 기준 f6b616213c6a77f1ba726d4e27a9b516cfdac97d. 기존 패키지/Stage/Install/Resume 및 성공 검사를 실행하지 않았습니다.
Issue #66 최신 기획 판독 6439920038 및 6439787364/6439796482 결과를 재사용했습니다. Entry/Result 보호 디렉터리는 Codex 비관리자 조회가 접근 거부되어 원본은 읽지 못했습니다. 이를 원본 확인 완료로 표시하지 않습니다.

## 현재 확정과 미확정

- 대표 제공 설치 JSON: 승인 생성/소비true, 설치1회, Bot1009·Worker1010 생성, Task 시도1회, CREATE_ONLY_DISABLED_TASK에서 HOLD.
- 직접 원인 UNKNOWN. 해당 단계는 등록 COM 호출, Principal 변환, 실제 SDDL 조회, ExactSddl, readback을 포함합니다. MethodInvocationException/InvalidOperationException HRESULT만으로 암호 오류 또는 등록 실패라고 확정할 수 없습니다.
- Task PRESENT/ABSENT/UNKNOWN 중 현재 UNKNOWN. 권한 거부는 NOT_FOUND가 아닙니다.
- 새 계정 최종 활성/그룹/권리·배포 Code/State/Auth·Task 보안/Disabled/Trigger0·전체 SHA는 관리자 결과 대기입니다.
- existing_preserved=true는 설치기 보고입니다. 현재 독립 전체 OS 감사 PASS가 아닙니다.
- 설치·소비 완료 후 같은 명령 재시도/새 승인/소비 초기화/계정·Task·ACL 변경은 금지합니다.

## 전용 도구와 범위

Tools/DiscordAIServiceDiagnostics/R2Readonly/probe-r2-partial.ps1. 기본 SelfTest, 실제 조회는 Collect -ConfirmReadOnly 및 관리자 PS7 x64에서만 가능합니다. 설치기/API factory/새 계정/Task 등록 코드는 호출하지 않습니다.
별도 결과 JSON 한 개만 CreateNew로 생성합니다. 기존 자산은 읽기만 하며 비밀번호 입력0, OS 설정변경0, 설치/IPC/AI/재시도0입니다.

Task COM Connect/GetFolder/GetTask를 단계별 기록하고 GetTasks(1)의 숨김 포함 목록에서 정확한 경로만 출력합니다. GetTask의 정확한 0x80070002만 ABSENT로 분류하며 접근거부/timeout/그 외 실패는 UNKNOWN입니다. 목록에 일치 작업이 있으면 PRESENT입니다. 보안 descriptor7에서 Owner/Group/보호DACL과 frozen ExactSddl의 header/ACE 조건을 구분 비교합니다. 현재 보안 불일치가 관측되더라도 과거 예외의 직접 원인으로 자동 확정하지 않습니다.
Principal/LogonType/Enabled/Triggers/상태/재시도/90초/Action 경로·인수 비교값·호스트 코드 SHA 및 Task XML 해시만 기록합니다. 인수 원문/XML/이벤트 메시지는 저장하지 않습니다. PowerShell 실행파일 SHA는 관측값이며 패키지에 기준 SHA가 없으므로 검증 PASS라고 부르지 않습니다.
RegistrationInfo.Date는 선언값이고 실제 등록 시각 증명이 아닙니다. 정확한 작업의 TaskScheduler/Operational 이벤트(2026-10-27 한국시간 시작 이후, 최대128개)에서 ID/record/time/숫자 오류값만 수집합니다. 로그가 꺼져 있거나 이벤트가 없어도 작업 부재를 증명하지 않습니다. 로그 설정은 변경하지 않습니다.

신규 두 계정은 Get-LocalUser와 독립 NetUserGetInfo4로 조회합니다. SID/Enabled/flags/만료/로그온 시간·워크스테이션 제한 여부만 기록하고 암호 포인터는 역참조하지 않습니다. NetUserGetLocalGroups(간접 포함)→SID, LsaOpenPolicy(POLICY_LOOKUP_NAMES 0x800)/LsaEnumerateAccountRights로 직접 권리를 읽습니다. LSA 원시 NTSTATUS→Win32 및 예외 체인 Type/HRESULT를 보존합니다. 새 로그온을 만들지 않으므로 실제 유효 토큰/GROUP 실행 영향은 계속 UNKNOWN입니다.
고정 r2 ZIP/Manifest에서 기대값을 읽고 새 설치 payload766 및 Manifest/Binding, 실제 파일 Owner/Group/DACL/ID/링크수/ADS/예상외 파일 수를 조회합니다. 이는 기존 성공한 Stage 전체 재검사가 아닌 새 부분 설치 대상 감사입니다. reparse 조상을 거부합니다. 조회는 시점별 읽기 snapshot이며 설치용 원자적 무결성 보증으로 확대하지 않습니다.
승인/소비/Entry/Result는 허용된 상태·오류 필드만 정제합니다. 승인 부모의 과거 기록은 내용 대신 metadata/hash만 조회하고 변경하지 않습니다. r1/r2 Stage 루트·V1 루트 metadata, 운영 Worker/V1 SID상태와 OnBoot2 XML 기준 SHA를 대조합니다. 기존855파일 전수 성공 검사는 반복하지 않으며 과거 기준이 없는 메타데이터만으로 전체 보존을 확정하지 않습니다.
Lease 파일 및 Control의 기록 목록만 읽습니다. 커널 Job 객체나 IPC 독점은 조사하지 않으므로 미검증 HOLD입니다.

## 오프라인 검증

신규 17 PASS / 0 FAIL: 접근거부/정확NOT_FOUND/timeout/불명 HRESULT 구분, 내부 Win32 보존·본문 제거, 기록 중첩 비밀 제거, 정확 Task 이벤트 필터/DTD 거부, 실제 보안 조회함수의 가짜 Task exact/header mismatch/access-denied, native SID·계정 범위 거부, 읽기 전용 native import allowlist 및 OPEN_EXISTING 읽기속성 접근 검증.
C# 컴파일/스크립트 구문/최종 명령 구문 PASS. 실제 관리자 Collect/OS 계정조회/Task조회는 시험에서0. 증거 DedicatedV2R2Readonly20261027/OfflineTests.json 및 Preparation.json.

## 바울 대표 관리자 PowerShell 7 한 줄
도구 SHA256: d3e9b499491dae3b6087878c1b40c2b8f4d6b3d96c1b0d8c4847377d621fe407

`powershell
$r2ProbeCmd='$r2ProbeBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R2Readonly\probe-r2-partial.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r2ProbeBytes)).ToLowerInvariant() -cne "d3e9b499491dae3b6087878c1b40c2b8f4d6b3d96c1b0d8c4847377d621fe407"){throw "R2_READONLY_TOOL_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r2ProbeBytes))) -Mode Collect -ConfirmReadOnly';Start-Process -FilePath 'C:\Program Files\PowerShell\7\pwsh.exe' -ArgumentList @('-NoLogo','-NoProfile','-EncodedCommand',[Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($r2ProbeCmd))) -NoNewWindow -Wait
`

실행은 위 진단 명령만 사용합니다. 암호 입력이 없습니다. 새 NoProfile 자식이 같은 콘솔에서 결과 요약 JSON을 출력하고 부모 창은 닫지 않습니다.
결과 경로: F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\문서\00_프로젝트\팀운영\Evidence\DedicatedV2R2Readonly20261027\Admin-<GUID>.json.
전체 파일은 로컬에서 정제되어 저장됩니다. 요약과 해당 JSON으로 실제 Task/계정/부분자산/오류 근거를 판독합니다. 현재는 관리자 실행 전이라 원인 확정/권한조건 수정/설치 PASS 보고가 불가능합니다.

## 최소 복구 계획과 중지 조건

1. 대표 직접 읽기 수집 후 Entry/Result 시각·현재 Task·이벤트·SD 조건을 비교합니다.
2. Task가 PRESENT라도 Disabled/Trigger0/Principal/Owner/DACL/Action/SHA를 확인하기 전 사용 금지입니다.
3. Task가 ABSENT여도 소비된 승인으로 Install을 다시 실행하지 않습니다.
4. 직접 실패 원인과 부분 자산을 확정한 뒤 정확한 코드/보안 조건 수정안을 별도 제시합니다. 현재 특정 ACL 완화나 계정 삭제를 권고하지 않습니다.
5. 별도 복구 승인 전 기존 증거/계정/파일/Task/소비 기록 그대로 보존합니다. 실제 토큰/GROUP/Session0 IPC/Job 및 V1/MANUAL/A HOLD 유지. 설치·복구·AI·Discord·운영/Unity/Save 변경·Push0. 진단 결과 판독 후 중지합니다.

공식 참고:
- https://learn.microsoft.com/en-us/windows/win32/api/taskschd/nf-taskschd-iregisteredtask-getsecuritydescriptor
- https://learn.microsoft.com/en-us/windows/win32/api/taskschd/nf-taskschd-itaskfolder-gettasks
- https://learn.microsoft.com/en-us/windows/win32/api/lmaccess/ns-lmaccess-user_info_4
- https://learn.microsoft.com/en-us/windows/win32/api/ntsecapi/nf-ntsecapi-lsaenumerateaccountrights
