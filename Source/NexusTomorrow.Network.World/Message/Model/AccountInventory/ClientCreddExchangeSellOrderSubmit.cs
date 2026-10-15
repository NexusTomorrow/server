using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientCreddExchangeSellOrderSubmit)]
[WireMessage(GameMessageOpcode.ClientCreddExchangeSellOrderSubmit, Direction.ClientToServer)]
public sealed partial class ClientCreddExchangeSellOrderSubmit : IReadable
{
    public Identity Unused { get; private set; } = new();
    [UInt(64)] public ulong Price { get; private set; }
    [UInt(1)] public bool Immediate { get; private set; } // exchange order = 0, purchase now = 1

    public void Read(GamePacketReader reader)
    {
        Unused.Read(reader);
        Price = reader.ReadULong();
        Immediate = reader.ReadBit();
    }
}
