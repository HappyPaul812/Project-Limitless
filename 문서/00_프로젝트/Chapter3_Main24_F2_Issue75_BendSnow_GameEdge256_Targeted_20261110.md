# LIMITLESS / 신규 아트 / Issue #75 / F2 BendSnow 256px 투명 잔설 경계 최종 표적 후보 (2026-11-10)

**브랜치:** `docs/art-new`  
**상태:** `F2_BEND_SNOW_EDGE256_ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_FINAL_ART_REVIEW_PENDING / UNITY_NOT_VERIFIED / NOT_UNITY_APPLIED`.  
**새 게임용 PNG 수:** **1개**.

## 1. 실제 검수·기획 근거

- [Issue #75 2026-11-10 총괄 기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6757397006) 및 [Codex F2 실제 RT 읽기 전용 좌표·스펙 인계](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6758024653).
- Codex는 이전 68PASS의 **실제** `After_Bend_1280.png`를 확인함: SHA256 `8079a18d1dcb61a2e12633ee07f2ac888d99798caa0ba0b88ac5122149c21eb8`, Camera(2,0,-10), ortho5, 1280×720 = **72px/WU**.
- R1 눈 반복 screen x180–350/y100–650; R2 길 굽음 screen x480–880/y180–510. 중심선 `(0,-12)→(0,0)→(3,0)→(3,8)` / 통로 시각 폭 **4.5WU**. 이는 이전 격리 결과에서 얻은 기준값이며 **이번 신규 시안의 Unity 기능 검사 결과가 아님**.
- Codex 측정 **F1 WornTrail / F2 FirstSnow Overlay** 256×256/PPU128/Pivot(0.5,0.5), 승인 Ground9만 Pivot(0.5,0). Point/Uncompressed/FullRect/Clamp/Scale1은 기존 규격. 이번 새 Sprite 256×256/PPU128/중앙Pivot은 **소스 규격을 따른 Import 제안**이며 실제 새 PNG .meta·Renderer·Tilemap에 적용해 승인한 결과는 없음.

## 2. 실제 납품

- **실제 ZIP:** `LIMITLESS_Issue75_F2_BendSnow_GameEdge_v1_20261110.zip` **6,169,252 bytes / 14 files**
- **ZIP SHA256:** `c38e97b3856245e474fea61f0b71f584af95618fa864723113041c6b1f7fdf58`. ZIP CRC `testzip()` **PASS**, `SHA256SUMS.txt` **13/13 PASS**, PNG 디코딩·투명RGBA/256² 확인.
- **실제 게임용 신규 독립 후보 딱 1장**: `Candidate/F2_BendSnow_SnowEdge_256x256_PPU128_CANDIDATE.png`, **256×256 투명 RGBA**, SHA256 **`c3eeee57c5c5597c2d764fdd831b9d25516df57c1751a6108db9e0fd1ec892c4`**.
- **원본 1774×887 미술 후보 바이트 동결**: SHA256 `1db66eb4f92931318d5fafd46b2cd337fb9d47033537b428e21c1471d71490dd`. 추가 생성 1254×1254 투명 원화는 눈/흙의 분리를 위한 시각 소스로만 참조, **전체를 Sprite로 적용하지 않음**.
- **기존 F2 FirstSnow GroundOverlay PNG 바이트 보호**: SHA256 `61141ace90d5c64c2277da0cb7e27d9674bce6c05f276fb3b9801cd05ffa272a`. 기존 Ground9/F1 WornTrail/F2눈 원본 재인코딩·교체·게임 패키지 중복 수록0.
- **라이브러리 실물 등록 성공:** `/LIMITLESS/Issue75_F2_BendSnow_GameEdge_20261110/`에 **ZIP + Before/After 오프라인 비교판 PNG** 별도 등록. 사용자 Windows `F:\Downloads` 자동 복사나 로컬 SHA 확인은 실시하지 않았으며 GitHub에는 ZIP/PNG 바이너리 업로드0(보고서·인계만).

## 3. 제작 방법과 안전한 재사용 계획

- **첫 번째 재사용안:** 기존 F1 WornTrail의 실제 길 굽음 및 승인 Ground9는 그대로 유지. F2 P1 눈 Overlay의 현재 **1.7WU 규칙 반복**을 기존 원본 바이트 변화 없이 소폭 좌표·각도·겹침을 불규칙하게 분배하는 시각 제안. 굽음 좌표 `Worn_Bend(1.5,0)`, 기존 Collider·4.5WU 폭은 수정하지 않음.
- **보조 최소 PNG 1개안:** 새 1254 생성 이미지의 **흰 청회색 잔설만** 선택·투명 추출, 바위·관목·갈색 흙길을 최대한 배제. 256²/PPU128 기준 **2WU×2WU 표시 후보**, 중앙 Pivot. 기존 길 위에 작은 잔설 테두리를 얹는 용도로만 사용하고 **4.5WU 통로 폭이나 물리 Collider를 바꾸지 않음**. 실제 위치 `(1.5,0.25)`는 오프라인 비교를 위한 **배치 제안**, 최종 Unity WorldTransform 확정값 아님.
- 현재 게임에 이미 있는 오브젝트를 이 Sprite에 다시 굽지 않았고 다른 소품 Sprite를 건드리지 않았음. **타일 자동 Seamless/WorldSorting/게임 본편 검증 미실시**. Codex가 제공한 실제 1280×720 RT 이미지 바이너리는 이 세션에 없으므로 정확 원본 RT의 실픽셀 Before/After라고 주장하지 않음.

## 4. 실제 검토 이미지 및 QA

- `Preview/F2_BendSnow_BeforeReusePatch_1280_OFFLINE.png` — **Codex 실제 R1·R2 좌표, 카메라/길 기하를 수용한 독립 OFFLINE 재구성**의 3단계 A 기존 규칙 Overlay / B 기존 Overlay 재사용 불규칙 분산 / C B+이번 신규 PNG 1장. **실제 GameView 스크린샷/Unity 변경 결과가 아님**. 표시 통로 4.5WU는 원 모델의 기하 시각화이며 새 Sprite의 실제 물리 통행 PASS가 아님.
- `Preview/ReferenceBefore_1280_OFFLINE.png`, `ReuseOnly_1280_OFFLINE.png`, `PatchAfter_1280_OFFLINE.png` — 동일 카메라 좌표·기존 F2 GroundOverlay 이용해 1280 오프라인 재구성. 실제 F2 RenderTexture 대신 재구성된 배경임을 명확히 함.
- `Preview/F2_BendSnow_Sprite_256_128_40_OFFLINE.png` 및 `F2_BendSnow_SnowEdge_{128,40}px_ACTUAL.png` 실제 파일에서 추출한 128·40px 축소 가독성.
- `Docs/DesignNotes.md`, `Manifest.json`, `QA_Result.json`, `SHA256SUMS.txt`, `README_KR.md` 포함. 기술적인 파일 재현·해시 검사 PASS와 실제 Unity 품질 검증을 분리.
- **승인 경계:** 사용자는 기존 1774 시안에 “후보가 괜찮네”라며 **미술 방향을 수용**했지만 **새 256px 부분추출 Sprite 자체에 대한 최종 미술 승인과 실제 F2 화면 배치·PPU Import 승인**은 아직 받지 않음.

## 5. 보호 및 다음 단계

- 기승인 Frostway Ground9/Environment43, F1 WornTrail·F2 P1/P2, 승인 Stump·Owl, 냉기 VFX·정식 아이콘, Ch3 BattleBG v2, 원본 Unity Scene/Prefab/Renderer/Collider·Save/UserData·BuildSettings·Packages·main **모두 수정0**.
- 기존 격리 68PASS·#83 90PASS·#85 81PASS 전수 재검증0; Unity 실행0.
- **바울 대표 최종 신형 256px 미술 승인 필요**. 승인 후에만 Codex가 로컬 실 RT/시각 참조를 기존 원본 폴더 밖에서 비교하고, 먼저 **기존 Overlay만 재배치한 안**이 충분한지 확인 → 필요한 경우 이번 PNG 1장 Import/Sorting/4.5WU 시각 폭·도보/휠체어 통행·HUD·3해상도 DeltaQA → 별도 정식 적용 승인. 사용자 승인 전 main 직접 commit/Push/merge0, Unity 본편 적용0.

**최종 상태:** `F2_BEND_SNOW_EDGE256_ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_FINAL_ART_REVIEW_PENDING / UNITY_NOT_VERIFIED / NOT_UNITY_APPLIED`.
