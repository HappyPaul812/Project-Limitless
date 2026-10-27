# Issue66 Worker 확인 대기·native staging 구현·새 V2 후보

**최종 후보 r11 / 신규 오프라인 78 PASS·0 FAIL / NOT_APPROVED·NOT_INSTALLED.** Worker 실제 유휴는 HOLD, native staging 실측도 별도 승인 전 NOT_VERIFIED입니다. V1 REFERENCE_VERIFICATION_HOLD 및 MANUAL/A HOLD 유지. 실제 보호 staging 생성·파일 배치·Windows 계정/Task/ACL 변경·설치·IPC 실행0입니다.

branch `codex/discord-usage-diagnostics-20261026`, 지정 기준124be621ce1883b79f4659769bde74b6bdb153c3과 인계b6c44eb5486f8bd23bf6e681d8004da69c94907e에서 이어졌습니다. 최신 기획 Issue66 댓글6431791139 및 기존 관리자3a5803a2 JSON을 재사용했습니다. 기존 PASS6/855전수/19보호프로세스/기존 성공suite를 반복하지 않았습니다.

## Worker 단발 관리자 확인

원본 probe SHA `0fa7933f00ad42ab7fcd20dc2d07063d6b076aa72718841948aab57500d85307`, helper SHA `d6471b2b2427da3b94b9724c45e367408a1846d1ec230a4957c45c0e01265a18`을 먼저 확인했고 변경하지 않았습니다. 현재 Codex 토큰 admin=false이며 새 관리자 결과는 아직 없습니다. 기존 foreign_processes11/known=false만으로 현재 사용/유휴를 확정하지 않습니다. 실제 상태 **HOLD_WAIT_ADMIN_TARGETED_RESULT**.

새 관리자 PowerShell7에서 아래 **한 줄**만 실행합니다. Worker SID1007 계정/로그온 관계의 PID·생성시각, UserId/GroupId/LogonType 및 활성/실행중 작업 충돌, 기존 V2 lease 부재를 읽습니다. 이전 전체 preflight·인증·운영DB·보호프로세스 전수조회는 하지 않습니다. Worker/Task 종료·권한 변경·lease 획득·생성0입니다.

```powershell
$b=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\V2\probe-worker-hold2-readonly.ps1'); if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne '0fa7933f00ad42ab7fcd20dc2d07063d6b076aa72718841948aab57500d85307'){throw 'WORKER_PROBE_SHA_HOLD'}; & ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b)))
```

결과: `F:\study\codex\Project-Limitless\Temp\V2Hold2Worker20261027\<GUID>.json`. 권한거부/정보불명/프로세스 존재/활성 또는 실행중 Worker 작업/그룹 멤버십 불명/lease 존재는 HOLD. lease는 존재해도 잠금 여부를 시험하지 않고 PRESENT_NOT_PROBED로 보존합니다. 오류의 함수/정제된 자료형·길이/숫자 코드만 기록하며 비밀 원문을 기록하지 않습니다. exit/특권 활성화/실행policy 변경0입니다.

## native staging 생성기·검증기 구현

`V2Staging.cs`의 IV2StageFs를 통해 실제 Windows adapter와 가짜 파일 시스템이 **동일 Create/Verify 순서**를 실행합니다. V2NativeStageFs는 실제 P/Invoke 구현이며 선언만 있는 정책 모델에서 진전했습니다. `stage-v2-package.ps1`은 기본 InspectArchive, 별도 Verify, 명시 Create+ConfirmCreateProtectedStaging을 구분합니다. 이번에는 InspectArchive와 미승인 Create 차단만 실행했으며 native Create/Verify는 호출하지 않았습니다.

- 미래 staging: `C:\Program Files\LimitlessV2Staging20261027\13f746811f8a4b7da4c1bc3402c67f41\Bundle` 및 인접 `Bundle.zip`. 코드/상태 설치 root와 구분합니다. package 경로 기존 존재는 HOLD, 자동 재시도/삭제/덮어쓰기0. 공용 container가 이미 있다면 정확한 보호 owner/group/DACL인 경우만 부모로 재사용합니다.
- SECURITY_ATTRIBUTES로 CreateDirectory/CreateFile(CREATE_NEW) 시점부터 owner BA/group SY, protected DACL BA/SY만 FullControl. directory `O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)`, file `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`. Worker/Bot/user write 추가·사후 Set-Acl·SeDebug/SeRestore 활성화0.
- 읽기 handle에서 GetFinalPathNameByHandle, GetFileInformationByHandle의 volume/file ID·link수, GetSecurityInfo의 owner/group/DACL, GetFileInformationByHandleEx의 stream/삭제대기 정보를 확인합니다. NTFS 외/경로 불일치/reparse/hardlink>1/ADS/삭제대기/알 수 없는 ACE는 HOLD.
- 부모/조상을 고정 경로와 비교해 handle로 유지하고 실제 owner/비관리자 write·자식삭제·대체/WRITE_DAC/WRITE_OWNER를 검사합니다. 정규 BA/SY/TrustedInstaller 외 owner는 거부합니다. 직접 부모는 비관리자 write도 거부합니다. 검증된 package 부모를 먼저 잠근 뒤 ZIP을 읽습니다.
- source ZIP은 쓰기·삭제 공유를 허용하지 않는 handle로 읽고 동일 SHA의 고정 bytes만 복사합니다. 원본 후보/ZIP의 owner/ACL/위치/내용은 변경하지 않습니다. source의 reparse/다중 hardlink/ADS도 거부합니다.
- ZIP 크기128MiB/총 확장256MiB/각 file64MiB 상한, 정확한 Manifest+payload 집합과 모든 SHA, 중복/case alias/잘못된 경로/예약명/ADS/ZIP symlink·directory 속성/예상 밖 파일을 검사합니다. 일반 압축해제로 임시 링크를 만들지 않습니다.
- 새 directory/file 모두 보호 descriptor로 생성하고 handle 기반 owner/group/DACL/path/ID/links/ADS와 실제 읽은 bytes를 재검증합니다. 전체 file/directory 목록도 비교합니다. 설치 직전에도 재검증하며 handle과 고정 bytes를 설치 종료까지 유지합니다.
- 실제 경로 밖이나 `..`/ADS로 이탈하는 native 쓰기 인수는 거부합니다. 오류 stage/type/HRESULT/가용 Win32/close 실패 코드만 반환합니다. 부분 생성 가능성은 created=null과 creation_attempted로 구분하고 보존/HOLD하며 자동 복구하지 않습니다.

Windows SECURITY_ATTRIBUTES와 파일 생성 동작은 [공식 문서](https://learn.microsoft.com/en-us/windows/win32/fileio/creating-and-opening-files), hardlink 수는 [BY_HANDLE_FILE_INFORMATION](https://learn.microsoft.com/en-us/windows/win32/api/fileapi/ns-fileapi-by_handle_file_information), ADS 조회는 [GetFileInformationByHandleEx](https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-getfileinformationbyhandleex)에 근거합니다. 실제 Windows의 SDDL 정규화·handle 공유·부모 ACL·NTFS API 성공 여부는 **미실측**입니다. fake PASS가 이를 대신하지 않습니다.

## 새 패키지와 필수 8개 계약

정본 폴더 `F:\study\codex\Project-Limitless\Temp\Session0V2Candidate20261027-r11`, ZIP은 인접 `.zip`, sidecar `.package.json`. package_id `13f746811f8a4b7da4c1bc3402c67f41`, epoch `20261027-v2-13f746811f8a4b7da4c1bc3402c67f41`, approval ID `v2-install-13f746811f8a4b7da4c1bc3402c67f41`. schema2/FAKE_ONLY/approved=false/auto_start=false/90초 유지. ZIP **15,270,008 bytes**.

**payload769 + Manifest1 = ZIP770 entries.** r9의 기존766개를 모두 유지하면서 그중3개 수정,3개 추가입니다. 요구의 기존766개를 임의로 누락하지 않았으며 새 파일 수를766이라고 오기하지 않습니다. r10은 중간 검증 후보로 보존하되 superseded/미승인, 최종 검토는 r11만입니다. r9/기존원본은 덮어쓰지 않았습니다.

| 산출물 | SHA256 |
|---|---|
| 새 Installer | eb321fc5ee0426cdd7e1c8f989f05a5e2ec55e6d1887789f0e16490598311bc2 |
| 새 Manifest | e8d6ed623934a577f88a5bb99ebafc78a2a469c10485a0c648c40dd457356dc0 |
| 새 ZIP | 0250f736b44798d3ba73b5d9674fc154fa233bc74e3b972f85931c2aa59a41d0 |

Candidate Builder는 전용 진단/probe/모델/test를 배포에서 제외하고 staging+Worker 공통 조회를 포함합니다. Installer는 보호 staging Verify와 SHA 대조를 로컬 암호 입력보다 먼저 수행하고 handle을 유지합니다. 기존 V1 fresh-state/참조 HOLD 차단을 완화하지 않았습니다. Install Wrapper·C# PipeBoundary·Host PS·Client PS·Python Transport·Python Runtime은 그대로 포함되며 V2 namespace/schema2/실제 SID/새 epoch/승인 소비/Disabled·trigger0·FAKE_ONLY 계약을 유지합니다. Worker 조회는 그룹 principal을 지원하는 공통 코드로 연결했습니다. 설치 core가 별도 lease를 획득·유지하므로 설치 조회는 자기 lease 존재를 foreign busy로 오판하지 않게 process/task만 판정하며 lease 획득 보호는 제거하지 않았습니다.

r9 대비 정확한 payload 변경:

- 추가: Code/V2/V2Staging.cs, Code/V2/V2WorkerQuery.ps1, Deployment/stage-v2-package.ps1.
- 수정: Code/V2/InstallAssembly.cs, Code/V2/V2WindowsAdapter.ps1, Deployment/install-session0-v2.ps1.
- 삭제0, 그 외763개 payload 동일. Manifest는 새 GUID/epoch/파일SHA로 변경. Runtime741개와 Python 실행모듈은 동일해 기존 Runtime 성공 suite를 재실행하지 않았습니다.

원본 r9/새 후보를 혼용한 승인 ID/epoch는 실제 새 Manifest를 기준으로 합성 검사해 거부했습니다. 합성 approved=true 객체는 메모리에서만 사용했으며 실제 INSTALL_V2 승인 파일을 만들지 않았습니다.

## 신규 검증 결과와 근거

**78 PASS / 0 FAIL = staging60 + Worker연결11 + 새 패키지7.** staging은 같은 C# Create/Verify를 fake FS에서 실행했으며 원자 생성 descriptor, source 불변, owner/group/DACL/reparse/hardlink/ADS/extra/경로 이탈/부모 ID 변경/부분 쓰기 오류·권한거부·중복 생성·설치시 bytes 변조 등을 확인했습니다. Worker 연결은 실제 수정 함수와 공통 조회를 fake CIM/Scheduler로 결합하여 그룹/실행중작업/11개busy/권한거부·실제 오류코드·비밀 비출력을 확인했습니다.

패키지7개는 실제 새 ZIP/전체SHA/entry 집합/소스일치, stage InspectArchive, wrapper Inspect, 봉인 memory bootstrap, 미승인 Create 거부, r9 diff, 승인 혼용 거부입니다. actual native staging/계정/Task/IPC/Worker 호출0. 모든 PS parse오류0/exit노드0. native API 성공 검증은 포함하지 않았습니다.

증거 `문서/00_프로젝트/팀운영/Evidence/Session0V2R11_20261027/`: 전체 Candidate·HostManifest·파일별 SHA, SourceSHA256, FinalProof, StagingOfflineTests, WorkerIntegrationTests, PackageIntegrationTests. 이전 r10 근거는 superseded로 별도 보존합니다.

| 주요 변경 소스 | SHA256 |
|---|---|
| V2Staging.cs | acdb23a85554daf0811979d0c1b0fdf282e5e57149d08bc82a1cb5c0be04938f |
| stage-v2-package.ps1 | 534d556497867df6ff03d0378b4ee361ddc40957dfcab821b96c7d0217e61db9 |
| V2WindowsAdapter.ps1 | 50a13f91a987b65d14c0169b1bf34d10e96889c5321118d832687e60d22dca08 |
| build-v2-candidate.py | 12c27e97b9bb3274677a2b99d0f90c6c9d2ab49c4889c61839e4f461a8416ef8 |

## 별도 승인 범위와 남은 조건

실제 staging 생성 승인은 정확 r11 ZIP/Manifest/Installer 및 생성기 SHA, 위 고유 경로, BA/SY descriptor, 새 보호 staging bytes 배치와 검증에만 한정해야 합니다. 계정·Task·설치·IPC 승인을 포함하지 않습니다. 관리자 fresh -NoProfile PowerShell7을 사용하고 실패/부분 상태는 보존/HOLD/자동 재시도0. 실제 Create 명령은 이번에 실행하지 않았으며 별도 승인 전 실행을 요청하지 않습니다.

이후 Worker 관리자 단발 JSON 판독, 실제 부모/NTFS/owner·DACL·file ID·모든 SHA/link 검증과 보호 staging 생성 별도 승인, actual staging readback, 설치 직전 재검증, 정확 새 SHA의 단발 INSTALL_V2 승인이 필요합니다. 기존 r9 승인은 새 후보에 사용할 수 없습니다. 실제 설치 성공도 Disabled/Trigger0/FAKE_ONLY로 중지하며 FAKE_ONLY IPC는 다시 별도 승인입니다.

V1 계정/Code root/소비·참조 HOLD, Worker1007/인증/CODEX_HOME, OnBoot2/봇·아트, 운영SQLite·Unity·게임·Save 변경0. V1 설치855를 이번에 재검사하거나 안전한 삭제를 확정하지 않았습니다. 실제 보호 staging/계정/Task/ACL/LSA/IPC/FakeWorker/AI/usage/Discord/설치기/승인파일 생성/Push/main병합/재부팅0.

LOCAL_COMMITTED / REMOTE_NOT_PUSHED. Worker 결과는 HOLD로 남기고 새 후보 준비 범위에서 중지합니다.
