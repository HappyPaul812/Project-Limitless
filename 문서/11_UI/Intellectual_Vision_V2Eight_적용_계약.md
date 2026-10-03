# Intellectual3 / Vision5 V2 적용 계약

2026-10-03. 새 입력 `F:/Downloads/Limitless_IntellectualVision_8_Fixed_v2.zip`, SHA256 `eb7becc25e1728fae0e87b9042b48b174d428f68b4b60c31d3c9390b70e9b308`. 캐릭터PNG8개·README1개, CSV/Manifest 없음. 모두512×512 RGBA·4×4/128px/16Frame, 추가PNG/중복/누락0. 파일명/README·LOCAL Inventory/Matrix로 기존8개 조합에 대응한다. Marksman은 LOCAL Job ID sharpshooter다.

|Path|Job|Gender|기존 Appearance ID|이전 상태·문제 Frame|
|---|---|---|---|---|
|Intellectual|fighter|Female|appearance.external.v1.intellectual.fighter.female|BLOCKED_ART4–15|
|Intellectual|mage|Female|appearance.external.v1.intellectual.mage.female|BLOCKED_ART4–15|
|Intellectual|mage|Male|appearance.external.v1.intellectual.mage.male|BLOCKED_ART8–11|
|Vision|fighter|Male|appearance.external.v1.vision.fighter.male|BLOCKED_ART3–15|
|Vision|guardian|Male|appearance.external.v1.vision.guardian.male|BLOCKED_ART4–15|
|Vision|mage|Female|appearance.external.v1.vision.mage.female|BLOCKED_ART4–11|
|Vision|sharpshooter|Female|appearance.external.v1.vision.marksman.female|BLOCKED_ART4–11|
|Vision|sharpshooter|Male|appearance.external.v1.vision.marksman.male|BLOCKED_ART4–11|

사용자 요청의 기존 문제 색상/방향 설명은 참고하며 LOCAL 직전 QA에서는 Vision Fighter Male Left 방향은 이미 해결, Art 조각만 남았다고 기록했다. 이번에는 실제V2 모든128Frame을 다시 관찰한다.

문서→구현→검증. 기존8 PNG만 V2 원본 바이트 그대로 교체하며 ZIP/추출원본과 이전8PNG/Git 이력을 보존한다. 기존 ID/Catalog Entry/128 Sprite GUID/fileID/meta/PPU128/Point/Uncompressed/Multiple/Rect/Pivot(64,0)·자동 Gender+Path+Job Mapping·Save/fallback 유지. 새 ID·Migration·수동 외형 UI·기능 변경 없음.

다른42종은 PNG/meta/참조/QA 모두 시작값과 비교해 보존한다. 특히 Intellectual Fighter Male은 교체/재판정하지 않으며 READY·8 Clip을 그대로 유지한다. 승격 대상이 있으면 해당 Entry의8 방향Clip만 추가한다. 전체50 Build/Import 금지. 검증 감사 도구의 대상/대표 조합만 이번8종에 맞춘다.

Background/Edit Mode 우선, 격리 Save/Settings·PlayUnfocused에서 대상8 UI Mapping/Preview 및 Intellectual/Vision 대표 World/Battle Left Idle/Save→Continue를 확인한다. Foreground/GameView 선택/포커스 전환/물리 입력은 별도 승인 없이 실행하지 않는다. 실제 미검증은 최종QA에 남긴다. 원본 문제·V2 문제/판정·Ready집계·42종 보호를 문서화한다. GitHub Push 없음.
