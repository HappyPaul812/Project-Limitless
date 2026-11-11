> EOD 선별 보관: 이 문서는 해당 작성 시점 기록입니다. 최신 승인/본편 상태는 EOD_20261111_Integration_PushAudit.md 및 각 담당 LATEST의 EOD 정정이 우선합니다. PNG·QA 원시파일·검사기는 이 문서 복사에 포함하지 않습니다. 현재 원본 제품은469b219f이며 EOD Push는 안전 감사 HOLD입니다.

# Main24 Issue85·88 — 서리올빼미 첫 한기 전투 정식 구현·원본 적용 2026-11-11

**OWL_FIRST_CHILL_PRODUCTION_LOCAL_APPLIED / NEW_DELTA_81PASS_0FAIL / GENERAL_F2_TRIGGER_ON / USER_REALPLAY_PENDING / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

## 승인·기준·구조 확인

[기획/사용자 최종 승인](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6791766233), docs/planning PLANNING_LATEST blob **8d823f2c95eb3ad6ab528aec4d13c04f7f63f83b**를 확인했습니다. 기존 B 코어7파일은 이미원본 main4a9b72f8에 있으므로 재이식0, 기존99/44/68 전수QA 재실행0.

기존 F2는 Main24FrostOwlCandidate가 좌표(5.7,-2)에 선택형 조우를 설치하고 E/F/클릭→기존Choice→MonsterEncounterService→BattleSceneFlow로 진입합니다. BattleSceneController.EnemyAction이 기존 TargetResolver/ChooseEnemyTarget을 거쳐 PlayBasicAttack을 부릅니다. 직접 피해는 PathCombatTraitRuntime→BattleStatusEffectRuntime.ApplyIncomingDamage→기존방어/보호·TakeDamage 후 onImpact(damage)로 연결됩니다.

**현 코드에 별도 Hit/Miss·명중률/회피 난수는 없습니다.** 기존 기본공격은 유효한 살아있는 대상의 피해를 최소1로 처리합니다. 이번에 새 명중률/강제명중/숨은확률을 만들지 않았습니다. 기존 onImpact의 **damage>0 && target.IsAlive** 결과에서만 한기부여를 시도하고, damage0/KO/부여거절도 해당 패턴의 시도1회를 소비합니다. 그러므로 이번 실패 QA의 0피해는 **콜백 계약 주입 검사**, 실제 존재하지 않는 Miss 난수의 실전검증이 아닙니다. 실제 타격KO 미부여/후속재시도없음은 별도 실연출로 확인했습니다.

## 정확한 제품 변경 — 기존 C#4파일만

브랜치 **codex/main24-owl-first-chill-production-20261111**, worktree `F:/study/codex/Project-Limitless/Temp/OwlProduction20261111/Worktree`.

| 파일(Assets/_Project/Scripts/) | 변경 |
|---|---|
| Battle/BattleSceneController.cs | F2 Owl에서만 기존B정책 주입·정식표시명, EnemyAction 최초패턴 분기, 종료/도망/파괴 기록정리 |
| Battle/BattleSceneController.OwlChillPilot.cs | 전투당1회 시도기록·예고확인·기존공격연결, 0피해/KO재시도방지, production/audit분리·기존VFX연결 |
| Battle/BattleColdStatusRuntime.cs | 허용된 RemoveChill 조기해제에도 동일 정상행동1회 보호, KO/전체정리/없는상태는 새보호없음 |
| World/Main24FrostOwlCandidate.cs | 기존 F2 조우 표식/진입안내를 정식규칙으로 정정(위치/선택/몬스터수치 불변) |

신규제품파일0/PNG0/meta·GUID변경0/Scene·Prefab변경0. 몬스터Definition/ID/명중·피해·AI공통 TargetResolver/ChooseEnemyTarget/기존방어·보호공식/B352/일반NPC·배경·Quest·Save schema 수정0. 후보 내부ID·레벨/HP/보상0 정의는 기존 그대로이며 새밸런스를 확정하지 않았습니다. 전투표시 이름만 서리올빼미로 정리했습니다.

### 정식 동작

1. F2 Owl 전투만 B정책을 주입하며 다른 전투는 냉기정책OFF를 유지합니다. 기존 감사arm은 자동호출0, production모드에서 감사arm 재설정/시험제목을 거절합니다.
2. Owl의 기존 AI가 고른 대상에 대해 최초 공격을 예고할 때 시도기록을 먼저 소비합니다. 기존 Main21ChoicePanel을 재사용해 **한기 공격 예고 / 대상: 맞으면 자기 행동2회 불가 / 확인하고 계속**을 표시합니다. 시간제한/QTE/회피반응 요구0. Enter/마우스로 읽고 확인한 뒤 공격합니다. 기존 한칸 선택창의 Esc/패드B도 동일 확인을 진행하며 공격을 취소/회피시키지 않습니다.
3. 기존 PlayBasicAttack의 피해/방어 판정을 그대로 사용합니다. 살아있는 대상에 유효피해면 한기2. 피해0/KO·보호중이면 부여하지 않고 후속 일반공격에서 다시 시도하지 않습니다. 이후 Owl은 기존 일반공격만 합니다.
4. 전투종료/도망/파괴에서 기록과 표시를 정리하고 새조우는 새인스턴스에서 미사용으로 시작합니다. Save에 패턴 flag 추가0.
5. 자연만료 보호는 기존B 유지. RemoveChill의 성공적인 조기해제도 상태제거 알림 전에 보호를 등록합니다. 중복제거/KO/전투전체정리는 새보호를 만들지 않으며, 차단종료/취소/다른인물 차례는 소비하지 않고 본인 정상행동완료만 소비합니다.

## 새 Unity Delta QA — 75+6=81 assertion PASS/0FAIL

Unity **6000.6.5f1**, 현재원본 복사본+정확4파일의 격리 `Temp/ChillStage2B20261111/QA/Client`에서 이번 전용 OwlProductionBatch.Run/ExtraOnly만 실행했습니다. 기존코어99/44/68 Run 호출0. 합성Save/Settings/진행/HP/상태·가상Keyboard/Mouse만 사용. 원본Editor Play/Scene/포커스 조작0.

- 첫실행64PASS 중 핵심패턴 **57**을 보존했습니다. 이후 비Owl 표본 외형 fallback이 적절치 않은 것을 발견해 그 부분6항목과 마지막전체오류1항목은 최종집계에서 제외했습니다. 실제휠체어 Context로 비Owl 부분만 다시 확인하고 KO/차단중조기제거 경계를 더한 Extra19 중 중복버전1을 제외해 **18**을 합산했습니다. 통과한 전체 패턴57은 반복하지 않았습니다. 64+19를 중복합산하지 않았고 원시ledger도 보존했습니다.
- 격리 **75**는 진입/기회완료/캡처크기·가시성 assertion 포함이며 독립75개 전수스위트가 아닙니다. 원본 추가 **6**은 Import/compile/GUID·순수메모리 조기제거 보호·Editor상태 검사입니다. Console/C#·게임Runtime 오류0. 과거81PASS 전수검사와도 별개입니다.

| 새 경계 | 실제 결과 |
|---|---|
| F2 진입 | 실제F2설치·E선택·Mouse전투시작→Battle, 자동productionON/시도미사용/감사armOFF·시험제목없음 |
| 예고 | 최초EnemyAction에서 시도소비, 피해/한기 전 안내, 1.2초 기다려도 확인창유지·피해0, Enter로 진행 |
| 유효타격 | 기존Guard 적용 실제타격후HP감소·생존/한기2·impact처리1회 |
| 차단/복귀 | 2→1→0·기존CD3→2→1→0·독/화상 지속경과, 자연만료즉시재부여거절, Mouse정상방어 완료후보호소비·행동재개 |
| 1회제한 | 다음실제Owl행동은 확인창/한기없는 일반공격, Flee때기록정리, 실제Field복귀/재진입때미사용으로새예고 |
| 실패 | 0피해콜백주입→부여0/시도소비, 뒤양수callback에서도강제부여0/재시도0. 실제HP1 타격KO→한기/보호0, 다음생존대상일반공격 |
| 조기해제 | QA RemoveChill 성공시보호/중복제거보호유지/취소미소비/정상방어완료소비·재부여허용. 차단중제거 알림재진입부여거절·차단완료도보호유지 |
| 정리/비Owl | EndBattle 정리, Ch1슬라임/Ch2그을음사냥개 실제EnemyAction에 자동정책/예고/한기0 |
| 접근성/화면 | 실제960/1280 예고·차단·1280복귀GameView, 키보드/마우스·시간제한없음·휠체어, 가시알파8px/viewport/위치불변 |

검사 대상 고정은 **QA에서 기존도발 API**를 사용했을 뿐 제품에 강제타깃을 넣지 않았습니다. TurnOrder 참가자를 결정적으로 지정하고 다음자동기회 coroutine만 멈췄으며 캡처동안 차단완료 연출만 잠시정지한 뒤 동일종료경로를 재개했습니다. 독/화상1·지속2/HP1은 fixture, 제품수치0변경. 기타 버프/MP·쿨다운 전체는 기존B 근거 재사용입니다.

## 실제 화면/시각 한계

화면 근거(로컬 작업브랜치 보관, 이번 복사 제외): `../../Evidence/OwlProduction20261111/Pattern/Owl_Telegraph_960x720.png`
화면 근거(로컬 작업브랜치 보관, 이번 복사 제외): `../../Evidence/OwlProduction20261111/Pattern/Owl_Blocked_1280x720.png`
화면 근거(로컬 작업브랜치 보관, 이번 복사 제외): `../../Evidence/OwlProduction20261111/Pattern/Owl_Resumed_1280x720.png`

모두 실제 격리UnityEditor.GameView.RenderView RT 캡처, 원본새Play촬영/합성판 아님. 예고확인 중 기존Modal이 중앙전장 일부를 가리고, 확인후 원래전투HUD로 돌아옵니다. 960 VFX(461.19,201.49,107,92.5), 1280(664.47,196.98,107,92.5), 소유자가시경계와8px/화면안 PASS. **VFX소유자 식별·태온/휠체어몸체 밀집은 기존 VISUAL_PARTIAL 유지**. 그림/Formation/몸체배치수정0, 이를해결완료로 표시하지 않습니다.

## 안전한 원본 적용·보호

시작main **4a9b72f88d58c313cee81efc03cb4118354fa38f** → 제품/최종main **469b219ffbdbf4c8e5b51e977bbe2940eb6119a2**, 별도브랜치 작성후 정확4스크립트만 **FF-only/no-autostash** 반영성공. 원본표적Import후DomainReload/컴파일완료, 기존GUID4·unconfiguredRuntimeOFF/피해HOLD·순수메모리 조기해제보호·기존EditorScene/선택/Play보존 **6PASS**, Console오류0.

사전 **8322파일/1,220,455,531bytes** 보호 및 승인4코드 백업. 사전changed0, 반영후 허용4코드외 **8318 SHA동일**·사용자dirty302/untracked550 status byte-exact동일. 냉기Icon3/VFX3 원본SHA를 원본/격리 모두 대조동일. Save/UserData/UserSettings·B352·NPC03/09/06/16·승인배경·Quest·기존meta/GUID·Scene/Prefab 보호. 원본Editor6000.6.5f1 기존Bootstrap/dirtyfalse/PlayOFF/선택null/비포커스 유지. 원격Push/Merge·main직접commit·강제reset/clean/stash0.

문서·CODEX_LATEST/CURRENT_STATUS·검사기/증거는 **적용브랜치 로컬Docs커밋에만** 보관하며 원본main에는 추가이식하지 않습니다. 개인Save목록/내용·라이선스포함 원로그는 커밋하지 않았습니다. 4파일SHA(Git blob LF기준과 실제원본Checkout SHA를 분리기록, EOL정규화일치)·원시/최종ledger·보호요약·Final.json은 Evidence/OwlProduction20261111 참조.

## 대표 실플레이·남은 기능

**일반F2 자동 패턴 활성화 완료**이며 Editor arm이 필요하지 않습니다. 서릿길마을 북문→첫눈의숲F2, 기존 Owl **(5.7,-2)** 근처 E/F/클릭→전투시작. Owl 첫행동의 **한기공격예고**를 읽고 확인하고계속→유효타격/생존시 한기2→해당캐릭터2기회차단→정상행동복귀 확인. 이후 Owl은 일반공격. 기존공격에맞기전에처치/도망하거나타격KO면 한기체험을 강제하지 않습니다. 새조우는 패턴1회가 다시준비됩니다. 실제대표손플레이수용은 아직대기입니다.

조기해제 보호 코드는 구현됐지만 **현재정화Ⅰ은 한기를 해제하지 않으며 변경0**. 한기 제거 사용자용신규수단은 이번에만들지 않아 RemoveChill QA API로만검증했습니다. 정화Ⅱ·어묵탕·화로 신규해제, 동상피해·동결괴사보스/즉사실전연결, 신규상품/전리품/전체냉기몬스터는 여전히 미연결/HOLD. 기존후보코어API가있다는것을 실전기능완료로 처리하지 않습니다. 다음구현은별도승인범위이며 이번완료후자동착수0.
