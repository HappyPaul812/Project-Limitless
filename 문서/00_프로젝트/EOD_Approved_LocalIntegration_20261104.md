# 2026-11-04 승인 아트 최종 로컬 통합 마감

**ISOLATED_INTEGRATION_COMMITTED / VISUAL_PARTIAL / LOCAL_MAIN_SYNC_HOLD_DIRTY / REMOTE_NOT_PUSHED**

통합 브랜치 `codex/integrate-20261104-approved`, 구현 커밋 `b5a41b3089ed43abd725c836d8934d27cf88ca58`.
실물 작업 공간: `F:/study/codex/Project-Limitless/Temp/EODApproved20261104/Integration`.
원본 `F:/study/codex/Project-Limitless`는 main `634a91d3e4174c87761c0d377cfa5a695ca79e7f` 그대로입니다. **원본 적용과 main 동기화는 완료되지 않았습니다.**

## 정본과 승인 경계

`docs/planning`의 PLANNING_LATEST 및 `20261104_EOD_ThreeApprovals_LocalIntegration_SafeRunbook.md`(확인 commit `1205d7933c065c51b217d0089e86850538bfdbaa`)를 우선했습니다. 최신 사용자 승인 댓글 #63/6627888182, #89/6627893169, #92/6627894117, #90/6624688949를 대조했습니다.
Runbook의 원본 dirty 시 main 갱신 중단 규칙에 따라 별도 worktree에만 통합했습니다. 기존 308 tracked dirty·550 untracked를 숨기거나 stash/reset/clean/강제 덮어쓰기하지 않았습니다.

| 대상 | 통합 브랜치에 실제 반영 | 원본 적용·남은 조건 |
|---|---|---|
| #63 | 신규 Prepare·IdleFolded·IdleSpread + 승인 Sting·Recover v3, 명시적 Sprite 참조. Idle2/공격10슬롯 연결 | 원본 미반영. 공격10은 새 그림10장이 아니라 Prepare2·Sting2·Recover6 유지. 접침점/FootLock 측정은 별도 미검증 |
| #90 | 승인 Attack8 + 기승인 유기형 Idle8/Walk8 참조, 기본 공격 시간/완료 대기 연결 | 원본 미반영. Shoot8 신규 제작·교체 HOLD. 과거 표적접촉/접지 미검증을 미술 승인만으로 PASS로 바꾸지 않음 |
| #89 | 정식 아이콘3·승인 VFX3·B안 표시 API/2행 HUD·문자·괴사2/3 KO 경고 | **표시 계층만 통합.** 원본에는 냉기 상태 생성기가 없으므로 자동 VFX 수명/동시 우선순위·상태부여·미정 피해/재적용/해제/화로 규칙은 구현 완료 아님 |
| #92 | Chapter2 catalog 신설, Field05~08 Ashen·Field09~10 Obsidian 연결 | 6개 실 Scene 이름 조회와 실제 UI 렌더 확인. 아군 두 명 겹침으로 전투 전체 가독성 VISUAL_PARTIAL; 원본 연결 HOLD |
| #91 | 미승인 Chapter3 배경은 통합하지 않음 | USER_VISUAL_REVIEW_PENDING / NOT_UNITY_APPLIED 유지 |

기존 완료 보고가 있는 이전 DocsWorktree는 main 대비 Unity 코드 변경이 없는 문서/검수 브랜치였습니다. 기존 F2 라벨/viewport 보완은 원본 사용자 dirty 코드가 승인 commit `8f94cdd0`와 LF 정규화 후 일치함을 확인하여 통합 브랜치에도 보존했습니다. 원본 파일 자체는 변경하지 않았습니다. 승인되지 않은 별도 구현을 일괄 cherry-pick하지 않았습니다. 이전 보고 Markdown 20개와 이력 색인을 선별 보존했고, 중복 대용량 과거 QA 증거는 기존 worktree에 유지했습니다.

## 승인 바이트·외부 원본 보호

승인 PNG **37/37** 원본과 통합본 SHA256 동일(독침벌5, 거미24, 냉기아이콘3/VFX3, 배경2). PNG 재저장/리터치/재생성 0. `ApprovedAssetsIntegrated.json`에 파일별 SHA/GUID/실물 출처를 기록했습니다. 새 자산에는 새 GUID, 기존 MonsterDefinition meta GUID는 유지했습니다.
F: ZIP 전체 SHA는 앞서 1회 확인한 결과를 재사용하며 이번에는 재계산하지 않았습니다.
- #63 `172df8f2d658b02f6859554284ab4b719175b2ce86b922fe5270f4ae9803b325`
- #89 `f758b59b73847f18c13e0d48aafee2f58eac33802ec597a1c739078335b2baa3`
- #92 `e439cd32bda173063ee05fd3cb3fd8a099dc4f3a6a67b805150b2777ba6b7ff0`
- #90 기존 확인값 `58c51cb49d63cde4669dc183da8583701e7e0bc9dceae9fae5542a5573f0fc9d`

PilotBee ThirdParty 원본·Resources 비교 이미지/ZIP/라이선스는 통합본의 `Archive/ExcludedPilotBee/`로 바이트 그대로 이동하여 Assets 밖에 보존했습니다. 새 독침벌 정의에 구 PilotBee sheet/GUID 참조는 없습니다. 신규5 원화만 사용합니다. 기존 CC BY 버전 충돌 조사는 재개하지 않았으며 권리 보증을 새로 선언하지 않습니다. 과거 검증 C# 클래스/비활성 비교 Scene은 역사 기록으로 유지됩니다. **원본 main에는 아직 기존 PilotBee가 남아 있습니다.** 통합본 배제를 원본 제품 배제 완료로 보고하지 않습니다.

## 실제 Unity 검증

Unity **6000.6.5f1 (3ff58d469c8a)** 직접 batchmode. 버전 변환/업그레이드 없음. 원본 Editor는 조작하지 않았습니다.
제품 Assets의 격리 사본에서 실제 `BattleSceneController`와 실제 MonsterDefinition/Resources를 사용했습니다. QA만 자동 초기화 attribute 제거·최소 기존 QA Packages/Settings 사용·사용자 데이터 미복사·합성 Save 경로 지정했습니다. 원본 26개 enabled Scene 경로는 빌드에 그대로 전달했습니다. 제품 Packages/ProjectSettings/Scene/Prefab/BuildSettings 파일 변경 0. **원본 프로젝트 자체의 출시 빌드/수동 플레이 QA와는 구분합니다.**

- 신규 연결 Delta **60 PASS / 0 FAIL**: 실제 정의/자산 로드, Chapter2 6매핑/DeepCore fallback, HUD 정식3종과 문자 폭, 독침벌10슬롯 약1초·거미8슬롯 약0.8초 재생/승인 Idle 복귀/실제 아군 HP 감소. 공격 완료 전 프레임 잘림 방지 확인. 이전 153/15/99/50/16 전수 QA를 재실행한 것이 아닙니다.
- 후속 시각 한정 **52 PASS / 0 FAIL**: 누락된 합성 player Sprite 입력과 대상 ID를 수정하여 실제 아군3 Sprite가 있는 캡처8장을 다시 확보했습니다. 모션 검사는 이 단계에서 실행하지 않았습니다. 두 실행의 공통 검사를 합산해 112개의 고유 PASS라고 주장하지 않습니다.
- 첫 실행의 공격 표적은 제어 가능한 아군 중 첫 멤버 태온입니다. 실제 피해 검증이지 주인공별 피격 검증은 아닙니다. 최종 냉기 표시는 `Id==player`인 휠체어 주인공에 주입했습니다.
- #89 합성 읽기 전용 한기2턴·동상2턴·괴사1/3,2/3 KO 경고를 실제 HP행에 전달했습니다. 상태 부여/턴 진행/KO 판정의 런타임 QA가 아닙니다. 밝은 기존 설원 카메라960/40px 두 장, 승인 Obsidian960/40px·1280/128px·1680/128px 세 장으로 계획된5조건을 포함합니다. 추가 Ashen3크기도 #92와 공유했습니다. 설원은 **기존 승인 F2 카메라 자료를 QA배경으로 재사용**했으며 #91 미승인 배경은 아닙니다.
- 40px VFX는 몸체 아래 가시 알파 간격8px, 128px는 캐릭터 뒤/24px 하향 API를 사용했습니다. 문자·턴·중첩을 아이콘과 함께 보여 색상만으로 구분하지 않습니다. 공식 아이콘은14UI 크기로 축소되므로 독립 그림만으로 3상태를 식별하는 접근성 PASS는 부여하지 않습니다.
- 새 런타임 오류0. Editor SearchDatabase 초기화 ArgumentOutOfRangeException은 환경 로그로 별도 보존했습니다. 기존 CS0618 등 컴파일 경고는 빌드 보고에 남기며 0경고로 보고하지 않습니다.

## 실제 캡처의 남은 시각 문제

아래 좌표는 해당 캡처의 화면 좌표(좌상단0,0, 우하단 제외)입니다. 아트 승인 자체를 취소하거나 새 배경을 요구하는 항목이 아닙니다.
- **960×720 [620,335,723,466]**: 태온과 휠체어 주인공이 같은 전열 열에서 겹쳐 상반신/실루엣 일부가 가려집니다. 1280에서도 [800,350,916,493] 유사합니다. 기존 전투 배치에서 관찰한 현상으로 이번 마감에 임의 편성/행간 디자인 변경을 하지 않았습니다. 따라서 실제3명 존재 PASS와 전체3명 독립 가독성 PASS는 구분합니다.
- **960×720 하단 [250,579,710,607]**: 기존 스킬 안내 문장이 메뉴 상단 테두리에 근접/겹칩니다. 전투 필드 가로 테두리도 일부 화면 밖입니다. 이 기존 UI까지 전면 수정하거나 QA 전체를 확장하지 않았습니다.
- 밝은 설원40px 한기는 몸체와 분리되나 작은 청백색 결정의 세부 형상 자체는 제한적입니다. 상태명/2턴이 HUD에 명시되어 보완합니다. 어두운 Obsidian128px는 휠체어 뒤에 가려지는 부분이 있어 효과 전체 모양 단독 가독성은 PARTIAL입니다.
- 냉기 런타임 자동 표시 호출자는 아직 연결하지 않았습니다. 미정 기획을 확정하지 않고 표시 API/승인 자산만 준비한 상태입니다.

최초 수동 YAML 연결이 배열0개로 로드된 실패는 Unity SerializedObject 저장으로 수정한 뒤 통과했습니다. 초기 QA 팝업 가림·합성 Sprite 누락/대상 선택·출력경로 초기화 오류와 재시도 기록은 Temp에 보존합니다. 오류가 난 실행을 PASS 근거로 사용하지 않았습니다. 최종 시각 자료는 `VisualEvidence/`, 모션/원래 연결 근거는 `Evidence/`를 사용합니다.

## 원본 보호와 동기화 마감

`ProtectionFinal.json`: 원본 tracked 스냅샷 **7,374개 변경0**, 기존 dirty/untracked 파일 바이트 변경0, 개인 Save/UserData 해시 **23개 변경0**, git status 집합 동일. 원본 main/HEAD 동일. Scene·Prefab·기존GUID/meta·BuildSettings·개인 저장파일을 덮어쓰지 않았습니다.
**LOCAL_MAIN_SYNC_HOLD_DIRTY**: 원본308 tracked dirty/550 untracked가 유지되므로 runbook에 따라 main fast-forward/작업트리 파일 복사 모두 하지 않았습니다. 검증 통과분은 별도 로컬 통합 커밋으로 보존했고, 시각 PARTIAL 항목까지 원본 적용 완료로 표시하지 않습니다.
원격 Push/병합0, main 직접커밋0. 오늘 작업은 이 상태로 마감하며 신규 제작/전체 QA를 시작하지 않습니다.

상세 BuildReport·승인 SHA 목록·최종 캡처·도구는 아래 Evidence 폴더를 참조합니다. 해당 로컬 커밋은 원격에 없으므로 GitHub blob 링크가 공개되어 있다고 주장하지 않습니다.

최종 상세 격리 빌드: **Succeeded / 오류0 / 경고38 / 승인PNG37개 포함 / PilotBee 외부 아트0 / exit0**. 과거 검증 클래스 C# 이름은 남아 있으나 외부 이미지·ZIP 포함과 구분했습니다. 증분 빌드의 생략된 자산목록을 배제 근거로 쓰지 않았습니다. 최종캡처 SHA는 `Evidence/EODApproved20261104/SHA256.json`에 있습니다.
