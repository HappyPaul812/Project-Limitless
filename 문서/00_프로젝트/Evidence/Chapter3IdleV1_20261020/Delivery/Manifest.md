# LIMITLESS Chapter 3 — FrostFieldMouse A / FrostTyrant B Idle 4-frame Pilot v1

## Status
**DELIVERED / OFFLINE_TECH_QA_PASS / USER_ANIMATION_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED**

This pilot exclusively animates **approved 512x512 RGBA original one-pose artwork**, with source PNG pixels preserved. Original artworks are not modified and not overwritten.

## Approved source hashes
- FrostFieldMouse A: `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`
- FrostTyrant B: `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830`
- Source ZIP: `0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919`

## Actual deliverables
- `Sprites/FrostFieldMouse_A_Idle_4Frames.png` — 2048x512 RGBA, 4 horizontal 512x512 cells, facing right, visualScale=1
- `Sprites/FrostTyrant_B_Idle_4Frames.png` — same size/format, visualScale=3
- `Preview/FrostFieldMouse_A_4Frame_ContactSheet.png` — per-frame review
- `Preview/FrostTyrant_B_4Frame_ContactSheet.png` — per-frame review
- `Preview/FrostFieldMouse_A_Idle_8FPS_Exact.apng` / `Preview/FrostTyrant_B_Idle_8FPS_Exact.apng` — exact 125 ms per frame / 8 FPS animated PNGs
- `Preview/FrostFieldMouse_A_Idle_8FPS_Loop.gif` / `Preview/FrostTyrant_B_Idle_8FPS_Loop.gif` — animated GIFs, 120/130/120/130 ms alternating due to GIF's 10 ms timing precision; still exact 500 ms per loop on average
- `QA/Display_128px_Comparison.png` — all frames at 128px
- `QA/Offline_QA.json` — source SHA, frame difference counts and bounding boxes, grounding validation
- `SHA256SUMS.txt` — checksum list for all other package files

## Locked import contract provided by Codex
- Frame sequence 0→1→2→3, repeat; 8 FPS; 0.5 sec per loop
- Cell 512x512, strip 4 columns x 1 row
- PPU 512, Pivot bottom-center (0.5,0), Point filter, RGBA transparency
- Direction right only. VisualScale 1 (field mouse), 3 (frost tyrant)

## Animation production method
The approved pixels provide the anatomical and palette baseline. Each frame gets **non-rigid region-weighted deformation** rather than whole-object vertical shifts or 4 identical copies. Rat: localized breathing of back/ribcage, head/whisker/tail secondary action; boss: ribcage/shoulder motion with secondary skull/antler/forearm movement. **The bottom 74 rows remain pixel-identical to the approved pose in every frame to lock foot placement**. Frame 00 exactly matches the approved RGBA sprite image (pixel equivalence). No new attack, hit, defeat, map, code, prefab or existing asset modifications.

## Caveats
- Technical QA here is Pillow/OpenCV image validation, not Unity Editor import, Play Mode, combat UI size validation, sprite controller linkage, or artistic approval.
- Please review whether the motion is visible but restrained at actual 128px UI display and whether the left/right silhouette fits the intended in-game scene. Not approved for Unity merge until user animation-art acceptance.
