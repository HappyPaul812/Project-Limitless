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

## Codex 로컬 실행 지시 (TTS 제작 전용)

1. `F:\study\codex\Project-Limitless`의 `AGENTS.md`, `CURRENT_STATUS.md`, `TTS_음성_제작_정책.md`, 본 계획/CSV, 기존 `Tools/TTS`와 `F:\Downloads\Voice`를 로컬 **읽기 전용** 감사. 최신 main, docs/tts, 실제 LOCAL Manifest/Dialogue/Voice Registry, checkpoint, 기존 WAV·해시 대조. 기존 성공 ID는 절대 재생성하지 말 것. 타 세션 Main18 통합 작업과 원본 Editor/파일 충돌 금지.
2. 키를 출력/기록하지 않고 각각의 실제 Google Project ID 및 FreeTier model `gemini-3.8-flash-tts` 현재 RPD 잔량, Billing/유료호출 위험 확인. **Project 다름/무료 잔량 최소7·10 확인** 전에는 실제 TTS 요청 금지. 키1/키2가 같은 Project이면 독립 호출17 계획을 실행하지 말고 실제 프로젝트 단위 quota에 맞춰 보류/보고. 사용자 숫자를 넘는 요청·Paid fallback·키 무작위 교체 금지.
3. 승인된 기존 생성 스크립트의 SDK/API wire 포맷·속성·키 슬롯 선택 로직을 감사하고, **새 Main20/21 전용 배치**를 별도 경로에 준비. 자동화가 기존 Main18 checkpoint/HOLD/lock 또는 승인 WAV에 손대면 BLOCKED. `check`(API0/읽기 전용)과 `run`(명시적 17회 상한 및 프로젝트/무료 확인) 분리. 키1 최대7 → 키2 최대10의 17 Dialogue를 CSV 순서대로. 1 Stable Dialogue ID=1 API 요청=1 WAV. 동일 캐릭터 VoiceId 고정, 장면별 Style은 승인 Registry와 기존 음색 일관성 우선. 프롬프트 속 캐릭터 대사 본문을 임의로 번역/수정하지 않는다.
4. 첫 API 에러, quota429, 실패/응답 불확실, SHA/원문 불일치 시 **즉시 전체 배치 중단**. 다른 Key로 이어서 무작정 진행하거나 다음 ID 앞으로 당김/자동 재시도 금지. 요청 전·후 attempt/checkpoint+ID+KeySlot+소속 Project(비밀제외)+모델+VoiceId+내용 해시+결과 WAV SHA+성공/불확실 여부를 남기고 원본 보존.
5. 새 WAV는 `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019\` 등 **독립 출력**에 저장(기존 이름 충돌 금지). WAV PCM/24kHz/mono/16bit 등을 실제 응답 기반으로 검증, clipping·전체 decode·길이·본문/화자 대응 자동 QA. 키/Secret/민감 payload GitHub 저장 금지.
6. 새 WAV 모두 **USER_LISTENING_PENDING**. 바울 대표의 원문/음색 청취 승인 전 Unity Import/StoryVoiceCatalog/Manifest 완료 상태 변경 금지. 승인되면 Codex 별도 Unity 통합 작업을 수행. 원본 Editor 포커스/Play/사용자 Save 보호.
7. 로컬 실행은 Codex의 독립 `codex/tts-main20-main21-batch-20261019` worktree/브랜치에서 준비·QA. `main` 직접 commit/Push/merge, 무단 원격 Push 금지. 문서·실제 생성 CSV·SHA/시도/청취 대기/남은 quota·정본과 차이·사용자 승인 상태를 보고. 문서 GitHub 직접 기록은 TTS 담당 `docs/tts`만.

## 실행 상태와 중요 구분

- 이번 담당 세션이 만든 것은 **17행 제작 큐 및 인계 문서**다. 실제 Google Project/Quota, 사용자 PC의 로컬 출력/코드·Key mapping은 확인하지 못했으며 **API 호출0, 새 WAV0, Unity 적용0**.
- 사용자 발언은 배분 승인으로 취급하지만 무료 Project 분리 및 현재 Billing 확인이 실패하면 멈춘다. 단일 프로젝트로 확인되면 한도가 허용하는 범위를 다시 계산해야 한다.
- Main18 세린10 Codex 기존 [GitHub Issue #17](https://github.com/HappyPaul812/Project-Limitless/issues/17)는 본 신규 배치와 별도 작업이다.
