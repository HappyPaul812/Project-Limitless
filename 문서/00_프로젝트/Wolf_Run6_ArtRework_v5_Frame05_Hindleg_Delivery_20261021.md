# LIMITLESS — Wolf Run6 Frame05 뒷다리 국소 재작업 v5 납품 (2026-10-21)

## 작업 근거와 상태
- 사용자 발주: GitHub Issue #43 및 Codex Wolf Run6 v4 QA: ZIP/PNG/Strip/Atlas 기술 PASS, Unity 격리 실제 12FPS 재생 PASS, 그러나 **Frame05 뒷다리 x315~338/y235~280 직각 단절 미술 FAIL**.
- 이번 작업은 **v4 Frame05 해당 뒷다리 한 곳만 변경**, Frame00~04 압축 PNG 바이트 전체 보존. 기존 복원된 03·04 꼬리와 승인 정적 Wolf v5 원화 수정0.
- 사용자 안내 전 단계에 나타난 이미지 생성기의 새 늑대 그림은 **사용하지 않았음**. 실제 입력은 v4 패키지 내부 `Wolf_Run_05_v4.png`였음.
- **정확 상태:** `FRAME05_ANATOMICAL_ARC_FIX_DELIVERED / OFFLINE_PNG_STRIP_ATLAS_ZIP_PASS / CODEX_ISOLATED_12FPS_V5_QA_PENDING / USER_FINAL_MOTION_APPROVAL_PENDING / UNITY_OFFICIAL_NOT_APPLIED`.

## 실제 원본과 표적 수정
- 원본 ZIP `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v4.zip` SHA256 `3ab0b57d2389b55ed33f73c1d3d13862b9b595245e196d1f3a4c98f090b8a412` 실파일에서 v4 소스 6PNG를 추출해 검사.
- 승인 정적 원화 `Wolf_KeyArt_v3_AlphaFix_v5.png` SHA256 `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1` 이번 턴 직접 재해시 PASS. 바이트 변경0.
- 기존 Frame05 `Wolf_Run_05_v4.png`의 **불연속 수평단과 직선 경계**가 발생한 관절·정강이 내측을 국소 곡선으로 연결하고 새로 노출되는 약간의 픽셀에만 인접 원본 늑대 다리 색/질감을 재구성. 몸통·꼬리·눈·털 전체, 다른 발과 00~04 전부 보호. *새 이미지를 합성해서 몸 전체를 바꾸거나 Alpha만 단순 삭제한 것이 아님*.
- 수정 픽셀 총 **174개**; 실제 bbox **x=318..333, y=245..280** (사용자 허용 원인 범위 x315..338, y235..280 내부). 바깥 픽셀 변경 0. RGB는 새로 복원된 뒷다리 색 채움 픽셀 외에는 기존 원화 그대로. 전체 캔버스 512×384 RGBA.
- v5 Frame05 SHA256 **`d3d53b94be9084bfd9b2a0bbe6fa492b903138b37c86e7068c9eaefbe8b1194c`**, 원본 프레임00~04 SHA256 5/5 보존은 ZIP `Manifest.json`으로 재현.

## 실제 납품 ZIP (변경 전후·무결성)
- **`Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5.zip`** / **2,663,380바이트** / SHA256 **`16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351`**.
- 이 대화 `sandbox:/mnt/data/Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5.zip` 실제 다운로드 파일. GitHub 바이너리/바울 대표 F:에 자동 저장하지 않음.
- `Assets/Poses/Wolf_Run_{00..05}_v5.png`: 6PNG, 512×384 RGBA, LEFT, **00~04 v4 압축 PNG 파일 byte-for-byte 동일**.
- `Assets/Wolf_Run6_v5_Strip_3072x384.png` 6셀 / `Assets/Wolf_Run6_v5_Atlas3x2_1536x768.png`, PNG 소스와 6/6 RGBA exact.
- `Preview/Wolf_Run6_v5_12FPS_128px.gif` 및 APNG: 6 frames, 표시 간격 총 500ms/loop(12FPS 평균), 128px 고정 캔버스. `Preview/Wolf_Run_v4_vs_v5_12FPS_128px.gif` 동일배율 움직임 좌우 비교, 40px Icon 시트, `Preview/Frame05_HindLeg_v4_vs_v5_5x.png` 실제 픽셀 확대, `Preview/Loop_05_to_00_v4_vs_v5.png` 전환 비교.
- `QA/Frame05_Changed_Pixels.csv`는 원본·신규 RGBA 각각 기록한 실제 174행, `QA/Offline_Validation.json`, `Manifest.json`, `README.md`.
- **검증:** Python `zipfile.testzip()==None` CRC PASS, 엔트리 전체 바이트 재열기 PASS, Manifest 개별 SHA PASS, PNG 재해독6/6, Strip/Atlas exact6/6, 00~04 원본 PNG 바이트 유지5/5, 변경영역 ROI 체크 PASS, GIF·APNG 6 frames / 500ms.
- ZIP 자체 SHA/크기는 외부 `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5_ReleaseQA.json`에 고정, ZIP 안 Manifest에는 각 엔트리 SHA만 기입 (자기 ZIP 해시의 순환 포함 방지).
- **미술 판정은 별도:** 확대 시각으로 직각 단절은 완화했으나 실제 게임 월드 크기/Unity Frame05 뒤다리의 근육·접지/05→00 동작의 자연스러움은 **이번 작업환경에서 실제 Unity Play하지 않았으므로 Codex 후속 Delta QA**와 바울 대표 최종 모션 승인 대기.

## Codex 단일 후속과 안전
- Codex는 격리 `codex/*`에서 **Frame05만 교체한 v5 후보**를 Import. 00~04 byte SHA, v5 승인 키아트, 6-cell Strip/Atlas 대조하고 **12FPS 실제 재생 시 Frame05 뒷다리 x315~338/y235~280, 04→05→00 발 접지/Loop만** 변경분 비교. 기존 전체 Battle/Quest/Save 검증 반복 금지.
- 승인 전 본게임 Wolf/BeastCompanionSystem/기존 ThirdParty 원본 PNG/GUID/.meta/Unity scene/Main22·23/Save/UserData 변경, Bear/Fox/독사 모션, main 직접 commit/Push/merge 금지.
