# 서리들쥐 A 전투 스프라이트 제작 계약 — 2026-10-20

상태: **TECHNICAL_CONTRACT_LOCKED**. 미술 제작·Unity 구현은 이번 작업에서 수행하지 않았습니다. 제작 수치 미결 사항은 없습니다.

## 정본과 선택 근거

- [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30), 최신 우선 제작 지시 댓글 `6285672398` 확인.
- docs/planning의 `Chapter3_FrostFieldMouse_FirstCombatSprite_Contract_Preflight_20261020.md` (blob `539393bf68854210b3c7fb57a1c9c80da45e6a74`) 기준.
- `문서/08_몬스터/Chapter3_설빙의폭군_온기의화로_북부몬스터_상세기획_20261014.md` (blob `fc82dcdeb6ef07da3fa401f7d69670474ed34902`)의 M1 기본 공격은 **앞발로 할퀴기: 단일 물리 공격**입니다. 물기·신규 상태이상·방어 버프를 추가하지 않습니다. 외형은 이후 승인된 A 원본이 우선합니다.
- GitHub main 코드 기준 `12c69c72f9c09f716ee6242418b1d62f83a4a0d2`. 실제 Chapter2 정의 11~18은 Idle/Attack/Hit/Defeat 각 4프레임, 8 FPS입니다. 공식 시트는 1256×1256 / 셀314 / PPU314이나 신규 승인 자산의 셀512 / PPU512를 유지할 수 있습니다.
- **모션별 독립 Strip 3장**으로 확정합니다. MonsterDefinition의 명시적 Sprite 배열은 서로 다른 텍스처를 지원합니다. 승인 Idle 재배치·재압축이 필요 없고, 모션별 재납품 범위도 작습니다. 2048×2048 통합 시트는 납품하지 않습니다.

## 확정 규격표

| 모션 | PNG | 크기 / 프레임 | FPS / 명목 길이 | 종료 |
|---|---|---|---|---|
| Idle 재사용 | FrostFieldMouse_A_Idle_4Frames.png | 2048×512 / 512×512 가로4 | 8 / 0.5초 | 반복 |
| Attack 신규 | FrostFieldMouse_A_Attack_4Frames_v1.png | 동일 | 동일 | Idle00 복귀 |
| Hit 신규 | FrostFieldMouse_A_Hit_4Frames_v1.png | 동일 | 동일 | Idle00 복귀 |
| Defeat 신규 | FrostFieldMouse_A_Defeat_4Frames_v1.png | 동일 | 동일 | 03 유지 |

공통: RGBA PNG, 투명 배경, PPU512, 개별 Sprite Pivot **(0.5, 0)**, VisualScale1, 오른쪽 원화 1종. 새로 제작할 PNG는 정확히 3장입니다. 승인 Idle은 변경하거나 ZIP에 다시 넣지 않습니다.

```
왼쪽 → 오른쪽      00          01          02          03
픽셀 x          0..511     512..1023  1024..1535  1536..2047
시간(초)       0..0.125  .125..0.25  .25..0.375  .375..0.5
Attack         Idle00       준비       앞발 타격      회수
Hit            즉시 움찔    최대 반응      회복       Idle00
Defeat         힘 풀림      몸 낮아짐     옆으로 쓰러짐  완전히 쓰러짐
```

Attack00와 Hit03은 승인 Idle00 셀 RGBA를 그대로 복사합니다. 나머지 10셀은 각 역할에 맞는 신규 포즈입니다. 각 Strip 내부 4프레임을 같은 그림 복제로 채우지 않습니다. Hit은 공격 방향으로 전진하는 타격 포즈와 명확히 구분합니다.

## 발 접지와 미술

- 각 셀 좌상단 기준 Pivot 위치는 (256,512), 승인 Idle의 알파 하단 bbox는 y490 exclusive, 하단 여백22px입니다. 동일 접지 가이드 **y490**와 캔버스·원화 배율을 유지합니다. 보이는 발끝과 Pivot을 동일 위치로 오해하여 아래로 이동시키지 않습니다.
- Attack/Hit 지지 발·루트는 안정적으로 유지하되 들어 올리는 앞발은 움직여도 됩니다. Idle의 하단74px 전체를 전투 모션에서 고정할 필요는 없습니다. Defeat는 몸과 발 자세를 바꾸되 같은 지면에 쓰러집니다.
- 셀 경계 최소2px 투명 여유, 꼬리·털·발톱 잘림 및 고립 잡픽셀 없음. 자동 bbox 크롭·프레임별 리사이즈·가로세로 비율 변경 금지.
- 승인된 야생성·긴 꼬리·앞니·발톱·거친 털·긴장된 체형과 색상을 유지합니다. 마스코트 체형으로 되돌리지 않습니다.
- Attack은 오른쪽 앞발 할퀴기입니다. 과도한 궤적·이펙트·플래시·투사체·숫자를 굽지 않습니다. Hit 흔들림/플래시와 사망 색상·알파는 UI에도 있으므로 그림에 중복 적용하지 않습니다. Defeat03은 투명 소멸이 아닌 식별 가능한 쓰러진 몸입니다.

## Unity Import 및 재사용 연결 계약

- Sprite (2D and UI), Multiple, PPU512. 셀 Rect는 Unity 좌하단 좌표 `(0,0,512,512)`, `(512,0,512,512)`, `(1024,0,512,512)`, `(1536,0,512,512)`.
- 이름: `FrostFieldMouse_A_Attack_00`~`03`, `FrostFieldMouse_A_Hit_00`~`03`, `FrostFieldMouse_A_Defeat_00`~`03`. 승인 Idle 이름/meta/순서 보존.
- 모든 개별 Sprite Pivot 하단 중앙, Mesh Full Rect, Point, Clamp, MipMap Off, Compression None, sRGB On, Alpha Source Input / Alpha Is Transparency On, NPOT None, Max Size 2048 이상. 아트 담당은 meta를 납품하지 않고 향후 Codex가 신규 GUID로 Import합니다.
- 향후 Definition의 ExplicitIdle/Attack/Hit/DefeatFrames에 각 00~03을 연결하고 공통 animationFramesPerSecond를 **8로 명시**합니다(기본값10 방지). SourceFacesRight=true, VisualScale1. 314 자동 시트 생성기 또는 중앙 Pivot 텍스처 fallback은 사용하지 않습니다.
- Animator/AnimationClip 신규 제작 없이 MonsterSpriteSheetAnimation 재사용 가능. Idle 반복, Attack/Hit 1회 후 Idle00 복귀, Defeat는 .375초부터 03에 도달하여 오브젝트가 존재하는 동안 유지합니다.
- 전투 적 기본 UI Rect136×112 / preserveAspect에서 정사각 셀은112×112 표시, 승인 실루엣은 약102×80px입니다(Canvas 기준1280×720). 필드 셀은1 world unit입니다. 512px를 전투 화면512px로 확대하지 않습니다.
- 오른쪽 원화로 제작하고 반대 방향은 기존 X 반전으로 처리합니다. 전투 적 오른쪽 표시에는 X 양수. 별도 왼쪽/4방향 PNG는 요구하지 않습니다. 필드 방향 자동 지원을 새로 약속하지 않습니다.
- 기존 필드 Sorting Default/3, 공용 CircleCollider 반경0.4 등은 변경하지 않습니다. Walk 신규 제작도 이번 범위가 아닙니다.

## 기존 코드의 한계와 향후 통합 확인

- 일반 공격 경로 PlayAttack 사용. 현재 PlaySkill은 Hit 프레임을 재사용하므로 서리들쥐를 신규 강공격/Skill 경로로 연결하지 않습니다. 선택적 몸 낮추기나 별도 Skill은 이번 제작 범위 밖입니다.
- BattleActionPresenter의 기본 접근0.16초+정지0.09초 후 피해 호출은 명목0.25초입니다. 이에 Attack02를 타격 포즈로 정합니다. 애니메이션은 unscaledDeltaTime, Presenter는 scaled 시간이라 정확한 프레임 이벤트 동기는 보장되지 않습니다. 실제 프레임/TimeScale에서 향후 통합 Delta QA가 필요합니다.
- 기본 전체 행동은 약0.61초이므로 애니메이션0.5초 후 Idle로 돌아오는 것이 현재 동작입니다. 프레임을 늘려 맞추지 않습니다.
- PlayAttack이 reacting을 해제하지 않는 점, RestoreBattleIdle이 현재 Hit/Defeat 재생 상태를 종료하지 않고 Image.sprite를 설정하는 점은 소스에서 확인했습니다. 연속 Hit→Attack / 마지막 타격→Defeat 시 우선순위와 마지막 프레임 유지 여부를 통합 때 확인합니다. 이번에는 코드 수정·실행 재현을 하지 않았습니다.
- 기본 3모션은 엔진 신규 기능 없이 연결 가능하지만 프레임 이벤트·모션별 FPS·독립 Skill을 원한다면 별도 구현 승인이 필요합니다. MonsterID/수치/보상 기획은 여기서 정하지 않습니다.

## 납품 ZIP

`Limitless_Chapter3_FrostFieldMouse_A_Attack_Hit_Defeat_v1.zip`

```
Sprites/  위 신규 PNG 정확히 3장
Manifest.json
README.md
SHA256SUMS.txt
QA/Offline_QA.json
Preview/  4프레임 비교판, 128px 비교판, 125ms 간격 APNG 또는 HTML
```

Manifest에 실제 파일명/bytes/SHA256, 규격, 프레임 순서, 8FPS, PPU/Pivot/오른쪽/Scale 및 아래 승인 기준선 2개를 기록합니다. SHA256SUMS는 자기 자신을 제외한 납품 파일을 대상으로 작성하고 ZIP SHA는 생성 후 외부 납품 보고서에 기록합니다. 임의 해시를 미리 만들지 않습니다.

Preview는 Attack/Hit 00→03→기존 Idle 반복, Defeat 00→03→마지막 프레임 최소1초 유지로 제공합니다. QA는 PNG 디코딩/CRC/규격/셀 경계/알파/접지/루프를 기록합니다. Unity 코드·Scene·Controller·meta와 설빙의 폭군 자산은 포함하지 않습니다.

## 승인 원본 SHA256 및 검증

- 1포즈 `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`
- Idle `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2`

로컬 승인 ZIP에서 두 PNG SHA를 재확인했습니다. [소스 감사](Evidence/FrostFieldMouseCombatContract_20261020/SourceAudit.json)는 실제 코드6종·공식 정의8종을 대조한 **문서/소스 검사 PASS**입니다. 새 전투 모션 런타임 PASS를 뜻하지 않습니다. 기존 합격 QA는 재실행하지 않았습니다. [보호 검사](Evidence/FrostFieldMouseCombatContract_20261020/Protection.json)로 원본 파일·Git 상태를 확인합니다.

브랜치: `codex/chapter3-mouse-combat-contract-20261020`. 원본 Unity·공식 자산·승인 Idle·Save/UserData·Editor 변경0, Push/main 병합0. 제작 지시문은 [메인 아트 인계](FrostFieldMouse_Combat_Art_Handoff_20261020.md)를 사용합니다.
