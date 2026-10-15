using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoldierAddTowerDefense)]
[WireMessage(GameMessageOpcode.ServerPathSoldierAddTowerDefense, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierAddTowerDefense : IWritable
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(14)] public ushort PathSoldierTowerDefenseId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSoldierEventId, 14);
        writer.Write(PathSoldierTowerDefenseId, 14);
    }
}
