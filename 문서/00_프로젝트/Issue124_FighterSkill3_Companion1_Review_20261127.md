# Issue #124 투사 기본 스킬 3종과 동료의 습격 원화 표적 검수

2026-11-27 · [기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7207318682) · [정정 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7207482757)

**종합 PARTIAL · 본편 HOLD.** 파일·계열 화풍·Unity 버튼 표시 PASS. 30px 무라벨에서 회오리의 회전 참격과 동료의 습격의 종족 중립 표현은 확인했습니다. 난도와 회심의 일격의 안정적인 구분은 PARTIAL입니다.

## 파일 및 화풍

- 실물 `F:/Downloads/LIMITLESS_Issue124_FighterSkill3_Companion1_20261127.zip` SHA256 `fd7a2058b425795a85147643bd7d163ce77c2f8c484ae70133ee63d9460f3e42`, ZIP CRC 60/60, Manifest 경로·바이트·SHA256 58/58, SHA256SUMS 59/59. 후보 4종 128×128 투명 RGBA. 참조 11종은 직전 납품 ZIP의 원본과 바이트 일치합니다.
- 난도 `f59f7e0413009991874ae8753a65f11014d2f007cd128845a6adaf7abca98a2a`, 회심의 일격 `b1e41c8b931fe80960e010792d6d617b86644d9e1ceab8ad981ecbdc8ec88b6d`, 회오리 베기 `bd50c4ea5e3965ca1cc3706010f86cd9ba80bb1506701db42cb5ae1ea0adec7b`, 동료의 습격 `aab77f422bbdd9f927e9478df8f67314a07440c2581456918b5b13e6b9c9b270`.
- 은색 금속, 금색 하이라이트, 효과 궤적이 기본 명령·수호자·치유사·사수 비교본과 맞습니다. 참조에는 기획 채택 사수도 포함됩니다. 후보군의 공통 주황 궤적은 자체 구분 단서를 약하게 만듭니다.

## 무라벨 가독성

| 항목 | 판정 |
| --- | --- |
| 난도 ↔ 회심의 일격 | **30px PARTIAL.** 40px에서는 난도의 상승 표식과 하단 초승달, 회심의 하단 착탄 섬광이 비교됩니다. 30px 밝은 흑백에서 둘 다 비슷한 대각선 검격과 둥근 궤적으로 읽힙니다. 난도는 기본 공격과도 대각선 검 구도가 비슷합니다. |
| 회오리 베기 | **30/40px PASS.** 검을 감싼 넓고 가로로 도는 고리가 한 지점에 모이는 결정타와 구분되며 광역 근접 회전 참격을 전달합니다. |
| 동료의 습격 | **종족 중립 PASS.** 발톱 자국과 발자국에 머리·몸통·털색이 없어 특정 늑대나 다른 펫 전용으로 읽히지 않습니다. **30px 의미 PARTIAL.** 밝은 흑백에서 작은 발자국이 옅어져 동료의 돌진보다 여러 갈래 공격으로 먼저 읽힐 수 있습니다. |
| 24/20px | **PARTIAL.** 회전 고리는 비교적 남고, 나머지는 공통 광휘가 겹칩니다. 난도의 상승 표식·회심의 착탄점·동료 발자국 세부가 약합니다. 실제 상태 HUD는 미검증입니다. |

## Unity · 보호

Unity 6000.6.5f1 격리 Battle의 기존 버튼 생성 경로를 사용했습니다. 기존 cross/skull/spinner/늑대 Sprite를 먼저 확인하고, 실행 중 네 Image의 Sprite만 교체해 무라벨 30/40px 실제 Canvas 렌더를 캡처했습니다. 7 PASS, 종료 0. 기존에 회심의 일격과 기세 HUD가 같은 skull 경로를 공유한다는 점은 정적으로 확인했고, 임시 버튼 검수에서 해당 매핑을 변경하지 않았습니다. 이번에는 HUD 적용이나 실제 스킬 실행을 확인하지 않았습니다.

증거는 `Temp/i124-fighter-companion-qa/`에 보존했습니다.

- `Actual_Dark.png` SHA256 `ea0e5e5bb21bd38065ce30958faaf99d1707b896eb24890e30c21afe5e894ef9`.
- `Actual_Light.png` SHA256 `d3c3e3a6f2a341ae8ae9beefadb462fab11dd3a5052d53c16927f2bebfb1ed85`.
- `Unity_Battle_Comparison.png` SHA256 `6c66b05c73df48f1afd64353fb6211490b96519012e54b84848f10fb537454e8`.

격리 임시 파일 원복, 합성 저장/설정 경로 사용. #85/#131/#132·승인 자산·원본 main·Save/UserData 보호. 기능·수치·VFX·투사체·제품 코드 변경, 본편 적용, main 병합, Push 없음.
