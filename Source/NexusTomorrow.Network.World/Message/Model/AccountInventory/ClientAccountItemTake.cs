using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientAccountItemTake)]
[WireMessage(GameMessageOpcode.ClientAccountItemTake, Direction.ClientToServer)]
public sealed partial class ClientAccountItemTake : IReadable
{
    [UInt(64)] public ulong UserInventoryId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UserInventoryId = reader.ReadULong();
    }
}
