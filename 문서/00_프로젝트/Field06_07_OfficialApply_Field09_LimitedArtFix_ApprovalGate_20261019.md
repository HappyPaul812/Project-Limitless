# LIMITLESS / Field06·07 원본 적용 완료, Field09 기존 아트 8종 국소 보완 승인 게이트 (2026-10-19)

- 담당 기획: `docs/planning`. 기록 근거는 바울 대표가 전달한 Codex 최종 통합 보고 캡처이며, 본 기획 세션의 실물 Unity Editor 조작이나 Field09 PNG 직접 픽셀 검수가 아니다.
- **상태:** `FIELD06_ORIGINAL_DISK_APPLIED / FIELD07_ORIGINAL_DISK_APPLIED / FIELD06_07_DELTA_124_PASS_0_FAIL / FIELD08_EXISTING_OFFICIAL_PROTECTED / FIELD09_VISIBLE_DEFECTS_CONFIRMED_BY_CODEX / FIELD09_ART8_MINIMAL_FIX_SCOPE_PROPOSED / FIELD09_USER_REWORK_AUTHORIZATION_PENDING / GIT_MAIN_MERGE_NOT_DONE / ORIGINAL_EDITOR_USER_PLAY_NOT_VERIFIED`.

## 1. Codex 최신 완료 보고 우선

- **Main10**: 복구 승인 음성18 StoryVoiceCatalog 정식 연결/원본 디스크 안전 반영, **164 PASS/0 FAIL** 보존, 새 재제작·TTS/API0.
- **Field06**: `B42` 도로 선택 확정, 이전 Runtime Ground49 빈틈 수정 유지. Arbel/Field06 Spawn 약0.455(또는 실제 계측 약0.47) 이동 편차는 과거 FAIL이었으나 **도착 Spawn Player-Collider 겹침이 안전 위치 보정을 유발한다는 근본 원인**을 고쳐 격리 재검증 완료. 도착 이후 Save/Continue/월드 연결 보호. **원본 디스크 적용 완료 보고**.
- **Field07**: `B42` 도로 선택 확정, 기존 투명 먼지 자산 활용해 `Ash7` 불투명 사각형 문제를 교정, 선택 리뉴얼 신규 Runtime PNG는 미사용. **원본 디스크 적용 완료 보고**.
- Field06/07 공통 변경분 Delta QA **124 PASS/0 FAIL**. 이전 Field07의 `324 PASS/1 FAIL` 및 Field06 Spawn 0.455 FAIL은 **해결 전 이력**, 최신 QA로 OPEN 유지하지 않는다.
- **Field08**: Field07↔Field08 연결/Spawn/Continue 확인, 기존 공식 환경 PNG9종 보호, 변경0.
- **Field09**: 실제 화면에서 **지면 반복선 및 소품6종 흰 가이드선 노출** 결함 확인. 아직 신규/수정 PNG 제작·Scene 교체0.
- 기능/보고 커밋(사용자 Codex 캡처): `codex/field06-v11-unity-integration-20261019` `0202da42fc2def05d18bcc1781669d02a7b6ba02`; `codex/field07-v1-unity-integration-20261019` `c5453d874f68fbc15d9da64564cc362c92e7e828`; `codex/field09-existing-art-visual-review-20261019` `2747fa45d1bc32c0d3acce8f3c542fccceb700cf`. GitHub 원격 main/작업 브랜치의 실제 병합은 발생하지 않았다는 보고. 사용자 원본 보호 **4,267파일 SHA/UserData23**, 원본 Editor의 사용자 실제 플레이·Main15/16 전체 스토리 전주행 미검증.
- 위 상태를 이전 기획 인계의 Field06/07 `ORIGINAL_APPLY_PENDING` 및 Field07 `1 FAIL OPEN`보다 우선하는 **최신 보고**로 기록.

## 2. Field09 보완 — 기존 공식 PNG 전체 리뉴얼 금지

근거: `docs/art-renewal/문서/00_프로젝트/Field09_ObsidianScar_ExistingArt_MinimalReplacement_Audit_20261019.md`. 기존 공식 `Unity/Client/Assets/_Project/Resources/Main18/Environment` **PNG10종** 중 활성 Sprite9종, 실제 국소 가시 결함 대상 **최대 8종**. 6개 흰 가이드선은 실제 화면에서 확인되었다는 Codex의 최신 결과(과거 '의심' 상태 갱신). 세부 픽셀 범위는 Codex QA 증거에 기반해 리뉴얼 아트 담당이 확정.

### 8종 범위 제안

**지면2종 (반복선/불투명 접합):**
- `ObsidianScar_Ground_Base.png`
- `ObsidianScar_Ground_Cracked_01.png`

**가이드선6종 (가이드 흰 테두리/점선만 국소 제거):**
- `ObsidianScar_Cliff_Blocker_01.png`
- `ObsidianScar_Fissure_Edge.png`
- `ObsidianScar_Fissure_Glow.png`
- `ObsidianScar_HeatVent_01.png`
- `ObsidianScar_Obsidian_Slab_01.png`
- `ObsidianScar_Obsidian_Spire_01.png`

기존 공식 `ObsidianScar_AshPatch_01.png`, 미사용 `ObsidianScar_Ground_Cracked_02.png`은 **바이트 단위 보호·미제작·미배치**. 타 몬스터/세린/과열 VFX/보스/Field08/Field10 아트는 제외.

### 최소 변경 준수

- **바울 대표의 보완 제작 승인 전**: 리뉴얼 담당은 위 8종 각각의 실제 결함 위치와 원본 기준 전후 확대 모식도/기존 Codex 실제 화면 자료 대조까지만 진행하며 **새 PNG/ZIP 제작0**. 이 문서는 작업 제안서로, 승인 대행 문서가 아니다.
- **사용자 승인 이후**: 기존 공식 8종 원본·meta/GUID/정본 SHA를 영구 보존하고, 보완 후보만 별도 버전명 `Limitless_Field09_ExistingArt_MinimalFix_v1.zip`으로 납품. 새 오리지널 콘셉트 전체 제작, 원색 변화, 사용자 가시 공식 포즈·실루엣/열기/장애물 형태 재디자인 금지.
- Ground 2종은 512×512 원본 규격/PPU128/Pivot 유지, Tiled 21×15 + 서쪽 Overlay9×15 경계에서 반복/접합이 실제 사라지는 최소 alpha/색 연속성 개선; 단 기존 Tiled/Tint/Importer 설정만의 변경으로 해결 가능하면 그림 불필요 수정은 제외(예: 지면2종 원본 보호).
- 가이드선 6종은 실제 보여지는 흰 임시 안내선·반듯한 점선/캔버스 테두리 **문제 픽셀만 제거**; 의도된 과열 Glow/Crack 형상/충돌 가독성을 약화시키지 않음. Transparent alpha bbox/foot, Collider 정합, Single Sprite Importer, sorting, readability 보존.
- 8개를 무조건 새 PNG로 납품하라는 뜻이 아님. **최대 8종 후보로 한정**하여 실제 픽셀/Runtime 설정으로 오류 원인이 설명된 것만 생성. 각 항목 개별 원본 SHA·보완 후보 SHA·Before/After·변경 RGBA 좌표/입자 또는 패턴 증거를 명확히 기록.
- ZIP에는 후보 실사용 PNG, `Manifest.json`(SHA/size/PPU/pivot), `Changed_Pixel_Regions.csv`, `Renderer_Mapping.csv`, `QA/Offline_Validation.json`, `Rights_and_Provenance.md`, ContactSheet·Codex Import/QA 인계. 합성 프리뷰는 Unity 실제 화면으로 표시 금지.

## 3. Codex와 병렬 운영

1. **현재 Codex**는 Field06/Field07 원본적용·변경분 QA 완료 결과를 유지하고, Main10+Field06/07 반복 수정/전수 QA를 하지 않는다. 원본 사용자 Editor Play/포커스·Save를 임의로 조작하지 않음.
2. **리뉴얼 아트 담당**은 사용자 제작 승인 이후 별도 `docs/art-renewal` 납품/인계, PNG 작업만 맡는다. 다음 몬스터18·기본 펫 Wolf/Bear/Fox 오리지널 시안·실사용 파일럿은 사용자 최신 발주에 따라 **병렬 진행**하며 Field09로 전체 리뉴얼 생산라인을 되돌리지 않음.
3. Field09 후보 리뉴얼 담당 납품→대표 미술 승인→Codex가 별도 `codex/*` 격리 Scene에서 미술 변화/가독성/액터·Quest마커/Field08↔09/과열/Save·Continue/휠체어·화면비 delta QA 후 안전한 원본 적용 판단. Main18 기존500PASS/0FAIL 전체 재실행 금지.
4. 모든 커밋은 담당별 브랜치에만; `main` 직접 Commit·Push·병합 금지. 각 `USER_ART_APPROVED / ISOLATED_QA_PASS / ORIGINAL_DISK_APPLIED / PR_MAIN_MERGED / USER_EDITOR_PLAY_PASS`를 구분한다.

**기획 결론:** 환경 정식 통합 Field06/07은 최신 Codex 보고상 완료. Field09는 실제 불량 확인된 **최대8종만 국소 보완 제작**을 권고하나 신규 PNG/ZIP 만들 권한은 **바울 대표 추가 승인 대기**. 몬스터·펫 작업을 계획대로 추진.
