# Project-Limitless 현재 개발 상태

## 기준

- 갱신일: 2026-09-09
- 기준 브랜치: `main`
- 마지막 기능 관련 commit: `4027d2a` (`Feature: 길 공식 아이콘 Sprite 연결`)
- 마지막 오류 수정 commit: `486ff5a` (`Fix: 월드 경험치 바 채움 비율 수정`)
- 마지막 관련 문서 commit: `ace782e` (`Docs: 사수 전용 야수 동료 설계 확정`)
- 마지막 전투 UI 관련 commit: `7de1918` (`Refactor: 전투 스킬 설명 UI 정리`)
- 마지막 몬스터 후보 에셋 commit: `849bc12` (`Chore: 독 몬스터 후보 에셋 보존`)
- 마지막 Pilot Bee 검증 오류 수정 commit: `7a07f2a` (`Fix: Pilot Bee 검증 Scene 입력과 Camera 수정`)

이 문서는 완료된 기능과 미구현 범위를 빠르게 파악하기 위한 상태 요약이다. 세부 설계는 각 시스템 문서를 따른다.

## 최근 길 공식 표시와 동료 길 적용

- 2026-09-09: `PlayerPathDefinition` 표현 자료를 자체 제작 공식 `PathSymbol`과 Game-icons.net 전투 `TraitIcon`으로 분리했다. 새 청각·시각·지체 심볼과 기존 마음의 상처·지적 심볼로 5개 공식 문장을 완성했으며, 캐릭터 Variant 외형을 공식 문장으로 사용하지 않는다.
- PathSelection 카드와 상세는 큰 PathSymbol·길 이름·작은 TraitIcon·특성 이름·추천 순서로 표시한다. 전투 상세는 두 아이콘과 두 이름을 분리하고, HUD의 `path.*` 상태는 TraitIcon과 한글 상태명을 함께 표시한다. 플레이어와 태온(지적/패턴 익히기), 미엘(마음의 상처/회복탄력)은 같은 Resolver를 사용한다.
- SaveData는 기존 PathId만 유지한다. `traitStatusMarkerIds`로 런타임 상태와 TraitIcon을 연결하며 Sprite 누락 시 Image만 숨기고 텍스트는 유지한다. Path 전투 수치와 NPC Runtime은 변경하지 않았다.
- 새 자체 제작 원본 3개는 `Assets/_Project/Art/Characters/PathVisuals/Symbols/`, 기존 2개는 원래 위치를 유지한다. Game-icons.net White/Black 10개와 라이선스도 원본 그대로 유지한다.
- 위치: `Assets/ThirdParty/GameIconsNet/Path/{White,Black}/`, 라이선스: `Assets/ThirdParty/GameIconsNet/license.txt`. 저작자·정확한 압축 내부 경로는 `외부에셋.md` 참조. 원본 PNG 10개 및 두 압축 라이선스의 SHA256 일치, 5개 Sprite GUID 연결을 확인했다. Sprite/Single·Bilinear·무압축·Alpha·비율 유지와 기존 Null 방어를 확인했다.
- 새 PNG 3개가 다운로드 원본과 SHA256 일치하며 5개 PathSymbol·5개 TraitIcon GUID가 각각 한 Sprite `.meta`로 해석됨을 확인했다. 전체 Assembly-CSharp 정적 컴파일 오류 0개, 기존 CS0618 경고 4개이며 관련 diff 검사를 통과했다. 실제 Unity Play Mode의 다섯 카드, 상세, HUD는 수동 확인한다.
- 다음 확인: Bootstrap 새 캐릭터 → PathSelection의 서로 다른 공식 심볼 5개와 작은 TraitIcon → 임의 직업 → Battle 플레이어 상세 → 태온(수호자/지적/Companion Emblem/Mesh Network) → 미엘(치유사/마음의 상처/Heart/Heart Shield) → 런타임 상태 TraitIcon과 Console 오류.

- `PathPresentationResolver`가 PathId로 공식 PathSymbol·길 이름·TraitIcon·특성·추천·설명을 같은 `PlayerPathDefinition`에서 제공한다. PathSelection과 전투 상세가 같은 Resolver를 사용한다.
- PathSelection을 상단 안내, 가로 5개 카드, 하단 상세 패널과 `이 길을 선택` 버튼 구조로 개편했다. 아이콘·이름·특성·한 줄 추천을 함께 표시하고 선택 시 배경, 굵은 테두리, 1.03배 확대, `✓ 선택됨`을 함께 사용한다.
- 공식 PathSymbol은 자체 제작 5종이며 Heart Shield, Sound Waves, Eye Target, Cog, Mesh Network는 White 기본의 TraitIcon이다. 누락 시 텍스트를 유지하는 방어도 유지한다.
- `BattleParticipantSetup.PathId`를 추가해 플레이어는 저장 PathId, 태온은 지적의 길, 미엘은 마음의 상처를 데이터로 전달한다. 이름 문자열 비교와 추천 강제는 없다.
- `PathCombatTraitRuntime`은 PathId가 있는 여러 실제 Combatant를 한 전투에서 독립 관리한다. 태온의 패턴 익히기와 미엘의 회복탄력·행동 2회 소비가 플레이어와 동일한 피해·치유 경로에 실제 연결된다.
- 전투 상세 팝업은 PathSymbol·길 이름과 TraitIcon·특성 이름을 분리하고, 집중·잔향·패턴 등 임시 상태는 TraitIcon을 쓰는 기존 상태 영역에 따로 표시한다. Field 이름표 `Lv.n 이름`은 변경하지 않았다.
- 전체 Assembly-CSharp 응답 파일 별도 컴파일 오류 0개, 기존 deprecated API 경고 4개. 관련 파일 `git diff --check` 통과. 실제 Game View의 5카드 배치와 태온·미엘 패시브 발동, 저장 이어하기는 수동 확인이 남았다.
- 상세: `문서/02_세계관/Path_시스템.md`, `문서/11_UI/길_선택.md`, `문서/10_전투/전투시스템.md`. 마지막 기능 commit: `a77c17d`.

## 최근 길 전투 특성 구현

- `SelectedPlayerPathId`를 전투 시작 때 한 번 읽는 `PathCombatTraitRuntime`을 추가했다. 플레이어에게만 적용하고 태온·미엘 및 추천 외 직업에는 별도 제한이 없어 5×5 조합이 모두 같은 경로를 사용한다. 전투 종료 뒤 상태는 저장하지 않는다.
- 마음의 상처 `회복탄력`은 아군 HP가 처음 50% 이하로 실제 내려갈 때 전투당 1회 발동해 다음 플레이어 성공 행동 2회의 직접 피해·치유를 10% 높인다. 청각 `잔향 포착`은 공격을 마친 실제 적별 잔향과 소비·다음 행동 시작 만료를 처리한다.
- 시각 `집중`은 같은 실제 적 단일 공격을 최대 3중첩으로 추적해 현재 공격에 0/3/6/9%를 적용한다. 광역은 기존 집중 대상만 보정하고 중첩을 만들지 않는다. 지체 `굳건한 자리`는 전열 직접 피해 -5%, 후열 직접 피해·치유 +5%다.
- 지적 `패턴 익히기`는 실제 적·아군 참조 쌍을 추적해 반복 직접 공격 뒤 다음 해당 피해를 10% 줄이고 소비한다. 패턴 보유 아군에게 플레이어가 주는 직접 치유는 대상별 10% 증가하며 패턴을 소비하지 않는다.
- 길 배율은 기존 방어·철벽·가이아·수호의 맹세 앞에서 한 번만 적용한다. 화상·독 DoT에는 길 배율을 적용하지 않고 회복탄력의 실제 HP 경계 통과만 관찰한다. HUD와 상세 팝업은 회복탄력·잔향·집중·굳건한 자리·패턴 및 대상 정보를 텍스트로 표시한다.
- 길 데이터의 직접 능력치 보너스를 비우고 캐릭터 생성 계산과 화면을 `기본 10 + 직업`으로 통일했다. 추천 직업 설명은 실제 전투 시너지를 안내하지만 선택을 잠그지 않는다.
- 전체 `Assembly-CSharp` 응답 파일 별도 컴파일 오류 0개, 기존 deprecated API 경고 4개. 관련 파일 `git diff --check` 통과. 실제 Play Mode에서 다섯 길의 수치·HUD·새 전투 초기화와 기존 스킬/DoT/Save 회귀를 직접 확인해야 한다.
- 상세 규칙: `문서/02_세계관/Path_시스템.md`, `문서/10_전투/전투시스템.md`. 마지막 기능 commit: `c86e931`.

## 최근 월드 레벨·EXP UI 구현

- EXP Fill에 Source Sprite가 없어 uGUI가 `Image.Type.Filled`와 `fillAmount` 대신 전체 사각형을 그리던 문제를 수정했다. 1×1 흰색 런타임 Sprite를 연결하고 왼쪽 시작 Horizontal Filled 방식 하나로 실제 길이를 갱신한다.
- 기존 Screen Space Overlay `PlayerNameplate`에 현재 슬롯의 Level을 연결해 머리 위에 `Lv.n 이름`을 흰색으로 표시한다. 위치·피벗·Camera 추적은 유지하고 몬스터 난도 색은 적용하지 않는다.
- 같은 Overlay Canvas 하단 중앙에 `Lv.n 이름`, `EXP 현재 / 필요`, 금색 진행 Bar를 표시하는 얇은 World HUD를 추가했다. 필요 EXP와 만렙은 기존 중앙 성장 API를 사용하며 Lv50은 `MAX LEVEL`과 Bar 100%로 표시한다.
- 이름·Level·CurrentExperience 값이 실제로 달라졌을 때만 갱신한다. Scene별 Player/Canvas 생명주기와 이름 기반 재사용으로 World 전환 중 중복을 막으며 Battle에는 생성하지 않는다.
- 격리 Unity 6000.5.7f1 빈 Scene Play Mode에서 0/24/50/75/99%, Lv3 85/170=50%, 레벨업 후 Lv2 20/130≈15.4%, Lv50=100%의 fillAmount와 실제 렌더 메시 폭 검사를 통과했다. 실제 Game View의 배치와 저장 슬롯 이어하기·SceneTransition·정상 전투 복귀는 수동 확인이 남았다.
- 상세: `문서/11_UI/월드_레벨과_EXP_HUD.md`. 마지막 기능 commit: `abd0175`, Fill 수정 commit: `486ff5a`.

## 최근 초반 성장과 공용 독 구현

- MaxLevel 50, 다음 레벨 요구 EXP `100 + 25*(L-1) + 5*(L-1)^2`(Lv1~49). CurrentExperience는 현재 레벨 진행량이며 초과분 이월·다중 레벨 업·Lv50 EXP 0을 처리한다.
- 슬라임/독침벌/숲거미/맹독뱀의 Level은 1/2/3/4, Base EXP는 8/12/16/20, HP는 60/70/80/95, 기본 공격은 10/12/14/16이다. Field_01 권장 Lv1~3, Field_02 Lv3~5.
- 몬스터와 플레이어의 레벨 차이로 이름색과 EXP 배율을 공용 판정한다. 회색/녹색/노랑/주황/빨강은 각각 0/50/100/125/150%이며 몬스터 이름에 Lv를 표시한다. 흰색은 플레이어/NPC용으로 유지한다.
- Field_02 기존 거미 4개를 유지하고 맹독뱀 2개를 추가했다. 원본 CC0 이동 4프레임을 프로젝트 전용 복사본에서 재사용하고 전투에서는 오른쪽을 향한다. 뱀 조우는 거미 2+뱀 1, 처치 후 30초 리스폰이다.
- 공용 PoisonDefinition으로 일반 독 최대 HP 5%×3회·맹독 7%×3회를 처리한다. 강한 독은 교체, 약한 독은 거절, 동급은 지속시간만 갱신하며 중첩하지 않는다. 정화·방어 무시 Tick·개체별 부여 대기시간을 유지한다. 향후 투사 독칼/사수 독화살도 같은 구조를 사용할 예정이다.
- 승리 시 실제 처치 개체별 EXP를 합산하고 결과 패널에 EXP/레벨 업/진행량을 표시한다. 현재 슬롯에 Level/EXP와 마지막 월드 위치를 즉시 저장하며 다른 슬롯과 전투 중간 상태는 저장하지 않는다.
- 정적 Assembly-CSharp 컴파일 오류 0개, 기존 CS0618 경고 4개. 격리 Unity 프로젝트에서 성장/배율/독/5개 슬롯/15개 버튼 감사와 실제 Field→Battle→승리→Field→31초 리스폰 자동 Play 검증 통과. Editor Search 패키지 시작 예외 1건은 별도로 관찰됐으며 게임 감사 실패는 없었다.
- 실제 사용자 Editor의 정상 전투 입력·시각적 애니메이션·전 직업 스킬/VFX/Timeline/Targeting 회귀는 수동 확인이 남았다. 자동 전투 검증은 적을 테스트 코드로 처치했다. 레벨 업 HP/MP 완전 회복은 미확정이며 기존 새 전투 시작 시 최대 HP/MP 생성 규칙을 변경하지 않았다.
- 상세 규칙·수정 파일·검증 범위: `문서/08_몬스터/초반_성장과_공용_독.md`. 마지막 관련 commit: `5c5a34c`.

## 최근 스킬 선택 버튼 아이콘 수정

- 파이어 볼 Warm Explosion index 4·썬더볼트 Electric Impact index 1·정화 Spectral Bloom index 5를 버튼 전용 고정 Sprite로 생성하고 null/파괴된 캐시는 다시 읽는다. 상세·상태·VFX 로더와 전투 계산은 변경하지 않았다.
- 기존 캐시는 파괴된 Sprite도 그대로 반환해 버튼 자식 Image가 생성되지 않았고, Rebuild로 복구되지 않았다. 수정 전 파괴 캐시 재현 실패를 확인했다. 실제 사용자 세션에서 최초 무효화를 일으킨 이벤트는 미확정이다.
- 추가로 Play 진입 때 동료의 습격 Wolf 버튼의 무효 캐시 재사용을 확인·복구했다. Wolf Run index 4와 나머지 정상 매핑은 유지한다. 현재 코드/문서 기준 도발은 pawn_left, 치유의 빛은 suit_hearts다.
- 15개 스킬의 실제 버튼 생성 감사: Edit Mode 및 빈 Scene Play Mode 2회, 각각 3회 재생성/부모 닫기·열기에서 Sprite 할당·enabled·activeSelf·alpha·캐시 재사용 통과. 세 Grid 버튼의 파괴/null 캐시 복구 후 Image 재할당도 통과했다.
- 전체 Assembly-CSharp 정적 컴파일 오류 0개, 기존 deprecated API 경고 4개. Unity 배치 Editor 컴파일 및 관련 staged diff 검사 통과. 상세 매핑/검증 범위는 `문서/10_전투/전투_UI_아이콘_에셋.md` 참조.
- 실제 Battle의 마도사 → 치유사 → 수호자/사수/투사 각 3개 버튼을 눈으로 확인하고, `열기 → Esc → 다시 열기` 반복·다음 턴·다음 전투 유지 여부를 수동 확인해야 한다. 자동 감사는 전투 진행을 실행하지 않았다.
- 마지막 관련 commit: `c870aaa`.

## 구현 확인된 항목

### 캐릭터 생성과 선택

- `Level + JobId` 기반 결정적 6능력치 성장과 Lv50 중앙 상한 구현. 딜러 주 스탯 평균 +1.5 정수 패턴, 체력 HP, 직업별 HP 성장, 수호자/치유사 의지 파생 수치를 계산하며 Path는 성장 계산에서 제외. 수호자·치유사의 미확정 6능력치 레벨 성장은 적용하지 않음
- 파생 능력치는 저장하지 않고 각 슬롯의 Level/JobId로 복원. 경험치 곡선·몬스터 EXP·현재 캐릭터 승리 보상을 구현했으며 향후 파티 EXP 분배 정책은 미확정

- Bootstrap의 5개 캐릭터 슬롯 목록과 슬롯별 `이어하기 / 새 캐릭터` 런타임 UI. 새 캐릭터는 기존 4단계 생성 흐름 유지
- `GameSaveData` Version 1 JSON을 Unity Editor 프로젝트 로컬 `UserData/Saves/save_slot_01.json`~`05.json`에 독립 저장
- 선택 슬롯만 자동 저장하며 손상 슬롯은 다른 슬롯에 영향을 주지 않고 사용 불가로 표시
- 기존 `Application.persistentDataPath/project_limitless_save.json`은 슬롯 1이 비었을 때만 검증·복사하고 원본 유지
- FinalConfirmation 확정, 정상 마을/Field 전환 완료, Battle 종료 뒤 Field 복구 시 자동 저장하며 Battle 중간 상태는 제외
- 손상 JSON·Version 불일치·잘못된 Visual/Path/Job/Scene을 삭제·덮어쓰기 없이 거부하고 새 게임으로 안전 복귀
- Editor 전용 `Project Limitless/Test/Manage Local Saves` 창의 슬롯별/전체 삭제 기능
- WorldBounds가 있는 마을/Field에서 5초 주기·Pause/Quit·Scene 도착·Battle 복귀 시 실제 float 좌표를 선택 슬롯에 저장
- 이어하기는 같은 Scene의 Bounds 안 유효 좌표를 우선하고 좌표 없음·NaN/Infinity·Bounds 밖이면 기존 SpawnPoint로 fallback
- SceneTransition 시작 시 이전 Scene 좌표를 무효화하고 목적지 Spawn 배치 뒤 새 좌표 저장
- Bootstrap 저장 슬롯의 `삭제` 버튼, 캐릭터 정보·복구 불가 경고 확인창, 해당 슬롯만 삭제 후 즉시 빈 슬롯 갱신

- `Bootstrap → CharacterCreation → PathSelection → JobSelection → FinalConfirmation → World_StarterVillage` Scene 흐름
- 이름 입력과 검증
- Male/Female 기본 외형 선택과 세션 유지
- 5개 길 선택 및 길별 전투 패시브 프리뷰, 길 직접 능력치 보너스 없음
- 길별 추천 직업 안내와 모든 5×5 조합 선택 허용
- 5개 직업 선택
- 직업 능력치, 패시브, 시작 스킬 3개 프리뷰
- FinalConfirmation의 선택 결과와 최종 능력치 표시

### 플레이어 외형과 월드 표시

- Player Nameplate
- Path Visual 데이터와 UI Preview
- 청각·시각·지체 길의 Male/Female Character Variant
- 지체 길의 전투형 전동 휠체어 Variant
- 지적 길의 동행의 문장 Symbol
- 마음의 상처 길의 이어진 심장석 Symbol
- 기본/Variant 공통 Player 이동과 방향 Animation 구조

### 마을과 필드

- `World_StarterVillage`의 Player 이동, Camera 추적, NPC E 대화
- `Field_01` Scene과 야외 환경
- 마을 남문 → Field_01 북쪽 입구 이동
- Field_01 북쪽 입구 → 마을 남문 복귀
- `SceneTransitionService`, `SceneTransitionTrigger`, `SceneSpawnPoint`
- 공통 `WorldBounds2D`와 `WorldBoundaryGeneratorUtility`
- Orthographic viewport 크기를 고려한 Camera Bounds
- Exit Opening을 제외한 월드 외곽 Collider
- 마을 외곽 울타리의 Tile 영역 밖 돌출 방지

### 첫 필드 몬스터

- 모든 Field 출입구에 데이터 기반 원형 몬스터 안전지대 적용. 생성점과 활동 반경 전체를 보정하며 2초 재조우 유예와 별도로 유지
- Field_01 북/남 안전지대와 Field_01↔Field_02 데이터 기반 양방향 연결
- Field_02 숲길 기본 Scene·그늘 테마·World/Camera Bounds 재사용, 숲거미 4개 스폰
- CC0 Spider Idle/Walk/Attack/Shoot 선별 프레임 연결, `독액 분사` 생존 아군 전체 직접 피해 20 구현
- Field_02 거미 조우를 전열 독침벌 2·후열 숲거미 1로 변경하고 숲거미 기본 공격 14, 독액 분사 20을 분리 유지
- 신규 Field/같은 Field 신규 몬스터가 직전 최강 일반 몬스터와 함께 등장하고 약 10~15%를 출발점으로 Play Mode에 맞춰 데이터 조정하는 공통 성장 규칙 문서화
- 같은 MonsterDefinition을 공유하는 복수 몬스터도 실제 Combatant 참조별로 감전·독·화상·독침 대기시간을 독립 저장하며, 썬더볼트 피해 처리 뒤 살아남은 EnemyAll 각 대상에 감전 1을 별도 단계로 적용

- 데이터 기반 `MonsterDefinition`과 Scene별 `FieldMonsterSpawnDefinition`
- Field_01 여러 빈터의 `초원 슬라임` 5마리 데이터 기반 런타임 배치
- 스폰별 시작 위치와 활동 반경 안의 독립적인 느린 무작위 배회
- Rigidbody2D 기반 장애물 Collider 충돌
- 플레이어 접촉 시 이동 정지와 `MonsterEncounterService.EncounterStarted` Event 발생
- 사용자 제공 4×4 Sprite 시트 기반 초원 슬라임 방향별 Idle/Walk Animation
- 배회 방향·이동/대기 상태와 `GrassSlime.controller` 동기화
- 초원 슬라임 데이터에 실제 Sprite/Animator 연결 및 녹색 Placeholder 미사용
- 승리한 스폰만 제거하고 데이터 기본값 30초 후 원래 위치에 독립 리스폰, 도망 시 스폰 유지
- `MonsterDefinition.DisplayName`을 표시하는 재사용 가능한 필드 몬스터 Overlay 이름표
- 레벨 차이별 이름색·Lv 표기·검은 외곽선의 이동 추적 이름표, 필드 HP Bar 미포함
- 독 몬스터 원본 보존: Pilot Bee(CC BY, 라이선스 버전 표기 충돌 기록)·2D Spider(CC0)·Simple Green Snake(CC0). 벌은 Field_01, 거미와 맹독뱀은 Field_02에 구현
- Pilot Bee 검증 Scene: Idle 238×215×10·Attack 315×253×10·기본 우향 구조를 런타임 분할하고 현재 초원 슬라임과 나란히 비교. Point Filter·무압축·투명·Read/Write 검증 복사본과 기본 Scale 0.85 제공, Field/Battle 미연결
- Pilot Bee 검증 Scene 입력을 새 Input System의 null 안전 `Keyboard.current` 방식으로 수정하고 메인 키보드·Numpad +/-를 지원. Scene 전용 직교 Main Camera를 연결해 `No cameras rendering` 표시 제거
- Field_01에 데이터 기반 독침벌 3개 스폰(`venom_bee_01`~`03`)을 추가해 기존 초원 슬라임 5개와 총 8개 배치. 공용 설치기·배회·접촉 조우·개별 30초 리스폰·도망 유지·2초 재조우 유예 재사용
- `02_VenomBee` MonsterDefinition이 Pilot Bee Idle/Attack 시트 구조와 Scale 0.85를 Field/Battle에 공통 제공. 런타임 분할 재생으로 원본 PNG를 수정하지 않으며 공용 일반 독을 부여

### 1차 턴제 전투

- 치유사 MP 숫자를 상단 HP HUD에서 제거하고 `UsesMp` 참가자의 Hover/Focus 상세 팝업 HP 다음 줄로 이동. 다른 직업은 빈 MP 줄을 표시하지 않음
- 모든 스킬 Tooltip·재사용 중 UI 용어를 `재사용 대기시간`으로 통일하고 공통 Tooltip 생성 경계에서 `구현: 사용 가능/미구현` 개발 상태 문구를 제거. MP 사용 스킬은 고정 비용을 함께 표시
- 화살비·회오리 베기·썬더볼트 광역 Target과 실행기에서 실제 Combatant 참조 중복을 제거해 대상당 피해 1회를 보장. 빈 슬롯·VFX 수는 피해/기세 횟수에서 제외하며 독침벌 A/B는 별개 인스턴스로 유지. 숲거미 독액 분사도 같은 참조 안전장치 적용
- 적대 광역 스킬은 스킬 메뉴를 닫기 전에 유효 Target을 검사한다. 화살비 후열 0명·회오리 베기 전열 0명·썬더볼트 전체 0명이면 범위별 경고 후 스킬 메뉴·취소·키보드 포커스를 즉시 복구하며 행동·턴·자원·쿨타임·VFX를 시작하지 않음
- 치유사 전용 MP 런타임 자원과 `MP 현재/최대` HUD 구현. MaxMP는 `임시 기본값 + (Level-1)×1 + 지능×2`로 재계산하며 레벨 성장 +1과 지능 계수 +2는 확정. 자기 행동 종료 회복 및 치유의 빛/회복의 파동/정화 선검사·성공 차감 적용
- 치유사 스킬은 고정 비용으로 `정화 < 치유의 빛 < 회복의 파동`을 유지한다. 치유의 빛은 쿨타임 없음, 회복의 파동 3턴·정화 2턴은 유지하며 기본 MP·회복량·실제 비용 숫자는 임시값, 레벨업 시 MP 완전 회복 여부는 미확정
- 행동 우선도→민첩→전투 시작 1회 Tie Break 순서 구현. 실제 Combatant 참조별 값을 전투 동안 유지하고, 민첩 동률 전투에서만 Overlay를 열어 전투 RNG와 분리된 지역 `System.Random`으로 1~6 눈을 0.09초 간격으로 1.2초간 변경. 최종 눈도 직전 값과 다르게 별도 추첨하며 입력 잠금과 실제 첫 행동/Timeline 판정 유지
- 회복의 파동·정화·철벽·수호의 맹세·가이아 웰 VFX는 캐릭터 본체와 독립된 Image만 사용한다. Image는 생성 즉시 숨기고 첫 유효 Sprite·Tint 설정 뒤 표시하며, null 프레임은 숨기고 종료 즉시 비활성화·파괴한다. CharacterSprite 뒤 강제 배치를 제거해 원본 후광의 안쪽이 가려져 바깥 색 레이어처럼 분리되는 현상을 방지했고, 수호의 맹세 이전 섬광의 의도적 null Sprite도 유효 Orb Sprite로 교체

- 재사용 가능한 `Battle` Scene과 Build Settings 연결
- `Combatant`, 전열/후열 2×3 `Formation`, `TargetResolver`, `TurnOrderQueue` 분리
- 근거리 전열·직선 후열 보호, 원거리 후열 우선, 마법 자유 대상 판정
- 플레이어와 몬스터 공통 대상 판정 및 단일 행동 도발 강제 대상 구조
- 민첩과 행동 우선도 기반 순서 및 다음 행동 타임라인
- 플레이어 이름·현재 외형, 초원 슬라임 Sprite와 양측 HP 표시
- 마우스·키보드 지원 남색/금색 전투 UI와 공격 가능·불가능 텍스트 구분
- 기본 공격, 다음 행동까지 피해 50% 감소 방어, 일반전 도망
- HP 0 전투불능, 적 전멸 승리, 승리·도망·패배 후 `Field_01` 복귀
- 복귀 시 전투 상태 초기화, 접촉 위치 이격과 2초 조우 유예
- 스킬 버튼과 NPC 직접 지시 확장용 `IsPlayerControlled` 구조
- 기본 공격 사거리: 수호자·투사 근거리, 사수 원거리, 마도사·치유사 마법. 치유사는 낮은 피해의 마법 공격
- 논리 2×3 Formation을 숨기고 적 왼쪽·아군 오른쪽의 사이드뷰 전장 좌표로 Sprite 배치
- 타임라인 아래·전장 위의 상단 HP 전용 HUD와 적군 3열×최대 2줄·아군 3열×1줄 이름/비율 HP 항목
- 캐릭터 근처 이름·HP·상태 텍스트 제거, Sprite 가로 중앙 위 대상 화살표와 행동 중 표시만 유지
- 상단 HP 항목의 행동 중·방어·도발·구현 스킬 재사용 턴 한 줄 요약과 상세 팝업 역할 분리
- 전투불능 시 HP HUD·상세 팝업의 행동 상태/쿨타임 제거와 회색 `전투불능` 단일 표시
- Hover/마우스 선택/키보드 포커스 공용 단일 상세 상태 팝업과 포커스 캐릭터·고정 HP 행 동시 강조
- 평상시 회색 발판 제거, 대상 선택 중 공격 가능 대상만 얇은 금색 선과 선택 화살표로 표현
- 상단 한글 `전투` 제목과 현재 행동자 강조·이후 순서 타임라인, 하단 전용 명령 패널
- 조작 안내를 13px 글씨로 명령 패널 내부 하단에 배치하고 버튼과 하단 padding을 확보
- 약 17% 축소한 공용 명령 버튼·축소 명령 패널과 대상/스킬 선택 중 Esc와 같은 흐름을 실행하는 마우스 `취소` 버튼
- Kenney Game Icons·Board Game Icons CC0 실제 Sprite와 한글을 함께 사용하는 공격·스킬·방어·도망·취소 버튼 및 HP HUD 상태 배지
- 역할 ID와 Resources 경로를 한곳에서 연결하는 `BattleUiIconCatalog`, Sprite 누락 시 문자 기호 없이 한글만 남기는 fallback
- HP HUD 도발은 `pawn_right`, 3턴 재사용은 표시값 3=`hourglass_top`·2=`hourglass`·1=`hourglass_bottom`으로 구분
- 직업 스킬 데이터의 `IconId`를 통해 수호자 도발=`pawn_left`, 치유의 빛=`suit_hearts`, 정조준=`target` Sprite를 스킬 이름 왼쪽에 표시
- 스킬 버튼은 아이콘·이름 중심으로 단순화하고, Hover와 키보드 포커스가 공유하는 팝업에서 설명·대상·효과·재사용·구현 여부를 표시
- 도발·치유의 빛·정조준의 현재 구현 규칙과 일치하는 설명 데이터를 `BattleSkillDefinition`에 연결하고 취소 아이콘을 `arrowLeft`로 교체
- 외부 신규 에셋 없이 남색·금색 하늘·원경·지면 층의 임시 전투 배경 구성
- 플레이어·NPC·몬스터가 공통 사용 가능한 `BattleActionPresenter`와 전투 계산 분리
- 근거리 기본 공격의 짧은 전진·타격 대기·원위치 복귀, 피격 좌우 흔들림·점멸, 떠오르는 피해 숫자
- 연출 중 명령·대상 선택·취소 입력 잠금과 연출 완료 후 다음 턴 진행
- 사수 기본 공격의 짧은 조준, 재사용 가능한 UI Projectile 이동, 도착 시 피해·피격 연출과 다음 턴 연결
- 마도사·치유사 기본 공격의 짧은 캐스팅과 Projectile, Fireball·밝은 금빛 구체 시각 구분
- 필드 Animator 현재 상태와 분리된 전투 Sprite 해석기, 아군 Left Idle·적 Right Idle 진입 및 공격 후 복구
- 전투 적 구성을 전열 초원 슬라임 A/B와 후열 독침벌 1로 변경. 독침벌은 원본 우향 Idle을 반복하고 기본 공격 중 Attack 10프레임으로 전환한 뒤 Idle 복귀, 피해 계산은 기존 적 기본 공격 유지
- 독침벌 기본 공격은 MonsterDefinition의 120% 데이터로 슬라임 10 대비 12 피해를 매 행동 적용. 독 부여 가능 공격만 독 3 부여·갱신 후 공격자별 독침 대기 2를 시작하고, 이후 해당 벌의 행동 종료에만 `2→1→0` 감소하여 네 번째 행동부터 다시 부여
- 대상의 독은 자신의 행동 종료마다 최대 HP 5% 올림·최소 1 피해로 `3→2→1→제거`되고 재적중은 독 3 갱신이다. 대상 독 지속시간과 벌의 독침 대기시간은 별도 저장되어 정화·자연 종료가 공격자 대기시간을 초기화하지 않음
- 독 HUD·상세 팝업에 PVFX Venom Ward index 6 아이콘과 `독 n`, `행동 종료 시 최대 HP 5% 피해` 표시. 틱은 작은 Acid Splash index 3~8과 피해 숫자만 사용하며 화상과 함께 있으면 두 틱 후 다음 턴 진행
- CC0 Polar_34 - Projectiles 원본 GIF 보존, 32×32 PNG Sprite 프레임 변환 및 사수 golden arrow·마도사 fireball 기본 공격 적용
- 시작·목표 X 좌표 비교 기반 공용 Projectile 좌우 반전과 GIF 프레임 지연 재생
- PVFX Foundry 0.3.0 CC0 원본·라이선스 보존, Magical Projectile travel 5프레임을 치유사 기본 공격에 적용
- 치유사 치유의 빛: 자신 포함 살아 있는 단일 아군, 최대 HP 35% 올림 회복, 최대 HP·전투불능 안전 처리
- 치유사 회복의 파동: Formation의 살아 있는 아군 전체(자신 포함)를 치유의 빛 기본 회복량의 60%로 동시 회복, 전투불능 제외·최대 HP 상한·회복 대상 없음 행동 미소비·치유사 행동 기준 3턴 쿨타임
- 회복의 파동 연출: 치유사 중심 Arcane Parry 1회와 대상별 작은 Radiant Heal 병렬 재생, Radiant Heal peak에서 전체 HP·회복 수치·HUD 동시 갱신, peak 고정 프레임 버튼/상세 아이콘 분리
- 치유사 정화: 살아 있는 아군 1명의 독·화상·감전을 `HarmfulStatusType` 공통 분류로 모두 제거, 상태 없음 행동 미소비, 치유사 행동 기준 2턴 쿨타임
- 정화 연출: PVFX spectral-bloom 96×96 전체 16프레임을 대상 위치에서 20 FPS로 재생하고 release index 7에서 상태·HUD 갱신, peak index 5 고정 버튼/상세 아이콘 분리
- PVFX Radiant Heal 96×96 14프레임을 대상 위치에서 재생하고 peak 7프레임에 회복·`+회복량`·HUD 갱신
- 치유 대상 선택 중 Esc/취소는 스킬 메뉴로, 스킬 메뉴 취소는 기본 명령으로 돌아가는 단계별 입력 흐름
- 사수 정조준: 기존 원거리 TargetResolver 후열 우선, 기본 공격력 160% 정수 올림, 사수 행동 기준 2턴 쿨타임
- `정조준!` 강조와 기존 golden_arrow 0.42초 이동, 도착 순간 피해·HP HUD·피격 연출 적용
- 사수 `화살비`: `EnemyRearRowAll`의 살아 있는 적 후열 전체에 일반 공격 120% 피해, 후열 0명에서는 전열 전환 없이 행동·쿨타임 미소비, 사수 행동 기준 2턴 쿨타임
- 화살비 `bow` 버튼 아이콘과 기존 golden_arrow 기반 조준→3발 상승→공중 대기→대상별 3발 낙하→공유 타격·동시 피격의 약 0.75초 연출
- 다중 Projectile은 순수 연출로 관리하고 실제 피해는 마지막 낙하 시점에 후열 대상마다 한 번만 적용한 뒤 전체 피격 완료 후 다음 턴 진행
- 화살비 다중 후열 실검증용 프로토타입 배치: 초원 슬라임 A는 전열 0열, B·C는 후열 0·1열에 배치하여 전열 1명+후열 2명 유지
- 정조준과 화살비 쿨타임은 같은 `BattleSkillCooldowns`에서 참가자·Skill ID별로 독립 관리하며, 화살비는 모든 피격 반응 완료 후 성공 확정 시 2턴 등록
- 사수 `동료의 습격`: 전후열 자유 단일 적, 도발 강제 대상 우선, 일반 공격 180%, 사수 행동 기준 3턴 쿨타임
- 별도 Combatant가 아닌 기본 Wolf가 매 사용 시 현재 사수 `ActionRoot` 위치·크기로 계산한 근처 지점에서 출발한다. 384×40 시트를 64×40 Sprite 6개로 나눈 12 FPS Run Coroutine과 570 UI 단위/초 Transform 이동 Coroutine이 독립적으로 동시에 실행되며, 대상 바로 앞 도착 시 Run을 멈추고 타격 후 0.12초 뒤 제거된다. `BeastCompanionDefinition` 경계로 Bear/Fox 교체 가능
- 모든 스킬 설명 팝업은 메뉴 Hover/키보드 포커스 중에만 표시하고, 스킬 확정 즉시 공통 경계에서 닫아 대상 선택·연출·행동 종료 뒤 기본 명령 화면까지 숨김 유지
- 모든 Battle Action의 `actionPlaying` 중 스킬 설명과 캐릭터/상태 상세 팝업을 함께 억제하고 기존 Hover·포커스 대상을 비워, 마우스가 HUD 위에 남아 있어도 공격·회복·Projectile·VFX를 가리지 않음
- 동료의 습격 버튼은 작은 버튼에서 실루엣이 선명한 Wolf Run 다섯 번째 프레임(index 4) 기반 실제 동물 아이콘을 사용하고 정조준은 기존 Kenney `target.png` 유지. Wolf 경로와 아이콘 프레임 번호는 `BeastCompanionDefinition`에서 제공
- 마도사 `파이어 볼`: 전후열 자유 단일 마법·도발 우선, 즉발 170%, 명중 당시 Attack의 30% 화상 2회, 마도사 행동 기준 3턴 쿨타임
- 화상은 대상 행동 종료 시 작은 fireball 불꽃과 함께 저장 피해를 한 번 적용하며, 재적중 시 중첩 없이 새 명중 피해·2회로 갱신. HUD와 상세 팝업에 Warm Explosion peak 아이콘+`화상 n` 표시
- 파이어 볼 연출은 Solar Shrapnel 초기 Charge 2프레임→기본 공격보다 큰 기존 fireball→Warm Explosion 15프레임이며 index 4에서 즉발 피해·화상 적용
- 마도사 `썬더볼트`: `EnemyAll`로 살아 있는 적 전열·후열 전체에 90% 피해, 대상별 감전 1, 마도사 행동 기준 3턴 쿨타임 구현. 광역이라 단일 도발 강제 대상은 적용하지 않음
- 감전은 다음 행동의 주는 피해를 15% 감소시키고 공격하지 않는 행동도 종료 시 제거되며, 재적중은 중첩 없이 감전 1 갱신. 전투불능 시 저장소와 HUD에서 제거
- PVFX electric-impact 96×96 14프레임을 청백색 예고 뒤 적 전체에서 거의 동시에 재생하고 peak index 1에서 피해·감전을 함께 적용. 스킬 아이콘은 peak 고정 Sprite, 상태 아이콘은 Kenney `power.png`
- 마도사 `가이아 웰`: 자기 자신에게 받는 피해 60% 감소를 부여하고 현재 행동을 제외한 자신의 다음 2회 행동 종료에서만 감소, 마도사 행동 기준 4턴 쿨타임 유지
- 가이아 웰 활성 중 공용 방어 입력은 지정 안내 후 행동·턴 미소비로 차단하고 버튼을 사용 불가 색상으로 표시. 계산에서도 방어 단계를 건너뛰어 두 효과가 절대 중첩되지 않음
- PVFX arcane-parry 96×96 16프레임을 자신 위치에서 20 FPS 재생하고 peak index 8에서 상태 적용. 실제 `dice_shield.png`로 `가이아 2/1` HUD·상세 상태 표시
- 수호자 `철벽`: 자신에게 받는 피해 70% 감소를 부여하고 사용 행동을 제외한 자신의 다음 2회 행동 종료에서만 `철벽 2→1→제거`, 수호자 행동 기준 4턴 쿨타임 유지
- 철벽은 도발과 동시에 유지하지만 공용 방어 50%와는 입력·피해 계산 양쪽에서 중첩을 차단한다. 활성 중 방어 시 지정 안내 후 행동·턴을 소비하지 않고, 원시 피해 10은 기존 올림 규칙으로 3이 됨
- PVFX `earth-rupture` 96×96 20프레임을 발밑에서 20 FPS로 한 번 재생하고 peak index 9에서 상태를 적용한다. Kenney `structure_wall.png`를 스킬 버튼과 `철벽 2/1` HUD·상세 상태에 함께 사용
- 수호자 `수호의 맹세`: 수호자의 다음 행동 시작 전까지 자신을 제외한 같은 진영 생존 아군 전체의 직접 공격·공격 스킬 피해를 최대 50% 감소시키고 감소량 절반을 수호자에게 이전. 고정 3인 목록 없이 진영·생존 상태를 피격 순간 판정
- 수호의 맹세 이전 예산은 사용 시 수호자 최대 HP 40%를 올림 계산하고 철벽 적용 전 이전 예정량으로 소비. 예산 부족 시 실제 이전량의 두 배까지만 감소하며 0 또는 수호자 전투불능에서 즉시 종료. 발동 시 대상별 `수호의 맹세 -감소량` 한 줄과 금색 이전선·섬광 표시
- 이전받은 실제 피해는 철벽 70%·공용 방어 등 수호자의 기존 개인 방어를 정상 적용하며, 화상·독 같은 DoT는 `BattleDamageOrigin` 분류로 보호에서 제외. 도발·철벽과 동시 유지하고 수호자 행동 기준 4턴 쿨타임
- PVFX `frost-nova` index 4~10을 금백색·낮은 알파의 짧은 파티 범위 VFX로 사용하고, 실제 이전 때 피격 아군→수호자 금색 선·섬광 표시. Kenney `pawns.png`를 버튼·수호자 HUD에 사용하고 상세 팝업에 남은/최대 이전 예산 표시
- 투사 `난도`: 기존 근거리 TargetResolver로 적 1명을 선택하고 전진 타격 순간 일반 공격 150% 피해와 자신 기세 +1 적용
- 기존 근거리 기본 공격 Presenter를 재사용하는 전진→타격·피해 숫자·피격 반응→원위치 복귀→다음 턴 흐름
- `난도`는 스킬명, `기세`는 Combatant별 0~3 개인 자원이며 난도 직접 세 번째 사용 기준 2턴 쿨타임과 독립 관리
- 투사 `회심의 일격`: 근거리 단일 공격, 기세 0/1/2/3에 일반 공격 100/130/160/190%, 적중 후 기세 전부 소비, 자체 쿨타임 없음
- `GetMomentum`·`AddMomentum`·`ConsumeAllMomentum` 구조로 회오리 베기 명중당 획득 확장 준비와 회심 소비 연결
- 난도 버튼 `cross`, 기세 HUD·회심의 일격 버튼 `skull`, 상단 `기세 n`·상세 `기세 0/3~3/3`, 전투불능 시 숨김
- 회심의 일격 설명 팝업에 데이터 기반 현재 기세와 현재 예상 피해 배율 표시
- 투사 `회오리 베기`: 살아 있는 적 전열 전체에만 일반 공격 80% 피해, 실제 적중한 전열 적 1명당 기세 +1(최대 3), 자체 쿨타임 없음. 전열이 비면 행동 미소비로 사용 불가
- 스킬 전용 광역 범위 `EnemyFrontRowAll`·`EnemyRearRowAll`·`EnemyAll`과 공용 해석기 구조. 회오리 베기는 전열, 화살비는 후열, 향후 썬더볼트는 적 전체 범위로 확정
- 회오리 베기는 `spinner` 버튼 아이콘과 투사 중심 약 0.3초 코드 기반 회전 참격을 사용하며, 동시 피해 숫자·피격 반응·HUD 갱신 후 다음 턴 진행
- 회오리 베기의 기세 획득은 공용 `AddMomentum`만 사용하고 난도 직접 사용 기록을 변경하지 않아 난도 쿨타임과 독립
- JobDefinition 프리뷰를 사용하는 재사용 가능한 전투 스킬 카탈로그·실행기·참가자별 쿨타임·상태효과 런타임
- 실제 스킬 메뉴와 Esc 복귀, 미구현 스킬 비활성 표시, 수호자 도발 제자리 강조 연출
- 수호자 도발의 적 전체 적용, 적별 다음 2회 행동 소모, 수호자 행동 기준 3턴 쿨타임과 적 HUD 상태 표시
- 참가자 목록 기반 `BattleEncounterSetup`과 3대3 프로토타입 Factory, 기존 2×3 Formation을 사용하는 실제 N대N 전투 생성
- 플레이어·태온(수호자)·미엘(치유사)의 플레이어 직접 조작과 독립 HP 미니 HUD
- 전열 슬라임 2명·후열 슬라임 1명의 독립 Combatant·턴·HP·도발 상태와 6명 전체 행동 타임라인
- 적·아군 후보를 공통 처리하는 대상 선택 UI 기반과 태온·미엘 코드 생성 임시 Visual
- 현재 HP·직업/몬스터 분류·방어·도발·구현 스킬 쿨타임을 계산 코드 변경 없이 조합하는 `BattleCombatantStatusViewModel`
- 방어·도발 표식을 안정적 ID와 수치로 분리해 향후 무료 아이콘 Asset으로 교체 가능한 표시 구조

## 데이터만 있고 실행 로직이 없는 항목

- 길 능력치와 고유 패시브의 전투 효과
- 직업 능력치, 패시브, 시작 스킬 프리뷰
- AP, 상태이상, 협동 기술, 보스 패턴의 초안 방향

이 항목들은 캐릭터 생성 UI에서 표시되지만 실제 전투 계산이나 효과로 실행되지 않는다.

## 문서화된 전투 설계

- 아군·적 공통 전열 3칸과 후열 3칸의 2×3 진형
- 근거리·원거리·마법 사거리와 바로 앞 전열의 후열 보호
- 단일 적대 행동에 일반 사거리보다 우선하는 도발 강제 대상
- 민첩·행동 우선도 기반 솔로 순서와 시간 제한 없는 직접 행동 지시
- 공용 공격·스킬·방어·도망 명령 및 승리·도망 필드 복귀
- 멀티플레이는 동시 입력, 기본 45초 제한, 남은 시간 10초 경고, 시간초과 시 방어로 설계되어 있으나 네트워크는 미구현
- AP, 상태이상, 행동·협동 기술, 보스 패턴은 미확정이며 구현하지 않음

## 미구현

- 투사 시작 스킬 3종을 제외한 나머지 직업별 스킬 효과와 길 패시브
- `BeastCompanion` 선택·장착·저장 UI와 Wolf/Bear/Fox 패시브 수치·능력치 계산
- NPC 동료 정식 CompanionDefinition·파티 편성·최종 Sprite
- 여러 몬스터 배치 전투와 보스전 실제 콘텐츠
- AP와 상태이상, 행동·협동 기술, 보스 패턴
- 멀티플레이 네트워크 전투
- 인벤토리·아이템·아이템 보상·파티 EXP 분배 정책
- 퀘스트와 영구 저장·불러오기
- 인스턴스 던전과 Field_02 이후 지역

## 현재 검증 상태

- MP 상세 팝업 이동·Tooltip 공통 문구 정리·광역 참조 중복 제거는 관련 파일 `git diff --check`와 Unity 6000.5.7f1 배치 실행 종료 코드 0을 확인했다. Presenter의 화살비·회오리 베기·썬더볼트·독액 분사 계산 콜백이 연출당 한 번인 구조와 Executor의 대상당 단일 피해를 정적으로 교차 확인했으며, 실제 1/2/3명 피해 숫자와 HUD 배치는 Play Mode 확인이 필요하다.
- 치유사 MaxMP 공식 변경은 관련 파일 `git diff --check`와 Unity 6000.5.7f1 배치 실행 종료 코드 0을 확인했다. 지능 12 기준 Lv1 44·Lv2 45·Lv3 46·Lv50 93, 마도사 0을 정적으로 교차 확인했으며 실제 MP 소비·행동 종료 회복·쿨타임·저장 슬롯별 재계산은 Play Mode 확인이 필요하다.
- 성장 능력치·치유사 MP·민첩 Tie Break 변경은 관련 파일 `git diff --check`를 통과했고 Unity 6000.5.7f1 배치 실행이 종료 코드 0으로 완료됐다. 실제 Play Mode의 Lv별 표시, MP HUD/회복/부족 거절, 동률 유무별 Overlay와 Timeline 일치는 사용자가 직접 확인해야 한다.
- Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조 응답과 Roslyn으로 새 전투 코드를 포함해 컴파일했으며 Compiler Error 0개를 확인했다.
- 치유사 기본 공격을 마법 사거리로 매핑하고 기존 검증용 공격력보다 4 낮게 적용한 뒤 동일한 Unity 참조로 Compiler Error 0개를 재확인했다.
- 임시 실행 테스트로 근거리 보호·후열 개방, 원거리 후열 우선, 마법 자유 대상, 도발 우선·2회 지속, 방어 50%, 행동 우선도·민첩 정렬의 12개 검증을 모두 통과했다.
- `Battle.unity` GUID와 Build Settings GUID 일치, Battle 항목 1개, 새 C# meta 3개와 `git diff --check` 통과를 확인했다.
- 사용자가 Unity Play Mode에서 `Field_01 → 초원 슬라임 → Battle` 진입, 기본 공격, 적 자동 공격, 턴 순환을 정상 검증했다.
- 사용자가 방어 시 받는 피해가 10에서 5로 감소해 50% 방어 규칙이 정상임을 확인했다.
- 사이드뷰 UI 변경 후 Unity 전체 `Assembly-CSharp` 참조로 Compiler Error 0개를 확인했다. 실제 사이드뷰 화면 배치와 입력 회귀는 사용자가 다시 확인해야 한다.
- 조작 안내를 명령 패널 내부로 옮긴 뒤 동일한 Unity 전체 참조로 Compiler Error 0개를 확인했다. 16:9에서의 하단 padding과 버튼 간격은 사용자가 직접 확인해야 한다.
- HP Fill의 실제 Rect 폭 갱신과 스폰 ID별 처치·30초 리스폰 구조를 적용한 뒤 Unity 전체 `Assembly-CSharp` 참조로 Compiler Error 0개를 확인했다.
- 사용자 Play Mode 확인에서 한 마리만 생성되는 문제를 재현했고, Unity Editor 로그에서 `grass_slime_02`~`05`의 meta YAML 마지막 줄바꿈 누락으로 GUID가 무효 처리되어 Asset import가 제외된 원인을 확인했다.
- 새 4개 meta를 정상 형식으로 수정하고, Installer의 실제 로드 개수·고유 ID 로그와 Field01SceneGenerator의 5개 ID·30초·최소 3유닛 간격 검증을 추가했다. Runtime/Editor C# 컴파일 오류 0개를 확인했으며 Play Mode 재검증이 필요하다.
- `BattleCore.cs`가 변경되지 않았으며 공격 10, 방어 50%, Formation·TargetResolver·TurnOrderQueue 계산 규칙을 유지했다. 실제 HP Bar 비율과 필드 리스폰 시간은 Play Mode에서 사용자가 확인해야 한다.
- 이전 사이드뷰 UI 작업에서는 `BattleCore.cs`와 조우/복귀 코드가 변경되지 않았음을 정적으로 확인했다. 이번 작업은 `BattleSceneFlow.cs`의 승리·도망 결과 전달만 확장했으며 전투 계산 규칙은 변경하지 않았다.

- 월드 전환 Scene/Spawn ID, Missing Script, Bounds 참조와 viewport 계산은 정적으로 확인했다.
- 초원 슬라임 Script GUID, Monster/Spawn Asset 연결, Field_01 대상 Scene과 조우 Event 구조를 정적으로 확인했다.
- 초원 슬라임 PNG를 1256×1256, 314×314 Cell의 4×4 구조로 확인하고 Sprite 16개, Animation Clip 8개, Animator와 데이터 참조를 정적으로 교차 확인했다.
- `MonsterNameplate`가 몬스터별 Text를 분리하고 `DisplayName`을 받으며 Camera 이동 뒤 화면 좌표를 갱신하는 구조를 정적으로 확인했다.
- Unity 6.5에서 오류가 된 `GetInstanceID()`를 권장 API인 `GetEntityId()`로 교체했다.
- Unity 6000.5.7f1이 사용하는 Roslyn과 전체 `Assembly-CSharp` 응답 파일로 재컴파일하여 Compiler Error 0개와 종료 코드 0을 확인했다.
- 최신 Unity Editor 로그에서 `MissingReferenceException`과 `NullReferenceException` 기록이 없음을 확인했다. 실제 Play Mode 기능 검증은 사용자가 직접 확인해야 한다.
- 관련 C# 변경은 `git diff --check`를 통과했다.
- 기본 공격 액션 연출 코드를 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조와 Roslyn으로 컴파일해 오류 0개를 확인했다. 기존 API deprecation 경고만 남아 있다.
- `BattleCore.cs`, `TargetResolver`, `Formation`, 피해·방어 계산은 변경하지 않았다. 근거리 연출을 유지하고 사수 원거리 기본 공격만 Projectile 연출에 연결했으며, 마법 기본 공격은 기존 즉시 처리 흐름을 유지했다.
- 사수 Projectile 코드를 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조와 Roslyn으로 컴파일해 오류 0개를 확인했다. 조준 시간·Projectile 위치와 방향·입력 잠금은 Play Mode 확인이 필요하다.
- 기본·Path Variant·휠체어·초원 슬라임 Animator Controller에서 `Idle_Left`·`Idle_Right` 상태를 확인하고, `BattleVisualResolver`를 포함한 전체 `Assembly-CSharp` 컴파일 오류 0개를 확인했다. 실제 방향과 첫 프레임은 Play Mode 확인이 필요하다.
- `AnimationClip.SampleAnimation`이 Sprite PPtr 곡선을 적용하지 못하던 경로를 임시 Animator 상태 평가로 교체하고, Path Variant 기본 Sprite fallback과 null UI 투명 처리를 추가했다. 전체 `Assembly-CSharp` 컴파일 오류 0개이며 흰 사각형·경고 제거는 Play Mode 확인이 필요하다.
- 마도사·치유사 기본 마법 Projectile과 런타임 Orb Graphic을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 피해·치유사 감소값·마법 사거리 계산은 변경하지 않았으며 색상·도착 시점·Idle 복구는 Play Mode 확인이 필요하다.
- golden arrow 8프레임(80ms), fireball 7프레임(60ms)을 원본 32×32 크기로 추출하고 복사 전후 SHA-256 일치를 확인했다. 실제 에셋 연결 후 전체 Assembly-CSharp 컴파일 오류 0개이며 표시 크기·방향·프레임 재생은 Play Mode 확인이 필요하다.
- 사수 golden arrow와 마도사 fireball 이동시간을 각각 0.35초로 조정하고, 프레임 간격·도착 후 피해 적용·치유사 Projectile 0.24초·근거리 속도는 유지했다. 전체 Assembly-CSharp 컴파일 오류 0개이며 체감 속도는 Play Mode 확인이 필요하다.
- 치유사 임시 Orb를 PVFX Magical Projectile의 96×96 travel 프레임 5개로 교체했다. manifest 픽셀 해시 일치, Point Filter·투명 Sprite·50ms 프레임·0.24초 이동 유지와 전체 Assembly-CSharp 컴파일 오류 0개를 확인했다.
- 수호자 도발 스킬 구조를 Unity 전체 Assembly-CSharp 참조로 컴파일해 오류 0개를 확인했다. Combatant·TargetResolver 기존 도발 우선 판정을 재사용하고 BattleCore·Formation·TurnOrderQueue는 수정하지 않았다. 메뉴 조작·HUD 배치·2회 소모·3턴 쿨타임은 Play Mode 확인이 필요하다.

- 3대3 Encounter와 공용 대상 선택 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. `BattleCore`·기존 스킬 런타임·ActionPresenter·BattleVisualResolver는 변경하지 않았으며 실제 3대3 UI·입력·도발 분산은 Play Mode 확인이 필요하다.
- 전투 상태 UI와 ViewModel을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 전투 계산·N대N·대상 판정·연출 코드는 변경하지 않았으며 16:9 팝업 위치와 마우스/키보드 동작은 Play Mode 확인이 필요하다.
- 고정 HP 목록 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. `BattleCore`·Formation·TargetResolver·TurnOrderQueue·도발·피해 계산과 하단 명령 패널은 변경하지 않았으며, 좌우 목록 배치·실제 HP 비율·포커스 행 연동은 Play Mode 확인이 필요하다.
- 상단 HP HUD 재배치를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. HP HUD·전장·상태 Anchor·팝업 안전 영역만 변경했으며 N대N·Formation·Combatant·대상/턴/도발/피해 계산·명령·연출 코드는 변경하지 않았다.
- 대상 화살표 정렬·HP HUD 상태 요약·명령 버튼 축소·공용 취소 흐름을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. `BattleSceneController`의 UI와 입력 단계만 변경했으며 전투 계산·N대N·도발/방어 판정·행동 실행은 변경하지 않았다.
- 전투불능 상태 정리·회색 발판 숨김·단색 버튼/상태 아이콘·죽은 대상 선택 정리를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. ViewModel과 UI 갱신만 변경했으며 Combatant·Formation·TargetResolver·TurnOrderQueue와 피해/도발/방어 계산은 변경하지 않았다.
- Kenney 실제 Sprite 카탈로그·명령 버튼·HP HUD 상태 배지 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. 기존 deprecated API 경고 4개만 유지되며 `BattleCore`·Combatant 계산·Formation·TargetResolver·TurnOrderQueue·Projectile/VFX·취소 흐름은 변경하지 않았다. 실제 import와 화면 정렬은 Play Mode 확인이 필요하다.
- 도발 pawn_right와 재사용 3단계 모래시계 표시를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. `BattleSkillCooldowns`의 시작·감소 계산은 변경하지 않고 ViewModel이 남은 턴과 총 턴을 UI에 전달하며, 전투불능 목록 제거와 텍스트 fallback을 유지한다.
- 치유의 빛 회복 API·아군 대상 선택·Radiant Heal Presenter 확장을 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 원본 grid sheet SHA-256 일치와 96×96 프레임 14개를 확인했으며, `TakeDamage`·방어 50%·TargetResolver·Formation·TurnOrderQueue·도발·기존 Projectile/VFX는 변경하지 않았다. 실제 회복 시점과 화면 위치는 Play Mode 확인이 필요하다.
- 사수 정조준 데이터·원거리 대상 연결·golden_arrow 도착 피해를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 정수 퍼센트식은 Attack 12→20, 15→24, 20→32를 확인했으며 TargetResolver·Formation·BattleCore·Presenter·기본 공격과 기존 Projectile 에셋은 변경하지 않았다. 실제 후열/전열 선택과 2→1→사용 가능 흐름은 Play Mode 확인이 필요하다.
- 투사 난도의 근거리 대상 선택·150% 타격·적중 후 자원 증가·직접 사용 3회 쿨타임·HUD 표시를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 공격력 12→18, 15→23, 20→30의 정수 올림 계산을 확인했고 기존 deprecated API 경고 4개만 있었다. Formation·TurnOrderQueue·BattleCore·Projectile/VFX와 기존 세 직업 스킬은 변경하지 않았으며 이후 사용자 Play Mode 확인을 통과했다.
- 사용자가 Unity Play Mode에서 기존 난도 150% 근거리 공격·기세 획득 전 동작이 정상임을 확인했다.
- 기세 용어·API 리네임과 회심의 일격 데이터·근거리 타격·기세 소비·skull UI를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 공격력 10 기준 기세 0/1/2/3 피해 10/13/16/19와 원본 ZIP·런타임 skull PNG SHA-256 일치를 확인했고 기존 deprecated API 경고 4개만 있었다. 실제 연출·HUD·쿨타임 독립은 Play Mode 확인이 필요하다.
- 회오리 베기 데이터·광역 실행·동시 피격 Presenter·spinner 아이콘 연결을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 기존 deprecated API 경고 4개만 있었고 Formation·TargetResolver·TurnOrderQueue·BattleCore·기존 Projectile/VFX는 변경하지 않았다. 실제 1~3명 대상 피해·기세와 연출은 Play Mode 확인이 필요하다.
- 회오리 베기 대상을 `EnemyFrontRowAll` 데이터와 공용 스킬 대상 해석기로 전열에 제한한 뒤 Unity 전체 `Assembly-CSharp` 참조 컴파일 오류 0개를 확인했다. Formation·TargetResolver·기존 VFX는 변경하지 않았으며, 후열 제외·전열 0명 행동 미소비는 Play Mode 확인이 필요하다.
- 화살비 `EnemyRearRowAll` 대상·120% 광역 실행·다중 golden_arrow Presenter·bow 아이콘을 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. Kenney 원본과 bow 복사본 해시 및 golden_arrow 8프레임 유지를 확인했고, 기존 deprecated API 경고 4개만 있었다. 실제 타이밍·후열 0명·대상당 단일 피해는 Play Mode 확인이 필요하다.
- 화살비 다중 후열 실검증용 프로토타입 적 배치를 전열 1명+후열 2명으로 바꾼 뒤 Unity 전체 `Assembly-CSharp` 참조 컴파일 오류 0개를 확인했다. 전투 계산·범위·연출 코드는 변경하지 않았으며 실제 동시 타격과 후열 전투불능 제외는 Play Mode 확인이 필요하다.
- 사용자가 Unity Play Mode에서 화살비가 후열 여러 명을 정상 공격하는 것을 확인했다.
- 화살비 성공 완료 후 2턴 쿨타임 등록과 설명 데이터를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 120%·EnemyRearRowAll·golden_arrow 연출은 변경하지 않았고 기존 deprecated API 경고 4개만 있었다. 2→1→사용 가능과 정조준 독립 표시는 Play Mode 확인이 필요하다.
- **동료의 습격 구현**: `BeastCompanionDefinition`·카탈로그, 자유 단일 대상+도발 우선, 180% `TakeDamage`, 3턴 쿨타임과 Wolf Run+이동+도착 타격 구조를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 기존 deprecated API 경고 4개만 있으며 실제 화면 위치·속도·입력 잠금·쿨타임 흐름은 Play Mode 확인이 필요하다.
- **동료의 습격 Wolf 연출 조정**: 선명한 Wolf Run index 4 실물 프레임 아이콘, 사수 근처 출발, 최초 이동 속도의 3/4, 대상 바로 앞 정지·도착 타격·0.12초 여운 뒤 제거를 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 원본 SpriteSheet는 수정하지 않고 야수 정의가 경로·아이콘 프레임을 제공하며, 대상·180% 피해·도발·3턴 쿨타임 코드는 변경하지 않았다. 실제 아이콘 가독성·간격·체감 속도는 Play Mode 확인이 필요하다.
- **동료의 습격 Run 프레임 수정**: 원본 Wolf Run의 64×40 프레임 6개가 모두 서로 다른 이미지임을 확인하고, 전투 `Image.sprite`를 0→1→2→3→4→5→0 순서로 12 FPS 교체하는 Coroutine을 570 UI 단위/초 Transform 이동 Coroutine과 분리했다. 출발·도착점은 매 사용 시 현재 사수·대상 `ActionRoot` 위치와 표시 크기로 계산하며, 아이콘용 index 4 고정 Sprite와 전투용 6프레임 배열은 분리했다. 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 실제 다리 움직임과 접촉 위치는 Play Mode 재확인이 필요하다.
- **마도사 파이어 볼 구현**: Magic 자유 단일 대상·도발 우선, 170% 즉발 피해, 명중 당시 Attack 30%를 저장한 행동 종료 화상 2회와 비중첩 2회 갱신, 마도사 행동 기준 3턴 쿨타임을 공용 스킬·상태·쿨타임 구조에 연결했다. PVFX Charge·Warm Explosion과 큰 기존 fireball, 작은 화상 틱을 새 시각 정의 경계로 연결했으며 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했다. 실제 크기·타격 프레임·행동 종료 틱은 Play Mode 확인이 필요하다.
- **마도사 썬더볼트 구현**: EnemyAll 생존 적 전체 90% 동시 피해, 대상별 감전 1의 15% 주는 피해 감소·행동 종료 제거·비중첩 갱신, 마도사 행동 기준 3턴 쿨타임을 기존 런타임에 연결했다. electric-impact 14프레임과 peak index 1 동시 타격, VFX/상태 고정 아이콘 분리를 적용했고 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했다. 실제 다중 대상 크기·동시성·상태 수명은 Play Mode 확인이 필요하다.
- **마도사 가이아 웰 방어 규칙 수정**: 자신 전용 받는 피해 감소를 60%로 상향하고 공용 방어 50%와 중첩되지 않게 입력·피해 계산 양쪽에서 차단했다. 활성 중 방어 버튼은 사용 불가 색상으로 보이지만 클릭·키보드 시 지정 안내를 제공하고 행동을 소비하지 않는다. 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 실제 버튼 상태·피해 10→4는 Play Mode 확인이 필요하다.
- **수호자 철벽 구현**: 참가자별 철벽 상태·70% 받는 피해 감소·자신의 다음 2회 행동 지속·4턴 쿨타임과 공용 방어 중첩 차단을 기존 상태·쿨타임 구조에 연결했다. Earth Rupture 20프레임과 `structure_wall.png` 연결 후 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 실제 크기·피해 10→3·HUD·입력 흐름은 Play Mode 확인이 필요하다.
- **수호자 수호의 맹세 구현**: 직접 행동 피해 분류, 수호자별 최대 HP 40% 이전 예정 예산, 예산 비례 감소, 철벽 적용 후 실제 이전 피해, 다음 수호자 행동 시작·전투불능·예산 소진 종료를 공용 상태 경계에 연결했다. Frost Nova 중간 7프레임·금색 이전선과 실제 `pawns.png`, 대상별 감소량 피드백을 연결했으며 실제 VFX 범위·홀수 피해·예산 소진·복합 방어는 Play Mode 확인이 필요하다.
- **공통 스킬 설명 팝업 흐름 조정**: 스킬 확정 시 정보 UI를 닫는 공통 경계와 대상 선택·행동 완료 안전 숨김을 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 스킬별 설명 내용·효과·대상·쿨타임은 변경하지 않았으며 실제 Hover/키보드 포커스와 취소 복귀 흐름은 Play Mode 확인이 필요하다.
- **전투 연출 중 정보 팝업 억제**: `actionPlaying` 화면 갱신에서 스킬 설명·캐릭터 상태 상세 팝업과 기존 Hover/포커스 대상을 함께 정리하고 두 표시 함수의 재오픈을 차단하는 코드를 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. Wolf 570·Run 12 FPS·출발/정지 위치와 모든 전투 계산은 변경하지 않았으며 실제 마우스 잔류·새 Hover 복귀는 Play Mode 확인이 필요하다.
- **치유사 회복의 파동 구현**: 단일힐 35%에서 파생한 대상별 21% 올림 광역 회복, Formation 생존 아군 동적 목록, 치유사 행동 기준 3턴 쿨타임과 Arcane Parry+Radiant Heal 병렬 연출을 연결했다. 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 대상 수·동시 HP 갱신·팝업 억제·쿨타임은 Play Mode 확인이 필요하다.
- **치유사 정화 구현**: 기존 독·화상 Dictionary와 감전 HashSet은 유지하고 `HarmfulStatusType`·공통 조회/단일 제거/전체 제거 API를 추가했다. 살아 있는 아군 1명의 세 해로운 상태를 모두 제거하며 상태가 없으면 행동·턴·쿨타임을 소비하지 않는다. spectral-bloom 16프레임·release index 7 제거·peak index 5 아이콘과 치유사 행동 기준 2턴 쿨타임을 연결했다. Unity 6000.5.7f1 자동 컴파일 오류 0개를 확인했으며 실제 대상 선택·복합 상태 제거·VFX·팝업 억제·쿨타임은 Play Mode 확인이 필요하다.
- **독침벌 독 부여 대기시간 구현**: `BattleMonsterAbilityRuntime`이 공격자 Combatant별 독침 대기 상태를 독 상태 저장소와 분리해 관리한다. 첫 공격은 120% 피해+독 3 뒤 대기 2, 다음 두 벌 행동은 120% 피해만 주고 행동 종료마다 1·0으로 감소하며 네 번째 행동부터 다시 독을 부여한다. 정화·독 자연 종료·다른 참가자 행동은 대기시간에 영향을 주지 않는다. Unity 6000.5.7f1 자동 컴파일 오류 0개를 확인했으며 4행동 순서와 복수 벌 독립성은 Play Mode 확인이 필요하다.
- **독 몬스터 후보 에셋 보존 및 Pilot Bee 검증 준비**: F:\Downloads 원본과 프로젝트 복사본 SHA-256 일치를 확인하고 벌·거미·뱀을 제작자별 ThirdParty 폴더에 분리했다. Pilot Bee 검증 스크립트를 포함한 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개), Point·무압축·투명 Import 설정을 정적으로 확인했다. Unity 프로젝트가 이미 열려 있어 두 번째 배치 인스턴스는 안전하게 중단했으며 실제 화면의 Scale·픽셀 밀도·Idle/Attack 자연스러움은 전용 Scene Play Mode 확인이 필요하다.
- **동료의 습격 Run 검증 환경 유지**: ScratchIO `Animated Wild Animals` CC0 원본 ZIP과 Wolf/Bear/Fox 자산을 보존하고, 실제 Battle과 분리된 `CompanionAssaultRunValidation` Scene을 유지한다. 원본/복사본 SHA-256, 64px 프레임 구조, Point·무압축·투명 Import와 통일된 아래 중앙 Pivot은 정적으로 확인했으며 Unity Play Mode 직접 확인은 남아 있다.
- Working Tree에는 이번 문서 작업과 무관한 사용자 Asset·Scene·ProjectSettings 변경이 남아 있으며 이 상태 문서는 해당 미커밋 변경의 완성 여부를 판단하지 않는다.

## Unity에서 사용자가 직접 확인할 사항

1. Play Mode를 종료해 Asset 재import를 완료한 뒤 다시 시작하고, Console에 `필드 몬스터 배치 로드: Field_01, 5개 [grass_slime_01, ..., grass_slime_05]`가 출력되는지 확인한다.
2. HP가 100%에서 50%가 되었을 때 숫자와 함께 HP Bar 길이도 정확히 절반이 되는지 확인한다.
3. Player와 초원 슬라임 양쪽 HP Bar가 피해 직후 정상 감소하고, HP 0에서 완전히 비는지 확인한다.
4. 슬라임 처치 후 `Field_01` 복귀 시 조우했던 해당 슬라임만 사라져 있는지 확인한다.
5. 처치하지 않은 다른 슬라임들은 그대로 존재하며 계속 배회하는지 확인한다.
6. 약 30초 후 처치한 스폰의 시작 위치에서 슬라임이 다시 나타나는지 확인한다.
7. 도망한 경우 조우했던 슬라임이 사라지지 않고, 기존 2초 재조우 유예 뒤 정상 배회하는지 확인한다.
8. `Field_01`에 총 5마리의 초원 슬라임이 서로 다른 활동 반경 안에서 독립적으로 배회하는지 확인한다.
9. Console에 Compiler Error, MissingReferenceException, NullReferenceException이 없는지 확인한다.
10. 근거리 직업으로 플레이어 기본 공격 시 대상 앞까지 짧게 전진하고, 타격 후 원래 자리로 복귀하는지 확인한다.
11. 초원 슬라임 공격에도 같은 전진·복귀가 적용되고, 양측 피격 시 좌우 흔들림·짧은 점멸과 `-피해량` 숫자가 표시되는지 확인한다.
12. 연출 중 공격·스킬·방어·도망·대상 선택·Esc 취소가 중복 실행되지 않고, 연출 종료 뒤 다음 턴 입력이 정상 복구되는지 확인한다.
13. 공격 10과 방어 중 피해 5, HP 숫자·Bar, 행동 순서 타임라인, 승리·도망·패배 및 Field_01 복귀·리스폰이 기존과 동일한지 확인한다.
14. 사수 직업으로 기본 공격 시 제자리에서 짧게 조준하고 golden arrow 애니메이션이 약 0.35초 동안 왼쪽 적을 향해 이동하는지 확인한다.
15. Projectile 도착 순간에만 HP와 피해 숫자가 갱신되고 기존 흔들림·점멸 뒤 다음 턴으로 정상 진행되는지 확인한다.
16. Projectile 이동과 피격 연출 중 명령·대상 선택·Esc가 중복 실행되지 않는지 확인한다.
17. 필드에서 플레이어가 걷거나 상·하·우 방향을 보는 중 조우해도 Battle에서는 기본·Path Variant·휠체어 모두 Left Idle 첫 프레임인지 확인한다.
18. 초원 슬라임이 필드에서 이동하거나 다른 방향을 보는 중 조우해도 Battle에서는 Right Idle 첫 프레임인지 확인한다.
19. 근거리·Projectile 공격 종료 후 양측이 각자의 Left/Right 전투 Idle Sprite로 복귀하는지 확인한다.
20. 남자 지체의 길 수호자와 초원 슬라임이 흰 사각형 없이 표시되고, `Battle Visual` Warning·NullReference·Compile Error가 없는지 확인한다.
21. 마도사 기본 공격 시 제자리 캐스팅 후 fireball 애니메이션이 약 0.35초 동안 왼쪽의 전열 또는 후열 대상까지 이동하고, 도착 순간에만 HP·피해 숫자가 갱신되는지 확인한다.
22. 치유사 기본 공격 시 PVFX Magical Projectile이 0.24초 동안 왼쪽 대상을 향해 재생되고, 도착 순간 피해와 기존 낮은 피해량·마법 자유 대상 규칙이 유지되는지 확인한다.
23. 두 마법 연출 중 입력이 잠기고 완료 후 Left Idle과 다음 턴이 복구되며, 치유사 Magical Projectile과 근거리 연출도 정상인지 확인한다.
24. 테스트용으로 시작 X보다 목표 X가 큰 배치를 구성할 수 있을 때 golden arrow와 fireball이 원본 오른쪽 방향으로 표시되는지 확인한다.
25. 수호자로 Battle에 진입해 스킬 → 도발을 마우스와 키보드로 선택하고, Esc로 명령 메뉴에 복귀되는지 확인한다.
26. 도발 사용 시 수호자가 제자리에서 강조되고 도발! 텍스트 뒤 슬라임 HUD에 도발 2가 표시되는지 확인한다.
27. 슬라임 첫 행동 완료 후 도발 1, 두 번째 행동 완료 후 표시 제거를 확인한다.
28. 수호자 다음 행동 차례의 메뉴에 도발 [재사용 2턴], 이후 1턴, 종료 후 다시 사용 가능 상태가 표시되는지 확인한다.
29. 쿨타임 중 도발 버튼이 실행되지 않으며 철벽·수호의 맹세와 다른 직업 스킬 버튼의 구현 상태·재사용 표시가 실제 규칙과 일치하는지 확인한다.
30. 공격·방어·도망과 기존 근거리/Projectile 연출, HP Bar, 승리·패배·Field_01 복귀·30초 리스폰을 회귀 확인한다.
31. Console에 Compile Error, NullReferenceException, MissingReferenceException이 없는지 확인한다.

32. Battle 진입 시 플레이어·태온·미엘과 초원 슬라임 A·B·C가 3대3으로 겹치지 않고 표시되며 각 HUD에 이름·직업·HP가 보이는지 확인한다.
33. 타임라인에 여섯 참가자의 민첩 기반 순서가 표시되고 플레이어·태온·미엘 차례마다 시간제한 없이 직접 명령할 수 있는지 확인한다.
34. 전열 슬라임 A·B와 후열 슬라임 C에 근거리·원거리·마법 기본 공격 대상 규칙이 기존대로 적용되는지 확인한다.
35. 슬라임 하나의 HP가 0이 되어도 다른 두 슬라임의 HP·행동이 유지되고, 세 마리 전멸 때만 승리하는지 확인한다.
36. 태온 또는 플레이어 수호자의 도발 후 세 슬라임 HUD가 모두 도발 2가 되고, 각 슬라임 행동 때 자기 표시만 1로 감소한 뒤 두 번째 행동 후 사라지는지 확인한다.
37. 도발 중 세 슬라임의 단일 공격이 시전자 수호자에게 집중되고, 시전자 행동 기준 재사용 2턴→1턴→사용 가능인지 확인한다.
38. 아군 한 명 전투불능 시 그 참가자만 행동에서 제외되고 나머지 전투가 계속되며, 아군 세 명 전멸 때만 패배하는지 확인한다.
39. Console에 Compile Error, NullReferenceException, MissingReferenceException이 없는지 확인한다.

40. 16:9 Battle이 제목 → 타임라인 → 상단 HP HUD → 실제 전장 → 하단 명령 패널 순서로 표시되고 각 영역이 겹치거나 잘리지 않는지 확인한다.
41. 현재 3대3에서는 상단 HUD의 적군 첫 줄과 아군 한 줄에 각각 3개 HP 항목만 표시되고, 캐릭터 주변 이름·HP Bar가 없는지 확인한다.
42. 참가자 수를 줄인 Encounter에서는 빈 HP 항목이 생기지 않고, 적 4~6명 구성에서는 적군이 한 줄 최대 3명·최대 2줄로 배치되는지 확인한다.
43. 피해 직후 해당 HP 항목의 Bar 길이가 현재/최대 HP 실제 비율만큼 줄고, HP 0에서 완전히 비며 이름에 `[전투불능]`이 표시되는지 확인한다.
44. 대상 선택을 방향키로 전환할 때 역삼각형이 적/아군 전열·후열 각 Sprite의 가로 중앙 위로 정확히 이동하는지 확인한다.
45. 캐릭터 주변에는 대상 화살표와 `행동 중`만 보이고, 상단 HP 항목에 실제 Sprite와 함께 `행동 중`, `방어`, `도발 2/1`, 구현 스킬 `재사용 n턴`이 한 줄로 표시되는지 확인한다.
46. 참가자가 전투불능이 되는 즉시 화살표·행동 중·도발·방어·재사용 표시가 사라지고 HP HUD에는 회색 `전투불능`만 남는지 확인한다.
47. 전투불능 참가자 상세 팝업에는 이름·분류·HP 0/최대 HP만 표시되고 방어·도발·쿨타임이 남지 않는지 확인한다.
48. 평상시 모든 캐릭터 아래 회색 발판이 보이지 않고, 대상 선택 중 유효 대상에게만 얇은 금색 선이 표시되는지 확인한다.
49. 공격=sword, 스킬=star, 방어=shield, 도망=exitRight, 취소=cross Sprite와 한글이 함께 보이며 비율·정렬이 깨지거나 버튼 크기가 커지지 않았는지 확인한다.
50. 대상 선택 중 포커스된 참가자가 전투불능이 되면 강조가 제거되고 다음 유효 대상으로 이동하거나 후보가 없을 때 기본 명령으로 복귀하는지 확인한다.
51. 대상/스킬 선택의 취소 버튼·Esc와 공격·방어·도발 수치, Projectile·승패·필드 복귀가 기존과 같고 Console 오류가 없는지 확인한다.
52. 행동 중 arrowRight, 방어 shield와 재사용 단계별 hourglass가 상태와 함께 갱신되는지 확인한다.
53. 전투불능 즉시 모든 상태 Sprite와 글자가 사라지고 회색 `전투불능`만 남으며 Console에 MissingReference·NullReference가 없는지 확인한다.
54. 도발 상태에 pawn_right가 표시되고 기존 target은 나오지 않으며, 도발 2→1→제거가 유지되는지 확인한다.
55. 도발 사용 직후 재사용 3턴=hourglass_top, 다음 자기 차례 2턴=hourglass, 마지막 1턴=hourglass_bottom, 0턴=아이콘·텍스트 제거인지 확인한다.
56. 미엘 또는 플레이어 치유사 차례에 스킬→치유의 빛이 활성화되고 자신·플레이어·태온·미엘 중 살아 있는 아군만 선택되는지 확인한다.
57. 피해를 받은 아군에게 사용하면 최대 HP의 35% 올림 값만큼 회복하되 최대 HP를 넘지 않고, peak 순간 HP Bar·상세 HP와 `+회복량`이 함께 갱신되는지 확인한다.
58. 최대 HP 아군 선택 시 `이미 HP가 가득 찼습니다` 안내 후 스킬 메뉴로 돌아가며 행동이 소비되지 않는지 확인한다.
59. 전투불능 아군과 적은 대상이 아니며, 대상 선택 중 Esc/취소는 스킬 메뉴로, 스킬 메뉴 Esc/취소는 기본 명령으로 돌아가는지 확인한다.
60. Radiant Heal이 대상 위치에서 14프레임으로 재생되고 peak 뒤 연출 종료 시 다음 턴으로 진행하며, 연출 중 중복 입력과 Console 오류가 없는지 확인한다.
61. 플레이어 사수 차례에 스킬→정조준이 활성화되고 `강한 원거리 · 160% · 2턴` 안내가 보이는지 확인한다.
62. 후열 슬라임이 살아 있으면 후열만 선택되고, 후열 전멸 뒤 전열만 선택되며 전투불능 적은 후보에서 빠지는지 확인한다.
63. 정조준 선택 후 즉시 HP가 줄지 않고 `정조준!`→golden_arrow 0.42초 이동→도착 순간에만 피해·HP Bar·상세 HP·피해 숫자·피격 연출이 갱신되는지 확인한다.
64. 기본 공격력 12 기준 정조준 raw 피해가 20이며 방어 중 대상에는 기존 50% 감소가 적용되고 방어 무시·치명타·상태이상이 없는지 확인한다.
65. 사용 직후 HUD에 재사용 2턴/hourglass_top, 다음 사수 행동에 1턴/hourglass_bottom, 그다음 사용 가능 및 표시 제거인지 확인한다.
66. 정조준 대상 선택 중 Esc/취소는 스킬 메뉴, 스킬 메뉴 Esc/취소는 기본 명령으로 돌아가며 연출 중 입력이 잠기는지 확인한다.
67. 수호자·치유사·사수 스킬 메뉴에서 각각 pawn_left·suit_hearts·target 아이콘과 한글 스킬명이 함께 보이고, 적 도발 HUD에는 기존 pawn_right가 유지되는지 확인한다.
68. 스킬 버튼 Hover와 방향키 포커스에서 하나의 상세 팝업이 즉시 갱신되고, 설명·대상·효과 수치·재사용·미구현 여부가 현재 규칙과 일치하는지 확인한다.
69. 스킬 메뉴를 닫거나 대상 선택으로 이동하면 팝업이 사라지고, 16:9에서 HP HUD·하단 명령 패널과 겹치거나 화면 밖으로 잘리지 않는지 확인한다.
70. 취소 버튼에 arrowLeft와 `취소`가 함께 표시되고 cross가 나오지 않으며, 취소 버튼과 Esc의 단계별 복귀가 기존과 같은지 확인한다.
71. 투사 스킬 메뉴에 cross 아이콘+난도, skull 아이콘+회심의 일격이 함께 표시되는지 확인한다.
72. 난도 1회 적중 시 기존 150% 피해와 skull 아이콘+`기세 1`이 표시되는지 확인한다.
73. 기세 1에서 회심의 일격이 130% 피해를 주고 타격 후 기세 0이 되는지 확인한다.
74. 난도 2회 후 기세 2가 되고 회심의 일격이 160% 피해를 준 뒤 기세를 0으로 소비하는지 확인한다.
75. 난도를 직접 세 번째 사용할 때 기세 3과 난도 재사용 2턴이 표시되는지 확인한다.
76. 기세 3 회심의 일격이 190% 피해를 주고 기세만 0으로 만들며 난도의 남은 쿨타임은 유지하는지 확인한다.
77. 기세 0에서도 회심의 일격을 사용할 수 있고 일반 공격 100% 피해를 주는지 확인한다.
78. 상단 HUD는 기세 1/2/3만 표시하고 0에서 숨기며, 상세 팝업은 기세 0/3~3/3을 계속 표시하는지 확인한다.
79. 회심의 일격 Hover·키보드 포커스 설명에 100/130/160/190%, 전부 소비, 재사용 없음과 현재 기세·예상 배율이 표시되는지 확인한다.
80. 난도 설명이 적중 후 기세 +1·기세 최대 3·세 번째 직접 사용 후 2턴으로 통일됐는지 확인한다.
81. 회심의 일격 대상 선택 중 Esc/취소가 스킬 메뉴로 복귀하고 선택만으로 피해나 기세 소비가 발생하지 않는지 확인한다.
82. 방어 중 적에게 회심의 일격을 사용하면 기존 50% 피해 감소가 적용되는지 확인한다.
83. 투사 전투불능 시 기세·쿨타임 표시가 숨겨지고, 기존 도발·치유의 빛·정조준·기본 공격·승패·도망·Field 복귀와 Console이 정상인지 확인한다.
84. 투사 스킬 메뉴에 spinner 아이콘+회오리 베기가 표시되고 Hover·키보드 포커스 설명에 적 전열 전체·80%·적중당 기세 +1·최대 3·재사용 없음이 보이는지 확인한다.
85. 살아 있는 전열 적이 1/2/3명일 때 회오리 베기가 전열에만 각각 80% 정수 올림 피해를 동시에 적용하고 기세가 각각 +1/+2/+3 되는지 확인한다.
86. 기세 2에서 적 2명 이상을 맞혀도 기세가 3을 넘지 않고, 기세 3에서도 회오리 베기를 계속 사용해 피해는 주되 기세는 3으로 유지되는지 확인한다.
87. 회오리 베기로 기세 3을 만든 뒤 난도에 2턴 쿨타임이 생기지 않고, 회심의 일격이 190% 피해 후 기세를 0으로 소비하는지 확인한다.
88. `회오리 베기!` 뒤 투사 중심 회전 참격→모든 대상 피해 숫자·피격 반응→기세 HUD 갱신→다음 턴 순서와 연출 중 입력 잠금이 정상인지 확인한다.
89. 전열 2명·후열 1명에서 전열만 피해를 받고 기세 +2인지, 전열 1명·후열 2명에서는 기세 +1인지 확인한다.
90. 전열이 전멸하고 후열만 살아 있을 때 `회오리 베기로 공격할 전열 적이 없습니다.` 안내 후 행동과 턴이 소비되지 않는지 확인한다.
91. 사수 스킬 메뉴에 target+정조준과 bow+화살비가 함께 표시되고, 화살비 설명에 원거리 물리·적 후열 전체·120%·재사용 없음이 보이는지 확인한다.
92. 후열 생존자가 1/2/3명일 때 후열만 각각 120% 피해를 거의 동시에 한 번씩 받고 전열 HP는 변하지 않는지 확인한다.
93. 대상마다 golden_arrow 3발이 떨어져도 피해 숫자와 실제 HP 감소는 대상당 한 번이며, 방어 중 후열은 기존 50% 감소가 적용되는지 확인한다.
94. 후열이 전멸하고 전열만 남았을 때 `화살비로 공격할 후열 적이 없습니다.` 안내 후 Projectile·행동·턴이 시작되지 않는지 확인한다.
95. 도발 상태와 관계없이 후열 전체가 유지되고, `화살비!`→상승→공중 대기→다중 낙하→동시 피격의 약 0.75초 순서와 연출 중 입력 잠금·완료 후 단일 턴 진행이 정상인지 확인한다.
96. 화살비 설명에 재사용 2턴이 보이고, 성공 연출 완료 후 화살비 재사용 2턴/hourglass_top이 표시되는지 확인한다.
97. 다른 아군·적 행동에는 2턴이 유지되고 다음 사수 행동에 1턴/hourglass_bottom, 그다음 사수 행동에 0으로 사라져 다시 사용 가능한지 확인한다.
98. 화살비 쿨타임 중 정조준이 사용 가능하면 정상 사용되고, 정조준 사용·쿨타임이 화살비 남은 턴을 변경하지 않는지 확인한다.
99. 후열 0명에서 화살비 거절 후 쿨타임 표시가 생기지 않고 행동이 그대로 유지되는지 확인한다.
100. `Assets/_Project/Scenes/Validation/CompanionAssaultRunValidation.unity`를 열고 Play 시 Wolf/Fox/Bear가 같은 속도로 오른쪽→왼쪽을 반복하며 픽셀 흐림이나 발 기준점의 불필요한 흔들림 없이 보이는지 확인한다.
101. `Space` 일시정지, `+/-` Run FPS 조절, `F` 좌우 Flip이 동작하고 Wolf/Fox/Bear의 속도감·타격감·현재 Battle Sprite 표시 크기와의 조화를 직접 비교한다.
102. 검증 Scene 실행 뒤 기존 `Battle.unity`의 사수 기본 공격·정조준·화살비와 다른 직업 스킬, Formation·대상·턴·승패/도망/Field 복귀에 회귀가 없는지 확인한다.
103. 사수 스킬 메뉴에서 Wolf 실제 Sprite 아이콘+동료의 습격이 표시되고, 정조준은 기존 `target.png`를 유지하며 설명의 야수/단일 물리·적 1명·전후열 자유·180%·3턴이 보이는지 확인한다.
104. 전열과 후열 생존 적을 모두 선택할 수 있고, 사수에게 도발 강제 대상이 있으면 그 적만 선택되는지 확인한다.
105. 사수는 제자리에 있고 Wolf가 현재 행동 중인 사수의 실제 `ActionRoot` 위치 근처에서 나타나는지 확인한다. 전열/후열 등 사수 표시 위치를 바꿔도 고정 화면 좌표가 아니라 새 사수 위치를 따라 출발하며, 6개의 서로 다른 다리 자세가 0→1→2→3→4→5→0 순서로 12 FPS 반복되는 동안 Transform은 독립적으로 570 UI 단위/초로 대상 바로 앞까지 이동해야 한다.
106. Wolf가 대상을 관통하지 않고 바로 앞에 정지하며, 도착 전 HP가 줄지 않고 도착 순간 한 번만 180% 피해·피해 숫자·피격 반응이 발생하고 방어 중 대상에는 기존 50% 감소가 적용되는지 확인한다.
107. Wolf가 타격 위치에서 짧게 멈춘 뒤 제거되고 피격·제거가 모두 끝날 때까지 입력이 잠긴 뒤 다음 턴이 한 번만 진행되는지 확인한다.
108. 사용 직후 재사용 3턴/hourglass_top, 다음 사수 행동마다 2/hourglass→1/hourglass_bottom→0/사용 가능 순서이며 정조준·화살비 쿨타임과 독립인지 확인한다.
109. 대상 선택 중 Esc/취소가 스킬 메뉴로 복귀하고, 기존 도발·치유의 빛·투사 스킬·Formation·턴 순서·HP HUD·승리/도망/Field 복귀에 회귀와 Console 오류가 없는지 확인한다.
110. 모든 스킬 메뉴에서 Hover/키보드 포커스 중 설명이 보이고, 스킬 선택 즉시 대상 선택 또는 연출 전에 사라지며 연출 종료·기본 명령 복귀 뒤에도 숨겨졌다가 다음 스킬 메뉴의 새 Hover/포커스에서만 다시 표시되는지 확인한다.
111. 캐릭터/상태 상세 팝업을 연 채 기본 공격·도발·치유의 빛·정조준·화살비·동료의 습격·난도·회심의 일격·회오리 베기를 시작하면 즉시 닫히고, 마우스를 기존 HUD 위에 계속 둬도 연출 중 재오픈되지 않으며 연출 종료 뒤 새 Hover/포커스에서 다시 열리는지 확인한다.
112. 마도사 스킬 메뉴의 파이어 볼에 Warm Explosion peak 아이콘과 단일 마법·전후열 자유·170%·화상 30%×2·재사용 3턴 설명이 표시되는지 확인한다.
113. 전열과 후열 적을 자유롭게 선택할 수 있고 마도사에게 도발 강제 대상이 있으면 그 한 명만 선택되는지 확인한다. 대상 선택 Esc/취소는 스킬 메뉴로 돌아가며 행동·쿨타임을 소비하지 않아야 한다.
114. `파이어 볼!`→Solar Shrapnel 초기 Charge→80×80 fireball 이동→192×192 Warm Explosion 순서이며, 폭발 index 4 전에는 HP가 줄지 않고 index 4에서 한 번만 170% 피해·화상 2·피격 반응이 발생하는지 확인한다.
115. 화상 대상이 행동을 마칠 때마다 46×46 작은 불꽃과 30% 저장 피해가 한 번 발생해 `화상 2→1→제거`되는지 확인한다. 큰 Warm Explosion이나 새 Projectile은 틱 때 나오지 않아야 한다.
116. 화상 1회가 남았을 때 다시 맞으면 합산되지 않고 2회로 갱신되며, 명중 당시 마도사 Attack 기준의 새 틱 피해로 교체되는지 확인한다. 화상 종료 후 재적중하면 새 화상 2가 생겨야 한다.
117. 사용 직후 파이어 볼 재사용 3턴/hourglass_top, 다음 마도사 행동마다 2/hourglass→1/hourglass_bottom→0/사용 가능 순서이고 다른 참가자 행동에는 감소하지 않는지 확인한다.
118. 화상 틱과 VFX 중 입력·두 정보 팝업이 억제되고 피격 완료 뒤 다음 턴이 한 번만 진행되는지, 화상으로 전투불능/승리가 발생해도 턴·승리·Field 복귀가 정상인지 확인한다.
119. 마도사 스킬 메뉴에서 썬더볼트에 electric-impact peak index 1 아이콘과 `대상: 적 전체`, 90%, 감전 1, 다음 행동 주는 피해 15% 감소, 재사용 3턴 설명이 표시되는지 확인한다.
120. 전열·후열에 적을 나누어 배치하고 전투불능 적을 섞었을 때 살아 있는 적 전체에만 짧은 청백색 예고와 144×144 electric-impact가 거의 동시에 나타나는지 확인한다.
121. electric-impact index 1 전에는 HP가 줄지 않고 index 1에서 모든 대상이 각각 일반 공격 90% 피해와 감전 1을 동시에 받으며, 방어 중 대상에는 기존 50% 받는 피해 감소가 유지되는지 확인한다.
122. 적에게 도발 상태가 있어도 썬더볼트는 한 명으로 좁혀지지 않고 EnemyAll 전체를 유지하며, 전열 전용 회오리 베기와 후열 전용 화살비 범위는 그대로인지 확인한다.
123. 감전된 서로 다른 적의 HUD와 상세 팝업에 각각 Kenney power 아이콘+`감전 1`이 보이고, 한 대상이 다시 맞아도 감전 2가 아니라 1로 갱신되는지 확인한다.
124. 감전된 대상의 다음 기본 공격·단일/광역 스킬 피해가 기존 값의 85%로 적용되고, 여러 대상을 공격해도 그 행동의 모든 피해가 감소하는지 확인한다.
125. 감전된 대상이 방어·회복·도발처럼 공격하지 않는 행동을 해도 행동 종료 후 감전이 제거되며 다른 대상의 감전은 독립적으로 남는지 확인한다.
126. 감전 대상이 전투불능이 되면 power 아이콘과 상세 상태가 즉시 사라지고, VFX·피격 완료 전 입력과 두 정보 팝업이 억제된 뒤 다음 턴이 한 번만 진행되는지 확인한다.
127. 썬더볼트 성공 뒤 재사용 3턴/hourglass_top, 다음 마도사 행동마다 2/hourglass→1/hourglass_bottom→0/사용 가능이며 파이어 볼 쿨타임과 독립인지 확인한다.
128. 기존 파이어 볼·화상·도발·치유의 빛·사수/투사 스킬·Formation·대상 선택/Esc·턴 순서·HP HUD·승리/도망/Field 복귀와 Console에 회귀가 없는지 확인한다.
129. 마도사 스킬 메뉴의 가이아 웰에 `dice_shield.png` 아이콘과 자기 보호·자신·받는 피해 60% 감소·자신의 다음 2회 행동·재사용 4턴 설명이 정확히 표시되는지 확인한다.
130. 가이아 웰 선택 시 대상 선택이나 Projectile 없이 마도사 위치에서 150×150 arcane-parry 16프레임이 20 FPS로 재생되고 peak index 8에서 HUD에 `가이아 2`가 나타나는지 확인한다.
131. 가이아 웰 사용 행동이 끝난 직후에도 `가이아 2`가 유지되고, 다른 아군·적의 여러 행동에는 감소하지 않는지 확인한다.
132. 마도사가 다음 행동으로 공격·파이어 볼·썬더볼트 등을 정상 사용한 뒤 `가이아 1`, 그다음 자기 행동 종료 뒤 상태와 아이콘이 제거되는지 확인한다.
133. 가이아 웰 활성 중 원시 피해 10이 4로 줄어드는지 확인하고, 방어 버튼이 사용 불가 색상으로 보이는지 확인한다. 클릭·키보드 Submit 시 `더 강한 방어 효과가 이미 적용 중이라 방어를 사용할 수 없습니다.` 안내 후 행동·턴이 유지되어야 한다.
133-1. 가이아 웰과 방어가 어떤 경로에서도 중첩되지 않고, 가이아 종료 후 방어 버튼 색상과 기존 10→5 피해 감소가 정상 복구되는지 확인한다.
134. 가이아 웰이 다른 아군 피해를 줄이거나 도발 대상을 바꾸지 않고, 마도사 전투불능 시 `가이아 n` HUD·상세 상태가 즉시 제거되는지 확인한다.
135. 사용 직후 재사용 4턴 표시가 생기고 다음 마도사 행동 시작마다 3→2→1→0으로 감소하며, 쿨타임 중 다시 사용할 수 없고 다른 참가자 행동에는 감소하지 않는지 확인한다.
136. 연출 중 입력과 두 정보 팝업이 억제되고 완료 후 다음 턴이 한 번만 진행되며, 기존 파이어 볼/화상·썬더볼트/감전·도발·치유·사수/투사 스킬·승리/도망/Field 복귀에 회귀가 없는지 확인한다.
137. Field_01에서 기존 초원 슬라임 5마리와 독침벌 3마리, 총 8마리가 서로 과도하게 겹치지 않고 Bounds 안에서 배회하며 독침벌 Idle 날갯짓과 Scale 0.85가 자연스러운지 확인한다.
138. 각 독침벌과 접촉해 Battle로 진입하고 전열 초원 슬라임 A/B·후열 독침벌 1 배치, 이름·HP HUD·대상 판정과 독침벌 우향 Idle 반복을 확인한다.
139. 독침벌 기본 공격에서 실제 Attack 시트가 재생되고 공격 완료 뒤 Idle로 복귀하며, 피해량과 턴 진행은 기존 적 기본 공격 규칙이고 독 상태가 생기지 않는지 확인한다.
140. 독침벌 조우 승리 시 접촉한 필드 스폰만 사라졌다 30초 뒤 자기 시작 위치에 리스폰하고, 도망 시 제거되지 않으며 복귀 직후 2초 재조우 유예가 유지되는지 확인한다.
141. 독침벌 직접 공격이 슬라임 10 대비 12 피해이며 방어 중에는 기존 방어 규칙이 유지되고, 정상 적중 대상 HUD에 Venom Ward index 6 아이콘과 `독 3`이 표시되는지 확인한다.
142. 독 대상 자신의 행동 종료에만 최대 HP 5% 올림·최소 1 피해와 작은 Acid Splash가 발생해 `독 3→2→1→제거`되고 다른 참가자의 행동에는 감소하지 않는지 확인한다.
143. 독 1/2/3에서 다시 독침벌 공격을 받으면 더해지지 않고 독 3으로 갱신되며, 완전 종료 뒤 재적중하면 새 독 3이 생기는지 확인한다.
144. 상세 팝업에 `독 n`과 `독: 행동 종료 시 최대 HP 5% 피해`가 보이고, 독 피해로 전투불능이 되면 독 아이콘·문구가 즉시 사라지는지 확인한다.
145. 화상과 독이 같은 대상에 있으면 행동 종료에 두 작은 틱 연출이 순서대로 한 번씩 발생하고 모두 끝난 뒤 다음 턴이 한 번만 진행되는지 확인한다.
146. 치유사 스킬 메뉴에서 spectral-bloom peak index 5 아이콘과 `정화` 이름이 함께 보이고 상세 팝업에 상태이상 해제·살아 있는 아군 1명·독/화상/감전 모두 제거·재사용 2턴·구현 사용 가능이 표시되는지 확인한다.
147. 정화 대상 선택에 자신을 포함한 살아 있는 아군만 포함되고 전투불능 아군은 선택할 수 없으며, Esc/취소 시 행동 없이 스킬 메뉴로 돌아가는지 확인한다.
148. 상태가 없는 살아 있는 아군을 선택하면 `정화할 해로운 상태가 없습니다.` 안내 후 VFX·행동·턴·쿨타임 없이 스킬 메뉴가 유지되는지 확인한다.
149. 독 3 대상에게 정화를 사용하면 spectral-bloom 전체 16프레임이 20 FPS로 재생되고 release index 7 전후에 독 HUD·상세 상태가 즉시 사라지며 HP는 변하지 않는지 확인한다.
150. 독+화상 및 독+화상+감전 대상에게 각각 한 번 사용해 걸린 해로운 상태가 모두 동시에 제거되는지 확인한다.
151. 정화가 대상의 방어·가이아 웰·도발 관련 상태·기세·각 스킬 쿨타임을 제거하거나 변경하지 않는지 확인한다.
152. 정화 연출 중 입력과 스킬/상태 상세 팝업이 억제되고 연출 종료 뒤 다음 턴이 한 번만 진행되는지 확인한다.
153. 성공 직후 정화 재사용 2턴이 표시되고 다른 아군·적 행동에는 유지되며, 다음 치유사 행동에 1턴, 그다음 치유사 행동에 사용 가능으로 돌아오는지 확인한다.
154. 기존 치유의 빛·회복의 파동·독 틱·화상 틱·감전 피해 감소와 모든 직업 스킬, 방어·도망·승패·Field 복귀에 회귀가 없고 Console에 Compile Error·NullReference·MissingReference가 없는지 확인한다.
155. 독침벌의 첫 행동이 12 피해와 독 3을 함께 적용하고, 두 번째·세 번째 행동은 각각 12 피해만 적용하며 독을 새로 부여하거나 갱신하지 않는지 확인한다.
156. 두 번째 독침벌 행동 종료에 내부 대기 2→1, 세 번째 행동 종료에 1→0이 되고 네 번째 행동에서 다시 12 피해+독 3을 적용하는지 확인한다.
157. 첫 독 부여 직후 치유사 정화로 독을 제거하고, 이어지는 벌의 두 공격에서는 12 피해만 받고 독이 즉시 재적용되지 않으며 네 번째 벌 행동에서만 다시 독이 생기는지 확인한다.
158. 독이 정화 없이 자연 종료되어도 해당 벌의 독침 대기는 별도로 유지되는지 확인한다.
159. 독침이 사용 가능한 상태에서 이미 독인 대상을 공격하면 독이 4 이상 중첩되지 않고 3으로 갱신되며 해당 벌의 대기가 다시 2로 시작하는지 확인한다.
160. 향후 검증용으로 독침벌을 2마리 이상 배치했을 때 한 벌의 독 부여와 행동 종료가 다른 벌의 독침 대기시간을 시작하거나 감소시키지 않는지 확인한다.
161. 매 행동 120% 직접 피해, 독 5%×3, 독 HUD 3/2/1, 정화·화상·감전·모든 직업 스킬과 승리/도망/Field 복귀가 유지되고 Console 오류가 없는지 확인한다.
162. 수호자 스킬 메뉴에서 `structure_wall.png` 아이콘과 `철벽` 이름이 함께 보이고, 상세 팝업에 자기 보호·자신·70% 감소·자신의 다음 2회 행동·재사용 4턴·공용 방어 중첩 불가가 표시되는지 확인한다.
163. 철벽 사용 시 대상 선택·Projectile 없이 수호자 발밑에서 Earth Rupture 전체 20프레임이 짧게 한 번 재생되고 peak index 9 부근에 `철벽 2` HUD가 나타나며, 연출 종료 후 암석이 캐릭터를 가리지 않는지 확인한다.
164. 철벽 사용 행동 종료 직후 `철벽 2`가 유지되고 다른 아군·적 행동에는 감소하지 않으며, 수호자의 다음 공격·스킬·방어 외 행동 종료 후 `철벽 1`, 그다음 자기 행동 종료 후 제거되는지 확인한다.
165. 철벽 중 원시 피해 10이 3으로 적용되고, 방어 버튼이 사용 불가 색상이며 클릭·키보드 Submit 시 `더 강한 방어 효과가 이미 적용 중이라 방어를 사용할 수 없습니다.` 안내 후 행동·턴이 유지되는지 확인한다.
166. 철벽 종료 후 공용 방어가 다시 가능하고 피해 10→5가 유지되며, 철벽과 공용 방어가 어떤 입력·피해 경로에서도 중첩되지 않는지 확인한다.
167. 철벽과 도발을 동시에 유지한 상태에서 적 단일 공격이 수호자로 향하고 70% 감소가 적용되며 두 상태의 남은 횟수가 각자 규칙대로 독립 감소하는지 확인한다.
168. 철벽 성공 직후 재사용 4턴/hourglass_top이 표시되고 수호자의 다음 행동 시작마다 3→2→1→0으로 감소하며 다른 참가자 행동에는 변하지 않는지 확인한다.
169. 수호자 전투불능 시 철벽 상태·아이콘·상세 문구가 즉시 제거되고, 연출 중 스킬/상태 팝업 억제와 완료 후 입력 복구·단일 턴 진행이 정상인지 확인한다.
170. 기존 도발·공용 방어·가이아 웰·모든 직업 스킬·독/화상/감전/정화·Formation/TurnOrder·승리/도망/Field 복귀에 회귀가 없고 Console에 Compile Error·NullReference·MissingReference가 없는지 확인한다.
171. 수호자 스킬 메뉴에서 실제 `pawns.png`와 `수호의 맹세`가 보이고 상세 설명에 광역 보호·자신 제외 생존 아군 전체·직접 피해 최대 50%·감소량 절반 이전·최대 HP 40% 보호 예산·DoT 제외·다음 행동 전·4턴 재사용이 표시되는지 확인한다.
172. 사용 시 Frost Nova 중간 테두리가 금백색·낮은 강도로 파티 전체에 짧게 한 번 재생되고 공격용 얼음 폭발처럼 과도하지 않으며 지속 VFX가 남지 않는지 확인한다.
173. 원시 피해 20을 보호 아군이 받으면 아군 10, 이전 예정 5가 되고 수호자에게 철벽이 없을 때 실제 5 피해가 적용되며 금색 선·섬광과 HUD 예산 감소가 함께 보이는지 확인한다.
174. 최대 HP 100 수호자의 이전 예산이 40으로 시작하고 철벽 중에도 이전 예정량 기준으로만 40까지 소비되며, 각 실제 이전 피해에는 70% 감소가 적용되어 예정 5가 실제 2가 되는지 확인한다.
175. 남은 예산 3에서 큰 직접 피해를 받으면 수호자 이전 예정량 3, 아군 감소량 최대 6만 적용되고 예산 0과 동시에 수호의 맹세 HUD가 사라지는지 확인한다.
176. 화상·독 행동 종료 틱은 아군에게 그대로 적용되고 수호자 HP·이전 예산·금색 이전 연출이 변하지 않는지 확인한다.
177. 수호자의 다음 행동 시작 직전에 수호의 맹세가 종료되고 쿨타임이 3으로 감소하며, 다른 참가자의 행동 시작·종료에는 보호와 쿨타임이 유지되는지 확인한다.
178. 수호자 전투불능 즉시 보호가 종료되고 도발·철벽과 동시에 유지 가능하며, 수호자 자신의 직접 피격은 이전 없이 기존 철벽·방어 규칙으로만 처리되는지 확인한다.
179. 단일 기본 공격·파이어 볼 등 단일 공격 스킬·화살비/회오리 베기/썬더볼트 같은 광역 공격의 아군 대상마다 보호 예산이 순서대로 소비되고, 대상 수를 4명 이상으로 늘려도 고정 3인 제한이 없는지 확인한다.
180. 기존 공용 방어·가이아 웰 대상이 보호받을 때 수호의 맹세 감소 뒤 각 대상의 기존 개인 방어가 유지되고, 도발·정화·감전·회복·승패·도망·Field 복귀에 회귀 및 Console 오류가 없는지 확인한다.

기존 Male/Female, Path Visual과 Wheelchair Variant, 이름표, 월드 경계·전환·초원 슬라임 필드 Animation도 회귀가 없는지 함께 확인한다.

181. 한 번의 Play Mode에서 회복의 파동·정화·철벽·수호의 맹세·가이아 웰을 각각 3회 이상 사용해 캐릭터 본체 색 변화·흰 네모·단색 사각형·잔존 Image·반복 누적이 없고 원본 VFX 후광만 정상 표시되는지 확인한다. 이어서 Play→Stop을 최소 3회 반복해 같은 항목을 재확인한다.
182. 파이어 볼·썬더볼트·화살비·동료의 습격·독/화상/감전·숲거미 독액 분사의 기존 색과 피해 횟수가 유지되는지 확인한다.
183. 민첩 동률 전투를 여러 번 새로 시작해 주사위가 순차 반복이 아닌 1~6 랜덤 눈으로 바뀌고 최종 눈도 항상 1이 아닌지, 결과 문구·Timeline·첫 행동자가 일치하고 다음 라운드에는 재표시되지 않는지 확인한다.
184. 민첩 동률이 없는 전투에서는 Overlay가 없고 바로 정상 입력으로 시작하는지 확인한다.
185. 화살비 후열 0명과 회오리 베기 전열 0명에서 범위별 경고 뒤 같은 스킬 메뉴가 즉시 조작되고 Esc·취소로 상위 명령에 복귀하며, 행동·턴·쿨타임·기세·VFX가 변하지 않는지 확인한다.
186. 기본 공격과 단일 스킬 Target Selection에서 화면 취소·Esc가 각각 같은 이전 메뉴로 돌아가고, 대상이 있는 화살비·회오리 베기·썬더볼트의 대상당 1회 피해와 감전·기세가 유지되는지 확인한다.

VFX PNG 전부가 RGBA Alpha 0~255이고 Sprite Import의 Alpha Is Transparency가 켜져 있으며, Arcane Parry 16·Radiant Heal 14·Spectral Bloom 16·Earth Rupture 20·Frost Nova index 4~10의 선언 범위가 실제 시트 크기 안에 있음을 확인했다. 회복의 파동 중앙 VFX에 남아 있던 구형 `CreateEffectImage` 일곱 번째 인수를 제거했고 프로젝트 내 15개 호출이 현재 5/6개 인수 시그니처와 일치한다. 광역 스킬 0 Target 입력 복구까지 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 응답 파일로 별도 출력 컴파일해 오류 0개, 기존 deprecated API 경고 4개만 확인했으며 관련 파일 `git diff --check`를 통과했다. Tie Break와 수호의 맹세 계산·피드백·VFX 코드는 변경하지 않았다. 실제 Play Mode의 화살비·회오리 베기 Invalid Action 메뉴 복구와 Esc·취소, 수호의 맹세 및 위 171~186 항목은 직접 확인해야 한다.

## 다음 권장 작업

길별 새 캐릭터로 Battle에 진입해 회복탄력 50% 통과와 2회 소비, 적별 잔향 소비·만료, 집중 0/3/6/9%와 광역 비증가, 굳건한 자리 전열/후열, 적·아군 쌍별 패턴 피해 감소·치유 증가를 확인한다. 같은 전투에서 화상·독이 길 배율을 받지 않는지, 전투 종료 후 새 전투와 저장 이어하기에서 길 런타임이 초기화되는지도 확인한다.

우선 실제 정상 입력으로 Field_01/02 전투 승리·레벨 업·즉시 저장/재실행 복원·맹독 교체/정화·뱀 이동/공격/리스폰을 확인한다. 이어 실제 Battle에서 위 15개 스킬 버튼의 가독성·Esc 재진입·다음 턴/새 전투 아이콘 유지를 확인한다. 썬더볼트 버튼 Electric Impact와 감전 상태 power.png의 구분을 유지한다.

Unity Play Mode에서 월드 안쪽으로 이동 후 5초 자동 저장과 Stop/재실행 뒤 실제 위치 복원, Bounds 밖 좌표의 SpawnPoint fallback, Field 전환 뒤 새 Scene 좌표 저장, Battle 중 종료 시 마지막 안전 좌표 유지를 확인한다. Bootstrap 삭제 확인의 취소·단일 슬롯 삭제·즉시 빈 슬롯 갱신과 Editor 관리 창도 함께 확인한다. 이후 Field_02 감전 독립 적용과 기존 전투·외형·리스폰 회귀도 확인한다.

## 갱신 규칙

기능 작업 완료 시 완료 기능, 검증 상태, Unity 직접 확인 사항, 다음 권장 작업과 마지막 관련 commit hash를 갱신한다. 긴 작업 로그는 기록하지 않는다.
