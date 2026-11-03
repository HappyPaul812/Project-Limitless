# Issue #85 — F2 서리올빼미 실제 전투 격리 후보

## 완료 범위

**F2_ENCOUNTER_PASS / OWL_BATTLE_RUNTIME_PASS / NOT_ORIGINAL_APPLIED**.
Unity **6000.6.5f1**에서 실제 F2 → Battle UI → 공격·피격·승패 → 탐험 복귀를 구현하고 **신규 #85 81 PASS / 0 FAIL**을 확인했습니다. 이는 기존 #80의81PASS 재실행이 아닙니다. #83 90/#84 37/저장85·72·72/Main23 기존 QA는 반복하지 않았습니다.

한기는 **COLD_STATUS_HOLD**, Hit·Defeat는 **HIT_DEFEAT_GENERIC_FEEDBACK_ONLY**입니다. 정식 몬스터 ID·밸런스·보상 확정은 별도입니다. 이번은 기존 수치를 차용한 명시적 **격리 전투 후보**이며 본편 통합·출시 완료가 아닙니다.

브랜치 `codex/main24-frost-owl-finish-20261103`, 출발 `d3dae9c51c8f96238ea527e42c51f10112ea97fc`. 승인·F2 작업 기준을 재사용했고 Bee 타이밍 후보나 #70/#77/#78 복구 코드는 병합하지 않았습니다. 실제 커밋은 아래 영수증에 기록합니다.

## 승인 아트 연결

- ZIP: `F:/Downloads/Limitless_FrostOwl_Attack02_Continuity_20261103.zip`
- SHA256 **`d6648b7451dc2184c2187c0b1c2f8e0f52fc800ac6975753d0f6d38f6fbf9192`** 일치. 중복 CRC/기존 모션 품질 QA는 생략했습니다.
- 승인 Idle8은 이전 납품의 보호 SHA와 대조해 그대로 복사했습니다. Attack4는 승인 ZIP의 실제 네 PNG를 그대로 복사했습니다. **12 PNG 바이트 보존**, 재작화/리샘플/재인코딩0. [전체 SHA·출처](Evidence/OwlBattle85_20261103/InputProtection.json).
- 정지v3/Idle00/Attack03 `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467`, Attack02 `6dcb1799e0970cd08ceadc111e7e2645f21eb973ae75f9b859c2ce356d07e936` 동결.
- 새 Resources `Main24FrostOwlCandidate/Definition.asset`의 explicitIdleFrames8/explicitAttackFrames4 → **기존 MonsterSpriteSheetAnimation.Configure(Image, definition)**에 연결했습니다. #84 SpriteRenderer Clip/Animator를 UI에 붙이지 않았습니다.
- 8FPS / PPU512 / 중앙 Pivot / VisualScale1 유지. 기본 전투 표시는 기존136×112 Image 영역·preserveAspect를 유지했습니다. **40×40 하한은 같은 실제 Battle Image를 QA에서만 잠시 조정해 캡처 후 원복**했으며 제품 표시 크기를 임의로40px로 확정하지 않았습니다.
- 별도 Hit/Defeat/VFX 원화·.anim 제작0. 미승인 냉기 궤적/입자 미적용.

## 실제 구현

### F2 선택형 조우

기존 `World_NorthF2_CANDIDATE` Scene을 그대로 사용합니다. 새 Scene/Prefab·그루터기/길/경계 Collider 수정0. `Main24FrostOwlCandidate`가 해당 Scene에 **한 개만** 설치되며, Owl은 길 바깥 `(5.7,-2)`에 있습니다. 새 Collider가 없어 도보·휠체어 길을 막지 않습니다.

접근 후 **E/F·패드 버튼·마우스 클릭 → 시간 제한 없는 선택창 → 전투 시작/머무르기**. 취소는 Quest를 변경하지 않습니다. 원거리 마우스 이동은 기존 Main23MouseTravel.GoToPoint를 유지하며, 접근 후 다시 클릭할 수 있습니다. 패드 바인딩은 추가했으나 물리 패드 실기는 별도입니다.

기존 MonsterEncounterService와 BattleSceneFlow의 단일 전환 잠금·Context·2초 재조우 억제를 사용합니다. 같은 프레임의 두 번째 진입 요청은 거부됩니다. 후보 Resources를 넣지 않은 프로젝트에는 조우를 설치하지 않습니다.

### 실제 Battle·HP·종료

BattlePrototypeEncounterFactory에서 **기존 파티 편성은 그대로 재사용**, 적만 후보 Owl1마리로 구성합니다. 실제 BattleSceneController의 Awake/CreateParticipants/TurnOrderQueue/명령·대상 선택/PlayBasicAttack/피해·HP Bar/결과 UI를 실행했습니다. QA는 UI Button의 실제 명령 콜백과 가상 필드 입력을 사용했고, 전투를 흉내 낸 별도 UI나 직접 HP 차감으로 승패를 만들지 않았습니다. 패배 시나리오의 시작 HP1만 합성 세션 자원으로 주입했습니다.

기존 동률 주사위·선공 규칙을 유지했습니다. 최종 실행에서도 Owl 선공→Player 대응이 실제 발생했습니다. 한 공격의 HP 감소는1회, 각 Player 공격도1회로 확인했습니다. 예시 실행의 Owl HP는 **60→42→24→6→0**, Player는 기존 방어/피해 규칙을 거쳤고 패배 시나리오는 **1→0**입니다. 이는 정식 Owl 밸런스 값이 아니라 검증 데이터입니다.

Owl 피격은 기존 공용 shake/alpha 피격 반응, 처치는 공용 회색·반투명 표시와 결과/Scene 퇴장을 사용합니다. explicitHitFrames/explicitDefeatFrames는 빈 배열이며 새 전용 모션 완성으로 보고하지 않습니다.

승리 결과 확인 버튼 → 기존 BattleReturnSafety로 접촉점에서 떨어진 **F2 안전 복귀**. 패배는 기존 규칙대로 **파티 회복 → 최근 안전거점인 서릿길 마을 복귀**이며 패배 즉시 F2로 돌리는 새 규칙은 만들지 않았습니다. 마을 북문을 통해 다시 F2로 들어가 탐험 가능한 상태까지 확인했습니다.

### 수치·보상·진행 경계

정식 기획값/ID가 미확정이므로 ID는 `CANDIDATE_frost_owl_basic_qa`, 조우도 `CANDIDATE_f2_owl_repeatable_qa`로 분리했습니다. **새 정식 ID가 아닙니다**. 기존01_GrassSlime의 Level1/HP60/공격비율100/민첩10 등 기본 데이터를 차용하고 EXP·탈렌트·드롭은0으로 명시했습니다. [후보 정의 사실](Evidence/OwlBattle85_20261103/CandidateDefinitionFacts.json).

기존 승리 보상 파이프라인으로0보상을 표시·저장합니다. 미확정 종의 펫 해금 기록은 건너뜁니다. StableEncounterId는 기존 일반 조우 규칙상 비어 있어 Main24 전투 완료/보상이나 Main25 해금으로 연결되지 않습니다. 이전 Quest/Portal ID는 변경하지 않았습니다. 반복 검증용 조우는 기존 재진입 억제 이후 선택 가능하며 정식 리스폰 정책 확정이 아닙니다.

중복 EndBattle 호출의 기존 battleEnded guard를 확인했습니다. 후보 보상 자체가0이므로 이 검증을 비영 보상의 전수 회귀 PASS로 확대하지 않습니다. 화폐77·인벤토리·펫 해금·Main24 진행은 그대로였고 구매/아이템 지급은 없습니다.

## 한기 HOLD

정본 `Chapter3_냉기_보스기믹_20261012.md`는 ‘확정 기획 / 런타임 미구현’입니다. Scripts/Battle 및 전체 runtime 검색에서 한기 상태 등록·확률/피해 구현은 확인되지 않았고, 기존 Main24 안내 텍스트만 있습니다. 따라서 **기본 공격만 완성**, 한기 상태/확률/피해·VFX/무료 해제템·상품 판매·Main24 보상은 추가하지 않았습니다. Battle 제목·선택창·필드 표식에 한기 HOLD/수치 후보/보상 없음이 표시됩니다.

## 신규 Unity QA

**81 PASS / 0 FAIL**, 새 compile/runtime 오류0. 기존 UnityEditor.Search.SearchDatabase 시작 예외1은 별도 환경 로그에 보존했습니다. 기존 deprecation/라이선스 토큰 경고를 ‘Console 완전 무오류’로 표현하지 않습니다. 실제 entitlement 확인 후 Batch는 정상 종료했습니다.

- 기존 북문 선택 → F2, 선택창 E/F/마우스·취소/재진입, 단일 조우/중복 전환 거부.
- 실제 Player Rigidbody/Collider의 휠체어·도보 접근, 조우의 Collider0, 승인 그루터기 유지.
- 실제 Battle Image의 Idle8/Attack4 전 프레임 표시·8FPS 설정·PPU512, 공통 표시 크기 및 같은 Image40px.
- 1280×720 / 960×720 / 1680×720 실제 Unity 캡처와 후보 전장 테두리 viewport 내 배치.
- 기존 턴 순서·단발 HP 감소·기존 피격·HP Bar, Owl 처치·승리 결과, 중복 승리 방지.
- 승리 F2 복귀·안전 Collider 위치·2초 재진입 억제·반복 전투, 패배·기존 회복/마을 복귀·F2 재진입.
- **합성 슬롯 Save/Continue의 F2/조우/승리 후 HP/안전거점/Main24 진행 보존**, 화폐·아이템·펫 변화0.

근거: [체크81](Evidence/OwlBattle85_20261103/checks.txt), [실제925 런타임 샘플](Evidence/OwlBattle85_20261103/Trace.json), [프레임·HP 읽기 분석](Evidence/OwlBattle85_20261103/Readback.json), [코드·자산·원본 보호](Evidence/OwlBattle85_20261103/Protection.json).

처음 입력 실패는 숨김 Batch의 IgnoreFocus 설정 누락이었습니다. 가상 장치와 반경/잠금 상태를 기록한 후 기존 #83 방식으로 QA 프로세스만 설정해 해결했습니다. 새 조우 입력은 기존 InteractionSystem과 같은 InputAction 콜백 방식을 사용합니다. 또 QA가 Player 선공을 가정했던 부분은 기존 동률 주사위와 충돌해 각 공격 직전부터 피해를 세도록 수정했습니다. 게임의 선공 규칙은 바꾸지 않았습니다.

실제960 캡처에서 기존 고정1180폭 전장 테두리 잘림을 확인해 **Owl 후보일 때만** 가로 앵커.04~.96으로 조정했습니다. 다른 전투 레이아웃/로직은 그대로입니다. 과거 실행/실패 로그·75/79 중간 근거는 Temp에 보존했고 최종 근거는 `QA/Evidence/`, `unity-viewport-verified.log`입니다.

## 실제 영상·스크린샷

단일 OFFLINE 열람판:
`F:/study/codex/Project-Limitless/Temp/Main24FrostOwlFinish20261103/Review/index.html`

HTML SHA256 **`250621e946a411de9fa55facd84ad5cf2d74aea27032b158f035fcb8a89210bd`**.

- [승리 전투](Evidence/OwlBattle85_20261103/VictoryBattle.mp4): 1280×720,235프레임,15.496초, SHA `14a20777a975640817d46b5855fb0963e8f6d7d24d7817e9c60febabe9276e23`.
- [패배 전투](Evidence/OwlBattle85_20261103/DefeatBattle.mp4): 91프레임,4.921초, SHA `a2491da851a0314e82169090d7b219fba8616d41fa7f9a0c2d868e61182a9c45`.
- [실제 Battle UI](Evidence/OwlBattle85_20261103/Battle_ActualUI_1280.png), [40px 검증](Evidence/OwlBattle85_20261103/Battle_ActualImage_40px.png), [960](Evidence/OwlBattle85_20261103/Battle_ActualUI_960.png), [1680](Evidence/OwlBattle85_20261103/Battle_ActualUI_1680.png), [승리](Evidence/OwlBattle85_20261103/Battle_VictoryResult.png), [패배](Evidence/OwlBattle85_20261103/Battle_Defeat.png), [F2 재진입](Evidence/OwlBattle85_20261103/F2_AfterVictoryDefeat.png).

내용은 **실제 Unity Battle Canvas/Camera RenderTexture 캡처**입니다. OS GameView/사람 플레이 녹화나 원화 PNG 모의 재생이 아닙니다. 영상은 연속 캡처 PNG의 파일 기록 시각을 기준으로 VFR 구성했으며 **영상 기록 속도와 모션8FPS를 구분**합니다. 원시326프레임 순서/전체 decode PASS, 캡처 시각·PNG SHA는 동 Evidence의 CaptureTimes CSV에 있습니다. VFR decode도 동일 timebase로 확인했습니다. [영상 메타데이터](Evidence/OwlBattle85_20261103/VideoQA.json).

## 적용 조건·원본 보호

후속 본편 채택에는 **정식 Monster/Spawn ID·밸런스/보상·한기 및 대응 수단 결정**, 이 후보와 기존 F2 기능의 통합 범위 승인, 원본 BuildSettings의 F2 등록 승인이 필요합니다. 이번에는 **fixture BuildSettings만** 기존 F2/Battle/마을을 연결했고 작업 브랜치의 Scene/Prefab/BuildSettings/Packages/ProjectVersion은 변경하지 않았습니다. 자동 본편 이식하지 않습니다.

수정 runtime C#6파일: BattlePrototypeEncounter/BattleSceneController/BattleSceneFlow, Main24FrostOwlCandidate(신규), Main24F2Traversal, Chapter3Main24Flow. 승인12PNG·새 Resources 정의/조우/meta만 추가. [QA Batch](../../Tools/QA/OwlBattle85Batch.cs)·[Probe](../../Tools/QA/OwlBattle85Probe.cs)·[fixture 준비](../../Tools/QA/OwlBattle85FixturePrepare.py)는 제품 Assets 밖 Tools에 있습니다. 준비 스크립트는 이 세션의 정확한 로컬 경로/선행 보호 레코드를 사용합니다. 새 세션은 경로를 먼저 확인하고 기존 QA를 무작정 재실행하지 마세요.

원본 main **ab47b931fec92976170f9f50aabde1822d78bb90**, status344/상세858 유지. 원본 Unity·개인 Save/UserData 열람/해시/복사/수정0. 합성 데이터는 새 fixture의 SyntheticSaves/SyntheticSettings에만 있습니다. 승인 Owl/Spider·그루터기 SHA 보존, 미승인 Sign/Fence/VFX·TTS/Discord66 변경0. 후보 코드6파일은 실제 시험 fixture와 byte-exact입니다. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_ORIGINAL_APPLIED**.

## 실제 기능 커밋·GitHub 보고 영수증

- 기능·승인 자산 연결·QA 커밋: `61dce08b271bd2e801205a711a8af7d54400c84f`.
- 실제 [Issue #85 구현·실증 보고](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6601090552) 게시 완료. GitHub에는 댓글만 있으며 코드·캡처·PNG 파일은 원격에 없습니다.
- 브랜치 `codex/main24-frost-owl-finish-20261103`. 신규 #85 81 PASS/FAIL0, 기존 #80/#83/#84/저장QA 재실행0.
- `F2_ENCOUNTER_PASS / OWL_BATTLE_RUNTIME_PASS / COLD_STATUS_HOLD / HIT_DEFEAT_GENERIC_FEEDBACK_ONLY / LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_ORIGINAL_APPLIED`.
- 원본 적용은 바울 대표 별도 승인 전 보류. 정식 ID·밸런스/보상·한기/대응수단·통합 범위 결정이 남습니다.