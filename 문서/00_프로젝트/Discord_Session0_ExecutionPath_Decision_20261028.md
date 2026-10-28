# Session 0 실행 경로 결정 — 2026-10-28 / 11-01 최종 집중 기간

판정: **NO_GO_CURRENT_CONDITIONS**. 현재 보호 조건에서 실행 가능한 경로는 확보되지 않았습니다. Windows 기능 자체의 불가능 판정은 아닙니다. 준비 조사만 완료하고 중지합니다.
브랜치 `codex/discord-usage-diagnostics-20261026`, 기준 `c04c17c99a5d71d387c9f36de0c697e16cf7a51e`. 최종 문서 commit은 Issue #66 후속 댓글에 기록합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

## 실제 확인 근거와 한계

- Issue #66 전체 댓글을 조회해 최신 `6441915174`의 기준 c04c17c9, 실제 실행 HOLD를 확인했습니다. 이전 28·31·48 PASS와 Task descriptor PASS는 재사용했습니다. 이번 새 모의시험·검증기·모델 제작 및 실행은 0입니다.
- 2026-10-28T00:58:32.7436628Z(09:58:32 KST) 읽기 전용 Windows 조회: 조사 프로세스 SessionId=11, SID 끝1001, Elevated=false. 관리자 PowerShell 또는 Session 0 실행 주체가 아닙니다.
- 정확 Task `\LIMITLESS-DedicatedV2-20261027-Host` COM 조회: UnauthorizedAccessException/HRESULT -2147024891(0x80070005). 현재 Task 상태 UNKNOWN이며 부재로 해석하지 않습니다.
- Schedule/SamSs 상태 조회: ServiceCommandException/-2146233087. 계정 LIMITLESSWorkV2D 조회: NullReferenceException/-2147467261. 현재 서비스 가동·계정 상태 UNKNOWN. 오류를 우회하거나 관리자 재조회하지 않았습니다.
- 과거 실제 관리자 수집(10-27T11:53:17Z): Task Principal SID1010, Password1/Limited0, Disabled/Trigger0/Restart0/IgnoreNew/HardTerminate/PT90S, descriptor 읽기 PASS. 이것은 과거 Windows 관측이며 현재 재확인/실제 토큰 PASS가 아닙니다.
- 과거 관리자 수집 Admin-33a677…(10-27T11:15:47Z)의 SHA256 `4de232b3d5af7aa6662ce4e4ec6a4d28ddc529b5c0d50656a9114f71e679140e`를 다시 확인했습니다. Worker1010 Enabled/Users, 직접 Batch 허용 및 Interactive/RemoteInteractive/Network/Service 거부가 기록돼 있습니다. 유효 토큰·간접 정책까지 증명하지 않습니다.
- 같은 수집의 계정 만료 예정 **2026-10-28T10:36:51Z = 오늘 19:36:51 KST**. 설치 소스도 계정을 생성시각+24시간 만료로 설정합니다. 현재 만료값 재조회는 실패했으므로 '이미 만료'로 단정하지 않습니다. 11-01까지 시험하려면 현재 만료 확인과 필요한 경우 명시적 연장 승인이 필요합니다.
- Task Action은 고정 pwsh → `Code/V2/start-host-v2.ps1`입니다. 현재 저장소 사본 SHA256 `063c063223f8989605b1031ea0978d890b43ee95d4787e8ad578b923ba361f46`은 기존 관리자 Host pin과 같습니다. 보호 설치파일을 이번에 직접 다시 읽었다는 뜻은 아닙니다.
- 해당 Host 소스는 DedicatedIpcApproval 검증 → worker.lease → runtime admission → ipc 소비 marker CREATE_NEW → LimitlessV2Host.Run으로 이어집니다. 토큰 관측 전용 진단 PE가 아닙니다. FAKE_ONLY 표기 자체가 무IPC·무소비를 뜻하지 않습니다. 시작 후 안전 게이트가 막을 것이라고 기대하여 실행하지 않습니다.

## 공식 지원 경로 비교 및 하나의 최소 변경 후보

| 경로 | 현재 사용 가능 여부 | 결정 이유 |
|---|---|---|
| 기존 r2 Password Task 그대로 | NO_GO | Disabled이고 고정 Action이 IPC/승인소비 Host. 활성화만으로 이번 계약 충족 불가. Action/Manifest 교체는 기존 설치 봉인 변경 |
| 일반 관리자 PS → LogonUser/CPAU | NO_GO | 관리자 여부만으로 Session0/필요 privilege 입증 불가. 현재 조사 세션은 비상승 Session11. TCB·권한 추가/Session 변경으로 우회 금지 |
| 기존 서비스 활용 | NO_GO | 현재 서비스 상태·승인된 진단 진입점 근거 없음. 기존 프로세스/서비스 재시작·주입·IPC로 전용 금지 |
| SID1010 전용 새 서비스 | NO_GO | 과거 직접 DenyService 기록. 서비스권리 변경·등록·native 서비스 수명주기 필요, 변경이 더 큼 |
| 별도 SYSTEM 서비스/Task 감독 + Batch1010 | NO_GO | SYSTEM 주체와 암호 전달·새 실행/특권 경계 추가. 기존 대상1개 계약보다 큰 변경 |
| **별도 일회성 SID1010 Password 배치 Task + native 감독기** | **최소 변경 후보로 선정, 아직 NO_GO** | Windows Scheduler가 Batch 로그온 담당. 감독기가 같은 토큰으로 진단 child 생성하므로 별도 TCB/CPAU 권한 부여 없이 설계 가능. 기존 r2/OnBoot/설치 봉인은 보존. 신규 Task·PE·보호 경로 및 총2프로세스 승인 필요 |

S4U도 Windows 공식 비대화형 배치 선택지지만 기존 Password와 다른 인증 방식이며 EFS/네트워크 제한이 있습니다. 미검증 인증 방식을 추가하지 않고 Password 배치 하나로 후보를 좁힙니다. 기존 저장 암호를 추출·복사할 수 있다는 가정은 없습니다. 현재 암호를 권한 있는 바울 대표가 로컬 보안 입력으로 제공할 수 없다면 이 후보도 차단됩니다. 암호 재설정은 제안에 포함하지 않습니다.

위 후보의 Session0는 설계상 예상이며 실제 Windows 검증 전 보장하지 않습니다. Scheduler 서비스의 현재 가동·등록 권한·로그온 정책·암호/계정 유효성·OS attribute 지원·중첩 Job은 아직 확인되지 않았습니다.

## 바울 대표가 승인해야 할 정확한 변경 범위

1. 기존 r2 Task 재등록/활성화/Action 교체 없이 **새 고유 이름의 진단 Task 1개**와 별도 보호 staging/evidence 경로 생성. Trigger0/재시도0/IgnoreNew/Limited/Password, 수동 단발만. 생성·활성화·실행·종료 후 비활성화/삭제 범위는 별도로 봉인하고 승인받아야 합니다. 기존 Task ACL/OnBoot는 변경하지 않습니다.
2. native 감독기1 + 진단 대상 PE1의 **총2 SID1010 프로세스** 허용. 기존 '대상1개'는 Job 내부 진단 대상1개로 유지하되, 외부 SID1010 감독기가 추가됨을 명시적으로 승인해야 합니다. 두 프로세스를 이미 승인된 것으로 취급하지 않습니다.
3. 새 코드/증거 위치에 한정한 BA/SYSTEM 소유·Worker RX/필요 evidence 쓰기 ACL 설계와 검토. 기존 Code/State/Auth ACL 변경0. 같은 SID 감독기/대상 간 handle·증거 변조 가능성과 외부 handle 경계는 native 구현 검토에서 해결해야 합니다. 입증 불가하면 HOLD입니다.
4. 필요 시 **Worker1010 계정 만료만 유한 연장**(예: 11-01 집중기간 종료까지의 확정 시각). 암호·권리·그룹·다른 계정 변경은 제외합니다. 현재 값이 이미 연장돼 있으면 변경하지 않습니다.
5. 준비된 PE2개의 실제 바이트 SHA/Action/XML/전후 fingerprint/고유 승인 ID/단발 소비·cleanup 경계를 검토한 후 실제 Batch 로그온1회·Task 시작1회·대상1회 실행을 별도로 승인. 기존 consumed 초기화·설치승인 재사용0.

이번에는 위 변경을 하나도 실행하지 않았습니다. 현재 승인 금지 조건이 유지되면 경로는 계속 NO_GO이며 추가 모의시험으로 우회하지 않습니다.

## 실행 주체·Session0 판정·90초·실패 정리 계약

- 관리자는 보호 파일·새 Task 등록/검토/단발 시작/자기 Task 정리를 담당하며, UI 포커스 전환 없이 수행할 절차가 필요합니다. 관리자 콘솔을 Session0로 간주하지 않습니다.
- Scheduler가 SID1010 Password Batch 로그온으로 native 감독기를 실행하고, 감독기는 자기 실제 TokenUser/AuthenticationId와 LsaGetLogonSessionData(LogonType=4), TokenSessionId=0, 비상승/비관리자·실제 그룹/privileges를 확인합니다. 하나라도 불명/불일치면 child 생성 전 종료. 승인 전 새 로그온/토큰 조회는 하지 않습니다.
- 대상은 같은 primary token의 CreateProcessW + 고정 절대 PE + 최소 환경·상속 handle 없음으로 생성할 후보입니다. 새 Session 조작/토큰 빌림/impersonation0. 반환 process handle과 생성UTC/AuthLUID/SID/Session을 결합하여 재확인하고 기존 AccessCheck core를 연결합니다. PID만으로 재탐색하지 않습니다.
- 감독기만 소유하는 private noninheritable Job에 PROC_THREAD_ATTRIBUTE_JOB_LIST로 생성 시 원자 편입, child restriction/active1/no-breakaway/KILL_ON_JOB_CLOSE/readback을 요구합니다. API/중첩Job 미지원이면 생성 전 HOLD, 사후 Assign fallback0.
- 감독 시작 기준 단조 시계 예산90초, 85초 이내 자기 Job Terminate, 남은5초 active0·process signaled·handle 정리·최종 evidence 확인. Task PT90S/HardTerminate는 추가 안전장치이며 Job의 전체 자식 정리 증명이나 정확한 wall deadline 증명이 아닙니다. CPU시간 제한도 대체물이 아닙니다.
- **현재 native 감독기/PE가 없으므로 Job 90초 종료 보장은 확보되지 않았습니다.** 감독 정지/OS sleep/API block/handle duplicate/시계 범위를 포함한 실측 근거 없이는 GO 불가. Windows 일반 스케줄링에서 절대 실시간 보장을 약속하지 않습니다. 바울 대표가 엄격한90초 walltime을 예외 없이 요구하면 이 한계도 별도 NO_GO 요인입니다.
- 부분 실패 시 resume0/자기 Job만 종료·drain, 부모/자식/thread/job handle 해제, 오류단계/Win32/NTSTATUS/시간/정리결과를 새 증거에 보존. 감독 강제종료 시 마지막 Job handle close에 의존하며 finally/최종로그 보장은 주장하지 않습니다. 사전 intent만 있으면 UNKNOWN_CLEANUP/HOLD.
- cleanup 불명 시 자동 재시도/전역 PID kill/기존Task 종료/소비삭제0. 별도 승인된 새 Task instance와 생성UTC 일치 process handle만 사후 확인·정리 대상으로 제한합니다. 로그·PE·등록 XML/만료값 전후 근거는 보존하고 cleanup 완료 전 새시험 금지.

## 다음 단발 명령과 중지 조건

**지금 제공할 수 있는 실제 FAKE_ONLY 시험 명령은 없습니다.** 기존 inspect 스크립트는 Run 차단만 구현했고 native launcher/PE는 없습니다. 기존 Task를 시작하는 명령을 대체 명령으로 발급하지 않습니다. 이 NO_GO 판정 아래 실행 명령을 만들어 붙이는 것은 안전 조건을 충족하지 못합니다.

다음 실행 명령을 발급할 전제는 새 Task/PE/2프로세스/필요 만료 연장 범위 승인 → 코드와 보호 staging 준비·SHA 봉인 → 관리자 읽기 전용 현재상태 확인 → 정확 Task 이름/승인ID/90초·Job/정리계약과 복사 가능한 명령을 최종 검토 → 바울 대표의 **별도 실행 승인**입니다. 준비 승인과 실행 승인을 합치지 않습니다. 이번에는 그 준비 변경도 제안까지만 수행했습니다.

현재 HOLD: 실제 SID1010 Session0 Batch 토큰·유효권한/간접그룹·외부 handle/독점·native Job90/cleanup·PE2/보호 staging·현재 서비스/계정 상태·실행 승인. V1/MANUAL/A/AI/운영 HOLD 유지.
Task 시작/서비스 재시작/새 시험 로그온·대상 프로세스·Job 생성/IPC/AI/사용량조회/계정암호권리ACL변경/승인소비 초기화/게임변경/Push/main병합0.읽기 조사용 기존 도구 호스트 실행과 시험 프로세스 생성을 구분합니다.

**2026-11-01까지 실동작이 검증되지 않으면 추가 재시도 없이 인계·현재 증거·원복 가능한 상태를 보존하고 게임 개발로 복귀합니다.** 이번 준비 보고 후 자동 진행하지 않습니다.

## 공식 근거 (Windows 관측과 별개)

- [Task Enabled](https://learn.microsoft.com/en-us/windows/win32/taskschd/taskschedulerschema-enabled-settingstype-element): Enabled=true일 때만 수행 가능.
- [Security Contexts](https://learn.microsoft.com/en-us/windows/win32/taskschd/security-contexts-for-running-tasks), [Logon types](https://learn.microsoft.com/en-us/windows/win32/api/taskschd/ne-taskschd-task_logon_type): Password/S4U Batch 권리·등록 자격증명·비대화형 방식의 차이.
- [CreateProcessAsUserW](https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-createprocessasuserw): 토큰 Session/필요 privilege 조건.
- [Interactive Services](https://learn.microsoft.com/en-us/windows/win32/services/interactive-services): 비대화형 서비스 경계.
- [Job Objects](https://learn.microsoft.com/en-us/windows/win32/procthread/job-objects), [Creation attributes](https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-updateprocthreadattribute), [Task time limit](https://learn.microsoft.com/en-us/windows/win32/taskschd/tasksettings-executiontimelimit): 생성·중첩·종료와 Task 시간제한은 별개 계약.
