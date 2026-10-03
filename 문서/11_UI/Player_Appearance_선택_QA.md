# Player Appearance 선택·Save QA (2026-10-03)

## 결과와 범위

Catalog 50종을 모두 유지한다. **Selection Ready 35종, blocked 15종**이다. 준비된 외형은 남17·여18이며 Vision5, Hearing10, Intellectual10, EmotionalScar10이다. Mobility10종은 전부 보류다. 기본 Male/Female+Path Variant도 별도 선택지로 유지한다. 전체50종이 선택 가능하다는 뜻은 아니다.

PNG50개·800 Sprite의 원본 해시, Catalog Stable ID와 GUID를 유지한다. 그림과 Sprite meta는 수정하지 않는다. Cell은 위→아래 Down/Left/Right/Up, 방향마다4프레임으로 정적 검수했다. 동적 발 미끄러짐·프레임 흔들림·크기·장치 표현의 최종 육안 평가는 미완료다.

## Alpha 경계 경고 10종

모두 `appearance.external.v1.vision.` 접두사다. 기존157 Cell 접촉을 재현했다. **각 방향·frame·상/하/좌/우 접촉과 Alpha 강도는 [전수 Frame Bounds JSON](Player_Appearance_Frame_Bounds.json)의 cells/edges에 기록**한다. Cell 번호0~3=Down,4~7=Left,8~11=Right,12~15=Up이다. 접촉은 각 시트 전체에서 네 면에 존재하지만 개별 셀의 면은 서로 다르다.

| ID 끝부분 | 경고 Cell | 판정 | 확대 이미지 근거 |
| --- | ---: | --- | --- |
| fighter.female | 13 | Ready | 검·망토 끝 접촉과 낮은 Alpha 잔류. 본체와 방향은 이어지며 명백한 절단/인접 행 조각 없음 |
| fighter.male | 16 | blocked | Left/Up 행 위쪽에 이전 행의 발·무기 조각이 분리되어 보임. 경계 오염/잘림 의심 |
| guardian.female | 16 | Ready | 머리·망토·방패 윤곽 접촉과 잔류 Alpha. 다른 행의 본체 조각은 확인되지 않음 |
| guardian.male | 16 | blocked | Right/Up 행 경계에 인접 행 머리·발 조각이 따로 보임 |
| healer.female | 16 | Ready | 모든 면의 낮은 Alpha 잔류. 경계에 Alpha128 이상 픽셀0, 본체·지팡이 정상 윤곽 |
| healer.male | 16 | Ready | 지팡이/망토 끝의 좌우 접촉. 상하 인접 행 본체 조각은 보이지 않음 |
| mage.female | 16 | blocked | Right/Up 경계 위쪽에 발/장치 조각이 분리됨 |
| mage.male | 16 | Ready | 모든 면의 낮은 Alpha 잔류. 경계에 Alpha128 이상 픽셀0, 본체·지팡이 정상 윤곽 |
| marksman.female | 16 | blocked | Down/Right/Up 경계에 발·활/화살 장치 조각. 행 오염/잘림 의심 |
| marksman.male | 16 | blocked | Right/Up 행 위쪽에 분리된 발/장치 조각 |

단순히 경계에 닿았다는 이유로10종 전체를 차단하지 않았다. Alpha 수치는 확대 검수의 보조 자료이며 단독 합격 기준이 아니다. 실제 원본 수정이나 재슬라이스는 하지 않았다.

## 추가 blocked Mobility10종

모두 `appearance.external.v1.mobility.{job}.{gender}`다. 각 행의 전체4프레임을 비교했고 원본을 보존한다. 세부 사유는 Inventory와 Catalog의 reviewReason에도 기록한다.

| 직업 테마 (남/여 각각) | 보류 이유 |
| --- | --- |
| Fighter | 방향 행 혼재·분리된 본체/장치. Female Right 두 번째 프레임(9)은 흰 머리의 다른 캐릭터로 변함 |
| Guardian | 본체·방패/장치가 셀 안에서 분리됨. Male 머리 외곽 Alpha 손상 의심과 방향 일관성 부족 |
| Healer | 첫 열 본체가 수직으로 끊기고 다른 열에 지팡이/장치 조각이 따로 나타남 |
| Mage | 대부분 프레임이 좁은 세로 조각으로 분리됨. 네 방향 캐릭터로 사용 불가 |
| Marksman | Female 분리된 장치 조각, Male Left/Right 행이 왼쪽을 향한 자세를 반복하여 방향 오류 의심 |

## UI·Runtime·Save

- CharacterCreation 안에 성별 필터, 전체/5테마 필터, 이전/다음 외형 카드 탐색, 큰 Down Idle Preview, `✓ 선택됨`·이름·목록 번호를 추가했다. 50개를 작은 카드로 동시에 늘어놓지 않는다. 준비된 신규 외형이 없는 테마에는 기존 기본 외형만 보인다.
- 키보드 Tab/Shift+Tab과 기존 InputSystemUIInputModule의 방향/Submit 경로를 유지한다. 버튼12개의 명시적 Navigation과 Tab·EventSystem 이동을 검사한다. 실제 키보드/컨트롤러 기기 조작은 별도 수동 확인이다.
- 필터/외형 초안은 Session이나 디스크를 바꾸지 않는다. 이름 검증을 통과한 다음에서 성별·이름·Stable ID를 Session에 확정하고, FinalConfirmation의 기존 첫 저장에서 영구 확정한다. 다시 돌아오면 확정된 초안을 복원한다.
- Session.SelectedAppearanceId → GameSaveData.AppearanceId → Continue → Catalog.Resolve → 실제 Player Sprite/Animator 순서다. Version1에 선택적 문자열만 추가하며 기존 필드는 유지한다. ID가 누락/invalid/blocked이거나 Sprite·Clip 참조가 부족하면 기존 성별·Path Variant로 fallback한다. invalid ID만으로 정상 슬롯을 거부하지 않는다.
- 기존 성별 Controller·PlayerSpriteAnimator의 상태와 이동 처리를 그대로 사용한다. Ready35종의 Idle/Walk8개씩 총280 Clip을 Catalog subasset으로 만들고 Runtime AnimatorOverrideController로 Sprite Clip만 바꾼다. 새 Controller Asset50개는 만들지 않는다.
- Path/Job/최종 확인 Preview에서도 선택 외형을 유지한다. 공식 PathSymbol과 Story/Quest/전투 특성은 실제 Path를 따른다. Hearing Appearance+Vision Path를 실제 생성해 Story가 Hearing으로 분류되지 않음을 확인한다.
- Battle은 기존 World Controller를 평가하는 `BattleVisualResolver`의 Left Idle을 사용한다. 신규 외형도 Left 프레임으로 반영하며 전용 Attack/Guard Sprite를 생성하지 않는다. 별도 Battle 전체 전투 playthrough는 이번 검증 범위가 아니다.
- 미용실/진행 중 외형 변경 NPC는 미구현이다.

## 백그라운드 검증 방법

`PlayerAppearanceSelectionBuild.Build()`는 원본/Scene/기존 Animator를 변경하지 않고 검수 정책과 Clip을 Catalog에 반영한다. 이전 편입 도구 `ValidatedPlayerAppearanceImport.Import()`는 보수적으로 selection을 초기화하므로 재편입 후에는 새 검수 Inventory 확인과 선택 Build가 필요하다. BGM/배경을 함께 편입하는 과거 도구는 실행하지 않았다.

`PlayerAppearanceSelectionAudit.Launch()`는 clean Bootstrap에서 기존 GameView의 진입 동작을 임시 PlayUnfocused로 설정하고, domain reload 생략·격리 Save/Settings·무음을 사용한다. foreground/창 활성화/GameView 선택/OS 입력은 사용하지 않는다. 실제 빈 슬롯 버튼→CharacterCreation→PathSelection→JobSelection→FinalConfirmation→World를 실행하고 actual Bootstrap Continue 버튼으로 재진입한다. UI는 임시 RenderTexture Camera로 캡처한 뒤 Overlay로 복원한다. 검사 종료 시 원래 Play 진입/Editor 옵션·백그라운드 설정·격리 경로를 복원한다.

검사 목록:

- Catalog50/800, 중복 Stable ID0, Missing Sprite0, Missing meta0, Gender/Theme 누락0.
- 남/여 × 전체+5테마 필터, 선택/Preview, 색상 외 선택 표시, 이름 오류 차단, 확정 전 데이터 보존.
- 5×5 Path/Job 조합과 외형 독립, Job Preview, 실제 생성·World 진입.
- Ready35종 실제 World 교체,4방향 이동 상태·Idle·Walk16프레임, Battle Left Idle, 실제 JSON 저장·ID/외형 복원.
- 기존 남/여 × Path5종 × 빈 ID/invalid/blocked30조합 fallback.
- 신규 남/여 실제 Bootstrap Continue, 구버전 ID 필드 제거 JSON 및 invalid ID의 남/여 실제 Continue.

첫 검사에서 검사 도구의 Path 버튼 이름 참조를 수정했고, 확대 검수로 Vision5종을 추가 보류했다. Play 종료 후 Mixer 이벤트가 남는 검사 환경 문제는 domain reload 생략 시의 Audio 구독을 정상 초기화하여 해결했다. Audio/BGM 코드나 Asset은 수정하지 않았다.

최종 검사 수·Console·보호 파일·commit은 아래 최종 결과에 기록한다. 실제 기기 입력과 foreground 움직임 QA를 완료했다고 주장하지 않는다.

## 최종 결과

- PlayUnfocused 실제 Runtime **1,197 PASS / 0 FAIL**. 이동 방향140, Walk 프레임560, Idle140, Battle Left Idle35, 외형별 Save/Restore35, Path×Job25, legacy/invalid/blocked fallback30, 실제 신규 Continue2 및 구버전/invalid Continue4를 포함한다. [감사 요약 JSON](Player_Appearance_Runtime_Audit.json)을 따른다.
- Catalog50·800 Sprite·Ready35·blocked15·Clip280. Duplicate Stable ID0, Missing Sprite0, Missing meta0, Gender/Theme 누락0. 기존50 ID와800 Sprite GUID/local file ID 모두 동일하며 Catalog GUID `5d34e6efc319dba46b15f1df6baf5236` 유지. 빌드 후 YAML 공백 정리는 직렬화 의미를 변경하지 않았고 재Import/참조 검사도 통과했다.
- 원본 PNG50개의 SHA-256이 Inventory와 동일하다. 검증 전 보호한798파일(Save/Settings·Scene·Prefab·기존 Player Animator·Audio·World 코드·ProjectSettings)의 해시 변화0. Main16/BGM/Voice/배경과 기존 사용자 변경을 보존했다.
- Unity6000.5.7f1 컴파일 실패false, 최종 Console Error0/Warning0. 재컴파일 중 과거 `ExternalAssetImportEditor.cs`의 spritesheet CS0618 경고2개가 있었으나 이번 코드의 경고는 아니다. 검사 환경의 종료 시점 Mixer 오류는 정리 경로 수정 후 재실행에서0이다.
- 최종 clean Bootstrap Edit Mode·Editor unfocused, GameView 진입 정책/Editor Play 옵션/백그라운드 설정 복원, AuditSaveDirectory/AuditSettingsDirectory=null. foreground 조작 없음.
- 문서 선행 `eeb48b2`, 선택/Runtime `0ad3cc5`, Save/Continue/회귀 `cc3aa90`. 관련 staged diff --check 통과. 직접 변경 파일만 commit했으며 GitHub Push 없음. 전체 작업 트리의 기존 사용자 변경/공백 경고는 이번 변경과 구분한다.
- 실제 UI 캡처와 전체1197행 로그는 `Unity/Client/Temp/AppearanceSelectionAudit/creation.png`, `runtime.txt`다. Contact Sheet는 `Temp/AppearanceQA/{Theme}Full.png`에 있으며 `Tools/PlayerAppearanceFrameAudit.py`로 원본을 변경하지 않고 재생성할 수 있다.

## 남은 사용자 확인

35종의 실제 움직임에서 발 미끄러짐·방향 전환·프레임 흔들림·캐릭터 크기와 장치 표현을 검수한다. 보류15종은 사용자 원본의 프레임/Alpha 문제를 먼저 해결한 뒤 다시 검수해야 한다. 임의 재생성·픽셀 수정을 하지 않았다.
