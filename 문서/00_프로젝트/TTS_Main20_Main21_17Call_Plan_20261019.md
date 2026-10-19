# LIMITLESS — Main20/21 TTS 17회 배분 및 Codex 제작 인계 (2026-10-19)

**상태: USER_REPORTED_QUOTA / CALL_ALLOCATION_PLANNED / PROJECT_BOUNDARY_UNVERIFIED / LOCAL_GENERATION_NOT_EXECUTED / WAV_NEW=0 / UNITY_UNCHANGED**

## 바울 대표 지시

“이제 키1 7회, 키2 10회니까 거기에 맞게 작업해줘.” 사용자 보고한 각각의 잔여 횟수(키1 7 / 키2 10)를 **최대 배분 상한**으로 사용한다. 이는 Google Project+Model 잔량 또는 결제/무료 설정을 담당자가 독립 조회한 값이 아니다. 키1·키2가 동일 Google Project 소속이면 7+10을 합산하지 않는다. 서로 다른 Project의 무료 `gemini-3.8-flash-tts` quota로 확정될 때에만 17회가 가능하다. 확인 불가·유료/과금 위험이 있으면 요청0으로 BLOCKED.

## 정본·범위

- Main20: `문서/00_프로젝트/Main20_TTS_Manifest.csv`, source Git blob `2746d645317da651949a71247be89e6ff10f1707`. VoiceExpected=true 세린5만 제작. Player와 지문 TTS 금지.
- Main21: `문서/03_스토리/Main21_TTS_Manifest_20261010.json`, source Git blob `90dfa70452731f04d47af7dcb0334bd2ae8dd5ec`. VoiceExpected=true 17 중 하렌3개(`story_haren`) 미확정 Voice로 BLOCKED. 나머지14 중 스토리 순서상 앞12만 제작. Player·지문 TTS 금지.
- 이번 실행용 **정확한 17개 ID/화자/Voice/원문/키 배분**: [CSV 큐](TTS_Main20_Main21_17Call_Queue_20261019.csv), 원문을 기계적으로 복제. 로컬 DialogueCatalog·Runtime Text와 생성 전에 반드시 정확히 대조하고 차이가 있으면 중단해 보고; 임의 대사/ID 수정 금지.
- Main18 사용자 최종 청취 승인 세린10 (ground1 + PREVIEW6 + 수동복구3)은 **추가 생성0**. 별도 [기존 Unity 통합 인계](TTS_Main18_Serin10_Approved_Unity_Handoff_20261019.md)의 Codex 작업을 중단/교체하지 않음. 이전 정상 음성25 및 체크포인트/잠금/원본 보호.
- 하렌 VoiceId/Voice Design 미확정; 다른 인물 Voice를 임의 대용 금지.

## 17회 사용 계획

| Key slot | 최대 | 내용 |
|---|---:|---|
| KEY1 | 7 | Main20 세린5 + Main21 첫2 |
| KEY2 | 10 | Main21 다음10 |
| 합계 | 17 | Main20 5/5 + Main21 일반 12/14 |

01. [키1] `main20_confront_serin_01` — companion_serin/Schedar
02. [키1] `main20_confront_serin_02` — companion_serin/Schedar
03. [키1] `main20_confront_serin_03` — companion_serin/Schedar
04. [키1] `main20_after_colossus_serin_01` — companion_serin/Schedar
05. [키1] `main20_after_colossus_serin_02` — companion_serin/Schedar
06. [키1] `main21_heat_serin_01` — companion_serin/Schedar
07. [키1] `main21_pulse_serin_01` — companion_serin/Schedar
08. [키2] `main21_route_paul_01` — companion_paul/Achird
09. [키2] `main21_route_serin_01` — companion_serin/Schedar
10. [키2] `main21_route_paul_02` — companion_paul/Achird
11. [키2] `main21_route_taeon_01` — companion_taeon/Gacrux
12. [키2] `main21_arbel_leon_01` — arbel-leon/Orus
13. [키2] `main21_aid_miel_01` — companion_miel/Sulafat
14. [키2] `main21_aid_paul_01` — companion_paul/Achird
15. [키2] `main21_aid_miel_02` — companion_miel/Sulafat
16. [키2] `main21_carriage_leon_01` — arbel-leon/Orus
17. [키2] `main21_haren_taeon_01` — companion_taeon/Gacrux

### 다음 quota로 이월: Main21 2개

- `main21_haren_taeon_02` (태온 / Gacrux)
- `main21_prepare_serin_01` (세린 / Schedar)

### Voice 확정 대기: 하렌 3개

- `main21_haren_haren_01`, `main21_haren_haren_02`, `main21_haren_haren_03` (Voice 미정). 별도 승인을 받기 전에는 대상에 포함하지 않는다.

## 역할 정정 — TTS 담당 직접 Python 제작 (2026-10-19)

바울 대표 지적대로 **Python TTS 생성 스크립트 작성·음성 제작 운영·음색/원문 품질 검수는 TTS 담당**이 수행한다. 바울 대표는 본인 로컬 PC에서 Python 스크립트를 실행하고 키/쿼터 및 청취 승인을 확인한다. **Codex는 사전에 승인된 WAV의 Unity 통합 및 런타임 검증만 담당**한다. 최초 Codex 생성 작업 할당은 오류이며 철회한다.

### 실제 전달한 실행팩

- `LIMITLESS_TTS_Main20_Main21_17_Python_Pack_20261019.zip` (9,874 bytes), SHA256 `1a5a80afc98023be8324809e5fd6e7dad8cd12eccb852843525bf132c75bf612`.
- Python `LIMITLESS_TTS_Main20_Main21_17.py` + 고정 17개 원문 CSV + README. 비밀키 없음.
- 스크립트 직접 제작 후 offline `check`: 17행, KEY1 7/KEY2 10, SHA 정합 PASS. 17회 모의 SAVED·SHA 변조 차단 및 두 번째 요청 실패 모의 즉시 HOLD/LOCK/뒤 호출0 PASS. **실제 TTS API 요청0, 실제 WAV 생성0, 실제 사용자 PC 실행0**.
- 출력 위치: `F:\\Downloads\\Voice\\Limitless_TTS_Main20_Main21_20261019\\`. 기존 Main18 WAV/체크포인트/HOLD/잠금 접근·변경하지 않음.
- 사용자가 압축해제 후 `python .\\LIMITLESS_TTS_Main20_Main21_17.py check` → `python .\\LIMITLESS_TTS_Main20_Main21_17.py run --confirm-free` 실행. 동일 Google Project에 속한 키는 각각 독립 quota로 합산하지 않음. Google AI Studio에서 소속 Project ID·모델별 무료 잔여량·Billing 직접 확인. 스크립트는 이를 API로 자동 보증하지 않으며 확인 불가 시 중단.
- API Key는 환경변수 또는 숨김 입력으로만 받음. 첫 오류·429·원본 훼손 의심 시 즉시 전체 중단하고 신규 HOLD/잠금을 보존. 자동 재시도·합본·성공 음성 덮어쓰기0.
- 바울 대표의 WAV 최종 청취 승인 후 **Codex 별도 Unity 통합**: Main20 세린5/Main21 일반12를 Manifest/DialogueID/화자/Voice PCM/SHA 매핑 검증, 기존 정상본 보호, 원본 Editor·Save 비간섭, 작업 `codex/...` 브랜치에서 진행.

## 실행 상태와 중요 구분

- 이번 담당 세션이 만든 것은 **17행 제작 큐 및 인계 문서**다. 실제 Google Project/Quota, 사용자 PC의 로컬 출력/코드·Key mapping은 확인하지 못했으며 **API 호출0, 새 WAV0, Unity 적용0**.
- 사용자 발언은 배분 승인으로 취급하지만 무료 Project 분리 및 현재 Billing 확인이 실패하면 멈춘다. 단일 프로젝트로 확인되면 한도가 허용하는 범위를 다시 계산해야 한다.
- Main18 세린10 Codex 기존 [GitHub Issue #17](https://github.com/HappyPaul812/Project-Limitless/issues/17)는 본 신규 배치와 별도 작업이다.
