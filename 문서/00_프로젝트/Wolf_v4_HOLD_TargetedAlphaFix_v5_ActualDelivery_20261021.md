# LIMITLESS — Wolf v4 미술 HOLD 후 잔여 알파 세 군데 국소 보완 v5 실제 납품 (2026-10-21)

## 발주와 승인 경계
- 근거 [GitHub Issue #43](https://github.com/HappyPaul812/Project-Limitless/issues/43) 최신 Codex 댓글 `#issuecomment-6306969646` 및 기획 정본 `docs/planning/문서/00_프로젝트/20261021_Wolfv4_F1v2_QA_TargetedFix_Orders.md`.
- **Wolf v4: 기술 PASS / 미술 HOLD.** 새로 그리거나 포즈 변경 금지; 가슴 위 회색 매트·다리 주변 미세 배경 잔여·앞발 밑 바닥 조각 **세 영역의 알파만** 수정.
- **맹독뱀 v3: 정적 1포즈 Codex PASS 권고, 대표 최종 승인 별개.** 원본 수정·재압축·애니 제작0.
- 상태: `WOLF_V5_REAL_PNG_ZIP_DELIVERED / OFFLINE_ALPHA_QA_PASS / VISUAL_DELTA_PENDING / USER_ART_APPROVAL_PENDING / MAIN_UNCHANGED`.

## 원본·결과물 실물 SHA256

| 파일 | SHA256 | 보존 |
| --- | --- | --- |
| `Wolf_KeyArt_v3.png` | `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e` | 원본 SHA 일치, 파일 변경0 |
| `Wolf_KeyArt_v3_AlphaFix_v4.png` | `62f72e97525f0266cbb6132da8b21571594f7c0deb3b1e1749cce749c9bf1c0a` | **v5의 유일한 입력 PNG**; SHA 일치, 원본 변경0 |
| `Wolf_KeyArt_v3_AlphaFix_v5.png` | **`b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1`** | 새 후보 독립 PNG 952×1306 RGBA |
| `VenomSnake_KeyArt_v3.png` | `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0` | 기존 Codex PASS 권고 후보 보존, 변경0 |

- v4→v5 전 픽셀 RGB 완전 동일(**RGB 변경 0**), 전체 원본 PNG dimensions unchanged, **알파 변경 2,174 픽셀 전부 감소**, 증가0. 외곽 RGB 색 변경/늑대 전체 재디자인/이미지 생성 새 그림0.
- 국소 실제 수정 영역별 좌표와 픽셀 수(좌표 원점 **왼쪽 위**, x,y, 최대값 미포함): 
  1. 가슴 상단 회색 배경 매트: x=620..667, y=790..843, **900 px**;
  2. 다리 주변 작은 수평 배경 잔여 조각: x=475..549, y=1158..1187, **237 px**;
  3. 앞발 아래 바닥 잔여 조각: x=754..882, y=1230..1260, **1,037 px**.
- 위 값은 **허용 검증 영역의 bounding box**이지 꼭 그 박스 전체 픽셀이 바뀌었다는 의미가 아님. 실제 변경 각 픽셀 좌표와 v4/v5 alpha 차이는 ZIP 내 `QA/Changed_Alpha_Coordinates_v4_to_v5.csv`의 2,174 행으로 정확히 추적.
- **주의:** 오프라인 검사에서는 털/발톱 색(RGB) 보존이 증명되나, 알파 경계가 실제 동작과 128px 표시에서 미술상 최종 통과라는 뜻은 아니다. 생물 윤곽 주변 국소 알파 감쇠가 털 숱을 적절히 보존했는지는 대표와 Codex 시각 Delta 확인 필요.

## 실제 ZIP 및 미리보기

- **실파일:** `Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v5.zip`, **2,214,467 bytes**, **SHA256 `920459eb2ef4e160461b2b6425a9b183da633cd14411655a4d5affdbaf3d8bc7`**, **13 entries**.
- **실제 대화 납품 경로:** `sandbox:/mnt/data/Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v5.zip`; GitHub에 ZIP 바이너리 자동 업로드·대표 F: 자동 저장 없음. ZIP 내 **신규 아트는 `Assets/Wolf_KeyArt_v3_AlphaFix_v5.png` 한 장만**. v3/v4/독사 원본의 별도 바이너리는 원래 경로에서 그대로 보존.
- `Preview/Chest_Upper_Matte_2x.png`, `Preview/Leg_Micro_Remnants_2x.png`, `Preview/FrontPaw_Ground_2x.png`: PNG v4와 v5의 실제 픽셀을 동일 crop/2배 Nearest로 합성한 **정적 오프라인 시각 비교**. 실제 Unity GameView 아님.
- `Preview/Wolf_v4_128px_RGBA.png`, `Wolf_v5_128px_RGBA.png`, `Wolf_128px_BeforeAfter_dark.png`, `Wolf_128px_BeforeAfter_field.png`: 동일 최대116px 스프라이트 크기 128px 슬롯 미리보기(실제 GameView/ActorScale 아님).
- `Preview/Alpha_Changes_v4_to_v5_RedOverlay.png`: 빨간색은 변경 알파 위치 표식이며 **실제 게임용 PNG에 빨간 칠하지 않음**.
- `Manifest.json`: 원본·수정 SHA/보존 규칙/좌표별 변경 수/검증 상태. `QA/Offline_Validation.json`과 `QA/Changed_Alpha_Coordinates_v4_to_v5.csv` 및 `README.md`.

## 무결성 검증
- 실제 PNG 파일 Pillow 재해독 PASS, RGBA952×1306 PASS; v4 대비 전체 RGB array 동일/alpha 증가0/로컬 변경 영역 내에만 변경 사실 확인.
- ZIP 표준 `zipfile.testzip() is None` **CRC PASS**, 13개 엔트리 ZIP readback 전부 원본 파일 바이트와 일치; ZIP에서 신규 PNG 재해독 및 SHA 재대조 PASS.
- ZIP 자체 SHA는 ZIP 안에 순환적으로 포함할 수 없으므로 본 보고서/별도 `Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v5_ReleaseQA.json`에서 제공.
- **미술 최종 판정:** `OFFLINE_FILE_PASS / CODEX_VISUAL_PENDING / USER_ART_APPROVAL_PENDING` (기술 검사와 미술 승인 엄격 분리).
- 후속: Codex가 가능할 때 이미 수행한 전체 전투 QA 재실행하지 말고, **세 결함 부위의 v4↔v5 시각 Delta만** 원본 동일 크기 조건으로 확인. 유저 승인 전 Wolf Run·Bear·Fox/독사 전투 애니 및 Unity 본게임 연결 없음.
- GitHub 사용 범위: **`docs/art-renewal`** 문서 커밋만, `main` 직접 commit·Push·병합0, 승인된 Field09 및 원본 Unity/Save/UserData/MonsterDefinition 변경0.
