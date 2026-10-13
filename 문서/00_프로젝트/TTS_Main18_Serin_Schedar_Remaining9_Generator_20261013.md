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
