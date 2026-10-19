# LIMITLESS / TTS 담당 최신 인계 — 2026-10-13

## 오늘 결과
- **Main18 세린10 오류 전용 Schedar 안전 준비 v2** 작성 및 격리 fixture 테스트 완료. GitHub 기록: [TTS_Serin10_Schedar_v2_Preflight_20261013.md](../../TTS_Serin10_Schedar_v2_Preflight_20261013.md).
- 별도 배포 Python: `M18_Serin10_Schedar_v2_SAFE.py`, SHA256 `2edd62ff4c7cbc7fe055c665b39f35aaa5ef126691d1393c692fb42e8e1cd741`, 13,057 bytes. **Gemini/HTTP 함수 없음; sample 명령 항상 차단**.
- 명령: `python .\M18_Serin10_Schedar_v2_SAFE.py audit` (읽기 전용) → 로컬 결과 확인 → `python .\M18_Serin10_Schedar_v2_SAFE.py prepare` (독립 새 폴더에 오류 WAV10 SHA 검증 백업·잠금 보고).
- v1 `F:\Downloads\M18_Serin10_Staged.py` (Erinome/Kore만 허용) 실행 보류, 수정0.

## 확정 보호
- 세린 Voice Schedar 유지. Main18 오류10만 REGEN_REQUIRED, 사용자 청취 정상 Main18 세린5/Main19 세린7/Main17 승인 세린11 및 Unity 정상 통합 음성25 보호.
- 기존 오류10의 실제 당시 API Wire Voice는 UNKNOWN; 원인 확정 없음.
- 샘플 대상 `main18_ground_serin_01`, 원문/DialogueId/SpeakerId 변경 금지.
- 본 도구는 준비만. 샘플 생성 **NOT_EXECUTED**, 실제 사용자 LOCAL Stage 실행 **NOT_EXECUTED**.
- 사용자 무료 quota 잔량, Google Project별 모델 RPD 및 Billing 상태 **NOT_VERIFIED**, 사용 허용 확정 호출 **0**. 키 변경으로 quota 추가되었다고 가정 금지.

## 다음 선행조건
1. Codex 로컬 정적 대조 또는 사용자 `audit` 실행 결과 수령. `prepare` 성공 시 백업10 SHA, BASELINE/LOCKED 보고 확인.
2. 바울 대표가 Google AI Studio Projects/Billing, Dashboard Usage/Rate limits에서 **현재 해당 프로젝트·모델 무료 사용 가능 여부와 과금 위험 없음**을 확인.
3. 바울 대표의 명시적 **샘플 1회 호출 승인** 이후 새 1-call-generator 별도 코드 감사·실행. 실패/429/불확실한 경우 즉시 중단·재시도/키 변경 없음.
4. 사용자가 샘플 WAV의 실제 음색·원문 청취 PASS → 1개 정식 본으로 사용. 나머지9는 **별도 승인** 후 제작.
5. 전체 청취 승인 이후 Codex가 Unity 음성 연결. 담당 TTS는 Unity 수정하지 않음.

## GitHub 협업
- TTS 작업 브랜치: `docs/tts` 전용; `main` 직접 commit/push/merge 금지.
- 문서 갱신을 마쳤다면 해당 담당 커밋 SHA를 완료 보고에서 별도 확인.
- 세션 전환 시 최신 `main` 정본과 `docs/tts` 미병합 내용 양쪽 확인.


## 후속 LOCAL 실행 확인 — 2026-10-13

사용자가 실제 `py3_12` PowerShell 실행 화면을 제공:
- `M18_Serin10_Schedar_v2_SAFE.py audit` → `[AUDIT PASS] target=10, protected=25, Voice=Schedar, API=0`, 읽기 전용.
- `M18_Serin10_Schedar_v2_SAFE.py prepare` → `[PREPARE COMPLETE / API 0]`, 오류 WAV10 백업 SHA 검증·정상25 비교, Unity 수정0.
- 신규 외부 출력: `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2`.
- **현재 상태 = `AUDIT_PASS / PREPARE_COMPLETE / SAMPLE_NOT_EXECUTED`**. 위쪽 과거 `LOCAL NOT_EXECUTED` 항목은 해당 시점 이전 기록.
- 사용자의 터미널 로그 확인이며 담당자 독립 로컬 파일 접근 검증은 아님.
- Gemini TTS 생성0 / WAV 신규 생성0 / 전체 무료 잔량·Billing 상태 `NOT_VERIFIED`. 생성기 없음·`sample` 실행 차단 유지.
- 다음은 사용자 AI Studio의 현재 해당 Project/모델 한도·과금 상태 확인 및 **샘플 한 번 명시적 승인**. 승인 후 안전한 별도 생성기 → 사용자의 실제 청취 승인 → 이후9개 별도 승인. 샘플 ID: `main18_ground_serin_01`, Voice `Schedar`.
- 현황 상세: [v2 사전 준비 및 LOCAL 실행 후속](../../TTS_Serin10_Schedar_v2_Preflight_20261013.md).


## 2026-10-13 첫 Schedar 샘플 사용자 음색 승인 — 현재 최신

**상태: `SAMPLE_SAVED / USER_VOICE_APPROVED / REMAINING_9_NOT_AUTHORIZED`.**

- 사용자 PC에서 `M18_Serin_Schedar_SAMPLE_ONE.py --key-slot 2 --approve-one-call` 및 `YES`로 `main18_ground_serin_01` 샘플 **1회** 요청·저장.
- 모델 `gemini-3.8-flash-tts`; Voice `Schedar`; 새 WAV `F:\\Downloads\\Voice\\Limitless_TTS_Serin10_Schedar_v2\\sample_candidate\\main18_ground_serin_01.wav`.
- 사용자 로그 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`; 로컬 파일 별도 해시 재검증은 아직 수행하지 않음.
- **사용자 직접 청취 발언: "목소리 제대로 나와." → 음색 `USER_VOICE_APPROVED`.** 원문 누락/단어 정확성까지 사용자가 명시적으로 PASS한 것은 아니므로 `EXACTTEXT_LISTENING_PENDING` 유지.
- 샘플은 정식 재제작 10개 중 1개로 **재사용, 중복 생성 금지**. 남은9개는 **별도 생성 승인 필요**; 아직 호출0. 정상 Main18·19 Unity 음성25, Main17 승인 세린11, 기존 오류 WAV 원본/백업 보호.
- 이후: 로컬 checkpoint와 샘플 hash 대조 → 현재 프로젝트·모델 무료 quota/과금 확인 → **나머지9개 명시적 승인** → 9개만 StoryOrder대로 생성/QA → 사용자의 전체 청취 → Codex 별도 Unity 통합.
- 이 항목이 상단 과거 `SAMPLE_NOT_EXECUTED`를 대체. 현재 상태는 샘플 성공1·잔여9 잠금.


## 2026-10-13 나머지9 생성기 오프라인 준비 — 최신
- **사용자가 남은9개 제작 명시 승인**, 샘플 `main18_ground_serin_01` 재사용. 그러나 이 ChatGPT 작업에서 실제 Gemini 호출0·신규 WAV0 (사용자 PC 실행 대기).
- 사용자 다운로드용 `M18_Serin_Schedar_REMAINING9.py`, SHA256 `94b75a9f0c7901bb4a94b2f0da017985036075809a593ee6c5838196a6a46c76`. SAFE v2와 샘플 생성 Python 2개 SHA 고정, 기존 25 정상/원본10/백업10/샘플 SHA 검증.
- `check`는 API0, `run --key-slot 2 --max-new-calls 9 --approve-remaining-nine`는 사용자가 **실제 무료 잔여량과 Billing을 직접 확인하고 YES** 입력해야 시도. Project+Model quota, 키 변경에 따른 신규 예산 가정 금지. 쿼터 불확실 시 `--max-new-calls` 줄이기.
- 기존 샘플 제외 Story `02,07,08,09,10,11,12,13,14` 순서. 자동 retry0, 호출 25초 간격, 실패/429 즉시 HOLD, 기존 성공 WAV 재생성 차단. 결과 Stage 폴더의 `remaining9_wav`와 StoryOrder CSV.
- 오프라인 mock 정상9회/둘째429 즉시 중단 테스트 통과. 실제 SDK/사용자 PC 진행 여부는 아직 `NOT_EXECUTED`/후속 스크린샷 필요.
- 상세: [나머지9 생성기 보고](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). 이번 문서는 `docs/tts` 전용, main 변경/병합0, Unity 변경0.


## 2026-10-13 잔여9 실제 제작 부분 성공 / Story13 SAFE HOLD — 최신 상태

사용자 PC PowerShell 스크린샷으로 `M18_Serin_Schedar_REMAINING9.py` 실행 결과 확인:
- 승인 샘플 Story06 1개 **재사용**(신규 호출0).
- Story02,07,08,09,10,11,12 `[SAVED]` **7 WAV 신규 생성**(ID별 SHA 출력, 실제 바이트 검사 별도 대기).
- Story13 `main18_watcher_witness_serin_01` `[API REQUEST]` 후 `[SAFE HOLD] / RuntimeError` **FAILED_OR_UNCERTAIN_HOLD**; 세부 원인 UNKNOWN, 429라고 단정하지 않음. Story14 `main18_watcher_witness_serin_02` **NOT_CALLED**.
- **전체10: SAVED8(샘플1+7), 불확실1, 미호출1.** 지금까지 생성 시도 총9(기존 샘플1+이번8)로 관측. 전체10 사용자 청취 승인 아직 아님.
- **중요: 추가 API0, 같은 ID 자동 재시도/새 Key/다음 Story14 호출/잠금 및 checkpoint 초기화 전부 금지.** 다음은 Codex가 사용자 로컬 오류로그·attempt·HOLD·lock·생성 파일의 SHA를 읽기 전용 대조하고 결과 보고. 재개는 별도 승인과 현재 무료 quota 재확인 후.
- GitHub 결과 문서: [나머지9 생성기 및 SAFE HOLD 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). 사용자 화면 근거, 독립 검사 미실시. Unity/Main 변경·WAV 적용0.


## 2026-10-13 API KEY Project 귀속 불일치 — 최신 조사 대기
- 사용자 Google AI Studio 10/13 Gemini3.8 Flash TTS RPD: 키1이라고 설명한 Default Gemini Project **9/10**, 키2라고 설명한 Project-Limitless **0/10**. 그래프상 관측값이며 정확한 실시간 잔여 호출 수 확인이 아님.
- 생성 CLI `--key-slot 2`인데 로그상 샘플 포함 API 시도9개. 키2 실제 프로젝트 연결 여부, 스크립트 환경변수 선택, Google 사용량 반영 지연 **모두 미확정**.
- Codex 정적 감사로 `--key-slot` 구현, 키→Project 대응(값 출력 금지), Story13 RuntimeError/잠금 체크포인트를 확인할 것. API 호출·추가 생성·HOLD 해제·키 변경 0.
- 현재 WAV 상태 **8 SAVED(샘플1+7), Story13 실패/불확실1, Story14 미호출1**, 정상25/원본10/백업10 보호. [세부 문서](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 Codex 읽기 전용 감사 후 최신 인계 — SAFE HOLD 유지

바울 대표가 Codex 감사 완료 보고 화면 제공:
- 샘플1+신규 Story02/07~12 WAV7 = **8개 SHA256/checkpoint/PCM 대조 PASS**.
- Story13 `main18_watcher_witness_serin_01`: 시도+HOLD 기록 있음, 신규/부분 WAV 및 결과 JSON 없음. `RuntimeError` **예외 클래스만** 기록돼 정확한 실패 지점/429/할당량 여부 **UNKNOWN**.
- Story14 `main18_watcher_witness_serin_02`: 로컬 시도·결과·신규 WAV 없음 → 미호출로 기록.
- Codex 보고: 대상 폴더42 + 프로젝트 보호 파일5,199개 SHA 불변, HOLD·SESSION.lock·시도 기록 보존. 조사 중 API 0/WAV 수정삭제0/Unity0/Push0.
- **현재 상태 SAVED8 / FAILED_OR_UNCERTAIN_HOLD1 / NOT_CALLED1.** 기존 샘플 재생성 금지. Stage, checkpoint, lock 삭제·초기화 금지, Key 변경/Story14 선호출/Story13 재시도 금지.
- Google 두 프로젝트의 RPD 9/10 vs 0/10의 귀속 문제와 실제 KEY2 연동, quota/과금 위험은 확인되지 않았음. 새 요청에 대한 **재개 승인 없음**.
- 남은2개는 조건 확인 후 사용자 **별도 승인**을 받아 새로운 수동 재개 이력으로 처리. 전체 음성 청취/Unity 통합은 이후 별도 승인.
- 기록 정본: [잔여9 생성기 및 Codex 감사 후속](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 Story07 사용자 음색 불일치 의심 — 최신 청취 상태
- 사용자 청취 인상: `main18_heat_vent_serin_01.wav` **“감기걸린 여자? 변성기 전 미소년?”**.
- **저장·SHA/PCM PASS와 별개로 음색 `USER_REPORTED_TIMBRE_MISMATCH / USER_REVIEW_REQUIRED / HOLD`**. 기존 승인 샘플 Story06과 음색 일치 확인 전 `KEEP` 또는 Unity 적용 불가. 현재 사용자가 최종 폐기/재제작 판정을 내린 것은 아니므로 `REGEN_REQUIRED`은 사용자 판단 대기.
- 사용자 PC 해당 WAV 자체 직접 재생하지 않았음. 기존8개 저장·Story13 실패불확실·Story14 미호출 상태 불변. 추가 API0, 기존 WAV/잠금/체크포인트 보존, main/Unity 변경0.
- [상세 청취 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 Story07 업로드 오디오 직접 비교 후 재제작 권고 — 최신
- 사용자 첨부 `main18_heat_vent_serin_01.wav` 4.52초, SHA256 `2426af7024c27cee7bd2bbd94883a65215a1ef406aedb2bd7dea2bf36a60c3b1`. 승인 샘플 `main18_ground_serin_01.wav` 6.52초, SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`는 과거 사용자 샘플 로그 SHA와 동일.
- 실제 업로드 WAV 비교 결과 heat_vent는 사용자가 지적한 비음/탁한 질감이 승인 샘플과 달라 **`TTS_QA_RECOMMENDS_REGEN`**. 사용자 `REGEN 확정`이나 신규 생성 호출 승인은 아직 없음. 원본/체크포인트 보호.
- WAV 중앙 F0 각각 약175/180Hz; 음색 차이는 성별/음높이 단정이 아니라 캐릭터 일관성 문제. 업로드본이 로컬 성공 WAV와 동일한지 SHA 재대조 필요.
- 현재 저장성공8 유지, 그중 승인샘플 음색PASS1·Story07 재제작 권고1·나머지6 청취 대기; Story13 불확실, Story14 미호출. 추가 API0·Unity0·main0. 상세 [생성 및 WAV 비교 QA](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 신규 TTS 6개 업로드 — 세린 음색 일관성 검수 확대

사용자가 Story02/08/09/10/11/12 WAV 여섯 개를 업로드하고 “다른 것도 조금씩 다르다”고 보고했다. 업로드 WAV 포맷과 F0 기초 검수: Story02 209.5, 08 198.2, 09 181.1, 10 189.7, 11 207.2, 12 211.0Hz 중앙값(참고용). 기준 승인 샘플 Story06 약180.2Hz. **음높이만으로 화자/재제작 필요 여부 판정 불가.**

- Story06 샘플은 사용자 음색 승인 유지.
- Story07은 사용자 불일치 보고 및 담당 재제작 권고, 기존 파일 보존.
- Story02/08~12 여섯 개는 **`USER_LISTENING_REQUIRED`**. 사용자 최종 KEEP/REGEN 판정 미정; 저장성공을 청취 PASS로 승격하지 않음.
- 전체 저장8개, Story13 실패/불확실 HOLD1, Story14 미호출1. 기존8개 중 사용자 음색 승인1, 재제작 권고1, 청취 판정 대기6.
- 추가 생성·재호출/잠금 해제 금지. 잔여 신규 제작·Story07 재제작은 프로젝트 quota 및 Billing 확인/사용자 명시적 별도 승인 후.
- 신규 음색 QA 상세: [TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md).


## 2026-10-13 정정 — 사용자 신규8 WAV 전부 청취 완료

사용자가 **“난 다 들어봤다고....”**라고 명확히 밝혔으므로 이전 '나머지6 청취 대기'는 오기다.

- 저장 성공8개 모두 사용자 직접 청취: `USER_LISTENED=8`. 다시 들어보라고 요구하지 말 것.
- Story06 ground 1개: `USER_VOICE_APPROVED`.
- Story07 heat_vent 1개: `USER_REPORTED_TIMBRE_MISMATCH / TTS_QA_RECOMMENDS_REGEN`.
- Story02 leon, Story08~11 overheat4, Story12 vibration1의 6개: `USER_REPORTED_TIMBRE_VARIANCE`; 각각 음색이 조금씩 달라 사용자가 직접 지적함. 사용자 파일별 최종 KEEP/REGEN 결정은 미표명, 추측 금지.
- Story13 실패/불확실 HOLD, Story14 미호출. 기존 WAV8/정상25/backup/LOCK 보호. 추가 API0, Unity0, main0.
- 정본: [생성기 및 사용자 청취 정정](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md)


## 2026-10-13 사용자 요청 오프라인 음색 보정 6 WAV — 미리듣기

사용자: **“heat_vent는 빼고 니가 못고쳐?”**. 이전에 업로드한 세린 `leon`, `overheat_01~04`, `vibration` WAV6개만 승인 `ground` 샘플 기준의 경미한 pitch/EQ/음량 보정으로 **`*_PREVIEW.wav`**를 제공했다. `heat_vent`는 제외. 기준 `ground` WAV는 변경 없이 ZIP 안에 동봉. 출력 ZIP `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` (별도 다운로드 산출물), QA CSV 포함.

- WAV6개 샘플 길이 동일, 24kHz mono PCM16, clipping0, ZIP CRC PASS. 기존 원본/체크포인트/Unity WAV25/Story13 HOLD/기타 WAV 모두 무변경. Gemini API0.
- PREVIEW는 확정 교체본 아님. 동일 화자로 들리는지 사람 청취 최종 평가 전 `USER_APPROVAL_PENDING`. TTS 모델이 달리 만든 발성 질감을 DSP로 완전 복구한다는 의미 아님.
- 이전 관례의 기본 후처리 금지 정책에 따라 **정식 WAV를 덮어쓰지 않음**. 사용자 요청에 의한 비교 전용 새 파일만 제작.
- 상세: [생성 및 PREVIEW 기록](../../TTS_Main18_Serin_Schedar_Remaining9_Generator_20261013.md). `docs/tts`에만 기록, main 변경 없음.


## 2026-10-19 보정본 6개 재전달 / 다음 Main18·20·21 제작 사전준비

사용자: “니가 수정한거 아직 안받았지 나? 일단 수정한 주고 더 뽑을 꺼 준비해줘.”

- 기존 `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` 실제 파일 재확인, SHA256 `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`; ZIP CRC와 보정 WAV6 형식 PASS. 사용자 대화에 재다운로드 링크 제공. `heat_vent` 제외, `ground` 기준 음성 포함. PREVIEW는 아직 정식 Unity 승인 아님.
- 새 `LIMITLESS_TTS_Next_Preflight_20261019.py`(읽기 전용) 및 25행 다음 제작 큐/README/Codex 조사 지시가 포함된 `LIMITLESS_TTS_Next_Preflight_Pack_20261019.zip` 제공. SHA `de2036d1557399fa34aa02257ecd61c6cf887585786c8fbacf6465d9ca5c55e9`. 사용자 PC F: 실행 `NOT_EXECUTED`.
- 격리 fixture 정상 preflight 및 SHA 손상 중단 테스트 PASS. Google TTS SDK/API/키 사용0, 오프라인 읽기만 실행 가능.
- **Main18 다음 3개:** Story07 heat_vent 음색 문제 재제작 후보 (기존 SAVED 유지), Story13 watcher_01 FAILED_OR_UNCERTAIN_HOLD(잠금/시도 기록 보존), Story14 watcher_02 NOT_CALLED (Story13 선호출/건너뛰기 금지).
- **이후 Main20 세린 5개**, **Main21 NPC17개 중 하렌3 Voice 미승인 BLOCK / 기타14 후보**. 3+5+14=22는 향후 잠재 호출 수일 뿐 현재 quota 확인 수치 아님. 정확한 LOCAL Runtime 순서/기존 WAV 존재 먼저 확인.
- 2026-10-19 프로젝트별 실시간 RPD 잔여·Billing/키 소속 `NOT_VERIFIED`, 추가 API 요청 승인0. 이전 성공8 및 Preview6 원본 유지, 승인 없이 LOCK 해제·재시도·다른 Key 사용 금지.
- 상세: [보정본 재전달 및 다음 배치 계획](../../TTS_Serin_Preview6_and_NextBatch_Preflight_20261019.md). 기록은 `docs/tts` 브랜치만, `main` 변경 없음. Unity 수정0.


## 2026-10-19 사용자 로컬 PRE-FLIGHT PASS — 최신 확인

- 사용자 PowerShell 화면에서 실제 `LIMITLESS_TTS_Next_Preflight_20261019.py check`가 `[PRE-FLIGHT PASS]`를 반환.
- Unity 정상25 SHA PASS, 샘플 ground 원본 SHA PASS, Story02/07~12 성공7 WAV·시도 SHA PASS.
- Story13 `SESSION.lock`, failed/uncertain attempt, HOLD 유지; 신규 WAV/result 없음. Story14 새 attempt/result/WAV 없음.
- 추가·중복 attempt 없음. Main20 5 세린 예정, Main21 17 NPC 중 하렌3 BLOCK.
- `[PASS]`는 생성 허가 아님. 지금 quota/project/billing `NOT_VERIFIED`; TTS API0 / writes0 / Unity0 / Git writes0으로 사용자 화면에 표시.
- 사용자에 다시 preflight를 요구할 필요 없음. 이후 수동 재개는 HOLD 원인 판독과 Project+Model 사용 가능 무료 횟수 확인·비용 통제·별도 사용자 승인 후에만 진행.
- 세부 [보정본 및 다음 배치 사전검사](../../TTS_Serin_Preview6_and_NextBatch_Preflight_20261019.md).


## 2026-10-19 Main18 마지막3 수동복구 생성기 준비 (최신)

사용자: **“그래, 그럼 tts 작업 마저 하자”**. 기존 8 WAV·승인 ground/보정 Preview6을 재생성하지 않고 **Story07 heat_vent 재제작, Story13 watcher_01 불확실 실패 신규 수동 시도, Story14 watcher_02 신규 제작**만 진행하도록 생성기 준비.

- 배포: `LIMITLESS_Main18_Serin3_Recovery_20261019.zip`, SHA256 `0b580820b4d0c2bbedd8e86266f38054312f0b641a1bce452b0c64b72a76650d`.
- Python `LIMITLESS_Main18_Serin3_Recovery_20261019.py` SHA256 `799e1492312199de21daad014786ae636350f3944611c621907e37854a7df191`.
- `check`: 기존 10오류 원본+10백업, Unity 정상25, 승인 샘플 SHA, 성공 신규7, Story13 기존 HOLD+SESSION.lock, Story14 미호출, 새로운 복구 기록 검사. API0/쓰기0. 2026-10-19 사용자 PC 실행 상태 `NOT_EXECUTED`.
- `run`: 현재 Key2 연결된 실제 Google Project + model 무료 사용 가능량/비용을 **사용자가 직접 확인**하고 `--confirm-free-quota` + 대화형 `FREE`/`YES` 조건을 만족할 때만 요청. Story13가 포함되면 **`--authorize-manual-story13-retry` + `RETRY13`** 추가 확인. 같은 KEY SLOT2 사용, 자동 재시도0, 최대3회, 최소26초 간격.
- Output `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\ManualRecovery_20261019\candidates\`. 기존 `remaining9_*`·오류 시도·HOLD/SESSION.lock **절대 삭제/초기화하지 않음**. 신규 별도 checkpoint만 기록. 실패/429 즉시 새로운 HOLD 잠금 후 뒤 ID 중단.
- 오프라인 mock 3회 연속 성공·두번째429시 즉시중단·기존 HOLD 불변·성공 후보 SHA 변조 감지 PASS. 실제 Gemini TTS 호출0, 실제 무료 quota/Billing `NOT_VERIFIED`, 신규 WAV0.
- 새 후보는 사용자 실제 음색/원문 청취 승인 필요. 승인후 Codex Unity 별도 적용. Main20 세린5, Main21 17 중 하렌3 BLOCK은 향후 별도 제작, 이번 생성기 대상 아님.
- 상세: [Main18 세린3 수동복구 도구와 승인 게이트](../../TTS_Main18_Serin3_ManualRecovery_20261019.md). 이번 작업은 `docs/tts` 전용, `main` 및 Unity 수정0.


## 2026-10-19 최신: Main18 세린3 수동복구 사용자 실행 성공

- 사용자 제공 PowerShell 결과: `Story07 main18_heat_vent_serin_01` SAVED(4.88s), `Story13 main18_watcher_witness_serin_01` SAVED(4.96s), `Story14 main18_watcher_witness_serin_02` SAVED(4.20s). `[SUCCESS] New candidate WAV 3 / no Unity changes / legacy HOLD preserved`.
- 이번 실제 생성 요청 **3회 성공**. 사용자 PC 로그 근거이며 신규 세 WAV의 독립 SHA/PCM/의미 청취 검증은 아직 대기.
- 새로운 세 후보는 `ManualRecovery_20261019\candidates\` 아래 보관하고, 기존 실패 HOLD/SESSION.lock 및 과거 checkpoint 변경하지 않음.
- Main18 세린10 **후보 WAV 10개 확보**: 기존 ground 승인 샘플1 + 보정 PREVIEW6 + 신규 후보3. 사용자 전체10 최종 음색·원문 승인 아직 아님. 정상 Unity25와 Main17 승인본 보호.
- Main18 추가 생성은 현재 불필요. 다음은 새 후보3만 사용자 청취 QA, 필요시 Preview6 정식 승인 분리, 이후 Main20 세린5 / Main21 NPC14(하렌3 음성 미확정 BLOCK) 별도 생성·무료 quota 확인.
- 이번 담당은 Github `docs/tts` 기록 외 신규 API 호출0, Unity/main 변경0.
- 상세: [Main18 3개 복구 완료 후 후보 상태](../../TTS_Main18_Serin3_ManualRecovery_20261019.md).


## 2026-10-19 Main18 세린 10개 최종 사용자 승인 → Codex Unity 통합 위임 — 최신 상태

사용자 최신 명시적 지시: **“다 이상 없으니까 메인18은 끝인거 같으니 게임에 넣자, 지금 파일이 여기저기에 있으니까 알어서 할 수 있게 코덱스한테 시켜줘.”**
- **Main18 세린 교체 TTS10 사용자 최종 청취·Unity 적용 승인**. 과거 `USER_LISTENING_PENDING`/오류 음색 이력은 이 지시로 후보 세트에 대해 변경. 승인된 유일한 최종 소스는 `ground` 1, 오프라인 보정 PREVIEW6, 10/19 최신 수동 복구 후보3. **기존 잘못된 raw `remaining9_wav` 또는 원본 `Limitless_TTS_Output_Main18`의 불합격 TTS를 Unity에 넣지 말 것**.
- **Codex 구현 이슈:** https://github.com/HappyPaul812/Project-Limitless/issues/17
- **TTS→Codex 실제 파일/SHA/검증 인계:** [TTS_Main18_Serin10_Approved_Unity_Handoff_20261019.md](../../TTS_Main18_Serin10_Approved_Unity_Handoff_20261019.md), `docs/tts` 작업 브랜치.
- 샘플 `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\sample_candidate\main18_ground_serin_01.wav` SHA `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`. PREVIEW6 ZIP `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` SHA `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`. 복구3 `ManualRecovery_20261019\candidates`의 Story07/13/14, SHA는 로컬 results에서 확인.
- 기존 Main18 정상12+Main19 정상13 **Unity 연결25 보호**, 기존 Catalog/GUID/Save/기존 WAV/Story13 HOLD/lock 보호. **Codex는 `codex/tts-main18-serin10-integration-20261019` 독립 로컬 worktree**로 실제 Unity 추가10·Main18 22/22 QA 수행. `main` 직접 commit/Push/merge0, 선별 로컬 commit만.
- **현재 상태:** `USER_APPROVED_10 / CODEX_ASSIGNED_VIA_GITHUB_ISSUE / UNITY_INTEGRATION_PENDING`. 이슈를 만든 것만으로 Codex 로컬 실행·Unity 통합 성공을 뜻하지 않음. 대표가 Codex 세션에서 이슈 링크를 전달하거나 작업을 시작해야 함.
- TTS API 호출0, 본 TTS 세션 Unity 파일 수정0. Codex 완료 보고에 SHA 매핑·최종 Main18 22/22·QA/브랜치/로컬 commit와 Push 여부 기재 요구.


## 2026-10-19 키1 7회·키2 10회 보고 / Main20·Main21 17회 배분 (최신)

- 사용자 현재 잔여 호출 보고: Key1=7 / Key2=10. 서로 다른 Google Project+gemini-3.8-flash-tts의 실제 FreeTier 사용량·Billing 검증 **NOT_VERIFIED**; 각 키당 독립 사용 가능 시에만 최대17. 동일 Project면 합산 금지.
- 제작 큐: Main20 세린5 + Main21 비하렌 NPC12 = 17. Key1: Main20 5+Main21 2, Key2: Main21 10. Main21 미생성2: `main21_haren_taeon_02`, `main21_prepare_serin_01`. 하렌3 Voice미확정 BLOCK.
- [계획과 Codex 로컬 실행 지시](../../TTS_Main20_Main21_17Call_Plan_20261019.md) 및 [17행 CSV 큐](../../TTS_Main20_Main21_17Call_Queue_20261019.csv) 기록. 기존 Manifest·Main18 세린 승인10·정상25 불변.
- 이번 TTS 세션에서는 Google API 실제 호출0 / 사용자 로컬 WAV0 / Unity0 / main 변경0. Codex 실행 대기, 신규 음성 모두 사용자 청취 승인 전 Unity 적용 금지.


## 2026-10-19 역할 재정정 / 직접 Python 스크립트 제작 (현재 최신)

- 바울 대표가 **“TTS는 니가 파이션 스크립트로 하는건데 누구한테 시키니?”**라고 업무 책임을 명확히 교정. TTS 스크립트 작성/음성 제작 계획/품질 검수는 TTS 담당 책임, 실제 Gemini API는 바울 대표 로컬 PC의 Python에서 실행. **Codex는 사용자 청취 승인 후 WAV Unity 통합·QA만** 담당한다. 이전 Codex 생성 위임 지시는 철회.
- Main20 세린5 + Main21 비하렌 일반12, KEY1=7/KEY2=10 고정 큐. 하렌 Voice 미승인3 BLOCK, 이번 quota 이월 Main21 일반2.
- TTS 담당이 직접 `LIMITLESS_TTS_Main20_Main21_17_Python_Pack_20261019.zip` 작성·다운로드 제공. ZIP 9874 bytes SHA256 `1a5a80afc98023be8324809e5fd6e7dad8cd12eccb852843525bf132c75bf612`. 포함 Python/17행 CSV/README. Google 비밀키 없음.
- 실행: `python .\\LIMITLESS_TTS_Main20_Main21_17.py check` (API0) → 실제 별도 Project+Model 무료 quota/Billing 확인 후 `python .\\LIMITLESS_TTS_Main20_Main21_17.py run --confirm-free`. 실제 사용자 PC API0/WAV0이며 앞으로 사용자가 로컬에서 실행하여 로그 공유.
- Python compile/읽기 전용 CSV SHA·키배분 점검 PASS, 오프라인 모의 성공17/보존, 실패2번째 즉시 HOLD/LOCK·뒤호출0 및 저장 WAV SHA 변조 검출 PASS. **실제 Google 서비스 요청/성공/청취는 미검증**.
- 최초 잘못 만들었던 이슈 #20을 **TTS 담당 제작 추적 이슈**로 수정하여 Codex 생성 요청을 명시적으로 취소. 정정 정본: [17회 계획/스크립트](../../TTS_Main20_Main21_17Call_Plan_20261019.md), [17개 큐](../../TTS_Main20_Main21_17Call_Queue_20261019.csv).
- Main18 세린 승인10 별도 Unity 통합 이슈 #17, Main18/19 기존 정상25·원본/HOLD/WAV/Save 변경0. docs/tts만 문서 갱신, main commit/merge/Push0.
