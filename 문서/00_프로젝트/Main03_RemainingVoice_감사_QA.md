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

# Main03 태온 후반부 전체 Voice 감사 계획·QA

2026-10-05 LOCAL. 문서화 → 최소 수정/집중 QA 구현 → 실제 격리 Runtime 검증.005/006/007/003/004의 사용자 Runtime PASS와 WAV/meta/구조를 보호하고 새 TTS API/정상 WAV 수정/GitHub Push는 하지 않는다.

## 실제 순서와 요청 차이

“역시 이상합니다.”는 `AfterBattleConversation` index0, ID `main03_taeon_supp_008`이다. 실제 LOCAL은 첫 만남005/Player/006/Player/007 → 전투 전003/004 → 첫 Battle → 승리/Field_01 복귀 →008부터 후속7페이지 → 마지막 단서001/002 →Main03 종료다.008 이후 첫 Battle을 시작하는 Sequence는 없다. 사용자 요청의 전후 순서와 차이를 보고하며 Story 구조는 바꾸지 않는다. 요청 의도인 남은 대사 전체 감사는 후속7페이지와 마지막2페이지를 모두 포함한다.

## 적용 전 조사

- 후속7페이지는008/Player/009/Player/010/011/012. Player2개는 Voice/Portrait 없음이 정상. 마지막 단서001/002도 포함하여 남은 총9페이지/Voice7개를 한 번에 검사한다. 보호 회귀를 포함한 Main03 전체 작성 페이지는16/Voice12/Player4다.
-008: companion_taeon/태온, 본문 “역시 이상합니다.”, 보충 Manifest/Catalog/WAV 존재, GUID `16a05ac1b6d319545a8f9113e2bd9aba`,2.25초/RMS−21.63dBFS. 단순 파일 없음/저음량으로 단정할 근거는 없으므로 실제 Battle 복귀 뒤 Resolve/Play/Audibility를 추적한다.
-012 원본1.45초/RMS−51.90/Peak−32.62dBFS는 비정상 저레벨 후보다.010은−31.83dBFS로 낮지만 단일 RMS만으로 사실상 무음이라고 자동 판정하지 않는다.009 2.76초/011 15.24초는 본문 대비 길이 의심이 있어 의미 청취 대상으로 기록한다. 길이만으로 WRONG_CLIP/잘림을 확정하지 않는다.
- 모든 대상 ID/Text/Speaker/Manifest/Catalog/WAV/GUID/Import·PCM·중복을 조사하고 실제 Sequence표/전체 최소 TTS 전달목록을 만든다. 한 줄마다 멈추거나 자막을 WAV에 맞추지 않는다.

## Runtime 계획

기존 격리 Save/Settings/PlayUnfocused harness 재사용. 실제 Actor의 첫 만남→003/004→Battle을 실행하고 QA 전용 승리 종료 호출/정식 결과 복귀 경로로 후속7페이지를 실행한다. 이 승리는 전투 조작/전략 플레이의 검증이 아니라 실제 종료·Quest·Scene 연결 검증이다. 마지막 단서 Location의 실제 TryReach 경로까지 확인한다.

각 행의 Loaded/Clip/IsPlaying/Audibility/PCM/RMS/자막/Portrait/UI bounds와 Next/정리·최종 Scene/Quest를 기록한다. 긴 Voice의 완료 대기와 빠른 Next skip 정책을 구분한다. 짧은 대사 생략 규칙/볼륨·Mute 변경/Scene 타이밍 원인을 감사한다. Metadata/RuntimePlayback/사람 의미 청취를 분리하고 확정 못한 의미는 USER_LISTENING_REQUIRED로 남긴다. 새 원본 문제가 있으면 모든 항목을 한 전달목록으로 정리한다.

검증 결과는 아래에 추가한다.

## 최종 결과·한 번에 전달할 범위

- 후속7페이지+마지막 단서2페이지 **총9페이지/Voice7/Player2** 전수 검사. 첫 만남/전투 전 보호 회귀를 합치면 Main03 전체16페이지/Voice12/Player4다. 실제 전체 실행 순서는 앞의 LOCAL 흐름 그대로이며008 이후에는 또 다른 첫 Battle이 없다.
- [실행 순서·참조·결과 CSV](Main03_Taeon_Remaining_Sequence_Audit.csv)에 Order/Sequence index/ID/Speaker/전체본문/VoiceExpected/Manifest/Catalog/WAV/GUID/Loaded/Played/길이/RMS/Peak/Result/조치를 기록했다. Stable Player Dialogue ID는 원래 없으므로 NONE을 기록했으며 새 ID를 만들지 않았다.
-008: Manifest/Catalog/Clip/GUID/24kMono Import 정상, RMS−21.63/Peak−4.07dBFS,2.25초. 실제 첫 Battle 결과 복귀 뒤 올바른 Clip을 Resolve→Play했고 PCM 전체54000샘플은 원본과 오차0이다. Mute OFF/Master0dB/Voice−2.85dB/Listener volume1·pause false/IsAudible true. 문자수/짧은 대사 skip 규칙 없음. **사용자가 보고한 무음은 이 격리 조건에서 재현되지 않아 근본 원인을 확정하지 못했다.** 실제 WAV 내용/사용자 당시 환경을 확인하기 전 G. PLAYBACK_LOGIC_BUG로 단정하거나 무관한 Playback/Mapping을 수정하지 않았다.
- 후반부7개 WAV 모두 Loaded/Played/GetData/올바른 ClipName·태온 Portrait/자막 PASS. **012만 F. LOW_OR_SILENT_AUDIO 확정**, RMS−51.90/Peak−32.62dBFS,1.45초. 정상 레벨의 완전한 원본이 필요하다.010 RMS−31.83은 낮지만 사실상 무음 판정 없이 청취 검토.009/011의 본문 대비 길이는 의심 근거만 기록하며 Wrong/Truncated를 꾸며내지 않았다.
- 사용자 이번 답변은 **“이번 재생을 듣지 못함”**이다. 따라서7개 모두 `USER_LISTENING_REQUIRED`, `METADATA_PASS`/`RUNTIME_PLAYBACK_PASS`와 의미PASS를 분리한다. 기존005/006/007/003/004 의미PASS는 유지하며 이번미청취를 이전PASS 취소로 해석하지 않는다.
- [한 번에 넘기는 Handoff7행](Main03_Taeon_Remaining_TTS_Handoff.csv): 확정 재생성1(012), 재생성 미확정/원본 의미 청취6(008/009/010/011/001/002). 각 ID/전체Runtime·Manifest본문/기존WAV/새Outputfilename/Gacrux VoiceDesign/사유를 함께 기록했다. 청취6건을 자동 TTS 생성 대상으로 삼지 말고 기존 원본이 맞으면 재사용한다. **확정 TTS_REGEN_REQUIRED0→1**. 다른 WAV/확정PASS5개 재생성0, TTS API 호출0.

| 실제 Order | ID | Speaker | Runtime Text | Played / 결과·조치 |
|---|---|---|---|---|
| 1 / index0 | main03_taeon_supp_008 | 태온 | 역시 이상합니다. | YES / 의미청취 필요·기존 무음 미재현 |
| 2 / index1 | NONE | Player | 방금 몬스터들도 같은 방향을 피했습니까? | NO 정상 / PLAYER_SILENT_EXPECTED |
| 3 / index2 | main03_taeon_supp_009 | 태온 | 네. 싸우는 동안에도 몇 번이나 그쪽으로 움직이지 않으려고 했어요. | YES / 의미청취 필요 |
| 4 / index3 | NONE | Player | 제가 본 흔적도 그 반대쪽에서 시작됐습니다. | NO 정상 / PLAYER_SILENT_EXPECTED |
| 5 / index4 | main03_taeon_supp_010 | 태온 | 그렇다면 우연은 아닌 것 같습니다. | YES / 낮은 음량 검토·의미청취 필요 |
| 6 / index5 | main03_taeon_supp_011 | 태온 | 저도 저쪽을 확인하려던 참이었습니다. | YES /15.24초 의미청취 필요 |
| 7 / index6 | main03_taeon_supp_012 | 태온 | 목적이 같다면 잠시 함께 가시죠. 혼자 움직이는 것보다는 안전할 겁니다. | YES / ZERO_OR_LOW_VOLUME·원본 재생성 필요 |
| 8 / FinalClue index0 | main03_taeon_supp_001 | 태온 | 바닥에 간단히 치료한 흔적과 떨어진 붕대가 남아 있습니다. | YES / 의미청취 필요 |
| 9 / FinalClue index1 | main03_taeon_supp_002 | 태온 | 누군가 먼저 이곳을 지나간 것 같습니다. | YES / 의미청취 필요 |

## 집계와 Runtime 증거

- 대상 Voice7: 비정상 저음량1/나머지6는 기술 재생 정상이며 의미PASS 미확정. Player Silent2 정상. Mapping 수정0/Missing WAV0/확정 Wrong WAV0(의미미확인7을 정상내용으로 확정한 것 아님)/Playback Logic Bug 재현0/자동 Transition Cut0. USER_LISTENING_REQUIRED7, 확정 TTS_REGEN_REQUIRED1.
- 최종 무음 기술QA **202 assertions PASS/FAIL0 + PCM 측정9**. 별도 실제 음성 **214 assertions PASS/FAIL0 + PCM 측정9**. 앞뒤 겹치는 체크를 독립416기능으로 합산하지 않는다. 기존 전체331Runtime 재감사가 아니라 실제 Main03 전체16페이지 집중 실행이다.
-005→Player→006→Player→007→003→004→실제Battle Scene→QA전용 EndBattle 승리 처리→정식 결과 버튼→Field_01 복귀→008/Player/009/Player/010/011/012→Location TryReach001/002→Main03완료 연결 PASS. 전투 입력/전략은 생략했으며 실제 종료 callback/결과 복귀를 검증했다.012 이후 또 전투가 시작된다고 보고하지 않는다.
- NPC→Player 이전Voice/Portrait 정리·Player Voice0/Portrait0·Player→태온 복원·Taeon→Taeon·짧은→긴 대사·끝까지 기다린Next·빠른Next Resolve/Play·최종cleanup PASS. 빠른 수동Next가 이전 음성을 skip하는 기존정책을 유지했다. 정상 재생 중 자동Battle절단 없음.
- 후속 모든9페이지 자막/화자/본문/Footer/Portrait 비중첩·Wrap/font24~28/Panel최대40%/전체PreferredHeight PASS. 대표008 배경 캡처 검수, UI 코드 변경0. 일곱 Voice 모든 PCM 샘플은 기존 WAV와 최대 오차0. 로그/PCM/캡처/3040파일 보호baseline은 `Temp/Main03Remaining20261005/`에 보존한다.
- 초기 QA의 새 helper deprecated/불필요 dead-code 경고는 helper에서 제거했다. 실제 재생 때 선택슬롯 없음 경고2는 격리 QA Seed 때문이었고 이후 AuditSaveDirectory 확인/격리slot1 선택으로 최종검증에서 해소했다. 사용자 저장에 쓰지 않았다. Compile Error0·최종 Console Error0/Warning0, 기존 ExternalAssetImportEditor CS0618 2건 재컴파일 이력 보존.
- 격리slot1을 추가한 중간 QA에서 Session의 Scene ID Seed 누락으로 저장 검증 오류1건이 발생했다. QA helper에 Field_01 위치 Seed를 추가하고 다시 실행하여 해소했다. 이 오류는 별도로 기록하며 사용자 Save/게임 Save코드를 수정하거나 원복하지 않았다.
- 보호3040기존파일 해시 변경0: 모든 WAV/meta/Catalog/Mixer/게임 C#/StoryFlow/UI/Quest/Save/Settings/Scene/Packages/PlayerSprite 불변. 사용자419Git항목 보존, 새 Editor QA/helper meta와 감사증거/문서만 변경. clean Bootstrap EditMode·격리 저장·설정/Play옵션복원·포커스 전환0.
- 기존 NEEDS_LISTENING244 관리범위 유지, 이번7개는 그부분집합이며 기존PASS5를 보호한다. Source 전체331 Matrix 최소 consistency247Catalog/중복0/Missing0/Manifest본문 mismatch0. 알려진012 저레벨과 사람청취 미확인7을 따로 기록한다.

감사 helper/증거 보고 commit `d743227`(Editor QA/meta/Python3파일). 게임/WAV 수정0. 직접 staged diff --check PASS, 전체 작업트리 기존 사용자 whitespace 보존. 최종 문서는 별도 Docs commit으로 기록한다. GitHub Push 없음.
