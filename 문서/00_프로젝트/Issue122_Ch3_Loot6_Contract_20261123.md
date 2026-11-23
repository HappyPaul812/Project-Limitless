# Issue #122 · Chapter 3 전리품 6종 등록 계약 (2026-11-23)

상태 **CONTRACT_READY / DROP_LINK_HOLD (6/6) / REPRESENTATIVE_APPROVAL_REQUIRED**. 기획 및 아이콘 승인은 확보됐습니다. 본편 등록 승인과 Stable ItemId 확정이 필요합니다.

## 기준과 전리품별 판정

로컬 main 기준 `736df561adb408c0225ed9052c37f2d45b7e24f5`. Unity `6000.6.5f1 (3ff58d469c8a)`. [기획 정본](../08_몬스터/Chapter3_북부몬스터7종_전투밸런스_20261118.md) §7과 [Issue #122](https://github.com/HappyPaul812/Project-Limitless/issues/122) 기준입니다. Recovery의 아래 6개 PNG는 [원격 복구 SHA](https://github.com/HappyPaul812/Project-Limitless/tree/f64082914e114096b6b9393263e7a36f7d829a55/Recovery/ApprovedArt/2026-11-19/Chapter3Loot)에서 각 파일을 회수해 로컬과 바이트 단위로 대조했습니다. 모두 128×128·8비트 RGBA이며 [#116 기록](Chapter3_Loot_Icons_Issue116_20261119.md)의 SHA-256과 일치합니다.

| 지급 몬스터 / 전리품 | 확률 / 판매 탈렌트 | 공식 ItemId / 검토용 후보 | 승인 PNG (`Recovery/ApprovedArt/2026-11-19/Chapter3Loot/`) / SHA-256 |
|---|---|---|---|
| 서리들쥐 / 서리 묻은 털뭉치 | 50% / 8 | 미등록 / `material_frost_vole_frosted_fur` | `Loot_FrostVole_FrostedFur.png` · `356bdef577ee85d6a609c5476601821d07b3e49a4a475b5c1f41b05c12bea704` |
| 설원족제비 / 설원 솜털 | 50% / 9 | 미등록 / `material_snow_stoat_snow_down` | `Loot_SnowStoat_SnowDown.png` · `2dca5ba78a8e1da51f40492ccb209c41f2046914d4c831d6cb8e7296ce0a43fc` |
| 서리올빼미 / 서리깃 | 45% / 10 | 미등록 / `material_frost_owl_frost_feather` | `Loot_FrostOwl_FrostFeather.png` · `6be6ac2502cb7476dbff11d52514148d99773a4e61272e2fb06ddf4935c3832e` |
| 얼음갑충 / 빙갑 조각 | 45% / 12 | 미등록 / `material_ice_beetle_ice_carapace` | `Loot_IceBeetle_IceCarapace.png` · `371c5fbc50d1682de7a35a3c116000b30e5b0aeb65839b098156b8968d688853` |
| 눈보라 정령 / 눈폭풍 결정 | 45% / 14 | 미등록 / `material_blizzard_wisp_blizzard_crystal` | `Loot_BlizzardWisp_BlizzardCrystal.png` · `16b100a6e109cfebd5ba3ed7d2dc2729018dcc3b47153a82c6e8e48854103b25` |
| 빙각 수호체 / 마력차단 결정 | 100% (단회) / 22 | 미등록 / `material_rime_sentinel_magic_blocking_crystal` | `Loot_RimeSentinel_MagicBlockingCrystal.png` · `0978353c3562b527072a70b907d06ae74985fa9b0f9e4b8ec7cd334673da25a8` |

- `Resources/ItemDefinitions`의 ItemId 22개와 대조했습니다. Catalog는 대소문자를 구분하고 중복 ID의 첫 항목만 남기므로 등록 직전에도 중복을 검사합니다.
- Recovery에 PNG 6개, 대응 .meta 0개입니다. Unity `Resources/ItemIcons`에 대응 원본이 없습니다. 로컬 Unity Library 디렉터리는 존재합니다. #116 기록에 Library 납품 경로 `/LIMITLESS/Issue116_Chapter3_Loot/`의 재회수 증거가 있으며, 이번에는 해당 서비스에서 실물을 재회수하지 않았습니다. 승인 후 Recovery 바이트를 복사하고 Sprite 임포트로 새 meta를 생성합니다.
- `Resources/MonsterDefinitions` 18개에 대상 6종이 없습니다. 서리올빼미 `CANDIDATE_frost_owl_basic_qa`는 별도 QA 리소스, 눈보라 정령은 시각 프리팹입니다. Main22의 실험용 들쥐도 정식 연결 대상에서 제외합니다. 6종 모두 **DROP_LINK_HOLD**. 보스 일반 드롭은 비워 둡니다.

## 승인 후 최소 구현

1. 기존 Ch2 재료 계약을 따릅니다. `ItemDefinition` 6개에 승인 ItemId·Sprite·`Material`·`MaxStack=99`·`UseType=None`·`EffectType=None`·확정 판매가를 설정합니다. `ItemCatalog`는 Resources에서 자동 조회합니다. `ShopService.IsSellable`은 양의 판매가와 재료 분류를 허용합니다. `BuyPrice=0`은 구매를 차단하지 않으므로 모든 `ShopDefinition.ItemIds`에서 제외하고 최대 구매량 0 및 직접 구매 거부까지 확인합니다.
2. 정식 몬스터가 등록된 뒤 해당 `MonsterDefinition.LootEntries`에만 성공 수량 1을 연결합니다. `BattleVictoryReward.Calculate`는 전투불능 개체를 참조로 중복 제거하고 각 개체의 전리품을 한 번 판정합니다. `BattleSceneController.EndBattle`은 `battleEnded`와 승리 분기로 전투 내 재호출 및 패배·도주 지급을 막습니다. `RewardBundle.ApplyBestEffort`는 수용 가능량으로 제한하며 실제 지급량만 UI에 반환합니다. EXP·탈렌트의 기존 동작을 유지합니다.
3. **100% 판정 결함**. `UnityEngine.Random.value`는 1을 반환할 수 있고, 현재 `>= DropChance` 비교는 확률 1에서도 이 값을 탈락시킵니다. 0 이하 제외, 확률 1 미만에서만 난수 비교, 확률 1은 확정으로 통과시키는 최소 수정과 경계 테스트가 필요합니다.
4. **단회 엘리트 영속성**. `MonsterEncounterService.MarkDefeated`의 비재생성 기록은 메모리 상태입니다. 승인된 고유 조우의 지속 완료 키를 사용해 조우 진입과 보상 확정을 함께 차단해야 합니다. 인벤토리·완료 키를 같은 저장 스냅샷으로 커밋하고, 실패 시 지급·완료의 불일치를 복구할 수 있게 기존 저장 연동을 검토합니다. 현재 `GameSaveService`는 임시 파일에서 기존 파일로 복사하며, 승리 호출부는 저장 반환값을 처리하지 않습니다. 한도 초과로 수령 0일 때 단회 보상을 소진할지 보류할지는 승인 필요합니다. 기존 전투 전체에 일괄 지급 변경을 적용할 필요는 없습니다.

## 검증과 승인 게이트

- Delta **PASS (정적·파일 대조)**. 로컬 Recovery/원격 복구/승인 기록 해시 6/6, PNG 헤더 6/6, 대응 .meta 0, ItemId/정식 몬스터 조회 및 드롭·판매·저장 코드 경로 대조. 원본 Recovery와 ProjectVersion에 HEAD 대비 경로 차이 0.
- **격리 .NET Delta 60 PASS**. 제품 C# 8개를 Unity·전투 경계 스텁과 컴파일해 비제품 ID 6개로 검사했습니다. 재료·아이콘 참조, Stack99, 판매액과 재고, 구매 거부, 비승리 난수/지급 0, 전투불능 개체 중복 제거, 수용량 제한, 합성 JSON 인벤토리 왕복, 확률 경계를 확인했습니다. 확률 1에서 난수 1이면 드롭 0인 기존 결함이 재현됐습니다. 실제 전투 종료 콜백, Unity Sprite 임포트, GameSaveService 및 가방 한도 미만에서의 재진입 방지는 이 검사에 포함되지 않습니다.
- **Unity 실행 미검증**. 본편·격리 Unity 임포트, UI 표시, 실제 Battle/Save는 이번 조사에서 실행하지 않았습니다. 후속에서는 별도 `GameSaveService.AuditSaveDirectory`와 메모리 감사 ItemDefinition을 사용합니다. 6종 Sprite 연결, 98→99→100 거부, 판매가/잔고/재고, 구매 거부, 승리·패배·도주·동일 콜백 재호출, 확률 경계(0/0.45/0.50/1), 엘리트 저장 실패 및 재접속을 확인합니다. 실제 저장 데이터는 읽거나 쓰지 않습니다.
- **대표 승인 대기**. Stable ItemId 6종 및 본편 적용, 정식 몬스터 등록과 단회 조우/지속 완료 키, 가방 한도 시 단회 보상 처리. #85 HOLD와 #118 수치, 사용자 작업 트리, 승인 자산과 6000.6.5f1 보호. #121 QA/NPC50/몬스터75 재실행 0.
- 담당 브랜치 `codex/ch3-loot6-contract-20261123`. 실제 보고 커밋 SHA는 Issue 댓글에 기록합니다. 로컬 보고용이며 원격 Push는 별도 승인 사항입니다.

- 등록 계약 최초 커밋 cfa9b02d3e5c52aa5355765ab158f860c66bed8c.
