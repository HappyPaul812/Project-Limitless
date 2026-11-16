> 2026-11-16 EOD 선별 인계: 출처 `codex/issue85-psd1501-guid-plan-20261112`, Git blob `cd3a7cf3206bc19912a0e2fb11cf42bb8dc360d8`. 이하 보고의 승인/미적용/PENDING 표시는 작성 당시 상태입니다. 최신 결재는 EOD 보고 우선: Stoat KeyPose·Idle4·Attack4v2·Hit4 미술 승인/Unity미적용, NPC02/17·10/07 로컬적용, Issue85 Import/GUID HOLD. NPC 손 한정 지시는 기획 해석 오류였으며 최신 정정대로 인계합니다. 기존 제품·미완료 자산을 새로 적용하지 않습니다.

# Issue #85 기술 복구 2차 — PSD 컴파일 PASS / Import 예외 HOLD / GUID 계획

**ISOLATED_CSHARP_COMPILE_PASS / IMPORT_TOOLING_EXCEPTION_HOLD / GUID_PLAN_ONLY / NOT_ORIGINAL_APPLIED / REMOTE_NOT_PUSHED**

[기획 최신 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/85#issuecomment-6815984636)과 사용자 2차 지시를 확인했습니다. 이번 범위는 패키지 격리 시험과 GUID 수정 계획 작성이며, GUID 실제 수정·원본 적용·Push는 수행하지 않습니다.

## 실제 Unity 결과

- 새 `codex/issue85-psd1501-guid-plan-20261112` worktree에서 **Unity6000.6.5f1 (3ff58d469c8a)** Batch Import·전체 C# 컴파일 1회, 종료코드 **0**, 고유 compiler error **0건**입니다. 기존 Animation SetChecked/IsChecked CS1061 6곳과 PSD Toggle.IsHovered CS1061 1곳 모두 이번 로그에서 사라졌습니다.
- **Import 전체 무오류는 아닙니다.** 기존 2D Tooling3.0.1의 USS/UXML을 찾지 못하는 **DirectoryNotFoundException 8개 경로, 총12회**가 새로 관찰됐습니다. 사용자 지시에 따라 추가 패키지 변경·재시험을 중단하고 **Import/배포 HOLD**로 보고합니다. C# PASS를 Console 전체오류0 또는 Release Ready로 확대하지 않습니다.
- 실패 위치는 `Library/PackageCache/com.unity.2d.tooling@6f06ffb033c0/Editor/Insider/SpriteAtlas/SpriteAtlasIssueReport/` 아래입니다. 실제 전체 경로가263~274자이며 Python 읽기 전용 검사에서도 해당8파일이 없습니다. 긴 경로 또는 패키지 추출 문제 가능성은 있으나 **원인 미확정**입니다. 2D Tooling 변경, 경로 이동, 재설치, 재시험은 하지 않았습니다.
- 투사체6개 GUID 충돌 로그도 기존대로 발생했습니다. 계획한 meta6과 PNG6의 디스크 SHA는 Import 전후 동일하며 이번 GUID 수정은 없습니다. Resources 실행/게임 애니메이션 검증은 이번 범위 밖으로 미실행입니다.
- 시험 중 자동 변경된 추적 파일 **236개**와 자동 생성 meta **6개**는 private Temp에 SHA와 함께 보존하고, 새 시험 worktree에서만 시험 전 상태로 복원·분리했습니다. ThirdParty·Settings·BuildSettings의 자동 변환을 제품으로 채택하지 않았습니다. 패키지2파일 외 제품 diff는 없습니다.

### 오류 원문과 구분

이번 C# 컴파일 오류 원문: **없음(0건)**. 1차에서 해결 대상으로 확인한 원문은 다음과 같습니다.

```text
Library\PackageCache\com.unity.2d.psdimporter@5d82044f9e43\Editor\UTK\ImportColumn.cs(71,39): error CS1061: 'Toggle'에는 'IsHovered'에 대한 정의가 포함되어 있지 않고, 'Toggle' 형식의 첫 번째 인수를 허용하는 액세스 가능한 확장 메서드 'IsHovered'이(가) 없습니다. using 지시문 또는 어셈블리 참조가 있는지 확인하세요.
```

이번 Import 예외 원문 예시(동일 접두 경로의 반복을 줄였으며 전체 원문은 UnityImport.log에 보존):

```text
DirectoryNotFoundException: Could not find a part of the path "F:\study\codex\Project-Limitless\Temp\Issue85Psd1501GuidPlan20261112\Worktree\Unity\Client\Library\PackageCache\com.unity.2d.tooling@6f06ffb033c0\Editor\Insider\SpriteAtlas\SpriteAtlasIssueReport\UIElement\CommonSpriteAtlasIssueView\CommonSpriteAtlasIssueView.uss"
```

| SpriteAtlasIssueReport 하위 누락 파일 | 전체 경로 문자 수 |
|---|---|
| `SourceTextureWithCompressionIssue\SourceTextureWithCompressionIssue.uss` | 267 |
| `SourceTextureWithCompressionIssue\SourceTextureWithCompressionIssue.uxml` | 268 |
| `SpriteAtlasTextureSpaceUsedIssue\SpriteAtlasTextureSpaceUsedIssueSettings.uss` | 273 |
| `SpriteAtlasTextureSpaceUsedIssue\SpriteAtlasTextureSpaceUsedIssueSettings.uxml` | 274 |
| `SpriteWithSecondaryTextureIssue\SpriteWithSecondaryTextureIssue.uss` | 263 |
| `SpriteWithSecondaryTextureIssue\SpriteWithSecondaryTextureIssue.uxml` | 264 |
| `UIElement\CommonSpriteAtlasIssueView\CommonSpriteAtlasIssueView.uss` | 263 |
| `UIElement\CommonSpriteAtlasIssueView\CommonSpriteAtlasIssueView.uxml` | 264 |

## 패키지 실제 해석·변경 목록·SHA

기존 컴파일 오류 때문에 제품에서 API 실행이 막히므로 새 빈 bootstrap의 UPM `Client.AddAndRemove`로 기존 직접 의존성53개를 요청했습니다. 변경 요청은 **Animation16.0.1 유지 + PSD14.0.3→15.0.1**뿐이며, 빈 bootstrap 기본 tetgen/timelinefoundation2개는 API 제거 요청으로 제외했습니다. API 성공 후 요청53개와 manifest 전체가 동일함을 확인했습니다.

UPM 생성 manifest/lock을 실제 Clean worktree에 복사한 뒤 실제 프로젝트에서 다시 UPM 해석·Import·컴파일했습니다. 파일 SHA는 실제 Import 전후 동일합니다. **직전 Animation16 시험 대비 lock 변경은 PSD1항목뿐**이며, PSD의 common 의존성14.0.1→15.0.0은 이미 해석된 common15.0.0과 일치합니다. 예상하지 못한 추가 패키지0, PackageCache 직접 수정0, lock 수작업 편집0입니다.

원래 Clean lock 대비 **19항목 = 기존 baseline17 + 승인 Animation1 + 승인 PSD1**입니다. 기존17개는 같은 Unity에서 이미 관찰된 lock 정합화이며 이번 새 오류로 분류하지 않습니다. [Unity 공식6000.6 PSD 안내](https://docs.unity.com/en-us/engine/6000.6/manual/packages-list/packages-all/pack-safe/com-unity-2d-psdimporter)의15.0.1 및 실제 package.json의 unity6000.6/의존성을 확인했습니다.

| lock 항목 | 원래 Clean | 실제 해석 | 구분 |
|---|---|---|---|
| `com.coplaydev.unity-mcp` | 없음 | https://github.com/CoplayDev/unity-mcp.git?path=/MCPForUnity#v10.2.0 | 기존 baseline |
| `com.unity.2d.animation` | 15.1.0 | 16.0.1 | 승인 Animation |
| `com.unity.2d.common` | 14.0.1 | 15.0.0 | 기존 baseline |
| `com.unity.2d.psdimporter` | 14.0.3 | 15.0.1 | 승인 PSD |
| `com.unity.burst` | 1.8.30 | 2.0.0 | 기존 baseline |
| `com.unity.collections` | 6.5.0 | 6.6.0 | 기존 baseline |
| `com.unity.graph-authoring` | 없음 | 1.0.0 | 기존 baseline |
| `com.unity.multiplayer.center` | 1.0.1 | 2.0.1 | 기존 baseline |
| `com.unity.nuget.newtonsoft-json` | 없음 | 3.2.2 | 기존 baseline |
| `com.unity.profiling.core` | 없음 | 1.0.3 | 기존 baseline |
| `com.unity.render-pipelines.core` | 17.5.0 | 17.6.0 | 기존 baseline |
| `com.unity.render-pipelines.universal` | 17.5.0 | 17.6.0 | 기존 baseline |
| `com.unity.render-pipelines.universal-config` | 17.5.0 | 17.6.0 | 기존 baseline |
| `com.unity.searcher` | 4.9.4 | 4.9.5 | 기존 baseline |
| `com.unity.shadergraph` | 17.5.0 | 17.6.0 | 기존 baseline |
| `com.unity.test-framework` | 1.7.0 | 1.8.0 | 기존 baseline |
| `com.unity.test-framework.performance` | 3.5.0 | 6.6.0 | 기존 baseline |
| `com.unity.timeline` | 1.8.12 | 6.6.0 | 기존 baseline |
| `com.unity.ugui` | 2.5.0 | 2.6.0 | 기존 baseline |

| Unity/Client/Packages 시험 파일 | SHA256 |
|---|---|
| `manifest.json` | `1ef917c530fb58a8349ec593f195f8e39e67b29995d9ebab360c9bbcb99129e4` |
| `packages-lock.json` | `9392f919014bd6d27461549977f32f4ca28d8041db2113a0de8c1001c44ccf16` |

이2파일만 보존한 **격리 시험 커밋은 `87665b3444d4ca9aee7f13afc220ae7ff9085e66`**입니다. 컴파일 성공을 재현하기 위한 로컬 기록이며 본편 적용이나 배포 승인이 아닙니다. 1차 보고 e2718597에서 시작했고 원래 Clean 후보는 변경하지 않았습니다.

## GUID 정본 추천과 수정5개 계획 — 미적용

기존GUID **ea7264dba5af46cda070ad3e3aa3eac3**를 유지할 정본은 **GoldenArrow/golden_arrow_00.png.meta**로 추천합니다.

1. `2359e1e174518f8d9d2466f8274e48ba6832b969`(2026-08-24 14:44 KST)에서 GoldenArrow_00이 먼저 해당 GUID를 사용했습니다.
2. 후속 `9a59e14c7df5502671d7a60d8ea59ce7ec8e2587`(같은 날15:40 KST)에서 Healer00~04가 추가됐고, 5meta 모두 같은 GUID/내용입니다. 앞 커밋은 뒤 커밋의 조상이며 GoldenArrow의 GUID는 이후 원격 baseline까지 동일합니다.
3. 추적 Assets 전체에서 해당 GUID 직접 직렬화 참조는0입니다. 실제 사용은 BattleSceneController의 경로 기반 `Resources.LoadAll<Sprite>` 후 이름순 로딩입니다. 따라서 **최초 소유자1개 유지 + 후속5개만 수정**이 최소 범위입니다. 이는 Git 이력에 근거한 추천이며 원저작자의 의도를 확정하지 않습니다.

아래 GUID는 **계획 문서에만 생성한 제안값**입니다. 기존 모든 meta와 대조해 충돌이 없고 서로 다름을 확인했습니다. 승인 후 작업 직전에 다시 충돌을 검사합니다.

| BattleProjectiles 하위 meta | 유지/제안 GUID |
|---|---|
| `GoldenArrow/golden_arrow_00.png.meta` | `ea7264dba5af46cda070ad3e3aa3eac3` |
| `HealerMagicalProjectile/healer_magical_projectile_00.png.meta` | `7d33c47cb2e8594499a7da1f92704b70` |
| `HealerMagicalProjectile/healer_magical_projectile_01.png.meta` | `6cd6786a5be45800a0dd582cccefc690` |
| `HealerMagicalProjectile/healer_magical_projectile_02.png.meta` | `e8a01d90a7945994866550d7635e606c` |
| `HealerMagicalProjectile/healer_magical_projectile_03.png.meta` | `d0e238ebf8b556e78a2c229e8f95ce59` |
| `HealerMagicalProjectile/healer_magical_projectile_04.png.meta` | `39a8fe57b22c5578bd1ffbaea5314f82` |

수정 승인 대상은 **HealerMagicalProjectile/healer_magical_projectile_00.png.meta~04.png.meta 정확히5개**, 각각 최상위 `guid:` 한 줄입니다. PNG, 폴더meta, GoldenArrow, meta의 다른 필드, Scene/Prefab/AnimationClip, Resources 경로와 스크립트는 변경하지 않는 계획입니다. SpriteMode1, PPU32, Pivot(0.5,0.5), 기존 spriteID/localFileID·필터·압축·임포터 설정과 원본 픽셀을 보존합니다. SpriteID는 각 Importer 내 기존 값이므로 최상위 GUID 수정과 섞어 변경하지 않습니다.

| 보호 PNG | SHA256 |
|---|---|
| `GoldenArrow/golden_arrow_00.png` | `31f7786ea2500aab71e71b5937aace01dd029b2245826b78cb22f34242bb52a3` |
| `HealerMagicalProjectile/healer_magical_projectile_00.png` | `8ceeab4c7d1a1576209b115914406d93a8cafa3cd78016addb350d92c1574fe8` |
| `HealerMagicalProjectile/healer_magical_projectile_01.png` | `8c9666fe94de2661bdad2d299354a9d56c04de10c1154b404936e30d48f7c868` |
| `HealerMagicalProjectile/healer_magical_projectile_02.png` | `645ae06ba0647cbd87cb47a66ef8bd5049179a081a6e813ea175e305ff3fc4a3` |
| `HealerMagicalProjectile/healer_magical_projectile_03.png` | `e2598e6ec872bfc014abede9ae62356d668e8b59459e09853218455a4ca3dc82` |
| `HealerMagicalProjectile/healer_magical_projectile_04.png` | `6180ae92cb6652de89ed719b01354cf85a7ddd52c056fd144229d9b63b80908a` |

## 승인 후 최소 검증 계획

1. 별도 격리 branch에서6meta/PNG SHA와 참조를 다시 확인하고 백업합니다. 5개 `guid:` 치환 외 diff가 없음을 검사합니다. 사용자 수정과 충돌하면 HOLD합니다.
2. Unity6000.6.5f1의 새 Library에서 GUID 충돌0, C# 오류0, meta 디스크값과 AssetDatabase의 경로↔GUID 왕복 일치를 확인합니다. 자동 재할당에 의존하지 않습니다.
3. Resources.LoadAll<Sprite>로 **GoldenArrow 전체8프레임/Healer5프레임**의 개수·null0·이름순·각 Sprite.texture의 실제 원본 경로·PPU·Pivot·localFileID를 대조합니다. 충돌 GoldenArrow1개 외 기존7프레임도 보호합니다.
4. 기존 공용 Presenter의 두 투사체 프레임 순서와 애니메이션 참조만 표적 검증하고 PNG 불변/MissingSprite0을 확인합니다. 기존 전투 전체QA를 반복하지 않습니다. **이번에는 이 수정·Resources 실행·애니메이션 재QA를 하지 않았습니다.**
5. 격리 결과 보고 후 원본 적용과 원격 Push는 각각 별도 승인으로 결정합니다. local main 이력 정렬도 새 백업과 별도 승인 대상입니다.

## 원본 보호·완료 경계

원본 **8,322파일** 시작 SHA와 기존 PrivateBackup SHA를 대조했고, 종료 원본 SHA 변경0/원본 git status 바이트 동일(tracked302/untracked550)을 확인했습니다. Save/UserData/UserSettings·사용자 이미지·음성은 보존됐고 개인 Save를 검증 프로젝트에 복사하지 않았습니다. 승인 게임235파일 누락/SHA 불일치0이며 최종 Git blob도 commit 후 확인합니다. GUID6meta/PNG6 변경0입니다.

기존 local main **d360813667d2ee47515067b8dd07c1a599efec15**, 원래 Clean **61fdc6902a6d0c4d109c36b308a3b8889e7aaac7**, 1차 보고 **e2718597a28f82adf1d1837cd31a8f7f53ca58fc**의 ref와 worktree 상태는 그대로입니다. 원격 main 서버 조회는 **bfd281545632877325f5d21e04cefb924042c465**입니다. Push/merge/rebase/reset/자동 이력 정렬은 없습니다.

실행 전 기존 Unity.exe는 없었으며 새 bootstrap/새 worktree의 숨김 batch만 실행·종료했습니다. 기존 Editor/Scene/Play/선택/OS 포커스/ComputerUse 조작0. 기존 QA 전체 재실행0, PlayMode0입니다. 로그·검사기·개인 보호목록·자동 Import 변환본은 private Temp에만 보존하며 제품 커밋에서 제외합니다.

다음 승인 필요 항목:

- 새 Import 예외를 조사할 후속 범위. 먼저 읽기 전용 경로·동일 패키지 파일 존재 확인과 짧은 격리 경로 재시험 여부를 결정해야 합니다. 원인 미확정 상태에서 2D Tooling 버전 변경을 제안값으로 확정하지 않습니다.
- GoldenArrow_00 정본 유지와 Healer5meta의 GUID 한 줄만 바꾸는 위 **격리 수정·표적 검증 계획 승인**. 원본 GUID 수정은 별도입니다.
- 본편 패키지 적용·원격 Push·local main 정렬은 모두 이번 범위 밖입니다. 성공한 컴파일만 근거로 자동 진행하지 않습니다. Owl USER_REALPLAY_PENDING 유지.

담당 브랜치 `codex/issue85-psd1501-guid-plan-20261112`, 패키지 SHA는 위 commit, 최종 보고 commit은 Issue85 댓글/branch HEAD에 기록합니다. CODEX_LATEST/CURRENT_STATUS를 같은 branch에서 갱신합니다. GitHub에는 Issue 댓글만 실제 게시하며 코드/보고 문서는 로컬 commit만 존재합니다. 최종 보호와 파일/로그 SHA receipt는 private `Temp/Issue85Psd1501GuidPlan20261112/Final.json`에 보존합니다.
