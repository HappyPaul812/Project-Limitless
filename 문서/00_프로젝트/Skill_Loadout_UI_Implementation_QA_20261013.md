# 6칸 스킬 장착·전투 UI 구현 — 2026-10-13

상태: **IMPLEMENTED / 새장착·UI72 PASS / 기존15스킬154 PASS / 전체 PARTIAL / Save 확장 계약 결정 대기**.

- 브랜치: `codex/skill-loadout-ui-20261013` (격리 worktree `Temp/ABC20261013/B`). 기준 `origin/main 0e04766`, 기획 정본 `origin/docs/planning c0f1d7c`.
- 기획 문서의 과거 '구현 승인 없음'은 이번 대표님의 확정 범위 실제 구현 지시로 갱신된 것으로 해석했습니다. 신규 Chapter3 정의·가격·구매·Save 규격까지 승인한 것으로 확대하지 않았습니다.

## 구현한 범위

| 파일 | 최소 변경 |
|---|---|
| `Battle/BattleSceneController.cs` | 전투 시작 시 캐릭터ID/직업별 장착/칸위치 스냅샷, 1040×112의 2행×3열, 오른쪽 별도 돌아가기. 빈 슬롯 버튼0. 실패 안내가 메뉴에 가려지지 않게 선택 중 안내 위치만 조정. 기본5명령/상세팝업/HP/Timeline/실행기 그대로 |
| `Battle/SkillLoadoutService.cs` | 캐릭터ID+직업별 습득 목록과6칸 별도, 시작3개 기본 장착, 무료 이동/교체/해제, 중복/미습득/7번째칸 거절, 전투·월드 잠금 중 변경 금지, 복사 스냅샷 |
| `Core/GameSessionData.cs` | 기존 Reset에 임시 장착 초기화1줄. 새게임과 GameSaveService.RestoreSession의 기존 Reset 경로 활용 |
| `UI/SkillManagementPresenter.cs` | 월드·Field·Dungeon·Arbel에서 K/패드Menu·Start/화면버튼 접근, 동료별6칸+별도스크롤 보관함, 슬롯선택→스킬선택→장착/해제, Esc/패드B 닫기, 입력잠금·공용Modal 보호, 재열기 때 최신 동료 명단 |
| 신규2개 `.cs.meta` | 신규 GUID만 생성. 기존 meta/Scene/PNG 변경0 |

장착 화면은 드래그를 요구하지 않습니다. 키보드 Tab/Shift+Tab과 방향키·패드Navigation/마우스로 같은 버튼을 사용합니다. 화면 밖 보관함 행에 포커스하면 자동 스크롤합니다. 닫으면 월드 UI 포커스를 비워 패드 South가 NPC 상호작용 대신 메뉴를 재개하지 않게 합니다. 관리6번칸은 전투에서도 아랫줄6번 위치를 유지합니다. 현재 실제 직업별 습득 스킬은 기존 시작3개뿐입니다. 검색/필터는 현재3개에서 불필요하여 생성하지 않았고, 이후 실제 보관함 규모가 커지면 별도로 승인·검증합니다.

## 저장·구매 계약의 한계

**현재 장착은 세션 메모리만 유지합니다.** 지도 및 전투 왕복 중에는 유지되지만 **새게임·Save 로드·프로그램 재시작 때 시작3개로 복원**됩니다. 화면 상단에 이 제한을 직접 표시했습니다. 저장했다고 장착 순서가 보존된다고 주장하지 않습니다. 기존 Save 버전1/5슬롯/JSON 필드/소유 펫/동료/재화/퀘스트는 변경하지 않습니다.

영속 학습/강화/장착 저장은 결정 대기입니다. 승인 전 정해야 할 항목은 캐릭터별/직업별 소유권, stable Skill ID와 원본↔강화형 관계, Save 버전1 누락 필드 기본값·미습득/타직업/중복ID 복구, 저장 실패 롤백입니다. 기존 정상 Save를 변환·재저장하거나 삭제하지 않았습니다.

`RegisterApprovedDefinition`은 미래 구매 계층이 가격·직업·거래를 검증한 뒤 호출할 **내부 준비 계약**입니다. 새 정의를 자동 장착하지 않고, 기존 정의를 강화 정의로 대체할 때 장착 위치를 승계합니다. 현재 상품/Chapter3 ID/강화 정의를 만들거나 연결하지 않았습니다. 이 메서드만 호출해서 구매·영속 저장이 완성되는 것은 아닙니다.

## 검증 근거와 구분

- **PASS:** `git diff --check`; 모든 runtime 소스를 기존 격리 Unity 참조 DLL로 Roslyn compile(오류0). 신규 QA helper도 compile 오류0. 신규2meta GUID는 Assets 전체각각1회라 충돌0.
- **PASS:** 루트 담당의 실제 격리 Unity6000.6.5f1 Batch Compile/Runtime 새장착·UI72검사, 기존15개 스킬154검사(effect15/CD/MP/대상취소/자원과행동보존),1280×720/1600×900/1920×1080/1024×768/1680×720 실제 RenderTexture. Character/Library/슬롯·World/Field/Arbel/Dungeon·같은Scene 동료해금·기본5명령복귀·슬롯6 위치·36행 Layout/Mask/focus PASS.
- **PARTIAL Console:** 새 Runtime 오류0/error CS0/NullReference0/MissingReference0. 기존 `UnityEditor.Search.SearchDatabase.IndexationOnStartup` ArgumentOutOfRangeException1은 원시로그에 유지합니다. 그래서 Console전체무오류라고 보고하지 않습니다.
- **NOT_VERIFIED:** 기존15스킬과 메뉴회귀를 넘어서는 장기 전투는 별도입니다.
- **NOT_VERIFIED:** 정본 Unity6000.5.7f1 직접실행. 격리 참조 및 실제 Runtime은 사용자 원본 Editor와 같은6000.6.5f1이므로 버전 차이가 있습니다.
- **NOT_VERIFIED:** 실물 키보드·패드의 감각, 사람이 읽는 장시간 가독성, Save 영속 장착/실제 상품 구매.

## QA 절차

`Evidence/SkillLoadout_20261013/SkillLoadoutBatchQA.cs.txt`는 자동 실행 파일이 아닙니다. 지정된 격리 `Temp/Field01RenewalQA/Client`의 Assets/Editor로 복사한 QA 작업본만 컴파일합니다. Batch Mode/Client 경로/Temp 출력/Save containment guard를 통과해야 실행됩니다. 원본 Editor·GameView를 열거나 OS 포커스를 바꾸지 않습니다.

루트 담당은 위4개 runtime 소스/신규2meta만 overlay, QA 복사본 `SecondRegressionAudit.Output`만 `LIMITLESS_B_QA_OUTPUT/Skill15_Runtime.json`으로 redirect합니다. 환경변수 `LIMITLESS_B_QA_OUTPUT`를 작업별 Temp 결과 폴더로 지정하고 `-batchmode -executeMethod ProjectLimitless.EditorTools.SkillLoadoutBatchQA.Launch` 실행(자체 종료, `-quit` 제외). RT/GPU 검사이므로 `-nographics` 제외합니다.

검사는 시작3·6번째칸 장착·중복·미습득·전투잠금, 월드/Field/Arbel/Dungeon 접근, 실제관리 클릭, 같은Scene 동료해금 재열기, 36행 보관함 Layout/Mask/포커스 fixture,1280×720/1600×900/1920×1080/1024×768/1680×720 실제 RT, 기본5명령 복귀와 기존15개 UI 실행/CD/MP/대상취소/효과를 포함합니다. 6칸 그림 및36행은 **레이아웃 fixture**이며 존재하지 않는 신규 스킬/학습 데이터를 만들지 않습니다.

### 추가 공존·침묵 검증

**PASS:** C의 기존 펫 서비스 확대 코드와 함께 월드 시작마을/아르벨/Field03에서 스킬 관리와 펫 시설의 Modal 상호차단·닫기 입력잠금 복귀를28검사로 확인했습니다. MissingScript0/신규 RuntimeConsole.Errors0입니다. 이 검증은 C+B overlay에 대한 결과이며 B브랜치만으로 펫 확대 기능이 들어갔다고 주장하지 않습니다.

**PASS:** 기존 `BattleSilenceAudit.Run()`을 마지막 별도 Batch로 직접 실행하여 `BATTLE_SILENCE_AUDIT ALL PASS`를 확인했습니다. 침묵 중 Skill명령/MP 미소비·자기 정화 차단·기존4상태 정화·방어/버프/도발/기세/CD 보존을 기존 감사로 회귀 검증했습니다. 실제상품/미래상태효과를 새로 정의하지 않았습니다.

[공존28검사](Evidence/SkillLoadout_20261013/Coexistence/results.txt), [침묵 직접검사](Evidence/SkillLoadout_20261013/Coexistence/silence_existing_audit.txt), [RuntimeConsole0](Evidence/SkillLoadout_20261013/Coexistence/console_errors.txt), [기존Search예외 보존](Evidence/SkillLoadout_20261013/Coexistence/known_editor_search_errors.txt). 새4,318 보호파일SHA/원본status822·HEAD 불변은 [보호결과](Evidence/SkillLoadout_20261013/Original_Protection.json)를 근거로 합니다.

## 실제 Preview·실패 보존

- [스킬 관리1280×720](Evidence/SkillLoadout_20261013/Runtime_Final/management_1280x720.png)
- [실제 장착3개·칸위치 유지 전투1280×720](Evidence/SkillLoadout_20261013/Runtime_Final/battle_1280x720.png)
- [21:9 전투·상세팝업 최소간격](Evidence/SkillLoadout_20261013/Runtime_Final/battle_1680x720.png)
- [6칸 배치 fixture](Evidence/SkillLoadout_20261013/Runtime_Final/six-cell-layout-fixture_1280x720.png), [36행 보관함 fixture](Evidence/SkillLoadout_20261013/Runtime_Final/management-36-row-layout-fixture_1280x720.png)
- [최종72검사](Evidence/SkillLoadout_20261013/Runtime_Final/SkillLoadoutQA.txt), [기존15스킬154검사](Evidence/SkillLoadout_20261013/Runtime_Final/Skill15_Runtime.json), [로그 검증 범위](Evidence/SkillLoadout_20261013/Runtime_Validation.txt)

최초 BuildCharacters의 Open 호출누락으로 동료 명단 행이 없던 FAIL은 호출1개 추가 후 재검증했습니다. 두 번째21:9 실제 RT에서 기존팝업과 새안내문이 겹친 FAIL은 원래430×220/.37 위치를 유지하며 필요 높이만8px 간격에 맞춰 보정했습니다. HP/Timeline/전장 크기는 바꾸지 않았습니다. 각각 Attempt1/Attempt2 결과를 Evidence에 보존했고 최종 세번째 실행은72+154 PASS/FAIL0입니다. 원시Unity로그는 Temp/ABC20261013/b_unity_attempt1~3.log에 남겼으며 머신/라이선스 정보가 포함될 수 있는 로그전체는 Commit하지 않습니다.

## 보호 및 다음 단계

B는 Scene/Quest/BattleSkillSystem 실행기/SaveService/펫/TTS/BGM/승인PNG/기존meta를 수정하지 않았습니다. 원본4,318 보호파일SHA·미커밋822항목의 status/HEAD는 루트의 전후 보호 검사에서 변경0입니다. Push/main 병합0.

대표님 결정 필요: 영속 장착·학습 저장 계약과 보관함 시각/조작 확인. 현재 실행 세션 한정 구현 및 격리 결과를 먼저 리뷰하신 뒤 작업브랜치의 사용자 Push/PR/main 반영을 따로 결정하시면 됩니다. 신규 스킬 정의·가격·구매는 별도 확정 후 연결합니다.
