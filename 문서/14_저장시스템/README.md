# 로컬 저장 시스템

## 목적

게임을 종료했다 다시 실행해도 기존 캐릭터로 테스트를 이어갈 수 있게 합니다.

## 상태

🟢 1차 단일 로컬 슬롯 구현

## 마지막 수정

2026-09-01

## 설명

- 저장 파일은 `Application.persistentDataPath/project_limitless_save.json` UTF-8 JSON입니다.
- `GameSessionData`는 현재 실행 중 Scene 사이의 메모리이고, `GameSaveData`는 게임을 종료해도 남는 디스크 값입니다.
- 저장은 Session → SaveData → JSON, 불러오기는 JSON → SaveData 검증 → Session 복원 순서입니다.
- Unity Object와 Asset 경로를 저장하지 않고 이름, Player Visual ID, Path ID, Job ID, Scene ID, SpawnPoint ID와 숫자만 기록합니다. ID를 복원하면 기존 Resources Resolver가 실제 ScriptableObject와 Sprite를 찾습니다.

## GameSaveData Version 1

- `Version`, `PlayerName`, `PlayerVisualId`, `PathId`, `JobId`
- `Level` 기본 1, `CurrentExperience` 기본 0
- `CurrentSceneId`, `SpawnPointId`

Version은 향후 저장 구조 변경과 명시적 마이그레이션을 구분하기 위해 필요합니다. 지원하지 않는 Version은 자동 변환·덮어쓰기하지 않습니다.

## 시작과 자동 저장

- Bootstrap은 유효한 저장이 없으면 `새 게임`, 있으면 `이어하기`와 `새 게임`을 표시합니다.
- 새 게임은 기존 CharacterCreation → PathSelection → JobSelection → FinalConfirmation → World 흐름을 유지합니다.
- FinalConfirmation 확정 직전, Field/마을 SceneTransition 성공 직후, Battle 종료 뒤 Field Player 복구 완료 후 저장합니다.
- Battle 도중 적 HP·턴·상태이상은 저장하지 않습니다.
- 향후 레벨업은 `GameSessionData.ConfigureProgress`와 `GameSaveService.SaveCurrentSession` 공용 API를 사용합니다.

## 오류와 개발 테스트

파일 없음, JSON 손상, Version 불일치, 잘못된 Visual/Path/Job/Scene ID는 한국어 로그를 남기고 새 게임 UI를 유지합니다. 문제 파일은 자동 삭제·덮어쓰기하지 않습니다. Editor에서는 `Project Limitless/Test/Delete Local Save` 메뉴로 테스트 저장만 삭제할 수 있습니다.
