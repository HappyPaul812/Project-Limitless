# LIMITLESS — 맹독뱀 v3 PASS 권고 원본 보호 / 늑대 v3 국소 알파 보완 v4 실제 납품 (2026-10-21)

## 근거와 승인 상태
- GitHub Issue #43 최신 Codex 댓글 `#issuecomment-6305728893`, 기획 `docs/planning/문서/00_프로젝트/20261021_ArtReview_Result_SnakeWolfAndNorthF1_TargetedProduction.md` 결과와 사용자 직접 지시 준수.
- Codex는 **정적 1포즈 Snake v3 시각 PASS 권고 / Wolf v3 FAIL**, 실제 게임 GameView 아닌 **오프라인 합성** QA. 사용자 최종 미술 승인 별도.
- **독사는 이번에 PNG 재제작·수정·추출·포장 0.** 원본 `VenomSnake_KeyArt_v3.png` SHA256 **`7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0`**, 1254×1254 RGBA, 해시 재검증 PASS. 좌측 x=0 알파 접촉25픽셀 및 작은 알파 군집548건 경고는 **문서상의 기술 경고**로만 남김. Snake를 최종 USER_APPROVED라고 표시하지 않음.

## 늑대 실제 수정 PNG 1포즈
- **수정 입력**: `Wolf_KeyArt_v3.png` 952×1306 RGBA, 원본 SHA256 `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`, 1,022,752bytes. 원본 자체 읽기전용 보존.
- **수정 후보 출력**: `Wolf_KeyArt_v3_AlphaFix_v4.png`, 952×1306 RGBA, SHA256 **`62f72e97525f0266cbb6132da8b21571594f7c0deb3b1e1749cce749c9bf1c0a`**.
- 수정 범위: Codex가 지적한 **가슴 밑 불투명 회색 매트, 뒷다리·다리 뒤 배경 잔여물**의 알파값만 위치 제한하여 감소/제거. **변경 알파 픽셀 12,088개**. 원본/후보 RGB 채널 3개 **전 픽셀 완전 동일**, 캔버스 크기·기존 흰/회색 털·발끝 색·독사 아트·전체 디자인 변경0. 이번 수정에 이미지 생성 모델의 별도 늑대 그림은 **전혀 사용하지 않음**.
- 미술 품질 주의: 검사 결과는 **오프라인 파일/마스크 QA**이며 실제 게임 Import·최종 시각승인 증거가 아님. 회색 원화의 마스킹 경계가 일부 털과 닿는 부분은 바울 대표와 Codex가 실제 GameView와 함께 확인하고 추가 국소 조정 필요 여부를 판단.

## 실제 납품 ZIP/검증
- **실제 파일** `Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v4.zip`; SHA256 **`e241535f1c505bf50f4de09317e74dbc3780be722139ff0baa02c908223765c6`**; **2,466,565 bytes / 14 entries**. 납품 장소: 본 ChatGPT 대화의 `sandbox:/mnt/data/Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v4.zip`. 사용자 F: 또는 GitHub repo에 ZIP 자동 복사/업로드 없음.
- ZIP `Assets/`: 신규 늑대 보완 후보 PNG 1종 + 비교용 기존 Wolf v3 원본을 **원본 해시 그대로** 사본 수록. Snake 원본은 ZIP에 미포함, SHA 기록만.
- `Preview/`: Chest_Matte_BeforeAfter_2x.png, HindLeg_Background_Remnants_BeforeAfter_2x.png, Leg_And_Paw_Edge_BeforeAfter_2x.png, Wolf_128px_BeforeAfter.png, 원본/새 128px RGBA 2장, Alpha_Changes_RedOverlay.png. 모두 실제 PNG 바이트로 작성된 오프라인 체커보드 비교이며 **Unity GameView 아님**.
- `QA/`: 변경 12,088개 모든 좌표별 old/new alpha CSV, `Offline_Validation.json`, `Final_ZIP_Result.json`. `Manifest.json`에 원본/수정/독사 정본 SHA·오프라인 QA.
- **검사**: Pillow PNG 후보 원본 실제 Decode, 952×1306 RGBA, 후보 Alpha가 원본보다 증가한 픽셀0, RGB 변경0, ZIP `zipfile.testzip() is None` CRC PASS, 아카이브 추출 readback이 소스 파일과 byte-for-byte 일치, 새 PNG와 보존 원본의 SHA 대조 PASS. ZIP 자체 해시는 파일 내부에 자기 자신을 embed할 수 없으므로 본 보고서/다운로드 바이트에서 SHA 제공.

## 보호/다음 게이트
- v1 반려/v2 HOLD, v3 Snake PASS 권고/Wolf v3 FAIL, Wolf v4 **보완 실물 납품/사용자 미술·Unity 검증 대기**.
- Codex는 현재 별도 Chapter3 Mouse QA를 완료/진행하고 있을 수 있어 이 세션에서 **Codex 새 실행 작업 지시 없음**. 후속 사용자는 Wolf v4 원화와 128px 비교를 검토하고 필요한 경우 Codex의 *변경분만* 동일 Camera/WorldScale 3화면 시각 QA 요청.
- Wolf Run, Bear, Fox 애니메이션, Snake 전체 동작, 원본 Unity·MonsterDefs·펫 시스템·Save·Editor·Git main 변경0. `docs/art-renewal` 문서만 변경하며 main 병합 금지.

**STATUS:** `SNAKE_V3_PASS_RECOMMENDED_SHA_RECORDED_USER_PENDING / WOLF_V4_ACTUAL_PNG_DELIVERED / WOLF_V4_ALPHA_ONLY_OFFLINE_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / NO_ANIMATIONS`.
