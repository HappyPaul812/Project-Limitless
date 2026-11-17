# Issue109 — 승인 Main13 레온 전용 외형 원본 적용 / Issue110 보존 확인

2026-11-17 / **USER_ART_APPROVED / PORTRAIT_OUT_OF_SCOPE / DELTA178PASS_0FAIL / LOCAL_ORIGINAL_WORKING_TREE_APPLIED / ORIGINAL_COMPILE_ERRORS0 / ISSUE110_ALREADY_DONE_NOT_REPEATED / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

바울 대표의최종승인·초상화제작제외지시와 [Issue109 승인](https://github.com/HappyPaul812/Project-Limitless/issues/109#issuecomment-6940447233), [기존오프라인실물검수](https://github.com/HappyPaul812/Project-Limitless/issues/109#issuecomment-6940006218)를확인했습니다. 공식초상화미발견은더이상적용차단조건이아니며초상화/새대사는제작하지않았습니다.

## 전용 Sprite·체격·원본 보호

- 입력 `F:/Downloads/Limitless_Main13_Leon.zip` 실제SHA256 **`dfaaf5fe453dadeaa268a24edbdcefafb6a9b0266db4c6e6251131b741d6504a`** 및CRC재확인PASS. 기존Manifest7/SUMS8/512RGBA/직접128·40/실제비교판검수PASS를재사용했습니다.
- PNG SHA256 **`ff7cfd081e7727a7ab228bae0a5ece879727b98594514c932d4a616396bc8fdc`**,512×512투명RGBA. 픽셀수정·재생성0.
- 전용 Resources **`VillageNpcSprites/Eldiran/Main13_Leon_Approved`**, GUID **`ed9108a8402b48d3a8ae5c2b9b4a8769`**, **PPU360.5 / Pivot(.5,.083984375)=(256,43)px**.
- alpha8bbox(158,57,354,469)의x중심256/하단469→Pivot아래43px. 가시높이412/360.5=**1.142857WU**, 기존마을NPC32/28과동일. 하단접지0·좁은몸통비율은승인원화그대로,TransformScale1/색white/Sorting5/Collider·위치**Arbel(0,3)** 유지.
- **arbel-leon 매핑1개만** 기존OGA09공유경로→새전용Sprite. `starter-village-main-guide` 및다른OGA09참조/승인원본PNG/meta/GUID/PPU/Pivot는그대로입니다.

## 이름표·E·목표판 가림 방지의 최소 범위

처음전용Sprite+매핑만준비한QA에서기존큰파란목표판이E안내위치에겹치는문제가재현됐습니다. 원본적용전에보완했습니다.

- 레온의기존Role.Resident와상시이름표숨김정책유지. 대화/퀘스트목표판의이름은기존“레온”입니다. 일반NPC이름표Y1.37/문자크기·색·숨김정책변경0.
- 기존레온E Y1.48/상자높이54·QuestMarkerY1.92/높이50/표시조건불변. E하단-머리약0.0671WU/퀘스트표식하단-머리약0.5271WU로실제새Sprite머리위간격확인.
- `VillageNpcAppearanceCatalog`에서 **stableID가arbel-leon일때만** `ArbelLeonGuideClearance`를한번부착합니다. 같은파일내전용MonoBehaviour는공용목표판계산후현재추적목표ID가arbel-leon이고화면안일때만큰판을 **오른쪽286px(판270+여백16)** 옮기고뷰포트/상단114px아래로제한합니다. 월드목표Transform/거리/StableID/화면밖방향의의미는그대로입니다.
- 오른쪽은같은높이의공식세린이있는왼쪽을피한선택입니다. 최종QA에서모든활성역할NPC의실불투명픽셀과큰판의겹침0을확인했습니다. 다른NPC/다른추적목표에는보완이적용되지않으며공용QuestNavigationPresenter/기존Main04·Main07보완코드는변경하지않았습니다.

## 실제 변경 파일 — 총3개

- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/Main13_Leon_Approved.png`
- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/Main13_Leon_Approved.png.meta`
- `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs`

제품카탈로그변경은레온매핑1개+레온전용안내보완입니다. PNG/meta는새레온전용파일이고기존NPC파일은덮어쓰지않았습니다. 새Scene/초상화/대사/퀘스트데이터추가0.

## 최소 Delta QA — 178 PASS /0 FAIL

Unity6000.6.5f1 기존 `Temp/Art95_91Unity20261111/QA/Client` 재사용·숨김Batch·OS포커스전환/새대형복제/전체QA반복0. 합성Save/설정은 `Temp/Issue109LeonApply20261117/Evidence/`에분리했습니다.

- 실제Arbel레온승인Sprite·Resources/PPU/Pivot/기존좌표·Scale·Role·이름PASS.
- **HUD ON960×720/1280×720/1680×720 ×Main13최종대화목표/Main14시작/Main14최종보고3상태**의9화면·Main13대화1화면. 이름표기존숨김·E활성/기존위치·E/QuestMarker머리상단간격·뷰포트·큰판-E상자분리확인. 실제각NPC PNGalpha>.5픽셀좌표와큰판bounds를비교해레온/세린/상인/치유사/파티안내인/펫NPC/주민/Portal등역할NPC겹침0.
- NpcController/StableID/대사문자열·Collider/Transform/이름표직렬화값보존PASS. 직접E·패드A실제대화진입. Main13기존환영문구·최종목표완료,Main14기존우물의뢰문구·다음목표진행,Main14기존조사보고문구·완료PASS. 새대사/초상화/화자ID변경0.
- 앞선월드조사·전투목표는합성저장으로준비한 **레온대화분기표적검증**입니다. Main13/14전체여정·전체전투를재플레이했다고주장하지않습니다. Main15이후동일NPC공유경로는기존로직을그대로사용하며모든후반대화전수검증도아닙니다.
- 합성Save쓰기/읽기/RestoreSession/Arbel재진입으로전용매핑·Main14완료기록보존PASS. Field03파티안내인·Arbel파티안내인기승인OGA10연결유지,시작마을대표OGA09원본사용유지PASS.
- 새런타임오류0. 기존격리UnityEditor.Search초기화예외는EnvironmentErrors로분리기록,전체격리EditorConsole0이라고주장하지않습니다. 검증전보조코드메서드명충돌은QA에서만수정후최종컴파일/검사완료,원본에검증코드를복사하지않았습니다.

근거 `Evidence/checks.txt`178PASS·Errors.txt빈파일·status.txtPASS·PNG10장, `Unity-FinalDelta.log`. 원본PNG/이름표를숨기거나HUD를끄고합격처리하지않았습니다(레온상시이름표숨김은기존정책그대로).

## Issue110 완료 상태 — 반복하지 않음

[Issue110 완료보고](https://github.com/HappyPaul812/Project-Limitless/issues/110#issuecomment-6940618986)의제품`6c81a03d229f0075b39dcbfa07656182e7f6bae5`·85PASS를재사용했습니다. 현재원본·작업브랜치·QA에두파티ID모두`OGA10_PartyManager_HandFix_v2_Approved`인것을확인했고이번QA에서도두Scene재진입실물연결을확인했습니다. **두매핑재수정/재커밋·파티기능전체QA반복0**,이번레온코드에그결과를보존했습니다. #110에서발견한잔여OGA17/OGA11·단색placeholder는이번에도변경하지않았습니다.

## 원본 적용·보호·대표 확인

**178PASS후검증3파일만원본working tree적용**. 표적PNG/카탈로그Import후실제Resources/GUID/PPU360.5/Pivot256,43/전용안내Type/마을OGA09/#110두맵/portraitNONE확인. **Unity6000.6.5f1·compileFalse·Console오류0/경고104**,Bootstrap/PlayFalse/dirtyFalse/선택null/포커스null유지. 원본Scene/Play/화면포커스전환0.

제품commit **`c00fbb63c65c67d36a2cd59736f6822c1f3368c3`**,브랜치 `codex/issue109-leon-approved-20261117`. rootmainHEAD `52f84e643d5d860665eadfe1614a30c745209f00` 그대로·main직접commit/Push/Merge/reset/clean/stash0. 원본main에제품commit을합류시킨것이아니라검증파일working tree적용입니다.

보호8354기존파일최종SHA에서허용카탈로그/보고2문서외기존변경0·기존비보고git상태byte-exact를Final.json에확인합니다. 승인OGA09/NPC12/Main04/07/#110결과·기존미커밋/미추적·Save/UserData/Discord/Issue85패키지·GUID HOLD보호. 초상화정의/기존공식초상화변경0.

대표확인: **Arbel북쪽 레온(0,3)**,E/패드A로기존대화. Main13마지막목표/Main14시작·보고에서큰“현재목표레온”판은오른쪽에분리되어E와얼굴을가리지않습니다. 시작마을대표와두파티안내인은기존승인그림그대로입니다. 대표실제Play수용확인은남아있습니다.

본상세보고·CODEX_LATEST·CURRENT_STATUS별도Docscommit. 최종DocsSHA/보호대조는Issue109/110댓글·`Temp/Issue109LeonApply20261117/Final.json`. 문서는미Push이며Issue댓글만실제작성·PNG/ZIP/개인자료업로드0.
