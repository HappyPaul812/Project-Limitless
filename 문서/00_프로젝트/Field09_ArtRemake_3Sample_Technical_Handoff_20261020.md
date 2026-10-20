# Field09 환경 미술 재제작 — 샘플3 기술 인계

2026-10-20 바울 대표 지시. **기존 v2 국소 보정 재납품 단계에서 새 환경 아트 샘플3 방향 확인으로 전환합니다.** 이 문서는 미술 스타일·맵 구조·퀘스트를 새로 확정하지 않습니다. 현재 상태: `SAMPLE3_AWAITING_DELIVERY / FULL_ENVIRONMENT_APPLY_HOLD`.

## 결함 원인 — 기존 증거로 1회 정리

- **PNG:** 두 지면의 외곽 한 줄이 같아도 경계 주변의 명암·무늬가 연속적이지 않아 반복띠가 남았습니다. v2는 띠가 가늘어졌지만 제거되지 않았습니다. 소품의 흰/회색 직선·점선도 PNG RGBA에 실제 존재합니다.
- **Tiled/Overlay:** 4×4 유닛 타일이 넓은 면에 반복되며 결함이 노출됩니다. 서쪽의 서로 다른 불투명 지면이 겹치는 끝 x=-1.5에서 별도 접합선이 보입니다. 반복 이음과 이종 지면 접합은 서로 다른 검사 대상입니다.
- **Renderer:** 기존 검사에서 Sprite 연결·PPU·Pivot·Point·NoMip·Collider는 통과했습니다. 잘못된 참조나 필터 설정이 원인이라는 근거는 없습니다. 지면은 Tiled, 소품은 Simple, Tint는 흰색/alpha1입니다. Renderer 설정만으로 PNG 가이드선·재질 경계를 해결했다고 판단할 수 없습니다.

| 흰 선이 들어 있는 PNG | 실제 Scene 오브젝트 | 위치 (x,y) |
|---|---|---|
| `ObsidianScar_Cliff_Blocker_01.png` | `ObsidianScar_Cliff` | (-9, 0) |
| `ObsidianScar_Fissure_Edge.png` | `ObsidianScar_Fissure` | (-5, 3.8) |
| `ObsidianScar_Fissure_Glow.png` | `ObsidianScar_Glow` | (-5, 3.8), Edge와 겹침 |
| `ObsidianScar_HeatVent_01.png` | `ObsidianScar_HeatVent` | (3, -2) |
| `ObsidianScar_Obsidian_Slab_01.png` | `ObsidianScar_Slab` | (5.2, 1) |
| `ObsidianScar_Obsidian_Spire_01.png` | `ObsidianScar_Spire` | (-8.8, 4.8) |

## 리뉴얼 담당이 먼저 만들 파일 — PNG3만

공통: **512×512 RGBA PNG, Sprite Single, PPU128, 중앙 Pivot (0.5,0.5)=256px, Point, Mipmap OFF, Border0, Full Rect**. 새 후보 파일명/폴더를 쓰고 기존 PNG·meta는 포함하거나 덮어쓰지 않습니다. 기술 설정은 Codex가 격리 Import에 적용합니다. 한 장 전체는 scale1에서 4×4 월드 유닛입니다.

| 샘플 | 제작 기준 | 기존 사용 위치와 검증 크기 |
|---|---|---|
| 기본 지면1 | 불투명 기본 재질. 좌우·상하 반복 모두 연결되는 명암/무늬. 테두리 프레임·가이드·눈에 띄는 반복띠 없이 제작 | `ObsidianScar_Ground`, 중심(0,0), scale1, Tiled **21×15**, 정렬 -20. 범위 x±10.5/y±7.5 |
| 균열/오버레이 지면1 | 기본 지면과 같은 밀도·조명·색 기준. 자체 반복과 기본 지면 위의 접합을 모두 고려. **샘플 제안:** 기존 배치 범위 안에서 RGBA 가장자리로 기본 지면에 자연스럽게 연결되는 후보. 불투명 버전이면 기본 재질과 맞춘 경계가 필요하며, 샘플 검수 전 혼합 방식을 정식 확정하지 않음 | `ObsidianScar_GroundCrackedWest`, 중심(-6,0), scale1, Tiled **9×15**, 정렬 -19. x=-10.5~-1.5. 기존 오른쪽 끝 x=-1.5 접합을 그대로 확인 |
| 대표 소품1 — Slab | 투명 배경의 독립 바위 소품. 가이드·프레임·텍스트 없이 제작. 기존 Slab은 크기 참고용이며 새 미술 방향은 리뉴얼 담당 제작/대표 검토 대상 | `ObsidianScar_Slab`, 중심(5.2,1), Simple, scale **0.5**, 정렬 -5. 이미지 전체 기준 **2×2** 유닛, 실제 바위 실루엣은 투명 여백만큼 작음 |

지면의 21×15·9×15는 PNG 크기가 아니라 Renderer가 채우는 월드 영역입니다. 소품 2×2도 투명 여백을 포함한 이미지 사각형 크기이며 Collider 크기 지시가 아닙니다. 좌표·Scale·Sorting·Collider·Bounds·Spawn·Quest는 유지합니다. AshPatch/Cracked02 등 나머지 공식 아트는 이번 샘플 범위 밖입니다.

**최소 납품:** PNG3 + 각 파일 역할/이름·SHA256을 적은 짧은 목록. 큰 QA 보고서·전체 환경 세트·새 Scene·meta 납품은 필요하지 않습니다. 샘플이 아직 도착하지 않아 새 미술 방향 합격 판정은 없습니다.

## 샘플 도착 후 Codex 확인 범위

기존 격리 Unity에서 후보3만 새 Asset으로 Import하고 해당 Renderer3의 Sprite만 임시 교체합니다. 원본 Scene은 열거나 수정하지 않습니다. 같은 카메라·Scale의 전체 화면1, 지면 반복/접합 확대1, Slab 확대1을 우선 확인합니다. 정적 PNG 반복 프리뷰를 실제 Unity 화면 합격으로 대체하지 않습니다.

가이드0·눈에 띄는 반복띠0·접합 자연스러움·기존 캐릭터와의 크기/가독성으로 짧게 판정합니다. 미술 방향은 바울 대표의 확인 대상으로 남깁니다. 코드·Quest·Save 변경 없이 기존 Main18 92PASS 및 v2 기술40PASS를 재사용하며 수백 건 QA를 다시 실행하지 않습니다. 신규 Import/Console 오류가 생긴 경우에만 그 변경분을 확인합니다.

**샘플 기술·시각 확인과 대표의 미술 방향 확인 후 전체 환경 제작 단계로 진행합니다. 샘플 합격은 Field09 전체 적용이나 main 병합 허가가 아닙니다.** 원본 Unity Scene·Save·UserData·공식 PNG/.meta·GUID 보호. 원본 Editor/포커스 조작0, 자동 전체 적용0, Push/main 병합0.

근거: 원본 `Field_09_ObsidianScar.unity`와 공식 Environment meta의 읽기 조회, [기존 v2 검증](Field09_v2_Unity_Delta_QA_20261020.md) 및 [실제 v1/v2 화면](Evidence/Field09V2_20261020/index.html). 이번에는 추가 Unity 실행·기능 QA를 하지 않았습니다.
