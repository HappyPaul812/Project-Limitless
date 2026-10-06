# 현재 구현 2차 종합 회귀검증 — 2026-10-06

## 구현 전 범위와 방법

LOCAL 정본과 최신 종합 QA의 J(15 Starting Skill 실제 UI), N(던전 일반8조우 전승리), T(다중해상도 주요 UI)를 우선한다. 기존 PASS의 단순 반복 대신 남은 하위 경로를 검사한다. 문서화→최소 Editor QA→백그라운드 검증 순서로 진행한다.

새 Story/Voice/TTS/Beast 기능과 미술 수정은 하지 않는다. Voice 의미 청취219와 Main04 기존재생성5는 이번 범위 밖이며 기존 상태 그대로 보존한다. 로컬 BeastCompanion에는 이미 passive/equip 구현이 있으므로 요청의 ‘아직 구현되지 않은’ 표현과 현재 상태 차이를 보고하고, 현재 코드 검증만 수행한다. 새로운 기획을 확정하지 않는다.

격리 Save/Settings 및 PlayUnfocused를 사용하고 사용자 Save·기존 변경·PNG·WAV·Catalog·Scene·설정을 시작 해시로 보호한다. 창 활성화/포커스/물리 입력/foreground 승인은 재사용하지 않는다. 실제 물리 입력은 USER_INPUT_REQUIRED로 남긴다.

15스킬은 실제 BattleSceneController가 생성한 명령/메뉴/대상 버튼 및 정상 연출·행동완료 경로로 검사한다. 통제된 전투 fixture와 일반 조우 실제 승리는 구분하며 테스트용 능력치/상태 주입은 fixture로 명시한다. 거절·취소 시 행동/자원/CD 보존 및 효과/종료를 별도 검사한다. Guardian/Healer/Sharpshooter/Fighter/Mage 세부 규칙은 스킬 README·전투시스템 정본과 대조한다.

던전은 최신 문서의 미검증 B2-02~04와 B1을 조사한 뒤 남은 조우에서 실제 Battle 진입→정상 공격/스킬→승리→보상→spawn 제거→Dungeon return을 검사한다. EndBattle 호출·강제 KO를 실제 승리로 기록하지 않는다. Boss는 기존 광범위 증거를 유지하고 순수 fixture 대표 회귀로 제한한다.

주요 UI의 1920×1080/1600×900/1280×720은 Canvas bounds/텍스트 생성/상호 접근과 가능하면 background render를 확인한다. 프로그램 경계 검사와 사람이 본 미관 PASS를 구분한다. 생성 대표5조합은 Preview→Save→World→Bootstrap Continue→Battle을 실제 경로로 검사한다. 전50 Mapping은 정본 참조만 확인하고 PNG 재검수/수정하지 않는다.

Save/Party/Formation/Inventory/성장/Beast/안전지대 및 던전 좌표 복원, 상태/아이템/길 특성 대표 fixture, Main01~16 정적+자동진행과 Main16 위험 경로를 다룬다. 이미 충분한 기존 실행은 변경 유무를 확인한 이력 근거로 표시하며 신규 실행 횟수에 합산하지 않는다.

최종 상태는 PASS/FAIL/NOT_VERIFIED/USER_INPUT_REQUIRED/LISTENING_REQUIRED로 나눈다. 실패는 게임 Bug와 fixture 실패를 구분하며 실제 Bug만 최소 수정한다. 직접 관련 QA/문서만 commit, GitHub push0. 시작 선행commit `b8cd54c4d28abfa0ac99ff722fc7459b8bfdb5ee`.

## 결과 — RESUME 완료

최종 세부 체크 **817개: PASS 812 / FAIL 0 / NOT_VERIFIED 3 / USER_INPUT_REQUIRED 2 / LISTENING_REQUIRED 0(이번 실행 범위)**. 같은 category/id는 마지막 결과 하나만 유지한다. 세부 assert 수이며 서로 독립된 게임 시나리오 수가 아니다. Main16 내부75×2는 wrapper2 안의 증거로만 기록하고 총수에 중복 합산하지 않았다. 범위 밖 Voice Queue219는 별도 LISTENING_REQUIRED이며 위 분모에 포함하지 않는다.

중단 당시 완료750 PASS를 재사용했다. 다음 시작점은 MP 부족/침묵/Guard 조건과 전멸 보존이었다. 이후 안전지역 Party 결함1건(영향2지역)을 실제 재현→문서화→최소 수정→회귀했고 Main16 Hearing/Default 축소 Runtime 왕복을 추가 완료했다. 기존507 Main16 전체 QA, Sprite 전수검사, Voice 감사, 성공한15 Skill/8 Dungeon/Quest103 진행을 재실행하지 않았다.

재실행 사유: Mage MP 비용0을 부족 조건으로 기대한 Fixture를 Healer MP0으로 수정; Field03 자동 Main10 시작을 누락한 전멸 Fixture의 영구 진행 비교만 유효 상태로 재검사; 최초 Battle/Skill/Item 캡처는 QA가 입장 Coroutine을 중단해 남은 패널 때문에 시각 근거가 부적절하여 9PNG만 재캡처. 실제 결함 수정 후 실패 지역2를 다시 검사했다. 최초 실패 증거는 Temp에 유지하며 game bug와 Fixture 오류를 구분했다.

|범주|PASS|FAIL|NOT_VERIFIED|USER_INPUT_REQUIRED|
|---|---:|---:|---:|---:|
|Appearance|2|0|0|0|
|Audio|36|0|0|0|
|Beast|1|0|0|0|
|Boss|5|0|0|0|
|Creation|40|0|0|0|
|Defeat|4|0|1|0|
|Dungeon|41|0|1|0|
|Fighter|6|0|0|0|
|Guardian|10|0|0|0|
|Input|24|0|0|2|
|Item|12|0|0|0|
|Mage|2|0|0|0|
|Main16|2|0|0|0|
|Party|13|0|0|0|
|Path|16|0|0|0|
|Quest|359|0|0|0|
|Resource|1|0|0|0|
|Save|25|0|1|0|
|Scene|9|0|0|0|
|Skill15|140|0|0|0|
|Status|7|0|0|0|
|Technical|4|0|0|0|
|UI|40|0|0|0|
|Visual|13|0|0|0|

### 전투·캐릭터·던전

- 15 Starting Skills: 실제 Battle 명령/메뉴/대상 버튼으로 통제 Fixture140체크 PASS. 취소/차단/효과/대상/행동 종료/CD·순자원 사용을 검사. 전투 입력 상태 주입과 일반 조우 승리를 구분하며 물리 마우스 입력 PASS를 주장하지 않는다. 최근 QA의 엄격 MP 공식 추가는 전15 재실행하지 않아 별도 전수 증거로 주장하지 않는다.
- Status/Item: 독·화상·감전·침묵 처리/기간 대표 Fixture, 회복4종 실제 UI 취소·무효 대상·정상 제거12체크 PASS. MP0 Healing Light와 침묵 Skill 차단, 강한 방어 상태에서 Guard 중첩 거절 PASS.
- Guardian30/40%·맹세 직접피해 전달/DoT제외·도발단일/광역제외, Fighter기세0~3/난도3번째CD, Boss HP60%·telegraph·phase2shield70/사망 해제, 5 Path 직접피해/치유/DoT 대표 Fixture PASS. Boss 실제 도망 버튼 차단 PASS; 이번 정상 Boss 승리는 추가 실행하지 않고 기존 QA 이력을 유지한다.
- READY50/800참조 및50조합 고유, 요청한 남녀/길/직업 대표5 생성 Preview→Confirm→World→자동저장→Bootstrap Continue→Battle Left Idle PASS. 기존 PNG800을 수정하거나 다시 육안 전수 검수하지 않았다.
- Dungeon B1 4/B2 4 일반 Encounter 서비스 진입→정상 공격 승리→EXP/탈렌트→스폰 제거→던전 복귀 PASS. 강제 KO/EndBattle 승리 없음. OnCollisionEnter2D 물리 접촉은 NOT_VERIFIED. 실제 던전 위치/성장/소지품/Party/자원/Beast Save·Continue PASS.
- 전멸 Fixture는 의도적으로 아군 HP0을 주입했고 일반 승리로 계산하지 않았다. 최근 Field03 안전지대 도착, HP/MP 완전 회복, EXP23/Level12/Talent123/소모품 미환불/Quest/Party 보존, Bootstrap Continue PASS. 장비 정식 장착 상태 저장은 미구현 NOT_VERIFIED이며 Inventory 보존으로 대신 PASS하지 않는다.

### Party·Story·UI·입력

- 시작 마을 기존 편성 제한/행/저장 PASS 재사용. 수정 후 Field03/Arbel 실제 NPC 확정·격리 Save·Continue·화면 유지·취소 PASS. 일반 Field/Battle 진입 거절 PASS. Scene source 불일치는 코드 보호조건이며 별도 Runtime 전환 중 확정 시도는 실행하지 않았다.
- Companion 정본 해금/명단/Formation 보존과 Story 임시 편성 보호 이력 유지. Beast 기존 패시브 구조/Save 유지 및 실제 Battle 별도 Combatant 없음 PASS. 신규 동물/장착 기능 추가0.
- Main01~16의103Objective 진행/오ID무시/JSON복원/완료/다음 Main 해금359체크 PASS 재사용. Objective는 퀘스트 복합키103고유, 문자열101개 중2문자열 퀘스트별 재사용. NextMain optional 필드가 없는 구간은 prerequisite 해금이 정본 동작이다.
- Main16 Hearing/Default(SerinIn) 각75내부PASS: 실제 NPC·World 왕복·조사·정상 공격 승리·증인·최종 보고·Continue. Dialogue 분기 ID 기술 확인이며 Voice 의미 청취 아님. 기존 모든 길/SerinOut 전체507은 재사용 이력이며 이번 신규 실행과 구분한다.
- 1920×1080/1600×900/1280×720의13대표 화면39PNG 확인. 최초 유효27PNG재사용+전투3화면9교체+Timeline3추가. 주요 문구/버튼 잘림 없음. 모든 상태·모든 스킬 설명 장문과 실물 디스플레이 미관을 전수 PASS로 확대하지 않는다.
- EventSystem/InputSystemUIInputModule Move/Submit/Cancel 연결 및 화면 포커스 PASS. Battle 방향 이벤트의 실제 선택 이동 PASS. 물리 Keyboard/Gamepad 각USER_INPUT_REQUIRED, foreground 승인 없이 포커스 전환/OS입력0.
- BGM 정본 Clip/Scene→Battle→Return/Field07·단일 Service·Mixer routing/loop/source 음량1 PASS. 사람 음량/음질 청취0. TTS/WAV/meta/Catalog/Story Voice 변경0, 기존 사용자 의미PASS23·재생성필요5/청취219 유지.

### 남은 항목·정본 차이·종료

미해결 game bug0(이번 재현 범위). 미검증: 장비 실제 장착 저장, 물리 Dungeon 접촉, standalone OS 종료·재시작. 사용자 입력2: 실물 키보드/게임패드. 다음 세션은 이 항목부터 시작하며 완료 세부 체크를 반복하지 않는다. OS 포커스가 필요한 실물 입력만 새 명시적 foreground 승인이 필요하고 이번에는 요청/실행하지 않았다. Voice 의미청취219는 별도 세션, Main04 기존5 재생성 필요 유지.

문서의 과거 구현상태에는 전투 SafeZone 미구현·Beast 장착/Save 미구현 설명이 남아 현재 코드와 다르다. 신규 기획으로 해석하지 않고 현재 구현 검증 결과를 이 보고서에 기록했다. 설계 변경0.

Unity6000.5.7f1 Compile0/최종 Console Error0 Warning0. Bootstrap clean EditMode·is_focused=false·AuditSaveDirectory null·slot0·runInBackground 원복. 보호3075파일 중 기존 게임 코드Party1만 변경, 누락0; Sprite/ThirdParty/Voice/사용자Save/Settings 보존. 파티 수정commit `adbbfbb`; QA commit은 최종보고 참조. 직접 파일만 stage/commit, 기존103 작업 보존·GitHub Push0. 이번 staged diff --check 오류0; 전체 working diff의 기존 unrelated 공백 경고는 별개다.

증거: [전체 체크 CSV](LIMITLESS_SecondRegression_Cases.csv) / [Runtime JSON](LIMITLESS_SecondRegression_Runtime.json) / [체크포인트](LIMITLESS_SECOND_REGRESSION_RESUME.md). 로컬 캡처·초기실패·Main16두로그·해시근거는 `Temp/SecondRegression20261006/`에 보존하며 Git ignored 임시 증거다.


## 실제 결함 재현 — 안전지역 Party 확정

RESUME Runtime에서 Field03 지하묘지 입구 안내인과 Arbel 안내인 각각으로 Party UI를 열고 Player 행을 바꾼 뒤 `편성 확정`을 실행했다. 두 지역 모두 UI는 열리지만 확정 후에도 열린 채 원래 행이 유지됐다(`Party/Field_03/confirm`, `Party/Arbel/confirm` FAIL). 시작 마을의 기존 확정/Save/Continue는 PASS다. 재현 JSON은 `Temp/SecondRegression20261006/party-confirm-reproduction.json`에 보존했다.

원인: `PartyManagementPresenter.OpenAt`은 시작 마을·Field03·Arbel을 허용하지만 `Confirm`은 `World_StarterVillage` 이외의 Scene이면 즉시 return한다. [파티 정본](../10_전투/동료_파티_편성.md)의 허용 안전지역 정책 및 [던전 입구 정책](../20_월드/던전_입구_안전지대.md)과 다르다. 실제 게임 결함 1건(영향 지역2)이다.

최소 수정 계획: UI를 연 source가 현재 Scene에 속하고 기존 허용 지역3 중 하나일 때만 확정하도록 Open/Confirm 정책을 일치시킨다. 일반 Field/Battle/Scene 변경 후 확정은 계속 거절한다. 새 지역·편성 규칙·Beast 기능은 추가하지 않는다. 수정 후 두 지역의 적용/저장/Continue와 거절·취소 경계를 재검증한다.

수정 완료: Open/Update/Confirm의 기존 허용 지역3 정책 공유 및 현재 source Scene 보호. Field03/Arbel 각각 확정·Save·Continue/다음 프레임 유지/취소 통과. Field/Battle 거절 통과.
