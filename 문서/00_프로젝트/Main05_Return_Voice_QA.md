# Main05 귀환 보고 Dialogue/Voice 감사 및 수정

## 2026-10-07 구현 전 계획

사용자 실제 플레이에서 GuardReport 태온001 이후 흐름이 어긋나고, Main05 미엘 위치에서 Main04의 “마을에도 이 상황을 알려야 해요.”가 발화했다. 이 청취 증거는 기술 Mapping PASS로 무효화하지 않는다. Main04 마지막 AfterBattle→Main05 GuardReport6페이지→독립 주민대표 상호작용 RepresentativeReport8페이지→정식동료해금→Main06 첫조사001을 범위로 한다.

현재 LOCAL 코드의 두 보고 배열/정본 부록/Stable ID는 사용자 기대와 일치한다. Resolver는 ID+Speaker를 비교하며 Speaker별 순번/Resources 배열 순서를 사용하지 않는다. Main04 late5 commit은 WAV5만 바꿨고 Catalog/GUID/Text 변경이 없다. Main05 전체6Voice의 연결과 원본 content 판정은 별도로 감사한다. Hash/길이/PCM 일치만으로 의미 PASS하지 않는다.

Unity는 실제 사용자 Field02/Main06 첫조사 Play Mode였다. 읽기 조회로 실제001 Clip 확인 후 사용자에게 Play 종료 허락을 받아 종료했다. 이후 비포커스 격리Save/Settings Launch를 사용하며 사용자 저장과 정상 Main04 late5·Main06 이후·Main17 Voice를 보호한다.

1. 파일/사용자상태 baseline, Main05 Matrix6행 및 전체페이지순서를 작성한다.
2. 실제 Actor/NPC 경로의 연속 QA로 objective 완료시점·중복/누락·clip reference/PCM·portrait·scene cleanup을 검사한다.
3. 가상 Input System Keyboard/Gamepad만 생성해 Enter/Space/A의 UI Submit와 상호작용 중복 전달을 재현한다. OS 입력/포커스 조작은 하지 않는다. 공용수정은 재현된 원인에만 한정한다.
4. WAV 자체 의미 문제는 개별 확정증거와 구간의심을 구분한다. 임의Clip교환/TTS생성 없이 정확한 본문의 재생성 목록을 남긴다.
5. 수정후 동일 영향범위 QA·Compile/Console/Missing Script 확인, CURRENT_STATUS 실제결과만 갱신, 관련파일만 commit·Push0.

Main05 Matrix의 연결 PASS와 Semantic NOT_VERIFIED/SUSPECT를 분리하며, Guard/대표의 ID없는 기존 무음 정책은 MISSING으로 오인하지 않는다. 사용자 현재 청취 증거에는 문제의 미엘 Stable ID가 특정되지 않았으므로 개별 Wrong 판정을 추측하지 않는다.

## 수정 전 Runtime 재현

가상 InputSettings 복제본의 IgnoreFocus 설정에서 OS 입력 없이 재현했다. GuardReport page0에서 동일프레임 Enter+Space→page2(한 페이지 추가 생략), 단일 마지막페이지에서 동일프레임 A 두 번→닫힘 후 GuardReport page0 재열림. 다음프레임 Enter/A는 정상 응답한다. `Temp/Main05Return/runtime/probe-final.txt`에 기록했다. 첫 QA 시도는 비포커스 가상 이벤트가 무시되어 검증 환경 실패였고, 설정 복제/복구로 보완한 위 실행이 실제 재현 근거다.

수정 범위는 DialoguePresenter의 실제 입력용 Advance와 열림/닫힘 프레임 경계, InteractionSystem의 실제 입력 진입점이다. 프로그램용 Advance와 기존 페이지배열/Save/Objective/Clip은 유지한다. 사용자 답변 “기억이 안난다”로 문제의 개별 미엘 자막은 미확정이며, 어느 WAV가 잘못된 문장을 발화하는지 단정하지 않는다.


## 최종 결과

### 재현과 원인 구분

사용자 관찰(Main05 미엘이 Main04 “마을에도 이 상황을 알려야 해요.” 발화)을 실제 결함 증거로 유지한다. 사용자는 후속 질문에서 당시 미엘 자막을 기억하지 못한다고 답했다. 해당 발화 자체를 이번 무음 자동QA로 재청취·재현한 것은 아니며, 어느 개별 Main05 WAV가 틀렸는지 단정할 수 없다.

- **Input bleed:** 수정전 가상 Keyboard Enter+Space 동일프레임 page0→2, Gamepad A 두 번 동일프레임 마지막페이지닫힘→GuardReport재열림 재현. 수정후 page1 및 닫힘유지, 다음프레임 Enter/A 정상. [수정전 로그](Main05_Input_Before_Results.txt).
- **Sequence/Objective:** LOCAL 정상 배열 유지. GuardReport6페이지 완료 뒤에만 대표목표 전환, 대표 상호작용은 별도. 첫 대표3페이지 포함8페이지 모두 유지. 각페이지 Before/After Objective 및 완료경계 [24페이지 Runtime 기록](Main05_Runtime_Page_Order.jsonl).
- **Stable ID/Resolver:** Main056개 ID+Speaker→실제 GUID/Clip 정확. 순번/배열Index resolve 없음. Wrong Speaker reject 정책 유지. Main04late5는 WAV5교체뿐이어서 Catalog 순서 변경/밀림 근거 없음.
- **Reference/cache:** Main05는 Main05 Source6개만 실제재생, 단일VoiceSource·이전Clip정리 및 Scene전환정리. Main06첫001도 해당Scene Source로 시작. Main04 Clip Main05참조/직전Voice 잔존 재현0.
- **WAV content:** Runtime6PCM은 현재 Main05 WAV 전sample오차0이나 의미PASS근거가 아니다. 내용오류 가능성 미해결. 사용자 구간오발화 증거와 개별 파일판정 미확정을 함께 유지한다.

### Main05 정상 페이지 순서

GuardReport(6):

1. 남문 경비병: 돌아오셨군요. / 두 분은…?
2. Player: 초원에서 만났습니다. / 함께 상황을 조사했습니다.
3. 태온 main05_taeon_supp_001: 몬스터들이 마을 쪽으로 움직이는 건 맞습니다. / 하지만 마을을 노리고 있는 것 같지는 않습니다.
4. 미엘 main05_miel_supp_001: 무언가를 피해서 내려오고 있는 것 같아요.
5. 남문 경비병: 피해서… 말입니까?
6. Player: 아직 원인은 모릅니다.

보고 종료→report_to_village_representative→독립 주민대표 Interaction→RepresentativeReport(8):

1. 주민 대표: 그렇다면 단순히 몬스터를 쫓아내는 것만으로 / 해결될 일은 아니겠군요.
2. 주민 대표: 초원 너머 숲에서도 / 비슷한 일이 있다는 이야기가 있었습니다.
3. 주민 대표: 세 분 덕분에 적어도 / 어디부터 살펴봐야 할지는 알게 됐습니다.
4. 태온 main05_taeon_supp_002: 저도 이 움직임이 / 어디서 시작됐는지 확인하고 싶습니다.
5. 태온 main05_taeon_supp_003: 괜찮으시다면… / 조금 더 함께 가겠습니다.
6. 미엘 main05_miel_supp_002: 이대로 두면 / 다치는 사람이 더 생길 겁니다.
7. 미엘 main05_miel_supp_003: 저도 같이 가겠습니다. / 제가 할 수 있는 일이 있을 거예요.
8. Player: 그럼 앞으로도 잘 부탁드립니다.

이후 Main05완료·태온/미엘 정식해금→마을Main06 Available→Field01 Main06시작→Field02 inspect_anomaly_trace→첫001 “초원에서 봤던 움직임과 비슷합니다.”. Main06 이후본문·Asset·코드 수정0.

### Voice Matrix와 TTS

[Matrix6행](Main05_Voice_Matrix.csv)에 StableID/Speaker/표시본문/예상Voice본문/Asset/ResolverKey/실제Resolve/길이·SHA·PCM·판정을 분리기록했다. 각Clip은 `Assets/_Project/Audio/Voice/Story/Main05/<StableID>.wav`이며 아래 정상본문과 동일이름의 Clip이 정확히 Resolve된다. 파일명/Hash만으로 의미PASS하지 않았다.

**연결/재생6PASS, 개별 의미NOT_VERIFIED6.** 시작기준 태온001은 별도anchor이며, 후속태온002/003·미엘001/002/003을 SEMANTIC_INTEGRITY_SUSPECT/TTS_REGEN_CANDIDATE5로 기록했다. **확정 TTS_REGEN_REQUIRED0은 음성이 정상이라는 뜻이 아니다.** 사용자가 들은 오발화는 구간에서 확인됐지만 개별미엘ID가 미확정이기 때문에 임의파일을 Wrong로 지정하지 않았다. [후보5의 StableID/Speaker/ExactText](Main05_TTS_Regen_Candidates.csv). 개별원본확인 후 필요한ID만 정식 재생성한다. 임의TTS 생성0, 다른Clip 대체0, WAV/VoiceReference 변경0.

### 수정 및 검증

게임코드2: DialoguePresenter.AdvanceFromInput 및 열림/닫힘 프레임경계, InteractionSystem 실제입력 호출·닫힌프레임 새상호작용차단. 시간지연/읽기시간제한 없이 다음프레임 입력 허용. 자동QA 등 프로그램용Advance는 기존대로 유지한다. 최종 Runtime debug spam0; Editor전용 QA helper만 로그를 쓴다.

**221PASS / FAIL0**, 기술자동 NOT_VERIFIED0, 별도 의미NOT_VERIFIED6 및 실물Keyboard/Gamepad2 미검증. 실제Actor/NpcRole→대화Next→Quest완료/합류→Main06첫조사 경로. Main04전투후 직전진행도만 격리설정했으며 전투를 재실행하거나 강제승리하지 않았다. Scene전환은 실제SceneManager.LoadSceneAsync를 사용했고 실제출구물리접촉은 검증하지 않았다. Main06 첫페이지는 표시/Source재생/Clip정합 경계검증만 하고 뒤3페이지는 변경·진행하지 않았다.

최종Compile완료/Error0, 기존CS0618 Warning16(이번파일Warning0). 최종Runtime Console Error0/Warning0/Assert0, Field02 Runtime MissingScript0, 정본 World_StarterVillage/Field01/Field02 Scene 각각 MissingScript0. 종료clean Bootstrap EditMode/is_focused=false, 격리Save/Settings 해제, InputSettings native정상/원래 background/editorInput값복원. 최초가상이벤트가 비포커스로 무시된 환경실패, InputSettings복제/교체가 package의 임시기본설정객체를 제거한 QA환경실패·assert 이력은 최종PASS와 분리한다. 프로젝트InputSettingsAsset 저장0; 현재설정값 임시변경·복구 및 기본임시객체 정상복원 후 최종연속QA를 다시 실행했다. [최종로그](Main05_Runtime_Results.txt) · [검증JSON](Main05_Runtime_Verification.json).

### 보호와 Git

baseline3135 중 변경은 게임C#2, **3133 byte보호**. Main04late5 및 전체Voice/원본WAV/meta/GUID/Catalog/Registry/PNG/Sprite/Main06이후/Main17Voice/사용자Save/Settings/Scene/ProjectSettings/Packages 변경0. NEEDS_LISTENING244 역사marker와 Queue219 유지. Matrix는 후속Main05후보5행만 새의심증거를 추가했고 Main04최신PASS상태는 유지했다. Main17 TTS_PENDING14 작업과 분리.

수정commit `f2d7e0c`. QA/문서별도commit은 최신local log. 관련파일만 stage, 작업범위 git diff --check PASS, 기존unrelated 상태 유지, Push0. 전체WorkingTree의 기존 unrelated trailing whitespace는 수정하지 않으며 전체diff검사PASS를 주장하지 않는다.
