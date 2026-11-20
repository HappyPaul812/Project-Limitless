using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;

const string approval = "01ad352abd5c48caa2138587a230a118";
if (args is ["admission-only"]) return StagingAdmissionTests.Run();
if (args.Length != 0)
{
    try { StageSecurityApi.ConsumeIsolatedApproval(args[0], approval); return 0; }
    catch (IOException e) when ((e.HResult & 0xffff) is 80 or 183) { return 17; }
}

int passed = 0;
void Check(string name, Action test)
{
    test();
    Console.WriteLine($"PASS {name}");
    passed++;
}
void Reject(string tag, Action test)
{
    try { test(); }
    catch (InvalidOperationException e) when (e.Message == tag) { return; }
    throw new Exception($"Expected {tag}");
}
byte[] Descriptor(string sddl)
{
    var descriptor = new RawSecurityDescriptor(sddl);
    var bytes = new byte[descriptor.BinaryLength];
    descriptor.GetBinaryForm(bytes, 0);
    return bytes;
}

byte[] security = Descriptor("O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)");
var child = new BootstrapNode(StageSecurityApi.Stage, "synthetic", 42, true, false, 0, security);
Check("created-handle-owner-dacl-and-volume", () => StageSecurityApi.ValidateCreated(child, 42));
Check("reject-reparse-handle", () => Reject("BOOTSTRAP_PATH_HANDLE",
    () => StageSecurityApi.ValidateCreated(child with { Reparse = true }, 42)));
Check("reject-wrong-final-path", () => Reject("BOOTSTRAP_PATH_HANDLE",
    () => StageSecurityApi.ValidateCreated(child with { Path = @"C:\Different" }, 42)));
Check("reject-inherited-handle", () => Reject("BOOTSTRAP_PATH_HANDLE",
    () => StageSecurityApi.ValidateCreated(child with { HandleFlags = 1 }, 42)));
Check("reject-cross-volume", () => Reject("STAGE_CREATED_VOLUME",
    () => StageSecurityApi.ValidateCreated(child, 43)));
Check("reject-wrong-owner", () => Reject("BOOTSTRAP_OWNER_READBACK", () =>
    StageSecurityApi.ValidateCreated(child with { Security = Descriptor("O:SYG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)") }, 42)));
Check("reject-untrusted-dacl", () => Reject("BOOTSTRAP_DACL_READBACK", () =>
    StageSecurityApi.ValidateCreated(child with { Security = Descriptor("O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BU)") }, 42)));
Check("parent-security-and-identity-change", () =>
{
    using var host = new ParentFixture(child);
    string[] keys = ["root", "parent"];
    string[] hashes = [BootstrapPolicy.Hash(security), BootstrapPolicy.Hash(security)];
    if (StageSecurityApi.ValidateParents(host, keys, hashes) != 42) throw new Exception("Volume");
    host.Parent = host.Parent with { Key = "replacement" };
    Reject("STAGE_PARENT_CHANGED", () => StageSecurityApi.ValidateParents(host, keys, hashes));
    host.Parent = host.Parent with { Key = "parent", Security = Descriptor("O:SYG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)") };
    Reject("STAGE_PARENT_CHANGED", () => StageSecurityApi.ValidateParents(host, keys, hashes));
});
Check("reject-expired-contract", () => Reject("STAGE_EXPIRED",
    () => StageSecurityApi.DemandTime(BootstrapPolicy.Cutoff.AddDays(20), BootstrapPolicy.Cutoff)));
using var signer = RSA.Create(2048);
using var otherSigner = RSA.Create(2048);
byte[] source = Encoding.UTF8.GetBytes("isolated source");
string sha = BootstrapPolicy.Hash(source);
byte[] signature = signer.SignData(source, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
Check("source-exact-signed-bytes", () =>
{
    if (StageSecurityApi.VerifySignedSource(new MemoryStream(source), sha, signer, signature) != "isolated source")
        throw new Exception("Signed content");
});
Check("sha-match-alone-denied", () => Reject("STAGE_SOURCE_SIGNER",
    () => StageSecurityApi.VerifySignedSource(new MemoryStream(source), sha, otherSigner, signature)));
Check("source-replacement-denied", () => Reject("STAGE_SOURCE_SHA",
    () => StageSecurityApi.VerifySignedSource(new MemoryStream([1, 2, 3]), sha, signer, signature)));

string ledger = Path.Combine(Path.GetTempPath(), "Limitless-StagingDelta-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(ledger);
Check("source-handle-denies-write-and-rename", () =>
{
    string file = Path.Combine(ledger, "source.fixture");
    File.WriteAllBytes(file, source);
    using var pinned = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
    void RequireSharingViolation(Action mutation)
    {
        try { mutation(); }
        catch (IOException e) when ((e.HResult & 0xffff) == 32) { return; }
        throw new Exception("Expected sharing violation");
    }
    RequireSharingViolation(() => { using var writer = File.OpenWrite(file); });
    RequireSharingViolation(() => File.Move(file, file + ".moved"));
    if (StageSecurityApi.VerifySignedSource(pinned, sha, signer, signature) != "isolated source")
        throw new Exception("Pinned source");
});
Process Worker()
{
    var start = new ProcessStartInfo(Environment.ProcessPath!) { UseShellExecute = false };
    if (string.Equals(Path.GetFileNameWithoutExtension(start.FileName), "dotnet", StringComparison.OrdinalIgnoreCase))
        start.ArgumentList.Add(typeof(ParentFixture).Assembly.Location);
    start.ArgumentList.Add(ledger);
    return Process.Start(start) ?? throw new Exception("Worker start");
}
Check("cross-process-race-and-replay", () =>
{
    using var first = Worker();
    using var second = Worker();
    if (!first.WaitForExit(10000) || !second.WaitForExit(10000)) throw new Exception("Worker timeout");
    int[] exits = [first.ExitCode, second.ExitCode];
    Array.Sort(exits);
    if (!exits.SequenceEqual([0, 17])) throw new Exception("Expected one consumer");
    using var replay = Worker();
    if (!replay.WaitForExit(10000) || replay.ExitCode != 17) throw new Exception("Replay accepted");
});
Check("empty-marker-retained", () =>
{
    const string emptyId = "f41c8b28124244b69bbbc1a7a330a118";
    string marker = Path.Combine(ledger, emptyId + ".used");
    using (File.Open(marker, FileMode.CreateNew)) { }
    try { StageSecurityApi.ConsumeIsolatedApproval(ledger, emptyId); }
    catch (IOException) when (new FileInfo(marker).Length == 0) { return; }
    throw new Exception("Existing marker overwritten");
});
Console.WriteLine($"{passed} PASS / 0 FAIL");
return 0;

sealed class ParentFixture(BootstrapNode template) : IBootstrapHost
{
    public BootstrapNode Parent = template with { Path = @"C:\ProgramData", Key = "parent" };
    public BootstrapNode PinAndRead(string path) => path == @"C:\"
        ? template with { Path = path, Key = "root" } : Parent;
    public DateTimeOffset Now => throw new NotSupportedException();
    public bool FixedNtfsVolume => throw new NotSupportedException();
    public BootstrapActor ObserveActor() => throw new NotSupportedException();
    public bool Exists(string path) => throw new NotSupportedException();
    public void Bind(BootstrapWriteGate gate) => throw new NotSupportedException();
    public void CreateProtected(string path, string sddl) => throw new NotSupportedException();
    public void Dispose() { }
}
