# Main20 Audio / BGM Handoff

정본: [Main20 설계](../03_스토리/Chapter2_Main20_심부의_거신.md). **AUDIO_SOURCE_AUDITED / IMPORT_PENDING / USER_LISTENING_REQUIRED**. 이번 원본 가공·MP3 복사·Unity Import·Catalog/Service 변경0.

## 배정

| 역할 | 정식 곡 / 원본 |
| --- | --- |
| Field11 탐색·전투 복귀 | Paths of Cracked Earth, 기존 `Unity/Client/Assets/_Project/Audio/Music/Paths_of_Cracked_Earth.mp3` 재사용 |
| Boss Encounter field11_main20_veinfire_colossus | The Weight of Crowns, `F:\Downloads\bgm\The_Weight_of_Crowns.mp3` |
| Iron and Incantation | Main20 미사용, 미래 마법형 강적/중간보스 후보 유지 |

## 원본 읽기 전용 감사 — 2026-10-09

| 항목 | 실제 결과 |
| --- | --- |
| 존재 / 절대 경로 | 존재 / F:\Downloads\bgm\The_Weight_of_Crowns.mp3 |
| 파일 크기 | 4,273,649 bytes |
| SHA256 | 4f91dc087a57927a354dd28de5cc0fe2eddf47bf3c1103200bbe72d46260a9fc |
| codec | MP3 |
| duration | 177.815458초（약2분57.82초） |
| sample rate / channels | 44,100Hz / 2 / stereo |
| audio stream bitrate | 192,000bps |
| container bitrate | 192,273bps |
| 전체 decode | ffmpeg -nostdin → null 출력, exit0·error0 |
| Unity 동일 이름 검색 | Crowns/crowns 파일 없음 |
| Unity 동일 byte 검색 | Assets의 MP3/WAV/OGG/FLAC/AIF/AIFF/M4A286개 SHA256 대조, 동일0 |

ffprobe/ffmpeg는 로컬 `E:\Program Files\anaconda3\Library\bin\` 실행 파일을 사용했다. 재생·포커스 전환·출력 파일 생성 없이 전체 디코딩했다. 파일 존재와 decode 성공은 사용자 음악 청취 승인을 뜻하지 않는다. Import 직전 같은 SHA를 다시 확인하고 승인 원본 바이트 그대로 가져온다. trim/normalize/gain/pitch/속도/재인코딩·원본 loop 편집은 금지한다.

## 후속 Runtime 계약

`BgmPlaybackService`의 persistent AudioSource1 / DontDestroyOnLoad / 2D / BGM Mixer 라우팅을 유지한다. 사용자 음량은 Mixer에만, Source.volume은 fade gain에만 적용해 이중 곱을 피한다. LoopON·Time.unscaledDeltaTime·새 요청/Scene/Continue/Disable의 fade 취소를 유지한다.

Battle 진입: 실제 참가자 준비 후 `PlayBattleMusic(isBoss)` → Field곡 Stop/교체 →0.20초 FadeIn. 승리 결과 화면 동안 Boss곡 유지. 필드 복귀 시0.75초 FadeOut→Stop/clip교체→Paths of Cracked Earth0.9초 FadeIn. 한 Source로 순차 전환하므로 곡이 겹치지 않는다. 패배/재도전·Continue에서도 원래 Scene/Encounter의 배정을 사용한다. 추가 Boss Source·Phase별 새곡·임의 시간 전환은 만들지 않는다.

**현재 연결 공백:** `BgmSceneCatalog.FindBattle`은 파수꾼 Encounter 전용곡을 먼저 판정하고 그 외 isBoss=true에 null을 반환한다. Main20 전용 Encounter ID→Crowns clip의 명시 매핑을 이 null 판정보다 앞에 추가하고 Field11 Scene→Paths를 등록해야 한다. IsBoss만으로 모든 Boss에 Crowns를 강제하지 않는다. 기존 파수꾼/Chapter1/2 배정은 보존한다. Service의 공용 Source/Fade/Mixer는 재사용하며 Catalog 필드/매핑만 소규모 확장 대상으로 기록한다.

Import는 기존 Chapter2 BGM 선례인 Streaming / Preserve sample rate / Vorbis quality1 / preload false / stereo를 기준으로 별도 구현 작업에서 설정한다. 이는 Unity Import 설정이며 원본 MP3 재인코딩 요청이 아니다. 실제 loop 경계·fade 청취는 **USER_LISTENING_REQUIRED**. 이 단계에서는 SFX 신규 제작/수치도 확정하지 않는다. TTS는 [Main20 Manifest](Main20_TTS_Manifest.csv)와 기존 quota 정책을 따른다.
