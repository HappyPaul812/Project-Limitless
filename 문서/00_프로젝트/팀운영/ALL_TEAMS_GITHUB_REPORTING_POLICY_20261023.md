# LIMITLESS — 전 담당 GitHub 정식 보고 의무화 (2026-10-23 바울 대표 결정)

**적용:** 2026-10-23 KST부터 전 역할·신임/후임 세션. 사용자 지시: **“각 담당은 다 의무화로 해줘.”**  
**적용 업무:** 기획, Codex/Unity 구현·검증, Chapter1·2 리뉴얼, Chapter3 이후 신규 아트, TTS, BGM, SFX, 개발기록 영상. 새 역할에도 동일하게 적용.  
**상태:** `POLICY_APPROVED / PLANNING_BRANCH_DOCUMENTED / MAIN_MERGE_REQUIRES_NORMAL_APPROVAL`.

## 1. 작업 완료 및 인계의 기본 경로 — 채팅만으로 끝내지 않는다

모든 담당은 작업 단위가 끝나거나 일일 마감/후임 인계가 발생하면 **GitHub를 먼저 정본으로 기록**한다.

1. 최신 `main`, 담당 작업 브랜치의 인계·관련 Issue, 그리고 다른 담당의 최근 승인·QA 상태를 읽는다. 이미 완료된 것은 재생성하지 않는다.
2. **담당 브랜치**에서 해당 산출물의 Markdown 보고/진척/QA 자료를 신규 작성하거나 기존 최신 문서의 상단을 갱신한다. 파일이 존재해야 하고 **실제 쓰기 성공(commit SHA)**을 확인한다. 작업 산출물이 없는 ‘시작 지시’만 받은 경우에는 Issue에 진행 상태를 기록한다.
3. `문서/00_프로젝트/팀운영/인계/<ROLE>_LATEST.md`를 갱신한다. 여기에는 작업 결과, 승인/검증 게이트, 다음 작업, 실물 파일 경로·SHA 및 **담당 브랜치·문서 commit SHA**를 적는다. 작업보고와 인계가 동일한 범위라면 한 번의 간결한 갱신으로 충분하다.
4. 관련 GitHub Issue 또는 PR에 **최종 진행 댓글**을 남긴다. 문서 링크·브랜치·커밋·무엇을 검증했고 무엇이 보류인지 포함한다. 해당 작업의 Issue가 없으면 기획에 Issue 등록을 요청하거나 자신의 권한 내 Issue를 사용한다.
5. 바울 대표에게는 **GitHub 문서/Issue 링크, 브랜치, 실제 commit SHA, 승인 필요 여부, 블로커**를 짧게 보고한다. **보고서를 채팅에만 붙여넣고 GitHub를 비운 상태는 공식 완료보고가 아니다.** 사용자에게 부서 간 보고서를 반복 복사하도록 부담을 전가하지 않는다.

**보고할 때 실제 이미지·음악·음성·영상 등을 GitHub에 무조건 업로드한다는 뜻이 아니다.** 큰 ZIP/WAV/PNG/MP4는 기존 승인 납품 방식·Repository 크기/저작권/비밀보호 계약을 따르고, GitHub 보고에는 **실제 납품처/파일명, 길이·규격, 바이트/파일 SHA256, manifest·CRC 검증, 접근 여부(실물 GitHub 업로드 여부 포함)**를 반드시 기술한다. 파일을 볼 수 없다면 `NOT_ACCESSIBLE`, 아직 납품 안 했다면 `NOT_DELIVERED`로 명시한다. 유저의 드라이브 경로는 실제 확인된 경우에 한해 별도 비공개 인계에 남기고, 개인정보·비밀키·토큰·개인 Save·미승인 원본을 공개 GitHub로 올리지 않는다.

## 2. 역할별 전용 브랜치·인계 파일

| 담당 | 문서 브랜치/작업브랜치 | 최신 인계 |
|---|---|---|
| 기획·총괄 | `docs/planning` | `PLANNING_LATEST.md` |
| Codex·Unity | `codex/작업명` 별도 worktree | `CODEX_LATEST.md` 및 `CURRENT_STATUS.md` |
| Ch1·2 리뉴얼 | `docs/art-renewal` | `WORLD_ART_RENEWAL_LATEST.md` |
| Ch3+ 신규 아트 | `docs/art-new` | `SPRITE_ART_LATEST.md` |
| TTS | `docs/tts` | `TTS_LATEST.md` |
| BGM | `docs/bgm` | `BGM_LATEST.md` |
| SFX | `docs/sfx` | `SFX_LATEST.md` |
| 개발기록 영상 | `docs/video` | `DEV_VIDEO_LATEST.md` |

인계 파일 공통 위치: `문서/00_프로젝트/팀운영/인계/`. 역할별 문서 경로가 최신 정본과 다르면 실제 파일을 확인해 수정한다. 브랜치별 기존 문서와 역사 기록·사용자 고유 변경을 무단 덮어쓰지 않는다.

## 3. 반드시 분리해서 기록할 상태

- `제작 후보(ART_CANDIDATE)` / `사용자 미술·청취 승인(USER_APPROVED)` / `오프라인 QA PASS` / `격리 Unity PASS` / `사용자 원본 PC 제품 적용` / `GitHub PR main 병합` / `원본 local sync` 각각 별도다.
- 완료된 검사만 수치와 증거로 보고. 이미지 미리보기는 **오프라인 합성인지 실제 Unity GameView인지** 구분.
- 사용자 미승인 대사·음원·아트/가격/Quest·미해결 Bug·OPEN 항목을 완료로 승격 금지.
- 각 부서는 **할당된 작업 범위의 변화와 보호할 기존 공식 파일**을 명시한다.
- 세션 한도 위험 시 **후임 인계도 GitHub 우선**(깃허브 쓰기 성공 후 문서 링크/SHA 보고), 권한이 없으면 미반영 상황과 대체 텍스트를 명확히 보고.

## 4. GitHub 권한·메인 통합 안전 규칙은 그대로

- **`main` 직접 commit/Push/Force Push/무단 병합 금지.** 담당 브랜치에만 보고서를 commit한다. PR/main 병합은 별도 사용자 승인과 안전한 diff·QA 절차가 있어야 한다.
- **Codex 예외:** 루트 `AGENTS.md`의 “로컬 GitHub `push`는 사용자 명시적 요청 시에만” 규칙을 유지한다. 본 문서화 의무 자체가 **Codex의 자동 remote push나 제품 머지 승인은 아니다**. Codex는 작업브랜치에서 로컬 문서·증거를 확실히 만들고, GitHub 쓰기가 허용된 별도 직접 도구가 있으면 Issue에 상태를 남긴다. remote Push 미승인/불가 시 `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`와 commit SHA·명시적 사용자 승인 필요를 보고하고 절대 `GITHUB_REPORT_COMPLETE`라 쓰지 않는다.
- GitHub 직접 문서 쓰기 권한이 없는 담당은 도구 연결/권한 문제를 알리고, 파일 초안·실제 로컬 commit·증거를 준비하되 **GitHub 반영 전에는 `REPORT_PENDING_GITHUB_WRITE`**로 표시한다. 단순 채팅 보고를 최종 공식 보고라고 부르지 않는다.
- 미커밋/미추적·Save/UserData·Editor 상태·승인 펫/공식 아트와 타 부서 기록은 수정하지 않는다. 변경 충돌 시 확인 없이 덮어쓰기 금지.

## 5. 담당 최종 보고 공통 템플릿

```md
# [LIMITLESS / 담당 / 작업명 / YYYY-MM-DD]
- 상태: COMPLETED / PARTIAL / HOLD / USER_REVIEW_PENDING / NOT_VERIFIED
- 실제 완료: 파일/기능/범위 (이전 완료분과 구분)
- 증거: QA PASS/FAIL 및 실제 근거, 납품 파일·규격·SHA256/위치 (실물 확인 여부)
- 보호/제외: 기존 승인자산·UserData·금지한 변경
- 다음 단계: 사용자 승인 필요 여부 / Codex 통합·추가QA 여부 / 담당간 인계
- GitHub: Issue/PR 링크, 문서 상대경로, 담당 브랜치, 실제 commit SHA
- 통합: 격리 / 원본PC 적용 / 원격 main 병합 / 로컬 동기화 각각 상태
```

**검수 지표:** `GITHUB_REPORT_COMPLETE`는 **유효한 GitHub 문서 커밋+최신 인계+Issue/PR 완료댓글+바울 대표 요약 보고**가 확인된 상태만 의미한다. 증거가 일부 없으면 완료 범위를 좁혀 `PARTIAL`로 보고한다.

## 6. 시행 및 전달

기획 담당은 공통 규칙·시작 안내·후임 프로토콜·관련 Issue에 이 정책을 링크하고, 모든 담당에게 **자기 브랜치에서 이 정책을 수용해 최신 인계를 갱신**하도록 지시한다. 이미 활동 중인 세션들이 **GitHub Issue만으로 자동으로 새 지시를 읽거나 실행한다는 보장은 없다**. 따라서 기존 담당에게는 다음 업무 시작 지시나 종료 보고 전에 본 정책을 직접 전달하며, 확인되지 않은 시행·동기화를 주장하지 않는다.
