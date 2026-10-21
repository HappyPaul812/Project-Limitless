# LIMITLESS / Chapter1·2 Wolf Run6 v4 표적 미술 보완 실물 납품 (2026-10-21)

## 근거·제한
- [Issue #43](https://github.com/HappyPaul812/Project-Limitless/issues/43) 최신 Codex 댓글 `#issuecomment-6310494103`.
- 기획 `docs/planning/문서/00_프로젝트/20261021_Village_WolfRun_JointQA_ShaMismatch_TargetedProduction.md` 확인.
- v3는 격리 Unity 12FPS 실제 재생 기술 PASS, **미술 HOLD**: Frame04 뒷다리, 중간 접지·몸체비율, 05→00 루프.
- 승인 Wolf v5 KeyArt `Wolf_KeyArt_v3_AlphaFix_v5.png` SHA256 `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1` **원본 직접 해시 재검증**, 변경0.

## 실제 v4 변경 및 출처
- 6 프레임 LEFT, 12FPS, 각 512×384 RGBA.
- **Frame 00·01·02·03**: 기존 v3 PNG 원본 compressed bytes **동일**. Frame03/04 잘림 복원 중 04 **꼬리 원화 영역 pixel exact preserved**, 03 완전 unchanged.
- **Frame 04**: 원래 부자연스럽게 뒤로 꺾인 위쪽 뒷발의 꼬인 실루엣을 제거하고 같은 늑대의 Frame01에 있던 **그려진 관절·피부·털·발톱 조각을 근거로 분리·세로길이 조절·재배치**해 뒷다리의 무릎/발목과 접지 포즈를 보완함. 원본 프레임 대비 RGBA 변화 4,368 픽셀; 기존 복원 꼬리 상단은 동일.
- **Frame 05**: 다음 Frame00의 전방 팔 뻗음 직전, 원래 딱딱한 수직 접지 다리를 걷어내고 Frame01의 **원화 굽힌 전완·앞발 포즈를 구성 요소로** 붙여 지면에서 발이 올라오는 동작으로 보완. RGBA 변경 7,542 픽셀.
- 중요: 이 v4는 **기존 페인팅에서 각 관절 소스 조각을 따로 사용한 새로운 포즈 구성**이며 완전히 새로 손으로 그린 6장이라고 주장하지 않음. **단순 6장 XY 이동/Scale만으로 만든 가짜 Run은 아님.** 실제 해부학/회전 모션 합격은 Codex 실화면 DeltaQA와 바울 대표 모션 승인 대기.
- 원본 v3 ZIP/원본 Wolf v5 PNG 및 `main`/Unity/펫런타임은 무변경.

## v3 ZIP SHA 불일치 조사
- 사용자가 가진 Codex 초기 검수 ZIP SHA256 `f7eb4122b150e351900a4ab7ea82c802809605bd0444be89e36f6bf167dcd008`, 당시 납품기록 2,788,009 바이트.
- 이번 작업환경에 실제 보관된 최종 v3 ZIP SHA256 `171b954ddeef2b37d1bfaeb197b8a9f028293494457b66664c1d83509b87b5e0` 및 **2,788,020 바이트** 직접 재해시 확인. **11 바이트 차이**의 두 배포본이 존재했던 이력으로 확인. 예전 v3 첫 문서에는 `f7eb...`, 후속 GIF 12FPS 재인코딩·압축 및 README/QA 갱신 후 `171b...`을 최종 값으로 기록.
- 그러나 Codex가 받은 초기 ZIP 원본바이트가 이번 컨테이너에 없어 **어느 내부 ZIP entry의 압축데이터/README/metadata가 정확히 달라졌는지는 입증 불가**. 예상과 실제가 다르다는 사실을 그대로 기록하며 ‘바이너리 동일 PASS’라고 바꾸지 않음. 새 v4 ZIP은 재포장 후 고정, 검증용 최종 SHA 아래 참조.

## v4 실제 납품 ZIP·Manifest
- **`Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v4.zip`**
- **3,024,517 bytes / 34 entries / SHA256 `3ab0b57d2389b55ed33f73c1d3d13862b9b595245e196d1f3a4c98f090b8a412`**
- 채팅 Sandbox 사용자 실제 다운로드 `sandbox:/mnt/data/Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v4.zip`이며 GitHub 바이너리/F: 자동 저장이 아님.
- ZIP 내 `Assets/Poses/Wolf_Run_00_v4.png` ~ `05_v4.png` 독립 6 PNG. `Assets/Wolf_Run6_v4_Strip_3072x384.png` (6× 512x384), `Assets/Wolf_Run6_v4_Atlas3x2_1536x768.png` (3x2).
- `Preview/Wolf_Run6_v4_12FPS_128px.gif` (6 프레임 500ms 루프), `Preview/Wolf_Run6_v4_12FPS_Transparent.apng`, `Preview/Wolf_Run_v3_vs_v4_12FPS.gif`, 128px/40px 동일 슬롯, `Preview/Frame04_HindLeg_Joint_Paw_v3_vs_v4_2x.png`, `Preview/Frame05_Foreleg_LoopPrep_v3_vs_v4_2x.png`, `Preview/Loop_05_to_00_v3_vs_v4_Static.png`, 변경픽셀 색 표식 2개.
- `QA/Frame_Gait_And_Alpha_Checks.csv`, `QA/Offline_Validation.json`, `Manifest.json`, `README.md`. PPU/월드비율 Import 계약은 미결.

## 오프라인 QA
- ZIP Python `zipfile.testzip() == None` CRC PASS, 전 PNG 6 해독 PASS, Strip/Atlas 각 6 cell과 원본 Pixel RGBA 바이트 일치, 각 frame 투명 경계 3px 누수 0.
- 원본 v3 PNG 프레임 00~03 byte identity PASS, Frame04 v3 Tail(346,88..485,164) pixel identity PASS.
- 프레임당 아래쪽 alpha 끝 y: [322,316,322,281(도약),330,325], 05→00 3px 차이(발 하한선 단순 수치이므로 실제 해부학/접지 속도/스텝 모션 PASS를 대체하지 않음).
- **검증 상태** `OFFLINE_ASSET_QA_PASS / MOTION_ART_QA_PENDING / ISOLATED_UNITY_V4_NOT_TESTED / USER_FINAL_RUN_APPROVAL_PENDING`. 대상 프레임04 뒷다리와 05→00 변경분은 후속 Codex 격리 실제 12FPS 동작 재검수 필요. 앞선 v3 미술 HOLD가 이번 v4에서 해결됐다고 선포하지 않음.

## 보호·후속
- 바울 대표와 Codex가 v4 04/05 실제 12FPS 동작, 128px·40px, 05→00 연결을 비교해 최종 승인 여부 결정.
- 기존 Wolf v5 KeyArt v3/v4/v5, v2/v3 Run ZIP, 맹독뱀 모션·Bear·Fox, Unity 원본 Scene/GUID/meta/BeastCompanionSystem/Save/UserData/Editor, Chapter3 Main22/Main23 및 main 직접 Commit·Push·병합 **모두 보호**.
- 해당 GitHub 작업은 **`docs/art-renewal`의 문서만** 커밋.
