# LIMITLESS / 2026-11-23 / 빙각 수호체 승인 원화 게임용 연결 및 전투 검증 지시

- 담당: Codex, 작업 브랜치 `codex/issue126-rime-sentinel-runtime-20261123` (충돌 시 고유 접미사)
- 승인 근거: 바울 대표 “빙각이는 나도 좋아, 승인,” → [#126 최종 미술 승인](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7098166179).
- 수행 승인: **승인 원화 5종을 실제 Unity 게임 구조에 안전하게 연결하고, 빙각 수호체의 공격·피격·전투불능 애니메이션을 격리 Play로 검증하는 최소 구현 작업**.
- **제품 main 병합·Push/원격 main 변경·신규 F5 Scene·Save 확장·실제 Drop/Spawn 활성화는 이번 승인 범위 밖.**
- 시작 기준: 사용자 PC의 **실제 최신 로컬 커밋과 미커밋 작업**을 재조회. 2026-11-19 보고 로컬 main `736df561adb408c0225ed9052c37f2d45b7e24f5`; 원격 복구 ref `backup/2026-11-19-safe-checkpoint`=`f64082914e114096b6b9393263e7a36f7d829a55`; 원격 main `bfd281545632877325f5d21e04cefb924042c465`은 **오래된 기준**. 지금 상태를 이 과거 SHA로 강제 되돌리지 않는다.

## 승인 아트 — 재제작·내용 수정 불가
| 항목 | 승인 PNG SHA256 | 적용·검증 조건 |
|---|---|---|
| KeyPose | `c34ea53a95aa6a1fdba4f5e355715fbc25797d6d51e4d544cb9e4dab52f7073c` | 동일 개체·Pivot/PPU 확인 |
| Idle4 | `0757ebedd37f24e7939d26f38be2efbf9560906b832fae187c61273a314b753e` | 4프레임 반복, **260ms/프레임** |
| Attack4 v2 | `0649ef42da7aa628e471a49f4554ad150a0a5fc1b28235a5e9b42bf025030621` | 4프레임 비반복, **170/120/150/220ms**, 기본 얼음 주먹110%용 |
| Hit4 | `bd6d66be3b990d9bc7f33261f1d9835abd413c28b91a5842ac9800a5dec28496` | 4프레임 비반복, **100/160/130/220ms**, Idle4 복귀 |
| Defeat4 | `a7d12a5a86495b59b2c7e0a96e658ff1cf743cdec3daafa9fb4e9ce9a8124d8d` | 4프레임 비반복, **180/220/250/1800ms**, KO 후 최종 포즈 고정 |

- 모든 시트 형식은 기존 승인 보고의 원본 기준으로 유지. Idle·Attack·Hit·Defeat 시트는 각 2048×512 PNG, 가로 4×512 셀. Unity 표준 PPU/Filter/Pivot는 기존 북부 공식 몬스터와 비교하고, 기존 격리 QA에서 승인된 값 우선.
- 납품 단서는 `LIMITLESS_Issue126_RimeSentinel_Defeat4_20261123.zip`(SHA256 `c7b840ce80815814be0a11269d29b636acfa0b2966981801fa19228bade296ef`) 및 [SPRITE_ART_LATEST](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-new/문서/00_프로젝트/팀운영/인계/SPRITE_ART_LATEST.md). KeyPose/Idle4/Attack4v2/Hit4 ZIP과 실제 PNG는 사용자의 기존 로컬 납품 위치·Library/승인 보고에서 **찾아 확인**, 존재하지 않으면 **MISSING_INPUT**으로 보고. 유사 파일 대체, 새 그림 생성, 승인 SHA 불일치 무시 금지.

## 실행 절차 — 중복 QA 금지

1. `AGENTS.md`, `START_HERE.md`, `COMMON_RULES.md`, `CODEX.md`, `CODEX_LATEST.md`, #126, #122, [북부7종 승인 전투 정본](../08_몬스터/Chapter3_북부몬스터7종_전투밸런스_20261118.md) 확인. 실제 `git status`/Unity Editor 버전 `6000.6.5f1`/Scene·Prefab·MonsterDefinition·스프라이트·애니메이션·Save·미커밋 및 사용자 작업을 비파괴 조사.
2. 별도 worktree를 실제 최신 로컬 베이스에서 만들고 `codex/issue126-rime-sentinel-runtime-20261123` 작업. Codex #122의 서리들쥐 1/6 및 기존 전리품 6개 격리 작업과 동일 카탈로그를 동시 충돌 수정하지 않음. 이미 다른 브랜치에 있는 유효 결과는 검사 후 재사용하고, 덮어쓰거나 무리하게 병합하지 않음.
3. 승인 다섯 PNG 실물 SHA256 대조·메타/구조·기존 M1~M5 Runtime 연결 경로 확인. 실물이 확인된 **5종 원본만** 프로젝트 전용 경로에 비파괴 Import; 메타 GUID 새로 발급이 필요한 것만 유일하게 발급, 기존 승인/외부 자산의 GUID 덮어쓰기 금지.
4. 빙각 수호체 **전용 Sprite/Animation/Animator 또는 기존 전투 시각 바인딩** 최소 구성. Idle 반복, Attack4 v2와 Hit4 단발, Defeat4 종료 시 마지막 셀 유지·Idle 덮어쓰기 방지. 기존 `#120` 전투불능 Idle 덮어쓰기 수정 재사용. 호출 순서/이벤트를 실제 기존 코드와 결합하되 다른 몬스터 공통 로직의 무단 리팩터링·행동 수치 변경 금지.
5. 공식 등록/전투 접점이 구현 가능하면 **기존 자료 계약만** 따른다: 빙각 수호체 `Lv21 HP820 ATK40 AGI9 EXP125 탈렌트32`, 전열 단회 엘리트, `얼음 주먹110%→팔들어올리기0·대상 예고→균열 강타180%` 반복, 도망 허용, 동상·동결괴사 신규 부여 없음. 본 공격 시트는 **기본 얼음 주먹용**. 예고/강타의 신규 애니메이션이나 텔레그래프를 임의 확정하지 않는다. 실제 기존 행동/예고 기능이 없거나 검증 불가하면 MonsterDefinition/전투 AI를 **기능 완성으로 위장하지 않고 HOLD**, 시각 바인딩·격리 QA까지 진행한다.
6. **F5 실존 Scene/공식 stable encounter ID/스폰 경로/단회 승리 Save키/전리품 수령 안전장치**가 실측 확인되는 경우에만 연결 가능 범위를 보고하고 추가 결합 게이트를 제시. 이번에는 F5 새 Scene 생성·임의 Spawn/Quest/Drop/Save 연결·`마력차단 결정` 100% 지급 활성화 **금지**. #122에서 승인된 전리품/드롭 준비 상태 보호.
7. 실제 **Unity 6000.6.5f1 격리 Play**의 몬스터 전투 UI **112px 기준** Idle→Attack→Idle, Idle→Hit→Idle, HP0→Defeat→최종 셀 유지, Victory/다른 몬스터 회귀, 잘못된 재생 순서·타격 중복·오류·필드 접근성을 최소 델타로 검증. **이미 PASS인 기존 개별 GIF·시트 검수 및 #126 초기 Animator QA를 처음부터 반복하지 말고 이번 신규 결합부만 테스트.** 배경/공식 실제 F5 전투가 불가능하면 `ISOLATED_RUNTIME_PASS / F5_PRODUCT_NOT_VERIFIED`로 정확히 분리.
8. 결과/미검증/변경 목록/원본 SHA/검수 캡처·로그를 `CODEX_LATEST.md`, 별도 `문서/00_프로젝트/Issue126_RimeSentinel_Runtime_Integration_20261123.md` 및 Issue #126 댓글에 기록. `브랜치 / 제품 커밋 SHA / 보고 커밋 SHA / PASS·PARTIAL·HOLD / 남은 차단 사항 / 사용자 승인 필요 여부` 제출. 직접 Windows 파일 접근 불가능하면 정직하게 보고. **원본 `main`, remote push/merge 미실행**.

## 보호·중단 조건

- `#85 HOLD`, 개인 Save/UserData, Discord, TTS/BGM/Mixer, 다른 승인 아트·기획 수치, Unity `6000.6.5f1` 보호.
- 새 에셋 5개 중 하나라도 무결성 불일치·유실 또는 현재 worktree 병합 충돌 시 **수정·대체 추측하지 말고 안전 범위까지만 완료, 명확히 HOLD 보고**.
- UI 포커스를 가져오는 foreground 에디터 조작은 대표의 별도 명시적 사전 허락 없이는 하지 않음.
- 사용자의 이번 “그래 하자”는 **격리된 실제 게임 연결 및 제한된 기술 QA 착수 승인**이고 `main`에 통합하거나 공개 배포·Push할 승인이 아님.
