# LIMITLESS — Haren / Serin Art Pack v1

**Review status:** Visual review required before Unity production approval. No game files or existing official Serin artwork were modified.

## Art files

| File | Purpose | Size |
|---|---|---|
| Haren_Portrait.png | Haren portrait / background version | 1254×1254 RGBA |
| Haren_Portrait_Transparent.png | Haren portrait / alpha version | 1254×1254 RGBA |
| Haren_Field_Walk_4Dir.png | Haren field; Down, Left, Right, Up | 512×512 RGBA (4×4, 128 px each) |
| Haren_Battle_6Row.png | Haren battle | 1254×1254 RGBA (6×6, 209 px each) |
| Serin_Battle_6Row.png | Serin battle | 1254×1254 RGBA (6×6, 209 px each) |

## Battle row contract (both characters)

| Row (top to bottom) | Action | Content cells | Empty cells |
|---|---|---|---|
| 0 | Idle | 0–3 | 4–5 |
| 1 | Attack | 0–5 | — |
| 2 | Guard | 0–3 | 4–5 |
| 3 | Skill | 0–5 | — |
| 4 | Hit | 0–3 | 4–5 |
| 5 | Defeat | 0–5 | — |

Total: 30 nonempty frames, 6 empty cells, no content on cell boundary lines. Battle PPU100, bottom-center pivot. Field PPU128, bottom-center pivot.

## Art continuity

- Haren: maintained the pre-existing portrait and 4-direction field artwork, bronze horizontal fantasy visor, blue-and-white cloak, veteran fighter identity.
- Serin: original official field and portrait remain untouched. Combat sheet follows observed official sprite styling: long chestnut-brown hair, blue circular hearing accessory, bronze/brown armor, red cloak, bow and quiver.
- This pack contains no new canonical Serin portrait or field sprite; the existing game assets are retained as canonical.
- `Preview/` is a presentation aid only and must not be imported as Battle Sprite.

## Technical inspection

- Each Battle PNG: 1254×1254 RGBA, 36 fixed cells 209×209.
- Six blanks are fully transparent; 30 useful frames are present.
- Alpha edge contact: zero across cell boundaries in new Battle PNGs.
- Requires user art review plus actual Unity animation playback/visual audit before final READY status.
- No code, Unity project, TTS, BGM or GitHub changes.
