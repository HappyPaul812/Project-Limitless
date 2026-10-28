# Discord V2 FAKE_ONLY 전용 독점 계약 구현 — 신뢰 근거 HOLD

2026-10-28 / 기준 Issue #66·aa3a4fac34b406258c57881a0b3cec3f1d0b39c2 및 바울 대표의 한정된 계약 변경 승인. NO_GO / MANUAL / HOLD, 2026-11-01 마감 유지.

## 실제 구현

- 기존 운영/기본 계약의 전역 조건과 HandleCensus.Exclusive는 변경하지 않았습니다. TrialContract에 선택적 FakeOnlyIsolation 확장만 추가했습니다. 미지정 시 직렬화에 필드도 추가하지 않으며 기존 전역 검사 경로를 유지합니다. 기존 동작 승인·소비 기록을 전용하지 않습니다.
- 별도 프로필 FAKE_ONLY_EXECUTION_V1은 GlobalSidAbsence=UNKNOWN만 허용합니다. Profile/신뢰 자료 SHA·strict JSON 검사를 적용하고, FAKE_ONLY/noIPC/noAI/90초/Supervisor1/Probe1만 허용합니다. 전역 PASS 표기·LIVE·IPC/AI·프로세스 수 확대·중복/불명 필드를 거부합니다.
- FakeOnlyPolicy는 관리자 소유 보호 파일로 읽은 신뢰 진술·검토 원문의 SHA, 정확 trial/계정/Task/보호자산/두 이미지/관측·만료를 결합합니다. 비협조 동일SID 코드 없음·자격증명 비공유·다른 실행기 미사용·위험 수용을 명시적으로 요구합니다. 해시는 자료 결합과 무결성만 확인하며 사실성을 새로 증명하지 않습니다. 관리자의 실제 검토/진술이 신뢰 가정입니다.
- 관리자 CLI controller는 근거 누락 시 등록/시작·승인 소비 이전에 HOLD합니다. 실제 Windows 어댑터는 기존 보호 경로·OWNER/DACL/최종경로 검사로 자료를 읽습니다. Supervisor는 동일 정책을 생성/영구 소비 전에 검사하고, 보호 파일 HANDLE을 계속 pin하여 재검사 때 HANDLE을 늘리지 않습니다. 관측은15분 이내·만료는 계약 이내여야 합니다. cleanup은 기존 승인 만료 자체로 안전 정리를 불가능하게 하지 않되 결합/신뢰 근거·정확 instance·Task idle·정리 증거 검사를 유지합니다.
- 전용 프로필에서만 전역 부재 대신 위 신뢰 근거를 요구합니다. Job·child restriction/no-breakaway·HANDLE census/외부 Job zero-rights 금지·파일 pin/OWNER RIGHTS·CREATE_NEW/영구 소비·기존 lease·실패/취소 정리는 유지합니다. 감독 증거에 GLOBAL_SID_ABSENCE_UNKNOWN을 별도 영구 기록하도록 연결했습니다. PID 예외·전역 UNKNOWN→PASS 없음.
- 기존 일반 진입 게이트 ReleaseAdmissionImplemented=false 유지, 이번 실행 승인 발급0. 새 계약 SHA로 동작 승인을 새로 결합해야 하므로 기존 운영 승인으로 새 FAKE_ONLY 프로필을 시작할 수 없습니다.

## 검증과 보장 한계

새 계약 차이만 **17 PASS / 0 FAIL**. 기존 운영 경로 보존, scoped 근거 성공/누락/변조·binding·만료·가정 불충족·전역 PASS 거부, 관리자 모의 등록/시작·근거 유실 시 비소비, 구승인 재사용 거부를 검증했습니다. 기존 시험 프로젝트의 별도 선택 진입점이며 새 대규모 프레임워크가 아닙니다. 이17개는 순수 정책/모의 backend 검증이며 실제 Task 등록·실행 성공이 아닙니다. 관리형 시험 및 실제 binding Supervisor 일반 컴파일 각각 경고0/오류0. 기존52 및 Windows7 PASS 재실행0, PID4·계정·Task 현재 조회0.

이전 실제7 PASS는 해당 관리자 fixture의 근거로 재사용합니다. 전역 동일SID 부재는 계속 UNKNOWN입니다. Job은 그 소속 대상, lease는 동일 보호 승인 경로 참여자, Task 중복 제한은 해당 Task에만 적용합니다. 비협조 동일SID 공존·증거 선점/위조·가용성 방해와 시간 간격 사이 간섭은 동등 수준으로 해결된 것이 아닙니다. 기존 운영 계약에 대한 보안 수준 축소는 없습니다.

## NativeAOT 판정과 남은 입력

**Probe/Supervisor NativeAOT 각각0회, 새 native 실행 파일·SHA·크기·아키텍처 산출물 없음.** 변경 코드의17 PASS만으로 실제 신뢰 가정을 확인했다고 하지 않습니다.

확보한 자료는 관리자의 다른SID381개 관측·관측 불가 token1개와 이번 한정된 위험 수용 승인입니다. 현재 작업 자료에는 시험 계정의 비협조 코드 부재/자격증명 비공유/다른 실행 경로 미사용과 보호 자산의 실제 관리 검토를 확인한 근거가 없습니다. 이번 승인 문구는 이 가정을 명시하고 근거 부족 시 HOLD하라고 요구합니다. 따라서 사실 근거를 임의 true로 발급하지 않고 빌드의 필수 전제 미충족으로 발행을 보류합니다.

이는 아직 없는 새 이미지 SHA를 발행 전부터 요구하는 순환 조건이 아닙니다. 먼저 계정·보호 자산 사용에 대한 실제 검토 원문을 확보해야 하며, 신뢰 파일에 최종 이미지 SHA를 결합하고 보호 경로에 봉인하는 단계는 이미지 생성 후 실제 시험 승인 발급 때 완성합니다. 이번에는 실행 불가/Approved=false 로컬 예시와 확인 항목만 보존했습니다. 범위 축소·잔여 위험 수용 승인은 이미 받았으므로 재승인을 요구하지 않습니다. 남은 것은 그 신뢰 가정의 구체적 사실 확인 자료입니다. 계정 재조회·PID4 재조회·설정 변경을 요구하지 않습니다.

근거가 확보되면 빌드 적격성을 판정하고 기존 E 공식 패키지·도구로 각1회 발행할 수 있습니다. 이번 E 폴더 생성·Restore/Publish·다운로드/설치0, 기존 NuGet7 재검증0. 생성한 managed DLL은 native 산출물로 보고하지 않습니다.

실제 단발 시험은 별도 승인: 신규 배포 보호 자산/신뢰 원문 및 최종 이미지SHA·계약SHA 봉인, 정확 등록/시작/중지/정리 동작별 신규 승인, 실제 Batch4/Session0·제한 token의 HANDLE 지원성·Job/90초 종료 및 실패 정리 확인 범위가 필요합니다. 현재 그 OS 변경/등록/시작/Session0 승인은 없으며 gate도 열지 않았습니다.

## 변경 파일 / 중지

실제 소스: Tools/DiscordV2/NativeBatch/Supervisor/FakeOnlyPolicy.cs, AdmissionPolicy.cs, Program.cs, ReleaseAdmission.cs, AdminController.cs, AdminWindows.cs 및 공통 프로젝트 연결. 최소 시험: 기존 Tests/Program.cs의 선택 경로·FakeOnlyContractTests.cs 및 프로젝트 연결. 상세 보호 위치·형식 예시·확인 항목은 Git 제외 로컬 검토 문서에만 보관합니다.

기존 계정·ACL·Task·OnBoot·운영 프로세스·Unity·Save·기존 승인 소비 기록 변경0. Session0/실제 IPC·AI/Discord 업무 실행0, Push/main 병합0. 브랜치 codex/discord-usage-diagnostics-20261026, 마지막관련aa3a4fac34b406258c57881a0b3cec3f1d0b39c2; 이번 최종SHA는 완료보고/Issue66. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 근거 부족 HOLD 상태로 중지하며 11-01 마감 이후 자동 연장·재시도 없이 증거/인계를 보존하고 게임 개발로 복귀합니다.