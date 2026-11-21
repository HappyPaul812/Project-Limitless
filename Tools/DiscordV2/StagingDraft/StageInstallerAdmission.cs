using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;

internal static partial class StageSecurityApi
{
    internal const string InstallerProfile = "DISCORD_V2_INITIAL_STAGING_INSTALL_V1";
    internal const string InstallerRoot = @"C:\ProgramData\LimitlessDiscordV2InstallerRuntime";
    internal const string InstallerImage = InstallerRoot + @"\Limitless.InitialStagingInstaller.exe";
    private const string TrustedVerifierRoot = @"C:\ProgramData\LimitlessDiscordV2VerifierRuntime";
    private const string TrustedVerifierImage = TrustedVerifierRoot + @"\Limitless.StagingVerifier.exe";
    private const string TrustedVerifierSha = "86ba45f0139122f80c1cfb9492700edb9a537e63bd9ee511ac9231e34fb3d7ca";
    private const string SystemRuntime = @"C:\Program Files\dotnet";

    internal sealed record InstallerContract(string InstallerSha256, InstallationApproval Approval);

    // 운영 경로에서는 고정 관리자 전용 위치의 입력과 기존 서명 검증기만 사용합니다.
    internal static string InstallProtectedStaging()
    {
        BootstrapPolicy.Need(string.Equals(Environment.ProcessPath, InstallerImage, StringComparison.OrdinalIgnoreCase),
            "STAGE_INSTALLER_PATH");
        string actor = WindowsBootstrapHost.Administrator();
        var account = new NTAccount(Environment.MachineName, "windows");
        BootstrapPolicy.Need(account.Translate(typeof(SecurityIdentifier)).Value == actor, "STAGE_ACTOR_MISMATCH");
        using var host = new WindowsBootstrapHost();
        BootstrapPolicy.AdministrativeSecurity(host.PinAndRead(InstallerRoot).Security, BootstrapPolicy.AdminSid);
        BootstrapPolicy.AdministrativeSecurity(host.PinAndRead(TrustedVerifierRoot).Security, BootstrapPolicy.AdminSid);
        using var image = OpenPinnedInput(host, InstallerImage);
        using var verifier = OpenPinnedInput(host, TrustedVerifierImage);
        const string sourcePath = InstallerRoot + @"\StagingSource.ps1";
        using var source = OpenPinnedInput(host, sourcePath);
        using var approval = OpenPinnedInput(host, InstallerRoot + @"\InstallApproval.json");
        using var approvalSignature = OpenPinnedInput(host, InstallerRoot + @"\InstallApproval.sig");
        using var sourceSignature = OpenPinnedInput(host, InstallerRoot + @"\StagingSource.sig");

        BootstrapPolicy.Need(HashImage(verifier) == TrustedVerifierSha, "STAGE_EXISTING_VERIFIER_SHA");
        string installerSha = HashImage(image);
        byte[] publicKey = VerifyAuthenticode(source, ApprovedCertificateSha, ApprovedSignerSha, sourcePath);
        source.Position = 0;

        return AdmitInstaller(source, ReadBounded(approval, 8192), ReadBounded(approvalSignature, 4096),
            ReadBounded(sourceSignature, 4096), publicKey, ApprovedSignerSha, installerSha, actor,
            () => DateTimeOffset.UtcNow, RequireExistingVerifier,
            (scope, demandWindow) => ObserveWindows(host, scope, demandWindow),
            (scope, demandWindow) => ConsumeWindowsApproval(host, scope, demandWindow),
            (scope, demandWindow) => CreateAndReadBack(host, scope, demandWindow));
    }

    internal static string AdmitInstaller(Stream source, byte[] approvalBytes, byte[] approvalSignature,
        byte[] sourceSignature, byte[] publicKey, string signerSha, string installerSha, string actor,
        Func<DateTimeOffset> now, Action<string> verifyInstaller,
        Action<InstallationApproval, Action> observe, Action<InstallationApproval, Action> consume,
        Action<InstallationApproval, Action> create)
    {
        using var signer = OpenApprovalSigner(publicKey, signerSha);
        var contract = ReadSignedApproval<InstallerContract>(approvalBytes, approvalSignature, signer);
        var scope = contract.Approval ?? throw new InvalidOperationException("STAGE_APPROVAL_SCOPE");
        CheckApprovalScope(scope, InstallerProfile);
        BootstrapPolicy.Need(contract.InstallerSha256 is not null && BootstrapPolicy.Sha(contract.InstallerSha256)
            && contract.InstallerSha256 == installerSha, "STAGE_INSTALLER_SHA");
        BootstrapPolicy.Need(scope.AdministratorSid == actor, "STAGE_ACTOR_MISMATCH");

        void DemandWindow()
        {
            DateTimeOffset time = now();
            BootstrapPolicy.Need(scope.NotBefore != default && scope.NotBefore.Offset == TimeSpan.Zero
                && scope.Expires.Offset == TimeSpan.Zero && scope.NotBefore <= time && time < scope.Expires
                && scope.Expires - scope.NotBefore <= TimeSpan.FromMinutes(5), "STAGE_INSTALL_WINDOW");
        }

        DemandWindow();
        _ = VerifySignedSource(source, scope.SourceSha, signer, sourceSignature);
        verifyInstaller(installerSha);
        observe(scope, DemandWindow);
        DemandWindow();
        // 원장이 승인을 선점한 뒤 실패해도 기록과 부분 결과를 보존합니다.
        consume(scope, DemandWindow);
        observe(scope, DemandWindow);
        DemandWindow();
        create(scope, DemandWindow);
        return "STAGE_CREATED_AND_READBACK";
    }

    private static FileStream OpenPinnedInput(WindowsBootstrapHost host, string path)
    {
        var file = host.ProtectedFile(path);
        // 원본 HANDLE은 host가 계속 유지합니다. 스트림은 동일 HANDLE을 빌려 읽습니다.
        return new FileStream(new SafeFileHandle(file.DangerousGetHandle(), ownsHandle: false), FileAccess.Read);
    }

    private static byte[] ReadBounded(Stream stream, int maxSize)
    {
        BootstrapPolicy.Need(stream.Length > 0 && stream.Length <= maxSize, "STAGE_INSTALL_INPUT");
        var bytes = new byte[stream.Length];
        stream.ReadExactly(bytes);
        return bytes;
    }

    private static string HashImage(FileStream stream)
    {
        BootstrapPolicy.Need(stream.Length is > 0 and <= 67108864, "STAGE_IMAGE_SIZE");
        return Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();
    }

    private static void RequireExistingVerifier(string approvedInstallerSha)
    {
        // 별도 보호 경로의 기존 서명 검증기가 설치 이미지 서명과 동일 HANDLE SHA를 독립 확인합니다.
        // 만료된 --review 계약은 재사용하지 않습니다. 새 승인·원본·원장은 설치 경계가 자체 검증합니다.
        var launch = new ProcessStartInfo(TrustedVerifierImage)
        {
            WorkingDirectory = TrustedVerifierRoot,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        launch.ArgumentList.Add("--verify-image");
        launch.ArgumentList.Add(InstallerImage);
        launch.ArgumentList.Add(approvedInstallerSha);
        launch.Environment.Clear();
        launch.Environment["SystemRoot"] = @"C:\Windows";
        launch.Environment["WINDIR"] = @"C:\Windows";
        launch.Environment["SystemDrive"] = "C:";
        launch.Environment["PATH"] = @"C:\Windows\System32";
        launch.Environment["DOTNET_ROOT"] = SystemRuntime;
        launch.Environment["DOTNET_ROOT_X64"] = SystemRuntime;
        launch.Environment["DOTNET_ROLL_FORWARD"] = "LatestPatch";

        using var process = Process.Start(launch) ?? throw new InvalidOperationException("STAGE_VERIFIER_START");
        var output = process.StandardOutput.ReadToEndAsync();
        var errors = process.StandardError.ReadToEndAsync();
        if (!process.WaitForExit(20000))
        {
            process.Kill(entireProcessTree: true);
            throw new InvalidOperationException("STAGE_VERIFIER_TIMEOUT");
        }
        BootstrapPolicy.Need(process.ExitCode == 0 && output.GetAwaiter().GetResult().Trim() == "VERIFIED_IMAGE_ONLY"
            && errors.GetAwaiter().GetResult().Trim().Length == 0, "STAGE_EXISTING_VERIFIER_DENIED");
    }
}
