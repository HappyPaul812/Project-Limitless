# Intellectual4 / Vision5 Revised9 QA

2026-10-03. [입력·9개 ID·이전 판정·계약](Intellectual_Vision_Revised9_적용_계약.md)에 따라 지정9 PNG를512×512 원본 바이트 그대로 교체했다. 원본ZIP/추출PNG·교체 전 PNG/기존Git 이력을 보존했다. 다른41종은 변경하지 않았다.

## 개별 판정

144Frame 배경 합성 확대 관찰·Alpha 분리 성분 검사. Empty0, 시트별 픽셀 Hash Duplicate0, RGBA Alpha0–255. 본체 Down/Left/Right/Up·성별/직업 Identity와 Scale를 관찰했다. 심각한 내부 Alpha 손상·Character Swap 없음. 같은128px Cell 안쪽에 남은 조각/절단도 불합격 사유로 기록한다. Inspector 규격만으로 PASS하지 않았다.

|Path|Job|Gender|기존→이번|0-based 문제 Frame|구체적 사유|
|---|---|---|---|---|---|
|Intellectual|Fighter|Female|BLOCKED_ART→BLOCKED_ART|4–15|Left 발 아래 외딴 픽셀, Right 발 아래 큰 머리 조각, Up 머리 상단 절단|
|Intellectual|Fighter|Male|BLOCKED_ART→READY|없음|16Frame에서 기존 절단·분리 조각 해결, 본체4방향/Alpha/Scale/캐릭터 일관성 확인|
|Intellectual|Mage|Female|BLOCKED_ART→BLOCKED_ART|4–15|Left 머리 위 잔여 픽셀, Right 머리 조각, Up 머리 상단 절단|
|Intellectual|Mage|Male|BLOCKED_ART→BLOCKED_ART|8–11|Right 발 아래 갈색 조각. Alpha>10의 별도 성분13+5/12+5/13+6/13+7px|
|Vision|Fighter|Male|BLOCKED_DIRECTION→BLOCKED_ART|3–15|Left5/7 방향 해결. Left 머리 위 검은 발 조각·Right 아래 조각·셀 옆 검 끝 조각 잔존|
|Vision|Guardian|Male|BLOCKED_ART→BLOCKED_ART|4–15|Left 아래 검은 조각, Right 아래 흰 머리 조각, Up 머리 상단 절단|
|Vision|Mage|Female|BLOCKED_ART→BLOCKED_ART|4–11|Left 머리 위 잔여 픽셀, Right 아래 검은/청색 조각|
|Vision|Sharpshooter|Female|BLOCKED_ART→BLOCKED_ART|4–11|Left 머리 위 검은 발 조각·옆 픽셀, Right 아래 잔여 조각|
|Vision|Sharpshooter|Male|BLOCKED_ART→BLOCKED_ART|4–11|Left 머리 위/옆 조각, Right 발 아래 검은 조각|

대상 Ready1/Blocked8. 전체 **Ready22/Blocked28**(작업 전21/29). Intellectual10은Ready7/Blocked3, Vision10은Ready5/Blocked5. Hearing10/Mobility10 BLOCKED_ART·EmotionalScar10 PASS 유지. 남은8종은 완전한 머리/장비를 각Cell 안에 배치하고 잔여 조각을 제거하는 원본 미술 수정이 필요하다.

## Import·연결·보존

기존9 Appearance ID/Gender/Path/Job·144 Sprite GUID/fileID·meta Hash 동일. Multiple/PPU128/Point/Uncompressed·Rect128×128/Pivot(64,0) 유지. Intellectual Fighter Male에만 Down/Left/Right/Up Idle/Walk8개 Catalog subasset을 추가했다. Idle각 행 첫 Frame, Walk행4장·6fps/loop. 공용 성별 Animator Override 구조 유지. 새 Appearance ID·Save Migration·수동 UI 없음.

다른41종 PNG/meta·Catalog Entry/Sprite/Clip·Inventory·QA 값 및 표 행은 시작 상태와 동일하다. 기능 C#·UI/Scene/Quest/Voice/BGM/전투/Save 코드는 변경하지 않았다. 기존 QA 감사 도구만 Ready 집계를 Inventory와 비교하고 승격한 조합도 검사하도록 보정했다. 전체 Import/Build는 실행하지 않았다.

## 검증

사용자 승인 범위는 격리 Save/Settings·백그라운드 PlayUnfocused다. **89 PASS/0 FAIL**: 9조합 실제 UI Preview/Mapping, Intellectual Male Fighter·Vision Female Mage 대표 생성/World/Battle Left Idle/Save→Bootstrap Continue를 기존 감사로 확인했다. Ready1은 수정판을 적용하고 Blocked8은 기본 성별 임시 fallback을 유지한다. Ready8 Clip의 각 key/16Frame 연결·6fps/loop도 검사했다. 사람의 걷기 품질·Game View·물리 입력·포커스 전환 검증은 포함하지 않는다.

Compile/Console Error0·최종 Warning0. 컴파일 직후 기존 ExternalAssetImportEditor.cs deprecated 경고2개는 확인했다. clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션/백그라운드 설정 복원. 관련 diff 검사 통과.

결과는 [Runtime 출력](Intellectual_Vision_Revised9_Runtime_Results.txt)·[JSON](Intellectual_Vision_Revised9_QA.json)을 따른다.144Frame 증거는 `Unity/Client/Temp/Revised9Audit/*_Frames.png`, 이전9PNG는 `Before/`, 새9원본은 `Original/`에 보존했다. GitHub main 조회·Push 없음.
