# Intellectual/Vision Cleanup8 QA

2026-10-03. [계약](Intellectual_Vision_Cleanup8_적용_계약.md). 문서 b73d949 → 반영 745a913 → 검증 기록. ZIP SHA256 `8549ddac9d27465ce5db017a47af3306680de332cd4f87e48290265879acb03e`. PNG8/추가0/중복0/누락0. 모두512×512 RGBA·4×4·16Frame·128px, 변환 없이 원본 바이트 적용. ZIP Vision/Sharpshooter는 기존 Visual/Marksman에 대응한다.

128Frame 확대 관찰·Alpha>10 성분 검사: Empty0, 시트별 프레임 픽셀 Hash 중복0. 심각한 본체 내부 Alpha 손상·Character Swap·Scale 급변 없음. Vision Fighter Male Left 방향 정상. 잔여 Art 문제로 READY0/BLOCKED_ART8, 전체 **Ready22/Blocked28 유지**.

|Path|Job|Gender|판정|0-based 문제 프레임|사유|
|---|---|---|---|---|---|
|Intellectual|fighter|Female|BLOCKED_ART|4-15|Left 4–7 발 아래 조각, Right 8–11 큰 머리 조각, Up 12–15 머리 상단 수평 절단.|
|Intellectual|mage|Female|BLOCKED_ART|8-15|Right 8–11 큰 머리 조각, Up 12–15 머리 상단 수평 절단.|
|Intellectual|mage|Male|BLOCKED_ART|8-11|Right 8–11 셀 하단 갈색 조각 17/23/18/21px가 몸체와 분리되어 남음.|
|Vision|fighter|Male|BLOCKED_ART|4-10|Left 4–7 머리 위 검은 조각, Right 8–10 발 아래 조각. Left 방향은 정상.|
|Vision|guardian|Male|BLOCKED_ART|8-15|Right 8–11 큰 머리 조각, Up 12–15 머리 상단 수평 절단.|
|Vision|mage|Female|BLOCKED_ART|8-11|Right 8–11 셀 하단 분리 조각. 8/10 파란 조각과 회색 선, 9/11 작은 잔여 픽셀.|
|Vision|sharpshooter|Female|BLOCKED_ART|4-7|Left 4–7 머리 위 셀 상단 검은 조각 19/24/23/26px.|
|Vision|sharpshooter|Male|BLOCKED_ART|8,11|Right 8/11 셀 최하단 10px 가로 조각이 다음 Up 행 머리 위에 나타남.|

8종 Appearance ID, sheet GUID/128 Sprite GUID/fileID, meta 해시, Multiple/PPU128/Point/Uncompressed/Rect128/Pivot(64,0), Clip 참조 유지. 다른42종 PNG/meta·Catalog Entry·Inventory/QA/Matrix 판정 동일. 전체Assets 시작 Hash 비교: 8PNG+Catalog+Inventory 10파일만 변경, meta변경0. Hearing/Mobility·Intellectual Fighter Male READY 보존. 이전8PNG는 Temp/CleanupEightAudit/Previous와 Git 이전 이력, 원본은 Original 및 사용자ZIP 보존.

[런타임 기록](Intellectual_Vision_Cleanup8_Runtime_Results.txt) 86PASS/0FAIL: 대상8 자동 Mapping/Preview/Selection, 대표 Intellectual Mage Male·Vision Mage Female World/Battle Left Idle/Save→Bootstrap Continue·save-restore, Empty/불일치 Appearance ID 재계산 통과. BLOCKED는 기존 성별 임시 fallback을 표시한다. Save/Runtime 코드 변경 없음.

Unity6000.5.7f1, import 오류/참조 누락0, compile 진행 없음·최종 Console Error0/Warning0. PNG/데이터 변경으로 새 C# 컴파일은 수행하지 않았다. 기존 deprecated 경고는 이전 이력으로 별도 유지한다. 잘못된 ScriptableObject JSON 생성 도구 요청 1회는 수정했으며 Import/런타임 오류는 없었다. 깨끗한 Bootstrap Edit Mode·is_focused=false, 격리 Save/Settings/Play 설정 복원 확인.

Foreground/Game View/포커스 전환·실제 걷기 품질·키보드/게임패드 입력 미검증. 다음 작업은 각 원본의 crop 복구와 외딴 조각 제거다. 자동 픽셀 수정·시트 재구성 없음. 작업 범위 diff --check 통과. 전체 diff --check에는 기존 사용자 변경의 whitespace가 남아 수정하지 않았다. 기존 사용자94항목 보존, 직접 변경 파일만 commit. GitHub Push 없음.
