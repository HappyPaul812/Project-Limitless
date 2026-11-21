internal static class InitialStagingInstallerProgram
{
    internal static int Main(string[] args)
    {
        try
        {
            if (args is not ["--install"])
            {
                Console.Error.WriteLine("HOLD_INVALID_COMMAND");
                return 2;
            }
            Console.WriteLine(StageSecurityApi.InstallProtectedStaging());
            return 0;
        }
        catch (InvalidOperationException e) when (
            e.Message.StartsWith("STAGE_", StringComparison.Ordinal) &&
            e.Message.All(c => char.IsAsciiLetterUpper(c) || c == '_'))
        {
            Console.Error.WriteLine($"HOLD {e.Message}");
            return 1;
        }
        catch (Exception)
        {
            Console.Error.WriteLine("HOLD_INSTALL_FAILED_NO_RETRY");
            return 1;
        }
    }
}
