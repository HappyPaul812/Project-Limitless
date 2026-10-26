# Issue66 원자 설치 실제1회 — 부분 설치 HOLD

대표가 로컬 암호 입력 후 PowerShell 창이 닫혔다고 보고하여 기존 결과 JSON만 읽었습니다. 재설치·권한 수정·자동 정리·Worker 실행은 하지 않았습니다.

- 요청 `0f8e15c951a144efa79f5ccdbeed9c55`, 실제 관리자 Session9, Install 시도1/자동재시도0.
- 승인 Installer/Manifest/ZIP SHA 및855개/854보존 PASS.
- `APPROVED_INSTALL_ONCE`에서 `INSTALL_HOLD_NEW_TRIAL_ONLY_NO_RETRY_CHECK_PARTIAL_INSTALL`. RuntimeException/HRESULT -2146233087.
- 설치 코드 폴더 존재, 상태 폴더 미생성. 신규 Bot SID `S-1-5-21-2858504392-1958168942-1662906075-1008`, Enabled=false(승인 설치기의 실패 catch에서 비활성화).
- task 조회 결과 `UNKNOWN_OR_ABSENT`이므로 실제 부재로 단정하지 않습니다. 실제 task owner/DACL·설치 전체 ACL/권한/Users/24시간 만료·SID3개 완전 검증은 미완료. 실제 Session0/logon4 실행 관찰0.
- OnBoot2개 XML SHA 전후 동일, boot approval SHA 동일. Host/IPC/FakeWorker/AI/usage/Discord/SQLite 작업0. MANUAL/A HOLD 유지.

## 창 종료와 설치 실패는 구분

보조 명령 마지막 `if($result.status -eq 'HOLD'){exit 2}`를 메모리 ScriptBlock으로 현재 대화형 PowerShell에서 실행하도록 안내했습니다. 따라서 HOLD 후 exit2가 현재 PowerShell 세션까지 종료하는 안내/실행 구조 문제였습니다. 별도 자식 프로세스에서 실행하거나 대화형 호출에서는 return으로 끝내야 합니다. **현재 승인 파일/보조 스크립트를 수정하거나 재실행하지 않았습니다.**

설치 실패의 직접 원인은 미확정입니다. 승인 설치기 catch가 원래 예외를 고정 HOLD 문자열로 대체하여 현재 JSON에 구체적 원인이 없습니다. 기록상 신규 계정 생성 이후·ProgramData 상태 폴더 생성 이전 범위지만 특정 함수 실패로 단정할 수 없습니다. 창 종료가 암호 오류 때문이라고 결론내리지 않습니다.

## 보호와 다음 절차

기존 운영 파일/부팅/봇/인증/Unity/Save/Push/main/PC에 변경 작업0. 원본 후보 SHA 변경0. 새 계정은 비활성화 상태로 보존하고 부분 설치 폴더·결과 증거도 남깁니다. 자동재등록/계정 재생성/삭제/ACL 완화 금지.

다음은 관리자 **읽기 전용 부분 상태 진단**(새 계정 그룹·직접 권한·파일/Manifest/record 존재·소유권·task 실제 존재/SD)과 기존 오류 증거 확인입니다. 신규 설치나 IPC 승인을 이 진단으로 확대하지 않습니다. 성공 DeploymentRecord가 없을 수 있으므로 기존 Remove를 무조건 실행하지 않습니다. 자기 신규 자원임을 확인한 뒤에만 별도 복구 범위에서 정리합니다.

증거: `팀운영/Evidence/Session0AtomicInstall20261026/0f8e15c951a144efa79f5ccdbeed9c55.json`.
브랜치 `codex/discord-usage-diagnostics-20261026`, 설치 실행 준비 commit `30cc720d59396409b76ee4642ec56d16fdb79816`. 실제 보고 commit은 Issue66 참조.

**설치 PASS 아님. HOLD로 중지하며 이전 설치 명령을 다시 실행하지 않습니다.**
