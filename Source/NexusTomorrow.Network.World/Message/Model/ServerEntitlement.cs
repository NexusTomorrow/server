using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntitlement, Direction.ServerToClient)]
public sealed partial class ServerEntitlement
{
    [UInt(14)] public EntitlementType Entitlement { get; set; }
    [UInt(32)] public uint Count { get; set; }
}
