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

    // A message below fields must be pinned unless it is dormant, which counts apart; a pinned message must still be below fields and not dormant,
    // so reading one, or marking it dormant, shows up as an entry to delete.
    [Fact]
    public void EveryDeclaredMessageReachesFieldsOrIsPinned()
    {
        var messages = s_models.SelectMany(Messages).ToList();
        var declared = messages.Select(m => m.Key).ToHashSet(StringComparer.Ordinal);
        var dormant = messages.Where(m => m.Type.IsDefined(typeof(DormantAttribute), inherit: false)).Select(m => m.Key).ToHashSet(StringComparer.Ordinal);
        var below = messages.Where(m => MessageVerification.RungOf(m.Type) is null or < Rung.Fields).Select(m => m.Key).Where(k => !dormant.Contains(k)).ToHashSet(StringComparer.Ordinal);
        TestContext.Current.TestOutputHelper?.WriteLine(
            $"{declared.Count} declared messages: {declared.Count - dormant.Count - below.Count} read to fields; {dormant.Count} dormant; {below.Count} unverified.");

        var report = new List<string>();
        report.AddRange(below.Where(k => !RegistryVerification.Unverified.Contains(k)).Order(StringComparer.Ordinal).Select(k => $"below fields, not pinned: {k}"));
        report.AddRange(RegistryVerification.Unverified.Where(k => !below.Contains(k)).Order(StringComparer.Ordinal)
            .Select(k => dormant.Contains(k) ? $"pinned, but it is dormant: {k}"
                : declared.Contains(k) ? $"pinned, but it reached fields: {k}"
                : $"pinned, but no message declares it: {k}"));
        if (report.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, report));
    }

    // A mark says the member's meaning is two-observed and documented, so a marked member without a summary shows nothing for it.
    [Fact]
    public void EveryVerifiedMemberCarriesASummary()
    {
        Dictionary<string, string> summaries = XmlDocs.SummariesOf(s_models);
        var missing = new List<string>();
        foreach (Type type in s_models.SelectMany(assembly => assembly.GetTypes()))
        {
            foreach (MemberInfo member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (member.GetCustomAttribute<VerifiedMemberAttribute>() is null)
                    continue;

                string id = (member is PropertyInfo ? "P:" : "F:") + type.FullName!.Replace('+', '.') + "." + member.Name;
                if (!summaries.ContainsKey(id))
                    missing.Add(id);
            }
        }

        Assert.Empty(missing);
    }

    [Fact]
    public void EveryMessageCarriesTheDocumentationItsRungRequires()
    {
        var problems = new List<string>();
        Dictionary<string, string> summaries = XmlDocs.SummariesOf(s_models);
        foreach (Assembly assembly in s_models)
        {
            foreach ((Type type, string key) in Messages(assembly))
                problems.AddRange(MessageVerification.Problems(type, key, summaries, s_models));
        }

        Assert.Empty(problems);
    }
}
