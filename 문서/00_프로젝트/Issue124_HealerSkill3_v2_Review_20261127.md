# Issue #124 치유사 원화 v2 표적 검수

2026-11-27 · [리뉴얼 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7206417588) · [기획 검수 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7206496602)

**30px 핵심 항목 PASS. 24/20px 의미 판독 PARTIAL. 실제 상태 HUD 미검증. 본편 반영 HOLD.**

- **파일 PASS.** `LIMITLESS_Issue124_HealerSkill3_v2_Targeted_20261127.zip` 실물 SHA256 `3cd4ee38db090799ef9e1b1d8f9eb34abc338f7c4d8f8956ef28f3d17177c46d`. ZIP CRC 47/47, Manifest 경로·크기·SHA 45/45, SHA256SUMS 46/46. 두 후보 모두 128×128 투명 RGBA입니다. Reference에 보존된 v1 3종과 승인 비교본 6종도 기존 ZIP과 바이트 일치합니다.
- 치유의 빛 v2 SHA256 `80ba80300e069f9092de546f1333af750d9f7fcb51b85ff794a65f2b14fbeb01`.
- 정화 v2 SHA256 `850a23b528b0bebc8625829729a32991da671670e00ec82412ad0049ac70b731`.
- 회복의 파동 v1 KEEP SHA256 `fecfc5b86cd8d54c91bf5ac7fb2b13ee7dc604a0afe36d9ba5708925803b3593`. 기존 v1 납품 ZIP의 Art 원본과 바이트 단위로 동일합니다.

## 가독성

| 크기 | 판정 | 관찰 |
| --- | --- | --- |
| 40px | PASS | 치유의 빛의 단일 인물·집중광, 정화의 비대칭 균열·파편 형태가 두 배경과 흑백에서 분명합니다. |
| 30px | PASS | 같은 배경에서 라벨 없는 흑백 실크기로 직접 비교했습니다. 치유의 빛은 세로로 모인 밝은 인물형, 정화는 중앙의 어두운 틈과 비대칭 파편형으로 구분됩니다. 30px에서 상태 해제의 구체적 기능까지 독립적으로 전달하는지는 별개입니다. |
| 24px | PARTIAL | 밝은 세로 중심과 어두운 균열로 짝 구분은 가능하나 인물과 낱개 파편의 세부 판독이 약해집니다. |
| 20px | PARTIAL | 명암 덩어리는 다르지만 공통 외곽이 남고 중심 인물·상태 문양 파편의 의미는 라벨 지원이 필요합니다. |

## Unity 및 화면

Unity 6000.6.5f1 백그라운드 배치의 격리 Battle에서 첫 플레이어 턴을 기다린 뒤 기존 버튼 생성 경로로 두 버튼을 만들었습니다. 라벨을 비운 상태에서 128×128 Sprite 2종의 import 및 별개 표시를 확인했습니다. 실제 Canvas 캡처는 v1/v2 30px와 v2 40px입니다. 표적 검증 5 PASS, 종료 0. 기존 20 PASS 전수 재실행 없이 격리 소스·임시 자산을 원복했습니다. 합성 저장·설정 경로를 사용했습니다.

실제 상태 HUD는 미검증입니다. 24/20px 평가는 원화의 오프라인 실제 크기 비교입니다.

| 실물 (로컬 `Temp/i124-healer-v2-qa/`) | SHA256 |
| --- | --- |
| `Actual_v1_v2_Dark.png` | `b4c492404c5178cbadbb35b80743dd6c69e658f027031b3b2d0cbc84ca05fc32` |
| `Actual_v1_v2_Light.png` | `a334d7ae132807d574f7432cb16546243b41d96f717d628428626fc3193956f1` |
| `Unity_Battle_Comparison.png` | `8f325a4c38a6904a6f9bd6e0e44b11196721a2478e62b17dd647a4abd1ef7323` |

같은 폴더에 전체 Unity 캡처 3장과 `unity-active.log`, `file-qa.txt`를 보존했습니다. #85/#131/#132, 승인 자산, 원본 main, Save/UserData 보호. 본편 적용·main 병합·Push 없음. 미술 승인 대기.
