# Issue66 V2 HOLD 2건 수정·검증 계획 — 2026-10-27

**신규 오프라인 46 PASS / 0 FAIL. 실제 Worker 유휴 HOLD / 실제 staging HOLD.** V1 REFERENCE_VERIFICATION_HOLD, MANUAL/A HOLD 유지. 실제 staging 생성·설치·승인 파일 생성0.

브랜치 codex/discord-usage-diagnostics-20261026. 기준 구현62578e5174d208dd4e83ebe23c2ac438303eb478, 직전 판독362bca3b1ff5c176797dacf5fc804ed94d9a5930. 기존 관리자 JSON 3a5803a201ca4d1aa055ceeca114aaba, SHA fc7085b0412a0ac3e120500b0c9d1038a53b4493c3ded6c738400eccc0614fd3를 재사용했습니다. 기존 PASS6/855전수/19보호프로세스/기존 성공suite는 반복하지 않았습니다.

## Worker 오류 원인과 한계

실패 가능 코드 경로: Get-V2WorkerSnapshot → Visit-V2Tasks → NTAccount.ctor(UserId) → Translate(SecurityIdentifier). 기존 코드는 UserId의 자료형·빈 문자열·GroupId를 확인하지 않았습니다. NTAccount 생성자에 System.String/Length0을 넣으면 MethodInvocationException → ArgumentException, HRESULT -2147024809가 기존 JSON과 동일하게 재현됩니다. 그룹 principal을 사용자 이름으로 처리하는 것은 코드 결함입니다. [공식 NTAccount 계약](https://learn.microsoft.com/en-us/dotnet/api/system.security.principal.ntaccount.-ctor), [GroupId 계약](https://learn.microsoft.com/en-us/windows/win32/taskschd/principal-groupid).

다만 원본에는 당시 Task/인수/함수 위치가 없습니다. **실제 당시 UserId가 빈 문자열이었다고 확정할 수 없습니다.** 동일 오류 합성 재현과 코드 결함 확인은 완료했으나 실제 발생 인수는 UNKNOWN입니다. 현재 비관리자 COM 연결은 UnauthorizedAccessException/HRESULT -2147024891로 거부됐습니다. 접근 제한 우회0.

최소 수정은 원래 함수에서 잘못된 UserId를 변환 전에 HOLD, ctor(string)/Translate(Type) 단계와 자료형/길이/LogonType 기록, script: counter를 공유 객체로 변경한 것입니다. 그룹 principal을 원래 설치 함수에서 임의로 제외하거나 안전 판정하지 않고 HOLD합니다. 계정·LSA·등록·권한 정책은 변경하지 않았습니다.

별도 V2WorkerQuery.ps1은 UserId/GroupId/LogonType4를 구분합니다. 그룹 멤버십 불명·중첩/도메인 그룹·둘다없음/둘다있음·잘못된 자료형은 HOLD, Worker가 속한 활성 그룹 작업도 충돌입니다. 실패 함수·정제된 인수 shape·숫자 코드만 저장하고 원시 principal/명령줄/환경/비밀/Message/Stack은 기록하지 않습니다.

## 유휴 상태 및 lease

원본 Worker SID는 S-1-5-21-2858504392-1958168942-1662906075-1007입니다. batch/groups는 기존 PASS 재사용. foreign_processes11/known=false는 조회 중 집계일 뿐 현재 정확11개나 유휴/타작업0 확정이 아닙니다. 원본에 없는 PID·생성시각은 복원하지 않습니다.

새 진단은 해당 SID 한 건 → 로그온 관계 → 프로세스 PID/생성시각만 읽습니다. 생성시각 누락·PID 재사용은 HOLD. process_query_known/task_query_known/lease_query_known을 분리합니다. Worker user/소속 group의 Enabled 또는 실행중(State4) 작업은 충돌입니다. Task 실패와 별개로 lease 메타데이터를 조회합니다.

V2 lease C:\ProgramData\LimitlessRelayV2Approvals\Worker-<SID1007>.lease는 부모 목록으로 부재를 확인합니다. 열기/잠금 획득/생성0. 존재하면 PRESENT_NOT_PROBED/HOLD, 접근거부·불명·reparse도 HOLD. 이 부재만으로 기존 SQLite 소비·다른 시스템 lease까지 배제하지 않습니다. 운영 DB/인증은 열지 않습니다. 비협조적인 기존 launcher는 V2 lease로 막히지 않으므로 실제 설치 직전 유휴·동시실행 재검증이 필요합니다.

## 최소 관리자 읽기 전용 명령

새 관리자 PowerShell7에서 아래 한 줄만 실행합니다. Worker HOLD 전용이며 이전 PASS6이나 staging/설치/승인 생성은 실행하지 않습니다.

```powershell
$b=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\V2\probe-worker-hold2-readonly.ps1'); if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($b)).ToLowerInvariant() -cne '0fa7933f00ad42ab7fcd20dc2d07063d6b076aa72718841948aab57500d85307'){throw 'WORKER_PROBE_SHA_HOLD'}; & ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($b)))
```

Helper도 고정 SHA로 memory load. 결과 F:\study\codex\Project-Limitless\Temp\V2Hold2Worker20261027\<GUID>.json에 CREATE_NEW. exit/실행policy변경/특권 활성화0. Codex 실행은 비관리자 gate HOLD 확인뿐이며 cdb2ef919b73411a9e16a043d6696153.json을 보존했습니다. 관리자 Worker 진단은 아직 미실행입니다.

## 관리자 보호 staging 설계 — 생성 미승인

신규 경로: C:\Program Files\LimitlessV2Staging20261027\R9-01f8a36c9bfe4241b24b7eaa6caaddc2. 그 아래 Bundle\(767개 file)과 인접 Bundle.zip을 배치하는 안입니다. root/package 경로가 이미 있으면 HOLD, 재사용·덮어쓰기0. F: 원본 후보/ZIP의 ACL·위치·bytes는 변경하지 않습니다.

쓰기 주체 BA/SY만, owner BA/group SY. Directory SDDL O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA), file SDDL O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA). Worker/Bot/user 접근을 추가하지 않습니다. SECURITY_ATTRIBUTES의 descriptor로 CreateDirectory/CreateFile(CREATE_NEW) 시점부터 제한하며 사후 Set-Acl은 사용하지 않습니다. [공식 생성시 descriptor 동작](https://learn.microsoft.com/en-us/windows/win32/fileio/creating-and-opening-files).

별도 승인 후 native 구현·검증 순서:

1. 로컬 NTFS·고정경로 확인. 부모/조상을 reparse를 따라가지 않는 handle로 열어 실제 경로·volume/file ID·owner를 pin합니다. owner BA/SY/정규 TrustedInstaller만 허용하고, 비관리자 자식삭제·대체·WRITE_DAC·WRITE_OWNER를 거부합니다. 직접 부모의 비관리자 write도 거부합니다. rename/delete 경쟁을 막는 handle 유지 및 재조회가 필요합니다. 부모 부적합 시 기존 ACL을 수정하지 않고 HOLD. 실제 부모 검사는 이번에 하지 않았습니다.
2. 원본 ZIP을 read-only/share-write·delete 불허 handle로 열어 reparse/ADS/다중hardlink를 거부하고 고정bytes로 읽습니다. 정확 ZIP SHA만 복사하며 원본에 쓰지 않습니다. Copy-Item/Expand-Archive로 보호 전 임시파일을 만드는 절차는 사용하지 않습니다.
3. root/package/Bundle/모든 하위 directory와 file은 CREATE_NEW+보호descriptor로 생성합니다. ZIP은 payload766+Manifest1의 exact entry만 허용. case중복·directory entry·symlink속성·절대경로·..·ADS·예약명·말미dot/space·예상밖파일·과도한 크기는 거부합니다. 링크를 생성하는 압축해제 동작은 없습니다.
4. 배치 후 ZIP전체 SHA, Manifest SHA, Installer SHA, payload766개 각각의 SHA 및 767entry/767file의 정확집합과 bytes를 비교합니다. 기존 성공 증거는 이번에 재사용했으며 실제 배치 시 다시 증명해야 합니다.
5. 모든 file/directory/ZIP의 실제 owner/보호DACL/ACE/mask를 handle에서 재조회. 일반file link수1·ADS0·reparse0·여분0과 조상/file ID도 확인합니다. [FILE_STANDARD_INFO.NumberOfLinks](https://learn.microsoft.com/en-us/windows/win32/api/winbase/ns-winbase-file_standard_info)를 사용할 계획입니다. 실패하면 신규 부분 staging 보존/HOLD, 자동삭제·ACL수정·원본복원쓰기0.
6. 설치 시작 시 위 검사와 SHA를 다시 확인하고 검증된 memory bytes만 사용합니다. staging PASS와 INSTALL_V2 승인은 별개입니다.

유지하는 r9 ZIP SHA a4a652bdf73c9a1007bd1e761ae539d3459896b46d8a021fa1459012a1d15c41, Manifest 250bab2d46010841353a319fbcf34581784366ac3042e6bc0fdc97aadd17c09e, Installer 1a1d671d1cf86aab56c9c2392ed4ba35a8240e348134e6a467f8eb8ec81f17a0.

**현재 구현은 합성 proof 거부 모델뿐입니다. native staging 생성기와 handle 검증기는 미구현·미실측입니다.** bool 조건의 합성 PASS를 Windows 보호 증명으로 사용하지 않습니다. native 구현/검증 이후 실제 생성·배치는 바울 대표의 별도 승인이 필요합니다.

## 오프라인 검증과 변경 SHA

신규46 PASS/0 FAIL: Worker19, 원래 함수 fake CIM/Scheduler 메모리 회귀2, staging 모델25. native 부작용0. parse0, 진단/모델 exit0·금지변경cmdlet0. 증거 문서/00_프로젝트/팀운영/Evidence/V2Hold2Resolution20261027/에 OfflineTests/SourceSHA256/SourceAndStaticChecks JSON 보존. 기존 성공suite 반복0.

| 변경 코드 | SHA256 |
|---|---|
| V2WindowsAdapter.ps1 | f161ee6b91659a53adf5cb1063fdd3d2bbcb14bcb0515fa201ebeb51bb5ae527 |
| V2WorkerQuery.ps1 | d6471b2b2427da3b94b9724c45e367408a1846d1ec230a4957c45c0e01265a18 |
| V2StagingPlan.ps1 | 9fdadb938ffffe1e8432850761219f1a0e0644b175027ebb4f85ee65346d68ca |
| probe-worker-hold2-readonly.ps1 | 0fa7933f00ad42ab7fcd20dc2d07063d6b076aa72718841948aab57500d85307 |
| test_hold2_offline.ps1 | 9ef8bd64277e0e88a6c2d89e1e1ee2582504230f468c3d963c3d171ad638b599 |

## 실제 설치 전 남은 조건

Worker 실제 유휴 HOLD(관리자targeted 결과 대기), staging 실제 보호 HOLD(설계/합성검증만 완료). 수정 source adapter는 고정 r9 ZIP에 반영하지 않았습니다. r9를 수정본이라고 주장하거나 고정SHA로 내용을 바꿀 수 없습니다. 원래 r9 설치조회가 다시 HOLD할 수 있으므로, 실제 설치 전 수정소스 별도패키지/SHA/승인 결정이 필요합니다. staging만으로 설치기 코드 결함이 해소되지는 않습니다.

실제 유휴·동시실행 방지·native staging 안전검증·생성배치 별도승인·설치시 재검증·명시 단발INSTALL_V2 승인이 남았습니다. 실제 설치 성공도 Disabled/trigger0/FAKE_ONLY로 중지, IPC는 별도 승인입니다. V1·Worker1007·인증/CODEX_HOME·OnBoot2·기존봇/아트·운영SQLite·Unity·Save 변경0. 계정/Task/LSA/IPC/FakeWorker/AI/usage/Discord/설치기/승인파일/Push/main병합0.

LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 원인 수정·검증 계획에서 중지합니다.
마감: 구현/증거 커밋124be621ce1883b79f4659769bde74b6bdb153c3. 실제 Issue66 보고 https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6431728309 . LOCAL_COMMITTED / REMOTE_NOT_PUSHED, 실제Worker/staging HOLD·실제생성/설치0.
