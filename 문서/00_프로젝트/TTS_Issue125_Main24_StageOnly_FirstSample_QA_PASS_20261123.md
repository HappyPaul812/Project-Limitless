# LIMITLESS TTS / Issue #125 / Main24 첫 STAGE WAV 생성 및 사용자 청취 PASS

**날짜:** 2026-11-23  
**브랜치:** `docs/tts`  
**상태:** `MAIN24_STAGE_1_OF_10_CREATED / USER_LISTENING_QA_PASS_REPORTED / LOCAL_QA_MARKER_PENDING / RUNTIME_ID_0_OF_10 / EXISTING_162_KEEP`

## 사용자 실제 로컬 실행·청취 결과(사용자 제공 PowerShell 출력과 직접 청취 확인)

- 사용자 실행: `python .\Chapter3_Principal72_STAGING_ONLY.py run --max-calls 1`; 승인 문구 `RUN_MAIN24_1` 직접 입력.
- 프로그램 출력: `[READY - STAGING ONLY, NOT UNITY] Main24 stage items=10, free cap permits at most 1 POST(s).`, `[API 1]` 및 `[CREATED]` 확인. **실제 합성 성공 1건, Main24 stage WAV ready 1/10(로그상 신규 생성, 사용자 PC의 파일 자체는 이 세션에서 직접 읽지 않음)**.
- 대사 후보 키 `main24_serin_001`, 스테이징 발화 ID `ch3stage_main24_serin_001` (**정식 Unity Runtime ID 아님**). 화자 `companion_serin`; 승인 목소리 **Erinome**. 원문 정확히 **`첫눈이네요. 장갑을 더 챙기길 잘했어요.`**
- 사용자 출력에서 확인한 음성 WAV SHA256: **`6bf00d66d0b230cb339d0a68079272a9fc2f1510f254a0cff365751b53f14464`**. **로그 전사값**이며 GitHub/ChatGPT가 로컬 WAV 파일을 직접 해시하여 검증한 값은 아님. 엔진 청취 승인 작업에서 WAV bytes의 실제 해시와 비교 예정.
- 예상 저장 경로: `F:\Downloads\Voice\Chapter3_StageOnly_72\Serin\ch3stage_main24_serin_001.wav`. 실제 파일 존재·sidecar 확인은 사용자 로컬 `approve` 프로그램이 수행.
- 사용자 직접 답변 **`들었어. 좋아.`** → **첫 샘플 청취·음색/발화 합격(사용자 보고) PASS**. Python 생성 스크립트의 `MAIN24_STAGE_SAMPLE_APPROVED.json` 기록은 아직 명령 실행 결과를 받지 못했으므로 **PENDING**. 다음 명령 `python .\Chapter3_Principal72_STAGING_ONLY.py approve` 후 `APPROVE_MAIN24_SAMPLE` 입력. 이 명령은 1개 기존 Stage WAV SHA를 검증하고 QA 마커 파일만 생성, **Google POST0**.
- 요청 시점 `PT=2026-11-23 UTC-8`, 이전 공유 로컬 장부 0/40. 이번 성공 출력은 실제 API 1 POST를 뜻함; **최신 공유 journal의 실제 시도 누계는 아직 재조회하지 않았으며 1/40이라고 단정하지 않음**. 금일 1회 무료 요청 허용 증거만 확인됐으므로, **QA 마커 등록만으로 추가 1~3회 API 요청 승인으로 확대 해석 금지**. 다음 실행 전 프로그램 `check`로 최신 PT 장부/QA 상태 확인 및 사용자 본인의 실제 Google 모델 무료 잔여량/외부 프로젝트 사용량을 다시 확인하여 `PT_QUOTA_VERIFIED.json`의 `max_new_posts_confirmed_today`를 실사용 총량 기준으로 증설해야 함.
- Main24 나머지 **9행 대기**. Main25~29 **62행 원문/기존 캐스팅 잠금 유지**. 일반 NPC 21 텍스트, 마레2 검토만.
- 이전 Codex 감사 상태 계속 `STAGED_ID_PLAN / 실제 Runtime 연결 0/10`; 590 WAV 경로 중복 여부 미검증 위험은 별개로 기록, 승인 WAV162개 절대 변경 금지. 이번 생성 성공은 Unity StoryVoiceCatalog 적용과 무관하고 **Unity6000.6.5f1/Save/main/기존 WAV162 변경·병합0**.
- **다음 단계:** (1) 사용자가 `approve` 완료 출력 확인, (2) 현재 PT 무료 API 잔량/외부 사용 실확인, (3) `check`와 QA SHA마커 확인, (4) **기존 성우/음색 유지**하며 1~2건만 소배치 `run --max-calls 2`·`RUN_MAIN24_BATCH` (해당 횟수의 실제 무료 호출이 확인된 경우에만), (5) 생성 직후 검수·SHA/사용량 기록. 429 즉시 HOLD, 재시도·자동 키 전환 없음.

**소스:** [Issue #125](https://github.com/HappyPaul812/Project-Limitless/issues/125), [스테이징 파일럿 안전 정책](TTS_Issue125_Main24_STAGED_ID_PLAN_0of10_제작분리_20261123.md), [72개 대사 정본](https://github.com/HappyPaul812/Project-Limitless/blob/docs/tts/Tools/TTS/Chapter3_Protagonist72_ApprovedQueue_20261123.json). 실 음성 파일 자체를 GitHub에 올린 것은 아님.
