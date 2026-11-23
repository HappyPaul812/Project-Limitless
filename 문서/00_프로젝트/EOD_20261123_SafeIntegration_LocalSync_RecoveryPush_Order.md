# LIMITLESS / 2026-11-23 / 전체 작업 안전 통합 · 로컬 동기화 · 복구 Push 마감 지시

- 사용자 직접 승인: **“오늘은 여기까지 하자, 거의 13시간 했으면 할만큼 한거지 뭐, 지금까지 한 작업들 다 합치고 로컬 동기화, 복구 푸시까지 해줘.”**
- 실행 책임: **Codex 로컬 Windows Git + Unity**, 기획은 GitHub 지시서·승인 상태 인계만. 이 문서를 게시한 것만으로 실제 로컬 작업 완료라고 보고하지 않는다.
- 목표: 사용자 PC의 **원본 Unity 프로젝트**에 승인·검증된 2026-11-23 작업을 안전 통합하고 로컬 `main` 동기화, **새 원격 복구 참조**를 GitHub로 Push하며, 독립 내려받기·SHA 검증 후 오늘 종료.
- 안전 우선: 기존 사용자 미커밋/미추적/개인 Save·UserData·Discord/기밀·키 보존. `main` 및 원격 Push에 **승인 대기/HOLD 기능·시안·미완성 데이터 임의 투입 금지**. 사용자 재확인 없이 충돌·파괴성 작업 진행 금지.

## A. 오늘 근거 및 브랜치/커밋 후보

1. 직전 로컬 `main` 기록: `736df561adb408c0225ed9052c37f2d45b7e24f5` (2026-11-19 EOD #121). **이 SHA는 과거 시점**이며, 오늘 실제 원본 로컬 `main` HEAD와 파일 상태를 반드시 다시 읽는다.
2. 기존 원격 복구 ref `backup/2026-11-19-safe-checkpoint` SHA `f64082914e114096b6b9393263e7a36f7d829a55`는 **절대 덮어쓰거나 삭제하지 않기**.
3. 원격 `main`은 확인 시점 `bfd281545632877325f5d21e04cefb924042c465`로 로컬 최신 제품보다 과거. **원격 `main`에 직접 Push/force push 금지**; 복구 전용 새 브랜치만 Push한다.
4. **빙각 수호체 Unity 전투 연결** [#126](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7098552079), 브랜치 `codex/issue126-rime-sentinel-runtime-20261123`, 제품 `a2faf0b02c960ddc795cc3054ad4beebbeb49d5f`, 보고 `b2580729ac6c57f22195bef83094b256e96dca60`. 승인 PNG5 SHA PASS; 격리 Play 10 PASS/0 FAIL. Attack44 피해 1회·Hit/Idle·Defeat KO 마지막 셀 유지, Ember 회귀. **F5 공식 Scene/Encounter/유니크 AI/단회 Drop/Save/전체 승리 전환 HOLD**. 두 커밋의 실제 포함 관계·의존 커밋을 확인.
5. **Chapter3 승인 BGM 4곡 Intro+Loop WAV8 Unity 연결** [#31](https://github.com/HappyPaul812/Project-Limitless/issues/31#issuecomment-7099212296), [#127](https://github.com/HappyPaul812/Project-Limitless/issues/127#issuecomment-7099209695), 브랜치 `codex/ch3-approved-bgm4-runtime-20261123`, 제품 `c24f3bec02b0c4511032cfb26e7a7452efdaabea`, 보고 `0ad2d36907a2b7b5cbe3b7e63e0e2e7539e62fe2`; 격리 Play 24 PASS/0 FAIL. F1·서릿길 마을·F2 후보 배정, F3 마스터·Catalog 등록만. **F3 정식 씬 배정 HOLD, 실제 기기·전체 Scene·Save Continue·TTS 명료도 미검증**, 상용 출시 권리 `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING`.
6. **Chapter3 서리들쥐 MonsterDefinition** #122 브랜치 `codex/ch3-monster-definitions-20261123` 제품 `8706671145c59fdbb0aa4c1ba268edfcbe65ec86`, 보고 `04f47f94bf5b9f139d34d114fd698ba783f2406d`, 격리10 PASS, 1/6 등록. **다른5종/공식 Spawn·Drop/Save HOLD**. 독립 자산만/충돌 없음이 확인될 때에만 통합 대상; 불확실하면 이 브랜치를 보존하고 통합은 HOLD로 분리.
7. **리뉴얼 #124** ZIP `LIMITLESS_Issue124_StingerGel_Tint_TipOnly_20261123.zip` SHA `a62442885359d75abd556cca01f648e1c7d71058dcf44050391860eff9cb2a98`, 기획 ZIP/Manifest24/24 PASS. White Tint 방향만 권고, 독침 59픽셀 팁 후보 **대표 최종 미술 승인 전**. PNG 교체/`ItemDefinition` 틴트 변경 **본편 통합 금지**, 문서·원본 실물은 안전 보존.
8. **TTS #125** Main24 신규 Stage WAV10/10 제작·사용자 청취 PASS. 실제 Runtime DialogueID 0/10 매핑 미완. **원본 162 WAV 보호, Stage를 Unity Catalog에 자동 연결 금지**, 실제 Windows `F:\Downloads\Voice` 등에서 WAV/Manifest/원장 안전 보존 현황만 보고. 비공개 키·과금 자료 원격 공개 금지.
9. **기타 11/23 진행 사항**: 기획 `docs/planning` (EOD 전 HEAD `8f6dd04231136733021980994e98743bf58b9ba4`), 메인 신규 아트 `docs/art-new` (#126 Defeat4 승인), 리뉴얼 `docs/art-renewal` (#124), TTS `docs/tts`, BGM `docs/bgm` (#31/#127) 및 #123 F3 아트·#118 성장밸런스 등: **담당 브랜치 최신 SHA와 문서·이슈 댓글을 실측**. 담당 docs만 안전 통합 가능하나 제작 중 시안·미검증 제품 적용 금지. 추가 로컬 작업 브랜치/미커밋 변경을 놓치지 않도록 전체 `git worktree list`, `git branch -avv`, `git log --all --since=2026-11-19` 등으로 조사.

## B. 단 한 차례 안전 실행 순서 — 충돌 시 자동 중단/대기

### 1. 원본·브랜치·개인 파일 안전 감사
- **실제 사용자 원본 프로젝트 경로를 확인**하고 `git rev-parse --show-toplevel`, `git status --porcelain=v2 --untracked-files=all`, `git worktree list --porcelain`, `git branch -avv`, `git remote -v`, `git log --graph --all --oneline`으로 정확한 현재 상태를 캡처. 다른 창/프로세스가 로컬 원본을 사용 중인지 검사.
- 현재 원본 `main`의 SHA와 파일 변경량, 미커밋/미추적/ignored 보호 대상을 목록화하고 **사용자 파일을 삭제/초기화/자동 stash pop하지 말 것**. `git reset --hard`, `git clean -fdx`, `git checkout -f`, `git push --force` 금지.
- 현재 HEAD의 로컬 원본 안전 분기 `backup/pre-eod-2026-11-23-<unique>` 생성(기존 ref 있으면 덮어쓰기 금지). **Git 미추적·ignored·개인 Save는 HEAD 백업에 포함되지 않으므로** 별도 안전 사본이 가능한 경우에만 개인·기밀 제외 목록과 SHA 보관. 알 수 없는 미커밋 변경은 자동 통합·커밋 금지, 사용자 작업 범위와 충돌하면 HOLD.

### 2. 분리 통합 브랜치로 안전한 승인 작업만 통합
- `codex/eod-20261123-safe-integration` 별도 worktree를 실제 최신 로컬 `main` 기준으로 생성. 기존 브랜치의 공통 선행 커밋/merge-base 확인. **Blind merge/cherry-pick 금지**: #126·#31/#127의 제품/보고 커밋이 같은 공통기반을 쓰는지, 변경 파일·GUID/meta·Unity 버전·BGM/전투 서비스 상호 충돌 여부 확인. 이미 적용된 커밋 중복 적용 금지.
- 승인된 #126 **전투 표시/KO 애니메이션 코드·PNG5만**, #31/#127 **승인 BGM 4곡 8 WAV·DSP 서비스·Catalog 및 안전한 배정만** 포함. F3 없는 Scene은 Catalog까지만, F5 등록/드롭/스폰·단회 보상·Save 확장 금지. #122 1/6은 현재 원본과 호환되고 지시된 수치·아트 전부 맞을 때만 독립 등록, 나머지 5/6 HOLD.
- `docs/planning`, `docs/art-new`, `docs/art-renewal`, `docs/tts`, `docs/bgm`, Codex 관련 인계/근거 문서를 중복·경합 없는 경로별로 통합. 각 담당 최신 이슈와 실제 원격 docs branch를 원본으로 사용. 서로 다른 담당이 같은 `CURRENT_STATUS`/`CODEX_LATEST` 파일을 건드렸다면 **무조건 ours/theirs 선택 금지**; 최신 항목 전체가 살아있는지 읽고 보존. 충돌 시 해당 경로만 보류 후 남은 안전 작업을 지속.
- 승인 전 시안/테스트 샘플/Unity 분리 QA 파일이 제품 코드에 흘러들지 않게 검수. 개인 `UserData`, Save/설정, Discord, TTS 키·원장, 임시/캐시 `Library`, Unity Editor 내부 상태, LFS 미수신 pointer는 본편 변경/원격 복구 업로드 금지.

### 3. 통합 품질·회귀
- **Unity 6000.6.5f1** 버전 실제 `ProjectVersion.txt` 확인(오래된 원격 main의 구버전으로 되돌리기 금지), #85 HOLD 유지. 사용자 전경 Editor 포커스 변경 금지(백그라운드/비대화형 검증).
- 신규 통합 접점에 한해서 컴파일·Sprite GUID/meta·BGM 4곡 WAV 참조·Intro→Loop 전환, #126 KO 최종 프레임, 기존 Ch1·2 BGM/Ember·슬롯 Save 보호 회귀. 기존 #126 10PASS, BGM 24PASS, #122 10PASS 및 개별 미술 QA를 무리하게 전수 반복하지 말 것.
- 기존 `Editor Search` 초기화 예외는 분리 보고. 새 통합 자체가 실패하면 원본 main으로 진행하지 말고 안전 브랜치에서 HOLD 기록.

### 4. 원본 로컬 main FF 동기화
- 통합 내용·QA가 안전할 때만 `codex/eod-20261123-safe-integration`에 EOD 커밋. 원본 로컬 `main`의 초기 HEAD가 그대로인지 재검사하여 원본 작업 트리를 보존하는 **fast-forward only** 동기화를 실행. `main`에 직접 작업 커밋/임의 merge commit 생성·강제 이동 금지. 로컬 원본 미커밋/미추적 상태로 안전 FF 불가능하면 **local main 미동기화(HOLD)**를 보고하고 통합 브랜치·복구 Push만 계속하는 방식으로 손실을 피한다.
- 로컬 `main` 또는 분리 통합 브랜치의 결과 SHA 및 `git diff --name-status`를 보존, 승인 자산 SHA 대조.

### 5. GitHub로 재난복구 Push — 명시적 사용자 승인 있음
- **새 참조 `backup/2026-11-23-safe-checkpoint`만** 검증된 통합 Commit에 생성 후 `git push origin refs/heads/backup/2026-11-23-safe-checkpoint:refs/heads/backup/2026-11-23-safe-checkpoint` 수행. 기존 같은 이름이 있으면 절대 덮어쓰기 금지; 고유한 뒤접미사로 새 ref. 사용자 승인 범위는 **이 신규 복구용 Push만**이며 remote `main`과 11/19 backup은 불변.
- 공개/원격에 올리기 전 Secrets/API.txt/TTS 키·개인 설정·민감 데이터·고용량/라이선스 불명 바이너리가 포함되지 않았는지 스캔. Git 저장소 밖 Stage WAV/ZIP/영상 등은 **GitHub Push만으로 백업되지 않음**: 안전한 비공개 외부 백업 저장 위치가 없으면 복구대상 미포함임을 정확히 알리고 파일 삭제/이동 금지. 승인 아트 5 PNG와 WAV8은 통합 Git 관리 대상인지 실제 blob·해시 확인.
- Push 성공 후 **GitHub 원격 ref 실제 SHA(`git ls-remote`) 일치**, 새 임시 bare repo 또는 별도 clone에서 `git fetch` 및 `git fsck --full`과 주요 Sprite/WAV/meta/문서의 object·SHA 확인. 네트워크/대역폭/권한 문제면 실패를 숨기지 말고 안전 종료, 임의 반복/force 금지.

### 6. 마감 공식 보고
- 사용자에게 `LOCAL_MAIN_SHA`, `INTEGRATION_SHA`, `REMOTE_RECOVERY_REF_SHA`, `PRE_EOD_REF`, `REMOTE_MAIN_UNCHANGED_SHA`, `CHANGED_FILES`, `QA PASS/FAIL/PARTIAL`, `MISSING_BACKUP(nonGit)`, `HOLD`, `실패/보호`, `REMOTE_PUSH_VERIFIED`를 짧게 보고.
- `문서/00_프로젝트/EOD_20261123_SafeIntegration_Recovery_Result.md`, `CODEX_LATEST.md`, 해당 Issue #126/#31/#127/#121 또는 신규 EOD 이슈에 GitHub 근거·브랜치/커밋 기록. `main`/remote main과 복구 브랜치는 분명히 구분.
- 이 요청은 **마감 작업에 대한 명시적 승인**으로 별도 재확인 질문 없이 안전 조건을 만족하는 범위에서 실행하되, 해결할 수 없는 위험/충돌은 자동 강행하지 말고 기록한 다음 안전 종료. 복구 Push 후 진행하면 좋을 신규 작업 발주 금지.

## 현재 기획 기록 상태
- `INSTRUCTION_RECORDED / LOCAL_EOD_EXECUTION_NOT_VERIFIED / RECOVERY_PUSH_NOT_YET_VERIFIED`.
- 이 문서가 작성되었다고 로컬 원본 main·브랜치·새 복구ref가 바뀌는 것은 **아니다**.
