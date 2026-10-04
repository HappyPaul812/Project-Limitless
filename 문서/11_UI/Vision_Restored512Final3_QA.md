# Vision512 Final 복원3종 QA

입력 `F:\Downloads\Limitless_Vision_3_Restored_512_Final.zip`, SHA256 `278db42e44d3e58c564742d02fe743000f70df7f6b0d262703efe1029d6b0f98`. 문서화 선행 후 원본3PNG 바이트 교체. PNG3/중복0/누락0·모두512×512 RGBA·4×4·16Frame·Cell128. 직전1254 입력은 규격 불일치로 QA 보류됐으며 이번 판정은 새512원본의48Frame을 기준으로 한다.

2026-10-05 Vision512Final 새48Frame QA: 지정3종 READY3/BLOCKED0·전체 **READY30/BLOCKED20**. 다른47종 및 이전 READY27 보존. [최신 QA](Vision_Restored512Final3_QA.md). Runtime 검증 완료.

|Path|Job|Gender|최종 판정|남은 Frame|실제 복원/검수 결과|
|---|---|---|---|---|---|
|Vision|Fighter|Male|READY|없음|전체16Frame 방향 정상. Left5/7·Right9/11 검 끝이 뾰족한 닫힌 윤곽으로 복원되고 검/머리/망토/발이 셀 안에 포함됨. 절단 단면/외딴 조각/인접 셀 침범 없음.|
|Vision|Mage|Female|READY|없음|전체16Frame 정상. Up12-15 머리 상단·머리카락/장식·지팡이 꼭대기가 온전하며 수평 접합선/이중 봉우리/지팡이 이중 원/접합 어긋남 없음. 인접 셀 침범/잔여 조각 없음.|
|Vision|Sharpshooter|Male|READY|없음|전체16Frame Left/Right/Up 방향 정상. Up12/15 머리 끝 및 Up13 우측 망토 끝이 셀 안에서 닫힌 윤곽으로 복원됨. 검은 외딴 조각/경계 노출/인접 셀 침범 없음.|

Head/Hair/Accessory/Foot/Weapon/Cloak/Staff Crop0, Stray/Neighbor Intrusion/Character Swap/Empty/Direction Error0·심각한 Scale 변화/Alpha 손상0. 모든48셀 본체가 셀 안에 포함되고 Alpha>10 연결성분은 셀별1개. Frame은0-based. 이미지 변환/수정0. 기존3 ID/meta/48Sprite/Import 유지, 기존Entry에만24Clip 연결 완료. 다른47종/기존READY27 보호.

Runtime/검증 완료: 격리 PlayUnfocused **90PASS/0FAIL**, 대상3종 자동Mapping/Job 및 최종확인 Preview·성별 Character Creation Preview·World Sprite·Battle Left Idle/실제Battle Scene·Save→Bootstrap Continue 모두 통과. 조합 없는 오래된Save 기본성별 fallback·누락/불일치 AppearanceID 재계산 통과. 기존정식Save 구조 변경/새Migration 없음.

컴파일 완료·Error0, 기존 ExternalAssetImportEditor.cs53/59 CS0618 경고2건 보존. 최종 Console Error0/Warning0. clean Bootstrap Edit Mode·Save/Settings/Play 설정 복원·포커스 전환 없음. 실제 걷기/물리 키보드/게임패드 미검증.

보존검사 PASS: ZIP·원본3PNG 바이트 동일, 모든meta/48SpriteID·다른47PNG/Entry/QA 판정·기존216Clip 블록 동일. READY3 기존Entry에만24Clip 추가. Assets/ProjectSettings/UserData 변경은3PNG+Inventory+Catalog+QA helper 총6파일뿐이며 Save/Settings 불변. 사용자94항목/410Git 상태 보존. [Runtime 결과](Vision_Restored512Final3_Runtime_Results.txt)·[세부 QA](Vision_Restored512Final3_QA.json). 다른BLOCKED20은 이번 범위 밖으로 유지. 다음 권장작업은 남은20조합의 기존QA Art 수정이며 이번3종 추가 Art 수정 없음. GitHub Push 없음.
