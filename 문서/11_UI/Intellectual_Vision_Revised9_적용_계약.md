# Intellectual4 / Vision5 Revised9 적용 계약

2026-10-03. 입력 `F:/Downloads/Limitless_Intellectual4_Vision5_Revised.zip`, SHA256 `eebc04cd0c0cb293c49d011dd03ad347d2fef922b810ba3a359f9dbf537a45e7`. PNG9개와 README/VALIDATION.csv. 모두512×512 RGBA·4×4/128px/16Frame. 변환 없이 원본 바이트 그대로 기존 PNG 경로에 반영한다. 이전9 PNG는 로컬 `Unity/Client/Temp/Revised9Audit/Before/`와 기존 Git 이력에 보존하고 입력ZIP/추출 원본도 보존한다.

## 범위·기존 판정

|Path|Job|Gender|기존 Appearance ID|기존 판정|
|---|---|---|---|---|
|Intellectual|fighter|Female|appearance.external.v1.intellectual.fighter.female|BLOCKED_ART|
|Intellectual|fighter|Male|appearance.external.v1.intellectual.fighter.male|BLOCKED_ART|
|Intellectual|mage|Female|appearance.external.v1.intellectual.mage.female|BLOCKED_ART|
|Intellectual|mage|Male|appearance.external.v1.intellectual.mage.male|BLOCKED_ART|
|Vision|fighter|Male|appearance.external.v1.vision.fighter.male|BLOCKED_DIRECTION|
|Vision|guardian|Male|appearance.external.v1.vision.guardian.male|BLOCKED_ART|
|Vision|mage|Female|appearance.external.v1.vision.mage.female|BLOCKED_ART|
|Vision|sharpshooter|Female|appearance.external.v1.vision.marksman.female|BLOCKED_ART|
|Vision|sharpshooter|Male|appearance.external.v1.vision.marksman.male|BLOCKED_ART|

파일名 Intellectual/Visual 및 Marksman의 대응은 기존 Inventory/README/LOCAL Job ID로 확정한다. Duplicate0/Missing0. 다른41종 PNG/meta/Catalog 참조/QA는 보존한다. Identity/Appearance/Sprite GUID/fileID·Gender+Path+Job 자동 Mapping·Save 구조·fallback 정책은 유지한다. 기존 Multiple/PPU128/Point/Uncompressed/128px Rect/Pivot(64,0)을 유지한다.

## 절차·검증

문서→구현→검증. 이번144Frame 관찰로만 Ready/Blocked 재판정하고 대상 Entry의 QA와 필요한 방향Clip만 반영한다. Ready 승격 시 기존 공용 Animator 구조에8개 Clip만 연결하며 다른41 Clip은 수정하지 않는다. 전체50 Build/Import 금지. 기능/UI/퀘스트/음성/BGM/전투/Save 코드는 수정하지 않는다. QA 감사의 대상·기대 집계만 필요한 경우 갱신한다.

사용자가 이번 백그라운드 PlayUnfocused·격리 Save/Settings 검증을 승인했다. 창 활성화/GameView 선택/포커스 전환/실입력은 승인되지 않았으며 실행하지 않는다. 대상9 UI Preview/Mapping·대표 World/Battle Left Idle·Save→Continue를 확인한다. 원본/기존41 Hash와 참조/판정을 비교하고 실제 결과·전체 Ready/Blocked·미검증을 QA/CURRENT_STATUS에 기록한다. GitHub main 조회 및 Push 없음.
