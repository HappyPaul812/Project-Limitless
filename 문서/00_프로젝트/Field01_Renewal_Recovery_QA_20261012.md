# Field_01 리뉴얼 복구 및 최종 QA — 2026-10-12

## 판정

**환경 통합 완료 / 기능 검사 PASS / 전체 QA PARTIAL.** 사용자 미술 승인 v1을 유지한다. 기존 Unity Search 내부 예외 2건은 해결하지 않았다. 마지막 격리 실행에도 SearchDatabase 예외 1건이 발생했다. 실제 일반 전투 전체 조작과 Screen Space 이름표 가독성은 NOT_VERIFIED다.

## A. 복구 결과

- 시작 HEAD `40f9517fde2bc89db9b2e09fefc93a0a020048e3`: 환경12종·전용meta는 이미 등록/Commit 완료. 다시 추출·생성하지 않았다.
- 중단 당시 사용자 미커밋335항목을 보존. 재개 시337항목의 추가2개는 Field_01 Scene과 Generator뿐이며 제거된 항목0.
- `Temp/Field01RenewalQA/`의 기존 격리 Client, scene_before, generator_before, mapping, Importer/보호/정적 증거와 qa.log/attempt2/attempt3를 보존·재사용했다.
- 기존 Importer12·RenderTexture·Rock B·8개 일반 전투 진입/승리 복귀는 로그의 PASS와 대조했다. attempt2 전체 FAIL, attempt3 SceneGuard FAIL을 그대로 유지했다.
- 기존 QA는 종료 상태였다. 샌드박스 프로세스 조회가 원본 Editor를 숨겨 권한 있는 읽기 조회로 보완했다. 원본 PID31832 유지, 새 격리 QA만 Hidden Batch로 실행 후 정상 종료. 원본 Scene/Play/종료/화면 포커스 조작0.
- 실행 전 여유 F:약533GiB/C:약36GiB. 기존 복사본 하나를 재사용했고 추가 프로젝트 복사본은 만들지 않았다. 실패별 로그와 테스트 Save는 별도 경로에 보존했다.

## B. Field_01 환경

- PNG12종은 원본 ZIP과 바이트 단위 일치. 기존meta와 GUID 변경0, 신규 GUID 충돌0.
- Scene 환경363개/12종 모두 전용 v1 참조. 초원315·길30·나무5·덤불3·바위2·울타리8.
- Scene 복구 전 baseline=HEAD; 전체 Diff는 m_Sprite 363줄뿐. 위치/Scale/flipX/Sorting/Collider/Prefab override·Spawn·Bounds·Exit 변경0. Scene 재생성0.
- Generator는 12역할을 전용 v1 로더로 대응하며 미등록/누락은 예외로 보고한다. Generator 실행0; 로더는 실제 AssetDatabase로 검사했다.
- 실제 Importer Single/PPU128/Pivot중앙/Point/Mipmap off/무압축/AlphaTransparency 및 GUID·fileID21300000 PASS. Missing Sprite/Script0.
- RockNorth 위치(4.5,4.4), Collider size(0.4,0.22)/offset(0,-0.22) 유지. 128px/PPU128/Pivot64와 실제 RenderTexture 및 Collider 점검 PASS. 가시 bbox110×65px로 기존 검수의 크기 예외를 확인했다. 접지·배치 조정0.
- 잔디 반전 상하 경계의 엄밀 픽셀 불일치는 기존 승인 미술의 알려진 특성이다. 길 계단형 변곡/울타리 접합은 실제 이미지에서 투명 틈·단절 없이 확인. PNG/flipX 수정0.
- PlayerPrefab의 사용자 Diff는5개 빈 scalar 후행공백뿐, HEAD와 의미 동일이며 Scene override 의존성의 HEAD 누락0. 관련 Prefab·사용자 파일은 Commit 제외.

## C. 몬스터 리스폰

**실패 원인은 QA 계측 오류. 게임의 리스폰 버그는 재현되지 않았다.**

attempt2의 respawn_state는 `Scene=World_StarterVillage; count=0`이며8개 Spawn 모두 available=True다. attempt3도 대기 전 이미Field_01이 아닌 Scene에서 실패했다. Field_01 몬스터를 마을에서 집계한 것이 직접 원인이다. QA의 자동 이동/복귀 위치를 통제하지 않은 이전 실행의 정확한 Scene 이탈 경로는 로그만으로 단정하지 않는다.

MarkDefeated는 **VictoryReturn의 ReturnToField(true)**에서 접촉 Spawn 하나에 `Time.realtimeSinceStartup + RespawnSeconds`를 기록한다. EndBattle 순간이 아니다. 기본30초/ID/정의/배치 데이터는 변경하지 않았다. timeScale/이동 잠금과 무관한 실제 경과시간이며, FieldMonsterSpawnRuntime.Update가 현재 Scene에서 누락된 instance를 생성한다. Scene 부재 동안 타이머는 계속 지나고 재진입시 Configure에서 가용 Spawn을 생성한다. 플레이어 거리는 리스폰 허가 조건이 아니며 입구 안전지대는 생성 중심만 조정한다. Update가 실행되지 않는 상태에서는 실제 오브젝트 생성이 다음 Update까지 늦을 수 있다.

QA만 수정: 격리 Player 입력 비활성·중앙 배치, Scene guard, Field_01 Scene 소속별 집계, Spawn ID 중복/조기생성 검사, 실제 전투승리로 기록된8개 deadline 관측. 별도 MarkDefeated 재호출이나31/35초 우회 대기를 하지 않았다. 5초 상한은 타이머 변경이 아니라 검사 종료 watchdog이며 실제 결과는30초 직후다.

8/8은각30초 만료 후1.6~2.2ms 다음 Runtime 갱신에 생성. 누락/중복/조기생성0. [원시 타이밍](Evidence/Field01RenewalRecovery_20261012/respawn_timing.tsv) 열: Spawn ID / 처치 기록 시각 / 원래 만료 / 최초 실제 관측 / 만료 대비 지연(초). 회귀용 게임 C# 수정0.

## D. 실제 Runtime QA 범위

| 검사 | 결과 및 한계 |
|---|---|
| 환경12종·참조363/Importer/GUID/누락 | PASS, 정적+AssetDatabase+Runtime |
| Rock B·Bounds·기준 이동 지점 | PASS, 좌표/Collider와 실제 Direct3D12 RenderTexture |
| 초원슬라임5·독침벌3, 정확 리스폰 | PASS, 실제8개 조우 Scene 진입/승리 반환 및 원래30초 |
| Main02 | PASS, 조사 Trigger/전투승리 진행/흔적 상호작용/Continue |
| Main03·태온 | PASS, 첫 대화/2인 Story 진입·승리복귀/후속 대화/단서 대화 종료 |
| Main04·미엘 | PASS, 등장/첫 대화/3인 Story 진입·승리복귀/후속 대화 |
| Main05 | PASS, 마을 진입/의뢰선택창 기존이야기 버튼/경비·대표 보고/동료해금 |
| Main06 | PASS, Field02 진입/흔적 상호작용/완료 |
| 양방향 출입구 | PASS, 실제 Trigger handler·Scene 로드·Spawn좌표. 전체 보행 경로 조작은 아님 |
| Save/Continue | PASS, 실제 Bootstrap 슬롯버튼/월드좌표·Quest JSON 일치; 테스트Save만 사용 |
| 동료·파티 보호 | PASS, Story2/3인 및최종 Save의 태온·미엘 해금/편성/자원. 전체 시스템 회귀는 NOT_VERIFIED |
| Quest Marker | PASS, 태온·미엘 WorldSpace 목표·E안내 실물 확인 |
| 이름표 가독성 | PARTIAL, WorldSpace 여행자 표시 확인; ScreenSpace Overlay 플레이어/몬스터/NPC 이름표는 Camera RT에 포함되지 않아 NOT_VERIFIED |
| 전투 전체 입력/스킬/난이도 | NOT_VERIFIED. QA는 실제 BattleSceneController의 EndBattle 승리경로를 호출하므로 정상 공격으로 적전멸한 증거가 아님 |
| Compile/신규 Runtime 오류 | Compile 오류0, 마지막 실행 신규Runtime 오류0 |
| Console/전체 QA | FAIL/PARTIAL, 기존 SearchDatabase 예외. 기존2건 미해결 유지 |

Main03 초기 실패는 최종 단서의 대화 종료 콜백을 기다리지 않은 QA 오류였다. Main05 초기 실패는 NPC 의뢰 선택창과 동일 프레임 클릭 방지 규칙을 QA가 처리하지 않은 오류였다. 각각 QA만 보완하고 이미 통과한 구간은 테스트 Save 체크포인트부터 재개했다. 원본 Save 수정0. 이전 FAIL 증거를 삭제하거나 PASS로 바꾸지 않았다.

카메라를 QA에서 확대/이동한 RenderTexture는 환경·Marker 검사 증거다. 실제 플레이 카메라 전체 viewport/ScreenSpace UI 캡처나 게임패드·키보드 수동 플레이 완료를 뜻하지 않는다. 미엘/여행자 주변은 기존 Actor와 나무가 시각적으로 겹쳐 전체 이름표 및 조작 화면의 확인을 남긴다. 배치 디자인 변경0.

## 증거

- [종합 QA 기록](Evidence/Field01RenewalRecovery_20261012/qa_rollup.json), [정적 복구](Evidence/Field01RenewalRecovery_20261012/static_resume_20261012.json), [원본 보호](Evidence/Field01RenewalRecovery_20261012/preservation_resume_20261012.json)
- [리스폰/초기 구간](Evidence/Field01RenewalRecovery_20261012/respawn_checks.txt), [Main03/04](Evidence/Field01RenewalRecovery_20261012/main03_main04_checks.txt), [Main05/06 최종](Evidence/Field01RenewalRecovery_20261012/main05_main06_checks.txt)
- [필드 실제 화면](Evidence/Field01RenewalRecovery_20261012/field_runtime_overview.png), [Rock B](Evidence/Field01RenewalRecovery_20261012/rock_b_runtime.png), [길](Evidence/Field01RenewalRecovery_20261012/winding_road_runtime.png), [태온](Evidence/Field01RenewalRecovery_20261012/taeon_first_runtime.png), [미엘](Evidence/Field01RenewalRecovery_20261012/miel_first_runtime.png)
- Evidence 텍스트 사본은 Git 검사에 맞춰 줄끝공백/개행만 정리했으며 원시 로그는 그대로 보존했다. Evidence SHA-256은 Git blob의 LF 개행 기준이며 Windows 작업복사본의 CRLF는 정규화하여 비교한다. 실행 QA 소스는 Evidence의 `*.cs.txt`에 보관하고 원본 Assets에 추가하지 않았다. 원시 로그는 Temp의 qa_resume/remaining/final/final2_20261012.log와 기존 attempt2/3에 유지. 실패한 실행은 전체 FAIL로 기록하고 그 실행의 개별 PASS만 재사용했다.
- 최종 단독 잔여 실행89개 assertion PASS, Search1건으로 QA_PARTIAL_KNOWN_SEARCH. 앞선 실행의 구간 PASS를 합쳐 필수 기능검사를 판정했다.

## E. 보호·Git 및 다음 작업

재개시 원본 Assets/Packages/ProjectSettings/UserData3564파일 해시 대비 변경0·삭제0. 기존3536 보호기록 대비 변화는 중단작업의 Scene/Generator2개뿐. 환경 PNG/meta와 기존Save/ThirdParty 불변.

선별 Commit 대상: Scene Sprite363 교체, Generator 전용매핑, 본 QA보고/Evidence와 CURRENT_STATUS의 이번 요약만. 환경12종은기존 `40f9517`에 있으므로 중복 등록하지 않는다. 기존 사용자335항목 및 CURRENT_STATUS의 별도 MCP기록은 Working Tree에 유지한다. git add -A/reset/clean/checkout/Scene재생성/Save삭제/Push0.

다음 권장 작업: 기존 Search 내부 예외를 별도 범위로 진단하고, 사용자 편한 시점에 정상 전투 입력/전체 이름표/미엘 주변 가독성을 확인한다. 화면조작 허락을 요청하거나 원본 Editor를 전환하지 않았다. GitHub Push는 사용자 직접 수행.

선별 기능 통합 Commit: `37e4e3dc0573dc19d1ce142d9b87c77cb90cf6e9`. CURRENT_STATUS는 이 기능 SHA를 기록하는 별도 Docs Commit으로 갱신하며 기존 사용자 MCP 기록은 스테이징에서 제외한다.
