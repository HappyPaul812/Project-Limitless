# Main22 첫 플레이 — 원본 반영 준비·안전성 조사

2026-10-21 / 적용 준비 완료 / **원본 적용 보류 · PR 미생성 · Push/병합 미실행**

작업 브랜치: `codex/chapter3-main22-apply-preparation-20261021`. 구현 기준 `be7f64a3ff48a8848471166d4aefc4969080bd8b`, 실제 구현 커밋 `1764741c572b32b6e22c2ed8b6032c81a1031b41`. 원본 local main `e4e9891f6f01ea95cada9e63b1f09946d94d6041`. 바울 대표의 “잘된다” 직접 플레이 확인을 수용하며 같은 플레이 승인을 다시 요구하지 않습니다.

## 1. 변경 파일과 적용 단위

제품 **58경로 전체 목록·분류·원본/main/후보 SHA256**은 [ProductFiles.tsv](Evidence/Main22ApplyPreparation20261021/ProductFiles.tsv)에 있습니다. 원래 구현 [ChangedFiles.json](Evidence/Main22FirstPlayable20261021/ChangedFiles.json)과 동일 집합입니다.

| 묶음 | 경로 수 | 처리 |
|---|---:|---|
| `Art/Environment/Chapter3.meta` 및 `Chapter3/NorthF1/` 환경6종·meta·Tile3종 | 20 | 신규 승인 원본 바이트 유지 |
| `Art/Monsters/FrostFieldMouse*`, `Resources/MonsterVisuals*` | 12 | 원본에 이미 동일 바이트 존재; 다시 덮어쓰지 않음 |
| `MonsterSpriteSheetAnimation.cs` | 1 | 승인 선행 수정이 원본에 존재; CRLF만 다름, 재적용하지 않음 |
| Quest1/지정조우1/임시 몬스터1 및 meta | 6 | 신규; `StoryEncounterReturns`의 기존 폴더 GUID 유지 |
| `Scenes/World_NorthF1.unity` 및 meta | 2 | 신규; 기존 마을 Scene은 편집하지 않음 |
| `Main22Progress`, `Chapter3Main22Flow`, `Main22GuidanceHud` 및 meta | 6 | 신규 런타임 코드 |
| BattlePrototypeEncounter/BattleSceneController/QuestService/PlayerController | 4 | 기존 파일에 Main22 관련 최소 변경 |
| Main22ContentBuilder/Main22FirstPlayableQA/Main22PlaytestSetup 및 meta | 6 | 격리 제작·QA·플레이 준비 도구; 원본에서 실행 금지 |
| `ProjectSettings/EditorBuildSettings.asset` | 1 | Scene 항목만 추가; 원본 설정 전체 교체 금지 |
| 합계 | **58** | 이미 반영13 + 신규40 + 기존 수정5 |

`main..be7f64a3` 전체는 **271경로**입니다. 과거 Wolf v3/v4/v5 검수, F1 시험 Scene/중복 이미지, 서리들쥐 검수 증거와 누적 CURRENT_STATUS가 포함됩니다. **Main22 브랜치 통째 병합 또는 1764741c 단독 cherry-pick 모두 그대로 적용하는 방식으로 권장하지 않습니다.** 전자는 불필요 범위를 섞고, 후자는 서리들쥐 선행 의존성을 별도로 확보해야 합니다. 제품58경로 allowlist와 관련 Main22 문서/증거만 선별하는 전용 통합 브랜치를 권장합니다. 이번 준비 브랜치 역시 검토용이며 통째 main 병합용이 아닙니다.

## 2. 원본 local main·dirty 충돌 조사

[정적 실측 JSON](Evidence/Main22ApplyPreparation20261021/StaticSafetyAudit.json), [읽기 전용 patch 검사](Evidence/Main22ApplyPreparation20261021/PatchCheck.json).

- `main`은 구현본의 조상입니다. 이력 분기 충돌은 없지만 **현재 작업복사와의 안전성은 별도**입니다.
- 제품58경로: 원본 바이트 동일12, 줄바꿈만 다른 기반 적용 완료1, 신규 부재40, 기반과 동일 기존 코드4(그중3개 CRLF), 의미상 원본 변경1입니다.
- 유일한 의미상 중첩은 `EditorBuildSettings.asset`입니다. 원본에 `com.unity.dt.app-ui` config 항목이 추가되어 있습니다. 후보 전체 파일로 교체하면 이 항목이 유실됩니다. 기존 Scene23개 순서와 configObjects 두 항목을 보존한 채 `World_NorthF1`의 enabled/path/GUID3줄만 추가해야 합니다.
- 45경로 patch의 읽기 전용 적용 검사는 BuildSettings1개에서 실패했습니다. 해당 파일을 제외한 [44경로 검사](Evidence/Main22ApplyPreparation20261021/PatchCheck44.json)는 exit0입니다. [BuildSettings 의미 병합 검토본](Evidence/Main22ApplyPreparation20261021/EditorBuildSettings_Proposed_REVIEW_ONLY.txt)은 원본을 읽어 Scene3줄만 삽입한 문서 부록이며 실제 설정 파일에 적용하지 않았습니다.
- 서리들쥐 외형12경로는 원본에서 아직 Git 미추적일 수 있습니다. 디스크에 있다는 사실과 main에 기록됐다는 사실은 다릅니다. PR에는 선행13경로도 포함하거나 해당 선행 PR을 명시해야 합니다.
- Main22 신규 Scene/Quest/코드/아트의 같은 경로 파일은 원본에 없습니다. 기존 사용자 변경을 포함하거나 되돌릴 이유가 없습니다.
- 원본 `ProjectVersion.txt`는6000.6.5f1, Git 정본/후보는6000.5.7f1입니다. 기존136건은6000.6.5f1 검증입니다. 이 차이는 본 작업이 해결하거나 확정하지 않았습니다. 원본 버전/Packages/Settings를 후보로 덮어쓰지 마십시오. 정본6000.5.7f1 실행 호환은 별도 미검증입니다.

새로운 실행 QA 수치를 만들지 않았습니다. 기존 Main22 **136 PASS/0 FAIL**, F1 **64 PASS**, 서리들쥐 **40 PASS**를 재사용하며 이번 결과는 파일·GUID·patch 정적 안전성 조사입니다.

## 3. 실제 의존성·영향 범위

**Quest:** `main_22_northwind_signs`; Main21 완료 prerequisite로 Available 판정, 북문 명시적 선택으로 시작. Main21 Asset의 빈 nextMainQuestId를 바꾸지 않습니다. 입장→서리→서로 다른 흔적2→고유 지정전투 승리→쉼터의5목표. Main23 연결 없음. QuestService의 데이터 캡처/복원/Reset에 흔적 플래그가 추가됩니다.

**Scene/Spawn:** `World_NorthF1`; 진입 `Spawn_NorthF1_FromStarterVillage`=(0,-8.5), 쉼터 `Spawn_NorthF1_Shelter`=(0,8). 마을 복귀 `Spawn_StarterVillage_FromNorthF1`=(0,4.8)는 런타임 생성됩니다. 기존 World_StarterVillage 파일은 동일하지만 런타임 영향이 없다는 뜻은 아닙니다. sceneLoaded 등록이 마을과 F1에 흐름 오브젝트를 설치하고, Main21 완료 시 `Boundary_Top_0`을 비활성화하여 북문 양옆 Collider와 수동 통과 방지벽을 만듭니다. 마을 북문(0,5.4)/F1 남문(0,-10)에서 선택 패널을 통해 SceneTransitionService를 호출합니다. 자동 접촉 Trigger 출발이 아닙니다. Bounds·뷰포트·도보/휠체어 검증은 기존 결과를 재사용하며 원본 적용 뒤 재확인합니다. 향후 마을 북벽 이름/폭/좌표가 변경되면 이 의존성부터 재조사해야 합니다.

**Save:** GameSaveService와 SaveVersion1, 기존 사용자 JSON은 변경하지 않습니다. QuestSaveData의 선택 int `Main22ClueFlags`가 추가되고 누락 시0, 복원 시 비트마스크3으로 제한됩니다. 슬롯 교체/Reset 때도 초기화합니다. 쉼터에서 `safezone_northf1_shelter`와 안전 Spawn을 세션에 활성화합니다. 기존 구버전 읽기 검증은 통과했으나 새 Main22 진행 Save를 옛 실행파일에 다시 쓰는 양방향 호환을 보장하지 않습니다. 향후 적용 전 슬롯 백업 후 복사본만 사용하며, 테스트 슬롯을 원본 UserData에 복사하지 않습니다.

**Battle:** `northf1_main22_mouse_encounter` 분기만 기존 저장 파티+전열 서리들쥐1기로 연결합니다. `prototype_main22_frost_field_mouse`는 공식 몬스터 ID가 아닌 임시 복제본입니다. 기존 BattleSceneFlow/SceneTransitionService/QuestCatalog/Chapter2Main21Flow/Main21ChoicePanel/WorldModalState/플레이어 prefab/승인 몬스터 외형·프레임을 재사용합니다. 승리만 진척, 도주/패배/안전 복귀는 기존 QA 참조. HP60/Level1/공격100%/Agility10은 임시 상속, EXP/Talent/drop/완료 보상0입니다.

**입력/UI:** PlayerController에 안내 방향 입력을 추가합니다. Main21 이전에는 활성화하지 않고, 수동 입력/모달이 안내 이동을 취소합니다. Main21 완료 뒤 마을/F1의 클릭 이동·목표HUD·상호작용이 영향 범위입니다. Quest와 전투만 변경된다고 축소 보고하지 않습니다.

**GUID closure:** 제품58경로의 직렬화 GUID는 기존 main, 제품 allowlist, 원본 URP `Sprite-Lit-Default` 패키지에서 모두 해석됩니다. QA_F1 시험 Scene/아트를 제품 Scene이 참조하는 누락 의존성은0입니다. 단, Builder 재실행은 `QA_F1`를 입력으로 사용하므로 배포된 Scene 검증과 재생성 경로를 구분합니다. 전체 후보에서 발견된 기존 GoldenArrow/HealerMagicalProjectile GUID 중복1군은 Main22가 만든 변경이 아니며 원본에서 이미 로컬 meta 차이가 있습니다. 이번에 임의 수정하지 않았고 이후 Unity Import 확인 항목으로 유지합니다.

## 4. 후속 적용 절차 초안·PR 순서 제안

아래는 **미실행 초안**입니다. 실제 적용은 바울 대표의 별도 지시 후 수행합니다.

1. 원본 HEAD/status 및 보호 대상 SHA와 UserData 별도 백업을 새로 기록합니다. dirty를 stash/reset/clean하거나 사용자 PNG/meta를 일괄 교체하지 않습니다.
2. 그때의 local main에서 **새 통합 worktree/전용 브랜치**를 만듭니다. 준비 브랜치 전체를 merge하지 않습니다.
3. 선행 서리들쥐 승인13경로를 확보합니다. 원본과 같은 바이트12개+기존 코드 의미동일1을 재확인하고 제품 기록에 포함합니다. F1 승인 환경20경로와 Main22 런타임/Scene/Quest를 allowlist로 선별합니다. QA_F1·기존 Wolf 미술 검수는 제외합니다.
4. EditorBuildSettings는 원본의 configObjects·Scene순서를 보존한 의미 병합을 합니다. 아래 REVIEW_ONLY patch는 이를 자동으로 해결한 배포 패키지가 아닙니다. Main22 도구6경로는 격리 검증 부록으로 유지할지 전용 QA 커밋에 둘지 구분합니다. 하드코딩된 격리 경로 차단을 해제하지 않습니다.
5. 깨끗한 통합본을 백그라운드 컴파일하고 Quest/리소스/GUID/Scene목록/구버전Save와 Main22 각단계 Continue, 북문선택/왕복/Bounds, 기존 Main21/펫/전투 회귀를 복사 Save로 확인합니다. Editor 포커스 전환은 하지 않습니다.
6. 제품 커밋과 Main22 검증/인계 문서 커밋을 나누어 draft PR 내용을 준비합니다. 제안 제목: `Feature: Main22 북풍의 징후 첫 플레이 연결`. 본문에 임시 대사·무보상·임시 전투수치·기존136/64/40 재사용과 새 통합 검증을 구분합니다. Push/PR 생성/병합은 이번에 하지 않습니다.
7. 별도 적용 지시가 있을 때만 승인 제품 경로를 원본 디스크에 최소 적용합니다. Editor 자동 Import 결과/Console을 비포커스로 확인하고, 보호 SHA와 Save/UserData 무변경을 대조합니다. 문제가 생기면 해당 적용 경로만 사전 백업에서 복구하며 사용자 기존 변경은 보존합니다.

[Pending45_REVIEW_ONLY.patch.gz](Evidence/Main22ApplyPreparation20261021/Pending45_REVIEW_ONLY.patch.gz)는 이미 원본에 있는13개를 제외한 **검토용45경로**입니다. 읽기 전용 `git apply --check` 결과는 PatchCheck.json을 따릅니다. 이 파일을 원본에 직접 실행하는 작업은 승인되지 않았습니다.

## 5. 직접 플레이·남은 미확정

[기존 시작 안내 유지](Main22_Isolated_Play_Guide_20261021.md). 플레이용 경로는 새 준비 worktree가 아니라 `Temp/Main22FirstPlayable20261021/Worktree/Unity/Client`입니다. Unity6000.6.5f1에서 Bootstrap →5번 슬롯 이어하기→마을 북문 직접 출발→서리/흔적2곳/서리들쥐 승리/쉼터. 기존5번 슬롯을 덮어쓰지 않는 격리 전용 메뉴가 있습니다. 원본 슬롯을 이 안내로 바꾸지 않습니다.

정식 대사/DialogueId/TTS, 공식 전투 밸런스·몬스터 ID·EXP/Talent/drop/완료 보상은 **미확정·임시**입니다. Main23·한기/동상·설원족제비 신규 조우·정식 음성/음악은 추가하지 않았습니다. 기획 DRAFT의 `UNITY_NOT_IMPLEMENTED` 표시는 과거 기획 상태이고 현재 격리 구현과 차이가 있습니다. 이를 정식 대본/밸런스 승인으로 해석하거나 원본 기획을 임의 확정하지 않았습니다. 원본에서의 새 통합 실행과 정본6000.5.7f1 호환은 이번에 미검증입니다.
