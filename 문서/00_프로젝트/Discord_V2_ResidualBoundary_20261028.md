# Discord V2 잔여 HANDLE·프로세스 검증 — 경쟁 조건 수정 / 전체 독점성 HOLD

2026-10-28. 기준 Issue #66, 로컬6e211e6b57c1f0efb3fde04782935c26573655f8 및 대표 제공 관리자 결과. NO_GO / MANUAL / HOLD, 2026-11-01 마감 유지.

## 기존 관리자 결과 보존

원본 Result.json SHA256 `32D4AF881C34D10F6D09BB433872D4788F7B655EFEE6C0BD561255AF8DF1D8D8` 실제 일치 확인. 비 AppContainer/관리자/x64/비 Session0/대상 배치 계정 아님. OWNER RIGHTS, 공유잠금, 제한token거부, 빈Job ABI/DACL, 새객체 DACL, Task COM **6 PASS 보존**. 이전 비관리자 환경의 OWNER RIGHTS FAIL/Task COM 거부를 관리자 결과와 혼동하지 않습니다. 기존6 PASS 및 합성52 재실행0.

관리자 HANDLE 조회는 class64 크기 재할당 후 NTSTATUS0, 자체 HANDLE2개 실제 식별까지 성공했습니다. 이후 ready 파일 공유 오류32에서 외부 HANDLE 검사가 중단됐습니다. 프로세스 snapshot379개/PID4 읽기 성공과 별개로 다른 프로세스/토큰 접근 오류5가 발생했습니다. 당시 결과에는 실패 PID·단계가 없어 정확한 대상은 UNKNOWN입니다. 오류5만으로 시스템/PPL이라고 단정하지 않습니다.

## HANDLE 파일 경쟁 조건 및 최소 확인

원인: 자식은 최종 ready 파일을 CREATE_NEW/쓰기/공유Read로 먼저 노출한 뒤 쓰고 있었습니다. 부모는 File.Exists 직후 File.ReadAllBytes의 읽기/공유Read로 열었습니다. 공유 계약은 양방향이므로 부모의 공유Read가 자식의 기존 쓰기 HANDLE을 허용하지 않아 오류32가 날 수 있습니다. 파일 존재는 write/flush/close 완료 증명이 아니었습니다. 이는 운영 HANDLE 독점 검사의 결함이 아닌 시험 전달 순서 결함입니다.

수정: 새 시험 root 안에서 부모PID+nonce ready 경로와 자식PID 임시 경로를 분리했습니다. 자식이 임시에 write→flush→close 후 같은 디렉터리의 최종 이름으로 Move(overwrite=false)합니다. 부모는 게시된 파일만 읽고 자식PID·zero-rights 복제 증명을 확인합니다. 실제 HANDLE 검사 종료 후 별도 release 새 파일을 남기고, 자식은 제한 시간 내 자체 HANDLE을 닫고 종료합니다. release 기록 실패에도 부모의 자체 시험 자식 정리를 수행합니다. 기존 파일/승인 기록을 덮어쓰거나 보안 검사·공유 제한·Job 외부 HANDLE 금지를 완화하지 않았습니다.

실제 Windows 새 빈 Job+새 시험 자식으로 **게시 경로만 PASS**, peer_exit0. 구현 수정 후 같은 관련 항목만 확인했습니다. 이번 실행은 AppContainer이므로 전체 HANDLE 조회 거부가 이미 확인된 경로와 기존6개를 반복하지 않았습니다. 따라서 파일 경쟁 조건 수정 확인과 실제 외부 HANDLE 차단 PASS를 구분합니다. 후자는 관리자 잔여 확인 전까지 UNKNOWN입니다. 관리형 시험 및 Supervisor 일반 컴파일 각각 경고0/오류0, NativeAOT 아님.

공유 계약 근거: [Microsoft CreateFile](https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea). 준비된 파일의 게시 방식 변경은 보안 검사 대신 사용하지 않습니다.

## 프로세스 독점성 판정

기존 Exclusive에 선택적 관측 출력을 추가했습니다. 진단 호출만 PID·이름·PROCESS_OPEN_UNKNOWN/TOKEN_OPEN_UNKNOWN/OTHER_SID_CONFIRMED/TARGET_SID_FOUND를 로컬 JSON에 기록하며, 거부 후 후속 읽기 관측도 모읍니다. 미확인이 한 건이라도 있으면 마지막에 반드시 HOLD합니다. production 호출은 관측 인수 없이 기존처럼 첫 실패에서 차단하고, 대상SID 발견도 즉시 차단합니다. 접근 권리0x1000 및 token query 유지, 특권 활성화/우회/운영 process 변경 없음. 시스템/PPL 분류를 추측해서 예외 허용하지 않습니다. 관리자 직접 잔여 관측 전에는 실패 대상 PID를 확정할 수 없습니다.

기존 구현 검토 결과 **전체 프로세스 열거를 대체하는 동일한 보장은 입증 불가**입니다.

- Job active1·생성시 JOB_LIST·자식 생성 제한은 해당 Job의 소속 Probe/자식에 대한 경계입니다. Job 밖에 별도로 생성된 동일SID 프로세스의 부재를 증명하지 않습니다. [Microsoft Job Objects](https://learn.microsoft.com/en-us/windows/win32/procthread/job-objects), [ActiveProcessLimit](https://learn.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-jobobject_basic_limit_information).
- 비상속 HANDLE·DACL은 새로운 접근을 제한하지만 이미 존재하는 외부 HANDLE을 회수하지 않습니다. 실제 HANDLE census는 해당 보호 객체의 외부 참조를 검사하며 동일SID 전체 프로세스 부재와 다른 보장입니다.
- CREATE_NEW/공유없음/영구 소비 marker는 같은 보호 경로와 계약을 지키는 진입을 한 번으로 제한합니다. 이 marker를 열지 않는 다른 프로세스까지 커널이 같은 Job/lease로 강제 편입하지 않습니다.

이 결론은 기존 코드와 공식 Job 적용 범위에 따른 검토 결과이며 전체 Windows 프로세스 부재의 실제 증명이 아닙니다. 새 아키텍처·PPL 허용목록·권한 확대를 만들지 않습니다. [Microsoft Process Security](https://learn.microsoft.com/en-us/windows/win32/procthread/process-security-and-access-rights)와 같이 보호된 프로세스는 접근 제한이 존재하므로 관리자라는 이유만으로 확인된 것으로 처리하지 않습니다.

## 다음 최소 절차 / 중지

기존 전체8항목 명령은 재사용하지 않습니다. 별도 Git 제외 로컬 보호 문서에 새 DLL/deps/runtimeconfig·입력·기존 관리자 결과·스크립트 SHA를 고정한 **잔여2항목만**의 관리자 PowerShell7 x64 명령을 준비하고 구문 검사했습니다. 관리자 직접 처리 지시에 따라 Codex의 승격 실행0. 새 E 시험 root/자체 시험 자식만 사용, 현재 계정·Task·기존 ACL·권한 설정 변경 없음. PID/프로세스명/상세 명령은 공개 보고에서 제외합니다. 부분 실행은 두 항목 모두 PASS여도 전체8개 실행이 아니므로 runner_exit2/HOLD로 보존하며 각 row를 따로 판단합니다.

최소 후속은 대표 직접 잔여2개 읽기/격리 시험 결과 확인입니다. 접근 불가 항목이 남으면 전체 독점성 UNKNOWN을 유지하고 NativeAOT를 진행하지 않습니다. 현재 계약의 전체성 보장을 축소하는 승인을 이 작업에 포함시키지 않았습니다. 이번 NativeAOT Probe/Supervisor 발행 각0회·새 native 이미지/SHA 없음, 실행gate=false, 기존 공식 NuGet7 보존/Restore·다운로드·설치0. 실제 Session0 단발 시험은 전체 필수 보안 조건 통과와 별도 계정/등록/시작/정리 승인 전까지 불가합니다.

운영 계정·Task·기존 ACL·OnBoot·운영 프로세스·서비스·Unity·Save·승인 소비 기록 변경0. 새 시험 자원만 생성/정리, Session0/업무IPC/AI/Discord 실행0, Push/main 병합0. 브랜치 codex/discord-usage-diagnostics-20261026, 로컬 최종SHA는 완료보고/Issue66. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 11-01 미검증이면 추가 재시도 대신 증거와 복구 가능한 인계를 보존하고 게임 개발 복귀.