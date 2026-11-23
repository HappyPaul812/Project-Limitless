# Issue #126 빙각 수호체 전투 연결 · 2026-11-23

- 브랜치 `codex/issue126-rime-sentinel-runtime-20261123`
- 제품 커밋 `a2faf0b02c960ddc795cc3054ad4beebbeb49d5f`
- 로컬 커밋 완료 · 원격 Push 및 병합 없음
- 근거 [Issue #126 승인 지시](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7098294356)

## 제품 변경

`BattleActionPresenter.cs`, `BattleSceneController.cs`, `MonsterDefinition.cs`, `MonsterSpriteSheetAnimation.cs`를 변경했다. `Resources/RimeSentinelPresentation/`에 승인 PNG 5종과 해당 meta, `VisualDefinition.asset`과 폴더 및 에셋 meta를 추가했다. 표시 정의는 정식 몬스터 검색 위치 밖에 두고 전리품 슬롯을 비웠다. 기존 Idle/Attack/Hit/KO 전환을 재사용하고, 빙각 수호체는 Attack4 시작 340ms 후 타격, Hit4 종료 후 대기 복귀, HP 0에서 Defeat4 마지막 프레임을 유지하도록 연결했다. 기존 몬스터는 기존 FPS와 타격 기본 경로를 사용한다.

| 연결 PNG | 승인 SHA-256 일치 |
|---|---|
| KeyPose.png | c34ea53a95aa6a1fdba4f5e355715fbc25797d6d51e4d544cb9e4dab52f7073c |
| Idle4.png | 0757ebedd37f24e7939d26f38be2efbf9560906b832fae187c61273a314b753e |
| Attack4.png (v2) | 0649ef42da7aa628e471a49f4554ad150a0a5fc1b28235a5e9b42bf025030621 |
| Hit4.png | bd6d66be3b990d9bc7f33261f1d9835abd413c28b91a5842ac9800a5dec28496 |
| Defeat4.png | a7d12a5a86495b59b2c7e0a96e658ff1cf743cdec3daafa9fb4e9ce9a8124d8d |

## 검증과 적용 조건

- **PASS · 10 PASS / 0 FAIL.** Unity 6000.6.5f1 격리 Play에서 제품 연출기·전투원·스프라이트 애니메이터를 실행했다. 타격 프레임 2의 0.3417초에 피해 44가 한 번 적용됐다. 공격·피격 후 대기로 복귀했다. HP 0에서 Defeat4로 전환하고 2.45초가 지난 후에도 마지막 프레임을 유지했다. Ember의 기존 FPS, 타격 단회 처리, KO 유지도 확인했다.
- **미검증.** 제품 BattleSceneController가 이끄는 전체 전투와 승리 전환은 재생하지 않았다. 격리 테스트는 제품 컴포넌트와 별도 테스트 전투원을 사용했다. 격리 Editor에서 테스트 외 Search 예외 1건이 로그에 남았다.
- **HOLD.** F5 정식 Scene·조우 ID·빙각 수호체 공식 몬스터 등록 및 고유 AI 경로를 확인하지 못했다. F5 배치와 조우, 전리품, 단회 Save 및 승리 후 연출은 정식 조건 확인과 별도 승인 후 검증한다.
- 승인 PNG 기존 파일·기존 몬스터 데이터·Save/UserData·Discord·원본 main 보호. #85 HOLD 유지.

[격리 Unity 화면](../../Evidence/Issue126_RimeSentinel_Runtime_20261123/Rime_Runtime_Combat_Comparison.png) · [실행 로그](../../Evidence/Issue126_RimeSentinel_Runtime_20261123/runtime.txt)
