# Intellectual/Vision Restored8 원본 교체 계획

2026-10-05. LOCAL 프로젝트 정본, 시작 Ready22/Blocked28. 입력 `F:/Downloads/Limitless_IntellectualVision_8_Restored.zip`. 문서화 → 구현 → 검증. PNG8/중복0/누락0/512×512 RGBA·4×4·16Frame·Cell128px를 확인한다. 파일명 Vision/Sharpshooter를 기존 Visual/Marksman 명명에만 대응하며 외형으로 매핑하지 않는다.

대상은 Intellectual Fighter Female·Mage Female/Male, Vision Fighter Male·Guardian Male·Mage Female·Sharpshooter Female/Male의8종이다. 이전 문제는 [최신 Cell QA](Intellectual_Vision_CellCleanup_QA.md)를 기준으로 검사한다. 모든16프레임에서 원래 소실된 머리/리본/망토/검/지팡이 윤곽의 실제 복원, 조각/경계 침범/방향/Alpha/Character Swap/Empty/Scale을 확인한다. Vision Fighter Male 정상 방향을 유지한다. 파일명이 Restored라는 이유로 READY 승격하지 않는다.

PNG/ZIP의 바이트를 그대로 반영한다. 이미지 재생성·픽셀 수정·이동·crop·조각 삭제·파츠 복원은 하지 않는다. QA용 확대 관찰 이미지는 Temp에만 만들며 게임 Art는 원본 그대로 유지한다. 각 대상은 실제 결과로 READY 또는 구체적인 BLOCKED 사유/0-based Frame/위치/파츠를 기록한다.

기존 Stable Appearance ID·Catalog Entry·Sprite Definition·Gender/Path/Job·Save Mapping·128 Sprite GUID/fileID·meta·Import/Multiple/PPU/Pivot/Filter/Compression을 유지한다. 다른42종 PNG/meta/Entry/QA/Clip 보존, 특히 Intellectual Fighter Male READY·Hearing/Mobility/EmotionalScar 불변. 승격할 때만 기존8 Entry의 검수 필드와 같은 Catalog 안의 기존 정의에 필요한8방향 Idle/Walk Clip을 연결한다. 전체 Catalog 재생성 도구는 실행하지 않는다.

원본/이전8PNG 및 Assets/ProjectSettings/UserData 시작 해시와 git 상태를 보존한다. Unity Import/Slice/참조/8종 Mapping/Preview 및 대표 Intellectual Mage Male·Vision Mage Female World/Battle Left Idle/Save→Continue를 기존 격리 PlayUnfocused 감사로 확인한다. C#/Save Migration/UI 기능 변경 없음. Console/컴파일 상태와 Save/Settings/Play 설정 복원 검사. Foreground/Game View/물리 입력은 실행하지 않는다.

개별 QA·50조합 Matrix·CURRENT_STATUS를 갱신하고 직접 변경 파일만 stage/commit한다. ZIP·다른42종·Voice/BGM/Quest stage 및 GitHub Push 없음.
