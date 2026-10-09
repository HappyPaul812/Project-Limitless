# Chapter2 잡템 7종 검수 및 Unity 구현 — 2026-10-09

TECHNICAL_PASS / IMPLEMENTED_7_OF_7 / USER_ART_REVIEW_PENDING. 사용자 시각 승인은 미완료다.

## ZIP 및 아이콘 검수
원본 `F:\Downloads\Limitless_Chapter2_Loot_Icons_v1.zip`, 72,440bytes, SHA256 `82eeed5bd31f4d3cc812694099ef98a06eb352fd62ff32753d982ff0bbe0123d`.
아이콘7 + Contact Sheet PNG1(1120×742 RGB) + Manifest1. 기대 파일명7/7 정확일치·누락0·이름차이0. 아이콘7 PNG decoding PASS,128×128 RGBA, alpha0~255, 테두리 투명·여백 있음·빈 이미지0·이미지 완전중복0. 송곳니/깃털/비늘/붉은갑각/잔불결정/검은외피/열핵 형태는 파일명과 대응한다. 명백한 잘림0. Contact Sheet32/48px에서도 형태가 구분된다. 실제 UI50px 가독성/기존 아이콘과 미술 일관성은 사용자 승인 대상. Contact Sheet는 게임에 Import하지 않았다.

## 정의 및 매핑
공통: Material, MaxStack99, BuyPrice0, SellPrice 아래표, UseType/EffectType None, EffectAmount0, 성공 시 min/max1. 상점 구매목록 제외로 구매 차단(BuyPrice0 자체는 차단 조건이 아님).

|PNG|MonsterId|ItemId|이름|확률|판매가|
|---|---|---|---|---|---|
|Loot_SootHound_Fang.png|soot_hound|material_soot_hound_fang|그을음 송곳니|50%|6|
|Loot_HeatwindHawk_Feather.png|heatwind_hawk|material_heatwind_hawk_feather|열풍 깃털|50%|6|
|Loot_FissureLizard_Scale.png|fissure_lizard|material_fissure_lizard_scale|균열 비늘|45%|7|
|Loot_EmberBeetle_Carapace.png|ember_beetle|material_ember_beetle_carapace|화열 갑각|45%|8|
|Loot_EmberWraith_EmberShard.png|ember_wraith|material_ember_wraith_ember_shard|잔불 파편|45%|9|
|Loot_ObsidianBeetle_Shell.png|obsidian_beetle|material_obsidian_beetle_shell|흑요석 갑각|45%|10|
|Loot_ScorchingWatcher_CoreFragment.png|scorching_watcher|material_scorching_watcher_core_fragment|감시자의 열핵 조각|60%|15|

### 설명 출처
별도 잡템 Art Handoff 설명 원문은 저장소/제공 첨부에서 발견하지 못했다. ZIP Manifest 설명을 사용했으며 별도 Handoff 문구 일치 여부는 미확인이다. Manifest의 ID/이름/확률/판매가는 사용자 지시와7/7 일치한다.
- 그을음 송곳니: 그을음들개의 날카로운 송곳니. 검은 재가 깊숙이 스며들어 있다.
- 열풍 깃털: 뜨거운 바람을 가르던 열풍매의 깃털. 끝부분에 열기의 흔적이 남아 있다.
- 균열 비늘: 균열도마뱀의 단단한 비늘. 표면에 뜨거운 지맥을 닮은 무늬가 새겨져 있다.
- 화열 갑각: 화열딱정벌레의 붉은 갑각. 열기가 식은 뒤에도 미세한 온기가 남는다.
- 잔불 파편: 사라진 불씨망령이 남긴 결정 조각. 내부의 붉은 빛이 희미하게 흔들린다.
- 흑요석 갑각: 흑요석 갑충의 무거운 외피 조각. 돌처럼 단단하며 깨진 면이 날카롭다.
- 감시자의 열핵 조각: 작열 감시자에게서 떨어져 나온 열핵의 일부. 작지만 상당한 열기가 응축되어 있다.

### 파일 검사
ZIP 상대경로 공통 접두사 `Limitless_Chapter2_Loot_Icons/Icons/`. bbox=(left,top,right,bottom), 우/하 경계 제외.

|PNG|bytes|bbox|투명픽셀|SHA256|
|---|---:|---|---:|---|
|Loot_SootHound_Fang.png|1322|[22, 16, 104, 116]|12696|`5020e7cd572399632a3e19df1318a17477350778272fb0fc15029ada9ef0dc63`|
|Loot_HeatwindHawk_Feather.png|1312|[34, 16, 104, 116]|13200|`9b25288bac730fa2453b0e9f0bb61caae2a84c77372095628e72647a9f8ea332`|
|Loot_FissureLizard_Scale.png|1320|[20, 28, 110, 108]|11412|`629df9280aefeca307436c5b6e1ce55d56acff6e73ab897ff2eded21162ce888`|
|Loot_EmberBeetle_Carapace.png|1340|[22, 22, 108, 102]|11316|`bedbe891d1ed0b7d144533003887879ef8d8cabe82c7e0af19e4fc795519e326`|
|Loot_EmberWraith_EmberShard.png|1239|[28, 20, 106, 116]|11496|`e1e3ee1ac24cb8d78e95c30470ac8a3c841b2be2fab11085671a20ebc81bb617`|
|Loot_ObsidianBeetle_Shell.png|1322|[20, 26, 110, 110]|11008|`613e9914f8b3539bf204204e74a0a07a45c84436b1a1da21aa3f84f155ea40b2`|
|Loot_ScorchingWatcher_CoreFragment.png|1593|[18, 18, 110, 112]|10332|`cb7358d81f711372118281dbd0b565e17654178a34f6f791effed34cceab3b6d`|

## 구현·호환성
Unity6000.5.7f1에서 Sprite Single,PPU100,Pivot(0.5,0.5),Point,Clamp,mipmap off,alpha transparency on,uncompressed,MaxSize128로 Import했다. 원본PNG byte 그대로 복사했고 신규meta/GUID만 생성했다. 기존128px/PPU100 아이콘과 호환된다.
기존 ItemCatalog Resources 로드→InventoryPresenter Icon 표시→BattleVictoryReward→RewardBundle.ApplyBestEffort→InventoryService, ShopService 판매, GameSaveService ItemId/Count 저장을 재사용한다. C# 변경0, SaveVersion 변경0. 몬스터7은 lootEntries만 변경했고 능력치/EXP/Talent/패턴/스폰 보존. Main20 일반잡템 추가0·서브퀘스트0.

### 실제 구현 파일
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/11_SootHound.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/SootHound_Fang.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/SootHound_Fang.asset.meta`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_SootHound_Fang.png`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_SootHound_Fang.png.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/12_HeatwindHawk.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/HeatwindHawk_Feather.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/HeatwindHawk_Feather.asset.meta`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_HeatwindHawk_Feather.png`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_HeatwindHawk_Feather.png.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/13_FissureLizard.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/FissureLizard_Scale.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/FissureLizard_Scale.asset.meta`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_FissureLizard_Scale.png`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_FissureLizard_Scale.png.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/14_EmberBeetle.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/EmberBeetle_Carapace.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/EmberBeetle_Carapace.asset.meta`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_EmberBeetle_Carapace.png`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_EmberBeetle_Carapace.png.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/15_EmberWraith.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/EmberWraith_EmberShard.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/EmberWraith_EmberShard.asset.meta`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_EmberWraith_EmberShard.png`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_EmberWraith_EmberShard.png.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/16_ObsidianBeetle.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/ObsidianBeetle_Shell.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/ObsidianBeetle_Shell.asset.meta`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_ObsidianBeetle_Shell.png`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_ObsidianBeetle_Shell.png.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/17_ScorchingWatcher.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/ScorchingWatcher_CoreFragment.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/ScorchingWatcher_CoreFragment.asset.meta`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_ScorchingWatcher_CoreFragment.png`
- `Unity/Client/Assets/_Project/Resources/ItemIcons/Loot_ScorchingWatcher_CoreFragment.png.meta`

## 자동 검증
Unity MCP execute_code로 Edit 상태에서 실제 서비스 호출. 테스트 전 Inventory/Currency를 보관하고 finally 복원했다. 실제 Save 파일 쓰기0·Play Mode0·Scene전환0·화면포커스조작0.

|검사|결과|
|---|---|
|Catalog 신규7/전체22, ID중복 없음, Icon Sprite 연결|PASS|
|몬스터7 매핑/확률/min=max1, 정의/판매가격|PASS7/7|
|성공난수0→마리당1개, 난수=확률→실패|PASS7/7|
|동일Combatant 중복 입력 보상 방어|PASS7/7|
|중첩98→99, 추가1 차단|PASS7/7|
|2개 판매 수량/탈렌트, 상점 구매차단|PASS7/7|
|GameSaveData JSON 왕복99 유지, Version1 누락Inventory|PASS|
|Console error|0; Editor ready, 컴파일 중 아님|
|기존3315파일 보호/Chapter1 아이템·드롭|몬스터7외 SHA변경0 PASS|
|몬스터7 비Loot 데이터|HEAD 원문에 lootEntries만 삽입 PASS|
|실제 UI/전투종료 중복호출/Save파일 Continue|미검증|
|Main17~19 전체 전투 Runtime 회귀|미검증; 관련코드/Scene 보존만 정적 PASS|
EndBattle의 기존 battleEnded guard는 정적 확인했다. RewardBundle 자체 반복 호출 멱등성까지 검증한 것은 아니다. 전체 EconomyInventoryAudit는 다른 세션 상태 초기화 영향 때문에 실행하지 않았다.

## 보호·Git·후속
시작HEAD `0483167d49e6b0af55d2d683977c70526a6e75a4`. 시작75 tracked 수정/25 untracked 최상위항목(전체 펼침562행) 보관. Assets/Packages/ProjectSettings/UserData3315파일 SHA baseline 대비 몬스터7외 변경0. 기존 PNG/WAV/meta/Save·사용자변경·미추적파일 보존, 관련파일만 Commit, Push0.
작업Commit은 이 문서 포함 `Feature: Chapter2 잡템 7종 및 몬스터 드롭 등록` 참조.
후속: 사용자7아이콘 미술승인, 별도 Handoff 문구 확인, 실제 Inventory/Shop UI·전투승리→Save/Continue·Main17~19 Runtime 회귀 확인. 기술상 필수 추가아트/재납품 사유 없음.
