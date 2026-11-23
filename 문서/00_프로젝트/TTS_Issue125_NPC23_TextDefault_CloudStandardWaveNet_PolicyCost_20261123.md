# LIMITLESS TTS / 2026-11-23 신규 NPC 무음 기본·Cloud TTS 비용 검토 및 주연 5인 캐스팅 보존

**상태:** `USER_POLICY_ACCEPTED / NPC_TEXT_ONLY_DEFAULT / EXCEPTION_REVIEW_2 / TTS_NOT_GENERATED / API_CALLS_0 / BILLING_UNTOUCHED`

**기준:** [Issue #125](https://github.com/HappyPaul812/Project-Limitless/issues/125) 본문과 [2026-11-23 새 기획 TTS 정책 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7087343324), `docs/planning`의 11/19 [Chapter 3 정식 대사·보상 기준](../03_스토리/Chapter3_정식대사_보상기준_밸런스검증_20261119.md), [101행 무호출 사전검토](TTS_Issue125_Ch3_Main24_29_Side6_20261123_Preflight.md).
**담당 브랜치:** `docs/tts`. 11/23 정책은 이전 101행 CSV의 `voice_call_if_approved=1`이라는 **최대 가정값을 폐기**한다. 기존 CSV는 원본 추출 감사용으로만 KEEP. **신규 실행 기준은 이 문서와 분류 CSV**. 어떤 NPC도 현재 TTS 발주를 승인받은 것이 아니다.

## A. 정책 결론

1. **폴·미엘·세린·태온·하렌만 기존 Gemini 기반 승인 캐스팅 유지:** 폴 `companion_paul/Achird` 15행, 미엘 `companion_miel/Sulafat` 17행, 세린 `companion_serin/Erinome` 15행, 태온 `companion_taeon/Algieba` 12행, 하렌 `story_haren/Alnilam` 13행. **주연 신규 대사 72행에서 VoiceID 변경0, API 호출0.** 72행은 새 원문 후보이지 기존 WAV를 재사용해 문장을 바꿔치기하는 것이 아님. Runtime DialogueID와 사용자 실제 제작 승인 전 TTS HOLD.
2. **기존 승인 음성 162개**(세린67 + 태온85 + 하렌3 + Main21 보존 폴3·미엘2·레온2) 및 기존 Unity StoryVoiceCatalog·GUID/.meta·Save를 **절대 재생성/교체하지 않는다**.
3. **일반 NPC/주민 23행 기본 무음·화면 대사 텍스트 제공.** 아래 2행만 **‘음성이 정말 필요한지 검토할 후보’**이며 자동 음성화 **0건**. 해당 2행도 텍스트/자막·퀘스트 안내·접근성 확인에서 충분하면 계속 무음.
4. 예외가 승인될 때만 기존 Gemini TTS가 아닌 **공식 Google Cloud Text-to-Speech의 `ko-KR-Standard` 또는 `ko-KR-Wavenet`** 후보를 신규 별도 검토. 목소리 성별·실제 SpeakerID·듣기 좋은지·주연과 음색 혼동·상업 게임 사용 조건은 **그때 승인받을 항목**이지 현 문서에서 특정 A/B/C/D를 확정하지 않는다.
5. **Google Cloud 계정/결제 계정 연결·과금·API 활성화·키 조회/생성·Google TTS 호출·gTTS 사용 전부 별도 승인 전 금지**. 정식 Google Translate `gTTS`는 Google Cloud TTS 공식 제품과 다른 비공식 라이브러리이므로 상용 게임 출처로 사용하지 않는다.

## B. NPC 23행 분류 결과

**대사 원문/후보 ID를 그대로 보존한 전체 검토표:** [`TTS_Issue125_NPC23_TextDefault_CloudVoice_Review_20261123.csv`](TTS_Issue125_NPC23_TextDefault_CloudVoice_Review_20261123.csv).

| 분류 | 행 수 | Unicode 문자(공백·문장부호 포함) | 실제 승인 WAV/API |
|---|---:|---:|---:|
| **TEXT_ONLY_DEFAULT** | 21 | **626** | **0 / 0** |
| **CONDITIONAL_REVIEW_ONLY** | 2 | **150** | **0 / 0** |
| **합계** | **23** | **776** | **0 / 0** |

**예외 검토 후보 2행(둘 다 ‘음성 발주 승인’ 아님):**
- `Main24` 마레 `main24_mare_001`, **84자**: “북쪽에서는 한기에 걸리면 두 차례 행동하지 못할 수 있어요. 이 어묵탕 두 그릇을 가져가세요. 행동할 수 있는 동료가 한기에 걸린 사람에게 써 주세요.” — 한기·어묵탕 사용법을 최초 안내하는 교육적/상태이상 정보. Text+UI·접근성 QA에서 이해가 어려운 경우에만 음성 필요성 별도 판단.
- `Main26` 마레 `main26_mare_001`, **66자**: “이번에는 동상에 대비하세요. 해빙 연고 두 개를 준비했습니다. 한기와 동상은 다른 상태예요. 연고는 동상에 써 주세요.” — 동상·연고 사용과 한기 구분 설명. Text/지속 안내가 충분한 경우에는 무음 유지.
- 두 ID 모두 `PROPOSAL_NOT_RUNTIME`이며 **마레의 실제 Runtime CharacterId/VoiceID도 미확정**. 같은 NPC로 취급해 새 음성을 이미 배정한 것이 아니다.

**텍스트만 유지할 나머지 21행:** Main24 루덴 스킬안내1, Main26 피난 주민·철수 주민2, 서브 6종 수락·미충족·완료18 (오르나6·마레6·도아3·루덴3). 모든 대사의 정확 원문·화자·Quest/Scene 순서를 보존하고 텍스트/QuestLog의 ‘완료’를 사용한다. 주민·피난 주민을 같은 Runtime 화자로 합치지 않는다.

**캐릭터별 총 NPC 행 수:** 마레8(검토 후보2 + 무음6), 오르나6, 도아3, 루덴4, 피난 주민1, 주민1. **NPC는 대사 발화와 신규 캐스팅을 별도 결정**해야 한다. NPC 초상화/모션이 없다는 정책과 무음 텍스트 정책을 혼동하지 않는다.

## C. Google Cloud Text-to-Speech 무료 사용 범위·문자 예산(2026-11-23 공식 문서 기준)

| 항목 | 공식 문서에 근거한 정보 | 이번 배치 가정 |
|---|---|---|
| Standard/Legacy WaveNet | 두 제품 모두 **월 0~4,000,000자 무료 구간**으로 가격표 표시. **같은 SKU `9D01-5995-B545`**, 사용량을 각각 400만씩 분리 합산한다고 가정하지 않음 | 별도의 프로젝트/월별 실제 SKU 누적 사용량 확인이 필수 |
| 무료 구간 초과 | 두 종류 모두 **$0.000004/문자**, 즉 **$4/100만 자**(공식 상세 가격표) | 기존 다른 사용분·과금 기간/환율/세금 및 리전 확인 |
| 활성화 조건 | Cloud 프로젝트에 **Billing account 연결 및 TTS API 활성화 필수**, 무료 분량을 초과하면 **자동 과금 가능** | 이번엔 결제·계정 활성화·API 요청 **0** |
| 한국어 선택지 | `ko-KR-Standard-A/B` 여성, `-C/D` 남성 / `ko-KR-Wavenet-A/B` 여성, `-C/D` 남성 | 테스트/캐스팅 자체 미승인 |
| API 제한 | 입력 합성 요청당 **최대 5,000바이트**(문자 수가 아닌 UTF-8 바이트; 한국어는 여러 바이트일 수 있음) | 이번 2행은 요청마다 충분히 짧음. 실제 API 호출0 |
| 생성 파일의 앱·미디어 사용 | Google Cloud TTS 공식 문서는 약관·관련 법률 준수 전제로 앱/미디어 사용을 설명 | 정식 상업 배포 시 사용권·서비스별 조건 최종 확인 |

공식 자료:
- 가격: https://cloud.google.com/text-to-speech/pricing
- 한국어 보이스: https://docs.cloud.google.com/text-to-speech/docs/list-voices-and-types
- 시작/결제 연결: https://docs.cloud.google.com/text-to-speech/docs/get-started
- 요청 크기·앱/미디어: https://docs.cloud.google.com/text-to-speech/quotas

**월 무료 구간이 ‘100% 무과금 보장’이라는 뜻은 아니다.** 공식 Google Cloud 제품 소개 일부 구 페이지는 WaveNet “월 100만 자”로 소개하는 반면 **현행 상세 가격표는 Standard/WaveNet 모두 400만 자(같은 SKU)**라고 표시하므로, *계정을 실제 사용하기 직전* Google Console에 표시된 모델·SKU·가격·무료 잔량을 재확인한다. 가격표가 최우선 근거이지만 과금 책임을 대체하지 않는다.

### 정확 원문 기준 문자 수 및 초과 시 이론 요금

| 시나리오(새로 Cloud TTS를 승인한다고 가정할 때만) | 실제 합성 승인 | 해당 원문 문자 수 | 무료 구간을 이미 모두 소진했다면 이론 추가요금(USD) |
|---|---:|---:|---:|
| 현 정책 그대로 NPC 전원 무음 | **0** | **Cloud API 입력 0자** | **$0** |
| 조건부 검토 2행만 별도 승인할 경우 | 현재0 | **150자** | **$0.000600** |
| NPC 23행을 전부 예외 승인하는 가상 상한(현재 정책과 불일치) | 현재0 | **776자** | **$0.003104** |
| 기존 5인 신규72행(기존 Gemini 계열 캐스팅) | 현재0 | **1,655자** | **Cloud Standard/WaveNet 과금 대상 아님** |

- 단위는 **11/19 승인 문장 그 자체의 유니코드 문자 수**(한글·공백·문장부호 포함). 합성 요청에 `SSML`, 줄바꿈, 지시 텍스트, 반복 호출 등이 추가되면 과금 문자가 증가한다. 공식 가격표는 공백과 줄바꿈, `<mark>`를 제외한 SSML 태그도 집계한다고 밝힌다. 1회 문장길이보다 실제 Cloud Billing 계산이 최종이다.
- 본문 2행 문자 계산의 재현: `84+66=150`; 전체 NPC `776`, 무음대사 `626`. 주연72행 **1,655자**는 **대사 규모만 비교**한 것이고 기존 Gemini API의 문자 무료 구간으로 전용한 숫자가 아니다.
- 낮은 추정 달러액은 **결제 계정 등록/활성화 승인, 무료 보장, 전액 무과금 보장**에 대한 근거가 아니다. 새 프로젝트 자체의 다른 비용·서비스·재호출/실제 청구 조건을 포함하지 않는다.

## D. 예상치 못한 과금 방지 — **등록·승인 전 점검 전용**, 지금 실행 금지

1. **본 정책의 기본값이 가장 확실한 비용 차단:** `NPC_TEXT_ONLY_DEFAULT=ON`, `NPC_CLOUD_API_ENABLED=false`, 요청 코드/키/로그인 미실행, 실제 **Google POST 0**, 결제 계정 연결0. 주연용 API 키4개를 Cloud Standard/WaveNet에 임의 재사용하지 않는다.
2. **별도 사용자 승인이 나오더라도** 계정/프로젝트의 Billing 상태·SKU/모델별 무료 사용량·그 달 이미 누적된 문자·외부 프로젝트 사용·가격/환율/약관을 **개별 확인 후** 계산한 허용 문자 상한에서 *요청 전* 앱이 자체적으로 중단하도록 설계. 원문+SSML/공백 포함, 멱등성 ID/WAV 보호, 호출 횟수·문자 장부와 단일 실행 잠금, 예산 여유분 설정. **클라이언트 상한은 다른 사용자·외부 프로젝트 사용까지 완벽히 차단하지는 못하므로 실제 Google Billing과 병행 점검 필요.**
3. **Google Cloud Budget ‘알림 전용’은 하드 차단이 아님.** 임계치 알림을 받아도 API 호출/과금이 자동 중단되지 않음. https://docs.cloud.google.com/billing/docs/how-to/budgets
4. **강제 spend-cap 예산은 대상 서비스 제한:** 2026-11-10 갱신 공식 문서 기준 적용 서비스는 Gemini API, Agent Platform(Vertex AI), Cloud Run/Cloud Run functions. **Cloud Text-to-Speech는 현 목록에 없음**. Cloud TTS에 그 기능이 적용된다고 가정하지 않는다. https://docs.cloud.google.com/billing/docs/how-to/budgets-spend-caps
5. Google Cloud API quota/RPM 제한은 **요청량 제어**이지 완전한 ‘월 비용 0원’ 보장 수단이 아님. Billing 알림·월 비용 확인 및 API 사용 중지/권한 해제 등의 **수동 운영 계획** 마련. 알림 지연·프로젝트간 사용/다른 서비스 비용으로 한도를 넘길 수 있음.
6. **다른 제품 혼용 금지:** 기존 Gemini TTS의 VoiceID·일일 사용량 및 Cloud Standard/WaveNet의 **문자 기반 월별 무료 SKU는 별개**. 예전 세린/태온/하렌 162개를 Cloud 무료 분량 때문에 다시 만들거나 교체 금지.
7. **비공식 gTTS 금지:** 개발자 프로젝트 README에 Google 공식 제품이 아니고 Google Translate의 문서화되지 않은 기능을 사용한다고 명시. https://github.com/pndurette/gTTS . MIT 라이브러리 라이선스가 Google Translate에서 생성한 음원의 상업적 배포 허락까지 보장하지 않는다.

## E. 음성 재검토/Runtime 승인 게이트

- 현재 확정: 기존 주요 5인 VoiceID 유지 + 기존 162개 WAV·Unity `6000.6.5f1`/Save/`main` 보호, NPC 21행 무음 기본, 마레2행도 **실제 음성 생성 승인0**.
- 필요하면 먼저 **텍스트만으로 접근성/튜토리얼 이해도 QA**: 한기 2턴 행동불가/어묵탕2개의 동료 사용법, 동상과 한기 차이/해빙연고2개의 사용법이 음성 없이 전달되는지. 충분하면 **23행 전부 TEXT_ONLY**로 결정하고 별도 음성 사용0.
- 음성이 정말 필요하다고 사용자 별도 승인한 예외에서만, 런타임 `Main24`/Main26 정확 대사 ID·마레 CharacterId·SpeakerId/기존 화면 자막·동작 확인 → `ko-KR` Standard/WaveNet 비교 데모 **1개씩** 요청 승인 여부 판단 → 무료/과금/사용권 확인 → 사용자 직접 제작·청취 QA → **최종 별도 Unity 연동 승인**.
- **주연72행**은 추가 대사 구간/실제 Runtime ID가 확정되었을 때만 기존 VoiceID로 사용자 본인이 TTS 생성 여부 결정. 이번 보고만으로 Google AI Studio/Cloud API 호출 허가가 생기지 않는다.
- 기존 Issue#125의 101행 원문 검토 CSV 및 실용 후보 ID는 **`PROPOSAL_NOT_RUNTIME`**. 대량 제작/자동 ID 확정 금지. Main25~29·서브6는 기획 기준 Runtime 미구현. 그 사실과 `HOLD`는 원문 승인과 모순되지 않음.

**검증:** 23/23 원문 추적·21 무음/2 검토/0 승인, 72/72 주연 기존 Voice 매핑 보존, 문자열 길이 776/150/1,655, 새 TTS0/비용0/Cloud 계정·결제·API0, Unity/Save/main/#85/Discord 변경0. **문서화·분류만 수행**.

**승인 필요:** NPC 2행 예외 음성화 여부, 해당 캐릭터 실제 Runtime VoiceID, 그 경우 Cloud 계정·결제 활성화/외부 비용 가능성·샘플 시험, 주연72 신규 대사 TTS·Runtime ID 승인. 기본 21행 텍스트 유지에는 새 승인 불필요.
