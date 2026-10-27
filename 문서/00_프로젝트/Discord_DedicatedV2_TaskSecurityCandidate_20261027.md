# r2 Task 권한 검증기 격리 후보 — 2026-10-27

상태 OFFLINE_CANDIDATE_PASS_NOT_WINDOWS_REVERIFIED / D3_SD_RIGHTS_HOLD 유지.
브랜치 codex/discord-usage-diagnostics-20261026, 기준970286453b93a7202f701361d0e93df4601d3890. 최신 Issue 기획6440747926 및 기존 관리자 JSON/원인 재현2PASS를 재사용했습니다.
신규 TaskSecurityCandidate 폴더만 구현했으며 고정 DedicatedContract.cs/r2 ZIP/Manifest/설치 Code/State/Auth/계정/Task/Stage/승인소비EntryResultLease는 수정하지 않았습니다. 기존 관리자 전체조회/설치/Stage 성공 검사를 반복하지 않았습니다.

## 1. GR/FR 원인과 동등성의 범위

GENERIC_READ(GR)=0x80000000은 객체별 권리 매핑 입력입니다. Microsoft 문서상 파일 객체의 읽기 매핑은 READ_CONTROL|SYNCHRONIZE|FILE_READ_DATA|FILE_READ_EA|FILE_READ_ATTRIBUTES입니다.
Task Scheduler는 task file의 읽기/쓰기/삭제/실행 권리로 Task 조회/수정/삭제/Run을 제어한다고 설명합니다. 따라서 이 Task의 알려진 파일 권리 프로필을 적용하면:
0x20000|0x100000|0x1|0x8|0x80 = 0x00120089 = FILE_GENERIC_READ(FR).
MapGenericMask API 역시 호출자가 제공하는 객체별 GENERIC_MAPPING을 적용하며, 객체 종류를 알아내거나 다른 추가 비트를 제거해 주지 않습니다.

기존 관리자 관측은 GR 요청 후 FR readback이며, 같은 SID/Owner/Group/Protected DACL을 유지했습니다. 고정 r2 ExactSddl은 미매핑 raw AccessMask 문자열을 비교하므로 이 표현 차이를 D3_SD_RIGHTS로 거부했습니다. 과거 throw 태그/시각 미확정이라는 기존 한계는 그대로입니다.
GR↔FR 읽기 권리의 의미는 위 Task-file 프로필 안에서 설명됩니다. 일반적인 모든 Windows 개체, 모든 Task 또는 Worker 토큰의 유효 권한 동등성을 이 관측으로 증명하지 않습니다. 현재 Windows 재조회/AccessCheck/실제 실행을 하지 않았으므로 실 Task 권한 검증 PASS도 아닙니다.

## 2. 격리 후보 구현

TaskReadSecurity.cs: Limitless.TaskSecurityCandidate.DedicatedTaskReadContract.
Task 전용 별도 계약이며 파일/계정/프로세스 등의 기존 ExactSddl을 수정하지 않습니다.
TaskPath \LIMITLESS-DedicatedV2-20261027-Host와 실제 Worker SID1010을 고정하고 objectKind SCHEDULED_TASK_FILE_RIGHTS만 받습니다.
Mapping Evidence는 임의 bool/사용자 지정 mapping 수치 대신 SHA로 고정한 관리자 JSON 바이트에서만 만듭니다. SHA4de232b3d5af7aa6662ce4e4ec6a4d28ddc529b5c0d50656a9114f71e679140e, 관리자 snapshot/정확TaskPath/Worker 실제SID/관측FR를 확인합니다. 과거 snapshot 바인딩이며 최신 상태 증명이나 서명된 OS 증거라는 의미는 아닙니다.

정규화는 Worker의 GR 비트만 TaskFileRead로 펼칩니다. 다른 비트는 삭제하지 않습니다. 결과는 정확히0x120089여야 하므로 FILE_WRITE_DATA/APPEND/WRITE_EA/WRITE_ATTRIBUTES/EXECUTE/DELETE_CHILD/DELETE/WRITE_DAC/WRITE_OWNER/ACCESS_SYSTEM_SECURITY/MAXIMUM_ALLOWED/GW/GE/GA 및 알 수 없는 비트가 섞이면 모두 HOLD입니다.
GR과 FR이 중복되거나 읽기 비트가 중복 표현돼도 정규화 결과가 같은 읽기 집합일 때만 통과합니다. FR 문자열 자체를 화이트리스트로 통과시키는 방식이 아닙니다. 읽기 비트가 하나라도 빠진 specific mask도 정책 불일치로 거부합니다.

Owner BA / Group SYSTEM / Protected DACL 필수.
DACL ACE 정확3개, 순서 SYSTEM→Administrators→Worker, allow CommonAce만, callback/object/deny/상속 flags/추가·중복SID·추가ACE 전부 거부.
SYSTEM/Administrators의 기존 FA=0x1f01ff를 그대로 비교하고 그 외 권리를 추가하지 않습니다.
관측 PAI의 AutoInherited 제어표시만 P와 함께 허용합니다. 이것이 ACE Inherited/ObjectInherit 등 상속 플래그 허용을 의미하지 않습니다. 그 외 controlflags/NULL·빈 DACL/SACL/OwnerGroup불일치/알수없는형식은 거부합니다.
SDDL을 다시 저장하거나 ACE 순서를 바꾸는 기능이 없으며 메모리 검증만 합니다.

## 3. 테스트 및 오류 기록

신규 오프라인48 PASS / 0 FAIL.
기대GR·관측FR·중복읽기 정규화, 고정실제snapshot 판독, 증거누락/변조·다른개체·Task·SID거부, 쓰기/삭제/실행/WRITE_DAC/WRITE_OWNER/unknown비트·읽기누락, 추가ACE/SID/순서/상속/callback외형·Owner/Group/Protected불일치·NULL/빈/오류SD 검증.
원인 재현 기존2PASS는 반복 실행하지 않았습니다. 실제 관리자조회/계정Task변경/설치/IPC/AI0.
기존 snapshot을 후보로 판독한 별도 드라이버 결과도 OFFLINE_CANDIDATE_PASS_NOT_WINDOWS_REVERIFIED이며 LiveVerified=false/RuntimeAdmitted=false입니다.
원설치 HOLD와 MethodInvocationException HRESULT -2146233087 → InvalidOperationException -2146233079를 결과 별도 필드에 보존합니다. 후보 오류는 HOLD와 Type/HRESULT/해당 Win32만 기록하고 민감한 예외 메시지는 출력하지 않습니다.

파일 SHA256:
- TaskReadSecurity.cs: 5cca69ee5991e6f20d58d3325c67eb102e94a9b92464884d0d3b615df696b103
- test-task-read-security.ps1: 0f6afa935b5ef3159ad5aaaa333c59f1202804e6b41e79a0fd0da5791501f2db
- check-pinned-snapshot-offline.ps1: 3a3c0ac34db1db10a87b63868a70901f50aa2c998659e04d47809a99cc859935
증거: 팀운영/Evidence/TaskSecurityCandidate20261027/OfflineTests.json, PinnedSnapshotVerdict.json, SourceSHA256.json.
고정 DedicatedContract/r2 패키지 및 현장 파일의 새 버전을 만들거나 덮어쓰지 않았습니다.

## 4. 적용 경로와 Task 재등록 없는 후속 검증 계획

지금 후보는 실제 installer/core/audit에 연결하지 않았습니다. 따라서 기존 설치 명령은 여전히 사용할 수 없습니다.
후속 승인 시 Task 보안 비교 call site만 격리된 Task 전용 계약으로 연결하는 방법을 검토합니다:
- DedicatedCore.ps1의 CREATE_ONLY_DISABLED_TASK 이후 Task SDDL 비교
- R2Install/install-r2-only.ps1 후감사의 Task 전용 ExactSddl 호출
- R2Readonly의 Task 보안 판독
파일 경로 ACL을 검사하는 DedicatedContract.ExactSddl은 기존 strict 정책을 유지해야 합니다. 설치된 core/host를 직접 패치하는 것은 별도 변경 승인 대상입니다.

실제 검증은 새 계정/Task를 만들 필요 없이, 별도 승인된 새 읽기 도구에서 후보·실행기 SHA와 실제 바인딩/관리자/SID/정확TaskPath를 확인하고 COM Connect→GetFolder→GetTask→GetSecurityDescriptor(7)로 현재 descriptor를 얻는 방식입니다.
기존 snapshot mapping 근거를 사용하되 현재 descriptor/Principal·Disabled·Trigger0/Action/SHA와 SID·코드바인딩을 함께 읽어 대조합니다. 접근거부·변경·불명은 HOLD, 원시HRESULT를 보존합니다. 현재 snapshot 전용 후보의 LiveVerified=false를 임의 true로 바꾸거나, 오래된 snapshot만으로 실제 PASS를 기록하면 안 됩니다.
수집 전후 Task XML/SDDL hash를 읽어 변경0 근거로 남길 수 있습니다. SetSecurityDescriptor/Register/Delete/Run 및 새 로그온은 필요하지 않습니다. 이번에는 관리자 명령을 실행하거나 관리자 실행을 요청하지 않습니다.
이후 승인된 read-only wrapper의 최신 판독 PASS와 후보 PASS를 별개 상태로 기록해야 합니다. 그 다음에도 유효토큰/GROUP 영향/Session0/Job 독점·승인소비/FAKE_ONLY IPC는 별도 승인·검증입니다.

## 5. HOLD 해제 조건과 보호

Task 보안 비교 HOLD만 해제하려면 후보의 정책 검토 및 SHA 고정, 승인된 현재 Task 읽기 재검증과 no-overgrant 확인이 필요합니다. 설치승인 소비 상태/기존부분자산을 초기화해서 해결하지 않습니다.
런타임 HOLD는 위 조건과 별개이며 실제 토큰/GROUP/Session0/Job/독점 미검증으로 유지합니다. V1 REFERENCE_VERIFICATION_HOLD와 MANUAL/A HOLD도 유지합니다.
계정1009/1010/1007, Task, V1/r1/r2 Stage, OnBoot2·11프로세스·인증·Unity/Save 변경0, Push/병합0. 오프라인 후보 완료 후 중지합니다.

공식 근거:
- https://learn.microsoft.com/en-us/windows/win32/secauthz/generic-access-rights
- https://learn.microsoft.com/en-us/windows/win32/taskschd/security-contexts-for-running-tasks
- https://learn.microsoft.com/en-us/windows/win32/api/securitybaseapi/nf-securitybaseapi-mapgenericmask
