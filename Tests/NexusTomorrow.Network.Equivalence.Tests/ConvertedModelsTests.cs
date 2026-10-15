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

        foreach (Type type in declared)
        {
            string fullName = type.FullName ?? type.Name;
            if (ConvertedModels.Exclusions.ContainsKey(fullName))
                continue;

            Type? twin = ConvertedModels.TwinOf(type);
            if (twin is null)
            {
                untwinned.Add(fullName);
                continue;
            }

            differences.AddRange(Equivalence.Check(type, twin, ConvertedModels.TwinOf, legacyServices, Seeds));
            pairs++;
        }

        TestContext.Current.TestOutputHelper?.WriteLine($"Checked {pairs} of {declared.Count} declared types against their legacy twins; {ConvertedModels.Exclusions.Count} excluded.");
        if (untwinned.Count > 0)
            Assert.Fail($"These declared types have no legacy twin and no exclusion: {string.Join(", ", untwinned)}");
        if (differences.Count > 0)
            Assert.Fail(Report(differences));
        Assert.Equal(declared.Count, pairs + ConvertedModels.Exclusions.Count);
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
