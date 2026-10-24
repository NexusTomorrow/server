using System.Reflection;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Facts;

public sealed class RegistryFactsTests
{
    private static readonly Assembly[] s_models =
    [
        typeof(global::NexusTomorrow.Network.WireMessages).Assembly,
        typeof(global::NexusTomorrow.Network.Auth.WireMessages).Assembly,
        typeof(global::NexusTomorrow.Network.World.WireMessages).Assembly,
    ];

    private static WireLayout[] Layouts() =>
    [
        .. global::NexusTomorrow.Network.WireMessages.Layouts,
        .. global::NexusTomorrow.Network.Auth.WireMessages.Layouts,
        .. global::NexusTomorrow.Network.World.WireMessages.Layouts,
    ];

    // Every message the server declares, compared with the client's facts for its opcode: what disagrees must be pinned with its reason and its problem
    // text, and what is pinned must still disagree in the same words, so a fix shows up as a stale entry to delete and a changed disagreement as a
    // changed entry.
    [Fact]
    public void EveryDeclaredMessageAgreesWithTheClientFactsOrIsPinned()
    {
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);
        WireLayout[] layouts = Layouts();

        var found = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        int agreeing = 0;
        foreach (WireLayout layout in layouts)
        {
            List<string> problems = WireFactsComparison.Compare(layout, rows.GetValueOrDefault(layout.Opcode));
            if (problems.Count == 0)
                agreeing++;
            foreach (string problem in problems)
            {
                int colon = problem.IndexOf(':', StringComparison.Ordinal);
                string key = problem[..colon];
                if (!found.TryGetValue(key, out List<string>? texts))
                    found[key] = texts = [];
                texts.Add(problem[(colon + 1)..].Trim());
            }
        }

        TestContext.Current.TestOutputHelper?.WriteLine($"{layouts.Length} declared messages: {agreeing} agree with the client facts; {found.Count} disagreements pinned.");

        var report = new List<string>();
        foreach ((string key, List<string> texts) in found)
        {
            string problem = string.Join(" | ", texts);
            if (!RegistryDisagreements.Pinned.TryGetValue(key, out Pin pin))
                report.Add($"not pinned: {key}: {problem}");
            else if (pin.Problem != problem)
                report.Add($"pinned, but the problem changed: {key}: {problem}");
        }
        report.AddRange(RegistryDisagreements.Pinned.Where(entry => !found.ContainsKey(entry.Key)).Select(entry => $"pinned, but it agrees now: {entry.Key} ({entry.Value.Reason})"));
        if (report.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, report));
    }

    // Every message at Fields, and every layout-confirmed message, takes each field the client reads as one field whole, or lists the split as settled (FieldsSplits).
    [Fact]
    public void EveryMessageAtFieldsOrLayoutConfirmedTakesEachClientFieldWholeOrSettlesItsSplit()
    {
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);
        var rungs = s_models.SelectMany(assembly => assembly.GetTypes())
            .Select(type => (Message: type.GetCustomAttribute<WireMessageAttribute>(), Verified: type.GetCustomAttribute<VerifiedAttribute>()))
            .Where(type => type.Message is not null && type.Verified is not null)
            .ToDictionary(type => (ushort)type.Message!.Opcode, type => type.Verified!.Rung);

        HashSet<ushort> confirmed = [.. s_models.SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsDefined(typeof(LayoutConfirmedAttribute), inherit: false) && type.GetCustomAttribute<WireMessageAttribute>() is not null)
            .Select(type => (ushort)type.GetCustomAttribute<WireMessageAttribute>()!.Opcode)];

        List<string> report = FieldsSplits.Check(
            Layouts().Select(layout => (layout, rungs.TryGetValue(layout.Opcode, out Rung rung) ? rung : (Rung?)null, confirmed.Contains(layout.Opcode))), rows, FieldsSplits.Settled);

        TestContext.Current.TestOutputHelper?.WriteLine($"{rungs.Values.Count(rung => rung >= Rung.Fields)} messages at fields; {confirmed.Count} layout-confirmed; {FieldsSplits.Settled.Count} splits settled.");
        if (report.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, report));
    }
}
