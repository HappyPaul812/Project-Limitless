# 월드 레벨과 EXP HUD

## 표시 구조

- `PlayerNameplate`는 기존 Screen Space Overlay Canvas와 머리 위 위치 추적을 유지하며 `Lv.{Level} {PlayerName}`을 흰색 글자로 표시한다. 몬스터 난도 색은 적용하지 않는다.
- `WorldExperienceHud`는 같은 Overlay Canvas의 화면 하단 중앙에 폭 420, 높이 76의 얇은 고정 패널로 생성된다.
- HUD 첫 줄은 `Lv.n  이름`, 둘째 줄은 `EXP 현재 / 필요`, 아래쪽은 금색 진행 Bar다. `EXP` 글자를 함께 표시해 HP와 구분한다.
- Battle 참가자는 `PlayerNameplate`를 사용하지 않으므로 World EXP HUD를 생성하지 않는다. 기존 Battle Victory 결과 UI는 유지한다.

## 데이터와 갱신

Level, PlayerName, CurrentExperience는 현재 선택된 저장 슬롯을 복원한 `GameSessionData`에서 읽는다. 다음 필요 EXP와 MaxLevel은 기존 `ExperienceProgression.RequiredExp`와 `CharacterGrowthCalculator.MaxLevel`을 사용하며 UI에 공식을 복제하지 않는다.

Lv1~49 Bar는 `Clamp01(CurrentExperience / RequiredExp(Level))`이다. Lv50은 다음 구간이 없으므로 `MAX LEVEL`을 표시하고 Bar를 가득 채운다.

Fill은 왼쪽 시작 `Image.Type.Filled`와 `fillAmount`만 사용한다. uGUI가 Filled 메시를 실제로 생성하도록 1×1 흰색 런타임 Sprite를 Source로 연결한다. RectTransform 폭 방식은 함께 사용하지 않는다.

이름표와 HUD는 Scene 시작·활성화 때 즉시 초기화된다. 이후 매 프레임 UI를 재생성하지 않고 이름·Level·CurrentExperience 숫자의 변경만 비교한다. Battle 승리 후 Field 복귀, 다중 레벨업, 이어하기와 다른 슬롯 선택에서도 최종 세션 값이 표시된다.

`WorldExperienceHud.EnsureOn`은 Canvas 아래 같은 이름의 HUD를 재사용해 한 Scene에 중복 생성을 막는다. Single Scene 전환에서 Player가 제거될 때 Player가 소유한 Overlay Canvas도 함께 정리되고, 새 World Scene의 Player가 현재 세션 값으로 다시 만든다.

## 검증

격리 Unity 6000.5.7f1의 빈 Scene Play Mode 감사에서 다음을 확인했다.

- Overlay Canvas와 이름표/HUD 각 1개 생성
- `Lv.1 마도바울이`, `EXP 0 / 100`, Bar 0%
- EXP 24/100·50/100·75/100·99/100의 `fillAmount`와 실제 렌더 메시 폭 24%·50%·75%·99%
- Lv3 EXP 85/170의 Bar 50%
- Lv1 EXP 90/100에서 30 획득 후 Lv2 EXP 20/130과 Bar 약 15.4%
- 같은 Scene에서 Lv3 EXP 20/170으로 이름표와 HUD 갱신
- Lv50 `MAX LEVEL`과 Bar 100%
- 컴파일 오류·NullReference·MissingReference 없음

실제 프로젝트 Game View에서는 이름표 위치와 하단 HUD의 시각적 크기·다른 UI 겹침, StarterVillage↔Field_01↔Field_02 전환, 실제 슬롯 이어하기, 정상 전투 승리 후 복귀를 직접 확인한다.
