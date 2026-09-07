# 로컬 저장 시스템

## 목적

게임을 종료했다 다시 실행해도 기존 캐릭터로 테스트를 이어갈 수 있게 합니다.

## 상태

🟢 1차 5개 로컬 캐릭터 슬롯 구현

## 마지막 수정

2026-09-01

## 설명

- Unity Editor 저장 폴더는 프로젝트 기준 `Unity/Client/UserData/Saves/`이며 `Application.dataPath`의 부모에서 계산합니다. 특정 PC의 드라이브 경로를 하드코딩하지 않습니다.
- 파일은 `save_slot_01.json`부터 `save_slot_05.json`까지의 UTF-8 JSON입니다. 슬롯 수는 `GameSaveService.DefaultSaveSlotCount` 한 곳에서 관리합니다.
- Standalone 배포 저장 위치는 아직 최종 확정하지 않았습니다. 현재 빌드 fallback은 `Application.persistentDataPath/Saves/`입니다.
- `GameSessionData`는 현재 실행 중 Scene 사이의 메모리이고, `GameSaveData`는 게임을 종료해도 남는 디스크 값입니다.
- 저장은 Session → SaveData → JSON, 불러오기는 JSON → SaveData 검증 → Session 복원 순서입니다.
- Unity Object와 Asset 경로를 저장하지 않고 이름, Player Visual ID, Path ID, Job ID, Scene ID, SpawnPoint ID와 숫자만 기록합니다. ID를 복원하면 기존 Resources Resolver가 실제 ScriptableObject와 Sprite를 찾습니다.

## GameSaveData Version 1

- `Version`, `PlayerName`, `PlayerVisualId`, `PathId`, `JobId`
- `Level` 기본 1, `CurrentExperience` 기본 0
- `CurrentSceneId`, `SpawnPointId`
- `HasSavedWorldPosition`, `SavedPositionX`, `SavedPositionY`

좌표는 `Vector2`나 `Transform` 참조가 아니라 float 두 개로 저장합니다. 기존 Version 1 JSON에는 좌표 필드가 없으므로 `HasSavedWorldPosition=false` 기본값으로 읽고 기존 SpawnPoint를 사용합니다. 이 호환 가능한 필드 추가는 Version을 올리지 않으며, 향후 형식 자체가 바뀌면 Version 마이그레이션을 사용합니다.

## 슬롯 선택과 자동 저장

- Bootstrap은 5개 슬롯을 목록으로 표시합니다. 유효한 슬롯에는 캐릭터 이름·직업·레벨·마지막 지역과 `이어하기`, 빈 슬롯에는 `새 캐릭터`를 표시합니다.
- 빈 슬롯을 고르면 현재 슬롯 번호를 먼저 기억한 뒤 기존 CharacterCreation → PathSelection → JobSelection → FinalConfirmation → World 흐름을 유지합니다. 이미 저장된 슬롯을 새 게임으로 덮어쓰는 UI는 제공하지 않습니다.
- 이어하기는 선택한 슬롯만 Session으로 복원하며, 이후 자동 저장도 현재 선택 슬롯 파일만 갱신합니다.
- FinalConfirmation 확정 직전, Field/마을 SceneTransition 성공 직후, Battle 종료 뒤 Field Player 복구 완료 후 저장합니다.
- WorldBounds가 있는 마을/Field에서 약 5초마다 실제 위치를 저장하고 Application Pause/Quit 때 가능한 범위에서 한 번 더 저장합니다. 매 프레임 JSON을 쓰지 않습니다.
- Battle 도중 적 HP·턴·상태이상은 저장하지 않습니다.
- 승리 EXP 및 레벨업은 `GameSessionData.ConfigureProgress`와 `GameSaveService.SaveCurrentSession` 공용 API를 사용합니다. 현재 슬롯에 Level과 현재 레벨 진행 EXP를 즉시 저장하며 마지막 안전 월드 Scene/좌표는 유지합니다. Battle 중간 상태 저장은 추가하지 않습니다.

`CurrentExperience`는 평생 누적 경험치가 아니라 현재 레벨 진행치입니다. 비용을 뺀 초과 EXP는 이월하며 연속 레벨업을 허용하고 Lv50에서 진행치는 0입니다. 기존 Version 1 필드와 5슬롯 경로는 그대로입니다. 성장 상세는 `문서/08_몬스터/초반_성장과_공용_독.md`를 따릅니다.

## 오류와 개발 테스트

파일 없음은 빈 슬롯으로 처리합니다. JSON 손상, Version 불일치, 잘못된 Visual/Path/Job/Scene ID는 해당 슬롯만 사용 불가로 표시하고 다른 슬롯은 유지합니다. 문제 파일은 자동 삭제·덮어쓰기하지 않습니다.

Editor의 `Project Limitless/Test/Manage Local Saves` 창에서 슬롯별 삭제와 전체 슬롯 삭제를 할 수 있습니다. 저장 폴더와 JSON은 `.gitignore` 대상입니다.

Bootstrap의 유효 슬롯에는 `이어하기`와 `삭제` 버튼이 함께 표시됩니다. 삭제는 캐릭터 이름·직업·레벨과 복구 불가 안내를 담은 확인창을 거쳐 해당 슬롯 JSON 하나만 지우며, 성공 즉시 빈 슬롯 행으로 갱신합니다.

## 실제 월드 위치와 SpawnPoint fallback

이어하기는 저장 Scene과 좌표가 현재 `WorldBounds2D` 안에 있을 때 실제 좌표를 우선 사용합니다. 좌표가 없거나 NaN/Infinity이거나 Bounds 밖이면 기존 `PendingSpawnPointId`를 지우지 않아 SpawnPoint가 안전한 위치에 배치합니다. SceneTransition을 시작할 때 이전 Scene 좌표를 무효화하고, 목적지 Spawn 배치가 끝난 뒤 그 새 좌표를 저장하므로 Field 간 좌표가 섞이지 않습니다.

Battle Scene에는 월드 위치 저장기를 두지 않습니다. 전투 도중 종료하면 마지막 안전 월드 위치를 유지하고, 승리·패배·도망으로 Field Player 복구가 끝난 뒤 새 위치를 저장합니다.

## 기존 단일 저장 마이그레이션

Bootstrap 진입 시 옛 `Application.persistentDataPath/project_limitless_save.json`이 유효하고 새 슬롯 1 파일이 없을 때만 슬롯 1로 복사합니다. 구 파일은 삭제하지 않으며 슬롯 1을 덮어쓰지 않습니다. 구 파일이 손상되었으면 한국어 경고만 남기고 다른 슬롯을 정상 표시합니다.
