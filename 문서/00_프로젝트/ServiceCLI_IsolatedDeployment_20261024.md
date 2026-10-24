# 공식 CLI 격리 배치 완료 — 서비스 실행 HOLD

2026-10-24. 선행 commit 0a71e56d8e039b475d78c9b2b48ed27c08d49729. 기존 검증 패키지 재사용, 다운로드0, 실제 추론0.

## 설치·무결성 결과

격리 배치 경로:

`F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\data\runtime\codex\official-0.164.0-alpha.11.1-x64`

앞서 검증한 공식 전체 패키지에서 45개 파일을 그대로 복사했다. 기존 파일은 덮어쓰지 않았다. 모든 파일을 사전 근거 JSON과 복사 전후 대조하여 **45/45 SHA 일치**, 추가 다운로드0. manifest/bin/resources/path/voice 하위 구조 유지. 이는 격리 portable 배치이며 PATH·레지스트리·자동 시작을 등록하는 설치기는 실행하지 않았다.

본체와 보조3개 Authenticode 재확인 **Valid / OpenAI OpCo, LLC**:

| 상대 경로 | SHA256 |
|---|---|
| bin/codex.exe | 4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73 |
| bin/codex-code-mode-host.exe | 89713bec82069f1fefc70ec6d52fcb96d79cfd5f0d8d831aedde0b2005a39402 |
| codex-resources/codex-command-runner.exe | 61d0b76d62cd1b852cb4b404a025bb9b26cb927550c0d2225fd17ee9b743db62 |
| codex-resources/codex-windows-sandbox-setup.exe | 834d3be787e1c669ea9b18e5f33799b743819a4e6e84bd3d0b6f7b9158564565 |

rg.exe의 NotSigned와 DLL별 서명 미검사 범위는 [이전 근거](Evidence/ServiceCLI_OfficialPackage_20261024.json)대로 유지한다. 전체45파일의 해시는 공식 게시 아카이브 SHA가 일치한 추출본과 같다. [공식 배포 안내](https://learn.chatgpt.com/docs/enterprise/windows-deployment)의 전체 런타임 유지 원칙을 따랐다. 파일 존재는 Sandbox/추론 성공 증거가 아니다.

## 최소 권한

새 격리 루트에만 보호된 DACL을 설정했다. 상속된 Authenticated Users/일반 사용자 그룹 Modify 권한은 새 루트에 전달하지 않았다.

- 현재 소유자 PAUL-PC\windows, SYSTEM, Administrators: FullControl.
- PAUL-PC\LIMITLESSRelaySvc: ReadAndExecute + 자식 파일/폴더 상속. 쓰기·수정 허용 없음.
- 기존 부모/운영 CLI 폴더·프로필·CODEX_HOME·Sandbox ACL은 변경0.

bin/codex.exe의 실제 상속 ACE에서도 같은4주체 확인. 이는 새 경로의 DACL 확인이며 서비스 토큰으로 부모 경로 전체를 통과해 실행했다는 판정은 아니다.

## 무모델 실행 결과와 서비스 HOLD

현재 계정 PAUL-PC\windows / Session8에서 격리 bin/codex.exe를 절대 경로로 실행했다.

- --version: 종료0, codex-cli 0.164.0-alpha.11.1.
- --help: 종료0, Usage 출력 확인.
- CODEX_HOME 환경값 변경0, 인증파일 읽기·복사 명령0, login/usage/exec/Worker/Sandbox setup 실행0.
- 실행 내용은 버전/도움말로 제한. 원문 stderr나 환경 덤프를 기록하지 않았다.

**서비스 계정 실행은 NOT_VERIFIED/HOLD**다. 현재 계정이 비관리자이며 이전 확인대로 기존 작업 접근이 거부된다. 기존 비대화형 배치 등록 경로는 관리자 문맥과 서비스 암호의 로컬 보안 입력을 요구한다. 이를 보유하지 않아 새 서비스 실행을 만들지 않았다. 기존 task 재사용·변경, 서비스 프로세스 토큰 추출/도용, 인증정보 복사, 암호를 채팅으로 요청하는 행위를 하지 않았다.

남은 단계는 적법한 관리자 문맥에서 서비스 계정 자신의 자격 증명을 로컬 보안 입력으로 제공한 임시 단발 batch가 같은 격리 경로의 --version/--help만 실행하도록 하는 것이다. 계정/SID·Session0/logon4·경로/SHA·종료코드·CODEX_HOME 유지 여부를 기록하고 임시 작업을 제거한다. 두 기존 부팅 작업은 수정하지 않는다. 이 실행은 이번 승인 범위에 있으나 현재 도구의 권한/자격 증명 제약 때문에 완료하지 못했다. 반복 등록·자동 재시도는 하지 않았다.

과거 turn.failed 직접 원인은 계속 UNKNOWN이다. 일반 사용자에서 bootstrap이 성공했다고 서비스 추론 장애가 해결됐다고 보고하지 않는다.

## 운영 참조 변경 대상 — 후속 승인 전 적용 금지

`C:\ProgramData\LimitlessRelay\boot-approval.json`:

- service_codex.path: 현재 `F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\data\runtime\codex\c6144bc19b2705f2\codex.exe` → 후보는 위 격리 루트의 `bin\codex.exe`.
- service_codex.sha256: 현재 `c6144bc19b2705f25a40d8488b3d57d17a223c5df3c5f65e3c58d72601956c1e` → 후보 `4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73`.
- service_codex.version: 0.164.0-alpha.11.1 그대로.

전체 패키지45파일의 무결성 관리 방식도 전환 전 검토가 필요하다. 이번에는 전용 승인 목록을 새로 등록하거나 기존 runtime_sha256/사용자Worker 목록을 확장하지 않았다. service_ai.select_executable가 이 참조를 사용한다는 코드 근거는 이전 보고와 같다. 현재 참조는 기존 단독exe 그대로다.

## 복구 및 다음 실제 시험 조건

이번 격리 배치는 어떤 운영 참조에도 연결되지 않아 운영 복구 변경은 필요 없다. 격리 폴더를 남겨도 기존 운영은 기존 경로를 사용한다. 제거가 필요하면 기록된 정확한 새 루트만 확인하여 제거하고 부모·기존runtime·인증은 보존한다. 현재 제거하지 않았다.

향후 전환 시 기존 service_codex 세 필드와 승인 JSON SHA/ACL을 먼저 기록하고, 실패하면 그 변경 필드만 원래 값으로 복원한다. 기존 단독exe는 삭제하지 않는다. 전체 승인JSON 과거본 덮어쓰기를 하지 않는다. 새 루트 ACL 변경은 그 루트에 한정되어 기존 Sandbox 정책 복구는 필요 없다. Sandbox setup 자체는 실행하지 않았다.

실제 읽기 전용 추론1회 전에 서비스 --version/--help 확인을 먼저 완료해야 한다. 이후 대표의 **운영 참조 전환 승인**과 **실제 추론 승인**을 각각 받고, 새 서비스용 슬롯·90초·최대1회·재시도0·공식구독 인증/5시간·주간 usage·역할/계약/SHA·로그보존·기존JSONL/출력 검증·Discord 성공 조건을 확인한다. 기존 CONSUMED12와 사용자 승인대기1건은 재사용하지 않는다. PIN전 부팅 상태의 실제 추론 증명도 별도 필요하다. MANUAL 유지.

## 보호 확인

기존 승인Python42 SHA 불일치0. boot-approval SHA `0cd2ce55f7e23b44190e7f4786c2e9f68e2e291b1888e5768ea606f391c8d2d6` 동일. SQLite 읽기전용 조회 MANUAL·사용자Worker 승인대기1건. Bot/Art RUNNING heartbeat 갱신·Worker비활성/Art AI0. 운영CLI/봇/아트/부팅2개/SQLite/승인목록/인증·CODEX_HOME/게임/Unity/Windows로그인·PIN 변경0. A활성/추론/Push/merge/재부팅0. 문서·[격리 배치 근거](Evidence/ServiceCLI_IsolatedDeployment_20261024.json)만 Git에 기록하며 바이너리는 제외한다.
