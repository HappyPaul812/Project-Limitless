# LIMITLESS Chapter3 Main23 — 서릿길 마을 VariantTiles v7 6종 모티프 재설계 및 오프라인 QA

- **담당:** Chapter3 신규 아트 / **브랜치:** `docs/art-new` / **날짜:** 2026-10-22 / [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45)
- **상태:** `V7_ART_CANDIDATES_DELIVERED / OFFLINE_FORMAT_SEAM_PASS / TILE_VISUAL_ART_HOLD_NOT_CLEARED / CODEX_UNITY_DELTA_QA_PENDING / USER_ART_APPROVAL_PENDING / NOT_INTEGRATED`
- 최신 `SPRITE_ART_LATEST.md`, `docs/planning/PLANNING_LATEST.md`, Issue #45 v6 Codex 댓글 확인. v6 기술PASS, 5×5 혼합에서도 Earth/ThinFrost/Gravel 반복무늬 미술 HOLD. 이번 v7은 **모티프 위치 교환 반복 금지**, 원형·토양/자갈/서리 분포 자체 차별화 목표.

## 1. 기준 실물 및 납품 ZIP

- 읽기 원본: `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v6.zip`, 실제 이 대화 환경 ZIP SHA256 **`519ed96fa6157b7de451830412d6e881b633241b0f8cc459d186465765a4ea7e`** (사용자 로컬 F: 경로 자체에는 접근하지 않음).
- **v7 최종 실제 ZIP:** `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v7.zip`, SHA256 **`d499f08481751c0fcda3298bbd37513466168b73105dd224b2cb767651bb691c`**, 30,503,515 bytes, ZIP 멤버 **88개**, ZIP CRC `testzip()==None`, **SHA256SUMS 87/87 PASS**.
- PNG **49** = 이전 v5→v6 기반 **43 PNG 바이트·SHA 43/43 동일** + 개별 변형 02/03 **6 PNG만 변경**. 추가 Overlay나 Unity 구성 요소를 임의 생성·확정하지 않음.
- ZIP 바이너리 실제 납품 경로: 이 ChatGPT 대화의 다운로드 첨부. **GitHub에는 이 문서·인계만 기록**, PNG/ZIP 바이너리를 GitHub나 대표 PC F:에 직접 배포/반영한 것으로 주장하지 않음.

## 2. 실제 수정한 6 PNG

| 변경 파일 | v7 PNG SHA256 | 실질 미술 변경 |
|---|---|---|
| `Tiles/Frostway_Ground_Earth_02.png` | `22a19d176c842a38b94385209003bf7dd202cd10d36ad3a634db05a6997389f4` | 흙의 다른 표면 입자와 서로 다른 각진 편암·성긴 풀 실루엣 |
| `Tiles/Frostway_Ground_Earth_03.png` | `d5c2786e9de75d5a2aa5b5734e13c32bbea56cf22115f7de6a738ba8718aad6f` | 잔균열·다른 편암 조각·고립 식생; 02의 형태를 그대로 옮기지 않음 |
| `Tiles/Frostway_Ground_ThinFrost_02.png` | `0c2262272b7c3aeabd0c6afdd16d7c66fdf5b3928ed67666f3d4cac26cc67c88` | 적은 눈 밀도, 깃털형 국소 서리 띠·갈색 흙 위 얇은 결정 |
| `Tiles/Frostway_Ground_ThinFrost_03.png` | `6596010f506dfdf3078d8be929ac89bdfecaa71f9b18fe52adab00b4d9f37c80` | 02와 다른 고립 석재·서리 결정 분포 및 냉색 흔적 |
| `Tiles/Frostway_Ground_Gravel_02.png` | `12ac969c0ae7deaa4d7cb0fe27fe73495cfddfa98773cb885c290b03e31183e3` | 소형 각진 쇄석 군집·여백이 있는 흙길 리듬 |
| `Tiles/Frostway_Ground_Gravel_03.png` | `012e607523da894e0b243b93edc2f434598bf13252725720bb3818f21dfd4d84` | 다른 퇴적 굴곡·작은 자갈 비대칭 밀도·편평 암편 |

### 제작 방식

- 소스 01의 RGB 분포 및 미세 텍스처 주파수를 소재 기준으로 추정하고, 기존 인지 가능한 풀·돌 패치를 그대로 복제/재배열하지 않도록 **새 미세 배경 텍스처를 합성**한 뒤, 모양이 다른 돌·풀·서리 결정과 불규칙 분포를 별도로 작성.
- 256×256 RGBA, 알파 255. 소스 01과의 이음새 유지를 위해 가장 바깥 픽셀을 맞추고 안쪽은 위치에 따라 달라지는 짧은 feather 영역으로 소재를 자연 연결. 고정 12px 테두리를 단순히 확대하는 처리 대신 변하는 연결 폭 사용.
- 서로 다른 지형 간 경계(Earth↔ThinFrost, Gravel↔StonePath) 및 실제 맵 TileGrid/PPU/Renderer는 이번 이미지 QA의 PASS 범위 **아님**.

## 3. 오프라인 QA

- PNG 6/6 규격·RGBA·알파 PASS; **기존 43/43 바이트 동일**.
- 동일 지형 안의 ordered 조합 01/02/03 각 3×3가로9·세로9 = 18 neighbor boundary cases/family, **세 지형 모두 최대 RGBA edge 차이 0**. *이 측정은 1픽셀 경계 색 일치이지 화면에서 그리드 감지가 전혀 없다는 뜻이 아님.*
- `Preview/V7_NineTiles_Original256px.png`: 원본01과 새 02/03을 원 해상도에 표시.
- `Preview/*_v6_v7_3x3_SameScale_256pxPerCell.png`: 768×768 원해상도, 좌 v6 / 우 v7 같은 tile layout.
- `Preview/*_v6_v7_5x5_SameScale_128pxPerCell.png`, `Preview/*_v6_v7_8x8_SameScale_128pxPerCell.png`: v6/v7 각각 같은 무작위 tile ID 순서로 만든 **오프라인 동일 크기 비교**. 5×5/8×8은 각각 타일을 **128px/셀로 축소한 미리보기**, Unity GameView 캡처 또는 게임 내 PPU 실측 크기가 아님.
- `QA/Repeat/`에 v6·v7 3×3/5×5/8×8 혼합 및 **v6 01 단독 8×8**를 별도 보관; `QA/Seams/`에 01→02 실제 맞대기, `QA/Motifs/`에는 정중앙 128×128 범위 2배 픽셀 확대 전후 자료 보관.
- 변형들끼리 중앙 240×240 영역의 SSIM 비교 지표 (동일 형상은 1에 가까움):
  - Earth: v6 01↔02=0.7754, 01↔03=0.7985, 02↔03=0.6951; v7 각각 **0.2609, 0.2504, 0.1603**
  - ThinFrost: v6 0.7801/0.8018/0.6986 → v7 **0.2383/0.2463/0.1604**
  - Gravel: v6 0.8095/0.8004/0.7490 → v7 **0.2481/0.2340/0.1633**.
  - *SSIM 감소는 **형태/패턴 유사도 감소 지표**이지 공식 픽셀아트 퀄리티·불필요한 격자 부재를 증명하지 못함.*

### 중요한 미술 한계

- **시각 완성도 PASS라고 주장하지 않음.** 실제 8×8 오프라인에서 기존 01과 질감이 새 02/03 사이에 차이가 존재하여 큰 구획이 느껴질 수 있다. 01을 그대로 보존해야 하는 보호정책과 새 모티프 차별화를 동시에 지키면서 나타나는 현상이며, **아직 미술 HOLD 해제 미확정**이다.
- 유사도 수치가 극적으로 감소해도 비주얼 품질은 대표님 및 Codex 실제 Unity 동배율로 재검토 필요. 01의 기존 모티프는 계속 반복되므로 자동 Tile 배치 정책/추가 Overlay는 이번 미확정.
- 원본 43개 중 SkillShop·Roof Gable·StonePath/비활성 포탈/건물·문 등 모두 그대로; F1·Wolf·Bear·Fox·다른 Chapter3, Chapter1/2·Unity 본게임·Save/UserData 원본 수정 **0**.

## 4. OPEN16 실제 근거

- GitHub Issue #45와 `PLANNING_LATEST.md`: Codex가 OPEN16 원본 CSV/좌표 JSON을 **로컬에서 확보한 사실**은 확인. 그러나 **16개의 개별 행 원문·파일명·좌표·재현조건은 이 아트 세션과 현재 GitHub 문서/댓글에서 열람 불가**; 파일 Library 검색에서도 내용 확인 불가.
- 따라서 항목별 연결 여부 **미확정**, 기존 43 PNG는 하나도 수정하지 않았고 **OPEN16 해결 0 / 미해결 16 유지**. 사유/심각도/좌표를 만들어 보고하지 않음. `QA/OPEN16_Evidence_Status.md` 명시.
- 추후 Codex가 16행 증거를 Issue #45 또는 원문 기획문서에 공유하면 6개 타일과 직접 연관된 행만 검토.

## 5. 이후 Gate

1. Codex 격리 v7 Delta QA: 이 대화 ZIP 실제 SHA/43KEEP SHA, 6개 PNG Import 설정, **5×5/8×8 실제 Unity GameView**, 크로스 타일 접합·정렬·휠체어 통로·동일 기준 v6 비교, 화면 내 격자 인지 여부 확인. 기존 합격 전체 Runtime QA 반복 금지.
2. 바울 대표 **마을 미술 최종 승인 필수**. 미술 HOLD 유지 중 본편 Main23 적용·새 포탈 동작·TileGrid 자동 배치 확정 금지.
3. 작업 브랜치 `docs/art-new`만 문서 작성, main 직접 Commit/Push/Merge 금지.
