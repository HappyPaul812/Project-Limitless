# Importing PVFX Foundry

## Grid sheets

Grid sheets are designed for engines and tools that only need regular cells.

- Cell size: 96×96 pixels.
- Columns: 5.
- Order: row-major, starting at frame 0.
- Frame duration: 50 ms for v0.2.0.
- Filtering: nearest-neighbor.
- Wrapping: clamp.
- Background: transparent RGBA.

For zero-based frame `n`:

```text
x = (n % 5) * 96
y = floor(n / 5) * 96
width = 96
height = 96
```

The final row may contain transparent unused cells. Stop at the declared frame
count; do not infer it from sheet dimensions.

## Packed sheets

Packed sheets trim every frame and use deterministic placement with one pixel
of extrusion plus one pixel of transparent padding. They require the adjacent
manifest.

For each manifest frame, read `sheet.x`, `sheet.y`, `sheet.width`, and
`sheet.height`. The pivot in sheet pixels is:

```text
[sheet.x, sheet.y] + trimmed_pivot * sheet.scale
```

The original canvas-space pivot remains in `pivot`. A trimmed pivot may fall
outside the cropped frame; that is valid and necessary for stable gameplay
alignment.

## Timing and markers

Every frame records exact rational milliseconds as `numerator_ms / denominator`.
All v0.2.0 sources evaluate to 50 ms per frame, but consumers should read the
manifest so future packs can use authored variable durations.

Read `loop_mode` from each manifest. `Rift Portal` and `Venom Ward` use `loop`
and have byte-identical first and final canonical frames for a clean seam. The
remaining effects use `none` and should stop after their final source frame.

Markers identify useful lifecycle beats such as `launch`, `contact`, `peak`,
`breakup`, and `release`. They are optional gameplay hooks, not extra frames.

## Pixel-perfect playback

- Disable smoothing, mipmapping, and lossy texture compression when crisp
  pixels are required.
- Draw at integer scale and integer positions.
- Preserve straight RGBA transparency.
- Use the manifest's original pivot consistently across frames.
- Treat `pixel_hash` as the identity of decoded canonical RGBA pixels, not of
  the PNG file bytes.
