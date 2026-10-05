# Main03 후속 Voice·장문 Dialogue Layout 적용 계획과 QA

2026-10-05 LOCAL 기준. 문서화 → 최소 구현 → 백그라운드 검증. 사용자 이번 요청으로005/006/007 실제 Runtime 의미 청취 PASS를 확정했고 기존 TTS_REGEN_REQUIRED3→0으로 정리했다. 세 WAV/GUID/Catalog는 다시 변경하지 않는다. 나머지244 NEEDS_LISTENING은 유지한다.

## 후속 Voice 조사

실제 `MainQuest03TaeonActor.TryInteract`의 `win_taeon_encounter` 목표에서 첫 조우 이후2페이지가 나온다. 첫 만남005/Player/006/Player/007 완료 후 다음 상호작용이다.

| Sequence index | ID | Speaker | Runtime/Manifest 본문 | 현재 근거 |
|---|---|---|---|---|
| 0 | main03_taeon_supp_003 | companion_taeon / 태온 / Gacrux | 옵니다. | Source·Catalog·GUID 정상, Loaded, 1.10초, RMS−61.60dBFS/Peak−39.04dBFS |
| 1 | main03_taeon_supp_004 | companion_taeon / 태온 / Gacrux | 제가 앞을 막겠습니다. / 뒤를 부탁드리겠습니다. | Source·Catalog·GUID 정상, Loaded, 1.65초, RMS−25.91dBFS/Peak−6.59dBFS |

원래 요청은1개였으나 사용자 추가 확인으로 **두 줄 모두 문제**다. 문제 수를1개에 억지로 맞추지 않는다. 두 행은 보충 Manifest에 있으며 Unity WAV와 원본 보충 WAV를 대조한다. Entry/파일 부재나 ID/Speaker 조회 실패가 아니라 실제 재생/원본 내용/Next/Scene 타이밍을 더 검사해야 한다. 003은 매우 낮은 원본 음량이 확인됐지만 원본 음성을 임의 증폭·재생성하지 않는다. 004 의미는 사용자 원본 청취에서 다른/부분 문장으로 확인됐다. 실제 Actor→Presenter→Battle 흐름에서 재생 완료 전 자동 전환 여부와 수동 Next 정리를 확인한다. 확정된 원본 결함만 최소 TTS 재생성 목록으로 넘긴다.

## 장문 UI 원인과 수정 계약

현재는 TMP가 아니라 `UnityEngine.UI.Text`다. 글꼴·CanvasScaler1280×720/ScaleWithScreenSize는 유지하고 TMP Migration/Scene 재생성은 하지 않는다. 기존 fixed anchor height는 Canvas의23%(720기준165.6)이며, 화자/본문/Footer를 한 Text에 넣고 verticalOverflow=Overflow를 사용한다. top62/bottom20 여백으로 글자 가용 높이가 작아 장문에서 Panel 밖/Portrait/Footer 겹침이 가능하다.

- 기존 Panel과 Portrait 구조를 유지하고 Speaker/Body/Continue Footer Text 영역을 분리한다. Body 기본28, 작은 범위24~28만 허용, 무제한 AutoSize 금지. Word Wrap ON.
- 현재 기본 높이를 최소로 사용하고 PreferredHeight로 확장한다. 상한 Canvas 높이40%. Portrait150과 Footer에 필요한 높이도 보장한다. NPC 왼쪽 여백190, Player는28로 전체 가용 폭 복원.
- Speaker/Body/Footer의 간격과 Portrait 하단 공간을 한 중앙 측정 경로에서 계산한다. 선택창도 Footer/Body 영역 분리하고 기존 버튼 동작은 보존한다. Text 축약/Story/Voice/Mixer/Quest/Save 구조 변경 없음.
- SHORT1줄/MEDIUM3줄/LONG6줄, Player·태온·미엘/폴과 현재Story 최장10개를 1920×1080/1600×900/1280×720에서 확인한다. 실제 Text generator의 line/PreferredHeight/Panel·영역bounds와 glyph overflow를 검사한다. 배경 screenshot을 생성·검수하되 OS포커스는 전환하지 않는다.

## 검증/보호

현재 clean Bootstrap EditMode다. 기존 공용 격리 Save/Settings/PlayUnfocused 경로를 재사용한다. 원본3041파일·Git415항목을 baseline으로 기록했다. Voice Loader/GetData/Source·UI·Portrait·Next·Battle 전환을 집중 검사하고 기존 전체331개는 최소 consistency를 대조한다. 005~007 재교체/다른 Voice 수정/새 TTS/Player Portrait 생성/Audio Mixer 변경/사용자 Save 변경/GitHub Push는 하지 않는다.

## 확정 결과

005/006/007은 사용자 실제 Runtime 의미 청취 PASS로 기존 재생성 필요3→0을 해결했다. 새 전투 전 문제003/004는 기존244 NEEDS_LISTENING의 부분집합이며 추가 재생성 대상2다. 따라서 현재 TTS_REGEN_REQUIRED2, NEEDS_LISTENING244를 유지한다. 004 원본은 사용자 추가 답변으로 **다른 문장 또는 일부만 말함**이 확인됐지만 실제 들린 문구는 제공되지 않았다. 원문을 추정하거나 잘못된 WAV에 자막을 맞추지 않았다.

| ID / 출력 파일 | 정확한 본문 | 분류 / 남은 조치 |
|---|---|---|
| main03_taeon_supp_003.wav | 옵니다. | G. OTHER / SOURCE_LOW_LEVEL_AUDIO: 원본 RMS−61.60/Peak−39.04dBFS. 정상 레벨의 온전한 원본 필요; 의미 청취 미확정 |
| main03_taeon_supp_004.wav | 제가 앞을 막겠습니다.\n뒤를 부탁드리겠습니다. | G. OTHER / WRONG_CLIP: 사용자 원본 청취에서 다른/부분 문장. 두 문장 모두 포함한 원본 필요 |

두 항목 모두 Main03/Field_01, companion_taeon/태온, Voice Registry Gacrux다. 보충 Manifest `Limitless_TTS_Output_Missing_Main01_05_08_12_v2/dialogue_manifest_missing_main01_05_08_12_v2.csv`와 Catalog/WAV/meta/Speaker 조회는 정상이며 A~F에 해당하지 않는다. 정확한 Manifest 경로는 Matrix에 기록된 정본을 따른다. 첫 조우 005/Player/006/Player/007 → win_taeon_encounter에서 다시 TryInteract →003(index0)→004(index1)→완료 callback에서 Battle 순서다. 새 TTS 생성/음량 보정/추측 Mapping 변경은 하지 않았으며 [전달 CSV](Story_Dialogue_TTS_REGEN_REQUIRED.csv)에 두 항목을 기록했다.

Runtime 실제 Actor/Quest/Presenter 경로에서003/004 Clip.name·IsPlaying·GetData와 원본 음량을 확인했다. 각 Clip 길이+.3초를 기다려도 Field_01/열린 Dialogue를 유지하고 음성만 정상 종료한다. 최종 수동 Next 후에만 Battle로 이동하며 이전 Dialogue/Voice/Footer/Portrait 객체가 남지 않는다. 자동 조기 Battle 전환이 원인이 아니므로 Transition/Save/Quest 코드는 수정하지 않았다. 수동 Next가 음성을 정리하는 기존 정책을 유지한다.

UI는 공통 Presenter만 수정했다. Speaker/Body/Footer 분리·Word Wrap·내용 기반 확장·기존 최소23%/최대40%·본문28→최소24를 적용했다. NPC Portrait150/왼쪽190, Player Portrait없음/왼쪽28을 유지했다. 상한/최소 글자 크기에서도 넘는 본문은 Mask 안에서 스크롤하여 전체 원문을 읽는다(↑↓/패드 방향/마우스 휠·드래그). 화자/안내문/선택 버튼은 고정 영역에 남는다. 기존 Legacy Text/Font/CanvasScaler 유지, Story 원문 축약0, Player Portrait 생성0이다.

foreground 조작 없이 격리 PlayUnfocused와 배경 캡처를 사용했다. 최종 체크 수·해상도별 결과·보호 비교·커밋은 아래 완료 기록에 추가한다.

## 완료 검증

- 격리 PlayUnfocused **550 assertions PASS / FAIL0 + PCM 측정2건**(로그 count552). 실제 첫 조우5페이지→후속2페이지→Battle, Player Voice0/Portrait0·Named Portrait 정상·종료 객체 정리를 통과했다. 기존 전체Story2574검사와 이번 집중 검사는 다른 범위이며 합산하지 않는다.
- 1920×1080/1600×900/1280×720 각각22 Layout: Player/태온/미엘×1/3/6/24개행 Fixture12 + 현재 Story 최장10. NPC Wrap에 따라 실제 줄 수는 달라지며 CSV 측정값을 따른다. 각 해상도 선택창 본문/버튼 비중첩도 PASS.
- 화면 경계·Speaker/Body/Footer/Portrait 영역 분리·본문 전체 PreferredHeight·Word Wrap·최대 Canvas40%·font24~28 PASS. 최장10개(58~83문자)는 모두2줄/font28로 스크롤 없이 표시한다. PreferredHeight 약63논리px, Panel165.6(Player)/212(NPC); 해상도 배율에 따라 소수 측정 차이가 있다.
- 24개행 stress9조합은 상한288논리px/본문24·Mask/ScrollRect 활성·마지막줄 접근 PASS. 전체 원문을 보존했다. 이는 프로그램으로 ScrollRect 이동한 검사이며 실제 키보드/패드/마우스 입력은 미검증이다.
- 배경 Screenshot18개(3해상도×3화자×LONG/stress), 문자수·줄수·PreferredHeight·PanelHeight CSV/실행 로그는 `Temp/DialogueFollowup20261005/`에 보존한다. 대표 Player/태온/미엘·세 해상도/스크롤 캡처를 시각 검수했다. Bootstrap 배경 위 Fixture이며 Story/Scene 파일 변경0이다.
- Compile Error0·최종 Console Error0/Warning0. 재컴파일의 기존 ExternalAssetImportEditor CS0618 2건/MCP 연결 경고 이력은 범위 밖으로 보존했다. clean Bootstrap EditMode·격리 Save/Settings·Play옵션/runInBackground·렌더 해상도 복원·포커스 전환0.
- 보호3041기존파일 중 변경은 공통 Presenter와 기존 Editor QA2개뿐. 새 집중 QA/helper meta 추가. WAV/meta/Catalog/Mixer/Save/Settings/Scene/Packages/사용자 원본3041중 나머지3038개 동일. 기존415Git항목을 Stage하지 않는다.
- 331 Matrix 최소 consistency: Catalog247/247·Missing0/중복0/Manifest본문 mismatch0·Player42 정책 유지. 새003/004 원본 결함2와005~007 사용자 Runtime PASS3,244 NEEDS_LISTENING을 분리 기록했다. 전체247음성을 새로 듣거나 Story전수 Runtime를 재실행하지 않았다.

### Stable 참조

003 GUID `9e9daa2d47a707049a7c0c73b5921ae4`,004 GUID `574895e9a2b5b3b45a251673fd085670`. PCM24kHz/mono/16bit와 원본 SHA가 같으며 meta/import 유지다. 정확한 파일경로·SHA·Manifest·Speaker는 Matrix에 보존했다. `StoryVoiceCatalog`와 Registry는 수정하지 않았다.

### Git·최종 상태

UI/QA 구현 commit `284cbf2`(Presenter·기존 QA2개·새 QA와meta 총5파일). QA 종료 시 Console Error0/Warning0이었다. 이후 주석/인코딩 정리 후 마지막 재컴파일은 **Error0/기존 CS0618 Warning2**이며 경고를 지워0으로 보고하지 않는다. 사용자/ThirdParty 파일은 수정하지 않았다. 직접 Stage한 diff --check PASS; 전체 작업트리의 기존 사용자 whitespace는 별도로 기록하고 보존한다. GitHub Push 없음.
