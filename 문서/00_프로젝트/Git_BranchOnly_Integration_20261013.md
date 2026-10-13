# 담당별 브랜치 전환·기존 통합 이력 보존 — 2026-10-13

**준비 완료 / 작업 브랜치만 Push 대상 / Push 미실행.** 사용자 최신 지시에 따라 AI는 main에 직접 커밋하지 않습니다. 기획은 docs/planning, Codex는 codex/작업명 브랜치를 사용합니다. 이전 HEAD:main 안내는 이 기록의 작업 브랜치 Push 안내로 대체합니다.

## 관측·보존

- 원본 `F:\study\codex\Project-Limitless`의 main HEAD는 `ff022e63dfe364f559fcc29819d27b123ad31127`입니다. 사용자337 status 항목/822미커밋 파일이 있으며 원본 checkout/index/파일을 수정하지 않았습니다.
- 이전 통합 worktree `Temp/GitIntegration20261013/main-merge`와 브랜치 `codex/integrate-main-20261013`의 HEAD `961c90d5d0c35145ba2acd80358fcfda783947cd`를 그대로 유지했습니다. 버리기/cherry-pick 재작성/amend/reset/stash/force Push0입니다.
- 안전한 fetch 후 origin/main은 `73c37bf02232056bb9c101d80f0e82d383d1b980`. 이전 통합과 비교해 원격 신규1커밋이며 Chapter3 회복/부활 아이템 문서8줄 변경입니다. 최신 원격 원문을 보존했고 이번에 아이템/스킬 구현을 하지 않았습니다.
- 기획 규칙은 `origin/docs/planning`의 `74c5a0bedb277fbfa369c2bf8072d821feea5e0a`에서 읽었습니다. GIT_BRANCH_WORKFLOW_20261013.md와 COMMON_RULES의 main 직접 커밋 금지를 확인했습니다. 이 정책 문서는 아직 main 미병합 상태입니다. 사용자 최신 지시를 즉시 따르며 **기획 브랜치를 덮거나 승인 없이 그 정책 커밋을 main으로 올리지 않습니다.** 기획 변경은 해당 원격 브랜치에 보존되어 있습니다.

## 새 통합 준비

- 새 worktree: `F:\study\codex\Project-Limitless\Temp\GitIntegration20261013V2\main-integration`.
- 새 브랜치: `codex/integrate-main-20261013-v2`.
- 이전 통합 SHA961c90d에서 새 브랜치를 만들고 origin/main을 merge했습니다. 충돌0. 이전 통합961c90d와 최신 main73c37bf **둘 다 새 HEAD의 조상**입니다. 기존 A/B·QA/Preview·원격 문서 이력을 재작성하지 않고 유지합니다.
- 새 merge 메시지는 `Chore: 기존 통합 이력과 최신 main 문서 보존 병합`. 본 보고서/이전 Push 안내 수정/통합 worktree 상태와 후임 인계만 Docs 커밋으로 추가합니다.
- Unity/gameplay/TTS/Save/원본 아트 변경0. 새 origin/main 대비 Unity diff0, 선택 diff 공백 검사 및 새 worktree clean 상태를 확인합니다. 이번 요청은 Git 통합이므로 Unity 실행/QA 재실행0입니다.
- 작업 시작에 현재 원본 보호집합4,750파일을 다시 SHA256으로 확보하고 종료 후 비교합니다. 기존 미커밋337항목의 status raw bytes·822파일과 원본/이전 통합 HEAD도 유지합니다. 원본 Editor나 OS 포커스 조작0입니다.

## 사용자가 직접 실행할 안전한 명령

아래는 **Codex 작업 브랜치만** 생성/갱신합니다. main에 Push하지 않습니다.

```powershell
git -C "F:\study\codex\Project-Limitless\Temp\GitIntegration20261013V2\main-integration" push origin HEAD:refs/heads/codex/integrate-main-20261013-v2
```

최종 응답에는 검증된 SHA를 출발점으로 고정한 명령도 제공하므로, 그 명령을 복사하시면 됩니다. 원격 동일 브랜치에 더 새로운 이력이 있다면 일반 Push가 안전하게 거절하며 강제 옵션은 사용하지 않습니다. main 병합/PR 생성/승인은 이번 작업에서 실행하지 않고 사용자 결정으로 남깁니다. GitHub 반영 완료라고 보고하지 않습니다.

## 후임

앞으로 로컬 main 또는 다른 담당 브랜치에서 작업하지 말고 codex/주제별 브랜치+별도 worktree를 사용합니다. 원본 사용자 파일과 이전 통합 worktree를 정리 명목으로 삭제하지 않습니다. 아트 승인 대기·TTS 호출 금지·투사 미승인 스킬 구현0 등 기존 검수 경계를 유지합니다.

최종 SHA는 이 문서가 포함된 마지막 Docs 커밋과 최종 응답으로 확인합니다. 원본 main 동기화나 main Push를 자동 실행하지 않습니다.
