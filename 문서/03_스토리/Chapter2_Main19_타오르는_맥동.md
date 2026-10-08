> 2026-10-08 후속: Art Pack13 PNG 정식 반영·Witness/맥동/Continue 기술 검증314PASS. SOURCE_RECEIVED/AUDITED/IMPORTED/RUNTIME_VERIFIED, 최종 미적 USER_ART_REVIEW_REQUIRED13 유지. 기존 ART_PENDING 납품 대기는 해소. 최신 판정은 [Main19 Final QA](../00_프로젝트/Main19_Final_QA.md). 아래 기초 구현/납품 전 기록은 이력이다.

# Chapter2 Main19 — 타오르는 맥동

2026-10-08 사용자 승인 정식 설계. 이전 Main19 후보를 대체한다. 구현 판정은 Main19_Design_QA.md를 따른다. Main20 보스/최종 원인/새 상태·몬스터·상점·동료는 확정하거나 구현하지 않는다.

## 구현 전 기술 계약

Main19 Flow가 Main18 완료 기록을 이어받는다. Field09 원본 Scene과 Main18 Flow는 변경하지 않고 새 FieldConnections/실행 중 서쪽 Bounds opening·완료 Gate로 왕복을 연결한다. 기존 Main18 closed 안내는 Main18 완료 후에만 실행 중 숨긴다. Field10은 기존 Field09 골격의 새 복사본으로 생성하고 원본 Sprite를 임시 재사용한다. 동쪽 Exit만 실제 개방, 서쪽은 물리 Bounds·안내 유지. Spawn7.8와 Trigger10.25는 분리한다.

지정 A/B는 원본 MonsterDefinition을 공용 Factory에서 조합한다. A 갑충전열0/도마뱀전열1, B 갑충전열0/망령후열0. Optional3은 단독. 저장 Party·Formation은 그대로다. 도망·패배 기존 공용 경로, 승리만 Quest 진행1회. A 승리 후 공통 세린 발화는 다음 pulse 조사 첫 페이지로 이어져 별도 목표를 추가하지 않는다. Path5 Player 관찰은 동일 조사 목표로 완료하며 능력 조건이 아니다. Witness 동료3 Flavor는 현재활성파티만 표시, 필수 세린은 Story 안내다.

최종 Art8+VFX2는 ART_PENDING/USER_ART_REVIEW_REQUIRED. 먼 실루엣은 기존 열기 장식의 명시적 임시 배치이며 새Boss외형을 확정하지 않는다. NPC TTS_PENDING·Player/지문 VoiceExpected=false, 음성Catalog수정/API0. SaveVersion1·새필드0.

## 사용자 승인 정본 계약

1. Main19 정식 기본 계약
==================================================

Quest명:
Main19 「타오르는 맥동」

Quest ID:
main_19_burning_pulse

Scene:
Field_10_BurningPulse

표시명:
맥동의 열맥

선행 Quest:
main_18_black_heat

완료 보상:
EXP 80
탈렌트 70

Main19의 역할:
- Main18 이후 심부 진입
- 과열 시스템의 숙련 구간
- 혼합 조우에서 냉각약/과열 관리 경험
- 거대한 화염 존재의 흔적 조사
- Main20 보스전 직전 긴장감 형성

중요:
Main19에서는
- 새 상태이상 추가 없음
- 새 상점 없음
- 새 동료 없음
- 새 마을 없음
- 새 Elite 없음
- 새 일반 몬스터 없음

기존 시스템을 조합과 연출로 확장합니다.

==================================================
2. BGM 정식 배정
==================================================

Field 탐색:
Paths of Cracked Earth

일반/Story 전투:
Blade and Gambit

신규 BGM 제작 없음.
Boss 전용 음악 사용 없음.
Main20용 음악 선사용 금지.

==================================================
3. Main19 Story 흐름
==================================================

Main18에서 더 깊은 길을 확인했지만
중심부 바로 앞까지는 도달하지 못했습니다.

Main19는:

Field09 더 깊은 길 접근
→ Field10 진입
→ 거대한 흔적 조사
→ 녹아내린 암벽 조사
→ Story Encounter A
→ 지면 맥동 조사
→ 열기 통로 통과
→ Story Encounter B
→ 더 거대한 흔적 조사
→ 능선 도달
→ 멀리서 거대한 화염 존재 Witness
→ 지금은 싸울 때가 아니라는 판단
→ Main20 준비 Hook

입니다.

중요:
Witness 장면에서 보이는 존재는
Main20의 거대한 화염 형상과 연결되지만,
이 시점에서 “모든 현상의 최종 원인”으로 단정하지 않습니다.

정확한 의미는:
“움직일 때마다 열기와 진동을 크게 증폭시키는 존재”
입니다.

==================================================
4. Quest Objective 정식 계약
==================================================

총 12개.

1.
objectiveId:
reach_field09_deeper_route

type:
ReachLocation

target:
field09_main19_gate

내용:
더 깊은 길로 향하세요.

--------------------------------------------------

2.
objectiveId:
enter_burning_pulse

type:
ReachLocation

target:
field10_main19_entry

내용:
맥동의 열맥에 들어가세요.

--------------------------------------------------

3.
objectiveId:
inspect_giant_track

type:
Interact

target:
field10_main19_track_1

내용:
거대한 흔적을 조사하세요.

--------------------------------------------------

4.
objectiveId:
inspect_melted_cliff

type:
Interact

target:
field10_main19_cliff

내용:
녹아내린 암벽을 조사하세요.

--------------------------------------------------

5.
objectiveId:
defeat_patrol_a

type:
DefeatEncounter

target:
field10_main19_patrol_a

내용:
앞을 막는 무리를 물리치세요.

--------------------------------------------------

6.
objectiveId:
inspect_pulse_node

type:
Interact

target:
field10_main19_pulse_1

내용:
지면의 맥동을 확인하세요.

--------------------------------------------------

7.
objectiveId:
cross_heated_corridor

type:
ReachLocation

target:
field10_main19_corridor

내용:
열기가 몰리는 통로를 지나가세요.

--------------------------------------------------

8.
objectiveId:
defeat_patrol_b

type:
DefeatEncounter

target:
field10_main19_patrol_b

내용:
심부를 지키는 무리를 물리치세요.

--------------------------------------------------

9.
objectiveId:
inspect_massive_footprint

type:
Interact

target:
field10_main19_track_2

내용:
더 거대한 흔적을 조사하세요.

--------------------------------------------------

10.
objectiveId:
reach_silhouette_ridge

type:
ReachLocation

target:
field10_main19_ridge

내용:
앞쪽 능선까지 이동하세요.

--------------------------------------------------

11.
objectiveId:
witness_flame_colossus

type:
Interact 또는
현재 안전한 witness 구현 구조와 동일한 방식

target:
field10_main19_witness

내용:
심부의 존재를 확인하세요.

--------------------------------------------------

12.
objectiveId:
withdraw_and_prepare

type:
Interact

target:
field10_main19_withdraw

내용:
물러나서 다음 준비를 정리하세요.

완료 시 Main19 종료.

==================================================
5. Story Encounter 계약
==================================================

이번 Main19에는
신규 Monster 종 추가 없음.

기존 Runtime 수치를 그대로 쓰며
조합으로 난도를 올립니다.

Story Encounter A:
field10_main19_patrol_a

구성:
obsidian_beetle ×1
fissure_lizard ×1

--------------------------------------------------

Story Encounter B:
field10_main19_patrol_b

구성:
obsidian_beetle ×1
ember_wraith ×1

--------------------------------------------------

중요:
- 기존 obsidian_beetle 수치 변경 금지
- 기존 fissure_lizard 수치 변경 금지
- 기존 ember_wraith 수치 변경 금지
- 신규 Elite 생성 금지
- 신규 Boss 생성 금지

도망:
가능

패배:
기존 공용 패배 규칙

승리:
목표 정확히 한 번만 진행

==================================================
6. 과열/냉각약 정책
==================================================

Main18에서 확정한 과열/냉각약 규칙을 그대로 사용합니다.

- Overheat 새 규칙 추가 없음
- Cooling Remedy 새 효과 추가 없음
- Cleanse / 기존 해제약의 Overheat 미제거 정책 유지

Main19는 새로운 상태이상이 아니라
기존 과열 시스템의 숙련 구간입니다.

==================================================
7. 주요 대사 정식 의미
==================================================

거대한 흔적 조사:

세린:
"발자국 하나의 간격이 너무 넓어요.
여러 개체가 아니라, 큰 하나가 지나간 흔적에 가깝습니다."

플레이어:
"감시자보다 더 큰 게 있다는 뜻입니까?"

세린:
"단정은 이르지만, 적어도 여기까지 남은 압력은 그 수준입니다."

--------------------------------------------------

녹아내린 암벽 조사:

폴:
"불길이 스친 정도가 아니네요.
안쪽부터 녹았다가 다시 굳은 것처럼 보입니다."

미엘:
"가까이 오래 있으면 사람도 버티기 어렵겠어요."

폴:
"네. 다음에는 관찰보다 대비가 먼저일 것 같습니다."

--------------------------------------------------

Story Encounter A 후:

세린:
"둘이 우연히 모인 게 아니에요.
같은 맥동에 밀려 한쪽으로 몰린 것처럼 움직였습니다."

--------------------------------------------------

더 거대한 흔적 조사:

플레이어:
"이건 발자국이라기보다, 길을 눌러 만든 자국 같군."

세린:
"네. 여기서부터는 '무언가가 지나갔다'가 아니라,
'무언가가 이 길을 쓰고 있다'에 가깝습니다."

--------------------------------------------------

Witness:

세린:
"저 존재가 모든 일의 원인이라고 단정할 수는 없어요.
하지만 움직일 때마다 열기와 진동이 함께 치솟습니다."

플레이어:
"지금 덤빌 상대는 아니군요."

조건부 동료 Flavor 한 줄 허용:

태온:
"준비를 갖추고 다시 오겠습니다."

미엘:
"다친 채로 밀어붙일 상대는 아니에요."

폴:
"다음에는 관찰보다 대응이 먼저겠네요."

--------------------------------------------------

마지막 Hook:

세린:
"저게 중심부에 가까운 존재라면, 다음에는 피할 수 없을 거예요."

플레이어:
"다음에는 끝을 볼 준비를 하고 오죠."

==================================================
8. Path Reactive 관찰
==================================================

inspect_pulse_node에서
5 Path별 Player 관찰 1회 추가.

시각의 길:
"붉은 균열이 밝아지는 지점과 흔적의 진행 방향이 겹칩니다."

청각의 길:
"낮은 울림이 두 번 끊어졌다가 다시 이어집니다.
안쪽일수록 간격이 짧아져요."

지적의 길:
"진동 간격과 열기 분출 시점이 비슷합니다.
무작위 현상으로 보기 어렵습니다."

지체의 길:
"갈라진 지면 사이에도 버틸 만한 길이 이어집니다.
무게가 큰 존재가 지나간 흔적일 수도 있습니다."

마음의 상처의 길:
"갑작스러운 열기에도 반복되는 리듬이 있어요.
패턴을 알면 다음 움직임을 예측할 수 있을 것 같습니다."

공통 세린:
"네. 더 깊은 쪽일수록 반응이 빨라집니다."

Path는 Flavor이며
진행 필수 능력이 아닙니다.

==================================================
9. Field10 Geometry
==================================================

Scene:
Field_10_BurningPulse

Bounds:
21 × 15

대략적 배치:

Entry:
(7.8, 0.0)

Track1:
(5.5, 1.0)

Melted Cliff:
(3.4, -2.0)

Patrol A:
(1.2, -0.8)

Pulse Node:
(-0.6, 1.8)

Heated Corridor:
(-2.5, 0.5)

Patrol B:
(-4.0, -1.3)

Massive Footprint:
(-5.3, 1.0)

Ridge:
(-7.0, 0.8)

Witness:
(-7.6, 0.2)

Withdraw:
(-6.2, -1.2)

세부 Collider/배치는
기존 Field Scene 안전 규칙에 맞춰 조정 가능하지만
동선 순서는 바꾸지 마세요.

==================================================
10. Scene 연결
==================================================

동쪽:
Field09 ↔ Field10 왕복

서쪽:
Main20 방향 통로가 보이지만
현재 Scene 전환 없음

안내 문구:
"더 깊은 심부는 열기와 진동이 너무 강해 지금은 바로 들어갈 수 없습니다."

==================================================
11. Optional 일반 조우
==================================================

우회 가능한 일반 조우 3개:

A.
obsidian_beetle ×1

B.
fissure_lizard ×1

C.
ember_wraith ×1

기존 Field Respawn 정책 재사용.
Story Encounter와 별도 구분.

==================================================
12. Art 방향
==================================================

Main19는 환경 중심이다.

신규 몬스터 Sheet는 이번 설계에서 요구하지 않는다.

필요 목록:

1. Field10 지면 Base
2. 맥동 균열 바닥 장식 2종
3. 녹아내린 암벽 오브젝트
4. 거대한 발자국/압흔 데칼
5. 긴 긁힌 자국/끌린 흔적
6. 열기 기둥/맥동 분출구
7. 능선/전망 절벽 타일
8. 배경용 먼 화염 실루엣 장식

VFX:
- 지면 맥동 1종
- 먼 화염 맥동 1종

재사용 가능:
- Main18 흑요석/균열/재 타일 일부
- obsidian_beetle / fissure_lizard / ember_wraith 기존 Sprite

최종 Art Pack이 아직 없으면
기존 자산으로 임시 배치하고
USER_ART_REVIEW_REQUIRED / ART_PENDING으로 남길 수 있습니다.

==================================================
13. TTS 정책
==================================================

Main19 신규 NPC 발화는
Stable Dialogue ID를 부여하고
Main19_TTS_Manifest.csv
또는 현재 프로젝트 관례 문서를 생성하세요.

NPC:
TTS_PENDING

Player:
VoiceExpected=false

지문:
VoiceExpected=false

Codex는:
- TTS API 호출 금지
- 음성 생성 금지
- 임의 기존 Voice 연결 금지

==================================================
