# Hearing Player Sprite 수정판 적용 계약

2026-10-03 사용자 요청: Hearing Path × 5Job × Male/Female 10종만 교체한다. Gender+Path+Job 자동 매핑, 기존 Appearance Stable ID, Save 구조를 유지한다. 다른40종 및 BGM/Voice/Main16은 변경하지 않는다.

## 입력 및 승인

원본 `F:/Downloads/Limitless_Hearing_Path_Sprites_Fixed.zip`, SHA256 `5ab8b684deeca852c1bed310f3da194608da7567d02d3873ec672910db2c1ee3`. PNG10개·README1개, Male5/Female5, Guardian/Healer/Marksman/Fighter/Mage 각각 남여1개, Duplicate0/Missing0. Marksman은 LOCAL Job ID `sharpshooter`이며 기존 Appearance ID의 marksman 명칭은 유지한다.

입력10개는1254×1254 RGBA다. 사용자가 원본을 보존한 별도512×512 변환본 제작을 명시적으로 승인했다. 원본 ZIP/PNG를 수정하지 않는다. 전체 시트를 최근접 보간으로512에 동일비율 축소하여 배치를 유지한다. 프레임 재배치·그림 복원·재생성·알파 보정·장치 추가를 하지 않는다. 4×4/128px 규격을 맞춰도 원본의 미술 문제까지 해결됐다고 간주하지 않는다.

## 적용

변환본만 기존 `Assets/_Project/Art/Characters/Player/Validated50/Hearing/`의 동일 파일명에 반영한다. 기존 meta GUID·Sprite Rect/Pivot/PPU/Filter/Compression과160 Sprite ID를 유지한다. Catalog Entry의Identity/매핑은 그대로이며 QA 상태만 실제 검사 결과로 갱신한다. 다른40종 Entry/Sprite/Clip/QA는 보존한다. 전체 Build/Import 도구는 실행하지 않는다.

## 검증

원본→변환본 SHA/해상도/Alpha/셀 내용·중복 프레임·분리 조각·Crop·방향·Character Swap·Scale를 검사한다. 경계 접촉만으로 판정하지 않고 실제 프레임을 관찰한다. 미술 QA와 Mapping/Runtime 참조 QA는 분리한다. Blocked에는 기본 성별 임시 fallback 정책을 유지한다.

Import160 Sprite·Stable ID·Catalog 참조,10조합 Preview/World/Battle용 참조와 대표 남여 Save→Continue를 격리 환경에서 확인한다. Ready/Blocked는 실제 결과로 집계하고 Matrix/CURRENT_STATUS를 갱신한다. 다른40종 Hash/참조·QA를 시작값과 비교한다. Foreground 승인은 포함되지 않으며 필요하면 별도 요청한다. GitHub Push 없음.
