using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSettlerBuildStatus, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerBuildStatus
{
    [UInt(14)] public ushort PathSettlerHubId { get; set; }
    public SettlerImprovementGroupStatus Status { get; set; }
}
