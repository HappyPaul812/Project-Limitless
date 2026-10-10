# Field_01 리뉴얼 아트 v1 기술 검수 — 2026-10-10

## 판정 및 범위

**최종 판정: USER_ART_REVIEW_PENDING.** ZIP/PNG 무결성 및 실제 환경 참조 대응은 PASS. 현재 배치에 맞춰 교체할 수 있는 자료는 모두 있으나, 미술 승인과 일부 경계·Importer 확인을 거치기 전에는 즉시 통합하지 않는다. 확인된 필수 재작업 PNG는 **0종**이다. 잔디 3종의 반전 반복 경계, 길 2종의 계단형 변곡을 미술 검토 대상으로 남긴다. 엄밀한 모든 방향 픽셀 경계 일치 검사는 일부 FAIL이므로 “전 조건 무결점 기술 PASS”로 해석하지 않는다.

이번 작업은 원본 ZIP을 읽고 로컬 Scene·생성기·연결 데이터 및 실제 Editor의 기존 Sprite 정보를 대조한 검수다. 신규 아트 Import 0, Scene 교체 0, Prefab 수정 0, Generator 수정 0, Save 수정 0, Scene 재생성 0. Unity Play/화면 조작/포커스 전환/강제 종료/Push 0.

검수 기준 HEAD: `4e78d70d0c2acba911aa2ebe4c214acea0ebe580`. 프로젝트 및 실행 중 Editor 버전 모두 `6000.6.5f1`. Editor는 Bootstrap/Edit, 컴파일 중 아님, Scene dirty=false, focused=false 상태를 유지했다. Field_01을 Editor에서 열지 않았다.

## 입력·기준 자료

- 원본: `F:\Downloads\Limitless_Field01_Renewal_Art_v1.zip`
- ZIP SHA-256: `6db9d63034423d982ead97c2c9ab8d42767f8608dfa0f37121ac75abef05bbd4`
- Scene: `Unity/Client/Assets/_Project/Scenes/Field_01.unity`
- 생성기: `Unity/Client/Assets/_Project/Scripts/Editor/Field01SceneGenerator.cs`
- 승인 마을: [StarterVillage v4 검수](StarterVillage_Renewal_Art_v4_Inspection_20261010.md), [CURRENT_STATUS](CURRENT_STATUS.md). 승인 Scene/NPC Commit `7309f772`를 기준으로 하고 문서의 과거 미커밋 기록은 현재 상태와 구분한다.
- 연결·안전 구역: `Resources/FieldConnections/Field01_SouthToField02.asset`, `Resources/FieldEntranceSafetyZones/Field01_North.asset`, `Field01_South.asset`.
- 상세 수치·로컬 Editor Sprite rect/PPU/GUID/fileID: [technical_audit.json](Evidence/Field01ArtV1_20261010/technical_audit.json).

GitHub 원격의 최신 상태를 fetch/pull하여 갱신하지 않았다. 이번 대응표와 판정은 현재 LOCAL을 기준으로 한다.

## ZIP·PNG·Manifest

ZIP 21개 항목: 환경 PNG 12개, Preview PNG 3개, 반복 QA PNG 1개, Manifest JSON/MD 2개, ChangeNotes MD 1개, QA JSON 2개. CRC 오류 0, 모든 항목 읽기 성공, 파일명 대소문자 중복 0, 외부/상위 경로 항목 0. PNG **16/16 정상 디코딩**. JSON 3개 및 Markdown 정상 읽기.

환경 PNG 12종의 파일명·크기·SHA-256이 Manifest와 모두 일치한다. 대응 GUID/fileID 중복·누락 0. 5개 바닥 PNG는 RGBA 완전 불투명(alpha=255), 7개 소품은 RGBA alpha=0/255로 배경이 투명하다. 반투명 값은 없으며 픽셀 아트에 적합하다. 검토용 PNG 4개는 RGB 불투명이며 정상이다. 눈에 띄는 색상 손상이나 소품의 의도치 않은 알파 절단은 발견하지 못했다. 투명 여백의 존재 자체를 손상으로 간주하지 않는다.

Manifest의 `original_meta_ppu_seen: 100` 및 Scene GUID/fileID 불일치 주의문은 로컬 상태 전체를 설명하지 못한다. 실제 Editor AssetDatabase로 확인하면 현재 참조 **12/12가 해석**되고, **11종 PPU128 / Rock B 한 종 PPU100**이다. 이름 `rock_02`는 로컬 Sprite `rock_02_0`를 가리키는 별칭이며 GUID/fileID는 맞다. 통합 시 이 보고서의 로컬 대응을 사용한다. 원본 Manifest는 수정하지 않았다.

## 실제 Scene 대응표

현재 Scene 환경 SpriteRenderer **363개 / 12종**을 전부 분류했으며 Manifest의 12종·363개와 일치한다. 별도 환경 장식물 누락은 없다. 플레이어 Prefab과 런타임 NPC·몬스터는 이 환경 교체 수에 포함하지 않는다.

| 기존 Sprite | 신규 PNG (접두사 `LL_C1_F01_`, 접미사 `_v1.png`) | 수 | 해상도 | Sorting |
|---|---|---:|---|---:|
| tiles_grass_4_0 | Ground/Grass_A_128 | 105 | 128×128 | -10 |
| tiles_grass_5_0 | Ground/Grass_B_128 | 106 | 128×128 | -10 |
| tiles_grass_6_0 | Ground/Grass_C_128 | 104 | 128×128 | -10 |
| tiles_grass_4_4 | Ground/Path_Left_128 | 15 | 128×128 | -8 |
| tiles_grass_5_4 | Ground/Path_Right_128 | 15 | 128×128 | -8 |
| tree_big | Props/Tree_Big_255x256 | 2 | 255×256 | 4 |
| tree_medium | Props/Tree_Medium_128x156 | 3 | 128×156 | 4 |
| bush_01 | Props/Bush_A_128 | 2 | 128×128 | 2 |
| bush_02 | Props/Bush_B_128 | 1 | 128×128 | 2 |
| rock_01 | Props/Rock_A_128 | 1 | 128×128 | 3 |
| rock_02_0 | Props/Rock_B_128 | 1 | 128×128 | 3 |
| fence_tiles_2_2 | Props/Fence_Wood_128 | 8 | 128×128 | 2 |
| **합계** | **12종** | **363** | | |

분류: 잔디315 + 길30 + 나무5 + 덤불3 + 바위2 + 울타리8. 위치·스케일·Sorting·flipX는 현재 Scene을 그대로 사용해 검토했다. 직접 만든 2688×1920 합성의 RGB 픽셀과 ZIP 제공 Layout 사이 차이는 **0픽셀**이다. 제공 Layout이 현재 로컬 환경 배치와 대응함을 별도로 확인한 결과다.

## 규격·반복·연결

신규 권장 설정은 PPU128, Pivot(0.5,0.5), Single, Point, Mipmap off, 무압축이다. Importer를 실제 생성하지 않았으므로 신규 Importer QA는 NOT_VERIFIED. 나무 대형은 255px 폭이라 중앙 Pivot x127.5가 정상이며 이를 256px로 확대하거나 Pivot을 임의 반올림하지 않는다.

| 검사 | 결과 | 해석 |
|---|---|---|
| 각 잔디 A/B/C 자체 좌우·상하 반복 | PASS | 제공 단일 타일 반복 조건 |
| 잔디 A/B/C와 좌우 반전의 가로 36조합 | PASS | 연결 가장자리 RGBA 일치 |
| 같은 반전 방향의 잔디 상하 혼합 | PASS | 원본끼리/반전끼리 일치 |
| 서로 다른 반전 방향의 잔디 상하 혼합 | 픽셀 일치 FAIL | 18/36조합, 각 84/128px 차이, RGB 최대55 |
| Scene의 잔디 세로 인접 쌍 | 픽셀 일치 FAIL | 294쌍 모두 서로 반대 flipX; 길에 가려지는 부분 포함 |
| 길 Left/Right 각각 세로 반복 | PASS | 상단·하단 일치 |
| 길 중앙 Left→Right | PASS | 좌우 역할 및 중앙 가장자리 일치 |
| 길 바깥 가장자리→잔디 | PASS | 좌우 가장자리 일치 |
| 울타리 좌우 반복 | PASS | 레일 끝 연결, 의도된 수평 끝 alpha 접촉 |

잔디 차이는 `(원본 x,y)` 기준 A/B/C의 y0·y127 가장자리와 좌우 반전된 상대 가장자리 사이에서 발생한다. 차이 x 구간은 4–15,20–35,48–61,66–79,92–107,112–123(양 끝 포함)이다. 투명 틈이나 누락은 없고 확대 합성에서는 초록 무늬가 꺾인다. 일반적인 텍스처의 인접 색 차이만으로 대규모 접합 오류라고 확정하지 않는다. **기존 flipX를 보호한 상태의 미술 승인 항목**이며, 불연속 무늬가 거슬리면 잔디 3종 가장자리를 반전에도 이어지도록 납품 보완한다. 이번에 flipX 제거·PNG 수정으로 우회하지 않았다.

길은 현재 각 행 중심이 1 world unit씩 옮겨지는 곳에서 직각 계단형 변곡을 만든다. Left/Right 역매핑은 없다. 갈색 길 가장자리의 폭209px와 128px 어긋난 행 사이81px 겹침을 색상 마스크(R>G>B)로 확인했다. 완전히 끊어진 길은 발견하지 못했다. 이 값은 시각 진단이고 Nav/Collider 테스트가 아니다. 계단형 곡선을 승인할지, 별도 코너 아트가 필요한지는 사용자 미술 검토로 남긴다. 추가 자산 수·새 배치를 임의 확정하지 않는다.

소품 7종 중 나무·덤불·Rock A·울타리 **6종은 로컬 StarterVillage v4 PNG와 RGBA 픽셀이 완전히 동일**하다. 나무/바위/덤불의 비투명 영역은 캔버스 안에 있고, 외곽 배치의 큰 나무도 합성 범위에서 잘리지 않는다. 울타리는 연결용 끝 픽셀이 의도적으로 이미지 경계에 닿는다. 바위·나무·덤불은 개별 소품이며 타일처럼 서로 이어 붙여야 하는 자산이 아니다.

### Rock B의 예외

기존 `rock_02_0`는 PPU100, Sprite rect(22,3,85,54), Pivot(42.5,27)이다. 기존 rect의 월드 크기0.85×0.54와 신규128px 캔버스(PPU128)의 1×1은 다르다. 신규 비투명 bbox(9,44,119,109)의 크기110×65px=약0.859×0.508world unit라 가시 크기는 비슷하지만 중심과 지면 접점은 동일하지 않다. 예를 들어 기존 rect 아래끝은 중심 대비-0.27, 신규 비투명 아래끝은 약-0.352world unit다(기존 실제 alpha 아래끝과 동일하다는 뜻은 아님).

통합 시 RockNorth 위치(4.5,4.4)와 Collider size(0.4,0.22), offset(0,-0.22)를 보존하고 실제 렌더의 지면 접점·Collider 대응을 확인해야 한다. 363개를 하나의 규격으로 무조건 교체한 뒤 완료 처리하지 않는다. 새 PNG를 PPU100으로 단순 적용하면 캔버스가1.28×1.28로 커지므로 Manifest 권장128을 기본으로 검토한다. 지금 PNG 재작업 필요로 판정한 것은 아니다.

## 스타일·기능 보존 가능성

승인 마을의 녹색·갈색 팔레트와 동일한 나무/덤불/울타리가 이어지므로 마을 바로 바깥 초원이라는 연속성이 높다. 잔디 B의 꽃, C의 풀 무리가 황량한 단색보다 야외 초원 느낌을 준다. 무료 원본의 형태를 벗어난 프로젝트용 아트 구성은 확인되지만 독창성의 정도와 미술 호불호는 사용자 승인 범위다. 넓은 화면에서 잔디 패턴 밀도와 반복감, 길 변곡, 바위·나무의 체감 크기를 확인해야 한다.

현재 시리얼화 BoxCollider2D는21개(나무5·바위2·울타리8·외곽5·마을 Trigger1). WorldBounds21×15, 북쪽 Spawn(0,4.7), 마을 귀환 Trigger(0,6.65)를 유지할 수 있다. 남쪽 연결은 Scene의 이전 남쪽 안내/Boundary만 보고 판단하지 않고 `FieldConnectionInstaller` 및 `Field01_SouthToField02.asset`을 확인했다. 실제 데이터의 Spawn(0,-4.6), Trigger(0,-6.8), size(3,1), 목적지Field_02/Spawn_From_Field01도 변경하지 않았다.

몬스터 Resources 데이터는 초원슬라임 **5개**, 독벌 **3개**로 총8개이며 초기 생성기의 초원슬라임3개 설명보다 현재 구현을 우선한다. 위치·activityRadius·respawn·정의 참조는 보존한다. 출입구 안전 구역도 기존 데이터를 유지한다. 실제 몬스터/Quest Marker/이름표는 런타임 설치되므로 환경 전용 제공 Preview만으로 가독성 PASS를 주장하지 않는다.

주요 현장 기준: Main02 조사(0.4,1)·흔적(5.1,-1.2), Main03 태온(7.5,-0.8)·다음 단서(8.2,4.6), Main04 미엘(7.2,5.5). 특히 북동쪽 나무 주변의 Main03/04 인물·조사 목표에 대해 사용자 실제 화면 검토와 후속 격리 Runtime QA가 필요하다. 원본 Collider/좌표를 유지하면 물리 동선의 변경은 피할 수 있지만, 새 실루엣과 통과 가능한 공간의 인상 차이는 실물 검증 대상이다. Main05 귀환/Main06 진입 및 기존 Save/Continue 로직도 손대지 않았다.

생성기는 아직 ThirdParty 원본 Sprite를 사용한다. 후속 통합 때 전용 Art 경로·신규 GUID·명시적 대응 로더와 재생성 정책을 함께 다뤄야 한다. Scene Sprite만 바꾸면 다음 GenerateField01에서 구형 아트로 돌아갈 수 있다. 이 검수 단계에서는 생성기/공용 Sprite/원본 meta/GUID를 바꾸지 않았다.

## 검수용 이미지

아래는 Pillow 오프라인 합성으로, Unity RenderTexture/Game View 캡처가 아니다. 기존 환경의 Source PNG와 Editor Sprite rect/PPU를 사용한 원본 비교는 현재 LOCAL 원본 참조 그대로다. 카메라·광원·플레이어·동적 NPC/몬스터·Quest Marker·실제 가림 처리를 재현하지 않는다.

- [현재 LOCAL 원본 / 신규 가상 교체 비교](Evidence/Field01ArtV1_20261010/before_after.png)
- [현재 Scene 좌표의 전체 합성](Evidence/Field01ArtV1_20261010/local_scene_composite.png)
- [Collider·Spawn·Quest·몬스터 기준점 표시](Evidence/Field01ArtV1_20261010/collider_quest_overlay.png) — 빨강은 시리얼화 Collider, 노랑은 현장 기준점. Runtime Collider 전부를 재현한 그림은 아님.
- [길 변곡 확대](Evidence/Field01ArtV1_20261010/winding_road.png)
- [잔디 반전 상하 경계 8배 확대](Evidence/Field01ArtV1_20261010/grass_flip_seam.png)
- [납품 전체 ContactSheet](Evidence/Field01ArtV1_20261010/LL_C1_F01_ActualAssets_ContactSheet_v1.png)
- [납품 StarterVillage 연속성 비교](Evidence/Field01ArtV1_20261010/LL_C1_F01_StarterVillage_v4_Continuity_v1.png)

## 후속 승인·QA

1. 사용자 미술 검토: 잔디 밀도/반전 경계, 길 변곡, 마을 색감 연속성, 소품 크기. 필요한 경우 해당 PNG 3+2종의 보완 요청 범위를 먼저 확정한다.
2. 승인 후 별도 통합: 전용 환경 경로, 신규 GUID,363개 Sprite 참조만 명시적 대응. 원본 ThirdParty/meta/GUID·공용 Field Sprite는 보존한다. Rock B 예외를 확인한다.
3. 격리 프로젝트에서 Importer/레이어/누락 Sprite·Script, 실제 RenderTexture, Collider·Spawn·북/남 왕복, 몬스터8·Main02/03/04 동적 목표 가독성, Main05/06과 Save/Continue 회귀를 검사한다. 원본 Editor 포커스를 바꾸지 않는다.
4. 본 검수의 Runtime/전투/입력/Save 회귀 결과는 **NOT_VERIFIED**다. 신규 Import 없이 기존 기능 PASS를 재주장하지 않는다. 기존 Unity Search 내부 예외2건은 별도 미해결 QA 상태로 유지한다.

## 보호·Git

Assets·Packages·ProjectSettings·프로젝트 UserData의 검수 전 **3536개 파일 SHA-256**을 비교해 변경/삭제0을 확인했다([preservation.json](Evidence/Field01ArtV1_20261010/preservation.json)). 원본 ZIP 해시 불변. Scene/Prefab/Generator/Save·설정·기존 아트/음원 불변. 기존 미커밋 Git335항목을 보호한다.

Commit 범위는 본 보고서·검수 이미지/JSON·CURRENT_STATUS의 이번 항목뿐이다. CURRENT_STATUS의 기존 미커밋 MCP 기록8줄은 working tree에 그대로 두고 이번 Commit에는 포함하지 않는다. 검수 스크립트/중간 데이터는 무시된 Temp에만 존재하며 실행 코드/자산으로 등록하지 않았다. GitHub Push 하지 않는다.
