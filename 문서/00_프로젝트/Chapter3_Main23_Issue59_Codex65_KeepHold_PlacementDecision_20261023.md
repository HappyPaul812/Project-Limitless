# LIMITLESS / Chapter 3 Main23 — Issue #59 × Codex #65 최종 배치 후보·KEEP/HOLD 판정 기록

## 2026-10-23 최종 표시 기준 정정 — 이전 82px 전체캔버스 기본 후보를 대체 (기획 확정, 미술 미승인)

**이번 갱신 후 적용할 최신 기획 기본안은 `보이는 인물(alpha bbox) 높이 80px`**입니다. **전체 캔버스 높이는 별도 값 `84.63px`**이며 80과 84.63은 같은 단위/기준값이 아닙니다. 본 문서의 아래쪽에 보존된 `82px 전체캔버스 기본 후보`는 **2026-10-23 이전 시점의 임시 기록**으로서 더 이상 최신 기본안이 아닙니다.

| 구분 | 우선 기획 표시안 | 비교용 대안 | 이전 후보(정본 대체됨) |
|---|---|---|---|
| **보이는 alpha 인물 높이** | **80px (기본)** | 96px | 약77.5px |
| **전체 캔버스 표시 높이** | **84.63px** | 101.55px | 82px |
| **Transform scale** | **1.175390** | 1.410468 | 1.1389 |
| **공통/기타** | 512×512 원본, PPU512, Pivot(256,0), ortho5·화면세로720에서 테스트, 새 Collider0 | 사용자 미술 판단 시 비교 | PPU·Pivot 등 그대로 |

- [Codex #65 추가 Unity 실측](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358720003): alpha bbox=(107,24)~(403,508), 원화 Master/512 바이트 불변, **40/80/96/128px alpha 인물높이 시연** 및 신규23 PASS/0 FAIL. 이전 82px의 **캔버스** 표시 높이를 80px **인물** 높이로 이름만 바꾼 것이 아니라 **실측 기반으로 선택을 변경**한 것임.
- [최신 #65 기획 결정](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6359021033), [#59 최신 기획 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6359022163): 80px visible 기획 기본안, 96px visible 비교 대안. **바울 대표 최종 외형 미술 승인 및 정식 Unity 제품 적용 아님**. 근접 이름표의 NPC 하체 가림 OPEN. StableID/VoiceID/Quest 변경 없음.
- **주택**: 기존 Residence 본체·지붕·출입문 **KEEP_OLD**, 신규 RoofGable·RoofTrim·EntryDoor 3 PNG는 삭제·수정 없이 **CANDIDATE_HOLD / NOT_PLACED**. #65 북서 Residence (-12.5,7.2)와 #58 남문 목표 초안 (-13,1)은 동일 대상이라고 단정할 수 없음. **Codex가 실제 남문 목표 좌표/신규 문짝 필요성을 확인하고 바울 대표가 별도 제작 승인하기 전 신규 문 PNG 생성 금지.**
- **여관**: 기존 Inn **본체 내장 간판 하나만 KEEP_OLD**, 외부 `Signs/Frostway_Sign_Inn.png`을 2차로 붙이지 않음. `Inn_StandaloneSign_CANDIDATE.png` 후보 원본 보관/배치 HOLD.
- **새 아트 산출/수정:** PNG 0 / ZIP 0 / Scene·Prefab 0 / 원본 파일 0, 금회 Markdown 보고+최신 인계+Issue 갱신만. 기존43 환경·승인지면9·공식 NPC·펫·몬스터 및 Main23 기능·Save·Quest·OPEN16 불변. `USER_FINAL_ART_REVIEW_PENDING / NOT_UNITY_APPLIED / NO_MAIN_MERGE`.
- **실 Unity 대 오프라인**: Codex의 실 Unity 검수·SHA **댓글 텍스트만 이 세션 직접 확인**. 사용자 로컬 `F:\Downloads\Limitless_MinimalDelta65_Final_Unity_20261023_17db915d.zip`의 실제 34 PNG, index.html은 이 담당이 직접 열지 않았음; 이전 `OFFLINE/APPROX` 합성을 Unity 캡처로 간주하지 않는다.


**일자:** 2026-10-23
**담당:** Chapter3 신규 아트 / `docs/art-new`
**업무상태:** `ISSUE65_TECH_DELTA_PASS / ART_CANDIDATE / USER_REVIEW_PENDING / NOT_UNITY_APPLIED`
**사용자 결정 반영:** 오르나 **82px 전체 표시 높이 기본 후보**, **96px 대안**; 기존 Residence 지붕·출입문 KEEP; 신규 RoofGable·RoofTrim·EntryDoor 배치 HOLD; 여관 본체 **내장 간판 1개만 KEEP**, 신규 독립 간판 보관.

> 본 문서는 **신규 게임 PNG 제작·수정 보고가 아니라, Codex 검수 결과를 근거로 한 배치 및 미술 후보 기록**이다. 사용자 최종 미술 승인과 Unity 본편 적용은 모두 **미완료**.

## 1. 근거 수준과 최신 결과

1. [Issue #59 기획 최종 검수 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6358511540): 오르나 전체캔버스82px 기본, 96px 보조; Residence 기존 지붕/문 KEEP과 별도 남문 위치 불명; Inn 내장간판 KEEP. [Codex #65 직접 인계](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6358419225) 확인.
2. [Codex #65 첫 Unity A/B 직접 보고](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358407741): 격리 변경분 신규33 PASS / 고정카메라 비교8 PASS, 기존 148/136/41/15 전수 재실행 없음. 82px(전체 캔버스)·96px 대안, Residence의 문+박공 중복, 여관 KEEP 판정.
3. [Codex #65 최신 MinimalDelta 실제 Unity 직접 보고](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358720003): 입력 오프라인 참고 ZIP의 SHA/CRC 및 원본 바이트 확인, 오르나 **alpha bbox 인물 높이 40/80/96/128px**를 별도 실 Unity에서 비교, 새23 PASS/FAIL0, 기존 QA 전수 반복0. 보고에는 원본/기존 worktree 28,180 파일 SHA 불변과 본편 적용0/원격 Push0 확인이 포함됨.
4. [Issue #59 최신 Codex 수령 인계](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6358734565): **전체캔버스 높이와 alpha 인물높이는 서로 다르다**는 실측, EntryDoor 접합 HOLD, Inn 내장간판 KEEP을 정리.

**자료 접근한계:** 위 판정은 GitHub에 실제 등록된 **Codex의 서술형 기술 보고 및 개별 SHA 명세를 읽고 정리한 것**이다. 해당 Unity 이미지 **바이너리를 이 아트 세션에서 직접 열어 시각 판정한 것은 아니다**(`CODEX_GAMEVIEW_TEXT_EVIDENCE_ONLY`). Codex 로컬 A/B PNG·`Main23_Frostway_VisualGapList.csv` 전체·Scene 원본 실물은 본 담당이 직접 열람하지 않았음. 이전 아트 담당의 `OFFLINE / APPROX` 합성 이미지와 Codex의 Unity RenderTexture/Camera 비교물을 절대로 동일한 실측으로 간주하지 않는다.

## 2. 현재 배치계약 및 KEEP/HOLD 결정

| 대상 | KEEP / 기본 후보 | HOLD / 보류 | 실제 Unity 증거와 남은 문제 |
|---|---|---|---|
| 오르나 주민 안내 NPC | 기존 Master/512 RGBA 원화 **바이트 유지**. 대표 지시대로 **전체 표시 높이 82px 기본**, **96px 대안** (시각검토 후보) | 40px(너무 작음), 128px(기본 대비 큼); 새로운 원화·StableID·VoiceID·Role 확정 없음 | #65 Orna 표시·전면 정렬·휠체어 통행 `TECH_PASS`; 근접 이름표가 하체를 가림. **82px 최종 사용자 시각 승인 전 미확정** |
| Residence(주택/문/지붕) | **기존 주택 전체 Sprite/지붕/기존 출입문/Collider KEEP** | 신규 `SouthGate_RoofGable_Joint_CANDIDATE.png`, `SouthGate_RoofTrim_Joint_CANDIDATE.png`, `SouthGate_EntryDoor_Joint_CANDIDATE.png` **씬 배치 HOLD**, 파일·SHA 보존 | 신규 Gable·Trim 미설치로 이중 대형지붕 해소. **EntryDoor만 설치해도 작은 눈지붕·문틀·계단이 기존 입구와 중첩**하여 `ART_HOLD_JOIN`. 기존 아트 배치 `TECH_PASS_LAYOUT`와 신규문 미술 PASS는 다름 |
| 여관 Inn | **기존 Inn 본체 Sprite에 포함된 내장 간판 1개만 KEEP** | `Signs/Inn_StandaloneSign_CANDIDATE.png` 신규 독립 후보 보관·미설치. 기존 `Signs/Frostway_Sign_Inn.png` 조각을 두 번째 간판으로 별도 설치하는 것도 금지 | #65 기존 내장 간판 하나만 표시해 중복 해소·휠체어 접근 `TECH_PASS_KEEP_OLD`. 사용자 미술 승인 미완료 |

### 오르나 픽셀 단위 엄격 구분

- **대표님이 지시한 기본 후보 = 82px `전체 캔버스 표시 높이`**: Codex 첫 A/B에서는 `Orna 512×512 PNG / PPU 512 / Pivot(256,0) / Transform scale 1.1389`, 전체캔버스 약82px, alpha 인물 약77.5px. 기존 NPC 캔버스82.286px과 비교 기준.
- **최신 #65 추가 실측 = 80px `보이는 alpha 인물 높이`**, 전체캔버스 약84.63px / `scale 1.175390`. 새23 PASS는 이 단위를 명확히 분리하여 얻은 추가 검증이다.
- **이 두 크기를 동일 측정치로 오인하지 않는다.** 본 팀의 기본 표기 계약은 사용자 지시대로 `82px(whole-canvas)`, 대안 `96px(whole-canvas comparison candidate)`; 최신 #65의 `80px(alpha-visible)`, `96px(alpha-visible)` 검사는 **참고 기술 실측**으로 병기한다. 실제 게임에 크기를 확정 적용하려면 PPU·캔버스 vs alpha 측정기준을 통일한 검토본으로 사용자가 승인해야 한다. 전 캐릭터 PPU 일괄 변경 금지.
- 오르나 테스트 위치 `(-3,-4)` / order2400 / 새 Collider0 / 기존 클릭 Owner 유지. 기존 공식 이름·StableID·VoiceID·Quest대사 확정하지 않음.

### 주택 위치 동일성 주의

- #65 Unity Residence의 실제 검증 좌표는 **북서 `(-12.5,7.2)`** (주택 Collider `3.1×1.6WU`, 본체 PPU128). #58의 초기 **'남문 모듈 주택' 가설 좌표 `(-13,1)`**와 다른 곳이다.
- **북서 Residence의 기술 PASS가 곧 남문 실제 목표 건물/출입구 최종 배치 PASS는 아니다.** Codex는 실제 Main23 Scene 및 `Main23_Frostway_VisualGapList.csv`에서 남문 목표 자체가 별도 필요한지 읽기 전용 대조해야 한다. 없으면 새로운 건물/문을 만들지 않는다.
- 본 담당은 신규 문짝·문틀 단독 PNG **제작하지 않음**. 후속 발주 조건은 (i) Codex가 **실제 남문 목표 좌표 및 문 결함을 확인**하고 (ii) **추가 단일 문짝·문틀 아트가 필요하다고 판정**한 다음 (iii) **바울 대표가 제작 진행을 지시**하는 세 조건이 충족될 때로 제한한다. 그 전에는 기존 출입문 KEEP.
- 휠체어 남북3WU·동쪽 예약7×7WU 신규 오브젝트/콜라이더 추가0. #65 기존 레이아웃 QA 재사용, 아트 담당 별도 Unity 재실행0.

## 3. 보존된 파일·검수 비교 자료 (신규 납품 0)

- **이번 갱신의 신규 PNG/ZIP 생성·교체·수정은 모두 `0`**. 문서 기록만 변경. 과거 ZIP과 이미지의 파일별 SHA는 기존 [#59 아트 파일럿 정식 보고](Chapter3_Main23_Orna_SouthGate_InnSign_ArtPilot_v1_QA_20261023.md) 및 [오프라인 최소보완 QA](Chapter3_Main23_Art59_PostCodex_MinimalDelta_Review_20261023.md)를 따른다.
- 기존 아트 후보 ZIP `Limitless_Main23_Orna_SouthGate_InnSign_ArtPilot_20261023.zip` **기보고 SHA256** `425292ebd1e8a58ab7c4486f6fac39491c265d49b5db933dfe7586e69cee7351`. 소스 PNG 6 후보의 삭제/재작성 없음.
- 기존 OFFLINE 리뷰 ZIP `Limitless_Main23_Issue59_MinimalDelta_Review_20261023.zip` 제작 SHA `2bb31ffaba77f977f426c216b2a1cc9b86934d640c993f4697679b4eb9d67e11`; 오프라인 `Orna_40_80_96_128_Height_ABCD_OFFLINE.png`, `SouthGate_Residence_Keep_vs_DoubleRoof_vs_DoorOnly_APPROX_OFFLINE.png`, `Inn_KeepOriginal_vs_DuplicateStandalone_APPROX_OFFLINE.png` **절대 Unity 화면으로 표시하지 않음**.
- Codex #65 실제 격리 Unity 검수 ZIP (사용자 Windows PC 로컬 경로·Codex 보고 SHA, **이 세션에서 파일 실물 검증하지 않음**):
  - `F:\Downloads\Limitless_Issue65_ArtReview_20261023_4a3afef6.zip`, 보고 SHA `4a3afef6e0abfc3a6b90afd19fe35005c67d9522bcefbdc5993a2777d52c95d1`, `index.html` Board1/2/3.
  - **최신 후속** `F:\Downloads\Limitless_MinimalDelta65_Final_Unity_20261023_17db915d.zip`, 보고 SHA `17db915d60dab9dcf0de521d0ac4e37e8cf9a7c0a8876461dd2aa6e944c333fd`, `index.html`; 신규 실제 Unity14 PNG/기존 실제·근거15 PNG/오프라인 입력3 PNG 등이 **혼합**되어 있으므로 `ImageIndex_SHA256.csv`의 유형 라벨 필수 확인.
  - `B_Orna_Visible40_80_96_128_UnityCrops_UNAPPROVED.png` 보고 SHA `8fb0c1cff9268e02760bdad84364169ec705a77e1ed154397a803d5c458eb566` = Codex가 명시한 실제 Unity 크롭. `A_REUSED_Board2_Roof_Excluded_EntryDoorOnly_UNAPPROVED.png` SHA `8c01be54b4a2eeaf66c3c3d1f2f811d229efb2b84a7cddddd55dd7da4973eb25`; `A_REUSED_Board3_Inn_OneSign_KEEP_OLD.png` SHA `fd1d3f4e7d5baa1eccb9fab3a3c5e67c53d881a67b59cf96bac0e4b2746583a1`. **출처·SHA를 인용하는 것과 이미지 실물 직접 열람은 별개.**
- **GitHub에 이번 새 PNG/이미지 업로드0**; Codex 로컬 `codex/art-minimaldelta-final-review-20261023` 검수 `bc357232...`/보고 `26328382...`는 **`LOCAL_COMMITTED / REMOTE_NOT_PUSHED`**. 이 아트 브랜치에는 위 결과의 텍스트 출처만 반영한다.

## 4. 최종 게이트 및 다음 행동

- 기승인 Earth/ThinFrost/Gravel 지면 타일9, 기존 환경43, NPC/펫/몬스터, Main23 148·136 기능, F1, Quest, Save/UserData, Script, Unity Scene/Prefab, GUID/.meta, OPEN16 **본 작업 변경0**.
- 미술/사용자 승인 상태: `ORNA_82PX_WHOLECANVAS_DEFAULT_CANDIDATE / ORNA_96PX_ALTERNATIVE / RESIDENCE_ROOF_DOOR_KEEP / GABLE_TRIM_ENTRYDOOR_HOLD / INN_BUILTIN_SIGN_KEEP / INN_STANDALONE_CANDIDATE_HOLD / SOUTHGATE_TARGET_LOCATION_CHECK_NEEDED / CODEX_GAMEVIEW_TEXT_EVIDENCE_ONLY / USER_REVIEW_PENDING / NOT_UNITY_APPLIED`.
- 바울 대표가 Codex 실제 이미지 Board1 Orna/Board2 Residence/Board3 Inn의 **시각적 수용을 결정하기 전까지 미술 최종 승인0**. #65 기술 PASS만으로 게임 등록/제품 PR·main 병합을 진행하지 않는다.
- 다음은 Codex가 실제 목표 남문 좌표·중복 문 유무를 읽기 전용 확인하여 Issue #59에 알리는 것. **확정되지 않은 미술 결함을 추정해 새 PNG를 생성하거나 대규모 재제작하지 않는다**.
