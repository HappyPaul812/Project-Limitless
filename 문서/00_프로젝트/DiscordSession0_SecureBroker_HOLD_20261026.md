# Issue #66 Session0 보안 연결 후보 — 2026-10-26

**판정: PARTIAL / SECURITY_DEPLOYMENT_HOLD.** 새 프로토콜·인증 경계·단발 정책·취소 Job 후보를 구현하고 신규42 PASS를 확인했습니다. 실제 Worker 전달 경로 전체가 완성되거나 배포 가능한 상태는 아닙니다. 서비스 Host와 고정 Worker backend의 최종 접합·실제 계정 간 IPC 검증이 남습니다. 실제 AI/usage/Discord/서비스 등록은 수행하지 않았습니다.

기준: `codex/discord-usage-diagnostics-20261026`, `f93d13e79b336453711a69ea5de34be2035b1d99`. 기존 Phase1 VERIFIED·웹 Fake4역할·버튼35 PASS는 재사용했으며 전체 재실행하지 않았습니다. 이번42는 기존35와 합쳐 새로 검증했다고 표시하지 않습니다.

## 동일 SID 문제의 결정

같은 Windows 사용자의 일반/관리자 토큰을 `IsInRole`만으로 나누는 방식은 최종 보안 경계로 채택하지 않습니다. Microsoft는 UAC를 별도의 강한 사용자 보안 경계와 구분합니다. 따라서 후보는 **봇 SID / 사전 승인 관리자 SID / Worker 서비스 SID가 모두 달라야** 합니다. 같은 SID는 관리자 토큰이더라도 서비스 arm/실행을 HOLD합니다. 실제 TokenElevation·관리자 그룹·integrity 검사는 추가 검사이며 SID 분리의 대체재가 아닙니다.

현재 시험 봇은 기존 일반 사용자 Windows 계정으로 실행했고 관리자 PowerShell도 같은 계정이었습니다. 별도 비관리자 시험 봇 실행 계정을 구성하지 않았습니다. 보호된 신규 Program Files 설치도 없습니다. 현재 구성을 그대로 활성화하지 않습니다. 이 제한은 Windows 계정/ACL/부팅/운영 설정을 이번에 바꾸지 말라는 범위 안에서 안전하게 중지하기 위한 결정입니다.

## 구현한 구성요소

| 구성요소 | 구현 및 검증 범위 |
|---|---|
| `secure_session0_broker.py` | 서비스 내부 순수 정책. 관리자 arm→봇 submit/cancel/result의 고정 스키마, 요청ID/대표/guild/channel/전체taskSHA·ticketSHA/범위/만료, 별도 SQLite durable consume, 재시작 HOLD, 중복0, 안전 투영 |
| `Session0PipeBoundary.cs` | 실제 Windows token 기반 Peer(외부 생성자 없음), token elevation/admin/integrity, LSA Session0/logon4, 서비스 SID·비관리자 확인, pipe first-instance/원격 거부/제한 DACL, 개별 arm/work/control 채널, bounded frame, 보호 경로 owner/reparse/쓰기권한 검사 |
| `session0-pipe-client.ps1`, `session0_pipe_transport.py` | 봇은 IPC만 수행. 기본 서비스 SID는 기존 boot approval SHA 확인 후 읽고, OS pipe 서버 PID/token SID/Session0를 확인한 다음 요청 전송. busy·실패·timeout 후 재접속0. helper SHA 고정, 입력/응답16KiB 제한 |
| `session0_planning_bridge.py` | 무조건 거부 stub 대신 인증된 pipe transport 경로, 요청·결과 hash 바인딩, 고정 응답/숫자 usage만 수용. 직접 dict/fixture의 live 주입 거부. endpoint가 없거나 구성되지 않으면 HOLD |
| `mobile_planning_buttons.py` | 5버튼 유지. 관리자 사전 arm과 같은 단발 request ID를 쓰는 선택적 prepared_request_id 지원. 전체 SHA·message/대표/채널·절대 만료·승인 소비 검증 유지. 이전 소비/기존 미술 승인 변경0 |
| `BrokerJob90.cs` | 원본 Job을 보존한 격리 파생본. suspended 생성→Job 할당→시작, kill-on-close, 취소 토큰·예산 종료, bounded stdout/stderr drain·정리 active0 확인 |

원격 사용자가 pipe를 만들거나 프로그램 경로·명령행·prompt·SID/admin 값을 JSON으로 지정해 권한을 얻는 구조는 없습니다. pipe의 write 권한에는 새 서버 instance 생성 권한을 섞지 않았습니다. arm/work/control을 분리하므로 업무 응답을 기다리는 동안 별도 취소 채널이 가능합니다.

서비스는 제한 계정으로 실행될 계획이며 관리자 권한으로 모델을 실행하지 않습니다. 관리자 승인 자료를 사용자-writable 폴더에서 읽어 실행하지 않습니다. 인증된 관리자 pipe에서 받은 사전 arm을 별도의 서비스 소비 장부에 저장하는 구조입니다. 장부도 실제 배포에서 service/SYSTEM/관리자만 변경할 수 있어야 합니다. 새 봇 계정은 이 장부나 실행 코드를 쓰거나 교체할 수 없어야 합니다.

`Broker.context` 및 Python `Peer` dataclass는 순수 정책 계약입니다. 이 값을 외부 JSON에서 만들어서는 안 됩니다. 실제 Host는 네이티브 검증 결과만 내부 callback에 연결해야 하며 **그 최종 Host/backend 조립은 이번에 완성하지 못했습니다**. C# pipe helper가 컴파일됐다고 두 언어 경계와 전체 서비스 실행이 검증된 것은 아닙니다. 허위 VERIFIED로 활성화하지 않습니다.

## 단발·시간·결과 정책

- 관리자 arm은 정확한 request ID·task SHA·기존 정책·최대600초 만료에 묶고 서비스 trial 장부당1개만 허용합니다. 모바일 task 생성 후 hash/예정 request ID로 사전 arm하고, 모바일 실행 버튼 시 ticket을 확정하는 순서입니다. 후속 역할은 승인하지 않습니다.
- 승인 소비는 Worker 준비·사용량·모델 실행 이전에 SQLite FULL synchronous transaction으로 기록합니다. 시작 실패·결과 불명·서비스 재시작 후에도 소비를 복구하거나 재호출하지 않습니다. 재시작은 미사용 arm도 폐기합니다.
- ticket 생성의 UTC deadline과 서비스 단조 시계 잔여 예산을 함께 사용합니다. 전달·준비 시간을 차감하고 마지막2초는 종료 확인에 남깁니다. 기본90초를 연장하지 않습니다. Worker handle은 nonblocking start/poll/bounded close 계약입니다.
- 새 Job은 cancel/timeout 시 트리 종료, 성공 시에도 잔여 프로세스 정리 후 active0를 요구합니다. **실제 서비스 Host가 이 Job을 고정 Worker handle에 연결하는 검증은 남았습니다.** 합성 clock이 90초에 HOLD한 것과 실제 서비스 전체90초 보장은 구분합니다.
- 공식 usage 사전 조회·keyring·builtin OpenAI·읽기 전용 Sandbox·strict JSONL은 기존 검증된 실행기를 재사용해야 합니다. 기존 exec1/App Server1·응용 재시도0·내부 기본 HTTP/stream 재시도 허용(서버 정확1회 보장 불가) 정책 유지. 이번에는 고정 Worker backend를 실제 실행하지 않았습니다.
- 서비스 내부 결과는 기존 project_child로 검증한 고정 문장·숫자 사용량·고정 상태만 pipe에 전달합니다. stderr·private 경로·토큰은 공개하지 않습니다. 클라이언트도 schema/요청 hash/최종 문장/usage 타입을 재검증합니다. Discord 보고 실패 시 기존 UNKNOWN_NO_RETRY·체크포인트 정책을 재사용하며 재추론하지 않습니다.

## 새 검증 결과

**신규 정책·전달34 PASS / Windows 네이티브8 PASS =42 PASS / 0 FAIL.**

34개: 관리자 arm/단발, 동일SID 일반·관리자 토큰 모두 거부, 외부 peer·JSON 관리자 주장·요청/대표/guild/channel/SHA 변조, 만료·취소·중복 소비, 시작 실패·재시작·결과 누락·정리 불명, quota 소진 결과, 보호 결과의 정제, 임의명령/중복JSON/크기 차단, 위조 투영, mocked IPC helper의 정상/변조 응답, 결과 재조회 시 실행 추가0. 이 중 정상 IPC helper 경로는 합성이며 실제 서비스 토큰 검증 성공으로 확대하지 않습니다.

네이티브8개: 새 C# 컴파일, 정상 fake 프로세스 exit0/active0, timeout active0, 취소된 가짜 트리 total4/active0, 현재 제한 Windows 토큰의 관리자 승인 거부, workspace 경로 신뢰 거부, 비서비스 계정의 서버 listen 거부, 초과 frame 거부. 별도 PowerShell fake만 사용했고 foreground/포커스 변경0입니다. 실제 서비스/관리자 토큰 간 연결과 긍정 DACL 검증은 하지 않았습니다.

시험 원문과 전체 SHA는 `팀운영/Evidence/SecureSession0Broker20261026/`에 기록합니다. 기존 원본 실행기4개와 boot approval SHA는 일치합니다. 이전 PreparedPlan/소비/진단/완료 Queue를 덮어쓰거나 초기화하지 않았습니다. 새 활성 permit/서비스 arm0, synthetic arm/소비는 테스트 scratch에만 생성 후 정리했습니다.

## 실전 시험 전 필요한 조건 — 이번에는 실행하지 않음

1. 별도 비관리자 **시험 봇** Windows 계정 또는 동등하게 검증된 별도 사용자 보안 주체를 확정해야 합니다. 운영 봇 계정/기존 service 계정 권한을 바꾸거나 토큰을 복사하지 않습니다. 필요 시 대표가 승인한 별도 로컬 보안 입력만 사용합니다.
2. 관리자 소유 고정 코드/런타임 전체 의존성을 보호된 새 설치에 배치하고 owner/DACL/reparse/실제 SHA·고정 task action/인스턴스 제한을 검증해야 합니다. 현재 workspace에서 관리자 Host를 실행하지 않습니다. 원본 runtime44 승인 목록은 확대하지 않고 legacy Worker package와 새 broker package를 분리합니다.
3. 네이티브 Peer→Python broker→고정된 기존 서비스 Worker backend→새 취소 Job→보호 결과 reader의 최종 Host 조립 및 중단·재시작 시험이 필요합니다. 프로세스가 임의 요청 경로를 실행하지 않는지, Host 강제 종료에도 Job이 닫히는지 실제 계정으로 fake-only 검증해야 합니다. Windows token query/pipe 권한이 실패하면 권한을 완화하지 않고 HOLD합니다.
4. 위 조건 PASS 후 대표가 시험 앱 명령1개·Planning 읽기 전용1건·usage1/exec1·90초·결과 게시·정리를 별도로 승인해야 합니다. 지금 명령 등록·모델 호출을 하지 않습니다. 휴대전화에서는 기존 5버튼을 사용하고 긴 SHA를 입력하지 않습니다.

PIN 없는 실행은 기존 Session0 모델 VERIFIED 근거가 있지만, 이번 신규 IPC/Host 전체 흐름은 아직 미검증입니다. 계정 분리와 사전 고정 task/최소권한 서비스 경계를 구성하면 PIN 입력 없이 동작하도록 설계할 수 있습니다. 현재 성공한 것으로 단정하거나 PIN/자동 로그인을 변경하지 않습니다.

## 배포·복구 계획과 현재 보호

현재 설치·계정·서비스/task·bot·Discord 명령을 전혀 변경하지 않아 운영 rollback은 없습니다. 새 후보/로컬 commit/증거만 보존합니다. 운영 SQLite 업무0·MANUAL/A 미활성·인증/토큰 복사0·Unity/게임/Save/Push/main/PC 변경0입니다. 운영 봇 건강·동적 SQLite 상태를 이번에 새로 점검한 것으로 주장하지 않습니다.

향후 별도 승인 시 새 시험 계정/새 고정 task/새 설치만 사용합니다. 시작 전 운영 파일·명령 inventory·승인 SHA snapshot을 보존하고, 시험 후 신규 접수 차단→서비스 arm 비활성→취소 Job 종료/active0→시험 Host 종료→반환 command ID 하나만 삭제→전후 대조 순서입니다. 새 시험 설치/task만 철수하고 소비·체크포인트·정제 증거는 보존하며 기존 OnBoot2개나 기존 운영 승인을 복구 명목으로 덮어쓰지 않습니다.

공식 근거: [MSRC 보안 경계·UAC 분류](https://www.microsoft.com/en-us/msrc/windows-security-servicing-criteria), [Windows pipe 보안 권한](https://learn.microsoft.com/en-us/windows/win32/ipc/named-pipe-security-and-access-rights), [RunAsClient](https://learn.microsoft.com/en-us/dotnet/api/system.io.pipes.namedpipeserverstream.runasclient), [WindowsPrincipal.IsInRole](https://learn.microsoft.com/en-us/dotnet/api/system.security.principal.windowsprincipal.isinrole).

구현 로컬 commit: `82af13e15469ae23d8ca13e96ac6b651f3160a9f`. 문서 마감 실제 commit은 Issue #66 완료 댓글을 따릅니다. Push 없음.
