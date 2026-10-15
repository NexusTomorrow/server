using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathExplorerScavengerHuntClueRemoveByClueIdAndUnitId)]
[WireMessage(GameMessageOpcode.ServerPathExplorerScavengerHuntClueRemoveByClueIdAndUnitId, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerScavengerHuntClueRemoveByClueIdAndUnitId : IWritable
{
    [UInt(14)] public ushort PathExplorerScavengerClueId { get; set; }
    [UInt(32)] public uint UnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathExplorerScavengerClueId, 14);
        writer.Write(UnitId);
    }
}
