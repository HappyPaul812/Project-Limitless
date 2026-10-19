# LIMITLESS — Main09 Field03 사용자 실플레이 대사·음성 불일치 및 재회 순서 수정 계약 (2026-10-19)

- 작성/담당: 기획, `docs/planning`
- 사용자 실측: Unity 6.6 (6000.6.5f1) / `Field_03` / 2026-10-19 캡처 6장과 청취 제보.
- 상태: **USER_RUNTIME_QA_FAIL / SOURCE_SEQUENCE_CONFIRMED / VOICE_PCM_ROOT_CAUSE_NOT_VERIFIED / FIX_NOT_IMPLEMENTED / USER_FINAL_LISTENING_QA_PENDING**.
- 범위: Main09 `침묵 속의 재회`의 음성↔자막 일치와 서사적 대면 순서. Chapter3 신규 개발/Field05 리뉴얼/Main18 재제작과 혼합 금지.
- 근거 소스 (main `675988c9f2c5c507d9a696b0378be27c0bd3309b`): `Unity/Client/Assets/_Project/Scripts/World/MainQuest09FieldFlow.cs` (`MainQuest09Dialogue.Lines`), `Resources/QuestDefinitions/Main09_ReunionInSilence.asset`, `Resources/Audio/Voice/Story/StoryVoiceCatalog.asset`, `Scripts/UI/DialoguePresenter.cs`, `Scripts/Audio/VoicePlaybackSource.cs`. 과거 유사 오류의 원인 검증 절차는 `Main08_Field02_Field03_Taeon007_011_Voice_Content_Audit_20261014.md` 참조.

## 1. 실플레이 제보 — 확정된 화면/청취 관찰과 기술적 원인 미확정 구분

| 현재 자막/화자(발췌) | 코드상 dialogueId | 바울 대표 청취 제보 | 판정 |
| --- | --- | --- | --- |
| 미엘 ‘빛은 이쪽에서 보였어요. 지금은 사라졌네요.’ | `main09_miel_001` | 무음 | USER_FAIL; 음원 실제 상태·재생 경로 미검증 |
| 태온 ‘빛의 정체는 아직 모릅니다. 남아 있는 흔적부터 보죠.’ | `main09_taeon_001` | ‘…모릅니다.’에서 음성 종료 | USER_FAIL; 본문 후반 누락/조기 중단 원인 미검증 |
| 태온 ‘두 줄의 폭이 일정합니다. 이번에는 깊게 빠진 자국이 아니에요.’ | `main09_taeon_002` | 앞 페이지의 ‘남아 있는…’ 음성 재생 | USER_FAIL; 한 단계 밀림 유사, 단순 ID 오프셋 단정 금지 |
| 미엘 ‘단단한 지면을 따라 이어져 있네요. 폴 씨도 이쪽으로 가셨을까요?’ | `main09_miel_002` | 해당 문장 대신 다른 대사 2개 혼합 | USER_FAIL; WAV 내부 구간/AudioSource 조사 |
| 폴 ‘그 길은 괜찮습니다! 오른쪽 가장자리만 피해 주세요.’ | `main09_paul_001` | 엉뚱한 대사 | USER_FAIL |
| 폴 ‘또 뵙네요. 이번에는 제가 길을 안내해드릴 차례인가 봅니다.’ | `main09_paul_002` | 무음 | USER_FAIL |

나머지 Main09 등록 대사 `main09_*` 21개는 제보가 없는 것이지 청취 PASS가 아니다. 기존 Catalog에서 Main09 **27개 ID가 등록**되어 있으며 직접 조회한 위 6개와 `main09_miel_003`의 GUID는 동일 이름 WAV.meta와 일치한다. **GUID 일치만으로 실제 WAV 발화의 동일성이나 런타임 음성 정상 재생을 입증하지 못한다**. 사용자 캡처·청취가 표시한 현상은 실제이고 원인 분류는 Codex의 로컬 WAV/PCM 및 런타임 계측 후에만 확정한다.

## 2. 실제 코드 대사 순서 및 서사 수정 계약

`MainQuest09Dialogue.Lines(step)`의 현재 순서는:
- step 0 푸른빛 조사: `miel_001 → taeon_001 → 플레이어 무음`
- step 1 평행한 바퀴자국: `taeon_002 → miel_002 → paul_001(‘그 길은 괜찮습니다!’)`
- step 2 폴 첫 대면: `paul_002(‘또 뵙네요’) → miel_003 → paul_003 → taeon_003 → paul_004 → 플레이어 무음`
- step 3 폴에게 재대화/조사 정보 교환, step 4 석재 조사, step 5 폴에게 재대화/합류.

**문제:** `paul_001` 음성·자막은 step 1에서 폴을 만나기 전에 재생되지만 실제 폴 오브젝트는 `index >= 2`에서 활성화된다. 화면과 내용 모두 폴이 누구에게서 말하는지 불명확한 선행 대사. 바울 대표는 재회 인사가 먼저 나오는 것이 맞다고 명확히 지적했다.

**수정 요구:** step 1에서 폴의 말 `main09_paul_001`을 제거하고, step 2의 `main09_paul_002` 첫 인사 뒤 자연스러운 자리에 **동일 문장·동일 원래 dialogueId로 한 번만** 이동한다. 구체적 배치는 폴이 일행 상태를 확인하고 길을 설명한 직후 등 대화 의미가 통하는 자리로 정하되, 바울 대표가 말한 ‘또 뵙네요’가 폴 첫 발화라는 규칙을 지킨다. 데이터/대사 재녹음이 불필요하도록 원문과 Voice ID를 보존한다. 외부 불가시 폴 음성 원거리 연출을 억지로 추가하지 않는다. step 1은 태온·미엘·플레이어(필요시 최소 기존 문구)로 끝내며 퀘스트 목표의 의미는 동일하다.

**절대 보호:** 기존 6개 Objective ID/Target/순서, `main_08` 선행 완료 조건, `main_09` 보상 EXP35/탈렌트35, 폴 합류/로스터, NPC·몬스터·Collider·Scene·Save/Continue, Main08 이미 승인한 WAV/meta/Catalog·Main18 10개 및 전체 TTS 승인본, 사용자 편집·UserData. 사용자 명시 승인 없이 TTS API 유료 호출, 기존 WAV overwrite, main 직접 커밋/Push, Editor 창 포커스 변경 금지.

## 3. Codex 구현 전 단계 — '왜' 음성이 잘못 들리는지부터

1. 먼저 GitHub 원격 최신 main/원본 로컬 동기화 및 dirty308/untracked548 보호, `AGENTS`와 본 문서, 기존 Main08 오류 감사/회복 보고 확인. 새 `codex/main09-dialogue-voice-fix-20261019` worktree에서 조사. 원본 Unity Editor Play/Save/포커스 무조작.
2. `MainQuest09Dialogue.Lines(0~5)` 전체 **27개 NPC 음성**의 `step/페이지/화자/원문/dialogueId/Catalog speakerId/clip GUID/WAV 경로/.meta GUID/SHA256/PCM 실제 본문/leading silence/clipLength`를 1행1발화 CSV 작성. 플레이어 무음 페이지는 별도 표시. 첫 6개 원인분석 우선, 원본 오디오 재생의 `clip.name`, `timeSamples`, `isPlaying`, `Stop/Next` 및 비정상 이전 clip 잔류 검사.
3. 카탈로그 GUID와 WAV.meta가 같더라도 *파일 내부*가 다른 문장이거나 2문장 합본/앞뒤 누락일 수 있다. 001무음, taeon001 후반 소실, taeon002 선행 음성, miel002 혼합, paul001 오발화, paul002 무음을 각 파일 PCM/타임라인과 계측 결과로 분류. **무조건 한 번호씩 앞으로 당기는 기계식 relink 금지**.
4. 소스 PCM 중 정확한 문장이 있는 경우 기존 승인본 수정 없이 **별도 후보 WAV**에서 문장별 분리·소량의 선행 진짜 무음 trim만 수행. 발화/호흡/어절 잘림·오디오 합성 왜곡 금지. 실제 문장이 존재하지 않으면 해당 ID만 `TTS_REQUIRED`로 분리, 사용자 승인 전 생성 금지.
5. 수리 후보의 오디오/Mapping CSV·짧은 청취 세트를 제작하여 바울 대표 확인. 재회 순서 코드 수정은 새 audio ID 없이 최소 diff. 공식 음성 연결은 청취 승인 및 참조 정합 후 별도 단계로 진행.

## 4. 검수 게이트

- `Field_03`에서 평범한 Main08 완료 Save로 Main09 진입하여 **step0→1→2→3→4→5 실제 순서**와 재입장·취소·Continue 검증. 별도 강제플레이 테스트용 기능/개발 빌드 추가 금지.
- **6개 제보 사례 모두** 자막의 첫 글자부터 끝 글자까지 해당 인물 목소리 한 개만 재생, 원문 완성, 무음/합본/넘어간 음성/앞뒤 겹침0. 27개 NPC 음성의 ID/원문과 실제 파일 대응 정적 감사. 플레이어·지문 무음 유지.
- step1에는 폴의 대사가 등장하지 않고, step2 **폴 첫 대사가 ‘또 뵙네요’**이며 실제 폴 오브젝트를 보고 대면한 후 대화가 이어짐. `paul_001`이 step2에 정확히 1회 존재.
- 수동으로 페이지를 일찍 넘기면 이전 음성이 정상 중단되고 다음 대사로만 전환; 끝까지 듣는 동안 타이머 강제 종료 없음. Unity 원본의 Volume/Voice Mixer/Clip/AudioSource 길이와 GUI 입력 이벤트 모두 점검.
- 퀘스트6목표, 보상35/35·폴 합류1회·Save/Continue, 16:9/4:3/21:9 UI/도보·휠체어/몬스터·Field02 왕복 무회귀. Main08/18/19·Field05/기존 모든 승인 자산 SHA 불변.
- 검수 상태는 `STATIC_PASS`, `ISOLATED_RUNTIME_PASS`, `USER_LISTENING_APPROVED`, `ORIGINAL_EDITOR_NOT_VERIFIED`를 분리. 과거 Search/URP 예외는 기준선과 비교하여 별도 보고. **QA 성공 이전에 완료/PASS로 선언 금지**.

## 5. 다음 전달

Codex는 **먼저 원인 읽기 전용 감사와 재사용 후보**를 보고한 뒤 최소 코드/오디오 교정 단계로 넘어간다. 커밋은 `codex/*`에만, main 병합은 바울 대표 승인 뒤; 기획 문서는 `docs/planning`에만. 전체 Main09를 TTS로 재제작하지 않으며 기존 27개 후보를 처음부터 파괴하지 않는다. 문제와 무관한 Chapter3/Main20~21 TTS 발주를 겹치지 않는다.


## 2026-10-19 후속 — 복구 27개 전부 사용자 청취 승인

Codex 로컬 `codex/main09-dialogue-voice-fix-20261019` 보고: GUID/Catalog/meta 27개 일치, 기존 WAV 내부 잘못된 분할·합본이 문제, 원본 발화 기반 후보 **27개 복원**, 격리 Unity **550 PASS/0 FAIL**, 원본4,180파일·UserData23 SHA 보호, TTS/API0, 원본 교체/Push/main병합0. ASR 20개 원문 일치 / 7개 청취 필요 보고를 받은 바울 대표가 **“다 들어봤는데 다 맞아”**라고 확인. **27개 전부 `USER_LISTENING_APPROVED`**, 복구 후보 ID·문장 짝 청취 승인 게이트 해제. 이는 원본 Unity에 승인 WAV가 이미 정식 적용됐다는 주장은 아님. 폴 `paul_001`이 step2 `paul_002`의 “또 뵙네요” 뒤로 이동된 구현안과 기존 6단계/35EXP·35탈렌트 보호는 동일하게 유지. 승인 WAV의 개별 SHA는 Codex 최종 후보 매핑 정본 사용, 임의 재제작/새 TTS 금지. **다음: feature 상 승인27만 정식 연결+delta QA+비파괴 원본 적용(안전검증 충족 시), PR/main 통합 별도 지시**. 후속 명세 `Main09_Voice27_Approval_Field06_Art_v1_Handoff_20261019.md`.
