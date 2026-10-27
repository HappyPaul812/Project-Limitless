# Issue #66 — 전용 Worker V2 오프라인 후보

2026-10-27. **분리 구조 구현 가능 / 신규 151 PASS·0 FAIL / 실제 Windows 설치·독점성 검증 HOLD**입니다. 기존 운영 Worker의 11개 프로세스는 조사 정본을 재사용했으며 종료·제외·재조사하지 않았습니다. 새 후보는 `Session0DedicatedV2Candidate20261027-r1`, 배포 schema3 / 기존 검증 wire2 결합입니다. r11을 새 계약으로 덮어쓰거나 승인하지 않습니다.

기준 요청 커밋 `d2102bb9eca1d0e0826cbcb6b3ae4f8f35deb3e3`, 실제 작업 시작 HEAD `27b9108966f09c1f965a862ecc7e482eb0dc365d`, 브랜치 `codex/discord-usage-diagnostics-20261026`. 구현·시험 커밋은 후속 인계 항목에 기록합니다. 상태는 LOCAL_COMMITTED / REMOTE_NOT_PUSHED이며 원격 소스 링크가 제공된 것으로 오해하지 않습니다.

## 선택과 비용

| 방안 | 필요한 변경 | 남는 위험 | 판단 |
|---|---|---|---|
| 기존 SID1007 공유 | 모든 직접 실행기·기존 Task4·하위 Job이 공통 coordinator를 강제 사용하도록 변경, 기존 인증/OnBoot 영향 검증 | 비협조 실행기와 기존 역할 UNKNOWN, 동작 중 운영 수정 필요 | 이번 범위에서 독점성 증명 불가, 유지 HOLD |
| 전용 Worker | 새 Worker/Bot 실제 SID, 전용 경로·Task·CODEX_HOME·승인·lease와 새 binding, 배포 부트스트랩 수정 | 같은 PC/구독 한도·자원 경합, 관리자 우회, GROUP 유효 토큰, native 검증 미완료 | 기존 운영 변경을 줄이는 우선 후보, FAKE_ONLY 범위 구현 |

공유안은 코드 몇 줄로 끝나지 않으며 정상 상주 프로세스를 foreign=0으로 취급하지 않습니다. 전용안도 PC 전체/같은 구독 전역 잠금은 제공하지 않습니다. 비협조 실행기를 통제했다는 증거가 없거나 공용 자원 격리가 필요하면 HOLD하고 별도 OS/VM/구독 범위를 검토합니다. 이번에 VM이나 계정은 만들지 않았습니다.

## 별도 자원과 권한 계약

| 역할/자원 | 전용 후보 |
|---|---|
| Bot / Worker | `LIMITLESSBotV2D` / `LIMITLESSWorkV2D` |
| SID | Windows 생성 결과만 InstalledBinding에 기록. 두 신규 SID·관리자 SID 상호 분리, 기존 전체 local SID 및 SID1007/1008 재사용 거부 |
| Code | `C:\Program Files\LimitlessRelayDedicatedV2_20261027` |
| State | `C:\ProgramData\LimitlessRelayDedicatedV2_20261027` |
| CODEX_HOME | State 아래 `Auth\CodexHome`, 새 빈 저장소 |
| 배포 승인 | `C:\ProgramData\LimitlessRelayDedicatedV2Approvals` |
| 보호 staging | `C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\a5af3087b78b45d1baae46067d5b7ac9\Bundle` |
| Task | `LIMITLESS-DedicatedV2-20261027-Host` |
| IPC | `LimitlessDedicatedV2-a5af3087b78b45d1baae46067d5b7ac9` + arm/work/control |
| epoch / 승인 | `20261027-dedicated-a5af3087b78b45d1baae46067d5b7ac9` / `dedicated-install-a5af3087b78b45d1baae46067d5b7ac9` |

Code는 BA/SYSTEM 쓰기, Bot/새 Worker RX입니다. State 상위·Control·Auth는 BA/SY 쓰기·새 Worker RX이며, Data/Temp/Ledger/CodexHome 및 기존 생성 lease 파일만 새 Worker Modify입니다. Bot·기존 Worker에는 Auth 권한을 부여하지 않습니다. private staging·승인·설치 증거는 BA/SY만 씁니다. SECURITY_ATTRIBUTES 생성, 실제 Owner/Group/보호 DACL·SHA·handle/FileID·최종 경로·ADS/hardlink/reparse 검증은 재사용 native 계층과 전용 계약을 조합했습니다. 실제 OS 적용 결과는 미검증입니다.

미래 승인 설치 순서는 두 계정 **Disabled 생성 → 실제 신규 SID 확인 → Users-only/권리 설정 → 보호 파일/빈 인증/binding → 새 Worker만 batch 용도 Enabled → Disabled Task 등록·readback**입니다. Bot은 Disabled 유지합니다. 새 Worker 활성화는 암호 기반 Batch 실행을 위한 별도 명시 승인 필드 `worker_install_state=ENABLED_BATCH_ONLY`에 묶었습니다. 기존 SID는 활성화 경로에 들어갈 수 없습니다. 계정 만료 24시간이며, 재활성화/갱신은 자동 수행하지 않습니다.

권리는 Batch 허용과 interactive/RDP/network/service 로그온 deny를 두 신규 SID에만 부여하는 설계입니다. 이는 네트워크 송신 차단이나 일반 사용자 계정의 완전한 샌드박스라는 뜻이 아닙니다. `SeDenyNetworkLogonRight`는 해당 PC로의 네트워크 로그온을 제한합니다. [Microsoft 정책 문서](https://learn.microsoft.com/en-us/previous-versions/windows/it-pro/windows-10/security/threat-protection/security-policy-settings/deny-access-to-this-computer-from-the-network).

## 실행 경계와 미해결 조건

- 설치 승인과 IPC 승인을 분리하고 schema3·Manifest/Installer/ZIP·epoch·SID·만료를 재바인딩합니다. V1/r11 승인·소비 장부와 혼용하지 않습니다. 설치 승인 소비는 생성 전 CREATE_NEW, 실패해도 소비 보존·재시도0입니다.
- Host는 새 Worker의 실제 Batch4/Session0/비관리자 토큰, 권리·그룹, PID/생성시각, 같은 신규 SID의 외부 프로세스, 직접 Task·Lease를 검사합니다. 기존 Worker의 11개는 새 SID의 자원으로 분류하지 않으며 운영 프로세스의 무충돌을 증명했다고 주장하지 않습니다.
- 실행 가능한 GROUP 작업은 유효 토큰/새 Worker 실행 가능성 증거가 부족하므로 `D3_GROUP_EFFECTIVE_TOKEN_UNKNOWN` HOLD입니다. interactive deny만 보고 GROUP를 일괄 제외하지 않습니다. 이 보수적 분기는 현재 PC의 실행 가능한 GROUP 작업 때문에 실제 Host를 막을 수 있습니다. 다음 단계는 신규 SID에 국한한 유효 토큰·기존 interactive logon 부재·자격증명 사용 범위의 읽기 전용 증거 설계이며 기존 전체 검사를 반복하지 않습니다. Scheduler GROUP4와 Windows Security BATCH4는 다릅니다. [Microsoft TASK_LOGON_TYPE](https://learn.microsoft.com/en-us/windows/win32/api/taskschd/ne-taskschd-task_logon_type).
- IPC 승인에는 자격증명 비공유와 비협조 실행기 통제 증거가 필요합니다. 승인 JSON의 true 자체는 실제 증거가 아닙니다. 증거를 대표가 검토하기 전 승인 파일을 생성하지 않습니다. 관리자/같은 사용자 악성 실행기를 기술적으로 완전히 봉쇄했다는 주장을 하지 않습니다.
- Host의 전체 예산은 프로세스 시작부터 최대90초, Task PT90S·AllowHardTerminate·IgnoreNew·Restart0, Host lease 독점·단발 소비, Job 자식 정리 및 PID 생성시각 fence를 유지합니다. Job 종료는 미래에 이 후보가 만든 자식만 대상으로 합니다. 기존 Worker/다른 Job을 종료하는 경로는 없습니다.
- Task는 Disabled/Trigger0입니다. Python runtime은 단발 승인으로 가짜 Job 결과만 검증하며 실제 AI·usage·Discord 기능을 제공하지 않습니다. 독점 lease는 전용 계정의 협조 경로를 제한하며 다른 PC/앱/구독의 공용 한도는 제한하지 못합니다.

## 구현 파일과 패키지

신규 소스만 `Tools/DiscordAIServiceDiagnostics/DedicatedV2/`에 추가했습니다. 기존 V2/r11 소스 파일 수정은 없습니다.

| 파일 | 역할 |
|---|---|
| build-dedicated.py | r11 고정 SHA를 확인한 재사용·새 후보 생성·전체 SHA·정확한 ZIP/차이 목록 |
| DedicatedContract.cs / v2_contract.py | C#/Python 배포3·SID/경로·보안/승인 검증. wire2와 구분 |
| DedicatedCore.ps1 | fake/native 공통 설치 순서와 runtime admission |
| DedicatedWindows.ps1 / DedicatedAccountPolicy.cs | 새 두 계정에 한정한 native adapter/LSA UTF16 byte 길이·원시 코드 보호 기록 |
| install-session0-v2.ps1 | 기본 Inspect인 승인 wrapper 및 설치 진입점; 보호 staging 재검증 계약 |
| DedicatedBootstrap.ps1 | 설치 파일 SHA/owner/writer 검증·새 프로세스 환경 최소화 |
| start-host-v2.ps1 / session0-client-v2.ps1 / fixed-worker-v2.ps1 | 전용 Host/client/고정 fake Job 연결 |
| AuthenticationPlan.md | 토큰 복사 없는 후속 공식 인증 절차 |
| test_dedicated.ps1 / test_runtime.py / test_package.py | 신규 결합 범위만 검증 |

기존 Session0PipeBoundary·V2Host·Job90·Python transport/runtime·내장 Python741개 파일을 검증된 바이트로 재사용합니다. V2Staging은 root 의존성/Bundle 검증 2개 지정 슬롯만 adapter 적용, WorkerFence는 WMI 마이크로초 생성시각 동일성 1개 슬롯만 수정했습니다. 전체 문자열 치환을 사용하지 않았습니다. 통합 C# assembly는 실제 컴파일 성공했습니다.

후보 폴더 `F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r1`, ZIP은 같은 경로의 `.zip`, 메타는 `.package.json`입니다. **payload766+Manifest1**, `approved=false / auto_start=false / FAKE_ONLY / real_ai_enabled=false`. 이 Temp 후보는 관리자 보호 staging이 아니며 직접 설치에 사용하지 않습니다.

| 산출물 | SHA256 |
|---|---|
| Installer | `83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f` |
| Manifest | `43f6afcc91eae196e015393670cd9cd4ff37194099b313c2b901177226cd32e3` |
| ZIP | `be58f8cdbe0befdcc64cd67287865c6cd6fc2611b4a706d26cda022a8014ca25` |

파일별 SHA·전체 Manifest·재사용 원본 SHA·r11 대비 added/changed/omitted 목록은 [PackageProof](팀운영/Evidence/DedicatedV2_20261027/PackageProof.json), 로컬 변경 소스별 SHA는 [SourceSHA256](팀운영/Evidence/DedicatedV2_20261027/SourceSHA256.json)에 있습니다. omitted는 새 후보에 넣지 않았다는 뜻이며 기존 파일 삭제가 아닙니다. ZIP은 Temp에만 보존하고 Git에 바이너리를 추가하지 않았습니다. r11 Installer/Manifest/ZIP SHA 세 개도 기존 고정값 일치 확인했습니다.

## 신규 오프라인 결과

| 범위 | PASS | FAIL | 증거 |
|---|---:|---:|---|
| fake 설치·Task·SID·권리/보호·admission·메모리 staging | 71 | 0 | PowerShellTests.json |
| binding3/wire2·가짜 Worker/Job·SQLite 장부 | 67 | 0 | PythonTests.json |
| 패키지 정확한 entry·SHA·소스 동일성·구문·r11 불변 | 13 | 0 | PackageTests.json |
| 합계 | **151** | **0** | Evidence/DedicatedV2_20261027/ |

다른 SID·구 승인·다른 epoch·SHA 오류·신규 SID 재사용·권한 확대·기존 자원 충돌·부분 실패·재등록·조회 불명을 거부했습니다. 가짜 동시 submit은 소비 장부로 1회만 진입, 가짜 timeout/AI 호출표시/잔존 자식/정리 불명은 HOLD이며 실제 Worker나 OS Job을 시작하지 않았습니다. staging은 메모리 fake FS에서만 생성·오류주입했고 네이티브 생성기를 호출하지 않았습니다. Python 테스트 DB는 자동 생성 전용 Temp에만 만들고 정리했습니다. 기존 성공 테스트 전체/관리자 전수조사는 실행하지 않았습니다.

이 PASS는 native OS 설치·실제 프로세스 fence·Windows 토큰/ACL·실제 Job 정리·부팅 동작이 입증되었다는 뜻이 아닙니다. 실제 사전검사·설치·IPC는 여전히 각각 별도 승인/증거가 필요합니다.

## 인증·PIN 전 동작 검증 계획

현재 fake 후보에는 공식 인증이 필요하지 않으며 저장소가 비어 있지 않으면 HOLD합니다. 바울 대표가 새 SID·CLI 버전/SHA·구독·목적을 별도 승인한 뒤 공식 `codex login --device-auth` 절차를 사용할 수 있는지 검토합니다. 계정/워크스페이스 정책이 허용해야 하며, 장치 코드와 인증 캐시는 로컬 보안 화면/저장소에만 두고 에이전트·Issue·로그로 수집하지 않습니다. [공식 인증 문서](https://learn.chatgpt.com/docs/auth).

interactive deny를 해제하거나 기존 auth를 복사하지 않습니다. 새 SID의 승인된 Batch enrollment launcher와 로컬 보안 표시 수단은 **후속 설계·구현 대상**입니다. 현 후보에는 로그인 도구가 없습니다. 실제 인증 후에는 EMPTY_NO_IMPORT_NO_LOGIN 계약이 달라지므로 또 다른 Manifest/승인이 필요합니다. 상세는 패키지 AuthenticationPlan.md에 보존했습니다.

로그인/PIN 전 Batch 실행은 PASSWORD Task와 Batch 권리 설계상 검토 가능하지만 이 PC에서 실증하지 않았습니다. Task는 지금 Trigger0이므로 부팅 자동 실행이 아닙니다. 후속 승인에서 새 Worker 전용 일회 부팅 시험/취소·복구 범위를 정하고, 새 profile/Session0·비대화형 토큰·PIN 미입력·USB WiFi 준비 지연을 확인해야 합니다. 네트워크 미준비는 HOLD·자동 재시도0, 기존 OnBoot2 수정0, 이번 부팅/재부팅0입니다.

## 후속 승인과 복구 순서

1. 신규 SID 전용 read-only 사전검사/지원 OS·PowerShell7·보안 정책·배치 로그온 deny의 유효 그룹 영향·부모 ACL·전용 자원 부재·GROUP/직접 launcher 증거를 준비합니다. 정보 부족이면 HOLD합니다. 기존11개/전체 전수진단 반복은 하지 않습니다.
2. **보호 staging 생성**만 별도 승인: 위 신규 staging root/package에 BA/SY 생성시 DACL로 동일 ZIP/payload 배치, Owner/Group/writer/FileID/ADS/reparse/hardlink/정확 entry/전체 SHA와 설치 직전 handle 재검증. 원본 Temp/r11/V1 ACL 수정0. 승인 저장소의 관리자 보호 provisioning도 별도 범위로 검토해야 합니다.
3. **실제 설치 1회** 별도 승인: 정확한 신규 SHA3개·외부 보호 승인·adminSID·두 신규 계정·권리·Worker batch-only 활성화·Disabled Task1·새 디렉터리·소비 기록 범위 명시. 암호는 로컬 SecureString 입력, 기존 암호 요청0. 현재 승인 파일/실행 명령을 생성하지 않았습니다. 관리자 native 재조회가 모두 PASS여야 합니다.
4. 설치 실패 시 보호 결과/소비/실제 생성 SID 목록과 stage/Win32/HRESULT/NtStatus를 보존하고 **재시도·자동 삭제·권한 완화 없이 HOLD**. 기존 운영/V1 자원에는 손대지 않습니다. 필요 시 별도 승인 후 실제 생성 SID/Owner/Task명/파일SHA가 신규 목록과 일치하는 항목에만 비활성화/격리·정리 계획을 작성합니다. 기존 Cleanup/Install은 사용하지 않습니다. 현재 복구 명령/삭제 도구도 실행·제공하지 않았습니다.
5. **실제 FAKE_ONLY IPC 1회**는 그 다음 승인입니다. 새 Worker/Task/PIPE ACL·승인/consume/lease·유효 토큰·비협조 launcher 통제·PID세대/Job 감독·기존 운영 보존이 모두 필요합니다. Bot은 Disabled+interactive deny 상태이므로 새 Bot의 제한된 Batch client 실행 경로/활성화도 별도 검토·승인해야 합니다. 이번 후보 설치만으로 IPC 실행 준비 완료가 아닙니다.
6. 실제 인증·AI1회·공식 사용량·Discord·A는 각각 후속 승인입니다. 같은 구독/PC 공용 한도·다른 실행기 위험이 남으면 계속 HOLD합니다.

실제 계정 생성/활성화/권리·ACL 변경/Task/staging/설치/인증/IPC/Worker/AI/usage/Discord/A/PC 종료·재부팅/Push **전부0**. V1 REFERENCE_VERIFICATION_HOLD, 기존 Worker/11개·인증/CODEX_HOME·OnBoot2·봇/아트·운영SQLite·Unity/게임/Save 보존, MANUAL/A HOLD 유지하고 중지합니다.
