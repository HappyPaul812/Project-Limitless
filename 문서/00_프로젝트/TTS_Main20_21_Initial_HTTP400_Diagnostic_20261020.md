# LIMITLESS TTS / 2026-10-20 최대20회 제작 첫 요청 HTTP400 진단

**USER_SCREEN_OBSERVED / KEY1_FIRST_POST_HTTP400 / FIRST_CALL_FAILED / SUCCESS_NEW_WAV=0 / TAEON_NOT_CALLED / HOLD_LOCK_PRESERVED / NO_NEW_LIVE_API_BY_TTS_SESSION**

## 바울 대표 실제 PowerShell 화면

1. `LIMITLESS_TTS_Today20_20261020.py check`는 `[CHECK PASS]`: 원본/기존 출력/로그 SHA 검증, API 0 / Unity 0.
2. 같은 스크립트 `sample --confirm-free` 수행: KEY1 Google Project 및 Gemini 3.8 TTS 무료 잔여10, FREE 입력, YES 실행.
3. 첫 요청 `main20_confront_serin_01`, Voice `Schedar`, `gemini-3.8-flash-tts` → **HTTP 400**, 출력 `[STOP]`. 기존 `HOLD.json` 및 `RUNNING.lock` 생성/유지, 뒤 태온 호출0, 신규 성공 WAV0.
4. 사용자가 즉시 다시 `sample --confirm-free` 호출했으나 `[BLOCKED] 기존 HOLD/lock 존재`로 거절. 이는 **정상적인 중복 호출 방지 동작**.

## 원인 평가: HTTP400 정확한 사유 미확정

- 2026-10-20 TTS 담당이 기존 제작 Python `LIMITLESS_TTS_Main20_Main21_17.py`와 신규 `LIMITLESS_TTS_Today20_20261020.py`의 모델/Voice/Style 및 실체 REST Payload를 정적으로 대조; 동일한 `parts[].speech_metadata.style`, `generationConfig.responseModalities`, `speechConfig.voiceConfig.voice` 형식임을 확인.
- Google [Gemini 3.8 TTS 공식 Legacy GenerateContent 가이드](https://ai.google.dev/gemini-api/docs/generate-content/speech-generation)는 위 요청 구조를 실제 공식 예제로 지원.
- API가 400을 반환한 **본문 `error.message`를 기존 스크립트가 출력/저장하지 않은 것이 담당 스크립트의 진단 로그 결함**이다. 따라서 400이 모델·Content 정책·지원 보이스·사용자 입력·키프로젝트·백엔드 등 어떤 이유인지 **정확히 판정 불가능**. 곧바로 Key 변경/재시도/원본 잠금 삭제 금지.
- 원본 첫 시도는 최대 20회 예산에서 **최소 시도1**, 실제 Google Project+Model 잔량에서 차감되는지는 계정 대시보드에서 확인 필요. 보수적으로 KEY1 사용1로 계산.

## TTS 담당 직접 제작 — 복구 없는 격리 오류 진단

- 사용자 다운로드 ZIP: `LIMITLESS_TTS_HTTP400_Diagnose_20261020.zip`, SHA256 `d1270b807435d8f222e5a976ba00b65c133ea5fa19fb800840b87c645296815a`.
- 포함 `LIMITLESS_TTS_400_Diagnose_20261020.py` SHA256 `2b459b3fd6475f316cbb1f97e031bf2026f2be731355333047195bb2fec90333` 및 `README_TTS_400_KO.txt`. TTS 세션에서 직접 작성, 사용자에게 ZIP 배포.
- 기존 실행 폴더 `F:\Downloads\Voice\LIMITLESS_TTS_Today20_20261020`에 **스크립트만 추가**(기존 원본 Python/CSV 절대 대체 금지).
- `inspect`: 원본 WAV/결과 스냅샷·실패 당시 `HOLD.json`, `RUNNING.lock`, `attempts.jsonl`, 원문·Payload SHA를 **읽기 전용/Google API0** 비교.
- `model-info`: 같은 KEY1로 Google 모델 메타데이터를 **GET 1회**, TTS generateContent POST **0회**. 모델 존재와 공개된 supportedGenerationMethods 확인. 모델 조회가 성공해도 음성 생성 승인은 보증하지 않는다.
- `diagnose-one`: *사용자와 TTS 담당의 차후 별도 확인 후에만* 동일 `Schedar` 보이스/대사/JSON으로 **독립 폴더에서 정확히 POST 1회**, HTTPError 응답의 `error.code`, `error.status`, `error.message`를 비밀키 마스킹 후 `F:\Downloads\Voice\Limitless_TTS_Today20_400Diagnostic_20261020\result.json`로 안전 기록. 성공 시 격리 WAV1개 청취 대기. 원본 HOLD/lock 절대 해제/삭제하지 않음. 중복 진단 호출 차단, 실패 즉시 중단.
- 실사용 안내:
  ```powershell
  cd "F:\Downloads\Voice\LIMITLESS_TTS_Today20_20261020"
  python .\LIMITLESS_TTS_400_Diagnose_20261020.py inspect
  python .\LIMITLESS_TTS_400_Diagnose_20261020.py model-info
  ```
  우선 이 두 실행 결과만 공유. `diagnose-one`은 무료 잔여량과 결제 여부 확인 + 별도 승인 후.
- 오프라인 통합 모의: 기존 원본17 SHA 및 HOLD/lock/시도1 보존, `inspect` 파일쓰기0, 모델 메타 GET만 호출, 단발 POST가 400을 반환할 때 `INVALID_ARGUMENT`과 메시지 캡처·키 마스킹, 추가 호출0, 독립 진단 폴더 중복 방지 PASS. **실제 사용자 PC 모델 GET, POST 실행0** (로컬 Python 수행 대기).
- Python 코드에는 어떤 비밀키도 포함하지 않음. 실제 키 입력/Environment read만, 로그/파일에 저장 금지. 해당 오류 상세 JSON 공유 전 개인정보/키 문자열 확인.

## 보호 및 다음 단계

기존 `HOLD/lock`, 원본17 WAV, Main18 세린 승인10, 기존 정상 Main18/19 음성25, 로컬 Save/Unity, 다른 담당 브랜치 모두 불변. 오늘 20회 재제작 프로세스는 **원인 확인 전 BLOCKED**. 오류 원인을 파악한 후에만 실제 무료 quota에 맞춰 재계획하고 사용자 승인을 얻어 진행한다. TTS Python 개발/진단 담당은 이 세션, Codex는 청취 승인 이후 Unity 연결만. `docs/tts` 브랜치만 Git 문서화, main push/merge0.


## 2026-10-20 후속 사용자 실제 진단 로그 — API KEY INVALID 확인

- 사용자가 진단 `inspect` 실행: `[CHECK PASS]`, 기존 HOLD/lock 및 원본 WAV/요청 SHA 일치, 기존 1회 HTTP400, 성공 신규 WAV0, 이후 요청0. 초기 스크립트가 HTTP 응답 원문을 저장하지 않아 첫 실패 원인 자체는 복원 불가.
- 이어 `model-info` 모델 메타데이터 GET을 **2회** 실행. 양쪽 모두 HTTP400 / `INVALID_ARGUMENT`, `API key not valid. Please pass a valid API key.` 반환. 해당 GET은 음성 생성 요청이 아님.
- **확인된 직접 장애:** 진단 시 입력한 KEY1이 Google API 인증에서 거절됨. 따라서 KEY1의 유효성/활성 상태/복사 완전성/Google AI Studio 발급 내역을 점검해야 함. 사용자가 이전 TTS 샘플 호출에 입력한 키와 이 진단 입력 키가 동일했는지는 현재 로그만으로 확정하지 않음.
- 프로젝트 ID(`gen-lang-client-...`)와 실제 API Key를 혼동하지 말 것. 비밀키는 채팅·문서·로그·스크린샷에 노출하지 말고, 실제 `https://aistudio.google.com/api-keys`에서 확인한 Key로 `model-info` 재검증. `model-info` PASS 확인 전 `diagnose-one`/`sample`/HOLD 삭제/키 임의 변경에 의한 재생성 금지. KEY1 유효해진 뒤 기존 HOLD를 보존한 상태로 원인별 후속 결정.
- 실제 새 WAV0, 사용자 로컬 HOLD 보존, Unity0, GitHub main 변경0. 모델 음색 불안정 문제와 이번 명확한 API 키 인증 거절은 **별도 이슈**로 구분.


## 2026-10-20 추가 화면 — 유효 API 키로 모델 조회 성공

- 같은 `model-info` 진단에서 최초 두 번은 HTTP400 `API key not valid` 반환, **이후 사용자가 다시 입력한 키로 `[MODEL GET OK] generation request 0`, `models/gemini-3.8-flash-tts`, methods=`generateContent,countTokens,batchGenerateContent` 출력**.
- 그러므로 **최종 입력 키는 Google 모델 메타데이터 조회에 사용 가능한 유효한 인증키임을 확인**. 최초 TTS POST HTTP400의 상세 본문은 당시 기록되지 않아 그 원인 자체는 여전히 미확정. `MODEL GET OK`가 실제 음성 `generateContent` 성공을 보장하지는 않음.
- 이후 사용자가 기존 `sample --confirm-free`를 시도했으나 기존 `HOLD/lock`으로 `[BLOCKED]` 처리됨. 의도된 중복 재호출 방지; HOLD를 수동 삭제하지 말 것.
- **다음 권장 단계는 이미 만든 독립 진단 스크립트의 `diagnose-one` 정확히 1회 실행**(Google Project+모델 실제 무료 잔여량 확인, 해당 성공한 키 재사용, `FREE`·`DIAGNOSE` 수동 승인). 기존 `HOLD/lock` 불변, 독립 폴더에 1회 요청 결과+HTTP 상세 또는 1개 청취용 WAV를 보존하고 자동 후속 호출0. 
- `diagnose-one` 실행 결과가 오기 전 기존 `sample` 재실행/lock 삭제/다른 키로 무작정 재호출 금지. 실제 추가 TTS 생성 호출0 상태에서 화면 판독만 수행. 기존 Unity 및 WAV 변경0.
