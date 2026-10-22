# LIMITLESS / Bear Run6 v3 실제 납품 (2026-10-22)

## 지시 및 보존
Codex v2: technical PASS, motion-art HOLD. Fix focus 03→04 landing/weight support.
Approved Wolf Run6 v5, other assets, game runtime, main, and original Unity Scene untouched.
v2 ZIP SHA256: 3d74103e01fcd9f17127afcecf52eaa9f578af96f8fa23a197a1237681c3c147

## 실제 PNG 수정
03/04 replaced using two genuinely different poses from separate generated alternate art sheet. 03 shows recovery in the air; 04 lands on forefoot and rear support. V2 00/01/02/05 compressed PNG bytes preserved. Remaining gait realism is not approved.
RGBA changed pixels by frame 00..05: [0,0,0,77049,91942,0]
Six individual 512x384 RGBA LEFT frames; 3072x384 strip; 1536x768 3x2 atlas; 12 FPS 6-frame 500ms GIF/APNG. 128px and 40px contact sheets, v2/v3 comparison, transition and loop reference.

## 실제 v3 ZIP
Filename: Limitless_Beast_Bear_Run6_v3.zip
Size: 5554445 bytes. ZIP SHA256: adc3f36a65c9bc093cf2c1b3e68b7da3abe5d68c4edac461096fb1611f4a0a35. Entries: 24.
Actual downloadable file is in current ChatGPT conversation sandbox:/mnt/data/Limitless_Beast_Bear_Run6_v3.zip. ZIP binary not uploaded to GitHub and not automatically copied to user's F: drive.
Assets/Poses/ contains 6 independent PNG frames; Assets/ contains strip and atlas. Preview/ contains GIF and APNG. Manifest.json, SHA256SUMS.txt, QA/Offline_Validation.json and QA/verify_bbox.py included.

## bbox CSV repaired based on actual PNG
File: QA/Frame_Gait_And_Contact.csv. Six rows re-extracted from actual PNG alpha A>0. Top-left 0-based image coordinates, x toward right, y downward. Header and ordering: bbox_min_x, bbox_min_y, bbox_max_x_exclusive, bbox_max_y_exclusive. Six per-package CSV rows exactly rechecked against original PNG alpha, combined Bear+Fox twelve rows PASS. Non-target v2 frames were not regenerated merely to fix CSV.

## Integrity QA and approval gate
PNG 6/6 decode and correct RGBA dims. Six distinct frame pixel hashes. 3px outer transparent border PASS. Strip/Atlas exact frame pixel match 6/6. PNG SHA in Manifest and SHA256SUMS. ZIP zipfile.testzip() CRC PASS; entries SHA readback PASS. 12FPS GIF/APNG 6 frames total 500ms.
TECHNICAL OFFLINE PASS IS NOT MOTION-ART PASS. Codex isolated Unity 12FPS change-only QA and representative artistic approval are still pending, independently for Bear. No game asset replacement, no main push/merge.