# Issue #123·#124 · Chapter1 전리품 2종과 LastWarmth IceBackdrop 표적 검수

2026-11-24 · 보고 전용 · `codex/issue124-loot2-attack-icon-20261124`

[잡템 기획](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7121604210) · [빙벽 기획](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7121661390) · [직전 취소 버튼 제품·QA](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7121719099)

| 다운로드 실물 | ZIP SHA256 | ZIP CRC / Manifest / SUMS |
|---|---|---|
| LIMITLESS_Issue124_Chapter1_Loot2_Original_20261124.zip | 907f6a188cdc1eca792f2026ef2156b0696ae38d53b76b273a9555630d2d2f39 | 29/29 · 27/27 · 28/28 |
| LIMITLESS_Issue123_LastWarmth_IceBackdrop_Pilot_20261124.zip | ad909b7b54f51caf5f6d112f176a5db759505ef2e47ab8b6719ff39f7db460ab | 7/7 · 5/5 · 6/6 |

**파일 PASS** · 납품 ZIP 및 후보 PNG SHA256 일치. Manifest 파일 목록·크기·해시와 SHA256SUMS의 전 항목, 모든 PNG 청크 CRC 및 디코딩 확인.

## #124 Chapter1 잡템

- **ForestSpiderSilk 미술 PASS** · `af0270104e6f9a18230fdfc12367c12b4b4068bd0e385b9f1e2151985802369a`. 128×128 투명 RGBA, 알파 바운드 [6,9,122,118). 숲거미의 이끼·갈색 톤을 받는 밝은 섬유 묶음입니다. 기존 실사용 `spider-web`의 펼쳐진 거미줄 도형과 구별되고 채집 재료로 읽힙니다. 30px에서 가는 실 가닥은 뭉치지만 실 묶음 실루엣은 유지됩니다.
- **VenomSnakeScale 원색 미술 PASS / 현재 Tint PARTIAL** · `ab5233986383622101e0b3b168cabf6050c6d1cb1b13a34606f7beb81e74eea2`. 128×128 투명 RGBA, 알파 바운드 [8,6,120,122). 맹독뱀의 흑갈색과 금색 띠가 겹친 비늘 조각에서 읽힙니다. 기존 실사용 `dorsal-scales`는 평면 흰 무늬이며, 후보는 겹침과 앞뒤 음영이 있습니다. 30px에서 일부 면이 합쳐져도 독침·방패와 구별되는 조각 형태가 남습니다. 현행 녹색 Tint의 오프라인 모의 결과에서는 금색 띠와 갈색 재질이 약해지고 어두운 배경에서 30px 판독성이 낮아집니다. 원색 기준 판정이며 Tint 설정은 변경하지 않았습니다.
- 30/40/50px 실제 축소 6장이 원본 PNG 재축소와 픽셀 일치. 숲거미 #90 Attack00·맹독뱀 #97 Idle00은 기존 납품 ZIP과 바이트 일치. 승인 침·젤은 격리 제품 PNG와 바이트 일치. 신규 2종의 재질 하이라이트와 입체 음영이 침·젤과 이어지고 네 재료의 외곽은 구별됩니다. 기존 ItemDefinition의 GUID로 확인한 외부 PNG를 직접 비교했습니다.
- **HOLD** · 최종 미술 승인과 적용. Unity 및 게임 데이터 변경 없음.

## #123 LastWarmth IceBackdrop

- **파일·미술 방향 PASS** · `5abfe30fcb1a09579a7617fe1df08079c0b83eeb982677cf57ec2bfc5166b6d5`. 512×384 투명 RGBA, 알파 0–255, 바운드 [8,95,504,374). 층진 회청 빙벽과 고드름이 F3의 설암, F4의 설면 명암, F5의 균열과 이어집니다. F5의 낮고 비스듬한 균열보다 높고 가파른 벽으로 북쪽 보스 구역에 위압감을 줍니다. F3~F5 승인 환경 6종과 설빙의 폭군 B 1포즈·Idle4의 실물 SHA 8/8 확인.
- **개념 배치 PASS** · 남쪽 F5 입구 → 북쪽 보스 목표, 북쪽 화살표 유지. 빙벽은 목표 뒤 북쪽 외곽에 있으며 중앙 통행로와 배치 상자 11개를 다시 계산해 교차 0을 확인했습니다. 4.5 WU는 미실측 목표입니다. 제안 PPU 128 및 하단 중심 Pivot (0.5,10/384)은 확인했고 실제 Sorting은 미검증입니다.
- **화로 실물 대비 PARTIAL** · 개념 합성에서 희미한 금빛 자리표시와 차가운 벽이 구분됩니다. 승인 보스 실루엣은 전경에서 읽히며 밝은 얼음과 겹치는 밝은 고드름·뿔 가장자리의 최종 분리는 실제 카메라·조명·Sorting 확인이 필요합니다. 기존 화로 원화는 미확보이며 현재의 금빛은 도식입니다. 온기 오버레이, 파일럿 최종 승인, 실제 Scene·Unity 적용·실주행·보스전 구현 **HOLD**.

로컬 검수 증거 `Temp/i123124-loot-ice/`. 제품·ItemDefinition·GUID/meta·드롭·판매가·수치·#85·원본 main·Save/UserData·Discord 보호. 제품 반영, 병합 및 원격 Push 없음.
