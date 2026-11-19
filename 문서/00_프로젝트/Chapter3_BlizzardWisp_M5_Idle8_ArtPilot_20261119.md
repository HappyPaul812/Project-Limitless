# LIMITLESS / Chapter3 M5 눈보라 정령 Idle8 — 2026-11-19

- Issue [#112](https://github.com/HappyPaul812/Project-Limitless/issues/112)
- [회전형 자연 정령 기획](https://github.com/HappyPaul812/Project-Limitless/commit/98ef6141c5160bfa68cce516017c5938e21f0a46), [Idle8 발주](https://github.com/HappyPaul812/Project-Limitless/issues/112#issuecomment-6997033601)
- 담당 `docs/art-new`. `KEYPOSE_KEEP / ROTATION_DIRECTION_APPROVED / IDLE8_ART_CANDIDATE / USER_IDLE8_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`.

## 1. 선행 승인 및 원본 보호
- [#116 전리품6종 사용자 최종 승인](https://github.com/HappyPaul812/Project-Limitless/issues/116#issuecomment-6997032271) 확인. 기존 ZIP SHA256 `8d7ff5e8c3c16aebe9d2992ace2326002e8e72946d62c1b0faad37a251acb54c`, 6종 원화 재제작 없음. [Codex 상태](https://github.com/HappyPaul812/Project-Limitless/issues/116#issuecomment-6997270316)는 로컬 적용 준비 보관, 정식 Item/드롭 연결 0/6, 게임 미적용.
- M5 기존 KeyPose를 ChatGPT Library에서 회수. 실제 원본 SHA256 **`0c72c3f8fc80c9e74bf648236a0ff1746ef344ef71f03b4c005c38db59915586`** 작업 전후 일치. 원본 변경0. Frame00은 원본 전RGBA 픽셀 동일.

## 2. 제작 방식
- **신규 게임용 시트 1개만**: `Sprite/BlizzardWisp_M5_Idle8_4096x512_RGBA_CANDIDATE.png` 4096×512 투명 RGBA, 가로8×512셀. 1,605,449 bytes. SHA256 **`28a8ae1d325617431ed3c2441467c235c323ce229fe91fa5190018ac8bde1cd7`**.
- 기존 원본에서 연결된 본체1개와 독립 얼음 조각12개를 분리. 핵 주변의 국소 각도·반경 변위에 주기적인 이동파를 적용하며 안정된 내부 핵과 실루엣을 유지. 얼음 조각들은 각자 위상을 가진 1주기 공전 경로와 방향 변화, 전후 겹침 처리. 상하 부유 최대 약3px. 전체 원화 평면 회전 없음. 가장자리 근처 변위 제한, 조각 위치의 안전 여백 제한. 2D 국소 변형 및 레이어 합성이므로 실제 입체 유체 시뮬레이션이 아니며 최종 회전 감각/루프의 자연스러움은 사용자 미술 검토 대상. 앞서 별도로 생성한 시안은 최종 게임용 파일에 사용하지 않음. 외부 게임 자산 복제0.
- 8프레임 상이, 셀 경계 alpha0. 가시 bbox: [36,8,475,464], [37,6,476,464], [36,4,478,462], [34,5,483,460], [31,6,481,461], [30,9,478,462], [31,10,475,463], [29,9,475,464]. Frame00과 원본 전RGBA 동일. 8개 개별 QA PNG. 모든 미리보기는 실제 최종 8프레임 기반.
- 실제 512/128/40 비교판, 128/40 투명 축소 스트립, 간략 비교판. 검토 GIF 512×512 어두운 배경 합성 후 색상 양자화, 디코드 8프레임 RGB 제작용 이미지 일치, loop=0, 실제 지연 120ms×8 = 960ms/주기(약8.33FPS). GIF 재생 타이밍은 실제 Unity 계약 아님.

## 3. 납품
- **`BlizzardWisp_Issue112_Idle8.zip`**, 6,121,505 bytes / 17 entries, SHA256 **`a4db522ec21d96edd7718655bb5889f32d9d25c5cfe79c0c44dbbf729428e58d`**. CRC PASS, Manifest 15/15 파일크기+SHA+커버 PASS, SHA256SUMS 16/16 PASS, 자기·순환참조0. SourceDeclaration_And_QA.json 동봉.
- Library `/LIMITLESS/Issue112_BlizzardWisp_Idle8/` ZIP·PNG·GIF·비교판2 저장 성공. Windows F: 직접 복사 및 GitHub 바이너리 업로드 없음.

## 4. 승인 게이트
**Idle8 사용자 미술 승인 필요 YES.** 실제 Unity Import/Animator/PPU/전투 및 런타임 QA 미진행. Attack/Hit/Defeat, 스킬/수치/드롭/펫/Spawn/ItemId 작업 없음. Chapter2 불씨망령은 NPC 리뉴얼 완료 후 별도 발주까지 HOLD. Unity6000.6.5f1, #85 HOLD, Save/UserData/Discord/기존 승인 자산 보호, main 직접 commit·Push·Merge 없음.
