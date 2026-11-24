# 2026-11-24 · Issue #124 승인 아이콘 3종 적용

제품 `39fa4576e1ae984c09684ac14f56e48f1d541ca1`, 브랜치 `codex/issue124-loot2-attack-icon-20261124`, 기준 `65cceb2d6c3283d18abec6e27d8a6f1296411f17`. [승인 및 적용 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7117133033)

| 승인 PNG | SHA256 | 새 GUID |
|---|---|---|
| VenomBeeStinger_Original_v2_128_RGBA.png | `9b6d0b82f63b2e38813c9a0a979274b82729a970d84b8851456d22ea0179edd3` | `c19b35e07fb24b9f850eedf4227e67fb` |
| GrassSlimeGel_Original_128_RGBA.png | `ca06a5384883e0c848c4a2e893f5f524ee0d81ef22a0420425f3c0c58a902a55` | `81df6646910a4ac4ad8c10605e3bc322` |
| AttackButton_Original_128_RGBA.png | `cf775727e27be825809a6b3d9bd04b227abe3408055873f70616cc8a9ca19a32` | `f2a3199d03a84284a6274d4ba652e6ec` |

PNG 세 개와 고유 메타, ItemDefinition 두 개의 Sprite 참조와 White Tint, `command.attack` 매핑 한 줄이 제품 변경 전체입니다. TipOnly 후보, 방어 버튼, 외부 자산의 GUID·라이선스, 드롭 정의, 아이템 ID·가격·수치, 전투 처리 및 Save 코드는 기존 상태를 보존했습니다.

## Unity 표적 검증

Unity 6000.6.5f1 숨김 배치. 제품 worktree의 별도 QA 복제본 `Temp/q124`에 기존 #85 호환 패키지·투사체 메타 및 Library의 복사본을 사용했습니다. 제품 커밋의 Packages와 투사체 메타에는 변경이 없습니다. 원본 Save 대신 검증 폴더의 합성 경로와 테스트 인벤토리를 사용했습니다.

- **인벤토리 PASS** · 실제 Starter Village의 InventoryPresenter에서 두 승인 Sprite와 White Tint, 보유 수량과 기존 사용 불가 상태를 확인했습니다. 실제 Canvas를 50/40/30px 배율로 오프스크린 렌더링했습니다. 30px에서 침 실루엣은 구별되며 침끝 세부 가독성은 PARTIAL입니다. 화면 오른쪽의 일부 기존 UI는 캡처 경계에서 잘려 아이콘 검증 범위에 포함하지 않았습니다.
- **전투 PASS** · 실제 Battle 씬에서 플레이어 턴의 공격 Sprite, 기존 방어 Sprite, 대상 선택 중 공격 비활성화, UI 대상 버튼을 연속 두 번 눌렀을 때 대상 HP 감소 1회를 확인했습니다. 첫 시도에서 검증기가 동률 순서 결정 연출 중 입력해 전투 검증만 보완했고, 플레이어 턴을 기다린 재시험이 종료 코드 0으로 통과했습니다. 인벤토리는 반복 실행하지 않았습니다.
- 두 배치에서 제품 C# 컴파일 오류 0건. 제품 브랜치 자체의 클린 Import는 미검증이며 #85 HOLD는 그대로입니다. 드롭 확률·저장 호환은 기존 데이터와 코드 변경 없음으로 확인했습니다. 드롭 재추첨과 Save 왕복 QA는 이번 범위에 포함하지 않았습니다.

로컬 증거 `Temp/i124-evidence`의 `Inventory_30.png`, `Inventory_40.png`, `Inventory_50.png`, `Battle_Attack_Active.png`, `Battle_Attack_Disabled.png`, `unity.log`, `battle.log`. 캡처는 실제 게임 씬의 UI를 오프스크린으로 렌더링한 이미지입니다.

원본 main과 Git 상태 853경로 및 #85 HEAD `c5d9ed202a445ce3ef68c72f0b54cc47c1719b9c` 보존. main 병합 및 원격 Push 없음.
