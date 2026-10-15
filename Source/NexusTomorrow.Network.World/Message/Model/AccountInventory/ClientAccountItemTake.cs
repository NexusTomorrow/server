using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.ClientAccountItemTake, Direction.ClientToServer)]
public sealed partial class ClientAccountItemTake
{
    [UInt(64)] public ulong UserInventoryId { get; private set; }
}
