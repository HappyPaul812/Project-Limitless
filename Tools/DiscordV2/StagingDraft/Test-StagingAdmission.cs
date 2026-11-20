using System.Security.Cryptography;
using System.Text.Json;

internal static class StagingAdmissionTests
{
    internal static int Run()
    {
        using var key = RSA.Create(2048);
        byte[] publicKey = key.ExportSubjectPublicKeyInfo();
        string anchor = BootstrapPolicy.Hash(publicKey);
        byte[] source = "isolated staging source"u8.ToArray();
        byte[] Sign(byte[] data) => key.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
        byte[] sourceSignature = Sign(source);
        DateTimeOffset now = BootstrapPolicy.Cutoff.AddHours(-2);
        var approval = new StageSecurityApi.InstallationApproval(StageSecurityApi.Profile, Guid.NewGuid().ToString("N"),
            StageSecurityApi.Stage, StageSecurityApi.InitialLedger, "synthetic-ledger", "synthetic-actor",
            now.AddMinutes(-1), now.AddMinutes(1), BootstrapPolicy.Hash(source), ["root", "parent"], [anchor, anchor]);

        string ledger = Directory.CreateTempSubdirectory("Limitless-Admission-").FullName;
        int creates = 0, consumes = 0, passed = 0;
        void Check(string name, Action action)
        {
            action();
            passed++;
            Console.WriteLine($"PASS {name}");
        }
        void Reject<T>(Action action, string? tag = null) where T : Exception
        {
            int before = creates;
            try { action(); }
            catch (T e) when (tag is null || e.Message == tag)
            {
                if (creates != before) throw new Exception("unexpected create");
                return;
            }
            throw new Exception($"Expected {typeof(T).Name} {tag}");
        }
        string RunApproval(StageSecurityApi.InstallationApproval value, string? fingerprint = null,
            byte[]? changedSource = null, bool changeSignedApproval = false, Action? observe = null,
            Func<DateTimeOffset>? clock = null, bool failConsumption = false)
        {
            byte[] signed = JsonSerializer.SerializeToUtf8Bytes(value);
            byte[] signature = Sign(signed);
            if (changeSignedApproval) signed[5] ^= 1;
            using var pinned = new MemoryStream(changedSource ?? source);
            return StageSecurityApi.Admit(pinned, signed, signature, sourceSignature, publicKey,
                fingerprint ?? anchor, BootstrapPolicy.Cutoff, clock ?? (() => now),
                _ => observe?.Invoke(),
                a =>
                {
                    consumes++;
                    if (failConsumption) throw new IOException("fixture write failure");
                    StageSecurityApi.ConsumeIsolatedApproval(ledger, a.ApprovalId);
                },
                a =>
                {
                    if (!File.Exists(Path.Combine(ledger, a.ApprovalId + ".used")))
                        throw new Exception("missing consumption");
                    creates++;
                });
        }

        Check("production-missing-trust-stops-before-open", () => Reject<InvalidOperationException>(
            () => StageSecurityApi.InstallFromApproval("", [], [], [], []), "STAGE_TRUST_HOLD"));
        Check("deny-unapproved-signer-before-consume", () => Reject<InvalidOperationException>(
            () => RunApproval(approval, fingerprint: new string('0', 64)), "STAGE_TRUST_HOLD"));
        Check("deny-tampered-signed-approval", () => Reject<InvalidOperationException>(
            () => RunApproval(approval, changeSignedApproval: true), "STAGE_APPROVAL_SIGNATURE"));
        Check("deny-tampered-source", () => Reject<InvalidOperationException>(
            () => RunApproval(approval, changedSource: [1, 2, 3]), "STAGE_SOURCE_SHA"));
        Check("deny-expired-approval-and-contract", () =>
        {
            Reject<InvalidOperationException>(() => RunApproval(approval, clock: () => approval.Expires), "STAGE_EXPIRED");
            Reject<InvalidOperationException>(() => RunApproval(approval with { Expires = BootstrapPolicy.Cutoff.AddSeconds(1) }), "STAGE_EXPIRED");
        });
        Check("deny-wrong-ledger-and-legacy-profile", () =>
        {
            Reject<InvalidOperationException>(() => RunApproval(approval with { Ledger = "alternate" }), "STAGE_APPROVAL_SCOPE");
            Reject<InvalidOperationException>(() => RunApproval(approval with { Profile = "DISCORD_V2_INITIAL_STAGING_V1" }), "STAGE_APPROVAL_SCOPE");
        });
        Check("deny-unsafe-or-missing-ledger-before-consume", () =>
        {
            Reject<IOException>(() => RunApproval(approval, observe: () => throw new IOException("fixture ledger unsafe")));
            if (consumes != 0) throw new Exception("consumed invalid approval");
        });
        Check("deny-consumption-failure", () => Reject<IOException>(() => RunApproval(approval, failConsumption: true)));
        Check("verified-approval-observed-consumed-rechecked-created", () =>
        {
            int observations = 0;
            string report = RunApproval(approval, observe: () => observations++);
            if (observations != 2 || creates != 1 || report != "STAGE_CREATED_AND_READBACK")
                throw new Exception("admission sequence");
        });
        Check("deny-replay-after-consumption", () => Reject<IOException>(() => RunApproval(approval)));
        Check("deny-mutation-after-consume-without-unspending", () =>
        {
            var next = approval with { ApprovalId = Guid.NewGuid().ToString("N") };
            int observations = 0;
            Reject<IOException>(() => RunApproval(next, observe: () =>
            {
                if (++observations == 2) throw new IOException("fixture ledger replaced");
            }));
            Reject<IOException>(() => RunApproval(next));
        });
        Check("deny-expiry-after-consume-without-unspending", () =>
        {
            var next = approval with { ApprovalId = Guid.NewGuid().ToString("N") };
            int observations = 0;
            Reject<InvalidOperationException>(() => RunApproval(next, observe: () => observations++,
                clock: () => observations < 2 ? now : next.Expires), "STAGE_EXPIRED");
            Reject<IOException>(() => RunApproval(next));
        });
        Console.WriteLine($"{passed} PASS / 0 FAIL");
        return 0;
    }
}
