using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathExplorerScavengerHuntClueRemoveByClueIdAndUnitId)]
public class ServerPathExplorerScavengerHuntClueRemoveByClueIdAndUnitId : IWritable
{
    public ushort PathExplorerScavengerClueId { get; set; }
    public uint UnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathExplorerScavengerClueId, 14);
        writer.Write(UnitId);
    }
}
