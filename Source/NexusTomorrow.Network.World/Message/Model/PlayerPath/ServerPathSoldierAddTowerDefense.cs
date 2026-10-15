using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSoldierAddTowerDefense, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierAddTowerDefense
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(14)] public ushort PathSoldierTowerDefenseId { get; set; }
}
