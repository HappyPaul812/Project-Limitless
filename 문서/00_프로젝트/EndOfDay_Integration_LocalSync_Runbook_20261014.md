# LIMITLESS — 2026-10-14 일일 통합·음성 보정·로컬 동기화 실행 계약

**기획 담당 작성, `docs/planning`. 실행 담당 Codex. 최신 status `PARTIAL_COMPLETED / USER_BROAD_INTEGRATION_AUTHORIZED / AUDIO_REPAIR_LOCAL_SYNC_PENDING`.**

## 0. 사용자 최종 지시와 현재 GitHub 현황

바울 대표: "**다 들어봤는데 원래 짝은 다 찾았는데 앞 공백이 좀 긴게 있는데 그건 알아서 처리해주고, 다 고치고, 버그 다 잡고, 오늘 꺼 다 합치고, 깃허브랑 로컬 동기화까지 해줘. 피곤해서 일찍 자려고**." 추가 청취 승인을 반복 요구하지 않고, 원본 보호 및 실제 QA를 통과한 **당일 승인 작업**에 대해 통합과 로컬 비파괴 동기화까지 위임한다. 단, ‘다 버그’는 범위가 무한대인 명령이 아니며 **확인된 당일 범위**에서 해결·미해결을 구분해야 한다. 배경의 과거 OPEN을 임의 수정/QA 0오류로 위장하지 말 것.

19시경 GitHub 최신 조회:
- `main` 기존 `314f7fbbd66799ff93ec71404bcf388023c655be` + **Arbel PR#12 이미 기획 세션이 병합 완료** → `3aa66981f2545d463126e01a96c359c21ca804ea`.
- `codex/arbel-v3-official-apply-20261014` HEAD `652867c988d015f0eb65ccb82591d2ef3a36551b`: QA **3,953 PASS/0 FAIL(단위 assertion)**, 승인23 PNG/국소 투명·처마 접합, 기존 Scene Arbel 1파일 변경·신규 경로55, Portal/나무와 Editor/URP 기지 이슈 별도 OPEN. **다시 main에 병합하지 말고 기록으로만 보존**.
- `docs/planning`에 Main21 귀환·펫 서비스·Main22~24 탐험/전투·미엘 복선·Arbel 승인·음성007~011 승인 등의 문서 작업. **기획 PR을 만든 경우 머리 SHA 확인 후 QA·충돌 확인 뒤 병합**; 기획 내용에는 제안/확정/미구현 상태 표시 보존.
- `docs/art-renewal`, `docs/art-new`, `docs/tts` 등 부서별 기존 장기 브랜치를 오래됐다는 이유만으로 일괄 main으로 끌어오지 말 것. **오늘 검증·승인된 파일만 선별**. TTS Main18/세린 미승인·별도 대기는 병합해도 승인작품으로 승격하지 않는다.
- 기존 PR#10 EOD/Sprite 복구와 PR#11 Main08 Taeon005/006은 이미 `main`에 병합됐다. 다시 적용·cherry-pick하지 말 것.
- Codex `F:\study\codex\Project-Limitless` 실제 디스크 HEAD/dirty 상태는 기획 담당이 직접 볼 수 없다. 원본에 Arbel v3 Scene1 + 파일55가 이미 있는지 **Codex가 해시로 확인**하고 보호한다.

## 1. MAIN08 태온 007~011 — 이제 음성 정식 적용 승인

기획 문서 `문서/00_프로젝트/Main08_Field02_Field03_Taeon007_011_Voice_Content_Audit_20261014.md` 최신 승인 섹션을 읽는다.

사용자 청취 승인 결과 **원래 맞는 짝은 모두 찾았음**, **일부 선행 공백만 길다**. Codex가 제작한 로컬 후보/HTML 재생 목록/manifest를 검색·사용자 확인 버전과 대조. 다섯 ID와 정식 원문은:

| ID | 본문 | 과거 Runtime 증상 |
| --- | --- | --- |
| `main08_taeon_supp_007` | 폭이 같습니다. 아마 맞을 겁니다. | 무음 |
| `main08_taeon_supp_008` | 누군가 이곳을 조사한 흔적입니다. | 007 본문 재생 |
| `main08_taeon_supp_009` | 너무 조용합니다. | 008 본문 재생 |
| `main08_taeon_supp_010` | 그 이유가 무엇인지는 아직 모르겠습니다. | 앞 태온 음성과 겹치거나 늦게 시작 |
| `main08_taeon_supp_011` | 마법 같습니다. | 본문 정상이지만 시작 공백 길다 |

ID마다 **실제 연결 source WAV의 PCM 파일·ASR·발화 시작 onset·후행 여운·GUID/meta와 표기 본문을 직접 교차 확인**하고 합본/동일 원문 분할이 정확한지 증명. 기계적인 ID +1 이동 금지. 011도 시작 공백만 최소 제거하고 **'마' 시작 자음과 호흡**을 보존. WAV 내부 공백이 아닌 AudioSource 지연이면 TTS 편집만으로 가리지 않고 실제 재생 경로를 수정하되 최소 범위/QA 후 적용. 기존 WAV·승인005/006·다른 목소리 오디오 원본 불변. **새 TTS 생성·유료 호출 금지.**

결과가 모두 맞으면 **별도 `codex/main08-taeon007011-final-20261014` 작업 브랜치**(기존 이름 충돌 시 고유 이름)에서 신규 복구 WAV와 필요한 meta, `StoryVoiceCatalog.asset`의 **007~011 참조만** 최소 수정한다. 사용자 가사 아닌 텍스트 원문/StableID 변경 금지. 미엘004~008 등 전후의 모든 미엘 음성, 태온005/006 PCM SHA/GUID, Player 무음, Main08 목표·DeepZone 5+3페이지·Save/Continue·UI/Combat 전후 멈춤·재시작 QA를 보호.

검증: source↔catalog GUID/meta/clip SHA 1:1, 발화 앞여백 과다 0, 무음/과한 무음/자음 손상·클릭/클리핑0, 각 5개 실제 Unity 페이지 호출 재생이 자기 텍스트와 일치하며 다음 미엘 페이지에 새 이전 태온 음성 잔재0. 독립 ASR은 **청취 재승인 대체 보조**로만. 사용자 귀에 들린 정본과 후보가 일치하지 않으면 **STOP**, 임의 음성 발주 금지.

PASS 시 Commit/Push → 음성 전용 PR → 대표의 오늘 통합 승인에 따라 HEAD 검증 후 `main` 병합까지 수행. FAIL/PARTIAL이면 **문제가 남은 파일을 합격 처리하거나 섞어 병합하지 않고** 작업 브랜치에 보고, 무엇이 막혔는지 정확히 인계.

## 2. 당일 승인 범위 통합 QA

`main` 최신 fetch, origin 브랜치 목록 및 이미 병합된 PR과 로컬 overlay를 읽기 전용으로 비교해 **중복 merge/reapply 금지**.

- Arbel v3 23기본/새 sorting/depth/shader/접합: 배경 Renderer284/284·Missing0·원본 Scene/Portal·상점/치유/펫/접근성/Save/Continue·Field04 Road42 및 Story 이전 퀘스트 회귀.
- 음성007~011: 바로 앞/뒤 미엘·태온005/006·무음Player·Quest·Save/Next·월드모달·AudioSource.Stopped 경계.
- Main21 이후 초보 마을 펫 NPC **완료 뒤만** 분양/관리 각1, 아르벨 기존 펫 NPC 중복0, 사수 전용/세린 같은 사수 NPC가 사용 가능, 인근 동선 보호.
- Chapter3 Main22~24 설계는 `STORY_DRAFT` **신규 Unity Scene/퀘스트/몬스터/보스 구현 지시가 아니다**. Main21 13목표 변경/Chapter3 Scene 구현 금지.
- 기존 애셋/아트/TTS·BGM/SFX 및 Player 원본·SaveVersion·Dirty·Unity Editor Play 포커스 보존.
- QA는 isolated worktree/별도 폴더, **백그라운드 무포커스 전환** 방식. 기존 URP/Search 패키지 에러는 원본 대조 시에만 KNOWN/PARTIAL로 분류, 새로운 오류가 있으면 FAIL. 실사용 Play·스피커 실청취 등을 하지 않았으면 NOT_VERIFIED 그대로 남김.

## 3. 로컬 GitHub 동기화 — 사용자 PC 보호

1. **로컬 원본 사전 감사:** `git status --porcelain=v1 -uall`, `git branch -vv`, `git worktree list`, HEAD/SHA, 현재 Editor/Play 상태, 변경·미추적 파일 목록과 허용 경로별 해시를 백그라운드 기록. Git remote는 저장소 정본임을 확인. Unity Assets/Persistent UserData/Saves·Library·LibraryIgnore 제외 정책 확인.
2. 작업 중 기존 사용자 dirty·단독 에셋·게임Save를 **제거/덮어쓰기 금지**. `git reset --hard`, `git clean -fd(x)`, 강제 Push, 무단 `git stash --all`, 사용자 폴더 복구불가 이동 금지. 충돌 가능하면 별도 백업/격리 worktree에서 먼저 merge 시뮬레이션.
3. GitHub `main` 최종 SHA를 `git fetch origin --prune`로 받는다. 로컬 main/현재 checkout을 함부로 이동하기 전 active Unity 워크트리의 staged·unstaged·untracked와 변형된 Arbel Scene을 점검. 기존 로컬 Arbel 정식 적용 overlay와 원격 병합 Arbel 자산 SHA가 같으면 **같은 바이트 중복으로 재적용하지 않도록** 보호된 안전 절차 사용. 이미 변경된 파일과 충돌 시 사용자 dirty snapshot 백업 + 재현 가능한 비파괴 통합; 불확실하면 STOP하고 local sync PARTIAL이라고 정확히 보고.
4. **가능한 경우에만** 로컬 `main` fast-forward (`git merge --ff-only origin/main`) 및 사용자 작업 브랜치·worktree 설정 복원. `git branch -vv`, `git rev-parse main` vs `git rev-parse origin/main`, remote `main` 3자 SHA 정확 비교. GitHub `main`와 로컬 `main` 동일한 SHA까지 확인할 때만 **SYNC_COMPLETE**.
5. 안전 검증 실패나 저장·dirty conflict면 **로컬 안전 보존/부분 동기화 보고**로 종료. 잘못 덮어쓰는 것보다 중단이 우선.
6. 최종 보호 및 실행 결과를 `문서/00_프로젝트` QA 문서와 `CURRENT_STATUS`·`CODEX_LATEST`에 정확한 시점·브랜치·SHA·PASS/FAIL/PARTIAL과 OPEN 남긴다. 기존 사용자 작업파일을 마음대로 커밋해 사적 변경을 원격 업로드하지 않는다.

## 4. 결과 정의

- **DONE:** 오늘 승인·검증 완료된 변경이 main에 병합, 실제 원격/main/로컬 main SHA 동기화, 지정버그 회귀 PASS, 기존 사용자 데이터 보호 증거 PASS.
- **PARTIAL:** 미승인/미검증 TTS·UI/Editor 손검사·기존 Portal/tree 겹침/정본 Unity버전 미확인 등은 정확히 열거. 중요 실패를 PASS로 숨기지 않는다.
- **STOP:** 버그 복구 원음 없음/침묵 과다 수정에 발화 손상, 새 Runtime/Quest/Save/Collider FAIL, local dirty 충돌·비파괴 동기화 불가능, 승인되지 않은 대규모 파일 차이. 원본 안전 보호 및 보고.
- 보고 요약에는 **브랜치·Commit·PR·원격 main SHA·로컬 main SHA·origin/main SHA·QA 숫자·부족한 직접 검증·Open 이슈·다음 행동**을 명시한다.

**이 문서는 완료 보고가 아니라 Codex 실행·승인 경계 기록**이다. 기획 담당은 대표 PC에 직접 접근할 수 없다.
