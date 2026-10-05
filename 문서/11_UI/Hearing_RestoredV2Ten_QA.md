# Hearing v2복원10종 QA

입력 `F:\Downloads\Limitless_Hearing_Path_10_Restored_v2.zip`, SHA256 `8eda5abebdfd207e87e39e3fbca624f3afc0720ae1251f7929849306435ca93e`. 문서화 선행 후 원본10PNG 바이트 그대로 교체, 이미지수정0. Male5/Female5/Job5×Gender2·PNG10·중복0/누락0·512×512 RGBA/4×4/16Frame/Cell128 확인. 새160Frame 전체 관찰 및Alpha 연결성분 검사.

2026-10-05 Hearing v210종 원본 반영·새160Frame QA: READY0/BLOCKED_ART10, 전체 **READY30/BLOCKED20 유지**. Mobility 포함 다른40종 보존. [최신 QA](Hearing_RestoredV2Ten_QA.md).

|Path|Job|Gender|판정|남은0-basedFrame|정확한 위치/파츠/사유|
|---|---|---|---|---|---|
|Hearing|Fighter|Female|BLOCKED_ART|6,8,10|이전4/9 발밑선 해결,8은축소됐지만[60,121,65,122)5px 검은선잔존. 새Frame6 우측망토옆[124,110,125,113)3px 검은조각,10 머리/망토옆[23,74,24,75)1px 분리점. 머리/귀장치/검/방향정상.|
|Hearing|Fighter|Male|BLOCKED_ART|1,4-6,8-11|이전Left 큰발조각과Up13/15 머리위선 해소. Left4-6 발밑2-3px선 및5 우측[123,108,125,112)5px 조각잔존. 새Down1 우측[120,107,125,118)34px 빨간/검은검파츠, Right8-11 발아래[60,119,71,122)26px+[56,121,58,122)2px/[57,119,67,122)25px/[57,122,68,125)25px/[60,122,71,125)27px 살색/검은분리파츠.|
|Hearing|Guardian|Female|BLOCKED_ART|1,2,6,9,14|이전Left4/5 조각해소. Down1/2 우측[122,109,124,116)10px/[123,108,125,116)13px,Left6[123,102,125,107)6px 검은파츠잔존. Right9 좌측[3,101,7,113)35px 검끝조각과발밑[59,118,74,121)32px. 새Up14 좌측[6,85,10,97)34px 검파츠.|
|Hearing|Guardian|Male|BLOCKED_ART|8,10|이전Down1 조각해소. Right8 우측[123,81,125,87)10px 검은/금색절단파츠잔존. 새Right10 우측[124,82,125,85)3px 조각. 귀장치/머리/방향정상.|
|Hearing|Healer|Female|BLOCKED_ART|0,1,3,4,5,7,9,10,11,15|이전큰발조각과Up13 상단선해소. Frame0 우측[122,36,125,52)37px 지팡이잘린파츠,4[121,38,125,53)38px 및머리위[66,7,71,8)5px 검은선,7 좌측[3,63,7,79)43px 잘린장식. Right9/10/11 좌측지팡이파츠[3,34,7,52)47px/[3,33,10,54)91px/[3,29,13,55)184px. 머리위선9[39,3,47,5)13px/11[46,5,53,7)11px/Up15[87,3,93,5)9px. Up15좌측[5,28,8,39)24px조각. 정상음파효과자체는실패로세지않음.|
|Hearing|Healer|Male|BLOCKED_ART|1,3,5,8,9,10,11|이전0 우측큰파츠해소. Right9 좌측[3,42,5,49)12px,11 좌측[4,33,10,50)58px 잘린빨간/금색지팡이파츠잔존. Left5 좌측[3,93,4,101)8px 검은선. 발아래Frame8[59,120,62,121)3px,10[57,122,61,123)4px+[50,122,53,123)3px+1px. 1 우측3px/3,5에1px 잔여점. 귀장치/방향정상, 정상지팡이발광과구분.|
|Hearing|Mage|Female|BLOCKED_ART|0,1,2,3,5,6,7,9,10,14|이전Frame1 큰장식조각축소. 새Down2 좌측[3,68,9,106)142px 금색/빨간매달린장식파츠,Left6 좌측[3,64,9,102)140px 동일이웃장식. Left5[3,76,7,94)43px+[3,68,5,75)11px 잔존. Right9[4,28,6,41)21px,10[3,30,5,43)22px 좌측지팡이/장식절단조각,10우측9+3px. 0/1/3/7/14 작은잔여점. 정식귀장치보존/방향정상.|
|Hearing|Mage|Male|BLOCKED_ART|9,10,11,14|Right9 왼쪽[3,45,6,59)33px 빨간/흰지팡이원잘린파츠잔존. Right10[3,51,4,55)4px와11[3,51,4,54)3px잔여선,10[33,65,34,66)1px 및Up14[83,48,84,49)1px분리점. 다른행귀장치/머리/방향정상.|
|Hearing|Sharpshooter|Female|BLOCKED_ART|0,4,6,7,8,9,11,12,14|이전3/13/15 셀옆큰파츠해소. Left4/6/7 발아래[70,122,79,124)14px/[67,123,73,124)6px/[69,123,74,124)5px잔존. 새Right8/9 발조각[51,117,71,122)58px/[55,119,73,122)34px. Up12/14 포니테일상단이셀y=3부근에서넓은수평단면으로잘림(셀여백은있으나원래머리끝윤곽불완전). 0/9/11 작은분리점. 귀장치보존/방향정상.|
|Hearing|Sharpshooter|Male|BLOCKED_ART|4,5,7,13,15|이전Right8-11 머리위선/Left5 큰셀옆파츠해소. 새Up13 머리위[44,3,52,4)8px+[78,3,82,4)4px검은선,Up15[80,3,84,4)4px검은선잔존. Left4발아래1px/5발아래2+2px/7발아래3+2px분리점. 귀장치/방향정상.|

좌표는각128px 셀좌상단 기준 bbox[x0,y0,x1,y1), 성분수는Alpha>10/8방향연결. 귀장치의존재/방향유지 확인. 머리/귀장치는대체로개선되고셀 Alpha여백이 있으나 잘린파츠가여백 안에남아READY 승격 불가. 정상 음파 효과와 무관한 잔여선/발/지팡이·이웃파츠를 구분하여 판정했다. Empty0/CharacterSwap0/방향오류0·심각한Scale/Alpha손상 없음. 다른40종/기존READY30 재판정 없음. ID/meta/Import/160Sprite 및 기존240Clip 보존, 새Clip0. BLOCKED 기본성별fallback 유지.

백그라운드 격리PlayUnfocused **88PASS/0FAIL**. Hearing10조합 전체 StableID 자동Mapping/JobPreview/Blocked표시 정상. 대표Male Fighter/Female Healer의 CharacterCreation 성별Preview/최종확인Preview/World/BattleLeftIdle·실제BattleScene/Save→BootstrapContinue 정상. 모두BLOCKED이므로 **기존기본성별fallback 표시**를 확인했으며 새HearingArt Runtime 표시 통과로 보고하지 않는다. 누락/불일치ID 재계산·PathJob없는옛Save fallback 통과, SaveMigration/Runtime 기능변경 없음.

컴파일요청완료·현재 Error0/Warning0. 기존CS0618 2건(ExternalAssetImportEditor.cs53/59)은직전검증이력으로만보존하며이번조회에서는재발생없음. 최종Console Error0/Warning0. cleanBootstrap EditMode/격리Save·Settings/Play설정복원, 포커스전환 없음. 실제걷기/물리키보드/게임패드 미검증.

보존검사PASS: 원본10PNG/ZIP Hash동일·모든meta/160SpriteID/Import·다른40PNG/Entry/QA 판정/기존240Clip 블록동일. 새Clip0. Assets/ProjectSettings/UserData 변경은10PNG+Catalog+Inventory12파일뿐, Save/Settings불변. 기존사용자94항목(410Git 파일)보존. 직접변경 diff--check 통과, 전체workingtree의기존whitespace는보존. [세부QA](Hearing_RestoredV2Ten_QA.json)·[Runtime결과](Hearing_RestoredV2Ten_Runtime_Results.txt). 다음권장작업은 위Frame/좌표의분리파츠를원본Art에서보정하며이번Codex는수정하지않았다. GitHubPush 없음.
