# LIMITLESS — VenomSnake/Wolf v3 실제 개별 PNG 임시 납품 및 Field09 v2 제작 보류 사유 (2026-10-19)

## 핵심 구분
- 기획 정본 `docs/planning/문서/00_프로젝트/Field09_v1_VisualFail_And_MonsterWolf_KeyArt_Immediate_Art_Production_20261019.md` 직접 확인.
- 바울 대표가 지적한 것처럼 이전 사진식/홍보판식 생성 이미지는 **실제 공식 5종 이미지와 실물 배율을 측정한 비교가 아니었고**, Field09 전후 픽셀 수정 실적이 없는데도 완료처럼 보일 위험이 있었다. 그런 보드를 `QA_PASS`, `OFFICIAL_COMPARE`로 사용 금지.
- 새 단일 원화 파일 **2개는 현재 세션 컨테이너에 실제 PNG로 저장·PIL 전체 디코드/Alpha/SHA 검증 완료**, 그러나 실제 Chapter2 스타일/동일 World Scale 미술 비교 및 사용자 미술 승인 **미완료**.
  - `VenomSnake_KeyArt_v3.png`: RGBA 1254×1254, 2,285,408 bytes, SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0`. 이미지 생성 후 투명 배경 독립원화. v2 32px 확대 아님.
  - `Wolf_KeyArt_v3.png`: RGBA 952×1306, 1,022,752 bytes, SHA256 `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`. 별도 생성한 고해상도 늑대 이미지에서 배경 마스킹한 원화 후보. 64×40 v2 확대 아님; 털 테두리/기존 배경 마스크의 미세 잔류 검토 필요.
- PNG2 실물은 **해당 ChatGPT 세션의 sandbox**에서 파일로 납품됨. GitHub 아트 바이너리/대표의 F: 디스크에 자동 업로드된 것은 아님. 후임은 파일명과 SHA를 확인.

## 공식 실물 비교 미완료 — 허위 결과 금지
- GitHub `main` 내 `Unity/Client/Assets/_Project/Art/Monsters/11_SootHound`~`15_EmberWraith` 공식 PNG 파일 존재/경로/크기 등 **메타데이터만 확인**, 네트워크/커넥터에서 PNG raw 바이너리 전부를 현재 모델 로컬 컨테이너에 안전하게 내려받지 못했음. 그러므로 공식 5종 이미지의 실제 동일 World Size 비교판을 제작했다는 주장은 거짓임.
- 기존 메타에서 314×314 셀/PPU314 일부 확인 사례는 실물 스케일 검증의 출발점일 뿐 실제 렌더·화면샷과 다름.
- 바울 대표가 v3 KeyArt 스타일을 승인하기 전 Snake20 + Wolf6 프레임 전체 애니메이션 및 Bear/Fox 제작 없음. Sprite READY/Unity Applied 아님.

## Field09 보완 v2 BLOCKED
- Codex 전건 `FIELD09_ZIP_REPACK_PASS / TECH_QA_40PASS / MAIN18_DELTA_92PASS` 및 **`USER_VISIBLE_ART_VISUAL_FAIL`** (지면 6 반복띠+소품 6 흰 가이드선) 기록 확인. 고쳐지지 않은 v1의 단순 재포장으로 처리해서는 안 됨.
- 실질 v2 수리는 **실제 실패 화면 좌표/원본 Main18 Environment 8 PNG 픽셀/v1 재포장 정본**을 내려받아 먼저 직시해야 하는데, 이 세션 작업 컨테이너에는 이들 공식 PNG와 실패 스크린샷이 없어 안전한 바이트 보존 픽셀 보완이 불가능. 따라서 Field09 v2 PNG8/ZIP은 이번에 **실제 제작·검증·납품했다고 보고하지 않음**.
- **Codex 로컬 읽기 전용 인계 필요 최소 범위:** 현재 v1 복구 ZIP의 수정 PNG8, 공식 원본 PNG8, 실제 Unity 화면 Before/After 및 각각 실패 위치 좌표·레포트의 파일 경로. 이를 아트 담당이 직접 열람 가능한 아티팩트로 제공받으면 실제 잔여선 중심 Pixel Fix -> `Changed_Pixels.csv` -> Manifest/SHA -> 표준 ZIP testzip -> 3해상도 재검수 단계 진행.
- 원본 공식10 PNG/meta/GUID, Scene·Save, 승인 이미지, v1/v2 ZIP 모두 보호. `main` 직접 Commit/Push/merge0.

**현재 정확 상태:** `KEYART_V3_2PNG_FILE_DELIVERED / OFFLINE_IMAGE_DECODE_PASS / USER_ART_STYLE_REVIEW_PENDING / OFFICIAL_FIVE_REAL_SCALE_COMPARE_BLOCKED / FIELD09_V2_PNG_NOT_PRODUCED / FIELD09_V2_ZIP_NOT_PRODUCED / UNITY_UNCHANGED`.
