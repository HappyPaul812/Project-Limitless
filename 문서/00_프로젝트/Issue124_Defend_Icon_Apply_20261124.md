# 2026-11-24 · Issue #124 승인 기본 방어 아이콘 적용

기존 #124 격리 브랜치 `codex/issue124-loot2-attack-icon-20261124`의 제품 `1d8ec56059458b18a3344eb1753c9f63fc041ae2`, 직전 보고 `f96b732c78a82159b84d3c4acf24a757e39fa172` 기준. [최신 승인 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7118370936)

- 승인 ZIP SHA256 `3cdfa248cd0fc2d363f3bcbc3345665da5aa72de24fd6aa46bdcde834ffb3a5a`, PNG SHA256 `026574c506cf03db37eca4b57a24ba74251db4d6e48f5c868cf421ca69c43413` 일치. `DefendButton_Original_128_RGBA.png`, 메타 GUID `4bf1e5088a404f548e8039cbd425d36c`, `BattleUiIconCatalog.cs`의 `command.defend` 매핑 한 줄만 제품 변경.
- **Unity 6000.6.5f1 PASS.** 기존 호환 QA 복제본 `Temp/q124`의 실제 Battle 씬에서 플레이어 턴 방어 버튼을 클릭했습니다. 같은 원시 피해를 게임의 직접 피해 경로에 넣어 비방어 20 → HP 20 감소, 방어 20 → HP 10 감소, 방어 21 → 올림으로 HP 11 감소를 확인했습니다. 실제 적의 턴에서도 방어 상태로 피해를 받았고, 다음 플레이어 턴에는 방어 상태가 풀리고 버튼이 다시 활성화됐습니다.
- 기존 공격 Sprite와 전리품 두 Sprite·White Tint는 런타임에서 확인했습니다. 전투 로직·드롭·수치·Save·Kenney 원본·#85 후보는 변경하지 않았습니다. 배치는 합성 Save/Settings를 사용했습니다.
- 실제 UI 캡처는 `Temp/i124-defend-evidence/Defend_Active_30px.png`, `Defend_Active_40px.png`, `Defend_Disabled_30px.png`, `Defend_Disabled_40px.png`입니다. 게임의 18px 기준 아이콘 크기는 그대로 두고 실제 전투 Canvas를 30/40px 배율로 오프스크린 렌더링했습니다. 로그 `unity.log` 968–1585행, 배치 종료 0.
- 제품 브랜치 클린 Import·Save 왕복은 미실행입니다. #85 및 본편 적용 HOLD. 원본 main `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, 원본 Git 상태 및 #85 보고 HEAD `c5d9ed202a445ce3ef68c72f0b54cc47c1719b9c` 보존. 병합·Push 없음.
