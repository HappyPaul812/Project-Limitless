# Discord V2 최초 staging 운영 승인 경계

2026-11-21 · Issue #66 · NO_GO / MANUAL / EXECUTION_HOLD

## 원장과 최초 준비

- 최초 staging과 단발 소비 원장을 최종 제품 트리의 바깥에 고정했습니다. staging은 `C:\ProgramData\LimitlessDiscordV2BootstrapStaging`, 최초 설치 원장은 `C:\ProgramData\LimitlessDiscordV2BootstrapLedger`입니다. 최종 제품은 기존 `C:\ProgramData\Limitless\DiscordV2`와 기존 DeploymentLedger를 계속 사용합니다.
- 순환을 끊는 선행 작업은 최초 설치 원장의 **별도 승인된 사전 준비**입니다. 관리자가 신뢰된 배포 수단으로 고정된 단일 원장 경로를 관리자 전용 초기 descriptor로 원자적으로 생성하고, HANDLE에서 최종 경로·owner·DACL·볼륨·파일 키를 readback하여 승인 발급자에게 인계합니다. 생성 작업은 기존 경로가 보이면 중단합니다. 기존 경로의 출처, 보안 및 연속성 확인 없이 채택·수선·재생성하지 않습니다. 이 준비 작업의 실행 수단·경로 승인은 이번에 제공하지 않았습니다.
- 설치 호출은 원장을 생성하지 않습니다. 서명된 파일 키와 보호 상태를 대조한 뒤 HANDLE을 유지합니다. 소비 기록은 초기 관리자 전용 descriptor 및 `CREATE_NEW`로 생성하고 같은 HANDLE에서 권한을 검증한 뒤 flush합니다. 기존 기록과 부분 기록은 보존하며 원장 교체 또는 소실 시 중지합니다. 최종 원장으로 이동·병합·초기화하지 않습니다.

## 독립 신뢰와 승인

- 실제 Windows 쓰기 함수는 관리형 설치 진입점 안에서만 연결했습니다. 승인 JSON 원문 서명, 독립 공개키 식별값, 승인에 묶인 원본 SHA와 원본 서명을 검사합니다. 원본은 쓰기·삭제 공유 없이 한 번 연 HANDLE에서 읽고 실행하지 않습니다. 관리자 토큰·원장·조상을 관찰하고, 시간창 확인 → 소비 → 재관찰 → 시간창 확인 → 생성 및 readback 순으로 진행합니다.
- 바울 대표가 승인한 독립 배포 채널에서 검증기의 릴리스 서명자와 공개키 SPKI SHA-256을 확인하고, 검토·서명한 릴리스에 키 지문을 봉인해야 합니다. 배포 패키지는 관리자와 배포 주체만 변경 가능한 위치에 두고, OS 실행 정책으로 승인된 검증기만 상승 전에 실행 허용해야 합니다. 공개 키 바이트는 사전에 봉인된 지문으로 검사합니다. 서명 개인키는 별도 보관합니다. 현재 릴리스의 키 지문은 비어 있으며 신뢰된 런처나 패키지는 배치하지 않았습니다.
- 새 서명 승인에는 V2 staging 프로필, 이전 승인과 겹치지 않는 단발 ID, 정확한 staging·원장 경로 및 원장 파일 키, 검토한 관리자 SID, 시작·만료, 원본 SHA, 조상 파일 키·보안 descriptor SHA가 필요합니다. 원본과 승인 JSON 각각의 분리 서명이 필요합니다. 11월 1일 cutoff를 대체할 계약과 11월 2일 계정 만료 재검토는 별도 승인 사항입니다.
- 대표 GUI에서는 승인된 출처에서 받은 검증기 파일의 디지털 서명과 서명자를 파일 속성에서 확인하고, Windows 계정 관리에서 의도한 계정과 만료 상태를 확인할 수 있습니다. 이번 단계에서 UAC·보호 폴더 변경·Task 실행은 진행하지 않습니다.

## 검증

관리형 컴파일 경고 0·오류 0. 신규 12 PASS / 0 FAIL. 신뢰 미설정, 승인·원본 변조, 계약 만료, 잘못된 원장·구 프로필, 원장 확인 및 소비 실패, 중복, 소비 직후 만료·교체 차단을 검증했습니다. 파일 기록은 격리 Fixture에서 시험했고 Windows 운영 디렉터리에는 접근하거나 쓰지 않았습니다. 기존 15/25/59 PASS 재실행, 전체 솔루션·Unity 검사, 대량 해시, 기존 이미지 실행, NativeAOT, Task·Session0·AI 호출 0회. 기존 Bootstrap 소스의 HANDLE 읽기 함수 접근 범위만 조정했습니다.

## 단일 PC 로컬 서명 체계 결정

- Windows Authenticode의 WinVerifyTrust 결과가 성공이어야 하며, 해당 결과의 서명자 인증서 SHA-256과 RSA SPKI SHA-256이 승인 릴리스에 봉인된 두 지문과 일치해야 합니다. 명시적 코드서명 EKU 및 현재 유효기간도 확인합니다. 성공한 Windows 서명에서 얻은 공개키만 기존 RSA-PSS 승인·원본 검증에 넘깁니다. 원본은 동일한 읽기 HANDLE을 유지하고 Authenticode 이후 처음부터 읽어 승인 SHA 및 분리 서명을 검사합니다. 기존 원장 및 staging 흐름을 유지합니다.
- 최초 등록은 별도 관리자 세션에서 승인합니다. Microsoft 공식 발급 기능으로 전용 RSA 코드서명 인증서를 로컬 컴퓨터 개인 저장소에 만들고, CNG 개인키를 내보내기 불가로 설정합니다. 인증서 관리자 MMC에서 용도·만료를 확인하고 개인키 관리에서 지정된 서명 관리자만 접근하도록 확인합니다. 키 저장 공급자에서 내보내기 불가 설정을 별도로 확인합니다. 인증서 내보내기 마법사에서는 공개 인증서만 내보냅니다. 개인키·비밀번호는 내보내거나 기록하지 않습니다.
- 독립 관리자 승인 경로에서 공개 인증서 원본과 DER SHA-256 및 RSA SPKI SHA-256을 대조·보관합니다. Windows 신뢰 체인이 성립하도록 필요한 로컬 컴퓨터 신뢰 저장소 변경, 승인된 인증서 한 건의 등록, 검증기 패키지 및 상승 전 실행 정책은 별도 승인으로 제한합니다. 서명자 인증서를 루트 또는 게시자 저장소에 넣는 경우 그 영향 범위도 승인에 포함합니다. 설치 스크립트나 서명 파일 안에서 신뢰 지문을 공급받지 않습니다. 검증기 자체는 신뢰된 배포 경로와 Windows 실행 정책에서 먼저 검증·허용합니다.
- Authenticode 서명 후 확정 바이트로 원본 SHA 및 RSA-PSS 분리 서명을 발급합니다. 인증서 등록과 파일 서명만으로 운영 승인이 성립하지 않습니다. 새 계약·계정 재검토·원장 준비 및 신규 단발 승인이 필요합니다. 지문 상수는 비어 있고 기존 cutoff는 유지합니다.
- 신규 컴파일 경고·오류 0, 신규 7 PASS / 0 FAIL. Windows 미서명 원본 거부 및 합성 지문·EKU·유효기간 분기를 검증했습니다. 실제 신뢰 등록, 승인된 서명본의 성공 경로 및 상승 전 배포 정책은 미검증입니다. 인증서·개인키 생성, 관리자 변경, 기존 시험 반복 0.

공식 근거 [WinVerifyTrust](https://learn.microsoft.com/en-us/windows/win32/api/wintrust/nf-wintrust-winverifytrust) · [코드서명 인증서 발급과 키 내보내기 정책](https://learn.microsoft.com/en-us/powershell/module/pki/new-selfsignedcertificate?view=windowsserver2025-ps)

## 대표 실행용 로컬 코드서명 발급 준비

2026-11-21 기준 준비문입니다. 발급, 신뢰 등록, 배포 및 운영 staging 실행은 서로 다른 승인입니다. 현재 세션의 관리자 역할은 비활성이고 관리자 그룹 SID는 관측되었습니다. 두 개인 저장소 모두 읽을 수 있고 Windows PKI의 발급·내보내기·가져오기 cmdlet을 확인했습니다. 지정 서명 계정의 실제 상승과 키 접근은 발급 후 확인합니다.

### 1. 발급 승인 후 전용 인증서 한 건 발급

바울 대표가 지정한 **서명 계정으로 관리자 PowerShell 7**을 열고 아래 블록을 실행합니다. 승인한 인증서 만료일은 시간대가 포함된 값입니다. 기존 동일 이름의 인증서가 보이면 중단하고 별도로 검토합니다.

```powershell
$ErrorActionPreference = 'Stop'
$identity = [Security.Principal.WindowsIdentity]::GetCurrent()
$principal = [Security.Principal.WindowsPrincipal]::new($identity)
if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    throw '관리자 PowerShell이 필요합니다.'
}
$expected = Read-Host '승인된 서명 계정 이름 (컴퓨터이름\계정)'
if ($identity.Name -ne $expected) { throw '승인 계정 불일치' }
$expires = [DateTimeOffset]::ParseExact(
    (Read-Host '승인된 인증서 만료 시각 (yyyy-MM-ddTHH:mm:ss+09:00)'),
    "yyyy-MM-dd'T'HH:mm:sszzz", [Globalization.CultureInfo]::InvariantCulture)
if ($expires -le [DateTimeOffset]::Now) { throw '만료 시각을 확인하세요.' }

$subject = 'CN=LIMITLESS Discord V2 Local Code Signing'
if (Get-ChildItem Cert:\LocalMachine\My | Where-Object Subject -eq $subject) {
    throw '동일 이름 인증서가 있습니다. 기존 항목을 검토하세요.'
}
$cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject $subject `
    -CertStoreLocation Cert:\LocalMachine\My -Provider 'Microsoft Software Key Storage Provider' `
    -KeyAlgorithm RSA -KeyLength 3072 -HashAlgorithm SHA256 -KeyUsage DigitalSignature `
    -KeyExportPolicy NonExportable -NotAfter $expires.LocalDateTime
```

발급 뒤 `certlm.msc` → **개인 → 인증서 → 해당 인증서 → 모든 작업 → 개인 키 관리**에서 지정 서명 계정과 승인된 보안 주체의 키 접근을 확인합니다. 추가 ACL 변경은 별도 승인 대상입니다. 아래 블록은 동일 세션에서 키를 내보내지 않고 속성과 지문을 확인합니다.

```powershell
$private = [Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPrivateKey($cert)
if ($null -eq $private) { throw '현재 서명 계정이 키를 열 수 없습니다.' }
try {
    if ($private -isnot [Security.Cryptography.RSACng] -or
        $private.Key.ExportPolicy -ne [Security.Cryptography.CngExportPolicies]::None) {
        throw 'CNG 키 내보내기 불가 설정을 확인하세요.'
    }
} finally { $private.Dispose() }

$public = [Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($cert)
try {
    $eku = @($cert.Extensions |
        Where-Object { $_ -is [Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension] } |
        ForEach-Object { $_.EnhancedKeyUsages } | ForEach-Object { $_.Value })
    if ($eku -notcontains '1.3.6.1.5.5.7.3.3') { throw '코드서명 EKU가 없습니다.' }
    [pscustomobject]@{
        CodeSigningEku      = $eku -join ', '
        NotBefore           = $cert.NotBefore
        NotAfter            = $cert.NotAfter
        WindowsThumbprint   = $cert.Thumbprint
        CertificateDerSha256 = [Convert]::ToHexString(
            [Security.Cryptography.SHA256]::HashData($cert.RawData)).ToLowerInvariant()
        PublicKeySpkiSha256 = [Convert]::ToHexString(
            [Security.Cryptography.SHA256]::HashData($public.ExportSubjectPublicKeyInfo())).ToLowerInvariant()
    }
} finally { $public.Dispose() }

$cerPath = Read-Host '기존 승인 폴더 안의 새 공개 .cer 절대 경로'
if (-not [IO.Path]::IsPathFullyQualified($cerPath) -or $cerPath.StartsWith('\') -or
    [IO.Path]::GetExtension($cerPath) -ne '.cer' -or (Test-Path -LiteralPath $cerPath) -or
    -not (Test-Path -LiteralPath (Split-Path -Parent $cerPath) -PathType Container)) {
    throw '기존 승인 폴더 안의 사용하지 않은 로컬 .cer 경로가 필요합니다.'
}
Export-Certificate -Cert $cert -FilePath $cerPath -Type CERT -NoClobber
```

Windows 지문은 저장소에서 항목을 식별하는 값입니다. 운영 검증기는 인증서 전체 DER의 SHA-256과 공개키 SPKI의 SHA-256을 별도로 고정합니다. 공개 자료만 별도 관리자 승인 경로로 전달합니다.

### 2. 신뢰 등록 승인 후 승인된 인증서 한 건 등록

자체 서명 인증서는 체인 신뢰를 위해 일반적으로 `LocalMachine\Root`의 해당 인증서 신뢰가 필요합니다. 컴퓨터 범위 Root 등록은 Discord V2 밖의 Windows 검증에도 영향을 줍니다. `LocalMachine\TrustedPublisher`는 별도의 게시자 신뢰이며 루트 체인 신뢰를 대체하지 않습니다. 실제 실행 정책이 요구하고 명시적으로 승인된 경우에만 게시자 저장소에 추가합니다. MMC에서는 **로컬 컴퓨터 → 신뢰할 수 있는 루트 인증 기관 → 인증서 → 가져오기**를 사용합니다. 명령으로 진행한다면 별도 승인 채널에서 확인한 DER 지문을 입력합니다. 같은 메모리에서 읽고 지문을 확인한 공개 인증서만 저장소에 넣습니다.

```powershell
$ErrorActionPreference = 'Stop'
$cerPath = Read-Host '승인 경로에서 확인한 공개 .cer 절대 경로'
$approvedSha = (Read-Host '독립 관리자 승인 기록의 인증서 DER SHA-256').ToLowerInvariant()
$publicCert = [Security.Cryptography.X509Certificates.X509CertificateLoader]::LoadCertificateFromFile($cerPath)
try {
    $actualSha = [Convert]::ToHexString(
        [Security.Cryptography.SHA256]::HashData($publicCert.RawData)).ToLowerInvariant()
    if ($actualSha -cne $approvedSha -or $publicCert.HasPrivateKey) { throw '승인 공개 인증서 불일치' }
    $store = [Security.Cryptography.X509Certificates.X509Store]::new('Root', 'LocalMachine')
    try {
        $store.Open([Security.Cryptography.X509Certificates.OpenFlags]::ReadWrite)
        if ($store.Certificates.Find('FindByThumbprint', $publicCert.Thumbprint, $false).Count) {
            throw '이미 등록되어 있습니다. 기존 항목을 검토하세요.'
        }
        $store.Add($publicCert)
    } finally { $store.Dispose() }
} finally { $publicCert.Dispose() }
```

### 3. 검증기 연결 및 staging 인계

- 별도 관리자 승인 기록의 DER·SPKI 지문을 `StageAuthenticode.cs`의 `ApprovedCertificateSha`와 `StageAdmission.cs`의 `ApprovedSignerSha`에 각각 봉인합니다. 보호된 승인 배포 경로와 상승 전 실행 허용 정책을 확정하고, 기존 검증기 릴리스의 실제 디지털 서명을 독립 검증합니다. 작업트리와 기존 초안은 운영 배포 경로로 승인되지 않았습니다.
- Authenticode 서명 완료 후 확정한 원본 바이트로 SHA 및 RSA-PSS 분리 서명을 발급합니다. 실제 승인 서명본 성공 경로를 쓰기 없이 검증한 뒤 최초 보호 원장 생성과 HANDLE readback의 별도 승인을 받습니다.
- 관리 계정을 재검토하고 새 계약·시간창·원장 바인딩을 신규 단발 승인에 묶습니다. 그 뒤 실제 staging 생성과 readback을 별도로 승인합니다. 기존 7 PASS 유지, 재실행 0회. NO_GO / MANUAL / EXECUTION_HOLD.

공식 근거 [인증서 발급 및 키 정책](https://learn.microsoft.com/en-us/powershell/module/pki/new-selfsignedcertificate?view=windowsserver2025-ps) · [공개 인증서 내보내기](https://learn.microsoft.com/en-us/powershell/module/pki/export-certificate)

## 운영 공개 인증서 연결 및 서명 대상 HOLD

승인한 공개 CER 파일 SHA-256과 인증서 DER SHA-256이 일치합니다. Windows 지문 및 LocalMachine My·Root·TrustedPublisher 사본을 대조했습니다. 인증서 DER SHA-256과 RSA SPKI SHA-256을 각각 소문자 64자리로 기존 코드에 고정했고 기존 cutoff는 유지했습니다. 관리형 컴파일 경고·오류 0입니다. 기존 시험은 실행하지 않았습니다.

Windows SDK의 Microsoft 서명이 유효한 SignTool을 확인했습니다. 승인된 검증기 릴리스나 정확한 서명 대상 이미지 목록이 확인되지 않았습니다. 테스트·초안·기존 배포 이미지는 서명하지 않았으며 실제 서명자 확인 및 서명본 변조·다른 인증서 거부 시험은 미실행입니다.

다음 승인 순서는 검증기 릴리스 및 서명 대상 파일·보호 배포 경로 확정 → 격리 복사본 서명 및 파일별 확인 → 운영 정책에서 상승 전 독립 검증 확인 → 최초 원장의 별도 보호 생성·readback입니다. 최초 원장 생성은 기존 경로가 있으면 중단하고, 승인 관리자·조상을 확인한 뒤 관리자 전용 비상속 descriptor로 원자 생성합니다. 동일 HANDLE에서 경로·owner·DACL·파일 키를 확인한 뒤 새 계약과 관리자 계정 재검토를 거친 신규 승인에 파일 키를 묶습니다. 이후 staging 생성에 별도 승인이 필요합니다. 실제 관리자 작업과 ACL·원장 변경 없음. NO_GO / MANUAL / EXECUTION_HOLD.

## 독립 운영 검증기 릴리스 후보

- 소스 커밋 `3f674efb1be6466d57aa99f017563fd29ba62b7c`, 버전 `0.1.0+3f674efb1be6466d57aa99f017563fd29ba62b7c`. 기존 승인·Authenticode·동일 HANDLE SHA·보호 원장 검증을 별도 Windows x64 단일 EXE의 읽기 전용 진입점에 연결했습니다. 기존 .NET 10 런타임을 사용하고 운영 설치 명령은 노출하지 않습니다.
- 로컬 후보 `Temp/DiscordUsageDiagnostics20261026/StagingVerifierRelease-3f674efb/candidate/Limitless.StagingVerifier.exe`. 미서명 SHA-256 `145f3921e88ab2caba98afd4c5bb39eb7f9706aa6ad5e3fca5d5891f9f73adbb`. 신규 6 PASS / 0 FAIL로 버전, 설치 명령·잘못된 SHA 형식·미서명·만료·다른 유효 서명자 거부를 확인했습니다.
- 공식 SignTool은 승인 인증서를 선택할 수 없었고 현재 계정에서 개인키 열기는 0x80090016으로 실패했습니다. 기존 권한으로 키를 열 수 있는 지정 서명 계정에서만 후보 SHA를 먼저 확인하고 서명합니다. 서명본 검증과 변조 시험은 미실행입니다. 인증서·키 재발급, 권한 변경, 기존 시험 재실행 없음.
- 선행 배포 경로 후보는 `C:\ProgramData\LimitlessDiscordV2Verifier`입니다. 고정 경로와 조상의 reparse·소유자·DACL·최종 경로를 확인합니다. 소유자는 Administrators, 보호 DACL은 SYSTEM·Administrators에 전체 제어와 파일 상속을 부여하고 지정된 사전 검증 계정에는 필요한 경우 읽기·실행만 부여합니다. 일반 사용자 쓰기·추가·삭제·ACL 변경 권한을 허용하지 않습니다. 승인 파일 SHA·서명자 및 상승 전 Windows 허용 정책을 별도로 연결합니다. 이 경로 또는 ACL을 생성·변경하지 않았습니다. 실제 원장·staging·Bootstrap 실행은 별도 승인입니다. NO_GO / MANUAL / EXECUTION_HOLD.

## 독립 검증기 서명 계정 사전 점검

- 기존 후보 SHA-256 일치 및 `NotSigned` 확인. 지정 인증서는 `LocalMachine\My`에서 지문 일치, 코드서명 EKU, 키 연결 표시와 유효기간을 확인했습니다. 유효기간 KST 2026-11-21 08:20:56부터 2027-11-21 08:30:56까지.
- 현재 비상승 토큰에서 RSA 개인키 열기 실패 `0x80090016`. 코드 자체로 키 컨테이너 부재와 권한 거부를 구분할 수 없습니다. 지정 서명 계정의 정확한 계정과 토큰은 로컬 승인 기록에서 대조해야 하며, 현재 세션에서 지정 계정의 키 접근은 확인하지 않았습니다.
- 지정 서명 계정에서 관리자 PowerShell로 현재 계정이 승인 기록과 맞는지 확인한 뒤, 같은 EXE 해시·인증서 지문·유효기간·코드서명 EKU를 대조합니다. 같은 인증서를 `GetRSAPrivateKey`로 열 수 있고, RSA 키가 인증서 SPKI와 일치하며 기존 내보내기 불가 정책을 유지하는지 확인합니다. 확인 단계에서는 서명하지 않습니다. 실패하면 중단하고 ACL·인증서 저장소·키 연결을 변경하지 않습니다.
- 사전 점검이 모두 맞고 별도 서명 승인을 받은 경우에만 기존 Microsoft 서명 SDK SignTool로 명시된 EXE 한 개를 서명합니다. 서명 뒤 Authenticode·서명자 지문을 검증하고 서명 후 SHA-256을 미서명 해시와 별도로 기록합니다. 서명본 변조·다른 서명자 거부 시험과 운영 배포는 후속 승인 단계입니다. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7054226849).

## 승인 키 연결 및 접근 실패 분류

- EXE SHA 및 승인 인증서 지문은 유지됩니다. `LocalMachine\My`에는 승인 인증서와 영구 키 연결 속성 `CERT_KEY_PROV_INFO`가 있고, `CurrentUser\My`에는 같은 인증서가 없습니다. 연결은 Microsoft Software Key Storage Provider의 명명된 머신 키 컨테이너를 가리킵니다.
- `NCryptOpenStorageProvider` 성공, 연결 속성 그대로 머신·무인 읽기 전용 `NCryptOpenKey` 호출은 `0x80090010 (NTE_PERM)`입니다. 이전 상위 API의 `0x80090016`보다 현재 비상승 토큰의 접근 거부를 직접 보여 줍니다. 컨테이너의 실재, 키 공개 부분과 인증서의 일치, 지정 서명 계정의 기존 권한은 미확인입니다.
- 최소 경로는 바울 대표가 승인한 기존 서명 계정의 토큰에서 같은 머신 컨테이너의 무인 열기를 읽기 전용으로 재확인하는 것입니다. 성공하면 공개 SPKI와 내보내기 불가 정책을 대조합니다. 접근 거부가 계속되면 지정 계정의 기존 권한만 관리자와 검토하고, 키 부재 또는 공개키 불일치가 드러나면 HOLD합니다. ACL·키 연결·인증서 변경과 실제 서명은 이번 범위 밖입니다. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7054326187).

## 지정 서명 계정의 읽기 전용 검증

- 승인 후보 EXE SHA, 인증서 Windows 지문과 DER SHA 일치를 재확인했습니다. 현재 실행 토큰의 SID는 조회 가능하고 관리자 역할은 비활성입니다. 외부에 기록할 승인 SID는 없으며 승인 기록의 서명 계정 SID와 현재 토큰의 동일 여부는 미확인입니다. 현재 토큰의 CNG 접근 거부 결과를 재사용했습니다.
- 지정 계정의 기존 토큰에서 보호된 로컬 승인 기록의 SID를 대조한 뒤 새 PowerShell 7 세션에서 아래 명령을 실행합니다. 스크립트 소스는 이 커밋의 검토된 사본을 사용합니다. `$approvedSid`에는 로컬 승인 기록 값, `$candidate`에는 확인할 후보 EXE의 절대 경로를 설정합니다. SID·컨테이너 이름·키 데이터를 Issue·로그에 올리지 않습니다.

```powershell
pwsh -NoProfile -File '.\Tools\DiscordV2\StagingDraft\Test-ApprovedSigningKey.ps1' -ApprovedSignerSid $approvedSid -CandidatePath $candidate
```

- 스크립트는 일치하는 SID에서만 실행하고, 기존 KSP 연결의 머신 컨테이너를 무인 모드로 엽니다. 인증서 DER·유효기간·EKU, 실제 키의 공개 SPKI와 비내보내기 정책을 대조합니다. 서명, 저장소/ACL 변경, 개인키 복사·내보내기 없음. 파서·SID 불일치 조기 거부·네이티브 어댑터 컴파일만 확인했습니다. 지정 계정의 성공 경로는 아직 미실행입니다.
- `PASS_SIGNER_SID_KEY_SPKI_READ_ONLY` 확인 후 별도 서명 승인이 있을 때만 같은 계정에서 EXE 해시 재대조 및 SignTool 서명으로 이어집니다. `HOLD_KEY_OPEN` 또는 SPKI 불일치는 운영 HOLD를 유지합니다.

## 기존 로컬 계정의 서명 승인 준비

- 후보 계정의 실제 SID를 로컬 사용자 정보에서 확인했습니다. 활성 콘솔·최근 대화형 로그인·현재 토큰과 모두 일치했고 현재 토큰의 관리자 역할은 비활성입니다. 확인 결과는 계정 승인, 키 접근 성공, ACL 원인의 확정을 뜻하지 않습니다. SID는 기록하지 않습니다.
- **대표 지정 전에는 미승인**입니다. 실제 기록은 쓰기 권한이 제한된 기존 승인 로컬 저장소를 대표가 지정한 뒤에만 작성합니다. 보호가 확인되지 않으면 기록을 발급하지 않습니다. 항목은 지정 계정 및 실제 SID, 승인 주체·시각, 인증서 Windows 지문·DER SHA-256·RSA SPKI SHA-256, 후보 EXE 경로·미서명 SHA-256, 검사 스크립트 소스 커밋·실제 파일 SHA-256입니다. 지문과 후보 해시는 기존 구현의 승인값을 대조합니다. 현재 검사 스크립트 SHA-256은 `ab57366eef3eefe1e74c6f50cebf440c815d88e6ef8aada2ffa66c616448b24f`입니다. 실제 승인서나 SID가 포함된 파일은 이 작업에서 만들지 않았습니다.
- 계정 지정 뒤 **읽기 전용 검사 1회의 별도 승인**에 작업 ID·시간창·허용 실행 토큰을 적습니다. 기존 정상 로그인에서 승인된 토큰을 사용하고, 승인 기록의 SID와 실제 토큰 및 검사 스크립트·후보 해시를 재대조합니다. 보호된 승인 기록에서 미사용 상태를 확인한 뒤 같은 ID의 시작 시각을 먼저 남깁니다. 실행 주체는 한 명이고, 시작 기록이 있으면 결과가 없거나 실패해도 같은 승인으로 재실행하지 않습니다. 시작을 기록할 수 없으면 실행을 보류합니다.
- 검토한 PowerShell 7 세션에서 `& $approvedScript -ApprovedSignerSid $approvedSid -CandidatePath $candidate`를 한 번만 호출합니다. 세 변수는 보호된 승인 기록에서 로컬로 읽습니다. `PASS_SIGNER_SID_KEY_SPKI_READ_ONLY`가 나오면 검사 결과를 기록합니다. 그 외 결과나 중단은 HOLD입니다. 이 1회 검사는 서명 승인과 별개입니다.
- 검사 PASS 후 서명에 필요한 별도 승인에는 같은 계정/SID, 동일 인증서 지문과 후보 EXE SHA-256, Microsoft 서명 SDK SignTool, 서명할 격리 후보 한 개를 묶습니다. 그 전에는 서명과 배포가 금지됩니다. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7054692478).

## 보호 승인 저장소 최초 생성 승인 범위

- 기존 승인 경로와 후보 `C:\ProgramData\LimitlessDiscordV2Verifier`는 조회 시 부재입니다. C:는 고정 NTFS이고 C: 루트와 ProgramData는 reparse가 아닙니다. 현재 ACL의 직접 적용 규칙에서 비관리자의 조상 대체·삭제 또는 권한 변경을 허용하는 항목은 확인되지 않았습니다. 하위 디렉터리 생성 허용 규칙은 있어 이름 선점이 가능합니다. 이 확인은 실제 실행 때 검증할 부모 HANDLE과 효과 권한을 대신하지 않습니다.
- 최초에는 승인 저장소 **후보 한 디렉터리만** 생성합니다. owner Administrators, group SYSTEM, 상속 차단 DACL, SYSTEM과 Administrators 전부 허용을 초기 descriptor `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`로 전달합니다. 하위 승인 파일은 추후 별도 승인 범위에서 상속에 의존하지 않고 보호 descriptor와 CREATE_NEW로 작성합니다. 일반 사용자의 쓰기·하위 추가·삭제·ACL 변경 권한은 허용하지 않습니다. 기존 상위 ACL과 다른 후보 경로를 건드리지 않습니다.
- 독립 신뢰 출발점의 후보는 Microsoft Authenticode가 유효한 Windows 내장 PowerShell 절대 경로입니다. 확인 결과 해당 호스트는 Microsoft 서명 Valid였습니다. 관리자가 보호된 시스템 경로·서명 및 실행 정책을 상승 전에 확인하고, 최소 생성 입력은 검토된 확정 바이트를 별도 승인 채널의 SHA-256과 대조한 뒤 동일 메모리에서 사용합니다. 프로필 및 외부 작업트리 의존을 배제하고 시스템 API만 사용합니다. 호스트 서명은 생성 입력을 승인하지 않습니다. 미서명 검증기는 이 절차에 사용하지 않습니다. 정확한 생성 입력과 해시를 확정하는 작업은 실제 승인 요청에 선행합니다.
- 실행 범위는 승인 계정의 정상 UAC 상승과 비위장 토큰 확인 → 고정 NTFS와 조상 HANDLE의 최종 경로·볼륨·file key·owner/DACL 검증 및 유지 → 후보 부재 재확인 → 초기 descriptor를 지정한 CreateDirectoryW 단발 → 동일 디렉터리 HANDLE로 최종 경로·reparse·owner/group·비상속 DACL·file key 확인입니다. OS가 기존 항목 또는 경합을 보고하거나 readback이 실패하면 중단합니다. 기존 경로를 채택하거나 ACL을 수정하거나 부분 결과를 삭제하지 않습니다.
- 대표 별도 승인의 최소 범위는 승인 계정·호스트·확정 입력 해시·대상 단일 경로·보안 descriptor·시간창·1회 생성 및 readback입니다. 계정 승인 기록 작성, 원장·staging 생성, 검증기 배포·키 검사·서명은 범위 밖입니다. 폴더 또는 ACL 변경은 이번에 없었습니다. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7054838675).

공식 근거 [CreateDirectory 및 SECURITY_ATTRIBUTES](https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-createdirectory)

## 보호 승인 저장소 최종 입력과 실행 명령

입력은 `Tools/DiscordV2/StagingDraft/Prepare-VerifierApprovalStore.ps1`입니다. SHA-256은 **8e54b904e70f14d8b5444593f1ee1840c4ad41c81f3a396a68e53f6ca9ab65f0**입니다. `VerifierStorePreparation.cs`에서 미리 컴파일한 DLL을 내장했으므로 관리자 세션에서 컴파일하거나 외부 DLL을 열 필요가 없습니다. 소스나 개행을 고치면 새로운 승인 대상입니다.

### 상승 전 독립 검증

기존 승인 채널의 SHA-256과 아래 출력이 일치해야 합니다. 결과는 로컬에서 대조합니다. 검증할 스크립트 자체에서 승인 해시를 읽지 않습니다. 관리자 실행은 생성 작업에 대한 별도 승인 및 승인 시간창 발급 후에만 진행합니다.

```powershell
$hostExe = 'C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe'
$inputFile = 'F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordV2\StagingDraft\Prepare-VerifierApprovalStore.ps1'
$hostSignature = Get-AuthenticodeSignature -LiteralPath $hostExe
if ($hostSignature.Status -ne 'Valid' -or
    $hostSignature.SignerCertificate.Subject -notmatch '(^|,\s*)O=Microsoft Corporation(,|$)') {
    throw 'HOLD_HOST_SIGNATURE'
}
(Get-FileHash -LiteralPath $inputFile -Algorithm SHA256).Hash
```

### 별도 승인 후 관리자 실행

지정 계정의 정상 로그인에서 승인 시간창 안에 아래 명령으로 UAC 상승을 요청합니다. 이 명령은 저장소를 생성하지 않고 확인된 내장 호스트만 엽니다.

```powershell
Start-Process -FilePath 'C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe' -Verb RunAs -ArgumentList '-NoLogo','-NoProfile'
```

승인 SID·시작·종료는 독립 로컬 승인 기록에서 조회합니다. 아래 블록 실행 전 해당 승인 ID가 미사용인지 확인하고 1회 시작을 기록합니다. 승인 해시는 별도 채널에서 대조한 값을 입력합니다. 스크립트 바이트를 한 HANDLE에서 제한 크기로 읽고 검증한 메모리에서 실행합니다. 경로를 다시 열어 실행하지 않습니다.

```powershell
$ErrorActionPreference = 'Stop'
$inputFile = 'F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordV2\StagingDraft\Prepare-VerifierApprovalStore.ps1'
$approvedSha = (Read-Host '독립 승인 기록의 스크립트 SHA-256').ToUpperInvariant()
if ($approvedSha -cnotmatch '^[A-F0-9]{64}$') { throw 'HOLD_APPROVAL_SHA_FORMAT' }
$approvedSid = Read-Host '로컬 승인 기록의 SID'
$approvedStart = Read-Host '승인 시작 yyyy-MM-ddTHH:mm:ss+09:00'
$approvedEnd = Read-Host '승인 종료 yyyy-MM-ddTHH:mm:ss+09:00'

$stream = [IO.FileStream]::new($inputFile, [IO.FileMode]::Open, [IO.FileAccess]::Read, [IO.FileShare]::Read)
try {
    if ($stream.Length -lt 1 -or $stream.Length -gt 65536) { throw 'HOLD_INPUT_SIZE' }
    $bytes = New-Object byte[] ([int]$stream.Length)
    for ($offset = 0; $offset -lt $bytes.Length; $offset += $read) {
        $read = $stream.Read($bytes, $offset, $bytes.Length - $offset)
        if ($read -le 0) { throw 'HOLD_INPUT_READ' }
    }
    $sha = [Security.Cryptography.SHA256]::Create()
    try { $actual = [BitConverter]::ToString($sha.ComputeHash($bytes)).Replace('-', '') }
    finally { $sha.Dispose() }
    if ($actual -cne $approvedSha) { throw 'HOLD_INPUT_SHA' }
    $verifiedText = [Text.UTF8Encoding]::new($false, $true).GetString($bytes)
    & ([ScriptBlock]::Create($verifiedText)) -ApprovedSignerSid $approvedSid -ApprovedStart $approvedStart -ApprovedEnd $approvedEnd
} finally { $stream.Dispose() }
```

`STORE_CREATED_HANDLE_VERIFIED`의 파일 ID와 descriptor SHA는 보호된 로컬 경로에서만 보관합니다. 기존 항목, 권한 불일치, 경합, 검증 실패 또는 일부 생성이 발견되면 HOLD하며 재실행·삭제·수선하지 않습니다. 승인 기록 작성과 키 검사는 이후 별도 단계입니다. 관리형 컴파일, 내장 DLL 바이트 일치, Windows PowerShell 5.1 구문, 실제 조상 HANDLE 읽기만 확인했습니다. 생성 함수 미호출, 대상 여전히 부재. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7054941208).

## 보호 승인 기록 1회 작성 실행 자료

최초 저장소는 대표가 생성하고 HANDLE 검증을 완료했습니다. 이전 기록 작성 시도는 Codex 토큰에서 ACL 조회가 거부되어 파일 생성 호출 전 종료됐습니다. 해당 1회 승인은 생성 호출로 소비되지 않았습니다. 현재 지시에 따라 대표의 관리자 PowerShell 5.1에서만 1회 진행합니다.

실행 입력 `Tools/DiscordV2/StagingDraft/Write-VerifierSigningDesignation.ps1`, SHA-256 `2019653b99997fe46ce892c320f05cfa792080316c4bda6be299fc4cc41f0482`. 기존 저장소 생성 스크립트 및 검사 스크립트는 변경하지 않았습니다. Windows PowerShell 5.1 구문과 두 가지 해시 입력, 관리형 컴파일과 저장소 생성 진입점 미포함을 확인했습니다. 실제 기록 생성은 미실행입니다.

새 내장 5.1 호스트에서 입력 바이트의 고정 해시를 대조하고 같은 바이트를 실행합니다. 보호 저장소 생성 성공 출력의 `=` 뒤 값을 로컬 입력으로 사용합니다. 확인된 Microsoft 호스트·지정 계정 SID·상승 토큰, 부모 보호, 저장소 HANDLE 파일 ID·descriptor SHA와 원래 성공 출력을 대조합니다. 잠근 후보 EXE 및 검사 스크립트 SHA와 인증서 DER를 대조합니다. 보관하는 SPKI 지문은 해당 정확한 DER에 대해 이미 확인한 값입니다. 승인 범위는 계정 지정이며 개인키 검사는 수행하지 않습니다.

`SigningAccountDesignation.v1.json`은 고정 경로의 관리자 전용 초기 descriptor와 CREATE_NEW로 한 번만 만듭니다. 기존 파일·부분 파일은 보존합니다. flush 후 동일 파일 HANDLE에서 바이트와 ACL을 읽고 부모·저장소를 다시 확인한 뒤에만 `DESIGNATION_CREATED_HANDLE_VERIFIED`를 반환합니다. 오류가 나면 다시 실행하거나 파일을 삭제·수선하지 않습니다. SID·저장소 식별값·결과 SHA는 로컬에만 보관합니다. NO_GO / MANUAL / EXECUTION_HOLD.

## 승인 기록을 확인하는 키 검사 1회 실행 자료

대표가 보호 승인 기록을 생성하고 HANDLE 검증을 완료했습니다. 별도 읽기 전용 1회 승인을 받았습니다. Codex 토큰은 비상승이며 고객의 기존 관리자 PowerShell 창에서만 실행합니다. 공식 서명이 유효한 고정 PowerShell 7 경로를 사용합니다.

실행 입력 `Tools/DiscordV2/StagingDraft/Invoke-VerifierSigningKeyCheckOnce.ps1`, SHA-256 `5423a41b681771fba5f39a46ee4d09371c258d613f4b17811b3a5d5a789ea291`. 보호 저장소 생성과 승인 기록 작성의 성공 출력 각각의 `=` 뒤 값을 로컬에서 입력합니다. 동일 HANDLE에서 최종 경로·조상·저장소·승인 파일 owner/DACL·원문 해시를 확인하고, 기록에 봉인된 계정 SID가 상승 토큰과 일치한 뒤에만 키 검사로 넘어갑니다. 기존 검사 스크립트의 SHA-256을 확인해 동일 메모리 바이트에서 한 번 실행합니다. 기록·키·ACL을 변경하는 진입점은 없습니다. 실패하면 재시도하지 않습니다.

관리형 컴파일, 실행 입력과 기존 검사 스크립트의 구문, 읽기 전용 진입점의 범위를 확인했습니다. 실제 키 열기·SPKI 검사는 아직 실행하지 않았습니다. 성공 표식은 `PASS_SIGNER_SID_KEY_SPKI_READ_ONLY`입니다. SID 및 로컬 결과 해시는 게시하지 않습니다. NO_GO / MANUAL / EXECUTION_HOLD.

## 고정 독립 검증기 한 파일의 서명 준비

- 원본 후보 `StagingVerifierRelease-3f674efb/candidate/Limitless.StagingVerifier.exe`는 `NotSigned`이며 승인 기록과 동일한 SHA-256 `145f3921e88ab2caba98afd4c5bb39eb7f9706aa6ad5e3fca5d5891f9f73adbb`입니다. 승인 인증서의 Windows 지문과 유효기간을 확인했고, 지정 계정의 앞선 읽기 전용 검사 PASS를 재사용했습니다.
- 확인한 Microsoft SDK SignTool의 SHA-256은 `0d0189af3356554a462c2a60d0d59f6006b68d743f5c0cc93c530b083706f075`입니다. 후보와 SDK 위치에는 승인 주체 외 쓰기 허용 ACE가 관측됐습니다. 해당 경로에서 관리자 서명을 진행하지 않습니다. 기존 ACL을 수정하지 않고, 다음 승인에서 보호된 작업 복사본·SignTool 및 필요한 SDK 종속 파일의 실행 위치를 지정합니다. 보호 승인 저장소와 승인 기록은 그대로 보존합니다.
- 별도 승인의 대상은 지정 계정, 후보 원본 SHA와 보호 복사본 한 개, 승인 인증서 지문, SDK 바이너리 SHA 및 보호 실행 경로, 외부 타임스탬프 없는 로컬 서명 한 번입니다. 보호 경로의 조상·owner·DACL·최종 경로 확인과 승인 복사본의 같은 SHA 확인이 선행합니다. 전후 SHA는 별개 값입니다.

### 보호 경로 확정 및 별도 승인 이후에만 실행

승인 기록의 SID와 같은 실행 토큰을 사용합니다. `$tool`과 `$target`은 승인된 보호 절대 경로입니다. 기존 후보에서 바이트가 동일한 작업 복사본 한 개를 사용하고 해당 위치의 교체·쓰기 조건을 검증한 다음 실행합니다. 아래는 서명 명령과 후속 판정입니다.

```powershell
$ErrorActionPreference = 'Stop'
$expectedBefore = '145f3921e88ab2caba98afd4c5bb39eb7f9706aa6ad5e3fca5d5891f9f73adbb'
$expectedTool = '0d0189af3356554a462c2a60d0d59f6006b68d743f5c0cc93c530b083706f075'
$thumb = '437423567AD4B703578F231E34A06592B38DB4EB'
$before = (Get-FileHash -LiteralPath $target -Algorithm SHA256).Hash.ToLowerInvariant()
$toolSig = Get-AuthenticodeSignature -LiteralPath $tool
if ($before -cne $expectedBefore -or (Get-AuthenticodeSignature -LiteralPath $target).Status -ne 'NotSigned' -or
    (Get-FileHash -LiteralPath $tool -Algorithm SHA256).Hash.ToLowerInvariant() -cne $expectedTool -or
    $toolSig.Status -ne 'Valid' -or $toolSig.SignerCertificate.Subject -notmatch '(^|,\s*)O=Microsoft Corporation(,|$)') {
    throw 'HOLD_SIGN_INPUT'
}

& $tool sign /sm /s My /sha1 $thumb /fd SHA256 $target
if ($LASTEXITCODE -ne 0) { throw 'HOLD_SIGN_NO_RETRY' }

& $tool verify /pa /all $target
if ($LASTEXITCODE -ne 0) { throw 'HOLD_AUTHENTICODE_VERIFY' }
$signature = Get-AuthenticodeSignature -LiteralPath $target
if ($signature.Status -ne 'Valid' -or $signature.SignerCertificate.Thumbprint -cne $thumb) {
    throw 'HOLD_SIGNER_THUMBPRINT'
}
$leaf = $signature.SignerCertificate
$publicKey = [Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPublicKey($leaf)
try {
    $der = $leaf.GetCertHashString([Security.Cryptography.HashAlgorithmName]::SHA256).ToLowerInvariant()
    $spki = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData(
        $publicKey.ExportSubjectPublicKeyInfo())).ToLowerInvariant()
    $eku = @($leaf.Extensions |
        Where-Object { $_ -is [Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension] } |
        ForEach-Object { $_.EnhancedKeyUsages } | ForEach-Object { $_.Value })
    $now = Get-Date
    if ($der -cne '7dba5310d6ded0035595e5e7ab8b8b73422f2925b49c633426995c322940672c' -or
        $spki -cne '2a6d0a7d31e7642df209b6cbe12fa628500663f540eb6bd557cac2aca2d31f7c' -or
        $eku -notcontains '1.3.6.1.5.5.7.3.3' -or $now -lt $leaf.NotBefore -or $now -ge $leaf.NotAfter) {
        throw 'HOLD_SIGNER_POLICY'
    }
} finally { if ($null -ne $publicKey) { $publicKey.Dispose() } }
$after = (Get-FileHash -LiteralPath $target -Algorithm SHA256).Hash.ToLowerInvariant()
if ($after -ceq $before) { throw 'HOLD_IMAGE_UNCHANGED' }
[pscustomobject]@{ Authenticode = 'PASS'; UnsignedSha256 = $before; SignedSha256 = $after }
```

현재 서명 및 서명 후 SHA는 NOT_RUN입니다. 실패하면 재시도·인증서 교체·ACL 변경을 하지 않고 산출물을 배포에서 제외합니다. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7055935996) · [Microsoft SignTool 문서](https://learn.microsoft.com/en-us/windows/win32/seccrypto/signtool). NO_GO / MANUAL / EXECUTION_HOLD.

## 대표 실행용 단발 보호 복사본 서명

대표의 서명 승인을 받았습니다. 입력은 `Invoke-VerifierSigningOnce.ps1`, SHA-256 `1b7388b915530c7f061b555bb03f2f62e5ff455a442c6547660c1676bd76277d`입니다. 원본과 기존 승인 저장소는 수정하지 않습니다. 지정 상승 토큰·보호 승인 원문 및 기존 로컬 receipt 두 개를 대조한 뒤에만 별도 신규 작업 공간 `C:\ProgramData\LimitlessDiscordV2SigningOnce`를 초기 관리자 전용 descriptor로 원자 생성합니다. 이미 있으면 중지합니다. 작업 공간은 서명 성공 여부와 관계없이 재시도에 재사용하지 않습니다.

같은 HANDLE에서 읽어 승인 SHA를 대조한 후보 및 공식 SDK SignTool 바이트를 두 보호 파일에 씁니다. SignTool은 보호 작업 폴더에서 System32 PATH로만 실행하고 SDK 원래 디렉터리의 DLL은 복사하지 않습니다. SDK 단독 실행의 `verify /pa /all`을 비관리자 격리 복사본에서 확인했습니다. 실제 서명은 해당 EXE만 `sign /sm /s My /sha1 <승인 지문> /fd SHA256`으로 한 번 수행합니다. 외부 타임스탬프 없이 로컬 서명합니다.

성공 표식 `SIGNING_AUTHENTICODE_VERIFIED`는 SignTool 검증, Windows Authenticode, 인증서 지문·DER·SPKI/EKU/유효기간, 보호 파일 HANDLE 및 서명 후 SHA 확인 뒤에만 출력합니다. 조상/작업 공간/파일 보호 불일치, 서명 또는 검증 실패는 HOLD입니다. 실패물을 수정·삭제·재시도하지 않습니다. 현재 실제 관리자 서명 NOT_RUN, 기존 키 검사 재실행 없음, 배포·ACL 변경 없음. NO_GO / MANUAL / EXECUTION_HOLD.

## 보호 복사본 서명 1회 결과

바울 대표의 관리자 PowerShell 실행 결과 `SIGNING_AUTHENTICODE_VERIFIED` 및 SignTool `verify /pa /all` 성공을 확인했습니다. 서명본은 `C:\ProgramData\LimitlessDiscordV2SigningOnce\Limitless.StagingVerifier.exe`입니다. 서명 입력 SHA-256 `145f3921e88ab2caba98afd4c5bb39eb7f9706aa6ad5e3fca5d5891f9f73adbb`, 서명 후 SHA-256 `86ba45f0139122f80c1cfb9492700edb9a537e63bd9ee511ac9231e34fb3d7ca`. SHA-256 Authenticode, 타임스탬프 없음. 실행 자료 `cd602656afd836d8dddcfca17d56f3ce41e95479`. 대표가 제공한 실행 결과를 기록했으며 Codex에서 서명·검증을 반복하지 않았습니다. 서명본 변조 시험, 운영 배포, 원장·staging 및 AI 실행은 별도 승인 대기입니다. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7056070665). NO_GO / MANUAL / EXECUTION_HOLD.

## 서명본 변조 방어 시험 준비 (실행 승인 대기)

- 보호 서명본은 이전 SHA와 경로를 보존합니다. 격리 폴더는 `Temp/DiscordUsageDiagnostics20261026/VerifierTamperFixtures-86ba45f0`입니다. 승인 미서명 후보 `unsigned.exe` 및 Microsoft 서명 SDK의 기존 파일 `foreign-signer.exe`를 복사해 각각 사전에 기록한 소스 SHA와 같음을 확인했습니다. 운영 파일을 변경하지 않았습니다.
- 보호된 서명본은 현재 비상승 토큰에서 읽기가 거부됩니다. 기존 관리자 세션에서 보호 서명본을 읽기 전용으로 열고 같은 메모리 바이트의 SHA-256 `86ba45f0139122f80c1cfb9492700edb9a537e63bd9ee511ac9231e34fb3d7ca`를 대조한 후, 격리 폴더에 `signed-baseline.exe`로 CREATE_NEW 복사하고 같은 해시를 readback합니다. 복사 단계에서 검증기·시험 입력을 실행하지 않습니다.
- 별도 시험 승인 후, `signed-baseline.exe`의 전체 파일 SHA와 Authenticode 정책을 재확인하고 유일한 검증기 실행 파일로 사용합니다. 기준본을 `--verify-image`로 검증해 `VERIFIED_IMAGE_ONLY` 확인 후 다음 세 시험을 각각 한 번 수행합니다. 시험 대상은 데이터로 전달합니다.
  - 변조는 기준본의 별도 `tampered.exe` 복사본에서 서명 범위가 확실한 PE 섹션 데이터 1바이트만 바꿉니다. 변경 바이트의 실제 SHA를 검증 인수로 사용해 단순 SHA 입력 불일치로 인한 거부를 배제합니다. 예상 `HOLD STAGE_AUTHENTICODE` 및 비정상 종료.
  - 미서명은 `unsigned.exe`의 원래 SHA를 넘깁니다. 예상 `HOLD STAGE_AUTHENTICODE` 및 비정상 종료.
  - 다른 서명자는 유효한 서명과 원래 SHA를 유지한 `foreign-signer.exe`를 넘깁니다. 예상 `HOLD STAGE_SIGNER_PIN` 및 비정상 종료.
- 원본·서명 기준 복사본을 변조하지 않고, 신뢰되지 않은 시험 실행 파일이나 변조본 자체를 실행하지 않습니다. 예상과 다른 결과가 나오면 즉시 중단하며 남은 케이스는 NOT_RUN입니다. 현재 변조·거부 시험 모두 NOT_RUN, 운영 배포·AI 실행 없음. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7056114329). NO_GO / MANUAL / EXECUTION_HOLD.

## 서명 검증 1회 실행 최종 승인 요청

격리 폴더 `Temp/DiscordUsageDiagnostics20261026/VerifierTamperFixtures-86ba45f0`에서 입력을 고정했습니다. 세 파일은 실제 SHA를 읽기 전용으로 확인했습니다. 네 번째 해시는 파일 생성 없이 메모리에서 계산했습니다. 기존 성공 복사 작업은 반복하지 않았습니다.

| 파일 | SHA-256 | 예상 결과 |
|---|---|---|
| signed-baseline.exe | `86ba45f0139122f80c1cfb9492700edb9a537e63bd9ee511ac9231e34fb3d7ca` | 종료 0 / VERIFIED_IMAGE_ONLY |
| tampered.exe (미생성, 예상 SHA) | `d6af9bfbe3c77136b200181135d6eed1e4f24ddac838624b476076e171ab9712` | 종료 1 / HOLD STAGE_AUTHENTICODE |
| unsigned.exe | `145f3921e88ab2caba98afd4c5bb39eb7f9706aa6ad5e3fca5d5891f9f73adbb` | 종료 1 / HOLD STAGE_AUTHENTICODE |
| foreign-signer.exe | `0d0189af3356554a462c2a60d0d59f6006b68d743f5c0cc93c530b083706f075` | 종료 1 / HOLD STAGE_SIGNER_PIN |

별도 승인 후에만 PE .text의 파일 오프셋 `0x420`을 `0x00 → 0x01`로 바꾼 새 복사본 한 개를 만듭니다. 수정 오프셋이 PE 인증서 테이블 밖의 서명 대상 섹션 안에 있음을 확인했습니다. 정상 서명 기준본은 Windows Authenticode 유효, 승인 인증서 DER 및 공개키 SPKI, 코드서명 용도 및 현재 유효기간, 같은 HANDLE의 전체 파일 SHA가 모두 일치해야 합니다. 각 부정 입력에는 그 입력 자체의 정확한 SHA를 넘겨 서명 정책의 거부를 분리합니다.

대표의 별도 승인 범위는 비관리자 격리 폴더에서 새 변조본 1개 생성 및 해당 기준본만 검증기로 실행하여 기준본 통과 1건과 거부 3건을 각각 한 번 검사하는 것입니다. 예상과 다른 결과가 나오면 즉시 HOLD하고 남은 건은 미실행으로 기록합니다. 실패 자동 재시도, 기준본·원본·보호 승인 기록 변경, 운영 배포·AI 없음. 현재 실제 결과는 모두 NOT_RUN입니다. [Issue #66](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-7056199196). NO_GO / MANUAL / EXECUTION_HOLD.
