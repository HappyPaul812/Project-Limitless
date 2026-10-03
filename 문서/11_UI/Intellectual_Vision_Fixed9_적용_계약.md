# Intellectual4 / Vision5 수정판 적용 계약

2026-10-03. 정확히9종만 교체한다. Gender+Path+Job 자동 매핑·Appearance Stable ID·Catalog Entry·Save 정본과 스키마를 보존한다. 새 ID·수동 외형 UI·Migration 없음. 다른41종 PNG/meta/참조/QA·Hearing/Mobility 상태·Voice/BGM/Main16은 변경하지 않는다.

## 입력과 승인

- `F:/Downloads/Limitless_Intellectual_4_Fixed.zip`: SHA256 `7136b0069e6c405614b573ca43a13b593808d0bcdcd4c3d613622bee1b0814db`. Intellectual Fighter/Mage 남녀4종,512×512 RGBA·4×4/128px/16Frame, README/CSV 있음.
- `F:/Downloads/Limitless_Visual_5_Fixed.zip`: SHA256 `feaafcda0a5d9722348d864a9293f2a0b748b743bbb80ea1d26738f93c3d1bd8`. Vision Fighter Male/Guardian Male/Mage Female/Marksman Female/Male5종,1254×1254 RGBA, README 있음. 사용자가 이번 팩의 별도512×512 최근접 변환본 제작·적용을 명시적으로 승인했다. 전체 시트 동일 비율 축소만 수행하며 Frame 재배치/그림 복원/재생성/Alpha 보정 없음. 원본 ZIP/추출 PNG 보존.

파일명·README·LOCAL Inventory/Matrix로 대응했으며 모두 기존 BLOCKED_ART 대상이다. Duplicate0/Missing0. Marksman은 LOCAL `sharpshooter`다.

|Path|Gender|Job|기존 Appearance ID|수정판 파일|
|---|---|---|---|---|
|Intellectual|Female|fighter|appearance.external.v1.intellectual.fighter.female|Intellectual_Fighter_Female.png|
|Intellectual|Male|fighter|appearance.external.v1.intellectual.fighter.male|Intellectual_Fighter_Male.png|
|Intellectual|Female|mage|appearance.external.v1.intellectual.mage.female|Intellectual_Mage_Female.png|
|Intellectual|Male|mage|appearance.external.v1.intellectual.mage.male|Intellectual_Mage_Male.png|
|Vision|Male|fighter|appearance.external.v1.vision.fighter.male|Visual_Fighter_Male.png|
|Vision|Male|guardian|appearance.external.v1.vision.guardian.male|Visual_Guardian_Male.png|
|Vision|Female|mage|appearance.external.v1.vision.mage.female|Visual_Mage_Female.png|
|Vision|Female|sharpshooter|appearance.external.v1.vision.marksman.female|Visual_Marksman_Female.png|
|Vision|Male|sharpshooter|appearance.external.v1.vision.marksman.male|Visual_Marksman_Male.png|

## 순서와 검증

문서→구현→검증. 정식 PNG 경로9개만 교체하고 기존 Multiple/PPU128/Point/Uncompressed/Pivot(64,0)/4×4 Slice·144 Sprite GUID/fileID를 유지한다. 전체50 Build/Import 금지. QA 결과에 따라 대상9개 Entry만 갱신한다. Blocked에는 기존 기본 성별 임시 fallback을 유지한다.

144Frame 실제 관찰(Empty/Duplicate/방향/잘림/분리 조각/Alpha/Swap/Scale),9조합 실제 Preview/자동 Mapping, Intellectual/Vision 대표 World/Battle/Save→Continue를 격리 PlayUnfocused에서 확인한다. 시작 Hash/참조/QA와 다른41종을 비교하고 전체 Ready/Blocked를 집계한다. Foreground/물리 입력은 별도 승인 없이 하지 않는다. 관련 파일만 commit, GitHub Push 없음.
