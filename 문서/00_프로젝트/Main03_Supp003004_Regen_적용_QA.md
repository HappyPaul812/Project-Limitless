# Main03 태온 후속003/004 새 원본 적용과 Runtime QA

> 후반부 전체 후속감사: [008부터 남은9페이지 QA](Main03_RemainingVoice_감사_QA.md). 이 문서의003/004 포함 기존PASS5는 유지된다. 새012 원본저음량으로 현재 전체 확정재생성1이며,008 무음 보고는 격리 재생에서 미재현·원인 미확정이다. 아래 재생성0은 두 교체본 완료 시점의 이력이다.

2026-10-05 LOCAL 기준. 문서화 → 두 WAV 내용 교체 → 격리 백그라운드/실제 재생 검증 순서로 진행한다.

## 적용 전 상태·계약

- 입력: `F:/study/codex/Project-Limitless/Limitless_TTS_Regen_Main03_Taeon_Supp003_004/`의 `main03_taeon_supp_003.wav`, `main03_taeon_supp_004.wav`. 자동 Audio QA/사용자 실제 원본 청취 PASS, **Unity 반영·Runtime 확인 대기**. 이 시점에 최종 해결로 기록하지 않는다.
- 003 본문 “옵니다.”, 기존 RMS−61.60dBFS 원본 결함 → 새0.96초/RMS−19.73dBFS.004 본문 “제가 앞을 막겠습니다.\n뒤를 부탁드리겠습니다.”, 기존 다른/부분 발화 → 새4.36초/RMS−18.95dBFS. 모두 PCM24kHz/mono/16bit, companion_taeon/Gacrux, 원문·Speaker·Manifest 일치.
- 기존 Asset Path와 meta/GUID/Import/Catalog/Registry/Dialogue ID 유지.003 GUID `9e9daa2d47a707049a7c0c73b5921ae4`,004 GUID `574895e9a2b5b3b45a251673fd085670`. 파일 전체 바이트 복사만 하고 normalize/trim/재인코딩/새 TTS 생성은 하지 않는다.
-005~007 사용자 Runtime semantic PASS 유지, 다른 Voice/Player Sprite/Mixer/Story/Save/Quest/장문 UI 코드 수정 금지. 기존 사용자 Git 항목과 Save/Settings를 baseline으로 보호한다. Source 폴더와 ZIP/unrelated 변경은 Stage하지 않는다. GitHub Push 없음.

## 검증 계획

- 331 Matrix 최소 consistency·Catalog247개·ID/Speaker/Text/Manifest/Clip/GUID/Import를 대조한다. 실제 Unity GetData의 모든 PCM 샘플을 새 원본과 비교한다. Missing/중복 ID/Clip/Speaker·Manifest mismatch0 확인.
- 기존 PlayUnfocused 격리 harness로 실제 Field_01 Actor에서005→Player→006→Player→007→후속003→004→Battle을 실행한다. 사용자 Next 중 재생 정리와 끝까지 대기 후 Next를 구분하며 자동 전환/발화 절단 여부를 확인한다. 정상 수동 Next 정책을 임의로 변경하지 않는다.
- 003/004 자막·태온 Speaker/Portrait·본문/Footer 비중첩·Wrap·전체 높이 확인. Player Voice0/Portrait0·NPC→Player 정리·Player→태온 복원·Battle 이전 UI/Voice 잔류0 검사. 음량/Clip 길이/재생 상태와 원본 의미 청취 증거는 별도로 기록한다.
- METADATA_PASS, USER_LISTENING_PASS, RUNTIME 재생/PCM 동일성과 실제 사람 Runtime 의미 청취 결과를 구분한다. 원본 사용자 PASS만으로 사람 Runtime 청취 완료를 만들어내지 않는다. 미확정 Runtime 청취는 확인 대기 유지.
- 완료 후 TTS 재생성2건의 상태를 실제 근거에 맞춰 갱신하고244 NEEDS_LISTENING을 별도 보존한다. 기존 전체244음성 청취/재생성은 하지 않는다.

## 최종 결과

| ID | 자막 / Speaker | Unity Length·samples | Unity PCM RMS / Peak | 최종 판정 |
|---|---|---|---|---|
| main03_taeon_supp_003 | 옵니다. / companion_taeon·태온 | 0.960초 /23040 | −19.73 /−3.29dBFS | METADATA_PASS / USER_LISTENING_PASS / RUNTIME_LISTENING_PASS |
| main03_taeon_supp_004 | 제가 앞을 막겠습니다. / 뒤를 부탁드리겠습니다. / companion_taeon·태온 | 4.360초 /104640 | −18.95 /−0.10dBFS | METADATA_PASS / USER_LISTENING_PASS / RUNTIME_LISTENING_PASS |

- 새 WAV를 기존 두 Unity Asset에 전체 바이트 복사했다. GUID/meta/Importer/Catalog/Registry/본래 Manifest와 모든 게임 Runtime C# 그대로. 모든 Unity float PCM 샘플을 새16bit 원본과 비교해003/004 모두 최대 오차0, sample count/24kHz/mono/길이 일치.003 저음량 문제 해소,004 실제 의미 일치와 두 문장 온전함은 사용자 Runtime 청취 답변으로 확정했다.
- 무음 격리 기술 QA **83 assertions PASS/FAIL0 + PCM 측정2**. 이어서 실제 음성 QA **88 assertions PASS/FAIL0 + PCM 측정2**. 겹치는 검사는 독립171기능으로 합산하지 않는다. 실제 Actor/Quest 경로의 첫 조우5페이지/후속2페이지와 Battle 진입만 실행했으며 Main03 전체 장시간 플레이는 하지 않았다.
- 실제 재생은 사용자 요청과 안내 후 PlayUnfocused에서 진행했다. 사용자 답변 **“003·004 모두 의미 일치·끝까지 재생 PASS”** 확보. Codex가 Metadata/PCM만으로 사람 청취 PASS를 만들지 않았다. 정상 레벨·IsAudible·실제 IsPlaying 확인과 사람 의미 청취 증거를 구분한다.
- 003/004 각각 Clip 길이+.3초 기다려도 열린 Dialogue/Field_01 유지, Voice만 끝난다.004 최종 수동 Next 후에만 Battle. 자동 조기 전환/발화 절단0. 별도 기술 QA에서004 재생 중 명시적 수동 Next는 기존 skip/cleanup 정책으로 즉시 Battle을 시작함도 확인했다. 이는 사용자 입력에 의한 의도적 skip이며 자동 전환 문제가 아니다. Transition Timing/Quest/Save 코드 수정0.
-005/006/007 Mapping·WAV·이전 사람 Runtime PASS 유지. 실제 같은 Sequence에서 재생 완료 후 Next, NPC→Player Stop/Clip clear/Portrait없음·Player→태온 복원 PASS. 본문 원문/태온 Portrait·화자·Footer·Wrap·글자 가용 높이와 영역 비중첩 PASS. 실제1920×1080 두 페이지 배경 캡처를 검수했으며 다른 해상도/장문stress 전수는 이전550assertions 증거와 UI 코드 불변으로 보존했다.
- Battle 진입 후 이전 DialoguePresenter/VoicePlaybackSource/Footer/Portrait 잔류0. Compile Error0·최종 Console Error0/Warning0. 이번 재컴파일에서 기존 ExternalAssetImportEditor CS0618 2건은 별도 이력으로 보존/수정하지 않았다.
-331 Matrix/Catalog247 최소 consistency: Missing Audio0·Duplicate ID0/Clip0·Speaker mismatch0·Manifest본문 mismatch0. **확정 TTS_REGEN_REQUIRED2→0**. 기존005~007 해결 상태 유지.
- 사용자 지시대로 **NEEDS_LISTENING244 관리 범위 유지**. 이244에는 이번003/004 두 행도 원래 포함되어 있으므로 해당 행에 `RUNTIME_LISTENING_PASS`와 완료 Boolean을 추가하고 backlog membership marker를 유지했다.244 전체가 현재 미확인이라는 의미가 아니며 실제 PASS5와244를 더해249 Voice라고 계산하지 않는다. 전체 Catalog는247이다. 다른242행 의미 청취/재생성은 수행하지 않았다.
- 보호3041기존파일 중 변경은 정확히 대상 WAV2개뿐. 다른 모든 WAV(005~007/Intro/Story 다른 화자)/meta/Catalog/Mixer/Save/Settings/Scene/ProjectSettings/Packages/Player Sprite 불변. 현재 사용자419Git항목(새Source4 포함)을 Stage하지 않았다. 격리 Save/Settings/Play설정/runInBackground 복원·clean Bootstrap EditMode·포커스 전환0.

원본 SHA003 `63a12b7b026195eb49b336d17653d05e6b4e4d6bc5a0cff6313bf2ec4dbb2bbe`,004 `b77ac1bf874b4d9966e08c60570f4767dbf43797a1e456ca0b7af1ca79da43e6`. 실행 로그/PCM/형식·음량·보호 baseline/캡처는 ignored `Temp/Main03SuppRegen20261005/`에 보존한다. 새 Editor 집중 helper와 감사 도구의 증거 보고만 추가하며 게임 기능 수정0이다.

반영·집중 QA commit `1f9fa8f`(WAV2·Editor QA와meta·Python 증거 보고5파일). 직접 staged diff --check PASS. 최종 문서는 별도 Docs commit으로 기록하며 기존 사용자419Git항목을 그대로 남긴다. 전체 작업트리의 기존 whitespace는 수정하지 않는다. GitHub Push하지 않았다.
