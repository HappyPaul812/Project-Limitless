# LIMITLESS / 2026-11-16 EOD — 승인 작업 안전 통합 및 PC 고장 대비 원격 복구

**발주자:** 바울 대표. **상태:** `USER_EOD_BACKUP_AND_CONDITIONAL_PUSH_APPROVED / LOCAL_CODEX_NOT_YET_EXECUTED`.
**수행:** 기획은 GitHub 승인과 복구 계획 기록만. 사용자 PC, Codex 로컬, Git/Unity 원본 및 원격 main은 아직 이 문서 생성 시점 변경·Push하지 않았다.

## 승인된 범위
- [Issue #100](https://github.com/HappyPaul812/Project-Limitless/issues/100#issuecomment-6913468404): SnowStoat M2 **Hit4 사용자 미술 최종 승인**. 승인 PNG `SnowStoat_M2_Hit4_2048x512_RGBA_CANDIDATE.png` SHA256 `dc2c2588b4ec3d06766ab042c4297898aebca4f907593a5820ce8cbad2f6aab5`. 담당 납품 ZIP `SnowStoat_Issue100_Hit4_20261116.zip` SHA256 `e058e9610ec90cf9c2f7e8b0f48b5cde42a70fe7319f861d5c53d1736ecaadfc`. 사용자 F: 실물 자체는 별도 Codex 검사 필요. **미술 승인 ≠ Unity 적용**. KeyPose·Idle4·Attack4 v2도 이전 승인. **Defeat4는 미제작**.
- [Issue #101](https://github.com/HappyPaul812/Project-Limitless/issues/101#issuecomment-6913091059): **OGA10·OGA07 손보정 v2 승인 및 로컬 Unity 원본 적용 완료**, 53/53 최소 QA PASS, 컴파일/Resources·원본 Console error0, 사용자 직접 실플레이 확인 대기. 기존 승인 NPC6 + OGA20 HOLD 보호. 로컬 main FF 최종 보고 커밋 `977f3b6c83793c50d03f44c851d6205d2fe7b2f5`; **REMOTE_NOT_PUSHED**.
- [Issue #99](https://github.com/HappyPaul812/Project-Limitless/issues/99#issuecomment-6906678804): OGA02/OGA17 로컬 적용 완료, Delta 62/62 PASS. 그 외 기존 승인 몬스터·환경·TTS·아트·문서는 원격 main보다 최신 로컬 버전에 존재할 수 있음.
- 2026-11-16 개발 영상은 **영상 담당의 별도 로컬 결과**다. 실제 동영상 산출물·최신 원본 영상의 Git 상태와 **별도 백업 여부** 확인 필요. 승인이 없는 대형 영상 파일을 공개 GitHub 저장소에 임의 Push하지 말 것.

## 원격과 기술 보류 기준

- 이 문서 작성 시 GitHub 직접 조회한 원격 main `bfd281545632877325f5d21e04cefb924042c465`.
- 원격 main `Unity/Client/ProjectSettings/ProjectVersion.txt`에는 `6000.5.7f1` 기재, 실제 작업 Editor는 **6000.6.5f1**. 어느 쪽이 맞는지 버전 파일만으로 판단해 덮어쓰지 않는다. 정상 프로젝트 버전과 manifest·lock은 **실제 로컬 에디터/프로젝트/인계**에 기반해 보호한다.
- [Issue #85](https://github.com/HappyPaul812/Project-Limitless/issues/85): PSD Importer·Animation 격리 C#오류0까지 확인됐으나 **2D Tooling USS/UXML Import 예외**, BattleProjectiles 실사용 자산 **GUID 6중복** 실제 충돌. 기술 게이트 `HOLD`. 이를 사용자 EOD 종료 지시만으로 PASS 처리/파일 수정/Push 강행하는 것은 금지. 현재 Release-ready 후보 아님.
- [Issue #85 마감 조건부 Push/원격 백업 지시](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6913469590)가 최신 사용자 승인 범위. 이전 Push 포기 지시보다 최신이지만 **비FF/force·검증 우회는 여전히 금지**.

## Codex 안전 실행 플랜 — 장시간 작업 분산/남은 사용량 낭비 금지

**0. READ-ONLY 상태 조사 (간단 재사용):** 기존 233 프로젝트·161worktree 감사와 현재 목록, 기존 로컬 원본 main/refs/dirty/untracked, `origin/main`과 HEAD, 사용자 Editor/Save·Discord 절대보호, 원본 Git status 바이트 및 F: 여유 공간. `git worktree list --porcelain`, `git status --porcelain=v2`, `git show`/remote fetch 등 읽기 검토. 기존 대용량 QA 전체복제/삭제/자동 클린업 금지.

**1. 회복점 생성:** 승인된 변경과 중요 미Push ref/원본 헤드별 복구 경로를 분류해 **유실·중복 없이 보존**. `git bundle` 등 별도 복구 아카이브는 로컬 한 곳에만 있으면 PC 고장 복원 불가; 이미 설정된 다른 물리 매체·암호화 원격이 있는지 확인하고 해시·실제 재열람/클론 가능성 검증. Git으로 보존할 수 없는 미커밋/untracked/ZIP/TTS/녹화영상/Discord/Save는 별도 명세로 남기고 적절한 안전한 비공개 위치로 백업하는 것만 승인. **공개 GitHub에 인증키·개인정보·Save·Discord 환경파일·비공개 영상/계약 자료를 넣지 말 것.**

**2. 승인된 완료 제품 정리:** 승인된 NPC 원본 적용, 서리들쥐 아트/기획/문서 등 **검증된 기존 로컬 commit**을 통합 후보에서 추적하되, **아트 승인만 있고 Unity 미적용인 SnowStoat 4모션은 아직 게임 적용하지 않는다**. 저장소에 없는 F: PNG/ZIP은 파일로 남겨 복구 대상을 표시. `main`에서 직접 커밋 금지, 별도 통합 브랜치에서 변경 목록·충돌·권리·비밀/바이너리 크기 심사. `main`으로의 로컬 FF는 ancestry/정확한 approved HEAD 확인 후에만, autostash/reset/rebase --onto/force rewrite 금지. 충돌이면 STOP/HOLD.

**3. 원격 보존은 main과 분리해 먼저 시도:** `origin/main`에 없는 검증된 **safe Git commit/branch**를 새 `backup/2026-11-16-safe-checkpoint` 같은 원격 ref로 **non-force Push**. 이 브랜치는 릴리스가 아닌 복구용. 전송될 **모든 신규 Git object/조상 history**에 토큰·패스워드·Discord 키·Save·개인정보·제3자 라이선스·50MiB 초과·Git LFS pointer 누락 등 위험이 없는지 사전 검사. 원격 공개 여부 확인. 문제가 있으면 그 브랜치도 Push 중지, 이유만 기록. 주요 ref를 추가로 백업하려면 각각 동일 보안 감사를 거쳐야 하며 `git push --all`, `git push --mirror`는 금지.

**4. 조건부 원격 main Push:** fetch 후 예상 remote SHA·fast-forward ancestry·필수 검사/권한·관계 충돌, 승인 제품 SHA, #85 Package Import·GUID 실제 runtime/Unity 테스트 게이트가 **검증된 PASS**일 때만 대상 통합 브랜치를 `origin/main`에 일반 Push. 현재 알려진 #85 HOLD가 유지되면 **원격 main Push하지 않는다**. `--force`/`--force-with-lease`/오류 무시/기존 대용량 QA 반복/패키지 실험 범위 확대/5 meta 자동 변경 금지. PR-only 보호면 PR 검사 경로에 따르고 우회 금지. Success는 remote `main` actual SHA 재조회로만 보고.

**5. 마지막 로컬 확인:** 안전한 FF/checkout만, 사용자 dirty/untracked/Unity Editor session/Scene/selection/BuildSettings/Save/UserData/Discord 변경0, 작업 종료 뒤 F: 실제 여유 공간 측정. 사용 중인 다른 worktree, Hold/미승인 ref 그대로. 리뉴얼·아트/영상/Discord 부서별 중요 미완료 작업은 원격 메타 기록과 복구 목록에 반드시 기록.

## 복구 완료 판정

| 항목 | 완료 조건 |
|---|---|
| `LOCAL_SYNC` | 실제 원본 로컬 main HEAD·통합 HEAD와 refs/working tree audit, FF 결과 |
| `REMOTE_MAIN_PUSH` | 원격 GitHub `main` 새 SHA 링크, lineage/릴리스 기술 PASS |
| `REMOTE_RECOVERY_REF` | 원격 backup 브랜치 실제 SHA·비밀검사 PASS, 다른 환경 fetch 가능 |
| `NON_GIT_OFFSITE_BACKUP` | **PC 밖**에 보관된 승인 작업 ZIP/PNG/영상/Save/Discord 미커밋·비밀자료의 안전한 암호화 복구 검증 |
| `HOLD` | 확인 안 된 항목을 `NOT_VERIFIED`로 명시. 보류 사유/최소 다음 단계 |

**주의:** GitHub 백업 브랜치만 성공했다면 Git에 들어간 데이터에 대한 부분 복구일 뿐, 컴퓨터 고장에도 전체 게임 개발을 100% 재개할 수 있는 것은 아니다.

**오늘의 공식 종료 기준:** 실제 원격 Push에 실패해도 로컬/원격 데이터 유실이 없고, 검증 가능하게 보존된 범위와 남은 리스크가 문서화돼 있어야 함. 이 문서는 기획 브랜치에 이미 원격 저장했지만, 사용자 PC 실제 최종 백업을 대체하지 않는다.
