# 2026-11-24 · Issue #123 F5 구상 및 #124 기본 도망 버튼 기술 검수

**F5 개념 PASS / 구현 HOLD · 도망 버튼 파일·Unity 기능 PASS / 포털과의 시각 구분 PARTIAL / 미술 승인·본편 적용 HOLD.** [F5 발주](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7119304884) · [F5 납품](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7119593774) · [도망 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7119871416)

| 납품 | ZIP SHA256 | PNG SHA256 | 검증 |
|---|---|---|---|
| F5 구상도 1500×1440 | `d42e2c730ed622525fbf4ca065c9b9f027e0b313db077b1656599105696925ba` | `5f06d3353a47a8955bf454cd1bb7e0843e24a054ee3a7fe79945efec35c03110` | CRC 5/5, Manifest 3/3, SUMS 4/4 |
| 도망 아이콘 128×128 투명 RGBA, 알파 0–255 | `2d406c8556e2eef67b0b6d3cceab5868ee0804036490981a273eada238221f91` | `b8bb8399c52ea6992810ce93eb1b56198817f789cae431a5313c855cd4bce054` | CRC 12/12, Manifest 10/10, SUMS 11/11 |

## F5 동토의 균열

- 화면 아래 F4 입구에서 위쪽 후속 보스 구역 방향 출구까지 밝은 세로 통행면이 이어집니다. 북쪽 화살표, 상·하단 표지, 중앙 진행 화살표가 명료합니다. 가로 풍설 화살표는 외곽에 따로 표시되어 있습니다.
- F4보다 수목을 성기게 두고 청회색 동토와 어두운 이중 균열선을 노출해 더 차갑고 위험하게 읽힙니다. 밝은 통행면과 낮은 균열 테두리로 경로와 위험을 구별합니다. F3 눈·암석 접합, 저상 암반, FlushStone v2 및 F4 수목·풍설의 재사용은 이 전환에 자연스럽습니다. F4 두 원화와 F3 FlushStone v2 실제 해시를 대조했습니다. 나머지 F3 두 원화는 납품 QA의 참조 해시와 구상도 재사용 표기로 확인했습니다.
- 균열은 통행면을 가로지르지 않으며 도보·휠체어가 같은 중앙 경로를 이용하는 구상입니다. 4.5 WU는 목표 폭입니다. P1 균열 가장자리 및 선택 P2 층상 암반의 2종 제안이며, 신규 게임 PNG는 없습니다.
- 정식 맵·좌표·Collider·Scene 전환·실주행은 미검증이며 F5 Unity 적용 및 실행은 하지 않았습니다. F5 개념 최종 승인 및 구현 HOLD.

## 기본 도망 버튼

- **기술 PASS** · 승인 공격·방어·스킬 참조 PNG 3개가 기존 제품 격리 브랜치와 바이트 일치합니다. 금속 장식·윤곽·광택이 기존 아이콘과 이어집니다. 30/40px 실제 축소 및 Unity 화면에서 출구 방향이 읽히며, 기존 취소의 좌향 화살표와 구별됩니다. Kenney의 단순 출구 아이콘은 30px에서 화살표가 더 선명합니다.
- **의미 PARTIAL** · 후보의 금색 아치와 청색 광휘는 특히 30px에서 포털처럼 읽힐 여지가 있습니다. 실제 전투의 ‘도망’ 레이블이 의미를 보완합니다. 별도 포털 UI와의 직접 동배율 대조나 사용자 판독 검증은 미실행입니다.
- **Unity 기능 PASS** · Unity 6000.6.5f1 기존 호환 QA 복제본에서 실제 Battle의 도망 Image만 실행 중 잠시 교체했습니다. 기존 버튼 레이아웃을 유지한 30/40px 배율, 대상 선택 중 비활성, 비활성 클릭 차단, 취소 후 활성 복귀, 클릭 후 전투 종료 및 기본 Field_01 복귀를 확인했습니다. 기존 승인 버튼 3개 Sprite 표시 유지. 보스전 도망 제한은 코드를 변경하지 않았으며 별도 실행 검증은 하지 않았습니다. 배치 종료 0.
- 검수 화면 `Temp/i123124-f5-flee/Flee_Unity_30_40_Comparison.png` SHA256 `1fa93df8f161c97f053fa65f5221e773a5c0c69f76e168a4b420ba76d722fa3b`, F5 원본 `F5_Northbound_FrozenRift_Concept_OFFLINE.png`, 로그 `unity.log` 578–1134행.

기존 제품·승인 원화·#85 후보, 원본 main과 Git 상태를 보존했습니다. 검증은 합성 Save/Settings 경로를 사용했습니다. 제품 반영·맵 생성·main 병합·원격 Push 없음.
