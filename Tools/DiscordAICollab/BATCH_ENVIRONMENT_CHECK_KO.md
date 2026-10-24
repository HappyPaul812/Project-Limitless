# 1번 배치 등록·환경 검증 결과 — 2026-10-24 KST

이번 실행은 Windows 등록 인증 오류로 중단했습니다. 실제 비대화형 실행은 검증하지 못했으며 2번 AI 추론·3번 A 활성화는 수행하지 않았습니다.

## 확인한 정확한 오류

발생 지점은 Register-ScheduledTask의 서비스 계정 자격 증명 검증 단계입니다.

- 외부 CimException/.NET HRESULT: -2146233088 (0x80131500)
- ErrorRecord.FullyQualifiedErrorId 및 CimException.ErrorData.MessageID: HRESULT 0x8007052e
- Windows 오류: 1326 / ERROR_LOGON_FAILURE
- Category: AuthenticationError
- 내부 메시지: 사용자 이름 또는 암호가 올바르지 않습니다.

[Microsoft 오류 코드 문서](https://learn.microsoft.com/en-us/windows/win32/debug/system-error-codes--1300-1699-)의 ERROR_LOGON_FAILURE와 일치합니다. 계정이 활성이고 SID가 기존 설정과 일치하며 배치 권한이 있어, 권한 부족/실행 파일 경로/XML 형식을 수정할 근거는 확인되지 않았습니다. **이번 등록에 전달된 자격 증명을 Windows 작업 스케줄러가 거부했다는 계층은 확정**입니다. 실제 암호 입력 오류인지 그 밖의 Windows 인증 조건인지는 비밀을 검사하지 않았으므로 단정하지 않습니다.

## 확인 범위와 보존

- 대상: PAUL-PC\LIMITLESSRelaySvc. 활성·일반계정·기존 SID 일치.
- SeBatchLogonRight 직접 보유, 직접 SeDenyBatchLogonRight 없음. Everyone/Authenticated Users/Users의 배치 거부 없음.
- 실행기: C:\Program Files\PowerShell\7\pwsh.exe, 기존 worktree가 WorkingDirectory.
- 임시 작업은 -NoProfile/-NonInteractive/-WindowStyle Hidden과 안전한 batch-account-check.ps1만 참조.
- child 명령은 실행 SID·로그온 유형·세션·허용 환경·종료 코드만 기록. Codex/AI/Discord/외부 네트워크는 호출하지 않음.
- 등록 1회 시도 후 실패하여 자식 프로세스는 시작하지 않음. 실제 실행 계정·Batch 로그온 유형4·종료 코드0은 **NOT_RUN**.
- 임시 작업은 등록되지 않아 제거할 대상 없음(NO_TEST_TASK_REGISTERED).
- LIMITLESS-Bot-OnBoot / LIMITLESS-Art-OnBoot XML SHA 전후 동일, Password/Limited·실행 경로·enabled 보존.
- 배치 권한 전후 동일. Windows 암호·Bot Token·Codex 인증·계정 권한·PIN·부팅 작업 변경 없음.
- 현재 MANUAL, Bot/Art heartbeat 유지, 기존 SQLite 업무/usage/승인/체크포인트 동일.
- 재등록/암호 재설정/권한 확대/다른 인증 방식 우회·자동 재시도 없음.

## 최소 수정

기존 probe-service-ai.ps1은 오류 메시지 안의 0x 패턴만 찾아 실제 HRESULT를 놓치고 있었습니다. 현지화된 메시지에 숫자가 없더라도 ErrorId/CIM MessageID에서 원래 Windows HRESULT를 추출하고 1326을 WINDOWS_REGISTRATION_CREDENTIAL_REJECTED로 표시하도록 보완했습니다. 인증이나 실행 승인 로직은 변경하지 않았습니다.

새 batch-registration-check.ps1은 이 범위만 위한 단발 진단/안전 환경 검증 도구입니다. 현재 실행은 실패했으므로 다시 실행하지 않습니다. 다음에 대표가 명시적으로 1번을 재개할 때만 같은 안전 검증을 사용할 수 있습니다. 기존 계정 암호를 로컬에서 확인해야 하며 대표 PIN/ChatGPT 암호를 대신 입력하지 않습니다. 이 작업에서 암호를 재설정하면 정상 부팅 작업의 저장된 자격 증명과 인증 보호에 영향을 줄 수 있으므로 임의 변경하지 않습니다.

도구는 원문 Windows 오류에서 암호의 원형/XML/URI 표현을 제거한 뒤 원인 계층만 저장하며, 입력 암호·전체 바인딩 인수·Codex 인증 저장소를 기록하지 않습니다. C:에 별도 SAFE_BATCH_ENVIRONMENT_ONLY 승인 및 3개 파일 SHA를 두어 기존 부팅 승인과 실행 경로를 분리했습니다.

## 결과

| 항목 | 결과 |
| --- | --- |
| 실제 등록 | FAIL — 0x8007052e / 1326 |
| 실제 배치 실행 | NOT_RUN |
| 실제 실행 계정 | 자식 시작 전 차단, 의도 계정만 확인 |
| 내부 오류·현지화 메시지 회귀 | PASS |
| 암호/XML/URI 제거 fixture | PASS |
| 스크립트4 구문 | PASS |
| 본인 로그온 메타데이터 조회 | 제한 밖 읽기 PASS, 실제 서비스 배치 증거 아님 |
| 원본 작업2/계정 권한/SQLite/봇·아트 보존 | PASS |
| 2번/3번 진행 | 금지 유지 |

[진단 원본(비밀 제거)](../../문서/00_프로젝트/Evidence/BatchEnvironment20261024/diagnose.json) · [요약](../../문서/00_프로젝트/Evidence/BatchEnvironment20261024/summary.json)

다음은 로컬 서비스 계정 자격 증명을 확인한 뒤 별도 재개 지시로 1번의 안전한 등록·실행 검증부터 다시 확인하는 것입니다. 아직 2번이나 3번으로 진행할 수 없습니다.

## 2026-10-24 최소 복구 재개 — 로컬 입력 대기

대표의 재검증 지시로 1번만 재개합니다. 계정 활성·SID 일치를 다시 확인했습니다. 기존 Password/Limited 등록 방식은 유지합니다. 등록 직전 공식 Windows LogonUserW(LOGON32_LOGON_BATCH, 기본 provider)로 보안 입력 암호와 예상 SID를 1회 검증합니다. 토큰은 실행/가장에 사용하지 않고 닫으며 네이티브 암호 버퍼를 즉시 지웁니다. 실패 시 등록 없이 중단하고 숫자 오류만 기록합니다. 성공 시 기존 안전 환경 작업 1건만 등록·실행·정리합니다. 암호는 CLI 인수/파일/로그에 저장하지 않습니다. 작업 스케줄러에 전달하는 암호 문자열은 등록용 메모리에만 일시 존재합니다.

[Microsoft LogonUserW](https://learn.microsoft.com/windows/win32/api/winbase/nf-winbase-logonuserw). 구문·네이티브 헬퍼 컴파일 PASS, 실제 자격 증명 시도0. 현 단계는 WAITING_FOR_LOCAL_CREDENTIAL이며 새 인증/배치 성공으로 간주하지 않습니다. 기존 부팅 작업 및 Codex 인증은 변경하지 않았습니다.
