# Issue107 Main04 승인 여행자 조건부 적용 검증 — 2026-11-17

**USER_ART_APPROVED / FILE_TECH_PASS / FUNCTIONAL_DELTA_PASS / VISUAL_DELTA_FAIL / LOCAL_ORIGINAL_APPLY_HOLD / NOT_APPLIED / REMOTE_NOT_PUSHED**.

## 지시와 적용 게이트

바울 대표 최종 미술승인 및 [기획 승인·발주](https://github.com/HappyPaul812/Project-Limitless/issues/107#issuecomment-6936953484)를 확인했습니다. 기존 오프라인 검수29d32499를 재사용하고 실제 ZIP SHA/CRC 및 PNG SHA를 재확인했습니다. **최소 Delta QA PASS 때만 원본 적용** 조건 때문에 화면 검증에서 남은 실패를 무시하지 않고 원본 반영을 보류했습니다. 미술 승인은 유효하며 파일 불량 판정이 아닙니다.

## 파일·후보

- ZIP: F:/Downloads/Limitless_Main04_Traveler.zip / SHA256 `286fe97b6f68f44f45d22a6a4940b9e5b84fc09d8b53d7d9333211e3bc4311a3` / CRC PASS. 기존 Manifest7·SUMS8 PASS 재사용.
- 신규 PNG 512×512 투명RGBA / SHA256 `2687c1c62e5bd20e6e0fb16c3428225aa28fc156c80a5c08b6af2350d9e9715d`. 원화 픽셀 변경·재생성0.
- 후보 Resources: `VillageNpcSprites/Eldiran/Main04_WoundedTraveler_Approved`. 기존 OGA07 Asset은 보존, MainQuest04FieldFlow.CreateWoundedTraveler의 참조 1줄과 한국어 설명만 교체하는 3파일 후보(PNG/meta/C#)를 격리에서 준비했습니다. **원본에는 3파일 모두 미적용**, 제품 commit 없음.
- 최종 후보 GUID `f325180de4944364a0dd74c1a814d6df` / **PPU360 / Pivot(.5,.08203125) = (256,42)px**. alpha bbox(106,122,406,470)의 하단을 원점에 맞춰 접지오차0, 가시높이348/360=0.966667WU. 미엘 실제 Idle00의 불투명 몸체 높이143/100=1.43WU 대비 약67.6%인 앉은 높이입니다. 미엘 원본에는 저알파 셀 경계 잡음이 있어 전체209셀 bounds를 인물 키로 오인하지 않았습니다.
- 위치(8.15,5.15), scale1, tint(.82,.82,.82,1), sorting7, **StoryLabel Y1.15 유지**. 처음 PPU320/Y1.37 후보는 이웃 미엘 얼굴에 더 겹쳐 폐기했습니다. 최종 후보는 PPU360으로 본인 머리와 레이블 보수적 하단 사이0.0633WU 확보; 이웃 얼굴/상단 HUD 문제는 별도 아래 FAIL.

## 검증 결과

기존 `Temp/Art95_91Unity20261111/QA/Client`만 재사용·Unity6000.6.5f1 숨김 Batch·포커스조작0·신규 대형복제0. 합성 Save/설정은 Evidence/또는 Layout/ 하위 QA 전용 경로입니다. 최종 후보는 Import·컴파일 및 아래 자동 검사 PASS이며 **시각 게이트는 FAIL**입니다. 자동 검사 성공을 전체 QA PASS로 표현하지 않습니다.

- 기능 Delta **44 PASS /0 FAIL**, 런타임 Console 오류0: Main04 자동시작·MeetingTrigger·실제 E 미엘 첫 대화·Story Encounter/Battle 진입·도망복귀 미진행/여행자 재생성·재도전·승리결과 처리·복귀 후 표시·전투후 대화완료·완료후 숨김·Scene 재진입후 숨김.
- 최소 전투 확인 방식: 실제 Battle Scene에 진입한 뒤 도망은 기존 ReturnToField(false), 승리는 **QA에서 EndBattle(true) 결과 처리기를 호출**했습니다. 전체 3대3 플레이 승리를 수행한 것은 아니며 전투 밸런스·전체 전투 회귀 PASS 주장이 아닙니다. 기능44검사는 PPU320 중간 후보, 이후 Quest/전투 코드 불변·최종 PPU360/기존LabelY1.15 변경만 별도 화면/Import 재검증했습니다.
- 최종 레이아웃/Import 자동 검사 **16 PASS**, 960×720/1280×720/1680×720 각각 기존 OGA07·후보 HUD ON·HUD만 숨긴 진단용 렌더 총9장. 본인 Sprite 규격/PPU/Pivot/이름표Y/접지/머리간격 확인. 최종 런타임오류0. 기존 격리 UnityEditor.Search 초기화 예외는 EnvironmentErrors로 별도 기록했으며 전체 Editor Console0 주장은 하지 않습니다.
- **시각 FAIL/HOLD:** 정상 HUD ON에서 우측 상단 현재목표 패널·F10 설정 영역이 미엘 얼굴과 여행자 이름표를 덮습니다. 최초 목표갱신 토스트가 사라진 뒤에도3해상도 재현. HUD를 숨긴 진단 렌더에서는 여행자 레이블이 미엘 하단 얼굴/머리 영역에 걸치므로 공용HUD만 숨겨도 완전 해결이 아닙니다. 여행자 본인의 얼굴·바닥 접지는 읽히지만 요청한 인접 인물/이름표/안내까지의 안전한 통합 PASS는 불가합니다.
- 기존 OGA07+원래LabelY1.15의 동일 조건에서도 HUD가림 재현. 원본과 QA의 Field_01.unity/CameraFollow/WorldBounds2D SHA가 모두 일치함을 확인하여 다른 QA Scene 탓으로 단정하지 않았습니다. 1280 camera=(1.61,2.50),size5, 최종 여행자 머리는 screenY약620(상단100px),label은screenY약633.6(상단86.4px)에 있어 HUD와 겹칩니다.
- 캡처: `Temp/Issue107Apply20261117/FinalLayout/`의 Baseline_OriginalOGA07/Candidate_HUDOn/DiagnosticOnly_OverlayHidden. **DiagnosticOnly는 실제 플레이 PASS 화면이 아니며 게임에서 HUD를 숨기지 않았습니다.** 비교판 `Temp/Issue107Apply20261117/Main04_HOLD_Baseline_Candidate_Diagnostic.png`.

## 원본 보호·실제 적용

원본 `Unity/Client` 파일 변경0·Import0·신규 Asset0·Main04 여전히 legacy OGA07 참조. Main07 부상자/승인 NPC12/미엘·태온/Scene/Quest/Save/UserData/Discord/기존dirty 보호. 원본 보호8350 종료 SHA는 최종 receipt에서 보고문서 외 변경0을 검증합니다. Unity 원본 MCP 읽기종료: Bootstrap/PlayFalse/dirtyFalse/compileFalse/선택null/포커스null/Console오류0·경고0. #106 공용이름표1.37·E코드 유지. Issue85 패키지/GUID HOLD 유지·수정0. root main HEAD `52f84e643d5d860665eadfe1614a30c745209f00` 유지. reset/clean/stash/Push/Merge0.

제품 후보는 `Temp/Issue107Apply20261117/Candidate/`와 기존 격리 QA에만 보존합니다. 소형 Git worktree의 후보3파일은 이번 작업 생성분만 회수했으며 문서3개만 commit합니다. 후보 GUID가 원본 Asset에 등록됐다고 주장하지 않습니다.

## 다음 단계·확인 위치

**적용 HOLD 해제에 필요한 다음 작업:** Main04 장면에 한정한 레이블/안내와 화면 상단 겹침 해소 범위를 정한 뒤 최소 수정·동일3해상도 재검증. 전역 HUD/Camera/배치/다른 NPC를 임의 변경하지 않았습니다. 본 미술 승인을 재요청하지 않습니다. 화면 게이트 PASS 후 검증3파일만 원본 반영하고 원본 표적Import/compile/Console을 확인해야 합니다.

확인 위치: Field_01 Main04 진행 중 미엘 옆(8.15,5.15). **현재 원본은 기존 OGA07 그대로**이며 신규 Sprite는 QA 증거에서만 확인 가능합니다. 브랜치 `codex/main04-traveler-approved-20261117`, 로컬 보고 commit SHA는 Issue107 댓글·Final.json에 기록. CODEX_LATEST/CURRENT_STATUS 갱신. GitHub 댓글만 실제 작성하고 문서/Asset Push0.
