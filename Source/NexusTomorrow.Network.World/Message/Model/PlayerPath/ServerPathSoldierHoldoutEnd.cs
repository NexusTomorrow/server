using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSoldierHoldoutEnd, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierHoldoutEnd
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(32)] public PlayerPathSoldierResult Reason { get; set; }
}
