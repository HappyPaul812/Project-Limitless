# Intellectual/Vision Cleanup v2 8종 QA

2026-10-05. [적용 계획](Intellectual_Vision_CleanupV2_적용_계약.md). 입력 `F:\Downloads\Limitless_IntellectualVision_8_Cleanup_v2.zip`, SHA256 `95d2299df6b09e69165bc31a51045fad691901be1df58b97aad2edde3e737822`. PNG8/추가0/중복0/누락0, 모두512×512 RGBA·4×4·16프레임·128×128 Cell. ZIP과 픽셀 변환 없이 원본 바이트를 반영했다. Vision/Sharpshooter는 기존 Visual/Marksman 명명에 대응하며 새 ID를 만들지 않았다.

8종 전체128프레임을 확대 관찰했다. 이전 문제 프레임을 먼저 확인하고 나머지 프레임도 검사했다. Alpha>0 및 >10 연결 성분으로 육안 결과를 교차 확인했다. 투명 여백이 생겨도 이미 잘린 머리/리본 윤곽은 복구된 것으로 판정하지 않았다. Alpha1–2만 남은 미세한 antialias를 단독 차단 사유로 삼지 않았다. Empty0, 시트별 프레임 Hash 중복0. 심각한 본체 내부 Alpha 손상·Character Swap·Scale 급변 없음. Vision Fighter Male Left/Right 방향 정상 유지. **READY 승격0 / BLOCKED_ART8 / 전체 Ready22·Blocked28**.

프레임 번호는 모두0-based, 좌표는 각128px Cell의 왼쪽 위가(0,0)이다. 경계 접촉은 단독 crop 증거가 아니며 실제로 잘린 끝과 수평 절단 윤곽을 함께 확인했다.

|Path|Job|Gender|이전 문제|해결 및 변화|판정|남은 Frame|위치·사유|
|---|---|---|---|---|---|---|---|
|Intellectual|fighter|Female|Left 4–7 발 아래 조각, Right 8–11 큰 머리 조각, Up 12–15 머리 상단 수평 절단.|Left 발 아래 및 Right 분리 머리 조각 제거. Up 본체 상단 절단은 미해결.|BLOCKED_ART|8-15|Right 8–11 머리 리본 끝이 셀 상단 y=0에서 잘림. Up 12–15 머리카락·리본 상단에 수평 절단 형태가 남음.|
|Intellectual|mage|Female|Right 8–11 큰 머리 조각, Up 12–15 머리 상단 수평 절단.|Right 8–11 분리 머리 조각 제거. Up 리본·머리 절단 미해결.|BLOCKED_ART|12-15|Up 12–15 양쪽 파란 리본·머리 상단에 수평 절단 형태가 남음. 투명 여백 안쪽으로 옮겨졌으나 원래 잘린 윤곽이 복구되지 않음.|
|Intellectual|mage|Male|Right 8–11 셀 하단 갈색 조각 17/23/18/21px가 몸체와 분리되어 남음.|Right 8–11 셀 하단 갈색 분리 조각 제거. 전체 재검수에서 Up 머리 끝 crop 확인.|BLOCKED_ART|12-15|Up 12–15 머리 위 솟은 갈색 머리카락 끝이 셀 상단 y=0에서 수평으로 잘림.|
|Vision|fighter|Male|Left 4–7 머리 위 검은 조각, Right 8–10 발 아래 조각. Left 방향은 정상.|기존 Left 머리 위·Right 발 아래 조각 제거. Left/Right 방향 정상 유지. 다른 셀의 검·망토 crop 및 인접 검 조각 잔존.|BLOCKED_ART|1-3,5,7,9,11-14|Down 1–3/Left 5,7/Right 9,11 검·망토가 좌우 셀 경계에서 잘림. Down 2 오른쪽 x=125–127,y=109–118에 23px 잔여 조각(max Alpha36). Up 12–14 머리 상단 crop, 14 왼쪽 x=0–12,y=82–102에 인접 검 분리 조각169px.|
|Vision|guardian|Male|Right 8–11 큰 머리 조각, Up 12–15 머리 상단 수평 절단.|Right 8–11 큰 분리 머리 조각 제거. Up 잘린 머리 윤곽은 미복구. Left 작은 잔여 조각도 확인.|BLOCKED_ART|5,6,8-15|Left 5 망토 우측 경계 crop, 6 왼쪽 x=0–1,y=77–83에 10px 분리 조각(max Alpha155). Right 8–11 머리 끝 상단 crop, 9 망토 우측 crop. Up 12–15 머리카락 윗부분에 수평 절단 형태가 남음.|
|Vision|mage|Female|Right 8–11 셀 하단 분리 조각. 8/10 파란 조각과 회색 선, 9/11 작은 잔여 픽셀.|Right 8–11 파란 조각·회색 선 제거. Up 상단 crop 확인.|BLOCKED_ART|12-15|Up 12–15 머리와 지팡이 꼭대기가 셀 상단 y=0에서 수평으로 잘림. 저Alpha 잔여물은 Left6 발 아래29px(max Alpha2), Up12 오른쪽8px(max Alpha2)도 존재.|
|Vision|sharpshooter|Female|Left 4–7 머리 위 셀 상단 검은 조각 19/24/23/26px.|Left 4–7 머리 주변 검은 분리 조각 제거. Left5,6 망토 우측 crop 확인.|BLOCKED_ART|5,6|Left 5,6 뒤쪽 파란 망토 끝이 셀 우측 x=127에서 잘림. 머리 위 경계 접촉만으로 crop 판정하지 않았으며, 기존 검은 머리 분리 조각은 제거됨.|
|Vision|sharpshooter|Male|Right 8/11 셀 최하단 10px 가로 조각이 다음 Up 행 머리 위에 나타남.|Right 8/11 최하단 가로 조각 제거. Left6 인접 망토 잔여 조각 확인.|BLOCKED_ART|5,6|Left 5 망토 우측 셀 경계 crop. Left 6 왼쪽 x=0–5,y=77–88에 34px 파란/검은 분리 조각(max Alpha248).|

다른42종 PNG/meta·Catalog Entry·Sprite/Clip·Inventory/QA/Matrix 판정을 보존했다. 특히 Intellectual Fighter Male READY와 Hearing/Mobility 변경 없음. 대상8종 Stable Appearance ID, 128 Sprite GUID/fileID, meta, Import Multiple/PPU128/Point/Uncompressed/Rect128/Pivot(64,0), 자동 Mapping/Save 구조를 유지한다. 기존42종에 대한 재판정이나 Runtime 기능 코드 변경 없음.

원본8PNG와 이전8PNG, 전체Assets 시작 해시는 `Temp/CleanupV2Audit/Original`, `Previous`, `baseline.json`에 보존했다. 전체128프레임 확대 Contact Sheet와 문제 머리 확대 이미지도 같은 Temp 폴더에 있다. 세부 Alpha 성분과 프레임 해시는 [기계 판독 QA](Intellectual_Vision_CleanupV2_QA.json)에 기록했다. 이전 Cleanup QA는 이력으로 유지한다.

Runtime·최종 보존 검증은 아래 후속 검증 기록에 갱신한다. Foreground/실제 걷기·키보드/게임패드 입력은 미검증이다. 다음 권장 작업은 위 crop 윤곽 복구와 남은 조각의 원본 미술 수정이다. GitHub Push 없음.

## 최종 검증

관련 commit: 문서 선행 `4c9563a`, 원본 반영·개별 QA·Runtime 기록 `501c0c8`.

- [Runtime 기록](Intellectual_Vision_CleanupV2_Runtime_Results.txt): PlayUnfocused86PASS/0FAIL. 8종 자동 Mapping/Job Preview, 대표 Intellectual Mage Male·Vision Mage Female 생성/Confirm Preview/World/Battle Left Idle/실제 Battle Scene/Save→Bootstrap Continue 및 구버전·불일치 Appearance 재계산 통과. BLOCKED 기본 성별 fallback 동작 검증이며 수정 Art의 실제 걷기 재생 검증은 아니다.
- 전체50 Entry·800 Sprite 참조 누락0, Rect128×128/Pivot(64,0)/PPU128 오류0. 8종 각각16 Sprite·512×512·Multiple/Point/Uncompressed 유지. meta 전부 동일하여128 Sprite GUID/fileID 유지. Catalog 전체에서 지정8 reviewReason을 제외한 텍스트 동일: 다른42 Entry 및 embedded Clip 바이트/참조 보존. Inventory/Matrix/QA의 다른42 Entry 동일.
- 시작 Assets 전체 해시와 비교해8PNG+Catalog+Inventory 총10파일만 변경. ZIP SHA256 및 반영8PNG 원본 바이트 동일. 사용자 기존 Working Tree 변경 유지.
- 첫 검증 중 Catalog/Inventory의 외부 파일 변경 시각과 SourceAssetDB가 불일치하여 Worker Import Error Code4가2건 발생했다. 두 Asset 동기 Import로 정합성을 복구한 뒤 Console 이력을 비우고86개 Runtime 전체를 재실행했다. 재실행·종료 후 Console Error0/Warning0, 참조/Import 정상. C# 변경 없음·새 전체 C# 컴파일 미수행·isCompiling=false, 현재 Compile Error0. 기존 deprecated 경고는 이전 이력으로 유지한다.
- clean Bootstrap Edit Mode 복귀, is_focused=false, Audit Save 경로null, 원래 Settings 경로·Play 옵션·runInBackground·GameView Play 진입 설정 복원 확인. OS 입력/포커스 전환 없음. 실제 걷기·키보드/게임패드 입력은 미검증.
- 직접 변경 범위 diff --check 통과. 전체 diff에는 기존 사용자 변경의 whitespace가 남아 있다. GitHub Push 없음.
