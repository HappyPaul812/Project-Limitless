# LIMITLESS TTS 음성 제작 정책

> 2026-09-29 별도 조사·논의에서 확정한 **개발 방침**을 2026-09-30 기록한다. 법률 자문이나 서비스 조건의 영구 보장이 아니다. 실제 출시 전 최신 Google 약관·모델 제공 조건을 다시 확인하며, 변경된 정책을 우선한다.

## 적용 범위와 기존 자료

이 문서는 앞으로 캐릭터 음성 생성의 Source of Truth다. 기존 [MeloTTS 오프라인 음성 제작](MeloTTS_오프라인_음성_제작.md)은 과거 시험·제작 이력으로 보존한다. 기존 Intro WAV와 연결은 자동 교체하지 않는다. 다음 음성 제작은 현재 LOCAL Intro Story 정본을 그대로 사용하여 Gemini Free Tier로 처음부터 Pipeline을 검증한다. TTS에 맞추어 Story 문구를 임의 변경하지 않는다.

## 모델과 Tier

우선 후보는 **Gemini 3.8 Flash TTS / Gemini 3.8 Flash-Lite TTS**다. 2026-09-29 당시 상업 게임의 사전 생성 음성 Asset 제작과 한국어 제작을 검토할 수 있는 후보로 판단했다. 최종 사용은 출시 전 최신 정책 및 실제 음질 확인을 전제로 한다.

현재 개발 단계의 확정 결정은 **Free Tier로 먼저 시작**이다. 실제 음질, 캐릭터 Voice Design, Intro 제작 Pipeline, Unity 적용 방식을 검증하기 위한 결정이며 Paid Tier를 강제하지 않는다.

같은 모델에서 Free Tier라는 이유만으로 의도적으로 음질을 낮춘 모델이라고 보지는 않는다. 당시 이해한 주요 차이는 사용량·Quota, 처리 우선순위, 기능·제한, Billing 및 데이터 사용 조건이다. 동일 음질·동일 처리 우선순위를 보장한다는 뜻은 아니다.

당시 확인 기준으로 Free/Unpaid 서비스의 입력·출력이 제품 개선에 사용될 수 있는 조건이 있고 Paid 서비스는 데이터 사용 조건이 다르다. 실제 적용은 지역·계정·Billing 연결 등에 따라 달라질 수 있으며 Free Quota와 약관상 Unpaid 서비스가 항상 같은 의미는 아니다. 미공개 Story Script·미출시 Narrative Data를 대량 처리하기 전과 출시 전에 최신 정책을 재확인한다. 현재는 이 차이를 이해한 상태에서 Free Tier부터 사용하기로 했다.

Paid Tier는 개발자 관점에서 Google Cloud Billing 등 결제 계정과 연결해 실제 사용량에 따라 과금되는 운영 Tier다. `Paid = 더 좋은 목소리`로 정의하지 않는다. 가격·Quota·한국어 제공·모델 ID는 실제 제작 시 공식 자료로 재확인한다.

## Voice Design과 장면 연기

같은 캐릭터는 가능한 한 동일 Voice Design / Voice ID를 유지한다. 기본 음색, 성별·연령감, 말하는 질감과 캐릭터 정체성을 장기적으로 유지하려는 원칙이다. 같은 ID만으로 모든 생성 결과가 완전히 같아진다고 가정하지 않고 청취 검수한다.

- **Voice Design**: 캐릭터의 영구적인 목소리 특징.
- **Scene Style / Direction**: 장면별 감정·속도·억양·분위기·말의 세기.

예를 들어 세린의 기본 음색은 유지하면서 평상시·긴장·전투·걱정·약한 농담은 장면 지시로 조절한다. 캐릭터 말투는 기존 Story/등장인물 정본을 따른다.

## Character Voice Registry

아래 구조로 캐릭터별 정본을 관리한다. 미확정 값은 `TBD`, 검증 전 상태는 미검증으로 남기며 Voice ID를 임의로 만들지 않는다. 캐릭터 Stable ID는 LOCAL Character Definition과 대조해 기록한다.

| 항목 | 기록 기준 |
| --- | --- |
| Character Stable ID / Name | 기존 정식 ID / 이름 |
| TTS Model | 실제 사용 모델과 버전, 미확정 TBD |
| Voice Design / Voice ID | 영구 특징 / 실제 제공 ID, 미확정 TBD |
| 기본 Voice 설명 | 음색·질감·성별/연령감, 미확정 TBD |
| 기본 말하기 속도 / Tone | 캐릭터 기준, 미확정 TBD |
| Scene별 Style 지시 | 장면 ID와 연기 지시 |
| 테스트 완료 여부 | 미검증 / 검증 날짜·결과 |
| 최종 채택 여부 | 미채택 / 채택 날짜·근거 |

세린의 `companion_serin`은 기존 ID를 쓰며 모델·Voice Design·Voice ID·기본 속도·음색은 현재 `TBD`, 음성 테스트는 미검증, 최종 채택은 미채택이다. 다른 캐릭터의 음성도 아직 확정하지 않는다.

## 모델별 운용 후보

Flash TTS는 주요 캐릭터·중요 Story Dialogue·Intro·Cutscene, Flash-Lite TTS는 대량 NPC·반복 보조 대사·비용과 대량 생성이 중요한 영역의 **운용 후보**다. 강제 배분 규칙은 아니며 청취 결과에 따라 한 모델로 통일하거나 역할을 조정할 수 있다.

## 사전 생성 Audio Asset Pipeline

`LOCAL Story Script → Voice Design / Voice ID → Scene Style → TTS 생성 → WAV 등 원본 청취·검수 → 프로젝트 Audio 규칙에 맞는 최종 Asset → Unity Import → Dialogue / Cutscene 연결`

게임 실행 중 사용자 PC에서 Gemini API를 호출하는 구조보다 개발 중 음성을 사전 생성해 Audio Asset으로 포함하는 방식을 우선한다. Unity Client에 Google API Key를 직접 포함하지 않는다. 미래 Runtime Cloud TTS는 별도 Server/Security 설계 후 검토한다.

첫 실제 제작 대상은 **LIMITLESS Intro**다. 현재 LOCAL Story 정본은 `Unity/Client/Assets/_Project/Scripts/Core/OpeningIntroSequence.cs`의 실제 대사와 관련 Story 문서를 먼저 대조하고, 기존 `Tools/TTS/opening_narration.csv`는 제작 입력 이력으로 참고한다. 불일치가 있으면 임의 수정하지 않고 보고한다. 새 WAV의 샘플레이트·채널·포맷은 기존 규칙을 확인한 뒤 결정한다.

최초 정책 기록은 문서화만 수행했다. 후속 작업은 사용자가 생성 완료한 Intro 18개 WAV를 원본 그대로 교체·연결한다. 새 음성 생성·SDK·Key·HTTP는 추가하지 않는다.

## 2026-09-30 Intro 적용 계획과 입력 점검

- 입력: `F:/Downloads/Voice/Limitless_TTS_Output/02_intro_flash_Storyteller_4/segments`의 `opening_001.wav`–`opening_018.wav` 18개. 모두 PCM WAV, 24,000Hz, 모노, 16bit, 전체 프레임 디코딩 성공, 합계 83.20초.
- 대응 근거: 인접 외부 `limitless_gemini_tts_v6.py`의 `INTRO_LINES`가 LOCAL CSV 18문장과 정확히 일치하고 무음 분할 결과를 동일 ID로 저장한다. LOCAL Sequence의 18개 VoiceClipId와 Catalog 유지. 마지막 제목은 음성 없음. 실제 발화 경계·문장 청취 일치는 미검증이다.
- Narrator 개발 표시명: **Storyteller_4**. `VOICE_USED.txt`의 display_name은 `Storyteller 4`, model은 `gemini-3.8-flash-tts`, voice_id는 `ko-kr-storyteller-4`. 제작 메타데이터의 기록이며 제공자의 현재 지원 여부 검증은 아니다.
- Narrator 기본 Voice/Scene Style: 차분하고 따뜻한 판타지 이야기꾼, 자연스러운 표준 한국어, 중간보다 느린 속도, 절제된 감정·경이·애수. 신의 목소리가 아닌 창조 신화의 서술자. 테스트: 2026-09-30 Unity Import·전체 자동 재생·전환 검증 완료. 최종 채택: 개발 Intro 적용, 청취 검수 대기.
- Player / Taeon / Miel / Paul / `companion_serin`의 Model·Voice Design·Voice ID·속도·음색은 모두 TBD.
- Asset: 기존 `Assets/_Project/Audio/Voice/Opening`의 WAV를 원본 바이트 그대로 교체하고 GUID 유지. Import 정규화·강제 모노 변환을 끄고 PCM/원본 샘플레이트 사용. 원본 속도·pitch·trim·gain 변경 없음.
- 기존 안정 ID→AudioClip Catalog와 음성 전용 AudioSource 재사용. 현재 Master/BGM/SFX/Voice Mixer 및 볼륨 설정 없음. 독립적인 전체 Voice Volume 기반만 추가하며 설정 화면은 확장하지 않는다.
- Next는 이전 음성을 즉시 중지하고 다음 문장·음성 시작. 자동 진행은 기존 `max(문장 최소 시간, Clip 길이 + 0.5초)` 유지. Skip·정상 종료·외부 Scene 전환/비활성화에서 Coroutine·Source·Clip 정리. null Clip은 자막·기존 시간으로 진행.
- 백그라운드 Play Mode 검증 계획: 전체 자동 진행, 18개 순서·참조·디코딩, 중간/연속 Next, Skip, 외부 전환, 재진입, 복제 Catalog null fallback, Console. 청취 검수와 자동 상태 검증을 구분한다.

적용 결과: 기존 GUID/Catalog를 유지한 원본 18개 교체와 즉시 Next·종료 정리를 `7e60218`에 포함했다. 전체 19블록 자동 진행, 18개 자막/ID/Clip 대응, 중간·연속 Next, Skip→CharacterCreation, Replay 정상 종료→Bootstrap, 재진입 001, null fallback 자동 진행, 외부 Scene 전환 후 음성 정리를 실제 Play Mode에서 확인했다. 청취·분할 발화 경계는 별도 확인이 필요하다. 입력 상세와 Console 분류는 [적용 QA](Intro_Storyteller4_적용_QA.md)를 따른다.

## 정식 Audio Volume 연결 (2026-09-30)

Intro Narration과 향후 캐릭터 대사는 [Audio 설정 정책](Audio_설정_정책.md)의 Voice Mixer Group을 사용한다. Voice/SFX/BGM은 독립적인 사용자 환경 설정이며 전체 음소거에서도 채널값을 보존한다. Intro의 공용 Voice Volume을 Source와 Mixer에서 중복 곱하지 않는다. 기존 18개 TTS 원본·속도·Import는 변경하지 않는다. 이전 Intro 적용 기록의 Mixer 없음/Source Volume 기반은 당시 상태이며 후속 정식 Mixer 작업으로 대체한다.

## 정책 재확인 자료

2026-09-30 공식 문서 열람 참고이며, 2026-09-29 개발 결정을 소급해 영구 보증하는 자료가 아니다.

- [Gemini 3.8 Flash TTS](https://ai.google.dev/gemini-api/docs/models/gemini-3.8-flash-tts)
- [Gemini 3.8 Flash-Lite TTS](https://ai.google.dev/gemini-api/docs/models/gemini-3.8-flash-lite-tts)
- [TTS 생성 가이드](https://ai.google.dev/gemini-api/docs/speech-generation)
- [Gemini API 추가 약관](https://ai.google.dev/gemini-api/terms)
- [Gemini API 가격·Tier](https://ai.google.dev/gemini-api/docs/pricing)

정식 대량 제작·출시 전에 모델 제공 여부, 한국어 품질, Quota/Billing, 입력·출력 데이터 사용 조건과 생성물의 상업 사용 관련 조건을 최신 자료에서 재확인한다.
