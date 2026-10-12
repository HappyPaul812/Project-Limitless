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
