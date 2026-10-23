# Phase2 실연결 준비·제한 검증 착수 계약

바울 대표의 이번 지시로 de619848 파일럿을 이어갑니다. 기존 큐/승인/검역을 재작성하지 않고 별도 codex/discord-ai-collaboration worktree에서 운영 도구와 문서만 보완합니다.

- Discord 계정·bot·토큰은 바울 대표가 직접 설정합니다. 코드가 토큰을 요구하거나 대화/로그에 출력하지 않습니다. 미설정 시 WAITING_FOR_USER_SETUP, 외부 연결0.
- 공식 CLI의 기존 ChatGPT 로그인 사용, 고정 짧은 입력1건만 승인된 것으로 처리합니다. API 키 전환0, 애플리케이션 재시도0, 사용자 설정/셸·앱·브라우저·MCP·웹검색 비활성, 빈 작업공간·read-only 실행. 토큰 파일은 읽거나 복사하지 않습니다.
- 실제 호출 직전에 영속 단발 장부를 확정합니다. 실패·결과불명도 허용1회를 소비하며 재실행하지 않습니다. 실제 모델 호출 수는 CLI 실행1회/완료 turn 수/사용량 이벤트를 구분합니다.
- CLI 원문 stdout/stderr·추론·계정 정보는 보고에 저장하지 않고 허용된 완료표식/숫자 사용량/오류 분류만 보존합니다. 계정 전체 사용률은 정확한 단일 호출 비용으로 환산하지 않습니다.
- 결과는 같은 Store 규격의 전용 로컬 업무 큐에 연결합니다. 실제 Discord 왕복 확인은 설정 후 별도입니다. 추론 성공을 미술 승인으로 사용하지 않습니다.
- 아트→Codex 검수→바울 대표 최종 승인→승인범위 내 적용 순서 유지, Push/main병합 별도 승인. 제품/Unity/Save 변경0·SFX PAUSED·기존 Unity 전수 QA 재실행0.

기존57 테스트와 구분해 이번 변경분의 전송·권한·설정누락·단발·APIfallback차단·사용량 파싱을 검사합니다. 실제 서버 연결/명령9종/비소유자 거부가 미실행이면 NOT_VERIFIED로 보고합니다.

실행 후 확인된 제약: 설치 CLI는 reserved built-in openai provider 설정 덮어쓰기를 금지합니다. 최초 단발 실행의 retry=0 override가 bootstrap에서 거부되어 완료turn0이며 재호출하지 않았습니다. 해당 override를 제거한 준비안만 추론 없는 features list로 확인합니다. 후속 승인이 있다면 CLI1회/90초 상한을 유지하되 built-in의 유한 transport retry 기본값은 남습니다. API endpoint/provider를 우회하거나 자동 업그레이드하지 않습니다.
