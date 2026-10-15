using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathExplorerScavengerHuntClueAddWorldLocation)]
[WireMessage(GameMessageOpcode.ServerPathExplorerScavengerHuntClueAddWorldLocation, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerScavengerHuntClueAddWorldLocation : IWritable
{
    [UInt(17)] public uint WorldLocation2Id { get; set; }
    [UInt(14)] public ushort PathExplorerScavengerClueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(WorldLocation2Id, 17);
        writer.Write(PathExplorerScavengerClueId, 14);
    }
}
