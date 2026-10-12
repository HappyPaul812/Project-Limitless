# 하렌·세린 공식 아트 v1 검수 및 Unity 통합 — 2026-10-12

## 최종 판정

**기술 자산 통합 완료 / 신규 기능 QA PASS / 전체 QA PARTIAL(기존 Search 내부 예외).** 하렌 전투는 **ART_READY / RUNTIME_PENDING**. 세린 공식 전투 외형과 하렌 Main21 첫 만남 외형·초상화만 연결했다. 하렌 정식 동료/CompanionDefinition/파티/Chapter3/능력치·스킬 추가0.

시작 HEAD `2cb23a4`, Field01 기능 Commit `37e4e3d`·기존 아트 `40f9517` 완료 후 별도 작업. 시작 사용자 Git335항목 보호. 프로젝트 정본과 실제 실행 버전은 `6000.6.5f1`이다. 원본 Editor PID31832 유지, Scene/Play/종료/포커스 조작0. 연결 가능한 Unity MCP 도구 없음·CLI Pipeline 연결 없음으로 새 패키지 설치 없이 기존 격리 Client에서 Batch 실행했다.

## 1. ZIP과 실제 납품

원본 `F:/Downloads/Limitless_Haren_Serin_Official_Art_v1.zip`, SHA-256 `7469173f8f572ab75754443a9afee1f0f4c6ac52d96aeeb88329a2b28751cd99` 불변. ZIP10항목 CRC PASS, PNG8/8 디코딩 PASS, JSON/MD 정상. 누락 파일 생성0. 실제 구성은 PNG 본자산5·Preview3·Manifest2다.

| 파일 | 실제 규격 / 연결 |
|---|---|
| Haren_Portrait.png | 1254×1254 **RGB**, 배경형 등록. Manifest의 RGBA 표기와 다름 |
| Haren_Portrait_Transparent.png | 1254×1254 RGBA, alpha0~255. story_haren 대화 초상화 |
| Haren_Field_Walk_4Dir.png | 512×512 RGBA, 4×4 / 128px,16프레임 |
| Haren_Battle_6Row.png | 1254×1254 RGBA, 6×6 / 209px,30유효+6투명 |
| Serin_Battle_6Row.png | 1254×1254 RGBA, 6×6 / 209px,30유효+6투명 |
| Preview3 | 미술 검토용, 게임 Sprite로 Import하지 않음 |

Manifest는 `ART_REVIEW_REQUIRED; NOT UNITY_QA_CERTIFIED`라는 제작 당시 상태를 유지한다. 이번 사용자 요청으로 기술 검수·Runtime 연결을 수행했으며 Manifest를 고쳐 승인 증거로 만들지 않았다. battle_contract.unused_columns는 Idle/Guard/Hit 행에만4·5열이 비는 것으로 MD 상세표와 대조했다. Attack/Skill/Defeat은6열 전부 유효다.

두 Battle 시트의30프레임과 투명6셀을 실제 alpha로 검증, 셀 경계 alpha접촉0. Field16프레임 bbox는 모든 셀 안에 있으며 최하단 bbox123/128로5px 여백, 잘림0. 미사용 빈 셀을 애니메이션에 넣지 않았다. Preview의 Down/Left/Right/Up과 실물 시트 방향이 대응한다.

외형은 하렌 갈색 머리·청동 바이저·청백 망토, 세린 갈색 긴 머리·청각 보조 장치·붉은 망토·가죽 사수 장비가 유지된다. 기존 세린 Field/Portrait 및 폴·미엘·태온과 비슷한 판타지 SD 계열이다. 새 Battle의 프레임별 장식/망토/머리 형태·키 변화, 일부 활이 사라지는 release 포즈와 셀 안의 효과 조각은 생성 아트의 미술 검토 대상이다. 셀 경계 잘림과 구분하며 PNG를 임의 보정하지 않았다. 색상·시각 선호의 최종 무결점을 주장하지 않는다.

## 2. Import·Animator

- 모든 신규 PNG는 ZIP 원본과 바이트 일치, 기존 공식 PNG 덮어쓰기0.
- Haren Field: PPU128, 하단중앙 Pivot(64,0),16개 방향 Sprite, Idle/Walk4방향=8Clip, 신규 Haren_Field.controller. 기존 세린 Field Sprite/Animator·플레이어 Animator 불변.
- Haren/Serin Battle: PPU100, 하단중앙 Pivot(104.5,0),209×209 Sprite30개씩. Idle4/Attack6/Guard4/Skill6/Hit4/Defeat6, 10FPS.
- 각 Battle Controller6상태·Trigger5(Attack/Guard/Skill/Hit/Defeat), Idle만 Loop. Attack/Guard/Skill/Hit은 ExitTime1에 Idle 복귀, Defeat은 마지막 프레임 유지. 기존 TaeonBattleAnimationBuilder 규칙을 신규 경로에만 적용; 기존3명 builder 실행0.
- Unity ISpriteEditorDataProvider와 capability checks를 사용해 생성. 기존meta 수동 편집0. 신규meta 포함68파일·새GUID38개, 충돌0. 기존meta GUID 변경0.
- 생성 스크립트는 격리 Editor에만 두고 Evidence의 cs.txt로 보관했다. 원본에 사용하지 않는 Editor 생성기를 추가하지 않았다.

## 3. 세린 실제 전투 및 화살

BattleCharacterSpriteAnimator에 `companion_serin`의 신규 Resources 경로만 추가. CompanionDefinition의 sharpshooter/path.hearing·능력치·Formation·기존 파티3슬롯 불변.

기본 공격·방어·피격·전투불능은 기존 공용 호출을 그대로 사용한다. 정조준/화살비/동료의 습격에는 공식 CharacterAnimation.PlaySkill 호출이 없어서 **외형 호출3개와 한국어 설명만** 추가했다. 피해·MP·쿨타임·입력·대상·턴·투사체 계산은 바꾸지 않았다. 플레이어에 CharacterAnimation이 없을 때 기존 동작 유지.

실제 Field 독침벌 조우로 Battle에 들어가3인 파티의 세린 전투 Animator를 확인했다. 기존 BattleSceneController의 기본 공격/정조준/화살비/야수 스킬 실행경로와 실제 코루틴·피해·피격·전투불능을 검사했다. QA는 대상과 currentActor를 격리 실행에서 지정했으며 키보드/게임패드 버튼을 수동 플레이한 검사가 아니다. 정상 공격/정조준 피해 감소와 Attack/Skill/Hit/Defeat Sprite 변화를 확인했다.

화살은 GoldenArrow 원본 + 기존 PlayProjectileAttack을 사용. 별도 화살 QA에서 **기본 공격·정조준 모두 x≈+423→-424로 이동**, 실제 Sprite가 좌우 반전돼 화살촉이 왼쪽 적을 향한다. 발사 순간 세린은 적을 향하는 Attack/Skill 포즈다. 움직이는 실제 Sprite/반전/회전을 확대 RenderTexture로 확인했다. GoldenArrow PNG/meta·BattleActionPresenter 변경0. 화살 문제 재현0·수정0.

[기본 공격 화살](Evidence/HarenSerinArt_v1_20261012/basic_arrow_actual.png)·[정조준 화살](Evidence/HarenSerinArt_v1_20261012/aim_arrow_actual.png)·[기본 이동 기록](Evidence/HarenSerinArt_v1_20261012/basic_projectile.tsv)·[정조준 이동 기록](Evidence/HarenSerinArt_v1_20261012/aim_projectile.tsv).

폴·미엘·태온은 실제 공용 TryAttach와6Trigger 재생·단발 종료·Defeat 유지 검사 PASS. 모든3명의 전체 스킬/밸런스 회귀를 새로 반복한 것은 아니다. 이전 보호 기능과 관련 PNG/Clip/Controller는 해시 불변으로 대조했다.

## 4. 하렌 Main21

DialoguePortraitDefinition Haren.asset은 기존 화자 ID `story_haren`에 투명 초상화를 연결. 새로운 영구 Character ID를 확정하지 않는다.

Main21Site의 index11에 Main21HarenVisual을 추가한다. 기존 Site좌표(2.8,2)·거리조건·목표·대사·콜백은 보존하며 Collider/Scene/Prefab 변경0. 첫 만남 목표에서만 Haren_Idle_Down을 보여 주고 이후 등장/합류를 임의 구현하지 않는다. 표식의 ART_PENDING 문구를 제거하고 이 지점 이름표 높이만1.35로 옮겨 신규 Field 외형과 겹치지 않게 했다. 콘텐츠 누락은 오류를 기록하되 기존 대화·목표는 유지한다.

격리 Main21 첫 만남 체크포인트 복원→실제 Site.TryInteract→하렌 화자 페이지에서 공식 portraitImage.sprite 일치→실제 대화 종료 후12번 목표 진행 PASS. Quest정의13개 유지, Roster에하렌0·CompanionDefinition하렌0. 새 Save스키마0·보상수치 변경0·Main22/Chapter3 진입0.

[실제 하렌 대화·필드 외형](Evidence/HarenSerinArt_v1_20261012/haren_dialogue.png). 화면은 격리 Runtime UI를 임시 WorldSpace 투영해 offscreen RenderTexture로 캡처하고 모든 RectTransform/Canvas를 복원했다. 원본 Game View/OS 화면 캡처가 아니며 기본 해상도의 완벽한 UI 배치 검증으로 해석하지 않는다.

하렌 Battle6상태는 실제 Animator 재생 PASS이나 전투 참가자로 연결하지 않았다. **ART_READY / RUNTIME_PENDING**: 합류 기획 확정 후 별도 연결.

## 5. QA 및 한계

| 항목 | 판정 |
|---|---|
| ZIP/PNG·규격·프레임 경계·Manifest 대응 | PASS, RGB 표기 차이 기록 |
| Import/Slice/PPU/Pivot·6상태5Trigger·Field8상태 | PASS |
| 5명 Battle Animator의6Trigger 재생/종료·기존3명 TryAttach | PASS |
| 세린 실제 전투 등장/기본·정조준·화살비·야수·피격·전투불능 | PASS |
| 기본·정조준 화살 이동/화살촉/사격 외형 | PASS |
| 하렌 Main21 외형·실제 화자 초상화·목표12진행 | PASS |
| Missing Sprite/Script | 검사한 Field01/Battle/StarterVillage에서0 |
| Unity6000.6.5f1 compile / 신규Runtime오류 | compile0·신규Runtime오류0 |
| Console/전체 QA | 기존 Unity Search 예외가 각 격리 실행에1건 발생해 PARTIAL. 이전 알려진2건 유지 |
| 원본 Editor import/recompile 상태 | 강제 refresh/Play/포커스 전환 없이 미검증. 동일 source/asset의 격리 compile·Runtime 증거 사용 |
| 미술 연속성 최종 체감·전체 기존 스킬·수동 입력·Main21 전체13목표 재주행 | NOT_VERIFIED. 이전 Main21 QA와 불변 계약 증거 재사용 |

최초 Runtime119 assertions, UI/attachment 보완124 assertions, 별도 화살 측정 실행도 신규오류0. 원시로그는 `Temp/HarenSerinArtQA/build.log`, runtime.log, focus_runtime.log, arrow_runtime.log에 보존. 테스트 Save/설정은 격리 Client의 각HarenSerinResults/FocusResults/ArrowResults에만 있다.

## 6. 보호·변경·Git

기존3564파일 중허용3C# 외3561개 불변·삭제0. 원본 ZIP·기존 Portrait/세린Field/플레이어Animator/폴미엘태온Battle/GoldenArrow/StarterVillage v4/Field01/Quest13정의/기존Save·설정/meta 불변. 원본 Editor PID31832 유지, QA프로세스 종료, 디스크F약533GiB/C약35GiB 확인·새대용량복사본0.

새 자산68파일 목록은 [integration_files.json](Evidence/HarenSerinArt_v1_20261012/integration_files.json). 런타임 수정3개: BattleCharacterSpriteAnimator.cs(세린매핑), BattleSceneController.cs(사수Skill외형3호출), Chapter2Main21Flow.cs(하렌Visual부착·표식). 신규 런타임 Main21HarenVisual.cs는 메타와 함께 목록에 포함. 문서: 본 검수·Evidence·Main21상태·세린아트상태·CURRENT_STATUS의 이번 항목만 선별 Commit한다.

사용자 기존335개 변경은 제외·보존하며 CURRENT_STATUS의 기존MCP기록도 미스테이징 유지. git add -A/reset/clean/checkout/원본Save수정/Scene재생성/기존아트덮어쓰기/Push0. 다음: 사용자에게 새 포즈 연속성 미술검토, 전체 수동 전투 입력, 원본Editor의 자연스러운 Import 완료 확인을 권장. 하렌 합류·전투는 별도 기획 확정 후 진행.

[보호 증거](Evidence/HarenSerinArt_v1_20261012/protection.json)·[Runtime](Evidence/HarenSerinArt_v1_20261012/runtime_checks.txt)·[보완](Evidence/HarenSerinArt_v1_20261012/focus_checks.txt)·[화살](Evidence/HarenSerinArt_v1_20261012/arrow_checks.txt). Evidence텍스트의 SHA256은 Git blob LF기준이다. 원시로그를 삭제/정규화하지 않았다.

Git 검사: C#/문서/Evidence 기본 diff --check PASS. Unity가 생성한 meta/anim/controller의 빈 YAML scalar 후행공백은 직렬화 원본 보존을 위해 수정하지 않았다. 해당 자동생성 YAML은 blank-at-eol만 제외한 전체 staged diff --check PASS이며 GUID·Importer 데이터는 변경하지 않았다.

선별 기능 통합 Commit: `3674add11e8cc153ffb91dc8ebaecb5d1b7fbbbe`. CURRENT_STATUS의 이번 요약과 기능 SHA 연결은 별도 Docs Commit이며 기존 사용자 MCP 기록은 스테이징에서 제외했다.
