## 2026-11-04 / Issue #90 — Attack8 02·04·05·07 4프레임 국소보완 납품 (최신 증분)

- [신규 표적수정 실물·SHA 보고](../../Renewal_20261104_Issue90_Attack8_4Frame_TargetedArt_v2_Delivery.md), **`docs/art-renewal` commit `ac917c83544003799cabe97ac6ed4052abee64ec`**. [#90 최신 총괄 4프레임 발주](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6622866401) 및 이전 10FPS QA 기준. **승인 00·03·06과 정상 01 바이트 exact KEEP**. 원본 v5·Idle8·Walk8 재작업0.
- **최종 통합 ZIP** `LIMITLESS_ATTACK8_ISSUE90_TARGETED_4FRAME_FIX_20261104.zip`: **6,023,162bytes / SHA256 `58c51cb49d63cde4669dc183da8583701e7e0bc9dceae9fae5542a5573f0fc9d` / 26entries / CRC PASS / Manifest 24/24 SHA PASS**. 4개 PNG만의 미니ZIP **796,853bytes / SHA256 `e7dac2ab05ce807aa8a9a1fdb0d82b3125007f3ec8eca9427b8f86d9beac085d`**. 두 ZIP + 4PNG + 128·40px 10FPS GIF + 128px 전후/3x관절 확대판 합계10개 ChatGPT Library `/LIMITLESS_Art_Delivery/` 업로드 확인; 사용자 Windows F: 자동복사 아님.
- **신규 4 PNG 512 RGBA / 새 SHA/변경 픽셀:** slot02 `85ca201cecb4b51acad26b39f5146fe6637c8408dd08b02eb2723f0c65193622` (14,992px), slot04 `3ccdfa06022ea900c60857211db81042f44489719e6235a886c60d351d5ead7a` (3,587px), slot05 `7ed984d56c41f284ad8fc8646275d674f7a6e533649b41644f0baa5c42d15bde` (44,406px), slot07 `35af96b928108206488b1b27bdcf607d3f9f890bc0d3b7df2a16d4a932a6955e` (10,604px).
- **표적 델타:** 02 전방 다리/관절 국소 변위; 04 우측 분리 가시알파31px 삭제; 05 좌측 분리 3+14+17=34px 삭제·몸통 복귀 국소 정렬; 07 우측 분리7px 삭제 및 무릎 정렬. 지정 ROI에 작은 분리 가시알파 재검출0. 00·01·03·06 바이트·기존 SHA 전부 동일, 임의 재작화 없음. 4×2 전체8 시트, 512/128/40 비교 및 10FPS 전후8×100ms GIF 포함.
- **미술 QA 한계:** 04→05 실루엣 IoU 기존 약0.720 → 수정 후 약0.723 (소폭 변화). PNG/SHA/CRC/타이밍 기술 PASS는 **실제 8다리 수작업 관절 IK·지지발 물리 고정·Attack 타깃 접촉 PASS가 아님**. 기법은 기존 이미지의 제한영역 국소 픽셀 리매핑+알파 정리. `USER_FINAL_MOTION_ART_REVIEW_PENDING / FOOT_LOCK_NOT_VERIFIED / UNITY_NOT_TESTED` 유지. 필요시 04→05만 수작업 해부학적 교정.
- **보호:** Shoot8 미제작, VenomBee v3 RIGHTS_HOLD, 승인 Owl·Ch2 공식 몬스터/펫·Chapter4 메카닉 Spider, Unity 6000.6.5f1 원본/Save/UserData/main 변경0, Push·병합·본편 적용 금지.

---

## 2026-11-04 / Issue #90 / Attack8 10FPS 오프라인 관절·발접지 미술 검수 (현재 최신)

- **정식 프레임별 검수 보고:** [Renewal_20261104_Issue90_Attack8_10FPS_OpticalFlow_Visual_QA.md](../../Renewal_20261104_Issue90_Attack8_10FPS_OpticalFlow_Visual_QA.md), `docs/art-renewal` commit **`f3694ca0a390153277b6330b6470382dc10b51ca`**. 최신 [#90 총괄 기획 검수](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6622459940)와 실 ZIP 사본 직접 대조. **Attack8 기존 ZIP** `LIMITLESS_SPIDER_ATTACK8_ISSUE90_20261104.zip` **5,922,807bytes/SHA `e612a88a6d4c6b4422cc025c0d32fb34e4ffc2f40337ffa909a1885cfae34041` CRC PASS**. 512 RGBA8·128/40 GIF 각8×100ms 진짜10FPS 확인.
- **보호:** 승인 **00 Prepare / 03 Contactv2 / 06 Recoverv2는 BYTE_KEEP**, 원본 Organic v5·Idle8·Walk8·공식 아트·펫·Unity·Save/UserData·main **변경0**. 신규 PNG 생산0, 기존 Attack8 프레임 수정0. Shoot8/Bee 수정0, Bee RIGHTS_HOLD 유지.
- **프레임 판정:** 신규 **01 Windup `KEEP_CANDIDATE`**. 신규 **02 Lunge `ART_REVISE`**(오른쪽 앞다리 관절 얇아지고 실루엣 10.8% 축소·x502까지 전개). 신규 **04 Recoil `ART_REVISE`**(오른쪽 다리 끝 분리 알파31px x491..496/y278..286). 신규 **05 Return `ART_REVISE_PRIORITY`**(왼쪽 다리 옆 작은 분리 조각 17+14+3px, 04→05 알파실루엣 IoU.720/무게중심 약 +11x,-7y 변화). 신규 **07 IdleBridge `ART_REVISE`**(오른쪽 지지다리 분리 알파7px x459..461/y309..314 및 승인 Idle 복귀 연결 주의).
- **그림/게임의 구분:** 07→Idle00 오프라인 실루엣 겹침 IoU.752, GIF 자동 루프 07→Attack00 IoU.651이지만 **GIF 루프를 실제 Runtime 전환으로 간주하지 않음**. 위 수치들은 alpha 중심/실루엣이지 발관절·Pivot·지면 접점이 아님. **`FOOT_LOCK_NOT_VERIFIED / TARGET_CONTACT_NOT_VERIFIED / UNITY_RUNTIME_NOT_TESTED`**.
- **오프라인 검수 증거** `LIMITLESS_ATTACK8_ISSUE90_MOTION_QA_20261104.zip` 295,072bytes/SHA256 `045fed92f86b50b2b4ea1eda3b95b397fa1219171c7702846934daa7fa63b2e8` / CRC PASS; 프레임/부위 확대·10FPS 원본 GIF·SHA/실루엣 CSV/JSON·리뷰문서(새 게임 아트0). 새 4프레임은 **아직 제작한 것이 아니라 수정 대상으로만 지정**.
- **최종:** `ATTACK8_FILE_TECH_PASS / USER_MOTION_ART_REVIEW_PENDING / KEEP_00_03_06 / NEW01_KEEP_CANDIDATE / REVISE_ONLY_02_04_05_07 / NOT_UNITY_APPLIED`. 승인3장 보존 후 문제가 있는 네 프레임만 부분 재작화→격리 모션 QA→대표 최종 승인이 다음 순서. Attack8 전체 재양산 및 Shoot8/본편 적용 금지.

---

## 2026-11-04 / Issue #90 / Attack8 신규5 중간 프레임 제출 (현재 최신)

- **정식 실물 보고서:** [Renewal_20261104_Issue90_OrganicSpider_Attack8_5Intermediate_Pilot_Delivery.md](../../Renewal_20261104_Issue90_OrganicSpider_Attack8_5Intermediate_Pilot_Delivery.md) / `docs/art-renewal` **commit `d22bec7e19b0cd986ba46484ba18c7fa111c97c1`**. 근거: [Codex 승인3 SHA확인](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6621574553) 및 [기획 신규5장 발주](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6621779059).
- **ZIP 실제 납품:** `LIMITLESS_SPIDER_ATTACK8_ISSUE90_20261104.zip`, **5,922,807 bytes / SHA256 `e612a88a6d4c6b4422cc025c0d32fb34e4ffc2f40337ffa909a1885cfae3404` / 23entries / CRC PASS / Manifest21/21 SHA+bytes PASS**. Library `/LIMITLESS_Art_Delivery/`에 ZIP·새5PNG·4×2 Sheet·128/40px 10FPS GIF·512/40 비교판 총11파일 업로드·검증. F: 자동 다운로드·GitHub 바이너리 Push0.
- **8슬롯 확정은 오프라인 후보 한정:** **00 Prepare 승인 KEEP** SHA `439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2`; 01 NEW `5473314b0970a7a8e99f9e39f8e04f1fb7997ab0835ceb48db947071a19a4fe7`; 02 NEW `89fc8493bb7f6602ceb1c24cb03aac9de1806679cd1cc21482568ee43fdf0d81`; **03 Contactv2 승인 KEEP** `0d0e24c2fae579f2f944255fe3bd3841216299b14f439cc3612906269020f697`; 04 NEW `9ec1b98f260fb436b8701f6ecd7cc131946111e81eb6e1302e01c51ec56d347c`; 05 NEW `c3f0f09b46a22fd0a3d3bf50eb878b3e3f29436decf62f4230f95555ca3fabfa`; **06 Recoverv2 승인 KEEP** `0501a452467a07f34688abdfff5ea5ef49b0dfd93e1c99bdc368229986d44096`; 07 NEW `092c1dc5681d5736f7ae810c3fbce529c0922df4d26c74e249b3dbedc06e31d7`. 승인3 파일은 바이트 복사/재인코딩 0.
- **파일 QA:** 전체8 PNG 512×512 RGBA, 4×2 시트 2048×1024, 각 8슬롯 100ms 10FPS 미리보기, 40px 동배율 시퀀스. PNG8 SHA 모두 별개, 가시 알파 네변 0. 신규5장만 광학흐름 기반 중간 래스터 포즈 변위(명백한 이미지 중복 아님); **개별 8다리 수작업 관절 작화·FootLock·TargetContact·Unity GameView/10FPS 모션 품질은 NOT_VERIFIED**. 특히 02→03/03→04/06→07/07→Idle 연속성은 대표 미술 QA 필요.
- **보호:** 승인 v5/Idle8/Walk8/3키포즈/Shoot8 미제작, Bee v3 RIGHTS_HOLD, Owl·Ch2공식/펫·기존 Unity·Save/UserData/main 변경0. `ATTACK8_8FILES_OFFLINE_TECH_PASS / NEW5_USER_MOTION_ART_REVIEW_PENDING / ACTUAL_CONTACT_HOLD / NOT_UNITY_APPLIED`. **완성 게임 애니메이션 승인이나 본편 적용을 뜻하지 않음**.

---

## 2026-11-04 / Issue #90 — Contact01·Recover02 표적 수정 v2 (현재 최신)

- **정식 보고서:** [Renewal_20261104_Issue90_AttackContactRecover_Targeted_v2_Delivery.md](../../Renewal_20261104_Issue90_AttackContactRecover_Targeted_v2_Delivery.md) / `docs/art-renewal` commit **`157b432ac2960250091da350db0c9c0008543aed`**. 근거: [#90 Codex F: 좌표/관절 검증](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6620212147), [기획 최소 보완 지시](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6620386396).
- **실 ZIP** `LIMITLESS_SPIDER_ATTACK3_TARGETED_ISSUE90_v2.zip` **1,719,271 bytes / 12 entries / CRC PASS / Manifest 10/10 SHA PASS / ZIP SHA256 `804f8ebf85dbe7f7301cbfdecc6e85adc4e7d8888e23c3ee132bfca95854499b`**. ChatGPT 라이브러리 `/LIMITLESS_Art_Delivery/`에 ZIP·두 PNG·512/128/40 비교판·관절 연결/8슬롯 자료 8파일 업로드 성공. 사용자 F: 자동 저장/원격 바이너리 Push0.
- **신규 Contact01** 512 RGBA SHA256 `0d0e24c2fae579f2f944255fe3bd3841216299b14f439cc3612906269020f697` (기존 `4a4d4418...` 대비 **RGBA 47,381픽셀 변경**). 두부 각도·복부 높이·오른쪽 전방 어깨/무릎/발끝을 국소 조정. **신규 Recover02** 512 RGBA SHA `0501a452467a07f34688abdfff5ea5ef49b0dfd93e1c99bdc368229986d44096` (기존 `9fad6682...` 대비 **54,747픽셀 변경**), 복부 상단 복귀 높이/두부/지지다리 국소 보정. 2개 모두 가시 알파 네 변 접촉0.
- **원본 보호:** 기존 Prepare00 SHA `439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2`, 승인 v5/Idle00 SHA `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`, Walk07 SHA `edf725d87aafafcb81d01f09b121bf1e20a71b4fbfd829dbd1b0b6a3abf1b189`, 기존 ZIP SHA `91411d15...` 그대로 KEEP. 승인 Idle8/Walk8 보호, Owl·Chapter2 공식/펫·Chapter4 메카닉·VenomBee v3 RIGHTS_HOLD·Unity/Save/UserData/`main` 변경0.
- **정확 판정:** `TWO_NEW_DELTA_PNGS_TECH_PASS / STATIC_SHA_CRC_PASS / PREPARE_KEEP / JOINT_CONTINUITY_ART_REVIEW_PENDING / FOOT_LOCK_NOT_VERIFIED / TARGET_CONTACT_NOT_VERIFIED / USER_VISUAL_REVIEW_PENDING / ATTACK8_5_NOT_PRODUCED / SHOOT8_NOT_STARTED / NOT_UNITY_APPLIED`. 국소 픽셀 리매핑 후보이며 수작업 독립 관절 재작화/실전 접지 PASS가 아님. `10FPS/PPU256/scale0.7`은 기존 계약 참조, Attack8 실제 재생 테스트0. 대표 미술 승인 전 확대 제작/본편 교체 금지.

---

# WORLD_ART_RENEWAL / 2026-11-04 최신 증분 인계 — Issue #90 Organic ForestSpider Attack3

**2026-11-04 최신 증분:** 기존 `WORLD_ART_RENEWAL_LATEST.md`의 102KB 이상 기존 승인·QA 이력은 덮어쓰지 않고 그대로 보호. 이 문서를 Issue #90 후속 인계로 사용하며, 기존 인계 본문의 직접 수정이 완료됐다고 주장하지 않는다.

- **정식 보고서:** [Renewal_20261104_OrganicForestSpider_Attack3_Issue90_ArtPilot.md](../../Renewal_20261104_OrganicForestSpider_Attack3_Issue90_ArtPilot.md) · `docs/art-renewal` commit **`14495ecefc7d7db291e8389262c0807c97b2dc93`**.
- **실 납품 ZIP:** `LIMITLESS_SPIDER_ATTACK3_ISSUE90.zip` · **4,122,683 bytes / SHA256 `91411d159d401f1772e80e848d78550dd62552f65c1a76d659c15d8b0df44d3e` / 21 entries / ZIP CRC PASS / Manifest 19/19 SHA PASS**. ZIP·PNG3·512 비교판·128/40 비교판·8슬롯 도식 총7개를 ChatGPT 라이브러리 **`/LIMITLESS_Art_Delivery/`** 에 직접 업로드 성공. 과거 채팅 다운로드 장애를 고려한 별도 전달 방식. 바울 대표 PC `F:\Downloads`에는 자동 저장되지 않음.
- **신규 PNG3:** Prepare SHA **`439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2`**; Contact SHA **`4a4d4418590db7d3b0f4eeb32dc79bb620822991ab4d08c978f10e538307de04`**; Recover SHA **`9fad6682201222f90e8f908d3c4eb497012c9f567cf40504dc19d080c71609d6`**. 각 512×512 RGBA·가시 알파 캔버스4변 접촉0. 실제 128/40 축소 비교, 별도 512 기준 비교 및 `SourceDeclaration.md` 기록.
- **절대 보호 승인 자산:** Organic ForestSpider v5 SHA **`c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`** / 승인 Idle8·Walk8(기존 Walk8 ZIP `771bd1813608aa3e350c6124ab0d43293c6d2988b1833014da64484d5272c8da`) / Ch2 공식 몬스터·펫/Chapter4 메카닉 후보/승인 Owl 모두 변경0. 원래 레퍼런스 v5·Idle00·Walk07을 ZIP 내부에 SHA 동일 참조 복사만 하고 원본을 덮어쓰지 않음.
- **기술 계약** 기존 ForestSpider 10FPS·PPU256·scale0.7·우향. 3개 새 키포즈는 별도 생성 2172×724 3포즈 원화의 connected-alpha 추출·비율유지 리샘플 후보이며 **모델 동일성/8다리 독립 관절/실제 접지/타깃 접촉 미술 최종 PASS가 아님**. 8-slot 지도는 Prepare00, Contact03, Recover06만 제작, 나머지 5개 **NOT_PRODUCED**. Attack8 전체·Shoot8 새 프레임0.
- **VenomBee v3** 기존 2포즈 및 20프레임/Prepare/Idle2 불변, `RIGHTS_HOLD / TARGET_CONTACT_NOT_VERIFIED` 유지. Unity/Scene/Prefab/Sprite GUID/meta·AI/Spawn·Save/UserData·main 직접 변경/Push/병합0. 본게임 적용0. Codex 기존 #84/#85/#63 대규모 QA 반복0.
- **현재 상태:** `ART_PILOT_3_POSES / PNG_HASH_CRC_TECH_PASS / USER_VISUAL_REVIEW_PENDING / ATTACK8_NOT_COMPLETE / SHOOT8_NOT_STARTED / CONTACT_NOT_VERIFIED / NOT_UNITY_APPLIED`.
- **다음 결정권:** 바울 대표가 세 포즈의 실제 비교판으로 미술 승인을 판단. 승인 전 나머지 Attack5/8 또는 Shoot8 제작과 Unity Import 금지.