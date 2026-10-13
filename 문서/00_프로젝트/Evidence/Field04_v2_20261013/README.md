# LIMITLESS Field04 east-west road correction v2 — USER_ART_REVIEW_PENDING

This ZIP preserves original Field04 v1 (external file, SHA256 in Manifest), never modifies Unity. Only user-approved Codex integration may apply the proposal.

## What's new
- `Road_30/` has **30 real, independent 128×128 RGBA road tile PNGs**, mapped to the **30 existing RoadLeft/Right GameObjects by exact fileID**. These are NEW east-west art, not a rotated old PNG.
- `RoadPlacement_30.csv` gives exact old/new positions. Proposed x=-7..7 and y=+0.5/-0.5 for 2 lanes. Road objects are SpriteRenderer-only; DO NOT move any collision/quest/NPC/scene exit.
- `Optional_Endpoint/` contains **12 optional extra PNGs** (x=-10,-9,-8,+8,+9,+10, two rows) extending visual road to both actual exits. Adding new Renderer objects requires separate user approval and collision/West Spawn clearance testing. This is necessary because 30 tiles alone span just 15 columns while Field04 is 21 columns wide.
- `Manifest.json` corrects: Grass old label `tiles_grass_6_0` for original fileID1606921373; actual TreeMedium3 (GUID5b888) uses Medium art; actual TreeBig2 (GUIDd062) uses Big art.
- `Props/LL_C2_F04_RockNorth_Grounded_128_v2.png` reconstructs Field04-specific rock art to approximately old .83x.52 visible size and ~-.26 local-world alpha-foot under proposed PPU128 Center. **Not the Field03 RockB v2.** Physical/Unity exact silhouette clearance NOT_VERIFIED.
- Ground/other Props PNGs are byte-exact carried from v1. `Optional_Story/` remains unapproved extras.

## Functional hold
East x=+10.7 -> Field03; West x=-10.7 -> Arbel. Protected (7,0), (5,-2), (2,2), (-1,0), (-3,0). No movement, Quest IDs, Spawn, NPC, Save, Collider, bounds, settings, TTS/BGM, official Serin/monster changes were made. Existing west Spawn (-8,0) TreeWest overlap and northern non-exit boundary opening identified in v1 inspection remain OPEN problems, NOT solved by art.

## QA boundary
Actual PNG decoding, names/counts/SHA and alpha bbox checked offline. Preview images are **synthetic composites**, NOT live Unity screenshots. No original Unity import, runtime/Editor, accessibility, collision or gameplay claim. User art approval followed by Codex isolated background Unity QA required.
