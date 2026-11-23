# Issue #122 · 승인 전리품 등록 및 Unity Delta (2026-11-23)

담당 브랜치 `codex/ch3-loot6-registration-20261123`. 제품 커밋 `9588d75cfbe46e656ee6dcc4b5e8f85c4aa04cc0`. 승인 전 근거 [설계 선택지](Issue122_123_Approval_Options_20261123.md) 및 [기존 검증](Issue122_Ch3_Loot6_Followup_20261123.md).

- `Scripts/Battle/BattleVictoryReward.cs`의 승인된 한 줄을 반영했습니다. 확률 1 미만에서만 확률 판정용 난수를 비교합니다. 기존 확률 값과 EXP·탈렌트·수량 규칙은 보존했습니다.
- 제품 변경은 C# 1개와 신규 ItemDefinition 6개, 승인 PNG의 게임용 복사본 6개, 신규 meta 12개입니다. 기존 Ch2 아이콘과 같은 Sprite Single·100 PPU·Point·중앙 Pivot을 사용했습니다. 신규 12 GUID의 기존 트리 충돌 0, PNG 6개는 승인 Recovery Git blob과 일치합니다.
- 여섯 아이템은 Material, Stack99, 판매 전용, Use/Effect None입니다. 상점 판매 대상에서 제외되므로 구매할 수 없습니다.

| 정식 ItemId | 에셋 이름 | 판매 탈렌트 |
|---|---|---:|
| `material_frost_vole_frosted_fur` | FrostVole_FrostedFur | 8 |
| `material_snow_stoat_snow_down` | SnowStoat_SnowDown | 9 |
| `material_frost_owl_frost_feather` | FrostOwl_FrostFeather | 10 |
| `material_ice_beetle_ice_carapace` | IceBeetle_IceCarapace | 12 |
| `material_blizzard_wisp_blizzard_crystal` | BlizzardWisp_BlizzardCrystal | 14 |
| `material_rime_sentinel_magic_blocking_crystal` | RimeSentinel_MagicBlockingCrystal | 22 |

## 신규 등록만 Unity Delta

숨김 BatchMode Unity 6000.6.5f1 (3ff58d469c8a), 소형 격리 프로젝트의 제품 Item/Catalog/Shop/Inventory 코드와 기존 ItemDefinition 22개·ShopDefinition 3개를 사용했습니다. **32 PASS / 0 FAIL**. Sprite import 6, 카탈로그 중복 및 기존 상점 로드 2, 정의·Sprite 연결 6, 구매 거부 6, Stack99 6, 판매가·재고 6입니다. [검사 결과](Evidence/Issue122_Registration_20261123/delta.txt)

제품 전체 Unity 실행, 실제 Save 및 전투는 미검증입니다. 기존 C# 56 PASS와 합성 저장 39 PASS는 근거로 재사용했습니다. 새로 실행한 검사는 등록 Delta뿐입니다. 격리 SpriteImporter가 생성한 YAML의 줄끝 공백만 커밋 전에 정리했습니다.

## 남은 적용 조건

- 정식 대상 조우 ID 0/6. 몬스터 등록 및 Drop 연결 `DROP_LINK_HOLD`. 공식 조우 ID를 확인한 뒤 단회 완료 키와 보상 대기·저장 실패 설계를 구현합니다. 실제 Save 연동 및 재접속을 별도 격리 검증합니다.
- #123 Import 3/3 기술 PASS 재사용. PNG 재작업·반복 검사 0. 지면 접합 단일 오버레이, F3 정식 Scene·휠체어 통행 검증 및 최종 미술 승인 전 적용 HOLD.
- 기존 아이템·가격·드롭률, 승인 자산·Save/UserData·Discord, Unity 버전 및 #85 HOLD 보호. 원본 local main 동기화·Push 0.
