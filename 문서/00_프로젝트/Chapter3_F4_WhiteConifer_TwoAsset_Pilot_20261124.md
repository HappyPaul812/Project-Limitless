# LIMITLESS / Issue #123 / F4 백색 침엽림 환경 2종 파일럿 — 2026-11-24

[대표 F3 최종 승인 및 F4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7118370238) 확인. GroundTransition, LowFrostRock, FlushStone v2 F3 사용자 최종 미술 승인. F3 실제 Scene/통행 및 v2 Unity Import 미검증 유지. 기존 원본 및 v1 보호. F4 두 후보만 신규 제작, 대표 미술 승인 대기.

## 실제 납품
- Art/F4_WhiteConifer_384x512_RGBA_CANDIDATE.png: 384×512 RGBA, bbox [22,10,361,502), SHA256 398b07335d84b0702a54e1bfae109bce75c60679f8e6a6fda6ccad6cd4386fad. 제안 pivot (0.5,0.01953125), 길 밖 배치.
- Art/F4_DirectionalSnowdrift_384x192_RGBA_CANDIDATE.png: 384×192 RGBA, bbox [10,26,374,165), SHA256 1ae4ae973495d7d5c09efa3c7b6604233e6d4e66e74ca4d2d0e275e3bef630b1. 최대 alpha168/255, 제안 pivot (0.5,0.5), 단일 배치 풍설 오버레이. 반복 타일/Collider/동적 VFX 아님.
- 신규 생성 원화 각각 1254×1254 및 1774×887에서 알파 잡음 제거·비율 유지 축소·투명 캔버스 정규화. 원화 전체 재작화 없음. 최종 비교판은 기존/신규 실물 PNG 직접 합성. 생성된 가상 비교판은 미사용.
- 기존 F2 승인 그루터기와 F3 승인 3종 및 FlushStone v1 SHA256 작업 전후 동일. F2 그루터기 SHA 22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8. F3 GroundTransition d63c8d5ae60921dc7007caf839b720d68dda8078f273b86f1404f6121475a4be, LowFrostRock ddbba749c8f60f3b207769c35883aa844e80e782e8dc512077608d2568f07bb1, FlushStone v2 e4d6af0f3aa854eeeef591091312ae593e2c928ed31123f4ca83cfe049b704b3, v1 ccb99fa51dec1c33d07eaca3fd0fede994b65f0fd5776c98cda690b197ffedc0.
- Review/F4_F2_F3_Actual_1to1_30_40_Comparison.png: 실제 1:1 원본 캔버스 비교 및 30/40px 폭 축소. Review/F4_AccessibleRoute_Concept_NOT_VERIFIED.png: 실제 납품 PNG의 개념 배치, 55% 도식 배율, 중앙 밝은 통행로 및 길 외곽 수목/풍설. 4.5WU 목표는 실측 아님. F3 암반/표식/전환 오버레이 재사용.
- ZIP LIMITLESS_Issue123_F4_WhiteConifer_2Asset_Pilot_20261124.zip, 1110832 bytes / 8 entries / SHA256 cc011d31e8af7bccccc4ea7379b78c58db62e72b8bfb3b4ab5b4cf5564ee4a54. CRC PASS, Manifest6/6, SHA256SUMS7/7 PASS, 순환 참조0. QA/AssetQA.json, Docs/SourceDeclaration.md 포함.

## 범위
F4_TWO_ASSET_CANDIDATE / USER_ART_APPROVAL_REQUIRED / UNITY_NOT_APPLIED. 신규 게임 PNG 2종뿐. 기존 F2/F3 변경0, 전체 맵/추가 VFX/Scene/Unity 적용0. PPU/Pivot/Collider/Sorting/4.5WU 실제 통행 미검증. Unity6000.6.5f1/#85 HOLD/Save/UserData/Discord/main 보호. 담당 docs/art-new 문서와 Issue 댓글만, main 직접 commit/Push/Merge0.
