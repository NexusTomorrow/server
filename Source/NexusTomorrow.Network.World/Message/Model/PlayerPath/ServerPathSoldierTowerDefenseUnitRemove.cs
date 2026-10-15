using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSoldierTowerDefenseUnitRemove, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierTowerDefenseUnitRemove
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public TowerDefenseUnitType Type { get; set; }
}
