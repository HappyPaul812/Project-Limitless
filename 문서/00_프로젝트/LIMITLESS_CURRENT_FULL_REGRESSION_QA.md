# LIMITLESS 현재 전체 회귀검증 계획/결과 — 2026-10-05

LOCAL 프로젝트가 정본이며 GitHub main 조회/Push 없음. 순서: 문서화→검증/녹화 도구 준비→백그라운드 전체 회귀→실제버그 최소수정/재검증→녹화상태 준비→리허설/녹화. 사용자 기존변경/Save/Settings/원본Sprite/Audio/Catalog를 보존한다.

## 상태와 근거

PASS/FAIL/NOT_APPLICABLE/NOT_VERIFIED를 사용한다. 기존 신뢰 가능한 QA는 현재 코드/참조 보존 여부를 확인하여 재사용하고 신규 실행/정적 검사/이력 근거를 명확히 구분한다. 범주 PASS는 기재된 기술 검증 범위를 뜻하며 사람의 청취 품질/모든 입력장치/전체 수동 playthrough까지 뜻하지 않는다. UI와 음성의 미검증 하위항목은 별도로 기록한다.

|범주|상태|이번 검사/재사용 근거|
|---|---|---|
|A Bootstrap/Title|PASS|격리 Bootstrap 슬롯/Title 정상; Replay 경계 최소 수정 후 별도 재검증|
|B Intro|PASS|신규 Title→Intro/완결된 발화 구간/Skip;18 Clip 연결 및 Next/cleanup 기존 Audio QA 재사용|
|C Character Creation|PASS|신규50 Mapping/Job Preview·대표남녀 기본정보→Confirm→World;추천은비제한·수동외형UI없음|
|D Save/Continue|PASS|신규대표남녀 실제 Bootstrap Continue/구버전ID 재계산/fallback·Main09/10 단계별Continue|
|E Main01–Main16|PASS|16Quest/103Objective ID 정적검사·순수Quest감사·Main09/10 신규실행·Main01–08/11–16 이력통합|
|F World/Scene Transition|PASS|대표 생성→마을→Field01→Battle→Field·Main09/10 Field03↔Bootstrap↔DungeonB1;전체기존Scene/Bounds이력|
|G Party/Companion|PASS|Main09 115PASS:고정Player/동료2/폴해금·3편성실제전투승리·StoryOverride·Continue;세린Main16이력|
|H BeastCompanion|PASS|신규Player Bear/Serin Fox 독립장착·JSON복원·Bear+10%/Fox+2/비사수거절;Wolf/HP비율/전투교체차단등기존QA·코드보존|
|I Battle|PASS|신규일반슬라임정상스킬/공격/승리·Main09 3편성전투;Turn/Target/Guard/Item/Flee 기존실행이력|
|J Skill15|NOT_VERIFIED|15종정식Catalog/직업연결보존·Mage3종및Paul Fireball 실제실행.15종모두의현재UI/취소/타깃회귀는미완료|
|K Status Effects|PASS|신규BattleSilenceAudit:4상태동시Cleanse/비용·버프보존/침묵명령차단;독·화상·감전·Guard 기존계산/실행QA|
|L Item|PASS|신규EconomyInventoryAudit:Catalog/보상/상점/사용·실패원자성;취소·개별치료기존QA|
|M Defeat/Flee|PASS|녹화리허설의실제패배→안전지대/HP회복;정식Flee/Grace/재조우·성장/소모품보존기존QA|
|N DungeonB1/B2|NOT_VERIFIED|신규Main10 63PASS:실제B1진입/4Spawn/Field03복귀·기존B1/B2/Main11왕복/Save;일반8조우전승리미완료|
|O Silent Warden|PASS|기존SilentWarden/Main11 실제승리·패배·보스후목표·보상/저장/norespawn 이력·현재AI/참조보존;별도이번보스전반복없음|
|P Chapter2|PASS|Main15/16 Runtime·Voice·Audio 이력재사용(코드/원본SHA보존);Hearing 실제Path 우선분기/Default수렴;13–15retrofit후보는미구현|
|Q Audio/BGM|PASS|정식Scene/Boss BGM참조·설정/Mixer/Loop/Save기존QA·신규Intro/마을/Field/Battle 실제녹화신호;Chapter2Battle TBD 유지|
|R Story Voice|PASS|Intro18/Story229 Clip 누락0/ID중복0·기존본문/화자/Next/Scene기술QA;실제사람청취품질미검증별도|
|S PlayerSprite50|PASS|신규50/50 Mapping·800Sprite·대표남녀Preview/World/Continue/BattleFrame4;READY50/BLOCKED0·Art재검사없음|
|T UI/Resolution|NOT_VERIFIED|신규1920×1080 Title/생성/Dialogue/Battle실제영상시각확인·Title경계수정별도검증;Party/Pet/Settings포함전체다중해상도시각QA미완료|
|U Compile/Console/References|PASS|현재컴파일/Console 및활성Serialized 참조검사;Renderer2D Missing6최소복원·원본/사용자변경보존|

최근 PlayerSprite 정본은 READY50/BLOCKED0 및 [low-alpha정책](../11_UI/Player_Sprite_QA_Policy.md)이다. Main16 Runtime507PASS/Audio217PASS, Main15/Voice/Audio 문서의 범위와 미검증 사항을 보존한다. Main13–15 Path반응형 retrofit는 설계후보이며 현재 모든분기를 구현했다고 표시하지 않는다. Main01/02 일부NPC 대사에는 음성이 없는 정식상태이므로 녹화를 위해 Voice를 새로 생성하지 않는다.

## 검증/녹화 준비

회귀는 백그라운드·비대화형 우선. CleanBootstrap/설정 스냅샷·격리Save/Settings를 사용하고 종료시복원한다. RunBatch 중 Editor종료/Scene교체 메서드는 그대로 호출하지 않고 실제Audit 내부/기존Harness를 범위에 맞게 사용한다. 모든 FAIL을 게임Bug/Fixture오류로 분류하며 심각한 영상노출Bug가남으면녹화하지않는다.

준비 당시 UnityRecorder 미설치였으며 기존 ffmpeg/ffprobe를 확인했다. 공식 Recorder5.1.7을 임시사용해 순수 GameView+GameAudio1920×1080/30fps를 기록했다. 완료 후 임시 Package/manifest/lock/RecordingAsset를 정리했고 미커밋이다. 사용자Desktop/EditorUI/Console/개인정보 녹화없음. 리허설과 최종파일의 신호/실제 프레임을 검사했다.

## 녹화 계획

사용자가이번작업의리허설/실제녹화/GameView/PlayMode/필요한Unity포커스전환을명시승인했다. 이범위외Foreground회귀/다른앱조작은하지않는다. Output `F:/Downloads/Limitless_DevJourney_2026-10-05/Limitless_DevJourney_EarlyStory_2026-10-05.mp4`(5–8분목표). Title10–15초→완결된Intro30–60초→성별/Path/Job Preview2–3조합45–75초→초반Town/NPC/Story1–2분→이동/Field전환30–60초→일반Battle/Skill/승리복귀1–2분. 실제정식흐름을사용하며QAOverlay/테스트버튼없이녹화한다. Main01무음대사는기존상태로보이고Intro등기존Voice로음성경험을보여준다. 자연스러운짧은구간을우선한다.

리허설후Resolution/FPS/Duration/AudioTrack/신호/실제GameFrame/시작끝을검사한다. 추가Teaser는주영상완료후여유가있을때만. MP4는Git미포함/업로드없음. 녹화결과는이문서또는별도개발기록에저장하고CURRENT_STATUS에는실제게임기능QA만기록한다.

## 실행 근거와 한계

- 추가 Main09 감사에서 오래된 Seed가 현재 Catalog의 Main10–16까지 완료로 표시해 Continue 시 후속 Story/동료 복원과 충돌했다. Main09/Main10 QA의 Seed를 해당 퀘스트 이전 Main만 완료한 유효 상태로 제한하고 재검증한다. Runtime 진행/해금 코드를 변경하지 않는다.
- 실제 화면에서 Title 다시보기 버튼의 하단 Outline이 화면 밖에 약 2px 나가는 것을 발견했다(720 reference의 중심25.2/반높이25/Outline2). 사용자 변경에 없는 BootstrapLoader의 해당 anchor Y만 0.035→0.055로 올려 최소 여백을 확보하고 16:9 3해상도 경계·클릭/Intro 정리를 재검증한 뒤 최종 테이크를 다시 기록한다. 슬롯/Save/기획/다른 UI 구조 변경은 하지 않는다.

- 신규 격리 Play 실행: 50조합 Mapping/Job Preview, 800 Sprite 참조, READY Inventory 일치, 대표 Mobility 남녀의 기본성별 Preview→Confirm→World→Save→실제 Bootstrap Continue→Battle Left Idle/Scene 전달. QuestSystemAudit/EconomyInventoryAudit/EarlyLevelingAudit 규칙·파티 자원/BattleSilenceAudit 본문 통과. RunBatch의 Editor 종료 경로는 호출하지 않았다. 최종 전체 wrapper 체크244 PASS/0 FAIL. Main09 내부115 PASS, Main10 내부63 PASS는 wrapper 요약과 중복되므로 합산하지 않는다. [보존 로그](LIMITLESS_FullRegression_Runtime_2026_10_05.txt)를 따른다.
- 신규 정식 데이터 조회: Main01–16 총16 Quest/103 Objective, Objective ID 중복0. 5직업×3스킬=15. Intro18/Story229 Voice Clip 참조 누락0·Catalog ID 중복0.
- 이력 재사용: [Main15 Runtime QA](../03_스토리/Main15_Runtime_QA_2026_09_30.md), [Main16 Runtime507PASS](../03_스토리/Main16_Runtime_QA_2026_10_03.md), [Main16 Audio217PASS](Main16_Audio_마무리_QA.md), [Story Voice](Main16_Voice_Foreground_QA.md), [Audio 설정](Audio_Volume_Runtime_QA_2026_09_30.md), CURRENT_STATUS의 Main01–14/Party/Beast/Dungeon/Boss 실행 기록. 이번 SHA-256 검사에서 해당 Runtime 코드·데이터의 변경이 없음을 확인하는 조건으로 통합한다. 이력의 미검증 항목은 이번에도 자동 PASS로 바꾸지 않는다.
- 확정하지 않는 항목: 스킬15개 전부의 실제 Battle UI 실행, B1/B2 일반조우8개 전부의 실제 승리, 주요 모든 화면의 3해상도 시각 QA. 기존 스킬/던전 QA의 미검증 범위를 그대로 남긴다. 모든 물리 입력장치·청취 품질·환경 미관도 별도 검수다.
- 최초 침묵 명령 감사 실패는 Fixture가 Battle Awake의 입장 연출 상태를 해제하지 않은 원인이었다. `BattleSilenceAudit`에 연출 종료 상태를 명시한 뒤 통과. 게임 기능 수정0. 순수 감사의 세션 Reset으로 유효 이름이 없는 테스트 자동 저장 경고가 발생했으며 사용자 Save는 격리되어 보호됐다.
- 녹화 도구의 가상 키 Event 전달 실패는 게임 Bug로 계산하지 않는다. 최종 자동 이동은 임시 방향 드라이버가 기존 PlayerController의 이동 값과 FixedUpdate 물리 경계를 구동했다. 물리 키보드/InputAction 장치 입력을 통과했다고 주장하지 않는다. 좌표/Quest/HP/적 상태 주입 없이 녹화한다. 실패 테이크는 최종으로 사용하지 않는다.
- YAML 정적 검사313파일의 현재 Assets GUID 중복0. 구형 Renderer2D YAML에 해결되지 않는 GUID7개가 있으며 현재 Serializer의 제거된 probe debug/falloff 필드 여부를 대조한다. 미사용 이전 필드를 게임 Missing Sprite/Material 오류로 오인하여 사용자 설정을 수정하지 않는다.
- 추가 조사: 현재 Serializer의 `probeVolumeResources`에 실제 Missing 참조6개가 남는다(Shader4/Mesh1/Texture1). Renderer2D는 작업 전 사용자 변경 목록에 없으며 원본 패키지는 수정하지 않는다. 설치된 Core 패키지의 공식 동명 리소스 GUID/fileID를 Editor로 조회했다. 설정 Asset의 해당 GUID6개만 정상 재연결했고 SerializedObject의 활성 Missing0과 실제 녹화/Console0을 재검증했다(수정commit `1e0a957`). 현재 클래스에서 제거된 `m_FallOffLookup`의 옛 YAML1행은 비활성 이력으로 별도 기록하며 임의 Renderer 구조 변경은 하지 않는다.

## 최종 범주 집계

**21범주: PASS18 / FAIL0 / NOT_VERIFIED3 / NOT_APPLICABLE0.** J(15스킬 실제UI전체), N(B1/B2 일반8조우전승리), T(주요모든UI 다중해상도시각)의 남은 범위를 PASS로 승격하지 않았다. PASS18도 위 표에 명시한 기술 범위/보존된 과거 실행의 통합 판정이다. 사람 청취, 물리 입력장치, 배포 Player 저장, 모든 World/카메라 미관은 전체 PASS를 뜻하지 않는다.

Main09/10 추가 Fixture 보정: 미래Quest 완료 Seed 제외, Main10이 재사용하는 기존 석재 허용, 구현된 Dungeon 진입을 옛 무음 안내 Hook 대신 실제검증. 모두 Editor QA만 변경했으며 Game의 Quest/Save/Party/Battle 기능 변경0. 규칙 감사의 슬롯미선택 자동저장 생략 Warning은 격리검사 경계로 기록하고 게임 FAIL과 구분했다.

신규 이동/녹화는 기존 Runtime 명령의 자동 조작이며 테스트용 HP·진행·적 상태 조작0. 리허설패배/Recorder입력실패는 최종 테이크에서 제외. 게임 설계나 스탯을 변경해 승리를 만들지 않았다. Title 하단 버튼 Outline의 경미한 화면이탈은 anchor만 최소수정했다. 별도 녹화 기록은 [개발기록](LIMITLESS_DevJourney_Recording_2026_10_05.md)을 따른다.

## 최종 무결성·복원

- Title 최소수정은 `80d2f0b`. 실제1920×1080/1600×900/1280×720에서 버튼/Outline 경계, Replay Intro Voice, 연속Next8, Skip 후 Source0의 **11 PASS**를 Recorder 제거 후 다시 확인했다. 전체/Fixture QA commit `deb181c`.
- Editor Serialized 검사 **12242 객체/31 Scene**, Missing Script0. 현재 Runtime 활성 Missing Sprite/Audio/Material/기타 참조0. 활성 Build Scene19의 Scene 참조 누락0. 과거 `Assets/_Project/Backup/Milestone01/20260810_*` Scene5개의 Main Camera target만 누락이며 Build/현재 실행 경로 밖이므로 수정하지 않았다.
- YAML313파일 Assets GUID 중복0. 남은 정적 GUID1개는 Renderer2D의 현재 Serializer에 없는 옛 `m_FallOffLookup` 행이다. 활성 Missing으로 집계하지 않고 원본 이력으로 보존한다. 최초 활성 probeVolumeResources Missing6개는 정상복원 완료.
- 원본 SHA-256 대비 Assets/ProjectSettings/UserData/Packages의 변경은 **Renderer2D 설정1 + BootstrapLoader의 UI anchor1 + Editor QA4개**뿐이다. PNG·Audio·Catalog·meta·Scene·사용자Save/Settings·ProjectSettings·Packages 불변, baseline 파일 누락0. 사용자410 Git항목(기존94그룹)을 보존했다.
- 컴파일Error0, 최종ConsoleError0/Warning0. Recorder 제거 재컴파일에서 기존 ExternalAssetImportEditor의 deprecated TextureImporter.spritesheet CS0618 경고2건이 관찰됐고 그대로 보존했다. 새 QA의 deprecated 정렬 호출은 현재 non-sort overload로 바꾸고 Title11PASS를 재실행했다. 마지막 Play의 Unity 기존 Console clearing 동작 뒤 Error/Warning0을 재조회했으며 경고 이력을 감추지 않는다.
- 임시 녹화 스크립트2개/meta2개 제거·Recorder 의존성 제거·manifest/lock 작업전 바이트복원/resolve 완료. clean Bootstrap Edit Mode, Scene dirty=false, compiling=false, AuditSave 경로 비움, runInBackground=false/Play 옵션·Game View 진입 상태 복원. QA에서 창 Focus·OS 입력·Desktop 조작 없음.
- 직접변경만 Stage/로컬Commit, diff--check 통과. MP4/ZIP/임시RecordingAsset/Package/unrelated Stage 없음. GitHub Push 없음.
