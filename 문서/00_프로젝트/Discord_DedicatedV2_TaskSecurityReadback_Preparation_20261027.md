# r2 Task 실제 보안 읽기 재검증 준비 — 2026-10-27

상태 PREPARED_NOT_EXECUTED / SEPARATE_ADMIN_APPROVAL_REQUIRED.
브랜치 codex/discord-usage-diagnostics-20261026, 기준 7cbb7e4c0730d7e542db15cae7657e74d47f2384.
Issue #66 최신 후보 보고6440906147/6440910930 및 기존48PASS·후보SHA 재사용. 기존48시험/관리자45항목/설치/Stage 재실행0. 실제 관리자조회0.
후보 TaskReadSecurity.cs SHA 5cca69ee5991e6f20d58d3325c67eb102e94a9b92464884d0d3b615df696b103 불변.

## 최소 읽기 범위와 실행 전 보호 조건

새 TaskSecurityReadback 도구는 설치기/Stage/Resume/승인소비 함수를 호출하지 않습니다.
별도 승인을 받은 바울 대표의 관리자 PowerShell7 x64·새 NoProfile 프로세스에서만 실행하도록 보호합니다. 시작 시 명시 switch/관리자/버전/후보·core·과거근거SHA를 확인합니다. 암호 입력/새 로그온이 없습니다.
정확 Task 경로 \LIMITLESS-DedicatedV2-20261027-Host, Principal LIMITLESSWorkV2D→SID1010, PASSWORD1/RunLevel0/GroupId비움, Disabled/State1/Trigger0/Restart0/MultipleInstances2/HardTerminate/90초를 확인합니다.
COM Connect/GetFolder/GetTask/GetSecurityDescriptor(7)만 사용하며 수집한 SDDL을 기존 후보의 Task 한정 GR→FR 계약으로 비교합니다. GetTask 실패/권한거부/불명은 HOLD이고 ABSENT나PASS로 변환하지 않습니다.
Task Action은 기대 pwsh 경로·WorkingDirectory·인수 일치 bool만 기록합니다. 인수/XML/인증 원문은 출력하지 않습니다. XML은 과거 SHA4ef912d925510838b18765f6b18204c7da83de4b5d0b182381eaf36dde89a47b와 비교해 예상외 기존 변경을 거부합니다.

파일은 설치된 Manifest/Binding/Host3개만 SHA/Owner·Group·DACL을 기존 고정 관리자 근거와 대조합니다. reparse 조상 경로를 거부합니다. SHA는 각각7c841014…/d698ae90…/063c0632…로 고정됐습니다. Binding의 Package3eff…/Bot1009/Worker1010/Manifest SHA를 확인합니다.
Worker 계정1개만 현재SID/Enabled/만료 여부를 읽습니다. Batch권리/전그룹/이벤트/OnBoot/766payload/Stage/인증저장소/기존11프로세스 전체를 다시 조사하지 않습니다.
수집 전후 정확 Task2회와 파일3개/Worker 확인을 비교해 중간 변화가 있으면 HOLD합니다. 이는 실패 재시도가 아니라 정상 조회의 시작·종료 안정성 검사입니다. 최대 read2/자동재시도0. 시점별 관측이며 원자적 OS snapshot 보장은 아닙니다.

## 비인가 Worker 권한과 판정 한계

후보는 OwnerBA/GroupSYSTEM/ProtectedDACL·명시allow ACE3/순서 SYSTEM→BA→Worker·ACEflags0·추가SID/ACE거부 및 Admin/SYSTEM FA를 유지합니다.
Worker GR만 읽기specific0x120089로 펼치고 나머지 비트를 그대로 비교하므로 실행/쓰기/삭제/WRITE_DAC/WRITE_OWNER/unknown비트는 통과할 수 없습니다.
이것은 Worker 직접 ACE의 정책 검사입니다. 새 유효토큰·간접그룹·특권·다른 실행기가 주는 경로/권한까지 조사하지 않습니다. 따라서 비인가 실행/수정/삭제의 실제 유효 가능성이0이라고 단정하지 않습니다.
성공하더라도 wrapper 상태는 TASK_DESCRIPTOR_READONLY_PASS_EFFECTIVE_ACCESS_HOLD이고 descriptor_readback_status=PASS, effective_worker_access=UNKNOWN_TOKEN_NOT_CHECKED, runtime_admitted=false입니다.
후보 자체의 LiveVerified=false/RuntimeAdmitted=false는 수정하지 않습니다. wrapper의 현재 COM readback 판정과 과거 mapping 근거를 분리합니다. 설치 성공·권한복구완료·IPC실행 승인으로 확대하지 않습니다.

## 신규 오프라인 검증

신규22 PASS / 0 FAIL, 기존48 재실행false.
가짜 backend의 정상/보호조건별거부/소스·Task변화/overgrant/증거없음/접근거부HRESULT·비밀제거, 실제 adapter 객체생성·가짜Task GetTask/GetSD7·SID분기·Action불일치·stable fingerprint, 실제Prepare의 가짜파일/Worker, 금지변경메서드 부재/구문을 확인했습니다.
대표 명령과 같은 NoProfile EncodedCommand→ScriptBlock 방식으로 시험했고 실제 COM/관리자조회·OS변경0입니다.
증거 팀운영/Evidence/TaskSecurityReadback20261027/OfflineTests.json, Preparation.json, SourceSHA256.json.

새 파일 SHA256:
- probe-task-security-readonly.ps1: de9bf4639b1d1ae079802ade7d1e5f8bdc251af51a587bfdfa61f799eb95af41
- ReadbackCore.ps1: 3f29d411c2aa2453c02c5a9ef5c77aeb9b52b29c31c60bd3e09f627b5a0e0b0f
- test-readback-offline.ps1: fd5e3d279938bee03e647d965be6997c918e42e1777993e212a8b000dc421102

## 별도 승인 후에만 사용할 관리자 PowerShell7 한 줄

이번 준비 승인은 실제 조회 승인이 아닙니다. 지금 실행하지 않으며, 바울 대표의 다음 별도 승인 후 직접 한 번 실행합니다.
`powershell
$r2TaskCmd='$r2TaskBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\TaskSecurityReadback\probe-task-security-readonly.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r2TaskBytes)).ToLowerInvariant() -cne "de9bf4639b1d1ae079802ade7d1e5f8bdc251af51a587bfdfa61f799eb95af41"){throw "R2_TASK_READONLY_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r2TaskBytes))) -ConfirmApprovedReadOnly';Start-Process -FilePath 'C:\Program Files\PowerShell\7\pwsh.exe' -ArgumentList @('-NoLogo','-NoProfile','-EncodedCommand',[Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($r2TaskCmd))) -NoNewWindow -Wait
`

결과 파일:
F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\문서\00_프로젝트\팀운영\Evidence\TaskSecurityReadback20261027\Admin-<GUID>.json
기존 승인/소비/증거 파일을 덮어쓰지 않고 정제 결과1개만 CREATE_NEW합니다. 시작 시 도구/권한 검사가 실패하면 결과파일 전 생성 없이 고정 오류가 표시될 수 있습니다. 수집 중 오류는 stage·Type·HRESULT·해당Win32로 HOLD에 기록합니다. 창을 닫는 exit가 없습니다.
JSON 결과를 받은 후 실제 descriptor readback 판정과 원래 설치HOLD를 별개로 보고하고 중지합니다. 실패 시 반복/자동복구/새승인/계정TaskACL변경하지 않습니다.

## 남은 HOLD

실제 관리자 읽기 승인·수집·결과판독 대기. 현재Task 검증PASS 주장0.
이후에도 유효토큰/간접그룹/비협조 실행기/Job독점/Session0 IPC는 미검증 HOLD이며 별도 승인·검증 필요합니다. 설치승인소비는 초기화하지 않습니다.
V1 REFERENCE_VERIFICATION_HOLD 및 MANUAL/A HOLD 유지. 계정암호/ACL/Task등록시작/설치/AI/IPC/Discord/운영/Unity/Save 변경0. LOCAL_COMMITTED / REMOTE_NOT_PUSHED, 최종 커밋은 Issue 후속.
