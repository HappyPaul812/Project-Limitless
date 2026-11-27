# Issue #124 마도사 기본 스킬 v2 표적 검수

2026-11-27 · [리뉴얼 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7209089063) · [기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7209113845)

**파일·30px 무라벨 핵심 윤곽·Unity 변경 2종 표시 PASS. 가이아 웰의 단일 인물 판독 PARTIAL. 본편 HOLD.**

- ZIP SHA256 `ea0d8ba5692674ea32eafbbc19a0408f027a2c6cc8bbe68723a68ee11b00032c`, CRC 84/84, Manifest 82/82 경로·크기·SHA256, SHA256SUMS 83/83. 128×128 투명 RGBA 3/3. v1 참조 3/3 및 비교본 15/15 일치.
- 썬더볼트 v2 `84d1b5407b30148eebc574af3b7a51b29d59f4b261b0091c3107d9c290db007e`, 가이아 웰 v2 `87ca3a402c6874d404a879d008f58f1f9987efed5f0a11570b004d412e35b6e1`, 파이어 볼 KEEP `5743937ce641fec1ecaefddaf7a85192c0af116e8cdb1181e58912a703cee67e`가 납품값과 일치합니다. 파이어 볼의 바이트는 v1 ZIP Art 원본과 동일합니다.
- 밝고 어두운 동일 배경 30px 무라벨 흑백에서 썬더볼트는 돌출된 X자 가지, 가이아 웰은 닫힌 둥근 돔, 수호의 맹세는 넓은 아치와 중앙 방패로 구분됩니다. 기본 방어의 뾰족한 방패 및 철벽의 성벽 형태와도 구분됩니다.
- 가이아 웰은 40px에서 중앙 인물과 돔을 관찰할 수 있으나 30px 특히 밝은 흑백에서는 격자와 인물의 명암이 섞입니다. 자기 한 몸 보호는 PARTIAL. 24/20px에서 도상 세부가 더 약해집니다.
- Unity 6000.6.5f1 격리 Battle의 기존 버튼에 변경 두 장만 런타임 표시, 무라벨 30/40px 캡처. 버튼 범위 5 PASS. 같은 배치 후속 Defeat 실시간 샘플에서 중간 프레임 관측이 빠져 프로세스 코드 3이었고, 별도 Defeat 검증에서 해결했습니다. 마도사 2종 캡처와 검사에는 실패가 없습니다. 파이어 볼/화상 및 가이아/HUD 기존 공유, 썬더볼트/감전 별도 계약을 유지했습니다. 후보 실제 HUD 미검증.

로컬 `Temp/i30-defeat4-i124-mage-v2-qa/`의 `Mage_v1_v2_Light.png` SHA256 `dfd37e37a5b323af0f30298aac3a23e5af5bf71dd883ff52c650296ea269f428`, `Mage_v1_v2_Dark.png` SHA256 `00ddfc5871a1e1e13cb9ab09753ab643e07644eabc3e7f7a02056acdc80be73f`, `MageV2_Unity_Battle.png` SHA256 `fa3619fafbe68d877b68f412d1a1a75ef8e94e41f307da12d449e9b0775b25fa`.

기존 PASS 전수 반복 없음. 격리 임시 파일 원복, 기능·수치·VFX·승인 자산·Save/UserData 및 #85/#131/#132 보호. 본편 적용·main 병합·Push 없음.
