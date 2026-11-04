# LIMITLESS / 리뉴얼 아트 / Issue #90 — Organic Spider Attack8 중간 5프레임 납품 (2026-11-04)

**브랜치:** `docs/art-renewal` · **상태:** `ATTACK8_8FILES_OFFLINE_TECH_PASS / 3_APPROVED_EXACT_KEEP / 5_NEW_INTERMEDIATE_ART_REVIEW_PENDING / JOINT_CONTACT_NOT_VERIFIED / UNITY_NOT_APPLIED`.

## 1. 검증 근거와 승인 게이트

- [Issue #90 Codex F: 승인3장 SHA 일치](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6621574553), [총괄 기획 신규 중간5장 제작 발주](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6621779059), 기존 리뉴얼 전용 [Attack3 증분 인계](팀운영/인계/WORLD_ART_RENEWAL_ISSUE90_ATTACK3_DELTA_20261104.md) 확인.
- 바울 대표 승인: Prepare00, Contact01v2, Recover02v2 정적 키포즈 **세 장에만 미술 승인**. 따라서 원본 바이트를 유지한 채 Attack8 최종 슬롯 **00(Prepare) / 03(Contact v2) / 06(Recover v2)**에 정확히 배치. 이 인덱스는 **오프라인 후보 배치**이지 게임 피해 시점 결정이 아님.
- 신규 슬롯 **01/02/04/05/07 = 5 PNG만 생성**. 기존 승인 v5, Idle8, Walk8, 신규 Attack 3장 재작화/덮어쓰기 금지 준수. 기존 Unity 계약은 우향 / **10FPS = 각100ms** / PPU256 / Scale0.7 참고. 실제 Unity 임포트·AnimationClip·AI·판정·스킬 이벤트는 검증/적용하지 않음.

## 2. 실제 ZIP 및 QA

- 납품 ZIP **`LIMITLESS_SPIDER_ATTACK8_ISSUE90_20261104.zip`**, **5,922,807 bytes / SHA256 `e612a88a6d4c6b4422cc025c0d32fb34e4ffc2f40337ffa909a1885cfae3404` / 23 entries / ZIP CRC PASS / Manifest 21/21 files SHA256+bytes PASS**.
- 모든 프레임 **512×512 RGBA**. `ForestSpider_Attack8_4x2_512_RGBA.png` 2048×1024 RGBA(4열×2행, 셀512). GIF 128·40px 각 **8프레임×정확히100ms=10FPS**, 원래 종횡비 유지. `Evidence/Attack8_512px_8Frame_ApprovedVsNew.png`와 `Evidence/Attack8_40px_Sequence_8Slots.png` 수록.
- 모든 프레임 시각 알파(>8) **상·하·좌·우 경계 픽셀 접촉0**. 서로 다른 PNG 8개의 파일 SHA 전부 고유. 신규 5개의 512 이미지가 각각 고유 SHA·픽셀로 생성됨.
- `Evidence/QA_Results.json`, `Evidence/SourceDeclaration.md`, `Manifest.json`, `SHA256SUMS.txt`에 실물 검사값과 제작법 기록.

| 최종 슬롯 | 상태 | 역할 | SHA256 |
|---|---|---|---|
| 00 | 승인 원본 바이트 KEEP | Prepare | `439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2` |
| 01 | **신규 후보** | 전진 준비 / Windup | `5473314b0970a7a8e99f9e39f8e04f1fb7997ab0835ceb48db947071a19a4fe7` |
| 02 | **신규 후보** | 타격 직전 / Lunge | `89fc8493bb7f6602ceb1c24cb03aac9de1806679cd1cc21482568ee43fdf0d81` |
| 03 | 승인 원본 바이트 KEEP | Contact v2 | `0d0e24c2fae579f2f944255fe3bd3841216299b14f439cc3612906269020f697` |
| 04 | **신규 후보** | 반동 / Recoil | `9ec1b98f260fb436b8701f6ecd7cc131946111e81eb6e1302e01c51ec56d347c` |
| 05 | **신규 후보** | 복귀 진행 / Return | `c3f0f09b46a22fd0a3d3bf50eb878b3e3f29436decf62f4230f95555ca3fabfa` |
| 06 | 승인 원본 바이트 KEEP | Recover v2 | `0501a452467a07f34688abdfff5ea5ef49b0dfd93e1c99bdc368229986d44096` |
| 07 | **신규 후보** | Idle 연결 / Settle | `092c1dc5681d5736f7ae810c3fbce529c0922df4d26c74e249b3dbedc06e31d7` |

**승인·보호 기준** v5·Idle00 SHA `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`. 종전 승인 Walk8 ZIP `771bd1813608aa3e350c6124ab0d43293c6d2988b1833014da64484d5272c8da`은 이번에 편집/덮어쓰기하지 않음. 승인 Attack3 v2 원본 ZIP `804f8ebf85dbe7f7301cbfdecc6e85adc4e7d8888e23c3ee132bfca95854499b`도 보존.

## 3. 실제 제작법·미술 판정 한계

- 기존 3개 승인 PNG와 승인 Idle00을 소스 입력으로 사용하여 **양방향 밀집 광학 흐름 기반 포즈 간 픽셀 변위 + 프리멀티플라이 알파 리샘플링**으로 중간5장 PNG를 생성했다. 이전 시도에서 반투명 두 다리/눈이 겹치는 문제가 보였으므로 이중 소스 알파 블렌드는 폐기하고 **각 신규 프레임에서 하나의 원본 실루엣만 변형**하도록 정리함. (원본 3개는 아예 재인코딩하지 않고 바이트 복사.)
- **정확한 표현:** 신규 5장은 기존 프레임을 이름만 바꾼 반복 PNG가 아닌 **서로 다른 신규 래스터 중간 후보**이나, 다리8개를 각각 수작업 해부학적 관절로 재작화한 작품은 아님. 특히 02→03, 03→04, 06→07, 07→실제 Idle 전환에서 **피부·관절 텍스처 늘어남 및 발끝 순간이동** 가능성이 남는다. 본편 10FPS 미술/접지 PASS로 승격 금지.
- `WALK_CONTACT`, 실제 공격 Target Hit, 전열·후열 Collision, Root motion·PPU Pixel Density, 발 지면고정, 슬롯 간 자연스러운 10FPS 연속성, Attack→Idle 루프는 **NOT_VERIFIED**. 후보 스토리보드 슬롯03은 실제 피해/Hit 타임이라고 주장하지 않는다.
- 차기 최소 검수는 바울 대표가 128/40 10FPS 미리보기를 검토하고 **신규 5장 미술 승인 여부** 결정 → 필요한 경우 문제 슬롯만 독립 다리 관절 재작화 → Codex 격리 Unity에서 10FPS·접점/동일 PPU QA → 본게임 적용은 별도 승인. 반복적 단순 보간 수정만으로 완성 판단 금지.

## 4. 납품 장소 및 보호 자산

- 채팅 다운로드 장애를 피하려 **ChatGPT Library `/LIMITLESS_Art_Delivery/`에 신규 ZIP, 5PNG, 4×2 Sheet, 128·40px 10FPS GIF, 40px 시퀀스, 512px 시퀀스 등 총 11파일 업로드 후 목록 확인**. 사용자 Windows F: 자동 복사·GitHub 바이너리 Push는 하지 않음.
- 기존 승인 v5/Idle8/Walk8/Prepare00/Contactv2/Recoverv2, Chapter2 공식 몬스터·보스, Wolf/Bear/Fox, Owl, Chapter4 메카닉 Spider, VenomBee v3 `RIGHTS_HOLD`·소스 기록, Unity/Scene/Prefab/GUID/meta/MonsterDefinition/Spawn/AI/Quest/Save/UserData/`main` 변경0. **Shoot8 제작0, Unity 적용0, main 직접 커밋·Push·병합0**.
- 브랜치 `docs/art-renewal` **문서만 기록**, 대표 최종 미술 승인 필요: `ATTACK8_8PNG_TECH_PASS / NEW_5_ART_REVIEW_PENDING / TARGET_CONTACT_HOLD / NOT_UNITY_APPLIED`.
