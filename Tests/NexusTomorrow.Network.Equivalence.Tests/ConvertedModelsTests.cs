using Microsoft.Extensions.DependencyInjection;

namespace NexusTomorrow.Network.Equivalence.Tests;

public sealed class ConvertedModelsTests
{
    private const int Seeds = 32;
    private const int MaxReportedLines = 1200;

    [Fact]
    public void EveryDeclaredTypeWritesAndReadsLikeItsLegacyTwin()
    {
        List<Type> declared = ConvertedModels.DeclaredTypes();
        using ServiceProvider legacyServices = ConvertedModels.CreateLegacyServices();
        var untwinned = new List<string>();
        var differences = new List<string>();
        int pairs = 0;
        int holders = 0;

        foreach (Type type in declared)
        {
            string fullName = type.FullName ?? type.Name;
            if (ConvertedModels.Exclusions.ContainsKey(fullName))
                continue;

            Type? twin = ConvertedModels.TwinOf(type);
            if (twin is null)
            {
                // A type only the declaration has is checked through the twinned type that holds it: the mapper and comparer pair its members with
                // the legacy member's own type, with the bare arm where the seed keeps a union's arm itself, or with the holder's own members.
                if (IsHeldByATwinnedType(type, declared))
                    holders++;
                else
                    untwinned.Add(fullName);
                continue;
            }

            differences.AddRange(Equivalence.Check(type, twin, ConvertedModels.Pairing, legacyServices, Seeds));
            pairs++;
        }

        TestContext.Current.TestOutputHelper?.WriteLine(
            $"Checked {pairs} of {declared.Count} declared types against their legacy twins, {ConvertedModels.ReadOnly.Count} of them against the legacy Read only; "
            + $"{holders} types without a twin checked through the types holding them; {ConvertedModels.Exclusions.Count} excluded. "
            + $"Rules: {ConvertedModels.Rows.Count} rows, {ConvertedModels.Limits.Count} limits, {ConvertedModels.Presets.Count} presets, {ConvertedModels.Renames.Count} renames.");
        if (untwinned.Count > 0)
            Assert.Fail($"These declared types have no legacy twin and no exclusion: {string.Join(", ", untwinned)}");
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
            .Concat(ConvertedModels.Renames.Keys.Select(key => key.Type)))
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

        Assert.Empty(problems);
    }

    /// <summary>
    /// Whether a twinned, checked type holds <paramref name="type"/> as a member, an element, or an arm of one of its unions.
    /// </summary>
    private static bool IsHeldByATwinnedType(Type type, List<Type> declared)
    {
        return declared.Any(holder => ConvertedModels.TwinOf(holder) is not null
            && !ConvertedModels.Exclusions.ContainsKey(holder.FullName ?? holder.Name)
            && WireMembers.Of(holder).Any(member => member.ValueType == type || (member.Shape == WireShape.Union && member.ValueType.IsAssignableFrom(type))));
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
