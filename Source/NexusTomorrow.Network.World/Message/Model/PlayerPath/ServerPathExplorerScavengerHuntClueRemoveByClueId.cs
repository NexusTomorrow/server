using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathExplorerScavengerHuntClueRemoveByClueId)]
[WireMessage(GameMessageOpcode.ServerPathExplorerScavengerHuntClueRemoveByClueId, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerScavengerHuntClueRemoveByClueId : IWritable
{
    [UInt(14)] public ushort PathExplorerScavengerClueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathExplorerScavengerClueId, 14);
    }
}
