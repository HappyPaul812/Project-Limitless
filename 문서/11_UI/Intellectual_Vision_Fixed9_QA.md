# Intellectual4 / Vision5 수정판 QA

2026-10-03. [입력 ZIP·9조합·기존 ID·적용 계약](Intellectual_Vision_Fixed9_적용_계약.md)대로 정확히9개만 교체했다. Intellectual4개는512×512 원본 바이트 그대로, Vision5개는 이번 사용자 승인으로 원본1254×1254를 보존한 별도512×512 최근접 변환본이다. 원본 ZIP 수정·그림 재생성/복원/재배치/Alpha 보정 없음.

## 미술 재판정

144Frame을 확대하여 관찰했다. Empty0·시트별 동일 픽셀 Duplicate0·RGBA Alpha0–255. 심각한 본체 내부 Alpha 구멍·캐릭터 교체·본체 Scale 급변은 관찰되지 않았다. 셀 안쪽의 잘린 머리/잔여 조각은 경계 접촉 여부와 별도로 판정했다. Pivot/Rect/PPU는 기존과 동일하다. Idle은 각 행 첫 Frame, Walk는 행4장이다. 실제 걷기·물리 입력·Foreground는 미검증이다.

|Path|파일명|Gender|Job Stable ID|0-based 문제 Frame|판정·구체적 사유|
|---|---|---|---|---|---|
|Intellectual|Intellectual_Fighter_Female.png|Female|fighter|4–15|BLOCKED_ART: Left/Right 머리 조각·검 끝 분리, Right/Up 머리 절단|
|Intellectual|Intellectual_Fighter_Male.png|Male|fighter|4–15|BLOCKED_ART: Left 머리 위/발 아래 조각, Right 머리 조각, Up 머리 절단|
|Intellectual|Intellectual_Mage_Female.png|Female|mage|4–15|BLOCKED_ART: Left/Right 머리 조각, Right/Up 머리 절단|
|Intellectual|Intellectual_Mage_Male.png|Male|mage|4–15|BLOCKED_ART: Left 발 아래/옆 조각, Right 머리 조각, Up 머리 절단|
|Vision|Visual_Fighter_Male.png|Male|fighter|4–11|BLOCKED_DIRECTION: Left frame5/7이 Right를 향함. Left/Right 잔여 조각·frame7/11 검 끝 절단도 있음|
|Vision|Visual_Guardian_Male.png|Male|guardian|4–15|BLOCKED_ART: Left 잔여 조각, Right 흰 머리 조각, Up 머리 절단|
|Vision|Visual_Mage_Female.png|Female|mage|8–11|BLOCKED_ART: Right 하단에 지팡이·의상으로 보이는 외딴 픽셀 조각|
|Vision|Visual_Marksman_Female.png|Female|sharpshooter|4–7|BLOCKED_ART: Left 머리 위 이전 행 발로 보이는 검은 조각|
|Vision|Visual_Marksman_Male.png|Male|sharpshooter|8–11|BLOCKED_ART: Right 발 아래 외딴 검은 조각|

Intellectual PASS0/Blocked4, Vision PASS0/Blocked5. 본체는 대체로 Down/Left/Right/Up을 구분하지만 Vision Fighter Male은 Left 행 방향 혼합 때문에 방향 검수 불합격이다. 각128px Cell에 완전한 머리·몸·장치를 배치하고 잔여 조각을 제거하며, Fighter Left5/7을 실제 Left 그림으로 수정하는 것을 권장한다.

확대 증거는 `Unity/Client/Temp/Partial9FixedAudit/*_Frames.png`, 상세 Hash/변환 여부/144Frame bbox는 [JSON](Intellectual_Vision_Fixed9_QA.json)에 기록한다.

## Import·Identity·다른41종

기존 Appearance ID9개·Catalog Entry·Gender/Path/Job·Save 구조 유지, 새 ID0. Multiple/4×4/128px Cell/PPU128/Point/Uncompressed/Pivot(64,0),144 Sprite GUID/fileID와 meta Hash 동일. 대상9개만 명시적으로 ForceImport했고 전체 Build/Import는 실행하지 않았다. 다른41종 PNG/meta·Catalog Entry/Sprite/Clip·Inventory/QA 동일. Mobility10 BLOCKED_ART/Hearing10 BLOCKED_ART 보존. Voice/BGM/Main16 변경 없음.

## Runtime·전체 상태

격리 Save/Settings·PlayUnfocused 감사 **89 PASS/0 FAIL**: 대상9조합 실제 Path/Job UI 자동 Mapping/Preview/fallback 표시, 대표 Intellectual Male Fighter / Vision Female Mage 생성→Confirm→World→Save→Bootstrap Continue→Battle Scene 전달 통과. 과거 Appearance ID 누락/불일치도 정본 조합으로 복원한다. 미술 Blocked는 기본 성별 임시 fallback으로 실행되며 수정판 Ready Art를 재생했다고 간주하지 않는다. 결과는 [Runtime 감사](Intellectual_Vision_Fixed9_Runtime_Results.txt)를 따른다. 자동 매핑/Save 정본과 미술 Ready를 구분한다.

Compile/Console Error0·최종 Warning0, clean Bootstrap Edit Mode. 격리 Save/Settings와 Play 옵션/백그라운드 실행 설정 복원, 포커스 조작 없음. 시작 보호880파일 중 대상9 PNG·Catalog·Inventory11개만 변경. 관련 diff 검사 통과.

작업 전 Ready21/Blocked29 → 작업 후 **Ready21/Blocked29**. 추가 미술 수정 대상은 Hearing10·Mobility10·Intellectual Fighter/Mage 남녀4·Vision Fighter Male/Guardian Male/Mage Female/Sharpshooter 남녀5이다. 다른21종 PASS는 재판정하지 않았다. GitHub Push 없음.
