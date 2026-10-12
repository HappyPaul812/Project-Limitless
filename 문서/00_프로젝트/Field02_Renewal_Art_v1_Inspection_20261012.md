# Field_02 리뉴얼 아트 v1 검수 — 2026-10-12

**ART_REVIEW_PENDING / 기술 PARTIAL / 원본 Unity 영구 적용 0.** 납품 완료와 사용자 최종 미술 승인은 구별한다. 원본 ZIP과 승인된 마을 v4·Field01, Scene·Theme·Generator·Prefab·Save·WAV·기존 GUID는 변경하지 않았다.

## 원본 및 실제 검사

원본 `F:\Downloads\Limitless_Field02_Renewal_Art_v1.zip`, SHA256 `06d191e64fe9d146d458573588c00911666c4677e504c4b888a8d0d8d63ccc8d`. ZIP 30항목, PNG25개(실사용 후보17 / Preview6 / QA2), CRC 오류0, PNG 전체 디코딩 성공. Manifest17개 파일·크기·SHA256 모두 일치. RGBA17개; 바닥5개는 불투명, 나머지 alpha 범위·bbox는 [기계 검사](Evidence/Field02Renewal_v1_20261012/zip_audit.json)에 기록했다. 나무255×256·128×156, 나머지128×128. 외부 자산 원본 변경0; 독창성/권리 귀속은 납품자의 'original' 선언만 있으며 별도 권리 증빙은 NOT_VERIFIED.

Unity6000.6.5f1, 기존 `Temp/Field01RenewalQA/Client` 재사용, 숨김 Batch, 별도 Save/Settings, 스피커0. 원본 Editor PID31832 유지. 신규17개는 **격리 프로젝트에만** Single/PPU128/Center/Point/무압축/Mipmap Off/NPOT None로 Import했다. 원본 프로젝트 Assets에 후보를 복사하지 않았다.

실제 Scene 환경363개 + `Field02_Forest` 테마의 TreeWest 복제6·BushWest 복제4 = 후보 참조373개. [개별 대응표](Evidence/Field02Renewal_v1_20261012/sprite_mapping_373.csv), [원래 Runtime](Evidence/Field02Renewal_v1_20261012/runtime_before.json), [후보 Runtime](Evidence/Field02Renewal_v1_20261012/runtime_candidate.json). 373개의 위치·Scale·FlipX·Sorting·Tint·Collider 값 모두 교체 전후 동일. 후보 교체는 Play 메모리에만 적용, Scene 저장/재생성0.

| 기존 Sprite | Scene+Runtime 수 | 대응 후보 | Rect 변화(px) |
|---|---:|---|---|
| `bush_01` | 6 | `Props/LL_C1_F02_Bush_A_128_v1.png` | 128×128 → 128×128 |
| `bush_02` | 1 | `Props/LL_C1_F02_Bush_B_128_v1.png` | 128×128 → 128×128 |
| `fence_tiles_2_2` | 8 | `Props/LL_C1_F02_Fence_MossWood_128_v1.png` | 128×128 → 128×128 |
| `rock_01` | 1 | `Props/LL_C1_F02_Rock_Moss_A_128_v1.png` | 128×128 → 128×128 |
| `rock_02_0` | 1 | `Props/LL_C1_F02_Rock_Moss_B_128_v1.png` | 85×54 → 128×128 |
| `tiles_grass_4_0` | 105 | `Ground/LL_C1_F02_ForestFloor_A_128_v1.png` | 128×128 → 128×128 |
| `tiles_grass_4_4` | 15 | `Ground/LL_C1_F02_Path_Left_128_v1.png` | 128×128 → 128×128 |
| `tiles_grass_5_0` | 106 | `Ground/LL_C1_F02_ForestFloor_B_128_v1.png` | 128×128 → 128×128 |
| `tiles_grass_5_4` | 15 | `Ground/LL_C1_F02_Path_Right_128_v1.png` | 128×128 → 128×128 |
| `tiles_grass_6_0` | 104 | `Ground/LL_C1_F02_ForestFloor_C_128_v1.png` | 128×128 → 128×128 |
| `tree_big` | 2 | `Props/LL_C1_F02_Tree_Big_255x256_v1.png` | 255×256 → 255×256 |
| `tree_medium` | 9 | `Props/LL_C1_F02_Tree_Medium_128x156_v1.png` | 128×156 → 128×156 |

## 미술 및 배치 검토

![현재/후보 실제 RenderTexture 비교](Evidence/Field02Renewal_v1_20261012/before_after_comparison.png)

위 세 화면은 실제 Unity RenderTexture이며 첫 화면의 원래 배치에서 Sprite만 바꿨다. 오른쪽은 바닥만 흰색으로 비교한 **제안 화면**이다. `FieldThemeInstaller`가 기존 바닥에 `(0.62,0.76,0.60)`을 곱하므로 이미 그늘색인 새 아트가 더 어두워진다. 원본 Theme를 바꾸지 않았다. 사용자가 기존 Tint 유지/후보색 기준 중 선택해야 한다.

납품 Contact Sheet와 TileRepeat는 오프라인 합성이며 Unity 캡처가 아니다. [Contact Sheet](Evidence/Field02Renewal_v1_20261012/package_contact_sheet.png), [납품 타일 반복](Evidence/Field02Renewal_v1_20261012/package_tile_repeat.png). 현장 화면에서 길 중앙 접합·울타리·나무/바위 크기를 확인했다. 기존 바닥의 랜덤 FlipX를 유지하므로 반복감/경계 시임은 최종 미술 승인 대상이며 수치 검사만으로 '완벽한 시임'이라 판정하지 않는다. 큰 나무·중간 나무의 실제 Rect/PPU는 대응표대로 유지된다. 바위B의 원본 Slice와 신규128px 사이의 크기 변화도 대응표·Runtime 확대를 기준으로 최종 승인해야 한다.

17개 중 직접 치환 가능한 것은12종이다. Mud Patch·Root·Branch·Story helper2개는 기존 환경 Sprite의 1:1 대상이 없다. 기존 Main06 흔적과 Main07 절차적 바퀴자국을 덮어쓰지 않았다. 다섯 보조 자산 신규 배치는 별도 승인 후 검토하며 Scene 배치 변경으로 숨겨 적용하지 않는다.

## 게임 구조 보호 및 검증 한계

![실제 Runtime 위 좌표/Collider 설명 Overlay](Evidence/Field02Renewal_v1_20261012/quest_collider_overlay.png)

Overlay의 노란 좌표 표시는 설명 합성이다. 실제 Field bounds21×15, 북 Spawn(0,4.6)/Transition(0,6.8), 남 Spawn(0,-4.6)/Transition(0,-6.8); 방향별 Field01↔02↔03 Stable SpawnID 대응은 LOCAL connection4개로 확인했다. 북 입구 안전지대 center(0,5.65),radius2.2. 서로 다른 Spawn/Trigger 위치 유지. WorldBounds·경계·콜라이더 기존 구조 보존.

Main06 조사(0.2,-1.6), Main07 바퀴 자국(2.7,-1.2), 여행자(4.4,0.2), 폴(6.5,-2.35)는 LOCAL 코드와 Manifest 일치. 네 지점 환경 Collider probe 통과. Main06 실제 Marker가 후보 바닥에서도 읽히는 RenderTexture 확보. Main07은 활성 목표별로 폴이 최초 지점→여행자 옆(+0.8,0), 미엘/폴 복귀 표식으로 바뀌는 기존 코드 확인; 이번 캡처는 Main06 fixture이므로 Main07 활성 NPC/휠체어 전 동선/대사·E/F 전체 플레이는 NOT_VERIFIED. 기존 Field01 복구 QA의 Main06 실제 조사·왕복 PASS는 재사용하며 본 검수에서 재실행하지 않았다.

숲거미4·맹독뱀2 실제 초기 스폰6 확인. SpawnDefinition6개·ID·Monster GUID·activityRadius·respawnSeconds30·전투/드롭 설정을 변경하지 않았다. Field02 몬스터 승리 후30초 재스폰과 Main07 StoryBattle 전체는 이번 검수에서 NOT_VERIFIED. 양방향 실제 Trigger 왕복·출입구 전체 도보 접근도 정적 연결 검증과 구별해 NOT_VERIFIED. 신규 미술 영구 통합 이후 관련 Runtime 회귀가 필요하다.

Compile 오류0, 환경 후보 참조 Missing Sprite0, Scene Missing Script0. [Batch assertion](Evidence/Field02Renewal_v1_20261012/checks.txt), [Console](Evidence/Field02Renewal_v1_20261012/console_errors.txt). 기존 Unity Search 내부 예외가 남아 기술 전체는 PARTIAL이며 게임 기능 신규 오류0. QA 완료를 미술 승인으로 간주하지 않는다.

## 승인 이후 복붙용 통합 지시

> Field02 v1 미술 승인 범위를 먼저 확인하고 이 보고서의373개 대응표로 신규12종 Sprite-only 통합을 진행한다. Scene363개 및 Theme 복제10개, 원래 위치·Scale·Flip·Sorting·Collider·퀘스트/몬스터/Save를 보존한다. Tint 선택은 승인 내용대로 적용한다. 나머지 보조5개는 별도 배치 승인이 없으면 추가하지 않는다. 기존 meta/GUID를 변경하거나 Scene을 재생성하지 않는다. 원본 Editor 무접촉, 기존 격리 프로젝트에서 Main06/07·폴·Marker·휠체어 접근·몬스터6·30초 리스폰·Field01/03 왕복·Save/Continue를 검사하고 PARTIAL/FAIL을 숨기지 않는다. 선택적 Commit만, Push 금지.
