# LIMITLESS / Field09 기존 공식 환경 PNG 8종 최소 보완 v1 납품 — 2026-10-19

- **담당:** Chapter1·2 리뉴얼 아트
- **작업 브랜치:** `docs/art-renewal` (**main 직접 커밋/Push/병합 없음**)
- **기준 main:** `675988c9f2c5c507d9a696b0378be27c0bd3309b`
- **발주:** 바울 대표가 지정한 기존 공식 8개 PNG의 반복 지면 경계 + 흰 가이드선 **실제 결함 부분만** 픽셀 최소 보완. 공식 콘셉트·실루엣·색상·오브젝트 역할 보존, 신규 재디자인/불필요한 리컬러 금지.
- **상태:** `DELTA_ART8_PRODUCED / OFFLINE_PNG_ROUNDTRIP_QA_PASS / USER_DELTA_VISUAL_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.
- **이번 보완 대상 밖:** `ObsidianScar_AshPatch_01.png` 및 `ObsidianScar_Ground_Cracked_02.png`는 **무변경**. Main18 Quest·과열·Monster·Save·Field08/09/10·기존 Scene/GUID/meta 전부 무변경.

## 실제 ZIP 납품 — GitHub 담당 브랜치 직접 다운로드

- **파일:** `Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip` (**담당 브랜치에 실제 업로드한 바이너리 ZIP**).
- **ZIP 크기:** **2,995,422 bytes / 총 13 entries**.
- **ZIP SHA256:** **`7dfa2417a8135e29e3e72c7d7921d3ade76fbedfdb376f764c09b02892296139`**
- **Git Blob SHA1:** `34bce32d0d91b9ef8167308383355590d3834f9f`, GitHub 등록 커밋 `434f506ff20b9b6cf99ab9937616a76803467ae0`.
- [바이너리 ZIP 직접 다운로드 (raw branch)](https://raw.githubusercontent.com/HappyPaul812/Project-Limitless/docs/art-renewal/Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip) / [GitHub 파일 상세](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-renewal/Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip).
- ZIP 구성: `Fixed_Candidates/` **실제 512×512 RGBA 수정 PNG 8개**, `Manifest.json` 원본·수정본 SHA256/Byte/변경 수치, `QA/Original_Source_SHA256_and_Delta.csv` 원본 Git blob·해시·변경 bbox, `QA/Offline_Validation.json`, `README.md`, `Codex_Integration_HOLD.md`. 원본 PNG **바이너리 자체는 ZIP에 중복 포장하지 않음**. 원본 8개는 기존 `main`의 `Unity/Client/Assets/_Project/Resources/Main18/Environment/`에 **바이트 그대로 보존**하고 ZIP에 각 **원본 sha256 + Git blob sha + path**를 제공하여 추적·검증 가능.
- ZIP은 ChatGPT 샌드박스 파일이 아니라 **GitHub 담당 브랜치에 직접 호스팅**하므로 사용자 PC `F:\Downloads`에 자동 저장됐다고 말하지 않는다.

## 변경 대상 8종 및 1차 파일 QA 수치

| 실제 PNG 이름 | 변경 형태 | 변경 픽셀 | 오프라인 검사 |
|---|---|---:|---|
| `ObsidianScar_Ground_Base.png` | 좌우·상하 타일 반복 외곽 **12px 폭** 최소 결합(중심부 byte 동일) | **21,919** | RGBA 4채널 외곽 반복 mismatch 0 |
| `ObsidianScar_Ground_Cracked_01.png` | 위와 동일, 기존 음영·암석 색상 중심부 보존 | **22,859** | RGBA 4채널 외곽 반복 mismatch 0 |
| `ObsidianScar_Cliff_Blocker_01.png` | 상·하·좌·우 외곽에서 밝은 무채색 반투명 가이드선 픽셀만 Alpha0 | **2,363** | 512 RGBA decode/roundtrip PASS |
| `ObsidianScar_Fissure_Edge.png` | 가이드선 후보만 삭제 | **895** | decode/roundtrip PASS |
| `ObsidianScar_Fissure_Glow.png` | 가이드선 후보만 삭제 | **2,785** | decode/roundtrip PASS |
| `ObsidianScar_HeatVent_01.png` | 가이드선 후보만 삭제 | **873** | decode/roundtrip PASS |
| `ObsidianScar_Obsidian_Slab_01.png` | 가이드선 후보만 삭제 | **2,030** | decode/roundtrip PASS |
| `ObsidianScar_Obsidian_Spire_01.png` | 가이드선 후보만 삭제 | **1,169** | decode/roundtrip PASS |

보완 방식:
- 지면은 기존 `512x512 RGBA`를 확대/교체하지 않고 좌·우, 상·하 **각 최대12px 경계 띠**만 색/Alpha를 부드럽게 연결. 최외곽 1px의 좌우·상하 채널 완전 일치. **기존 Ground Tiled 21×15와 CrackedWest Tiled 9×15 사이의 이종 Material 오버레이 경계까지 자동 PASS했다고 주장하지 않는다**.
- 안내선은 원본에서 관찰된 투명 배경 둘레의 백·회색 가이드 요소 중 **밝은 중립색·낮은 Alpha를 가진 가이드 계열만** 선택하여 완전 투명으로 변경, 기존 rock/heat/fissure 전체 색감은 그대로 둔다. 실제 Unity 화면에서 Art 경계와 겹치는 원화 픽셀의 시인성·잔여선을 반드시 사용자/ Codex 검수.
- 각 PNG original source를 GitHub `main`에서 Base64로 읽고 디코딩·프레임 없이 RGBA 픽셀 편집. 수정 PNG를 zlib DEFLATE PNG로 재인코딩하여 **다시 PNG 자체를 decode한 결과가 수정한 원본 픽셀과 전부 일치**함을 확인. 모든 PNG 크기 512×512, 색상/알파 채널 RGBA, Filter Point·Pivot Center·PPU128·MipmapOFF **기존 Importer 유지 권고**.
- Manifest SHA256은 각 PNG 실제 byte에서 산출. GitHub ZIP의 업로드 후 응답 파일 크기 **2,995,422 bytes / Git Blob SHA** 일치 확인. ZIP Entry마다 유효 CRC32/local/central header 포함. 별도 Unity Play·실제 Importer 회귀/화면 캡처·접지 QA는 **실행하지 않았음**.
- **주의:** '가이드선 후보 픽셀 자동 제거'는 픽셀 검사 PASS이지 미술 최종 승인/게임 화면 시인성 PASS가 아니다. 임의 화면 확대나 사이즈 리터치 금지.

## Codex 통합 보류와 승인 순서

1. 바울 대표가 ZIP `Fixed_Candidates/` 8개의 최소 보완 상태를 **미술 검토·승인 또는 국소 수정 지시**.
2. 승인 후 Codex `codex/*` 별도 작업공간에서 **`QA/Original_Source_SHA256_and_Delta.csv`의 소스 해시와 현재 `main` Asset Hash 선대조**. 원본의 기존 GUID/meta·Project sprite association/PPU128 Pivot Point를 **보존**하여 지정된 8 PNG만 승인된 방식으로 기술 교체.
3. 실제 Unity RenderTexture **1280×720/1600×900/1920×1080**, Ground Tiled-Overlay 시각 접합, 가이드 제거 흔적, NPC/Monster·HeatVent·Cliff Collider·Main18 과열·저장/씬 전환을 확인. 원본 Textures/Scene/Layout/Save 미커밋 변경은 보호.
4. **다른 공식2종 AshPatch / Cracked02** 및 UI/VFX/Monster/Field08 기존 아트 재제작0. main 직접 Push/Commit·Unity 정식 적용은 이 아트 납품으로 승인된 것이 아니며 Codex/대표 결정 별도.

**Git:** `docs/art-renewal`에 바이너리 후보 ZIP 및 이 문서/최신 인계만 커밋.
