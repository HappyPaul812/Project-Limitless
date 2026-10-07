> 2026-10-07 최신 Main07 미엘기존3 PCM복구/태온3보존·재도전·동료성장은 [종합QA](Main07_Integrated_Retry_Growth_Voice_QA.md)를 참조. 이 문서의 이전 기록과 기존244 청취 목록은 유지한다. Early7/Paul20 원본과 결과는 보호했다.

## 2026-10-07 최신 Main07 상태 — Early7 적용 / Paul20 PCM 복구

Early7 Unity resolve/PCM PASS. Paul은 묶음WAV/단편/번호밀림으로20개 전수감사·기존PCM복구, 재생성필요0/TTS API0. 사용자 복구Paul001/002 청취PASS, 나머지25청취PENDING과기술검증구분. [최신 정본](Main07_Early7_Paul_Recovery_QA.md), [복구Matrix20](Main07_Paul_Recovery_Matrix.csv). 아래TTS_REQUIRED7/Paul001재생성1은해소된과거기록이다.

## 2026-10-07 Main07 초반 Voice / Paul 감사

NPC7 Stable ID 부여 완료, TTS_REQUIRED7/Catalog pending. 사용자 첫 자막 유지 확인 및 Runtime page0 유지/001 PCM 정합으로 Paul001 WAV 내용 오류 TTS_REGEN_REQUIRED1 분리(Voice 교체0). Main07 custom E/A 동일프레임 닫힘→재열림 재현·수정, 기존 공용 fix 유지. [상세 Main07 QA](Main07_Early_Dialogue_Voice_QA.md), [TTS7 handoff](Main07_Early_TTS_Handoff.csv). 기존 Voice/244 NEEDS_LISTENING 보호. 아래는 이전 감사 이력이다.

## 2026-10-07 최신 Main05 상태: CLOSED

사용자 전체 청취 PASS 신규5개를 기존GUID에 적용했다. Main05전체6 resolve/RuntimePCM PASS, Main04→Guard6→독립대표8→정식해금→Main06첫001 연속 및 Save/Continue3회 PASS. 입력 수정 f2d7e0c 유지. 과거 의미 미확정5개는 최신 승인 원본으로 해소했다. 역사적 NEEDS_LISTENING244 보존. 상세 정본은 [Main05 최종 QA](Main05_FinalVoice_QA.md), [검증 결과](Main05_Final_Verification.json), [최종 Matrix6](Main05_Voice_Matrix.csv). 아래는 이전 감사 이력이다.

## 2026-10-07 Main05 귀환 보고 감사 / 입력 결함 수정 / Voice 의미 대기

같은프레임 Enter+Space page0→2 및 A 종료→NPC 재열림을 실제 가상장치 입력으로 재현·수정했다. DialoguePresenter 실제입력용 Advance/열림·닫힘 프레임 경계와 InteractionSystem 입력진입점만 최소수정, 대사배열/StableID/Objective/Catalog/WAV 변경0. Main04전투후9→GuardReport6→독립대표Report8→정식합류→Main06첫조사001 한 Play 연속 **221PASS/FAIL0**, 24페이지 기록·Main05 6PCM 원본오차0·단일Source·조기진행/자동다음NPC0. Programmatic Advance 유지.

**전체 Voice 의미 해결은 아직 아니다.** 사용자 Main05의 Main04문장 오발화 증거 유지. 현재 Mapping6PASS와 개별 WAV 의미는 구분: 의미NOT_VERIFIED6, 후속태온2/미엘3 **재생성후보5**, 개별Wrong/확정TTS_REGEN_REQUIRED0(어느 미엘 자막인지 사용자가 기억하지 못함). 후보를 확정 재생성목록으로 자동승격하지 않았다. TTS생성/Clip교환0·NEEDS_LISTENING244 marker 유지·Main04late5/Main06이후/Main17Voice 변경0. [최종 감사·한계](Main05_Return_Voice_QA.md) · [Voice Matrix6](Main05_Voice_Matrix.csv) · [정확본문 후보5](Main05_TTS_Regen_Candidates.csv). 최신 근거는 이 문서이며 이전 일반metadata PASS는 의미판정이 아니다.

## 2026-10-07 Main04 후반 음성 5개 최종 반영 완료

현재 Main04 Late Sequence의 재생성 대기 **5→0**. 미엘009/010/011·태온006/007 모두 **REGENERATED / USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED**. 사용자 전체 원본 청취PASS와 이번 Runtime 기술 검증을 구분한다. 실제 Actor 9페이지·7Voice 전체재생/Next/Portrait/화자/본문, 새5개 PCM 오차0, 완료→Main05/완료전후 Bootstrap Continue, 동일프레임 Next2/단일Source, Player·지문 무음 PASS. **148PASS/FAIL0**, Compile Error0·최종Console Error0/Warning0·Field Runtime Missing Script0. 원본·meta·GUID·Catalog·대사 변경0; WAV 정확히5만 교체. 기존 NEEDS_LISTENING244 marker/Queue219 및 Main17 TTS_PENDING14 유지·일괄PASS승격0. [최종 QA](Main04_Late5_Final_QA.md)와 [현재5개 Mapping](Main04_Late5_Source_Mapping.csv)이 우선하며 아래 이전 Wrong/Suspect/대기5 표기는 역사 기록이다. 음성commit `0e7809a`; Push0.

## 2026-10-06 전체 Story Voice 의미 위험 사전 감사

[Semantic Risk 사전 감사](StoryVoice_SemanticRisk_Audit.md)를 최우선 현재 요약으로 참조한다. 현재 Source 재추출331/Voice247/Player Silent42. 최신 사용자·Runtime 의미PASS23(태온005 경계PASS 보완)을 보호하고 기존244 관리 범위와 새 청취219·기존 재생성5를 구분했다. HIGH35/MEDIUM129/LOW83은 청취 우선순위로 신규 의미 불일치/TTS필요 확정0. 기존Main045 판정유지. LOW60도 의미미확인이다. [자연 진행 Queue](StoryVoice_ListeningQueue.csv)와 [위험 구간 우선 Queue](StoryVoice_PriorityListeningQueue.csv) 제공, 추천 Main09→Main11.

Editor529/합성10/독립247WAV/과거PASS23SHA 검증PASS, Compile Error0/Console Error0. 기존2260파일/WAV/Catalog/게임본문/Registry 변경0·TTS0·포커스/Play전환0. 기존 Matrix331행에 위험/보호/현재metadata 컬럼 반영, 기존 Listening JSON 역사 자료와244 marker 유지. Source→Manifest→Catalog 기술 일치가 실제 발화 의미 PASS를 보장하지 않는다. 다음은 사용자 실제Queue청취 결과 수집이다. 선행commit `85592e54d3109b1ccbb031c9deba7d1d9509b2bc`, push0.

## 2026-10-06 Main04 대기 지문 수정 및 전투 후 Voice 5건 재생성 전달

대기 문장 “상황을 살피고 있습니다.”는 `MainQuest04FieldFlow.CreateStoryActor`의 공통 NpcController 설정으로, Dialogue ID·Sequence index·Manifest·Voice entry가 없다. 태온 기본 상호작용에서 이름/Portrait가 붙던 상태 설명을 **`<지문> 상황을 살피고 있습니다.`**로 수정했다. DialogueLine의 명시적 prefix 규칙으로 Character Speaker·Portrait·Voice를 제거하며, 잘못된 Voice ID가 전달돼도 Resolve하지 않는다. Player와 기존 Character 대사는 유지한다. 331 authored Dialogue에는 이 대기 설정이 원래 포함되지 않아 개수는 유지한다.

Main01~16 Matrix 및 Flow의 행위 표현 자동 검색 후보 **6행(고유본문5개)** 중 확정 지문은 **고유본문1개 / NPC설정2곳**이다. 나머지4행은 실제 발화 문맥으로 유지했다. [정식 작성 규칙](../03_스토리/Story_대사_지문_작성_규칙.md)과 [후보 감사](Main01_16_Direction_Candidates.csv)에 근거를 기록했다.

미엘 `main04_miel_supp_009` / AfterBattleConversation index2의 정본은 “저도 볼 겁니다.\n이번에는 순서대로요.”다. Runtime·Manifest·정본 부록 일치. 사용자 Runtime 및 직접 원본 WAV 청취 “원본도 자막과 다름”으로 **WRONG_AUDIO_CONTENT 1건**을 확정했다. 추가 청취 답변 “순서가 뒤죽박죽이다”에 따라 인접 태온006·미엘010·태온007·미엘011 **4건은 SEMANTIC_INTEGRITY_SUSPECT**로 등록했다. 정확한 오발화 문장/개별 Wrong·Partial은 미확정이며 metadata로 의미 PASS를 추정하지 않는다. 실제 Clip/PCM 정합으로 Mapping·Cache 문제 재현0. **TTS_REGEN_REQUIRED 0→5**, 단일5행(Miel3/Taeon2) 전달. TTS 생성·WAV 수정·기존 파일 재배열0, **NEEDS_LISTENING244 유지**.

백그라운드 실제 대기 표시 및 Character→지문→Player 전환, 첫 조우10→전투 전3→Story Battle/QA 승리/실제 결과버튼 복귀→전투 후9 전체 **335개 기술 검사 PASS / FAIL0**. 전투 후7Voice 전체 원본PCM오차0/Player Voice·Portrait0/Next·Portrait 전환·Voice cleanup·UI Wrap/동적높이/본문·Footer 비중첩 PASS. 실제 지문 스크린샷도 prefix 전체·이름/Portrait 없음 확인. 기술 PASS와 위5건 의미 문제를 구분한다. 초기 QA의 단일 Show 창을 Next로 닫는 잘못된 기대는 기존 Esc/Hide 정책에 맞게 수정해 전체 재실행했고 초기 실패는 최종 PASS 수에 포함하지 않았다.

보호 대상 기존 3,046파일 중 변경은 Presenter 및 Main04 Flow 코드2개뿐이다. 새 Editor QA와 meta 추가, Main03/정상Main04첫9/후속004·007·008/태온005/전체WAV·meta·Catalog·Registry·사용자Save·설정 변경0. Compile Error0/Console Error0, 기존 CS0618 경고2개는 이력으로 구분한다. 종료 clean Bootstrap Edit Mode/격리Save 해제/is_focused=false/포커스전환0. 전략전투 조작 검증은 생략했다.

[13페이지 인접 순서](Main04_Direction_Miel009_Adjacent_Sequence.csv) · [전투 후9페이지 WAV/PCM 감사](Main04_Direction_Miel009_Sequence_Audit.csv) · [5행 TTS Handoff](Main04_Direction_Miel009_TTS_Handoff.csv) · [실제 Runtime 로그](Main04_Direction_Miel009_Runtime_Results.txt). 다음은5개 정식본문 새원본 제작 및 청취 후 별도 Unity 교체/연속검증이다. 선행 관련 commit `a5460c8f9843c9a800d628915e1da9872ed67285`, 이번commit은 최종보고 참조. GitHub Push 없음. 아래는 준비 당시 이력이다.

## 2026-10-06 Main04 지문 및 미엘009 원본 불일치 수정 계획

“상황을 살피고 있습니다.”는 MainQuest04FieldFlow.CreateStoryActor의 NpcController 공통 대기 설정이다. Dialogue ID·Sequence index·Manifest·Voice entry가 없으며 태온/미엘 두 Actor가 같은 문장을 공유한다. InteractionSystem→VillageNpcRole의 기본 대화 표시에서 태온 이름과 Portrait가 붙었다. Story 설계 Main04의 현장 관찰 문맥과 Flow의 공통 대기 분기를 근거로 상태 설명 지문으로 분류한다. 정식 표기는 `<지문> 상황을 살피고 있습니다.`이며 공통 DialogueLine 규칙으로 Character Speaker/Portrait/Voice를 제거한다. 문장 형태만으로 다른 대사를 자동 전환하지 않는다.

미엘 `main04_miel_supp_009`는 AfterBattleConversation index2이며 Runtime·Manifest·정본 부록의 “저도 볼 겁니다.\n이번에는 순서대로요.”가 일치한다. 사용자 Runtime 불일치와 이번 직접 WAV 청취 “원본도 자막과 다름”으로 WRONG_AUDIO_CONTENT를 확정했다. 틀린 실제 문장의 정확한 전사는 미확정이다. WAV/Mapping 재배열 없이 TTS Handoff를 작성하며 TTS 생성은 하지 않는다. 전투 후9페이지 전체와 대기 지문을 백그라운드 QA로 검사한다. 기존 정상 Main03·Main04첫9개·후속004/007/008 및 태온005를 보호한다. NEEDS_LISTENING244 유지. 선행 관련commit `a5460c8f9843c9a800d628915e1da9872ed67285`.

## 2026-10-06 Main04 후속 Voice 3개 최종 반영 및 연속 Runtime PASS

`Limitless_TTS_Regen_Main04_PostFirstEncounter_3`의 태온004·미엘007·008을 기존 Unity WAV 경로에 반영했다. 사용자 최종 지시 “맡겠습니다 로 해.”에 따라 태온004 본문은 “또 옵니다.\n제가 앞을 맡겠습니다.”로 유지했다. Runtime·Manifest·Handoff·제작 보고서가 일치하며 본문/Registry/Catalog 변경은 없다.

실제 첫 조우10페이지 종료 → 태온004 → 미엘007 → Player “갑시다.” → Story Battle → QA 전용 승리 처리/실제 결과 버튼 → Field 복귀 → 미엘008 → 태온005 경계까지 연속 실행했다. **221개 기술 검사 PASS / FAIL 0**, 새3개 전체 PCM 원본 오차0. 사용자 이번 답변 **“다 잘 들린다”**로 새3개와 태온005의 의미·전체발화·화자·음량 PASS를 확인했다. **TTS_REGEN_REQUIRED 3 → 0**, **NEEDS_LISTENING 244 유지**. 경계005 WAV는 교체하지 않았으며 NEXT_AUDIT_BOUNDARY_FAIL은 없다.

Player Voice/Portrait0, 태온·미엘 Portrait 복원, UI Wrap/동적높이/본문·Footer·Portrait 비중첩, 자연 종료 후 Next, Battle 전후 잔류 제거와 최종 Cleanup PASS. 전략 전투 조작 및 Main04 전체 Quest 완료 검사는 수행하지 않았다. 경계 페이지의 로그 NEXT 표시는 공통 QA 기록이며 실제 Advance는 호출하지 않고 검증 후 Hide로 종료했다.

보호 대상 기존 3,044개 파일 중 변경은 대상 WAV3개뿐이다. 정상 첫 조우9개/Main03/태온005/meta/GUID/Importer/Catalog/게임 C#/설정/사용자 Save 변경0. 새 Editor QA와 meta만 추가했다. Compile Error0 · Console Error0, 기존 CS0618 경고2개는 별도 이력이다. 종료 clean Bootstrap Edit Mode/격리 Save 해제/is_focused=false, 포커스 전환0.

[Source 및 Unity PCM 감사](Main04_PostFirstEncounter_Final_Source_Audit.csv) · [연속 Runtime 결과](Main04_PostFirstEncounter_Final_Runtime_Results.txt) · [전체 Sequence](Main04_PostFirstEncounter_PreSelfCare_Sequence_Audit.csv). 다음 권장 작업은 범위 밖 Voice의 별도 청취 감사다. 선행 관련 commit `83d0b4fd684434a219d1c1a6f23d9445f6e4a0fa`, 이번 완료 commit은 최종 보고에 기록한다. GitHub Push 없음. 아래 준비 단계는 당시 이력이며 현재 판정은 이 단락을 따른다.

## 2026-10-06 본문 충돌 해소 및 새 Voice 3개 적용

사용자가 최종 답변 “맡겠습니다 로 해.”로 태온 004의 기존 정본 “또 옵니다.\n제가 앞을 맡겠습니다.” 유지를 지시했다. 중간 “막겠습니다” 의도는 이 지시로 대체되었다. Runtime·Manifest·Handoff·새 제작 보고서 본문이 일치하며 본문 변경은 없다. 새 WAV 3개를 기존 경로에 반영했고 연속 Runtime 검증 대기다. 의미 해결 및 TTS_REGEN_REQUIRED 3→0은 이번 Runtime 청취 확인 이후 판정한다. 첫 조우 9개/Main03/태온 005/meta/GUID/Catalog/Registry 보호.

## 2026-10-06 Main04 후속 Voice 3개 적용 준비

새 폴더 `Limitless_TTS_Regen_Main04_PostFirstEncounter_3`에 대상 3개가 제작되어 있다. 미엘 007·008은 Unity 반영 대기다. 태온 004는 사용자가 이번 답변에서 정식 본문을 “또 옵니다.\n제가 앞을 막겠습니다.”로 변경하는 의도를 확인했다. 현재 로컬 Flow·Manifest·Handoff와 새 제작 보고서는 “맡겠습니다”이므로 MANIFEST_RUNTIME_DRIFT에 앞서 사용자 최신 의도와 기존 정본의 충돌이 확인되었다. “막겠습니다” 원본을 준비하기 전 태온 004를 적용하거나 의미 PASS 처리하지 않는다.

문서화 → 구현 → 검증 순서로 진행한다. 첫 조우 정상 Voice 9개, Main03 전체, 종료 경계 태온 005 WAV, GUID·meta·Importer·Catalog·Registry를 보호한다. 연속 Runtime 완료 전 TTS_REGEN_REQUIRED 3 및 NEEDS_LISTENING 244를 유지한다. 선행 관련 commit `83d0b4fd684434a219d1c1a6f23d9445f6e4a0fa`.

## 2026-10-06 Main04 첫 조우 이후 SelfCare 경계 직전 Voice 3건 감사

직전 FirstConversation 10페이지 / 9 Voice의 사용자 Runtime PASS를 유지한다. 감사 시작은 `main04_taeon_supp_004` (EncounterConversation index 0)이다. 종료 경계는 `main04_taeon_supp_005` (AfterBattleConversation index 1), 태온의 “본인부터 보셔야 하는 것 아닙니까?”이며 **HANDOFF_INCLUDED=NO**이다. 범위는 EncounterConversation 3페이지 → Story Battle → AfterBattleConversation index 0까지다. 총 Dialogue 4개, Voice 3개(미엘 2 · 태온 1), Player Silent 1개이며 전투는 Dialogue 수에 포함하지 않는다.

사용자가 실제 Runtime에서 이 범위의 전반적인 자막/발화 불일치를 확인했다. Voice 3개를 **SEMANTIC_INTEGRITY_SUSPECT / TTS_REGEN_REQUIRED 후보**로 일괄 등록했다. 각 WAV의 정확한 오발화 내용과 Wrong/Partial 개수는 개별 청취 증거가 없어 미확정이다. Metadata 정상만으로 의미 PASS를 확정하지 않는다. 기존 WAV를 재배열하지 않고 현재 Runtime 본문으로 새 원본을 제작한다. 재생성 목록은 **0 → 3**, 기존 NEEDS_LISTENING 관리 범위는 **244**로 유지한다.

[단일 3행 TTS 전달](Main04_PostFirstEncounter_PreSelfCare_TTS_Handoff.csv) · [전체 4행 Sequence 및 원본 감사](Main04_PostFirstEncounter_PreSelfCare_Sequence_Audit.csv) · [해시/PCM 교차 비교](Main04_PostFirstEncounter_PreSelfCare_CrossComparison.csv). 이번 작업의 WAV, Unity 코드, Catalog, 본문, Main03 및 정상 Main04 Voice 변경은 모두 0이다. TTS API 실행, Editor/Play 재실행, 포커스 전환도 없다. 마지막 관련 commit은 `ca103570983e18711184f4d63eaa5e8e396a2056`이며 이번 문서 commit은 최종 보고에 기록한다. 다음 작업은 이 3개 원본 제작 → 청취 채택 → 별도 반영 및 연속 Runtime 검증이다.

## 2026-10-06 Main04 첫 조우9개 새Voice 반영·전체Runtime 의미PASS

원본 `Limitless_TTS_Regen_Main04_FirstEncounter_9` 미엘6 Sulafat·태온3 Gacrux,제작원본9건USER_LISTENING_PASS.기존WAV9개전체바이트교체/Path·meta·GUID·Importer·Catalog·Speaker·본문유지. **사용자 이번 “9개 모두 Runtime 청취 PASS”** 확인으로전체9건 RUNTIME_LISTENING_PASS 확정.001 두문장 같은Line/다른위치이동0/한칸밀림0/Main03재등장0/다른화자0/부분누락0. **TTS_REGEN_REQUIRED9→0**,NEEDS_LISTENING244관리범위유지.

실제Main04 MielActor.TryInteract→FirstConversation10페이지(Voice9/Player1)전체연속검증 149 assertions PASS/FAIL0,TRACE9/NEXT10별도.9Clip전PCM원본오차0/SourceUnitySHA동일·Registry/Speaker/Manifest일치·Catalog247 ID/Clip중복0/Missing·Null0.각Clip자연종료후수동Next/Source정리·정확한다음Clip·Miel/TaeonPortrait/Player Voice0·Portrait0/Wrap·동적높이·본문/Footer/Portrait비중첩·최종Panel/Source/Portrait종료PASS.다음Main04전투Objective·격리자동Save읽기/첫대화완료countPASS.첫대화완료이지Main04전체Quest완료검사는아님.

[최종QA](Main04_FirstEncounter_Final_적용_QA.md)·[9개Source/Unity/PCM감사](Main04_FirstEncounter_Final_Source_Audit.csv)·[이번연속Runtime로그](Main04_FirstEncounter_Final_Runtime_Results.txt).보호 3044파일중변경은WAV9+기존EditorQA1(결과경로/캡처프레임대기/격리Save진행검사)뿐.게임C#/Main03전체12WAV/meta/Catalog/Save/Settings/Scene/Packages불변.CompileError0·최종ConsoleError0/Warning0,재컴파일기존CS0618경고2건은이력보존.종료cleanBootstrapEditMode/audit해제/Play옵션·설정복원/is_focused=false,포커스전환0.

다음권장:이번9건추가TTS불필요,244관리범위나머지Voice별도청취.마지막선행관련commit `4b1d2e1a8b3722e7a226add5862756ac2485a978`,이번완료commit은최종보고참조.원본폴더Stage0/직접변경만commit·GitHubPush0.아래는이전이력이다.

## 2026-10-06 Main04 첫 조우 Voice9개 전체 재생성 전달 확정

사용자 실제Runtime:001 첫문장만정상/두번째문장이다른위치에서발화,이후미엘·태온Voice전반적자막순서불일치/Main03태온처럼들리는발화재등장.9건모두 **SEMANTIC_INTEGRITY_SUSPECT / TTS_REGEN_REQUIRED**,001은기존직접WAV청취로 **PARTIAL_AUDIO_CONTENT** 추가확정.나머지각파일의틀린정확한문장/Character를추측하지않는다.이전Runtime Playback/PCM/148검사PASS는기술증거로유지하고의미PASS로사용하지않는다.

LOCAL FirstConversation10페이지/Voice9(미엘6 Sulafat·태온3 Gacrux)/Player1 정상무음.원문을Flow에서재추출하고현재Manifest와줄바꿈포함9건일치,본문Drift0.기존WAV재배열/Mapping수정없이9개전체정식본문재제작방향확정. **현재LOCAL 재생성필요1→9**(사용자요청의0→9는이전001등록전기준;001중복등록없이전체9). NEEDS_LISTENING244관리범위유지/Main03해결상태보호.

정본전달 [9행 Handoff](Main04_FirstEncounter_TTS_Regen_Handoff.csv),[WAV9개감사](Main04_FirstEncounter_WAV_Audit.csv),[Main03비교원본목록](Main04_FirstEncounter_Main03_Comparison_Inventory.csv),[교차비교](Main04_FirstEncounter_CrossQuest_Comparison.csv),[Main04내부비교](Main04_FirstEncounter_Internal_PCM_Comparison.csv).현재Main03 WAV12개와9×12=108쌍/첫조우내36쌍비교.동일SHA/PCM cross duplicate 0,내부PCM duplicate 0.내용이유사한발화는해시불일치여도배제할수없어사람청취증거와구분한다.

WAV/TTS API/Unity코드/Catalog/정식본문/Player/Main03Voice변경0.이번Editor/Play재실행0/포커스변경0,문서화→감사·전달생성→정적검증.다음:이9행으로온전한원본제작→개별청취→별도승인된반영작업에서기존GUID보존교체·연속Runtime검증.마지막관련commit `3ab8e67a71fc684d55796b012aade22ea943ad07`,이번문서commit은최종보고참조.아래는이전감사이력이다.

## 2026-10-06 미엘 첫 조우 전체10페이지 감사 / 원본001 누락 확정

[전체 QA](Miel_First_Encounter_Voice_QA.md)·[순서10행](Miel_First_Encounter_Sequence_Audit.csv)·[확정 TTS전달](Miel_First_Encounter_TTS_Handoff.csv).Main04 FirstConversation,미엘6/태온3/Player1.문제 main04_miel_supp_001 정식본문 `조금만 참으세요. / 출혈은 멎었습니다.` 중사용자Runtime와직접WAV모두첫문장만청취:CaseA PARTIAL_AUDIO_CONTENT.2초원본전체PCM48000오차0/자연종료/자동Next·조기Cleanup재현0.이번TTS생성/Playback수정/WAV수정0,정확한전체본문으로재생성전달.

연속148assertions PASS/FAIL0,9Voice전PCM정합/Player Voice0·Portrait0/MielPortrait·UI/최종Cleanup·첫대화종료→다음Encounter Objective·격리Save읽기PASS.의미정상확정0/Partial확정1/나머지8 USER_LISTENING_REQUIRED;Metadata정상으로의미PASS추정금지. **TTS_REGEN_REQUIRED0→1**,NEEDS_LISTENING244기존관리범위유지,Main03태온008~012완료보호.Compile/최종ConsoleError0·Warning0.3042기존파일해시변경0,새EditorQA만추가/cleanBootstrapEditMode/포커스전환0.

다음:미청취8건전체발화확인→확정누락001원본재제작→사용자청취/기존GUID보존교체·회귀.마지막관련commit `5e660dba9fac846932eb3e075735a22e19ff6fa9`,이번감사commit은최종보고참조.직접변경만커밋·GitHubPush없음.아래는이전이력이다.

## 2026-10-06 Main03 008~012 최종 반영·연속 Runtime 사람 청취 PASS

008/010/011 새 원본·009 **NeFix 최종본**을 기존 Unity WAV 내용만 교체.012는 지정 원본과 이미 동일하여 재작성0/해결유지.5건 Source 사용자청취PASS + 이번 사용자 **“5개 모두 Runtime 청취 PASS”** 확인: 의미·전체문장·음량PASS,009 `네.` 포함.**TTS_REGEN_REQUIRED4→0**, NEEDS_LISTENING244 기존관리범위유지.331 Matrix에 완료flag 기록.005/006/007/003/004 기존PASS와001/002 보호,Player 정상무음.

LOCAL Runtime/Manifest 본문5건 줄바꿈 포함일치·Source/Unity WAV SHA동일·실제Unity PCM5개 전샘플오차0.실제 첫Battle→QA전용승리종료→정식복귀→008→Player→009→Player→010→011→012→단서001→002→Main03종료 연속PASS.전략전투/물리입력검증 아님.기존 QA 도구로 PlayUnfocused·격리Save/Settings 사용,포커스전환0.기술검사 227 PASS/FAIL0(7 TRACE·9 AUDIO는별도),Player Voice0/Portrait0·태온복원·UI Wrap/동적높이/본문잘림0/Footer·Portrait비중첩·끝까지Next/빠른Next/최종정리/Quest완료/격리자동Save읽기PASS.

[최종 적용QA](Main03_Supp008_012_Final_적용_QA.md)·[5건 Source/PCM 감사](Main03_Supp008_012_Final_Audit.csv)·[이번 연속 실행로그](Main03_Supp008_012_Final_Runtime_Results.txt).보호 3042파일 비교:변경은 Unity008~011 WAV4개뿐,meta/GUID/Importer/Catalog/게임·Editor C#/Save/Settings/Scene/Packages불변.종료cleanBootstrap EditMode/audit해제/격리설정·Play옵션복원.CompileError0/ConsoleError0·Warning0 조회;C#변경없어강제재컴파일0.초기 준비스크립트 실행실패 후 조기QA를중단했고 WAV교체후전체재실행PASS,조기실행은최종증거에포함하지않음.

다음 권장:244 관리범위의미확인Voice를별도청취.이번5건추가TTS불필요.마지막선행관련commit `e355b44c9b2a87b7072d504d95db33560ebf7b7c`,이번완료commit은최종보고참조.직접변경만Stage,원본TTS폴더/다른WAV Stage0·GitHubPush0.아래는이전이력이다.

## 2026-10-06 Main03 008~011 사용자 Runtime 의미 불일치 확정·TTS 전달

사용자 실제 Runtime 청취에서 008~011 모두 자막과 발화 불일치 확인. 네 건 모두 **WRONG_AUDIO_CONTENT / RUNTIME_SEMANTIC_MISMATCH**이며 Metadata 정상 여부로 의미 판정을 되돌리지 않는다. **TTS_REGEN_REQUIRED 1→4**, NEEDS_LISTENING **244는 기존 관리 범위**로 유지(이번 확정 문제/이미 해결된 항목을 포함하므로 미확인 244건이라는 뜻 아님). 012는 새 WAV·사용자 직접 청취·Unity Runtime 의미/전체 문장/정상 음량 PASS 해결 유지. 005/006/007/003/004 PASS와 001/002 기존 상태, Player 정상 무음 보호.

LOCAL MainQuest03FieldFlow.AfterBattleConversation 및 DialogueLine의 구형 화자 접두어 제거 규칙에서 전체 표시 본문을 추출한다. 현재 Manifest와 네 건 모두 줄바꿈까지 일치. 실제 AfterBattle 페이지 순서는 1/3/5/6(index 0/2/4/5); Player 페이지를 제외한 전달 4행이다. 009는 `네.` 뒤 줄바꿈을 보존한다. Actual Spoken Text는 **USER_CONFIRMED_MISMATCH**로 기록하며 잘못된 문장을 추측하지 않는다. 이전 008 직접청취의 별도 증거는 보존한다.

정본 전달: [4행 Handoff](Main03_Taeon_Supp008_011_TTS_Handoff.csv), [원본/참조 감사](Main03_Taeon_Supp008_011_Voice_Audit.csv), [공통 연기/설정/검증](Main03_Taeon_Supp008_011_TTS_Handoff_QA.md). 기존 7행 Remaining Handoff는 원본 청취 감사 목록이며 새 재생성 전달에는 이 4행만 사용한다. 문서화→감사/전달 생성→백그라운드 정적 검증 순서. WAV/meta/Unity 코드/Catalog/설정 변경 및 TTS API/Push 0. 이번 Editor/Play 재실행 없음; 새 음성 제공 후 직접/Runtime 의미 청취가 다음 작업이다.

마지막 선행 관련 commit: `bd4bffeca1efb1789079197f3b215e19ddb0fe48`. 이번 문서 commit은 최종 보고 참조. 아래 기존 기록은 당시 이력이며 현재 판정은 이 단락과 최신 Matrix/재생성 목록을 따른다.

## 2026-10-05 Main03012 최종 반영·후반부 연속 QA

012 새원본7.76초/RMS−16.16dBFS를기존WAV에교체,GUID/meta/import/Catalog보존. 사용자원본/이번Unity Runtime **의미·전체문장·음량PASS** 확인,PCM186240개오차0.012저음량해결.008은직접WAV/Runtime의006계열발화로 **WRONG_AUDIO_CONTENT** 확정,이번실제008 Resolve/Source도008이며MappingFix없음. **TTS_REGEN_REQUIRED2→1(008)**,244 NEEDS_LISTENING원래관리범위유지,후반미청취5종유지. 기존005006007003004보호.

008→Player→009→Player→010→011→012→단서001→002→Main03완료를격리연속검증:무음213/실제재생227 assertions PASS/FAIL0,각AUDIO9/TRACE7별도. Player Voice0/Portrait0,태온Portrait/UI/Next/최종Voice·Portrait정리/Quest완료/격리자동Save읽기PASS. 사용자Save/Settings/포커스변경0. CompileError0/최종ConsoleError0·Warning0;기존deprecated2/MCP연결경고1이력보존. 새기능/게임C#변경0.

상세 [적용·최종 QA](Main03_AfterBattle_FinalVoice_적용_QA.md). 다음작업:008정확한 “역시 이상합니다.” 원본제공→청취/교체/회귀,009010011001002는일괄의미청취. 마지막관련구현commit `06666354eeaf9e2c328b058895751872b405ef88`. 최종문서commit은보고참조. 아래는이전감사이력이며최신판정은이단락을따른다.

## 2026-10-05 Main03 008 의미 오류 원인 확정

008 WAV 직접청취와 실제 Runtime 모두 사용자 확인 “그냥 돌아다니는 것 같지만…” 계열. 기대 “역시 이상합니다.”와 불일치: **Case B WRONG_AUDIO_CONTENT / RUNTIME_SEMANTIC_MISMATCH**. 실제008 페이지 Resolve/AudioSource는008이며006과 Path/GUID/reference/hash가 모두 다르다. 기존 재생PASS는 기술 결과이며 의미PASS 해석을 철회한다. C#/Mapping/WAV 변경0. 올바른008 새 TTS 대기. 확정 재생성 **2건(008 의미 오류 +012 저음량 별도)**; 의미 청취 대기6종. 기존244 관리범위 유지. 정상005/006/007/003/004 보호. 기존 Player Voice0/Portrait0 결과 유지. 사용자 Play/Scene/Save/포커스 변경0. Console Error0/Warning0 조회 확인, C# 변경 없어 강제 컴파일 없음. 상세: [008 원인 QA](Main03_008_VoiceIdentity_원인_QA.md). 이전 관련 commit9a82ff8, 이번 commit은 최종 보고 참조.

아래 이전 감사는 당시 기술 검증/미청취 이력이며008 최신 판정은 위 결과를 따른다.

# Story Dialogue / Subtitle / Voice / Portrait 전수 감사

> 후반부 전체 감사(2026-10-05): [Main03 남은9페이지QA](Main03_RemainingVoice_감사_QA.md).008 실제 전투후 첫대사이며 연결/재생 정상, 보고된무음은미재현·원인미확정.012원본저음량으로 확정재생성0→1. 후반부7Voice 의미청취는 사용자 “이번재생을듣지못함”으로 USER_LISTENING_REQUIRED이며 일괄Handoff에모두포함했다.005~007/003/004 기존PASS5·244관리범위보존, WAV/Mapping/게임코드수정0.

> 최종 후속(2026-10-05):003/004 새 원본을 기존 Unity WAV/GUID/meta 그대로 반영, 사람 실제 Unity 의미/끝까지 재생 PASS. [최종 집중 QA](Main03_Supp003004_Regen_적용_QA.md). 확정 재생성2→0·005~007 기존PASS 유지.244 NEEDS_LISTENING 관리범위는 보존하며 이번 완료2행은 RUNTIME_LISTENING_PASS를 함께 기록한 기존 backlog 부분집합이다. 현재 미확인244개 또는 총249음성으로 해석하지 않는다. Catalog247 유지.

> 최신 정정(2026-10-05): 사용자005~007 실제 Runtime 의미 청취 PASS 확정으로 기존 재생성3→0 해결. 새 전투 직전003 저음량/004 다른·부분 문장 원본 문제는 G. OTHER로 분류하고 재생성2건을 기록했다.244 NEEDS_LISTENING은 그대로이며 새2건은 그 부분집합이다. [후속 Voice·장문 UI QA](Main03_FollowupVoice_DialogueLayout_계획_QA.md)에서 실제 Actor→Battle/3해상도550 assertions PASS + PCM2건를 확인했다. 아래 표와 최초 음성 불일치3건은 최초 감사 시점 이력이며 현재 Matrix/전달CSV를 정본으로 사용한다.

2026-10-05 LOCAL Source of Truth. 문서화 → 중앙 Runtime 수정 → 격리 PlayUnfocused 검증 순서로 진행했다. **Player 화자/Portrait 수정은 PASS, 음성 의미 정합성은 미완료다.** 사용자 청취로 태온 첫 만남 3개 불일치를 확인했고 나머지 244개는 NEEDS_LISTENING이다. 기술 연결 PASS를 실제 발화 PASS로 해석하지 않는다.

## 전체 작성 대사와 범위

| 항목 | 수 |
|---|---:|
| 전체 작성 페이지 | 331 |
| Intro Narrator | 18 |
| Main01~16 | 313 |
| Player | 42 |
| Voice 대상 / 실제 Catalog 연결 | 247 (Story229 + Intro18) |
| 의도적 무음 | 84 |
| 사용자 청취 불일치 / 재생성 필요 | 3 |
| 아직 의미 청취 미검증 | 244 |

| Main | 작성 페이지 | Main | 작성 페이지 |
|---|---:|---|---:|
| 01 | 10 | 09 | 32 |
| 02 | 7 | 10 | 25 |
| 03 | 16 | 11 | 31 |
| 04 | 22 | 12 | 4 |
| 05 | 14 | 13 | 8 |
| 06 | 4 | 14 | 11 |
| 07 | 40 | 15 | 22 |
| 08 | 23 | 16 | 44 |

작성된 페이지를 센다. 같은 대사를 여러 번 보여 주는 재진입 횟수는 분모에 더하지 않는다. Main02는 5 Path+default 반응6개와 공통 결론1개다. Main16은 start/ash/vibration/tracks/witness_ground/witness_emerge/retry/afterimage/canyon/report와 실제 Hearing/Default/공통 분기를 모두 포함한다. Main13~15의 Hearing Retrofit은 설계 후보이며 실제 존재하지 않는 분기를 만들어 세지 않는다. Main16 Player9개·관찰7개는 정상 무음이다.

Intro의 마지막 무음 제목은 Narrator18개에 넣지 않는다. 공용 NPC 서비스 대사는 Story 작성 페이지와 별도다. `Chapter2IntroFlow`의 npc.Dialogue fallback, `VillageNpcRole`의 편성/치료/상점 확인과 일반 npc.Dialogue, `InteractionSystem`의 일반 상호작용, `MainQuest11DungeonFlow`의 던전 입구 확인은 공용 Show/ShowConfirmation 경로를 사용한다. 중앙 Player 차단은 이 경로에도 적용된다. 동적 NPC 데이터 전체를 새로운 Main 대사로 늘려 세거나 전부 실제 상호작용했다고 주장하지 않는다.

## 문서·원문·Manifest 정합성

- [Matrix](Story_Dialogue_Audit_Matrix.csv)와 [상세 JSON](Story_Dialogue_Audit.json)에 Source 위치, raw 값, 실제 생성한 Runtime Speaker/이름/본문, 분기, Manifest, Catalog/GUID/WAV/해시/길이, Portrait, 청취 결과를 기록했다. 감사 키는 Source 위치 식별자이며 새 게임 Dialogue ID가 아니다.
- 기존 문서의 정확한 본문 대조 근거는 23개였다. **308개는 DOC_TEXT_NOT_SPECIFIED**였으며 이를 308개 DOC_RUNTIME_DRIFT로 부르지 않는다. 최신 LOCAL 본문을 [원문 부록](../03_스토리/LOCAL_Story_Dialogue_원문_부록.md)으로 기록한 뒤 331개 모두 정본 근거가 있다. 이는 기존 구현의 명문화이며 별도 게임 기획 확정이 아니다.
- 실제 발화 본문 기준 문서↔Runtime↔Manifest의 확인된 문장 불일치0. Trim/개행/구형 Main03 화자 접두사는 정규화해 비교하고 raw 값도 보존한다. 기존 디자인 문서와 의미상 동일하다고 자동 판정한 것은 아니다. Story README의 Main16 미구현 표시는 LOCAL 후속 구현 이력에 맞게 정정했다.
- Manifest Text mismatch0, Manifest의 확인된 STALE_TTS_TEXT0. 최신 Text 정본은 LOCAL 현재 구현 본문이며 잘못된 WAV에 자막을 맞추지 않는다.
- 네 제작 Manifest와 Intro 제작 CSV를 조사했다. PreSerin104행 중 실제 Main14에 해당하는 main12_miel_001/main12_taeon_001 두 입력은 기존 제외 정책에 따라 미사용이다. 올바른 Main14 팩으로 대체된 항목이며 Missing Audio가 아니다. Supplement Existing Voice Wins 정책과 합본 제외를 유지했다.

## Speaker / Catalog / 실제 음성

| 검사 | 결과 |
|---|---|
| Speaker mismatch | 중앙 처리 전4 → 처리 후0 |
| Duplicate Dialogue ID | 0 |
| 동일 ID의 서로 다른 화자/본문 Branch Collision | 0 |
| 기술 ID/화자/파일 연결 | 247/247 |
| Missing Audio / Unexpected Audio | 0 / 0 |
| 원본 파일과 Import 해시 불일치 | 0 |
| Duplicate Clip Reference / 미사용 Catalog ID | 0 / 0 |
| 실제 음성 Wrong Clip | 사용자 청취 보고3 |
| 잘린 발화 | 007에 확인1 |
| NEEDS_LISTENING | 244 |
| TTS_REGEN_REQUIRED | 3 |

Wrong Clip3개도 파일명/GUID/원본 해시/Manifest는 정상이다. **원본 제작 파일의 실제 내용과 문서상 ID/Text가 불일치**한다. 006이 005 문장을 말하고 007이 006의 첫 문장만 말한다는 점은 사용자 청취 증거다. 제작 단계의 파일 밀림 여부나 어디서 잘렸는지는 확정할 수 없다. Duplicate ID/중복 Clip/Manifest Text drift/Player Voice 잔류를 원인으로 단정하지 않는다.

WAV와 Catalog는 이번에 변경하지 않았다. 잘못된 Voice를 정상이라고 채택하지 않으며 세 파일은 재생성/올바른 원본 교체 전까지 **KNOWN_BAD_AUDIO**다. 잘린 파일을 임의 Shift Mapping해 다른 문장에 사용하는 추측 수정은 하지 않는다. 이번 요청은 TTS 생성 금지이므로 [재생성 전달 CSV](Story_Dialogue_TTS_REGEN_REQUIRED.csv)에 필요한 정확한 본문/실제 청취/사유만 기록한다. 현재 게임에는 잘못된 원본 연결이 남아 있으며 음성 문제 해결 완료를 선언하지 않는다.

## 태온 첫 만남 집중 결과

Scene Field_01, Main03 FirstConversation. 태온 ID는 companion_taeon / Gacrux. 연결 파일은 모두 `Assets/_Project/Audio/Voice/Story/Main03/<ID>.wav`. Manifest는 보충팩 `dialogue_manifest_missing_main01_05_08_12_v2.csv`이며 아래 기대 본문과 일치한다.

| 순서 / ID | 화면·Manifest 기대 본문 | 실제 WAV 청취 | 판정 |
|---|---|---|---|
| 0 / main03_taeon_supp_005 | 잠깐만요. 더 가까이 가지 않는 게 좋겠습니다. | 사용자: 나머진 다 틀려. 005의 실제 발화 원문은 미확정 | WRONG_CLIP 사용자 보고 / 재생성 필요 |
| 1 / ID 없음 / Player | 무슨 일이 있습니까? | 음성 없음 | 정상 |
| 2 / main03_taeon_supp_006 | 저 몬스터들 말입니다. / 그냥 돌아다니는 것 같지만… / 계속 같은 쪽을 피하고 있어요. | 잠깐만요. 더 가까이 가지 않는 게 좋겠습니다. | WRONG_CLIP / 재생성 필요 |
| 3 / ID 없음 / Player | 저도 조금 전에 이상한 흔적을 발견했습니다. / 마을 쪽으로 몰려온 흔적이었습니다. | 음성 없음 | 정상, Portrait 수정 |
| 4 / main03_taeon_supp_007 | 그렇군요. / 그러면 제가 보고 있던 움직임하고 / 이어질지도 모르겠습니다. | 저 몬스터들 말입니다. 만 말하고 종료 | WRONG_CLIP + TRUNCATED_AUDIO / 재생성 필요 |

005의 불일치는 제공한3개에 대한 사용자 보고로 기록한다. 실제 발화 미확정과 006/007의 명시적 청취 원문을 구분한다. “나머지도 틀렸다”를 아직 듣지 않은 전체244개 불일치로 확대하지 않는다. 청취 결과는 [사용자 증거 JSON](Story_Dialogue_Listening_Results.json)에 남겼다. Codex 현재 세션은 오디오 입력/전사를 지원하지 않아 직접 듣고 맞다고 꾸며내지 않았다.

## 중앙 Player 규칙과 Portrait

구형 Main03의 raw SpeakerId는 모든 페이지가 companion_taeon이고 표시 이름은 대화, 본문 첫 줄에 태온/플레이어를 넣었다. Player4개가 태온 Portrait를 얻던 것이 확인된 Portrait 원인이다. 기존 실제 Player 화면에서는 Clip null/IsPlaying false였으므로 음성 잔류 원인으로 확정하지 않았다.

- `DialogueLine`이 구형 접두사를 중앙에서 분리하고 Player stable ID를 player로 통일한다. 현재 PlayerName을 표시하며 발화 본문과 순서는 보존한다. 정식 NPC ID는 Player가 NPC와 같은 이름을 골라도 보존한다.
- `DialoguePresenter`는 Player 페이지에서 Catalog 조회와 음성 fallback을 차단하고 기존 Play(null)의 즉시 Stop/Cleanup을 사용한다. 단일 Show/확인창도 동일 DialogueLine 정규화 경로를 쓴다.
- `DialoguePortraitCatalog`는 Player ID의 Portrait를 중앙에서 차단한다. 기존 ApplyPortrait(null)가 Sprite clear/Container inactive/글자 여백 복원을 수행한다. 개별 Flow 변경이나 Player 임시 Portrait 제작은 없다.

| 화자 | 작성 페이지 | Voice 연결 | Portrait |
|---|---:|---:|---|
| Player | 42 | 0 | NONE, 실제 표시0 |
| 태온 | 79 | 76 | Portrait_Taeon, 자체 Portrait |
| 미엘 | 54 | 50 | Portrait_Miel, 자체 Portrait |
| 폴 | 75 | 70 | Portrait_Paul, 자체 Portrait |
| 세린 | 25 | 25 | Portrait_Serin, 자체 Portrait |
| 레온 | 8 | 8 | 미제작, NONE/text-only |
| 주민 대표 | 8 | 0 | 등록 null, text-only |
| 남문 경비병 | 7 | 0 | 등록 null, text-only |
| 잡화 상인 | 1 | 0 | 미등록, text-only |
| 관찰/조사 | 14 | 0 | NONE |
| Narrator | 18 | 18 | Intro 규칙, Character Portrait 없음 |

잘못된 Fallback은 처리 전 Main03 Player4 → 처리 후0이다. Named Portrait4종은 정상이고 미제작 Portrait를 다른 인물로 대체하지 않는다.

## 백그라운드 Runtime·컴파일

사용자 승인으로 기존 Play 종료 후 격리 Save/Settings·PlayUnfocused QA를 실행했다. OS/Game View/Unity foreground 전환 없음. 2574 checks PASS / FAIL0. 기술 테스트는 실제 WAV 의미를 판정하지 않는다.

- 실제 Main03/04/05 factory, Main09/10/11 step와 Main11 AfterBoss, Main16 모든10scene×양분기를 호출해 원문/ID/화자를 대조했다. Main01~16의313개 Source 작성 대사를 실제 DialogueLine/Presenter에 전수 전달해 자막·Clip·Portrait·Next/끝 정리를 확인했다. 전체 Quest를 사용자 입력으로 끝까지 플레이한 테스트는 아니다.
- Player42개 모두 Clip0/playing false/Portrait Sprite null/Container inactive. NPC→Player Stop/Cleanup, Player→NPC Portrait 복원, 연속 Player/연속 Next, 단일 Show/확인창 PASS. 잘못 등록한 Player Voice/Portrait 테스트 fixture도 중앙 차단 PASS, 실제 Asset 변경 없음.
- Disable/Scene 전환 시 Voice 정리 PASS. Intro18개 ID/Clip 확인, 연속 Next8회와 Skip→Bootstrap 정리 PASS. Intro18개를 실제 의미 청취하거나 전체 수동 Story 재플레이하지 않았다.
- Voice0, Mute, Unmute와 독립 Voice/SFX/BGM 값 유지 PASS. 정식 Mixer/볼륨 저장 원본 변경 없음.
- Unity 6000.5.7f1 컴파일 완료/오류0, 최종 Console Error0/Warning0. 기존 ExternalAssetImportEditor CS0618 경고2건은 이전 이력이며 수정 범위 밖이라 유지했다. QA 도구 호출 중 namespace/refresh 전 타입 미해결2회는 MCP 임시 실행 컴파일 실패이며 프로젝트 C# 오류가 아니다. 전체 refresh 후 해결했다.
- 종료 상태 clean Bootstrap Edit Mode, is_focused false. 보호 파일3032개 중 기존 변경은 중앙 UI 코드2개뿐. 원본 WAV/meta/GUID/Catalog/Mixer/Save/Settings/Packages/Scene/사용자 기존410항목(94그룹)은 유지했다.

## 다음 작업과 재검증

이번 범위에서 TTS 생성/normalize/trim/reencode/rename, 새 기능/큰 구조 변경, 사용자 수동 이미지 수정, GitHub Push는 없다. 다음 음성 제작 세션에는 재생성 CSV의3개만 현재 본문으로 제작하거나 올바른 완전한 원본을 제공한 뒤 **실제 청취 → 기존 ID 파일 교체 → 격리 회귀 QA**를 진행한다. 나머지244개는 우선순위에 따라 별도 청취하며 현재 기술 PASS만으로 자동 채택하지 않는다.

재실행 도구: `StoryDialogueConsistencyAudit.Launch()`는 clean Bootstrap에서 실행하며 `Temp/StoryConsistency20261005/runtime.txt`에 결과를 쓴다. `python Tools/TTS/story_dialogue_consistency_audit.py`는 실제 Runtime export와 PASS 근거가 있어야 Post Matrix를 갱신한다. 구현 전 Source 조사만 `--pre`로 실행할 수 있다. 원본 제작 WAV/게임 데이터는 Python 도구가 수정하지 않는다.

문서화 commit `af48c2e`. Runtime/최종 감사 commit은 CURRENT_STATUS와 최종 보고 참조. 직접 변경 파일만 Stage하고 작업 diff --check를 검사한다. 기존 사용자 whitespace 문제는 수정하지 않는다.
