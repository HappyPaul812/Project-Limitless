# main Push 거절 안전 통합 준비 — 2026-10-13

**INTEGRATION_PREPARED / PUSH_NOT_EXECUTED.** 원본 dirty 작업 트리를 변경하지 않고, 원격 main 기반 별도 worktree에 로컬 전용 문서 두 커밋을 순서대로 cherry-pick했습니다. 충돌0, 원격 네 커밋과 원래 로컬 두 커밋 모두 보존했습니다.

## 원인·실제 이력

- 원본 경로 `F:\study\codex\Project-Limitless`, 브랜치 main, 시작/보호 HEAD `ff022e63dfe364f559fcc29819d27b123ad31127`.
- 안전한 `git fetch origin` 성공. 확인된 원격 main `927f519aa09718400e683f0c51c9da00c04343ce`.
- `git log --left-right --graph main...origin/main`: 로컬 전용2, 원격 전용4. 공통 기반 `509886f85b6b50deb3f2b69ad7d1f1cc85945157`.
- 원격4: `83a5d1d`(Chapter2 리뉴얼 로드맵), `b833c1a`(역할/결정 인계), `eca6f1e`(10/13 지시), `927f519`(Chapter3 미술 콘셉트 인계). 게임 변경이 아닌 팀 문서9파일 변경입니다.
- 로컬2: `3a37aa2e9a972b1ff9de7c051c6c744b057c918f`(세린 정적 감사), `ff022e63dfe364f559fcc29819d27b123ad31127`(Field04 정적 대응표/보호 기록).
- 원격이 원본 로컬 HEAD의 조상이 아니어서 원본 main의 단순 Push는 fast-forward가 아니었습니다. 사용자 미커밋을 잃을 필요 없이 이력만 통합하면 됩니다.

## 격리 통합 결과

- 새 worktree `F:\study\codex\Project-Limitless\Temp\GitIntegration20261013\main-merge`.
- 브랜치 `codex/integrate-main-20261013`, origin/main 기반 생성. 이전 `Temp/GitIntegration20261012/main-merge` 및 `codex/integrate-main-20261012`는 그대로 유지했습니다.
- A → `93ccd6d` / B → `a759f66e8880980b49b5b8bfc68f3a7ebe8fdc7f`로 순서대로 재현. `git range-diff` 두 패치 모두 `=`(동일).
- `git merge-base --is-ancestor origin/main HEAD` 성공. `git diff --quiet origin/main HEAD -- Unity` 성공(게임 변경0). Cherry-pick 직후 worktree clean. 이후 본 보고서와 아트 검수 문서/Evidence만 선별 Docs 커밋으로 추가합니다.
- 원본 main 브랜치/HEAD는 이동하지 않았습니다. 원본 미커밋337 status 항목·실제822파일, 기존 게임/Save/TTS 포함 보호집합4,750파일은 별도 전후 SHA로 대조합니다. `pull --rebase/reset --hard/stash/checkout -f/clean`은 사용하지 않았습니다.
- 최종 재fetch에서 원격이 `9256687f7f380e15ffb9fb75f37af9675a3d58cc`로 전진했습니다. 추가 `0feffa5`(Chapter3 시각 가이드)와 `9256687`(투사 공격형 방향/리뉴얼 인계)을 통합 worktree에서 보존 merge했습니다. 충돌0, 원본 작업 트리 변경0입니다. 투사 컨셉 확정과 스킬 후보/ID/효과 미승인을 구분하고 게임 구현은 하지 않았습니다.

마지막 fetch에서 `405ce12b71a52cb94ce79dd313eed68861839537`의 Chapter3 회복/부활 아이템 **제안 문서1개**가 추가되어, 종료 전 Chore merge `4446d787e7ccf6f073f4c912deb5e742ba923c4e`로 보존했습니다. 최초 원격4개+진행 중2개+종료 전1개 = **원격 전용7커밋**을 모두 포함합니다. 아이템/스킬 구현이나 기획 확정은 하지 않았습니다.

## 사용자가 직접 실행할 Push 명령

최종 QA 보고의 통합 HEAD SHA를 확인한 뒤 다음을 실행하시면 됩니다.

```powershell
git -C "F:\study\codex\Project-Limitless\Temp\GitIntegration20261013\main-merge" push origin HEAD:main
```

`--force`를 쓰지 않습니다. 그 사이 다른 담당이 원격 main을 갱신하면 안전하게 다시 거절될 수 있으므로 원격 보존 통합부터 재확인합니다. 원본 dirty 작업 트리에서 단순 `git push`를 반복해도 원본 main은 아직 이전 분기 상태입니다. 위 명령은 **검증된 통합 worktree의 HEAD**를 main으로 올립니다. 이 작업에서 Push/원격 branch 변경은0입니다.

Push 후 원본 로컬 main 동기화는 사용자 미커밋을 보존하는 **별도 작업**입니다. 자동 stash/reset/원본 checkout을 권하지 않습니다. 원본 Unity·미술·TTS·Save를 새 통합 checkout으로 자동 이전하지도 않습니다.

## 후임 경계

아트 검수는 원본 게임 정식 적용을 포함하지 않습니다. Rock B v2 및 Field04 후보 Preview/기술 검수 후 사용자 최종 미술 승인 필요. TTS 원본 감사와 호출0 보호. Chapter3 투사 메모는 미확정이며 SkillDefinition/UI/ID/Save/코드 추가0입니다.

최종 SHA와 보호검증은 [이번 검수 총괄](ArtInspection_20261013_Summary.md), 각 보고서 및 로컬 Git 이력에서 확인합니다. 아직 GitHub 반영 성공으로 보고하지 않습니다.
