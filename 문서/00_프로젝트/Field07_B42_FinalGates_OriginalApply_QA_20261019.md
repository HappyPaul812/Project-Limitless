# Field07 최종 B42·Spawn/Ash 변경분 및 원본 반영 (2026-10-19)

상태: **B42_ISOLATED_PASS / ORIGINAL_DISK_APPLIED / USER_EDITOR_NOT_VERIFIED / NO_PUSH_NO_MAIN_MERGE**. 기능 `ffdb5ff162f915cb88e4aeab03ac34c4ad9a7039`, 브랜치 `codex/field07-v1-unity-integration-20261019`.

최신 `docs/planning` `Main10_Field06_07_Final_Gates_Field09_VisualReview_20261019.md`의 조건부 B42 선택과 이번 사용자 지시(검증 통과·충돌 없을 때 원본 적용)에 따라 실행했습니다. 이전 Field06 349 / Field07 324 성공·Visual20·Bridge21은 역사 증거로 재사용, Main10 164/입력132 및 기존717 전체 QA 반복0/TTS0.

## 최종 변경과 실제 검증

- 두 Field 모두 B42를 **실제 Scene에 저장**: 기존 Road30 재배치+Renderer12 추가. Field06 Ground315/Road42/Props18=375, Field07 AshSoil315/Road42/나머지27=384. Road Color.white/Flipfalse/PPU128, Upper/Lower/WestEnd 대응, 같은1280·960·1680×720 실제 RenderTexture 확인. 비도로 Transform/Collider/Quest Script 참조는 변경하지 않았습니다.
- 공통 신규 변경분 **124 PASS/0 FAIL**. Spawn 실제 충돌/안전보정·Clearance/물리 후속 drift, 휠체어 Main16 진동 접근·Path Hearing, 세린·Encounter4/12목표 정의 보존, Field06/07/Arbel 저장·Continue, 도보/휠체어 상태별 Field05·06·07·08 왕복 서비스/Spawn, Ash7 pulse·정식 투명Sprite·3비율 화면. 기존 Story12단계 전체·불씨망령 승리전투는 재플레이하지 않았으며 새 전체PASS로 주장하지 않습니다. 기존 몬스터/전투/TTS/Story 데이터·흐름은 보존했습니다.
- 최초 새 harness에서 Field05→Arbel의 별도 Spawn까지 Field06의 Fence 기준으로 비교한 오류1은 조건을 좁혀 고쳤습니다. 해당 harness 실패는 소스게임 오류가 아니며 최종124의 FAIL0과 구분하여 Temp/Attempt1 원본로그를 보존했습니다. 원래 기록의324PASS/1FAIL은 실제 Arbel Spawn검사였고 Ash7은 별도 시각 결함입니다. 기획문서의 실패 원인 요약과 이 세부 로그 차이를 명시하며 두 문제 모두 이번 수정으로 확인했습니다.
- 기존 UnityEditor.Search/URP Host type mismatch 기준선 유지, 신규 Runtime 오류0. 원본 Editor/Play/Refresh/포커스 조작0, 자연Import·사용자 손플레이는 미검증입니다.

## Arbel Spawn 실제 원인·수정 기준

정의(-6,5)와 울타리 `Fence_-6.5_5.3` BoxCollider(중심 약-6.5,4.95/반폭.45/.1)에 플레이어 Circle 반경.5가 겹쳤습니다. 이전0.4548683 이동은 물리 엔진의 겹침 분리입니다. 정식 Spawn 데이터/울타리/Player Collider를 옮기지 않고 **SceneSpawnPoint에서 실제 ColliderDistance2D 겹침 깊이·법선으로 분리 후 저장**합니다. Trigger/자기Collider/이동Rigidbody/충돌제외Layer는 보정에서 제외, 여유.01, 최대8회입니다. [Unity 공식 ColliderDistance2D 기준](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/ColliderDistance2D.html).

새 Arbel 안전 도착은 x≈-5.53/y5, 정의 대비0.4700003입니다. **정의 좌표 편차를0으로 만들었다는 뜻이 아닙니다.** 울타리 bounds.max.x+플레이어반경+.01에서 허용오차.015, y오차.01, 후속물리 drift<.005, Continue복원오차.02로 검증했습니다. Main15/16귀환지점(-6,5)의 조사 반경 안입니다. 다른 Field05/06/07/08 Spawn 오차는<.02(측정최대약0.000000477)로 유지됐습니다. 고정0.455 보정은 사용하지 않습니다.

## Ash7·선택 아트 경계

Ground복제를 제거하고 기존 `Art/Environment/Dungeon01B1/V1/Ground/LL_C1_D01B1_Dust_Overlay_128_v1.png`를 바이트 그대로 `Resources/Chapter2/Field07AshDust.png`에 복사·새GUID로 연결했습니다. 원본 PNG93.26% 완전투명/maxAlpha120, 신규그림/리터칭0. 새 복사본·원본 SHA동일, 7개 기존 위치/pulse/Sorting 유지, 작은먼지가 보이도록 scale(.5,.3)만 사용. 실제확대에서 불투명사각형이 사라졌고 움직임·7개생성 검증. 선택형 WindlessAsh PNG는 비교가 필요하지 않아 **미적용**. Field06 Ground49 해결 유지, Runtime6/Decal2 선택PNG 미적용 유지.

## 안전한 원본 반영

작업 시작4272파일 SHA 고정/UserData·Settings·ProjectSettings 백업, 기존 변경대상5가 예상 origin/main 내용과 정확히 일치함을 확인했습니다. 원본 Unity 프로세스가 없음을 확인하고 기존5파일 백업 SHA/새67 GUID 충돌0 후 두 지역을 비파괴 반영했습니다. 기존4267파일 SHA 불변, 신규67 정확 SHA, UserData23·Main09승인27/Main10승인18/Main18·19·Field05/Arbel/Field08·09 공식PNG 보호. 원본HEAD/index 유지, main `675988c9f2c5c507d9a696b0378be27c0bd3309b`. GitPush/PR/main병합0.

다음: 대표님 평소 원본Editor 자연Import·Main15/16 실제 화면/연속 진행 확인. 이 두 필드의 B42·기존투명먼지 재사용은 이번 승인 범위로 완료되어 반복 배치승인이 필요하지 않습니다. Field09의 지면반복선·6오브젝트 가이드는 별도 읽기검수 결과이며 새PNG 보완은 별도 승인 전 금지합니다.
