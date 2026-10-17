namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// Compares a declared layout with the client's facts for its opcode, and lists every disagreement: against the client's tree, the same bits
/// on every path (<see cref="WireTreeComparison"/>), or by empty path for a side the facts give no tree.
/// </summary>
internal static class WireFactsComparison
{
    private static readonly Dictionary<string, WirePartShape> s_shapes = new()
    {
        ["wide-string"] = WirePartShape.WideString,
        ["narrow-string"] = WirePartShape.NarrowString,
        ["terminated-wide-string"] = WirePartShape.TerminatedWideString,
        ["array"] = WirePartShape.Array,
        ["union"] = WirePartShape.Union,
        ["blob"] = WirePartShape.Blob,
    };

    public static List<string> Compare(WireLayout layout, WireFactsRow? row)
    {
        string opcode = $"0x{layout.Opcode:X4}";
        var problems = new List<string>();
        if (row is null)
        {
            problems.Add($"{opcode}: the client facts have no row for this opcode");
            return problems;
        }

        if ((layout.Direction & Direction.ClientToServer) != 0)
            CompareSide(layout, row.ClientToServer, readerView: false, $"{opcode} client to server", problems);
        if ((layout.Direction & Direction.ServerToClient) != 0)
            CompareSide(layout, row.ServerToClient, readerView: true, $"{opcode} server to client", problems);
        return problems;
    }

    private static void CompareSide(WireLayout layout, WireFactsSide? side, bool readerView, string label, List<string> problems)
    {
        if (side is null)
        {
            problems.Add($"{label}: the client does not handle the message in this direction");
            return;
        }

        if (side.Kind == "unknown")
            return;

        if (side.Tree is not null)
        {
            problems.AddRange(WireTreeComparison.Compare(label, layout.Tree, WireTree.Parse(side.Tree)));
            return;
        }

        // A union without arm 0 leaves the empty path incomplete; the tree comparison names the arm itself.
        if (!layout.IsComplete)
            problems.Add($"{label}: a union has no arm for selector 0, so the declaration's empty path is incomplete");

        List<int> widths = readerView ? ReaderWidths(layout) : [.. layout.EmptyPathWidths.Select(width => (int)width)];
        bool clientFixed = side.Kind == "fixed";
        if (clientFixed != layout.IsFixed)
            problems.Add($"{label}: the client's layout is {side.Kind}, the declaration's is {(layout.IsFixed ? "fixed" : "computed")}");

        if (side.Widths is not null)
        {
            if (!side.Widths.SequenceEqual(widths))
                problems.Add($"{label}: the client's widths are [{string.Join(", ", side.Widths)}], the declaration's [{string.Join(", ", widths)}]");
        }
        else if (side.Bits is { } bits && bits != widths.Sum())
        {
            problems.Add($"{label}: the client's empty path is {bits} bits, the declaration's {widths.Sum()}");
        }

        List<WirePart> unmatched = [.. layout.Parts];
        foreach (WireFactsPart part in side.Parts)
        {
            if (!s_shapes.TryGetValue(part.Shape, out WirePartShape shape))
            {
                problems.Add($"{label}: the client facts name a part shape the codec does not know: {part.Shape}");
                continue;
            }

            int index = unmatched.FindIndex(candidate => candidate.Shape == shape && (part.ElementBits is null || candidate.ElementBits == part.ElementBits));
            if (index < 0)
                problems.Add($"{label}: the client has a {part.Shape}{(part.ElementBits is { } elementBits ? $" of {elementBits}-bit elements" : "")} that the declaration lacks");
            else
                unmatched.RemoveAt(index);
        }
    }

    /// <summary>
    /// The widths the client's reader takes on the empty path: it reads an empty terminated string as its count alone, where the writer also emits the terminator.
    /// </summary>
    private static List<int> ReaderWidths(WireLayout layout)
    {
        List<int> widths = [.. layout.EmptyPathWidths.Select(width => (int)width)];
        foreach (WirePart part in layout.Parts.Where(part => part.Shape == WirePartShape.TerminatedWideString).OrderByDescending(part => part.At))
            widths.RemoveAt(part.At + 1);
        return widths;
    }
}
