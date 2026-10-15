using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountEntitlement, Direction.ServerToClient)]
public sealed partial class ServerAccountEntitlement
{
    [UInt(32)] public EntitlementType EntitlementId { get; set; }
    [UInt(32)] public uint Count { get; set; }
}
