# Owl·Spider 최종 승인 및 VenomBee 공격·권리 계약 — 2026-11-03

## 결론과 정본

바울 대표의 이번 명시적 지시에 따라 **Owl Attack4 / Organic Spider Walk8 USER_ART_APPROVED**로 확정합니다. 승인 모션 추가 재작화 금지. #84 Unity 37 PASS는 재실행 없이 인정하며 실제 제품 적용 승인은 별도입니다. Spider 실제 발 접지·미끄러짐은 NOT_VERIFIED / WALK_CONTACT_HOLD를 유지합니다.

Bee는 **BEE_NOT_DONE / MOTION_REVISE_CONTINUE / TARGET_CONTACT_HOLD / RIGHTS_HOLD / FULL_COMBAT_READY_NO**입니다. 화면 방향·명목 피해 시간은 확인했으나 정확한 침끝 접촉 좌표·거리·프레임 이벤트는 미구현입니다. PNG 제작·Unity 실행·본편 변경 없이 문서와 최소 제작 지시서만 작성했습니다.

AGENTS → PROJECT_CONTEXT/CURRENT_STATUS/CODEX_WORKFLOW → START_HERE/COMMON_RULES/CODEX 역할 → 전투시스템/초반 성장·공용 독/북부 몬스터 기획 → 코드·정의·Scene 순으로 확인했습니다. GitHub main 공통 규칙/CODEX_LATEST, docs/planning PLANNING_LATEST, docs/art-new SPRITE_ART_LATEST, docs/art-renewal WORLD_ART_RENEWAL_LATEST 및 #60/#63/#74/#83/#84 본문·최신 댓글을 조회했습니다. main 인계는 10/24 기록으로 남아 있지만 최신 계획 브랜치·댓글과 로컬 격리 QA는 11/03입니다. 오래된 대기 문구를 최신 승인보다 우선하지 않습니다. [조회 SHA·댓글](Evidence/BeeTargetRightsApproval20261103/GitHubContext.json).

브랜치 `codex/bee-target-rights-and-art-approval-20261103`, 출발 `c6eab1bb5059d49120cce9c5e359010c255e8173`. #84 검증 커밋 `bde96a92e14c663e33ce7640f98f863d58a0d306` 재사용. 조사 대상은 이 격리 checkout이며 원본 Unity/개인 Save는 열지 않았습니다.

## 승인 자산 동결

| 자산 | 승인 SHA256 | 상태 |
|---|---|---|
| Limitless_FrostOwl_Attack02_Continuity_20261103.zip | `d6648b7451dc2184c2187c0b1c2f8e0f52fc800ac6975753d0f6d38f6fbf9192` | USER_ART_APPROVED / UNITY_CLIP_PASS 8FPS |
| LIMITLESS_WALK8_EDGE_FIX_20261103.zip | `771bd1813608aa3e350c6124ab0d43293c6d2988b1833014da64484d5272c8da` | USER_ART_APPROVED / UNITY_CLIP_PASS 10FPS |
| Owl Attack02 | `6dcb1799e0970cd08ceadc111e7e2645f21eb973ae75f9b859c2ce356d07e936` | 동결 |
| Owl 승인 v3 / Attack03 / Idle00 | `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467` | 기존 Idle8·정지원화 보호 |
| Spider 승인 v5 / Idle00 | `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9` | 기존 Idle8·정상 Walk 보호 |

F:/Downloads ZIP2 SHA가 승인값과 다시 일치했습니다. CRC/decode/Manifest/Unity QA 반복0. [개별 승인12프레임 전체 SHA 등록부](Evidence/BeeTargetRightsApproval20261103/ApprovedAssetRegistry.json)의 Assets/Input 이름은 #84 fixture 사본명입니다. 원본 납품 이름은 ZIP을 따릅니다. 미승인 Owl 분리 냉기 VFX는 HOLD. Spider Organic Attack8/Shoot8은 미제작이며 기존 옛 Resources 모션과 구분합니다. Chapter4 메카닉 v3 ARCHIVE_ONLY 보호.

공식 승인 근거: [#74 Owl](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6598037098), [#60 Spider](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6598038404), [#84 통합](https://github.com/HappyPaul812/Project-Limitless/issues/84#issuecomment-6598039586). 이전 검수의 ‘최종 미술 대기’는 과거 상태이며 이번 승인으로 해소됐습니다. 기술 미검증 항목은 해소되지 않습니다.

## 게임 연결 조건 — 이번에는 미적용

**Owl:** #83 World_NorthF2_CANDIDATE/Main24F2ContentBuilder는 탐험·관찰·왕복뿐이며 Owl 전투·Spawn은 없습니다. 조사한 MonsterDefinitions에 정식 frost_owl도 없습니다. 새 ID·수치·퀘스트·무료 한기 대응을 확정하지 않았습니다.

Battle은 UI Image입니다. #84의 SpriteRenderer.m_Sprite AnimationClip/Animator를 UI에 그대로 붙여 완료 처리하면 안 됩니다. 기존 MonsterSpriteSheetAnimation은 Configure(Image, definition)에서 명시적 Idle/Attack 배열 또는 Resources 경로를 읽습니다. 후속은 이 경로로 승인 Idle8/Attack4를 연결하거나 Image용 Animator 어댑터를 별도 선택해야 합니다. 두 재생기의 동시 Sprite 덮어쓰기는 금지합니다. PPU512/중앙 Pivot/Scale1/8FPS를 유지하되 UI 표시 크기는 PPU만으로 결정되지 않습니다. Battle 타격·복귀 타이밍 및 F2 전투 경계는 별도 적용 승인 후 최소 검증합니다.

Field Animator를 사용한다면 Idle_Up/Down/Left/Right, Walk_* 상태와 FieldSprite/FieldAnimatorController 계약을 확인해야 합니다. 기존 시트 재생 경로면 별도 Animator 없이 SpriteRenderer에 연결할 수 있습니다. 없는 Owl Walk/Hit/Defeat를 임의 생성하지 않습니다. VFX/Spawn/전투 구현은 이번 범위 밖입니다.

**Spider:** 03_ForestSpider.asset의 forest_spider는 FPS10/PPU256 기존 프레임/visualScale0.7/sourceFacesRight1/fieldMoveSpeed0.65입니다. Idle/Walk는 MonsterAnimations/Spider/Idle, /Walk, Attack/Shoot 경로는 옛 자산을 가리킵니다. FieldAnimatorController null, 실제 재생기는 MonsterSpriteSheetAnimation입니다. LoadAll 후 이름 ordinal 정렬, explicit 배열이 있으면 경로보다 우선합니다.

후속은 승인 유기형 Idle8/Walk8만 별도 경로/명시 배열로 연결하고 00~07·중앙/PPU256/.7/10FPS를 보존하는 범위입니다. 기존 폴더 덮어쓰기·신구 프레임 혼합 금지. 기존 옛 Attack/Shoot를 새 유기형 제작 완료로 간주하지 않습니다. Field SetFieldMoving/이동속도 동기화와 발별 접촉 계측은 별도 게이트입니다. 전체 전투 교체 완료가 아닙니다.

## Bee 실제 전투 계약 — 정적 코드 확인

[코드 발췌·줄 번호](Evidence/BeeTargetRightsApproval20261103/CodeEvidence.txt), [코드 SHA](Evidence/BeeTargetRightsApproval20261103/CodeSourceSHA.json), [계산 좌표](Evidence/BeeTargetRightsApproval20261103/BattleCoordinateContract.json)를 제공합니다. Battle UI는 런타임 생성이며 고정된 독침 socket/접촉점은 Scene/Prefab에 없습니다. 아래 값은 코드 계산이며 실제 Unity 픽셀 실측이 아닙니다.

| 진영/행 | Anchor x | 전장 로컬 UI x |
|---|---:|---:|
| 적 후열 | .14 | -424.8 |
| 적 전열 | .32 | -212.4 |
| 아군 전열 | .68 | +212.4 |
| 아군 후열 | .86 | +424.8 |

전장1180×320, Canvas 기준1280×720/match.5. Column0/1/2는 위/중간/아래: Anchor y=.68/.47/.26, 로컬 y=57.6/-9.6/-76.8. 논리 Front/Rear를 위/아래로 혼동하지 않습니다. 화면비에 따라 Canvas 스케일이 달라집니다.

CreateMonster에서 Bee는 MeleePhysical. Main03은 적 후열0, Main04는 후열0/1 Bee입니다. TargetResolver는 생존 전열과 같은 Column 전열이 비어 열린 후열을 허용하며 단일 공격 도발은 우선합니다. 고정 우하단 가상표적 하나는 실제 대상 계약이 아닙니다.

적은 왼쪽에서 오른쪽 아군을 봅니다. sourceFacesRight=true인 Bee의 Battle X scale은 +1. 좌향 원본이면 -1로 전체 flip할 뿐 몸통·독침의 개별 회전은 없습니다. 이 플래그는 머리 방향 정규화이지 침끝 접촉 판정이 아닙니다.

적 Image 기본136×112 × Bee .85 = **115.6×95.2 UI 사각형**, preserveAspect=true, 중심은 ActionRoot보다 y=-7.5입니다. Attack315×253은 약115.6×92.85, Idle238×215는 약105.38×95.2로 fit합니다. 포즈별 몸 기준 anchor가 필요합니다. **40px은 검토 배율이며 현재 Battle이 Bee를40px로 고정한 규칙은 아닙니다.**

## 피해 시간·접촉 HOLD

PlayAttack 후 PlayMeleeAttack. 공격자A, 대상T, 수평 방향d=sign(T.x-A.x)에 대해 정지점 D=T-d×145. D.y=T.y이므로 다른 Column에서도 대각 접근 후 같은 높이에 도착합니다. 몸통 자동 회전은 없습니다.

| 구간 | 코드 명목 시간 | 처리 |
|---|---|---|
| 접근 | 0~.16초 | EaseOutCubic A→D |
| 준비 정지 | .16~.25초 | ImpactPause .09 |
| 피해 | 약 .25초 | Sprite 충돌 없이 applyImpact/onImpact |
| 피격 | 약 .25~.43초 | 대상 수평 shake .18 |
| 복귀 | 약 .43~.61초 | .18 뒤 StopAttackAndReturnToIdle |

10FPS의 명목 피해 프레임은 **02(.2~.3초)**입니다. 그러나 재생기는 unscaledDeltaTime, 이동/Wait는 scaled time이므로 TimeScale/프레임 지연에 따라 일치가 보장되지 않습니다. ContactFrame 필드/독침 socket/종별 이동거리/회전곡선은 없습니다. 1초짜리 Attack10은 약.61초에 중단되어 뒤쪽 프레임이 보이지 않을 수 있습니다. 전체10프레임이 실제 공격에서 끝까지 재생된다고 선언하지 않습니다.

기본 플레이어 Image112×126 기준 가로 최대 반폭 Bee57.8+Player56=113.8 <145이므로 최종 정지 위치에서 **사각형 외곽 사이도 최소31.2 UI 간격**입니다. 투명 여백이면 더 큽니다. 다른 대상 크기는 별도 계산해야 하며 이 값은 alpha 실측이 아닙니다. 315×253 안의 침끝 재작화만으로 간격이 자동 해결되지 않습니다.

**확정:** 적 공격의 최종 표적축 +X, Sting 뒤침 끝/진행축이 오른쪽 표적을 향해야 합니다. 독침은 복부 뒤 구조를 유지하고 몸통·복부 회전으로 설명합니다. 머리 앞에 침 부착 금지. 기존 Sting 우하향 관찰과는 차이가 있어 최소 보완이 필요합니다.

**TARGET_CONTACT_HOLD:** 정확 접점/거리·실제 몸통 회전 각도는 미확정입니다. 후속은 대상 Image 안 접점C와 침끝S를 source pixel/normalized 좌표로 선언한 뒤 같은 Canvas 좌표로 변환해야 합니다. 미래 정지점은 C에서 변환된 침끝 offset을 뺀 값으로 계산하되 preserveAspect/flip/몸 기준을 포함해야 합니다. 임의 픽셀 접점·전역145 변경을 이번 계약으로 승인하지 않습니다.

## 재사용 범위·최소 수정

기존 활성 PilotBee Idle10+Attack10과 이전 오프라인20 납품은 원본 보존하며 신규 최종 전투20 완료로 간주하지 않습니다. 이번에 동일 SHA PNG를 decode/전수검사하지 않았습니다. TargetReview는 기존 Attack3+Idle2 5SHA 재납품과 TEST_ONLY 보드뿐이며 새 Sting/Recover0이었다는 판정을 유지합니다.

**수정 대상은 Sting·Recover 두 후보만**입니다. Prepare와 정상 Idle2는 보호 비교 기준입니다. Sting은 복부 뒤침을 유지한 몸통/복부 회전으로 +X 타격축을 읽히게 하고 침뿌리·침끝S·몸 중심을 표시합니다. 정확 각도는 근거 없이 강제하지 않습니다. Recover는 같은 축에서 침을 뽑고 복부/몸통·날개 뿌리·다리를 순차 회수해 Prepare/Idle로 연결합니다. 전체 mirror로 순간 복귀 금지.

검토 시간표 **제안**: 10FPS 스트립00~01 Prepare,02~03 Sting,04~06 Recover,07~09 기존 Idle. 기존 포즈 반복을 이용한 비교이며 새10장 양산 지시가 아닙니다. .25초와02를 표시하되 실제 프레임 이벤트 PASS로 주장하지 않습니다. 현행 약.61초 조기 복귀와 불일치하므로 전체 회수의 runtime 연결은 HOLD입니다. [리뉴얼 담당 지시서](VenomBee_Minimal_StingRecover_ArtBrief_20261103.md)에 제작 전 게이트와 제출물을 분리했습니다. 이번에는 PNG를 만들지 않습니다.

## 권리 출처·사용 구분

[OpenGameArt 원배포 페이지](https://opengameart.org/content/pilotbeegamecharacter)를 직접 조회했습니다. 2016-08-27, 작성자2DPIXX, Idle/Attack 두 시트, **CC BY3.0** 표시입니다. [작성자 프로필](https://opengameart.org/users/2dpixx)은 2DPIXX 사이트를 연결합니다. 반면 로컬 원본 ZIP 내부 p_bee/LICENSE.TXT는 **CC BY4.0 / Jana Ochse, 2D!PIXX**입니다. 보관 LICENSE와 ZIP 내부 원문도 같습니다. 웹 ZIP 직접조회는 Cache miss라 오늘 원격 ZIP 바이트 동일성은 NOT_VERIFIED입니다. 권리자 문의·답변을 받은 사실은 없습니다.

| 구분 | 경로/참조 | 확인 |
|---|---|---|
| 외부 보관 | ThirdParty/2DPIXX/PilotBee/Original | 해당 보관 meta GUID의 asset/Scene/Prefab/controller 직접참조0 |
| 활성 사본 | _Project/Resources/MonsterValidation/PilotBee | 02_VenomBee GUID ...04/...05가 참조, Validation이라는 폴더명과 실제 사용은 별개 |
| Spawn | Field01_VenomBee_01~03 | 정의 GUID b33f06912b2a48d0afcd626d1d88fd30 연결 |
| 신규 후보5 | Temp/ArtOwlBeeSpiderReview20261103/Extracted/Bee/Bee/Source_KEEP | 제품 연결 없음, 다른 SHA만으로 독립 권리 입증 아님 |

보관 파일 자체는 직접 미참조여도 **동일 바이트 활성 사본은 실제 로더 경로에 있습니다**. ThirdParty README의 ‘실 Field·전투 미연결’은 현재 정의와 충돌하는 오래된 설명입니다. 원본 README를 바꾸지 않고 여기서 차이를 보고합니다. 실제 현재 게임 프로세스/GameView를 이번에 실행한 것은 아닙니다. [SHA·ZIP 내부 원문](Evidence/BeeTargetRightsApproval20261103/BeeSourceMapping.json), [보관 GUID 참조 범위](Evidence/BeeTargetRightsApproval20261103/ThirdPartyDirectReferences.json).

- 원본 ZIP SHA `c979b691e76e850fb8c1949d8bf53fe55b0b085b0a5012d83d5f6262ae48a2b6`.
- Idle 활성=보관=ZIP SHA `febcca0316167d50d4638216d9534ed2974de01c62e1c0c2daaba849726b6c63`.
- Attack 활성=보관=ZIP SHA `0e8886c56d7670fb317d4cada41a03e4684f9cf97adf908f2b860740630c018e`.

[CC BY3 공식 조건](https://creativecommons.org/licenses/by/3.0/)과 [CC BY4 공식 조건](https://creativecommons.org/licenses/by/4.0/)은 조건 준수하에 공유·수정·상업 사용을 허용하지만 이 패키지 버전 충돌을 해결하지 않습니다. 배포 전 작품명 Pilot_Bee_Game_Character/제작자/출처/정확한 라이선스 링크/수정 내역·제공 고지를 기록하고 추가 법적·기술적 제한을 걸지 않는 조건을 확인해야 합니다. BY3는 제공된 작품명 표시도 확인합니다. 오늘 한 버전을 유일한 정답으로 확정하지 않았습니다.

**RIGHTS_HOLD 유지:** 해당 ZIP의 권리자 버전 확인 또는 충돌 없는 권리 근거가 필요합니다. 신규 후보는 제작 과정·사용 원본·외부 참조/권리·독립 제작 여부 SourceDeclaration을 별도 제출해야 합니다. ‘새 SHA/오리지널 이름’만으로 CC 의무 제거를 선언하지 않습니다. 기존 게임·크레딧·외부 원본 삭제/교체0.

## 보호와 다음 게이트

① 방향/anchor 계획·SourceDeclaration → ② 기획·구현의 정확 접점/거리·동기화 계약 승인 → ③ Sting/Recover만 새 독립2후보·10FPS 비교 → ④ 최소 검증·대표 미술 판단 → ⑤ 별도 제품 적용. 승인 Owl/Spider는 추가 전수 재작화하지 않습니다.

원본 main HEAD/status 유지, 개인 Save/UserData 열람·해시·복사·수정0. Unity 실행0, #84 37/#83 90/저장QA 반복0. worktree Unity/Client diff0. 원본 Scene/Prefab/BuildSettings/Packages/ProjectVersion/아트/TTS/Discord66 변경0. **LOCAL_DOCUMENTED / NOT_UNITY_APPLIED / REMOTE_NOT_PUSHED**. 실제 커밋·댓글 영수증은 후속 기록합니다.
