# Issue #124 가이아 웰 v3 표적 검수

2026-11-27 · [리뉴얼 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7209595350) · [기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7209731180)

**파일·30px 윤곽 구분·기존 화풍·Unity 표시 PASS. 지면 상승과 자기 보호의 30px 단독 의미 PARTIAL. 본편 반영 HOLD.**

- ZIP 실물 SHA256 `2182c3352ad0d132c4dae9e7bac244a278f1902e25c202435783992769826c63`, CRC 93/93, Manifest 91/91 경로·크기·SHA256, SHA256SUMS 92/92. 후보와 KEEP 3장 모두 128×128 투명 RGBA. 가이아 v3 SHA256 `04c838023f5863e124e76a4c10d42513636313d24f64ae6a922135db550e5f1f`.
- 파이어 볼 `5743937ce641fec1ecaefddaf7a85192c0af116e8cdb1181e58912a703cee67e`는 v1·v2 패키지의 PNG와 바이트 동일. 썬더볼트 `84d1b5407b30148eebc574af3b7a51b29d59f4b261b0091c3107d9c290db007e`는 직전 v2와 바이트 동일. 비교 참조 21/21도 직전 ZIP과 동일합니다.
- **암석 장벽 표현.** 갈라진 바닥과 불규칙하게 위로 향한 암석을 금빛 균열이 연결합니다. 원본과 40px 컬러에서 대지 장벽의 물성과 솟는 방향을 읽을 수 있습니다. 30px 흑백에서는 돌무더기 윤곽이 먼저 남아, 지면에서 형성되는 동작까지 전달되는 정도는 PARTIAL입니다.
- **30px 무라벨 흑백 구분 PASS.** 가이아 v3의 낮고 들쭉날쭉한 바위층은 기본 방어의 뾰족한 방패, 철벽의 정형 성벽, 수호의 맹세의 아치와 중앙 방패와 구분됩니다. 썬더볼트의 돌출 X자 번개와도 구분됩니다. 같은 밝고 어두운 배경에서 확인했습니다.
- **자기 보호 PARTIAL.** 원본에서 한 명의 인물을 둘러싼 반원 장벽이 보이지만 30px에서는 얼굴·몸 윤곽이 바위와 균열에 묻힙니다. 24/20px에서는 더 약합니다. **화풍 PASS.** 돌을 주재료로 삼으면서 금빛 하이라이트와 마력 궤적은 기존 승인 세트와 맞습니다.
- **Unity 6000.6.5f1 격리 Battle PASS.** 가이아 웰 v3 한 장만 임시로 기존 버튼에 표시하고 무라벨 30/40px 실제 Canvas 렌더를 확보했습니다. 표적 4 PASS, 종료 0. 가이아 스킬과 상태 HUD의 기존 동일 리소스 매핑은 유지했습니다. 후보 실제 상태 HUD와 발동은 미검증입니다.

화면은 `Temp/i124-gaia-v3-qa/`에 로컬 보존했습니다.

- `Actual_Dark.png` SHA256 `08a7a5b4565669478841de770839c900c7669b9726cf3fdae93ae3cb93b850c6`.
- `Actual_Light.png` SHA256 `e499f12a065635d8cfa64d63a4153ce14fffd04576e741a446b7dd7f8ac66040`.
- `Unity_Gaia_v3_Battle.png` SHA256 `3db201b07cbd5aa23576d029f05532f496618bea04dbb9f04de774c41d02d04a`.

기존 PASS 전수 반복 없음. 격리 임시 파일 원복. 기능·수치·VFX·공유 계약·승인 자산·Save/UserData 및 #85/#131/#132 보호. 본편 적용·main 병합·Push 없음.
