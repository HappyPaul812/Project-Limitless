# LIMITLESS / Chapter3 빙각 수호체(Rime Sentinel) Defeat4 파일럿 — 2026-11-23

- **Issue:** [#126](https://github.com/HappyPaul812/Project-Limitless/issues/126)
- **선행 승인:** [Hit4 조건부 승인 발효 및 Defeat4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7092168466)
- **담당 브랜치:** `docs/art-new`
- **상태:** `KEYPOSE_IDLE4_ATTACK4V2_HIT4_USER_APPROVED / DEFEAT4_ART_CANDIDATE / USER_DEFEAT4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`.

## 1. 정본 승인과 4종 원본 보호

Hit4는 Codex 실제 Unity 6000.6.5f1 격리 Play Mode 610ms 4프레임·112px 검증 PASS에 따라 최종 미술 승인으로 전환. 단, 제품 전투/F5 실전/Save·Drop·원본 Unity 적용은 NOT_VERIFIED. 이 작업은 **Defeat4 파일럿 하나만 신규 제작**.

로컬 읽기 전용 승인 PNG 실물 및 작업 종료 후 SHA256 검증:
- KeyPose: `c34ea53a95aa6a1fdba4f5e355715fbc25797d6d51e4d544cb9e4dab52f7073c`
- Idle4: `0757ebedd37f24e7939d26f38be2efbf9560906b832fae187c61273a314b753e`
- Attack4 v2: `0649ef42da7aa628e471a49f4554ad150a0a5fc1b28235a5e9b42bf025030621`
- Hit4: `bd6d66be3b990d9bc7f33261f1d9835abd413c28b91a5842ac9800a5dec28496`

네 파일 모두 기존 기록 해시와 일치; 기존 승인 원본 편집·재생성·덮어쓰기0.

## 2. 신규 Defeat4 파일

- **`Sprite/RimeSentinel_M6_Defeat4_2048x512_RGBA_CANDIDATE.png`** 2048×512 투명 RGBA, 가로4×512셀, **975,465 bytes**, SHA256 **`a7d12a5a86495b59b2c7e0a96e658ff1cf743cdec3daafa9fb4e9ce9a8124d8d`**.
- **모션**: 균형 상실 → 상체 및 무릎이 낮아짐 → 쓰러진 자세 → 최종 전투불능 자세에서 정지. 폭발·소멸·추가 파편/VFX 없음.
- 이번 대화에서 생성한 신규 패배 4포즈 이미지 `서리_파수꾼의_얼음_패배_애니메이션.png` 2172×724 RGBA SHA256 `93c86b5ecf58732e3707323e8216cf772e9c5768d7c8d4dc2f1b8f6be91bfce5`를 소스로 사용. 원본에 **반투명 흰 배경이 있었으므로** 실제 캐릭터 본체를 색상·연결성분 기반으로 투명 분리하고 각 포즈 영역을 크롭, **단일 축소율 0.888...** (프로그램 QA JSON에 정확한 비율)로 동일 스케일에 정렬. 모든 셀 하단을 **y489**로 맞추고 알파 경계0 유지. 원본 KeyPose나 승인 애니메이션 PNG를 직접 변형한 출력이 아니므로, **얼굴·갑옷 내부 세부 묘사에 미세한 차이가 생길 수 있어 사용자 미술 시각검토 필수**.
- 4개 셀 모두 상이, 개별 QA/최종 시트 픽셀 일치. 셀 bbox (반열림): `[35,94,477,489]`, `[43,133,468,489]`, `[31,211,480,489]`, `[31,211,481,489]`. 4변 외곽 alpha0, 동일 하단/규격 및 비점프 정렬 PASS. 이는 실제 Unity pivot·sorting·collider·전투 화면 판정과 다름.

## 3. 검토 미디어

- `Preview/RimeSentinel_M6_Defeat4_512_128_40_Review_OFFLINE.png`: 게임용 PNG 실물에서 직접 **512/128/40px** 생성, 네 프레임 비교.
- `Preview/RimeSentinel_Defeat4_Actual_128px_4Frames_RGBA.png` 및 `Preview/RimeSentinel_Defeat4_Actual_40px_4Frames_RGBA.png`: 투명 스트립.
- `Preview/RimeSentinel_ApprovedKeyPose_vs_Defeat4_OFFLINE.png`: 승인 KeyPose와 신규 첫/최종 포즈를 실제 PNG 기반 비교, 동일 인물 구조 및 세부 변화 시각 검토용.
- `Preview/RimeSentinel_M6_Defeat4_NonLoop_FinalHold_OFFLINE.gif`: 게임용 4셀 기반 GIF, **180/220/250/1800ms** 재개봉 확인(총 2450ms), **loop extension 없음**. 마지막 1.8초 정지. Unity에서 최종 포즈 계속 유지하려면 Defeat 애니메이션 종료 상태를 별도 설정해야 함; **Unity Animator 타이밍/종료 유지 규칙 아직 NOT_RUN**.

## 4. 실제 ZIP, Manifest, SHA256SUMS

- **`LIMITLESS_Issue126_RimeSentinel_Defeat4_20261123.zip`**: **3,607,746 bytes / 14 entries**, SHA256 **`c7b840ce80815814be0a11269d29b636acfa0b2966981801fa19228bade296ef`**.
- ZIP 재개봉 **CRC PASS**, Manifest 내용물 **12/12 경로·크기·SHA256/전체커버 PASS**, SHA256SUMS payload12+Manifest = **13/13 PASS**. Manifest 자기참조·SUMS 자기참조 또는 순환해시 참조0. `QA/Defeat4_Offline_Validation.json`과 `Docs/SourceDeclaration_And_QA.md` 동봉.
- ChatGPT Library `/LIMITLESS/Issue126_RimeSentinel_Defeat4_20261123/`에 ZIP/PNG/비교판/GIF 저장 예정. 사용자 PC `F:\Downloads` 자동 복사, GitHub 원격 PNG/ZIP 바이너리 업로드 없음.

## 5. 승인 게이트 및 프로젝트 보호

- **Defeat4 대표 미술 승인 필요 YES**. 파일 규격/CRC/축소판 QA는 PASS이지만 **Unity 6000.6.5f1 임포트·Animator·실제 F5 전투 GameView·PPU/피벗/Sorting·전투불능 상태 유지 검증 NOT_RUN**. 제품 적용 금지.
- 기존 KeyPose/Idle4/Attack4 v2/Hit4 원본·F1~F3·M1~M5·설빙의 폭군·BattleBG/Issue #85 HOLD 및 Codex #122, Save/UserData, Discord·Unity 원본·main 보호. `docs/art-new`에 **문서만 기록**, Issue #126 댓글. 직접 main 커밋/Push/Merge 없음.
