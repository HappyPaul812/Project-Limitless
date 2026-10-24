# 운영 CLI 참조 전환 — 로컬 관리자 실행 대기

대표의 이번 승인은 운영 참조 두 값의 전환뿐이다. 실제 추론·A·서비스 재시작·게임 변경은 승인 대상이 아니다. 현재는 관리자 문맥과 로컬 서비스 암호가 없어 **PENDING_LOCAL_ADMIN_EXECUTION / 운영 참조 미변경**이다. 과거 서비스 bootstrap PASS를 이번 전환 후 재검증으로 대체하지 않는다.

## 사전 확인

- 현재 승인 JSON SHA: `0cd2ce55f7e23b44190e7f4786c2e9f68e2e291b1888e5768ea606f391c8d2d6`, 기존 참조 유지.
- 공식 격리 전체45개 SHA 일치·45개 서비스 ReadAndExecute ACE 확인. 본체/보조3개 서명은 실제 실행 직전과 직후 다시 검사한다.
- Bot/Art 상태 RUNNING·AI Worker 비활성·Art AI0 확인. 실제 AI0.
- 합성 파일 검증4 PASS: 두 필드 외 값 보존, 원자적 파일 교체, 의도적 사후 실패 후 원본 바이트 복원, ACL 유지. 최초 시험에서 PowerShell의 null 문자열 변환 문제를 발견해 NullString 인수로 수정하고 통과했다. 이 시험은 운영 파일을 쓰지 않았다.

## 정확한 변경 대상

파일 `C:\ProgramData\LimitlessRelay\boot-approval.json`의:

| 항목 | 현재 | 승인된 전환값 |
|---|---|---|
| service_codex.path | 아래 tool root의 data/runtime/codex/c6144bc19b2705f2/codex.exe | 같은 root의 data/runtime/codex/official-0.164.0-alpha.11.1-x64/bin/codex.exe |
| service_codex.sha256 | c6144bc19b2705f25a40d8488b3d57d17a223c5df3c5f65e3c58d72601956c1e | 4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73 |

tool root는 `F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab`이다. version=0.164.0-alpha.11.1 및 기존42 Python 승인·나머지 모든 JSON 값은 보존한다. 전체45개 허용 목록을 무조건 추가하지 않는다. source 패키지의 파일/권한도 바꾸지 않는다.

## 준비된 단발 전환기

`Tools/DiscordAIServiceDiagnostics/switch-service-codex-reference.ps1`:

1. 관리자 확인 후 기존 설정·CLI SHA·42 Python·공식45파일 및 RX·서명·기존 서비스 bootstrap 검증 스크립트 SHA를 확인한다. 예상과 다르면 전환 전 HOLD한다.
2. 같은 ProgramData 폴더의 `before-cli-reference-<nonce>.json`으로 정확한 원본 바이트를 백업하고 SHA/ACL을 보존한다.
3. path/sha256만 바꾼 문서를 같은 폴더의 임시 파일로 준비한 뒤 원자적으로 교체한다. 원본 백업은 유지한다. JSON 의미상 나머지 값과 ACL이 같은지 검사한다.
4. 기존 `verify-isolated-batch.ps1`를 별도 pwsh로 실행한다. 대표의 로컬 Get-Credential 입력으로 서비스 Password/Batch/Limited 임시 작업이 --version/--help만 실행한다. 인증 저장소·CODEX_HOME·기존 부팅2개·운영 큐를 바꾸지 않는다. 작업 종료0·Session0/logon4·새 CLI SHA·정리 REMOVED·부팅/승인 불변까지 확인한다.
5. 공식45파일·권한·서명과 참조를 사후 재검증한다. 정상일 때만 PASS·reference_switched=true를 기록한다.
6. 어느 전환 후 검사라도 실패하면 백업 원본 바이트와 ACL로 자동 복원하고 HOLD한다. 다른 주체가 설정을 동시에 변경한 경우는 그 설정을 덮어쓰지 않고 복원 미확인 HOLD를 보고한다. 남아 실행 중인 진단 작업도 강제 중지하지 않고 HOLD한다. 응용 재시도0.

암호는 기존 검증기의 로컬 보안 입력을 통해 Windows 작업 등록 API에만 메모리로 전달된다. 파일/로그/Discord/GitHub로 출력하지 않는다. 기존 프로필의 ChatGPT 인증을 다른 계정으로 복사하지 않는다.

## 대표의 로컬 실행

관리자 PowerShell에서 다음 한 줄을 실행하고, 요청받은 서비스 Windows 암호는 로컬 입력창에만 제공한다. 채팅으로 암호를 보내지 않는다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAIServiceDiagnostics\switch-service-codex-reference.ps1"
```

스크립트 정책이 차단하면 정책을 임의 완화하지 않고 중지한다. 현재 비관리자 실행 가드는 `HOLD_ADMIN_LOCAL_CREDENTIAL_REQUIRED_REFERENCE_UNCHANGED`로 변경 없이 종료했다. 별도 실행 승인을 다시 요구하는 것이 아니라 이번에 승인된 서비스 재확인에 필요한 관리자/로컬 보안 입력 단계다.

결과는 `Tools/DiscordAICollab/data/service-reference-switch/switch-<nonce>.json`에 남는다. 내부 서비스 재확인 결과는 기존 `data/service-isolated-batch-check/run-<nonce>.json`에 기록한다. 결과 파일의 status/rollback/reference_switched를 모두 확인해야 한다. 현재 새 운영 SHA·백업 경로·서비스 사후 결과는 아직 없으며 성공했다고 선언하지 않는다.

## 다음 단계

전환 PASS 후에도 실제 읽기 전용 추론1회는 **다음 별도 승인**이다. 새 서비스 전용 슬롯·90초/최대1회·응용 재시도0·공식 구독 인증/두 사용량 창·계약/역할 정본 SHA·로그 보존 사전검사·기존 출력/JSONL 검증 조건이 필요하다. 기존CONSUMED12와 사용자 Worker 승인 대기 슬롯 재사용 금지. MANUAL 유지. 이전 turn.failed 원인은 여전히 UNKNOWN이다.
