# LIMITLESS TTS / Main18 세린 Schedar 남은9 생성기 — 2026-10-13

**CODE_PREPARED / MOCK_TEST_PASS / LOCAL_EXECUTION_NOT_EXECUTED / GEMINI_API_CALLS_THIS_TASK=0**

## 사용자 승인 및 샘플 재사용
- 사용자: “뽑아. 그리고 지금 샘플도 쓸 수 있으면 쓰고 아깝잖아...ㅋㅋ” → **잔여 9개 제작 승인**.
- 기존 샘플 `main18_ground_serin_01`은 **정식 10개 중 한 개로 재사용**. 다시 호출하지 않음.
- 승인 샘플 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`. 기존 성공 사용자의 직접 음색 청취 `USER_VOICE_APPROVED`, 문장 전체 정확성 `PENDING`.
- 예정 Voice `Schedar`, 모델 `gemini-3.8-flash-tts`, SpeakerId `companion_serin`, ExactText/Stable ID 변경 없음.

## 새 사용자 다운로드 생성기
- 신규 `M18_Serin_Schedar_REMAINING9.py`, SHA256 `94b75a9f0c7901bb4a94b2f0da017985036075809a593ee6c5838196a6a46c76`, 21,865 bytes.
- `M18_Serin_Schedar_REMAINING9.zip`으로 별도 배포. 기존 `M18_Serin10_Staged.py`, `M18_Serin10_Schedar_v2_SAFE.py`, `M18_Serin_Schedar_SAMPLE_ONE.py` 수정 없음.
- `check`: 기존 SAFE v2 10원본/10백업/25정상 WAV hash, Manifest+Catalog, 기존 샘플 생성 기록과 사용자 로그 SHA, Script SHA, Scene Style 일치 확인. API0/파일 쓰기0.
- `run`: **Key slot 2만**, 기존 샘플 제외 StoryOrder `02,07,08,09,10,11,12,13,14`의 9개만 최대 호출. 사용자가 현재 프로젝트+모델별 무료 남은 사용량과 과금 위험을 직접 확인한 뒤 대화형 `YES` 필요. SDK attempts1, 자동 retry0, 호출간 최소25초 대기.
- 생성 전 Dialogue ID별 독립 배타적 attempt checkpoint, 성공 후 결과/해시 기록. 중복 성공본은 다시 호출하지 않음. 오류/429/호출 결과 불확실 시 HOLD+세션 잠금, 다른 Key/ID 재개 금지. StoryOrder 중간 건너뛰기 방지.
- 출력은 프로젝트 외부 별도 `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\remaining9_wav\<ID>.wav`, 승인 샘플은 `sample_candidate\main18_ground_serin_01.wav`에 그대로 보관. 전체10개 StoryOrder CSV에 두 경로를 구분 기록.
- 원본10·백업10·기존 정상25·Main17 승인 세린11·Unity Catalog/Scene 수정0. Script가 사용자 PC에 다운로드된 것과 실제 실행은 미확인.

## 검증
- Python syntax compile/AST에서 실제 Gemini `generate_content` 호출 지점 1개, ZIP CRC 통과.
- 오프라인 mock 9회 순차 생성 → mock WAV9개, 샘플 재호출0, StoryOrder review, 결과9개 PASS.
- 오프라인 mock 둘째 호출에서 429 강제 → 저장 첫1개 보존, 2회만 시도하고 HOLD/Lock, 뒤7개 호출0 PASS.
- **테스트는 MOCK SDK이고 실제 Gemini 요청0**, 실제 성공/할당량·Billing 보증 불가.

## 계정 quota와 후속
- Google AI Studio 화면에서 무료 Tier/RPD 예시10·RPM3을 확인했으나 **Key2가 사용하는 프로젝트의 오늘 정확한 잔여량/결제 위험은 완전 검증되지 않음**. Key별 별도 quota라는 가정 금지.
- 1회 샘플이 이미 같은 Key2 슬롯에서 성공했으므로, 9회는 **다른 호출이 없고 Project+Model 무료 잔량 ≥9일 때만** 요청 가능. 사용자가 `--max-new-calls`로 안전한 수만 지정할 수 있다.
- 실패/429이면 즉시 중단하고 로그 판독 전 재실행 금지. 9개 WAV 성공 ≠ 사용자 청취 승인. 전체10 음색·원문·혼입/누락/길이 검수 후 Codex에게 별도 Unity 음성 연결 승인 필요.
- 이번 업무의 Unity 수정0, main 수정/merge0, 실제 GitHub 문서 기록은 `docs/tts`에서만.


## 2026-10-13 사용자 LOCAL 제작 결과 — Story13 런타임 실패 안전 중단

사용자가 Anaconda PowerShell 스크린샷을 제공했다. **사용자 터미널 출력 기반 관측**이며, 별도 WAV 바이트 검사나 실제 청취는 이번 담당이 직접 수행하지 않았다.

- 승인 샘플 Story06 `main18_ground_serin_01`은 이전 성공1/사용자 음색 PASS, 기존 SHA 그대로 **중복 생성하지 않음**.
- 남은9 대상 중 Story **02,07,08,09,10,11,12** 성공 저장 **7개**. 출력 각각 `[SAVED]` 및 SHA256 기록 있음. WAV 파일별 실물/SHA 재검증은 로컬 검사 대기.
- 이번 9대상 중 Story13 `main18_watcher_witness_serin_01`은 `[API REQUEST]` 직후 `[SAFE HOLD] ... / RuntimeError` 발생. **결과 불확실 1회**: 서버측 quota 소진인지, API 응답 오류인지, 저장/PCM 변환 문제인지 **UNKNOWN**. 단순히 quota 초과로 단정 금지.
- Story14 `main18_watcher_witness_serin_02`는 **NOT_CALLED**.
- **최종 상태(사용자 로그): 정식 목표10개 중 저장8(샘플1+이번7), 실패/불확실1, 미호출1.** 이번 배치 추가 API 전송 시도8개(성공7+불확실1), 최초 샘플까지 합계 시도9(사용자 제공 로그 기준).
- 실패 위치 원본 코드 `except BaseException`은 감지된 `RuntimeError`의 상세 메시지를 숨기고 `FAILED_OR_UNCERTAIN_HOLD`를 남긴다. `RuntimeError` 원인은 **메시지가 없으므로 미확정**.
- **SAFETY HOLD 상태 유지**: 기존 성공8/정상 Unity25/오류 원본10/백업10 및 모든 checkpoint 보호. 원본의 `remaining9_attempts`, `remaining9_results`, `remaining9_wav`, `SAMPLE_ONE_ATTEMPT.json`, `remaining9` HOLD/LOCK 관련 자료 삭제·수정 금지.
- API 재시도0, KEY 변경0, 잠금/체크포인트 삭제0, Story14 선호출0. 현재 quota와 Billing 불확실, **추가 실행 허가0**. 이전 나머지9 생성 승인은 이미 시도되어 중단한 상태로, 실패후 재개는 원인 확인과 별도 사용자 승인 필요.
- 다음: Codex가 해당 사용자 LOCAL stage를 **읽기 전용**으로 조사하여 Story13 attempt/hold/lock/result/WAV 유무, 7개 SAVED hash·PCM·기존 샘플 및 정상25 보호, SDK 버전과 `RuntimeError` 발생 가능 코드/오류 경로 확인. API/Python 생성기 실행 금지. 결과를 받은 후 정확한 재개 지점(Story13 재시도 필요 여부/Story14 미호출)을 결정.
- 사용자 전체 음색/원문 청취 승인은 아직 완료되지 않았음. 최종 승인 전 Unity 적용 금지. 본 갱신은 `docs/tts` 문서만이며 `main` 변경·Git Push/Unity 작업0.


## 2026-10-13 Google AI Studio RPD 화면 — 프로젝트 식별 분리

사용자가 AI Studio의 최근28일 `일일 최대 요청 수 (RPD)` 그래프 화면을 제공했다.

- 화면 URL의 선택 프로젝트 식별자는 `gen-lang-client-0895671759`로, 과거 사용자 화면에서 **키1 / Default Gemini Project**로 표시된 프로젝트와 일치한다.
- 2026-10-13 툴팁 `Gemini 3.8 Flash TTS 9 / 10`: **해당 선택 프로젝트의 관측 일일 요청수9 / 표시 한도10**. 사용 가능 잔여량이 9라는 뜻이 아니다.
- 실제 생성 CLI는 `--key-slot 2`이며, 앞서 키2 프로젝트 선택 화면의 식별자는 `gen-lang-client-0954145637` (`Project-Limitless`)였다. **로컬 KEY2 환경 설정이 어느 Google Project에 연결돼 있는지 실측 근거는 아직 없다.** 이번 그래프만으로 KEY2의 정확한 RPD 사용량을 입증할 수 없다.
- 사용자의 로그에서 총 API 시도 9(샘플1 + 잔여 배치 시도8)와 선택 프로젝트 그래프9가 숫자로 일치해도 **실제 귀속 프로젝트를 단정하지 않는다**.
- Story13의 `RuntimeError`가 RPD 소진/429이라는 직접 증거는 여전히 없다. 최신 `FAILED_OR_UNCERTAIN_HOLD` 유지.
- 작업 후 사용자에게 **실제 KEY2 소속 프로젝트 및 해당 프로젝트의 RPD 오늘 수치**, 마지막 시도 원인 (가능한 경우 키·Secret 제외) 확인을 요청. 새 호출/재시도/Key변경/Lock수정0, Unity·main 변경0.


## 2026-10-13 두 프로젝트 RPD 화면 비교 — KEY2 귀속 불일치 조사 대기

사용자가 두 Google AI Studio Rate Limit 화면을 순서대로 제공했다. 각 화면 모두 최근28일의 `Gemini 3.8 Flash TTS` RPD 차트에서 **2026-10-13** 툴팁을 선택했다.

| 화면 선택 프로젝트 | 화면상 RPD 사용/한도 | 판독 |
| --- | --- | --- |
| 키1로 설명된 Default Gemini Project | **9 / 10** | 사용자 제공 계정 화면의 관측값 |
| 키2로 설명된 Project-Limitless | **0 / 10** | 사용자 제공 계정 화면의 관측값 |

**위 0/10은 지금 서버에 10회가 남았다는 보장이 아니다.** 그래프 동기화 지연/쿼터 별도 집계가 있을 수 있고, Key slot2의 실제 키가 어느 Project에 속하는지도 스크립트와 실제 환경에서 아직 독립 확인되지 않았다.

- 사용자는 `M18_Serin_Schedar_REMAINING9.py --key-slot 2`로 생성했고, 이전 샘플도 slot2였으며, TTS 로그상 총 **요청 시도9**(샘플1+추가8, 성공8·Story13 불확실1)이다. 이 숫자9와 첫 프로젝트 RPD9가 같아도 **동일 프로젝트 귀속의 직접 증거는 아님**.
- 즉시 할 일: 기존 프로젝트 로컬 `--key-slot` 처리, 사용 API 키 환경변수의 *이름만*, 실제 키와 Google Cloud Project의 대응을 **비밀 값 출력 없이** 확인하도록 Codex 정적 감사 요청. 키값·결제정보·환경변수 전체 출력 금지. 원인 확인 전 네트워크/생성 요청 금지.
- Story13 `RuntimeError`의 메시지·원인 여전히 UNKNOWN. `FAILED_OR_UNCERTAIN_HOLD` 해제 금지, Story14 선호출 금지.
- 승인된 샘플1·신규성공7 원본과 SHA, 오류원본10/백업10, Unity 정상25 모두 보존. 추가 TTS API0, Unity 변경0, main 병합·Push0.


## 2026-10-13 Codex 읽기 전용 원인 감사 수령 — SAFE HOLD 유지

사용자가 Codex의 감사 완료 메시지 화면을 제공했다. **Codex가 보고한 로컬 감사 결과**이며 본 TTS 담당은 사용자 PC 파일에 직접 접근하지 않았다.

- `main18_ground_serin_01` 승인 샘플1 + 추가 Story02·07~12 WAV7 = **8개 모두 로컬에 존재**, 각 SHA256·기존 checkpoint·PCM 형식 **PASS**라는 보고.
- Story13 `main18_watcher_witness_serin_01`: **시도 기록 및 HOLD 기록 존재**. 새/부분 WAV·신규 결과 JSON **없음**. 기존 오류 백업 WAV는 새 성공 결과 아님.
- Story14 `main18_watcher_witness_serin_02`: **로컬 시도 기록·결과·신규 WAV 없음** → `NOT_CALLED_LOCAL_RECORD`; 서버 호출 자체가 전혀 없었다는 사실까지 실측한 것은 아님.
- Story13 원인: **UNKNOWN**. 로컬 기록에는 예외 이름 `RuntimeError`만 남아 Google API 응답·429/할당량·WAV 변환/검증·저장·SDK 단계 중 어느 오류인지 구분 불가. 429로 확정 금지.
- 보존: Codex가 대상 폴더 **42개 파일 + 프로젝트 보호 파일 5,199개**의 SHA256을 비교해 불변이라고 보고. `HOLD`, `SESSION.lock`, 시도 기록 보호.
- Codex 조사 과정의 API 시도 **0**, WAV 수정/삭제 **0**, Unity 변경 **0**, Git Push **0**.
- **SAFE HOLD 유지**. 원본 checkpoint/lock 삭제 또는 수정하지 않음, Story13 재시도·Story14 선호출·Key 변경 금지. 재개를 위한 별도의 새 이력/수동 승인 흐름은 향후 quota·프로젝트 귀속·비용 확인과 사용자 별도 승인 후 설계·실행.
- 전체10 음색/ExactText 사용자 청취 검수는 샘플을 제외하고 아직 완료되지 않음. 신규8개 Unity 적용 승인도 없음.
- Codex 메시지 내 외부 보고서 링크/ID별 SHA 표 자체는 이 대화에 제공되지 않음. 세부 보고 경로/근거는 추가 자료가 있을 때 재확인한다.

  
## 2026-10-13 사용자 음색 검토 — Story07 보류

사용자는 생성 WAV `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\remaining9_wav\main18_heat_vent_serin_01.wav`에 대해 **"감기걸린 여자? 변성기 전 미소년?"**이라고 청취 인상을 보고했다.

- Dialogue ID: `main18_heat_vent_serin_01` / Story07 / `companion_serin`.
- 파일 제작 및 checkpoint 상태: `SAVED` (앞서 SHA/PCM 검증 PASS). **제작 성공과 화자 음색 승인 별개**.
- 음색 상태: `USER_REPORTED_TIMBRE_MISMATCH / USER_REVIEW_REQUIRED / HOLD`. 기존 승인된 Story06 샘플과 음색 일관성 확인 필요. 최종 `REGEN_REQUIRED` 승격은 사용자 판정 또는 실제 비교 검토 후.
- 담당자는 해당 사용자 로컬 WAV를 **직접 청취하지 못했음**. 여성/남성 음색·발화 정확성을 단정하지 않는다.
- 샘플1 포함 WAV8개 저장 상태는 그대로 유지, 단 Story07은 사용자 최종 음색 승인 **불가·보류**. 다른 새 WAV 자동 승인 금지.
- 원본 WAV/기존 정상25/backup/checkpoint/lock 보존. API 재호출0, Unity 변경0, GitHub main 변경0.
- Story13 `SAFE HOLD` 해제 없음. 기존 Story13/14 2개 미완료 상태 유지.
