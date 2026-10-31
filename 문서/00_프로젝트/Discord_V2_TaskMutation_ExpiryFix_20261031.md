# Discord V2 보안 회귀 표적 수정 — 변경 직전 승인 재검증

**TARGETED_REGRESSION_FIXED / DELTA_34_PASS / NATIVE_BUILD_APPROVAL_PENDING / NO_GO / MANUAL / EXECUTION_HOLD.** 이번에는 코드 수정·격리 검증만 했습니다. NativeAOT 신규 발행0, Probe/Supervisor/NativeMain 실행0, 실제 Windows 변경 호출0입니다.

- 브랜치 `codex/discord-v2-restricted-gate-impl-20261031`.
- 시작 `8744763b133629a8ad3b2d1c746e7315d254cb76`, 이전 구현4323359e·실패보고398aa711/Issue66 6539490315 확인.
- 실제 수정 commit **`1717e443c9dfe89f2e82e7897d657dc99c7a0fa7`**. 보고/증거·영수증은 후속 commit입니다.

## 결함과 최소 수정

기존에는 관리자 승인 검사 뒤 등록용 암호 입력을 대기하고, 그 동안 만료돼도 실제 COM 등록 직전에는 취소만 검사했습니다. 이전 별도 회귀는 rejected=False/모의등록1회로 FAIL이었습니다.

`TaskSchedulerNative.Register`에 **암호 입력 종료 직후**와 **실제 COM 등록 호출 직전** 두 번의 검증을 연결했습니다. 비밀값을 검증기에 전달하거나 로그로 만들지 않습니다. 암호의 기존 SecureString/BSTR 수명·finally의 ZeroFreeBSTR는 유지했습니다.

새 `TaskMutationBoundary`는 최초 진입의 승인 bool을 재사용하지 않고 다음을 다시 읽습니다.

1. 보호된 원문 계약의 정확한SHA·strict JSON·고정TrialId/profile/SID/경로·scope·현재 만료/cutoff.
2. 최초 검증한 동작승인 원문의SHA와 동일한 새 읽기·정확mode/ApprovalId/ContractSHA·승인만료. 입력 대기 중 새 승인으로 조용히 교체하지 않습니다.
3. 보호된 신뢰진술/basis·해시·15분 freshness·두 이미지SHA·최종경로/소유/DACL·현재관리자/비위장/상승token·계정/직접Batch권리.
4. 현재Task의 부재(등록), 기존 등록receipt·정의fingerprint·disabled/idle·start-intent/정확instance·cleanup증거(기타동작).
5. 고비용 재조회 뒤 **현재시각을 다시 취해** 만료/신뢰 freshness·취소를 마지막으로 확인합니다. 이 검증 실패 뒤 실제 변경 호출로 진행하지 않습니다.

`AdminController.ExecuteNative`가 초기 승인/계약 원문의 불변 snapshot과 취소token을 묶어 경계를 전달하며, 실제 AdminWindows와 모의backend가 같은 검증기를 사용합니다. TaskSchedulerNative의 기본 생성자는 조회만 가능하고 변경용 경계가 없으면 거부합니다. 환경변수/강제옵션/새무조건true는 없습니다.

## Enable·Run·Stop·Remove 표적 점검

각 실제 COM 변경 바로 앞에도 같은 검증을 연결했습니다. Run은 호출 시도 자체를 소비하여 응답 유실이 재호출 권한이 되지 않습니다. Stop은 정확한 기록instance와 현재instance를 다시 맞추고, Remove는 정확한 자기Task·disabled/idle·시작 이력이 있으면 정리증거를 다시 요구합니다. 다른Task를 채택/삭제하지 않습니다.

예외는 **이미 유효한 Start 승인으로 자기Task를Enable하려던 경우의 안전 재비활성화**뿐입니다. 시간 만료·취소·신뢰자료 유실이 이 finally 정리를 막지 않도록 하되, 앞서 확보한 시작lease와 정확한자기Task fingerprint/등록receipt를 요구합니다. lease가 없거나 Task가 달라졌으면 새 변경권한으로 쓰지 못합니다. Supervisor의 기존 자기Job 종료·JobMonitor/child 제한 코드는 변경하지 않았습니다.

등록 검증 실패는 등록호출0·기존Task변경0입니다. 유효한Enable 후 다음Run 검증이 실패한 경우에는 새Run을0회로 막고 **승인된 자기Task의재비활성화**를 수행하는 차이를 명시합니다. HOLD/intent/소비/실패증거는 CREATE_NEW로 보존하고 초기화·재사용하지 않습니다.

## 검증 결과

| 검증 | 결과 |
|---|---|
| 기존 FAIL1 표적 재검증 | **PASS1**, rejected=True / registration_calls=0 |
| 1차 수정범위Delta(위1건포함) | **31 PASS / 0 FAIL** |
| 재조회중만료·두검사사이신뢰변조·마지막재조회중취소 | **3 PASS / 0 FAIL** |
| 표적 합계 | **34 PASS / 0 FAIL**, 잔여확인된회귀FAIL0 |
| 최종 관리형 DLL 컴파일 | **경고0 / 오류0** |
| 실제 Windows 변경·암호입력·NativeMain | **각0회** |
| NativeAOT 신규 발행·새 Supervisor SHA | **0회 / 없음** |

암호 입력지연/계약·동작승인 만료/cutoff/취소·잘못된계약/TrialId·신뢰자료변경/유실·이미지/계정/관리자권한거부·대기중다른Task출현·중복승인/원래intent보존을 검증했습니다. Enable/Run/Stop/Remove의 변경 직전 만료·정의/instance변경, Run응답유실·취소·증거유실 후 안전재비활성화, 새정리승인의비활성계정처리도 모의로 확인했습니다.

기존58PASS 전체는 재실행하지 않았습니다. 이전FAIL1은 이번31건에포함하므로별도로더하지않습니다. 최초FAIL로그와 이전보고는 그대로보존합니다. 모의시각만 전진시켰고 OS시계·암호·Task는 사용/변경하지 않았습니다. 실제코드의 COM 호출 지점도 검증호출뒤에 있음을 정적으로 대조했습니다.

격리 E루트는 `E:/LimitlessBuildDependencies/NativeAOT20261028/TaskMutationFix20261031-f9f766cd26874a8390b1b9ca56d53583`입니다. UseAppHost=false/NativePublish=false/PublishAot=false의 테스트DLL만 dotnet으로 실행했습니다. 새3건 추가 후에는 기존31건을 다시 돌리지 않고 새3건만검증했습니다. SDK10.0.403·기존offline feed/설치reference만사용, 새다운로드/설치0입니다.

## 유지한 HOLD와 한계

전역 **ReleaseAdmissionImplemented=false**와 실제진입/배포의명시HOLD 가드를 유지했습니다. 기존차단태그는호환보존했습니다. 모든Delta가PASS여도실행승인·실Windows COM/권한/보호배포·Batch4/Session0/HANDLE/Job90초의실측완료는아닙니다. 검사 실패시실제호출을하지않도록한수정이며 이미시작된COM서버동작의완료시각을원자취소/rollback하는보장은하지않습니다.

기존 Probe SHA `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322`/1,892,352bytes와 Supervisor SHA `CCA8202417DD4D3A1E4F0D301143E9960EEFC3B984EAC79FC92DC6FA08974ECD`/7,536,640bytes는 보존PASS입니다. Probe/NativeInterop·기존Job코드·실물변경0입니다. 새Native이미지가없어새Native경고/PE/V3결과는NOT_VERIFIED, 기존새이미지V3공식정상판정NOT_VERIFIED·전역SIDUNKNOWN유지입니다.

## 다음 승인 요청과 중지

**새 Supervisor NativeAOT 단발 빌드 승인 요청 상태**입니다. 현재승인은코드/격리검증까지만이므로빌드를진행하지않습니다. 승인후에도격리발행최대1회·경고/PE/SHA·V3관측·Probe보존확인까지와실제보호배포/게이트개방/Task등록·시작·정리는별도입니다.

cutoff **2026-11-01 23:59:59KST** 유지, 기존승인/소비재사용·초기화·계정/승인자동연장0입니다. 계정/암호/ACL/OnBoot·V3예외/종료·Task/Session0/IPC·AI/Discord업무·UnitySave·Push/main병합 변경0으로 보고후중지합니다. CODEX_LATEST/CURRENT_STATUS와Issue66을갱신하며 LOCAL_COMMITTED / REMOTE_NOT_PUSHED입니다.

보고/증거commit `98b8f11114aec1e7636b3b19f87b69d45b392ead`, [Issue66 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6539632620) 실제등록·전체본문재조회일치 확인. 후속영수증은문서만변경하고 원격에는정제댓글만게시했습니다. 새NativeAOT·실행은별도승인대기입니다.
