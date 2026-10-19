# LIMITLESS / Field05 동서 Road v2 — 4종 아트 제작·기술 인계 (2026-10-19)

**담당:** Chapter1·2 리뉴얼 아트 / **브랜치:** `docs/art-renewal` / **기준 `main`:** `c263d9e36fc0570b16130d9c011abf3965def5f7`  
**기획 기준:** `docs/planning` 커밋 `b614a5c6d6a1e51316c0ed22e03cc34b517982c5`, `Chapter2_Field05_EastWestRoad_VisualCorrection_20261019.md`. 사용자 전달 Codex 임시 Z=-90 실제 RenderTexture 비교/A30·B42 기술 명세(2026-10-19)를 추가 근거로 사용했으나, 이 담당이 Codex 로컬 `F:` 경로의 검증 파일이나 임시 RenderTexture 자체를 직접 열었다고 주장하지 않습니다.

**현재 상태:** `ART_DELIVERED / OFFLINE_SEAM_QA_PASS / A_B_UNDECIDED / USER_FINAL_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.

## 1. 기술 검토 및 최소 미술 변경

- v1 Scene 환경 SpriteRenderer **363개 = Ground315 + Road30 + Props18**. 기존 Road30은 남북 15행×2열이므로 v1 PNG만 교체해도 잘못된 남북 진행 인상이 유지됨.
- Codex는 격리 Unity 6000.6.5f1에서 A 동서15열(30개, x=-4..10/y±0.5), B 동서21열(42개, x=-10..10/y±0.5) 임시 회전 비교를 완료했다고 전달. **A/B 최종 선택·정식 적용은 미승인**.
- 기존 회전 비교의 외곽 불투명 사각띠/서쪽 끝 수직 절단을 고치기 위해, 기획 요구에 따라 정확히 **4종 새 Road PNG**만 제작. v1 황토/올리브 계열 소재를 직접 참조하면서 **PNG 자체에 동서 타이어 흔적·자갈·마모와 불규칙 투명 경계**를 그려, 최종 Inspector에서는 Z=0, Flip X/Y false를 권장.
- 도로 북·남 외곽 픽셀 Alpha0(사각 끝띠 없음), Road 직선 반복 간/직선 Upper↔Lower/서쪽 End→직선 연결부 **RGBA 4채널 경계 불일치0**, 서쪽 End x=0 Alpha0인 흙길 소멸 형태. 새 Portal/방향 화살표/출입구 그림 없음.

## 2. 실제 납품 파일 및 SHA256

- ChatGPT 직접 생성 다운로드: **`Limitless_Field05_EastWest_Road_v2.zip`**. **589,850 bytes**, **SHA256 `3a0d3b91cd87a5362ebbe435c1e5acd06ffc74354eafaa41f02b4f181d27c3d3`**. 사용자 PC `F:` 폴더에 직접 저장했다고 주장하지 않음.
- ZIP **14파일 / PNG8개** = 새로운 실사용 Road4 + 오프라인 Preview4. `Manifest.json`, `README.md`, `Road_Placement_A_B.csv`, `QA/Seam_Alpha_Validation_v2.json`, `Codex_Integration_Handoff.md`, `Rights_and_Provenance.md` 포함.
- Road 실제 PNG4:
  1. `Road/LL_C2_F05_Road_EW_Upper_128_v2.png`
  2. `Road/LL_C2_F05_Road_EW_Lower_128_v2.png`
  3. `Road/LL_C2_F05_Road_EW_WestEnd_Upper_128_v2.png`
  4. `Road/LL_C2_F05_Road_EW_WestEnd_Lower_128_v2.png`
- 4 PNG 각각 **128×128 RGBA**, **PPU128 / Pivot Center(0.5,0.5) / Point / mipmap OFF**, `Manifest.json`에 PNG별 **SHA256/Alpha bbox/Import 설정/크기** 수록. 새 GUI/Unity .meta/GUID를 공급한 적 없음.
- v1 ZIP `Limitless_Field05_Renewal_Art_v1.zip`을 직접 읽어 **SHA256 `a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258`** 일치 확인 후 별도 보존. 기존 Ground3 / Props7 / Optional7 / 그외 v1 Road2까지 원본 ZIP·원본 PNG 전체 변경0. 새 ZIP에는 변경 대상 Road4와 Preview/문서만 넣은 델타 납품. 기존 Field04 승인v2/Arbel 승인v3 자산 재제작0.

## 3. QA 수치 (오프라인 파일 검수 전용)

**검증 파일:** `QA/Seam_Alpha_Validation_v2.json`

| 검사 | 불일치 픽셀 |
|---|---:|
| 직선 Upper 좌우 반복 RGBA | 0 |
| 직선 Lower 좌우 반복 RGBA | 0 |
| 직선 Upper 아래 ↔ Lower 위 RGBA | 0 |
| WestEnd Upper 오른쪽 ↔ 직선 Upper 왼쪽 RGBA | 0 |
| WestEnd Lower 오른쪽 ↔ 직선 Lower 왼쪽 RGBA | 0 |
| WestEnd Upper 아래 ↔ Lower 위 RGBA | 0 |
| Upper 북쪽 최외곽 불투명/비투명 픽셀 | 0 |
| Lower 남쪽 최외곽 불투명/비투명 픽셀 | 0 |
| WestEnd 서쪽 외곽 불투명/비투명 픽셀 | 0 |

- ZIP CRC `testzip()==None` PASS; PNG4 decode·Mode RGBA·128×128, Manifest PNG4 SHA PASS; 추가 Preview4 생성.
- `Preview/LL_C2_F05_Road_EW_BeforeAfter_v2.png`: v1 Road를 도식적으로 90도 회전한 임시 시안(위) 대 새 v2 4타일 합성(아래), 실제 Unity RenderTexture가 **아님**.
- `Preview/LL_C2_F05_Road_EW_ContactSheet_v2.png`, `Preview/LL_C2_F05_Road_EW_AlphaChecker_v2.png`, `Preview/LL_C2_F05_Road_EW_A_B_Schematic_v2.png`: 새로운 Road의 alpha/연결, A/B 배치 개념. 실제 동작·Collider/세린 가독성·휠체어/Quest/Save QA 완료로 주장하지 않음.

## 4. A / B 배치안 및 보호 대상

- **A**: 기존 30 Renderer를 15열×상하2행, x=-4..10, y=+0.5/-0.5 재배치. 가장 서쪽 x=-4 두 Renderer는 WestEnd 2종; 나머지 직선. 추가 Renderer0.
- **B**: 42 Renderer, 21열×상하2행, x=-10..10, y=+0.5/-0.5. 가장 서쪽 x=-10에 WestEnd, 나머지 직선. **기존 대비 12 Renderer를 추가하는 배치**이므로 별도 구조 승인 없이는 불가.
- `Road_Placement_A_B.csv`에 **A30 + B42 = 총 72행** 완전 배치 후보. 둘 다 예시일 뿐, **두 가지를 동시에 적용하지 않음**. 서쪽 B 끝이 맵 가장자리와 만난다고 새 Portal/Field06 출구가 생기는 것은 아님.
- 그대로 보호: 수로(5,2), 초소(-2,2), 진동(-6,-2), 세린(1,0), 그을음들개(3,-3), 균열도마뱀(-4,3), 활동반경1/리스폰35초, 동쪽 Arbel Spawn(8,0)/Exit(10.7,0), 기존 315 Ground/18 Props, Monster/NPC/Quest/Collider/Save/Camera/휠체어/기존 Sprite 복구 자산. 선택 소품7 **배치0**, 지선 Decal **추가0**. Field03 Rock B 미술 승인 문제 별도.
- **다음 승인 게이트**: (1) 사용자 미술: 4 PNG의 경계·색상/무드 (2) 사용자 A30 vs B42 배치 최종 선택 (3) Codex 격리 Unity에서 승인안 Import(새 GUID)·Road Transform·실제 RenderTexture 및 접근성/Quest14/몬스터/Save·Arbel 귀환 회귀 (4) 원본 적용은 별도 사용자 승인 후.
- **Git 정책:** 이 문서는 `docs/art-renewal`만 기록. Git 대용량 ZIP 추가 없음, `main` Commit/Push/병합/force 없음.
