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
