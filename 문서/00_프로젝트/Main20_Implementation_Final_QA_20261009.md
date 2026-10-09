# Main20 「심부의 거신」 구현 및 최종 백그라운드 QA

2026-10-09. 시작 LOCAL HEAD `ff9a2e30501ebc2842d282e5b669ce41ddd71b06`. Unity `6000.5.7f1`. **IMPLEMENTED_BACKGROUND_QA_PASS / USER_RUNTIME_REVIEW_REQUIRED**.

사용자 지시로 v3 미술 및 Phase2 Overlay 채택 승인, 기술 계약 3건을 확정하고 실제 Quest9/Field11/Boss/아트/BGM/Save를 구현했다. 기존 v1/v2/v3 검사 및 StarterVillage 조사 문서는 보존한다. Main21 상세 Quest/Story, TTS 생성, 일반 몬스터 추가, 원본 아트 수정은 하지 않았다.

## 구현 범위

| 항목 | 결과 | 실제 연결 |
| --- | --- | --- |
| Main20 Quest | 9/9 | `main_20_colossus_of_the_depths`, Main19 선행·승리 후 진행·완료 EXP100/Talent80 |
| Field11 | 구현 | `Field_11_DeepCore` / 열맥 심부, Bounds21×15, 승인 환경8, 일반 몬스터0 |
| Field10 왕복 | 구현 | 서쪽 Runtime Opening+Main19 완료 Gate, Field11 동쪽 Exit, Spawn과 Trigger 분리 |
| Boss | 구현 | `veinfire_colossus`, Encounter `field11_main20_veinfire_colossus`, Lv15/HP1200/Attack30/AG10/BaseEXP140/Talent40 |
| Boss 제한 | 적용 | IsBoss=true, 도망·야수 동료 습격 차단, Loot0, 파티 저장 편성 사용 |
| 아트 | 적용 | 신규 PNG17: Boss 기본/Overlay2, VFX7, 환경8(추가 Trace 포함), QA5 제외 |
| BGM | 적용 | Main20 지정 조우 Crowns, 승리 화면 유지, Field11 탐색/복귀 Paths·기존 Fade/Loop/단일 Source 재사용 |
| Save/Continue | Version1 유지 | Quest·HP/MP·Inventory·Party/Formation·Beast·Path·좌표·보상 상태 복원, 5슬롯 |
| 대사/TTS | 텍스트8 연결 | Manifest Stable ID/원문/줄바꿈 일치, Serin5 음성 제작 대기·Player2/지문1 무음, API0/신규 WAV0 |

Main20 Boss는 별도 Runtime과 Controller partial로 연결해 파수꾼60%·소환·장막 상태와 분리했다. Battle Scene/StarterVillage/Field10 원본 Scene을 재생성하지 않았다. Field11은 격리 복사본에서 새 GUID로 생성하고 Field10 전용 목격 Presenter를 제거한 새 Scene이다.

## 승인된 전투 계약

- HP600 이하 최초 피해 직후 Phase2 1회, HP·Queue·Buff/Debuff·Burn/Overheat·Taunt·Cooldown 초기화/추가 턴0. `Combatant.HpChanged`를 관측하며 DoT도 동일 경계다.
- 미실행 Phase1 응축 파동 예고가 있으면 다음 자기 행동에 원래 파동1회 이행 → Phase2 열압 주입. 예고가 없으면 다음 자기 행동 열압 주입. 공명 예고는 다음 자기 행동 분출 후 열파로 이어진다.
- Phase1: 열압 주입90% 단일/생존 과열+1 → 용융 강타125% 단일/Burn9×2회 → 응축0·예고 → 파동80% 생존 전체. Phase2: 주입 → 강타 → 공명0·예고 → 분출60% 전체/생존 과열+1 → 열파55% 전체.
- 용융 강타는 승인된 `MeleePhysical`. 도발 우선 및 전열 보호/빈 전열의 후열 대상 규칙을 재사용한다. 주입은 마법 범위의 도발 우선 후 가장 높은 과열 후보를 고른다. 방어 무시0.
- 직접 피해는 올림·최소1, 기존 outgoing/path/받는 피해/Guard·철벽·가이아/수호의 맹세 경로를 재사용한다. 과열3이면 최대HP8% 올림 피해 후0, 냉각약과 Burn 공용 코드 유지.
- Boss BaseEXP140에 기존 레벨 차 배율 적용(동레벨140, 붉은210, 회색0). 전투 Talent40과 Quest EXP100/Talent80은 별개로 각각1회. EndBattle 중복 경계 및 Quest 완료 경계를 재사용한다.
- Phase 무음 지문은 전환 이벤트1회로 Boss HUD에 보존한다. 일반 피해 메시지에 덮이지 않으며 입력/턴을 추가로 중단하지 않는다. 예고는 HUD와 기존 Timeline을 함께 사용하며 색에만 의존하지 않는다.

## Objective·좌표·표현 대응

좌표는 World(x,y), 표시 Objective는1부터 시작한다. Site는 현재 목표만 거리0.75 이내에서 반응하고 대화 완료 및 프레임 경계를 확인한다.

| 목표 | Objective ID | 목표 Type / Target | 좌표 | 아트·처리 |
| --- | --- | --- | --- | --- |
| 1 | enter_deep_core | Reach / field11_main20_entry | (7.8,0) | Ground, 실제 Field10 출구로 진입 후 도달 |
| 2 | inspect_core_rift | Interact / field11_main20_core_rift | (5,1.1) | Rift02, 조사 대화 완료 |
| 3 | follow_colossus_trace | Reach / field11_main20_trace | (2.8,0.2) | ColossusTrace 추가 납품 사용 |
| 4 | reach_colossus_arena | Reach / field11_main20_arena | (0,0) | ArenaPressureMark, Boss는(-2,0) |
| 5 | confront_veinfire_colossus | Interact / field11_main20_confront | (-1.7,0) | Serin/Player 본문4, 완료 후 자동 Boss전 |
| 6 | defeat_veinfire_colossus | DefeatEncounter / field11_main20_veinfire_colossus | (-1.7,0) | 승리만 완료, 패배 후 같은 Site에서 재도전 |
| 7 | inspect_collapsed_core | Interact / field11_main20_collapsed_core | (-2,1) | 승리 후 Boss 숨김·CollapsedCore, 본문3 |
| 8 | confirm_heat_recession | Interact / field11_main20_heat_recession | (1,-1.2) | HeatRecession·Rift01, 열기 변화 확인 |
| 9 | return_from_deep_core | Reach / field11_main20_return | (7.8,0) | 돌아갈 길 확보→Main20 완료, 동쪽 출구로 Field10 복귀 |

Field10 신규 Spawn_From_Field11(-7.8,0), 서쪽 Exit(-10.25,0)/size(1.1,3), Main20AccessGate(-9.45,0)/size(0.35,3). Field11 Spawn_From_Field10(7.8,0), 동쪽 Exit(10.25,0)/size(1.1,3). Field11 서쪽/상하 경계는 닫혀 있다. 절벽2개(-4,±4.7)의 하부 폭3×높이1 충돌 범위는 중앙9목표와 겹치지 않는다. Boss 대치·조사는 키보드E/F·패드South binding을 사용한다.

## Art·Audio Import 및 원본 보호

- v3 ZIP SHA256 `eed33ea29d31788ee30d38750bc186fe6178d658c9c10881bb02a6355a7da60f`. 게임 PNG17 모두 ZIP 내부 해당 파일과 byte 동일. 기존 PNG/WAV/meta/GUID/Save를 덮어쓰지 않고 신규 경로·신규 GUID를 사용했다.
- Boss 둘 모두1256×1256, 314×314 FullRect 셀4×4/16, top-row index00~15, PPU314/Point/무압축/NPOT None/미리보기 잘림 없는 FullRect. 기본Idle00~03, Attack04~07, Skill-Hit08~11, KO12~15, 8FPS·KO 마지막 유지.
- 공식 Phase2 완성 시트 항목은 사용자 승인된 `Veinfire_Colossus_Phase2_Overlay.png` 대안으로 대응한다. 기본 `CurrentFrame`의 번호를 LateUpdate에서 참조하고 독립 타이머 없이 동일 Rect/Scale/Flip/Pivot/색을 따른다. 원본 KO14~15 alpha0 유지.
- VFX7 모두512×512 Single/PPU512, 응축·공명은 Boss 예고, 나머지는 대응 피해 대상에 정적 단일 프레임을0.5초 부드러운 alpha로 표시한다. 4분할/새 Sprite/강한 섬광·화면 진동0.
- 환경8 Single/PPU128/Point/무압축. Ground Tiled21×15, Arena7×5, Rift3×2, Trace3×1.8, 잔해3×2, 냉각5×3. Sorting Order Ground-20, Rift-9, Trace-8, Arena-7, 냉각-6, 잔해1, 절벽2, Boss5. 기본Sorting Layer 재사용.
- Crowns 원본4273649bytes/SHA256 `4f91dc087a57927a354dd28de5cc0fe2eddf47bf3c1103200bbe72d46260a9fc`, 신규 MP3와 byte 동일. Unity Streaming/Vorbis quality1/원본 sample rate/stereo/preload false; 원본 MP3 재인코딩/정규화/길이 수정0. Paths 원본과 Mixer/Fade 서비스 보호.

## 백그라운드 QA 증거

격리 프로젝트 `Temp/Main20QA/Client`에 시작 당시 LOCAL Assets/Packages/ProjectSettings를 복사했다. Unity Batch Mode/nographics/Hidden으로 실행하고 AuditSaveDirectory와 격리 설정만 사용했다. 원본 Editor에서는 포커스 전환 없이 MCP 상태·컴파일/Console을 확인했다. 원본 Play0·Scene 전환0·화면 조작0.

[최종 assertion 결과](Main20_Runtime_Results_20261009.txt). 아래 숫자는 고유 테스트 케이스 수가 아닌 PASS assertion 수다. 전체1373 PASS/FAIL0.

| 격리 검사 | 판정 | PASS assertions | FAIL |
| --- | --- | --- | --- |
| Main20 | PASS | 151 | 0 |
| Main20 일반 턴 전투 | PASS | 12 | 0 |
| Main18 | PASS | 467 | 0 |
| Main19 | PASS | 169 | 0 |
| Side9 | PASS | 574 | 0 |

| 검사 범위 | 결과와 한계 |
| --- | --- |
| Quest9 및 이동 | 실제 Site Update/대화 완료/자동 전투, 물리 Exit Trigger 양방향, Main19 미완료 잠금, 패배 안전지대·같은 목표6 재도전·완료 후 재지급 차단 PASS |
| Boss 패턴 | 순수 Runtime 상태의 두 반복 사이클, 601/600·1회·예고 보존 PASS. 실제 Controller 두 Phase 전체 coroutine/VFX·피해/status 연결 PASS |
| 일반 전투 | Lv15 전사+세린+미엘, 실제 Attack Button→대상 선택→Turn Queue, 공격19회 승리. HP160/100/56, BossHP0, Phase 전환1회·승리복귀 PASS. 기본 공격 검증이며 전 직업 난이도 승인은 아님 |
| 대상·제한 | 전열 보호·후열 도발·광역 생존 집합 모델 PASS, 실제 Controller 도망/야수 습격 차단·HP/턴 무소비 PASS |
| EXP/보상 | 실제 Boss 동레벨EXP140/Talent40·중복 차단, Quest EXP100/Talent80 실제 완료 지급·중복 차단 PASS |
| Save/Continue | Boss 전/승리 직후/Quest 완료 후 실제 Bootstrap Slot01 버튼, 정확 Quest/좌표/Currency/파티·편성·야수/Inventory·HP/MP·Path·EXP 복원 PASS. 5개 격리 슬롯 저장/읽기 Version1 PASS |
| Geometry/Art | 4:3/16:9/21:9 각4방향 카메라Viewport, Spawn-Exit 분리·절벽 충돌3×1/동선 비중첩 PASS. Boss16+Overlay16 등록, 실제Phase 및 KO최종15 PASS. 픽셀 프레임 경계/KO소거는 승인 v3+Import metadata 검사 |
| BGM | 실제 Boss Crowns·결과 화면 유지·Field Paths/Continue 복원 PASS. 사람의 음량·음질·연결 청취는 별도 |
| Main18 | 기존 규칙/두 실제 전투/Retry/냉각약 지급·구매·판매/Continue까지 PASS. 기존 NPC 메뉴와 Main19 서쪽 이동을 반영하도록 격리 구형 검사기만 보정 |
| Main19 | 실제 혼합 두 전투·Retry·Quest 완료·Continue·Camera/Field10, Main19 종료 후 Main20 Gate 해제 PASS. 기존 단일 출구/영구폐쇄 가정은 신규 연결에 맞춰 격리 검사기만 보정 |
| Side9 | 실제 NPC 수락/거절/확인 납품, 부족·판매 후 감소·중복·보상 실패 보호·Save/Continue PASS. 기존 파일 및 자산 보존 |
| Loot7 | 7종 Definition/확률/판매가/99중첩/아이콘/구매 제외, 실제 공용 보상 성공1개·확률경계 실패·중복 개체1회·판매가 PASS. 해당 수량의 Main20 Continue 복원 PASS. 모든 종 필드 자연 접촉 전투와 확률 통계 측정은 NOT_VERIFIED |
| 파수꾼 | 원래60%·소환 장막30%·DoT 제외·수호체KO 장막 해제 모델 PASS. 던전 Boss 전투 전체 UI 완주 재실행은 NOT_VERIFIED |
| Compile/Console | 최종 격리 compile error0·Main20 Runtime Console Error0. 원본 Refresh/compile 후 MCP Console Error0·Bootstrap/Edit/포커스false 유지 |
| Missing Script | Main20 실제 로드 Bootstrap/Battle/Field10/Field11/StarterVillage 0. Main18/19/Side9 로드 Scene 검사도0 |

## 실패 이력과 해결

실패를 숨기거나 PASS로 전환하지 않았다. 최종 증거는 수정 후 재실행 결과다.

1. 초기 격리 QA의 존재하지 않는 ItemCatalog.Find 호출은 기존 TryGet으로 수정하고 컴파일 재검증했다.
2. 격리 Unity Search 시작 인덱스 내부 ArgumentOutOfRangeException은 격리 UserSettings/Search.settings의 indexOnEditorStartup=false로 비활성화했다. 게임 코드/원본 설정을 바꾸지 않았고 이후 Console0이다.
3. 추가 검사 fixture의 미엘 미해금과 Spawn GameObject 이름 가정은 실제 정식 해금/Stable Spawn ID로 바로잡고 재실행했다.
4. 구형 Main18 검사기는 새 SideQuest NPC 메뉴를 선택하지 않아 냉각약 단계에서, 기존 Main19 양방향 출입구를 단일 출구로 가정해 Geometry에서 실패했다. 격리 복사본의 실제 기존 업무 버튼 선택/잠긴 Gate·양쪽 Opening 검사로 보정했다. Main18/19 원본 검사기와 게임 코드는 변경0.
5. 원본 scripts-only 컴파일 요청은 신규 클래스 미등록 CS0246을 냈다. 원본 Scene/Play/포커스 유지한 전체 Asset Refresh 후 컴파일 및 MCP Console Error0으로 해결했다.

## 생성·수정 파일

소스는 `Unity/Client/Assets/_Project/Scripts/` 기준이다. 아래 신규 C#의 meta만 신규 생성했고 기존 source meta는 보존했다.

- `F:/study/codex/Project-Limitless/Battle/BattleMain20BossRuntime.cs`
- `F:/study/codex/Project-Limitless/Battle/BattleSceneController.Main20.cs`
- `F:/study/codex/Project-Limitless/Editor/Main20ContentBuilder.cs`
- `F:/study/codex/Project-Limitless/Editor/Main20RegressionBatch.cs`
- `F:/study/codex/Project-Limitless/Editor/Main20RuntimeAudit.cs`
- `F:/study/codex/Project-Limitless/Monster/Main20PhaseOverlay.cs`
- `F:/study/codex/Project-Limitless/World/Chapter2Main20Flow.cs`
- `F:/study/codex/Project-Limitless/World/Main20DialogueCatalog.cs`
- `F:/study/codex/Project-Limitless/World/Main20FieldPresentation.cs`
- `Audio/BgmSceneCatalog.cs`
- `Battle/BattleBackgroundCatalog.cs`
- `Battle/BattleCore.cs`
- `Battle/BattlePrototypeEncounter.cs`
- `Battle/BattleSceneController.cs`
- `Monster/MonsterSpriteSheetAnimation.cs`

신규/명시 변경 자산은 `Unity/Client/` 기준이다. PNG17 및 새 자산/폴더 meta를 포함한다. 기존 BgmSceneCatalog에 Field11/Crowns만 추가하고 기존 사용자 BuildSettings config를 보존한 Field11 Scene 추가 부분만 Commit한다.

- `Assets/_Project/Audio/Music/The_Weight_of_Crowns.mp3`
- `Assets/_Project/Resources/Audio/Music/BgmSceneCatalog.asset`
- `Assets/_Project/Resources/FieldConnections/Field10_WestToField11.asset`
- `Assets/_Project/Resources/FieldConnections/Field11_EastToField10.asset`
- `Assets/_Project/Resources/Main20/Boss/Veinfire_Colossus_Phase2_Overlay.png`
- `Assets/_Project/Resources/Main20/Boss/Veinfire_Colossus_Sprite_Sheet.png`
- `Assets/_Project/Resources/Main20/Environment/DeepCore_Arena_PressureMark.png`
- `Assets/_Project/Resources/Main20/Environment/DeepCore_Cliff_Boundary.png`
- `Assets/_Project/Resources/Main20/Environment/DeepCore_CollapsedCore.png`
- `Assets/_Project/Resources/Main20/Environment/DeepCore_ColossusTrace.png`
- `Assets/_Project/Resources/Main20/Environment/DeepCore_Ground_Base.png`
- `Assets/_Project/Resources/Main20/Environment/DeepCore_HeatRecession.png`
- `Assets/_Project/Resources/Main20/Environment/DeepCore_Rift_01.png`
- `Assets/_Project/Resources/Main20/Environment/DeepCore_Rift_02.png`
- `Assets/_Project/Resources/Main20/VFX/Veinfire_CoreCondensation_Telegraph.png`
- `Assets/_Project/Resources/Main20/VFX/Veinfire_CoreEruption_VFX.png`
- `Assets/_Project/Resources/Main20/VFX/Veinfire_CoreResonance_Telegraph.png`
- `Assets/_Project/Resources/Main20/VFX/Veinfire_CoreWave_VFX.png`
- `Assets/_Project/Resources/Main20/VFX/Veinfire_HeatPressureInjection_VFX.png`
- `Assets/_Project/Resources/Main20/VFX/Veinfire_HeatWave_VFX.png`
- `Assets/_Project/Resources/Main20/VFX/Veinfire_MoltenStrike_VFX.png`
- `Assets/_Project/Resources/MonsterDefinitions/18_VeinfireColossus.asset`
- `Assets/_Project/Resources/QuestDefinitions/Main20_ColossusOfTheDepths.asset`
- `Assets/_Project/Resources/StoryEncounterReturns/Main20_ColossusReturn.asset`
- `Assets/_Project/Scenes/Field_11_DeepCore.unity`
- `ProjectSettings/EditorBuildSettings.asset`

문서: 이 문서와 Main20_Runtime_Results_20261009.txt 생성, Story/Design_QA/CURRENT_STATUS 갱신. v1/v2/v3 Art 검사, Handoff/TTS CSV/Audio 계약, StarterVillage 기준 문서 보존.

## 보호 검증 및 Commit

시작 Git status는 tracked 사용자 변경75/미추적 top-level25(전체porcelain562행)였고 staged0이었다. 기존4306개 파일의 SHA256 기준 목록을 기록했다. 승인된 코드/문서/BGM catalog와 BuildSettings의 신규 추가를 제외한 파일은 재검사하여 예상 밖 변경0이다. 기존 Scene/Prefab/PNG/meta/WAV/TTS/Save/Side9/Loot7/StarterVillage 사용자 작업을 복구/reset/revert하지 않았다. Unity Refresh 이후에도 보호 SHA 차이0이다.

구현 Commit과 QA 문서 Commit은 본 작업 파일만 포함한다. 기존 사용자 App UI BuildSettings 항목은 로컬에 유지하고 Index에는 신규 Field11 항목만 추가했다. GitHub Push0·fetch0. 구현 Commit `eaefb0c8b11596a50bc0e81ffa46c8582ad92ccc`. QA 문서 Commit은 이 문서를 포함한 `Docs: Main20 최종 백그라운드 QA와 현재 상태 기록`이며 정확 SHA는 최종 보고를 따른다.

## 사용자 직접 확인 및 후속 작업

- **NOT_VERIFIED:** 실제 렌더링에서 Ground 반복/전투장 연결·절벽 충돌 체감·Boss 크기/피격/KO·Overlay/HUD 겹침·VFX 구분과 가독성. nographics 검증을 사용자 미술 승인으로 확대하지 않는다. v3/Overlay 채택 승인은 완료 상태로 유지하고 추가 재제작 요구0.
- **USER_LISTENING_REQUIRED:** Crowns 최종 음악 청취, Paths↔Crowns 전환·음량/Loop·Mixer 체감.
- **NOT_VERIFIED:** 키보드/마우스/게임패드 물리 입력·UI 포커스 체감, 전 직업/길/야수 구성 및 다양한 저장 상태의 난이도. Lv15 한 파티 승리를 전 직업 보장으로 해석하지 않는다.
- **TTS_PENDING:** 세린5개 WAV 제작/연결은 별도 quota 운영과 Main18→19 제작 우선순위를 따른다. 텍스트로 Main20 완주 가능하며 이번 API 호출0.
- 사용자는 Main19 완료 Save에서 Field10 서쪽→Field11→9목표/승리·귀환을 직접 확인할 수 있다. 불필요한 아트 재제작이나 Main21 상세 구현은 남겨두지 않았다. 다음 작업은 사용자 실제 화면/음악/난이도 검토와 기존 TTS 후속 제작이다.
