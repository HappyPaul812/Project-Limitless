# Chapter3 Main22 「북풍의 징후」 첫 플레이 구현·변경분 QA

2026-10-21 / 격리 구현 완료 / **136 PASS · 0 FAIL**

작업 브랜치: `codex/chapter3-main22-first-playable-20261021`.
근거: GitHub Issue #44 및 `Chapter3_Main22_FirstPlayable_IsolatedImplementation_Order_20261021.md`. 최종 커밋은 CURRENT_STATUS와 CODEX_LATEST에 기록합니다.

## 구현 결과

1. Main21 완료 Save에서 Main22가 Available이 되며, 마을 북문의 **직접 출발 선택**으로만 시작합니다. 취소·접근만으로 자동 출발하지 않습니다.
2. F1의 얇은 서리를 조사합니다.
3. 남쪽으로 향하는 동물 흔적 **서로 다른 두 곳**을 조사합니다. 같은 흔적 중복 조사로 진행되지 않습니다.
4. 승인 서리들쥐 1기 지정전투에서 승리합니다. 도주·패배는 진행으로 계산하지 않으며 안전 복귀 후 재도전할 수 있습니다.
5. 안전 쉼터에서 조사 결과를 정리하면 Main22가 완료됩니다. Main23은 시작하지 않습니다.

하단 퀘스트 안내에 목표·방향·거리·조작법을 표시하고 이동/조사 버튼을 제공합니다. 클릭 이동은 실제 Collider를 확인하며 수동 입력·대화가 개입하면 중단합니다. Main21 이전에는 새 이동 기능이 개입하지 않습니다. 마을 복귀, 각 단계 Save/Continue, 기존 선택적 펫 서비스도 유지합니다.

## 변경 범위와 계약

[제품 변경 파일 58개](Evidence/Main22FirstPlayable20261021/ChangedFiles.json): F1 Scene·승인 환경6종/Tile, Main22 Quest/Encounter/기존 시각용 몬스터 복사본, 진행·안내·이동·전투 연결 및 격리 제작/검증/플레이 준비 도구입니다. 이 중 승인 서리들쥐 선행 연결 13개 파일은 기존 승인본을 그대로 재사용합니다. 다른 사용자 변경 사본은 선택 커밋에서 제외합니다.

| 구분 | 확정한 고유 ID |
|---|---|
| Quest | `main_22_northwind_signs` |
| Scene | `World_NorthF1` |
| 지정전투 | `northf1_main22_mouse_encounter` |
| F1 진입 Spawn | `Spawn_NorthF1_FromStarterVillage` |
| 마을 복귀 Spawn | `Spawn_StarterVillage_FromNorthF1` |
| 쉼터 Spawn | `Spawn_NorthF1_Shelter` |

실제 정의와 충돌 여부를 확인했습니다. Main21 Quest/대사/흐름/기존 마을 Scene, GameSaveService, 펫 서비스 등 보호 대상 8개 파일은 원본과 바이트 동일합니다. SaveVersion 1을 유지하고 Main22 흔적 비트 필드만 선택적으로 추가하여 옛 Save를 읽습니다.

전투 수치는 기존 시각 검증용 정의의 Level1/HP60/AttackPercent100/Agility10을 **임시 상속**했습니다. 신규 공식 밸런스 확정이 아닙니다. EXP·Talent·드롭·퀘스트 보상은 0이며 새 아이템/스킬/소모품을 만들지 않았습니다. 조사 대사는 화면에도 임시 대사로 표시합니다. 정식 대사·밸런스·보상은 후속 기획 확정 대상입니다.

## 변경분 QA

| 검증 묶음 | PASS | FAIL |
|---|---:|---:|
| 실제 PlayMode 5목표 완주·Save·도주/패배/재도전·복귀·통행 | 111 | 0 |
| 실제 GameView 3화면비·HUD 이동/조사 | 11 | 0 |
| 실제 서리들쥐 1기 전투 화면·명령 턴 | 9 | 0 |
| Main21 전 잠금·직접 북문 선택·시작 표본 | 5 | 0 |
| 합계 | **136** | **0** |

기존 F1 Unity64 PASS / 서리들쥐 Battle40 PASS는 재사용했으며 전체 검사를 반복하지 않았습니다. 걷기·휠체어 통행, 선택 전 시작 방지, 조사 순서/중복 방지, 무관한 처치 무효, 각 단계 Continue, 전투 도주/패배 후 안전 복귀와 승리, 무보상, 쉼터 완료 후 재처리 방지, 마을 왕복을 확인했습니다.

최종 배치 로그는 컴파일 오류·크래시 없이 정상 종료했습니다. 기존 SearchDatabase/URP Host 환경 예외는 제품 오류와 구분했습니다. 초기 검증 도구의 컴파일·턴 준비 대기·GPU 캡처 버퍼 오류는 수정 후 관련 검사를 통과했으며 최종 결과에 실패로 남아 있지 않습니다. [검사 목록·로그·SHA·도구 이력](Evidence/Main22FirstPlayable20261021/Result.json).

## 실제 화면

다음은 **실제 Play Mode GameView 렌더링**입니다. 숨겨진 메모리 HostView에서 Native Camera/GUI를 렌더링했으며 OS 포커스를 변경하지 않았습니다. 오프라인 합성이 아닙니다.

- [16:9 · 퀘스트 목표/방향/조작 안내](Evidence/Main22FirstPlayable20261021/GameViews/Main22_GameView_1280x720.png)
- [4:3](Evidence/Main22FirstPlayable20261021/GameViews/Main22_GameView_960x720.png)
- [21:9](Evidence/Main22FirstPlayable20261021/GameViews/Main22_GameView_1680x720.png)
- [마을 북문 직접 선택](Evidence/Main22FirstPlayable20261021/GateView/Main22_NorthGate_ActualGameView.png)
- [승인 서리들쥐 1기 실제 전투](Evidence/Main22FirstPlayable20261021/BattleView/Main22_SingleMouse_ActualGameView.png)

Playthrough 폴더의 `ActualUnityCameraRT` 이미지는 실제 Unity Camera RT와 실제 uGUI를 캡처한 별도 자료입니다. GameView 창 캡처와 구분하며 오프라인 합성은 아닙니다.

## 대표님 직접 플레이 준비

[격리 프로젝트 실행 안내](Main22_Isolated_Play_Guide_20261021.md). 격리 프로젝트에 **5번 슬롯 시작 표본**을 준비했습니다. 기존 Main01~21 완료 상태의 합성 검증 Save이며 사용자 원본 Save가 아닙니다. 시작 시 Main22는 미시작 상태입니다. 테스트용 Level14/기존 동료/통화77은 공식 신규 보상이 아닙니다.

대표님은 격리 프로젝트를 수동으로 열어 Bootstrap Play → 5번 슬롯 이어하기 → 북문 직접 출발 후 5목표를 확인하시면 됩니다. 직접 플레이 감상과 임시 대사·밸런스·보상 확정이 남아 있습니다. 원본 적용/PR/병합은 별도 후속 범위입니다.

## 보호 결과

원본 Assets/Packages/ProjectSettings/UserData **4,351개 SHA 동일**, 원본 Git status/HEAD 동일. 원본 Editor는 Bootstrap/Play OFF/비포커스 상태를 유지했습니다. [보호 기록](Evidence/Main22FirstPlayable20261021/Protection.json).

**원본 정식 반영 미실행. main 직접 커밋·Push·병합 모두 미실행.** 신규 아트·Main23·한기/동상·보스·TTS/BGM 추가도 없습니다.
