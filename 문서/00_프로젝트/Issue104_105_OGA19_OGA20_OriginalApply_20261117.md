# Issue #104·#105 — OGA19 주민4·독립 OGA20 경비 원화 검증 및 조건부 원본 적용 (2026-11-17)

**USER_NEW_DESIGNS_ACCEPTED / FILE_TECH_PASS / TAEON_DIRECT_COMPARE_DISTINCT / DELTA_118_PASS_0_FAIL / BOTH_LOCAL_WORKING_TREE_APPLIED / ORIGINAL_IMPORT_COMPILE_CONSOLE0 / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

## 승인·결론

[#104 시각 승인](https://github.com/HappyPaul812/Project-Limitless/issues/104#issuecomment-6934714159), [#105 조건부 적용·태온 비교 지시](https://github.com/HappyPaul812/Project-Limitless/issues/105#issuecomment-6934715031), [#106 적용 결과](https://github.com/HappyPaul812/Project-Limitless/issues/106#issuecomment-6934444417), 현재 로컬 코드·프로젝트 최신 인계를 확인했습니다. 바울 대표의 이번 **검증·안전할 때만 적용** 지시를 근거로, 파일 검증→공식 태온 직접 비교→기존 격리 Delta PASS를 모두 확인한 후 원본에 반영했습니다. 아트 담당의 이전 `TAEON_VISUAL_NOT_VERIFIED`를 근거 없이 해제하지 않고 실물로 보완했습니다.

**OGA19·새 OGA20 모두 적용 성공**. 과거 태온 닮은 OGA20 시안은 HOLD 이력·원본 그대로 보존하며 이번 독립 신규 원화로만 외형을 교체했습니다. 가족/연령/직업·스토리의 새 캐논을 확정하지 않습니다.

## ZIP·PNG 실물 무결성

`F:/Downloads/Limitless_NPC_104_105.zip` **2166760bytes/14entries**, 실측 SHA256 **`807f7d6bb3c29f8846c08d5c7e985244b009ebc6b1bea4a1181c641ed3d9351e`**(최신 납품일치). CRC PASS, Manifest12/12 크기·SHA, SHA256SUMS13/13(payload12+확정Manifest) PASS, 자기·순환참조/위험경로/중복entry0. PNG2 모두512×512RGBA·외곽alpha0. 128/40 납품프리뷰4개는 실제PNG의LANCZOS축소와RGBA전부 일치. 기존 승인10종의 실물 SHA10/10도 납품참조와 일치했습니다.

| 대상 | 새 PNG SHA256 | 새 GUID | PPU | 정규화 Pivot |
|---|---|---|---:|---|
|OGA19|`d0ec6f5712ea9d7d6cada6335742986613b2d4149b1f4b75c662b0a3e22a1028`|`ef41bc764f92496087b0c8ec823536bc`|367.5|(0.4990234375, 0.076171875)|
|OGA20|`19efaea3859246a41d9bd3cb1880744f3b0802b9c51ec22f0937a6794e201ef9`|`e63ac7d55ed144bd89897f57ec3e7112`|367.5|(0.4990234375, 0.076171875)|

두 후보의 alpha>8 bbox는 OGA19 [153,53,358,473), OGA20 [134,53,377,473), 높이420px입니다. **PPU367.5→기존 가시높이32/28=1.142857WU**, 발끝 기준 Pivot(255.5,39px)입니다. 기존 PNG·meta·PPU28·Pivot 및 legacy Resources 경로는 그대로 남겼고 신규 승인 PNG에 독립 importer를 부여했습니다. OGA20의 가시상단은 장병기 끝을 포함하므로 사람 머리만으로 정규화한 값이라고 하지 않습니다. 원본 픽셀/비율/Transform/Collider 수정0.

## 태온·기존 주민 직접 비교

공식 초상화는 `DialoguePortraitDefinitions/Taeon.asset`의 GUID연결을 실제 `Art/Portraits/Portrait_Taeon.png.meta`와 대조했습니다. 공식 필드 태온은 `MainQuest03FieldFlow.cs`가 Resources의 `Taeon_Idle_00`을 사용하므로, `Resources/BattleCharacters/Taeon/Taeon_Battle_Final.png.meta`의 실제 rect(0,1045,209,209)로 해당 프레임을 잘라 직접 확인했습니다.

- 공식 태온 초상화 SHA **`3194944077b7b176abb53497ec0991d64864bebd13c16333d93a620ec0192994`**.
- 공식 태온 Sprite 시트 SHA **`da140dcdf7db326d0a0442b5fe9e7aa746d2c80b5e435142e17464c7ecc2a4cf`**.
- 직접 비교판: private `Temp/Issue104105NPC20261117/Taeon_vs_OGA20_ActualSources_OFFLINE.png`. 공식 초상화/메타기준Idle00/실제새OGA20을 나란히 표시했습니다. 원화 크기와 소스 해상도가 서로 달라 캔버스/축소 표시를 구분하며 Unity 캡처는 아닙니다.

**시각 판단:** 새 OGA20은 짧고 위로 선 머리·노출된 이마/귀·넓은 턱·굵은 눈썹·회색 수염과 큰 어깨, 갈색 가슴방어구/장병기로 읽힙니다. 태온은 길게 내려오는 갈색 앞머리·수염 없는 갸름한 얼굴·푸른 망토/별 문양/검·방패 실루엣입니다. 남색·금속 방어구라는 공통 요소는 있으나 얼굴·헤어·연령감·주장비에서 구분돼 **동일 인물/가족으로 오인할 정도의 강한 유사성은 관찰하지 못했습니다**. 픽셀 직접 비교에 따른 미술적 판단이며 수학적 유사도/자동 승인 점수로 포장하지 않습니다. 이 조건을 만족해 다음QA로 진행했습니다.

OGA19는 OGA07/13보다 넓은 얼굴·좁고 올라간 눈매·옆가르마 웨이브·단단한 체형·청회색 외투/천가방으로 구별됩니다. 128에서 세부,40에서 복장/체형과 경비 장병기를 구별할 수 있고 세부눈·수염 판독은 제한적입니다. 같은 캐릭터를 복사한 후보로 보이지 않습니다. 기존10종 사이에도 단순한 면/세밀한 묘사의 차이가 있어 완벽한 화풍 통일을 새로 확정하지 않습니다. 승인된 그림은 수정하지 않았습니다.

## 실제 변경5파일·공유 영향

- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA19_Resident04_Original_Approved.png`
- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA19_Resident04_Original_Approved.png.meta`
- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA20_GateGuard_Distinct_Approved.png`
- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA20_GateGuard_Distinct_Approved.png.meta`
- `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs`

카탈로그 변경은 **4매핑 줄만**입니다:

- `starter-village-resident-04`→`VillageNpcSprites/Eldiran/OGA19_Resident04_Original_Approved`.
- `starter-village-gate-guard`, `safezone-catacomb-keeper`, `arbel-portal-keeper`→`VillageNpcSprites/Eldiran/OGA20_GateGuard_Distinct_Approved`.

StableID/기존Resources원본/역할/대사/Scene/Quest/Collider/Nameplate정책은 그대로입니다. OGA20 공유3지역의 의도된 외형 변화만 반영했고 다른 NPC의매핑과OGA11/13최근적용을 덮어쓰지 않았습니다. **#106 NameplateY1.37 및 NpcInteractionPrompt 코드 전체SHA 불변**. Portal관리인은 기존 Resident역할이므로 상시이름표숨김·E1.48 그대로, 남문/카타콤은 표시이름표Y1.37·E1.87(기존표식활성시2.48) 그대로입니다. Portal 연결 기능은 기존미구현 안내 상태로 유지했습니다.

## 최소 Delta QA와 원본 검증

Unity6000.6.5f1 기존 QA `Temp/Art95_91Unity20261111/QA/Client`/Library 재사용, **118PASS/0FAIL**. 새 대형 프로젝트/전수QA복제0, 원본Scene/Play/포커스 조작0. 합성Save/Settings와 가상 입력만 사용했습니다.

- 시작마을주민4/남문경비, Field03카타콤감시인, ArbelPortal관리인 **4대상×960/1280/1680×720** 전후24실제렌더+대화4장=28PNG. 가시높이·발접지·viewport·카탈로그선택·PPU/Pivot·기존Collider/Transform/대사/StableID/이름표 정책 PASS.
- 4대상 E/Esc·패드A/B의 실제 기존대화 열기닫기, 남문 도보/휠체어 입력, Scene재진입 외형 유지 PASS. 원본대사를 API로 직접연 것이 아니라 가상 입력으로 실제상호작용 경로를 통과했습니다.
- 세지역의 대상외 기존NPC10 및 다른NPC속성/외형 보존 PASS, 주민정책과#106 E위치/이름표 회귀 PASS.
- 신규Runtime오류0. 기존격리 `UnityEditor.Search` 시작예외는 별도환경로그이며 전체격리로그 무오류라고 하지 않습니다. Issue85 패키지/GUID HOLD 유지, 추가변경/우회0.
- PASS후 원본working tree에제품5개만 반영하고 MCP로PNG2/Catalog만표적Import. 실제 Resources4매핑·GUID·PPU367.5·Pivot(255.5,39) 및 NameplateY1.37을 확인했습니다. **compileFalse·Console errors0/warnings42**, ConsoleClear0.
- 시작/최종 원본Bootstrap·PlayOFF·dirtyFalse·선택null·포커스null 유지. 기존Editor 종료/새원본Batch/전체Refresh0. 원본Play는 시작하지 않았으며 대표새실게임확인은USER_REALPLAY_PENDING입니다.

## 보호·Git·대표 확인

원본 기존8346보호파일 중 허용Catalog1개만 제품변경·나머지8345 SHA변경0(완료보고갱신전), 제품5개 제외기존git상태byte-exact동일. 승인NPC10·공식태온2원본SHA변경0. Save/UserData/Discord/기존미커밋·미추적/구형HOLD원화 보호. 이후 보고3문서만 별도갱신합니다.

로컬 브랜치 **`codex/npc104-105-approved-original-20261117`**, 제품5파일 commit **`a0d5674bc83ddae5223cc4c62f23b2889876f2ed`**. 원본main HEAD **`52f84e643d5d860665eadfe1614a30c745209f00` 그대로**, 제품은 원본working tree 적용이며 main에 commit이 합류한 것은 아닙니다. main직접commit/로컬Merge/Push/PRMerge/reset/clean/stash0. 후속Docs최종SHA는Issue104/105실제댓글과private Final.json에 기록합니다. F:시작321436598272→검증후321402310656bytes(시점별 실측).

대표 확인: **시작마을 동쪽 주민4(7.2,0)**, **남문 경비병(2.4,-5.1)**, **Field03 카타콤입구 감시인(2.85,-4.55)**, **Arbel Portal관리인(-7,-3)**. 가까이E/패드A로기존대화를 확인하시면 됩니다. 주민4·Portal관리인은 상시이름표가 없는 것이 정상입니다. 추가적용승인대기는 없으며 사용자 새Play수용만 남습니다.

로컬보고/CODEX_LATEST/CURRENT_STATUS를 기록하고 GitHub에는Issue댓글만 실제작성합니다. 원격 문서/PNG/ZIP 업로드0. private근거: `Temp/Issue104105NPC20261117/intake.json`, product.json, Final.json, 공식태온비교판, Evidence/checks.txt(118)/Errors.txt(0)/EnvironmentErrors.txt/InteractionDiagnostics.txt/28PNG, Unity-QA.log, original-state-before.json/original-state-after.json. 추가새NPC/Quest/Portal구현·다른아트작업 자동착수0.
