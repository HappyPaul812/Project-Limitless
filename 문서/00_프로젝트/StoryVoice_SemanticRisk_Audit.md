# 전체 Story Voice Semantic Risk 사전 감사 (2026-10-06)

## 구현 전 계획

TTS 생성·WAV 수정/재배열·게임 본문·Catalog·Mixer·Portrait 변경 없이 현재 Intro/Main01~16 Flow, 제작 Manifest, 실제 Editor Catalog와 WAV를 읽는다. 기존 Source 추출기의 `authored()`를 재사용하여 현재 작성 페이지를 새로 추출하고 실제 `DialogueLine` 생성자의 화자/본문 처리 결과로 대조한다. 무음 Player·조사/관찰·ID 없는 미제작 NPC는 Voice 누락과 구분한다. 마지막 Intro 제목 및 공용 NPC fallback은 작성 Voice 분모에서 제외한다.

순서는 파일명 대신 QuestDefinition objective와 실제 Flow의 분기/배열 순서를 따른다. Main16 hearing/default는 서로 다른 경로이며 공용 페이지는 각 경로의 위치를 보존한다. 청취 Queue는 Main을 위험도 순으로 선택하되 선택한 Main 안에서는 실제 Sequence/Page 순서를 유지한다. 전체 gameplay order도 별도 컬럼으로 제공한다.

Editor 전용 C# 감사 도구는 원본 WAV의 RIFF/PCM을 읽어 길이·sample 수·RMS/Peak dBFS·파일 SHA256·PCMHash를 산출하고, 현재 AssetDatabase GUID/실제 Catalog Clip과 비교한다. 누락·중복 ID·PCM 중복·교차 Main/화자 중복·본문 대조를 검사한다. 점수는 의미 판정이 아닌 `SEMANTIC_LISTENING_PRIORITY_*`다. 의미 불일치/TTS 필요 상태를 자동 생성하지 않는다.

HIGH 후보는 비정상적으로 짧은 다문장/문자당 길이, 저음량, 중복 PCM, 과거 문제 보충팩 및 인접 같은 화자 이상치다. MEDIUM은 긴 다문장/3줄 이상, 과도한 길이, 제작팩 경계, 미청취 중요 Story다. 분포 중앙값과 화자별 비교로 수치 기준을 결정하고 아래에 기록한다. -45 dBFS는 극단 저음량 후보 기준으로 검토한다. LOW는 정상 metadata 또는 기존 사람 청취 PASS이며 새 의미 PASS를 뜻하지 않는다.

최근 Main03 태온003~012와 Main04 미엘001~008/태온001~005 **23개**의 최신 사람 Runtime PASS를 최우선 적용한다. 오래된 Listening JSON의 Main03 008 오판정 이력보다 최신 최종 QA를 우선한다. 해당 WAV/meta/path/GUID/Catalog/SHA를 보호 목록으로 고정하고 Queue에서 제외한다. 기존 Main04 미엘009 Wrong1·인접4 Suspect의 재생성 필요5건은 그대로 유지한다. 기존 NEEDS_LISTENING244는 이력 관리 범위이며 최신 PASS와 겹치므로 실제 미확인 수를 별도로 재계산한다.

검증은 백그라운드 Editor Edit Mode에서 수행한다. Scene/Play/Save/설정/포커스를 바꾸지 않는다. 새 도구의 임계값·중복·보호 우선·CSV escaping을 합성 입력으로 검사하고 실제 전수 출력/개수·현재 컴파일/Console 및 기존 파일 해시를 확인한다. 직접 관련 도구/CSV/문서만 commit하며 push하지 않는다. 선행 commit: `85592e54d3109b1ccbb031c9deba7d1d9509b2bc`.

## 결과

현재 Flow 재추출 **331 Dialogue / 247 Voice(Intro18 + Story229) / Player Silent42**. 나머지42는 ID 없는 기존 무음 NPC28 및 조사/관찰14다. Intro 마지막 제목1과 공용 NPC 대기 설정2는 작성 Voice 분모 밖이다. Voice 대상은 캐릭터/나레이터의 정식 Dialogue ID가 있는 페이지로 정의하며 미제작 NPC를 Missing WAV로 취급하지 않는다. Catalog 실제 Editor entry Story229·Intro18, 사용 ID247·중복0·null0·Missing0. Manifest249행 중 과거 Main12로 잘못 표시되어 이미 제외된 main12_miel_001/main12_taeon_001 두 행은 미사용 이력이며 새 누락이 아니다. 본문·화자·Catalog/원본 형식 drift0. Registry는 별도 Asset이 아닌 TTS_음성_제작_정책.md 및 최신 배치 QA의 casting 기록이며 Gacrux/Sulafat/Achird/Schedar/Orus를 Manifest와 확인했다. 구체적인 발화 의미는 측정으로 확인하지 않는다.

최신 사용자 실제 의미 PASS와 Runtime 청취 PASS는 각각 **23**으로 같은 집합이다. Main03 태온003~012의10개 및 Main04 미엘001~008/태온001~005의13개. 태온005 경계 사용자 PASS가 이전 Matrix runtime_listening_pass에 누락되어 이를 보완했다. 오래된 Listening JSON의 Main03 008 해결 전 불일치 기록은 역사 자료로 보존하고 최신 최종 QA가 우선함을 명시했다. 23개 모두 과거 PASS 증거 SHA와 현재 원본이 같고 GUID/path/meta를 보호했다. 새 사용자 청취 PASS를 만들지 않았다.

**NEEDS_LISTENING244는 기존 관리 범위로 유지**한다. 그 안에는 최근 PASS20개와 기존 재생성5개가 겹치므로 244 = PASS20 + 기존재생성5 + 새청취대기219. 전체 Voice247 = 기존 관리범위 밖 초기 PASS3 + 244 = 전체PASS23 + 기존재생성5 + 새청취대기219. 따라서 244개를 전부 새 불량 후보로 계산하지 않는다. 기존 Main04 미엘009 WRONG_AUDIO_CONTENT1 및 인접4 SEMANTIC_INTEGRITY_SUSPECT/TTS_REGEN_REQUIRED는 변경0, 신규 WRONG_AUDIO_CONTENT/PARTIAL_SPEECH/TTS_REGEN_REQUIRED 확정0이다. [기존5행 전달](Story_Dialogue_TTS_REGEN_REQUIRED.csv)을 재사용한다.

**HIGH35 / MEDIUM129 / LOW83**. HIGH에는 이미 알려진 재생성5개가 포함되며 새 청취 Queue의 위험 분포는 HIGH30 / MEDIUM129 / LOW60 =219다. LOW83 중 보호PASS23을 제외한60도 의미 미확인이다. 각 Voice의 priority 컬럼은 SEMANTIC_LISTENING_PRIORITY_HIGH/MEDIUM/LOW이며 semantic_state와 분리한다.

### 임계값 및 점수

전체 RMS 중앙값 -20.545189 dBFS, 최소 -57.712936 dBFS, 문자당 길이 중앙값0.250667초다. PCM 전체 sample(무음 포함)의 RMS이며 gain/trim은 하지 않았다. 문자 수는 공백/문장부호를 제외한 Unicode 문자·숫자 수, 문장 수는 .?!。！？… 연속 경계를 하나로 처리한 발화 구간 수다. 줄바꿈만으로 문장 수를 늘리지 않으며 sentence_count와 line_count를 따로 기록한다.

- LOW_RMS: -45 dBFS 미만이면80점. 중앙값보다24.45 dB 낮은 극단 기준이며18개가 해당한다. 저음량 후보일 뿐 내용 오류 확정이 아니다.
- Short: 문자12개 이상이고 문자당 길이가 min(0.07초, 화자 중앙값×0.45) 미만이면70점. 화자 장문 표본5개 미만이면 전체 중앙값을 사용한다.12개.
- Multi-Sentence Short: 문장2개 이상이고 길이2.2초 미만 또는 Short이면70점.15개. 두 후보 집합 합집합17개이며 중복 집계하지 않는다.
- 같은 Main/같은 화자/인접Sequence의 타ID3개 이상 대비 RMS가18dB 이상 낮거나 문자당 길이35% 미만이면70점.13개.
- 다른 ID PCM 완전 일치80점, cross-Main/화자는 각각20점. 현재 모두0. PCMHash는 format:channels:sampleRate:bits: ASCII prefix + 실제 data chunk의 SHA256이며 WAV header/추가 chunk 차이를 제외한다. 샘플 인코딩/샘플레이트가 다르면 동일 PCM으로 추정하지 않는다.
- 과거 문제가 있던 Main03/04 보충팩 미확인 Voice70점. 기존 재생성 필요는100점이나 기존 판정 보존 표시일 뿐 신규 판정이 아니다. Missing100점/본문·Mapping·ID 중복 후보80점.
- 3문장 또는3줄 이상/35자 이상 다문장25점. 길이가 max(16초, 문자수×화자중앙값×2.5) 초과면25점. 인접 제작팩 경계20점, 미청취 보충팩20점, 첫 조우/전투후/Intro/Main12 이후 중요 Story 미청취20점.
- 합계70점 이상 HIGH,20~69 MEDIUM,20 미만 LOW. 보호PASS는0점/LOW/PROTECTED_PASS로 최우선 고정하여 문제 후보/Queue에서 제외한다. 제작일은 파일 수정일로 추측하지 않고 Manifest batch 및 채택된 replacement_source 폴더로만 구분한다.

이상 후보는 Low Volume18 / Short Duration12 / Multi-Sentence Short15 / Duplicate PCM0 / Cross-Main0 / Speaker-cross0. ID 중복0·Missing0·Manifest drift0·Catalog 화자 불일치0. 수치 후보는 서로 겹치며 잘못된 발화 수로 합산하지 않는다.

### Main별 현재 집계

| 구간 | Dialogue | Voice | HIGH | MEDIUM | LOW | 사용자/Runtime PASS | 미확인 청취 | 기존 재생성 | 누락 | 부분발화 후보 | PCM중복 | 저음량 |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| Intro | 18 | 18 | 0 | 18 | 0 | 0 | 18 | 0 | 0 | 0 | 0 | 0 |
| Main01 | 10 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Main02 | 7 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Main03 | 16 | 12 | 2 | 0 | 10 | 10 | 2 | 0 | 0 | 0 | 0 | 0 |
| Main04 | 22 | 18 | 5 | 0 | 13 | 13 | 0 | 5 | 0 | 0 | 0 | 0 |
| Main05 | 14 | 6 | 0 | 6 | 0 | 0 | 6 | 0 | 0 | 0 | 0 | 0 |
| Main06 | 4 | 3 | 0 | 0 | 3 | 0 | 3 | 0 | 0 | 0 | 0 | 0 |
| Main07 | 40 | 26 | 4 | 10 | 12 | 0 | 26 | 0 | 0 | 1 | 0 | 0 |
| Main08 | 23 | 19 | 3 | 16 | 0 | 0 | 19 | 0 | 0 | 1 | 0 | 2 |
| Main09 | 32 | 27 | 7 | 4 | 16 | 0 | 27 | 0 | 0 | 6 | 0 | 6 |
| Main10 | 25 | 18 | 3 | 5 | 10 | 0 | 18 | 0 | 0 | 3 | 0 | 1 |
| Main11 | 31 | 28 | 7 | 2 | 19 | 0 | 28 | 0 | 0 | 4 | 0 | 5 |
| Main12 | 4 | 3 | 0 | 3 | 0 | 0 | 3 | 0 | 0 | 0 | 0 | 0 |
| Main13 | 8 | 8 | 0 | 8 | 0 | 0 | 8 | 0 | 0 | 0 | 0 | 0 |
| Main14 | 11 | 11 | 1 | 10 | 0 | 0 | 11 | 0 | 0 | 1 | 0 | 1 |
| Main15 | 22 | 22 | 1 | 21 | 0 | 0 | 22 | 0 | 0 | 0 | 0 | 1 |
| Main16 | 44 | 28 | 2 | 26 | 0 | 0 | 28 | 0 | 0 | 1 | 0 | 2 |

### 내일 청취 Queue

[자연스러운 Story 진행 순서219행](StoryVoice_ListeningQueue.csv)은 Intro→Main03→Main05…→Main16, 각 Main의 실제 Sequence/Page 순이다. [위험 구간 우선219행](StoryVoice_PriorityListeningQueue.csv)은 Main09→11→07→08→10→16→03→14→15→12→Intro→13→05→06 순으로 구간 착수 순서를 정하되 Main 안의 발화를 점수/파일명으로 뒤섞지 않는다. 첫 추천 Main09는27Voice·HIGH7·저음량6이며 다음 Main11은28Voice·HIGH7·저음량5다. 우선 비교 대상 HIGH는 신규청취30개이며 전체219개를 한 번에 재생성하라는 목록이 아니다. 1차 Main09→Main11의55Voice를 게임 순서로 들으며 후보를 확인할 수 있다. 현재 플레이 진행에 맞춰 자연 순서 파일을 사용해도 된다. 기존Main04재생성5는 Queue에서 빼고 기존 Handoff로 관리한다.

Main16은 한 플레이에서 hearing/default가 동시에 나오지 않는다. 각 Sequence에서 hearing+공통 페이지를 기준 순서로 두고 default 전용 차분을 별도 route 행으로 이어서 기록한다. 공유 ID는 한 번만 세며 playback_locations에는 양쪽 실제 factory의1부터 시작하는 페이지 위치를 모두 기록한다. canyon 공통 첫/후반 및 afterimage 경로 합류 위치도 실제 배열로 확인했다. route를 섞어 한 Runtime에서 모두 나왔다고 주장하지 않는다.

### 검증과 보호

Editor 감사529검사 PASS: 새 Source/Manifest hash, 실제 DialogueLine/현재 Catalog/WAV247 sample layout, Main03/04/05/07/08/09/10/11/15/16 factory 위치 및 보호 제외. Main01/02/06/12~14와 동적 NPC 분기는 현재 Source 생성자로 대조하며 새 Play Mode 검증은 실행하지 않았다. 합성10case PASS(다문장/짧은길이/보호/저음량/CSV/정상metadata≠의미PASS/중복/과도한길이/PCM수/RMS/header독립 hash). Python wave+NumPy 독립247WAV 전수검증은 길이/sample/SHA/PCM/RMS/Peak 모두 일치, 최대RMS·Peak 오차0dB. 과거PASS23해시도 전부 일치. CSV header 유일성/열 개수/219 ID 유일성/자연Main순서/Main별Sequence순서/분모 및 기존 재생성5 일치 PASS.

초기 합성 fixture의 null Sequence와 Edit Mode 빈 PlayerName 대조는 감사 도구 안에서 보완하여 최종 전수검증에 실패를 남기지 않았다. 실제 사용자 Session/이름을 바꾸지 않는다. Compile Error0·최종Console Error0. Bootstrap Edit Mode/is_focused=false 유지, Scene·Play·Save·설정·OS포커스 변경0. 현재 의미 청취는 실행하지 않았으므로 다음 작업은 실제 사람이 Queue의 자막/전체발화/화자/음량을 듣고 ID별 결과를 기록하는 것이다.

시작 시 보호한 기존2260파일 전부 동일 SHA이며 WAV/meta/Catalog/Registry/게임C#/Story 원문/사용자 수동이미지/Scene/Packages/설정 변경0. 새 Editor 감사 C#/meta만 추가했다. Matrix의331행 및 기존 관리244 marker는 유지하며 현재 위험/해시/Queue 위치/보호 상태 컬럼과 태온005 최신PASS를 반영했다. 기존 Story_Dialogue_Audit.json의 Runtime 역사 rows는 보존하고 current_semantic_summary 및 새 감사 링크를 추가했다. 기존 Story_Dialogue_Listening_Results.json/TTS_REGEN_REQUIRED.csv는 수정하지 않았다.

### 파일과 재실행

- StoryVoice_SemanticRisk_Audit.csv/json: Voice247 전수/작성331 및 검사 근거.
- StoryVoice_ListeningQueue.csv / StoryVoice_PriorityListeningQueue.csv: 새 청취219 자연순서/위험구간 착수순서.
- StoryVoice_Protected_PASS.csv: 보호23개 현재 SHA/PCM/GUID/path·최신 근거.
- StoryVoice_MainSummary.csv / StoryVoice_SemanticRisk_Verification.json: Intro+Main01~16 17구간 요약 및 독립 검사.
- Unity/Client/Assets/_Project/Scripts/Editor/StoryVoiceSemanticRiskAudit.cs: 파일 읽기/측정/분류/CSV 및 합성 검사.
- Tools/TTS/prepare_story_semantic_risk.py → Editor StoryVoiceSemanticRiskAudit.SelfTest()/Run() → Tools/TTS/verify_story_semantic_risk.py. 준비 입력은 Temp/StorySemanticRisk20261006/input.json이며 git 대상이 아니다.

직접 관련 감사/문서만 commit한다. 이번 작업 파일의 git diff --check는 오류0이다. 전체 작업트리 diff --check는 기존 무관 Asset/Scene의 trailing whitespace 진단(출력858줄)로 exit2이며 보호 원칙에 따라 수정하지 않았다. 기존 다른 작업103개는 유지하며 GitHub push0. 마지막 선행 관련commit `85592e54d3109b1ccbb031c9deba7d1d9509b2bc`, 이번 commit은 최종보고 참조.
