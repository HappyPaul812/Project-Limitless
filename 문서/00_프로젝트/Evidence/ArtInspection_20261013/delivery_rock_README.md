# LIMITLESS Field03 Rock B — v2 최소 변경 보완

- **원본 보존:** 승인된 Field03 v1 ZIP 및 Unity Scene/원본 Sprite GUID/meta는 변경하지 않았습니다.
- **수정 근거:** `문서/00_프로젝트/Evidence/Field03Official_20261012/RockB_Art_Correction_Request.md`.
- **변경 사항:** 상단 분리 불투명 픽셀 14개 제거; 원본 바위 본체 **3,537개 픽셀을 RGBA 값 그대로** 내부에서 위로 19px 이동. 색조·형태 재제작 없음.
- **실사용 파일:** `Props/LL_C1_F03_Rock_Moss_B_128_v2.png` (128×128 RGBA; 권장 PPU128, Center Pivot).
- **교체 대상:** `Field_03`의 `RockNorth` **한 개 참조만**. 좌표 (4.5, 4.4), Collider size(.4,.22), offset(0,-.22) 유지.
- **계산상 접지:** 기존 본체 하단 월드 3.92344 → 수정 4.071875. Collider 하단 4.07; 차이 0.001875 world.
- **주의:** 이 값은 PNG 좌표·문서에 따른 기하학 계산입니다. 실제 Importer, Scene Scale, Sorting, Collider 가림 및 플레이 동선은 Codex 격리 Unity에서 검증해야 합니다.
- **사용권/출처:** 사용자 승인된 기존 Field03 v1 원본 PNG에서 불필요 픽셀만 제거하고 본체 위치를 변경했습니다. 외부 이미지 픽셀은 추가하지 않았습니다. 원본 아트의 독립적 상업권 증빙 여부는 별도 **NOT_VERIFIED**입니다.
- **최종 승인:** USER_ART_REVIEW_PENDING. 승인 후 Codex가 적용하며 전체 375개 승인 Sprite 참조를 재연결하지 않습니다.
