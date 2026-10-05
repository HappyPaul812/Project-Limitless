# Player Appearance Foreground 최종 판정

2026-10-05 Hearing v210종 원본 반영·새160Frame QA: READY0/BLOCKED_ART10, 전체 **READY30/BLOCKED20 유지**. Mobility 포함 다른40종 보존. [최신 QA](Hearing_RestoredV2Ten_QA.md).

2026-10-05 Hearing 최종10종 원본 반영·새160Frame QA: READY0/BLOCKED_ART10, 전체 **READY30/BLOCKED20 유지**. Mobility 포함 다른40종 보존. [최신 QA](Hearing_RestoredFinal10_QA.md).

2026-10-05 Vision512Final 새48Frame QA: 지정3종 READY3/BLOCKED0·전체 **READY30/BLOCKED20**. 다른47종 및 이전 READY27 보존. [최신 QA](Vision_Restored512Final3_QA.md). 백그라운드90PASS/0FAIL·최종Console Error0/Warning0.

2026-10-05 Vision Final3 형식검사 보류: `F:/Downloads/Limitless_Vision_3_Restored_Final.zip` 지정3종이 모두1254×1254 RGBA여서512×512·Cell128 규격 불일치. 게임 자산 반영0/READY 승격0, 전체 **READY27/BLOCKED23 유지**. 다른47종 및 기존 READY/사용자 변경 보존. [최신 QA](Vision_RestoredFinal3_QA.md).

2026-10-05 Restored8 원본 반영: 대상 READY5/BLOCKED_ART3, 전체 **Ready27/Blocked23**. 다른42종 보존. [최신 QA](Intellectual_Vision_Restored8_QA.md). 아래는 이전 이력이다.

2026-10-05 최신 셀 정밀 수정: 지정8종 문제41셀만 수정, 정상84셀 Pixel Diff0. 기존 절단 윤곽 잔존으로 READY승격0, 전체 Ready22/Blocked28 유지. 다른42종 보존. [최신 개별 QA](Intellectual_Vision_CellCleanup_QA.md). 아래는 이전 이력이다.

2026-10-05 Cleanup v2: 지정8종 READY0/BLOCKED_ART8, 전체 **Ready22/Blocked28 유지**. 다른42종 보존. [최신 개별 QA](Intellectual_Vision_CleanupV2_QA.md). 아래는 이전 이력이다.

최신 Cleanup8: READY0/BLOCKED_ART8, 전체 Ready22/Blocked28 유지. 다른42종 보존. [최신 QA](Intellectual_Vision_Cleanup8_QA.md). 아래는 이전 이력이다.

2026-10-03 최신 V2: Intellectual3/Vision5 대상8종 BLOCKED_ART, 신규READY0·전체 Ready22/Blocked28 유지. 다른42종과 기존 Intellectual Fighter Male READY 보존. [최신 V2 QA](Intellectual_Vision_V2Eight_QA.md). 아래 Revised/Fixed 집계 설명은 이전 작업 이력이며 표/JSON은 최신 판정이다.

2026-10-03 최신 Revised9: Intellectual Fighter Male READY 승격, 대상 Ready1/Blocked8·전체 Ready22/Blocked28. Vision Fighter Male은 방향 해결 후 BLOCKED_ART. 다른41종 판정 유지. [최신 QA](Intellectual_Vision_Revised9_QA.md). 아래 집계 설명은 이전 작업 이력이며 표/JSON은 최신 판정이다.

Intellectual4/Vision5는 최신 제공 수정판으로 교체 후 백그라운드 관찰로 재판정했다. Intellectual4 BLOCKED_ART, Vision4 BLOCKED_ART/1 BLOCKED_DIRECTION, 전체 Ready21/Blocked29 유지. 다른41종의 판정은 유지한다. 최신9종 입력/증거는 [QA](Intellectual_Vision_Fixed9_QA.md)를 따른다. 아래 Mobility/Hearing 문단은 해당 작업 당시 기록이다.

Mobility10종은 제공512×512 원본으로 교체 후 백그라운드 프레임 관찰로 재판정했다. Hearing10종의 이전 수정판 판정과 나머지30종의 기존 Foreground 판정은 유지한다. 이번 Foreground 재검수 없음. [Mobility QA](Mobility_Player_Sprite_Fixed_QA.md)·[Hearing QA](Hearing_Player_Sprite_Fixed_QA.md).
총50 / PASS21 / Blocked29 / 외형 시트 미판정0. 현재 PNG Hash는 각 적용 Inventory를 따른다. Frame 번호는 0-based이다. 전체800 Sprite와 Stable ID는 유지한다.

Mobility 수정판10: 모두 BLOCKED_ART(분리 조각·머리 상단 절단). Vision 기존 PASS5/Art 보류5, Hearing10·Intellectual Fighter/Mage4 보류 유지.

증거: `Unity/Client/Temp/ForegroundReviewQA/Detail_*.png`, 기존 group Game View, `preview_00..34.png`, `world.txt`와 World_*.png. 상세 증거는 로컬 Temp에 남아 있으며 이 표는 당시 직접 관찰 기록이다.

|Appearance ID|파일|성별|판정|Frame|문제/결과|
|---|---|---|---|---|---|
|appearance.external.v1.hearing.fighter.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Fighter_Female.png|Female|BLOCKED_ART|6,8,10|이전4/9 발밑선 해결,8은축소됐지만[60,121,65,122)5px 검은선잔존. 새Frame6 우측망토옆[124,110,125,113)3px 검은조각,10 머리/망토옆[23,74,24,75)1px 분리점. 머리/귀장치/검/방향정상.|
|appearance.external.v1.hearing.fighter.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Fighter_Male.png|Male|BLOCKED_ART|1,4-6,8-11|이전Left 큰발조각과Up13/15 머리위선 해소. Left4-6 발밑2-3px선 및5 우측[123,108,125,112)5px 조각잔존. 새Down1 우측[120,107,125,118)34px 빨간/검은검파츠, Right8-11 발아래[60,119,71,122)26px+[56,121,58,122)2px/[57,119,67,122)25px/[57,122,68,125)25px/[60,122,71,125)27px 살색/검은분리파츠.|
|appearance.external.v1.hearing.guardian.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Guardian_Female.png|Female|BLOCKED_ART|1,2,6,9,14|이전Left4/5 조각해소. Down1/2 우측[122,109,124,116)10px/[123,108,125,116)13px,Left6[123,102,125,107)6px 검은파츠잔존. Right9 좌측[3,101,7,113)35px 검끝조각과발밑[59,118,74,121)32px. 새Up14 좌측[6,85,10,97)34px 검파츠.|
|appearance.external.v1.hearing.guardian.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Guardian_Male.png|Male|BLOCKED_ART|8,10|이전Down1 조각해소. Right8 우측[123,81,125,87)10px 검은/금색절단파츠잔존. 새Right10 우측[124,82,125,85)3px 조각. 귀장치/머리/방향정상.|
|appearance.external.v1.hearing.healer.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Healer_Female.png|Female|BLOCKED_ART|0,1,3,4,5,7,9,10,11,15|이전큰발조각과Up13 상단선해소. Frame0 우측[122,36,125,52)37px 지팡이잘린파츠,4[121,38,125,53)38px 및머리위[66,7,71,8)5px 검은선,7 좌측[3,63,7,79)43px 잘린장식. Right9/10/11 좌측지팡이파츠[3,34,7,52)47px/[3,33,10,54)91px/[3,29,13,55)184px. 머리위선9[39,3,47,5)13px/11[46,5,53,7)11px/Up15[87,3,93,5)9px. Up15좌측[5,28,8,39)24px조각. 정상음파효과자체는실패로세지않음.|
|appearance.external.v1.hearing.healer.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Healer_Male.png|Male|BLOCKED_ART|1,3,5,8,9,10,11|이전0 우측큰파츠해소. Right9 좌측[3,42,5,49)12px,11 좌측[4,33,10,50)58px 잘린빨간/금색지팡이파츠잔존. Left5 좌측[3,93,4,101)8px 검은선. 발아래Frame8[59,120,62,121)3px,10[57,122,61,123)4px+[50,122,53,123)3px+1px. 1 우측3px/3,5에1px 잔여점. 귀장치/방향정상, 정상지팡이발광과구분.|
|appearance.external.v1.hearing.mage.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Mage_Female.png|Female|BLOCKED_ART|0,1,2,3,5,6,7,9,10,14|이전Frame1 큰장식조각축소. 새Down2 좌측[3,68,9,106)142px 금색/빨간매달린장식파츠,Left6 좌측[3,64,9,102)140px 동일이웃장식. Left5[3,76,7,94)43px+[3,68,5,75)11px 잔존. Right9[4,28,6,41)21px,10[3,30,5,43)22px 좌측지팡이/장식절단조각,10우측9+3px. 0/1/3/7/14 작은잔여점. 정식귀장치보존/방향정상.|
|appearance.external.v1.hearing.mage.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Mage_Male.png|Male|BLOCKED_ART|9,10,11,14|Right9 왼쪽[3,45,6,59)33px 빨간/흰지팡이원잘린파츠잔존. Right10[3,51,4,55)4px와11[3,51,4,54)3px잔여선,10[33,65,34,66)1px 및Up14[83,48,84,49)1px분리점. 다른행귀장치/머리/방향정상.|
|appearance.external.v1.hearing.marksman.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Marksman_Female.png|Female|BLOCKED_ART|0,4,6,7,8,9,11,12,14|이전3/13/15 셀옆큰파츠해소. Left4/6/7 발아래[70,122,79,124)14px/[67,123,73,124)6px/[69,123,74,124)5px잔존. 새Right8/9 발조각[51,117,71,122)58px/[55,119,73,122)34px. Up12/14 포니테일상단이셀y=3부근에서넓은수평단면으로잘림(셀여백은있으나원래머리끝윤곽불완전). 0/9/11 작은분리점. 귀장치보존/방향정상.|
|appearance.external.v1.hearing.marksman.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Marksman_Male.png|Male|BLOCKED_ART|4,5,7,13,15|이전Right8-11 머리위선/Left5 큰셀옆파츠해소. 새Up13 머리위[44,3,52,4)8px+[78,3,82,4)4px검은선,Up15[80,3,84,4)4px검은선잔존. Left4발아래1px/5발아래2+2px/7발아래3+2px분리점. 귀장치/방향정상.|
|appearance.external.v1.emotionalscar.fighter.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Fighter_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.emotionalscar.fighter.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Fighter_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.emotionalscar.guardian.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Guardian_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.emotionalscar.guardian.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Guardian_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.emotionalscar.healer.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Healer_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.emotionalscar.healer.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Healer_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.emotionalscar.mage.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Mage_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.emotionalscar.mage.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Mage_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.emotionalscar.marksman.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Marksman_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.emotionalscar.marksman.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/EmotionalScar/Heartscar_Marksman_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.intellectual.fighter.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Fighter_Female.png|Female|PASS|0-15|Right 리본 끝·Up 머리/리본 상단 복원. 전체16프레임 본체·장비·방향 정상.|
|appearance.external.v1.intellectual.fighter.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Fighter_Male.png|Male|PASS|없음|Down/Left/Right/Up16Frame에서 기존 머리 절단·분리 조각·캐릭터 교체·심각한 Alpha/Scale 문제 없음. 실제 물리 입력 걷기 품질은 미검증.|
|appearance.external.v1.intellectual.guardian.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Guardian_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.intellectual.guardian.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Guardian_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.intellectual.healer.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Healer_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.intellectual.healer.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Healer_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.intellectual.mage.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Mage_Female.png|Female|PASS|0-15|Up 머리/양쪽 리본 윤곽 복원. 전체16프레임 정상.|
|appearance.external.v1.intellectual.mage.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Mage_Male.png|Male|PASS|0-15|Up 머리카락 끝 복원. 전체16프레임 정상.|
|appearance.external.v1.intellectual.marksman.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Marksman_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.intellectual.marksman.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Intellectual/Intellectual_Marksman_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.mobility.fighter.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Fighter_Female.png|Female|BLOCKED_ART|4-15|Left/Right 발 아래 다른 행의 머리 조각, Right 및 Up 머리 상단 수평 절단. 일부 검 끝 조각이 셀 옆에 분리됨.|
|appearance.external.v1.mobility.fighter.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Fighter_Male.png|Male|BLOCKED_ART|4-15|Left/Right 발 아래 머리·무기 조각과 옆 셀의 검/망토 조각. Up 머리 상단 수평 절단.|
|appearance.external.v1.mobility.guardian.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Guardian_Female.png|Female|BLOCKED_ART|4-15|Left 머리 위 분리된 바퀴 조각, Right 발 아래 다음 행 머리 조각. Up 머리 상단 수평 절단.|
|appearance.external.v1.mobility.guardian.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Guardian_Male.png|Male|BLOCKED_ART|8-15|Right 발 아래 다음 행 머리 조각, Up 머리 상단 수평 절단.|
|appearance.external.v1.mobility.healer.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Healer_Female.png|Female|BLOCKED_ART|4-15|Left/Right 발 아래 다른 행의 금발 머리 조각. Up 머리 상단 수평 절단.|
|appearance.external.v1.mobility.healer.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Healer_Male.png|Male|BLOCKED_ART|4-15|Left 머리 위 바퀴 조각, Right 발 아래 머리·장비 조각. Up 머리 상단 수평 절단.|
|appearance.external.v1.mobility.mage.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Mage_Female.png|Female|BLOCKED_ART|4-15|Left/Right 발 아래 다른 행의 머리 조각. Up 머리 상단 수평 절단.|
|appearance.external.v1.mobility.mage.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Mage_Male.png|Male|BLOCKED_ART|4-15|Left 머리 위 바퀴 조각, Right 발 아래 다음 행 머리 조각. Up 머리 상단 수평 절단.|
|appearance.external.v1.mobility.marksman.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Marksman_Female.png|Female|BLOCKED_ART|4-15|Left/Right 발 아래 다른 행의 금발 머리 조각. Up 머리 상단 수평 절단.|
|appearance.external.v1.mobility.marksman.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_Marksman_Male.png|Male|BLOCKED_ART|0-15|Down 발 아래 잔여 조각, Left/Right 발 아래 다음 행 머리 조각. Up 머리 상단 수평 절단.|
|appearance.external.v1.vision.fighter.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Fighter_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.vision.fighter.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Fighter_Male.png|Male|PASS|0-15|전체16Frame 방향 정상. Left5/7·Right9/11 검 끝이 뾰족한 닫힌 윤곽으로 복원되고 검/머리/망토/발이 셀 안에 포함됨. 절단 단면/외딴 조각/인접 셀 침범 없음.|
|appearance.external.v1.vision.guardian.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Guardian_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.vision.guardian.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Guardian_Male.png|Male|PASS|0-15|Left 망토·Right/Up 머리 윤곽 복원, Frame6 분리 조각 없음. 전체16프레임 정상.|
|appearance.external.v1.vision.healer.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Healer_Female.png|Female|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.vision.healer.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Healer_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.vision.mage.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Mage_Female.png|Female|PASS|0-15|전체16Frame 정상. Up12-15 머리 상단·머리카락/장식·지팡이 꼭대기가 온전하며 수평 접합선/이중 봉우리/지팡이 이중 원/접합 어긋남 없음. 인접 셀 침범/잔여 조각 없음.|
|appearance.external.v1.vision.mage.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Mage_Male.png|Male|PASS|0-15|Down/Left/Right/Up 전체 프레임과 6fps Walk를 이전 승인된 실제 Game View에서 확인. 심각한 잘림·캐릭터 교체 없음.|
|appearance.external.v1.vision.marksman.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Marksman_Female.png|Female|PASS|0-15|Left5/6 망토 끝 닫힌 외곽 윤곽 복원. 전체16프레임에서 심각한 crop/조각/방향/Alpha 손상 없음. 경계의 미세 antialias와 정상 발바닥 선은 잘림으로 단정하지 않음.|
|appearance.external.v1.vision.marksman.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Vision/Visual_Marksman_Male.png|Male|PASS|0-15|전체16Frame Left/Right/Up 방향 정상. Up12/15 머리 끝 및 Up13 우측 망토 끝이 셀 안에서 닫힌 윤곽으로 복원됨. 검은 외딴 조각/경계 노출/인접 셀 침범 없음.|
