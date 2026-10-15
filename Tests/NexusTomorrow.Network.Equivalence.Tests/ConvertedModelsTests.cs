using Microsoft.Extensions.DependencyInjection;

namespace NexusTomorrow.Network.Equivalence.Tests;

public sealed class ConvertedModelsTests
{
    private const int Seeds = 32;
    private const int MaxReportedLines = 200;

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

    private static string Report(List<string> differences)
    {
        IEnumerable<string> lines = differences.Take(MaxReportedLines);
        if (differences.Count > MaxReportedLines)
            lines = lines.Append($"... and {differences.Count - MaxReportedLines} more");
        return string.Join(Environment.NewLine, lines);
    }
}
