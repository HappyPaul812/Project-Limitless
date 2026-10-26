# Issue #66 예약 작업 보안 설치기 원자 등록 후보

2026-10-26. 구현 commit `2ef4012fb9a963a80de5a9910b8853917fd870fd`, 기준 commit `5d259e2738768dd0816540b9a31228049db70a56`, branch `codex/discord-usage-diagnostics-20261026`. **OFFLINE_CANDIDATE_PASS / NOT_INSTALLED / 실제 설치 승인 대기**.

이번 변경은 예약 작업 생성 시점 SDDL 전달과 실제 descriptor 대조만 다룹니다. 기존29 PASS는 반복하지 않았습니다. 기존 Ready 후보 및 승인 SHA를 변경하지 않았고 기존 설치 명령도 실행하지 않았습니다. 실제 Windows Scheduler/계정/권한 검증 PASS를 뜻하지 않습니다.

## 구현

`Tools/DiscordAIServiceDiagnostics/install-session0-trial.ps1`에서 COM NewTask로 기존 설정(Password/Limited/Disabled/trigger0/IgnoreNew/restart0/15분)을 유지합니다. 15분은 한시 Host 수명이며 개별 Worker의 기존90초 제한과 다릅니다.

`RegisterTaskDefinition(name, definition, 50, fixedServiceUser, password, 1, sddl)` **한 번**으로 생성합니다. 50=CREATE(2)+DONT_ADD_PRINCIPAL_ACE(16)+IGNORE_REGISTRATION_TRIGGERS(32). UPDATE/CREATE_OR_UPDATE/Force/사후 SetSecurityDescriptor를 사용하지 않습니다. 기존 이름이 생기는 경쟁 상황에서도 CREATE가 기존 작업을 덮어쓰지 못하게 합니다. 고정 서비스 계정만 받으며 등록 credentials가 Principal보다 우선하는 공식 동작을 고려합니다.

SDDL:

```text
O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)(A;;GR;;;<실제 Worker SID>)
```

owner=Administrators, group=SYSTEM, 상속 차단 DACL. 명시적 Allow ACE 정확3개: SYSTEM/Administrators Full, Worker GenericRead만. Bot/일반 Users/현재 관리자 개인 SID 등의 추가 ACE, deny/inherited/callback/중복 ACE, owner/group/DACL 누락·변조를 거부합니다. Worker가 SYSTEM/Administrators SID인 경우도 거부합니다.

반환 객체에서 즉시 `GetSecurityDescriptor(7)`(owner1+group2+DACL4)을 호출해 .NET RawSecurityDescriptor로 검사합니다. 문자열 순서가 아니라 SID/ACE/mask를 검사합니다. 예상 경로·Disabled·trigger0도 확인한 뒤에만 DeploymentRecord를 씁니다. Verify 및 StartFake 직전에도 같은 SD 검사를 합니다. SACL 조회 권한을 요구하거나 권한을 사후 수정하지 않습니다.

재조회 mask는 SYSTEM/BA `0x001f01ff`, Worker `0x80000000`만 허용합니다. 실제 Scheduler가 GenericRead를 다른 mask로 정규화하여 반환할 가능성은 아직 실제 Windows에서 확인하지 않았습니다. 그런 값도 자동 수용하지 않고 **HOLD**합니다. 알 수 없는 권한 매핑을 지원한다고 주장하지 않습니다.

한 번 등록을 시도하면 예외·null·조회 실패·불일치 모두 같은 실행에서 재등록 불가입니다. 기존 이름 충돌/응답 불명에 자동 삭제·권한 복구·등록 재시도를 하지 않습니다. 성공 기록이 없으면 기존 StartFake 게이트를 통과할 수 없습니다. 등록 정의는 처음부터 Disabled/trigger0입니다. 등록 오류 시 신규 계정만 비활성화하는 기존 catch를 유지합니다. 오류가 났을 때 실제 생성 여부까지 확인됐다고 간주하지 않습니다.

## 오프라인 검증

신규 **35 PASS / 0 FAIL**, `Evidence/Session0TaskSecurityAtomic20261026/Tests.txt`.

- AST로 순수 함수만 추출해 가짜 Scheduler를 주입. 실제 COM 연결 및 설치기 본문 실행0.
- Disabled/trigger0/Password/Limited/IgnoreNew/restart0/수명 유지, SDDL과 CREATE-only flags 동시 전달, 즉시 readback.
- owner/group/보호 DACL/ACE 변조16종 거부, 동등한 ACE 순서 변경 허용.
- 이름 충돌·등록 실패·생성 후 응답 불명·null·readback 실패·잘못된 반환 경로·Enabled 반환 시 HOLD. 각각 등록 시도1/자동 재시도0.
- 성공 후 중복 호출 및 정의 Enabled/trigger/logon/principal/restart/runlevel 변조 차단.
- 내장 SID의 Worker 사용, 기존 등록 함수/사후 보안 수정 사용 차단.

초기 검사 1건은 정적 검사 regex가 `Unregister-ScheduledTask`를 등록 함수로 잘못 인식한 테스트 문제였습니다(34 PASS/1 FAIL). 단어 경계로 수정한 뒤 최종35 PASS/0 FAIL입니다. 설치 보안 로직을 완화하여 통과시키지 않았습니다. 기존29/모델/usage/IPC 성공 시험 재실행0.

## 별도 후보와 SHA256

후보: `F:\study\codex\Project-Limitless\Temp\Session0TaskSecurityAtomicCandidate20261026`

압축: 동일 경로 이름 뒤 `.zip`. 새 후보 생성 도구는 원본 SHA와855개 실제 bytes를 확인하고 새 경로에서만 생성합니다. 이미 후보/zip이 있으면 덮어쓰지 않습니다. zip의 모든 파일 bytes도 다시 대조했습니다.

| 항목 | 이전 SHA256 | 새 SHA256 |
|---|---|---|
| 설치기 (source 및 후보 Deployment 동일) | `337ad613ef6471ae6f4058043965bb3b812a0e6b31adc05ce1efb61ce0158829` | `117065bbfca686474f9104bc0dff20fa6528dfb9c204e5a2f2720d91994958e9` |
| HostManifest.json | `9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec` | `6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab` |
| 새 후보 zip | 없음 | `4b2c56edf5bb8edc6ee7a40a10504f537223bc77874b43a36e61be735c318f83` |
| test-task-security-offline.ps1 | 없음 | `615fe94d6866c3c91a7e2ebb23871b382d8e5c15738f66df555b0a0cc40edd94` |
| prepare-task-security-candidate.py | 없음 | `18f6ed49cceda60868bc61eff4df7a109852b152c6842334f065598007317ab0` |

정확855개: Code66/Runtime741/Legacy47/Deployment1. 변경 `Deployment/install-session0-trial.ps1` **1개**, 동일854, 추가0/삭제0. metadata HostManifest는 별도1이며 의미 차이는 해당 파일 hash 항목 하나뿐입니다. 테스트/생성 도구는 설치 payload에 추가하지 않았습니다. 원본 Ready855개+Manifest는 생성 전후 전체 SHA 일치. 기존 읽기 전용 run-approved-trial-install.ps1 SHA `42befb2a631ce9f61489f15586b22ead13cce899dffac8dbe4d106d8598fb8b2`도 보존하고 이전 후보의 HOLD를 해제하지 않았습니다.

## 다음 관리자 설치 절차 — 이번에는 실행하지 않음

1. 대표가 **새 설치기/Manifest/zip의 위 SHA 및 새 후보**에 대한 설치를 별도 승인해야 합니다. 기존 Ready 후보 설치 승인을 새 SHA로 자동 확대하지 않습니다. 현재 관리자 실행 명령이나 암호 입력을 요청하지 않습니다.
2. 승인 후 관리자 PowerShell에서 새 후보 SHA·855개 범위·코드 보호 경로·기존 boot 승인 SHA·신규 계정/작업/설치 경로 부재를 다시 확인합니다. 기존 운영 계정/OnBoot2개는 변경하지 않습니다.
3. hash 확인한 설치기 bytes만 사용해 Action Install/새 Bundle/새 BundleManifestSha/명시적 ConfirmSystemChanges를 지정합니다. 신규 Bot 암호와 기존 서비스 계정 암호는 대표의 로컬 SecureString/Credential 입력으로만 받습니다. 토큰·인증파일 이동0.
4. 등록 readback 실패·불명 시 즉시 HOLD, 재등록0. 성공 시 별도 Verify로 실제 owner/group/DACL과 코드/상태 ACL, 신규 SID, Disabled/trigger0를 확인합니다. 성공 전 Host 시작 금지입니다.
5. 실제 계정 간 FAKE_ONLY IPC는 그때 승인 범위와 선행검사를 확인해 수행합니다. 실제 AI·usage·Discord·A 모드는 이 후보 설치와 별개입니다.

## 실패 정리·되돌리기

- **현재는 시스템 설치0**이므로 운영 rollback은 필요 없습니다. 새 후보/zip만 격리 보관하면 됩니다. 원본 후보를 덮어써 복구하지 않습니다.
- 향후 기존 이름 충돌이면 해당 기존 작업에 아무 작업도 하지 않고 중지합니다.
- 신규 생성 후 readback 불일치/결과 불명은 재시도·자동 삭제 없이 실제 생성 여부/경로/description/실행 경로/SID/SD를 관리자가 읽기 전용으로 확인해야 합니다. 성공 DeploymentRecord가 없을 수 있으므로 기존 Remove를 무조건 실행하지 않습니다.
- 자기 시험 작업임이 확정된 경우에만 새 작업을 중지/비활성화하고 PID tree0를 확인합니다. 새 SID가 기록과 일치할 때만 새 Bot 비활성화, 새 작업 단일 삭제를 별도 승인 범위에서 수행합니다. 코드/상태/프로필/소비 증거는 보존합니다. 기존 작업/운영 계정/ACL/인증은 건드리지 않습니다.

## 보호 및 검증 한계

실제 계정·task·서비스 생성0, 시스템 ACL/부팅 변경0, Host/IPC/FakeWorker0, 모델/공식usage/Discord0. 봇/아트 감시 재시작0, 운영SQLite0, 토큰/구독 인증 이동0, MANUAL 및 A HOLD 유지. Unity/게임/Save/Push/main/PC 변경0. 운영 건강과 OnBoot 정의 SHA를 새로 검증한 것은 아닙니다. boot-approval SHA `3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f` 보존 확인.

증거: `문서/00_프로젝트/팀운영/Evidence/Session0TaskSecurityAtomic20261026/`의 Tests.txt, Candidate.json, OriginalInventory.json, CandidateInventory.json, SourceSHA256.json. 실제 commit은 CURRENT_STATUS/CODEX_LATEST 마감 기록과 Issue66 참조.

## 공식 근거

- [RegisterTaskDefinition의 CREATE/기타 flags·credentials·SDDL](https://learn.microsoft.com/en-us/windows/win32/taskschd/taskfolder-registertaskdefinition)
- [GetSecurityDescriptor](https://learn.microsoft.com/en-us/windows/win32/taskschd/registeredtask-getsecuritydescriptor)
- [조회 대상 owner/group/DACL과 SACL의 권한 구분](https://learn.microsoft.com/en-us/windows/win32/secauthz/security-information)
- [Generic access rights의 객체별 매핑](https://learn.microsoft.com/en-us/windows/win32/secauthz/generic-access-rights)

**새 후보 오프라인 검증 후 중지합니다. 실제 Windows 등록/IPC는 미검증이며 별도 설치 승인 전 실행하지 않습니다.**
