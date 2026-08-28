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
| 취소 | Game Icons `PNG/White/1x/arrowLeft.png` | `command.cancel` |
| 도발 | Board Game Icons `PNG/Default (64px)/pawn_right.png` | `status.taunt` |
| 행동 중 | Game Icons `PNG/White/1x/arrowRight.png` | `status.acting` |
| 재사용 시작 | Board Game Icons `PNG/Default (64px)/hourglass_top.png` | `status.cooldown.start` |
| 재사용 진행 | Board Game Icons `PNG/Default (64px)/hourglass.png` | `status.cooldown.progress` |
| 재사용 마지막 | Board Game Icons `PNG/Default (64px)/hourglass_bottom.png` | `status.cooldown.end` |
| 방어 상태 | 공격 명령과 같은 Board Game Icons `shield.png` 재사용 | `command.defend` |
| 수호자 도발 스킬 | Board Game Icons `PNG/Default (64px)/pawn_left.png` | `skill.guardian.taunt` |
| 치유사 치유의 빛 | Board Game Icons `PNG/Default (64px)/suit_hearts.png` | `skill.healer.healing_light` |
| 사수 정조준 | Game Icons `PNG/White/1x/target.png` | `skill.sharpshooter.aim` |
| 사수 화살비 | Board Game Icons `PNG/Default (64px)/bow.png` | `skill.sharpshooter.arrow_rain` |
| 사수 동료의 습격 | ScratchIO `Wolf_Run.png` 다섯 번째 64×40 프레임(index 4) 런타임 Sprite | `skill.sharpshooter.companion_assault` |
| 투사 난도 스킬 | Game Icons `PNG/White/1x/cross.png` | `skill.fighter.nando` |
| 투사 기세 상태 | Board Game Icons `PNG/Default (64px)/skull.png` | `status.fighter.momentum` |
| 투사 회심의 일격 | Board Game Icons `PNG/Default (64px)/skull.png` | `skill.fighter.critical_strike` |
| 투사 회오리 베기 | Board Game Icons `PNG/Default (64px)/spinner.png` | `skill.fighter.whirlwind` |
| 마도사 파이어 볼 | PVFX Foundry `warm-explosion` peak index 4 런타임 Sprite | `skill.mage.fireball` |
| 화상 상태 | 파이어 볼과 같은 `warm-explosion` peak index 4 | `status.burn` |
| 마도사 썬더볼트 | PVFX Foundry `electric-impact` peak index 1 런타임 Sprite | `skill.mage.thunderbolt` |
| 감전 상태 | Kenney Game Icons `PNG/White/1x/power.png` | `status.shock` |

파일명은 프로젝트에 보존한 압축 내부에서 실제 존재 여부를 확인한 뒤 선택했다. 적에게 남는 도발 **상태**는 `pawn_right.png`를 계속 사용하고, 수호자가 누르는 도발 **스킬 버튼**은 `pawn_left.png`를 사용한다. `target.png`는 도발 상태가 아니라 사수 정조준 버튼에 연결한다. 같은 전투 개념이라도 상태 요약과 실행 버튼의 역할 ID가 다르므로 서로의 아이콘이 바뀌지 않는다.

취소는 동작 의미가 더 분명한 `arrowLeft.png`를 유지한다. `bow.png`는 사수 `화살비`, `cross.png`는 투사 `난도`, `spinner.png`는 전열을 도는 `회오리 베기` 스킬에 사용하고, `skull.png`는 투사 개인 자원 `기세`와 이를 소비하는 `회심의 일격`에 사용한다. 상단 HUD는 아이콘과 `기세 n` 한글을 함께 보여 그림만으로 상태를 전달하지 않는다.

동료의 습격은 전투 연출과 같은 원본 Texture에서 몸통·머리·꼬리·다리 간격이 작은 버튼에서도 비교적 잘 보이는 다섯 번째 Run 프레임(index 4)을 런타임에 잘라 실제 Wolf 아이콘으로 표시한다. 별도 PNG를 만들거나 원본 SpriteSheet를 수정하지 않는다. 리소스 경로·프레임 폭·아이콘 프레임 번호는 `BeastCompanionDefinition`이 제공하므로 UI와 `BattleSceneController`에 Wolf 경로를 하드코딩하지 않으며, 사수 정조준은 기존 Kenney `target.png` 연결을 그대로 유지한다.

파이어 볼은 PVFX Foundry 0.3.0 CC0 `warm-explosion/grid/sprite-sheet.png`의 폭발 최대 프레임 index 4를 런타임에 잘라 버튼과 화상 상태 아이콘으로 사용한다. 버튼은 고정 한 장이고 실제 명중 연출은 15프레임 전체이므로 서로 섞이지 않는다. HUD에는 그림과 `화상 n` 한글을 함께 표시해 색상만으로 상태를 전달하지 않는다.

썬더볼트 버튼은 PVFX Foundry 0.3.0 CC0 `electric-impact/grid/sprite-sheet.png`의 peak index 1을 고정 Sprite로 잘라 사용하고, 전투 연출은 같은 원본의 14프레임 전체를 별도로 순환한다. 감전 상태는 CC0 Kenney `power.png`를 사용하며 HUD와 상세 팝업에 `감전 1` 한글을 함께 표시한다. 원본 ThirdParty 파일은 수정하지 않고 Resources용 사본과 런타임 Sprite만 사용한다.

재사용 대기는 실제 HUD 숫자를 기준으로 단계를 고른다. 현재 구현된 3턴 스킬은 사용 직후 `재사용 3턴`이므로 `hourglass_top`, 다음 자기 행동 시작 뒤 `2턴`은 `hourglass`, 마지막 `1턴`은 `hourglass_bottom`을 표시한다. 다음 감소로 0이 되면 텍스트와 아이콘을 함께 숨긴다. 이 선택은 UI 표현이며 `BattleSkillCooldowns`의 저장·감소 방식은 변경하지 않는다.

## Unity Import와 코드 연결

- 필요한 PNG만 각 팩의 `Resources/KenneyBattleIcons/`에 복사한다.
- Texture Type은 `Sprite (2D and UI)`, Mesh Type은 `Full Rect`, Filter Mode는 `Point`, Compression은 `None`, Max Size는 `512`로 둔다.
- `BattleUiIconCatalog`가 명령·상태·직업 스킬 역할 ID를 `Resources` 경로와 연결하고 한 번 읽은 Sprite를 재사용한다.
- 버튼과 HP HUD는 파일명을 직접 쓰지 않고 역할 ID만 요청한다. 향후 그림 교체는 카탈로그 경로와 ThirdParty PNG만 바꾸면 된다.
- `BattleSkillDefinition.IconId`가 스킬별 역할 ID를 보관하고 스킬 메뉴는 그 값만 `BattleUiIconCatalog.Load`에 전달한다. 따라서 메뉴 코드에는 직업명·스킬명 비교나 PNG 파일명이 없다.
- 향후 스킬 아이콘은 해당 스킬 정의의 `IconId`와 카탈로그 매핑만 추가하거나 교체하면 된다. 아이콘 데이터는 표시 전용이며 피해·회복·쿨타임·대상 판정에는 관여하지 않는다.
- Sprite가 누락되면 아이콘 Image만 숨기고 한글 텍스트는 유지한다. 폰트 기호 fallback은 사용하지 않는다.

## 변경하지 않은 전투 영역

`BattleCore`, `Combatant`의 계산, `Formation`, `TargetResolver`, `TurnOrderQueue`, 도발 지속 규칙, 방어 50%, Projectile/VFX, 승리·패배·Field 복귀와 3대3 참가자 흐름은 변경하지 않는다. 아이콘 카탈로그와 표시 코드만 전투 상태를 읽는다.

## Unity Play Mode 확인

1. 공격·스킬·방어·도망 버튼과 대상/스킬 선택 중 취소 버튼에 실제 Sprite와 한글이 함께 보이고, 취소에는 arrowLeft가 표시되는지 확인한다.
2. 아이콘이 약 16~18px 크기로 원본 비율을 유지하며 기존 버튼 크기와 간격을 바꾸지 않는지 확인한다.
3. 도발 후 pawn_right 아이콘과 `도발 2 → 도발 1 → 제거`가 일치하며 target 아이콘이 나오지 않는지 확인한다.
4. 스킬 사용 직후 3턴=hourglass_top, 진행 2턴=hourglass, 마지막 1턴=hourglass_bottom으로 바뀌고 0에서 모두 사라지는지 확인한다.
5. 방어·행동 중 상태에 각각 shield·arrowRight Sprite와 텍스트가 한 줄로 표시되는지 확인한다.
6. 전투불능 즉시 모든 상태 아이콘과 텍스트가 사라지고 회색 `전투불능`만 남는지 확인한다.
7. 취소 버튼과 Esc가 기존과 같은 대상/스킬 선택 복귀 흐름을 실행하는지 확인한다.
8. Console에 Compile Error, `NullReferenceException`, `MissingReferenceException`이 없는지 확인한다.
9. 스킬 메뉴에서 수호자 도발=`pawn_left`, 치유의 빛=`suit_hearts`, 정조준=`target` Sprite가 각각 스킬 이름 왼쪽에 표시되는지 확인한다.
10. 적의 도발 상태에는 기존 `pawn_right`가 유지되고, 스킬 버튼 아이콘 변경이 도발 2→1·회복·정조준 피해와 쿨타임에 영향을 주지 않는지 확인한다.
11. 취소 버튼에 cross가 더 이상 나오지 않고, 대상 선택·스킬 메뉴에서 Esc와 같은 단계로 복귀하는지 확인한다.
