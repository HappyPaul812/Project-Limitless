# Main10 조사 입력 차단 수정 및 음성18 감사·청취 후보 (2026-10-19)

## 상태와 기준

입력 수정은 격리 Runtime PASS, 음성은 **청취 승인 대기·Catalog 미적용**입니다. 전용 브랜치 `codex/main10-voice-quest-interaction-fix-20261019`, 기능 커밋 `5c36a6531f23550c535737a96e1cd18648229f6b`. Main09 승인27 보호를 위해 기존 최종 `424b19122221d1555fef14d46c459fa174ac07a2`에서 분기했습니다. 해당 Main09 커밋은 아직 main에 없으므로 추후 PR 범위는 선행 Main09 통합 상태에 맞춰야 합니다. Push·PR·main 병합·원본 적용은 하지 않았습니다.

정본: GitHub `docs/planning` `ea3281c483b3d530cb1ebde1f79d87e851add15c`의 `Main10_Field03_Voice_Mapping_And_Quest_NPC_Blocker_20261019.md`. 일정 지시 준수: 17:17:17 한도 재설정(사용0%) 확인 후 구현·QA 시작. main/origin/main/원본 HEAD `675988c9f2c5c507d9a696b0378be27c0bd3309b`.

## P1 원인과 최소 수정

원본 슬롯02 사본의 실제 위치는 돌 조사 반경1.6과 치유사 NPC 반경2 양쪽에 들어갑니다. 기존 InteractionSystem에는 Main09 우선 처리만 있고 Main10 현재 조사 대상 처리가 없어 치유사 안내·대화가 먼저 입력을 소비했습니다. 실제 E에서도 치유 확인창이 열렸고 NPC 콜백을 먼저 실행한 경우 동일하게 재현했습니다.

`InteractionSystem.cs`와 `MainQuest10FieldFlow.cs` 두 파일만 변경했습니다. 현재 Main10 목표가 실제 조사 반경 안에 있을 때만 NPC보다 먼저 처리하고 NPC 대화 프롬프트를 숨깁니다. 반경 밖에서는 치유사를 다시 사용할 수 있습니다. 공용 입력 프레임 보호를 Main10에도 적용하여 입력 콜백 순서/E+F 중복에 의한 페이지 건너뛰기·재열기를 방지했습니다. Quest ID/7목표/대사/보상40·40/던전/Scene/Collider/위치/Spawn/이동경로는 그대로입니다.

## 변경분 Runtime QA

Unity6000.6.5f1, 기존 격리 Client Batch Mode, 원본 Save의 사본만 사용. 강제 순간이동·Quest 강제 단계 변경 없이 실제 물리 이동·가상 키보드/게임패드 입력으로 검증했습니다. 기존 대규모427/550 QA 반복 없음.

- Before 88 PASS / 0 FAIL: 사용자 상태 재현, 18 NPC+6 무음 Player 페이지, 실제 Catalog GUID/AudioSource Play clock/Next/Stop·참조 해제. 이 PASS는 **오발화 재현·측정 성공**이며 기존 음성 정상 판정이 아닙니다.
- After 132 PASS / 0 FAIL: E/F/A 조사 우선, 콜백 순서/동시 E+F/취소 무진행, 치유사 반경 밖 정상 치료·저장, Main10 step1~6 순차 진행, Main09 완료 Save에서 step0 연결, EXP40/탈렌트40 1회, 폴 roster 보존, 단계1·4·완료·초기 연결 Continue, 던전 실제 A 진입·기존 출구 복귀, Main09 승인 인사 참조 유지.
- 1280×720(16:9), 960×720(4:3), 1680×720(21:9) 실제 Unity 화면 확보. 휠체어 사용자 Save와 키보드·패드 물리 입력 검증. 별도 도보 전구간 재검증은 하지 않았습니다.
- 신규 Runtime 오류0. 기존 UnityEditor.Search IndexOutOfRange/URP Host type mismatch는 별도 기준선이며 해결 범위에 넣지 않았습니다.
- 자동 경로의 막힌 도착점과 Batch GameView 비동기 해상도 때문에 초기 harness 실행은 중단됐습니다. Scene/Collider를 바꾸지 않고 도달 가능한 경로·해상도 대기를 수정한 최종 실행 수치만 위에 기재했습니다. 치유사는 키보드 E 확인·치료를 최종 검증했으며 패드 NPC 확인창의 동시 Submit 동작은 미확정입니다.

## 음성18 전수 감사와 복구

18개 원문/step/page/화자/DialogueId/Catalog GUID/meta GUID/WAV·meta·PCM SHA·발화 구간을 [CSV](Evidence/Main10_VoiceQuest_20261019/Main10_Audit18.csv)에 기록했습니다. GUID 참조18/18은 올바른 동명 파일을 가리키지만 **파일 내부 발화가 자막에 맞지 않습니다**. 실제 Runtime도 해당 GUID와 동일 Clip을 재생했으므로 단순 번호 relink 또는 AudioSource 이전 Clip 잔류로 단정할 수 없습니다. 기존 제작 원음의 문장 묶음·분할 경계가 등록 ID 경계와 어긋난 것이 확인된 원인입니다. Next는 수동으로 이전 Clip을 Stop하고 새 Clip을 재생합니다.

현재 파일 중 `taeon004`는 T002~004 세 문장, `taeon006`은 T005~006 두 문장, `miel002`는 Main09 마지막 문장+Main10 M001, `paul003`은 P006~007을 포함합니다. `taeon005`, `paul004`, `paul006`은 대응하는 완전한 문장이 없거나 추임새뿐입니다. 나머지도 다른 ID/챕터 문장이며 표에 실제 전사를 남겼습니다.

기존 원음에서 18 후보를 제작했습니다: byte동일 복사10, PCM 무손실 무음 분리8. 무음 분할은 4개 소스의 5경계에서 최소80ms 저레벨 구간·중앙0샘플·±10ms peak≤-45dBFS를 확인했습니다. 게인/페이드/리샘플/합성/신규TTS/API0. Main09/10 원음과 `main11_miel_001`의 실제 발화를 대조해 찾았으며 기계적 번호 이동은 하지 않았습니다. 현재 신규 음성 필요 확정0, 최종 여부는 대표님 청취 판단에 따릅니다.

후보 로컬 ASR 자막 정규화 일치11/18, 우선 청취7: main10_taeon_001, main10_paul_001, main10_taeon_002, main10_paul_002, main10_taeon_004, main10_paul_007, main10_taeon_006. ASR은 사람 청취 승인이 아닙니다. 특히 P007 앞 원음의 “음…”을 삭제하지 않고 보존했습니다. **18개 모두 승인 전 StoryVoiceCatalog/Unity Assets 적용 금지**.

청취 HTML: `F:/study/codex/Project-Limitless/Temp/Main10VoiceQuest20261019/Delivery/index.html`
ZIP: `F:/study/codex/Project-Limitless/Temp/Main10VoiceQuest20261019/Main10_Voice18_ListeningCandidates.zip`
ZIP SHA256: `8b916ee6b0d64c026151679ceafe574ae0bed4d9a2cf33d44d6741e6fbec0b70`

## 보호·다음 재개

원본 Assets/UserData/UserSettings/ProjectSettings/Packages 총4235파일 SHA 불일치0, UserData23 별도 백업, 원본 Git status raw byte동일/staged0/HEAD 유지. Main09 승인27·Catalog·기존 WAV·Field05 포함. 원본 Editor/Play/Refresh/포커스 조작0, 사용자 dirty/Save 덮어쓰기0. Main10 코드도 원본에는 아직 적용하지 않았습니다.

다음: 대표님은 청취 HTML의 후보18을 검토합니다. 승인된 후보 SHA만 별도 공식 경로/meta로 연결하고 Main10 Catalog18 변경분·실제 재생·Continue만 재검증합니다. 입력 수정 원본 적용/PR/Push/main 병합은 별도 지시 후 충돌 SHA 확인부터 진행합니다. 이 브랜치를 main에 직접 병합하면 선행 Main09 미병합 변경도 포함되므로 범위를 반드시 검토합니다.
