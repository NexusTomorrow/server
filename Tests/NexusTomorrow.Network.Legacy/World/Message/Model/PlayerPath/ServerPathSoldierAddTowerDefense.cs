using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoldierAddTowerDefense)]
public class ServerPathSoldierAddTowerDefense : IWritable
{
    public ushort PathSoldierEventId { get; set; }
    public ushort PathSoldierTowerDefenseId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSoldierEventId, 14);
        writer.Write(PathSoldierTowerDefenseId, 14);
    }
}
