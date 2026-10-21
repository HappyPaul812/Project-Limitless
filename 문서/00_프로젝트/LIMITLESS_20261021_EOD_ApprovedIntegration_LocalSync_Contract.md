# LIMITLESS — 2026-10-21 EOD 승인분 선별 통합·원본 로컬 동기화 실행 계약

- 작성: 기획 `docs/planning` · 기준 원격 `main` `e4e9891f6f01ea95cada9e63b1f09946d94d6041` (2026-10-21 확인 시점), 열린 PR 0. **Codex는 실행 직전 재조회**.
- 바울 대표 최종 지시: **“오늘은 여기까지 하자. 오늘 한 작업들 다 합치고 로컬 동기화도 해줘.”**
- 상태: **`USER_EOD_INTEGRATION_AUTHORIZED / CODEX_EXECUTION_REQUIRED / MAIN_NOT_YET_MERGED / LOCAL_SYNC_NOT_YET_VERIFIED`**.
- 이 문서는 **마감 실행 권한과 안전 경계**이지 수행 완료 보고가 아니다. 이 기획 담당은 사용자 Windows `F:\study\codex\Project-Limitless`의 원본 저장소·Unity Editor에 직접 접근하지 못한다. 실제 파일 병합/비파괴 동기화는 Codex가 수행하고 증거를 남겨야 한다.
- 원칙: **당일 승인·QA를 통과한 제품 기능과 정확한 문서 정본만 선별 통합**. 대화상 긍정 피드백을 미완성 음성·마을 미술 등 다른 작업의 승인으로 확대하지 않는다. 외부 사용자 문서·고유 로컬 파일/커밋·Save·UserData·Settings를 무단 공개/덮어쓰기 금지.

## 1. 제품 통합 allowlist와 우선순위

### A. Wolf Run6 v5 — 승인+원본 디스크 적용 완료, GitHub 정식 통합 대상

- 사용자 **미술/모션 승인** + 본게임 적용 지시 완료. 승인 ZIP `F:\Downloads\Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5.zip`, SHA256 `16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351`.
- 작업브랜치 `codex/wolf-run6-v5-apply-20261021`, 최신 **로컬** 커밋 `f0dd59dd010759760811a7a1b516eab3969d500c`. 이전 기능 연결 커밋 `ab1b47ff9d46f4ed5883b85b73e87ca2036c83e6` — 두 커밋 계보와 실제 제품 diff 확인.
- 승인 게임변경 **6파일**(새 Run PNG와 .meta 2, 기존 C#4). 원본 사용자 PC Unity 디스크 이미 6/6 SHA 일치 적용됐으며 기존 4파일 복구백업 확보. 격리 Unity **106 PASS/0 FAIL** 재사용, 추가 최소 Import/compile/Resources 검증 PASS. 원본 Editor 자연 Import·실 손플레이는 미검증.
- **통합은 기존6파일만**, 다른 늑대 실물·KeyArt·아이콘·기존 GUID/meta·Animator/Controller·Bear/Fox·다른 클래스·Save/Quest/Chapter3 마을 변경 금지. 원본 디스크에 이미 있는 동일 바이트를 **재복사하지 말 것**.
- 코드 PR에 문서/QA를 필요한 범위로 한정. 별도 깨끗한 통합 브랜치로 올리고 diff·QA·보호검증 후 승인된 이 EOD 범위로 PR 병합.

### B. Main22 + F1 + 서리들쥐 — 첫 플레이 기능의 선별 통합 후보

- 바울 대표가 첫 플레이 보고에 **“잘된다”** 긍정 의견. 이번 EOD는 오늘 진행된 **승인·격리 검증 완료분**의 안전 통합 권한이지만, 최종 정식 스토리 텍스트·경제·보상·TTS를 확정하는 승인으로 확대하지 말 것.
- Main22 첫 플레이 `codex/chapter3-main22-first-playable-20261021`, commit `be7f64a3ff48a8848471166d4aefc4969080bd8b`, **136 PASS/0 FAIL**; 본편 안전 적용 준비 `codex/chapter3-main22-apply-preparation-20261021`, commit `2631e8750df8d98fb298754f3c82c5ca651c2fda` (실제 로컬 브랜치/커밋 검증 필요).
- F1 승인 PNG6 격리 Unity **64 PASS**, 서리들쥐 전투 Fixv2 바울 대표 미술 승인·Battle **40 PASS** 기존 QA 재사용. Main21→북문 명시 출발→F1 5목표·서리들쥐1기 지정전투→안전쉼터, Save/Continue·도보/휠체어·도주/패배 재도전 기능만 선별.
- Codex의 기존 적용준비 문서에 “전체 583 경로 분석/이미 포함13·신규40·수정5” 등 **합계 해석이 다른 수치**가 있으므로 **실제 파일 allowlist와 scene/meta/GUID/quest ID diff**로 확인해야 한다. 비인가 Scene 덮어쓰기·기존 북문/Save 파손 위험이 있으면 Main22 부분은 **별도 보류**하고 Wolf+정본 문서 통합을 계속한다. 기존 136/64/40 전수 QA를 다시 반복하지 말고 변경분과 충돌만 최소 검증.
- 초기 구현의 임시 대사/무보상/미정 EXP·드롭·아이콘·TTS·경제는 **`PROTOTYPE / DESIGN_NOT_LOCKED`**로 구분. 준비된 플레이어블을 런타임에 연결하더라도 최종 Main22 스크립트나 TTS 승인/실제 QA 합격으로 주장 금지. 안전조건 불만족시 이 기능은 다음날로 이월하며 `EOD_PARTIAL` 보고.

### C. 문서 선별 통합

- `docs/planning`: 10/21 Main22·Main23 Story, Quest/ID, Wolf QA/승인, Village v5 HOLD, Main24 skill persistence #46 추적 및 본 EOD 계약/인계, HISTORY 보존.
- `docs/art-new`: 승인 F1 PNG6/서리들쥐 자산 QA 문서, Village v5 **기술PASS·미술HOLD/OPEN16 미해결** 기록, 기존 납품 본문/해시 보존. 바이너리 파일을 GitHub에 실제 추가했다고 주장 금지.
- `docs/art-renewal`: 승인 Wolf Run6 v5/리뉴얼 진행 문서. 오래된 몬스터 v3 HOLD나 과거 반려 자산을 main 제품으로 섞지 않음.
- `docs/tts`: 10/21 사용자 요청 **재사용 가능한 다중 프로젝트·할당량·429 전환·HOLD/체크포인트 TTS 운영 표준** 문서와 정직한 제작 현황. READY **세린63/67 + 태온1/85=64/152**, 당일 PT 39/40·PROJECT_3 격리·HOLD=False; 청취·Unity 승인 아님. API 키/Project ID·로그의 개인 경로·비밀 문서 유출 금지.
- `docs/bgm,docs/sfx,docs/video` 등은 신규 확정 문서가 있을 때만 실제 승인·저작권·QA 상태를 유지해 선별, 외부 ZIP/MP3/WAV를 무단 게임 적용하지 않음.
- 다른 담당 브랜치를 통째로 merge하지 말고 `main`과 개별 문서/기존 history semantic diff로 안전 선별·충돌시 기록 보존. 10/20까지 이미 main에 병합된 Field09·Main09/10·Field05/06/07·Main18 승인 TTS 등을 중복 적용하지 않는다.

## 2. 통합 제외·별도 보관 (절대 메인 런타임으로 승격 금지)

- **Main23 FrostwayVillage GameArt v5**: ZIP 실 SHA `1affc1a4c23f3bcc6a9e9e18924b08bce3134eab065d8362757725f96e7f71b4`, CRC/Manifest/Seam 기술PASS/실 Unity 통행 proxy12/12 PASS이나 **Earth·ThinFrost·Gravel 타일 3종 미술 HOLD**. 이전 OPEN16 원본 CSV·좌표 JSON은 Codex가 확보했으나 **16건 해결0**. 미술 승인·실제 Main23 Scene/Quest 구현/Unity 본편 import 없음. 후보 ZIP/QA 근거만 비파괴 보관, **상품/자산 전체 승인 아님**. Tile variant 파일럿2종/지형마다 추가안은 **제안**일 뿐 제작/반영 승인 아님.
- **Wolf 관련 미승인 별도 동작·독사·Bear/Fox 신규 모델**과 기존 FAIL 소재, Field09 HeatVent/Spire 등 잔여 품질 보류자산.
- **Main24 스킬 6슬롯 구매·강화·장착 Save persistence 이슈 #46**(구버전 세이브 migration 미구현), Main23 첫 SafeZone·NPC/Quest 실제 구축도 미실행. 오늘 EOD로 신규 구현 명령 추가하지 말 것.
- **TTS 세린·태온 재캐스팅 신규 WAV**, 미승인 하렌·기타 화자, BGM 샘플 및 SFX/영상 미완, 신규 API 사용/자동 생성/자동 청취/게임 교체 금지. 백그라운드 TTS 프로세스·잠금/장부 상태는 관찰만, 안전 종료 요청 없는 임의 중단/재개 금지.
- 과거 `main` 반영 완료된 일일 작업을 다시 import/Cherry-pick하지 않는다.

## 3. Codex 실행 프로토콜 — 사용자 원본 비파괴·실제 EOD 완료증명

1. 시작: `AGENTS.md`, `CURRENT_STATUS.md`, `CODEX_WORKFLOW.md`, `COMMON_RULES.md`, `CODEX_LATEST.md`, `PLANNING_LATEST.md`, 이 계약과 `LIMITLESS_20261020_EOD_ApprovedMerge_OriginalLocalSync_SafetyContract.md`, 어제 `OriginalLocalSync_Proof.json` 확인.
2. 사용자 원본 `F:\study\codex\Project-Limitless`에 대해 Git HEAD·`origin/main`·index/stage·tracked dirty·untracked·ignored·worktree·editor session 상태를 **읽기 전용으로 사전기록**. 원본 Save/UserData/Personal Settings/사용자 Scene PNG/WAV·승인 Wolf 디스크6 파일 **사전 SHA** 및 보호 파일 manifest/충돌 파일 목록을 확보한다. 원본에 새 커밋/checkout/rebase/stash/clean/reset/Play/Editor foreground 변경 금지.
3. 원본 외 `Temp/EOD20261021/` 내 안전 공간에 기존 소중한 dirty/untracked/설정과 중복 경로를 복원 가능한 방식으로 **SHA256 backup**. 기존 원본의 보호 인벤토리 수치는 10/20 기록(당시 dirty308/untracked548/staged0)에 불과하고, 오늘 값은 **반드시 새로 실측**한다. `UserData/Saves`, `.git` 및 API 비밀값을 원격 PR에 올리지 않는다.
4. 최신 remote main과 승인된 후보 로컬 커밋의 변경 목록 비교 → **allowlist 파일·SHA·이미 디스크 적용된 Wolf6 중복 여부 확인**. 새 `codex/eod-integration-20261021` **격리 worktree**에서 선택 변경을 적용. Main22가 안전조건 미충족이면 분리해 보류하여 Wolf 통합을 막지 않는다.
5. 수정 경로 중심 `git diff --check`, Unity 컴파일/참조/후속delta QA(가능하면 batch/noninteractive), Scene/Quest/Save compatibility/기존 Wolf loop 변경분 확인. **기존106/136/64/40PASS 반복0**, 새 기능 영향에 한해서만 검증. 기존 URP/Search 환경 결함과 새로운 FAIL 분리. 테스트 통과 범위의 결과만 PASS; Editor 자연 Import·사용자 손플레이는 미검증으로 기재.
6. 통합 후보 branch에서 `main` 최신 기반 기능 PR 준비/원격 Push 후 PR create, 실제 PR diff·승인 범위·비인가 파일0·중복 main 변경0 확인. 이번 사용자의 마감 통합 승인에 따라 **선별 PASS 결과만 PR 방식으로 병합**; 충돌·테스트 FAIL/권리 불명·GitHub 오류·보호 위반이면 해당 범위만 `BLOCKED`로 중단. **main 직접 commit/Push, force push, force-merge 금지**.
7. 최종 merged GitHub main SHA 재조회 → 사용자 원본 local/main의 ancestry/dirty overlap/승인된 원본 제품 파일 SHA와 비교. 원본 디스크에 이미 적용된 Wolf6은 **재복사/덮어쓰기하지 않음**. clean worktree에서 먼저 동기화 리허설·rollback 가능성을 확인, **원본 사용자 dirty/untracked/index/Save/Settings/Editor/미사용 고유 커밋 완전보호가 증명될 때만** 실제 로컬 `main`을 safe fast-forward로 최신 원격과 일치시킨다. 겹침 충돌/dirty로 안전 동기화 불가하면 원본 main을 강제로 옮기지 않고 `SYNC_SAFE_BLOCKED` 혹은 `LOCAL_SYNC_PARTIAL`로 보고.
8. 이후 GitHub `main` / `origin/main` / 사용자 원본 `main` 의 3자 SHA, index=HEAD/staged0, 사용자 dirty/untracked 보호목록 및 SHA불일치0, Save/UserData·Settings·사용자 고유 파일·브랜치 보존을 재검증. 기존 Editor scene/play/focus 무변경. 이전 SHA백업·변경분 allowlist와 증거를 `Temp/EOD20261021/`에 보존.
9. `CURRENT_STATUS.md`, `CODEX_LATEST.md`, 관련 EOD QA 문서/PR URL/mergeSHA와 기획·담당 인계는 **실제 완료한 결과만** 추가로 기록(역사 삭제 금지). 종료 시 원본 Unity 에디터를 강제 열거나 play하지 않는다. 안전하게 불가능하면 복구가능한 상태로 즉시 STOP하고 사용자에게 다음날 보고할 정확한 blocker/경로/변경0을 기록. 사용자에게 밤중 추가 결정을 요구하지 않는다.

## 4. 최종 Codex 산출 및 판정

- `Temp/EOD20261021/BackupManifest.json`, `SelectedChangesAllowlist.json`, `FeatureCommitMap.csv`, `IntegrationDeltaQA.json`, `UserOriginalProtectionProof.json`, `PRMergeCloseout.md`, `OriginalLocalSync_Proof.json` (파일명은 기존 runbook과 맞춤).
- 필수 보고: 통합한 기능/문서 및 **제외한 HOLD**, 기존102? 등 임의 PASS숫자 합산금지; 각 기능 기존 검증 PASS 재사용 여부/이번 추가 검사 수, Source↔SHA/기존파일 비인가 변경0, branch/commit/PR, 최종 GitHub main/head·origin main·원본 local main, dirty/untracked/staged 사전·사후, 백업SHA mismatch, Save·Editor 보호, 실제 원본 Import/실사용 미검증.
- 완료 정의:
  - `SYNC_COMPLETE`: **GitHub main = origin/main = 사용자 원본 로컬 main**, index·제품경로·승인 범위 실제 동일성과 사용자 보호SHA 불일치0을 동시 입증.
  - `REMOTE_MERGED_LOCAL_SYNC_PARTIAL`: PR 머지는 확인됐으나 원본 로컬 안전한 main ref/index 동기화 미달.
  - `PARTIAL_APPROVED_SCOPE`: Wolf 등 안전범위만 반영, Main22 후보는 아직 기술·기획 계약 Gate로 보류.
  - `SYNC_SAFE_BLOCKED`: 안전장치 충돌로 실행 중단 및 증거 보존.
  - 정확한 비교증거 없이 `SYNC_COMPLETE`나 `0 FAIL`을 단정하지 않는다.
