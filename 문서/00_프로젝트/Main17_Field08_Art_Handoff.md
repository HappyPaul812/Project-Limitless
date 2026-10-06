# Main17 Field08 Art Handoff

상태: **ART_WAITING_EXTERNAL**. 외부 아트 담당 세션용이며 현재 Placeholder를 완성 아트로 취급하지 않는다. 2026-10-06 실제 Field_08_RedRift Additive 조회, SpriteImporter 및 Renderer/Transform에서 추출했다. 원본 PNG와 ThirdParty는 수정하지 않는다.

## 현재 Scene 규격과 교체 슬롯

Scene: Assets/_Project/Scenes/Field_08_RedRift.unity. Bounds 중심(0,0), 크기21×15. 동쪽 출구, 서쪽 경계 닫힘. 필수 조사 좌표는 Main17 정식 설계의12목표 표를 따른다. 깊은 틈 중앙은 통과 불가, 북 y4.5/남 y-3.5 우회 유지.

현재 새 도형44개는 기본 Quad Mesh이고 **PPU·Sprite Pivot·원본 이미지 픽셀 크기가 존재하지 않는다**. Mesh UV0~1/Transform 중심/4개의 공유 Material을 사용한다. 정확한 제작 요구는 아래 world unit footprint다. Quad 크기를 존재하지 않는 PNG 규격으로 오인하면 안 된다.

| 용도 / 객체 | 수 | 실제 world footprint | 위치 | Sorting Default | Collider | Animation | 현재 Material |
| --- | ---: | --- | --- | ---: | --- | --- | --- |
| 붉은 지면 / Ground | 1 | 21×15 | (0,0) | -20 | 없음 | 없음 | RedRift_Ground |
| 깊은 틈 / DeepCrack | 1 | 0.7×7 | (-5,0.5) | -10 | 기존 BoxCollider2D 유지 | 없음 | RedRift_Crack |
| 붉은 틈 빛 / InnerGlow | 1 | 0.1×6.5 | (-4.9,0.5) | -9 | 없음 | 없음 | RedRift_Glow |
| 분기 균열 / Branch0~5 | 6 | 1.3×0.08 | 중앙, 25° 또는 -30° | -8 | 없음 | 없음 | RedRift_Crack |
| 재 / Ash0~23 | 24 | 0.15×0.07 | 분산, 전체 배치는 아래 CSV | -7 | 없음 | 기존 Main16AshPulse 수평 ±0.1unit/1.7rad/s | RedRift_Ash |
| 동물 발자국 / Track0~7 | 8 | 0.1×0.16 | 북쪽 (3,3)에서(0.2,2.2) | -6 | 없음 | 없음 | RedRift_Ash |
| 흑요석성 암석 / Obsidian0~2 | 3 | 0.7×1.5 | (-7/-6.4/-5.8,4.8), 25/45/65° | -6 | 없음 | 없음 | RedRift_Crack |

Material 경로: Assets/_Project/Resources/Chapter2/RedRift_{Ground,Crack,Glow,Ash}.mat. Shader Sprites/Default. 기존 색 Tint가 곱해지므로 최종 색상은 Texture+Tint 조합으로 확인해야 한다. Material Texture 교체만으로 공유 슬롯의 구조/충돌/Save는 유지된다. 단 Crack Material은 틈·분기·암석이 공유하므로 서로 다른 그림을 쓰려면 각 Renderer의 sharedMaterial을 구별된 프로젝트 Material로 배정한다. 별도 Scene 재생성은 필요 없다.

## Sprite 방식으로 교체하는 경우의 실제 기준

Scene의 비활성 바닥 AshSoil 타일은 `tiles_grass.png`에서 rect(512,512,128,128), **PPU128**, pivotPx(64,64)=정규화(0.5,0.5), AlphaIsTransparency=true, Default:-10이다. 실제 1unit 타일의 픽셀 규격은128×128이다. 반복 가능한 붉은 지면을 이 방식으로 교체할 경우 128×128/PPU128/중앙 Pivot을 그대로 사용할 수 있다. 상하좌우 경계가 이어져야 하고 풀/초록 테두리가 반복되지 않아야 한다. 현재 Ground Quad는 하나의 Texture UV0~1을 늘려 표시하며 자동 타일 반복 셰이더는 없다. 타일 이미지 전달만으로 자동 반복된다고 가정하지 않는다.

활성 RockWest는 rock_01.png rect128×128/PPU128/pivot64,64/Alpha=true/Default:3. RockNorth는 rock_02.png rect85×54/PPU100/pivot42.5,27/Alpha=true/Default:3이다. 두 Props의 PPU는 서로 다르며 모두128로 추정하지 않는다. 활성 AshCrack_0~7은 기존128타일/PPU128/중앙Pivot/Alpha=true/Default:1이다. 외부 아트 도착 후 새 균열 그림으로 교체하는 대상이다.

PNG는 지면 전체 채움과 투명 배경 Prop을 구분한다. Prop/균열/재/발자국/빛에는 투명 영역이 필요하며 외곽 프린지가 없어야 한다. 지면은 빈 투명 영역으로 맵 밖 검은 영역을 드러내지 않는다. Quad Texture 납품의 pixel width/height·PPU는 현 Scene에 없으므로 이번 문서가 숫자를 새로 확정하지 않는다. 위 world footprint와 기존 타일128px/unit을 아트 담당에게 전달하고 납품 해상도를 합의한다. Sprite 교체 시 PPU는 납품 픽셀/목표 unit으로 실제 계산하며 현재 transform 중심을 유지한다.

예상 신규 경로: Assets/_Project/Art/Environment/Chapter2/RedRift/ 및 프로젝트 전용 Material. **이 경로는 납품 예정 위치이며 아직 PNG가 없다.** Sprite/Material만 바꾸고 Boundary/Exit/Spawn/조사 좌표/Collider/Actor를 유지한다. 새 Sprite Pivot이 발 밑이거나 비중앙이면 시각 자식에만 offset을 적용하고 충돌 root는 이동하지 않는다.

신규 Monster/Elite Art는 Main17 필수가 아니다. 현재 공식 균열도마뱀/화열딱정벌레를 유지한다. 작열 감시자·Overheat·Elite는 Main18 후보이며 이번 납품 범위에 넣지 않는다.

정확한 전체44개 좌표·크기·회전·Sorting·Collider·Material: Main17_Field08_Geometry.txt. 기존 Sprite/import 원본은 Main17_Field08_SpriteImports.txt. 이 두 추출 파일을 함께 인계한다.

## 기존 활성 Prop 충돌 보존

RockWest(-5.8,-2.2)·RockNorth(4.5,4.4)는 scale1, 활성 비Trigger BoxCollider2D를 가진다. CanyonLip_0/1은(-9.1,±1.9), scale1, 비Trigger BoxCollider2D로 서쪽 접근 경계를 구성한다. AshCrack8개는 scale(0.7,0.08), Collider 없음이다. 실제 좌표/scale는 Main17_Field08_ActiveSpritePlacement.txt를 따른다. Sprite 교체 시 원본 collider를 지우거나 새 sprite outline로 자동 덮어쓰지 않는다. 바닥 타일 importer filterMode=0(Point), Sprite textureType=8을 확인했다.
