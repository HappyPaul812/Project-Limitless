# Issue66 공식 사용량 단발 성공 — Session0 / logon4 (2026-10-26)

**USAGE_ONLY_SESSION0_VERIFIED / ONE_READ_COMPLETED / STOPPED / MANUAL**.

바울 대표가 실행한 신규 승인 request `773d9ecb7d944d9783c57ead117f471c`의 전달 JSON을 실제 ProgramData summary.json과 대조했습니다. 해당 단발 사용량 시험은 성공했습니다. 추가 조회·재실행·모델/A 활성화를 하지 않고 종료합니다.

## 실제 실행과 공식 결과

| 항목 | 실제 결과 |
| --- | --- |
| 서비스 계정 | PAUL-PC\LIMITLESSRelaySvc, 승인 SID 일치 |
| 서비스 세션 / 로그온 | Session0 / Batch logon4 |
| 관리자 부모 | 관리자 true / Session9, 서비스와 구분 |
| 격리 배포 | PASS_44_EXACT_40_PRESERVED |
| launch / usage read | 각각1회 |
| RPC | account/rateLimits/read, request id3, PASS |
| RPC error.code | null |
| app-server / Python Job / task 종료코드 | 각각0 |
| timeout / Job active processes | false / 0 |
| Job 정리 확인 | cleanup_verified=true |
| 실제 모델 / Discord / 자식SQLite업무 / 재시도 | 각각0 |

공식 응답 source=CODEX_APP_SERVER_OFFICIAL, ordinary_allowed=true. 관측시각은 **2026-10-26 10:58:46 KST**입니다. 아래는 그 단일 관측값이며 현재 사용량을 다시 조회한 수치가 아닙니다.

| 한도 창 | 사용률 | 잔여율(100-사용률) | 공식 resets_at의 KST 변환 |
| --- | --- | --- | --- |
| 300분(5시간) | 81% | 19% | 2026-10-26 12:19 |
| 10080분(주간) | 13% | 87% | 2026-11-02 07:19 |

원본 observed=1792979926.131802, resets_at=1792984740 / 1793571540입니다. 계정/토큰/인증 원문이 아닌 기존 응답의 정규화된 숫자만 기록했습니다. 결과 표시를 위해 추가 API를 호출하지 않았습니다.

## 종료·소비·운영 보호

- permit_deactivated=true, 활성 `ProgramData/LimitlessRelay/usage-only-approval.json` **부재를 실제 확인**. 해당 trial의 permit.consumed.disabled.json 존재 확인.
- 임시 작업 cleanup=REMOVED, task_exit_code=0. 성공한 Job의 active_processes=0 및 cleanup_verified=true. 이 결과는 실제 실행 스크립트가 종료 후 기록한 값이며, agent에서 기존 CIM 작업 검사를 반복하지 않았습니다.
- 새 승인 launch marker의 request773d...와 Output/child-run-consumed.json 존재 확인. 소비 상태를 그대로 보존했습니다. 같은 명령을 재실행하지 않습니다.
- 이전987a request의 marker SHA `62a867f542d6800a68f9a626d1c792ace7143081a7ff8e15ce1638b412cab97b` 그대로. previous_launch_marker_preserved=true를 실제 결과에서도 확인.
- boot_tasks_unchanged=true, ledger_unchanged=true, approval_unchanged=true. parent의 readonly 보호 함수는 전후 MANUAL 및 tasks/approvals/operation_mode를 확인한 코드이므로 **MANUAL 유지**를 결과와 함께 확인합니다. 자식의 SQLite 업무처리는0입니다.
- 기존 boot-approval SHA `3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f` 유지. CODEX_HOME unchanged=true. 기존 운영 코드/CLI/봇·아트/OnBoot/계정·인증 재설정·기존 ACL/Unity/게임/Save를 변경하지 않았습니다. 소유자의 로컬 보안 입력 외 암호·토큰을 복사하거나 출력하지 않았습니다.
- 새 ProgramData trial 패키지와 비활성 permit/로그는 증거로 보존합니다. 추가 live 배포/철수·기존 시스템 재시작·PIN/자동로그인/PC종료·재부팅·Push/main병합0입니다.

## 이번에 확정한 것과 남은 것

이번에 **공식 CLI의 기존 ChatGPT 구독 인증으로 서비스 Session0/logon4에서 단발 사용량 조회가 완료됨**을 확인했습니다. Windows PIN을 제거하지 않았으며 모델 호출도 하지 않았습니다. 관리자 부모가 임시 작업을 직접 등록한 시험이므로, PIN미입력 부팅 직후부터의 무인 AI 업무 체인 전체나 Discord→승인→실제 모델→결과 게시를 검증한 것은 아닙니다.

10월24일 OFFICIAL_READ_REJECTED의 원시 오류는 당시 유실되어 **직접 원인 미확정** 기록을 유지합니다. 이번 성공으로 과거 오류 원인을 소급 확정하지 않습니다. 이전 turn.failed도 별개입니다. 실제 모델 추론과 AUTO_RESUME/A는 **미실행·미승인**, 관련 Issue66 전체를 완전 해결/닫힘으로 처리하지 않습니다.

추가 시험은 하지 않고 이 단발 결과를 보고한 뒤 중지합니다. 기존44/22+12/Job4/변수검사 전체를 반복하지 않았으며 이번 턴은 보존된 결과·마커·permit을 읽고 완료 문서만 기록했습니다.

## 증거 및 커밋

실제 결과: `C:/ProgramData/LimitlessRelay/UsageTrials/773d9ecb7d944d9783c57ead117f471c/Output/summary.json`.

정제 공개 증거: `Evidence/UsageOnlySession0PASS20261026/`의 Summary.json 및 Completion.json. 서비스 자기 프로필의 상세 진단 경로/내용은 원래 보호 경로에 보존하며, 공개 사본에서는 해당 경로를 대체했습니다. 인증정보/원문SID/상세 private로그를 복사하지 않았습니다.

실행 소스 commit **b44da900d36f8940161d941139952cc789a5721f**, 브랜치 **codex/discord-usage-diagnostics-20261026**. 이번 docs-only 완료 commit은 Issue66/최종보고를 따릅니다. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / USAGE_ONLY_COMPLETE / AI_EXECUTION_HOLD**.
