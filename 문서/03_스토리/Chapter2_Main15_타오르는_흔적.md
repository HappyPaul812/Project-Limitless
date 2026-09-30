# Chapter 2 Main15 — 타오르는 흔적

> Main12~14 다음의 확정 설계. 실제 구현·검증 상태는 [CURRENT_STATUS](../00_프로젝트/CURRENT_STATUS.md)를 따른다.

## 범위와 서사

`main_15_burning_traces`는 Main14 완료 뒤 아르벨의 레온에게서 시작한다. 서쪽 길의 따뜻한 땅과 작은 화재 흔적, 동쪽으로 이동하는 야생동물에 관해 듣고 세린과 더 서쪽을 조사한다. 완전한 사막이나 화산은 아니다. 갈라진 땅·검게 변한 돌·마른 식물은 관찰 사실이고, 열과 진동의 원인은 미상이다. Chapter 1 푸른빛, 화염 존재, 최종 흑막과의 인과관계도 단정하지 않는다.

`Field_06_ScorchedTrail`은 Arbel 서쪽의 별도 길로 왕복한다. 아르벨 근처의 마른 풀과 일부 나무에서 안쪽의 황토·붉은 갈색 흙, 늘어나는 균열·고사목·그을린 바위로 점진적으로 변한다. 환경 자체의 열기 가능성을 암시하지만 용암과 큰 산불은 보이지 않는다. World Bounds와 실제 출구에만 열린 경계, 출구와 떨어진 Spawn을 사용한다. 새 Safe Zone은 없으며 최근 거점은 Arbel이다.

## 순차 목표

| 순서 | Objective ID | 종류 / Target ID | 내용 |
| --- | --- | --- | --- |
| 1 | `hear_western_reports` | TalkToNpc / `arbel-leon` | 서쪽 상황 듣기 |
| 2 | `enter_scorched_trail` | ReachLocation / `field06_east_entry` | 새 서쪽 길 도착 |
| 3 | `inspect_soot_traces` | Interact / `field06_main15_soot` | 첫 그을린 흔적과 변한 들개 관찰 |
| 4 | `inspect_fleeing_tracks` | Interact / `field06_main15_tracks` | 동쪽으로 향한 야수 흔적 |
| 5 | `face_affected_beasts` | DefeatEncounter / `field06_m15_01` | 첫 그을음들개 지정 조우 |
| 6 | `follow_shorter_pulse` | Interact / `field06_main15_pulse` | 간격이 조금 짧고 강해진 진동 |
| 7 | `inspect_cracked_ground` | Interact / `field06_main15_crack` | 긴 균열과 약한 열기 |
| 8 | `inspect_heat_traces` | Interact / `field06_main15_heat` | 검게 변한 바위와 마른 식물 |
| 9 | `compare_with_serin` | TalkToNpc / `field06-serin` | 관찰 결과를 함께 정리 |
| 10 | `return_to_arbel` | ReachLocation / `arbel_main15_return` | Arbel 귀환 |
| 11 | `report_to_leon` | TalkToNpc / `arbel-leon` | 원인을 단정하지 않고 보고 |
| 12 | `welcome_serin` | TalkToNpc / `arbel-serin` | 세린의 자발적 정식 합류 |

마지막 대화 완료 콜백에서 기존 `CompanionRosterService`의 해금 ID에 세린을 추가한다. 현재 편성 2명은 교체하지 않는다. Main15 진행 중에는 기존 Story Temporary 참가 규칙을 쓰며, 정식 해금 뒤에는 일반 동료 선택과 저장 경로를 쓴다. 세린은 사수·청각의 길, 기본 Beast Fox, 기존 공식 시각 자료와 시작 스킬을 그대로 사용한다. 별도 세린 전투 시스템은 없다.

## 대화와 조우

세린은 장치로 진동의 존재·방향·느슨한 반복 패턴을 알아차리지만 정확한 발생점을 지목하지 않는다. 첫 들개에서는 불로 만들어진 존재라 단정하지 않고 환경 영향을 받은 생물로 관찰한다. 폴과의 짧은 건조한 대화로 동행에 익숙해진 모습을 보인다. 세린은 기존 조사 목적과 파티의 목적이 같고 함께 움직이는 편이 합리적이라고 스스로 판단해 합류한다. 레온은 합류를 명령하지 않는다.

필드 일반 조우는 소수만 둔다. M15-01 그을음들개 1, M15-02 그을음들개+열풍매, M15-03 균열도마뱀 1, M15-04 화열딱정벌레+열풍매다. 첫 지정 조우는 기존 Battle 승리 알림으로 5번 목표를 진행한다. 이 네 종의 Victory 기록은 기존 Arbel 분양소에 반영된다. 불씨망령·Boss·과열·새 Pet Tutorial은 없다.

## 다음 단서

Main15는 서쪽으로 갈수록 건조·열기·진동이 심해진다는 관찰로 끝난다. 후속 Main16은 [「재 속의 형상」 정식 설계](Chapter2_Main16_재_속의_형상.md)에 확정했다. 재바람 황야·불씨망령 첫 Story Encounter와 생물 변이만으로 설명하기 어려운 현상을 다루며 Runtime/Scene/Voice/BGM은 미구현이다. Main15 자체의 구현 범위는 바꾸지 않는다.

## Main15 Path Retrofit 후보

**DESIGN RETROFIT CANDIDATE / 미구현.** 공통 기준은 [Path 반응형 Story 정본](Path_반응형_Story_연출_규칙.md)이다. 현재 대사·Voice·Runtime·Quest 목표를 수정하지 않는다.

| 기존 장면·대사 ID | Hearing Player 후보 | 보존 정보 |
| --- | --- | --- |
| `follow_shorter_pulse` / `main15_serin_003` | Player가 먼저/거의 동시에 멈추고 짧아진 간격·강도를 첫 관찰로 제시, 세린이 교차 확인한다. | 완전히 일정하지 않은 반복, 원인 미상, 동일 조사 목표. |
| `compare_with_serin` / `main15_serin_004` | Player가 반복 신호를 먼저 정리하고 세린이 방향/이전 조사 경험을 보완한다. | 더 서쪽에서 오지만 위치는 모름. 일반 분기는 기존 세린 공유 유지. |
| `inspect_cracked_ground` / `main15_serin_006` | Player가 서쪽 신호를 먼저 제시하고 세린이 방향을 확인한다. | 현재 지점이 발생 시작점이라고 단정하지 않는다. |

“또 시작됐어요.”에 대한 Player의 우선 반응을 후보로 삼되, 요청에 든 폴의 “진동입니까?”는 현재 LOCAL 대사 인용이 아닌 흐름 예시다. 기존 대사는 최신 코드/Manifest를 기준으로 별도 분기를 설계하고 Story→Branch→ID 확정 뒤 필요한 TTS 검수를 진행한다.
