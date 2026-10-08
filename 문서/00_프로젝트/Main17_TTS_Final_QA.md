# Main17 승인 TTS14 정식 Unity 반영

## 2026-10-08 작업 계약 — 구현 전

사용자가 신규14개를 직접 청취하고 모두 정상이라고 최종 승인했다. 의미 상태는 **USER_LISTENED_PASS14/14**이며 자동 ASR·메타데이터 판정으로 대체하지 않는다. 제작 Review CSV의 과거 PENDING은 이번 사용자 승인이 대체한다. 원본 Review/WAV는 수정하지 않는다.

시작 HEAD `837271fc2519dec925105bbf326d262294b95d55`는 사용자 Push 기준837271f와 동일하다. 기존 미커밋 사용자 Sprite·Animation·패키지·설정·Save를 보존한다. Unity6000.5.7f1 clean Bootstrap Edit Mode·비포커스를 확인했다.

정식 Main17 Manifest37페이지와 실제 Main17DialogueCatalog의 다섯 길/조건부 동료 원문을 대조한다. VoiceExpected=true14개만 승인 원본의 파일명·SHA256·PCM·길이·샘플레이트·채널·비트·Speaker·Voice ID를 검사하고, 모두 일치한 뒤 `Assets/_Project/Audio/Voice/Story/Main17/`에 byte 그대로 복사한다. Unity가 새 meta/GUID를 생성하며 기존236 Story Catalog 항목을 순서와 참조 그대로 보존하고14개를 추가한다.

본문·StableID·VoiceID·Quest·Party·Formation·기존 Voice/BGM 로직을 수정하지 않는다. Player/지문은 무음, Main18 Manifest/TTS_PENDING22 보호. TTS API/재생성/Trim/Normalize/Pitch/Speed/Re-encode/WAV 편집0.

검증은 기존 격리 저장/설정·비포커스 실행기를 사용한다. 실제 Default/Hearing 조사·조건부 Paul/Miel/Taeon·단일 Source/Next/자연 종료 후 페이지 유지·Scene/Battle 정리·Continue4지점·Voice Mixer 음량/Mute를 확인한다. 사용자 청취 승인, Unity 적용, Runtime 기술 검증의 증거를 따로 기록한다. GitHub Push0.


## 최종 결과 — 2026-10-08

**사용자 청취 승인14 / Unity 적용14 / 실제 Runtime 재생14 / 최종 TTS_PENDING0**. 기술 검사 고유 **646PASS / FAIL0**이며, 250 Catalog 항목 확인과 실제 페이지 검증을 포함한다. 사람의 청취 승인은 자동 테스트와 별개로 유지했고 이번14개를 다시 USER_LISTENING_REQUIRED로 되돌리지 않았다. Main18 TTS_PENDING22는 그대로다.

| 대상 | 화자 / 승인 Voice ID | 원본 길이 | 사용자 청취 / Unity 적용 / Runtime |
|---|---|---:|---|
| main17_after_default_serin_01 | companion_serin / Schedar | 4.560s | PASS / PASS / PASS |
| main17_after_hearing_serin_01 | companion_serin / Schedar | 5.800s | PASS / PASS / PASS |
| main17_compare_paul_01 | companion_paul / Achird | 5.440s | PASS / PASS / PASS |
| main17_compare_serin_01 | companion_serin / Schedar | 11.360s | PASS / PASS / PASS |
| main17_resonance_default_serin_01 | companion_serin / Schedar | 7.000s | PASS / PASS / PASS |
| main17_resonance_hearing_serin_01 | companion_serin / Schedar | 6.800s | PASS / PASS / PASS |
| main17_route_serin_01 | companion_serin / Schedar | 7.280s | PASS / PASS / PASS |
| main17_serin_serin_01 | companion_serin / Schedar | 9.000s | PASS / PASS / PASS |
| main17_tracks_miel_01 | companion_miel / Sulafat | 2.720s | PASS / PASS / PASS |
| main17_vibration_default_serin_01 | companion_serin / Schedar | 7.480s | PASS / PASS / PASS |
| main17_vibration_hearing_serin_01 | companion_serin / Schedar | 7.680s | PASS / PASS / PASS |
| main17_withdraw_serin_01 | companion_serin / Schedar | 5.160s | PASS / PASS / PASS |
| main17_withdraw_taeon_01 | companion_taeon / Gacrux | 5.800s | PASS / PASS / PASS |
| main17_witness_serin_01 | companion_serin / Schedar | 5.120s | PASS / PASS / PASS |

### Source·Import·Catalog

WAV14/14 존재·파일명·decode·SHA256·duration·24kHz·mono·16bit PCM·Review 본문/화자/Voice ID 일치. Manifest37페이지 전체와 실제 Main17DialogueCatalog가 일치했다. 원본 Review CSV의 과거PENDING 및 WAV는 byte 그대로 보호한다. [Source 감사](Main17_TTS_Source_Audit.json)와 [적용표](Main17_TTS_Applied_Matrix.csv)에 USER_LISTENED_PASS→UNITY_APPLIED→RUNTIME_VERIFIED 세 단계 증거를 분리했다. Manifest의 기존 단일 Status에는 최종RUNTIME_VERIFIED를 기록했다.

등록 경로 `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/`. 파일 복사 뒤 SHA14/14, WAV PCM14/14, Unity AudioClip.GetData native float 전 sample14/14 **최대 오차0**. [PCM 결과](Main17_TTS_PCM_QA.json). 파일 re-encode/Trim/Normalize/속도·Pitch 변경0. Import는 기존 Main16 PCM/PreserveSampleRate/DecompressOnLoad/forceToMono=false/normalize=false 계약을 따른다. Unity가 만든 신규 GUID14개는 기존 Voice GUID와 중복0.

Story Catalog **236→250**, 기존236 prefix의 순서·ID·화자·Clip 경로 동일, 기존 WAV/meta/GUID byte 동일. 추가14는 정확한 stable Character ID를 사용한다. MissingAudio·MissingEntry·DuplicateDialogue/CatalogID·NullClip·Speaker/TextMismatch·WrongCharacter·VoiceExpected=false 연결 모두0. 기존 Runtime 본문·대화 순서·Quest Flow·Voice 서비스 수정0.

최초 Import 자동화에서 Unity6000.5의 Normalize 직렬화 이름을 잘못 조회했으나 실제 `m_Normalize`를 확인하고 연결 전에 수정했다. 원본 파일 변경이나 실패 Catalog 연결은 없었다. Unity ProjectAuditor가 Import 중 덧붙인 숨김 설정 객체는 시작 전 해시와 일치하는 부분만 검증해 복구했고 최종 설정 변경0이다.

### 실제 Story 경로

Default(Vision)와 Hearing을 기존 Main17Site.TryInteract→DialoguePresenter→VoicePlaybackSource를 통해 처음부터12목표 완료까지 실행했다. 각 페이지의 자막·화자·stable ID·AudioClip을 대조하고 Player/지문 무음·단일Source·Source volume1/pitch1/non-loop/Voice Mixer routing을 확인했다. 단순 Catalog Resolve만으로 완료 판정하지 않았다.

Default의 Serin8개·Hearing 고유3개·조건부 Paul/Miel/Taeon3개를 포함한 **14개 모두 실제 페이지 재생 및 자연 종료 후 같은 page·objective 유지**. 긴 compare/serin과 짧은 tracks/witness도 실제 끝까지 기다렸다. 명시 Next로만 다음 페이지가 진행된다. Hearing Player 우선/Default Serin 우선과 서로 다른 Clip 대응 PASS. 실제 활성 Party에서 조건부3종 발화, 각각 미편성 시 해당 페이지 없음 PASS. 대화 Flow가 편성·Formation을 강제 변경하지 않았다. 검증 Fixture의 편성은 격리 저장에서만 구성했다.

두 경로 모두 목격 Voice→실제 Battle→기본 공격 버튼·실제 대상 선택으로 정상 승리→필드 복귀→정확한 after Voice→마지막 철수 대화→Main17완료 PASS. Field Beneath The Cracked Earth / Battle Blade and Gambit의 실제 Clip 참조를 확인했으며 BGM Asset/로직은 변경하지 않았다.

Next·Dialogue Hide·완료·Battle 진입/승리/복귀·Scene 전환·Bootstrap Continue 잔류0, Voice 두 Clip 동시 재생0. 보충 검사에서는 **실제 재생 중** Next→Player, Field08→Field07, Bootstrap Continue, Battle 진입·도망 복귀를 확인했다. 전체 Story 승리와 보충 중단 검사는 별도 로그로 구분한다.

### Save·음량·백그라운드

실제 Save→Bootstrap Slot01 Action→복귀 **5지점**: A음성 전, B중간목표, CStory승리후, DMain17완료후, E음성재생중 중단. Quest count/Completed ID·Party·Formation·CompanionUnlock·Beast·HP/MP·Scene·Position 일치, SaveVersion1·Voice playback state 저장0·Continue 자동 발화0. 완료 후 기존 Main18 자동 시작 기록도 그대로 복원한다.

대표 Serin Clip에 기존 Voice Mixer100/40/0의 실제 노출 dB, Voice0 inaudible, MasterMute -80dB·채널40 유지, unmute/100 복원과 SFX/BGM 설정 불변을 확인했다. Source routing과 Mixer gain의 기술 검증이며 OS 출력 에너지·음색 취향을 새 청취 판정으로 주장하지 않는다. 자동 대사가 사용자 스피커에 반복 출력되지 않도록 AudioListener.volume만 검사 세션에서0으로 두고 끝에 원래 값을 복원했다. Voice 전용 볼륨 로직 추가0.

기존 비포커스 실행기의 Save/Settings 격리 사용. Foreground·GameView 활성화·OS 입력·ComputerUse0. 종료 clean Bootstrap EditMode·is_focused=false·AuditSaveDirectory=null, 격리설정과 Editor Play 옵션 복구.

### 보호와 최종 진단

baseline **3253파일 중 3252 byte 동일**, 기존 변경은 의도한 StoryVoiceCatalog1개만, 예상 밖 변경/삭제0. [보호 감사](Main17_TTS_Protection_QA.json). Opening18, Main03~16 WAV/meta/GUID와 최근 Main05태온001·Main07Paul/Miel복구, BGM 원본, 사용자 Sprite·Portrait·Animation·Save·패키지/설정, 승인Source14/Review 보호. Main17 Flow/본문/ID·Main18 Asset/코드/Manifest 변경0. Main18 생성/적용/Catalog추가0, TTS API0.

CompileError0·최종 RuntimeConsoleError/Warning0·신규Warning0·MissingScript0. 기존 CS0618 컴파일Warning16는 기존 코드의 이력이며 수정하지 않았다. [통합 Runtime](Main17_TTS_Runtime_Results.txt) · [재생 중 중단](Main17_TTS_Boundary_Results.txt) · [집계](Main17_TTS_QA_Summary.json).

이번 승인 음성의 추가 사용자 청취 요청0. 기존 Main17 음악 청취·실물 입력과 Main18의 별도 미확인 항목은 이번 TTS 적용 범위 밖이다. 다음 권장: 실제 게임에서 원하는 후속 확인 또는 별도 승인 Main18 음성 제작. 자동으로 Main18 작업을 시작하지 않는다.

### Git·변경 목록

시작 HEAD `837271fc2519dec925105bbf326d262294b95d55` = 사용자 Push 기준837271f. LOCAL을 정본으로 사용했고 fetch/push0. 기존 unrelated 미커밋 변경 보존, Source 입력 폴더는 stage하지 않았다. 구현 commit `f0f44b3` (`Feature: Main17 승인 TTS 14개 정식 적용`), QA는 이 문서를 포함한 최신 `Docs: Main17 TTS 최종 QA 및 상태 갱신` commit이다. CURRENT_STATUS/MAIN17_FINAL_QA/Checkpoint의 최신 항목이 이전 TTS_PENDING14 이력을 대체한다.

변경 파일:

- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_after_default_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_after_default_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_after_hearing_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_after_hearing_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_compare_paul_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_compare_paul_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_compare_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_compare_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_resonance_default_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_resonance_default_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_resonance_hearing_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_resonance_hearing_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_route_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_route_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_serin_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_serin_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_tracks_miel_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_tracks_miel_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_vibration_default_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_vibration_default_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_vibration_hearing_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_vibration_hearing_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_withdraw_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_withdraw_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_withdraw_taeon_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_withdraw_taeon_01.wav.meta`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_witness_serin_01.wav`
- `Unity/Client/Assets/_Project/Audio/Voice/Story/Main17/main17_witness_serin_01.wav.meta`
- `Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main17TtsRuntimeAudit.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/Main17TtsRuntimeAudit.cs.meta`
- `문서/00_프로젝트/CURRENT_STATUS.md`
- `문서/00_프로젝트/MAIN17_FINAL_QA.md`
- `문서/00_프로젝트/MAIN17_RESUME_CHECKPOINT.md`
- `문서/00_프로젝트/Main17_TTS_Applied_Matrix.csv`
- `문서/00_프로젝트/Main17_TTS_Boundary_Results.txt`
- `문서/00_프로젝트/Main17_TTS_Final_QA.md`
- `문서/00_프로젝트/Main17_TTS_Manifest.csv`
- `문서/00_프로젝트/Main17_TTS_PCM_QA.json`
- `문서/00_프로젝트/Main17_TTS_Protection_QA.json`
- `문서/00_프로젝트/Main17_TTS_QA_Summary.json`
- `문서/00_프로젝트/Main17_TTS_Runtime_Results.txt`
- `문서/00_프로젝트/Main17_TTS_Source_Audit.json`

GitHub Push 하지 않음
