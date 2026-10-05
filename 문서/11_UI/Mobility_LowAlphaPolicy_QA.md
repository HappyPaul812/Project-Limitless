# Mobility low-alpha 정책 재판정

정본: 현재LOCAL의 v2 redownload PNG9종. 원본/모든50PNG는수정하지않음. [정책](Player_Sprite_QA_Policy.md)·[거리/좌표/Alpha 세부정보](Mobility_LowAlphaPolicy_QA.json). 이전QA의가시적오류해소검수와144Frame 시각자료를확인했다. 이전의non-zero Alpha 일괄차단은노이즈오탐이었으며 새정책으로만재판정. 기존READY41종/HealerFemale은재검사하지않음.

|Job|Gender|노이즈MaxAlpha|경고성분수|최대성분px|최대bbox변길이px|판정|실제가시Art오류|
|---|---|---|---|---|---|---|---|
|Fighter|Female|6|27|30|13|READY|없음|
|Fighter|Male|4|40|8|6|READY|없음|
|Guardian|Female|4|8|17|6|READY|없음|
|Guardian|Male|7|19|30|12|READY|없음|
|Healer|Male|16|29|33|14|READY|없음|
|Mage|Female|3|9|14|8|READY|없음|
|Mage|Male|7|28|31|14|READY|없음|
|Sharpshooter|Female|1|10|2|2|READY|없음|
|Sharpshooter|Male|4|16|17|9|READY|없음|

총LOW_ALPHA_NOISE/NON_BLOCKING_WARNING 186개(성분단위). 모든성분은작고비가시이며실루엣/휠체어/바퀴/장비/방향/셀침범 영향없음. 큰면적/긴선/연속4Frame동일bbox 반복후보0. 회귀11PASS: A머리절단/B가시분리조각/C인접장비 BLOCKED, DAlpha1-16작은노이즈경고/READY, E정상READY. 셀내절단/큰면적/긴선/반복/실루엣오류/빈Frame/시각검수누락도검증.

작업전41/9→승격9→전체50/0. 검증완료.

최소회귀: EditMode9Mapping+72Clip curve 총81PASS, Preview/World Frame0 참조9정상, 격리PlayUnfocused BattleLeftIdle9PASS. 이전85PASS 전체흐름 재실행없음. Save→Continue는기존85PASS 이력과StableID/정본Mapping/Save코드불변으로보호, 실제Save→Continue 반복검증하지않음.

검증중Runtime전용Battle해석기를EditMode에서호출해Destroy오류9건발생. 생성된숨은임시객체9개정리후properPlay에서최소Battle검사9PASS. 게임코드변경없이해소, 최종ConsoleError0/Warning0·현재컴파일Error0. 모든C#/사용자Save/Settings불변, cleanBootstrap EditMode/Play설정복원/포커스전환없음.

보존PASS: PNG50개/meta/import/대상144SpriteID/StableID 동일, HealerFemale 포함기존41Entry/QA/기존328Clip동일. 통과9Entry에만72Clip추가(전체400). Assets/Settings/UserData 변경은Catalog/Inventory2파일뿐. 사용자410Git항목보존. 정책/QA helper 및회귀테스트는Tools/SpriteQA에저장. PNG Stage0/수정0·GitHubPush없음. [최소Runtime결과](Mobility_LowAlphaPolicy_Runtime_Results.txt).
