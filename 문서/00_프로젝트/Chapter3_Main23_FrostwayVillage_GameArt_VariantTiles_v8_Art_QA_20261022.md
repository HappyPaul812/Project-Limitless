# LIMITLESS Chapter3 Main23 — Frostway Village VariantTiles v8 질감 통일 표적보완 (2026-10-22)

> **V8_DELIVERED / OFFLINE_FORMAT_PASS / 43_ORIGINAL_PNG_BYTE_PRESERVED / USER_ART_APPROVAL_PENDING / CODEX_V8_UNITY_DELTAQA_PENDING / MAIN23_NOT_INTEGRATED**

- **담당·브랜치:** Chapter3 신규 아트 / `docs/art-new`, [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45).
- **최신 Codex 실제 v7 격리 QA:** `codex/qa-village-v7-bear-fox-v2-20261022` 커밋 `f76d8dbf`/`8982d40a`. 원본43 byte 동일, Unity Camera/RenderTexture 16:9/4:3/21:9와 5×5/8×8 및 통행 proxy v6/v7 6/6 기술PASS. 그러나 01과 02·03 간 **미세 입자 크기/명암/바탕·풀/돌 밀도**가 달라 사각 격자 느낌이 남아 Earth/ThinFrost/Gravel 미술 HOLD. 정본 `docs/planning/팀운영/인계/PLANNING_LATEST.md` 및 Issue 최신 댓글 참고.
- **v8 수정 계약:** 기존 01 포함 43 PNG 바이트 보호, **02·03 여섯 장만** 표면질감·미세/중간 주파수 명암 리듬 교정; 새 돌·풀·서리 모티프 추가 금지; Unity 새 자동배치·Overlay 임의 확정 금지.

## 1. 실제 파일 납품

- **읽은 v7 실제 ZIP:** `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v7.zip`, SHA256 `d499f08481751c0fcda3298bbd37513466168b73105dd224b2cb767651bb691c`; ChatGPT 작업 환경의 바이트 readback 확인. 사용자 Windows `F:\Downloads\` 경로에 직접 접근한 것은 아님.
- **새 v8 다운로드 ZIP:** `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v8.zip` (**ChatGPT 대화 다운로드 첨부에 실물 제공**).
- **ZIP SHA256:** `6a9fa1c3c0c1ffef3ac43d3541a28bbc0a16310d2e0f8c267ec7177d731d712f`; **40,921,124 bytes**, **98 entries**, ZIP `testzip()==None`, ZIP 내부 `SHA256SUMS.txt` **97/97 대조 PASS**.
- 게임용 PNG 총 49 = 기존 `43`개 ZIP 엔트리 바이너리 SHA/바이트 **43/43 동일** + 신규 v6 이후 후보 02/03 **6개 교체**.
- 문서·QA: `Manifest.json`, `Manifest.md`, `ChangeLog.md`, `SHA256SUMS.txt`, `QA/Offline_QA.json`, `QA/OPEN16_Evidence_Status.md`, 파일별 v7↔v8 비교/혼합 Tile Preview, seam2×2 이미지 등.
- **원본 보호:** 01 각 3종, StonePath·SkillShop·Roof Gable·비활성 포탈·기존 각 건물/간판·상점/소품 모두 byte identical. F1, Wolf, Bear, Fox, Chapter1/2, Unity/Scene/Save/UserData/GUID 건드리지 않음.

## 2. 정확히 수정한 게임 PNG 6장

| 파일 (Tiles/) | 새 SHA256 | 변경 이유 |
|---|---|---|
| `Frostway_Ground_Earth_02.png` | `248eb13eacd07e71bf570d5d2a0c1fd04e89fab45732ebc5f60407b83a0a20f2` | 흙의 작은 입자와 중간 질감 대비를 01과 조화; 새 v7 암편/식생 유지 |
| `Frostway_Ground_Earth_03.png` | `bb6224b1be20b9560e053910f21e201c663e2215e2462dee86cb82a3618fc6f8` | 고유 돌·풀을 유지하며 명도와 세부 입자 대비 조화 |
| `Frostway_Ground_ThinFrost_02.png` | `16c3fd3bb36e978e052950828a60b0031eab83cc20c0ed2366ee0ea59663cc7d` | 기존 국소 서리 패턴 유지, 흙 바탕·미세입자의 차이 감소 |
| `Frostway_Ground_ThinFrost_03.png` | `0bbf2a5f7fe166b854117f28c37df1b10bfb8052743505b3b0978efa46b89942` | 서리의 위치·실루엣 불변, 01에 맞춘 세부/중간 스케일 질감 |
| `Frostway_Ground_Gravel_02.png` | `905c1efabfa3efb22e1df179c1c6762c371d1999a41f25561a8d7074d8114a34` | 원래 각진 쇄석 군집 유지, 자갈 입자 대비/명도 통일 |
| `Frostway_Ground_Gravel_03.png` | `a5dead2b6acb5aa7ec6634f66ddc6bcead5b77c335ada9da130e55106f05f48f` | 다른 퇴적 돌무리 보존, 배경 바탕 질감만 정합 |

**보완 방법:** 단순 RGB 평균 필터나 전체 이미지 크기 변경 없이, 원본 01을 기준으로 각 색상 채널의 Gaussian 주파수 구간 **1.1 / 3.9 / 11.8px**의 fine·medium·coarse 명암 에너지를 별도 보정. v7 각 타일의 고유 공간 배치/형상·RGBA alpha를 보호하고, 외곽 RGB는 유지한 채 안쪽 약 11px에서 자연스럽게 보정 영역으로 전환. 새로운 무늬/오브젝트를 인위 추가하지 않았음.

## 3. 검증 — 숫자와 미술을 분리

- **49 게임PNG 모두 PNG 디코딩 PASS**, 6 변경 PNG **256×256 RGBA/alpha255**, 43 보호원본 **SHA/바이트 43/43 PASS**. 각 수정 02·03 alpha 채널은 v7과 일치.
- 같은 지형 01·02·03 조합의 `01→01 ... 03→03` 가로9+세로9 = **18 seam 조합/지형, 총54**, 모든 edge RGBA pixel max difference **0**.
- 밝기 표준편차와 Gaussian 대역별 미세입자 명암·저/중주파 질감 + 평균밝기 다섯 통계량을 이용한 **01 기준 특성 거리(단위: 복합 휘도 통계)**:
  - Earth 02: **6.96→1.06**, Earth 03: **7.21→1.05**
  - ThinFrost 02: **5.74→1.13**, ThinFrost 03: **6.05→1.17**
  - Gravel 02: **6.77→0.91**, Gravel 03: **6.65→0.76**.
  - **해석 주의:** 이는 측정한 질감 통계가 유사해졌다는 근거이지 *이미지의 돌/풀 모티프 동일성 또는 미술 PASS/그리드 제거를 입증하는 점수*가 아니다.
- v7↔v8 동일 배치 **3×3(256px/셀)·5×5(128px/셀)·8×8(128px/셀)** 각 family 3장 비교 및 **01 단독 8×8**, 접합 예시, 모티프 1:1 확대를 포함. 전부 **Python/Pillow 오프라인 합성**이며 **Unity GameView가 아님**.
- 시각 상 남을 수 있는 결함: **01에만 있는 큰 풀/돌 군집 모티프 vs 02·03의 작고 다양한 실루엣 차이**가 그대로 남고 01을 보호하므로 반복 구획이 완전히 사라지지 않을 수 있음. user art HOLD 해제를 스스로 선언하지 않음.

## 4. OPEN16 / Overlay 대안 검토

- Codex가 기존 **OPEN16 원본 CSV·좌표JSON을 로컬에서 확보**했다는 Issue·기획 보고는 확인. 그러나 이 art-new 작업환경에는 개별 16행 파일/좌표/재현정보가 공개되어 있지 않아 **OPEN16 해결0 / 잔여16**, 관련성 검증 전 다른 기존 PNG 수정금지.
- 01을 바꾸지 못하는 전제에서 시각 구획이 남으면, 기존 타일 RGB를 덮지 않는 **별도 투명 소규모 전환 Overlay 또는 불규칙 배치 변형**을 향후 기술 제안 가능. 다만 새 TileGrid/Sorting/랜덤 시드/Overlay 자동배치 규칙은 **Codex 기술 계약 선행**, v8에서는 후보 구현/파일 임의 추가 없음.

## 5. 변경·승인 Gate

1. Codex는 동일 01·02·03 혼합 5×5/8×8 **실제 격리 Unity Camera 16:9/4:3/21:9**를 v7과 동배율 비교; 검증 범위는 신규6/기존43 보존, seam·표면 격자/모티프 경계 시각, 통행 proxy 영향으로 제한. 기존 통과 Main23 Runtime 전체 재실행 불필요.
2. 바울 대표의 마을 **최종 미술 승인 대기**. 이번 오프라인 PASS로 마을 전체 미술 HOLD 자동 해제 금지.
3. GitHub **`docs/art-new` 문서만 커밋**, 바이너리 Github 업로드/본게임 Unity 반영/`main` 직접 커밋·Push·병합 모두 금지.
