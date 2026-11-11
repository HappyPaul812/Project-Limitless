# Issue96 — 시작마을 HP·MP 승인 오리지널 아이콘 실제 게임 반영

2026-11-10. **STARTERVILLAGE_SHOP_POTIONS_ORIGINAL / UNITY_DELTA_QA_PASS / LOCAL_ORIGINAL_APPLIED / USER_REALPLAY_REVIEW_PENDING / REMOTE_NOT_PUSHED**.

[최신 기획 승인](https://github.com/HappyPaul812/Project-Limitless/issues/96#issuecomment-6766154803)과 docs/planning PLANNING_LATEST(blob `14a0a2b13bf273b2a0aa8dcd560d9631651d36c4`)를 확인했습니다. 바울 대표의 최종 미술 승인을 반영했으며 추가 아트 제작·승인 질문은 하지 않았습니다.
브랜치 **`codex/ch1-shop-potions-original-20261110`**, 구현 commit **`f5f3a07aa4c5b797c417e6bcc80783d8b5501d05`**. 원본 local main `4ae186e305d1762d8a08002cf309a5371a7c7e72`에서 제품 commit으로 ff-only 적용했습니다. 문서 포함 최종 SHA는 Issue96 결과 댓글 및 `Temp/Issue96Original/Evidence/FinalResult.json`에 기록합니다. main 직접 commit/원격 Push·Merge0.

## 승인 납품과 제품 변경5파일

실제 `F:/Downloads/LIMITLESS_ISSUE96_HP_MP_ORIGINAL_POTIONS_20261110.zip`을 읽었습니다. **2,286,596bytes / ZIP SHA `d1ce0e1cbbf64b588dd2b905a50cc4ecbc2e9bf2bd06378b58ab1d2cb62d7ead` 일치 / 13entries CRC PASS / Manifest11 SHA·bytes PASS**.

| 승인 PNG / 새 Resources 경로 | SHA256 / 신규 GUID |
|---|---|
| `Candidates/Small_HP_Potion_Original_128_RGBA.png` → `ItemIcons/StarterVillageShop_HP_Original128` | SHA `675a62927ee6a1321ad552674e931a8f53f88680f8d14de7a0acde7c17497f64`; GUID `b9934fac8bf6472992f57b8b1be5944d` |
| `Candidates/Small_MP_Potion_Original_128_RGBA.png` → `ItemIcons/StarterVillageShop_MP_Original128` | SHA `39b90dfc0dac057c61ef39adb23d215236bb9ef7abe721c6bce0e713cdfdb64a`; GUID `b3bd31231d94467cac840a284036859a` |

두 PNG와 각각의 meta를 `Unity/Client/Assets/_Project/Resources/ItemIcons/`에 새로 등록했습니다. 승인 PNG 바이트 그대로이며 128×128 RGBA, Single/PPU100/Pivot(64,64)/Bilinear/Uncompressed/Clamp/no-mipmap입니다. Preview와 고해상도 소스는 게임에 추가하지 않았습니다. [납품 SHA](Sources.json), [등록 경로·GUID](NewAssets.json), [원본 Editor 임포트](OriginalImport.json).
다섯 번째 제품 파일은 `Unity/Client/Assets/_Project/Scripts/UI/ShopPresenter.cs`입니다.

## 적용 범위와 유지한 계약

BuildItemRows의 이미지 선택만 ResolveShopItemIcon으로 연결했습니다. **활성 Scene=World_StarterVillage AND shopId=shop_general_starter_village AND itemID=대상HP/MP**인 경우에만 신규 Sprite를 씁니다. 신규 원화 색을 보존하도록 해당 Image의 Tint만 Color.white, 기존64×64/preserveAspect를 유지합니다. 자산이 없으면 기존 아이콘으로 돌아갑니다.
시작마을 구매·판매 목록의 대상2종만 새 표시입니다. 기존 Kenney PNG2/meta/GUID·공용 ItemDefinition·Scene/Prefab·다른 상점·인벤토리·전투 UI는 수정하지 않았습니다.

| 아이템 | 유지한 값 |
|---|---|
| item_healing_potion_small | HP+30 / 구매20 / 판매10 |
| item_mana_potion_small | MP+12 / 구매30 / 판매15 |
| 기타 | ID·설명·MaxStack·수량·거래 서비스·Save 형식·입력/이동 잠금 로직 변경0 |

## 실제 Unity Delta QA

Unity **6000.6.5f1**. 기존 ShopPresenter의 교체 전 **13 PASS**, 변경 후 **46 PASS / 0 FAIL / Runtime Error0**. 반영 원본5파일과 바이트가 같은 격리 복제본의 재실행도 **46 PASS / 0 FAIL**입니다.

- 실제 World_StarterVillage와 Npc_GeneralShop owner, ShopPresenter의 공개 진입 함수를 사용했습니다. HP 둥근 병/MP 가는 병을 모양과 색으로 구별할 수 있고, 960×720·1280×720 GameView에서 정상 표시됩니다.
- Image의 논리 크기64×64 유지. 1280폭에서는 실제64px, 960폭에서는 기존 CanvasScaler의 폭 기준 배율 때문에48px 상당입니다. 레이아웃/Scaler는 바꾸지 않았습니다.
- Input System 가상 마우스로 MP 선택·수량 증가·구매·판매·닫기, 가상 키보드로 Tab 이동/Enter 선택/Esc 닫기/D 이동을 실행했습니다. OS 입력·사용자 포커스는 조작하지 않았습니다.
- HP2 구매: 수량5→7, 잔액1000→960. MP2 구매:3→5, 960→900. HP2 판매:7→5, 900→920. MP2 판매:5→3, 920→950. 수량·구매 가능 수·판매 후 목록 갱신 PASS.
- HP+30/MP+12·구매20/30·판매10/15·설명/효과 상세 PASS. 기존 보유 상한·잔액 기반 판매 수 계산을 사용하며 새로운 재고 시스템은 만들지 않았습니다.
- 창이 열린 동안 D 입력 이동0, Esc/마우스 닫기 후 잠금 해제와 실제 이동 복귀. 다른 아이템의 판매 행은 기존 Icon 유지 PASS.
- 같은 마을의 다른 Catacomb 상점 ID, 마을 밖 Field_01에서 같은 StarterShop 정의를 열어도 기존 HP/MP Sprite·Tint 유지 PASS. 인벤토리·전투용 공용 ItemDefinition Icon도 기존 Kenney 경로를 유지합니다.
- 구매·판매는 격리 Synthetic Save에 기존 자동 저장 경로로 저장했고 잔액950/HP5/MP3과 안정 ID를 정상 로드했습니다. Save에 신규 표시 경로가 들어가지 않는 것도 확인했습니다. 사용자 원본 Save/UserData 쓰기0.

교체 전 화면은 신규 조건을 우회한 모의 화면이 아니라 원본 ShopPresenter와 같은 바이트의 기존 코드로 촬영했습니다. [Before13](Before_checks.txt), [After46](After_checks.txt), [반영 후46](PostApply_checks.txt). 검증기는 Tools/Issue96/PotionProbe.cs에 보관하며 제품 Assets에 넣지 않았습니다.
기존 격리 캐시 `Temp/Issue97/QA/Client`를 재사용했지만 Snake/F2 등의 이전 QA는 호출하지 않았습니다. 첫 검증기 컴파일의 불필요한 namespace를 수정했습니다. 첫 After 실행 중 격리 환경의 Define 재컴파일·도메인 재로드가 상태를 초기화해 완료되지 않아, 짧은 격리 검증 동안만 재로드를 보류하고 종료 시 해제하도록 보완한 후46/46 PASS했습니다. 제품 변경 범위를 늘려 해결한 것이 아닙니다. 기존 Search/Burst 초기화 로그는 환경 문제로 구분하며 제품 Runtime Error0입니다.

## 실제 GameView

- [교체 전960](Before_GameView_960x720.png) / [교체 전1280](Before_GameView_1280x720.png)
- [교체 후960](After_GameView_960x720.png) / [교체 후1280](After_GameView_1280x720.png)

UnityEditor.GameView.RenderView의 실제 출력이며 Camera.Render·오프라인 UI 목업이 아닙니다. 크기 변경 후 warm-up하고 D3D 저장 방향만 정규화했습니다.

## 원본 반영 후 확인과 보호

반영 전5128파일 SHA·사용자 변경과 제품 경로 충돌0·신규4경로의 기존 파일 없음을 확인했습니다. 기존 ShopPresenter는 Temp/Issue96Original/Evidence/OriginalBackup에 백업했습니다. 원본에 ff-only 반영한 후 새 Sprite2개를 표적 임포트하고 원본 Editor 컴파일 완료·Console Error0·실제128²/PPU100/Pivot64/독립GUID를 확인했습니다.
원본 Editor에서 컴파일된 Scene/shop/itemID/ResourcePath 조건 문자열과 공용 HP/MP의 기존 Icon·회복량·가격을 실조회했습니다. [원본 확인](OriginalFinal.json). 원본 Editor의 Scene/Play/선택/포커스를 바꾸지 않았습니다. 원본의 새 Play를 켠 검증과 동일 파일 격리 복제본 실행 검증은 구분합니다.
**5128개 중5127 SHA 불변, 승인한 기존 코드1개 변경, 신규 PNG2/meta2만 추가**했습니다. dirty302/untracked550 경로 집합 동일, Save/UserData·기존 PNG/WAV/meta·공용 ItemDefinition·Scene/Prefab·Packages/BuildSettings 보존. [보호 결과](ProtectionAfter.json).

## 바울 대표 실플레이 확인

원본 `F:/study/codex/Project-Limitless/Unity/Client`에서 새 Play → Chapter1 시작마을 **잡화 상인**(Npc_GeneralShop, 위치 약 -5.8,1.2)과 대화 → **물건을 본다** → 빨간 둥근 HP 병/파란 가는 MP 병을 확인하시면 됩니다. 수량과 가격은 기존과 같습니다. 기존 Save 초기화는 필요하지 않습니다. 기술 HOLD는 없으며 실제 손 조작의 최종 수용 확인은 USER_REALPLAY_REVIEW_PENDING으로 남깁니다.
별건 Issue57 출처 감사는 `codex/ch1-monster-provenance-final` / `1c8f94c9c9a3241a651ed46fb0547e2bf7ed67d1`에서 마감됐고 ZIP 대기는 해소됐습니다. F2 원본 적용·NPC 신규 아트 적용은 이번에 하지 않았습니다.
