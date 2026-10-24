# 서비스 계정 AI 실행 환경 최종 진단 — 원인 미확정 / 실제 추론0

## 결론

첫 실패의 직접 원인은 **NOT_VERIFIED**다. 서비스 계정 비대화형 실행 자체가 금지되거나 지원되지 않는다고 단정할 근거도 없다. 이번 조사에서 추론·로그인·사용량 API·Discord 게시·새 배치 작업 실행0. 기존 OnBoot2·봇·아트·인증·MANUAL을 변경하지 않았다.

## 새로 확인한 근거

| 조사 | 직접 확인 | 한계 |
| --- | --- | --- |
| TaskScheduler Operational | sandbox 밖 읽기에서 IsEnabled=false, RecordCount=null | 과거 해당 작업 이벤트를 찾지 못함. 지금 로그를 활성화해도 과거 복구 불가. 활성화/설정 변경하지 않음 |
| 계정·세션 | 현재 PAUL-PC\\windows/Session8, 과거 실제 서비스 PAUL-PC\\LIMITLESSRelaySvc/Batch4/Session0 | 현재 프로세스는 정상 사용자 데스크톱의 새 CLI 세션이 아니라 Codex가 만든 자식 프로세스이므로 일반 사용자 기준선의 한계 명시 |
| HOME/CODEX_HOME | 현재 사용자 둘 다 unset, USERPROFILE C:\Users\windows. 과거 서비스 배치 CODEX_HOME unset, 실제 실행기에서 HOME/USERPROFILE/ CODEX_HOME을 서비스 자기 프로필로 설정 | 과거 서비스 전체 환경은 저장되지 않았고 현재 서비스 프로세스 환경을 읽지 않음 |
| TEMP/TMP | 현재 Codex sandbox 안에서는 sandbox 패키지 Temp, 밖에서는 C:\Users\windows\AppData\Local\Temp | 서비스 당시 TEMP/TMP 값 미기록 |
| CLI 버전 | 양쪽 0.164.0-alpha.11.1 | 동일 버전 표시가 동일 바이트를 뜻하지 않음 |
| CLI SHA | 현재 사용자1103e9e09c71fa2cd0afd6d8f3b9a2d63174fe8efd7cb5bd09f1cb14ef58a098, 서비스c6144bc19b2705f25a40d8488b3d57d17a223c5df3c5f65e3c58d72601956c1e | 교체·업데이트하지 않음 |
| 동반 파일 | 현재 설치에는 codex.exe 외 codex-code-mode-host.exe, codex-command-runner.exe, codex-windows-sandbox-setup.exe. 서비스 고정 폴더에는 codex.exe만 있음 | 런타임 배치 차이 확인. 이번 실패가 동반 파일을 필요로 했다는 오류 근거는 없으므로 원인 확정 아님 |
| 사용자 설정 | 현재 config model=gpt-6.1-sol, effort=low, sandbox_mode=workspace-write, approval_policy=never, windows sandbox 설정 존재 | 현재 사용자 설정을 서비스로 복사하지 않음 |
| 과거 서비스 설정 | 인증 증거 config SHA818c2e7b…가 생성 코드의 두 줄 템플릿(keyring/forced chatgpt, CRLF) SHA와 일치. 명시적 model 없음 | 현재 서비스 config 읽기는 ACCESS_DENIED. 과거 설정 상태와 현재 상태를 구분 |
| exec와 조회 차이 | login status/App Server는 사용자 설정 사용, exec는 --ignore-user-config, 모델 미지정, read-only/never/금지도구 비활성 | 설정 경로 차이는 확인. keyring 자동 해석/기본 모델/Windows sandbox의 실제 해석 결과는 실패 원문 부재로 미확인 |
| 프록시 | 현재 사용자 HTTP_PROXY/HTTPS_PROXY/ALL_PROXY/NO_PROXY 미설정. WinHTTP 직접 접근, 사용자 WinINET ProxyEnable=false/PAC없음 | 서비스 프록시는 미확인. CLI child_environment는 proxy 변수·API키·Discord키를 허용하지 않음. 실제 서비스가 프록시를 필요로 했는지는 미확인 |
| 네트워크 | 현재 사용자 chatgpt.com/auth.openai.com/api.github.com DNS 및 TCP443 성공. 과거 서비스 GitHub·공식 사용량 조회 성공 | TCP 확인은 TLS/HTTP/모델 스트림 성공을 증명하지 않음. 추론 endpoint 요청하지 않음 |
| 파일 권한 | 과거 서비스는 schema/result/marker 생성 가능. schema 소유자 서비스, 서비스 ReadAndExecute ACL. 현재 서비스 config/log는 접근 거부 | parent 쓰기와 CLI/Sandbox 내부 접근은 다른 검증. ACL/소유자/권한 변경하지 않음 |
| 서비스 사용자 hive | HKU 서비스 SID Environment 경로 ItemNotFound | hive 비탑재/경로 부재 가능, 환경변수 없음으로 단정하지 않음. hive 로드하지 않음 |
| 기존 Codex 로그 | `.codex/log/*.log` 및 `.codex/.sandbox/sandbox.log` 오프라인 검사 경로 준비 | 현재 계정은 서비스 .codex 접근 거부. 로그 존재 여부·내용 미확정. 우회/복사 안 함 |

## Session 0 및 공식 지원 범위

[비대화형 실행](https://learn.chatgpt.com/docs/non-interactive-mode)은 scripts/CI의 `codex exec`와 저장 인증 재사용을 안내한다. [인증](https://learn.chatgpt.com/docs/auth)은 OS keyring을 공식 저장 옵션으로 설명하며 keyring이 없으면 실패하는 동작을 명시한다. 따라서 비대화형이라는 이유만으로 ChatGPT 구독 추론이 불가능하다고 할 수 없다.

반면 [ChatGPT 인증 CI/CD 고급 안내](https://learn.chatgpt.com/docs/auth/ci-cd-auth)는 신뢰된 비공개 runner의 직렬 실행과 갱신된 auth 보존을 다루며 해당 안내의 keyring 환경은 file 전환을 요구한다. 이는 **현재 Windows Session0 + keyring 구조의 검증 근거가 아니다**. 이번 지시는 인증정보·저장방식을 유지하는 것이므로 파일 전환·토큰 복사·API 전환은 하지 않는다.

[Windows Sandbox 문서](https://learn.chatgpt.com/docs/windows/windows-sandbox)는 native Windows CLI와 sandbox 설정/권한/로그 조사를 안내한다. 이 서비스 계정·고정 단독exe·keyring·Session0 조합을 안정적 운영 구성으로 보증하는 문구는 확인하지 못했다. Batch Session0에는 사용자 대화형 콘솔이 없고 현재 정책은 UI/도구 요청을 금지하므로 사용자 입력이나 setup 승인을 요구하는 경로는 처리할 수 없다. 그러나 그런 요청이 실제 발생했다는 로그가 없으므로 이것을 turn.failed 원인으로 확정하지 않는다. sandbox setup 실행·UAC/foreground 조작·약한 sandbox로 변경0.

## 향후 진단 보존 구현

`collab/private_execution_log.py`와 단발 실행기만 보완했다. 새 실제 호출을 실행하거나 기존 소비 슬롯을 재발행하지 않았다.

- 추론 전 서비스 자신의 보호된 `.codex/limitless-diagnostics/<request_id>/`에 기록 위치를 준비하고 CurrentUser DPAPI 암호화 사전검사를 수행한다. 실패하면 추론 전 HOLD한다. 기존 .codex ACL/소유자는 바꾸지 않는다.
- `stdout.original.jsonl.dpapi`: subprocess가 반환한 **원본 stdout 바이트 전체**를 CurrentUser DPAPI/UI_FORBIDDEN으로 암호화. 다른 계정으로 복사·평문 원본 생성·자동 복호화/게시0. JSONL 원본에 잠재 비밀/추론이 있어도 공용 workspace에는 노출하지 않는다.
- `stdout.redacted.jsonl`: 이벤트 구조·유형을 유지하는 비밀 제거 사본. `stderr.redacted.txt`: known 환경 비밀, Bearer/JWT/API키/민감 헤더/URL 자격 증명 등을 제거한 stderr. 자동 정제는 완전한 비밀 탐지를 보장하지 않으므로 **같은 보호 프로필에만 보존하고 자동 외부 전송하지 않는다**.
- `execution.json`: 종료코드, 최초 error 전체 이벤트(비밀 제거), turn.failed 이벤트(비밀 제거), request_id, 실제 계정·세션·로그온, argv/CLI 버전/CODEX_HOME/launcher cwd/timeout. 원본 stdout SHA/바이트 수 포함. 공용 result에는 private 경로·크기만 반환한다.
- bytes capture로 변경하여 UTF-8 디코딩 손상 전 원본 보존. 파서 전에 archive를 기록하며 90초 timeout의 partial stdout/stderr도 보존한다. 모델·Sandbox·인증/금지 도구/검증 설정 변경0.
- FORBIDDEN_ITEM 진단에는 `TOOL_OR_SIDE_EFFECT_REPORTED`와 `UNSUPPORTED_ITEM_TYPE`을 별도로 기록한다. 전자는 금지 도구/변경 항목 보고이지 실제 부작용 완료 확정이 아니다. 양쪽 모두 기존 검증기에서 계속 거부한다. 알 수 없는 유형도 원본 암호화 archive에 남는다.

새 보존 경로를 실제 서비스 계정에서 실행한 검증은 아직 없으며, 이번에는 합성 데이터만 사용했다. 원본 첫 실패의 로그를 새로 복구했다고 주장하지 않는다.

## 검증 및 보호

- 오프라인 **36 PASS**: 기존 parser/진단30 + DPAPI 합성 바이트 실제 roundtrip, 원본 바이트 암호화 전달, 비밀 제거/토큰 수 유지, 최초 error·turn.failed/종료코드 보존, 도구 vs 미지원 유형 분리, 잘못된 UTF-8 원본 보존. 실제 AI·로그인·사용량 호출0.
- 초기 테스트2개가 Codex sandbox Temp 쓰기 권한으로 실패하여 테스트용 임시 폴더를 workspace data로 한정하고 재실행36 PASS. 서비스 장애의 원인으로 간주하지 않음. PowerShell 구문0오류, Python compile 및 git diff --check PASS.
- Bot/Art RUNNING·heartbeat 증가, Bot ai_worker_enabled=false/Art ai_calls0. DB MANUAL·기존 CONSUMED 마커 유지. 부팅 작업·운영 서비스 코드 수정0. Operational 채널 활성화·프록시·방화벽·ACL·인증/런타임 변경0. Unity/아트/Push/merge/종료/재부팅0.

## 구조적 대안 — 제안, 구현·활성화하지 않음

PIN 없는 기존 서비스 봇·아트 감시는 유지한다. AI만 바울 대표가 Windows에 로그인한 뒤 별도 사용자 Worker에서 **완전한 공식 CLI 설치와 그 사용자 자신의 ChatGPT 인증**을 사용하도록 분리하는 안을 권장한다. 로그아웃·미로그인 시 AI HOLD, 중계/아트는 계속 실행한다. 이것은 공식 CLI 기능에 기반한 프로젝트용 설계 제안이지 이 Worker 전체에 대한 OpenAI 보증은 아니다.

Worker는 기존 단일 SQLite 승인/usage/lease/중복 방지와 연결하되 서비스에는 인증정보를 전달하지 않고 비밀 없는 승인 작업·검증 결과만 교환한다. 새 Bot/Art를 중복 실행하거나 MANUAL을 자동 변경하지 않는다. 서비스/사용자 간 raw 진단로그도 공유하지 않는다. 이 분리는 keyring·대화형 세션 차이를 줄이지만 실제 무호출 설정 검사와 별도 승인된 성공 시험이 필요하다.

## 별도 승인 전 실행하지 않는 단발 진단 재시험 계획

1. 먼저 관리자/기존 서비스 계정에서 **모델 호출 없이** 현재 환경·허용 설정·완전한 공식 런타임/동반 파일·기존 sandbox 로그를 읽는다. 필요 시 고정 단독exe 대신 공식 설치 단위를 준비하는 변경은 별도 검토한다. 기존 부팅 작업과 인증은 변경하지 않는다.
2. 최초 원인 로그가 복구되면 그 부분만 수정한다. 없으면 UNKNOWN을 유지하고 새 request/별도 폴더·새 1회 승인 슬롯을 준비한다. 기존 marker/CONSUMED 슬롯 삭제 금지. 이번 구현에는 재시험 실행 경로/새 승인을 추가하지 않았다.
3. 대표가 계획을 승인한 경우에만 동일 구독/동일 읽기 범위로 최대1회·90초. 실행 직전 공식 두 사용량 창 확인, 기존 gate/정지/중복 제한 유지. 계정·모델 해석·설정·원본 암호화 JSONL/비밀제거 stderr를 위 경로에 보존한다. 단독exe/모델/Sandbox를 한꺼번에 바꾸지 않고 승인한 변수만 변경한다.
4. 실패 자동 재시도0, 원본 로그를 같은 계정 안에서 오프라인 분석. 검증기 우회/금지도구 완화0. 성공/실패 모두 MANUAL 유지하고 중지한다. Discord 게시가 필요하면 별도 승인 범위에 포함하며 진단 원문은 보내지 않는다.

실제 추론 경로의 성공을 증명하려면 결국 별도 승인된 실제 시험이 필요하다. **현재는 추가 시험 승인 대기가 아니라 환경·진단 계획 검토 단계이며 재시험 자체 미실행**이다.

브랜치 `codex/discord-ai-collaboration`, 선행 commit `8dbe4076`. 이번 commit SHA는 최종 보고를 따른다.
