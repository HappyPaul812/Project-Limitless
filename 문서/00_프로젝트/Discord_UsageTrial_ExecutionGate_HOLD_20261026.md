# Issue66 승인된 격리 배포·사용량 1회 — 실행 선행 조건 HOLD (2026-10-26)

**HOLD_EXECUTION_CONTEXT_BEFORE_DEPLOYMENT**. 바울 대표의 이번 지시는 격리 배포와 배포검증 PASS 뒤 실조회1회를 명시적으로 승인했습니다. 승인 부족으로 재확인을 요청한 것이 아닙니다. 지정한 필수 실행 조건을 현 환경에서 충족·검증할 수 없어 지시문의 중지 조건을 적용했습니다.

## 실제 확인과 중지 지점

기준0f98620bbd565e0ee8df6b743a6a251b21ae75b6·격리worktree clean을 확인했습니다. 검토 패키지의44개 SHA·기존40개 승인 보존·변경3+신규1·PSM1 보존·entry/roles/기존CLI/boot승인 SHA는 PASS입니다. 기존22/12 오프라인 기능검사는 반복하지 않았습니다.

현재 Windows 토큰의 실제 확인 결과는 **비관리자, 승인된 서비스계정 아님, Session9**입니다. 승인된 서비스계정 Session0/배치로그온4로 실행했다고 주장할 수 없습니다. 도구의 require_escalated는 Codex sandbox 밖에서 명령을 실행하는 범위이며 **Windows 관리자나 서비스 토큰을 부여하는 UAC 승격이 아닙니다**. 이 범위에서도 기존 OnBoot2개에 대한 읽기 시도는 Get-ScheduledTask의 CimJobException / HResult -2146233087로 실패했습니다. 실패 후 Task API 재시도·다른 로그온형식으로 대체·기존 OnBoot 변경·자격증명 탐색/복사/암호입력창/UAC/UI 조작을 하지 않았습니다.

따라서 Session0/logon4 실행 수단과 실제 task/supervisor20초 검증을 통과하지 못했습니다. **ProgramData 시험 배포 또는 활성 permit 생성 전에 중지**했습니다. 이는 공식 Codex/ChatGPT 사용량 RPC의 인증·권한·네트워크·프로토콜 오류가 아닙니다. app-server를 실행하지 않았으므로 그 RPC 원인 코드/method/id/stderr/exitcode는 이번에 존재하지 않으며 과거 OFFICIAL_READ_REJECTED나 turn.failed의 직접 원인도 여전히 미확정입니다. CimJobException의 HResult를 공식 서버 코드로 바꾸어 보고하지 않습니다.

## 조회·종료 상태

| 항목 | 실제 결과 |
| --- | --- |
| 승인 패키지 정적 SHA 점검 | PASS 44 / 기존40 보존 |
| 보호 ProgramData 격리 시험 배포 | NOT_DEPLOYED (선행 게이트 HOLD) |
| 서비스계정 실제 시험 | NOT_EXECUTED |
| Session0 / logon4 | NOT_VERIFIED |
| 공식 사용량 실조회 | NOT_ATTEMPTED, 0회 |
| 실제 모델 / Discord 게시 / trial SQLite 업무 | 각각0회 |
| 자동 재시도 | 0회 |
| trial 프로세스 생성 | 0개, 종료할 시험프로세스 없음 |
| 신규 request ID / 활성 permit | 발행·생성·활성화 안 함 |
| permit 비활성화 | NOT_NEEDED, 활성 permit 부재 재확인 |

소비마커/과거진단은 삭제하거나 초기화하지 않았습니다. 기존 비활성 승인 초안과 ReviewPackage는 그대로 보존했습니다. 종료/복구를 위해 기존 봇·아트 감시·CLI를 중지하거나 재시작하지 않았습니다. 운영 permit/코드 변경이 없으므로 운영 롤백도 필요하지 않습니다.

## 보호 근거

- 기존 boot-approval.json SHA `3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f` 그대로.
- 기존 CLI SHA `4fd2f057bad603bf53dc202457ffd0c3591de62e867a8b01e73816e3fbce5a73` 그대로. CLI를 실행한 검사가 아닌 파일 SHA 확인입니다.
- usage-only entry SHA `f176f64145009894659daecdeaed5085e633d624eebfcb0db3a651e0aea48fd7` 그대로.
- 운영52파일 SHA 불일치0, 운영worktree/원본main·porcelain 상태 동일. MANUAL은 readonly SQLite의 단일 mode 조회로 확인했으며 trial의 SQLite 업무 처리는0입니다. 현재 부팅 작업 정의/실제 봇 온라인까지 새로 검증했다는 뜻은 아닙니다.
- 승인파일·ACL·CODEX_HOME·인증/암호/토큰·OnBoot·봇/아트·A·Unity/게임/Save·Push/main병합·종료/재부팅 변경0. 이번 커밋은 격리 Markdown/정제증거만입니다.

## 재개에 필요한 실행 환경

이번 승인 범위를 반복해서 묻기보다 **기존 안전한 관리자/서비스 배치 실행 환경**을 확보해야 합니다. 그 환경에서 실제 Session0/logon4·정확한 격리 패키지 ACL·20초 전체 supervisor·기존 운영 보호를 먼저 검증한 뒤에만 ProgramData 배포/새 permit/새 request ID/단발 usage를 진행할 수 있습니다. 현재 사용자 일반 세션에서 실행해 서비스 시험으로 간주하거나 S4U/로그온/인증방식을 임의 변경하지 않습니다. 암호·토큰을 채팅에 전달하거나 인증파일을 복사하는 해결책을 사용하지 않습니다.

실조회 허용1회는 이번에 소비하지 않았습니다. 그렇더라도 자동 대기·예약·재실행하지 않고 여기서 중지합니다. 실제 AI 추론과 A 활성화는 계속 별도 승인 대상입니다.

브랜치 codex/discord-usage-diagnostics-20261026, 제품/진입점 마지막 commit0f98620b. 이번 로컬 문서 commit은Issue66/최종보고 참조. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_DEPLOYED / USAGE_NOT_ATTEMPTED**.
