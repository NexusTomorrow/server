using NexusTomorrow.Game.Static.Map;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerPendingWorldRemoval, Direction.ServerToClient)]
public sealed partial class ServerPendingWorldRemoval
{
    [UInt(3)] public WorldRemovalReason Reason { get; set; }
}
