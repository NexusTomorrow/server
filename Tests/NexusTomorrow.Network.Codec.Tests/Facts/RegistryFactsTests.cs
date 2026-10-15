namespace NexusTomorrow.Network.Codec.Tests.Facts;

public sealed class RegistryFactsTests
{
    // Every message the server declares, compared with the client's facts for its opcode: what disagrees must be pinned with its reason, and what is pinned
    // must still disagree, so a fix shows up as a stale entry to delete.
    [Fact]
    public void EveryDeclaredMessageAgreesWithTheClientFactsOrIsPinned()
    {
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);
        WireLayout[] layouts =
        [
            .. global::NexusTomorrow.Network.WireMessages.Layouts,
            .. global::NexusTomorrow.Network.Auth.WireMessages.Layouts,
            .. global::NexusTomorrow.Network.World.WireMessages.Layouts,
        ];

        var found = new Dictionary<string, string>(StringComparer.Ordinal);
        int agreeing = 0;
        foreach (WireLayout layout in layouts)
        {
            List<string> problems = WireFactsComparison.Compare(layout, rows.GetValueOrDefault(layout.Opcode));
            if (problems.Count == 0)
                agreeing++;
            foreach (string problem in problems)
                found[problem[..problem.IndexOf(':', StringComparison.Ordinal)]] = problem;
        }

        TestContext.Current.TestOutputHelper?.WriteLine($"{layouts.Length} declared messages: {agreeing} agree with the client facts; {found.Count} disagreements pinned.");

        var report = new List<string>();
        report.AddRange(found.Where(entry => !RegistryDisagreements.Pinned.ContainsKey(entry.Key)).Select(entry => $"not pinned: {entry.Value}"));
        report.AddRange(RegistryDisagreements.Pinned.Where(entry => !found.ContainsKey(entry.Key)).Select(entry => $"pinned, but it agrees now: {entry.Key} ({entry.Value})"));
        if (report.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, report));
    }
}
