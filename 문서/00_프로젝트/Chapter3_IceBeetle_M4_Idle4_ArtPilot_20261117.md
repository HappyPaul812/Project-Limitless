# LIMITLESS / 메인 신규 아트 / Chapter 3 M4 얼음갑충 Idle4 파일럿 — 2026-11-17

- **담당 브랜치:** `docs/art-new`
- **GitHub Issue:** [#103](https://github.com/HappyPaul812/Project-Limitless/issues/103)
- **미술 판정:** `KEYPOSE_USER_APPROVED / IDLE4_CANDIDATE_DELIVERED / USER_IDLE4_ART_REVIEW_PENDING / OFFLINE_FILE_TECH_PASS / UNITY_NOT_APPLIED`

## 1. 승인 원본 회수 / 보존

- ChatGPT Library `/LIMITLESS/Issue103_IceBeetle_OnePose/IceBeetle_M4_KeyPose_512_RGBA_CANDIDATE.png`를 실제 회수하여 SHA256 재측정.
- 512×512 RGBA 승인 원본 SHA256: **`b7ff0592226c2a1f28270053cf03483b6051c69edb412b59a7965ccf8699b3ad`**, 일치 PASS.
- **Idle4 Frame00은 승인 KeyPose와 512×512 RGBA 픽셀 전체 동일**. 기존 KeyPose 이미지 바이트 수정/재생성/덮어쓰기0.
- 이전 별도 이미지 생성기를 통해 만든 2172×724 신규 프레임/별도 리뷰보드/애니 GIF 시안은 **게임용 스프라이트 시트로 사용하지 않음**. 최종 납품은 승인된 512×512 원본 RGBA 픽셀에서만 만든 국소 변주 프레임이다.

## 2. 신규 Idle4 단일 시트

- 새 게임용 후보 **`Sprite/IceBeetle_M4_Idle4_2048x512_RGBA_CANDIDATE.png`**: **2048×512, 투명 RGBA, 4개 가로 프레임 각 512×512, 우향**.
- **PNG SHA256:** `bd06c6d9a901d4c3550882ad4ce061629195b60e75f04636ec4814ebb0967793`, 801,439 bytes.
- 프레임 00: 승인 KeyPose 픽셀 그대로. 01: 두꺼운 상부 갑각 소폭 내려앉음. 02: 아주 약하게 높이 회복/반동. 03: 원자세 근처로 돌아감. 전체 무게중심·얼음 갑각 분절·청록 명암·금빛 미광·우향 3/4 실루엣 보존; 과장된 몸통 점프나 다리 걸음 동작 없음.
- 서브픽셀 보간 적용은 **주로 상부 갑각/몸체 y125~397**에서만 이루어짐. **y397~511 하부 다리·발의 RGBA 픽셀 4/4 모두 승인 KeyPose와 동일**. 4개 프레임은 각각 서로 구분되고 바깥 알파 테두리 4/4 투명 PASS.
- 승인 원본 대비 각 프레임의 RGBA 변경 픽셀 수: **[0, 78,587, 78,571, 74,352]**. 수치가 많은 이유는 상부 갑각 픽셀의 아주 작은 서브픽셀 변위에 따른 보간이며 새로운 디자인/몸통 재제작이 아니다.
- 분리 프레임 QA PNG 4종 `Sprite/IceBeetle_M4_Idle_00_512_RGBA_QA.png` ~ `_03`, 512×512 투명 RGBA; 최종 Sheet와 분리 이미지 재합성 비교 PASS.

## 3. 검토용 실제 축소 / GIF

- `Preview/IceBeetle_Idle4_Actual_128px_RGBA.png`: 4×128 셀, **512×128 RGBA**.
- `Preview/IceBeetle_Idle4_Actual_40px_RGBA.png`: 4×40 셀, **160×40 RGBA**.
- `Preview/IceBeetle_Idle4_512_128_40_Review_OFFLINE.png`: 실제 최종 Idle4 프레임에서 **512·128·40px**로 변환한 이미지와 체크무늬·밝은 설원 가독성 비교. ImageGen의 별도 구성 이미지를 비교판인 척 사용하지 않음.
- `Preview/IceBeetle_Idle4_8FPS_OFFLINE.gif`: 최종 네 프레임으로 만든 125ms×4 = **8FPS 반복** 미리보기. 밝은 갑각이 잘 보이도록 GIF 프리뷰에는 어두운 단색 배경을 합성했으나 게임용 시트는 투명 RGBA임. 모두 **OFFLINE**이며 Unity 화면/Animator 적용 검증이 아님.

## 4. 비순환 Manifest / ZIP 무결성

- **납품 ZIP:** `IceBeetle_Issue103_Idle4_20261117.zip`
- **ZIP SHA256:** `8a9ecec00a98cb094e155aa31f7e737190358d9481ad333d2371e8013281ea87`
- **ZIP 실측:** 3,010,425 bytes, 12 entries
- ZIP 재개봉 후 `zipfile.testzip()==None` **CRC PASS**.
- `Manifest.json`에는 **payload 10개** 실제 크기/SHA256만 기재 → **10/10 PASS**. Manifest 자체와 SHA256SUMS는 Manifest.files 목록에 포함하지 않음.
- `SHA256SUMS.txt`에는 **payload10 + 확정된 Manifest1** 기록 → **11/11 PASS**. SHA256SUMS 자기참조 없음; 과거 #100의 Manifest 순환해시 오류 회피.
- `Docs/SourceDeclaration_And_QA.md`와 Manifest/SHA256SUMS 동봉.
- 기존 승인 KeyPose/기존 Monster 원본 바이트 보호. 새 ZIP·시트·리뷰보드·GIF는 ChatGPT Library **`/LIMITLESS/Issue103_IceBeetle_Idle4/`**에 각 1개 업로드했고 채팅 다운로드 링크도 제공. GitHub에는 보고 Markdown만 commit. 사용자 Windows F:\Downloads에 직접 복사하거나 Unity 프로젝트에 적용한 적 없음.

## 5. 후속 승인·권한 경계

- **기존 얼음갑충 1포즈:** `USER_ART_APPROVED`.
- **이번 Idle4:** `USER_IDLE4_ART_REVIEW_PENDING` (실제 기술 QA 합격 ≠ 바울 대표 시각 승인).
- **금지 유지:** Attack/Hit/Defeat 신규 제작, 다방향·펫 동작·신규 스킬/전리품/MonsterID·Spawn/전투 수치·Unity Import/Animator 변경 없음.
- Unity **6000.6.5f1**, Issue **#85 HOLD**, 기존 승인 Mouse·Stoat·Owl·Chapter2 환경/몬스터/보스, Save/UserData/Discord, Scene/Prefab/meta/GUID 변경0. 대형 Unity QA/worktree 복제0. `main` 직접 commit/원격 Push·Merge 없음.
