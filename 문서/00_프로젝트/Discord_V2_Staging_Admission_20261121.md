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
