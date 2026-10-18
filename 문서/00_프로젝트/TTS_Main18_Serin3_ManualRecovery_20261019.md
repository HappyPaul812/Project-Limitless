# LIMITLESS / Main18 세린 남은 3개 안전 수동 복구 생성기 — 2026-10-19

**PREPARED / OFFLINE_MOCK_TEST_PASS / LOCAL_NOT_EXECUTED / LIVE_TTS_API_CALLS=0 / QUOTA_NOT_VERIFIED**

## 제작 대상과 기존 보호

| Story | DialogueId | 상태·처리 |
|---|---|---|
| 07 | `main18_heat_vent_serin_01` | 기존 SAVED(음색 부적합), 기존 파일 보호하고 새 *candidate* 1개 |
| 13 | `main18_watcher_witness_serin_01` | 이전 `RuntimeError` 결과 불확실; 원본 `REMAINING9_HOLD.json`/`REMAINING9_SESSION.lock` 절대 수정 금지. **명시적 수동 신규 시도** |
| 14 | `main18_watcher_witness_serin_02` | 이전 미호출. Story13 새 요청 성공 후만 첫 생성 |

그 외 승인된 `ground` 샘플 1, 기존 신규저장 7, 오프라인 보정 PREVIEW 6, Unity 정상25, Main17 승인본, 원본 WAV10·백업10 모두 변경하지 않는다. `Schedar`/model `gemini-3.8-flash-tts` 및 LOCAL Manifest/DialogueCatalog ExactText/Stable ID/화자 불변.

## 배포 파일

- `LIMITLESS_Main18_Serin3_Recovery_20261019.py` — SHA256 `799e1492312199de21daad014786ae636350f3944611c621907e37854a7df191` (17,205 bytes).
- 검증용 `LIMITLESS_TTS_Next_Preflight_20261019.py` 이전 승인 원본 SHA256 `e80b91b4bb912f39bd39210d327f2bac34da6b077e4c3a0f32683e04febe6599`.
- `LIMITLESS_Main18_Serin3_Recovery_20261019.zip` SHA256 `0b580820b4d0c2bbedd8e86266f38054312f0b641a1bce452b0c64b72a76650d`. README 포함.

```powershell
cd F:\Downloads\Voice
python .\LIMITLESS_Main18_Serin3_Recovery_20261019.py check
```

위는 API0·쓰기가 없는 읽기 전용 검증. 실제 사용자 F:에서 실행한 것은 **아직 아님**.

**해당 프로젝트·모델의 현재 무료 RPD 잔여량과 Billing 과금 위험이 모두 확인된 경우에만**, 사용자가 추가로 `FREE`, `RETRY13`, `YES`를 입력하여 아래를 실행할 수 있다:

```powershell
python .\LIMITLESS_Main18_Serin3_Recovery_20261019.py run --max-new-calls 3 --key-slot 2 --confirm-free-quota --authorize-manual-story13-retry
```

안전 예산이 1회뿐이면 `--max-new-calls 1`로 첫 Story07 후보만 생성 가능. 향후 다음 2개는 실측 quota 재확인/추가 사용자 승인 후 진행. 계정별 quota 및 과금은 이 생성기에서 **실시간으로 직접 검증하지 못한다**. API Key 개수와 과거 10/13 사용량 화면에서 잔여 요청을 추론하지 않음. Key 변경·자동 fallback 없음.

## 독립 새 이력과 테스트

출력 `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\ManualRecovery_20261019\` 하위의 `candidates`, `attempts`, `results`에 **이번 실행만** 기록한다. 기존 `remaining9_*` 및 기존 HOLD/SESSION.lock은 절대 변경/삭제하지 않는다. 성공 후보는 `<DialogueId>.wav`로 신규 별도 위치에 저장하며 원본 덮어쓰기 금지.

- SDK attempts1, ID 1개당 1회, 독립 요청 사전 잠금, 중복·변조 체크, 분당 최소26초, 첫 실패/429/응답 오류 시 신규 `HOLD.json` + `SESSION.lock` 유지 후 뒤 ID 절대 호출하지 않음.
- 오류 클래스뿐 아니라 `API_RESPONSE` / `AUDIO_DECODE` / `WAV_QA` / `SAVE_WAV` / `SAVE_RESULT` 단계도 비밀 제외 상태로 기록한다. 메시지·API Key 원문 기록 금지.
- 오프라인 fake SDK 테스트: 3개 순차 저장 PASS, 샘플/legacy HOLD 보존 PASS, 후보 WAV 변조 감지 PASS, **2번째 요청 429 모의 실패 후 3번째 호출0 PASS**, 신규 잠금 유지 PASS.
- Python compile PASS, ZIP CRC PASS, **실제 Gemini 호출0**.

## 이후

새 후보는 모두 `SAVED_USER_QA_PENDING`; 승인된 ground 음색·대사 원문·혼입·잘림 등을 사용자 직접 확인한 뒤 Codex 별도 Unity 연결을 요청한다. Main20 세린5와 Main21 NPC17(하렌3 Voice 미확정)은 이후 별도 제작 계획이며 이번 도구에서는 절대 호출하지 않는다.

TTS 기록은 `docs/tts` 브랜치만. `main` 변경/병합 금지, Unity 수정0, Push 없음.
