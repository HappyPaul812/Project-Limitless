# SPRITE_ART 최신 인계 — 2026-10-12

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
