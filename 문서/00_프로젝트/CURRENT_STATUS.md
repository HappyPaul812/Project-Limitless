# Project-Limitless 현재 개발 상태

## 기준

- 갱신일: 2026-08-19
- 기준 브랜치: `main`
- 마지막 기능 관련 commit: `3cdb893` (`Feature: Field_01 초원 슬라임 배회 및 조우 추가`)
- 마지막 관련 문서 commit: `bdbd352` (`Docs: 월드 전환 및 경계 코드 한국어 주석 보강`)

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
- Field_01 중앙의 `초원 슬라임` 1마리 런타임 배치
- 시작 위치 중심 활동 반경 안의 느린 무작위 배회
- Rigidbody2D 기반 장애물 Collider 충돌
- 플레이어 접촉 시 이동 정지와 `MonsterEncounterService.EncounterStarted` Event 발생
- 적절한 Slime Sprite가 없어 기존 `PlaceholderVisual`로 임시 표시

## 데이터만 있고 실행 로직이 없는 항목

- 길 능력치와 고유 패시브의 전투 효과
- 직업 능력치, 패시브, 시작 스킬 프리뷰
- AP, 상태이상, 협동 기술, 보스 패턴의 초안 방향

이 항목들은 캐릭터 생성 UI에서 표시되지만 실제 전투 계산이나 효과로 실행되지 않는다.

## 미구현

- 조우 Event 이후 실제 전투 진입
- 실제 턴제 전투 루프
- HP·AP와 행동 순서 계산
- 직업 스킬과 길 패시브 실행
- 몬스터 AI
- 전투 UI
- 승리·패배·보상·성장
- 퀘스트
- 영구 저장과 불러오기
- 인스턴스 던전
- Field_02 이후 지역

## 현재 검증 상태

- 월드 전환 Scene/Spawn ID, Missing Script, Bounds 참조와 viewport 계산은 정적으로 확인했다.
- 초원 슬라임 Script GUID, Monster/Spawn Asset 연결, Field_01 대상 Scene과 조우 Event 구조를 정적으로 확인했다.
- 관련 C# 변경은 `git diff --check`를 통과했다.
- 현재 작업 환경의 Unity Hub에 설치된 Editor가 등록되어 있지 않아 Unity Compile과 Play Mode는 실행하지 못했다.
- Working Tree에는 이번 문서 작업과 무관한 사용자 Asset·Scene·ProjectSettings 변경이 남아 있으며 이 상태 문서는 해당 미커밋 변경의 완성 여부를 판단하지 않는다.

## Unity에서 사용자가 직접 확인할 사항

- Male/Female 및 각 Path Visual, Wheelchair Variant의 실제 이동·Animation
- StarterVillage 좌·우·상·남쪽 끝에서 검은 외부 영역이 보이지 않는지
- Field_01 네 방향 끝에서 Camera와 Collider가 정상인지
- 초원 슬라임이 중앙에 나타나 활동 범위 안을 천천히 배회하는지
- 슬라임이 나무·바위·울타리를 통과하지 않는지
- Male/Female 및 Wheelchair Variant Player 접촉 시 슬라임이 멈추고 Console에 `몬스터 조우: 초원 슬라임`이 한 번 표시되는지
- 마을 남문과 Field_01 북쪽 입구의 양방향 전환
- Spawn 직후 역전환이 발생하지 않는지
- Console Error, Missing Reference, NullReference가 없는지

## 다음 권장 작업

`MonsterEncounterService.EncounterStarted`를 받을 최소 턴제 전투 진입 흐름과 전투 데이터 경계를 설계한다. Scene 전환 여부, 조우 후 필드 복귀, 반복 조우와 몬스터 제거 규칙은 구현 전에 사용자에게 선택지를 확인한다.

## 갱신 규칙

기능 작업 완료 시 완료 기능, 검증 상태, Unity 직접 확인 사항, 다음 권장 작업과 마지막 관련 commit hash를 갱신한다. 긴 작업 로그는 기록하지 않는다.
