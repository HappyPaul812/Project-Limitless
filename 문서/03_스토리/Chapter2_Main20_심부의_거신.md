# Chapter2 Main20 — 심부의 거신

2026-10-09 사용자 요청으로 확정한 정식 설계. **DESIGN_CONFIRMED / IMPLEMENTED_BACKGROUND_QA_PASS**. 정본 수치·서사·Objective9와 승인된 기술 계약으로 Unity 구현 및 격리 Runtime QA를 수행했다. 과거 TBD보다 이 문서를 우선한다. 사용자 실제 화면·음악·전 직업 난이도 승인은 별도다. [최종 구현 및 QA](../00_프로젝트/Main20_Implementation_Final_QA_20261009.md).

## 2026-10-09 승인된 구현 계약

사용자 지시로 v3 아트 및 기본 Boss + Phase2 Overlay 채택을 승인했다. 최종 음악 청취는 별도 대기한다.

- HP600 이하 즉시 전환1회. Phase1 응축의 미실행 파동 예고가 있다면 다음 자기 행동에서 그 파동을 먼저1회 이행한 뒤 Phase2 열압 주입부터 순환한다. 예고가 없으면 다음 행동부터 Phase2 열압 주입. 추가 턴/Queue/HP/상태 초기화 없음.
- 용융 강타는 TargetRangeType.MeleePhysical, 도발 우선 및 기존 전열/후열 대상 규칙. 방어 무시/무조건 후열 관통 없음.
- Boss BaseExperience140에 기존 레벨 차 배율을 적용한다. Quest EXP100/Talent80과 전투 Talent40은 별개이며 각각1회.
- Field11 신규 구현은 기존 저장 Party/Formation을 유지하며 불필요한 일반 몬스터0. Overlay는 기본 프레임과 동기화하고14~15 소거를 유지한다.

정식 Scene/Quest/Boss/Overlay/VFX/BGM을 구현했다. v1~v3 검사 이력은 보존하며 실제 검증 범위와 미검증 사항은 최종 QA 문서를 따른다.

## 기본 계약과 서사 경계

| 항목 | 정식 값 |
| --- | --- |
| Quest | Main20 「심부의 거신」 |
| Quest ID | main_20_colossus_of_the_depths |
| Scene / 표시명 | Field_11_DeepCore / 열맥 심부 |
| 선행 | main_19_burning_pulse 완료 |
| Boss | 열맥 거신 / Veinfire Colossus / veinfire_colossus |
| Story Encounter ID | field11_main20_veinfire_colossus |
| 완료 보상 | EXP100 / Talent80（현재 Currency） |

Chapter2 주요 Boss전으로 Main18 과열 관리와 Main19 목격을 종합한다. Boss는 현재 서부의 열기·진동을 크게 증폭시키는 존재다. 모든 이상 현상의 최초 원인, Chapter1/2의 공통 최종 원인, 최종 흑막, 신이 만든 존재·신의 의도·세계 전체 사건의 근원은 확정하지 않는다.

처치 후 강한 열기·지표 화염·동물의 대규모 피난·서부 길 불안정이 눈에 띄게 완화된다. 약한 지하 진동과 더 깊은 미스터리는 남는다. 모든 문제가 해결됐다고 말하지 않는다. Main21은 후일담·서부 안정화·귀환·남은 미스터리·Chapter3 연결을 맡지만 상세 Quest/Dialogue는 여기서 확정하지 않는다.

## Boss 외형과 수치

검게 굳은 암석질 거체, 내부의 붉은 열맥, 거대한 준인간형 실루엣. 사람은 아니며 얼굴은 검은 머리 형태와 균열/열빛 정도다. 흔한 용암 골렘·화염 악마·뿔/날개/서양식 Devil·인간 얼굴·정교한 갑옷 기사·거대한 검·특정 게임 Boss 유사 디자인은 금지한다. LIMITLESS 고유 Boss다.

Main19 `BurningPulse_DistantFlameSilhouette.png`의 체격·어깨 너비·준인간형 비율·규모감과 연결한다. 해당 픽셀을 확대/복사하거나 Boss Sprite로 사용하지 않는다. [Art Handoff](../00_프로젝트/Main20_Art_Handoff.md)의 새 정식 디자인을 제작한다.

| Level | HP | Attack | Agility | Boss EXP | Boss Talent | IsBoss | Flee | Pet | 일반 Loot |
| ---: | ---: | ---: | ---: | ---: | ---: | --- | --- | --- | --- |
| 15 | 1200 | 30 | 10 | 140 | 40 | true | 불가 | 불가 | 없음 |

Boss 전투 보상 EXP140/Talent40과 Quest 완료 EXP100/Talent80은 별개로 각각 한 번만 지급한다. 도망·패배에 전투 보상0, 완료 전 Quest 보상0, 중복 승리/완료 지급0. 특수 장비·세계관 핵심 Item을 추가하지 않는다. 현재 EXP에는 레벨 차이 배율이 있으므로 140은 기존 데이터상 기본 EXP에 대응한다. 사용자 승인으로 BaseExperience140에 기존 레벨 차 배율을 적용한다. Lv15 동레벨140, 회색0 등 기존 배율을 유지한다.

## 전투 철학과 Phase

HP만 오래 깎는 전투가 아니라 패턴 인지·Overheat/Burn 관리·Guard·직업 방어 Skill·Healing·Cooling Remedy·공격 타이밍 판단을 요구한다. 새 전용 상태이상·Path 능력·Boss 약점 시스템을 추가하지 않는다. 이번에는 Path별 추가 Flavor도 만들지 않는다.

Phase1은 HP50% 초과（표현상 100~51%）, Phase2는 50% 이하. **HP600 이하가 처음 되는 순간 전환 정확히1회**. HP 회복·Battle 재시작·Turn Queue 전체 초기화·Party/Overheat/Burn/Buff/Debuff 초기화는 없다. 같은 전투를 이어간다. 전환 flag는 Boss Runtime·표현 상태이며 별도 스택 상태이상이 아니다.

전환 지문（VoiceExpected=false）:

> <지문> 열맥 거신의 몸체가 갈라지며 안쪽의 열핵이 드러납니다.

상태 표시는 「열핵 폭주」/Phase2다. HP600 경계에서 기존 응축 파동 예고를 보존하고 다음 자기 행동에 원래 파동을 먼저 실행한 뒤 Phase2 열압 주입부터 순환한다. 예고가 없으면 다음 자기 행동부터 열압 주입이다. 추가 턴·Queue·상태 초기화는 없다.

## Phase1 행동 순환

열압 주입 → 용융 강타 → 열핵 응축 → 열핵 파동 → 반복. Boss 자신의 실제 행동 기준 고정 순환이며 랜덤 스킬 난사는 없다.

| 행동 / 내부 영문 후보 | 직접 피해 | 부가 효과 / 대상 |
| --- | --- | --- |
| 열압 주입 / Heat Pressure Injection | 단일90% | 공용 Taunt 우선, 없으면 생존 아군 최고 Overheat Stack 우선, 동률 기존 안정 선택; 피해 후 생존 대상 Overheat+1 |
| 용융 강타 / Molten Strike | 단일125% | 명중 후 생존 대상에 기존 공용 Burn; Overheat 없음 |
| 열핵 응축 / Core Condensation | 없음 | 상태이상 없음; 다음 Boss 행동 「열핵 파동」을 명확히 예고 |
| 열핵 파동 / Core Wave | 생존 아군 전체80% | Overheat/Burn 없음; Taunt 무관 |

## Phase2 행동 순환

열압 주입 → 용융 강타 → 열핵 공명 → 열핵 분출 → 열파 → 반복. 열압 주입·용융 강타는 Phase1과 같은 계약이다.

| 행동 / 내부 영문 후보 | 직접 피해 | 부가 효과 / 대상 |
| --- | --- | --- |
| 열핵 공명 / Core Resonance | 없음 | 다음 Boss 행동 「열핵 분출」이 생존 아군 전체에 직접 피해와 Overheat+1을 남긴다고 예고 |
| 열핵 분출 / Core Eruption | 생존 아군 전체60% | 각 대상 직접 피해 처리 후 살아남은 아군마다 Overheat+1; Burn 없음; Taunt 무관 |
| 열파 / Heat Wave | 생존 아군 전체55% | Overheat/Burn 없음; Taunt 무관 |

모든 직접 피해는 기존 올림·최소1·주는 피해/받는 피해 규칙과 Guard·철벽·가이아 웰·수호의 맹세·Path/Beast 직접 피해 경로를 사용한다. 광역은 각 생존 대상을 한 번씩 처리한다. 단일 용융 강타는 승인된 MeleePhysical로, 도발 우선과 기존 전열 보호·빈 전열의 후열 선택 규칙을 적용한다. 방어 무시와 무조건 후열 관통은 없다. 열압 주입은 감시자 선례의 전열+후열 선택과 Taunt 우선 구조를 재사용한다.

## 기존 상태 계약 유지

- Overheat: 0→1→2→3 도달 즉시 `max(1, ceil(MaxHP×0.08))` 상태 고유 피해 후 Stack0. Direct/DoT가 아니며 Guard·철벽·가이아·수호의 맹세·Path/펫 직접 방어 적용/소비 없음. KO 가능. KO/전투 종료/새 전투에서 정리한다. HUD 아이콘+「과열 1/3」「과열 2/3」·위험 텍스트 유지.
- Cooling Remedy: 과열 전부 제거. Stack0 대상은 소모0·행동0. 정화와 기존 네 해제약은 과열을 제거하지 않는다.
- Burn: `BattleStatusEffectRuntime.ApplyOrRefreshBurn` 재사용. 적용 당시 Boss Attack30의 30%=원시9 피해를 저장하고 대상 성공 행동 종료2회에 틱. 비중첩·재적용 피해/횟수 갱신·기존 정화/화상 연고·전투 종료 정리·야수 Burn 방어 유지. 별도 Boss Burn 수치/시스템 없음.
- Phase 전환에서 위 상태·도발·보호·쿨타임을 지우지 않는다. 예고는 다음 자신의 행동까지 Boss 상태/HUD 텍스트로 읽을 수 있게 유지하며 색/음성/VFX만으로 정보를 전달하지 않는다. 빠른 Flash·흰색 Strobe·강한 반복 Shake 금지. 반응속도 입력을 요구하지 않는다.

## Quest Objective — 정확히9개

사용자가 허용한 공용 타입 중 현재 안전한 ReachLocation/Interact/DefeatEncounter를 선택한다. target ID는 후속 구현과 Navigation이 공유할 문서 계약이다.

| 순서 | objectiveId | type | target | 내용 |
| ---: | --- | --- | --- | --- |
| 1 | enter_deep_core | ReachLocation | field11_main20_entry | 열맥 심부로 들어가세요. |
| 2 | inspect_core_rift | Interact | field11_main20_core_rift | 심부의 거대한 균열을 조사하세요. |
| 3 | follow_colossus_trace | ReachLocation | field11_main20_trace | 거신의 흔적을 따라가세요. |
| 4 | reach_colossus_arena | ReachLocation | field11_main20_arena | 심부의 열린 지대로 이동하세요. |
| 5 | confront_veinfire_colossus | Interact | field11_main20_confront | 열맥 거신을 확인하세요. |
| 6 | defeat_veinfire_colossus | DefeatEncounter | field11_main20_veinfire_colossus | 열맥 거신을 쓰러뜨리세요. |
| 7 | inspect_collapsed_core | Interact | field11_main20_collapsed_core | 무너진 거신의 흔적을 조사하세요. |
| 8 | confirm_heat_recession | Interact | field11_main20_heat_recession | 주변 열기의 변화를 확인하세요. |
| 9 | return_from_deep_core | ReachLocation | field11_main20_return | 서부로 돌아갈 길을 확보하세요. |

목표5의 명시 대화가 끝나면6으로 진행한다. Boss 재도전은6에서 같은 Encounter를 호출하며 전투 전 대화를 필수 반복하지 않는다. 승리만6을 한 번 진행, 패배는6 유지·공용 필드 복귀/회복 후 재도전. 목표7에서 전투 후 대화·잔해 조사,8에서 열기 완화 확인,9에서 귀환 길 확보 후 Main20 종료·Main21 Hook. 다음 Quest 상세는 미정이다.

## 정식 Dialogue와 TTS

아래 본문은 [Manifest](../00_프로젝트/Main20_TTS_Manifest.csv)의 Stable ID와 정확히 대응한다. 세린5개 companion_serin/Schedar, Player2개와 지문1개 무음. **TTS_PENDING5 / NOT_EXPECTED3 / 전체8**, API0·신규 WAV 생성0·음성 Import0·텍스트/Stable ID8 연결. 기존 Main18/19 상태는 변경하지 않는다.

| 위치 | Dialogue ID | 화자 | 본문（↵는 실제 줄바꿈） |
| --- | --- | --- | --- |
| 목표5 전투 전 | main20_confront_serin_01 | 세린 | 저 존재가 움직일 때마다 지면의 맥동이 따라옵니다.↵가까이 오니 더 분명해졌어요. |
| 목표5 전투 전 | main20_confront_player_01 | Player | 그렇다고 저게 모든 일의 시작이라고 볼 수는 없겠죠. |
| 목표5 전투 전 | main20_confront_serin_02 | 세린 | 네. 하지만 지금 이 지역의 열기를 크게 키우고 있는 건 확실해 보여요. |
| 목표5 전투 직전 | main20_confront_serin_03 | 세린 | 열핵이 뛰는 간격을 보세요.↵공격하기 전마다 반응이 달라집니다. |
| Battle Phase 전환 | main20_phase2_direction_01 | 지문 | <지문> 열맥 거신의 몸체가 갈라지며 안쪽의 열핵이 드러납니다. |
| 목표7 전투 후 | main20_after_colossus_serin_01 | 세린 | 열기가 내려가고 있어요.↵적어도 이 존재가 크게 증폭시키고 있던 건 맞았던 것 같습니다. |
| 목표7 전투 후 | main20_after_colossus_player_01 | Player | 그런데 진동은 남아 있군요. |
| 목표7 전투 후 | main20_after_colossus_serin_02 | 세린 | 네. 훨씬 약해졌지만…↵완전히 멈추지는 않았어요. |

Boss KO 후 몸체의 강한 불꽃 약화·검게 굳은 암석 붕괴·지면 적열 감소·뜨거운 바람 감소를 보여 준다. 목표7 대화는 승리 직후 필드 복귀 상태와 일치해야 한다. Continue에서 완료한 일회 연출/보상이 재발동하지 않도록 Quest 목표/완료 기록으로 표현 상태를 복원한다.

## Field11 / Scene / 제작 계약

검은 암석·더 깊고 굵은 붉은 열맥·압력 균열·거신 이동 흔적·중앙 열린 Boss 공간·주변 붉은 맥동·깊은 틈·흑요석화 암석. Main19보다 강한 열기지만 여전히 갈라진 지표/심부 대지이며 용암 바다·화산 내부·지옥·악마 성·닫힌 Boss Dungeon 방은 금지한다.

Geometry/크기/좌표/Collider는 Art와 구현 전 기술 검토 후 확정한다. Main19 Field10 서쪽 현재 폐쇄 통로는 선행 완료 확인 후 Field11과 왕복으로 연결할 후속 대상이다. Scene/Build Settings/FieldConnection은 아직 없다. 모든 Field에 World Bounds·viewport 고려 Camera 제한·실제 Exit만 통과 가능한 Boundary·Spawn/Trigger 비중첩을 적용한다.

Boss 집중 구간으로 필수 신규 일반 몬스터0·필수 잡몹전0. 우회 가능한 기존 소수 Encounter는 필요 시 후속 선택하며 이번에는 배치·수를 확정하지 않는다. 저장 Party/Formation/Beast를 보호하고 새 임시 편성을 임의 확정하지 않는다.

Field 탐색/복귀 **Paths of Cracked Earth** 기존 Project Asset 재사용. Boss **The Weight of Crowns** 승인 원본 사용 예정, 이번 Import0. **Iron and Incantation 미사용**, 미래 마법형 강적/중간보스 후보 유지. [Audio Handoff](../00_프로젝트/Main20_Audio_Handoff.md) · [정적 설계 QA](../00_프로젝트/Main20_Design_QA.md).
