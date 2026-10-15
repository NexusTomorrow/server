using NexusTomorrow.GameTable;

namespace NexusTomorrow.GameTable.Text.Search;

public class TextReverseIndex : ITextReverseIndex
{
    private readonly Dictionary<string, List<uint>> _index;
    public bool IsEmpty => !_index.Keys.Any();

    public TextReverseIndex(TextTable? textTable)
    {
        _index = new Dictionary<string, List<uint>>(StringComparer.OrdinalIgnoreCase);
        if (textTable == null)
            return;

        foreach (TextTableEntry entry in textTable.Entries)
            AddEntry(_index, entry);
    }

    private void AddEntry(Dictionary<string, List<uint>> dictionary, TextTableEntry entry)
    {
        if (!dictionary.TryGetValue(entry.Text, out List<uint>? list) || list == null)
            dictionary[entry.Text] = list = [];
        list.Add(entry.Id);
    }

    public IEnumerable<uint>? ExactSearch(string text)
    {
        if (IsEmpty)
            return null;
        if (!_index.TryGetValue(text, out List<uint>? values))
            return [];
        return values ?? Enumerable.Empty<uint>();
    }

    public IEnumerable<uint> FuzzySearch(string text)
    {
        return [.. _index.Keys.Where(i => i.Contains(text, StringComparison.OrdinalIgnoreCase)).SelectMany(i => _index[i]).Distinct()];
    }
}
