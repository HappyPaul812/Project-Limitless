# Issue #66 사용량 오류 진단 보존 수정·오프라인 검증 — 2026-10-26

**OFFLINE_PASS / NOT_DEPLOYED / AI_EXECUTION_HOLD / MANUAL**. 바울 대표가 승인한 격리 수정·오프라인 검증만 완료했습니다. 실제 사용량/모델 호출과 운영 배포는 하지 않았습니다. 과거 오류의 원시 응답을 복구한 결과가 아닙니다.

## 수정 범위

- usage_bridge.py: 기존 OFFICIAL_READ_REJECTED/HOLD를 유지하면서 실패 RPC method/id/error.code 및 정제 message/data를 기록합니다. malformed JSON·비객체·잘못된 응답·조기EOF·timeout도 기존 실패 사유/단계·고정 코드 위치·실제 app-server 종료코드와 함께 기록합니다. stdout 계정/인증/사용량 원문과 프로세스 argv/환경값을 기록하지 않습니다.
- 신규 usage_diagnostics.py: 서비스 자기 USERPROFILE/.codex/limitless-usage-diagnostics/usage-<로컬난수>.json 전용 기록기입니다. 기존 서비스 프로필을 고정하는 service_environment 뒤에만 연결합니다. 다른 CODEX_HOME·symlink·Windows reparse point를 거부하며 기존 ACL/소유자/인증은 변경하지 않고 이미 보호된 .codex의 ACL을 상속합니다. 일반 사용자 기록기를 운영 경로로 전환하지 않습니다.
- service_ai.py: 검증된 서비스 CODEX_HOME의 기록기를 연결하고 SERVICE_READINESS 단계를 전달합니다. 두 사전 실행기 service_batch_once.py/service-final-once.py도 OFFICIAL_USAGE_AND_APPROVAL 단계를 명시합니다. 오류 상세는 공용 result/Discord/SQLite에 넣지 않고 기존 Denied 사유만 전달합니다.
- tests/fake_usage_app_server.py와 test_usage_diagnostics_delta.py: 새 진단 경계만 검사합니다. 기존 VERIFIED 전수시험은 반복하지 않았습니다.

## 보존·마스킹·출력/시간 상한

error.code는 실제 응답의 정수 코드(32bit), method/id는 우리가 발행한 RPC 값입니다. message는 알려진 진단 어휘만 남기고 환경 비밀·이메일·주소/경로·인용문·임의 식별자/숫자/알 수 없는 단어를 마스킹합니다. 정제 message 최대2048자, data는 상태코드/재시도초/정제 reason·message 등 제한된 필드만 보존하고 임의 필드는 버리며 누락 수를 기록합니다. 따라서 **원문과 동일한 문자열 보존이 아니라 필요한 진단 구조를 보존하는 방식**입니다. 과도한 마스킹으로 일부 원인 설명이 사라질 수 있으며, 이를 보완하려고 비밀 원문을 출력하지 않습니다.

stderr는 별도 비동기 task로 동시에 끝까지 drain하되 원시 메모리는32KiB까지만 보유하고 이후 버립니다. 최종 stderr는 정제4096자·truncated/총bytes를 기록합니다. 오류 뒤 stdout도 저장하지 않고 drain하여 wait/파이프 교착을 피합니다. stdout 라인65,536byte/응답당최대64event, JSON 기록24KiB, 로그 폴더최대64파일이며 용량이 차면 기존 로그를 삭제하지 않고 조회 전 HOLD합니다. 디스크 상한은 이 기록기 전용 폴더 기준입니다.

기존20초 상한을 늘리지 않고 RPC/생성18초와 종료 정리 최대1.8초로 나눴습니다. 정상 종료대기0.8초→terminate대기0.5초→kill대기0.3초, stdout/stderr drain각0.1초입니다. 파일시스템 자체 I/O 지연이나 OS 프로세스 스케줄링까지 hard realtime 보증하는 것은 아닙니다. kill 뒤에도 종료 확인이 안 되면 종료코드는 null/cleanup NOT_VERIFIED·HOLD로 남고 무한 대기하지 않습니다. 정상 응답 뒤 비정상 종료도 APP_SERVER_EXIT_NONZERO로 차단합니다.

로그 경로 준비 실패 시 subprocess0회, 최종 로그 쓰기 실패 시 정상 응답도 HOLD입니다. 로그 폴더/파일은 신규 생성만 하고 ACL 조정·기존 기록 삭제는 하지 않습니다. 운영 승인·응답 파서·Sandbox·인증·RPC allowlist·모드·자동재개 정책은 완화하지 않았습니다.

## 오프라인 결과 — 22 PASS / 0 FAIL

정상 응답, initialize 거부, 인증401, 권한403, 한도429, 잘못된 JSON/비객체/usage, 조기종료17, timeout, 비정상종료23, stdout 초과, stderr 대량출력, 정상 한도소진·ordinary허용false의 기존 gate USAGE_HOLD, 민감값 마스킹·로그 크기·64파일상한·경로/reparse 차단, 기록실패/준비실패 HOLD, 서비스 기록기/두 실행기 단계 연결을 확인했습니다. 401/403/429/-32603/17/23은 가짜서버 시험값이며 **과거 실제 장애 코드가 아닙니다**.

가짜서버는 Python stdin/stdout/stderr/time만 사용하며 네트워크·Codex·인증 모듈을 사용하지 않습니다. 테스트에서 create_subprocess_exec를 이 fixture로 한정하고 실제 Codex 대신 NEVER_EXECUTE_CODEX sentinel을 확인합니다. timeout분기는18초 대신0.15초로 축소해 검증했습니다. 실제모델0·공식실조회0·자동재시도0입니다.

첫 테스트 실행은 Codex sandbox 내부 Windows asyncio 파이프 생성에서 대기하여 **그 진단 테스트 세션만 종료**했습니다. 같은 신규 fixture 검사를 승인된 도구 실행 경계 밖에서 수행해 통과했습니다. 이것은 운영 서비스/CLI의 오류가 아니며 운영 프로세스 종료·재시작0입니다. 대형 마스킹 입력의 정규식 비효율을 수정한 최종본22건이2.025초에 통과했습니다. Python7파일 구문·git diff --check PASS. 테스트 기록은 Evidence/UsageDiagnosticsPreservation20261026/에 있습니다.

## 기존 운영 보호

운영 worktree는 codex/discord-ai-collaboration / fcd0488c 그대로입니다. 운영52파일(코드/진입점/CLI/approval/wrapper/기존부팅XML사본) SHA 불일치0. SQLite readonly immutable 조회로 핵심8테이블내용동일·MANUAL 확인. DB파일 전체 바이트 불변이나 현재 OnBoot/CIM 상태를 새로 검증했다고 주장하지 않습니다. 원본 main/status 동일, Unity/게임/Save/인증/ACL·운영배포·부팅작업수정·봇재시작·A·Push/병합/종료/재부팅0입니다.

## SHA256 — 보존된 운영 원본 vs 격리 수정본

이 표는 같은 fcd0488c 소스의 **운영 worktree 실제 원본 bytes**와 **격리 후보 실제 bytes**를 비교합니다. checkout 줄바꿈까지 포함하므로 Git blob SHA와 다릅니다. 신규파일에는 원본 SHA가 없습니다.

| 경로 | 수정 전 SHA256(보존 원본) | 수정 후 SHA256(격리 후보) |
| --- | --- | --- |
| Tools/DiscordAICollab/collab/usage_bridge.py | d410857d4046721cf63bbf380b78dc1e710d81bba101c2780e41c6cfb6587751 | 2bc508b260f9bedaebbe58f27d663b7c989b39b66263c28bf949fb712483c0af |
| Tools/DiscordAICollab/collab/usage_diagnostics.py | 신규 | 869deb9382404fae002055e562852a394e6fddbddb1b553196c21bb9667c29e2 |
| Tools/DiscordAICollab/collab/service_ai.py | 6bc4351fb5288b0edc2e7ad6adeeb46864aa197144291f7529073dd81895ddeb | c72d88ee558824287c0284e349c1b51e65f906782eb3f0b4f8d39fa963980215 |
| Tools/DiscordAICollab/collab/service_batch_once.py | de3ce4c90fbad6c2de6e99d278b3ab10ca0e65ab9d16106ef09f79360ab54bdb | 1dfa673007b71ebc2c70b75d6f107cee3a558f8e4fc4070ff032e7ac2be8be70 |
| Tools/DiscordAIServiceDiagnostics/service-final-once.py | 1c21aa6b584e0b0d166551658f96085652d9ec27b49d02d44528df46531127be | 36a3213652d883c5c1e849babe8bd755477d9ba33e7e827e442ac99a12ff13ca |
| Tools/DiscordAICollab/tests/fake_usage_app_server.py | 신규 | 9f2ebce678044a2aeb57098efa1d1c423811db58cf7b85dffb5bde81baf5bd05 |
| Tools/DiscordAICollab/tests/test_usage_diagnostics_delta.py | 신규 | 340de06bd402688a8974629ee5add469177c8a81710906e03c52cc45c993ef69 |

## 승인 목록과 다음 실전 검증 조건

**현재 approval JSON·승인 SHA·검증기 고정값은 갱신하지 않았습니다.** 이 브랜치에는 새 collab모듈이 있어43개가 되지만 기존 단발 PowerShell 검증기는42개를 요구하므로 그대로 실행하면 HOLD가 정상입니다. 최신 approval의 runtime_sha256, 변경된 기존 collab3파일(usage_bridge/service_ai/service_batch_once), 신규usage_diagnostics, runner SHA, bootstrap count/고정 manifest 등 **실제 영향이 있는 항목만 별도 리뷰·승인**해야 합니다. 공식 codex.exe 경로/SHA와 기존 나머지 승인 항목을 무조건 덮어쓰면 안 됩니다. 수정후 SHA들은 증거이지 운영승인 자체가 아닙니다.

다음 사용량-only 실전 시험 전에는 (1) 바울 대표의 최대1회/20초/재시도0 명시승인, (2) 이번 격리diff/새모듈/의존SHA와 적절한 배포방식 별도 승인, (3) 서비스계정 Session0/logon4·기존CLI/자기인증·보호로그경로 용량/권한·MANUAL·기존업무/부팅 보호 확인, (4) **모델 subprocess/Discord게시/A활성화를 포함하지 않는 usage-only 진입점**의 고정SHA/새 request ID 확인이 필요합니다. 과거 모델 단발 runner는 사용량 성공 후 추론으로 이어지므로 다음 조회시험에 재사용하지 않습니다. 소비마커/승인슬롯 삭제·우회0. 실제 서비스 ACL 상속/Session0 동작은 이번 합성 프로필 검증으로 대체하지 않으며 별도 실전 검증 대상입니다.

브랜치 codex/discord-usage-diagnostics-20261026, 기반 fcd0488ccc8a140cd08661822383f7ec3194e058. 최종 로컬 커밋은 Issue66/최종보고 참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. **수정·오프라인 검증 후 여기서 중지하며 실제 사용량 조회/AI/A는 실행하지 않습니다.**
