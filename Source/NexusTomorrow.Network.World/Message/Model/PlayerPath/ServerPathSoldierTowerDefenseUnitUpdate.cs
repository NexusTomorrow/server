using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoldierTowerDefenseUnitUpdate)]
[WireMessage(GameMessageOpcode.ServerPathSoldierTowerDefenseUnitUpdate, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierTowerDefenseUnitUpdate : IWritable
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public TowerDefenseUnitType Type { get; set; }
    [Float32] public float MaxHealth { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSoldierEventId, 14);
        writer.Write(UnitId);
        writer.Write(Type, 32u);
        writer.Write(MaxHealth);
    }
}
