using System.Text;

namespace NexusTomorrow.ProtocolReference;

/// <summary>
/// Writes a generated set of reference pages into a folder and owns the message pages there: a <c>Messages-*.md</c> page the set no
/// longer has (an area renamed or emptied) is never deleted but becomes a tombstone, because outside links still point at it. A page is
/// a tombstone when its text, read with LF line ends, ends with a blank line, the tombstone sentence and a newline. An existing tombstone is never rewritten, so
/// its verified date stays the date it was retired. <see cref="RetiredPages"/> names the pages that were once written, and a tombstone
/// is written for each that is missing. Every other file is left alone.
/// </summary>
public static class ReferenceSet
{
    private const string MessagePages = "Messages-*.md";

    private static readonly UTF8Encoding s_utf8 = new(encoderShouldEmitUTF8Identifier: false);

    /// <summary>
    /// The page names (without <c>.md</c>) that were once written and are no longer: each gets a tombstone when its file is missing and
    /// no area writes it. A message page found retired in a run belongs here, so the next run recreates its tombstone if it is lost.
    /// </summary>
    public static IReadOnlyList<string> RetiredPages { get; } = ["Messages-Unsorted"];

    /// <summary>
    /// Write <paramref name="pages"/> into <paramref name="output"/> as UTF-8 without a byte order mark. A message page there that is not
    /// among them and is not yet a tombstone becomes a tombstone stamped <paramref name="date"/>, and its name is returned in name order
    /// so a name not yet in <see cref="RetiredPages"/> can be added. A missing listed page also gets a tombstone, but is not returned.
    /// </summary>
    public static IReadOnlyList<string> Write(string output, IReadOnlyList<(string Name, string Text)> pages, string date)
    {
        Directory.CreateDirectory(output);
        var written = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach ((string name, string text) in pages)
        {
            File.WriteAllText(Path.Combine(output, name), text, s_utf8);
            written.Add(name);
        }

        string tombstone = PageWriter.TombstonePage(date);
        var retired = new List<string>();
        foreach (string path in Directory.EnumerateFiles(output, MessagePages).Order(StringComparer.Ordinal))
        {
            string name = Path.GetFileName(path);
            if (written.Contains(name) || IsTombstone(File.ReadAllText(path)))
                continue;
            File.WriteAllText(path, tombstone, s_utf8);
            retired.Add(name);
        }

        foreach (string listed in RetiredPages)
        {
            string name = listed + ".md";
            string path = Path.Combine(output, name);
            if (!written.Contains(name) && !File.Exists(path))
                File.WriteAllText(path, tombstone, s_utf8);
        }

        return retired;
    }

    private static bool IsTombstone(string text) => text.ReplaceLineEndings("\n").EndsWith("\n\n" + PageWriter.TombstoneSentence + "\n", StringComparison.Ordinal);
}
