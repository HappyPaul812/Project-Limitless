# Intellectual/Vision 8종 셀 단위 픽셀 정밀 수정 계획

2026-10-05. LOCAL 정본, Cleanup v2 ZIP `F:/Downloads/Limitless_IntellectualVision_8_Cleanup_v2.zip` SHA256 `95d2299df6b09e69165bc31a51045fad691901be1df58b97aad2edde3e737822` 기준. 문서화 → 구현 → 검증. 입력 ZIP은 수정하지 않는다. 시작 Ready22/Blocked28.

|대상|수정 허용 0-based Frame|
|---|---|
|Intellectual Fighter Female|8–15|
|Intellectual Mage Female|12–15|
|Intellectual Mage Male|12–15|
|Vision Fighter Male|1–3,5,7,9,11–14|
|Vision Guardian Male|5,6,8–15|
|Vision Mage Female|12–15|
|Vision Sharpshooter Female|5,6|
|Vision Sharpshooter Male|5,6|

512×512 RGBA를 정확히16개의128×128 셀로 추출한다. 지정 셀만 Alpha Bounds/연결 성분·주변 프레임을 교차 분석한다. 본체와 분리된 확실한 작은 잔여 조각을 제거하고, 캐릭터 또는 필요한 파츠에 정수 픽셀 최소 이동을 적용한다. 픽셀 색/Alpha 보간·리사이즈·반전·방향 재설계·프레임 순서 변경·생성형 재작업·머리 다시 그리기·의상/얼굴/장비 디자인 변경을 금지한다. 이동이 반대쪽 잘림이나 파츠 연결 손상을 만들면 적용하지 않는다. 이미 원본에서 소실된 윤곽은 이동만으로 복원됐다고 보고하지 않는다.

정상 셀은 RGBA 바이트/Hash/Pixel Diff 변경0을 강제한다. 원본·셀별 작업 내역·삭제 영역·정수 이동량·수정 전후 이미지와 해시를 보존하고 재합성 시 크기/셀 순서를 검사한다. 8종 전체128프레임을 최종 관찰하여 남은 문제를 기록한다. 경계 접촉만 제거하거나 투명 여백만 추가한 것으로 READY 승격하지 않는다. READY는 crop/외딴 조각/방향/본체Alpha/Character Swap 등 실제 QA 전체 통과 때만 부여한다.

대상8종 기존 Stable Appearance ID·Sprite ID·meta·Import·Gender/Path/Job·Catalog Entry·Save 자동 Mapping을 유지한다. 다른42종 PNG/meta/Entry/QA/Clip을 변경하지 않는다. 특히 Intellectual Fighter Male READY·Hearing/Mobility 보존. 상태 변경이 필요하면 대상 Entry 검수 필드만 갱신한다. C#/Runtime 기능 개발 없음.

Unity Import/16 Slice/Mapping/Preview·대표 Intellectual Mage Male 및 Vision Mage Female World/Battle Left Idle/Save→Continue를 기존 격리 PlayUnfocused 감사로 백그라운드 확인한다. 컴파일 상태·Console 및 설정/Save 경로 복원 확인. Foreground/실제 키보드·게임패드 이동은 미검증으로 남긴다. 개별 QA·Matrix·CURRENT_STATUS를 갱신하고 직접 변경 파일만 commit, ZIP stage 및 GitHub Push 없음.
