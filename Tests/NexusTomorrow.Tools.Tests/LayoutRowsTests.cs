using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Entity;

namespace NexusTomorrow.Tools.Tests;

public sealed class LayoutRowsTests
{
    [Fact]
    public void ACountedArrayIsItsOwnRowThenItsElementsRows()
    {
        Assert.Equal(
            [("Unknown0", "32", "unsigned"), ("Unknown1", "", "array, counted on the wire"), ("Unknown1[]", "32", "float")],
            LayoutRows.Of(typeof(Server0931)));
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
}
