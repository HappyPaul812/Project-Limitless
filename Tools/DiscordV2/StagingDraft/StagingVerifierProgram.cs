using System.Reflection;

try
{
    switch (args)
    {
        case ["--version"]:
            Console.WriteLine(Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion);
            return 0;
        case ["--verify-image", var image, var sha]:
            StageSecurityApi.ReviewReleaseImage(image, sha);
            Console.WriteLine("VERIFIED_IMAGE_ONLY");
            return 0;
        case ["--review", var source, var approval, var approvalSignature, var sourceSignature]:
            StageSecurityApi.ReviewFromApproval(source, ReadBounded(approval, 8192),
                ReadBounded(approvalSignature, 4096), ReadBounded(sourceSignature, 4096));
            Console.WriteLine("REVIEW_ONLY_NO_APPROVAL_CONSUMED");
            return 0;
        default:
            Console.Error.WriteLine("HOLD_INVALID_COMMAND");
            return 2;
    }
}
catch (InvalidOperationException e) when (
    e.Message.All(c => char.IsAsciiLetterUpper(c) || c == '_')
    && (e.Message.StartsWith("STAGE_", StringComparison.Ordinal) || e.Message.StartsWith("BOOTSTRAP_", StringComparison.Ordinal)))
{
    Console.Error.WriteLine($"HOLD {e.Message}");
    return 1;
}
catch (Exception)
{
    // 승인 파일 경로와 Windows 세부 오류를 출력하지 않습니다.
    Console.Error.WriteLine("HOLD_VERIFICATION_FAILED");
    return 1;
}

static byte[] ReadBounded(string path, int maxBytes)
{
    using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
    BootstrapPolicy.Need(stream.Length > 0 && stream.Length <= maxBytes, "STAGE_REVIEW_INPUT");
    byte[] data = new byte[stream.Length];
    stream.ReadExactly(data);
    return data;
}
