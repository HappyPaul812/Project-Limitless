# LIMITLESS / M5 눈보라 정령 Idle8 v2 표적 보완 — 2026-11-19

- Issue [#112](https://github.com/HappyPaul812/Project-Limitless/issues/112)
- [v1 기술 PASS·표적 보완 지시](https://github.com/HappyPaul812/Project-Limitless/issues/112#issuecomment-6999769300)
- 담당: `docs/art-new`
- 상태: `KEYPOSE_KEEP / IDLE8_V1_KEEP / IDLE8_V2_ART_CANDIDATE / USER_V2_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`

## 1. 원본 보호 및 변경 범위
- 기존 KeyPose와 Idle8 v1 PNG 및 v1 ZIP은 읽기 전용으로 사용. 작업 전후 SHA256 일치 확인.
  - KeyPose: `0c72c3f8fc80c9e74bf648236a0ff1746ef344ef71f03b4c005c38db59915586`
  - v1 PNG: `28a8ae1d325617431ed3c2441467c235c323ce229fe91fa5190018ac8bde1cd7`
  - v1 ZIP: `a4db522ec21d96edd7718655bb5889f32d9d25c5cfe79c0c44dbbf729428e58d`
- v1에서 부족했던 본체 은백색 눈바람띠의 움직임을 강화하는 단일 v2 후보. 프레임수8, 재생 간격120ms, 전체 960ms 주기 유지. 별도로 생성된 원화 시안은 최종 게임용 PNG에 사용하지 않음.

## 2. 실제 신규 게임용 자산
- `Sprite/BlizzardWisp_M5_Idle8_v2_4096x512_RGBA_CANDIDATE.png` 4096×512 RGBA, 가로8셀×512, 1,678,792bytes, SHA256 **`ba6a7bddcd37bdf7fe78939862e93a50828561a08564756572d0bba19f51692a`**.
- 기존 원화의 본체와 12개 독립 얼음 조각을 분리. 핵 주위 은백색 띠에 비강체 각도·반경 진행파 변형 및 이동하는 명암 융선을 적용. v1 얼음 조각 중심 경로 재사용, 개별 조각 회전·전후 합성. 중심핵 안정 및 미세 상하 부유. 전체 원화 단순 평면 회전0.
- 8셀 모두 상이, 셀 경계 alpha0, 저장된 시트 셀과 개별 PNG 일치. **v2 첫 프레임은 KeyPose와 픽셀 동일하지 않음**(원본 파일 자체는 보존). 2D 국소 변형·합성 방식으로 실제 3D 유체 회전 시뮬레이션은 아님.
- 512px 전체 프레임 비교판, 실제 128/40px 축소 스트립, 간략 비교판, v2 단독 GIF와 실제 v1/v2 시트에서 직접 생성한 동일 타이밍 나란히 비교 GIF 제공. GIF 재개봉 8프레임, 실제 디코드 120ms×8, loop=0. 미리보기에는 배경 합성 및 GIF 색상 양자화 사용. 회전감 및 실루엣의 최종 미술 판단은 사용자 검토 대상.

## 3. 납품 및 파일 검증
- `BlizzardWisp_Issue112_Idle8_v2.zip` 6,528,393bytes / 18 entries, SHA256 **`8be07f1984d77065743ff587fb2319e3225f1003e1d8201169974cb9dc413f9c`**.
- CRC PASS, Manifest 16/16 파일 크기·SHA 및 파일 목록 일치, SHA256SUMS 17/17 PASS. Manifest 자기/SUMS 미포함 및 SUMS 자기 제외. SourceDeclaration_And_QA.json 동봉.
- ChatGPT Library `/LIMITLESS/Issue112_BlizzardWisp_Idle8_v2/`에 ZIP·게임 PNG·v1/v2 GIF·v2 GIF·비교판 저장 성공. 사용자의 Windows PC 자동 복사 및 GitHub 바이너리 업로드 없음.

## 4. 승인 및 보호
**v2 사용자 미술 승인 필요 YES.** Unity Import/Animator/PPU/런타임 테스트 미실행. Attack/Hit/Defeat, 드롭·수치·펫·전투 변경0. 기존 승인 자산, Unity6000.6.5f1, #85 HOLD, Save/UserData, Discord 보호. 불씨망령 NPC 리뉴얼 완료 전 HOLD. main 직접 commit·Push·Merge0. v1은 삭제하지 않고 보존.
