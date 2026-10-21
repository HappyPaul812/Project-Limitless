# LIMITLESS Chapter3 F1 — 3종 미술 HOLD/FAIL 표적 수정 v3 (2026-10-21)

> **ART_V3_DELIVERED / KEEP_3_BYTE_IDENTICAL / OFFLINE_TECH_QA_PASS / CODEX_DELTA_QA_PENDING / USER_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED**

- 담당 브랜치: `docs/art-new`; GitHub Issue [#30](https://github.com/HappyPaul812/Project-Limitless/issues/30).
- 승인 근거: `docs/planning/문서/00_프로젝트/20261021_Wolfv4_F1v2_QA_TargetedFix_Orders.md` 및 Issue #30 Codex v2 기술 PASS / 미술 3 PASS, 2 HOLD, 1 FAIL 판정.
- 이번 작업은 이전 v2 ZIP `Limitless_Chapter3_NorthF1_Environment_6_ArtImprovement_v2.zip`을 실제로 열어 그 안의 6개 `Art/` PNG만 기준으로 검사 및 보완했다. 원본 Pilot6·v2 전체6·몬스터 승인 Idle/Combat·보스·마을·Chapter1/2 공식 자산·Unity·Save는 수정하지 않았다.

## 게임용 후보 PNG 6종 (Art/)

| 파일명 | 구분 | SHA256 |
|---|---|---|
| `NorthF1_Ground_Earth_Base_v2.png` | **KEEP / 이전 미술 PASS / 100% 바이트 동일** | `ccf2c22e90bee292ecf2d0d030dad53e0032457272ebb94fccdf6491fabcf8b9` |
| `NorthF1_Ground_ThinFrost_Base_v3.png` | **HOLD 보완 v3**: 눈에 띄는 큰 밝은 서리 덩어리 감소, 흙 재질/지형 유지 | `12fe03d156944e1b72aec4e807770ee8016c61fe1de5804366819e716b97c30c` |
| `NorthF1_EarthToFrost_Transition_Overlay_v3.png` | **FAIL 보완 v3**: 투명 알파·서리 밀도 우향 경사, 옆 얇은 서리 타일 접합 | `93126dd1cda0f3b97823ae4756344600ed7f6af619eb42013d1ec45a4933c4d1` |
| `NorthF1_Path_WornTrail_Overlay_v2.png` | **KEEP / 이전 미술 PASS / 100% 바이트 동일** | `e78b41d48707dfce6a79dd4e826f864c5c2608b45a0f457af414ee9768a20175` |
| `NorthF1_Prop_FrostShrub_01_v2.png` | **KEEP / 이전 미술 PASS / 100% 바이트 동일** | `6c83c88881e7f89245ed580d50424b2671629ad9491da93f247aab99bfe4746e` |
| `NorthF1_Prop_Waymarker_01_v3.png` | **HOLD 보완 v3**: 왼쪽 뾰족한 끝을 단단한 꼬리 형태로 바꾸고 오른쪽 화살촉 명암 강조 | `6e55b64dbc2e906a50cf8b7310849bb156a90bc40436f28a86832e59e232b589` |

- 6장 모두 **256×256 RGBA**. 3 KEEP 파일은 v2 PNG 실제 압축 내부 데이터와 SHA가 같음(재인코딩/저장 안 함).
- **방향 주의:** 표지판은 그림의 기본 방향을 **오른쪽**으로 뚜렷하게 보여주는 시안이고, Main22 실제 길 안내 방향·목적지·퀘스트 텍스트를 추가하거나 확정한 것이 아님.

## 신규 ZIP 실물 / QA 기록

- ZIP: `Limitless_Chapter3_NorthF1_Environment_6_TargetedFix_v3.zip` (**이번 ChatGPT 대화 다운로드 첨부**).
- ZIP SHA256: **`488a65c051d1d017787771e0169124e47437b472911caa9949fdd59e55079054`**, 5,524,917 bytes, 내부 파일 **20개**, CRC `testzip()==None` PASS.
- 구성: `Art/` 6 PNG, `Preview/` 합성/전후/128px/표지판/전체 컨택트 등 5 PNG, `QA/` 얇은서리 v2/v3 2×2·3×3 총 4 PNG + 새 전환부 v2/v3 실제 3인접 타일 합성 2 PNG + `Offline_QA.json` 1, `Manifest.md`, `SHA256SUMS.txt`.
- **오프라인 타일 테스트:** Earth/ThinFrost v3 RGB/RGBA 상·하·좌·우 에지 일치. `Earth Base` | `Earth Base + Transition v3 RGBA Overlay` | `ThinFrost v3 Base`를 1:1 배열했을 때 **왼쪽/오른쪽 합성 경계의 최대 RGBA 픽셀차 0**. 이는 **지정된 1방향 조합에만 해당**하며 임의 그리드/코너·오토타일이나 실제 Unity Import를 보증하지 않음.
- **서리 패턴 밀도:** Earth 기준 RGB 밝기 차이가 큰 픽셀(색차 유클리드 >28)의 수가 v2 **4,358px → v3 1,569px**로 감소. 이는 서리의 큰 밝은 패치 반복감을 약화하는 정적 참고 수치이지 화면 적설 비율이나 실제 반복무늬 제로 판정이 아님. 3×3 반복 미리보기에 단일 타일 복제의 고유 패턴이 일부 보일 수 있음.
- **오프라인 합성:** `F1_v3_Path_Frost_Connection_OFFLINE.png` 및 `F1_Transition_v2_v3_Adjacent_Comparison_OFFLINE.png`은 Python/Pillow로 자산을 합친 이미지. Unity GameView 스크린샷 아님. 플레이어/휠체어 통행폭·PPU/TileGrid/Collider/Sorting/Scene 동작 미측정.
- 새 표지판의 가독성은 `F1_Waymarker_Direction_v2_v3_256_128.png`에서 256px/128px 동일 표시크기로 비교 가능.
- 구체 수정은 기존 PASS 파일을 유지하고 딱 3개 PNG에만 적용했다. 전체 아트 재생성, 신규 몬스터나 보스/마을/F2~F5 제작, Unity 코드/GUID/원본 변경 없음.

## 검증 요청 및 승인 게이트

1. **Codex Delta QA:** KEEP 3 바이트/SHA 동일, 신규 3개 PNG RGBA와 Alpha, 지면 v3 2×2/3×3 반복/미술 반복감, 전환 경계 실제 인접 타일/투명도, 표지판 128px 방향 가독성만 검사. 기존 전체 QA 일괄 재실행 불필요.
2. **대표 최종 미술 승인 별도:** F1 전반 미술 판정은 검수 전 계속 **HOLD**. 서리 텍스처/게임 실표시 크기에서의 가독성은 사용자·Codex 피드백 필요.
3. 미술 승인 이후에만 Codex 실제 F1 Import/Runtime/Main22 작업 단계 의뢰 가능. Unity 실제 적용·GitHub main Push/병합 금지.
4. GitHub에는 이번 **QA 문서/인계 텍스트만** 저장했고 PNG/ZIP 바이너리는 GitHub에 올리지 않았음. 실물은 ChatGPT 다운로드 링크를 통해 전달됨.
