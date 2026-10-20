# Chapter3 승인 A·B Idle 제작 기술 계약 — 2026-10-20

상태: **TECHNICAL_CONTRACT_READY / IDLE_NOT_CREATED / UNITY_NOT_APPLIED**.
기획·신규 아트 담당 인계본. 이번 사용자 요청에 따라 제작 수치를 확정한 문서이며 새 몬스터 구현·애니메이션 제작·게임 적용 승인은 포함하지 않는다. 납품 후 격리 화면 QA와 바울 대표의 움직임 미술 승인이 필요하다.

## 1. 정본과 조사 범위

- [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30), [사용자 1포즈 승인 정본](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Chapter3_ApprovedAB_1Pose_ArtRework_v2_UserApproval_20261020.md), [신규 아트 사전조사](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-new/문서/00_프로젝트/Chapter3_ApprovedAB_Idle_Animation_Technical_Preflight_20261020.md), Issue #35를 확인했다. 사전조사의 미확정 제작 수치를 아래에서 잠근다.
- AGENTS, PROJECT_CONTEXT, CURRENT_STATUS, CODEX_WORKFLOW, COMMON_RULES, CODEX 담당·최신 인계 확인. 작업 시작 기준 커밋 `7f9ca843f42d181ef2256aab6caa7d549e4f6a9d`.
- Chapter2 11~18 MonsterDefinition 8개와 실제 시트 메타, Field 설치기, 공용 프레임 재생기, BattleSceneController를 읽기 조사했다. 원본 Editor는 Bootstrap, 비실행 상태이며 변경하지 않았다.
- GitHub main과 로컬 재생기·BattleSceneController는 Git blob 기준 각각 `67d6d69195e068e691de1cd7c4594829f278c0da`, `7f5f9ce75bcba84ff2117f106b5536b5a57a379e`로 일치한다. 로컬 CRLF/LF 차이는 코드 차이로 세지 않았다.
- 기존 시각 검수와 합격 QA는 재사용한다. 아래 표시 크기는 **현재 코드의 UI 수식 및 승인 PNG 알파 bbox 계산값**이다. 신규 Idle의 실제 GameView 재생 합격을 주장하지 않는다.

## 2. 승인 원본 보호

ZIP: `F:\Downloads\Limitless_Chapter3_ApprovedAB_1Pose_ArtRework_v2.zip`

ZIP SHA256: `0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919`.
ZIP CRC 및 두 원본 PNG 디코딩·해시 재확인 PASS. 둘 다 512×512 RGBA.

| 대상 | ZIP 내 승인 원본 | SHA256 |
|---|---|---|
| 서리들쥐 A | `Sprites/FrostFieldMouse_A_1Pose_Rework_v2.png` | `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2` |
| 설빙의 폭군 B | `Sprites/FrostTyrant_B_1Pose_Rework_v2.png` | `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830` |

원본 ZIP/PNG는 수정하지 않는다. 승인된 해부학·체형·색·실루엣을 유지한다. Idle00은 승인 PNG의 512×512 RGBA 픽셀을 **재샘플링·이동·트리밍 없이 그대로** 첫 셀에 복사한다. 합성 PNG 파일 해시는 달라도 첫 셀의 디코딩 RGBA가 승인 원본과 같아야 한다.

## 3. Chapter2 실제 구현 기준

| 항목 | 일반 몬스터 11~17 | 열맥 거신 18 |
|---|---|---|
| 공식 시트 선례 | 1256×1256 / 4열×4행 / 셀314×314 | 동일 |
| Sprite Import | Multiple, PPU314, Point, 개별 Rect Pivot(0.5,0) | 동일 |
| 명시적 Idle / Attack / Hit / Defeat | 각4프레임 / 8FPS | 동일 |
| Walk | 11~15 배열 없음→이동 중 Idle; 16~17은 배열4 | 배열4에 Idle 재사용 |
| 방향 | 우향 원본1종; 방향별 행 아님 | 동일 |
| VisualScale | 1 | 3 |
| Field 사각 프레임 폭·높이 | 1×1 world | 3×3 world |
| Battle Image Rect / 정사각 Sprite 표시 | 136×112 / 112×112 UI단위 | 408×336 / 336×336 UI단위 |

공식 4행은 위부터 Idle00~03 / Attack00~03 / Hit・Skill 공유00~03 / Defeat00~03. `.asset`의 `idleFrameCount:1`은 사용하지 않는 Texture2D 경로의 기본값이다. 실제 우선 적용 배열 `explicitIdleFrames`는4개다.

`MonsterDefinition` → `MonsterSpriteSheetAnimation.Configure` → Field `SpriteRenderer.sprite` / Battle `Image.sprite`로 연결된다. 11~18의 `fieldAnimatorController`는 null이며 이 경로에 AnimationClip·AnimatorController 신규 제작은 필요 없다. 기존 슬라임의 4방향 Animator는 다른 구현이다.

Field 설치기는 Visual 자식 Scale만 적용하고 Sorting Layer는 기본 Default, Order3이다. 루트 CircleCollider2D 반경0.4는 공용 설치기의 현재값이며 그림 크기에 비례 확대되지 않는다. 보스 전용 충돌 규격을 의미하지 않는다. Battle은 Collider 대신 UI HitArea를 사용하며 CanvasScaler 기준1280×720, match0.5다. `VisualSize`는 이 애니메이션 경로의 실제 크기 계산식에 쓰이지 않는다.

Idle는 누적 `unscaledDeltaTime × FPS`의 정수 인덱스를 프레임 수로 나눈 나머지로 반복한다. Attack·Hit·Skill은 배열 끝에서 Idle로 복귀하고 Defeat는 마지막 프레임을 유지한다. FPS는 Definition당 하나여서 상태별 FPS를 따로 지정할 수 없다.

## 4. 확정 Idle 납품 규격

| 제작 항목 | 서리들쥐 A | 설빙의 폭군 B |
|---|---|---|
| 납품 파일명 | `FrostFieldMouse_A_Idle_Pilot_v1.png` | `FrostTyrant_B_Idle_Pilot_v1.png` |
| PNG | **2048×512 RGBA**, 투명 배경 | 동일 |
| 셀 / 배치 | **512×512, 가로4셀×1행**, 간격0·여백0 | 동일 |
| 프레임 / FPS | **4 / 8FPS**, 各0.125초, 0.5초 루프 | 동일 |
| PPU / Pivot | **512 / (0.5,0)** 하단 중앙, 네 셀 동일 | 동일 |
| 방향 | 승인 우향1종, 별도 상하좌우 행 없음 | 동일 |
| VisualScale 제작 기준 | **1** | **3** |
| Field 전체 셀 | **1×1 world** | **3×3 world** |
| Battle Image Rect | **136×112 UI단위** | **408×336 UI단위** |
| 실제 정사각 셀 표시 | **112×112 UI단위** | **336×336 UI단위** |
| 승인 포즈 알파 실효 표시 | **102.375×80.063 UI단위** | **307.125×305.156 UI단위** |
| 승인 포즈 알파 실효 Field 크기 | **0.9141×0.7148 world** | **2.7422×2.7246 world** |

**512 셀 선택 이유:** 기존314 셀은 아트 해상도 선례이며 엔진 고정 상수가 아니다. 명시적 Sprite 배열은 임의 Rect를 받아 재생한다. 승인512 원본을314로 축소하면 승인 디테일이 손실되므로 원본 셀을 보존하고 PPU를512로 맞춘다. `셀폭/PPU=1`을 유지하여 기존 Field 상대 크기와 Battle UI 크기는 동일하다. 2048폭은 기존 Import 최대2048 선례에도 맞는다. 이 선택은 새 엔진 규격 구현을 요구하지 않는다.

Battle 값은1280×720 참조 좌표계다. 실제 디스플레이 픽셀은 CanvasScaler 배율을 추가 곱한다. 128px 비교판은 전체512 셀을128로 표시하며 알파 bbox만128로 늘리지 않는다. 일반 몬스터와 보스는 각각 Scale1 / Scale3 그룹끼리 비교한다.

그을음들개의 첫 Idle 알파313×296/314는 약111.64×105.58 UI단위다. 서리들쥐는 그보다 폭 약92%, 높이 약76%이며 승인된 낮은 설치류 체형을 보존한다. 열맥 거신 알파291×270/314는 약311.39×288.92 UI단위다. 폭군은 폭 약99%, 높이 약106%로 기존 보스 체급을 유지한다. 위압감은 기존 v2 승인 결과를 재사용하며 새 호흡 동작의 무게감은 납품 후 별도 판단한다.

## 5. 배치도·좌표·원점

```text
PNG 좌상단(0,0)                                  우하단(2048,512)
┌────────────┬────────────┬────────────┬────────────┐
│ Idle_00    │ Idle_01    │ Idle_02    │ Idle_03    │
│ 승인 원본  │ 호흡 진행  │ 호흡 정점  │ 원점 복귀  │
└────────────┴────────────┴────────────┴────────────┘
  x=0          x=512        x=1024       x=1536
재생: 00 → 01 → 02 → 03 → 00 ... (마지막에 00 추가 금지)
```

Unity SpriteRect는 좌하단 원점으로 `(x,y,w,h)=(0,0,512,512), (512,0,512,512), (1024,0,512,512), (1536,0,512,512)`.
Sprite 이름은 각각 `FrostFieldMouse_A_Idle_00`~`03`, `FrostTyrant_B_Idle_00`~`03`. 네 Sprite를 `ExplicitIdleFrames`에00→03순으로 직접 연결하는 것이 계약이다.

각 셀 내부의 승인 원본 좌표를 고정한다. 알파 bbox는 PNG 좌상단 기준 우하단 배타 범위 A `[22,124,490,490)`, B `[22,25,490,490)`이다. 두 원본의 알파 하단은 y490 경계이고 하단 투명 여백22px다. Idle01~03도 **발 접점의 기존 위치와22px 하단 여백을 고정**하고 프레임별 자동 중심맞춤·Crop·몸 전체 상하 이동을 하지 않는다. 털·수염·가슴 등 소폭 움직임에 여백을 쓰되 캔버스 밖으로 뿔·꼬리·발톱이 잘리면 반려한다. 임의로 새 테두리나 배경을 추가하지 않는다.

Sprite Pivot는 기존과 같은 하단 중앙(셀 좌표256,512)이며 실제 알파 발끝과 같지 않다. 승인 위치를 보존하므로 발끝은 Field 원점보다 A 약0.043world, B 약0.129world 높다. 이번 계약에서 원본을 아래로 이동시키거나 Pivot를 중앙으로 바꾸지 않는다. 향후 Field 실제 배치에서 지면 접촉 보정이 필요하면 별도 배치 QA 대상으로 보고하며 신규 Collider·원점 보정 기능을 이번에 구현하지 않는다. Battle Image는 Rect 중앙 배치 경로이므로 Sprite Pivot 변경으로 Battle 발 위치를 조절하지 않는다.

## 6. Import·연결 인계

향후 **격리 검수 환경에서만** 적용할 Import 계약: Texture Type Sprite(2D and UI), Multiple, Point, Clamp, Mipmap OFF, sRGB ON, Alpha Source Input Texture, Alpha Is Transparency ON, Compression None, Max Size2048 이상·NPOT None(비례 축소 금지), PPU512. Full Rect와 위 Rect4개/하단 중앙 Pivot를 명시한다. 기존 Main18ContentBuilder의314×314/4×4용 자동 분할을 그대로 실행하지 않는다. 새 파일의 `.meta`만 새 GUID로 생성하며 공식 기존 PNG/.meta를 덮어쓰지 않는다.

`ExplicitIdleFrames=[00,01,02,03]`, `FieldSprite=00`, `AnimationFramesPerSecond=8`, `SourceFacesRight=true`, `VisualScale=A1/B3`를 향후 데이터 연결값으로 전달한다. 이번에는 MonsterDefinition도 만들지 않았다. 런타임 `CreateFrames(Texture2D)`는 중앙 Pivot(0.5,0.5)를 만드는 다른 경로이므로 사용하지 않는다. `.anim`·`.controller` 납품은 불필요하다.

납품 ZIP에는 위 PNG2개와 Manifest(파일명/역할/SHA256/2048×512/셀512/4프레임/8FPS/PPU512/Pivot), README(방향·배치·접점 고정·승인 원본 SHA), 8FPS 루프 Preview를 포함한다. Preview는 검토용이며 게임 자산이 아니다. 4장을 복제한 정지 그림이나 원본의 단순 확대만으로 Idle 제작 완료라 하지 않는다.

## 7. 후속 상태 확장과 구현 경계

- 후속 제작 승인 후 상태별 **동일512셀·4프레임·8FPS·2048×512 스트립**을 추가한다. 권장 이름은 `FrostFieldMouse_A_Attack_v1.png` / `Hit_v1` / `Defeat_v1`, B도 `FrostTyrant_B_...` 방식이다. Idle 스트립을 교체하거나 빈12셀을 미리 만들 필요 없다.
- 각 상태00~03을 ExplicitAttackFrames / ExplicitHitFrames / ExplicitDefeatFrames에 연결하면 기존 재생기를 재사용한다. Attack·Hit0.5초 후 Idle, Defeat0.5초 이후 마지막 포즈 유지가 기준이다. 전투 Presenter가 액션 완료 시 복귀를 먼저 호출할 수도 있으므로 후속 공격 접촉 타이밍은 행동 코드와 별도 조율한다.
- 현재 `PlaySkill()`은 **Hit 배열을 공유**한다. 피격에 공격 발사·냉기 폭발을 그려 넣으면 부적합하다. 독립 Skill 동작·독립 FX·상태별 FPS·애니메이션 이벤트 기반 타격을 원하면 데이터/재생기/전투 호출의 추가 구현과 별도 승인이 필요하다.
- `BattleSceneController` 일반 공격은 `PlayAttack`, 강한 연출은 `PlaySkill`; HP 감소 시 `PlayHit`, 최초 사망 시 `PlayDefeat`, 행동 종료 시 `StopAttackAndReturnToIdle`를 호출한다. 이동은 `SetFieldMoving`과 Walk 배열 유무로 선택된다. 별도 Walk는 이번 발주 대상이 아니며 없음이면 Idle을 사용한다. Field 프레임 경로는 자동4방향 전환을 제공하지 않는다.
- Idle 재생 자체에 새 엔진 기능은 필요 없다. 향후 새 Definition/프레임 Import/스폰·전투 데이터 연결과 Chapter3 보스 행동 구현은 별도 작업이다. 몸집3배에 맞춘 Collider 변경, 새 Scene, Save 구조, F1·서릿길 환경은 본 계약 범위 밖이다.

## 8. 검증·인계 결과

**정적 기술 검증 PASS:** 승인ZIP CRC·원본 SHA2종, 실제512 RGBA·bbox, Ch2 Definition8개 Idle4/8FPS, 명시적 프레임 재생·크기 계산 호환성을 확인했다. 근거는 [Validation.json](Evidence/Chapter3AB_IdleContract_20261020/Validation.json). 기존 비교판은 [v2 검수 문서](Chapter3_AB_ArtRework_v2_ReadOnly_Review_20261020.md)를 재사용한다.

**미실행:** 신규 Idle 제작·Import·GameView 재생·애니메이션 미술 승인. 이번 결과를 애니메이션 QA PASS로 간주하지 않는다. 기존 전체 QA를 다시 실행하지 않았다.

기획 담당 인계: 본 문서4~6절을 Idle2종 발주 정본으로 전달하고, 신규 아트 사전조사의 Cell/PPU/Scale TBD를 이 값으로 대체한다. 다음 단계는 Idle2종 납품 → 격리 Import·루프·표시 크기·접점 QA → 바울 대표 미술 승인 → 후속 상태 별도 승인이다. 원본 적용·Push·main 병합은 보류한다.

작업 브랜치: `codex/chapter3-ab-idle-contract-20261020`. 문서 커밋은 CODEX_LATEST의 최종 인계 항목에 기록한다.
