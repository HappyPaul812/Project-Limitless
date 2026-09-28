# Main11 — 침묵의 지하묘지

## 범위와 시작

- Quest ID `main_11_silent_catacomb`, 선행 `main_10_center_of_silence`, 표시명 `침묵의 지하묘지`.
- Main10 완료 후 Field_03의 기존 지하묘지 입구에서 Dungeon_01 B1로 진입할 때 시작한다. 기존 B1/B2 일반 조우와 보스 전투를 재사용한다.
- B1은 실제로 사용된 오래된 묘지다. B2는 사용되지 않은 안치 공간과 정교한 석재·원형 구조·홈·반복 문양이 늘어나는 구역이다. 문양의 의미나 제작자는 밝히지 않는다.
- 진행은 현재 QuestService의 순차 Objective만 사용한다. 조사 대화를 끝내야 상호작용 목표가 진행되고, 역순 방문·반복 상호작용은 진행되지 않는다.

## 순차 목표와 stable ID

| 순서 | Objective ID | 표시 목표 | 타입과 Target ID |
| --- | --- | --- | --- |
| 1 | `explore_b1` | 지하묘지 B1 내부를 살펴보세요. | ReachLocation `dungeon01_main11_b1_central` |
| 2 | `inspect_west_ossuary` | 서쪽 안치실을 조사하세요. | Interact `dungeon01_main11_west_ossuary` |
| 3 | `inspect_collapsed_chamber` | 동쪽 무너진 묘실을 조사하세요. | Interact `dungeon01_main11_east_chamber` |
| 4 | `open_lower_passage` | 북쪽 봉인된 하강로를 여세요. | Interact `dungeon01_main11_lower_gate` |
| 5 | `descend_to_b2` | 열린 하강로로 B2에 내려가세요. | ReachLocation `dungeon01_main11_b2_entry` |
| 6 | `explore_b2` | B2의 안치 공간을 확인하세요. | Interact `dungeon01_main11_b2_entrance` |
| 7 | `inspect_sealed_tombs` | 서쪽 폐쇄 묘실을 조사하세요. | Interact `dungeon01_main11_sealed_tombs` |
| 8 | `inspect_unknown_chamber` | 동쪽 용도불명실을 조사하세요. | Interact `dungeon01_main11_unknown_room` |
| 9 | `trace_blue_flow` | 중앙 석실의 푸른 흐름을 살펴보세요. | Interact `dungeon01_main11_blue_flow` |
| 10 | `defeat_silent_warden` | 침묵의 파수꾼을 쓰러뜨리세요. | DefeatEncounter `dungeon01_b2_boss` |
| 11 | `inspect_sealed_chamber` | 파수꾼 뒤 봉인실을 조사하세요. | Interact `dungeon01_main11_sealed_chamber` |
| 12 | `recover_broken_tablet` | 부서진 석판 조각을 확보하세요. | Interact `dungeon01_main11_broken_tablet` |
| 13 | `leave_silent_catacomb` | 단서를 가지고 지하묘지 입구로 돌아가세요. | ReachLocation `safezone_catacomb_entrance` |

## 조사와 대사

- B1 서쪽: 석관·손상된 묘표·안치 흔적. 동쪽: 붕괴와 비교적 새로운 긁힌 흔적. 양쪽 조사 후 북쪽 장치를 움직일 수 있다. 새 퍼즐이나 수집 퍼즐은 없다.
- B2 서쪽·동쪽·중앙을 차례로 확인한다. 중앙의 푸른 흐름은 방에서 생성되지 않고 더 안쪽 또는 아래에서 유입되는 듯하다. 실제 근원은 모른다.
- 태온은 차분한 존댓말로 관찰→판단→보호한다. 폴은 사실과 추측을 구분하는 친근한 존댓말과 가벼운 농담을 쓴다. 미엘은 상태와 환경을 살피는 부드러운 존댓말을 쓴다. 폴과 미엘의 편안해진 대화는 짧게만 두며 연애나 보호자 관계로 확정하지 않는다.
- 보스는 쓰러질 때 무너지며 정지하고 몸의 빛은 약해진다. 주변 푸른 흐름은 유지된다. 이 존재가 빛의 근원은 아니라는 관찰만 가능하다.
- 뒤쪽 작은 봉인실에는 파손된 석제 구조·아래로 이어지는 균열·반복 문양·푸른빛이 있다. 빛은 아래에서 올라와 구조물을 통과하는 듯하다. 다른 보스·일반 몬스터·보물 상자는 두지 않는다. 아래로 통행하거나 근원을 단정하지 않는다.

## 석판, 귀환, 보상

- `quest_broken_catacomb_tablet` 「부서진 석판 조각」은 Quest Item이다. 읽을 수 없지만 던전에서 본 문양과 같다. 기존 Inventory에 한 번만 넣고 저장한다. 판매·버리기·전투 사용·장비는 불가하며 Main11 완료 뒤에도 유지한다. 퀘스트 보상 품목은 아니다.
- 석판 획득 후 기존 대화 선택으로 `돌아간다` 또는 `아직 조사한다`를 제공한다. 후자는 현 Scene과 진행을 유지한다. 전자는 Field_03 입구 Safe Zone Spawn으로 이동한다. 강제 자동 이동은 없다.
- 보스 승리로 10번 목표만 끝난다. 보스 후 조사와 석판을 거쳐 Safe Zone에 정상 귀환해야 13번 목표 및 Main11이 완료된다. Boss 전투 패배·도주는 진행하지 않는다. Save/Continue 후에는 목표 진행 기록으로 보스 재도전을 요구하지 않는다.
- 완료 보상 EXP 60·탈렌트 50·아이템 없음. Main10의 40/40보다 높지만 보스 자체 보상 120/35와 별도로 한 차례만 지급하도록 보수적으로 정했다. 기존 QuestService의 완료 기록과 RewardBundle을 사용한다. 별도 Dungeon Checkpoint는 만들지 않고 기존 입구 Safe Zone 전멸 복귀를 유지한다.
- Main12 「남겨진 기록」은 향후 석판 조각과 문양을 밖에서 조사할 방법을 찾는 방향만 남긴다. 이번에는 Main12 Definition·목표·Scene·전체 대사를 만들지 않는다.

## 아직 밝히지 않는 것

푸른빛의 정체·최초 발생원, 묘지를 만든 세력, 문양의 의미, 파수꾼 제작자와 보호 대상, 몬스터 이상 행동의 정확한 원인, 최종 흑막과 신의 의도는 미확정이다.
