# LIMITLESS TTS — 세린 동일 Voice Schedar 재시험 1~2회 (2026-10-20)

**USER_WANTS_RETRY / TWO_STAGE_SCRIPT_PREPARED / REAL_GOOGLE_CALLS_THIS_TURN_0 / UNITY_0 / ORIGINALS_PROTECTED / USER_LISTENING_PENDING**

바울 대표 요청 “18회 버리더라도 또 뽑아봐?”에 대해 **18회 자동 연속 호출 대신 기존 음성 설정의 1회 시험 후 청취 게이트**로 진행한다. 두 번째 호출은 첫 시험 청취 후 `FIRST_FAIL` 입력을 요구한다. 세린은 이전 `Schedar` 요청 여러 번에서 남성처럼 나와 반복 성공을 보장할 수 없다. 실제 18회 무료 사용 가능 여부/두 키 Project별 한도는 미확인, 스크립트 자체는 최대 2회만 호출한다.

## 제공한 직접 제작 Python 패키지

- 다운로드 `LIMITLESS_TTS_Serin_Retry2_20261020.zip`, 279,734 bytes / SHA256 `6369b14fe7032046111f8fe4f4296e0a6cadf8633299bc579d128bf22fee4180`.
- Python `LIMITLESS_TTS_Serin_Retry2.py` SHA256 `086204750bf2d05e0d16995e4f32f14072b8f097a402cb6b84fb8b433a79353b`.
- 대사 정본 CSV SHA256 `6d951073665e12141fc3d2af9cfdc9e3df53f3a8dc351818493db8224652deff`.
- **바울 대표 승인 비교 원본** `APPROVED_main18_ground_serin_01.wav` SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e` 포함. 이 WAV는 해시 확인/청취 비교용이며 Google 요청으로 업로드하지 않는다.
- `README_KO.txt`에 실행·보호 원칙 명시.

## 실행 방식

압축 해제 경로: `F:\Downloads\Voice\LIMITLESS_TTS_Serin_Retry2_20261020`.

```powershell
conda activate py3_12
cd F:\Downloads\Voice\LIMITLESS_TTS_Serin_Retry2_20261020
python .\LIMITLESS_TTS_Serin_Retry2.py check
python .\LIMITLESS_TTS_Serin_Retry2.py trial --attempt 1
```

- 모델 `gemini-3.8-flash-tts`, Voice `Schedar`, 스타일 원본 `Measured, observant Korean dialogue, natural intonation and clear diction.`, 원문 `main20_confront_serin_01` 정확히 보존.
- API 키는 환경변수 또는 마스킹 입력. 생성 전 모델 GET으로 유효키 확인(생성0), Google Project+Model 무료 잔여 확인은 사람이 입력하고 `FREE` 및 `YES` 승인 필수.
- 첫 호출 성공 WAV는 `F:\Downloads\Voice\Limitless_TTS_Serin_Retry2_20261020\main20_confront_serin_01__attempt01.wav`에 별도 저장, 청취 전 상태 `SAVED_USER_QA_PENDING`.
- **두 번째 호출은 자동이 아니라**, 사용자가 첫 파일을 청취하고 불합격 시에만 `trial --attempt 2` 실행 후 `FIRST_FAIL` 확인. 결과 별도 저장. 같은 번호 중복 생성 차단.
- 처음 API 실패/HTTP429/결과 불확실은 별도 출력 디렉터리에 HOLD+RUNNING.lock 보존, 자동 retry0. 이전 20회 스크립트의 잠금, 원본17, 승인 Main18 10, Unity/Save 불변.
- Python compile/오프라인 mock GET+POST1/2, 기존 SHA 불변, 중복 호출 차단, 두 번째 mock 단계 실패 시 HOLD/lock+후속호출0 PASS. 실제 Gemini 호출·실제 결과 음질 검증0.

TTS 제작/QA는 TTS 담당, 바울 대표 PC에서 Python 실행. Codex는 승인된 WAV의 Unity 통합만. GitHub `docs/tts` 문서만 수정, main 직접 커밋/푸시/병합 없음.
