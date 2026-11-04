# LIMITLESS / 2026-11-04 / 최종 미술 승인 3건과 로컬 통합·동기화 종료 지시

- 발주자: 바울 대표. 원문 **“다 승인하고 오늘은 여기까지 하자 다 합치고 로컬 동기화 해줘.”**
- 총괄기획: `docs/planning`; 본 문서는 **GitHub 미술 승인 기록·로컬 Codex 실행 지시**이며 **실제 사용자 PC에서 통합/동기화 실행을 대신하지 않는다.**
- user final approvals: [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6627888182), [#89](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6627893169), [#92](https://github.com/HappyPaul812/Project-Limitless/issues/92#issuecomment-6627894117). 확인에 사용된 F: 세 ZIP 전체SHA가 **2026-11-04 Codex 최신 #63·#89·#92 보고에서 각각 일치**.
- 이미 승인 #90 Spider Attack8 전체8, 기승인 #89 냉기 VFX 정지PNG3 및 B안 격리50PASS, 이전 #88 냉기 Stage1 격리63PASS 등을 품질·구현·원본 적용 별도 상태로 보호. #91 Chapter3 전투배경 v2는 이 **3ZIP 승인 문구만으로 소급 승인하지 않는다**.
- **Unity 버전 절대 고정: 6000.6.5f1**, ProjectVersion.txt/UnityHub/실제 Editor 설치 비교. **6000.6.6/다른 Editor 자동 사용·프로젝트 업그레이드 금지.** 프로젝트 원본 사용자 dirty 약 308 tracked (Codex 기존 보고)일 수 있으므로 최신 `git status` 실제 확인.

## 확정 승인 자산
| 대상 | 승인 ZIP SHA256 | 실제 승인 범위 | 안전한 Unity 통합 조건 |
|---|---|---|---|
| #63 신규 벌 3포즈 | `172df8f2d658b02f6859554284ab4b719175b2ce86b922fe5270f4ae9803b325` | **11/04 Prepare·IdleFolded·IdleSpread 3PNG + 기승인 Sting/Recover v3**. 기존 11/03 세포즈는 이력 보관만, 새 벌 시퀀스에 섞지 않음 | 신규5포즈/10FPS/Idle 전환·실전 타격과 상태 연결 표적 QA; 기존 PilotBee의 Resources·02_VenomBee.asset GUID 참조 제거 후 최종 빌드 포함 여부 확인. 새 외부권리·비침해 법률 보증을 주장하지 않음 |
| #89 냉기 정식 아이콘3 | `f758b59b73847f18c13e0d48aafee2f58eac33802ec597a1c739078335b2baa3` | 한기·동상·동결괴사 128RGBA 각1, 원본바이트 동결 | 기승인 B 레이아웃·전장VFX3를 보호하고 **독립 아이콘 매핑/실제 약12px HUD 최소5화면** QA. #88 미확정 피해/재적용/화로 전투 로직 임의 확정 금지 |
| #92 Ch2 전투 배경2 | `e439cd32bda173063ee05fd3cb3fd8a099dc4f3a6a67b805150b2777ba6b7ff0` | Field05~08 AshenWasteland, Field09~10 ObsidianRift 각1672×941 | 기존 Ch1 3종 실화소1:1 비교 보고 #92 PASS 접수, 격리 Unity BattleBG Catalog/오른쪽 아군3·왼쪽 적/휠체어·3화면비·암부 실가독성 최소 QA. Field11/Main20 DeepCore 기존 유지 |

## Codex 로컬 종료 지시 — 실제 실행, 단 조건 미충족은 중단

1. **선행 보존·충돌 검사(필수):** 올바른 로컬 프로젝트 절대경로·`git rev-parse`/상태, `Unity/Client/ProjectSettings/ProjectVersion.txt`, 실제 설치된 **6000.6.5f1**을 확인한다. 원본 Editor 열림 여부·사용자 저장·Tracked/Untracked/Ignore 목록·BuildSettings/Packages/Scene/Prefab/GUID/Meta/Save/UserData 미커밋 상태를 변경 전 출력/체크섬 기록. 사용자가 작업중인 원본을 덮지 않도록 **별도 Codex integration worktree**와 백업/충돌 리스트를 만들고 `reset --hard`, `clean -fd(x)`, 무차별 `git stash -u`, `git checkout -- .`, force, Editor 자동 업그레이드를 금지한다.
2. **자산 원본 확인:** F: 세 ZIP 전체 SHA256이 위 값인지 변경없을 때 간단 확인(이전 실조회 기록이 유효하면 불필요 중복 연산X). #90 승인 Attack8 ZIP SHA `58c51cb49d63cde4669dc183da8583701e7e0bc9dceae9fae5542a5573f0fc9d` 및 해당 완성 모션은 기승인 기록 재사용. Ch3 #91 배경·독침벌 이전3·미승인 후보는 무단 적용 금지.
3. **로컬 통합:** `codex/integrate-20261104-approved` 격리 브랜치/worktree에서 승인 파일·기존 동작코드의 *필요한 변경분만* 통합. docs/planning, docs/art-renewal, docs/art-new 및 Codex 로컬 문서 커밋의 내용은 충돌없는 **선별 cherry-pick/패치**로 병합하되, 다른 담당의 승인아트·긴 이력 문서·메타/GUID 덮어쓰기 금지. **GitHub 원격 main 직접 커밋/Push/병합 금지.** 기존 main의 변경 부분과 충돌하면 자동선택하지 않고 중지하여 보류표 기록.
4. **표적 검증:** 이행한 부분에 대해서만 Unity6000.6.5f1 격리 Import/Compile·실제 BattleHUD/3인전투·공격→Idle / ColdIcon/Ch2BG 표시의 최소 Delta QA를 수행. #63 구PilotBee를 옮기거나 삭제하기 전에 GUID 참조 교체·Resources/Atlas/Build 입력을 안전하게 조사하고 필요 시 실제 BuildReport로 제외를 확인. 이전 #63 153+15, #89 99+50, #90 16, #88 63 등을 전수 재실행 금지. 실제 테스트 범위·수치만 기록.
5. **안전한 로컬 동기화:** 1~4가 통과한 항목만 integration branch에서 별도 commit/백업. **원본 작업트리의 dirty 변경을 덮지 않고, fast-forward 조건과 충돌0을 만족할 때에만 로컬 main 업데이트**. 원본 작업트리가 수정 상태이면 자동 `pull`/`merge`/`reset`으로 덮지 말 것. 대신 원본 상태 그대로 유지하고 integration branch가 동기화 준비됐다는 명확한 **`LOCAL_MAIN_SYNC_HOLD_DIRTY`** 보고. 원격 git fetch는 가능하나 Push/원격 Merge 없음.
6. **마감 보고:** 적용 성공/보류/미적용 자산별 표, Unity 실제 Editor 버전, local main·integration branch HEAD SHA, 기존 dirty 파일 보호/Unity 프로젝트·Save·GUID/Meta/원본 BuildSettings 무변경 증거, 변경 파일 수, 신규 테스트 PASS/FAIL, 브랜치·커밋SHA, 필요 승인 항목을 `CODEX_LATEST`·`CURRENT_STATUS`와 #63/#89/#92/#90 이슈에 인계. **원본Unity적용 완료라고 쓰려면 실제 원본 바이트·런타임 확인 근거가 있어야 함.** 미완료 시 DONE 대신 HOLD. 밤새 자동 예약/사용량 초기화 후 무인 재시도 금지.

## 상태/정책 경계
- **지금 확정된 것:** 사용자 3건 미술 승인 + 기존 Spider8 미술 승인.
- **지금 미확정인 것:** 실제 Unity 원본에서의 신아트 교체·새 상태 아이콘 Gameplay/HUD·Ch2 Battle Catalog/필드 매핑, PilotBee 출시 빌드 제외, 새 자산의 실제 Runtime QA와 로컬 main 동기화 여부.
- **원격 기록만으로 "다 합쳤다/로컬 동기화했다"라 주장 금지.** PC에서 위 Codex 실행이 끝난 실제 작업보고로 갱신.
