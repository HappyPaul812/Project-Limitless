# LIMITLESS / 2026-10-20 Field09 공식 PNG GitHub 원본 확인 및 미술 v2 실제 제작 실행 지시

- 담당: 기획 `docs/planning`; 실제 미술 제작: `docs/art-renewal`; 원본 파일 열람이 기술적으로 막힌 때의 1회 읽기 전용 로컬 자산 추출만 Codex.
- 바울 대표 최신 지시: **“Field09 미술 원본은 깃허브에 없으면 없는거니까 알아서 지시해줘.”**
- 상태: `GITHUB_OFFICIAL_SOURCE_10_PNG_CONFIRMED / FIELD09_ART_V1_VISUAL_FAIL / V2_8PNG_PRODUCTION_ORDERED / V2_NOT_DELIVERED / OFFICIAL_UNITY_APPLY_BLOCKED`.
- 이 문서는 미술 자산 자체를 생성/수정하거나 Unity Editor/Windows F:드라이브를 조작하지 않았다. 완료보고에 PNG8+정상 ZIP+픽셀 변경 자료 없으면 **ART_NOT_DELIVERED**.

## 1. 더 이상 "원본이 없다"고 보류하지 말 것 — GitHub 소스 확인 완료

2026-10-20 GitHub `main` HEAD `12c69c72f9c09f716ee6242418b1d62f83a4a0d2`. REST contents에서 **원본 공식 PNG10개와 동일 경로의 .meta10개가 실재**함을 확인.

- 공식 경로: `Unity/Client/Assets/_Project/Resources/Main18/Environment/`
- GitHub 폴더: `https://github.com/HappyPaul812/Project-Limitless/tree/main/Unity/Client/Assets/_Project/Resources/Main18/Environment`
- 파일 10종: `ObsidianScar_Ground_Base.png`, `ObsidianScar_Ground_Cracked_01.png`, `ObsidianScar_Ground_Cracked_02.png`, `ObsidianScar_AshPatch_01.png`, `ObsidianScar_Cliff_Blocker_01.png`, `ObsidianScar_Fissure_Edge.png`, `ObsidianScar_Fissure_Glow.png`, `ObsidianScar_HeatVent_01.png`, `ObsidianScar_Obsidian_Slab_01.png`, `ObsidianScar_Obsidian_Spire_01.png`.
- 확인된 **8개 보완 대상 PNG**(크기 bytes, GitHub 확인): Base 409914, Cracked01 453448, Cliff 290618, FissureEdge 279709, FissureGlow 128508, HeatVent 209716, Slab 164657, Spire 232977. 각 `.png.meta`도 모두 main에 존재. 원본 파일의 Git Blob SHA는 바이트 SHA256과 다른 해시이므로 섞어 쓰지 말고 다운로드 후 PNG 자체 SHA256 측정.
- 이전 `docs/art-renewal/Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip`는 손상 ZIP 헤더로 최초 불량이었고, Codex가 정상 재포장한 후보 역시 **실제 Unity 시각 FAIL: 회색 지면 반복띠6 + 소품 흰 가이드선6**. 이 파일을 'v2 미술 해결본'으로 이름만 바꿔 재납품 금지.

### 파일 입수 실패의 대응원칙

1. 리뉴얼 담당이 **GitHub main의 위 경로 PNG 바이너리**를 GitHub 웹 다운로드/커넥터가 허용하는 실제 바이너리 취득 경로/로컬 git clone 등으로 스스로 내려받고 열어 픽셀을 확인한다. GitHub '메타데이터만 읽힘'과 'GitHub에 원본 없음'은 다른 상태다.
2. 현재 리뉴얼 세션 컨테이너만 바이너리 다운로드를 못 하는 **도구 제약**이라면, Codex에게 아래 범위의 **단 1회 읽기 전용 최소 자산 추출**을 맡겨 담당자가 접근 가능한 채팅/문서 첨부 또는 별도 안전 GitHub 아티팩트/브랜치에 전달. 사용자에게 F드라이브 업로드/스크린샷 반복을 요청하지 않는다.
   - 정확히 공식 PNG8 + 필요한 `.meta` + 정상 재포장 v1 후보8 + 이전 Field09 Unity 실제 VisualFail 캡처·좌표 + SHA/Manifest 매핑만.
   - 불필요한 16GB Unity 프로젝트/Library/Backup/Save/UserData/다른 음성/씬 전체 ZIP 수집 금지. 해당 **공식 이미지 파일 재등록은 기존 `main` 파일의 복사본**이지 새 미술 제작이 아님.
   - 파일이 어디에도 없다고 단정하는 대신 `GITHUB_EXISTS / TOOL_BINARY_READ_BLOCKED / CODEX_SOURCE_EXTRACT_REQUESTED`로 보고. Codex가 다른 작업 중이면 art 담당은 다운로드 가능한 공식 이미지·기존 납품/원본 분석 작업을 계속 진행.
3. 실제 GitHub main에 특정 원본이 **목록 조회와 blob 검증에서도 없고** 백업도 확보할 수 없는 경우에만 `SOURCE_ABSENT_CONFIRMED`로 보고하고 원본 복제 불가능한 해당 항목을 **신규 오리지널 후보**로 제작·낙관적 '국소 픽셀 수정'이라고 주장하지 않는다. **현재 Field09에는 해당 경우가 아님: 공식10종 존재 확인.**

## 2. 리뉴얼 담당의 오늘 최우선 실제 미술 납품

**범위 고정: PNG 최대8**. 공식 Original10과 .meta/GUID byte는 절대 덮어쓰지 않는다. 기존 스타일·실루엣·원래 과열 표식·색상은 보호.

- Ground2: `ObsidianScar_Ground_Base.png`, `ObsidianScar_Ground_Cracked_01.png` — 실제 Unity Tiled 반복 6개 회색 띠와 **GroundBase↔CrackedWest 이종 Overlay**까지 비교한 뒤 근본적인 반복의 색/불투명-알파/경계 문제를 수정. 단순 주변12px 자동 편집 후 `edge seam0`만 체크하는 것은 합격 근거 아님. 코드/Tile Renderer의 Tint/UV/Tiled Mode/Sorting/광원/Overlay 결함이라면 PNG만 억지 편집해 덮지 말고 Art 코드작업 경계와 정확한 좌표·샘플로 Codex에 기술 결함이라고 분리 보고.
- Guide6: `ObsidianScar_Cliff_Blocker_01.png`, `ObsidianScar_Fissure_Edge.png`, `ObsidianScar_Fissure_Glow.png`, `ObsidianScar_HeatVent_01.png`, `ObsidianScar_Obsidian_Slab_01.png`, `ObsidianScar_Obsidian_Spire_01.png` — 화면에 실제 보이는 **흰 임시 안내선**을 원본 PNG 알파/색/배경·Material과 맞대조하여 해당 픽셀만 국소 보완. 의도된 밝은 Glow/암석 내부 윤곽은 살리고, 임의 '희고 낮은 alpha' 픽셀을 무조건 지워 망가뜨리지 않는다.
- 보호: `ObsidianScar_AshPatch_01.png`, `ObsidianScar_Ground_Cracked_02.png`, Field08 공식 PNG9, Main18 퀘스트13/과열/세린/몬스터/Collider/Save/Main09·10 음성, 사용자 PC 원본 변경0.

**필수 납품:** `Limitless_Field09_ObsidianScar_Minimal_Fix_v2.zip` — 실제 수정 PNG8(불필요 수정이 있는 경우 실제 고친 파일만 포함하며 누락 사유 명시), `Manifest.json` 각 PNG 원본/후보 SHA256·Importer 권장, `Changed_Pixels.csv` 실제 수정 좌표/개수, `VisualFail_Cases.csv` 실제 지면6·Guide6 사례별 개선 근거, `README.md`, 원본 출처, 전후 확대 이미지를 포함.
- ZIP writer는 **표준 Python zipfile** 등으로 작성, 새 ZIP와 각 PNG 직접 Decode/CRC/`testzip()`/Manifest SHA와 파일명 대조한 로그 필수. 이전 헤더 바이트 순서 오류 절대 재발 금지.
- 실제 Unity Before/After 장면이 없으면 **OFFLINE_ART_QA_PASS / UNITY_VISUAL_QA_PENDING**으로만 보고. Preview 합성은 실제 Unity 캡처처럼 표시 불가. Codex에게 실제 Unity 비교 QA만 후속 요청하고 검사 대상 파일을 명확히 인계.
- 단순 보고서/분석표/고해상도 독사·늑대 이미지만 납품해 Field09를 완료 처리하지 말 것. **이번 Field09 지시의 완료 조건은 실사용 가능한 국소 수정 PNG와 정상 ZIP**이다.

## 3. 역할·검증·Git

- 리뉴얼 담당: 위 8개 PNG 미술 수리→ZIP 제작→오프라인 QA→`docs/art-renewal`에 산출물 경로/ZIP SHA·수정 항목/소스 해시/검수 보류사항 기록. 리뉴얼 담당은 기존 몬스터·늑대 v3 초상화와 Field09를 혼동하지 않는다.
- Codex: **기존 40PASS/0FAIL + Main18 delta92PASS/0FAIL 재사용**. v2 납품 이후 실제 Tiled Repeat/Overlay·Guide6 시각 Delta QA 및 Main18 제한적 동선 확인; 실제 **시각 결함0**, 안전한 원본 보존·별도 Asset Reference 적용이 확인된 후에만 정식 Unity 반영. UserData/원본 Editor/원격 main 보호.
- 전체 `main`의 HEAD는 2026-10-19 EOD `SYNC_COMPLETE`로 통합돼 있으나, **Field09 v1 실패 및 v2 새 미술 미납품은 EOD main에서 공식 적용 제외**. 승인되지 않은 아트 ZIP을 `main`에 직접 commit/Push/merge 금지. 담당 `docs/art-renewal`, 기획 `docs/planning`, Codex `codex/*` branch 전용.
- 다음 보고는 (1) 원본8 GitHub 바이너리 읽기/접근 결과, (2) 지면6·Guide6 중 실제 수정 확인된 사례, (3) v2 PNG8 산출과 ZIP SHA, (4) Unity 미검증 및 대표 미술 승인 필요 항목, (5) Branch/Commit을 명시. 동일한 '원본 없음' 보고만 반복하면 `BLOCKED_NO_DELIVERABLE` 유지.
