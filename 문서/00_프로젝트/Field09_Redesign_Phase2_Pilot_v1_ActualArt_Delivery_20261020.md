# LIMITLESS / Field09 리디자인 Phase2 소품 2종 실제 PNG 신규 제작·검수용 ZIP 납품

- 날짜: 2026-10-20 / 담당: Chapter1·2 리뉴얼 `docs/art-renewal`.
- 발주: [Issue #38](https://github.com/HappyPaul812/Project-Limitless/issues/38), 기획 정본 `docs/planning/문서/00_프로젝트/Field09_Redesign_Sample3_UserVisualApproval_And_Phase2_20261020.md`.
- **이번 납품은 Phase2 파일럿 2종만**. 나머지 소품 신규 제작 HOLD, 공식 Scene/원본/main 변경 없음.

## 1. 승인 Sample3 3종 보호 — 파일 바이트 자체 재검증

| 승인 소스 PNG | SHA256 정본 | 해시 직접 비교 |
|---|---|---|
| `LL_Field09_Redesign_Ground_Base_v1.png` | `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203` | PASS / 수정·재압축 0 |
| `LL_Field09_Redesign_Ground_Cracked_Overlay_v2.png` | `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7` | PASS / 수정·재압축 0 |
| `LL_Field09_Redesign_Obsidian_Slab_v1.png` | `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692` | PASS / 수정·재압축 0 |

Codex 격리 Unity17PASS/0FAIL 및 대표 스타일 승인 기록을 그대로 존중. 본 세션에서 기존 자산/Scene을 다시 만들거나 Unity 실행하지 않음.

## 2. 실제 공식 원본6종 + 현재 Field09 Scene SpriteRenderer 확인

GitHub main의 `Unity/Client/Assets/_Project/Resources/Main18/Environment` 공식 PNG6종 **실제 이미지로 열어 확인**. `Unity/Client/Assets/_Project/Scenes/Field_09_ObsidianScar.unity` 실제 YAML의 GameObject/Transform/Sorting/SpriteRenderer을 확인. 판정은 미술 감사이고 **실제 Unity Play 검수는 아님**.

| 기존 공식 PNG | 실사용 Scene GameObject | Position | Local Scale | Sorting Order | Phase2 판정 |
|---|---|---|---|---:|---|
| `ObsidianScar_AshPatch_01.png` | `ObsidianScar_Ash` | (0,-1.2) | (0.525,0.525) | -5 | **KEEP 후보**: 기존 먹색 재질과 비교적 조화 / 화면 재확인 |
| `ObsidianScar_Cliff_Blocker_01.png` | `ObsidianScar_Cliff` | (-9,0) | (0.65,0.65) | 2 | **REDESIGN 이번 제작**, 공식 이미지에 흰 가이드선 |
| `ObsidianScar_Fissure_Edge.png` | `ObsidianScar_Fissure` | (-5,3.8) | (0.525,0.625) | -8 | **REDESIGN 이번 제작**, 공식 이미지에 흰 가이드선 |
| `ObsidianScar_Fissure_Glow.png` | `ObsidianScar_Glow` | (-5,3.8) | (0.525,0.625) | -7 | REDESIGN/후속 HOLD, 과열 시각정보 보호 |
| `ObsidianScar_HeatVent_01.png` | `ObsidianScar_HeatVent` | (3,-2) | (0.45,0.45) | -6 | REDESIGN/후속 HOLD, 열기/연기 역할 보호 |
| `ObsidianScar_Obsidian_Spire_01.png` | `ObsidianScar_Spire` | (-8.8,4.8) | (0.625,0.625) | 2 | REDESIGN/후속 HOLD |

6종 모두 Scene 원래 SpriteRenderer **Simple** (m_DrawMode:0); 기존 위치·Scale·Sorting·Collider·기능 **변경 금지**. `Ground_Cracked_02`는 비활성/미사용이므로 HOLD.
원본 매핑/기준 증거는 ZIP 내부 `Asset_Keep_Redesign_Classification.csv`, `Asset_Renderer_Mapping.csv`.

## 3. 신규 독립 PNG 2종 실제 납품

| 신규 PNG (ZIP `Assets/`) | 사양 | SHA256 | 바이트 |
|---|---|---|---:|
| `LL_Field09_Phase2_Cliff_Blocker_v1.png` | 512×512 RGBA, 제안 PPU128/중앙 Pivot, 투명 소품 | `2e7a863547688773dbb3d6672623e0958fbab01c8241d2665d8739a0777a231c` | 264,517 |
| `LL_Field09_Phase2_Fissure_Edge_v1.png` | 512×512 RGBA, 제안 PPU128/중앙 Pivot, 투명 소품 | `7c892de74b1f3327b37f4fb3ff7673954d64542498b32f6ab76826e6ca53b1d0` | 208,478 |

둘 다 이번 Phase2의 **신규 창작 원화**를 별도로 생성한 후 각 소품만 추출하고 체크무늬/글자 영역을 분리해 실제 투명 PNG화한 산출물. 이전 부분수정 v1/v2 PNG 재포장, 기존 공식 원본 픽셀 복제, 이전 합격 Sample3 자산 수정·재생성 **없음**. 소형 표시·접지 및 투명 외곽에는 아직 시각 QA 필요.

## 4. ZIP/QA

- **납품 ZIP 실파일** `Limitless_Field09_Redesign_Phase2_Pilot_v1.zip` (이번 대화 `sandbox:/mnt/data/Limitless_Field09_Redesign_Phase2_Pilot_v1.zip`).
- **크기 1,672,537 bytes / SHA256 `f12b056924665eeb593d084d5a5b73ba67a1a3a335d2bd9f882a547e5b8a7ff6` / 9 entries.**
- `Manifest.json`, PNG2, `Asset_Keep_Redesign_Classification.csv`, `Asset_Renderer_Mapping.csv`, `Preview/Phase2_Pilot_vs_ApprovedSample3_ActualPNG.png`(새 실제 PNG2와 정본 승인3 동배치), `Preview/Phase2_Pilot_Offline_Composition_NOT_UNITY.png`(승인3+신규2 실제 파일 합성 예상화면), `QA/Offline_Validation.json`, `README.md`.
- 실제 파일 기반 QA: PNG 2/2 열기/512² RGBA PASS, 알파 배경 PASS, 235 이상 순백 불투명 가이드 픽셀0, 크기 7px 미만 고립 alpha 군집0; 세 승인 PNG SHA256 3/3 일치; ZIP 표준 zipfile testzip()=None CRC PASS 및 각 ZIP entry 외부 파일과 byte-for-byte 동일, Manifest SHA 대조 PASS.
- **미술 리스크:** 이미지 생성 원화에서 별도 스프라이트를 추출한 만큼 암석·용암 윤곽의 잔여 매트 느낌은 사람이 게임 표시 크기로 검사할 필요가 있음. `Fissure Edge`의 붉은 계곡 표현이 기존 별도 FissureGlow와 Sorting -8/-7 관계에서 과열·조사 정보를 가리지 않는지 확인 필요. Preview는 **실제 Unity GameView가 아닌 오프라인 합성**이고 해당 화면은 월드 Transform 정확 재현을 주장하지 않음.
- `main` 공식 Field09 PNG/meta/GUID/Scene, Main18/Save/UserData/Collider/몬스터/퀘스트/과열/세린/Field08 승인 자산 변경0. 전체 자산 대량 제작0.

## 5. Codex QA 후속 / 현재 상태

Codex `codex/*` 격리 worktree에서 본 PNG 2종만 새 GUID로 후보 Import, 512×512 RGBA PPU128 center-pivot, 원래 `ObsidianScar_Cliff`, `ObsidianScar_Fissure` SpriteRenderer만 교체한 **임시 씬**으로 3 화면비 GameView 및 접지/흰선/Sorting/퀘스트 마커·과열 정보/충돌만 변경분 시각 Delta QA. `ObsidianScar_Glow`는 기존 그대로 두고 Fissure와 겹침 검사. 기존 승인3 17PASS 및 Main18 92PASS 재실행 금지. 실제 FAIL시 픽셀 아트/Renderer 원인 분리, 샘플만 보완.

**현재 상태 `PHASE2_2_NEW_PNG_DELIVERED / FILE_QA_PASS / ZIP_CRC_PASS / APPROVED_SAMPLE3_3_SHA_PROTECTED / USER_ART_APPROVAL_PENDING / CODEX_UNITY_DELTA_QA_PENDING / ORIGINAL_SCENE_NOT_APPLIED`.** 나머지 Phase2 아트 신규 제작은 Codex 시각 PASS 및 대표 승인 전까지 HOLD.

**GitHub 보고서/재현 매핑만 `docs/art-renewal` 커밋. 신규 PNG/ZIP 바이너리는 이번 대화 sandbox 실제 파일이며 GitHub에 업로드했다고 허위 주장하거나 바울 대표 F: 자동 복사했다고 주장하지 않음.**
