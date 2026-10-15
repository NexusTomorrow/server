using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathExplorerScavengerHuntClueAddWorldLocation)]
public class ServerPathExplorerScavengerHuntClueAddWorldLocation : IWritable
{
    public uint WorldLocation2Id { get; set; }
    public ushort PathExplorerScavengerClueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(WorldLocation2Id, 17);
        writer.Write(PathExplorerScavengerClueId, 14);
    }
}
