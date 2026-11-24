# LIMITLESS / Issue #123 / F3 FlushStone EdgeMarker v2 국소 색상 보완 — 2026-11-24

[기획 실물 검토 및 보완 발주](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7116769430) 확인. GroundTransition·LowFrostRock은 PLANNING_ART_PASS, F4는 PLANNING_CONCEPT_DIRECTION_PASS. 대표 최종 미술 승인은 별도.

## 원본 보호 및 신규 후보
Library 실물 3종 회수, 작업 전후 SHA256 일치:
- GroundTransition: d63c8d5ae60921dc7007caf839b720d68dda8078f273b86f1404f6121475a4be
- LowFrostRock: ddbba749c8f60f3b207769c35883aa844e80e782e8dc512077608d2568f07bb1
- FlushStone v1: ccb99fa51dec1c33d07eaca3fd0fede994b65f0fd5776c98cda690b197ffedc0

신규 후보 1개: Art/F3_FlushStone_EdgeMarker_v2_256x128_RGBA_CANDIDATE.png, 256×128 RGBA, SHA256 **e4d6af0f3aa854eeeef591091312ae593e2c928ed31123f4ca83cfe049b704b3**.
기존 v1의 중앙 푸른 표면을 수동 다각형 ROI 및 색상 가중치로 선택하여 명도 기반 저채도 회청색으로 보정. 앞서 생성한 별도 이미지 시안/가상 비교판은 실제 납품에 미사용. 리샘플링·재작화 없음. 변경 픽셀1613개, 변경 bbox [80,91,168,121) (반열림). ROI 밖 RGBA 동일, 전체 alpha 동일, 돌/눈의 형태·위치 유지. 변경 픽셀 RGB 최대-최소 평균 78.89→18.62 (채도 자체의 표준화 수치가 아닌 RGB 색차 지표). 기존 3종 원본 수정0.
실제 PNG에서 직접 40×20 및 30×15 RGBA로 축소, 원본 256×128 1:1 및 중앙부 3배 확대를 함께 비교판에 표기. 물웅덩이 오독 감소 기대, 실제 게임 배경 검증 NOT_RUN.

## 납품
LIMITLESS_Issue123_FlushStone_v2_Targeted_20261124.zip, 104505 bytes, 9 entries, SHA256 **31bea127a99135cfe0965fedbf6d3a3547d4f571359ad43435202202e30b5ec4**. CRC PASS, Manifest 7/7 경로·크기·SHA 일치, SHA256SUMS 8/8 PASS, 순환참조 없음. QA/TargetedColor_QA.json 포함. Review/F3_FlushStone_v1_v2_30_40_Review.png 및 실제 축소 PNG 4종 포함. v1 원본은 기존 Library에서 보존. 새 후보 미술 승인 필요 YES.
기존 GroundTransition 단일 오버레이 전용, LowFrostRock 길 밖 배치. F3 실제 4.5WU 통행·Collider·Sorting·PPU 및 v2 Unity import NOT_VERIFIED. 이전 격리 Import3/3 PASS를 v2에 전이하지 않음. F4 기획 방향 PASS이나 신규 F4 PNG/VFX/Scene 제작0. Unity6000.6.5f1/#85/Save/UserData/Discord/기존 승인 아트 보호. Unity 실행·본편 적용·새 Codex 발주·main 직접 commit/Push/Merge0.
