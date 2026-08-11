# Milestone 01 플레이어블 프로토타입

## 목표

Unity 실행 시 첫 번째 마을에서 플레이어를 자유롭게 이동시키고, NPC 한 명과 상호작용할 수 있는 최소 프로토타입을 준비한다.

## 구현된 기능

- Bootstrap Scene에서 Character Creation을 거쳐 첫 번째 마을 Scene을 단일 로드
- Input System 기반 키보드(WASD/방향키) 및 게임패드 왼쪽 스틱 8방향 이동
- Rigidbody2D 기반 이동과 대각선 입력 정규화
- Inspector에서 변경 가능한 플레이어 이동 속도
- 게임플레이 Component와 분리된 128×128 Male/Female Player Visual 선택
- Character Creation 화면에서 선택한 외형과 이름을 런타임 세션에 유지하고 외형을 Starter Village Player에 적용
- Starter Village의 플레이어 머리 위에 세션의 캐릭터 이름을 표시
- 부드러운 2D Orthographic 카메라 추적
- 마을 외곽, 건물, 우물, 상자의 Collider2D
- 고정 NPC 한 명과 시간 제한 없는 대화 상호작용
- 플레이어와 가장 가까운 NPC를 `interactionRadius` 안에서 현재 상호작용 대상으로 선택
- 대상 NPC 위에 UI Text로 `[E] 대화하기`를 표시하고, 범위를 벗어나면 숨김
- E/F 또는 게임패드 A로 상호작용, Esc 또는 게임패드 B로 대화 닫기

## Scene 구조

Unity Editor 메뉴 생성 후 다음 Scene을 사용한다.

- `Assets/_Project/Scenes/Bootstrap.unity`: `BootstrapLoader`로 `CharacterCreation`을 로드한다.
- `Assets/_Project/Scenes/CharacterCreation.unity`: Male/Female 외형을 선택하고 게임 시작 버튼으로 월드에 입장한다.
- `Assets/_Project/Scenes/World_StarterVillage.unity`: 마을 바닥, 경계, 건물, 장애물, 플레이어, NPC, 카메라, 대화 UI를 포함한다.

생성 도구는 플레이어와 NPC의 `Collider2D`, 관련 Controller를 각각 `Assets/_Project/Prefabs/PlayerPlaceholder.prefab`, `Assets/_Project/Prefabs/VillageNpcPlaceholder.prefab`으로 만든 뒤 월드 Scene에 배치한다. 플레이어의 `SpriteRenderer`와 `Animator`는 게임플레이 Component와 분리된 `Visual` 자식에 두고, 이름표는 외형에 관계없이 따라오도록 Player 부모의 공통 `PlayerNameplate`가 만든다.

`Project-Limitless/Milestone 01/Generate Scenes`는 기존 Milestone 01 Scene/Prefab이 있으면 `Assets/_Project/Backup/Milestone01/<timestamp>/`에 먼저 복사해 확인한 뒤 새로 생성한다. 저장되지 않은 해당 Scene이 열려 있으면 작업을 중단하고 저장을 요청한다.

## Script 구조

- `Scripts/Core/BootstrapLoader.cs`: 시작 Scene에서 월드를 로드한다.
- `Scripts/Core/GameSessionData.cs`: Character Creation의 외형과 이름 선택을 Scene 사이에 유지한다.
- `Scripts/Core/PlaceholderVisual.cs`: 실제 Sprite가 없을 때 SpriteRenderer에 단색 placeholder를 제공한다.
- `Scripts/Player/PlayerController.cs`: Input System과 Rigidbody2D 이동을 담당한다.
- `Scripts/Player/PlayerVisualController.cs`: 이동 로직과 독립적으로 Male/Female Sprite와 Animator를 선택한다.
- `Scripts/Player/PlayerNameplate.cs`: 세션의 플레이어 이름을 전용 Screen Space Overlay 이름표로 표시한다.
- `Scripts/Player/PlayerSpriteAnimator.cs`: 선택된 Visual에서 공통 이동 방향에 맞는 애니메이션을 재생한다.
- `Scripts/Camera/CameraFollow.cs`: 카메라 추적을 담당한다.
- `Scripts/NPC/NpcController.cs`: NPC 이름과 대사를 보관한다.
- `Scripts/NPC/InteractionSystem.cs`: 가장 가까운 현재 상호작용 대상과의 상호작용을 담당한다.
- `Scripts/NPC/NpcInteractionPrompt.cs`: NPC 주변에 `[E] 대화하기` UI Text를 표시한다.
- `Scripts/UI/DialoguePresenter.cs`: 최소 대화 UI를 표시한다.
- `Scripts/UI/CharacterCreationController.cs`: 접근 가능한 외형·이름 입력 UI, 이름 검증, 월드 입장을 담당한다.
- `Scripts/Editor/StarterVillageSceneGenerator.cs`: 실제 Unity Scene과 Build Settings를 생성하는 Editor 메뉴 도구다.

## 환경 그래픽

Starter Village 환경은 `Assets/ThirdParty/Kenney/RPGBase/PNG/`의 Kenney RPG Base 원본 Sprite를 참조한다. 원본 PNG는 수정하지 않는다.

- 잔디 바닥: `rpgTile003.png`
- 흙길: `rpgTile008.png`
- 집 구성: `rpgTile101.png`, `rpgTile103.png`, `rpgTile105.png`, `rpgTile120.png`, `rpgTile122.png`, `rpgTile124.png`
- 우물: `rpgTile184.png`
- 상자: `rpgTile163.png`
- 나무: `rpgTile195.png`, `rpgTile197.png`, `rpgTile200.png`
- 울타리: `rpgTile181.png`, `rpgTile182.png`, `rpgTile215.png`, `rpgTile216.png`

Kenney PNG는 64×64 픽셀이므로 Sprite, Pixels Per Unit 64, Point Filter, 압축 없음, Mipmap 비활성화로 가져온다. 1 타일을 월드 1 단위로 유지해 기존 플레이어 크기 및 2D 물리 스케일과 일관되게 맞춘다.

독립적인 돌 타일은 이 패키지의 실제 PNG 목록에서 확인하지 못해 배치하지 않았다.

## Player Visual 구조

플레이어 부모에는 `Rigidbody2D`, `CircleCollider2D`, `PlayerController`, `InteractionSystem`, `PlayerVisualController`, `PlayerNameplate`를 둔다. `Visual` 자식에는 `SpriteRenderer`, `Animator`, `PlayerSpriteAnimator`를 둔다. 외형을 바꾸어도 이동 속도, 충돌 크기, NPC 상호작용, 카메라 추적 대상과 이름표는 바뀌지 않는다.

`PlayerNameplate`는 실행 시 Scene 루트에 전용 `PlayerNameOverlayCanvas`를 만들고 Screen Space Overlay 방식의 uGUI `Text`에 `GameSessionData.PlayerName`을 표시한다. 이름 앞뒤 공백을 제거한 결과가 비어 있을 때만 `플레이어`를 사용한다. `LateUpdate`에서 Player 위치에 월드 Y 1.15를 더한 뒤 `Camera.main.WorldToScreenPoint`로 화면 좌표를 계산한다. 따라서 Player와 Camera가 이동해도 머리 위를 따라가며 Camera 확대·축소와 무관하게 24px 글자 크기를 유지한다. NameText 크기는 240×36이고 배경 없이 흰 글자와 1px 검은 Outline을 사용한다. 글꼴은 Unity 6의 `LegacyRuntime.ttf`이다.

`PlayerVisualController`의 `Visual Type`을 Inspector에서 `Male` 또는 `Female`로 선택할 수 있으며 기본값은 `Male`이다. 각 외형은 동일한 `Speed`, `MoveX`, `MoveY` Parameter와 `Idle/Walk` 4방향 State 구조를 가진 전용 Animator Controller를 사용한다. 캐릭터 생성 화면과 선택 저장은 이후 시스템에서 `SetVisual`을 호출하는 방식으로 연결할 수 있다.

NPC는 실제 Sprite가 준비되기 전까지 `PlaceholderVisual`의 단색 SpriteRenderer와 글자 레이블을 유지한다. 텍스트 레이블을 함께 표시하여 색상만으로 대상을 구분하지 않는다.

NPC 상호작용 가능 여부는 색상이나 NPC의 정면 방향에 의존하지 않는다. 플레이어가 어느 방향에서든 `interactionRadius` 안에 들어오면 `[E] 대화하기` UI Text가 표시된다. 여러 NPC가 범위에 있으면 가장 가까운 NPC만 현재 대상으로 표시한다.

NPC 안내와 대화 UI는 Unity 6의 `LegacyRuntime.ttf`를 기본 폰트로 사용한다. `NpcInteractionPrompt`와 `DialoguePresenter`의 Font 필드는 Inspector에서 프로젝트 전용 폰트로 교체할 수 있다.

## 테스트 결과

- 코드 구조 및 Unity 패키지 의존성(Input System, uGUI)을 정적 검토했다.
- 현재 작업 환경에서는 Unity Editor 실행 파일을 확인하지 못해 컴파일, Scene 생성, Play Mode 테스트는 실행하지 못했다.
- `Project-Limitless/Milestone 01/Generate Scenes` 메뉴 실행 후 Unity Editor에서 실제 검증이 필요하다.

## 다음 단계

- 실제 Sprite 및 애니메이션 에셋 교체
- 마을의 구체적 기획 확정 후 Tilemap/환경 에셋 적용
- 대화 데이터와 UI 접근성 옵션 확장
- 퀘스트·필드·전투 시스템은 별도 Milestone에서 설계와 구현
