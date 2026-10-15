using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientCreddExchangeBuyOrderSubmit)]
[WireMessage(GameMessageOpcode.ClientCreddExchangeBuyOrderSubmit, Direction.ClientToServer)]
public sealed partial class ClientCreddExchangeBuyOrderSubmit : IReadable
{
    [UInt(64)] public ulong Bid { get; private set; }
    [UInt(1)] public bool Immediate { get; private set; } // exchange order = 0, purchase now = 1

    public void Read(GamePacketReader reader)
    {
        Bid = reader.ReadULong();
        Immediate = reader.ReadBit();
    }
}
