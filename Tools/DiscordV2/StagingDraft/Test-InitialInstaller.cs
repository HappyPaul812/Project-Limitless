using System.Security.Cryptography;
using System.Text.Json;

internal static class InitialInstallerFixture
{
    internal static int Main()
    {
        using var signer = RSA.Create(2048);
        byte[] publicKey = signer.ExportSubjectPublicKeyInfo();
        string pinnedKey = BootstrapPolicy.Hash(publicKey);
        string installerSha = new string('a', 64);
        byte[] source = "fixture only"u8.ToArray();
        byte[] Sign(byte[] bytes) =>
            signer.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
        byte[] sourceSignature = Sign(source);
        var now = new DateTimeOffset(2030, 1, 1, 0, 1, 0, TimeSpan.Zero);
        var scope = new StageSecurityApi.InstallationApproval(
            StageSecurityApi.InstallerProfile, Guid.NewGuid().ToString("N"), StageSecurityApi.Stage,
            StageSecurityApi.InitialLedger, "synthetic-ledger", "synthetic-admin", now.AddMinutes(-1),
            now.AddMinutes(1), BootstrapPolicy.Hash(source), ["root", "parent"], [pinnedKey, pinnedKey]);
        int passed = 0;

        void Test(string label, Action action)
        {
            action();
            Console.WriteLine($"PASS {label}");
            passed++;
        }
        void Reject(Action action, string tag)
        {
            try { action(); }
            catch (InvalidOperationException e) when (e.Message == tag) { return; }
            throw new Exception($"Expected {tag}");
        }

        (string Result, List<string> Events) Run(StageSecurityApi.InstallationApproval approval,
            HashSet<string> ledger, string actualSha = "", string actor = "synthetic-admin",
            bool tamperApproval = false, bool tamperSource = false, bool denyVerifier = false,
            bool denyPermissions = false, bool expireAfterConsume = false, bool readbackFailure = false,
            List<string>? events = null)
        {
            events ??= [];
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new StageSecurityApi.InstallerContract(installerSha, approval));
            byte[] signature = Sign(json);
            if (tamperApproval) json[0] ^= 1;
            using var stream = new MemoryStream(tamperSource ? [0] : source);
            DateTimeOffset clock = now;
            string result = StageSecurityApi.AdmitInstaller(stream, json, signature, sourceSignature, publicKey,
                pinnedKey, actualSha == "" ? installerSha : actualSha, actor, () => clock,
                _ =>
                {
                    events.Add("verifier");
                    if (denyVerifier) throw new InvalidOperationException("FIXTURE_VERIFIER_DENIED");
                },
                (_, demand) =>
                {
                    demand();
                    events.Add("observe");
                    if (denyPermissions) throw new InvalidOperationException("FIXTURE_ACCESS_DENIED");
                },
                (contract, demand) =>
                {
                    demand();
                    if (!ledger.Add(contract.ApprovalId)) throw new InvalidOperationException("FIXTURE_REPLAY");
                    events.Add("consume");
                    if (expireAfterConsume) clock = contract.Expires;
                },
                (_, demand) =>
                {
                    demand();
                    events.Add("create");
                    if (readbackFailure) throw new InvalidOperationException("FIXTURE_READBACK");
                });
            return (result, events);
        }

        Test("valid signed contract and required sequence", () =>
        {
            var (result, events) = Run(scope, []);
            if (result != "STAGE_CREATED_AND_READBACK" ||
                !events.SequenceEqual(["verifier", "observe", "consume", "observe", "create"]))
                throw new Exception("wrong order");
        });
        Test("changed installer bytes cannot reach verifier or consumption", () =>
        {
            HashSet<string> ledger = [];
            List<string> events = [];
            Reject(() => Run(scope, ledger, actualSha: new string('b', 64), events: events), "STAGE_INSTALLER_SHA");
            if (ledger.Count != 0 || events.Count != 0) throw new Exception("unexpected side effect");
        });
        Test("changed approval and source denied before writes", () =>
        {
            HashSet<string> ledger = [];
            Reject(() => Run(scope, ledger, tamperApproval: true), "STAGE_APPROVAL_SIGNATURE");
            Reject(() => Run(scope, ledger, tamperSource: true), "STAGE_SOURCE_SHA");
            if (ledger.Count != 0) throw new Exception("unexpected consume");
        });
        Test("wrong actor denied", () => Reject(() => Run(scope, [], actor: "other"), "STAGE_ACTOR_MISMATCH"));
        Test("expired, future or over-five-minute windows denied", () =>
        {
            Reject(() => Run(scope with { Expires = now }, []), "STAGE_INSTALL_WINDOW");
            Reject(() => Run(scope with { NotBefore = now.AddSeconds(1) }, []), "STAGE_INSTALL_WINDOW");
            Reject(() => Run(scope with { Expires = now.AddMinutes(5) }, []), "STAGE_INSTALL_WINDOW");
        });
        Test("existing verifier denial blocks consumption", () =>
        {
            HashSet<string> ledger = [];
            Reject(() => Run(scope, ledger, denyVerifier: true), "FIXTURE_VERIFIER_DENIED");
            if (ledger.Count != 0) throw new Exception("unexpected consume");
        });
        Test("permission failure blocks consumption", () =>
        {
            HashSet<string> ledger = [];
            Reject(() => Run(scope, ledger, denyPermissions: true), "FIXTURE_ACCESS_DENIED");
            if (ledger.Count != 0) throw new Exception("unexpected consume");
        });
        Test("duplicate approval cannot reach create", () =>
        {
            HashSet<string> ledger = [];
            _ = Run(scope, ledger);
            List<string> events = [];
            Reject(() => Run(scope, ledger, events: events), "FIXTURE_REPLAY");
            if (events.Contains("create")) throw new Exception("duplicate create");
        });
        Test("post-consume expiry preserves ledger and stops create", () =>
        {
            HashSet<string> ledger = [];
            List<string> events = [];
            Reject(() => Run(scope, ledger, expireAfterConsume: true, events: events), "STAGE_INSTALL_WINDOW");
            if (!ledger.Contains(scope.ApprovalId) || events.Contains("create")) throw new Exception("wrong expiry handling");
        });
        Test("readback failure preserves consumed marker", () =>
        {
            HashSet<string> ledger = [];
            Reject(() => Run(scope, ledger, readbackFailure: true), "FIXTURE_READBACK");
            if (!ledger.Contains(scope.ApprovalId)) throw new Exception("marker lost");
        });

        Console.WriteLine($"{passed} PASS / 0 FAIL");
        return 0;
    }
}
