# Issue97 격리 Delta QA

Unity 6000.6.5f1 전용입니다. `SnakeProbe.cs`는 제품 Assets에 포함하지 않습니다.
복제 QA 프로젝트 `Temp/Issue97/QA/Client/Assets/SnakeProbe.cs`에 넣고 숨김 batchmode `-executeMethod SnakeBatch.Run`으로 실행합니다.
원본 Editor의 Scene/Play/포커스를 조작하지 않습니다. Save와 설정은 QA 전용 SyntheticSaves/SyntheticSettings로 우회합니다.
실제 Field_02와 Battle Scene, MonsterSpriteSheetAnimation, BattleSceneController의 기본 공격을 실행하며 GameView.RenderView에서 캡처합니다.
GameView 크기 변경 후 warm-up을 거치며 D3D 저장 방향만 정규화합니다. Camera.Render나 오프라인 합성은 사용하지 않습니다.

최종 시트는 640px 셀 4열(2560×640), 각 셀 좌상단 기준 (64,128)에 승인 512px 원본을 무손실로 배치했습니다. 리사이즈·재드로잉은 없습니다.
소스 순서는 Idle00/01/02/03 및 Attack00/01_v2/02/03_v2입니다. source/derived/ZIP SHA는 docs/art-renewal/issue97-anim8-20261110/ JSON을 참조합니다.
Runtime Sprite.Create는 중심 Pivot, PPU=cellWidth를 사용합니다. Import PPU640/Point/Clamp/무압축/no-mipmap/max4096, visualScale1.05로 기존 Idle 폭 23/32WU와 신규438/640×1.05WU를 맞춥니다.
원본 32px 시트와 meta, MonsterDefinition meta GUID는 보존합니다.
