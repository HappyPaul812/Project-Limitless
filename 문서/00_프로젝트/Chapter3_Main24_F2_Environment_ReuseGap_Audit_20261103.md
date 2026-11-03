# LIMITLESS / Chapter3 Main24 F2 환경 마감 — 실제 격리 결과 대조·기존 자산 재사용 감사 (2026-11-03)

**담당:** 메인 신규 아트 / `docs/art-new`  
**관련 Issue:** [#71](https://github.com/HappyPaul812/Project-Limitless/issues/71) · [#74](https://github.com/HappyPaul812/Project-Limitless/issues/74) · [#75](https://github.com/HappyPaul812/Project-Limitless/issues/75) · [#83](https://github.com/HappyPaul812/Project-Limitless/issues/83) · [#85](https://github.com/HappyPaul812/Project-Limitless/issues/85)  
**최신 현재 상태(실제 화면을 열람한 Codex 결과 접수):** `CODEX_F2_RT_READONLY_AUDIT_REPORTED / PLACEHOLDERS_CONFIRMED / EXISTING_ASSET_REUSE_OK / NEW_GAME_PNG_REQUIRED_0 / REPLACEMENT_VISUAL_QA_PENDING / SIGN_FENCE_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED`



## 0-1. Codex 기존 아트 재사용 격리 Unity 68 PASS 접수 — 2026-11-03

**근거:** [Issue #83 Codex 실 QA](https://github.com/HappyPaul812/Project-Limitless/issues/83#issuecomment-6603754190) · [Issue #85 동일 결과](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6603755245). Codex 실제 브랜치 `codex/f2-environment-reuse-delta-20261103`, **로컬 commit `f6797d7ae8e744f0e22ac760df7778551188bb90`**. **Unity 6000.6.5f1 격리 PlayMode 변경분 68 PASS / 0 FAIL, Runtime 0 오류**. 기존 #83 90 / #85 81 / 저장 QA 재실행하지 않음. 본 신규 아트 담당은 Codex가 보고한 결과를 인수하였고 실제 RT 18장 실물을 이 채팅에서 직접 확인하지 못했으므로 현장 미술 최종 합격으로 표시하지 않는다.

**실제 격리 후보 구성**: Frostway 공식 지면9·NorthF1 `WornTrail`·기존 F2 P1 GroundOverlay/SnowPine/FrostShrub로 **GROUND/PATH/TREE placeholder의 표시가 0**이 됨. 새 PNG 0, 비균일 스케일0, 원본파일/원본 Scene/Prefab/BuildSettings/Save/UserData/main 불변. UV 반복·회전·메시 알파로 길경계 완화, 기존 Collider 보존, **4.5WU 실제 Rigidbody2D 도보·휠체어 왕복** 통과. 나무 8개 PPU128·Scale1·알파 발 Pivot/발기준 Sorting, 관목4개 통로밖, 승인 Stump/Owl 보존. HUD 원래 폭304/글씨14/버튼12와 콜백을 유지한 좌하단·우상단 배치 후보에서 남쪽길 및 OwlSprite 가림0; **장식 나무 일부 HUD 뒤 가림은 디자인 판단 대기**.

**실증 산출:** South(0,-7), Bend(2,0), North(3,5) × 960/1280/1680×720 동일 Camera/Player/ortho5 **전후 Camera RT 18장·9쌍**. 단일 오프라인 리뷰 `F:/study/codex/Project-Limitless/Temp/F2EnvironmentReuse20261103/Review/index.html`, SHA256 `577ee06e630fe613f1930e8f79aee65faa7d5e9c733f4e35695b2feafafee325`; 실제 RT 증거 파일은 `.../QA/Evidence/`. OS GameView나 사람 직접 입력 확인 아님. HTML 내부 이미지 decode/JS 문법 PASS, 브라우저 실제 조작 `NOT_VERIFIED`.

**남은 미술 판단(1280×720 After_Bend 좌상단 기준):**
- **눈패치 반복:** x180–350 / y100–650 (170×550px) — 기존 텍스처 배치·반복/밀도·회전 다양화로 해결 우선, **별도 새 PNG 발주 근거 없음**.
- **길 굽음 질감:** x480–880 / y180–510 (400×330px) — 곡선/경계/시각 폭을 실제 4.5WU Collider와 비교하면서 기존 UV/경계 알파 조정 우선.
- **HUD 뒤 배경:** x964–1268 / y90–326 (304×236px) — 좌하단·우상단 후보의 남쪽길 차폐0 결과를 유지하면서 장식나무 일부 가림이 수용 가능한지 **대표 시각 판단**. HUD 수정 자체는 UI/기획 담당의 별도 격리 작업이고 신규 아트 PNG 제작 과제는 아님.

**상태·보호:** `F2_REUSE_ISOLATED_UNITY_TECH_68_PASS_0_FAIL / NEW_GAME_PNG_REQUIRED_0 / VISUAL_SNOW_PATH_HUD_USER_REVIEW_PENDING / NOT_ORIGINAL_APPLIED / REMOTE_NOT_PUSHED`. Sign/Fence FULLREDRAW `KEEP_CANDIDATE` 미승인, 승인 Owl v3 Idle8·Attack4 / Stump NaturalBroken v4·지면9·환경43 동결, 한기 VFX 미승인 HOLD. 기존 SearchDatabase 초기 환경 예외1은 Runtime 0과 구분. **아트 담당은 이번 판정에서 추가 PNG/게임 본편 적용/Unity 작업 수행0**. 다음은 기존 RT18장 동일위치 확대 비교 및 바울 대표 눈패치·굽음·HUD 아트 검토만 진행; 남은 결함을 기존 소스 배치로 해결할 수 없다는 격리 실증 전까지 신규 자산 발주0.

## 0. Codex 실제 카메라 RT 현장 감사 추가 접수 — 2026-11-03

- **근거:** Codex [#83 현장 감사](https://github.com/HappyPaul812/Project-Limitless/issues/83#issuecomment-6602568367) · [#85 현장 감사](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6602569428). 로컬 문서전용 브랜치 `codex/f2-environment-readonly-audit-20261103`, 실제 Codex 로컬 commit `b70b14175b67dc8490ea8efc3bccf94eb1c22126`. 이 결과는 **Codex가 기존 #83·#85 실제 Unity Camera RT 원본을 읽기 전용으로 검토하고 GitHub 댓글에 보고한 것**이다. 아트 담당이 로컬 F:의 RT PNG를 직접 수령해 픽셀 검증한 것은 아니다. Codex Unity 재실행0, 기존 90/81 PASS 재검사0, 수정파일 문서2개만, 원본 Scene/Save/main 변경0.
- **실제 남은 임시 아트 3:** 눈바닥 `PLACEHOLDER_GROUND`, **직각 구간이 인공적인 흙길** `PLACEHOLDER_PATH`, **접지 음영이 약한 단순 침엽수** `PLACEHOLDER_TREE`. 따라서 다음 과제는 아트 생성보다 **기존 소스의 격리 장면 배치·시각 델타 QA**다. 교체가 완료됐다는 기록은 아직 없음.
- **기존 아트로 재사용 가능(Codex REUSE_OK):** 승인 Frostway Earth/ThinFrost/Gravel **9개(각256×256)**, 기존 F1 `NorthF1_Path_WornTrail_Overlay_v2` **256×256**; F2 P1 `FirstSnow_GroundOverlay` **256×256**, `SnowPine` **256×320**, `FrostShrub` **256×160** 후보. **REUSE_OK는 디자인 승격/배치 완료/Unity 교체 후 PASS가 아니다**. F2 P1은 미승인 후보 상태 유지. 승인 NaturalBroken Stump v4는 F2 접지/앞뒤 Sorting 기존 실증 유지.
- **기존 물리/화면 근거:** 960·1280·1680×720 실제 RT에서 길의 물리 폭·플레이어 표시 크기 유지, 경계 형태 식별. 그러나 남쪽 길 일부가 **좌하단 HUD**에 가려짐. 이 문제는 새 아트가 아니라 격리 HUD Safe Area/앵커 조정 문제로 **Codex 구현/UI 담당에 전달**. 휠체어 실제 Collider 통과는 이전 #83 결과를 재사용하며 새 교체 뒤 충돌 상태가 검증됐다는 뜻은 아님.
- **교체 후 검증 필요(`NOT_VERIFIED`):** 눈 Overlay 반복/적설 분포, 흙길 굽음의 직각 seam/이음새 및 4.5WU 길폭 보존, 나무 Pivot·접지·Sorting·시야, 새 눈 지면↔그루터기 색감, 시각 경계와 Collider의 일치, 3개 비율에서 좌하단 HUD의 길 가림. 기존 길 PNG를 **4.5WU 통로에 일괄 늘리기 금지**, 나무를 기존 2×3WU 영역에 **비율 왜곡 금지**.
- **신규 PNG 판정:** `NEW_PNG_REQUIRED = 0` 종. 결손 위치·규격의 확정 발주가 없으므로 환경 타일/나무 신규 그림을 제작하지 않는다. 더 필요한 경우에도 **기존 자산 배치가 불가능하다는 실제 동일배율 캡처 근거를 받은 뒤**에만 최대 2개를 별도 발주한다.
- **동결·승인:** Owl v3 Idle8/Attack4 `USER_ART_APPROVED`, NaturalBroken Stump v4 `USER_APPROVED`, 기존 승인 9지면/43환경 불변. Sign·Fence FULLREDRAW `KEEP_CANDIDATE / USER_ART_REVIEW_PENDING` 및 냉기 VFX `HOLD`. Owl 전투 #85 81 PASS·왕복 #83 90 PASS 보호, 본편 적용·main 병합 없음.

**권장 다음 Codex 격리 Delta 작업(별도 지시 전에는 실행 금지):** 임시 GROUND/PATH/TREE 3종의 실제 SpriteReference·위치·PPU/Pivot을 먼저 확인 → 기존 승인/후보를 별도 카피의 격리 장면에만 비파괴로 재사용 배치 → 960/1280/1680 RT 동배율 전후 및 휠체어·도보 길폭/경계/HUD 가림/나무 접지 최소 변경 QA → `REUSE_PRESENTATION_PASS` 또는 정확한 잔여 ArtGap만 보고. 해당 QA가 승인 없는 기존 F2 P1 후보를 공식 승격시키는 것은 아님.

## 1. 실증 범위와 왜 신규 PNG를 0종 제작했는가

- [#83 Codex 실제 격리 Unity 보고](https://github.com/HappyPaul812/Project-Limitless/issues/83#issuecomment-6594041936)에서는 `World_NorthF2_CANDIDATE`에서 **`PLACEHOLDER_GROUND`, `PLACEHOLDER_PATH`, `PLACEHOLDER_TREE` 3종을 검증용 임시 아트**로 시연. 폭4.5WU 실제 흙길, 도보·휠체어 Rigidbody2D/Collider와 1280·960·1680×720 카메라의 북문→F2→북문 이동 **90PASS**, 정적23PASS. 자연형 승인 Stump v4가 위치 `(-0.3,1)`에서 지면/Sorting·통행 표시 검증된 증거. **이 QA는 임시자산 미술 승인 또는 본편 적용 PASS가 아님**.
- [#85 Codex 신규 Owl 전투 보고](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6601090552)에서는 같은 격리 F2를 재사용해 실제 전투 **81PASS/0FAIL**, 3화면비, 도보·휠체어·합성 Save 복귀 PASS. 그러나 해당 보고는 **#83의 임시 GROUND/PATH/TREE 3종을 어떤 최종 환경 Sprite로 교체했는지 기록하지 않음**. #85의 전투 기능 통과를 최종 F2 환경 아트 통과로 승격하면 안 됨.
- 관련 Unity 카메라 RenderTexture PNG·전투영상·단일 리뷰HTML은 Codex 사용자의 Windows PC 로컬 `F:/study/codex/Project-Limitless/Temp/Main24F2FirstTraversal20261103/Review/index.html` 및 `.../Main24FrostOwlFinish20261103/Review/index.html`에 저장됐다고 보고. **본 ChatGPT 아트 세션에서는 실물 캡처/영상 파일이 현재 접근 가능한 GitHub·Library·대화에 제공되지 않아 이미지를 직접 열어 확인하지 못함**. 이 회차의 PNG 비교판 2장은 **OFFLINE / 실제 F2 Unity GameView가 아님**.
- 바울 대표의 명시 규칙은 **'실제 부족한 아트가 확인된 경우에만 최대2종'**이고, 현재 사유가 미확정인 3 placeholder에는 **기존 승인 지면9·F1 전환·길 오버레이·F2 P1 소나무/눈 오버레이 등 재사용 후보가 이미 존재**. 그러므로 증거 없이 새 접합 PNG·나무·울타리를 만들지 않고, **신규 게임용 PNG 0개**, 비교판·갭표·검증 요청안만 제작함.

## 2. 우선 재사용안·상태별 갭표

| 영역 | 직접 확인 가능한 사실 | 기존 자산 우선 | 현재 판정 |
|---|---|---|---|
| F1→F2 Earth/ThinFrost/눈 첫 접합 | #83 GROUND는 검증 placeholder; 실제 접합 픽셀 미열람 | **Frostway v10 공식 Ground Earth/ThinFrost/Gravel 9** + **NorthF1 EarthToFrost Transition v3** + F2 P1 `F2_FirstSnow_GroundOverlay_v1` | `REUSE_FIRST / VISUAL_SEAM_NOT_VERIFIED` |
| 흙길 윤곽·첫눈 경계 | #83 PATH placeholder / 물리경로 폭4.5WU | **NorthF1_Path_WornTrail_Overlay_v2**, 공식 Ground9 + F2 눈오버레이. 길 형태·밝기 외 질감·눈둑 윤곽으로 식별 | `WAYFINDING_VISUAL_NOT_VERIFIED` |
| 통행/휠체어 | #83 실제 Collider/Rigidbody 90PASS, #85 후속81PASS. 격리 기능 확인 | 이미 확정한 이동 폭/출입 동선 보호; 나무·관목·경계 소품을 길 바깥에 배치 가능성만 검토 | `PHYSICS_ISOLATED_PASS / VISUAL_ACCESSIBILITY_NOT_VERIFIED` |
| 나무·관목 | #83 TREE placeholder, #85 최종 Sprite 교체 증거 없음 | F2 P1 눈 소나무 `738827ff...`, 서리 관목 `ba66a675...`, 기존 F1 승인 소품 | `REUSE_FIRST / SORTING_NOT_VERIFIED` |
| 자연형 Stump v4 | 사용자 미술승인/기술격리 #82 21PASS, #83 F2 실제 배치 접지/통행 | **승인 v4** SHA `22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8` | `USER_ART_APPROVED / ISOLATED_F2_PASS / NOT_ORIGINAL_APPLIED` |
| Branch / Gentle Snow | 기존 F2 P2 원본 후보 | 나뭇가지 `b05856bb...` + 약한 눈발 `3a57f2c9...` | `KEEP_CANDIDATE / NOT_UNITY_APPLIED` |
| Sign / LowFence | FULLREDRAW 두 대안 Codex `KEEP_CANDIDATE`, 바울 대표 최종 미술 승인 없음 | 기존 v3와 FULLREDRAW를 같은 규격 비교, **신규 제작/승격/Scene 배치 금지** | `USER_ART_REVIEW_PENDING` |
| 한기 VFX | #85 Runtime COLD_STATUS_HOLD | 분리된 IceArc 후보 보관, 제작0 | `UNAPPROVED_VFX_HOLD` |

**Unity 기존 실물 .meta 확인:** `main`의 NorthF1_Ground_Earth_Base_v2.png.meta **PPU128/Pivot(0.5,0.5)**, NorthF1_Path_WornTrail_Overlay_v2.png.meta **PPU128/Pivot(0.5,0.5)**, Frostway Ground ThinFrost_01.png.meta **PPU128/Pivot(0.5,0)**, NorthF1 FrostShrub_v2 `PPU128/Pivot(0.5,0.0546875)`. F2 새 후보의 PPU/Pivot/footprint는 각 용도별 검수 전 **SPEC_PENDING**. 기존 Ground9 **모든9개 픽셀 원본**은 이번 회차에서 직접 열어보지 않았으므로 전체 지면 맞춤 PASS가 아님.

## 3. 실제 납품(비교·증거·검증 지시) 및 무결성

- ZIP **`Limitless_Main24_F2_Environment_ReuseAudit_20261103.zip`** / **1,465,842 bytes / 16 entries** / **SHA256 `1df607fc10be70a1815ec5b643e5fc23be948a67c322d5bed7ec877dd10a175d`**.
- **ZIP CRC PASS, 내부 SHA256SUMS 15/15 PASS**.
- 작업파일 실제 생성 `/mnt/data/Limitless_Main24_F2_Environment_ReuseAudit_20261103.zip`. **ChatGPT Library 업로드 시 `container_session_expired` 오류 발생하여 Library 업로드 실패**; 성공했다고 주장하지 않음. GitHub에는 Markdown 텍스트 정식보고만 커밋하며 ZIP/PNG 원격 바이너리 업로드0. 대화의 sandbox 링크는 제공 가능하나 사용자 이전 다운로드 장애 때문에 별도 확인 필요.
- 실제 파일: `Preview/F2_ExistingReuse_GapAudit_OFFLINE.png` 2080×1430 (F2 P1/P2 6종 기존 이미지 + #83/#85 증거 격차 표시), `Preview/F2_SignFence_Before_FULLREDRAW_128_40_OFFLINE.png` 2030×1500 (v3 기존 vs FULLREDRAW 2쌍), `Preview/01_...04_128px_DERIVED.png` 등 128/40 실원본 리샘플8 PNG, `Docs/F2_GapMatrix_20261103.csv`, `Docs/SourceAsset_SHA256_Inventory.csv`, `Docs/Codex_ReadOnly_ActualF2_Screenshot_Check.md`, `Manifest.json`, `SHA256SUMS.txt`, `README_KR.md`.
- **생성한 10종 PNG는 전부 Preview/OFFLINE 파생·설명 이미지**이며 **새 게임용 Sprite/Candidate PNG 0개**. 승인·보호 이미지 및 후보 원본을 입력으로 읽기만 하고 파일 단위 원본 SHA를 정본 ZIP에서 직접 검증.
- 원본 파일 해시(대표):
  - Sign 기존 v3 `70b8ed868d61b1abc37d8f775116613ea45352b80fd53fa0cacf7d29a61ce37c`, Sign FULLREDRAW **`864e964ee4cc3868bd315af2249fe92d04e888937ae66f963232636cda8e4488`** = `KEEP_CANDIDATE`·미승인.
  - LowFence 기존 v3 `3aa0a8081ff1a58a4ab7157a974a550fc0b6c57a8263e1a81774b0027edadd6e`, Fence FULLREDRAW **`49c2969b8607d91b6bf15cb0e4485bcc3f25379669064b025d63b099c9c62e8c`** = `KEEP_CANDIDATE`·미승인.
  - F2 P1 Ground Overlay `61141ace90d5c64c2277da0cb7e27d9674bce6c05f276fb3b9801cd05ffa272a`, SnowPine `738827ff84a602ecc1c377e2c8c1398566cdec414f272c4b74855f850ed5075a`, FrostShrub `ba66a6754b90db16d26c60e4c634a90cd27d5fd7a8ccc8ded931591071ff85f2`. 모두 기존 파일 바이트 변경0. 서리올빼미 승인 Idle8/Attack4도 변경0.

## 4. 다음 실제 검수 게이트 / Codex에게 전달할 지시

- 패키지 `Docs/Codex_ReadOnly_ActualF2_Screenshot_Check.md`에 바로 붙여넣을 **Codex 읽기 전용 실제 F2 캡처/Renderer/Sprite 매핑 감사 지시문** 수록. `#83`의 실제 Camera RT 6PNG 및 `#85` 실제 Camera/전투 후 복귀 화면을 같은 장소/배율로 비교한 뒤, `PLACEHOLDER_GROUND/PATH/TREE` 실제 최종 사용 여부를 정확히 보고.
- 가장 먼저 **REUSE_OK** 여부를 기존 승인 Ground9/F1 Overlay/F2 P1 P2로 검토. 기존 소스로 해결 불가능하다고 입증된 영역만 `NEW_PNG_REQUIRED` 최대 2개로 분류하고 **문제 이미지 좌표·픽셀 규격/PPU/Pivot·보존 SHA**를 명시. 그 전에는 신규 제작 명령을 내려서는 안 됨.
- 미술 승인 대기 Sign/Fence의 비교판을 사용자가 직접 시각판정하기 전 공식 Sprite 적용 불가. Owl·Stump는 이미 승인; Owl 기본 전투 #85 81PASS 재실행 금지. 한기 VFX 미승인/HOLD 유지.
- 이번 회차 **Unity 원본/Scene/Prefab/Save/UserData/BuildSettings/Packages/MonsterDefinition 및 제품 main 변경0**. `docs/art-new` 문서만 수정/추가; 원본/개인파일 해시·복사·수정0, 원격 main 직접 Commit/Push/Merge0. 바울 대표의 추가 본편 적용 승인 없음.

**최종 게이트:** `AUDIT_COMPLETED / 0_NEW_GAME_ASSETS / ACTUAL_F2_VISUAL_QA_PENDING_LOCAL_SCREENSHOTS / USER_SIGN_FENCE_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED`.
