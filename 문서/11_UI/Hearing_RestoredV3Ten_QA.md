# Hearing v3 복원10종 QA

입력 `F:\Downloads\Limitless_Hearing_Path_10_Restored_v3.zip`, SHA256 `e2a7044a46ac735a55cff22541b9f0ee1d5cd52a8e432b6edfb902f892d67985`. 문서화 선행 후 원본10PNG 교체. 10PNG/Male5/Female5/Job5×Gender2·중복0/누락0·512×512 RGBA·4×4/16Frame/Cell128 확인. Sharpshooter 파일명은기존Marksman asset/ID에만대응. 이미지수정0/C#수정0.

2026-10-05 Hearing v3 새160Frame QA: **READY10/BLOCKED0·전체READY40/BLOCKED10**. Mobility 포함다른40종/기존READY30 보존. [최신QA](Hearing_RestoredV3Ten_QA.md). 백그라운드88PASS/0FAIL 완료.

|Path|Job|Gender|판정|남은0-basedFrame|v3 실제검수결과|
|---|---|---|---|---|---|
|Hearing|Fighter|Female|READY|없음|v2 Frame6/8/10 잔여선/조각 해소. 16Frame 검/머리/발/망토 윤곽과 귀장치·방향 정상.|
|Hearing|Fighter|Male|READY|없음|v2 Down1 검파츠와 Left/Right 발 아래 조각 해소. 16Frame 검 끝/머리/망토/발 완전.|
|Hearing|Guardian|Female|READY|없음|v2 Down1/2·Left6·Right9·Up14 셀 옆 검파츠/발 조각 해소. 16Frame 방패/검/포니테일/귀장치 정상.|
|Hearing|Guardian|Male|READY|없음|v2 Right8/10 우측 분리 파츠 해소. 16Frame 무기/방패/머리/망토/귀장치 정상.|
|Hearing|Healer|Female|READY|없음|v2 지팡이/장식 잘린 파츠와 머리 위 선 해소. 16Frame 지팡이 꼭대기/귀장치/발 정상.|
|Hearing|Healer|Male|READY|없음|v2 Left/Right 지팡이 파츠·발밑 선/점 해소. 16Frame 정상 의도된 지팡이 발광만 유지.|
|Hearing|Mage|Female|READY|없음|v2 Down2/Left6 큰 이웃장식과 다른 셀 좌우 지팡이 조각 해소. 16Frame 귀장치/지팡이/장식 일관.|
|Hearing|Mage|Male|READY|없음|v2 Right9-11 잘린 원/잔여선과Up14 점 해소. 16Frame 지팡이/머리/귀장치/발 정상.|
|Hearing|Sharpshooter|Female|READY|없음|v2 발 아래 파츠 해소, Up12/14 포니테일 상단이 둥근 닫힌 윤곽으로 복원. 16Frame 활/화살/귀장치/방향 정상.|
|Hearing|Sharpshooter|Male|READY|없음|v2 Left 발밑 점 및 Up13/15 머리 위 검은선 해소. 16Frame 활/머리/귀장치/망토/방향 정상.|

새160Frame 전체확대관찰: 가시Head/Device/Hair/Foot/Weapon/Staff/Cloak crop 및분리파츠/인접셀침범/방향오류/CharacterSwap/Empty/심각한Scale/Alpha손상없음. 귀보조장치존재/방향유지. Alpha>10자동검사 일부1-4px 성분은Alpha11-29의미세가장자리이며대부분Alpha>0에서는본체와연결됨. 이를선명한v2분리파츠와동일시하지않고육안관찰과함께판정했다. RGBA가완전히단일성분/노이즈0이라는주장은하지않는다.

기존10AppearanceID/160SpriteID/meta/Import/SaveMapping·fallback 유지. 기존대상Entry에만8Clip씩총80추가완료, 다른40PNG/Entry/QA/기존240Clip 보존.  다음이미지세션최소수정리스트: 없음. 다음대상Mobility10종입력수령/계획수립준비(이번작업에서는변경하지않음).

백그라운드격리PlayUnfocused **88PASS/0FAIL**. Hearing10조합 자동Mapping/JobPreview v3Frame0 정상. 대표MaleFighter/FemaleHealer의CharacterCreation 기본성별Preview·최종확인v3Preview/WorldFrame0/BattleLeftIdle Frame4/실제BattleScene/Save→BootstrapContinue v3적용 통과. 누락/불일치ID재계산/PathJob없는옛Save 기본fallback 유지. 수동UI/SaveMigration/C#기능변경 없음.

컴파일요청완료·현재컴파일Error0, 최종ConsoleError0/Warning0. 이전CS0618 2건이력보존/현재재발생없음. cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/물리키보드/게임패드미검증.

보존PASS: ZIP/원본10PNG bytes동일, 모든meta/160SpriteID/Import동일, 다른40PNG/Entry/QA·기존240Clip동일. Hearing기존10Entry에만80Clip추가. Assets/ProjectSettings/UserData 변경은10PNG+Catalog+Inventory12파일뿐, 모든C#/사용자Save/Settings불변. 기존사용자94항목(410Git 파일)보존. [세부QA](Hearing_RestoredV3Ten_QA.json)·[Runtime결과](Hearing_RestoredV3Ten_Runtime_Results.txt). 직접변경diff--check통과, 기존사용자whitespace보존. GitHubPush없음.

검증과정MCP WebSocket not initialised 일시경고1건이발생했으나도구재연결/Runtime정상통과, 최종Console0. 프로젝트C#문제와구분해기록하며범위밖MCP패키지수정없음.
