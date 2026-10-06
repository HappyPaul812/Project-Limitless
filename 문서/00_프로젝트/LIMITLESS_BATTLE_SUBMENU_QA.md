# 전투 하위 메뉴 기본 명령 숨김 및 통합 회귀 — 2026-10-06

## 적용 계획

LOCAL `5a4d05a`와 현재 작업 트리 기준. 기존812 PASS는 생성50/대표5·일반던전8·Quest103/Main09~16·Main03/04 기술Voice 및 Beast 구조의 재사용 근거다. 변경하지 않는 데이터를 전수 재검사하지 않는다. 문서화→최소 구현→백그라운드 영향 검증 순서이며 사용자 변경103항목과 PNG/WAV/사용자 Save·설정은 보존한다.

현재 BattleSceneController는 스킬/아이템 메뉴에서 기본5명령을 interactable=false로만 만든다. 이번에는 기본5 GameObject를 비활성화하여 렌더링·GraphicRaycast·Selectable/Navigation 후보에서 함께 제외한다. 명령 패널 전체는 끄지 않아 메시지·취소·스킬 메뉴를 유지한다. 기본명령 포커스는 먼저 비우고 메뉴의 첫 유효 버튼으로 이동한다. 하위 대상 선택·Invalid Action·실제 행동 중에도 숨김을 유지하고, 취소/뒤로가기 또는 다음 조작 가능한 아군 명령 시 활성화·interactable·기존 포커스를 복원한다. 수치·스킬·Beast/Story/Save 기능은 바꾸지 않는다.

## 영향 검증 계획

기존 격리 Save/Settings·PlayUnfocused launcher를 재사용하는 별도 QA로 실제 Battle 생성 상태와 정상 입장 연출 종료 후 검사한다. 5직업의 메뉴 열기/닫기 반복, 실제 EventSystem 방향/Submit/Cancel 경로와 Raycast 후보, 대상 취소·무효 행동·유효 Skill/Item 사용·명령 복구를 검사한다. 기본 공격/방어/도망 및 일반 정상 승리/결과 복귀·Continue, 전멸 SafeZone 대표를 추가한다. UI3해상도 background 캡처를 확인한다. 물리 Keyboard/Gamepad는 USER_INPUT_REQUIRED이며 가상 이벤트를 실물입력 PASS로 표현하지 않는다.

Main03/04는 기존 기술QA/보호Voice 원본 근거를 재사용하고 가능하면 실제 authored Dialogue→Story Battle→후속 대화 대표를 재생한다. 기존 의미PASS23/Voice청취219/Main04재생성5는 보존하고 의미 청취/TTS생성은 하지 않는다. Main09~16 전체 재실행 없이 공용 Battle/Scene/Save 영향 대표를 확인한다. DEFERRED_FEATURE1/DEFERRED_RELEASE_VALIDATION1/USER3 잔여목록 유지.

## 결과

## 결과 — 구현·통합 영향 회귀 완료

이번 고유 판정ID **484: PASS482 / FAIL0 / NOT_VERIFIED0 / USER_INPUT_REQUIRED2**. 같은ID 재확인은 마지막 결과 하나만 CSV에 집계, 원 Runtime 로그는 실행 이력이다. 기존812 PASS는 이력 재사용이며 이번 수치에 합산하지 않는다. 기존817 CSV/JSON은 변경0. 변경 UI/대표전투에 관한 옛 근거는 이번 QA가 갱신하며 생성/50외형/Path/Job/Field상호작용/Main09~16/Beast/Save 정본은 변경 없이 과거 PASS를 재사용한다.

**구현:** BattleSceneController.commandMenuHidden 및 SetCommandButtons에 기본5명령 GameObject 비활성·interactable 차단·선택 포커스 해제를 함께 처리. 부모 CommandPanel은 유지해 메시지/취소/하위 메뉴를 보존한다. 활성화된 Selectable Navigation과 Graphic Raycast에서 기본 명령이 제외된다. 대상 선택·Invalid Action·실제 행동 중 숨김 유지, 취소/돌아가기 및 다음 조작 가능한 아군 명령에서 복원한다. 이전 선택 강조는 SetSelectedGameObject(null)/Selectable.OnDisable로 정리한다. 한 메서드 안에서 동기적으로 전환하며 개별 UI 표시 사이에 프레임 대기가 없다.

**추가 코드 결함1:** 기존 Battle Update는 키보드가 없으면 return하고 Esc만 확인해 패드 B 취소 경로가 없었다. Esc/B를 같은 CancelCurrentSelection으로 연결했다. 실제 UI Cancel Action은 */{Cancel}, 로컬 공식 Gamepad.buttonEast/Keyboard.escape는 Cancel usage를 선언한다. 실제 기기 B 동작은 USER_INPUT_REQUIRED이며 static/native 연결 및 UI button 취소 PASS로 대체하지 않는다.

### 새 실행 근거

- 5직업: 스킬 열기/취소 반복, 기본5 비활성+입력 불가, 숨긴 Button Submit 거절, Raycast 후보 제외, 활성 Selectable 4방향 목적지 및 방향 이벤트/원래 Focus 복구 PASS. 각 첫 스킬 실제 선택/대상 취소→메뉴/정상 사용→다음 명령 복구. 기존15스킬 수치 전수는 재사용한다.
- Item: 목록/대상 선택/대상 취소/최대HP 거절/메뉴 복구/회복 실제 사용·수량1 차감/기본명령 복구 PASS. 빈 목록의 ItemBack 및 SkillBack PASS. 전환 뒤5회 표본에서 숨김/복구·포커스 유지, 별도 모든 프레임/모든 물리기기의 무깜빡임 전수 판정은 아니다.
- 1920×1080/1600×900/1280×720: 실제 background Skill3/Item3 캡처에서 뒤 기본 버튼 없음·주요 문구/취소/하위목록 잘림 없음. 기존Art를 수정하지 않고 Temp에 새 QA 캡처만 생성했다.
- 실제 일반 Dungeon_01 Encounter 서비스→Battle→Item 열기/취소→Flee→Dungeon 복귀, 재조우→정상Attack 승리→VictoryReturn→Dungeon→격리Save→Bootstrap Continue PASS. 적 HP/EndBattle/강제승리 주입 없음. 실제 물리 접촉은 기존USER 유지.
- Main03/04: 저작된 First/AfterBattle 배열을 실제 DialoguePresenter로 기술 재생해 전체 자막·Portrait·정본Voice Clip/Player 무음 확인. 실제 Story 전환 서비스→임시2/3인 편성→스킬/아이템 숨김·취소→정상Attack 승리→결과버튼 Field_01 복귀·영구Roster 보존 PASS. 자연 Quest/NPC 전체 재생 대신 대표 통제 Story fixture이며 이전 기술QA/사용자 의미PASS를 재사용한다. 의미 청취/TTS0, 알려진 Main04 의미불일치5는 기술Clip PASS와 별개다.
- 전멸: 메뉴 사용 후 의도적 아군KO fixture→최근Field03 안전지대/HP·MP 회복/Level12·EXP23·Talent123→실제 Continue PASS. 전멸용 KO를 정상승리로 기록하지 않는다.
- Beast는 기존 구조/Save 정본 보존·대표 Continue 정확 복원 확인, 신규 종/패시브/장착 구현0. Sprite/생성50 및 Main09~16 전체를 반복하지 않았다.

### 실패 이력 구분

초기 QA enum이 실제 RecoverHp 대신 RestoreHp로 작성돼 컴파일 오류가 있었으나 실행 전에 수정·재컴파일 성공. QA Continue 버튼 경로 오기(Slot01Action→실제 StartMenuCanvas/Slot01/Action), 입구 가상조우 위치에 복귀이격을 더해 마을Exit 통과한 Fixture, Cancel binding에 기기별명시 path만 기대한 Fixture를 근거 확인 후 보완했다. 실제 메뉴/전투 결함 FAIL은 남지 않았다. 실패원본은 Temp/BattleSubmenu20261006에 보존하고 성공한 묶음은 반복하지 않았다. 실패 Story 복귀를 재현하려고 최소 정상승리만 다시 필요했으며 Main03 첫대화 등 완료한 하위검사는 SKIP했다.

### 보호·잔여·종료

기존812/817 이력 재사용, 최신통합QA는 별도 결과. USER_INPUT_REQUIRED는 물리Keyboard/Gamepad2(이번QA)이며 프로젝트 잔여는 Dungeon접촉 포함3. DEFERRED_FEATURE1=일반Equipment장착미구현, DEFERRED_RELEASE_VALIDATION1=Standalone Build/재시작. [잔여목록](LIMITLESS_SECOND_QA_DEFERRED.md). Voice Queue219/Main04재생성5 유지. 창 활성화/GameView 활성/OS입력0·사용자Save/설정/PNG/WAV변경0·새기능중복0·Push0.

Compile Error0/Console Error0·기존CS0618 Warning16·이번 helper 신규Warning0. 종료 Bootstrap clean EditMode·is_focused=false·AuditSaveDirectory null·slot0·runInBackground 원복. 시작보호3075 파일 비교는 과거PartyFix와 이번Battle 코드2만차이·누락0; 이번기능게임코드변경1. 캡처6PNG/초기실패/무결성 파일은 Temp이며 commit하지 않는다.

[고유 판정 CSV](LIMITLESS_BattleSubmenu_Cases.csv) · [실행 이력](LIMITLESS_BattleSubmenu_Runtime.txt). 선행관련commit5a4d05a, 이번commit최종보고참조. 관련게임코드1/EditorQA+meta2/보고서·결과·CurrentStatus·전투문서만 Stage, 기존103작업보존. 이번commit diff --check 오류0, 기존unrelated 작업의공백 경고와구분.


## 체크포인트1
5직업 Skill·Item 숨김/입력/Navigation·취소/사용·Guard·일반Flee/정상Victory·결과버튼복귀까지 PASS. 마지막Continue의 QA 버튼이름 Slot01Action은 실제 Action 오브젝트와 달라 helper 오류 발생. 게임 오류가 아니며 로그를 Temp에 보존하고 기존309PASS를 재사용한다. 다음은 실제 Action 경로로 Continue, Main03/04 대표 Story와 전멸뿐이다. 새로운격리실행 때문에 SaveWrite만 새Fixture에서 필요하다. 추가확인: Battle 취소 입력은 Esc뿐이고 패드B가 누락돼 같은 공통취소로 연결하는 최소수정을 반영하며 실물입력은 USER로 유지한다.

## 체크포인트2
Continue 버튼 경로 보완후 실제Dungeon Continue PASS. Main03 저작대화·Clip/Portrait·임시2인편성·정상Victory까지 PASS. 반환 Scene+Roster 합친검사가 FAIL하여 아직원인미확정; 두값을 나눠로그로 남겨 실패한Story반환만 재현한다. 기존menus/일반승리/Continue는 반복하지 않는다.

## 체크포인트3
Main03 반환 증거: Roster 전후완전히동일, 실제Scene은 World_StarterVillage였다. QA가 Field01 기본입구 좌표에 가상조우를 만들고 복귀이격1.2를 더해 마을Exit를통과한 Fixture 준비오류. 실제Actor 조우처럼 포털과떨어진유효좌표로 준비하고 실패반환만재검사한다. 이미PASS한저작첫대화/Clip/Portrait는 SKIP. 게임Scene/Exit/복귀코드는 변경하지 않는다.

## 체크포인트4
돌아가기/빈Item/전환후5샘플숨김·복구·포커스와Move/Submit/Cancel 활성은PASS. 마지막Cancel binding 검사는 명시적buttonEast/escape path만기대한QA오류였다. 현재패키지DefaultInputActions.UI.Cancel 정본은 */{Cancel}; 공식로컬Gamepad.buttonEast와Keyboard.escape가Cancel usage를선언한다. 실제사용Binding을기록하고usage도인정하는검사만보완한다. 앞선기능검사는반복하지 않는다.

최종 재컴파일에서 기존 Editor API CS0618 경고16(ExternalAssetImportEditor2/SecondRegressionAudit 계열14)을 확인했다. 이번 helper의 FindObjectsSortMode 폐기예정 경고6은 신규 조회 overload로 정리해 신규경고0/컴파일오류0이다. 기존 helper/외부가져오기 코드는 이 작업에서 수정하지 않았다. earlier Play종료 Console0과 최종재컴파일 경고를 구분한다.
