# LIMITLESS / 2026-10-22 — 서릿길 마을 v6·Bear Run6 v1·Fox Run6 v1 독립 격리 검수 계약

- 사용자: 바울 대표가 Windows `F:\Downloads`의 아래 세 ZIP 실물 경로를 전달. 기획 담당은 해당 Windows 디스크를 직접 읽지 못하므로 아래 SHA는 **각 미술 담당의 GitHub 납품 신고값**이며 **Codex 사용자 PC 실제 readback 전 확인값 아님**.
- 상태: `THREE_ART_DELIVERIES_RECEIVED / CODEX_ACTUAL_ZIP_READBACK_PENDING / USER_ART_APPROVAL_PENDING / GAME_NOT_INTEGRATED`.
- 전날 2026-10-21 EOD 승인 Wolf/Chapter3 Main22/F1/서리들쥐 제품58건 PR #48·문서PR #49 main 및 로컬 동기화 `SYNC_COMPLETE` 이후 최신 main 정본 보호. 기존 기능/전수QA 반복 또는 원본 Editor·사용자 dirty/Save/UserData·배포 자산 수정 금지.
- 담당 정본: `docs/art-new/문서/00_프로젝트/Chapter3_Main23_FrostwayVillage_GameArt_VariantTiles_v6_Art_QA_20261022.md`, `docs/art-renewal/문서/00_프로젝트/Beast_Bear_Run6_v1_ArtPilot_Delivery_20261022.md`, `Beast_Fox_Run6_v1_ArtPilot_Delivery_20261022.md`; Issue #45; 팀 공통 Git/worktree 보호 규칙.

## 1. 사용자 실 ZIP / 담당 신고 해시

| 모듈 | 사용자 PC 입력 파일 | 담당 납품 SHA256 | 기준 |
|---|---|---|---|
| A Village v6 | `F:\Downloads\Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v6.zip` | `519ed96fa6157b7de451830412d6e881b633241b0f8cc459d186465765a4ea7e` | v5 PNG43 동일+신규 02/03 총6, 49PNG, ZIP71 |
| B Bear Run6 v1 | `F:\Downloads\Limitless_Beast_Bear_Run6_v1.zip` | `1a29475207c0f99db87726b637d301e5010bf5ef6790e2415ad679f67ffd7f19` | 소스6, Strip/Atlas, 12FPS, ZIP37 |
| C Fox Run6 v1 | `F:\Downloads\Limitless_Beast_Fox_Run6_v1.zip` | `6d352219505497a261ffe2c1d687cece28bdf4859003d16fce88334d9a15f43d` | 소스6, Strip/Atlas, 12FPS, ZIP37 |

입력 파일별 SHA/ZIP CRC/Manifest/SHA256SUMS는 먼저 실제 readback; 불일치면 모듈별 중단 및 원인을 보고, 다른 두 모듈 독립진행. 기존 승인 Wolf Run6 v5 SHA `16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351`와 비교 참고, Wolf 자체 변경 금지.

## 2. A Village v6 — 기술/미술/OPEN16 분리

1. 기준 v5 PNG43 전체 **byte-identical** 43/43 검증. 신규 타일6: `Tiles/Frostway_Ground_Earth_02/03.png`, `Tiles/Frostway_Ground_ThinFrost_02/03.png`, `Tiles/Frostway_Ground_Gravel_02/03.png` (각 실제 파일명은 `Frostway_Ground_Earth_02.png` 등). 256×256 RGBA Alpha255, 경계12px 동일, 01/02/03 ordered same-terrain seam 검증. Tile PathStone/SkillShop/Gable/Portal_Inactive·모든 기타43 png/GUID 보존.
2. 분리 격리 Unity Scene/TileGrid의 실제 PPU/Pivot·Import·TileGrid 대응을 검사하여 01/02/03 각 세 지형별 **5×5 이상 실 혼합 패턴** 구성, 단일01 반복 v5와 **동일 카메라·scale/PPU 조건** 비교. 16:9/4:3/21:9 GameView 실제 RenderTexture vs 오프라인 합성 엄격 구별.
3. 격자 패턴/동일 큰 돌·풀 모티프 반복·연결 seam/정렬, 길/건물 경계·Sorting, NPC·남북문/휠체어 통행 proxy **변경분만** 확인. 타일 변형 규칙은 의도치 않은 매핑 변경 없이 작은 시연용 배치로만 테스트. 새로운 정식 Village Scene/Quest/Main23 자동 구현/승인 금지.
4. Codex가 10/21 확보한 **OPEN16 원본 CSV+좌표 JSON**의 16행을 실제 파일명·원문 결함·좌표·재현조건 중심으로 안전하게 문서/Issue #45에 인계(개인기밀/OS 로컬 개인정보 제외), 별도 severity를 임의 생성하지 말 것. v6 신규6은 기존43 byte 동일이므로 OPEN16 해결을 자동 주장 금지; 관련 항목이 실제로 수정·검증됐는지 분리 검토. 16건의 부재근거를 보존하고 아트 신규 담당에게 대조 가능한 정확한 원문 제공.
5. 결과: `TECH_PASS/FAIL`, Earth/ThinFrost/Gravel `ART_PASS_RECOMMENDED`/HOLD, OPEN16 항목별 유지/신규 영향, 사용자 최종 미술 승인 PENDING. 본편 Import/PR/main 병합 금지.

## 3. B·C Bear/Fox — 서로 독립된 수락 판단

1. Bear와 Fox 각각 별개 SHA/CRC/Manifest/6 PNG(512×384 RGBA LEFT), Strip3072×384/Atlas1536×768 픽셀 일치6/6, alpha bbox 침범/잘림/프레임 재사용, GIF·APNG 루프 타이밍 총500ms 확인.
2. **예상 리스크**: art-renewal 문서에 따르면 두 동물은 **승인된 고해상도 Bear/Fox KeyArt를 기반 제작했다는 증거가 없고**, 약120px AI 콘셉트 보드 유래 개별 실루엣을 512×384 캔버스에 배치한 파일럿. 기존 타사 ScratchIO 검사 자산은 참고만 했다고 신고. 따라서 파일규격 PASS는 공식 미술 승인과 무관하며 **펫별 일관된 디자인·출처·원본 포즈 품질/확대 블러·털 디테일**을 선행 검수해야 한다.
3. 각 동물 실제 격리 Unity Play Mode **12FPS, 128px 및 40px 동일 Wolf 기준 크기 시연**. Frame00→01→…→05→00 발 접지·몸통 비율·외곽 점프, Bear 체중/짧은 꼬리/포즈, Fox 꼬리·귀/민첩성·발 미끄러짐 및 좌우방향 flip·loop 판정. 3화면비는 필요한 표시에만 변경분 검사; 기존 Wolf 106PASS·Main22/F1 전수 반복 금지.
4. 필요한 경우 기존 `BeastCompanionSystem` 및 `PetFacility`에서 **임시 프리뷰만**, 승인/정식 runtime 연결 금지. Wolf 공식 원본 PNG/meta/GUID·Controller/Animator 및 기존 Bear/Fox 시스템 수정 금지.
5. 각자 `TECH_PASS`/ `ART_PASS_RECOMMENDED`/ `ART_HOLD`/ `ART_FAIL`와 원인 프레임·좌표·실 GameView/동적 비교 시연, 미술 최종승인 PENDING. Bear가 실패해도 Fox를 미술성급하게 승인하지 말고 독립 검증.

## 4. 공통 안전·보고

- 권장 Codex 격리 브랜치 `codex/qa-village-v6-bear-fox-run6-20261022`, 결과 A/B/C 파일보고 분리. 이미 동일 검수가 수행됐으면 그 결과/커밋 재사용; 중복QA 금지. 안전한 read-only ZIP 원본·분리 worktree·SHA 보호 검사 후 테스트한다.
- 사용자 `F:\study\codex\Project-Limitless` 원본 Unity/Editor/Play/포커스/dirty/untracked/index, Save/UserData/Personal Settings/Chapter1·2/3 승인 자산, GitHub `main`을 바꾸지 말 것. `reset --hard`, `git clean`, 강제Push/미승인 본편 Import·PR 병합 금지.
- 최종 보고: 각 ZIP **실 SHA**, CRC/Manifest, 49PNG·43KEEP+6/new 또는 6frame strip 검증, A/B/C 실제 Unity 프레임/화면 증거, 기술/미술 독립 결과, OPEN16 원문 인계 여부, Git branch/commit/변경파일, 원본 SHA 보호 및 대표 최종 승인 여부.
