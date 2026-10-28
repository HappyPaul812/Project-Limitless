# Discord V2 안전 정리·게임 개발 복귀 인계

작성: Codex / 2026-10-28 KST. 기준: GitHub Issue #66·최신보고6467004063, branch `codex/discord-usage-diagnostics-20261026`, 구현 commit `fe5b22e60a3c505961cead1b7b80089194f0a44a`. 이번 문서 최종 commit은 완료보고/Issue66에 기록합니다.

**최상위 일정 정책:** 바울 대표의 이번 지시로 2026-11-01은 무조건적인 Discord 전체 기능 완성 기한이 아닙니다. 그때까지 안랩 회신 없이 안전하게 가능한 기존 코드 정적 검토와 미완료 문서 정리만 마무리합니다. **2026-11-02부터 게임 개발로 복귀하며 안랩 회신 지연 때문에 복귀를 미루지 않습니다.** 과거 보고의 전체완성 마감 표현은 당시 이력이고 이 정책이 우선합니다. 기능 전체 성공·종료를 선언하거나 기다림/자동재시도/감시 작업을 만들지 않습니다.

## 완료된 구현·근거 — 재작업하지 않음

| 항목 | 보존할 사실 | 실제 완료로 확대하면 안 되는 부분 |
|---|---|---|
| Supervisor 핵심/관리자 CLI | 승인·범위/만료/정확SHA/receipt, 영구단발소비·lease, 보호경로/AccessCheck/DACL/HANDLE, Job 감독/정리·정확Task 동작 소스 구현 | 실Windows 등록·시작·정리/Session0 수행 완료 아님 |
| 기존 시험 | 핵심/모의52 PASS, FAKE_ONLY 계약17 PASS, 실제관리자7 PASS 보존 | 동일한 실행환경/통합runtime 전체PASS로 합산하지 않음 |
| 독점성 계약 | 운영용 전역 조건 유지. 별도 FAKE_ONLY 전용 한정 위험 수용 승인·신뢰근거 결합 구현 | 전역 동일SID 부재 UNKNOWN, 신뢰 가정 실제확인 미완료 |
| NativeAOT 시도 | Probe1회 MSB3021/동시각 V3 실제삭제, Supervisor0회·새 정상native2개 미확보 | generated native image SHA/크기/PE 검증·실행승인 없음 |
| V3 안전성 | Trojan/Win.Overlord.C5916070, 공식UI 대상이 copy source Probe native exe와 OCR정규화후일치. 읽기source/공식공급검토는변조·예상밖악성동작근거미발견 | 악성/오진 분류 UNKNOWN, cloud미확정은안전판정아님 |
| 안랩 | 대표가 #745042에 Probe.exe 제출완료·공식회신대기 | Codex 발송/회신수신 주장0, 자동추가전송/샘플복원허가아님 |
| IL2091 | Native.Info<T>에 PublicConstructors/NonPublicConstructors 보존선언 최소수정, ABI선언무변경. 관리형변경3 PASS, Probe/Supervisor DLLcompile 경고오류0 | 정적분석 대조군도미반응, IL2091 진단소멸·NativeAOT트리밍/링크/실행 NOT_VERIFIED |

근거: [핵심/관리자 구현](Discord_V2_Supervisor_CoreAndAdmin_20261028.md), [전용계약 구현](Discord_V2_FakeOnlyContract_Implementation_20261028.md), [독점성 판정](Discord_V2_ExclusivityContract_Decision_20261028.md), [빌드 실패](Discord_V2_NativeAOT_BuildOnly_Result_20261028.md), [V3 source·공급망 검토](Discord_V2_V3_SourceSupply_Review_20261028.md), [IL2091 수정·미검증](Discord_V2_IL2091_ContractFix_20261028.md). 옛 문서의 '미제출/진단대상미확인'은 당시 상태이며 최신 대표제출/경로대조 결과와 구분합니다.

## 현재 진행 지점 / 남은 HOLD

현재는 안전하게 중단 가능한 source·문서 로컬커밋 지점입니다. 실행gate `ReleaseAdmissionImplemented=false`, NO_GO/MANUAL/HOLD 유지. 실제새시험승인·운영전환 없음.

1. 안랩 #745042 공식 악성/오진 판정과 기존클라우드 처리/제출범위의 사실 확인 대기. 복원된Probe.exe/제출Probe.zip 접근·실행·재빌드·예외는 공식판정전금지. 판정후에도 자동허용하지않음.
2. IL2091 소스 보존계약은 수정완료이나 native 분석/링크의 경고 소멸·최종이미지SHA/PE는 NOT_VERIFIED. 이전 관리형0경고를 NativeAOT해소PASS로 바꾸지않음.
3. 전용 FAKE_ONLY 실행의 비협조 동일SID 코드부재·자격증명비공유·다른실행경로미사용/보호자산의 실제 신뢰근거 미확보. 위험수용승인은이미완료이며 그승인재요청과 사실근거확보를혼동하지않음.
4. 실제 Batch4/Session0·제한된실행token의HANDLE관측지원·Job내대상1/child제한/90초·취소/강제종료·정리/Task XML·instance/승인소비통합실증 미완료. 각각별도승인필요.
5. **일정과 유효기간은 별개:** 코드의 기존 계약상한은 `2026-11-01T23:59:59+09:00` 그대로입니다. 이번 일정조정은 승인/계정만료 연장 명령이 아니므로 코드를 임의 수정하지 않았습니다. 이후 재개 시 cutoff·유한계정/승인만료와 새요청의 정합성부터 대표정책을 확인해야 하며 옛승인·소비기록 재사용/초기화 금지입니다. 현재계정/Task값은이번조회하지않았습니다.

## 보존과 복구 절차 — 이번에는 실행하지 않음

- source와 관련 commit, 빌드 실패·V3 시간/진단·기존PASS·공식공급 기록을 보존합니다. 정확 Task/계정/보호경로·명령·개별증거위치는 Git제외 로컬 `DeadlineHandoff20261028/PRESERVATION_LOCAL.md`에 별도로 기록했습니다. 비공개 원문/샘플/인증자료는 Issue/공유source ZIP에 넣지 않습니다.
- 이번 정리에서 새 OS transaction이 없으므로 되돌릴 Task/계정/ACL/서비스 변경은 없습니다. 기존 r2/OnBoot·원본Unity/Save·과거계정만료변경과 승인소비기록은 보존하며 자동cleanup·권한변경·계정갱신을 하지 않습니다.
- 운영과상관없는 source복구가필요해도 먼저 status/diff·관련commit을 확인하고 텍스트수정만새작업으로검토합니다. 이번 revert/reset/clean/일괄삭제/branch전환0. 인계자체가 복구명령 실행승인이 아닙니다.
- ignored 로컬증거는 commit에 들어있지 않습니다. worktree/Temp를삭제·archive하지 않습니다. 별도백업이나공유는샘플금지·민감정보범위를분리해승인받기전까지수행하지않습니다.
- 악성판정 또는 미확정이면 샘플을 그대로차단하고 vendor가안내한후속도먼저대표검토를받습니다. 예외/종료/복원으로우회하지않습니다. source수정으로탐지를회피하거나공식판정을대체하지않습니다.

## 후속 1~3개 우선 작업 / 담당 분리

1. **11-02 게임복귀 우선:** 안랩 회신 유무와무관하게 Discord차단상태를그대로보존하고 게임요청의정본·CURRENT_STATUS·해당게임작업branch/작업트리를확인합니다. 이번단계에서게임코드수정/branch체크아웃/자동게임실행은하지않습니다. 게임원본에Discord변경을합치거나본branch를Push하지않습니다.
2. **안랩회신기록:** 대표가회신을제공하면공식판정·해당sample/version·제한조건만기록하고잔여HOLD를갱신합니다. 회신지연은게임복귀지연사유가아닙니다. 이메일감시/회신poll/추가자료자동전송/Discord개발자동재개0.
3. **별도 재개지시가 있을 때:** 승인cutoff/만료정책·공개범위를정리→공식판정과별도빌드승인→새source봉인/NativeAOT분석·PE/SHA→실행신뢰근거·정확동작별승인→그때만새격리실행검토. 통과못한단계는HOLD로남기고이미완료된시험을반복하지않습니다.

다른 담당에게는 '게임개발 복귀를 안랩대기와 분리, Discord는운영승인없음'을 인계합니다. 이 문서 작성으로 다른 담당에게 메시지를 자동 발송하지 않았습니다.

## 후임 첫 답변 / 시작 문구

후임은 AGENTS→PROJECT_CONTEXT→CURRENT_STATUS→CODEX_LATEST→본인계→Issue #66 최신기록→git status 순서로대조합니다. 'IL2091 소스수정완료/NativeAOT NOT_VERIFIED, 안랩#745042대기, 실행gate=false, 11-02게임복귀독립'을먼저보고하고대표가요청한게임작업으로이동합니다. 이번main원격최신내용은재조회/갱신하지않았고게임정본변경도없으므로후속게임작업시해당정본을읽어야합니다.

시작 문구: “너는 Codex 담당이다. GitHub Issue #66과 문서/00_프로젝트/팀운영/인계/CODEX_LATEST.md 및 Discord_V2_SafeCloseout_Handoff_20261028.md를 확인해. Discord는 HOLD로 보존하고, 안랩 회신을 기다리느라 11월 2일 게임 개발 복귀를 지연하지 마.”

이번 수행: 기존 코드의 gate/cutoff 및 최신 기록 정적대조와 미완료문서정리만. 시험/컴파일/Publish/샘플접근·실행/계정TaskACLOnBoot/업무IPCAI/UnitySave/Push main 변경0. 원격에는관련Issue정제보고만남기며source와문서는LOCAL_COMMITTED / REMOTE_NOT_PUSHED입니다. 안전정리완료후중지합니다.