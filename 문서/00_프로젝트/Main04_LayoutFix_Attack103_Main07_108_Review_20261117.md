# Main04 화면 가림 수정·조건부 적용 및 Issue103 Attack4·Issue108 Main07 오프라인 검수

2026-11-17 / **A LOCAL_ORIGINAL_APPLIED / DELTA_PASS / B FILE_TECH_PASS_ART_PENDING_NOT_APPLIED / C FILE_TECH_PASS_USER_ART_APPROVAL_PENDING_NOT_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

바울 대표의 이번 세 갈래 지시, [Issue107 승인](https://github.com/HappyPaul812/Project-Limitless/issues/107#issuecomment-6936953484)·[이전 실제 HOLD](https://github.com/HappyPaul812/Project-Limitless/issues/107#issuecomment-6937365506), [Issue103 KeyPose/Idle4 승인](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6936952632)·[Attack4 납품](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6937699347), [Issue108 납품](https://github.com/HappyPaul812/Project-Limitless/issues/108#issuecomment-6937162840)을 확인했습니다. B/C 게임 적용 승인으로 확대하지 않았습니다.

## A. Main04 화면 가림 원인 및 수정

이전 HOLD 원인을 분리했습니다. ①Scene 자체가 위쪽이어서 상단 F10/목표갱신 토스트에 인물이 접근 ②여행자 StoryLabel이 미엘 옆머리에 겹침 ③공용 E와 퀘스트 표식의 세로 간격 부족 ④큰 파란 QuestNavigation HUD가 목표 위치를 중심으로 그려져 인물·안내를 덮음. sorting 값을 높이는 방식으로 HUD 위에 얼굴만 덧그리지 않았습니다.

| 항목 | 전 | 후 |
|---|---|---|
| Main04 MeetingPosition | (7.2,5.5) | (7.2,3.4) |
| 미엘 / 태온 위치 | (7.55,5.6) / (5.95,5.6) | (7.55,3.5) / (5.95,3.5) |
| 여행자 위치 | (8.15,5.15) | (8.85,3.05) |
| 여행자 StoryLabel localY |1.15|1.65|
| Main04 미엘 E 안내 localY/상자높이 |1.48/54|1.66/40|
| Main04 미엘 퀘스트 표식 localY/상자높이 |1.92/50|2.08/36|
| 일반 NPC 이름표/E 정책 |#106 공용 값|변경0, 이름표1.37 유지|

Main04MielActor LateUpdate만 전용 안내 위치를 적용합니다. 글자 크기·색·문구·상호작용/표식 상태 조건 유지, 상자 높이만 글자가 들어가는 범위로 줄였습니다. 해당 퀘스트/Field_01 인물 활성 때만 동작하며 원본 Scene Asset·공용 CameraFollow는 변경하지 않았습니다. 미엘3WU 근처에서는 상단120px 아래로 안내가 들어오도록 필요한 카메라 Y만 올리고 기존 WorldBounds 내부로 제한합니다. 남쪽1.1WU 접근 기준960/1280 약+.0267WU,1680 약+.36WU의 보정이며 타 Scene/멀리 떨어진 플레이/완료후는 적용하지 않습니다. Scene 만남 Trigger와 실제 표시 그룹을 함께 내려 stable ID·전투 목표/퀘스트 순서는 유지했습니다.

QuestNavigationPresenter에는 **Main04 추적 때만** 기존 Main09의 빈 공간 선택을 재사용하도록 조건을 추가했습니다. 실제 대상 Transform/StableID/거리/화면 밖 방향 의미는 그대로이며 화면상 파란 판만 이동합니다. 후보 선택은 Main04에서 상단114px을 제외하고, 인물 왼쪽 추가 여백260px 후보를 사용합니다. 기존 Main09의72px/상단0 설정은 그대로입니다. 여행자도 충돌 회피용 Sprite 대상으로 포함했습니다.

### 제품4파일·승인 PNG

- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/Main04_WoundedTraveler_Approved.png`
- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/Main04_WoundedTraveler_Approved.png.meta`
- `Unity/Client/Assets/_Project/Scripts/World/MainQuest04FieldFlow.cs`
- `Unity/Client/Assets/_Project/Scripts/Player/QuestNavigationPresenter.cs`

새 PNG SHA256 `2687c1c62e5bd20e6e0fb16c3428225aa28fc156c80a5c08b6af2350d9e9715d` / GUID `f325180de4944364a0dd74c1a814d6df` / PPU360 / Pivot(.5,.08203125)=(256,42)px. 512RGBA 원화 픽셀 불변, alpha bbox(106,122,406,470), 가시높이0.966667WU·미엘 실불투명 인물높이1.43WU 대비 앉은 높이67.6%·하단접지0. tint(.82,.82,.82,1)/sorting7 유지. OGA07 원본은 보존하고 전용 Resources만 사용합니다.

### Delta QA와 실제 적용

- Unity6000.6.5f1 기존 격리 `Temp/Art95_91Unity20261111/QA/Client` 재사용. 새 대형 복제/전체 QA 반복0·숨김 Batch·포커스 전환0. Save/설정은 Temp 합성경로. 일반 몬스터 무작위 접촉은 QA에서만 SuppressForSeconds로 분리하여 지정 Story 전투를 확인했습니다(제품의 일반조우 억제 변경0).
- 기능 **65 PASS/0 FAIL**: 실제 E 미엘 대화/만남Trigger/Story ID·Battle Scene 진입/도망후 미진행·여행자 재생성/재도전/승리결과 후 복귀/전투후 대화/완료후 숨김·재진입 숨김. 승리는 기존 EndBattle(true) 결과처리기를 QA에서 호출했으며 전체3대3 실전 플레이 승리가 아닙니다. 이 검사 후 StoryLabelY·큰 목표판 후보 간격만 추가 보완했고 퀘스트·전투 로직은 변경하지 않았습니다.
- 최종 화면/추가 표적 **59 PASS/0 FAIL**:960×720/1280×720/1680×720·각4방향1.1WU 접근, Camera Bounds 보존/가이드 상단120px 여백, 이름표 뷰포트 내 표시, 실제 미엘 RGBA alpha>.5 픽셀과 StoryLabel 글자 bounds 겹침0, 큰 목표판과 미엘/태온/여행자 실불투명 픽셀 겹침0. **65+59=124 PASS**는 범위가 다른 두 검사 합계이며 단일 전체 회귀 결과가 아닙니다.
- 최종 정상 HUD ON 렌더3장과4방향12장 직접 확인. HUD숨김을 합격 증거로 사용하지 않았습니다. 원본 PNG/공용NPC/이웃 인물 외형 유지. 초기시안에서 발견한1680 안내가림·파란판의태온가림은 최종 후보 전 개선했으며 최종 산출물에 남겨두지 않았습니다.
- 기능/최종 화면 런타임 오류0. 기존 QA UnityEditor.Search 초기화 예외는 EnvironmentErrors에 별도 기록; 전체 격리 Editor Console0 주장은 하지 않습니다.
- **PASS 후 원본 working tree에 위4파일만 적용**. 원본 MCP 표적 Import 완료, 실제 Resources/GUID/PPU360/Pivot256,42·Meeting(7.2,3.4)·compileFalse·Console errors0/warnings104. Bootstrap/PlayFalse/dirtyFalse/선택null/포커스null 유지. 새 스크립트 컴파일 도메인재로드 중 MCP 일시503이 있었고 재연결 후 실제 값을 확인했습니다.
- 제품 로컬 commit **`3f66fdb25d9445738f8b484d065d92d84e462442`**. 원본 main HEAD는`52f84e643d5d860665eadfe1614a30c745209f00` 그대로·원본 main commit/Push/Merge0. 별도 작업브랜치의 검증 파일을 원본 working tree에 복사했으므로 원본 main에 제품commit이 합류했다고 주장하지 않습니다.

확인 위치: **Field_01, Main04 진행 중 미엘(7.55,3.5)·여행자(8.85,3.05)**. 이전5.5높이가 아닌 새 만남영역(7.2,3.4). 대표 실제 Play 수용 확인은 남아 있습니다. 증거 `Temp/Main04Fix_Art10310820261117/Evidence/`, `LayoutEvidence/`, `Main04_BeforeAfter.png`. 최종 원본 상태 `original-after.json`.

## B. Issue103 IceBeetle Attack4 — 오프라인만

**FILE_TECH_PASS / ART_REVIEW_PENDING / USER_ATTACK4_ART_APPROVAL_REQUIRED / NOT_APPLIED**.

- `F:/Downloads/IceBeetle_Issue103_Attack4_20261117.zip` / 2669553bytes /12파일. 실제 ZIP SHA256 **`ebb4172037c7b7e74115be0aaf38e70dc9c58b6e928549934415e290d682f997`**.
- CRC PASS·Manifest10/10 크기/SHA 및 payload전체커버·SUMS11/11 payload+최종Manifest·안전경로/중복/메타순환0. Manifest가 다른Main07과구조가달라 실제files/payload키로 각각검증했습니다.
- 시트2048×512 RGBA·가로4×512셀·PNG SHA256 `d8dab5e411f7eeb0aa73b1e412b8d820ecfeb38d514c60cf5fef7eb29af4b236`. 분리 QA4장 모두셀RGBA일치·4프레임상이·셀경계alpha0.
- 구성: 시트1+분리4+128/40스트립2+512/128/40비교판1=PNG8, GIF1, 문서/Manifest/SUMS3. 축소본 직접 LANCZOS 보이는RGBA/알파 일치(숨은채널차이도0), 비교판12배치불투명전체원본일치. GIF4프레임·무한반복·**180/110/130/180ms=600ms/주기**, 원시트배경합성+MEDIANCUT256색4/4RGB정확일치.
- 승인KeyPose b7ff0592…/Idle4 bd06c6d9… 실물SHA 재확인·보존. **첫프레임은 준비 자세로 KeyPose 전픽셀 동일이 아니고, 4번 회복프레임이 승인KeyPose와 전RGBA동일**입니다. 처음부터 동일프레임이라고 오보고하지 않습니다. 첫프레임실루엣/청록갑각/금빛균열/작은다리/우향은 승인외형계열 유지.
- y430..511 발접지RG​BA4/4 승인KeyPose 동일. bbox8은1번(18,133,481,464)·2번(35,127,493,464)·3번(42,132,505,464)·4번(29,125,484,464). 정점3번 우측7px여백·클리핑없음.
- 미술 의견: 뒤로 준비→짧은 전방 갑각 밀치기→정점→승인자세 회복으로 읽힘. 과장점프·입위주공격·큰갑각파괴변형 발견0, 특정이상프레임없음.40px에서는다리/세부움직임약하므로512/128/GIF도함께검토권고. 묵직한감각의최종사용자승인은필요합니다.
- Unity Import/Assets복사/GUID생성/게임적용0. 새Hit/Defeat/스킬/수치/Spawn개발0.

## C. Issue108 Main07 숲길 부상자 — 오프라인만

**FILE_TECH_PASS / USER_ART_APPROVAL_PENDING / NOT_APPLIED**.

- `F:/Downloads/Limitless_Main07_Traveler.zip` /815952bytes/9파일. 실제ZIP SHA256 **`b943fb9198f02da7a6df90853457e1bac2b74d916c356b2faab6bac8089db6bd`**.
- CRC PASS·Manifest7/7 크기/SHA/payload전체커버·SUMS8/8 최종Manifest포함·경로안전/중복/메타순환0.
- `Main07_WoundedTraveler.png` 실제512×512투명RGBA·SHA256 `90c727b95bee6b2586333d44ebb05b8077664a7e5877084cbbe4a1010676b432`, alpha8bbox(96,122,414,466). 구성PNG4(원본1/128·40각1/비교판1)+QA/README/출처/Manifest/SUMS5.128/40은직접LANCZOS축소RGBA전픽셀동일·비교판Main04/승인OGA07/OGA11/새Main07의12배치원본불투명전체일치.
- 미술 의견: 회색앞머리·좁은눈매·자주색외투·한손으로바닥을지지하는앉은자세·아랫다리붕대로 Main04의갈색머리/녹색외투/팔붕대/장비와구별됨. 서있는생활복주민들과자세/여행복/피로한표정차이확인. 미엘의치료중부상·휴식으로자연스럽게읽히며과한피/고어없음.40px에서는붕대/표정판독약하므로512/128병행검토권고.
- **Main04와동일인/가족/특정성별·직업·부상부위캐논으로확정하지 않음**. 사용자최종미술승인필요. 실제미엘과의Unity체격/PPU/Pivot/접지는이번검수범위아님. Unity Import/Assets복사/GUID수정/게임적용0, 기존Main07게임코드/임시원본변경0.

## 공통 보호·기록

Unity6000.6.5f1·Issue85패키지/GUID HOLD 유지. 원본PNG수정0·시작마을승인NPC12/Main07/Save/UserData/Discord·기존미커밋/미추적보호. 보호8350목록 SHA 및 비보고git상태최종대조에서 허용Main04코드2+보고2 외 기존변경0을 Final.json에 확인합니다. B/C실물은Temp에만 추출·외부업로드0. 원본창/Scene/Play/포커스조작0·새프로젝트복제/전체QA반복0·reset/clean/stash0.

브랜치 **`codex/main04-layout-art103-108-20261117`**, 제품commit `3f66fdb25d9445738f8b484d065d92d84e462442`. 이 상세보고·CODEX_LATEST·CURRENT_STATUS만 별도Docscommit. 최종DocsSHA는Issue103/107/108댓글·`Temp/Main04Fix_Art10310820261117/Final.json`에기록. Git문서는미Push이며GitHub댓글만실제로작성합니다. 보고서의로컬문서경로를유효원격문서링크로가장하지않습니다.
