# Vision 최종 복원3종 QA — 형식 불일치로 반영 보류

2026-10-05. 문서화 선행 commit `e58f432` 이후 ZIP 형식 검사에서 차단. 입력 `F:/Downloads/Limitless_Vision_3_Restored_Final.zip`, SHA256 `283bdf5511d9dc1beb2c104f8ed563b4207e3908ce443ec83e6f00f080070460`.

목적은 Vision Fighter Male·Mage Female·Sharpshooter Male의 잘린 검/머리/지팡이/망토 복원본 적용과 READY3승격이었다. 기대 전체 READY30/BLOCKED20은 달성되지 않았으며 실제 전체 **READY27/BLOCKED23 유지**, 이번 대상 **READY0/BLOCKED_ART3 유지**이다.

## 형식 및 적용 결과

- ZIP 정확히 지정3PNG, 중복0/누락0, RGBA 모두 확인. 원본3개는 `Temp/VisionFinal3Audit/Original`에 바이트 그대로 추출했다.
- **모두1254×1254**로 필수512×512 규격 실패. 1254÷4=313.5로 균등 정수4×4 셀을 구성할 수 없고 기존16개의128px Sprite rect에 맞지 않는다. 기존 meta를 유지한 PNG 교체만으로 사용할 수 없는 입력이다.
- 게임 PNG 교체0, Inventory/Catalog/Mapping/Clip/Sprite/meta/import 수정0. 임의 리사이즈/재슬라이스/ID 변경/placeholder 생성 없음. 다른47종과 기존 READY27조합을 포함하여 모든50종 자산 보존.
- 전체 시트 관찰용 미리보기에서는 복원 파츠 형태를 확인했으나, 규격이 맞지 않아 정식128px 셀48Frame QA(검 끝/방향/접합선/조각/경계/침범)와 READY 승격 판정은 보류했다. 아래 Frame은 새 입력의 결함 확정이 아닌 기존 적용본의 잔존 이력이다.

|Path|Job|Gender|최종 판정|남은 문제 Frame|남은 사유|
|---|---|---|---|---|---|
|Vision|Fighter|Male|BLOCKED_ART|5,7,9,11 (기존)|입력1254×1254 규격 불일치. 기존 아트 사유 유지: Left5/7 검 끝의 넓은 절단 단면과 Right9/11 검 끝의 평평한 단면이 남음. 경계 안에 있어도 끝 윤곽 복원이 불완전함. Up 머리/망토 및 분리 조각은 개선됐고 Left/Right 방향 정상 유지.|
|Vision|Mage|Female|BLOCKED_ART|12-15 (기존)|입력1254×1254 규격 불일치. 기존 아트 사유 유지: Up12–15 머리/지팡이 상단 복원 조각이 기존 머리/지팡이와 어긋남. 셀 상단 y≈17 부근에 수평 접합선, 머리 좌우 이중 봉우리와 지팡이 원의 이중/절단 형태가 생김. 동일 캐릭터 파츠 접합 문제이며 Character Swap은 아님.|
|Vision|Sharpshooter|Male|BLOCKED_ART|12,13,15 (기존)|입력1254×1254 규격 불일치. 기존 아트 사유 유지: 기존 Left5/6 망토 및 Frame6 분리 조각은 해결. 전체16Frame 재검수에서 Up12/15 머리 위 흰 머리카락 끝이 y=0에서 수평 절단(각11/10px Alpha>128). Up13 파란 망토 끝은 x=127 우측 경계에서 잘림.|

## 참조·Runtime·컴파일

Unity MCP 읽기 검증: 기존3종512×512, Multiple/PPU128/Point/Uncompressed, 각16Sprite rect128×128, 대상 Missing0/전체800Sprite Missing0. 기존3 Appearance ID와48Sprite ID/meta·기존216Clip 보존. Inventory/Catalog/Mapping/Preview 연결 및 기본 fallback 코드의 바이트 변경0.

새 입력 미반영으로 새 복원본 Preview/World/Battle Left Idle/Save→Continue Play 검증은 수행하지 않았다. 직전 Restored8의86PASS는 역사 기록으로만 유지한다. C# 변경0/새 전체 컴파일 미수행, 현재 컴파일 중 아님·Console Error0/Warning0. 직전 컴파일의 기존 CS0618 2건(TextureImporter.spritesheet)은 범위 밖으로 보존. clean Bootstrap Edit Mode·dirty false·포커스 전환 없음.

시작 Assets/ProjectSettings/UserData 해시와 최종 비교: 변경0, 모든meta·Save/Settings 보존. 사용자 기존94항목(개별 파일410 Git 상태) 보존. 직접 문서 변경 diff --check 통과, 전체 working tree diff --check는 기존 사용자 파일 trailing whitespace로 실패하며 정리하지 않았다. GitHub Push 없음.

다음 필요한 입력: 동일3종의512×512 RGBA·4×4·Cell128×128 원본 ZIP. 수신하면 문서화 → 지정3PNG 교체 →48Frame/참조/Runtime 재검증을 이어간다. [계획](Vision_RestoredFinal3_적용_계약.md)·[세부 JSON](Vision_RestoredFinal3_QA.json).
