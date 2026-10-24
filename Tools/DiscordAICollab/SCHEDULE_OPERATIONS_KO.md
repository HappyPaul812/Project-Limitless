> 2026-10-24 후속: 새6종 명령 실연결과 공식 App Server 자동 사용량 조회를 완료했습니다. 현재 운영은 [최신 안내](OPERATIONS_LIVE_KO.md)를 따릅니다. 아래 모의 단계의 미연결 표기는 당시 기록이며 실제 PC 종료와 자동 시작 등록은 계속 비활성입니다.

# 예약·사용량 보류·조건부 종료 모의 운영 안내

2026-10-23 / Asia/Seoul / 모드 `SIMULATION_ONLY`

## 현재 범위

기존 Discord 봇·SQLite·네 AI VERIFIED·아트 납품 경로를 보존합니다. 예약 게이트·한국어 명령 6개·영속 보류·종료 조건 모의 평가를 추가했습니다. 실제 AI·유료 API·제품 변경·Git 통합·컴퓨터 종료는 실행하지 않습니다.

이번 단계에서는 실행 중인 봇을 재시작하거나 새 Slash 명령을 Discord에 등록하지 않았습니다. 아래는 **새 코드 활성화 이후의 조작 방법**입니다. 현재 기존 9개 명령 운영은 유지하며, 새 6개 명령의 실제 서버 응답은 NOT_VERIFIED입니다. 사용자 서버에서 등록·실연결 확인은 별도 활성화 단계입니다.

## 명령

모든 명령은 기존 대표 숫자 ID·서버·허용 채널·사람 계정 여부를 확인하고 응답을 본인에게만 표시합니다.

| 명령 | 입력 예 | 동작 |
|---|---|---|
| `/limitless schedule` | until:`2026-10-24 23:00` cleanup_minutes:`10` | 22:50 접수 중단, 23:00 마감 |
| `/limitless schedule` | until:`2026-10-24 22:00` schedule_id:`SCH-...` | 현재 ID를 지정한 예약 수정 |
| `/limitless schedule-status` | 없음 | 예약 ID·상태·다음 확인 시각 |
| `/limitless cancel` | schedule_id:`SCH-...` | 해당 예약/모의 종료 요청 취소 |
| `/limitless usage` | 없음 | 5시간·주간 사용률/회복 시각/관측 시각·확인 불가 안내 |
| `/limitless finish` | schedule_id:`SCH-...` | 신규 접수·실행 차단, 실행 중/완료/대기 업무 기록 |
| `/limitless shutdown` | schedule_id:`SCH-...` confirm:`SCH-...` | 해당 예약의 PC 종료 의사 모의 확인; 항상 실제 종료 HOLD |

`HH:MM`, `오늘 HH:MM`도 지원하며 이미 지난 시각을 내일로 임의 이월하지 않습니다. 종료 ID 재확인은 포괄적인 PC 종료 허가나 통합 허가가 아닙니다. 자연어 문장 전체를 모델로 해석하지 않습니다. “오늘 23시까지”는 `until:23:00`으로 명시합니다.

정리 기본 10분(허용 2~120분)입니다. 90초 실제 CLI 또는 300초 lease 작업이 정리 시작 전에 끝나지 않으면 새 실행을 차단합니다. 미실행 업무는 원래 상태·승인·evidence와 함께 DB에 남고 별도 `control_holds`에 표시됩니다. 실행 중 결과 수신·저장은 허용하되 강제 종료/성공 위장은 하지 않습니다. 실제 CLI의 기존 90초 한도·동시 2개·소비된 1회 슬롯 재실행 금지를 유지합니다.

예약 취소는 기존 수동 정지·역할 정지·미술 HOLD를 풀지 않습니다. 이미 정리가 시작된 예약을 취소하면 `CANCELLED_HOLD`로 유지하므로 새 작업을 시작하려면 새 예약을 명시적으로 설정해야 합니다. 반복 전송은 이벤트 영수증으로 중복 등록을 막습니다.

`next_start`는 마감 이후의 **다음 확인 시각**을 저장합니다. 주일 시각도 허용하며 기존 승인·모드·마감·사용량 검사는 유지합니다. 현재 외부 공식 사용량 자동 조회 브리지가 없고 다음 작업 마감·실행 승인도 별도이므로, 시각만 지났다고 작업을 자동 재개하지 않습니다. 봇/고정 tick은 재검증 상태를 제공하며 실제 재개는 새 유효 예약·새 공식 관측·기존 미소비 승인 모두 확인 후 별도 실행 단계입니다. 자동 재시도 0회이며 UNKNOWN/결과불명/소비된 슬롯은 자동 재시도하지 않습니다.

## 공식 사용량 확인

공식 Codex 앱 `get_usage_limits` 도구로 300분과 10080분 창을 직접 확인했습니다. 이 도구는 현재 Codex 앱의 도구이며 **Python Discord 봇에 공개 API로 제공된다고 가정하지 않습니다**. 임의 HTTP endpoint·인증 파일·쿠키 스크래핑을 사용하지 않습니다. 공식 [사용량 안내](https://developers.openai.com/codex/pricing)는 대시보드와 CLI `/status`를 안내합니다.

`Scheduler.record_official_usage`는 신뢰되는 로컬 세션에서 공식 결과의 출처·관측 시각·일반 사용 허용 여부·두 창의 사용률·회복 시각만 받는 브리지입니다. Discord로 숫자를 입력받거나 일반 JSON 파일을 자동 신뢰하지 않습니다. 계정 ID·토큰·재설정권·크레딧 정보는 저장하지 않습니다.

관측 후 5분이 지나거나 값/일반 사용 허용을 확인할 수 없으면 UNKNOWN/HOLD입니다. 한도가 소진되면 더 늦은 관련 회복 시각을 기록합니다. 시각 경과는 `RECHECK_REQUIRED`이며 **새 공식 관측 전까지 계속 보류**합니다. 봇의 외부 자동 관측·무인 자동 재개는 이번 단계 미연결입니다. 설정한 관측 기한은 서비스 안전 정책이며 OpenAI가 보장하는 캐시 유효 시간이 아닙니다.

유료 API 자동 전환·크레딧 구매·재설정권 사용 기능은 없습니다. 이번 외부 AI 실행기 호출은 0회입니다. 이 개발 대화 자체의 구독 사용량과 외부 자동 AI 실행 횟수는 구분합니다.

## Windows 공식 예약 방식

`export-safety-task.ps1`는 Windows ScheduledTasks 공식 cmdlet으로 **미등록 XML**만 생성합니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\export-safety-task.ps1" -At "2026-10-24 20:00"
```

출력 `data/safety/task-preview.xml`은 예시 시각의 점검 정의이며 등록하지 않았습니다. 고정 `.venv/Scripts/python.exe -m collab.safety_tick`만 참조하고 임의 명령/인수를 받지 않습니다. 현재 사용자 InteractiveToken·최소 권한·중복 IgnoreNew·강제종료 비허용·1분 실행 한도입니다. tick은 예약 장부와 상태 파일을 갱신할 뿐 AI/네트워크/Git/종료를 실행하지 않습니다. SQL 쓰기는 트랜잭션과 원자적 상태 파일 교체를 사용합니다.

향후 별도 활성화 시 Windows `Register-ScheduledTask`로 이 고정 작업을 등록하고 SQLite 예약 변경/취소와 작업 정의를 함께 갱신해야 합니다. 현재 Discord 예약에서 OS 작업을 자동 등록하지 않습니다. 실행 중 봇의 5초 점검과 실제 시작 시 트랜잭션 게이트로 시각을 재확인합니다. 백그라운드 수령 서비스는 새 코드를 읽기 위해 별도 재시작이 필요하지만 이번에는 기존 서비스에 손대지 않았습니다.

**PC가 완전히 종료되면 자동 재개할 수 없습니다.** 사용자가 PC를 켜고 로그인한 뒤 서비스/봇을 실행해야 합니다. 원격 부팅·계정 자동 로그인·암호 저장을 구성하지 않습니다.

## 통합과 PC 종료

통합 승인 없음 → Git 동기화 HOLD입니다. 단순 마감 예약·미술 승인·기술 PASS로 Push/main 병합을 허가하지 않습니다. 실제 통합 어댑터도 이번에는 연결하지 않았습니다.

별도 통합 승인 후 구현할 실행 계약은 작업 ID·승인된 base/결과 SHA·파일 범위·대상 브랜치/PR·Push/merge 허용 여부를 묶어야 합니다. 원본 dirty 목록·파일 SHA·격리 백업을 먼저 보관하고, 별도 복제 환경에서 충돌·테스트·원격 HEAD 이동을 점검합니다. 보호 파일이나 미커밋 사용자 작업이 겹치면 HOLD입니다. 원본과 원격을 강제로 reset/덮어쓰기하지 않습니다. 동기화 결과와 보호 증거의 Discord 영수증을 확인하기 전에는 종료를 허용하지 않습니다.

`shutdown_preview`는 합성 근거에서 다음 조건을 모두 참으로 확인할 때만 WOULD_SHUTDOWN을 반환합니다: 해당 예약 대표 확인, 로컬 ID와 종료 권한, 승인 범위 완료, 명시적 통합 승인, SHA 동기화, 보호 파일, Unity/복사 종료, 미저장 작업 없음, 다른 예약 충돌 없음, Discord 최종 전송, 취소 가능. 근거는 30초 이내여야 하고 취소 시 즉시 HOLD입니다. 이 함수는 OS API를 호출하지 않습니다. Discord 요청은 외부에서 이런 참/거짓 근거를 받지 않으며 항상 로컬 검증 필요 HOLD입니다. 로컬 종료 권한과 미저장 사용자 작업을 실제로 확인하는 실행기는 미구현입니다.

향후 실제 종료에는 120초 이상 취소 유예·조건 재검증·대표/로컬 이중 확인·비강제 Windows API를 별도 검증해야 합니다. [Microsoft shutdown 문서](https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/shutdown)에 따르면 `/t` 양수는 `/f`를 암묵 적용하므로 이 구현에서는 shutdown.exe를 사용하지 않습니다. [InitiateShutdownW](https://learn.microsoft.com/en-us/windows/win32/api/winreg/nf-winreg-initiateshutdownw)의 강제 플래그 없는 방식은 후속 후보이며 이번에는 호출하지 않습니다.

아트 정책은 **아트 제작 → Codex 검수 → 바울 대표 미술 승인 → 승인 범위 내 게임 적용**, 이후에도 Push/main 병합 별도 승인입니다. 예약은 어떤 승인 상태도 변경하지 않습니다.
