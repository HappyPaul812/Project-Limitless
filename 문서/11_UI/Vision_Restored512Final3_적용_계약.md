# Vision 복원 최종3종 적용 계획

2026-10-05 Vision Final3 적용 준비: 입력 `F:/Downloads/Limitless_Vision_3_Restored_512_Final.zip`. Vision Fighter Male·Mage Female·Sharpshooter Male의 잘린 파츠 복원본만 반영한다. 현재 전체 READY27/BLOCKED23, 기대 READY30/BLOCKED20은 검증 전 미확정. 다른47종 자산·조합 및 기존 READY는 수정 범위 밖으로 보존한다. [계획](Vision_Restored512Final3_적용_계약.md)·[QA](Vision_Restored512Final3_QA.md).

문서화 → 구현 반영 → 검증 순서. ZIP에 정확히3PNG/중복0/누락0인지 확인하고 원본 바이트만 기존 Visual_Fighter_Male·Visual_Mage_Female·Visual_Marksman_Male 경로로 교체한다. 기존 Appearance/Sprite ID·meta·Import·512×512 RGBA·4×4·16Frame·Cell128 유지. 이미지 재가공·placeholder 생성 없음.

- Vision_Fighter_Male.png: 이전 BLOCKED_ART, Frame 5,7,9,11 — Left5/7 검 끝의 넓은 절단 단면과 Right9/11 검 끝의 평평한 단면이 남음. 경계 안에 있어도 끝 윤곽 복원이 불완전함. Up 머리/망토 및 분리 조각은 개선됐고 Left/Right 방향 정상 유지.
- Vision_Mage_Female.png: 이전 BLOCKED_ART, Frame 12-15 — Up12–15 머리/지팡이 상단 복원 조각이 기존 머리/지팡이와 어긋남. 셀 상단 y≈17 부근에 수평 접합선, 머리 좌우 이중 봉우리와 지팡이 원의 이중/절단 형태가 생김. 동일 캐릭터 파츠 접합 문제이며 Character Swap은 아님.
- Vision_Sharpshooter_Male.png: 이전 BLOCKED_ART, Frame 12,13,15 — 기존 Left5/6 망토 및 Frame6 분리 조각은 해결. 전체16Frame 재검수에서 Up12/15 머리 위 흰 머리카락 끝이 y=0에서 수평 절단(각11/10px Alpha>128). Up13 파란 망토 끝은 x=127 우측 경계에서 잘림.

직전1254×1254 ZIP은 규격 불일치로 신규QA/반영이 보류됐다. 이번512Final 원본은 새48Frame 실제QA로 판정하며 이전 BLOCKED 사유를 새 판정에 복사하지 않는다.

모든48Frame을 관찰하여 검 끝/머리/지팡이/망토 복원, 접합선, 경계/외딴 조각/인접 셀 침범 및 방향을 검증한다. 실제 통과 대상만 READY 승격한다. 각 기존 Catalog Entry 검수 필드와 필요한 Idle/Walk Clip 연결만 갱신하며 전체 Catalog 재생성은 하지 않는다. Inventory/Mapping/Preview/World/Battle Left Idle/Save→Continue 및 기본 성별 fallback 규칙 유지. 다른47종 PNG/meta/Entry/기존216 Clip·사용자 변경94항목을 해시와 Git 상태로 보존 확인한다.

Unity clean Bootstrap 상태·격리 Save/Settings·PlayUnfocused 백그라운드 감사, 컴파일/Console 및 종료 시 설정 복원 검증. Foreground/Game View 활성화·물리 입력·GitHub Push 없음. 실제 걷기/물리 입력은 미검증으로 남긴다. 관련 문서와 직접 변경만 명시적으로 stage/commit한다.
