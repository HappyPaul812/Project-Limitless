# 대상 밖 최초 staging 검토 — 경로 충돌 해소 가능 / 설치 HOLD

기준 `969f35df9a69291635934e85cf6832b40a0516c6`, 2026-10-31. GitHub #66 최신 인계 및 로컬 인계를 확인했습니다. 이번 지시가 대상 밖 staging 검토를 명시 허용한 점을 반영합니다. 기존 경로 제한 때문에 불가능하다는 직전 결론을 그대로 적용하지 않습니다.

## 결과

**최초 staging 순환 의존성 해결: 최종 PASS 아님, HOLD.** 대상 밖 경로를 사용하면 경로상의 모순은 해소할 수 있습니다. 그러나 최초 생성·이미지 봉인을 수행할 신뢰된 실행 절차는 미확정입니다. 안전한 단일 절차 확정 후에만 코드를 수정하라는 조건 및 새 위험 시 중지 지시에 따라 제품 코드·게이트 변경과 모의 테스트를 시작하지 않았습니다. 신규 QA는 NOT_RUN이며 PASS로 집계하지 않습니다.

후보 `C:\ProgramData\LimitlessDiscordV2BootstrapStaging`은 이번 읽기 확인에서 부재였습니다. **검토 후보이며 운영 binding/생성 승인 아님**. `C:\ProgramData\Limitless`의 하위가 아닌 형제이므로 Bootstrap의 ProtectedFile 입력 금지 범위와 겹치지 않습니다. 해당 후보를 별도로 안전하게 준비할 수 있다면 기존 여섯 대상 부재 계약·기존 Bootstrap 코드와 이미지를 변경할 필요가 없습니다. 후보의 leaf ACL/owner는 아직 존재하지 않아 검증하지 못했습니다.

## Windows 기본 기능 비교

|방법|확인한 범위|채택하지 않은 이유|
|---|---|---|
|New-Item/Explorer 후 Set-Acl 또는 icacls|일반 생성 및 사후 보안 변경|생성 순간부터 명시적 보호라는 계약 미충족|
|DirectorySecurity 기반 CreateDirectory|초기 디렉터리 보안 지정 가능|기존 디렉터리도 반환하는 API. 단독 호출로 기존 항목 거부·부모 pin·최종 경로/ID·승인 경계가 보장되지 않음|
|CreateDirectoryW + SECURITY_ATTRIBUTES|초기 SD 생성과 기존 경로 오류 가능, 기존 코드에서도 사용|API 자체가 아니라 이 API를 호출할 최초 신뢰된 실행 주체/봉인 절차가 필요|
|Windows Installer MsiLockPermissionsEx|폴더 등 객체의 SDDL 지정 가능|새 설치 패키지 작성·권한 부여 시점·경합·실패/rollback 계약 검증이 필요. 기존 검증된 설치 절차가 아니므로 더 작은 해결로 단정하지 않음|
|PowerShell Add-Type/PInvoke로 기존 코드 재사용|별도 NativeAOT 프로그램 없이 Windows API 호출 구조 가능|특권 스크립트 및 최초 로더의 승인·신뢰 검증이 필요. 단순히 Windows 기본 제공 기능을 썼다는 이유로 새 특권 코드가 검증 완료되지는 않음|

Microsoft 공식 근거:

- [New-Item 구문](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.management/new-item?view=powershell-7.6)
- [보안 지정 CreateDirectory와 기존 디렉터리 반환](https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemaclextensions.createdirectory?view=net-10.0)
- [CreateDirectoryW 초기 SECURITY_ATTRIBUTES](https://learn.microsoft.com/ko-kr/windows/win32/api/fileapi/nf-fileapi-createdirectoryw)
- [MsiLockPermissionsEx](https://learn.microsoft.com/en-us/windows/win32/msi/msilockpermissionsex-table)
- [Add-Type](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/add-type?view=powershell-7.3)

상기 공식 기능의 존재와 로컬 전체 절차의 안전성은 구분합니다. 사용 가능한 모든 Windows 설치 수단이 불가능하다는 결론은 아닙니다.

## 정확한 신뢰 경계와 최소 후속안

기존 Bootstrap의 자기 SHA 검사는 **진입 후 검사**입니다. 변조 가능한 E 원본을 관리자 실행한 뒤 자기 검사에 맡기는 것은 악성 코드가 검사 전에 실행될 가능성을 배제하지 못합니다. 경로 hash를 확인한 뒤 별도 Copy-Item/실행을 하는 순서도 사이의 교체를 막지 않습니다. 새 staging을 만들 프로그램을 그 staging에 먼저 요구하면 순환이 반복됩니다.

최소 후속 검토안은 **기존 Windows 관리자 호스트에서 명시적으로 승인·검증한 한 번의 설치 스크립트로 기존 보안 API 코드를 재사용**하는 방식입니다. 새 EXE를 연쇄 제작하는 방식을 권고하지 않습니다. 다만 이것은 현재 승인된 안전 절차가 아니라 다음 설계·검증 대상입니다. 최소 필수 경계는 다음과 같습니다.

1. 신뢰된 관리자 호스트·상승·비위장 상태, 실제 승인 계정, 새 설치 범위/시간창을 확정합니다. 기존 승인과 SID를 자동 채택하지 않습니다.
2. 설치 스크립트를 비관리자가 바꿀 수 있는 경로에서 그대로 실행하지 않습니다. 승인된 신뢰 채널의 스크립트 SHA와 **같은 읽기 바이트**를 검증하고, 그 동일 바이트를 실행하는 최초 로더 경계를 정의해야 합니다. 모듈/프로필 및 동적 컴파일 입력의 바꿔치기도 검토 대상입니다. 이 로더를 이번에 구현하지 않았습니다.
3. C:\와 ProgramData의 현재 owner/DACL/reparse/volume/FileKey를 HANDLE로 확인·유지하고, 별도 staging이 존재하면 중지합니다. 기존 C/E ACL은 수정하지 않습니다.
4. 신규 staging만 CreateDirectoryW의 명시적 초기 SD로 생성하고 `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`의 owner/group/비상속 DACL 및 최종 경로/FileKey를 readback합니다. API 재사용 후보는 기존 WindowsBootstrapHost의 관련 로직입니다. 기존 Bootstrap에 seed 우회 모드를 추가하지 않습니다.
5. 원본 EXE는 쓰기·삭제 공유를 허용하지 않는 동일 HANDLE에서 읽고 승인 SHA를 검증합니다. 목적 EXE는 CREATE_NEW·초기 SD·동일 데이터 복제 후 SHA/owner/DACL/최종 경로 readback으로 봉인합니다. 단순 경로 사전 hash만으로 충분하지 않습니다.
6. 승인 JSON 역시 별도 승인된 원문과 SHA로 봉인합니다. 미승인 ID/SID/Approved=true/신뢰 판정을 만들어 넣지 않습니다.
7. 실패 시 보존·중지하며 삭제/ACL 복구/자동 재시도는 하지 않습니다. 일회성 설치 승인·실패 기록을 어디에서 어떻게 신뢰할지까지 확정해야 합니다.

위 최초 로더와 승인 경계가 미검증인 것이 이번의 정확한 차단 이유입니다. 이 위험을 남긴 채 코드·실행 명령을 만들어 안전한 절차로 제시하지 않습니다. 기존 Bootstrap 계약의 수정은 필요하지 않으며, 현재 게이트를 완화하지 않았습니다.

## 실제 실행까지 남은 단계

1. 위 단일 스크립트 최초 신뢰 경계의 구체적 설계·승인 및 필요한 최소 코드 범위를 확정합니다. 이 단계에서 성립하지 않으면 HOLD합니다.
2. 해당 변경에만 모의 QA: 잘못된 스크립트/이미지 SHA·비관리자·만료/취소·조상 교체/reparse·기존 staging·복사 실패·readback 불일치·재사용 거부·인가 전 쓰기 0회. 기존 59 PASS 등은 반복하지 않습니다.
3. 실제 staging 생성·봉인 복사에는 별도 승인이 필요합니다. 최초 적용 성공 후 여섯 대상 부재를 재확인하고 현재 부모 native SD SHA/FileKey·실제 관리자 SID·새 승인 원문 SHA를 발급합니다.
4. Bootstrap 실제 --review/--create 실행은 별도 승인 대상입니다. 새 이미지 공식 V3 판정 NOT_VERIFIED와 다른 신뢰 미결을 함께 보고합니다. staging 성공은 Task·배포·FAKE_ONLY 승인과 무관합니다.

대표 GUI 확인은 의도한 관리자 계정과 UAC 프로그램/출처, V3의 정확한 새 이미지 판정을 직접 확인하는 범위입니다. 생성·파일 봉인·권한 readback은 검증된 관리자 절차가 담당해야 하며 Explorer의 사후 권한 변경으로 대체하지 않습니다. 비밀 암호 공유나 보안 경고 우회를 요구하지 않습니다.

## 보존·11월7일 인계

기존 이미지/검증은 재사용했고 재측정·재빌드·시험 반복하지 않았습니다.

|이미지|기존 SHA256|바이트|
|---|---|---:|
|Bootstrap|2FF5669350A6761A7EB979F6133A4B0A85B44F5440F96A05644A57601026349E|2,951,168|
|Probe|0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322|1,892,352|
|Supervisor|6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7|7,559,680|

Bootstrap 소스 `5a105bcadedd8f75dfe0c864def4f42dfc921ecf`, 직전 인계 `969f35df9a69291635934e85cf6832b40a0516c6`. 이번 브랜치 `codex/discord-v2-external-staging-boundary-20261031`. 제품 코드·이미지·binding 무변경. 실제 생성/ACL/EXE실행/Task계정OnBoot/V3/UnitySave 변경 0회. 신규 SID/승인 파일 발급 0회. 공개 댓글에는 경로·SHA·SID·승인 구조를 올리지 않습니다.

추가 사용량을 쓰는 다음 작업은 API 재조사나 기존 QA 반복이 아니라 **위 최초 로더 신뢰 경계 한 가지의 구체화**입니다. 이번에는 새 위험 시 중지 지시에 따라 더 조사·구현하지 않습니다. NO_GO / MANUAL / EXECUTION_HOLD. cutoff 2026-11-01 23:59:59 KST 자동 연장 금지. 11월2일 평일 게임 개발 복귀, 11월7일 Discord 재개 시 새 시간창·영향 코드/이미지·계정 신뢰·새 승인부터 검토합니다. 오늘 이미지/옛 승인으로 자동 재개하지 않습니다.
