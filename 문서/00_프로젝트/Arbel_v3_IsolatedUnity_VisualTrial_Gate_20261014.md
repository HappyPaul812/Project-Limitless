# LIMITLESS / 아르벨 환경 리뉴얼 v3 — 격리 Unity 시각 시험 및 사용자 미술 승인 게이트

2026-10-14 · 기획 담당 · 작업 기록 브랜치: `docs/planning`

**상태: USER_AUTHORIZED_ISOLATED_VISUAL_TRIAL / USER_ART_FINAL_APPROVAL_PENDING / ORIGINAL_UNITY_NOT_APPLIED / MAIN_NOT_CHANGED.**

## 승인 범위와 사용자 의사

기획 담당이 "아르벨 v3를 격리 Unity에 시험 적용해서 실제 게임 화면부터 보시겠습니까?"라고 제안했고, 바울 대표가 "그래."라고 승인했다.

**사용자 승인 대상은 별도 격리 환경의 임시 시각 시험 및 게임 캡처 제작이다.** 원본 Unity Scene/프로젝트에의 영구적 교체, 실사용 아트 최종 승인, 선택 장식 11종 추가, 다른 지역 변경, 원격 Push/main 병합 승인은 아직 아니다.

## 정본·예상 입력

- GitHub 최신 `main` / `CURRENT_STATUS.md`, `AGENTS.md`, `docs/art-renewal`의 `Arbel_Renewal_Art_v3_Metadata_Correction_Handoff_20261014.md`, `Renderer_Mapping_284.csv`, `Codex_Integration_Handoff.md`.
- ZIP: `F:/Downloads/Limitless_Arbel_Renewal_Art_v3.zip`, 기존 Codex 보고 SHA256 `84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b`. 재검증하지 않고 정상으로 가정하지 않는다.
- v3 60파일/PNG43 중 실사용 34(직접교체 후보 23, Optional 11), Preview8·QA1. 기존 v2와 PNG43 전체 byte 동일. 284/284 매핑, 보정 Rect8, Road seam0, 북서 십자 삭제 등 인계된 이전 PASS를 보호.

## Codex 격리 구현·캡처 작업

1. **원본 보호:** 사용자 원본 Unity 프로젝트(원본 활성 Editor/Play/포커스, 미커밋, Save·Settings·Scenes·TTS/BGM·Field04·기존 Sprite 참조 복구 자산)에 일체 변경 금지. 원본 HEAD/status/SHA baseline 기록. 별도 `codex/arbel-v3-visual-trial-20261014` 로컬 작업 브랜치 + 별도 worktree/복사된 격리 Unity Client, 격리 Save와 Library를 사용한다. 원본 Unity MCP에 연결해서 Play/Import/Refresh/Scene 열기 금지.
2. ZIP SHA, Manifest/CSV target·Importer PPU/Pivot/좌표/Sorting/GUID/fileID 대조 후, **격리 Client의 `Arbel`에서만** 승인 전 실험으로 새 Sprite 등록 및 후보23종 연결. 신규 `Assets/_Project/.../ArbelV3Trial/` 같은 별도 폴더·고유 meta 생성 허용은 격리 환경 한정. 기존 Scene 원본은 변경하지 않고 Git diff를 통해 역할별 최소 변경 확인. Optional 장식11개는 **배치0**. Portal 관련 장식은 절대 새로 배치하지 않는다.
3. **12개 바닥·도로·벽**은 본 배치 후보로 렌더링하고 seam·Sorting·현행 284 Renderer 대응 점검. **지붕2개**는 시각용으로 임시 연결하되 가림 HOLD로 구분해 원본 건물 좌표/Collider/NPC/출입구를 이동하지 않는다. **소품9개**도 시험용으로 연결하되 Alpha 그림자 픽셀 접지와 실제 줄기/본체/발·Collider의 맞물림을 따로 캡처·측정한다. 시각 불량은 시험 결과에 FAIL/HOLD로 남기고 임의로 Transform/Pivot 수정해 숨기지 않는다.
4. **진짜 Unity 스크린샷**: 가상/offscreen 또는 사용자 창을 건드리지 않는 별도 격리 Unity 프로세스의 Game/RenderTexture를 실제 렌더링해서 1280×720 기준 동일 카메라 위치의 `Before/After` 화면 제작. 예: (A) 마을 전체/광장, (B) 북서·동쪽 건물 지붕/출입구/NPC, (C) 길 16개 배치 연결부, (D) 소품9개 접지 및 Collider 표시 on/off, (E) Portal(-7,-4)/TreeSouthWest(-7,-4.2) 시야/이동 및 미배치 장식, (F) NPC 대화·상점·치유·펫 기능과 실제 HUD. 필요하면 실제 4:3/21:9 가독성 추가. 캡처마다 Scene, Camera, Unity 버전, `BEFORE/ISOLATED_AFTER`, Trial vs Official 여부를 파일명/외부 캡션에 표기. 기존 Static Composite Preview를 실 Unity 캡처로 오인하게 하지 않는다.
5. 만약 백그라운드 RenderTexture/격리 Unity가 환경상 지원되지 않으면 **오프라인 합성만으로 실제 런타임 PASS를 주장하지 않는다**. 장애 이유를 적고 가능한 산출물만 제공. 기존 사용자 Editor 포커스를 빼앗거나 Play를 강제 시작하는 대체 방법 금지.
6. 정량·기능 QA: 변경 23종 역할/PNG·Importer·meta 고유성, Sprite/Script Missing0, 원본 레퍼런스·기존 284 Renderer/실제 런타임 NPC 렌더러 차이, 지붕2 가림, 소품9 Alpha foot/Collider, 이동·Wheelchair, Portal/Exit, 카메라/Sorting, 상점·치유·펫, Save/Continue 및 주요 Main21 이후 기존 스토리. 범위를 벗어나는 기존 Search/Unity 버전 차이는 별도 PARTIAL. **기존 겹침을 발견해도 임의 수정 금지**.
7. 시각 결과를 한눈에 볼 수 있는 Index/Contact Sheet와 원본 해상도 캡처 파일, QA Markdown/CSV/변경 전후 비교, 위험 목록을 **사용자가 실제 열 수 있는 경로**로 납품. 파일 생성 여부와 경로 확인, 사용자 전달 가능한 파일/링크를 구분한다. 원본 자산에 새 Preview 저장하지 않는다.
8. 사용자가 보고서/이미지를 본 후 직접 **교체23종 미술 승인 범위**를 결정할 때까지 원본 프로젝트 공식 적용 금지. Optional 11종은 별도 대상별 승인 후에만 검토. 원본 파일/Scene/GUID/meta/Unity 실사용 Save·설정 보호검사 전후 결과 보고.
9. 별도 실험 브랜치의 Commit·Push 역시 이번 사용자 발언의 승인이 아니므로 **Push0/정식 Unity 적용0/main병합0**로 마친다. 결과 보고서 문서화를 원격 GitHub에 남기는 게 필요하면 사용자에게 범위를 물어본다. 직접 main Commit·Push/PR 병합 금지.

## 최종 산출물과 판정

- `ARBE L_V3_VISUAL_TRIAL` 유형의 실제 Unity GameView/RenderTexture Before-After 비교 이미지와 최소 6영역 시각 자료.
- `TECH_TRIAL_PASS / VISUAL_HOLD / FUNCTIONAL_FAIL / RUNTIME_NOT_VERIFIED` 항목별 결과 (기존 요건 유지). **Trial QA 합격은 사용자 미술 승인이나 원본 적용 승인이 아님**.
- 사용자에게 (1) 기본 23 후보 전체/일부 승인, (2) 지붕2·소품9 추가 수정 요구, (3) 선택 장식11의 배치 승인 여부를 **서로 구분해서** 물을 수 있는 비교 증거.
