> EOD 선별 보관: 이 문서는 해당 작성 시점 기록입니다. 최신 승인/본편 상태는 EOD_20261111_Integration_PushAudit.md 및 각 담당 LATEST의 EOD 정정이 우선합니다. PNG·QA 원시파일·검사기는 이 문서 복사에 포함하지 않습니다. 현재 원본 제품은469b219f이며 EOD Push는 안전 감사 HOLD입니다.

# Issue88·85 한기 Stage2 B 코어 원본 로컬 연결 — 2026-11-11

**LOCAL_ORIGINAL_CORE_APPLIED / UNITY_DELTA_QA_PASS / OWL_PRODUCTION_TRIGGER_OFF / CONTENT_POLICY_HOLD / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

## 승인과 제품 범위

바울 대표의 이번 원본 코어 연결 지시, [#88 최신 기획](https://github.com/HappyPaul812/Project-Limitless/issues/88#issuecomment-6791306760), docs/planning PLANNING_LATEST blob **14e5201f831dc5a44e76c7934397dacd28204d51**를 확인했습니다. [기존99PASS](https://github.com/HappyPaul812/Project-Limitless/issues/88#issuecomment-6788194741)와 첫 연결44PASS를 재사용, 전수 재실행0. 코어 이식과 Owl 공식 자동 발동은 별도입니다.

- 검증 소스 `codex/chill-stage2-b-isolated-20261111` **c886e89b44250c1088c49d2bc25170da6e51f429**. Stage1 포함 누적7파일의 Git blob을 정확히 재사용했습니다.
- 별도 적용 브랜치 **codex/chill-stage2-b-core-original-apply-20261111**, `F:/study/codex/Project-Limitless/Temp/ChillCoreApply20261111/Worktree`.
- 실제 시작main **548cf1e275684d8c4a647685fb6f08be0c495be7** → 최종main/이식제품 **4a9b72f88d58c313cee81efc03cb4118354fa38f**, **FF-only/no-autostash 완료**. main직접commit/원격Push/Merge/강제reset/clean/stash0.
- 기존3수정파일은 소스 기준과 원본이 동일해 3-way 충돌0. 새4경로/새GUID2 충돌0. diff를 정확7파일로 확인한 뒤 반영했습니다. 제품 커밋은 적용 worktree에서 작성했습니다.
- 이 문서·CODEX_LATEST/CURRENT_STATUS·검사기·증거는 **적용 브랜치 로컬 Docs 커밋에만 보관**, 원본main에 추가FF하지 않습니다. 원본main에는 제품7파일만 추가되었습니다.

공통경로 `Unity/Client/Assets/_Project/Scripts/Battle/`:

| 파일 | 변경 |
|---|---|
| BattleColdStatusRuntime.cs | 신규, Stage1/B 상태·자연만료 복귀보호 |
| BattleColdStatusRuntime.cs.meta | 신규 GUID **be602c43d67742eca9e6f2957ff659a8** |
| BattleSceneController.OwlChillPilot.cs | 신규, 기존 Editor 명시arm·기회분리·표시 |
| BattleSceneController.OwlChillPilot.cs.meta | 신규 GUID **32e9716abb8a4d2fad2b6700d64d5bb1** |
| BattleSceneController.cs | 수정, 기회경과/실제행동 완료 분리·기존 DoT 꼬리 연결 |
| BattleSkillSystem.cs | 수정, 시간형 지속과 실제행동 소비 분리 |
| BattleCombatantStatusViewModel.cs | 수정, 기존 상태표시 연결 |

신규PNG0/이미지재제작0/기존GUID·meta변경0/Scene·Prefab변경0. 냉기VFX/Icon6 승인 원본 SHA를 원본·격리 모두에서 확인했고 그대로입니다. 각7파일 SHA/GUID는 product-files.json에 기록했습니다(텍스트는 소스LF이며 원본Checkout EOL 차이만 정규화 비교, Git blob 동일).

## 신규 최소 Unity Delta QA

Unity **6000.6.5f1**, 현재 원본 Assets/Packages/ProjectSettings 읽기 복사본에 정확7파일만 덮은 기존 격리 QA경로 `Temp/ChillStage2B20261111/QA/Client`를 재사용했습니다. Library만 재활용했으며 코드 기준은 현재 NPC03/09 적용본입니다. 이번 전용 ChillCoreApplyBatch.Run/ChillCoreApplyProbe는 기존99/44 검사 Run을 호출하지 않습니다.

**격리62 assertion PASS/0FAIL + 원본 Import6 PASS/0FAIL = 신규68 assertion PASS/0FAIL**. 캡처/진입/기회종료 assertion도 포함하며 68개 독립 전수스위트가 아닙니다. 과거68·99 등 완료된 QA를 재집계한 것이 아닙니다. 게임Runtime/C#오류0, 원본Console error0.

| 표적 | 실제 확인 |
|---|---|
| Chapter1/2 | 실제 Field01 GrassSlime·Field05 SootHound Context/배경, 방어·기존HP포션·일반공격 완료, 실제 도망 후 원래 Field로 복귀. 비F2 arm 거절 |
| 일반 F2 | 생성 직후 arm=false/냉기정책null/시험제목없음. 정책미주입 한기 API 거절. 실제 Owl EnemyAction 완료 후 아군 한기0/자동부여flag=false |
| 명시arm | ConfigureOwlFirstChillPilot()를 검사기만 호출 후 기존 부여 API로 테스트 상태 주입. 활성중 중복 거절. 현재기회2→1→0 차단 |
| 기회 경과 | CD3→2→1→0, 방어 만료, 기존 화상·독 지속2→1→0 및 실제HP감소. MP0/도발2·감전·침묵 차단중 유지. 정상완료와 별개 |
| 복귀 | 자연 만료 직후 재부여 거절, 메뉴취소로 보호 유지. 실제 Keyboard Enter로 공격선택→가상Mouse 표적클릭→실제행동 완료 후 보호소비·재부여허용 |
| KO | 별도 합성HP1/화상1 표본에서 차단기회 DoT KO→한기/복귀보호 정리 |
| 표시 | 실제960×720·1280×720 GameView, 한기2회/행동불가 텍스트·명령버튼 차단. 기존VFX의 가시알파8px·viewport내·로컬전경/raycast없음·캐릭터위치불변 |
| 원본Import6 | 원본버전/compile완료, C#5개Import, 신규GUID2, 새Runtime기본정책OFF/동상피해HOLD, Editor명시arm API/비직렬화flag, 기존Scene/선택/Play보존 |

**검사 한계/합성 입력:** 실제 BattleSceneController/기존3인 파티/휠체어 외형/기존 행동Presenter·Field복귀를 사용했습니다. 소유자 상태/턴큐를 API로 결정적으로 지정하고 자동 다음기회 coroutine만 멈췄습니다. 약한 기존 적이 첫 검사타격에 죽어 나머지 명령이 생략되지 않도록 **QA 복제 MonsterDefinition의 HP만9999**로 설정했고 제품정의는 수정0. 화상1/HP1·100/포션1개는 QA 입력이며 동상피해·상점경제를 정한 것이 아닙니다. 960/1280 캡처 동안 차단완료 연출 coroutine만 일시 정지한 뒤 동일 완료 경로를 재개했습니다. 일반Owl 자동OFF는 실제EnemyAction으로 확인했으며 arm뒤 첫타격부여 hook은 기존44근거 재사용, 이번은 상태주입 경계 검사입니다. MP를 쓰지 않는 수호자의 MP0 표본을 실제MP사용자 증거로 확대하지 않으며 치유사MP 구분은 기존99근거입니다. 버프·모든스킬·승패 전수 검사는 반복하지 않았습니다.

## 실제 화면·기존 시각 PARTIAL

화면 근거(로컬 작업브랜치 보관, 이번 복사 제외): `../../Evidence/ChillCoreApply20261111/Core_ArmedBlocked_960x720.png`
화면 근거(로컬 작업브랜치 보관, 이번 복사 제외): `../../Evidence/ChillCoreApply20261111/Core_ArmedBlocked_1280x720.png`

UnityEditor.GameView.RenderView의 실제RT 캡처입니다. OFFLINE합성/모형UI/원본새Play가 아닙니다. 960 body(595.03,306.24,89.42,93.96), VFX(461.19,205.74,107,92.5), 1280 body(801.22,302.38,103.25,108.5), VFX(664.47,201.88,107,92.5). 가시8px 간격 PASS. 기존 VFX는 소유자 발밑 왼쪽으로 떨어져 있어 **소유자 식별·태온/휠체어 몸체 중첩 VISUAL_PARTIAL**은 유지합니다. Formation/몸체 위치·승인그림은 변경하지 않았습니다.

## 일반 발동 OFF와 남은 정책

제품에는 새로운 Awake/Start 자동arm·Scene플래그·발동확률·반복AI를 만들지 않았습니다. `owlChillPilotArmed`는 초기false/비직렬화, `ConfigureOwlFirstChillPilot()`는 **UNITY_EDITOR 조건부 명시 호출**만 있고 제품 내부 자동호출0. 새 Runtime coldRules도 null로 부여를 거절합니다. 일반 F2 Play에서 “한기 시험” 제목이나 한기 자동 부여는 없습니다.

**코어는 실제 원본 반영 완료**, **Owl 정식 한기 자동 발동은 활성화하지 않았습니다**. Main24 전체 첫한기 실전Playable 완료로 보고하지 않습니다. 코어 배포를 다시 승인받을 필요는 없으며, 향후 실전 발동률/AI·반복주기/예고/연속공정성, 조기해제후 보호, 동상 피해량/동결괴사보스·즉사, 해제아이템·상점·전리품, 시각수용/Formation 재설계는 별도 기획/사용자 승인 필요입니다. 이번에 임의 확정/활성화0.

검증을 다시 보려면 일반게임에서 임의 확률로 기다리는 방법이 아니라 **별도 Editor 감사 환경의 실제F2후보 전투에서 명시적으로 ConfigureOwlFirstChillPilot() 호출**이 필요합니다. 기존 테스트 hook은 arm 후 첫 유효 Owl 기본타격에1회만 연결하는 감사장치이며 제품 정식패턴이 아닙니다. 별도 QA도구는 적용브랜치 Tools/ChillCoreApplyProbe.cs에 있고 원본에서 자동실행하지 않습니다. 일반 플레이 수용과 Editor 명시 테스트 결과를 구분합니다.

## 원본 보호

사전8318파일/**1,220,431,056bytes** SHA 보호/승인3코드 백업. 반영전 changed0, 반영후 기존8318 중 승인한3코드만 변경·**나머지8315 SHA동일**, 새4파일은 검증소스와 일치. 사용자 tracked dirty302/untracked550 및 status byte-exact동일. Save/UserData/UserSettings·기존Scene/Prefab/GUID/meta·F2B352·NPC03/09·OGA06/16·승인BG·냉기6·Quest 보호.

합성Saves/Settings만 사용하고 원본Save를 읽어 플레이 상태로 가져오거나 저장하지 않았습니다. 원본 Editor6000.6.5f1 기존 Bootstrap 경로/dirtyfalse/PlayOFF·선택null·비포커스/컴파일idle 유지. 표적Import 외 Scene/선택/Play조작0. 보고/검사기·증거는 원본main에 넣지 않았습니다. 개인Save목록/내용·라이선스 포함 원Unity로그는 커밋하지 않았습니다. 마지막 보호/최종SHA는 Temp/ChillCoreApply20261111/Final.json 및 Issue88/85 댓글 참조.
