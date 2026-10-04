# Intellectual/Vision 8종 셀 단위 픽셀 정밀 수정 QA

2026-10-05. [적용 계획](Intellectual_Vision_CellCleanup_적용_계약.md). Cleanup v2 ZIP SHA256 `95d2299df6b09e69165bc31a51045fad691901be1df58b97aad2edde3e737822` 유지. PNG8·512×512 RGBA·4×4·16프레임·Cell128×128 유지. 기존8종만 수정, 다른42종과 Intellectual Fighter Male READY 유지.

## 실제 수정 방식

Pillow/NumPy/SciPy로 128px 셀 추출 → 지정44셀만 연결 성분 검사/잔여 성분 제거 → 안전한 셀 전체를 정수1–2px 이동 → 4×4 재합성했다. 본체 RGBA를 보간/변색/리사이즈/반전하지 않았다. 생성형 재작업·머리 그리기·프레임 순서/방향 변경 없음. 이동 시 반대쪽 Alpha를 잃거나 새 본체 경계 접촉을 만들면 이동하지 않았다.

실제41셀 수정·34셀 정수 이동, 정상84셀 RGBA Hash/Pixel Diff 변경0. 허용 셀 중3개도 그대로 유지해 전체87셀 변경0이다. 검수한 분리 성분341px 제거(Down2의23px, Up14의169px 검 조각, Guardian Left6의10px, Sharpshooter Male Left6의34px 포함). 이동 범위 밖 Alpha1–2 경계 잔여124px 제거를 별도 기록했다. 남긴 비투명 픽셀 RGBA multiset은 이동 전후 동일하며 새 그림/색/Alpha를 만들지 않았다. 지정하지 않은 프레임의 미세 antialias는 보존했다.

전체128프레임 확대 관찰·Empty/Character Swap/방향/Scale/Alpha 검사. 경계에서 떨어진 것과 이미 소실된 윤곽이 복구된 것은 다르다. 원본에 없는 머리·리본·망토·검 끝을 새로 그리지 않았으므로 절단 단면이 남은8종 모두 BLOCKED_ART다. READY승격0, 전체 Ready22/Blocked28 유지. 본체 Alpha 손상/새 Crop/Character Swap/방향 오류/Empty/Scale 급변 없음. Vision Fighter Male Left/Right 정상 유지. 본체와 떨어진 지정 잔여 조각은 제거했다.

|Sprite|판정|실제 수정 0-based Frame|해결/변경|남은 Frame|남은 위치·문제|
|---|---|---|---|---|---|
|Intellectual_Fighter_Female|BLOCKED_ART|8,9,10,11,12,13,14,15|Right8–11 아래2px, Up12/14 아래1px. 작은 Alpha 잔여 성분 제거.|8-15|Right 8–11 리본 끝의 기존 수평 절단 윤곽이 이동 후 y=2에 남음. Up 12–15 머리/리본 윗부분의 원본 절단 윤곽 잔존. 13/15는 발 Alpha를 보존하기 위해 전체 이동 제외.|
|Intellectual_Mage_Female|BLOCKED_ART|13,15|Up13 아래1px. Up15 미세 분리 성분 제거. 다른 셀 보존.|12-15|Up 12–15 양쪽 리본/머리 상단의 기존 절단 윤곽 잔존. 13 아래1px 이동으로 경계 여백 확보했으나 원래 소실된 윤곽은 복구되지 않음. 12/14/15는 발을 새로 자르므로 이동 제외.|
|Intellectual_Mage_Male|BLOCKED_ART|12,13,14,15|Up12–15 아래2px. 미세 분리 성분과 이동 밖 Alpha1–2 경계 잔여 제거.|12-15|Up 12–15 갈색 머리 끝의 원본 수평 절단 형태가 아래2px 이동 후 y=2에 남음. 경계 접촉은 해소됐으나 소실된 머리 끝 픽셀은 이동으로 복원 불가.|
|Vision_Fighter_Male|BLOCKED_ART|1,2,3,7,9,11,12,13,14|Down2 우측23px·Up14 좌측169px 분리 조각 제거. 검 경계에서 안쪽2px, Up 아래2px 이동. Left5 원본 유지.|1,3,5,7,9,11-14|Down1/3, Left5/7, Right9/11, Up13 검·망토의 원본 잘린 끝 잔존. Left5는 좌우 폭 부족으로 안전 전체 이동 불가. Up12–14 머리 상단의 원본 절단 윤곽 잔존. 방향 정상.|
|Vision_Guardian_Male|BLOCKED_ART|5,6,8,9,10,11,12,13,14,15|Left6 좌측10px 조각 제거. Left5 왼쪽2px, Right8–11 아래2px, Up12–14 아래1px 이동.|5,8-15|Left5 망토 끝의 원본 잘린 윤곽 잔존. Right8–11 머리 끝과 Up12–15 머리 상단의 원본 절단 형태 잔존. 15는 새 발 경계 접촉을 막기 위해 이동 제외.|
|Vision_Mage_Female|BLOCKED_ART|12,13,14,15|Up12–15 아래2px. 지정셀 내 미세 Alpha 잔여 성분 제거. 기존 정상 Left6 저Alpha 흔적은 수정하지 않음.|12-15|Up12–15 머리/지팡이 꼭대기의 원본 절단 단면이 아래2px 이동 후 y=2에 남음. 경계 접촉 해소와 소실된 디자인 복구는 구분함.|
|Vision_Sharpshooter_Female|BLOCKED_ART|5,6|Left5/6 왼쪽2px 이동. Left6 미세 분리 성분 제거.|5,6|Left5/6 파란 망토 끝의 원본 수평/수직 절단 형태가 왼쪽2px 이동 후 x=125에 남음. 경계는 떨어졌으나 망토 끝 소실은 미복구.|
|Vision_Sharpshooter_Male|BLOCKED_ART|5,6|Left5 왼쪽2px 이동. Left6 좌측34px 파란/검은 조각과 미세 잔여 제거.|5|Left5 파란 망토 끝의 원본 절단 단면이 왼쪽2px 이동 후 x=125에 남음. Left6 좌측 분리 조각은 해결.|

상세 셀별 Hash/Pixel Diff/이동량/제거 좌표/이동 제외 사유는 [JSON](Intellectual_Vision_CellCleanup_QA.json)에 기록한다. 원본/수정본/셀 이미지/Contact Sheet/전체 파일 시작 해시는 `Temp/PlayerSpriteCellCleanup`에 보존했다. 재현 Pipeline은 `Tools/PlayerSpriteCellCleanup.py`이며 게임 Runtime 코드가 아니다.

기존 meta·Sprite ID·Appearance ID·Import·Gender/Path/Job·Catalog Entry·Save Mapping을 유지한다. 다른42종 PNG/meta/Entry/QA/Sprite/Clip은 변경하지 않는다. Runtime 검증 결과는 후속 기록에 갱신한다. Foreground/실제 걷기·키보드/게임패드는 미검증이다. 다음 권장 작업은 잘리기 전 완전한 원본 파츠 확보 후 해당 문제 셀에 복원하는 것이다. GitHub Push 없음.

## 최종 검증

- PlayUnfocused86PASS/0FAIL: [Runtime 기록](Intellectual_Vision_CellCleanup_Runtime_Results.txt). 8종 Mapping/Job Preview, 대표 Intellectual Mage Male·Vision Mage Female CharacterCreation→Confirm Preview→World→Save→Bootstrap Continue→Battle Left Idle/실제 Battle Scene 전달 통과. BLOCKED는 기존 성별 기본 Sprite의 임시 fallback이며 수정본 Art의 실제 걷기 검증은 아니다.
- 대상8종 Import512×512/Multiple/16Sprite/PPU128/Point/Uncompressed 유지. 전체50 Entry·800 Sprite Missing0, Rect128×128/Pivot(64,0)/PPU128 오류0. Catalog와 Inventory 검수 이유 일치. 전체 meta/Sprite GUID/fileID 보존.
- 독립 검증에서 정상84셀 RGBA Hash/Pixel Diff0, 허용44셀 중41개만 수정, 전체87셀 유지. 다른42종 Inventory/QA/Matrix Entry와 Catalog의 대상8 reviewReason 외 전체 텍스트/embedded Clip 동일. Assets/ProjectSettings/UserData 시작 해시 비교에서 대상8PNG+Inventory+Catalog 총10파일만 변경. ZIP·사용자 Save/Settings 원본 동일.
- 최종 Console Error0/Warning0·현재 Compile Error0/isCompiling=false. C# 기능 코드 변경 없음·새 전체 C# 컴파일 미수행. Pipeline Python 구문 검사 통과. clean Bootstrap Edit Mode·is_focused=false, 격리 Save 경로null·Settings/Play 옵션/runInBackground/GameView 진입 설정 복원.
- Foreground/실제 걷기·키보드/게임패드 미검증. 직접 변경 파일 staged diff --check 확인 후 commit한다. 전체 Working Tree의 기존 사용자 whitespace는 보존하며 ZIP/다른42종/Voice/BGM/Quest는 stage하지 않는다. GitHub Push 없음.
