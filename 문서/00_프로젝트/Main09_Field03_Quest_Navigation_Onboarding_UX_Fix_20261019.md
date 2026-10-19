# LIMITLESS — Main09 「침묵 속의 재회」 Field03 퀘스트 길찾기/조사 UX 수정 요구

- 요청일: 2026-10-19 / 기획 담당 / `docs/planning`
- **바울 대표 실제 Play 스크린샷 피드백:** “여기서 뭘 어떻게 하라고? 퀘스틀 어찌 하는지 설명 해야 유저가 할거 아니야?”
- **상태:** `USER_REPORTED_BLOCKING_UX / ROOT_CODE_INSPECTED / CODEX_FIX_REQUEST_READY / NOT_IMPLEMENTED / NO_RUNTIME_QA`
- 대상: `main_09_reunion_in_silence` (Main09, **Main18 아님**), `Field_03`; 사용자가 캡처한 QuestLog는 “푸른 빛이 사라진 방향을 따라가세요.” 현재 1/6단계, HUD는 “◆ 현재 목표 푸른 빛의 흔적”만 표시하며 실제 조사 표식·조작법 구분 어려움.
- 정본 GitHub main 실코드: `Resources/QuestDefinitions/Main09_ReunionInSilence.asset`, `Scripts/World/MainQuest09FieldFlow.cs`, `Scripts/Player/QuestNavigationPresenter.cs`, `Scripts/World/QuestNavigationTarget.cs`, `Scripts/UI/QuestLogPresenter.cs`, `Scripts/Player/QuestHudPresenter.cs`.
- 이번 문서는 기획/UX 수정 조건만 기록; **Unity 코드, Scene, Save, 음성/TTS, Git main 실제 변경0**.

## 1. 실제 Runtime 단계·조작·위치 (실코드 기반)

`MainQuest09FieldFlow.Awake` 동적 조사 생성 좌표:
- `field03_main09_blue_trace`: (2.4,-4.1), 표시명 **푸른 빛의 흔적**
- `field03_main09_paul_tracks`: (3.5,-3.1), 표시명 **평행한 바퀴 자국**
- `companion_paul`: (1.8,-1.4), 표시명 **폴**, Talk 2회 + 마지막 1회
- `field03_main09_structural_trace`: (-0.3,-3.3), 표시명 **가공된 석재**

`MainQuest09Interactable.TryInteract`: **타깃 1.6 world units 이내**, `E`/`F`/`Gamepad South A`, 다른 Modal이 열려 있지 않을 때, 대화 시퀀스 완료 시 해당 Objective 1개 진척, 현재 단계 이외의 대상 불가. 폴 3회 대화는 3→4→6번째 단계에서 따로 트리거. Main09 completed일 때 기존 폴 동료 추가/메시지/Save 그대로 보존.

| 단계 | 지금 UI에 명시할 행동 | 실제 Target |
| --- | --- | --- |
| 1/6 | **“숲길의 푸른 흔적 표시까지 이동해 [E/F · 패드 A]로 조사하세요.”** | `field03_main09_blue_trace` |
| 2/6 | **“표시된 평행한 바퀴 자국에 다가가 [E/F · 패드 A]로 조사하세요.”** | `field03_main09_paul_tracks` |
| 3/6 | **“길 앞쪽의 폴에게 다가가 [E/F · 패드 A]로 대화하세요.”** | `companion_paul` |
| 4/6 | **“폴에게 다시 말을 걸어 조사 내용을 비교하세요. [E/F · 패드 A]”** | `companion_paul` |
| 5/6 | **“표시된 가공 석재 흔적에 다가가 [E/F · 패드 A]로 조사하세요.”** | `field03_main09_structural_trace` |
| 6/6 | **“폴에게 돌아가 다음 조사를 상의하세요. [E/F · 패드 A]”** | `companion_paul` |

표시 문구는 UI 폭에 맞춰 짧은 버전과 퀘스트 로그의 긴 버전을 설계하되, 행동(이동/대화/조사)·대상(흔적/폴)·버튼을 잃지 않을 것. 플레이어 Path/성별/동료 편성에 따라 스토리/단서 누락이 없어야 한다.

## 2. 실제 UX 결함

1. **설명만으로 무엇을 눌러야 하는지 불명료:** 로그에는 “방향을 따라가세요”라고 적혔으나 단계 완료는 **실제 위치에서 E/F/A 조사 후 대화 완료**다. “따라가기”를 위치 자동 Trigger라고 오해하게 만듦.
2. **조사 대상이 너무 작음:** 생성된 BlueTrace Sprite 대신 `Mark()`가 만드는 쿼드가 **0.35×0.10 world units**, 기존 녹색/갈색 Field03 숲바닥에서 거의 알아보기 어렵다. 어떤 오브젝트와 상호작용하는지 보이지 않음.
3. **목표 위치와 상호작용 실점 위치 오차:** `Site()`는 `QuestNavigationTarget.Attach(site, ..., new Vector3(0, 1.45f, 0))`를 이용해 **실제 조사점보다 1.45 world units 위**에 HUD 목표 앵커를 둔다. 화면 캡처에서도 큰 “◆현재 목표 푸른 빛의 흔적” 패널만 떠서 **표식이 있는 정확한 발밑 지점이 흐려짐**. HUD/마커 UI는 정확한 상호작용 중심을 알려주되 캐릭터를 가리지 않아야 함.
4. **단계3/4/6에서 동일 폴에 여러 번 말거는 이유 미표시**. 폴이 처음 말한 직후 다음 단계에서 폴이 그대로 타깃이면 퀘스트가 멈춘 것처럼 느껴짐. “폴에게 다시 이야기” 등 명시·Quest Changed 이벤트 직후 새 목표 짧은 안내 필요.
5. `QuestNavigationPresenter` 화면 밖 거리 표시는 **카메라 Transform과 타깃 사이 거리**로 계산(`camera.transform.position`의 z오프셋 포함). 플레이어로부터 실제 평면거리와 무관한 `m` 안내가 될 수 있음. **월드 XY 플레이어↔목표 거리**를 사용해야 한다. 화면 안 마커의 표시는 주목성을 유지하되 너무 큰 패널이 캐릭터·조사점/몬스터/초상화/다른 HUD를 가리지 않아야 한다.
6. `QuestLogPresenter.BuildDetails`는 설명·목표 1줄만 표현. 추적 버튼 `T/패드 Y`는 있으나 “파란 목표 패널에 가까이 가서 상호작용” 흐름을 설명하지 않는다. 추적 off/다른 side quest 추적/Field03 진입/Save·Continue 직후에도 조작법을 몰라도 찾아갈 수 있어야 한다.

## 3. 변경 기준 — 짧은 가이드 + 비침습 탐색 표시

### 필수 P0: 문구·버튼·도착 지점

- Main09 여섯 목표 안내에서 **무엇을/어디로/어떤 키**로 행동할지 구체적으로 제시.
- 월드 HUD는 현재 Objective의 **“1/6 · 푸른 흔적 조사 / 가까이서 E·F·패드 A”**처럼 두 줄 이하의 간단한 정보(키보드·패드 둘 다)와 단계 진척 정보를 표시. 정확한 목표에 접근하면 버튼 안내가 추가로 확실히 보이도록 한다. 최초 1회 `“파란 목표 표시에 다가가 E/F(패드 A)로 조사하세요.”` 간단 튜토리얼 토스트; 오래 화면 가리거나 반복 팝업X.
- **타깃의 발밑/상호작용 Collider 위치**를 오해 없이 볼 수 있는 바닥 링/아이콘(고대비·규칙적인 펄스는 광과민 보호; 과한 번쩍임·진동X) 추가. 파란색만으로 구분하지 말고 “조사” 텍스트+기하학 아이콘. 몬스터/NPC보다 항상 앞으로 나오는 큰 말풍선/전투 타깃 가림은 금지.
- `QuestNavigationPresenter` onScreen/edge 방향 화살표는 유지하고 **정확히 액션 대상 좌표**를 가리키도록 anchor offset/화면 마커 위치 수정. 월드 바닥 링을 Nav overlay와 함께 사용 가능하되 한 픽셀 찾기를 강요하지 않을 것. 오프스크린 거리 계산 XY 플레이어 기준.
- 스토리/Quest stableId/ObjectiveType/TargetId, 대상 월드 좌표·상호작용 반경 1.6, Quest 완료 조건·보상(경험35·탈렌트35)·폴 동료 가입·대사/TTS/Save 저장·몹 스폰·씬/동선은 **기본 보호**. 필요하다면 버튼 탐색 지원을 확대하는 변경은 QA 입증 후 별도 보고.

### P1: 앱/접근성 개선(범위와 리그레션 주의)

- `QuestLog`에서는 **현재 단계 1/6과 조작 안내를 동시에** 표시하고, 이미 제공된 `[T/패드Y] 추적`을 유지; 추적을 별도 클릭하지 않아도 Main09 자동추적이라면 그 상태와 안내 일관.
- NPC 폴이 3/4/6목표에서 대화를 한 번 마치면 상단 퀘스트 알림이 **“다시 폴에게 대화”**로 바뀌어야 한다. Modal/QuestLog가 닫힌 뒤 navigation marker와 실제 조사점/새 목표가 즉시 갱신돼야 한다.
- 마우스를 발로 사용하거나 버튼 입력에 어려움이 있는 플레이어를 포함해 큰 판독 UI·작은 대상 클릭 금지·이미 설정된 게임패드/키보드 단일 버튼 지원. 시각·청각만으로 목표 파악 요구X.

## 4. 필수 검증 시나리오

- 사용자가 실제 제시한 **Slot01 Field03 Main09 1/6** 상태로 동일 장면 격리 재현. 화면의 월드 목표표식/퀘스트 로그/이동/키보드 E/F, 게임패드 A를 캡처한 16:9 화면 Before/After 비교.
- 1→2→3→4→5→6→QuestComplete 실세계 좌표·대화 종료 후 단계 전환, 동일 폴에 다시 말을 걸 때 동작, Cancel/Esc/거리 이탈/Modal/QuestLog 추적 on/off/다른 side quest 추적/전투·Scene reload/SaveContinue. 진행 순서 및 보상 **한 번만** 지급, 폴 동료 등록 이상X.
- 16:9/4:3/21:9 해상도에서 목표 마커가 NPC/플레이어/몬스터/EXP HUD와 겹치지 않는지 스크린샷, offscreen arrow의 XY 거리/방향, 퀘스트 로그 열기 Q→닫기/Escape→실제 조사, Button A/E/F 단일 입력.
- 휠체어/도보 Collider·접근 경로/도착 1.6 반경, 폴 관련 다른 Main08/Main10·기존 음성/Save·Main18 승인 세린10·Field05 v2 격리·Arbel/Field04 공식 아트 변동0.
- Unity 실제 비교 QA·신규 Console 0, Search/URP 기존 환경 예외는 본래 baseline으로 별도 분리. 원본/스피커/OS 수동 입력 미검증은 명시적으로 `PARTIAL`.
- **원본 Unity 활성 Editor/Play/Save·dirty 보호**, 분리 worktree, 처음에는 씬 직접 수정하지 않는 최소 코드/Quest text 변경 우선. 현재 Codex 5시간 잔여3%였으므로 바로 착수 중 5시간 한도 도달 시 인계·안전 checkpoint.

## 5. 담당·승인

**Codex:** Field03 Main09 안내 UX 개선을 전용 `codex/main09-quest-clarity-20261019` 브랜치로 설계/최소 수정/QA; 원본 활성 Unity 작업 보호, 원격 main 직접 PushX. 바울 대표 실제 캡처와 개선 비교 화면 제출. **아직 코드/Unity/실사용 게임 수정 완료 아님**.

**기획:** `docs/planning`에 UI 텍스트·QA 요구 문서화. 추후 성공하는 공통 퀘스트 안내 패턴은 Main01~21 및 Chapter3에 확산 검토하되 다른 퀘스트를 대량 자동 일괄 수정하지 않는다. **Main09 먼저 정확히 해결**하고 사용자의 납득/승인 후 확장한다.


## 2026-10-19 사용자 최신 우선순위 — Main09 수정 계속, Field05 별도 플레이 검수 취소

바울 대표는 Field05 B42 아트를 보고서 기준으로 **최종 미술 승인**했으며, 해당 리뉴얼을 직접 돌아보려다가 **Main09 Field03 1/6의 목적지·상호작용 안내 문제 때문에 게임 진행이 막힌 것**임을 명확히 함. **퀘스트를 우회하고 Field05에 강제 입장시키는 별도 플레이형 검수 빌드/개발용 텔레포트는 만들지 않는다.** Codex 5시간 한도에 걸렸으므로 초기화 후 **기존 Main09 Quest UX 수정 지점부터 재개**한다.

최우선: Main09 1~6단계에서 현 위치·대상·조작키가 명확히 보이도록 실제 안내/퀘스트 로그/동적 마커 일치 수정, 일반 유저가 **정상 퀘스트를 진행해** 이후 콘텐츠에 도달하도록. Field05의 최종 아트 승인에 대한 추가 QA 자료/체험 서버를 이유로 Main09를 밀어내지 않는다. Main09 개선 검증 없이 다른 모든 Quest를 일괄 교체하지 않는다.

현재 상태: `MAIN09_UX_FIX_PRIORITY_1 / CODEX_QUOTA_RESET_PENDING / FIELD05_VISUAL_APPROVED / FIELD05_APPLY_LATER`. 이 추가 기록은 기획 승인/순서 변경이며 **아직 Main09 Unity 수정이 실행된 것 아님**.
