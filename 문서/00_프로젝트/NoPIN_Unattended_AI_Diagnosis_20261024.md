# PIN 없는 무인 AI 운영 진단 및 다음 변경안

2026-10-24. 기준 commit 8a0191c471f9d400abac725c23e354a9f9c8863c. 이번 조사 실제 모델 호출 0회. 실행·인증·승인 목록·예약 작업을 변경하지 않았다.

## 운영 목표와 판정

Windows 잠금화면을 유지한 채 부팅 중계 → 모바일 Discord 승인 → AI → 검수·보고를 목표로 한다. 로그인 후 사용자 Worker는 예비 수단이며 최종 구조로 채택하지 않는다. 이전 환경 보고의 사용자 Worker 권장 부분은 이 우선순위로 대체한다. 기존 신규 슬롯은 승인 대기 상태 그대로이며 서비스 재시험 승인으로 전용하지 않는다.

현재 **PIN 전 실제 추론부터 Discord 보고까지 NOT_VERIFIED**. 서비스 배치·구독 인증·공식 사용량 조회는 과거 PASS이나 추론 성공은 입증되지 않았다. Session 0 자체가 지원 불가라는 결론도 입증되지 않았다.

## 확인된 차이와 원인 미확정 범위

| 항목 | 확인 근거 | 판단 |
|---|---|---|
| 일반 사용자 CLI | C:\Users\windows\AppData\Local\OpenAI\Codex\bin\2993767bd1dfb009\codex.exe, SHA256 1103e9e09c71fa2cd0afd6d8f3b9a2d63174fe8efd7cb5bd09f1cb14ef58a098 | 공식 설치 폴더에 동반 exe 3개 존재 |
| 서비스 CLI | Tools/DiscordAICollab/data/runtime/codex/c6144bc19b2705f2/codex.exe, SHA256 c6144bc19b2705f25a40d8488b3d57d17a223c5df3c5f65e3c58d72601956c1e | 폴더에 codex.exe만 존재. 동일 표시 버전 0.164.0-alpha.11.1도 동일 배포본을 의미하지 않음 |
| 동반 파일 | codex-code-mode-host.exe / codex-command-runner.exe / codex-windows-sandbox-setup.exe | 누락이 장애 원인인지는 불명. 서로 다른 버전 파일을 섞지 말아야 함 |
| 모델·설정 | 정상 사용자 config 모델 gpt-6.1-sol. 과거 서비스 config SHA는 keyring/chatgpt 두 줄 템플릿과 일치 | 현재 서비스 config 직접 열람 불가. 실제 exec는 --ignore-user-config·명시 모델 없음. 인증/usage 성공과 추론 설정 성공은 별개 |
| Sandbox | 실행 명령 read-only, approval never, shell/apps/browser/computer/MCP/plugins/hooks/web 차단 | 유지. setup가 완료됐는지·필요 보조파일 실행 여부는 미확인 |
| 작업 경로·권한 | 서비스 HOME/CODEX_HOME 자기 프로필, 읽기 전용 시험 작업 경로 사용 | 배치 성공이 sandbox 자식 프로세스·TEMP·모든 파일 ACL 성공까지 입증하지 않음 |
| 네트워크 | 서비스 당시 인증/usage/GitHub 읽기 성공. 일반 사용자 DNS/TCP443·직접 프록시 검사 성공 | 추론 endpoint HTTP/TLS·서비스 proxy/인증 갱신·모델 권한 성공은 미확인 |
| 최초 오류 | 기존 실행 code1/error10/item.completed2/turn.failed1, 완료 턴0 | 원본 stderr/JSONL 유실. 첫 오류 원문·모델·권한·네트워크 중 직접 원인 UNKNOWN |
| 기존 로그 | Operational 채널 과거 조사 IsEnabled=false. 이번 서비스 .codex/log 열람 UnauthorizedAccessException | 다른 계정 ACL 우회·비밀 복사 안 함. 기존 계정 내부에서만 비밀 제거 진단 필요 |

FORBIDDEN_ITEM 2개는 허용 목록 밖 item이라는 검증 결과다. 원본 item 유형이 없어 실제 위험 도구 호출이었다고 단정할 수 없다. 현재 구현은 위험 도구 보고와 미지원 유형을 구분해 기록하되 모두 기존 검증 기준대로 거부한다.

## 1순위: 서비스 실행 환경의 최소 정합화

실제 추론 전에 아래만 진행하는 변경 계획이다. 이번에는 배포·setup·서비스 실행을 하지 않았다.

1. 서비스 자신의 실행 문맥에서 비밀을 제외한 계정/세션/HOME/CODEX_HOME/cwd/TEMP 쓰기 가능 여부·프록시 유무·허용된 config 키·기존 sandbox 로그 오류 분류를 수집한다. auth 파일 원문/keyring/토큰은 수집하지 않는다. 현재 사용자 권한으로 서비스 로그를 읽을 수 없으므로 서비스 소유 실행 경로가 필요하다.
2. 설치된 공식 배포 단위를 별도 후보 폴더에 전부 준비하고 4개 파일 SHA·버전을 고정한다. 현재 단독 exe와 승인된 42개 파일·51개 무결성 핀을 덮어쓰지 않는다. 서비스에서 --version 및 기능 설정 확인만 먼저 한다. 이 검사는 모델 성공 판정이 아니다.
3. 모델은 기존 승인·계약과 호환되는 명시 값의 차이를 먼저 검토한다. 런타임·모델·Sandbox를 한꺼번에 변경하지 않는다. sandbox 설치가 필요하면 로컬 계정/ACL/방화벽 등 영향을 공식 문서와 대조하여 별도 변경안을 제출한다. setup 실행은 관리자 권한·일회성 설정이 필요할 수 있으며 무인 실행 중 UAC를 요구하는 구조는 부적합하다.
4. 승인 목록에는 검토된 변경 SHA만 등록하고 실행 승인과 분리한다. 두 기존 부팅 작업은 변경하지 않는다. 서비스 로그 보존 사전검사 실패 시 추론 전에 HOLD한다.

[공식 Windows sandbox 안내](https://learn.chatgpt.com/docs/windows/windows-sandbox)는 native Windows 실행과 sandbox 구성을 다루지만, 이 고정 단독 exe/keyring/Session 0 조합의 성공을 보증하지 않는다. **보조 파일 누락은 수정 후보이지 확정 원인이 아니다.**

## PIN 없는 대안과 비용

| 우선순위 | 구조 | 비용·조건 | 현재 상태 |
|---|---|---|---|
| 1 | 기존 Windows 서비스 계정 + 완전한 공식 CLI 배포 + 유지되는 sandbox | 기존 구독·PC 범위, 추가 API 결제 없음. 일회성 설치/권한 확인 필요 | 추론 NOT_VERIFIED |
| 2 | 동일 PC의 Linux VM + 부팅 시 VM/worker 서비스 | 기존 하드웨어 활용 시 추가 서버 임대료 0, RAM/디스크/전력 소비. Windows Pro는 확인했으나 vmms/vmcompute/WslService/LxssManager는 이번 조회에서 발견되지 않음. 기능/CPU/메모리·부팅 가능 여부 추가 검사 필요 | 미설치·미실행 |
| 3 | 기존 별도 Linux 장치 + systemd worker | 장치 보유 시 추가 임대료 0, 전력·운영 관리 비용. 새 장치 구매가는 사양/판매처 확인 전 미확정 | 설계안 |
| 4 | 신뢰된 Linux VPS + 서비스 worker | DigitalOcean Basic 2GiB/1vCPU/50GiB 월 US$12 표기가 확인됨. 세금·백업 등 별도, 최소 권장 사양이나 성능 보장으로 해석하지 않음 | 계약/구매/배포 없음 |

[Hyper-V AutomaticStartAction](https://learn.microsoft.com/en-us/powershell/module/hyper-v/set-vm)은 호스트 서비스 시작에 맞춘 VM 자동 시작 설정을 제공한다. 이것을 이용하면 Windows 대화형 로그인에 의존하지 않는 구조를 설계할 수 있으나 이 PC에서 실제 잠금화면 부팅 검증은 하지 않았다. 기능 활성화가 재부팅을 요구하면 별도 승인까지 중지한다. 디스크 암호화의 부팅 전 입력 요구도 확인하며 보호 기능을 임의 해제하지 않는다. WSL 실행 파일의 존재만으로 PIN 전 서비스 실행이 입증되는 것은 아니다.

[공식 VPS 가격표](https://www.digitalocean.com/pricing/droplets). 서버 임대료는 ChatGPT 사용량 비용과 별개다. 새 유료 서버도 자동 계약하지 않는다.

## 구독 인증을 유지하는 Linux 실행 조건

[공식 인증 안내](https://learn.chatgpt.com/docs/auth)에서 headless device-code 로그인을 지원한다. 대상 Linux 계정에서 직접 공식 로그인하고 대표가 휴대전화 브라우저로 최초 승인하는 방향을 검토한다. 기존 Windows 토큰·인증파일을 복사하지 않는다. 계정/조직 정책에서 해당 로그인이 허용되는지 먼저 확인해야 한다.

[공식 CI/CD 고급 안내](https://learn.chatgpt.com/docs/auth/ci-cd-auth)는 ChatGPT 관리 인증을 신뢰된 영속 runner에서 직렬로 사용하고 Codex가 갱신한 인증 상태를 보존하는 방식을 설명한다. 이 문서가 요구하는 file 저장 방식은 현재 Windows keyring과 다르므로 그대로 적용하지 않는다. Linux 대상의 저장 방식·최소 권한·암호화·갱신 및 만료 시 HOLD를 별도 검토한다. 문서의 인증 복사 예제는 사용자 금지 조건 때문에 사용하지 않는다. 무인 운전 중 인증 만료/취소/정책 변경에 따른 재인증까지 영구히 무인이라고 보장할 수 없다.

원격 장치에는 SQLite 파일을 네트워크 공유로 열게 하지 않는다. 기존 Windows 중계 서버가 로컬 SQLite의 유일한 업무 원장을 유지하고, 인증된 사설 연결을 통해 비밀 없는 승인 작업·계약SHA·checkpoint·단일 claim lease·결과만 교환하는 확장안이 필요하다. 단절/중복 결과는 HOLD, 중단 업무 자동 재시도 없음. 큐 연결은 현재 로컬 Worker에서만 오프라인 검증됐으며 Linux 전송 연결은 구현·검증 전이다. 구독 사용량 한도·단일 실행·대표 승인·4역할 검증기·아트 최종 승인·MANUAL 정책을 그대로 유지한다.

[조직 Codex service accounts](https://learn.chatgpt.com/docs/enterprise/service-accounts)는 pay-as-you-go 전용이고 Windows 로컬 서비스 계정과 다른 개념이다. 현재 개인 구독의 추가 비용 없는 대체물로 취급하거나 자동 도입하지 않는다. 조직 access token 또한 현재 계정 자격·비용 확인 전 제안 수준이다.

## 별도 승인 후에만 가능한 단발 재시험

오프라인 차이 수집·변경 SHA 검토·서비스 자체 로그 보존 사전검사를 먼저 완료해야 한다. 새 서비스용 단일 슬롯과 계약에 대표가 명시적으로 승인한 후에만 실제 읽기 전용 최대1회/90초/자동 재시도0을 실행한다. 기존 CONSUMED12개와 로그인 Worker 승인 대기 슬롯 재사용 금지.

시험 직전 공식 구독 인증·5시간/주간 사용량·승인 범위·정본/코드SHA·큐claim을 재확인한다. 실제 로그온 유형/세션·CLI SHA·명시 모델·설정·cwd·request ID·종료코드·토큰 사용량을 기록한다. 원본 JSONL은 해당 계정 DPAPI 보호, 정제 stderr/최초 error/turn.failed는 동일 보호 프로필에 보존한다. 원문 외부 게시 금지. PIN 입력 전 성공을 증명하려면 현재 로그인한 세션에서 성공했다는 결과만으로 판정하지 않고 부팅 후 대화형 로그인 전 상태라는 근거도 필요하다. 이번에는 재부팅/로그아웃하지 않는다.

성공 판정은 큐→실제 추론→기존 JSONL/출력 검사 PASS→Discord 전달 확인 모두 충족할 때만 한다. 게임 변경0·정책 유지·사용량/재시도 제한을 별도로 확인한다. 실패 시 NOT_VERIFIED/HOLD 후 중지한다.

## 이번 작업 보호·검증

CLI 파일/디렉터리 읽기·SHA 재계산과 OS/서비스 목록·상태 파일 확인 및 공식 문서 조사만 수행했다. 서비스 로그 접근 실패를 숨기지 않았다. 조회 명령의 exit1은 선택 서비스가 발견되지 않은 결과로, 모델 실행 종료 코드가 아니다. Bot/Art 상태 파일 RUNNING, ai_worker_enabled=false/Art ai_calls0 확인. 부팅 작업·승인 목록·SQLite·실행 코드·인증/ACL/프록시/방화벽 변경0. MANUAL 유지, 실제 AI·Discord 게시·게임/Save/Unity·Push/merge·설치·자동로그인·PC 재부팅/종료0. 문서 변경만이므로 기존 31개 테스트를 반복하지 않는다.
