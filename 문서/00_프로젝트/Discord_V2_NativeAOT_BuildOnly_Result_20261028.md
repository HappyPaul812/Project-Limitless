# Discord V2 NativeAOT 빌드 전용 결과 — Probe FAIL / Supervisor 미시도

2026-10-28 / Issue #66 / 소스 기준00141490b80c582509271b5f46801ef41a64cc37. 이번 대표 승인으로 컴파일·발행을 실제 시도했습니다. 실행 신뢰 근거 부재를 빌드 차단 조건으로 사용하지 않았습니다. NO_GO / MANUAL / HOLD, 2026-11-01 유지.

| 대상 | NativeAOT Publish | 결과 | 실행파일 SHA256·크기·실아키텍처 |
|---|---|---|---|
| Probe | 1회 / exit1 | FAIL — MSB3021, native Probe.exe를 발행 폴더로 복사할 때 원본 경로 접근 거부 | 산출물 미확보·검증 불가 |
| Supervisor | 0회 | 첫 실패 시 중지 / NOT_ATTEMPTED | 없음 |

자동 재시도0, 생성 이미지 실행0. 두 native 이미지 생성 성공이라고 보고할 수 없습니다. 빌드 실패와 권한/실행 승인 상태를 분리했습니다.

## 확인된 실패와 한계

오프라인 restore 성공(약5.26초), C# 컴파일 후 Generating native code 단계에 도달했습니다. 마지막 publish copy에서 MSB3021/Access denied가 발생했습니다. 읽기 전용 후속 확인에서 중간 native 경로와 최종 publish 경로 모두 exe를 찾지 못했습니다. 파일을 읽어 hash·PE header를 확인할 수 없으므로 native PE32+/AMD64라고 검증했다고 주장하지 않습니다. 빌드 목표 RID는 win-x64입니다.

추가 경고: NativeInterop.cs:74의 Native.Info<T>가 Marshal.PtrToStructure<T>의 constructor 보존 annotation 요구를 충족하지 못한 IL2091. 컴파일/트리밍 단계에 같은 경고가 기록됐습니다. copy 실패의 직접 원인과는 구분하며 이번 소스 수정·경고 억제·재시도는 하지 않았습니다.

현재 직접 확인된 차단은 **복사 원본 경로 접근 거부**입니다. 이후 파일 미발견의 세부 원인은 미확정입니다. 관련 Windows 보안 이벤트 읽기도 현재 문맥에서 UnauthorizedAccessException으로 거부됐으므로 보안제품 삭제/격리·오탐이라고 단정하지 않았습니다. 승격 조회/ACL·보안 설정 변경/격리 복원/제외 등록·우회0. 후속은 대표의 해당 시각 보안/접근 기록 확인과 원인 검토가 먼저이며 새 빌드를 자동 재시도하지 않습니다.

## 격리 공급과 보호

기존 SDK10.0.403/MSVC14.51.36231/Windows SDK10.0.26100.0 사용. SDK가 기본 선택하는10.0.14 대신 기존 공식 공급7종10.0.10을 사용하도록 E 새 빌드 root 안에서만 runtime/ILCompiler/ILLink 고정. MSBuild 항목 평가로 net10.0의 compiler/linker10.0.10을 확인했습니다. project.assets의 복원 공급원은 기존 E 오프라인 feed1개뿐, 네트워크 패키지 다운로드·설치·전역 SDK/설정 변경0. 기존 NuGet 서명 검증 PASS를 재사용했습니다.

기존 두 csproj·관련20개 source 입력을 E 격리 root에 상대 구조 그대로 복사하고 SHA 일치를 확인했습니다. 실제 구현 소스는 변경하지 않았습니다. NuGet packages/http-cache·dotnet-home·temp·obj/bin·발행 출력은 새 E root입니다. 설치 위치·전역 환경변수·기존 cache는 변경하지 않았고 vcvars/process-only 환경만 적용했습니다. SDK 개발 인증서 생성·telemetry·workload 자동 알림을 차단했습니다. 세부 경로·로그·입력 hash·설정은 Git 제외 로컬 보고에 보존합니다.

C 여유28,321,234,944→28,322,959,360 bytes, E 여유909,555,699,712→908,983,468,032 bytes. E 사용량 snapshot 증가572,231,680bytes(약545.7MiB), 배경 작업 포함이라 빌드의 정확 쓰기량이나 C쓰기0을 증명하는 감사 수치는 아닙니다.

준비 스크립트 구문/항목 평가 오류 수정은 실제 Publish 이전입니다. 이때 restore/publish 시도0을 확인했고 실제 Publish marker는 Probe1개만 생성했습니다. 이번 최대1회 제한을 실패 후 재시도로 우회하지 않았습니다.

## 실행 차단과 후속

소스 ReleaseAdmissionImplemented=false 유지·복사 원본 SHA 봉인. 격리 root에 BUILD ONLY/실행 미승인 표식 및 release_gate=false/trust_verified=false/execution_approved=false/global_sid_absence=UNKNOWN 결과 기록. native 파일이 없으므로 binary gate를 실행해 검증했다고 주장하지 않습니다. 기존17·52 및 실제7 PASS 재실행0.

실제 단발 시험 전에는 비협조 동일SID 코드 부재·자격증명 비공유·다른 실행기 미사용/보호 자산 관리의 신뢰 근거, 빌드 오류·IL2091 검토와 두 native 이미지 확보·SHA/아키텍처 봉인, 신규 보호 배포·정확 계약/동작 승인·Task 등록/단발 시작/정리·실제 Batch4/Session0/제한 token HANDLE 지원성·Job90초 실패 정리 범위를 별도로 승인받아야 합니다. 가짜 신뢰 진술·기존 승인 소비 재사용/초기화0. 이번 승인으로 실제 실행은 허용되지 않았습니다.

계정·기존 ACL·Task/OnBoot·운영 프로세스·Unity·Save 변경0, 실제 Session0/Job/IPC/AI/Discord 실행0, Push/main 병합0. 이번은 결과 문서만 로컬 커밋합니다. 브랜치 codex/discord-usage-diagnostics-20261026, 마지막 관련00141490b80c582509271b5f46801ef41a64cc37; 최종SHA는 완료보고/Issue66. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 실패 후 중지하며11-01 마감 이후 자동 연장·재시도 없이 증거와 인계 보존 후 게임 개발 복귀.