# LIMITLESS / Chapter3 / Issue #89 — 냉기 VFX 3종 최소 표적 보완 정식 보고 (2026-11-04)

**담당:** Chapter3 이후 신규 아트 / `docs/art-new`  
**판정:** `TARGETED_ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED`

## 1. 최신 발주 및 판정 근거

- [Issue #89 기획 표적 보완 발주](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6620385952)와 [Codex 실제 Windows F: ZIP 검증·40px/설원 시각 분석](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6620211048)에 기반.
- [Issue #88 냉기 3상태 VFX 계약](https://github.com/HappyPaul812/Project-Limitless/issues/88) 및 공식 냉기·보스기믹·해제 문서에 따른 독립된 상태3종을 보호. 한기=연무/눈송이, 동상=붙는 얼음결정, 동결괴사=분리된 검푸른 균열·큰 쐐기.
- 기존 `Limitless_Ch3_ColdVFX_Issue89_Pilot_20261104.zip` **1,535,561 bytes, SHA256 `83b4efeee62cdb4cabf7dbcc4d2aea21cbefdb4507e03f53b8c59d8079e9e4ba`**를 재사용했으며 ZIP 안의 원본 3PNG SHA가 기존 GitHub 납품값과 완전히 동일함을 확인. **기존 v1 ZIP 및 원본 PNG를 전혀 덮어쓰지 않음**.

## 2. 이번 신규 실물 납품 — 별도 후보 PNG 3개

- **새 ZIP:** `Limitless_Ch3_ColdVFX_Issue89_MinimalFollowup_20261104.zip`
- **ZIP SHA256:** `9c07f5ff782c6e272796c4f003b816a39806ab35248c2353884325ac3c24a1eb`
- **크기:** **1,434,793 bytes / 23 entries**
- **QA:** ZIP CRC `testzip() PASS`, `SHA256SUMS.txt` **22/22 파일 대조 PASS**, 파일명 중복0, 실제 후보 **3/3 PNG 512×512 투명 RGBA PASS**.
- **실물 전달:** ChatGPT 파일 라이브러리 **`/LIMITLESS/Chapter3_Issue89_ColdVFX_MinimalFollowup_20261104/`**에 ZIP과 비교판 2개 업로드 및 목록 재조회 성공. 사용자 Windows `F:\Downloads`에 자동 복사하거나 사용자 PC 사본의 SHA를 직접 검증한 적 없음. GitHub PNG/ZIP 원격 바이너리 업로드 **0**, 문서만 커밋.
- 모든 새 게임용 후보는 `Candidate/`에 `_v2_TARGETED_CANDIDATE_512_RGBA.png`로 별도 파일, 기존 v1은 이전 ZIP에 KEEP.

| 대상 | 신규 PNG | v2 SHA256 | v1 SHA256 |
|---|---|---|---|
| 한기 | `VFX_Ch3_Chill_Oneshot_01_v2_TARGETED_CANDIDATE_512_RGBA.png` | `e50831520d21ca51b51bca0fab030446f5c15f034d699c6ee3fd79304d1691c2` | `dd7bd4bb73d6e5c4b3d02521da2e0f37e0ba4347597ee97c125d3e8fd693d793` |
| 동상 | `VFX_Ch3_Frostbite_Sustain_01_v2_TARGETED_CANDIDATE_512_RGBA.png` | `5b8ca776ec14c52fd671117182fa299fed31b8774d85f6843f8f62ec70b96415` | `49937d84472a2adc59da04a17c43d22a37d3d63992a45e26581d534f7b93e035` |
| 동결괴사 | `VFX_Ch3_FrostNecrosis_Stack2_01_v2_TARGETED_CANDIDATE_512_RGBA.png` | `59621e0b912e7c30544e559cb99446dcfb7f9c7c8dd1c846a07f32dce2dfd299` | `f1e69de225e6958f813fd155a7a06a7aea34e6ba7f039e7665e1e614b6868a4a` |

## 3. 국소 변경 및 원본 보호

- **한기**: 기존 얇은 연무·눈송이·바닥 고리 위치/전체 RGBA bbox **(40,70)-(472,442) 보존**. 기존 알파가 있는 픽셀 중 **하단 고리 가장자리와 가운데 얕은 연무 경계**에서 차분한 청회색 RGB를 조금 어둡게, 알파를 소폭 보강. 차이 픽셀 **46,694**, 알파 변경 46,694, **변경 bbox (44,152)-(469,441)**. 추가 링/검정 테두리/새 효과/광량 플래시 없음. 설원에서 읽힘은 비교판 제공; 실제 게임의 시각적 PASS 주장하지 않음.
- **동상**: 기존 붙은 결정 고리·얼음 군집·몸통 위의 차가운 인상 보존. **중앙 위쪽 긴 단일 결정 끝만 국소적으로 아래로 약24px 변형**하고 바깥 분산된 작은 부스러기의 낮은 알파만 약화하여 군집형 실루엣 강화. 차이 픽셀 **10,464**, 알파 변화 7,705, 부분 bbox (56,38)-(455,417). 신규 bbox (54,59)-(457,474). 기존 파일을 전체 축소/재설계하지 않음.
- **동결괴사**: 기존 검푸른 중심 균열과 큰 쐐기 모티프 유지. **하단 좌·우 큰 쐐기 외곽만 최대18px의 국소 비강체 변위**로 퍼지게 하고 본래 어두운 중앙 균열 명암만 소량 강화. 변경 픽셀 **72,721**, 알파 변경 36,398, **변경 bbox (46,211)-(470,475)**, 중앙 위쪽 세로 큰 결정과 2/3 보스 위험 형태를 유지. 많은 픽셀 변경을 작은 색 보정으로 과장하지 않고 **파일3개만 선택적으로 보완**한 후보로 기록.
- **정확한 시각 검수 범위:** PNG 파일의 픽셀/해상도/투명/새 SHA/기존 SHA 기술 검수 PASS. 40px에서 동상과 동결괴사의 차이는 **위 결정 높이/바깥 큰 쐐기**로 개선 후보를 제시했지만, **사용자 시각 승인 전 구분성 최종 PASS를 선언하지 않음**. 밝은 설원의 한기 대비도 사용자가 확인할 때까지 미승인.
- 기존 승인 Owl v3 Idle8·Attack4, NaturalBroken Stump v4, 승인 지면9/환경43, F2 원본, 이전에 분리된 IceArc VFX 및 #88 Codex 냉기 런타임 격리 63PASS 모두 **읽기 전용 보호**. 신규 정적 PNG 외 애니메이션/시트/Particle/Hit/동상 HP 틱 수치/동결괴사 3중첩 지속 효과 **없음**.

## 4. 전후 비교·기술/미술 게이트

- 실제 게임용 v2 PNG 3개와 별도로 `Preview/{CHILL,FROSTBITE,NECROSIS}_{v1,v2}_{128,40}px_ACTUAL.png` 총 **12장**: 반드시 원본 및 새 512×512 픽셀의 실제 PIL LANCZOS 축소 결과.
- `Preview/Issue89_v1_v2_128_40_BeforeAfter_OFFLINE.png` **1670×1440**: 세 종류별 v1 KEEP ↔ v2 후보, 밝은 설원/어두운 배경128과 40 실축소를 정확한 캔버스에서 묶은 한장 비교. 40의 큰 표시 버전은 **NEAREST 3배** 검토판일 뿐 게임 렌더 해상도 변경 아님.
- `Preview/Issue89_v1_v2_Snow_DarkBG_SameScale_OFFLINE.png` **1420×1220**: 동일 128px 스케일 밝은 #eceff2 인근 배경/어두운 #172231 배경 전후 비교판 및 40px 실축소. **OFFLINE 합성**, Unity 전투 UI/GameView 또는 실제 960/1280/1680×720 QA가 아님.
- `Docs/DesignNotes.md`, `Docs/SourceDeclaration.md`, `README_KR.md`, `Manifest.json`, `QA/DeltaQA.json`, `SHA256SUMS.txt` ZIP 동봉. 전체 파일 SHA 및 각 PNG RGBA 변경 바운딩 박스는 Manifest에 수록.
- UI 상태명/각 아이콘·남은 턴/중첩 숫자/위험 문구는 **PNG에 그려 넣지 않았음**. 3종 상태의 자동 진화 금지, 동결괴사 세 번째는 기존 즉시 KO라는 기획 계약 유지. FPS 없음(정지), `PPU/Pivot/Sorting/WorldScale=SPEC_PENDING`; 사용자 미술 승인 전 Unity Import 계약 확정·Scene/Prefab/Save/UserData/BuildSettings/Packages/코드/HUD 변경0.

## 5. 최종 판정

**`TARGETED_ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_UNITY_APPLIED`**.

- 바울 대표가 128px/40px 설원·어두운 전장 검토판의 **한기 외곽 대비**와 **동상(결정 군집)↔동결괴사(큰 쐐기/균열)** 모양 차이를 승인/반려.
- 승인 후 Codex **Unity 6000.6.5f1 격리 최소 VFX/UI Delta QA**; 승인된 상태명/아이콘·중첩/턴 표시, 실 캐릭터 40px 및 HUD/입력 방해 없는지 별도 검증 필요.
- 기존 Owl/Save/Unity 원본, `main` 직접 commit/Push/merge 금지, 본편 적용은 사용자 별도 승인.
