# LIMITLESS Chapter3 F1 — environment original 6 + 3 targeted art samples (2026-10-21)

**Status:** `ART_VISUAL_REVIEW_HOLD / TARGETED_GROUND_SAMPLES_CREATED / USER_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED`.

## Basis

- Issue #30 and `docs/planning/문서/00_프로젝트/Chapter3_20261021_F1_Main22_FirstPlayable_Priority_And_BossDefer.md`.
- Original source package: `Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip`; all six original F1 PNGs were extracted and preserved **byte-for-byte**.
- Chapter2 official visual references inspected directly: `Environment/DeepCore_Ground_Base.png` and `Environment/DeepCore_Cliff_Boundary.png` from `Limitless_Main20_DeepCore_VeinfireColossus_Art_Pack_v3.zip`. These are **different biome / 512px source**, used to benchmark surface depth and material clarity, not to copy palette nor to infer F1 PPU or 1:1 GameView scale.

## Contents

`Original_F1/` — **six unchanged source PNG** (each 256×256 RGBA):

- `NorthF1_Ground_Earth_Base.png`
- `NorthF1_Ground_ThinFrost_Base.png`
- `NorthF1_EarthToFrost_Transition_Overlay.png`
- `NorthF1_Path_WornTrail_Overlay.png`
- `NorthF1_Prop_FrostShrub_01.png`
- `NorthF1_Prop_Waymarker_01.png`

`Targeted_Samples/` — **three NEW art-test overlays/ground textures**, not approved game originals:

- `NorthF1_Ground_Earth_Base_ArtSample_v2.png` — the same dirt/green composition, light micro-pebbles and sparse grass detail.
- `NorthF1_Ground_ThinFrost_Base_ArtSample_v2.png` — still earth-dominated, thin cold-gray flecks along small stones and grass; visible local RGB changes (>7 per channel) cover approximately **6.7%** of pixels relative to new earth base.
- `NorthF1_EarthToFrost_Transition_Overlay_ArtSample_v2.png` — sparse, translucent right-weighted frost transition; not a 4-direction terrain transition set.

`Preview/` — original-six contact, original versus new 128px comparison, offline F1 composite connecting earth/frost/path, Chapter2 official-source visual comparison. **These are composite previews, NOT Unity GameView capture**.

`QA/` — two 2×2 repetition checks and Offline_QA.json.

## Visual QA

1. **Earth base v1 HOLD**: limited normal/depth cues and geometric repeating olive forms; compared with Chapter2 official rocky ground, the terrain microtexture is sparse. The new sample adds subtle chips but **is not guaranteed to match the polish of official Chapter2 art**.
2. **Thin Frost base v1 HOLD**: broad neutral-gray tint reads like later winter stage, not early first frost. New sample restores brown earth as primary color and adds restrained icy flecks. **Snow blanket intentionally absent**.
3. **Transition overlay v1 HOLD**: powdered pixel noise rather than convincing dirt/frost material transition. New sample reduces flakes; richer edge variants still need artist approval.
4. **Walkable dirt path v1 HOLD (unmodified)**: dirt/road contrast too weak in composite. Before game-ready, increase material/edge contrast and test with a wheelchair/player sprite at actual F1 PPU; do not assume pass based on contact sheets.
5. **Frost shrub v1 HOLD (unmodified)**: useful silhouette, but minimal depth/branch variation. **Waymarker v1 HOLD/PARTIAL (unmodified)**: iconically readable, but simplified compared with richer Chapter2 art. Rework only if requested/if initial composite fails user review.
6. All 6 source files remain untouched and are included for proper before/after decision. No Scene, Collider, asset GUID/Unity metadata/Save/UI/monsters/boss were changed.

## Offline QA

- Each source and new art PNG: 256×256 RGBA, decoded successfully, original ZIP source byte hashes match exact files in `Original_F1/`.
- Both ground candidates: 2×2 repeat boundary left/right and top/bottom exact pixel equality. **Subpixel visual repetition and actual terrain adjacency require Unity/art approval**.
- Overlay PNG maintains alpha transparency. No baked sprite, UI labels or annotations inside game-candidate PNGs.
- **PPU, tile cell sizing, world geometry and Pivot not asserted**. 128px thumbnails are PNG size comparison, not actual Unity display scale. Chapter2 comparisons are matching visual panel widths (200px) for detail assessment and are explicitly different biome/source dimensions.
- Samples are **pre-approval**. User art signoff and Codex final actual map import/clear route testing remain pending.

## Recommended next action

Review original-six contact and **three revised texture samples** before touching the remaining path, shrub or waymarker. If dirt / frost direction accepted, request Codex actual Unity F1 PPU & passable trail width and then commission only the remaining material transition/path readability corrections. Do not begin Main23 village NPCs, FrostTyrant combat, F2-F5 or modify Chapter1/2 renewal assets.
