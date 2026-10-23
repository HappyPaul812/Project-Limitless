# SPRITE_ART 최신 인계 — 2026-10-12

## 2026-10-23 Main23 #59 최종 표시 기획 규격 — 80px visible 우선 (82px canvas 과거 후보 대체)

- **최신 정본 지시**: 바울 대표 + [Issue #65 기획 최신 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6359021033) + [#59 기획 선택](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6359022163)에 따라 **오르나 보이는 alpha 인물 높이80px을 기획 기본안**으로 한다. **전체캔버스84.63px**은 별도 관측치이다. **96px visible**은 비교 대안(전체캔버스101.55px). `512×512 PNG / PPU512 / Pivot(256,0) / scale1.175390`, #65 실제 Unity23PASS/0FAIL은 Codex 검증치; 과거 **82px 전체캔버스 / alpha약77.5px**은 이전 시점 비교안으로 보존만 하고 **최신 기본값 아님**. 원화 Master·512·StableID·VoiceID·Quest 및 콜라이더·기능 변경0; 근접 이름표 하체 가림은 OPEN 관찰. **기획 추천 ≠ 사용자 최종 미술 승인**.
- **기존 북서 Residence(주택)**의 건물 본체·지붕·출입문·Collider **KEEP_OLD**. 후보 PNG `SouthGate_RoofGable_Joint_CANDIDATE.png` / `SouthGate_RoofTrim_Joint_CANDIDATE.png` / `SouthGate_EntryDoor_Joint_CANDIDATE.png` **원본 유지 + 배치 HOLD**. EntryDoor는 작은 눈지붕·계단/문틀이 기존 주택과 중복되어 `ART_HOLD_JOIN`. 현재 북서 `(-12.5,7.2)` Residence와 #58 남문 예상 `(-13,1)`은 다름. Codex가 실제 목표 남문 필요 아트를 입증하고 대표님이 별도 제작 지시하기 전까지 **신규 PNG/기존 원화 보완0**.
- **여관**은 Inn 본체 PNG에 **내장된 간판 하나만 KEEP_OLD**. 기존 `Signs/Frostway_Sign_Inn.png` 조각도 별도 두 번째 설치하지 않음. `Inn_StandaloneSign_CANDIDATE.png`는 **원본 파일 보관/CANDIDATE_HOLD/배치0**.
- **이번 작업 산출/변경**: `docs/art-new` Markdown 보고 수정 및 인계/Issue 댓글만; PNG0 / ZIP0 / Unity 본편0 / main merge0. 기존43/승인 Earth·ThinFrost·Gravel9/기타 공식 NPC·펫·몬스터/Save·Quest·Main23 기능·OPEN16 유지.
- [최신 **오르나 visible80px 정정 + KEEP/HOLD 보고서**](../../Chapter3_Main23_Issue59_Codex65_KeepHold_PlacementDecision_20261023.md) `docs/art-new` 보고 수정 실제커밋 `855d501c56f2107df01e49a5fefe84d87b0320bb`. Codex 실제 GameView는 로컬 ZIP·텍스트 SHA로 존재하지만 **이 신규 아트 세션은 이미지 자체 미열람**(`CODEX_GAMEVIEW_TEXT_EVIDENCE_ONLY`), 기존 OFFLINE/APPROX 비교와 혼동 금지. **최종 `PLANNING_VISUAL_RECOMMENDED / USER_FINAL_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`**. 바울 대표 최종 승인 전 본게임 적용·main push/merge 금지.


## 2026-10-23 Main23 #59 / Codex #65 최신 실측 — 오르나 82px 기본·남문/여관 KEEP 판정

- **최신 바울 대표 지시·기획/기술 보고 우선**: [#59 최신 기획](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6358511540), [#65 첫 실 Unity A/B](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358407741), [#65 후속 실 Unity 높이 검증](https://github.com/HappyPaul812/Project-Limitless/issues/65#issuecomment-6358720003). **이번 회차 신규 게임 PNG/ZIP 생성·수정·삭제 0, GitHub 문서만 변경**.
- **오르나** 원본 Master/512 Candidate SHA/바이트 보존. **전체 캔버스 표시82px 기본 후보**, 96px 대안(사용자 최종 미술 승인 전 배치 규격 미확정). 82px whole-canvas 인물alpha약77.5px, scale1.1389; 최신 Codex 후속 **80px alpha visible**/캔버스84.63px는 **별개 단위**로 검증(새23 PASS). 40px는 작고128px는 큼; 근접이름표 하체가림 HOLD. PPU512/Pivot(256,0)/오르나(-3,-4), Collider0/현 클릭Owner 유지. StableID/VoiceID/Quest 변경·확정0.
- **Residence/남문** 기존 주택·지붕·출입문·Collider **KEEP_OLD**, 신규 RoofGable/RoofTrim/EntryDoor **현재 배치 모두 HOLD**, 파일 원본 보존. #65 실제 EntryDoor-only Unity에서도 작은 눈지붕/계단/문틀이 기존 출입문과 중복되어 ART_HOLD. #65 실제 검수는 **북서 Residence(-12.5,7.2)**이며 #58 초기 **남문 제안(-13,1)**과 다른 지점! 남문 실제 목표를 Main23 Scene/VisualGapList로 Codex가 확인하기 전 **새 문짝·문틀1PNG라도 제작0**. 필요 입증+대표 제작 지시 후에만 국소 제작 검토.
- **여관** 기존 본체 PNG **내장 간판 딱 1개만 KEEP**. 기존 `Signs/Frostway_Sign_Inn.png`을 별도로 설치해 두 개로 만들지 않음. 신규 `Inn_StandaloneSign_CANDIDATE.png` 후보 바이트 보존·미설치 HOLD. Codex 기술/휠체어 PASS는 최종 사용자 미술승인과 구분.
- **비교 근거:** Codex 실제 Unity board 사진은 사용자 **Windows F:\Downloads 로컬 ZIP** `Limitless_Issue65_ArtReview_20261023_4a3afef6.zip` SHA `4a3afef6e0abfc3a6b90afd19fe35005c67d9522bcefbdc5993a2777d52c95d1` 또는 최신 `Limitless_MinimalDelta65_Final_Unity_20261023_17db915d.zip` SHA `17db915d60dab9dcf0de521d0ac4e37e8cf9a7c0a8876461dd2aa6e944c333fd`의 `index.html`에서 사용자 직접 열람. GitHub에는 **Codex 실측 텍스트/파일별SHA만** 있고 이미지 바이너리 원격 업로드0. 본 담당은 Unity 원본 사진 직접 열람하지 않았으므로 **`CODEX_GAMEVIEW_TEXT_EVIDENCE_ONLY`**. 기존 `MinimalDelta_Review_20261023.zip`의 `OFFLINE/APPROX` PNG3은 실제 Unity 캡처가 **아님**.
- **[이번 정식 판정 보고](../../Chapter3_Main23_Issue59_Codex65_KeepHold_PlacementDecision_20261023.md)** 브랜치 `docs/art-new`, 보고 커밋 `213cffb0db80187bc89276bd418d05ee6caade58`. Codex #65 기술 새33+8 및 후속23 PASS 결과는 **독립 과거/후속 검수 수치**로 합산하지 않고 전체 기존148/136/41/15 재검증0. 원본43/승인지면9/기존 NPC·펫·몬스터·OPEN16·Main23 Quest/Save/Scene/HEAD/index 유지. **`ART_CANDIDATE / USER_REVIEW_PENDING / NOT_UNITY_APPLIED`**, main 직접 Push/merge0.
- **다음:** 대표님 Board1~3 검토·최종 미술 수용 여부 판단 → 필요시 Codex가 남문 실제 타깃 위치/결함만 읽기전용 대조 → 실제 필요 확인 + 사용자 별도 제작 지시 시만 국소 1PNG 제안. 승인 전 제품 적용 금지.


## 2026-10-23 Main23 #59 Codex #64·#65 후속 / 최소 보완 비교, 실제 GameView 근거 대기

- 최신 지시/근거 [#59](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6358022168), [#64](https://github.com/HappyPaul812/Project-Limitless/issues/64#issuecomment-6357901621), [#65](https://github.com/HappyPaul812/Project-Limitless/issues/65). #64 사용자 전달 Codex 로컬 QA상 오르나 TECH_PASS/40px 너무 작음, 남문 이중 지붕 ART_HOLD, 여관 신규 간판 렌더 PASS이나 기존 간판과 중복; #65 실제 Unity 비교캡처·GapList 파일은 아트 담당에게 미전달, 2026-10-23 조회시 #65 댓글0. **실 Unity A/B 최종 QA 주장 금지**.
- 새 게임용 PNG 0 / 기존 후보 및 기승인 PNG 수정 0. **기존 오르나 Master·512 보존**, 오프라인 40/80/96/128px 표시 높이 비교(80·96 중간 검토, 128 Codex 요청 대조); 기존 Residence 주택·지붕 **KEEP**, 신규 큰 RoofGable/RoofTrim **배치 제외 후보**, EntryDoor 단독 오프라인 접합 C안에서 큰 중복은 줄지만 EntryDoor 자체의 작은 눈지붕/기존 출입문 겹침 위험으로 **ART_HOLD 유지**; Inn 기존 간판 **KEEP_OLD_RECOMMENDED**, 신 StandaloneSign 삭제 없이 HOLD 보존.
- **실물 파일(신규 아트 ZIP이 아닌 A/B 증거 ZIP):** `Limitless_Main23_Issue59_MinimalDelta_Review_20261023.zip` 812,046 bytes / 7 entries / SHA256 `2bb31ffaba77f977f426c216b2a1cc9b86934d640c993f4697679b4eb9d67e11`; SHA256SUMS6/6 + ZIP CRC PASS. 오프라인 비교 PNG3(오르나 1480×590 `376cd075...`, 남문 1840×680 `52172ca3...`, 여관 1350×655 `f7e83059...`)와 QA 보존 목록·Manifest. 과거 파일럿 v1 ZIP SHA `425292ebd1e8a58ab7c4486f6fac39491c265d49b5db933dfe7586e69cee7351` 보존. 실물 ZIP은 본 ChatGPT 세션 다운로드용 /mnt/data, GitHub 바이너리 미업로드.
- [새 정식 검토 보고](../../Chapter3_Main23_Art59_PostCodex_MinimalDelta_Review_20261023.md) `docs/art-new` commit `d5a07c99b722bb2f00fd69088ccb6c5a0f1ee08e`.
- **상태:** `ART_CONTRACT_WAITING_CODEX_EVIDENCE / ART_CANDIDATE / USER_REVIEW_PENDING / NOT_UNITY_APPLIED`. 기존43·승인 타일9·공식 NPC/펫/몬스터/Save/Quest/Scene/OPEN16 보호, Unity 제품/import/원격 main 병합0. 다음은 #65 실제 화면/PPU·Pivot/Collider 전달 후 새 PNG 없이 배치 판단 → 필요 입증된 국소 변경만 별도 지시 → 대표 미술 승인.


## 2026-10-23 Main23 #59 정식 파일럿 수령 확인 — 사용자 Windows ZIP / 미술 승인 대기

- 바울 대표가 `F:\Downloads\Limitless_Main23_Orna_SouthGate_InnSign_ArtPilot_20261023.zip` **수령 직접 확인**. ZIP 제작본 SHA256 `425292ebd1e8a58ab7c4486f6fac39491c265d49b5db933dfe7586e69cee7351`, 8,008,308 bytes / 20 entries / ZIP CRC PASS / SHA256SUMS 19/19 재확인. 사용자의 F 드라이브 복사본 바이트를 아트 세션에서 직접 해시한 것은 아님.
- 작업군 오르나 안내 NPC1, 남문 주택 문·지붕 접합1세트(3 PNG), 여관 독립 간판1; 신규 RGBA PNG6(오르나 원본/512 2종 포함). 오르나 128·40px 오프라인 미리보기/단독 원화, 남문·간판 전후, 모의 3비율 포함. ZIP에는 Manifest/파일별 SHA/QA 포함. 기존 건물·문·지붕·간판 재사용 KEEP, 기존 게임용43과 승인 지면9 및 공식 기능 변경0.
- 상태 **`ART_CANDIDATE / USER_REVIEW_PENDING / NOT_UNITY_APPLIED`**. 남문 기존 지붕과 후보 박공이 중복되는 **`SOUTHGATE_JOIN_ART_HOLD`**, Codex 실 GapList CSV 행·GameView·PPU/Pivot/Sorting/Collider 실측 미입수/아트 QA NOT_TESTED. Codex 다른 격리 변경분136 PASS/0 FAIL은 **이 신규 미술 승인 아님**. 오르나 StableID/VoiceID 미확정.
- 공식 문서 [Main23 오르나·남문·여관 간판 v1 QA](../../Chapter3_Main23_Orna_SouthGate_InnSign_ArtPilot_v1_QA_20261023.md) 수령·승인 게이트 보강 커밋 `f1c559d370b0016fd346d2b8b97328438ee13f49`, 담당 브랜치 `docs/art-new`. Issue #59 댓글 최신화 대상. **사용자 최종 미술 승인 전 Unity 본게임 적용·main 병합 금지**. OPEN16 현상 유지.


## 2026-10-23 Main23 #59 — 오르나·남문 접합·여관 간판 실 PNG 시안 납품 v1

- **현재 정본 신규 아트**: 사용자 요청 3작업군(오르나 NPC1, 남문 주택 출입구/지붕접합3 PNG, 여관 독립간판1), 오르나 Master+512파생을 포함해 **독립 후보 PNG6**. 기존 43개 환경과 승인 지면9·F1/펫·Scene/Quest/Save/GUID 수정0.
- **실물** `Limitless_Main23_Orna_SouthGate_InnSign_ArtPilot_20261023.zip` / SHA256 `425292ebd1e8a58ab7c4486f6fac39491c265d49b5db933dfe7586e69cee7351` / 8,008,308 bytes / 20 entries / ZIP CRC PASS / SHA256SUMS19/19 PASS. ZIP과 개별 신규 RGBA PNG는 이번 ChatGPT 대화 다운로드로 사용자 수령 가능, GitHub는 바이너리 미업로드. Manifest/개별 후보SHA·크기/오르나 512·128·40px 오프라인 미리보기/접합·간판 전후/오프라인 3화면비 증거 포함.
- **중요 HOLD**: 남문 기존 Residence에 새 출입구·지붕을 합성한 참조판에 이중 박공/겹지붕 결함이 보이므로 **SOUTHGATE_JOIN_ART_HOLD**. 현재 신규 3작업군은 `ART_CANDIDATE / USER_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED`, 재사용 가능한 기존 건물·문·간판(43자산)은 KEEP 검토. 오르나 StableID·VoiceID 정식확정0.
- Codex #58 사용자 제공 요약 **격리화면 변경분136PASS/0FAIL, 기존43 중18종 시연, 승인 지면9 보존**을 확인. 하지만 행별 `Main23_Frostway_VisualGapList.csv`와 실제 GameView 파일은 아트측에 미전달이므로 신규 PNG의 PPU/Pivot/footprint/Sorting/Collider/휠체어동선 **NOT_TESTED**. 128·40px 미리보기는 오프라인 대용이지 실제 게임 크기 QA PASS 아님.
- **[GitHub 상세 제작·QA 보고](../../Chapter3_Main23_Orna_SouthGate_InnSign_ArtPilot_v1_QA_20261023.md)** `docs/art-new` commit `1921c3020f77eb53f232ef58b73490ac962fb948`. Issue #59 최신 기획 댓글 `#issuecomment-6355677632` 반영. 운영방침: 아트 완료 보고 → 최신 인계 갱신 → #59 보고 댓글 → 대표님 링크·SHA 전달. 사용자 승인 전 Unity 본편/main PR·Push·병합 없음. OPEN16 미해결16 유지.
- **후속 작업**: Codex GapList 상위3행/실화면 근거 안전 전달 → 남문 접합 표적 보완과 여관 중복 간판 실제 재사용 판정 → 대표 최종 미술 승인 → 승인분만 Codex 격리 DeltaQA/별도 적용 승인.


## 2026-10-23 Main23 서릿길 마을 외형 보완 #59 — 정식 GitHub 증거보고 / 신규 아트 미납품

- 바울 대표 명시 새 운영규칙: 작업 완료 보고는 **`docs/art-new` 문서+본 인계 실제 커밋, Issue 댓글**로만 성립. 대화 요약은 납품 완료 증거가 아님. 해당 작업의 파일별 실제 존재/SHA256/QA/사용자 승인, `main` 미반영을 반드시 분리한다.
- [Issue #59](https://github.com/HappyPaul812/Project-Limitless/issues/59) 최신 [2026-10-23 기획 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6355299474), `docs/planning` Main23 Presentation 계약 확인. Main23 첫 플레이 **격리 기능 148PASS**는 신규 아트 검수/유저승인 아님. Codex #58 실제 `Main23_Frostway_VisualGapList.csv`, GameView·PPU/Pivot·Sorting/footprint 증거 **미전달**.
- **현재 #59 신규 실물 상태: 오르나 PNG `NOT_DELIVERED`; 남문 목석 건물 접합 신규1세트 `NOT_DELIVERED`; 스킬상점/여관 신규 외형·간판1세트 `NOT_DELIVERED`; 접합 전후/128px·40px/화면비3종 신규 아트 QA `NOT_TESTED`.** 잘못된 요약의 ‘제작 완료’를 따르지 않는다.
- 실물 보유 과거 v9 ZIP SHA `afea1ed0b357a5bdce9e962c7969b81b77d29e1ed11b7b9dc170d57747fbeb79`에서 기존 49 게임 PNG 49/49 RGBA 경로·SHA 확인. 기존 Residence502×374 / SkillShop511×418 / Inn540×443 및 Roof Gable175×219, WoodFrame Door148×210, SkillShop Sign125×173 등은 **재사용 검토 후보**; v5 실제 로컬 ZIP·Unity 화면 및 동일SHA는 직접 미검증. 승인 지면9/기존 43/펫3종 불변.
- **이번 턴 실제 생성한 것은 신규 게임 PNG가 아니라 검수 참고자료**: `Limitless_Main23_Issue59_ReuseAudit_PRELIMINARY_20261023.zip` SHA `83d621d8f32fde8f31b10f10e4fa5fb29c25ff2677b472e74862f9eccd93739d` (CSV49 + 기존자산 Contact Sheet + Manifest + SHA256SUMS; CRC/SHA 3PASS). 실물은 이 대화 다운로드 첨부, **GitHub 바이너리 미업로드**.
- [정식 #59 보고서](../../Chapter3_Main23_FrostwayVillage_VisualCompletion_Pilot_20261023.md) 보고 커밋 `88e0e60a2048e25f1fab6effb782848c5c13516c`. 상태 **`REUSE_BASELINE_AUDITED / NEW_ART_NOT_DELIVERED / USER_ART_REVIEW_PENDING / NO_UNITY_APPLY`**. `OPEN16` 미해결16, main 직접 commit/Push/merge0.
- 다음: Codex #58 실 GapList와 GameView/좌표/PPU 확보→KEEP 대 신규 필요표 확정→3개 표적 신규 자산 비파괴 제작/비교/실물 SHA·ZIP→**그때** `docs/art-new` 새 완성보고 커밋·Issue 댓글·바울 대표 미술 승인 판단. 공식 적용 금지.


## 2026-10-22 Main23 서릿길 마을 v9 — 새 01 포함 family 9종 비파괴 대체 파일럿

- **사용자 최신 발주**: Codex v8 실제 기술PASS/미술 Earth·ThinFrost·Gravel 모두 HOLD. 02·03만 반복 수정하는 방향 중단. 기존 공식43 PNG 원본 보존 상태로 **family별 새 01/02/03 전9개 `Candidate/Tiles/` 독립 후보**. 원본 v8 ZIP 실물 SHA `6a9fa1c3c0c1ffef3ac43d3541a28bbc0a16310d2e0f8c267ec7177d731d712f` 확인.
- **v9 납품 실물**: `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v9.zip` / SHA256 `afea1ed0b357a5bdce9e962c7969b81b77d29e1ed11b7b9dc170d57747fbeb79` / 21,738,388bytes / 73 entries. 오프라인 QA: 원본 게임 PNG49 byte/SHA 모두 보존(승인43 포함); 새 후보9 전부256 RGBA/opaque; 54 ordered seam delta0; ZIP CRC·SHA256SUMS72/72 PASS. v8↔v9 동일 타일ID/128px 셀 5×5/8×8 비교6, **Unity GameView 아님**.
- **01 고정 원인**: v8만의 큰 01 돌/풀과 신02/03 가는 입자 차이가 사각 구획의 유력 원인. v9 전3종 질감·모티프를 한꺼번에 교체한 후보에서 개선 가능성을 시각 비교하지만 01 단독의 인과 검증은 아니다. **Gravel 등의 미세 반복/그리드 잔존·ThinFrost 표현**은 바울 대표 미술 검토 및 Codex 실제 Unity 검사 필요.
- **OPEN16**: Codex CSV/XY 원문 art-new에 아직 미전달. 미해결16/완화0/해결0, 원본문제와 무관한 자산 수정0. 공식 SkillShop·RoofGable·StonePath·43게임PNG·v8 variant6 및 F1/Wolf/Bear/Fox/Mouse·Unity/Save/원본 PC파일 보호.
- **GitHub 보고**: [v9 제작·QA](../../Chapter3_Main23_FrostwayVillage_GameArt_VariantTiles_v9_Art_QA_20261022.md), `docs/art-new` 보고 커밋 `134271fbc83c6d0a79e465c4ac0388a5a004b077`. ZIP/PNG는 대화 다운로드로 납품, GitHub 바이너리 업로드0. v9는 **CANDIDATE / OFFLINE_QA_PASS / USER_ART_APPROVAL_PENDING / NO_UNITY_APPLY**.
- **다음**: 바울 대표 9개 PNG 및 5×5/8×8 동일배치 전후 미술 검토 → (별도 승인/지시 시) Codex 격리 Unity DeltaQA 및 OPEN16 원문 전달 → 최종 미술 승인 후에만 공식 타일 반영 검토. `main` 직접 commit/Push/merge 금지.


**최종 사용자 결정(2026-10-12): 리뉴얼 담당은 Chapter 1·2 기존 아트 교체(환경·몬스터·보스·펫·NPC·VFX)만 전담합니다. 기존 스프라이트 담당은 Chapter 3 이후 모든 신규 아트(환경·필드·던전·몬스터·보스·펫·NPC·캐릭터·VFX·UI 필요 이미지)를 총괄합니다. 기존 공식 승인 자산은 별도 승인 없이 교체하지 않습니다. Codex는 구현·Unity 통합·QA 전담입니다.**

**Chapter 3 이후 모든 신규 아트** 전담. 신규 배경·지역·던전·몬스터·보스·펫·NPC·캐릭터·초상화·VFX·필요 UI 제작. Chapter1·2 무료 아트 교체는 리뉴얼 담당 작업이므로 중복 제작 금지. 최신 Chapter3 설계 `문서/06_스킬/Chapter3_스킬상점_강화_냉기_동결괴사_20261012.md`와 스토리·몬스터 정본을 확인하고 공식 원본 보존. 실제 사용자 미술 승인·Codex QA 전에는 구현 완료로 보고하지 않습니다. 자세한 결정은 `../../LIMITLESS_20261012_전체대화_결정_인계.md`.

## 2026-10-13 Chapter3 북쪽 환경 비주얼 기획 v1 — 콘셉트 초안, 사용자 미술 검토 대기

- **역할**: Chapter3부터 모든 신규 아트(환경·던전·몬스터·보스·펫·NPC·캐릭터·VFX·UI)의 아트 총괄. Chapter1·2 기존 무료/공용 아트 리뉴얼은 별도 담당; 이미 승인된 하렌/세린/태온/폴/미엘/플레이어와 기존 Scene/PNG는 변경하지 않음.
- **확인한 원격 기준**: `HappyPaul812/Project-Limitless` `main` `eca6f1e5ceb42e9d6c88b53368bc109280a547b8`. Chapter3 초기 방향, 태온 형제애, 스킬 상점·강화/한기·동상·동결괴사, 냉기 보스 화로, Chapter3 몬스터·펫·경제 준비, Haren/Serin 통합 QA, 등장인물 정본 확인. PC LOCAL `git status`와 실제 Unity 실행은 확인하지 않음.
- **기획 확정 배경**: Chapter3 Main22 시작, 북쪽으로 추위→서리→첫눈→눈길→본격 설원→심부 동토의 환경 흐름. 이는 시각/기후 단계이지 공식 Field/Scene 6개를 확정하는 숫자가 아님. Main21 초보 마을에는 본격 적설이 없고 차가운 공기/북풍/아침 서리만 제안.
- **오늘 제작물(대화 산출물·GitHub 미등록)**: `Limitless_Chapter3_North_Visual_Dev_v1.zip`; 내부 `Docs/Chapter3_North_Visual_Tone_Guide.md`, `Docs/Chapter3_New_Art_Requirements.md`, `Concept/Chapter3_NorthernApproach_Stage01_Concept.png`(1200×765 RGB 참고 시안), `Concept/Chapter3_North_Visual_Moodboard_Draft.png`(1536×1024 RGB 참고 보드), `Manifest.md`. ZIP 무결성 검사 PASS; 픽셀아트의 게임용 Tile/PUU/Pivot/Collider나 Unity Import 검증 아님. 생성 시안의 비공식 인물/생물/건축물은 확정 캐논 아님.
- **톤 가이드 제안**: 초기 흙색/회녹색/생활감 유지 → 가장자리 서리 → 흙이 노출된 첫눈 → 눌린 눈길 → 넓은 설원 → 암반·얼음이 혼합된 깊은 동토. 완전 흰색 단일화 금지, 가독성·명도층·바람 패턴·통행경계 차이를 강화. 제안 팔레트 및 적설 퍼센트는 **연출 참고치**이며 스킬/게임플레이 시스템 수치 아님.
- **자산 제안**: 신규 Ground/Transition/눈 패치/식생/바위/얼음·동토, 이후 던전/신규 몬스터·보스·펫·NPC·캐릭터·VFX·UI/전리품·상점 아이콘. 구체 Scene/지역 수/몬스터 ID/보스 이름/게임용 해상도/PPU/프레임은 전부 미정.
- **검증 및 승인**: `ART_CONCEPT_DRAFT / USER_ART_REVIEW_PENDING / NOT_UNITY_INTEGRATED`. 사용자 아트 승인0, Codex Runtime QA0. 기존 공식 PNG/Unity/Save/코드/GUID 변경0. 사용자 미술 승인과 기술 검증은 분리.
- **접근성**: 기존 이동 보조기/휠체어를 포함한 통행 경로의 실루엣·패턴 대비 보호, 상태 전달은 텍스트+형태와 함께, 번쩍임/빠른 반복 화면 흔들림 금지. 보스 전투 중 화로는 Field 소품으로 임의 확정 금지.
- **다음 우선 작업**: (1) 사용자에게 첫 구간 시안 및 단계별 톤 검토/승인 받기 (2) 기획 담당에게 실제 첫 신규 구간/Scene·몬스터·NPC와 별도 계약 확인 (3) Codex에 기존 Tile/PPU/Pivot/Collider 등 LOCAL 비대화형 조사를 의뢰하고 실사용 PNG 분해 제작, 승인 후에만 Unity 통합·Runtime QA. 중복 아트는 리뉴얼 담당에 조율.
- **후임 시작 문구**: “너는 LIMITLESS Chapter3 이후 신규 아트 총괄 담당이다. GitHub SPRITE_ART.md, SPRITE_ART_LATEST.md, Chapter3 초기 방향·형제애·냉기/스킬·몬스터/경제 정본 및 CURRENT_STATUS를 읽고 와. 2026-10-13 북쪽 환경 콘셉트 초안은 미술 승인 대기이며, 기존 공식 캐릭터/Chapter1·2 리뉴얼 파일을 건드리지 마.”

## 2026-10-13 GitHub 브랜치 협업 규칙

- **본 담당의 유일한 GitHub 문서 작업 브랜치: `docs/art-new`**. `main` 직접 커밋·Push·병합하지 않음. main 반영은 사용자 승인 후 별도 절차.
- 타 담당 브랜치: 기획 `docs/planning`, 리뉴얼 `docs/art-renewal`, TTS `docs/tts`, BGM `docs/bgm`, SFX `docs/sfx`, 영상 `docs/video`, Codex `codex/작업명`(별도 로컬 worktree).
- 최신 원격 main 및 담당 브랜치, 수정 대상 blob SHA 확인 후 자기 문서만 변경. 다른 담당 변경 강제 덮어쓰기·force push 금지.
- 완료 보고 필수: 브랜치 이름/커밋 SHA/변경 결과/사용자 승인 필요 여부.
- 이전 Chapter3 북쪽 콘셉트 초안은 `ART_CONCEPT_DRAFT / USER_ART_REVIEW_PENDING / NOT_UNITY_INTEGRATED` 상태를 유지하며, 이번에는 게임 아트·Unity 자산을 변경하지 않음.
- 이 규칙 갱신은 담당 아트 역할 문서와 인계문에 한정; 공통 규칙이나 타 담당 파일을 이 브랜치에서 수정하지 않음.

## 2026-10-20 Chapter3 병렬제작 Issue #30 — 파일럿 아트 납품·사용자 미술 승인 대기

- **발주:** `docs/planning`의 `문서/00_프로젝트/Chapter3_Team_Parallel_Production_Kickoff_20261020.md`, GitHub [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30). 이번 단계는 `CH3_PARALLEL_PREPRODUCTION_AUTHORIZED`; Main22~24 정식 ID/Scene/Dialogue Production Lock·아트 승인·전체 Runtime 미허가.
- **완료(콘셉트 파일럿):** F1 흙·얇은서리 지면·Transition/길 Overlay/서리 관목/목제 표지판 6 PNG, 서릿길 마을 자갈 바닥/목석건물/보급상자/등불 4 PNG, 서리들쥐 1포즈·설빙의 폭군 실루엣 1포즈 KeyArt 2 PNG. 총 **독립 PNG12**. 미리보기3/기본 타일2×2 반복QA3 별도. 모두 새로운 독립 아트 시안, 기존 공식 인물/게임 씬/아트 파일 불변.
- **파일:** `Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip`는 이번 ChatGPT 대화에서 직접 다운로드 가능. SHA-256 `6a22ac4e9c4d4f4f9e37c2ac8270b145d5483676d00f83c320cdf0e6925382b4`. **실제 PNG/ZIP 바이너리는 GitHub에 올리지 않았고, 담당 브랜치의 문서에는 명세와 SHA만 기록**. 별도 대화 세션에서는 해당 ZIP이 자동 제공되지 않으므로 사용자/기획 담당에게 전달 경로를 확인할 것.
- **GitHub 기록:** [Chapter3_ArtNew_F1_Frostway_Pilot_Production_QA_20261020.md](../../Chapter3_ArtNew_F1_Frostway_Pilot_Production_QA_20261020.md), 담당 브랜치 `docs/art-new`, 작업 보고 커밋 `5b46e3d6bed2e4dbe3b4c3ef5aba0f94949f3d44`. 최종 인계 변경 커밋은 해당 문서의 Git History 참조.
- **오프라인 QA:** 18 PNG 디코딩, 독립 PNG12 RGBA/256·512 규격 PASS, 3 Ground Base 좌우/상하 경계 픽셀 일치 PASS, 압축21항목 CRC PASS, Manifest 파일별 SHA 기록. 반복무늬의 눈에 띔/초상화와 스타일 일치·아트 UI/동선 가독성은 사람 미술검토 필요. 샘플 이미지는 Unity 캡처가 아님.
- **미정 및 게이트:** Codex Issue #35의 실측 PPU/Pivot/Collider/Sorting/WorldBounds/휠체어 통행 경로·Scene 구조 보고 전 `GAME_READY` 표기 금지. 마을 북문/남문·동쪽 포탈 공간은 콘셉트 표시만, 동쪽 포탈 **비작동**. Monster/NPC/Quest/Scene ID·장착 스킬 등 새 정식 ID 임의 확정 금지. 기존 하렌·태온·세린·폴·미엘·플레이어/Chapter1·2 공식 자산 보호.
- **다음 작업:** (1) 바울 대표 F1/마을/두 KeyArt 미술 의견 받기 (2) 기획 Production Lock·Codex PPU/Pivot/Collider 보고 확인 (3) 승인받은 자산만 실제 Import 계약에 따라 조정·확장 후 아트 승인 및 Codex Unity 통합 분리 진행. `main` 직접 Push/병합 금지.
- **새 세션 시작 문구:** “너는 LIMITLESS Chapter3 이후 신규 아트 총괄이다. `docs/art-new`의 `SPRITE_ART_LATEST.md`와 `Chapter3_ArtNew_F1_Frostway_Pilot_Production_QA_20261020.md`, 기획 `docs/planning`의 병렬 착수 문서 및 Issue #30을 읽어 와. F1·서릿길마을 독립 PNG12 파일럿은 사용자 미술 승인/PPU 기술 계약 대기 중이고, 21항목 ZIP은 이전 대화 산출물이니 실물이 확인되기 전 GitHub에 업로드됐다고 말하지 마.”

## 2026-10-20 몬스터·보스 1차 키아트 반려 및 2안씩 재시안 v2

- 바울 대표 **반려 확정**: 첫 파일럿 KeyArt `NorthF1_FrostFieldMouse_KeyArt_Concept.png` 및 `FrostTyrant_Silhouette_Concept.png`는 몬스터가 완구/마스코트 같고 보스가 충분히 무섭지 않음. **이전 1차 ZIP 안의 해당 원본을 덮어쓰지 않음.**
- **재시안 제작/포장 완료(승인 대기)**: `Limitless_Chapter3_Monster_Boss_Concept_Revisions_v2.zip`; 영문 개별 원본 `FrostFieldMouse_Concept_A.png`/`B.png`, `FrostTyrant_Concept_A.png`/`B.png`; 비교 미리보기 1, MD/JSON Manifest 2, QA JSON 1 = 총8항목. ZIP SHA256 `7b6cc00e896ffa1ddcf4e4aa6fff4b05cbf160d587ef209db5f5e72c53e97561`.
- **한계 명시:** 네 원본은 1254×1254 RGB(쥐)/1122×1402 RGB(보스)이며 고해상도 배경 포함 콘셉트로, 픽셀아트 투명 스프라이트가 **아님**. 사용자께 A/B 디자인 검토를 받은 뒤 Chapter2 공식 아트 톤에 맞는 정식 **2D 게임용 픽셀아트 1포즈**로 다시 제작해야 함. Unity/게임 원본 수정 없음.
- 문서·파일별 SHA·QA: [Chapter3_Monster_Boss_Concept_Revisions_v2_QA_20261020.md](../../Chapter3_Monster_Boss_Concept_Revisions_v2_QA_20261020.md) (커밋 `2cbe7299e0d3c88ab8cea100247fe4e826e583de`). 바이너리는 이 ChatGPT 대화에 다운로드 링크로 제공했으며 GitHub 브랜치에는 **기록 문서만** 올렸음.
- F1/서릿길 마을 지형 PNG 10종은 별도 기존 파일럿 그대로 보존; 몬스터/보스 신시안은 기존 2개와 별도 패키지.
- **후임 우선**: 서리들쥐/설빙의 폭군 각각 채택안 확인→게임용 픽셀아트 스타일로 단독 1포즈 제작→Codex PPU/Pivot/Collider 조사 반영→대표 미술 승인 후 Unity 연계. 전 몬스터 7종 애니메이션 양산 금지. `docs/art-new`만 사용, main 직접 수정 금지.

## 2026-10-20 채택안 서리들쥐 A·설빙의 폭군 B — 픽셀아트 1포즈 v1

- 바울 대표 디자인 **서리들쥐 A, 설빙의 폭군 B 채택**. 기존 반려본/기존 원본은 보호. 채택은 고해상도 콘셉트의 디자인 방향 승인이고, 아래 픽셀아트 1포즈 **별도 미술 승인 필요**.
- 새 대화 산출 ZIP: `Limitless_Chapter3_ApprovedAB_PixelArt_1Pose_v1.zip` (SHA256 `4bd67210e4d813b1ceb5c22cd76a35af577837b63068af57392c41226da968fc`). `Sprites/FrostFieldMouse_A_PixelArt_1Pose.png`/`Sprites/FrostTyrant_B_PixelArt_1Pose.png` **512×512 투명 RGBA 각1포즈**, 비교 및 128px 검토 이미지·Manifest·QA JSON 총 6항목. 실제 PNG는 ChatGPT 대화 첨부이며 GitHub에는 바이너리 미업로드.
- 오프라인: 두 PNG 크기/투명도/단일 주요 컴포넌트/외곽 여백/ZIP CRC PASS. 본 데이터는 승인 후보; PixArt 마감, 게임 내 스케일/유사 아트 결합, 움직임은 검증·제작 안 됨. `SPRITE_ART_APPROVAL_PENDING / NOT_UNITY_INTEGRATED`.
- 정식 기록: [Chapter3_ApprovedAB_PixelArt_1Pose_v1_QA_20261020.md](../../Chapter3_ApprovedAB_PixelArt_1Pose_v1_QA_20261020.md), 커밋 `ad5f35cbe8d4f2ffde8768ba55b0d4c05ad4395a`.
- 후임 다음 단계: 바울 대표 1포즈 아트 검토 → Codex Issue #35 PPU/Pivot/Collider·Monster Frame 계약 대조 → 별도 요청을 받아 필요 애니메이션만 제작. 기존 F1/서릿길 마을 환경과 Chapter1·2 원본, 하렌/태온/세린/폴/미엘/플레이어 변경 금지. `docs/art-new`만 사용하고 main 커밋/Push하지 않음.

## 2026-10-20 서리들쥐 A·설빙의 폭군 B 1포즈 v2 재작업

- Codex 비교검수 v1: 규격 기술 PASS, 미술 HOLD. **기존 A/B 디자인 선택 유지**. 새 512×512 RGBA **투명 1포즈 PNG 2개** 재제작했으나 사용자 미술 승인 대기.
- 새 납품: `Limitless_Chapter3_ApprovedAB_1Pose_ArtRework_v2.zip`, SHA256 `0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919`. 게임 후보 PNG 2 + 비교 Preview1 + 128px QA1 + 공식 Ch2 이미지 비교1 + Manifest1 + Offline QA JSON1 + SHA256SUMS1 = 총8파일; CRC PASS.
- 새 고해상도 원화에서 출력하여 이전 v1 75×49/142×121 최근접 확대 작업을 반복하지 않음. **픽셀풍 고해상도 디지털 드로잉으로서 최종 수동 픽셀 마감/Ch2 완전 일치 미확인**. 임시 Chapter2 비교판은 동일 윤곽 크기로 정규화한 시각 참고판이며 실제 Unity PPU와 동일 실게임 크기 검증 아님.
- 담당 QA·SHA 보고: [Chapter3_ApprovedAB_1Pose_ArtRework_v2_20261020.md](../../Chapter3_ApprovedAB_1Pose_ArtRework_v2_20261020.md), 커밋 `a3822c157c754b1c17ecd1c001b245ff33a41eff`. 실제 바이너리 ZIP/PNG는 이 대화 다운로드 첨부에만 존재하며 GitHub에 올리지 않음.
- **USER_ART_APPROVAL_PENDING / CODEX_ACTUAL_GAME_SCALE_QA_PENDING / NOT_UNITY_INTEGRATED**. 차기 우선: 바울 대표 미술 감상→Codex 실제 PPU/표시 크기 비교 QA→개별 수정 필요 시 새 승인본. F1·마을 기존 PNG10/공식 캐릭터/Chapter1·2 아트/Unity 원본 보호, 애니메이션 양산 금지, `main` 미수정.

## 2026-10-20 Chapter3 A/B Idle 애니메이션 기술 사전조사

- 바울 대표: 서리들쥐 A/설빙의 폭군 B 1포즈 v2 미술 승인. SHA PNG A 0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2 / PNG B f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830. 원본 변경 금지.
- GitHub 원격 Main의 MonsterDefinition, MonsterSpriteSheetAnimation, FieldMonsterInstaller, BattleSceneController와 Ch2 .asset/.png.meta 확인. 기존 Ch2 선례 Idle4/8FPS/314px 셀/PPU314/BottomCenter Pivot/Point, 일반 VisualScale1·열맥거신3, 전투 Image136×112 UI px × scale.
- 주의: Texture2D 런타임 분할 Sprite.Create Pivot은 (0.5,0.5)이고, TextureImporter 분할 ExplicitIdleFrames Pivot은 (0.5,0). 섞으면 발밑 정렬 위험.
- [Idle 기술 사전조사 보고](../../Chapter3_ApprovedAB_Idle_Animation_Technical_Preflight_20261020.md), 커밋 104e005952ba833e4eafa7d44333410ebe721180. 아트 담당의 GitHub 원격 확인 기록이지 Codex 로컬 구현·Unity 실측 기술 계약 완료가 아님.
- Codex가 별도 codex/작업명 로컬 worktree에서 4프레임 전용 strip/Import PPU·Pivot·실효 UI 크기/신규 Scale과 field sorting·collider 계약을 잠가 기획/아트 담당에 인계한 후에만 Idle 파일럿 PNG 제작. Main, F1/서릿길 마을, 승인 캐릭터, Unity/Save 변경 금지.

## 2026-10-20 서리들쥐 A·설빙의 폭군 B Idle 4프레임 v1 실제 납품

- 바울 대표가 두 1포즈 v2 미술 승인 후 Codex **최종** Idle 계약: **2048×512 RGBA / 4프레임(가로 4×512) / 8FPS / PPU512 / Pivot(0.5,0) / 우향 / VisualScale Mouse1·Tyrant3**. 이전 PPU314안은 미확정 임시 참고였으며 폐기.
- 실제 PNG2: `FrostFieldMouse_A_Idle_4Frames.png` SHA `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2`; `FrostTyrant_B_Idle_4Frames.png` SHA `9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43`.
- 실제 대화 ZIP: `Limitless_Chapter3_FrostFieldMouse_FrostTyrant_Idle_v1.zip`, SHA256 `6c01e55c147ab57147c767f43503553164197cc037a3834ecd2d15d10d96a23c`, 약 6.95MB. 총 12개 파일: 시트PNG2 + 각 4프레임 확인PNG2 + 정확한 8FPS APNG2(125ms/프레임) + GIF2(120/130ms 교대, 총0.5초) + 128px 전체 비교PNG1 + Manifest/QA/해시목록3. 실제 ZIP/PNG는 이 ChatGPT 대화 다운로드에 존재하며 GitHub에는 문서/해시만 기록.
- 제작: 승인된 1포즈 PNG를 직접 기준으로 사용하고 국소적인 몸통·머리/수염·꼬리·흉곽·팔 움직임을 제작. Frame00는 승인 원본 픽셀과 같고 각 후속 3프레임은 독립적 변형. 발밑 y438~511 영역 원본과 100% 픽셀 동일. 2048×512 RGBA/4×512/투명 외곽/프레임 차별화/ZIP CRC/원본 SHA 오프라인 PASS.
- [Idle v1 제작·검수 상세](../../Chapter3_ApprovedAB_Idle_4Frame_v1_Production_QA_20261020.md) — 최신 커밋 `8c94f9de53b3c706f67a1bf59b515cdae092eaf8` 참조.
- **USER_ANIMATION_ART_APPROVAL_PENDING / NO_UNITY_INTEGRATION**: 움직임 미술 승인과 Codex 실제 Unity 재생 검증은 아직 하지 않았다. Attack/Hit/Defeat·다른 방향·몬스터 양산 금지. 승인된 기존 1포즈 원본·F1/마을·Chapter1·2 아트·Unity/Save 변경0. `main` 직접 Push 금지.
- 후임: 바울 대표가 GIF/APNG 및 128px 확인 후 미술 승인하면 별도 기술 검증을 Codex에 의뢰. 신규 수정은 문제 프레임만 별도 승인받아 진행.

## 2026-10-20 서리들쥐 A 전투 3동작 v1 실제 납품

- 기획 발주 `Chapter3_FrostFieldMouse_AttackHitDefeat_3Strip_ProductionOrder_20261020.md`와 Codex 계약 `ad9e4b46` 기준, **Attack/Hit/Defeat PNG 시트3** 새 제작. 2048×512 RGBA, 각 4×512, 8FPS, PPU512, Pivot (0.5,0), 우향 1종, VisualScale1. Attack/Hit 후 기존 Idle 복귀, Defeat frame03 유지(미리보기 GIF/APNG은 동작 확인용 반복일 뿐 게임 loop 의미 아님).
- 실제 ZIP: `Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_v1.zip`; SHA256 `24ef8dddf76fb5843e8e7d3fac05942f1450c9f39cf366b2422a0ab82cba6741`, 8,688,417bytes. 실제 PNG/ZIP은 본 대화 첨부 다운로드 제공, GitHub 문서에는 SHA 및 QA만 기록.
- PNG SHA: Attack `08ed9ffb3d2807b0e1a3b27450f8f3bf1ff3e56f5f33e8b6607b2983db4e402e`, Hit `37c51117ae18f2fed2bda5a8cef6af3099812b5e0979e1368634172937568f1b`, Defeat `bda2809d177d03088e85389ecea23815b92962d61c36cc68967cd0015c96dfcc`.
- 각 프레임 확인판·128px 전체 비교·8FPS APNG/GIF·Manifest/OfflineQA/SHA 동봉. 프레임4개 각각 상이, 셀 크기/투명 가장자리/발 접지선/ZIP CRC 오프라인 PASS.
- **주의/미술승인 보류**: 새 전투 포즈의 털 색이 승인된 차가운 회백색보다 갈색이므로 사용자 시각 확인 필요. 픽셀아트 디테일과 Unity 실표시는 미검증. **USER_COMBAT_ART_APPROVAL_PENDING / NO_UNITY_IMPORT**.
- 기록: [Chapter3_FrostFieldMouse_Combat_3Strips_v1_ArtDelivery_QA_20261020.md](../../Chapter3_FrostFieldMouse_Combat_3Strips_v1_ArtDelivery_QA_20261020.md), 커밋 `74d918f189ce67ea2ee7118b9dfc710d2b506264`. 기존 공식 서리들쥐 Idle·1포즈 원본, 설빙의 폭군, F1·마을, Unity/Save 등 전부 변경0. `main` 직접 반영 금지.
- 다음: 대표님께 사용 가능한 ZIP 다운로드 확인 → 미술 승인/수정 지시 → Codex 격리 Unity QA → 별도 승인 후 게임 정식 통합. Attack/Hit/Defeat 외 양산 금지.

## 2026-10-20 서리들쥐 A 전투 3 Strip v1 미술 FAIL → 표적 수정 v2

- Codex 이전 v1 QA: **36 PASS/0 FAIL 기술**, 미술 Attack FAIL / Hit FAIL / Defeat HOLD / 승인 Idle의 털색 불일치 FAIL. 사용자 발주: `docs/planning/문서/00_프로젝트/Chapter3_FrostFieldMouse_Combat_v1_VisualFail_3Strip_TargetedFix_20261020.md` 및 Issue #30.
- 실 납품 v2 ZIP: `Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2.zip`, **SHA256 `518d210a0e5b29c7214ad960c0baa3d23a88d13d4f789b5f7420982ce701bfe3`**, 10,784,249 bytes, 20 files / testzip CRC PASS. ChatGPT 대화 첨부에 실물 납품, **GitHub에 PNG/ZIP 미업로드**.
- 신규 PNG3 SHA: Attack `c83e35b5aebbce908b6025d5a071fafc463cfe301763f68806ecddb8bd4f576b`, Hit `695ed7db1f794c3f71df1b058eb5e91f01de7ecb125369264f3624b36c9fd3c2`, Defeat `3f6a52039b766c57a5367c7145e0234b68ac0a552940de1e88ffe8f2404f3395`. 모두 2048×512 RGBA, 4×512cell, 8FPS, PPU512, Pivot(0.5,0), Scale1.
- 수정: **Attack00과 Hit03 RGBA 전체 셀이 승인 Idle00 픽셀과 동일**, bbox [22,124,490,490]. **Attack02 최대 앞발 할퀴기** (v1 Attack01의 강한 공격을 올바른 이벤트 타이밍 위치로 이동). 중간 프레임 털 부위 냉회백 톤/명암 국소 보정, 동작 특성 유지. **Defeat03 알파 실루엣 v1과 정확 동일**, RGB는 털색 정합 목적의 부분 변경만.
- ZIP에 프레임별 컨택트 시트·8FPS APNG/GIF·Idle→Attack/Hit 8FPS 전환 프리뷰·112px v1/v2/Idle 비교·Attack02/Defeat03 확대·128px 전체 비교·Manifest·QA JSON·SHA 제공.
- 제작 QA 문서: [Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2_ArtDelivery_QA_20261020.md](../../Chapter3_FrostFieldMouse_Combat_3Strips_Fix_v2_ArtDelivery_QA_20261020.md), 커밋 **`3833a3262dd9966344c7ee9f946a896b37b39d6f`**.
- **상태:** `ART_FIX_V2_DELIVERED / CODEX_DELTA_QA_PENDING / USER_COMBAT_ART_APPROVAL_PENDING`. 이전 36개 QA 전수 재검사 금지, 새 이미지 3종만 경계 프레임/Attack02 타격/Defeat03 HOLD/색상 불연속 최소 실제 Unity 검증 필요. 승인된 Idle/1포즈/보스 B/F1·서릿길 마을/Chapter1·2 및 Unity/Save/GUID 원본은 보호. **main 직접 Push/병합 없음**.
- 후임: 이 ZIP과 승인 Idle ZIP 두 실물 확보 → Codex 변경분 QA 결과 확인 → 사용자 미술 승인 또는 표적 수정 후 새 버전. 다른 캐릭터/몬스터 애니메이션 무단 확대 제작 금지.

## 2026-10-21 Chapter3 Main22 첫 플레이 F1 환경 6종 시각 검토

- **기획 우선순위 재잠금:** 서리들쥐 A Combat Fix v2는 바울 대표 **미술 승인 완료**, Codex **23PASS/0FAIL** Delta QA. 실제 게임 몬스터 연결 완료 보고는 별도 대기. **Main22 F1 첫 플레이**가 최우선, 설빙의 폭군 B 후반 보스 애니메이션/마을 NPC/F2~F5는 보류. 서리폭군은 잘못된 표기, 공식 보스 이름은 **설빙의 폭군**.
- 실물 기준 `Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip` 중 **F1 PNG6** 모두 실제 열어 확인. 각 256×256 RGBA; 원본 PNG 바이트를 원본 ZIP과 SHA 대조, 6/6 완전 동일. Chapter2 공식 `DeepCore_Ground_Base.png`, `DeepCore_Cliff_Boundary.png` 2종 참조로 질감·명도만 비교(서로 다른 바이옴/다른 소스 크기).
- 원본 미술 진단: 흙·잔디 평면적, 서리가 지나치게 전면 회색, 전환부 점무늬가 소재보다 노이즈, **길 오버레이 통로 가독성 부족**. 관목/표지판은 이번 수정 대상 제외해 보존. 6종 모두 **F1 사용자 미술 최종 승인 전: HOLD**.
- 새 **3종 표적 수정 샘플**(흙 지면/절제된 얇은 서리/전환부 오버레이)은 별도 후보 PNG로 제작, 기존 6개를 전혀 덮어쓰지 않음. 128px 전후 비교/오프라인 길 연결 시안/Chapter2 공식 아트 참조/2×2 기본 지면 Seam QA 포함. 후보 미술완성도 PASS나 Unity GameView 주장 금지.
- 실제 ChatGPT 다운로드 ZIP: **`Limitless_Chapter3_NorthF1_ArtReview_Samples_v1.zip`**, SHA256 **`dd9963d2a2ce442ad12ecccc8c3ae74edbfb7421eb838003d21ae2df767bd086`**, 약 1.15MB, 원본6+수정후보3+Preview4+QA3+Manifest1+SHA 목록1 = 18파일. GitHub에는 바이너리 미등록, 해당 대화에서 받는 실물임.
- 제작·원본 SHA·검증·한계 문서 [Chapter3_F1_Original6_ArtReview_TargetedGroundSamples_20261021.md](../../Chapter3_F1_Original6_ArtReview_TargetedGroundSamples_20261021.md), 커밋 **`929eff1f668cbcbea1fdd97475aef75741a10bfd`**.
- **승인/기술 게이트:** 대표가 초기 서리 밀도/흙 질감/전환부 시안을 검토해 준 뒤 좁고 불명확한 길의 **최소 수정** 범위를 확정; Codex의 진짜 F1 PPU/Pivot/TileGrid/Collider와 휠체어 통로 조사 및 실제 Unity F1 구현/Play Mode는 이후 단계. 이번에는 2×2 Seam·PNG RGBA·ZIP CRC 등 오프라인 QA만. `main`/Unity/Save/GUID/Monster 승인원본 및 다른 담당 Chapter1·2 리뉴얼 수정0.
- 새 세션 재개 시 가장 먼저 Issue #30 및 `docs/planning/문서/00_프로젝트/Chapter3_20261021_F1_Main22_FirstPlayable_Priority_And_BossDefer.md`를 다시 읽고 F1 3종 후보 미술 의견 확인할 것. 설빙의 폭군 전투 애니메이션 선행 제작 금지.

## 2026-10-21 F1 환경 미술 HOLD — 신규 6종 개선 v2 실물 납품

- [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30) 및 최신 기획/코덱스 F1 미술 HOLD에 대응. F1 **흙/얇은 서리/단방향 전환/흙길/서리 관목/길 표지판** 각각 신규 후보 1종씩 **256×256 RGBA PNG6** 제작. 이전 F1 원본6 바이트 유지, 기존 지면 시안3 별도 보존. Main22 우선, 마을·F2~F5·설빙의 폭군·신규 전투 아트 제작 안 함.
- 실물 ZIP: `Limitless_Chapter3_NorthF1_Environment_6_ArtImprovement_v2.zip`, **SHA256 `8158f501763b525022b8b9fc1645fe2b0a5c0bc8bf9f73aabcb02024c3a3235c`**, 3,897,421 bytes, 내부23파일 (후보6·원본6·Preview4·반복QA4·JSON1·Manifest1·SHA목록1). ChatGPT 대화 다운로드 첨부; GitHub에는 **PNG/ZIP 바이너리를 올리지 않음**.
- **QA 오프라인 PASS**: 파일 디코딩/규격6, 원본6 바이트 SHA 일치, Ground/ThinFrost 첫/끝 변두리 X/Y 픽셀 일치, Ground2의 2×2 및 3×3 반복 QA, ZIP CRC. `Preview/F1_Earth_Frost_Path_Composite_OFFLINE.png`은 Unity 화면이 아니라 PIL 합성.
- **미술 승인 대기**: 그림은 1254 고해상도 원화에 기반한 256 후보이므로 수동 픽셀 마감/실 게임 적용 보장 아님. 큰 반복무늬는 3×3에 보일 수 있음. 단방향 오버레이·보행 통로의 실제 통행 폭/PPU/Collider/TileGrid·GameView는 Codex 실측 대기. **USER_ART_REVIEW_PENDING / NOT_UNITY_INTEGRATED**.
- [제작·QA 보고](../../Chapter3_NorthF1_Environment_6_ArtImprovement_v2_QA_20261021.md), `docs/art-new` commit `4821d6fec8180a266a1580dc4eb175f6b6145474`. **main 직접 커밋/Push·병합 없음**.
- 후임: 새 ZIP의 실물 6 PNG/비교판을 확인해 대표 미술 판단 먼저 받기 → 필요 시 그 지적 파일만 표적 수정 → Codex 기술 계약/Play Mode F1 검증 후 Unity 통합 요청. 기존 공식 캐릭터·서리들쥐 전투·설빙의 폭군·Chapter1/2 리뉴얼 보호.

## 2026-10-21 북부 F1 v2 부분 합격 → 3종만 v3 표적 보완

- Issue #30의 Codex v2 격리 QA: **기술 PASS / 미술 3 PASS, 2 HOLD, 1 FAIL**. 대표님 지시대로 KEEP 3 **흙 지면/흙길/서리 관목**의 바이트·SHA 절대 보존, Thin Frost/Transition/Waymarker **3종만** v3 PNG 보완.
- 실제 납품: **`Limitless_Chapter3_NorthF1_Environment_6_TargetedFix_v3.zip`**, SHA256 **`488a65c051d1d017787771e0169124e47437b472911caa9949fdd59e55079054`**, 5,524,917 bytes, **20파일**. `Art/` 후보6 PNG는 모두 256×256 RGBA, PASS v2 PNG 3개는 각각 SHA `ccf2c22e90bee292ecf2d0d030dad53e0032457272ebb94fccdf6491fabcf8b9`, `e78b41d48707dfce6a79dd4e826f864c5c2608b45a0f457af414ee9768a20175`, `6c83c88881e7f89245ed580d50424b2671629ad9491da93f247aab99bfe4746e`로 완전 동일.
- 수정 v3 SHA: Thin Frost `12fe03d156944e1b72aec4e807770ee8016c61fe1de5804366819e716b97c30c`, Transition `93126dd1cda0f3b97823ae4756344600ed7f6af619eb42013d1ec45a4933c4d1`, Waymarker `6e55b64dbc2e906a50cf8b7310849bb156a90bc40436f28a86832e59e232b589`.
- v3 변경: 큰 반복 서리 얼룩의 밝기 감소(기술 참고 색차>28 픽셀 4358→1569), 흙 바탕+우향 알파 오버레이→얇은 서리 오른쪽 타일 **양쪽 경계 최대 RGBA delta0**, 표지판은 한쪽 평평한 꼬리/오른쪽 한쪽 화살촉. **오른쪽은 PNG 표시 기본 방향이지 Main22 실제 길 안내 방향이나 목적지를 확정한 것이 아님.**
- 실제 ZIP/PNG는 이 ChatGPT 대화에서 다운로드 제공; GitHub에는 QA/Manifest/해시 **문서만** 기록하고 바이너리 미업로드. `docs/art-new` 제작 QA [Chapter3_NorthF1_Environment_6_TargetedFix_v3_QA_20261021.md](../../Chapter3_NorthF1_Environment_6_TargetedFix_v3_QA_20261021.md), 커밋 `b18d06fbbf318cfeff085a86aeb198b8b3aafe95`.
- **오프라인 PASS / 미술·Unity 미승인**: 2×2/3×3 반복, 우측 연결, 표지판 128px 비교, ZIP CRC. 이미지 합성은 Unity GameView가 아님. Single-tile 반복 특징 잔존 가능/4방향 오토타일 아님/PPU·TileGrid·Collider·휠체어 통행폭 및 실제 F1 Play Mode 미검증. 현재 F1 전체 미술 상태 계속 **HOLD**.
- 다음: Codex에 **이번 변경3+KEEP3 SHA만 격리 Delta QA** 요청 → 바울 대표 최종 미술 승인 → 별도 승인 후 Unity Main22 첫 플레이 구현. 다른 F2~F5·마을·설빙의 폭군·승인 서리들쥐·Chapter1/2 원본 보존, `main` 직접 반영 금지.

## 2026-10-21 Main23 서릿길 마을 v3 — 개별 게임 아트 후보 43종

- 이번에는 ArtPilot v2 11장 콘셉트를 다시 포장한 것이 아니라 **독립 Sprite PNG 43종** 제작: Tile4 (256 RGBA/상하좌우 경계 일치), 전환/길 경계2, 독립 건물5, 지붕·벽·문 모듈9, 생활소품14, 상점/방향 표식6, 비작동 포탈 석재3. 2×2·3×3 반복QA8·16:9/4:3/21:9 오프라인 배치3·자산 미리보기1, Manifest/QA/SHA4 = ZIP 총59파일.
- 실제 사용자 ZIP: `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_v3.zip`, 최종 ZIP SHA **`2a74677d5f62d7e13e64ea85ba62e12175be0be0e0d19864c23233f1bd84e79a`**, 17,311,281 bytes; ChatGPT 대화 다운로드에 실제 파일 존재. 브랜치에는 **아트 바이너리 없이 보고 문서만** 기록함.
- SHA mismatch 감사: 이 작업공간의 v2 ZIP 해시 `2142fe641c64a2ead8ad37309258d49727f35e5aaea74e429cbd6bae546154ad`는 기존 기록과 일치, 19 ZIP entry/CRC PASS·Manifest 기준 15 PNG SHA 일치. Codex 대표님 F: 실물에서 측정한 **`19dfad3cc6f7e27b03e1dcfc194cdb1bf56d9e133b48f303288eff98886833d`**는 다른 사본. 두 번째 사본 실물 미접근이므로 **차이 원인 미확정**, Codex ZIP 엔트리·PNG SHA 대조 필요. 신규 v3 ZIP은 작성 후 바이트 해시 기록했으며 최종 전달 파일과 동일.
- 오프라인 PASS: PNG 디코딩/독립 RGBA/전부 외곽에 투명 여백/지면4종 edge pixel match/2×2·3×3 자료/ZIP CRC·SHA. 최초 지면 외곽의 검은 선 결함은 최종본에서 제거 후 재검증. 오프라인 마을 배치 PNG는 **Unity GameView 아님**.
- 제한: 건물 지붕·벽·문 모듈 간 실사용 접합/Unity PPU·Pivot·Collider·실제 휠체어 3WU 통행/포탈 비작동·캐릭터와의 스타일 조화·Sorting 미검증. 미술 **ART_CANDIDATE / REPRESENTATIVE_APPROVAL_PENDING**, 정식 GameReady PASS 주장 금지. Portal은 돌기단만 있고 VFX/연결 기능 없음.
- [GitHub 보고](../../Chapter3_Main23_FrostwayVillage_GameArt_v3_ActualDelivery_QA_20261021.md) 커밋 `b7ddd71895288a077c55fdb3e0a91375ab43f604`. Main23 기획 정본 및 Issue #45를 우선 확인하고, Codex 변경분 검수/대표 승인 후에만 실제 Unity 적용. `docs/art-new`만 사용, **main 직접 Commit/Push·병합 금지**. F1 원본6·기존 공식 인물/몬스터·Chapter1/2 아트/Unity/Save 보호.

## 2026-10-21 Main23 Frostway Village v4 — 두 PNG 분리잔여 제거

- Codex [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45) v3 DeltaQA: **PNG43/CRC/Manifest 기술 PASS, 격리 Unity 3화면비 조립·통행 QA 완료**, 다만 두 PNG의 다른 원화 조각 때문에 미술 FAIL, Tile4의 3×3 반복무늬 미술 HOLD. Main23 정식 씬/퀘스트 구현 완료 상태 아님.
- **v4 표적 수정 완료**: `Buildings/Frostway_SkillShop_Building.png` x12..28/y196..376 범위의 **별도 연결조각 2496픽셀**, `Buildings/Modular/Frostway_Roof_Gable.png` x100..144/y183..206의 **별도 조각 712픽셀**만 삭제. 사각형 일괄 마스킹 아닌 alpha connected-component 제거. **두 Sprite 본체 RGBA 픽셀은 전부 v3와 동일**.
- 변경 PNG v4 SHA: SkillShop `ef34b49de4f807debf4a69e1947eb9b8e02a543ee117ca2867fe3aa06a5efb17` / Roof Gable `37ea6a69844b662c40583cea3d457ff0c47344aef7054ff8e32b4d09870dceb0`. v3 원본 SHA 각각 `7e3562c22f00a07a85f0ab0349b8119dff924f3ef4dccf199cccdc9ec367be89` / `4971eea0e473ec7c7c1579001a454be52417fd9f3f38b9e223b00e0deaaf71ec`.
- **기존 게임 아트 나머지41 PNG의 원본 ZIP entry 바이트/SHA 41/41 동일**, 특히 Tiles4 PNG 모두 수정0. 새 3×3 Pattern HOLD 표시 자료 5 PNG 추가. Earth: x9..94,y120..252 풀/돌; Gravel: x23..130,y26..120 자갈; ThinFrost: x9..94,y120..252; Stone Path: x60..215,y94..240 특정 포석 배열. 2×2/3×3 경계 기술 PASS와 시각 반복 HOLD를 분리.
- 실제 ZIP `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v4.zip`, **SHA256 `ee25baef91ec740b85dd3bb62b32583292d703f9261863bd534d0eb8241d1896`**, **19,767,770 bytes / 70 entries**. 원본게임 PNG43(변경2, 보존41), 기존 반복·화면비 오프라인 참고12, 신규 before-after 6, 신규 TileHold5, Manifest/QA/SHA4. 압축 CRC/SHA/PNG 43 정상. 실물은 ChatGPT 다운로드 첨부, GitHub에는 문서만 기록.
- **QA 문서** [Chapter3_Main23_FrostwayVillage_GameArt_TargetedFix_v4_QA_20261021.md](../../Chapter3_Main23_FrostwayVillage_GameArt_TargetedFix_v4_QA_20261021.md), 커밋 `cbacca86c6a69faf3240e8fc09971a3b2fdceb6a`.
- **승인 게이트:** v4 Codex 격리 변경분 Unity QA + 바울 대표 미술 승인 대기. 타일 패턴 때문에 전체 미술 HOLD 지속. 이전 Preview/ 의 16:9/4:3/21:9는 v3에서 생성된 **오프라인 참조이미지**로서 v4 Unity GameView 아님. Main23 Scene/Save/Unity/GUID, 공식 F1·Wolf·이전 v2 콘셉트11, Chapter1·2 변경0. `docs/art-new`만 문서 커밋, `main` 직접 Commit/Push/병합 금지.
- 후임 세션은 해당 문서+ZIP/Issue #45 재확인→Codex 변경분 QA 확인→대표 승인 여부 확인. Tiles4는 대표 허가 전 수정하지 말고 임의 전체 타일 재제작 금지.

## 2026-10-21 Main23 서릿길 마을 타일 반복 표적완화 v5

- 인수 즉시 `docs/art-new` 이 문서 + `docs/planning`의 `PLANNING_LATEST.md` 최신(2026-10-21 늑대 Run6 v5 승인 / Village v4 HOLD) + Issue #45 최신 Codex 결과 참조. 늑대 Run6 v5는 이미 바울 대표 **모션 승인**이지만 Main23 마을은 **미술 HOLD**.
- v4 원본 ZIP `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v4.zip` SHA `ee25baef91ec740b85dd3bb62b32583292d703f9261863bd534d0eb8241d1896` 이 작업환경에서 다시 확인; 사용자 PC `F:\Downloads\`의 파일은 직접 다루지 않음.
- **v5 실제 생성·납품**: `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_TargetedFix_v5.zip`, SHA256 **`1affc1a4c23f3bcc6a9e9e18924b08bce3134eab065d8362757725f96e7f71b4`**, 20,085,423 bytes, **72 entries**. 게임용 PNG 43 중 **수정3/바이트 동일40**. Tile Earth(24062 pixels), ThinFrost(23973), Gravel(31120) **국소 텍스처 반복 모티프 완화**. Tile StonePath·SkillShop·RoofGable v4 PASS 등 기타40 byte SHA 동일.
- **오프라인 QA PASS**: 전부 PNG RGBA/타일 256×256/수정 타일 주변 RGBA edge 보존 + X/Y 첫/끝 edge 일치, 40 byte-identical, SHA256SUMS 71/71, ZIP CRC. Tile4의 v4↔v5 2×2·3×3 비교20 PNG/128px 원본 크기 미리보기와 독립 v5 실 PNG 사용 3 화면비 1600×900·1200×900·2100×900 **오프라인 합성**. 반복 무늬는 약화 시안이지 최종미술 합격 보증 아님.
- **OPEN16 주의**: GitHub 기획/Issue에 기존 Codex CSV 기반 OPEN 16개라는 **집계**는 있으나 **행별 CSV 원문 파일명·좌표·조건을 해당 소스 ZIP·원격 담당자료·유저 Library에서 확보하지 못함**. 따라서 개별 OPEN16 해결0 / 종결0, 원본 검수 CSV 요청 필요. 어떤 건물/소품의 미확인 결함도 임의 수정하지 않음.
- 상세 QA·실물 SHA/기타 제한: [Chapter3_Main23_FrostwayVillage_GameArt_TargetedFix_v5_QA_20261021.md](../../Chapter3_Main23_FrostwayVillage_GameArt_TargetedFix_v5_QA_20261021.md), `docs/art-new` 커밋 `006fc8f4edfa00943052a00e4ace8217e11306fc`. **실 PNG/ZIP은 ChatGPT 대화 다운로드 첨부**이며 GitHub는 문서/해시만, Unity/Editor/Scene/Save/Main/F1/Wolf/다른 아트 변경하지 않음.
- 다음: Codex 기존 16 CSV **행별 원본 공유** → 중복/이전 v4 해결 여부 검증 → 해당 OPEN만 표적 추가 수정. v5 3 Tile는 Codex 격리 실3×3/UI DeltaQA와 바울 대표 최종 미술 승인 필요. `main` 직접 커밋/Push/병합 금지.

## 2026-10-22 Main23 서릿길 마을 VariantTiles v6 — 지면별 02·03 6종 신규

- 바울 대표 명시 발주: 단일 Base를 계속 수술하는 방식 대신 Earth·ThinFrost·Gravel 각각 **01 보존 + 02·03 신규**. 사용자 ZIP v5 실제 SHA `1affc1a4c23f3bcc6a9e9e18924b08bce3134eab065d8362757725f96e7f71b4` 입력 검증.
- 실물 납품 `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v6.zip`, **SHA256 `519ed96fa6157b7de451830412d6e881b633241b0f8cc459d186465765a4ea7e`**, **17,658,178bytes / 71 entries**. 실제 ZIP은 ChatGPT 대화 다운로드 첨부; GitHub에는 문서만 커밋하고 바이너리/Unity로 자동 복사하지 않음.
- 게임 PNG **기존 43/43 byte-identical SHA 유지 + 신규6 = 49**. 신규 `Tiles/Frostway_Ground_Earth_02/03.png`, `Tiles/Frostway_Ground_ThinFrost_02/03.png`, `Tiles/Frostway_Ground_Gravel_02/03.png`; 모두 256×256 RGBA, 완전 불투명. 새 아트는 원본 특징 돌/풀/서리 실루엣의 **비왜곡 국소 패치 교환**으로 원본 팔레트·접지 질감 유지, 01 3종 원형.
- 01/02/03 타일 각각 외곽12px 원본 완전 보존. 동일 지면 family 내부 3×3 ordered 이웃쌍 × 세로·가로 모두 max RGBA edge delta **0**. 독립 시트 PNG/ZIP CRC/70 SHA 목록 검증 PASS.
- **3×3·5×5** 01 단독 비교와 신규 01/02/03 **혼합 오프라인 Preview**, 5×5 모든 수평/수직 같은 variant 이웃 **0쌍**으로 구성, 실제 Unity TileGrid/화면/통행 검증은 아직 수행하지 않음. 공통 배경 질감 일부 주기성은 남아 **미술 ART_HOLD 유지** 및 대표 최종 승인 대기. `Frostway_Path_Stone_01`과 SkillShop·Roof Gable 기존 승인 수정·문·지붕·포탈 등 모든 기존 43 원본 PNG byte 보존.
- **OPEN16**: Codex가 이전 CSV/좌표JSON 실물을 로컬 확보했다는 Issue #45 최신 보고 확인. 그러나 **행별 원문 파일·좌표·재현조건은 현재 아트 세션 및 공개 Issue에서 열람 불가**. v6는 신규변형6만 제작해 기존 원본 OPEN16 **해결0 / 현재16**. 행별 근거가 없으므로 타일 관련 항목과의 정확한 매핑·타 담당 기존 PASS 항목 수정 금지. 증거 문서 공유 후 후속 선별수정.
- 제작·QA 상세 [Chapter3_Main23_FrostwayVillage_GameArt_VariantTiles_v6_Art_QA_20261022.md](../../Chapter3_Main23_FrostwayVillage_GameArt_VariantTiles_v6_Art_QA_20261022.md), 커밋 `baeaee947f44233a4e50e6456dc3725c4f7e958d`.
- **후임**: Issue #45, docs/planning 최신 정본 → ZIP 실제 readback SHA 확인 → Codex 격리 5×5 01/02/03 섞은 실제 Unity 3비율·통행 DeltaQA → 대표 미술 승인 판단. 본게임 Main23/원본 Unity/Save/UserData/F1/승인 Wolf/Bear/Fox/main 변경 금지. `main` 직접 Commit·Push·Merge 금지.

## 2026-10-22 Main23 Frostway GameArt v7 — 기존43 보호, 변형02/03 모티프 재설계

- 바울 대표 v7 별도 승인 발주와 Issue #45 최신 Codex v6 격리 검수: 43PNG+신규6 ZIP 기술 PASS·Unity 3화면비/통행6/6 PASS이지만 Earth/ThinFrost/Gravel 01/02/03 혼합 5×5 모티프 유사성으로 미술 HOLD. 개별 OPEN16 원본 CSV/좌표JSON은 Codex 로컬 회수 보고만 존재, art-new가 직접 열람할 원문 없음.
- 소스 ZIP `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v6.zip` SHA `519ed96fa6157b7de451830412d6e881b633241b0f8cc459d186465765a4ea7e`. 최종 v7 실물 ZIP `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v7.zip`, **SHA256 `d499f08481751c0fcda3298bbd37513466168b73105dd224b2cb767651bb691c`**, 30,503,515 bytes / ZIP 멤버 **88**, CRC·내부 SHA256SUMS 87/87 PASS.
- **43 PNG 기존 v6 바이너리 43/43 완전 동일** (01 타일3·SkillShop/Gable/StonePath/포탈 등). **02·03 변형6 PNG만 변경**, 파일명 그대로. 256×256 RGBA/alpha255. 다른 PNG나 자동 배치·Overlay 추가 없음.
- 변형 미술 방향: 원본 미세 입자의 **공간 주파수/색 범위**를 참조한 새 독립 바탕 질감과 서로 다른 **각진 파쇄암·토양 미세균열·서리결정 띠·비대칭 자갈 퇴적/풀 실루엣** 사용. 같은 원본 대형 돌/풀 픽셀패치 위치만 바꾸는 v6 방법에서 전환.
- **오프라인 QA**: 동일 지형 내 01/02/03 가로9/세로9 이웃조합(18/family) 모든 max edge RGBA delta **0**; v6/v7 동일 Tile ID 랜덤배치 **3×3(원본256/cell)·5×5·8×8(표시128/cell)** 전후 비교, 01 단독 반복/실제 셀 접합/모티프 중심2× 확대, SHA/Pillow PNG PASS. v6 대비 변형 SSIM 크게 감소. **주의**: 01은 기존 미술 보존 때문에 v7 02/03과 질감이 차이나 큰 사각 구획이 느껴질 수 있음. 실제 Unity 시각 반복/타일 그리드 은폐 **미술 승인 미완**.
- GitHub [제작·QA 문서](../../Chapter3_Main23_FrostwayVillage_GameArt_VariantTiles_v7_Art_QA_20261022.md), 커밋 `e641438cf2010164c2ec83298bab7e5709504004`. **실제 PNG/ZIP은 ChatGPT 현재 대화 다운로드 첨부만**, GitHub에 바이너리를 올리지 않음.
- **OPEN16 해결0/미해결16**: 로컬 Codex가 원본 CSV/좌표 JSON 확보했지만 파일별 16행 원문이 art-new에게 공유되지 않음. 현재 원본 43 자산 모두 보존했으므로 OPEN16이 사라졌다고 주장 금지. 다음 세션/담당은 Codex 증거 전달→기획/아트 항목 분류→근거에 따라 해당 자산만 추가 수정.
- **후속:** Codex 격리 Unity 16:9/4:3/21:9 실제 TileGrid에 v7 5×5/8×8 설정·통행 proxy/접합/격자 판단 후 대표 미술 승인. v7 상태 `CANDIDATES_DELIVERED / OFFLINE_QA_PASS / ART_HOLD_REVIEW_PENDING / NOT_INTEGRATED`. 본게임 Main23, 승인 F1/Wolf, Bear/Fox/다른 챕터, Save/UserData 변경 없음. `main` 직접 commit/push/merge 금지.

## 2026-10-22 Main23 Frostway VariantTiles v8 — texture harmonization

- **2026-10-22 15:08 KST 지시**, Codex v7 실Unity 5×5/8×8/3화면비·통행 기술 PASS지만 Earth/ThinFrost/Gravel 01·02/03 미세입자/중간 명암·지면 바탕 질감 차이로 **ART_HOLD**. 01/기존43 PNG 바이너리 보호한 채 **변형 02·03 여섯장만 질감 정합**, 추가 돌·풀·서리 모티프/타일 자동배치 생성 금지.
- 소스 v7 ZIP `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v7.zip` 실제 SHA256 `d499f08481751c0fcda3298bbd37513466168b73105dd224b2cb767651bb691c`.
- 실제 v8 ZIP `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v8.zip` SHA256 **`6a9fa1c3c0c1ffef3ac43d3541a28bbc0a16310d2e0f8c267ec7177d731d712f`**, 40,921,124 bytes, **98 entries**, 내부 `SHA256SUMS.txt` 97/97 + ZIP CRC PASS. **게임 PNG49 = 수정6 + 원본 byte-identical43**. 실제 ZIP은 ChatGPT 대화 다운로드 첨부에 있으며 GitHub에는 바이너리 등록하지 않음.
- 수정 정확6: `Tiles/Frostway_Ground_Earth_02/03.png`, `ThinFrost_02/03.png`, `Gravel_02/03.png`. 256×256 RGBA alpha 그대로. **새 모티프 없이 v7의 형상/위치를 보존**하며 Gaussian 대역별 미세/중간·저중주파 대비와 바탕 명암을 소스01과 조화 (1.1/3.9/11.8px). 01 각3·StonePath·SkillShop/Gable·Portal/건물 등 기존43 파일 **바이트 동일**.
- 3 family 각 01→02→03 ordered 조합 **54 seam 사례 edge RGBA maxdiff0**. 3×3 원본256/cell, 5×5·8×8 128/cell 동배치 v7-v8 오프라인 비교, 01 8×8 단독 및 1:1 모티프/사각 구획 검사. **실제 Unity GameView 아님.** 통계거리 Earth 02 6.96→1.06 /03 7.21→1.05; ThinFrost02 5.74→1.13 /03 6.05→1.17; Gravel02 6.77→0.91 /03 6.65→0.76. 이 숫자는 **입자/명암 통계 정합**이지 미술 PASS 자동 판정 아님.
- **남는 위험:** 01의 큰 풀·돌 실루엣과 02/03 작은 모티프 체급은 유지되어 큰 구획이 남을 수 있음. 01 변경 금지 때문에 완전 해결이 어려우면 Codex가 신규 얇은 Overlay/비정규 변형 배치의 **명세·Sorting/PPU/타일 경계·랜덤 정책**을 먼저 확정한 후 시안 검토. v8에서 새 Overlay PNG/TileGrid 임의 구현하지 않음.
- **OPEN16 해결0/미해결16**: Codex가 CSV·좌표JSON 로컬 확보 보고만 공유. 원문 16행은 아트 담당 읽기 불가. 파일/좌표/심각도를 추정해 다른 자산 수정하지 않음.
- [제작·오프라인 QA 정본](../../Chapter3_Main23_FrostwayVillage_GameArt_VariantTiles_v8_Art_QA_20261022.md) **`docs/art-new` commit `391bb582dec0680858e56f8d009b2e595322559a`**. 현 상태 `OFFLINE_QA_PASS / CODEX_V8_DELTA_QA_PENDING / USER_ART_APPROVAL_PENDING / VILLAGE_ART_HOLD`. **main 직접 commit/push/merge 0**, 본게임 Unity Scene/Save/UserData 및 F1/Wolf/Bear/Fox 등 승인자산 변경0.
- 다음: v7·v8 ZIP SHA 확인 → Codex 격리 실제 Unity 16:9/4:3/21:9 **5×5/8×8 동일 Tile ID·카메라 동배율** 시각/통행 DeltaQA → 바울 대표 최종 미술 승인. OPEN16 CSV 원문 공유 후 관련 항목만 후속 결정. 세션 포화 전에 문서 우선 최신화.
