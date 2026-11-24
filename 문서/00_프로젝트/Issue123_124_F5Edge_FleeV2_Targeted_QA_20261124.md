# Issue #123·#124 표적 검수 · 2026-11-24

**Flee v2 파일·보완 효과·격리 Unity PASS. F5 파일·아트 방향·개념 배치 PASS, 승인 참조 원본 확인 PARTIAL. 두 후보의 최종 미술 승인과 본편 적용 HOLD.**

- 최신 기획 [#124 도망 국소 보완](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7120275605) · [#123 F5 단일 외곽 소품 발주](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7120274835). 최신 납품 [#124](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7120356707) · [#123](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7120586776).

| 납품 ZIP (로컬 다운로드 실물 확인) | ZIP SHA256 | 후보 PNG SHA256 | ZIP CRC / Manifest / SUMS |
|---|---|---|---|
| LIMITLESS_Issue124_FleeButton_v2_Targeted_20261124.zip | 931274626db9230cdaeb63e191100c68a43af14e8b50969c0ed26608ecb74fed | 58d7f66283031737f65069df6fb281a818d8c32e6ae7a6230b2f5e53783f5d73 | 15/15 · 13/13 · 14/14 |
| LIMITLESS_Issue123_F5_FrozenRift_Edge_Pilot_20261124.zip | 1fc7e2c8edf4db21e445d1f20f6f7d2ec6e2be835a7b2dba44d096f4f5e4d96b | 29d2eff4659aa3c4aad0b63cf8a7ae661409d111488f8e625042f1b6339dbd7c | 7/7 · 5/5 · 6/6 |

모든 PNG 청크 CRC 및 디코딩 PASS. Manifest 파일 목록·바이트 수·해시와 SUMS 전체 범위를 대조했습니다.

## #124 FleeButton v2

- **PASS** · 128×128 투명 RGBA. v1 청색 원호와 내부 광휘를 제거하고 직선 상단 문틀과 어두운 내부를 넣었습니다. 30/40px에서 문 바깥으로 향하는 화살표가 읽히고 포털 인상이 줄었습니다. 우향 은색 화살촉, 금색 기둥·문턱, 금속 광택이 유지됩니다. 공격·방어·스킬 승인 PNG 3종은 이전 격리 제품 파일과 바이트 일치합니다.
- 가시 픽셀 5,362개 변화. 동일 캔버스에서 알파 바운드는 v1 [5,5,123,122) → v2 [19,20,123,113)입니다. 원호가 빠져 30px에서 도형의 점유 면적이 작게 보이지만 문틀과 우향은 구분됩니다.
- **격리 Unity PASS** · Unity 6000.6.5f1의 기존 호환 QA 복제본에서 런타임 Sprite만 임시 교체했습니다. Battle UI 30/40px 렌더 및 활성/비활성, 취소 후 v2 복귀, 일반전 도망 성공 후 Field_01 복귀 확인. 배치 종료 0. 보스전 제한 미실행.
- 로컬 증거 `Temp/i123124-target/Flee_Unity_v1_v2.png` SHA256 `6627cf8e921ac97dac3c668401c4439b2f12980ce37cc829bc9605d9d421f1b8`, `unity-retry.log` 1032–1216행. 최초 배치는 라이선스 IPC 시간 초과. 숨김 배치 재시도로 완료했습니다.

## #123 F5 FrozenRift Edge

- **PASS** · 384×256 RGBA, 바깥 테두리 완전 투명. 알파 범위 0–255, 바운드 [8,30,376,248), 반투명 3,280픽셀. 비교판에서 외곽 광륜이 보이지 않습니다. PPU 128이면 캔버스 3×2 WU, 보이는 범위 약 2.88×1.70 WU. Pivot (0.5, 0.03125)는 하단 중앙에서 8px 위로 알파 하단 여백과 일치합니다. 길 밖 단일 소품용 제안으로 적절합니다.
- F3의 둥근 설암·고드름·위에서 내려다보는 시점과 이어집니다. F4 밝은 풍설과 침엽수 사이에서 짙은 회청색 깊이와 날 선 고드름이 더 차갑고 위험하게 읽힙니다. F3 저상 암반 320×192보다 큰 384×256 비례가 낙차를 구별해 줍니다. **PARTIAL** · 승인 원본 5종 중 F3 경계석과 F4 2종 실물 SHA 일치. F3 지면 전환과 낮은 암반은 납품 비교판과 승인 댓글의 해시로 확인했으며 독립 원본 파일을 확보하지 못했습니다. [F3 승인 기록](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7118370238)
- **개념 배치 PASS** · 남쪽 F4 입구 → 북쪽 출구의 세로 동선, 별도 북쪽 화살표·바람 방향, 입출구 표기 확인. 도식의 중앙 x=[344,624)에 배치 사각형 13개 중 침범 0, 최소 여유 18px. 4.5 WU는 목표 폭입니다. P2 추가 암반 및 실제 Scene/Collider/Sorting/실주행은 HOLD. 로컬 증거 `Temp/i123124-target/F5_Northbound_Placement_OFFLINE.png`, `F5_F3_F4_Actual_1to1_30_40_Comparison.png`.

보고 브랜치 `codex/issue124-loot2-attack-icon-20261124`. 기존 승인 자산·#85 후보·원본 main·Save/UserData·Discord 보호. QA 저장 경로는 합성 데이터용으로 분리했습니다. 제품 적용·병합·원격 Push 없음.
