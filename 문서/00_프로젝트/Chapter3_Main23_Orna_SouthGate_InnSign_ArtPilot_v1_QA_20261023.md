# LIMITLESS / Chapter 3 Main23 — 오르나·남문 접합·여관 독립 간판 / 신규 아트 1차 파일럿 (2026-10-23)

## 2026-10-23 바울 대표 납품 수령 확인 / Issue #59 정식 보고 보강

**정식 상태: `ART_CANDIDATE / USER_REVIEW_PENDING / NOT_UNITY_APPLIED`.** 오프라인 파일검사는 PASS이지만 미술 최종 승인 및 실제 Unity 적용은 아닙니다. **남문 지붕/문 접합은 별도 `SOUTHGATE_JOIN_ART_HOLD`**(기존 지붕과 신규 박공의 이중 겹침).

- **사용자 수령 확인**: 바울 대표가 Windows `F:\Downloads\Limitless_Main23_Orna_SouthGate_InnSign_ArtPilot_20261023.zip` 전달 사실을 직접 확인함(2026-10-23). 이는 사용자 전달경로 확인이며 이 세션에서 사용자 PC 파일의 SHA를 직접 읽었다는 뜻은 아님. 제작본 원본 ZIP SHA256 `425292ebd1e8a58ab7c4486f6fac39491c265d49b5db933dfe7586e69cee7351`, 8,008,308 bytes, 20 entries; CRC PASS, 내부 SHA256SUMS 19/19 PASS **본 턴 재확인**.
- **후보 PNG/규격**: 아래 6행 파일 표는 모두 실제 ZIP 내부 `RGBA` 이미지이며 가로×세로 px 기준. ① `NPC/Orna_VillageGuide_Concept_Master_CANDIDATE.png` 769×1251(오르나 단독 원본) ② `NPC/Orna_VillageGuide_512_CANDIDATE.png` 512×512 ③ 남문 출입구 575×923 ④ 남문 지붕 박공 711×498 ⑤ 지붕 받침 280×240 ⑥ 독립 여관 간판 1135×1097. **3개 작업군, 총 신규 후보 파일 6개(오르나 2/남문 3/여관 1)**. SHA256 파일별 전수 값은 바로 아래 원래 표 및 ZIP `QA/Individual_NewAsset_Sizes_SHA256.csv`에 기재.
- **오르나 단독 검토**: `NPC/Orna_VillageGuide_Concept_Master_CANDIDATE.png` 원본, `Preview/Orna_128px_FullCanvas_OFFLINE.png` (128×128), `Preview/Orna_40px_FullCanvas_OFFLINE.png` (40×40), `Preview/Orna_512_128_40_Readability_OFFLINE.png` (1020×650 통합판). 축소 비교는 Unity PPU·가독성 기술 PASS 판정이 아닌 오프라인 미리보기.
- **남문 전후 접합**: `Preview/SouthGate_Entry_Roof_Before_After_APPROX_OFFLINE.png` (1356×575); 기존 주택 유지·신규 문/지붕 후보 별도 합성. **현재 이중 박공 아트 HOLD**, Codex 정확 좌표/Sorting 확인 필요. 개별 모듈은 `Preview/SouthGate_Separated_ModuleSheet_OFFLINE.png`.
- **여관 간판 전후**: `Preview/Inn_Sign_Before_After_APPROX_OFFLINE.png` (1356×575); 작은 기존 `Signs/Frostway_Sign_Inn.png`과 새 `Signs/Inn_StandaloneSign_CANDIDATE.png`의 구분·비교는 `Preview/Inn_Sign_Existing_vs_New_OFFLINE.png`(890×410). 교체 필요 여부 사용자 미술 판단 대기.
- **기존 KEEP**: `Buildings/Frostway_Residence_Building.png`, `Buildings/Frostway_Inn_Building.png`, 기존 `Buildings/Modular/Frostway_Door_*.png`·`Buildings/Modular/Frostway_Roof_*.png`, `Signs/Frostway_Sign_Inn.png`; **기존 환경43·최종 승인 Earth/ThinFrost/Gravel 지면9·게임용 기타 승인 자산 바이트 수정0**. 새 후보는 독립 파일이며 공식 스프라이트 승격0.
- **오프라인 3화면비**: `Preview/OfflineTown_16x9_NO_UNITY.png`, `OfflineTown_4x3_NO_UNITY.png`, `OfflineTown_21x9_NO_UNITY.png`. Codex `Main23_Frostway_VisualGapList.csv` 및 실 GameView/PPU/Pivot/Collider/footprint 원문은 이 세션에서 직접 미열람, 아트용 실제 인게임 규모 검증 `NOT_TESTED`. 별도 Codex 격리 시각/기능 136 PASS/0 FAIL과 혼동 금지.
- **승인 게이트**: 바울 대표 미술 최종 승인 **필요**. 사용자 승인 전 Unity 정식 적용0, main 커밋/Push/병합0, OPEN16 자체 해결 주장0. 현재 역할은 정식 *접수된 파일럿*, 공식 게임아트 승인 아님.


## 판정과 대상

**상태**: `ART_CANDIDATES_DELIVERED / FILE_OFFLINE_QA_PASS / SOUTHGATE_JOIN_ART_HOLD / CODEX_UNITY_DELTA_QA_PENDING / USER_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED`.

- 기획 근거: [Issue #59](https://github.com/HappyPaul812/Project-Limitless/issues/59) 최신 기획 댓글 `#issuecomment-6355677632`, [Codex #58](https://github.com/HappyPaul812/Project-Limitless/issues/58) 인계 댓글 `#issuecomment-6355678485`.
- Codex 제공 요약은 **격리 마을 시연 18/43 자산, 변경분 136 PASS/0 FAIL, 기존 첫 플레이 148 PASS 재실행 없음**. 이는 별도의 **기능/Unity 프레젠테이션 변경분 QA**이며 본 후보 신규 아트의 Unity 합격이 아니다.
- Codex 로컬 `Main23_Frostway_VisualGapList.csv`와 실제 3화면비 GameView 실물은 현재 아트 담당에게 직접 공유되지 않았음: 행별 XY·PPU·Pivot·footprint·Sorting·Collider 미확인, `NOT_RECEIVED / NOT_TESTED`. 이슈 본문·댓글의 우선순위 3개를 기준으로 비파괴 이미지 파일럿만 제작.
- 이번 신규 미술 작업군 **3개**: ① 안내 NPC 오르나 1인 ② 남문 주택 문+지붕+지붕 받침 1세트(3 PNG) ③ 여관 독립 간판 1종. 오르나 원화 Master와 512 축소본 포함 신규 **독립 후보 PNG 6개**.
- 기존 43개 환경/기존 v9 기준 총 게임 PNG49/최종 승인 지면9는 **소스 덮어쓰기 0**. 기존 주택 `Buildings/Frostway_Residence_Building.png`, 여관 `Buildings/Frostway_Inn_Building.png`, `Signs/Frostway_Sign_Inn.png`, 문·지붕 모듈 등은 KEEP/비교만 수행. 유관 없는 지형·펫·Quest·Save·GUID·Scene/Prefab 수정0.

## 실제 납품 파일

- 다운로드 전달 실물: **`Limitless_Main23_Orna_SouthGate_InnSign_ArtPilot_20261023.zip`**. 정확한 **ZIP SHA256 `425292ebd1e8a58ab7c4486f6fac39491c265d49b5db933dfe7586e69cee7351`**; 크기 **8,008,308 bytes**, ZIP **20 entries**, `testzip()` CRC PASS; `SHA256SUMS.txt` 내부 **19/19 PASS**.
- 실물 저장·전달 경로: **본 ChatGPT 대화 생성 파일 `/mnt/data/Limitless_Main23_Orna_SouthGate_InnSign_ArtPilot_20261023.zip`**. 사용자가 이 대화 다운로드 링크로 수령 가능. **GitHub는 본 Markdown·인계만 기록하며 PNG/ZIP 바이너리는 GitHub 등록하지 않음.** **2026-10-23 대표님께서 `F:\Downloads\Limitless_Main23_Orna_SouthGate_InnSign_ArtPilot_20261023.zip` 수령을 직접 확인**. Codex 로컬 복사 및 수령 PC 내 바이트 SHA 직접 재확인은 미실행.
- ZIP 내부 `Manifest.json`은 소스 generated PNG3의 SHA, 신규 후보6, 기존 v9 참조 게임 PNG49 각각의 SHA와 실제 미검증 게이트를 포함. `QA/Offline_Art_QA.json`, `QA/Individual_NewAsset_Sizes_SHA256.csv`, `SHA256SUMS.txt` 수록.
- 제작 특성: 신규 컷아웃은 **AI 생성 페인터리 콘셉트 PNG 기반**. 각 시트의 컴포넌트를 개별 투명 RGBA로 추출. 정식 스프라이트 애니메이션/유니티 Scale 최종본을 주장하지 않음. 오르나/남문/간판의 정식 스타일 승인 별도.

| 후보 PNG (ZIP 내부 상대경로) | 해상도, RGBA | 파일 SHA256 |
|---|---|---|
| `NPC/Orna_VillageGuide_Concept_Master_CANDIDATE.png` | 769×1251 | `ba750bde78fdc41f560625258a584055779ce7e06323628b3556cdbd50fbb57e` |
| `NPC/Orna_VillageGuide_512_CANDIDATE.png` | 512×512 | `9b6ebe892777874a859724d4c2326083e3c2d25541d3b5f7013c88498ffba74d` |
| `Buildings/SouthGate_EntryDoor_Joint_CANDIDATE.png` | 575×923 | `f90c096d71e11258b5bbb087197652f6bc31019b4c2fd669386ed965d7973e52` |
| `Buildings/SouthGate_RoofGable_Joint_CANDIDATE.png` | 711×498 | `a8b8fc6d7a911f05a7eabe1ad7096e21963fb53db0d630dff96e27df9f262d6a` |
| `Buildings/SouthGate_RoofTrim_Joint_CANDIDATE.png` | 280×240 | `eaf17535a1565ee1d4c153b7cbc859191fd77ef31cf4275ff44ebf29dc3412db` |
| `Signs/Inn_StandaloneSign_CANDIDATE.png` | 1135×1097 | `c2806a5ba3b4feed8e10433d96c43a12e59fb53209600d09e0ee6cb4e67c6ddf` |

## 비교·게임 크기 대용 미리보기 (ZIP 내부)

- `Preview/Orna_512_128_40_Readability_OFFLINE.png` + `Preview/Orna_128px_FullCanvas_OFFLINE.png` + `Preview/Orna_40px_FullCanvas_OFFLINE.png` — 512·128·40px **순수 픽셀 축소 판독성**, 실제 Unity 표시 크기·PPU 아님.
- `Preview/SouthGate_Separated_ModuleSheet_OFFLINE.png` — 남문 출입구, 지붕 박공, 목재 접합 받침 독립 스프라이트 확인.
- `Preview/SouthGate_Entry_Roof_Before_After_APPROX_OFFLINE.png` — 기존 Residence 원본 좌/별도 후보 합성 우. **현재 합성에 겹지붕/이중 박공이 보이므로 미술 접합 HOLD**. Codex 실제 건물 문·지붕 XY 계약을 받고 문·지붕 모듈 맞춤을 검증하기 전 공식 교체 불가.
- `Preview/Inn_Sign_Existing_vs_New_OFFLINE.png`; `Preview/Inn_Sign_Before_After_APPROX_OFFLINE.png` — 이미 존재하는 작은 여관 간판 재사용과 새 독립 간판의 오프라인 비교. **기존 간판 충분 여부는 실제 Codex 스케일 기준 재확인 후 KEEP 결정**; 새 간판은 필수 교체가 아닌 후보.
- `Preview/OfflineTown_16x9_NO_UNITY.png`, `OfflineTown_4x3_NO_UNITY.png`, `OfflineTown_21x9_NO_UNITY.png` — 동일 크기 모의 배치의 화면비별 크롭. 배경/건물은 오프라인 참고 합성이며 **Unity GameView·통행 PASS 증거 아님**.

## 기술·미술 보류, 다음 게이트

- **검증 완료(오프라인)**: 독립 신규 후보 6 PNG decode RGBA, 투명 alpha 채널 존재, 원본 ZIP에서 분리 컴포넌트/재현 상대경로 보유, ZIP CRC 및 SHA 전수 19/19. 기존 승인 원본 파일은 어떠한 바이트도 본 작업으로 변경/덮어쓰기 하지 않았음.
- **미검증**: 실제 마을 GameView에서 동일 카메라/정식 PPU·Pivot·SpriteRect·Sorting·Collider·footprint·남북 3WU 통행·동쪽 7×7WU 예약공간 가림. Codex CSV 행 원문과 실물 GameView 접근 전 `TECH_GAME_READY` 금지.
- **시각적 리스크**: 오르나 머리/옷 디테일의 40px 가독성 및 기존 NPC 톤 조화, 남문 출입구·겹지붕 접합(현재 모의 합성에서 결함 확인), 독립 여관 간판 시각 밀도/기존 Inn PNG 간판과 중복. **USER_ART_PASS 0 / USER_APPROVED 0**.
- 오르나는 작업 가칭이며 공식 Stable ID, VoiceID, 이름/퀘스트 대사 확정0.
- **다음**: Codex가 `#58` GapList 실제 공개 가능 상위 3행의 XY/PPU/Pivot/footprint/Sorting/Collider 및 동일배율 GameView를 인계 → 특히 남문 조인트/간판만 표적 보완 → 바울 대표 미술 승인 → 승인분만 Codex 격리 Runtime DeltaQA 및 본게임 반영을 **별도 사용자 지시**로 진행.
- 사용자 승인 전 제품 Unity 적용, 제품 PR/main 직접 Push·Merge 금지. OPEN16 기존 잔여16/해결0.
