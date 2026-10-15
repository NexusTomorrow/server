using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSettlerBuildStatusList, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerBuildStatusList
{
    [UInt(14)] public ushort PathSettlerHubId { get; set; }
    [Array(CountBits = 32)] public List<SettlerImprovementGroupStatus> ImprovementGroupStatuses { get; set; } = [];
}
