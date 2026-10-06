## 최신 Audio 정본 — 2026-10-06

Field07 탐색/조사/전투 후 복귀는 Where the Earth Breathes 유지. Chapter2 서부 일반 Encounter 및 Main16 불씨망령 Story Battle은 **Blade and Gambit**로 확정·실제 반영했다. Field08 탐색/복귀는 Beneath The Cracked Earth, Main17 일반/Story 전투도 Blade다. 기존 전용 Boss곡 우선, 다른 Boss 공통 강제 배정 없음. 기존 단일 Source의 Fade/Loop/Field07·08 실제 일반조우 및 Main17 Story승리/Continue 검증 PASS. [현재 정본](../00_프로젝트/Chapter2_BGM_정본.md), Audio commit53ef42c, 최종 QA70f8921. 아래 null/TBD·후보/미적용 문장은 당시 이력이며 현재 배정으로 해석하지 않는다.

---

# Main16 Runtime 구현 계약

후속 Audio 확정: Field07 탐색·조사·전투 후 복귀는 Where the Earth Breathes다. Voice28개는 기존 Mapping을 유지한다. 전투 음악은 사용자 재확인으로 null/TBD, 목격/Main17 곡은 후보·미적용이다. 아래 미배정 기록은 최초 구현 이력이며 [Audio 마무리 계약](../00_프로젝트/Main16_Audio_마무리_계약.md)이 최신 정책이다. Quest/World/Story Encounter 구조는 변경하지 않는다.

2026-10-03 사용자 요청에 따라 [정식 Story 설계](Chapter2_Main16_재_속의_형상.md)를 구현한다. 기존 Main13~15 대사·Voice·편성 정책과 직전 BGM/Background/Player Asset 작업은 변경하지 않는다.

Quest ID는 `main_16_shape_in_the_ash`, Scene은 `Field_07_AshenReach`, 표시명은 재바람 황야다. Main15 완료 뒤 레온 대화 종료로 시작한다. 아래 목표는 기존 QuestDefinition의 순차 목표로 구현하며 모든 count는 1이다.

| # | Objective ID | 종류 / Target ID |
| --- | --- | --- |
| 1 | talk_to_leon_about_ash | TalkToNpc / arbel-leon |
| 2 | head_deeper_west | ReachLocation / field06_main16_west_exit |
| 3 | enter_ashen_reach | ReachLocation / field07_main16_entry |
| 4 | inspect_moving_ash | Interact / field07_main16_ash |
| 5 | trace_ground_vibration | Interact / field07_main16_vibration |
| 6 | inspect_broken_tracks | Interact / field07_main16_tracks |
| 7 | witness_ember_wraith | ReachLocation / field07_main16_witness |
| 8 | defeat_ember_wraith | DefeatEncounter / field07_main16_ember_wraith |
| 9 | inspect_afterimage | Interact / field07_main16_afterimage |
| 10 | find_western_route | ReachLocation / field07_main16_canyon (현장 확인 대화 종료) |
| 11 | return_to_arbel | ReachLocation / arbel_main16_return |
| 12 | report_the_anomaly | TalkToNpc / arbel-leon |

보상 수치는 정식 설계에 미확정이므로 신규 Quest 보상은 빈 RewardBundle로 두고 기존 전투 보상만 적용한다. 완료 보상을 임의로 확정하지 않는다. 별도 TurnIn 목표 없이 12번 대화 종료로 완료한다. Main17·협곡 내부·Overheat·새 Boss·Voice/BGM 생성·Asset 변경은 범위 밖이다. Field07 BGM은 미배정 상태를 유지한다.

## World / Encounter

Field06의 실제 서쪽 Exit와 Field07 동쪽 Exit를 기존 SceneTransitionTrigger/Service로 연결한다. Spawn은 각각 `Spawn_From_Field07`(-7,0), `Spawn_From_Field06`(7,0), Trigger는 맵 경계 x±10.25로 분리한다. 새 Field07은 기존 Field06 Scene 구조/로컬 환경 Sprite를 재사용하고 별도 Additive Scene에서 작성한 뒤 닫아 원래 활성 Scene·선택·창 포커스를 유지한다. Bounds 21×15와 카메라 viewport 제한을 유지하고 동쪽 실제 출구만 개방한다. 협곡은 서쪽 경계 안의 조사 지점이며 새 출구가 아니다.

Field07에는 새 Safe Zone이 없다. 기존 마지막 활성 거점 Arbel을 유지하며 패배는 기존 Party Defeat→Safe Zone 경로를 따른다. 일반 조우는 낮은 밀도의 그을음들개+열풍매, 균열도마뱀+화열딱정벌레 두 배치다. Story 전투는 기존 `ember_wraith` 한 개체, 후열·공용 Burn·기존 AI 그대로다.

목격은 지면 조사→약한 진동→재 모임→불씨→연기/재의 형체 순서의 수동 Next 대화와 공식 Sprite 활성화로 표현한다. 일반 Idle Spawn으로 먼저 노출하지 않는다. 목격 완료만 7번을 진행한 뒤 8번 Story Battle에 진입한다. 기존 일반 Story Battle은 도망을 허용하므로 도망 허용·목표 유지·재도전 정책을 재사용한다. 패배도 목표를 진행하지 않는다. 지정 Story ID 승리만 8번을 진행한다.

사용자 요청의 첫 Encounter 완료 후 조건에 따라 일반 Ember Wraith 배치 한 개는 8번 지정 승리 완료부터 활성화한다. Save/Continue에서도 Quest count로 재구성한다. 반복 일반 승리는 Story ID를 갖지 않아 Main16 목표를 진행하지 않는다. 불씨망령에는 Beast Definition이 없으므로 분양·펫 Unlock은 추가하지 않는다.

## Path / Party / Save

Hearing 판별은 정식 `path.hearing` Stable ID다. 재·진동·잔향·협곡 장면에서 Hearing Player가 첫 관찰, 세린이 교차 확인/해석을 맡고 Default는 세린이 먼저 공유한다. 분기 ID는 `main16_{scene}_{hearing|default}_{speaker}_{nn}`로 분리한다. 공통 대사도 고정 ID를 부여하고 Player는 기존 `player` 무음 대사를 쓴다. 모든 분기는 같은 Quest 결과로 수렴한다.

세린은 현장 Story NPC로 표현하며 저장 Party/Formation이나 전투 참가자를 변경하지 않는다. 태온·미엘·폴의 대화 참여도 전투 편성과 독립적이다. Battle은 현재 저장 Party 그대로다. 기존 QuestProgress의 고정 Quest/Objective ID·count와 Player Path, Scene/Position, Safe Zone만 재사용한다. 첫 등장/일반 Spawn Unlock/협곡 발견은 순차 count에서 파생하며 불필요한 Save Flag·버전 변경을 추가하지 않는다. 중간 대화 취소/Scene 제거는 완료 콜백을 실행하지 않아 재개 시 해당 조사부터 다시 읽는다.

모든 주요 Target은 기존 QuestNavigationTarget에 등록하고 다른 Scene에서 조사/귀환 목표를 추적할 때는 해당 방향의 실제 출구를 안내한다. Save·Party·Settings는 격리된 감사 경로에서만 검증하고 foreground는 사용하지 않는다.
