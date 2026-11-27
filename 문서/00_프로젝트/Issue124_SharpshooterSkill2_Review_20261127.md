# Issue #124 사수 기본 스킬 2종 표적 검수

2026-11-27 · [리뉴얼 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7207057343) · [기획 검수 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7207087462)

**신규 2종 파일·화풍·30/40px·격리 Unity 표시 PASS. 24/20px 의미 판독 PARTIAL. 동료의 습격 고유 자산 기준 KEEP 보류. 본편 반영 HOLD.**

- ZIP `F:/Downloads/LIMITLESS_Issue124_SharpshooterSkill2_Original_20261127.zip` SHA256 `6d5a9dbfed242e0d28604b3011dc5f9954927407be9aad02caa03a5961192a60`. CRC 38/38, Manifest 경로·크기·SHA256 36/36, SHA256SUMS 37/37. 신규 후보 2장 128×128 투명 RGBA. 기본 명령·수호자 승인 참조 6종과 기획 채택 치유사 참조 3종은 앞선 납품 바이트와 일치합니다.
- 정조준 SHA256 `1312e3b8b7dc5772005227ac457f07010813fc3d9527a75c79cad0e9ccd857ba`. 화살비 SHA256 `0f022a455a1df51d7048f2a2503ce496b5362013ce3c8b5b11a522859d7d7f31`.
- **화풍 PASS.** 금은색 금속, 날카로운 광택과 따뜻한 효과 궤적이 승인 기본 공격 및 수호자 세트와 조화를 이룹니다.
- **40/30px PASS.** 동일한 밝은·어두운 배경의 컬러/흑백 실크기에서 정조준은 대각선 단일 화살과 위쪽 조준점, 화살비는 아래로 모이는 여러 화살과 하단 착탄 고리로 구분됩니다. **24/20px PARTIAL.** 외곽 방향은 남지만 24px부터 화살 개별 획이 뭉치고 20px 밝은 흑백에서 조준선·착탄 고리의 세부 판독이 약합니다. 실제 소형 HUD 장착은 범위 밖입니다.
- **Unity PASS.** 6000.6.5f1 격리 Battle의 기존 버튼 생성 경로에서 Kenney target/bow를 확인하고 실행 중에 후보 2장만 표시했습니다. 동일 배경 무라벨 30/40px 캡처, 표적 5 PASS·종료 0. 합성 저장·설정 경로 사용, 격리 파일 원복. 치유사 20+5 전수 재실행 0건.

## 동료의 습격 출처 확인

`BattleUiIconCatalog.LoadSkillButtonIcon → Load → BeastCompanionCatalog.GetDefaultDefinition` 경로는 현재 기본 늑대를 사용합니다. 장착별 야수 선택은 별도 전투 경로에 있습니다. 카탈로그의 `IconResourcePath`와 `RunResourcePath`가 분리되어 있어 승인 고유 Run6 전투 애니메이션이 아이콘 소스로 쓰이지 않습니다.

| 야수 | 정의의 실제 아이콘 원본 | 셀 | ScratchIO 보존 ZIP과 바이트 |
| --- | --- | --- | --- |
| 늑대 | `Wolf_Run.png` | 64×40 index 4 | 일치 |
| 곰 | `Bear_Run.png` | 64×33 index 0 | 일치 |
| 여우 | `Fox_Run.png` | 64×36 index 0 | 일치 |

세 파일은 `Assets/_Project/Resources/CompanionAssaultValidation/`에서 읽으며 `Assets/ThirdParty/ScratchIO/AnimatedWildAnimals/All.zip`의 원본과 3/3 동일합니다. 프로젝트 출처 기록은 ScratchIO Animated Wild Animals, CC0 1.0입니다. 따라서 고유 자산 요건으로는 KEEP 보류입니다. 현행 동적 잘라내기와 전투 연출은 그대로 유지하며 고정 늑대나 새 아이콘을 적용하지 않았습니다.

## 증거 및 보호

- `Temp/i124-sharpshooter-qa/Actual_Comparison.png` SHA256 `974ad465648f9e080b2ad3802c9b585122041f7f37fe8e195df4dc009202f72e`.
- `Temp/i124-sharpshooter-qa/Unity_Battle_Comparison.png` SHA256 `2c572bdc169024d6f2c828fba86a68d85c1d2d14326fcf0c56eb97d621d7d6f1`.
- 동일 폴더에 전체 Unity 화면 3장, `unity.log`, `file-qa.txt` 보존. #85/#131/#132·승인 자산·Save/UserData 보호. 전투 기능·수치·VFX·투사체 변경, 본편 적용, main 병합, Push 없음.
