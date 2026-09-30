# Chapter 2 도입부 — Main12~14와 아르벨

> 확정된 서사·구현 계약. 실제 구현과 검증 상태는 [CURRENT_STATUS](../00_프로젝트/CURRENT_STATUS.md)를 따른다. Main15, 세린 영구 해금, 후반 사막·던전·보스, 과열, 실제 지역 빠른 이동은 범위 밖이다.

## Main12 「남겨진 기록」

`main_12_remaining_record`는 Main11 완료와 `quest_broken_catacomb_tablet` 보유 뒤 시작한다. 석판에는 지하묘지와 같은 반복 문양이 있고 매우 오래됐다. 기존 시작 마을 잡화 상인 `starter-village-general-shop`은 과거 서쪽 상인이 비슷한 문양의 돌을 가져왔다는 경험만 전한다. 문양의 의미·푸른빛의 근원·서쪽과 침묵/열기의 동일 원인은 확정하지 않는다. 폴은 사실과 추측을 분리하고, 태온은 직접 확인을 권하며, 미엘은 이동할 이유를 정리한다.

| 순서 | Objective ID | 종류·Target ID | 내용 |
| --- | --- | --- | --- |
| 1 | `inspect_broken_tablet` | Interact `field03_main12_tablet` | 석판 재확인 |
| 2 | `ask_western_trader` | TalkToNpc `starter-village-general-shop` | 서쪽 교역 기억 듣기 |
| 3 | `compare_old_marks` | Interact `village_main12_records` | 반복 문양만 비교 |
| 4 | `decide_westward` | Interact `field03_main12_party_decision` | 파티와 이동 결정 |
| 5 | `enter_western_border` | ReachLocation `field04_west_entry` | 서쪽 첫 Field에 도착 |

## Main13 「메마르는 땅」과 세린

`main_13_drying_land`는 Main12 뒤 첫 서부 Field `Field_04_WesternBorder`에서 시작한다. 살아 있는 나무와 마른 잎·풀, 갈라진 땅, 줄어든 물길이 공존하며 서쪽으로 갈수록 건조하다. 완전한 사막은 아니다. 원인은 단정하지 않는다. 세린은 귀의 보조 장치로 말소리와 지면 진동을 인식 가능한 신호로 받아들이고, 의미는 자신의 경험으로 판단한다. 장치는 청력을 완전히 회복하거나 모든 적을 찾는 초능력이 아니다. 연민·극복 서사로 묘사하지 않는다.

| 순서 | Objective ID | 종류·Target ID | 내용 |
| --- | --- | --- | --- |
| 1 | `inspect_dry_soil` | Interact `field04_main13_dry_soil` | 마르는 토양 |
| 2 | `inspect_shallow_stream` | Interact `field04_main13_shallow_stream` | 줄어든 물길 |
| 3 | `meet_serin` | TalkToNpc `field04-serin` | 첫 대화와 장치의 간접 표현 |
| 4 | `defeat_fleeing_beasts` | DefeatEncounter `field04_main13_serin_encounter` | 진동 경고 뒤 공동 전투 |
| 5 | `arrive_arbel` | ReachLocation `arbel_main13_arrival` | 아르벨 도착·안전지대 활성화 |
| 6 | `meet_arbel_warden` | TalkToNpc `arbel-leon` | 지역 책임자와 대화 |

세린은 `companion_serin`, 청각의 길·사수다. 공식 Portrait·Sprite·Animator를 재사용한다. 공동 전투부터 아르벨까지 Story Temporary로 합류하고, Main14에서 재합류한다. 정식 명단·영구 해금은 하지 않는다. 공용 사수 스킬·BeastCompanion을 사용하며 초기 장착은 Fox다. 세린 없이 적을 찾을 수 없는 진행 조건은 두지 않는다. 관리 UI가 정식 명단만 다루면 임시 세린은 관리 목록에서 제외한다.

## 아르벨과 지역 이동

`Arbel`은 생활·상점·치유·보급이 운영되는 거점이다. 지역 책임자 레온 `arbel-leon`, 일반 상점, 무료 치유사, Party Guide, 펫 분양·관리 담당, 서쪽 정보 주민, Portal 관리인을 Quest 완료 후에도 상주시킨다. 분양 담당은 직접 상대해 본 종의 길들여진 개체를 맡긴다고 설명하며, 싸운 개체를 그대로 판매하는 설정은 쓰지 않는다. 기존 분양·관리 UI와 100 탈렌트 규칙을 재사용한다.

정상 진입 때 기존 `GameSessionData.ActivateSafeZone`으로 `safezone_arbel` / `Arbel` / `Spawn_Arbel_Center`를 최근 복귀 지점으로 등록·저장한다. 중앙 Spawn은 치유·보급에 가깝고 NPC/Collider·출구 Trigger와 떨어진다. 이후 전멸은 기존 경로로 아르벨에 복귀한다. Portal 공간과 `arbel-portal-keeper`는 준비 상태를 알리는 기본 상호작용만 둔다. 향후 이전 지역 재진입을 레벨로 막지 않으며 실제 Fast Travel은 만들지 않는다. 모든 Field/World에 Bounds·카메라 제한·실제 출구 Opening과 떨어진 Spawn을 둔다.

## Main14 「땅 아래의 울림」

`main_14_ground_pulse`는 Main13 뒤 레온의 의뢰로 시작한다. 서쪽 건조는 오래됐지만 최근 변화 속도가 비정상적으로 빨라졌다. 열기는 지속적이고 진동은 주기적이라는 차이를 관찰하되 동일 원인을 단정하지 않는다.

| 순서 | Objective ID | 종류·Target ID | 내용 |
| --- | --- | --- | --- |
| 1 | `hear_arbel_situation` | TalkToNpc `arbel-leon` | 지역 사정 듣기 |
| 2 | `inspect_old_well` | Interact `arbel_main14_old_well` | 서쪽 우물 수위 비교 |
| 3 | `inspect_irrigation` | Interact `field05_main14_irrigation` | 마른 수로와 그늘의 미열 |
| 4 | `rejoin_serin` | TalkToNpc `field05-serin` | 주기적 진동 공유·임시 재합류 |
| 5 | `inspect_watchpost` | Interact `field05_main14_watchpost` | 풀·돌·열 흔적 |
| 6 | `follow_fleeing_beasts` | DefeatEncounter `field05_main14_fleeing_beasts` | 서쪽에서 도망친 야수 |
| 7 | `feel_strong_pulse` | ReachLocation `field05_main14_strong_pulse` | 파티도 느끼는 강한 진동 |
| 8 | `report_arbel_findings` | TalkToNpc `arbel-leon` | 조사 결과 보고 |

감시초소의 열 흔적은 순간 열 또는 무언가의 이동 가능성만 암시한다. 야수는 동쪽을 습격하러 온 것이 아니라 서쪽에서 도망쳤다. 강한 진동은 현상의 강도 증가만 뜻한다. 보고 뒤에도 원인은 미상이다. 세린은 임시 동행을 유지한다.

## Main13~14 Path Retrofit 후보

**DESIGN RETROFIT CANDIDATE / 미구현.** [Path 반응형 Story 정본](Path_반응형_Story_연출_규칙.md)에 따라 아래 장면을 향후 별도 수정한다. 현재 본문·ID·Runtime·Voice는 그대로 유지한다. 분기 확정 뒤 ID/Manifest와 영향을 받는 음성 검수 범위를 다시 확인한다.

| Main / 기존 장면·대사 ID | Hearing Player 후보 | 일반 Player / 보존 정보 |
| --- | --- | --- |
| Main13 `meet_serin` / `main13_serin_001`~`003` | 장치·신호를 알아보는 짧은 반응, Player의 첫 관찰 또는 거의 동시 멈춤을 검토한다. 장치를 처음 보는 사람처럼 설명하지 않는다. | 기존 세린의 장치/경험 소개와 적의 방향 단서를 공유한다. 주변 적의 존재·공동 전투 목표는 동일하다. |
| Main14 `rejoin_serin` / `main14_serin_001` | Player가 서쪽 진동 증가·반복을 먼저 제시하고 세린이 확인·보완한다. | 세린의 기존 발견/공유를 유지한다. 열기·진동의 인과관계는 미상이다. |
| Main14 `feel_strong_pulse` / `main14_serin_002`~`003`, `main14_taeon_002` | Player의 첫 진동 관찰 또는 동시 반응 뒤 세린이 방향·강도를 확인한다. | 파티도 강한 진동을 느끼는 현상과 서쪽에서 도망친 야수 정보를 유지한다. 정확한 원인은 확정하지 않는다. |

대사 ID는 현재 제작 Manifest와 연결된 참조 키다. 위 후보가 구현됐다는 뜻이 아니며 대사 변경/신규 분기에 같은 ID를 덮어쓰지 않는다.

## 공통 구현 경계

모든 주요 목표는 기존 Quest Navigation Target에 연결한다. 임시 동행 여부는 Quest 단계로 재구성하고 새 Save System이나 영구 해금을 만들지 않는다. 전투 승리만 DefeatEncounter 목표를 진행한다. 기존 World Transition·SceneSpawnPoint·Safe Zone·Save 구조를 사용한다. 실제 Runtime/Visual QA는 별도 작업이다.
