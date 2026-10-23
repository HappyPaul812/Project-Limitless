# 운영 실사용 준비 최종 점검 — 2026-10-24 Asia/Seoul

판정: **MANUAL 조회 운영 준비 PASS / 무인 재개·통합·종료 실증은 조건부 보류**.
브랜치 `codex/discord-ai-collaboration`, 선별 검증 커밋 `996f8888`, 기존 구현 `1e7943933a124cf4a07d5a48d9cf4bfb9229b42b` 보존.
이번에는 실행 코드 수정·서비스 재시작·실제 AI 호출 없이 검사와 문서만 추가했습니다.

## 실제 Discord 및 A·B 점검

대표 계정 `/mode`, `/usage`, `/schedule-status` 3종 한국어 응답 및 MANUAL 유지 확인. 실제 Gateway·대표 ID·비공개 응답 영수증은 각각 `1563220451394588885`, `1563220770954682408`, `1563221094306156644`입니다. B 예약/취소/마감 등 기존 6종 실응답 성공은 보존하며 재실행하지 않았습니다. 이번 B 예약→마감 HOLD→취소는 격리 DB 합성 Discord 검사입니다.

A의 기존 서버 전환/전송 성공 영수증 `1563217994253860956`은 보존합니다. 이번 격리 callback에서도 A 응답·비공개 전송·영수증 저장이 PASS였습니다. 하지만 이전 화면 오류 원문이 없고 이번에는 MANUAL을 보존하므로 A를 실제로 다시 설정하지 않았습니다. **화면 오류 해결 확정 NOT_VERIFIED**, 원인 확인/수정 완료라고 선언하지 않습니다. 현재 세 조회 화면은 대표가 정상 확인했습니다.

A 보류·복원은 기존 공식 관측/기한/미소비 승인/SHA/동시수 검증을 사용합니다. 만료된 10분 승인을 자동 연장하지 않습니다. 이미 실행한 단발 추론은 중간 토큰부터 재호출하지 않습니다. 오래 소진된 뒤에는 새 실행 승인이 필요할 수 있습니다. 실제 사용량 회복 후 AI 재개는 미검증입니다.

B는 승인 범위 기록만 하며 통합 실행기는 연결하지 않습니다. 격리 시험의 scope는 TEST_ONLY이며 NO_INTEGRATION_APPROVAL로 유지됩니다. 실제 동기화 증거 없이는 SYNC_COMPLETE로 기록하지 않았습니다. 기존 원본/미커밋/Unity GUID/meta/Save/UserData/승인 아트를 수정하지 않았습니다. 불명확한 동기화·미저장 작업은 HOLD, 실제 종료 기능은 비활성입니다.

## 사용량·시간대

공식 Codex App Server 최신 관측(2026-10-24 01:01:15 KST): 5시간 79%, 회복 2026-10-24 03:23:53 KST / 주간 12%, 회복 2026-10-30 22:23:53 KST. 일반 구독 사용 허용 true. 회복 시각은 실행 허가가 아니며 그 이후 새 공식 관측·유효 승인·기준SHA를 재확인합니다. 주일 00:00 KST 경계에서 신규 업무 접수 차단을 합성 시계로 확인했습니다.

추가 역할 AI 호출0·입력/출력 토큰0·유료 API 호출0·추가 유료 API 비용0입니다. 현재 대화 자체의 구독 사용량과 역할 실행 비용을 혼동하지 않습니다. 공식 메타데이터 조회는 계속 정상 작동합니다.

## Windows 자동 시작 승인안

`LIMITLESS-Art-OnLogon`, `LIMITLESS-Bot-OnLogon` 두 작업 XML을 검토했습니다.

- 동일 DPAPI 사용자 로그인 30초 후, InteractiveToken, LeastPrivilege
- 계정명이 기록된 XML은 Windows SID로 정규화하여 principal·trigger 모두 현재 사용자와 일치 확인
- IgnoreNew, 서비스별 byte lock 및 30초 heartbeat로 기존 감시기 중복 시작 차단
- 실패 재시작 최대3회/1분, 강제 종료 금지, 작업 시간 제한0
- 현재 enabled=false, 승인 파일 없음, 실제 OS 등록0, wrapper/starter SHA 일치
- 토큰은 명령줄/XML/Git에 넣지 않으며 같은 사용자의 DPAPI·전용 ACL wrapper로만 사용
- Codex 인증 복사·설정 변경·API 키 전환 없음

예상 영향은 로그인 후 봇과 아트 감시 프로세스가 배경에서 시작되고 네트워크·로그 기록이 이어지는 것입니다. 기본 MANUAL이며 자동 시작 승인은 새 AI 호출·Unity Worker·Git 통합·PC 종료 승인이 아닙니다. 무인 사용은 별도 작업 승인과 모드/큐 상태를 확인해야 합니다.

현재 구성은 **로그인 세션 기반**입니다. 로그아웃 상태 지속 실행·PC가 꺼진 동안 실행·자동 전원 켜기를 보장하지 않습니다. 다음 로그인 후 서버가 시작되면 SQLite를 복원하고, 중단된 RUNNING AI는 NOT_VERIFIED로 보존하며 자동 재시도하지 않습니다. 실제 로그아웃·재부팅 복구는 NOT_VERIFIED입니다. [Windows 작업 보안 컨텍스트](https://learn.microsoft.com/en-us/windows/win32/taskschd/security-contexts-for-running-tasks).

### 승인 후 사용할 해제 절차 — 이번에는 실행하지 않음

작업 스케줄러에서 두 LIMITLESS 작업을 각각 ‘사용 안 함’으로 바꾸거나, 등록 후 아래 명령으로 비활성화할 수 있습니다. 이는 현재 실행 중 프로세스를 강제 종료하지 않습니다. [공식 Disable-ScheduledTask](https://learn.microsoft.com/en-us/powershell/module/scheduledtasks/disable-scheduledtask?view=windowsserver2025-ps).

```powershell
Disable-ScheduledTask -TaskName 'LIMITLESS-Art-OnLogon'
Disable-ScheduledTask -TaskName 'LIMITLESS-Bot-OnLogon'
```

진행 중 서비스도 안전하게 멈추려면 `operations-service.ps1 -Action Stop -Service Art`와 `-Service Bot`을 각각 실행합니다. 이 경로는 지속 비활성 표식과 안전 중지 요청을 남기며 업무 DB/납품 파일을 지우지 않습니다. 실제 등록·활성화는 바울 대표의 별도 승인 후 진행합니다.

## 검증 및 보호

| 항목 | 결과 |
|---|---|
| 이번 선별 검사 | 8 PASS / 0 FAIL |
| 기존 36 및 27 PASS | 보존, 전체 재실행0 |
| 실제 조회 3종 | 대표 화면·Gateway 비공개 영수증 PASS |
| 자동 시작 XML 2종 | 설정 검토 PASS, 미등록 |
| A 이전 화면 오류 해결 | NOT_VERIFIED |
| 실제 quota 회복 AI 재개·재부팅/로그아웃·통합/종료 | NOT_VERIFIED / 실행0 |
| 원본 파일 | 7356 SHA변경0·누락0·git상태 동일 |
| 기존 업무/기록 | 21 tasks·13 usage·0 approvals·28 settings·5 art 수령 동일 |
| 현재 모드·재개 큐 | MANUAL·0건 |
| 기존 감시 프로세스 | Bot/Art RUNNING·각각 lock HELD |

실제 Git 통합·Push·main 병합·PC 종료·OS 자동 시작 등록 모두0입니다. main 직접 커밋0이며 격리 codex 브랜치에만 검사/문서를 기록합니다. 추가 승인 대상은 두 OS 작업 등록·활성화, 실제 재부팅 검증, 별도 읽기 전용 AI 재개 실증, 작업별 통합/Push/병합, 예약별 실제 종료입니다. 이번 점검이 그 승인을 대신하지 않습니다.

근거: [보호](../Evidence/OperationsReadiness20261024/protection.json), [실제 조회](../Evidence/OperationsReadiness20261024/new-command-receipts.json), [XML 검토](../Evidence/OperationsReadiness20261024/windows-plan-review.json), [선별 검사](../Evidence/OperationsReadiness20261024/selected-tests.txt).
