using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSoldierHoldoutDeath, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierHoldoutDeath
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
}
