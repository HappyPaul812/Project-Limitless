> 최신 자동 시작 승인안은 부팅 전용 `Boot_Relay_PreRegistration_20261024.md`입니다. 아래 OnLogon 방식은 이력으로 보존하며 활성화하지 않습니다. 부팅 전용 프로필은 MANUAL·AI HOLD입니다.

# 운영 모드 A·B 사용 안내

날짜·시각은 Asia/Seoul입니다. 기존 `/resume role` 수동 정지 해제는 `/unpause role`로 이동했습니다. 새 `/resume`은 A 모드 선택이며 실행 승인을 발급하지 않습니다.

- 조회: `/limitless mode`
- A: `/limitless resume` 또는 `/limitless mode value:AUTO_RESUME`
- B: `/limitless mode value:SAFE_SHUTDOWN`, 이어 `/limitless finish until:2026-10-24 23:00 cleanup_minutes:10 scope:승인된 읽기전용업무정리`
- 즉시 마감 점검: `/limitless finish schedule_id:SCH-...`
- 예약 확인: `/limitless schedule-status`
- 공식 사용량: `/limitless usage`
- 취소: `/limitless cancel schedule_id:SCH-... release_intake:true`
- 수동 복귀: `/limitless mode value:MANUAL`

A에서 B로 바꾸면 A 재개가 차단되고 기존 체크포인트는 남습니다. B 예약이 남은 상태에서는 A 선택이 거부됩니다. 예약을 취소한 뒤 A로 전환하세요. 실행 중 업무가 있으면 모드 변경이 거부됩니다. 동시 작업은 기존 최대2, 자동 재개 큐 실행은 최대1입니다.

A는 공식 Codex App Server의 account/rateLimits/read만 사용하며 5시간·주간 소진 후 최신 관측을 다시 요구합니다. 조회 실패는 WAITING_FOR_QUOTA_VERIFICATION, 최대3회 조회 실패 후 회로 차단입니다. 모델 재시도는 하지 않습니다. 한국 시간 일요일에도 기존 사전 승인 업무와 A 모드의 사용량 회복 재개를 허용합니다. MANUAL은 대표의 명시적 활성화 전까지 유지합니다. 격리 파일 보관은 제품 적용 승인이 아닙니다.

SQLite operation_checkpoints에 업무 ID·역할·범위·기준SHA·단계·남은처리·원인을 보존합니다. 기존 승인 기간을 연장하지 않습니다. 소진 중 진행중인 최대90초 단발 추론은 강제 종료하지 않고 안전 경계에서 결과를 보존합니다. 부분 응답을 새 AI 호출로 이어 쓰지 않습니다. 소비·만료·결과불명 슬롯은 재개하지 않습니다. 재개 직전에 main 및 입력 정본 ref SHA를 읽고 불명·변경은 영속 HOLD합니다. 새 기준 승인/재계획은 별도 절차입니다.

B의 마감·보류·보고는 연결됐지만 실제 Git 통합/동기화 실행기는 연결하지 않았습니다. 현재 범위 기록은 통합 승인으로 취급하지 않으며 NO_INTEGRATION_APPROVAL입니다. Push·merge·백업/통합 실행에는 작업별 별도 승인이 필요합니다. 동기화·보호·Unity/Codex/복사 유휴·미저장 사용자 작업을 신뢰할 수 있게 검증하지 못하면 종료 HOLD입니다. 실제 종료 실행기는 비활성입니다. 종료 취소 의사는 기존 장부에 보존하며 강제 종료 명령은 없습니다.

Windows 자동 시작은 기존 operations-service.ps1 비활성 작업 XML 설계만 유지합니다. 실제 작업 등록/활성화·PC 종료는 별도 승인입니다. PC가 꺼져 있으면 실행할 수 없으며 다시 켜서 중계 서버를 시작하면 SQLite를 복원합니다. RUNNING 중 끊긴 추론은 NOT_VERIFIED로 남고 자동 재시도하지 않습니다. 실제 재부팅·한도 회복 후 모델 재개·PC 종료는 이번에 검증하지 않았습니다.

구독 사용량과 유료 API 비용을 분리하며 이번 구현·시험의 추가 모델 호출 및 유료 API 호출은 0회입니다. 아트 제작 → Codex 검수 → 바울 대표 미술 승인 → 승인 범위 적용 원칙을 유지합니다.
