# LIMITLESS TTS / Issue #125 — Main24 `STAGED_ID_PLAN` 검수와 신규 WAV 선제제작 분리 옵션

## 2026-11-23 사용자 Stage-only 방식 채택 및 첫 read-only 체크

- 사용자 **"그래"**로 **"게임 구현을 기다리지 않고 임시 WAV 먼저 제작"** 방식을 명시 승인. 기존 `STAGE_ONLY_OPT_IN_PENDING` 상태는 `STAGE_ONLY_METHOD_APPROVED`로 갱신. 다만 사용자가 로컬 Codex 보고서와 590경로 미확인 중복 위험을 직접 확인하고 승인 JSON에 기록해야 한다는 스크립트의 증거 게이트는 그대로 유지. 현재 첫 유료/무료 API POST 승인은 아직 없음.
- 사용자가 실행한 `Chapter3_Principal72_STAGING_ONLY.py check`의 실제 출력: `[QUEUE] 72/72 valid`; Runtime ID0/10, 기존162 보호; `[STAGE AUTHORIZATION PENDING] ... codex_report_examined_by_user=false`; `[SHARED LOCAL LEDGER] PT=2026-11-22 UTC-8, tried=0/40`; `[QUOTA VERIFICATION PENDING] ... Quota proof not for current PT 2026-11-22 and model`; `[NO POST]` 확인. 오류가 아닌 fail-closed 정상 상태.
- 체크 시점이 한국 오후5시 전으로 **현지 PT=전날(11/22)**이 맞다. 17:00 KST부터 11/23 PT로 전환. 승인서의 `pt_date`는 **재실행 때 프로그램이 실제 출력한 현재 PT 날짜**와 일치해야 하며 이전 날짜 확인을 재사용하거나 `0/40`을 실제 Google 무료잔량으로 간주 금지.
- 사용자 로컬 안내: `F:\Downloads\Voice\Chapter3_StageOnly_72\Main24_STAGE_AUTHORIZATION.json`에서 실제 Codex **근거 보고서 파일 실경로**를 `codex_report_path`에 기재하고 보고서의 Runtime0/10 및 590 WAV 중복 미확인을 검토한 다음 5개 true 설정. `real_runtime_id`는 null로 유지. `PT_QUOTA_VERIFIED.json`은 해당 실행 날짜와 기존 모델 `gemini-3.8-flash-tts`의 실제 무료사용잔량/4키 실 프로젝트 소속/외부 사용 누계/무과금 상태를 사용자 스스로 확인한 경우만 5개 확인값 true와 `max_new_posts_confirmed_today` 실제 허용량(첫 샘플 1이면 1)을 기록.
- 두 게이트 통과시 `check`→`run --max-calls 1`, 사용자 `RUN_MAIN24_1` 승인. 429 자동 retry/failover OFF, 한 WAV만 Stage-only 폴더에 생성. 사람 청취 합격 전 소배치 중단.
- **보고 현재상태:** Stage-only 제작 방식은 승인됨, 필수 실증자료/현재 PT 원격 무료quota 확인 대기, 실제 API0/새 WAV0/생성파일 SHA 없음. 기존 WAV162/Unity6000.6.5f1/Save/main 유지.


**기록 날짜:** 2026-11-23  
**담당 브랜치:** `docs/tts`  
**실제 상태:** `RUNTIME_ID_0_OF_10 / STAGED_ID_PLAN / APPROVED_162_KEEP / 590_DUPLICATE_UNVERIFIABLE / API_POST_0 / NEW_WAV_0 / STAGE_ONLY_METHOD_APPROVED; EVIDENCE_AND_PT_FREE_QUOTA_PENDING`  
**근거:** 사용자 Codex 완료 스크린샷, [Issue #125](https://github.com/HappyPaul812/Project-Limitless/issues/125), [72개 승인 발화 Manifest](https://github.com/HappyPaul812/Project-Limitless/blob/docs/tts/Tools/TTS/Chapter3_Protagonist72_ApprovedQueue_20261123.json), [기존 Main24 10/10 HOLD 기록](TTS_Issue125_Main24_10_RuntimeID_전체HOLD_후속조치_20261123.md).

## 1. Codex 최신 감사 결과 — 이것은 실패가 아니라 정상 보호

- 현재 Main24 북문·관찰 단계는 **탐험 후보 흐름**이고 올빼미 전투는 **선택형 QA 조우**. 승인 주요 인물 대사 10개의 실제 정식 발화/조사/승리 트리거가 아직 존재하지 않아 Codex가 **원본 Runtime을 변경하지 않고 `STAGED_ID_PLAN`**으로 남겼다.
- **실제 Runtime 연결 0/10**, `VERIFIED_NEW=0`, `VERIFIED_KEEP_EXISTING=0`, `HOLD_RUNTIME_ID=10`, `reviewed_by_user=false`. 기존 탐험/안내 ID 12개 보존. 후보 발화 ID를 Runtime ID로 허위 승격하지 않는다.
- 기존 WAV 941경로 중 이전 메타 완비348은 중복 확인 가능, 나머지 **593경로 추가 조사 후 새로 확정된 것은 3경로**, **590경로는 여전히 미확인**. 단순 파일명 단서 8경로는 중복 판단 근거가 아니다.
- 기존 승인 **WAV 162/162** SHA·화자·GUID 일치, StoryVoiceCatalog 312개 ID 중복0. Unity `6000.6.5f1` 격리 컴파일·메인24 원래 탐험 후보 ID·대사 QA **17 PASS / 0 FAIL**. 선택형 올빼미 전투는 현재 사용자 화면 증거상 전체 정식 스토리 트리거로 통합하지 않음.
- Codex 보고 worktree `codex/main24-tts-dialogue-contract-20261123`, 로컬 커밋 `4602c7817587d051484a67769918e189178fda0a` (**사용자 스크린샷 전사; GitHub 원격에 올라온 SHA라고 단정하지 않음**). TTS·WAV·Save·원본 `main` 변경0.

## 2. 제작 지연 방지 방안 — Stage-only WAV 1개 시범은 `옵션`, 현재 미승인

**서로 다른 두 문제를 분리**한다.

1. **정식 Runtime 적용:** Story/Quest/Scene에 정식 대사 발화 트리거가 실제 생길 때까지 `HOLD_RUNTIME_ID`을 유지한다. 기존 `Main24_RuntimeAudit.json`에서 `VERIFIED_NEW`를 허위로 true로 바꾸지 않는다. Codex의 이전 17 PASS 결과나 Stage 생성은 이 항목을 통과시키는 증거가 아니다.
2. **음성 원본 미리 제작:** 이미 11/19 승인된 **주요 5인 정확 원문과 VoiceID**만 `STAGE_AUDIO_ONLY_NOT_RUNTIME`라는 임시 ID 아래 다른 폴더에 녹음하는 선택지. 10개 중 **1개 청취 파일럿** → 합격·사용자 단계별 승인 후 소배치. 추후 실제 Runtime ID가 결정되면 `candidate_id + speaker_id + text_sha256 + voice + wav_sha256`를 기준으로 검수·매핑하여 재생성 없이 Unity 연결 가능. **이 음성 파일을 Runtime에 연결했다고 주장하지 않는다.**

**왜 새 동의가 필요한가:** 이전 파일럿은 `VERIFIED_NEW` + 전체 중복조사가 끝난 경우만 요청하게 설계되어 있었다. 지금의 Stage-only는 **593중 590경로 중복 여부 미확인 상태에서 임시 WAV를 먼저 만드는 다른 진행 방식**이며, 불필요한 중복 API 호출 가능성을 0으로 증명할 수 없다. 따라서 **사용자 본인이 이 위험과 정식 Runtime 분리 원칙을 확인하여 명시적으로 동의하기 전까지는 신규 TTS 합성을 해서는 안 된다.**

### 독립 신규 파일럿 패키지

- **파일:** `LIMITLESS_Main24_StageOnly_TTS_Pilot_20261123.zip`
- **크기:** 16,607바이트
- **SHA256:** `36b4764ad645ecf296c7a1e3c8214b5d82d685c5687f315349169c10a5f097c7`
- **포함:** `Chapter3_Principal72_STAGING_ONLY.py`, `Chapter3_Protagonist72_ApprovedQueue_20261123.json`, `README_Main24_STAGING_KO.txt`, `test_ch3_staging_offline.py`.
- **설치 위치(사용자가 직접):** `F:\Downloads\Voice`에 이 4개 파일 추가. **기존 `Chapter3_Principal72_Pilot.py` 또는 `Renewal_Auto_TTS_Failover_v2.py`를 덮어쓰지 않는다.**
- **생성 대상 분리:** `F:\Downloads\Voice\Chapter3_StageOnly_72\Serin\ch3stage_main24_serin_001.wav` 같은 **임시 스테이징 ID**로 저장. 이 이름은 실제 Runtime ID가 아니며 다른 기존 파일이나 Unity로 자동 복사하지 않는다.
- **기존 승인 162 보호:** 모든 신규 WAV+sidecar는 `Chapter3_StageOnly_72` 하위. Google 키는 `F:\Downloads\Voice\API.txt` 및 기존 `Renewal_Voices\API.txt`를 **메모리에서만 동일한 키 세트인지 검사**하며 키값 출력/파일 재작성 금지. 기존 append-only PT 사용량 로그를 **공유**, 전체 로컬 하루40·그룹별10 안전상한 및 HOLD/LOCK·sha 검증 재사용. 429 자동 재시도/자동 키전환 **OFF**.
- **기본 게이트 모두 False, 보고서 실파일 존재 확인:** 새 `Main24_STAGE_AUTHORIZATION.json`의 본인 Codex 보고서 확인(실제 보고서 경로의 파일 존재까지 검사)/Runtime0/10 인정/**593경로 미확인 위험 인정**/Stage-only 합성 명시 승인/162보호 확인이 모두 true여야만 파일럿 합성 허용. `real_runtime_id`는 계속 null이어야 한다. **기존 RuntimeAudit의 reviewed_by_user=false를 무단 true로 바꾸지 않는다.**
- **별도 같은 PT 날짜 Google quota 확인:** 신규 `PT_QUOTA_VERIFIED.json`의 **그날 Gemini 무료 실제 잔량/모델/Key→Project ID·외부 호출 누계/유료 차단 사용자 검증**이 전부 true여야만 실행. 로컬 0/40이 원격 무료잔량 보장이 아님. 미국 태평양 표준시(PST) 11월에는 현지 날짜가 한국시간 오후5시에 바뀐다.
- **첫 WAV 생성:** *두 게이트 모두 확인된 경우에만* `python .\Chapter3_Principal72_STAGING_ONLY.py run --max-calls 1` → `RUN_MAIN24_1` 직접 입력. 첫 결과 청취 승인 전 나머지 추가 호출0. 합격 시 `approve`/ `APPROVE_MAIN24_SAMPLE`, 그 다음 최대 2개 소배치는 `run --max-calls 2` / `RUN_MAIN24_BATCH`.
- **기존 주연 캐스팅 보호:** 폴 Achird, 미엘 Sulafat, 세린 Erinome, 태온 Algieba, 하렌 Alnilam. 승인 72행 순서·원문·문장부호 그대로. NPC21 무음+마레2 검토, 플레이어/HUD6 무음, 기존 승인162 WAV/GUID 그대로.
- **계정·API 안전:** 무료 Gemini 모델·키 실제 확인 불가면 Google POST 금지; 유료 결제/새 Cloud TTS 활성화/비공식 gTTS 금지. `HTTP429`/결과불명 발생 시 실패 시도 보수적 1회 집계, HOLD·RUNNING.lock 보존, 수동 복구 전 자동 재호출0.

### 사용자가 Stage-only 방식을 선택했을 때의 무호출 준비 명령

```powershell
conda activate py3_12
cd F:\Downloads\Voice
python .\Chapter3_Principal72_STAGING_ONLY.py init
python .\Chapter3_Principal72_STAGING_ONLY.py check
```

이는 API 호출0이고 두 승인 JSON을 fail-closed로 생성하며, 사용자가 실제 확인한 값만 직접 승인한다. 패키지의 `README_Main24_STAGING_KO.txt`에 나머지 첫 단일 합성·QA 승인 명령까지 수록되어 있다. 사용자 **명시적 Stage-only 승인 및 무료 quota 실조회가 아직 없으므로 실제 합성은 실행된 상태가 아니다**.

## 3. 오프라인 검증 범위

새 전용 프로그램 **Python 컴파일 PASS**, ZIP CRC/파일 목록 PASS, **합성 네트워크를 모의한 9개 시험 PASS**:
1. 잠금 72행·StageID만 생성, RuntimeID=null/Google POST0.
2. 사용자 Stage 사전승인이 false면 POST0.
3. 동의 시 내부 임시 출력 StageID만 인식, 정식 ID 없음.
4. 당일 무료 quota 미확인이면 POST0.
5. 모의 첫 단일 WAV만 생성(Stage-only, Unity 불변).
6. 첫 WAV 청취 승인 전 추가 API0.
7. SHA 일치 청취 승인 후 최대2개 모의 소배치·기존 파일 무덮어쓰기.
8. 이전 PT 날짜 무료quota 검증 파일은 거부.
9. Stage 승인 JSON에 `real_runtime_id`를 넣어 Runtime인 척 하면 안전 중단.

**실제 사용자 PC, Unity, Google API, WAV 생성·삭제 = 모두 0**. 테스트 WAV는 임시 폴더 mock이며 실제 합성 파일 아님.

## 4. 다음 담당 및 사용자 승인

- Codex에게 Main24 전체 Story/Quest를 억지로 구현하도록 반복 지시하지 않는다. 현재 STAGED_ID_PLAN 0/10은 기존 탐험 흐름을 보호하는 정당한 결과다. 나중에 Main24 정식 스토리 단계 구현 시 Runtime ID를 연결하면 된다.
- **사용자 승인 필요:** (1) Stage-only 먼저 1개 녹음할지, 중복 미확인 593경로 위험 인정 여부, (2) 그때 실제 Gemini 무료quota·Billing/4 Project 소속·외부 사용량 확인, (3) 첫 샘플 청취 합격. 기존 72개 원문·VoiceID는 이미 승인돼 재승인 대상이 아니다.
- 실제 새 WAV를 생성하는 사람은 사용자 본인이고, Codex가 API 키를 다루거나 TTS를 만들지 않는다. 게임 적용은 사용자 별도 승인 후.
- 이번 GitHub 작업은 `docs/tts` 문서와 [TTS_LATEST.md](팀운영/인계/TTS_LATEST.md), Issue #125 인계만 수정. main·기존 WAV162·Save·Unity6000.6.5f1 변경 없음.
