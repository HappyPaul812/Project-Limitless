# Main15 Runtime QA — 2026-09-30

## 환경과 검증 방법

LOCAL Unity `6000.5.7f1`, 시작/종료 Scene `Bootstrap`, 기존 Scene 저장 상태 clean. Main12~14 전체 회귀 없이 Main14 완료·Lv11 수호자·선택 동료 미엘/폴·세린 미해금 상태를 QA 선행 상태로 준비했다. 기존 `AuditSaveDirectory`로 사용자 슬롯과 분리했다. 실제 이동 입력 전체를 수동 플레이한 검증은 아니다. QA가 플레이어를 목표 근처에 배치한 뒤 실제 출구/몬스터 Collider, 조사·NPC 대화 처리와 마지막 페이지 콜백을 실행했다.

전투는 실제 Battle의 공격 버튼·합법 대상 선택 또는 기존 스킬 메뉴·대상 선택을 사용했다. 적 HP/아군 HP를 강제로 0으로 만들거나 EndBattle/RecordVictory로 승리를 주입하지 않았다. M15-01 이후 긴 전투는 TimeScale 4로 진행하고 원복했다. M15-03/04 시작 전 파티 자원 완전 회복은 QA 준비다. 전멸은 회복약 1회와 방어 명령 뒤 적 AI 피해로 발생시켰다. 모든 임시 명령 드라이버는 메모리에만 설치했고 종료했다.

기능 검증은 PlayUnfocused였다. 사용자가 이번 작업의 Game View 시각 QA를 명시적으로 승인한 뒤 캡처했다. 실제 캡처 해상도는 1016×569이며 3개 화면 비율의 UI 시각 전수 검증은 아니다. 카메라 비율 검사는 런타임 Camera.aspect와 viewport 계산으로 수행했다.

## Main15 진행과 연계 결과

| 항목 | 실제 결과 |
| --- | --- |
| 시작 | Available에서 레온 대화 마지막 페이지 뒤 Active, 목표 1 완료 |
| 12개 순차 목표 | 의뢰→진입→그을음→발자국→지정 승리→울림→균열→열 흔적→세린 정리→귀환→레온 보고→세린 합류로 완료 |
| 순서·중복 | 그을음 전에 발자국 조사, 완료된 그을음 재조사 모두 진행 변화 없음 |
| Navigation | 현장 조사 목표와 첫 조우 등록, 귀환 후 레온/세린 안내 확인. 전체 화면 밖 방향·거리 정밀 회귀는 제외 |
| Arbel→Field06 | 실제 북서 출구 접촉, Field06 `(8,0)` 도착 및 도착 목표 완료 |
| Field06→Arbel | 실제 동쪽 출구 접촉, Arbel `(-5.55,5)` 도착 및 귀환 목표 완료 |
| Spawn 분리 | 양방향 즉시 역전환 없음. Field06 Spawn은 Exit bounds 밖, 최근접 간격 약 2.19 |
| Safe Zone | Field06 별도 거점 없음, `safezone_arbel` 유지 |
| 실제 전멸 | M15-02 적 AI 공격으로 전멸, Arbel 중앙 `(0,-1.5)` 복귀. Dungeon01 복귀 없음 |
| 전멸 자원·진행 | Player 154/154, 미엘 104/104·MP44, 세린 108/108로 회복. 귀환 목표 index9·기존 승리 기록 유지, 회복약 3→2 유지. 패배 조우 재도전 가능 |
| M15-01 | 그을음들개 135HP, 일반 공격 명령 10회로 승리, 지정 목표 index4→5 |
| M15-02 | 그을음들개135+열풍매110. 일반 공격만으로 재시도한 분기는 전멸, 철벽/치유/습격을 포함한 정상 명령 22회로 실제 승리 |
| M15-03 | 균열도마뱀190, 정상 공격/스킬 명령 16회로 실제 승리 |
| M15-04 | 화열딱정벌레220+열풍매110, 정상 공격/스킬 명령 33회로 실제 승리 |
| Victory/Pet 목록 | 네 종 실제 승리 뒤 네 후보가 등장. 패배한 M15-02는 열풍매 승리를 기록하지 않았음 |
| 구매 | 기존 미해금 그을음들개를 분양 UI 버튼으로 구매, 269→169 탈렌트. 영구 해금·중복 구매 거절 확인 |
| 세린 영구 합류 | 마지막 대화 callback 뒤 Main15 Completed, Story Temporary false, 세린 해금 true |
| 기존 파티 보존 | 저장 선택 동료 미엘/폴 유지. 임시 전투는 Player+미엘+세린, 영구 합류 후 강제 교체 없음 |
| Party Manager | 세린이 미선택 상태로 등장, 사수/청각의 길/Fox 표시. 이미 동료2명일 때 세 번째 선택 거절 |
| 세린 데이터 | `companion_serin`, `sharpshooter`, `path.hearing`, 기본 `fox`, 기존 공용 스킬/Beast 시스템 |
| 공식 외형 | World는 `Art/Characters/Serin/Serin_Sprite_Sheet.png`, Portrait는 `Art/Portraits/Serin_Portrait_InGame.png` |
| 대사 | 장치·지면 진동의 방향/느슨한 반복 분석과 정확한 위치 불확실성 유지. 연민/초능력/완전 청력 회복 표현 없음. 태온/미엘/폴/세린 말투 유지, 폴–세린은 건조한 짧은 농담 |

## Save → Continue

실제 Save 파일을 만든 뒤 Bootstrap의 해당 슬롯 `이어하기` 버튼을 사용했다. 완료 직전 `welcome_serin`(index11)과 완료 상태를 각각 확인했다. 수정 전 Arbel 좌표 복원이 실패했고 아래 최소 수정 후 재검증했다.

- 완료 직전: Arbel `(2,-4)` 정확히 복원, index11·임시 세린·영구 미해금·Fox·Arbel Safe Zone 유지.
- 완료 후: Arbel `(1,-4)` 정확히 복원, Main15 완료·영구 세린·임시 종료·Fox·미엘/폴 편성·네 종 Victory·그을음들개 해금·잔액219 복원. 잔액219는 구매 후169에 실제 Main15 완료 보상50이 더해진 값이다.
- Field06: `(5,-2)` Save/Continue 정확히 복원, Safe Zone Arbel 유지.
- 수정 후 정상 왕복 Spawn 및 기존 전멸 복귀 서비스의 중앙 Spawn도 재확인했다. 수정 후 복귀 서비스 직접 호출은 새 실제 전멸 검증으로 계산하지 않는다.

## 재현한 버그와 최소 수정

| 버그 | 원인·수정 | 재검증 |
| --- | --- | --- |
| Arbel 유효 저장 좌표가 출입구 Spawn으로 대체됨 | `Chapter2IntroFlow.Start`에서 Bounds를 늦게 생성해 위치 저장기의 Awake/Start에서 Bounds를 못 찾음. Arbel Bounds를 flow의 Awake에 준비하고 `WorldPositionSaveController`를 일반 Spawn Start보다 앞서 실행 | 동일 Continue에서 `(1,-4)` 및 `(2,-4)` 정확 복원, Field06 위치·왕복·중앙 Spawn 정상 |
| Field06 북쪽 비출구로 맵 밖 이동 | 복제 Scene의 북쪽 중앙 출구 틈이 남음. 동쪽 출구는 유지하고 북쪽 중앙 Collider 1개만 추가 | 같은 Rigidbody MovePosition 이동: 수정 전 `(0,14.5)` 밖, 수정 후 `(0,6.7)`에서 차단. 동쪽 실제 왕복 유지 |

수정 commit: `cfadcc8`(위치 복원), `1e5cb54`(북쪽 경계). 저장 Version·대사·몬스터 수치·Sprite/이미지·기존 Scene Asset을 변경하지 않았다.

## Camera / 시각 QA와 한계

- Field06 World Bounds 21×15. 16:9·16:10·4:3 각 네 모서리에서 카메라 viewport가 Bounds 안에 들어옴을 확인했다. 좌/하/동쪽 상하 경계 Collider와 수정된 북쪽 틈을 확인했다. 모든 지형과 전 구간의 실제 입력 경로 탐색은 미수행이다.
- 실제 캡처에서 전체 배치·공식 세린 World Sprite·몬스터 상대 크기·동쪽 진입부·조사 목표 Navigation·분양 목록을 확인했다. 서쪽으로 마른 색과 그을음이 늘지만 직사각형 환경 Patch가 드러나 초기 배치 품질이다. 작은 현장 TextMesh는 가독성이 약하며 큰 Quest Navigation은 읽을 수 있다. 미관 개선을 임의로 구현하지 않았다.
- 실제 승리 뒤 화열딱정벌레/열풍매의 최종 KO index15가 유지되는 것을 런타임 Image와 캡처로 확인했다. 승리 결과 패널은 QA 캡처 동안만 숨겼다 원복했다. KO 전 구간의 실시간 시각 판단은 미검증이다.
- 세린 Battle 외형은 현재 코드의 공식 **Portrait fallback**이다. 전투 전용 Sprite/Animation은 별도 제작하지 않았다. 같은 후열의 미엘과 부분적으로 겹치는 화면을 확인했으며 전투 외형/레이아웃 개선은 별도 후속 검토다. World Sprite 연결 실패로 오인해 원본을 교체하지 않았다.
- NPC/Player 전 위치의 겹침, 출구 미술 표식, 모든 비율의 UI와 모든 Monster Animation·Skill 수치는 전수 검증하지 않았다.
- 로컬 캡처는 `Unity/Client/UserData/Main15QA_20260930/Captures/`에 보존한다. 사용자 저장·QA fixture JSON은 포함하지 않으며 캡처는 Git commit하지 않았다.

## Console / 정리

게임 컴파일 오류와 기능 검증 중 게임 코드 Error/Warning은 0건이다. 재컴파일 시 MCP WebSocket 미초기화 Warning 1건을 별도 확인했다. 마지막 캡처 이후 MCP `ScreenshotUtility.cs:197`의 `EditorApplication.Step()`에서 PlayerLoop 재귀 호출 Error 5건이 생겼다. 캡처 도구 오류로 기록하고 추가 캡처를 중단했다. 외부 Package 원본은 수정하지 않았다. 이 기록을 남긴 뒤 Console을 정리하고 Edit Mode 상태·Console을 다시 조회했다. 정리 뒤 0건이라는 값은 캡처 오류가 발생하지 않았다는 뜻이 아니다.

원래 사용자 저장 슬롯 파일 목록/SHA-256은 전후 동일하다. 감사 폴더/선택 슬롯/Session을 해제하고 TimeScale과 Game View Play 설정을 원복했다. Bootstrap clean Edit Mode로 종료했다. 관련 변경의 `git diff --check` 통과. 기존 사용자 변경은 stage/삭제/원복하지 않았으며 GitHub push 없음.

## 문서 정합성·다음 작업

기존 BeastCompanion 설계의 “Chapter2 시설·세린 미구현” 및 안전지대 문서의 “필드 복귀/필드 없음” 구간은 과거 상태 설명이 최신 LOCAL 구현과 다르다. 현재 구현은 최신 CURRENT_STATUS·Chapter2 도입/Main15 문서와 코드를 기준으로 확인했으며, 별도 문서 감사 없이 기존 설계를 임의 변경하지 않았다.

다음 권장 작업은 Field06 미술·현장 표식 가독성과 세린 전투 외형/후열 겹침에 대한 별도 확정·개선, 실제 이동 입력의 장애물/조사 접근 경로 검증, KO 전환 시각 확인이다. Chapter2 몬스터 정밀 QA는 별도 작업으로 남긴다. TTS 다음 실제 제작은 [정식 정책](../00_프로젝트/TTS_음성_제작_정책.md)에 따라 Intro부터 Free Tier로 시작한다.
