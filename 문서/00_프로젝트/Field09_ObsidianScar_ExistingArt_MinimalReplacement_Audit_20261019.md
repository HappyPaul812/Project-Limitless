# LIMITLESS / Chapter2 Field09 ObsidianScar 기존 환경 아트 및 최소 교체 필요성 감사

**감사 날짜:** 2026-10-19  
**GitHub 정본:** `main` `675988c9f2c5c507d9a696b0378be27c0bd3309b`  
**담당:** Chapter 1·2 리뉴얼 아트 / **기록 브랜치:** `docs/art-renewal`  
**상태:** `FIELD08_EXISTING_ART_PROTECTED_CLOSED / FIELD09_SOURCE_AUDIT_DONE / CONFIRMED_MUST_REPLACE_ENV_PNG_0 / VISUAL_DEFECT_REVIEW_REQUIRED / NO_NEW_ART_AUTHORIZED / UNITY_UNCHANGED`.

## 0. 최신 사용자 결정

- 바울 대표는 **Field08 기존 공식 Main17 환경 아트 유지·추가 PNG/ZIP 생략 결정**을 승인했습니다. Field08 신규 환경 제작 작업은 `CLOSED_OFFICIAL_ART_PROTECTED`. Field07 승인 아트 실제 Unity 적용 뒤 동서 연결에서 문제 *실증 시* 최소 보완만 별도 제안. 기존 감사 기록 [Field08 ExistingOfficialArt](Field08_RedRift_ExistingOfficialArt_Audit_20261019.md) 보존.
- 다음 대상 Field09는 **읽기 전용 감사 및 필수 교체 필요 아트 목록 작성까지만 허가**됨. 기존 Main18 아트 PNG를 재제작, 리컬러, Crop, Size/Alpha 변경하거나 Scene/Meta/GUID/Quest/Save/과열 값을 변경하지 않습니다. 신규 PNG 제작은 별도의 **사용자 보완 범위 승인 이후**입니다.

## 1. 확인 자료와 본체 정적 구조

읽은 정본:
- `Unity/Client/Assets/_Project/Scenes/Field_09_ObsidianScar.unity` (scene Git blob SHA **`f02ef9f2eccba4a32cc9afb7e819a3f9536816fc`**, 47,839문자).
- `Unity/Client/Assets/_Project/Resources/Main18/Environment/`의 **Main18 오리지널 PNG 10개 및 Unity .meta**, `문서/00_프로젝트/Main18_Art_Import_QA.md`, `Main18_Art_Source_Audit.json`, `Main18_Importer_Settings.txt`, `Main18_Final_QA.md`, `Main18_QA_Summary.json`.
- `Unity/Client/Assets/_Project/Scripts/World/Chapter2Main18Flow.cs`, Field08↔Field09↔Field10 전환 정의 및 Main18 과열/몬스터 인계.
- `문서/00_프로젝트/QA_증거/Main18/Field09_{1280,1600,1920}.png`의 **기존 기록과 경로는 확인**, 그러나 GitHub 바이너리 이미지의 픽셀을 이번 세션에서 직접 열거나 실제 새 Runtime 캡처를 촬영하지는 **않았음**. 따라서 Guide 모양의 실제 사용자 화면 노출 여부는 이전 Codex QA 기록에 근거한 **검토 필요**이지 본 세션의 직접 시각적 PASS/FAIL이 아닙니다.

실제 Scene의 직렬화 구성: **GameObject18**, **SpriteRenderer9(모두 GameObject 활성/Renderer.enabled1)**, **MeshRenderer1**(`ObsidianScarTitle`, Unity 기본 Font용으로 환경 PNG 대체 대상 아님), **BoxCollider2D6**(기존 세계 경계·흑요석 바위), 다른 Main18 필드 조사·Story NPC/몬스터는 `Chapter2Main18Flow`가 **Runtime 생성**하므로 Scene 9개 SpriteRenderer에 합산하지 않습니다.

### 기존 9개 Renderer 실제 대응표 (main Scene YAML + PNG .meta GUID 대조)

| Scene GameObject | 좌표 (x,y) | Scale | Sorting | 실제 현재 Source PNG / 적용 분류 |
|---|---|---|---:|---|
| `ObsidianScar_Ground` | (0,0) | (1,1) | -20 | `ObsidianScar_Ground_Base.png`; SpriteRenderer **Tiled, m_Size=(21,15)** |
| `ObsidianScar_GroundCrackedWest` | (-6,0) | (1,1) | -19 | `ObsidianScar_Ground_Cracked_01.png`; **불투명 Tiled (9,15)**의 서쪽 지면 Overlay |
| `ObsidianScar_Slab` | (5.2,1) | (.5,.5) | -5 | `ObsidianScar_Obsidian_Slab_01.png` |
| `ObsidianScar_HeatVent` | (3,-2) | (.45,.45) | -6 | `ObsidianScar_HeatVent_01.png` |
| `ObsidianScar_Ash` | (0,-1.2) | (.525,.525) | -5 | `ObsidianScar_AshPatch_01.png` |
| `ObsidianScar_Fissure` | (-5,3.8) | (.525,.625) | -8 | `ObsidianScar_Fissure_Edge.png` |
| `ObsidianScar_Glow` | (-5,3.8) | (.525,.625) | -7 | `ObsidianScar_Fissure_Glow.png` |
| `ObsidianScar_Cliff` | (-9,0) | (.65,.65) | 2 | `ObsidianScar_Cliff_Blocker_01.png` |
| `ObsidianScar_Spire` | (-8.8,4.8) | (.625,.625) | 2 | `ObsidianScar_Obsidian_Spire_01.png` (기존 BoxCollider) |

`ObsidianScar_Ground_Cracked_02.png`는 GitHub 등록·보관되어 있지만 **위 Scene 9개 Renderer의 실제 Source 참조 없음**. 이를 임시 쓰레기라고 판단하거나 임의 사용·제거하지 않습니다. Scene 9개 Sprite 참조는 전부 Main18 환경 전용 PNG.meta의 실제 Single Sprite **`fileID21300000` + GUID**와 일치했습니다. 기존 승인 등록 자산으로 보호합니다. 현재 프로젝트 `_Project/Art/Environment/Chapter2`에 **Field09 별도 폴더가 없는 것은 자산 누락이 아니라 공식 자산 위치가 `Resources/Main18/Environment`인 것**입니다.

Importer: 위 10개 모두 512×512 RGBA, `Sprite`, PPU128 / Center (0.5,0.5) / Point / Mipmap OFF. 원본들은 Main18 납품 ZIP에서 **Byte-identical Import17/17**이었다는 기존 `Main18_Art_Import_QA`/Source Audit 기술 PASS_WITH_NOTE 보고가 있습니다. 이는 **최종 미술 승인 17/17을 뜻하지 않습니다**. 원본 ZIP SHA `4a3dca636251e4f6a12ce9f007b27be5babb36b98c0b5c9b88bf4a73aaa5de2b`도 역사 QA의 보고값이며 이 세션에서 로컬 ZIP 직접 검증을 한 것이 아닙니다.

## 2. Main18 과열/진행/연결 보호

- 퀘스트: `main_18_black_heat`, **13 목표**. 아르벨 레온 보고·냉각약3→Field08 심부→Field09 진입·지면(5.2,1)·열 분출구(3,-2)→갑충 튜토리얼(1.2,-.6)→과열 확인(0,-1.2)→진동(-2.2,2)→감시자 목격/전투(-4.4,.5)→후속 조사(-5.2,-1.2)→심부 경로(-7.4,0). 세린 Runtime Actor(4.4,1.2), 퀘스트/내비게이션/유지/저장·전투 연출 기존 값 보호.
- 과열 Runtime 0~2 스택, **3번째 중첩 시 ceil(MaxHP×8%), 최소1 데미지 후 과열0**, KO 가능; 냉각약은 생존 아군 하나의 과열 스택만 모두 제거. 전투 종료/KO/새 전투에서 초기화. 과열 UI, `Overheat_Apply/High/Burst` VFX, 전투 수치·무적/정화 계약은 이번 **환경 아트 감사 범위 밖**, 수정0.
- `Field08_WestToField09`: Field08 서쪽 Spawn(-7.2,0), Exit(-10.25,0) ↔ `Field09_EastToField08`: Field09 동쪽 Spawn(7.2,0), Exit(10.25,0). `Field09_WestToField10` 정의는 존재하지만 **Main18 단계의 Field09 서쪽은 Boundary 폐쇄·`Main18ClosedDeepRoute` 안내**로 검증됨. 향후 Quest에 따른 조건부 지역 연계를 임의 삭제/개방/신설 금지.
- Main18 통합 QA의 기존 **고유 Runtime500 PASS / FAIL0**, 13목표·7개 Continue 지점·사각 UI/과열·기능·4:3/16:9/21:9 카메라 검사 PASS는 **기존 Codex 결과 재사용**. 이번 리뉴얼 담당이 Unity 새 실행·검증한 사실 없음.

## 3. 공식 PNG / 임시 에셋 구분 및 반드시 교체 필요 여부

- **확인 결과**: Scene에서 켜져 있는 환경 SpriteRenderer **9/9 모두 Main18 공식 등록 PNG**이고 임시 Ground/Grass/무료 공용 Sprite가 그대로 활성인 증거 없음. `PASS_WITH_NOTE17`, `FAIL_BLOCKING0`, `USER_ART_REVIEW_REQUIRED17`은 **기술 적용 성공 + 미술 승인 미확정**을 의미합니다.
- 즉시 반드시 **새 PNG로 교체**해야 한다는 기술적·사용자 실물 미술 판정은 **현재 0건**. 단, Main18 Art QA가 기존 **원본 흰색 가이드 형태 테두리/점선과 반복 지면 경계가 보이는 문제를 실제 기록**했기 때문에, 다음 조건부 최소 후보는 삭제하지 않고 보존합니다.

| 우선 | 이미 존재하는 공식 PNG | 인계된 문제 및 확인 게이트 | 최종 신규 PNG 여부 |
|---|---|---|---|
| **P1, 부분 결함 의심** | `ObsidianScar_Ground_Base.png` + `ObsidianScar_Ground_Cracked_01.png` | Scene **21×15 Tiled 위 9×15 불투명 CrackedWest Overlay**. 기존 QA의 반복 지면 타일 경계/경계선 보고와 연결될 수 있음. **동쪽 Field08 진입 화면 및 x≈-1.5에서의 동서 전환**을 기존 1280/1600/1920 캡처·실제 Apply 상태와 비교하고, seam이 확실하면 *먼저 기존 SpriteRenderer Tiled/Tint·배치만으로 해결 가능한지* 진단. 임의 Cracked02 추가 불가 | **미확정. 실제 seam 실패 + Import/Material 조정 불가일 때만 승인 발주** |
| **P1, 백색 가이드 흔적 QA** | `ObsidianScar_Cliff_Blocker_01.png`, `ObsidianScar_Fissure_Edge.png`, `ObsidianScar_Fissure_Glow.png`, `ObsidianScar_HeatVent_01.png`, `ObsidianScar_Obsidian_Slab_01.png`, `ObsidianScar_Obsidian_Spire_01.png` | 기존 `Main18_Art_Import_QA.md`·Source Audit의 **흰 가이드 형태 선/테두리** 기록이 이 6종에 있음. 각 품목이 실제 Scene에서 사용 중이므로 **해당 장면의 가이드선이 게임 화면에서 보이는지** 6역할 중심 국소 확대 검증. 선이 실제 리듬/문양이 아닌 잘못된 아트 흔적이라고 확정된 *개별 파일만* 최소 교체/보완 허가 요청. 파일 전체 재디자인 불필요 | **현재 0종 확정 / 최대 6종 조건부 개별 검토** |
| **보호, 비대상** | `ObsidianScar_AshPatch_01.png` | Scene에서 사용 중이지만 위 6종과 같은 흰 가이드선 결함 특정 보고 없음 | **재제작 금지** |
| **보호, 미사용** | `ObsidianScar_Ground_Cracked_02.png` | 실제 Scene 참조 없음. 보존용 공식 등록본 | **재제작·활성화 금지** |

**최소 제안:** 현 단계 새 PNG `0`, 교체 확정 `0`. Codex가 **기존 Field09 1280/1600/1920 시안/게임 내 동일화면만 소규모 읽기 전용 비교** → 보고된 **지면 seam(2개 소스)** 및 **가이드선(6개 소스)** 중 실제 플레이에서 눈에 띄는 부분만 점검 → 변경 범위를 *픽셀 영역 단위로* 제시하여 바울 대표에게 **개별 아트 보완 승인** 요청. **유저 승인 전 이 8종조차 재제작하지 않음**.

## 4. 보호·검증·Git 상태

- 이 세션은 **GitHub의 Scene/Importer/문서/FieldConnection 코드만 읽기 전용**으로 감사. Field08·Field09·Main18 공식 PNG 원본/Meta/GUID/Scene/Quest/Monster/Heat/Collision/Save/사용자 dirty를 **변경하지 않음**. PNG 제작·ZIP 제작·Unity Editor 실행·Runtime 테스트 **0회**.
- 이전 `Main18_Final_QA`의 17개 ART `USER_ART_REVIEW_REQUIRED` 및 TTS·BGM 별도 검증을 이 감사만으로 자동 승인하지 않습니다.
- 팀 정책: **`docs/art-renewal`에 문서만 기록**. `main` 직접 커밋/강제 Push/병합 없음.
- 다음: 바울 대표가 조건부 검수 범위에 동의하거나 실제 스크린샷 결함 증거가 제시되면 **최소 PNG 후보 별도 발주/승인**, 그 이전 제작 중단. Field10 이후는 별도 발주.
