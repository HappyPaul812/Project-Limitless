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
