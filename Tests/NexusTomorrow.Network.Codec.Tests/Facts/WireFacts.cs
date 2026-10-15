using System.Text.Json;

namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// One computed part of a side of a facts row: its shape, and for an array the bits one element takes on the empty path.
/// </summary>
internal sealed record WireFactsPart(string Shape, int? ElementBits);

/// <summary>
/// One direction of a facts row: the client-to-server layout, or the server-to-client one. An unknown side has a kind and nothing else.
/// </summary>
internal sealed record WireFactsSide(string Kind, int? Bits, IReadOnlyList<int>? Widths, IReadOnlyList<WireFactsPart> Parts);

/// <summary>
/// One opcode's row of the client facts table.
/// </summary>
internal sealed record WireFactsRow(int Opcode, string Direction, WireFactsSide? ClientToServer, WireFactsSide? ServerToClient);

/// <summary>
/// Reads the client's layout facts for build 16042, one JSON object per wire opcode per line.
/// </summary>
internal static class WireFacts
{
    public static string TablePath => Path.Combine(AppContext.BaseDirectory, "Facts", "wire-facts-16042.jsonl");

    public static string MetaPath => Path.Combine(AppContext.BaseDirectory, "Facts", "wire-facts-16042.meta.json");

    public static Dictionary<int, WireFactsRow> Load(string path)
    {
        var rows = new Dictionary<int, WireFactsRow>();
        foreach (string line in File.ReadLines(path))
        {
            if (line.Length == 0)
                continue;

            using var document = JsonDocument.Parse(line);
            JsonElement root = document.RootElement;
            int opcode = root.GetProperty("op").GetInt32();
            rows.Add(opcode, new WireFactsRow(opcode, root.GetProperty("dir").GetString() ?? "", ReadSide(root, "c2s"), ReadSide(root, "s2c")));
        }

        return rows;
    }

    private static WireFactsSide? ReadSide(JsonElement root, string name)
    {
        if (!root.TryGetProperty(name, out JsonElement side))
            return null;

        int? bits = side.TryGetProperty("bits", out JsonElement bitsElement) ? bitsElement.GetInt32() : null;
        List<int>? widths = side.TryGetProperty("widths", out JsonElement widthsElement)
            ? [.. widthsElement.EnumerateArray().Select(width => width.GetInt32())]
            : null;
        List<WireFactsPart> parts = side.TryGetProperty("parts", out JsonElement partsElement)
            ? [.. partsElement.EnumerateArray().Select(ReadPart)]
            : [];
        return new WireFactsSide(side.GetProperty("kind").GetString() ?? "", bits, widths, parts);
    }

    private static WireFactsPart ReadPart(JsonElement part)
    {
        int? elementBits = part.TryGetProperty("elem_bits", out JsonElement bits) ? bits.GetInt32() : null;
        return new WireFactsPart(part.GetProperty("shape").GetString() ?? "", elementBits);
    }
}
