# Issue #124 FighterSkill3 + Companion1 v2 표적 검수

2026-11-27 · [리뉴얼 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7207829305) · [기획 검수 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7208051017)

**30px 무라벨 핵심 도상 구분 PASS · 24/20px 세부 표현 PARTIAL · Unity 변경 3종 표시 PASS · 본편 반영 HOLD.**

- ZIP 실물 SHA256 `c093b0caa73eb84338a4ef555edebe90c36e4c0c1e694893904948f93c565758`. ZIP CRC 96/96, Manifest 94/94 경로·크기·SHA256, SHA256SUMS 95/95. 후보 및 KEEP 4장 모두 128×128 투명 RGBA. 이전 승인·채택 참조 11/11 바이트 일치.
- 난도 `d010dbde76ef9f9dad66f4234a265cc1e46826a6595090fb2decec2ee1f6fdfb`, 회심의 일격 `27552b554efa24a2ead170de2876af759359ef4cda106adaac4e0ad047b889d1`, 동료의 습격 `4116525786bbb2fbf64e8c83b89c9866031818346d9877b1d41fcc2d6f00bdb1`가 납품값과 일치합니다. 회오리 베기 KEEP `bd50c4ea5e3965ca1cc3706010f86cd9ba80bb1506701db42cb5ae1ea0adec7b`는 기존 v1 ZIP 원본과 바이트 동일합니다.

| 검수 | 판정 |
| --- | --- |
| 난도 ↔ 회심 | **PASS.** 30px의 같은 밝은·어두운 배경에서 라벨과 색을 제거해도 난도의 상향 3단 표식과 회심의 수직 하향 검·하단 착탄이 구분됩니다. |
| 난도 ↔ 기본 공격 | **PASS.** 난도의 가장 큰 도상은 상향 표식입니다. 기본 공격의 대각선 검 실루엣과 구분됩니다. |
| 동료의 습격 | **PASS.** 30px에서 큰 발바닥 패드와 발가락이 먼저 읽힙니다. 발톱과 속도선은 보조 단서입니다. 특정 야수의 얼굴·실루엣·털색 없이 늑대·곰·여우 공통 표식으로 읽힙니다. |
| 40 / 24 / 20px | 40px 주요 도상 PASS. 24/20px에서도 전체 도상은 분리되지만 난도의 작은 검, 회심의 착탄선, 동료의 속도선은 약해 세부 표현 PARTIAL입니다. 실제 상태 HUD 미검증입니다. |

**격리 Unity 6000.6.5f1 PASS.** 기존 Battle 버튼 생성 경로에서 기존 cross/skull/늑대 표시를 확인한 뒤 실행 중 Image Sprite만 v2 세 장으로 교체했습니다. 무라벨 30/40px 실제 Canvas 캡처, 표적 6 PASS·종료 0. 회오리 베기 로딩 및 기존 7 PASS 전수 재실행 없이 임시 파일을 원복했습니다. 회심의 일격/기세 HUD가 같은 skull 경로를 공유하는 현재 계약을 보호했습니다. v2 HUD 연결과 스킬 실제 발동은 미검증입니다.

비교 화면 `Temp/i124-fighter-companion-v2-qa/`

- `Actual_v1_v2_Dark.png` SHA256 `21d0c806fa281fe7169b1fff30e56f87239ba39282bb0dd3fa3f2df072eff394`
- `Actual_v1_v2_Light.png` SHA256 `e83386eaa5a707b48c9e01f5d9bc5825a6c5b31f9860452816cb30816eeb1549`
- `Unity_Battle_Comparison.png` SHA256 `3b3c7081ce8102f3a2ccdbb083a162ac2a4658d249b698b40949c50ef0e15a75`

기존 기능·수치·VFX·야수 애니메이션·#85/#131/#132·Save/UserData·원본 main 보호. 미술 승인 대기. 본편 적용·main 병합·Push 없음.
