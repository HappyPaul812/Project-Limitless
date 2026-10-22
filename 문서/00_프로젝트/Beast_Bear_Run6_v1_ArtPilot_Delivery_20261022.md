# LIMITLESS — Bear Run6 v1 독립 미술 파일럿 실물 납품 (2026-10-22)

## 작업 근거
- 바울 대표: Wolf Run6 v5 최종 미술 승인·실게임 적용·main/로컬 동기화 완료. 후속 단계로 Bear, Fox 각각 분리된 6프레임 달리기 PNG 납품 지시.
- 기존 Wolf/Monster/Environment/Unity/Save/Collider/Quest/기타 모든 승인 자산 변경 0.
- GitHub main 리포지토리 전체 트리(7088항목)에서 **Bear/Fox 별도 승인 고해상도 Key Art 파일을 발견하지 못함**. `Resources/CompanionAssaultValidation/{Bear,Fox}_Run.png` 기존 제3자 ScratchIO 검증 후보만 확인. 현행 프레임/방향/FPS 참고만 하였고 픽셀 재활용·추적/재색칠 없음. **'승인된 Bear/Fox 정적 원화를 기반'이라는 주장 불가**, 스타일 참고는 승인 Wolf의 현실적인 야수 방향임.

## 실제 아트 납품
- ZIP `Limitless_Beast_Bear_Run6_v1.zip`, **5,725,723 bytes**, SHA256 **`1a29475207c0f99db87726b637d301e5010bf5ef6790e2415ad679f67ffd7f19`**, **37개 엔트리**. 채팅 실제 `/mnt/data/Limitless_Beast_Bear_Run6_v1.zip` 다운로드. GitHub/대표 F:에 바이너리를 자동 업로드·복사한 것은 아님.
- 개별 실제 RGBA: `Assets/Poses/Bear_Run_00.png` ~ `Assets/Poses/Bear_Run_05.png` (512×384×6, 좌향).
- `Assets/Bear_Run6_Strip_3072x384.png` (6×512), `Assets/Bear_Run6_Atlas3x2_1536x768.png` (3열×2행).
- `Preview/Bear_Run6_12FPS_128px.gif` 및 APNG; GIF 정확 총 500ms (80+90+80+90+80+80ms, 평균12FPS), APNG 83+83+84+83+83+84=500ms.
- `Preview/Bear_Run6_128px_ContactSheet.png` 및 `40px_ContactSheet.png`, 개별 축소 6×2, `Loop_05_00_01.png`, 승인 Wolf와 같은 **128px 미리보기 슬롯** 비교 `Approved_Wolf_vs_Bear_128px_NotUnity.png`. **실제 Unity 동일 World Unit / GameView 비교가 아님**.
- `Manifest.json` 개별 파일 SHA/크기, `SHA256SUMS.txt`, `QA/Offline_Validation.json`, `QA/Frame_Gait_Alpha_BBox.csv`, 6프레임 국문 설명/Importer 후보 계약/권리 설명, `Source/` 원본 생성 이미지 보드와 프레임 독립 추출 스크립트.
- 소스 출처 **정직한 제한**: 이번 세션에서 생성한 여섯 포즈의 AI 콘셉트 보드에서 각 동물 원화를 투명도로 분리하여 512×384로 배치한 파일럿이다. 실제 원본 보드에서 포즈당 시각 데이터는 대략120px이므로 **512px에서 신규로 전 픽셀 원화를 수작업 채색한 것처럼 보고하지 않는다**. 별도 원본 이미지 파일과 추출 코드는 Source에 수록, ThirdParty 시트 기반이 아님.

## 프레임과 기술/미술 QA
- 00 앞발 접지 → 01 보폭 전진 → 02 뒷다리 추진 → 03 짧은 전진 보폭 → 04 착지·체중 압축 → 05 다음 접지·00 루프 준비. 묵직한 몸통 볼륨과 짧은 꼬리 유지.
- PNG6 RGBA/512×384 재해독 PASS, 6프레임 각각 SHA 중복0, 프레임 사방 Alpha 침범0, Strip/Atlas 셀 픽셀 동일6/6 PASS, 6프레임 GIF/APNG 루프 시간 PASS.
- ZIP `zipfile.testzip() == None` **CRC PASS**, ZIP 37개 엔트리 byte-for-byte 외부 파일과 동등, Manifest SHA 재대조 PASS. ZIP 자체 SHA는 최종 바이너리를 재열어 산출하였으며 `/mnt/data/Limitless_Beast_Bear_Run6_v1_ReleaseQA.json`로 별도 기록.
- **아트 검토 위험**: 6자세는 서로 달라도 12FPS에서 발 접지/05→00 루프의 실제 자연스러움과 512px 털의 선명도는 Wolf 수준의 최종 미술승인에 못 미칠 수 있음. 현재 **`REAL_ASSET_PILOT_DELIVERED / FILE_QA_PASS / USER_MOTION_AND_STYLE_REVIEW_PENDING / UNITY_NOT_APPLIED`**. 자동 production-ready 표시 금지.

## 다음 단계
- Bear, Fox 각각 별도 대표 미술 검토 → 필요한 개별 포즈만 보완; 승인 후 Codex에서 실제 Bear/Fox 고유 PPU, 리소스 경로, GameView 16:9/4:3/21:9, 12FPS, 40px UI·PetFacility·BeastCompanionSystem 기능을 격리 검사.
- 이 작업에서 **실제 Unity 본게임 변경, 기존 Wolf Run 재수정, main 직접 Commit/Push/Merge 전부 0**.
