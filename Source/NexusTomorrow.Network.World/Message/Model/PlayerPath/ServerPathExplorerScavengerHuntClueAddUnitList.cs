using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathExplorerScavengerHuntClueAddUnitList)]
[WireMessage(GameMessageOpcode.ServerPathExplorerScavengerHuntClueAddUnitList, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerScavengerHuntClueAddUnitList : IWritable
{
    [UInt(14)] public ushort PathExplorerScavengerClueId { get; set; }
    [Array(CountBits = 32), UInt(32)] public uint[] UnitIdList { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathExplorerScavengerClueId, 14);
        for (uint i = 0u; i < UnitIdList.Length; i++)
            writer.Write(UnitIdList[i]);
    }
}
