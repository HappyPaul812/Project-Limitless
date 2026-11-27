# Issue #124 · 기본 공격 전용 투사체 3종 · 2026-11-27

**PASS · 격리 적용.** 기준 `7f89d5be7acb01ba12c184b038f801c4d65a6e8c`, 제품 `308c160b9748161a46f34f9943b1e091bb5e552b`. [3종 정정](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7210987313) · [납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7211048281)

ZIP SHA256 `b36c33570075c0ac84a83082330f07d4dbf9a311659bb6a6fe353b9fe3ef6fc6`. CRC42/42, SUMS41/41, Manifest40/40. 개별/시트/APNG RGBA 및 APNG·명암 GIF 프레임 간격 일치. 제품 PNG 20/20은 납품본과 바이트 일치. 기본 불 시트의 마지막 셀 RGBA 0. 모두 우향 파일, PPU32·중앙 Pivot·Point 실제 임포트 확인.

| 기본 공격 | 프레임·규격 | 화면·재생 | 기존/신규 동일 피해 및 실측 준비 + 비행 |
|---|---|---|---|
| 사수 | 32×32, 8, 4×2 | 52 UI px, 12.5 FPS | 38, 1회, .217 + .342초 |
| 치유사 | 96×96, 5, 5×1 | 72 UI px, 20 FPS | 18, 1회, .275 + .233초 |
| 마도사 | 32×32, 7, 4×2 | 52 UI px, 60ms/약 16.67 FPS | 36, 1회, .275 + .342초 |

실측은 격리 Battle의 120FPS 시간 샘플입니다. 기존과 신규 각각 22 PASS·종료 0. 실제 공격 명령과 대상 선택, 발사 준비·Sprite 이동·좌우 반전·도착 및 도착 뒤 제거를 확인했습니다. 피해·타이밍 로그는 바이트 동일합니다. 제품 코드는 `PlayBasicAttack`의 아군 직업 리소스 선택만 변경해 정조준·화살비·화상 틱·파이어 볼 스킬·적 공유 참조를 보존했습니다. 이 공유 스킬들은 재실행 없이 소스 차이로 확인했습니다. 기존 Editor Search 시작 예외 재현.

1:1 밝음/어둠 비교에서 세 도상이 구분됐습니다. 사수 화살은 어두운 전투 배경에서 기존 금빛 화살보다 대비가 낮으나 방향과 비행은 확인됩니다. 치유사 빛은 밝은 배경에서 가장자리 대비가 약하나 고리형 핵이 남습니다.

[실제 Battle 비행·도착](../../Evidence/Issue124_BasicProjectile3_20261127/Basic3_Battle_Comparison.png) · [밝은 1:1](../../Evidence/Issue124_BasicProjectile3_20261127/Basic3_Actual_Light.png) · [어두운 1:1](../../Evidence/Issue124_BasicProjectile3_20261127/Basic3_Actual_Dark.png)

기존 PASS 전수 재실행, Save/UserData·보호 브랜치 변경, main 병합·Push 없음. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
