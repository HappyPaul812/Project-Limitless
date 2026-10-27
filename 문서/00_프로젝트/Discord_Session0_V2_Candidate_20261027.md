# Issue66 V2 독립 시험 후보 구현·패키지·오프라인 결합 검증 — 2026-10-27

**V2_CANDIDATE_OFFLINE_PASS / NOT_APPROVED / NOT_INSTALLED.** V1 REFERENCE_VERIFICATION_HOLD, MANUAL/A HOLD는 그대로입니다. 이번 승인 범위인 코드·패키지·오프라인 결합 검증을 완료하고 중지합니다.

branch `codex/discord-usage-diagnostics-20261026`, 사용자 지정 코드 기준 `63b97539ae4e1655d9f36e905b1cb2e3601b22d7` 위 기존 문서 마감 `1c01c58ac5d29d6be517b1a5867c15958c709cbe`에서 이어갔습니다. V1 guard/설치기/cleanup/원본 게임 변경0.

## 1. 최종 실물 후보와 SHA

**검토 정본은 r9만입니다.** 중간 r1~r8은 덮어쓰거나 삭제하지 않고 미승인·superseded 상태로 남겼습니다. 어느 후보도 Windows에 설치하지 않았습니다.

- 폴더: `F:\study\codex\Project-Limitless\Temp\Session0V2Candidate20261027-r9`
- ZIP: `F:\study\codex\Project-Limitless\Temp\Session0V2Candidate20261027-r9.zip`
- ZIP 크기 **15,254,406 bytes** (최종 FinalProof의 실제 stat 값이 우선합니다).
- payload **766개**, HostManifest 포함 ZIP **767 entries**. ZIP CRC·entry집합·모든 bytes SHA 일치.
- package ID **01f8a36c9bfe4241b24b7eaa6caaddc2**, epoch **20261027-v2-01f8a36c9bfe4241b24b7eaa6caaddc2**.
- 별도 설치 승인 ID **v2-install-01f8a36c9bfe4241b24b7eaa6caaddc2**.
- `approved=false / auto_start=false / execution_mode=FAKE_ONLY / schema2` 유지. 후보 Manifest에는 임의 bot SID가 없습니다. 실제 OS 생성 SID는 향후 설치 시 InstalledBinding.json에만 바인딩합니다.

| 실제 산출물 | SHA256 |
|---|---|
| Deployment/install-session0-v2.ps1 | `1a1d671d1cf86aab56c9c2392ed4ba35a8240e348134e6a467f8eb8ec81f17a0` |
| HostManifest.json | `250bab2d46010841353a319fbcf34581784366ac3042e6bc0fdc97aadd17c09e` |
| 배포 ZIP | `a4a652bdf73c9a1007bd1e761ae539d3459896b46d8a021fa1459012a1d15c41` |

전체 [Manifest](팀운영/Evidence/Session0V2Candidate20261027/HostManifest.json), [파일별 SHA·V1/V2 비교·실물 경로](팀운영/Evidence/Session0V2Candidate20261027/Candidate.json), [소스 SHA](팀운영/Evidence/Session0V2Candidate20261027/SourceSHA256.json), [최종 증거](팀운영/Evidence/Session0V2Candidate20261027/FinalProof.json). ZIP 실물은 로컬 Temp에 보존하며 GitHub 업로드/Push0입니다.

## 2. 필수 8개 구성요소 구현

모든 새 소스는 `Tools/DiscordAIServiceDiagnostics/V2/` 아래이며 V1 코드는 변경하지 않았습니다.

| 구성요소 | 구현 파일 | 실제 연결 |
|---|---|---|
| Candidate Builder | build-v2-candidate.py | 프로젝트 Temp의 새 지정이름만 생성, V1 공개 후보 Runtime SHA 대조 복사, 실제 파일목록·unique GUID/epoch/schema2/ZIP 산출 |
| Installer | install-session0-v2.ps1 + V2InstallCore.ps1 + V2WindowsAdapter.ps1 | 검증된 메모리 bytes만 사용, 가짜/실제 adapter가 같은 설치 순서·거부 조건을 공유 |
| Install Wrapper | run-session0-v2-install.ps1 | 기본 Inspect, 실제 Install은 별도 보호 승인과 정확 SHA, 파일/ZIP bytes 봉인 후 memory ScriptBlock 호출 |
| C# PipeBoundary | Session0PipeBoundary.cs | V2 exact Code/State root·pipe 계약, 실제 Windows peer SID/Session/권한, 로컬/first-instance pipe 및 bounded frame |
| Host 시작 PS | start-host-v2.ps1 + V2Host.cs | 컴파일 전 소유자·쓰기권한·SHA bootstrap, batch logon4/Session0/제한토큰·Task·별도 IPC 승인·lease 검증 |
| Client PS | session0-client-v2.ps1 | 실제 설치 Manifest/Binding SHA와 pipe GUID·서버 SID pin, SQOS Identification·bounded 응답·no retry |
| Python Transport | v2_transport.py | arm/work/control 고정 channel, client bytes pin, 임의 exe/서비스 시작/AI 호출 경로 없음 |
| Python Runtime | v2_runtime.py + entry-v2.py + v2_worker.py + fixed-worker-v2.ps1 | 새 State의 단발 SQLite 승인/소비, 고정 fake Worker·90초 예산·Job 정리·엄격한 결과 검증 |

C# V2Contract.cs와 Python v2_contract.py가 exact key/type/path/schema/epoch/GUID/FAKE_ONLY를 검증합니다. C# 네이티브 pipe/token/frame scaffolding은 V1에서 재사용했지만 root 검증·계약·시작 경계·runtime·설치 흐름은 새로 연결했습니다. 단순 전역 문자열 치환이나 V1 guard의 예외 처리로 구현하지 않았습니다.

V2 식별자는 요청 그대로 `LIMITLESSBotTrialV2`, `C:\Program Files\LimitlessRelayTrialV2_20261027`, `C:\ProgramData\LimitlessRelayTrialV2_20261027`, `LIMITLESS-Trial-V2-20261027-S0Host`, `LimitlessPlanningV2-01f8a36c9bfe4241b24b7eaa6caaddc2`입니다. Windows 상 실제 부재·가용성은 이번에 조회하지 않았습니다.

## 3. 권한·승인·원자성

- 후보 폴더는 관리자 설치 경로가 아닙니다. Builder는 프로젝트 Temp의 새 후보 이름 외의 경로, 기존 폴더/ZIP을 거부합니다.
- Wrapper는 기본 Inspect에서 패키지 C#을 실행하지 않습니다. 실제 Install은 별도 관리자 보호 `C:\ProgramData\LimitlessRelayV2Approvals\<approval_id>.json`의 schema2/INSTALL_V2/approved=true/expiry/정확 Manifest·Installer·ZIP SHA/Worker·Admin SID/기존 V1 HOLD 보존 동의가 필요합니다. **이 승인 파일은 이번에 만들지 않았습니다. 후보 Manifest의 approved를 true로 바꾸는 방식이 아닙니다.**
- 검사 뒤 mutable 후보 경로를 다시 실행하지 않습니다. SHA를 확인한 Manifest·payload·ZIP bytes를 메모리에서 사용하고, 검증된 installer ScriptBlock·합성 InstallAssembly.cs·PS core/adapter도 같은 bytes로 로드합니다. V1 보존/운영 pin metadata도 봉인된 payload에서 받습니다. 별도 fresh `-NoProfile` PowerShell 자식이 전제입니다.
- Consume는 V2 approval ID별 BA/SY protected CREATE_NEW 디렉터리/marker로 한 번만 진행합니다. 예외·응답 불명·부분 실패가 소비를 초기화하지 않습니다. V1 Install1/cleanup 기록을 V2 승인으로 사용할 수 없습니다.
- 새 계정은 `New-LocalUser -Disabled`로 생성하고, Windows 반환 SID를 다시 읽어 계정명/marker/LastLogon/Users 단일그룹과 구분합니다. SID는 기존1008·Worker·Admin과 달라야 합니다. 임의 SID/RID 고정0. 코드·State는 native CREATE_NEW와 **생성 시점 protected SDDL**을 사용하고 owner/group/ACE/mask를 읽어 대조합니다. 생성 후 Set-Acl 보정 경로가 없습니다.
- 코드 writer SY/BA만, 새 Bot·Worker는 RX, State writer SY/BA/Worker만, Bot write0. Task는 flags50(CREATE-only + principal ACE 자동추가 금지 + 등록 trigger 무시), Disabled/trigger0/restart0/Limited/IgnoreNew/90초/HardTerminate 정책을 등록 시 SDDL과 함께 지정하고 실제 readback을 검증합니다. 알려지지 않은 ACL 확대·정규화는 HOLD입니다. ISO duration은 XmlConvert로 읽어 정확히90초인지 검사합니다.
- 기존 수정된 TrialAccountPolicy.cs **SHA d9a0bc50137119bc28c17c5496b5fc20b4da81925b806bc472a538c4415f850b**를 bytes 그대로 재사용합니다. UTF-16 Length34/Max36 계약·raw NTSTATUS/Win32/close 오류 보존 유지. Batch 권한 부여는 이번 실행이 직접 생성한 새 SID만 가능하고 직접 권한 readback을 검증합니다.
- 오류의 stage/type/HRESULT/Win32/NTSTATUS/cleanup 숫자만 기록하며 Message/Stack/InvocationInfo/원시 명령줄/환경변수/암호/토큰은 기록하지 않습니다. 설치 결과는 V2 보호 소비 디렉터리에 CREATE_NEW JSON으로, Host 실패는 State 보호를 확인한 경우만 새 제한 오류 파일에 보존합니다. 기록 실패 자체도 HOLD입니다. 정보 미제공에는 오류 코드를 지어내지 않습니다.
- 모든 V2 PS에는 대화형 창을 종료하는 `exit` statement가 없습니다. 성공 판정은 OS exit code만이 아니라 JSON.status와 readback 근거로 합니다. 실제 설치 성공만 `V2_INSTALLED_DISABLED_FAKE_ONLY_NO_EXECUTION`입니다.

## 4. 공유 Worker 보안·충돌 결과

공유 Worker SID1007을 새 계정으로 대체하거나 기존 권한/암호/인증/CODEX_HOME을 변경하지 않습니다. 실제 설치에서는 기존 **직접 SeBatchLogonRight가 이미 존재**하고 DenyBatch가 없으며 Enabled·Users 단일그룹 상태가 확인돼야 합니다. 없거나 조회불명이면 권한을 추가하지 않고 HOLD합니다. 권한 목록을 전후 대조하여 Scheduler의 암묵적 권한 추가도 허용하지 않습니다.

Worker SID → Win32_UserAccount → Win32_LoggedOnUser → Win32_LogonSession → Win32_SessionProcess 관계로 해당 계정 프로세스만 확인합니다. 기존19개 Windows 보호 프로세스의 command line/메모리를 강제로 조회하지 않습니다. 다른 실행 중 Worker·다른 실행 가능한 Worker principal 작업·provider 조회불명·lease busy는 HOLD입니다.

설치/Host가 동일 Worker 전용 보호 lease 파일을 FileShare.None으로 보유합니다. Host는 자기 PID와 생성시각이 일치해야 하고 자식의 부모관계·생성시각도 검사합니다. 오래된 parent PID 재사용/foreign sibling/빈·불완전 snapshot을 거부합니다. 실행 중250ms 감시가 충돌/불명을 발견하면 **새 V2 쪽 취소·자기 Job 정리**로 끝내고 기존 프로세스/서비스/OnBoot는 건드리지 않습니다.

**한계:** 같은 Windows SID·인증·OS를 공유하는 구조로 VM 격리가 아닙니다. 기존 비협력적 직접 실행·간접 launcher와 snapshot/감시 사이 race를 전역 원자적으로 제거했다는 증거는 없습니다. 실제 WMI 완료성/COM·CIM 동작과 기존 작업의 공유 Worker 사용 여부도 이번에는 미조회입니다. 따라서 현재 후보를 실전 동시 실행 안전 확인으로 승격하지 않으며, 실설치/IPC 사전검사에 불명·충돌이 남으면 HOLD합니다. 기존 OnBoot를 자동 변경해 통과시키지 않습니다.

공식 관계 정의: [Win32_LoggedOnUser](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-loggedonuser), [Win32_SessionProcess](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-sessionprocess). 등록 시 보안 기술자 지정: [Microsoft Task security](https://github.com/MicrosoftDocs/win32/blob/docs/desktop-src/TaskSchd/security-contexts-for-running-tasks.md).

## 5. 신규 오프라인 검증

| 범위 | 결과 | 근거 |
|---|---|---|
| 실제 설치 core + 가짜 Windows account/Scheduler/worker 상태 | **84 PASS / 0 FAIL** | CoreOfflineTests.txt, 모든 새 C# compile·PS parse/no-exit 포함 |
| 실제 Python runtime/transport/Worker 접합 + 가짜 Popen/Job/clock | **18 PASS / 0 FAIL** | RuntimeOfflineTests.txt/json |
| C#/Python schema2 교차 | **9 vectors 모두 일치 PASS** | PackageOfflineTests.json와 FinalProof.json |
| Builder의 설치 경로 차단·기존 후보 재사용 차단 | **2 PASS / 0 FAIL** | BuilderBoundaryTests.txt |
| 최종 package bytes/ZIP CRC·집합·SHA·Inspect·memory bootstrap·mutable-source 무재독 | **PASS** | PackageOfflineTests.json, Candidate.json |

Python18은 r4에서 시험한 **5개 실행 모듈 bytes와 최종 r9 SHA가 동일**함을 확인해 재사용했습니다. 최종 r9 Manifest의9개 교차 벡터는 양쪽에서 다시 검증했습니다. 기존 모델72·기존 성공suite·프로세스19개·Task282/service-driver792/process334 전수 검사는 재실행0입니다.

핵심 새 검증은 정상 설치 시뮬레이션, V1 불변, OS 반환 SID 충돌, V1/V2 승인 혼용, Worker busy/launcher/lease/정보불명, SHA·추가파일·empty bytes, ACL 확대, 설치/LSA/등록/기록 실패·부분 상태, 중복/재시도 거부, 90초·IgnoreNew·HardTerminate readback, fake Job 정리, 가짜 VERIFIED/모델호출/과대출력·비밀 추가필드·잘못된 epoch·cancel/timeout·정리 실패 거부입니다. 최종 결과는 **FAKE_ONLY_COMPLETE_NOT_AI_VERIFIED**만 허용하고 실제 AI VERIFIED를 발급하지 않습니다.

개발 중 adapter의 필드/메서드 이름 충돌, Add-Type의 분리 assembly 참조, memory payload의 빈 byte[]→null 변환을 검출·수정했습니다. 초기 실패를 실전 실패로 혼동하지 않습니다. 최종 결과는 위와 같고 실제 계정/Task/LSA/IPC/Worker는 모든 시험에서0입니다. Native Windows 계정 생성·원자 SDDL·Scheduler normalization·실제 Session0/logon4·pipe·Job의 커널 동작은 **미검증**입니다.

## 6. V1 보존 결과와 변경 비교

- 기존 V1 소스13개·기존 JSON3개 **16경로 SHA 변경0**. V1 Installer79194…/Cleanup4d272…·Install1 소비80523…·Cleanup HOLD237984…·관리자 프로세스1b981… 증거 보존. Source/FinalProof 참조.
- 이번 설치된 V1 root855개 최신 재검사0·계정/권한 조회0·쓰기0입니다. 과거 관리자 성공 근거를 재사용했으며 현재 OS 상태를 새로 검증했다고 주장하지 않습니다. V1 SID1008 Disabled를 바꾸는 코드/실행0, Install1 소비 및 cleanup_consumed=false HOLD 기록 초기화0.
- 패키지 비교: 기존 public payload855 중 **Runtime741개 동일**, 변경0, V2 독립파일25개 추가, V1 Code/Legacy 등114개는 **V2에서 제외**했습니다. V1에서114개를 삭제한 것이 아닙니다. 불필요한 기존 AI/Discord/인증 코드와 Legacy를 V2 실행 패키지에 넣지 않았습니다.
- 기존 인증·CODEX_HOME·OnBoot2개·운영 DB·서비스·Unity·게임·Save 변경0. 후보 ZIP/Manifest/코드는 프로젝트 Temp/작업브랜치에만 생성했습니다.

## 7. 실제 관리자 설치 조건 — 이번 승인에 포함되지 않음

1. 바울 대표의 **r9 정확 SHA에 대한 별도 Windows 설치 승인**. 후보 Manifest/ZIP/코드 변경 없이 새 보호 승인 파일에 schema2/INSTALL_V2/approved=true/approval ID/epoch/세 SHA/실제 Worker·Admin SID/1시간 이내 expiry/preserve_v1_hold=true를 기록해야 합니다. 보호 ApprovalRoot ownerBA/groupSY/protected SY·BA Full만 필요합니다. 이 root/승인/소비/lease를 이번에 생성하지 않았습니다.
2. fresh NoProfile PowerShell7 자식에서 검토된 wrapper bytes만 사용. V2 이름/Code/State/Task가 실제 부재·권한/경로가 신뢰 가능해야 합니다. 충돌/부분 설치/조회불명은 HOLD이며 재사용·덮어쓰기하지 않습니다.
3. 기존 Worker의 SID/직접 Batch 권한/Users·Enabled/작업·프로세스·lease·기존 OnBoot 및 boot 승인 pins 확인. 기존 Worker 권한/인증을 고치는 단계는 없습니다. V1 실제855 SHA/기존 비활성SID·운영 pins도 승인된 설치가 새로 확인하고 전후 보존을 확인해야 합니다.
4. 새 Bot 암호와 기존 Worker credential은 **관리자 로컬 보안 입력만**, 채팅/스크립트 평문/로그/복사0. 기존 계정·root·task·Manifest의 overwrite와 V1 Cleanup/Install 호출0.
5. 설치 성공 후에도 새 Bot/Task Disabled, trigger0, 실제 Host/IPC/Worker 실행0. 별도 IPC 승인 없이는 start-host가 거부합니다.

실제 IPC는 설치 결과 판독 이후 **또 다른1회 승인** 대상입니다. 필요한 V2 계정 임시 활성·Task 단발 시작/비활성 복귀·새 Host 자기 Query ACL 범위·충돌 검증·90초 종료/active0·서버/클라이언트 actual SID/Session/logon4를 먼저 확정해야 합니다. IPCApproval.json도 BA 보호된 별도 epoch/Manifest·Binding SHA·최대90초 승인이고 Host 소비 marker가 CREATE_NEW입니다. AI/usage/Discord/A모드는 이 후보로 허용되지 않습니다.

## 8. 부분 실패와 원상복구 계획

자동 재시도·자동 삭제·원상복구0입니다. account 생성은 처음부터Disabled, 등록 Task도 처음부터Disabled/trigger0입니다. publish/권한/Task readback/전후 보존/기록 실패는 즉시HOLD하고, 생성이 응답불명인 수는 null/attempt와 단계로 구분합니다. 새SID/Binding/소비·원시 숫자 오류와 남은 자산을 보호 기록합니다. 기존 V1은 복구 대상으로 다시 쓰지 않습니다.

실제 설치가 승인돼 실행된 뒤 부분 실패가 발생하면 **같은명령 재실행 금지**입니다. 관리자 결과를 판독하고 정확 V2 SID/marker/생성파일 SHA/Task owner·상태/자기 Job active0를 확인한 별도 복구 승인이 필요합니다. 그때도 V1 원본·1008·Worker1007·OnBoot·원래 승인/소비를 지우거나 덮어쓰지 않습니다. V2만의 비활성 유지/제한 정리/신규 SID 직접권한 해제 등은 승인된 별도 절차에서 검토하며, 이번 패키지에는 Remove/Cleanup/Stop 실행 action을 제공하지 않습니다.

## 9. 마감

CURRENT_STATUS/CODEX_LATEST 갱신과 관련 Issue66 보고를 연결합니다. 실제 commit SHA는 마감 기록 참조. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**. ZIP 실물은 로컬, 문서는 로컬 commit이며 Issue 댓글만 실제 원격 게시하므로 GITHUB_REPORT_COMPLETE라고 주장하지 않습니다.

실제 계정·폴더 설치/ACL/Task/LSA·IPC·Worker·AI·공식사용량·Discord·A·봇·아트·OnBoot·Unity·Save·Push/main·종료/재부팅0. **후보 구현·패키지·오프라인 QA 후 중지, 실제 설치는 별도 승인 대기**입니다.
## V2 후보 구현 마감 기록 (2026-10-27)

- 구현·증거 로컬 커밋: 62578e5174d208dd4e83ebe23c2ac438303eb478
- 실제 Issue #66 보고: https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6430994946
- LOCAL_COMMITTED / REMOTE_NOT_PUSHED. Issue 댓글만 원격 반영, 코드·문서는 로컬입니다.
- V1 REFERENCE_VERIFICATION_HOLD 유지. V2 r9 미승인·미설치이며 별도 실제 설치 승인 전 중지합니다.
