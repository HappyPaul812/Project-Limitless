# 운영 자동화 실연동 안내

완료일 2026-10-24 KST. 실제 PC 종료·자동 시작 등록·GitHub Push/main 병합은 비활성입니다.

## Discord 현재 동작

기존9개+새6개 Slash를 실제 서버에 등록했습니다. 허용 대표 숫자ID1개·서버·채널·사람 계정 여부를 검증합니다. 새 관리 응답은 한국어·본인 전용이며 실제 interaction/response ID와 ephemeral 여부를 DB에 기록합니다.

| 명령 | 사용 |
|---|---|
| `/limitless usage` | 공식 사용량을 새로 읽고 5시간/주간 사용률·회복 시각 표시 |
| `/limitless schedule until:2026-10-24 23:00` | 서울 시각 마감 등록, 정리 기본10분 |
| `/limitless schedule-status` | 현재 예약·운영 상태 조회 |
| `/limitless schedule until:... schedule_id:SCH-...` | 기존 예약 수정 |
| `/limitless shutdown schedule_id:SCH-... confirm:SCH-...` | 대상 PC 지문·실행중 업무 확인, 실제 종료는 HOLD |
| `/limitless finish schedule_id:SCH-...` | 신규 접수/시작 차단, 장부 요약·동기화 HOLD 보고 |
| `/limitless cancel schedule_id:SCH-...` | 예약 취소, 이미 정리 중이면 보류 유지 |
| `/limitless cancel schedule_id:SCH-... release_intake:true` | 대표가 명시적으로 접수 보류도 해제. RUNNING 업무 있으면 거부 |

취소는 수동 pause·역할 정지·승인 보류를 해제하지 않습니다. 이번 시험 예약 `SCH-79e9740a0440`은 취소됐고 접수 보류도 해제됐습니다. 원래 업무의 상태·승인·결과는 그대로입니다. TEST_ONLY는 조회에서 시험용으로 구분하며 실제 미술 승인을 거부합니다.

## 공식 사용량과 단발 재개

공식 [Codex App Server 문서](https://learn.chatgpt.com/docs/app-server)의 stdio JSONL을 사용합니다. 설치 CLI `0.163.0-alpha.11.2`에서 실제 성공했습니다. 보낸 메서드는 `initialize`, `initialized`, `account/read(refreshToken:false)`, `account/rateLimits/read`뿐입니다. 계정 종류가 chatgpt여야 합니다. thread/turn 생성·로그인 변경·크레딧/재설정권 소비·추가 구매 API를 호출하지 않습니다. 서버 추가 인증 요청이나 불명확한 응답에는 응답하지 않고 보류합니다.

봇은 정상 상태에서120초마다 조회하고 `/usage`는 대표 요청으로 즉시 새로 확인합니다. 300분·10080분 창을 구분하며 출처/관측시각/허용여부/사용률/회복시각만 저장합니다. 계정ID·이메일·토큰·크레딧·인증정보·stdout/stderr 원문은 저장하지 않습니다. 모델 자식 환경에 Discord/API 키를 전달하지 않습니다.

한도가 소진되면 해당 창들의 가장 늦은 회복 시각까지 대기하고 그 이후 새 공식 조회로 재검증합니다. 시각만 지났다고 가용량을 추측하지 않습니다. 조회 실패 즉시 기존 AVAILABLE 관측도 무효화해 UNKNOWN/HOLD입니다. 자동 조회 실패3회면 회로를 열고 중단하며 대표 `/usage`로 다시 확인할 수 있습니다. 관측 유효기간5분은 서비스의 보수적 정책입니다.

기존 `run_once`가 예약/사용량으로 실행 전에 보류되면, **이미 승인된 정확한 plan SHA·role·slot**만 별도 큐에 저장합니다. 미소비·미만료 승인, 빈 격리 폴더, 실행 파일 SHA, 주일에도 유효한 기존 승인 범위, 예약 여유, 수동/역할 정지 없음, 동시2 이내를 확인한 뒤 기존 읽기전용90초 실행기를 최대1회 재사용합니다. 모델 결과는 기존 출력 검증기를 사용합니다. 실패·결과불명·소비된 슬롯은 재시도하지 않습니다. 프로세스 재시작 시 RUNNING 재개 건은 NOT_VERIFIED로 보존합니다.

새 승인이나 승인기한 연장을 만들지 않습니다. 기존 승인 유효기간10분을 유지하므로 회복 시 만료됐다면 재승인이 필요합니다. 이번 실제 재개 큐는0건이며 AI를 추가로 실행하지 않았습니다. 회복/재개 조건은 모의 runner로만 시험했으며 실제 한도 소진을 유도하지 않았습니다. 기존 MOCK·아트 수령 대기·TEST_ONLY는 재개 대상이 아닙니다.

## 안전 중지·오류 기록

봇 수동 실행: `register-discord.ps1 -Action Run`. 봇 중지 요청: `-Action OperationsBotStop`. 수령 서비스는 `-Action Phase3Run` / `-Action Phase3Stop`입니다. 둘 다 서로의 프로세스나 Unity를 종료하지 않습니다. PID 전체 kill을 사용하지 않습니다.

봇/수령 상태는 `data/phase2/bot-service-status.json`, `data/phase3/service-status.json`, 업무·사용량·오류 이벤트는 `data/phase2/relay.sqlite`에 남습니다. 공식 사용량 실패는 고정 사유만 저장합니다. 향후 자동 시작 wrapper의 이벤트는 `data/operations/service-events.jsonl`, 표준 출력/오류는 시각별 로컬 로그로 남겨 덮어쓰지 않습니다. 로그 원문은 검토·마스킹 없이 외부에 게시하지 않습니다.

이번에 정상 종료 때 timer 취소가 SDK close 작업까지 취소하던 충돌을 수정했습니다. 신규 모의 시험과 실제 정상 중지 exit0·STOPPED·재접속을 확인했습니다. 최초 구형 봇 교체는 RUNNING0·DB백업 후 정확한 봇 PID만 종료했습니다. 이후 수정본은 정상 중지 표식을 사용했습니다. 별도 아트 수령 서비스도 커서/지문을 보존해 정상 재접속했으며 재처리/새 업무 등록이 없었습니다.

## Windows 자동 시작 설정안 — 미등록·비활성

| 설정 | 준비 내용 |
|---|---|
| 작업 이름 | LIMITLESS-Art-OnLogon / LIMITLESS-Bot-OnLogon |
| 계정 | 현재 DPAPI 토큰을 등록한 동일 Windows 사용자 |
| 트리거 | 해당 사용자 로그인 후30초 |
| 권한 | InteractiveToken·LeastPrivilege |
| 중복 | IgnoreNew + 각 Python 서비스 file lock |
| 오류 재시작 | 최대3회·간격1분, wrapper가 자식 종료까지 대기 |
| 강제 종료 | 비허용, 실행시간 한도0(서비스용) |
| 현재 상태 | XML Disabled, 작업 등록0, 승인파일 없음, 시작 게이트 비활성 |

`operations-service.ps1 -Action Preview -Service Art` 또는 `Bot`으로 비활성 XML/설정안을 생성합니다. 실제 XML은 `data/operations/LIMITLESS-Art-OnLogon.xml`, `LIMITLESS-Bot-OnLogon.xml`입니다. 이 스크립트에 Register/Enable 기능은 없습니다. 별도 승인 이후 등록과 활성화 절차를 진행해야 합니다.

향후 시작은 고정 스크립트·Service enum·동일 사용자 SID·wrapper/starter SHA·명시적 승인 파일을 검증해야 합니다. 토큰/비밀번호는 작업 인수에 넣지 않습니다. CurrentUser DPAPI/전용 ACL을 유지하고 SYSTEM이나 다른 사용자로 우회하지 않습니다. `StartIfEnabled`는 미승인 상태에서 HOLD를 확인했으며 서비스를 새로 시작하지 않았습니다.

영속 중지(향후 자동 시작까지 비활성화): `operations-service.ps1 -Action Stop -Service Art` 또는 `Bot`. 대표의 중지 표식이 있으면 재시작하지 않습니다. 이번에는 자동 시작 등록·활성화·재부팅·로그아웃을 하지 않았습니다. PC가 완전히 꺼져 있으면 재개할 수 없으며 로그아웃 중 지속 운영은 이 로그인 방식으로 보장하지 않습니다.

## PC 종료·Git·아트 승인

현재 PC 지문과 SQLite RUNNING 수는 확인합니다. 미저장 사용자 작업·파일복사·Unity의 안전 종료와 명시적 통합 승인·원격/로컬 SHA 동기화는 미확인이므로 **종료 HOLD**입니다. 실제 종료 API/시스템 명령을 호출하는 경로가 없습니다. `/shutdown`은 요청 검증과 조건 보고만 수행합니다. 요청은 `/cancel`로 취소할 수 있으며 향후 실제 기능에는120초 이상 취소 유예와 모든 조건 재검증이 필요합니다.

마감/미술 승인/운영 등록이 Push나 main 병합 허가가 되지 않습니다. 아트 제작 → Codex 검수 → 바울 대표 미술 승인 → 승인 범위 내 적용을 유지합니다. Unity Worker와 임의 AI 생산 작업은 활성화하지 않습니다.
