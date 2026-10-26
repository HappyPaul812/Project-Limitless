# Issue66 프로세스 참조 HOLD 대안 — 자산 검증과 V2 보존형 독립 시험 설계

2026-10-27 KST. 상태 **REFERENCE_VERIFICATION_HOLD / ALTERNATIVES_OFFLINE_ONLY / V2_NOT_IMPLEMENTED_NOT_INSTALLED**.
브랜치 codex/discord-usage-diagnostics-20261026, 기준 d6de3befd4449cb733ce23c9ed64f17068fb7e8a. 이번은 대안 분석·합성 검증만입니다.

## 권고와 두 방법 비교

**기존 자산을 삭제하지 않는 V2 후보의 별도 구현을 다음 단계로 권고합니다.** 현재 설치 가능하다는 뜻은 아니며, 새 Manifest·설치기·런타임 경로 분리와 검토가 선행돼야 합니다. 기존 REFERENCE_VERIFICATION_HOLD는 그대로 유지하고 V2 전용 승인 게이트를 별도로 둡니다. 기존 설치기의 부분설치 거부를 삭제하거나 예외 처리하지 않습니다.

| 항목 | Restart Manager 보조 검사 | 기존 자산 보존형 V2 |
|---|---|---|
| 기존 프로세스19개 식별·과거 참조 복구 | 해결 불가 | 해결하지 않으며 기존 HOLD 보존 |
| 얻을 수 있는 근거 | 등록한855개 파일의 현재 사용자를 RM이 보고하는지 | 별도 시험 자산·승인·이름공간과 실행 경로의 독립성 |
| 삭제 가능 판정 | 빈 목록만으로는 불가 | 기존 자산 삭제 자체가 불필요 |
| 새 개발량 | 작음: 전용4함수 adapter+배열/오류/정리 검증 | 중간: 최소8개 구성요소의 V2 파생본+결합검증 |
| Windows 실제 변경 | 세션·리소스 등록의 내부 기록/쓰기 동반 | 후속승인 시 새 계정/코드·상태/Task/신규SID 배치권한 생성 |
| IPC 목표까지의 효율 | 검사 후에도 HOLD가 남아 V2 작업이 추가될 수 있음 | 삭제 문제와 시험을 분리하여 목표에 직접 연결 |
| 현 상태 | 합성 모델만, 실제 API 호출0 | 합성 모델만, 실제 설치기/패키지/Manifest 없음 |

가장 적은 **즉시 개발량**은 RM이지만, 목표인 실제 Session0 IPC까지의 총 작업과 반복 검사 비용은 V2로 바로 후보를 준비하는 편이 적을 가능성이 높습니다. 이는 코드 의존관계 분석에 근거한 권고이며 실전 성공 보장은 아닙니다. RM 실측은 향후 별도 승인 시 필요한 보조 증거로만 사용합니다.

## 기존 근거와 보호 범위

최신 Issue66 [기획 후속](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6429398165), [관리자 판독](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6429320304) 재사용. 관리자JSON f5c37d29… SHA1b981f432d715025724d94381e97c993532bacd773ad2a6de9b16389b2a65a18, Cleanup판독23798422…, Install1소비80523d2b… 보존 확인. 현재15개 메타데이터·종료객체1·누락3, 직접참조0/비참조확정0에서 새 실관측은 없습니다. Task282/서비스·드라이버792/프로세스334/855파일의 기존 성공검사를 반복하지 않았습니다.

기존 Code root C:\Program Files\LimitlessRelayTrial·비활성 LIMITLESSBotTrial SID1008·소비/승인/실패기록·기존 후보는 그대로입니다. 현재 Windows 자산의 최신상태를 다시 검증했다는 의미는 아닙니다. 이번 실제 SHA 점검은 지정된 로컬 소스·기존 JSON에만 한정했습니다. 13개 소스/3개 JSON SHA는 Evidence/TrialAlternatives20261027/PreservedSourceSHA256.json에 기록했습니다. 승인된 외부 후보의855개 파일을 재스캔하거나 기존 보호경로를 열거하지 않았습니다.

## Restart Manager로 증명 가능한 범위와 한계

공식 API는 등록한 자원을 현재 사용하는 응용프로그램·서비스 목록을 제공합니다. 해당 시점·해당 파일 목록에 대한 보조 관측입니다. 문서의 설치/재시작 절차 중 종료/재시작 API는 이번 설계와 실행 경계에 포함하지 않습니다.

1. 시작 전 원래855개 목록을 승인 Manifest SHA와 연결하고, 파일 존재·SHA·최종 경로·보호ACL·ancestor/자식 reparse·hardlink/ADS 등 자원 동일성은 별도 검사로 확인해야 합니다. RM 자체는 이 조건들을 인증하지 않습니다. 이번 모델은 가짜 파일 증거만 사용합니다.
2. `RmStartSession(flags=0)`으로 세션 하나를 얻고, 성공한 경우에만 후속 호출합니다. 사용자 세션당 동시 최대64개 제한, 실패는 숫자코드와 HOLD로 남깁니다.
3. `RmRegisterResources`에 855개의 **완전한 파일 경로**를 한 묶음으로 전달하는 안입니다(nFiles855/nApplications0/nServices0). 설치 root 디렉터리 자체를 자원으로 등록하지 않습니다. root는 경로 경계·ACL 검증 기준입니다. 폴더를 파일처럼 등록하면 RmGetList가 ERROR_ACCESS_DENIED5를 반환할 수 있습니다. 파일별855회 등록은 하지 않습니다.
4. `RmGetList`의 234(ERROR_MORE_DATA)는 배열 크기를 재할당하여 최대3번만 처리하는 설계입니다. 4096행 제한은 이번 모델의 자원상한이며 Windows 공식 최대치가 아닙니다. 지속 변동/초과/기타오류/비정상 개수는 HOLD. 실제 adapter는 RM_UNIQUE_PROCESS의 PID+생성 FILETIME을 함께 보존하고, 서비스이름·Session 등 허용필드만 기록해야 합니다. 855개를 한 세션에 등록한 집합 결과이므로 각 파일과 각 프로세스의 일대일 대응을 추정하지 않습니다.
5. 목록이 있거나 크기응답이 사용자를 시사하면 `RM_USERS_REPORTED_HOLD`, 빈 성공 목록도 `RM_NO_USERS_REPORTED_HOLD`. **어느 경우도 REFERENCE_VERIFICATION_HOLD 해제/NO_REFERENCE_VERIFIED/삭제 허가로 전환하지 않습니다.**
6. 시작 성공 시 등록·조회 예외에도 finally에서 `RmEndSession`을 시도합니다. End 실패도 별도 숫자코드/HOLD. 121/29/5/6/160/1223 등 오류를 정제 기록하며 재부팅·서비스 시작·권한 우회·자동 전체 재검사는 하지 않습니다.

중요한 차이: 이 API 묶음은 프로세스/파일 내용을 변경하는 용도가 아니지만 **엄밀한 무변경 조회는 아닙니다**. 등록 호출은 쓰기를 동반하고 세션 API는 Registry write mutex를 사용합니다. 기존 문서에서 RM을 무변경 사전검사에 쓰지 않은 이유와 충돌하지 않습니다. 이번 사용자 요청은 이를 **합성 평가**하는 범위로 이해했으며 실제 RM 등록은0입니다. 향후 실제 사용 승인에는 이 내부 상태 쓰기를 명시해야 합니다.

빈 목록은 미등록 파일/계정·Task의 문자열 참조, 향후 열기, 이미 닫힌 파일, 환경변수·junction·우회 별칭, 다른 파일을 통한 간접 참조, 과거 실행 이력을 증명하지 않습니다. 보호프로세스 탐지의 완전성도 이번에 검증하지 않았습니다. RM이 보지 못하는 항목이 없다고 가정하거나 접근 제한을 우회하지 않습니다. 실제 사용 파일은 현재 **미검사**입니다(가짜 사용중 사례만 시험).

공식 출처: [RmStartSession](https://learn.microsoft.com/en-us/windows/win32/api/restartmanager/nf-restartmanager-rmstartsession), [RmRegisterResources](https://learn.microsoft.com/de-de/windows/win32/api/restartmanager/nf-restartmanager-rmregisterresources), [RmGetList](https://learn.microsoft.com/zh-cn/windows/win32/api/restartmanager/nf-restartmanager-rmgetlist), [RmEndSession](https://learn.microsoft.com/en-us/windows/win32/api/restartmanager/nf-restartmanager-rmendsession), [등록한 자원만 사용](https://github.com/MicrosoftDocs/win32/blob/docs/desktop-src/RstMgr/using-restart-manager-with-a-primary-installer.md). 일부 영어 API 페이지 응답 실패로 동일 Microsoft Learn의 공식 다른 언어 페이지를 확인했습니다.

## V2 독립 환경 — 기술적 구현 가능 / 현 패키지로 설치 불가

현재 V1은 installer/runner/Host path verifier의 고정 경로·계정·Task 및 양쪽 pipe 정규식이 결합돼 있습니다. 경로 변수만 바꾸거나 기존 설치기를 재실행해서는 V2가 되지 않습니다. V1 guard `Assert-TrialFreshState`와 runner의 `NEW_INSTALL_ONLY_HOLD`는 현재 SHA 그대로 보존합니다.

검토용 이름은 아래와 같습니다. **제안이며 OS의 현재 부재를 확인한 이름이 아닙니다.** 실제 계정·Task·경로가 이미 존재하거나 조회가 불명확하면 V2도 HOLD합니다.

| 요소 | 기존 V1 보존 | V2 제안 |
|---|---|---|
| 계정 | LIMITLESSBotTrial / SID1008 Disabled | LIMITLESSBotTrialV2 / OS가 신규 할당한 독립SID / 생성부터 Disabled |
| Code | C:\Program Files\LimitlessRelayTrial | C:\Program Files\LimitlessRelayTrialV2_20261027 |
| State/DB/Workers/ready | 기존 경로·증거 그대로 | C:\ProgramData\LimitlessRelayTrialV2_20261027 아래에서만 생성 |
| Task | LIMITLESS-Trial-S0Host | LIMITLESS-Trial-V2-20261027-S0Host |
| pipe | LimitlessPlanning-… | LimitlessPlanningV2-새난수GUID-(arm/work/control) |
| 승인/소비 | Install1 및 기존SHA 불변 | 새 epoch20261027-v2-01·별도Manifest/receipt/단발소비 |
| 실행정책 | MANUAL/A HOLD | FAKE_ONLY, TaskDisabled, triggers0/restart0/Limited |

신규 이름/SID/경로는 대소문자·8.3·reparse·hardlink·최종경로·ancestor 권한 및 파일 ID 충돌을 읽어 검증한 뒤만 사용합니다. 충돌/모름은 변경 전 HOLD. OS 신규SID를 예측하거나1009/1010 등으로 예약하지 않습니다. 합성 모델의111-222-333-1010은 가짜fixture이며 실제SID 후보가 아닙니다.

### 생성 시점 보안과 승인 경계

- 새 installer는 V2에 고정하고 일반적인 임의 root/account/task 매개변수나 V1 fallback을 제공하지 않습니다. V1 존재는 **보존 계약**의 일부일 뿐이며 V1을 fresh로 판정하지 않습니다. 기존 참조 HOLD를 감수한 병행 namespace 설계 승인을 명시적으로 받은 이후에만 V2 전용 단계로 진행합니다. 새 경로의 참조/상태가 불확실하면 V2도 HOLD입니다.
- 새 Code·State·모든 child는 protected DACL과 정확 owner/group을 **생성과 동시에** 적용합니다. 코드 writer는 SY/BA만, Worker·새Bot은 필요한 RX만. 새 State의 writer는 SY/BA/Worker만, 새Bot의 쓰기·Task 실행/변경권한 없음. 복사 후 ACL을 보정하여 노출 창을 만드는 방식은 사용하지 않습니다. Task는 등록 호출의 SDDL로 BA/SY Full+Worker 정확Read만, flags50(CREATE/별도principalACE금지/등록trigger무시), Disabled·trigger0·Limited·retry0를 원자 등록하고 즉시 읽어 대조합니다. 알 수 없는 mask 정규화는 HOLD입니다.
- 새Bot은 Users만·한정만료·생성부터Disabled. 기존1008/Worker1007/Admin1001 및 SY와 달라야 합니다. 필요한 SeBatchLogonRight는 새 생성SID만 한정해 별도 실제설치 승인에 포함하고 기존 Worker 권한/암호/인증·CODEX_HOME은 변경하지 않습니다. Credential은 향후 관리자 로컬보안입력만, 기존 계정암호/인증복사0입니다.
- 기존 Worker 계정으로 **별도 새Task**를 구성하는 안이라 같은 서비스 SID·인증·Windows 자원을 공유합니다. 이는 VM 수준 격리가 아니며 Worker principal 자체는 기존 자산에 접근할 능력이 있을 수 있습니다. 따라서 고정 실행경로·새 DB/workspace·모드검사·승인SHA·실행 그래프 검사로 실제 접근 범위를 제한해야 합니다. FAKE_ONLY에서 Legacy/AI/usage/Discord import·호출 경로로 가지 않음을 새 결합시험으로 확인해야 합니다. CODEX_HOME은 읽어 변경 여부를 비교할 수 있으나 내용 수집/복사/갱신 금지입니다.
- Session0Host는 install 인자로 경로를 받아 재사용 여지가 있습니다. 그러나 `Session0PipeBoundary.VerifyProtectedPath`와 start PS는 V1 고정이므로 V2 파생본에서만 새 고정root를 허용하고 기존guard를 넓히지 않습니다. Python runtime의 schema1/실추론허용 모드는 V2 schema2/FAKE_ONLY 고정으로 제한합니다. pipe prefix는 C#/PS/Python 양쪽 모두 분리합니다. Request IDs·DB/permits·소비는 새State 안에서만 사용합니다.
- 설치 단계는 IPC/Host/가짜Worker 실행0. 향후 IPC별도 승인 시에만 **V2 계정 활성화와 Task의 단발 시작/비활성 복귀**까지 정확 범위를 합의해야 합니다. 기존1008 활성화와 V1 시작은 계속 금지입니다. 시작 전 새SID·실제Session0/logon4·비관리자 토큰·서버/클라이언트 신원·파일/pipe ACL·모든SHA를 확인해야 하며 합성PASS를 이 증거로 대신하지 않습니다.
- 실패 시 자동삭제/롤백/재설치0, 기존 자산 무변경, 신규 부분자산은 생성당시 제한 상태로 남기고 단발소비·정제오류·정확 생성내역을 보존/HOLD합니다. Task 응답불명은 재등록하지 않습니다. 기존실패증거/소비를 초기화하지 않습니다. 나중에 새 부분자산 정리도 별도 검토·승인 대상입니다.

### 예상 수정 범위와 SHA

**이번에는 실제 설치·런타임 코드는 하나도 수정하지 않았습니다.** 최소8개 별도 V2 파생 구현 및 전용 결합검증이 필요합니다. 현재 소스SHA는 아래 증거파일에 있고, 미래 코드SHA는 구현 후에만 발행할 수 있습니다.

| 별도 신규 파생본(후속 작업 제안) | 근거/수정 목적 |
|---|---|
| prepare-session0-v2-candidate.py | 새 폴더/ZIP에만 만들고 V1 후보 보존, schema2·고유epoch/승인·pipe·실제 파일목록 고정 |
| install-session0-v2.ps1 | V2고정 namespace·-Disabled 생성·새SID경계·원자ACL·fresh V2만·V1불변·성공/실패읽기검증 |
| run-session0-v2-install.ps1 | 별도SHA/단발소비·V1보존계약·V2 충돌/승인만 검증, 설치뿐이며 IPC 시작 기능 없음 |
| V2/Session0PipeBoundary.cs | V2 exact Code/State/pipe만, 기존SID·Session·peer 검증 유지 |
| V2/start-session0-host.ps1 | V2 root/schema2/FAKE_ONLY/task action pin, 임의모드·외부CLI경로 거부 |
| V2/session0-pipe-client.ps1 | V2pipe만 허용하고 원본클라이언트 불변 |
| V2/session0_pipe_transport.py | V2pipe prefix·새바이트pins, 기존boot승인 읽기만 |
| V2/broker_host_runtime.py | schema2·FAKE_ONLY·새State 소비장부 고정, 실제모드 승격거부 |
| 전용 신규 결합시험/별도Manifest·ZIP·receipt | 위8개·전용scope 및 생성ACL, 패키지변조/경계/실패주입 검증 |

기존 TrialAccountPolicy.cs의 수정된UTF16/오류보존 코드는 같은SHA로 재사용하는 안이며 새installer가 **직접 생성한 새SID**와 기록SID 및 기존SID불일치를 검증한 후만 호출하도록 해야 합니다. 필요하면 V2전용 추가SIDguard 파생본을 더할 수 있으나 아직 구현하지 않았습니다. Session0Host.cs/fixed-planning-entry.py/probe-session0-host.py는 경로를 인자로 받거나 상대경로여서 재사용 가능성이 있지만 **전체 실행 그래프에 V1 고정경로가 없는지** 후보생성 후 확인해야 합니다. 보존대상 source가 무조건 재사용 가능하다고 확정하지 않습니다.

새 패키지는 실제 구성된 파일목록·SHA를 확정해야 하므로 최종 payload 개수를855로 가정하지 않습니다. 이번855는 RM 및 복사형 합성 fixture 기준입니다. 설계JSON은 HostManifest가 아니며 설치기가 사용할 수 없습니다. **V2 installer/installed Manifest/package SHA는 아직 없음(null)**. V1 LSA후보 Manifest33ed…/ZIPd678…를 V2 승인으로 재사용할 수 없습니다.

## 이번 산출물과 SHA
| 파일 | 실제 SHA256 |
|---|---|
| Tools/DiscordAIServiceDiagnostics/TrialAlternativesOfflineModel.cs | `9a54522d106bdfdd514b06c12446c9a0dd74e090af2a55d78fe05f6de2e19f89` |
| Tools/DiscordAIServiceDiagnostics/TrialAlternativesOfflineTests.cs | `9c102f1c94b765c9cf5d75ec074b054765ee7dba90a4f2526f590db4f1158449` |
| Tools/DiscordAIServiceDiagnostics/test-trial-alternatives-offline.ps1 | `61eded4faab654b8a32a20072f0ba78fe27b695bef292797aa1636537b01c448` |
| Tools/DiscordAIServiceDiagnostics/v2-isolation-design.json | `f75d41fdf455188e3db87f1e78d9fed9e463b6be7e5de29de32096187813f8e9` |
기존 source SHA 전체는 [PreservedSourceSHA256.json](팀운영/Evidence/TrialAlternatives20261027/PreservedSourceSHA256.json), 새 source SHA는 [SourceSHA256.json](팀운영/Evidence/TrialAlternatives20261027/SourceSHA256.json). 미래 수정본 SHA를 미리 지어내지 않았습니다.

## 오프라인 검증 — 신규72 PASS / 0 FAIL

[OfflineTests.txt](팀운영/Evidence/TrialAlternatives20261027/OfflineTests.txt). BaselineInventory SHA c2d398be42d7a12fb4574bff5e2a8820271ec82e355993f24e017a6e706f16d3를 pin하고 이미보존한855개 경로/SHA를 **가짜 존재/보호/SHA 상태**로 넣었습니다. 실제855파일 상태/Windows보안은 조회하지 않았습니다.

- RM: 단일855등록·빈목록HOLD·사용자발견HOLD·234배열증가경쟁3회상한·세션한도353·오류5/6/29/121/160/1223·등록실패/예외finally End·End실패/예외·과다버퍼/재부팅이유HOLD, 폴더/누락/SHA/권한불명/reparse/hardlink/중복/경로탈출 거부.
- V2: 가짜파일·계정·Scheduler 상태에서 기존 Code855/계정Disabled/Manifest/Install1소비/Worker인증/CODEX_HOME/OnBoot2/boot승인/운영SQLite 불변. 계정·Task·경로·pipe·SID 충돌/Worker바인딩변경·만료/소비승인·SHA·Enabled/trigger/retry/RunLevel/등록UPDATE·실모드·관리자그룹·Debug권한·코드Worker쓰기/StateBot쓰기/TaskBot접근 등 확대거부.
- 새경로 참조 불명/사용중·보존증거불명 거부, publish/account/Scheduler/post검증 실패마다 HOLD·소비보존·자동재시도0·기존자산불변·새계정/Task제한상태유지·실제/가짜Worker호출0.

정적검토: C# 모델에 native DLL/파일시스템/프로세스 실행 adapter 없음. PS runner 명령은 Add-Type/JSON·지정기존Inventory읽기/hash·필터만, 파싱PASS. 새 C# 컴파일PASS. git diff --check PASS. 기존 성공테스트suite 재실행0. 새72개만 baseline hash pin 추가 후 최종확인했습니다.

**한계:** 이는 설계상 상태기계·거부조건 시험입니다. 실제 RM 마샬링·커널 자원식별·Windows 계정/SID생성·원자ACL·Scheduler정규화·Session0 배치logon·실IPC는 미구현/미검증입니다. 가짜 ACL role 배열과 가짜 Scheduler 상태가 실제 SDDL/권한검증을 대신하지 않습니다. 아직 설치/IPC 실행을 승인할 수 없습니다.

## 다음 승인 순서와 중지점

1. V2보존형 병행 구조를 선택하면 다음 범위는 **후보 구현·새Manifest/ZIP SHA·추가 오프라인결합검증**입니다. 별도승인 전 실제Windows변경0.
2. 후보가 검토 가능해진 뒤 새계정/만료/신규SID배치권한·V2코드/상태원자ACL·Disabled Task등록·보호기록/승인소비만 한정한 **실제 설치 승인**을 받습니다. 기존 V1 HOLD/소비는 그대로입니다. 설치성공 후에도 실행0.
3. 실제Session0 IPC는 새 SID·토큰·logon4/Session0·상대신원 검증을 포함한 별도1회 승인 단계입니다. 임시계정활성/Task시작·종료정책·새Host자기 Query ACL 조정의 필요성과 범위도 구체화해야 합니다. AI/usage/Discord/A모드는 이 단계에서도 승인되지 않습니다.
4. RM실측을 별도로 선택한다면 내부세션/등록쓰기와855파일 한정 범위를 승인받은 뒤 전용adapter를 구현·검토합니다. 빈 결과를 기존 Cleanup 재실행 근거로 사용하지 않습니다.

이 문서는 후속승인 요청을 위한 대안입니다. **이번에 실행명령/관리자 설치명령은 제공하거나 실행하지 않습니다.** 기존 Cleanup/Install 재실행·파일/폴더/계정삭제·계정생성/활성·Task등록·권한변경·프로세스/서비스종료·IPC/FakeWorker/AI/usage/Discord/A/봇·아트·OnBoot/Unity·게임·Save/Push/main/재부팅 모두0.

## 보고 상태

CURRENT_STATUS/CODEX_LATEST 갱신, 같은 작업브랜치 로컬커밋. 실제commit과 Issue66 댓글은 마감기록에 연결합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 문서는 원격 미반영이므로 GITHUB_REPORT_COMPLETE가 아닙니다. **대안 설계·오프라인 검증 후 HOLD로 중지합니다.**