using System.Reflection;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Verification;

public sealed class RegistryVerificationTests
{
    private static readonly Assembly[] s_models =
    [
        typeof(global::NexusTomorrow.Network.WireMessages).Assembly,
        typeof(global::NexusTomorrow.Network.Auth.WireMessages).Assembly,
        typeof(global::NexusTomorrow.Network.World.WireMessages).Assembly,
    ];

    private static IEnumerable<(Type Type, string Key)> Messages(Assembly assembly) =>
        assembly.GetTypes()
            .Select(t => (Type: t, Attribute: t.GetCustomAttribute<WireMessageAttribute>()))
            .Where(m => m.Attribute is not null)
            .Select(m => (m.Type, $"0x{(ushort)m.Attribute!.Opcode:X4}"));

    // A message below fields must be pinned, and a pinned message must still be below fields, so reading one shows up as an entry to delete.
    [Fact]
    public void EveryDeclaredMessageReachesFieldsOrIsPinned()
    {
        var messages = s_models.SelectMany(Messages).ToList();
        var declared = messages.Select(m => m.Key).ToHashSet(StringComparer.Ordinal);
        var below = messages.Where(m => MessageVerification.RungOf(m.Type) is null or < Rung.Fields).Select(m => m.Key).ToHashSet(StringComparer.Ordinal);
        TestContext.Current.TestOutputHelper?.WriteLine($"{declared.Count} declared messages: {declared.Count - below.Count} read to fields; {below.Count} unverified.");

        var report = new List<string>();
        report.AddRange(below.Where(k => !RegistryVerification.Unverified.Contains(k)).Order(StringComparer.Ordinal).Select(k => $"below fields, not pinned: {k}"));
        report.AddRange(RegistryVerification.Unverified.Where(k => !below.Contains(k)).Order(StringComparer.Ordinal)
            .Select(k => declared.Contains(k) ? $"pinned, but it reached fields: {k}" : $"pinned, but no message declares it: {k}"));
        if (report.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, report));
    }

    [Fact]
    public void EveryMessageCarriesTheDocumentationItsRungRequires()
    {
        var problems = new List<string>();
        foreach (Assembly assembly in s_models)
        {
            Dictionary<string, string> summaries = XmlDocs.SummariesOf(assembly);
            foreach ((Type type, string key) in Messages(assembly))
                problems.AddRange(MessageVerification.Problems(type, key, summaries));
        }

        Assert.Empty(problems);
    }
}
