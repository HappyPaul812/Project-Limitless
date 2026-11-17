# Issue #102 — 승인 OGA11·OGA13 주민 원화 로컬 원본 적용 (2026-11-17)

**USER_ART_APPROVED / LOCAL_ORIGINAL_WORKING_TREE_APPLIED / DELTA_53_PASS / ORIGINAL_IMPORT_COMPILE_PASS / ORIGINAL_CONSOLE_ERRORS_0 / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**.

## 승인과 실제 반영 상태

바울 대표의 이번 직접 최종 미술 승인·로컬 원본 적용 지시를 근거로 수행했습니다. [#102 최신 납품](https://github.com/HappyPaul812/Project-Limitless/issues/102#issuecomment-6931445900), [오프라인 검수](https://github.com/HappyPaul812/Project-Limitless/issues/102#issuecomment-6932056730), CODEX_LATEST·#99/#101 적용 문서를 먼저 확인했습니다. GitHub의 기존 PENDING 기록보다 이번 직접 승인이 우선하며, **설원족제비 Defeat4 승인/적용으로 확대하지 않습니다**.

- 원본 main/HEAD **`52f84e643d5d860665eadfe1614a30c745209f00` 그대로 유지**. 기존 미커밋 상태를 보호하고 Merge 금지 지시를 따라, 검증된 제품5개를 **원본 작업 트리에만 적용**했습니다. 원본 디스크와 실제 AssetDatabase Import 완료입니다. main에 제품 commit이 들어갔다고 주장하지 않습니다.
- 전용 브랜치 **`codex/npc102-approved-original-20261117`**, 제품5파일 로컬 commit **`b7ace6b7f7958b4d74457f4c31db01e667b28856`**. 기존 소형 worktree 재사용, 새 Unity 프로젝트/대형 QA 복제0. 제품 코드/아트의 정본 commit이며 원본 working tree는 이5파일과 동일합니다.
- 본 보고/CODEX_LATEST/CURRENT_STATUS만 별도 Docs commit. 최종 실제 문서SHA는 Issue102 댓글과 private `Temp/Issue102NPC20261117/Final.json`에 기록합니다. 원격 Push/PR Merge/로컬 Merge/main ref 이동0.

## 입력과 제품 변경5파일

`F:/Downloads/Limitless_NPC_102.zip` **1436792bytes/13entries**, ZIP SHA256 **`c6e2268f817df91447f462ad7b7f30de3710fcc2abb168556884f7136c285b95`**, CRC PASS. 개별 승인 PNG2의 SHA256을 다시 확인했으며 픽셀/인코딩 재저장0입니다.

- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA11_Resident02_Original_Approved.png`
- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA11_Resident02_Original_Approved.png.meta`
- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA13_Resident03_Original_Approved.png`
- `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA13_Resident03_Original_Approved.png.meta`
- `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs`

| 신규 PNG | 원본 PNG SHA256 | 새 GUID | PPU | Pivot(정규화) |
|---|---|---|---|---|
|OGA11_Resident02_Original_Approved.png|`5fde2e4cfbcdca2839f7da1071dbbbc4b7f239ee85ab7f7b9985be48a5cb91f9`|`152b9c2ebef44afe9f39710fd1377ff7`|358.75|(0.4990234375, 0.080078125)|
|OGA13_Resident03_Original_Approved.png|`0b2b5a1a19965eeff4bbdccff0072d25c7881e6695bdc32b146542311be7f849`|`2009f218d7fd4a67bcf57d99a26e10f5`|360.5|(0.5, 0.080078125)|

카탈로그는 `starter-village-resident-02`→`VillageNpcSprites/Eldiran/OGA11_Resident02_Original_Approved`, `starter-village-resident-03`→`VillageNpcSprites/Eldiran/OGA13_Resident03_Original_Approved` **2줄만 변경**. 기존 PNG/기존 meta/GUID/Scene/Prefab/대화/Collider/Nameplate/StableID는 바꾸지 않았습니다. **Arbel의 arbel-resident-west는 기존 OGA11_Resident02 그대로**입니다.

PPU/Pivot는 #99/#101과 같은 alpha>8 가시영역 기준입니다. OGA11 bbox [149,61,362,471)→높이410px/PPU358.75, OGA13 [169,59,343,471)→높이412px/PPU360.5. 두 가시높이 모두 기존 **32/28=1.142857월드 단위**입니다. 발끝 아래 경계471에 맞춰 pivot y41px(정규화0.080078125), x는 가시영역 중심255.5/256px입니다. Transform scale1·기존Collider를 유지해 신규 캔버스512를 그대로PPU28로 크게 표시하지 않습니다. 독립 GUID2를 원본 meta 전체와 대조해 충돌0을 확인했습니다.

## 실제 Delta QA

Unity **6000.6.5f1** 기존 `Temp/Art95_91Unity20261111/QA/Client`와 Library 재사용, 숨김 Batch Mode에서 **53 PASS / 0 FAIL**. 원본/사용자 창·OS 포커스·실물 키보드/마우스 입력 조작0. QA의 가상 입력과 합성Save/Settings만 사용했습니다.

- 960/1280/1680×720의 전후12실제 렌더링, 두 대화2장·Overview1장. 두 주민의 가시높이/발접지/viewport 정상, 기존 승인NPC들과 나란히 확인했습니다. 사용자 승인 그림을 바꾸거나 다른 NPC를 재작화하지 않았습니다.
- 두 주민 **일반 Resident의 상시 이름표 숨김 정책 유지**. 접근 `[E] 대화하기`와 대화창의 기존 이름 `주민` 정상. OGA11 기존 대사 “남문 밖은 초원이에요. 준비를 확인하고 나가세요.”, OGA13 “마을 길은 넓으니 천천히 둘러보셔도 됩니다.” 실제 표시 확인.
- 가상 키보드E/Esc·게임패드A/B로 실제 기존 대화 열기/닫기 PASS. 주민에 새 상점/업무 UI를 만들지 않았습니다. Scene 재진입/카탈로그 선택·Collider/Transform/sorting·기존 승인8 및 다른NPC 외형·Arbel 기존OGA11 재사용·가격 보존 PASS.
- **신규 게임 런타임 오류0**. 격리 QA 시작 시 기존 UnityEditor.Search 색인 ArgumentOutOfRangeException1건은 재현됐으며 제품오류와 별도 `Evidence/EnvironmentErrors.txt`에 기록합니다. 전체 QA 로그 무오류라고 하지 않습니다. Issue85 패키지/GUID 복구검증을 수행한 것이 아닙니다.

## 원본 Import·컴파일·Console

원본 Editor 실행 중이 아님을 프로세스·CLI로 먼저 확인하고 원본 역시 **숨김 Batch**에서 새PNG2 Import·컴파일·실제 Resources/StableID 매핑/GUID/PPU/Pivot를 검사했습니다. 원본Play·Scene전환·Scene저장0. 기존 Editor 세션을 임의 종료하거나 foreground로 가져오지 않았습니다.

원본 실제 결과: **PASS**, `compiling=False`, **Console errors=0 / warnings=104 / logs=0**. 오류를 Clear해서0으로 만들지 않았습니다. 경고가 없는 프로젝트라는 뜻은 아닙니다. `original-resources.txt`에 로드값이 기록돼 있습니다. 원본 패키지/기존GUID 수정0·Issue85 HOLD 유지.

원본 일회성 검사기 `Assets/_Project/Scripts/Editor/Issue102OriginalVerify.cs`와 해당 새meta는 이번에만 추가해 검사한 뒤 제거했습니다. 원본 제품/커밋에는 포함하지 않습니다. 기존 보호파일의 자동 Import 변환은 채택하지 않았으며 이번에는 추가 자동변경 복원 대상도0이었습니다. 검사기 제거 후 다음 Editor 열기에서 정상 소스 재검사가 일어날 수 있습니다.

## 보호와 실제 확인 위치

- 시작 보호8342개 중 허용된 카탈로그1개만 제품 변경, **나머지8341 SHA변경0**(완료보고 갱신 전 검사). 기존 사용자 git status는 제품5개를 제외하고 byte-exact 동일. 문서 갱신2개는 별도 허용 범위입니다. 신규PNG SHA는 승인원본과 동일, Save/UserData/Discord/기존승인8·OGA20HOLD/설원족제비 원본 보호.
- 기존 Packages/ProjectVersion/BuildSettings/Scene/개인Settings/음성/기존dirty·미추적 원본을 그대로 유지했습니다. 기존 복구목록/백업 재사용+어제 이후 달라진 소형파일만 private ProtectionDelta에 보완, 대형 재백업/캐시삭제0.
- F: 시작 **321517383680bytes**, 원본 Import·보호검증 후 **321502527488bytes**. 시점별 실측이며 다른 프로세스 용량도 반영될 수 있습니다.
- 바울 대표 확인: **시작마을 남동쪽 주민2 (3.2,-2.6)** — 모자/조끼/바구니, **서쪽 주민3 (-7.2,0)** — 숄/치마. 가까이 가서 E 또는 패드A로 기존 대화를 확인하시면 됩니다. 두 주민은 상시 이름표가 없는 것이 정상입니다.
- 사용자 최종 미술승인·로컬 적용 완료, 대표 직접 새Play 수용은 **USER_REALPLAY_PENDING**. 추가 적용승인 대기는 아니며 원격반영 없음.

증거: private `Temp/Issue102NPC20261117/Evidence/checks.txt`(53PASS), status.txt/Errors.txt(0)/EnvironmentErrors.txt/InteractionDiagnostics.txt/실제PNG15장, Unity-QA.log/Unity-Original.log/original-resources.txt/original-status.txt/Final.json. 공개 Git에 검사기·합성Save·개인보호목록·전체증거를 추가하지 않았습니다. 후속 기획·아트·Issue85·설원족제비 작업 자동착수0.
