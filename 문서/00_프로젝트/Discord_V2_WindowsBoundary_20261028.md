# Discord V2 실제 Windows 보안 경계 — 부분 PASS / NO_GO

2026-10-28. 기준 Issue #66 / d6d8c007deb754c2685f2a9542c0f0a76612cb40. NO_GO / MANUAL / HOLD, 2026-11-01 마감 유지.

## 실제 관측

현재 실행 주체는 Windows x64, 비관리자 AppContainer, 비 Session 0이며 대상 배치 계정이 아닙니다. 기존 합성52 PASS는 이전 근거로만 보존했고 이번 재실행은 0회입니다. 기존 테스트 프로젝트에 live 선택 경로를 추가했으며 별도 프레임워크/아키텍처를 만들지 않았습니다. 최초 관측 후 결함에 관련된 항목만 새 격리 자원에서 확인했습니다.

| 항목 | 실제 결과 | 근거/한계 |
|---|---|---|
| 파일 공유 잠금 | PASS | 새 파일 pin 중 쓰기/삭제 열기 ERROR_SHARING_VIOLATION, 해제 후 열기 성공 |
| 제한 토큰 접근 거부 | PASS | 현재 token 제한 clone의 새 파일 읽기 AccessCheck 거부 |
| 빈 Job x64 ABI·DACL | PASS | 실제 QueryLimits/Accounting, active=total=0; 객체별 권리 매핑 후 정확 DACL readback |
| 새 token/thread/process DACL | PASS | 새 객체 3종 보호·readback 성공, 준비 handshake 후 시험 자식 정상 종료0 |
| OWNER RIGHTS 비교 | FAIL | OW 없는 파일의 WRITE_DAC: AccessCheck 거부/커널 열기 허용. OW deny 파일은 양쪽 거부. 현재 token의 불일치 원인 미확정 |
| 실제 전체 HANDLE/외부 독점 | UNKNOWN/HOLD | NtQuerySystemInformation class64: 0xc0000022. 외부 시험 handle 전송 단계 미진입; 실제 전체 HANDLE ABI/독점 PASS 주장 불가 |
| 전체 process 독점 | UNKNOWN/HOLD | 필터링 snapshot 4개/PID4 미노출, 직접 읽기 open 오류5. AppContainer 전체성 오판을 수정하여 안전 차단 확인 |
| Task Scheduler COM 읽기 | UNKNOWN/HOLD | HRESULT 0x80070005. 기존 Task 등록/Disabled/Trigger 현재 readback 비교 미완료 |

위 표는 관련 수정 후 마지막 관측을 합산한 항목별 결과입니다. 단일 전체8항목 PASS 실행 결과가 아닙니다. 최초/수정 후/부분 재확인 JSON을 모두 로컬 보존했습니다. 제한 토큰 AccessCheck 통과는 대상 SID 프로세스나 PPL 관측 성공을 의미하지 않습니다.

## 실제 결함 최소 수정

- GetSecurityInfo에 GROUP 포함(5→7). 실제 AccessCheck ERROR_INVALID_SECURITY_DESCR(1338) 해소.
- 커널 DACL 비교 시 SDK의 객체별 GENERIC_ALL 확장을 적용. 실제 Job/Token/Thread/Process DACL과 정확 비교 유지. ACE 삭제/권리 확대/검사 생략 없음.
- AppContainer의 필터링된 프로세스 snapshot을 전체 목록으로 인증하지 않고 HOLD. 기존 접근 불가 process 검사도 유지.
- 시험 자식은 초기화 완료 파일을 확인한 후 DACL을 적용하도록 fixture 순서 수정. 운영 코드 권한 완화 없음.

관리형 실제 binding 시험 프로젝트 및 Supervisor 일반 컴파일 각각 경고0/오류0. SDK x64 C++ ABI/권리 mask 구문 컴파일 성공. 기존 합성52 미실행. 실행 게이트 false 유지.

AccessCheck의 Owner/Group 요구는 [Microsoft AccessCheck 문서](https://learn.microsoft.com/en-us/windows/win32/api/securitybaseapi/nf-securitybaseapi-accesscheck), 객체별 generic 권리 해석은 [Microsoft Generic Access Rights](https://learn.microsoft.com/en-us/windows/win32/secauthz/generic-access-rights) 및 설치된 Windows SDK 정의를 근거로 했습니다. OWNER RIGHTS 잔여 불일치는 이 근거만으로 해결됐다고 판단하지 않습니다.

## NativeAOT 및 다음 승인

필수 실제 보안 경계가 통과하지 않았으므로 Probe/Supervisor NativeAOT 발행 **각0회**, 새 실행 이미지/그 SHA 없음. 생성 native 프로그램 실행0, Restore/다운로드/설치0. 기존 공식 NuGet7 보존. 설치 메타데이터의 VS 표시 버전은18.11.2(빌드18.11.12324.101)로 지시의18.11.1과 다르며 이번 변경/업데이트 없음.

관리자 직접 수행용 PowerShell7 x64 절차는 Git 제외 로컬 문서에만 보관했습니다. 스크립트·관리형 시험 DLL/deps/runtimeconfig·입력 SHA를 고정하고 새 E 격리 폴더만 사용합니다. 관리자 직접 검증에 대한 별도 승인 후 실행하며 현재는 미실행입니다. 관리자 실행도 UNKNOWN이면 권한 변경/반복 없이 증거 보존 후 HOLD합니다. 이후 OWNER RIGHTS 비교 및 전체 HANDLE/제한 process/COM 관측 해소가 필요합니다. 실제 배치 계정·Session0·Job90초 종료·등록/실행/정리 시험은 별도 승인 대상으로 남습니다. 이번 빈 Job 검증은90초 실실행 검증이 아닙니다.

운영 계정/Task/OnBoot/서비스/기존 ACL/Unity/Save 변경0. 새 격리 파일·빈 객체·시험 자식에만 보안 서술자를 적용했고 반환된 자체 HANDLE을 닫았습니다. 시험 파일은 원래 증거와 함께 보존, 기존 승인 소비 기록 미사용/미초기화. 실제 업무 IPC/AI/Discord/Session0 실행0. 로컬 브랜치만 커밋하며 Push/main 병합0.

브랜치 codex/discord-usage-diagnostics-20261026. 마지막 관련 기준 d6d8c007deb754c2685f2a9542c0f0a76612cb40, 이번 최종 SHA는 완료 보고와 Issue #66에 기록합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.