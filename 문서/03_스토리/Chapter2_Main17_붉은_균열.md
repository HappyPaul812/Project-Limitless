# Chapter 2 Main17 — 붉은 균열

2026-10-06 사용자 요청으로 확정한 설계다. 구현 완료 여부는 `MAIN17_RESUME_CHECKPOINT.md`를 따른다. LOCAL Main15/16와 현재 코드가 정본이며 이전의 Main17 후보/미구현 표시는 이 문서로 후속 확장한다.

## 기존 계약과 충돌 해결

세린은 Main15 `welcome_serin` 완료로 이미 정식 해금되었다. 사용자 요청의 첫 만남/후속 영구 합류와 충돌한다. 요청에 명시된 LOCAL 우선 규칙에 따라 해금을 취소하거나 다시 만들지 않는다. Main17의 `meet_serin`은 협곡에서 조사 중인 세린과 합류하는 현장 만남이다. 기존 공식 Portrait/Animator, Hearing/Sharpshooter, 기본 Fox, 기존 사수 3스킬을 사용한다. 대화용 현장 Actor는 저장 Party와 독립적이며 편성/Formation을 강제 변경하지 않는다. Story Battle 역시 저장 Party를 유지한다. 세린 전투 참여는 기존 안내인에서 사용자가 선택한다.

Chapter2 전투 BGM은 최신 Main16 Audio 계약/현재 FindBattle 기준 null/TBD다. 요청의 ‘기존 해당 Battle BGM’은 이 상태를 유지한다. Field08 탐색은 실제 `F:\Downloads\bgm\Beneath_The_Cracked_Earth.mp3`를 연결하고 복귀 때 재생한다. 새 전투곡을 임의 배정하지 않는다.

## Quest 계약

Quest `main_17_red_rift`, 제목 「붉은 균열」, Scene `Field_08_RedRift`, 지역 붉은 균열 협곡. Main16 완료가 선행조건이다. Field07 서쪽 협곡 진입 때 시작하며 활성 Main을 덮어쓰지 않는다. 완료 보상은 Main16과 같이 빈 RewardBundle로 유지하고 전투 기존 보상만 제공한다.

| 순서 | Objective ID | 종류 | Target ID / 현장 위치 |
| --- | --- | --- | --- |
| 1 | enter_red_rift | ReachLocation | field08_main17_entry / (7,0) |
| 2 | inspect_first_crack | Interact | field08_main17_crack / (4,0) |
| 3 | follow_vibration | Interact | field08_main17_vibration / (2,-2) |
| 4 | inspect_animal_tracks | Interact | field08_main17_tracks / (1,3) |
| 5 | meet_serin | Interact | field08_main17_serin / (0,1) |
| 6 | compare_findings | Interact | field08_main17_compare / (-1,1) |
| 7 | investigate_deep_resonance | Interact | field08_main17_resonance / (-3,-2) |
| 8 | witness_new_threat | ReachLocation | field08_main17_witness / (-4,0), 목격 대화 완료 |
| 9 | defeat_new_threat | DefeatEncounter | field08_main17_threat / 같은 지점에서 재도전 |
| 10 | inspect_after_battle | Interact | field08_main17_after / (-4,-1) |
| 11 | locate_deeper_route | Interact | field08_main17_route / (-7,0) |
| 12 | decide_to_withdraw | Interact | field08_main17_withdraw / (-7,1) |

count는 모두1, 순차 진행한다. 조사 취소/중간 Scene 종료는 완료 콜백을 실행하지 않는다. 도망/패배는9번 유지, 지정 승리만 진행한다. Continue 뒤 재도전은 수동 입력으로 즉시 재전투 루프를 방지한다.

## Story / Path / 참여자

더 강한 신호를 따라 협곡 진입→균열/열/짙은 재/갈라지는 동물 흔적 조사→세린과 기존 황야 관찰 비교→깊은 반복 신호→균열 주변 위협과 전투→형체를 없애도 진동 지속→깊은 방향의 추가 반응→안전한 준비 없이 내려가지 않기로 결정한다. Main18은 열기/흑요석 심부로 이어질 Hook만 남기며 Quest/원인/세력/보스/Overheat를 만들지 않는다.

Hearing `path.hearing`은 Player 첫 관찰→세린 교차 확인, Default는 세린 공유→Player 비교/결정이다. 다른 네 Path는 공간/기록 순서/바닥과 경사/긴장 관찰 수준의 Player 관점만 추가하고 결과는 같다. 태온/미엘/폴의 대사는 실제 활성 Party에 있을 때만 포함한다. 세린은 현장 공동 조사자로 미편성 상태에서도 핵심 단서를 공유한다. `<지문>`은 빈 Speaker·Portrait/Voice 없음이다.

Stable ID `main17_{scene}_{hearing|default|공통}_{speaker}_{nn}`로 구분한다. 모든 신규 NPC 발화는 TTS_PENDING, Player와 지문은 VoiceExpected=false. 새 WAV나 다른 대사의 Clip을 연결하지 않는다. 확정 본문 Manifest는 Dialogue 구현 단계에서 추출한다.

## Field / 전투 / Save

Field08은 기존21×15 Bounds/Camera/공식 환경 Sprite를 재배치·Tint한다. PNG 원본은 변경하지 않는다. 붉은 흙·검게 식은 암석·좁은 균열과 약한 붉은 틈·짙은 재를 표현하고 용암 바다/최종 보스 지역은 만들지 않는다. 동쪽 Field07 왕복, 중앙 초기 조사, 북/남 우회 조사, 서쪽 깊은 균열. 서쪽 외곽은 닫힌 후속 경계다. 실제 출구만 Boundary Opening, Spawn±7과 Trigger±10.25 분리·기존 전환 grace를 유지한다. Field07 기존 Scene 파일은 보존하고 Runtime에서 서쪽 Opening을 설치한다.

새 몬스터/수치/Art/Elite는 이번 최소 구현에서 추가하지 않는다. 기존 곡선과 공식 5종을 보호하며 Phase5에서 실제 곡선을 확인해 Story는 기존 균열도마뱀1체(HP190·공격12), 일반 조우는 우회 가능한 화열딱정벌레1체(HP220·공격9, 4,-4.5)로 확정했다. 기존 Main16 망령HP150에 비해 Story HP는 약27% 증가하고 기존 울림/돌진 기믹을 재사용한다. 최초2체후보 합계HP410은 과도하여 적용하지 않는다. 기존 몬스터/스킬/상태/경제 수치는 변경하지 않는다. Guardian15 기본공격 전용 초기2체 감사 실패는 Temp에 보존했으며 연결 QA는 기존 Main16 Fighter20 조건으로 수행한다.

Save Version/새 Flag를 추가하지 않는다. Quest ID/count/CompletedIds·Scene/Position·기존 Party/Formation·Encounter 상태에서 재구성한다. 사용자 Save는 검증에 사용하지 않고 격리 슬롯을 사용한다. 일반 조우 승리/Story 완료는 구분한다.

## 단계별 구현·검증 계획

1. 이 설계·현재 상태·Checkpoint 확정.
2. QuestDefinition12목표, Main16 prerequisite, ID 중복/순서/빈 보상 확인.
3. 새 Field08·07↔08 연결/잠금/Bounds/Spawn/BuildSettings, 기존 Scene 보존 확인.
4. 현장 세린/공식 Art/공동 조사와 수동 Dialogue 완료 진행, 저장 편성 보존.
5. 실제 Story factory·지정 승리/도망/패배/복귀 검증.
6. Hearing/Default·다른 네 Path·활성 동료·지문/StableID/Manifest 확인.
7. 격리 Save/Continue 각 중간 목표·완료·위치·편성/Beast 보존.
8. 실제 로컬 BGM 복사·Catalog·Scene/전투/Continue 단일 재생.
9. 대표 전체 흐름·기존 Main16/전투 메뉴 보호·Compile/Console·diff 확인.

각 단계의 관련 파일만 commit, Push0. 기존812PASS와 전투 메뉴482PASS는 재사용하고 실제 변경 영향만 재검증한다. USER_INPUT_REQUIRED(물리 입력/실제 청취), DEFERRED_FEATURE(일반 장비), DEFERRED_RELEASE_VALIDATION(Standalone)은 이전 결과와 분리하여 유지한다. 작업 완료 전 Runtime PASS를 추정하지 않는다.

## Phase7 저장 검증 경계

Save Version1/일반 저장 코드는 변경하지 않았다. 실제 격리 Save→Bootstrap Continue 5지점(진입 조사중/목격전/목격완료·승리전/승리후/완료)을 검증한다. Scene/좌표·Quest count/Completed·Party/Formation·Beast·Hearing·세린 정식해금·11조사지점 재구성이 동일하다. 도망 공용 복귀 API 제어는 실물 도망 입력 검증과 구분한다. 일반 Field08 갑충은 기존35초 런타임 respawn 정책이며 일반 몬스터 재생성 타이머는 Save JSON에 영구 저장되지 않는다. 같은 Play 세션 Continue의 런타임 상태와 OS재시작 저장을 구분하며 후자는 기존 Release 검증 범위다.

## Phase9 영향 범위 마무리

Field08의 복사 타일 녹색 경계 반복을 제거하고 기존 Sprite/기본 Quad/4종 Material로 붉은 바닥·짙은 재·물리적 깊은 틈·약한 붉은 틈·어두운 암석을 구성한다. 전용 환경 원화 대신 임시 지형을 사용하며 PNG 생성0. 깊은 틈은 (-5,0.5), 폭0.7×높이7, 북쪽 y4.5/남쪽 y-3.5 우회 통로를 유지한다. Field07 원본은 변경하지 않는다. 아르벨/Field06/Field07의 기존 서쪽 출구에 Main17 Navigation Target만 덧붙인다. 같은 게임패드 A 입력이 마지막 Next와 다음 인접 조사에 동시에 전달되지 않도록 완료 프레임의 추가 조사를 막는다. 이 상태는 저장 Flag가 아닌 일시적인 입력 보호이며 새 Play 시작 때 초기화한다.
