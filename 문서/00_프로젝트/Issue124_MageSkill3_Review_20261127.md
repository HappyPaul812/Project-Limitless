# Issue #124 마도사 기본 스킬 3종 검수

2026-11-27 · [납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7208537347) · [기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7208576439)

**파일·격리 표시 PASS. 30px 썬더볼트↔가이아 웰 및 가이아 웰의 자기 보호 표현 PARTIAL. 본편 반영 HOLD.**

- ZIP 실물 SHA256 `3bb8ae16511970170e738ba43f1128b87ea58949747bb8fc5b310b3cb303ed96`. CRC 57/57, Manifest 55/55 경로·크기·SHA256, SHA256SUMS 56/56. 신규 3장 128×128 투명 RGBA. 비교본 15/15 승인 9종 적용 worktree의 원본과 바이트 일치.
- 파이어 볼 `5743937ce641fec1ecaefddaf7a85192c0af116e8cdb1181e58912a703cee67e`, 썬더볼트 `a0460b67ea10a4936d1bb22be774bc691277034a1cccdf47bfdcc90c581b37e8`, 가이아 웰 `f759b7fe413bef81531a2e0095f8ad3c8d1781e43062e2242f754f580351eeb7`.
- **파이어 볼 30/40px PASS.** 치우친 화염 꼬리와 밝은 구체가 컬러·흑백에서 두 원형 후보와 구분됩니다.
- **썬더볼트↔가이아 웰 40px PASS / 30px PARTIAL.** 40px에서는 삐져나오는 가지 번개와 가이아의 둥근 격자 돔이 보입니다. 30px 어두운 배경에서는 썬더볼트의 대각선이 남지만 밝은 배경 흑백에서는 내부 선이 약해져 두 후보가 비슷한 원형으로 읽힙니다.
- **가이아 방어계 비교.** 기본 방어의 뾰족한 방패, 철벽의 성벽 상단과는 30px에서도 구분됩니다. 수호의 맹세의 아치형 보호막과는 40px에서 돔 격자와 중앙 방패로 구분되며 30px 밝은 흑백에서는 보호막 외곽이 유사해 PARTIAL입니다. 마력 장벽은 읽히나 자기 한 몸 보호라는 범위는 아이콘만으로 명확하지 않습니다. 24/20px 세부 도상 PARTIAL.
- **격리 Unity 6000.6.5f1 PASS.** 기존 Battle 버튼 경로에서 마도사 후보 3장만 실행 중 교체해 무라벨 30/40px로 캡처했습니다. 표적 6 PASS, 공용 배치 종료 0. 현행 파이어 볼/화상의 Warm Explosion peak 4 도상 공유, 가이아 웰/상태 HUD의 dice_shield 경로 공유, 썬더볼트/감전의 별도 도상을 정적으로 확인해 유지했습니다. 후보 실제 상태 HUD와 스킬 발동은 미검증입니다.

로컬 `Temp/i30-hit4-i124-mage-qa/`의 실제 비교:

- `Mage_Actual_Dark.png` SHA256 `8285de99e31cb1b73ee9c69bf5999547a8a2dc7b6c4915401aefdd365565aaf9`
- `Mage_Actual_Light.png` SHA256 `caedb09f0864167bc5e82a1360dff0e26ca2b19f0813fa1c0cdc05885821e8db`
- `Mage_Unity_Battle.png` SHA256 `3798f571974da1e4356f774b00f2e15da9264fa947c24589750c904e27a25811`

격리 임시 파일 원복. 기존 승인 스킬 12 PASS 반복 없음. 기능·수치·VFX·Save/UserData·#85/#131/#132 보호. 본편 적용, main 병합, Push 없음.
