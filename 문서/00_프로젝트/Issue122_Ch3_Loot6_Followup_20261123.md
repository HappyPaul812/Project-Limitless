# Issue #122 · 100% 드롭 및 단회 엘리트 후속 — 2026-11-23

**BOUNDARY_FIX_VALIDATED_IN_ISOLATION / PERSISTENCE_MODEL_PASS / DROP_LINK_HOLD 6/6**

[기존 등록 계약 및 60 PASS](Issue122_Ch3_Loot6_Contract_20261123.md), 기준 커밋 `0863351efc349a5cf4857f5d61b65f51e9355aad`. PNG 6종 및 기존 60 PASS는 재사용했습니다.

## 제품 경로와 최소 수정안

`BattleSceneController.EndBattle` 승리 분기 → `BattleVictoryReward.Calculate` → `UnityBattleRewardRandom.NextValue` → `UnityEngine.Random.value`. 후자는 1.0을 반환할 수 있습니다. `random.NextValue() >= loot.DropChance`는 확률 1에서도 난수 1을 실패 처리합니다. 현재 전리품 6종은 미등록이므로 해당 제품 경로의 경계 결함을 확인한 것이며, 실제 Ch3 드롭 사고를 관찰한 것은 아닙니다. [Unity Random.value](https://docs.unity.com/en-us/engine/6000.0/script-reference/unityengine/random/value)

```diff
-                        || random.NextValue() >= loot.DropChance) continue;
+                        || (loot.DropChance < 1f && random.NextValue() >= loot.DropChance)) continue;
```

0 이하는 계속 제외하고 0.45·0.50은 기존 경계 비교를 유지합니다. 확률 1은 확률 판정 난수를 건너뛰고 기존 수량 경로를 사용합니다. 따라서 확률 1이 포함된 구성에서는 후속 난수 호출 순서가 바뀔 수 있습니다. 현 정식 몬스터 데이터에 숫자 값 1인 드롭 엔트리는 검색되지 않았습니다.

- 최신 제품 C#의 실제 `Calculate`를 격리 컴파일하고 의존 경계만 스텁으로 구성했습니다. 4개 확률 × 7개 난수 × 결과/호출 수 2개 = **원본 56 PASS / 0 FAIL, 수정안 56 PASS / 0 FAIL**. 원본 검사는 의도한 결함 재현을 포함하고, 수정안은 1.0 입력에서 확정 드롭을 확인했습니다.
- 합성 JSON 슬롯·원자적 파일 교체 모델 **39 PASS / 0 FAIL**. 빈 가방·98·99, 정상 저장·교체 전 실패·교체 후 응답 실패, 재호출·재열기·만석 수령 보류·공간 확보 후 재청구를 확인했습니다. 실제 `GameSaveService`와 Unity 재접속은 미검증입니다. 수치는 인벤토리와 승리 적용 횟수를 구별하기 위한 시험값입니다.

## 저장·만석·재접속 설계

`battleEnded`는 같은 컨트롤러 인스턴스에서만 중복 완료를 막고, `MonsterEncounterService`의 비재생성 표시는 메모리입니다. `RewardBundle.ApplyBestEffort`는 가방에 들어가는 수량만 반환해 99개에서 단회 보상이 소실될 수 있습니다. 현 `GameSaveService`는 임시 파일을 `File.Copy`로 덮어쓰며 승리 호출부는 저장 결과를 확인하지 않습니다.

1. 승인된 고정 조우의 기존 stable encounter ID 계약을 검토합니다. 슬롯별 지급 키로 사용할 수 있는 영속 ID가 확정되면 승리 완료와 수령 상태를 함께 식별합니다. 패배·도주에는 완료/지급 없음. 기존 Quest ID를 전용 키로 임의 재사용하지 않습니다.
2. 승리 시 인벤토리·기존 승리 결과와 완료 상태를 하나의 저장 후보로 준비합니다. 공간이 있으면 1개 지급 완료, 만석이면 처치 완료와 보상 1개 수령 대기를 함께 저장합니다. 수령 대기가 남아도 조우는 닫히며 공간 확보 후 승인된 수령 지점에서 동일 보상을 한 차례 청구합니다. 청구 또한 인벤토리 증가와 수령 완료를 한 스냅샷으로 확정합니다.
3. 같은 슬롯 쓰기를 직렬화하고, 검증한 임시 파일을 같은 볼륨에서 원자적으로 교체하는 방식과 복구 동작을 실제 저장 경로에서 검증합니다. 커밋 전 실패 시 임시 변경을 롤백하거나 재시도 가능한 같은 결과를 유지합니다. 커밋 여부가 모호하면 완료 키가 포함된 유효 슬롯을 다시 조회해 재적용 여부를 결정합니다. 성공 확인 전에 승리·수령 안내와 다음 저장을 진행하면 이전 스냅샷을 덮을 수 있으므로 실제 흐름의 저장 호출 순서도 승인 후 검증합니다. 구버전 저장은 새 이력을 빈 상태로 읽는 방안을 검토하되, 이미 완료된 조우 이력의 이관은 기존 콘텐츠 정본이 있을 때 별도로 확인합니다.

## 등록 의존관계 및 승인

| 검토용 ID | 충돌 |
|---|---|
| `material_frost_vole_frosted_fur` | 0 |
| `material_snow_stoat_snow_down` | 0 |
| `material_frost_owl_frost_feather` | 0 |
| `material_ice_beetle_ice_carapace` | 0 |
| `material_blizzard_wisp_blizzard_crystal` | 0 |
| `material_rime_sentinel_magic_blocking_crystal` | 0 |

ItemDefinition 22개와 제품의 asset/cs/unity/prefab/json에서 후보 문자열을 대소문자 무시로 재검색했습니다. 공식 ItemId **0/6**, 공식 MonsterDefinition **0/6**입니다. 대표의 ID·Item 등록 승인 → 승인 PNG Sprite 및 판매전용 카탈로그 등록 → 정식 MonsterDefinition과 안정 조우 ID 승인·등록 → 대상별 드롭 연결 → Unity 저장·전투 격리 검증 순입니다. 서리올빼미 QA 후보와 눈보라 정령 시각 프리팹에는 연결하지 않습니다. 보스 일반 드롭 없음. 대표가 단회 엘리트의 완료 키, 수령 대기와 청구 지점, 저장 실패 UX를 승인해야 합니다. 코드·저장 DTO·EXP·스폰·기존 자산은 제품 적용 0입니다.

검증 로그 [loot-delta.txt](Evidence/Issue122_123_20261123/loot-delta.txt). 담당 브랜치 `codex/ch3-loot6-contract-20261123`, 실제 보고 SHA는 Issue 댓글에 기록합니다.
