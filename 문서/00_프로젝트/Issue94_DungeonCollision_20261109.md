# Issue #94 — Dungeon01 B1·B2 내부 벽 충돌

상태: **LOCAL_MAIN_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED / USER_REALPLAY_PENDING**.
제품 적용 로컬 main SHA: `5024d2b6ffd1a07f90dbb15cf073eed16bb3630a`. 브랜치: `codex/fix-20261109-dungeon-b1-b2-collision`.

## 확인한 원인

- 실제 원본 Unity6000.6.5f1의 Dungeon_01_B2 플레이어: Dynamic Rigidbody2D, 비트리거 CircleCollider2D 반경0.5, Default 레이어 간 충돌 허용. 원본 Scene dirty=false·Play=true를 읽기만 했습니다.
- B1·B2 기존 환경은 방/복도 바닥의 모든 가장자리에 벽이 있는 구조가 아닙니다. WorldBounds는 외곽 네 면, 기존 벽은 일부 구간만 차단합니다. 리뉴얼 Renderer는 표시만 교체하므로 누락된 내부 허공이 그대로 통과 가능했습니다.
- 원격 main보다 최신인 `docs/planning`의 PLANNING_LATEST 2026-11-09 및 Issue94를 기준으로 했습니다. 시작 로컬 main은 b0ea195fbaa8148146c3fe5d7b5f609385d21a4e입니다.

## 구현

- DungeonEnvironmentDefinition.Placement에 walkableFloor를 추가하고 기존 승인 프로필의 B1 방/복도9개·B2 방/복도7개만 표시했습니다. StoneBase는 배경이므로 보행 바닥에서 제외합니다.
- DungeonFloorCollision이 sceneLoaded에서 해당 프로필 바닥 합집합의 여집합을 분할한 BoxCollider2D로 구성합니다. Static Rigidbody2D와 Polygon 방식 CompositeCollider2D로 합쳐 접합선을 제거합니다. 전체 사각형 덮기·Player Clamp·매 프레임 텔레포트가 아닙니다.
- B1 북쪽 바닥의 기존0.05 간격은 캐릭터가 지나가던 줄눈이므로0.06 이내 인접 끝점만 한 선으로 맞춥니다. PNG·Scene·기존 벽·소품·봉인문·계단 좌표는 변경하지 않습니다.
- WorldPositionSaveController는 던전 충돌 설치 후 Start에 저장 위치를 검증합니다. 실제 접지 반경으로 정상 위치/벽 접촉은 유지하고 벽 안 좌표만 가까운 검증된 바닥으로 복원합니다. 보스 미처치 시 장벽 뒤로 복구하지 않습니다. 복구된 Scene에서 주기/종료 위치 저장을 생략하여 복구 자체로 원본 Save를 덮지 않습니다. 기존 명시적 출구/진행 저장 흐름은 유지합니다.

## 검증 및 한계

최종 Unity6000.6.5f1 숨김 Play 검증 **110PASS / 0FAIL**. 중간 실패 실행과 이전 #93 QA는 합산하지 않습니다.

- B1·B2 모두 같은 키보드 서쪽 이동으로 수정 전 허공 진입을 재현했습니다. 원본 벽/입력은 유지하고 새 충돌 루트만 비활성화한 대조군입니다.
- 도보·휠체어 선택 외형의 실제 렌더 프레임을 대조했고, 공통 접지 CircleCollider 반경0.5를 확인했습니다. 가상 키보드·게임패드 및 공용 안내 이동 API에서 차단/대각 이동이 통과했습니다.
- 전체 바닥8방향 실제 Rigidbody 스트레스 B1 72/B2 56 표본에서 허공 진입·관통0. 각 층13200점 Physics2D 마스크 불일치0. 실제 Scene의 기존 외곽/벽/소품과 새 Composite를 함께 검사했습니다.
- B1 동쪽 잔해와 북쪽 벽 사이0.95 틈은 접지 지름1.0보다 좁습니다. 실제 물리 접촉 깊이0.0383과 후진 탈출을 관찰한 뒤, 기존 통과 불가 틈1.5×0.95만 프로필 blockedGaps로 닫았습니다. 최종 접촉 깊이0.0050(물리 접촉 오차), 후진 탈출 및 스트레스 모두 통과. 그림·정상 통로는 불변입니다.
- Main11 B1 중앙/서쪽/동쪽/하강로, B2 입구/안치 공간/서쪽/동쪽/중앙/보스/봉인실/석판 접근. 실제 하강로 상호작용 B1→B2, 마우스 확인 버튼 B2→B1→Field03, Spawn_From_B2(0,6.8) 보존 통과. 살아 있는 몬스터는 실제 Collider를 피해서 우회했습니다.
- 보스 전 실제 장벽 차단, 보스 처치 fixture에서 장벽 개방 및 실제 몸체의 봉인실 내부 진입(1.47,11.00), 석판 상호작용 거리 확보. 보스 전투 전체는 재실행하지 않았습니다.
- 벽 안 이어하기 B1(-4,-6)→(-3.99,-4.79), B2(-4,-6)→(-3.79,-7.19). 정상(0,-8.5) 불변. 실제 Continue 복원과 복구 후 SaveNow의 복제 슬롯 바이트 불변 통과. 동적 부착 컴포넌트 Start보다 Spawn 코루틴이 먼저 동작할 수 있어, 유효 저장 좌표 확인 즉시 PendingSpawn을 해제하도록 수정했습니다.
- 프로젝트 Runtime 오류0. 기존 UnityEditor.Search.SearchDatabase 내부 ArgumentOutOfRangeException1은 별도 기록하며 Console 전체0이라고 주장하지 않습니다.
- 초기 QA의 단순 겹침 판정·몬스터 무시 경로·Rect 직렬화 버전 누락 및 이전 Scene 자동 저장의 fixture 간섭은 수정 후 최종 실행으로 확인했습니다. 실패 기록은 Temp/Issue94에 별도 보존했습니다.

- 원본 Editor는 플레이 중이므로 Scene·선택·Play·GameView·포커스를 변경하지 않았습니다. 별도 작업브랜치 worktree의 숨김 batch Unity를 사용합니다.
- 원본 Packages는 보존했습니다. 검증 전용 manifest에서 설치된 같은 버전 패키지의 로컬 캐시를 참조합니다. CLI Pipeline은 현재 프로젝트에 연결되지 않아 MCP 읽기 조회와 Unity batch 실행을 사용했습니다.
- 최초 네트워크 Git/registry resolve 지연 및 `-noUpm` 시 패키지 미등록으로 발생한 컴파일 실패는 검증 환경 문제입니다. 이 실행을 제품 PASS로 집계하지 않습니다.
- B1·B2 현행 QuestNavigationPresenter는 방향·거리 안내이며 자동 마우스 길찾기는 연결되어 있지 않습니다. 자동 길찾기는 Main22/23 전용입니다. 공용 SetGuidedMovement의 실제 물리 이동과 기존 마우스 귀환 버튼 검증을 별도로 구분합니다. 이번 수정에서 새 길찾기 기획/시스템은 추가하지 않습니다.
- 실제 물리 장치 손 플레이와 사용자 최종 수용은 자동 물리/가상 입력 검증과 별개입니다.

## 근거

- 실행기: Tools/QA/Issue94CollisionAudit.cs.txt (격리 Editor에만 .cs로 복사)
- 비공개 원본 보호·Save 복제: Temp/Issue94/Private/
- 실제 Camera 렌더/Collider 윤곽: Temp/Issue94/Evidence/
- Unity6000.6 API 확인: [MovePosition](https://docs.unity.com/en-us/engine/6000.6/script-reference/unityengine/rigidbody2d/moveposition), [CompositeOperation](https://docs.unity.com/en-us/engine/6000.6/script-reference/unityengine/collider2d/compositeoperation).

## 원본 보호·실제 적용

- 시작 tracked dirty302, untracked 디렉터리 표기36개(내부 파일 포함 별도 보호). 보호995개 중994개 SHA불변, 이미지/음성1369개 불변, 원본 git status 동일. 슬롯01은 실행 중인 원본 게임이 SavedPositionX/Y만 자체 갱신했습니다. Codex가 원본 슬롯을 쓰거나 과거 백업으로 덮은 횟수0. 원본 슬롯02/03은 시작부터 없었고 기존 #93 복제본만 fixture로 사용했습니다.
- 변경7경로와 사용자 미커밋/미추적 경로 겹침0, ff-only 사전 게이트 통과. 로컬 main을 b0ea195f→**5024d2b6ffd1a07f90dbb15cf073eed16bb3630a**로 실제 fast-forward했습니다. 기존 #93 숲거미/TTS/Field03/귀환 코드는 변경하지 않았습니다.
- 제품 파일: DungeonEnvironmentDefinition.cs, DungeonFloorCollision.cs/meta(GUID7b8e12470ec84af9a14cab95e2ed19a2), WorldPositionSaveController.cs, Dungeon01B1Renewal.asset, Dungeon01B2Renewal.asset. 재현 실행기는 Tools/QA/Issue94CollisionAudit.cs.txt입니다. Scene·Prefab·PNG·WAV·기존meta/GUID·Packages·BuildSettings를 제품 커밋에 포함하지 않았습니다.
- 원본 Unity는 B2 Play 중이므로 강제 refresh/종료/Scene/선택/포커스 조작0. **Unity 새로고침 후 새 Play에서 새 충돌이 설치됩니다.** 현재 실행 중인 이전 인스턴스를 수정 완료로 오인하지 않습니다.
- 문서 포함 최종 local main SHA는 자기 참조 commit을 추정하지 않고 `Temp/Issue94/Evidence/local-main-final.json`과 실제 Issue94 댓글에 기록합니다. 원격 Push/병합0, main 직접 커밋0.

## 실제 렌더 캡처

Camera.Render→RenderTexture에서 출력한 실제 게임 렌더입니다. OS GameView를 활성화하지 않았으며 Collider 디버그 선은 실제 Collider 좌표를 LineRenderer로 출력했습니다. 오프라인 합성/AI 그림이 아닙니다.

- [B1 수정 전](../../Temp/Issue94/Evidence/Dungeon_01-before-game.png) / [B1 도보 벽 접촉](../../Temp/Issue94/Evidence/Dungeon_01-path.hearing-wall-contact.png) / [B1 휠체어 벽 접촉](../../Temp/Issue94/Evidence/Dungeon_01-path.mobility-wall-contact.png)
- [B2 수정 전](../../Temp/Issue94/Evidence/Dungeon_01_B2-before-game.png) / [B2 도보 벽 접촉](../../Temp/Issue94/Evidence/Dungeon_01_B2-path.hearing-wall-contact.png) / [B2 휠체어 벽 접촉](../../Temp/Issue94/Evidence/Dungeon_01_B2-path.mobility-wall-contact.png)
- [B1 전체 Collider](../../Temp/Issue94/Evidence/Dungeon_01-path.mobility-after-colliders.png) / [B2 전체 Collider](../../Temp/Issue94/Evidence/Dungeon_01_B2-path.mobility-after-colliders.png) / [봉인실 진입](../../Temp/Issue94/Evidence/B2-chamber-final.png)
- 공개 가능한 텍스트 근거: Evidence/Issue94Collision20261109/checks-final.txt, geometry.txt, capture-manifest.json, protection-summary.json. PNG 실물은 로컬에 보존했으며 원격 업로드하지 않았습니다.

다음 권장: 바울 대표가 새 Play에서 B1·B2 벽 접근과 휠체어 모서리·조사·귀환을 짧게 수용 확인. 기존 던전 자동 마우스 길찾기 미구현은 별도 기획/작업이며 이번 충돌 수정으로 추가하지 않았습니다.

- 동기화 후 원본 Editor 읽기 조회: Unity6000.6.5f1, Play=false, 기존 Bootstrap Scene dirty=false, compiling=false, C# Console 오류0. 새 DungeonFloorCollision 타입은 아직 미로드이며 external_changes_dirty=true입니다. Codex가 원본 Play/Scene/포커스를 제어하거나 refresh하지 않았습니다. Unity로 돌아가 자연스러운 import/새로고침 후 새 Play에서 확인해야 합니다. 격리 검증 컴파일 완료와 원본 Editor 아직 미import 상태를 구분합니다.
