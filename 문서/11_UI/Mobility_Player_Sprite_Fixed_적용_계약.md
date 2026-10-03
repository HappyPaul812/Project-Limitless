# Mobility Player Sprite 수정판 적용 계약

2026-10-03 사용자 요청. Mobility × 5Job × Male/Female 10종만 제공된 수정판으로 교체한다. Gender+Path+Job 자동 매핑·기존 Appearance ID·Catalog Entry·Save 구조를 유지한다. 다른40종과 Hearing QA, Voice/BGM/Main16은 변경하지 않는다.

## 입력

원본 `F:/Downloads/Limitless_Mobility_Path_Sprites_Fixed.zip`, SHA256 `a7dbb1f962a6ecc2c9443c8aad8a198a31bf941161dfdcbde614e99e2a426133`. 정식 Physical PNG10개와 참고 Preview.png1개, README.txt·VALIDATION.csv가 있다. Preview는 캐릭터 시트 수에 포함하거나 Unity에 등록하지 않는다. Male5/Female5, Fighter/Guardian/Healer/Mage/Marksman 각각 남녀1개, Duplicate0/Missing0. LOCAL `03_Sharpshooter.asset`의 Job ID는 `sharpshooter`; 파일·Appearance ID의 marksman 명칭은 유지한다.

정식10개는512×512 RGBA, Alpha0–255, 4×4/128px Cell/16Frame이다. 별도 변환 없이 ZIP의 PNG 바이트 그대로 기존 Mobility 경로에 반영한다. 원본 ZIP은 보존한다. 미술 재생성·복원·재배치·Alpha 보정·장치 추가를 하지 않는다.

## 적용과 보존

기존 `Assets/_Project/Art/Characters/Player/Validated50/Mobility/Physical_{Job}_{Gender}.png`만 교체한다. meta GUID·Sprite16 참조·PPU·Pivot·Filter·Compression·Slice를 유지한다. 기존 `appearance.external.v1.mobility.{job}.{gender}` 10개를 유지하며 새 ID를 만들지 않는다. Catalog와 Inventory는 Mobility QA/원본 추적 정보만 갱신한다. 전체50종 Build/Import 도구는 실행하지 않는다.

## 검증

문서→구현→검증 순서다. 160Frame의 Empty/Duplicate/Crop/분리 조각/Alpha/Character Swap/방향/Scale를 실제 관찰한다. CSV 경계 접촉0만으로 PASS하지 않는다. 기존 휠체어 디자인 요소를 확인한다. 미술 Blocked에는 기존 기본 성별 임시 fallback을 유지한다.

Unity Import160·Stable ID·Catalog 참조·10조합 실제 Path/Job Preview 및 대표 남녀 생성/World/Save→Bootstrap Continue/Battle를 격리 Save/Settings와 PlayUnfocused로 검증한다. 다른40종 PNG/meta Hash와 Catalog Entry/Sprite/Clip/QA 상태를 시작값과 비교한다. 실제 확인 결과로 Ready/Blocked를 집계한다. Foreground·물리 입력은 이번 승인 범위에 없으며, 필요한 경우 별도 승인을 요청한다. 관련 파일만 commit하고 GitHub Push하지 않는다.
