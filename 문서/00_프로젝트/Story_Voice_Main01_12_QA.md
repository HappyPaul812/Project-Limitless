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

# Main01~12 Story Voice 적용

## 입력과 적용 기준 — 2026-09-30

입력은 `F:/study/codex/Project-Limitless/Limitless_TTS_Output_PreSerin`이다. CSV `dialogue_manifest_main01_12.csv`의 화자·본문·source_file을 LOCAL 코드와 정확히 대조한다. 파일 순서나 source_pos만으로 추측하지 않는다. CASTING.json의 Voice ID를 사용하며 모델·기본 속도·Tone·제작 Batch는 자료가 없어 TBD로 둔다.

| Character Stable ID | 이름 | Voice Design / 제공 ID | 적용 수 |
| --- | --- | --- | ---: |
| companion_taeon | 태온 | Gacrux / Gacrux | 28 |
| companion_miel | 미엘 | Sulafat / Sulafat | 22 |
| companion_paul | 폴 | Achird / Achird | 52 |

위 Design은 CASTING의 display_name이다. 음색·연기 지시를 임의로 확정하지 않는다. 개발 적용을 채택하되 실제 청취 품질은 미검증이다. Narrator Storyteller_4와 Intro 18개는 기존 Registry를 유지한다. 세린 companion_serin은 모두 TBD·미적용이다.

107개 WAV 중 대사 segment는 104개, 캐릭터별 연결본은 3개다. CSV가 Main12로 표시한 미엘·태온 각 1개는 LOCAL `field05_main14_strong_pulse`의 Main14 대사다. 이 2개와 연결본 3개는 Import/Mapping에서 제외한다. TEXT_AUDIO_MISMATCH는 0개이며 정확히 대응하는 102개만 적용한다.

Main01~05·Main08·실제 Main12 음성은 제공되지 않았다. 기존 본문을 보존하고 선택적 Voice 없는 텍스트 대화로 진행한다. 통계는 해당 Main Flow의 정적으로 작성된 대화 쪽/분기 본문을 세며 재시도에 다시 작성된 쪽도 포함한다. Main12는 명시적 조사 3종과 잡화상인 1종이다. 공용 NPC 전체 대사를 포함하는 전 게임 통계는 아니다.

## 연결 계획

원본 WAV를 `Assets/_Project/Audio/Voice/Story/MainXX/`로 바이트 그대로 복사한다. Unity Import는 PCM·원본 샘플레이트·강제 모노 변환 및 정규화 없음으로 구성한다. 속도/pitch/trim/gain/EQ/재인코딩·새 TTS 생성은 하지 않는다.

기존 DialogueLine에 선택적 Dialogue ID를 추가하고 기존 VoiceClipCatalog 및 VoicePlaybackSource를 재사용한다. Manifest clip_id를 stable ID로 명시하며 Catalog의 Character ID까지 확인한다. 한 쪽=한 Clip인 제공 자료이므로 별도 segment 재생 구조는 추가하지 않는다. Next/연속 Next는 같은 Source의 현재 Clip을 즉시 교체한다. 수동 Next 정책은 유지하고 음성이 끝나도 대화를 자동 진행하지 않는다. Hide·단일 대화/확인창 전환·비활성화·Scene 제거에서 음성을 정리한다. Voice Mixer와 사용자 설정만 사용하며 Story Save에 Voice 상태를 추가하지 않는다.

## 검증 계획

재현 감사 도구는 `Tools/TTS/audit_story_voice.py`, 결과는 `Tools/TTS/story_voice_audit.json`이다. Import 디코딩/GUID/참조/중복 및 원본 SHA-256을 검증한다. 백그라운드 Play Mode에서 인물별 샘플, 실제 대사 factory, Next/연속 Next/null fallback/종료, Voice Volume/Mute 복원, Intro Next/Skip/18개 참조 회귀를 확인한다. Scene·Save·사용자 설정은 검증 전 상태로 복원한다. Game View 활성화나 foreground 검증은 현재 작업에서 별도 허락 없이는 실행하지 않는다.

실제 청취에서 발음·감정·호흡·문장 발화 일치·컷 경계·대사 간 음량 균형·음성/자막 타이밍은 사람이 확인해야 한다. 메타데이터와 자동 상태 검증을 청취 검수로 간주하지 않는다.

## 적용 결과

- 전체 입력 107개: PCM WAV 24kHz/mono/16bit, 합계1116.56초. Segment104개558.28초, 연결본3개558.28초. 실제 Import102개550.97초. 원본 바이트102개 동일, 전체 입력107개 프레임 디코딩 성공. 신규 GUID102개 중복 없음, Catalog 참조102개 정상. Import PCM/원본 rate/normalize0/forceToMono0, 오류0.
- `verify_story_voice.py`로 변경한5개 Flow의 모든 정적 화자/본문 순서가 Git 기준과 동일함을 검증했다. Mapping ID102개, 중복0, TEXT_AUDIO_MISMATCH0. 미연결2개는 Main14 범위 제외이며 연결본3개는 개별 Dialogue Clip이 아니다.

| Main | 대화 쪽 | 입력 Segment | Mapping | Voice 없음 | Mismatch | Unmapped |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 01 | 10 | 0 | 0 | 10 | 0 | 0 |
| 02 | 7 | 0 | 0 | 7 | 0 | 0 |
| 03 | 16 | 0 | 0 | 16 | 0 | 0 |
| 04 | 22 | 0 | 0 | 22 | 0 | 0 |
| 05 | 14 | 0 | 0 | 14 | 0 | 0 |
| 06 | 4 | 3 | 3 | 1 | 0 | 0 |
| 07 | 40 | 26 | 26 | 14 | 0 | 0 |
| 08 | 23 | 0 | 0 | 23 | 0 | 0 |
| 09 | 32 | 27 | 27 | 5 | 0 | 0 |
| 10 | 25 | 18 | 18 | 7 | 0 | 0 |
| 11 | 31 | 28 | 28 | 3 | 0 | 0 |
| 12 | 4 | 2 | 0 | 4 | 0 | 2 |
| 합계 | 228 | 104 | 102 | 126 | 0 | 2 |

백그라운드 실제 Play Mode에서102쪽을 같은 프레임에 Next로 진행해 ID/Clip/Character/Voice Group과 마지막 정리를 확인했다. Missing ID·다른 화자 ID는 Clip 없이 자막과 Advance로 진행한다. 실제 Main07 PaulFirst·Main09·Main10·Main11 factory와 Main01 배열·Main02 반응/결론·Main05 보고의 무음 진행/완료 callback을 확인했다. Main06은 감사 대응 데이터로 재생했으며 OnInteract의 실제 입력·퀘스트 전체 진행 검증은 아니다. Main12 조사 fallback은 실제 Line factory로 확인한다. 대화 Hide/단일 Show/확인창/비활성화 및 재생 중 Scene 전환으로 Story 객체 제거를 확인했다.

AudioListener 출력 RMS peak는 Main06 태온0.25854/Main07 폴0.14828/Main09 미엘0.01681/Main11 폴0.04523이다. Voice0은 Mixer -80dB이며 전환 포함 peak0.00006305, Mute는0, 복원은0.18701이다. Voice0에서도 Source의 재생 시간은 진행하며 텍스트는 수동 Next를 기다린다. 전체 Mute는 Master -80dB/해제0dB, 채널72/63/47 값을 유지한다. BGM/SFX 정식 음원 신규 도입은 없다.

Intro 18개 Catalog 참조, 실제8회 연속 Next의 상대 인덱스/Clip, Voice0/Mute/복원, Skip→CharacterCreation 및 잔류 VoiceSource0을 확인했다. 최초 QA 도우미는 Scene 진입 후 자동 진행된 시간을 무시하고 Next가002라고 고정 가정하여 실패했다. 실제 현재 인덱스 기준으로 수정한 도우미로 재검증했으며 게임 코드 오류가 아니고 최종 Console Error/Warning0이다.

격리 Save/Settings 경로로 검증했고 정상 경로·Game View 진입 동작·runInBackground를 복원했다. Bootstrap clean Edit Mode로 종료하며 OS 포커스/Game View 활성화는 하지 않았다. 사람의 실제 청취·입력·시각 QA 및 전체 퀘스트 playthrough는 미검증이다. Main13 이후·세린은 적용하지 않았다.
