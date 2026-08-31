# Project-Limitless 프로젝트 컨텍스트

## 문서 역할

이 문서는 자주 바뀌는 작업 진행 상황이 아니라 Project-Limitless의 장기적인 설계 원칙과 이미 확정된 구조를 기록한다. 현재 완료 범위와 다음 작업은 `CURRENT_STATUS.md`, 실제 작업 절차는 `CODEX_WORKFLOW.md`에서 관리한다.

## 프로젝트

- Unity 6 기반 2D RPG다.
- 월드 탐험은 Top-Down 실시간 이동을 사용한다.
- 전투는 접근성과 전략을 고려한 턴제 JRPG 방향이다. 반응속도를 강요하지 않는다.
- 마을은 안전 지역이며 필드 탐험 중 몬스터와 조우하는 흐름을 지향한다.
- 향후 별도 탐험 공간인 인스턴스 던전을 기본 게임 루프와 연결한다.
- 재미를 최우선으로 하되 접근성을 희생하지 않고, 장애나 트라우마를 고정관념이나 초능력의 원인으로 표현하지 않는다.

## 캐릭터 생성 흐름

현재 Scene 흐름은 다음과 같다.

`Bootstrap → CharacterCreation → PathSelection → JobSelection → FinalConfirmation → World_StarterVillage`

`GameSessionData`가 이름, Male/Female 외형, 길 ID, 직업 ID와 Scene 전환용 Spawn ID를 실행 중에 유지한다. 이는 아직 영구 저장 데이터가 아니다.

## 길

길은 삶을 경험해 온 방식과 캐릭터 정체성, 기본 능력 성향, 고유 패시브를 담당한다. 직업의 선택을 제한하지 않는다.

현재 5개 길은 다음과 같다.

- 시각의 길
- 청각의 길
- 지적의 길
- 지체의 길
- 마음의 상처

길 데이터는 `PlayerPathDefinition`과 Resources의 PathDefinition Asset으로 관리한다. UI는 길의 수를 5개로 하드코딩하지 않으며 향후 새로운 길을 추가할 수 있는 목록형 구조를 사용한다.

## 길 외형

- 청각의 길: 헤드폰 Male/Female `CharacterVariant`
- 시각의 길: 바이저 Male/Female `CharacterVariant`
- 지체의 길: 전투형 전동 휠체어 Male/Female `WheelchairVariant`
- 지적의 길: 연결된 두 고리와 중앙 빛으로 구성된 `동행의 문장`
- 마음의 상처: 균열을 금빛 에너지가 잇는 `이어진 심장석`

Variant는 이동 로직과 분리된 Visual과 Animator를 교체한다. 지적의 길과 마음의 상처는 기본 캐릭터를 유지하고 UI Symbol로 정체성을 표시한다.

## 직업

현재 기본 직업은 다음 5개다.

- 수호자: 탱커
- 치유사: 힐러·지원
- 사수: 원거리 물리 딜러·적 후열 압박·사수 전용 야수 동료
- 투사: 근거리 물리 딜러
- 마도사: 마법 딜러·전장 제어

길과 직업은 독립 구조다. 추천 직업은 초보자 안내일 뿐이며 모든 길 × 모든 직업 조합을 선택할 수 있다. `JobDefinition`은 안정적인 ID, 표시 정보, 능력치 보너스, 패시브와 시작 스킬 프리뷰를 데이터로 보관한다.

## 직업 시작 스킬

다음 항목은 현재 `JobDefinition` Asset에 저장된 캐릭터 생성용 프리뷰 데이터다. 실제 전투 실행 로직은 아직 구현되지 않았다.

- 수호자: `도발`, `철벽`, `대신 막기`
- 치유사: `치유의 빛`, `회복의 파동`, `정화`
- 사수: `정조준`, `화살비`, `동료의 습격`
- 투사: `난도`, `회심의 일격`, `회오리 베기`
- 마도사: `파이어 볼`, `썬더볼트`, `가이아 웰`

세부 수치 중 미확정으로 표시된 항목은 전투 테스트 없이 임의로 확정하지 않는다. 실제 전투 구현 시 별도 SkillDefinition과 실행 로직으로 연결하거나 안전하게 마이그레이션한다.

## 사수 전용 야수 동료

사수는 마도사와 다른 방식으로 원거리 역할을 구분하기 위해 원거리 물리 공격과 적 후열 압박에 더해 사수 전용 야수 동료를 핵심 직업 정체성으로 사용한다. 기본 직업 시스템에서 다른 직업은 야수 동료를 장착하거나 데리고 다니지 않는다. 특별한 스토리 예외는 향후 별도 기획으로만 다룬다.

용어와 시스템 경계는 다음과 같이 구분한다.

- `Companion`: 플레이어와 함께 3인 파티를 구성하는 일반 동료 NPC. 파티 슬롯, `Combatant`, HP, Formation과 자기 턴을 가질 수 있다.
- `BeastCompanion`: 사수에게 별도로 귀속되는 야수 동료. 3인 파티 슬롯을 차지하지 않고 별도 `Combatant`, HP, Formation 슬롯이나 독립 턴을 갖지 않는다. 사수의 패시브와 특정 스킬 연출에만 관여한다.

보유 야수는 Wolf, Bear, Fox 3종을 유지하며 1차 기본 야수는 Wolf다. 향후 여러 야수 중 하나를 선택·장착할 수 있게 확장하되 선택의 핵심 차이는 `동료의 습격` 기본 피해 격차가 아니라 패시브 성향과 플레이 스타일에 둔다. Wolf는 공격형, Bear는 방어·생존형, Fox는 민첩·기동형 방향이며 정확한 수치와 선택 UI는 아직 미확정·미구현이다.

`동료의 습격`은 향후 현재 장착한 `BeastCompanion`이 등장해 공격하는 스킬로 확장한다. 1차 구현은 선택 시스템이 없으므로 Wolf를 사용하지만, 전투 화면 코드에 Wolf를 직접 고정하지 않고 향후 `BeastCompanionDefinition` 또는 동등한 데이터 구조에서 현재 장착 야수를 전달받을 수 있는 경계를 둔다.

## 그래픽과 사용자 Asset

- Male/Female 기본 캐릭터와 길별 Character Variant가 존재한다.
- 이동·충돌·NPC 상호작용·이름표는 Visual 교체와 독립적으로 유지한다.
- 기존 PNG를 Codex가 임의로 수정하거나 재생성하지 않는다.
- 사용자가 수동 수정한 Sprite를 최우선 원본으로 취급한다.
- ThirdParty 원본을 직접 수정하지 않으며 프로젝트 전용 수정본은 `Assets/_Project/` 아래에서 관리한다.

## 월드와 필드

- `World_StarterVillage`는 현재 안전 지역이다.
- `World_StarterVillage` 남문과 `Field_01` 북쪽 입구가 양방향으로 연결되어 있다.
- 모든 World/Field는 `WorldBounds2D`, Camera Bounds, 네 면 Boundary Collider를 가진다.
- 실제 `SceneTransitionTrigger`가 있는 Exit Opening만 외곽 경계를 통과할 수 있다.
- `SceneSpawnPoint`는 Trigger와 떨어뜨려 Scene 진입 직후 역전환되지 않게 한다.
- Camera는 Orthographic Size와 현재 aspect를 고려해 화면 전체가 맵 Bounds 밖으로 넘어가지 않게 한다.
