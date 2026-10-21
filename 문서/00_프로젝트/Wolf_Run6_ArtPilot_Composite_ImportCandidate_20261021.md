# LIMITLESS — Wolf v5 승인 후 Run6 콤포지트 분리 / 격리 Import 후보 패키지 납품 (2026-10-21)

## 원본 보호 및 역할
- 사용자: "이걸 게임에 어떻게 넣니?" — 이전에 제공된 한 장짜리 늑대 6포즈 그림을 그대로 게임에 넣는 것이 불가능하여 **실제 import 검사 가능한 개별 PNG와 동일 셀 Strip**으로 분리.
- 기획 정본 `docs/planning/문서/00_프로젝트/20261021_Chapter12_Wolf_v5_Approved_Run6_ArtProduction_Order.md`, Issue #43 확인. 승인된 단일 포즈 원본 `Wolf_KeyArt_v3_AlphaFix_v5.png` SHA256 `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1` 실제 파일 해시 확인. **원본 v3/v4/v5 PNG 수정 0**. 기존 Wolf Run v1/v2 재활용 안 함; 이번 대화에서 새로 생성됐던 다중 포즈 원화를 분리했음.
- **중요한 진실:** 새 6포즈 래스터 원화는 이전 생성된 **콤포지트 그림의 개별 프레임**이고, 승인 v5의 시점/색감에서 착안했지만 v5 원화를 자체 리깅·보증된 Run6으로 변환한 것이 아님. 일부 포즈가 서로 겹쳐 분리 경로를 추정했으므로 **미술 합격·바로 Unity 본게임 연결 가능이라고 주장하지 않음**.

## 실제 패키지
- **`Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtPilot_v2_CANDIDATE.zip`** (실제 ChatGPT 샌드박스 파일)
- ZIP 크기 **2,482,392 bytes**, SHA256 **`c69ca17e8ebe2b8591c4dc77ebc6531b60dbe8a44ee3475d39d4cc6159f8f2ad`**, **29 entries**.
- `Assets/Poses/Wolf_Run_00.png`~`Wolf_Run_05.png`: 서로 다른 좌향 포즈 **6개 PNG, 각 512×384 RGBA**, 원본 스케일을 유지해 좌우/발 위치 정규화, 다른 포즈에서 유입된 큰 연결되지 않은 잔조각 제거.
- `Assets/Wolf_Run6_3072x384_RGBA.png`: 가로 6-cell Run Strip, 각 512×384. Unity MaxTextureSize **4096 이상** 안 하면 원본 자동 축소 위험.
- `Assets/Wolf_Run6_3x2_1536x768_RGBA.png`: Sprite Import MaxTextureSize 2048 이하에서도 원본 보존 가능하도록 대체 **3×2 Atlas** 제공.
- `Preview/Wolf_Run6_12FPS_128px.gif`, `Wolf_Run6_12FPS_Transparent.apng`: 6프레임 12FPS 0.5s loop; `Wolf_Run6_128px_ContactSheet.png`, 각 128px/40px 단일 프레임, 승인 v5 대비 비교.
- `Manifest.json` 소스 SHA/파일별 SHA256/Rects, `QA/Final_Delivery_QA.json`, `CODEX_IMPORT_QA_KO.md` Codex Import 지시.
- 바울 대표 F: 자동 복사/원본 Unity 변경/GitHub 바이너리 업로드 **없음**. 이 보고서만 담당 브랜치에 기록. 납품 ZIP은 사용자 대화 파일 링크로 제공.

## 오프라인 검사와 경고
- 실제 PNG6 Frame 해독, Strip의 동일 Rect 픽셀대조6/6, Atlas Rect 동일6/6, 프레임 RGBA 가장자리 알파 비투명 픽셀0, 프레임 SHA 중복0, ZIP 표준 CRC testzip PASS, ZIP 항목 바이트 재읽기 SHA 대조 PASS.
- **원화 품질 위험:** 콤포지트에서 접촉/교차한 동물 그림으로 인해 추출 경계 4→5 및 5→6 부근에서 약 36/40 지점의 알파 직교차가 있었음. 표면적으로 큰 잔편은 제거했으나 꼬리·발·털의 흐름이 본래 원화에서 정확히 복구된다고 보증할 수 없음. 일부 포즈의 몸 비례/모션과 루프 05→00 자연스러움은 실제 시각승인 전. 색상 프린지 등도 세부 정밀 검증 미완료.
- v5 KeyArt는 1포즈 정적 **대표 승인**, 이번 Run6 모션은 **대표 미술 승인 전**.
- **현 상태 `FORMAT_IMPORT_CANDIDATE_PNG6_ZIP_REAL / OFFLINE_INTEGRITY_PASS / ART_MOTION_VISUAL_HOLD / UNITY_WORLD_SCALE_UNVERIFIED / NO_OFFICIAL_RUNTIME_CHANGE`**.

## Codex 다음 격리 Import 검수 계약 (실행은 승인 후)
- 별도 `codex/*` worktree 신규 GUID로 import, `Sprite(2D and UI)`/`Multiple`; 512×384 6셀, LEFT, 12FPS, Loop. 후보 pivot `(.5,.14)`; 정식 PPU/World Scale은 지금 강제 지정 불가 — 기존 64×40×6/12FPS/좌향, icon 0-based index4, legacy 2x display와 동일 게임월드 표시 크기 매핑 후 확정.
- 기존 `Resources/CompanionAssaultValidation/Wolf_Run`과 기존 펫 시스템/동료의 습격/세린/Save·UserData, v5 원본 등 **덮어쓰기 금지**. 새로운 sprite 후보에서 접지, 05→00 점프, 128px/40px/3화면비, UI visibility와 alpha/loop를 **미술 Delta**만 검사하고 리포트. 정식 Runtime 적용은 사용자 Run6 미술 승인 후 별도 승인.
- Snake 모션/Bear/Fox/Chapter3/Field09 및 main 직접 commit·Push·병합 금지.
