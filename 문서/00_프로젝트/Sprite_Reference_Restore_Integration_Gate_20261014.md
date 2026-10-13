# LIMITLESS / 2026-10-14 기존 Scene Sprite 참조 복구 및 PR #10 재통합 게이트

**상태: AUTHORIZED_FOR_CODEX_REPAIR_QA / MAIN_MERGE_BLOCKED / PENDING.**
**기획 출처:** 2026-10-13 EOD 합본 BLOCKED 보고를 이어서 처리. 본 문서는 작업 지시이며 Unity/프로젝트 파일이 복구됐다는 뜻이 아니다.

## 검증된 현재 기준 (2026-10-14 오전 원격 GitHub)

- 통합 브랜치: `codex/eod-integration-20261013`, 원격 HEAD `47b865e9c19c23d30c2c9eb5e14c79cddb79a52d`.
- main: `0e0476649b667129cb7aa6004f4b677335ed8be2`.
- [PR #10](https://github.com/HappyPaul812/Project-Limitless/pull/10) = OPEN/Draft/미병합. 기존 PR #7은 중복 통합 금지.
- `문서/00_프로젝트/EOD_Integration_Final_QA_20261013.md` 기준: 통합 A/B/C·Chapter3 문서·Main08 승인6 WAV·Field04 공식42/Renderer375/PNG52 유지. 기능 854 PASS/0 FAIL·정적 83 PASS. 23 BuildScene Edit Mode 전체 검사에서 Sprite null 기존 main 2111, 통합 1749. Field04 null362→0. Arbel Edit285/Runtime284, Field05 Edit/Runtime362와 실제 배경 누락. Field06~08도 참조누락. 본 문제는 main에서도 재현되며 신규 EOD 회귀 증가0. 다만 실제 게임 배경 결함이므로 배경 수정 없이 main 병합 금지.
- 기존 결함/제약: UnityEditor.Search 예외, 6000.5.7f1 정본과 실제 QA 6000.6.5f1, Field04 서쪽 Spawn↔TreeWest 겹침·북쪽 Collider 틈, 태온005/006 기존 음성 오류, 스킬 Save 영속 미구현, 수동 입력 미검증.

## Codex 전담 복구 요청

1. `origin/codex/eod-integration-20261013` 최신 HEAD를 시작점으로, **새 독립 worktree/브랜치 `codex/sprite-reference-recovery-20261014`** 에서 작업. 먼저 main/원본 로컬의 미커밋·Scene·Save·PNG/WAV/meta·Settings·HEAD·SHA 목록 보호. 기존 사용자 원본 Unity Editor/Play/OS 포커스, Unity MCP foreground 조작 금지.
2. 기존 main과 합본의 23개 BuildScene Edit/Runtime 검증 근거를 재사용하되, 실제 Sprite 누락 Scene·Renderer·원래 sprite GUID+localFileID·Texture importer(Single/Multiple·rect/pivot/PPU)·source PNG SHA를 전수 표로 인벤토리. **동적 할당·의도적 null**(예: StarterVillage VillageNpc1 Runtime 초기화로 해결)은 구분하고 합법적인 예외 근거를 기록. 실제 화면이 빈 Arbel/Field05부터 복구한 후 Field06~08 및 다른 활성 씬의 누락을 검사.
3. `main`의 ThirdParty 원본 PNG/meta를 직접 수정하지 않는다. 기존 사용자 수동 Importer/meta를 읽기 전용으로 비교한다. 원래 그림을 보존하는 **동일바이트 `Assets/_Project/...` 프로젝트 소유 복사본 + 올바른 Sprite subasset등록/참조 최소 변경**을 우선. 기존 프로젝트의 `LegacySpriteRegistration20261013` 후보와 PR7 Importer 수정 근거를 참고하되, Field04 정식 Road42의 현재 승인 Scene·Sprite375를 역행시켜 깨뜨리지 말 것. 새 GUID의 유일성 및 기존 안정 GUID/파일ID·스프라이트 분할 매핑을 증명한다.
4. **Scene 전체 재생성 금지.** 배경 Sprite 참조와 필요한 프로젝트 관리 meta/PNG만 수정하고, Collider·Transform·Sorting·Tint·Quest/NPC·Spawn/Exit·카메라·몬스터·Story·Save 포맷·전투 스킬·Main08 WAV6·TTS/BGM·다른 아트 작품은 그대로 보존. 기존/신규 아트 변경은 미술 리뉴얼 담당의 별도 승인 대상이다.
5. 먼저 Arbel·Field05 배경을 실제 RenderTexture/Runtime로 검증하고, 23 BuildScene의 Edit+Runtime Sprite 참조를 끝까지 검사. 가능한 경우 **실제 렌더링 이미지가 기존 사용자 원본의 올바른 Sprite 설정과 동등**함을 증명. GUID 파일 존재만으로 PASS 판정 금지.
6. Unity Batch Compile + 전체 Scene 참조, Main12~13·Field04 Road42/Renderer375·Main08 승인 PCM6, 기존 스킬 관리·2×3/기본 15, 펫 서비스3거점, 아르벨/Field05~08 출입·전투·Camera·Save/Continue·지체의 길·35초 리스폰 등 **854 PASS 검증 범위에 미치는 회귀**를 검사. 새 누락·스크립트 Error0, `git diff --check` 및 보호 파일 SHA 검증. 기존 Search 예외/버전/미검증 항목은 PARTIAL로 명시.
7. 독립 QA가 성공하면 수정사항만 작업 브랜치 Push. 통합 브랜치에는 **별도 충돌 점검 및 브랜치 QA 후** 반영·Push하고 PR #10 업데이트. 사용자 이전 통합 승인은 검증 성공 시 PR #10 Ready→`main` 정상 병합에 사용 가능하나, **필수 전체 Sprite 게이트 미충족이나 새 회귀가 있으면 `main` 병합 금지**. 직접 `main` commit/Push, force Push/clean/reset hard, 기존 원본 worktree 변경 금지.

## 보고 형식

- 오류 원인: 왜 GUID파일은 있으나 실제 SpriteRenderer.sprite가 null인지, importer/subasset·localID 증거
- Arbel·Field05~08 각 Scene별 **전/후 Edit null, Runtime null, 실제 배경 출력**; 다른 활성 씬 전수
- 예외: 동적으로 채워지는 항목, 해결 불가 참조 및 남은 리스크
- 승인 Field04 42/Renderer375/PNG52, Main08 승인6, B/C, Chapter3 문서의 보호 상태
- 기능·정적·참조 QA PASS/FAIL/PARTIAL, 기존 문제와 신규 회귀 구분
- 원본 로컬/사용자 미커밋 100% 보호 및 Save/설정/Unity Editor 비접촉
- 새 브랜치·커밋·Push 상태, `codex/eod-integration-20261013` 통합 HEAD, PR #10 Draft/Ready, 원격 main SHA 및 실제 병합 여부

**운영 원칙:** 아트 신규 제작/Arbel 리뉴얼과 Sprite 참조 정정은 별개 업무. 먼저 기존 화면이 올바르게 나오도록 기술적 복구를 마친 뒤 정식 아트 리뉴얼을 진행한다.
