# Main07 폴 작별 Voice 누락 / TTS Handoff

## 구현 전 계약 — 2026-10-08

사용자 Play에서 작별 첫 문장부터 Main08 태온 “잠깐만요.” 직전까지 무음. 로컬 Main07 마지막 fallback 6페이지 중 폴5페이지에는 ID가 없으며 Player1페이지는 의도된 무음이다. Main08 첫 페이지는 main08_taeon_supp_001 및 기존 Catalog 연결로 사용자 관찰과 일치한다.

신규 main07_paul_supp_001~005만 추가하고 본문·화자·순서·입력·Quest·Save를 유지한다. companion_paul/폴/Achird, TTS_REQUIRED5. WAV 미제작/Catalog 미적용이며 음성 문제 CLOSED가 아니다. 기존 Paul001~020/Early7/Miel3/Taeon/Main08/전체 Voice·meta·Catalog를 보호한다. TTS API0/Push0. 실제40페이지 감사, 작별 text fallback/명시 Next/같은 프레임 입력/Main08 경계/전후 Continue를 비포커스 격리 실행한다.

## 최종 결과 — 2026-10-08

**Stable ID 수정·전달 완료, 실제 음성 문제 OPEN / TTS_REQUIRED5.** WAV 미제작·Catalog 미적용·USER_LISTENED_PASS 없음. TTS 담당 별도 세션에서 정확한 본문과 Achird로 제작→사용자 청취 승인→Unity 적용→별도 Runtime 확인 후 CLOSED한다. 이번 API0/음성 가공0/가짜 또는 null Catalog entry0.

시작 HEAD `ae4c10ddbebac92633d7d3a9dc04f4c4f48062d3`, 당시 origin/main과 동일(사용자 Push ae4c10d). 구현 commit `4a9e4c7`; QA는 이 문서를 포함하는 최신 Docs commit. LOCAL 기준 사용, fetch/push0.

### 원인·전수 감사

MainQuest07Interactable.GetLines 마지막 fallback 6페이지의 폴5줄은 stable ID가 비어 있었다. VoiceClipCatalog.Find(empty)는 null이므로 Presenter가 text-only로 진행한 것이며 Main08 첫 태온 ID는 정상 연결되어 있다. 사용자 실제 증상과 코드 원인이 일치한다. 수정 전 Play를 별도로 재실행하지 않았으며 수정 후 실제 페이지/Clip 상태는 검증했다.

[40페이지 전수표](Main07_Paul_Farewell_Dialogue_Audit.csv): 전체40 / Character38 / Player2 / 지문0. 기존 Character33 resolve 유지, 신규5는 null resolve, 추가 missing ID0. Paul001~020의 번호·본문·화자·순서 그대로다. Player “혼자 가시려고요?”에는 Voice ID를 만들지 않았다.

| 순서 | ID | 정확한 본문 |
|---:|---|---|
|1|main07_paul_supp_001|아무래도 저희가 보고 있는 게 같은 현상 같기는 하네요.|
|2|main07_paul_supp_002|그런데 저는 확인해볼 곳이 하나 더 있습니다.|
|3|없음 / Player 무음|혼자 가시려고요?|
|4|main07_paul_supp_003|이번에는 진흙 없는 길로요.|
|5|main07_paul_supp_004|아까 충분히 배웠습니다. 헤헤.|
|6|main07_paul_supp_005|다시 만나게 되면 그때 정보부터 맞춰보죠.|

폴5개 공통 SpeakerId=companion_paul / 표시명=폴 / VoiceId=Achird / VoiceExpected=true / Status=TTS_REQUIRED. [정본 Handoff5](Main07_Paul_Farewell_TTS_Handoff.csv), Tools/TTS/main07_paul_farewell_tts_manifest.csv는 기존 제작 스키마를 따른 동일5행 전달용이며 API 실행기는 추가하지 않았다.

### 실제 Runtime·Continue

[최종 로그](Main07_Paul_Farewell_Runtime_Results.txt) **73PASS / FAIL0**. 격리된 작별 직전 Quest fixture에서 Field02 실제 폴 상호작용 E→6페이지 본문/화자/ID/순서·Player 무음·폴 null Clip fallback→각 페이지 무입력 유지→명시적 Next 1회=1페이지→같은프레임 E/A/E 중복진행·재열림0→실제 완료 callback/Main07완료→자동 NPC 연결0.

Main08는 기존 SceneLoaded에서 시작하므로 Main07 완료 직후 Available, Field02 재진입 뒤 Active다. 이 정상 경계를 그대로 실행했다. Main08 Trace01 실제 E 상호작용→태온 “잠깐만요.” / main08_taeon_supp_001 / 기존 Catalog Clip 실제 재생→자연 종료 후 page0유지·자동진행0→Hide잔류0. 실제 사람 청취 판정은 추가하지 않았다. 전투 재실행 대신 작별 직전 fixture를 사용했고 기존 재도전/성장/Voice QA 이력은 유지한다.

실제 Save→Bootstrap Slot01 이어하기→Field02 복귀 2회(작별 직전, 완료 후 Main08 정상 재진입 상태). Quest/Completed·Party·Formation·Beast·HP/MP·Scene/위치 보존, 자동 Dialogue/Voice0. 완료 callback 자동 저장도 실제 실행됐다. SaveVersion1·새 Save field0·입력/Quest 코드 변경0.

초기 QA는 Main08 Available→Active 정상 시작을 동일 Quest snapshot으로 비교하여 완료 후 Continue에서 fixture FAIL했다. [초기 기록](Main07_Paul_Farewell_Initial_Fixture_Results.txt)을 분리 보존하고 기존 Scene 시작 경계를 통과한 상태로 재실행해 최종 PASS했다. 게임 Main08/Save를 수정하지 않았다. 새 QA 코드의 최초 컴파일에서 Monster namespace 누락1건을 수정한 뒤 컴파일 오류0, 기존 CS0618 경고16/신규경고0, 최종 Runtime Console Error/Warning0이다. 최초 scripts-only refresh가 신규 파일을 가져오지 않아 all refresh 후 컴파일했다.

### 보호·종료

기존 baseline 3263파일 중3262 byte동일, 변경은 Main07 Flow1개(폴 ID5 및 관련 한국어 주석), 삭제0. 전체 기존 WAV/meta/GUID/Catalog·Main08·Main17·Main18·사용자 Save/Sprite/설정 byte보호. [검증 JSON](Main07_Paul_Farewell_Verification.json). Field02/Bootstrap MissingScript0. 종료 clean Bootstrap EditMode·is_focused=false·격리 Save 해제·Listener 원값1·Input 설정/가상 장치 정리. Foreground/GameView 강제 활성화/OS 입력0.

### 변경 파일

- Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs
- Unity/Client/Assets/_Project/Scripts/Editor/Main07FarewellAudit.cs 및 .meta
- Tools/TTS/main07_paul_farewell_tts_manifest.csv
- 문서/00_프로젝트/Main07_Paul_Farewell_TTS_Handoff.csv
- 문서/00_프로젝트/Main07_Paul_Farewell_Dialogue_Audit.csv
- 문서/00_프로젝트/Main07_Paul_Farewell_Runtime_Results.txt
- 문서/00_프로젝트/Main07_Paul_Farewell_Initial_Fixture_Results.txt
- 문서/00_프로젝트/Main07_Paul_Farewell_Verification.json
- 문서/00_프로젝트/Main07_Paul_Farewell_QA.md
- 문서/00_프로젝트/CURRENT_STATUS.md

GitHub Push 하지 않음
