# Project-Limitless

**LIMITLESS**는 Unity 6로 개발 중인 2D 턴제 RPG다. 저장소와 C# namespace에는 내부 이름 Project-Limitless를 사용한다. 월드는 실시간 Top-Down 이동, 전투는 턴제이며 접근성을 주요 설계 원칙으로 둔다.

현재 캐릭터 생성, 마을·필드 이동, 전투와 직업 스킬, 길 전투 특성, 동료 파티, 로컬 저장, Main01~10 퀘스트가 구현되어 있다. 구현·검증·남은 확인 사항은 [CURRENT_STATUS.md](문서/00_프로젝트/CURRENT_STATUS.md)를 따른다.

## 폴더

- `Unity/Client/`: Unity 클라이언트 프로젝트와 프로젝트 전용 C#·Scene·Asset
- `문서/`: 세계관, 직업, 스킬, 전투, 저장 등 설계와 구현 기준
- `아트/`, `오디오/`: 제작 자료
- `Tools/`: 개발과 검증 도구

개발 규칙은 [AGENTS.md](AGENTS.md), 장기 설계 원칙은 [PROJECT_CONTEXT.md](문서/00_프로젝트/PROJECT_CONTEXT.md), 실제 전투 스킬 기준은 [스킬 문서](문서/06_스킬/README.md)를 참고한다.
