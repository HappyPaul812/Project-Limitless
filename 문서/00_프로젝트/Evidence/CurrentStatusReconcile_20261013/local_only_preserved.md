
---

## 2026-10-10 개발환경 — Unity 6.6.5f1 MCP 복구

사용자 업그레이드 `6000.6.5f1` 확인. 최초 서버 로그의 `uvx` 명령 미발견으로 HTTP/WebSocket 실패; 기존 실행 스크립트에 DryRun·uvx 검사·UV_PYTHON·Editor 중복 실행 방지 추가. Python3.12.14/uv0.12.18/MCP10.2.0 유지, 기존 바로가기 정확 인수 DryRun Exit0. 사용자가 닫은 Editor를 명시적 허락 후1회 실행해 HTTP8080/Plugin35도구/MCP 상태·Scene·Console 왕복 PASS. Bootstrap clean EditMode·포커스false·컴파일/Import 종료·현재 Error0/Warning0. 게임 보호3457파일 SHA 동일·Save/설정 동일·기존 Git변경 보존, 게임/아트/음성 변경0·Play0·화면조작0·commit/push0.

최초 업그레이드 로그 C#Error0/CS0618 20건(FindObjects계열18·spritesheet2)/구형Importer경고3 기록; 이번에 API/meta 대량 수정하지 않음. [정확한 원인·검증·경고·보호 보고서](Unity_665_MCP_Recovery_20261010.md). 다음: 기존 개발 시작 바로가기 사용, 별도 Unity6.6 Main20/SideQuest Runtime 회귀. 네이티브 Codex 도구 표시 갱신과 게임전체 호환은 미검증. 시작 관련HEAD `485138b`, 이번 변경 미커밋.
