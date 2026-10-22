| 항목 | 결과 |
|---|---|
| 원본 게임 파일 반영 | **GAME_APPLIED_LOCAL** — 제품4파일 백업·충돌 검사 후 반영 |
| 격리 Unity 변경분 | **162 PASS / 0 FAIL**, 제품 compile/runtime 오류0 |
| 승인 아트 보존 | ZIP/Strip SHA 동일, 미술 재제작·기존 전체 QA 재실행0 |
| GitHub | **Push/PR/병합0**, 읽기 재확인한 main에는 Fox v7 연결 미포함 |

# 승인 Fox Run6 v7 게임 적용

바울 대표 최종 미술·모션 승인과 [Issue #52](https://github.com/HappyPaul812/Project-Limitless/issues/52)를 근거로 실행했습니다. 이전 미술 PASS 권고를 임의 승인으로 격상한 것이 아니라 사용자 직접 최종 승인을 반영한 작업입니다. 승인된 미세 밝은 털띠를 포함하여 아트는 그대로 동결했습니다.

[실제 제품 Presenter 좌향128px](Evidence/FoxRunV7Apply20261022/ActualPresenter_Trial0_128px.gif) · [우향128px](Evidence/FoxRunV7Apply20261022/ActualPresenter_Trial1_128px.gif) · [40px Camera](Evidence/FoxRunV7Apply20261022/ActualCamera_Trial0_40px.gif) · [전체 실제 RT 캡처](Evidence/FoxRunV7Apply20261022/ActualCamera_Trial0.png)

## 실제 변경4파일

| 파일 | 변경 목적 |
|---|---|
| `Unity/Client/Assets/_Project/Resources/CompanionAssaultValidation/Fox_Run6_v7.png` | 승인 Strip을 압축 PNG 바이트 그대로 신규 등록 |
| 위 파일의 `.meta` | 신규 GUID, Point/Uncompressed/Mip없음/Readable/최대4096. 기존 메타·GUID는 그대로 |
| `Unity/Client/Assets/_Project/Scripts/Battle/BeastCompanionSystem.cs` | Fox Run 경로·512px 셀·0.25표시배율 연결. `fox` 저장 ID·12FPS·속도570·기존 Fox_Run 아이콘64px/Frame00 유지 |
| `Unity/Client/Assets/_Project/Scripts/Battle/BattleActionPresenter.cs` | Fox도 승인512px 규격을 사용한다는 설명 주석3곳만 갱신. 실행 로직 변경0 |

KeyArt/Idle·Animator/Controller·Scene·Quest·Save schema·기존 아이콘 원본·Wolf/Bear PNG/meta는 변경하지 않았습니다. 승인6프레임을 다시 그리거나 가공하지 않았고, 제품에는 단일 Strip만 추가했습니다.

승인 ZIP SHA **`de6139858b2f5de95c9e8256a350f0e45744f15e404efb00161688c88e1c0030`**, CRC PASS를 새로 읽어 확인했습니다. 제품 Strip SHA **`5487868681c3c93278417b754201aff681c2f78317e66774b3404b0629570e8a`**, **840,191bytes**, 승인 Manifest 지정값과 일치합니다. 이전 전체 미술·Manifest26/SUMS27/CSV6 검사와 사용자 최종 승인 결과는 재사용하며 전수 반복하지 않았습니다. [납품·제품 원본 SHA](Evidence/FoxRunV7Apply20261022/Delivery.json).

## 기존 Bear 미커밋 연결 보호

원본은 main HEAD `510c021b4159167e211be60ca90e1cb7328bafe9`이며 공유 코드2개에 **이전 승인 Bear 제품 패치**가 미커밋으로 존재했습니다. 두 파일 모두 승인 Bear 커밋 `5b89ac925402855251f01386009390780d74a6d4`와 정규화 바이트가 정확히 같고 별도 사용자 코드가 섞이지 않았음을 확인했습니다.

따라서 별도 Fox 브랜치를 그 승인 Bear 제품 커밋에서 시작했습니다. **Fox 제품 커밋의 부모는 Bear 승인 커밋**이며, 이번 새 제품 diff는 Fox4파일뿐입니다. Bear PNG/meta를 원본에 재복사하지 않았고, 공유 카탈로그 안 Wolf/Bear 선언·설정·주석은 그대로 보존했습니다. main 기준 단순 diff에 이미 존재한 Bear 변경이 보이는 것과 이번 Fox 변경을 구분해야 합니다.

최종 적용 직전 원본 HEAD/index/status·모든 기존 파일 SHA를 재확인하고, 공유 코드2개가 여전히 승인 Bear 기준과 같은지 재확인했습니다. 두 원본 파일은 `Temp/FoxRunV7Apply20261022/OriginalBackup/`에 백업한 뒤 승인된 Fox 변경만 복사했습니다. 무단 dirty 덮어쓰기·reset/clean/stash는0입니다.

## 실제 격리 Unity 결과

Unity **6000.6.5f1 / Direct3D12**, **162 PASS / 0 FAIL**. 기존 제품 검증용 scratch의 컴파일/패키지 캐시를 재사용하되 **새 FoxApplyQA 빈 장면과 Fox 전용 저장 경로만 실행**했습니다. scratch 경로가 `Temp/BearRunV4Apply20261022/UnityQA`인 것은 환경 재사용 때문이며 **BearApplyQA·Bear154·Wolf·Main22 테스트를 실행하지 않았습니다**. 새 결과/Save/설정은 `Temp/FoxRunV7Apply20261022/Runtime`에 분리했습니다.

- 실제 `BeastCompanionService.TryEquip("player","fox")` → 격리 JSON 저장 → `TryLoadSlot` → `RestoreSession` → `GetEquippedOrDefault`가 승인 Fox v7을 선택하는 경로 PASS.
- 실제 `PetFacilityPresenter.GetIcon`이 기존 Fox_Run의64px Frame00을 사용하는지 PASS. 새 Run으로 아이콘 원본을 바꾸지 않았습니다.
- 실제 `BattleActionPresenter`의 런타임 slicer가6셀512×384, Import texture3072×384를 로드하는지 PASS.
- 제품 Presenter를 좌향·우향·높이차 **3조건**에서 실제 실행. 각 조건 00~05와05→00 루프, engine frame/time 증가·좌우 flip·이동·표시128×96 확인. 별도 같은 프레임40×30 축소 Image를 Camera로 함께 촬영했습니다. 40px는 기존 시설 아이콘 교체나 제품 표시 크기 변경이 아닙니다.
- 각 조건 actual `Combatant.TakeDamage(10)`으로 HP100→90 정확히1회, impact/notify/complete 각1회 PASS. 연출 뒤 배우/대상 좌표 복원·임시 Fox 객체 제거 및 호출 이후 저장 복원에서도Fox v7 선택 PASS.
- 이는 Fox 호출·공격·연출종료 영향 경계 검사입니다. 전체 BattleScene 승패/Scene전환·전투밸런스·Main22 회귀를 재검증한 것이 아닙니다.

실제 Camera/RenderTexture PNG를 근거로 GIF를 생성했습니다. **Editor GameView UI 캡처가 아닙니다.** GIF는 실측 프레임 간격을10ms 단위로 반올림한 참고입니다. 제품12FPS 설정은 유지하며 Batch PNG 캡처 중 실측 프레임 간격은 **85.64~91.29ms, 평균87.66ms**로 기록했습니다. 벽시계12.000FPS를 보장했다고 표현하지 않습니다. [엔진 프레임·좌표·시간·PASS 원기록](Evidence/FoxRunV7Apply20261022/RuntimeQA.json), [요약](Evidence/FoxRunV7Apply20261022/RuntimeSummary.json).

제품 C# 컴파일/런타임 오류0. **기존 Editor SearchDatabase 초기화 예외1은 OPEN**으로 별도 분류·원문 보존했습니다. Console 전체 오류0으로 보고하지 않으며 범위 밖 Search/URP를 수정하지 않았습니다. 원본 Editor의 자연 Import·사용자 손플레이는 수행하지 않았습니다.

## 원본 보호 결과

- 시작 시 원본 Unity 관련4,402파일 중 **허가된 공유 코드2개만 변경**, 그 외 기존파일4,400개 SHA 동일. 승인 새 PNG/meta2개만 추가.
- 기존 dirty/untracked862파일 중 두 공유 코드의 Fox 변경 외 **860파일 SHA 모두 동일**. 두 공유 코드는 변경 전 승인 Bear 기준 일치를 확인했고, 변경 후 Wolf/Bear 선언 영역은 그대로입니다. 미커밋862개 전부 바이트 동일이라고 오기하지 않습니다.
- 승인 Wolf/Bear 원본 자산·기존 Fox 아이콘/원본 meta·Save/UserData·Packages/ProjectSettings·Village v10 승인 후보9 SHA 보호.
- 원본 main HEAD/index 동일. 원본 Editor/Play/Refresh/포커스 조작0, 원본 Save/UserData 쓰기0. [백업·파일별 최종 SHA와 보호 결과](Evidence/FoxRunV7Apply20261022/ProtectionAndApplication.json).

## 브랜치·커밋·GitHub

브랜치 **`codex/fox-run6-v7-apply-20261022`**, 제품 커밋 **`332d993253b2ce6ae722cf04a01c41bd23d4c004`**. 부모 승인 Bear 제품 커밋은 `5b89ac925402855251f01386009390780d74a6d4`입니다. 원본 main 직접 커밋은0이며 실제 제품은 원본 working tree에만 승인범위로 반영됐습니다.

**GitHub Push/PR생성/main병합 미수행.** Issue52는 조회 당시 OPEN, 원격 main 카탈로그를 실행 후 다시 읽어 Fox_Run6_v7 미포함을 확인했습니다(해당 blob `c80f42b025c26f7219c84b82874e52ec9cd4c1e8`). 따라서 **GAME_APPLIED_LOCAL**과 **MAIN_MERGED 아님**을 구분합니다. 원격 통합은 별도 마감 승인 절차입니다. 보고·현재상태 갱신도 별도 worktree에만 기록하며 기존 원본문서는 덮어쓰지 않습니다.
