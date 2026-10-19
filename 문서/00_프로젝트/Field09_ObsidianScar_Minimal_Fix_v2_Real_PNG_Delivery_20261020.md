# LIMITLESS / Field09 ObsidianScar 공식 아트 최소 보완 v2 — 실제 바이너리 ZIP 납품 (2026-10-20)

- **범위 승인:** GitHub [Issue #29](https://github.com/HappyPaul812/Project-Limitless/issues/29), 기획 `docs/planning/문서/00_프로젝트/Field09_OfficialPNG_SourceConfirmed_ArtV2_ExecutionOrder_20261020.md`.
- **담당 브랜치** `docs/art-renewal`, **기준 main** `12c69c72f9c09f716ee6242418b1d62f83a4a0d2`.
- **상태** `ACTUAL_V2_8PNG_DELIVERED / OFFLINE_PNG_ROUNDTRIP_PASS / ZIP_BINARY_UPLOADED / USER_ART_REVIEW_PENDING / UNITY_VISUAL_QA_PENDING / OFFICIAL_UNITY_NOT_APPLIED`.
- 이 보고는 v1의 단순 ZIP 재포장이 **아니며 실제 원본 PNG8 각 RGBA 픽셀을 새로 수정**한 후보의 납품입니다. 공식 원본10·모든 .meta/GUID·Unity Scene·Save·Quest·그외 자산 **변경0**.

## 실제 업로드된 v2 ZIP
- GitHub URL: https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v2.zip
- 경로: `Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v2.zip`
- **2,659,837 bytes**, Git Blob SHA1 **`380cc2c0f588f0a6d562e8250cfe223bf229cd9c`**, ZIP 자체 SHA256 **`30f102c223aa127a7b6a290ef9fa503354cb9e0104faac03e6c395c2b247b8b8`**(순수 JS SHA256 계산).
- 실제 납품 커밋 **`4babb98b1c90bbd001dda1cff4be46097d02cfc7`**.
- GitHub REST에서 업로드 후 **실제 파일명/크기/Blob SHA 재조회 일치**. 본 세션 GitHub 바이너리 fetch API는 크기가 큰 ZIP 본문을 비워 반환하므로 업로드 후 Windows Explorer / Python `zipfile.testzip()` **독립 재다운로드 검사는 수행 못했음**. ZIP 작성 자체는 표준 ZIP_STORED **14개 엔트리**, local/central directory/EOCD, 각 내용 CRC32 생성; Codex가 수신 파일을 독립 3도구로 한 번 검증할 것. `PYTHON_TESTZIP_PASS` 주장 금지.

## 실제 변경 내용
| 파일명 | 변경 픽셀 수(전체 512×512 RGBA) | 변경 범위 bbox (x0,y0,x1,y1) | 비고 |
|---|---:|---|---|
| `ObsidianScar_Ground_Base.png` | 135085 | 0,0,511,511 | 외곽 88px 양면 보정, 반복 최외곽 RGBA mismatch0 |
| `ObsidianScar_Ground_Cracked_01.png` | 139085 | 0,0,511,511 | 위와 동일, 실 게임 이종 오버레이 경계는 별도 QA |
| `ObsidianScar_Cliff_Blocker_01.png` | 3120 | 18,53,495,457 | 가이드 외곽 중성 명부 픽셀 제거 |
| `ObsidianScar_Fissure_Edge.png` | 2457 | 16,31,493,480 | 위와 동일 |
| `ObsidianScar_Fissure_Glow.png` | 3035 | 16,36,495,475 | 위와 동일 |
| `ObsidianScar_HeatVent_01.png` | 2258 | 16,22,495,488 | 위와 동일 |
| `ObsidianScar_Obsidian_Slab_01.png` | 2297 | 16,53,493,458 | 위와 동일 |
| `ObsidianScar_Obsidian_Spire_01.png` | 2905 | 19,21,495,489 | 위와 동일 |

**중요:** 위 수치는 원본에서 이미지 데이터를 직접 읽어 비교한 오프라인 변경 픽셀 수입니다. Ground2는 단순 12px/반복 끝 한 줄만 맞춘 v1보다 외곽 보정을 **88px로 확대**하여 실제 무늬 연결을 조정했습니다. 다만 `GroundBase` / `GroundCrackedWest`의 서로 다른 재질 중첩과 반복띠6개가 **실제 Unity 화면에서 0인지 확인한 결과는 아님**. Guide6도 대상 가이드선 근방의 밝은 무채색 픽셀을 제외했으며 미술상의 손실·잔여선0 판정은 사람/Runtime QA 대기.

## ZIP 구성 / 검사
- `Fixed_Candidates/` 수정 RGBA512×512 PNG8 (새 파일, 원본 Unity 경로를 건드리지 않음)
- `Manifest.json`: 각 PNG **GitHub 원본 Git Blob SHA**, 원본 SHA256, v2 SHA256, CRC32, 바이트, 변경 BBox와 이음 검사.
- `Changed_Pixels.csv`: 기존/새 파일 SHA256, 실제 변경 픽셀 개수·좌표 경계.
- `VisualFail_Cases.csv`: 반복지면/이종 Material/Guide6 이전 실패 게이트와 현재 확인 상태.
- `Preview/Before_After_8.png`: **원본과 수정 후보의 실제 RGBA 픽셀을 1/4 크기로 나란히 구성한 오프라인 비교**, 실 Unity 캡처 아님. 각 행 왼쪽 original / 오른쪽 v2.
- `README.md`, `QA/Offline_Validation.json`.

실행된 오프라인 검사: 원본8 **PNG CRC + Zlib decode**, 수정8 다시 **PNG CRC + Zlib decode**, 수정 후보 픽셀 **RGB/RGBA 원본 수정 결과와 roundtrip8/8일치**, PNG Manifest SHA·바이트 계측, Ground2 각각 최외곽 반복 4채널 mismatch **0**. ZIP byte 자체 생성+파일별 CRC 삽입, GitHub 업로드 후 SHA1/크기 확인. **ZIP 파일을 Python에서 재열람하여 `testzip()==None` 확인한 것이 아님**.

## Codex 후속 계약 / 출시 게이트
1. 이 ZIP을 **별도 격리 작업공간**에서 다운로드하고 SHA256/CRC·14 entries·Manifest PNG8 확인 (Python `zipfile.testzip()`, .NET, Explorer 3종).
2. 원본 main PNG8 해시와 Manifest SHA256 일치 선검증, 기존 메타/GUID/Asset 원본 보존.
3. 기존 Field09 v1 **실제 실패 캡처/좌표**와 v2를 같은 RenderTexture/Scale/Camera로 비교: **회색 반복띠6 위치 / GroundBase↔CrackedWest 이종 오버레이**, 안내선6종, Cliff 경계/Collider, Main18 Heat/Quest/Save, 경계 이동. 실제 안내선 0 / 지면6개 띠0 / 손실0이어야 미술 PASS. 실패 시 건별 bbox·스크린샷 회신.
4. 바울 대표 미술 승인 + Codex 실제 Unity 검수 PASS 후에만 원본 적용/PR/main 병합을 *별도 명시 승인*으로 결정. 과거40/92 및500 PASS 전체 불필요 재수행 금지.

**보호:** `ObsidianScar_AshPatch_01.png`, `ObsidianScar_Ground_Cracked_02.png`, 다른 승인8 공식 원본/.meta/타 Field·Quest/Save·TTS·몬스터, 모든 사용자 로컬 수정분 변경0. **main 직접 Commit/Push/병합0.**
