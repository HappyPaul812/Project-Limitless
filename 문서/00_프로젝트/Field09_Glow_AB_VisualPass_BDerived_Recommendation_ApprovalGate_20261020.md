# LIMITLESS — Field09 Fissure/Glow A/B 두 후보 시각 PASS, B 파생 Glow 기술추천 및 미술 승인 게이트 (2026-10-20)

- 기획 담당 `docs/planning`; 관련 GitHub [Issue #38](https://github.com/HappyPaul812/Project-Limitless/issues/38).
- 2026-10-20 바울 대표가 전달한 Codex 격리 QA 스크린샷 기반. 이 기획 세션은 Codex 로컬 임시 GameView와 실제 PNG 파생 파일을 직접 검사하지 않았다.
- Codex branch `codex/field09-glow-composition-ab-20261020`, QA commit `68855edd`, 인계 commit `453736cb`. **원본 Editor·Save·UserData·승인 5종·기존 Glow PNG/meta/GUID 보호**, 공식 원본 Scene 적용·Push·main 병합 0.

## A/B 결과
| 검증 | A SpriteMask | B 별도 파생 Glow |
|---|---|---|
| 붉은 띠/기존 가이드 제거 | PASS | PASS |
| 내부 열기 및 조사 마커 가독성 | PASS | PASS |
| 다른 Renderer/Sorting 부작용 | 관찰되지 않음 | 관찰되지 않음 |
| 구성 | SpriteMask 객체+범위·maskInteraction 관리 | Fissure Glow 전용 새 후보 asset/참조만 교체 |
| 구/신 원본 바이너리 보호 | PASS | PASS |

- 기존 Glow의 **필요한 내부 RGB 및 Alpha 약 99.9% 보존**이라는 보고는 가시적 품질·압도적 성능 향상의 계량 근거와 다르다. 원래 Glow와의 Alpha 형상 차이가 핵심. 두 방법 모두 동일 3 GameView 화면비 시각 PASS, **A/B 변경분 검사34PASS, 공통 과열 위험 UI 스냅샷10PASS**. 성능 수치는 **미측정**; 'B가 더 빠르다'라는 과잉 주장 금지.
- **기획 기술추천 B**: Mask 객체/렌더범위·정렬 범위 관리 없이 **별도 새 Asset의 Sprite 참조 변경**이므로 유지보수·회귀 범위가 간명하다. A도 PASS하였으나 기본값은 B 추천. Fissure Edge2개+승인 Base·Overlay·Slab·Cliff v2 자체 SHA 변경 금지.
- **상태**: `A_TECH_VISUAL_PASS / B_TECH_VISUAL_PASS / B_RECOMMENDED / USER_B_DERIVED_GLOW_ART_APPROVAL_PENDING / OFFICIAL_UNITY_NOT_APPLIED / MAIN_NOT_MERGED`.
- 바울 대표의 이 메시지는 **Codex 결과 전달**이며 B안을 공식 최종 적용 승인하거나 신규 Glow 미술 최종 승인했다고 임의 해석하지 않는다.

## 다음 1단계 — B 후보 실물 승인 (기존 결과 재실행 불필요)
1. Codex가 이미 격리 생성한 **B 별도 파생 Glow PNG/GUID/Importer 후보**와 **동일 3 GameView/확대 비교 캡처**, Alpha/기존 이미지 원본 SHA vs 파생 SHA·파일명·예상 Sprite 참조 변경 1곳을 바울 대표에게 간명하게 보여준다. 이미 결과 자료가 갖춰졌으면 다시 생성/QA 실행하지 말고 파일 경로·캡처만 안내한다.
2. 대표가 B 후보의 **미술(붉은 열기/조사 가독성)**과 **정식 적용 범위**를 승인하기 전에는 원본 Field09 Scene·기존 공식 Glow/GUID/meta·Save/UserData/Editor 및 `main`을 수정하지 않는다.
3. 승인이 오면 새 별도 asset/메타/GUID를 유지한 채 코드·씬 참조만 최소 교체하는 `codex/*` 분리 기능 브랜치 작업 지시서를 작성하고, 기존 기술34PASS/과열10PASS는 재활용하며 정확히 변경분·Save·Quest 열기와 Scene 연결만 안전QA, 그 뒤 PR/main 반영은 바울 대표 요청·승인 별도.
4. Field09 리뉴얼 담당은 **Cliff v3/Edge v3·Glow 원화 재제작 금지 및 남은 환경 Phase2 양산 대기**. Chapter3 신규 몬스터 Idle2 제작과 독립 진행.

## 근거 이력
- `docs/planning/문서/00_프로젝트/Field09_Phase2_FissureGlow_CompositionRootCause_And_PrototypeGate_20261020.md`
- `docs/art-renewal/문서/00_프로젝트/Field09_Phase2_Pilot_v1_VisualFail_CliffFissure_ArtFix_v2_Delivery_20261020.md`
- Codex 사용자 보고: 기술/시각 A/B PASS, 34PASS/0FAIL, 10 UI PASS, QA `68855edd`, 인계 `453736cb`.
