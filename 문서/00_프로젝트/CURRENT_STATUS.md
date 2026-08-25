# Project-Limitless 현재 개발 상태

## 기준

- 갱신일: 2026-08-25
- 기준 브랜치: `main`
- 마지막 기능 관련 commit: `f98746f` (`Feature: 3대3 프로토타입 전투 확장`)
- 마지막 오류 수정 commit: `1594054` (`Fix: Projectile 이동시간 조정`)
- 마지막 관련 문서 commit: `246300e` (`Docs: 전투 설계 규칙 정리`)
- 마지막 전투 UI 관련 commit: `815dc49` (`Refactor: 전투 HP HUD 상단 재배치`)

이 문서는 완료된 기능과 미구현 범위를 빠르게 파악하기 위한 상태 요약이다. 세부 설계는 각 시스템 문서를 따른다.

## 구현 확인된 항목

### 캐릭터 생성과 선택

- `Bootstrap → CharacterCreation → PathSelection → JobSelection → FinalConfirmation → World_StarterVillage` Scene 흐름
- 이름 입력과 검증
- Male/Female 기본 외형 선택과 세션 유지
- 5개 길 선택 및 길별 능력치·패시브 프리뷰
- 길별 추천 직업 안내와 모든 5×5 조합 선택 허용
- 5개 직업 선택
- 직업 능력치, 패시브, 시작 스킬 3개 프리뷰
- FinalConfirmation의 선택 결과와 최종 능력치 표시

### 플레이어 외형과 월드 표시

- Player Nameplate
- Path Visual 데이터와 UI Preview
- 청각·시각·지체 길의 Male/Female Character Variant
- 지체 길의 전투형 전동 휠체어 Variant
- 지적 길의 동행의 문장 Symbol
- 마음의 상처 길의 이어진 심장석 Symbol
- 기본/Variant 공통 Player 이동과 방향 Animation 구조

### 마을과 필드

- `World_StarterVillage`의 Player 이동, Camera 추적, NPC E 대화
- `Field_01` Scene과 야외 환경
- 마을 남문 → Field_01 북쪽 입구 이동
- Field_01 북쪽 입구 → 마을 남문 복귀
- `SceneTransitionService`, `SceneTransitionTrigger`, `SceneSpawnPoint`
- 공통 `WorldBounds2D`와 `WorldBoundaryGeneratorUtility`
- Orthographic viewport 크기를 고려한 Camera Bounds
- Exit Opening을 제외한 월드 외곽 Collider
- 마을 외곽 울타리의 Tile 영역 밖 돌출 방지

### 첫 필드 몬스터

- 데이터 기반 `MonsterDefinition`과 Scene별 `FieldMonsterSpawnDefinition`
- Field_01 여러 빈터의 `초원 슬라임` 5마리 데이터 기반 런타임 배치
- 스폰별 시작 위치와 활동 반경 안의 독립적인 느린 무작위 배회
- Rigidbody2D 기반 장애물 Collider 충돌
- 플레이어 접촉 시 이동 정지와 `MonsterEncounterService.EncounterStarted` Event 발생
- 사용자 제공 4×4 Sprite 시트 기반 초원 슬라임 방향별 Idle/Walk Animation
- 배회 방향·이동/대기 상태와 `GrassSlime.controller` 동기화
- 초원 슬라임 데이터에 실제 Sprite/Animator 연결 및 녹색 Placeholder 미사용
- 승리한 스폰만 제거하고 데이터 기본값 30초 후 원래 위치에 독립 리스폰, 도망 시 스폰 유지
- `MonsterDefinition.DisplayName`을 표시하는 재사용 가능한 필드 몬스터 Overlay 이름표
- 흰색 글자·검은 외곽선의 이동 추적 이름표, 필드 HP Bar 미포함

### 1차 턴제 전투

- 재사용 가능한 `Battle` Scene과 Build Settings 연결
- `Combatant`, 전열/후열 2×3 `Formation`, `TargetResolver`, `TurnOrderQueue` 분리
- 근거리 전열·직선 후열 보호, 원거리 후열 우선, 마법 자유 대상 판정
- 플레이어와 몬스터 공통 대상 판정 및 단일 행동 도발 강제 대상 구조
- 민첩과 행동 우선도 기반 순서 및 다음 행동 타임라인
- 플레이어 이름·현재 외형, 초원 슬라임 Sprite와 양측 HP 표시
- 마우스·키보드 지원 남색/금색 전투 UI와 공격 가능·불가능 텍스트 구분
- 기본 공격, 다음 행동까지 피해 50% 감소 방어, 일반전 도망
- HP 0 전투불능, 적 전멸 승리, 승리·도망·패배 후 `Field_01` 복귀
- 복귀 시 전투 상태 초기화, 접촉 위치 이격과 2초 조우 유예
- 스킬 버튼과 NPC 직접 지시 확장용 `IsPlayerControlled` 구조
- 기본 공격 사거리: 수호자·투사 근거리, 사수 원거리, 마도사·치유사 마법. 치유사는 낮은 피해의 마법 공격
- 논리 2×3 Formation을 숨기고 적 왼쪽·아군 오른쪽의 사이드뷰 전장 좌표로 Sprite 배치
- 타임라인 아래·전장 위의 상단 HP 전용 HUD와 적군 3열×최대 2줄·아군 3열×1줄 이름/비율 HP 항목
- 캐릭터 근처 이름·HP Bar 제거, 행동 중·방어·도발·대상처럼 즉시 필요한 전장 표식만 유지
- Sprite 위 전용 상태 Anchor와 상단 HP HUD의 구조적 분리, 상세 팝업의 HUD·명령 패널 회피 배치
- Hover/마우스 선택/키보드 포커스 공용 단일 상세 상태 팝업과 포커스 캐릭터·고정 HP 행 동시 강조
- 공격 가능 대상은 금색 바닥 마커, 선택 대상은 화살표, 공격 불가 대상은 어두운 Sprite로 표현
- 상단 한글 `전투` 제목과 현재 행동자 강조·이후 순서 타임라인, 하단 전용 명령 패널
- 조작 안내를 14px 글씨로 명령 패널 내부 하단에 배치하고 버튼과 하단 padding을 확보
- 외부 신규 에셋 없이 남색·금색 하늘·원경·지면 층의 임시 전투 배경 구성
- 플레이어·NPC·몬스터가 공통 사용 가능한 `BattleActionPresenter`와 전투 계산 분리
- 근거리 기본 공격의 짧은 전진·타격 대기·원위치 복귀, 피격 좌우 흔들림·점멸, 떠오르는 피해 숫자
- 연출 중 명령·대상 선택·취소 입력 잠금과 연출 완료 후 다음 턴 진행
- 사수 기본 공격의 짧은 조준, 재사용 가능한 UI Projectile 이동, 도착 시 피해·피격 연출과 다음 턴 연결
- 마도사·치유사 기본 공격의 짧은 캐스팅과 Projectile, Fireball·밝은 금빛 구체 시각 구분
- 필드 Animator 현재 상태와 분리된 전투 Sprite 해석기, 아군 Left Idle·적 Right Idle 진입 및 공격 후 복구
- CC0 Polar_34 - Projectiles 원본 GIF 보존, 32×32 PNG Sprite 프레임 변환 및 사수 golden arrow·마도사 fireball 기본 공격 적용
- 시작·목표 X 좌표 비교 기반 공용 Projectile 좌우 반전과 GIF 프레임 지연 재생
- PVFX Foundry 0.3.0 CC0 원본·라이선스 보존, Magical Projectile travel 5프레임을 치유사 기본 공격에 적용
- Radiant Heal은 기본 공격에서 제외하고 향후 치유사 치유의 빛 스킬 VFX 후보로 보류
- JobDefinition 프리뷰를 사용하는 재사용 가능한 전투 스킬 카탈로그·실행기·참가자별 쿨타임·상태효과 런타임
- 실제 스킬 메뉴와 Esc 복귀, 미구현 스킬 비활성 표시, 수호자 도발 제자리 강조 연출
- 수호자 도발의 적 전체 적용, 적별 다음 2회 행동 소모, 수호자 행동 기준 3턴 쿨타임과 적 HUD 상태 표시
- 참가자 목록 기반 `BattleEncounterSetup`과 3대3 프로토타입 Factory, 기존 2×3 Formation을 사용하는 실제 N대N 전투 생성
- 플레이어·태온(수호자)·미엘(치유사)의 플레이어 직접 조작과 독립 HP 미니 HUD
- 전열 슬라임 2명·후열 슬라임 1명의 독립 Combatant·턴·HP·도발 상태와 6명 전체 행동 타임라인
- 적·아군 후보를 공통 처리하는 대상 선택 UI 기반과 태온·미엘 코드 생성 임시 Visual
- 현재 HP·직업/몬스터 분류·방어·도발·구현 스킬 쿨타임을 계산 코드 변경 없이 조합하는 `BattleCombatantStatusViewModel`
- 방어·도발 표식을 안정적 ID와 수치로 분리해 향후 무료 아이콘 Asset으로 교체 가능한 표시 구조

## 데이터만 있고 실행 로직이 없는 항목

- 길 능력치와 고유 패시브의 전투 효과
- 직업 능력치, 패시브, 시작 스킬 프리뷰
- AP, 상태이상, 협동 기술, 보스 패턴의 초안 방향

이 항목들은 캐릭터 생성 UI에서 표시되지만 실제 전투 계산이나 효과로 실행되지 않는다.

## 문서화된 전투 설계

- 아군·적 공통 전열 3칸과 후열 3칸의 2×3 진형
- 근거리·원거리·마법 사거리와 바로 앞 전열의 후열 보호
- 단일 적대 행동에 일반 사거리보다 우선하는 도발 강제 대상
- 민첩·행동 우선도 기반 솔로 순서와 시간 제한 없는 직접 행동 지시
- 공용 공격·스킬·방어·도망 명령 및 승리·도망 필드 복귀
- 멀티플레이는 동시 입력, 기본 45초 제한, 남은 시간 10초 경고, 시간초과 시 방어로 설계되어 있으나 네트워크는 미구현
- AP, 상태이상, 행동·협동 기술, 보스 패턴은 미확정이며 구현하지 않음

## 미구현

- 수호자 도발 외 나머지 직업별 스킬 효과와 길 패시브
- NPC 동료 정식 CompanionDefinition·파티 편성·최종 Sprite
- 여러 몬스터 배치 전투와 보스전 실제 콘텐츠
- AP와 상태이상, 행동·협동 기술, 보스 패턴
- 멀티플레이 네트워크 전투
- 인벤토리·아이템·경험치·보상·성장
- 퀘스트와 영구 저장·불러오기
- 인스턴스 던전과 Field_02 이후 지역

## 현재 검증 상태

- Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조 응답과 Roslyn으로 새 전투 코드를 포함해 컴파일했으며 Compiler Error 0개를 확인했다.
- 치유사 기본 공격을 마법 사거리로 매핑하고 기존 검증용 공격력보다 4 낮게 적용한 뒤 동일한 Unity 참조로 Compiler Error 0개를 재확인했다.
- 임시 실행 테스트로 근거리 보호·후열 개방, 원거리 후열 우선, 마법 자유 대상, 도발 우선·2회 지속, 방어 50%, 행동 우선도·민첩 정렬의 12개 검증을 모두 통과했다.
- `Battle.unity` GUID와 Build Settings GUID 일치, Battle 항목 1개, 새 C# meta 3개와 `git diff --check` 통과를 확인했다.
- 사용자가 Unity Play Mode에서 `Field_01 → 초원 슬라임 → Battle` 진입, 기본 공격, 적 자동 공격, 턴 순환을 정상 검증했다.
- 사용자가 방어 시 받는 피해가 10에서 5로 감소해 50% 방어 규칙이 정상임을 확인했다.
- 사이드뷰 UI 변경 후 Unity 전체 `Assembly-CSharp` 참조로 Compiler Error 0개를 확인했다. 실제 사이드뷰 화면 배치와 입력 회귀는 사용자가 다시 확인해야 한다.
- 조작 안내를 명령 패널 내부로 옮긴 뒤 동일한 Unity 전체 참조로 Compiler Error 0개를 확인했다. 16:9에서의 하단 padding과 버튼 간격은 사용자가 직접 확인해야 한다.
- HP Fill의 실제 Rect 폭 갱신과 스폰 ID별 처치·30초 리스폰 구조를 적용한 뒤 Unity 전체 `Assembly-CSharp` 참조로 Compiler Error 0개를 확인했다.
- 사용자 Play Mode 확인에서 한 마리만 생성되는 문제를 재현했고, Unity Editor 로그에서 `grass_slime_02`~`05`의 meta YAML 마지막 줄바꿈 누락으로 GUID가 무효 처리되어 Asset import가 제외된 원인을 확인했다.
- 새 4개 meta를 정상 형식으로 수정하고, Installer의 실제 로드 개수·고유 ID 로그와 Field01SceneGenerator의 5개 ID·30초·최소 3유닛 간격 검증을 추가했다. Runtime/Editor C# 컴파일 오류 0개를 확인했으며 Play Mode 재검증이 필요하다.
- `BattleCore.cs`가 변경되지 않았으며 공격 10, 방어 50%, Formation·TargetResolver·TurnOrderQueue 계산 규칙을 유지했다. 실제 HP Bar 비율과 필드 리스폰 시간은 Play Mode에서 사용자가 확인해야 한다.
- 이전 사이드뷰 UI 작업에서는 `BattleCore.cs`와 조우/복귀 코드가 변경되지 않았음을 정적으로 확인했다. 이번 작업은 `BattleSceneFlow.cs`의 승리·도망 결과 전달만 확장했으며 전투 계산 규칙은 변경하지 않았다.

- 월드 전환 Scene/Spawn ID, Missing Script, Bounds 참조와 viewport 계산은 정적으로 확인했다.
- 초원 슬라임 Script GUID, Monster/Spawn Asset 연결, Field_01 대상 Scene과 조우 Event 구조를 정적으로 확인했다.
- 초원 슬라임 PNG를 1256×1256, 314×314 Cell의 4×4 구조로 확인하고 Sprite 16개, Animation Clip 8개, Animator와 데이터 참조를 정적으로 교차 확인했다.
- `MonsterNameplate`가 몬스터별 Text를 분리하고 `DisplayName`을 받으며 Camera 이동 뒤 화면 좌표를 갱신하는 구조를 정적으로 확인했다.
- Unity 6.5에서 오류가 된 `GetInstanceID()`를 권장 API인 `GetEntityId()`로 교체했다.
- Unity 6000.5.7f1이 사용하는 Roslyn과 전체 `Assembly-CSharp` 응답 파일로 재컴파일하여 Compiler Error 0개와 종료 코드 0을 확인했다.
- 최신 Unity Editor 로그에서 `MissingReferenceException`과 `NullReferenceException` 기록이 없음을 확인했다. 실제 Play Mode 기능 검증은 사용자가 직접 확인해야 한다.
- 관련 C# 변경은 `git diff --check`를 통과했다.
- 기본 공격 액션 연출 코드를 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조와 Roslyn으로 컴파일해 오류 0개를 확인했다. 기존 API deprecation 경고만 남아 있다.
- `BattleCore.cs`, `TargetResolver`, `Formation`, 피해·방어 계산은 변경하지 않았다. 근거리 연출을 유지하고 사수 원거리 기본 공격만 Projectile 연출에 연결했으며, 마법 기본 공격은 기존 즉시 처리 흐름을 유지했다.
- 사수 Projectile 코드를 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조와 Roslyn으로 컴파일해 오류 0개를 확인했다. 조준 시간·Projectile 위치와 방향·입력 잠금은 Play Mode 확인이 필요하다.
- 기본·Path Variant·휠체어·초원 슬라임 Animator Controller에서 `Idle_Left`·`Idle_Right` 상태를 확인하고, `BattleVisualResolver`를 포함한 전체 `Assembly-CSharp` 컴파일 오류 0개를 확인했다. 실제 방향과 첫 프레임은 Play Mode 확인이 필요하다.
- `AnimationClip.SampleAnimation`이 Sprite PPtr 곡선을 적용하지 못하던 경로를 임시 Animator 상태 평가로 교체하고, Path Variant 기본 Sprite fallback과 null UI 투명 처리를 추가했다. 전체 `Assembly-CSharp` 컴파일 오류 0개이며 흰 사각형·경고 제거는 Play Mode 확인이 필요하다.
- 마도사·치유사 기본 마법 Projectile과 런타임 Orb Graphic을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 피해·치유사 감소값·마법 사거리 계산은 변경하지 않았으며 색상·도착 시점·Idle 복구는 Play Mode 확인이 필요하다.
- golden arrow 8프레임(80ms), fireball 7프레임(60ms)을 원본 32×32 크기로 추출하고 복사 전후 SHA-256 일치를 확인했다. 실제 에셋 연결 후 전체 Assembly-CSharp 컴파일 오류 0개이며 표시 크기·방향·프레임 재생은 Play Mode 확인이 필요하다.
- 사수 golden arrow와 마도사 fireball 이동시간을 각각 0.35초로 조정하고, 프레임 간격·도착 후 피해 적용·치유사 Projectile 0.24초·근거리 속도는 유지했다. 전체 Assembly-CSharp 컴파일 오류 0개이며 체감 속도는 Play Mode 확인이 필요하다.
- 치유사 임시 Orb를 PVFX Magical Projectile의 96×96 travel 프레임 5개로 교체했다. manifest 픽셀 해시 일치, Point Filter·투명 Sprite·50ms 프레임·0.24초 이동 유지와 전체 Assembly-CSharp 컴파일 오류 0개를 확인했다.
- 수호자 도발 스킬 구조를 Unity 전체 Assembly-CSharp 참조로 컴파일해 오류 0개를 확인했다. Combatant·TargetResolver 기존 도발 우선 판정을 재사용하고 BattleCore·Formation·TurnOrderQueue는 수정하지 않았다. 메뉴 조작·HUD 배치·2회 소모·3턴 쿨타임은 Play Mode 확인이 필요하다.

- 3대3 Encounter와 공용 대상 선택 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. `BattleCore`·기존 스킬 런타임·ActionPresenter·BattleVisualResolver는 변경하지 않았으며 실제 3대3 UI·입력·도발 분산은 Play Mode 확인이 필요하다.
- 전투 상태 UI와 ViewModel을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 전투 계산·N대N·대상 판정·연출 코드는 변경하지 않았으며 16:9 팝업 위치와 마우스/키보드 동작은 Play Mode 확인이 필요하다.
- 고정 HP 목록 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. `BattleCore`·Formation·TargetResolver·TurnOrderQueue·도발·피해 계산과 하단 명령 패널은 변경하지 않았으며, 좌우 목록 배치·실제 HP 비율·포커스 행 연동은 Play Mode 확인이 필요하다.
- 상단 HP HUD 재배치를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. HP HUD·전장·상태 Anchor·팝업 안전 영역만 변경했으며 N대N·Formation·Combatant·대상/턴/도발/피해 계산·명령·연출 코드는 변경하지 않았다.
- Working Tree에는 이번 문서 작업과 무관한 사용자 Asset·Scene·ProjectSettings 변경이 남아 있으며 이 상태 문서는 해당 미커밋 변경의 완성 여부를 판단하지 않는다.

## Unity에서 사용자가 직접 확인할 사항

1. Play Mode를 종료해 Asset 재import를 완료한 뒤 다시 시작하고, Console에 `필드 몬스터 배치 로드: Field_01, 5개 [grass_slime_01, ..., grass_slime_05]`가 출력되는지 확인한다.
2. HP가 100%에서 50%가 되었을 때 숫자와 함께 HP Bar 길이도 정확히 절반이 되는지 확인한다.
3. Player와 초원 슬라임 양쪽 HP Bar가 피해 직후 정상 감소하고, HP 0에서 완전히 비는지 확인한다.
4. 슬라임 처치 후 `Field_01` 복귀 시 조우했던 해당 슬라임만 사라져 있는지 확인한다.
5. 처치하지 않은 다른 슬라임들은 그대로 존재하며 계속 배회하는지 확인한다.
6. 약 30초 후 처치한 스폰의 시작 위치에서 슬라임이 다시 나타나는지 확인한다.
7. 도망한 경우 조우했던 슬라임이 사라지지 않고, 기존 2초 재조우 유예 뒤 정상 배회하는지 확인한다.
8. `Field_01`에 총 5마리의 초원 슬라임이 서로 다른 활동 반경 안에서 독립적으로 배회하는지 확인한다.
9. Console에 Compiler Error, MissingReferenceException, NullReferenceException이 없는지 확인한다.
10. 근거리 직업으로 플레이어 기본 공격 시 대상 앞까지 짧게 전진하고, 타격 후 원래 자리로 복귀하는지 확인한다.
11. 초원 슬라임 공격에도 같은 전진·복귀가 적용되고, 양측 피격 시 좌우 흔들림·짧은 점멸과 `-피해량` 숫자가 표시되는지 확인한다.
12. 연출 중 공격·스킬·방어·도망·대상 선택·Esc 취소가 중복 실행되지 않고, 연출 종료 뒤 다음 턴 입력이 정상 복구되는지 확인한다.
13. 공격 10과 방어 중 피해 5, HP 숫자·Bar, 행동 순서 타임라인, 승리·도망·패배 및 Field_01 복귀·리스폰이 기존과 동일한지 확인한다.
14. 사수 직업으로 기본 공격 시 제자리에서 짧게 조준하고 golden arrow 애니메이션이 약 0.35초 동안 왼쪽 적을 향해 이동하는지 확인한다.
15. Projectile 도착 순간에만 HP와 피해 숫자가 갱신되고 기존 흔들림·점멸 뒤 다음 턴으로 정상 진행되는지 확인한다.
16. Projectile 이동과 피격 연출 중 명령·대상 선택·Esc가 중복 실행되지 않는지 확인한다.
17. 필드에서 플레이어가 걷거나 상·하·우 방향을 보는 중 조우해도 Battle에서는 기본·Path Variant·휠체어 모두 Left Idle 첫 프레임인지 확인한다.
18. 초원 슬라임이 필드에서 이동하거나 다른 방향을 보는 중 조우해도 Battle에서는 Right Idle 첫 프레임인지 확인한다.
19. 근거리·Projectile 공격 종료 후 양측이 각자의 Left/Right 전투 Idle Sprite로 복귀하는지 확인한다.
20. 남자 지체의 길 수호자와 초원 슬라임이 흰 사각형 없이 표시되고, `Battle Visual` Warning·NullReference·Compile Error가 없는지 확인한다.
21. 마도사 기본 공격 시 제자리 캐스팅 후 fireball 애니메이션이 약 0.35초 동안 왼쪽의 전열 또는 후열 대상까지 이동하고, 도착 순간에만 HP·피해 숫자가 갱신되는지 확인한다.
22. 치유사 기본 공격 시 PVFX Magical Projectile이 0.24초 동안 왼쪽 대상을 향해 재생되고, 도착 순간 피해와 기존 낮은 피해량·마법 자유 대상 규칙이 유지되는지 확인한다.
23. 두 마법 연출 중 입력이 잠기고 완료 후 Left Idle과 다음 턴이 복구되며, 치유사 Magical Projectile과 근거리 연출도 정상인지 확인한다.
24. 테스트용으로 시작 X보다 목표 X가 큰 배치를 구성할 수 있을 때 golden arrow와 fireball이 원본 오른쪽 방향으로 표시되는지 확인한다.
25. 수호자로 Battle에 진입해 스킬 → 도발을 마우스와 키보드로 선택하고, Esc로 명령 메뉴에 복귀되는지 확인한다.
26. 도발 사용 시 수호자가 제자리에서 강조되고 도발! 텍스트 뒤 슬라임 HUD에 도발 2가 표시되는지 확인한다.
27. 슬라임 첫 행동 완료 후 도발 1, 두 번째 행동 완료 후 표시 제거를 확인한다.
28. 수호자 다음 행동 차례의 메뉴에 도발 [재사용 2턴], 이후 1턴, 종료 후 다시 사용 가능 상태가 표시되는지 확인한다.
29. 쿨타임 중 도발 버튼이 실행되지 않으며 철벽 방어·대신 막기와 다른 직업 스킬이 [미구현]으로 안전하게 표시되는지 확인한다.
30. 공격·방어·도망과 기존 근거리/Projectile 연출, HP Bar, 승리·패배·Field_01 복귀·30초 리스폰을 회귀 확인한다.
31. Console에 Compile Error, NullReferenceException, MissingReferenceException이 없는지 확인한다.

32. Battle 진입 시 플레이어·태온·미엘과 초원 슬라임 A·B·C가 3대3으로 겹치지 않고 표시되며 각 HUD에 이름·직업·HP가 보이는지 확인한다.
33. 타임라인에 여섯 참가자의 민첩 기반 순서가 표시되고 플레이어·태온·미엘 차례마다 시간제한 없이 직접 명령할 수 있는지 확인한다.
34. 전열 슬라임 A·B와 후열 슬라임 C에 근거리·원거리·마법 기본 공격 대상 규칙이 기존대로 적용되는지 확인한다.
35. 슬라임 하나의 HP가 0이 되어도 다른 두 슬라임의 HP·행동이 유지되고, 세 마리 전멸 때만 승리하는지 확인한다.
36. 태온 또는 플레이어 수호자의 도발 후 세 슬라임 HUD가 모두 도발 2가 되고, 각 슬라임 행동 때 자기 표시만 1로 감소한 뒤 두 번째 행동 후 사라지는지 확인한다.
37. 도발 중 세 슬라임의 단일 공격이 시전자 수호자에게 집중되고, 시전자 행동 기준 재사용 2턴→1턴→사용 가능인지 확인한다.
38. 아군 한 명 전투불능 시 그 참가자만 행동에서 제외되고 나머지 전투가 계속되며, 아군 세 명 전멸 때만 패배하는지 확인한다.
39. Console에 Compile Error, NullReferenceException, MissingReferenceException이 없는지 확인한다.

40. 16:9 Battle이 제목 → 타임라인 → 상단 HP HUD → 실제 전장 → 하단 명령 패널 순서로 표시되고 각 영역이 겹치거나 잘리지 않는지 확인한다.
41. 현재 3대3에서는 상단 HUD의 적군 첫 줄과 아군 한 줄에 각각 3개 HP 항목만 표시되고, 캐릭터 주변 이름·HP Bar가 없는지 확인한다.
42. 참가자 수를 줄인 Encounter에서는 빈 HP 항목이 생기지 않고, 적 4~6명 구성에서는 적군이 한 줄 최대 3명·최대 2줄로 배치되는지 확인한다.
43. 피해 직후 해당 HP 항목의 Bar 길이가 현재/최대 HP 실제 비율만큼 줄고, HP 0에서 완전히 비며 이름에 `[전투불능]`이 표시되는지 확인한다.
44. `행동 중`, `방어`, `도발 2/1`, 대상 화살표가 Sprite 위 상태 Anchor 안에 표시되고 상단 HP HUD에 가려지거나 캐릭터를 과도하게 덮지 않는지 확인한다.
45. 캐릭터 Hover와 대상 선택 중 키보드 포커스 모두 상세 팝업을 열고 이름·직업/몬스터·HP 숫자·방어·도발·쿨타임 중 현재 값만 표시하는지 확인한다.
46. 상세 팝업이 상단 HP HUD나 하단 명령 패널을 가리지 않고, 포커스된 캐릭터의 HP 항목도 밝은 배경·금색 테두리로 함께 강조되는지 확인한다.
47. 하단 명령 패널의 크기·위치와 타임라인, 3대3 조작, 피해·방어·도발·Projectile·승패·도망·필드 복귀가 기존과 같은지 확인한다.
48. Console에 Compile Error, NullReferenceException, MissingReferenceException이 없는지 확인한다.

기존 Male/Female, Path Visual과 Wheelchair Variant, 이름표, 월드 경계·전환·초원 슬라임 필드 Animation도 회귀가 없는지 함께 확인한다.

## 다음 권장 작업

Unity 16:9 Play Mode에서 상단 HP HUD의 3열 배치, 전장·상태 Anchor 분리, 팝업 안전 영역과 실제 HP 비율을 우선 검증한다. 이어 적 4~6명 배치와 3대3 전투 회귀를 확인한 뒤 미엘의 치유의 빛을 공용 아군 대상 선택 흐름에 연결한다.

## 갱신 규칙

기능 작업 완료 시 완료 기능, 검증 상태, Unity 직접 확인 사항, 다음 권장 작업과 마지막 관련 commit hash를 갱신한다. 긴 작업 로그는 기록하지 않는다.
