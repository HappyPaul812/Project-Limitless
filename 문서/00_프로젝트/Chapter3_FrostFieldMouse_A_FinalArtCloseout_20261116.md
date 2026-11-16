> 2026-11-16 EOD 선별 인계: 출처 `origin/docs/art-new`, Git blob `4d948e91ede582c4f3a9d63572cb78005ad42bcf`. 이하 보고의 승인/미적용/PENDING 표시는 작성 당시 상태입니다. 최신 결재는 EOD 보고 우선: Stoat KeyPose·Idle4·Attack4v2·Hit4 미술 승인/Unity미적용, NPC02/17·10/07 로컬적용, Issue85 Import/GUID HOLD. NPC 손 한정 지시는 기획 해석 오류였으며 최신 정정대로 인계합니다. 기존 제품·미완료 자산을 새로 적용하지 않습니다.

# LIMITLESS / Chapter3 신규 아트 / 서리들쥐 FrostFieldMouse A 최종 미술 마감 (2026-11-16)

**담당**: 메인 신규 아트 (`docs/art-new`)  
**판정**: **`ART_COMPLETE / USER_ART_APPROVED / ORIGINAL_ART_KEEP / 4_MOTIONS_16_FRAMES_PRESENT / NO_NEW_ART_REQUIRED`**  
**다른 상태**: `MAIN22_SOURCE_PRESENT / PRIOR_MAIN22_ISOLATED_136_PASS / CURRENT_LOCAL_RUNTIME_NOT_RETESTED / OFFICIAL_MONSTER_GAMEPLAY_NOT_FINALIZED`.

## 1. 최신 기획·검증 정본

- [Issue #30 / 2026-11-16 총괄 기획 마감 지시](https://github.com/HappyPaul812/Project-Limitless/issues/30#issuecomment-6904538244), [2026-11-16 병렬 발주](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/20261116_Parallel_Art_MouseCloseout_NPC02_17.md), [Issue #44 Main22 첫 플레이 결과](https://github.com/HappyPaul812/Project-Limitless/issues/44#issuecomment-6309212708)와 [원본 전투외형 연결/40PASS QA](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/00_프로젝트/Chapter3_FrostFieldMouse_Fix_v2_Official_Apply_QA_20261021.md) 조회.
- [전투 v2 원화 납품표](https://github.com/HappyPaul812/Project-Limitless/blob/docs/art-new/문서/00_프로젝트/Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2_ArtDelivery_QA_20261020.md)를 확인하고 이전 승인과 결과 재사용. 2026-10-21 바울 대표 미술 승인과 Attack/Hit/Defeat v2 Codex 23PASS, 기존 전투 외형 연결 실제 40PASS/0FAIL을 **재실행하지 않음**.
- Issue #44 첫 플레이 **격리 Unity 136PASS/0FAIL**, Main21→F1 북문→서리 조사→흔적2곳→서리들쥐 지정 전투1기→쉼터 완료. 사용자 당시 **"잘된다"** 피드백은 **격리 첫 플레이 흐름에 대한 긍정 확인**이지 신규 정식 EXP/드롭/몬스터밸런스/모든 사용자 기기 검증 PASS가 아님.

## 2. 최종 승인 PNG 원본 4개 (각 4셀×512² = 2048×512 RGBA)

**실제 원격 `main` 트리에서 4개 PNG 및 .meta 총8개 경로 존재 확인**, 네 Sprite Importer .meta는 명시적 4셀/PPU512/Point/셀 Pivot `(0.5,0)`로 구성. 원격 `MonsterVisuals/FrostFieldMouse.asset`에도 Idle/Attack/Hit/Defeat 각각 배열 4개씩 **16개 실제 Sprite GUID+fileID**를 확인. 게임용 이미지를 재출력하지 않음.

| 모션 | `Unity/Client/Assets/_Project/Art/Monsters/FrostFieldMouse/`의 파일 | **기존 승인 PNG SHA256** | 판정 |
|---|---|---|---|
| Idle 00–03 | `FrostFieldMouse_A_Idle_4Frames.png` | `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2` | KEEP · USER_APPROVED |
| Attack 00–03 | `FrostFieldMouse_A_Attack_4Frames.png` | `c83e35b5aebbce908b6025d5a071fafc463cfe301763f68806ecddb8bd4f576b` | KEEP · USER_APPROVED |
| Hit 00–03 | `FrostFieldMouse_A_Hit_4Frames.png` | `695ed7db1f794c3f71df1b058eb5e91f01de7ecb125369264f3624b36c9fd3c2` | KEEP · USER_APPROVED |
| Defeat 00–03 | `FrostFieldMouse_A_Defeat_4Frames.png` | `3f6a52039b766c57a5367c7145e0234b68ac0a552940de1e88ffe8f2404f3395` | KEEP · USER_APPROVED |

- 기존 1포즈 `FrostFieldMouse_A_PixelArt_1Pose` 독립 미술 기준, 이전 승인 SHA256 **`0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`**(전투 아트 납품문서의 기획 정본값). 현재 `main` 제품은 **1포즈 별도 Sprite가 아니라 승인 Idle00**을 필드 표시로 재사용하므로 독립 1포즈가 현 게임 폴더에 없다고 해서 새 필드 PNG가 필요하다고 단정하지 않는다.
- **SHA 정확성 범위**: 위 SHA256은 10/20~21 납품·Codex 원본 적용 검수와 11/16 기획 정본의 **기존 실제 검사 수치**임. 이번 세션 GitHub 연결에서 4개 PNG 경로와 Git blob/metadata/YAML 참조는 다시 읽었으나, API의 바이너리 본문 제공이 제한되어 **4개 파일을 내려받아 SHA256을 새로 독립 계산하지는 못함**. **신규 독립 `SHA256_REHASH_PASS`라고 표시하지 않는다.** Git blob SHA-1과 위 PNG SHA256은 서로 다른 검증 항목.

**기존 규격 계약**: 가로 4셀 512×512, PPU512, 실 Sprite Pivot `(0.5,0)`, Point/Clamp/MipmapOFF/FullRect, VisualScale1, 우향, 공통 8FPS. Attack00/Hit03은 Idle00과 픽셀 동일 경계 자세, Attack02가 주 할퀴기, Attack03은 승인 Idle03 복귀, Defeat03 최종 홀드라는 이전 합격 구조를 그대로 보호.

## 3. 필드 Idle00과 Main22 실제 사용 범위 (정적 원격 파일 + 과거 실제 QA 분리)

- [`MonsterVisuals/FrostFieldMouse.asset`](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Resources/MonsterVisuals/FrostFieldMouse.asset) **원격 `main` 실내용 확인**: `fieldSprite`는 `Idle_00` fileID `-1481457045` / GUID `6dd66eaa0fa89ad4387b49463367ff15`. explicitIdle/Attack/Hit/Defeat는 각각 Sprite 4개; `explicitWalkFrames: []`·`fieldAnimatorController: 0`. 필드용 별도 4방향 Walk 애니메이션을 요구하는 정본 근거 없음.
- [`StoryEncounterReturns/Main22_Encounter.asset`](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main22_Encounter.asset): `sceneName: World_NorthF1`, `spawnId: northf1_main22_mouse`, position (0,4), 연결된 `Monster` 데이터는 임시 프로토타입 리소스. [`Main22_MousePrototype.asset`](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Resources/StoryEncounterReturns/Main22_MousePrototype.asset)에는 **`monsterId: prototype_main22_frost_field_mouse`**, 필드 `Idle00` 및 전투4모션 명시적 배열이 존재.
- [`Chapter3Main22Flow.cs`](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Scripts/World/Chapter3Main22Flow.cs)에는 F1 (0,4) 지정전투 상호작용, `ApprovedFrostFieldMouse` 필드 Renderer 및 `MonsterSpriteSheetAnimation`, `StoryEncounterReturns/Main22_Encounter` 로드가 존재. `QuestDefinitions/Main22_NorthwindSigns.asset`에는 서리들쥐 1기 승리가 4번째 목표로 연결.
- **실제로 과거 격리 Unity에서 136PASS, 대표 "잘된다" 반응** 확인. `main`의 정적 소스와 실제 과거 Play Mode 검증은 별개. **11/16 현재 사용자의 로컬 원본 Unity 실제 Main22 실행/필드 표시를 재기동해 확인하지는 않음**. 제품 파일이 `main`에 존재해도 지금 PC의 원본 상태, Build, 개인 Save까지 동일하다는 의미는 아님.

## 4. 완료/후속 구분 — Codex·기획 담당 인계

**미술 팀 마감 `ART_COMPLETE`**:
- 승인 1포즈, Idle4/Attack4/Hit4/Defeat4 **총16프레임 확보**, 게임 Import 원본, 필드 Idle00 모두 충족.
- 추가 그림0 / 새 파일럿0 / 4방향 Walk·Field Run·Hit/Defeat 추가 프레임·미술 재승인 요구0. 과거 완료 QA 중복실행0.

**Codex·기획 후속 `GAMEPLAY_NOT_FINALIZED`**:
1. **정식 `MonsterDefinitions` 항목/고유 `monsterId` 결정**: `MonsterVisuals/FrostFieldMouse.asset`에는 `monsterId:` **빈값**, Main22는 `prototype_main22_frost_field_mouse`를 사용. 실제 독립 몬스터 공식 ID·Registry·상시 필드 Spawn 확정은 미완료. 기존 Main22 지정조우용 `spawnId` 존재와 일반 몬스터 공식 Spawn 완료를 혼동하지 말 것.
2. **전투 수치·보상**: Main22 임시 레벨1/HP60/기본공격100/Agility10은 QA/프로토타입용 상속값, `baseExperience: 0`, `currencyReward: 0`, `lootEntries: []`; `Main22` Quest reward도 EXP0/Currency0/Items[]. EXP·Drop·최종 전투 밸런스는 기획 승인 후 Codex가 적용.
3. **최종 실제 플레이/제품QA**: Main22 사용은 과거 격리 첫 플레이·원격 정적 코드로 확인. 11/16 로컬 원본 최신 Scene/Save·빌드·실제 플레이 상태는 직접 검증하지 않았음. 현재 실제 게임에 반영된 Main22 흐름의 사용자 플레이, 반복/세이브, 몬스터 스폰/리워드 확인은 별도 Codex 범위.
4. **기획 승인 없는 기정사항 없음**: 피해량/드롭아이템/정식 스킬·얼음 상태/서리 보스/보상·난이도 신규 정의 금지. 사용자 본편 반영·원본 Scene·Resources 변경은 별도 승인 게이트.

**보호**: Frost Owl Idle8/Attack4·F1/F2 승인 환경·FrostTyrant·냉기VFX/아이콘·승인FrostFieldMouse 원본16프레임·Discord 자동화·Unity 6000.6.5f1/Scene/Prefab/BuildSettings/Save/UserData·main 변경0. 원격 `main` 직접 commit·Push·merge0, 기존 대형 QA/격리 worktree 복제0.

## 5. 이번 산출물 및 보고 상태

- 본 **최종 Art Closeout 문서 1개**와 담당 `SPRITE_ART_LATEST.md` 인계·Issue #30 댓글만 작성. PNG/ZIP/프리뷰 재생산0, 새로운 Unity/제품 QA0.
- **기존 실제 기술 근거**: [`Chapter3_FrostFieldMouse_Fix_v2_Official_Apply_QA_20261021.md`](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/00_프로젝트/Chapter3_FrostFieldMouse_Fix_v2_Official_Apply_QA_20261021.md) **40PASS**, [Main22 첫 플레이 실제 격리 136PASS 문서](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/00_프로젝트/Chapter3_Main22_FirstPlayable_Implementation_QA_20261021.md), [Main22 1280×720 실제 Battle GameView](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/00_프로젝트/Evidence/Main22FirstPlayable20261021/BattleView/Main22_SingleMouse_ActualGameView.png), [기존 Idle 비교판](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/00_프로젝트/Evidence/Chapter3IdleV1_20261020/Delivery/Preview/FrostFieldMouse_A_4Frame_ContactSheet.png). 이미 있는 근거를 재사용.
- **최종:** `MOUSE_ART_COMPLETE / ORIGINAL_MOTION_PNG_KEEP / OFFICIAL_MONSTER_ID_STATS_DROPS_PENDING_PLANNING / MAIN22_PRODUCT_RUNTIME_LATEST_NOT_RETESTED / NO_NEW_IMAGES`.

**바울 대표 추가 미술 승인 필요:** **없음**. **Codex 후속 필요:** **공식 ID·스폰·EXP/전리품/밸런스 기획 승인 이후 별도 구현**, 최신 원본 제품 실제 플레이 확인은 별도. 
