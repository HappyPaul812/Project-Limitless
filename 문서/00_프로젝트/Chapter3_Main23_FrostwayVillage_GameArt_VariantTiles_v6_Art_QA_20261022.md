# LIMITLESS Chapter3 Main23 서릿길 마을 — VariantTiles v6 6종 신규 변형 타일 실제 납품 / 오프라인 QA (2026-10-22)

> **DELIVERED_VARIANTS_6 / V5_43_ASSET_BYTES_PRESERVED / SAME_TERRAIN_CROSS_SEAM_PASS / FIVE_BY_FIVE_PREVIEW_PASS / CODEX_DELTA_QA_PENDING / ART_HOLD_NOT_CLEARED / USER_FINAL_ART_APPROVAL_PENDING**

- 담당 신규아트: `docs/art-new`; [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45)
- 우선 인계 `팀운영/인계/SPRITE_ART_LATEST.md`(`docs/art-new`), `PLANNING_LATEST.md`(`docs/planning`) 2026-10-21 최신 EOD; Issue #45의 2026-10-21 Codex v5 격리 QA 댓글 확인.
- 지난 Codex v5: ZIP 실제 SHA 대조/CRC/Manifest/Seam/Unity 16:9·4:3·21:9/통행 proxy 12/12 기술 PASS. 그러나 기존 단일 타일 Earth·ThinFrost·Gravel 3종 주기 반복으로 **미술 HOLD**. Codex는 **OPEN16의 원본 CSV와 좌표JSON 로컬 회수**했지만 16행 상세는 아트 담당 세션에 미공유. v5로 OPEN16 해결 0.
- 바울 대표 2026-10-22 명시 발주: 기존 01 3장 그대로 보존, **02/03을 각 지형당 2장, 총 6장 추가**하는 v6. 기존 건물·지붕·SkillShop·StonePath·프로젝트 원본/F1/Wolf/Bear/Fox·Unity/Save 변경 금지.

## 원본/새 납품
- 소스 ZIP `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v5.zip` 실제 sha256 **`1affc1a4c23f3bcc6a9e9e18924b08bce3134eab065d8362757725f96e7f71b4`** 직접 확인.
- 최종 실제 ZIP `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v6.zip`
- **ZIP SHA256 `519ed96fa6157b7de451830412d6e881b633241b0f8cc459d186465765a4ea7e`**
- ZIP 크기 **17,658,178 bytes / 71 entries**, ZIP `testzip()==None`. `SHA256SUMS.txt`은 자기 자신 제외한 70항목을 모두 파일 내용의 SHA와 대조 PASS.
- PNG 게임용 **49장 = 기존 43장(바이트/SHA 모두 불변) + 새 6장**. ZIP `Tiles/`, `Buildings/`, `Buildings/Modular/`, `Transitions/`, `Props/`, `Signs/`, `Portal_Inactive/` 기존 상대경로 유지.
- 실제 바이트는 본 ChatGPT 대화 다운로드 첨부에서 제공, GitHub에는 이번 *문서와 SHA*만 기록. 바울 대표 F: 드라이브에 직접 복사하지 않았음.

## 신규 Sprite 6장

| 새 상대경로 | SHA256 | v5 01 대비 변경된 픽셀 | 설명 |
| --- | --- | ---: | --- |
| `Tiles/Frostway_Ground_Earth_02.png` | `05acefdc96d79e9e67a15a2d16f34513b748f4e7b2877e27c323d9fd54337180` | 20,395 (31.12%) | 풀·돌·흙얼룩 부분 배치 교환 02 |
| `Tiles/Frostway_Ground_Earth_03.png` | `a691ea9dfaf92193db2b3135d2e3d5e8017b9c1fbe5295cefb9f381e2f162fdd` | 18,973 (28.95%) | 돌·풀 군집을 02와 다른 배치로 교환 03 |
| `Tiles/Frostway_Ground_ThinFrost_02.png` | `4c877d3f5d0fb6557734b11e4ceeb6d651592c8ed723784aaecd8773876164f7` | 20,419 (31.16%) | 원본 흙/희미한 서리 색조 보존, 식생·자갈 분포 변경 |
| `Tiles/Frostway_Ground_ThinFrost_03.png` | `debc73f44e63f6b9c6ce7180e17f3f3394dfb37ac3983a4d242911a9261b52a8` | 18,969 (28.94%) | 별도 방향의 작은 자갈·서리 흔적 분포 변경 |
| `Tiles/Frostway_Ground_Gravel_02.png` | `421af3d388620c2fa15d1e95e0988714ca5617b4b2ebb272804f6cca30015686` | 18,347 (28.00%) | 큰 자갈·작은 돌 군집의 집중 위치 분산 |
| `Tiles/Frostway_Ground_Gravel_03.png` | `97010cbe07013ff63d7e236c21ab24798d6eb01664bcde10ea82fb424b10288a` | 18,037 (27.52%) | 이전 두 형태와 다른 돌무리/빈 공간 구성 |

- 6개 전부 **256×256 PNG RGBA / Alpha255(완전 불투명)**. 원본 `_01` PNG 3개는 byte-identical.
- 구현: **기존 원본의 특징적인 돌·식생·표면 픽셀을 비왜곡 국소 패치 교환**으로 새 위치에 재배치, 중심 픽셀 형태 및 주변 색을 그대로 유지하며 테두리만 작은 반경으로 부드럽게 연결. 새로운 무기/건물/눈지형 디자인 생성 아님. 원본에 없던 색상 팔레트·명도 변화 강요하지 않음.
- 가장 바깥쪽 **12px 원본 RGBA 완전 보존**, 덕분에 **동일 지형 01↔02↔03의 총 3×3 ordered neighbor 조합 x2축**을 전부 검사하여 horizontal/vertical max RGBA difference **0** 확인. 이는 **서로 다른 지형(Earth↔Gravel 등)의 전환**이나 실제 Unity Grid 자동 배치를 검증했다는 뜻이 아님.

## Offline QA 및 미술 게이트

- 3×3·5×5 각 지형 **01 단독 vs (01,02,03) 혼합** 비교 PNG 총 12장 `QA/Repeat/`, 변형9장 1:1 컨택트시트, 3지형 혼합5x5 컨택트시트, 지형별 5x5 전후 프리뷰 3장 `Preview/`.
- 5×5 혼합 배치는 01/02/03을 **상하좌우 동일 변형이 연접하지 않도록** 생성한 *오프라인 조립 예시*이며 실제 Scene TileGrid 배치 규칙이나 출시 타일 배치 확정은 아님. 비교 이미지에 단일 Base의 규칙성이 얼마나 잔존하는지 육안으로 볼 것.
- 새 6 PNG 디코딩·정확규격 PASS, 43 원본 PNG byte SHA 보존 PASS, Tile 01/02/03 같은 지형 18쌍 경계/3지형 모두 PASS, ZIP CRC 및 70-entry SHA 목록 PASS.
- **제한사항:** 각 변형이 같은 기존 베이스 텍스처의 픽셀을 재배치한 것이므로 공통 배경 질감과 원본 일부 큰 모티프는 남는다. 5x5에서 시각적 반복은 개선 후보일 뿐 전체 `ART_PASS` 선언 불가. **Codex 격리 Unity 변경분 QA와 대표 최종 미술 승인이 별도 필요**.
- 기존 `Frostway_Path_Stone_01.png`, `Frostway_SkillShop_Building.png`, `Frostway_Roof_Gable.png`, 기존 40/43 관련 자산 모두 byte SHA 동일. 새 타일6의 Unity SpriteImporter/PPU/Pivot/GUID/Tile selection/성능/실제 통행은 **미검증**. 본편 Unity Scene·main·Save/UserData 불변.

## OPEN16 행별 연계

- Source: 2026-10-21 GitHub Issue #45 최신 Codex 댓글은 **"OPEN16의 실제 원본 CSV·좌표 JSON을 Codex가 확보했으나 16항목 모두 v5에서 해결0"**이라고 명시.
- 실제 16행 원문(CSV/JSON)이 이 작업환경·GitHub 댓글에 노출되지 않아 **그 중 3지면의 결함과 직접 관련된 행이 무엇인지는 아직 확정 불가**. 무증거 분류 또는 결함 좌표 허위 생성 금지.
- v6의 신규 자산 6장은 **원본 PNG 43장 전부 유지**이므로 기존 OPEN16 항목을 직접 수정하지 않았다. **해결0, OPEN16 유지16**. Codex 원본 CSV·JSON 항목을 Issue #45 또는 문서로 사실 그대로 제공하면 신원/좌표 매핑 후 필요한 부분만 따로 수정한다.

## 다음

1. Codex 별도 격리 변경분 QA: ZIP SHA/49장 및 old43 byte keep, 6개 신규 alpha+PPU/pivot 적용, 01/02/03 tile 혼합 5×5 실제 Unity GameView 16:9·4:3·21:9 접합/Sorting/휠체어 proxy 검증. 기존 v5 통과 PlayMode 전체 재실행 금지.
2. 사용자 **미술 최종 승인** 전까지 마을 HOLD 유지, Main23 본편 적용/병합 진행하지 않음.
3. `docs/art-new` 브랜치 보고 커밋만 실행. `main` 직접 커밋/Push/병합 금지.
