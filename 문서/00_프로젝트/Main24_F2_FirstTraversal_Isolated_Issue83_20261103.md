# Issue #83 — Main24 F2 첫 안전 왕복 격리 구현 / 2026-11-03

**MAIN24_F2_TRAVERSAL_ISOLATED_PASS / LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_ORIGINAL_APPLIED**

바울 대표 요청에 따라 서릿길 마을 북문 → F2 첫눈의 숲의 짧은 흙길 → 관찰 → 북문 복귀를 실제 Player/Collider로 연결했습니다. 기존 #80 Unity81·정적14, 저장85/72/72, Main23, 그루터기21 전수 재실행0. 새 구간 **Unity PlayMode 90 PASS / 0 FAIL + 정적23 PASS**. 이 결과는 격리 Scene 후보의 안전 왕복이며 Main24 완료나 본편 공식 맵 승인이 아닙니다.

## 현재성·보호 경계

- 원본 local main HEAD `ab47b931fec92976170f9f50aabde1822d78bb90`, status344 항목 전후 문자열 동일. 원격 최신 공통규칙·CODEX_LATEST(main의 과거 스냅샷)·PLANNING_LATEST(docs/planning 최신)와 #80/#82/#83 본문/댓글을 도구로 확인했습니다. 원격 담당 기록과 실제 로컬 선행 HEAD를 구별했습니다.
- 출발 `09c73d2e3383dbe2fe8b9c18bc0ecc4b21adc594`(#82 문서 영수증), 선행 #80 `fd869f60`·기능 `70e363be` 포함. #70/#77/#78 recovery/writer 변경 유입0. `GameSaveService.cs`, ProjectVersion, Packages는 원본main 대비 diff0.
- 브랜치 `codex/main24-f2-first-traversal-20261103`, 기능 commit **`e32d814f7f0dee8622cf386541eab94bf1fc1d69`**. 원본 Unity/Scene/Prefab/BuildSettings/Packages/ProjectVersion·개인 Save/UserData·승인 원본·TTS·Discord66 수정0. 개인 저장 열람/복사/해시0. 원본 Editor/포커스 변경0.
- worktree: `F:/study/codex/Project-Limitless/Temp/Main24F2FirstTraversal20261103/Worktree`.
- 실행 복제본: `F:/study/codex/Project-Limitless/Temp/Main24F2FirstTraversal20261103/QA/Client`. 기존 QA 캐시/추적 자산을 복제했고 UserData/UserSettings 및 이전 QA 저장은 제외했습니다. 새 합성 저장은 이 복제본의 `F2TraversalQA/Saves`만 사용했습니다.
- 실제 실행 Editor **6000.6.5f1 / 3ff58d469c8a**, 새 설치/구버전 실행0. worktree와 원본의 ProjectVersion/Packages는 보존, QA 복제본의 기존6000.6.5f1 메타데이터 재사용. CLI beta.13 업데이트 확인은 CDN DNS 실패·beta.15 알림, 설치/갱신0. 실제 검증은 기존 Editor 숨김 batchmode.

## 재사용·신규 후보

기존 F2 Scene/Portal/Quest의 정식 ID가 없음을 저장소와 #83에서 확인했습니다. #83의 후보 ID 허용 범위에서만 새 Scene을 만들었습니다. 기존 Scene을 복사해 GUID를 중복시키지 않고 빈 Scene에 실제 Player/Camera 구성만 인스턴스로 옮겼습니다.

| 구분 | ID / 구성 |
|---|---|
| 기존 Main24 Quest | `main_24_first_snow_warning` 보존 |
| 기존 목표4 | `hear_skill_guidance`, `hear_chill_preparation`, `approach_north_gate`, `await_f2_opening` 전부 보존 |
| 신규 후보 목표 | `main24_candidate_enter_f2` → `main24_candidate_observe_f2` → `main24_candidate_return_village` |
| 신규 후보 Scene | `World_NorthF2_CANDIDATE` |
| F2 Spawn | `Spawn_CANDIDATE_NorthF2_FromFrostway` (0,-8) |
| 마을 북문 복귀 Spawn | `Spawn_CANDIDATE_Frostway_FromNorthF2` (0,7), 런타임 설치 |
| 표식 | 마을 기존 북문(0,9), F2 남쪽 출구(0,-10), 관찰(3,5) |

기존 QuestService의 ObjectiveId 기반 import를 재사용했습니다. 예전 저장의 네 목표만 있는 합성 fixture를 불러도 앞의 세 진행을 보존하고 F2 진입을 기다립니다. 새 세 목표는 마지막 대기 목표 앞에 삽입했으며 `await_f2_opening`은 여전히 통지하지 않습니다. 반복 출입/관찰은 진행이나 보상을 중복시키지 않습니다.

Main23Site·Main23MouseTravel의 실제 Collider 경로탐색·기존 E/F/마우스·취소 UI·DialoguePresenter·SceneTransitionService·SceneSpawnPoint·WorldPositionSaveController를 그대로 재사용했습니다. 북문은 안내 후 명시 선택 이동, 취소 가능. F2 관찰 대화의 Esc 취소는 진행0, 재진입 후 끝까지 들으면 임시 탐험 진행만 갱신합니다. 보급 안내의 오래된 '숲 진입 불가' 문구만 현재 구간에 맞췄습니다. 구매/화폐/소유권 로직 추가0.

## 실제 Scene과 승인 그루터기

F2 후보는 32×24WU Bounds와 외곽 Collider, 폭4.5WU의 연결된 흙길/굽이, 남쪽 귀환 표식·북동쪽 관찰 지점으로 구성했습니다. 출입은 Trigger 자동 전환이 아닌 명시 선택 방식이므로 외곽 Collider를 임의로 뚫지 않았습니다. Spawn과 출구 표식은 각각2WU 분리되어 즉시 재전환하지 않습니다. 실제 Player가 통행하며 CameraFollow가 세 viewport 전체를 Bounds 안에 유지합니다.

눈바닥·흙길·나무3PNG는 **PLACEHOLDER_*_ValidationOnly**, 정식 F2 미술 아님. HUD에 **임시 환경 검증**을 상시 표시하고 이름/방향/거리로 안내합니다. 미승인 Sign/Fence FULLREDRAW·서리올빼미·Spider/Bee는 import/배치0. 전투·MonsterSpawn0.

승인 `F2_SnowStumpPerch_NaturalBroken_v4_CANDIDATE_320x384_RGBA.png` SHA **`22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8`** 바이트 그대로 격리 등록했습니다. **PPU128 / Pivot(0.5,11/384) / Scale1 / Point / Uncompressed / FullRect**, 배치(-0.3,1). 가시 알파 최하단이 y1 접지선에 놓입니다. 720높이에서 가시 높이 약130.5px, 캔버스2.5×3WU. 실제 캐릭터/바닥과의 표시를 캡처로 확인했습니다.

줄기 발 부분 BoxCollider(1.65×0.4WU, offset y0.2)와 실제 Player 충돌을 검증했습니다. 휠체어가 줄기를 관통하지 않고 오른쪽 길로 우회했으며 도보·휠체어 모두 왕복 경로를 통과했습니다. 그루터기 전용 발 위치 Sorting만 추가해 실제 Player 뒤/앞 렌더 순서와 캡처가 바뀝니다. 원본 Player Sorting은 수정하지 않았습니다. 주 통로의 목표나 플레이어는 가리지 않습니다. 후보 배치/크기가 최종 맵 미술 사양으로 자동 확정되는 것은 아닙니다.

## 새 Delta QA

| 검증 | 실제 결과 |
|---|---|
| 기존 #80 저장 후보 복원 | 앞3목표 + 마지막 대기ID 보존, 신규 F2 진입부터 대기 |
| 북문 E / Esc / 재선택 | 실제 선택창·취소 진행0·F2 실제 로드 |
| 휠체어 이동 | 마을→북문→F2 관찰→남쪽 출구→마을, 실제 Rigidbody2D/Collider 경로 통과 |
| 도보 이동 | F2 재진입·관찰·귀환, 실제 키보드 W 이동 + Collider 경로 통과 |
| 관찰 F / Esc / 마우스 | 취소 진행0, 마우스 재대화 후 임시 진행, 반복 완료/보상0 |
| 그루터기 | PPU/Pivot/Scale, 실제 줄기충돌·우회, Player 앞뒤 Sorting 확인 |
| 세 화면비 | 1280×720 / 960×720 / 1680×720 실제 Camera+Canvas RT, Bounds/목표/귀환 안내 표시 |
| 합성 Save/Continue | F2 관찰 후 실제 Scene·좌표·목표 복원, 마을 귀환 후 왕복기록 보존 |
| 기존 연결 보호 | 마을 남문→F1→마을 연결부만 좁게 확인, 기존 Main23 완료·안전거점 보존 |
| 6슬롯/경제 | 6번째 슬롯에 기본스킬을 넣은 fixture 유지, 소유권·잔액77 동일 |
| 진행 제한 | Main24 Active/마지막 대기 목표, Main24 완료·보상/Main25 연결 없음 |

[90개 실제 체크](Evidence/Main24F2FirstTraversal20261103/checks.txt), [정적23](Evidence/Main24F2FirstTraversal20261103/StaticDeltaQA.json), [실제 물리 위치 trace](Evidence/Main24F2FirstTraversal20261103/physics_trace.csv), [23개 변경파일 SHA](Evidence/Main24F2FirstTraversal20261103/ChangedFileSHA256.json), [캡처 SHA](Evidence/Main24F2FirstTraversal20261103/CaptureSHA256.json), [원본 보호 요약](Evidence/Main24F2FirstTraversal20261103/ProtectionSummary.json).

최종 compile 오류0·시험 실패0·게임 Runtime 오류0. 기존 UnityEditor.Search.SearchDatabase의 시작 시 `ArgumentOutOfRangeException` **1건은 별도 로그로 보존**했으며 PASS에서 숨기지 않고 Editor 환경 기존 OPEN으로 분리했습니다. Licensing access-token 갱신 경고 뒤 entitlement 확인 성공, batch 실행/RT 생성은 정상 종료했습니다. 기존 코드·저장 전수검증을 재실행하지 않았습니다. 납품 소스와 시험 소스는 동일 실행 내용이며 주석/생성 YAML 빈 값 뒤 공백만 정리했습니다.

## 증거 열람·재현·미검증

- 단일 **OFFLINE 증거 열람판**: `F:/study/codex/Project-Limitless/Temp/Main24F2FirstTraversal20261103/Review/index.html` (실제 Unity 캡처6장 내장).
- 실제 Unity PNG: `F:/study/codex/Project-Limitless/Temp/Main24F2FirstTraversal20261103/QA/Client/F2TraversalQA/Playthrough/` 및 본 문서 Evidence 경로.
- 캡처는 **실제 Unity PlayMode Camera RenderTexture**이며 OFFLINE 합성/프록시 시험이 아닙니다. HTML은 OFFLINE 파일 열람용입니다. **OS GameView/사람의 손플레이/물리 OS 키보드·마우스 입력은 NOT_VERIFIED**. 입력은 Unity InputSystem 가상 장치로 검증했습니다. 사용자 포커스를 바꾸지 않았습니다.
- 재현 제작기 `Assets/_Project/Scripts/Editor/Main24F2ContentBuilder.cs`의 `Build`, 신규 QA `Main24F2TraversalQA.Start`를 **명시된 QA 복제 경로에서만** 숨김 batch 실행합니다. QA 실행은 자체 종료하므로 `-quit` 없이 사용합니다. 구버전 설치/기존 전수QA 실행이 필요하지 않습니다.
- **BuildSettings F2 연결은 QA 복제본에만 추가**했습니다. worktree/원본 BuildSettings는 바꾸지 않았습니다. 소스 브랜치 자체가 원본 게임에 자동 적용·진입 가능한 상태라고 주장하지 않습니다. 향후 명시 적용 승인 시 후보 Scene을 검토하고 대상 프로젝트의 Scene 목록 연결을 별도 최소 Delta로 처리해야 합니다.

## 다음 최소 결정 (본편 적용 전)

1. 바울 대표의 격리 F2 플레이/표시 검토, 임시 환경을 정식 F2 자산으로 대체할 범위 확정. 미승인 Sign/Fence는 계속 KEEP_CANDIDATE/승인대기.
2. Owl Attack 연결 품질·전투 스프라이트 승인과 첫 한기 전투 기획·무료 해제 수단/재공급 정책 확정. 현재 전투·아이템 지급0.
3. 상품25 이름·가격·효과·ID/소유권 정책 확정 전 구매0. Main24 완료·보상·Main25는 별도 승인과 후속 구현 필요.
4. 최신 기획의 Spider v5 최종 원화 승인은 확인했으나 신규 모션/제품 교체와 별개입니다. Bee 모션·표적/회수 연결 및 CC BY3/4 RIGHTS_HOLD 지속. 이번 F2 코드에서 양쪽 사용0.

**현재 결과: 실제 격리 F2 왕복 PASS / USER_PLAY_REVIEW_PENDING / NOT_ORIGINAL_APPLIED / REMOTE_NOT_PUSHED.**


GitHub 보고 영수증: 기능 `e32d814f7f0dee8622cf386541eab94bf1fc1d69`, QA/보고 `8cfd6d0cbffdd8fcdfac4501d6824d9f5d113b05` 기준 [Issue #83](https://github.com/HappyPaul812/Project-Limitless/issues/83#issuecomment-6594041936) 실제 상세 댓글 및 [#80](https://github.com/HappyPaul812/Project-Limitless/issues/80#issuecomment-6594047044) · [#82](https://github.com/HappyPaul812/Project-Limitless/issues/82#issuecomment-6594047914) 연결 댓글 등록 성공. 원격에는 댓글만 게시, 파일 Push/PR/원본 적용0. 본 후속 커밋은 보고 영수증만 추가합니다.
