# LIMITLESS TTS 음성 제작 정책

> 2026-10-05 Player Dialogue 정식 정책: 고정 Voice/TTS 없음, Portrait 없음. NPC/Narrator/남녀 기본 외형 fallback 금지. 중앙 Presenter/Portrait Resolver에서 이전 음성 정리와 Sprite clear/영역 숨김을 수행한다. 아래 Player TBD는 과거 제작 이력이며 현재 규칙은 NONE이다. Named Character의 기존 Voice Registry는 유지한다. 태온 첫 만남 005/006/007은 사용자 실제 청취 불일치로 재생성 필요 상태이며 이번에 음성을 생성하거나 가공하지 않았다. [전수 감사](Story_Dialogue_Consistency_QA.md)와 [최소 재생성 목록](Story_Dialogue_TTS_REGEN_REQUIRED.csv) 참조.

> 2026-09-29 별도 조사·논의에서 확정한 **개발 방침**을 2026-09-30 기록한다. 법률 자문이나 서비스 조건의 영구 보장이 아니다. 실제 출시 전 최신 Google 약관·모델 제공 조건을 다시 확인하며, 변경된 정책을 우선한다.

## 적용 범위와 기존 자료

이 문서는 앞으로 캐릭터 음성 생성의 Source of Truth다. 기존 [MeloTTS 오프라인 음성 제작](MeloTTS_오프라인_음성_제작.md)은 과거 시험·제작 이력으로 보존한다. 기존 Intro WAV와 연결은 자동 교체하지 않는다. 다음 음성 제작은 현재 LOCAL Intro Story 정본을 그대로 사용하여 Gemini Free Tier로 처음부터 Pipeline을 검증한다. TTS에 맞추어 Story 문구를 임의 변경하지 않는다.

## 모델과 Tier

우선 후보는 **Gemini 3.8 Flash TTS / Gemini 3.8 Flash-Lite TTS**다. 2026-09-29 당시 상업 게임의 사전 생성 음성 Asset 제작과 한국어 제작을 검토할 수 있는 후보로 판단했다. 최종 사용은 출시 전 최신 정책 및 실제 음질 확인을 전제로 한다.

현재 개발 단계의 확정 결정은 **Free Tier로 먼저 시작**이다. 실제 음질, 캐릭터 Voice Design, Intro 제작 Pipeline, Unity 적용 방식을 검증하기 위한 결정이며 Paid Tier를 강제하지 않는다.

같은 모델에서 Free Tier라는 이유만으로 의도적으로 음질을 낮춘 모델이라고 보지는 않는다. 당시 이해한 주요 차이는 사용량·Quota, 처리 우선순위, 기능·제한, Billing 및 데이터 사용 조건이다. 동일 음질·동일 처리 우선순위를 보장한다는 뜻은 아니다.

당시 확인 기준으로 Free/Unpaid 서비스의 입력·출력이 제품 개선에 사용될 수 있는 조건이 있고 Paid 서비스는 데이터 사용 조건이 다르다. 실제 적용은 지역·계정·Billing 연결 등에 따라 달라질 수 있으며 Free Quota와 약관상 Unpaid 서비스가 항상 같은 의미는 아니다. 미공개 Story Script·미출시 Narrative Data를 대량 처리하기 전과 출시 전에 최신 정책을 재확인한다. 현재는 이 차이를 이해한 상태에서 Free Tier부터 사용하기로 했다.

Paid Tier는 개발자 관점에서 Google Cloud Billing 등 결제 계정과 연결해 실제 사용량에 따라 과금되는 운영 Tier다. `Paid = 더 좋은 목소리`로 정의하지 않는다. 가격·Quota·한국어 제공·모델 ID는 실제 제작 시 공식 자료로 재확인한다.

## Free Tier Quota 운영 규칙

> **TTS quota는 API Key 개수가 아니라 Project + Model의 실제 quota 상태를 기준으로 관리한다. Quota 실패 시 성공 결과를 보존하고 checkpoint에서 남은 StoryOrder만 재개한다.**

- 일일 제한을 Key 하나당 10회로 가정하거나 `Key1 10 + Key2 10 = 무조건 20`으로 계산하지 않는다. `GenerateRequestsPerDayPerProjectPerModel-FreeTier`는 PerDay / PerProject / PerModel을 명시하므로 같은 Google Project의 여러 Key는 해당 Project + Model quota를 공유할 수 있다. Key 교체가 새 호출 예산을 보장하지 않는다.
- 제작 전 Model, Google Project, 각 API Key의 소속 Project, 해당 Project/Model의 실제 남은 quota, 기존 당일 제작 이력과 checkpoint를 확인한다. Key가 여러 개면 같은 Project인지 먼저 확인한다. API Key 값이나 Project Secret은 문서·로그에 기록하지 않는다.
- TTS Handoff에는 **가능 최대 호출 수**와 **이번 작업에 안전하게 사용할 호출 수**를 구분한다. quota가 불확실하면 20회 사용 가능으로 선결정하지 않는다. 한국 날짜 변경, Key 변경, 전날 사용량만으로 초기화·추가 예산을 확정하지 않는다.
- API의 quota 오류와 `retryDelay`를 현재 상태의 최우선 근거로 사용한다. retryDelay를 영구적인 reset clock이나 자동 재시도 허가로 해석하지 않는다. 정확한 reset 시각은 공식 자료와 실제 계정 상태 재확인 전 확정하지 않는다.
- quota 실패 시 **즉시 중단**한다. 자동 retry, 다른 Dialogue를 앞으로 당겨 호출, Key 무작정 교체, 성공 WAV 재생성, checkpoint 삭제를 금지한다. 성공 목록, 실패 Dialogue, 미호출 Dialogue와 총 API 시도 수(실패 포함)를 기록한다.
- 재개 전 checkpoint와 실제 WAV의 ID·존재·hash를 대조하고 남은 Dialogue만 기존 StoryOrder로 생성한다. 기존 성공 WAV는 재생성하지 않는다. 실패 번호 때문에 뒤 Dialogue를 당기거나 재번호화하지 않는다. 불일치는 임의 복구·호출하지 말고 보고한다.
- **1 Stable Dialogue ID = 1 API Call = 1 WAV**를 유지한다. 여러 대사 합본과 자동 retry는 금지한다. 실패 대상의 후속 수동 재개는 별도 호출로 이력에 기록한다.
- 전체 제작과 사용자 직접 청취 승인이 끝나기 전 Unity 적용을 금지한다. 승인을 받은 뒤 별도 Codex 작업으로 원본을 적용한다.

실제 사례·증거의 한계와 정확한 재개 목록은 [TTS Free Tier Quota 운영 기록](TTS_FreeTier_Quota_운영_기록.md)을 따른다. 기존 합본·분할 제작 기록은 과거 이력이며 향후 제작에는 위 단일 Dialogue 호출 규칙을 적용한다.

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

## Main01~12 주요 인물 Story Voice 적용 (2026-09-30)

후속 Registry는 [Story Voice 적용 QA](Story_Voice_Main01_12_QA.md)를 따른다. 태온 companion_taeon은 Gacrux, 미엘 companion_miel은 Sulafat, 폴 companion_paul은 Achird로 개발 적용했다. 제공 CASTING/CSV의 실제 Voice ID이며 모델·기본 음색·속도·Tone·Batch는 자료가 없어 TBD다. 앞선 Intro 시점의 세 인물 TBD 기록을 이 후속 Registry로 대체한다. Narrator는 Storyteller_4 / ko-kr-storyteller-4 / gemini-3.8-flash-tts와 Intro 18개를 유지한다. Player·세린은 TBD·미적용이다.

LOCAL 화자와 본문이 정확히 일치한 102개만 연결한다. Main12로 표시된 2개는 실제 Main14이므로 제외한다. Main01~05·Main08·실제 Main12는 제공 음성이 없어 텍스트 진행을 유지한다. VoiceClipCatalog의 선택적 Character ID 확인과 기존 VoicePlaybackSource/Voice Mixer를 사용한다. 음성은 선택 사항이며 수동 Next·슬롯 저장 구조는 유지한다. 실제 청취 검수는 남아 있다.

## Main13~15 Voice Registry 확장 (2026-09-30)

[Main13~15 Voice/BGM 적용 QA](Main13_15_Voice_BGM_QA.md)의 `dialogue_manifest_main13_15.csv`를 최우선 Mapping 기준으로 쓴다. LOCAL ID·화자·본문과 일치하는41개만 기존 StoryVoiceCatalog에 추가한다. Main13 8개/Main14 11개/Main15 22개이며 Main16 이후는 미적용이다.

이번 CSV/CASTING의 실제 모델은 `gemini-3.8-flash-tts`, style_language는 ko-KR다. 태온 companion_taeon/Gacrux6개·미엘 companion_miel/Sulafat4개·폴 companion_paul/Achird11개·세린 companion_serin/Schedar15개·레온 arbel-leon/Orus5개다. CASTING의 voice_design=false와 README의 Voice Design 사용 안 함을 그대로 기록한다. 별도 Voice Design을 만들었다고 해석하지 않는다. 기본 음색·속도·Tone·Batch는 TBD, 개발 적용 채택·청취 미검증이다. 이 후속 Registry가 이전 세린 TBD·미적용 상태를 확장한다. 이전 Main01~12 배치의 모델 정보에 이번 배치 정보를 소급하지 않는다. Narrator/Intro18개와 기존 Story102개는 유지한다.

## Path 분기 제작 순서

향후 제작은 [Path 반응형 Story 정본](../03_스토리/Path_반응형_Story_연출_규칙.md)의 Story/Path Branch/서로 다른 Dialogue ID 확정 후 Manifest·Voice 제작 순서를 따른다. Main13~15 Retrofit은 설계 후보이며 이번에 기존 Voice를 바꾸지 않는다. [Main16](../03_스토리/Chapter2_Main16_재_속의_형상.md)은 설계만 완료했고 Voice 제작·연결은 미구현이다.

## 정책 재확인 자료

2026-09-30 공식 문서 열람 참고이며, 2026-09-29 개발 결정을 소급해 영구 보증하는 자료가 아니다.

- [Gemini 3.8 Flash TTS](https://ai.google.dev/gemini-api/docs/models/gemini-3.8-flash-tts)
- [Gemini 3.8 Flash-Lite TTS](https://ai.google.dev/gemini-api/docs/models/gemini-3.8-flash-lite-tts)
- [TTS 생성 가이드](https://ai.google.dev/gemini-api/docs/speech-generation)
- [Gemini API 추가 약관](https://ai.google.dev/gemini-api/terms)
- [Gemini API 가격·Tier](https://ai.google.dev/gemini-api/docs/pricing)

정식 대량 제작·출시 전에 모델 제공 여부, 한국어 품질, Quota/Billing, 입력·출력 데이터 사용 조건과 생성물의 상업 사용 관련 조건을 최신 자료에서 재확인한다.

## Missing Story Voice Supplement Pack (2026-09-30)

[보충팩 정책 및 QA](Missing_Story_Voice_Supplement_QA.md)를 따른다. Existing Voice Wins 원칙으로 기존143개/Intro18개를 보존하고 정식 Manifest와 LOCAL 화자/본문이 일치하는 기존 무음 대사에만 신규 ID와 Clip을 추가한다. Main03/04/05/08/12의58개를 적용했으며 Main01/02는 입력에 없다. 사용자 허용으로 일치하는 기존 무음 대사에 Manifest 안정 ID를 지정한다. Catalog 재생성/기존 Clip 교체/원본 가공은 금지한다. 적용 후 Story Catalog는 기존143+신규58=201개다. 기존 매핑 삭제/Clip 교체/GUID·Character 변경0을 확인했다. Coverage와 Background Runtime 결과는 QA 문서를 따른다. 실제 청취 QA는 미완료다.

## Main16 Voice Registry 확장 (2026-10-03)

정식 `dialogue_manifest_main16.csv`의 LOCAL ID·본문·화자가 일치한 segment28개를 원본 WAV 그대로 연결했다. 기존 Story201개에 추가해229개이며, 이전 매핑을 바꾸지 않는다. 위 Main16 미적용 기록은 당시 상태이고 현재 상태는 [Main16 QA](Main16_Voice_Foreground_QA.md)를 따른다.

제공 자료의 모델은 gemini-3.8-flash-tts, ko-KR, voice_design=false다. 레온 arbel-leon/Orus3개, 폴 companion_paul/Achird5개, 세린 companion_serin/Schedar10개, 태온 companion_taeon/Gacrux8개, 미엘 companion_miel/Sulafat2개다. 기본 음색·속도·Tone·Batch는 TBD이며 이전 배치에 소급하지 않는다. Hearing 전용5개·Default 전용6개·공통17개는 별도 Stable ID를 유지한다. Player9개·관찰7개는 정상 무음이다. 합본5개는 Unity에 연결하지 않는다. 청취 품질은 미검증이며 TTS를 재생성하지 않았다.
