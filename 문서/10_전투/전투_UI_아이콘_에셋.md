# 전투 UI 아이콘 에셋

## 사용 에셋과 라이선스

- `Kenney Game Icons` (`kenney_game-icons.zip`)
- `Kenney Board Game Icons 1.1` (`kenney_board-game-icons.zip`)
- 두 팩 모두 Creative Commons Zero(`CC0 1.0`)이며 개인·교육·상업 프로젝트에 사용할 수 있다.
- 원본 ZIP과 팩에 포함된 `License.txt`는 각각 `Unity/Client/Assets/ThirdParty/Kenney/GameIcons/`, `BoardGameIcons/` 아래에 보존한다.
- 원본 이미지는 수정하지 않는다. 실제 UI에서는 흰색 Sprite에 Unity `Image.color`로 금색을 곱해 화면 톤만 맞춘다.

## 실제 파일 매핑

| UI 역할 | 실제 원본 파일 | 런타임 식별자 |
|---|---|---|
| 공격 | Board Game Icons `PNG/Default (64px)/sword.png` | `command.attack` |
| 스킬 | Game Icons `PNG/White/1x/star.png` | `command.skill` |
| 방어 | Board Game Icons `PNG/Default (64px)/shield.png` | `command.defend` |
| 도망 | Game Icons `PNG/White/1x/exitRight.png` | `command.flee` |
| 취소 | Game Icons `PNG/White/1x/cross.png` | `command.cancel` |
| 도발 | Game Icons `PNG/White/1x/target.png` | `status.taunt` |
| 행동 중 | Game Icons `PNG/White/1x/arrowRight.png` | `status.acting` |
| 재사용 대기 | Board Game Icons `PNG/Default (64px)/hourglass.png` | `status.cooldown` |
| 방어 상태 | 공격 명령과 같은 Board Game Icons `shield.png` 재사용 | `command.defend` |

파일명은 다운로드된 압축 내부에서 실제 존재 여부를 확인한 뒤 선택했다. 특히 Game Icons에는 `sword`, `shield`, `hourglass`가 없어 해당 세 항목은 Board Game Icons의 실제 파일을 사용한다.

## Unity Import와 코드 연결

- 필요한 PNG만 각 팩의 `Resources/KenneyBattleIcons/`에 복사한다.
- Texture Type은 `Sprite (2D and UI)`, Mesh Type은 `Full Rect`, Filter Mode는 `Point`, Compression은 `None`, Max Size는 `512`로 둔다.
- `BattleUiIconCatalog`가 명령·상태 역할 ID를 `Resources` 경로와 연결하고 한 번 읽은 Sprite를 재사용한다.
- 버튼과 HP HUD는 파일명을 직접 쓰지 않고 역할 ID만 요청한다. 향후 그림 교체는 카탈로그 경로와 ThirdParty PNG만 바꾸면 된다.
- 향후 `BattleSkillDefinition`에 아이콘 역할 ID를 추가하면 같은 카탈로그 로딩 방식을 재사용할 수 있다. 이번 작업에서는 스킬 전투 규칙이나 데이터 구조를 변경하지 않았다.
- Sprite가 누락되면 아이콘 Image만 숨기고 한글 텍스트는 유지한다. 폰트 기호 fallback은 사용하지 않는다.

## 변경하지 않은 전투 영역

`BattleCore`, `Combatant`의 계산, `Formation`, `TargetResolver`, `TurnOrderQueue`, 도발 지속 규칙, 방어 50%, Projectile/VFX, 승리·패배·Field 복귀와 3대3 참가자 흐름은 변경하지 않는다. 아이콘 카탈로그와 표시 코드만 전투 상태를 읽는다.

## Unity Play Mode 확인

1. 공격·스킬·방어·도망 버튼과 대상/스킬 선택 중 취소 버튼에 실제 Sprite와 한글이 함께 보이는지 확인한다.
2. 아이콘이 약 16~18px 크기로 원본 비율을 유지하며 기존 버튼 크기와 간격을 바꾸지 않는지 확인한다.
3. 도발 후 target 아이콘과 `도발 2 → 도발 1 → 제거`가 일치하는지 확인한다.
4. 방어·행동 중·재사용 상태에 각각 shield·arrowRight·hourglass Sprite와 텍스트가 한 줄로 표시되는지 확인한다.
5. 전투불능 즉시 모든 상태 아이콘과 텍스트가 사라지고 회색 `전투불능`만 남는지 확인한다.
6. 취소 버튼과 Esc가 기존과 같은 대상/스킬 선택 복귀 흐름을 실행하는지 확인한다.
7. Console에 Compile Error, `NullReferenceException`, `MissingReferenceException`이 없는지 확인한다.
