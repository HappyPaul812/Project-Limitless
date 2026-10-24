# Issue65 MinimalDelta 실제 Unity 최종 비교

상태: **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / USER_ART_REVIEW_PENDING / NOT_PRODUCTION_APPLIED**.

작업 브랜치 `codex/art-minimaldelta-final-review-20261023`, 기반 `00e6d3c4`, 검수기 `bc357232b16f45cab541ee496694049950d7fdf0`. 최신 기획댓글 [6358377122](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358377122), 신규아트 보고 `docs/art-new` d5a07c99/bfa8100b, 리뉴얼 보고 `docs/art-renewal` f908e676/e3b64fe를 읽고 적용했다. 두 보고의 원격 이미지 미수령 기록은 당시 상태이며, 앞선 Codex 텍스트 인계 댓글은 현재 존재한다. PNG 바이너리의 원격 게시 제한은 여전히 명시한다.

## 새 ZIP 실물

`F:/Downloads/Limitless_Main23_Issue59_MinimalDelta_Review_20261023.zip`, **812,046 bytes / 7 entries / CRC PASS**.
실측 SHA256 **2bb31ffaba77f977f426c216b2a1cc9b86934d640c993f4697679b4eb9d67e11**, 제작본과 일치한다. 내부SHA256SUMS6/6, 보존원본9파일 SHA9/9, Preview Manifest SHA·해상도3/3, 합계18 정적대조 PASS. `StaticPackageChecks.json`과 `ZipAudit.json` 참조.

새 게임용 PNG0·기존 아트 수정0. 세 PNG는 모두 OFFLINE/APPROX 비교판이며 Unity에 스프라이트로 설치하지 않았다. Preview 규격은 오르나1480×590, 주택1840×680, 여관1350×655. 실제 사용자 로컬9원본이 제작보고 SHA와 일치하여 이전 주택/여관 Unity 검증의 적용 근거를 확인했다.

## 이번에 추가한 실제 검증과 재사용 범위

**B 신규 Unity23 PASS / FAIL0 / 최종 컴파일·런타임 오류0.** 인물 bbox높이12건(4높이×3화면비), 동일카메라3건,80/128 휠체어 실제경로·도착·전면정렬6건, 정적Collider불변1건, 런타임오류0 확인1건이다. 모든 검사가 별개 제품기능23개를 의미하지는 않는다.

**A 재사용:** 기존148/136/41/15는 전수 재실행0·이번PASS 합산0. 직전 Issue65의33/8 검사도 다시 호출하지 않았다. 주택/여관3비율 실제 고정카메라 Before/After, Collider·Sorting·휠체어 접근은 이전 실행 근거를 SHA와 함께 그대로 재사용했다. 독침벌은 #64 실제 비교판·방향·alpha 자료를 byte-identical 재사용한다. 새 ZIP은 게임PNG가 전혀 바뀌지 않아 해당 접합을 재시험할 이유가 없으며, 새로운 차이인 인물높이 표기와80px를 추가 검증했다.

새 실행은 숨김 Unity6000.6.5f1 batch Play Mode, 가상 Keyboard/Mouse, 합성fixture/Save만 사용했다. 프로젝트6000.5.7f1과의 버전 차이는 기존 격리 검수와 같다. 실제 OS GameView/Editor 포커스 조작0. 이미지 유형은 **ACTUAL_UNITY_PLAY_CAMERA_UI**(실행 중 Camera+Canvas RenderTexture), 원본 크롭 비교판, INPUT_OFFLINE, A_REUSED로 구분한다. 포커스를 가져온 OS GameView 스크린샷이라고 주장하지 않는다. 기존 Editor Search 예외는 환경 OPEN으로 분리했으며 새 런타임 검사는 오류0이다.

## 오르나 — 인물 높이80px 권고

납품 ZIP은 **alpha bbox가 보이는 인물 높이**를40/80/96/128px로 표시한다. 직전 검토판은512 전체캔버스 높이40/82/96/128px였다. 원본 alpha bbox는(107,24)~(403,508),296×484px이며 원본PNG는 수정하지 않았다.

PPU512를 유지하고 `scale = 인물목표px /72 ×512/484`로 표시한다. 실측은 현재 SpriteRenderer bounds와 Camera 배율에서 원본 alpha bbox를 투영한 높이로 확인했다. 보간 필터로 보이는 매우 낮은 알파의 마지막1px까지 별도 의미론적 실루엣으로 판정한 것은 아니다.

| 인물 alpha 높이 | 전체캔버스 높이 | Transform scale | 전체캔버스 WU |
|---:|---:|---:|---:|
|40|42.31|0.587695|0.587695|
|80|84.63|1.175390|1.175390|
|96|101.55|1.410468|1.410468|
|128|135.40|1.880624|1.880624|

카메라는 orthographic5·화면높이720이므로72px/WU.1280×720/960×720/1680×720 세 화면비에서 동일카메라 변환으로 네 안을 촬영했다. 기존NPC32px·PPU28은82.286px이고 alpha높이도32px다. **인물80px가 기존NPC와 가장 가까운 비례**다.96은 세부 가독성 대안,128은 기존NPC의1.56배여서 배경 점유가 커진다.40은 판독성이 낮다. 현재 플레이어 본체는128셀/PPU128의72px 캔버스이며 실제 그림과 비례를 같이 보드에 제시했다.

80/128에서 실제 Main23MouseTravel→PlayerController/Rigidbody 이동과 전면정렬을 확인했다. 휠체어 본체가 NPC 뒤에 가려지지 않으며 정적Collider도 바뀌지 않았다. 가까이 서면 기존 플레이어 이름표가 NPC 하체에 겹칠 수 있어 미술·UI 검토사항으로 남긴다.80px 추천은720px 기준 검토 권고이며 전체NPC의 PPU 변경이나 정식 외형/StableID/VoiceID 확정이 아니다. 이전82px 전체캔버스는 인물약77.5px였으므로, 권고 방향은 유지하면서 새 납품 기준을 명확히 맞췄다.

`OrnaReferenceRendererMap.csv`는 비교용 기존NPC 복사본과 Orna의 런타임 치수만 제공한다. 초기 Animator 평가 전 플레이어 기본프레임 로그는 현재 외형의 파일경로 근거로 사용하지 않았다. 실제 플레이어 모습은 촬영된 프레임에서 확인한다.

## 남문 제안 — 기존 주택·출입문 KEEP 우선

기존 Residence는 현재 **북서(-12.5,7.2)**에 있다. 남문에 주택을 신설하거나 옮긴 것이 아니다. 기존본체 PPU128/pivot(251,0)/3.922×2.922WU,Collider3.1×1.6은 유지한다.

EntryDoor만 시연한 실제 좌표는(-12.5,7.3),PPU256/pivot(287.5,0)/scale.55,1.235×1.983WU,그룹1280·내부3·자체Collider0이다. Gable/Trim 두 후보는 배치하지 않고 파일을 보존한다. 기존 Sprite 전체 지붕이 유지되고 큰 이중지붕은 사라지지만, **EntryDoor 파일 자체의 작은 눈지붕·문·계단이 기존 출입구와 겹친다.** 기존 다락창/현관 주변을 덮는 형태여서 자연스러운 접합은 ART_HOLD_JOIN이다.

이전 실제 검사 `DoorOnly.NewGableTrimNotInstalled`, `ExistingCompositeRoofPreserved`3비율, `CameraInsideWorldBounds`3비율, `Wheelchair.EntryDoorFrontPocket.Route/ActualArrival`, `AllMinimalLayouts.StaticCollidersUnchanged`를 근거로 재사용했다. `A_REUSED_B_New65_checks.txt` 및 실제 원본캡처6장을 연결했다. B 고정카메라의 기존출입문/EntryDoorOnly 쌍은 카메라Transform 동일이며 새로운 OFFLINE 합성으로 바꿔치기하지 않았다.

**기술:** 두 큰 모듈 제외/기존본체·충돌 유지/휠체어 접근 TECH_PASS(재사용).
**미술:** EntryDoor 접합 HOLD. **기존 출입문 KEEP**이 최소안이다. 꼭 바꿔야 한다면 문틀/문짝만 독립한 후보1PNG와 정확한 접합점을 요청한다. 기존43/원본Residence를 수정하지 않는다.

## 여관 — 본체 내장 간판 하나만 KEEP_OLD

기존 Inn(-6.55,2.2),PPU128/pivot(270,0)/4.219×3.461WU/그룹1780·내부0,Collider3.5×1.85 유지. 별도 StandaloneSign/구 조각간판을 추가하지 않아 본체 내장 간판 하나만 보인다. 기존 actual3비율 캡처와 `Inn.NewStandaloneSignNotInstalled`, `Inn.OnlyOriginalCompositeRenderer`, `Wheelchair.KeepOldInn.Route.Rest/ActualArrival`를 재사용했다.

간판중복 해소·휠체어 접근 TECH_PASS(재사용), **KEEP_OLD 권고**. 작은 맥주잔 아이콘만으로 모든 의미를 전달하지 않고 기존 여관 상호작용 텍스트를 함께 유지한다. 신규 독립 간판은 폐기나 승인취소 없이 후보 보관이다. 기존 Sign_Inn 조각의 외벽/술통 잔여를 독립간판으로 붙이는 권고가 아니다. 남북3WU/동쪽7×7 예약영역 새Collider설치0, 기능변경0.

## 독침벌 — #64 실제 비교판 원본 그대로

[재사용 Unity 비교판](Evidence/MinimalDelta65/A_REUSED_A64_Bee_Ch2_Original_UnityBoard_UNAPPROVED.png).
SHA256 **0c228ce8d9f679b6b6f88564f038e7eb2371234ce98d32d73cd3983d46b77777**.

현 Pilot Bee Idle/Attack, 신규 좌향1포즈, Chapter2 공식11~15 다섯 종의40/128px 공통캔버스·전투표시계약 비교다. 개별 실루엣을 따로 확대하지 않았다. 전투계약은 벌115.6×95.2/기타136×112이며 **제품Battle 교체 화면이 아닌 실제 Unity 검토Canvas**다. 동일 배경·크기 조건의 기존 캡처를 그대로 복사했으며 새 실행/모션검사는0이다.

기존 우향Idle10+Attack10·10FPS·scale.85는 보존. 신규 좌향1자세는 전체 애니메이션 대체 ART_HOLD다.128px 세부는 보이나40px 침/앞다리/날개맥은 미술 검토 필요.512Master 외곽alpha0·소형Preview 경계접촉/방향차이 자료도 #64 재사용이다. 원본제작자 Jana Ochse / 2D!PIXX, OGA CC BY3.0/원본LICENSE CC BY4.0 버전충돌은 RIGHTS_SOURCE_PENDING 유지. Source/GUID/독/AI/Drop/Spawn/공식Ch2 수정0, 신규20프레임·전투Sprite교체0.

## 파일별 문제·실제 이미지

[문제7행](MinimalDelta65_FileProblems.csv), [4주제 축소개요](Evidence/MinimalDelta65/Overview_SCALED_4Topics.png), [새 오르나 비교판](Evidence/MinimalDelta65/B_Orna_Visible40_80_96_128_UnityCrops_UNAPPROVED.png), [주택 실제 비교](Evidence/MinimalDelta65/A_REUSED_Board2_Roof_Excluded_EntryDoorOnly_UNAPPROVED.png), [여관 실제 비교](Evidence/MinimalDelta65/A_REUSED_Board3_Inn_OneSign_KEEP_OLD.png).

총34PNG: 신규Unity14, 이전실제/비교/alpha근거15, 납품OFFLINE3, 신규무배율크롭1, 축소개요1. 모든파일의 실제경로·분류·해상도·SHA는 `ImageIndex_SHA256.csv`. A의원본commit/경로/SHA는 `ReusedEvidenceReceipt.json`, B크롭좌표/SHA는 `B_BoardSources.json` 참조. INPUT_OFFLINE3은 실제Unity 증거로 사용하지 않았다.

## 보호·재현·승인

**원본/모든 기존worktree28,180파일 SHA 변경0**,4곳HEAD/index/status 불변. 이전14,206·21,170 보호를 포함한다. 새worktree 자동환경변환244개만 원복했고 사용자 Save/UserData·승인지면9·기존43·공식펫/Ch2·Main22/23·Main24 정의/씬/원본PNG 변경0이다.

재현: 현재검수기의 Start는 MinimalFinal만 호출한다. 기존33/8/41/15/148/136 루틴을 호출하지 않는다. `Temp/MinimalDelta65/Worktree/Unity/Client`에서 Unity6000.6.5f1 숨김batch `-executeMethod ProjectLimitless.EditorTools.ArtPilotsDeltaQA.Start`로 실행하며, 기존 합성 Checkpoint_Arrival fixture만 쓴다. 사용자가 작업 중인 Editor는 조작하지 않는다.

**4대상 기술 판정:** Orna B신규 TECH_PASS/80px미술선택대기, 주택 A재사용 TECH_PASS_LAYOUT/EntryDoor ART_HOLD_JOIN, Inn A재사용 TECH_PASS_KEEP_OLD, Bee A64재사용 TECH_PASS_SINGLE_POSE/교체 ART_HOLD. **최종 미술승인은 바울 대표 결정**. 본편적용·원격gitpush·main병합0. GitHub #65 정식보고와 #59/#63 인계는 텍스트로 게시하고, 실제PNG 원격미게시 제한을 명시한다.


## 바로 열어볼 로컬 결과물

새 파일 `F:/Downloads/Limitless_MinimalDelta65_Final_Unity_20261023_17db915d.zip`. 새 폴더에 압축을 푼 뒤 **index.html**을 여세요. 47639471 bytes /44 entries /CRC PASS, 전달본 SHA256 `17db915d60dab9dcf0de521d0ac4e37e8cf9a7c0a8876461dd2aa6e944c333fd` 확인 일치. 기존 납품파일 덮어쓰기0.


## 정식 인계 완료

[Issue65 정식 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358720003) · [Issue59 인계](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6358734565) · [Issue63 인계](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6358735741). 게시 후 직접 재조회 확인. 텍스트·SHA만 게시했고 PNG 원격미게시 제한을 명시했다. 로컬 보고/이미지 commit **263283823186ab99abddce45b97b2b3be7e31a82**.
