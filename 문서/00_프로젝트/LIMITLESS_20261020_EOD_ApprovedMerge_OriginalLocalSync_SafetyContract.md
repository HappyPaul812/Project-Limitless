# LIMITLESS — 2026-10-20 업무 마감 / 승인 분리 통합·main PR·원본 로컬 동기화 안전 계약

- 작성: 기획 `docs/planning`
- 바울 대표 현재 명시 요청: **“오늘은 그만하자 오늘 작업들 합치고 료컬 동기화.”**
- 원본 사용자 PC 작업 경로는 기존 `F:\study\codex\Project-Limitless`, 실제 다운로드 신규 ZIP은 `F:\Downloads\...`. GitHub 커넥터를 통한 기획세션은 **Windows F: 드라이브/Unity 원본을 직접 쓰거나 Play를 조작할 수 없음**. Codex만 최종 PR 병합/원본 보호·동기화 실증 가능.
- 본 문서는 **마감 실행 지시 및 선별통합 계약**, 실제 완료 증명 아님. `SYNC_COMPLETE`는 Codex의 최종 SHA/백업·원본 검증증거가 있어야 선언.
- 사용자 발화는 **이미 승인되고 기술 검증된 범위의 PR 방식 main 통합 및 안전 로컬 동기화 승인**으로 해석. **미검증/반려 PNG를 승인**하거나 별도 Main22 Runtime 구현·TTS·음원/아트 자동 Import·전체 Chapter3 제작 승인으로 해석 금지.

## 0. 현재 원격 출발점 / 충돌 분류

2026-10-20 GitHub 현재 확인:
- remote `main` HEAD **`12c69c72f9c09f716ee6242418b1d62f83a4a0d2`**, 2026-10-19 PR #27/28 EOD sync 최종 SHA. 열린 PR 0건. 오늘 신규 변경은 `main`에 없음.
- `docs/planning` 최신 `PLANNING_LATEST.md`는 이 EOD 계약 추가 전 최상단 서리들쥐 전투 v1 미술FAIL/표적 수정. `docs/art-new` 최신 commit `ed439bd41bfb` v2 납품·인계, `docs/art-renewal` 최신 `ef2ada264a84`, `docs/bgm` 최신 `e44aee0ecd8b`. 실제 사용 전에 Codex가 remote refs 다시 조회해야 함.
- 해당 담당 문서 브랜치는 오래된 `main`에서 분기되어 오늘 자산뿐 아니라 과거 차이를 포함함. **담당 브랜치 전체 무심한 merge/cherry-pick 금지**. 의미 있는 변경을 파일/기능별 선별 병합.
- 어제 EOD `SYNC_COMPLETE`의 정확 보호 기준 main SHA `12c69c72...`, 원본 사용자 dirty tracked308/untracked548·staged0, SHA 4,339/Backup1,059/UserData23; **이 수치는 어제 결과**, 오늘 실제 수치는 Codex가 다시 실측해야 함. 원본 Editor/Play·실제 Save/Settings·Git status를 강제로 바꾸지 말 것.

## 1. 제품/Asset 승인·검수 상태표

### A. 원본 Unity 정식 통합을 수행할 수 있는 승인 후보: Field09 부분 집합

- 바울 대표 **Field09 Sample3 3종 시각 승인**: Base Ground v1 SHA `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203`; Cracked Overlay v2 SHA `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`; Obsidian Slab v1 SHA `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692`.
- Phase2 실제 Unity QA: Cliff Blocker v2 하단/접지 PASS SHA `ad995f3cc4914759d6c88fa47c80227cb1a7c5f1a9a549679e06472402fee9f1`; Fissure Edge v2 자체 외곽 PASS SHA `c3dc0c131c11fef72e4d210a1a43528f9882d8c50486e8da0bc15b6984ea145a`. Fissure와 기존 Glow 조합만 FAIL하였으나 아래 B 파생으로 해결.
- **B 파생 Glow 대표 승인**. 기존 공식 Glow PNG/.meta/GUID는 **그대로 보존**, **별도 새로운 B PNG + .meta/GUID** 생성, Field09 해당 Sprite Renderer 참조 1곳 변경. 새 PNG SHA **`64529ce564c05deda576f7bb151fa6426c22c006009b073c76a59d7b5775ab48`**. Codex `codex/field09-derived-glow-b-integration-20261020` commit **`77978aa1`** (승인5 정본)→**`913eceb7`** (B 참조 연결)→**`b04c42e7`** (검증 보고).
- 이미 **격리 Unity 39PASS/0FAIL**, 3 GameView 붉은띠/안내잔흔 0, 내부 열기·조사·Missing Sprite0/Collider·SaveContinue PASS, 기존 A/B 34PASS+과열 UI10PASS는 재실행하지 않고 재사용. **이 승인 패키지를 main PR 통합 1차 대상으로**, 게임 원본 Scene에 필요한 새 PNG·메타·참조만 도입하고 기존 공식 Glow·GUID는 건드리지 말 것.
- **Field09 전체 환경 100% 완료로 선언 금지**: 나머지 AshPatch KEEP 후보/HeatVent/Spire 등 제작·검수 아직 별도. 어제 시각FAIL `Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip` 및 v2/손상 ZIP·구형 Fissure/Glow/Cliff v1을 main에 넣지 말 것. Scene/Field08/기존 Main18 Quest·Heat/Collider 보호.

### B. Chapter3 신규 아트 — 승인 참조자산 정리, **런타임 전투 연결은 이번 EOD 제외**

- **미술 승인 1포즈 v2** 2종: Mouse SHA `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`; FrostTyrant SHA `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830`. ZIP `Limitless_Chapter3_ApprovedAB_1Pose_ArtRework_v2.zip` SHA `0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919`.
- **Idle v1 서리들쥐A/설빙의 폭군B** Codex 격리 Unity **14PASS/0FAIL**, 대표님 “좋은데” 긍정 후 전투 Sprite 우선 지시. Mouse Idle SHA `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2`; Tyrant Idle SHA `9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43`; ZIP `Limitless_Chapter3_FrostFieldMouse_FrostTyrant_Idle_v1.zip` SHA `6c01e55c147ab57147c767f43503553164197cc037a3834ecd2d15d10d96a23c`.
- **이번 통합**: 위 1포즈·Idle 등 승인/검증 실물을 hash 기반 독립 `Chapter3/ApprovedReference` 또는 실제 프로젝트 기존 관례의 **게임과 연결되지 않은 명확한 참조용 경로**에 보존/문서화 가능(기존 폴더 관례 조사 후 선택). 별도 공식 MonsterDefinition/Scene/Animator/Chapter3 전체 Runtime에 자산을 **연결하지 않는다**. 현재 GitHub에는 art-new 문서만 있고 ZIP 바이너리가 업로드된 것으로 주장하지 않는다; 사용자 실제 F:\Downloads ZIP 존재/해시 검증 후, 프로젝트에 비게임용 참조로 담는 것이 충돌/Unity 자동 Import 위험이면 PR에는 정확한 문서/해시만 선별 통합하고 실물 ZIP은 원본 외 별도 안전 로컬 아카이브에 보존. 이는 실행 판단보고에 명시.
- **오늘 마지막 v2 ZIP**: `F:\Downloads\Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2.zip`. `docs/art-new`의 SHA256 **`518d210a0e5b29c7214ad960c0baa3d23a88d13d4f789b5f7420982ce701bfe3`**, 20파일/10,784,249bytes, Attack PNG SHA `c83e35b5aebbce908b6025d5a071fafc463cfe301763f68806ecddb8bd4f576b`; Hit SHA `695ed7db1f794c3f71df1b058eb5e91f01de7ecb125369264f3624b36c9fd3c2`; Defeat SHA `3f6a52039b766c57a5367c7145e0234b68ac0a552940de1e88ffe8f2404f3395`. **이는 제작 담당의 ZIP/PNG 자체 오프라인 PASS 보고만 있고 Codex Unity 변경분 QA 및 사용자 최종 미술 승인은 없음. `QA_PENDING / ART_APPROVAL_PENDING` 보류**. 오프라인 CRC/SHA 빠른 확인 및 안전 원본 외 archive는 가능하나 **main 공식 게임 아트/Runtime import·연결과 '승인' 표기 금지**. 사용자 오늘 종료 원하므로 v2 시각 검수는 내일 우선 과제로 보존. 기존 v1 Attack FAIL/Hit FAIL/Defeat HOLD/색감FAIL은 History로 유지.
- F1·서릿길 마을 환경 Pilot PNG10 (512/256), 1차 캐릭터 시안 반려(만화 느낌), v1 pixel HOLD는 **게임 통합 제외**. 승인 후로 가정하여 넣지 않음. FrostTyrant Boss Idle은 검수된 참조 아트라도 실제 보스 기믹·Scene·Main22~29 구현/타 몬스터 양산0.

### C. BGM 2곡 — 구성 호평, Loop·권리·실물 QA 남음

- 사용자 발화 “bgm은 좋고”는 두 곡의 **작곡 방향/분위기 긍정**으로 기록. `Where_the_Grass_Bends.mp3` F1 SHA `2bbbe6fe02b46c391653a5409802dde819ba64b5b5be99b20bbeafb4ad9b0fca` (74.24초, 끝 약2.36초 무음); `Hearth_in_the_Low_Valley.mp3` 마을 SHA `a8cae73d15d276ad5cf25415ee13bce6e9761b331822f8dcb5928da9b87c772b` (72.62초, 시작/끝 무음).
- GitHub `docs/bgm` 최신 보고 `Chapter3_BGM_Pilot_2Track_Listening_QA_20261020.md`에는 원본 MP3 binary 0, 사용자 BGM 세션에서 업로드한 파일이었음. 파일 실제 존재/상업이용 근거/최종 루프 편집 검사 전 **main product Audio 또는 Unity Mixer/Scene 할당 금지**. 현재 **COMPOSITION_USER_POSITIVE / LOOP_EDIT_REQUIRED / AUDIO_RUNTIME_NOT_APPROVED**. 오늘은 BGM 보고와 적절한 계획/승인 기록만 main docs에 선별 병합, 실제 오디오는 안전한 후보 아카이브만 가능.

### D. 나머지 담당
- `docs/art-renewal`에는 리뉴얼 출고 이력·Phase2/Glow 기술 원인·SHA 및 승인 상태 문서만 선별 병합, 이전 FAILED minimal ZIP 및 반려 Monster/Wolf GIF/PNG는 main product 제외.
- `docs/planning` 오늘 Chapter3 Main22~29/F1/서릿길 마을 설계 문서·Team kickoff/기술 규격 및 Field09 이력/검증 정본 중 필요한 신뢰 가능한 문서만 선별, HISTORY 유지. 담당 헤더 최신 상태/old draft 구별.
- `docs/art-new` 제작 및 유효 QA 문서만 선별, 미술 승인/미완료 상태 정확히 유지.
- `docs/bgm` 오늘 2곡 준비/QA 보고서 및 최신 인계 문서만.
- `docs/tts,docs/sfx,docs/video` 신규 진행 사용자 보류. 지난날 main 승인 자산/Main09·Main10/Field06·Field07/Field08/Field05/Main18 등 재반영/재QA 금지.

## 2. Codex 실행 단계 — 무손실 통합 및 원본 로컬 동기화

**A. 안전 Inventory/Backup (절대 생략 금지)**
1. `AGENTS.md`, `CURRENT_STATUS.md`, `CODEX_WORKFLOW.md`, 공통 규칙·Codex/Planning 최신 인계, 본 문서, `20261019_EOD_Merge_LocalSync_SafetyContract`, 기존 `OriginalLocalSync_Proof.json` 확인.
2. 사용자 실제 원본 worktree/remote GitHub 및 모든 Codex local branches/commit 존재, `git status --porcelain=v2`의 tracked/untracked/staged/ignored 보호, Save/UserData/Settings/Unity Editor active 상태/현재 HEAD/index/main remote SHA를 READ-ONLY로 실측. 현재 dirty/untracked는 어제 수치로 가정하지 말 것. 보호영역  SHA256 manifest+백업을 `Temp/EOD20261020` 안전 경로(다른 worktree 디스크/프로젝트 세이브와 분리) 생성. 사용자 소스 전체/Library 약16GB 무의미 복사/삭제/새 Unity Import 피하기.
3. 재시작/Library refresh/원본 Play focus 금지, `git clean -fdx`, `reset --hard`, `restore`, 무단 `stash`, force Push, main 직접 commit·Push 절대 금지. 기존 사용자 고유 커밋·문서·바이너리/원본 Save 무조건 보호. 원본 사용자 디스크에 다른 사람 파일을 임의 덮어쓰기 금지.

**B. 원격 main PR 병합: 승인·격리 QA 통과분만**
4. 최신 `origin/main` 기준 별도 `codex/eod-integration-20261020` integration worktree/branch 준비, **각 Codex local commit 내용**을 승인5종+B용으로 hash/Scene reference 기반 검증 후 선택적으로 가져옴. Codex `codex/field09-derived-glow-b-integration-20261020`: commit `77978aa1`, `913eceb7`, `b04c42e7`는 보고된 local SHA; **실제 로컬 확인 후** 관련 PNG5+meta(+기존 보존)/B PNG+신규 .meta 및 참조 Scene diff와 필요한 QA 문서만 통합. 기존 공식 `ObsidianScar_Fissure_Glow.png` 및 .meta SHA/GUID는 불변, Unity Scene 기존 Main18·NPC/Heat/Collider/Save 기능 유지.
5. Chapter3 1포즈+Idle2는 승인 참조 아트 원본 해시를 안전하게 archiving or non-runtime repository path로 수집(Import 변경 유발하면 실물 Git commit 제외)하고 docs 상태만 main에 병합. 미검증 Combat v2 PNG3, F1/마을 미승인 환경, BGM Loop미수정 원음원, TTS·SFX·영상 미승인 binaries·Field09 실패ZIP 배제. 나머지 담당 branch `docs/planning`, `docs/art-new`, `docs/art-renewal`, `docs/bgm`는 branch 통째 merge 금지, 필요한 today documents/인계 text per-file로 의미 병합.
6. 변경파일 allowlist/duplicate check 및 최소 delta QA (Field09 이미39PASS, 필요 Scene/UUID/PNG ref/Renderer delta+기본 Main18/Save 영향만 검증, 기존 수백 QA 전수 반복 금지). Unity 환경 제약(CI Search/URP/Editor version), 사용자 원본 실제 손플레이 미검증은 OPEN. 통합 zip/QA 증거는 별도 자료 보존하고 PR에서 대용량 Temp/Backup/UserData/Library 등 불필요 파일 제외.
7. 오늘 바울 대표의 “다 합치고 로컬 동기화”는 **위 명시한 승인 완료 분류에 한해** 별도 통합 PR `codex/eod-integration-20261020 -> main` 생성·정확한 변경 파일, SHA, CI/QA/충돌0 점검 후 main PR merge까지 요청한 것으로 해석. **직접 main push 금지**, force merge·문서 충돌 덮어쓰기 금지. 문제 발견 시 PR merge를 중단/원인 보고. 문서-only PR 별도 분리 필요하면 해도 되나 종료시 final HEAD 정확히 보고.

**C. 원본 로컬 동기화**
8. merge 된 GitHub remote main 확정 SHA 확인 후 fetch. 원본 사용자 local main과 remote main의 ancestry, dirty·index·untracked **겹침을 정확히 비교**. 기존 EOD처럼 백업/충돌의미병합/원본고유 변경 보존이 확인될 때만 실제 원본 main HEAD/index를 안전하게 동기화. **원본 Editor 켜진 작업폴더에서 위험한 branch checkout/rebase/reset/clean/stash를 실행하지 말 것**. 불가하면 별도 clean integration worktree만 최신 main으로 동기화하고 원본은 건드리지 않으며 `SYNC_SAFE_BLOCKED`로 명시, **SYNC_COMPLETE 거짓 선언 금지**.
9. 동기화 이후 user-tracked dirty/untracked/Save/UserData/개인 settings SHA **보호0불일치** 확인; 백업manifest/allowlist/old↔new commit diff/remote main=head proof·Staged counts·GUI editor 상태 확인. 원본 자연 Unity Import/실제 손플레이는 **NOT_VERIFIED**가 정상 (강제 Play 금지).
10. 바울 대표에게 요약(아래 항목) 보고. **이 작업에서 추가 확인 질문으로 사용자 수면을 방해하지 말고**, 안전하게 가능한 승인분은 진행하되 보호 충돌 발생하면 무조건 STOP+BLOCKED+복원가능한 증거, 비인가 파일 변경0. 사람의 미술 승인 없는 v2 전투 스트립을 통과로 처리하지 말 것.

## 3. 완료 산출/보고

- `Temp/EOD20261020/BackupManifest.json`, `SelectedChangesAllowlist.json`, `FeatureCommitMap.csv`, `IntegrationDeltaQA.json`, `UserOriginalProtectionProof.json`, `PRMergeCloseout.md`, `OriginalLocalSync_Proof.json`와 PR URL/merge SHA, 현재 main/remote/local SHA, 사용자 dirty/staged/untracked before-after/Save/UserData mismatches0, merge한 Field09와 docs file list.
- `CURRENT_STATUS.md`, Codex latest 및 기획 latest 정본 최신 헤더, 날짜와 `MERGED / ARCHIVED_NOT_RUNTIME / QA_PENDING / BLOCKED` 분리.
- 정확한 결론: `SYNC_COMPLETE`는 **remote main=origin/main=사용자 원본 local main의 verified ref+index**, 승인 기능/diff 정확성+보호 파일 SHA 일치, Editor 보존, 실제 적용 필드 상태 명확함이 입증될 때만. 그렇지 않으면 `REMOTE_MERGED_LOCAL_SYNC_PARTIAL` 또는 `SYNC_SAFE_BLOCKED`.
