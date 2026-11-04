# 2026-11-04 긴급 로컬 main 안전 동기화 — 실제 완료

**LOCAL_MAIN_SYNC_SUCCESS / USER_WORK_BYTES_PRESERVED / REMOTE_NOT_PUSHED**

사용자의 이번 직접 승인에 따라 기존 dirty를 숨기지 않고 파일별 분류·외부 백업·격리 재현을 마친 뒤 실제 원본 main을 fast-forward했습니다. 이전 `LOCAL_MAIN_SYNC_HOLD_DIRTY`는 이 보고로 해소합니다. **원본이 깨끗해졌다는 뜻은 아닙니다.** 관련 없는 사용자 미커밋302개·미추적550개는 원본 경로/바이트 그대로 유지합니다.

## 실제 저장소와 커밋

`git worktree list --porcelain`로 확인:
- 원본 main: `F:/study/codex/Project-Limitless`, 시작 HEAD `634a91d3e4174c87761c0d377cfa5a695ca79e7f`.
- 승인 통합본: `F:/study/codex/Project-Limitless/Temp/EODApproved20261104/Integration`, `codex/integrate-20261104-approved` **8768cbac91a291f13769c6bfa18a831673652acb**(변경하지 않음).
- 추가 보존/동기화 브랜치: `codex/local-main-safe-sync-20261104`, worktree `Temp/SafeMainSync20261104/Prepared`.
- 실제 코드·보존분 main 반영 SHA: **97992cdaff69a38b458a715fb4073907fb926d4c**, 승인8768의 후손. Main24 새 코드 변경 없이 과거 PilotBee 수정 meta의 별도 이력만 추가했습니다. 이 마감 문서를 포함한 후속 문서 커밋도 별도 브랜치에서 작성한 뒤 main으로 FF하며, 최종 전체 SHA는 GitHub 실제 댓글/로컬 최종 영수증으로 보고합니다. main 직접 commit은 없습니다.

GitHub docs/planning의 최신 PLANNING_LATEST(19:58 EOD), SafeRunbook과 원격 CODEX_LATEST/CURRENT_STATUS를 읽었습니다. 원격 두 Codex 문서는 오래된10/12 내용이며, 실제 최신 Codex 결과는 로컬8768 및 GitHub #63/#89/#90/#92 댓글이라는 차이를 확인했습니다. 오래된 문서로 신규 승인/검증을 취소하거나 복원하지 않았습니다. 이번 직접 승인은 파일별 안전 조정 후 FF 진행을 허용하며, 강제 갱신 금지는 유지합니다.

## 858개 파일 전수 분류

파일별 원본 경로·porcelain 상태·바이트수·SHA256·base/통합 SHA·분류·겹침·해결 방식을 CSV/JSON에 기록했습니다. 실제 작성 주체가 불명확한 파일을 자동 생성물이라고 단정하거나 승인 자산으로 승격하지 않았습니다.

| 분류 | tracked 변경308 | untracked550 |
|---|---:|---:|
| Unity meta/GUID/Importer 보호 | 256 | 38 |
| 사용자 코드·Scene·Prefab·Animator·Unity 설정 보호 | 47 | 20 |
| 다른 제작/검수 아트·음성/납품 자료, 자동 채택하지 않음 | 2 | 386 |
| 문서/검수 도구 | 2 | 73 |
| 개인 Save 관련 보호 | 0 | 11 |
| 생성 캐시·빌드/로그(주로 __pycache__, 삭제하지 않음) | 0 | 19 |
| .gitignore 등 설정/기타 보호 | 1 | 3 |
| 합계 | 308 | 550 |

별도로 ignored 개인 Save/UserData **23개**를 내용 공개 없이 보호했습니다. 위11개는 과거 감사용 경로 분류이며 이23개와 동일 집합으로 오인하지 않습니다. 원화·음성의 승인 여부를 이름만으로 확정하지 않고 기존 위치를 유지했습니다.

전체 목록 위치: `F:/LIMITLESS_Backups/20261104_SafeMainSync_201027/Final_FileClassification.csv` 및 `.json`. SHA는 `Evidence/SafeMainSync20261104/FullClassificationLocation.json`에 있습니다. 개인 관련 경로가 포함된 전수 목록/개인 파일은 GitHub 댓글에 붙이지 않습니다.

## 디스크·백업·실제 충돌 해결

백업 전 F: 여유347,264MB 수준(정확 바이트347,263,684,608), 분류파일188,469,597bytes. 실제 별도 백업 `F:/LIMITLESS_Backups/20261104_SafeMainSync_201027`.
**변경/미추적858개 + 추가 개인 보호23개 = 고유881개, 총189,292,519bytes**, 복사 후 SHA881/881 일치, 동기화 후 재검증도 불일치0. 개인 Save 내용은 원격/QA로 복사하지 않았고 사용자 지정 F: 보호 백업에만 추가 보관했습니다.

경로 겹침 **6개 / 해결6 / HOLD0**, 텍스트 한쪽을 버려야 하는 미해결 내용 충돌0:
1. `Unity/Client/Assets/_Project/Scripts/World/Main24FrostOwlCandidate.cs`: 원본 작업파일과 승인 통합본의 전체SHA가 동일(`36983eac…ce2fc`). 내용은 그대로 두고 이 파일 하나만 선택 staging하여 이미 승인된 동일 변경을 Git이 인식하게 했습니다.
2. 나머지5개는 `ThirdParty/2DPIXX/PilotBee/Original`의 Attack/Idle PNG meta2와 `Resources/MonsterValidation/PilotBee`의 SlimeComparison/Attack/Idle PNG meta3입니다. importer 직렬화 확장/설정 변경을 실제 비교했습니다. 과거 Git 원본 meta는 기존 `Archive/ExcludedPilotBee/`에 유지하고, 사용자 작업 meta는 `Archive/ExcludedPilotBee/LocalImporterChanges20261104/`에 별도로 바이트 보존했습니다. GUID/필드 어느 한쪽도 임의 폐기하지 않았습니다.

원본 PilotBee 폴더2개와 각 폴더meta는 F: 백업의 `RelocatedPilotBeeOriginal/`에 **보존 이동**했습니다(21파일 전부 SHA 확인). 파일 삭제/무차별clean을 사용하지 않았습니다. 따라서 외부 그림은 Unity Assets/Resources에서 제외되며 과거 원본과 사용자 importer 작업 모두 찾을 수 있습니다. 새 독침벌 원화와 섞거나 라이선스 조사를 재개하지 않았습니다.

## 격리 재현 → 실제 FF

- 별도 Simulation worktree에858개 dirty 상태/바이트를 그대로 재현했습니다. 동일 Main24 선택 staging과 PilotBee 보존 이동만 수행하여 `git -c merge.autoStash=false merge --ff-only --no-autostash 97992cda…` 성공,858개 바이트 보존 확인.
- 원본에서도858파일/전체tracked 사전스냅샷·빈 index·목표조상관계·원본HEAD·Unity버전을 재확인한 뒤 동일 절차로 **실제 FF 성공**했습니다.
- `reset --hard`, force, stash, clean, 일괄checkout/restore, 사용자파일 삭제, main 직접commit, 원격Push/Merge를 사용하지 않았습니다. 정상 변경·미추적 자료를 자동 커밋하지도 않았습니다.
- dirty308 →302의 차이6은 승인본과 동일했던 Main24 1개가 Git 이력에 반영되고, PilotBee meta5개가 바이트보존 이동/별도 이력으로 들어간 결과입니다. 사라진 사용자 작업이 아닙니다. 미추적550개는 경로 집합까지 동일합니다.

## 최소 Delta 검증·현재 적용 상태

신규 파일/Git 보호 검사 **16 PASS / 0 FAIL**(`SyncDeltaChecks.json`). 실제 source의858개 변경파일(보존 이동 포함) 바이트불변, 백업881개SHA불변, private23개불변, 관련 없는 tracked 원본 변화0, 미추적550 경로/바이트불변, 승인PNG37/37 동일, 새PNG GUID37개 각각 Assets 내 유일, 승인 Unity 변경경로 drift0, 기존 PilotBee PNG는 Assets내0, index clean.

Unity **6000.6.5f1 (3ff58d469c8a)** ProjectVersion 유지. MCP 읽기전용 조회의 연결Editor도6000.6.5f1이며 idle/PlayMode아님, 열린Scene은 원본main이 아닌 별도 Main22 worktree입니다. Editor/포커스/Scene 선택/PlayMode를 조작하지 않았습니다.
추가 변경은 Assets 밖의 meta 보관/문서뿐으로 **승인8768 대비 Unity 코드·자산 변경0**. 기존 격리60PASS/시각52PASS·상세빌드Succeeded(오류0·경고38)를 재사용했고, 이번 Unity Import/기능QA 전수/PlayMode 재실행0. 기존 사용자 dirty Scene/Prefab/Packages/BuildSettings는 그대로라 원본 전체제품의 새 런타임/출시QA 완료로 확대하지 않습니다.

| 대상 | 실제 원본main 파일 반영 | 남은 범위 |
|---|---|---|
| #63 | 승인 신규5포즈·정의·시퀀스·기본공격 연결, 기존PilotBee Assets제외 | 정밀 침접촉/FootLock 미검증. 원본출시빌드 재실행 없음; 승인통합 격리 BuildReport의 외부그림0 근거 재사용 |
| #90 | Attack8·기승인Idle/Walk·기본공격 완료대기 | Shoot8 HOLD, 정밀 접지/접점 미검증 |
| #89 | 정식아이콘3·VFX3·B표시API/문자/HUD | 냉기 자동상태생성/수명·동시우선순위·미정피해/재적용/해제/화로는 아직 미구현/미확정 |
| #92 | 배경2·Field05~10 catalog 매핑 | 기존 태온/휠체어 겹침·하단안내/128px가림 등 VISUAL_PARTIAL 유지 |
| #91 | 이번에도 미반영 | USER_VISUAL_REVIEW_PENDING, 미승인C3BG를 임의 완료 처리하지 않음 |

## 추가 승인/미결

**이 로컬 동기화를 위해 추가 승인이 필요한 파일은 없습니다.** 기존 사용자302 dirty·550 untracked의 별도 정리/채택/커밋·삭제, 미승인#91, Shoot8, 냉기미정규칙, 시각PARTIAL 해소를 이번 승인으로 확대하지 않습니다. 원격Push·Merge는 계속 금지입니다. 오늘 신규 제작/전체 QA를 시작하지 않습니다.
