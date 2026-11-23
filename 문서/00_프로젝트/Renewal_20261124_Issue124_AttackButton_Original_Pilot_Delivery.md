# LIMITLESS / Chapter 1·2 리뉴얼 아트 / Issue #124 / Attack Button 독립 아이콘 파일럿
작성일 2026-11-24 / 담당 브랜치 `docs/art-renewal`

## 최신 발주 근거
- Issue #124 2026-11-24 총괄 기획의 공격 전용 아이콘 **딱 1종** 발주: https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7115939762
- 기존 실제 Kenney BattleUiIconCatalog 기본 Attack(sword) 기능 유지. 소드/공격 행동을 확실하게 보이게 하되 Kenney 기존 PNG를 리터치·트레이스·단순 변형하지 않음.
- 제작 방향: 오른쪽 위로 향하는 은빛 검날·금색 손잡이·붉은 타격 궤적. 붉은 참격은 시각적 액션 표시일 뿐 화상/불속성 또는 신규 시스템 확정이 아님. 버튼 배경/테두리는 신규 PNG에 포함하지 않고 투명 아트만 제작.

## 실제 산출물
- `Art/AttackButton_Original_128_RGBA.png`: **128×128 투명 RGBA** 1종, 네 모서리 alpha=0, SHA256 `cf775727e27be825809a6b3d9bd04b227abe3408055873f70616cc8a9ca19a32`.
- `Art/AttackButton_Original_Master512_RGBA.png`: 동일 이미지 512 마스터(출처 기록/보존용), 별도 2번째 신규 아이콘이 아님.
- `Review/AttackButton_Actual_40px_RGBA.png` 및 `Review/AttackButton_Actual_30px_RGBA.png`: 128 원화에서 LANCZOS 실제 축소해 만든 두 개의 투명 프리뷰.
- `Review/Attack_128_40_30_Actual_UI_Comparison.png`: 128 원화와 40/30 실제 px 다크/라이트 가상 버튼 비교, 근접확대 구분.
- `Review/Attack_40_30_Actual_and_NearestZoom.png`: 40/30 실제 PNG 및 nearest 확대판.
- `Docs/Generated_Art_Source_1254_RGBA.png`: 생성 이미지 원천 바이트(기존 Kenney 이미지 아님). `Docs/README.md` 사용/보호/검증 한계 기재.
- `Manifest.json`, `SHA256SUMS.txt` (자기해시 순환 참조 없음).
- ZIP `LIMITLESS_Issue124_AttackButton_Original_20261124.zip`: **1,659,563 bytes**, SHA256 `14aeea5420912bbfc4c996f108a96db36058f34433324e02321c5de32b1ba9c7`, **10 files**, ZIP CRC/testzip PASS, Manifest payload **8/8 SHA256 + 파일크기 PASS**. 실제 위치 `/mnt/data/LIMITLESS_Issue124_AttackButton_Original_20261124.zip`. 이 대화에 다운로드 파일로 납품하며 대표님 Windows F드라이브 또는 GitHub에 PNG/ZIP 바이너리를 자동 업로드한 것은 아님.

## 검증과 한계
- PNG `RGBA`, 정확히 128×128, alpha extrema=0..255, 모서리 alpha0 PASS. 40/30 크기 Preview와 다크/라이트 비교 PASS(오프라인).
- 기본 전투 공격 UI는 검 아이콘을 호출한다는 이전 정적 감사와 Issue #124 최신 발주에 부합. **이번 실행환경에서 현재 Kenney 원본 PNG/실제 Unity UI GameView 1:1 픽셀 비교는 NOT_VERIFIED.** 기존 출처 기능만 참고했으며 Kenney 픽셀 원본을 생성·복제·수정본에 사용하지 않음. 외부 원본/라이선스 파일은 기존 상태 유지.
- 30px에서 검 실루엣은 알아볼 수 있으나 붉은 참격의 작은 분기와 금색 손잡이의 세부는 제한적. 대표 시각 승인 및 실제 Unity 게임 버튼 배율/배경/선택상태/접근성 30·40px 검증이 필요. 오프라인 파일 QA가 실제 사용 승인이나 30px GameView PASS는 아님.
- **원본 보호:** 기존 독침벌 Organic v2, 초원 슬라임 젤, 승인 Trait 5종, PathSymbol 5종, 공식 몬스터·펫·NPC·환경, Kenney 원본/라이선스/기존 버튼 코드, Unity6000.6.5f1, Issue #85, Scene/Prefab/GUID/meta/ItemDefinition/Save/UserData/Discord/main 모두 변경0. 신규 UI 적용·main 직접 커밋/병합/Push0.
- 상태: `ATTACK_ICON_1_ORIGINAL_CANDIDATE_DELIVERED / FILE_TECH_PASS / USER_ART_APPROVAL_PENDING / GAMEVIEW_NOT_VERIFIED / NOT_UNITY_APPLIED`.
