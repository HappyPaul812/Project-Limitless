# 승인 Wolf Run6 v5 제품 경로 반영 및 Delta QA

## 승인과 범위

바울 대표의 최종 승인 문구: **“늑대 괜찮아.. 승인..”**. 미술·모션 승인은 완료 상태이며 다시 요청하지 않습니다. 본 작업은 승인 파일의 제품 경로 연결과 변경분 검증입니다. 원본 local main 디스크 적용, 원격 Push/PR/main 병합과 구분합니다.

- 작업 브랜치: `codex/wolf-run6-v5-apply-20261021`
- 기준 커밋: `e4e9891f6f01ea95cada9e63b1f09946d94d6041`
- 실제 ZIP SHA256: `16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351` — 승인 기준과 일치.
- ZIP CRC 오류 0, Manifest SHA 31/31 일치. 승인 Strip의 각 셀과 6개 PNG 원본의 RGBA 픽셀 일치 6/6.
- 제품 Strip SHA256: `96f09da273c0e438b37ede80a6673c797b7623ed78b8e6e93de77eddb6aa649c`.

## 최신 정본 확인

GitHub `main` 및 `codex/eod-closeout-20261020`의 `팀운영/인계/CODEX_LATEST.md`에서 EOD 보호·동기화 기록을 확인했습니다. `docs/codex`라는 원격 브랜치는 없어 사용하지 않았습니다. `docs/planning`의 `PLANNING_LATEST.md` blob `2ffcf02574872a015d5781baeec6a8e9ed9e06e7` 상단에서 Wolf Run6 v5 `USER_ART_MOTION_APPROVED`를 확인했습니다. Issue #43을 읽었으며 최신 승인 상태는 사용자 직접 지시와 기획 인계 상단을 적용했습니다. 마을 HOLD는 별개입니다.

## 반영 방식 및 제품 변경 6파일

| 파일 (`Unity/Client/Assets/_Project/` 기준) | 변경 |
|---|---|
| `Resources/CompanionAssaultValidation/Wolf_Run6_v5.png` | 승인 ZIP Strip 바이트 그대로 신규 추가 |
| `Resources/CompanionAssaultValidation/Wolf_Run6_v5.png.meta` | 새 GUID, maxTextureSize 4096, RGBA 무압축·Readable·Point 설정 |
| `Scripts/Battle/BeastCompanionSystem.cs` | Wolf Run만 새 경로/512셀/12 FPS/0.25배 연결. 속도570 유지. 아이콘 경로·셀 규격을 별도로 보존 |
| `Scripts/Battle/BattleUiIconCatalog.cs` | 기존 아이콘 원본 경로·64px셀 사용 유지 |
| `Scripts/UI/PetFacilityPresenter.cs` | 기존 시설 아이콘 원본 경로·64px셀 사용 유지 |
| `Scripts/Battle/BattleActionPresenter.cs` | 승인 시트 규격에 맞게 설명 주석만 갱신; 실행 로직 불변 |

기존 `Wolf_Run.png`는 공용 아이콘과 과거 비교 Scene에도 쓰입니다. 이를 덮어쓰면 아이콘까지 바뀌므로 신규 Run 자산을 추가했습니다. 모든 기존 PNG/meta/GUID·정적 KeyArt·Idle·다른 애니메이션·Scene·Animator·Controller는 불변입니다. 새로운 시트를 별도 GUID로 추가해 기존 참조를 유지합니다. 아이콘 선택 index4 및 원래 RGBA도 그대로입니다. Bear/Fox는 선택적 아이콘 인수가 생략되므로 기존 Run 경로·셀 규격을 계속 사용하며 해당 정의 줄은 변경하지 않았습니다.

새 시트는 3072×384, 각 셀512×384입니다. 기존 Importer 한도2048을 그대로 쓰면 축소되므로 새 자산에만4096을 설정했습니다. 표시 영역은128×96 UI 단위이며 비율을 유지합니다. 승인 그림의 픽셀·포즈·색·알파를 다시 만들거나 보정하지 않았습니다.

## 실제 제품 연결과 검증 경계

현재 Wolf companion은 HP/독립 턴/필드 추종 GameObject가 없는 **전투 스킬 연출**입니다. 실제 호출은 `PlayBeastCompanionAssault`→`Resources.Load<Texture2D>`→`SliceBeastRunSheet`→기존12 FPS Coroutine과570 UI단위/초 이동입니다. 필드 follow나 Animator 연동을 새로 만들지 않았습니다. 실전 Battle 입력 전체 회귀는 범위 밖이며, 제품 Presenter·Catalog를 직접 호출하는 격리 Play Mode로 연결·이동·방향·종료를 확인합니다.

격리 프로젝트 `Temp/WolfRunV5Apply20261021/UnityQA`는 제품 복사본입니다. Unity6000.6.5f1을 숨김 Batch Mode로 실행하며 원본 Editor 조작/포커스 전환0입니다. QA 전용 Scene과 검수기는 제품 브랜치 Unity 경로에 포함하지 않습니다. Save/설정 경로는 scratch Runtime 하위로 감사 리디렉션합니다.

### 실행 결과 — 변경분 PASS

Unity6000.6.5f1 / Direct3D12 / 실제 Play Mode / 종료코드0. 전체 제품 C# 컴파일 오류0. 검수 체크 **106 PASS / 0 FAIL**, 실제 Sprite 교체 시점43개 및 엔진 프레임1→452 기록. 3회 모두 00→01→02→03→04→05→00을 실제로 관측했습니다.

- 제품 `Resources.Load` 결과3072×384, 런타임 Sprite6장 각각512×384, 표시128×96 확인. Unity에 전달된 제품6파일의 SHA가 작업 브랜치와 동일합니다.
- 왼쪽 돌진/오른쪽 돌진/출발·대상의 x·y를 바꾼 재실행3회. 각 프레임의 방향 반전·표시 크기, 실제 위치 진행, 시전자 위치 유지, 타격·알림·종료 각각1회 및 임시 Wolf 제거를 확인했습니다.
- 기존12f 설정과 `WaitForSeconds(1/12)` 재생 로직 불변. 이번 실행의 관측 프레임 간격은0.083895~0.090873초입니다. 실제 스케줄러·캡처 부하에 따른 양자화가 있으므로 정확한500ms 루프라고 주장하지 않습니다. 프레임 누락·순서 역전은 없습니다.
- 실제 렌더를 원본 승인6포즈와 대조했습니다. 발/발목·꼬리의 모양과 승인된 프레임별 자세를 유지하고 새 사각형·누락 Sprite·잘림을 발견하지 않았습니다. QA 장면에는 필드 지면/물리 발 접촉 시스템이 없으며 실전 Battle 입력·필드 수동 추종 검증으로 확대하지 않았습니다.
- 사진은 실제 Unity Camera/RenderTexture 렌더입니다. Editor GameView UI 화면 캡처 또는 OS 수동 플레이와 구분합니다. GIF는 실제 관측된 두 번째0~5 전체 사이클을 잘라 구성했고, 실제 관측 간격을10ms 단위로 반영했습니다.

[실제 제품 Presenter 좌향128px GIF](Evidence/WolfRunV5Apply20261021/ActualPresenter_Trial0_128px.gif) · [우향 GIF](Evidence/WolfRunV5Apply20261021/ActualPresenter_Trial1_128px.gif) · [위치 변경 GIF](Evidence/WolfRunV5Apply20261021/ActualPresenter_Trial2_128px.gif) · [실행기록](Evidence/WolfRunV5Apply20261021/RuntimeQA.json).

기존 Editor `UnityEditor.Search.SearchDatabase` 시작 시 `ArgumentOutOfRangeException`1건을 수집했습니다. 지난 격리 QA에서도 기록된 환경 OPEN이며 제품 NullReference/MissingReference/컴파일 오류0과 구분합니다. Console 전체0으로 선언하지 않습니다.

첫 scratch 실행은 패키지 갱신 단계에서 진행되지 않아 해당 QA PID만 종료했습니다. 실행 중 Unity 없음 확인 후 scratch의 잔여 Lockfile만 제거하고 재실행했습니다. Unity가 scratch에서 일부 패키지(ugui2.5→2.6 등)를 자동 갱신했으며 **제품 worktree 및 원본 Packages/ProjectSettings는 수정0**입니다. 제품이 선언한 모든 패키지 버전을 고정한 별도 환경 재검증은 하지 않았습니다. CLI 갱신 확인은 DNS 오류, Pipeline 연결 인스턴스 조회는0으로 돌아와 원본 Editor 연결을 가정하거나 설치/Refresh하지 않았습니다.

제품 구현 커밋: `eb0e932f0914727ce7205a8374e15122b2fcb0d7`. 문서·증거는 후속 Docs 커밋에 기록합니다.

## 보호 및 다음 단계

Main22 136/F1 64/서리들쥐40 전체 회귀 재실행0. Bear/Fox/독사/마을/Main23 구현0. 원본4351파일 전후 SHA 변경0·추가0, root HEAD/status bytes 동일을 확인했습니다. 원본 Save/UserData/Editor 조작0. 기존 Wolf PNG 바이트 동일, 기존 meta는 Git 내용·GUID 동일(원본/새 checkout의 CRLF 차이는 자산 변경과 구분)이며 원본 물리 파일 SHA도 전후 불변입니다. Push/PR/main merge0. 미술 재승인은 필요하지 않습니다. 다음 단계는 바울 대표의 별도 Push/PR 및 통합 지시입니다. 원본 디스크 동기화와 원본 Editor 자연 Import·손플레이는 그 단계에서 별도로 구분합니다.

근거: [Evidence](Evidence/WolfRunV5Apply20261021/), [기획 승인 인계](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/팀운영/인계/PLANNING_LATEST.md), [Issue43](https://github.com/HappyPaul812/Project-Limitless/issues/43).
