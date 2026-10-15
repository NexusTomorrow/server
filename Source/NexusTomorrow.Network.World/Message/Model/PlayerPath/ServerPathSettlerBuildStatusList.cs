using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSettlerBuildStatusList)]
[WireMessage(GameMessageOpcode.ServerPathSettlerBuildStatusList, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerBuildStatusList : IWritable
{
    [UInt(14)] public ushort PathSettlerHubId { get; set; }
    [Array(CountBits = 32)] public List<SettlerImprovementGroupStatus> ImprovementGroupStatuses { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSettlerHubId, 14);
        writer.Write(ImprovementGroupStatuses.Count);
        ImprovementGroupStatuses.ForEach(status => status.Write(writer));
    }
}
