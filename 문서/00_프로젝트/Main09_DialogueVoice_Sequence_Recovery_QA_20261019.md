# Main09 Field03 음성 내부 발화·재회 순서 감사 및 복구 후보 (2026-10-19)

**상태: STATIC_PASS / ISOLATED_SEQUENCE_RUNTIME_PASS / AUDIO_CANDIDATES_READY / USER_FINAL_LISTENING_QA_PENDING / ORIGINAL_EDITOR_NOT_VERIFIED.**

기준 main `675988c9f2c5c507d9a696b0378be27c0bd3309b`, 기획 `docs/planning` `4d6788c52d73d6abb7caa6ef2c6beee706c932f6`의 `Main09_Field03_DialogueVoice_Sequence_Bug_20261019.md`. 브랜치 `codex/main09-dialogue-voice-fix-20261019`, 코드 커밋 `3f35b54bc9f668696d5eec7d1c88b72c8a15e036`. Push/PR/main 병합/원본 디스크 적용0. 음성 정식 교체는 아직 완료되지 않았습니다.

## 원인 — 이름·GUID 정상과 실제 발화 정상은 별개

- 전체 NPC27개+Player 무음5개를 추출했습니다. Catalog ID/화자/GUID와 동명 WAV.meta 대응27/27 PASS. WAV/meta/PCM SHA와 길이·RMS·leading/trailing -45dBFS RMS 구간, 기존/변경 후 단계·페이지는 [전체 CSV](Evidence/Main09_DialogueVoice_20261019/Main09_All27_Mapping.csv)에 있습니다. 침묵 측정은 레벨 기준이며 의미 있는 발화 판정과 다릅니다.
- Unity 실제 AudioClip.GetData의 PCM은 기존 WAV와27/27 일치했습니다. 시작 timeSamples>0, isPlaying, Mixer/Source 음량 설정과 화자별 clip 참조를 계측했습니다. Unity가 임의로 한 번호 다른 WAV를 고른 것이 아니라 **이미 내부 문장이 다른 원본 WAV를 정상 연결해 재생**하고 있었습니다. 페이지 조기 Next/Hide 시 이전 clip 정리는 정상입니다.
- `Google TTS/Limitless_TTS_Output_PreSerin`의 같은 이름 원본27개도 Unity WAV와 byte 동일27/27입니다. 문제는 최신 Main09 UX나 Field05 반영 때 생긴 변형이 아니라 기존 제작 원본에 이미 존재합니다. 최초 적용 커밋은 `ea776a97f95c25ef8041f39c2f8ecea79520925c`입니다. 기존 `audit_story_voice.py`의 MAPPED는 Manifest 화자/텍스트↔코드 일치이며 PCM 청취가 아닙니다. `apply_story_voice.py`는 그 파일을 byte 복사했습니다. 원본 제작 파일의 문장 경계를 잘못 나눈 구체적 생성 알고리즘은 이번 증거만으로 확정하지 않습니다.
- 원본 ASR 원문 완전 일치0/27은 추가 감사 신호입니다. 사용자 제보 외21개를 사람이 청취 FAIL한 것으로 단정하지 않습니다. ASR은 이미 로컬에 있던 faster-whisper small을 offline로만 사용했습니다. 새 모델 다운로드/API/음성 생성0.

| 제보 ID | 확인한 기존 PCM | 복구 후보 근거 |
|---|---|---|
| miel001 | 0.85초, RMS -48.03dBFS, 유효 문장 ASR 없음 | 기존 miel002의 ‘빛은 이쪽…’ 뒤 구간 + miel003의 ‘지금은…’ 무손실 연결 |
| taeon001 | ‘빛의 정체는 아직 모릅니다’만 포함 | 기존 taeon001+002 PCM 연결 |
| taeon002 | 앞 페이지 ‘남아 있는 흔적부터 보죠’ | 실제 ‘두 줄의 폭…’가 있는 taeon004+005 연결 |
| miel002 | 목표 외 발화와 ‘빛은 이쪽…’ 혼합 | 실제 목표 문장이 있는 miel004 byte 복사 |
| paul001 | 원래 paul006의 마력/석재 문장 | 원본 Main07 paul016 앞 문장 분리 |
| paul002 | 1.36초, RMS -49.82dBFS, 유효 문장 ASR 없음 | 원본 Main07 paul016 뒤 재회 인사 분리; ASR의 앞 ‘하’ 추가 인식은 청취 확인 대기 |

번호 이동 규칙을 적용하지 않았습니다. **각 발화 실제 원문과 원본 PCM 구간을 확인해 개별 매핑**했습니다. Main07/08 승인본은 읽기만 했으며 교체0입니다.

## 재회 순서 최소 수정

`MainQuest09FieldFlow.cs` 1파일 4줄 추가/2줄 변경만 기능 수정했습니다. step1(2/6)의 paul001 제거 → step2(3/6)의 `paul002 → miel003 → paul003 → taeon003 → paul004 → paul001 → Player` 순서로 이동했습니다. 폴의 재회 인사가 첫 발화이고 경로 설명 뒤 안내가1회 나옵니다. 27개 화자/원문/ID 삼중값은 수정 전후 완전히 동일, Quest6단계·Target·보상·Scene·Collider·진행 조건 변경0입니다.

## 복구 후보와 승인 경계

- 별도 로컬 `Temp/Main09VoiceFix20261019/Delivery/Candidates` 27 WAV: byte동일 복사12 / 문장 분리11 / PCM 연결4. 분리 경계7개 모두 실제 저레벨 구간 안0샘플, ±10ms peak≤-45dBFS이며 양쪽 분리 PCM을 합치면 원본과 동일합니다. gain/fade/normalization/resample/음성 합성0. 연결 후보도 모든 출력 PCM을 출처 frame slice와 byte 대조했습니다.
- ASR 원문 일치20/27(공백·문장부호 제외), 불일치7/27입니다. 정확한 발음·화자·호흡·문장 사이 간격의 사람 청취 승인0입니다. ASR 불일치는 인식 오류일 수 있으므로 신규 TTS 필요로 단정하거나 임의 어절을 잘라 고치지 않았습니다.
- 우선 청취7개: `main09_miel_005`, `main09_taeon_006`, `main09_paul_002`, `main09_paul_003`, `main09_paul_008`, `main09_paul_009`, `main09_paul_012`. miel005의 ‘흙’ 반복 인식, taeon006의 ‘암반→안방’, paul002 앞 ‘하’, paul003/008/009/012의 어절 인식 차이를 대조해야 합니다. 정확한 원음이 없는 것으로 확정된 TTS_REQUIRED0, 청취 후 재제작 필요 여부 미확정7입니다. 새 음성이 필요하다고 판단되면 해당 ID만 별도 보고하고 생성하지 않습니다.
- [로컬 기존/후보 청취 페이지](../../../Delivery/index.html), ZIP `Temp/Main09VoiceFix20261019/Main09_Voice_Recovery_Listening_Candidates_20261019.zip`. 제보6건 먼저, 전체27개 기존/후보/자막/ASR/출처를 비교할 수 있습니다. 원본·후보를 함께 포함한 청취 자료이며 임의 Unity 복사 지시가 아닙니다.
- 기획 정본의 **‘공식 음성 연결은 청취 승인 및 참조 정합 후 별도 단계로 진행’**을 따릅니다. 후보만 격리 Client에 새 GUID/PCM Import로 넣고 **Play 중 Catalog 메모리 복제본**에 연결했습니다. 정식 StoryVoiceCatalog·WAV/meta 파일 변경0. 따라서 후보 Runtime의 정상 재생과 공식 음성 교체 완료를 구분합니다.

## 실제 Unity 변경 중심 QA

원본 Editor/MCP 연결 도구는 제공되지 않아 원본을 조작하지 않았습니다. 기존 격리 `Temp/Field05Review20261019/Client`를 Unity6000.6.5f1 `-batchmode`, Hidden으로 재사용했습니다. 새 게임 빌드/강제 퀘스트 진행 기능/순간이동 기능을 추가하지 않았습니다. 기존427항목의 안내·HUD·Q로그 전체 회귀는 반복하지 않았습니다.

| 범위 | PASS | FAIL | 경계 |
|---|---:|---:|---|
| 기존 음성27개 실제 PCM/시작/Next 기준선 | 195 | 0 | 파일 내용 오류는 별도이며 청취 PASS 아님 |
| 휠체어·키보드 실제 Main08 마지막 목표→Main09 여섯 단계 | 289 | 0 | 27개 후보 전체 길이·Subtitle/PCM/GUID·Next/취소·Save/Continue |
| 도보·게임패드 짧은 여섯 단계 회귀 | 261 | 0 | 전체 길이 음성·화면 캡처 재반복 없이 A/이동/순서/Continue |

- 실제 바울 대표 `save_slot_02`의 **복사본**에서 Main08 마지막 목표 미완료 상태를 이어하기했습니다. 완료 ID/Count를 조작하지 않고 실제 위치 트리거·대화 입력으로 Main08을 완료한 뒤 Main09을 자연 시작했습니다. AuditSaveDirectory/UserSettings BeginAudit로 쓰기는 격리 폴더에만 제한했습니다. 도보 변형은 같은 복사본에서 Path를 검증 메모리상 vision으로 선택했고 Quest 진행은 우회하지 않았습니다.
- 여섯 단계 순서, 폴 비대면 발화0, 첫 대사 paul002, paul001 1회,27ID중복0, 조기 Next한페이지만 진행/직전음성 정리, 취소무진행/clip null, 2/6·3/6·5/6 및 완료 후 Bootstrap 실제 Continue, EXP35/탈렌트35·합류·보상중복0을 검증했습니다. 물리 OS 입력은 사용하지 않았고 InputSystem 가상 장치만 사용했습니다.
- 16:9 1280×720 / 4:3 960×720 / 21:9 1680×720의 실제 Unity 화면에서 재회 첫 대사·인물·본문·조작 안내가 보이며 잘림 없음을 확인했습니다. [16:9](Evidence/Main09_DialogueVoice_20261019/Greeting_1280.png), [4:3](Evidence/Main09_DialogueVoice_20261019/Greeting_960.png), [21:9](Evidence/Main09_DialogueVoice_20261019/Greeting_1680.png).
- 최종 신규 Runtime Console 오류0, 컴파일 오류0. 기존 Search/URP 예외·비권장 API 경고는 기준선과 구분했습니다. 초기 실패4회는 검증 도구의 위치 트리거 이동 잠금 인식, Batch 가상 키보드 포커스 정책(Enable만으로는 해결 안 됨), 새 청취 후보의 기본 압축 Import로 생긴 PCM 차이였습니다. 모두 로그 `AfterAttempt1~4`에 보존하고 마지막 결과와 섞지 않았습니다. Game/원본 코드·Settings를 바꿔 우회하지 않았고 Batch 메모리의 InputSettings만 AllDeviceInputAlwaysGoesToGameView로 지정했습니다.
- Field02 왕복·전투·몬스터 전체 QA는 이번 delta에서 반복하지 않았으며 관련 Scene/Collider/Spawn/기존 시스템은 SHA로 보호했습니다. 원본 Editor의 실제 청취/포커스/자연 Import는 미검증입니다.

## 보호와 다음 재개

원본 Unity Assets·UserData·UserSettings·ProjectSettings·Packages 4180파일 SHA 불일치0, UserData 23파일 유지, root git status byte동일, main HEAD `675988c9f2c5c507d9a696b0378be27c0bd3309b` 유지. 원본 원음 출처43파일도 SHA 유지. Main08/18/19 승인 WAV/meta/Catalog·Field05·미커밋·Save·Settings를 교체하지 않았습니다. 보호 증거는 [FinalProtection.json](Evidence/Main09_DialogueVoice_20261019/FinalProtection.json)입니다.

**다음 실행:** 청취 페이지에서 제보6건과 ASR불일치7개 우선 확인 → 승인된 ID별 candidate SHA 확정 → 같은 전용 브랜치에서 승인 후보만 별도 새 음성 Asset/Catalog 연결(기존 원본 보존) → 변경된 참조·PCM·짧은 Runtime delta 재검증 → 별도 승인 후 Push/PR 및 원본 적용 여부 결정. 재제작이 필요하면 해당 ID/원문/실패 근거만 TTS 담당에게 인계하고 Codex가 API를 호출하지 않습니다. 미승인 후보를 정식 연결하거나 main에 병합하지 마세요.
