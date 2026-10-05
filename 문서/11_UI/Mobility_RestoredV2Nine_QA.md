# Mobility v2 redownload9종 QA

입력 `F:\Downloads\Limitless_Mobility_Path_9_Restored_v2_redownload.zip`, SHA256 `b2351935948e2028894d59cd24cbe9f6152128c010b11144b09f4a410ac0bc6a`. 문서화→원본9PNG바이트교체→검증. PNG9/Male5/Female4·대상9조합정확대응·중복0/누락0·512×512RGBA/4×4/16Frame/Cell128. 이미지수정0.

2026-10-05 Mobility v2 redownload9종: 가시적 기존문제 해소, Alpha>0 고립픽셀로 READY0/BLOCKED_ART9. 전체READY41/BLOCKED9 유지, HealerFemale 포함 다른41종 미변경. [최신QA](Mobility_RestoredV2Nine_QA.md).

144Frame전체 관찰: 머리/캐릭터/휠체어/바퀴/무기/지팡이/망토 Crop0, 이웃셀침범0/CharacterSwap0/휠체어DesignSwap0/방향오류0/Empty0/심각Scale변화0/심각Alpha손상0. 휠체어/바퀴/이동보조/자세 유지. 바퀴중심의푸른발광은효과로구분. 큰가시분리파츠0이나 Alpha>0 원본데이터의 고립성분이남아 이번 StrayPixel0 기준 미통과. 기존저Alpha허용QA와이번엄격기준의차이를명시하고다른41종은재판정하지않음.

|Path|Job|Gender|판정|남은0-basedFrame|사유|
|---|---|---|---|---|---|
|Mobility|Fighter|Female|BLOCKED_ART|0,1,2,4,5,6,7,10,11,12,13,14,15|이전 가시적 검/장비/바퀴아래 조각 해소. Alpha>0 엄격검사에서 본체와 8방향으로 연결되지 않은 저Alpha 고립 픽셀 잔존: Stray Pixel 0 미충족.|
|Mobility|Fighter|Male|BLOCKED_ART|0,1,2,5,6,7,8,9,10,11,12,13,14,15|이전 가시적 검/장비/바퀴아래 조각 해소. Alpha>0 엄격검사에서 본체와 8방향으로 연결되지 않은 저Alpha 고립 픽셀 잔존: Stray Pixel 0 미충족.|
|Mobility|Guardian|Female|BLOCKED_ART|1,2,5,6,10,12|이전 가시적 검/장비/바퀴아래 조각 해소. Alpha>0 엄격검사에서 본체와 8방향으로 연결되지 않은 저Alpha 고립 픽셀 잔존: Stray Pixel 0 미충족.|
|Mobility|Guardian|Male|BLOCKED_ART|0,2,3,4,5,6,7,8,9,11,15|이전 가시적 검/장비/바퀴아래 조각 해소. Alpha>0 엄격검사에서 본체와 8방향으로 연결되지 않은 저Alpha 고립 픽셀 잔존: Stray Pixel 0 미충족.|
|Mobility|Healer|Male|BLOCKED_ART|3,4,5,6,7,8,9,10,11,13,15|이전 가시적 검/장비/바퀴아래 조각 해소. Alpha>0 엄격검사에서 본체와 8방향으로 연결되지 않은 저Alpha 고립 픽셀 잔존: Stray Pixel 0 미충족.|
|Mobility|Mage|Female|BLOCKED_ART|0,4,6,7,11,13,14|이전 가시적 검/장비/바퀴아래 조각 해소. Alpha>0 엄격검사에서 본체와 8방향으로 연결되지 않은 저Alpha 고립 픽셀 잔존: Stray Pixel 0 미충족.|
|Mobility|Mage|Male|BLOCKED_ART|2,4,5,6,7,8,9,10,11,12,15|이전 가시적 검/장비/바퀴아래 조각 해소. Alpha>0 엄격검사에서 본체와 8방향으로 연결되지 않은 저Alpha 고립 픽셀 잔존: Stray Pixel 0 미충족.|
|Mobility|Sharpshooter|Female|BLOCKED_ART|1,2,3,4,5,7,8,13|이전 가시적 검/장비/바퀴아래 조각 해소. Alpha>0 엄격검사에서 본체와 8방향으로 연결되지 않은 저Alpha 고립 픽셀 잔존: Stray Pixel 0 미충족.|
|Mobility|Sharpshooter|Male|BLOCKED_ART|0,2,3,5,6,7,8,13,14,15|이전 가시적 검/장비/바퀴아래 조각 해소. Alpha>0 엄격검사에서 본체와 8방향으로 연결되지 않은 저Alpha 고립 픽셀 잔존: Stray Pixel 0 미충족.|

## 다음 이미지 세션 최소수정 리스트

좌표는128px 셀좌상단 bbox[x0,y0,x1,y1), 본체와8방향으로분리된 Alpha>0 픽셀. Codex는원본을수정하지않음. 큰파츠재생성/휠체어변경 없이 아래고립성분만다음이미지세션에서확인.

### Physical_Fighter_Female.png

기존문제 Frame 1,5,6,9,11,13-15의 가시적 잔여파츠 해소.

- Frame 0: [122, 113, 124, 117] 5px/최대Alpha4; [123, 106, 124, 108] 2px/최대Alpha1; [123, 109, 124, 111] 2px/최대Alpha1; [102, 15, 103, 16] 1px/최대Alpha1
- Frame 1: [125, 112, 126, 114] 2px/최대Alpha1
- Frame 2: [123, 52, 124, 54] 2px/최대Alpha1
- Frame 4: [73, 4, 76, 5] 3px/최대Alpha2; [125, 63, 126, 64] 1px/최대Alpha1
- Frame 5: [39, 7, 52, 10] 30px/최대Alpha6
- Frame 6: [39, 6, 48, 9] 22px/최대Alpha5; [2, 58, 3, 59] 1px/최대Alpha1
- Frame 7: [2, 46, 3, 47] 1px/최대Alpha1; [2, 49, 3, 50] 1px/최대Alpha1; [16, 56, 17, 57] 1px/최대Alpha1
- Frame 10: [2, 58, 3, 59] 1px/최대Alpha1
- Frame 11: [2, 63, 4, 64] 2px/최대Alpha1; [20, 4, 21, 5] 1px/최대Alpha1; [84, 4, 85, 5] 1px/최대Alpha1; [78, 119, 79, 120] 1px/최대Alpha1
- Frame 12: [117, 95, 118, 96] 1px/최대Alpha1
- Frame 13: [3, 77, 6, 79] 4px/최대Alpha2; [12, 92, 14, 94] 2px/최대Alpha1; [123, 99, 124, 100] 1px/최대Alpha1
- Frame 14: [4, 100, 10, 102] 11px/최대Alpha1; [4, 82, 6, 87] 8px/최대Alpha3
- Frame 15: [5, 80, 7, 83] 4px/최대Alpha1; [9, 100, 12, 102] 4px/최대Alpha1

### Physical_Fighter_Male.png

기존문제 Frame 1,5-7,9-11,14,15의 가시적 잔여파츠 해소.

- Frame 0: [60, 123, 66, 125] 8px/최대Alpha1; [47, 124, 50, 125] 3px/최대Alpha2; [88, 124, 91, 125] 3px/최대Alpha1; [125, 113, 126, 115] 2px/최대Alpha1
- Frame 1: [2, 81, 5, 85] 8px/최대Alpha4; [125, 115, 126, 119] 4px/최대Alpha2; [3, 73, 5, 76] 3px/최대Alpha1; [5, 77, 6, 79] 2px/최대Alpha1; [125, 113, 126, 114] 1px/최대Alpha1
- Frame 2: [87, 125, 90, 126] 3px/최대Alpha1; [2, 79, 3, 81] 2px/최대Alpha2; [45, 125, 47, 126] 2px/최대Alpha1; [2, 76, 3, 77] 1px/최대Alpha1; [2, 82, 3, 83] 1px/최대Alpha1
- Frame 5: [2, 57, 5, 60] 8px/최대Alpha2; [2, 81, 4, 82] 2px/최대Alpha1; [115, 117, 116, 118] 1px/최대Alpha1
- Frame 6: [8, 57, 9, 58] 1px/최대Alpha1; [2, 64, 3, 65] 1px/최대Alpha1; [8, 76, 9, 77] 1px/최대Alpha1; [2, 81, 3, 82] 1px/최대Alpha2
- Frame 7: [2, 60, 3, 61] 1px/최대Alpha1
- Frame 8: [101, 121, 102, 122] 1px/최대Alpha1
- Frame 9: [125, 69, 126, 73] 4px/최대Alpha2; [101, 121, 103, 122] 2px/최대Alpha1; [2, 55, 3, 56] 1px/최대Alpha1
- Frame 10: [2, 54, 4, 56] 3px/최대Alpha1; [101, 122, 104, 123] 3px/최대Alpha1
- Frame 11: [2, 57, 4, 59] 3px/최대Alpha2; [8, 46, 9, 47] 1px/최대Alpha2; [5, 57, 6, 58] 1px/최대Alpha1
- Frame 12: [107, 60, 108, 61] 1px/최대Alpha1
- Frame 13: [11, 29, 12, 30] 1px/최대Alpha1; [108, 38, 109, 39] 1px/최대Alpha1
- Frame 14: [7, 25, 10, 27] 4px/최대Alpha1; [95, 21, 96, 22] 1px/최대Alpha1; [4, 44, 5, 45] 1px/최대Alpha1
- Frame 15: [2, 41, 3, 44] 3px/최대Alpha4; [15, 19, 16, 20] 1px/최대Alpha1; [21, 26, 22, 27] 1px/최대Alpha1

### Physical_Guardian_Female.png

기존문제 Frame 0의 가시적 잔여파츠 해소.

- Frame 1: [67, 125, 70, 126] 3px/최대Alpha4
- Frame 2: [108, 68, 109, 69] 1px/최대Alpha1
- Frame 5: [123, 10, 125, 13] 4px/최대Alpha1; [78, 117, 79, 118] 1px/최대Alpha1
- Frame 6: [2, 47, 3, 51] 4px/최대Alpha1
- Frame 10: [83, 119, 85, 120] 2px/최대Alpha1
- Frame 12: [102, 100, 107, 106] 17px/최대Alpha1; [111, 21, 112, 22] 1px/최대Alpha1

### Physical_Guardian_Male.png

기존문제 Frame 6-11의 가시적 잔여파츠 해소.

- Frame 0: [56, 124, 60, 126] 6px/최대Alpha1
- Frame 2: [61, 123, 63, 125] 4px/최대Alpha1
- Frame 3: [62, 125, 64, 126] 2px/최대Alpha1
- Frame 4: [83, 2, 91, 5] 15px/최대Alpha6; [79, 2, 80, 3] 1px/최대Alpha1
- Frame 5: [81, 3, 93, 6] 30px/최대Alpha6; [2, 71, 6, 80] 20px/최대Alpha2
- Frame 6: [82, 3, 91, 6] 21px/최대Alpha7; [2, 83, 4, 88] 7px/최대Alpha1; [8, 71, 9, 75] 4px/최대Alpha1; [78, 3, 81, 5] 3px/최대Alpha1
- Frame 7: [80, 2, 89, 4] 13px/최대Alpha5; [2, 85, 4, 89] 6px/최대Alpha2; [2, 67, 4, 68] 2px/최대Alpha1; [98, 119, 99, 120] 1px/최대Alpha1
- Frame 8: [123, 65, 125, 72] 11px/최대Alpha1
- Frame 9: [80, 124, 83, 126] 5px/최대Alpha1
- Frame 11: [37, 2, 38, 3] 1px/최대Alpha1
- Frame 15: [107, 99, 108, 101] 2px/최대Alpha1

### Physical_Healer_Male.png

기존문제 Frame 4,15의 가시적 잔여파츠 해소.

- Frame 3: [124, 83, 125, 85] 2px/최대Alpha1; [10, 46, 11, 47] 1px/최대Alpha1
- Frame 4: [122, 28, 126, 32] 10px/최대Alpha2; [84, 4, 89, 5] 5px/최대Alpha5; [39, 4, 40, 5] 1px/최대Alpha1
- Frame 5: [2, 70, 6, 81] 33px/최대Alpha8; [82, 3, 89, 5] 8px/최대Alpha4
- Frame 6: [2, 66, 5, 80] 28px/최대Alpha16; [124, 30, 126, 36] 10px/최대Alpha2; [84, 4, 89, 5] 5px/최대Alpha4; [2, 81, 3, 83] 2px/최대Alpha1
- Frame 7: [84, 4, 89, 5] 5px/최대Alpha4
- Frame 8: [123, 72, 126, 81] 17px/최대Alpha4
- Frame 9: [14, 24, 17, 27] 4px/최대Alpha1; [5, 103, 7, 104] 2px/최대Alpha1
- Frame 10: [2, 35, 3, 36] 1px/최대Alpha1
- Frame 11: [117, 73, 118, 74] 1px/최대Alpha1; [78, 118, 79, 119] 1px/최대Alpha1
- Frame 13: [6, 57, 7, 59] 2px/최대Alpha1; [6, 60, 7, 62] 2px/최대Alpha1; [43, 2, 44, 3] 1px/최대Alpha1
- Frame 15: [9, 56, 10, 58] 2px/최대Alpha1; [15, 26, 16, 27] 1px/최대Alpha1; [18, 27, 19, 28] 1px/최대Alpha1; [18, 29, 19, 30] 1px/최대Alpha1; [21, 30, 22, 31] 1px/최대Alpha1; [16, 31, 17, 32] 1px/최대Alpha1; [19, 31, 20, 32] 1px/최대Alpha1; [6, 65, 7, 66] 1px/최대Alpha1

### Physical_Mage_Female.png

기존문제 Frame 0,8-10의 가시적 잔여파츠 해소.

- Frame 0: [117, 72, 119, 73] 2px/최대Alpha1; [61, 125, 62, 126] 1px/최대Alpha3
- Frame 4: [45, 5, 46, 6] 1px/최대Alpha1; [46, 9, 47, 10] 1px/최대Alpha1
- Frame 6: [124, 110, 125, 111] 1px/최대Alpha1
- Frame 7: [41, 3, 49, 5] 14px/최대Alpha2
- Frame 11: [2, 37, 3, 38] 1px/최대Alpha1
- Frame 13: [74, 124, 75, 125] 1px/최대Alpha1
- Frame 14: [102, 61, 103, 62] 1px/최대Alpha1

### Physical_Mage_Male.png

기존문제 Frame 5-7의 가시적 잔여파츠 해소.

- Frame 2: [3, 84, 4, 87] 3px/최대Alpha1
- Frame 4: [81, 5, 86, 6] 5px/최대Alpha5; [89, 5, 91, 6] 2px/최대Alpha1; [78, 5, 79, 6] 1px/최대Alpha1; [87, 5, 88, 6] 1px/최대Alpha1
- Frame 5: [2, 70, 6, 80] 31px/최대Alpha3; [76, 5, 90, 7] 21px/최대Alpha6; [78, 13, 79, 15] 2px/최대Alpha1; [2, 84, 3, 85] 1px/최대Alpha1
- Frame 6: [79, 5, 86, 7] 11px/최대Alpha7; [2, 64, 4, 70] 10px/최대Alpha2; [2, 86, 5, 90] 9px/최대Alpha2; [87, 5, 92, 7] 7px/최대Alpha1; [7, 79, 8, 84] 5px/최대Alpha1
- Frame 7: [76, 5, 89, 7] 19px/최대Alpha6; [2, 68, 7, 75] 16px/최대Alpha2; [2, 84, 5, 88] 8px/최대Alpha1; [2, 38, 3, 39] 1px/최대Alpha1
- Frame 8: [123, 78, 126, 84] 13px/최대Alpha1; [39, 17, 40, 19] 2px/최대Alpha2; [125, 36, 126, 38] 2px/최대Alpha2
- Frame 9: [17, 22, 18, 23] 1px/최대Alpha1
- Frame 10: [124, 78, 126, 84] 10px/최대Alpha1; [2, 37, 3, 40] 3px/최대Alpha1
- Frame 11: [111, 57, 112, 58] 1px/최대Alpha1
- Frame 12: [53, 116, 54, 117] 1px/최대Alpha1
- Frame 15: [46, 2, 47, 3] 1px/최대Alpha1; [38, 13, 39, 14] 1px/최대Alpha1

### Physical_Marksman_Female.png

기존문제 Frame 0의 가시적 잔여파츠 해소.

- Frame 1: [67, 125, 69, 126] 2px/최대Alpha1
- Frame 2: [115, 87, 116, 88] 1px/최대Alpha1
- Frame 3: [106, 100, 107, 101] 1px/최대Alpha1
- Frame 4: [125, 36, 126, 38] 2px/최대Alpha1
- Frame 5: [47, 5, 48, 6] 1px/최대Alpha1
- Frame 7: [44, 5, 46, 6] 2px/최대Alpha1; [47, 5, 48, 6] 1px/최대Alpha1
- Frame 8: [120, 43, 122, 45] 2px/최대Alpha1; [3, 73, 4, 74] 1px/최대Alpha1
- Frame 13: [63, 113, 64, 114] 1px/최대Alpha1

### Physical_Marksman_Male.png

기존문제 Frame 0-3의 가시적 잔여파츠 해소.

- Frame 0: [65, 121, 71, 125] 13px/최대Alpha3; [61, 121, 64, 125] 9px/최대Alpha2; [112, 66, 113, 68] 2px/최대Alpha1
- Frame 2: [4, 53, 6, 62] 17px/최대Alpha3
- Frame 3: [58, 125, 62, 126] 4px/최대Alpha2; [66, 125, 67, 126] 1px/최대Alpha1
- Frame 5: [2, 72, 3, 73] 1px/최대Alpha1
- Frame 6: [2, 75, 5, 84] 16px/최대Alpha4
- Frame 7: [2, 64, 4, 68] 5px/최대Alpha1; [6, 66, 7, 67] 1px/최대Alpha1
- Frame 8: [2, 72, 3, 73] 1px/최대Alpha1
- Frame 13: [24, 18, 25, 19] 1px/최대Alpha1; [36, 40, 37, 41] 1px/최대Alpha1
- Frame 14: [18, 81, 19, 82] 1px/최대Alpha1; [55, 113, 56, 114] 1px/최대Alpha1
- Frame 15: [4, 98, 5, 99] 1px/최대Alpha1

Runtime 검증 완료.

백그라운드 격리PlayUnfocused **85PASS/0FAIL**: 대상9조합 Mapping/JobPreview 정상. 대표Male/Female Fighter의World/BattleLeftIdle/실제Battle/Save→BootstrapContinue 통과. BLOCKED9종은 기존성별fallback 표시를 유지하며 원본아트 직접표시 통과로 보고하지 않는다. 구버전ID 재계산/PathJob 없는옛Save 기본fallback 유지. 새Migration/수동UI/게임기능코드변경 없음.

컴파일Error0/최종ConsoleError0/Warning0. 기존CS0618 deprecated경고2건 별도기록/미수정. cleanBootstrap EditMode 및Save/Settings/Play설정복원, 포커스전환 없음. 실제걷기/키보드/게임패드 미검증. 보존PASS: 원본9PNG바이트/ZIP동일·모든meta/144SpriteID/import동일·다른41PNG/CatalogEntry/QA/기존328Clip 동일, HealerFemale READY불변. 사용자410Git항목보존. Assets/Settings/UserData 변경은9PNG+Catalog+Inventory+QAHelper12파일. 신규Clip0, READY승격0, 전체41/9. 직접변경diff--check 확인/GitHubPush없음. [Runtime결과](Mobility_RestoredV2Nine_Runtime_Results.txt).
