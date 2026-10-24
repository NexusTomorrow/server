using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Spell;

namespace NexusTomorrow.Tools.Tests;

public sealed class LayoutRowsTests
{
    // Only fields: no string, counted array, union, blob or nested self (WireTree's text form).
    private static readonly Regex s_fixedTree = new(@"^(\d+( \d+)*)?$");

    [Fact]
    public void ACountedArrayIsItsOwnRowThenItsElementsRows()
    {
        Assert.Equal(
            [("Unknown0", "32", "unsigned"), ("Unknown1", "", "array, counted on the wire"), ("Unknown1[]", "32", "float")],
            LayoutRows.Of(typeof(Server0931)));
    }

    [Fact]
    public void AUnionIsItsOwnRowThenEachArmsRowsInCaseOrder()
    {
        List<(string Path, string Bits, string Encoding)> rows = LayoutRows.Of(typeof(CombatLog));

        Assert.Equal(("Type", "6", "unsigned"), rows[0]);
        Assert.Equal(("Payload", "", "union of ICombatLogPayload, selected by `Type`"), rows[1]);
        Assert.Equal(
            [("Payload{0}", "", "the fields below"), ("Payload{0}.Amount", "32", "unsigned"), ("Payload{0}.Action", "", "the fields below"),
             ("Payload{0}.Action.CasterId", "32", "unsigned")],
            rows[2..6]);
        List<string> arms = [.. rows.Where(row => row.Encoding == "the fields below" && !row.Path.Contains('.', StringComparison.Ordinal))
            .Select(row => row.Path)];
        Assert.Equal([.. arms.OrderBy(path => int.Parse(path[8..^1], CultureInfo.InvariantCulture))], arms);
    }

    [Fact]
    public void EveryServerMessageHasALayout()
    {
        int laidOut = 0;
        for (int opcode = 0; opcode <= ushort.MaxValue; opcode++)
        {
            if (OracleVerbs.CreateServerMessage((ushort)opcode) is not IWireMessage message)
                continue;

            _ = LayoutRows.Of(message.GetType());
            laidOut++;
        }

        Assert.True(laidOut > 100, $"only {laidOut} server messages were laid out");
    }

    [Fact]
    public void EveryFixedServerMessageListsTheGeneratorsWidthsInOrder()
    {
        var problems = new List<string>();
        int compared = 0;
        for (int opcode = 0; opcode <= ushort.MaxValue; opcode++)
        {
            if (OracleVerbs.CreateServerMessage((ushort)opcode) is not IWireMessage message)
                continue;

            Type type = message.GetType();
            string tree = type.GetCustomAttribute<WireLayoutDataAttribute>()!.Tree;
            if (!s_fixedTree.IsMatch(tree))
                continue;

            string rows = string.Join(" ", LayoutRows.Of(type).Where(row => row.Encoding != "the fields below").Select(Widths));
            if (rows != tree)
                problems.Add($"{type.Name}: the rows give \"{rows}\", the generator \"{tree}\"");
            compared++;
        }

        Assert.True(compared > 100, $"only {compared} fixed server messages were compared");
        Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems));
    }

    // The generator writes padding as fields of at most 64 bits; a row gives its padding as one width.
    private static string Widths((string Path, string Bits, string Encoding) row)
    {
        if (row.Encoding != "padding")
            return row.Bits;

        var chunks = new List<int>();
        for (int remaining = int.Parse(row.Bits, CultureInfo.InvariantCulture); remaining > 0; remaining -= 64)
            chunks.Add(Math.Min(64, remaining));
        return string.Join(" ", chunks);
    }
}
