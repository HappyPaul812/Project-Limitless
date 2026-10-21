# LIMITLESS / Chapter3 Main23 Frostway Village — 타일 반복무늬 국소 완화 v5 납품 (2026-10-21)

> **OFFLINE_TECH_QA_PASS / TILE_ART_REVIEW_PENDING / OPEN16_SOURCE_CSV_PENDING / CODEX_DELTA_QA_PENDING / USER_FINAL_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED**

브랜치: `docs/art-new` · GitHub Issue [#45](https://github.com/HappyPaul812/Project-Limitless/issues/45). 우선 인계 `팀운영/인계/SPRITE_ART_LATEST.md` 및 `PLANNING_LATEST.md`(docs/planning), Issue #45 최신 격리 Unity v4 QA를 읽고 수행.

## 기준 실물과 범위

- 소스 v4: `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v4.zip` SHA256 `ee25baef91ec740b85dd3bb62b32583292d703f9261863bd534d0eb8241d1896` — 이 작업 환경 내 파일을 실제 읽어 SHA 확인. 대표 Windows `F:\Downloads\` 파일에 직접 접근하지 않았음.
- 기준: 실사용 게임용 PNG43종/기존 SkillShop과 Roof Gable v4 표적 PASS, 격리 Unity 16:9·4:3·21:9 조립/Sorting 기술 PASS, **마을 전체 미술 HOLD**.
- 기획 최신: 흙·얇은 서리 반복무늬 개선 권고; 자갈 수용 여부 별도 판정, 돌길 반복 사용 가능 의견. 사용자 직접 이번 4종 검토 지시를 반영하여 **돌길은 KEEP** 처리하고 나머지 3종만 편집함.
- **Wolf Run6 v5는 사용자 최종 승인**, 이 사실을 마을 승인에 적용하지 않음. F1 승인 환경, 늑대, 기타 Chapter3/Chapter1/2 자산, Unity Scene, Save/UserData/GUID, original 43 PNG 구조 보존.

## 실제 ZIP 파일

- 납품 파일명: `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v5.zip`.
- **최종 ZIP SHA256 `1affc1a4c23f3bcc6a9e9e18924b08bce3134eab065d8362757725f96e7f71b4`**, 크기 **20,085,423 bytes**, **72 entries**, CRC `testzip()==None` PASS, `SHA256SUMS.txt` 항목 **71/71** 해시 대조 PASS.
- 실제 게임 PNG **43종 가운데 수정3 / 원본 동일40** (소스 ZIP 엔트리 바이트와 40/40 직접 비교). 40종은 PNG 재저장/최적화 없이 원본 ZIP의 bytes 그대로.
- 실물 ZIP/PNG는 **이번 ChatGPT 대화 다운로드 파일**. **GitHub에는 보고/인계 문서만**, 바이너리 자동 업로드 아님.

## 수정 3종과 미술 의도

| 파일 | 변경 픽셀 | 변경 비율 | 수정 이유 |
| --- | ---: | ---: | --- |
| `Tiles/Frostway_Ground_Earth_01.png` | 24,062 | 36.72% | 하단 큰 풀·돌 군집과 오른쪽 단일 바위 무늬의 강조를 낮추고 기존 흙 기반 미세 질감을 유지 |
| `Tiles/Frostway_Ground_ThinFrost_01.png` | 23,973 | 36.58% | 같은 반복 식생·돌 모티프의 시각적 반복 완화, 얇은 서리/흙 우세 유지 |
| `Tiles/Frostway_Ground_Gravel_01.png` | 31,120 | 47.49% | 두드러진 특정 자갈·돌 뭉침 위치의 반복감 감소, 자갈의 회갈색 밀도와 재료감 유지 |

- 편집 방식은 동일 원본 타일 내 주변 소재를 참고한 **국소 텍스처 재배치/완만한 결합**. 256×256 RGBA, alpha=255, 바깥 5픽셀 폭은 기존 RGBA와 같고 네 방향 맨자리의 에지 픽셀 배열은 서로 정확히 일치. 외곽 4픽셀과 원본 tile edge 및 원본 색상 범위를 보존했으며 다른 바이옴/지형을 새로 디자인하지 않음.
- `Tiles/Frostway_Path_Stone_01.png` **KEEP/SHA 그대로**. 포석 반복성은 남으나 Codex의 '반복 사용 가능' 의견을 존중해 이번에는 손대지 않음.
- 건물/문/간판/소품/비활성 포탈/Transition 등 전체 **40종** 바이트 보존. **SkillShop과 Roof Gable 기존 v4 2종 미술 PASS 그대로**.

## QA 실물 구성

- 43 PNG와 `Manifest.md`, `Manifest.json`, `SHA256SUMS.txt`, `QA/Offline_QA.json`, `QA/OPEN16_Evidence_Pending.md`.
- Tile4 각각 v4/v5 **2×2·3×3(총16 PNG)** 반복, `QA/BeforeAfter/`의 v4/v5 3×3 나란히 판 **4 PNG**, `Preview/Frostway_Tile4_BeforeAfter_1x_OFFLINE.png`.
- 실제 v5 개별 PNG를 배치한 **새로운** `Preview/Frostway_Village_v5_Layout_OFFLINE_16x9.png`(1600×900), `_4x3.png`(1200×900), `_21x9.png`(2100×900). 각각 실제 화면비를 준수하며 **100% 오프라인 조합**. `Unity GameView`나 실문/휠체어 이동/Sorting 재검증 결과가 아님.
- **PASS 범위**: 모든 게임 PNG 디코딩, RGBA, 40 KEEP SHA, 세 대상 타일 RGBA 보존/에지 동일, 모든 Tile X/Y 가장자리 일치, 2×2/3×3 반복 이미지 생성, ZIP CRC 및 71개 엔트리 SHA.
- **미술 한계**: **단 한 종류의 Base 타일 반복을 사용하는 이상 3×3 반복 무늬의 규칙성이 사라질 수 없음**. 큰 특정 모티프의 강도를 낮춘 시안일 뿐 반복무늬 '미술 PASS' 자동 판정하지 않는다. 추후 필요 시 새 변형 타일 도입은 별도 사용자 승인 및 Codex TileGrid 계약 필요.

## OPEN 16건 — 근거 확보 전 항목별 수정 금지

- `PLANNING_LATEST.md` 최신 기획과 Issue #45 댓글에 **이전 Codex QA CSV 기반 OPEN 16건이 남았다**는 집계는 명확하게 기록됨.
- 그러나 **16개 실제 행의 원본 Codex CSV(파일명·좌표·관찰 조건)는 v4 ZIP 70엔트리 안에 없고, 검토 가능한 Github issue/담당 문서와 사용자 Library 검색에서도 찾지 못했다**. 소스가 없는 16개의 이름/위치/해결 상황을 추측하면 기존 PASS 회귀가 발생한다.
- 그래서 이번 v5에서는 **16 OPEN의 개별 수정 0건, 검증/종결 0건**으로 기록, 현재 **OPEN 16 / USER_SOURCE_CSV_REQUIRED**. 이 숫자는 자체 새 QA에서 재발견한 16개를 뜻하지 않음. QA CSV 실물이 Codex 로컬 원본에 있다면 소스 증거 확보 후 파일별 범위를 좁혀 추가 v5.x 수정 필요.
- 거짓 'OPEN16 해결' 보고 없이 ZIP 안에 `QA/OPEN16_Evidence_Pending.md` 제공.

## 다음 검수 및 Gate

1. Codex는 소스 ZIP 바이트 SHA → 수정3 PNG만 import 및 3×3 반복 체감/ThinFrost F1 연결/동선 UI 읽기 여부 **격리 Delta QA**, 기존 v4 PASS 본체/40KEEP SHA 유지 확인. 기존 전체 Main22/23 QA 재실행 금지.
2. 이전 OPEN 16 CSV 파일을 Codex가 실제로 확보해 File/XY/Severity/OPEN 상태·기존 v4에서 해결된 항목과 중복 여부를 대조하고 정확한 대상만 art-new에 인계.
3. 대표 **타일 미술 승인 및 OPEN16 범위 결정 별도**, 이후에만 Codex 본게임 Main23 반영. 사용자 미술 승인 전 완전 GameReady/Unity 적용 완료 주장 금지.
4. `docs/art-new`에만 이 문서 기록; **main 직접 Commit·Push·Merge하지 않음**.
