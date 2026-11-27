# Issue #30 설빙의 폭군 Hit4 v1 검수

2026-11-27 · [납품](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7208522860) · [기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-7208575685)

**파일·외형/접지·연속성·336px 배율 PASS. 피격 즉시 판독 PARTIAL. 본편 반영 HOLD.**

- Hit4 ZIP SHA256 `dfc12372ffcb5076eb2e1d4546e6407c00496b8d7080f9ffce23b9bc3a2eb29c`, CRC 17/17, SHA256SUMS 16/16. Manifest 시트 SHA256 `952e44855b7319c339a22b99fc35d8395d182f75b8d926be2de2f55b0e703713` 일치. 2048×512 RGBA와 개별 512×512 셀 4/4 바이트 픽셀 동일. APNG 단독 4프레임 및 Idle→Hit→Idle 12프레임 모두 예상 원본 픽셀과 125ms 간격 일치.
- 동봉 KeyPose·Idle4·승인 Attack4 v3는 직전 ZIP과 3/3 바이트 일치합니다. Hit00/01/02/03의 y360–511 하단 152행은 각각 Idle00/01/02/00과 픽셀 동일합니다. 가시 하단 y490 exclusive, 테두리 알파 0, 잘림과 밝은 매트가 보이지 않습니다.
- 뿔·흉곽·발톱 색과 체급이 유지됩니다. 01에서 상체가 뒤로 움찔하고 02→03에서 풀립니다. 03→Idle00은 큰 위치 점프 없이 이어집니다. 접지가 고정돼 안정적이나 반동 폭이 작아 피격 맥락이 없는 화면에서는 Idle 변화로 읽힐 여지가 있습니다. 별도 전투 히트 이벤트, VFX, 실전 피격 시점과의 결합은 미검증입니다.
- Unity 6000.6.5f1 격리 SpriteRenderer 1280×720, PPU512, 하단 중앙 Pivot, Scale3, 셀 336px로 Idle4·Hit4 8장 실제 표시, 배치 종료 0. 연속 자료는 실제 렌더를 125ms로 이어 붙였습니다.

로컬 `Temp/i30-hit4-i124-mage-qa/`의 `Hit_Unity_336px.png` SHA256 `c762825a281bebff56a1b63ed747129fcff4d395fa830a9e85a0fa7229f4df35`, `Hit_Unity_Idle_Hit_Idle.png` SHA256 `fd0176085e49369bd7860e219428774b23e8fc5320693ffcad98c1ba0de9c3bd`.

승인 Attack4 v3와 기존 자산 보호. 격리 파일 원복, Defeat 제작·본편 적용·main 병합·Push 없음. #85/#131/#132 및 Save/UserData 보호.
