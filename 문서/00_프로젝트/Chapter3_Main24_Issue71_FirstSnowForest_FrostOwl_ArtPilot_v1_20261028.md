# LIMITLESS / Chapter 3 Main24 F2 · 첫눈의 숲 + 서리올빼미 1포즈 아트 파일럿 v1

**제작 및 검수일:** 2026-10-28 (KST)  
**Issue:** [#71](https://github.com/HappyPaul812/Project-Limitless/issues/71)  
**브랜치:** `docs/art-new`  
**최종 상태:** `ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`  
**승인 게이트:** 바울 대표의 시각적 최종 승인 **필요**. Codex 실제 Unity/PPU/Collider/SpriteRenderer 확인 **미실시**. 본게임·원본·`main` 적용 및 병합0.

## 1. 실물 납품 — 현재 대화에서 다운로드 가능한 ZIP

- **실제 파일명:** `Limitless_Chapter3_Main24_F2_FirstSnowForest_FrostOwl_ArtPilot_v1_20261028.zip`
- **실제 ZIP SHA256:** `e8b268617cc096ecb1f920c1f913a787ddbc234e247843d4e9a546fa12b09a53`
- **ZIP 크기:** 8,429,257 bytes, 14 entries, `testzip()` CRC PASS
- **SHA256SUMS:** ZIP 내부 13개 나머지 파일 **13/13 원본 바이트 해시 대조 PASS**, Manifest/CSV 포함
- **실물 전달:** 현재 ChatGPT 대화 아티팩트 `/mnt/data/Limitless_Chapter3_Main24_F2_FirstSnowForest_FrostOwl_ArtPilot_v1_20261028.zip` 에서 다운로드 가능. **Windows `F:\Downloads`에 자동 저장 또는 사용자 로컬 복사본을 직접 검사했다고 주장하지 않는다.** GitHub에는 본 QA 문서와 최신 인계만 등록하고 ZIP/PNG 바이너리는 원격 저장소에 업로드하지 않았다.

| 실제 ZIP 파일 (내부 상대경로) | 규격/검증 | SHA256 |
|---|---|---|
| `Concept/F2/FirstSnowForest_Environment_Concept_OFFLINE.png` | 1448×1086 RGB 환경 콘셉트 | `85928588798a1c3296546c94446f2d06872e2c3805ddf60c118e80167841075b` |
| `Candidate/Monster/FrostOwl_KeyPose_512_CANDIDATE.png` | **512×512 RGBA 투명, 1포즈** | `f60c41cfdc6f3d1bc1b05b62a2281d5f607dec322b9c5c4c3090cac725c09ab1` |
| `Preview/FrostOwl_128px_WholeCanvas_OFFLINE.png` | 128×128 RGBA, 실제 후보에서 축소 | `01e894f0b2505056b01af663110668a7c28b992fc527b433af6ca4566d4f6b4e` |
| `Preview/FrostOwl_40px_WholeCanvas_OFFLINE.png` | 40×40 RGBA, 실제 후보에서 축소 | `4ce6a40384274b5765f8a48c05409907ce4256ba3ccb51e279527d60418e2a8b` |
| `Preview/FrostOwl_512_128_40_ActualPNG_Readability_OFFLINE.png` | 1050×660 RGB, 실제 동일 PNG 3규격 나열 | `2a57f2194269dfa2cd4665ae6baf5152fe6d44971de4f0a3b703ac45bc70351e` |
| `Preview/F1ApprovedGround_vs_F2_FirstSnow_StyleCompare_OFFLINE.png` | 1600×850 RGB, **기존 승인 지면 실물** 3종 이미지와 신규 F2 콘셉트 | `cd94068ae4e83be244470a51db4fb15693013017ca46b3ab012419a1398f5f04` |
| `Preview/FrostOwl_vs_Chapter2_OfficialFive_PreviewReference_OFFLINE.png` | 1590×730 RGB, 기존 **Chapter2 공식 5종 표시 보드** 크롭과 신규 Owl 비교 | `3828f3af2c88f45b349e70bb25fbb711406ebb3102b10bdf8fdd3bce7a851cc3` |

추가 `Preview/`: F2 환경 원화의 경로 부분 크롭 1390×550 1장, 과거 공식 Ch2 다섯 몬스터 비교 보드의 참조 크롭1111×299 1장. `Manifest.json`, `Docs/F2_Environment_Asset_Needs.csv`(환경 필요 자산8종), `Docs/ArtPilot_Notes.md`, `QA/File_SHA256_and_Spec.csv`, `SHA256SUMS.txt` 포함. 모든 PNG decode/중복경로 0; 게임용/후보 폴더 신규 PNG **1개**, 나머지 콘셉트·검수용 이미지.

## 2. F2 첫눈의 숲 — 미술 시안 1장

- 기존 F1의 얇은 서리와 **Frostway 승인 Earth/ThinFrost/Gravel 지면 질감**에서, 아직 흙길이 읽히는 **첫눈이 내리는 숲**으로 점진적으로 진행. 기존 흙/서리색을 유지하되 길 밖 식생·경계에 얇은 눈이 쌓이고 외곽 나무가지·그루터기·돌만 부분 적설.
- 콘셉트 중심 통행로는 흙·자갈 노출과 낮은 방해물 밀도로 **걷거나 휠체어로 이동 가능한 길을 시각적으로 명확하게** 표현. 색상에만 의존하지 않고 길의 형태·질감·눈둑 간격을 사용.
- **신규 환경 자산 요청 목록 8종**: (P1) 설피나무/소나무 실루엣 변형, 첫눈 지면 부분 오버레이, 길 가장자리 낮은 눈둑·돌, 서리 관목·풀; (P2) 눈 덮인 쓰러진 나뭇가지, 그루터기/횃대, 표지판, 가벼운 눈발 레이어. CSV 참조. **이번에 환경용 개별 타일 PNG 생산 0 / 타일맵 전체 생성 0 / Scene 설계 0**.
- **비교 검증 한계:** 비교판 왼쪽은 v10 아카이브에서 기존 실제 Earth/ThinFrost/Gravel 01 타일 바이트를 추출해 표시한 것이며, **F1 실제 Unity GameView를 읽은 것은 아니다**. 오른쪽은 생성 F2 미술 콘셉트다. 16:9/4:3/21:9 실제 장면/PPU/Collider·휠체어 통과 폭 QA는 `TBD/NOT_TESTED`이며 낮은 대비에서 실제 통과 승인이라고 선언하지 않는다.

## 3. 서리올빼미 Frost Owl — 독립 한기 몬스터 후보

- **요구한 외형**: 청회색 큰 양 날개, 검은 날개 바깥 윤곽, 구분되는 얼굴과 꼬리 실루엣, 눈 결정·서리 기운의 미묘한 장식으로 F2에서 처음 `한기`를 쓰는 적임을 시각적으로 암시. 초안 기획 상 `날개 냉기 모으기` 연출을 제안 가능하나 **실제 VFX/한기 2턴 행동불가 구현 또는 새 게임 시스템 수정0**.
- 제작 원본 1254×1254 RGBA 콘셉트 이미지를 **실물 512×512 PNG 1장으로 정규화/최종 알파 여백** (원본 488×488로 downsample, 12px 투명 가장자리). 128/40 축소는 **이 실제 512 파일에서 생성한 픽셀 축소**. 이전 ImageGen 보드의 재창작 축소 이미지를 검증 자료로 활용하지 않음.
- 스타일 비교에는 과거 `필드09 몬스터 아트 프리뷰 보드.png`의 **Chapter2 공식 몬스터5종 표시된 부분의 이미지 크롭**을 재사용했다. 따라서 **실제 공식 314×314 셀 파일의 1:1 추출·공통 PPU·게임 내 동배율 입증은 아직 아님**. 검수용 보드에 오프라인 참조라고 명시하며 진짜 Ch2와 같은 Unity 스케일이라고 주장하지 않는다.
- **애니메이션:** `Idle0 / Attack0 / Hit0 / Defeat0` (모두 미제작). 공식 MonsterID/스폰/보상/Collider/Combat 구현0, 우선 1포즈 미술 검토만.
- **추가 아트 QA 미결:** 작은 40px 캔버스에서 눈·부리 선명도 및 초상화와 전투 실루엣의 비례, 외곽 날개의 넓은 점유율, 서리 이펙트 색이 게임에서 충분히 대비되는지는 바울 대표와 Codex 실제 Render 검수 필요.

## 4. 원본 보호·승인 게이트

- 기존 Main23 **환경 승인43**, v10 Earth/ThinFrost/Gravel **승인 지면9**, Main22 F1 및 서리들쥐, Wolf/Bear/Fox, Chapter1·2 기존 몬스터 및 NPC/펫, `Main23 148/136/33/8/23` 검증 기존 결과, Unity Scene·Save/UserData·GUID/.meta·Quest/포탈/스킬 상점 전부 **바이트 변경 0**. 보호 파일은 작업에 입력 참조만 사용하고 원본 수정·덮어쓰기하지 않음.
- Main23 #59 결정 유지: **Orna visible alpha80px (whole canvas84.63px) 기획 기준**; Residence 본체·지붕·문 KEEP; 신규 RoofGable/Trim/EntryDoor 후보 배치 HOLD; 여관 본체 내장 간판 하나 KEEP, 새 독립 간판 보관/HOLD.
- **미술 승인:** 바울 대표 `USER_ART_REVIEW_PENDING` / `USER_ART_APPROVED = false`. Codex 격리 Unity QA `NOT_RUN`. 제품 적용 `NOT_UNITY_APPLIED`; `main` 직접 커밋·Push·병합 없음.
- **다음:** 대표님 F2 콘셉트·서리올빼미 단독 512/128/40 비교 원화 미술 판단 → 필요 시 후보 소폭 수정 → Codex 별도 격리 Unity PPU·Pivot/Collider/통행 및 Ch2 실제 프레임 동일 배율 검수 → 승인 후에만 정식 통합 허가. **이 턴에서 추가 타일 대량 생성 또는 Idle/Attack 작업 진행하지 않는다.**
