## 2026-10-07 Main04 후반 음성 5개 최종 반영 완료

현재 Main04 Late Sequence의 재생성 대기 **5→0**. 미엘009/010/011·태온006/007 모두 **REGENERATED / USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED**. 사용자 전체 원본 청취PASS와 이번 Runtime 기술 검증을 구분한다. 실제 Actor 9페이지·7Voice 전체재생/Next/Portrait/화자/본문, 새5개 PCM 오차0, 완료→Main05/완료전후 Bootstrap Continue, 동일프레임 Next2/단일Source, Player·지문 무음 PASS. **148PASS/FAIL0**, Compile Error0·최종Console Error0/Warning0·Field Runtime Missing Script0. 원본·meta·GUID·Catalog·대사 변경0; WAV 정확히5만 교체. 기존 NEEDS_LISTENING244 marker/Queue219 및 Main17 TTS_PENDING14 유지·일괄PASS승격0. [최종 QA](Main04_Late5_Final_QA.md)와 [현재5개 Mapping](Main04_Late5_Source_Mapping.csv)이 우선하며 아래 이전 Wrong/Suspect/대기5 표기는 역사 기록이다. 음성commit `0e7809a`; Push0.

## 2026-10-06 Main04 대기 지문 수정 및 전투 후 Voice 5건 재생성 전달

대기 문장 “상황을 살피고 있습니다.”는 `MainQuest04FieldFlow.CreateStoryActor`의 공통 NpcController 설정으로, Dialogue ID·Sequence index·Manifest·Voice entry가 없다. 태온 기본 상호작용에서 이름/Portrait가 붙던 상태 설명을 **`<지문> 상황을 살피고 있습니다.`**로 수정했다. DialogueLine의 명시적 prefix 규칙으로 Character Speaker·Portrait·Voice를 제거하며, 잘못된 Voice ID가 전달돼도 Resolve하지 않는다. Player와 기존 Character 대사는 유지한다. 331 authored Dialogue에는 이 대기 설정이 원래 포함되지 않아 개수는 유지한다.

Main01~16 Matrix 및 Flow의 행위 표현 자동 검색 후보 **6행(고유본문5개)** 중 확정 지문은 **고유본문1개 / NPC설정2곳**이다. 나머지4행은 실제 발화 문맥으로 유지했다. [정식 작성 규칙](../03_스토리/Story_대사_지문_작성_규칙.md)과 [후보 감사](Main01_16_Direction_Candidates.csv)에 근거를 기록했다.

미엘 `main04_miel_supp_009` / AfterBattleConversation index2의 정본은 “저도 볼 겁니다.\n이번에는 순서대로요.”다. Runtime·Manifest·정본 부록 일치. 사용자 Runtime 및 직접 원본 WAV 청취 “원본도 자막과 다름”으로 **WRONG_AUDIO_CONTENT 1건**을 확정했다. 추가 청취 답변 “순서가 뒤죽박죽이다”에 따라 인접 태온006·미엘010·태온007·미엘011 **4건은 SEMANTIC_INTEGRITY_SUSPECT**로 등록했다. 정확한 오발화 문장/개별 Wrong·Partial은 미확정이며 metadata로 의미 PASS를 추정하지 않는다. 실제 Clip/PCM 정합으로 Mapping·Cache 문제 재현0. **TTS_REGEN_REQUIRED 0→5**, 단일5행(Miel3/Taeon2) 전달. TTS 생성·WAV 수정·기존 파일 재배열0, **NEEDS_LISTENING244 유지**.

백그라운드 실제 대기 표시 및 Character→지문→Player 전환, 첫 조우10→전투 전3→Story Battle/QA 승리/실제 결과버튼 복귀→전투 후9 전체 **335개 기술 검사 PASS / FAIL0**. 전투 후7Voice 전체 원본PCM오차0/Player Voice·Portrait0/Next·Portrait 전환·Voice cleanup·UI Wrap/동적높이/본문·Footer 비중첩 PASS. 실제 지문 스크린샷도 prefix 전체·이름/Portrait 없음 확인. 기술 PASS와 위5건 의미 문제를 구분한다. 초기 QA의 단일 Show 창을 Next로 닫는 잘못된 기대는 기존 Esc/Hide 정책에 맞게 수정해 전체 재실행했고 초기 실패는 최종 PASS 수에 포함하지 않았다.

보호 대상 기존 3,046파일 중 변경은 Presenter 및 Main04 Flow 코드2개뿐이다. 새 Editor QA와 meta 추가, Main03/정상Main04첫9/후속004·007·008/태온005/전체WAV·meta·Catalog·Registry·사용자Save·설정 변경0. Compile Error0/Console Error0, 기존 CS0618 경고2개는 이력으로 구분한다. 종료 clean Bootstrap Edit Mode/격리Save 해제/is_focused=false/포커스전환0. 전략전투 조작 검증은 생략했다.

[13페이지 인접 순서](Main04_Direction_Miel009_Adjacent_Sequence.csv) · [전투 후9페이지 WAV/PCM 감사](Main04_Direction_Miel009_Sequence_Audit.csv) · [5행 TTS Handoff](Main04_Direction_Miel009_TTS_Handoff.csv) · [실제 Runtime 로그](Main04_Direction_Miel009_Runtime_Results.txt). 다음은5개 정식본문 새원본 제작 및 청취 후 별도 Unity 교체/연속검증이다. 선행 관련 commit `a5460c8f9843c9a800d628915e1da9872ed67285`, 이번commit은 최종보고 참조. GitHub Push 없음. 아래는 준비 당시 이력이다.

# Main04 지문·미엘009 및 인접 Sequence QA

## 2026-10-06 Main04 지문 및 미엘009 원본 불일치 수정 계획

“상황을 살피고 있습니다.”는 MainQuest04FieldFlow.CreateStoryActor의 NpcController 공통 대기 설정이다. Dialogue ID·Sequence index·Manifest·Voice entry가 없으며 태온/미엘 두 Actor가 같은 문장을 공유한다. InteractionSystem→VillageNpcRole의 기본 대화 표시에서 태온 이름과 Portrait가 붙었다. Story 설계 Main04의 현장 관찰 문맥과 Flow의 공통 대기 분기를 근거로 상태 설명 지문으로 분류한다. 정식 표기는 `<지문> 상황을 살피고 있습니다.`이며 공통 DialogueLine 규칙으로 Character Speaker/Portrait/Voice를 제거한다. 문장 형태만으로 다른 대사를 자동 전환하지 않는다.

미엘 `main04_miel_supp_009`는 AfterBattleConversation index2이며 Runtime·Manifest·정본 부록의 “저도 볼 겁니다.\n이번에는 순서대로요.”가 일치한다. 사용자 Runtime 불일치와 이번 직접 WAV 청취 “원본도 자막과 다름”으로 WRONG_AUDIO_CONTENT를 확정했다. 틀린 실제 문장의 정확한 전사는 미확정이다. WAV/Mapping 재배열 없이 TTS Handoff를 작성하며 TTS 생성은 하지 않는다. 전투 후9페이지 전체와 대기 지문을 백그라운드 QA로 검사한다. 기존 정상 Main03·Main04첫9개·후속004/007/008 및 태온005를 보호한다. NEEDS_LISTENING244 유지. 선행 관련commit `a5460c8f9843c9a800d628915e1da9872ed67285`.


## 범위와 위치

대기 지문은 독립적인 공통 NPC 표시이며 AfterBattleConversation의 전후에 자동 삽입되는 페이지가 아니다. FirstConversation 마지막 index9 → EncounterConversation3 → Story Battle → AfterBattleConversation9,총13페이지(Voice10/Player3)의 인접 목록을 추출했다.실제 QA는 첫대화10개도 포함한22페이지를 검사한다.대기 태온은 VillageNpcRole.Interact 기본 표시로 접근하며,미엘의 설정은 같지만 MainQuest04MielActor가 상호작용을 우선 처리한다.대기 문장은 Manifest/Voice mapping/Dialogue ID 없음(index N/A),Portrait는기존 stableID companion_taeon에서 수정후NONE.331 Matrix authored Dialogue에 이 대기 설정은 포함되어있지 않아331수는 유지한다.
