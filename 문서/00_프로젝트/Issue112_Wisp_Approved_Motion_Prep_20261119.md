# Issue #112 · 눈보라 정령 승인 모션 기준 자산과 F4/F5 연결 준비

2026-11-19 · 바울 대표 최종 미술 승인 반영. 게임용 기준 경로는 `Assets/_Project/Art/Monsters/BlizzardWisp/`입니다.

| 승인본 | Unity 기준본 | 용도 |
|---|---|---|
| OnePose | `BlizzardWisp_KeyPose.png` | 512×512 정지 원화, 기본 외형 후보 |
| Idle8 v2 | `BlizzardWisp_Idle8_v2.png` + `BlizzardWisp_Idle.anim` | 512×512×8, 0.96초 반복, `Idle_Right` 기본 상태 |
| Attack4 | `BlizzardWisp_Attack4.png` + `BlizzardWisp_Attack.anim` | 4셀, 수동 검토용 비루프 0.82초 |
| Hit4 | `BlizzardWisp_Hit4.png` + `BlizzardWisp_Hit.anim` | 4셀, 수동 검토용 비루프 0.52초 |
| Defeat4 | `BlizzardWisp_Defeat4.png` + `BlizzardWisp_Defeat.anim` | 4셀, 수동 검토용 비루프 2.18초, 마지막 셀 1.60초 포함 |

승인 PNG 바이트 및 기존 Idle 시트·클립·시각 프리팹 참조를 보존했습니다. 기존 시각 프리팹의 컨트롤러에 세 개의 수동 확인 상태를 추가했습니다. 기존 Idle이 기본이며 자동 전환·게임 이벤트는 없습니다. 클립 시간은 승인 검토 GIF에서 재현한 **시각 확인용 시간**입니다. 출시 전투 피해·동상·행동 시간의 기준은 승인 전투 정책입니다. v1·중간 후보는 기존 ZIP과 검수 기록에서 보존합니다. Unity 6000.6.5f1 격리 Editor에서 잘린 Sprite, 각 클립에서 해당 Sprite 순서 및 마지막 홀드, 기존 프리팹 연결을 검증했습니다. [실물 및 해시 근거](Issue112_Wisp_Defeat4_Review_20261119.md)

## 정식 등록 가능 여부

기존 `MonsterDefinition.ExplicitIdleFrames/ExplicitAttackFrames/ExplicitHitFrames/ExplicitDefeatFrames`와 `FieldMonsterSpawnDefinition`으로 등록할 수 있습니다. 현재 원본에 M5 정식 정의·조우·F4/F5 Scene/Build 등록이 없고, 기존 Battle 냉기 생산 연결은 Owl 후보 경로에 한정되어 있습니다. 신규 Monster ID·조우 또는 동상 정책을 이번 작업에서 등록하지 않았습니다. 승인 데이터·다른 몬스터·Scene·개인 Save는 건드리지 않았습니다. [북부 몬스터 승인 기획](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/%EB%AC%B8%EC%84%9C/08_%EB%AA%AC%EC%8A%A4%ED%84%B0/Chapter3_%EB%B6%81%EB%B6%80%EB%AA%AC%EC%8A%A4%ED%84%B07%EC%A2%85_%EC%A0%84%ED%88%AC%EB%B0%B8%EB%9F%B0%EC%8A%A4_20261118.md)

## F4/F5 등록 순서와 실제 플레이 확인

1. **데이터 연결.** 정식 M5 stable ID, 승인 스탯·드롭·정식 Item ID와 Main26 조우 연결을 대조합니다. 첫 동상 전에 기존 계획의 해빙 연고 공급·안내, F4 정령 단독/갑충 혼합과 F5 정령 단독을 대조합니다. 동상 공격 예고·대상 고정·부적격 대상 기본 공격·한 번에 한 명 행동불가·두 행동 기회 및 동상 틱 경계가 실제 전투에서 검증돼야 합니다. 승인 수치 및 드롭률 변경은 이번 범위 밖입니다.
2. **Scene 연결.** 정식 F4/F5 Scene 및 빌드 목록, World Bounds·카메라 한계·네 면 Collider·출구 Opening·스폰과 Trigger 분리를 확인합니다. 안전거점·출입구에서 조우하지 않도록 하고 도보/휠체어 우회 접근·승리/패배 복귀와 재조우 유예를 확인합니다. 좌표와 정식 배치·조우 ID는 해당 Scene 계약으로 확정합니다.
3. **시각 연결.** `MonsterDefinition`의 명시적 프레임 배열(8/4/4/4)을 기존 Field/Battle 프레임 재생기에 연결합니다. 시각 프리팹과 명시적 배열이 같은 SpriteRenderer를 동시에 제어하는 구성은 피합니다. 기존 런타임의 공용 FPS와 이번 미리보기의 가변 홀드가 다르므로 승인된 타격 시점·기회 종료를 유지하면서 전투에서 타이밍을 확인합니다. 부유 기준점·PPU·피격/전투불능 프레이밍도 실제 Scene에서 확인합니다.
4. **실제 Play 수용.** F4 첫 지정전과 혼합 조우, F5 단독 조우를 합성 슬롯으로 실행합니다. Idle 반복→일반/예고 공격→생존 피격→KO를 추적해 피해/상태 적용 1회, Attack 후 Idle, KO 이후 Idle 깜박임 0과 마지막 셀 유지, 승리 보상·복귀·재로드 중복 지급 없음까지 확인합니다. 본 보고서는 자산 단계 검사이며 정식 조우 Play 결과는 아직 없습니다.

제품 `9324b65849a25a4322052d217a7baa087a2b01f0` · 로컬 `codex/issue112-wisp-motion-prep` · 원격 Push 미실행. [기존 Defeat 검수](Issue112_Wisp_Defeat4_Review_20261119.md) · [Issue #112](https://github.com/HappyPaul812/Project-Limitless/issues/112)
