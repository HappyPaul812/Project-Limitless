# Discord V2 NativeAOT 2종 확보 후 FAKE_ONLY 최종 사전확인

**판정: 이미지 보존2종 PASS / DEPLOYMENT_NOT_READY / TRUST_NOT_VERIFIED / NO_GO / MANUAL / EXECUTION_HOLD / gate=false.** 이번에는 읽기·정적 확인만 했습니다. 추가 빌드·EXE 실행·이미지 수정/이동·Task 등록/시작/정리·계정/ACL/OnBoot 변경0입니다.

기준: GitHub Issue66 최신보고6538790069, 보고commit `233a64316b37080aa60357068e37e798b7cb788e`. 실제 시작은 후속 영수증 `f5f8ea001dfa60dca2d9b52294c54e9598a2dafa`이며 `codex/discord-v2-fakeonly-final-preflight-20261031`로 분리했습니다. 구현fe5b22e6/기존 FAKE_ONLY 계약/소스는 변경하지 않았습니다.

## 1. 이미지 무결성 보존과 보호 배포 조건

2026-10-31 **13:20:28 KST** 읽기 확인:

| 이미지 | 보존 | SHA256 | 크기/PE |
|---|---|---|---|
| Probe | **PASS** | `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322` | **1,892,352bytes**, PE32+ AMD64(0x8664), CLR directory없음 |
| Supervisor | **PASS** | `CCA8202417DD4D3A1E4F0D301143E9960EEFC3B984EAC79FC92DC6FA08974ECD` | **7,536,640bytes**, PE32+ AMD64(0x8664), CLR directory없음 |

실물은 기존 E NativeResume20261031-7ed1a2ca6f724242a49884cba87f5360 루트의 `artifacts/ProbeAdditional20261031/Probe.exe`, `artifacts/Supervisor/Supervisor.exe`입니다. 실행하거나 바이트·위치를 바꾸지 않았습니다. 기존 NativeAOT warning/IL2091=0은 보존 근거이며 이번 재빌드/시험은0입니다.

**현재 배포 상태:** native binding의 정확한 CodeRoot·EvidenceRoot와 고정 Task는 읽기 확인상 모두 존재하지 않습니다. 두 빌드 파일은 계약 leaf 검사에서 허용하는 Administrators그룹/SYSTEM 소유가 아니고 DACL 상속 차단도 아닙니다. 이 사실은 파일 변조 판정이 아니라 **빌드 출력과 보호 배포의 차이**입니다. 현재 빌드 폴더를 실행 위치로 쓰면 안 됩니다.

향후 별도 배포 승인에 묶을 조건(이번에는 적용0):

1. 원본 이미지는 보존하고, 승인된 보호 복제본만 **Native.CodeRoot/Supervisor.exe·Probe.exe**에 둡니다. SHA/크기를 위 값과 재대조합니다. 정확한 비공개 절대 경로/계정/Task는 로컬 `GUI_Targets_PRIVATE.LOCAL.json`에 있습니다. 임의 폴더로 경로를 바꾸거나 우회 링크를 만들지 않습니다.
2. 자산 leaf 소유자는 **SYSTEM 또는 Administrators그룹**, 보호 DACL·상속 차단이 필요합니다. SID1010/다른 비신뢰 주체는 코드/계약/승인/신뢰자료의 읽기·실행만 허용하며 쓰기·교체·삭제·WriteDac·WriteOwner를 허용하지 않습니다. 단순히 실행한 관리자의 개인SID 소유라는 이유로 계약 PASS가 되지 않습니다.
3. EvidenceRoot는 별도 보호 디렉터리로 새 증거파일 생성만 허용하는 계약(mask0x1200ab), 기존 기록 변경/삭제/ACL/소유자 변경 금지·OWNER RIGHTS 제한을 유지합니다. 관리자 보호 증표와 워커 증거를 구분합니다. 코드 읽기 mask0x1200a9, 소비/lease·새 기록의 CREATE_NEW 및 영구 보존 계약을 임의 완화하지 않습니다.
4. 모든 상위 경로의 소유/교체 권한·최종경로·reparse/연결 여부도 검사해야 합니다. 소스는 상위에만 TrustedInstaller를 허용합니다. 파일/HANDLE pin·해시·비상속·실제 제한token AccessCheck와 외부 위험HANDLE 검사는 **실제 승인된 실행 환경**에서 통과해야 하며, 이번 Get-Acl 메타데이터만으로 대체하지 않습니다.
5. 새 TrialId·두 SHA·정확 SID/Task/보호루트·FAKE_ONLY/noIPC/noAI/Supervisor1/Probe1/Job90초·유효기간에 결합된 계약/신뢰자료/동작별 승인이 필요합니다. 이번에는 승인파일·신뢰진술·receipt/소비기록을 생성/서명/초기화하지 않았습니다.

## 2. V3 기록·실행 신뢰의 사실과 UNKNOWN

| 확인 항목 | 현재 근거/한계 |
|---|---|
| 이전 진단 | 기존 로컬 공식UI 사용자 전사: **Trojan/Win.Overlord.C5916070 /10-28 18:00:50KST /실시간삭제완료 /처리코드1008 /cloud미확정**. 원본CSV가 아닌 전사이며 삭제 샘플 SHA는미확보입니다. |
| 격리 기록 | quarantine.sdb 메타데이터12,288bytes·10-28 18:00:50.717KST 갱신 확인. 독자 DB의 사건/파일 매핑을 새로 해독하지 않았고 payload/복원 샘플을 열지 않았습니다. 갱신시각만으로 특정 샘플을 인증하지 않습니다. |
| 공식회신/설치엔진 | 바울 대표 제공 #745042의 제출Probe 테스트프로그램·진단제외 결정 보존. 설치 AscVersion=**2026.10.31.01**. 두 새이미지의 공식정상/미탐지 판정은 **NOT_VERIFIED**입니다. |
| 새 이미지 | 현재까지 SHA/존재 유지. 이를 백신 정상판정/Windows 실행신뢰 인증으로 부르지 않습니다. 새 검사·cloud요청·전송·예외·복원0입니다. |
| 계정 | binding의SID 끝1010을 직접 조회: Enabled=true, **만료2026-11-02 00:00KST**(UTC11-01 15:00). 로컬 Administrators그룹 직접 멤버 아님. 실제 Batch token 권한/그룹 검증은아닙니다. |
| 다른 사용 경로 | 정확SID와 일반 로컬 계정명 형식으로 조회한 Task Principal 일치0, 서비스 StartName 일치0, 고정Task 부재. 그룹 기반 실행·다른 사용자명 형식/런처/로그인/예약외자동화/관측불가 프로세스까지 배제한 전수증명은아닙니다. |
| 암호 메타데이터 | PasswordRequired=false, UserMayChangePassword=true. **실제 빈암호/약한암호/공유·유출을 뜻한다고 단정하지 않습니다.** 암호값/저장자격증명을 읽지 않았습니다. 실제 보관/공유·다른프로그램 사용은 대표의 직접 확인이 필요합니다. |
| 보호자산 관리 | 현재 보호루트 미생성·빌드파일 ACL은배포조건미충족. 누가 자산/자격증명에 접근하는지, 관리자가 실제 통제·검토하는지는 자동 인증할수없습니다. |
| Windows 전체 동일SID 부재 | **UNKNOWN**. PID4/PPL 예외를 만들지 않았고 전역token/프로세스 전수관측을 시도하거나 입증했다고 하지 않습니다. |

정확 Task/계정명/전체SID/ACL 원문·기타 로컬 메타데이터는 ignored `Temp/NativeBatchPreparation20261028/FinalPreflight20261031`에만 보관합니다. 공유 보고에는 최소 항목만 남깁니다. 기존 FAKE_ONLY 한정 위험 수용 승인은 보존하지만 위 사실 확인/새 trial 신뢰 근거를 대신하지 않습니다.

## 3. 바울 대표의 직접 GUI 확인 — 읽기만, 실행 승인이 아님

1. **V3 기록 화면:** 검사/치료·격리·클라우드 기록에서 기존10/28 진단·처리와 새 두 SHA에 해당하는 기록/상태를 확인합니다. 정상판정이 없으면 NOT_VERIFIED 유지. 검사시작/복원/제외/삭제/업데이트 설정은 누르지 않습니다.
2. **계정/자격증명 관리:** 계정관리GUI에서 로컬 대상계정·그룹·현재 상태를 확인하되 설정/암호를 바꾸지 않습니다. 암호를 다른 사람/프로그램과 공유했는지, 별도 실행기·수동 RunAs·서비스·자동업무에 사용했는지는 대표의 직접 관리 이력으로 확인합니다. Credential Manager 등에서 비밀 표시·암호전달/내보내기는 하지 않습니다. GUI 조회만으로 모든 자격증명 사용 부재를 입증하지 않습니다.
3. **Task/서비스·보호자산:** 기존 정의의 계정/Action/다른 사용 경로와 새 보호루트 미생성 상태를 확인합니다. 정확 대상은 비공개 GUI_Targets 파일을 참고합니다. Task 생성/실행/종료/삭제·서비스 동작·보안탭 적용/소유자 변경·복사배포를 하지 않습니다.

대표의 답변은 “확인된 사실/확인 불가”로 남기며 암호값을 요구하지 않습니다. 위 GUI 확인은 **Task 등록·시작·정리나 실제 EXE 실행 승인과 별개**입니다. Codex가 이번에 GUI를 열거나 화면포커스를 바꾸지는 않았습니다.

## 4. 실제 미검증 항목과 GO/NO_GO

| 단계 | 승인·기술 게이트 |
|---|---|
| 현재 | **NO_GO**: gate=false, 보호배포/신뢰자료/새 trial 동작승인 없음. 이미지보존PASS가 실행GO가아닙니다. |
| 실행 봉인 | Program.cs의false는 **관리자 모드와 --supervise 모두 진입 차단**합니다. 이번 두 이미지 그대로를 실행하면 승인만으로 봉인이 풀리지 않습니다. 향후 봉인 처리/검증 경로는 **별도 코드·이미지 변경 승인** 대상으로 남기며, 이번에는 빌드/패치/게이트개방을 하지 않습니다. SHA가 바뀌면 현재 이미지 승인을 전용할수없습니다. |
| 준비 GO 조건 | 관리 사실/현재 V3상태·잔여UNKNOWN을 사실대로 기록, 정확 보호 배포·새 trial 신뢰자료(관측15분이내·계약내만료), 새 승인/소비경로·계약결합을 검증. 전역UNKNOWN은 허용된전용프로필에서그대로기록하며 PASS로 바꾸지 않습니다. 새 탐지·증거불명·변조·계보불일치는즉시NO_GO입니다. |
| 등록 승인 | 정확Task·계정·Action·LeastPrivilege/Password·trigger0·disabled·IgnoreNew·PT90S 및 DACL/readback/정리계약에 대해 **등록만 별도승인**. 현재 등록미실시이며 운영Task/OnBoot를 전용하지 않습니다. |
| 단발 시작 승인 | 별도Start 승인·새 receipt/intent·일회 소비·정확 instance를 결합. 실제 **Batch logon4/Session0/비관리·위험privilege부재/HANDLE지원·Job생성시편입/active1/child·breakaway 제한/90초/취소·실패정리**는미검증이며 승인된실측대상입니다. 기존모의52/17·관리자fixture7을대체근거로쓰지않습니다. |
| 정리 승인 | 별도Cleanup 승인·정확자기Task/instance·idle/Job drain·종료증거·승인결합을 확인. 승인소비·lease·실패증거는영구보존하며 reset/일괄삭제/다른작업종료금지. 자동정리시험도이번승인밖입니다. |

코드 cutoff는 **2026-11-01 23:59:59KST**(UTC14:59:59), 계정 만료는 그다음 **11-02 00:00KST**입니다. 계정 만료가1초늦다는이유로승인시각을넘기지않습니다. 시작/종료/정리예산·계약유효기간을확보할수없으면NO_GO, 11-01이후기존승인/소비재사용·자동연장0입니다. 90초는단조deadline/기존계약의한도이며OS정지·APIblock까지절대wall-clock보장을증명한것은아닙니다.

**실제 실행은 새 별도 승인 대상**이며 이번 사전확인으로 등록/시작/정리 권한을 부여하지 않습니다. 계정/ACL/OnBoot/V3·IPC/AI/Discord업무/UnitySave·Push/main병합 변경0으로 중지합니다. 보고/증거는 LOCAL_COMMITTED / REMOTE_NOT_PUSHED입니다.
