# 서비스 Codex 공식 배포 복구 사전 검토

2026-10-24. 기준 2704d1a4ccf4a7308d2552b1343a7c0260a44b41 / codex/discord-ai-collaboration. 실제 AI 0회. 파일 복사·설치·교체·Worker 실행 없이 조사했다.

## 결론

**공식 설치 기반으로 동일 표시 버전의 완전 패키지를 준비할 경로는 확인했다. 복구 실행 및 PIN 전 추론 성공은 미검증이다.** 서비스 폴더의 단독 exe는 공식 standalone 배포 기준에서 불완전하다. 다만 첫 실패의 원본 JSONL/stderr가 유실되어 누락 파일을 turn.failed 직접 원인으로 확정할 수 없다. 로그인 후 Worker를 최종 해법으로 채택하지 않는다.

## 경로·SHA·버전 실측

사용자 설치 폴더: `C:\Users\windows\AppData\Local\OpenAI\Codex\bin\2993767bd1dfb009`

서비스 폴더: `F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\data\runtime\codex\c6144bc19b2705f2`

아래 3개 보조 exe는 사용자 폴더에 있고 서비스 폴더에는 없다. 해시는 **현재 사용자 설치본을 재계산한 값**이며 복구용 공개 패키지 내부 파일 해시라고 주장하지 않는다.

| 파일 | SHA256 | 역할·필요성 |
|---|---|---|
| 사용자 codex.exe | 1103e9e09c71fa2cd0afd6d8f3b9a2d63174fe8efd7cb5bd09f1cb14ef58a098 | CLI 본체 |
| 서비스 codex.exe | c6144bc19b2705f25a40d8488b3d57d17a223c5df3c5f65e3c58d72601956c1e | 승인 목록의 고정 실행 파일과 일치 |
| codex-code-mode-host.exe | 2727abaa2b30eb4afbe64b1da51a26cb594dd18dda82c6292b5373b15a758257 | Code Mode 통신·실행 호스트. 해당 실행 경로에서 필요; 과거 실패에서 실제 호출됐는지는 불명 |
| codex-command-runner.exe | 598fbfbad52ccd717b60aa94185541ccecf27e849349026f93884e9519f7ac61 | legacy elevated sandbox의 제한 토큰 명령 실행 담당. 그 경로의 명령 실행에 필요 |
| codex-windows-sandbox-setup.exe | 5765c3951b2c247611dec18176660bd3d2e81f20baba61096b454888ee821b2c | elevated sandbox 초기 설정 담당. setup 경로에서 필요; 매 추론마다 반드시 실행된다는 뜻은 아님 |

두 CLI 모두 이번 --version 결과 `codex-cli 0.164.0-alpha.11.1`, 길이 354,661,168바이트, Authenticode Valid / OpenAI OpCo, LLC. 동일 크기·버전·유효 서명도 바이트 동일성을 뜻하지 않는다. 서로 다른 SHA의 이유는 조사하지 못했으며 변조로 단정하지 않는다.

--version은 현재 일반 사용자 문맥에서만 실행했다. Codex sandbox의 프로필 TEMP 쓰기 제한으로 arg0 임시 경로/PATH alias 생성 Access denied 경고가 났으나 버전 출력은 성공했다. 이 경고를 과거 서비스 장애의 증거로 사용하지 않는다. CLI 초기화가 임시 경로 생성을 시도할 수 있으므로 이를 순수 파일 읽기와 구분한다. 서비스 프로세스나 모델·helper는 실행하지 않았다.

[공식 배포 안내](https://learn.chatgpt.com/docs/enterprise/windows-deployment)는 standalone 런타임을 함께 유지하고 단독 codex.exe 복사를 완전한 패키지로 취급하지 않도록 명시한다. 설치 계정·버전 고정·사용자 PATH·제거 절차를 별도로 검증해야 한다. SYSTEM 설치가 자동으로 모든 사용자 설치가 되는 것도 아니다.

[공식 sandbox 설계 설명](https://openai.com/index/building-codex-windows-sandbox/)은 setup/runner의 역할을 설명한다. [공개 Code Mode 호스트 진입점](https://github.com/openai/codex/blob/main/codex-rs/code-mode-host/src/main.rs)은 통신 transport를 받아 호스트를 실행한다. 이 자료는 역할 근거이며 현재 alpha 바이너리의 동일 소스 재현 증명은 아니다. 셸이 금지된 읽기 요약 시험은 도구 실행 경로를 사용하지 않을 수도 있으므로, 누락 3개 모두가 그 실패에서 호출됐다고 단정하지 않는다.

## 서비스 계정과 일반 사용자 차이

| 항목 | 확인된 내용 | 남은 확인 |
|---|---|---|
| 계정/세션 | 과거 batch-context: PAUL-PC\LIMITLESSRelaySvc / Session0 / logon4 / interactive=false | 이번 신규 서비스 배치 실행 없음 |
| HOME/CODEX_HOME | service_ai.service_environment가 OS 토큰·프로필 확인 후 C:\Users\LIMITLESSRelaySvc 및 그 아래 .codex 고정 | 서비스 현재 환경 직접 수집 전 |
| PATH | child_environment가 원래 PATH만 허용 전달. CLI는 승인된 절대 경로 우선, 사용자 PATH 복사 안 함 | 과거 서비스 PATH 전체 값 미보존. 사용자 PATH와 일치라고 주장 못함 |
| cwd | PS launcher가 Tools/DiscordAICollab로 이동. exec -C는 data/service-batch-once-20261024/empty | subprocess cwd 인수는 별도 지정 없어 상위 cwd 상속. 현재 sandbox 사용자 ACL 확인 필요 |
| 인증 | 서비스 자기 저장소의 ChatGPT 인증·공식usage 과거 PASS. 사용자도 자기 저장소에서 과거 PASS | 이번 로그인/usage 재호출 없음. 현재 만료/갱신 상태 불명 |
| 설정 | 사용자 모델 gpt-6.1-sol 기록. 과거 서비스 config SHA는 keyring/chatgpt 두 줄 템플릿 일치 | 현재 서비스 config 접근 불가; 실제 실패 모델 미기록 |
| exec 정책 | --ignore-user-config / read-only / approval never / 모델 명시 없음 / low effort / shell·apps·browser·computer·multiagent·MCP·plugins·hooks·web 차단 | login/usage는 config를 읽지만 exec는 무시. 설정 경로가 다름. Sandbox 구현의 실제 선택은 미확인 |
| 파일·프로세스 | 과거 서비스 batch/python/CLI/login/app-server 및 작업 파일 생성 성공 | 제한 토큰 자식 프로세스·helper 탐색·TEMP 접근은 별개로 미검증 |
| 네트워크 | 과거 서비스 인증·usage·GitHub 읽기 성공 | 추론 endpoint/TLS/WebSocket·서비스 proxy·모델 허용 여부 미검증 |

현재 서비스 로그 `C:\Users\LIMITLESSRelaySvc\.codex\log`는 UnauthorizedAccessException으로 열람 불가. 기존 offline-log-diagnosis.json도 SERVICE_LOG_ACCESS_DENIED_NO_ACL_CHANGE. 추가 후보는 같은 계정의 `.codex\.sandbox\sandbox.log` 및 `.codex\limitless-diagnostics\<request_id>\`이다. 후자는 보존 코드 추가 후 실행부터 생성되며 잃어버린 과거 원본이 있다고 주장하지 않는다. 서비스 자신 또는 적법한 관리자 경로에서 비밀 제거 요약만 확인해야 한다. ACL 변경·토큰 복사·원문 외부 게시 금지. Operational 채널 비활성 기록도 이전 보고대로이며 이번에 활성화하지 않았다.

## 정확한 동일 버전 공식 복구 경로

공식 설치기 공개 코드만 열람했다. 실행·파일 저장·패키지 다운로드는 하지 않았다. 문서의 -Release 고정 및 npm 정확 버전 지정 방식 중 **standalone 공식 설치기 방식 우선**을 제안한다.

공개 release metadata GET은 제한된 실행 환경에서는 실패했으나 sandbox 밖 읽기 전용 조회에서 HTTP200, tag `rust-v0.164.0-alpha.11.1` 확인했다. 서비스 네트워크 성공 근거로 전용하지 않는다.

메타데이터: https://releases.openai.com/codex/releases/0.164.0-alpha.11.1/release.json

| 공개 자산 | 게시된 SHA256 |
|---|---|
| codex-package-x86_64-pc-windows-msvc.tar.gz | 410520596d802c955859446bb75f2939f911a97eb738a68f9adc1cb377cd2a57 |
| codex-package-x86_64-pc-windows-msvc.tar.zst | 938450cbbfb8b700b8d11be34da41b0f5717ffe31b61c8d2162762169e35c3fd |
| codex-package_SHA256SUMS | f94464551dadd814035802e7136bf64527e9453497fb7c566029427fdfcb4c69 |
| codex-npm-0.164.0-alpha.11.1.tgz | 8fade2d9ba872feb14d1e3b8fd9e8da3af387afd28bb57dbf156048c958706cc |

공개 개별 exe digest도 사용자 설치본과 다름: 본체 4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73 / host 89713bec82069f1fefc70ec6d52fcb96d79cfd5f0d8d831aedde0b2005a39402 / runner 61d0b76d62cd1b852cb4b404a025bb9b26cb927550c0d2225fd17ee9b743db62 / setup 834d3be787e1c669ea9b18e5f33799b743819a4e6e84bd3d0b6f7b9158564565. 공개 메타데이터 값이며 미취득 바이너리의 실측값은 아니다.

[공식 설치기](https://chatgpt.com/codex/install.ps1)의 완전성 판정은 codex-package.json / bin\codex.exe / bin\codex-code-mode-host.exe / codex-path\rg.exe / codex-resources\codex-command-runner.exe / codex-resources\codex-windows-sandbox-setup.exe를 포함한다. 3개 exe만 추가하는 안으로는 부족하며 공식 패키지 구조를 유지해야 한다.

## 최소 변경안과 되돌리기

1. 대표의 설정 변경 승인 후, 서비스 계정에 적합한 설치 문맥에서 공식 설치기의 Release를 0.164.0-alpha.11.1로 고정한다. 실행 전 당시 설치기 본문·SHA·부작용·비대화형 옵션을 검토한다. latest 사용 금지. 기존 인증과 단독exe를 덮어쓰지 않는 독립 설치가 불가능하면 HOLD한다.
2. 아카이브 SHA를 위 표와 대조하고 설치 후 전체 파일·manifest·서명·버전·참조·ACL을 검사한다. 내부 파일 SHA는 미취득이므로 아직 미확정이다. 현재 사용자 설치본을 수동 혼합하지 않는다. 패키지가 다르면 승인 전 정확한 차이를 보고한다.
3. 서비스 자신의 로그·설정·helper 탐색을 모델 없이 확인한다. Sandbox 시작 검사가 필요하면 고정된 무해 명령만 사용하는 별도 검증안을 제출한다. setup는 로컬 사용자·ACL·방화벽·정책에 영향을 줄 수 있어 이번에 실행하지 않았다. Sandbox 완화·FullAccess 전환 금지.
4. 기존42 Python과 runtime_sha256 유지. 새 CLI 전용 무결성 manifest를 별도로 검토한다. 향후 service_codex 경로/SHA 변경이 필요하면 해당 값과 관계 설정만 구체화해 별도 승인하고 실행 승인과 분리한다. OnBoot XML 두 개와 boot-service.ps1 변경 없음. 승인 전 기존 참조 유지.
5. 되돌리기는 기존 CLI 참조·승인 파일 SHA/ACL·변경 대상 PATH를 사전 기록하고 변경한 값만 복원한다. 기존 runtime과 인증은 삭제하지 않는다. 제거는 공식 설치기 관리 범위에만 한정하고 CODEX_HOME이나 다른 도구는 보존한다. Sandbox setup까지 시행했다면 CLI 복원만으로 그 부작용이 복원되지 않으므로 별도 추적한다. 승인 JSON 무조건 덮어쓰기 금지.

## PIN 없는 대안과 다음 검증

native Windows가 성립하지 않으면 Linux VM 또는 기존 독립 Linux 장치의 부팅 daemon을 후보로 유지한다. [공식 ChatGPT 인증 CI 안내](https://learn.chatgpt.com/docs/auth/ci-cd-auth)는 신뢰된 영속 runner의 직렬 사용과 Codex가 갱신한 인증 상태 보존을 다룬다. 현재 keyring/Session0 조합에 대한 보증은 아니다. 대상 장치에서 직접 공식 device login하고 Windows 인증을 복사하지 않는 방식을 검토한다. 새 서버 구매·유료 API·자동 로그인 도입 없음. 로그인 후 Worker는 이번 미배포·미실행의 예비 수단이다.

다음 단계는 서비스 문맥의 무모델 metadata/log 수집과 별도 승인에 따른 공식 패키지 준비다. 이후 로그 보존 사전검사·공식 인증/usage·명시 모델/계약/코드SHA·새 서비스 전용 승인을 갖춘 뒤, 대표 별도 승인으로 읽기 전용 추론 최대1회/90초를 계획한다. CONSUMED12개와 사용자 승인 대기 슬롯 재사용 금지. PIN 전 큐→추론→검증→Discord 성공은 해당 시험 전까지 NOT_VERIFIED다.

## 보호 결과

Python42개 재계산 SHA 모두 일치. boot-approval SHA 0cd2ce55f7e23b44190e7f4786c2e9f68e2e291b1888e5768ea606f391c8d2d6 동일. DB 읽기 전용 조회 MANUAL, 사용자 Worker 1건 WAITING_FOR_OWNER_APPROVAL. Bot/Art 상태 RUNNING, ai_worker_enabled=false, Art ai_calls0. 기존12slot 재사용 없음. 실행 코드/승인/인증/작업/SQLite 쓰기·AI/Discord·Unity/Save·Push/merge·재부팅0. 문서만 변경, 과거 전체 테스트 재실행 없음.
