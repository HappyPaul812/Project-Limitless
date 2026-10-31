# Discord V2 FAKE_ONLY 보호 배포·실행 승인 경계 준비안

**PREPARED_ONLY / NOT_APPLIED / NO_GO / MANUAL / EXECUTION_HOLD.** 이번 변경은 문서와 실행 불가 명령 초안뿐입니다. 보호 폴더 생성·ACL 적용·이미지 복사/이동/수정·gate 활성화·컴파일/빌드·EXE/Task 실행을 하지 않았습니다.

기준: Issue66 최신보고6538958076·사전확인849e3e7e·영수증 `ba8bb57821bdca3e35b1870818efae1f2f49bc18`. 작업 브랜치 `codex/discord-v2-protected-deploy-gate-plan-20261031`. 기존 구현/보안 계약/기본gate=false는 그대로입니다.

## 1. 보존 확인과 현재 전제

10-31 13:30:49KST 재확인:

| 기존 이미지 | SHA256·크기 | 판정 |
|---|---|---|
| Probe | `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322`,1,892,352bytes | 보존PASS |
| Supervisor | `CCA8202417DD4D3A1E4F0D301143E9960EEFC3B984EAC79FC92DC6FA08974ECD`,7,536,640bytes | 보존PASS |

두 파일은 기존 PE32+AMD64·CLR없음 검증과 같은 바이트이며 수정/이동/실행0입니다. CodeRoot/EvidenceRoot는 여전히 없습니다. 코드에 고정된 두 루트의 **기존 상위 폴더**는 각각2개 관측(중복 포함4개), 소유자와 지원/변경 ACE를 기존 Descriptor의 정적 기준으로 비교했을 때 차단 사유0이었습니다. 이 메타데이터 확인은 실제 Worker Batch token의 AccessCheck나 HANDLE 검증 PASS가 아닙니다. 다음 적용 직전에도 재검사해야 합니다.

SID1010 계정 만료는 **2026-11-02 00:00KST**, 코드 승인 cutoff는 **11-01 23:59:59KST**입니다. 실제 암호·공유/다른 실행기 사용은 조회/인증하지 않았습니다. 새 V3 정상 판정은NOT_VERIFIED, Windows 전체 동일SID 부재는UNKNOWN입니다.

## 2. 관리자 승인 후의 최소 보호 배포 절차 — 현재 미적용

### 2.1 적용 대상과 중단 경계

정확한 SID/절대 경로/Task/원본 이미지 위치는 ignored 로컬 `Temp/NativeBatchPreparation20261028/FinalPreflight20261031/GUI_Targets_PRIVATE.LOCAL.json`을 사용합니다. 공개 문서에서는 Native.CodeRoot·Native.EvidenceRoot 역할명만 씁니다. 소스 binding과 사용자 승인 manifest의 값이 다르면 HOLD입니다. 기존 OnBoot/운영 자산·상위 폴더 ACL을 이 단계에서 수정하지 않습니다.

1. 관리자 토큰·대상SID·계정 만료·현재 V3상태·원본 해시·정확한 새 적용 manifest를 확인합니다. 코드/증거 루트는 분리되고 기존 Task·파일·소비/lease 기록이 없어야 합니다. 기존 항목을 발견하면 **덮어쓰기/상속 재설정/소유권 탈취/삭제 없이 HOLD**합니다.
2. 경로를 드라이브 루트부터 읽기 확인합니다. reparse/정션/네트워크·비지원 ACE·비신뢰 소유·상위 교체/DeleteChild/WriteDac/WriteOwner 권한 또는 유효권한 불명이면 중단합니다. 현재 상위 정적검사 통과는 이 단계의 실행 승인이 아닙니다. 상위 권한 문제가 생기면 별도 범위 승인 없이 `/T`나부모ACL수정을 하지 않습니다.
3. 필요한 **새 전용 중간 폴더부터** 명시 보안 descriptor를 가진 생성 API로 순서대로 만듭니다. 제안 API는 `CreateDirectoryW`+SECURITY_ATTRIBUTES(상속HANDLE=false)이며 ERROR_ALREADY_EXISTS를 부재로 취급하지 않고 HOLD합니다. “일단 mkdir 후 권한 줄이기”나 기존 폴더 채택 방식은 배제합니다. 원자적인 초기 DACL·생성 직후 owner/SDDL/최종경로 readback을 검증하는 준비 도구는 아직 구현/검증하지 않았습니다.
4. 승인된 새 이미지·제어자료만 `CreateFileW`+CREATE_NEW+명시 보안 descriptor/공유제한으로 복제합니다. 원본은 읽기 핸들을 유지하고 변경·삭제 공유를 허용하지 않습니다. 새 복제본의 SHA/크기·소유/DACL·최종경로를 읽어 확인한 뒤에만 배포 후보로 인정합니다. 원본 이미지 이동·직접 패치·기존 파일 교체 금지입니다.
5. 적용한 새 항목별 목록·전후 metadata·성공/실패 단계를 별도 관리자 기록에 남깁니다. 중간 실패 시 증거를 보존하고 HOLD, 자동rollback/폴더삭제/ACL복구를 하지 않습니다. 정리도 정확한 대상·별도 승인이 필요합니다. 실제 Task 생성은 이 단계에 포함하지 않습니다.

### 2.2 정확한 최소 권한/보존 계약

| 대상 | owner·DACL·Worker 권한 |
|---|---|
| 새 전용 경로·CodeRoot | owner=BA 또는SYSTEM, DACL protected(`D:P`), 기존상속 제거·명시ACE만. SY/BA full, Worker=0x1200a9(읽기/탐색/실행). 새 하위 항목도 각각 명시 descriptor 적용. |
| EvidenceRoot | owner=BA/SYSTEM·protected. SY/BA full, Worker=0x1200ab(읽기/탐색+새파일생성). 하위폴더생성/기존항목교체/DeleteChild/WriteDac/WriteOwner 불허. |
| 이미지 파일 | owner=BA/SYSTEM·protected, SY/BA full, Worker=0x1200a9. CodeRoot의 정확한Supervisor.exe/Probe.exe 경로·해시결합. |
| TrialContract·동작승인·신뢰진술/basis | owner=BA/SYSTEM·protected, SY/BA full, Worker=0x120089(읽기), 중복/미지필드거부·한도·해시결합. 자동으로Approved=true/실제확인 가정을 채우지 않음. |
| 관리자 receipt/intent | 기존 CreateAdminRecord: ownerBA·protected, OWNER RIGHTS의Delete/WriteDac/WriteOwner deny(0x000d0000), SY/BA full, Worker read0x120089. 파일명/TrialId/ContractSHA/ApprovalId 결합. |
| Worker 새 증거·소비/lease | 기존 CreateRecord/RecordDescriptor의CREATE_NEW·share0/write-through/flush·OW deny·owner/ACE readback 유지. **Worker 자신의 기록에0x120003 읽기/쓰기가 남는 현재 계약**을 “적대적동일SID도변조불가”로 과장하지 않음. 기존기록을코드가overwrite/delete/reset하지않는계약과비신뢰동일SID부재라는관리신뢰가정은별개. |

새관리경로/파일의SDDL 형식은 `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;<대상별mask>;;;<WorkerSID>)`입니다. Worker record 권한을관리자료에재사용하거나 `/grant:r`만으로기존다른ACE가제거됐다고가정하지않습니다. 최종판정은 raw descriptor와실제제한token AccessCheck·HANDLE pin/readback이합니다.

## 3. 실행 게이트 최소 변경안 — 제안만, 제품 소스 변경0

### 범위

전역 `ReleaseAdmissionImplemented=false`는 유지합니다. 일반운영/LIVE/IPC/AI나임의CLI에대해true로바꾸지않습니다. **새로승인한하나의TrialId+FAKE_ONLY_EXECUTION_V1에만종속되는별도진입정책**을 추가하는 안입니다. 기본 AllowedTrialId는미발급/빈값으로거부하며, 시험용값확정·구현·새빌드각각을별도승인받습니다. 환경변수·새`--force`·호출자bool·패스워드추측/예외·무기한허용flag로열지않습니다.

허용모드는정확한 `--supervise`, `--register-only`, `--start-once`, `--stop-owned-instance`, `--remove-owned-task`뿐이고 마지막네모드는각동작별승인/계약Scope를반드시요구합니다. mode/args/경로/역할/새TrialId/profile/해시/유효기간이하나라도불일치하면원래HOLD로닫습니다. 전역UNKNOWN은그대로기록합니다.

### 기존 검증을 통과한 뒤에만 실제 동작

| 접점 | 최소 변경 방향 |
|---|---|
| Program.Main | blanket gate를지우고즉시실행하는대신정확한형식·경로의제한된trial 사전검증 경로로만분기. 기본운영gate는계속false. 새경로도검증실패시exit/HOLD. |
| 관리자 동작 | AdminWindows의관리자/상승·비위장·보호tree/read를유지. AdminController의strict계약/SHA·FAKE_ONLY신뢰자료·현재계정/직접Batch권리·동작승인·Task/receipt결합확인뒤, **첫backend.Save/등록/Enable/Run/Stop/Remove 전에**검증된모드별trial permit을요구. 등록승인으로시작permit이생기지않음. |
| Supervisor | ReleaseAdmission의pure/read-only preflight와실제동작을나눔. 현재Prepare는계약을읽기전에자기process/thread/token DACL을Harden하므로, **그3개Harden 호출을승인검증후로이동**해야함. 먼저strict계약/고정TrialId·profile/경로·두이미지해시/Start승인·registered/start-intent·신뢰근거/계정/제한token을읽기검증. permit후기존Hardening/HANDLE검사·scope재검사→영구단발소비→기존Job생성시편입·자식제한→Probe재개순서. 검증전에DACL을바꾸거나소비기록을쓰지않음. |
| 소비·receipt·실패 | 기존CREATE_NEW/영구소비/lease·정확instance·경로/HANDLE pin·해시재검증은그대로. permit은process내검증된입력/역할/동작에만묶고직렬화/파일flag/다른동작에재사용하지않음. 실패·Run응답유실은새시도권이아님. |
| cleanup·cutoff | 기존cleanup 파서는만료후정리예외가있음(`cleanup || expiry>now`). 새trial진입은**새관리동작의현재시각≤cutoff·신규승인유효성을명시검사**하여이예외로마감후옛승인을재사용하지않음. 이미승인돼실행중인자기Job의fail-safe종료는막지않음. 마감후수동개입은별도사고대응지시가필요하며자동연장이아님. |

이변경은아직코드패치/컴파일/실Windows검증하지않은구현후보입니다. 순수/모의delta검증에서잘못된profile/TrialId/mode/SHA/만료/ACL·신뢰자료/계정/동작승인/receipt/재요청이각각거부되고**인가전보안변경·소비·Task호출0**인지확인해야합니다. 실제Batch4·Session0·HANDLE·Job90초·child/breakaway·실패정리는이후별도실험범위입니다. 기존52/17/Windows7을전수반복하지않습니다.

### 새 이미지가 필요한 순서와 해시 순환 방지

Supervisor 소스를바꾸면 **Supervisor 새 NativeAOT 빌드·봉인·경고/PE/V3확인·사용자이미지승인이필수**입니다. 기존2개이미지는그대로보존합니다. NativeBinding/Probe소스가그대로면Probe는기존SHA를재사용하는것이최소범위이며, binding경로/SID/Probe변경시Probe도별도빌드승인이필요합니다.

새TrialId는빌드전에독립적으로정하지만, **자기SupervisorSHA나그SHA가들어간최종ContractSHA를소스에상수로박지않습니다**(재빌드마다바뀌는순환발생). 새Supervisor빌드→SHA봉인→두SHA를포함한새TrialContract→ContractSHA→새동작승인/신뢰자료순서로결합합니다. 보호된고정경로·정확TrialId·기존검증기로자기이미지와외부계약을검증하며해시검사를생략하지않습니다. 기존계약/소비/승인을새이미지에전용하지않습니다.

## 4. 관리자 GUI·명령과 정확한 승인 묶음

### 대표 GUI 확인 — 조회만

- V3의기존진단/처리·새이미지상태확인(정상판정없으면NOT_VERIFIED). 검사/예외/복원/삭제/업데이트설정누르지않음.
- 계정/그룹상태·비공유자격증명보관·다른Task/서비스/수동RunAs/다른자동화사용여부를대표관리이력과조회화면으로확인. 암호표시/내보내기/변경금지. PasswordRequired=false로빈암호를단정하지않음.
- 제공되는로컬보안정책GUI에서직접SeBatchLogonRight와Deny충돌여부를**조회만**. GUI미지원/권한부족이면UNKNOWN, 도구설치/권리부여하지않음. 현재등록코드는직접Batch권리를요구하며자동부여를허용하지않음.
- Task/서비스정의·보호자산소유/상속을조회. 아직없는폴더/Task를만들거나보안설정을적용하지않음. GUI조회완료는실제등록/시작/정리승인이아님.

필요명령의**실행불가초안**은 Evidence/DiscordV2DeployGatePlan20261031/Commands_PREPARED_ONLY.ps1.txt입니다. 첫줄throw가있고정적구문만검토했습니다. 읽기명령·SDDL메모리예시·향후정확CLI형식을제시하지만적용용배포도구가완성됐다고주장하지않습니다. 특히원자적보호폴더/CREATE_NEW복제도구는추가승인후구현/검증해야합니다. 이번어느명령도실제배포/실행하지않았습니다.

| 별도 승인 | 정확히 허용받을 범위 / 포함하지 않는 것 |
|---|---|
| A. 코드·정적검증 | 위one-trial gate/인가전Hardening재배치/cleanup컷오프보완, 보호생성/복제준비도구의구현·정적/모의delta만. 실gate활성화/Native빌드/실배포/Task는불포함. |
| B. 새빌드·이미지 | 변경된Supervisor의격리NativeAOT최대1회·SHA/크기/PE/경고/V3확인. Probe/binding변경없으면Probe재빌드0. 기존이미지보존·신규이미지별승인. 실행불포함. |
| C. 보호배포·자료 | 정확새전용경로/누락중간폴더·owner/DACL·신규이미지/계약/동작승인/신뢰자료배치·readback·새관리증거만. 기존부모/OnBoot/증거소비/계정/암호변경불포함. 대표가확인하지않은가정을Approved로꾸미지않음. |
| D1. 등록 | 정확Task/계정/Action·disabled/trigger0/LeastPrivilege·Password/IgnoreNew/PT90S/정확SDDL/readback만. Password logon 등록의Windows Task Scheduler 자격증명처리를승인범위에명시. 필요한기존암호는등록콘솔비에코입력만, 인수/스크립트/로그/채팅기록금지·암호변경없음. 시작권한없음. |
| D2. 시작 | 정확새TrialId·ContractSHA·이미지/instance·90초/1+1/noIPC/noAI·새Start승인/영구소비·실패시자기Job안전정리에한정한1회. 업무/추가시도없음. |
| D3. 관리자 정리 | 정확자기Task/instance·idle/drain증거·새Stop/Remove승인별로결합. 기존소비/lease/실패증거삭제나운영Task정리불포함. |

cutoff는 **2026-11-01 23:59:59KST**, 계정만료는 **11-02 00:00KST** 그대로입니다. 준비·검증·실행·정리시간을기한내확보하지못하면NO_GO입니다. 11-01이후기존승인재사용/소비초기화/자동계정·승인연장0, 새로운정책지시없이기한을넘겨진행하지않습니다.

이번현재상태는 **A~D 모두 미승인/미적용**이며대표의이번지시는준비·정적검토만입니다. 빌드/배포/Task/계정ACLOnBoot/V3·Session0IPCAI/UnitySave·Push/main병합0으로보고후중지합니다. 소스변경0·이미지보존PASS, LOCAL_COMMITTED / REMOTE_NOT_PUSHED입니다.
