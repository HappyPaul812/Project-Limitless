# LIMITLESS / Main10 Field03 실플레이 오발화·조사점 NPC 입력 차단 — 오후 재개 계약 (2026-10-19)

- 담당: 기획, `docs/planning`. 사용자 실측: Unity 6.6.0 (6000.6.5f1), `Field_03`, 슬롯02 Lv4, 캡처3장. **원본 사용자 Editor 손플레이 재현 중 버그가 드러났으며 기존 격리 QA 통과가 이 제보를 무효화하지 않는다.**
- 상태: **MAIN10_USER_RUNTIME_QA_FAIL / 2_VOICE_PAGES_MISMATCH_CONFIRMED_BY_USER / QUEST_INPUT_BLOCKER_USER_REPORTED_AND_CODE_RISK_CONFIRMED / 18_VOICES_PCM_AUDIT_PENDING / CODEX_DEFERRED_UNTIL_AFTER_17_00 / USER_EDITOR_NOT_TO_BE_TOUCHED**.
- 바울 대표 명시 일정: Codex 5시간 남은 한도8%이므로 "**이따 5시에 잡는걸로 해**." 이전 캡처상 한도 초기화 예정 오후 **5:17**. 실제 Codex 작업은 **17:00 이후 실제 한도 재설정 여부 확인 후**, 안 되었으면 17:17 이후 진행. 17시 전 구현/QA/Play/원본 조작 금지. 기획 GitHub 문서화는 별도 허용됨.
- 기준: `main`의 `Unity/Client/Assets/_Project/Scripts/World/MainQuest10FieldFlow.cs`, `MainQuest09FieldFlow.cs`, `Unity/Client/Assets/_Project/Scripts/NPC/InteractionSystem.cs`, `Resources/Audio/Voice/Story/StoryVoiceCatalog.asset`, `Resources/QuestDefinitions/Main10_CenterOfSilence.asset`. 현행 사용자의 로컬 feature 수정은 본 main과 차이가 있을 수 있으므로 **17시 이후 Codex는 원본/기능 브랜치의 정확한 파일 SHA와 런타임 구성부터 조사**.
- 직전 Main09 브랜치 `codex/main09-dialogue-voice-fix-20261019` 기존 27개 복구 음성 사용자 직접 청취 승인·원본 디스크 정식 적용·격리 550 PASS/추가 261 PASS는 별도 성과로 보존, **Main10 실패를 Main09 재교정 허가로 확대 금지**.

## 1. 사용자 실플레이 제보와 코드 교차확인

| 화면·화자·원문 | 실제 dialogueId/원문 소스 | 사용자 들은 음성 | 판정/미확정 원인 |
| --- | --- | --- | --- |
| 태온 "폴이 가리킨 석재가 여기입니다. 주변 흙과 모서리의 방향이 다릅니다." | `MainQuest10Dialogue.Lines(step0)` / `main10_taeon_001` | "모서리..." 대사 | 실제 다른 발화. 주목: **`main10_taeon_002`**의 시작이 "모서리 간격이 반복됩니다..."여서 후보 내용 유사하지만 음원/PCM 확인 전 `002`로 단정 금지 |
| 폴 "네. 전에 본 자리를 다시 확인해보죠. 기억보다 돌이 도망가지는 않았을 겁니다." | `Lines(step0)` / `main10_paul_001` | "단단한... 표시..." 대사 | 실제 다른 발화. 주목: 후속 **`main10_paul_004`**의 "단단한 가장자리를 표시해두겠습니다..."와 내용 유사하지만 실제 발화 소스는 불명, 섣부른 clip relink 금지 |
| "현재 목표: 가공된 돌" 목표 마커가 치유사 NPC와 겹치고 `[E] 대화하기`가 표시 | `Main10` step1 `field03_main10_stonework`, `MainQuest10FieldFlow.Site` 좌표 **(-1.15, -3.7)** | 치유사 NPC가 E 입력을 받아 가공된 돌 진행 불가 | **USER_BLOCKED**. NPC 근접·최우선 입력 처리 결여 정황은 코드로 입증되나 실제 collider/Player 위치·Event 순서는 로컬 격리 재현 전 단정 금지 |

**정적 확인:** `StoryVoiceCatalog.asset`에는 Main10 NPC 음성 총 **18개**(태온6, 폴7, 미엘5)가 등록돼 있고 `_Project/Audio/Voice/Story/Main10`에 동명 WAV18개가 존재. **등록 사실은 원문/PCM 정상의 증거가 아니다**. 18개 전체 ID→대사→Catalog GUID→WAV.meta GUID→WAV 내부 실제 발화/선행무음/중복·합본·잘림을 전수 정적 점검. Main10 불량 음성 18개라는 주장은 불허(증상이 확인된 건 위 2개).

**상호작용 우선순위 코드 확인:** `InteractionSystem.cs`의 상호작용 시점에 `MainQuest09Interactable.FindCurrentInRange(transform)`만 특별 우선 처리 후 `FindNearestNpc()`를 처리한다. Update에서도 Main09 목표만 NPC 프롬프트를 숨긴다. **Main10 전용 동일 우선권 경로 부재**. `MainQuest10Interactable`의 Action에서 별도 `TryInteract()`가 호출되지만 NPC InteractionSystem의 Action과 입력 순서 경쟁할 수 있어 Quest 목표가 진행되지 않는 문제와 부합. 우회 목적으로 치유사 NPC 지우기, Quest 단계 강제 완료, Scene Collider/Spawn 재배치, 치유사 기능 비활성화 금지.

## 2. 17시 이후 Codex 실행 순서 (새 5시간 한도 확인 후)

1. `AGENTS.md`, `CURRENT_STATUS.md`, 인계/이 계약, Main09 원본 디스크 적용 최신 리포트, 정확한 local HEAD/dirty·UserData/원본 Editor 상태 확인. Codex feature worktree **`codex/main10-voice-quest-interaction-fix-20261019`** 또는 충돌 없는 새 codex 전용 브랜치 사용, main 직접 커밋/Push 금지.
2. **퀘스트 진행 막힘(P1) 먼저**: 슬롯02 등 사용자 원본 Save는 read-only 복사·격리한 후 Main10 step1 `field03_main10_stonework`와 치유사 NPC 겹침을 런타임에서 재현. 기존 Main09를 보존하는 범용 '현재 메인 퀘스트의 실제 상호작용 목표가 플레이어 반경 내이고 Modal 닫힘' 우선순위 규칙으로 Main10 조사점이 NPC 앞서 입력을 받고 **NPC 프롬프트는 현재 Main10 목표 반경에서만 숨김**. Input System 이벤트 중복/동일 프레임 쌍 입력, Main09/10 교차, 다른 NPC 정상 접근, E/F/게임패드 A·실제 Distance 등을 검증. 다른 Quest의 NPC 우선 규칙 임의 전역 변경 금지. 필요 시 Main10 전용 안전 예외로 제한.
3. **음성 18개 정적·PCM 실물 감사**: 모든 `step/page/speaker/screenText/stableDialogueId/catalogGUID/metadataGUID/path/clipSamples/first-and-last-speech/actualSpokenContent/sourceSINGLEorMERGED/SHA256` 표 작성. 사용자가 지목한 태온001/폴001 우선 분석, 나머지16 같은 묶음의 위험 전수 점검. 기존 Main09 때와 같이 PCM 내부 잘못된 분할·합본 여부를 확인하되 이전 원인을 그대로 확정 전제하지 않는다. Unity 런타임 Source `clip.name/timeSamples/Stop/Next` 계측도 최소 구간으로 실시.
4. **기존 원본 음성으로만 복구 후보**: 실제 원문 해당 발화가 다른 WAV 구간에 있으면 안전하게 비파괴 추출, 첫 음절/호흡·어절·끝 여운 보호. 정정 새 폴더/SHA 문서·후보 청취 페이지 작성. 필요 원문이 실제 어느 WAV에도 없다면 **그 ID만 `TTS_REQUIRED`**로 분리하고 API/새 TTS 호출0. 기존 WAV/Meta/GUID 덮어쓰기0, Main09/08/18/19·TTS별 승인본 원본 보존.
5. **대표 청취 승인 전에 Main10 신규 복구 후보를 정식 Catalog에 적용하거나 원본 디스크에 반영하지 않는다.** Quest blocker 최소수정은 음성 승인과 독립으로 격리 QA 및 별도 보고 가능. `QuestService` 7 objective·Main09→10 연결, 보상EXP40/탈렌트40, 던전 전환, Save/Continue, 휠체어·3개 화면비·NPC 원복/대화 유지. Main09의 검증550+261·Field06 717 검사를 재실행하지 말고 Main10 delta 문제만 실시.
6. Main10의 `main10_*` 음성27개로 잘못 부르지 말 것. **18개**가 대상. 무음 플레이어 페이지는 제외. Main11·그 뒤 챕터의 음성이 반드시 틀렸다고 미리 결정하지 않고 관련된 동일 제작 배치나 manifest 포맷이 있는 경우 **한 번의 가벼운 정적 위험 검사만**(필요 시 별도 제안). 재생성/대량 전수 QA 자동 확대 금지.
7. 결과물: Main10 18행(+무음 페이지 경계) 재생 원문/실 WAV CSV, 오류 구간 복구 후보/청취 샘플, 치유사 NPC 입력 차단 Before/After 격리 Unity 사진/영상 몇 장, 7 Objective 회귀 delta QA, 원본·Save/Editor 보호 해시, 해당 feature 브랜치/커밋/미승인 목록. **Main10 복구 후보는 대표 청취 승인 게이트 계속 보류**. Codex 5시간 한도 보호를 위해 결과 빨리 공유하고 필요한 경우 재개 프롬프트 저장.

## 3. 임시 사용자 안내 (수정 전)

화면의 `[E] 대화하기`는 치유사 NPC 입력이며 가공된 돌 조사와 다르다. 플레이어를 돌 위치 근처의 다른 각도로 조금 이동해 `[E/F]` 조사 Input이 먹을 수 있는지 확인할 수 있으나 **사용자에게 반복 직접 테스트를 요청하지 않으며** 저장 덮어쓰기/강제 Quest 스킵은 금지. Codex가 치유사 정상 서비스를 보존하면서 수정해야 함.

## 4. Git/계획

작업 전부 17시 이후 Codex 한도 재설정 확인 후 시작. 본 문서 저장·인계 갱신은 **`docs/planning` 브랜치만**. Field06 v1.1/Field07 별도 아트 업무는 이 버그와 분리하고 Codex 한도가 좁으면 **Main10 진행 불가 P1 > Main10 음성 수리 후보 생성 > Field06 후속** 순서. PR/main merge/원본디스크 voice 적용은 사용자 별도 승인에 따라 결정.


## 최신 사용자 확정(2026-10-19) — 원본 복구 음성 18/18 청취 승인

상단의 코드 검사 이전 상태 `18_VOICES_PCM_AUDIT_PENDING`과 `CODEX_DEFERRED`는 **과거 이력**입니다. Codex는 Main10 NPC 입력 차단을 수정하고 **격리132PASS/0FAIL**을 보고했으며, 기존 PCM에서 **후보18/18 복구**했습니다. 바울 대표가 **“다 들었고 다 맞고”**라고 직접 청취 승인했으므로 `MAIN10_USER_LISTENING_APPROVED_18_OF_18`입니다. 기존원본 WAV 삭제/덮어쓰기0, 새 TTS/API0, UserData23·Main09 승인27 보호 보고. 후보 WAV가 **원본 Unity에 정식 연결됐다는 뜻은 아니며** 다음 Codex는 승인 18개의 정확한 SHA·본문·GUID 대응을 지킨 상태로 Catalog 정식 연결과 최소 delta QA/조건부 원본 안전 적용을 별도 수행해야 합니다. 자세한 최신 정본: `Main10_Voice18_User_Approval_Field08_NoNewArt_Decision_20261019.md`.
