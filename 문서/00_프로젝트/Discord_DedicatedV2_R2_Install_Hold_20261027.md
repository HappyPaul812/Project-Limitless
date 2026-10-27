# r2 설치 결과 HOLD — 2026-10-27

판정: PARTIAL_INSTALL_HOLD / NO_RETRY. 대표 제공 JSON 기준이며 실제 관리자 원본 파일 경로는 제공되지 않았습니다.

- install_calls=1, approval_created=true, consumed=true, account_attempts=2, task_attempts=1.
- Bot SID S-1-5-21-2858504392-1958168942-1662906075-1009.
- Worker SID S-1-5-21-2858504392-1958168942-1662906075-1010.
- worker_install_state=ENABLED_BATCH_ONLY는 설치기 보고값입니다. 최종 SID/그룹/권리/활성 상태 감사는 미완료입니다.
- native stage CREATE_ONLY_DISABLED_TASK, native status HOLD.
- native errors MethodInvocationException HRESULT -2146233087, InvalidOperationException HRESULT -2146233079. 외부 RuntimeException -2146233087.
- existing_preserved=true는 설치기의 결과값이며 최종 FINAL_READBACK과 설치 후 audit는 실행 완료되지 않았습니다.
- ipc_calls=0, model_calls=0, auth_imports=0, automatic_retries=0. partial_assets_preserve_no_retry=true.

코드 판독: 해당 단계는 실제 Task 메서드 호출뿐 아니라 ExactSddl 및 readback 조건 검사까지 포함합니다. 따라서 등록 자체 실패, 등록 후 SDDL 불일치 등 어느 조건이 직접 원인인지 JSON만으로 확정할 수 없습니다. HRESULT만으로 암호 오류라고 판정하지 않습니다. Task가 없다고도 단정하지 않습니다.

Codex 읽기 전용 제한 조사: 고정 Manifest에서 정확한 Task 이름 LIMITLESS-DedicatedV2-20261027-Host를 얻어 COM GetTask만 호출했습니다. UnauthorizedAccessException -2147024891로 Task 존재/보안을 확인하지 못했습니다. Get-LocalUser 개별 조회도 NullReferenceException -2147467261로 UNKNOWN입니다. 실패를 ABSENT 또는 PASS로 바꾸지 않았습니다. 결과 CodexReadonly.json 보존. OS 변경/설치/Task 시작/권한 변경0.

Code/State/Binding은 Task 단계 이전 함수가 반환됐다는 코드 흐름상 부분 생성 가능성이 있습니다. 실제 파일 목록/전체 SHA/ACL 및 Bot Disabled 여부는 관리자 읽기 전용 추가 감사가 필요합니다. 보호 승인 부모의 Entry-3effaac30bf44760986fee9e6019b283.json 및 해당 r2 소비 디렉터리 Result JSON을 보존하고, 실제 계정2/Task/파일/승인·소비 상태를 조회해야 합니다. 오류 위치 구분은 등록 재실행 없이 기존 보호 결과 및 읽기 전용 상태로 조사해야 합니다.

복구 조건: 동일 Install/Stage/Resume 명령 재실행 금지. 만료 갱신·승인 재발급·소비 삭제·부분 자산 삭제·비밀번호 재설정·Task 수정/등록/시작을 수행하지 않습니다. 정확한 부분 자산과 원인 확인 후 별도 승인된 복구 계획이 필요합니다. 현재 설치 PASS 또는 사용 가능 상태가 아닙니다.

V1 REFERENCE_VERIFICATION_HOLD, MANUAL/A HOLD, 새 토큰/GROUP/Session0 IPC/Job 독점 미검증 HOLD 유지. 기존 Worker1007/11프로세스/OnBoot/봇/아트/인증/Unity/Save 변경 없음. Push 없음.
브랜치 codex/discord-usage-diagnostics-20261026, 설치 준비 커밋 bc1f27019df902c19fd00b68c246290971213ddb. 결과 기록 최종 로컬 커밋은 Issue 후속 참조, REMOTE_NOT_PUSHED.
