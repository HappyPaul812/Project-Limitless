# LIMITLESS / Issue #123 / F5 FrozenRift_Edge 파일럿 — 2026-11-24

[최신 기획 승인 및 발주](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7120274835), [Codex 개념 검수](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7120228919) (89dade05a51a73d68ee4488b04c952fb5e683ed8) 확인. F5 구상 기획 승인, 실제 Scene/통행 HOLD. 기존 승인 F3/F4 5종 KEEP.

## 실제 납품
- Art/F5_FrozenRift_Edge_384x256_RGBA_CANDIDATE.png: 384×256 투명 RGBA, SHA256 29d2eff4659aa3c4aad0b63cf8a7ae661409d111488f8e625042f1b6339dbd7c, 알파 bbox [8,30,376,248) 및 외곽 alpha0. 신규 생성 원화 1536×1024 RGBA SHA256 c3f93b0a9bf41c5078618ec30d3ee27dc6872a4bff7ec6ac376efc15719f24e2에서 alpha160..220을 0..255로 매핑해 광륜 제거, 비율 유지 축소(0.2643678161) 후 384×256 캔버스 배치, 낮은 알파<=5 제거. 단일 외곽 배치 균열 소품/오버레이, 반복 바닥·Collider·기믹 아님. 제안 128 PPU(캔버스 3×2WU), Pivot(0.5,0.03125), 실제 Import 미검증.
- 기존 승인 실물 PNG SHA256 작업 전후 일치: GroundTransition d63c8d5ae60921dc7007caf839b720d68dda8078f273b86f1404f6121475a4be, LowFrostRock ddbba749c8f60f3b207769c35883aa844e80e782e8dc512077608d2568f07bb1, FlushStone v2 e4d6af0f3aa854eeeef591091312ae593e2c928ed31123f4ca83cfe049b704b3, WhiteConifer 398b07335d84b0702a54e1bfae109bce75c60679f8e6a6fda6ccad6cd4386fad, DirectionalSnowdrift 1ae4ae973495d7d5c09efa3c7b6604233e6d4e66e74ca4d2d0e275e3bef630b1.
- Review/F5_F3_F4_Actual_1to1_30_40_Comparison.png: 실물 1:1 및 30/40px 폭 축소. Review/F5_Northbound_Placement_OFFLINE.png: 실제 PNG 축소 합성 도식, 남쪽 F4 입구→북쪽 출구, 북쪽 화살표, 풍설 방향 별도, 눈보라 정령→빙각 수호체 단독 조우 개념 표기. 중앙 통행로 도식 x344..624, 배치 bounds 침범0(실제 4.5WU 목표는 미실측). 실제 Scene/Collider/Sorting/실주행/좌표 미검증. 생성형 가상 비교판/배치도는 최종 검증에 미사용.
- P2 Permafrost_LayeredRock HOLD, 기존 LowFrostRock 재사용. 신규 게임 PNG 1개만.
- ZIP LIMITLESS_Issue123_F5_FrozenRift_Edge_Pilot_20261124.zip, 1,367,602 bytes / 7 entries, SHA256 1fc7e2c8edf4db21e445d1f20f6f7d2ec6e2be835a7b2dba44d096f4f5e4d96b. CRC PASS, Manifest5/5 및 SUMS6/6 경로·SHA·크기 검증, 순환참조 없음. QA/AssetQA.json, Docs/SourceDeclaration.md 포함.

## 상태
F5_CONCEPT_PLANNING_APPROVED / FROZEN_RIFT_EDGE_CANDIDATE / USER_ART_APPROVAL_REQUIRED / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED. 기존 승인 원화 수정0, 전체 맵/Scene/VFX/추가 암반/Unity6000.6.5f1 적용0. #85 HOLD, Save/UserData, Discord, main 보호. docs/art-new 문서·인계 및 Issue 댓글만, main 직접 commit/Push/Merge0.
