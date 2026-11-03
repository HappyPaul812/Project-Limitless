# LIMITLESS / 리뉴얼 아트 / Issue #60 / 2026-11-03 — Walk8 마지막 캔버스 잘림 표적 수정

**브랜치:** `docs/art-renewal` · **본게임 미적용** · **미술 Walk8 접지 검토 필요**.

## 1. 기준 및 최신 검수
- 확인 근거: `WORLD_ART_RENEWAL_LATEST.md`, Issue #60의 [Codex 새 클리핑 4개 검수](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6595712350) 및 [기획 마지막 표적 수정 계약](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6595844892).
- 원본 승인 `ForestSpider_Organic_v5_FORELEG_CONTOUR_ALPHA_512.png` SHA256 **`c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`** 바이트 보존. 원본 Idle8 전체 SHA도 이전 파일에서 읽어 QA에 기록하며 **Idle8 자체는 재제작/덮어쓰기0**.
- 수정 기준 ZIP `LIMITLESS_WALK8_FIX.zip` SHA256 **`d840e5ddd8ac2666e4b0c7a9f4b2d0bdfc688b5ff03e01a0019ba7808d6b86b8`** (Codex 검수 일치). 이전 Walk8에서 **01·02 오른쪽 x511 / 06·07 왼쪽 x0**에 걸친 다리끝이 잘렸으므로 그 네 프레임만 표적 보완.

## 2. 실제 최종 산출물 / 기술 검증

- **실물 ZIP**: `LIMITLESS_WALK8_EDGE_FIX_20261103.zip` / **4,431,916 bytes** / **SHA256 `771bd1813608aa3e350c6124ab0d43293c6d2988b1833014da64484d5272c8da`** / **20 entries** / **ZIP CRC PASS**, Manifest 내부 18/18 파일 SHA256 재검증 PASS.
- Walk8 각 셀 **512×512 RGBA**, 시트 `Walk8_4x2_512_RGBA.png` **2048×1024 RGBA**, `GIF/Walk8_128px_10FPS.gif`, `GIF/Walk8_40px_10FPS.gif`: 각 8프레임·100ms = 정확히 기존 Runtime **10FPS**.
- 수정 전후 확대 `Review/Walk01_02_06_07_Edge_BeforeAfter_Enlarged.png`, 전체 128/40 실축소 `Review/Walk8_BeforeAfter_128_40.png`, 지지/스윙 동작 순서 **계획표** `Review/Walk8_ContactPhase_10FPS_OFFLINE.png`, `QA/QA_Results.json` / `QA/Walk8_Frames_SHA.csv` / Manifest / SHA256SUMS 수록. 비교판은 **OFFLINE**, 실제 GameView가 아님.
- **캔버스 잘림 검증:** Walk01 오른쪽 x511 y277–302 **가시 알파 26→0**; Walk02 오른쪽 x511 y262–288 및 294–296 **30→0**; Walk06 왼쪽 x0 y328–385 **58→0**; Walk07 왼쪽 x0 y345–351·353–371 **26→0**. 알파는 기존 다리 끝을 내부 방향으로 국소 리매핑한 뒤 **곡선형 끝단으로 서서히 좁히는 형태**이며 전체 축소/사각형 일괄삭제가 아님. 전신 컨셉 재제작/메카닉화0.
- **바이트 KEEP 프레임:** Walk00 `cc1b5670...`, Walk03 `64a7297d...`, Walk04 `7de4a98b...`, Walk05 `435ed6e6...`. 해당 4개는 기존 ZIP과 **SHA 전체 동일/변경 픽셀0**, Walk01/02/06/07만 실제 변화. Idle8/승인 원본 모두 변경0.

### 실제 변경 PNG SHA·픽셀 수

| 수정 | 새로운 SHA256 | RGBA 변경 픽셀 |
|---|---|---:|
| Walk01 | `550ad6615e5315ee2d4680bf2c885a03abd1b422a5d4fce5074a1ead971f0d64` | 8,813 |
| Walk02 | `c4935be601a362682711fec1824293b57f3215247daca4c62a85ae5b0cae3547` | 9,413 |
| Walk06 | `29a0ae62f37f2044c8b3dcd206bc3456c2ee141484f79c5bfe224c35566edf49` | 10,604 |
| Walk07 | `edf725d87aafafcb81d01f09b121bf1e20a71b4fbfd829dbd1b0b6a3abf1b189` | 10,446 |

## 3. 보행 판정 — 잘림 기술 PASS ≠ 자연 접지 미술 PASS

- 새 조정은 기존 텍스처를 유지하는 **끝부분 픽셀 리매핑+비직사각형 테이퍼링**으로, 다리를 독립적으로 수작업 재묘사하거나 실제 8다리 IK/발 지면 고정을 구현한 것은 아니다.
- 따라서 정확한 결과는 **`EDGE_ALPHA_TECH_PASS / IDLE8_KEEP / WALK4_CHANGED / WALK4_KEEP / WALK_CONTACT_ART_REVIEW_PENDING / GAMEVIEW_NOT_TESTED / NOT_UNITY_APPLIED`**.
- 비디오·Unity 이동 속도/실 지형 위 지지 발끝의 궤적은 미측정. 스윙/지지 순서 계획표를 **실제 발고정 PASS 증거로 사용 금지**. Walk8 미술이 아직 자연스럽지 않으면 같은 픽셀 워프 반복 대신 다리별 마스크·관절 키포즈 수작업 작화로 전환.
- Attack8·Shoot8 신규 제작0, VenomBee 신규 제작0/권리 CC BY3.0·4.0 충돌 HOLD, Chapter2 공식 몬스터·펫 Wolf/Bear/Fox·Chapter4 기계형 Spider v3 `ARCHIVE_ONLY`, 원본 Unity/Scene/Prefab/MonsterDefinition/GUID/meta/AI/독/드롭/Spawn/Save/UserData 및 main 변경0.

## 4. 다운로드 장애 대비: 검증된 GitHub 로컬 재현 경로

바울 대표가 여러 차례 ChatGPT ZIP 다운로드에 실패했으므로 **Codex가 실제 보유 중인 원본 ZIP 두 개로 로컬에서 수정 파일을 만드는 Python 스크립트도 GitHub에 저장**:
- [Walk8_EdgeContact_Rebuild_20261103.py](Walk8_EdgeContact_Rebuild_20261103.py) — `docs/art-renewal` commit **`8a6c16c068e71e57341e9a457583678dce0f60e1`**.
- 입력은 **기존 검증된** `LIMITLESS_WALK8_FIX.zip`(현재 8장)과 `LIMITLESS_Spider_Idle8_Walk8.zip`(승인 v5·Idle8 확인용); 경로를 Codex가 로컬에서 탐색하여 명시적으로 지정. 산출 위치는 **격리 Temp 폴더만**. 파일이 없으면 `FILE_NOT_FOUND` 보고, 임의 대체·다운로드 링크 재시도 반복하지 않음.
- 이 텍스트 스크립트는 실제 사용한 **4프레임 내부방향 국소 리매핑 및 경계 테스트**를 재현하며 신규 Walk8 프레임·시트·100ms×8 GIF·파일 해시 QA를 로컬 생성. PNG 바이트 SHA는 로컬 Pillow/OpenCV 버전과 인코더에 따라 달라질 수 있으므로 **원본 보존/실제 RGBA 픽셀/경계=0**을 비교 기준으로 하고 Unity original에 임포트하지 않는다.

**후속 승인:** 바울 대표 Walk8 미술 검토 → Codex 격리 10FPS/PPU256 실제 지면 이동·지지발 정지/루프 접지 QA → 최종 승인 전 모든 본게임 교체, main Push·병합 금지.
