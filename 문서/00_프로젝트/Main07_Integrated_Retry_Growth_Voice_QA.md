# Main07 재도전·동료 성장·기존 Voice 종합 수정

## 작업 전 계획 — 2026-10-07

최신 LOCAL 3e1ba72를 기준으로 문서→구현→검증 순서로 수행한다. 원본/사용자 Save/기존 unrelated 변경은 Temp/Main07Integrated/baseline.json과 Git 상태로 보호한다. GitHub push와 신규 TTS API 호출은 하지 않는다.

1. 모든 QuestDefinition의 DefeatEncounter를 자동 추출하고 첫 진입, 도망/패배 목표 유지, 재도전, 승리 단일 진행, 보상 중복 방지, Save/Continue를 실제 Scene과 fixture로 구분해 기록한다. Main07의 Paul 위치는 대화 목표와 전투 목표를 모두 처리하되 재도전에는 첫 대화를 반복하지 않는다. 기존 정상 Main03/04/13/14/16/17 구조는 보존한다.
2. 동료 레벨은 GameSessionData.Level만 참조한다. Save 필드를 추가하지 않는다. CompanionDefinition의 고유 기본값을 유지하고 일반/임시 Story 참가자 생성에 같은 계산을 적용한다. HP는 Lv1 결과와의 증가분만, 공격은 고유 baseAttack과 기존 직업식을 적용한다. 민첩은 Lv1 기준 증가분만 적용한다. 수호자/치유사 능력치 성장 미확정과 전체 동료 성장 요청의 충돌은 사용자에게 확인 중이며 승인 전 해당 정책을 구현하지 않는다. Player 성장은 변경하지 않는다.
3. Main07 Miel001~003 및 Taeon001~003을 기존 원본 PCM/오프라인 ASR/긴 무음 경계로 조사한다. 올바른 발화가 확보될 때만 sample 단위 복구하며 애매하거나 없는 발화는 TTS_REGEN_REQUIRED로 남긴다. Early7/Paul20/다른 Main Voice와 Catalog/Stable ID 순서는 보존한다.
4. Main06→07 초반→Paul→도망/패배→재도전→승리→MielMeeting→Main07 완료를 격리 Save로 검증한다. 전체 Encounter, Lv1/3/5/10/20/50 성장, 레벨업/Continue/Party·Beast·자원 회귀를 확인한다. 컴파일/Console/Missing Script와 byte 보호를 검사한다. 창 활성화·포커스 이동은 하지 않는다.

## 초기 코드 감사

- Main07 Current()는 targetId==현재 목표만 허용하여 companion_paul 대화 완료 뒤 field02_main07_paul_encounter 상태에서 입력을 차단한다. Runtime 재현 예정.
- CompanionDefinition.CreateParticipant 및 Main03/04/07 Story Factory는 고정 수치: COMPANION_LEVEL_SCALING_MISSING. 수호자/치유사 공통 능력치 성장의 기존 미확정 상태는 별도 정책 확인 대상이다.
- Miel001의 첫 문장만/002의 이전 tail은 사용자 실제 청취 증거이며 metadata PASS로 무시하지 않는다.

## 검증 결과

최종 **고유 검사 label 431 PASS / FAIL0**, 반복·재개를 포함한 PASS 기록617개. 단일 Play에서 전수 검사를 모두 완료했다고 주장하지 않는다. 초기 실패는 아래 검사기 보완 기록과 분리한다. [실행 결과](Main07_Integrated_Runtime_Results.txt), [검증 JSON](Main07_Integrated_Verification.json).

### Story Battle Retry

QuestCatalog 전체 Main01~17과 YAML inline/일반 형식 모두 확인: DefeatEncounter10개. [전수10 Matrix](Story_Battle_Retry_Matrix.csv). Main07만 FAIL_FIXED, 다른9개 기존 진입/재도전 구조 변경0. Main11만 보스 도망 금지 NOT_APPLICABLE_BOSS_FLEE이며 패배 후 재도전·승리는 PASS. 모든 도망/패배 Count0, 실제 재진입/승리 Count1/중복 종료·승리알림의 진행·EXP 중복0 및 Save/Continue PASS. 퀘스트 완료 보상은 기존 QuestService의 완료 제거/완료ID 정책을 보존한다.

Main07 최초 대화부터 실제 Main06완료→Field02재진입→Early7→PaulFirst→도망→F 재도전→패배→안전지대/거점 복귀→Continue→A 재도전→정상 명령 승리→미엘귀환/MielMeeting→마지막 대화→Main07완료→Continue를 통과했다. 별도 E 입력의 도망→Continue→재도전→정상 승리도 통과했다. 재도전은 첫 대화0, “폴 주변 몬스터 재도전” marker, 승리 뒤 Current=false를 확인했다. 다른 전수 전투는 실제 Scene·기존 Interact/MonsterEncounter 이벤트와 HP 승패 fixture를 사용했다. Collision 접촉·실물 키보드/패드는 이번 자동 입력·이벤트 검증 범위와 구분한다.

### Companion Growth

원인 COMPANION_LEVEL_SCALING_MISSING: 일반 정의와 Main03/04/07 임시 Factory 모두 고정 HP/Attack/Agility였음. 이제 같은 CompanionDefinition과 현재 Player Level을 생성 직전에 읽으며 레벨 Save0. Player 성장식 변경0. 수호자/치유사 동료 공통+1/레벨은 사용자 승인 정책이다. [공식과 근거](../10_전투/동료_파티_편성.md), [전체4동료×6레벨24행](Companion_Growth_Runtime_Matrix.csv).

|동료|Lv|HP/Attack/Agility|동직업 Player HP/Attack/Agility|
|---|---:|---|---|
|태온|1|132/16/9|104/18/10|
|태온|3|146/17/11|114/18/10|
|태온|10|195/21/18|149/18/10|
|미엘|1|104/14/12|100/18/10|
|미엘|3|114/15/14|106/18/10|
|미엘|10|149/19/21|127/18/10|
|폴|1|96/38/10|100/36/10|
|폴|3|104/44/12|108/42/12|
|폴|10|132/64/19|136/62/19|
|세린|1|108/37/14|100/36/12|
|세린|3|118/43/16|110/42/14|
|세린|10|153/63/23|145/62/21|

Lv1/3/5/10/20/50 모두 역전0/범위 정상/최소HP·Attack·민첩1이상. 기존 HP/민첩 Lv1 값을 유지하고 공격만 baseAttack+기존직업보너스를 적용하므로 중복 성장0. 실제 EXP 레벨업3→4 이후 생성/SaveRestore와 실제 Bootstrap Continue 뒤4동료 EffectiveLv5 동일. 세린+미엘 수동행, Player여우/세린곰, 현재HP45/치유사MP7 등을 포함한 저장 보존 PASS.

Lv3 폴 Before HP96/Attack14/Agility10 → After104/44/12, 같은직업 Player108/42/12. 동일 방어0·충분한HP fixture에서 일반피해14→44, Fireball24→75(Player72): [실제 SkillExecutor 비교](Main07_Paul_Damage_Comparison.csv). [실제 Main07 명령 피해](Main07_Paul_Damage_Runtime.csv)는 남은HP 상한/기존 길 특성·전투 상태에 따라 다르며 원시 공격식과 구분한다. Fireball·기본공격의 실제 기여와 정상 승리 확인.

### Miel / Taeon Voice

미엘001/002/003의 원본 분할·번호 밀림 확인, 기존 rawPCM으로3개 복구. 최종 길이6.94/3.63/4.72초. Catalog/ID/본문/GUID/meta 변경0. [본문·기존 실제·원본·방법·최종WAV Matrix3](Main07_Miel_Recovery_Matrix.csv), [상세 Voice QA](Main07_Miel_Taeon_Voice_QA.md).

실제 MielMeeting 페이지8 미엘001 전체→Paul017/018→페이지11 미엘002→Paul019→페이지13 미엘003→Paul020, Next 수동/단일 Source/자연종료/이전 Clip 정리 및 PCM 일치. Early7/Paul20/Miel3/Taeon3 **실제33PCM 전sample 오차0**: [페이지 기록](Main07_Integrated_Voice_Pages.csv). 태온3은 해당문장과 대응하여 WAV 변경0. ASR 단어/문장부호 오차를 의미 오류나 사람청취 PASS로 자동 확정하지 않는다. 복구 미엘3의 사용자 청취는PENDING. 신규TTS API0/재생성필요0/남은 생성 여유 소비0.

### 통합 검증 / 보호

CompileError0, RuntimeConsoleError/Warning0, 최종컴파일 ConsoleError0/기존CS0618Warning16/이번변경새Warning0, 실제Runtime MissingScript0 + 관련9Scene 원본MissingScript0. [Scene 검사](Main07_Integrated_MissingScript.csv). 최종 cleanBootstrap EditMode/is_focused=false/격리Save·Settings해제/InputSettings 원값복원. 창foreground/OS입력0.

Baseline3274개 중 기존변경은 Main07 retry C#1/동료성장 C#2/MielWAV3=6, 나머지3268byte동일. Early7/Paul20/Main03~06Voice/Main17Voice·TTS/244전체목록/원본/사용자Save/PlayerGrowth/QuestReward/Party·Beast코드·Assets/meta/Catalog 보호. 검증 helper와 도구·문서만 추가했다.

사용자 직접 확인: 복구 미엘3 발화 청취, 기존의 실물 입력/물리Collision 별도 검증 범위 유지. 모든 Voice 사람청취 완료라고 보고하지 않는다. 다음 권장: 보호된 기존 미청취 목록과 별도 요청된 후속 범위 진행. GitHub push0.

### 검사기 보완 기록

최종 PASS와 구분하여 Temp/Main07Integrated/qa-*-attempt.txt에 초기 실행을 보존했다. 첫 실행의 Paul 기대공격42는 Player 값과 혼동한 QA 오류였고, 실제 폴은 고유 base14 + Intelligence15×2 =44다. 다음 실행에서 음성 종료 직전 마지막 표본만으로 자연 종료를 판정하던 QA를 최종 AudioSource 상태와 최대5초 여유로 보완했다. 게임 음성 재생 오류로 자동 확정하지 않았다.

Main02 초기 접촉 fixture가 필드 입구에서 몬스터 이벤트를 호출하여 복귀 안전 이격 뒤 마을 출구를 밟았다. 관측은 expected=Field_01/actual=World_StarterVillage이며 게임 재도전 결함과 구분한다. 실제 스폰 위치에서 접촉 이벤트를 호출하도록 fixture만 보완했다. 물리 Collision 접촉을 새로 검증했다고 주장하지 않는다.

## 성장 정책 확정

사용자 답변 “동료에만 공통 +1 성장 적용”을 받았다. 수호자/치유사 동료의 6능력치에만 레벨 증가분 +1을 적용하고 기존 직업별 공격/HP 계산식을 재사용한다. Player 계산기와 Player 수치는 보존한다. HP/민첩은 기존 Lv1 동료 고유값에 성장 증가분만 더한다. Attack 필드는 고유 baseAttack으로 사용하며 기존 직업 시작 보너스도 한 번만 포함한다. 별도 동료 레벨 Save는 추가하지 않는다.


### 후반 fixture 상태 보완

초기 Main11 SaveRestore 비교는 완료Main09 기록만 넣고 정식 폴 해금을 반영하지 않은 fixture였다. 기존 구버전 복원용 해금 경로와 같은 조건을 fixture에도 반영한 후 Main11 및 후반6전투의 동일 비교가 PASS했다. Save/Quest/CompanionRoster 게임 코드는 변경하지 않았다.

## Git

- `ec6d3e5` Fix: Main07 Story Battle 도망 패배 후 재도전.
- `74bea14` Fix: 동료 레벨을 Player 성장에 동기화.
- `907b2e4` Fix: Main07 미엘 Voice 3개 기존 PCM 경계 복구.
- QA/helper/복구도구/현재상태는 이 기록을 포함하는 최신 `Docs: Main07 재도전 동료 성장 Voice 통합 QA` commit.

관련 diff --check PASS. 기존107 상태 항목은 보호하고 작업 파일만 지정하여 stage한다. GitHub push하지 않는다.
