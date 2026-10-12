# Project-Limitless

**LIMITLESS**는 Unity 6로 개발 중인 2D 턴제 RPG다. 저장소와 C# namespace에는 내부 이름 Project-Limitless를 사용한다. 월드는 실시간 Top-Down 이동, 전투는 턴제이며 접근성을 주요 설계 원칙으로 둔다.

현재 캐릭터 생성, 마을·필드 이동, 전투와 직업 스킬, 길 전투 특성, 동료 파티, 로컬 저장, Main01~10 퀘스트가 구현되어 있다. 구현·검증·남은 확인 사항은 [CURRENT_STATUS.md](문서/00_프로젝트/CURRENT_STATUS.md)를 따른다.

## 폴더

- `Unity/Client/`: Unity 클라이언트 프로젝트와 프로젝트 전용 C#·Scene·Asset
- `문서/`: 세계관, 직업, 스킬, 전투, 저장 등 설계와 구현 기준
- `아트/`, `오디오/`: 제작 자료
- `Tools/`: 개발과 검증 도구

개발 규칙은 [AGENTS.md](AGENTS.md), 장기 설계 원칙은 [PROJECT_CONTEXT.md](문서/00_프로젝트/PROJECT_CONTEXT.md), 실제 전투 스킬 기준은 [스킬 문서](문서/06_스킬/README.md)를 참고한다.

## 담당 세션 시작 / 공통 운영

새 ChatGPT 담당 세션에서 **“너는 ○○ 담당이다. 깃허브에서 공부하고 와.”** 라고 요청하면 [팀 시작 안내](문서/00_프로젝트/팀운영/START_HERE.md)에서 역할을 찾고 [공통 규칙](문서/00_프로젝트/팀운영/COMMON_RULES.md), 담당별 행동요령, 최신 [CURRENT_STATUS](문서/00_프로젝트/CURRENT_STATUS.md)를 확인하도록 운영한다.

담당: **기획 / Codex·Unity 구현 / TTS·더빙 / 공식 스프라이트 아트 / 월드 리뉴얼 아트 / BGM / SFX / 개발기록 영상**. 모든 담당은 **한국어 존댓말**, 정확한 완료·검증 구분, 긴 세션의 사전 보고 및 [후임 인계](문서/00_프로젝트/팀운영/SESSION_HANDOFF.md)를 따른다.
