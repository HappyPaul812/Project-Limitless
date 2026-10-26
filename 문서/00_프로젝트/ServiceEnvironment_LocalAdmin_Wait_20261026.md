# Issue66 서비스 실행 환경 확인 — 관리자 직접 실행 대기 (2026-10-26)

**WAITING_FOR_OWNER_ADMIN_EXECUTION / USAGE_NOT_ATTEMPTED / MANUAL**. 바울 대표가 요청한 기존 성공 절차 조사와 전용 명령 준비를 완료했습니다. 관리자 PowerShell 및 서비스 Session0는 별개입니다. 현재 Codex의 비관리자 토큰으로 서비스 작업을 대신 등록하지 않았습니다.

## 재사용한 실제 PASS 절차

기존 `ServiceCLI_ServiceBatch_PASS_20261024.json`의 request `3483af069a9c4f1387e2fec54f15dd65`는 서비스 계정·Session0·배치로그온4·task종료0·임시작업제거·기존부팅/승인불변 PASS입니다. 기존 `verify-isolated-batch.ps1` 및 `batch-registration-check.ps1`의 **관리자 parent → 로컬 Get-Credential → Password/Limited 임시작업 → 자식 토큰/LSA 확인 → 일치하는 임시작업만 제거** 패턴을 재사용했습니다. 과거 CLI45파일·version/help 시험 자체를 다시 실행하지 않습니다.

기존 `BatchReadOnlyMetadata.psm1` SHA `09458bfc9a2b5516784efa7e9bda7013506dc0c21673b8aad7aec84240b60faf`의 CurrentLogon()을 그대로 사용합니다. 계정 권한 추가·토큰 복제·암호 추출 기능을 새로 만들지 않았습니다.

## 새 환경 확인 스크립트

`Tools/DiscordAIServiceDiagnostics/restore-service-environment.ps1`

SHA256: `f0b3589cadf63a2c47d363b614453ecf4a72bc565769436ac8ebb9f5b4eca7a0`

- 비관리자는 **HOLD_ADMIN_POWERSHELL_REQUIRED_NO_CHANGES_NO_USAGE**, exit2로 즉시 종료합니다.
- 관리자 parent가 기존 OnBoot2개 XML SHA·boot승인 SHA와 readonly 장부(tasks/approvals/operation_mode) digest/MANUAL을 먼저 확인합니다. 기존 작업/DB/승인을 수정하지 않습니다.
- `ProgramData/LimitlessRelay/UsageEnvironmentChecks/<새nonce>/`에 환경 확인 스크립트·기존 메타데이터 모듈만 복사하고, 신규 Output 폴더에만 서비스 Modify와 관리자/소유자/SYSTEM FullControl을 적용합니다. 기존 프로필·계정·디렉터리 ACL은 변경하지 않습니다. 실제 사용량 trial 패키지/permit 배포가 아닙니다.
- 서비스 계정 Windows 암호는 **바울 대표가 연 로컬 Get-Credential 입력**에서만 받아 Windows 작업 등록 API의 메모리 인수로만 사용합니다. 파일/콘솔/ChatGPT/Discord로 출력·복사하지 않습니다. 기존 암호/토큰/인증 캐시를 읽어내지 않습니다. 비밀번호를 입력받는 로컬 화면은 대표의 직접 실행 절차이며 Codex의 화면 조작이 아닙니다.
- 새 임시 `LIMITLESS-UsageEnvironment-<nonce>` 한 개만 Password/Limited·IgnoreNew·재시작0·ExecutionTimeLimit20초·AllowHardTerminate 설정을 확인하고 실행합니다. 관리자 parent가 Session0라고 가정하지 않습니다.
- 자식에서 실제 SID/계정·비관리자·Session0/logon4·비대화형을 확인하고, 기존 프로필/CODEX_HOME을 변경하지 않은 채 CLI SHA 읽기와 보호된 Output 쓰기/읽기를 확인합니다. CLI 실행/--version/--help/인증/사용량/모델/Discord/SQLite 업무 처리는0입니다.
- 자식의 원문 SID는 보호된 child-private.json에만 남깁니다. 공용 summary에는 승인 SID 일치 여부만 남깁니다. 오류는 고정 단계/안전한 코드·예외 타입/HResult/native/CIM코드만 기록하며 원시 예외 메시지·인증정보는 출력하지 않습니다.
- 완료 후 nonce/Description/명령이 맞는 해당 임시작업만 제거합니다. 20초 제한 뒤에도 해당 작업이 실행 중이면 그 진단 작업만 정지·종료 확인 후 제거하며 상태는 HOLD입니다. 기존 OnBoot/봇/아트 프로세스를 정지하지 않습니다. task/정리/보호 중 하나라도 미확인이면 HOLD합니다.
- 전후 OnBoot/승인·장부/MANUAL 동일성을 검사하고, 새 사용량 permit을 만들지 않습니다. 진단 증거/nonce/보호경로 probe는 보존합니다.

## 바울 대표의 정확한 로컬 명령

**관리자 PowerShell**을 직접 여신 뒤 실행합니다. 서비스 계정 암호는 뜨는 로컬 보안 입력에만 입력하고 채팅으로 보내지 않습니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\restore-service-environment.ps1"
```

ExecutionPolicy를 Bypass하거나 장치 정책을 낮추지 않습니다. 관리자 실행이 불가능하거나 정책/권한이 차단하면 그대로 HOLD하고 정제 결과만 확인합니다. 이 명령은 **환경 확인 전용**이며 기존 승인된 사용량1회를 소비하지 않습니다.

결과: `C:\ProgramData\LimitlessRelay\UsageEnvironmentChecks\<nonce>\Output\summary.json`. 원문 SID가 있는 child-private.json 대신 summary 또는 콘솔의 정제 결과만 전달합니다. 대표가 실행 완료를 알리면 Codex가 결과를 확인합니다.

## 이번 준비 단계의 검증과 한계

- 새 스크립트 PowerShell parser0오류, 현재 비관리자 가드 exit2/변경0/호출0 확인.
- 새 readonly Ledger-Snapshot 도우미의 인수·실행·JSON·MANUAL 확인 PASS. parent 보호 스냅샷 읽기만 했으며 업무 처리·운영 DB쓰기0입니다.
- 정적 명령 범위에서 Codex/사용량/모델/서비스 재시작/종료 명령 없음 확인. 원시 오류 메시지를 내보내지 않는 경로 확인.
- 기존 SHA44·22+12검증 재실행0. 기존 패키지/진입점5파일/테스트는 수정하지 않았습니다. 새 스크립트가 실제 관리자/서비스에서 성공했다는 결과는 **아직 없습니다**.
- 현재 ProgramData 환경 확인 폴더/임시작업/사용량permit 미생성, 서비스시험0·실조회0·AI0·Discord0. 현재 bot/art/OnBoot 실제상태는 관리자 실행 전 재검증하지 않았습니다. 기존파일·ACL·인증·CLI/CODEX_HOME·게임/Save·Push/main병합/재부팅 변경0.

## 이어서 진행할 조건

summary가 ENVIRONMENT_PASS_USAGE_NOT_ATTEMPTED이고 task종료0·Session0/logon4·SID일치·CLI/CODEX_HOME·보호경로·20초설정·임시작업제거·기존부팅/승인·장부 보존이 모두 확인되어야 다음 단계로 갑니다. 현재 명령 자체에는 사용량 실행 경로가 없습니다.

그 결과를 받은 뒤 기존 승인 범위로 격리 trial 패키지·별도 permit/새request·실제 usage-only **최대1회/전체20초/재시도0** 절차를 이어서 준비·실행합니다. 실제 사용량 task/supervisor의 프로세스 정리 범위까지 확인되지 않으면 조회 전에 HOLD합니다. 성공 이후도 모델/A는 별도 승인입니다. 관리자/서비스 실행 결과를 기다리는 동안 자동 재실행·시간예약·상태변경을 하지 않습니다.

브랜치 codex/discord-usage-diagnostics-20261026. 선행84486500·패키지기준0f98620b 보존. 최종 로컬 commit은Issue66/최종보고 참조. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / OWNER_LOCAL_ACTION_PENDING**.
