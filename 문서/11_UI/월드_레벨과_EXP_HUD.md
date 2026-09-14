# 월드 레벨과 EXP HUD

## 표시 구조

- `PlayerNameplate`는 기존 Screen Space Overlay Canvas와 머리 위 위치 추적을 유지하며 `Lv.{Level} {PlayerName}`을 흰색 글자로 표시한다. 몬스터 난도 색은 적용하지 않는다.
- `WorldExperienceHud`는 같은 Overlay Canvas의 화면 하단 중앙에 폭 420, 높이 76의 얇은 고정 패널로 생성된다.
- HUD 첫 줄은 32×32px 공식 `PathSymbol`과 `Lv.n  이름`, 둘째 줄은 `EXP 현재 / 필요`, 아래쪽은 금색 진행 Bar다. 심볼은 이름 왼쪽에 배경·테두리 없이 원본 비율과 색상을 유지하며, `EXP` 글자를 함께 표시해 HP와 구분한다.
- 이 HUD는 플레이어의 고정 정체성을 표시하므로 전투 기능용 `TraitIcon`이나 특성 이름·상태는 사용하지 않는다. 머리 위 `PlayerNameplate`에도 심볼을 추가하지 않는다.
- Battle 참가자는 `PlayerNameplate`를 사용하지 않으므로 World EXP HUD를 생성하지 않는다. 기존 Battle Victory 결과 UI는 유지한다.

## 상호작용 UI 우선순위

- World EXP HUD는 일반 탐험 중에만 표시한다. NPC 대화, 상점, 은행, 치유 확인, 동료 편성, 퀘스트처럼 화면을 점유하는 상호작용 UI가 열리면 일시적으로 숨기고 모든 상호작용 UI가 닫힌 뒤 복귀시킨다.
- 상호작용 UI는 `WorldExperienceHud.SetInteractionUiOpen(owner, isOpen)`으로 소유자별 열림 상태를 등록한다. 같은 소유자의 중복 등록은 한 번으로 처리하므로 반복 대화와 여러 UI의 중첩에서도 마지막 UI가 닫히기 전 HUD가 먼저 나타나지 않는다.
- HUD의 `CanvasGroup`은 숨김 중 alpha 0, interactable false, blocksRaycasts false를 사용한다. 따라서 보이지 않는 HUD가 상호작용 UI 입력을 가로막지 않는다.
- 대화 Canvas sortingOrder는 10, PlayerNameplate와 World EXP HUD의 Overlay Canvas는 5다. 표시 억제가 실패해도 대화창이 HUD보다 앞에 오며, 대화 Panel은 열릴 때 Canvas의 마지막 sibling으로 보정한다.
- Scene 전환이나 DialoguePresenter 비활성화·파괴 시 해당 소유자의 억제 상태를 해제한다. 새 World Scene의 HUD는 현재 열려 있는 상호작용 UI가 없으면 정상 표시되고 Battle에는 생성되지 않는다.

## 데이터와 갱신

Level, PlayerName, CurrentExperience와 PathId는 현재 선택된 저장 슬롯을 복원한 `GameSessionData`에서 읽는다. `PathPresentationResolver`가 PathId에 해당하는 `PlayerPathDefinition.PathSymbol`을 찾아 새 캐릭터와 이어하기가 같은 공식 심볼을 사용한다. 저장 파일에는 Sprite를 직접 넣지 않는다. PathId가 비었거나 잘못됐거나 Sprite가 누락되면 Image만 숨기고 기존 레벨·이름·EXP는 계속 표시한다. 다음 필요 EXP와 MaxLevel은 기존 `ExperienceProgression.RequiredExp`와 `CharacterGrowthCalculator.MaxLevel`을 사용하며 UI에 공식을 복제하지 않는다.

Lv1~49 Bar는 `Clamp01(CurrentExperience / RequiredExp(Level))`이다. Lv50은 다음 구간이 없으므로 `MAX LEVEL`을 표시하고 Bar를 가득 채운다.

Fill은 왼쪽 시작 `Image.Type.Filled`와 `fillAmount`만 사용한다. uGUI가 Filled 메시를 실제로 생성하도록 1×1 흰색 런타임 Sprite를 Source로 연결한다. RectTransform 폭 방식은 함께 사용하지 않는다.

이름표와 HUD는 Scene 시작·활성화 때 즉시 초기화된다. 이후 매 프레임 UI를 재생성하지 않고 이름·PathId·Level·CurrentExperience 값의 변경만 비교한다. Battle 승리 후 Field 복귀, 다중 레벨업, 이어하기와 다른 슬롯 선택에서도 최종 세션 값과 심볼이 표시된다.

`WorldExperienceHud.EnsureOn`은 Canvas 아래 같은 이름의 HUD를 재사용해 한 Scene에 중복 생성을 막는다. Single Scene 전환에서 Player가 제거될 때 Player가 소유한 Overlay Canvas도 함께 정리되고, 새 World Scene의 Player가 현재 세션 값으로 다시 만든다.

## 검증

Unity MCP로 `World_StarterVillage` Play Mode에서 일반 주민, 잡화 상인, 주민 대표 대화를 각각 열고 닫아 HUD alpha 0→1, blocksRaycasts false, 대화 Canvas 10 > World Canvas 5, DialoguePanel 마지막 sibling을 확인했다. 같은 대화를 3회 반복해 매회 숨김·복귀했고, `Field_01` 왕복 뒤 새 HUD 표시 및 재대화 숨김·복귀도 확인했다. 기능 관련 Error는 0개이며 저장 슬롯 없이 World Scene을 직접 연 경로의 기존 자동 저장 건너뜀 Warning 1개만 발생했다.

격리 Unity 6000.5.7f1의 빈 Scene Play Mode 감사에서 다음을 확인했다.

- Overlay Canvas와 이름표/HUD 각 1개 생성
- 시각의 길 PathId에서 32px PathSymbol·원본 비율 유지, 잘못된 PathId에서 심볼만 숨김
- `Lv.1 마도바울이`, `EXP 0 / 100`, Bar 0%
- EXP 24/100·50/100·75/100·99/100의 `fillAmount`와 실제 렌더 메시 폭 24%·50%·75%·99%
- Lv3 EXP 85/170의 Bar 50%
- Lv1 EXP 90/100에서 30 획득 후 Lv2 EXP 20/130과 Bar 약 15.4%
- 같은 Scene에서 Lv3 EXP 20/170으로 이름표와 HUD 갱신
- Lv50 `MAX LEVEL`과 Bar 100%
- 컴파일 오류·NullReference·MissingReference 없음

실제 프로젝트 Game View에서는 이름표 위치와 하단 HUD의 시각적 크기·다른 UI 겹침, StarterVillage↔Field_01↔Field_02 전환, 실제 슬롯 이어하기, 정상 전투 승리 후 복귀를 직접 확인한다.
