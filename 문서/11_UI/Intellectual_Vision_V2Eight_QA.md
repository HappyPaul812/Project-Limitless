# Intellectual3 / Vision5 V2 QA

2026-10-03. [입력 ZIP·8조합 ID·기존 문제·계약](Intellectual_Vision_V2Eight_적용_계약.md)대로8 PNG만 원본 바이트 그대로 교체했다.512×512 RGBA/Alpha0–255/4×4/128px/16Frame, Duplicate Target0/Missing0, 추가PNG0. 원본ZIP·추출PNG와 이전8PNG/Git 이력 보존. 변환/그림 복원/재생성/Alpha 보정 없음.

## 128Frame 재판정

모든16Frame 확대 관찰과 Alpha>10 연결 성분 검사. Empty0·시트별 픽셀 Hash Duplicate0. 심각한 본체 내부 Alpha 손상·Character Swap·Scale 급변 없음. 각 방향 본체를 확인했고 Vision Fighter Male Left4–7은 모두 Left를 향한다. 그러나 분리 조각/머리 절단이 남아 **V2 READY0/BLOCKED_ART8**, 전체 **Ready22/Blocked28 유지**다. 경계 내부로 옮겨진 잘린 그림이나 아주 작은 외딴 픽셀도 원본 문제로 기록했다.

|Path|Job|Gender|V2 판정|0-based 문제 Frame|남은 문제|
|---|---|---|---|---|---|
|Intellectual|Fighter|Female|BLOCKED_ART|4–15|Left 발 아래 외딴 픽셀, Right 큰 머리 조각, Up 머리 상단 수평 절단|
|Intellectual|Mage|Female|BLOCKED_ART|4–15|Left 머리 위 잔여 픽셀, Right 머리 조각, Up 머리 상단 절단|
|Intellectual|Mage|Male|BLOCKED_ART|9–10|큰 조각 제거됐으나 frame9 발 아래2px(y124), frame10 아래2px(y125)+3px(y127) 조각|
|Vision|Fighter|Male|BLOCKED_ART|4–11|Left 방향 일치, 머리 위 검은 선/Right 발 아래 조각 잔존. 검 끝/옆 셀 끝도 재검토 권장|
|Vision|Guardian|Male|BLOCKED_ART|8–15|Right 발 아래 흰 머리 조각·Up 머리 상단 절단|
|Vision|Mage|Female|BLOCKED_ART|8,10|셀 하단 청색15px씩, frame8 검은3px 잔여 조각(y125–127)|
|Vision|Sharpshooter|Female|BLOCKED_ART|5–7|머리 위 검은25/7/32px 조각, frame6/7 왼쪽 Cell끝14/12px 분리 조각|
|Vision|Sharpshooter|Male|BLOCKED_ART|6,8,11|Left6 경계 청색23px 조각, Right8/11 아래 y127 검은10/9px 조각|

Intellectual Ready0/Blocked3·Vision Ready0/Blocked5. 머리 절단은 완전한 원본 머리/장비를128px Cell 안에 배치하고, 잔여 조각은 실제 투명 배경에서 확대 검사하며 제거해야 한다. 본체 안쪽의 작은 색채 성분은 분리 장식일 수 있어 무조건 오류로 보지 않았고, 여기서는 본체 밖/셀 끝 성분을 구체적으로 기록했다.

## 보존·Import·Runtime

기존8 Appearance ID/Gender/Path/Job·128 Sprite GUID/fileID·meta/PPU128/Point/Uncompressed/Multiple·Rect128×128/Pivot(64,0) 유지. 모든8개 Frame 참조 정상. 새 ID/Clip·Save Migration/수동 UI 없음. 다른42종 PNG/meta·Catalog Entry/Sprite/Clip·Inventory/QA 동일. 특히 Intellectual Fighter Male의 직전 READY·PNG/meta/8 Clip은 교체/재판정하지 않았다. 전체 Import/Build 및 기능/UI/퀘스트/Voice/BGM/전투/Save 변경 없음.

기존 격리 PlayUnfocused QA 도구의 대상/대표만 이번8종에 맞췄다. **86 PASS/0 FAIL**: 대상8 UI Mapping/Preview와 Intellectual Mage Male·Vision Mage Female 대표 생성/World/Battle Left Idle/Save→Bootstrap Continue를 통과했다. 기존 ID가 누락/불일치한 Save도 정본 조합을 복원한다. Blocked8은 기본 성별 임시 fallback이며 수정판 Ready Art 재생을 완료했다고 하지 않는다. 실제 걷기·GameView/포커스 전환·물리 입력은 미검증이다.

Compile/Console Error0·최종 Warning0. 컴파일 직후 기존 deprecated 경고2개 확인. clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션/백그라운드 설정 복원. 관련 diff 검사 통과.

결과 [Runtime 출력](Intellectual_Vision_V2Eight_Runtime_Results.txt)·[QA JSON](Intellectual_Vision_V2Eight_QA.json). 증거 `Unity/Client/Temp/V2EightAudit/*_Frames.png`, `Original/`·`Before/`에 새/이전PNG 보존. 추가 수정 대상 전체28종은 Hearing10·Mobility10·이번8종이다. GitHub Push 없음.
