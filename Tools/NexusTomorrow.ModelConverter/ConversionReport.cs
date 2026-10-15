using System.Globalization;
using System.Text;

namespace NexusTomorrow.ModelConverter;

/// <summary>
/// A Markdown report of one conversion: the counts, then every model left legacy with the reason, grouped by reason.
/// </summary>
internal static class ConversionReport
{
    public static string Write(string project, ConversionResult result)
    {
        List<TypeOutcome> legacy = [.. result.Outcomes.Where(outcome => !outcome.Converted)];
        CultureInfo invariant = CultureInfo.InvariantCulture;
        var report = new StringBuilder();
        report.AppendLine(invariant, $"# Conversion of {Path.GetFileName(project)}");
        report.AppendLine();
        report.AppendLine(invariant, $"{result.Outcomes.Count} legacy models: {result.Outcomes.Count - legacy.Count} converted, {legacy.Count} left legacy.");
        report.AppendLine();
        report.AppendLine("## Left legacy, by reason");
        report.AppendLine();
        foreach (IGrouping<string, TypeOutcome> group in legacy.GroupBy(outcome => Kind(outcome.Reason ?? "")).OrderByDescending(group => group.Count()))
        {
            report.AppendLine(invariant, $"### {group.Key} ({group.Count()})");
            report.AppendLine();
            foreach (TypeOutcome outcome in group)
                report.AppendLine(invariant, $"- `{outcome.FullName}`: {outcome.Reason}");
            report.AppendLine();
        }

        report.AppendLine("## Converted");
        report.AppendLine();
        foreach (TypeOutcome outcome in result.Outcomes.Where(outcome => outcome.Converted))
            report.AppendLine(invariant, $"- `{outcome.FullName}`");
        return report.ToString();
    }

    /// <summary>
    /// The reason without the statement or names it quotes, so like failures group together.
    /// </summary>
    private static string Kind(string reason)
    {
        if (reason.StartsWith("Write:", StringComparison.Ordinal) || reason.StartsWith("Read:", StringComparison.Ordinal))
            return reason[..reason.IndexOf(':', StringComparison.Ordinal)] + " statement not matched";
        if (reason.StartsWith("it nests or derives from", StringComparison.Ordinal))
            return "nests or derives from a legacy type";
        if (reason.StartsWith("Read and Write disagree", StringComparison.Ordinal))
            return "Read and Write disagree";
        return reason;
    }
}
