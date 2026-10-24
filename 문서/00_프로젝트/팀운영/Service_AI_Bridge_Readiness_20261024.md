# 서비스 계정 A 실행기 연결 준비·검증 — 2026-10-24

현재 판정: **연결 코드 준비/디스크 반영 완료, 실제 서비스 인증 NOT_VERIFIED, MANUAL/AI HOLD 유지**.

기존 boot relay가 동일 Bot/Art lock·단일 SQLite를 유지하는 경로에 서비스 SID/일반계정/자기 프로필 확인과 공식 읽기 검증을 추가했습니다. 성공 시 기존 UsageMonitor/ResumeQueue/run_once를 재사용하고, 실패 시 중계만 유지합니다. 네 역할 공용 VERIFIED 실행기·승인 슬롯·큐·체크포인트 원본은 변경하지 않았습니다. 실제 서비스 재시작·새 Bot·아트·AI 호출0.

변경분 격리14 PASS + 추가 Bot 연결2 PASS, PS구문 오류0. 기존28/전체QA 재실행0. 부팅 승인39 SHA 일치 및 ACL/소유자 보존. 기존업무21/usage13/설정28/승인0/재개큐0·MANUAL 지문 동일, 체크포인트14 유지. 부팅 wrapper/supervisor/아트/실행기/큐 등 보호7파일 동일.

실제 현재 계정의 프로브는 SERVICE_ACCOUNT_CONTEXT_REQUIRED HOLD입니다. 서비스 계정의 로그인 저장소를 대신 읽거나 인증을 복사하지 않았습니다. 관리자 로컬 단발 점검 도구를 준비했고 바울 대표에게 로컬 실행을 요청했습니다. 이 작업은 별도 일반계정 배치 로그온을 위한 Windows 암호 입력이 필요하므로 Codex가 다른 사용자 문맥으로 우회 실행하지 않습니다. 현재 서비스 계정의 5시간/주간 회복 시각은 UNKNOWN입니다. 공유DB의 오래된 공식관측81건을 현재 서비스 계정 결과로 대체하지 않습니다.

사용량 회복 재관측·소진 양쪽 창·checkpoint RUNNING 재시도금지·기준SHA 변경·만료 승인의 HOLD를 선별 검증했습니다. 실제 한도회복 모델 실행은 미검증입니다. 기존 승인 TTL600초는 연장하지 않으며 소진 중 만료되면 재승인이 필요합니다.

[로컬 단발 점검·첫 읽기1건 절차](../../../Tools/DiscordAICollab/SERVICE_AI_BRIDGE_KO.md), [검증 근거](../Evidence/ServiceAIBridge20261024/summary.json).

다음: 실제 서비스 계정 프로브 → 인증 불가면 HOLD/필요한 로컬 설정 보고 → 성공 시 기존 서비스 정상재시작·동일 lock/heartbeat 확인 → 최신 정본의 새 1건 계획/슬롯 승인 → 대표 명시적 A 활성화 → 승인된 읽기1건 실증. 이번에는 새 승인·슬롯·실행큐 생성0. 원본게임/Save/미승인아트제품적용/유료API/Push/merge0, Unity 사용자 확인 없음.

기준 commit47267df5. 이번 관련 commit은 codex/discord-ai-collaboration의 서비스 연결 구현/마감 이력을 참조합니다. 실제 부팅/USB재연결은 이전 보고의 NOT_VERIFIED를 유지하며 이번 요청에서 정상으로 재판정하지 않습니다.
