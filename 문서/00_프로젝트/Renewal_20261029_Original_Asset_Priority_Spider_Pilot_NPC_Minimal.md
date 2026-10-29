# LIMITLESS / Chapter 1·2 리뉴얼 — 2026-10-29 바울 대표 우선순위·제작 단계 결정

**정본 범위:** Chapter 1·2 리뉴얼 아트. 담당 브랜치 `docs/art-renewal`. [Issue #60](https://github.com/HappyPaul812/Project-Limitless/issues/60), [Issue #63](https://github.com/HappyPaul812/Project-Limitless/issues/63), [10/28 조사](Renewal_Issue60_GateGuard_Taeon_Spider_Review_20261028.md) 및 Codex #60 최신 로컬 감사 결과 반영. **이번 회차는 문서·작업계획만 변경**. 새 PNG·기존 Unity 원본/세이브/GUID 변경 없음.

## 1. 정책 변경 — 디자인의 목적은 '오리지널화'

### 일반 NPC·퀘스트 NPC — 최소 투자

- 초상화 없이 제자리에 서 있는 배경형 상인·주민·경비·퀘스트 NPC는 **과도한 캐릭터성·고급 세부 묘사·반복 polishing 금지**. 목표: 제3자 공용 자산을 **LIMITLESS 고유 자산으로 대체**하면서 **기본 역할 구분·스타일 통일·128px/40px 가독성**을 보장.
- 기능 계약: NPC StableID, Quest 대화/완료, E키 상호작용, 상점, 좌표/정렬/Collider/게임 동선 그대로. 별도 초상화·리깅·고비용 애니메이션을 신규 요구하지 않음.
- OGA03 잡화상, OGA09 주민대표, OGA20 남문 경비 **이전 시안 3종과 ZIP 모두 보존**. 이전 태온 유사성 피드백은 무효화하지 않되 **머리/상징/창/파란 색면 등 역할 오인 방지 최소 차이**만 필요시 확인. 공식 태온 PNG와 OGA20 후보가 미확보인 실제 게임 A/B는 **NOT_VERIFIED**; 이를 이유로 장기 polishing·새 원화 전면 재제작하지 않음.
- NPC는 상업적 배포권·고유성, 최소 가독성, 기능 보호를 충족하면 승인 검토에 제출하고 종료. **배경 NPC를 주요 동료급 퀄리티로 재제작하지 않는 방침**.
- **우선순위:** 실사용 **비오리지널 전투 몬스터**의 리뉴얼을 일반 NPC보다 먼저 진행. 기존 승인된 공식 Chapter 1·2 아트·펫/환경/보스는 재제작 제외.

## 2. forest_spider — 오리지널 리뉴얼 대상 확정, 단계별 게이트

**사용자 결정:** 외부 `ethanf30games / 2D Spider` 유래 자산은 CC0 허용 여부와 관계없이 **LIMITLESS 오리지널 자산이 아님**. 그러므로 `FOREST_SPIDER_ORIGINAL_REPLACEMENT_REQUIRED`, 리뉴얼 큐 **P1**에 등록. 과거 조사 문서의 `KEEP_CURRENT`/신규 `HOLD`는 **원본 제품의 안전한 임시 사용·아직 미제작** 상태만 의미하며 **교체하지 않겠다는 정책이 아니다**.

### 확인된 실사용 계약 (바이트·기술 근거 구분)

- 현재 `MonsterDefinitions/03_ForestSpider.asset` `forest_spider`, Field02 4곳·Field03 1곳 Spawn, 런타임 `FieldMonsterInstaller`/Sprite 프레임 경로; `MonsterAnimations/Spider/{Idle,Walk,Attack,Shoot}` **각 8PNG, 총 32PNG**, 10 FPS, `sourceFacesRight=1` (우향), `visualScale=0.7`.
- 첫 PNG 확인 치수: Idle 336×226, Walk 359×229, Attack 325×228, Shoot 357×236. `.meta` PPU256·중앙 Pivot/Point. 전체 32장 개별 캔버스 규격과 Unity 실제 GameView 재생은 별도 Codex 검증 항목으로 남기되 전체 대형 감사를 반복하지 않음.
- 보존 원본: `Assets/ThirdParty/ethanf30games/2DSpider` / https://opengameart.org/content/2d-spider **CC0 1.0**, 제작자 `ethanf30games`. **Codex #60 로컬 감사에서 현행 32PNG와 ThirdParty 원본 ZIP 대응 32/32 SHA256 일치** 보고. 이는 Codex 로컬 조사 근거이며 리뉴얼 담당이 로컬 F: 자체 SHA를 재계산한 사실은 아님. 오래된 README의 '미연결'은 현행 Spawn/Runtime 참조와 불일치.
- 게임 로직·MonsterDefinition·스폰·공격·독액/드롭·AI·Save 및 기존 32PNG/GUID/meta는 적용 승인 전까지 **그대로 보존**.

### 제작 단계 (자동 일괄 진행 금지)

**Phase 1 — 오리지널 디자인 파일럿 (다음 아트 제작의 우선순위):**
- 기존 Spider 복제·트레이스·단순 리컬러가 아닌 **완전히 독립된 숲거미 1포즈**. Chapter 2 공식 몬스터와 조화되는 명암/질감, 작은 화면에서도 보이는 8다리·체절·눈/입·독액 분사 방향, 숲 환경 명확한 실루엣.
- **후보 1포즈만** 제작; 기존 Spider 실제 프레임과 128px/40px 동일 표시 크기 비교, 명암 다른 2~3 배경 시인성, RGBA 알파/독자성·외형 과장 금지 검토.
- 후보 PNG·프리뷰·비교판·간단 Manifest/QA·ZIP/SHA256을 독립 산출물로 납품. 원본 32프레임 교체·Unity 정식 적용 금지. 기존 CSS/인터랙션 등 게임 코드 수정 금지.
- 이번 10/29 지시는 **단계 계획 문서화**이며 **1포즈의 실제 제작은 아직 하지 않음**. 다음 단계에서 디자인 파일럿 제작을 시작할 수 있으나 사용자 승인 없는 애니메이션 진행은 금지.

**Phase 2 — 바울 대표 미술 검토 및 디자인 승인:**
- 실물 128px·40px 후보와 구형 Spider/Ch2 공식 몬스터 비교판으로 `APPROVED / TARGETED_FIX / REJECTED` 명확히 구분. **디자인 미승인 상태로 Phase 3 진행하지 않음**.
- 수정 시 실루엣/독침 위치/색면 등 **지적된 부분만 표적 보완**; 무의미한 전체 리드로잉·반복 polishing 금지.

**Phase 3 — 승인 후 애니메이션 교체 계약:**
- 승인된 동일 디자인으로 `Idle8 / Walk8 / Attack8 / Shoot8` **4동작 총 32프레임 호환 목표**(기존 프레임·10FPS·우향·PPU/Pivot/ResourcePath 유지 원칙). 셀 크기 변경이나 애니메이터 코드 변경은 선행 Codex 격리 검증·바울 대표 별도 승인 전 확정 금지.
- **키포즈 소수 → 중간 시각 검수 → 나머지 프레임 제작 → 10FPS 연속성/128px·40px/알파/실 Unity 격리 QA → 대표 최종 승인 → 안전한 본편 적용** 순서. 보스/펫/다른 몬스터는 이번 작업에 포함하지 않음.
- `forest_spider` 기존 MonsterDefinition ID, Field02/03 Spawn, HP·독액·전투 로직·드롭·퀘스트·Save/GUID/meta 및 사용자 PC 파일 수정 불가. 승인 전 `NO_UNITY_APPLY / NO_MAIN_MERGE`.

## 3. 독침벌 및 기승인 자산 보존

- 독침벌 `venom_bee` 신규 좌향 1포즈 `TECH_PASS`/실전 우향 Idle10+Attack10·10FPS 교체 `ART_HOLD`; 바울 대표 디자인 미승인. **신규 키포즈·전체 20프레임 제작 0**, v1 후보 변경 0.
- OpenGameArt CC BY 3.0 vs 원본 LICENSE CC BY 4.0 **충돌 미해결**, 출처·권리 조사와 제작 계약 원형 유지.
- 공식 Wolf/Bear/Fox, Chapter 2 공식 몬스터·보스/기존 승인 환경, NPC OGA03·09·20 후보, 54행 감사, Unity·Save·UserData·Scene·Quest·Sprites·GUID/meta 및 `main` **변경 0**.

## 4. 우선순위 작업 큐 / 실행 권한

| 순위 | 작업 | 상태 | 다음 허용 단계 |
|---|---|---|---|
| P1 | forest_spider 오리지널화 | **교체 결정 확정** / 기존32프레임 운영 KEEP | **신규 원화 1포즈 디자인 파일럿** (추후 제작 회차) → 대표 승인 → 애니메이션 |
| P2 | 그 외 Chapter1·2 실사용 외부 전투 몬스터 | 별도 실사용·기승인 여부 확인 | 이번 발주 밖, 선별 후 1종씩 |
| P3 | OGA03·09·20 및 일반 마을/퀘스트 NPC | **최소투자 원칙** / 기존 시안 보존 | 역할 구별·기능 보호 확인, 불필요한 재제작 STOP |
| 유지보류 | VenomBee v1 | 1포즈 TECH PASS / 교체 HOLD | 대표 미술 승인 전 원화·애니메이션 수정 없음 |

**최종 판정:** `DIRECTION_APPROVED / SPIDER_RENEWAL_QUEUE_CONFIRMED / SPIDER_PILOT_NOT_STARTED / NPC_MINIMAL_INVESTMENT / VENOMBEE_HOLD / GAME_MAIN_UNCHANGED`.

**근거:** [#60 Codex 로컬 32/32 직접검수 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6470843053), [#72 10/29 읽기전용 비교 보고](https://github.com/HappyPaul812/Project-Limitless/issues/72#issuecomment-6485438666). 두 로컬 Codex 브랜치는 `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`; 이를 원격 바이너리로 오인하지 않음.
