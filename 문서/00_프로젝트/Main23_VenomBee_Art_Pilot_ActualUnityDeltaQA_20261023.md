# Issue64 신규 아트 통합 실물 검수 — 격리 전용

상태: `LOCAL_QA_COMPLETE / LOCAL_COMMITTED / USER_ART_REVIEW_PENDING / REMOTE_NOT_PUSHED`.

## 검수 계약과 보호 범위

[Issue64](https://github.com/HappyPaul812/Project-Limitless/issues/64), [Issue59](https://github.com/HappyPaul812/Project-Limitless/issues/59), [Issue63](https://github.com/HappyPaul812/Project-Limitless/issues/63) 및 docs/planning Main23 Presentation 계약 기준. 기존 Main23 148 및 Presentation136을 호출하지 않고 두 ZIP의 신규 아트만 검증한다. 미술 최종 결정은 바울 대표 권한이다.

원본 main `47ad2eecb40135b2ee28e4947d642ebdbf0bf68a`와 기존 격리 HEAD `3f30d135`를 보존한다. 별도 `codex/art-pilots-isolated-qa-20261023`가 기존 격리 HEAD에서 분기했다. 새 PNG는 `Art/PresentationCandidates/Issue64`에 byte-identical 복사하고 Editor 검수기가 Play Mode에서만 배치한다. Scene 저장/MonsterDefinition/GUID 교체/기능 코드 변경은 하지 않는다. 승인 지면9/환경43/OPEN16/Quest/Save/Main24/펫 원본은 동결한다.

실행 환경: Unity6000.6.5f1 숨김 batch Play Mode, virtual Keyboard/Mouse, 합성 QA 저장만 사용. 저장소 버전6000.5.7f1과 실행 버전 차이는 기존 격리 검수와 같다. 실제 OS GameView 활성화/포커스 조작 없이 Camera+Canvas RenderTexture를 캡처한다. Unity 비교판은 제품 전투 화면 변경이 아닌 비교용 Canvas이며, ZIP의 OFFLINE 이미지는 실제 실행 근거로 사용하지 않는다.

## 실물 확인

`Evidence/ArtPilots20261023/ArchiveAudit.json`: Main23 8,008,308 bytes/20 entries, SHA `425292ebd1e8a58ab7c4486f6fac39491c265d49b5db933dfe7586e69cee7351`; Bee 2,291,579 bytes/8 entries, SHA `2e65662d68c77cfb90d7e39d9d40c2cd2816ee33882debe8a8e93ace782bd478`. 발주 SHA 둘 다 일치, CRC 오류0. SHA256SUMS Main23 19/19·Bee7/7, Manifest 신규파일 Main23 6/6·Bee6/6, 총38건 일치. PNG 크기·alpha 상세는 PngAudit/AlphaHistogram 참조. 원본 ZIP에는 쓰지 않았다.

## 발견한 문서와 실물의 충돌

1. 실제 마을 Scene의 Residence는 북서쪽(-12.5,7.2)에 있다. 남문에는 주택이 없다. 남문 조인트 제안은 이 Residence에 접합하는 후보로, 실제 위치를 임의로 남문으로 확정하지 않는다. 현재 좌표 그대로 Before/After를 촬영한다.
2. 두 PilotBee README는 실게임 미연결이라고 적었지만 현재 VenomBee 정의와 Field01 세 스폰은 해당 GUID를 참조한다. 원본 PNG와 활성 PNG SHA도 일치한다. README 원본은 수정하지 않고 모순을 보고한다.
3. [OGA 배포 페이지](https://opengameart.org/content/pilotbeegamecharacter)는 CC BY3.0, 보관 ZIP의 LICENSE.TXT는 CC BY4.0이다. 제작자 Jana Ochse / 2D!PIXX 확인. 라이선스 버전은 `RIGHTS_SOURCE_PENDING`; 출처를 확인했다는 사실과 충돌 해결은 별개이다.

## 검증 결과

신규 검수 실행 **41 PASS**, 비교 보정 촬영 **15 PASS**, 최종 런타임 예외0/컴파일 오류0. 후자의 Chapter2 참조5 및 3화면비 크기 확인은 보정 이미지 재확인으로 일부 반복이며 별개 56개 기능을 검증했다는 뜻이 아니다. 기존148/136/Main22/펫/Ch1·2 전체 QA 실행0. 의도적으로 재현한 미술 결함은 아래 ART_HOLD로 남겼으며 기술 검사 PASS를 미술 PASS로 해석하지 않는다.

초기 임포트에 패키지 `com.unity.2d.tooling`의 긴 경로 USS/UXML DirectoryNotFound 환경 오류가 있었고, 최종 Play Mode 신규 런타임 예외는0이다. 기존 Editor 환경 전체 정상화 검수는 범위 밖이다. Unity 버전 자동 변환244 tracked 파일을 검수 종료 뒤 격리 HEAD로 복구했다. 후보 PNG8개(신규6+독침벌1+기존 간판 참고1)의 전용 meta와 Editor 검수기 외 제품 변경0.

## 작업군별 판정

| 아트 | 기술 판정 | 미술·채택 보류 사항 |
|---|---|---|
| 오르나 | **TECH_PASS: 투명 PNG 표시·기존 상호작용 연결·통행** | 40px 주 표시 **ART_HOLD**. 기존 임시 NPC82px 대비 작음. 128px 원화 인지성은 개선되나 기존 픽셀 NPC와 질감/비례 및 최종 표시 크기 결정 필요. 정지 자세1장만 있음. |
| 남문 제안 문/지붕/받침 | **ART_HOLD: 이중 박공 재현** | 기존 주택 본체 지붕과 큰 신규 박공 중첩. Gable/Trim을 설치하지 않는 DoorOnly가 최소 대안이며 이것도 미술 승인 아님. 실제 남문 신설 위치는 미확정. |
| 여관 간판 | **TECH_PASS: 독립 alpha·표시 크기·통행**, 현재 설치 **ART_HOLD** | 새 그림은 독립 간판으로 사용 가능. 다만 현재 Inn 본체에 구 간판이 포함되어 중복. 현 Scene은 **KEEP_OLD(본체 내장 간판)** 권고. `Frostway_Sign_Inn.png` 조각을 별도로 붙이라는 권고가 아님. NEW_SIGN_CANDIDATE는 sign-less 입면/접점 승인 뒤 검토. |
| 독침벌 | **TECH_PASS: 단일 후보 표시/비교**, 실제 교체 **ART_HOLD** | 좌향1자세이며 기존 우향 Idle10·Attack10 계약과 불일치. 신규20프레임 존재/동작 PASS 주장0. 기존 외부 소스 라이선스 버전은 RIGHTS_SOURCE_PENDING. |

모든 행 **USER_ART_REVIEW_PENDING**. 후보 Master의 큰 사각형 배경/캔버스 테두리 잔상은 관찰되지 않았다. Main23 후보 내부 alpha가 주로252~253인 것은 측정 사실이며 가장자리 전체가 불량이라는 판정은 아니다. 흰색/암색/녹색 배경 비교에서 두꺼운 명암 경계는 보이지만 원화 표현과 추출 오류를 자동 동일시하지 않는다. Bee 작은 납품 Preview만 여백 점검 필요.

## 실제 크기·PPU·Pivot·정렬·통행

카메라 Main23 orthographicSize5, 720px 높이에서72px/WU. 1280×720(16:9),960×720(4:3),1680×720(21:9) 고정 수직 배율. 오르나40/128은 원본512 전체 캔버스 표시 높이이며 alpha 인물 높이는484/512배(약37.8/121px)이다.

| 파일/역할 | 실제 좌표 | PPU / pivot px / scale | 캔버스 WU / 픽셀 | Sorting·충돌 |
|---|---|---|---|---|
| 기존 임시 NPC | -3,-4 |28 /16,0 /1|1.143×1.143 /82.3px높이|2400, 자체Collider 없음|
| Orna512 | -3,-4 |512 /256,0 /.5556 또는1.7778|.556² /40px,1.778² /128px|기존Renderer2400 유지, Collider/클릭Owner 추가0|
| Residence 기존 본체 | -12.5,7.2 |128 /251,0 /1|3.922×2.922|그룹1280, 기존 Collider3.1×1.6 유지|
| EntryDoor | -12.5,7.3 |256 /287.5,0 /.55|1.235×1.983|그룹1280/내부3, 신규Collider0|
| Gable | -12.5,8.8 |256 /355.5,0 /1.4|3.888×2.723|그룹1280/내부2, 신규Collider0|
| Trim | -11.65,8.6 |256 /140,0 /.5|.547×.469|그룹1280/내부4, 신규Collider0|
| 새 Inn Sign | -5.35,3.8 |512 /567.5,0 /.55|1.219×1.178 /84.85px높이|그룹1780/내부5, 신규Collider0|
| 구 간판 참고 | 같은 접점 |128 /55,0 /1|.859×1.125 /81px높이|같은 그룹, 신규Collider0|
| 신규 Bee 필드 정지 후보 | -6,-1.5 |512 /256,256 /.85|.85² /61.2px높이|3, Collider/AI/Drop/Quest 연결 없음|

전체는 `RuntimeRendererMap.csv`. 초기 구 간판은 실수로 PPU512/20.25px로 촬영되어 공정 비교 자료에서 제외하고 PPU128/81px로 보정했다. 초기 Bee 하단 pivot도 현재 셀의 중심 pivot과 맞춰 수정했다. 최종 동일 이름 이미지는 보정본이며 초기 기록은 `Initial_*`로 구분했다. PNG 바이트를 수정한 것은 아니다.

실제 Main23MouseTravel→PlayerController/Rigidbody 이동으로 도보/휠체어 주민·여관 도착, 주민 E 대화 열기/Escape 취소, 남북 중심 통로 이동 확인. 새 간판은 건물의 기존 footprint 안 위쪽에 붙고 문 앞 바닥을 막는 Collider를 추가하지 않는다. 원래 Inn 본체 Collider3.5×1.85 유지. 동쪽7×7 예약공간 고정Collider0. 남문 전환/북문 비개통/F2·포탈 기능 코드는 변경0이며 과거 전수 회귀를 재실행하지 않았다. 새 PNG의 알파 모양만 보고 이동 Collider를 자동 생성하지 않았다. 사람의 실제 장치 손플레이·시각 접근성 최종 체감은 NOT_TESTED.

### 겹지붕 측정과 최소 요청

본체 rect x[-14.461,-10.539],y[7.2,10.122]; 새 박공 x[-14.444,-10.556],y[8.8,11.523]. AABB 중첩 **3.888×1.322WU**, 128sample/WU·alpha>128 실루엣 교차 약 **1.218WU²**. 두 그림이 같은1280 그룹 내부0/2에서 실제 겹쳐 보이는 캡처로 결함을 확인했다. 이 수치는 접합점의 정식 디자인 규격이 아닌 이번 시연 배치 측정이다.

최소 Delta 요청안: **기존 Residence PNG는 KEEP**, 현재는 새 Gable/Trim을 빼고 DoorOnly를 검토한다. 큰 새 지붕이 꼭 필요하면 기존 지붕을 소스에서 지우지 말고 아트 담당이 `roof-less facade CANDIDATE` 1파일 또는 기존 처마에만 붙는 작은 Gable 후보1파일을 별도 납품하고 XY 기준을 명시한다. 전체 환경43 재제작 불필요. 문 자체 작은 박공·계단/Trim 받침까지 최종 접점 승인 필요.

## 독침벌 실제 원본·라이선스와 동배율 비교

`02_VenomBee.asset` Idle GUID `24000000000000000000000000000004` → `Resources/MonsterValidation/PilotBee/pilot_bee_idle.png`(1904×430);238×215/8열/10프레임. Attack GUID `24000000000000000000000000000005` → 같은 폴더 `pilot_bee_attack.png`(3150×253);315×253/10열/10프레임.10FPS/right-facing/visualScale.85. 세 Field01 Spawn은 Definition GUID `b33f06912b2a48d0afcd626d1d88fd30`를 공유한다. 실제 Field01에서 세 MonsterFieldController 및 MonsterSpriteSheetAnimation의 Idle10/Attack10 배열·Idle texture 경로를 확인했다. `LiveBeeFrame.txt`와 실행 Checks 참조.

활성 Idle SHA `febcca0316167d50d4638216d9534ed2974de01c62e1c0c2daaba849726b6c63`, Attack SHA `0e8886c56d7670fb317d4cada41a03e4684f9cf97adf908f2b860740630c018e`. ThirdParty 보관 PNG·원본 ZIP 내부 PNG까지 각각 동일. ZIP SHA `c979b691e76e850fb8c1949d8bf53fe55b0b085b0a5012d83d5f6262ae48a2b6`, CRC PASS. 따라서 현 활성 벌의 출처가 Pilot Bee라는 사실은 확인됐다. README의 미연결 문구는 오래된 상태이다.

크레딧: **Pilot Bee — Jana Ochse / 2D!PIXX**, [OpenGameArt 원 배포](https://opengameart.org/content/pilotbeegamecharacter). 2026-10-23 조회 페이지CC BY3.0과 원본ZIP LICENSE의CC BY4.0 불일치는 해결0. 어느 버전으로 최종 권리 계약을 처리할지 Codex가 임의 확정하지 않는다. 신규 후보의 자체 제작 주장은 ZIP의 AI 생성 원화 분리/축소 설명에 따른 납품 출처이며 제3자 권리 독립 보증으로 바꾸지 않는다.

동일 배경·동일 캔버스40²/128², preserveAspect=true로 현재 Idle/Attack·신규1포즈·**Chapter2 공식11~15 다섯 종 실제 explicitIdleFrames[0]**를 Unity Canvas에 나란히 렌더했다. 실루엣 bbox를 개별적으로 키워 맞추지 않았다. 전투 줄은 실제 BattleSceneController 적 Image 계약136×112, 벌은visualScale.85를 적용한115.6×95.2이다. 이는 **실제 전투 크기 계약을 재현한 검수 Canvas**이지 원본 Battle Scene에서 후보를 교체해 전투를 끝냈다는 뜻이 아니다. 실제 필드에서는 .85 공통Scale·중심축을 맞춰 후보를 옆에 놓았다.

관찰: 기존 벌은 큰 눈/단순 평면 만화형, 후보는 갑각·명암·위협감이 강해 Chapter2의 세밀한 몬스터 표현에 더 가깝다. 128px에서 날개/침이 분리되나40px에서는 가는 다리·날개맥·침의 독립 식별이 약해진다. 후보 원화는 좌향이라 현재 우향 계약과 반대이며, Runtime flipX 미리보기도 별도 캡처했다. 방향·40px 단순화·위협감·팔레트 조화는 바울 대표 검토 사항이다. 기존20프레임 확인은 신규1자세의 애니메이션 검증과 별개이다.

## 화면과 문제 목록

실제 Unity Play Camera/UI PNG24종과 추가 합성 비교판/오프라인 alpha자료를 구분한다. 모든 신규 아트 게임 캡처 파일명은 UNAPPROVED를 포함한다. 3화면비에서 주제의 표시 배율은 같고 4:3은 좌우 환경 노출이 줄며 HUD가 차지하는 비율이 커진다. 오르나40의 작음, 새 간판의 내장 간판 중복과 상단 라벨 간섭, 이중 지붕은 화면비를 바꿔도 남는다. 21:9에서도 장식과 동선이 늘어날 뿐 이 결함이 해결되지는 않는다.

- [실제 주택 전후·최소 대안](Evidence/ArtPilots20261023/Roof_BeforeAfter_UnityComparison_UNAPPROVED.png)
- [오르나 기존·40·128 비교](Evidence/ArtPilots20261023/Orna_Scale_UnityComparison_UNAPPROVED.png)
- [여관 기존·구 조각·새 간판 비교](Evidence/ArtPilots20261023/Inn_BeforeAfter_UnityComparison_UNAPPROVED.png)
- [독침벌·Chapter2 다섯 종 동일40/128/전투박스](Evidence/ArtPilots20261023/UNAPPROVED_Bee_Ch2_Comparison_UnityCanvas_960x720.png)
- [실제 Field01 현재 벌·후보](Evidence/ArtPilots20261023/UNAPPROVED_Bee_OldAndNew_ActualField01_1280x720.png)
- [파일별 문제·최소 보완11행](Main23_VenomBee_ArtPilot_Problems_20261023.csv)

## 보호·재현·인계

원본 main 및 기존 Main23 worktree의 **14,206파일 SHA 변경0**, 양쪽 HEAD/index/status 불변(`ProtectionAfter.json`). 사용자 Save/UserData와 기존637 보호 대상도 이 범위에 포함한다. 새 브랜치의 Unity 자동환경 변환244파일은 복구했다. 새 검수기+후보 PNG/meta+보고서/근거만 로컬 커밋한다. 원본 Unity 적용·main 병합·원격 Push0. GitHub 댓글도 이 로컬 보고서의 원격 공개로 오인하지 않는다.

재현: `Reproduce.md`, 실행 체크 `Initial_checks.txt`41행 및 `Corrected_checks.txt`15행. 최종 미술 승인 요청 대상은 본 보고서와 실제 화면이며, 외부 배포/본편 적용/20프레임 제작은 후속 별도 지시 사항이다. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**. 관련 commit은 CURRENT_STATUS와 Git log에 기록한다.


검수기·후보 로컬 commit: `3bb4f9835fbb13264cf07c907a11e9eee4dc12f0`. 정적 신규11 PASS(`StaticFinalChecks.json`). 보고서/화면의 후속 로컬 commit은 Git log 참조.
