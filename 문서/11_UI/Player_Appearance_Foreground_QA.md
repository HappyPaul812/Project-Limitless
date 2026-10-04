# Player Appearance Foreground 최종 판정

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
|appearance.external.v1.hearing.fighter.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Fighter_Female.png|Female|BLOCKED_ART|BLOCKED_ART|5-15|Right 발 아래 인접 Up 머리 조각, Up 머리 상단 잘림, Left 일부 머리카락 경계 절단.
|appearance.external.v1.hearing.fighter.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Fighter_Male.png|Male|BLOCKED_ART|BLOCKED_ART|5-15|Left 망토 절단·인접 조각, Right 발 아래 머리 조각과 Up 머리 상단 잘림.
|appearance.external.v1.hearing.guardian.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Guardian_Female.png|Female|BLOCKED_ART|BLOCKED_ART|8-15|Right 발 아래 다음 행 머리 조각과 Up 머리 상단 잘림, 일부 무기·머리카락 경계 절단.
|appearance.external.v1.hearing.guardian.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Guardian_Male.png|Male|BLOCKED_ART|BLOCKED_ART|8-15|Right 발 아래 다음 행 머리 조각과 Up 머리 상단 잘림.
|appearance.external.v1.hearing.healer.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Healer_Female.png|Female|BLOCKED_ART|BLOCKED_ART|1,3,5-15|인접 셀 머리카락 조각, Right 발 아래 잔상과 지팡이 경계 절단, Up 머리 상단 잘림.
|appearance.external.v1.hearing.healer.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Healer_Male.png|Male|BLOCKED_ART|BLOCKED_ART|1-15|분리된 지팡이·망토 조각, Right 발 아래 머리 조각, Up 머리 상단 잘림.
|appearance.external.v1.hearing.mage.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Mage_Female.png|Female|BLOCKED_ART|BLOCKED_ART|1,5-15|분리된 빨간 지팡이 장식·인접 조각, Right 발 아래 잔상과 Up 머리 상단 잘림.
|appearance.external.v1.hearing.mage.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Mage_Male.png|Male|BLOCKED_ART|BLOCKED_ART|8-15|Right 인접 지팡이 장식·머리 조각, Up 머리 상단 잘림.
|appearance.external.v1.hearing.marksman.female|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Marksman_Female.png|Female|BLOCKED_ART|BLOCKED_ART|1-15|인접 머리카락·무기 조각, Right 발 아래 다음 행 머리 조각, Up 머리 상단 잘림.
|appearance.external.v1.hearing.marksman.male|Unity/Client/Assets/_Project/Art/Characters/Player/Validated50/Hearing/Hearing_Marksman_Male.png|Male|BLOCKED_ART|BLOCKED_ART|5-15|Left 망토 절단·인접 조각, Right 발 아래 머리 조각과 Up 머리 상단 잘림.
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
