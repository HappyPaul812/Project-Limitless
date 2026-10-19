# LIMITLESS — Main10 완료·Field06/07 최종 배치·Field09 화면 QA 계약 (2026-10-19)

- 담당: 기획 (`docs/planning`), 기준: 바울 대표가 공유한 Codex 2026-10-19 최종 통합 보고와 리뉴얼 담당 Field09 감사 정본.
- 상태: `MAIN10_APPROVED_VOICE18_OFFICIAL_LOCAL_DISK_APPLIED / FIELD06_B42_CONDITIONAL_LAYOUT_AUTHORIZED / FIELD06_SPAWN_FAIL_OPEN / FIELD07_B42_CONDITIONAL_LAYOUT_AUTHORIZED / FIELD07_ASH7_1_FAIL_OPEN / FIELD08_NO_NEW_ART / FIELD09_NO_NEW_PNG_NOW_VISUAL_QA_REQUIRED / GIT_MAIN_MERGE_PENDING`.
- 이 문서는 기획 승인·검증 게이트이며 Codex의 로컬 원본/Unity/main을 기획 세션에서 직접 변경했다는 뜻은 아니다.

## A. Codex 완료 보고 최신 우선

- **Main10**: 승인 기존 PCM 복구음성 18개 StoryVoiceCatalog 정식 연결, 원본 디스크 안전 반영 완료 보고. 대사·Next·중단·무음·Continue 및 화자/PCM/GUID 관련 **164 PASS/0 FAIL**. 기존 Main09 승인27과 UserData23·보호대상 SHA 보존, 신규 TTS/API0, Push/PR/main병합0. 원본 Editor 실제 사용자 재검증은 분리(`ORIGINAL_EDITOR_USER_PLAY_NOT_VERIFIED`). 기존 Main10 입력 차단 수정 격리132PASS 역사기록 재사용; Main10 새 음성 재생성/재감사 금지.
- **Field06 v1.1**: 납품 ZIP SHA/CRC/Manifest PASS, 격리 Unity **349 PASS/0 FAIL**, Ground 49 빈틈 현상 수정 보고. 다만 **Field06→Arbel 귀환/도착이 정식 Spawn 기준 약 0.455유닛 편차**이며 검사는 **FAIL로 보존**, Scene 원본미적용. A30/B42 선택 게이트 유지.
- **Field07 v1**: ZIP 검증 PASS, 격리 주행 **324 PASS/1 FAIL**, 화면20 PASS. 불합격 원인은 `Chapter2Main16Flow.CreateAshMarks()`가 Ground Sprite를 `WindlessAsh_0..6` 재 표현에 재사용하여 생기는 **작은 불투명 사각형**. 화면20 PASS와 독립적인 실제 주행 실패는 절대 0 FAIL로 덮지 않는다. Scene 원본미적용. A30/B42 선택 게이트 유지.
- **Field08**: 왕복 Spawn/Continue 확인 및 기존 공식 PNG9 보호, Scene 변경 없음. Field08 신규 공식 리뉴얼 불필요 결론 유지.
- 각 Codex 최종 feature 커밋/QA 상세 링크는 사용자 제공 보고 및 후속 Codex 로컬 report를 정본으로 사용하며 기획에서 임의 SHA를 생성·변경하지 않는다. GitHub remote `main/origin/main`은 보고상 `675988c9f2c5c507d9a696b0378be27c0bd3309b`; 로컬 승인 브랜치와 아직 통합/Push하지 않음.

## B. Field06/07 도로 배치 — 기획 B42 조건부 선택

- **B42를 두 지역의 우선 도로 레이아웃으로 승인하여 격리 feature 내 최소 구현 및 QA를 진행**한다. 이유: 기존 정식 승인 Field05 B42의 동서진행 연속성, 서쪽 현장 조사·출입구의 시각적 연결, 21×15 서↔동 가독성. 다만 도로 위치·신규12 Renderer 등 실제 Scene 구조가 달라지는 점을 감안해 **원본 Scene에 즉시 적용/최종 원격 병합하는 무조건 승인은 아님**.
- Field06·07 각각 B42의 2행 Upper/Lower/WestEnd 대응, Sprite GUI 및 PPU/Pivot/Tint/Sorting/Seam, collider/quest marker 숨김 없는지, 입출구 진입 부근 두 끝마감 위치, 걷기/휠체어 네비게이션 경로를 해당 장면만 delta QA. 미술 스타일·원본 승인 PNG byte 수정하지 않는다. B42가 새 실패를 만들면 A30 비교안과 실제 문제가 표시된 화면을 짧게 보고해 최종 사용자 결정 보류.
- 기존 v1 Field06 717 QA·Field06 현재349·Field07 현재324 전체 반복 금지. 이전 성공 근거는 이력으로 재사용, 변경된 샘플 및 edge만 최소 검증. 완료결과는 `B42_ISOLATED_PASS` / `ORIGINAL_APPLY_PENDING` 구분.

## C. Field06 P1 Spawn 편차 0.455 조사

- `Arbel ↔ Field06` 실제 SceneTransition/Spawn resolution, PlayerController collider offset/physics, Move/teleport-after-load/transition grace, Center+pivot의 관계를 격리에서 계측. 고정 숫자0.455만 반대로 더하여 임시 덮기 금지. 현행 공식 Spawn target·Field05/06/07 일반 월드 왕복 회귀를 해치지 말 것.
- 성공기준: 기존 정식 Spawn에 의도된 위치·충돌 회피로 도착, 잘못된 지점 이동/접근불가/타 Scene 전환0; 정식 샘플별 정확한 tolerance 수치와 의도된 안전 보정 보고. Main15 12목표/세린 합류/Encounter4/Save·Continue·도보/휠체어 입출구를 변경분 경계에서만 검사. 현재 0.455 FAIL은 처리 전 OPEN.

## D. Field07 P1 Runtime Ash7 작은 사각형

- 원인 코드의 `AshSoil_` 첫 Ground를 그대로 가져오는 Runtime7 Sprite 재사용을 확인. **기존 공식 투명한 재/이펙트/Mask/alpha 있는 자산 또는 현재 기존 텍스처/material을 우선** 활용한 최소 변경을 시도. 새 리뉴얼 ZIP v1의 `WindlessAsh` 선택 PNG1은 **기존 자산만으로 해결 실패 시** 별도 비교용으로 **격리 후보 연결만 조건부 허가**하며 원본 공식 배치·선택 아트 사용자 승인으로 자동 간주하지 않는다. 신규 아트 제작/TTS/범위 확대 금지.
- Main16의 '바람 없는 재가 지면 진동과 같은 간격으로 조금 움직인다' 연출을 해치거나 화면 번쩍임·불투명 사각형/흰 틀을 만들지 않아야 함. Quest12/Path Hearing·세린 Story Actor/불씨망령 등장·전투/Save/Continue/Field07↔08/휠체어 대상 국소 delta QA.
- 이 주행 1 FAIL이 완전히 소거되었다는 실제 증빙이 있기 전 Field07 원본 Scene 적용 불가. 검증 후 `ISOLATED_ASH7_DELTA_PASS`와 실제 `USER_EDITOR_NOT_VERIFIED` 분리.

## E. Field09 기존 공식 아트 유지 + 시각 확인 한정 승인

- 리뉴얼 정본: `docs/art-renewal/문서/00_프로젝트/Field09_ObsidianScar_ExistingArt_MinimalReplacement_Audit_20261019.md`. Scene `Field_09_ObsidianScar`에 활성 SpriteRenderer9/9가 기존 Main18 공식 Resources/Main18/Environment PNG, 총 등록10(실사용9, Cracked02 미사용). 기술 `PASS_WITH_NOTE17`, **`USER_ART_REVIEW_REQUIRED17`은 미술 승인으로 승격하지 않는다**.
- **결정:** 공식 PNG10/GUID/meta/Scene/과열/Quest·몬스터/BGM/TTS 보호, 신규 PNG·ZIP 제작 **0**, 실제 화면에 문제가 있는지 **읽기 전용/격리 시각검증만 승인**. 지면 GroundBase Tiled21×15와 CrackedWest Tiled9×15 경계 x≈-1.5, 반복 Seam 및 Cliff/Fissure Edge/Glow/HeatVent/Slab/Spire 6종의 흰 가이드 형태 선/테두리를 최소 이미지/현행 3비율 샘플로 확인.
- 실제 오류가 없거나 기존 Renderer Tint/Importer로 안전 해결 가능하면 `NO_NEW_ART_REQUIRED`로 종료. 문제가 **눈에 보이고 공식 기획 가독성을 해치는 것**이 실증되면 **해당 PNG·픽셀 구간만** 별도 리뉴얼 담당에게 발주 권고. 사용자 개별 승인 전 PNG 리페인트/Scene 영구 변경/기존 공식 미술 재승인 완료 선언 금지. 기존 Main18 Runtime 500 PASS는 역사 결과만 재사용.

## F. 실행 운영

Codex Main10의 기능/음성 작업 재작업 금지. 다음 Codex는 Field06→Field07 검증 실패 원인 해소와 B42 격리안 마무리 우선, 이후 Field09 경계/흰 가이드 시각 QA는 짧게 조사; Field07/08 경계 검수는 Field07 격리 수정 완료 이후. 변경한 작업을 분리된 codex/* branch/commit 및 사용자의 dirty/Untracked/UserData 보호 SHA 기록. 사용자 원본 Editor Play/포커스 무조작. git Push/main 직접 커밋/병합 금지.
기획 문서는 docs/planning, 리뉴얼 담당은 docs/art-renewal에서 상태 업데이트. 미술실물 승인·격리 런타임 PASS·원본 disk applied·사용자 Editor QA·원격 main merged는 각각 별도 게이트.