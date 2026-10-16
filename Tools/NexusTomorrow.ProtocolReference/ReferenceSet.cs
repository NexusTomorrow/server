using System.Text;

namespace NexusTomorrow.ProtocolReference;

/// <summary>
/// Writes a generated set of reference pages into a folder and owns the message pages there: a <c>Messages-*.md</c> page the set no
/// longer has (an area renamed or emptied) is removed, and every other file is left alone.
/// </summary>
public static class ReferenceSet
{
    private const string MessagePages = "Messages-*.md";

    private static readonly UTF8Encoding s_utf8 = new(encoderShouldEmitUTF8Identifier: false);

    /// <summary>
    /// Write <paramref name="pages"/> into <paramref name="output"/> as UTF-8 without a byte order mark, and return the names of the
    /// message pages removed, in name order.
    /// </summary>
    public static IReadOnlyList<string> Write(string output, IReadOnlyList<(string Name, string Text)> pages)
    {
        Directory.CreateDirectory(output);
        var written = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach ((string name, string text) in pages)
        {
            File.WriteAllText(Path.Combine(output, name), text, s_utf8);
            written.Add(name);
        }

        var removed = new List<string>();
        foreach (string path in Directory.EnumerateFiles(output, MessagePages).Order(StringComparer.Ordinal))
        {
            string name = Path.GetFileName(path);
            if (written.Contains(name))
                continue;
            File.Delete(path);
            removed.Add(name);
        }

        return removed;
    }
}
