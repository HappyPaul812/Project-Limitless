## Codex 후속 구현 완료 — 2026-10-24 저녁

제품 commit `1cb99be267f1653e5bd6e8d2e3fb06d783a75ba3`, 브랜치 codex/tts-voice-integration-20261024. 승인162개 적용(140교체+22신규), Catalog290→312·기존140GUID/meta·비대상150연결 보존. 원본1430파일불변. 숨김batch Play1031 PASS·재생시작162/162, Save/무음/Next/Skip/음량/주요대사배열PASS. 상세 환경 차이·EditorSearch예외1·전체퀘스트/독립전량청취미검증은 [최종QA](TTS_162WAV_Unity_Integration_QA_20261024.md) 참조. main병합/Push/원본동기화0·별도승인대기. 아래는 docs/tts 정본의 착수 전 인계 이력이다.

# LIMITLESS / 2026-10-24 저녁 Codex Unity 음성 일괄 통합 인계

**상태:** 2026-10-24 사용자 "좋아 다 잘됐어 이따 7시쯤에 다 넣게 코덱스 지시문 줘" — **하렌 R2.1 및 02/03 음성을 사용자 청취 PASS로 보고, 19시경 Codex에게 Unity 일괄 연결 구현 요청 예정**. 본 문서 준비만으로 실제 Unity 편집·사용자 로컬 파일 수정·main 병합이 승인/실행된 것은 아님.
**브랜치:** `docs/tts`에서 계획 기록. 코드/Unity 작업은 Codex의 별도 `codex/tts-voice-integration-20261024` worktree. **`main` 직접 수정·push·병합/원본 로컬 동기화는 별도 사용자 승인.**
**원칙:** TTS의 생성·Google API 호출·오디오 제작은 사용자 담당. **Codex는 기존 최종 WAV를 읽고 검수·Unity 연결·회귀 테스트만 수행**.

## 사용자 승인 대상 및 출처

| 묶음 | 수량 | 화자 | 소스 | 품질·승인 상태 |
|---|---:|---|---|---|
| 전면 재캐스팅 | 67 | 세린 `companion_serin` / `Erinome` | `F:\Downloads\Renewal_Voices\Serin` | 실제 로그 `67/67 READY`. 저장 기술상 생성완료; 사람 전량 청취를 별도로 했다고 꾸미지 말 것 |
| 전면 재캐스팅 | 85 | 태온 `companion_taeon` / `Algieba` | `F:\Downloads\Renewal_Voices\Taeon` | 실제 로그 `85/85 READY` |
| 하렌 최초 3대사 | 3 | 하렌 `story_haren` / `Alnilam` | `F:\Downloads\Voice\Haren_Main21\Haren` | 사용자 청취: 01 R2.1 억양 수정 승인; 이어 02·03도 "다 잘됐어" 합격 보고. Codex 로컬 파일/메타 재검증 필요 |
| Main21 기존 보존 | 7 | 레온2 `Orus`, 미엘2 `Sulafat`, 폴3 `Achird` | `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019\candidates` | 사용자 보존 지정 KEEP/재합성 금지. 실제 WAV SHA·발화 검사는 Codex가 읽기 전용 시행 |
| **합계** | **162** | | | **162개 통합 매핑 대상**, 반드시 모두 새로 추가한다는 뜻은 아님 |

**중요 수량 구조:** 이전 `main` 조사에서 세린58개·태온82개 = 기존 StoryVoiceCatalog 등록140개의 **음성 클립 교체 대상**, Main20 세린5·Main21 세린4/태온3 = 12개 신규/미등록 대사, Main21 하렌3·보존 폴3/미엘2/레온2 = 10개 추가 대상이었다. 따라서 **162 대상 = 기존140 매핑 재캐스팅 + 신규22 후보**라는 역사적 계약. 실제 최신 로컬 Scene/Story Catalog에 이미 추가된 ID가 있다면 중복 생성 금지. Game runtime 원본과 branch의 최신 ID·목소리를 재확인 후 실제 delta 집계하라. 기존 등록 290이 무조건 312여야 한다고 하드코딩하지 말 것.

### 하렌 가장 중요한 선택 규칙

- 반드시 `main21_haren_haren_01` **“태온. 너 아직 멀었어.”**는 사용자 청취 PASS한 **R2.1 최종 승인본**만 사용. 후속 승인 시 사용자가 `Haren_Prosody_Retake_R2_1.py approve`로 `APPROVE_FALLING_R2`을 입력했고 콘솔에서 `[QA APPROVED]`와 정식 경로로 교체되었다고 확인됨. R2 후보 경로 `F:\Downloads\Voice\Haren_Main21\ProsodyRetakes\R2\Haren\main21_haren_haren_01.wav`도 SHA 비교 대상. **동일 ID 최종 후보 SHA를 승인 마커·메타와 대조하고 정식 경로/후보 중 승인본만 선택**할 것. 실패한 초기 하렌 샘플 또는 과거 `rejected`, `samples`, `ProsodyRetakes`의 미승인 후보를 자동 스캔해 Unity에 편입 금지.
- `main21_haren_haren_02` **“다친 데는 없고?”** 자연스러운 질문 / `main21_haren_haren_03` **“그럼 됐어.”** 하강 평서문. 사용자 "다 잘됐어"로 02/03 청취 PASS 보고됐지만 실제 WAV·메타가 존재하는지 Codex 별도 검증. 무단 재합성0.

### 기존 Main21 7개 파일명(KEEP, 다운로드/재생성0)

```text
main21_carriage_leon_01.wav
main21_aid_miel_02.wav
main21_aid_paul_01.wav
main21_aid_miel_01.wav
main21_arbel_leon_01.wav
main21_route_paul_02.wav
main21_route_paul_01.wav
```

## Codex 실행 단계 — 저녁 7시 메시지를 받은 뒤 순서대로

1. **최신 정본/현황 선행:** repo `AGENTS.md`, `문서/00_프로젝트/PROJECT_CONTEXT.md`, `CURRENT_STATUS.md`, `CODEX_WORKFLOW.md`, `docs/tts`의 `TTS_LATEST.md`, `TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json`, `TTS_Main21_기존보존_7WAV_통합대기_20261022.md`, `TTS_Main21_Haren_R2_1_기존WAV실종_메타장부검증_20261024.md`, 이 문서, Issue #20과 최신 local/main 변경을 확인. 문서가 현재 로컬 Unity runtime/파일과 다르면 추측 대신 차이를 진단.
2. **작업 안전:** 기존 로컬 Unity 프로젝트/다운로드 소스가 수정된 상태인지 `git status`, 최신 HEAD·remote 확인. 승인된 TTS 업무용 별도 `codex/tts-voice-integration-20261024` worktree/branch 생성. 원본 `main` 작업폴더·Library·Save/UserData/Editor 개인설정·장면·사용자 다른 미커밋 파일은 직접 편집/clean/reset/stash/force 금지. 작업 전 보호 원본 SHA 목록 작성. 파일 복사는 프로젝트 내부 승인 대상 경로로만.
3. **원본 감사(읽기 전용):** 위 세 경로의 WAV 실존, Stable Dialogue ID, 음성 길이, wav PCM sample rate/채널, 헤더, 무음·클리핑 후보, 동일 ID 중복, SHA256·sidecar 원문 hash·모델/voice, 152 manifest의 67/85 카운트, 하렌3 최종 승인 SHA, Main21 기존7 목록을 검증. `F:\Downloads\Voice\API.txt` 등 키와 민감 데이터를 열거나 출력하지 말 것. **Google API/TTS 제작/재시도 금지.** QA 의심 목록은 표시하고 승인본이 아닌 임시/오류 오디오 자동 적용 금지.
4. **매핑 전수감사:** Unity `Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset`, 연결된 Unity AudioClip .meta GUID, 실제 Story Dialogue/Scene/Prefab Runtime mapping, Main20 `문서/00_프로젝트/Main20_TTS_Manifest.csv`, Main21 `문서/03_스토리/Main21_TTS_Manifest_20261010.json`을 검사. 분기·대사 원문·SpeakerId·ID가 일치하는 경우만 적용. **1개 Story Dialogue ID = 1개 의도한 음성**; 이미 있는 Main20·21 같은 ID를 또 append하지 말고 재사용/교체. Player·지문 `VoiceExpected=false`는 계속 무음.
5. **실제 적용:** 기존 `StoryVoiceCatalog` 등록 ID 중 세린58·태온82 등은 **기존 AudioClip GUID를 보존할 수 있는 방식**(클립 원본 바이트를 복사 전에 SHA 보호·.meta 유지 등 현재 프로젝트 구조에 적합한 방식)으로 합격 음성을 연결. 단, 이전 화자 원본/승인 WAV를 버리거나 덮어써 없애지 말 것; 격리 백업 후 Unity importer/파일 형식이 적합한지 확인. 신규 Main20·Main21 미등록 ID는 일관된 Assets/_Project 아래 폴더에 임포트, Unity가 정상 GUID 생성, 필요한 Catalog/Voice routing을 추가. 대본 변경/Stable ID 리네임/다른 기존 Voice/Music/SFX/Save/Quest 변경금지. `VoiceExpected`인데 파일이 없으면 ID별 BLOCKED로 보고하고 다음 미검증 대사에 임의 다른 WAV를 꽂지 않는다.
6. **효과/회귀 검증:** 162 대상의 `covered_id` / `correct_speaker_id` / `clip_guid_valid` / `source_file_sha` / `source_text_hash` / `technical_QA` / `listening_approval` 표 작성. 새 ID 중복0, None/null clip0, 잘못된 등장인물 음색0, 기존 세린/태온 이전 목소리 혼용0, 기존 폴·미엘·레온 타 챕터 정상 오디오 영향0, Story 진행·대사 순서/자막/Next/Skip/Voice On-Off/플레이어 무음/Path 분기/퀘스트·SaveLoad 회귀0. Main20 Boss 후 대사, Main21 마차·우회로·보급/치료·하렌 형제 대사 우선 샘플링.
7. **검증 실행 방법:** 대상 변경 관련 정적 참조 검사 및 `git diff --check`, Unity Compile, 가능하면 Unity batchmode/UTF 또는 포커스 이동 없는 MCP Console·Play Mode. **사용자의 현재 작업 화면·마우스/키보드·Unity 창 포커스를 강제로 전환 금지**. Play Mode 실행이 사용자 작업을 방해하거나 테스트가 미실행이면 정직하게 PARTIAL 표시하고 수동 검증 항목 작성. 전체 게임 기존 정상 테스트 재실행보다는 변경 영향 범위 우선.
8. **최종 보고/문서화:** 제품 변경 파일 정확 목록·세 린/태온/하렌/KEEP7 매핑별 READY/APPLIED/BLOCKED 수량, 기존 연결 교체·신규 ID 추가 별도 카운트, SHA/GUID 안전성, 에러/누락·청취검수 분리, 테스트 통과 여부와 Unity Play Mode 실행 여부를 한국어로 간결하게 제출. `CURRENT_STATUS.md` 및 적절한 인계에 실제 적용 결과와 브랜치/커밋 SHA 기록. **코드용 `codex/` 작업 브랜치에만 commit.** 기존 GitHub docs/tts 파일을 main에 덮어쓰지 않는다. **Git push/PR 병합/main 직접 commit/원본 로컬 동기화는 사용자 별도 명시적 승인까지 HOLD**. 필요하면 diff/PR 준비 상태만 보고.
9. **불변 기준:** 작업 완료를 "162개의 신생 WAV를 전부 새로 Unity에 중복 추가"로 판단하지 않는다. 이는 통합 대상 ID/클립의 합집합이며 실제 새로운 AudioClip Asset 또는 Catalog 항목 수와 다르다. 잘못된 화자·동일 ID 중복·경로 불명/메타 SHA 불일치·하렌 01 미승인 원본이 있으면 해당 항목을 수동 HOLD로 구분하고 **안전하게 부분 성공 + 예외 목록**을 보고하라. 불완전한 상태를 PASS라고 꾸미지 않는다.

**사용자 승인 해석:** 2026-10-24 "이따 7시쯤에 다 넣게 코덱스 지시문 줘"는 Codex의 **음성 Unity 통합 착수 및 기능 브랜치 구현** 승인으로 취급. 7시 이전에 자동으로 작업을 수행하라는 일정 예약 요청이 아니다. 별도 지시 없는 `main` merge/원본 동기화는 자동 승인 아님. TTS 생성/Google API는 끝까지 사용자 본인이 담당.

## 참조

- [현재 TTS 후임 인계](팀운영/인계/TTS_LATEST.md)
- [R2.1 하렌 첫 대사 오류/복구](TTS_Main21_Haren_R2_1_기존WAV실종_메타장부검증_20261024.md)
- [Main21 KEEP WAV7](TTS_Main21_기존보존_7WAV_통합대기_20261022.md)
- [152 Manifest](TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json)
- [Main21 Story Manifest](../03_스토리/Main21_TTS_Manifest_20261010.json)
- [Codex 작업 절차](CODEX_WORKFLOW.md)
- [TTS Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20)


---

## EOD 2026-10-24: previous main history retained

# LIMITLESS / 2026-10-24 저녁 Codex Unity 음성 일괄 통합 인계

**상태:** 2026-10-24 사용자 "좋아 다 잘됐어 이따 7시쯤에 다 넣게 코덱스 지시문 줘" — **하렌 R2.1 및 02/03 음성을 사용자 청취 PASS로 보고, 19시경 Codex에게 Unity 일괄 연결 구현 요청 예정**. 본 문서 준비만으로 실제 Unity 편집·사용자 로컬 파일 수정·main 병합이 승인/실행된 것은 아님.
**브랜치:** `docs/tts`에서 계획 기록. 코드/Unity 작업은 Codex의 별도 `codex/tts-voice-integration-20261024` worktree. **`main` 직접 수정·push·병합/원본 로컬 동기화는 별도 사용자 승인.**
**원칙:** TTS의 생성·Google API 호출·오디오 제작은 사용자 담당. **Codex는 기존 최종 WAV를 읽고 검수·Unity 연결·회귀 테스트만 수행**.

## 사용자 승인 대상 및 출처

| 묶음 | 수량 | 화자 | 소스 | 품질·승인 상태 |
|---|---:|---|---|---|
| 전면 재캐스팅 | 67 | 세린 `companion_serin` / `Erinome` | `F:\Downloads\Renewal_Voices\Serin` | 실제 로그 `67/67 READY`. 저장 기술상 생성완료; 사람 전량 청취를 별도로 했다고 꾸미지 말 것 |
| 전면 재캐스팅 | 85 | 태온 `companion_taeon` / `Algieba` | `F:\Downloads\Renewal_Voices\Taeon` | 실제 로그 `85/85 READY` |
| 하렌 최초 3대사 | 3 | 하렌 `story_haren` / `Alnilam` | `F:\Downloads\Voice\Haren_Main21\Haren` | 사용자 청취: 01 R2.1 억양 수정 승인; 이어 02·03도 "다 잘됐어" 합격 보고. Codex 로컬 파일/메타 재검증 필요 |
| Main21 기존 보존 | 7 | 레온2 `Orus`, 미엘2 `Sulafat`, 폴3 `Achird` | `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019\candidates` | 사용자 보존 지정 KEEP/재합성 금지. 실제 WAV SHA·발화 검사는 Codex가 읽기 전용 시행 |
| **합계** | **162** | | | **162개 통합 매핑 대상**, 반드시 모두 새로 추가한다는 뜻은 아님 |

**중요 수량 구조:** 이전 `main` 조사에서 세린58개·태온82개 = 기존 StoryVoiceCatalog 등록140개의 **음성 클립 교체 대상**, Main20 세린5·Main21 세린4/태온3 = 12개 신규/미등록 대사, Main21 하렌3·보존 폴3/미엘2/레온2 = 10개 추가 대상이었다. 따라서 **162 대상 = 기존140 매핑 재캐스팅 + 신규22 후보**라는 역사적 계약. 실제 최신 로컬 Scene/Story Catalog에 이미 추가된 ID가 있다면 중복 생성 금지. Game runtime 원본과 branch의 최신 ID·목소리를 재확인 후 실제 delta 집계하라. 기존 등록 290이 무조건 312여야 한다고 하드코딩하지 말 것.

### 하렌 가장 중요한 선택 규칙

- 반드시 `main21_haren_haren_01` **“태온. 너 아직 멀었어.”**는 사용자 청취 PASS한 **R2.1 최종 승인본**만 사용. 후속 승인 시 사용자가 `Haren_Prosody_Retake_R2_1.py approve`로 `APPROVE_FALLING_R2`을 입력했고 콘솔에서 `[QA APPROVED]`와 정식 경로로 교체되었다고 확인됨. R2 후보 경로 `F:\Downloads\Voice\Haren_Main21\ProsodyRetakes\R2\Haren\main21_haren_haren_01.wav`도 SHA 비교 대상. **동일 ID 최종 후보 SHA를 승인 마커·메타와 대조하고 정식 경로/후보 중 승인본만 선택**할 것. 실패한 초기 하렌 샘플 또는 과거 `rejected`, `samples`, `ProsodyRetakes`의 미승인 후보를 자동 스캔해 Unity에 편입 금지.
- `main21_haren_haren_02` **“다친 데는 없고?”** 자연스러운 질문 / `main21_haren_haren_03` **“그럼 됐어.”** 하강 평서문. 사용자 "다 잘됐어"로 02/03 청취 PASS 보고됐지만 실제 WAV·메타가 존재하는지 Codex 별도 검증. 무단 재합성0.

### 기존 Main21 7개 파일명(KEEP, 다운로드/재생성0)

```text
main21_carriage_leon_01.wav
main21_aid_miel_02.wav
main21_aid_paul_01.wav
main21_aid_miel_01.wav
main21_arbel_leon_01.wav
main21_route_paul_02.wav
main21_route_paul_01.wav
```

## Codex 실행 단계 — 저녁 7시 메시지를 받은 뒤 순서대로

1. **최신 정본/현황 선행:** repo `AGENTS.md`, `문서/00_프로젝트/PROJECT_CONTEXT.md`, `CURRENT_STATUS.md`, `CODEX_WORKFLOW.md`, `docs/tts`의 `TTS_LATEST.md`, `TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json`, `TTS_Main21_기존보존_7WAV_통합대기_20261022.md`, `TTS_Main21_Haren_R2_1_기존WAV실종_메타장부검증_20261024.md`, 이 문서, Issue #20과 최신 local/main 변경을 확인. 문서가 현재 로컬 Unity runtime/파일과 다르면 추측 대신 차이를 진단.
2. **작업 안전:** 기존 로컬 Unity 프로젝트/다운로드 소스가 수정된 상태인지 `git status`, 최신 HEAD·remote 확인. 승인된 TTS 업무용 별도 `codex/tts-voice-integration-20261024` worktree/branch 생성. 원본 `main` 작업폴더·Library·Save/UserData/Editor 개인설정·장면·사용자 다른 미커밋 파일은 직접 편집/clean/reset/stash/force 금지. 작업 전 보호 원본 SHA 목록 작성. 파일 복사는 프로젝트 내부 승인 대상 경로로만.
3. **원본 감사(읽기 전용):** 위 세 경로의 WAV 실존, Stable Dialogue ID, 음성 길이, wav PCM sample rate/채널, 헤더, 무음·클리핑 후보, 동일 ID 중복, SHA256·sidecar 원문 hash·모델/voice, 152 manifest의 67/85 카운트, 하렌3 최종 승인 SHA, Main21 기존7 목록을 검증. `F:\Downloads\Voice\API.txt` 등 키와 민감 데이터를 열거나 출력하지 말 것. **Google API/TTS 제작/재시도 금지.** QA 의심 목록은 표시하고 승인본이 아닌 임시/오류 오디오 자동 적용 금지.
4. **매핑 전수감사:** Unity `Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset`, 연결된 Unity AudioClip .meta GUID, 실제 Story Dialogue/Scene/Prefab Runtime mapping, Main20 `문서/00_프로젝트/Main20_TTS_Manifest.csv`, Main21 `문서/03_스토리/Main21_TTS_Manifest_20261010.json`을 검사. 분기·대사 원문·SpeakerId·ID가 일치하는 경우만 적용. **1개 Story Dialogue ID = 1개 의도한 음성**; 이미 있는 Main20·21 같은 ID를 또 append하지 말고 재사용/교체. Player·지문 `VoiceExpected=false`는 계속 무음.
5. **실제 적용:** 기존 `StoryVoiceCatalog` 등록 ID 중 세린58·태온82 등은 **기존 AudioClip GUID를 보존할 수 있는 방식**(클립 원본 바이트를 복사 전에 SHA 보호·.meta 유지 등 현재 프로젝트 구조에 적합한 방식)으로 합격 음성을 연결. 단, 이전 화자 원본/승인 WAV를 버리거나 덮어써 없애지 말 것; 격리 백업 후 Unity importer/파일 형식이 적합한지 확인. 신규 Main20·Main21 미등록 ID는 일관된 Assets/_Project 아래 폴더에 임포트, Unity가 정상 GUID 생성, 필요한 Catalog/Voice routing을 추가. 대본 변경/Stable ID 리네임/다른 기존 Voice/Music/SFX/Save/Quest 변경금지. `VoiceExpected`인데 파일이 없으면 ID별 BLOCKED로 보고하고 다음 미검증 대사에 임의 다른 WAV를 꽂지 않는다.
6. **효과/회귀 검증:** 162 대상의 `covered_id` / `correct_speaker_id` / `clip_guid_valid` / `source_file_sha` / `source_text_hash` / `technical_QA` / `listening_approval` 표 작성. 새 ID 중복0, None/null clip0, 잘못된 등장인물 음색0, 기존 세린/태온 이전 목소리 혼용0, 기존 폴·미엘·레온 타 챕터 정상 오디오 영향0, Story 진행·대사 순서/자막/Next/Skip/Voice On-Off/플레이어 무음/Path 분기/퀘스트·SaveLoad 회귀0. Main20 Boss 후 대사, Main21 마차·우회로·보급/치료·하렌 형제 대사 우선 샘플링.
7. **검증 실행 방법:** 대상 변경 관련 정적 참조 검사 및 `git diff --check`, Unity Compile, 가능하면 Unity batchmode/UTF 또는 포커스 이동 없는 MCP Console·Play Mode. **사용자의 현재 작업 화면·마우스/키보드·Unity 창 포커스를 강제로 전환 금지**. Play Mode 실행이 사용자 작업을 방해하거나 테스트가 미실행이면 정직하게 PARTIAL 표시하고 수동 검증 항목 작성. 전체 게임 기존 정상 테스트 재실행보다는 변경 영향 범위 우선.
8. **최종 보고/문서화:** 제품 변경 파일 정확 목록·세 린/태온/하렌/KEEP7 매핑별 READY/APPLIED/BLOCKED 수량, 기존 연결 교체·신규 ID 추가 별도 카운트, SHA/GUID 안전성, 에러/누락·청취검수 분리, 테스트 통과 여부와 Unity Play Mode 실행 여부를 한국어로 간결하게 제출. `CURRENT_STATUS.md` 및 적절한 인계에 실제 적용 결과와 브랜치/커밋 SHA 기록. **코드용 `codex/` 작업 브랜치에만 commit.** 기존 GitHub docs/tts 파일을 main에 덮어쓰지 않는다. **Git push/PR 병합/main 직접 commit/원본 로컬 동기화는 사용자 별도 명시적 승인까지 HOLD**. 필요하면 diff/PR 준비 상태만 보고.
9. **불변 기준:** 작업 완료를 "162개의 신생 WAV를 전부 새로 Unity에 중복 추가"로 판단하지 않는다. 이는 통합 대상 ID/클립의 합집합이며 실제 새로운 AudioClip Asset 또는 Catalog 항목 수와 다르다. 잘못된 화자·동일 ID 중복·경로 불명/메타 SHA 불일치·하렌 01 미승인 원본이 있으면 해당 항목을 수동 HOLD로 구분하고 **안전하게 부분 성공 + 예외 목록**을 보고하라. 불완전한 상태를 PASS라고 꾸미지 않는다.

**사용자 승인 해석:** 2026-10-24 "이따 7시쯤에 다 넣게 코덱스 지시문 줘"는 Codex의 **음성 Unity 통합 착수 및 기능 브랜치 구현** 승인으로 취급. 7시 이전에 자동으로 작업을 수행하라는 일정 예약 요청이 아니다. 별도 지시 없는 `main` merge/원본 동기화는 자동 승인 아님. TTS 생성/Google API는 끝까지 사용자 본인이 담당.

## 참조

- [현재 TTS 후임 인계](팀운영/인계/TTS_LATEST.md)
- [R2.1 하렌 첫 대사 오류/복구](TTS_Main21_Haren_R2_1_기존WAV실종_메타장부검증_20261024.md)
- [Main21 KEEP WAV7](TTS_Main21_기존보존_7WAV_통합대기_20261022.md)
- [152 Manifest](TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json)
- [Main21 Story Manifest](../03_스토리/Main21_TTS_Manifest_20261010.json)
- [Codex 작업 절차](CODEX_WORKFLOW.md)
- [TTS Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20)
