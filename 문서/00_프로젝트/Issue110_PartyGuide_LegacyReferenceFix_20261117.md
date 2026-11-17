# Issue110 — 카타콤·아르벨 파티 안내인 승인 외형 연결 및 외부 NPC 참조 감사

2026-11-17 / **TWO_MAPPING_FIX / DELTA85PASS_0FAIL / LOCAL_ORIGINAL_WORKING_TREE_APPLIED / ORIGINAL_COMPILE_ERRORS0 / OTHER_LEGACY_REPORTED_NOT_CHANGED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

바울 대표의 실제 플레이 피드백·이번 지시와 [Issue110](https://github.com/HappyPaul812/Project-Limitless/issues/110)을 확인했습니다. 원본현재카탈로그와변경전실제격리런타임모두두파티안내인에구형외부OGA10이사용됨을확인한뒤 **이미승인된동일역할OGA10 재사용 매핑2줄만** 변경했습니다. 그외NPC의외형은조사·보고만했습니다.

## 실제 변경 — 코드1파일·매핑2줄

`Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs`의아래2항목:

| StableID | 전 | 후 | 실제위치 |
|---|---|---|---|
| safezone-catacomb-party | VillageNpcSprites/Eldiran/OGA10_PartyManager | VillageNpcSprites/Eldiran/OGA10_PartyManager_HandFix_v2_Approved | Field_03 (2.85,-2.55) |
| arbel-party-guide | VillageNpcSprites/Eldiran/OGA10_PartyManager | VillageNpcSprites/Eldiran/OGA10_PartyManager_HandFix_v2_Approved | Arbel (-3,-2) |

승인PNG SHA256 **`6be1a2d777a312dedd392b8ad6fbb2ba77b350d46d7bd2113d9d11bf09c7e6f3`**, GUID **`012b7ee363574fe3900bbd56a472b984`**, PPU360.5/Pivot(.4990234375,.08203125)=(255.5,42)px 유지. 이미있던Sprite를재사용했으며 **PNG/meta/GUID/PPU/Pivot자체변경·재생성·신규파일0**입니다. 실제visible alpha8 높이412/360.5=1.142857WU로구형32/28과동일·하단접지0. NPC Transform/Collider/역할/StableID/이름/대화·Party관리진입로직유지.

공용이름표Y1.37/글자크기·색/그림자/일반주민숨김정책불변. E안내1.87·기존퀘스트표식활성시2.48로피하는#106규칙도불변. 시작마을파티관리인은이미승인경로라재수정하지않았습니다. 최근NPC12/Main04/Main07/레온#109후보매핑변경0. 원래카탈로그로역치환하면시작시점파일과정확히같음을확인해미커밋매핑을덮어쓰지않았습니다.

## 변경 전·후 실제 런타임 감사

Unity6000.6.5f1 기존격리 `Temp/Art95_91Unity20261111/QA/Client` 재사용. BASE는Main05완료/기존SideQuest완료의합성상태, POST_MAIN21은실제 `main_21_returning_warmth` 완료기록추가상태입니다. 새대형QA/전체기능회귀는하지않았으며원본Scene을전환하지않았습니다.

| 상태 | 시작마을 | 카타콤 Field03 | 아르벨 |
|---|---|---|---|
| BASE | active NpcController12 |4|10|
| Main21완료 |14|6|10|

전·후각56개고유관찰기록(tag/Scene/stableID)·실제SpriteRenderer Asset경로/GUID/PPU/Pivot/역할/위치/표시명비교. **변경된ID는두파티안내인뿐**이며다른NPC동일. After원시TSV에는초기검증재시도기록이있어같은키의마지막값으로중복제거했습니다. 근거 `Before/NpcUsage.tsv`, `After/NpcUsage.tsv`, `usage-audit.json`.

### 수정 후에도 실제 사용하는 외부 legacy

| 지역/조건 | ID·역할 | 실제Sprite | 이번조치 |
|---|---|---|---|
| Arbel BASE/이후 | arbel-pet-adoption · 펫분양 | OGA17_EquipmentShop.png | 보고만·미변경 |
| Arbel BASE/이후 | arbel-resident-west · 서쪽주민 | OGA11_Resident02.png | 보고만·미변경 |
| 카타콤 Main21완료후추가 | safezone_catacomb_entrance-pet-adoption | OGA17_EquipmentShop.png | fallback실사용확인·미변경 |
| 시작마을 Main21완료후추가 | safezone_starter_village-pet-adoption | OGA17_EquipmentShop.png | fallback실사용확인·미변경 |

`SafeZonePetServiceRollout`이Main21완료후해당역할이없는거점에생성하고,카탈로그에생성stableID가없어PetAdoption역할fallback으로arbel-pet-adoption의구형OGA17을사용합니다. PetManagement fallback은승인OGA16. 아르벨은기존펫NPC가있어추가중복생성되지않습니다. 단순히파일이보관되어있는것과다르게위항목은실제활성NPC에서확인했습니다.

카타콤BASE의나머지3명은보급상인OGA03/치유사OGA06/감시인DistinctOGA20 승인원화입니다. 아르벨의잡화상인OGA03·치유사OGA06·펫관리OGA16·레온OGA09공유·Portal관리DistinctOGA20도기승인원화. **레온전용#109원화는미승인후보상태로적용하지않았습니다.** 이중일부는역할별전용원화가아닌기존승인원화의공유사용이므로지역별고유디자인확정과구분합니다.

### 외부PNG와 다른 잔여 표시

- **arbel-serin**:현재실제경로 `Assets/_Project/Art/Characters/Serin/Serin_Sprite_Sheet.png`. Chapter2Main15Flow에서설치하는기존공식세린이며파일명에Approved가없다는이유만으로외부OGAlegacy로분류하지않았습니다. 변경0.
- **Arbel의NO_ROLE:VillageNpc**, 표시명마을주민,위치(1.5,-1.5):PlaceholderVisual=True·RendererEnabled=True·Texture1×1·Asset경로없음. 외부PNG가아닌 **실제남아있는런타임단색사각형플레이스홀더**입니다. 이번대상외이므로변경하지않았습니다. 근거 `After/NonAssetNpcDetails.txt`, PlaceholderVisual.CreateSquareSprite 소스.

### 보관 중인 legacy PNG 구분

현재Eldiran리소스폴더의구형OGA PNG는12개이고,수정후이번3Scene/2진행상태NPC의관찰범위에서는 **2종실사용·10종미표시보관**입니다. 구형OGA10도이제관찰범위의파티안내인에쓰이지않습니다.

| 파일 | 판정 | 관찰한사용자 |
|---|---|---|
|OGA02_TrainingGuide.png|관찰 범위 NPC 미사용/보관||
|OGA03_GeneralShop.png|관찰 범위 NPC 미사용/보관||
|OGA06_Healer.png|관찰 범위 NPC 미사용/보관||
|OGA07_Resident01.png|관찰 범위 NPC 미사용/보관||
|OGA09_VillageRepresentative.png|관찰 범위 NPC 미사용/보관||
|OGA10_PartyManager.png|관찰 범위 NPC 미사용/보관||
|OGA11_Resident02.png|실제 사용|arbel-resident-west|
|OGA13_Resident03.png|관찰 범위 NPC 미사용/보관||
|OGA16_Bank.png|관찰 범위 NPC 미사용/보관||
|OGA17_EquipmentShop.png|실제 사용|arbel-pet-adoption, safezone_catacomb_entrance-pet-adoption, safezone_starter_village-pet-adoption|
|OGA19_Resident04.png|관찰 범위 NPC 미사용/보관||
|OGA20_GateGuard.png|관찰 범위 NPC 미사용/보관||

**미표시/보관은이번3Scene·2합성상태의NPC기준**입니다. 다른Scene·모든퀘스트변형·Editor도구·동적Resources로드까지전수미사용을증명한것이아니며삭제승인도아닙니다. 구형PNG와ThirdParty원본을모두보존했습니다. 이후잔여2종/플레이스홀더처리는별도지시범위이며이번에일괄승인원화로바꾸지않았습니다.

## 최소 Delta QA — 85 PASS/0 FAIL

- 실제Field03/Arbel안내인Resources·승인GUID/PPU/Pivot·visible높이/발접지·카탈로그선택PASS.960×720/1280×720/1680×720에서전후6쌍렌더·이름표/그림자머리간격·표시/뷰포트PASS.
- 기존NPCJSON(대화/ID/역할)/Collider/Transform/이름표값/표시정책/정렬보존·다른NPC실제리소스동일PASS.
- 양쪽안내인 **실제E·패드A로파티UI열기**,실제UI버튼으로동료선택초안취소→원래편성동일,태온전열/후열변경→확정(기존공격역할경고있으면재확정),합성Save기록·Escape/B취소/닫기PASS. 업무/대화경로자체는수정하지않았고NpcController기존대사문자열보존을확인했습니다.
- Main21완료후재진입해두안내인승인Sprite유지·동적펫fallback감사PASS. 시작마을기승인파티관리인보존PASS.
- 던전안내인남쪽1.1WU는감시인에더가까워기존입력대상이감시인으로선택됩니다. 파티안내인 **왼쪽1.1WU**의분명한접근위치에서입력검증했습니다. 가장가까운NPC선택정책이나NPC위치를임의수정하지않았습니다.
- 합성Save/설정은Temp QA하위경로. 새런타임오류0. 기존격리UnityEditor.Search초기화예외는EnvironmentErrors로분리기록,전체격리EditorConsole0주장은아닙니다.

## 실제 원본 적용·보호·보고

**85PASS후원본working tree에카탈로그2매핑만적용**. 원본MCP해당스크립트만Import·실제두맵/Resources/GUID/PPU360.5/Pivot255.5,42/이름표1.37·compileFalse 확인. **Console오류0·경고42**. Unity6000.6.5f1/Bootstrap/PlayFalse/dirtyFalse/선택null/포커스null유지. 원본Scene/Play/UI를바꾸지않았습니다.

제품commit **`6c81a03d229f0075b39dcbfa07656182e7f6bae5`**, 브랜치 `codex/issue110-party-appearance-20261117`. rootmainHEAD52f84e643d5d860665eadfe1614a30c745209f00유지·main직접commit/Push/Merge0. 제품commit은작업브랜치에만있고원본main에는검증카탈로그working tree적용입니다.

보호8354파일최종SHA대조는Final.json에서허용카탈로그+보고2문서외기존변경0·기존비보고git상태byte-exact를확인합니다. 승인PNG/meta/GUID·Main04/07·NPC12·Save/UserData/Discord/Issue85패키지·GUID HOLD/미커밋미추적보호. reset/clean/stash·원본이미지삭제0.

대표확인: **Field_03던전앞파티안내인(2.85,-2.55)**, **Arbel파티안내인(-3,-2)**. 안내인옆에서E/패드A로편성UI. 대표실제Play수용확인은남아있습니다. 잔여외부2종/단색플레이스홀더·다른지역조건감사는위별도범위입니다.

이상세보고·CODEX_LATEST·CURRENT_STATUS별도Docscommit,최종SHA는Issue110댓글·`Temp/Issue110Party20261117/Final.json`. 문서는미Push이며GitHub댓글만실제작성합니다. 이미지/ZIP/개인자료원격업로드0.
