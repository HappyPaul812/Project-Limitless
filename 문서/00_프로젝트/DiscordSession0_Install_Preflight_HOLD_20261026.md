# Issue #66 별도 계정·FAKE_ONLY 설치 사전검사 — HOLD

**HOLD_BEFORE_INSTALL / TASK_DACL_AT_REGISTRATION_NOT_VERIFIED.** 대표의 설치·fake IPC 승인은 확인했지만, 사전검사의 작업 권한 경계가 충족되지 않아 설치 전에 중지했습니다. 실제 권한 유출이나 공격을 재현한 것은 아닙니다.

기준 branch `codex/discord-usage-diagnostics-20261026`, commit `a8b0be3971aa461d09432348b309179cade27981`. 완료된29 PASS는 재실행하지 않았습니다. 승인 후보·Manifest·Installer·855파일은 변경하지 않았습니다.

## 확인 결과

| 항목 | 결과 |
|---|---|
| Manifest SHA | PASS: `9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec` |
| Installer SHA | PASS: `337ad613ef6471ae6f4058043965bb3b812a0e6b31adc05ce1efb61ce0158829` |
| 정확한 파일 집합·SHA | PASS:855개, 불일치0·추가 실행파일0 |
| 파일 범위 | Code66 / Runtime741 / Legacy47 / Deployment1; Manifest는 별도 metadata1 |
| 초기 계약 | FAKE_ONLY / auto_start=false |
| 신규 설치·상태 폴더 | 조사 시 존재하지 않음 |
| 작업 등록부터 DACL 제한 | **미검증/HOLD**: 등록 후 별도 SetSecurityDescriptor |
| 작업 DACL 실제 readback | **없음** |
| 기존 OnBoot2개 정의 조회 | 현재 비관리자 Session9에서 실패. 수정/실행하지 않음 |
| 실제 계정 SID·Session0 IPC | 미실행·미검증 |

새 계정0·작업0·서비스0·권한 변경0·Host 시작0·FakeWorker0·실제 모델0·official usage0·Discord0·암호 입력0·토큰/인증 복사0입니다. 운영 되돌리기 작업은 필요하지 않습니다. 신규 BOT SID는 발급하지 않았고, 기존 Worker를 새로 실행하지 않았습니다. 조사 토큰은 Session9/비관리자이며 대표의 실제 관리자 토큰을 대신하지 않습니다.

## 중단 근거

승인된 Installer의 **112행**은 `Register-ScheduledTask`, **114행**은 생성된 작업의 `SetSecurityDescriptor(...,0)`입니다. 등록 시점에 원하는 보안 descriptor를 전달하지 않으며, 최종 descriptor를 다시 읽어 검사하는 코드도 없습니다.

Microsoft는 기본적으로 작업 생성자에게 조회·변경·삭제·실행 권한이 있고, 등록 시 security descriptor를 지정할 수 있다고 설명합니다. 따라서 이 두 호출 사이에 원하는 제한이 적용된다고 가정하지 않습니다. Disabled는 실행 상태이며 DACL 생성 시점 검증의 대체가 아닙니다. 실제 Windows에 이 작업을 생성하지 않았으므로 해당 PC의 중간 DACL이나 악용 가능성을 확정하지 않습니다.

앞선29 PASS는 Host/Worker 접합·가짜 Job/비관리자 게시 거부 등이었으며, 이 작업 등록 권한의 원자성/실제 readback까지 확인하지 못했습니다. 이전 보고의 설치 명령은 **현재 실행하면 안 됩니다**.

대표 지시의 “하나라도 검증에 실패하면 중단하고 보안 정책을 완화하지 마세요”를 적용했습니다. 기존 승인 SHA를 새 값으로 바꾸거나 등록 함수를 가로채는 방식으로 우회하지 않았습니다. 새 `run-approved-trial-install.ps1`은 읽기 전용 Preflight만 제공하며 설치 기능이 없습니다.

## 다음 최소 수정안 — 제안만, 적용·설치하지 않음

1. Task Scheduler COM `RegisterTaskDefinition`에 완성된 task definition과 SDDL을 **같은 등록 호출**로 전달합니다. 새 작업 생성 `TASK_CREATE(0x2)`, 자동 principal ACE 추가 금지 `0x10`, registration trigger 무시 `0x20`를 사용하고 update/create-or-update는 사용하지 않습니다. definition 자체도 Disabled/trigger0/Password/Limited/IgnoreNew/restart0/15분을 유지합니다.
2. owner BA, SYSTEM/Administrators full control, Worker read-only를 명시합니다. 등록 결과의 `GetSecurityDescriptor(7)`로 owner/group/DACL을 즉시 조회하고, 사용자 SID write/execute/owner 권한 및 예상 밖 principal ACE가 없음을 검사합니다. SACL 권한을 요구하거나 새로운 권한을 부여하는 방식으로 우회하지 않습니다.
3. 같은 호출의 새 반환 객체만 실패 시 정리합니다. 기존 작업을 갱신·덮어쓰기하지 않습니다. 암호는 대표의 로컬 보안 입력만 사용합니다.
4. 가짜 Scheduler로 등록 인자·SDDL 동시 전달, 중복 이름 거부, readback 불일치 HOLD, unknown result 재시도0을 검증합니다. 기존29 전체는 반복하지 않습니다.
5. 별도 수정 후보로 만들고 새 Installer/Manifest SHA와 변경 범위를 확인한 다음, 그 값에 대한 승인 아래 실제 설치를 재개해야 합니다. 현재 승인 후보는 역사로 보존합니다.

현재는 설치가 차단되어 있어 대표에게 암호 입력이나 관리자 설치 명령 실행을 요청하지 않습니다. 보안 조건을 만족하지 못한 기존 명령을 그대로 제공하지 않습니다. 원본 계정/CLI/CODEX_HOME/인증/부팅2/운영 SQLite/봇·아트/Unity·게임·Save/Push·main/PC는 변경하지 않았습니다. MANUAL/A 미활성 유지, 운영 건강이나 부팅 정의 SHA가 새로 검증됐다는 의미는 아닙니다.

## 증거·인계

`팀운영/Evidence/Session0InstallPreflightHOLD20261026/`에 파일 검사 결과·행 번호·읽기 전용 보안 gate 결과를 보존합니다. CURRENT_STATUS 및 CODEX_LATEST를 최신 HOLD로 갱신합니다. 실제 로컬 commit과 Issue 링크는 마감 보고에 기록합니다.

공식 근거: [작업 생성자의 기본 권한](https://learn.microsoft.com/en-us/windows/win32/taskschd/security-contexts-for-running-tasks), [등록 시 SDDL·생성 flags](https://learn.microsoft.com/en-us/windows/win32/taskschd/taskfolder-registertaskdefinition), [SetSecurityDescriptor flags](https://learn.microsoft.com/en-us/windows/win32/taskschd/registeredtask-setsecuritydescriptor).
