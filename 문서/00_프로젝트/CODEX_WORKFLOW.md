# Codex 작업 절차

## 작업 시작

항상 다음 순서로 확인한다.

1. Repository Root의 `AGENTS.md`
2. `문서/00_프로젝트/PROJECT_CONTEXT.md`
3. `문서/00_프로젝트/CURRENT_STATUS.md`
4. 현재 작업과 관련된 `문서/` 하위 세부 문서
5. `git status`

사용자가 `첫 몬스터 구현`, `전투 화면 만들기`, `치유사 스킬 구현`, `Field_02 만들기`처럼 짧게 요청해도 위 자료와 관련 코드를 먼저 조사한다. 저장소에 이미 있는 내용을 사용자에게 다시 요구하지 않는다. 문서와 코드에도 결정되지 않은 게임 디자인 선택만 질문한다.

## 코드 작성

- 기존 구조를 우선 재사용한다.
- 새 길, 직업, Field를 추가할 수 있는 데이터 기반 구조를 선호한다.
- 기존 정상 기능을 보호한다.
- 작업 범위를 넘어서는 대규모 리팩터링을 하지 않는다.
- 사용자 수동 수정 Sprite와 PNG를 임의로 수정하거나 재생성하지 않는다.
- ThirdParty 원본 Asset을 직접 수정하지 않는다.

## C# 주석

- 모든 새 C# 파일에 프로그래밍 비전공자도 이해할 수 있는 한국어 주석을 작성한다.
- 클래스가 무엇을 하고 게임의 어디에서 사용되는지 설명한다.
- 중요한 메서드에는 무엇을 하는지, 언제 호출되는지, 왜 필요한지 설명한다.
- 계산이나 Unity 생명주기처럼 바로 이해하기 어려운 부분을 중심으로 설명한다.
- 코드 한 줄을 그대로 번역하는 무의미한 주석은 피한다.

## Unity 작업

- Scene, Prefab, Generator를 변경하기 전에 현재 구조와 사용자 변경을 확인한다.
- Generator가 Scene을 덮어쓸 가능성이 있으면 사용자 확인 없이 실행하지 않는다.
- 사용자가 자동 생성 또는 검증 실행을 명시적으로 요청한 경우에는 요청 범위 안에서 수행할 수 있다.
- 모든 World/Field에 World Bounds, Camera Bounds, 네 면 Collider와 명시적인 Exit Opening을 적용한다.
- Exit의 `SceneTransitionTrigger`와 목적지 `SceneSpawnPoint`는 겹치지 않게 한다.

## 검증

가능한 범위에서 다음을 확인한다.

- Unity Compile
- Missing Script와 Missing Reference
- NullReference와 Console Error
- 기능별 Edit/Play Mode 또는 수동 검증 항목
- `git diff --check`

Unity Editor나 Play Mode를 실제로 실행하지 못했으면 검증 완료라고 표현하지 않는다. 정적 검증 결과와 사용자가 Unity에서 직접 확인해야 할 사항을 구분해 보고한다.

## Git

- 관련 변경만 스테이징하고 의미 있는 단위로 commit한다.
- 기존 사용자 Working Tree 변경을 포함하거나 되돌리지 않는다.
- GitHub Push는 사용자가 직접 수행하는 것을 기본으로 한다.
- 사용자가 명시적으로 요청하지 않으면 `git push`를 실행하지 않는다.

## 기능 작업 종료

기능 구현이 끝나면 `CURRENT_STATUS.md`에 다음 정보를 간결하게 반영한다.

- 완료 기능
- 현재 검증 상태
- Unity에서 직접 확인할 사항
- 다음 권장 작업
- 마지막 관련 commit hash

## 기본 종료 보고

주요 작업은 다음 순서를 기본으로 보고한다.

1. 작업 결과
2. 생성 파일
3. 수정 파일
4. 기존 기능 영향
5. Unity/정적 검증 결과
6. 사용자가 직접 확인할 사항
7. `CURRENT_STATUS.md` 갱신 내용
8. commit hash
9. GitHub Push 여부

장황한 내부 작업 과정은 생략하고 결과와 남은 확인 사항을 분명히 구분한다.
