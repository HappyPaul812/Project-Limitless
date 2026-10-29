# LIMITLESS / Main24 F2 / Issue #75 — 표지판 가상글자·횃대 원판 표적 수정 (2026-10-29)

**브랜치**: `docs/art-new`  
**최종 상태**: `F2_P2_ART_CANDIDATES / OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / PPU_PIVOT_SPEC_PENDING / NOT_UNITY_APPLIED`

## 근거
- [2026-10-29 Codex 통합 검수](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6492706191): 나뭇가지 `KEEP_CANDIDATE`, 눈발 `KEEP_CANDIDATE`, 기존 표지판의 무의미 문자와 그루터기/횃대의 스툴처럼 보이는 인공 원판이 표적 결함. 기존 LowFence v2 접지는 별도 `ART_HOLD`.
- [#75 기획 후속](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6493037627): 수정 후보 최대2 PNG, 화살표 방향 유지/없는 목적지 문구 발명 금지, 자연 나무 단면, 원본 KEEP SHA 유지, 실 Unity/통행 미검증.

## 실제 파일 납품
- 입력 `F2_P2_Environment_4Assets_20261029.zip` **원본 SHA256 `d749d7fd2937849326e9ecc3ca9f9e83f5646447e27d21fd1c3206ea0f650088`**, 재확인. 구 환경 4개 **원본 수정0**.
- 출력 `F2_P2_Targeted_Sign_Stump_20261029.zip`
- **ZIP SHA256 `436e5e839b86f66ec09c882852fcae28ab47aacc4eef11b254139d90192810f8`**
- 크기 **463,633 bytes / 5 entries**, ZIP CRC PASS 및 SHA256SUMS **4/4 PASS**. 현재 ChatGPT 대화 컨테이너 `/mnt/data/F2_P2_Targeted_Sign_Stump_20261029.zip`에서 납품. GitHub에 PNG/ZIP 바이너리 업로드0, 사용자 PC F:\ 저장·SHA 검증 미실시.
- **신규 후보 PNG 2종만**:
  1. `Candidate/F2_FirstSnowTrailSign_v2_CANDIDATE.png` **288×384 RGBA**, SHA256 `987b2d8e597d8d77feeb5c75910c5d1b0a0a18d08cbddbc760bc24187fba39ae`. 기존 우/좌 방향 나무 화살표 본체·눈지붕·지지 기둥 유지, 무의미한 가상문자를 국소적으로 제거하여 목재 텍스처로 채움. **변경 3,571px, 변경 bbox (91,135)-(209,214), alpha 변경0**. 공식 목적지 글자·새 기호 발명0. 새 목재 메움 부분의 색면/결이 주변보다 달라 보일 수 있어 **최종 미술 REVIEW 필요**.
  2. `Candidate/F2_SnowStumpPerch_v2_CANDIDATE.png` **320×384 RGBA**, SHA256 `cef5ca388730c7e86242997401616afbc925fd42d270158bc2ce6b878400a71d`. 오른쪽 횃대의 인공 평평한 원판을 불규칙 수피 윤곽과 나이테가 보이는 나무 단면·부분 적설로 대체. **변경 2,004px, bbox (196,157)-(271,192), alpha 변경1,178px**. 기존 왼쪽 눈 덮인 그루터기/접지 장식/나뭇가지, 기둥 하부 모두 KEEP. 단면 선의 픽셀·기존 원화 음영 통일은 미술 검토 대기.
- KEEP SHA `Candidate/F2_FallenSnowBranch_v1_CANDIDATE.png` **`b05856bb255b6072f0ec91987cfb07a17a464ff60460fde0b156bdda0e4318e0`**, `Candidate/F2_GentleFirstSnow_Overlay_v1_CANDIDATE.png` **`3a57f2c936bea6a07f4d80d3e49f0dbce7b8df9398fc33244d1c48a999702bf9`**. 실제 입력 원본 그대로 두고 새 ZIP에 재등록·복제하지 않음; 원본 기타 PNG도 변경0.
- `Preview/OnePage_F2P2_BeforeAfter_OFFLINE.png`은 실제 기존 vs 새 후보를 1장에 비교, `Manifest.json`은 신규파일 SHA와 기존 원본 SHA·`PPU/Pivot SPEC_PENDING` 확인. 128/40 게임 크기/실 휠체어 이동 확인은 이번 실제 Unity QA 미실시 — 이미지 리샘플은 실제 게임렌더와 다름.

## 보호·검수·후속
- **파일 기술 QA**: PNG2/2 decode·투명 RGBA, 알파 및 변경 바운딩, CRC PASS, SUMS4/4 PASS.
- **미술 승인**: 가상 글자 제거 후 목재 표면의 질감, 횃대 단면이 자연스러운지 **바울 대표 시각 승인 필요**. 파일 규격 PASS가 미술 PASS는 아님.
- **Unity 미검증**: 실제 PPU/Pivot/Collider/Sorting·표지판 목적지·휠체어 통행 공간·보행로 차폐, 16:9/4:3/21:9 검수 `SPEC_PENDING / NOT_RUN`.
- 기존 P1 눈 오버레이·소나무·서리 관목·LowFence v2 ART_HOLD, 메인23 승인 환경43·지면9, F1, Frost Owl 원화/애니 및 공식 몬스터·펫·리뉴얼 Spider/Bee·오르나/Residence/Inn, Scene/Quest/Save/UserData/Unity 원본·Main24 기능 변경0. 본게임 적용·main 직접 commit/Push/merge0.

**상태**: `F2_P2_KEEP_2 / TARGETED_REVISE_2 / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`.
