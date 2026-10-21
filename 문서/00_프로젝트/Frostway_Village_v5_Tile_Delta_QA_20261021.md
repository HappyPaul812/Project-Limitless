# 서릿길 마을 GameArt v5 — 변경 타일3 Delta QA

## 결과

**파일/PNG 기술 PASS · 타일3 미술 HOLD · OPEN16 원본 확보/해결0 · 본게임 반영0 · Main23 구현0**.

브랜치 `codex/frostway-v5-tile-delta-qa-20261021`, 기준 main `e4e9891f6f01ea95cada9e63b1f09946d94d6041`. Wolf 반영 브랜치와 별도의 worktree입니다. 제품 Unity/Assets 변경0이며 검수 문서와 실제 렌더 증거만 기록합니다. 최종 commit은 이 브랜치 완료 보고를 따릅니다.

GitHub 최신 main·docs/planning 인계(blob83729d23)·Issue43/45 최신 댓글과 art-new v5 납품 보고(blob9f107691)를 확인했습니다. 기존 Village 전체 QA 및 Main22 136/F1 64/서리들쥐40은 재실행하지 않았습니다.

## 실제 납품물 검사

- ZIP SHA256 **`1affc1a4c23f3bcc6a9e9e18924b08bce3134eab065d8362757725f96e7f71b4`**, 신고값 일치. 20,085,423bytes·72entries·CRC오류0·Manifest43/43·SHA256SUMS71/71.
- 게임 PNG43종 decode 정상. 수정3/원본바이트동일40, SkillShop/RoofGable v4 및 돌길 포함 KEEP40 SHA를 기록했습니다. ZIP 재압축/PNG 재저장/원화 수정0.
- 수정3은 모두256×256 RGBA, alpha255. 외곽5px 변경0, 좌우·상하 경계 RGBA 배열 동일, 코너 연결 유지. 이 수학적 Seam PASS를 반복무늬 미술 PASS와 구분합니다.

| 파일 (`Tiles/` 기준) | 변경 RGBA 픽셀 | v5 SHA256 |
|---|---:|---|
| `Frostway_Ground_Earth_01.png` | 24,062 | `d1d8a30b26ebb63bb43a7b32cc5498647eee6b9183fd797cd890da20c417790c` |
| `Frostway_Ground_Gravel_01.png` | 31,120 | `1f23f01d53c213d74e70cdc46486ae7c3c2a32232df0623bf30a56bd8a87cc4f` |
| `Frostway_Ground_ThinFrost_01.png` | 23,973 | `2810ff9ba199b77832e033c179dc3740f06d6257b5072833b6c9c3f66fc15ce6` |

변경 bbox(좌상단 기준, 우·하 exclusive): Earth/ThinFrost `[5,58,251,251]`, Gravel `[17,7,236,251]`. [패키지 검사 JSON](Evidence/FrostwayV5DeltaQA20261021/PackageDelta.json)에40KEEP SHA 포함.

## 실제 Unity 비교

Unity6000.6.5f1 / Direct3D12 / 숨김 Batch Mode / 종료코드0 / 컴파일 오류0. QA 전용 Editor 도구에서 실제 Camera/RenderTexture로16장을 렌더했습니다. **Editor GameView UI 캡처·PlayMode 수동 입력과 구분**합니다. PNG는 입력 폴더에서 원본 bytes를 LoadImage하여 표시했으며 본게임 Importer나 제품 Scene을 수정하지 않았습니다.

- 16:9=1600×900,4:3=1200×900,21:9=2100×900. 두 버전 모두 PPU128,64px/WU,타일128px,필터Point,3×3그리드,배치/카메라/광원/Sorting 동일. Stone은 변경되지 않은 비교 기준입니다.
- 3종 반복 패널과 흙·서리·자갈→돌길/건물/출입구 연결을 같은 배율로 전후 비교했습니다. 바뀐 지면 때문에 생긴 새 투명 틈·Sprite 깨짐·문/건물 가림은 발견하지 않았습니다. 타일 간 재질 경계의 급격한 변화는 기존 연결 방식이며 블렌딩 시스템을 새로 구현하지 않았습니다.
- 지면만 변경한 동일 QA 배치에서 남북 길·여관/스킬상점 입구를 반경0.3(도보 proxy)/0.5(휠체어 proxy)로 검사, v4/v5 합계12/12 통과. Collider 새 생성은 QA proxy 전용이며 실게임 Collider 변경0. 원본 휠체어 외형과 앞/뒤 Sorting 샘플 유지. 실제 PlayerController 이동·OS 손플레이·동적YSorting 검증으로 확대하지 않았습니다.

| 타일 | 미술 의견 | 잔여 관찰 구간(원본256px,좌상단 기준) |
|---|---|---|
| Earth | **HOLD**. 큰 밝은 돌군집 강조는 줄었으나 풀·돌의 대각선/물결 무늬가128px마다 반복됨 | x0~88/y96~255, x92~181/y160~250 |
| ThinFrost | **HOLD**. 같은 군집 완화는 확인, 식생·돌 패턴 반복은 남음 | Earth와 같은 구간 |
| Gravel | **HOLD**. 밀도/큰 군집 대비 일부 완화, 둥근 자갈과 빈 영역이 규칙적으로 반복됨 | x4~117/y2~121, x80~159/y160~235 |

위 좌표는 잔여 모티프의 관찰 ROI이며 해당 영역 전체를 제거하라는 수정 계약이 아닙니다. 개선이 없다는 뜻은 아니며, 실제 게임 크기의 넓은 지면에서도 규칙성이 보여 최종 수용 판단을 보류했습니다. Codex가 최종 미술 승인을 대신하지 않습니다. 바울 대표가 현재 반복을 수용하거나, 수정3파일 내 국소 완화 범위를 결정할 수 있습니다. 신규 변형 타일/타일 배치 시스템은 별도 범위입니다.

[흙128px 실제Unity 비교](Evidence/FrostwayV5DeltaQA20261021/Compare_Earth_Unity128px.png) · [서리](Evidence/FrostwayV5DeltaQA20261021/Compare_ThinFrost_Unity128px.png) · [자갈](Evidence/FrostwayV5DeltaQA20261021/Compare_Gravel_Unity128px.png)

[반복16:9](Evidence/FrostwayV5DeltaQA20261021/Compare_Repeat3x3_16x9.jpg) · [4:3](Evidence/FrostwayV5DeltaQA20261021/Compare_Repeat3x3_4x3.jpg) · [21:9](Evidence/FrostwayV5DeltaQA20261021/Compare_Repeat3x3_21x9.jpg) · [연결16:9](Evidence/FrostwayV5DeltaQA20261021/Compare_Connections_16x9.jpg) · [연결4:3](Evidence/FrostwayV5DeltaQA20261021/Compare_Connections_4x3.jpg) · [연결21:9](Evidence/FrostwayV5DeltaQA20261021/Compare_Connections_21x9.jpg).

## OPEN16 — 원본 근거 확보

**EVIDENCE_FOUND / OPEN_UNCHANGED_BYTES16 / RESOLVED_BY_V5 0**.

원본 `Temp/JointVillageV3WolfV4QA20261021/Worktree/문서/00_프로젝트/Evidence/JointVillageV3WolfV4_20261021/A_PNG43_Review.csv` 및 짝 `A_DefectRects.json`을 확보했습니다. 이전 QA commit은 `695f6f74e63996bb9d0ba0561dc6518f1e7af049`입니다. 원본은18개 ART_FAIL이며 SkillShop/Gable2개가 v4에서 해결되어16개가 남았습니다.

원본 CSV는 각 항목의 `format_tech=PASS/art=FAIL/use=FAIL`과 foreign-source-fragment 사유를 기록합니다. 좌표는 CSV 자체에 없고 동반 원본 JSON에 있습니다. **P1/P2·심각도 등급·상세 재현 절차는 원본에 미기재**이므로 추측해 채우지 않았습니다. 아래 심각도는 기존 '미술 FAIL/게임 사용 FAIL' 수준으로만 이월합니다. 재현은 독립 PNG를 대비 배경에 표시하고 원본 JSON의 픽셀 ROI를 확대 관찰하는 방식입니다(이번 근거 정리의 관찰 절차). 실제 Scene에서 노출되는 정도는 배치/Sorting에 따릅니다.

16종 모두 현재 v5 SHA가 원본 CSV와 동일합니다. 따라서 이번 타일3 수정으로 해결되지 않았음을 byte 동일성으로 확인했으며, 전체16개 미술을 새로 전수 판정하거나 임의 보정하지 않았습니다. **아래 좌표는 우·하 exclusive**입니다.

| 파일 | x 범위 | y 범위 | 기존 판정 / v5 대조 |
|---|---:|---:|---|
| `Buildings/Frostway_GeneralShop_Building.png` | [12,14) | [254,301) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Buildings/Modular/Frostway_Door_StoneArch.png` | [12,13) | [137,191) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Buildings/Modular/Frostway_Door_StoneFrame.png` | [12,25) | [33,181) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Buildings/Modular/Frostway_Door_WoodFrame.png` | [12,24) | [31,182) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Buildings/Modular/Frostway_Roof_Shingle_B.png` | [12,39) | [34,188) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Buildings/Modular/Frostway_Wall_Plain.png` | [12,16) | [136,187) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Portal_Inactive/Frostway_Portalsite_LowWall.png` | [12,18) | [136,152) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Portal_Inactive/Frostway_StonePlinth_Short.png` | [21,235) | [12,112) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Props/Frostway_Bench.png` | [12,17) | [108,127) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Props/Frostway_Fence.png` | [270,435) | [12,65) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Props/Frostway_SupplyBox_Food.png` | [12,32) | [81,136) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Props/Frostway_SupplyCrate_Brace.png` | [12,20) | [124,159) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Props/Frostway_SupplyCrate_Small.png` | [12,36) | [13,129) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Props/Frostway_SupplyCrate_Wood.png` | [12,37) | [28,157) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Props/Frostway_SupplySacks.png` | [12,68) | [12,21) | ART_FAIL/USE_FAIL · SHA동일 OPEN |
| `Props/Frostway_Washline.png` | [104,228) | [12,23) | ART_FAIL/USE_FAIL · SHA동일 OPEN |

[원본 CSV](Evidence/FrostwayV5DeltaQA20261021/A_PNG43_Review.csv) · [원본 좌표 JSON](Evidence/FrostwayV5DeltaQA20261021/A_DefectRects.json) · [행별 대조 CSV](Evidence/FrostwayV5DeltaQA20261021/OPEN16_Reconciliation.csv) · [원본 좌표를 표시한 v5 참고판](Evidence/FrostwayV5DeltaQA20261021/OPEN16_OriginalRects_v5.png). 원본 파일 SHA와 항목별 재현·미기재 한계는 대조JSON에 있습니다.

## 보호·남은 승인

A의 승인 Wolf6파일 원본 반영이 끝난 시점의6745파일을 B 기준으로 삼아 전후 SHA 변경0을 확인했습니다. B의 원본 Unity/Save/UserData/Editor 조작·제품 신규 자산0. 사용자 포커스 변경0. B main Commit/Push/merge0, 제품Scene/Quest/Main23 구현0. 원본 ZIP 불변입니다.

남은 것은 바울 대표의 타일3 미술 수용/국소 수정 범위 결정 및 근거를 확보한 OPEN16의 처리 우선순위 결정입니다. 마을 전체 GameReady·최종 미술 승인·본게임 적용 완료로 선언하지 않습니다.
