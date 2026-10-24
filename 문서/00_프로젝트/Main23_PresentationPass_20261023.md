# Main23 마을 화면·안내 보완 — 2026-10-23

**PRESENTATION_CANDIDATE_DELTA_QA_PASS / USER_VISUAL_REVIEW_PENDING**

[Issue #58](https://github.com/HappyPaul812/Project-Limitless/issues/58) 및 docs/planning 계약 `LIMITLESS_20261023_Chapter3_Main23_PresentationPass_Main24_SavePrep.md` blob `f7cbe0a434d43cefb51109f53c98f765d1ff3660`을 기준으로 진행했습니다.

- 기존 브랜치 `codex/chapter3-main23-first-playable-20261023`와 worktree `Temp/Main23FirstPlayable20261023/Worktree` 재사용.
- 기준 `2979e327` / 기존 Main23 **148 PASS 재사용**, 해당 전체 QA·Main22·기본 펫 전수 QA 실행0.
- 이번 시각·안내 제품 commit **`f90966df8e9e84da7670b6e7ef1551ff1bbe8850`**, 53파일. 승인 아트나 원본 제품 적용 commit이 아닙니다.
- 변경분 실행 체크 **118 PASS**, 보호·정적·최종 Scene 체크 **18 PASS**. 최종 수용목록 **136 PASS / 미해결 제품 FAIL 0**. 동일 경로의 세부 도착/거리/UI 판정 등을 센 체크 수이며 서로 다른136개 기능이라는 뜻은 아닙니다.

## 1. 배치 결과와 승인 경계

기존 GameArt43종을 파일별 조사해 **18종을 미승인 후보로 시범 배치**했습니다. 43개 PNG는 과거 v5 검증 영수증 SHA256과 43/43 일치했고, 복사한18 PNG도 원본 bytes와 일치합니다. 기획에 적힌 v5 ZIP은 현재 Downloads에 없으므로 기존 `Temp/FrostwayV5DeltaQA20261021/UnityQA/Input/v5` 실물을 사용했습니다. 새 아트 생성·크롭·PNG 재저장0입니다.

|시범 배치|역할·상태|
|---|---|
|완성형 건물5|여관·파티·스킬판매 예고·보급 건물 시안·주택. 보급 건물에는 상점 기능을 새로 만들지 않음|
|모듈3|목재 벽+Gable 지붕+StoneArch 문 접합 연구. 실제 화면에서 보인 지붕 틈은 Transform으로 줄임. 문/벽/지붕 규격 최종 확정 아님|
|소품6|장작 창고·장작더미·통·벤치·벽 랜턴·가로등. 문 앞 포켓과 남북 간선 밖에 배치|
|안내판3|중앙 안내·남문·북문. 976/966px 큰 안내판만 PPU512, 다른 후보는 PPU128, bottom-center Pivot|
|비활성 기단1|동쪽7×7WU 안의 돌바닥 후보. Collider/Trigger/Spawn/Quest 마커/순간이동 기능0|

Asset 경로는 `Assets/_Project/Art/PresentationCandidates/FrostwayV5/`입니다. Resources 정식 카탈로그에 등록하지 않았습니다. Hierarchy의 후보 GameObject 이름에 `ART_CANDIDATE_UNAPPROVED`를, 주민 임시 외형에는 `VISUAL_PLACEHOLDER_Orna`를 사용합니다. 실제 화면 오른쪽에는 **지면 승인 / 건물·환경 미승인 시안 / 주민 임시 외형** 안내가 표시됩니다. 이는 Unity TagManager 변경이 아니라 이름·화면·상태 분류입니다.

승인 `FrostwayVillageV10` Sprite9/Tile9/PNG/meta는 변경0입니다. 기존 지면 Tilemap·Bounds·Camera·Player·Spawn의 저장 블록도 그대로입니다. 원래 여관·파티 임시 박스2개(6 YAML 블록)만 제거하고 후보 Renderer/발 기준 Collider로 대체했습니다. 기존 블록 변경은 Scene 루트 목록1개뿐입니다. [Scene 변경 근거](Evidence/Main23Presentation20261023/VillageDeltaProof.json).

여관 앞(-6,2), 파티 앞(6,2), 주민(-3,-4), 거점(0,0), 남문(0,-10), 북문(0,9) 등 기존 상호작용 위치는 유지했습니다. 펫 NPC 두 곳은 **이 마을 좌표만** (-6,5)/(6,5) → (-6,7.5)/(6,7.5)로 옮겨 건물 지붕과 분리했습니다. 분양/관리 정책·가격·장착·기존 Wolf/Bear/Fox 코드는 변경하지 않았으며 앞 포켓 통행만 추가 확인했습니다.

## 2. 실제 부족 아트 — 파일별 인계

[Main23_Frostway_VisualGapList.csv](Main23_Frostway_VisualGapList.csv)는 **47행**입니다: 기존43종 + 현재 주민 임시 외형1 + 신규 파일럿 제안3. 행마다 원인·우선순위·SHA·픽셀 크기·PPU·Pivot·WU·Sorting·Collider·배치 좌표·수용 조건을 적었습니다. 미배치43종 전체를 재작업하라는 목록이 아닙니다. 우선순위는 이번 화면 기준의 작업 제안이며 과거 CSV 심각도 등급을 바꾼 것이 아닙니다.

|우선|신규 파일명 제안 — 아직 파일 없음|필요성|
|---|---|---|
|P0|`Frostway_NPC_Orna_Prototype.png`|현재 Eldiran OGA09는 임시 외형. Chapter3 주민 안내 NPC의 고유 실루엣·얼굴 부족. 이름/VoiceID 미확정|
|P1|`Frostway_Residence_FrontDoorRoof_Joined.png`|문·벽·지붕의 기준점과 스타일을 맞춘 정면 접합1세트. 현재 배치 보정만으로 정식 접합 품질을 확정하지 않음|
|P1|`Frostway_Sign_Inn_Standalone.png`|현행 분리 간판에 외벽/기단 조각이 들어 있어 작은 독립 간판 역할 부족|

**기존 OPEN16은 미해결16/해결0 유지**입니다. 과거 좌표 원문을 확보해 [Inherited_OPEN16.json](Evidence/Main23Presentation20261023/Inherited_OPEN16.json)과 CSV의 좌표 열에 연결했습니다. 이번 시안에 쓰인 GeneralShop/StoneArch/Bench 3종에도 과거의 작은 가장자리 잔여가 남아 있으며 CSV에서 표시했습니다. 후보를 렌더할 수 있다는 사실을 정식 게임 미술 PASS로 승격하지 않았습니다. 나머지 오염된 분리 소품·벽/문/지붕·간판은 억지로 붙이지 않았습니다.

이 CSV와 Renderer 매핑은 `docs/art-new` 담당이 검토할 수 있는 인계 산출물입니다. 리뉴얼 담당 연락/추가 발주·GitHub 쓰기·Push는 하지 않았습니다.

## 3. 퀘스트 안내와 접근성

5목표의 사건·ID·보상·시작/완료/저장 로직은 그대로입니다. `Main23Site.TryInteract()`의 대화·문·거점 처리 본문은 기준과 동일함을 비교했습니다. 화면 보완 범위는 다음과 같습니다.

- 목적지를 번호와 어두운 바탕의 월드 안내판으로 표시. 현재 목표는 기호로 구분하며 색만으로 전달하지 않음.
- HUD에 목적지 방향·거리, 클릭 이동/선택, E/F 상호작용, Escape/우클릭 취소를 표시.
- 안내판 자체를 클릭해도 해당 지점 owner 하나로 이동·선택. 표시 위치와 실제 발/상호작용 위치는 분리.
- 플레이어 이름표와 겹치지 않도록 안내판을 높였고, 먼 일반 표시는 숨겨 화면 혼잡을 줄임.
- 기존 HUD 골격을 유지하면서 글 영역을 늘리고 이동 취소 버튼·Escape/우클릭 이동 취소를 추가. 타이머·자동 구매·강제 진행0.

## 4. 변경분 Unity 검증

설치 Unity **6000.6.5f1**의 숨김 batch Play Mode에서 실제 Scene/PlayerController/Rigidbody2D/가상 Input System으로 검증했습니다. 저장소의6000.5.7f1과 차이가 있으며 자동 패키지·메타 변환244파일 및 생성 환경파일7개는 검증 후 복원/정리했습니다. 원본 Editor나 Game View를 foreground로 가져오지 않았습니다.

|검증 범위|결과|
|---|---|
|기존 합성 체크포인트에서 5단계 목적지·클릭/E/F/취소 안내|PASS. 목표 전체 플레이 재실행이 아니라 상태별 표시 검사|
|도보 주민 안내판 클릭 owner·E/F 여관/파티·Escape 취소|PASS|
|휠체어 주민·여관 문·파티 문·판매예고 문·남북문·펫 NPC 앞 포켓|PASS. 펫 구매/장착 전수 검사0|
|Escape·우클릭 이동 취소|PASS|
|새 건물 배치 후 거점 등록 → 합성 Save → Continue|PASS. 유효 좌표 복원·새 Collider 내부 진입0|
|남문 실제 F1 왕복·안전 Spawn|PASS. F1 Scene 파일 변경0|
|북문 F2 안내/비개통, 동쪽 예약7×7 비충돌·기단 접근·자동 Trigger0|PASS|
|16:9/4:3/21:9 Camera orthographic5 동배율·경계·시안 구분|PASS|
|최종 컴파일·Missing Script·기존 Tile9 참조|PASS|

초기 검증기에서 Continue 이전 Scene의 이동 컴포넌트를 재사용해 포탈 경로 체크1개가 실패했습니다. 새 Scene 참조로 수정하고 **남은 항목만** 이어 검증했습니다. 캡처용 Canvas를 Camera 모드로 바꿀 때 월드 건물이 HUD를 덮던 캡처 정렬과, 서쪽 캡처의 자동 화면비도 고쳤습니다. 원본 로그·초기 실패는 로컬 Temp에 보존했습니다. 최종 공개 화면은 수정된 캡처 경로로 촬영했으며, 지붕 후속 보완은 별도의 최종 서쪽 화면에 기록했습니다.

제품 런타임의 예상하지 않은 오류0, 컴파일 오류0입니다. 기존 **Unity Editor Search 인덱스 예외는 OPEN**이며 환경 오류와 제품 오류를 구분했습니다. 실제 사용자 장치 손플레이·최종 미술 승인은 미검증입니다.

[실행118체크](Evidence/Main23Presentation20261023/DeltaChecks.txt) / [보호·정적18체크](Evidence/Main23Presentation20261023/StaticChecks.txt) / [실제 Renderer22 매핑](Evidence/Main23Presentation20261023/CandidateRendererMap.csv) / [43종 SHA 영수증](Evidence/Main23Presentation20261023/Source43.json).

## 5. 실제 Unity 화면

모두 실제 Unity Camera+Canvas를 RenderTexture로 읽은 개별 프레임입니다. 오프라인 조합을 Unity GameView라고 주장하지 않습니다. 데스크톱 화면 조작/foreground 캡처는 하지 않았습니다.

- [16:9 중앙 마을](Evidence/Main23Presentation20261023/Village_Presentation_1280x720.png)
- [4:3 중앙 마을](Evidence/Main23Presentation20261023/Village_Presentation_960x720.png)
- [21:9 중앙 마을](Evidence/Main23Presentation20261023/Village_Presentation_1680x720.png)
- [주민·남쪽 상점 시안](Evidence/Main23Presentation20261023/South_ResidentAndShops_UnityCameraUI.png)
- [서쪽 지붕·문 접합 보완](Evidence/Main23Presentation20261023/West_ModularCandidate_UnityCameraUI.png)
- [북문 F2 비개통](Evidence/Main23Presentation20261023/North_F2Closed_UnityCameraUI.png)
- [동쪽 비활성 기단·휠체어](Evidence/Main23Presentation20261023/East_InactivePortal_UnityCameraUI.png)
- 목표별: [① 심부](Evidence/Main23Presentation20261023/Goals/Goal1_DeepDestination.png) → [② 흔적](Evidence/Main23Presentation20261023/Goals/Goal2_DistinctTrace.png) → [③ 마을](Evidence/Main23Presentation20261023/Goals/Goal3_VillageDestination.png) → [④ 주민](Evidence/Main23Presentation20261023/Goals/Goal4_ResidentDestination.png) → [⑤ 거점](Evidence/Main23Presentation20261023/Goals/Goal5_CheckpointDestination.png).

## 6. 원본 보호·다음 단계

원본 main HEAD `47ad2eec`, index SHA와 working status 불변, 원본 C#/Scene/Save/UserData/승인 지면/펫 등 **637파일 SHA256 불일치0**입니다. Main22·F1 Scene·승인3펫·BuildSettings·Main23 Quest Asset·Save/Economy/SkillLoadout 코드 변경0입니다. 원본 적용·Push·main 직접 commit/병합0입니다.

Main24 추가 작업은 [읽기 전용 6슬롯 Save 설계·14개 테스트 계획](Main24_SkillSixSlot_Save_ReadOnly_Design_20261023.md)으로 분리했습니다. 실제 판매·결제·획득/강화·세이브 구조 변경0, 해당 테스트 실행0입니다. Issue #46을 해결 완료로 취급하지 않습니다.

바울 대표의 **시안 화면 검토**가 다음 단계입니다. 후보18종 채택·OPEN16 표적 수정·신규 파일럿3종 우선순위와 실제 장치 손플레이 피드백을 받은 뒤 원본 적용 여부를 별도 결정합니다. 현재 결과는 격리 프레젠테이션 QA이며 정식 마을 미술 승인이나 본게임 적용 완료가 아닙니다.
