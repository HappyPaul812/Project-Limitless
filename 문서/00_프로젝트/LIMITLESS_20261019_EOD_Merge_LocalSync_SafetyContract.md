# LIMITLESS / 2026-10-19 퇴근 전 통합·GitHub main 병합·로컬 원본 안전 동기화 계약

- 기획 담당: `docs/planning`.
- 사용자 명시 요청: "**오늘은 여기까지 하자. 오늘 작업한거 다 합치고 로컬 동기화 해줘.**"
- 현재 이 문서는 **통합 실행을 위한 안전 계약/인계만**. GitHub main PR 실제 병합, Codex 로컬 브랜치 Push, Windows `F:\study\codex\Project-Limitless` 원본 Git/Unity 동기화는 **아직 이 기획 세션에서 실행하지 않았다**. 실제 완료는 Codex가 SHA/원본/PR/로컬 HEAD·index를 검증한 이후에만 `SYNC_COMPLETE` 선언.
- 사용자 **통합 승인 범위**: QA 합격/대표 미술·청취 승인된 변경 + 정확한 문서화·인계. 사용자 명시 반려·미검증·미술 실패 자산을 제품에 붙이라는 허가가 아니다. 원본 `main`에 직접 commit 금지, `codex/*` feature → review/delta QA → PR → main 병합 순서. 기타 담당 브랜치도 필요한 문서만 선택 병합.

## 1. GitHub 원격 조사에서 확인한 기준 (2026-10-19 21시대)

- GitHub remote `main`, `origin/main` 기준 SHA **`675988c9f2c5c507d9a696b0378be27c0bd3309b`**, 마지막 PR #26. 열린 PR **0개**.
- remote `docs/planning` **`87ee095d467b3abea814614c129d18dabbf9f249`**(본 EOD 추가 전), remote `docs/art-renewal` **`bda286997e8fa4fc08f8f265df6977f79a094aab`**, `docs/tts` **`118c60f645dba24b8c89d4154393581be5b8611d`**. `docs/art-new`는 예전 작업2커밋·이날 실제 신규 이미지 납품 내용 아님.
- remote 비교: `docs/planning` main 대비 ahead61/behind83, 변경 파일26; `docs/art-renewal` ahead37/behind185, 변경 파일19; `docs/tts` ahead43/behind185. **서로 오래 갈라져 있으므로 branch 전체 자동 merge 금지**. `docs/art-renewal`에는 **Unity 시각 QA 실패한 `Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip`** 바이너리가 포함돼 있으므로 **미술승인 실패 ZIP을 main에 가져오지 않는다**.
- **오늘 Codex 로컬 기능 브랜치들은 현재 GitHub 원격 branch list에 없음**. 로컬 worktree와 원본 프로젝트에 변경이 있으며, 이 GitHub-only 세션은 사용자 Windows F: 드라이브에 실행 권한이 없다. Codex가 실제 local commit SHA와 원격 main의 선후관계를 직접 조사해야 한다.

## 2. 제품/문서별 '오늘 통합 대상' 상태

**게임/원본 디스크 적용 완료라고 Codex가 보고한 변경 — local SHA·diff 검증 후 PR 통합**
1. Main09 대사/음성 27개 재회순서 및 PCM 복구: 사용자 전부 청취 승인, 격리550 PASS + 연결 추가261 PASS; feature `codex/main09-dialogue-voice-fix-20261019`, 최종 보고 `6e1a93c60df5e6e7f7fa88bc2e50f75f7425e968`(Codex 캡처 근거; local 실제 commit lookup 필수). 원본 WAV 소스는 보호된 별도 복구본, 이전 main에 이미 병합된 Main09 Quest UX PR21/24와 중복 반영하지 말 것.
2. Main10 조사점 NPC 입력 우선권 + 승인 기존 PCM 음성 18개: 사용자 18/18 청취승인, 격리 입력 132PASS, 음성 연결 164PASS. `codex/main10-voice-quest-interaction-fix-20261019` 최종 보고 `6de449ab1cde09bbf20db003ecc64d1e1ca18f0` (local commit lookup 필수), 원본 디스크 적용 완료. Main09 음성27 보호.
3. Field06 v1.1 B42: 환경 미술 사용자 승인, `codex/field06-v11-unity-integration-20261019` 최종 `0202da42fc2def05d18bcc1781669d02a7b6ba02`, Root Spawn Collider 안전 보정으로 Arbel 도착 약0.455~0.47 오차 해소, Ground49 빈틈 보호, 기존 합격 delta QA124PASS/0FAIL을 Field07과 공동 재사용. 원본디스크 적용 완료 보고.
4. Field07 v1 B42: 환경 미술 사용자 승인, `codex/field07-v1-unity-integration-20261019` 최종 `c5453d874f68fbc15d9da64564cc362c92e7e828`, 기존 투명 먼지로 Runtime Ash7 사각형 제거, 선택 PNG 미사용. Field06/07 공동 delta124PASS/0FAIL, 원본 디스크 적용 완료 보고.
5. Field08: 공식 아트 재제작/코드 변경 없음. 기존 연결 QA 결과 보존, 변경0.
6. Main18 기존 세린 10 음성 및 Field05 B42, Main09 Quest UX, 2026-10-19 문서5 의미 병합은 **이미 PR #18/19/21/22/23/24/25/26으로 remote main에 병합된 부분을 중복 cherry-pick 하지 말 것**.

**제품 통합 제외/보류, 단 보류의 원인·증거·선행 담당 기록은 docs에 보존**
1. Field09 ObsidianScar v1 최소 Fix: 정상 ZIP 재포장 성공, 격리 기술 40PASS/0FAIL + Main18 delta92PASS/0FAIL. 그러나 **시각 미술 FAIL**: 지면 회색 반복띠6개와 소품6종 흰 가이드선 실제 남음. **신규 후보 PNG/ZIP 공식 Scene 적용 및 main 병합 절대 금지**. 해당 실패 증거/재포장 ZIP은 Codex 증거·로컬 아카이브로 보관하고 `main`에는 필요 시 **보고서만** 포함. `codex/field09-minimal-fix-unity-integration-20261019` QA 최종 `68a9d83de2d449e181078a7a1e3d9f74ce88ffac` 로컬 조회. `docs/art-renewal`의 손상 v1 ZIP도 통합 제외.
2. VenomSnake/Wolf v1은 대표 미술 반려, v2는 Chapter2 공식 아트 대비 미술 승인 HOLD. v3 KeyArt 뱀/늑대 개별 PNG2 생성은 리뉴얼 세션 sandbox에만 존재했다는 담당 보고, **GitHub/Windows 원본 미업로드 + 대표 미술 승인 전**. **v1/v2/v3 이미지·애니메이션·시트는 official Runtime 통합0**. 문서에는 정확한 상태와 후속 작업만 보존, 배포 불가 PNG를 invented-path로 main에 만들지 말 것.
3. Main20/21 추가 TTS 22개 미등록(기존 17후보 및 5미존재, 9음색 오류 등), TTS 담당 `docs/tts`의 관련 큐·계획 문서는 히스토리/보류로 보존하되 미승인 WAV 생성·적용 금지.
4. Chapter3 Main22+ 북부 신규 콘텐츠/서비스는 기획 수립·승인 게이트를 분리. 구현 완료로 승격하지 않음. `docs/art-new` 신규 아트 콘셉트는 별도 담당 선행안, 사용자 최종 미술 승인 없이 official Unity Import 금지.
5. Unity 원본 Editor 사용자 실제 Play, Main15/16 전체 스토리 전주행, 기존 Search/URP 환경오류는 **NOT_VERIFIED / OPEN** 상태 유지. AI 격리 PASS를 사용자 직접 손플레이 PASS로 선언하지 않는다.

## 3. Codex 실행 절차 — 완료 전 어떤 위험한 명령도 선행하지 말 것

### A. 원본 보호 및 inventory (읽기 전용 우선)
- `AGENTS.md`, `CURRENT_STATUS.md`, Codex latest, 이 EOD, 팀 공통 규칙. 사용자 F: 원본 프로젝트 & 별도 worktree의 Git branch/ref/HEAD/log/status, `git worktree list --porcelain`, upstream, remotes, root commit ancestry와 `git diff --name-status`를 확인한다.
- **현재 dirty/tracked/untracked/Unity UserData(최소 이전23개), Save/Settings, 제작 출력·WAV·아트 ZIP, Docs5 고유 사용자 이력**를 전수 manifest/SHA256 로컬 별도 보호 위치에 백업(기존 `Temp/Doc5Recovery20261019` baseline 및 사용자가 최근 추가한 파일 모두). staged 0이라 단정하지 말고 실제 index를 기록. `git reset --hard`, `git clean`, 무단 `stash`, `force push`, 로컬 main `rebase`, `checkout`으로 원본 Editor 활성 트리 교체 모두 금지.
- 별도 **안전한 integration worktree** `codex/eod-integration-20261019`를 실제 원격 최신 main에서 생성하고 원본 사용자 작업트리는 보존한다. 필요한 기능 commit/부모 ancestry 검증 후 해당 worktree에서 선택 `cherry-pick`/merge. 로컬 브랜치에 커밋 파일이 없으면 바로 `MISSING_LOCAL_COMMIT` 보고, 원본 dirty를 강제 스테이징해 복원하지 말 것.

### B. 중복/충돌 없는 승인 feature 통합
- Main09 음성, Main10 조사·음성, Field06 B42, Field07 B42 순서로 최소 **기능 커밋** 선정·적용. 각 기존 브랜치의 모든 QA 생성물/백업/WAV 원본 보호용 파일을 자동으로 main에 포함하지 말 것. 해당 기능 내 승인된 recovered WAV와 `.meta`/StoryVoiceCatalog/Scene 참조는 반드시 함께 가져와 음성 파일만 누락하는 사태 금지.
- 최근 원본 디스크에 적용한 승인 변경이 원격 main 기능 파일과 정확히 같거나 별도 staged/diff로 존재한다면 **해시·기능 단위 별 중복 여부/원본 unchanged 여부** 판단 후 선택한다. 각 기능 branch의 사용자 미완료 파일을 덮어쓸 수 없다.
- `docs/planning`, `docs/art-renewal`, `docs/tts`는 HEAD들을 통째로 cherry-pick/merge하지 말고 최신 remote main과 **per-file 비교로 문서 중복·내용충돌 의미 병합**. `docs/art-renewal` 손상 Field09 v1 ZIP/미승인 Art Asset은 **통합금지**, 현행 정확한 보류 상태의 문서와 인계만 통합; `PLANNING_LATEST`, `WORLD_ART_RENEWAL_LATEST`, `TTS_LATEST`, `CURRENT_STATUS`에서 모순된 과거 기록은 HISTORY로 보존하고 최상단 최신 상태만 정리. 원본 문서 개별2 커밋 이력 보존.
- GitHub diff/CI 확인을 위한 소수 Delta QA (참조/컴파일/씬·매핑/음성 SHA/Quest 회귀)만. 기존 550,261,132,164,124,40,92,500 및 수백 QA 전부 재실행 금지. 미술 FAIL과 기존 Search/URP/Editor 사용자 손플레이 미확인은 FAIL/OPEN으로 별도 기록.

### C. PR을 통한 원격 main 갱신 (사용자 오늘 통합 요청에 따른 승인 범위)
- `codex/eod-integration-20261019` 로컬 기능+문서 통합 커밋 완성, local SHA/보호 Manifest 기록, GitHub feature branch **일반 Push만**(명시적으로 오늘 통합을 요청받은 통합 브랜치와 필요한 승인지원 브랜치에 한해; main 직접 Push/force Push 금지). PR `codex/eod-integration-20261019` → `main` 생성, changed-files 및 기존 승인/보류 자산 정확히 확인.
- CI/검증 및 mergeable 상태 합격 후 **PR 방식으로만 main 병합**; 사용자 오늘 "다 합치고"는 승인된 범위 PR merge 요청으로 해석하되 실패·충돌·사용자 자산 위험이 있으면 merge 보류하고 정확한 한 개의 blocker만 보고.
- PR merge SHA+병합 파일 목록·누락0·보호 예외0를 기록. 안전하지 않은 branch raw merge 또는 기존 리뉴얼 미술FAIL v1 ZIP 자동 포함 금지.

### D. Windows 원본 local main 동기화
- 새 원격 main을 fetch한 후 **원본 root worktree와 사용자 고유 로컬 commit·dirty·untracked·index를 훼손하지 않는 방식**으로 로컬 main을 안전하게 동기화. 원본 tree가 dirty이므로 상황에 따라 별도 clean integration worktree를 기본 사용하고, 원본 HEAD를 강제 바꾸지 말 것.
- `local main == origin/main == remote main`이라는 조건은 실제 원본 local HEAD·index/allowed dirty 검사까지 완료했을 때만 `SYNC_COMPLETE`. dirty 사용자 파일을 덮어쓸 우려가 있다면 **SYNC_SAFE_BLOCKED**로 보고하고 사용자 파일 변경없이 clean worktree HEAD만 sync하되 완료라고 말하지 말 것.
- 보호: 이전 tracked dirty308/untracked548 및 UserData23은 2026-10-19 오전 baseline이지 지금 수치 고정 아님; 실제 최신 값으로 교체/전후 SHA 검사. 프로젝트 약16GB·Unity Library/Temp 대용량 이동·삭제 금지. Editor Play/포커스/Refresh 조작 금지. 원본의 실제 Editor 자연 Import는 사용자 요청/직접 손플레이 이후.
- 최소 증거 `BackupManifest.json`, `ChangedFilesAllowlist.json`, `FeatureCommitMap.csv`, `IntegrationQA.json`, `PR_Merge_Closeout.md`, `OriginalLocalSync_Proof.json` 와 `CURRENT_STATUS.md`/Codex latest, 각 PR URL/헤드·merge SHA. 각 스텝 `NOT_STARTED`/`PASS`/`BLOCKED` 구분.

## 4. 마감 보고 형식

표로 Main09/Main10/Field06/Field07/Field08/Field09/몬스터·펫/TTS/Chapter3 및 docs 담당, GitHub PR/main merge, original local HEAD/upstream, dirty/untracked/UserData SHA, QA 재사용 vs delta, 사용자 미술승인과 다음날 BLOCKER를 구분한다. **PR merge+local sync 실증이 둘 다 완료되기 전에는 '오늘 작업 다 합침/원본 동기화 완료'라고 주장하지 않는다.**
