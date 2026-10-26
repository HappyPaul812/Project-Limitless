# Issue66 단발 시험 결과 — 실행기 변수 충돌 / 공식 조회0 (2026-10-26)

**HOLD_AFTER_LAUNCH / OFFICIAL_USAGE_NOT_STARTED / LOCAL_FIX_NOT_DEPLOYED / NO_RETRY**.

바울 대표 전달 request `987a8f4268534a0496a9e600e7af5dd7`의 실제 parent/child summary와 배포된 source를 확인했습니다. 실패 원인은 Codex가 작성한 PowerShell 실행기의 변수 이름 충돌 두 건입니다. 서비스 Session0 환경·구독 인증·네트워크 실패로 돌리지 않습니다.

## 직접 원인 1 — 서비스 자식

배포본 run-approved-usage-once.ps1:45의 `$home=Join-Path $serviceProfile '.codex'`는 PowerShell에서 대소문자를 구분하지 않는 **읽기 전용 자동 변수 `$HOME`**에 쓰려는 문장입니다. 실제 child-summary는 승인SID일치/계정 `PAUL-PC\LIMITLESSRelaySvc`/Session0/logon4를 기록한 뒤 `SessionStateUnauthorizedAccessException`, HResult -2146233087로 HOLD했습니다. 이는 **PowerShell 변수 쓰기 거부**이며 서비스 계정 파일 ACL 또는 ChatGPT 권한 거부라는 뜻이 아닙니다.

`Get-Variable HOME`의 Options=ReadOnly, AllScope를 확인했습니다. 격리 후보에서 `$serviceCodexHome`으로 이름만 바꾸고 이후 경로 참조를 맞췄습니다. CODEX_HOME 환경변수·실제 프로필·인증·권한을 바꾼 것이 아닙니다.

## 직접 원인 2 — 관리자 부모의 결과 수집

배포본:169의 `$child=Join-Path $out 'child-summary.json'`는 스크립트 매개변수 **`[switch]$Child`**와 충돌합니다. 문자열 경로를 switch로 바꾸려다 실제 parent-summary의 ArgumentTransformationMetadataException → PSInvalidCastException이 발생했고, 자식 결과를 부모 summary에 붙이지 못했습니다. 이 때문에 `usage_read_attempts`가 null로 남았습니다.

격리 후보에서 결과 파일 변수만 `$childSummaryPath`로 바꿨습니다. `-Child` 매개변수·보안/계정/모드/승인 검사는 유지합니다.

## 실제 도달 범위와 횟수

- 격리 ProgramData 배포 **PASS_44_EXACT_40_PRESERVED**. 서비스 계정 실제실행·Session0/logon4·SID 일치 확인.
- 자식의 첫 변수충돌은 private로그디렉터리 생성/Job생성/Python실행보다 앞입니다. 그 이후 문장에 있는 Output/child-run-consumed.json은 실제로 **없음**을 확인했습니다. 배포 source SHA도 기존 ea7a24...와 일치하므로 실행 흐름을 특정할 수 있습니다.
- **공식 사용량 provider/Python/app-server 시작0, 실제 사용량 RPC0, 모델0, Discord0, 자식SQLite업무0**. `usage_launch_attempts=1`은 임시 작업 시작 시도이며 공식 사용량 API1회라는 뜻이 아닙니다.
- 이번에는 새로운 RPC method/id/error.code/stderr/종료code가 없습니다. 기존 OFFICIAL_READ_REJECTED/과거 turn.failed의 원인은 여전히 별개·미확정입니다.
- task_exit_code=2는 PowerShell 실행기 실패입니다. 임시작업 REMOVED, 활성permit 비활성화·활성경로 부재 확인, OnBoot/boot승인/장부 보존 모두 실제 parent 결과에서 확인했습니다. 새 Job/사용량 자식이 생성되지 않았으므로 남은 app-server도 없습니다.

## 단발 marker 및 보존

날짜별 launch fuse는 request987a...를 담은 채 그대로 보존했습니다. SHA256 `62a867f542d6800a68f9a626d1c792ace7143081a7ff8e15ce1638b412cab97b` 전후 동일 확인. 실제 API는0이지만 **launch 슬롯이 소비된 상태를 자동 초기화하거나 미소진으로 재발급하지 않습니다.** 새 request/permit/task를 생성하지 않았고 명령을 재실행하지 않았습니다. 이어지는 실제 실행은 대표의 새 명시적 재개 지시 전까지 중지합니다.

배포본/비활성permit/자식summary·부모summary는 원래 위치에 보존했습니다. 운영 boot-approval SHA `3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f` 그대로. 기존 봇/아트/CLI/CODEX_HOME/계정/인증/ACL/OnBoot/SQLite/Unity/게임/Save 변경0. 현재 agent에서 부팅 작업을 재실행/수정하거나 전체44/22+12/Job4 검사를 반복하지 않았습니다.

## 격리 수정·신규 오프라인 회귀

`run-approved-usage-once.ps1`의 위 지역변수 두 개만 수정했습니다. **ProgramData에 배포한 파일은 수정하지 않았습니다.**

| 대상 | SHA256 |
| --- | --- |
| 실제 배포·실패 source(보존) | ea7a24d93315baadfab9a62ef37b5174a00695dfff1405fe595e00b8a43afe5a |
| 격리 수정 후보 source | 3a57e70ac09ac2d357d4224ef4cc9c5ad23d7610f499380436eeb2aac80fec74 |

새 `test_usage_launcher_bindings.ps1`로 **3 PASS / 0 FAIL**: 배포AST의 HOME/Child충돌2개 확인 및 후보0개, HOME읽기전용 메타데이터/순수 switch형변환 오류 재현, 새지역변수가 HOME와Child플래그 값을 보존함 확인. 바깥 예외 포장은 함수/파일 문맥에 따라 RuntimeException 또는 ArgumentTransformationMetadataException이어도 내부 PSInvalidCastException은 같음을 확인했습니다. 초기 fixture의 바깥 예외명 기대가 너무 좁아 수정한 뒤 최종3건 통과했으며, live 작업/API는 실행하지 않았습니다. parser/diff검사 PASS.

회귀는 파서 AST와 가짜 문자열만 사용합니다. 등록/계정/CLI/사용량을 호출하지 않으며 reserved변수나기존script매개변수에 새지역값을 쓰는 변경이 재발하지 않도록 검사합니다. 기존 검증이 성공한 native Job 및 Python entry는 수정하지 않았습니다.

브랜치 codex/discord-usage-diagnostics-20261026. 선행e6b24a0b. 이번로컬fix/문서commit은Issue66/최종보고참조. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / LOCAL_FIX_NOT_DEPLOYED**. 다음 실제사용량/배포/marker처리는 별도 지시 전 실행하지 않고 여기서 중지합니다.
