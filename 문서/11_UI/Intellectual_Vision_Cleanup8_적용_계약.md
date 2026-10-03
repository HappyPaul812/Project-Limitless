# Intellectual/Vision 8종 cleanup 반영 계약

2026-10-03. 입력 `F:/Downloads/Limitless_IntellectualVision_8_Cleanup.zip`, SHA256 `8549ddac9d27465ce5db017a47af3306680de332cd4f87e48290265879acb03e`. PNG 8개, 추가 파일·중복·누락 없음. 512×512 RGBA, 4×4, 16프레임, 셀 128px. 사용자 대상 목록과 LOCAL 매핑을 기준으로 ZIP의 Vision을 기존 Visual 파일명에, Sharpshooter를 기존 Marksman에 대응한다. 픽셀 변환 없이 원본 바이트를 반영한다.

|Path|Job|Gender|기존 판정|이번 검증 목표|
|---|---|---|---|---|
|Intellectual|Fighter|Female|BLOCKED_ART|Left 조각, Right 머리 조각, Up 머리 crop|
|Intellectual|Mage|Female|BLOCKED_ART|Right 머리 조각, Up 머리 crop|
|Intellectual|Mage|Male|BLOCKED_ART|Right 잔여 조각|
|Vision|Fighter|Male|BLOCKED_ART|Left 방향 유지 및 Left/Right 조각|
|Vision|Guardian|Male|BLOCKED_ART|Right 머리 조각, Up 머리 crop|
|Vision|Mage|Female|BLOCKED_ART|Right 잔여 조각|
|Vision|Sharpshooter|Female|BLOCKED_ART|Left 잔여 조각|
|Vision|Sharpshooter|Male|BLOCKED_ART|Right 잔여 조각|

순서는 문서화 → 구현 → 검증이다. 범위는 위 8종 교체 + 판정 재검증이며 다른 42종은 PNG/meta/참조/QA 유지 대상이다. Intellectual Fighter Male READY도 유지한다. 시작 상태는 Ready 22 / Blocked 28이며 기존 사용자 변경 94항목을 보존한다.

ZIP·추출 원본·이전 8 PNG를 보존한다. 기존 Appearance ID, Sprite GUID/fileID, meta, Multiple/PPU128/Point/Uncompressed/128px Slice/Pivot(64,0), Catalog Entry와 자동 Gender+Path+Job Mapping 및 Save→Continue/fallback 구조를 유지한다. 문제가 없을 때만 해당 Entry를 READY로 승격하며, 남은 Art 문제는 BLOCKED_ART와 0-based 프레임으로 기록한다. 시트 재구성·ID 재발급·다른 기능 작업은 하지 않는다.

백그라운드에서 Import/16 Sprite/참조·매핑·Preview/Selection·대표 World/Battle Left Idle·Save→Continue·Empty character swap·save-restore·compile/Console를 확인한다. 격리 Save/Settings와 기존 PlayUnfocused 감사 도구를 사용한다. Foreground Play/Game View/포커스 전환/실제 걷기·키보드·게임패드 조작은 실행하지 않으며 미검증으로 남긴다. 기존 deprecated 경고는 별도 기록한다. 직접 변경한 파일만 단계별 commit하며 GitHub Push는 하지 않는다.

반영 후 결과는 `Intellectual_Vision_Cleanup8_QA.md/.json`과 CURRENT_STATUS 및 Appearance/Matrix 문서에 기록한다.
