using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// Holds a message at <see cref="Rung.Fields"/> or above to taking each field the client reads as one field whole. The tree comparison
/// accepts a run of declared fields against one client field (8 6 against 14) as the same bits, but each member then holds part of one
/// client value, so such a split fails here unless it is listed in <see cref="Settled"/>, keyed by the label and path of the node it starts
/// at, with its reason and the split's text. One declared field against a run of client fields (32 against 8 8 8 8) takes the client's
/// values together and is not checked.
/// </summary>
internal static class FieldsSplits
{
    public static IReadOnlyDictionary<string, Pin> Settled { get; } = new Dictionary<string, Pin>(StringComparer.Ordinal);

    /// <summary>
    /// Every split at Fields that is not settled, every settled split whose text has changed, and every settled split that no longer occurs.
    /// </summary>
    public static List<string> Check(IEnumerable<(WireLayout Layout, Rung? Rung)> messages, IReadOnlyDictionary<int, WireFactsRow> rows, IReadOnlyDictionary<string, Pin> settled)
    {
        var found = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach ((WireLayout layout, Rung? rung) in messages)
        {
            if (rung is null or < Rung.Fields)
                continue;

            var splits = new List<WireSplit>();
            WireFactsComparison.Compare(layout, rows.GetValueOrDefault(layout.Opcode), splits);
            foreach (WireSplit split in splits.Where(split => split.Declared.Length > 1))
                found[split.Key] = $"the client reads {split.Client[0]} as one field, the declaration as {string.Join(' ', split.Declared)}";
        }

        var report = new List<string>();
        foreach ((string key, string text) in found)
        {
            if (!settled.TryGetValue(key, out Pin pin))
                report.Add($"split at Fields, not settled: {key}: {text}");
            else if (pin.Problem != text)
                report.Add($"settled, but the split changed: {key}: {text}");
        }

        report.AddRange(settled.Where(entry => !found.ContainsKey(entry.Key)).Select(entry => $"settled, but it is no longer split: {entry.Key} ({entry.Value.Reason})"));
        return report;
    }
}
