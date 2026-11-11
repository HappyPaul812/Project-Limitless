# Issue75 — 승인 B352 본편 연결·Delta QA / 2026-11-11

**F2_B_LOCAL_ORIGINAL_APPLIED / UNITY_DELTA_QA_PASS / USER_REALPLAY_PENDING / REMOTE_NOT_PUSHED**

원본 로컬 main에 **da34c933dbc889b9926f74a43c08fad2ca260a45**까지 FF-only/no-autostash 반영했습니다. 원본 Unity의 백그라운드 AssetDatabase 갱신·컴파일 후 Builder 타입과 신규 두 GUID, FirstSnow PPU128/중앙Pivot을 확인했고 Console 오류0입니다. 기존 Scene/dirtyfalse/PlayOFF/selectionnull을 유지했습니다. 보호5130개 중 승인 Scene1개만 변경·나머지5129 SHA 동일, 사용자 dirty302/untracked550 status byte-exact 동일, 제품5파일 Git blob 일치입니다. 이 완료 영수증 문서도 작업브랜치에서 commit 후 FF-only로 반영하며, 최종 문서 포함 main SHA는 Issue75 결과댓글과 원본 Temp/F2B352Apply20261111/Final.json에서 확정합니다.

## 승인과 제품 범위

바울 대표의 2026-11-11 채팅 **“어. 승인.”**으로 Builder B352의 원본 적용 확대 범위 최종 승인을 받았습니다. [기획 최신 검수](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6783034852)는 별도 사용자 승인이 필요하다고 명시했고 이번 채팅 승인이 그 조건을 충족합니다. 검증 기반은 `23ea5e58e4532c9e4b49b3a872d6700662e27a07`입니다.

브랜치 `codex/f2-b352-original-apply-20261111`, 제품 commit **c951e0c533ac5e1a12090ea446fb05c8fd91fd61**. 별도 worktree `Temp/F2B352Apply20261111/Worktree`, QA 복제본 `Temp/F2B352Apply20261111/QA/Client`, Unity **6000.6.5f1**만 사용했습니다. 제품 commit은 아래 **정확히5파일**입니다. 보고·검사 도구·증거는 별도 문서 commit으로 관리합니다.

| 제품 파일 | 변경 |
|---|---|
| Assets/_Project/Scripts/World/F2SnowPathBuilder.cs | 승인 파일럿 그대로 신규 |
| 위 .cs.meta | 신규 GUID `6dc8e6f8a6804021aa21cf12e31105f3` |
| Assets/_Project/Resources/F2_FirstSnow_GroundOverlay_v1_CANDIDATE.png | 기존 승인 PNG 그대로 신규 등록 |
| 위 .png.meta | 신규 GUID `1f2b49e1abdb4374b2376945c6778c10` |
| Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity | Root·Builder 연결만 추가 |

FirstSnow SHA256 **61141ace90d5c64c2277da0cb7e27d9674bce6c05f276fb3b9801cd05ffa272a**, 기존 F:/Downloads/Limitless_Sprites/F2_art_74.zip 내부와 동일합니다. 새 그림 제작0. Ground9/WornTrail/나무/Stump/Owl/Collider/HUD의 기존 PNG·코드·meta·GUID를 교체하지 않습니다. Pine8/Shrub4/C SnowEdge/F2ReuseHudLayout은 제품에 포함하지 않습니다.

## Scene 연결과 불변 범위

격리 Unity Editor API로 빈 `F2_ApprovedSnowPath` Root1개와 F2SnowPathBuilder1개를 추가했습니다. Ground9(Earth3→ThinFrost3→Gravel3), FirstSnow1, 기존 NorthF1 WornTrail1을 직렬화했습니다. Runtime에서 설치 코드를 따로 호출하지 않고 **Scene의 Start가 자동 생성**합니다.

Scene 저장 GO22→23, Runtime 추가 Root포함352(자식351), Sprite348/Mesh3/Material3. 기존 눈바닥1·흙길3만 실행 중 OFF이며 Scene 원문에서는 기존 enabled·Transform·Sprite 참조를 유지합니다.

- PLACEHOLDER_SnowGround_ValidationOnly (0,0)
- PLACEHOLDER_FirmTrail_South (0,-6)
- PLACEHOLDER_FirmTrail_North (3,4)
- PLACEHOLDER_FirmTrail_Bend (1.5,0)

Unity 저장 전후 기존 컴포넌트 JSON 동일을 확인했습니다. 저장 시 자동 추가된 공백만 정리한 최종 Scene은 **기존 YAML 블록 모두 동일**, SceneRoots 목록1줄과 새 GameObject/Transform/MonoBehaviour3블록만 추가했습니다. Scene diff는 **57삽입/삭제0**입니다. 기존 Scene/Prefab 참조·Scene meta/GUID·Collider14개는 그대로이며 새 Collider는0개입니다.

## 새 최소 Delta QA — 37PASS/0FAIL

**Runtime35PASS + Editor Scene 작성2PASS**, 기존 파일럿33이나 과거24/68/90/81을 재합산·전수 재실행하지 않았습니다. 체크 원문·이동 trace·실제 화면은 Evidence/F2B352Apply20261111에 있습니다.

1. 실제 SceneTransitionService로 마을→F2 입장, 자동 Start352 및 승인 눈14, Builder1개 PASS. 수동 Build 설치로 자동 실행을 대신하지 않았습니다.
2. 이미 생성된 Build 재호출 시 개체·Mesh가 늘지 않음 PASS.
3. 실제1280/960/1680 GameView 해상도·기존 Overlay HUD의 화면 안 배치·Camera RT 대체 없음 PASS.
4. 휠체어 실제 PlayerController/Rigidbody2D 굽은 길 왕복1회: (0,-3)→(0,0)→(3,0)→(3,3)→(3,0)→(0,0)→(0,-3). 준비 외 순간이동 판정0, 기존 Collider/나무/Stump/Owl 참조 보존 PASS.
5. 마을 복귀 후 소유 Mesh/Material 모두 Unity null로 해제·Builder 제거·잔존 결합Mesh0 PASS.
6. F2 재입장 시352개만 재생성·결합Mesh3만 존재·4개OFF PASS. 두 번째 퇴장 후 자원 해제 PASS.
7. 최종 C# 컴파일 오류0, 게임 Runtime Error0. 기존 MCP startup assembly-reference EndOfStreamException과 SearchDatabase 예외는 격리 Editor 환경 OPEN으로 분리합니다. 전체 Editor 오류0·장시간 메모리 soak 완료를 주장하지 않습니다.

Save/UserSettings는 격리 Audit 경로를 사용했습니다. 원본 Save는 복제·백업·SHA 읽기만 했으며 실제 슬롯 데이터를 QA 입력으로 바꾸거나 덮어쓰지 않았습니다. 사람의 물리 입력·실플레이 수용은 아래 사용자 확인 단계입니다.

## 실제 화면 비교

- [1280 실제 GameView](Evidence/F2B352Apply20261111/Applied_GameView_1280x720.png)
- [960 실제 GameView](Evidence/F2B352Apply20261111/Applied_GameView_960x720.png)
- [1680 실제 GameView](Evidence/F2B352Apply20261111/Applied_GameView_1680x720.png)

숨김 batch의 GameView.RenderView 실제 표면입니다. Camera.Render·합성·원본 Editor/GameView/OS포커스 전환0. Camera(2,0,-10)/Ortho5, 휠체어(2,0), 세 화면비 모두 HUD/버튼이 viewport 안에 있고 굽은 길 중앙과 북쪽 길이 보입니다. 기존 좌하단 HUD의 남쪽 길 일부 가림은 보존된 기존 한계이며 HUD를 재배치하지 않았습니다.

파일럿 B352와 새1280 화면의 변경은 **2,467픽셀**, 모두 올빼미 그림 영역 안입니다. 올빼미는 기존 애니메이션의 동결 시점 차이로 판단합니다. 실제 위치(5.7,-2),512px/PPU512/scale1, 카메라 환산 canvas [870,468)-(943,540) **밖은 변경 픽셀0**입니다. Builder Sprite의 좌표·회전·Sorting·텍스처·Pivot manifest는 파일럿과 완전히 동일합니다. 올빼미 원본·참조는 변경하지 않았습니다. 전체 화면 픽셀0이라고 보고하지 않습니다.

## 안전 동기화와 보호

시작 원본 main **7e8c9f1b701b0f817c965bbb82941248181ac588**. 원본 보호5,130파일/649,825,058bytes를 별도 Temp/Backup에 저장·SHA 재읽기 일치 확인했고, FF 직전 전체 SHA와 tracked dirty302/untracked550 status가 동일함을 확인했습니다. 개인 보호 manifest/Save/백업은 commit·GitHub에 올리지 않습니다.

제품5파일과 관련 보고·상태 문서만 충돌0·HEAD일치 조건에서 `--ff-only --no-autostash`로 반영했습니다. 원본 Editor가 비활성 상태여서 자동 Import가 보류된 것을 실제 타입/빈 GUID 조회로 확인하고, 포커스 전환 없이 MCP Refresh/컴파일을 요청했습니다. 완료 후 Builder 타입 로드·Script/Snow GUID 일치·PPU128·Pivot(128,128)·Console0·원래 Scene/Play/선택 유지까지 확인했습니다. 원본에서 Play를 새로 시작하거나 F2를 열어 검증하지 않았으며, 원본 적용 제품은 격리37PASS 실행본과 동일합니다. 완료 근거는 original-editor-imported.json 및 original-apply-receipt.json입니다.

원격 Push/Merge·main 직접commit·reset/clean/stash0. 배치 캐시 복제는 완료를 확인한 뒤 실행했고 준비 중 과거 전수 QA 실행0입니다.

## 바울 대표 실플레이 확인 방법

원본 반영 성공 후 **새 Play/이어하기 → 기존 Main24 진행의 서릿길 마을 북문 → 첫눈의 숲 F2**로 들어가 주세요. 굽은 흙길을 휠체어로 왕복하고 마을로 나갔다가 재입장했을 때 눈길이 한 번만 나타나는지 확인하시면 됩니다. 기존 나무·그루터기·올빼미·좌하단 HUD는 그대로입니다. 이미 F2에 저장한 캐릭터도 새 Play에서 Scene 자동 구성을 확인할 수 있습니다.
