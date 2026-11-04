# LIMITLESS / 리뉴얼 아트 / 2026-11-04 — ForestSpider Attack8 4프레임만 국소보완 (Issue #90)

**담당 브랜치:** `docs/art-renewal` · **실제 신규 PNG 4장만** · **원본 Unity 미적용 / 사용자 모션 미술 검토 대기**.

## 1. 변경 근거와 범위

- [Issue #90 최신 총괄 4프레임 제작 발주](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6622866401) 및 [기존 10FPS Motion QA](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6622626595) 기준.
- 새로 수정한 슬롯은 **02 Lunge, 04 Recoil, 05 Return, 07 IdleBridge**만.
- **원형 보존/바이트 exact KEEP:** 00 Prepare, 03 Contact01v2, 06 Recover02v2, 01 Windup. 승인 Organic v5·Idle8·Walk8·기존 Attack8 ZIP/공식 몬스터·펫·Owl·Ch4 기계형 Spider·VenomBee v3 `RIGHTS_HOLD`, Unity 6000.6.5f1 / Scene/Prefab/GUID/meta/Save/UserData/몬스터 기능 및 `main` 원형.
- 실제 F: 디스크 경로를 직접 다루지 않고, 이전 납품 `LIMITLESS_SPIDER_ATTACK8_ISSUE90_20261104.zip` ChatGPT Library 보관본 SHA **`e612a88a6d4c6b4422cc025c0d32fb34e4ffc2f40337ffa909a1885cfae34041`**의 8장 사용.

## 2. 새 납품물·무결성

- **ZIP** `LIMITLESS_ATTACK8_ISSUE90_TARGETED_4FRAME_FIX_20261104.zip` = **6,023,162 bytes, SHA256 `58c51cb49d63cde4669dc183da8583701e7e0bc9dceae9fae5542a5573f0fc9d`, 26 entries**, ZIP CRC PASS, Manifest **24/24 내부 페이로드 SHA+bytes 일치**.
- **최소 PNG용 별도 ZIP** `LIMITLESS_ATTACK8_ISSUE90_FOUR_REVISED_PNG_ONLY.zip` = **796,853 bytes, SHA256 `e7dac2ab05ce807aa8a9a1fdb0d82b3125007f3ec8eca9427b8f86d9beac085d`**, 수정 PNG4장+Manifest/CRC PASS.
- 새 이미지 **4개**는 `ChangedOnly/ForestSpider_Attack_{02,04,05,07}_512_RGBA.png`. 각각 512×512 RGBA. 전체8 완성 참고 시트 `Attack8_4x2_512_RGBA.png`(2048×1024, 4열×2행) 및 기존/수정 비교용 GIF: `Preview/Attack8_{BEFORE,AFTER}_{128,40}px_10FPS.gif`, **각 8프레임 × 100ms, 정확히 10FPS**. 시트에서 고정된 4장도 정확한 원본 바이트를 썼으며 **그 PNG를 재작화하지 않음**.
- `Evidence/Targeted_4Frame_BeforeAfter_{512,128,40}px.png`, `Evidence/FourFrame_LegJoint_Foot_ROI_BeforeAfter_3x.png`, `Evidence/QA.json`, `Evidence/BeforeAfter_SHA.csv`, `Manifest.json`, `SHA256SUMS.txt`, `Evidence/SourceDeclaration.md` 포함.
- **다운로드 안정성:** `/LIMITLESS_Art_Delivery/` **ChatGPT Library에 통합 ZIP + 4PNG ZIP + 개별4PNG + 수정 10FPS GIF2종 + 128px전후/3배 확대판 총10개 직접 저장 성공.** 사용자 Windows `F:\Downloads` 자동 복사나 GitHub PNG/ZIP 바이너리 Push는 **하지 않음**.

### 2-1. 파일 SHA256·실제 픽셀 변경

| 슬롯 | 새 PNG SHA256 | 이전 대비 변경 RGBA 픽셀 | 기술 처리 |
|---|---|---:|---|
| **02 Lunge** | `85ca201cecb4b51acad26b39f5146fe6637c8408dd08b02eb2723f0c65193622` | **14,992** | 오른쪽 앞다리/관절·발끝 국소 픽셀 변위 |
| **04 Recoil** | `3ccdfa06022ea900c60857211db81042f44489719e6235a886c60d351d5ead7a` | **3,587** | 우측 x491..496 y278..286의 분리된 가시 알파 **31px 제거**, 우상부 정렬 |
| **05 Return** | `7ed984d56c41f284ad8fc8646275d674f7a6e533649b41644f0baa5c42d15bde` | **44,406** | 왼쪽 x78..86 y308..330 분리 가시 알파 **3+14+17 = 34px 제거**, 왼쪽 지지다리 무릎 및 몸통 복귀 영역 국소 변위 |
| **07 IdleBridge** | `35af96b928108206488b1b27bdcf607d3f9f890bc0d3b7df2a16d4a932a6955e` | **10,604** | 오른쪽 x459..461 y309..314의 분리 가시 알파 **7px 제거**, 지지다리 무릎부/복귀 연결 국소 변위 |

- 분리 알파는 연결요소(가시 alpha>8) 기준 원본의 **31·34·7px**를 목표부위에서 제거했고, 새 파일에서 해당 ROI에 **작은 독립 가시 알파 조각 0** 재검사.
- **정확한 바이트 보호 SHA:**
  - slot00 Prepare `439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2`
  - slot01 Windup `5473314b0970a7a8e99f9e39f8e04f1fb7997ab0835ceb48db947071a19a4fe7`
  - slot03 Contact v2 `0d0e24c2fae579f2f944255fe3bd3841216299b14f439cc3612906269020f697`
  - slot06 Recover v2 `0501a452467a07f34688abdfff5ea5ef49b0dfd93e1c99bdc368229986d44096`.
  - 승인 정지 v5 `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`: 별도 원본 변경 없이 보호.

## 3. 미술 품질 및 미완료 항목 (PASS 과장 금지)

- **정확 제작 기법:** 네 기존 PNG의 문제 위치에 한정한 **부드러운 위치별 래스터 리매핑 + 알파 분리 섬 제거**. 이전 대화에서 ImageGen으로 생성된 다른 거미 그림을 **통째로 대체 사용하지 않았음**. 따라서 모델의 색감·기본 실루엣은 보존하나, **수작업 해부학적 다리 재작화·8다리 IK·실제 지지발 고정까지 완료된 것은 아님**.
- 04→05 실루엣 IoU 기존 약 **0.720**, 수정 후 약 **0.723**. **소폭 개선**으로 이미지 변형 점프/바닥 미끄러짐이 실제 사라졌다고 단정 불가.
- 07→Idle의 실제 게임 종료 트랜지션, PPU256/visualScale0.7 최종 표시, Hit/공격접촉·지면 발끝 고정·Unity GameView 검증 **미실행**. 10FPS GIF 파일 검증은 **OFFLINE TECH PASS**일 뿐 최종 모션 미술 PASS가 아님.
- 최종 **`TARGETED_4_PNG_DELTA_PASS / 00_01_03_06_BYTE_KEEP / ALPHA_ISLANDS_CLEAR / GIF_10FPS_PASS / ART_GAIT_REVIEW_PENDING / FOOT_LOCK_NOT_VERIFIED / ATTACK_CONTACT_NOT_VERIFIED / USER_FINAL_ART_APPROVAL_REQUIRED / UNITY_NOT_APPLIED`**.
- **다음:** 바울 대표가 수정 40px/128px 움직임과 3x 부위 전후를 직접 판단 → 필요 시 04→05 연결만 별도 해부학적 작화 재검토 → 최종 미술 승인 후에만 Codex 격리 Unity 6000.6.5f1 Clip/Hit/PPU 접지 QA. Shoot8 제작 및 본편 적용/Push/병합 금지.

## 4. 버전·보호 확인

Chapter1·2 승인 자산 및 Chapter4 메카닉 Spider, Bee v3 `RIGHTS_HOLD`는 수정0. 원본 Unity 6000.6.5f1 프로젝트/Scene/Prefab/MonsterDefinition/Spawn/AI/독/드롭/GUID/meta, Save/UserData, GitHub `main` 변경0. GitHub는 `docs/art-renewal` 문서 커밋만.
