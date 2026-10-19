# LIMITLESS / Arbel 리뉴얼 v3 — 소품 Single Sprite 원본 rect 문서 정정 인계 (2026-10-14)

> **최신 Arbel 납품 문서 기준: v3 (소품 메타데이터 정정판)**. v1/v2의 제작·교정 이력은 보존합니다. **이 문서는 Codex 실측·검증 결과를 전달받아 정리한 간접 인계 기록**입니다. 리뉴얼 담당 세션이 사용자 PC의 `F:` 드라이브 또는 v3 ZIP을 직접 열거나 실측했다고 주장하지 않습니다.

- 작업 상태: **`CODEX_METADATA_QA_PASS / ART_DELIVERED_BY_CODEX / USER_ART_FINAL_APPROVAL_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_APPROVED`**.
- 작업 브랜치: **`docs/art-renewal`** (문서만 갱신).
- 기준 GitHub `main`: **`bddca540d2fdd9e93f642f8d0f387b34730bd2d8`**; PR #10 복구 자산 정본 보존.
- 인계 근거: 바울 대표가 전달한 Codex v3 보완·무결성·Scene 대응 QA 요약(2026-10-14), 상세 로컬 보고서 `F:\study\codex\Project-Limitless\Temp\ArbelV3Metadata20261014\Arbel_v3_Metadata_Correction_Report_20261014.md` (**이 세션 미열람**).

## 1. 실제 납품·원본 보존 현황 (Codex 보고)

| 버전 | 파일 및 SHA256 | 상태 |
|---|---|---|
| **v3 최신** | `F:\Downloads\Limitless_Arbel_Renewal_Art_v3.zip` / **`84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b`** | Codex 제작 및 기술 문서 QA 완료. 바울 대표 최종 미술 승인 **대기** |
| v2 | `Limitless_Arbel_Renewal_Art_v2.zip` / `17625d35c93599bdd74a4b8280c0416240c402be279815c8966199410bca97b7` | 원본 보호, v3와 이미지 픽셀 동일 |
| v1 | `Limitless_Arbel_Renewal_Art_v1.zip` / `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626` | 원본 보호, 이전 제작 이력 |

- v3 **1,521,440 bytes / 총 60파일 / PNG43개**: 실사용 PNG 34개(직접 교체 후보 **23종** / 별도 미술·배치 승인 선택 장식 **11종**), Preview 8개, QA 이미지 1개.
- **v2 대비 전체 PNG43개 모두 byte 및 SHA256 동일**. 신규 PNG 제작·수정 **0**. 현행 Unity Scene/기존 복구 Sprite·meta/GUID·Collider·Quest·Save 변경 **0**.
- v3 ZIP 해시·무결성 및 로컬 경로는 **Codex 보고에서 전달된 값**입니다. 이 세션에서 실제 ZIP 파일 존재 여부나 원본 바이트를 독립 재검증하지 않았습니다.

## 2. v3 Single Sprite rect 정정 — 실제 이미지 변경 없음

현재 **Single Sprite (`fileID 21300000`) 8종은 PNG 전체 Canvas rect가 실제 `source_sprite_rect`**입니다. 옛 Multiple Sprite slice rect가 meta에 남아 있더라도, 현재 Sprite의 실제 rect로 사용하는 것은 잘못입니다. Codex가 과거 AssetDatabase 측정과 `main bddca540`의 Importer/PNG/GUID를 재대조한 결과를 정본으로 인계합니다.

| 소품 유형 | v2 문서의 구식 rect | v3 정정된 실제 Single Sprite.rect |
|---|---:|---:|
| Barrel | 64×82 | **128×128** |
| Bush_Dry | 99×76 | **128×128** |
| Bush_Olive | 100×84 | **128×128** |
| Campfire | 88×75 | **128×128** |
| Chest | 84×73 | **128×128** |
| DryMossRock | 102×62 | **128×128** |
| OldTree_Big | 205×247 | **255×256** |
| OldTree_Medium | 125×146 | **128×156** |
| Fence | 128×128 | **128×128 (변경 없음)** |

- 소품 **9종 중 8종의 `source_sprite_rect` 기록만 정정**, Fence는 원래 정상이라 변경하지 않았습니다.
- 비교 열은 **원본 Sprite.rect / 원본 Texture Canvas / Sprite-local Alpha bbox·하단 / 후보 Canvas / rect-bottom proxy**로 구분합니다. `Sprite.rect` 자체와 실제 Alpha bbox 또는 Collider 접지를 동일시하면 안 됩니다.
- v3 정본 CSV: `QA/Props_9_Grounding_Audit_v3.csv` 및 `QA/Props_8_Rect_BeforeAfter_v3.csv`. 원본 `QA/Props_9_Grounding_Audit_v2.csv`는 v3 ZIP 안에서 교정 기록으로 갱신됨.

### 원본 Alpha 하단 → 후보 Alpha 하단 (Codex의 오프라인 계측)

**PPU128 / Center / sprite-local world offset 기준**이며 **Alpha>0 그림자 픽셀 포함**입니다. 이 수치는 **실제 본체/물리 접지의 승인값이 아닙니다**.

| 소품 | 원본 | 후보 |
|---|---:|---:|
| Barrel | -0.328125 | -0.46875 |
| Bush_Dry | -0.359375 | -0.4375 |
| Bush_Olive | -0.359375 | -0.4375 |
| Campfire | -0.2890625 | -0.453125 |
| Chest | -0.296875 | -0.4765625 |
| DryMossRock | -0.2421875 | -0.46875 |
| OldTree_Big | -0.96875 | -0.90625 |
| OldTree_Medium | -0.5625 | -0.53125 |
| Fence | -0.296875 | -0.375 |

**특별 주의:** `Props9` Preview PNG의 과거 잘못된 숫자 자막은 **PNG43개 원본 바이트 보존** 때문에 수정하지 않았습니다. 해당 Preview는 v2 당시 역사 자료입니다. **표시된 소품 Sprite.rect 숫자를 v3의 실제 값으로 재사용하지 말고, 최신 CSV/Manifest/본 문서 수치를 기준으로 하세요.**

## 3. 갱신된 v3 ZIP 내부 문서

- 기존 문서 보완: `Manifest.json`, `Renderer_Mapping_284.csv`, `README.md`, `Codex_Integration_Handoff.md`, `QA/Props_9_Grounding_Audit_v2.csv`, `QA/Offline_Validation_v2.json`.
- 신규 정본: `Metadata_Correction_v3.md`, `QA/Props_9_Grounding_Audit_v3.csv`, `QA/Props_8_Rect_BeforeAfter_v3.csv`, `QA/PNG_SHA256_v2_v3.csv`, `QA/Offline_Validation_v3.json`.
- 이전 v2 도로 RGB/Alpha Seam 오류 **0**, 북서 건물 십자 **205px 제거된 v2 이미지 동일**, 남은 전체 실사용 PNG 및 Preview **원본 그대로**.

## 4. 검수 결과 및 적용 금지

**Codex가 전달한 기술 QA (직접 실행·확인 결과가 아닌 인계값)**

| 검사 | 결과 |
|---|---|
| 원본 Single Sprite.rect 정정 오류 | **8종 중 0** |
| Renderer 대응 / 이름·좌표·GUID·fileID·PPU·Pivot·Sorting·target | **284/284 보존** |
| PNG v2↔v3 byte·SHA 검증 | **43/43 동일** |
| 도로 RGB/Alpha Seam | **0 불일치** |
| ZIP CRC·Manifest·PNG decode | **PASS** |
| 실제 `main` 대응 | **2,455 PASS** |
| 패키지 검사 | **191 PASS / FAIL0** |
| 원본 프로젝트 5,221파일 SHA/HEAD/dirty·v1/v2 ZIP | **Codex 보고상 보호** |
| Unity 정식 적용·Play·미술 승인·Runtime 승인 | **없음** |
| Codex Commit/Push 또는 main 병합 | **없음** (이번 전달된 Codex 로컬 작업) |

아트 담당의 GitHub 문서 커밋은 위 Codex 로컬 ZIP 검증과 **별도 작업**입니다.

## 5. 별도 HOLD / 다음 담당 행동

1. **HOLD: Roof 2개** 확대에 따른 NPC·출입구·Camera 가림 및 실제 런타임 배치.
2. **HOLD: Props 9종** 그림자 포함 Alpha 접지와 본체/Collider 실제 접지를 구별한 실물 QA.
3. **HOLD: Optional 11종** 개별 미술 승인·배치 허가. 기본 교체 후보 23종과 합쳐서 자동 적용하지 말 것.
4. **HOLD: Portal** `(-7,-4)`의 기존 TreeSouthWest `(-7,-4.2)`와 겹침. 장식/나무/포털·출구/마차 동선 무단 조정 금지.
5. 바울 대표 최종 미술 승인 전 **Unity Import/Scene·Prefab·GUID/meta·Collider·Quest·Save·기존 승인 미술 수정 금지**. 승인 후 Codex가 전용 worktree에서 승인 범위만 통합하고 실제 Runtime·접근성·Save·Scene 회귀를 별도 검사할 것.

**상태 결론:** v3 **문서 정정 후보 QA는 PASS**, 그러나 **`USER_ART_FINAL_APPROVAL_PENDING`** 및 **`UNITY_NOT_APPLIED / RUNTIME_NOT_APPROVED`**. 이 GitHub 납품 문서는 ZIP 자체를 재업로드하거나 v3 PNG를 수정하지 않습니다.
