namespace NexusTomorrow.Network.Equivalence.Tests.SelfTest;

/// <summary>
/// Pairs each self-test declaration with the type of the same name in the SelfTest.Legacy namespace.
/// </summary>
internal static class SelfTestTwins
{
    private const string DeclaredNamespace = ".SelfTest.Declared.";
    private const string LegacyNamespace = ".SelfTest.Legacy.";

    public static Type? Of(Type declared)
    {
        string? fullName = declared.FullName;
        if (fullName is null || !fullName.Contains(DeclaredNamespace, StringComparison.Ordinal))
            return null;

        return typeof(SelfTestTwins).Assembly.GetType(fullName.Replace(DeclaredNamespace, LegacyNamespace, StringComparison.Ordinal), throwOnError: false);
    }
}
