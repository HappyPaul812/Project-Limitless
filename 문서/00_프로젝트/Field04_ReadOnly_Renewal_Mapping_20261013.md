# Field04 기존 역할·리뉴얼 대응표 — 2026-10-13

**ASSET_AUDIT / STATIC_COMPLETE / RUNTIME_NOT_VERIFIED / 신규 제작·배치·적용0.** 저장 Scene과 런타임 설치 코드/데이터를 분리해 조사했습니다. 리뉴얼 담당의 환경 제작 입력이며 미술 승인이나 정식 통합 완료가 아닙니다.

## 기준·확인 범위

- 원본: `Unity/Client/Assets/_Project/Scenes/Field_04_WesternBorder.unity`.
- GitHub main `eca6f1e5ceb42e9d6c88b53368bc109280a547b8`, 로컬 HEAD `509886f85b6b50deb3f2b69ad7d1f1cc85945157`. 병합 없이 최신 main의 업무지시/Chapter2 리뉴얼 로드맵을 읽었습니다. Scene Git blob은 원격/로컬 모두 `b9f7219864b8380219e7326fdb78cfab1595e223`입니다.
- 실제 **로컬 meta**를 기준으로 Slice/fileID/PPU/Pivot/Filter를 대조했습니다. 사용자 ThirdParty meta 미커밋 변경이 있으므로 GitHub importer나 실제 원본 Editor Import 완료와 같다고 가정하지 않습니다. Renderer의 GUID만으로 slice를 식별하지 않고 **GUID+signed fileID**를 함께 기록합니다.
- Unity 버전 ProjectVersion `6000.6.5f1`. Unity MCP 읽기 resource에서 원본 연결1개, Bootstrap/Edit Mode, is_focused=false, compiling/updating=false를 확인했습니다. Scene load/선택/Import/Play/카메라 조작/OS 포커스 조작0. 현재 열린 Scene을 Field04로 바꾸지 않았습니다. 저장 Scene YAML과 코드의 정적 조사이며 실제 Field04 런타임 hierarchy/화면은 미관측입니다.
- 지침: AGENTS/CODEX_WORKFLOW/팀 운영/인계 및 [Main12~14 정본](../03_스토리/Chapter2_도입부_Main12_14.md). 최신 원격 `LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md`도 확인했습니다. 이 로드맵은 로컬에 없으므로 임의 복사/병합하지 않았습니다.

## 대응표 사용법

1. [저장 Scene 374개 오브젝트 CSV](Field04_Serialized_Object_Mapping_20261013.csv): GameObject/fileID/계층/좌표/Scale/Renderer/Sprite GUID와 fileID/PNG/전체 크기/실제 Slice rect/PPU/Pivot/Sorting/Color/Collider/기능 컴포넌트. Renderer363개와 Collider21개를 각각 포함합니다. 부모 Transform의 위치·Scale을 합성했고 저장된 회전은 모두 identity여서 회전 누락으로 좌표를 왜곡하지 않습니다. WorldPositionDerived는 계산값입니다.
2. [런타임 보호 18행 CSV](Field04_Runtime_Protection_Mapping_20261013.csv): 현행 코드/Resources가 설치·제거하는 Exit/Spawn/Quest/NPC/몬스터/벽/Theme. **실행 관측 결과가 아니라 코드/데이터 근거**입니다.
3. 환경 그림은 아래 역할별 후보로 제작할 수 있지만, 현행 Transform·Collider·Stable ID·게임 로직과 사용자 승인 캐릭터는 보호합니다. 1 Sprite GUID 전역 치환이나 기존 Generator 실행은 이 조사 계약에 포함되지 않습니다.

## 환경 Renderer·Prefab 요약

저장 Scene은 GameObject374, Transform375(플레이어 stripped 참조 포함), SpriteRenderer363, BoxCollider2D21, MonoBehaviour4, Camera1, PrefabInstance1입니다. **환경 Tilemap 없음**; Grass/Road는 개별 SpriteRenderer입니다. 환경 Renderer363 = 바닥315 + 길30 + 나무5 + 수풀3 + 바위2 + 울타리8. 환경 Prefab instance는 없고 Player prefab만1개입니다.

| 역할 / slice | 개수 | GUID | fileID | 로컬 PPU / 원래 그림 단위 | SortingOrder | 제작 입력 |
|---|---:|---|---|---|---:|---|
| Grass / tiles_grass_4_0 | 105 | ecafa1446d0264141bb7b01e4005beff | 405149151 | 128 / 1×1 | -10 | 남은 녹지·마른 풀 반복 후보 |
| Grass / tiles_grass_5_0 | 106 | 위와 동일 | 1271049111 | 128 / 1×1 | -10 | 바닥 변형 후보 |
| Grass / tiles_grass_6_0 | 104 | 위와 동일 | 1606921373 | 128 / 1×1 | -10 | 바닥 변형 후보 |
| RoadLeft / tiles_grass_4_4 | 15 | 위와 동일 | 278596618 | 128 / 1×1 | -8 | 길 왼쪽 반복 후보 |
| RoadRight / tiles_grass_5_4 | 15 | 위와 동일 | 1662121770 | 128 / 1×1 | -8 | 길 오른쪽 반복 후보 |
| Tree medium | 3 | 5b888cd38e2bf8543a27554e427d128b | 21300000 | 128 / 1×1.21875 | 4 | 살아 있는 나무·마른 잎 공존 |
| Tree big | 2 | d06207b6940ba73478971dde7147ae92 | 21300000 | 128 / 1.9921875×2 | 4 | 큰 나무 실루엣/접지 보호 |
| Bush01 | 2 | a7170a8f8df7fc347a8e4861ba68821b | 21300000 | 128 / 1×1 | 2 | 잔존 녹지/마른 수풀 후보 |
| Bush02 | 1 | ccf732b59af6d4d4cbf48b806179440b | 21300000 | 128 / 1×1 | 2 | 수풀 변형 후보 |
| RockWest / rock01 | 1 | 6b5b784b46803054195f9cc3b2961699 | 21300000 | 128 / 1×1 | 3 | 바위 후보 |
| RockNorth / rock02_0 | 1 | 5b951bd26f9af3849a2fe92928a0c645 | -1321756614084088994 | **100 / .85×.54** | 3 | slice85×54, 전체PNG와 구분 |
| Fence / fence_tiles_2_2 | 8 | d1228d04780ccff429a9d5042442017d | 1892116053 | 128 / 1×1 | 2 | 울타리 반복 후보 |

위 경로들은 `Assets/ThirdParty/Schwarnhild/BasicHandDrawn/`의 assets/tiles 원본입니다. **ThirdParty 수정 금지**. 프로젝트 수정본은 후속 승인 작업에서 `_Project`에 따로 등록해야 합니다. 이 조사로 기존 Field03 Rock B의 보류 원본을 바꾸지 않습니다.

모든 환경 localScale/worldScale은(1,1,1), DrawMode0(Simple), SortingLayerID0입니다. 별도 tile size를 사용하는 Tiled 모드가 아닙니다. 기본 filter0(Point)/Clamp/AlphaIsTransparency이며 RockNorth만 filter1(Bilinear)/PPU100입니다. Slice alignment0은 Center입니다. Slice raw pivot(0,0) 필드는 실제 center 정렬과 구분해 CSV에 둘 다 적었습니다. native units는 slice 픽셀÷PPU이며 alpha 실루엣 bounds나 실제 GPU importer 결과가 아닙니다.

바닥은 x=-10~10, y=-7~7의21×15 개별 셀315개입니다. RoadLeft/Right는 y=-7~7 각15개이며 정확한 x·색은 CSV를 따릅니다. **직렬화 색 위에 Runtime groundTint(0.69,0.61,0.42,1)가 곱해집니다.** 새 그림이 이미 황갈색이면 이 tint가 중복되어 어두워질 수 있으므로 후속 격리 Preview에서 확인할 사항입니다. 이번에 tint/설정을 변경하지 않았습니다. Runtime 추가 tree/bush 배열은 비어 있어 Theme 복제0이 예상됩니다.

## 장식 접지·Collider 보호

| 앵커 | 좌표 | BoxCollider offset / size | 경계 |
|---|---|---|---|
| TreeWest | (-8.5,0) | (0,-.55) / (.45,.28) | 서쪽 Spawn(-8,0) 인접; 새 투명 여백/수관이 플레이어와 통로를 가리지 않아야 함 |
| TreeNorthWest | (-8,5) | (0,-.75) / (.5,.3) | 상단 장식 |
| TreeNorthEast | (7.5,5.2) | (0,-.55) / (.45,.28) | 상단 장식 |
| TreeEast | (8.5,-.5) | (0,-.75) / (.5,.3) | 동쪽 Spawn/진입 동선 인접 |
| TreeSouthWest | (-7.5,-5) | (0,-.55) / (.45,.28) | 하단 장식 |
| RockWest | (-5.8,-2.2) | (0,-.25) / (.45,.25) | 기존 장애물 유지 |
| RockNorth | (4.5,4.4) | (0,-.22) / (.4,.22) | 현재 PPU100·접지 기준 별도 |
| BushWest/East/South | (-5.5,2.2)/(5.7,2.8)/(4.8,-4.4) | 없음 | 그림만 후보; 새 장애물 추가 금지 |
| Fence 상단4 | x=-4,-3,3,4 / y=5.7 | 각(0,-.35) / (.9,.2) | 현행 울타리 틈 보호 |
| Fence 하단4 | x=-6,-5,5,6 / y=-5.8 | 각(0,-.35) / (.9,.2) | 현행 틈 보호 |

저장 Collider21 = 장식15(나무5+바위2+울타리8) + 외곽5 + 북쪽 Trigger1. Player prefab 내부 Collider는 별도이며 이21에 포함하지 않습니다. Player prefab GUID `624c274a3d8efd745befd82f4df249af` → `Assets/_Project/Prefabs/PlayerPlaceholder.prefab`; Scene override 위치(0,4.7,0). 사용자 수정 prefab/외형/애니메이션 전체 보호입니다.

## 실제 입구·Bounds와 저장 Scene 잔재 구분

- WorldBounds center(0,0), size(21,15), x[-10.5,10.5], y[-7.5,7.5]. 저장 Camera orthographic5, followSpeed8, WorldBounds 참조 유지. CameraFollow는 aspect×orthographic size로 viewport 전체를 clamp하고 넓은 aspect에서는 size도 줄입니다. 실제 해상도별 검증은 이번 미실행입니다.
- 저장 북쪽 Entrance_To_StarterVillage(0,6.65), trigger size(2.5,.9) 및 Spawn_From_StarterVillage(0,4.7)는 **현재 연결 데이터의 replaceExistingSceneConnections=1로 Scene 로드 때 제거 대상**입니다. 이를 현행 북쪽 출구로 리뉴얼하지 않습니다.
- 저장 BoundaryTopLeft/Right는(-6.5,7.7)/(6.5,7.7), size(9,1)로 중앙 **x[-2,2] 틈**이 남습니다. 북쪽 Trigger 제거 뒤에도 이 외곽 틈은 코드상 남을 것으로 보입니다. 현행 규칙 “실제 Exit만 통과”와 대조하면 **후속 기술 QA 확인 필요**입니다. 이동 제한/Player clamp와 실제 collider 통과 결과는 미검증이므로 북문 버그로 단정하지 않습니다. 이번 아트 조사에서 벽/문을 임의 추가하지 않았습니다.
- Bottom(0,-7.7) size(22,1), FutureSouthExit_TODO(0,-6.6)는 기능 없는 이름만 존재; 남쪽 출구를 새로 만들지 않습니다.
- 저장 좌우 BoundaryLeft/Right(±10.7,0), size(1,16)는 Chapter2IntroFlow.OpenSide에서 제거하고 (±10.5,±4.5) size(.3,6) 각2개로 대체합니다. 중앙 y[-1.5,1.5]만 동서 출구 Opening입니다.
- 실제 데이터 입구: 동쪽 Spawn_From_Field03(8,0), Trigger(10.7,0) size(1,3) → Field_03/Spawn_From_Field04. 서쪽 Spawn_From_Arbel(-8,0), Trigger(-10.7,0) size(1,3) → Arbel/Spawn_From_Field04. 각 Spawn↔Trigger 중심거리2.7이며 trigger half-width .5와 분리되어 있습니다. Player Collider 전체/휠체어 통과는 후속 런타임 확인 사항입니다.

## Quest·몬스터·서부 첫 방문 동선

**보호 동선:** Field03 서쪽 Gate → Field04 동쪽 Spawn(8,0) → Main12 도착(7,0) → Main13 토양(5,-2) → 물길(2,2) → 세린(-1,0) → Story 전투(-3,0) → 서쪽 Exit(-10.7,0) → Arbel 도착(7,0) → 레온. exact target/순서/상호작용 반경/전투 승리 조건을 유지합니다.

| 보호 대상 | 위치/현행 기능 | 미술 경계 |
|---|---|---|
| field04_west_entry | (7,0), Main12 마지막 도착, distance<1.7 | 도착/Navigation 정보와 동쪽 진입 시야 보호 |
| field04_main13_dry_soil | (5,-2), Main13목표1, E/F/패드 buttonSouth, distance≤1.7 | 현행 GroundClue Quad1.3×.75, Sprites/Default·Sorting4. 마른 땅 그림 후보지만 기능 parent/label 보존 |
| field04_main13_shallow_stream | (2,2), Main13목표2 | **현행 별도 물길 Sprite 없음**, label/상호작용만 설치. 낮아진 물길 그림은 후속 표현 제안이며 새 지형/충돌/Quest 추가 아님 |
| field04-serin | (-1,0), Main13목표3, Resources/Chapter2/Serin Animator | 공식 세린 Sprite/Portrait/Animator·귀 장치·화자·대사·Marker 보호 |
| field04_main13_serin_encounter | (-3,0), Main13목표4, StoryEncounterReturns/Main13_SerinReturn | 일반 몬스터 스폰으로 중복 제작 금지. 현재 그을음들개 정의와 Story Battle·임시동행·복귀 좌표 보호 |
| field04_soot_hound | (-4,3), activityRadius1 | 일반 몬스터; respawn35초/Collider radius.4/Sorting3/공식 원본 보호 |
| field04_heatwind_hawk | (2,-3), activityRadius1 | 위와 같은 보호. 토양 조사 동선 인접; 새로운 강제 조우/차단 배치 금지 |

Runtime Quest Site의 표식은 목표 활성 및 Modal 상태에 따라 표시됩니다. Navigation label 높이1.3, SiteLabel 높이1, 세린 이름표 높이1.02 등 기존 가독성 영역을 그림으로 가리지 않아야 합니다. 세린 임시 동행은 Main13 목표 index3~4와 Main14 조건으로 재구성되고 영구명단/Save와 분리되는 현행 구조를 보존합니다.

Field04 전용 FieldEntranceSafetyZones 데이터는 텍스트 검색에서 발견하지 못했습니다. 따라서 새로운 안전 반경을 정본으로 만들지 않습니다. 위 활동 원과 출입구/Quest 경로의 실제 겹침·배회/조우 결과는 후속 격리 QA에서 확인합니다. 일반 스폰2와 Story 반환 정의1의 역할을 구분합니다.

## 리뉴얼 담당 제작 입력·보류

- 정본 미술 방향: 살아 있는 숲/남은 녹지·마른 잎/풀·황갈색 토양·줄어든 물길, 서쪽으로 갈수록 건조. **완전한 사막/화염 지역이나 원인 확정 그림으로 바꾸지 않습니다.** 새 지역 수/Tile ID/Quest/장애물은 확정하지 않습니다.
- 후보 환경군은 위 바닥3/길2/나무2/수풀2/바위2/울타리1의 기존 역할12개입니다. 자산 수12개 납품을 강제하는 기획은 아니며 변형/atlas 구성은 제작 제안으로 구분합니다. 토양/물길 Quest 표현은 Runtime 별도 역할입니다.
- Seam은 반복 타일 좌우/상하, 원크기 소품은 slice·PPU·pivot·alpha 접지·기존 Collider 대비를 확인해 납품해야 합니다. 그림 해상도를 일괄128로 정하면 big tree/medium tree/RockNorth 비율이 달라집니다. 1:1 GUID 교체만으로 안전한 리뉴얼을 보장하지 않습니다.
- 승인된 세린/Player/몬스터·다른 지역 아트·ThirdParty·TTS/BGM·Save는 제작 대상에서 제외합니다. BGM의 Field04 기존 GUID `8dc555228b79489a947ec8dfafa45f72`와 후속 재생 로직도 보호합니다. 재생/청취0입니다.
- 프로젝트 전용 Field04 리뉴얼 profile/generator는 이번 검색에서 확인되지 않았습니다. 공용 Field01SceneGenerator/기존 Installer가 존재하지만 실행하지 않았습니다. 새 적용 방식을 확정하지 않습니다.
- 납품 제안: `Limitless_Field04_Renewal_Art_v1.zip`, PNG/Manifest(역할·target path/fileID·hash·크기·PPU/Pivot 제안)/Contact Sheet/Seam·alpha·접지 QA/기존 대비 Preview/권리·출처/Codex 인계. Preview의 합성/실제 Unity 여부를 표시합니다. **실제 제작은 본 작업에서0**입니다.

## 검증·다음 순서

PASS(정적): Scene blob 일치,374 Object/363 Renderer/21 Collider 수집,8 texture GUID와12 역할/slice 그룹 해석,모든 환경 identity 회전/Scale1 확인, 코드/Resources의 기능 보호표 작성. [보호 기록](Codex_ReadOnly_Audit_Protection_20261013.md)에 작업 전후 hash와 상태를 별도로 기록합니다.

NOT_VERIFIED: Field04 실제 현재 Editor hierarchy/Play/Importer·GPU Sprite bounds/전체 Missing Reference/Console 회귀/몬스터 전투·35초/왕복·SaveContinue/휠체어 물리 경로/해상도별 Camera/실제 가독성·미술 승인. 신규 코드/Unity 파일 수정이 없어 이번 컴파일/Runtime 시험은 실행하지 않았습니다.

다음은 리뉴얼 담당의 후보 납품과 사용자 미술 검토입니다. 승인 후 별도 Codex 격리 QA에서 동서 이동·Main12/13·두 일반 몬스터·Story Battle·세린 임시동행·Continue·휠체어·북쪽 외곽 틈을 확인하고 최소 적용 방식을 정해야 합니다. 북쪽 경계 우려도 환경 적용과 구분해 보고합니다.

후임 시작 문구: “Field04 두 CSV와 이 보고서를 먼저 읽으세요. 저장 환경363과 Runtime 입구/Quest/몬스터는 별도 층입니다. 기존 공식 캐릭터/좌표/Collider/Save/음성 보호, 아트 미승인/적용0입니다. 북쪽 저장 출구는 Runtime 제거 대상이며 외곽 틈은 기술 QA 미검증입니다.”

Commit은 이 문서가 포함된 로컬 Git 이력/최종 보고로 확인합니다. GitHub Push0입니다.
