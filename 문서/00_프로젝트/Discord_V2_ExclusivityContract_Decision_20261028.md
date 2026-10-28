# Discord V2 최종 프로세스 독점성 보안 계약 판정

2026-10-28 / Issue #66 / 기준54e924ad002ec5ac446e698507b8d5e6f2e28e1c.

**판정: 현 계약 NO_GO / MANUAL / HOLD 유지. 권고: 전용 FAKE_ONLY 진단 실행의 한정된 독점 계약으로 명시적 변경 승인 검토. 이번 문서는 변경 제안이며 현재 계약·코드·게이트를 변경하지 않습니다.**

## 1. 기존 계약과 위협 모델

[최소 배치 경로 명세](Discord_Session0_MinimalBatch_PathSpec_20261028.md)의 §1은 전체 대상SID 프로세스를 Supervisor1+Probe1로 규정하고 Scheduler helper조차 임의 제외하지 않습니다. §4.2는 외부 동일SID 실행기·중복·위험 HANDLE 확인 불가 시 생성 전 HOLD, hostile 동일SID 환경의 sandbox 인증은 NO_GO라고 명시합니다. [토큰 격리 준비](Discord_DedicatedV2_TokenIsolation_Preparation_20261027.md) §3은 외부 privileged HANDLE 우회 및 숨은 Job duplicate로 인한 kill-on-close 손상을 위험으로 봅니다. 현재 README와 HandleCensus.Exclusive도 확인 불가를 예외 처리하지 않습니다.

따라서 전역 부재 검사는 단순 Task 중복 방지보다 강한 보수적 승인 조건입니다. 같은 계정의 별도 실행기가 승인 경로를 우회하거나 증거/승인 소비 파일에 접근하고, 기존 HANDLE을 보유하거나 감독/Probe의 경계에 간섭할 위험을 줄이려는 것입니다. 파일·객체 ACL은 token 신원을 판단하며 같은SID의 서로 다른 실행파일을 자동으로 분리하지 않습니다. 이 동기를 사후에 불필요한 검사였다고 삭제하지 않습니다.

동시에 원래 문서도 악의적 동일SID 공존을 격리했다고 인증하지 않았습니다. 전역 부재의 현재 구현은 보조 관측이며 커널이 계정의 모든 프로세스 생성을 제어하는 강제 경계가 아닙니다.

## 2. 실제 증거와 공식 Windows 제한

기존 대표 제공 결과만 재사용: 7 PASS / 1 UNKNOWN_HOLD. 잔여 결과 원본 SHA256 `D80BEEBFAC78A0F36105CD07C7A50E5D5AFE53A6BC2E388D21C3210DB908FB2B`는 이전 응답에서 실제 일치 확인했습니다. HANDLE table 조회/자체2개 식별/외부 zero-rights Job HANDLE 감지·차단/시험 자식 정상 종료까지 PASS. 다른SID 확인381개, System 프로세스 token query 한 건 ERROR_ACCESS_DENIED. 이번 재조회·기존 PASS 재실행0. 대상 배치 token/실 Session0의 관측 성공 증거로 확대하지 않습니다.

[Microsoft OpenProcess](https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openprocess)는 System/CSRSS에 사용자 모드 접근 제한이 있음을 설명합니다. 그러나 이번 실제 관측은 query-limited process open 성공 후 **OpenProcessToken 실패**이므로 OpenProcess 문서만으로 정확한 token 실패 원인을 입증할 수 없습니다. [OpenProcessToken](https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openprocesstoken)은 process 조회 권리 외에도 요청한 token 권리가 token DACL 검사 대상이라고 명시합니다.

판정: 공식 보안 모델에 부합하는 허용 가능한 접근 거부이며 관리자에게 모든 token query가 보장된다는 공식 계약은 없습니다. 정확한 내부 거부 조건/보호 유형/PPL 여부는 미확정입니다. 표시명·PID나 관례로 TokenUser를 추정하지 않고, 해당 프로세스를 대상SID와 다르다고 예외 처리하지 않습니다. SeDebug/승격/커널 디버깅 등으로 알아내는 방법은 이번 승인 밖이며 제안 실행하지 않습니다.

[Microsoft Snapshots of the System](https://learn.microsoft.com/en-us/windows/win32/toolhelp/snapshots-of-the-system)의 snapshot은 읽기 시점 복사본입니다. SID를 모두 읽어도 이후 생성·종료·권한/HANDLE 이동을 막지 않습니다. 현재 주기 검사는 간격 사이의 경쟁 조건을 제거하지 않습니다. 그러므로 **지속적인 Windows 전체 동일SID 프로세스 부재**는 이 사용자 모드 열거 알고리즘만으로 증명할 수 없습니다. 현재 제한된 Supervisor에서 관리자조차 읽지 못한 token까지 읽을 수 있다고 가정할 수도 없습니다.

## 3. 기존 구현의 실제 강제 범위

| 장치 | 설정/구현 시 강제하는 범위 | 보장하지 않는 범위·현재 검증 한계 |
|---|---|---|
| private Job active1, no breakaway, JOB_LIST, child restriction | 생성하는 Probe를 Job에 편입하고 해당 Job 동시 대상1개와 생성 경계를 제한. Supervisor는 Job 밖 별도1개 | 다른 경로/다른 Job의 동일SID 프로세스 부재. Job 전체 count1을 Windows 전체 count1로 부를 수 없음. 실제 Session0 통합 생성/종료는 미검증 |
| protected CREATE_NEW·공유없음·영구 소비 marker | 같은 보호 경로에서 계약을 따르는 진입의 단발 소비·동시 열기 거부, 실패 후에도 자동 재소비 금지 | marker를 사용하지 않는 별도 실행기 부재/신뢰성. crash·선점·부분 기록 시 가용성 실패를 성공으로 돌릴 수 없음 |
| DACL/OWNER RIGHTS·비상속 HANDLE·외부 census | 새 접근 제한, 자기 생성 HANDLE의 신원 확인, 검사 시점 위험 외부 참조 거부. Job은 zero-rights 외부 참조도 거부 | DACL 변경 전 획득된 권리를 자동 회수하거나 미래 HANDLE 전송을 영구 금지하지 않음. 실제 배치 token/동작 전체 입증은 별도 |
| 커널 동기화 객체 | mutex 등이 있다면 해당 객체를 함께 획득하는 thread 사이에서만 배타적 소유 가능 | production에는 별도 전역 named mutex/semaphore 기반 승인 장치가 없음. 시험용 event를 운영 보장으로 셀 수 없음. 추가해도 비협조 실행기 차단이나 전역SID 인증은 안 됨 |
| Task IgnoreNew·disabled/trigger0·고정 Action/승인 receipt | 승인된 해당 Task 정의·instance의 중복 시작 제한 및 코드의 승인 진입 조건 | 다른 Task, 수동 실행, 다른 실행파일·계정 자격증명 사용을 전역 차단하지 않음. 신규 Task 등록/실행 전이므로 현재 설정이 실제 배포됐다고 주장하지 않음 |

Job 적용 범위는 [Microsoft Job Objects](https://learn.microsoft.com/en-us/windows/win32/procthread/job-objects), Task 단위 중복 정책은 [TASK_INSTANCES_POLICY](https://learn.microsoft.com/en-us/windows/win32/api/taskschd/ne-taskschd-task_instances_policy)를 따릅니다. [CreateMutexW](https://learn.microsoft.com/en-us/windows/win32/api/synchapi/nf-synchapi-createmutexw)는 mutex의 공유/선점 위험도 설명합니다. 동기화 객체는 계정 전체의 실행 권한 경계가 아닙니다.

합산 판정: 신뢰된 OS/관리자 아래 **동일 보호 계약에 참여하는 전용 실행의 단발 진입, 해당 Job 안의 대상 수, 검사 시점의 보호 HANDLE 경계**는 기존 구현으로 표현할 수 있습니다. 현재 7 PASS는 그중 fixture 경계의 근거이며 실제 전용 실행 전체의 PASS는 아닙니다. 전용 실행 독점성과 Windows 전체 동일SID 부재는 서로 다른 주장입니다.

## 4. 최종 권고와 승인 대상

**현행은 유지/HOLD. 향후에는 전용 FAKE_ONLY 진단 시험에 한정한 계약 변경 검토를 권고합니다.** Windows 보호 프로세스 SID 확인을 위해 더 높은 관측 도구를 덧붙이거나 PID 예외를 만드는 대신, 실제 커널 강제 범위를 계약과 맞추는 편이 타당합니다. 단, 목적이 hostile 동일SID 공존까지 견디는 sandbox라면 이 변경은 부적합하며 NO_GO를 유지해야 합니다.

변경 제안의 정확한 차이:

- 기존: Supervisor/Probe 외 Windows 전체 동일SID 프로세스가 없음을 확인하지 못하면 실행 거부.
- 제안: 승인된 보호 코드/receipt/단발 marker에 결합된 Supervisor1과 private Job 내부 Probe1의 전용 시험 경계를 보장 대상으로 한정. **전역 동일SID 부재와 관측 불가 token은 UNKNOWN으로 계속 기록**, PASS로 재분류하거나 PID4만 제외하지 않음. 승인 밖 동일SID 코드와 자격증명 사용이 없다는 신뢰 가정을 명시적으로 수용해야 함.
- 유지할 조건: noIPC/noAI/고정 이미지SHA·입력·만료·정확 Task·별도 동작 승인, 보호 경로/ACL/AccessCheck/비상속 HANDLE·위험 외부 HANDLE 거부, 생성 시 Job 편입·active1·child/breakaway 제한, 영구 단발 소비, 자기 객체만 종료/실패 증거 보존, 명확한 오류의 HOLD. 위험 동일SID가 실제 확인되면 묵인하지 않는 진단 조건은 유지하는 방향으로 검토.
- 새로 수용하게 되는 위험: 관측 불가/열거 사이의 비협조 동일SID 프로세스·별도 자격증명 실행, 같은SID의 증거 선점/위조·가용성 방해, 사전 HANDLE 간섭을 전역 부재 조건으로 배제하지 못함. 관리자/SYSTEM 또는 커널 침해를 방어한다고 주장하지 않음. Job·lease·mutex로 이 위험이 사라진다고 부르지 않음.
- 보완 대책/선행 조건: 기존 고정 이미지/무업무 시험·보호 코드/관리자 소유 receipt·Worker 증거와 관리자 증거의 구분·관측 오류 기록/소유 HANDLE 기반 정리·짧은 단발과 기존 소비 영구 보존을 유지. 자격증명 비공유/다른 동일SID 실행 경로 미사용은 대표가 확인·수용할 운영 가정으로 명시. 이 가정을 확보할 수 없거나 악의적 동일SID 공존 방어가 필요하면 변경안도 NO_GO. 새 계정/권한/ACL·동기화 장치 추가를 이번 대책으로 실행하지 않음.

이 축소는 원래 위험을 동일 수준으로 해결했다는 뜻이 아닙니다. **대표 승인 필요: 전용 FAKE_ONLY 범위로의 보장 축소, 전역 UNKNOWN 유지, 신뢰 가정 및 잔여 간섭·가용성 위험 수용**. 승인만으로 코드/증거가 자동 PASS가 되지 않습니다. 승인 후에도 기존 문서·판정 schema·gate와 source 검사의 모순을 함께 정리하고 필요한 최소 검증을 별도 범위로 확정해야 합니다. 이번 그 구현/검증/승인 발급은 하지 않았습니다.

기존 전역 조건을 필수로 유지할 경우: 현재 계정·권한·API와 보호 조건 안에서는 token 전체 가시성과 지속적인 전역 생성 통제가 확보되지 않아 충족 가능한 실행 경로를 제시할 수 없습니다. 반복 PID4 query, privilege 활성화, 예외·권한상승으로 우회하지 않고 HOLD로 인계합니다. 일반 관리자 성공조차 제한된 실제 Batch token 성공의 대체 근거가 아닙니다.

## 5. 중지 상태

이번은 문서/기존 코드·공식 문서 검토만 수행했습니다. PID4/프로세스/Task 현재 조회0, 기존 PASS·빌드·검증기 생성0, NativeAOT0, source/보안 계약/gate 변경0. 기존 7 PASS/1 UNKNOWN·gate=false 보존. 계정/ACL/OnBoot/운영 Task·프로세스/Unity/Save/승인 소비 기록 변경0, IPC/AI/Session0/Push/main 병합0.

브랜치 codex/discord-usage-diagnostics-20261026. 마지막 관련54e924ad002ec5ac446e698507b8d5e6f2e28e1c, 이번 문서 최종SHA는 완료 보고/Issue66. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 2026-11-01까지 실동작 미검증이면 추가 재시도/자동 연장 없이 인계·증거와 복구 가능 상태를 보존하고 게임 개발 복귀합니다.