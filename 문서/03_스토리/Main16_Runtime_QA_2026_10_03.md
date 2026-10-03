# Main16 Runtime QA — 2026-10-03

기능 commit `2ef6690`. Unity 6000.5.7f1, 현재 LOCAL 프로젝트에서 [구현 계약](Main16_Runtime_구현_계약.md)에 따라 실행했다. Main16 Runtime 구현과 아래 범위의 기능 검증을 완료했다. Voice 미적용·Field07 BGM TBD이며, 화면을 활성화하는 시각 QA는 실행하지 않았다.

## 실행 방법과 격리

- `Main16RuntimeAudit.Start()`를 PlayUnfocused에서 실행했다. EditorApplication.update가 실제 Scene·DialoguePresenter·Battle 명령 버튼·Bootstrap Continue를 구동한다. foreground/창 Focus/Game View 활성화/OS 키보드·마우스 입력은 사용하지 않았다.
- Save/Settings는 `Unity/Client/Temp/Main16Audit/{guid}/`로 격리했다. 설정은 감사 폴더에서만 Mute하고, 기존 사용자 슬롯을 선택하거나 덮어쓰지 않았다. 종료 후 CurrentSlotIndex=0, Save/Settings 감사 경로=null, 기존 Play 진입 옵션 복원, Bootstrap clean Edit Mode, Editor unfocused를 확인했다.
- 최종 `Temp/Main16Audit/runtime.txt`: **PASS 체크 507개**, 실패0. 이는 네 실행의 반복 상태 확인을 포함하는 체크 수이며 독립 테스트507종이라는 뜻은 아니다. 초기 진단/일반 조우 추가 확인은 `extra.txt`에 남았다.
- Fixture는 기존 Main15까지의 Main Quest 완료 기록과 정식 동료 해금을 주입한다. Main15 전편을 다시 플레이한 검증은 아니다. 그 완료 상태에서 실제 레온 NPC로 Main16을 시작했다. 이후 Main16은 목표 count나 Victory를 직접 주입하지 않고 실제 Runtime으로 완료했다.

## 전체 진행

| 실행 | Player Path | 저장 파티 | 결과 |
| --- | --- | --- | --- |
| A | Hearing (`path.hearing`) | 세린+미엘 | 12목표·보고·완료 PASS |
| B | Hearing | 태온+미엘, 세린 미편성 | 동일 진행 PASS |
| C | Default (Physical) | 세린+미엘 | 동일 진행 PASS |
| D | Default (Physical) | 태온+미엘, 세린 미편성 | 동일 진행 PASS |

Quest `main_16_shape_in_the_ash`: 레온 소문→Field06 서쪽 도착→Field07 진입→움직이는 재→진동→동물 흔적→형상 목격→지정 승리→남은 균열/열기→협곡 입구→아르벨 귀환→레온 보고를 순서대로 확인했다. Objective ID·종류·Target은 구현 계약의 12행을 따른다. 10번 협곡은 현장 확인 대화 종료에서 `ReachLocation`을 알린다.

- 대화 중에는 진행하지 않고 완료 콜백에서 한 번만 count를 올렸다. 재 조사 대화 취소는 진행하지 않았다. 목격 대화 취소 후 자동 재열림 없이 E/F 재확인이 가능했다.
- 재·진동·전투 후 조사·협곡의 실제 Dialogue ID 배열을 선택한 Path의 Catalog 결과와 대조했다. 재 장면에서 Hearing Player가 최초 관찰, Default 세린이 최초 관찰했다. 세린의 장치 확인과 교차 해석을 유지하고 두 분기가 같은 목표로 수렴했다. 대체 분기 대사 ID는 분리하며 같은 내용의 공통 대사는 공통 stable ID를 사용한다.
- 현장 세린은 두 미편성 실행에서도 존재했고 Story 정보 획득을 막지 않았다. 각 목표와 모든 Continue에서 저장 Party/Formation JSON이 최초 수동 편성과 같았다. 실제 Battle의 세린 참가 여부도 사용자 편성과 일치했다.
- 보고는 관찰 사실과 지하 열기 가설을 구분했다. 불씨망령의 정확한 정체/최종 원인을 확정하지 않았다. 완료 후 Main17이 자동 시작되지 않았다. 완료 Quest 보상은 미확정이므로 빈 RewardBundle이며 기존 전투 보상만 적용한다.

## 첫 불씨망령과 전투

- 지정 승리 전 일반 Field 불씨망령 없음. 먼저 지면→진동→재 모임→불씨/형체의 수동 Next를 표시하고, 그 뒤 공식 외형을 활성화했다. 접촉 가능한 일반 Idle Spawn으로 먼저 등장하지 않았다.
- Story ID `field07_main16_ember_wraith`를 Battle로 전달했다. 기존 `ember_wraith` 정의를 재사용하며 **1체·후열·Magic·비Boss**, 기존 AI/Burn 경로를 유지했다.
- 기존 AI 상태 객체의 자기 행동 순서 `scatter→concentrate→core→basic`를 확인했다. 각 실행의 Victory는 실제 공격 명령/적 Target 버튼과 기존 피해·턴·승리 처리를 통과했다. 새 몬스터 정의나 스킬 수치는 추가하지 않았다. 모든 스킬 VFX·Burn 연출을 전수 시각 검수한 것은 아니다.
- A 실행에서 기존 도망 정책을 호출해 Field07 복귀·8번 목표 유지·일반 불씨망령 잠금·자동 재진입 없음·수동 재도전을 확인했다.
- A 실행의 패배 검증은 아군 HP를 감사 코드로 0으로 만든 뒤 기존 턴 판정/Defeat 경로를 실행했다. Arbel 중앙 `(0,-1.5)`·`Spawn_Arbel_Center` 복귀, 지정 미승리 유지, 수동 재방문·재도전을 확인했다. 적 AI만으로 전멸할 때까지 기다린 검증은 아니다.
- 지정 승리만 8번을 진행했다. 그 뒤 균열/열기/진동 조사와 협곡 목표를 이어 갔다. 균열은 Scene에 남고, 형상만 전투 진입 시 숨긴다.
- 기존 일반 승리 이력에는 `ember_wraith`가 기록되지만 `IsMonsterPet=false`, `IsUnlocked=false`였다. 펫 분양/Beast 해금은 추가하지 않았다.
- 일반 조우 추가 검사: 들개+매는 전열/후열, 도마뱀+딱정벌레는 전열2체, 일반 불씨망령은 후열1체였다. 실제 일반 불씨망령 진입에서 StoryId/StableEncounterId가 빈 값이므로 Main16 지정 승리로 처리하지 않는다.

## World / Save / Navigation

- 실제 Collider 진입으로 Arbel→Field06→Field07→Field06→Arbel을 왕복했다. QA가 플레이어 위치를 출구로 옮겼으며 키보드/게임패드로 전 경로를 걸은 검증은 아니다.
- Field06 `Spawn_From_Field07=(-7,0)`, Field07 `Spawn_From_Field06=(7,0)`; Exit x±10.25, 폭1.1·높이3. Scene 로드 후 Spawn 근처에 도착하며 즉시 역전환하지 않았다. 기존 Arbel 북서 Spawn에서는 기존 환경 Collider가 x를 약0.45 밀어내므로 Spawn 근처 허용 거리0.75로 확인했다. 기존 Arbel 배치는 변경하지 않았다.
- Field07에 새 Safe Zone 없음. 모든 Field07 진입/Continue/전투에서 마지막 안전 거점은 Arbel로 유지했다. 패배는 중앙 Safe Zone Spawn을 사용했다.
- 주요 12목표의 현재 Target Registry와 다른 Scene의 방향 출구 Proxy를 확인했다. 완료 상태에서는 Main16 목표를 다시 진행하지 않았다.
- 네 실행 각각 재 조사 중간·지정 승리 후·협곡 발견 후·Main16 완료 후에 실제 Bootstrap 슬롯1 Continue로 Quest count/Party/Formation/Path/Scene 좌표를 복원했다. A는 도망 뒤 지정 전투 재도전 전 Continue도 확인했다. Save 형식/버전/Branch Flag는 추가하지 않았다.
- 일반 불씨망령은 8번 지정 승리 count 또는 Main16 완료 기록에서 해금한다. 완료 전 첫 등장 보존을 위한 조건이며, 일반 승리는 Story ID를 갖지 않는다. 협곡 발견은 10번 목표 count에서 복원한다.
- Field07은 Bounds21×15, 바닥315개, Boundary5개·동쪽 Opening1곳, 발자국 표식10개·협곡 바위2개다. Preview Scene의 Missing Script0. 동쪽 출구 외 상·하·서 경계는 닫혀 있고 협곡 내부 Transition은 없다.
- 초기 viewport 검사에서 21:9 전환 후 기존 카메라 중심의 보간 때문에 잠시 Bounds 밖이 나오는 오류를 발견했다. `CameraFollow`의 실제 보간 결과도 화면 반경으로 제한하도록 수정했다. 최종 4:3/16:9/21:9 ×4모서리에서 **비율 변경 첫 프레임** viewport 전체 Bounds 포함을 확인했다. OS 창/실제 Game View 크기를 변경한 검증은 아니다.

## 보존 / Git / 남은 확인

- 기존 변경과 직전 Asset 작업을 포함한 SHA-256 기준228개 중226개 동일. 나머지2개는 이번 허용 변경: `BattleSceneController`의 Field07 Encounter 분기9줄, `EditorBuildSettings`의 Scene3줄 추가다. 새 Scene3줄을 제외하면 BuildSettings 원래 내용은 바이트 단위로 같다. 사용자 App UI config는 unstaged로 남겼다.
- 원본 이미지·사용자 Sprite·BGM/Background/Player50/CharacterCreation Asset은 수정하지 않았다. 기존 환경 Sprite의 새 Scene 배치·Tint만 사용했다. 다운로드/이미지 생성/Voice 제작·Import/BGM 신규 배정 없음.
- 최종 Unity 컴파일 오류0, Console Error0/Warning0. 관련 변경 `git diff --check` 통과. 전체 작업 트리에는 기존 사용자 파일의 공백429건 때문에 exit2가 남아 있으며 수정하지 않았다. GitHub push 없음.
- 남은 사용자 시각 확인: 조사 표식/TextMesh의 한국어 가독성·배치, 미세 재 움직임/균열의 붉은 암시/협곡 암시의 미술 품질, 실제 이동·키보드/마우스/게임패드 입력, 모든 화면 비율의 UI/VFX. Main16 Voice와 Field07 BGM은 별도 확정·제작 작업이며 이번 범위의 미완료 기능으로 취급하지 않는다.
- 다음 권장 작업: Main16 시각 QA와 환경 배치 검수 후 필요 수정. Voice는 Branch/Dialogue ID 기반 Manifest부터, Field07 음악은 별도 승인된 배정부터 진행한다. Main17/협곡 내부/Overheat/Boss는 기획 확정 전 구현하지 않는다.
