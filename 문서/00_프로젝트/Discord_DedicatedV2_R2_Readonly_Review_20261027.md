# r2 관리자 읽기 진단 결과 판독 — 2026-10-27

판정 HOLD_READONLY_REVIEW_COMPLETE_NO_RECOVERY. 설치/Stage/Resume/새 승인/권한 수정/Task 실행 없이 판독 후 중지합니다.
브랜치 codex/discord-usage-diagnostics-20261026, 기준 34d86bc637950cd6e82ee158cf8d895e62483654. 최종 로컬 커밋은 Issue 후속 보고 / REMOTE_NOT_PUSHED.

## 증거

대표의 관리자 v2 수집 JSON Admin-33a67728035f4fe88db7f539e5b648dc.json(팀운영/Evidence/DedicatedV2R2Readonly20261027).
전달 문자열의 00_프로젝 트 공백은 실제 경로의 00_프로젝트와 대조했습니다. 원본 JSON 내부 경로도 공백 없는 실제 경로입니다. 원본 파일을 수정하지 않았습니다.
수집시각 2026-10-27T11:15:47.8969903Z, 원본 SHA256 4de232b3d5af7aa6662ce4e4ec6a4d28ddc529b5c0d50656a9114f71e679140e.
일반 query45 중 READ_OK44 / UNKNOWN1, 별도 설치 payload766건. READ_OK는 조회 성공이며 보안 조건 전체 PASS라는 의미가 아닙니다.
원본 Entry/Result는 관리자 도구가 정제한 내용과 metadata/SHA를 판독했습니다. Codex가 보호 원본을 독립 재열람했다고 주장하지 않습니다.

## 1. 실패 원인 근거와 한계

Task는 PRESENT이며 정확 경로는 \LIMITLESS-DedicatedV2-20261027-Host입니다. GetTask와 숨김 포함 GetTasks(1)에서 일치했습니다.
실제 Task SDDL:
O:BAG:SYD:PAI(A;;FA;;;SY)(A;;FA;;;BA)(A;;FR;;;S-1-5-21-2858504392-1958168942-1662906075-1010)

고정 r2 계약은 Worker ACE를 GR로 요청하고, ExactSddl에서 AccessMask 숫자를 그대로 비교합니다.
기대 GR=0x80000000, 실제 FR=0x00120089. Owner=BA/Group=SYSTEM/보호DACL/header는 일치하나 권리 마스크 비교는 불일치입니다. 실제 ACL의 Worker 쓰기/실행 권한을 추가한 증거는 없습니다. 다만 generic 권리를 실제 객체 권리로 매핑한 의미의 안전성을 계약별로 검토해야 합니다.

고정 r2 ZIP(기준 SHA 일치)에서 Manifest에 SHA로 고정된 DedicatedContract.cs만 읽어 메모리에 컴파일했습니다. OS/설치/API adapter는 실행하지 않았습니다.
관측 SDDL을 원래 ExactSddl(actual, TaskSddl(worker))에 넣으면 D3_SD_RIGHTS, MethodInvocationException -2146233087 → InvalidOperationException -2146233079로 실패합니다. 과거 설치 JSON의 예외 체인과 같습니다. 기대 SDDL 자체는 통과했습니다. 신규 오프라인 재현 2 PASS / 0 FAIL.
따라서 '등록 후 SDDL 숫자 비교가 관측된 Windows 표현을 거부한다'는 코드 결함을 재현했습니다. Task 등록 자체 실패 또는 암호 오류로 분류할 근거는 없습니다.
단, 원본 오류에는 throw 태그가 없고 등록 이벤트도 없어 과거 실행이 정확히 이 지점에서 던졌다는 역사적 추적 증명은 남아 있습니다. 현재 재현 원인과 과거 원인 확정의 한계를 구분합니다.

TaskScheduler/Operational은 Disabled이며 record_count=null, 정확 작업 이벤트 조회 UNKNOWN/System.Exception -2146233088입니다. 로그를 활성화하지 않았습니다. 실제 등록시각은 UNKNOWN입니다. RegistrationInfo.Date=null, LastRunTime/LastTaskResult만으로 실제 Session0 실행 성공을 주장하지 않습니다.

## 2. 계정과 Task 실제 상태

- Bot LIMITLESSBotV2D, SID S-1-5-21-2858504392-1958168942-1662906075-1009, Disabled.
- Worker LIMITLESSWorkV2D, SID S-1-5-21-2858504392-1958168942-1662906075-1010, Enabled.
- Get-LocalUser/NetUserGetInfo4 결과 일치. 서로/관리자/기존1007·1008 SID와 분리.
- 두 계정 모두 Users(S-1-5-32-545)만 로컬 그룹으로 보고됐으며 privilege_level1. 유효 토큰을 만든 검사는 아닙니다.
- 두 계정 직접 권리는 SeBatchLogonRight 및 DenyInteractive/DenyRemoteInteractive/DenyNetwork/DenyService의5개입니다. Bot에도 Batch 권리가 설정된 것은 고정 r2 공통 Rights 계약에 따른 관측값이며 이번에 수정하지 않았습니다.
- 잠금/암호만료false, 로그온시간·워크스테이션 제한false. 계정 만료는 2026-10-28T10:36:51Z(한국시간19:36:51), 갱신하지 않았습니다.
- Task Principal LIMITLESSWorkV2D, LogonType1(PASSWORD), RunLevel0.
- Disabled, Trigger0, Restart0, MultipleInstances2, 90초. Action 경로/WorkingDirectory/인수 비교true. 인수 원문은 저장하지 않았습니다.
- Host 파일은 payload SHA 기준과 일치. pwsh.exe SHA는 관측했으나 패키지에 공식 pin이 없어 baseline 검증 PASS로 확대하지 않습니다.

## 3. 부분 설치 파일과 기록

payload766 모두 SHA 일치/조회UNKNOWN0, 고정 Code DACL 비교불일치0, 링크수/ADS 이상0.
Manifest SHA 7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976 일치.
총 파일768(payload766+Manifest+Binding), 누락0/예상외0.
Binding SHA d698ae90f0909a9363b04dea67c186f95bfa5f7f2f5d937adf3ae7a23aa011c9 관측. 바인딩 내용 전체 검증과 모든 런타임 승인 조건 완료 주장은 하지 않습니다.
Code/State/Auth/CodexHome/Control은 PRESENT, 관리자 Owner·SYSTEM Group·보호 DACL. Code는 Bot/Worker 읽기·실행, State/Auth 상위는 Worker 읽기·실행, CodexHome은 Worker 상태 쓰기 범위로 관측됐습니다. 기존 코드/인증 복사는 없습니다.
CodexHome entry0, worker.lease 길이0. 빈 파일만으로 lease 잠금/Job 독점 해제를 증명하지 않습니다. Job 객체는 미열거입니다.
신규 승인·소비 marker·install.lease·Entry·Result 존재. consumed=true, 기존 승인 재사용 금지.
Entry SHA 5a146be183e82e291ef0a887503619a7fa4c3c2ad23a9f6830cf6d83be580689.
Result SHA 3c514a23ee9c953e13271d0364b8db92cdca78a31d8193656308578cffe5e8dd.
만료 r1 승인 SHA 845f3f7818e0302cb5ac0cec43822860758e184b541426683f94bc8a7719b4e8 및 보호 metadata 관측. 기록을 수정/삭제/재사용하지 않았습니다.

## 4. 기존 운영 보호

기존 Worker1007 Enabled/V1시험1008 Disabled 확인. OnBoot Art/Bot2개 XML 기준 SHA 일치/Enabled. V1 Code root와 r1/r2 Stage root 보호 metadata 존재 확인.
existing_preserved=true는 native 보고값입니다. 이번 독립 관측은 위 SID/계정상태/OnBoot2와 보존 경로 metadata에 한합니다. 기존855파일 전수검사·11프로세스 전수조사·운영 SQLite/인증/게임 전체 감사는 반복하지 않았으므로 독립 전체 보존 PASS라고 하지 않습니다.
진단 changes/install/IPC/model/retry0. Codex 추가 OS 변경/계정Task조작0.

## 5. 정확한 수정 범위 및 최소 복구안 (미승인·미실행)

후속 수정 대상은 DedicatedContract.cs의 TaskSddl와 Task 전용 readback 비교 계약, 같은 계약을 쓰는 installer/core/postaudit 및 가짜 Scheduler의 generic→specific readback 표현입니다. 전역 ExactSddl을 무조건 느슨하게 하거나 보안 검사를 생략하면 안 됩니다.
객체에 맞는 권리 매핑을 명시해 GR/FR 표현을 비교하되 Owner/Group/보호DACL/ACE 대상·종류·flags·개수 검증을 유지하고, Worker 쓰기·실행·DELETE·WRITE_DAC·WRITE_OWNER 추가는 계속 거부해야 합니다. 기존 설치 파일·Task ACL을 바꾸는 것이 선행 조건이라는 증거는 없습니다.
고정 r2/Stage는 그대로 보존하고 수정 후보·SHA·승인 계약을 별도로 검토해야 합니다.
현재 존재 계정2/Task1/CodeState를 다시 생성하는 Install 재실행은 금지합니다. 소비를 지우거나 새 승인을 임의 발급하지 않습니다.
별도 복구 승인 시 먼저 관측 상태/바인딩/보존 기준을 재검증하고, 필요한 변경만 대상으로 한 단발 복구 설계를 검토합니다. 현재 승인 범위에는 구현·복구·설치가 포함되지 않으므로 진단 결과와 설계 방향만 기록하고 중지합니다.
운영 이벤트 수집은 로그가 꺼져 있어 과거 등록시각 복원이 어려울 수 있습니다. 로그 활성화도 별도 변경이므로 수행하지 않습니다. 현 증거 분석을 위해 같은 관리자 전체 진단을 다시 실행할 필요는 없습니다.
새 유효 토큰/GROUP Task 영향/Session0 IPC/Job 정리·독점은 별도 검증/승인 필요. 설치 PASS/런타임 사용 가능/IPC 진행 가능으로 승격하지 않습니다.
V1 REFERENCE_VERIFICATION_HOLD, MANUAL/A/runtime HOLD 유지. 복구·계정/Task/ACL변경·IPC·AI·Discord·Unity/Save·Push0.

공식 근거(표현 차이 해석의 참고이며 이번 Task에 대한 실제 매핑은 관측값으로 구분):
https://learn.microsoft.com/en-us/windows/win32/secauthz/generic-access-rights
https://learn.microsoft.com/en-us/windows/win32/taskschd/security-contexts-for-running-tasks
