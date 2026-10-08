# Main19 정식 설계 및 기초 구현 QA

## 구현 전 — 2026-10-08

시작HEAD4e54c3e0bc7f36dbbd9c7ecf7272983b25ffd117, origin/main동일·사용자dirty75/미추적26보호. 설계→새Quest12/Field10골격/Field09왕복/StoryA/B/BGM/TTS Manifest→비포커스검증. 기존Monster수치·Main18과열/냉각·SaveVersion·Main07/17완료Voice를 보호한다. 신규Art/Voice/API/Push0.

검증 예정: Quest12 연속·도망/패배재도전·승리단일진행·Witness/Hook·Path5·원본수치/Party보존·Continue·Bounds/Spawn·Compile/Console/MissingScript. 최종미술/난이도체감/새NPC음성은 미검증상태로 명시한다.

## 완료 판정 — 기초 구현 PASS

- Main19 `main_19_burning_pulse` / 타오르는 맥동 / Main18 선행 / EXP80·Currency70. 승인 12개 Objective가 실제 Runtime에서 순서대로 완료됐다.
- Field10 「맥동의 열맥」 21×15 골격, Field09 왕복 정의2, 동쪽 실제 Exit1·서쪽 Bounds와 안내. 4:3/16:9/21:9 viewport 및 Spawn/Exit 비중첩 PASS. Main20 Scene·Boss·최종 원인 구현0.
- A 흑요 갑충1+균열 도마뱀1(전열), B 흑요 갑충1+잔불 망령1(후열). 실제 일반 Attack 입력으로 둘 다 승리. 각 조우 도망·전멸 시 목표·보상 변화0, 재도전 성공·승리 단일 진행. 중복 EndBattle/완료 보상0. Optional3 기존 종 단독·respawn35 정의/Factory 확인, Optional 실제 전투는 별도 미검증.
- 원본 MonsterDefinition HP·공격%·민첩·AI·과열/냉각/정화 변경0. 기존 Factory로 저장 Party·Formation 사용. 신규 Monster/Elite/Boss/상태/상점/동료0.
- 조사·Witness·Hook 실제 명시 Next 진행, 조기 목표 진행0. Path5 분기 고유 Player ID 확인, Runtime은 Hearing으로 전체 완료. Witness는 현재 활성 Taeon/Miel/Paul만 Flavor, 필수 Serin은 Story 안내. 다른 Party 조합 실제 Play는 미검증.
- Field BGM 기존 Paths of Cracked Earth, Battle 기존 Blade and Gambit 실제 clip 참조 PASS. 새 음원/Source/Fade/Loop/Mixer 변경0.
- TTS Manifest 23고유 페이지: NPC13 `TTS_PENDING`, Player9+지문1 `VoiceExpected=false`. 신규 StoryVoiceCatalog/WAV 변경·API0. Main07 작별5/Main17 신규14 CLOSED 유지, Main18 TTS_PENDING22 유지.
- 기존 Main18 그림을 Field10 임시 재사용. Art8종(균열 장식2변형)+VFX2 `ART_PENDING / USER_ART_REVIEW_REQUIRED`. 최종 미술·실루엣 가독성·실물 입력·전 직업 난이도 체감은 사용자 검토/후속 QA.
- Continue 실제 Bootstrap 슬롯 버튼4시점: A 전/A 후/Witness 전/완료 후. 위치·목표/완료 기록·Party/Formation/Unlock·Beast·HP/MP·Path·Inventory·Currency·Level/EXP 동일. SaveVersion1·직렬화 변경0, 사용자 Save/Settings 보호. 실행기 격리 경로는 기존 Partial9 실행기 재사용.
- Unity 6000.5.7f1 비포커스 PlayUnfocused, 창/OS 입력/포커스 조작0. 최종 clean Bootstrap EditMode·Save/Settings/Play 설정 복구. Compile Error0·Runtime Console Error0·신규 Runtime Warning0·Missing Script0. 프로젝트 기존 CS0618 컴파일 경고는 별도 기존 항목.

최종 자동 검사 **169 PASS / FAIL0**. [실행 결과](Main19_Runtime_Results.txt) · [보호 비교](Main19_Protected_Files_Audit.json). 기존3275파일 중3270 byte 동일, 승인된5만 변경·예상밖 변경/삭제0. Build Settings 기존 사용자 app-ui 변경은 커밋에서 제외한다.

## 변경 파일 목록

- `Unity/Client/Assets/_Project/Resources/Audio/Music/BgmSceneCatalog.asset`
- `Unity/Client/Assets/_Project/Resources/FieldConnections/Field09_WestToField10.asset`
- `Unity/Client/Assets/_Project/Resources/FieldConnections/Field09_WestToField10.asset.meta`
- `Unity/Client/Assets/_Project/Resources/FieldConnections/Field10_EastToField09.asset`
- `Unity/Client/Assets/_Project/Resources/FieldConnections/Field10_EastToField09.asset.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field10_Beetle01.asset`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field10_Beetle01.asset.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field10_Lizard01.asset`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field10_Lizard01.asset.meta`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field10_Wraith01.asset`
- `Unity/Client/Assets/_Project/Resources/MonsterSpawns/Field10_Wraith01.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/Main19_BurningPulse.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/Main19_BurningPulse.asset.meta`
- `Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main19_PatrolA.asset`
- `Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main19_PatrolA.asset.meta`
- `Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main19_PatrolB.asset`
- `Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main19_PatrolB.asset.meta`
- `Unity/Client/Assets/_Project/Scenes/Field_10_BurningPulse.unity`
- `Unity/Client/Assets/_Project/Scenes/Field_10_BurningPulse.unity.meta`
- `Unity/Client/Assets/_Project/Scripts/Audio/BgmSceneCatalog.cs`
- `Unity/Client/Assets/_Project/Scripts/Battle/BattlePrototypeEncounter.cs`
- `Unity/Client/Assets/_Project/Scripts/Battle/BattleSceneController.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main19ContentBuilder.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main19ContentBuilder.cs.meta`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main19RuntimeAudit.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main19RuntimeAudit.cs.meta`
- `Unity/Client/Assets/_Project/Scripts/World/Chapter2Main19Flow.cs`
- `Unity/Client/Assets/_Project/Scripts/World/Chapter2Main19Flow.cs.meta`
- `Unity/Client/Assets/_Project/Scripts/World/Main19DialogueCatalog.cs`
- `Unity/Client/Assets/_Project/Scripts/World/Main19DialogueCatalog.cs.meta`
- `Unity/Client/ProjectSettings/EditorBuildSettings.asset`
- `문서/00_프로젝트/CURRENT_STATUS.md`
- `문서/00_프로젝트/Chapter2_BGM_정본.md`
- `문서/00_프로젝트/Main19_Art_Handoff.md`
- `문서/00_프로젝트/Main19_Design_QA.md`
- `문서/00_프로젝트/Main19_Protected_Files_Audit.json`
- `문서/00_프로젝트/Main19_Runtime_Results.txt`
- `문서/00_프로젝트/Main19_TTS_Manifest.csv`
- `문서/03_스토리/Chapter2_Main19_타오르는_맥동.md`
- `문서/03_스토리/Chapter2_서부_방향.md`

다음 권장: Main19 Art 최종 전달/사용자 검토, NPC13 TTS 제작 및 전체 직접 청취 승인 뒤 별도 Unity 적용. Main18 TTS22는 기존 별도 대기 범위. GitHub Push0.

구현 commit `52259d5`. 시작 origin/main 동일(0ahead/0behind), 최종 문서 커밋 포함 로컬2ahead/0behind 예정·원격 조회/Push0. 최종 기존 사용자 tracked75/미추적26 유지, staged0 판정은 최종 Git 조회로 확인한다.
