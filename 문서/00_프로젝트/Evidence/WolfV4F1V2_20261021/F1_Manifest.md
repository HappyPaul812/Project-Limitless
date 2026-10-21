# LIMITLESS Chapter 3 — North F1 environment v2 art review pack

Status: ART_REVIEW_PENDING, NOT UNITY-INTEGRATED.

Six new candidate PNGs are in `Art/`; original pilot F1 6 PNGs are preserved byte-identically under `Original_Reference/`. These are art candidates, **not authorized game import or proven Unity texture contracts**.

## 6 edited assets

| File | Role | Size |
| --- | --- | --- |
| `Art/NorthF1_Ground_Earth_Base_v2.png` | natural brown earth + rocks + sparse grass, periodic texture | 256x256 RGBA |
| `Art/NorthF1_Ground_ThinFrost_Base_v2.png` | sparse thin frost on earth-dominant substrate, periodic texture | 256x256 RGBA |
| `Art/NorthF1_EarthToFrost_Transition_Overlay_v2.png` | directional transparent earth-to-frost transition (right-weighted), NOT auto-tile | 256x256 RGBA |
| `Art/NorthF1_Path_WornTrail_Overlay_v2.png` | dark worn brown trail with clear edges for visual wayfinding (not collider) | 256x256 RGBA |
| `Art/NorthF1_Prop_FrostShrub_01_v2.png` | bare woody winter shrub with sparse frost; transparent | 256x256 RGBA |
| `Art/NorthF1_Prop_Waymarker_01_v2.png` | weathered wood direction signpost with snow; transparent | 256x256 RGBA |

## Visual and technical QA

- All 6 new PNGs decode and are exactly 256×256 RGBA. Ground earth and thin-frost tile X/Y **edge pixels match** and both have 2×2 and 3×3 repeat images in QA. Seam math does not guarantee absence of large repeated patterns.
- Props and trail/transition use transparency. Independent connected objects may exist in overlays for pebbles or grass; **visual review required** before import.
- Preview is **Pillow offline compositing only, not Unity GameView**, and the reference panel compares images with different source resolutions and game biomes.
- Navigation accessibility is a visual proposal, **not collision geometry, route clearance, or gameplay validation**. PPU/Pivot/TileGrid/Collider/Sorting remain **TBD until Codex**.
- This version uses pixel-art style downsampling from 1254×1254 concept assets; therefore native hand-polished 256×256 pixel art is **not yet independently verified**.
- F2–F5, Frostway village, bosses and all previously approved monsters and Unity originals are untouched.

## Included references

- `Original_Reference/` source 6 PNGs from the earlier `Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip` unchanged.
- `Preview/F1_Art6_ContactSheet.png` shows six candidates; `Preview/F1_v1_vs_v2_SameDisplayScale.png` shows same pixel size old/new; `Preview/F1_Earth_Frost_Path_Composite_OFFLINE.png` shows layout concept; `Preview/F1_vs_Chapter2_Official_Environment_OFFLINE.png` compares artwork qualities, not Unity PPU.
- `QA/` includes 2×2/3×3 repeat panels and JSON test results; `SHA256SUMS.txt` covers all packed assets.

Approval gate: User art review → Codex actual F1 PPU/TileGrid/Collider/navigation → only then Unity integration. No main commit or merge.
