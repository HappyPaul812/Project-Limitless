# 서비스 Session0 실제 추론 단발 승인 — 로컬 보안 입력 대기

대표가 새 공식 CLI의 실제 읽기 전용 추론1회를 승인했다. A 활성화 승인이 아니다. 현재 상태는 **AUTHORIZED_NOT_STARTED / 실제 모델0회**다. 현재 Codex 세션은 비관리자이며 서비스 계정 Windows 자격 증명을 보유하지 않아 대신 실행하지 않았다. 이번 승인으로 실행할 준비는 끝났으며 대표의 관리자 PowerShell 로컬 보안 입력이 필요하다.

## 이번 단발 실행물

- `Tools/DiscordAIServiceDiagnostics/run-service-final-once.ps1`: 관리자 보안 입력·한정 임시 작업·실제 SID/Session0/logon4·운영 참조/45패키지/기존42코드 SHA 검증·전후 부팅 XML/승인파일 불변·임시 작업 제거.
- `service-final-once.py`: 기존 보호된 인증/공식usage/명령 구성/출력 검증/진단 보존/Discord 게시 함수를 그대로 재사용한다. collab 기존42개는 수정하지 않았다. 새 진입점과 runner의 SHA를 task child 인수로 고정한다. 기존 승인 목록을 확장하거나 부팅 실행 경로에 새 프로그램을 연결하지 않았다.
- 새 고정 슬롯 `SERVICE_FINAL_PLANNING_20261024_01`, 새 폴더 `Tools/DiscordAICollab/data/service-final-once-20261024`. 기존 CONSUMED12·이전 실패 시험 슬롯/마커·사용자Worker 슬롯은 재사용/수정하지 않는다.

## 실제 실행 순서

1. 운영 승인 JSON SHA `3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f`, service_codex.path와 SHA `4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73`, 전체45파일·본체/보조3개 서명 및 기존 runtime SHA를 검증한다.
2. 서비스 계정 자기 프로필의 Discord DPAPI 저장소만 메모리로 연다. 토큰은 CLI 하위 환경에 전달하지 않는다. 새 Bot/Gateway/감시 프로세스를 시작하지 않고 성공 결과1건만 기존 REST 게시 함수로 보낸다.
3. 서비스 자기 ChatGPT 구독 인증을 읽기 확인하고 자기 프로필의 CurrentUser DPAPI 로그 보존 사전검사를 한다. 사용자 원본과 서비스 worktree의 Assets/Packages/ProjectSettings/UserData 파일을 해시한다. 파일 접근·보존 사전검사가 실패하면 추론 전 HOLD한다. 인증/키 파일은 게임 해시 범위 밖이다.
4. 최신 docs/planning commit과 PLANNING.md 단일 문서만 가져와 SHA를 기록한다. 문서 내 추가 실행 지시는 reference data로만 취급하고 도구 호출을 금지한다. 5문장/600자·문장별 줄바꿈·고정 role/next_review의 기존 JSON 계약을 사용한다.
5. 실제 실행 직전에 공식 account/rateLimits/read의5시간·주간 사용량을 읽는다. MANUAL/usage/예약/paused/다른 AI RUNNING을 확인하고 새 슬롯을 CONSUMED로 예약한다. 정책을 완화하지 않는다.
6. 기존 read-only/approval never/도구·웹·MCP·플러그인·후속agent 차단 명령으로 **subprocess exec1회, timeout90초, 응용 재시도0**. 기본 모델 해석은 기존 CLI와 같으며 임의 모델/유료 API로 전환하지 않는다. CLI 내부 transport 동작은 기존 검증 명령의 정책 그대로이고, 응용에서 새 모델 프로세스를 반복 실행하지 않는다.
7. 파서 전에 원본 JSONL 바이트를 서비스 자신의 DPAPI 암호화 파일로 보존한다. 같은 보호 프로필에 정제stderr/JSONL·최초error/turn.failed·실행 계정/세션/CLI/Sandbox/argv/환경변수 이름·종료코드를 남긴다. 환경값·원문 오류·원본 추론을 공용 폴더/외부로 게시하지 않는다. 실제 프로세스 생성 전에 preflight.json도 남긴다.
8. 기존 JSONL/최종 파일 대조·역할/5문장·필수 토큰 수를 모두 검증하고 게임 해시가 같은지 확인한다. 실패 시 NOT_VERIFIED, Discord VERIFIED 게시0. 통과 시에만 `[REAL AI / VERIFIED / SERVICE BATCH]` 1건 게시한다. **게시가 실패해도 NOT_VERIFIED**이며 자동 재시도하지 않는다. 완료된 게시까지 확인한 뒤 최종 VERIFIED를 기록한다.

전체 작업 제한은 사전 해시/인증/전송 시간을 포함해6분이며 실제 CLI 추론 subprocess는90초다. launch-consumed/attempt-consumed가 생성되면 재실행을 차단한다. 잠금화면 부팅 직후부터의 전체 무인 체인을 시험한 것과, 이번 Password/Batch의 Session0 추론 성공은 구분한다. 이번에는 재부팅/로그아웃/PIN 설정 변경을 하지 않는다.

## 오프라인 검증

모의 subprocess·메모리SQLite·모의 인증/usage/Discord로 **8 PASS**: 정상 단발, CLI 오류 게시 차단, FORBIDDEN_ITEM 차단, timeout 부분 로그, 게임 변경 차단, Discord 실패 NOT_VERIFIED, 5문장 초과, 토큰 누락. 각 전체 경로 시험에서는 같은 마커로 재호출해도 subprocess가 추가되지 않음을 확인했다. 실제 CLI 추론/네트워크/운영 DB는 이 테스트에서 사용하지 않았다. PowerShell 구문0오류·비관리자 실행 가드는 `HOLD_ADMIN_LOCAL_SERVICE_CREDENTIAL_REQUIRED_NO_AI_CALL`로 중단됨을 확인했다.

실제 trial의 소비 마커 두 개는 아직 없다. 지금까지 실제 추론·Discord 게시0이다. Bot/Art RUNNING·AIWorker 비활성을 확인했으며 새 승인 슬롯도 실제 실행 직전까지 운영 DB에 넣지 않는다.

## 대표의 로컬 실행 명령

관리자 PowerShell에서 아래 한 줄을 실행하고 서비스 Windows 암호를 로컬 보안 입력창에만 입력한다. 암호를 채팅·로그·Discord·GitHub에 보내지 않는다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAIServiceDiagnostics\run-service-final-once.ps1"
```

실행 정책이 차단하면 보안 정책을 임의 완화하지 않고 중지한다. 이 명령은 기존 실행 승인을 받아 준비한 시험이며 새 승인을 다시 요청하는 단계가 아니다. 실행 뒤 성공/실패와 무관하게 **다시 실행하지 않는다**.

## 결과와 다음 단계

공용 안전 결과: `data/service-final-once-20261024/batch-summary.json`, `result.json`, `failure-diagnostics.json`, `failure-projection.jsonl`. 계정 내부 원본/정제 로그: `C:\Users\LIMITLESSRelaySvc\.codex\limitless-diagnostics\<request_id>`. 원본 JSONL은 DPAPI 암호화이며 다른 계정으로 복사/복호화하지 않는다. 정제는 best effort이므로 그 원문도 자동 외부 게시하지 않는다.

결과에는 실행 계정·Session0·CLI SHA/종료·완료 턴·검증 거부 코드·입출력 토큰·시간·게임 변경·Discord message ID/URL이 남는다. 부모는 해당 임시 작업만 정리하고 기존2개 작업 및 승인 JSON이 같은지 확인한다. 실사용 후 성공/실패 원인과 실제 호출 수를 보고하고 중지한다. A 활성화는 대표의 **별도 후속 승인** 대상이다. 기존 turn.failed 원인은 이 시험 전에 확정된 것으로 취급하지 않는다.
