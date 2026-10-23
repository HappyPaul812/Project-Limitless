# LIMITLESS / Main23 신규 아트 #59 / Codex #64·#65 후속 최소 보완 검토 (2026-10-23)

## 0. 결론·상태

- **현재 상태:** `ART_CONTRACT_WAITING_CODEX_EVIDENCE / ART_CANDIDATE / USER_REVIEW_PENDING / NOT_UNITY_APPLIED`.
- **실제 새 게임용 아트 PNG: 0개. 기존 게임용 또는 후보 PNG 수정: 0개.** 별도로 새롭게 만든 것은 **오프라인 합성 비교판 PNG 3개**, 증거 QA CSV·Manifest·설명·SHA 목록과 ZIP 한 개뿐이다.
- **사용자 최종 미술 승인: 없음. Unity 정식 적용 / main Push·병합: 없음.**
- **범위 판정:** ① 오르나 원화 KEEP, 장면 표시 크기 후보 검토(40/80/96/128px 보드) ② 기존 Residence 지붕 KEEP + 신 RoofGable·RoofTrim 배치 제외, EntryDoor만 접합 후보 ③ 기존 여관 내장 간판 KEEP, 신규 독립 간판 CANDIDATE_HOLD.

## 1. 근거 출처와 신뢰수준

- [기획 #59 최신 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/59#issuecomment-6358022168): #65 실 Unity A/B 수령 전에는 `ART_CONTRACT_WAITING_CODEX_EVIDENCE`로 표기, 불필요한 추가 PNG 생산 금지.
- [Codex #64 보고 접수](https://github.com/HappyPaul812/Project-Limitless/issues/64#issuecomment-6357901621): 사용자 전달 **로컬 Codex 결과 요약** 기준 신규 Unity 41PASS / 비교캡처 15PASS; 오르나 TECH_PASS이지만 40px가 작음, 남문 겹지붕 ART_HOLD, 여관 신규 독립 간판 렌더 TECH_PASS이지만 기존 내장 간판과 중복, KEEP_OLD 권고. **이 숫자는 본 회차 신작 오프라인 보드의 Unity 테스트 결과가 아님**.
- [Codex #65 후속 발주](https://github.com/HappyPaul812/Project-Limitless/issues/65): 실제 캡처와 `Main23_Frostway_VisualGapList.csv` 상위3행을 안전하게 공개/전달하고 최소 A/B 격리 시연 요청. **2026-10-23 본 보고 시점 #65 댓글 0건, 공개 비교 캡처 0건**, 원격 제품/QA 커밋 증거 확인되지 않음. 사용자 Windows 로컬 테스트 파일, Scene XY/PPU/Pivot/Sorting/footprint/Collider/3WU·7×7WU 검수 실물은 아트 담당 측 **NOT_RECEIVED / NOT_TESTED**.
- 파일 바이트 출처: 기존 미술 ZIP `Limitless_Main23_Orna_SouthGate_InnSign_ArtPilot_20261023.zip`(SHA256 `425292ebd1e8a58ab7c4486f6fac39491c265d49b5db933dfe7586e69cee7351`). 기존 Residence/Inn/Inn sign은 과거 비파괴 v9 패키지의 게임용 자산 바이트를 읽기만 하고 비교에 이용(`Limitless_Chapter3_FrostwayVillage_Environment_GameArt_VariantTiles_v9.zip`, SHA256 `afea1ed0b357a5bdce9e962c7969b81b77d29e1ed11b7b9dc170d57747fbeb79`). 현재 사용자 로컬 v5 파일과 직접 SHA 대조하거나 Unity Import 검증을 실행한 것이 **아님**.

## 2. 신규 오프라인 비교 ZIP 실물

- **ZIP 파일:** `Limitless_Main23_Issue59_MinimalDelta_Review_20261023.zip`
- **제작본 SHA256:** `2bb31ffaba77f977f426c216b2a1cc9b86934d640c993f4697679b4eb9d67e11`
- **실제 크기:** 812,046 bytes / **7 entries**. ZIP `testzip()` CRC PASS, `SHA256SUMS.txt` 항목 **6/6 PASS**.
- **실제 전달:** ChatGPT 현재 대화의 생성 아티팩트 `/mnt/data/Limitless_Main23_Issue59_MinimalDelta_Review_20261023.zip` 다운로드. **GitHub에 이 보고서 텍스트만 커밋; ZIP/비교 PNG GitHub 바이너리 등록0**. 바울 대표 Windows 다운로드 경로 보유 여부는 확인 전이다.
- ZIP 내용: `Manifest.json`, `README.md`, `QA/SourceAssets_9_SHA256_FROZEN.csv`, `Preview/*.png` 3장, `SHA256SUMS.txt`. 원본 PNG 9경로의 SHA 기준값은 CSV와 Manifest에 있고 원본 바이트는 변경하지 않았다.

| 생성한 비교 이미지(상대경로) | 픽셀 규격 | SHA256 |
|---|---|---|
| `Preview/Orna_40_80_96_128_Height_ABCD_OFFLINE.png` | 1480×590 | `376cd07592168488bef968bc1ccb29ab9bdb9e944cb386c6bab68206bdf4137b` |
| `Preview/SouthGate_Residence_Keep_vs_DoubleRoof_vs_DoorOnly_APPROX_OFFLINE.png` | 1840×680 | `52172ca356109fe4455b7ffab197f4509c72fb6f6d77ea044d1b545f64010009` |
| `Preview/Inn_KeepOriginal_vs_DuplicateStandalone_APPROX_OFFLINE.png` | 1350×655 | `f7e83059ee85bacf6f570169658f841cb17798561cccf7868bc4bb8a2434eff0` |

## 3. 대상별 미술·기술 검토

### 오르나 — `KEEP_ORIGINAL / SIZE_REVIEW`
- `NPC/Orna_VillageGuide_Concept_Master_CANDIDATE.png`, `NPC/Orna_VillageGuide_512_CANDIDATE.png` **원본 SHA/바이트 불변**.
- 동일 512 후보 컷아웃을 시각적으로 **bbox 높이 40px, 80px, 96px, 128px**로 출력한 동일 규격 오프라인 보드 제작. **원래 40px은 상대 판독성 부족**, 80·96px을 중간 후보로 보고, 128px을 #64 권고 대조 크기로 유지. 이 값은 실제 GameView PPU나 캐릭터 높이 계약이 아니다. 모든 NPC의 PPU를 강제로 변경하면 안 된다.
- 다음 검수: #65에서 플레이어/주변 NPC·입구·휠체어 이동과 동일 카메라 높이 기준 나란히 확인 후 바울 대표가 선택. StableID/VoiceID/퀘스트 ID 확정·변경 금지.

### 남문 — `KEEP_RESIDENCE_ROOF / REMOVE_NEW_GABLE_TRIM_FROM_PLACEMENT / ENTRYDOOR_ART_HOLD`
- 기존 `Buildings/Frostway_Residence_Building.png` 지붕·주택 원본을 유지하고 `Buildings/SouthGate_RoofGable_Joint_CANDIDATE.png` 및 `Buildings/SouthGate_RoofTrim_Joint_CANDIDATE.png` **배치에서만 제외**하는 최소안. 두 후보 PNG는 과거 ZIP에 그대로 보존, 삭제·수정하지 않음.
- 동일 오프라인 3열 비교: A 원래 Residence, B 기존+신 큰 Gable/Trim 겹침(불량 개념 재현), C 원래 지붕 유지+신 EntryDoor만. C에서 큰 이중 박공은 감소하지만 **EntryDoor 자체에도 작은 눈지붕이 있으며 원래 건물 출입문·현관과 이중 배치 위험**. 따라서 **`NATURAL_JOIN_PASS`가 아니다**.
- `SouthGate_EntryDoor_Joint_CANDIDATE.png`의 원본 바이트는 그대로. **Codex #65의 실제 집 입면·기존 Door 보존 여부, Pivot·PPU·Sorting·Collider·출입문 동선·출입구 높이**를 먼저 확인해야 한다. 겹침이 남으면 기존 출입문 KEEP이 우선이며, 실제 접합 문제가 입증된 해당 영역만 이후 국소 보완하되 무단 신규 제작 금지.

### 여관 — `KEEP_OLD_RECOMMENDED / STANDALONE_HOLD`
- 기존 `Buildings/Frostway_Inn_Building.png`와 내장 간판 및 기존 `Signs/Frostway_Sign_Inn.png` **재사용 우선**. 추가 `Signs/Inn_StandaloneSign_CANDIDATE.png`는 원본 ZIP에 별도 보관하고 **동시 배치하지 않음**.
- 비교판은 기존 간판 1개 vs 새 간판까지 붙인 중복 상태를 시각적으로 설명하는 **오프라인 모의 합성**. Unity Renderer 위치를 측정하지 않아 최종 가독성/보행 방해 판정은 미실행. 신규 간판 PNG를 삭제하거나 공식 승인 취소하는 것도 아님.

## 4. 보호·후속 조건
- 공식 환경43·승인 지면 Earth/ThinFrost/Gravel9·NPC·펫3·몬스터·Quest/Save/UserData·Scene·GUID/.meta·Main23 기존 148PASS/136PASS 작업 전부 **변경하지 않음**. 이 세션의 Unity 본게임·격리 worktree 실행·제품 commit0, main 반영0.
- 필요 증거: #65가 안전하게 공개 가능한 오르나 동배율 40↔128px Unity GameView, 남문 Door-only A/B, Inn KEEP-old A/B, 해당 3건의 PPU/Pivot/XY/Sorting/Collider와 가림, 3화면비 장면. 해당 자료가 없을 때 `ART_CONTRACT_WAITING_CODEX_EVIDENCE` 유지.
- **바울 대표 최종 미술 승인 후에만** 승인 자산의 Codex 격리 최소 Delta QA / 정식 Unity 적용 여부를 별도 판단. 현 단계 `USER_REVIEW_PENDING / NOT_UNITY_APPLIED`.
