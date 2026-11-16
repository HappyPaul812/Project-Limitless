# LIMITLESS / 신규 아트 / Chapter3 M2 설원족제비(Snow Stoat) 1포즈 파일럿 — 2026-11-16

**담당:** 메인 신규 아트 `docs/art-new` · **Issue:** [#100](https://github.com/HappyPaul812/Project-Limitless/issues/100)

**결론:** `ONE_POSE_CANDIDATE_CREATED / USER_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / SAME_SCALE_REFERENCE_PARTIAL / UNITY_NOT_RUN / NOT_ORIGINAL_APPLIED`.

## 1. 중복 검사
- [#100](https://github.com/HappyPaul812/Project-Limitless/issues/100), [2026-11-16 M2 기획 발주](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/20261116_Chapter3_SnowStoat_OnePose_ArtPilot_Order.md), [북부 몬스터 7종 상세](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/08_몬스터/Chapter3_설빙의폭군_온기의화로_북부몬스터_상세기획_20261014.md), `SPRITE_ART_LATEST.md` 직접 조회.
- GitHub 코드 검색 SnowStoat/설원족제비 결과 없음. ChatGPT Library의 사용 가능 기록에서도 완성된 M2 원화 납품 미발견. 다만 사용자 PC F: 드라이브 전수 탐색은 이번 세션 미실행이므로 `WINDOWS_LOCAL_NOT_AUDITED`; 기존 완성품이 100% 없다고 단정하지 않음.
- 기존 승인 FrostFieldMouse A 4동작 16프레임, Frost Owl, Wolf/Fox, F1/F2·보스 원본 변경0.

## 2. 실제 소량 납품 (신규 게임용 후보 PNG 딱 1개)
- `Candidate/SnowStoat_M2_KeyPose_512_RGBA_CANDIDATE.png` — **512×512 RGBA 투명**, SHA256 `fd25ef50528a5f630e44382e3fc40a3df607a267176f758706962a2cbc41e778`, visible alpha>8 bbox `(29,242)-(482,455)`. 우향 3/4 측면의 길고 낮은 흰 야생 족제비, 짧은 다리, 진한 외곽, 회청 털 명암, 검은 꼬리 끝. 1254px AI 원본(독립 모델 제작)을 오리지널로 생성한 뒤, 투명 픽셀 정리·종횡비 유지 축소하여 512px 게임 후보로 파생. 기존 공식 원본 재저장/트레이싱0.
- `Preview/SnowStoat_M2_Actual_128px_RGBA.png` / `Preview/SnowStoat_M2_Actual_40px_RGBA.png` 실제 픽셀 축소 출력.
- `Preview/SnowStoat_M2_512_128_40_ReadabilityBoard_OFFLINE.png`: 512, 128, 40px 및 설원/어두운 숲 비교. 실제 Unity GameView가 아닌 오프라인 검토판.
- `Preview/SnowStoat_vs_FrostOwl_Attack00_Reference_512_128_40_OFFLINE.png`: ChatGPT Library의 기존 `Limitless_FrostOwl_Attack4_Review_20261103.zip` 안의 **실제** Owl v3 Attack00 512px 참조 PNG를 *바이트 그대로 읽어* 동일 이미지 캔버스 크기로 비교. 이는 날짜가 있는 Attack00 한 프레임 비교에 한정하며 Owl 최신 Attack02 및 실제 Unity 월드 표시 배율 동등 검증을 의미하지 않음.
- `Docs/DesignNotes_And_SourceDeclaration.md`, `Manifest.json`, `SHA256SUMS.txt`. 정식 Mouse/Wolf/Fox 원본 바이너리 미확보로 이 3종 **실물 1:1 화풍 비교 NOT_VERIFIED**. 추정 이미지 재구성/허위 PASS 없음.

## 3. ZIP 검사 및 납품 채널
- ZIP 이름: `LIMITLESS_Issue100_SnowStoat_OnePose_20261116.zip`, **594,427 bytes / entries 8 / SHA256 `d18910b3ed611e127d43a64369bdf0f888e3efc175cc6bcbf7127f4c82221272`**.
- 실제 Python zipfile CRC `PASS`, ZIP 내부 `SHA256SUMS 7/7 PASS`, PNG RGBA/해상도/알파 검사 `PASS`.
- **실제 PNG/Preview/ZIP은 이번 ChatGPT 대화에서 사용자에게 제공.** GitHub에는 Markdown 보고만 저장; GitHub 바이너리 업로드/사용자 F:\ 다운로드/Unity 원본 반영을 했다고 주장하지 않음.
- Reference preview는 별도 `Preview` 파일이며 공식 Owl PNG 원본을 ZIP에 중복 포함하지 않음.

## 4. 보류 및 후속
- **바울 대표 1포즈 시각 승인 전:** Idle/Attack/Hit/Defeat/Run/Pet 시트0, 신규펫 능력/수치/MonsterID/스폰/드롭 확정0, PPU·Pivot·VisualScale `SPEC_PENDING`.
- 대표 시각 승인 이후 정식 스프라이트 구현의 512 기준 PPU/Pivot 및 40px GameView 판독은 Codex와 별도 계약. 신규 대형 Unity QA, Scene/Prefab/Save/UserData/Discord/Issue85 HOLD/Unity6000.6.5f1/main 변경0.
- **최종 상태:** `M2_KEYPOSE_ONE_CREATED / APPROVAL_PENDING / ZIP_CRC_AND_SUMS_PASS / RUNTIME_NOT_TESTED / MAIN_NOT_CHANGED`.
