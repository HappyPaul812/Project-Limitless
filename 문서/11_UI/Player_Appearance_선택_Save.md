# Player Appearance 선택·저장 계약

2026-10-03 LOCAL 검증본 Catalog의 Stable ID를 사용한다. 기존 PNG·Sprite meta·Animator Controller·Scene·Prefab과 Male/Female+Path Variant를 보존한다.

## 선택

CharacterCreation의 성별·이름·다음 단계를 유지하면서 성별 카드 아래에 외형 탐색을 추가한다. 성별 필터, 전체/5테마 필터, 이전/다음 외형, 큰 Down Idle Preview, 외형 이름과 선택 표시를 제공한다. 기본 외형도 선택할 수 있다. 준비되지 않은 외형은 목록에서 제외하며 50종 전체는 Catalog에 남긴다. 필터 조작은 UI 초안만 변경하며 다음을 눌러 이름 검증을 통과할 때 Session을 확정한다. 영구 Save는 기존 FinalConfirmation에서 확정한다.

외형 테마는 분류·추천일 뿐이다. Path/Job 선택을 제한하거나 Story/Quest 조건에 사용하지 않는다. 실제 Player Path가 Story와 공식 PathSymbol을 결정한다. 모든 버튼은 기존 EventSystem의 키보드/게임패드 Navigation과 Tab 순서에 포함한다. 선택은 색상과 함께 `✓ 선택됨` 텍스트로 표시한다.

## Runtime·Save

Session의 SelectedAppearanceId와 Save Version1의 선택적 AppearanceId 문자열을 추가한다. 배열 순서·파일명은 저장하지 않는다. 유효하고 준비된 ID만 신규 외형으로 적용한다. 필드가 없는 구버전, 알 수 없는 ID, blocked ID, Sprite/Clip 누락은 기존 성별·Path Variant로 돌아간다. 이름·성별·Path·Job과 기존 Version1 호환은 유지한다.

기존 성별 Animator Controller의 상태·입력·PlayerSpriteAnimator를 재사용하고 방향별 Sprite Clip만 AnimatorOverrideController로 바꾼다. Controller Asset을 50개 복제하지 않는다. Clip은 Editor에서 Catalog의 subasset으로 작성하며 기존 Controller/Clip은 수정하지 않는다. 새 Appearance가 적용되면 Path Variant는 본체를 덮어쓰지 않으며 실제 PathSymbol은 계속 표시한다. Battle은 기존 World Controller/Sprite 전달 구조를 재사용한다.

## 검수 정책

50종의 16프레임과 방향 행을 백그라운드 Contact Sheet로 검사한다. 행 계약은 위부터 Down/Left/Right/Up, 행마다 Walk4프레임이며 Idle은 첫 프레임이다. Alpha 경계 접촉만으로 잘림을 단정하지 않는다. 방향 불일치·분리된 본체·다른 캐릭터·심각한 Alpha 손상·참조 누락은 false와 구체적인 reviewReason을 유지한다. 정적 합격과 실제 움직임의 발 미끄러짐·흔들림·입력·크기 시각 검수는 별도다. foreground 검수에는 사용자의 사전 허락이 필요하다.

게임 진행 중 외형 변경 NPC/미용실은 이번 범위에 포함하지 않는다. 검증 결과와 실제 ready 수는 [QA 보고서](Player_Appearance_선택_QA.md)에 기록한다.
