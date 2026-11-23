# LIMITLESS / 빙각 수호체 Attack4 v2 — 타격 구간 국소 수정 (2026-11-23)

**관련 이슈:** [#126](https://github.com/HappyPaul812/Project-Limitless/issues/126)  
**담당 브랜치:** `docs/art-new`  
**승인 상태:** `KEYPOSE_IDLE4_USER_APPROVED / ATTACK4_V2_ART_CANDIDATE / USER_ATTACK4_V2_ART_APPROVAL_REQUIRED / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`

## 1. 발주/승인 및 원본 잠금

- 대표 최신 피드백: v1 전체 방향은 유지하면서 **주먹 공격치고 팔 전진이 약한 2·3프레임을 국소 보완**. 준비·회복 및 몸체 구조·접지·무게감은 보존.
- Codex [v1 격리 기술 검수](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7089964295): Unity 6000.6.5f1 v1 Animator PASS, 40px 타격 분리 PARTIAL. **v2는 별도 Unity 검증하지 않았음.**
- Library 실물 회수 및 SHA256 잠금 (모두 읽기 전용, 신규 결과가 원본을 덮어쓰지 않음):
  - 승인 KeyPose `c34ea53a95aa6a1fdba4f5e355715fbc25797d6d51e4d544cb9e4dab52f7073c`.
  - 승인 Idle4 `0757ebedd37f24e7939d26f38be2efbf9560906b832fae187c61273a314b753e`.
  - 기존 Attack4 v1 `7a44d60d540176d06c96ee3ff221db5ebbde40077977d8de8762a714ff480e62`.
- 실제 3종 소스는 제작/ZIP 작업 전후 위 SHA와 일치. 기존 공식 아트·Scene/Prefab/Unity 원본 변경0.

## 2. 신규 게임용 Attack4 v2 시트

- **`Sprite/RimeSentinel_M6_Attack4_v2_2048x512_RGBA_CANDIDATE.png`**, **2048×512 투명 RGBA**, 가로 4×512px 셀, **1,137,770 bytes**, SHA256 **`0649ef42da7aa628e471a49f4554ad150a0a5fc1b28235a5e9b42bf025030621`**.
- 기존 v1의 0-based Frame00·Frame03을 **전RGBA 픽셀 그대로 보존**. Frame03은 승인 KeyPose 전체 픽셀 동일. 별도 신규 캐릭터/갑옷/얼굴/주먹 디자인 생성 없이 **기존 v1 소스의 국소 역좌표 이미지 변형**만 적용.
- 0-based Frame01: 관객 오른쪽 주먹을 화면 앞쪽으로 투영해 전진감을 조금 더 강화, 전완 및 어깨의 경미한 추종 이동.
- 0-based Frame02: 타격 시 주먹을 화면 앞/아래로 약 30px 투영하고 국소 원근 확대 계수 약0.31(균일 캐릭터 확대 아님), 어깨/전완은 소폭 추종. 기존안 대비 피격을 일으키는 타격 순간의 사거리감 보강. **팔 들어올리기 예고·균열 강타 모션/VFX는 별도**.
- 프레임별 v1과 다른 RGBA 픽셀 수: **0 / 42,758 / 47,805 / 0**. 같은 40×40 RGBA로 축소한 뒤 v1과 다른 픽셀 수: **0 / 381 / 418 / 0** (리샘플링에 따른 색 차이도 포함하므로 실제 타격 범위 확정 수치가 아님). 프레임 가시 bbox 반열림: `[30,65,475,490]`, `[30,64,492,490]`, `[30,64,504,490]`, `[31,65,481,490]`. 네 변 알파0, 마지막 타격 프레임도 8px 이상 우측 여백. 4셀 각각 상이/분리 QA PNG와 전RGBA 동일.
- **발 접지 y424~511은 네 프레임 모두 승인 KeyPose RGBA와 100% 일치**, 기존 중심축/다리 구조 고정. 사전 승인 구조 변경·점프·전체 캐릭터 회전 금지 준수.
- 본 파일럿은 2D 국소 워프 기반이라 완전한 수작업 새 포즈가 아니며, **주먹의 입체 전진·어깨 연결의 자연스러움은 사용자 시각 승인 대상**.

## 3. 비교자료 / GIF

- `Preview/RimeSentinel_Attack4_v2_512_128_40_Review_OFFLINE.png`: 최종 신규 게임용 RGBA 시트에서 만든 512px 전프레임 및 실제 128px·40px 축소 비교판.
- `Preview/RimeSentinel_Attack4_v1_vs_v2_Comparison_OFFLINE.png`: v1/v2 4개 프레임 전후 및 타격 40px 비교.
- `Preview/RimeSentinel_Attack4_v1_vs_v2_ImpactFocus_OFFLINE.png`: 타격 주먹·전완 근접 비교.
- `Preview/RimeSentinel_Attack4_v2_NonLoop_660ms_OFFLINE.gif`, `Preview/RimeSentinel_Attack4_v1_vs_v2_NonLoop_660ms.gif`: 실제 납품 이미지 기반 비루프 검토용; 디코드 프레임별 **170/120/150/220ms, 총660ms**, 반복확장 없음. Animator·데미지 판정 타이밍의 확정 계약이 아님.
- 128/40px 투명 RGBA 4프레임 축소 스트립 및 개별 검증 PNG 4종. 미리보기 체크무늬/어두운 배경은 **게임용 원본 PNG에 없음**.

## 4. ZIP 검증 / 저장

- **`LIMITLESS_Issue126_RimeSentinel_Attack4_v2_Targeted_20261123.zip`**, **6,444,482 bytes / 16 entries**, SHA256 **`04cdead8ccffc14350fe9cd7e666a1635dddd6214631c8dc8ae7831f8dfc8661`**.
- ZIP 재개봉 CRC PASS; Manifest payload **14/14** 파일크기·SHA256·경로 전체커버 PASS; SHA256SUMS payload 14 + Manifest 1 = **15/15 PASS**. Manifest 자기·SUMS 참조 제외 / SUMS 자기 참조 제외 = 순환 해시0. SourceDeclaration 및 QA JSON 포함.
- ChatGPT Library **`/LIMITLESS/Issue126_RimeSentinel_Attack4_v2_20261123/`**에 ZIP, 게임용 시트, 512/128/40판, v1/v2 비교, 타격 확대, GIF 2종 보관. 사용자 Windows F: 직접 복사와 GitHub 원격 바이너리 업로드 없음.

## 5. 범위·후속

- **바울 대표 Attack4 v2 미술 승인 필요 YES.** v1 기술 PASS는 v2 제품 QA가 아니며, v2 실제 Unity Import/GameView/Animator/PPU/전열 타깃/Hitbox QA `NOT_RUN`.
- 기승인 KeyPose·Idle4 및 v1은 보존. Hit·Defeat·별도 예고 팔 올리기·균열 강타 신규 제작0. 기존 승인 자산·Unity6000.6.5f1·Codex#122/#85 HOLD·Save/UserData·Discord 및 main 보호. main 직접 commit·Push·Merge0. `docs/art-new` 문서만 기록.
