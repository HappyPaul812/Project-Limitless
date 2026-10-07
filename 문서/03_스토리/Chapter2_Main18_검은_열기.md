# Chapter 2 Main18 — 검은 열기

2026-10-08 사용자 승인 정식 설계. 아래 계약은 이전 Main18/Overheat/작열 감시자 후보·TBD를 대체한다. 구현 완료 판정은 Main18_Final_QA.md를 따른다. Main19/20 및 Cleanse II는 구현하지 않는다.

## 구현 정본 조사와 최소 기술 경계

- 시작 HEAD: 429428748b610e8ad8c8f4b3eefe590f3b48ca31. 사용자 원격 기준과 동일하며 미커밋 LOCAL 변경을 보존한다.
- 기존 일반 독은 TakeDamage(raw, applyDefending:false)로 상태 고유 피해를 준다. 과열도 별도 Stack API에서 이 HP 경계를 사용하며 Direct/DoT/Path/보호 계산에 진입하지 않는다.
- HarmfulStatusType은 기존 네 상태만 유지한다. 과열은 독립 Dictionary로 보관해 기존 정화 및 네 해제약 계약을 보호한다.
- Arbel은 StarterVillageGeneralShop을 재사용한다. 아르벨 전용 카탈로그를 조건부 연결하며 시작마을 원본은 변경하지 않는다.
- 납품 Manifest는 UTF-8로 읽으면 한글 Quest/지역명과 영문 자산 계약을 확인할 수 있다. VFX는 단일 PNG이며 peak/release 지정이 없어 짧은 단일 이미지 표시를 사용한다.
- 무료 냉각약 지급은 보고 대화 완료 때 수용량을 먼저 검증하고 Inventory+Objective를 같은 저장 경계에서 확정한다. 수용 불가 시 목표를 유지해 재시도한다. 판매 해금은 사용자 계약대로 Main17 완료 또는 Main18 시작부터이며 준비 완료와 Field09 출입 해금은 보고 완료 이후다.

2. Main18 정식 기본 계약
==================================================

Quest:

Main18 「검은 열기」

Quest ID:

main_18_black_heat

Scene:

Field_09_ObsidianScar

지역 표시명:

흑요석 상흔

선행 Quest:

main_17_red_rift

Main17 완료 후 이어집니다.

완료 보상:

EXP 70
탈렌트 60
완료 Item Reward 없음

아래에서 별도로
냉각약 3개를 한 번 지급합니다.

Main18의 역할:

- Chapter2 심부 진입
- 흑요석/내부 열기 지역 첫 진입
- Overheat 정식 도입
- 일반 몬스터에서 가볍게 학습
- Elite 작열 감시자에서 실제 대응 시험
- Main19 열/진동 중심부 Hook

Main20 Boss의 시각적/전투적 클라이맥스는
이번에 소비하지 않습니다.

==================================================
3. Main18 Story 흐름
==================================================

Main17에서 더 깊은 길을 확인했지만
준비 없이 들어가지 않기로 했습니다.

Main18은:

Main17 완료
→ 아르벨 귀환
→ 레온에게 붉은 균열 심부 보고
→ 조사용 냉각약 3개 지급
→ 아르벨 잡화점 냉각약 판매 해금
→ Field08 붉은 균열 서쪽 심부로 재진입
→ Field09 흑요석 상흔 진입
→ 검게 굳은 지면 조사
→ 열기 분출구 조사
→ 흑요석 갑충과 첫 Overheat 전투
→ Overheat 원리 확인
→ 더 깊은 진동 추적
→ 작열 감시자 목격
→ Elite 전투
→ 감시자를 쓰러뜨려도 열/진동이 남음을 확인
→ 더 깊은 중심부 길 확인
→ Main19 Hook

입니다.

작열 감시자를
모든 화염 현상의 원인으로 확정하지 않습니다.

오히려:

"중심부를 지키거나 그 영향을 받아 변한 수문장"

정도로 보입니다.

==================================================
4. Quest Objective 정식 계약
==================================================

총 13개.

1.
objectiveId:
return_to_arbel

type:
ReachLocation

target:
arbel_main18_return

내용:
아르벨로 돌아가세요.

--------------------------------------------------

2.
objectiveId:
report_to_leon

type:
TalkToNpc

target:
arbel-leon

내용:
레온에게 붉은 균열 안쪽 상황을 보고하세요.

이 목표 완료 시:

냉각약 3개 지급
아르벨 냉각약 판매 해금

한 번만 실행.

--------------------------------------------------

3.
objectiveId:
reach_deep_route

type:
ReachLocation

target:
field08_main18_deep_route

내용:
붉은 균열의 더 깊은 길로 돌아가세요.

--------------------------------------------------

4.
objectiveId:
enter_obsidian_scar

type:
ReachLocation

target:
field09_main18_entry

내용:
흑요석 상흔에 들어가세요.

--------------------------------------------------

5.
objectiveId:
inspect_obsidian_ground

type:
Interact

target:
field09_main18_ground

내용:
검게 굳은 지면을 조사하세요.

--------------------------------------------------

6.
objectiveId:
inspect_heat_vent

type:
Interact

target:
field09_main18_heat_vent

내용:
열기가 새어 나오는 틈을 조사하세요.

--------------------------------------------------

7.
objectiveId:
defeat_obsidian_beetle

type:
DefeatEncounter

target:
field09_main18_beetle_tutorial

내용:
흑요석 갑충을 물리치세요.

--------------------------------------------------

8.
objectiveId:
inspect_overheat_residue

type:
Interact

target:
field09_main18_overheat

내용:
전투 뒤 남은 열기를 확인하세요.

--------------------------------------------------

9.
objectiveId:
follow_deep_vibration

type:
Interact

target:
field09_main18_vibration

내용:
더 깊은 곳에서 이어지는 진동을 확인하세요.

--------------------------------------------------

10.
objectiveId:
witness_scorching_watcher

type:
ReachLocation 또는
현재 Main17의 witness 구현 방식과 동일한 안전한 구조

target:
field09_main18_watcher_witness

내용:
앞을 막고 있는 존재를 확인하세요.

목격 Dialogue 완료 전
다음 목표로 넘어가지 않습니다.

--------------------------------------------------

11.
objectiveId:
defeat_scorching_watcher

type:
DefeatEncounter

target:
field09_main18_watcher

내용:
작열 감시자를 물리치세요.

--------------------------------------------------

12.
objectiveId:
inspect_after_watcher

type:
Interact

target:
field09_main18_after_watcher

내용:
감시자가 쓰러진 자리를 조사하세요.

--------------------------------------------------

13.
objectiveId:
locate_deeper_route

type:
Interact

target:
field09_main18_deeper_route

내용:
열기와 진동이 이어지는 더 깊은 길을 확인하세요.

완료 시 Quest 완료.

==================================================
5. 아르벨 준비 장면
==================================================

Main18이 시작되면
첫 목표는 아르벨 귀환입니다.

Main17 기존 Quest/완료 동작을
불필요하게 수정하지 마세요.

Main18 Flow가
Main17 완료 상태를 보고 이어받는 구조를 우선합니다.

레온 보고의 핵심 내용:

레온:
"붉은 균열 안쪽까지 들어가셨군요.
상태부터 말씀해 주세요."

Player:
"바위가 검게 굳어 있고,
틈 안쪽에서 열이 계속 올라옵니다."

세린:
"진동도 끊기지 않았어요.
오히려 더 깊은 쪽에서 간격이 짧아졌습니다."

레온:
"그렇다면 준비 없이 다시 들어가면 안 되겠군요."

레온:
"열이 몸에 남을 때 쓰는 냉각약을 준비했습니다.
세 병은 먼저 가져가세요."

레온:
"더 필요하면 잡화 상인에게 구할 수 있게 해두겠습니다."

문맥을 자연스럽게 하기 위한
아주 짧은 연결 Player 문장은 허용하지만,
핵심 의미를 바꾸지 마세요.

Player는 Voice 없음.

NPC 신규 발화는 TTS_PENDING.

Codex가 TTS API를 호출하거나
임의 음성을 생성하지 않습니다.

==================================================
6. 냉각약 정식 계약
==================================================

Stable Item ID:

item_cooling_remedy

표시명:

냉각약

영문:

Cooling Remedy

Category:

Consumable

MaxStack:

99

BuyPrice:

40 탈렌트

SellPrice:

20 탈렌트

효과:

살아 있는 아군 1명의
Overheat Stack 전부 제거.

과열이 없는 대상에게 사용 시:

"제거할 과열이 없습니다."

또는 프로젝트 공통 어법에 맞는 동일 의미 안내.

이 경우:

- 아이템 소비 0
- 행동 소비 0
- 턴 소비 0

침묵 상태에서도
다른 Item과 동일하게 사용할 수 있습니다.

Poison/Burn/Shock/Silence는 제거하지 않습니다.

기존 네 전용 상태 해제약도
Overheat를 제거하지 않습니다.

기존 Cleanse도
Overheat를 제거하지 않습니다.

==================================================
7. 냉각약 지급 / 판매
==================================================

report_to_leon 완료 시:

item_cooling_remedy ×3

정확히 한 번 지급합니다.

Quest Objective 진행과
Inventory 저장을 이용해
중복 지급되지 않게 하세요.

Save/Continue 후
다시 받을 수 없어야 합니다.

아르벨 NPC:

arbel-shop

은 Main17 완료 또는
Main18 시작 이후부터

기존 판매품을 유지하면서
냉각약을 판매합니다.

현재 Arbel GeneralShop이
StarterVillage Catalog를 재사용한다면
기존 시작마을 Shop 자체를 수정하여
시작마을에서도 냉각약이 보이게 만들지 마세요.

Arbel 전용 Catalog 또는
최소한의 조건부 Catalog 연결을 사용합니다.

기존 회복약/마력 회복약은 유지합니다.

==================================================
8. Overheat 정식 Runtime 계약
==================================================

표시명:

과열

영문:

Overheat

최대 Stack:

3

Stack 1:
직접 패널티 없음.

Stack 2:
직접 패널티 없음.
위험 경고 상태.

Stack 3:
도달 즉시
대상 최대 HP의 8% 피해.

계산:

ceil(MaxHP × 0.08)
최소 1.

피해 적용 직후
Overheat Stack = 0.

3/3 상태를 지속해서 보관하지 않습니다.

==================================================
9. Overheat 피해 분류
==================================================

Overheat Burst는:

- 일반 직접 공격이 아님
- DoT가 아님
- Poison/Burn이 아님

별도의 Overheat 특수 상태 피해입니다.

따라서:

- 공용 방어 적용 안 함
- 철벽 적용 안 함
- 가이아 웰 적용 안 함
- 수호의 맹세 보호 안 함
- 직접 피해 Path 배율 적용 안 함
- DoT 배율/처리 적용 안 함
- 펫의 첫 직접피해 방어 등에 소비되지 않음

HP 0까지 내려갈 수 있으며
전투불능이 가능합니다.

현재 실제 피해 파이프라인에
정확히 맞는 최소 API를 사용하세요.

기존 TakeDamage를 억지로 우회해
다른 시스템을 깨지 마세요.

기존 Special/Status damage 경로가 있다면 재사용하고,
없으면 Overheat 전용 최소 경계를 만드세요.

==================================================
10. Overheat 저장/종료 규칙
==================================================

Overheat는 Battle Runtime 상태입니다.

저장하지 않습니다.

Battle 종료 시 제거.

전투불능 시 제거.

새 Battle 시작 시 0.

도망/패배/승리 후
다음 전투에 남지 않습니다.

SaveVersion을 올리지 마세요.

==================================================
11. Overheat UI
==================================================

Art:

Status_Overheat.png

을 사용합니다.

HUD에서 색이나 아이콘만으로 전달하지 않습니다.

반드시 Text:

과열 1/3
과열 2/3

을 표시합니다.

3 Stack은 즉시 Burst되므로
지속적인 3/3 표시는 없습니다.

상세 팝업 설명:

"열이 축적됩니다.
3중첩이 되면 최대 HP의 8% 피해를 받고 과열이 해제됩니다.
정화로 제거할 수 없습니다."

의 의미를 전달합니다.

2 Stack에서는
명확한 위험 표시가 있어야 합니다.

==================================================
12. Overheat VFX
==================================================

외부 Art Pack의 실제 Manifest를 먼저 확인합니다.

예상:

Overheat_Apply
Overheat_High
Overheat_Burst

Apply:
Stack +1 시 짧게 표시.

High:
2 Stack 도달 시 경고.

Burst:
3 Stack 도달 직전/피해 적용 시 표시.

피해 적용 시점은
VFX peak/release를 Manifest가 명확히 제공하면
그 시점을 따릅니다.

Manifest가 단일 PNG만 제공하면
임의 animation frame을 만들지 않습니다.

==================================================
13. 흑요석 갑충 정식 계약
==================================================

Stable Monster ID:

obsidian_beetle

표시명:

흑요석 갑충

분류:

곤충 / 전열 / 과열 튜토리얼형

Level:

13

HP:

250

Attack:

24

Agility:

8

EXP:

50

Talent:

14

Loot:

없음

Beast/Pet 분양:

이번 구현에서는 불가.

미래 후보로 자동 추가하지 않습니다.

IsBoss:

false

==================================================
14. 흑요석 갑충 행동
==================================================

고정 최소 행동 순환:

행동 1:
열압 분사

단일 대상
Attack 80%
+
Overheat +1

행동 2:
기본 공격

행동 3:
기본 공격

이후 반복.

열압 분사 대상은
현재 공용 단일 Target 규칙을 사용합니다.

과열 Stack이 있는 대상을
강제로 집중하지 않습니다.

따라서 첫 튜토리얼에서는
과도한 강제 압박을 만들지 않습니다.

==================================================
15. 흑요석 갑충 첫 Story Encounter
==================================================

Encounter:

field09_main18_beetle_tutorial

흑요석 갑충 1체.

첫 Overheat 적용 시
한 번만 비모달 방식으로:

"과열은 최대 3중첩입니다.
3중첩 시 최대 HP의 8% 피해를 받고 초기화됩니다.
냉각약으로 과열을 제거할 수 있습니다."

의 의미를 안내합니다.

현재 Battle UI의 기존 메시지/강조 시스템을 우선 재사용합니다.

새로운 대형 Tutorial Modal을 만들지 마세요.

도망/패배:

Objective 유지.

재접촉/상호작용으로 재도전.

승리:

정확히 한 번만 Objective 진행.

==================================================
16. 첫 전투 후 Overheat 설명
==================================================

inspect_overheat_residue에서
다음 의미를 확정합니다.

세린:

"방금 공격은 상처보다
몸에 열을 남기는 쪽이었어요."

Player:

"한 번에 끝나는 열이 아니군요."

세린:

"네.
열이 겹쳐 쌓입니다."

세린:

"세 번째까지 쌓이면 한꺼번에 터져요.
두 겹이 보이면 냉각약을 쓸지 판단하는 게 좋겠습니다."

Player:

"정화로 없앨 수 있는 상태와는 다릅니까?"

세린:

"네.
이 열은 정화로는 빠지지 않습니다."

이 대화는
게임 규칙과 정확히 일치해야 합니다.

==================================================
17. 작열 감시자 정식 계약
==================================================

Stable Monster ID:

scorching_watcher

표시명:

작열 감시자

분류:

Elite / 비인간형 수문장형

Level:

14

HP:

560

Attack:

28

Agility:

11

EXP:

85

Talent:

22

Loot:

없음

Beast/Pet:

불가

IsBoss:

false

중요:

Boss가 아닙니다.

따라서 기존 공용 규칙대로
도망 가능합니다.

Boss 전용 BGM 사용 안 함.
Boss 전용 UI를 강제하지 않습니다.

==================================================
18. 작열 감시자 행동
==================================================

4행동 순환.

행동 1:

열압 주입

단일 대상
Attack 85%
+
Overheat +1

--------------------------------------------------

행동 2:

작열 압박

단일 대상
Attack 105%
+
Overheat +1

--------------------------------------------------

행동 3:

열기 파동

생존 아군 전체
Attack 55% 직접 피해

Overheat 부여 없음.

--------------------------------------------------

행동 4:

열압 주입

단일 대상
Attack 85%
+
Overheat +1

이후 반복.

==================================================
19. 작열 감시자 Target 규칙
==================================================

Overheat를 부여하는 행동은:

살아 있는 아군 중
현재 Overheat Stack이 가장 높은 대상을 우선합니다.

동률이면
기존 안정적인 Target 선택 규칙을 사용하세요.

별도의 랜덤 결과로
QA가 흔들리지 않게
현재 프로젝트의 deterministic 정책을 우선합니다.

목적:

1 Stack
→ 2 Stack 경고
→ 냉각약 사용 또는 공격 지속 선택
→ 방치 시 Burst

를 실제로 경험할 가능성을 높입니다.

광역 열기 파동에는
Overheat를 붙이지 않습니다.

==================================================
20. 작열 감시자 Story Encounter
==================================================

Encounter:

field09_main18_watcher

도망:
가능.
Objective 유지.

패배:
Objective 유지.
기존 패배 복귀 정책.

승리:
정확히 한 번만 진행.

중복 승리 보상/Objective 진행 금지.

전용 BGM 없음.

Blade and Gambit 사용.

==================================================
21. 작열 감시자 목격 대화
==================================================

핵심:

세린:

"저 앞의 개체,
주변 열기가 움직임에 맞춰 올라갑니다."

세린:

"앞의 갑충보다
열을 모으는 속도가 훨씬 빨라요."

Player:

"지나가려면 상대해야겠군요."

활성 Party 동료가 있을 경우
아래 Flavor를 조건부로 한 줄씩 넣을 수 있습니다.

태온:

"한 사람에게 열이 몰리지 않게
상태를 계속 확인하겠습니다."

미엘:

"두 겹까지 쌓인 사람부터 보세요.
필요하면 바로 식혀야 합니다."

폴:

"광역 열기와 과열은 별개군요.
숫자부터 놓치지 않으면 되겠습니다."

조건부 동료가 없어도
Story 이해와 진행에는 문제가 없어야 합니다.

==================================================
22. 작열 감시자 전투 후
==================================================

inspect_after_watcher 핵심:

세린:

"쓰러뜨렸는데도
주변 열기가 줄지 않아요."

Player:

"이 녀석이 원인은 아니군요."

세린:

"네.
원인이라기보다 수문장에 가까워 보여요."

세린:

"진동도 계속 더 깊은 곳에서 올라옵니다."

==================================================
23. Main18 마지막 Hook
==================================================

locate_deeper_route:

세린:

"진동이 저 아래에서 올라옵니다."

세린:

"이제 중심부가 멀지 않은 것 같아요."

Player:

"다음에는 더 깊이 들어가야겠군요."

이후 Main18 완료.

Main19는 구현하지 않습니다.

서쪽의 더 깊은 통로는
현재 닫힌 상태로 유지합니다.

게임 내 안내는:

"더 깊은 열기 때문에 지금은 지나갈 수 없습니다."

정도의 세계관 문구를 사용하고

"Main19 미구현"

같은 개발 문구를 사용자에게 보여주지 않습니다.

==================================================
24. Path Reactive 관찰
==================================================

follow_deep_vibration에서
5 Path별 Player 관찰 한 번을 넣습니다.

Story 결과와 Objective는 동일합니다.

NPC 핵심 정보도 동일합니다.

Player Voice 없음.

시각의 길:

"붉은빛이 강해지는 균열과
지면이 흔들리는 위치가 이어져 있습니다."

청각의 길:

"낮은 울림이 두 번씩 반복됩니다.
서쪽으로 갈수록 간격이 짧아집니다."

지적의 길:

"열기 분출과 진동의 간격이 일정합니다.
무작위 현상은 아닌 것 같습니다."

지체의 길:

"깊은 균열 사이에도
무게를 버틸 수 있는 검은 판들이 이어져 있습니다."

마음의 상처의 길:

"갑작스러운 열기가 반복되지만
패턴을 알고 나니 다음 분출을 예상할 수 있습니다."

그 뒤 공통 세린:

"저도 같은 방향을 보고 있었어요.
더 깊은 곳으로 갈수록 반응이 빨라집니다."

Path는 정보상의 Flavor이며
진행 필수 능력이 아닙니다.

==================================================
25. Field09 Geometry
==================================================

Scene:

Field_09_ObsidianScar

기본 Bounds:

21 × 15

Main17/Field08의 실제
World Bounds / Camera / Boundary 정책을 그대로 따릅니다.

대략적 핵심 배치:

Entry:
(7.2, 0)

검은 지면 조사:
(5.2, 1.0)

열기 분출구:
(3.0, -2.0)

Tutorial Beetle:
(1.2, -0.6)

전투 후 조사:
(0.0, -1.2)

깊은 진동:
(-2.2, 2.0)

Watcher 목격/전투:
(-4.4, 0.5)

Watcher 후 조사:
(-5.2, -1.2)

더 깊은 길:
(-7.4, 0.0)

정확한 Collider/배치값은
기존 Scene 안전 규칙에 맞춰 소폭 조정 가능하지만,
Quest 동선 순서는 바꾸지 마세요.

==================================================
26. Field09 연결
==================================================

동쪽:

Field08 ↔ Field09 왕복.

Main18 report_to_leon 완료 전에는
Field08 서쪽 깊은 통로를 열지 않습니다.

준비 전 접근 시:

"더 깊은 곳은 열기가 너무 강합니다.
아르벨에서 준비를 마쳐야 합니다."

정도의 안내.

report_to_leon 완료 후
Field08→Field09 전환 해금.

Field09 동쪽으로 돌아가면
Field08 복귀.

서쪽:

Main19용 통로 위치만 남기고
현재 전환 없음.

Boundary는 닫힘.

==================================================
27. Field09 Optional 일반 조우
==================================================

Quest Story Encounter와 겹치지 않게
우회 가능한 일반 조우 3개를 둡니다.

A.
Obsidian Beetle 1체

대략:
(4.0, -4.6)

B.
기존 Fissure Lizard 1체

대략:
(-0.5, 4.7)

C.
기존 Ember Wraith 1체

대략:
(-2.5, -4.6)

정확한 위치는
Collision/Quest 동선에 맞춰 조정 가능합니다.

기존 일반 Field Respawn 정책을 재사용합니다.

Story Tutorial Beetle과
일반 Beetle 승리는 구분합니다.

==================================================

## Art·Audio·Save·검증 승인 계약

28. Main18 Art 원본
==================================================

외부 Art 담당 납품:

F:\Downloads\Limitless_Main18_ObsidianScar_Art_Pack.zip

원본 ZIP 수정 금지.

먼저 SHA256 / 파일 목록 / Manifest를 기록하고
임시 위치에 압축 해제하여 전수 감사하세요.

예상:

Environment/
Monsters/
UI/
VFX/
Manifest.txt

실제 ZIP이 다르면
실제 파일을 정본으로 감사하고
차이를 기록합니다.

==================================================
29. Art 전수 감사
==================================================

모든 PNG에 대해:

- decode
- pixel size
- RGB/RGBA
- alpha
- transparent background
- black rectangle
- white fringe
- watermark/text
- crop
- empty padding
- duplicate
- filename
- Manifest 일치

검사.

원본 PNG:

리사이즈 금지
재생성 금지
색보정 금지
Crop 금지

Unity Import/Material/Scale로 대응합니다.

치명적 문제가 있으면
임의 그림을 만들지 말고
ART_BLOCKER로 기록합니다.

==================================================
30. Environment Art
==================================================

예상 자산:

ObsidianScar_Ground_Base
ObsidianScar_Ground_Cracked_01
ObsidianScar_Ground_Cracked_02
ObsidianScar_Obsidian_Slab_01
ObsidianScar_Obsidian_Spire_01
ObsidianScar_Fissure_Edge
ObsidianScar_Fissure_Glow
ObsidianScar_HeatVent_01
ObsidianScar_AshPatch_01
ObsidianScar_Cliff_Blocker_01

실제 Manifest 기준으로 사용.

Field09은:

검은 지면
흑요석
굳은 재
깊은 틈
약한 내부 적열

이 주인공입니다.

용암 바다 금지.

화면 전체 빨강/주황 금지.

Main20 시각 클라이맥스 선소비 금지.

==================================================
31. Monster Sprite Import
==================================================

대상:

Obsidian_Beetle_Sprite_Sheet.png
Scorching_Watcher_Sprite_Sheet.png

기대:

1256×1256
4×4
16 Frames
314×314

Row1 Idle
Row2 Attack
Row3 Hit/Strong/Skill
Row4 Defeat

Point
314 PPU
기존 공식 Chapter2 Monster Pivot
기존 Compression 정책

기존 Chapter2 5종의
실제 Importer를 정본으로 따라갑니다.

원본 우향 유지.

KO 마지막 프레임 유지.

새 Animator Controller를
불필요하게 만들지 않습니다.

==================================================
32. Art Gate
==================================================

파일별:

PASS_IMPORT
PASS_WITH_NOTE
USER_ART_REVIEW_REQUIRED
FAIL_BLOCKING

으로 분류.

핵심 Monster sheet가 FAIL_BLOCKING이면
그 자산을 억지로 사용하지 않습니다.

가능한 다른 시스템 구현/문서화는 완료하되
시각 완료라고 보고하지 않습니다.

==================================================
33. BGM 정식 배정
==================================================

Field09 탐색/복귀:

The Weight of Obsidian

현재 프로젝트에 이미 존재하는
정확한 기존 MP3를 사용합니다.

새 파일 다운로드/생성 금지.

일반 Encounter:
Blade and Gambit

Tutorial Beetle Story Battle:
Blade and Gambit

Scorching Watcher Elite:
Blade and Gambit

전용 Elite BGM 없음.

Iron and Incantation 사용 금지.

Paths of Cracked Earth는
Main19 후보로 보존하고 이번에 연결하지 않습니다.

Chapter2_BGM_정본.md도 갱신합니다.

기존 단일 BGM Source /
Fade / Loop / Mixer 정책 유지.

==================================================
34. TTS
==================================================

이번 Main18 신규 NPC 발화는
Stable Dialogue ID를 부여하고

Main18_TTS_Manifest.csv

또는 현재 프로젝트 관례에 맞는 Manifest를 생성하세요.

상태:

TTS_PENDING

Player:
VoiceExpected=false

지문:
Speaker 없음
VoiceExpected=false

Codex가:

- TTS API 호출
- Voice 생성
- 임의 기존 Voice 연결

을 하면 안 됩니다.

TTS 담당 세션이 별도 작업합니다.

==================================================
35. Stable Dialogue ID
==================================================

현재 실제 프로젝트의
Main16/17 ID 정책을 조사한 뒤
충돌 없는 Main18 prefix를 사용합니다.

기본 prefix:

main18_

NPC/scene/branch가 구분되어야 합니다.

Path Player 문장은
Voice ID가 필요하지 않습니다.

동일 발화를 여러 페이지에
같은 Stable ID로 재사용하지 마세요.

==================================================
36. Save / Continue
==================================================

SaveVersion 변경 금지.

기존 저장 구조 사용.

최소 실제 격리 Save/Continue 검사 지점:

1.
Main18 시작 / 아르벨 귀환 전

2.
레온 보고 완료
냉각약3 지급 후

3.
Field09 입장 후

4.
Tutorial Beetle 승리 후

5.
Watcher 승리 전

6.
Watcher 승리 후

7.
Main18 완료

각 지점에서:

- Quest objective
- Completed Quest
- Scene
- Position
- Inventory
- Cooling Remedy 수량
- Currency
- Party
- Formation
- Beast
- HP/MP
- Path
- Companion Unlock

보존 검사.

Overheat Stack은
Battle 종료 밖으로 저장되지 않아야 합니다.

냉각약 3개 무료 지급은
Continue로 중복되면 안 됩니다.

==================================================
37. 전투 Retry
==================================================

두 Story Encounter:

field09_main18_beetle_tutorial
field09_main18_watcher

모두:

도망
→ Objective 유지
→ 재도전 가능

패배
→ Objective 유지
→ 재도전 가능

승리
→ 한 번만 진행

중복 승리
→ 보상/Objective 중복 0

Watcher는 Boss가 아니므로
도망 금지로 만들지 않습니다.

==================================================
38. Overheat 전수 검증
==================================================

반드시 자동 검증:

0→1
1→2
2→3→0

3 도달 피해:

ceil(MaxHP*0.08)

MaxHP 여러 값으로 검사.

방어:
감소 안 됨

철벽:
감소 안 됨

가이아 웰:
감소 안 됨

수호의 맹세:
보호 안 됨

Path direct multiplier:
적용 안 됨

독/Burn:
독립

Shock/Silence:
독립

Cleanse:
Overheat 유지

Poison Antidote:
유지

Burn Ointment:
유지

Shock Potion:
유지

Silence Remedy:
유지

Cooling Remedy:
전부 제거

Cooling Remedy 대상 Stack0:
소모0 / 행동0

전투 종료:
0

새 Battle:
0

==================================================
39. HUD 검증
==================================================

Stack1:
아이콘 + "과열 1/3"

Stack2:
아이콘 + "과열 2/3"
+ 위험 표시

Stack3:
Burst
→ 0
→ 잔류 3/3 없음

색만으로 전달하지 않음.

상세 Popup에서도
설명이 읽혀야 합니다.

1280×720
1600 계열
1920 계열

기존 대표 해상도 정책으로
잘림/겹침 확인.

==================================================
40. Monster Balance 검증
==================================================

Obsidian Beetle:

Lv13
HP250
Attack24
Agility8
EXP50
Talent14

Scorching Watcher:

Lv14
HP560
Attack28
Agility11
EXP85
Talent22

숫자를 임의 튜닝하지 마세요.

단 실제 계산상
명백한 즉사/무한루프/행동 불가가 확인되면
수정 전에 문서에 증거를 남기고 보고하세요.

기존 Chapter2 Monster 수치는
변경하지 않습니다.

==================================================
41. Quest 보상 검증
==================================================

Mandatory Main18 기준:

Tutorial Beetle Victory:
몬스터 EXP/Talent 정상 지급

Watcher Victory:
몬스터 EXP/Talent 정상 지급

Quest 완료:
EXP70
Talent60

각각 한 번.

도망/패배:
전투 보상 0.

Quest Reward:
완료 전 0.

Cooling Remedy3:
레온 보고에서만 한 번.

==================================================
42. 기존 시스템 보호
==================================================

절대 임의 변경 금지:

- Main01~17 Story
- Main05 수정 Voice
- Main17 Art
- 기존 Chapter2 Monster 수치
- 기존 15개 Player Skill
- 기존 Poison/Burn/Shock/Silence 규칙
- Cleanse 기존 네 상태 제거
- Party
- Formation
- Beast
- SaveVersion
- 기존 BGM 원본
- 기존 Voice
- 사용자 Save
- 사용자 수정 Sprite/PNG
- ThirdParty 원본

unrelated 리팩터링 금지.

==================================================
43. 백그라운드 검증
==================================================

AGENTS.md를 반드시 따릅니다.

모든 자동 검증은 가능한 한:

Command Line / Batch Mode
→ Unity Test Framework
→ 포커스 없는 Unity MCP
→ 로그/산출물 분석

순서.

금지:

- Unity foreground 전환
- Game View 강제 활성화
- OS 포커스 탈취
- 실제 키보드/마우스 조작
- Computer Use로 현재 화면 조작

실제 사람 청취가 필요한 BGM은:

USER_LISTENING_REQUIRED

Art의 미적 판단만 필요한 항목은:

USER_ART_REVIEW_REQUIRED

로 남깁니다.

자동 PASS로 올리지 마세요.

==================================================
44. 문서 및 QA 산출물
==================================================

최소:

문서/03_스토리/Chapter2_Main18_검은_열기.md

문서/00_프로젝트/Main18_Art_Source_Audit.json

문서/00_프로젝트/Main18_Art_Import_QA.md

문서/00_프로젝트/Main18_Balance_QA.md

문서/00_프로젝트/Main18_Final_QA.md

문서/00_프로젝트/Main18_TTS_Manifest.csv

필요한 Runtime 결과 txt/csv/json.

CURRENT_STATUS.md 갱신.

불필요한 중복 문서는 만들지 마세요.

==================================================
