using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountItemRemove, Direction.ServerToClient)]
public sealed partial class ServerAccountItemRemove
{
    [UInt(64)] public ulong UserInventoryId { get; set; }
}
