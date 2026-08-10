# Milestone 01 플레이어블 프로토타입

## 목표

Unity 실행 시 첫 번째 마을에서 플레이어를 자유롭게 이동시키고, NPC 한 명과 상호작용할 수 있는 최소 프로토타입을 준비한다.

## 구현된 기능

- Bootstrap Scene에서 첫 번째 마을 Scene을 단일 로드
- Input System 기반 키보드(WASD/방향키) 및 게임패드 왼쪽 스틱 8방향 이동
- Rigidbody2D 기반 이동과 대각선 입력 정규화
- Inspector에서 변경 가능한 플레이어 이동 속도
- 부드러운 2D Orthographic 카메라 추적
- 마을 외곽, 건물, 우물, 상자의 Collider2D
- 고정 NPC 한 명과 시간 제한 없는 대화 상호작용
- 플레이어와 가장 가까운 NPC를 `interactionRadius` 안에서 현재 상호작용 대상으로 선택
- 대상 NPC 위에 UI Text로 `[E] 대화하기`를 표시하고, 범위를 벗어나면 숨김
- E/F 또는 게임패드 A로 상호작용, Esc 또는 게임패드 B로 대화 닫기

## Scene 구조

Unity Editor 메뉴 생성 후 다음 Scene을 사용한다.

- `Assets/_Project/Scenes/Bootstrap.unity`: `BootstrapLoader`로 `World_StarterVillage`를 로드한다.
- `Assets/_Project/Scenes/World_StarterVillage.unity`: 마을 바닥, 경계, 건물, 장애물, 플레이어, NPC, 카메라, 대화 UI를 포함한다.

생성 도구는 플레이어와 NPC의 `SpriteRenderer`, `Collider2D`, 관련 Controller를 각각 `Assets/_Project/Prefabs/PlayerPlaceholder.prefab`, `Assets/_Project/Prefabs/VillageNpcPlaceholder.prefab`으로 만든 뒤 월드 Scene에 배치한다.

## Script 구조

- `Scripts/Core/BootstrapLoader.cs`: 시작 Scene에서 월드를 로드한다.
- `Scripts/Core/PlaceholderVisual.cs`: 실제 Sprite가 없을 때 SpriteRenderer에 단색 placeholder를 제공한다.
- `Scripts/Player/PlayerController.cs`: Input System과 Rigidbody2D 이동을 담당한다.
- `Scripts/Camera/CameraFollow.cs`: 카메라 추적을 담당한다.
- `Scripts/NPC/NpcController.cs`: NPC 이름과 대사를 보관한다.
- `Scripts/NPC/InteractionSystem.cs`: 가장 가까운 현재 상호작용 대상과의 상호작용을 담당한다.
- `Scripts/NPC/NpcInteractionPrompt.cs`: NPC 주변에 `[E] 대화하기` UI Text를 표시한다.
- `Scripts/UI/DialoguePresenter.cs`: 최소 대화 UI를 표시한다.
- `Scripts/Editor/StarterVillageSceneGenerator.cs`: 실제 Unity Scene과 Build Settings를 생성하는 Editor 메뉴 도구다.

## Placeholder 사용 현황

바닥, 경계, 건물, 장애물, 플레이어, NPC는 `PlaceholderVisual`의 단색 SpriteRenderer를 사용한다. 실제 Sprite를 준비하면 각 SpriteRenderer의 Sprite를 Inspector에서 지정할 수 있으며, 비어 있을 때만 placeholder가 생성된다. 텍스트 레이블을 함께 표시하여 색상만으로 대상을 구분하지 않는다.

NPC 상호작용 가능 여부는 색상이나 NPC의 정면 방향에 의존하지 않는다. 플레이어가 어느 방향에서든 `interactionRadius` 안에 들어오면 `[E] 대화하기` UI Text가 표시된다. 여러 NPC가 범위에 있으면 가장 가까운 NPC만 현재 대상으로 표시한다.

## 테스트 결과

- 코드 구조 및 Unity 패키지 의존성(Input System, uGUI)을 정적 검토했다.
- 현재 작업 환경에서는 Unity Editor 실행 파일을 확인하지 못해 컴파일, Scene 생성, Play Mode 테스트는 실행하지 못했다.
- `Project-Limitless/Milestone 01/Generate Scenes` 메뉴 실행 후 Unity Editor에서 실제 검증이 필요하다.

## 다음 단계

- 실제 Sprite 및 애니메이션 에셋 교체
- 마을의 구체적 기획 확정 후 Tilemap/환경 에셋 적용
- 대화 데이터와 UI 접근성 옵션 확장
- 퀘스트·필드·전투 시스템은 별도 Milestone에서 설계와 구현
