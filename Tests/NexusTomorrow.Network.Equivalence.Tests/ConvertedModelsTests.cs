using Microsoft.Extensions.DependencyInjection;

namespace NexusTomorrow.Network.Equivalence.Tests;

public sealed class ConvertedModelsTests
{
    private const int MinimumSeeds = 32;
    private const int MaxReportedLines = 1200;

    [Fact]
    public void EveryDeclaredTypeWritesAndReadsLikeItsLegacyTwin()
    {
        List<Type> declared = ConvertedModels.DeclaredTypes();
        using ServiceProvider legacyServices = ConvertedModels.CreateLegacyServices();
        int seeds = SeedsFor(declared);
        var uncovered = new List<string>();
        var differences = new List<string>();
        int pairs = 0;
        int holders = 0;

        foreach (Type type in declared)
        {
            string fullName = type.FullName ?? type.Name;
            if (ConvertedModels.Exclusions.ContainsKey(fullName))
                continue;

            Type? twin = ConvertedModels.TwinOf(type);
            if (twin is null || !Equivalence.CanCheck(twin))
            {
                // A type with no twin, or whose twin neither writes nor reads itself, is checked through the checked type that holds it: the mapper and
                // comparer pair its members with the legacy member's own type, with the bare arm where the seed keeps a union's arm itself, or with the
                // holder's own members.
                if (IsHeldByACheckedType(type, declared))
                    holders++;
                else
                    uncovered.Add(fullName);
                continue;
            }

            differences.AddRange(Equivalence.Check(type, twin, ConvertedModels.Pairing, legacyServices, seeds));
            pairs++;
        }

        TestContext.Current.TestOutputHelper?.WriteLine(
            $"Checked {pairs} of {declared.Count} declared types against their legacy twins over {seeds} seeds, {ConvertedModels.ReadOnly.Count} of them against the legacy Read only; "
            + $"{holders} types checked through the types holding them; {ConvertedModels.Exclusions.Count} excluded. "
            + $"Rules: {ConvertedModels.Rows.Count} rows, {ConvertedModels.Limits.Count} limits, {ConvertedModels.Presets.Count} presets, {ConvertedModels.Renames.Count} renames.");
        if (uncovered.Count > 0)
            Assert.Fail($"These declared types have no legacy twin that writes or reads, no checked type holding them, and no exclusion: {string.Join(", ", uncovered)}");
        if (differences.Count > 0)
            Assert.Fail(Report(differences));
        Assert.Equal(declared.Count, pairs + holders + ConvertedModels.Exclusions.Count);
    }

    [Fact]
    public void EveryPinnedRuleNamesADeclaredTypeTheCheckRuns()
    {
        var declared = ConvertedModels.DeclaredTypes().ToDictionary(type => type.FullName ?? type.Name, StringComparer.Ordinal);
        var problems = new List<string>();

        foreach (string name in ConvertedModels.ReadOnly.Keys.Concat(ConvertedModels.Rows.Keys).Concat(ConvertedModels.Limits.Keys).Concat(ConvertedModels.Presets.Keys)
            .Concat(ConvertedModels.Renames.Keys.Select(key => key.Type)).Concat(ConvertedModels.ZeroFedPads.Keys.Select(key => key.Type)).Concat(ConvertedModels.UncheckedSelectors.Keys))
        {
            if (!declared.ContainsKey(name))
                problems.Add($"{name} is not a declared type");
            else if (ConvertedModels.Exclusions.ContainsKey(name))
                problems.Add($"{name} is excluded, so its rule never runs");
        }

        foreach ((string name, RowColumns rows) in ConvertedModels.Rows)
        {
            if (!declared.TryGetValue(name, out Type? type))
                continue;

            foreach ((string column, _) in rows.Columns)
            {
                if (WireMembers.Of(type).FirstOrDefault(member => member.Name == column) is not { Shape: WireShape.Array, CountFrom: not null })
                    problems.Add($"{name}.{column} is not a column sharing a count");
            }
        }

        foreach (((string name, string member), _) in ConvertedModels.Renames)
        {
            if (declared.TryGetValue(name, out Type? type) && WireMembers.Of(type).All(candidate => candidate.Name != member))
                problems.Add($"{name}.{member} is not a wire member");
        }

        AssertNone(problems);
    }

    // The mapper copies a selector the generated Write takes from the arm's [UnionCase] onto the legacy member that stores it, so only a legacy Read, which
    // picks the arm by the selector, proves the case values. They belong to the arms, so one holder read back proves them for every holder of the union.
    [Fact]
    public void EveryStoredSelectorIsReadBackOrPinned()
    {
        List<Type> declared = ConvertedModels.DeclaredTypes();
        HashSet<Type> readCovered = ReadCovered(declared);
        HashSet<Type> unionsReadBack = [.. readCovered.SelectMany(WireMembers.Of).Where(member => member.Shape == WireShape.Union).Select(member => member.ValueType)];
        var problems = new List<string>();
        var stored = new HashSet<string>(StringComparer.Ordinal);

        foreach (Type type in declared.Where(IsChecked))
        {
            if (ConvertedModels.TwinOf(type) is not { } twin)
                continue;

            IReadOnlyList<WireMember> members = WireMembers.Of(type);
            foreach (WireMember union in members.Where(member => member.Shape == WireShape.Union && member.Selector is not null))
            {
                WireMember selector = members.Single(member => member.Name == union.Selector);
                if (MemberAccess.Find(twin, ConvertedModels.Pairing.LegacyNameOf(selector)) is not { } legacySelector || !MemberAccess.HasSetter(legacySelector))
                    continue;

                if (readCovered.Contains(type) || unionsReadBack.Contains(union.ValueType))
                    continue;

                string fullName = type.FullName ?? type.Name;
                stored.Add(fullName);
                if (!ConvertedModels.UncheckedSelectors.ContainsKey(fullName))
                    problems.Add($"{fullName}.{union.Name}: the legacy model stores {selector.Name} and no legacy Read checks it");
            }
        }

        foreach (string pinned in ConvertedModels.UncheckedSelectors.Keys.Where(name => !stored.Contains(name)))
            problems.Add($"{pinned} is pinned as unchecked, but a legacy Read checks its case values or it stores no selector");

        AssertNone(problems);
    }

    [Fact]
    public void EveryPadUnderTheReadCheckIsAccountedFor()
    {
        List<Type> declared = ConvertedModels.DeclaredTypes();
        var problems = new List<string>();
        var seen = new HashSet<(string Type, string Pad)>();

        foreach (Type type in ReadCovered(declared))
        {
            foreach (WireMember pad in WireMembers.Of(type).Where(member => member.Shape == WireShape.Pad && member.Member.DeclaringType == type))
            {
                (string Type, string Pad) key = (type.FullName ?? type.Name, pad.Name);
                seen.Add(key);
                if (!ConvertedModels.ZeroFedPads.ContainsKey(key))
                    problems.Add($"{key.Type}.{key.Pad} is fed zeros by the read check; pin what the legacy Read does with those bits");
            }
        }

        foreach ((string Type, string Pad) pinned in ConvertedModels.ZeroFedPads.Keys.Where(key => !seen.Contains(key)))
            problems.Add($"{pinned.Type}.{pinned.Pad} is pinned, but no read check reaches it");

        AssertNone(problems);
    }

    /// <summary>
    /// Enough seeds for the filler, which takes arm seed % count, to place every arm of the widest union at least once.
    /// </summary>
    private static int SeedsFor(List<Type> declared)
    {
        int widest = declared.SelectMany(WireMembers.Of).Where(member => member.Shape == WireShape.Union).Select(member => Filler.ArmsOf(member).Length).DefaultIfEmpty(0).Max();
        return Math.Max(MinimumSeeds, widest);
    }

    private static bool IsChecked(Type type)
    {
        return !ConvertedModels.Exclusions.ContainsKey(type.FullName ?? type.Name);
    }

    /// <summary>
    /// Whether a checked type with a twin that writes or reads holds <paramref name="type"/> as a member, an element, or an arm of one of its unions.
    /// </summary>
    private static bool IsHeldByACheckedType(Type type, List<Type> declared)
    {
        return declared.Any(holder => IsChecked(holder) && ConvertedModels.TwinOf(holder) is { } twin && Equivalence.CanCheck(twin)
            && WireMembers.Of(holder).Any(member => member.ValueType == type || (member.Shape == WireShape.Union && member.ValueType.IsAssignableFrom(type))));
    }

    /// <summary>
    /// Every checked type a legacy Read reaches: each checked type whose twin reads itself, and everything it holds as a member, an element, or an arm the
    /// filler places.
    /// </summary>
    private static HashSet<Type> ReadCovered(List<Type> declared)
    {
        var covered = new HashSet<Type>();
        var pending = new Stack<Type>(declared.Where(type => IsChecked(type) && ConvertedModels.TwinOf(type) is { } twin && Equivalence.Reads(twin)));
        while (pending.TryPop(out Type? type))
        {
            if (!IsChecked(type) || !covered.Add(type))
                continue;

            foreach (WireMember member in WireMembers.Of(type))
            {
                if (member.Shape == WireShape.Union)
                {
                    foreach (Type arm in Filler.ArmsOf(member).Where(arm => !ConvertedModels.Pairing.SkipsArm(arm)))
                        pending.Push(arm);
                }
                else if (member.Encoding == WireEncoding.Struct)
                {
                    pending.Push(member.ValueType);
                }
            }
        }

        return covered;
    }

    private static void AssertNone(List<string> problems)
    {
        if (problems.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, problems));
    }

    /// <summary>
    /// One line per type that differs, its first difference and how many it has, so a run shows every failing type.
    /// </summary>
    private static string Report(List<string> differences)
    {
        List<IGrouping<string, string>> types = [.. differences.GroupBy(difference => difference[..difference.IndexOf(' ', StringComparison.Ordinal)])];
        IEnumerable<string> lines = types.Take(MaxReportedLines).Select(type => $"{type.First()} ({type.Count()} in all)");
        if (types.Count > MaxReportedLines)
            lines = lines.Append($"... and {types.Count - MaxReportedLines} more types");
        return $"{types.Count} types differ:{Environment.NewLine}{string.Join(Environment.NewLine, lines)}";
    }
}
