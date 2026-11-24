# LIMITLESS / Issue #123 / F5 동토의 균열 북진 환경 구상 — 2026-11-24

[최신 기획 발주](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7119304884) 및 [Codex F4 검수](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7119159890) 확인. F3 GroundTransition·LowFrostRock·FlushStone v2와 F4 WhiteConifer·DirectionalSnowdrift 최종 미술 승인 KEEP. F4 파일/격리 Import PASS, 배치 PARTIAL, 실제 정식 Scene/이동/제품 적용 HOLD. F5_CONCEPT_ONLY / USER_CONCEPT_APPROVAL_PENDING.

## 환경 구상
남쪽 F4 입구 → 북쪽 후속 보스 구역 방향 출구의 **세로 주동선** 고정. 구상도 북쪽 화살표·남북 입출구·북진 가이드 명시, 풍설 방향 별도. 실제 후속 Scene ID·좌표 미확정. 중앙 밝은 설면, 4.5WU 유효 통행폭 목표(실측 아님), 균열·수목·암반·풍설 외곽 배치. F4 대비 침엽수 밀도 감소, 노출 동토와 깊은 균열, 차가운 침묵 강조. 후보색 F4 #DCEAF0 → F5 통행로 #D3E4EE / 동토 #8CA4B7 / 균열 외측 #536E88 / 깊이 #24394F. 균열은 명암·형태로 구별하며 경로를 차단하거나 새 미끄럼/강제이동/냉기 효과를 추가하지 않음. 보스 전용 동결괴사/화로 연출 선취 금지.

실제 F3/F4 승인 원본 5종 읽기 전용 회수, SHA256 일치 및 작업 전후 보존:
- GroundTransition d63c8d5ae60921dc7007caf839b720d68dda8078f273b86f1404f6121475a4be
- LowFrostRock ddbba749c8f60f3b207769c35883aa844e80e782e8dc512077608d2568f07bb1
- FlushStone v2 e4d6af0f3aa854eeeef591091312ae593e2c928ed31123f4ca83cfe049b704b3
- WhiteConifer 398b07335d84b0702a54e1bfae109bce75c60679f8e6a6fda6ccad6cd4386fad
- DirectionalSnowdrift 1ae4ae973495d7d5c09efa3c7b6604233e6d4e66e74ca4d2d0e275e3bef630b1

구상도는 실물 자산을 미리보기용 축소 합성하고 균열·지면을 도식으로 표현. 배치 사각형과 도식상 중앙 통행로 침범0. 실제 도보·휠체어 실주행/Collider/Sorting/Scene 연결 NOT_VERIFIED. 정본 전투 기획 유지: 눈보라 정령 단독 → 빙각 수호체 단독 별도 구역, 회피 공간/입출구 조우 금지, 엘리트 단회·반복 길막 금지. 스폰 좌표·수치·퀘스트 변경0.

## 신규 후보 최대 2종 (제안만)
1. P1 FrozenRift_Edge 384×256 RGBA 제안: 어두운 틈과 눈 덮인 낮은 턱, 외곽 단일 배치. 반복 타일이나 충돌체 아님.
2. P2 Permafrost_LayeredRock 320×192 RGBA 제안: 낮은 층상 동결 암반. 기존 LowFrostRock 재사용으로 충분하면 생략. 실제 PPU/Pivot/Collider/Sorting 별도.
신규 게임용 PNG 제작0. 사용자 구상 승인 및 별도 제작 발주 필요.

## 납품
F5_Northbound_FrozenRift_Concept_OFFLINE.png (1500×1440), SHA256 5f06d3353a47a8955bf454cd1bb7e0843e24a054ee3a7fe79945efec35c03110. F5_Environment_Proposal.md, QA.json, Manifest.json, SHA256SUMS.txt.
ZIP LIMITLESS_Issue123_F5_FrozenRift_Concept_20261124.zip, 1,464,477 bytes / 5 entries / SHA256 d42e2c730ed622525fbf4ca065c9b9f027e0b313db077b1656599105696925ba. CRC PASS, Manifest3/3, SUMS4/4 PASS, 순환참조0.
기존 원화 수정·맵/타일/Scene/Prefab/VFX 제작·Unity6000.6.5f1 실행/적용·새 Codex 발주0. #85 HOLD, Save/UserData, Discord, main 보호. docs/art-new 문서·인계 및 #123 댓글만. main 직접 commit/Push/Merge0.
