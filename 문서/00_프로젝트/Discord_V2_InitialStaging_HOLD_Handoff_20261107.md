# 최초 보호 staging 선행 의존성 — HOLD / 11월7일 인계

검토일 2026-10-31. 기준 `634f4a927a35ad198808b4111be67e5b94b0ec15`. GitHub #66 최신 인계(댓글 6542881924) 및 로컬 Bootstrap 관리자 실행 준비 문서를 확인했습니다. 이번 작업은 문서만 작성했으며 코드·이미지·binding 변경, 빌드·시험 반복, 실제 폴더 생성·ACL 변경·제품 실행은 없습니다.

## 결론

**현재 고정 경로와 기존 이미지 계약을 동시에 지키는 최초 staging 생성 방법은 확정되지 않았습니다. NO_GO / MANUAL / EXECUTION_HOLD.** 안전한 실제 관리자 실행 명령을 발급하지 않습니다. 조사 범위를 확대하거나 대체 경로·새 프로그램을 임의 도입하지 않고 중지합니다.

공식 대상은 `C:\ProgramData\Limitless\DiscordV2`이며 C:\Limitless_Temp 및 기존 E ACL은 변경하지 않습니다. Native CodeRoot/EvidenceRoot 및 기존 binding도 유지합니다.

## 선행 관계의 정확한 충돌

1. Bootstrap Main은 승인 JSON을 먼저 보호 경로에서 읽고, 자신의 이미지도 보호 경로에서 검증합니다. UAC 상승만으로 이미지 또는 승인 파일이 봉인되지 않습니다.
2. `WindowsBootstrapHost.ProtectedFile`은 여섯 생성 대상과 그 하위에 있는 입력 파일을 `BOOTSTRAP_INPUT_IN_NEW_TARGET`으로 거부합니다. 첫 대상은 `C:\ProgramData\Limitless`이므로 그 아래 staging도 모두 거부됩니다.
3. Bootstrap은 Limitless, DiscordV2, Control, DeploymentTool, Approvals, DeploymentLedger가 모두 부재일 때만 새로 생성합니다. 누군가 이 경로에 staging을 먼저 만들면 다음 Bootstrap은 기존 경로 거부로 중지합니다. 기존 항목의 안전성을 확인해 채택하는 모드는 없습니다.
4. 따라서 관리자 Explorer로 대상 폴더를 선행 생성하거나, 대상 Control 안에 EXE/JSON을 배치하는 방법은 기존 이미지를 그대로 실행하는 해법이 아닙니다.
5. E 빌드 원본은 앞선 검토에서 개인 소유·상속 DACL·Authenticated Users 수정권한으로 관측되었습니다. SHA 문자열만 대조한 뒤 그 경로에서 관리자 실행하는 것은 교체 방지 계약을 만족하지 않습니다. 이번에는 이미지·ACL을 재검사하지 않고 이전 근거를 재사용합니다.

근거: `Tools/DiscordV2/ProtectedBootstrap/Program.cs`, `WindowsBootstrapHost.cs`의 ProtectedFile/ReadProtectedAuthorization/ObserveActor 및 `BootstrapPolicy.cs`의 대상 부재 검사. 이전 관리형 59 PASS와 네이티브 함수 35개·분기 10개 정적 검증은 보존하며 재실행하지 않았습니다.

## 기존 Windows 기능 재사용 검토

Microsoft의 [CreateDirectoryW 계약](https://learn.microsoft.com/ko-kr/windows/win32/api/fileapi/nf-fileapi-createdirectoryw)은 SECURITY_ATTRIBUTES로 새 디렉터리의 보안 descriptor를 지정하고 기존 경로에는 ERROR_ALREADY_EXISTS를 반환합니다. 이 API는 이미 Bootstrap에서 사용 중입니다. OS API 재사용은 가능하지만 API 하나로 관리자 인증·부모 HANDLE pin·안전한 원본 읽기/복사·SHA readback·승인 일회성 관리 전체가 제공되지는 않습니다.

Microsoft의 [FileSystemAclExtensions.CreateDirectory](https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemaclextensions.createdirectory?view=net-10.0)는 보안 정보를 받지만 기존 디렉터리도 반환할 수 있습니다. 이것만 호출하는 PowerShell 한 줄은 기존 항목 거부와 경합 방지 전체 계약의 검증된 구현이 아닙니다. Add-Type/PInvoke로 대체하면 새 특권 스크립트를 도입하는 것이며, 검증 없이 기존 Windows 명령으로 간주할 수 없습니다.

Explorer/일반 mkdir 또는 New-Item으로 먼저 생성하고 icacls/Set-Acl로 나중에 보호하는 절차는 생성 순간부터 보호한다는 요구를 충족하지 않습니다. 기존 배포 도구 후보도 보호된 도구·승인 자료·소비 원장을 선행 요구하므로 최초 신뢰 루트를 제공하지 못합니다. 현 저장소에서 이 전체 조건을 이미 검증한 독립 최초 staging 설치 경로를 확인하지 못했습니다. Windows에 가능한 다른 설치 수단이 전혀 없다고 주장하는 것은 아닙니다.

## 관리자 절차가 갖춰야 할 조건 — 실행 명령 아님

최초 설치 경계는 별도 설계 승인 후 기존 Windows 보안 API/검증 코드를 최대한 재사용해야 합니다. 신규 프로그램 제작은 이번에 결정하거나 구현하지 않습니다.

- 관리자 상승·비위장 상태와 실제 승인 계정을 확인합니다. 승인되지 않은 SID를 채워 넣지 않습니다.
- 기존 항목·reparse·조상 owner/DACL 불일치를 거부하고 부모 HANDLE을 유지합니다. 단순 Test-Path 후 생성만으로는 경합 보호를 주장하지 않습니다.
- 신규 디렉터리는 명시적 초기 descriptor로 생성합니다. 검토 중인 SDDL은 `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`이며 실제 적용 승인은 아닙니다.
- 원본을 비관리자가 바꿀 수 있으므로 안전하게 연 동일 HANDLE에서 읽은 바이트를 승인 SHA와 대조하고, 새 목적 파일 CREATE_NEW·초기 보안·재읽기 SHA·최종 경로/owner/DACL 검증을 결합해야 합니다. 경로별 사전 hash 후 일반 Copy-Item을 실행하는 방법을 검증 완료로 취급하지 않습니다.
- 승인 자료도 생성·봉인·보호 원문 SHA 확인이 필요합니다. 실제 승인 ID·JSON·신뢰 판정은 대표의 별도 승인 없이 발급하지 않습니다.
- 실패·부분 생성은 보존하고 자동 삭제/ACL 수선/재시도·옛 승인 재사용 없이 중지합니다.

이 조건의 검증된 실행 주체와 계약 변경 범위가 없으므로 실제 생성/copy/icacls/Bootstrap 명령을 제공하지 않습니다. 이전 문서의 Bootstrap 명령도 미확정 자리표시자 템플릿 상태를 유지합니다.

## 대표 GUI 작업과 관리자 적용의 구분

대표가 직접 확인할 사항: V3의 정확한 새 이미지 판정/탐지 기록, 의도한 관리자 계정, Windows UAC에 표시되는 프로그램·계정·출처. 암호·자격증명은 공유하거나 로그에 남기지 않습니다. 확인되지 않은 V3 정상 여부 및 계정 신뢰는 UNKNOWN/NOT_VERIFIED로 둡니다. 이번에는 UAC를 호출하거나 GUI를 조작하지 않았습니다.

별도 승인할 관리자 작업: 최초 설치 신뢰 경계와 경로 계약의 변경 범위 결정 → 해당 구현의 정적·모의 검증 → 필요한 경우 별도 빌드·이미지 신뢰/봉인 → 정확한 신규 시간창과 승인 자료 발급 → 최초 staging 생성·복사/readback → Bootstrap 실제 실행. 오늘 어느 단계도 실제 적용 승인이 아닙니다. UAC 동의만으로 이 작업 승인을 대신할 수 없습니다.

## 보존 이미지·소스 및 다음 작업

아래는 이전 검증 기록의 재사용이며 이번 신규 hash 측정이나 런타임 검증이 아닙니다.

|이미지|SHA256|바이트|
|---|---|---:|
|Bootstrap|2FF5669350A6761A7EB979F6133A4B0A85B44F5440F96A05644A57601026349E|2,951,168|
|Probe|0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322|1,892,352|
|Supervisor|6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7|7,559,680|

Bootstrap 코드 `5a105bcadedd8f75dfe0c864def4f42dfc921ecf`; 빌드 결과 기준 `83b5d15e9be0158c13a2842476236f33fca97450`; 직전 실행 준비 `634f4a927a35ad198808b4111be67e5b94b0ec15`。보고 브랜치 `codex/discord-v2-initial-staging-hold-20261031`. 기존 Native 이미지·소스 위치는 이전 빌드/실행 준비 인계를 따릅니다. 기존 Probe·Supervisor와 과거 Supervisor 이미지도 이동·수정하지 않습니다.

11월7일 재개 순서:

1. 신규 지시와 시간창부터 확정합니다. 기존 cutoff **2026-11-01 23:59:59 KST** 이후 옛 승인 재사용·자동 연장 금지. 계정 만료는 기존 인계의 11월2일 값이며 이번 재조회하지 않았습니다.
2. 고정 대상 안 staging과 기존 Bootstrap의 대상 밖 입력 조건을 어떻게 해소할지 설계 결정이 필요합니다. 경로 제한을 몰래 완화하거나 기존 대상 채택을 추가하지 않습니다.
3. 결정된 최소 변경만 별도 승인·구현·검증합니다. cutoff 변경 등 새 코드가 필요하면 해당 이미지 새 빌드·새 SHA/신뢰 승인을 별도로 진행해야 합니다. 현재 이미지가 11월7일에 그대로 실행 가능하다고 간주하지 않습니다.
4. 실제 staging/승인 봉인/Bootstrap 생성/배포/Task/FAKE_ONLY는 각각 필요한 승인과 미검증 조건을 확인합니다. 소비 원장 binding·실행 승인 발급은 여전히 미완료입니다.

11월2일부터 평일 Limitless 게임 개발 복귀, Discord 개발은 11월7일 토요일 재개 방침을 유지합니다. 자동 예약·재개 설정 없음. GitHub에는 민감 경로·SID·승인 자료를 게시하지 않고 최소 HOLD 요약만 남깁니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
