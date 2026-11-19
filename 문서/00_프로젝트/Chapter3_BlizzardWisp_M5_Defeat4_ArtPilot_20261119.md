# LIMITLESS / M5 눈보라 정령 Defeat4 제작 및 오프라인 검증 — 2026-11-19

- Issue [#112](https://github.com/HappyPaul812/Project-Limitless/issues/112), [Hit4 최종 승인 및 Defeat4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/112#issuecomment-7004512175), [정령계 공통 기준](https://github.com/HappyPaul812/Project-Limitless/commit/98ef6141c5160bfa68cce516017c5938e21f0a46)
- 담당 `docs/art-new`. 상태 `KEYPOSE_KEEP / IDLE8_V1_KEEP / IDLE8_V2_USER_APPROVED / ATTACK4_USER_APPROVED / HIT4_USER_APPROVED / DEFEAT4_ART_CANDIDATE / USER_DEFEAT4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / DEFEAT4_UNITY_NOT_APPLIED`.

## 1. 기존 자산 보호
기존 실제 원본 PNG 5종 작업 전후 SHA256 일치. 원본 수정0.
- KeyPose: `0c72c3f8fc80c9e74bf648236a0ff1746ef344ef71f03b4c005c38db59915586`
- Idle8 v1: `28a8ae1d325617431ed3c2441467c235c323ce229fe91fa5190018ac8bde1cd7`
- Idle8 v2: `ba6a7bddcd37bdf7fe78939862e93a50828561a08564756572d0bba19f51692a`
- Attack4: `f8ca104d24c2826c4a757c5dd3ad80e30e29b222cbdcd6e38c4eaa4b051a9934`
- Hit4: `2e07e2178f48bf0dcd6367a73a9f9c4af5311073df2dc2c9a7e33e139b311de1`

## 2. 신규 후보 1종
- `Sprite/BlizzardWisp_M5_Defeat4_2048x512_RGBA_CANDIDATE.png`: 2048×512 RGBA, 가로4셀 각512×512, 677,357bytes, SHA256 `5f4c5942895035b23d824707a7e4e534c20c323b4a3aa82bc5cd824735d23299`.
- 핵 약화→눈바람 풀림 및 파편 분산→붕괴→전투불능 잔류 자세. 폭발·별도 VFX·게임 기능 변경 없음.
- 앞서 생성한 신규 2172×724 RGBA 패배 시안(`86c1b389cbb00fcfebb506d7398d7c4010cb49860b3b42eecb706420d01d5a0d`)을 4개 셀 분리하고 알파>32, 연결성분 면적>=22 필터, 동일 배율0.85 Lanczos 축소, 알파<=8 제거, 공통 x 중심 및 하단 y464로 정규화. 기존 승인 PNG는 참조 및 보호 대상으로 유지. **새로 그린 포즈이므로 기존 원화와 픽셀 동일하지 않으며 세부 묘사 차이 가능**. 전체 원화 단순 회전과 외부 게임 자산 사용 없음.
- 개별 QA PNG4장과 시트 전RGBA 일치, 4셀 상이, 셀 경계 alpha0. 실제 bbox 반열림 [62,16,449,464], [50,35,461,464], [45,144,466,464], [47,218,464,464]. 높이 및 알파 면적 순차 감소.
- 실제 512/128/40px 비교판, 간략 비교판 및128/40 투명 축소 스트립. 실제 최종 프레임 기반 비루프 GIF를 재디코드해 4프레임 RGB 일치. 실제 간격160/190/230/1600ms=2180ms, 마지막 1.6초 유지, loop 확장 없음. 런타임 타이밍 계약 아님.

## 3. 검증과 납품
- `BlizzardWisp_Issue112_Defeat4.zip` 2,562,804bytes/13entries, SHA256 `006f1d72bf7b536ced44ea8998abd8402d63950e78c0862a1cab4a91dba28a91`. CRC PASS, Manifest11/11 크기·SHA·커버 PASS, SUMS12/12 PASS, 비순환. SourceDeclaration_And_QA.json 포함.
- Library `/LIMITLESS/Issue112_BlizzardWisp_Defeat4/` ZIP·게임용 PNG·GIF·비교판2 저장 성공. GitHub 바이너리 업로드 및 사용자 PC 복사 없음.

## 4. 승인 게이트
Hit4 최종 미술 승인 반영. Defeat4 사용자 미술 승인 대기. 이번 Unity 실행/적용 및 전투·스킬·VFX·드롭·펫 변경0. 이전 Idle8 v2 로컬 Prefab 적용 이력과 구분. 정식 M5 등록·실전 조우 검증은 별도 발주. 승인 자산/#85 HOLD/Save/UserData/Discord/Unity6000.6.5f1 보호, main 직접 commit/Push/Merge0.
