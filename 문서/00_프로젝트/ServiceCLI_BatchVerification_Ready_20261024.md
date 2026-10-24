# 서비스 격리 CLI 배치 검증 — 로컬 보안 입력 대기

2026-10-24. 선행1742abe53b8644a93eb2e1d8e5fd75e29164c029. 실제 AI0, 서비스 실행 아직 미검증.

## 확인한 기존 절차

기존 batch-registration-check.ps1는 관리자 PowerShell → 서비스 계정 Get-Credential 로컬 입력 → 임시 Password/Batch 작업(Limited) 1회 → 결과/종료코드 확인 → 임시 작업 제거와 기존2개 작업 XML 대조 순서였다. 암호는 메모리에서 Windows 작업 등록 API에만 전달하며 디스크·채팅·Discord·GitHub에 기록하지 않는다. 현재 계정은 비관리자이므로 서비스 자격 증명 없이 대신 실행하지 않는다.

## 준비한 실행물

`Tools/DiscordAIServiceDiagnostics/verify-isolated-batch.ps1`

- collab 밖 독립 진단 경로. 기존42 Python·부팅 스캔·SHA 승인 목록 변경0.
- 기존45파일 근거 manifest SHA와 BatchReadOnlyMetadata.psm1 SHA를 고정 검증하고 45개 파일 전체 SHA 및 본체/보조3개 서명을 검사한다.
- 기존 격리 `data/runtime/codex/official-0.164.0-alpha.11.1-x64/bin/codex.exe`만 `--version`, `--help` 인수로 실행. 임의 인수·모델/로그인/AppServer/Worker/setup 호출 없음.
- 새 임시 작업 LIMITLESS-IsolatedCLI-<nonce>, Limited/Password/자동재시도0. child에서 실제SID·Session0/logon4·비대화형·프로필·CODEX_HOME 사전값 유지 검증. CODEX_HOME을 설정하지 않는다.
- 기존2개 OnBoot XML SHA와 boot-approval 파일 SHA를 실행 전후 대조한다. 운영 SQLite는 열지 않는다. 이전 승인 슬롯 접근0.
- 새 `data/service-isolated-batch-check` 출력 폴더에만 서비스 Modify, 관리자/현재관리자/SYSTEM FullControl을 적용한다. 암호·인증·CLI 원문 출력은 보존하지 않고 계정/세션·고정 검증값·종료코드만 기록한다.
- nonce/설명/명령이 맞는 해당 임시 작업만 제거한다. 제한시간까지 실행 중이면 강제 중지하지 않고 HOLD와 잔존 여부를 보고한다. 자동 재등록/재시도 없음.

PowerShell parser 0오류, 비관리자 실행 가드 `HOLD_ADMIN_POWERSHELL_REQUIRED_NO_CHANGES` 확인. 관리자·서비스 실제 경로는 아직 실행하지 않았다. 실제 AI0. 과거 추론 실패 원인은 UNKNOWN 유지.

## 대표의 로컬 실행

관리자 PowerShell에서 다음 한 줄을 실행한다. Get-Credential에 **Windows 서비스 계정 암호**를 입력하며 ChatGPT 인증·PIN·암호를 채팅으로 보내지 않는다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAIServiceDiagnostics\verify-isolated-batch.ps1"
```

PowerShell 보안 정책이 차단하면 정책을 임의 완화하지 않고 중지한다. 스크립트 실행 허용 여부는 해당 장치 정책을 따른다. 기존 프로필/부팅 프로그램/PIN 설정을 바꾸지 않는다. 이번 로컬 보안 입력은 초기 진단용이며 운영 시 매번 로그인하는 구조를 최종안으로 채택한 것이 아니다.

결과 위치: `Tools/DiscordAICollab/data/service-isolated-batch-check/run-<nonce>.json`, `child-<nonce>.json`. 실행 후 이 파일을 읽어 PASS/HOLD를 확인한다. 비밀번호·인증 원문을 전달할 필요가 없다. PASS 조건은 child 계정/SID·Session0·logon4, 45파일/서명, version/help 종료0, 버전일치, CODEX_HOME유지, task종료0, 임시작업제거, 기존부팅2개/승인목록불변이다. 스크립트 최종 status가 PASS라도 child 및 cleanup/protection 필드를 함께 검토한다.

## 성공 이후 별도 승인 대상

이번 실행은 운영 CLI 참조를 바꾸지 않는다. 후속 승인 시에만 `C:\ProgramData\LimitlessRelay\boot-approval.json`의 service_codex.path를 기존 c6144bc19b2705f2/codex.exe에서 official-0.164.0-alpha.11.1-x64/bin/codex.exe로, service_codex.sha256을 c6144bc19b2705f25a40d8488b3d57d17a223c5df3c5f65e3c58d72601956c1e에서 4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73로 변경한다. version은 동일하다. 전체 패키지 무결성 관리 방식도 별도 검토한다.

실패 복원은 기록한 기존 service_codex 필드만 복원하며 기존exe·인증은 삭제하지 않는다. 현재는 참조변경0이므로 운영 복원 조작이 필요 없다. 실제 추론은 별도 승인·새 서비스 전용 슬롯·90초/1회/재시도0·공식인증/usage·로그보존/기존검증기 조건을 충족한 후에만 실행한다. 사용자Worker/CONSUMED12슬롯/SQLite/MANUAL/봇/아트는 이번에 변경하지 않았다.
