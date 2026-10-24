# 공식 Codex 전체 패키지 격리 무결성 검증

2026-10-24. 선행 834d785c82c3d8ed9662422729eb33941fdd882f. 이번 실제 AI 추론0 / 운영 배포0 / 사용자 Worker 실행0.

## 판정

**PACKAGE_INTEGRITY_PASS / SERVICE_CONTEXT_HOLD / PIN 전 추론 NOT_VERIFIED.** 공식 동일 버전 전체 패키지를 실제 확보·안전하게 격리 해제했으며 무결성에 근거한 최소 복구안을 제출한다. 원인 확정·Sandbox 성공·서비스 실행 성공으로 확대 해석하지 않는다.

## 공식 패키지와 검증 근거

- 공식 source: https://releases.openai.com/codex/releases/0.164.0-alpha.11.1/codex-package-x86_64-pc-windows-msvc.tar.gz
- 공개 metadata tag: rust-v0.164.0-alpha.11.1. 아카이브 게시 SHA와 다운로드 실측 SHA가 동일: `410520596d802c955859446bb75f2939f911a97eb738a68f9adc1cb377cd2a57`.
- 격리 경로: `F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\data\official-package-review-20261024\package`.
- 아카이브 51개 항목(디렉터리 포함), 파일45개. 절대/상위탈출/역슬래시 경로·링크·특수 파일을 거부한 후 data filter로 추출했다. 기존 폴더를 덮어쓰지 않았다.
- codex-package.json의 version=0.164.0-alpha.11.1, target=x86_64-pc-windows-msvc, entrypoint=bin/codex.exe, resourcesDir=codex-resources, pathDir=codex-path 확인. manifest SHA `5a30b23d5d3cfec6e9dc1642720e767b01549422bce22bab6db4e66ec20d2537`.
- 본체/host/runner/setup 4개 Authenticode **Valid / OpenAI OpCo, LLC**. 포함 voice-host도 Valid. rg.exe는 **NotSigned**로 별도 표시하며 유효 서명이라고 주장하지 않는다. 전체 파일은 공식 아카이브 SHA 및 파일별 실측 SHA로 추적한다. DLL별 Authenticode는 이번 검사 대상 아님.
- 새 패키지의 바이너리는 --version을 포함해 실행하지 않았다. manifest 버전 확인과 실제 실행 버전 확인을 구분한다.

[공식 배포 문서](https://learn.chatgpt.com/docs/enterprise/windows-deployment)는 standalone runtime을 함께 유지하고 설치 계정/경로/버전 고정을 검증하도록 설명한다. 이번에는 설치기를 실행하지 않고 공식 전체 아카이브만 격리 검증했다. 전체45개 파일·해시·서명 결과는 [근거 JSON](Evidence/ServiceCLI_OfficialPackage_20261024.json)에 보존한다. 실행 파일은 Git에 추가하지 않는다.

## 본체·보조 파일 비교와 실제 역할

| 파일 | 격리 공식 패키지 내부 상대 경로 | 새 실측 SHA256 |
|---|---|---|
| codex.exe | bin/codex.exe | 4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73 |
| codex-code-mode-host.exe | bin/codex-code-mode-host.exe | 89713bec82069f1fefc70ec6d52fcb96d79cfd5f0d8d831aedde0b2005a39402 |
| codex-command-runner.exe | codex-resources/codex-command-runner.exe | 61d0b76d62cd1b852cb4b404a025bb9b26cb927550c0d2225fd17ee9b743db62 |
| codex-windows-sandbox-setup.exe | codex-resources/codex-windows-sandbox-setup.exe | 834d3be787e1c669ea9b18e5f33799b743819a4e6e84bd3d0b6f7b9158564565 |

Code Mode host는 해당 통신·실행 호스트 경로에 필요하다. runner는 legacy elevated sandbox의 제한 토큰 명령을 실행하고 setup는 그 초기 설정을 담당한다. [공식 Sandbox 설계](https://openai.com/index/building-codex-windows-sandbox/) 및 이전 [역할 조사](ServiceCLI_OfficialRecovery_Review_20261024.md) 참고. 매 순수 텍스트 추론에서 세 파일 모두 실행된다는 뜻은 아니다. 당시 실제 helper 호출 여부는 미확인이다.

현재 서비스는 `Tools/DiscordAICollab/data/runtime/codex/c6144bc19b2705f2/codex.exe` 단독이며 SHA `c6144bc19b2705f25a40d8488b3d57d17a223c5df3c5f65e3c58d72601956c1e`. 보조3개/manifest/resources/path 구조가 없다. 일반 사용자 본체 SHA `1103e9e09c71fa2cd0afd6d8f3b9a2d63174fe8efd7cb5bd09f1cb14ef58a098`도 공식 격리 패키지와 다르다. 두 기존 파일의 표시 버전 동일·유효 서명이라는 앞선 결과는 유지하며, 사용자 파일을 서비스로 복사하거나 혼합하지 않았다.

## 서비스 자체 진단의 확인 범위와 차단 사유

현재 OS 계정은 PAUL-PC\windows, 관리자 false이다. sandbox 밖 메타데이터 재조회에서도 두 예약 작업은 CimJobException으로 읽기 거부, 서비스 config.toml/log는 UnauthorizedAccessException이다. `.codex/.sandbox/sandbox.log`의 ItemNotFoundException은 상위 프로필 접근 제한이 있는 상태이므로 실제 부재의 확정 근거로 쓰지 않는다.

기존 batch-registration-check.ps1는 관리자 확인 → Get-Credential의 로컬 서비스 암호 입력 → Password/Batch 작업을 임시 등록하는 구조다. 기존 작업의 저장된 암호를 읽거나 재사용하는 경로는 없다. 현재 환경에서 해당 보안 문맥을 얻을 수 없어 **새 서비스 계정 자체 수집은 실행하지 못했다**. ACL 변경·토큰/암호 추출·다른 사용자 인증 복사·UAC 화면 전환을 하지 않았다. sandbox escalation은 UAC 관리자 승격이 아니며 이를 우회로 취급하지 않는다.

기존 서비스 자체 기록에서 확인한 사실은 다음과 같다. 이번 새 검증으로 오인하지 않는다.

- batch-account-check: Session0/logon4/비대화형/서비스 자기 프로필/cwd=Tools/DiscordAICollab/종료0. 당시 CODEX_HOME 환경값 null이지만 이후 service_environment는 자기 `.codex`로 명시 설정한다.
- service_ai.py는 승인한 절대 CLI 경로/SHA를 우선 사용하고 원래 PATH만 전달한다. PATH의 서비스 실제 값과 helper 탐색 실행 결과는 미보존이다. 새 공식 package는 manifest의 bin/resources/path 구조를 보존해야 한다.
- 서비스 자기 ChatGPT 인증·공식 사용량 및 GitHub 읽기 과거 PASS. 추론 endpoint/TLS/WebSocket·Sandbox 제한 자식 프로세스 성공은 별도 미검증이다.
- exec는 read-only/approval never/도구 차단/--ignore-user-config/명시 모델 없음. login·usage의 config 경로와 차이가 있다. 현재 서비스 config와 실제 선택 모델/Sandbox 구현은 아직 미확인이다.
- 원본 stderr/JSONL 유실. 기존 offline-log-diagnosis도 SERVICE_LOG_ACCESS_DENIED_NO_ACL_CHANGE. log 경로와 `.sandbox/sandbox.log`, 이후 실행부터의 `.codex/limitless-diagnostics/<request>`가 계정 내부 조사 대상이다. auth 파일 원문은 대상 제외.

따라서 최초 turn.failed 직접 원인 **UNKNOWN**. 불완전 배포는 확인된 결함 후보이며 모델·네트워크·설정·권한 중 직접 원인을 확정하지 않는다.

## 승인 전 미적용 최소 복구안

새 공식 설치 대상 제안 경로는 `F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\data\runtime\codex\official-0.164.0-alpha.11.1-x64\`이다. **이번에 생성하거나 복사하지 않았다.** 공식 설치기의 계정/설치 레이아웃을 먼저 검토하고, 이 장치 전용 배포 경로를 지원할 수 없으면 제안을 재검토한다. 서비스 자기 프로필 기본 설치는 별도 대안이며 경로가 달라지면 승인 전에 확정해야 한다.

1. 공식 version 고정 설치·배포 단위로 전체45파일 구조를 유지한다. 격리 폴더를 운영용으로 즉시 참조하지 않는다. 임의 helper 복사 금지. 추가/누락·SHA 변화가 있으면 HOLD 및 재검토.
2. 운영 승인 JSON의 기존 service_codex.path는 위 기존 단독exe, sha256은 c6144…에서 새 배포의 bin/codex.exe / 4fd2…로 변경하는 최소 후보이다. 버전은 동일하다. 42 Python 및 그 runtime_sha256은 변경하지 않는다. 새 패키지 전체 파일의 전용 무결성 검증 방식·manifest SHA를 별도 승인하며 등록 자체는 실행 허가가 아니다. 현재 boot-approval SHA는 아래 보존 결과와 같고 새 JSON SHA는 미적용 상태이므로 만들거나 예측하지 않는다.
3. 서비스 HOME/CODEX_HOME·keyring·인증·구독 방식은 유지한다. User PATH를 복사하지 않고 절대 CLI 경로 및 공식 package 탐색을 사용한다. 기존2개 OnBoot XML/boot-service.ps1·실행 중 bot/art를 수정·재시작하지 않는다.
4. 적법한 관리자 실행 문맥에서 서비스 자신의 암호를 로컬 보안 입력으로 제공할 수 있을 때만 임시 **무모델** batch 진단을 허용한다. 암호를 채팅/로그/파일로 받지 않는다. 이 일회성 설정 필요와 매 부팅 PIN 필요는 다르며, 정상화 후 운영 목표는 PIN 전 실행이다.
5. 무모델 시험에서는 서비스 계정/세션, CLI 전체 SHA/서명, 버전, whitelist config, HOME/CODEX_HOME/PATH 구성, cwd/TEMP/읽기 권한, helper 탐색 및 기존 오류 분류를 확인한다. 별도 고정 무해 명령 Sandbox 시험이 필요하면 그 부작용을 먼저 검토한다. setup는 로컬 계정/ACL/방화벽에 영향을 줄 수 있으므로 자동 실행하지 않는다. Sandbox 완화 금지.

## 되돌리기

변경 직전 승인 JSON의 service_codex 세 필드·파일 SHA/ACL과 설치기 소유 경로/PATH를 기록한다. 기존 runtime·인증은 삭제하지 않는다. 실패하면 **변경한 참조만** 원래 path/sha/version으로 되돌리고 전체42 SHA를 재검증한다. 승인 JSON 전체를 과거 백업으로 무조건 덮어쓰지 않는다. 새 설치 제거는 관리 범위에만 한정하고 CODEX_HOME 보존. Sandbox setup가 시행됐다면 CLI 참조 복원만으로 ACL/방화벽이 복원되지 않으므로 별도 승인·별도 복원 기록이 필요하다. 이번에는 그러한 변경0이다.

## 별도 승인 후 단발 추론 계획

공식 배포·서비스 문맥·보존 로그 준비가 확인된 후 새 서비스용 승인 슬롯을 만든다. 기존CONSUMED12와 사용자 승인 대기1은 재사용하지 않는다. 대표가 정확한 계약·역할 정본·코드/CLI SHA·서비스 시험을 승인할 때만 90초/읽기 전용/최대1회/응용 자동 재시도0으로 진행한다. 직전 구독 인증·공식5시간/주간 usage·예약·claim·중복·권한을 재검증한다.

서비스 자신의 DPAPI 원본 JSONL 및 같은 보호 경로의 정제stderr·첫error·turn.failed·종료/토큰 사용량·계정/세션·설정·request를 남긴다. 보존 사전검사 실패면 호출 전 HOLD. 기존 출력/JSONL 검증 PASS와 실제 Discord 전달 확인 후에만 VERIFIED. PIN 전 실행은 부팅 후 대화형 로그인 전이라는 별도 상태 근거가 필요하며 현재 로그인한 상태의 Session0 시험만으로 그 전체 목표를 입증하지 않는다. 이번 재부팅/로그아웃/추론/Discord 게시0.

## 보호 결과

42 Python SHA 불일치0, boot-approval SHA `0cd2ce55f7e23b44190e7f4786c2e9f68e2e291b1888e5768ea606f391c8d2d6` 보존. SQLite 읽기 전용 MANUAL·사용자 슬롯 WAITING_FOR_OWNER_APPROVAL 1건. Bot/Art RUNNING heartbeat 갱신·worker 비활성/Art AI0. 부팅 작업 XML 현재 직접 비교는 권한 부족으로 불가하나 변경 명령0. 승인/운영CLI/인증/실행코드/게임/Save/Unity·Push/merge·자동로그인/PIN/유료API 변경0. 과거 전체 테스트 반복0. 이번 생성물은 격리 패키지·해시 근거·보고서뿐이다.
