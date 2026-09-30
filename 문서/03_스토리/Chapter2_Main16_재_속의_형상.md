# Chapter 2 Main16 — 재 속의 형상

> 2026-09-30 정식 Story 설계 완료. **Runtime·Quest·Scene·Voice·BGM 미구현.** 공통 연출은 [Path 반응형 Story 정본](Path_반응형_Story_연출_규칙.md), 장면 기록은 [Scene 템플릿](Story_Scene_설계_템플릿.md)을 따른다. Main15 이후 설계이며 새 Stable ID는 구현 단계에서 확정한다.

## 역할과 지역

Main15까지의 “왜 서쪽 땅이 말라가는가?”를 “땅 아래에서 무엇이 일어나는가?”로 확장하는 Chapter2 중반 전환점이다. 건조·열기·변한 야생동물을 넘어 불씨망령이 처음 본격 등장한다. 단순 생물 변이만으로 설명하기 어려운 현상을 확인하되 정체와 최종 원인은 밝히지 않는다.

Main15 완료 뒤 Arbel의 레온에게서 시작한다. 신규 지역 표시명은 **재바람 황야**, Scene 가칭은 `Field_07_AshenReach`다. 번호·실제 Scene/Quest/Target/Encounter ID·지도 크기·배치·보상은 TBD이며 구현 시 LOCAL Convention으로 확정한다. Field06의 더 깊은 서쪽으로 이동해 Arbel로 귀환하는 연결을 설계한다.

Field06보다 풀이 거의 없고 고사목·그을린 돌·작은 재·열 아지랑이·균열이 늘어난다. 일부 균열에 약한 붉은 빛이 있으나 용암 강·불바다·화산 분화는 없다. 변화는 점진적이다. Field07 BGM은 **TBD**, Morning Over the Ridge를 계속 재사용하지 않는다. 별도 Chapter2 긴장/열기 테마가 필요하며 이번에 Audio를 만들지 않는다.

## 순차 Objective 계약

아래 키는 설계용 의미 키이며 확정된 Runtime Stable ID가 아니다. 기존 순차 Objective 구조로 한 번씩 진행하고 Path 분기는 같은 목표로 수렴한다.

| 순서 | 의미 키 | 종류 방향 | 내용 |
| --- | --- | --- | --- |
| 1 | talk_to_leon_about_ash | TalkToNpc | 레온에게 재 움직임 소문 듣기 |
| 2 | head_deeper_west | ReachLocation | Field06을 거쳐 더 서쪽으로 이동 |
| 3 | enter_ashen_reach | ReachLocation | 재바람 황야 진입 |
| 4 | inspect_moving_ash | Interact | 바람 없이 한 방향으로 움직이는 재 조사 |
| 5 | trace_ground_vibration | Interact | 지면 진동과 재 움직임 비교 |
| 6 | inspect_broken_tracks | Interact | 방향을 바꾸거나 흩어진 야생동물 흔적 조사 |
| 7 | witness_ember_wraith | ReachLocation/목격 연출 | 불씨망령 최초 목격, 진행 API는 구현 전 확인 |
| 8 | defeat_ember_wraith | DefeatEncounter | 지정 불씨망령1개체 승리 |
| 9 | inspect_afterimage | Interact | 형체 소멸 후 남은 현장 조사 |
| 10 | find_western_route | ReachLocation | 더 서쪽 협곡 입구 확인 |
| 11 | return_to_arbel | ReachLocation | Arbel 귀환 |
| 12 | report_the_anomaly | TalkToNpc | 레온에게 사실과 가설을 구분해 보고 |

목격과 전투 승리는 별도 목표다. 일반 몬스터 처치·도망·패배를 지정 조우 승리로 오인하지 않으며, 재시도에서도 완료·보고·보상이 중복되지 않게 구현 전에 계약을 확인한다.

## 시작과 움직이는 재

레온은 바람이 거의 없는데 재가 움직였다는 증언, 큰 불길 없이 땅이 뜨거웠다는 증언, 움직이는 검은 연기 같은 것을 봤다는 소문을 전달한다. 아직 현장에서 확인하지 않은 소문이며 공포를 과장하지 않는다.

레온: “평소 같으면 잘못 본 거라고 생각했겠지만, 요즘은 그렇게 넘기기가 어렵군요.”

폴: “확인할 수 있는 건 직접 확인하죠. 소문이 사실인지부터요.”

Field07에서는 바람이 거의 없는데 바닥의 재가 한 방향으로 조금씩 움직인다. 진동과 재 움직임이 동시에 발생하는 것을 비교한다. 이 장면은 Path 반응형 규칙의 첫 정식 대표 사례다.

### Path-Reactive Check: 재와 진동

- 관련 Path: Hearing.
- 해당 Path Player 반응: 장치/익숙한 감각에 반복 신호가 먼저 잡힌다. 플레이어가 먼저 관찰을 제시한다.
- 일반 Player 반응: 세린이 먼저 멈추고 장치를 조절해 관찰을 공유한다. 플레이어는 질문·비교·조사 결정에 참여한다.
- 관련 Companion: 세린의 교차 확인, 태온의 질문.
- Hearing Player일 때 Companion 역할: 세린이 같은 신호를 확인하고 인과관계의 불확실성 또는 세부 패턴을 보완한다.
- Story State 차이: 없음. 진동과 재 이동이 함께 발생하나 원인·선후관계는 모른다.
- Quest 진행 차이: 없음. 4·5번 조사 목표의 공통 정보로 수렴한다.
- 선택 Dialogue/ID: 아래 두 분기, 각각 별도 Stable ID 필요·최종 ID TBD.
- Accessibility/Representation 주의: 장치가 판단을 대신하지 않는다. 진동 단서는 자막·재의 움직임·서술로도 전달하고 반응속도를 요구하지 않는다.

| Hearing Player | 일반 Player |
| --- | --- |
| Player: “진동하고 재가 움직이는 간격이 같아요.” | 세린: “땅이 울릴 때마다 움직여요.” |
| 세린: “저도 그렇게 잡혀요.” | 태온: “진동과 관련이 있다는 말씀이십니까?” |
| 세린: “다만 어느 쪽이 먼저인지는 아직 모르겠습니다.” | 세린: “같이 일어나는 건 맞아요. 어느 쪽이 원인인지는 모르겠습니다.” |

## 흔적과 불씨망령 첫 등장

야생동물 흔적은 어느 지점에서 갑자기 방향을 바꾸거나 흩어진다. 시체를 과도하게 배치하지 않는다.

폴: “여기까지는 계속 서쪽으로 갔습니다.”

미엘: “그런데 여기서 전부 방향을 바꿨네요.”

태온: “무언가를 피한 것으로 보입니다.”

이 장면의 필수 Path 분기는 없음이다. 보조 진동 관찰이 필요해지면 Hearing 플레이어 우선 순서와 별도 Dialogue ID를 구현 전에 설계한다.

첫 불씨망령은 일반 Idle 배치가 아니라 **지면 조사 → 약한 진동 → 재가 한곳에 모임 → 작은 불씨 → 검은 연기와 재가 형태를 만듦 → 불씨망령 등장**의 Story Encounter다. 목격 연출은 색·소리만으로 이해를 요구하지 않고 충분히 읽을 수 있는 서술을 제공한다.

현재 분류는 **화염 현상형/정령형**, 정확한 정체는 불명이다. 이름의 망령은 죽은 사람의 영혼을 확정하지 않는다. 인간의 원혼·희생자의 영혼·Boss 소환물·최종 원인의 직접 산물·특정 세력의 창조물로 확정하지 않는다.

첫 전투는 **Ember Wraith ×1**이며 다른 몬스터와 섞지 않는다. 기존 `ember_wraith`의 잿불 비산·불씨 응축·열핵 분출 Runtime 계약을 [몬스터 정본](../08_몬스터/Chapter2_몬스터_1차_설계.md)에서 재사용한다. 여기서 수치·새 스킬·상태·펫 정책을 바꾸지 않는다. 전투 파티는 사용자의 저장 편성을 기본으로 유지한다.

## 전투 후와 지하 열기 가설

불씨망령의 형체는 재와 불씨로 흩어지지만 균열·열기·진동은 남는다.

| Hearing Player | 일반 Player |
| --- | --- |
| Player: “아직 계속돼요.” | 세린이 먼저 진동 지속을 알린다: “아직 계속됩니다.” |
| 세린: “네. 저도 잡힙니다.” | Player가 사라진 형체와 남은 현상을 비교한다: “형체는 사라졌는데 진동은 남았군요.” |

폴: “그러면 저게 진동을 만든 건 아니군요.”

태온: “적어도 모든 원인은 아니라는 뜻이겠습니다.”

폴의 말은 당시 추론이며 확정 정보에 그대로 승격하지 않는다. 공통 사실은 **처치 후에도 진동이 지속된다**는 것, 신중한 판단은 불씨망령 처치만으로 모든 현상이 해소되지 않았다는 것이다. 과거의 진동 발생에 전혀 관여하지 않았다고 증명한 것은 아니다.

균열 안쪽 열기·지하에서 올라오는 미세한 공기 흐름·표면 그을림·큰 산불 흔적 부재를 관찰한다. “열기의 일부가 지하에서 올라오는 것일 수 있다”는 **가설**을 세우며 확정 원인으로 처리하지 않는다.

이 장면의 Path-Reactive Check는 Hearing/Player 우선 확인·세린 교차 확인, 일반 분기는 세린 공유·Player 비교, Story State/Quest 차이 없음이다. 선택 대사는 별도 ID TBD, 재/빛/자막과 현장 관찰로 단서를 전달한다.

## 서쪽 통로와 귀환

Field07 끝에서 가칭 **붉은 균열 협곡** 입구를 발견한다. 붉게 갈라진 절벽·재·강한 아지랑이·더 강한 지면 진동만 확인하고 Main16에서는 진입하지 않는다.

Hearing 플레이어가 서쪽의 더 강한 신호를 먼저 감지하고 세린이 방향·반복을 확인한다. 일반 분기에서는 세린이 먼저 공유한다. 공통 정보는 “진동은 붉은 균열 협곡 방향에서 더 강하다”이며 정확한 발생 위치를 확정하지 않는다. Path-Reactive Check: Hearing, 같은 Companion 역할·공통 Story State/Quest, 분기 ID TBD, 방향은 자막·장면 서술로도 전달한다.

태온: “오늘은 여기까지 확인하는 편이 좋겠습니다.”

폴: “찬성입니다. 확인과 무모함은 다른 일이니까요.”

미엘: “아르벨에 먼저 알려야겠어요.”

## 레온 보고와 완료 상태

보고는 다음처럼 구분한다. **관찰 사실**: 바람 없이 움직이는 재, 진동과 재 이동의 동시 발생, 불씨망령 목격·처치, 처치 후에도 남은 진동, 더 서쪽 협곡 입구 발견. **가설**: 지하 열기의 일부가 올라올 가능성. 재·진동의 선후/원인, 불씨망령 정체·최종 원인·세력은 미상이다.

레온: “그러면 동물들만 변하고 있는 게 아니군요.”

폴: “네. 적어도 이제는 그렇게 보는 편이 맞겠습니다.”

설계상 완료 후에는 Field07 개방/재방문, 불씨망령 일반 출현 후보 검토, 협곡 입구 발견 기록을 남긴다. 일반 Spawn 수·배치는 아직 확정하지 않는다. Arbel Safe Zone과 세린 Permanent Companion을 유지하고 사용자의 활성 파티·Formation을 덮어쓰지 않는다. Mystery 미해결, Overheat 미사용, Chapter2 Boss 미노출, Field07 BGM TBD다. 이는 미래 완료 상태 계약이며 현재 Runtime 완료 기록이 아니다.

## 세린 미편성과 후속 방향

세린을 Party Manager에서 제외했어도 Main16을 막지 않는다. 필요한 교차 확인은 현장 Story NPC 또는 기존 Story Override로 검토한다. Hearing 플레이어의 핵심 감지는 직접 수행할 수 있고, 다른 Path도 현장 정보 공유로 같은 핵심 정보에 도달한다. Permanent Party/Save 선택은 보존하며 대화를 위해 전투 편성을 강제 교체하지 않는다. 태온·미엘·폴의 Story 대사 참여와 실제 전투 편성도 구분한다.

Main17은 정식 Quest로 작성하거나 구현하지 않는다. **협곡 진입 → 열기 심화 → Elite → 작열 감시자 후보 → Overheat 첫 도입 후보 → 화염 중심부 접근**의 후속 방향만 둔다. Main16은 생물 변화만으로 설명하기 어려운 현상, Main17은 전투 규칙이 더 위험해지는 단계의 후보다. 작열 감시자·Overheat의 ID·수치·규칙은 몬스터 정본의 TBD를 유지한다.

## 구현 전 체크리스트

- Field07 Scene 가칭/번호/ID·World Bounds·카메라 viewport·실제 Exit Opening·떨어진 Spawn·협곡 미진입 경계 확정.
- Field06↔Field07 World Transition과 Arbel 귀환 경로 확정.
- 기존 Quest 시스템의12 Objective·목격/지정 승리 API·Target/Encounter/Navigation Stable ID 확정.
- Path-Reactive Dialogue와 분기별 ID 확정, LOCAL Hearing Path 검사 API 확인.
- 세린 Story NPC/Override 및 모든 대사 인물 미편성 처리, Permanent Party/Formation 보존.
- Ember Wraith 단독 Story Encounter·기존 스킬·도망/패배/재시도/승리 진행 검증.
- Quest Navigation·Save/Continue·구버전 호환·연출 중 재개/중복 방지·Arbel Return/Safe Zone 확인.
- Story/Path Branch/Dialogue ID 확정 후 Main16 TTS Manifest와 Voice 제작·매핑·청취 검수.
- Field07/Encounter BGM TBD 확정, 기존 타이틀/마을/첫 필드 곡 임의 재사용 금지.
- Runtime QA: 두 Path 분기 공통 정보/진행, 미편성 세린, Next/Skip/정리, Save 재개, World 왕복·전투·접근성.
