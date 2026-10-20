# LIMITLESS — Field09 신규 환경 Sample3 최종 시각승인 및 잔여 아트 Phase2 (2026-10-20)

- 바울 대표 최신 승인: **“봤는데 좋아.”** 이전 Codex OverlayFix v2 PASS 보고 후 신규 리디자인 Sample3 실제 GameView 결과를 확인하고 **3종 스타일·시각을 승인**. 이전 부분수정 v1/v2 FAIL 후보와 혼동 금지.
- 보고된 Codex QA: `codex/field09-overlay-v2-qa-20261020` QA commit `d31bbf4f` / 상태·인계 `5ee6dc07`. Overlay 수정본 ZIP `F:\Downloads\Limitless_Field09_Redesign_Sample3_OverlayFix_v2.zip` SHA256 `ec831166a753eef179da8fc283faf1fe81d1880941bce10c8d93225269cd418c`, Overlay PNG SHA256 `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`, 정확히 alpha3705픽셀 수정. 별도 격리 Unity Delta17 PASS/0 FAIL, 동일 GameView 3 화면비에서 x≈-1.5 밝은 균열 직선 절단/경계잔재/알파 끊김/반복띠 없음.
- **미술 승인 자산 3종 정본 SHA256**: `Base Ground` = `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203` (v1 그대로); `Cracked Overlay v2` = `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`; `Obsidian Slab` = `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692` (v1 그대로). Cracked Overlay v1과 원래 공식 Main18 오버레이는 승인 대상 아님.
- 상태 `SAMPLE3_USER_VISUAL_APPROVED / ISOLATED_UNITY_VISUAL_PASS_17_OF_17 / ORIGINAL_SCENE_NOT_APPLIED / MAIN_NOT_MERGED / FIELD09_FULL_ENV_NOT_COMPLETE`. 기존 화면 적용/Push/Main 직접 변경은 아직 불허.

## Phase2: 리뉴얼 담당 실제 신규 환경 나머지 제작 진행

1. **반드시 승인3종의 픽셀 SHA 그대로 보호**, 새 시각 기준선을 이 3종(공통 512×512 RGBA/PPU128/Pivot center)으로 한다. Field09 전체를 재생성하거나 CrackedOverlay가 v1로 회귀하게 하면 안 됨.
2. GitHub `main`의 Field09 공식 환경 자산10종과 실제 Unity Scene 참조를 Codex가 이미 감사한 인계와 대조해 남은 **실사용 SpriteRenderer 역할**만 정리. 승인3종의 대응 BaseGround/CrackedWest/Slab을 제외하면 검사대상은 현재 `ObsidianScar_AshPatch_01`, `ObsidianScar_Cliff_Blocker_01`, `ObsidianScar_Fissure_Edge`, `ObsidianScar_Fissure_Glow`, `ObsidianScar_HeatVent_01`, `ObsidianScar_Obsidian_Spire_01` 6종이다. 이 중 과거 승인·원래 양호한 소품은 **KEEP** 가능, 그러나 흰가이드/스타일 불일치 등 실제 문제가 있으면 독립 신규 오리지널 PNG 재제작. `ObsidianScar_Ground_Cracked_02`는 현재 미사용이라면 **HOLD/KEEP**하고 불필요 양산하지 않는다. 10개를 무조건 새로 만드는 것이 목표 아님.
3. **한 번에 남은 전체 그림을 무작정 양산하는 것보다** 고립된 확인이 어려운 소품을 대표 타입별 2~3종 묶음으로 제작/Preview→실제 역할 및 현행 Collider/Sorting/접지 확인→나머지 필요 자산 제작. Main18 과열·퀘스트 상호작용 및 유효한 안내선(의도된 붉은 Glow 등)을 불필요하게 삭제하지 않음.
4. 작업물이 독립 PNG, 실제 SHA·Manifest·Role-to-Renderer 매핑·기존3종 유지 SHA·Alpha/ground seam/슬랩·소품 접지 QA·오프라인 실제 원본/신작 비교 및 정상 ZIP로 납품. 가능하면 `docs/art-renewal/Art_Deliveries/` GitHub 실제 바이너리로 Codex가 접근하도록 하되 담당 다른 ChatGPT 세션의 파일 경로가 사용자 로컬에 자동 저장된 것으로 주장 금지.
5. Art Phase2 납품 후 **Codex는 변경분만** 독립 Unity visual QA, 기존 Sample3 v2 17PASS 및 Main18 92PASS 재사용·기존 원본3,546+·Save/UserData/Editor 보호. 결과와 대표님 최종 전체 아트/정식 적용 승인은 별도. **원본 Unity scene/main 병합은 사용자 별도 지시 전 금지**.

## 운영
- 리뉴얼 `docs/art-renewal`, 기획 `docs/planning`, Codex `codex/*` 브랜치, `main` 직접 Push/병합 금지.
- Chapter3 신규 아트 `docs/art-new`와 현재 Field09 Chapter2 리뉴얼 업무 분리. 바울 대표 현재 Chapter3 신규 아트+ BGM만 작업 우선, 기타 TTS/SFX/영상 보류. 리뉴얼은 Field09 Phase2 계속.
- 기존 Issue #37은 **단일 Overlay v2 보완·검수/미술승인 게이트 충족**으로 닫고, 잔여 Field09 환경 Phase2는 별도 새 이슈.
