# Main21 「돌아온 온기」 상세 정본

상태: DESIGN_CONFIRMED / IMPLEMENTATION_IN_PROGRESS / USER_REVIEW_PENDING

2026-10-10 사용자 승인 계약. 이전 DESIGN_DRAFT의 TBD 중 아래에서 확정된 내용은 이 계약으로 대체한다. 하렌(Haren) 정식 이름·시각의 길/투사·제한적 바이저, 기존3인 전투와 Chapter3 마지막 “잘.했.어.”를 보호한다. StarterVillage v4는 USER_ART_APPROVED, Scene/NPC Commit `7309f77`.

# [LIMITLESS / Main21 「돌아온 온기」 최종 상세 설계 반영 및 구현]

## 0. 작업 목적

Main21의 승인된 전투 A/B, 안전 통로 퍼즐, 13개 목표를 정식 계약으로 문서화하고 실제 Unity 구현을 진행한다.

최신 GitHub/local 정본 및 실제 Runtime 구현을 우선 확인한다.

Unity: 6000.6.5f1
개발 저장소: F:\study\codex\Project-Limitless

원칙:
- 게임플레이 중심, 긴 대화로 플레이 시간을 채우지 않음
- 기존 게임 시스템 우선 재사용
- 임의의 신규 보스·몬스터·상태이상·전투 규칙 추가 금지
- 원본 사용자 변경 보호
- 모든 QA는 화면을 빼앗지 않는 백그라운드 방식
- GitHub Push 금지

## 1. 공식 퀘스트 계약

Quest: Main21 「돌아온 온기」

Quest ID:
main_21_returning_warmth

선행:
main_20_colossus_of_the_depths 완료

Chapter:
Chapter 2 마지막

Main22:
Chapter 3 시작, 아직 미구현

완료 보상:
- EXP 100
- Talent Currency 60
- 전용 아이템 0
- 보상 중복 지급 0

전투별 경험치·탈렌트·기존 잡템 드롭은 MonsterDefinition과 현재 레벨 차 배율 및 Loot 규칙을 그대로 따른다.

예상 플레이 시간:
30~45분 목표, 실제 QA 측정 전까지 미검증.

Main20 최종 보상·Objective·보스 상태는 변경하지 않는다.

Main20 완료→Field10 복귀 시 Main21 시작 가능하게 연결하되, 완료되지 않은 Main20 Save를 건너뛰지 않는다.

## 2. Main21 목표 정확히 13개

아래 objectiveId와 targetId를 신규 고유 ID로 사용한다. Type은 현재 Quest 시스템과 대조한다.

1.
objectiveId: inspect_receding_heat
Type: Interact
targetId: field10_main21_heat_change
문구: 달라진 지면의 열기를 확인하세요.

2.
objectiveId: inspect_remaining_pulse
Type: Interact
targetId: field10_main21_pulse_trace
문구: 세린과 남아 있는 진동을 조사하세요.

3.
objectiveId: defeat_residual_group_a
Type: DefeatEncounter
targetId: field10_main21_encounter_a
문구: 균열 주변의 몬스터를 물리치세요.

4.
objectiveId: inspect_passage_options
Type: Interact
targetId: field10_main21_passage_briefing
문구: 폴과 통행로 후보를 조사하세요.

5.
objectiveId: select_safe_passage
Type: Interact
targetId: field10_main21_passage_decision
문구: 단서를 비교해 안전한 통행로를 선택하세요.

6.
objectiveId: defeat_residual_group_b
Type: DefeatEncounter
targetId: field10_main21_encounter_b
문구: 통행로에 남은 몬스터를 물리치세요.

7.
objectiveId: verify_passage_safety
Type: Interact
targetId: field10_main21_passage_verified
문구: 태온과 통행로의 안전을 최종 확인하세요.

8.
objectiveId: report_to_leon
Type: Interact
targetId: arbel_main21_report_leon
문구: 아르벨의 레온에게 결과를 보고하세요.

9.
objectiveId: assist_residents
Type: Interact
targetId: arbel_main21_aid_point
문구: 미엘과 함께 주민 지원을 마무리하세요.

10.
objectiveId: return_by_carriage
Type: ReachLocation
targetId: starter_village_main21_carriage_arrival
문구: 귀환 마차를 타고 초보 마을로 돌아가세요.

11.
objectiveId: inspect_northern_frost
Type: Interact
targetId: starter_village_main21_north_trace
문구: 마을 북쪽의 이상 징후를 조사하세요.

12.
objectiveId: meet_haren
Type: Interact
targetId: starter_village_main21_haren_meeting
문구: 태온의 형 하렌을 만나세요.

13.
objectiveId: prepare_northbound
Type: Interact
targetId: starter_village_main21_departure_briefing
문구: 북쪽 탐험 준비를 정리하세요.

13개 모두 순차 목표로 구현한다. 보상은 13번 완료 시 정확히 1회 지급한다.

## 3. 지정 전투 A

명칭:
꺼지지 않은 불씨

Encounter ID:
field10_main21_encounter_a

몬스터:
- fissure_lizard ×1 / 전열
- ember_wraith ×1 / 후열

기존 MonsterDefinition과 Battle Runtime을 그대로 사용한다.

전투 의도:
- 도마뱀의 지면 울림 예고 및 단일 공격
- 망령의 화상·마법 공격
- 플레이어의 방어·정화·회복 판단

신규 AI, 신규 Burn, 신규 전투 스킬을 추가하지 않는다.

기존 3인 파티:
플레이어 1 + 자유 편성 동료 2

승리 시 목표3 정확히 한 번 진행.

패배·도망 시 목표3 유지, 기존 공용 복귀 및 재도전 규칙 사용.

## 4. 지정 전투 B

명칭:
마지막 통로의 위협

Encounter ID:
field10_main21_encounter_b

몬스터:
- obsidian_beetle ×1 / 전열
- soot_hound ×1 / 전열

기존 몬스터 수치·행동·Drop을 유지한다.

전투 의도:
- 흑요석 갑충의 과열 압박
- 그을음들개의 빠른 단일 공격
- 두 적 모두 전열이므로 근접 공격도 충분히 활용 가능

승리만 목표6 진행.

패배·도망·Continue 중복 진행 금지.

기존 레벨 차 EXP 배율과 Talent·Loot 시스템 변경 금지.

## 5. 안전 통로 판단 퍼즐

기획 명칭:
서부 복구 작전

목표:
주민과 보급 마차가 안전하게 다닐 수 있는 통행로 선택.

후보 3개:

A. 중앙 균열길
- 표면 온도는 내려감
- 일정 간격의 지면 진동이 남아 있음
- 안전하지 않음

B. 암반 지름길
- 이동 거리는 짧음
- 바닥에 높은 잔열 존재
- 안전하지 않음

C. 옛 우회로
- 지면 안정
- 잔열 낮음
- 통행 폭 확보
- 정답

**정답: 옛 우회로**

### 플레이 방식

플레이어가 Field10의 세 조사 지점을 직접 탐색한다.

각 지점은 글·시각적 표식으로 확인 가능하게 만든다.

조사 순서는 자유다.

필요한 단서를 확인한 뒤 선택 지점에서 세 후보 중 하나를 선택한다.

정답이면:
- 선택 결과 대화
- 해당 통로에 안전 표시
- Objective5 정확히 1회 진행

오답이면:
- 구체적인 위험 이유 설명
- 다시 조사 가능
- HP/아이템/돈/경험치 손실 없음
- Objective 진행 없음

잘못된 선택으로 추가 전투를 발생시키지 않는다.

### 중요한 구현 조건

현재 Field10에 실제 3갈래 도로가 있다고 가정하지 않는다.

LOCAL Scene의 기존 Geometry·Collider·NPC/Encounter 위치를 조사하고, 지형을 대규모로 변경하지 않는 방식으로 세 조사 지점을 배치한다.

단서별 내부 표시 상태의 Save/Continue 복구 방식을 명확하게 구현한다. 새로운 Save 필드가 불필요하다면 기존 Quest 상태를 우선 사용한다.

저장 후 재시작했을 때 선택 UI 잠금이나 영구적인 진행 불능이 발생하면 안 된다.

Path별 최초 관찰자 규칙은 기존 Path 반응형 Story 정본을 따른다.

특히:
- 청각의 길 플레이어의 첫 관찰 기회 보호
- 지체의 길 플레이어의 이동 경로 관찰 보호
- 세린·폴은 경험을 통해 교차 확인
- 태온은 이전 현장의 반복된 위험을 기억하고 자기 판단에 기여

특정 Path·특정 동료 전투 편성 필수 조건 없음.

## 6. 아르벨 주민 지원

목표8: 기존 NPC 레온에게 조사 결과 보고.

목표9: 주민 지원 지점에서 플레이어가 직접 지원을 마무리한다.

목표9는 NPC와 대화만 하는 이벤트로 끝내지 말고, 주민 상황을 확인하고 보급품을 배치·전달하는 짧은 상호작용을 포함한다.

이번 퀘스트를 위해 신규 소모 아이템을 만들거나 사용자 Inventory 재료를 강제로 소비하지 않는다.

별도 무료 HP/MP 전체 회복 보상을 임의로 추가하지 않는다.

미엘이 주민을 지원하고, 폴도 물자 점검이나 경로 정리에 능동적으로 참여한다.

## 7. 귀환 마차

기존 정본:
- 문서/03_스토리/Main21_자동귀환_마차_구현준비_20261010.md
- Main21 마차 v1 검수 및 Supplement v1 검수 문서

기존 정지 Sprite 2종은 등록돼 있다.

정확한 말 애니메이션 Rect와 탑승자 가림 레이어는 아직 미검증이다.

**따라서 정지 마차와 느린 배경 이동 방식의 연출을 우선한다.**

- 아르벨에서 명시적 탑승 수락
- 거절 시 출발 보류, 다시 선택 가능
- 정상 진행·Skip 모두 동일한 귀환 완료 경로
- 실제 마을 Spawn 배치 성공 뒤 Objective10 진행
- 연출 10~15초 목표
- Skip 시 로딩·저장·도착 목표 생략 금지

항상 접근 가능한 동일 마차를 사용한다.

폴 휠체어1대 + 지체의 길 플레이어 휠체어1대 + 승객3명의 공간을 전제로 한다.

정확한 내부 승객 가림 표현이 준비되지 않은 부분은 임의로 완성된 것처럼 표현하지 않는다. 공식 캐릭터 Sprite를 훼손하거나 보행 모습으로 바꾸지 않는다.

Main21 전용 연출만 구현하고 범용 Fast Travel 신규 시스템은 만들지 않는다.

Save/Continue 중 출발·재시도·Skip·도착 실패 처리는 기존 마차 준비 문서의 안전 계약을 따른다.

## 8. 주요 대사 정본

아래 대사는 Main21의 1차 정식 구현 본문으로 사용한다.

실제 대화 Stable ID는 아래를 기준으로 하며 Speaker ID는 기존 Catalog 규칙에 맞춰 연결한다.

### 달라진 열기

main21_heat_serin_01
세린:
"바람이 달라졌어요. 아직 뜨겁긴 하지만, 전처럼 밀어붙이는 느낌은 아닙니다."

### 잔류 진동

main21_pulse_serin_01
세린:
"울림은 남아 있어요. 하지만 간격이 훨씬 길어졌습니다."

main21_pulse_player_01
플레이어:
"아직 위험한 곳이 있는지 확인해야겠군요."

Path별 첫 관찰 대사가 필요한 경우 공통 대사를 무리하게 재사용하지 말고 서로 다른 Stable ID로 분리한다.

### 통행로 조사

main21_route_paul_01
폴:
"가장 짧은 길이 꼭 안전한 길은 아니겠죠. 바닥부터 확인해 봅시다."

main21_route_serin_01
세린:
"중앙 쪽은 아직 일정한 간격으로 흔들립니다."

main21_route_paul_02
폴:
"암반 쪽은 겉만 식었습니다. 보급 마차가 지나가기엔 위험하겠어요."

### 정답 선택

main21_route_player_01
플레이어:
"옛 우회로가 조금 멀어도, 사람들이 안전하게 지나갈 수 있겠군요."

### 통로 확인

main21_route_taeon_01
태온:
"여긴 지난번에 흔들리던 곳과 다릅니다. 제가 다시 확인하겠습니다."

### 아르벨 보고

main21_arbel_leon_01
레온:
"주민들이 지날 수 있는 길을 찾아주셨군요. 정말 큰 도움이 됐습니다."

### 주민 지원과 폴·미엘

main21_aid_miel_01
미엘:
"다치신 분들은 제가 살펴볼게요. 폴 씨는 물자 수량을 확인해 주시겠어요?"

main21_aid_paul_01
폴:
"이미 확인했습니다. 이번에는 다행히 약보다 붕대가 더 많이 필요하겠네요."

main21_aid_miel_02
미엘:
"그 말이 이렇게 반가울 줄은 몰랐네요."

두 사람의 Slow Burn이지만 공식 고백·연애 확정은 없다.

### 마차 탑승

main21_carriage_leon_01
레온:
"초보 마을로 돌아갈 마차를 마련했습니다. 준비되셨으면 출발하시죠."

선택:
- 마차로 돌아간다
- 아직 준비가 안 됐다

### 북쪽 조사

main21_north_direction_01
지문:
"서부의 열기와는 다른 차가운 바람이 마을 북쪽에서 불어옵니다."

### 하렌 첫 등장

main21_haren_haren_01
하렌:
"태온. 너 아직 멀었어."

main21_haren_taeon_01
태온:
"형? 오랜만에 만나서 첫마디가 그거야?"

main21_haren_haren_02
하렌:
"다친 데는 없고?"

main21_haren_taeon_02
태온:
"응. 괜찮아."

main21_haren_haren_03
하렌:
"그럼 됐어."

하렌은 시각의 길 투사이며 판타지 바이저 콘셉트를 유지한다.

하렌의 전용 공식 Sprite/Portrait가 아직 준비되지 않았다면 임의의 타인 캐릭터 아트를 정식 아트로 사용하지 않는다. 텍스트/임시 표식 기반으로 안전하게 동작시키고 ART_PENDING을 정확히 기록한다.

Chapter3 마지막 문구 "잘.했.어."는 Main21에서 절대로 사용하지 않는다.

### 마무리

main21_prepare_serin_01
세린:
"이번엔 북쪽이군요. 먼저 길부터 살펴봐야겠어요."

main21_prepare_player_01
플레이어:
"준비가 끝나면 출발하죠."

기존 NPC·Companion의 말투 및 서사 정본과 충돌하면 원문 임의 변경 전에 정확한 충돌을 보고한다.

## 9. Dialogue 및 TTS 정책

- 대사 ID 고유성 검사
- NPC·Companion 음성: TTS_PENDING
- Player·지문: VoiceExpected=false
- 실제 신규 TTS WAV 제작0
- 기존 음성 Catalog 및 공식 Portrait 보호
- 캐릭터별 대사 표시와 누락 음성의 텍스트 fallback 유지
- 전투 대기 동료도 필수 Story 장면에서 사라지지 않게 처리

TTS용 Manifest는 별도 신규 파일로 작성한다. 기존 Stable ID나 오디오를 덮어쓰지 않는다.

## 10. Chapter3 연결

Main21 완료 시 Chapter2 완료 상태를 저장한다.

하렌 첫 등장으로 Chapter3 형제애의 시작을 암시한다.

아직 구현되지 않은 Main22로 강제 Scene 전환하지 않는다.

북쪽 지역의 겨울·서리·설원은 점진적으로 등장한다.

태온의 형이 당장 정식 동료가 되거나 파티에 강제 편입되지 않는다.

## 11. Unity 작업 보호

다음은 반드시 유지한다.

- 기존 Main01~20
- Chapter1/2 SideQuest 9종
- StarterVillage v4 확정 환경 아트
- World_StarterVillage Scene과 NPC Prefab
- 사용자 기존 미커밋 변경
- 기존 Scene Spawn, Bounds, Collider
- 기존 3인 전투 및 자유 동료 편성
- Player의 Path/Job 선택
- 폴은 별도 동료 NPC
- SaveVersion1 / 저장 슬롯5개
- 기존 BGM·TTS·캐릭터 원본 아트
- 기존 사용자 설정과 Git 변경

기존 Scene 전체를 재생성하지 않는다.

필요한 신규 요소는 고유 Script/Runtime 설치와 최소한의 Asset 연결로 처리한다.

## 12. QA

Unity 6.6.5f1 기준 백그라운드 검증.

필수:
- Main20 완료→Main21 시작
- 13개 목표 순차 진행
- A/B 전투 승리/패배/도망/재도전
- 몬스터 기존 전투 보상·Loot
- 퍼즐 3단서·정답·오답·재선택
- 5개 Path 정보 접근
- 다양한 전투 동료 편성
- 레온·미엘 지원 기능
- 마차 Yes/No/Skip
- 중복 수락·중복 Skip 방지
- Scene 로드 실패·도착 Spawn 지연 복구
- Continue와 5개 슬롯 보호
- 퀘스트 보상 정확히 1회
- 초보 마을 NPC·Quest Marker·남문
- 하렌 등장과 Chapter3 미진입
- Main01~20 및 SideQuest 회귀

기존 Unity Search 내부 예외2건은 별도 알려진 미해결 이슈로 추적하고, 발생 시 전체 Console 무오류 PASS로 허위 판정하지 않는다.

모든 검증은 격리 환경·비대화형으로 진행한다.

원본 Unity Editor에서 Scene 전환·Play Mode·포커스 강탈·강제 종료 금지.

기존 사용자 Save를 테스트에 사용하거나 수정하지 않는다.

## 13. 문서와 Commit

다음 문서를 상세 확정 정본으로 갱신한다.

`문서/03_스토리/Chapter2_Main21_돌아온_온기.md`

관련 마차 구현 준비 문서와 CURRENT_STATUS를 최신 결과에 맞게 갱신한다.

완료 상태 구분:
- DESIGN_CONFIRMED
- 구현된 개별 항목: IMPLEMENTED / QA 상태
- 아직 없는 하렌 아트·말 애니메이션 등: ART_PENDING
- 실제 사용자 미술·게임플레이 검토: USER_REVIEW_PENDING

작업 순서:
1. 기존 정본/Runtime 확인
2. 상세 설계 문서 갱신
3. Unity 구현
4. 백그라운드 QA
5. 검증 결과 문서화
6. 관련 파일만 선별 Commit

문서 확정과 구현 Commit은 가능한 분리한다.

완료 후 보고:
- 13목표 구현 여부
- 전투 A/B·퍼즐 결과
- 마차 자동 귀환 구현 상태
- 하렌 첫 등장 구현 범위
- TTS/아트 대기 목록
- QA PASS/FAIL/NOT_VERIFIED
- 기존 사용자 변경 보존 결과
- Commit SHA

**GitHub Push 금지. 사용자가 직접 Push한다.**

목표는 Chapter2를 게임플레이로 마무리하는 Main21을 구현하고, Chapter3 시작을 자연스럽게 준비하는 것이다.

## 구현 저장 계약

13개 목표를 추가하거나 변경하지 않는다. 순서와 완료 보상은 기존 QuestService가 담당한다. 자유 순서 단서3개와 마차 수락을 보존하기 위해 QuestProgressSaveData에 Main21Flags(선택 int, 구버전 기본0)를 추가한다. 비트1/2/4=단서A/B/C, 비트8=마차 수락. SaveVersion1/슬롯5/기존 ID 유지. 마차 연출은 Arbel UI에서만 실행하며 출발 체크포인트를 저장한다. 기존 전용 Spawn/실제 배치/Bounds/Collider/위치 저장 확인 뒤에만 도착 목표를 완료한다. 별도 연출 Scene 저장이나 범용 빠른 이동 없음. 기존 준비 문서의 수락 GenericSignal 목표 후보는 승인된13개 계약에 따라 사용하지 않는다. 정확한 말 애니메이션·승객 가림·하렌 공식 아트는 ART_PENDING.
