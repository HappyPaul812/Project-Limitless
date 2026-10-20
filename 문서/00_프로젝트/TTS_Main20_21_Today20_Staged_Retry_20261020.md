# LIMITLESS TTS / 2026-10-20 최대20회 Python 제작 및 단계별 청취 게이트

**USER_APPROVED_UP_TO_20 / PYTHON_SCRIPT_READY / OFFLINE_TEST_PASS / GOOGLE_API_REAL_CALL_0 / USER_PC_EXECUTION_PENDING / UNITY_0**

## 바울 대표 지시

“혀여간, 오늘 20회 써보자고..” 2026-10-20(한국시간) **최대 20회** 제작 준비. **키1=최대10회, 키2=최대10회**는 두 키가 별도 Google Project에 속하며 `gemini-3.8-flash-tts`의 실제 무료 잔여량 및 과금 위험이 없다는 확인을 전제로 한다. 키 수가 아닌 Project+Model quota를 엄격히 따른다. 실제 사용 가능량 및 Billing/리셋 시각은 이 ChatGPT가 독립 검증하지 않았다.

### 실제 신규 Python 패키지 (TTS 담당 직접 제작)

- `LIMITLESS_TTS_Today20_20261020.zip`: **12,448 bytes**, SHA256 `7e2c80cc26f6127dbc22075e975959afab4246aa66f57ef6696e2b7c747ae635`. 대화에 다운로드 링크 직접 제공.
- 내부 `LIMITLESS_TTS_Today20_20261020.py` SHA256 `1576623009a73547467579a07bcb95ae1bdf59357ece48d14debcbd84d1bdc2e`, 정본 CSV SHA256 `6d951073665e12141fc3d2af9cfdc9e3df53f3a8dc351818493db8224652deff` 및 `README_KO.txt`.
- Python 코드에 비밀키/요금 결제 설정/Google 자동 호출 로직 없음. 실제 API Key는 사용자의 PowerShell에서 환경변수 또는 비표시 직접 입력. `check`는 외부 요청/쓰기 0.
- `F:\Downloads\Voice\LIMITLESS_TTS_20_20261020` 압축 해제, Python 경로에서 `check` → `sample --confirm-free`(2호출) → 사람이 직접 두 WAV 청취 → `approve-samples` → `main --confirm-free`(9호출) → 사용자가 실제 FAIL 확인한 ID에만 `retry --ids <ID들> --confirm-free`(최대9호출).
- 결과는 `F:\Downloads\Voice\Limitless_TTS_20Calls_20261020\round1` / `round2`로 격리. 이전 `Limitless_TTS_Main20_Main21_20261019` 원본17의 WAV/결과 해시 읽기 전용 검사, 스냅샷. Main18 승인10·Unity 정상25, 전역 Voice Registry, 모든 legacy HOLD/lock, Save 원본 보존.
- **기존 17회 제작의 요청 본문·스타일·Voice/Model·GenerateContent JSON 구조를 그대로 재사용**: 세린 `Schedar`, 태온 `Gacrux`, 모델 `gemini-3.8-flash-tts`; `speech_metadata.style`과 안정적인 Stable Dialogue ID/ExactText 고정. 성별 강제 Style, Erinome/Alnilam 보이스 교체, DSP 피치·포먼트 보정 적용 0.
- 이전 오류 9개: 세린8 + 태온1. 그 외 미제작 Main21 두개: `main21_haren_taeon_02`(태온) / `main21_prepare_serin_01`(세린). 하렌 자체 3개 Voice 미승인 BLOCK. 플레이어·지문 생성0.

## 분배

| 단계 | Key1 | Key2 | 합계 | 게이트 |
|---|---:|---:|---:|---|
| 샘플 | 2 | 0 | 2 | 기존과 동일한 세린/태온 각1. 직접 청취 |
| 본 작업 | 8 | 1 | 9 | 두 샘플 모두 청취 합격 시, 불량 세린 나머지7 + 미제작2 |
| 조건부 재시험 | 0 | 최대9 | 최대9 | 위11개 중 실제 청취 FAIL만 ID별1회 |
| 총 상한 | **10** | **10** | **20** | 불합격이 없으면 예비 사용하지 않음 |

## 오프라인 안전 시험 결과

- `python -m py_compile` PASS.
- 가짜 원본 17 WAV/결과 SHA와 해당 ID/보이스/원문 정합 검사 PASS.
- 가짜 Gemini 요청: sample2 → 승인 전 main 자동 차단 → 사용자 승인 모의 → main9 → retry9, 성공 총20/Key1 10/Key2 10 PASS.
- 사용자 제공 원본(가짜 격리 fixture)의 SHA 불변, 새 후보를 separate round1·round2에만 작성, 기존 결과 WAV 해시 변조 후 preflight 차단 PASS.
- 가짜 Gemini 2번째 요청 오류 모의: 첫 WAV 보존, HOLD+RUNNING.lock 보존, 이후 호출0, 재실행 차단 PASS.
- API 요청 사이 26초 간격, ID별 독립 WAV, 모든 호출 사전 기록 및 사용자 승인을 받은 뒤만 생성. 실패/429/결과 불확실 시 즉시 전체 중단, 자동 재시도/다른 키 전환 없음.
- **실제 사용자 PC/Google Project/생성 음질 미검증**. 오프라인 mock PASS는 음색 청취 PASS가 아님. 사용자가 오늘 20회를 요청했지만 무작정 전량 소진하지 않음.

## 역할·승인

TTS 담당 ChatGPT: Python 스크립트와 계획/QA/인계. 바울 대표: PC에서 확인 후 Python 실제 실행 및 WAV 직접 청취. Codex: 사용자 승인 WAV만 나중에 Unity에 통합. 이번 수정은 `docs/tts` 브랜치 문서만; GitHub main commit/Push/merge0, Unity 변경0.
