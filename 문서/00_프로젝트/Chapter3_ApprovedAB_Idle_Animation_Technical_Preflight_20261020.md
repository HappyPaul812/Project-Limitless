# LIMITLESS — Chapter3 서리들쥐 A·설빙의 폭군 B Idle 애니메이션 기술 사전조사 (2026-10-20)

> **REMOTE_SOURCE_VERIFIED / LOCAL_CODEX_CONTRACT_PENDING / ANIMATION_NOT_STARTED / UNITY_NOT_MODIFIED**.
> 담당: Chapter3 이후 신규 아트(`docs/art-new`). **이 문서는 Codex 로컬 Unity 기술 규격 확정 전의 정본 근거 목록 및 제작 준비 인계**다. Codex가 직접 로컬 worktree/실제 Editor 상태를 확인했다거나 Animation 규격을 최종 잠갔다고 주장하지 않는다.
> 기획 승인 정본: `docs/planning/문서/00_프로젝트/Chapter3_ApprovedAB_1Pose_ArtRework_v2_UserApproval_20261020.md`; Issue [#30](https://github.com/HappyPaul812/Project-Limitless/issues/30). Codex 기술 조사 후 메인 아트 담당에게 다시 규격 확정본 전달 필요.

## 승인 원본 바이트 보호

- 로컬 대화 작업공간에서 `Limitless_Chapter3_ApprovedAB_1Pose_ArtRework_v2.zip` SHA256 **`0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919`** 확인. 바울 대표 PC `F:\Downloads\` 경로 자체를 열지는 못했음.
- `Sprites/FrostFieldMouse_A_1Pose_Rework_v2.png` SHA256 **`0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`**.
- `Sprites/FrostTyrant_B_1Pose_Rework_v2.png` SHA256 **`f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830`**.
- 두 PNG는 512×512 투명 RGBA *승인된 정적 기준 포즈*이며 그대로 보존. 이후 새 Idle frame을 만들어도 원본 PNG를 덮어쓰지 않음.

## GitHub main 실제 구현 조사

1. `Unity/Client/Assets/_Project/Scripts/Monster/MonsterDefinition.cs`
   - 명시적 `ExplicitIdleFrames`, `ExplicitWalkFrames`, `ExplicitAttackFrames`, `ExplicitHitFrames`, `ExplicitDefeatFrames`; `AnimationFramesPerSecond`, `VisualScale`, `SourceFacesRight`, `VisualSize`. `ConfigureContent`는 `animationFramesPerSecond=8f`, `sourceFacesRight=true` 기록.
   - `MonsterSpriteSheetAnimation`은 `ExplicitIdleFrames`가 있으면 사용. 필드의 `SpriteRenderer`와 전투 UI `Image`에 같은 Sprite 프레임을 제공.
2. `Unity/Client/Assets/_Project/Scripts/Monster/MonsterSpriteSheetAnimation.cs`
   - Idle 반복 `floor(unscaledDeltaTime 누적 × fps) % frameCount`; Attack/Hit/Skill 종료 후 Idle 복귀, Defeat는 마지막 프레임 유지.
   - **주의**: `CreateFrames(Texture2D,...)` 런타임 수동 분할은 `pivot=(0.5,0.5)`, `PPU=frameSize.x`. 반면 공식 TextureImporter 분할 Sprite는 하단 중앙 Pivot `(0.5,0)`. 바이트 보존·발밑 고정이 중요하므로 두 방식을 섞지 말 것. 신규 Idle 파일럿은 `ExplicitIdleFrames` 방식 우선 검토.
3. `Unity/Client/Assets/_Project/Resources/MonsterDefinitions/11_SootHound.asset`, `16_ObsidianBeetle.asset`, `17_ScorchingWatcher.asset`, `18_VeinfireColossus.asset`
   - 전부 `animationFramesPerSecond:8`, `sourceFacesRight:1`, `explicitIdleFrames:4`가 GitHub YAML에 보인다.
   - `visualScale`: 그을음들개 **1**, 흑요석 갑충 **1**, 작열 감시자 **1**, 열맥 거신 **3**. `visualSize`: 그을음들개 `0.9×0.9`, 갑충/감시자 `1.25×1.25`, 거신 `3×3`. 이는 기존 구성의 값일 뿐 Chapter3 신규 MonsterDefinition 값 아님.
4. 기존 시트·Import 선례:
   - Soot Hound, Obsidian Beetle, Veinfire Colossus 공식 시트 `1256×1256`, **4×4/314×314 셀**, `.png.meta` `spritePixelsToUnits:314`, `filterMode:0` (Point), `spriteMode:2`(Multiple), 각 Sprite Rect 내 `pivot=(0.5,0)`; 16프레임 위쪽 행부터 읽는 구조.
   - 공용 4행 의미: `00~03 Idle`, `04~07 Attack`, `08~11 Skill/Hit 공유`, `12~15 Defeat`. 거신은 `explicitWalkFrames`에도 Idle4를 재사용, 모든 Ch2 몬스터에 이동 애니메이션이 실제 별도 제작됐다고 주장하지 않음.
   - 방향별 Animator(초원 슬라임)는 `Idle/Walk_Down,Left,Right,Up` 상태를 쓰는 **다른 경로**. Chapter2 신규 시트 경로는 `sourceFacesRight=true`·전투에서 방향 선택용 `Image.flip` 대신 `rectTransform.localScale.x` 반전 사용. 따라서 두 신규 적에게 4방향을 **첫 Idle 파일럿 필수로 요구할 근거 없음**.
5. `Unity/Client/Assets/_Project/Scripts/Monster/FieldMonsterInstaller.cs`
   - 공용 SpriteSheet 몬스터는 `visual.transform.localScale=VisualScale`; `SpriteRenderer.sortingOrder=3`(별도 sortingLayer 명시 없음); `CircleCollider2D.radius=0.4`가 생성기 공통 선례. 신규 Boss Collider 계약으로 확정하면 안 됨.
6. `Unity/Client/Assets/_Project/Scripts/Battle/BattleSceneController.cs` 실제 구현
   - 몬스터가 시트 재생 경로이면 전투 `Image` 크기 기본 **136×112 UI px × MonsterDefinition.VisualScale**, `preserveAspect=true`; 아군·기타 타입은 다른 규칙.
   - 기존 Scale1인 갑충/감시자는 명목 Image rect **136×112**, Scale3 거신은 **408×336**. 이는 UI Rect 기준일 뿐 실제 알파 실효 크기/화면 스케일/전장 UI 카메라에서의 체감 높이와 구별 필요.
   - 적은 전장 왼쪽에서 오른쪽을 바라보며 `SourceFacesRight` 여부에 따라 X 반전. 실제 게임 UI의 표시/전투 파티 위치와 충돌 여부는 로컬 Play/Frame 검증 전 미확정.

## 두 신규 대상 Idle 규격 — **기존 선례로 확인된 값과 미잠금 항목 분리**

| 항목 | 서리들쥐 A | 설빙의 폭군 B | 검증 성격 |
|---|---|---|---|
| 원본 정적 승인 | 위 512×512 RGBA, SHA | 위 512×512 RGBA, SHA | **확정/보호** |
| Idle frame count | **4 프레임 선례** | **4 프레임 선례** | 기존 Ch2 구조 확인; 신규 제작 잠금은 Codex 필요 |
| FPS | **8 FPS 선례** | **8 FPS 선례** | 기존 MonsterDefinition 기존 값 |
| SpriteSheet Import PPU | **314 선례** | **314 선례** | Ch2 314px 셀 사례에 한함 |
| Frame pivot | **(0.5,0) 선례** | **(0.5,0) 선례** | `.meta`의 Sprite Rect 값 |
| Direction | 승인 정적 포즈 우향 기반으로 **1방향 Idle 파일럿 가능** | 동일 | 별도 4방향 의무 없음, 방향 최종계약 필요 |
| Sprite 실제 표시 크기 | **TBD** (Ch2 Scale1 Image rect=136×112 UI px 참조) | **TBD** (Ch2 거신 Scale3 Image rect=408×336 UI px 참조) | 신규 VisualScale/알파 실효 크기/전장 자리 미확정 |
| Idle 파일명 후보 | `FrostFieldMouse_A_Idle_Pilot.png` | `FrostTyrant_B_Idle_Pilot.png` | 네이밍 제안, 최종 Codex 검토 필요 |
| 후속 모션 | Attack / Hit(및 Skill 공유) / Defeat | Attack / Hit(및 Skill 공유) / Defeat | 선례 확인, 프레임 추가 발주 **미승인** |

### 제작자가 사용할 임시 배치안 (정식 Slice 계약 아님)

기존 314px 셀과 4 Idle 프레임을 **그대로 재사용하는 방안**은 아래와 같다. 승인된 원본이 512px 그림인 만큼 314 셀에 억지 단순 축소하지 말고 새 프레임 제작 전에 **Codex 기술 계약으로 알파 bbox·최종 캐릭터 실효 px·보간/채색을 결정**해야 한다.

- 후보 A — **가로 4셀 Idle 전용** `1256×314`, 각 `314×314`, 프레임 순서 `[Idle00][Idle01][Idle02][Idle03]`, 발 기준선 정렬. 엔진의 `ExplicitIdleFrames` 재사용은 가능하지만 `Main18ContentBuilder`의 전체 4×4 자동 분할 스크립트를 그대로 적용하면 안 되고, **Codex가 4셀 SpriteRect 별도 Import 절차**를 지정해야 한다.
- 후보 B — 공식 선례 **4×4 1256×1256** 중 첫 행 00~03만 Idle을 사용하고 다른 행 12칸은 빈 미사용 데이터로 둠. 기존 16셀 Import 선례와 모양은 같지만 Idle 소형 파일럿 목적에는 공간 낭비이므로 임의 채택하지 않음.
- **잠금 판단 필수:** 512 승인 포즈의 픽셀 표현을 손실 없이 유지하는 것이 314 Cell의 밀도/스케일보다 우선인지; 1포즈 기반 4프레임을 별도 PNG4장으로 제공할지. 현재 로컬 Codex/Unity Editor 확인 전 어느 쪽도 확정하지 않는다.

## 후속 Attack/Hit/Defeat 제약

- 일반 Idle4를 기반으로 나중에 Attack4, Hit/Skill 공유4, Defeat4(마지막 유지)를 추가할 수 있는 엔진 경로 있음. 새 공격 스킬별 독립 6행 시트 등을 임의 발주하지 않음.
- 3행 공용 Hit/Skill을 사용하게 되면 **피격 시 공격 에너지가 발사되는 완성형 FX 금지** (Main20 검수 교훈). 보스 냉기 FX는 별도 분리.
- 애니메이션마다 같은 셀 크기, 동일 발 접지선/Pivot, 실루엣 폭·뿔·꼬리·발톱 Crop 0, Alpha 이상 조각 0; 원본 승인 디자인·색·체형/해부학 유지. Motion 변형은 가슴/호흡·수염/털·적설 흔들림 같은 소폭 Idle만 허용.
- `MonsterSpriteSheetAnimation`은 `UnscaledDeltaTime`으로 동작하고 `AnimationFramesPerSecond`로 전환; 복귀/Defeat 정의는 그대로 활용. 신규 몬스터/보스 기믹·Scene/Save 구현 자동 허가 아님.

## 로컬 Codex가 잠가야 할 미검증 항목 — 절대 값을 만들어 쓰지 말 것

1. 대표 PC `F:\Downloads\...` ZIP 해시 재확인 및 사용자 PC 실제 Unity Editor 열린 상태/로컬 worktree `git status` 보호.
2. Ch2 공식 몬스터·보스(실제 Alpha bbox 기준)를 현재 게임 UI 136×112, Boss 408×336 안에서 `preserveAspect`로 표시한 실효 화면 px 높이. 신규 512 원본의 같은 UI 렌더링 수치/위압감 비교.
3. 필드 `visualScale`과 원본 314/512 PPU의 월드 단위, collider 실제 반경 및 Sorting Layer 재검증. 신규 Scale/Pivot/PPU/Idle Cell 계약 확정.
4. 두 신규 적 각각 정식 4 Idle frames 단독 가로 Strip 여부, Import Metadata/프레임 이름 `_00..._03`/SpriteRect 순서 및 알파 접지선 정합성 확정.
5. Chapter3 신규 몬스터 구현 자체는 **하지 않고** 계약 문서/정적 최소 확인만. 기존 통합 QA, Scene, Animator, .meta/GUID, Save/UserData, 승인 PNG 변경0.
6. 기술 계약은 Codex **자기 로컬 `codex/...` worktree**에서 문서화해 기획 담당 `docs/planning`, 신규 아트 `docs/art-new`에게 전달. 이 사전조사 문서의 브랜치가 Codex 작업 결과와 같다고 오인하지 말 것.

## 다음 단계와 승인

- **현 상태**: 1포즈 사용자 승인 PASS, 이 Idle 제작 계약의 **유효 제작 규격 잠금은 PENDING**. 소형 Idle 파일럿 제작은 코덱스의 실효 표시/셀/Import 계약 이후 개시.
- `main` 직접 커밋/Push/병합·기존 Unity 수정/새 몬스터/애니메이션 제작 없음. 사용자 미술 승인/기술 QA는 별도로 관리.
