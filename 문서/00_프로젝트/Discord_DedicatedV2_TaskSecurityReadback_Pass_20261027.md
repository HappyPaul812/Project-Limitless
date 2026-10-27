# r2 Task 보안 descriptor 읽기 검증 PASS — 2026-10-27

판정 TASK_DESCRIPTOR_READONLY_PASS_EFFECTIVE_ACCESS_HOLD.
대표 직접 관리자 수집 결과 Admin-acd610e6e04348c88b881e8c0f17fe33.json을 로컬 실제 경로에서 판독했습니다. 전달 경로의 00_프로젝 트 공백은 실제 00_프로젝트와 대조했습니다. 원본은 변경하지 않았습니다.
수집 시각 2026-10-27T11:53:17.6429857Z. SHA256:e22bb23bdce8e6780faeaee70a2a400266d0f3ad21dc639e86f8f58de9ff0fae

## 확인 결과

- 정확 Task \LIMITLESS-DedicatedV2-20261027-Host, Worker Principal SID S-1-5-21-2858504392-1958168942-1662906075-1010 일치.
- Disabled/state_disabled/Trigger0/Action/policy_settings 조건 true. 도구의 정책조건은 PASSWORD1/RunLevel0/GroupId비움/Restart0/MultipleInstances2/HardTerminate/90초를 포함합니다.
- 실제 SDDL O:BAG:SYD:PAI(A;;FA;;;SY)(A;;FA;;;BA)(A;;FR;;;S-1-5-21-2858504392-1958168942-1662906075-1010).
- OwnerBA/GroupSYSTEM/ProtectedDACL/ACE3·순서/명시allow/Worker 읽기만의 Task 전용 계약 통과. raw/normalized Worker mask 1179785(0x120089). GR↔FR 비교에 따른 descriptor 정책 HOLD는 이번 관측 범위에서 해소됐습니다.
- Task XML SHA 4ef912d925510838b18765f6b18204c7da83de4b5d0b182381eaf36dde89a47b가 이전 기준과 일치.
- source_checks verifiedtrue: Manifest7c841014…/Bindingd698ae90…/Host063c0632… SHA 및 기존 파일 ACL/바인딩 확인, Worker1010 Enabled.
- read_attempts2/자동재시도0, 도구의 시작·종료 Task/파일 지문 비교가 통과해 STOP_NO_IPC 도달. 최종 raw 두 관측값 전체가 JSON에 별도 저장된 것은 아니므로 독립 원자적 snapshot 보장으로 표현하지 않습니다.
- changes/install_calls/ipc_calls/model_calls 모두0.

후보 class 자체 Status는 기존 OFFLINE_CANDIDATE_PASS_NOT_WINDOWS_REVERIFIED 및 LiveVerified=false/RuntimeAdmitted=false를 유지합니다. 현재 COM 수집 descriptor를 후보 계약과 대조한 wrapper의 descriptor_readback_status=PASS와 구분합니다. 과거 후보 결과를 수정하거나 실제 Token PASS로 바꾸지 않았습니다.

## 여전히 HOLD인 범위

effective_worker_access=UNKNOWN_TOKEN_NOT_CHECKED, runtime_admitted=false, runtime_status=HOLD_NEW_TOKEN_GROUP_EXCLUSIVITY_NOT_VERIFIED.
직접 Worker ACE에 비인가 쓰기/실행/삭제/WRITE_DAC/WRITE_OWNER·예상밖SID가 없다는 descriptor 검증이며, 실제 유효토큰·간접그룹·특권·비협조 실행기·GROUP Task 영향·Session0·Job 정리/독점 검증은 아닙니다.
원래 r2 설치 승인 consumed/원Install HOLD·부분자산·r1/r2 Stage/Entry/Result/Lease 기록은 그대로 보존합니다. 원설치 결과를 사후 성공으로 덮어쓰거나 승인·소비를 초기화하지 않습니다.
V1 REFERENCE_VERIFICATION_HOLD 및 MANUAL/A HOLD 유지. Bot 계정·권리/OnBoot/기존11프로세스/855파일 전수조사는 이번 최소조회에서 반복하지 않았습니다. 과거 검증을 현재 전체 감사 PASS로 확대하지 않습니다.

## 중지와 후속 조건

추가 관리자조회/재설치/Stage/Resume/계정TaskACL변경·암호변경·Task실행·새승인·IPC/FakeWorker/AI/Discord/운영·Unity/Save변경을 하지 않았습니다.
실제 유효토큰/GROUP/Session0/Job·실행 독점성의 다음 단계는 별도 승인과 검증 계획이 필요합니다. 이번 descriptor PASS는 IPC 실행 승인이 아닙니다.
이 결과 판독·Issue/상태/인계 문서화 후 중지합니다. 새 테스트/검사 반복0.
브랜치 codex/discord-usage-diagnostics-20261026, 기준 d906d9206b5a3df86aa5c12e439fb7adfedbf4fd. 최종 로컬 커밋은 Issue 후속, LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
