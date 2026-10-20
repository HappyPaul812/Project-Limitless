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


## 2026-10-19 Main20·21 사용자 청취 오류9개 / 샘플 우선 재제작 (현재 최신)

- 바울 대표가 실제 청취 후 **세린8개 남성 음색**(`main20_after_colossus_serin_02/01`, `main20_confront_serin_03/02/01`, `main21_route_serin_01`, `main21_pulse_serin_01`, `main21_heat_serin_01`), **태온1개 여성 음색**(`main21_route_taeon_01`) 보고. 이9개는 청취 불합격·Unity 적용 보류. 나머지 8개는 별도 최종 합격 확정 아님.
- 원래 17회 제작 Python의 Style에 성별 명시 부족 확인. 실제 모델 오류 원인/원격 voice wire/실제 사용자 파일의 바이트 검증은 **미확정**, 성별 Style 지시가 효과를 보증하지 않음.
- TTS 담당 직접 `LIMITLESS_Main20_Main21_VoiceFix9_SampleFirst_20261019.zip` 제작/배포, SHA256 `28c130098d1e33d7889ffd79dce715ace992283885b7e01c8cdca59255160dcb`, 자체 Python `LIMITLESS_TTS_VoiceFix9.py` SHA256 `8440a69fc7bc2031da7c592e257e4bd469efeab16be25f7efdd2dfbc667c50ca`. 기존 Stable ID/원문/Voice Schedar/Gacrux 유지, 명시적 세린 여성/태온 남성 Style 추가. **세린1+태온1 샘플 먼저** 제작·사용자 청취 승인 → 나머지 세린7 별도 실행. 오류 시 즉시 HOLD, 원본17 SHA/체크포인트 변경0.
- 압축 파일과 실행 안내는 사용자 대화에서 직접 제공, README 동봉. 일단 `python .\\LIMITLESS_TTS_VoiceFix9.py check` (API0), 새로운 무료 한도를 현재 해당 프로젝트에서 확인한 뒤에만 `sample-serin`/`sample-taeon`. **초기 키1 7·키2 10은 이미 소비됐을 수 있으므로 추가9회라고 가정 금지**.
- 오프라인 compile, 기존17 SHA mock, 샘플2+나머지7 모의 성공 및 2번째 API 오류에 즉시 HOLD/lock/다음호출0 PASS. 실제 사용자 PC/실제 Gemini API **신규 호출0, WAV 신규0**; 샘플 QA 미확인, Unity0, main0.
- 상세 새 QA 정본: [Main20·21 VoiceFix9 오류/검수](../../TTS_Main20_Main21_VoiceFix9_20261019.md). 기존 이슈 #20은 생산→청취 QA 오류/재제작 추적용으로 갱신. TTS 담당이 Python을 직접 만들고 Codex는 사용자 최종 승인 후 Unity 통합만 한다.


## 2026-10-20 / 세린·태온 보이스 성별 설정 문제 확인 및 새 Python 샘플 팩 (최신)

- 사용자: “tts 되니까 작업하자.” 최신 청취 오류9(세린8 남성처럼 들림, 태온1 여성처럼 들림) 재개.
- **Google 공식 Firebase voice table의 원인 확인:** `Schedar=Male`, `Gacrux=Female`. 전일 작성한 “기존 보이스 유지 + 성별 Style” 실행팩은 추가 재제작에 사용하지 않는다. 해당 팩의 Style 조건만으로 프리셋의 성별 차이 보완은 신뢰할 수 없음.
- TTS 담당이 **새 Python ZIP 직접 제작**: `LIMITLESS_TTS_Main20_Main21_GenderFix9_SampleFirst_20261020.zip`, SHA256 `c9f8e6af586c37c3b881f37cc4ff3db46b3485991022c485a2c3b9f7c752925d`, script `LIMITLESS_TTS_GenderFix9_20261020.py` SHA256 `fd55444cb033d314a462d7f3a41d109f42260c9c12569ea7e1606e8368349c27`. ZIP/CSV/README 사용자 대화에서 제공.
- 샘플 후보 보이스 **세린=Erinome(Female, Clear)**, **태온=Alnilam(Male, Firm)**. 이번 Main20·21 테스트에서만 임시 사용, 기존 Main18 청취 승인 세린10이나 다른 챕터 음성 Voice Registry를 일방 변경하지 않음. 사용자 두 샘플 청취 승인 후 나머지 세린7 생성.
- `F:\Downloads\Voice\LIMITLESS_TTS_GenderFix9_20261020`에 압축 해제한 뒤 `python .\\LIMITLESS_TTS_GenderFix9_20261020.py check` → 실제 Google 무료 quota 및 과금 확인 후 `sample-serin`, `sample-taeon`; 출력 `F:\Downloads\Voice\Limitless_TTS_GenderFix9_20261020\candidates`. 체크에서 기존 17 성공 WAV/결과 SHA 불일치·경로 문제 있으면 호출금지/보고.
- offline Python compile/ZIP CRC, 17원본 SHA mock 보호, 샘플2→승인→나머지7 성공, 2번째 요청 모의 오류 시 HOLD/lock 및 뒤호출0 PASS. **실제 PC API 호출·실제 WAV 신규 생성=0**, 게임 Unity 적용0. 실사용 결과와 청취 평가는 사용자 응답 대기.
- 상세: [2026-10-20 정정 및 실사용 Python 인계](../../TTS_Main20_Main21_VoiceGenderFix9_20261020.md). 기존 Issue #20 보이스 오류 추적 유지. Codex는 사용자 승인 WAV Unity 통합만.


## 2026-10-20 최신 — 파형 보정 오프라인 시험판 (바울 대표 요청)

- 사용자는 성별 프리셋 교체 대신 **이미 생성한 WAV9 파형 보정** 요청. TTS 담당이 `LIMITLESS_TTS_DSP9_Offline_20261020.zip` 직접 작성·배포 (SHA256 `1a91fa078a367c7a6336fdb44209b0cd743ee43d41da4c75c392fd04c99afec9`). Google API 호출0, Unity 수정0.
- `check` 원본9 읽기 전용 검사 → `preview` 세린1·태온1 각각 gentle/medium/strong (6개) → 바울 대표 청취 승인 후 `apply --serin ... --taeon ...`로 오류9개만 새 위치에 후보 제작. 피치+포먼트 근사 DSP, 원본 음색 완벽 복구 보장 없음.
- 샘플용 합성 WAV 기반 compile/check9/preview6/apply9/원본 SHA 불변/24k PCM16/중복 출력 보호 PASS. **실제 사용자 PC WAV 보정·음질 검증 미실행**.
- [정본/기술 상세](../../TTS_Main20_Main21_Offline_DSP9_Pilot_20261020.md). 기존 VoiceFix9/GenderFix9 API 생성 패키지는 계속 사용 보류. 기존 Main18 승인, 나머지 정상음성, 원본 WAV, save/HOLD/lock, Unity 보호.


## 2026-10-20 최종 청취 피드백 — DSP 보정 전량 불합격 (최신 정본)

- 바울 대표의 직접 판정: **“다 못쓰겠다. 안바뀌고 외계인 목소리로 돼..”**. 세린·태온 gentle/medium/strong DSP 미리듣기 음성은 기존 캐릭터 음색을 복구하지 못하고 인공적 왜곡 발생. **DSP_PREVIEW_ALL_USER_FAIL / DSP_STOPPED**.
- 오프라인 가상음성의 테스트 PASS는 처리 오류 없음/원본 SHA 보존만 검증했으며 **청취 품질을 보증하지 못했음**. 기존 `LIMITLESS_TTS_DSP9_Offline.py preview/apply` 결과 공식 음성 사용 및 Unity Import 금지. 반복·보정 강도 증폭 금지.
- 원본 Main20/21 17 WAV, 이미 승인된 Main18 세린10 및 기존 정상25, 사용자 Save와 checkpoint/HOLD 불변. Gemini API를 이 세션에서 새로 호출하지 않음.
- 후속 방침: 기존 정상본과 불량9의 실제 요청 설정/모델/VoiceID/스타일/SDK/대사·WAV SHA를 우선 API0 비교. 일치하면 기존 Voice ID/원문 유지하고 **불량1개만 통제 재생성 후 청취**, 실패 시 추가 비용/쿼터 낭비 중단. 대안 프리셋 무단 변경 금지. 상태·근거는 [DSP9 청취 불합격 정본](../../TTS_Main20_Main21_Offline_DSP9_Pilot_20261020.md).


## 2026-10-20 바울 대표 “오늘 20회 써보자” — 직접 Python 배치 준비 (최신)

- 20회는 **키1 최대10, 키2 최대10** 사용자 희망 예산. 두 키가 독립 Google Project, 모델 gemini-3.8-flash-tts FreeTier 실제 사용 가능/무과금이 확인될 때만 해당 상한 사용. 동일 Project 키 중복합산 금지. 실제 API 호출0 (바울 대표 PC 실행 대기).
- 이전 **DSP 미리듣기 전량 FAIL** 유지. 새로운 Python은 원래 생성 당시 **동일 Schedar/Gacrux + 동일 Style + 동일 REST 요청 JSON + 동일 ExactText**로 제작; 프리셋 교체/피치 보정0. 9개 오류=세린8, 태온1. Main21 추가 미제작2=main21_haren_taeon_02, main21_prepare_serin_01. 하렌 자체 Voice미승인3 BLOCK.
- TTS 담당 직접 `LIMITLESS_TTS_Today20_20261020.zip` 제작, SHA256 `7e2c80cc26f6127dbc22075e975959afab4246aa66f57ef6696e2b7c747ae635`; Python+고정 대사CSV+README. 사용자 대화에서 다운로드 제공. 별도 원본 `Limitless_TTS_Main20_Main21_20261019` 전체 17 결과 SHA 보호; 새로운 결과 폴더 `Limitless_TTS_20Calls_20261020`.
- 안전 단계: `check` API0 → `sample --confirm-free` 세린1/태온1 (KEY1 2) → 둘 다 사용자 청취 PASS면 `approve-samples` → `main --confirm-free` 나머지9 (KEY1 8, KEY2 1) → 실제 청취 FAIL인 ID에 한해 `retry --ids ... --confirm-free` 최대9 (KEY2 9). **실제 불량이 없는 상태에서 예비9를 무조건 소진하지 않음.**
- 오프라인 모의: 17원본 SHA 감사, sample2→승인 게이트→main9→retry9 = 20/Key1 10/Key2 10 PASS. 두 번째 가짜 API 오류 시 HOLD+lock 유지·뒤호출0 PASS, WAV 변조 감지 PASS. **실제 Gemini 신규 호출/WAV0/Unity0**.
- [상세 계획/스크립트 메타](../../TTS_Main20_21_Today20_Staged_Retry_20261020.md). Codex는 여전히 사용자 최종 청취 승인 뒤 Unity 통합만 담당. TTS 업무 책임/실행은 TTS 전담 세션+바울 대표 로컬 Python. docs/tts 브랜치만 갱신, main 변경·Push/merge 없음.


## 2026-10-20 최신 실제 실행 로그 — 샘플 첫 API HTTP400 / HOLD 유지

- 바울 대표 PowerShell 화면: 이전20회 `check` PASS; `sample --confirm-free` KEY1 첫 `main20_confront_serin_01`/`Schedar`/`gemini-3.8-flash-tts` 호출 **HTTP400**, 성공 WAV0, 이후 태온/나머지 요청0. 즉시 `HOLD.json`+`RUNNING.lock` 유지, 중복 실행 차단 정상.
- 신규 Python 스크립트/이전17회 스크립트/Google 공식 Gemini 3.8 REST JSON 구조 일치함을 정적 확인했으나 기존 코드가 **HTTP400 상세 응답 본문 저장하지 않은 결함** 있어 원인은 미확정. Key/Voice/모델/컨텐츠 문제로 단정 금지.
- TTS 담당 직접 안전 진단 ZIP `LIMITLESS_TTS_HTTP400_Diagnose_20261020.zip` 작성·배포 (SHA256 `d1270b807435d8f222e5a976ba00b65c133ea5fa19fb800840b87c645296815a`). 기존 스크립트 옆에 추가하고 `inspect`(API0/쓰기0), `model-info`(Google 모델 GET1/TTS POST0) 순서로 실행, 결과 보고. `diagnose-one`의 단 1회 POST는 상세진단 필요 시 사용자 추가 확인 후에만. 기존 HOLD/lock은 절대 해제/초기화/덮어쓰기 금지.
- 가상 원본17 SHA 및 HOLD/lock 보호, 모델 GET, 오류 400 POST 상세 메시지 추출/키 마스킹, 추가 호출 차단 오프라인 테스트 PASS. **실제 신규 Google API/모델 GET은 이 ChatGPT에서 실행하지 않았으며 사용자 PC 진단 대기**. 첫 시도는 보수적으로 KEY1 소모1회로 기록. Unity0/main0.
- 상세 정본: [2026-10-20 첫 HTTP400 진단](../../TTS_Main20_21_Initial_HTTP400_Diagnostic_20261020.md). Issue #20도 BLOCKED로 갱신.


## 2026-10-20 실제 진단 결론 — KEY1 API 인증 거절 (최신)

- 사용자 진단 `inspect`: HOLD/lock·원본 SHA 정상, 첫 샘플 HTTP400, 신규 WAV0/후속 생성 호출0.
- 사용자 `model-info` 2회: 모두 HTTP400 `INVALID_ARGUMENT` / `API key not valid. Please pass a valid API key.`. 이는 TTS generateContent가 아니라 모델 메타 조회 GET이므로 **이 시점에 입력한 KEY1의 API 인증이 거절된 것이 직접 문제**. 첫 실패에 사용된 키와 두 진단 입력의 동일성은 화면만으로 별도 확인 필요.
- 다음 조치: Google AI Studio에서 해당 Google Project의 유효 API Key(프로젝트 ID와 다름)를 안전하게 재확인, 채팅·화면에 키 노출하지 않은 상태로 `model-info`만 다시 검사. PASS 전 샘플·진단 POST 추가 실행 금지. 기존 HOLD/lock 삭제·재설정 금지, 이전 Main20/21 WAV와 Main18 승인본 보존.
- [정본 상세](../../TTS_Main20_21_Initial_HTTP400_Diagnostic_20261020.md). 이 진단을 통해 새 TTS 호출0, 새 WAV0/Unity0. 기존 음색 오류9는 별도 미해결.


## 2026-10-20 최신 — 진단1회 생성 성공, 세린 남성 음색 청취 FAIL

- 바울 대표 화면으로 유효 KEY1의 단일 generateContent 요청 성공, WAV1개 별도 저장 확인. 대상 main20_confront_serin_01, Schedar, gemini-3.8-flash-tts. result.json 경로 F:\Downloads\Voice\Limitless_TTS_Today20_400Diagnostic_20261020\result.json. 추가 TTS 호출0, 기존 HOLD/lock 유지.
- 사용자 청취: “세린인데 남자..;;” → USER_LISTENING_FAIL. 생성 성공과 캐릭터 음색 합격 구별, 해당 WAV Unity 적용 금지. Main20/21 세린8/태온1 불합격 및 DSP 오프라인 보정 전량 FAIL 유지.
- 관측된 오늘 생성 POST 시도2(처음 HTTP400 1, 진단 성공1), 새 WAV 성공1. 실제 무료 quota 차감은 미확인. 원래 Voice 반복 재생성 및 20회 무조건 소진 중단.
- 다음은 정상 승인 세린 음성(Main18)과 요청 기록 비교, 음색 변경 대안 검토/사용자 승인 후 제한적 제작. 기존 승인 음성 및 모든 Save/WAV/HOLD 보호. 상세 문서 TTS_Main20_21_Initial_HTTP400_Diagnostic_20261020.md.


## 2026-10-20 최신 — 승인된 세린 WAV를 직접 참고하는 1개 Voice Conversion 시범

- 사용자: “내가 OK한 세린 파형으로 니가 바꿔볼래?” 기존 단순 DSP/피치·포먼트 방식은 사용자 청취 FAIL 유지. **사용자 승인 WAV를 참조하는 OpenVoice 기반 화자 음색 변환**을 새 방식으로 1개만 검증 준비.
- 기준 WAV: `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\sample_candidate\main18_ground_serin_01.wav`, 승인 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`. 대상: 2026-10-20 단발 성공·음색 FAIL `F:\Downloads\Voice\Limitless_TTS_Today20_400Diagnostic_20261020\main20_confront_serin_01_DIAGNOSTIC.wav`.
- TTS 담당이 `LIMITLESS_Serin_ApprovedVoice_VC_20261020.zip` 직접 제작·사용자 다운로드 제공 (SHA256 `77293681c01e82f7be00fb2029f73ca287c0522dadbb338c8086097c7f567f06`). Python + 한국어 README만 동봉, 제3자 Windows x64 CPU OpenVoice 런타임/131MB 모델은 `setup` 사용자가 명시적으로 승인 후 다운로드, SHA 검사.
- `check` API0/쓰기0 → `setup` 외부 OSS 다운로드(유료 Gemini API0) → `preview` 사용자 로컬 WAV 1개만 독립 `Limitless_TTS_Serin_ReferenceVC_20261020` 폴더에 변환해 청취 대기. 원본 승인·오류 WAV, 그 외17/25, 모든 HOLD/lock·Unity·Save 불변. 이전 Main18 승인 보이스 레지스트리 변경0.
- 스크립트 compile/가짜 WAV 입력·승인 참조 SHA·윈도우 엔진 명령·중복 결과 방지·해시 QA mock PASS, **실제 F: 사용자 WAV, OpenVoice 모델 실행 및 음성 청취 아직 미검증**. 이 방식도 완벽한 원래 음색 복원 보장 없음. 결과 청취 FAIL시 추가7개 연속변환 금지.
- [GitHub 상세 정본](../../TTS_Serin_ApprovedReference_VoiceConversion_Pilot_20261020.md). TTS Python 작성은 TTS 담당, Unity 통합만 Codex. docs/tts만 기록, main 수정0.


## 2026-10-20 최신 — 바울 대표 요청에 따라 실제 세린 WAV 8개 직접 변환해 ZIP 전달

- 사용자 직접 첨부 `candidates.zip` 17 WAV 실제 바이트 확인. ChatGPT 파일 보관함에서 과거 바울 대표 승인 `main18_ground_serin_01.wav`(SHA `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`) 및 `LIMITLESS_Serin6_ConsistentVoice_Previews.zip`(SHA `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`) 확보·해시 확인. 이전에 파일 접근 불가하던 상태 해소.
- TTS 담당이 실제 사용자 오류 세린8에 대해 **A_natural/B_closer 두 버전, 16개 실 WAV**를 작업 컨테이너에서 직접 작성하고 사용자에게 `LIMITLESS_Serin8_ReferenceGuided_WAV_Previews_20261020.zip` 전달. ZIP SHA256 `349d6bf7047a92d04972f68c769595969b3ff69d8b6e03f1ea8bfd7179ea535e`, 크기4,419,922 bytes.
- 공식 승인 WAV7의 F0/평균 스펙트럼을 참조한 Praat PSOLA 피치/포먼트 + 제한적 참조 EQ 방식. **AI OpenVoice 사전학습모델을 다운로드/실행하지 못하므로 ML 목소리 복제라고 주장하지 않음**. 과거 DSP6 사용자 FAIL과 구별되지만 자연스러움 사용자 평가 **대기**, 두 샘플 모두 불합격이면 16개 전체 공식 불합격 처리/Unity 미적용.
- WAV 16개 24kHz mono PCM16·길이·SHA·리드디코딩 PASS; 업로드 원본/승인 기준7 SHA 불변. API0/Unity0. 태온 목소리는 다른 참조가 필요하여 제외.
- 세린 `main20_confront_serin_01` A와 B 샘플 WAV도 ZIP 밖 직접 공유. [상세 정본](../../TTS_Serin8_ReferenceGuided_Offline_Prevs_20261020.md). 브랜치 docs/tts만 기록, main 변경/Push/merge0.


## 2026-10-20 최신 — 승인참조 세린 A/B 16개 사용자 청취 전량 FAIL

- 사용자 직접 피드백 **“다 남자다..;;;;”**. 실자료 기반 reference-guided PSOLA+EQ A/B 16개 전체 **USER_QA_FAIL / DO_NOT_USE / UNITY_APPLY_FORBIDDEN**. 이전 단순 DSP 실패와 마찬가지로 여성 캐릭터 음색 복구 실패.
- 피치/포먼트/평균스펙트럼 참조 보정은 화자 음색 변환이 아니라 음향 통계 보정이며 결과의 남성 지각을 제거하지 못함. 본 계열의 추가 보정·강도 증가 중단. WAV 포맷 검사 PASS≠청취 PASS.
- 향후 사용자가 원하면 실제 화자 임베딩 기반 신경망 Voice Conversion 모델을 **승인 Main18 세린 기준 WAV로 1개 시범** 사용하거나, 신규 여성 보이스 한 개를 실제 청취해 정식 선정. 모델/변환의 성공은 미보장, 사용자 승인 전 기존 공식 보이스 설정·Main18 승인10 및 Unity 모두 불변. 태온1 음색 문제는 별도.
- 상세 결과: [세린 16개 직접 보정 전량 불합격](../../TTS_Serin8_ReferenceGuided_Offline_Prevs_20261020.md). 이번 업데이트 Github docs/tts 문서만, 음성 API0/Unity0.
