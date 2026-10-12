# LIMITLESS — Field_04 Western Border original environment art v1

**Status: GAME-READY INDIVIDUAL PNG / USER_ART_REVIEW_PENDING / UNITY_IMPORT_NOT_VERIFIED.**

## Theme
Dry forest edge between Field03 and Arbel. Living trees, faded grass, dried ochre soil, lower water; **NOT** desert, burning field or ash land. Chapter3 assets not included.

## Actual source investigation (GitHub Scene + current runtime code)
- Scene: `Field_04_WesternBorder`, WorldBounds 21×15.
- Existing environment SpriteRenderer **363** = Grass 3 types 315 + two-lane Road 30 + Trees/Bush/Rocks/Fence 18.
- 12 existing sprite roles mapped individually by fileID+GUID; **3 additional transparent decals** are OPTIONAL and are **not** replacement references.
- `Field04_DryForest` Theme groundTint `(0.69,0.61,0.42,1)` applies to Grass_/Road renderers **only**. Extra theme Tree/Bush positions are empty.
- Runtime connections are **west to Arbel / east to Field03**, despite deprecated North Entry objects left in Scene. Current connections: east transition (10.7,0) spawn (8,0), west transition (-10.7,0) spawn (-8,0). Do NOT change legacy Scene triggers or spawn IDs by guess.
- Main12/13 markers: west entry (7,0), soil (5,-2), stream (2,2), Serin (-1,0), scripted beast encounter (-3,0).

## Deliverables
- `Ground/` 3 unique 128×128 RGBA dry grass tiles and 2 original 128×128 road halves.
- `Props/` trees, bushes, rocks, fence (7 independent transparent sprites). Tree Big 255×256, Tree Medium 128×156, other Props 128×128.
- `Optional_Story/` three decals for optional site embellishment; NEVER auto-place on existing Quest Marker; no colliders.
- `Preview/` Scene coordinate composite, protected site and spawn overlay, optional decal proposal, contact sheet.
- `QA/` real PNG 6×6 repeat seam composite, raw/runtime GroundTint side-by-side, machine validation JSON.
- `Manifest.json`, `SpriteMapping.csv`, `README.md`, `Codex_Field04_Survey_And_Integration.md`, `Rights_And_Provenance.md`.

## Important QA boundaries
- Pixel-level tile edges and PNG decoding can be verified without Unity; those checks are provided. This is **not** a Unity captured Preview.
- Actual local Sprite texture Rect/PPU/Pivot and Rock A/B shapes, Collider feet, sorting, `FieldThemeInstaller` ordering, `Quest Marker` visibility, all spawning/combat, mouse/keyboard gameplay and Save/Continue are **NOT_VERIFIED**.
- All 363 existing Renderer transforms, sorting, world edges, colliers, GUID/meta, story, NPC/Monster Sprite, TTS/BGM, Save, and gameplay must remain unchanged. Do not bulk overwrite ThirdParty source PNG/guids.
