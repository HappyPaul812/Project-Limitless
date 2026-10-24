# 서비스 계정 A 실행기 연결 준비·검증 — 2026-10-24

현재 판정: **연결 코드 준비/디스크 반영 완료, 실제 서비스 인증 NOT_VERIFIED, MANUAL/AI HOLD 유지**.

기존 boot relay가 동일 Bot/Art lock·단일 SQLite를 유지하는 경로에 서비스 SID/일반계정/자기 프로필 확인과 공식 읽기 검증을 추가했습니다. 성공 시 기존 UsageMonitor/ResumeQueue/run_once를 재사용하고, 실패 시 중계만 유지합니다. 네 역할 공용 VERIFIED 실행기·승인 슬롯·큐·체크포인트 원본은 변경하지 않았습니다. 실제 서비스 재시작·새 Bot·아트·AI 호출0.

변경분 격리14 PASS + 추가 Bot 연결2 PASS, PS구문 오류0. 기존28/전체QA 재실행0. 부팅 승인39 SHA 일치 및 ACL/소유자 보존. 기존업무21/usage13/설정28/승인0/재개큐0·MANUAL 지문 동일, 체크포인트14 유지. 부팅 wrapper/supervisor/아트/실행기/큐 등 보호7파일 동일.

실제 서비스 계정 단발 점검 완료: request_id4858235fe10c40bea75ccc55f35521df, SERVICE_CODEX_PROFILE_NOT_INITIALIZED HOLD. 서비스 SID/일반계정/자기 프로필 검사 후 초기화된 .codex 프로필을 확인하지 못해 공식 인증·사용량 조회를 시작하지 않았습니다. 구독 인증 성공/실패 자체와 CLI 접근은 이후 단계 미검증이며 서비스 계정의 5시간/주간 회복 시각은 UNKNOWN입니다. 다른 계정 비밀 복사·임의 로그인/설치·모드 변경·AI 호출0. 최초 Action 호출 안내 문제는 pwsh -File 방식으로 실제 서비스 점검까지 진행했습니다.

사용량 회복 재관측·소진 양쪽 창·checkpoint RUNNING 재시도금지·기준SHA 변경·만료 승인의 HOLD를 선별 검증했습니다. 실제 한도회복 모델 실행은 미검증입니다. 기존 승인 TTL600초는 연장하지 않으며 소진 중 만료되면 재승인이 필요합니다.

[로컬 단발 점검·첫 읽기1건 절차](../../../Tools/DiscordAICollab/SERVICE_AI_BRIDGE_KO.md), [검증 근거](../Evidence/ServiceAIBridge20261024/summary.json).

다음: 실제 서비스 계정 프로브 → 인증 불가면 HOLD/필요한 로컬 설정 보고 → 성공 시 기존 서비스 정상재시작·동일 lock/heartbeat 확인 → 최신 정본의 새 1건 계획/슬롯 승인 → 대표 명시적 A 활성화 → 승인된 읽기1건 실증. 이번에는 새 승인·슬롯·실행큐 생성0. 원본게임/Save/미승인아트제품적용/유료API/Push/merge0, Unity 사용자 확인 없음.

기준 commit47267df5. 이번 관련 commit은 codex/discord-ai-collaboration의 서비스 연결 구현/마감 이력을 참조합니다. 실제 부팅/USB재연결은 이전 보고의 NOT_VERIFIED를 유지하며 이번 요청에서 정상으로 재판정하지 않습니다.

구현 commit94a0f739. 실제 서비스 계정 결과는 [service-probe.json](../Evidence/ServiceAIBridge20261024/service-probe.json)에 저장했습니다. 인증 준비 불가 시 HOLD하라는 지시에 따라 이번에는 여기서 AI 실행을 차단합니다. 다음은 별도 서비스 계정의 정식 Codex 프로필/구독 로그인 준비 및 필요 시 CLI 접근 확인 → 단발 읽기 점검 재확인 → 정상 코드 로드 → 새 1건 계획/슬롯 승인 → 대표 A 활성화입니다. 기존 계정 비밀 복사는 대안으로 사용하지 않습니다.
