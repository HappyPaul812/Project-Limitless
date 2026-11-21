using System.Security.Cryptography;
using System.Text.Json;
using System.Collections.Concurrent;

internal static partial class DeploymentAdminTests
{
    private static int CurrentProductContractChecks()
    {
        var binding = new AdmissionBinding(Native.Worker, @"\SyntheticV2\Trial", CurrentFakeOnlyContract.Code, CurrentFakeOnlyContract.Evidence);
        var now = new DateTimeOffset(2026, 11, 21, 0, 0, 0, TimeSpan.Zero);
        string supervisor = new('a', 64), probe = new('b', 64);
        using var signer = RSA.Create(2048);
        string pin = Hash(signer.ExportSubjectPublicKeyInfo());
        Dictionary<string, object> Contract() => new()
        {
            ["Profile"]="DISCORD_V2_FAKE_ONLY_PRODUCT_V1", ["Approved"]=true,
            ["WorkerSid"]=binding.WorkerSid, ["TaskPath"]=binding.TaskPath,
            ["CodeRoot"]=binding.CodeRoot, ["EvidenceRoot"]=binding.EvidenceRoot,
            ["SupervisorSha256"]=supervisor, ["ProbeSha256"]=probe, ["ImageSignerSpkiSha256"]=pin,
            ["NotBeforeUtc"]=now.AddMinutes(-1), ["ExpiresUtc"]=now.AddMinutes(4),
            ["Mode"]="FAKE_ONLY", ["NoAi"]=true, ["NoIpc"]=true, ["JobSeconds"]=90,
            ["SupervisorCount"]=1, ["ProbeCount"]=1
        };
        byte[] Sign(byte[] raw) => signer.SignData(raw, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
        void Validate(Dictionary<string, object> contract)
        {
            byte[] raw = JsonSerializer.SerializeToUtf8Bytes(contract);
            CurrentFakeOnlyContract.Validate(raw, Sign(raw), signer, pin, binding, now, supervisor, probe);
        }
        Test("current-contract-valid", () => Validate(Contract()));
        Test("current-contract-tampered", () =>
        {
            byte[] raw = JsonSerializer.SerializeToUtf8Bytes(Contract()), signature = Sign(raw);
            Deny(() => CurrentFakeOnlyContract.Validate([.. raw, (byte)' '], signature, signer, pin, binding, now, supervisor, probe));
        });
        foreach (var item in new (string Field, object Value)[]
        {
            ("ExpiresUtc", now), ("WorkerSid", "S-1-5-21-1-2-3-555"), ("TaskPath", @"\OtherTask"),
            ("CodeRoot", @"C:\Other"), ("ImageSignerSpkiSha256", probe), ("ProbeSha256", supervisor),
            ("Approved", false), ("NoAi", false)
        })
            Test("current-contract-denies-" + item.Field, () =>
            {
                var contract = Contract();
                contract[item.Field] = item.Value;
                Deny(() => Validate(contract));
            });
        Test("missing-native-binding-denied", () => Deny(() => CurrentFakeOnlyContract.RequireBinding(binding with { WorkerSid = "" })));
        Test("protected-onboot-path-denied", () => Deny(() => CurrentFakeOnlyContract.RequireBinding(binding with { TaskPath = @"\LIMITLESS-Bot-OnBoot" })));
        Test("current-ledger-and-code-overlap-denied", () => Deny(() =>
            DeploymentLedgerPolicy.Root(DeploymentLedgerBinding.Root, DeploymentLedgerBinding.Root, binding with { CodeRoot = CurrentFakeOnlyContract.Root })));
        Test("current-ledger-consumption-not-repeatable", () =>
        {
            var records = new ConcurrentDictionary<string, MemoryStream>();
            const string id = "aabbccdd000011112222333344445555";
            DeploymentLedger.Consume(DeploymentLedgerBinding.Root, id, probe, new MemoryLedger(records), () => Validate(Contract()));
            Deny(() => DeploymentLedger.Consume(DeploymentLedgerBinding.Root, id, probe, new MemoryLedger(records), () => Validate(Contract())));
            Check(records.Count == 1);
        });
        Console.WriteLine($"CURRENT_FAKE_ONLY_CONTRACT: {pass} PASS / {fail} FAIL");
        return fail == 0 ? 0 : 1;
    }
}
