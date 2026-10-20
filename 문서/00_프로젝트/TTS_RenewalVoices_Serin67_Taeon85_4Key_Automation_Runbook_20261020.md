# LIMITLESS / Renewal_Voices 세린·태온 152대사 자동 제작 실행 정본 — 2026-10-20

**작성자:** TTS 담당 / **브랜치:** docs/tts / **사용자 지시:** “작업 자동화 하자. 지금 작업 폴더에다 태온 세린 각각 폴더 만들어, 또 지금 API키 두개 더 추가해서 4개거든? API.txt 파일에 ;이랑 엔터로 구분해서 둘꺼니까 태온, 세린 각각 대사 다 뽑아서 파일에다 저장하고 뽑을 때마다 대사랑 API 사용량 카운트 해줘. API 사용량은 하루마다 리셋.. 오늘은 34회고 샘플도 쓸거면 각자 폴더로 복사하고 하렌은 빼고.”

> **상태: 실행 패키지 제작 및 ZIP 오프라인 모의검증 완료, 사용자 PC Google API 실제 호출/원본 Unity 변경은 이번 작업에서 0.** 실제 `F:` 디스크는 ChatGPT 세션에서 직접 접근하지 못하며, 사용자 PC에서 ZIP 병합 해제 및 명령 실행 시 폴더·manifest·WAV가 생성된다. 원본 네 API 키를 업로드하거나 GitHub에 작성하지 않는다.

## 1. 제작 대상 — 현재 Main21까지 확정 대사만

| Character | ID / New Voice | Unity 적용음성 교체대상 | Main20·21 미등록대상 | 목표 |
|---|---|---:|---:|---:|
| 세린 / Serin | `companion_serin` / `Erinome` | 58 | 9 | **67** |
| 태온 / Taeon | `companion_taeon` / `Algieba` | 82 | 3 | **85** |
| **총합** | | **140** | **12** | **152** |

**제외: 하렌3**. 하렌은 나중에 평서문 억양 문제를 포함해 3개를 별도 본제작한다(최신 사용자 지시). 기타 폴·미엘·레온·나레이터, Player 무음, Chapter3 미확정 대사도 범위 외.

**정확 원문 152개 GitHub 냉동 정본:** [TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json](TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json), `docs/tts` 커밋 `9183376e8292332be9a8a338a55eb5fbb0b9c9eb`. 실제 `main`의 `StoryVoiceCatalog.asset` 290개를 검사하여 두 화자 140개 ID만 추출하고, 정확 문자열은 2026-10-20 `Story_Dialogue_Audit_Matrix.csv`/Main07 보충/정본 Main17·18·19 Manifest의 실제 대사, Main20·21 미등록12를 결합. 중복 ID0, 세린67·태온85 모두 비어 있지 않음. 사용자 로컬 브랜치의 미병합 변경이 있다면 원문 비교 필요.

## 2. 작업 위치 및 납품 ZIP

**사용자 절대 경로:** `F:\Downloads\Renewal_Voices`.

**직접 제작 패키지:** `LIMITLESS_Renewal_Voices_Serin67_Taeon85_Auto_20261020.zip`; 크기 **16,492 bytes**, SHA256 **`4fc85d78105f08c21241877c05d351d37eae3092f42e51a5f9c247b46ddeebc2`**.

- `Renewal_Auto_TTS.py` (**25,921 bytes**, SHA256 `efb653101f34276483f821a0b3d7eb9b57142a6b999f8c9c74d805dfdf8bdf88`) — Google TTS GenerateContent 원본 대사 1ID→1POST→1WAV.
- `RENEWAL_SETTINGS.json`: 초기 전체 PT 하루 생성 시도 예산 `daily_request_budget: 34`, 안전 키/프로젝트 그룹 `10`회, 키 그룹 기본 동일 `SHARED`, `free_tier_confirmed:false` (Google AI Studio 무료/과금 상태 확인 전 호출 차단), 요청간30초.
- `README_KO.txt`, `selftest.py`, ZIP 내부 `Serin/`, `Taeon/`, `_progress/` 디렉터리.
- ZIP은 **기존 API.txt나 승인 WAV/samples, 기존 프로젝트 파일을 담지 않음/덮어쓰지 않음**. 사용자 입력 키4는 `API.txt` 기존 파일 또는 신규 사용자가 직접 채움.

## 3. API 키와 프로젝트 주의

- `F:\Downloads\Renewal_Voices\API.txt`는 UTF-8로 실제 키 **4개**를 `;` 또는 엔터(`\r\n`, `\n`)로 구분. 순서대로 KEY1~KEY4 라벨만 로그에 남고 원문 비밀키·파일은 버전관리/GitHub/이 대화에 기입 금지. 중복/잘못된 개수는 호출 전 차단.
- **Google API RPD는 키당이 아니라 프로젝트당 부과**되고 **태평양 시간 자정** 기준 리셋. 공식 근거: https://ai.google.dev/gemini-api/docs/rate-limits 및 https://ai.google.dev/gemini-api/docs/billing.
- 따라서 4키=4프로젝트라는 가정 금지. 최초 `run` 명령에서 사용자가 **SHARED**(동일 프로젝트) 또는 실질적으로 별도 Google Project 4개라고 확인한 경우에만 **SEPARATE**를 입력. 혼합 그룹은 SETTINGS의 `project_groups` 수동 지정·`project_groups_confirmed:true`; 같은 프로젝트 키는 반드시 같은 그룹.
- **오늘 34회는 사용자 보고한 남은 총 요청 예산**이지 제공자가 검증한 실시간 잔여 사용량 아님. 스크립트 자체의 PT 오늘 시도 횟수를 34개 이하로 제한하지만 프로젝트별 실잔여·외부 스크립트가 이미 소비한 요청은 자동으로 알 수 없다. 서비스 429/400/타임아웃 등 실패 시 즉시 HOLD, 새 키로 자동 우회/무한 재시도 금지.
- `per_key_safety_budget:10`은 과거 이 프로젝트 운영 경험의 보수적 운영값이며 **Google의 보편적 규칙이 아님**. 별도 프로젝트가 아닌 경우 여러 키가 동일 그룹 10개 한도 공유. 사용자가 검증한 실제 모델·프로젝트 별 할당량을 넘겨 자동 호출하지 않는다.

## 4. 한 번 실행 방법 (사용자 로컬)

압축을 `F:\Downloads\Renewal_Voices`에 **병합** 해제, 기존 `samples`·`API.txt` 보존.

```powershell
conda activate py3_12
cd F:\Downloads\Renewal_Voices
python .\Renewal_Auto_TTS.py init
python .\Renewal_Auto_TTS.py check
```

`init` 작업:
- 정본 경로 `F:\study\codex\Project-Limitless` 아래 main 기준 Unity Catalog·Audit Matrix 및 챕터 Manifest **읽기 전용** 파싱 후 생성 목록 `RENEWAL_DIALOGUES_152.json` 생성. 해당 폴더를 찾지 못하면 GitHub 정본 raw URL을 시도; private repo·네트워크 차단 시 오류를 알리고 대사 임의 생성 금지. `init --repo 다른경로` 지원.
- `samples/main20_confront_serin_01__Erinome.wav`를 `Serin/main20_confront_serin_01.wav`로, `samples/main21_route_taeon_01__Algieba.wav`를 `Taeon/main21_route_taeon_01.wav`로 **기존 사용자 긍정 샘플이 실제 존재하고 PCM/해시 검증에 통과하면 복사**, 보존·API0·재생성0. `samples` 기존 파일 이동/삭제 없음.
- 음성 대상 폴더 내 `dialogues.csv`로 ID·정확원문·현재 READY/PENDING 상태 작성. 기존 유효 WAV/meta는 SHA로 재확인.

실제 무료 Project/Model 잔여 및 Billing 비과금 여부 확인 후 `RENEWAL_SETTINGS.json`의 `free_tier_confirmed`를 `true`로 변경하고:
```powershell
python .\Renewal_Auto_TTS.py run
```
대량 제작에 `RUN` 입력 승인. API 호출은 **세린67 먼저→태온85 다음**. 하루 전송34 도달 또는 해당 프로젝트·키 안전 한도에서 종료한다.

소량 먼저 확인:
```powershell
python .\Renewal_Auto_TTS.py run --max-calls 2
```

PT 자정마다 날짜가 바뀌면 호출 횟수 로그의 **그 날 수만 새로 0부터 계산**(기존 사용 이력 삭제/초기화하지 않음). 사용자가 PC 프로세스를 계속 열어둘 때만 자동 이어가기:
```powershell
python .\Renewal_Auto_TTS.py watch
```
`watch`는 프로세스 종료/PC 종료 시 실제 작업이 중단된다. 별도 Windows Task Scheduler 설치·백그라운드 클라우드 작업은 없음. 사용자는 나중에 run/watch 재시작으로 중복 생성 없이 재개 가능. 미래 날짜도 기본 설정 예산34(실제 할당량 다르면 설정 수동 수정, Google 서버 실할당량 우선)이며 **향후 34회 보장 아님**.

## 5. 실시간 사용량·대사 추적

- `Serin/<DialogueId>.wav`, `Taeon/<DialogueId>.wav`: 개별 PCM16 24kHz mono. 동명 `<DialogueId>.json`에 원문/Voice/Model/응답 음원SHA/발화 길이/API 키 번호(값은 미기록)·청취 대기.
- `Serin/dialogues.csv` 및 `Taeon/dialogues.csv`: ID/대사 원문/소스/장면/READY/PENDING.
- `_progress/api_events.jsonl`: 절대 누락/삭제 금지 append-only `ATTEMPT`(전송 *직전* 선기록)·`SUCCESS`·`FAIL`. 실패 및 결과 불명도 사용 시도1로 카운트. 요청마다 문자열 대사와 KEY 번호 기록.
- `_progress/USAGE_LOG.csv`: 대사/KEY 번호/프로젝트 그룹/태평양 날짜/성공·실패를 한눈에 검사 가능.
- `_progress/USAGE_TODAY.json`: 태평양 오늘 요청 시도·키1~4별 횟수·프로젝트 그룹 횟수·남은 **로컬 예산** 및 Serin/Taeon 현재 합격 샘플+생성 건수.
- 생성/저장중단이나 429 등 실패 시 `_progress/HOLD.json` + `RUNNING.lock` 잠금으로 재실행 자동 진행 차단. 잠금 강제 삭제 금지, 의심시 청취/로그·과금 확인 먼저.
- PCM/샘플 길이 검증은 **파일 형식 검수**. 한국어 발음·성별 지각·캐릭터 일관성·원문 실제 청취 PASS는 여전히 바울 대표 최종 승인 대상.

## 6. 검증 및 구현 경계

- Python 구문 확인 PASS.
- 5개 로컬 unittest PASS: GitHub 구조에 준하는 가짜 로컬 저장소에서 152 원문 빌드·카운트, API.txt 키 구분/중복차단, 프로젝트 그룹 cap, 24k WAV 검증, 모의 HTTP GET/POST 성공4·JSON·LEDGER·동명 중복차단.
- 실제 ZIP 압축해제→가짜 저장소 `init`→`check` + 샘플 1개 실제 파일 복사와 API0, 전체 파일 검사 PASS.
- ZIP CRC PASS, ZIP 내부 실제 API.txt/실제 비밀키 **없음**.
- **Google Gemini live calls 0; 사용자 F 드라이브 쓰기0; 실제 신규 WAV0; Unity/Save/GUID 변경0; main 직접 커밋/Push/merge0**. 실제 Google 현행 API 모델 GET/GenerateContent 성공 여부는 사용자 로컬 실행 후 확인 필요.

## 관련 문서
- [3인 캐스팅 및 원래 음성 오류 사고 정본](TTS_캐스팅_성별역전_음색불일치_사고보고서_및_3인재캐스팅_20261020.md)
- [3인 3/67/85 대사 집계](TTS_세린_태온_하렌_캐스팅_대사집계_20261020.md)
- [하렌 단발 R1 보류(이번 자동화에서 제외)](TTS_하렌_평서문_억양_수정R1_20261020.md)
- [TTS 최신 인계](팀운영/인계/TTS_LATEST.md)
- [GitHub Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20)

**최종 상태:** AUTO_GENERATOR_READY / USER_LOCAL_INIT_PENDING / USER_API_KEYS_LOCAL_ONLY / USER_QUOTA_CONFIRMATION_REQUIRED / NO_REAL_POST / UNITY_UNCHANGED.
