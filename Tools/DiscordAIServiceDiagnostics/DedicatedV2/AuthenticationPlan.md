# 전용 Worker 인증 계획 — 현재 로그인 금지

현재 후보는 EMPTY_NO_IMPORT_NO_LOGIN/FAKE_ONLY이며 CODEX_HOME은 새 State/Auth/CodexHome입니다. 기존 Worker·관리자 profile/auth.json/keyring/환경 인증을 가져오지 않습니다. fake 시험에는 실제 구독 인증이 필요하지 않습니다.

공식 OpenAI 인증 문서 https://learn.chatgpt.com/docs/auth 는 기기 코드 방식 `codex login --device-auth`와 CODEX_HOME의 파일 저장 또는 사용자별 credential store를 설명합니다. 실제 허용 여부는 계정/워크스페이스 설정에 달려 있습니다. 이 문서는 기능 근거이며 아래 보안 절차는 프로젝트의 별도 설계입니다.

후속 인증 승인을 받는 경우:

1. 대표가 새 Worker 계정/공식 CLI 버전·SHA·인증 목적/선택 구독을 별도 승인합니다. 설치·fake IPC 승인과 분리합니다.
2. 새 SID의 OS 생성 profile과 전용 CODEX_HOME을 검사합니다. 인증 디렉터리는 BA/SY/새 Worker만 접근, Bot/기존 Worker 접근 없음. credential store=file 선택은 관리 정책이 허용할 때만 설정하며 강제 keyring/워크스페이스 정책을 우회하지 않습니다.
3. interactive/RDP/network/service 로그온 deny를 풀거나 runas interactive로 우회하지 않습니다. 별도 승인된 관리자 통제 Batch enrollment launcher/단발 Task 설계가 필요합니다. 이 도구는 아직 제공/실행하지 않습니다.
4. 기기 코드·인증 URL은 대표의 로컬 보안 화면에서만 표시하고 에이전트/Issue/로그/파일로 수집하지 않습니다. 브라우저 승인은 대표가 수행하며 장치 코드를 공유하지 않습니다. 비밀없는 완료 여부만 기록합니다.
5. 기존 auth cache 복사는 사용하지 않습니다. 실패는 HOLD, 자동 재로그인·API key 우회·동일 승인 재사용0.
6. 현재 FAKE_ONLY 후보는 인증 저장소가 비어 있어야 합니다. 인증을 채운 뒤 이 후보를 그대로 운영하지 않고 별도 auth 정책/Manifest/승인을 검토합니다.

같은 ChatGPT 구독의 한도·다른 PC/앱/클라우드의 실행은 Windows SID로 분리되지 않습니다. 실제 AI 모드는 이 후보에 없으며 전역 조정·구독 권한/한도 검증과 별도 승인 전 HOLD입니다. 공식 사용량 조회·로그인·모델 호출은 이번0입니다.
