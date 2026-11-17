# LIMITLESS / M4 얼음갑충 Attack4 제작 및 오프라인 검증 — 2026-11-17

**Issue:** [#103](https://github.com/HappyPaul812/Project-Limitless/issues/103)  
**승인 및 발주:** [Idle4 최종 사용자 미술 승인 / Attack4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6936952632)  
**담당:** `docs/art-new`  
**상태:** `KEYPOSE_IDLE4_USER_APPROVED / ATTACK4_ART_CANDIDATE / USER_ATTACK4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`.

## 1. 승인 원본 보존
- ChatGPT Library에서 승인 KeyPose·Idle4 실제 파일을 회수, 작업 전후 SHA256 일치 확인.
- KeyPose 512×512 RGBA: `b7ff0592226c2a1f28270053cf03483b6051c69edb412b59a7965ccf8699b3ad`.
- Idle4 2048×512 RGBA: `bd06c6d9a901d4c3550882ad4ce061629195b60e75f04636ec4814ebb0967793`.
- Idle4 Frame00과 KeyPose 전체 RGBA 동일. 기존 두 파일 변경0.
- [Codex Idle4 검수](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6936676152)에서 확인한 **기존 Idle4 GIF 실제 간격 120ms×4, 480ms 주기(약 8.33FPS)**로 과거 문서의 125ms/8FPS 표기를 정정. 기존 승인 PNG/ZIP/GIF 자체 재작업0.

## 2. 공격 시트 후보
- 실제 신규 게임용 PNG 1장: `Sprite/IceBeetle_M4_Attack4_2048x512_RGBA_CANDIDATE.png`, **2048×512 RGBA, 가로 4셀×512², 우향**, 616,816bytes.
- SHA256 **`d8dab5e411f7eeb0aa73b1e412b8d820ecfeb38d514c60cf5fef7eb29af4b236`**.
- 준비(상부 후방 -13px/하향 8px) → 밀치기(+9/+2) → 정점(+21/+7) → 회복(0/0). 승인 원화 픽셀에서 상부 갑각을 거의 강체처럼 이동시키고 다리 부근에서 부드럽게 변위를 줄이는 방식. 보간 때문에 세부 픽셀은 변할 수 있음. 앞서 생성한 시트·생성 리뷰보드·GIF 시안은 최종 납품에 사용하지 않음. 신규 외부 그림/VFX 사용0.
- 네 셀 독립적으로 내용 구별, y430..511 접지 영역의 RGBA가 승인 KeyPose와 4/4 동일, Frame03 전체 KeyPose와 완전 동일. 각 셀 네 변 alpha0. alpha>8 bbox(반열림): (18,133,481,464), (35,127,493,464), (42,132,505,464), (29,125,484,464). 정점 우측 가시 여백 7px. 실제 Unity에서 PPU/Animator/크롭 여유 검증하지 않았음.
- 4개 분리 QA PNG와 최종 시트 셀 일치. 512/128/40 실축소판 및 비교 보드는 실제 최종 PNG에서 직접 제작. GIF는 어두운 단색 배경 합성 후 팔레트 변환한 검토용으로, **실제 디코드 지연 180/110/130/180ms, 합계 600ms/주기, 4프레임 반복**. 가변 간격이며 고정 8FPS 아님. 각 디코드 프레임과 GIF 제작용 팔레트 변환 이미지의 RGB 일치 확인.

## 3. 납품 및 검증
- `IceBeetle_Issue103_Attack4_20261117.zip`: 2,669,553bytes / 12 entries. SHA256 **`ebb4172037c7b7e74115be0aaf38e70dc9c58b6e928549934415e290d682f997`**.
- ZIP 재개봉 검사 CRC PASS, Manifest 실제 payload 크기/SHA 및 전체 커버 10/10 PASS, SHA256SUMS payload10+Manifest1 및 전체 커버 11/11 PASS. Manifest 자체/SUMS 참조 제외, SUMS 자체 제외, 순환참조0. `Docs/SourceDeclaration_And_QA.md` 포함.
- ChatGPT 대화 및 Library `/LIMITLESS/Issue103_IceBeetle_Attack4/`에 ZIP/시트/비교판/GIF 저장. 사용자 PC F: 복사 및 GitHub 바이너리 업로드 없음.

## 4. 범위
**사용자 미술 승인 필요: YES (Attack4만).** 오프라인 후보 파일 QA PASS이며 Unity 통합 검수 아님. Hit·Defeat·펫·수치·스킬·드롭·MonsterID·Spawn·Unity 적용0. Unity6000.6.5f1, Issue85 HOLD, 기존 승인 몬스터·환경·보스·Save/UserData·Discord 보호. 대형 QA 복제, main 직접 commit·Push·Merge0. 문서 기록만 담당 브랜치.
