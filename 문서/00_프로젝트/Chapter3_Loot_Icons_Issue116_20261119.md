# LIMITLESS / Chapter3 전리품 6종 게임용 아이콘 후보 — 2026-11-19

- Issue [#116](https://github.com/HappyPaul812/Project-Limitless/issues/116), [기획 정본](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/08_몬스터/Chapter3_북부몬스터7종_전투밸런스_20261118.md)
- 담당 docs/art-new. `ART_CANDIDATE_6_OF_6 / OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED`.
- 선행 #116 댓글0/인계·Library 및 GitHub Loot_FrostVole/Loot_BlizzardWisp 검색에서 기존 완료 Ch3 아이콘 세트 미발견. Windows F: 전수 검색 미실행. 이번 대화의 생성된 개별 시안 6장을 사용, 중복 재생성0. 별도 생성 리뷰보드 납품 사용0.

## 실제 PNG 6종
모두 독립 128×128 투명 RGBA / alpha 0,255 / 가장자리 alpha0 / 2×2 픽셀 격자 / 17 가시색 / 파일 중복0. 파일명은 아트 후보이며 ItemId 확정 아님.

|전리품|PNG (Icons/)|SHA256|
|---|---|---|
|서리 묻은 털뭉치|Loot_FrostVole_FrostedFur.png|356bdef577ee85d6a609c5476601821d07b3e49a4a475b5c1f41b05c12bea704|
|설원 솜털|Loot_SnowStoat_SnowDown.png|2dca5ba78a8e1da51f40492ccb209c41f2046914d4c831d6cb8e7296ce0a43fc|
|서리깃|Loot_FrostOwl_FrostFeather.png|6be6ac2502cb7476dbff11d52514148d99773a4e61272e2fb06ddf4935c3832e|
|빙갑 조각|Loot_IceBeetle_IceCarapace.png|371c5fbc50d1682de7a35a3c116000b30e5b0aeb65839b098156b8968d688853|
|눈폭풍 결정|Loot_BlizzardWisp_BlizzardCrystal.png|16b100a6e109cfebd5ba3ed7d2dc2729018dcc3b47153a82c6e8e48854103b25|
|마력차단 결정|Loot_RimeSentinel_MagicBlockingCrystal.png|0978353c3562b527072a70b907d06ae74985fa9b0f9e4b8ec7cd334673da25a8|

## 실제 화풍 비교
- [Ch2 기존 구현 기록](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/00_프로젝트/Chapter2_Loot_Items_20261009.md)의 SHA와 일치하는 main ItemIcons 실제 PNG 3종 확보: 열풍 깃털 9b25288bac730fa2453b0e9f0bb61caae2a84c77372095628e72647a9f8ea332, 화열 갑각 bedbe891d1ed0b7d144533003887879ef8d8cabe82c7e0af19e4fc795519e326, 잔불 파편 e1e3ee1ac24cb8d78e95c30470ac8a3c841b2be2fab11085671a20ebc81bb617. 3종 실제 2×2 동일 픽셀 블록, 12~16 가시색.
- 신규 시안 6종 알파 정리 후 64px 아트 격자 내 긴 변 50px, 가시색16 양자화+어두운 1px 윤곽, nearest 2배로 128 정규화. 원래 시안 바이트 보존. 신규 내부 디테일은 기존보다 많으므로 화풍 일치의 최종 판단은 사용자 미술 검토. 실제 128/40px 비교판, Ch2 3종과 신규 대응 3종 실물 비교판, 40px 투명 스트립. Ch2 모든 7종 또는 인게임 UI 검증 아님. Ch2 원본 변경0.

## 납품/검증
- `Chapter3_Loot_Issue116.zip` 175,145 bytes/12 entries, SHA256 `8d7ff5e8c3c16aebe9d2992ace2326002e8e72946d62c1b0faad37a251acb54c`.
- CRC PASS, Manifest 10/10 payload 크기·SHA 일치, SHA256SUMS 11/11 PASS, 비순환. SourceDeclaration 포함. Library `/LIMITLESS/Issue116_Chapter3_Loot/` ZIP·PNG6·비교판2 저장 성공, Library ZIP 재회수 원본 byte-exact/CRC PASS.
- 신규 후보 미술 승인 필요 YES. Unity 적용/ItemId/드롭/판매가/펫/기존 승인 자산/Save/UserData/Discord 변경0. #112 눈보라 정령 원화 승인 대기·추가 모션0. #85 HOLD 유지. main 직접 commit/Push/Merge0. 사용자 PC 복사 및 GitHub 바이너리 업로드0.
