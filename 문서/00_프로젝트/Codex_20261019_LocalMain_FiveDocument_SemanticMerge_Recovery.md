# LIMITLESS — Codex 로컬 main 문서5 충돌 비파괴 해소 및 다음 단계 (2026-10-19)

**승인 발화:** 바울 대표 “충돌 해결하고 다음으로 가자.” / **기획 브랜치:** `docs/planning`. 이것은 로컬 컴퓨터 Codex 작업을 위한 실행 계획이며 문서 작성만으로 사용자의 Windows 파일·Unity 에디터를 변경하지 않는다.

**최신 확인 원격 `main`(2026-10-19):** `af21edbf5f0cdc0f241b09228ede960ab83e9b6c` (작업 시 반드시 fetch 후 재확인). PR #18/#19 Main18, #22 Field05 B42, #21 Main09, #23/#24 인계 모두 **main 병합 완료**. Main09 기능9파일·Field05 Scene+신규32파일·Main18 WAV10/meta10/Catalog은 Codex 보고상 **원본 디스크 안전 반영**됨. 사용자 원본 에디터의 *자연 Import·직접 Play*는 미검증. **각 기능 새로 제작/중복 병합 금지**.

## 1. 진단 데이터 — 5파일만 원격/로컬 내용 상이 (과거 보고 기준)

Codex 증거:
- `문서/00_프로젝트/Evidence/IntegratedCloseout20261019/ActualDirtySyncConflicts.json`
- `문서/00_프로젝트/Evidence/Field05Official20261019/FinalLocalProtection.json`.
- 기존 원본 로컬 `main`: `ff022e63dfe364f559fcc29819d27b123ad31127`. PR16이 로컬 전용 문서 2 커밋(`ff022e63...`, `3a37aa2e...`)을 **원격 main의 이력으로 보존**했다는 Codex 기록 있음. 하지만 실제 원본 `main` HEAD는 구버전 그대로라 `LOCAL_SYNC_PARTIAL`. 최근 tracked dirty320 + untracked1053, 원격과 겹친 경로517 중 **512 byte 동일**, 아래 문서5개가 **내용 불일치**. 실물 최신 값을 다시 산출해 과거 수치를 맹신하지 말 것.

정확한 5파일:
1. `문서/00_프로젝트/CURRENT_STATUS.md`
2. `문서/00_프로젝트/EndOfDay_Integration_Voice_LocalSync_QA_20261014.md`
3. `문서/00_프로젝트/팀운영/인계/CODEX_LATEST.md`
4. `문서/00_프로젝트/팀운영/인계/PLANNING_LATEST.md`
5. `문서/00_프로젝트/팀운영/인계/WORLD_ART_RENEWAL_LATEST.md`

**절대 금지:** `git reset --hard`, `git clean`, `git checkout -f`, `git restore`로 백업 없이 덮기, 강제 push, 무단 stash/삭제, Unity Editor/Play/포커스/강제 Refresh, Save/UserData/Settings 변경, Unity Library 중복 빌드, 변경 파일 내용 손실. 단순 `git merge -X theirs`/`ours`, `-s ours`, one-side overwrite로 이력 왜곡·내용 상실 금지. 단순 브랜치/HEAD 강제이동으로 가짜 SYNC_COMPLETE 금지.

## 2. 사전 보호 / 단계별 병합

1. 사용자 PC 원본 경로 `F:\study\codex\Project-Limitless`에서 상태를 **읽기 전용 스냅샷**: `git status --porcelain=v1 -uall`, `git branch -vv`, `git worktree list --porcelain`, `git rev-parse HEAD main origin/main`, `git merge-base main origin/main`, `git log --left-right --cherry-pick main...origin/main`, 현재 원본 Unity/Editor·Save23 상태. 다중 프로세스/Editor가 Git 대상 파일을 편집 중인지 확인. 안전한 `git fetch origin` 후 remote SHA 재측정.
2. 충돌5 **로컬 작업복사 실물** + **로컬 HEAD의 문서** + **원격 main 문서** + 각 조상/공통 기준 바이트를 별도 `Temp/` 또는 사용자 작업 외부 백업으로 저장하고 SHA256·라인/줄바꿈·변경 timestamp 및 인코딩 기록. tracked dirty 전체320, untracked1053, UserData23의 **현재 해시/경로 인벤토리 및 비파괴 백업**을 먼저 검증(모든 프로젝트 16GB나 Unity Library 중복 복사 지양). 승인 기능 원본 Field05 Scene·Main09 9파일·Main18 WAV/meta/Catalog의 SHA/기존 GUID도 보호.
3. `origin/main`에서 시작하는 **깨끗한 별도 worktree**, 브랜치 예 `codex/local-doc5-semantic-merge-20261019`를 만든다. 원본 폴더에서 `checkout`/HEAD 이동/파일 교체 하지 말 것.
4. 위 **5문서만 의미 병합**: 원본 로컬에만 있는 날짜별 과거 작업 기록, 사용자 승인·QA·OPEN/환경 PARTIAL·코드커밋·경로/인계사항을 원격 최신 문서에 보존 및 날짜/상태 정합. 특히 **신규 PR #21~24 병합/원본 적용은 최신 확정**, 10/14 미완료는 `[과거 당시]` 표시. 중복 문단 제거해도 고유 사실 유실0, 기획 고유 내용/리뉴얼 고유 내용 임의 삭제0. `PLANNING_LATEST`나 `WORLD_ART_RENEWAL_LATEST`의 독립 담당 변경을 원격과 대조하여 다른 담당 작업 덮지 말 것. 충돌 원본은 백업에 남기되 게임 repo에 `<<<<<<<` 따위 conflict marker 0.
5. 5문서 병합 뒤 `git diff --check`, 문서UTF-8/links/headings 검사, 유니크 문단/미반영 변경 인벤토리, **문서5 외 변화0**, QA evidence/기존 Git commits 보존 확인. **게임 코드·Scene·음성·Assets 변경0**이므로 Unity 수천 건 Runtime 재검증은 요구하지 않는다.
6. 전용 브랜치 commit→Push→PR→diff 검수 뒤 사용자 요청 범위인 문서정합에 한해 승인된 경우 main 병합. main 직접 commit/push 금지. 대표가 따로 결정을 해야 할 의미 충돌이 있으면 추측으로 선택하지 말고 `BLOCKED_DOCUMENT_DECISION`으로 정확한 문단/두 값만 질문.

## 3. 원본 local main 동기화의 별도 필수 게이트

**문서 5개 정리·PR main 병합만으로 원본 local main 충돌이 자동으로 해결되는 것은 아니다.** 새 origin/main SHA에 대한 원본 디스크와 index/HEAD 갭을 다시 조사한다.

- 먼저 **원본 작업 트리 전체를 지우지 않는 방법**만 검토하고, 깨끗한 격리 복제/시뮬레이션에서 정확한 dirty/untracked overlap과 commit graph를 복원해 **손실0 sync plan** 검증. 이미 원격에 포함된 로컬 문서 2커밋은 재-체리픽/재푸시하지 말 것. 사용자의 현재 문서5가 의미병합된 원격 내용과 다르면 보존 후 기준에 맞춰 reconciliation 절차 설계.
- Git `HEAD` / branch 참조 / index / working tree / untracked files / Unity 원본 disk는 **서로 별개**. 브랜치 포인터만 옮겨놓고 `SYNC_COMPLETE`이라고 선언하지 말 것. 원본 Unity Editor를 강제 종료하거나 Library/Save를 덮지 않는다.
- 비파괴 계획과 격리 리허설이 100% PASS하고 필요한 5문서·dirty 전체와 현재 사용자 승인 기능이 완전히 보존될 때에만 원본 실제 main 동기화를 실행. 한 경로라도 예상치 못한 바이트 변화/충돌 발견 시 원본 변경 STOP, **별도 clean worktree에서 승인 최신 main으로 개발을 진행하는 안전한 대안**을 제공하되, 원본 local sync는 `PARTIAL`로 정직하게 남길 것. 사용자 원본을 구버전 브랜치에 둔 채 새 worktree를 만들었다면 그 사실과 어느 경로를 Unity가 열어야 하는지를 분리 보고.
- 검증 상태를 정확히 3개로 나눠 보고: **GitHub main=head**, **로컬 original main과 origin/main 및 index**, **원본 Unity 디스크/Editor Runtime**. `git status`가 clean이어야 한다는 이유로 원래 소중한 미커밋작업을 삭제/임의 Commit하는 것은 금지. Sync_COMPLETE는 원본 브랜치/인덱스/작업실체·기능/백업 보호까지 끝났을 때만 선언.

## 4. 충돌 해결 다음 — 기존 구현 작업으로 복귀 (중복 업무 금지)

- **Main09, Field05, Main18은 이미 GitHub에 통합 및 원본 디스크 적용 기록**이 있으므로 기획/구현/미술 재승인·새 검수 빌드·Field05 강제 입장·모든 런타임 QA 재실행 **하지 않는다**. 에디터를 사용자가 평소처럼 열 때 자연 Import와 **실제 정상 플레이에서 Main09 안내/Field05 렌더 확인**만 검토(유저 Editor/입력/OS 포커스 강탈 금지).
- 게임의 **다음 미적용 TTS는 Main20(현재 Manifest상 세린5개 `TTS_PENDING`)와 Main21(현재 Unity Catalog 음성0)**. 이것은 TTS 담당 제작 구간이므로 Codex는 **재생성·API 과금0**. 동기화가 끝나고 사용자가 다음 구현을 선택할 수 있도록 Main20/21 실제 Dialog Manifest/기존 WAV·Catalog와 문서 상태를 **읽기 전용으로 짧게 점검**해 누락 목록/담당분담만 보고할 것. **새 TTS 제작 지시를 임의 발동하지 않는다.**
- 챕터3 기획의 Main22~29·서릿길마을 Main23~24는 `docs/planning` 정본에 준비되어 있으나 사용자 승인 없이 신규 Unity Chapter3 기능을 임의 대량 구현하지 않는다. 다음 우선순위 후보로 시간/리스크만 제시한다.

## 5. 최종 보고 서식

- 원격 main 정확SHA / 원본 local main SHA / origin/main SHA, 원본 Unity 디스크 반영 상태, branch/worktree별 HEAD.
- **5파일 각각** 로컬 유니크 내용 보존 근거(백업 SHA, 병합 후 SHA, 다른 담당 문서 보호) + PR번호/브랜치/커밋.
- 실제 기준 dirty tracked/untracked 시작·끝, UserData23/Save·WAV/Catalog/Main09/Field05 보호 SHA 변화0·변경 허용 목록.
- `SYNC_COMPLETE` vs `DOCS_MERGED_LOCAL_PARTIAL` vs `BLOCKED` 명확 구분. Editor 자연Import/손플레이 미검증은 PARTIAL로 계속 표시.
- Main20/21 TTS 읽기 전용 조사 결과와 다음 **단 하나의** 추천 개발 순서. 복붙형 재개 지시문을 함께 전달.

**요지:** 이번 명령은 사용자 문서5의 의미병합·로컬 동기화 안전화 및 다음 작업 전환 허가이지, 현장 사용자 저장 데이터 삭제·기존 완료 기능 재개발·비승인 신규 API 호출 허가가 아니다.
