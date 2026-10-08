# Main07 폴 작별 승인 Voice5 최종 QA

## 구현 전 계약 — 2026-10-08

사용자 직접 청취 USER_LISTENED_PASS5/5. 원본 Review의 PENDING은 제작 당시 이력이고 최신 사용자 승인이 대체한다. Source5의 SHA/decode/PCM/길이/24kHz·mono·16bit/Review/Handoff/제작 Manifest/Runtime 정본문 일치를 먼저 검사한 뒤 바이트 그대로 Unity Main07에 추가한다. 신규 meta/GUID5·Catalog 기존250 prefix 보존+5. WAV 편집/Trim/Normalize/재인코딩/속도·Pitch/API0. Main07 Flow·Main08·기존33Voice·Paul20/Miel3/Taeon3/Early7·Main17/18·사용자Asset/Save 보호.

작별 직전 격리fixture→실제E상호작용→6페이지5Voice/Player무음→모든5Clip 자연종료후page유지→명시Next1page→실제완료→기존Field02재진입→Main08Trace01 “잠깐만요.” 실제Voice 확인. 기존 실행기의 Save/Settings/Input/Listener/화면포커스 보호. Continue전후2회·VoiceMixer100/40/0/MasterMute/복원·전체38resolve·Compile/Console/MissingScript 확인.

## 최종 결과 — 2026-10-08

**이번 작별 누락 CLOSED / TTS_REQUIRED0.** 승인원본5/Unity5/Catalog5/실제Runtime5/전체PCM5 PASS. 기술 검사 **143PASS / FAIL0**. 사용자 직접 청취 USER_LISTENED_PASS5를 유지하고 자동 ASR/메타데이터 판정으로 대체하지 않았다. 기존 Main07 Paul20/Early7/Miel3의 별도 사람청취 대기 이력은 이5개 승인으로 승격하지 않는다.

시작HEAD `ebb867e311c78c80044191f3d7cce67fb1df89ca`, 당시 origin/main 동일(사용자 Push ebb867e). 구현 `4c9a889`, QA는 이문서를 포함한 최신Docs commit. fetch/push0.

| ID | 승인원본 길이 | 단계 |
|---|---:|---|
|main07_paul_supp_001|5.120s|USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED|
|main07_paul_supp_002|3.640s|USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED|
|main07_paul_supp_003|2.400s|USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED|
|main07_paul_supp_004|3.120s|USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED|
|main07_paul_supp_005|3.440s|USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED|

### Source·Import·Catalog·PCM

정식 Source `Limitless_TTS_Output_Main07_Paul_Farewell`. [감사5](Main07_Paul_Farewell_Source_Audit.json): 파일명/Review SHA/전체decode/Duration/24kHz/mono/16bit/Review/Handoff/Tools Manifest/실제Flow 본문·Speaker companion_paul·Voice Achird 전부5/5 일치 후복사. 원본 Review PENDING/제작파일/호출budget checkpoint byte보존; 사용자 최신 직접청취 승인이 대체한다. 원본/API/ASR새실행0.

Unity Main07에 원본byte그대로5추가·Unity 신규meta/GUID5 생성, 기존GUID재사용0. PCM/PreserveSampleRate/DecompressOnLoad/preloadfalse/forceToMono=false/normalize=false, Source volume1/pitch1/non-loop/기존VoiceMixer 확인. WAV파일 편집·Trim·Normalize·Speed/Pitch·재인코딩0. 신규meta의 빈 값줄 끝공백만 정리했으며 GUID/settings 유지.

Catalog250→255. 기존250 prefix ID·화자·GUID·순서 모두 동일. 신규5는 각 대응Clip/companion_paul로 연결. 전체255 null0/duplicate0. 실제Main07 GetLines40페이지 전수: Character38 모두 정확한 ID→같은이름Clip resolve, Player2 무음. Missing/WrongClip/WrongCharacter/Filename mismatch0.

[검증 JSON](Main07_Paul_Farewell_Final_Verification.json): Source↔Unity SHA5/5, WAV 전체PCM SHA5/5, 실제 재생Clip.GetData nativefloat 전sample=원본signed16/32768 **5/5 최대오차0**. [Matrix5](Main07_Paul_Farewell_Final_Matrix.csv)에 sourceSHA/GUID/본문/단계/길이를 기록했다. 음성혼입의 의미는 사용자 승인으로 확인됐고 기술적으로도 승인원본과 재생전체PCM이 동일하다. supp004에 별도supp005 Clip을 자동이어붙이는 동작0.

### 실제 연속 경계

[Runtime 로그](Main07_Paul_Farewell_Final_Runtime_Results.txt)는 page index/화자/본문/ID/Clip/재생 상태·자연종료 기록을 포함한다. 격리fixture에서 작별 직전 목표를 준비하고 실제MainQuest07Interactable E→Presenter 6페이지→완료callback→Main07완료→기존Field02재진입/Main08시작→Trace01 E→태온기존Voice까지 연속실행했다. 전투·초반Main07을 다시완주했다고 주장하지 않는다. 관련 Story/Quest/Input/Save게임코드 변경0.

1. 폴 supp001→supp002→Player “혼자 가시려고요?”→supp003→supp004→supp005 정확한6페이지.
2. **모든5개 자연종료까지 실제대기** 후 같은페이지/같은Clip유지·재시작0·다음Clip자동발화0. 명시E1회=다음page1회, 같은프레임A/E추가입력중복0. 단일VoiceSource 유지.
3. Player Resolve null·Clip null·재생0·이전Paul잔류0·Portrait 비표시 기존정책 유지. 명시Next 정상.
4. supp005 자연종료후page5유지→명시Next→Hide/Source정리→실제Main07완료, 자동NPC대화연결0.
5. 기존SceneLoaded 정책에 따라 Main08는 완료직후Available, Field02재진입 뒤Active. Trace01 태온 “잠깐만요.” / main08_taeon_supp_001 / 기존정확Clip 실제재생. Paul잔류0·자연종료후page0유지·Hide정리 PASS. Main08 코드/WAV/기존Catalog entry변경0.

### Continue·Volume·검증 한계

실제 Save→Bootstrap Slot01 Action→Field02 복귀 **2회**: A작별직전, BMain07완료후기존Main08재진입상태. Quest/Objective/Completed·Scene/Position·Party/Formation/CompanionUnlock·Beast·HP/MP 보존, Source잔류0/자동발화0. 완료callback자동Save도 실행됐다. Voice재생위치Save0·SaveVersion1·새Savefield0.

supp001 실제재생중 Voice100/40/0 노출Mixer dB값(0/-7.9588/-80)·Voice0 inaudible·MasterMute -80/채널40유지·Unmute/100복원·SFX/BGM값불변 PASS. 새AudioSource/Mixer/volume로직0. Mixer gain/state기술검사로 OS출력에너지 측정은 하지 않았다. 사용자스피커반복출력을 피하려검사Listener만0으로두고끝에원값1복구, 사용자청취승인은 그대로유지한다.

Commandline Source/SHA보호검사 후 실행중Editor를 쓰는기존PlayUnfocused 격리검증경로. 창foreground/GameView강제활성화/OS키보드마우스/ComputerUse0. 가상InputSystem E/A검사와 실물입력은 구분한다.

### 보호·진단

baseline3275 중3273 byte동일, 의도한변경 Catalog1·EditorQA1만. 삭제/예상밖변경0. 모든기존Voice/WAV/meta/GUID·Main07Flow·Main08이후·Main17/18·Battle/CompanionGrowth/Retry/Party/Formation/Beast/SaveVersion/BGM/Portrait/Sprite·사용자Save/설정·승인원본 보호.

CompileError0·최종ConsoleError/Warning0·신규Warning0·기존CS0618컴파일Warning16별도·Field02/Bootstrap MissingScript0. 이번QA 최종FAIL0, 이전무음단계초기fixtureFAIL기록은 과거증거로보호. 종료 cleanBootstrap EditMode/is_focused=false/AuditSaveDirectory=null·Input장치/설정·Listener·Play옵션 복구. 추가사용자청취요청0.

### 변경 파일

- Unity/Client/Assets/_Project/Audio/Voice/Story/Main07/main07_paul_supp_001~005.wav 및 각 .meta (10개)
- Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset
- Unity/Client/Assets/_Project/Scripts/Editor/Main07FarewellAudit.cs (제작전/승인후검증모드·새로그·PCM·Volume검사)
- Tools/TTS/main07_paul_farewell_tts_manifest.csv
- 문서/00_프로젝트/CURRENT_STATUS.md
- 문서/00_프로젝트/Main07_Paul_Farewell_QA.md
- 문서/00_프로젝트/Main07_Integrated_Retry_Growth_Voice_QA.md
- 문서/00_프로젝트/Main07_Paul_Farewell_TTS_Handoff.csv
- 문서/00_프로젝트/Main07_Paul_Farewell_Source_Audit.json
- 문서/00_프로젝트/Main07_Paul_Farewell_Final_Verification.json
- 문서/00_프로젝트/Main07_Paul_Farewell_Final_Runtime_Results.txt
- 문서/00_프로젝트/Main07_Paul_Farewell_Final_Matrix.csv
- 문서/00_프로젝트/Main07_Paul_Farewell_Final_QA.md

GitHub Push 하지 않음
