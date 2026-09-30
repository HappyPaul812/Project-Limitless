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

최소 BGM 서비스는 DontDestroyOnLoad 객체1개/AudioSource1개로 재생한다. 새 곡 요청은 이전 Clip을 중지/교체하고 같은 Clip 재요청은 재시작하지 않는다. Scene 진입 시 위 명시된 역할만 선택하고 미정 Scene은 정리한다. 기본 Loop, 기존 BGM Mixer, Source volume1/pitch1, 기본 Voice100/BGM80, Ducking/Crossfade 없음. AudioListener가 없는 실제 Scene에서는 해당 Scene 카메라에 Listener를 보충한다。Voice와 독립적으로 재생한다.

## 검증 계획

CSV 감사·전체 프레임 디코딩·원본 SHA-256·Unity Import/GUID/Clip 참조를 확인한다. 배경 PlayUnfocused·격리 Settings/Save로 Main13~15 인물별 샘플, Next/연속Next/종료/Voice0/Mute와 기존 Intro/Story 참조 회귀를 검증한다. Title→Town→Field→Town, Arbel/첫 서부 Field, 안전 반경 진입/이탈, 미정 Scene에서 정리, Loop/Source 중복 방지, BGM0에서 Voice/SFX 독립 출력·Mute/복원을 확인한다. Foreground나 Game View 활성화는 하지 않는다.

실제 청취의 취향·감정·발음·호흡·Voice/BGM 균형·Loop 경계·Scene 감정 적합성은 사용자 QA로 남긴다. 전체 Main13~15 Quest playthrough는 이번 검증 목표가 아니다.
