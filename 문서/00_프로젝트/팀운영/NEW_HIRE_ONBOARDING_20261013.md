# LIMITLESS 신입 담당 자동 온보딩 — 2026-10-13

목표: 사용자에게 역할과 프로젝트가 이미 지정된 담당 세션에서는 “공부하고 와”라는 한마디만으로 최신 GitHub 문서 확인, 역할 학습, 현재 상태 판독, 다음 업무 보고까지 스스로 진행합니다.

## 호출 조건
- 기존 역할 대화: “공부하고 와”로 즉시 시작. 이미 제공된 역할·저장소를 다시 묻지 않습니다.
- 완전히 새로운 대화: 최초 한 번 “너는 LIMITLESS [담당]이다. GitHub 공부하고 와.”라고 역할을 알려줘야 합니다. 역할·저장소가 전혀 없는 채 “공부하고 와”만 받으면 추측하지 말고 필요한 정보 하나만 묻습니다.
- 세팅은 문서 학습과 상태 확인입니다. Unity 실행, 외부 서비스 연결, 결제, TTS 호출, 아트 생성, Git Push까지 자동 승인한다는 뜻이 아닙니다.

## 정본과 담당 브랜치
저장소: HappyPaul812/Project-Limitless
- main: 사용자 승인 후 통합된 게임 코드·문서의 기준. 읽기 전용으로 취급합니다.
- docs/planning: 최신 팀 공통 행동요령, Git 협업, 이 온보딩 문서.
- docs/art-renewal: Chapter 1·2 기존 아트 리뉴얼 담당.
- docs/art-new: Chapter 3 이후 모든 신규 아트 담당.
- docs/tts: TTS 담당.
- docs/bgm: BGM 담당.
- docs/sfx: SFX 담당.
- docs/video: 개발 기록 영상 담당.
- codex/작업명: Codex Unity 구현·QA, 로컬 별도 worktree.

자기 담당 문서 경로: 문서/00_프로젝트/팀운영/담당/[ROLE].md
인계 경로: 문서/00_프로젝트/팀운영/인계/[ROLE]_LATEST.md
ROLE: PLANNING, WORLD_ART_RENEWAL, SPRITE_ART, TTS, BGM, SFX, DEV_VIDEO, CODEX.

## ‘공부하고 와’ 실행 순서
1. 현재 대화에 지정된 자기 역할을 파악합니다. 역할을 임의 변경하거나 다른 팀 일을 중복 맡지 않습니다.
2. GitHub main, docs/planning, 자기 담당 브랜치의 현재 SHA를 실제로 조회합니다. 접근 불가한 브랜치는 확인한 척하지 않습니다.
3. 문서/00_프로젝트/팀운영/START_HERE.md → COMMON_RULES.md → GIT_BRANCH_WORKFLOW_20261013.md → 자기 담당 역할 문서 → PROJECT_CONTEXT.md → CURRENT_STATUS.md 순서로 읽습니다. Codex는 루트 AGENTS.md 및 CODEX_WORKFLOW.md가 우선입니다.
4. 최신 담당 인계 파일과 실제 최신 QA/스토리·시스템 정본을 확인합니다. CURRENT_STATUS의 맨 위 최근 항목과 이전 기록의 상충 여부를 점검합니다.
5. 사용자 승인, 아트·음성 제작 완료, Unity 정식 적용, 기술 QA PASS/PARTIAL, 사람 직접 확인, Git commit/Push를 엄격히 구분합니다. 낡은 완료 대기 기록을 최신 기록인 것처럼 말하지 않습니다.
6. 자신의 현재/다음 업무를 1~3개 선정하고, 기존 승인 원본·저장 데이터·GUID/meta·Unity Editor·상용 라이선스 등 보호 사항을 정리합니다.
7. 새 작업을 실행하거나 파일을 작성하기 전에 사용자 승인 요구와 다른 담당 의존성을 확인합니다. 교육만 요청했으면 읽고 보고하는 데서 멈춥니다.
8. 아래 양식대로 보고합니다. 사용자가 별도 기초 교육을 요구하지 않아도 됩니다.

## 교육 완료 보고 양식
- 내 담당과 업무 범위 / 맡지 않는 다른 담당 업무
- 실제 읽은 정본 파일과 확인한 main·공통·담당 브랜치 SHA
- 이미 완료된 일 / 현재 진행 중 / 미승인·미검증·자료 없음
- 바로 가능한 다음 작업 1~3개와 사용자 승인이 필요한 항목
- 보호할 자산·Save·Git 주의점
- 상태: ONBOARDED_READ_ONLY (실제 문서 읽고 파악 완료) 또는 ONBOARDING_BLOCKED (접근/역할/정본 부족)
- 새 파일/커밋 작성 여부, 작업 브랜치, 커밋 SHA. 읽기만 했으면 “새 커밋 없음”으로 기록.

## 신입 리뉴얼 담당 특별 주의
- 담당: Chapter 1·2 기존 필드·던전·몬스터·펫·NPC·VFX 리뉴얼만. Chapter 3 이후 모든 신규 아트는 SPRITE_ART 담당입니다.
- 이전 QA: Chapter1 Field02·Field03·던전 B1/B2 정식 통합 보고, 기존 Unity Search 관련 PARTIAL. B1 봉인문은 큰 문 PNG 대신 작은 상태 표식으로 해결한 보고가 있습니다.
- 새 사용자 정보: 2026-10-13 현재 “F:\Downloads\Limitless_Field03_RockB_Fix_v2.zip”와 “F:\Downloads\Limitless_Field04_Renewal_Art_v1.zip” 납품 경로를 받았습니다. 이전 인계의 “Rock B 수정본 없음”은 오래된 이력이며 재확인 필요합니다.
- Codex 검수 보고에는 Rock B v2 및 Field04 타일 대응 조사 결과와 별도의 미술 보완·검증 사항이 있습니다. 사용자 최종 미술 승인 및 Unity 정식 적용은 아직 확인되지 않았으므로 절대 완료로 단정하지 않습니다.
- 다음 지역 순서: Field04 → Arbel → Field05~Field11 환경, 다음 기존 몬스터 → 펫 → NPC → VFX. 이미 승인된 공식 캐릭터·몬스터는 무단 교체하지 않습니다.
- 다른 담당/신입이 진행한 실제 ZIP을 다시 만들지 말고, Codex 조사 및 사용자 승인 결과를 먼저 비교합니다.

## 공통 제한
GitHub 조회나 로컬 자료 확인이 불가능하면 막힌 이유만 보고합니다. 다른 담당 세션과 자동 직접 통신했다고 주장하지 않습니다. user의 미커밋 자료를 삭제하거나 main을 직접 갱신하지 않습니다. 담당 문서 수정은 자기 docs/* 브랜치에서만, 병합은 사용자 승인 후 진행합니다. 세션 대화 한계 전에 사용자에게 미리 알리고 ROLE_LATEST 인계를 준비합니다.
