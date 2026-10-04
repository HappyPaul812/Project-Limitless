# Intellectual/Vision Cleanup v2 8종 적용 계획

2026-10-05. LOCAL 프로젝트가 정본이다. 문서화 → 원본 교체 → 검증 순서로 진행한다. 입력은 `F:/Downloads/Limitless_IntellectualVision_8_Cleanup_v2.zip`이며 ZIP과 픽셀은 수정하지 않는다.

대상은 Intellectual Fighter Female, Mage Female/Male 및 Vision Fighter Male, Guardian Male, Mage Female, Sharpshooter Female/Male의 정확히 8종이다. 기존 Cleanup QA의 문제 프레임은 각각 4–15, 8–15, 8–11, 4–10, 8–15, 8–11, 4–7, 8/11이다. 8종 모두 16프레임을 재검수하며 수정판이라는 이유로 READY를 부여하지 않는다. Vision Fighter Male의 정상 Left/Right 방향 유지도 확인한다.

ZIP의 Vision/Sharpshooter 파일명만 기존 Visual/Marksman Asset 명명에 대응한다. 기존 Appearance ID, Gender/Path/Job, Catalog Entry, Sprite Definition, 128 Sprite GUID/fileID, meta, Multiple/4×4/128px/PPU/Pivot/Filter/Compression, Animation 참조와 Save/Continue 자동 Mapping을 유지한다. 다른 42종과 Intellectual Fighter Male READY, Hearing/Mobility 판정은 변경하지 않는다. 작업 전 Ready22/Blocked28에서 실제 통과한 수만 승격한다.

ZIP Audit에서 PNG8/중복0/누락0/512×512 RGBA를 확인한다. 기존 PNG와 추출 원본, Assets 전체 해시와 사용자 git 상태를 Temp/CleanupV2Audit에 보존한다. 원본 프레임 확대 관찰 및 Alpha/연결 성분 검사를 함께 사용하며 경계 접촉만으로 crop을 단정하지 않는다. BLOCKED에는 0-based 프레임·위치·사유를 기록한다.

기존 Partial9FixedSpriteAudit는 정확히 이번 8종의 Mapping/Preview와 대표 Intellectual Mage Male·Vision Mage Female의 생성/World/Battle Left Idle/Save→Bootstrap Continue를 지원한다. clean Bootstrap Edit Mode 확인 후 PlayUnfocused·격리 Save/Settings를 이용해 실행하고 설정 복원을 확인한다. 화면 포커스 전환·Game View 활성화·실제 물리 입력은 하지 않는다. C# 변경 없이 PNG/검수 데이터만 반영하고 Unity 참조/Console/컴파일 진행 상태 및 diff를 확인한다.

개별 QA, 50조합 Matrix 및 CURRENT_STATUS를 갱신한다. 직접 변경 파일만 stage/commit하며 ZIP·기존 사용자 변경·다른 Sprite는 stage하지 않는다. GitHub Push는 하지 않는다.
