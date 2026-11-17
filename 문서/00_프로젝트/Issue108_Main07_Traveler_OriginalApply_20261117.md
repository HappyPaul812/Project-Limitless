# Issue108 Main07 승인 부상 여행자 로컬 원본 적용 — 2026-11-17

**USER_ART_APPROVED / FILE_TECH_PASS_REUSED / DELTA134PASS_0FAIL / LOCAL_ORIGINAL_WORKING_TREE_APPLIED / ORIGINAL_COMPILE_ERRORS0 / MIEL_SCALE_REFERENCE_ONLY / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

바울 대표의 최종 승인·조건부 적용 지시와 [기획 승인](https://github.com/HappyPaul812/Project-Limitless/issues/108#issuecomment-6938654608), [기존 오프라인 실제 검수](https://github.com/HappyPaul812/Project-Limitless/issues/108#issuecomment-6938133891)를 확인했습니다. Main07의 기존 구조를 먼저 조사하고 임시 Sprite와 전용 화면 안내만 최소 수정했습니다. Main04/시작마을NPC12/공용 이름표·navigation은 변경하지 않았습니다.

## 납품·Asset

- `F:/Downloads/Limitless_Main07_Traveler.zip` 실제815952bytes/9파일·SHA256 `b943fb9198f02da7a6df90853457e1bac2b74d916c356b2faab6bac8089db6bd`·CRC재확인PASS. 이전 Manifest7/7·SHA256SUMS8/8·128/40 직접 축소·비교판 검수 PASS를 재사용했습니다.
- `Main07_WoundedTraveler.png` 512×512투명RGBA·SHA256 `90c727b95bee6b2586333d44ebb05b8077664a7e5877084cbbe4a1010676b432`. 새 PNG의 픽셀 수정/재생성0, 기존 OGA07도 보존했습니다.
- Resources `VillageNpcSprites/Eldiran/Main07_WoundedTraveler_Approved` / **GUID `2cf0a7d9462740c58756aca0f18f3391` / PPU430 / Pivot(.498046875,.08984375)=(255,46)px**.
- alpha>8 bbox(96,122,414,466)의 중심x255와바닥y466을 기준으로 Pivot를 산정했습니다. 가시높이344/430=**0.8WU**, 하단경계 접지0. 기존 여행자월드위치(4.4,.2)/scale1/sorting8/white tint 유지. Collider/Scene Asset·StableID·8목표/대사/보상·전투 로직 변경0.

## 미엘·폴 비교 범위와 화면 보호

**현재 Main07에는 미엘 필드 Sprite가 없습니다.** Main07Miel 상호작용 지점·대사로 돌봄/재회를 표현하고 실제 생성되는 필드 인물 Sprite는 여행자·폴뿐입니다. 실제 코드와 다르게 미엘 실물 배치 검증 완료라고 주장하거나 미술 승인만으로 미엘 GameObject를 새로 추가하지 않았습니다.

- 폴은 기존 Paul_Idle_00/PPU100/scale.72 유지. 실제 Sprite 불투명 높이145/100×.72=**1.044WU**, 여행자0.8WU는 약76.6%인 앉은높이입니다. 폴의 기존 Atlas 셀 내용/경계는 재슬라이스하지 않았습니다.
- 미엘은 공식 Miel_Idle_00를 **비교판에서만 폴과 같은 .72축척**으로 놓은 참고 비교(1.0296WU, 여행자약77.7%)입니다. 이것은 기존 Main07 미엘 실제 Transform/체격 검증이나 새 배치 확정이 아닙니다. `Temp/Issue108Apply20261117/RelativeScale_REFERENCE_NOT_GAME.png`는 Pivot 기준 같은 접지선·같은world축척의 QA 참고판이며 게임 화면이 아닙니다.
- Main07의 기존 각 지점 QuestMarker Y1.5/320×70·글자/색/`[E/F / A]` 안내·현재목표 때만 표시하는 조건 유지. 새 앉은 Sprite 상단0.8WU보다 Marker하단1.15WU가 높아 얼굴 간격이 생깁니다.
- 큰 파란 목표판은 공용 안내가 같은 위치의 E/F 안내를 덮는 구조라 **MainQuest07Coordinator LateUpdate에만** 화면판 분리를 추가했습니다. 공용 QuestNavigationPresenter가 계산한 뒤, 화면 안 목표일 때만 판의width270+여백16=286px 왼쪽으로 옮기고 뷰포트 및상단114px영역 아래로 제한합니다. 실제목표Transform/StableID/거리/화면밖방향은 유지합니다. Main04 및 공용navigation코드변경0. Main07완료/비활성일때는 적용하지 않습니다.

## 최소 Delta QA — 134 PASS /0 FAIL

Unity6000.6.5f1 기존 `Temp/Art95_91Unity20261111/QA/Client`를 숨김 Batch로 재사용했습니다. 신규대형복제/전체QA반복/OS포커스전환0. 합성 Save/설정은 `Temp/Issue108Apply20261117/Evidence/` 하위로 분리하고 일반 몬스터 무작위 조우 억제는 QA에서만 적용했습니다(제품 일반조우코드 변경0).

- 기존8목표 시작·바퀴자국 실제E·부상자 실제F·폴흔적Trigger·폴첫대화·지정StoryBattleID/Scene진입 PASS.
- 도망복귀후 전투목표미진행/여행자보존·패드A재도전·승리결과후복귀·미엘복귀Trigger·폴기존복귀위치(5.2,.2) PASS.
- **승리는QA에서기존 EndBattle(true) 결과처리기를 호출**했습니다. 실제전투진입과결과복귀경로를확인한것이며 전체3대3 플레이승리를수행한것은아닙니다. 본외형작업에서전투밸런스/전체QA를반복하지않았습니다.
- 미엘·폴재회실제E·작별실제F·완료후여행자/폴숨김 PASS. 합성 SaveCurrentSession→TryLoadSlot→RestoreSession→Field02재진입으로재회진행상태와완료/숨김보존 PASS. 완료뒤다음퀘스트가자동시작할수있어완료검증은새목표인덱스가아닌Main07완료기록을확인했습니다.
- **HUD ON 960×720 /1280×720 /1680×720**에서 Wounded/Reunion/SavedReunion/Farewell4단계, 총12실제GameView렌더. 기존과같은통상1.1WU거리상호작용으로 여행자뷰포트/상단HUD회피·표시조건/EFA문구·Marker얼굴상단분리·큰판과여행자/폴의원본alpha>.5불투명픽셀겹침0 확인. 실제게임에없는미엘Sprite를QA화면에추가해PASS를만들지않았습니다.
- 새로운런타임오류0. 격리환경의기존 UnityEditor.Search 초기화예외는 EnvironmentErrors.txt로분리기록, 전체격리Editor Console0이라고주장하지않습니다. 원본종료상태와구분합니다.

증거: `Evidence/checks.txt`134PASS·Errors.txt빈파일·status.txtPASS·12PNG, `Unity-FinalDelta.log`·`RelativeScale_REFERENCE_NOT_GAME.png`. 초기검증용짧은접근거리.65WU에서는공용플레이어이름표가여행자쪽으로겹치는화면이있었습니다. 기존Collider/플레이어이름표정책을바꾸지않았으며최종화면검증은통상1.1WU거리범위입니다. 인물을겹쳐세우는모든좌표의화면안전을입증했다는의미는아닙니다.

## 실제 원본 적용·파일

**QA PASS 후 아래3파일만 원본 working tree에 적용**했습니다.

- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/Main07_WoundedTraveler_Approved.png`
- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/Main07_WoundedTraveler_Approved.png.meta`
- `Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs`

원본 MCP 표적 Import 완료, 실제 Resources/GUID/PPU430/Pivot255,46/Wounded(4.4,.2) 확인. **compileFalse / Console오류0·경고104**, Unity6000.6.5f1/Bootstrap/PlayFalse/dirtyFalse/선택null/포커스null 유지. 원본에서 Play Mode를 임의로 열거나 Scene을 변경하지 않았습니다.

제품로컬commit **`f1b0fc9258003d3c12e9c1f1bd830dda84f51b21`**, 브랜치 `codex/main07-traveler-approved-20261117`. 원본main HEAD `52f84e643d5d860665eadfe1614a30c745209f00` 그대로이며main에제품commit을합류시킨것이아니라검증3파일의working tree반영입니다. main직접commit/Push/Merge0·reset/clean/stash0.

보호목록8352의기존파일SHA와비보고git상태는최종 `Temp/Issue108Apply20261117/Final.json`에서검증합니다. 허용Main07Flow+보고문서외기존변경0, **Main04Flow/PNG/meta/공용navigation/이름표·NPC12/Save/UserData/Discord/Issue85패키지·GUID HOLD** 유지. 입력ZIP/승인PNG동일. 새Main07PNG/meta외기존Asset수정0.

## 대표 확인·다음

Field_02 Main07에서바퀴자국조사후 **부상여행자(4.4,.2)**. 처음0번목표에는기존정책대로숨겨지며1번이후표시됩니다. 폴첫위치(6.5,-2.35),전투후(5.2,.2). 여행자F→폴E/전투→미엘E→폴작별F/A·완료후숨김을새Play에서확인하실수있습니다. 미엘실필드Sprite추가는이번범위가아니며필요시별도기획/적용작업입니다.

본상세보고·CODEX_LATEST·CURRENT_STATUS별도Docscommit, 최종SHA는Issue108댓글·Final.json. 문서미Push/LOCAL_COMMITTED·REMOTE_NOT_PUSHED이며GitHub댓글만실제작성합니다. ZIP/PNG/개인자료원격업로드0. 대표실제Play수용확인은남아있습니다.
