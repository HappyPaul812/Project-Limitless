# LIMITLESS — 2026-10-19 Codex 통합 마감 실행 지시: Main18 세린10 + Field05 B42 리뉴얼

**작업 지시자:** 바울 대표, 기획 담당 정리. **브랜치:** `docs/planning`(이 문서만). **실행:** Codex on user's Windows PC (local) — 기획 대화의 GitHub 문서화 자체는 PC 작업 실행을 뜻하지 않음.
**상태:** `INSTRUCTION_READY / CODEX_EXECUTION_PENDING / AUDIO_REMOTE_PUSH_PENDING / FIELD05_V2_VISUAL_APPROVAL_PENDING / LOCAL_SYNC_PARTIAL`
**사용자 요청:** “리뉴얼 작업까지 다 묶어서 코덱스한테 시켜줘. 이번주에 재설정권 1장 써야 해. 기간이 얼마 없거든.”

## 0. 긴급 우선순위 / 별도 브랜치 / 안전게이트

1. **A / Main18 세린10 음성 이미 검증된 기능 커밋의 최종 Push→PR→QA→main 병합** (승인된10개만). Codex 보고 **`codex/tts-main18-serin10-integration-20261019`**, 기능 `94a68cee54be397b16d8bacd4c3f930bad2eece3`, 최종 `85633624f71bd18d5d79d495812980372c056975`; 격리 Unity Main18 22/22 Runtime, 최종1370PASS/0FAIL, 정상25·Main17 Serin11·PCM35 원문·원본415? 보호, 환경 Search 기존 OPEN. **GitHub 최신 원격 조회 당시 이 브랜치는 미Push, main=`c263d9e36fc0570b16130d9c011abf3965def5f7`**. 브랜치·커밋 존재 여부는 Codex 로컬에서 실물 확인. 절대 Main18 Serin 기존25 또는 다른 대화 ID/PCM 덮어쓰기 금지. 신규 TTS/API 호출0.
2. **B / Field05 승인 분위기 + 새 v2 PNG4 동서 Road42 격리 적용/QA**. B42는 기획 선호안이지 **바울 대표가 새 PNG4 실제 Unity 최종 화면을 미술 승인한 것은 아님**. Codex가 새 v2 Road4를 B42 실Unity 동일 카메라 렌더링해 사용자 검수자료와 안전 검증/전용 코드 브랜치 준비 가능. **정식 원본 Scene 적용·B42 PR main 병합은 화면 미술 승인 이후**. 리뉴얼 업체 PNG4 **이미 제작됨**: `Limitless_Field05_EastWest_Road_v2.zip`, SHA256 `3a0d3b91cd87a5362ebbe435c1e5acd06ffc74354eafaa41f02b4f181d27c3d3` (오프라인 seam0, 실제 v2 Unity QA 미검증). 사용자 PC ZIP 설치/위치는 미확인. ZIP 없으면 정확한 실제 다운로드 위치 확인 요청; **그림 다시 만들기·v1 Road PNG 90도 강제회전 임시판을 공식 적용 금지**. 추가 Road Renderer는 12로 B42 전체 315 Ground +42 Road +18 Props=**375 SpriteRenderer**(다른 씬 렌더러 종류와 별도 집계)로 예상; 격리 실측 필수. 옵션 소품7 미승인 미배치, 새 Portal0.
3. **C / GitHub·로컬 안전 동기화**. PR#16으로 기존 로컬2문서커밋 원격 Git 이력 보존됐으나 **로컬 `main`이 464개 dirty 경로와 겹치고 5파일 실제 차이**여서 FF 중단되었던 **`LOCAL_SYNC_PARTIAL`** 잔여. 백업·해시·비파괴 충돌 해소를 통하여 git HEAD를 안전하게 일치시킬 수 있을 때만 수행. 원본 dirty·미추적·유저 UserData23/Settings/Saves·Editor/Play/OS focus 보호. reset --hard / clean -fd / force push / 무단 stash / 원본 덮어쓰기 금지.

**작업 병렬성:** A PR 준비·GitHub 검증과 B ZIP/씬 감사는 각각 분리 worktree에서 병행 가능하나 사용자 원본 Unity Editor·Library를 동시에 사용하는 Batch 프로세스 충돌 금지. A와 B 파일을 한 commit으로 섞지 말 것. 시간이 부족하면 A 원격 병합 완료 우선, B는 격리 이미지·QA 및 안전 재개점까지. 사용량 재설정권은 Codex 앱/사용자 계정에서 사용 가능한 시간/조건을 직접 확인. 기획/CLI에서 소유권 없이 재설정 수행했다고 주장하지 말 것.

## 1. A / Main18 음성 실제 본게임 적용

- `git fetch` 및 원본 `git status`/worktree list/HEAD/branch 확인. 이미 로컬 feature commit들이 있는지 해시 검증: `94a68cee54be397b16d8bacd4c3f930bad2eece3`, `85633624f71bd18d5d79d495812980372c056975`. 기존 통합 PCM/GUID/메타/승인 ten/sample1+preview6/수동복구3 추적, 동일 SHA 재작업 금지.
- `main`의 Main18 StoryVoiceCatalog/음성25 및 Main17 Serin11·Main08 기존 승인 음성·대화 텍스트·퀘스트/Save·하렌/미엘/폴/태온 본문 보호. Main18 세린10만 신규 후보 Catalog를 안전하게 연결. 별도 원본 파일을 임의 변경하지 않음.
- 이전 QA 1370 PASS0/22실행을 바탕으로 **새 base main 대비 delta 검증** 및 CatGUID 존재/PCM/발화원문·초기 무음/초상화·실제 Unity 한 장면씩 동기재생/전후 일반 NPC 발화/SaveContinue/BuildScene. 환경 Search/URP 재현이 원본기준선과 동일하면 `KNOWN_ENV_PARTIAL`로 기록, 신규 콘솔 예외는 FAIL.
- QA PASS 시 기능 브랜치 `codex/tts-main18-serin10-integration-20261019` Push, 정확 HEAD 기반 PR 생성·검토→**바울 대표의 이번 통합 지시 범위에서 Main18 승인10 원격 main 병합**. 가능하면 실제 원본 프로젝트에도 안전하게 바이트/GUID 기준 공식 적용하고 로컬 Import 확인, 기존 dirty 충돌 시 무리하게 즉시 Import하지 말고 `REMOTE_MERGED / LOCAL_APPLY_PARTIAL` 명확 보고. Unity Editor/OS focus 비강탈.
- Push/merge 미실행 상태를 `Unity 게임에 넣었다`고 말하지 말고 **격리 적용 / 원격 병합 / 실제 원본 디스크 / 원본 Editor runtime**을 각각 구분.

## 2. B / Field05 동서 Road42 새 PNG4로 실제 최종 미술 후보

- `F:\Downloads\Limitless_Field05_Renewal_Art_v1.zip` SHA `a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258`(사용자 로컬 이전 확인 보고)과 v2 `Limitless_Field05_EastWest_Road_v2.zip`의 **실제 위치/ZIP SHA/Manifest/PNG4 CRC** 검증. v2 ZIP 파일이 실제 로컬 어디에도 없다면 **멈추고 경로/다운로드 자료만 요청**, 재생성 또는 임의 유사 PNG 합성하지 않는다.
- 최신 기획 `문서/00_프로젝트/Chapter2_Field05_EastWestRoad_VisualCorrection_20261019.md` 및 art-renewal `문서/00_프로젝트/Field05_EastWest_Road_v2_Art_Delivery_20261019.md`, B42 `Road_Placement_A_B.csv`를 읽는다.
- **B42 우선 시안**: 동서 Road42 x=-10..10,y=±0.5 21열×2행, 좌측 x=-10은 WestEnd Upper/Lower, 나머지 Upper/Lower. 신규 Road4 각각 128×128 RGBA/PPU128/Pivot Center/Point/mipmapOff/Z0/FlipFalse/alpha seam0. 기존 Road30 보존하면서 신규 GameObject12의 GUID/Scene ID 충돌0. 315Ground+42Road+18Props=375 환경 Renderer 목표. 카메라/도보·휠체어/세린(1,0)/Main14 수로(5,2),초소(-2,2),진동(-6,-2)/그을음들개(3,-3),균열도마뱀(-4,3)/Arbel eastSpawn(8,0),Exit(10.7,0)/35s respawn/Save·Continue/Monster·Collider 보호. 새로운 서쪽 Portal/전환 기능 추가0.
- v1 원본남북 vs 임시 B42 회전 vs **진짜 v2 Road4 B42** 최소 3열 고정 카메라 16:9와 접근용 확대, 4:3·21:9 품질. 길 끝이 서쪽 경계에서 **자연스럽게 흙으로 소멸**, 타일 사각띠/절단/공중부유/안개 가림/서쪽 false Portal0. 색/Props는 이전 사용자 긍정 v1 유지.
- 격리 Unity Play/Scene/Static 및 물리·Monster/Quest/Main14/Save/Continue·UserData/접근성 회귀 **QA PASS 후** 전용 브랜치 `codex/field05-eastwest-road42-v2-20261019`에 **격리 검증된 변경만 Commit**, 결과 실영상/스틸·ZIP SHA/QA/적용 범위·선택7 미배치 문서 작성. **바울 대표 실제 v2 미술 승인 전 원본 Unity에 정식 반영, feature Push/PR 완료 주장, main 병합 금지**. 사용자 동의 시 별도 승인안만 feature Push/PR 및 main 반영.
- 작업 B의 ZIP이 준비되었지만 새 v2 실제 렌더를 보지 못한 상태와 아트 자체 품질 승인을 혼동하지 않는다. 리뉴얼 담당에 동일 Road4 재제작 발주 금지.

## 3. 로컬 동기화·안전 검증

- 마지막 사용자 보고 PR16에 로컬 전용 문서2 커밋 원격 조상 보존 성공. 그러나 겹치는 dirty464 경로 중 실제 다른 내용5 경로로 ff-only STOP. **이번 작업 시작 시 최신 상태 재감사 필수**. `git status --porcelain=v1 -uall`, `git log --left-right --cherry-pick main...origin/main`, `git worktree list`, 현재 소스의 압축 전후 sha/backups/Editor/Save23. 5개 파일의 정확한 종류/실제 콘텐츠 구분.
- 백업 위치·sha checksum을 먼저 검증. `git reset --hard`, `git clean -fdx`, `git checkout -f`, 무단 stash/강제 Push/임의 문서 삭제 금지. 필요하면 기존 로컬 독립변경5개를 별도 보존 브랜치/파일로 보존한 상태에서 격리 worktree로 `origin/main` 최신 HEAD를 수용하는 비파괴 방식. 사용자 작업을 새 main에 몰래 커밋해 공개하지 않는다.
- **검증 세 가지 따로:** `remote main merged SHA`, `local main == origin/main == remote main`, `original active Unity disk updated/Editor available`. 하나라도 실패하면 `PARTIAL`, 손실0 기록. 새 파일/Meta/Main18 Catalog/Field05 scene에 dirty overlap이 있다면 백업 후 충돌 해결 못하면 원본 변경 STOP.
- 원본 editor가 열려 있으면 앱 포커스를 강제로 바꾸지 않는다. Unity 6.6 버전은 현장 `ProjectVersion.txt`와 설치 에디터 실제 버전을 보고; 과거 표시 `6000.5.7f1`/검증 `6000.6.5f1` 혼동을 정본으로 단정하지 않음.
- 기존 Portal-나무 겹침, Search/URP 예외(기준선)·미검증 OS 수동 입력은 OPEN/PARTIAL로 남긴다. 사용자가 무한 범위의 모든 버그 수정을 승인한 것으로 해석하지 않는다.

## 4. 완료 보고(각 독립 상태와 숫자)

- **A Main18**: 대상10 ID/실제 Catalog/음성 원문·보호25/11, QA, branch/feature commit/PR/main SHA, 원본 disk applied 여부.
- **B Field05**: 실제 ZIP 발견경로·SHA; v2 Road4/Renderer375/BeforeAfter3열·서쪽 길 끝/모든 Scene/Quest/휠체어 QA, 미술승인 `PENDING`, 적용/Push 여부.
- **C 동기화**: 원격 main sha/local main sha/origin main sha, 겹침464/차이5 최신 상태, UserData23·dirty 전후 SHA, 실제 Unity Editor 동작 미검증 사항 및 각 STOP 원인.
- `CURRENT_STATUS`, 담당 Codex 인계와 branch commit 기록에 **실행 결과만** 남긴다. UserData/Save·Settings/문서 커밋2 원래 SHA 보호, main 직접 커밋 금지.
- 시간/한도 종료 시까지 작업이 불완료면 완료된 A/B/C 경계·해시·담당 재개점을 철저히 작성하고 **실제 검증되지 않은 상태를 완료라고 표시하지 않는다**.

**중요:** 기획 문서/프롬프트는 Codex 실행 자체가 아니다. 사용자 PC 접근/주간 재설정권 사용은 사용자/Codex 환경에서 수행해야 한다.


**Field05 PNG 결합 범위:** 기존 리뉴얼 v1 ZIP의 Ground3+Props7을 유지하고, **v1 남북 Road2는 최종안에서 사용하지 않으며** 동서 v2 Road4(Upper/Lower/WestEnd Upper/Lower)로 교체하여 **실사용 원본 PNG 소스14종(3+7+4)**을 격리 적용한다. 기존 Scene Renderer는 B42에서 환경 315 Ground + 42 Road +18 Props =375개 목표. v1 Optional7은 미배치. ZIP에 들어있는 Preview 이미지는 실사용 Asset 수로 세지 않는다. 새 Road4의 v2 실Unity 시각 승인은 사용자 검토 전 Pending.
