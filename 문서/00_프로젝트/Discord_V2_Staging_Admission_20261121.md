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
