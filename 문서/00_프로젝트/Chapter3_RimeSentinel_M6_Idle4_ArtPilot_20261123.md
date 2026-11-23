# LIMITLESS / Chapter 3 F5 빙각 수호체(Rime Sentinel) Idle4 파일럿 — 2026-11-23

**Issue:** [#126](https://github.com/HappyPaul812/Project-Limitless/issues/126)  
**승인 근거:** [바울 대표 KeyPose 최종 외형 승인·Idle4 후속 발주](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7088805208)  
**담당 브랜치:** `docs/art-new`  
**상태:** `KEYPOSE_USER_APPROVED / IDLE4_CANDIDATE_DELIVERED / USER_IDLE4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`

## 1. 원본 회수 및 보호
- ChatGPT Library에서 승인된 **`RimeSentinel_M6_KeyPose_512_RGBA_CANDIDATE.png` 512×512 RGBA** 실물을 직접 회수. SHA256 `c34ea53a95aa6a1fdba4f5e355715fbc25797d6d51e4d544cb9e4dab52f7073c` 기존 승인 정본과 **일치 PASS**.
- 승인 KeyPose 원본 파일의 바이트는 수정/재생성/덮어쓰기 **0회**. Idle4 Frame00의 **RGBA 512×512 픽셀 전체가 승인 원화와 정확히 동일 PASS**. 나머지 3프레임은 원본을 기반으로 각각 독립 파생 제작.
- 기존 공식 M1~M5/F1~F3 환경·Chapter2 보스·설빙의 폭군·BattleBG 및 Issue #123 자산 변경0.

## 2. 실제 게임용 Idle4 한 시트
- **`Sprite/RimeSentinel_M6_Idle4_2048x512_RGBA_CANDIDATE.png`**: **2048×512 RGBA, 가로4×512×512셀**, 1,124,477bytes. PNG SHA256 **`0757ebedd37f24e7939d26f38be2efbf9560906b832fae187c61273a314b753e`**.
- 동작: **중립(원본) → 무게 이동 → 어깨 상승과 좌우 주먹 미세 반동 → 자세 안정·원자세로 전환**. 각 프레임의 어깨/주먹/상체 특정 부위만 독립 비강체 역좌표 변형. 전체 캐릭터 한 장을 통째로 이동·회전하거나 갑주 디자인을 새로 그리지 않음. 어깨 얼음·얼굴·주먹·다리 구조 유지; 눈/화로/동결괴사 VFX 등 추가0.
- 안전 기준: **각 프레임 y425~511 전체 RGBA 픽셀을 승인 KeyPose와 동일하게 고정**. 발/바닥접지 바이트 수준 프레임간 동일. Frame00 전체 동일. 4프레임 서로 상이, PNG 시트 셀과 `QA/` 독립 프레임4개 전RGBA 일치. 각 셀 바깥 알파 경계0.
- 가시 bbox (Pillow 반열림): Frame00 [31,65,481,490], Frame01 [30,64,482,490], Frame02 [30,63,482,490], Frame03 [31,65,481,490]. 전프레임 중심축·발끝 안정. 국소 **premultiplied-alpha bilinear remap**으로 개별 프레임 내 보간 픽셀이 다수 변할 수 있음(원형 유지 목적의 국소 보간, 디자인 변경과 다름). 원본 대비 RGBA 차이 픽셀 개수 [0,103860,107243,103037], **이 수치는 서브픽셀 보간 변화 수로 실제 아트의 신규 추가량이 아님**.
- 이 방식은 승인 원본 파생 **오프라인 2D 변형 파일럿**이며, 향후 수작업 픽셀 클린업/Unity 출력 검수 필요 여부를 대표가 미술 판단할 수 있음. 실제 게임 내 PPU/Animator/필드 배율은 아직 검증하지 않음.

## 3. 검토용 미디어
- `Preview/RimeSentinel_M6_Idle4_512_128_40_Review_OFFLINE.png`: 게임용 신규 시트 4셀에서 직접 생성한 원크기·실제128px·40px 축소 비교판. 오프라인 합성이지 Unity GameView 아님.
- `Preview/RimeSentinel_M6_Idle4_Actual_128px_4Frames_RGBA.png`: 4×128셀, 512×128 투명 RGBA.
- `Preview/RimeSentinel_M6_Idle4_Actual_40px_4Frames_RGBA.png`: 4×40셀, 160×40 투명 RGBA.
- `Preview/RimeSentinel_M6_Idle4_260ms_x4_LOOP_OFFLINE.gif`: 게임용 프레임을 어두운 배경에 합성한 **검토용 GIF**. 재개봉 실제 **260ms×4 = 1,040ms/주기**, loop=0(무한반복). 원본 게임용 PNG는 배경 없는 32bit RGBA. GIF 타이밍은 Unity Animator 정식 속도 지정이 아님.

## 4. ZIP 무결성 및 실납품
- 납품 ZIP **`LIMITLESS_Issue126_RimeSentinel_Idle4_20261123.zip`**, **3,649,494bytes / 13 entries**, ZIP SHA256 **`65520ee3b6fc5f1433489ba733ed40ea3256d4e50e977b9f67edd5fb19b2b0d2`**.
- 실측: ZIP 재개봉 CRC **PASS**, Manifest payload **11/11 파일 크기·SHA256·파일 커버 PASS**, SHA256SUMS payload11+Manifest1 **12/12 PASS**; Manifest에 자기 자신 및 SUMS를 포함하지 않고 SUMS도 자기 참조하지 않아 순환 해시 없음. `Docs/SourceDeclaration_And_QA.md` 및 `QA/animation_qa.json` 포함.
- 다운로드 문제 대비 ChatGPT Library **`/LIMITLESS/Issue126_RimeSentinel_Idle4_20261123/`**에 ZIP·게임 시트 PNG·실제 축소 비교판·검토 GIF 4종 직접 저장. 사용자 PC `F:\Downloads` 자동 복사 및 GitHub 원격 PNG/ZIP 바이너리 업로드 없음.

## 5. 승인 게이트·프로젝트 보호
- **대표 미술 승인 필요 YES (Idle4 한 시트)**. 기승인 KeyPose는 `USER_APPROVED`로 KEEP. Idle4는 **`USER_IDLE4_ART_REVIEW_PENDING`**이며 기존 기술 QA가 미술 승인을 대체하지 않음.
- Attack/Hit/Defeat 제작0; Unity6000.6.5f1 Editor/Asset/Prefab/Scene/Collider/PPU/Animator 작업0, 정식 Elite MonsterDefinition/Spawn/Drop/Save 및 Codex #122 작업 변경0. Issue #85 HOLD, Save/UserData, Discord 보호.
- `docs/art-new` **문서 2개 및 Issue 댓글만 기록**. main 직접 commit·원격 Push·Merge 0. 실제 Unity 런타임/접근성/상대크기 검증 `NOT_RUN`.
