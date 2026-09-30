# Main13~15 Voice와 정식 BGM 적용 — 2026-09-30

## 확정 정책과 조사

Voice Source는 `Limitless_TTS_Output_Main13_15/dialogue_manifest_main13_15.csv`다. 41행을 LOCAL 화자/본문/분기에 정확히 대조했고 41행 대응, 중복·누락·본문/화자 불일치0이다. WAV46개 중 개별 대사41개만 Import한다. 캐릭터별 연결본5개는 제외한다. Main13 8개/Main14 11개/Main15 22개, 세린15/폴11/태온6/레온5/미엘4개다. 모델은 CSV/CASTING의 `gemini-3.8-flash-tts`, style_language ko-KR다. 이번 CASTING은 `voice_design:false`, README도 Voice Design 사용 안 함으로 기록하므로 제공 Voice ID와 새 Voice Design을 구분한다. Tone·기본 속도·Batch는 TBD다.

| Character Stable ID | 이름 | 실제 Voice ID |
| --- | --- | --- |
| companion_taeon | 태온 | Gacrux |
| companion_miel | 미엘 | Sulafat |
| companion_paul | 폴 | Achird |
| companion_serin | 세린 | Schedar |
| arbel-leon | 레온 | Orus |

기존 Main01~12 Registry/음성102개 및 Intro18개를 보존한다. 이전 출력의 Main12로 잘못 분류된2개 대신 이번 CSV의 올바른 Main14 ID/파일을 사용한다. Main16 이후는 적용하지 않는다. 선택적 DialogueLine ID→기존 StoryVoiceCatalog→VoicePlaybackSource/Voice Mixer를 확장한다. Chapter2 조사에서 실제 폴 대사가 빈 Speaker ID를 쓰던5종만 올바른 companion_paul과 CSV ID를 연결한다. Main12는 유지한다. 본문·수동 Next·Save는 변경하지 않는다.

BGM 원본은 F:/Downloads의 `Before_the_First_Light.mp3`, `Morning_at_the_Gate.mp3`, `Morning_Over_the_Ridge.mp3`로 각각 한 파일이다. 제목의 공백 대신 밑줄이 있는 실제 파일을 확인했다. 원본을 바이트 그대로 `Assets/_Project/Audio/Music/`로 복사한다. MP3 가공·Normalize·Crop·Fade 굽기·속도 변경은 없다.

| 곡 | 확정 적용 |
| --- | --- |
| Before the First Light | Bootstrap 타이틀 |
| Morning at the Gate | World_StarterVillage, Arbel, Field_03 지하묘지 입구 안전 반경 내부 |
| Morning Over the Ridge | Chapter1 첫 필드 Field_01, Chapter2 첫 서부 필드 Field_04_WesternBorder |

Chapter2 도입부 문서는 Field04를 첫 서부 Field로 명시한다. Arbel→Field04는 첫 서부 테마로 전환한다. Field05·Field06은 건조/열기 전용 음악이 필요한 후속 지역이므로 TBD로 유지한다. Field02/03 안전 반경 밖·Dungeon/Battle/Boss·Intro/캐릭터 생성은 음악 미정으로 정리한다. Field03 전체에 Town 음악을 확대하지 않는다.

최소 BGM 서비스는 DontDestroyOnLoad 객체1개/AudioSource1개로 재생한다. 새 곡 요청은 이전 Clip을 중지/교체하고 같은 Clip 재요청은 재시작하지 않는다. Scene 진입 시 위 명시된 역할만 선택하고 미정 Scene은 정리한다. 기본 Loop, 기존 BGM Mixer, Source volume1/pitch1, 기본 Voice100/BGM80, Ducking/Crossfade 없음. AudioListener가 없는 실제 Scene에서는 해당 Scene 카메라 또는 Scene 수명의 Listener를 보충한다. Voice와 독립적으로 재생한다.

## 검증 계획

CSV 감사·전체 프레임 디코딩·원본 SHA-256·Unity Import/GUID/Clip 참조를 확인한다. 배경 PlayUnfocused·격리 Settings/Save로 Main13~15 인물별 샘플, Next/연속Next/종료/Voice0/Mute와 기존 Intro/Story 참조 회귀를 검증한다. Title→Town→Field→Town, Arbel/첫 서부 Field, 안전 반경 진입/이탈, 미정 Scene에서 정리, Loop/Source 중복 방지, BGM0에서 Voice/SFX 독립 출력·Mute/복원을 확인한다. Foreground나 Game View 활성화는 하지 않는다.

실제 청취의 취향·감정·발음·호흡·Voice/BGM 균형·Loop 경계·Scene 감정 적합성은 사용자 QA로 남긴다. 전체 Main13~15 Quest playthrough는 이번 검증 목표가 아니다.

## 적용 결과와 자동 검증

계획 `5f3dca9`, Voice 기능 `6818b42`, BGM 기능 `ea97a2d`다. 관련 파일만 커밋하며 제작 출력 전체·F:/Downloads 원본·기존 사용자 변경은 스테이징하지 않았다. GitHub push 없음.

Voice 입력46 WAV는 PCM24kHz/mono/16bit, 합계506.80초다. Segment41개253.40초를 Import하고 연결본5개253.40초는 제외했다. 원본 바이트41개 동일·GUID41개 중복0·PCM/원본 rate/normalize0/forceToMono0. Story Catalog는 기존102개를 유지한143개다. `Tools/TTS/audit_main13_15_voice.py`와 감사 JSON, `verify_main13_15_audio.py`로 원본·참조·LOCAL 대사/화자 보존을 확인한다.

| Main | 작성된 적용 대상 Dialogue | CSV | Mapping | Unmapped | Text mismatch | Speaker mismatch | Missing Audio | Duplicate ID |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 13 | 8 | 8 | 8 | 0 | 0 | 0 | 0 | 0 |
| 14 | 11 | 11 | 11 | 0 | 0 | 0 | 0 | 0 |
| 15 | 22 | 22 | 22 | 0 | 0 | 0 | 0 | 0 |

BGM 원본3개도 바이트 동일하다. Unity Import는 MP3 입력/44.1kHz/stereo/Streaming/원본 rate/강제 모노·Normalize 없음이다. 길이는 Before_the_First_Light176.587753초, Morning_at_the_Gate176.457138초, Morning_Over_the_Ridge179.330612초다. 저장 파일을 재인코딩하지 않았으며 Unity 런타임 Import 변환과 원본 파일 편집을 구분한다.

실제 백그라운드 Play Mode 결과는 `Tools/TTS/main13_15_runtime_qa.json`에 보관한다. 41개 CSV ID/화자/Clip을 같은 프레임 연속 Next로 확인하고 종료 시 Clip을 정리했다. 실제 Main13/14 NPC 분기, Main15 조사 factory10쪽, Chapter2 폴 조사5종의 Line/VoiceId factory 대응을 확인했다. 전체 Quest 완료나 실제 물리 입력 검증은 아니다.

세린/태온/미엘/폴/레온을 BGM0으로 각각 재생해 AudioListener RMS peak0.20236/0.30898/0.19079/0.17817/0.25139를 확인했다. Voice0에서 BGM 출력0.30564, BGM0에서 Voice0.20236 및 임시 SFX 신호0.05023으로 채널 독립성을 확인했다. 세 채널 재생 중 전체 Mute 출력0, 해제 시 BGM0.13046 및 채널72/47/63 보존을 확인했다. 재생 중 BGM Source는 계속 진행했고 Ducking/Voice에 의한 정지는 없다.

최종12회 실제 Scene 진입에서 Bootstrap→Town→Field01→Town→Arbel→Field04→Arbel→Field05→Field06→Dungeon01→Intro→Bootstrap의 곡 선택/정리를 모두 확인했다. 모든 Scene에서 BGM 서비스1개·Listener1개·Loop/BGM Group을 유지했고 세 곡의 실제 출력도 확인했다. 기존 안전 반경 중심(.55,-3.65)으로 QA 배치했을 때 Town 음악, 반경 밖(.55,-.1)으로 배치했을 때 정리를 확인했다. 실제 경로를 수동 이동한 검증은 아니다. 같은 곡 재요청은 timeSamples를 유지하고 중복 서비스 생성 시에도1개를 유지했다.

실제 발견한 Listener 수명 문제를 수정했다. sceneLoaded에서 제거될 이전 Scene Listener가 검색되어 Intro→Title 이후 출력이 없어질 수 있었다. LateUpdate에서 제거 후 Scene Listener를 보충한다. 보충 구현 중 Intro 자체 Camera/Listener 생성보다 앞서 새 Listener를 만들면 중복2개 경고가 발생해, Intro는 자체 Controller에 출력을 맡기도록 제한했다. 최종 Intro→Bootstrap Listener1개/출력·Intro 진입 Listener1개/Voice Next002·BGM 없음 회귀를 통과했다. 이전 경고는 해결된 이력이며 최종 Console Error0/Warning0이다. QA 도우미의 잘못된 PlayerVisualType namespace와 VoiceId switch를 본문으로 잡던 감사 도구를 수정했으며 게임 컴파일 오류와 구분한다.

기존 Story102개와 Intro18개 Catalog 참조 전부, Main09 실제 Next, Intro 실제 Next/Replay Skip/Scene 정리를 회귀 확인했다. 시작 전 사용자 변경·기존 음성/메타·새 제작 원본·MP3 원본 등370개 파일 SHA-256이 모두 동일하다. 격리 Save/Settings 경로를 사용하고 검사 JSON·임시 신호·메모리 객체를 정리했다. 정상 경로·Game View 진입 동작·runInBackground를 복원하고 Bootstrap clean Edit Mode로 종료했다. Foreground/Game View 활성화는 실행하지 않았다. 이번 변경 diff 검사 통과, 남아 있는 기존 사용자 변경은 보존한다.
