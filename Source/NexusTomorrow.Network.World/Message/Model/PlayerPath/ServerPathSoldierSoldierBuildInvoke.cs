using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoldierSoldierBuildInvoke)]
[WireMessage(GameMessageOpcode.ServerPathSoldierSoldierBuildInvoke, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierSoldierBuildInvoke : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> TowerDefenseIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(PathSoldierEventId, 14);
        writer.Write(TowerDefenseIds.Count);
        TowerDefenseIds.ForEach(id => writer.Write(id));
    }
}
