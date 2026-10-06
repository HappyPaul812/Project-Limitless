# Main17 / Chapter2 Battle Audio Handoff

상태: **Chapter2BattleMusic = AUDIO_WAITING_EXTERNAL**. 현재 BgmSceneCatalog.FindBattle의 Chapter2 기본 return null을 유지한다. 외부 담당이 곡을 선정하기 전 Chapter1 곡이나 가짜 Audio를 배정하지 않는다. TTS_PENDING14·Voice WAV는 별도이며 이 작업에서는 변경하지 않는다.

## 현재 생산 코드 계약

BgmPlaybackService는 DontDestroyOnLoad 단일 AudioSource다. playOnAwake=false, spatialBlend=0, volume=1, loop=true. AudioSettingsService.Route(source, GameAudioChannel.BGM)는 Resources/Audio/LimitlessAudioMixer의 이름 BGM Group에 연결한다. 사용자 BGMVolume과 MasterVolume/MuteAll은 Mixer에서만 적용하며 Source.volume으로 중복 적용하지 않는다.

| 시점 | 호출/요구 Cue | 현재 출력 | 외부 선정 후 연결 경계 |
| --- | --- | --- | --- |
| Field08 진입/Continue | sceneLoaded → ApplyScene → catalog.Find(Field_08_RedRift) | Beneath_The_Cracked_Earth | 현재 탐색곡 유지 |
| Story Battle | Controller.Start → PlayBattleMusic(false), origin Field_08_RedRift / ID field08_main17_threat | null, 기존곡 Stop | Chapter2 Story Battle용 선정곡 |
| 일반 Battle | 같은 호출, spawn Field08_Beetle01 / 별도 StoryID 없음 | null | Chapter2 일반전 선정곡 |
| Victory 결과 화면 | Battle Scene 유지, 별도 Victory BGM Cue 없음 | 현재 Battle곡 유지(지금 null) | 승리 Jingle은 별도 미구현/미확정, 임의 추가하지 않음 |
| VictoryReturn/일반 복귀 | BattleSceneFlow.ReturnToField → Field08 sceneLoaded | 탐색곡 복귀 | Battle Source Stop 후 Field08곡 Play |
| Field07 왕복 | ApplyScene 각 Scene | Field07 현재 배정 계약 / Field08 탐색곡 | 동일 Source 사용 |

**Fade는 현재 없다.** Play(clip)은 source.Stop → loop=true → clip교체 → Play로 즉시 전환하며 같은 clip이 재생 중이면 재시작하지 않는다. Fade duration을 존재하는 기능처럼 기록하지 않는다. 외부 선정곡은 이 즉시 전환 경계에서 클릭/잔향·루프 적합성을 사람이 청취해야 한다. 별도 Fade 변경은 이후 요청 범위다.

선정 담당 요구: 실제 Audio 파일과 곡 식별/출처, loop가 가능한 시작/끝, Chapter2 일반/Story 공유 여부. 현재 Boss는 isBoss=true에서 null을 반환하며 Main18/Boss곡을 이번에 만들지 않는다. 선정 파일을 Assets/_Project/Resources/Audio/Music/에 가져온 후 Catalog의 Chapter2 배정 분기를 최소 연결한다. 세부 import sample rate/compression/루프 편집은 실제 파일 도착 후 기록하며 가상 사양을 확정하지 않는다.

Field08 실제 탐색 파일은 Resources/Audio/Music/Beneath_The_Cracked_Earth.mp3(176.48초). 기존 Phase8/9의 진입·전투·복귀·Continue·왕복·단일 Source·Loop·Mixer PASS를 재사용한다. 사람의 음악적 적합성은 **USER_LISTENING_REQUIRED**다. 실물 키보드/게임패드/접촉은 USER_INPUT_REQUIRED이며 음악 청취와 구분한다.
