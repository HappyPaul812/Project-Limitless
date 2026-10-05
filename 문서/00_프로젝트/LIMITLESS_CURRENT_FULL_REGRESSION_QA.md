# LIMITLESS 현재 전체 회귀검증 계획/결과 — 2026-10-05

LOCAL 프로젝트가 정본이며 GitHub main 조회/Push 없음. 순서: 문서화→검증/녹화 도구 준비→백그라운드 전체 회귀→실제버그 최소수정/재검증→녹화상태 준비→리허설/녹화. 사용자 기존변경/Save/Settings/원본Sprite/Audio/Catalog를 보존한다.

## 상태와 근거

PASS/FAIL/NOT_APPLICABLE/NOT_VERIFIED를 사용한다. 기존 신뢰 가능한 QA는 현재 코드/참조 보존 여부를 확인하여 재사용하고 신규 실행/정적 검사/이력 근거를 명확히 구분한다. 범주 PASS는 기재된 기술 검증 범위를 뜻하며 사람의 청취 품질/모든 입력장치/전체 수동 playthrough까지 뜻하지 않는다. UI와 음성의 미검증 하위항목은 별도로 기록한다.

|범주|상태|이번 검사/재사용 근거|
|---|---|---|
|A Bootstrap/Title|NOT_VERIFIED|현재진입/슬롯/정식Title BGM|
|B Intro|NOT_VERIFIED|18Voice/Next/연속Next/Skip/정리|
|C Character Creation|NOT_VERIFIED|성별/이름/Path5×Job5/추천비제한/Preview/Confirm|
|D Save/Continue|NOT_VERIFIED|격리Save/StableID/구버전호환/정본복원|
|E Main01–Main16|NOT_VERIFIED|StableID/Objectives/시작완료/Story/Save; 최근Main15/16QA통합|
|F World/Scene Transition|NOT_VERIFIED|Scene 참조/Bounds/왕복/안전지역|
|G Party/Companion|NOT_VERIFIED|Player고정/동료2/해금/전후열/Story편성/Save|
|H BeastCompanion|NOT_VERIFIED|사수전용/독립소유/패시브/180%CD3/Save|
|I Battle|NOT_VERIFIED|턴/대상/명령/승리/복귀|
|J Skill15|NOT_VERIFIED|정식데이터/실행/쿨타임/타깃|
|K Status Effects|NOT_VERIFIED|Poison/Burn/Shock/Silence/Guard/Cleanse|
|L Item|NOT_VERIFIED|개별치료/취소/Invalidtarget 비용보존|
|M Defeat/Flee|NOT_VERIFIED|Boss불가/필드유지/안전지역/성장보존|
|N DungeonB1/B2|NOT_VERIFIED|각4Encounter/복귀/승리제거/최근QA|
|O Silent Warden|NOT_VERIFIED|침묵/예고/Guard/소환/방패/승리/norespawn|
|P Chapter2|NOT_VERIFIED|Arbel/Field04–07/Serin/Main12–16/Path정본分기|
|Q Audio/BGM|NOT_VERIFIED|현재정식배정/Mixer/설정/정리/TBD곡보호|
|R Story Voice|NOT_VERIFIED|Catalog/누락/중복/화자/본문/Next/Scene정리; 청취취향별도|
|S PlayerSprite50|NOT_VERIFIED|50/800/400Clip/Mapping/Preview; Art전수검사반복없음|
|T UI/Resolution|NOT_VERIFIED|1920×1080/1600×900/1280×720·주요화면잘림/겹침|
|U Compile/Console/References|NOT_VERIFIED|MissingScript/Sprite/Audio/Material/Scene/StableID/Catalog|

최근 PlayerSprite 정본은 READY50/BLOCKED0 및 [low-alpha정책](../11_UI/Player_Sprite_QA_Policy.md)이다. Main16 Runtime507PASS/Audio217PASS, Main15/Voice/Audio 문서의 범위와 미검증 사항을 보존한다. Main13–15 Path반응형 retrofit는 설계후보이며 현재 모든분기를 구현했다고 표시하지 않는다. Main01/02 일부NPC 대사에는 음성이 없는 정식상태이므로 녹화를 위해 Voice를 새로 생성하지 않는다.

## 검증/녹화 준비

회귀는 백그라운드·비대화형 우선. CleanBootstrap/설정 스냅샷·격리Save/Settings를 사용하고 종료시복원한다. RunBatch 중 Editor종료/Scene교체 메서드는 그대로 호출하지 않고 실제Audit 내부/기존Harness를 범위에 맞게 사용한다. 모든 FAIL을 게임Bug/Fixture오류로 분류하며 심각한 영상노출Bug가남으면녹화하지않는다.

UnityRecorder 현재미설치, 기존 ffmpeg/ffprobe 사용가능. 공식Recorder의 임시설치 가능성을확인하여 순수GameView+GameAudio1920×1080/60fps(불가시30fps)를우선한다. 임시Package/manifest/lock/RecordingAsset는완료후정리·미커밋. 사용자Desktop/EditorUI/Console/개인정보 녹화금지. 도구무음/검은화면은리허설파일로검사한다.

## 녹화 계획

사용자가이번작업의리허설/실제녹화/GameView/PlayMode/필요한Unity포커스전환을명시승인했다. 이범위외Foreground회귀/다른앱조작은하지않는다. Output `F:/Downloads/Limitless_DevJourney_2026-10-05/Limitless_DevJourney_EarlyStory_2026-10-05.mp4`(5–8분목표). Title10–15초→완결된Intro30–60초→성별/Path/Job Preview2–3조합45–75초→초반Town/NPC/Story1–2분→이동/Field전환30–60초→일반Battle/Skill/승리복귀1–2분. 실제정식흐름을사용하며QAOverlay/테스트버튼없이녹화한다. Main01무음대사는기존상태로보이고Intro등기존Voice로음성경험을보여준다. 자연스러운짧은구간을우선한다.

리허설후Resolution/FPS/Duration/AudioTrack/신호/실제GameFrame/시작끝을검사한다. 추가Teaser는주영상완료후여유가있을때만. MP4는Git미포함/업로드없음. 녹화결과는이문서또는별도개발기록에저장하고CURRENT_STATUS에는실제게임기능QA만기록한다.
