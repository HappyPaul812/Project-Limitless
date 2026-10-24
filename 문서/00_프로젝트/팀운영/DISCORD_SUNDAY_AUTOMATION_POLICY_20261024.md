# Discord AI 협업 — 주일 승인 업무 정책 적용·검증

2026-10-24 KST, 바울 대표 직접 지시. 기존 codex/discord-ai-collaboration 실행기를 이어서 수정하며 원본 게임·유료 API·Push·main 병합은 범위 밖입니다.

## 확정 운영 규칙

1. 바울 대표의 주일 휴식과 AI 시스템의 실행 가능 여부를 분리한다. Asia/Seoul 일요일에도 사전 승인된 업무를 실행할 수 있다. 기존 주일 일괄 작업 금지와 충돌하면 이 지시를 우선한다.
2. A 모드가 명시적으로 활성화된 경우 사용량 회복 후 자동 재개에도 일요일을 허용한다. 사용량 회복은 실행 모드·승인·한도 검사를 생략하는 권한이 아니다.
3. 기존 MANUAL 모드는 바울 대표의 명시적 활성화 지시 전까지 유지한다. 이번 정책 변경은 A 모드 활성화 지시가 아니다. 재부팅·재연결·사용량 회복으로 MANUAL을 자동 변경하지 않는다.
4. 승인된 범위와 산출물만 실행한다. 새 작업 범위와 미승인 기획 변경은 승인 대기한다. 승인 근거를 확인할 수 없는 업무는 자동 실행하지 않는다.
5. 아트는 Codex 검수 후에도 바울 대표의 최종 미술 승인을 기다린다. 검수 통과는 최종 미술 승인이 아니다. 미승인 아트의 Unity 본편 적용은 금지한다.
6. GitHub Push와 main 병합은 기존 별도 승인 규칙을 유지한다. 자동 실행 허용을 Push·병합 승인으로 해석하지 않는다.
7. 기존 AI 호출 횟수·사용량·동시 실행 제한을 유지한다. 유료 API로 자동 전환하지 않는다. 한도 수치를 확인하기 전 임의의 기본값을 만들거나 무제한으로 실행하지 않는다.
8. 승인 대기 항목은 그 의존 업무만 막는다. 다른 독립적이고 사전 승인된 큐 항목은 모드·한도·중복 실행 검사 후 계속 처리할 수 있다.
9. Discord 보고에서 완료·보류·승인 필요 사항과 근거·산출물·다음 조치를 구분한다. 바울 대표가 복귀했을 때 확인할 수 있게 남긴다. 비밀키·토큰은 보고하지 않는다.
10. 부팅 자동 시작과 USB Wi-Fi 재연결 기능을 보호한다. 해당 기능의 서비스 시작과 AI 업무 자동 실행 권한은 별개다.

## 현재 운영 상태와 적용 경계

- 실제 SQLite: MANUAL, 업무21/usage13/승인0/재개큐0, RUNNING 업무0. 모드·승인·업무·usage·한도 설정은 변경하지 않습니다.
- 현재 Bot/Art heartbeat RUNNING. Bot은 ai_worker_enabled=false, approved_readonly_resume_only=false입니다. 부팅 전용 Bot은 usage monitor·ResumeQueue가 연결되지 않고 /resume과 A/B 전환을 거부합니다. 따라서 현재는 AI 자동 실행 불가이며 MANUAL 유지와 별개인 실행기 연결 제한입니다.
- 주일 일괄 차단만 core/phase2/real_roles/scheduling 네 파일에서 제거합니다. 신규 호출 승인·유료 전환·새 제품 적용 권한을 만들지 않습니다. 승인 기한600초, 정확한 plan SHA/role/slot, 사용량300초 신선도, 총동시2/재개1, 90초 실행 한도를 유지합니다.
- 변경 파일은 다음 정상 시작 시 로드합니다. 현재 서비스를 중단/재시작하지 않았으므로 현재 메모리에 새 코드가 반영됐다고 주장하지 않습니다. 단순 재시작도 boot relay의 AI HOLD를 해제하지 않습니다.

## 실제 실행 경로 대조와 부팅 보호

저장된 관리자 실측 admin-task-verification.json에는 OnBoot2종 모두 pwsh → C:/ProgramData/LimitlessRelay/boot-service.ps1 → -ToolRoot 이 worktree의 Tools/DiscordAICollab 경로가 기록돼 있습니다. 현재 C: boot-approval.json의 tool_root도 같은 경로입니다. bootstrap → collab.boot_relay → register-discord.ps1 -BootRelay → 기존 Bot/Art·단일 SQLite 경로를 코드로 대조했습니다.

이번 Windows 실시간 Get-ScheduledTask 조회는 대상 반환 실패, schtasks는 network address invalid, System32/Tasks 원문은 접근 거부였습니다. 다른 계정 프로세스의 CommandLine도 null입니다. 이를 실제 등록 부재로 단정하지 않으며 **현재 태스크 원문·현재 프로세스 명령줄의 직접 검증은 미완료**입니다. 저장된 관리자 검증과 현재 heartbeat/승인 파일을 구분합니다. 기존 verify-boot-tasks.ps1을 관리자 PowerShell에서 -StartTest 없이 실행하면 조회 기록을 갱신할 수 있습니다. 작업 재설치·재등록은 하지 않습니다.

부팅 bootstrap·wrapper·supervisor·Wi-Fi 연결 대기/재연결·비밀 저장소·아트 수령 코드는 변경하지 않습니다. 승인 파일 runtime_sha256의 변경4파일 항목만 갱신하며 ACL/소유자·kind·tool_root·service_sid·bootstrap/wrapper/supervisor 해시는 보존합니다. 새 소스와 승인 SHA가 불일치한 상태로 마감하지 않습니다.

## 큐와 미술 승인

기존 큐는 APPROVAL_PENDING/REVIEW_PENDING 업무를 RUNNING 동시 슬롯으로 세지 않고, 역할 정지·승인 만료·정본 변경으로 막힌 재개 항목을 건너뛰어 독립 승인 업무를 처리합니다. 이번에는 큐를 재구현하지 않고 격리 테스트로 확인합니다. 아트 기술 검수와 바울 대표 미술 승인을 분리하며 제품 적용/Push/main 병합 실행 금지는 그대로입니다. 새 승인 또는 기존 승인 유효기간 연장0.

## A 모드 안전 실운영 전 미검증 조건

1. 현재 실제 OnBoot 태스크 원문과 실행 프로세스 경로를 관리자 읽기 조회로 재확인.
2. 서비스 계정의 Codex 구독 인증·F:와 격리 납품함 접근·공식 account/rateLimits/read 검증. 현재 계정의 인증을 서비스 계정 인증으로 간주하지 않음.
3. BootRelay AI HOLD를 보호하면서 별도 승인 범위에 맞는 AI 실행 경로를 결정하고 검증. 현재 A는 기존 읽기 전용 1회 검토 재개이며 일반 개발/Unity 검수 worker는 연결되지 않음. 범용 자동 개발 가능으로 보고 금지.
4. 새 유효한 정확한 계획 승인·미소비 슬롯·현재 정본 SHA·공식 사용량 최신 관측을 확보. 기존 승인 소진을 이번 정책으로 재발행하지 않음.
5. 격리 가짜 CLI 결과와 별개로 실제 한도 소진→회복→공식 재관측→승인1회 재개 및 Discord 완료/보류/승인 보고 확인. 현재 실호출 검증0.
6. 실제 PIN 전 부팅·USB Wi-Fi 단절/복구 실증은 별도 영향 안내·승인 후 확인. 이번 재부팅·망 단절·화면 조작0.
7. 위 조건 확인 후 바울 대표의 명시적 A 활성화 지시가 필요. 이번 지시는 MANUAL 유지이며 정책 허용만으로 모드 전환하지 않음.

## 검증과 증거

핵심 주일11을 포함한 격리28 PASS/0 FAIL. Discord 보고 SENT(영수증1563350905108168806). 구현 commit9cf84605. 추가 회귀 결과·배포 SHA·보호 파일·DB 지문·Discord 영수증은 Evidence/DiscordSundayPolicy20261024/summary.json 및 regression-tests.json에 기록합니다. 초기 테스트 명령이 원래 모듈을 잘못 로드하고 sandbox TEMP 권한 오류를 낸 것은 검증 실행기 문제였으며, 격리 경로·저장소 내 TEMP·명시적 테스트 목록으로 수정했습니다. 초기 실패를 제품 통과 근거에 포함하지 않습니다. 추가 비동기 시험의 sandbox socketpair 대기도 faulthandler로 확인하여 해당 테스트 프로세스만 정리한 뒤 같은 격리 시험을 제한 밖에서 재검증했습니다.

현재 프로세스는 계속 MANUAL/AI HOLD이며 실제 AI 호출·유료 API·Unity·제품 적용·Push·병합0. 서비스 재설치·작업 등록 변경0. 기준 commit47288310. 완료 commit은 codex/discord-ai-collaboration 브랜치 로그 참조.
