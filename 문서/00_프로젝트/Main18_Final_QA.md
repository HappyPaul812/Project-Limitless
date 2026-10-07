# Main18 통합 QA 및 최종 인계

2026-10-08. **정식 설계·Gameplay·Quest 구현과 자동 검증 완료. Art 최종 승인·BGM 사람 청취·실물 입력 확인은 보류**. Main19/20, Cleanse II, 신규 TTS·음성 연결은 만들지 않았다.

| 판정 | 수량·근거 |
|---|---|
| Runtime PASS / FAIL | **500 / 0**, 두 최종 로그의 고유 label 기준 |
| Runtime 기록 PASS | 635, Scene 등 반복 label 포함 |
| Art 기술 Gate | PASS_WITH_NOTE17 / FAIL_BLOCKING0 |
| USER_REQUIRED | **20** = Art17 + Listening2 + 실물 입력1 |
| Compile Error | 0, 최종 Unity 컴파일 완료 |
| Runtime Console Error / Warning / 신규 Warning | 0 / 0 / 0, 최종 Console 조회 기준 |
| Missing Script / Quest Blocker | 0 / 0 |
| 무료 냉각약 중복 / 승리·완료 중복 보상 | 0 / 0 |
| TTS_PENDING | **22**, NPC 발화 기준. Player·지문 VoiceExpected=false |

초기 QA에는 대화창 Scene 전환 후 null 참조와 적 행동 중 Flee 무효 입력을 기다리지 않은 검증기 오류가 있었다. 마지막 대화 종료/아군 명령 대기 fixture를 수정한 뒤 전체 경로를 다시 실행했다. 최종 결과에는 이 초기 실패를 PASS로 바꾸어 집계하지 않는다. 기존 컴파일의 CS0618 경고는 이번 변경 범위 밖이며 새 Warning은 확인되지 않았다.

## 13목표·Field·보상

Main17 완료→Main18 자동 시작→아르벨 귀환→레온 보고/냉각약3→Field08 심부→Field09→지면→분출구→Tutorial Beetle→과열 잔류→Path 관찰→Watcher 목격 대화 종료→Watcher 전투→후속 조사→더 깊은 길→완료 전 경로 PASS. EXP70/Talent60, 아이템 보상 없음. 끝난 조사의 재입력/EndBattle 중복/Continue 보고 중복0.

Field09 Bounds21×15, east Field08 왕복 데이터, 보고 전 Field08 west 물리 Gate, Field09 west 닫힌 Boundary·세계관 안내. 진입/Exit 위치 비중첩. 4:3·16:9·21:9 × 동서남북12 viewport 검사 PASS. 일반 Beetle/Lizard/Wraith는 Story ID와 분리한 기존 Respawn 서비스다. 이전 Scene 재생성0.

## 과열·아이템·몬스터

과열은 전투 Runtime0~2 값. 세 번째 적용에서 ceil(MaxHP×.08), 최소1 피해 후0, KO 가능. 방어/철벽/가이아/수호/Path/펫 Direct 경계를 통과하지 않으며 SaveVersion1 유지, KO/승리/도망/종료/새 전투0. 기존 Poison/Burn/Shock/Silence 및 정화 계약 유지. 아이콘+과열1/3·2/3 위험+상세 정화 불가, 단일 PNG Apply/High/Burst.

냉각약은 생존 아군 하나의 과열 전부만 제거. 침묵 Actor 사용 PASS, 무열 대상 안내+아이템/행동/턴0 실제 Controller PASS. stack99·40/20. 레온 보고 끝에3개 한번, 수용 공간 없으면 보고 유지. 아르벨 전용 상점은 Main17 완료 또는 Main18 시작부터, Starter 상점 원본 변경0. Buy/Sell 실제 서비스 PASS.

갑충·감시자는 승인 수치와 반복 AI, EXP/Talent·Loot 없음·분양 없음·도망 가능·IsBoss=false를 사용한다. 감시자 최고 열 우선은 도발 대상 규칙 뒤에 적용. 광역55%에는 과열0. 두 전투 모두 도망/패배는 목표·보상 유지, 정상 승리 단회. [5 Job 대표검증](Main18_Balance_QA.md).

## Save/Continue·보호·Audio·Voice

실제 Save→Bootstrap Slot01 Action→필드 복귀 **7지점**: 시작/보고후/Field09진입/Beetle승리/Watcher직전/Watcher승리/완료. Quest·Completed·위치·Inventory·냉각약·통화·Party·Formation·CompanionUnlock·Beast·HP/MP·Path·Level/EXP 일치. 사용자 슬롯 대신 기존 비포커스 audit 저장/설정 격리를 사용했고 종료 때 복구했다.

Field09 탐색 The Weight of Obsidian, 모든 Field09 Battle Blade and Gambit 실제 Clip 참조 PASS. 기존 MP3/WAV byte 보호. Iron and Incantation·Paths of Cracked Earth 신규 배정0, Source1/Fade/Loop/Mixer 로직 변경0. 사람의 실제 곡·전환 청취 두 항목은 **USER_LISTENING_REQUIRED2**.

[TTS Manifest](Main18_TTS_Manifest.csv) 38페이지 중 NPC 22 TTS_PENDING, 나머지 Player/지문은 NOT_EXPECTED. Stable main18_ ID, API 호출0·음성 생성0·연결0. TTS_PENDING은 음성 작업 백로그이며 이번 구현의 자동 실패가 아니다.

보호 baseline3156 중 **3144 byte 동일**, 의도한 기존12개만 변경, 삭제0. UserData/기존 WAV·MP3/ThirdParty 원본/Player·Companion Sprite/기존 Monster5/기존 Scene/사용자 사전 미커밋 Asset의 byte를 유지했다. [보호 감사](Main18_Protected_Files_Audit.json). Save schema·Main17 Quest·Main03~07 Flow·추천직업 안내·길/직업 독립 구조는 변경하지 않았다. BuildSettings는 Field09 3줄만 commit에 포함하고 사용자 app-ui 설정 diff는 미커밋 그대로 남겼다.

## 미확인 항목과 다음 작업

**USER_ART_REVIEW_REQUIRED17**: 각 납품 PNG의 실제 미술 검토. 특히 원본 흰색 guide/테두리·반복 지면 경계·축소 HUD 가독성·몬스터 피격/KO/VFX의 체감 타이밍. 원본 재생성/수정0. [Art QA](Main18_Art_Import_QA.md).

**USER_LISTENING_REQUIRED2**: Field09 Obsidian 탐색/복귀, Blade 전투/복귀 곡·Fade 청취. 자동 mute audit를 실제 청취 PASS로 올리지 않았다.

**USER_INPUT_REVIEW_REQUIRED1**: 사용자가 편한 시점에 키보드/마우스/게임패드로 출입·조사·대화·냉각약 대상/취소·상세 팝업을 확인한다. 자동 검증은 포커스 전환·OS 입력·Computer Use 없이 실행했다. 종료 상태 clean Bootstrap Edit Mode, Editor 비포커스. 전체 기존 QA를 재실행하거나 새 게임 디자인을 확정하지 않았다.

다음 권장: 납품 Art 검토 의견 수집→승인한 원본만 교체, BGM 청취, 이후 별도 승인 TTS 작업. Main19 구현은 별도 요청 대상.

## Git 및 변경 파일

시작 HEAD `429428748b610e8ad8c8f4b3eefe590f3b48ca31` = 사용자 기준 원격4294287 및 로컬 추적 origin/main. 네트워크 fetch/push0; 실시간 서버 상태를 조회한 것으로 주장하지 않는다. 원래 미커밋 변경이 있는 상태였고 이를 유지했다.

- `2e6e86c` Docs: Main18 검은 열기 정식 설계
- `543e82b` Feature: Main18 신규 몬스터 및 원본 Art 등록
- `f72d907` Feature: Main18 독립 과열 상태 및 냉각약
- `6361b00` Feature: Main18 Field09 Quest 전투 및 BGM 연결
- QA commit은 이 문서를 포함한 최신 `Docs: Main18 통합 QA 및 현재 상태`에서 확인한다.

최종 git status는 기존 관련없는 미커밋 변경 유지가 정상이며 clean이라고 주장하지 않는다. Art/Gameplay/Quest/QA 변경만 명시적으로 stage했다. 자동 검사 결과 [Summary JSON](Main18_QA_Summary.json), 원본 [Art 감사](Main18_Art_Source_Audit.json), Runtime [통합](Main18_Runtime_Results.txt)·[Balance](Main18_Balance_Runtime_Results.txt).

변경 파일 목록(이번 commit 범위, 기존 미커밋 파일 제외):

- `"\353\254\270\354\204\234/00_\355\224\204\353\241\234\354\240\235\355\212\270/Chapter2_BGM_\354\240\225\353\263\270.md"`
- `"\353\254\270\354\204\234/03_\354\212\244\355\206\240\353\246\254/Chapter2_Main18_\352\262\200\354\235\200_\354\227\264\352\270\260.md"`
- `"\353\254\270\354\204\234/03_\354\212\244\355\206\240\353\246\254/Chapter2_\354\204\234\353\266\200_\353\260\251\355\226\245.md"`
- `"\353\254\270\354\204\234/06_\354\212\244\355\202\254/README.md"`
- `"\353\254\270\354\204\234/07_\354\225\204\354\235\264\355\205\234/\354\240\204\355\210\254_\354\206\214\353\252\250\355\222\210.md"`
- `"\353\254\270\354\204\234/08_\353\252\254\354\212\244\355\204\260/Chapter2_\353\252\254\354\212\244\355\204\260_1\354\260\250_\354\204\244\352\263\204.md"`
- `"\353\254\270\354\204\234/10_\354\240\204\355\210\254/\354\240\204\355\210\254\354\213\234\354\212\244\355\205\234.md"`
- `Tools/main18_audit.py`
- `Unity/Client/Assets/_Project/Resources/Audio/Music/BgmSceneCatalog.asset`
- `Unity/Client/Assets/_Project/Resources/FieldConnections/Field08_WestToField09.asset`
- `Unity/Client/Assets/_Project/Resources/FieldConnections/Field08_WestToField09.asset.meta`
- `Unity/Client/Assets/_Project/Resources/FieldConnections/Field09_EastToField08.asset`
- `Unity/Client/Assets/_Project/Resources/FieldConnections/Field09_EastToField08.asset.meta`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/CoolingRemedy.asset`
- `Unity/Client/Assets/_Project/Resources/ItemDefinitions/CoolingRemedy.asset.meta`
- `Unity/Client/Assets/_Project/Resources/Main18.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_AshPatch_01.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_AshPatch_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Cliff_Blocker_01.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Cliff_Blocker_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Fissure_Edge.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Fissure_Edge.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Fissure_Glow.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Fissure_Glow.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Ground_Base.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Ground_Base.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Ground_Cracked_01.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Ground_Cracked_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Ground_Cracked_02.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Ground_Cracked_02.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_HeatVent_01.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_HeatVent_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Obsidian_Slab_01.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Obsidian_Slab_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Obsidian_Spire_01.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/ObsidianScar_Obsidian_Spire_01.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Monsters.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Monsters/Obsidian_Beetle_Sprite_Sheet.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Monsters/Obsidian_Beetle_Sprite_Sheet.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/Monsters/Scorching_Watcher_Sprite_Sheet.png`
- `Unity/Client/Assets/_Project/Resources/Main18/Monsters/Scorching_Watcher_Sprite_Sheet.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/UI.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/UI/Item_Cooling_Remedy.png`
- `Unity/Client/Assets/_Project/Resources/Main18/UI/Item_Cooling_Remedy.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/UI/Status_Overheat.png`
- `Unity/Client/Assets/_Project/Resources/Main18/UI/Status_Overheat.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/VFX.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/VFX/Overheat_Apply.png`
- `Unity/Client/Assets/_Project/Resources/Main18/VFX/Overheat_Apply.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/VFX/Overheat_Burst.png`
- `Unity/Client/Assets/_Project/Resources/Main18/VFX/Overheat_Burst.png.meta`
- `Unity/Client/Assets/_Project/Resources/Main18/VFX/Overheat_High.png`
- `Unity/Client/Assets/_Project/Resources/Main18/VFX/Overheat_High.png.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/16_ObsidianBeetle.asset`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/16_ObsidianBeetle.asset.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/17_ScorchingWatcher.asset`
- `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/17_ScorchingWatcher.asset.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field09_Beetle01.asset`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field09_Beetle01.asset.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field09_Lizard01.asset`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field09_Lizard01.asset.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field09_Wraith01.asset`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field09_Wraith01.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/Main18_BlackHeat.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/Main18_BlackHeat.asset.meta`
- `Unity/Client/Assets/_Project/Resources/ShopDefinitions/ArbelGeneralShop.asset`
- `Unity/Client/Assets/_Project/Resources/ShopDefinitions/ArbelGeneralShop.asset.meta`
- `Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main18_BeetleReturn.asset`
- `Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main18_BeetleReturn.asset.meta`
- `Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main18_WatcherReturn.asset`
- `Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main18_WatcherReturn.asset.meta`
- `Unity/Client/Assets/_Project/Scenes/Field_09_ObsidianScar.unity`
- `Unity/Client/Assets/_Project/Scenes/Field_09_ObsidianScar.unity.meta`
- `Unity/Client/Assets/_Project/Scripts/Audio/BgmSceneCatalog.cs`
- `Unity/Client/Assets/_Project/Scripts/Battle/BattleActionPresenter.cs`
- `Unity/Client/Assets/_Project/Scripts/Battle/BattleChapter2MonsterRuntime.cs`
- `Unity/Client/Assets/_Project/Scripts/Battle/BattleCombatantStatusViewModel.cs`
- `Unity/Client/Assets/_Project/Scripts/Battle/BattleItemUseService.cs`
- `Unity/Client/Assets/_Project/Scripts/Battle/BattlePrototypeEncounter.cs`
- `Unity/Client/Assets/_Project/Scripts/Battle/BattleSceneController.cs`
- `Unity/Client/Assets/_Project/Scripts/Battle/BattleSkillSystem.cs`
- `Unity/Client/Assets/_Project/Scripts/Core/ItemDefinition.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main18ContentBuilder.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main18ContentBuilder.cs.meta`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main18RuntimeAudit.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main18RuntimeAudit.cs.meta`
- `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcRole.cs`
- `Unity/Client/Assets/_Project/Scripts/World/Chapter2Main18Flow.cs`
- `Unity/Client/Assets/_Project/Scripts/World/Chapter2Main18Flow.cs.meta`
- `Unity/Client/Assets/_Project/Scripts/World/Main18DialogueCatalog.cs`
- `Unity/Client/Assets/_Project/Scripts/World/Main18DialogueCatalog.cs.meta`
- `Unity/Client/ProjectSettings/EditorBuildSettings.asset`
- `문서/00_프로젝트/CURRENT_STATUS.md`
- `문서/00_프로젝트/Main18_Art_Import_QA.md`
- `문서/00_프로젝트/Main18_Art_Source_Audit.json`
- `문서/00_프로젝트/Main18_Balance_QA.md`
- `문서/00_프로젝트/Main18_Balance_Runtime_Results.txt`
- `문서/00_프로젝트/Main18_Final_QA.md`
- `문서/00_프로젝트/Main18_Importer_Settings.txt`
- `문서/00_프로젝트/Main18_Protected_Files_Audit.json`
- `문서/00_프로젝트/Main18_QA_Summary.json`
- `문서/00_프로젝트/Main18_Runtime_Results.txt`
- `문서/00_프로젝트/Main18_TTS_Manifest.csv`
- `문서/00_프로젝트/QA_증거/Main18/Art_Manifest.txt`
- `문서/00_프로젝트/QA_증거/Main18/Art_Source_Contact.png`
- `문서/00_프로젝트/QA_증거/Main18/Battle_Heat1_1280.png`
- `문서/00_프로젝트/QA_증거/Main18/Battle_Heat1_1600.png`
- `문서/00_프로젝트/QA_증거/Main18/Battle_Heat1_1920.png`
- `문서/00_프로젝트/QA_증거/Main18/Battle_Heat2_1280.png`
- `문서/00_프로젝트/QA_증거/Main18/Battle_Heat2_1600.png`
- `문서/00_프로젝트/QA_증거/Main18/Battle_Heat2_1920.png`
- `문서/00_프로젝트/QA_증거/Main18/Field09_1280.png`
- `문서/00_프로젝트/QA_증거/Main18/Field09_1600.png`
- `문서/00_프로젝트/QA_증거/Main18/Field09_1920.png`

GitHub Push 하지 않음
