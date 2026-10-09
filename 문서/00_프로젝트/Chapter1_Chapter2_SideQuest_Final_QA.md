# Chapter 1·2 서브 퀘스트 9종 최종 QA — 2026-10-09

기획 완료 / 구현9종 완료 / 정적 검사 PASS / 격리 Runtime PASS / 사용자 시각·실입력 승인 대기.
Chapter1 5/5, Chapter2 4/4. EXP155+285=440 / Talent62+111=173.
시스템 commit `7a78aa8`, NPC·UI·Quest9 commit `14327fd`. 최종 QA commit은 이 문서를 포함한 `Chore: 서브 퀘스트 9종 격리 Runtime QA 기록` 참조.

## 검증 환경과 판정 범위

Unity6000.5.7f1, Windows, 원본 실행 Editor 보존. 프로젝트 복사 `Temp/SideQuestQA/Client`, native `-batchmode -nographics -executeMethod ProjectLimitless.EditorTools.SideQuestAudit.RunBatch`.
Editor Asset 검사(9종 존재) 후 Play 상태에서 서비스·실제 Scene/NPC/uGUI/Save·Continue 테스트 실행. Unity Test Framework의 NUnit suite 실행으로 표기하지 않는다. 격리 Runtime에만 runInBackground=true. 화면/포커스 조작0.
격리 Packages는 기존 PackageCache의 같은 버전을 로컬 연결하고 MCP Git 의존성만 제외하여 네트워크 대기를 제거했다. 원본 Packages 변경0. 격리 UserSettings/Search.settings의 indexOnEditorStartup=false로 Unity 검색 초기화 오류를 회피했다. 게임 기능 검사·오류 카운터는 유지.
Save는 격리 SideQuestAuditResults/Saves, 설정은 Settings. 사용자 Save/Settings 사용0.
최종 배치 종료0, 563개 PASS 표식, FAIL0, Runtime Console Error/Exception/Assert0. 반복된 fixture·Scene 표식도 포함한 수이며 독립 테스트 케이스 개수가 아니다.
[원시 결과](QA_증거/SideQuest_20261009/results.txt) · [서비스 결과](QA_증거/SideQuest_20261009/service_results.txt) · [Main18 규칙](QA_증거/SideQuest_20261009/main18_rules.txt) · [밸런스](QA_증거/SideQuest_20261009/balance.txt).

## 실제 자동 검증

| 범위 | 결과 | 검사 내용 |
|---|---|---|
| Quest9 데이터 | PASS | stable ID, 선행, 재료, 보상, 카탈로그 중복 없음 |
| 선행·수락 | PASS9/9 | locked 차단, 선행 완료 후 available, 실제 NPC 선택·거절·수락 |
| 현재 수량 | PASS9/9 | 수락 전 보유 인정, 상점 판매 감소, Changed 알림, 일부 보유 Save 복원 |
| 납품 원자성 | PASS | 부족/잘못된NPC/통화overflow/보상 중첩한도 실패는 소비 없음; 전체 수량 선검사 |
| 정확한 차감·보상 | PASS9/9 | 실제 납품 UI callback, 필요한 수량 차감, 여분1 보존, Talent 정확히1회 |
| 중복 방지 | PASS | 완료 후 재수락·납품 차단, 재로드 반복, 공유재료 합성 의뢰 중복 소비 차단 |
| 동시 진행·추적 | PASS | 9종 동시 수락, Main 추적 보존, Side 선택·Main 복원 |
| QuestLog | PASS9/9 | 실제 Scene 현재 수량 표시; Arbel 생성 연결 누락 수정 |
| Save/Continue | PASS9/9 | 수락·납품 직후 실제 Bootstrap 슬롯 버튼으로 Continue, Quest/Inventory/Talent/Level/EXP 동일 |
| 구버전·5슬롯 | PASS | Version1 합성 구형 데이터/누락필드,5슬롯 쓰기·읽기; 실제 사용자 Save 사용 안 함 |
| 신규 상태 | PASS | Reset 후 선행 잠금; 캐릭터 생성 전체 UI 동선은 미검증 |
| Main01~19 | PASS(서비스 범위) | 기존 정본 목표 이벤트 순서 완료/보상 및 중복 방지 계약; 전체 실제 전투·이동 완주 미검증 |
| Main18 | PASS(규칙 범위) | 기존 RuntimeAudit 과열/냉각약/AI/아트·경로 규칙; 실제 Boss 재전투 미검증 |
| Compile / Console | PASS | 최종 원본 Editor compile error0, 격리 Runtime 오류0 |
| Missing Script | PASS | World_StarterVillage / Arbel / Bootstrap의 전체 root 자식 누락0 |

Main09는 같은 폴 NPC 대상 연속 Talk 목표가 있어 서로 다른 대화 종료를 구분해야 한다. 기존 Notify에는 작업ID가 없으므로 동일 target의 다음 목표에 다시 넣은 이벤트를 중복 이벤트라고 단정하지 않는다. 이 경우 호출자 대화 종료 경계 계약을 기록하고 Main 로직은 보존했다. 기타 목표의 중복 이벤트 회귀 검사는 실행했다.

## 실패·재검증 이력

1. 첫 실행 MCP Git 패키지 다운로드 대기: 해당 격리 프로세스만 종료, 로컬 캐시 전환.
2. Edit 단계 Application.CanStreamedLevelBeLoaded=false로 Save 검사 실패: 테스트를 Play 단계로 이동. 제품 Save 조건 유지.
3. Main09 연속 동일 NPC 목표 테스트 가정 오류: 호출자 대화 경계에 맞춰 수정.
4. C1 Runtime5종 통과 후 Arbel QuestLog Instance null 발견: 기존 자동 생성 조건에 Arbel 추가. Scene/Prefab 재생성 없음.
5. 9종 완료 후 UnityEditor.Search.SearchDatabase 초기화 ArgumentOutOfRangeException1건으로 Console 검사 FAIL. 동일 환경 재실행에도 재현.
6. 격리 프로젝트 검색 시작 옵션만 false로 변경하고 동일 전체 테스트 재실행: 최종 PASS. 이전 FAIL을 PASS로 바꾸거나 오류 필터로 숨기지 않음.
이전 로그는 로컬 Temp/SideQuestQA/Client의 *_attempt.log 및 *_attempt_results.txt에 보존.

## 구현 파일

- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_fence_resin.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_fence_resin.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_forest_first_aid.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_forest_first_aid.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_grave_fragment.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_grave_fragment.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_night_wing_report.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_night_wing_report.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_reinforce_gate.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c1_reinforce_gate.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c2_ash_barrier.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c2_ash_barrier.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c2_ember_residue.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c2_ember_residue.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c2_fissure_repairs.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c2_fissure_repairs.asset.meta`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c2_obsidian_shell.asset`
- `Unity/Client/Assets/_Project/Resources/QuestDefinitions/side_c2_obsidian_shell.asset.meta`
- `Unity/Client/Assets/_Project/Scripts/Core/InventoryService.cs`
- `Unity/Client/Assets/_Project/Scripts/Core/QuestDefinition.cs`
- `Unity/Client/Assets/_Project/Scripts/Core/QuestService.cs`
- `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcRole.cs`
- `Unity/Client/Assets/_Project/Scripts/Player/QuestHudPresenter.cs`
- `Unity/Client/Assets/_Project/Scripts/Player/QuestNavigationPresenter.cs`
- `Unity/Client/Assets/_Project/Scripts/UI/DialoguePresenter.cs`
- `Unity/Client/Assets/_Project/Scripts/UI/QuestLogPresenter.cs`
- `Unity/Client/Assets/_Project/Scripts/UI/SideQuestNpcPresenter.cs`
- `Unity/Client/Assets/_Project/Scripts/UI/SideQuestNpcPresenter.cs.meta`
- `문서/00_프로젝트/Chapter1_Chapter2_SideQuest_Design_20261009.md`
- `Unity/Client/Assets/_Project/Scripts/Editor/SideQuestAudit.cs`
- `Unity/Client/Assets/_Project/Scripts/Editor/SideQuestAudit.cs.meta`

NPC 선택창은 현재 최대4개 의뢰+기존업무+닫기 버튼을 표시한다. 명시적 상하 UI Navigation, 첫 선택, Esc/B 닫기, 기존 확인 버튼, WorldModal·이동 잠금 및 같은 프레임 입력 차단을 사용한다. 향후 의뢰 증가 시 스크롤 검토.

## 보호 및 남은 확인

시작 HEAD f8ca63a, 기존 git status562행(전체 미추적 포함) 기록. 시작3792파일 SHA256 대조에서 기존 변경은 이번 소유 C#8개 및 CURRENT_STATUS뿐. 기존 PNG/WAV/Audio/.meta/Scene/Prefab/Monster·ItemDefinition/Packages/ProjectSettings/사용자 Save/Main20 작업물 보존. Main Complete 원문 유지. TTS 제작·VoiceCatalog 수정0, 잡템 재등록0, Push0.

사용자 확인: 실제 해상도·한글 가독성·아이콘·퀘스트 마커·HUD/로그 갱신의 시각 승인, 키보드/마우스/게임패드 물리 입력, 기존 대화/상점 선택 복귀, 실제 전투 획득부터 납품까지 일반 플레이, Main01~19 전체 이동·전투 회귀. 자동 Runtime은 실제 Button.onClick과 NPC 메서드를 호출했으며 물리 입력·렌더링 검사로 간주하지 않는다.
Chapter2 종료 Lv16~17은 실제 완주 미검증. 기획 문서의 전투 횟수 가정 기반 시뮬레이션만 검증. EXP 곡선/몬스터 EXP 변경 없음.
