# Hearing 최종복원10종 QA

입력 `F:\Downloads\Limitless_Hearing_Path_10_Restored_Final.zip`, SHA256 `a6a3828a123b8373302d9ff8dd0e1949385b8c0de5df4f28fa0b6e718c75e8dc`. 문서화 선행 후 원본10PNG 바이트 그대로 교체, 이미지수정0. Male5/Female5/Job5×Gender2·PNG10·중복0/누락0·512×512 RGBA/4×4/16Frame/Cell128 확인. 새160Frame 전체 관찰 및Alpha 연결성분 검사.

2026-10-05 Hearing 최종10종 원본 반영·새160Frame QA: READY0/BLOCKED_ART10, 전체 **READY30/BLOCKED20 유지**. Mobility 포함 다른40종 보존. [최신 QA](Hearing_RestoredFinal10_QA.md).

|Path|Job|Gender|판정|남은0-basedFrame|정확한 위치/파츠/사유|
|---|---|---|---|---|---|
|Hearing|Fighter|Female|BLOCKED_ART|4,8,9,10|발 아래 검은 외딴 선. Frame4 bbox[62,124,69,125),8[60,121,70,123),9[60,121,66,123)+1px,10[60,122,65,123). 본체와 분리된7/16/11/5px 잔여 조각.|
|Hearing|Fighter|Male|BLOCKED_ART|4-7,13,15|Left4-7 발 아래 분리된 살색/검은 파츠84/68/76/68px, bbox 각각[65,121,89,126),[66,122,89,126),[62,122,86,126),[65,122,88,126). Up13/15 머리 위 검은 수평선9/8px bbox[77,2,86,3),[76,2,84,3).|
|Hearing|Guardian|Female|BLOCKED_ART|1,2,4,5,6,9|Down1/2 및 Left4-6 오른쪽 셀 가장자리의 분리된 검은/금색 잘린 파츠. Frame1[123,111,126,122)24px,2[123,116,125,124)14px,5[123,104,126,112)19px. Right9 왼쪽[2,102,6,114)35px 검 파츠와 발 아래[56,118,69,121)26px 분리 조각.|
|Hearing|Guardian|Male|BLOCKED_ART|1,8|Frame1 우측 아래 검은 세로 조각[125,114,126,122)8px, Frame8 우측[124,81,126,88)11px 검은/금색 외딴 파츠. 셀 여백 안으로 옮겨져도 본체와 분리된 절단 조각이 남음.|
|Hearing|Healer|Female|BLOCKED_ART|4-7,10,13,15|Left4-7 발 아래 살색/검은 발 파츠60/48/43/47px 분리. Frame4[58,121,78,126),5[61,122,77,126),6[58,122,73,126),7[62,122,77,126). Right10 왼쪽[2,31,7,49)68px 지팡이 효과/테두리 절단 조각. Up13/15 머리 위 검은 조각[83,2,86,3)3px/[82,2,90,4)13px. 정상 지팡이 주위의 의도된 음파 발광 자체는 문제로 세지 않음.|
|Hearing|Healer|Male|BLOCKED_ART|0,1,5,9,11|Frame0/1/5 우측 셀 가장자리 지팡이 발광/테두리 잘린 분리 파츠[123,42,126,53)26px/[121,34,126,48)48px/[123,46,126,58)27px. Right9 좌측[2,42,4,49)11px 및 우측[124,89,126,97)12px, Right11 좌측[2,31,9,51)92px 금색/빨간 지팡이 파츠가 본체와 떨어져 절단된 모양으로 남음. 정상 음파 효과와 구분.|
|Hearing|Mage|Female|BLOCKED_ART|1,2,3,5,6,7,9,10,11|셀 좌우에 이웃 지팡이/장식 절단 조각 잔존. 대표Frame1 왼쪽[2,67,7,95)80px+[2,98,6,107)29px, Left5 왼쪽[2,60,10,94)197px 빨간/금색 지팡이·매달린 장식 조각, Left6[2,77,8,97)77px, Right9 왼쪽[2,23,8,42)62px 빨간 장식 절단 조각. 머리/귀장치는 개선됐으나 이웃 파츠 제거 미완료.|
|Hearing|Mage|Male|BLOCKED_ART|9,10|Right9/10 왼쪽 가장자리[2,43,7,60)54px/[2,47,5,58)27px 빨간/흰 지팡이 원 테두리 절단 조각이 본체와 분리됨. 정상 음파 발광으로 볼 수 없는 잘린 곡선 파츠.|
|Hearing|Sharpshooter|Female|BLOCKED_ART|3,4,6,11,15|Frame3 왼쪽[2,67,6,82)43px 머리/파츠 조각. Left4/6 및 Right11 발 아래 살색 발 조각[69,122,86,126)43px/[64,123,77,126)28px/[51,120,73,125)79px. Up15 왼쪽[2,74,7,88)55px 잘린 파츠. Frame5/7/13에도 작은 잔여 조각.|
|Hearing|Sharpshooter|Male|BLOCKED_ART|5,7,8,9,10,11,15|Left5 왼쪽[2,84,5,91)16px 잘린 파츠. Right8-11 머리 위 분리된 검은 수평선/머리 조각7/18/7/17px: [48,6,55,7),[33,6,43,8),[36,2,43,3),[39,2,49,4). Frame7/15 머리 위에도2/3px 잔여 조각.|

좌표는각128px 셀좌상단 기준 bbox[x0,y0,x1,y1), 성분수는Alpha>10/8방향연결. 본체/머리/귀장치는 개선되고 셀 Alpha여백이 있으나 잘린파츠가여백 안에남아READY 승격 불가. 정상 음파 효과와 무관한 잔여선/발/지팡이·이웃파츠를 구분하여 판정했다. Empty0/CharacterSwap0/방향오류0·심각한Scale/Alpha손상 없음. 다른40종/기존READY30 재판정 없음. ID/meta/Import/160Sprite 및 기존240Clip 보존, 새Clip0. BLOCKED 기본성별fallback 유지.

백그라운드 격리PlayUnfocused **88PASS/0FAIL**. Hearing10조합 전체 StableID 자동Mapping/JobPreview/Blocked표시 정상. 대표Male Fighter/Female Healer의 CharacterCreation 성별Preview/최종확인Preview/World/BattleLeftIdle·실제BattleScene/Save→BootstrapContinue 정상. 모두BLOCKED이므로 **기존기본성별fallback 표시**를 확인했으며 새HearingArt Runtime 표시 통과로 보고하지 않는다. 누락/불일치ID 재계산·PathJob없는옛Save fallback 통과, SaveMigration/Runtime 기능변경 없음.

컴파일Error0·기존CS0618 경고2건(ExternalAssetImportEditor.cs53/59)만기록. 최종Console Error0/Warning0. cleanBootstrap EditMode/격리Save·Settings/Play설정복원, 포커스전환 없음. 실제걷기/물리키보드/게임패드 미검증.

보존검사PASS: 원본10PNG/ZIP Hash동일·모든meta/160SpriteID/Import·다른40PNG/Entry/QA 판정/기존240Clip 블록동일. 새Clip0. Assets/ProjectSettings/UserData 변경은10PNG+Catalog+Inventory+QAhelper13파일뿐, Save/Settings불변. 기존사용자94항목(410Git 파일)보존. 직접변경 diff--check 통과, 전체workingtree의기존whitespace는보존. [세부QA](Hearing_RestoredFinal10_QA.json)·[Runtime결과](Hearing_RestoredFinal10_Runtime_Results.txt). 다음권장작업은 위Frame/좌표의분리파츠를원본Art에서보정하며이번Codex는수정하지않았다. GitHubPush 없음.
