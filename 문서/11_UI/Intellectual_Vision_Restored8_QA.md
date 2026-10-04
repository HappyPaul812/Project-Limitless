# Intellectual/Vision Restored8 QA

2026-10-05. [계획](Intellectual_Vision_Restored8_적용_계약.md). 입력 `F:\Downloads\Limitless_IntellectualVision_8_Restored.zip`, SHA256 `7adbeee03f0dffe44a54ab348edca3e3f1f9fa07b9449b7c65652b53f67d26d8`. PNG8/추가0/중복0/누락0, 모두512×512 RGBA·4×4·16Frame·Cell128px. 파일명으로 기존8 Definition에 연결(Vision→Visual, Sharpshooter→Marksman). PNG/ZIP 원본 바이트 그대로 반영, 이미지 수정/crop/이동/삭제/복원/생성 없음.

원본 전체128프레임 확대 관찰 및 Alpha 성분 검사. Empty0, 시트별 프레임 픽셀Hash 중복0, Character Swap/방향 오류/Scale 급변/심각한 본체 Alpha 손상 없음. 경계 접촉만으로 잘림을 단정하지 않았으며 실제 단면과 파츠 접합을 관찰했다. **READY승격5/BLOCKED_ART3, 작업 전22/28 → 작업 후27/23**.

Frame 번호는0-based, 좌표는 각128px 셀 왼쪽 위 기준이다.

|Sprite|이전 문제|최종 판정|남은 Frame|복원 결과/정확한 사유|
|---|---|---|---|---|
|Intellectual_Fighter_Female|Right 8–11 리본 끝의 기존 수평 절단 윤곽이 이동 후 y=2에 남음. Up 12–15 머리/리본 윗부분의 원본 절단 윤곽 잔존. 13/15는 발 Alpha를 보존하기 위해 전체 이동 제외.|READY|없음|Right 리본 끝·Up 머리/리본 상단 복원. 전체16프레임 본체·장비·방향 정상.|
|Intellectual_Mage_Female|Up 12–15 양쪽 리본/머리 상단의 기존 절단 윤곽 잔존. 13 아래1px 이동으로 경계 여백 확보했으나 원래 소실된 윤곽은 복구되지 않음. 12/14/15는 발을 새로 자르므로 이동 제외.|READY|없음|Up 머리/양쪽 리본 윤곽 복원. 전체16프레임 정상.|
|Intellectual_Mage_Male|Up 12–15 갈색 머리 끝의 원본 수평 절단 형태가 아래2px 이동 후 y=2에 남음. 경계 접촉은 해소됐으나 소실된 머리 끝 픽셀은 이동으로 복원 불가.|READY|없음|Up 머리카락 끝 복원. 전체16프레임 정상.|
|Vision_Fighter_Male|Down1/3, Left5/7, Right9/11, Up13 검·망토의 원본 잘린 끝 잔존. Left5는 좌우 폭 부족으로 안전 전체 이동 불가. Up12–14 머리 상단의 원본 절단 윤곽 잔존. 방향 정상.|BLOCKED_ART|5,7,9,11|Left5/7 검 끝의 넓은 절단 단면과 Right9/11 검 끝의 평평한 단면이 남음. 경계 안에 있어도 끝 윤곽 복원이 불완전함. Up 머리/망토 및 분리 조각은 개선됐고 Left/Right 방향 정상 유지.|
|Vision_Guardian_Male|Left5 망토 끝의 원본 잘린 윤곽 잔존. Right8–11 머리 끝과 Up12–15 머리 상단의 원본 절단 형태 잔존. 15는 새 발 경계 접촉을 막기 위해 이동 제외.|READY|없음|Left 망토·Right/Up 머리 윤곽 복원, Frame6 분리 조각 없음. 전체16프레임 정상.|
|Vision_Mage_Female|Up12–15 머리/지팡이 꼭대기의 원본 절단 단면이 아래2px 이동 후 y=2에 남음. 경계 접촉 해소와 소실된 디자인 복구는 구분함.|BLOCKED_ART|12-15|Up12–15 머리/지팡이 상단 복원 조각이 기존 머리/지팡이와 어긋남. 셀 상단 y≈17 부근에 수평 접합선, 머리 좌우 이중 봉우리와 지팡이 원의 이중/절단 형태가 생김. 동일 캐릭터 파츠 접합 문제이며 Character Swap은 아님.|
|Vision_Sharpshooter_Female|Left5/6 파란 망토 끝의 원본 수평/수직 절단 형태가 왼쪽2px 이동 후 x=125에 남음. 경계는 떨어졌으나 망토 끝 소실은 미복구.|READY|없음|Left5/6 망토 끝 닫힌 외곽 윤곽 복원. 전체16프레임에서 심각한 crop/조각/방향/Alpha 손상 없음. 경계의 미세 antialias와 정상 발바닥 선은 잘림으로 단정하지 않음.|
|Vision_Sharpshooter_Male|Left5 파란 망토 끝의 원본 절단 단면이 왼쪽2px 이동 후 x=125에 남음. Left6 좌측 분리 조각은 해결.|BLOCKED_ART|12,13,15|기존 Left5/6 망토 및 Frame6 분리 조각은 해결. 전체16Frame 재검수에서 Up12/15 머리 위 흰 머리카락 끝이 y=0에서 수평 절단(각11/10px Alpha>128). Up13 파란 망토 끝은 x=127 우측 경계에서 잘림.|

Vision Fighter Male 방향 정상 유지. Vision Mage Female은 같은 캐릭터 파츠의 접합 오류이며 다른 캐릭터 교체로 판정하지 않았다. Sharpshooter Male의 기존 Left 문제는 해결됐으나 전체 재검수에서 Up12/13/15 문제를 확인했다. 다른42종과 Intellectual Fighter Male READY, Hearing/Mobility/EmotionalScar의 판정은 유지한다.

원본8PNG·이전8PNG·시작 Assets/ProjectSettings/UserData 해시와 git 상태,128프레임 Contact Sheet/문제 확대 이미지는 `Temp/RestoredEightAudit`에 보존했다. [세부 QA JSON](Intellectual_Vision_Restored8_QA.json). 기존8 Appearance ID·128 Sprite GUID/fileID·meta·Import·자동Mapping/Save 구조를 유지한다. READY인 기존5 Entry에만 공용Controller용8 Clip씩 연결한다. 다른42종 Entry/Clip을 재생성하지 않는다.

Runtime 검증 기록은 후속 결과에 갱신한다. Foreground/실제 걷기·키보드/게임패드 미검증. 다음 권장 Art 작업은 BLOCKED3종의 표에 있는 문제 Frame 보정이다. GitHub Push 없음.

## 최종 검증

- [Runtime 기록](Intellectual_Vision_Restored8_Runtime_Results.txt)86PASS/0FAIL. 대상8 Mapping/Job Preview·READY/Blocked 상태 표시, 대표 Intellectual Mage Male·Vision Guardian Male 생성/Confirm Preview/World/Battle Left Idle/실제 Battle Scene/Save→Bootstrap Continue 및 구버전·불일치 ID 재계산 통과. 대표2종은 fallback이 아니라 READY Restored Sprite를 실제 참조한다. BLOCKED3종은 기존 기본 성별 임시 fallback을 유지한다.
- 50 Entry·800 Sprite Missing0, Rect128×128/Pivot(64,0)/PPU128 오류0, meta/Sprite ID 유지. 기존176 embedded Clip 블록 동일·READY5종에만40 Clip 추가(Down/Left/Right/Up Idle/Walk·6fps). 다른42 Entry/PNG/meta/Inventory/Matrix/QA 동일. Catalog의 기존8 Entry를 유지했으며 다른42종 Clip 재생성 없음.
- 시작 Assets/ProjectSettings/UserData 해시 비교: 8PNG+Inventory+Catalog+QA helper 총11파일만 변경. 사용자 Save/Settings·ZIP 원본 동일. 반영8PNG SHA256가 ZIP 추출 원본과 일치, 픽셀 수정 없음.
- QA helper의 대표 조합만 Intellectual Mage Male·Vision Guardian Male로 변경(주석 포함3줄), Runtime 기능 코드/Save Migration 변경 없음. Unity C# 컴파일/domain reload 완료·Compile Error0. 컴파일 직후 기존 ExternalAssetImportEditor.cs CS0618 경고2건, Runtime 종료 후 Console Error0/Warning0.
- clean Bootstrap Edit Mode·is_focused=false·Audit Save 경로null·Settings/Play 옵션/runInBackground/GameView 진입 설정 복원. OS 포커스 전환/Game View 활성화/물리 입력 없음. 실제 걷기·키보드/게임패드는 미검증.
- 직접 변경 파일 staged diff --check를 확인한 뒤 commit한다. 기존 사용자 Working Tree/whitespace 보존. GitHub Push 없음.
