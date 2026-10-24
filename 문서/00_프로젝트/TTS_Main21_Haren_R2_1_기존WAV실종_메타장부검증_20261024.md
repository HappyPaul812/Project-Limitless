# LIMITLESS TTS 하렌 01 / 기존 WAV 부재·메타 존재 시 R2 재시험 복구판 v2.1

**기록일:** 2026-10-24
**범위:** 사용자 직접 TTS 생성 / GitHub `docs/tts` 기술 인계, 원본 Unity/Save/WAV 무변경
**상태:** 사용자 콘솔 장애 검증 / R2.1 Python 패키지 제작 및 **모의 테스트 PASS** / 사용자 실제 재시험·청취 QA PENDING

## 1. 실제 사용자 증거

사용자는 `F:\Downloads\Voice`에서 직접 기존 `Haren_Main21_AfterRenewal_3.py run --max-calls 1`을 실행하여:
- `[API 1] main21_haren_haren_01 | Haren / Alnilam | '태온. 너 아직 멀었어.'`
- `[HAREN CREATED] main21_haren_haren_01 = user listening QA PENDING`
- `[RENEWAL READY] 152/152`, `[HAREN READY] 1/3`, `[PT USAGE SHARED] 2026-10-24 UTC-7 attempted=9/40`, `HOLD=False`, `PROTECTED OLD7=7/7`.
- 사용자 직접 청취: **첫 대사가 또 의문문 상승 억양으로 생성됐으므로 QA FAIL**. 나머지 하렌02/03 API 생성 중단 지시.

이후 기존 `Haren_Prosody_Retake_R2.py check`가:
- `[RENEWAL] 152/152; Haren first WAV exists=False meta=True`
- `[R2 CANDIDATE] wav=False meta=False`
- `[QA APPROVED] False; [HOLD] False`
- `[SHARED PT USAGE] 9/40`
- `[PROTECTED MAIN21] 7/7 kept`
로 보고. `retake` 두 차례 모두 `[STOP] Previous Haren first WAV/meta missing. Stop to inspect.`으로 **API 추가 POST0**.

**진단:** 기존 하렌 첫 대사의 설정 경로 `F:\Downloads\Voice\Haren_Main21\Haren\main21_haren_haren_01.wav`는 현재 **부재**, `main21_haren_haren_01.json`은 **존재**. 이전에 원본 WAV를 직접 들어봤다는 사실과 파일 현재 위치는 별개. 사용자가 음성을 삭제했는지/이동했는지/다른 방식으로 빠졌는지는 모른다. 원본 WAV나 기록이 이 도구 때문에 삭제됐다고 주장하지 않음. 기존 R2는 두 파일이 모두 있을 때만 생성/승인하도록 설계되어 있어 이 상황을 예상대로 차단한 것.

## 2. 신규 복구판

**패키지:** `LIMITLESS_Haren_Prosody_Retake_R2_1_OrphanMetaFix_20261024.zip`
- SHA256: `679b825293f19a6524bfae62dbcc39f9d98bca777e72ccc7d16f2a36826c345d`
- ZIP 7737 bytes / 내부 `Haren_Prosody_Retake_R2_1.py`(14757 bytes) 및 `README_Haren_Prosody_R2_1_KO.txt`. 실 API Key 및 WAV 제외.
- 이전 R2/하렌 생성기 덮어쓰기 없음. 사용자 직접 `F:\Downloads\Voice`에 압축 해제 후 `R2_1.py`를 실행해야 한다. Python 코드 자체는 대화 패키지에 제공되었으며 이번 GitHub 기록만으로 사용자 PC에 설치되지 않는다.

### R2.1 변경 범위

- 기존 WAV가 없어도 **원본 JSON**의 `dialogue_id, speaker_id, voice, text_sha256, wav_sha256`가 정본 `main21_haren_haren_01`, `story_haren`, `Alnilam`, `태온. 너 아직 멀었어.`와 일치하는지 검사.
- **공유 append-only 이벤트 장부** `F:\Downloads\Renewal_Voices\_progress\api_events.jsonl`에서 동일 `dialogue_id`, `Haren`, `Alnilam`, `wav_sha256`의 `SUCCESS` 기록을 확인. 양쪽 증거가 일치하지 않으면 생성 API 요청 전에 **STOP**.
- 원본 WAV가 있으면 원본 엔진 `completed`와 바이트 SHA 검사도 그대로 수행. 없으면 **기존 JSON/성공 기록은 그대로 둔 상태**에서 신규 R2 후보 생성을 1회만 허용하며 원본을 없던 일로 처리하지 않는다.
- `R2_1.py retake`는 이전 R2 같은 짧은 한국어 하강운율 지시, 원문 및 목소리 고정. 사용자 `RUN_RETAKE_01` 승인, 기존 프로젝트별/태평양 날짜 공유 quota 및 HOLD/LOCK 전부 유지.
- R2 출력은 별도 `F:\Downloads\Voice\Haren_Main21\ProsodyRetakes\R2\Haren\main21_haren_haren_01.wav`(공식 원본 또는 이전 bad WAV 덮어쓰기 금지).
- 실제 사용자가 **끝이 내려가는 평서문**으로 듣고 `APPROVE_FALLING_R2` 명시 승인한 경우에만 기존 원본 메타데이터 백업(원래 WAV 존재 시 WAV도 SHA 검증해 백업), R2 WAV·JSON을 공식 위치에 복사해 메타·SHA를 재검증하며 USER AUDIO QA 마커를 쓴다. 원본 WAV 부재 시 없던 원본 WAV를 가짜로 만들어 백업한 척하지 않고 **원본 WAV 누락 사실을 보관**.
- R2도 의문문이면 `reject-r2`로 QA 거절 표시·증거 보존, **하렌02/03 계속 HOLD**. 승인 없으면 `remaining` 명령으로 나머지 2개 제작 불가. 기존 `Haren_Main21_AfterRenewal_3.py run` 직접 사용 금지(청취 QA 승인 게이트가 없음).

### 사용자 직접 복사 명령

```powershell
cd F:\Downloads\Voice
python .\Haren_Prosody_Retake_R2_1.py check
python .\Haren_Prosody_Retake_R2_1.py retake
```

R2를 직접 청취해서 질문형이면:
```powershell
python .\Haren_Prosody_Retake_R2_1.py reject-r2
```

실제 청취 합격시에만:
```powershell
python .\Haren_Prosody_Retake_R2_1.py approve
# APPROVE_FALLING_R2 직접 입력
python .\Haren_Prosody_Retake_R2_1.py remaining
```

읽기 전용 기존 WAV 탐색:
```powershell
Get-ChildItem -LiteralPath 'F:\Downloads\Voice' -Filter 'main21_haren_haren_01.wav' -File -Recurse | Select-Object FullName, Length
```

## 3. 검증, 보존, 후속

- 패키지 원본 컴파일 `py_compile` PASS, ZIP CRC 및 내부 파일 목록 검증 PASS.
- **오프라인 가상 디스크/JSON journal 테스트:** ① WAV 없음 + 정확 JSON+SUCCESS 교차검증 → mock Google 요청 최대1 → R2 후보 생성, ② 사용자 R2 합격 승인시 기존 sidecar 백업 후 공식 파일 1개 반영, ③ 승인 마커 확인 후 하렌 나머지 허용, ④ SHA 안 맞는 JSON은 fail-closed 차단. **실제 Google POST0**, 사용자 F: 변경0, Unity 변경0.
- 사용자 화면의 현재 API 요청은 **9/40**; 이 오류로 +0. 실제 Google 무료 quota/과금/프로젝트 소속은 프로그램 카운트만으로 보증 불가.
- 별도 Main21 사용자 KEEP 레온2/미엘2/폴3 WAV 7/7 존재 **화면에서 확인(원본 실제 음질/SHA는 추후 검사)**. 새 TTS 제작 불필요, 원본 보호.
- [기존 하렌 R2 억양 불합격 기록](TTS_Main21_하렌01_의문문재발_교정_R2_20261024.md)
- [세린·태온 합성완료 및 TTS 최신 인계](팀운영/인계/TTS_LATEST.md)
- [하렌3 제작 절차](TTS_20261024_Post152_Haren3_VoiceFolder_SharedQuota_Runbook.md)
- [Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20)

**차기 보고 전까지 R2 청취 QA PENDING, 하렌02/03 신규 생성 보류, Unity 및 main 반영 금지.**
