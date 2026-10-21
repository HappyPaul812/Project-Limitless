# 서리들쥐 전투 Fix v2 승인본 정식 외형 연결 — 2026-10-21

**완료: 원본 디스크·원본 Editor Import 반영 / 실제 격리 Battle 40 PASS·0 FAIL.**

바울 대표께서 이 대화에서 전투 Fix v2를 최종 미술 승인하고 원본 연결을 지시하셨습니다. 승인 근거는 `codex/chapter3-mouse-combat-v2-review-20261021` / `757d6a759a3ecd53d4a4e14d03324c20dd3b7897` / 변경분23PASS입니다. 본 작업은 미술 재제작 없이 승인 PNG를 정식 자산 경로에 연결한 것입니다.

## 연결 범위와 기획 경계

원본에 서리들쥐의 기존 게임용 PNG·MonsterDefinition·조우가 없어 최초 연결했습니다. 기존 `MonsterDefinition` 형식의 **외형 전용 구성**을 `Resources/MonsterVisuals/FrostFieldMouse`에 저장하고 공용 `MonsterSpriteSheetAnimation`이 네 모션을 그대로 읽도록 연결했습니다. Animator나 AnimationClip은 필요하지 않습니다.

**Monster ID·스탯·보상은 기획상 TBD이므로 확정하지 않았습니다.** `monsterId`는 비워두었고 표준 `Resources/MonsterDefinitions` 목록에는 넣지 않았습니다. 해당 형식에 직렬화된 HP60 등은 클래스 기본값이며 정식 서리들쥐 전투 밸런스가 아닙니다. 신규 Field·Quest·월드 스폰·Chapter3 조우·다른 몬스터 자산을 추가하지 않았습니다. 따라서 일반 플레이에서 새 조우가 열렸다는 의미는 아닙니다. 향후 조우 구현 시 승인 외형 배열을 사용할 수 있습니다.

## 제품 변경 파일 — 13경로

| 경로 (`Unity/Client/` 기준) | 내용 |
|---|---|
| `Assets/_Project/Art/Monsters/FrostFieldMouse/FrostFieldMouse_A_Idle_4Frames.png` + `.meta` | 기존 승인 Idle을 바이트 그대로 최초 Import |
| 같은 폴더의 `FrostFieldMouse_A_Attack_4Frames.png` + `.meta` | 승인 Fix v2 Attack |
| 같은 폴더의 `FrostFieldMouse_A_Hit_4Frames.png` + `.meta` | 승인 Fix v2 Hit |
| 같은 폴더의 `FrostFieldMouse_A_Defeat_4Frames.png` + `.meta` | 승인 Fix v2 Defeat |
| `Assets/_Project/Art/Monsters/FrostFieldMouse.meta` | 새 자산 폴더 GUID |
| `Assets/_Project/Resources/MonsterVisuals.meta` | 외형 구성 폴더 GUID |
| `Assets/_Project/Resources/MonsterVisuals/FrostFieldMouse.asset` + `.meta` | Idle/Attack/Hit/Defeat 각각00~03 명시적 배열, fieldSprite=Idle00 |
| `Assets/_Project/Scripts/Monster/MonsterSpriteSheetAnimation.cs` | 피격 중 Attack 시작 시 reacting 해제, 사망 후 Attack 차단의 최소 수정 |

새 meta GUID7개는 원본 전체 Assets와 충돌0. 기존 GUID·meta·Scene·다른 몬스터 Definition 변경0입니다. 각 PNG는 2048×512 RGBA / Multiple4셀512 / 개별Pivot(0.5,0) / PPU512 / Point / Clamp / MipMap OFF / 무압축 / FullRect / 8FPS / scale1 / 우향입니다.

## 승인 원본 바이트

보관 ZIP `Temp/EOD20261020/Archives/PendingQA/Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2.zip`, SHA256 `518d210a0e5b29c7214ad960c0baa3d23a88d13d4f789b5f7420982ce701bfe3`. CRC 정상, 재다운로드·ZIP 재생성 없음.

| PNG | SHA256 |
|---|---|
| Idle | `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2` |
| Attack | `c83e35b5aebbce908b6025d5a071fafc463cfe301763f68806ecddb8bd4f576b` |
| Hit | `695ed7db1f794c3f71df1b058eb5e91f01de7ecb125369264f3624b36c9fd3c2` |
| Defeat | `3f6a52039b766c57a5367c7145e0234b68ac0a552940de1e88ffe8f2404f3395` |

## 실제 전투·최소 회귀 — 40 PASS / 0 FAIL

Unity6000.6.5f1 숨김 Batch Client에서 **기존 Battle Scene·BattleSceneController·BattleActionPresenter·BattleCore**를 사용했습니다. 원본 제품 C#244개와 격리 코드는 수정 전 내용이 모두 동일했고 핵심 Battle Scene도 줄바꿈 제외 동일했습니다. 과거36건 전체 검사 대신 새 연결·실제 피해/모션 경로·새로 재현한 상태 전환만 검사했습니다.

- 실제 Battle의 `BattleEncounterContext`에 정식 외형 구성 Asset을 주입했습니다. 기존 3적 fixture이며 정식 Chapter3 조우 구성은 아닙니다. 플레이어 외형은 이 fixture에 넣지 않아 오른쪽 아군 Sprite는 비어 있습니다. 이번 검증의 missing-sprite 판정은 몬스터16프레임 연결 범위입니다.
- 입장 우선순위 연출이 종료된 뒤 실제 GameView1280×720 / 적Image136×112 preserveAspect / scale1에서 각00~03을 관측·캡처했습니다.
- **Idle→Attack00/01/02/03→Idle**, 실제 기본 공격 피해1회 및 전열 원위치 복귀 PASS. 최대 할퀴기02, Attack00 경계 크기·색상 유지.
- **실제 아군 공격 피해→Hit00/01/02/03→Idle**, 생존 HP 감소 및 흔들림 이후 루트 복구 PASS. Hit03 경계 확대 점프 없음.
- 마지막 타격으로 전투불능→**Defeat00/01/02/03**, 03 도달 후1.3초 이상 유지·Idle 덮어쓰기 없음 PASS. 원래 공용 사망 색/알파 처리는 유지했습니다.
- **Hit→Attack**: 기존 reacting 플래그가 남아 Attack이 Hit에 가려지는 결함을 실제 경로에서 재현했습니다. 위 공용 코드의 최소 수정 후 재검사 PASS. 기존 그을음들개·잿불 망령도 Hit→Attack과 사망 후 공격 재시작 차단 PASS. 다른 몬스터 아트·수치 변경0.
- 셀 잘림·뚜렷한 체격 점프·의도된 공격 접근/피격 흔들림 외의 지지 발 미끄러짐이 캡처 순서에서 관찰되지 않았고, 몬스터 CurrentFrame null0입니다. 픽셀 자체는 이전 최종 승인본과 동일합니다.
- 8FPS 설정과 실제 프레임 순서 확인. 예: Attack00 3.98855→01 4.10888→02 4.24636→03 4.35962→Idle 4.47599초. Editor 관측 간격에 오차가 있으므로 모든 프레임의 벽시계125ms를 보장한다는 뜻은 아닙니다. 실제 피해 프레임02의 엄밀한 프레임 이벤트 동기화까지 새로 구현하지 않았습니다.
- 캡처 PNG는 메모리에 보관한 뒤 검사 종료 후 압축하여 저장 지연이 재생 관측을 방해하지 않게 했습니다. 제공 APNG는 그 캡처를125ms로 재구성했으며 연속 실시간 녹화가 아닙니다. 검토용 Defeat APNG만 반복되고 실제 런타임은03고정입니다.
- 최종 컴파일 오류0·신규 제품 런타임 오류0. 격리 환경의 기존 Search 예외·URP/SpeedTree 진단은 로그에 보존했습니다. 첫 하네스의 잘못된 피해 메서드명, PNG 저장 지연, 입장 연출 캡처 문제는 검수 하네스 수정 사항으로 제품 오류와 구분했습니다.

[실제 전투 공격02](Evidence/MouseCombatV2Apply_20261021/Runtime/Attack_Frame2.png) · [사망03 유지](Evidence/MouseCombatV2Apply_20261021/Runtime/Defeat_Hold03.png) · [동작 비교](Evidence/MouseCombatV2Apply_20261021/index.html) · [검사40건](Evidence/MouseCombatV2Apply_20261021/Runtime/checks.txt) · [프레임 실측](Evidence/MouseCombatV2Apply_20261021/Runtime/FrameTrace.tsv)

## 원본 적용·보호

승인된 제품13경로를 원본 `Unity/Client`에 반영했습니다. 원본 Editor에는 새PNG4·Definition·재생기 코드 **6개 경로만 명시적으로 Import**했으며 전역 강제 Refresh·Scene 전환·Play 진입·창/OS 포커스 변경은 하지 않았습니다.

원본 Editor에서 새 Definition 로드 및 **각4프레임·8FPS·scale1·우향**을 다시 확인했습니다. 컴파일/domain reload 종료 후 Console error 조회0, Bootstrap / PlayOFF / PausedOFF / 비포커스 유지입니다. 원본 자동 감지의 external_changes_dirty 표시가 남아 있어도, 위 대상 자산은 명시적 Import와 로드로 확인한 상태입니다.

원본 **4,339기존파일** SHA 대조: 허용한 재생기 코드1개 외 변경0, 신규12개 외 추가0. Save/UserData·Settings·Packages·기존 승인 아트·사용자 수정 파일 보호. 제품13경로는 원본과 기능 worktree의 바이트가 모두 동일합니다. [보호 증거](Evidence/MouseCombatV2Apply_20261021/ProtectionAfter.json).

## Git·다음 작업

- 사용 브랜치: `codex/chapter3-mouse-combat-v2-apply-20261021`.
- 제품 연결 commit: `2201da328e481b83f397860ea22e584b88f1da11`. 검수 증거·완료 문서는 후속 Docs commit에 기록합니다.
- 원본 main HEAD `e4e9891f6f01ea95cada9e63b1f09946d94d6041` 유지. 원본 index 조작·main 직접 commit·Push·PR·main 병합 **미실행**. 원본의 적용 파일은 작업 변경으로 남고, 재현 가능한 commit은 기능 브랜치에만 있습니다.
- 바울 대표의 이번 미술 승인은 반영 완료입니다. 동일 미술 재승인은 필요하지 않습니다. 다음 권장 작업은 별도 승인 범위에서 서리들쥐의 공식 ID/능력치/보상·Main22 조우를 확정해 이 외형 구성을 연결하는 것입니다. 실제 일반 플레이의 신규 Chapter3 조우 검증은 그 단계에 수행합니다.
