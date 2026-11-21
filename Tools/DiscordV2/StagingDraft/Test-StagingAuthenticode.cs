internal static class StagingAuthenticodeTests
{
    internal static int Run()
    {
        const string certificate = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        const string key = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb";
        var now = BootstrapPolicy.Cutoff;
        int passed = 0;
        void Test(string name, Action action, string? rejection = null)
        {
            try
            {
                action();
                if (rejection is not null) throw new Exception($"Expected {rejection}");
            }
            catch (InvalidOperationException e) when (e.Message == rejection) { }
            Console.WriteLine($"PASS {name}");
            passed++;
        }
        void Check(string cert, string publicKey, bool codeSigning, DateTimeOffset at)
            => StageSecurityApi.ValidateLocalSigner(cert, certificate, publicKey, key, codeSigning,
                at, now.AddDays(-1), now.AddDays(1));

        Test("approved-cert-and-spki-code-signing-window", () => Check(certificate, key, true, now));
        Test("deny-different-certificate-with-same-key", () => Check(key, key, true, now), "STAGE_SIGNER_PIN");
        Test("deny-different-spki", () => Check(certificate, certificate, true, now), "STAGE_SIGNER_PIN");
        Test("deny-missing-code-signing-eku", () => Check(certificate, key, false, now), "STAGE_SIGNER_USAGE_OR_TIME");
        Test("deny-expired-or-future-certificate", () => Check(certificate, key, true, now.AddDays(1)), "STAGE_SIGNER_USAGE_OR_TIME");
        Test("deny-not-yet-valid-certificate", () => Check(certificate, key, true, now.AddDays(-2)), "STAGE_SIGNER_USAGE_OR_TIME");
        using var source = new FileStream(Path.GetFullPath("Tools/DiscordV2/StagingDraft/Verify-StagingSource.Loader.ps1"),
            FileMode.Open, FileAccess.Read, FileShare.Read);
        Test("windows-authenticode-denies-unsigned-source", () => StageSecurityApi.VerifyAuthenticode(source, certificate, key),
            "STAGE_AUTHENTICODE");
        Console.WriteLine($"{passed} PASS / 0 FAIL");
        return 0;
    }
}
