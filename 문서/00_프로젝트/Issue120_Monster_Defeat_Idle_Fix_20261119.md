# Issue #120 후속 · 전투불능 중 대기 프레임 덮어쓰기 수정

2026-11-19 · Unity 6000.6.5f1, 기존 격리 전투와 Issue #120 추적 재사용. 수정 전 추적의 불씨망령 전투불능 순서는 `12 → 13 → 14 → 00(Idle 첫 셀) → 15`였습니다. 치명타 처리의 `RefreshCombatantViews`가 전투불능 재생을 시작한 뒤, 공격 연출 종료 콜백의 `RestoreBattleIdle(targetView)`가 Image에 IdleSprite를 한 번 직접 설정했습니다. 그 다음 몬스터 애니메이션 Update가 올바른 전투불능 셀을 복원했습니다.

수정은 `BattleSceneController.RestoreBattleIdle` 한 곳입니다. 표시 HP가 0인 몬스터 애니메이션은 공용 대기 복원에서 제외합니다. 생존 몬스터 및 플레이어·동료 기존 경로, 타격·피해·보상·아트·전투불능 시트는 그대로입니다.

## 실제 전투 회귀 검증

| 대상 | 실제 조우 | 확인 |
|---|---|---|
| 불씨망령 | Field07 일반 | Idle/Attack/Hit/Defeat 각 4셀, 공격 뒤 Idle 복귀, KO 후 Idle 0회와 마지막 전투불능 유지 |
| 화열딱정벌레 | Field08 일반 | 같은 검증 및 필드 복귀·승리 보상 |
| 균열도마뱀 | Field08 Main17 지정 | 같은 검증 및 필드 복귀·승리 보상 |

**75 PASS / 0 FAIL / 게임 런타임 오류 0 / 배치 종료 코드 0**. 기존 원본 전투 코드의 다른 보상/타이밍과 아트·메타는 변경하지 않았습니다. 실제 Battle 명령 버튼과 조우 서비스를 사용했고 저장·설정은 지정된 합성 경로로 격리했습니다. Unity Search 초기화 예외는 게임 오류와 별개로 로컬 기록했습니다. [프레임·실행 증거](Evidence/Issue120DefeatFix/) · [Issue #120](https://github.com/HappyPaul812/Project-Limitless/issues/120)

제품 `4b423a70585cfed7b4fbf1697bf61da7476ccd4b`, 로컬 `codex/issue120-ember-wraith-idle-attack`, 원격 Push 미실행.
