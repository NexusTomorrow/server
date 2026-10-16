using System.Globalization;
using System.Text;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.ProtocolReference;

/// <summary>
/// The commit a set of pages was generated at, for the badges and the note.
/// </summary>
public sealed record PageStamp(string Branch, string Commit, string Date);

/// <summary>
/// Renders the reference pages in the wiki's shape: CRLF text, the badge row first, then the scope sentence and the generated-page note.
/// A message's summary and remarks appear from <see cref="Rung.Purpose"/>, its fields' meanings from <see cref="Rung.Fields"/>.
/// </summary>
public static class PageWriter
{
    private const string Generator = "Tools/NexusTomorrow.ProtocolReference/";
    private const string Repository = "https://github.com/NexusTomorrow/server";

    private const string Grouping =
        "This grouping comes from where the declarations sit today. It claims nothing about what these messages do, and it changes when the client's areas are settled.";

    private static readonly (Direction Direction, string Title)[] s_chapters =
    [
        (Direction.ServerToClient, "Server to client"),
        (Direction.ClientToServer, "Client to server"),
        (Direction.Both, "Both ways"),
    ];

    /// <summary>
    /// The page for one area.
    /// </summary>
    public static string AreaPage(string area, IReadOnlyList<ReferenceMessage> messages, LayoutWalk walk, DocText docs, PageStamp stamp)
    {
        var blocks = new List<string>();
        Head(blocks, $"Every message in the {area} area: its opcode, direction, how far it is verified, and its fields.", stamp);

        // An area name is the folder the declarations sit in, a grouping and not a claim, until a message in it is read to Purpose.
        if (!messages.Any(message => message.Rung >= Rung.Purpose))
            blocks.Insert(2, Grouping);

        foreach ((Direction direction, string title) in s_chapters)
        {
            List<ReferenceMessage> chapter = [.. messages.Where(message => message.Direction == direction).OrderBy(message => message.Opcode).ThenBy(message => message.Type.Name, StringComparer.Ordinal)];
            if (chapter.Count == 0)
                continue;

            blocks.Add($"## {title}");
            foreach (ReferenceMessage message in chapter)
                Section(blocks, message, walk, docs);
        }

        IEnumerable<string[]> folders = messages
            .Select(message => DeclarationFolder(message.Type))
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .Select(folder => new[] { "Message declarations", Code(folder) });
        CodeLives(blocks, [.. folders, ["This page's generator", Code(Generator)]]);
        return Join(blocks);
    }

    /// <summary>
    /// The opcode index over every message, with the areas and how much of each is read to <see cref="Rung.Fields"/>.
    /// </summary>
    public static string IndexPage(IReadOnlyList<ReferenceMessage> messages, PageStamp stamp)
    {
        var blocks = new List<string>();
        Head(blocks, "Every client message by opcode, with its direction, how far it is verified, and the page that describes it.", stamp);

        blocks.Add("## Messages by opcode");
        var index = new List<string[]>();
        foreach (ReferenceMessage message in messages.OrderBy(message => message.Opcode).ThenBy(message => message.Type.Name, StringComparer.Ordinal))
        {
            index.Add(
            [
                Code(Hex(message.Opcode)),
                $"[{message.Type.Name}](Messages-{message.Area}#{Anchor(message)})",
                DirectionText(message.Direction),
                RungText(message.Rung),
                $"[{message.Area}](Messages-{message.Area})",
            ]);
        }

        blocks.Add(Table(["Opcode", "Message", "Direction", "Verified to", "Area"], index));

        blocks.Add("## Areas");
        IEnumerable<string[]> areas = messages
            .GroupBy(message => message.Area, StringComparer.Ordinal)
            .OrderBy(group => group.Key, StringComparer.Ordinal)
            .Select(group => new[] { $"[{group.Key}](Messages-{group.Key})", Number(group.Count()), Number(group.Count(message => message.Rung >= Rung.Fields)) });
        blocks.Add(Table(["Area", "Messages", "At fields or above"], [.. areas]));

        CodeLives(blocks,
        [
            ["Network messages", Code("Source/NexusTomorrow.Network/Message/Model/")],
            ["Login messages", Code("Source/NexusTomorrow.Network.Auth/Message/Model/")],
            ["World messages", Code("Source/NexusTomorrow.Network.World/Message/Model/")],
            ["The verification rungs", Code("Source/NexusTomorrow.Network/Message/VerifiedAttribute.cs")],
            ["This page's generator", Code(Generator)],
        ]);
        return Join(blocks);
    }

    private static void Head(List<string> blocks, string scope, PageStamp stamp)
    {
        blocks.Add(
            "![reference, committed](https://img.shields.io/badge/reference-committed-2ea44f) "
            + $"![verified {stamp.Date}](https://img.shields.io/badge/verified-{Shield(stamp.Date)}-6e7781) "
            + $"[![{stamp.Branch} {stamp.Commit}](https://img.shields.io/badge/{Shield(stamp.Branch)}-{Shield(stamp.Commit)}-6e7781)]({Repository}/commit/{stamp.Commit})");
        blocks.Add(scope);
        blocks.Add(
            "> [!NOTE]\r\n"
            + $"> Generated from the message declarations at {stamp.Branch} `{stamp.Commit}` by `{Generator.TrimEnd('/')}`. Don't edit it by hand: change the declaration's documentation and regenerate.");
    }

    private static void Section(List<string> blocks, ReferenceMessage message, LayoutWalk walk, DocText docs)
    {
        string typeId = DocumentationIds.Of(message.Type);
        bool purpose = message.Rung >= Rung.Purpose;
        bool fields = message.Rung >= Rung.Fields;

        blocks.Add($"### {Code(Hex(message.Opcode))} {message.Type.Name}");
        if (purpose && docs.Summary(typeId) is { } summary)
            blocks.Add(summary);

        blocks.Add(Table(["Opcode", "Direction", "Verified to"], [[Code(Hex(message.Opcode)), DirectionText(message.Direction), RungText(message.Rung)]]));

        string[] header = fields ? ["Field", "Bits", "Encoding", "Meaning"] : ["Field", "Bits", "Encoding"];
        var rows = new List<string[]>();
        foreach (LayoutRow row in walk.Walk(message.Type).Rows)
        {
            rows.Add(fields
                ? [Code(row.Path), row.Bits, row.Encoding, docs.Summary(row.DocumentationId) ?? ""]
                : [Code(row.Path), row.Bits, row.Encoding]);
        }

        if (rows.Count == 0)
            rows.Add(fields ? ["(none)", "0", "", ""] : ["(none)", "0", ""]);
        blocks.Add(Table(header, rows));

        if (purpose && docs.Remarks(typeId) is { } remarks)
            blocks.Add(remarks);
    }

    private static void CodeLives(List<string> blocks, IReadOnlyList<string[]> rows)
    {
        blocks.Add("## Where the code lives");
        blocks.Add(Table(["Piece", "Path"], rows));
    }

    // A model project's folder for the message: Source/ for the model projects, Tests/ for a test project's fixtures, then the namespace
    // below the assembly's own.
    private static string DeclarationFolder(Type type)
    {
        string assembly = type.Assembly.GetName().Name ?? "";
        string root = assembly.EndsWith(".Tests", StringComparison.Ordinal) ? "Tests" : "Source";
        string space = type.Namespace ?? "";
        string below = space.StartsWith(assembly, StringComparison.Ordinal) ? space[assembly.Length..].TrimStart('.') : space;
        return $"{root}/{assembly}/{below.Replace('.', '/')}/";
    }

    private static string Table(string[] header, IReadOnlyList<string[]> rows)
    {
        var text = new StringBuilder();
        text.Append(Row(header)).Append("\r\n");
        text.Append('|').Append(string.Concat(header.Select(_ => "---|"))).Append("\r\n");
        for (int i = 0; i < rows.Count; i++)
        {
            text.Append(Row(rows[i]));
            if (i < rows.Count - 1)
                text.Append("\r\n");
        }

        return text.ToString();
    }

    // An empty cell is written as "| |", the way the wiki's hand-written tables are.
    private static string Row(IEnumerable<string> cells) =>
        "|" + string.Concat(cells.Select(cell => cell.Length == 0 ? " |" : $" {cell.Replace("|", "\\|", StringComparison.Ordinal)} |"));

    private static string Join(List<string> blocks) => string.Join("\r\n\r\n", blocks) + "\r\n";

    // The anchor GitHub makes from a "### `0x0036` Server0036" heading.
    private static string Anchor(ReferenceMessage message) =>
        $"{Hex(message.Opcode)}-{message.Type.Name}".ToLowerInvariant();

    private static string Hex(ushort opcode) => "0x" + opcode.ToString("X4", CultureInfo.InvariantCulture);

    private static string Code(string text) => $"`{text}`";

    private static string Number(int value) => value.ToString(CultureInfo.InvariantCulture);

    // Shields.io reads "-" as a separator, so a literal dash is doubled, as is an underscore; a space is an underscore; a slash is escaped.
    private static string Shield(string text) =>
        text.Replace("-", "--", StringComparison.Ordinal).Replace("_", "__", StringComparison.Ordinal).Replace(" ", "_", StringComparison.Ordinal).Replace("/", "%2F", StringComparison.Ordinal);

    private static string DirectionText(Direction direction) => direction switch
    {
        Direction.ClientToServer => "client to server",
        Direction.ServerToClient => "server to client",
        _ => "both ways",
    };

    private static string RungText(Rung? rung) => rung switch
    {
        Rung.Purpose => "purpose",
        Rung.Fields => "fields",
        Rung.Conversation => "conversation",
        _ => "unverified",
    };
}
