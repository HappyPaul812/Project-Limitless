# LIMITLESS Chapter3 F1 북풍의 들머리 — 신규 환경 6종 개선 v2 아트 납품/QA (2026-10-21)

**브랜치:** `docs/art-new` · 담당 [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30) · **상태:** `6_ART_CANDIDATES_DELIVERED / OFFLINE_TECH_QA_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_INTEGRATED`

## 발주 및 원본 보호

- Issue #30 최신 Codex/기획 댓글: F1 원본·지면 후보3 모두 HOLD. Main22 첫 플레이 F1을 우선하고 마을/F2~F5/설빙의 폭군 신규 보스 전투 애니메이션 작업 제외.
- `Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip`에서 기존 F1 PNG6을 그대로 읽어 **바이트 동일 복사**. 기존 `Limitless_Chapter3_NorthF1_ArtReview_Samples_v1.zip` 후보3과 승인 Monster Idle/Combat/1Pose·Chapter1/2 원본은 수정하지 않음.
- 새 개선 그림 소스6은 고해상도 픽셀아트풍 원화를 기반으로 256×256 RGBA에 맞춰 분해·정리한 **미술 재시안**이며, 손으로 픽셀 단위 완성 마감한 공식 Game-ready PNG라고 단정하지 않음.

## 실제 납품 결과

- ZIP: **`Limitless_Chapter3_NorthF1_Environment_6_ArtImprovement_v2.zip`**. ZIP SHA256: **`8158f501763b525022b8b9fc1645fe2b0a5c0bc8bf9f73aabcb02024c3a3235c`**.
- ZIP 크기 **3,897,421 bytes**, 파일 **23개**, `ZipFile.testzip() == None`. 이번 ChatGPT 대화의 다운로드 첨부로 PNG/ZIP 실물 전달. **GitHub에는 바이너리 PNG/ZIP 미등록** (메타·QA 기록만 등록).
- 폴더: `Art/` 개선 후보6 PNG, `Original_Reference/` 원본 파일 바이트 동일6 PNG, `Preview/` 후보 접촉판·원본/후보 동배율·오프라인 연결 배치·Chapter2 공식 아트 비교 4 PNG, `QA/` Earth/Frost 각 2×2·3×3 반복 PNG4·Offline_QA.json, `Manifest.md`, `SHA256SUMS.txt`.

| 후보 PNG `Art/` | SHA-256 | 미술 방향 |
|---|---|---|
| `NorthF1_Ground_Earth_Base_v2.png` | `ccf2c22e90bee292ecf2d0d030dad53e0032457272ebb94fccdf6491fabcf8b9` | soil/pebbles/grass, edge-match repeat |
| `NorthF1_Ground_ThinFrost_Base_v2.png` | `5fcb2da32042a83beb036797d3e9eeb9a5e3cf0016062f88dd66d012fc969b85` | earth-dominant frost, edge-match repeat |
| `NorthF1_EarthToFrost_Transition_Overlay_v2.png` | `7d11443186fcad691a5830a4a959f3a3d9076b613669b8db26c4fe56a5551aa1` | right-heavy, directional alpha transition |
| `NorthF1_Path_WornTrail_Overlay_v2.png` | `e78b41d48707dfce6a79dd4e826f864c5c2608b45a0f457af414ee9768a20175` | contrast-enhanced worn path, no collider proof |
| `NorthF1_Prop_FrostShrub_01_v2.png` | `6c83c88881e7f89245ed580d50424b2671629ad9491da93f247aab99bfe4746e` | frost branch silhouette |
| `NorthF1_Prop_Waymarker_01_v2.png` | `40036738e7da0c25df5c0b69aacba37c3d6e4a681d566fa3a5d30d7caea7431d` | wooden waymarker |

## 오프라인 기술 QA와 남은 문제

1. **PNG 6/6** 실제 디코딩 및 **256×256 RGBA** PASS. Ground2는 반투명 프레임이 아닌 불투명 Base, 길/전환부/관목/표지판은 알파 이미지.
2. Earth Base·Thin Frost Base의 **상하 및 좌우 첫/끝 픽셀 배열 100% 일치**, 각각 2×2·3×3 반복 검토 이미지를 생성. 이는 가시적 큰 문양 반복이 절대 없다는 미술 PASS는 아님; 규칙적 타일 패턴은 여전히 실제 화면에서 검토해야 함.
3. 원본 6개의 SHA는 원본 파일럿 ZIP의 해당 엔트리 바이트와 직접 대조. **6/6 원본 SHA 보존**, 수정 아트는 `_v2` 파일명으로만 별도 기록.
4. `Preview/F1_Earth_Frost_Path_Composite_OFFLINE.png`: PIL로 개별 PNG를 조합한 **오프라인 합성**. Unity GameView/Scene 실행 스크린샷이 아님. 초반 **흙 우세** + 길 선명도 강화 + 국소 서리 포켓 + 관목과 길 표지판 배치 제안.
5. `Preview/F1_vs_Chapter2_Official_Environment_OFFLINE.png`: 실제 Chapter2 `DeepCore_Ground_Base.png` 참조 사용, 256px 비교용 별도 표시. Chapter2와 F1의 공식 PPU·WorldScale이 같다는 뜻 아님.
6. **잔여 미술 HOLD**: 단일 Base 반복 시 3×3에서 문양 반복 인지가 가능함. 전환부는 우측 서리 밀도 가중 **1방향 오버레이**, 완전 오토타일/4방향 연결 보장 아님. 투명 오버레이의 작은 바위는 미술상 독립 요소일 수 있으므로 자동 stray 제거가 충분한지 검수 필요.
7. **Unity 기술 미검증**: PPU, TileGrid, Pivot, Collider, 실제 휠체어 통로 폭, Main22 플레이어와의 명도 가독성은 Codex 실제 Unity 계약 후 확인. 이번 산출로 제작/승인을 일괄 PASS 처리하지 않음.
8. `main` 직접 커밋·Push·병합 없음. 기존 Unity/Save/UserData/몬스터/보스/F2~F5/마을 이미지 미수정.

## 미술 승인 게이트 / 다음

- 바울 대표가 이 ZIP의 6종과 `Preview/`, `QA/`를 보고 **미술 승인 혹은 세부 표적 수정** 지시.
- 미술 승인 뒤 Codex가 실제 F1 TileGrid·PPU·Pivot·Collision·장애인 접근 가능 통로와 기존 공식 스타일을 Unity GameView에서 검증. 기존 QA 전수 반복이나 Unity 임의 업데이트 금지.
